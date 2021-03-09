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
    public partial class frmDepartureCount : BaseForm
    {
        public frmDepartureCount()
        {
            InitializeComponent();
        }

        private void frmDepartureCount_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            //CommonClass.SetSite(bSite, true);
            SetSite(bSite, true);
           // CommonClass.SetSite(eSite, true);
            SetSite(eSite, true);
            //CommonClass.SetCause(Cause, true);
            SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "isover", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.LightGreen);
        }


        #region 获取基础信息
        public static DataSet dsWeb = new DataSet();
        private void SetSite(ComboBoxEdit cb, bool isall)
        {

            // 
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_ForDataStatistics", list);

                DataSet dsSite = SqlHelper.GetDataSet(sps);
                if (dsSite == null || dsSite.Tables.Count == 0) return;
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsSite.Tables[0].Rows[i]["SiteName"].ToString()) && !cb.Properties.Items.Contains(dsSite.Tables[0].Rows[i]["SiteName"].ToString()))
                    {
                        cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                    }
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void SetCause(ComboBoxEdit cb, bool isall)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE_ForDataStatistics", list);
                DataSet dsCause = SqlHelper.GetDataSet(sps);
                if (dsCause == null || dsCause.Tables.Count == 0) return;
                for (int i = 0; i < dsCause.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsCause.Tables[0].Rows[i]["CauseName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void SetArea(ComboBoxEdit cb, string AreaCause, bool isall)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA__ForDataStatistics", list);
                DataSet dsArea = SqlHelper.GetDataSet(sps);
                if (dsArea == null || dsArea.Tables.Count == 0) return;
                if (AreaCause == "全部") AreaCause = "%%";
                DataRow[] dr = dsArea.Tables[0].Select("AreaCause like '" + AreaCause + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["AreaName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
                else
                {
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

      
        private void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {

            try
            {
                if (dsWeb == null || dsWeb.Tables.Count == 0)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ForDataStatistics", list);
                     dsWeb = SqlHelper.GetDataSet(sps);
                }
                if (dsWeb == null || dsWeb.Tables.Count == 0) return;
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void SetDep(ComboBoxEdit cb, string DepArea, bool isall)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART_ForDataStatistics", list);
                DataSet dsDep = SqlHelper.GetDataSet(sps);
                if (dsDep == null || dsDep.Tables.Count == 0) return;
                if (DepArea == "全部") DepArea = "%%";
                DataRow[] dr = dsDep.Tables[0].Select("DepArea like '" + DepArea + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["DepName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetUser(ComboBoxEdit cb, string WebName, bool isall)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_USERNAME_ForDataStatistics", list);
                DataSet dsUsers = SqlHelper.GetDataSet(sps);
                if (dsUsers == null || dsUsers.Tables.Count == 0) return;
                if (WebName == "全部") WebName = "%%";
                DataRow[] dr = dsUsers.Tables[0].Select("WebName like '" + WebName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["UserName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        public static void SetCauseWeb(ComboBoxEdit cb, string BelongCause, string BelongArea)
        {
            
            try
            {
                if (dsWeb == null || dsWeb.Tables.Count == 0)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ForDataStatistics", list);
                    dsWeb = SqlHelper.GetDataSet(sps);
                }
                if (dsWeb == null || dsWeb.Tables.Count == 0) return;
                if (BelongCause == "全部") BelongCause = "%%";
                if (BelongArea == "全部") BelongArea = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("BelongCause like '" + BelongCause + "' and BelongArea like '" + BelongArea + "' ");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                cb.Properties.Items.Add("全部");
                cb.Text = "全部";
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        #endregion

        private void freshData()
        {
            myGridView1.ClearColumnsFilter();
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
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
                list.Add(new SqlPara("LoadingType", LoadingType.SelectedIndex));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ForDataStatistics", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                {
                 if (ds.Tables[0].Columns.Contains("SumDepartAmount"))
                      ds.Tables[0].Rows[i]["SumDepartAmount"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["SumDepartAmount"]), 0);
                if (ds.Tables[0].Columns.Contains("SumOptWeight"))
                    ds.Tables[0].Rows[i]["SumOptWeight"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["SumOptWeight"]), 2);
                if (ds.Tables[0].Columns.Contains("SumFeeWeight"))
                    ds.Tables[0].Rows[i]["SumFeeWeight"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["SumFeeWeight"]), 2);
                if (ds.Tables[0].Columns.Contains("SumFeeVolume"))
                    ds.Tables[0].Rows[i]["SumFeeVolume"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["SumFeeVolume"]), 2);
                if (ds.Tables[0].Columns.Contains("Freight"))
                    ds.Tables[0].Rows[i]["Freight"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["Freight"]), 2);
                }
                    gridDepartureList.DataSource = ds.Tables[0];
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

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmDepartureCountAdd wn = new frmDepartureCountAdd();
            //wn.Show();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            freshData();
        }

        private void barBtnMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            frmDepartureModyCount frm = new frmDepartureModyCount();
            frm.sDepartureBatch = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("DepartureBatch"));
            frm._arriveDate = myGridView1.GetFocusedRowCellValue("ArrivedDate") == DBNull.Value ? null : myGridView1.GetFocusedRowCellValue("ArrivedDate");
            frm.ShowDialog();
            //如果有修改数据再刷新数据
            if (frm.IsModify)
            {
                int rowHandel = myGridView1.FocusedRowHandle;
                btnRetrieve.PerformClick();
                myGridView1.FocusedRowHandle = rowHandel;
            }
        }

        private void barBtnVehicleDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ArrivedDate")) != "")
                {
                    MsgBox.ShowOK("本车已经到货,不能作废!\r\n如果确实需要作废,请联系本车到站负责人员取消本车到货!");
                    return;
                }
                string departureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
                List<SqlPara> listQuery = new List<SqlPara>();
                listQuery.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_Batch", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string billNos = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ConvertType.ToString(ds.Tables[0].Rows[i]["ToDate"]) != "")
                    {
                        MsgBox.ShowOK("本车已经有到货,不能作废!\r\n如果确实需要作废,请联系本车到站负责人员取消本车到货!");
                        return;
                    }
                    if (CommonClass.QSP_LOCK_1(ds.Tables[0].Rows[i]["BillNO"].ToString(), ds.Tables[0].Rows[i]["BillDate"].ToString()))
                    {
                        MsgBox.ShowOK("本车已经有运单被锁账，不能整车作废!");
                        return;
                    }
                    billNos = billNos + ds.Tables[0].Rows[i]["BillNO"].ToString() + "@";
                   
                }

                    if (MsgBox.ShowYesNo("确定作废本车？\r\n批次：" + departureBatch) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定作废本车？请三思！！\r\n批次：" + departureBatch) != DialogResult.Yes) return;

                //提前获取到轨迹信息
                List<SqlPara> lists = new List<SqlPara>();
                lists.Add(new SqlPara("DepartureBatch", null));
                lists.Add(new SqlPara("BillNO", billNos));
                lists.Add(new SqlPara("tracetype", "配载发车"));
                lists.Add(new SqlPara("num", 5));
                DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTURE", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("本车作废成功!");
                CommonSyn.DepartureDeleteSyn(departureBatch, billNos, 0);//2018-4-11 分拨同步
                CommonSyn.TraceSyn(null, billNos, 5, "配载发车", 2, null, dss);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void gridDepartureList_DoubleClick(object sender, EventArgs e)
        {
            barBtnMod.PerformClick();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
           // CommonClass.SetArea(Area, Cause.Text);
            SetArea(Area, Cause.Text, true);
           // CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
            SetCauseWeb(web, Cause.Text, Area.Text);
            
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
            SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void barBtnvExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "配载记录");
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

        private void barBtnLoadScan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            frmScanStatistics fss = new frmScanStatistics();
            fss.dtvehicleno = GridOper.GetRowCellValueString(myGridView1, "CarNo");
            fss.dtinoneflag = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            fss.dtchauffer = GridOper.GetRowCellValueString(myGridView1, "DriverName");
            fss.dtsenddate = ConvertType.ToDateTime(GridOper.GetRowCellValueString(myGridView1, "DepartureDate"));
            fss.Show();
        }

        private void barBtnLoadScanPeop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmScanBillMan fsb = frmScanBillMan.Get_frmScanBillMan;
            fsb.MdiParent = this.MdiParent;
            fsb.Dock = DockStyle.Fill;
            fsb.Show();
            fsb.Focus();
        }

        private void btnDepart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) {
                MsgBox.ShowOK("请选择一条配载信息！");
                return;
            }
            if (MsgBox.ShowYesNo("确定要点击发车？") != DialogResult.Yes) return;
            frmAddDepart fm = new frmAddDepart();
            string arriveSite = GridOper.GetRowCellValueString(myGridView1, "EndSite");
            string batchno = GridOper.GetRowCellValueString(myGridView1,"DepartureBatch");
            string[] sites;
            string firstsite, secondsite;
            if (arriveSite.Contains(","))
            {
                sites = arriveSite.Split(',');
                firstsite = sites[0];
                secondsite = sites[1];
                fm.siteOne = firstsite;
                fm.siteTwo = secondsite;
            }
            else {
                fm.siteOne = arriveSite;
            }
            fm.batch = batchno;
            fm.ShowDialog();
        }

        private void updateDriverTakePay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowError("请选择一条配载信息！");
                return;
            }
            frmUpdateDriverTakePay fm = new frmUpdateDriverTakePay();
            fm._batchNo = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            fm._diverTakePay = GridOper.GetRowCellValueString(myGridView1, "DriverTakePay");
            fm.ShowDialog();
        }
    }
}