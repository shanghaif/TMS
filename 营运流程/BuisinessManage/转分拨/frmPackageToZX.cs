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
using Newtonsoft.Json;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmPackageToZX : BaseForm
    {
        public frmPackageToZX()
        {
            InitializeComponent();
        }
    
        private void frmPackageToZX_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("分拨到中心");//xj/2019/5/28
            CommonClass.FormSet(this, false, false);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetCause(CauseName, true);
            CommonClass.SetSite(true, StartSite, TransferSite);
            CauseName.Text = CommonClass.UserInfo.CauseName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ExportToExcel(myGridView1, "货物分拨记录汇总");
            }
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                GridOper.ExportToExcel(myGridView2, "货物分拨记录明细");
            }
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

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
            list.Add(new SqlPara("SiteName", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
            list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));

            myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_TX", list));


            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("t1", bdate.DateTime));
            list1.Add(new SqlPara("t2", edate.DateTime));
            list1.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
            list1.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
            list1.Add(new SqlPara("SiteName", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
            list1.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));

            myGridControl2.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_DETAIL_1", list1));


        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text.Trim(), true);
        }

        private void StartSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(BegWeb, StartSite.Text.Trim(), true);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPackageToZXLoadNew w = frmPackageToZXLoadNew.Get_frmPackageToZXLoadNew;
            w.Show();
            w.TopMost = true;
            w.TopMost = false;
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (GridOper.GetRowCellValueString(myGridView1, rowhandle, "AcceptMan") != "")
            {
                MsgBox.ShowOK("本车已接收,不能取消分拨!");
                return;
            }
            if (MsgBox.ShowYesNo("确定取消分拨？") != DialogResult.Yes) return;
            try
            {
                string batch = GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch");
                string AcceptCompanyId = GridOper.GetRowCellValueString(myGridView1, rowhandle, "AcceptCompanyId");
                ResponseModelClone<string> result = null;
                RequestModel<string> request = new RequestModel<string>();
                if (AcceptCompanyId == "100")
                {
                    Dictionary<string, string> dty = new Dictionary<string, string>();
                    dty.Add("AllocateBatch", batch);
                    dty.Add("AcceptCompanyId", "100");
                    request.Request = JsonConvert.SerializeObject(dty);
                    request.OperType = 0;
                    string json = JsonConvert.SerializeObject(request);
                    result = HttpHelper.HttpPost(json, "http://lms.dekuncn.com:7016/CenterSynService/CancelAllocateToBranch");
                    if (result.State == "200")
                    {
                        CommonSyn.TraceSyn(batch, null, 17, "分拨到中心", 2, "分拨到中心", null);
                        if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_FB_485", new List<SqlPara> { new SqlPara("Batch", GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch")) })) == 0) return;//USP_DELETE_DEPARTURE_FB
                        myGridView1.DeleteRow(rowhandle);
                        MsgBox.ShowOK("取消分拨成功!");
                    }

                }
                else
                {
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_FB_485", new List<SqlPara> { new SqlPara("Batch", GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch")) })) == 0) return;//USP_DELETE_DEPARTURE_FB
                    myGridView1.DeleteRow(rowhandle);
                    MsgBox.ShowOK("取消分拨成功!");
                }
                
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
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

        //private void myGridControl1_DoubleClick(object sender, EventArgs e)
        //{
        //    int rowhandle = myGridView1.FocusedRowHandle;
        //    if (rowhandle < 0) return;

        //    frmPackageToZXDetail w = new frmPackageToZXDetail(GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch"));
        //    w.ShowDialog();
        //}

        private void myGridControl1_DoubleClick_1(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmPackageToZXDetail w = new frmPackageToZXDetail(GridOper.GetRowCellValueString(myGridView1, rowhandle, "AllocateBatch"));
            w.ShowDialog();
        }

        /// <summary>
        /// 打印清单 hj20180419
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
    }
}