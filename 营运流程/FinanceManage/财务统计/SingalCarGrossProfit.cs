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
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class SingalCarGrossProfit : BaseForm
    {
        public SingalCarGrossProfit()
        {
            InitializeComponent();
        }

        private void SingalCarGrossProfit_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("单车毛利统计表");//xj/2019/5/28
            CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1,myGridView3);
            // GridOper.SetGridViewProperty(myGridView1,myGridView3);
            BarMagagerOper.SetBarPropertity(bar3, bar4); //如果有具体的工具条，就引用其实例
            //GridOper.RestoreGridLayout(myGridView1,myGridView3);
            //FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            //FixColumn fix1 = new FixColumn(myGridView3, barSubItem4);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
            CommonClass.SetSite(true, StartSite, EndSiteName);
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
            StartSite.Text = "全部";
            EndSiteName.Text = "全部";
            GridOper.GetGridViewColumn(bandedGridView1, "GrossProfit").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);//hj20180704 
            GridOper.GetGridViewColumn(bandedGridView1, "incomeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView1, "SFFeeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView1, "AccBigcarTotal").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView1, "ZDFeeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.CreateStyleFormatCondition(bandedGridView1, "GrossProfit", FormatConditionEnum.Less, 0, Color.Yellow);

            GridOper.GetGridViewColumn(bandedGridView2, "GrossProfit").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);//hj20180704 
            GridOper.GetGridViewColumn(bandedGridView2, "incomeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView2, "SFFeeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView2, "CarCost").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.GetGridViewColumn(bandedGridView2, "ZDFeeAll").AppearanceCell.BackColor = Color.FromArgb(128, 255, 128);
            GridOper.CreateStyleFormatCondition(bandedGridView2, "GrossProfit", FormatConditionEnum.Less, 0, Color.Yellow);
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (bdate.DateTime.Date > edate.DateTime.Date)
                {
                    XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list.Add(new SqlPara("causeName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("webName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GrossProfit_By_DEPARTURE_New_SJ_1", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["GrossProfitLV"] = ds.Tables[0].Rows[i]["GrossProfitLV"] + "%";
                }
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(bandedGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridOper.SaveGridLayout(bandedGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //  GridOper.DeleteGridLayout(bandedGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(bandedGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView2);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            showdetail();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.xtraTabPage2.PageVisible = true;
            showdetail();
        }

        private void showdetail()
        {
            if (bandedGridView1.FocusedRowHandle < 0)
            {
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", bandedGridView1.GetFocusedRowCellValue("DepartureBatch")));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELISTBYDEPARTUREBATCH_new", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {

            MsgBox.ShowOK("如果收入合计=0，则毛利率等于0；如果收入≠0，则毛利率=毛利/收入合计" + "\n" + "毛利=收入合计-总成本合计" + "\n" + "收入合计=配载金额+异动增加金额" + "\n" + "总成本合计=大车费合计+始发费用合计+终端费用合计" + "\n" + "始发费用合计=始发其他费+提货费+短驳费+(装卸费-大车)" + "\n" + "终端费用合计=中转费+转送费+送货费+终端其他费+(装卸-大车到货)" + "\n" +
            "\n"+"配载金额："+ "\n"+"本公司开的单 配载金额=实收运费*本次发车件数/总件数" + "\n" + "转分拨过来的运单 配载金额=结算费用合计*本次发车件数/总件数" + "\n" + "结算费用合计=（结算始发操作费+结算干线费+结算送货费+结算中转费+结算终端操作费+结算装卸费+结算上楼费+结算进仓费）*本次发车件数/总件数");

        }

        private void bandedGridView2_DoubleClick(object sender, EventArgs e)
        {

            int rows = bandedGridView2.FocusedRowHandle;
            string a = bandedGridView2.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Tag = a;
            frm.ShowDialog();

        }

        private void btn_save_Click_1(object sender, EventArgs e)
        {
            MsgBox.ShowOK("如果收入合计=0，则毛利率等于0；如果收入≠0，则毛利率=毛利/收入合计" + "\n" + "毛利=收入合计-总成本合计" + "\n" + "收入合计=配载金额+异动增加金额" + "\n" + "总成本合计=大车费合计+始发费用合计+终端费用合计" + "\n" + "始发费用合计=始发其他费+提货费+短驳费+(装卸费-大车)" + "\n" + "终端费用合计=中转费+转送费+送货费+终端其他费+(装卸-大车到货)" + "\n" +
           "\n" + "配载金额：" + "\n" + "本公司开的单 配载金额=实收运费*本次发车件数/总件数" + "\n" + "转分拨过来的运单 配载金额=结算费用合计*本次发车件数/总件数" + "\n" + "结算费用合计=（结算始发操作费+结算干线费+结算送货费+结算中转费+结算终端操作费+结算装卸费+结算上楼费+结算进仓费）*本次发车件数/总件数");
        }
    }
}
