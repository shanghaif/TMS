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
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class frmPreArrived : BaseForm
    {
        public frmPreArrived()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            string proc = "QSP_GET_BILL_PREARRIVED";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", edsite.Text == "全部" ? "%%" : edsite.Text));
                list.Add(new SqlPara("WebName", edweb.Text == "全部" ? "%%" : edweb.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) (myGridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSendRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            CommonClass.SetSite(edsite, true);
            edsite.Text = CommonClass.UserInfo.SiteName;
            edweb.Text = CommonClass.UserInfo.WebName;
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "预到货记录");
        }

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void edsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(edweb, edsite.Text);
        }
    }
}