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
    public partial class frmDeparture : BaseForm
    {
        public frmDeparture()
        {
            InitializeComponent();
        }

        private void frmDeparture_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("分单配载");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);
            bdate.DateTime = CommonClass.gbdate.AddDays(+1).AddHours(-24);
            edate.DateTime = CommonClass.gedate.AddDays(+1).AddHours(-24);

            CommonClass.SetSite(bSite, true);
            CommonClass.SetSite(eSite, true);
            CommonClass.SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "isover", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.LightGreen);
        }

        private void freshData()
        {
            myGridView1.ClearColumnsFilter();
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            string carStartState = CarStartState.Text.Trim() == "全部" ? "%%" : CarStartState.Text;
            try
            {
                //if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                //{
                //    webName = CommonClass.UserInfo.WebName;
                //}
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("bSite", bsite));
                list.Add(new SqlPara("eSite", esite));
                list.Add(new SqlPara("webName", webName));
                list.Add(new SqlPara("CarStartState", carStartState));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_KT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) 
                {
                 if (ds.Tables[0].Columns.Contains("SumDepartAmount"))
                      ds.Tables[0].Rows[i]["SumDepartAmount"] = Math.Round(ConvertType.ToDecimal(ds.Tables[0].Rows[i]["SumDepartAmount"]),2);
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
            frmDepartureAdd wn = new frmDepartureAdd();
            wn.Show();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            freshData();
        }

        private void barBtnMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            if (ConvertType.ToString(myGridView1.GetFocusedRowCellValue("isCompany")) == "非本公司配载" )
            {
                MsgBox.ShowOK("非本公司配载的不允许修改！");
                return;
            }
            frmDepartureMody frm = new frmDepartureMody();
            frm.sDepartureBatch = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("DepartureBatch"));
            frm._arriveDate = myGridView1.GetFocusedRowCellValue("ArrivedDate") == DBNull.Value ? null : myGridView1.GetFocusedRowCellValue("ArrivedDate");
            frm.strPeiZaiType = myGridView1.GetFocusedRowCellValue("SystemType").ToString();//配载类型ld
            frm.strCompanyId = myGridView1.GetFocusedRowCellValue("companyid").ToString();
            frm.strCompanyName = myGridView1.GetFocusedRowCellValue("companyName").ToString();
            frm.strToken = myGridView1.GetFocusedRowCellValue("token").ToString();
            frm.AccPZ = ConvertType.ToDecimal(myGridView1.GetFocusedRowCellValue("SumDepartAmount").ToString());
            frm.ShowDialog();
            //如果有修改数据再刷新数据
            if (frm.IsModify)
            {
                int rowHandel = myGridView1.FocusedRowHandle;
                btnRetrieve.PerformClick();
                myGridView1.FocusedRowHandle = rowHandel;
            }
        }

        //车次作废
        private void barBtnVehicleDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string strState = myGridView1.GetRowCellValue(rowhandle, "CarStartState").ToString();
                if (strState == "在途中" || strState == "已到车" || strState == "已到货")     //zhengjiafeng20181013
                {
                    if (strState == "在途中")
                    {
                        MsgBox.ShowOK("请先取消发车!");
                        return;
                    }
                        
                    MsgBox.ShowOK("本车" + strState + ",不能作废!");  
                    return;
                }
                string departureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");

                //大车费核销过不能整车作废  除非反核销
                try
                {
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("BatchNo", departureBatch));
                    DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_BY_BatchNo", list1));
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        MsgBox.ShowError("大车费已核销，如需整车作废，请先反核销！");
                        return;
                    }

                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ex.Message);
                }


                List<SqlPara> listQuery = new List<SqlPara>();
                #region 如果已发车，需要取消本车，再整车作废 zb20190514
                listQuery.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_Batch", listQuery);
                DataSet ds = SqlHelper.GetDataSet(spsQuery);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                string billNos = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ConvertType.ToString(ds.Tables[0].Rows[i]["ToDate"]) != "")
                    {
                        MsgBox.ShowOK("本车已发车,不能作废!");
                        return;
                    }
                    if (CommonClass.QSP_LOCK_1(ds.Tables[0].Rows[i]["BillNO"].ToString(), ds.Tables[0].Rows[i]["BillDate"].ToString()))
                    {
                        MsgBox.ShowOK("本车已经有运单被锁账，不能整车作废!");
                        return;
                    }
                    billNos = billNos + ds.Tables[0].Rows[i]["BillNO"].ToString() + "@";

                }
                #endregion
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

                string ContractNO = GridOper.GetRowCellValueString(myGridView1, "ContractNO");
                string strToken = GridOper.GetRowCellValueString(myGridView1, "token");
                CommonSyn.CancleHaoDuoCheOrder(ContractNO, strToken);//取消好多车订单
                //string strPeiZaiType = GridOper.GetRowCellValueString(myGridView1, "SystemType");
                //if (strPeiZaiType == "ZQTMS")
                //{
                //    CommonSyn.LMSDepartureSysZQTMS(list, 1, "USP_DELETE_BILLDEPARTURE", billNos, departureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（整车作废）
                //}
                //else
                //{
                    //CommonSyn.DepartureDeleteSyn(departureBatch, billNos, 0);//2018-4-11 分拨同步

                    CommonSyn.CancelVecheil(billNos);
                    CommonSyn.TraceSyn(null, billNos, 5, "配载发车", 2, null, dss);
                //}

                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("本车作废成功!");
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
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
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
        public bool isbl = false;//是否发车成功
        private void btnDepart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (myGridView1.FocusedRowHandle < 0) {
            //    MsgBox.ShowOK("请选择一条配载信息！");
            //    return;
            //}
            //if (MsgBox.ShowYesNo("确定要点击发车？") != DialogResult.Yes) return;
            //frmAddDepart fm = new frmAddDepart();
            //string arriveSite = GridOper.GetRowCellValueString(myGridView1, "EndSite");
            //string batchno = GridOper.GetRowCellValueString(myGridView1,"DepartureBatch");
            //string[] sites;
            //string firstsite, secondsite;
            //if (arriveSite.Contains(","))
            //{
            //    sites = arriveSite.Split(',');
            //    firstsite = sites[0];
            //    secondsite = sites[1];
            //    fm.siteOne = firstsite;
            //    fm.siteTwo = secondsite;
            //}
            //else {
            //    fm.siteOne = arriveSite;
            //}
            //fm.batch = batchno;
            //fm.ShowDialog();

            #region   zaj 2017-7-27修改点击发车
            if (myGridView1.FocusedRowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条配载信息！");
                return;
            }
            if (GridOper.GetRowCellValueString(myGridView1, "isover").ToString() != "1")
            {
                MsgBox.ShowOK("请先完成本车，再做发车操作！");
                return;
            }
            try
            {
                string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
                List<SqlPara> list_check = new List<SqlPara>();
                list_check.Add(new SqlPara("DepartureBatch", DepartureBatch));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "GSP_Check_BILLVEHICLESTAR_Merge", list_check);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("该批次已经做过点击发车，不可重复操作！");
                    return;
                }

                //if (MsgBox.ShowYesNo("确定要点击发车？") != DialogResult.Yes) return;
                //frmSendCarCheckMessage fsc = new frmSendCarCheckMessage(DepartureBatch);
                //fsc.Owner = this;
                //fsc.ShowDialog();
                //if (string.IsNullOrEmpty(isPhone))
                //{
                //    return;
                //}

                frmAddDepartClone fm = new frmAddDepartClone();
                string arriveSite = GridOper.GetRowCellValueString(myGridView1, "EndSite");
                string batchno = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
                string loadWeb = GridOper.GetRowCellValueString(myGridView1, "LoadWeb");
                string loadSite = GridOper.GetRowCellValueString(myGridView1, "LoadSite");
                string LongSource = GridOper.GetRowCellValueString(myGridView1, "LongSource");
                string carType = GridOper.GetRowCellValueString(myGridView1, "CarType");
                string arriveWeb = GridOper.GetRowCellValueString(myGridView1, "EndWeb");//目的网点
                string beginSite = GridOper.GetRowCellValueString(myGridView1, "LoadSite");
                DateTime departureDate = Convert.ToDateTime(GridOper.GetRowCellValueString(myGridView1, "DepartureDate"));//财务发运时间
                string CarSoure = GridOper.GetRowCellValueString(myGridView1, "CarSoure");
                string CarNO = GridOper.GetRowCellValueString(myGridView1, "CarNO");
                string CarrNO = GridOper.GetRowCellValueString(myGridView1, "CarrNO");
                string DriverName = GridOper.GetRowCellValueString(myGridView1, "DriverName");
                string DriverPhone = GridOper.GetRowCellValueString(myGridView1, "DriverPhone");
                string ContractNO = GridOper.GetRowCellValueString(myGridView1, "ContractNO");
                string strPeiZaiType = GridOper.GetRowCellValueString(myGridView1, "SystemType");//配载类型
                string[] webs;
                string firstweb = "", secondweb = "", thirdweb = "";
                if (arriveWeb.Contains(","))
                {
                    webs = arriveWeb.Split(',');
                    firstweb = webs[0];
                    secondweb = webs[1];
                    if (webs.Length > 2)
                    {
                        thirdweb = webs[2];
                    }
                }
                else
                {
                    firstweb = arriveWeb;
                }

                string[] sites;
                string firstsite = "", secondsite = "", thirdsite = "";
                if (arriveSite.Contains(","))
                {
                    sites = arriveSite.Split(',');
                    firstsite = sites[0];
                    secondsite = sites[1];
                    if (sites.Length > 2)
                    {
                        thirdsite = sites[2];
                    }
                    //fm.siteOne = firstsite;
                    // fm.siteTwo = secondsite;
                    // fm.loadType = GridOper.GetRowCellValueString(myGridView1, "LoadingType");
                }
                else
                {
                    firstsite = arriveSite;
                    //fm.siteOne = arriveSite;
                    // fm.loadType = GridOper.GetRowCellValueString(myGridView1, "LoadingType");
                }
                fm.siteOne = firstsite;
                fm.siteTwo = secondsite;
                fm.siteThree = thirdsite;
                fm.loadType = GridOper.GetRowCellValueString(myGridView1, "LoadingType");
                fm.batch = batchno;
                fm.LoadWeb = loadWeb;
                fm.LongSource = LongSource;
                fm.carType = carType;
                fm.firstweb = firstweb;
                fm.secondweb = secondweb;
                fm.thirdweb = thirdweb;
                fm.loadSite = loadSite;
                fm.beginSite = beginSite;
                fm.departureDate = departureDate;
                //fm.isPhone = isPhone;
                fm.carSoure = CarSoure;
                fm.carNO = CarNO;
                fm.carrNO = CarrNO;
                fm.driverName = DriverName;
                fm.driverPhone = DriverPhone;
                fm.contractNO = ContractNO;
                fm.strPeiZaiType = strPeiZaiType;
                fm.Owner = this;
                fm.ShowDialog();

                //isPhone = string.Empty;//清空发车验证手机号
                if (isbl)
                {
                    this.myGridView1.SetFocusedRowCellValue("CarStartState", "在途中");
                    isbl = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            #endregion
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

        //取消发车
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int i = myGridView1.FocusedRowHandle;
                if (i < 0)
                {
                    MsgBox.ShowOK("请选择一条配载信息！");
                    return;
                }
                DataRow row = this.myGridView1.GetDataRow(i);
                if (row != null && row["CarStartState"].ToString() != "在途中")
                {
                    MsgBox.ShowOK("当前数据发车状态为《" + row["CarStartState"].ToString() + "》，不需要取消发车！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定取消发车吗？") != DialogResult.Yes) return;

                string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");

                //LMS配载同步ZQTMS（取消发车）    
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("batchNo", DepartureBatch));
                SqlParasEntity sp = new SqlParasEntity(OperType.Query, "QSP_Get_BillVehicleStar_By_ID", list);
                DataSet ds = SqlHelper.GetDataSet(sp);
                //删除发车记录
                list = new List<SqlPara>();
                list.Add(new SqlPara("batchNo", DepartureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLVEHICLESTAR_Merge_KT", list)) == 0) return;
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("batchNo", DepartureBatch));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPD_BILLDEPARTURE_STATE", list1));
                MsgBox.ShowOK();
                this.myGridView1.SetFocusedRowCellValue("CarStartState", "未发车");

                
                //string strPeiZaiType = GridOper.GetRowCellValueString(myGridView1, "SystemType");//配载类型
                //if (strPeiZaiType == "ZQTMS" && ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    list.Add(new SqlPara("ID", ds.Tables[0].Rows[0]["ID"].ToString()));
                //    CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_DELETE_BILLVEHICLESTAR", "", DepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（取消发车）                        
                //}
                //取消好多车司机打卡
                string strToken = GridOper.GetRowCellValueString(myGridView1, "token");
                CommonSyn.CancleHaoDuoCheSendDaKa(DepartureBatch, strToken);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("请选择一条信息！");
            //    return;
            //}
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "装卸费-大车";
            frm.ShowDialog();
        }

        //查看好多车车辆历史轨迹
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string ContractNO = string.Empty;
            DataRow dr = this.myGridView1.GetFocusedDataRow();
            if (dr == null)
            {
                return;
            }
            ContractNO = dr["ContractNO"].ToString();
            string strToken = dr["token"].ToString();
            CommonSyn.GetHaoDuoCheTrajectory(ContractNO, strToken);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MsgBox.ShowOK("配载金额 = “如果分拨明细表的运单号没有值，为实收运费*实际发车件数/件数；如果分拨明细表的运单号有值，为(结算始发操作费+结算干线费+结算送货费+结算中转费+结算终端操作费+结算装卸费+结算上楼费+结算进仓费)*实际发车件数/件数”" + "\n"
                       + "大车费合计= “干线运费+大车接货费+大车送货费+短驳费+多地卸运费+大车其他费”" + "\n"
                       + "毛利 = “结算干线费 -大车费合计-大车接货费-大车送货费”" + "\n"
                       + "实际装载重量=“实际载重”" + "\n"
                       + "实际装载体积=“实际容积”");

        }
    }
}