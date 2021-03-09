using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAgingCustomerDetail : BaseForm
    {
        public frmAgingCustomerDetail()
        {
            InitializeComponent();
        }
        public frmAgingCustomerDetail(string PayMent)
        {
            this.PayMent = PayMent;
            InitializeComponent();
            this.Text = "欠款账龄明细";
            
        }
        private string PayMent = "";
        private void frmAgingCustomerDetail_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("欠款账龄明细");//xj/2019/5/28
            GetAllWebId();
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            Web.Text = CommonClass.UserInfo.WebName;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_frmAgingCustomerDetail_1", list);
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
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//导出
        {
            GridOper.ExportToExcel(myGridView1, this.Text);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                Web.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    Web.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
                Web.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}