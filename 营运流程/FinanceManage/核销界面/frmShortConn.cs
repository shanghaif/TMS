using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmShortConn : BaseForm
    {
        public frmShortConn()
        {
            InitializeComponent();
        }

        private void frmShortConn_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("短驳费核销");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetCause(Cause, true);
            GridOper.CreateStyleFormatCondition(myGridView1, "SCVerifState", FormatConditionEnum.Equal, "1", Color.LightGreen);
            GridOper.CreateStyleFormatCondition(myGridView1, "SCVerifState", FormatConditionEnum.Equal, "2", Color.FromArgb(255, 255, 128));

            bsite.Text = CommonClass.UserInfo.SiteName;
            esite.Text = "全部";

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            
            //CommonClass.SetArea(Area, Cause.Text, true);
            //CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短驳费核销明细");
        }
        frmShortConnLoad ww;
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmShortConnLoad ww = new frmShortConnLoad();
            //ww.ShowDialog();
            string frm = "frmShortConnLoad";
            if (CommonClass.CheckFormIsOpen(frm) == false)
            {
                ww = new frmShortConnLoad();
                ww.Show();
            }
            else
            {
                ww.Focus();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ShortConn_Verify", list);
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

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}