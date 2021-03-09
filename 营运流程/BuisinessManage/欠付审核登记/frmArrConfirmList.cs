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

namespace ZQTMS.UI
{
    public partial class frmArrConfirmList : BaseForm
    {
        public frmArrConfirmList()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("账款收银确认登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView9);
            GridOper.SetGridViewProperty(myGridView9);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView9);
            FixColumn fix = new FixColumn(myGridView9, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView9, "ArrConfirmState", FormatConditionEnum.Equal, "已确认", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView9, "ArrConfirmState", FormatConditionEnum.Equal, "部分确认", Color.FromArgb(255, 255, 128));

            bdate.DateTime =    CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetCause(Cuase, true);
            Cuase.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            web.EditValue = CommonClass.UserInfo.WebName;
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
            TimeSpan ts1 = new TimeSpan(bdate.DateTime.Ticks);
            TimeSpan ts2 = new TimeSpan( edate.DateTime.Ticks);

            TimeSpan ts = ts1.Subtract(ts2).Duration();
            if(ts.Days > 31)
            {
                MsgBox.ShowOK("只能查询一个月的账务！请选择正确时间！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("cuasename", Cuase.Text.Trim()));
                list.Add(new SqlPara("areaname", Area.Text.Trim()));
                list.Add(new SqlPara("webname", web.Text.Trim()));
                list.Add(new SqlPara("ArrConfirmState", ArrConfirmState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_Get_WayBill_ArrConfirm]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl8.DataSource = ds.Tables[0];

                if (ds == null || ds.Tables.Count == 0) return;
                DataRow[] dr1;
                for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                {
                    string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]);

                    dr1 = ds.Tables[0].Select("BillNo='" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"])+"'");
                    if (dr1.Length > 1)
                    {
                        for (int j = 1; j < dr1.Length; j++)
                        {
                            ds.Tables[0].Rows.Remove(dr1[j]);
                            //if (ds.Tables[0].Columns.Contains("FetchPay")) dr[j]["FetchPay"] = DBNull.Value;
                            //if (ds.Tables[0].Columns.Contains("NowPay")) dr[j]["NowPay"] = DBNull.Value;
                            //if (ds.Tables[0].Columns.Contains("ShortOwePay")) dr[j]["ShortOwePay"] = DBNull.Value;
                            //if (ds.Tables[0].Columns.Contains("MonthPay")) dr[j]["MonthPay"] = DBNull.Value;
                        }
                    }
                }  
                 DataTable dt = myGridControl8.DataSource as DataTable;
               
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
            frmArrConfirmDetail ww = new frmArrConfirmDetail();
            ww.Show();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView9.FocusedRowHandle;
            if (rows < 0) return;
            if (myGridView9.GetRowCellValue(rows, "ArrConfirmID") == null)
            {
                return;
            }
            if (ConvertType.ToString(myGridView9.GetRowCellValue(rows, "ArrConfirmAduitMan")) != "")
            {
                MsgBox.ShowError("已审核的不能取消！");
                return;
            }
            if (MsgBox.ShowYesNo("确定取消确认?") != DialogResult.Yes) return;
            try
            {
                string BillNo = Convert.ToString(myGridView9.GetRowCellValue(rows, "ArrConfirmID"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("ArrComfirmCancelMan", CommonClass.UserInfo.UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[USP_Cancel_ArrConfirm]", list);
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

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmArrConfirmCancelList frm = new frmArrConfirmCancelList();
            frm.Show();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmArrConfirmCancelDetail frm = new frmArrConfirmCancelDetail();
            frm.Show();
        }

    }
}