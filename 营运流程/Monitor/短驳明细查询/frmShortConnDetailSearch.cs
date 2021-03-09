using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;


namespace ZQTMS.UI
{
    public partial class frmShortConnDetailSearch : BaseForm
    {
        public frmShortConnDetailSearch()
        {
            InitializeComponent();
        }
        private void frmShortConnDetailSearch_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("短驳明细查询");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(bSite, true);
            CommonClass.SetSite(eSite, true);
            CommonClass.SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
        }
        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            myGridView1.ClearColumnsFilter();
            string BillNos = "";
            if (!string.IsNullOrEmpty(textBillNos.Text))
            {
                string[] billNos = textBillNos.Lines;
                if (billNos.Length > 20)
                {
                    MsgBox.ShowOK("最多只能输入20个单号！");
                    return;
                }
                for (int i = 0; i < billNos.Length; i++)
                {
                    BillNos += billNos[i].Trim() + ",";
                }
            }
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            string state = cbbState.Text.Trim() == "全部" ? "%%" : cbbState.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("bSite", bsite));
                list.Add(new SqlPara("eSite", esite));
                list.Add(new SqlPara("webName", webName));
                list.Add(new SqlPara("state", state));
                list.Add(new SqlPara("BillNos", BillNos));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query,"QSP_GET_ShortConnDetail", list);
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短驳明细查询");
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void btnClose_Click(object sender, EventArgs e)
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

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
    }

}
