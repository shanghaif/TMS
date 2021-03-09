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
    public partial class frmArrConfirmCancelList : BaseForm
    {
        public frmArrConfirmCancelList()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
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

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "[QSP_Get_WayBill_ArrConfirm_CancelList]", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl8.DataSource = ds.Tables[0];


                 DataTable dt = myGridControl8.DataSource as DataTable;
                if (dt.Rows.Count == 1) return;

                DataRow[] dr;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    dr = dt.Select("BillNo=" + ConvertType.ToString(dt.Rows[i]["BillNo"]));
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
            frmArrConfirmDetail ww = new frmArrConfirmDetail();
            ww.Show();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView9.FocusedRowHandle;
            if (rows < 0) return;

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
    }
}