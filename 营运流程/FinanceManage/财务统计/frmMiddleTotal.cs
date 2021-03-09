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
    public partial class frmMiddleTotal : BaseForm
    {
        public frmMiddleTotal()
        {
            InitializeComponent();
        }



        private void frmMiddleTotal_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetSite(edbsite, true);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area,Cause.Text);
            CommonClass.SetWeb(edWeb, Area.Text);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            edWeb.Text = CommonClass.UserInfo.WebName;
            edbsite.Text = CommonClass.UserInfo.SiteName;
        }


        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", edbsite.Text.Trim() == "全部" ? "%%" : edbsite.Text.Trim()));
                //list.Add(new SqlPara("datetype", comboBoxEdit1.Text.Trim() == "中转时间" ? "0" : "1"));
                //list.Add(new SqlPara("WebType", comboBoxEdit2.Text.Trim() == "中转网点" ? "0" : "1"));
                list.Add(new SqlPara("datetype", comboBoxEdit1.SelectedIndex));
                list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
                list.Add(new SqlPara("WebName", edWeb.Text.Trim() == "全部" ? "%%" : edWeb.Text.Trim()));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MiddleTotal", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0 || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text.Trim());
            //CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
