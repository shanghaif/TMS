using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfmArrivedMonitor : BaseForm
    {
        public JMfmArrivedMonitor()
        {
            InitializeComponent();
        }

        private void fmArrivedMonitor_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            CommonClass.SetSite(edsite, true);
            edsite.EditValue = CommonClass.UserInfo.SiteName;
            edwebid.EditValue = CommonClass.UserInfo.WebName;

            bedate.DateTime = CommonClass.gbdate;
            enddate.DateTime = CommonClass.gedate;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "到货通知记录");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("selecttype", radioGroup1.SelectedIndex));
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRIVED_MONITOR", list);
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
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bedate", bedate.DateTime));
                list.Add(new SqlPara("enddate", enddate.DateTime));
                list.Add(new SqlPara("edsite", edsite.Text.Trim() == "全部" ? "%%" : edsite.Text.Trim()));
                list.Add(new SqlPara("edweb", edwebid.Text.Trim() == "全部" ? "%%" : edwebid.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_B_SMS_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void edsite_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(edwebid, edsite.EditValue.ToString());
        }
    }
}