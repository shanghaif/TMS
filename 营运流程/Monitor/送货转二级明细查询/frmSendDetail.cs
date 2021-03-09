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
    public partial class frmSendDetail : BaseForm
    {
        public frmSendDetail()
        {
            InitializeComponent();
        }
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", txtbDate.DateTime));
                list.Add(new SqlPara("t2", txteDate.DateTime));
                list.Add(new SqlPara("StartSite", txtbsite.Text.Trim() == "全部" ? "%%" : txtbsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", txtesite.Text.Trim() == "全部" ? "%%" : txtesite.Text.Trim()));
                list.Add(new SqlPara("WebName", txtebid.Text.Trim() == "全部" ? "%%" : txtebid.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendDetail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmSendDetail_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("送货转二级明细");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            CommonClass.SetSite(txtbsite, true);
            CommonClass.SetSite(txtesite, true);
            CommonClass.SetWeb(txtebid, true);

            txtbDate.DateTime = CommonClass.gbdate;
            txteDate.DateTime = CommonClass.gedate;
            txtesite.Text = CommonClass.UserInfo.SiteName;
            txtebid.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
    }
}
