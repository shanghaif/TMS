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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmSettlementDetail : BaseForm
    {
        public frmSettlementDetail()
        {
            InitializeComponent();
        }

        private void frmProfitsStatisiticals_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("加盟线路对账汇总");//xj/2019/5/28
            CommonClass.FormSet(this, false, true);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            //bdate.DateTime = CommonClass.gbdate;
            //edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
           // CommonClass.SetWeb(LoadWebName, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);
            GetAllWebId();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("esite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("webname", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("LoadWeb", LoadWebName.Text.Trim() == "全部" ? "%%" : "%"+LoadWebName.Text.Trim()+"%"));

                //gridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_ProfitsStatistical_Detail", list));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_SettlementDetail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0]; 
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //双击弹出明细
        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = bandedGridView1.FocusedRowHandle;
            //int rowhandle = gridControl1.fo
            if (rowhandle < 0 ) return;
            string VerifyOffType = bandedGridView1.GetRowCellValue(rowhandle, "IncomeProject").ToString();
            string VerifyOffType1 = bandedGridView1.GetRowCellValue(rowhandle, "SpendingProject").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
            list.Add(new SqlPara("esite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
            list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
            list.Add(new SqlPara("webname", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
            //list.Add(new SqlPara("LoadWeb", LoadWebName.Text.Trim() == "全部" ? "%%" : LoadWebName.Text.Trim()));
            list.Add(new SqlPara("LoadWeb", LoadWebName.Text.Trim() == "全部" ? "%%" : "%" + LoadWebName.Text.Trim() + "%"));
            list.Add(new SqlPara("VerifyOffType", VerifyOffType));
            list.Add(new SqlPara("VerifyOffType1", VerifyOffType1));

            //if (VerifyOffType != "") 
            myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_SettlementDetail_Summary", list));
            xtraTabControl1.SelectedTabPageIndex = 1;

            //if (VerifyOffType1 != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_SettlementDetail_Summary", list));
            //xtraTabControl1.SelectedTabPageIndex = 1;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.AllowAutoFilter(bandedGridView1);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.AllowAutoFilter(myGridView1);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.SaveGridLayout(myGridView1);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ExportToExcel(bandedGridView1, "结算汇总");
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.ExportToExcel(myGridView1, "结算汇总明细");
            }
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = bandedGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string VerifyOffType = bandedGridView1.GetRowCellValue(rowhandle, "IncomeProject").ToString();
            string VerifyOffType1 = bandedGridView1.GetRowCellValue(rowhandle, "SpendingProject").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
            list.Add(new SqlPara("esite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
            list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
            list.Add(new SqlPara("webname", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
            //list.Add(new SqlPara("LoadWeb", LoadWebName.Text.Trim() == "全部" ? "%%" : LoadWebName.Text.Trim()));
            list.Add(new SqlPara("LoadWeb", LoadWebName.Text.Trim() == "全部" ? "%%" : "%" + LoadWebName.Text.Trim() + "%"));
            list.Add(new SqlPara("VerifyOffType", VerifyOffType));
            list.Add(new SqlPara("VerifyOffType1", VerifyOffType1));
            //if (VerifyOffType != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_SettlementDetail_Summary", list));
            //xtraTabControl1.SelectedTabPageIndex = 1;

           //if (VerifyOffType1 != "") 
            myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_Get_SettlementDetail_Summary", list));
            xtraTabControl1.SelectedTabPageIndex = 1;
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

        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                LoadWebName.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    LoadWebName.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
