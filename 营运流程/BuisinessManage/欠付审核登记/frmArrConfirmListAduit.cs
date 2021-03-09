using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using System.Reflection;
using DevExpress.XtraGrid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmArrConfirmListAduit : BaseForm
    {
        public frmArrConfirmListAduit()
        {
            InitializeComponent();
        }
        GridColumn gcIsseleckedMode;
        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("账款收银确认审核");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView9);
            GridOper.SetGridViewProperty(myGridView9);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView9);
            FixColumn fix = new FixColumn(myGridView9, barSubItem2);

            bdate.DateTime = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString() + " 00:00:00");
            edate.DateTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

            CommonClass.SetCause(Cuase, true);
            Cuase.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            web.EditValue = CommonClass.UserInfo.WebName;
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView9, "ischecked");
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("cuasename", Cuase.Text.Trim()));
                list.Add(new SqlPara("areaname", Area.Text.Trim()));
                list.Add(new SqlPara("webname", web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_Get_WayBill_ArrConfirm_Aduit]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl8.DataSource = ds.Tables[0];
                DataTable dt = myGridControl8.DataSource as DataTable;
                if (dt.Rows.Count == 1) return;

                DataRow[] dr;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Select("BillNo='" + ConvertType.ToString(dt.Rows[i]["BillNo"]+"'"));
                    if (dr.Length > 1)
                    {
                        for (int j = 1; j < dr.Length; j++)
                        {
                            if (dt.Columns.Contains("ArrComfirmBalance")) dr[j]["ArrComfirmBalance"] = DBNull.Value;
                        }
                    }
                    Application.DoEvents();
                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.ToString());
            }
            finally
            {
                if (myGridView9.RowCount < 1000) myGridView9.BestFitColumns();
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView9.RowCount == 0) return;
            try
            {
                myGridView9.PostEditor();
                myGridView9.UpdateCurrentRow();
                string unitstr = "";
                string sumAmount = ""; 
                for (int i = 0; i < myGridView9.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView9.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        unitstr += ConvertType.ToString(myGridView9.GetRowCellValue(i, "ArrConfirmID")) + "@";
                        sumAmount = ConvertType.ToString(ConvertType.ToDecimal(myGridView9.GetRowCellValue(i, "ArrConfirmFee")) + ConvertType.ToDecimal(sumAmount));
                    }
                }
                if (unitstr == "") return;
                string sShowOK = "欠付审核总票数：" + ConvertType.ToString(myGridView9.RowCount)
                    + "\r\n欠付审核总金额：" + ConvertType.ToString(sumAmount) + "\r\n欠付审核人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";
                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", unitstr));
                list.Add(new SqlPara("ArrConfirmAduitCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ArrConfirmAduitArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("ArrConfirmAduitSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ArrConfirmAduitWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ArrConfirmAduitMan", CommonClass.UserInfo.UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ArrConfirm_Aduit", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView9.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView9.GetRowCellValue(rows, "ArrConfirmAduitMan").ToString() == "")
            {
                MsgBox.ShowOK("该单未审核！请重新选择");
                return;
            }
            if (myGridView9.GetRowCellValue(rows, "ArrComfirmBalance").ToString() == "")
            {
                MsgBox.ShowOK("该单金额已经全部审核！请重新选择");
                return;
            }
            if (myGridView9.GetRowCellValue(rows, "ArrComBedState").ToString() == "1"
                || myGridView9.GetRowCellValue(rows, "ArrComBedState").ToString() == "2")
            {
                MsgBox.ShowOK("该单金额已经进入坏账！请重新选择");
                return;
            }
            if (MsgBox.ShowYesNo("是否确认转入异常？") != DialogResult.Yes) return;
            try
            {
                string BillNo = Convert.ToString(myGridView9.GetRowCellValue(rows, "BillNo"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[USP_ADD_ArrConfirm_Exception]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                cbRetrieve_Click(null, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void Cuase_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cuase.Text, true);
            CommonClass.SetCauseWeb(web, Cuase.Text, Area.Text);
        }

        private void Area_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cuase.Text.Trim(), Area.Text.Trim());
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView9);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView9.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView9);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView9);
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView9.RowCount; i++)
            {
                myGridView9.SetRowCellValue(i,gcIsseleckedMode, a);
            }
        }
    }
}