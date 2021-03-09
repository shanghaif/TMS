using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmVerifiAccount : BaseForm
    {
        public frmVerifiAccount()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            //setweb();

            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            Web.Text = CommonClass.UserInfo.WebName;

        }
     

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_1", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            } 
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_Count", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                decimal a = ds.Tables[0].Rows[0][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                decimal b = ds.Tables[0].Rows[1][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[1][0]);
                edaccyestoday.Text = Math.Round(a, 2).ToString();

                edacctoday.Text = Math.Round(b, 2).ToString();

                decimal accyestoday = 0;
                if (edaccyestoday.Text.Trim() != "")
                {
                    accyestoday = Convert.ToDecimal(edaccyestoday.Text.Trim());
                }

                decimal acctoday = 0;
                if (edacctoday.Text.Trim() != "")
                {
                    acctoday = Convert.ToDecimal(edacctoday.Text.Trim());
                }
                edaccnow.Text = Math.Round(accyestoday + acctoday, 2).ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "核销日记账汇总");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rows = myGridView1.FocusedRowHandle;
                if (rows < 0) return;
                string VoucherNo = myGridView1.GetRowCellValue(rows, "VoucherNo").ToString();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", VoucherNo));
                list.Add(new SqlPara("Web", myGridView1.GetRowCellValue(rows, "WebName").ToString()));
                if (VoucherNo == "") 
                {
                    list.Add(new SqlPara("t1", bdate.DateTime));
                    list.Add(new SqlPara("t2", edate.DateTime));
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCTOTAL_VerifyOffAccount", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];

                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(Web, CauseName.Text, AreaName.Text);
        }
        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(Web, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "核销日记账汇总明细");
        }
    }
}