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
    public partial class frmShortConnOutbound : BaseForm
    {
        DataSet ds = new DataSet();

        public frmShortConnOutbound()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("短驳出库");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            //fix = new FixColumn(myGridView2, barSubItem4);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetSite(begSite, true);
            CommonClass.SetSite(endSite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
            begSite.EditValue = CommonClass.UserInfo.SiteName;

           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            myGridControl1.MainView = cmbCarOrGroup.SelectedIndex == 0 ? myGridView1 : myGridView2; //0 按车  1 按票
            (myGridControl1.MainView as GridView).ClearColumnsFilter();
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCSite", begSite.Text.Trim() == "全部" ? "%%" : begSite.Text.Trim()));
                list.Add(new SqlPara("SCDesSite", endSite.Text.Trim() == "全部" ? "%%" : endSite.Text.Trim()));
                list.Add(new SqlPara("begDate", bdate.DateTime));
                list.Add(new SqlPara("endDate", edate.DateTime));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("CarStartState", CarStartState.Text.Trim() == "全部" ? "%%" : CarStartState.Text));

                string sPara = cmbCarOrGroup.SelectedIndex == 0 ? "QSP_GET_SHORTCONN_CAR_GXKT" : "QSP_GET_SHORTCONN_BILL";
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    sPara = cmbCarOrGroup.SelectedIndex == 0 ? "QSP_GET_SHORTCONN_CAR_GXKT_ZQ" : "QSP_GET_SHORTCONN_BILL_ZQ";
                //    list.Add(new SqlPara("SCDESWeb", CommonClass.UserInfo.WebName));
                //}
                //else
                //{
                    list.Add(new SqlPara("SCDESWeb", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                //}
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, sPara, list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (cmbCarOrGroup.SelectedIndex == 0 && myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                else if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            begSite.Text = e.Node.Text;
        }

        private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            endSite.Text = e.Node.Text;
        }

        private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        {
            begSite.ClosePopup();
        }

        private void tvesite_MouseClick(object sender, MouseEventArgs e)
        {
            endSite.ClosePopup();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            frmRecShortConn ws = new frmRecShortConn();
            ws.U_SCBatchNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo").ToString();
            ws.U_SCDate = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate").ToString();
            ws.U_SCSite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite").ToString();
            ws.U_SCWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb").ToString();
            ws.U_SCCarNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo").ToString();
            ws.U_SCDriver = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver").ToString();
            ws.U_SCDesSite = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite").ToString();
            ws.U_SCDesWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb").ToString();
            ws.U_SCDContolMan = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCMan").ToString();
            ws.SCId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId").ToString();
            ws.isMod = false;
            //zaj 20181119
            ws.systemType = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SystemType").ToString();
            ws.shortType = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ShortType").ToString();
            ws.ShowDialog();
            //cbRetrieve.PerformClick();不刷新数据了
        }

        private void cancel()
        {
            if (myGridControl1.MainView.Name != myGridView1.Name) return;
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                //if ((myGridView1.GetRowCellValue(rowhandle, "SCWeb").ToString()) != CommonClass.UserInfo.WebName)
                //{
                //    MsgBox.ShowOK("请在短驳网点进行此操作！");
                //    return;
                //}
                #region 判断是否已发车 zb20190521
                string DepartBatch2 = Convert.ToString(myGridView2.GetRowCellValue(rowhandle, "SCBatchNo"));
                string DepartBatch1 = Convert.ToString(myGridView1.GetRowCellValue(rowhandle, "SCBatchNo"));
                string DepartureBatch = "";
                if (string.IsNullOrEmpty(DepartBatch1))
                {
                    DepartureBatch = DepartBatch2;
                }
                else
                {
                    DepartureBatch = DepartBatch1;
                }
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list1.Add(new SqlPara("Type", "短驳"));
                DataSet ds = new DataSet();
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_DEPARTUREBATCH", list1));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("本车已发车，请先取消发车!");
                    return;
                }
                #endregion

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "SCId").ToString());
                string SCBatchNo = (myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCId", id));
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                string json = CreateJson.GetShortJson(list, 1, "USP_DELETE_SHORTCONN_LMS", "", SCBatchNo, CommonClass.UserInfo.companyid);
                string url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                list.Add(new SqlPara("Json", json));
                list.Add(new SqlPara("URL", url));
                //list.Add(new SqlPara("ProcName", "USP_DELETE_SHORTCONN_LMS"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    string systemType = myGridView1.GetRowCellValue(rowhandle, "SystemType").ToString();
                    string shortType = myGridView1.GetRowCellValue(rowhandle, "ShortType").ToString();
                    CommonClass.SetOperLog(SCBatchNo, "", "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳取消短驳操作");
                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                    //zaj 20181115 代短驳整车取消
                    if ( shortType == "1")
                    {
                        CommonSyn.ShortReplaceLms(list, 1, "USP_DELETE_SHORTCONN_LMS", "", SCBatchNo, CommonClass.UserInfo.companyid);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAddShortconnect wpc = new frmAddShortconnect();
            wpc.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            cancel();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridControl1.MainView as ZQTMS.Lib.MyGridView);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gv = myGridControl1.MainView as GridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string SCBatchNo = ConvertType.ToString(gv.GetFocusedRowCellValue("SCBatchNo"));
            if (SCBatchNo == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_BYCAR_PRINT", new List<SqlPara> { new SqlPara("SCBatchNo", SCBatchNo) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i]["NowCompany"] = CommonClass.UserInfo.gsqc;
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang("短驳清单", ds);
            fpr.ShowDialog();
        }
        private void btnLockStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void btnStyleCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
            GridOper.DeleteGridLayout(myGridView2, myGridView2.Guid.ToString());
        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text, true);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text, true);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text, true);
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

        private void barButtonItem16_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmShortConnDelList frm = new frmShortConnDelList();
            frm.Show();
        }

        //点击发车
        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {

            int rowhandle = myGridView1.FocusedRowHandle;   //zhengjiafeng20181107
            if (rowhandle < 0) return;
            if (ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "StartTime")) != "")     
            {
                MsgBox.ShowOK("该批次已经做过点击发车");
                return;
            }
            try
            {
                if (cmbCarOrGroup.SelectedIndex == 1)
                {
                    MsgBox.ShowOK("短驳方式为【按车】的才能点击发车,请重新选择");
                    return;
                }
                if (myGridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条出库信息");
                    return;
                }
                string batchNo = GridOper.GetRowCellValueString(myGridView1, myGridView1.FocusedRowHandle, "SCBatchNo");
                string scDesSite = GridOper.GetRowCellValueString(myGridView1, myGridView1.FocusedRowHandle, "SCDesSite");
                string scDesWeb = GridOper.GetRowCellValueString(myGridView1, myGridView1.FocusedRowHandle, "SCDesWeb");
                string systemType = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SystemType").ToString();
                string shortType = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ShortType").ToString();
                frmShortVehicleStart frm = new frmShortVehicleStart();
                frm.batchNo = batchNo;
                frm.SCDesSite = scDesSite;
                frm.SCDesWeb = scDesWeb;
                frm.systemType = systemType;
                frm.shortType = shortType;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int row = myGridView1.FocusedRowHandle;
                if (row < 0)
                {
                    MsgBox.ShowError("请选择一条配载信息！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定取消发车吗？") != DialogResult.Yes) return;

                string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, "SCBatchNo");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("batchNo", DepartureBatch));
                string json = CreateJson.GetShortJson(list, 6, "USP_DELETE_ShortBILLVEHICLESTAR_ZQTMS", "", DepartureBatch, CommonClass.UserInfo.companyid);
                string url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                list.Add(new SqlPara("Json", json));
                list.Add(new SqlPara("URL", url));
                //list.Add(new SqlPara("ProcName", "USP_DELETE_ShortBILLVEHICLESTAR_ZQTMS"));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_ShortBILLVEHICLESTAR", list)) == 0) return;
                MsgBox.ShowOK();
                string systemType = myGridView1.GetRowCellValue(row, "SystemType").ToString();
                string shortType = myGridView1.GetRowCellValue(row, "ShortType").ToString();
                if (shortType == "1")
                {
                    CommonSyn.ShortReplaceLms(list, 6, "USP_DELETE_ShortBILLVEHICLESTAR_ZQTMS", "", DepartureBatch, CommonClass.UserInfo.companyid);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("请选择一条信息！");
            //    return;
            //}
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, "SCBatchNo");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "装卸费-短驳出库";
            frm.ShowDialog();

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MsgBox.ShowOK("按票提取 短驳费分摊： 短驳费乘以(单号的结算重量除以整个短驳批次的结算重量) ");
        }
    }
}