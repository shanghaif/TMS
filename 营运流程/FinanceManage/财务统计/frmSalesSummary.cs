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
    public partial class frmSalesSummary : BaseForm
    {
        public frmSalesSummary()
        {
            InitializeComponent();
        }

        private void frmSalesSummary_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("营业额汇总表");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            Web.Text = CommonClass.UserInfo.WebName;

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetCause(Cause, true);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("Cause", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SalesSummary", list);
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
            GridOper.ExportToExcel(myGridView1, "营业额汇总");
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

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void Cause_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void Area_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text);
        }
    }
}