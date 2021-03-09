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
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmPackageToZXAccept : BaseForm
    {
        public frmPackageToZXAccept()
        {
            InitializeComponent();
        }

        private void frmPackageToZXAccept_Load(object sender, EventArgs e)
        {
            GetAllWebId();//hj 20180124
            GetAllSite();//hj 20180124
            // CommonClass.FormSet(this);
            CommonClass.InsertLog("拨入接收");//xj/2019/5/28
            CommonClass.FormSet(this, false,true);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            //CommonClass.SetCause(SiteName, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            SiteName.Text = CommonClass.UserInfo.SiteName;
            WebName.Text = CommonClass.UserInfo.WebName;

            try { GridOper.CreateStyleFormatCondition(myGridView1, "SCState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已接", Color.FromArgb(128, 255, 128)); }
            catch { }
            try { GridOper.CreateStyleFormatCondition(myGridView2, "SCState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已接", Color.FromArgb(128, 255, 128)); }
            catch { }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ExportToExcel(myGridView1, "货物分拨接收记录汇总");
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.ExportToExcel(myGridView2, "货物分拨接收记录明细");
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.AllowAutoFilter(myGridView1);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.AllowAutoFilter(myGridView2);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.SaveGridLayout(myGridView1);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.SaveGridLayout(myGridView2);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
            }
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ShowAutoFilterRow(myGridView1);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.ShowAutoFilterRow(myGridView2);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("state", comboBoxEdit1.SelectedIndex));
            list.Add(new SqlPara("SiteName", SiteName.Text.Trim() == "全部" ? "%%" : SiteName.Text.Trim()));//hj 20180125
            list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));//hj 20180125

            myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_ACCEPT", list));


            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("t1", bdate.DateTime));
            list1.Add(new SqlPara("t2", edate.DateTime));
            list1.Add(new SqlPara("state", comboBoxEdit1.SelectedIndex));
            list1.Add(new SqlPara("SiteName", SiteName.Text.Trim() == "全部" ? "%%" : SiteName.Text.Trim()));//hj 20180125
            list1.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));//hj 20180125

            myGridControl2.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_ACCEPT_Detail", list1));

        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
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



        private void myGridControl1_DoubleClick_1(object sender, EventArgs e)
        {
            barButtonItem3.PerformClick();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            int rowhandle1 = myGridView2.FocusedRowHandle;
            if (rowhandle < 0 && rowhandle1<0) return;

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                frmPackageToZXDetailAccept w = new frmPackageToZXDetailAccept(GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch"));
                w.ShowDialog();
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                frmPackageToZXDetailAccept w = new frmPackageToZXDetailAccept(GridOper.GetRowCellValueString(myGridView2, rowhandle1, "AllocateBatch"));
                w.ShowDialog();
            }
        }

        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                WebName.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    WebName.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
                WebName.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void GetAllSite()
        {
            try
            {
                if (CommonClass.dsSite.Tables.Count == 0) return;
                SiteName.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsSite.Tables[0].Rows.Count; i++)
                {
                    SiteName.Properties.Items.Add(CommonClass.dsSite.Tables[0].Rows[i]["SiteName"]);
                }
                SiteName.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridView gv = new GridView();
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                gv = myGridControl1.MainView as GridView;
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                gv = myGridControl2.MainView as GridView;
            }
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string AllocateBatch = ConvertType.ToString(gv.GetFocusedRowCellValue("AllocateBatch"));
            if (AllocateBatch == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DepartureFB_BYCAR_PRINT", new List<SqlPara> { new SqlPara("AllocateBatch", AllocateBatch) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang("分拨清单", ds, CommonClass.UserInfo.gsqc);
            fpr.ShowDialog();
        }

        ////套打签收单
        //private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    myGridView2.PostEditor();
        //    string BillNoStr = "";
        //    for (int i = 0; i < myGridView2.RowCount; i++)
        //    {
        //        if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) > 0)
        //            BillNoStr += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + "@";
        //    }
        //    if (BillNoStr == "") return;

        //    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_ARRIVED_QSD_FB", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
        //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //    {
        //        MsgBox.ShowOK("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
        //        return;
        //    }
        //    frmRuiLangService.Print("提货单", ds.Tables[0]);
        //}



    }
}