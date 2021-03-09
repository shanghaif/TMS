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
    public partial class frmDepartureDistributionNew : BaseForm
    {
        public frmDepartureDistributionNew()
        {
            InitializeComponent();
        }

        private void frmDepartureDistributionNew_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("始发分拨费返款(新)");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);


        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //提取数据
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.Text.Trim()));
                list.Add(new SqlPara("edate", edate.Text.Trim()));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                //list.Add(new SqlPara("", CauseName.Text.Trim()));
                //list.Add(new SqlPara("", AreaName.Text.Trim()));
                list.Add(new SqlPara("SCDesWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("jswdxz", comboBoxEdit4.Text.Trim() == "全部" ? "%%" : comboBoxEdit4.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_sffbffkmx", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
            
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //导出
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "始发分拨费返款明细");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
    }
}