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
using ZQTMS.Lib;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class fmArrivalConfirm : BaseForm
    {
        public fmArrivalConfirm()
        {
            InitializeComponent();
        }

        private void fmArrivalConfirm_Load(object sender, EventArgs e)
        {
            ////直营:先到车再到货;3pl:到车到货一起
            //if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            //{
            //    simpleButton14.Visible = true;
            //    barButtonItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //}
            //else
            //{
            //    simpleButton3.Visible = true;
            //    barButtonItem5.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            //}
            CommonClass.InsertLog("到货确认");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
            GridOper.CreateStyleFormatCondition(myGridView1, "CarStartState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已到车", Color.Yellow);

            if (CommonClass.UserInfo.SiteName == "总部")
            {
                comboBoxEdit1.Enabled = true;
                CommonClass.SetSite(comboBoxEdit1, true);
                comboBoxEdit1.Text = "全部";
            }
            else
            {
                comboBoxEdit1.Enabled = true;
                CommonClass.SetSite(comboBoxEdit1, true);
                comboBoxEdit1.Text = CommonClass.UserInfo.SiteName;
            }

            BarMagagerOper.SetBarPropertity(bar1, bar2, bar3);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(popupContainerEdit1, true);
            CommonClass.SetSite(popupContainerEdit2, true);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);

            dateEdit3.DateTime = CommonClass.gbdate;
            dateEdit4.DateTime = CommonClass.gedate;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            popupContainerEdit1.Text = "全部";
            popupContainerEdit2.Text = CommonClass.UserInfo.SiteName;
            bsite.Text = "全部";
            esite.Text = CommonClass.UserInfo.SiteName;
            endweb.Text = CommonClass.UserInfo.WebName;
            begDate.DateTime = CommonClass.gbdate;
            endDate.DateTime = CommonClass.gedate;

            getdata();//打开加载在途车辆
            CommonClass.GetServerDate();
            btnFetch_Click(null, null);
        }

        /// <summary>
        /// 提取到货车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string site = CommonClass.UserInfo.SiteName == "广州" ? comboBoxEdit1.Text.Trim() : CommonClass.UserInfo.SiteName;
                string web = CommonClass.UserInfo.SiteName == "广州" ? "全部" : CommonClass.UserInfo.WebName;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", site));
                list.Add(new SqlPara("dt1", bdate.DateTime));
                list.Add(new SqlPara("dt2", edate.DateTime));
                list.Add(new SqlPara("webid", web));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRIVED_BILLDEPARTURE", list);
                myGridControl2.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getdata()
        {
            string proc = "QSP_GET_INROAD_BILLDEPARTURE";
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                proc = "QSP_GET_INROAD_BILLDEPARTURE_3PL";
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", Common.CommonClass.UserInfo.SiteName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
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

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "到货车辆");
        }

        /// <summary>
        /// 取消到货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton14_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string inoneflag = "", vehicleno = "";
            inoneflag = myGridView2.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
            vehicleno = myGridView2.GetRowCellValue(rowhandle, "CarNO").ToString();
            if (MsgBox.ShowYesNo("确定取消到货？") != DialogResult.Yes) return;
            if (GetUnitState(inoneflag,0))
            {
                MsgBox.ShowError("不能取消该车。原因是该车部分运单已经提货或者送货。");
                return;
            }
            if (MsgBox.ShowYesNo("车号：" + vehicleno + " 发车批次：" + inoneflag + "\r\n该车将设置成在途状态，该状态会立即传递给始发站。\n\r确认取消后，本地库存减少。\r\n在途车辆会增加。") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", inoneflag));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

            //提前获取到轨迹信息
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", inoneflag));
            lists.Add(new SqlPara("BillNO", null));
            lists.Add(new SqlPara("tracetype", "货物到达"));
            lists.Add(new SqlPara("num", 7));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL", list);
            if (SqlHelper.ExecteNonQuery(sps) == 0) return;
            myGridView2.DeleteRow(rowhandle);
            MsgBox.ShowOK();
            CommonClass.SetOperLog(inoneflag, "", "", CommonClass.UserInfo.UserName, "到货确认取消到货", "整车取消到货操作");

            //CommonSyn.ArrivalConfirmCancelSyn(inoneflag, "", 0);//zaj 2018-4-11 分拨同步
            if (inoneflag.Substring(0, 2) == "KP")
            {
                CommonSyn.LMSSynZQTMS(list, "到货确认取消到货", "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_LMSSynZQTMS");
            }
            //CommonSyn.TimeCancelSyn("", inoneflag, "", "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL");//时效取消同步 LD 2018-4-27
            //CommonSyn.TraceSyn(inoneflag, null, 7, "货物到达", 2, "货物到达", dss);
            //yzw 配载取消到货
            CommonSyn.BILLDEPARTURE_ARRIVED_CANCEL_SYN(inoneflag);
        }

        private bool GetUnitState(string DepartureBatch,int type)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = null;
                //type:0 到货确认 1：转二级短驳
                if (type == 0)
                {
                    list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                     sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_UNIT_STATE", list);
                }
                if (type == 1)
                {
                    list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                     sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendGoods_ARRIVED_UNIT_STATE", list);
                }
                ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return true;

                return ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return true;
        }

        /// <summary>
        /// 查看到货清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            showdetail(myGridView2, 2);
        }

        /// <summary>
        /// 查询所有在途车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit3.DateTime));
                list.Add(new SqlPara("t2", dateEdit4.DateTime));
                list.Add(new SqlPara("bsite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("esite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("WebName", webName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_INROAD_BILLDEPARTURE_ALL", list);
                myGridControl3.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView3.RowCount < 1000) myGridView3.BestFitColumns();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "所有在途车辆");
        }

        private void showdetail(MyGridView gv, int hitoreder)
        {
            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                fmArrivalShowDetail wasd = new fmArrivalShowDetail();
                wasd.DepartureBatch = GridOper.GetRowCellValueString(gv, "DepartureBatch");

                wasd.Hitorder = hitoreder;
                wasd.Arriveddate = gv.GetRowCellValue(rowhandle, "ArrivedDate") == DBNull.Value ? DateTime.Now : Convert.ToDateTime(gv.GetRowCellValue(rowhandle, "ArrivedDate"));
                wasd.ShowDialog();
            }
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
     
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            showdetail(myGridView3, 1);
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView1, 1);
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView3, 3);
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView2, 2);
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

        //到货短驳列表提取
        private void btnFetch_Click(object sender, EventArgs e)
        {
            string proc = "QSP_GET_SENDSHORTCONN_CONFIRM";
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            {
                proc = "QSP_GET_SENDSHORTCONN_CONFIRM_3PL";
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("t1", bdate.DateTime));
                //list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("DestinationSite", ""));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                myGridControl4.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnConfrm_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView4.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string sendBatch = myGridView4.GetRowCellValue(rowhandle, "SendBatch").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", myGridView4.GetRowCellValue(rowhandle, "SendBatch").ToString()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_SENDSHORTCONN_CONFIRM_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView4.DeleteRow(rowhandle);
                    myGridView4.PostEditor();
                    myGridView4.UpdateCurrentRow();
                    myGridView4.UpdateSummary();
                    DataTable dt = myGridControl4.DataSource as DataTable;
                    dt.AcceptChanges();

                    CommonSyn.SendToSiteSyn(sendBatch);//zaj 2018-4-10 分拨同步

                    //同步其他修改时效 LD 2018-4-27
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("SendBatch", sendBatch));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSENDGOODS_BY_SendBatch", list1);
                    DataSet ds_ture = SqlHelper.GetDataSet(sps1);
                    if (ds_ture != null && ds_ture.Tables.Count > 0 && ds_ture.Tables[0].Rows.Count > 0)
                    {
                        string Billnos = string.Empty;
                        foreach (DataRow row in ds_ture.Tables[0].Rows)
                        {
                            Billnos = Billnos + row["BillNO"].ToString() + "@";
                        }
                        //yzw 转二级到货确认
                        CommonSyn.SendToSiteSynNEW(sendBatch);
                        //CommonSyn.TimeOtherUptSyn(Billnos, "", "", "", sendBatch, "", CommonClass.UserInfo.WebName, "USP_SET_SENDSHORTCONN_CONFIRM", ds_ture.Tables[0].Rows[0]["SendToWeb"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //private void SendToSiteSyn(string SendBatch)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //       // listQuery.Add(new SqlPara("BillNOStr", ""));
        //        listQuery.Add(new SqlPara("SendBatch", SendBatch));
        //        //listQuery.Add(new SqlPara("Type", 0));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_SendToSiteSyn", listQuery);
        //        DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
        //        if (dsQuery == null || dsQuery.Tables.Count==0 || dsQuery.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(dsQuery);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //        string url = "http://localhost:42936/KDLMSService/ZQTMSSendToSiteSyn";
        //        //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
        //       // string url = HttpHelper.urlArrivalConfirmSyn;
        //        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //        if (model.State != "200")
        //        {
        //            MsgBox.ShowOK(model.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }

        //}

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView4.FocusedRowHandle;
            if (rowhandle < 0) return;
            //查看明细
            MyGridView gv = (MyGridView)myGridControl4.MainView;
            if (gv.FocusedRowHandle < 0) return;
            frmSendDetailForConfirm ws = new frmSendDetailForConfirm();
            ws.DepartureBatch = GridOper.GetRowCellValueString(gv, "DepartureBatch");//zxw 2016-12-12
            ws.gv = gv;
            ws.ShowDialog();
        }

        private void myGridView4_DoubleClick(object sender, EventArgs e)
        {
            btnGetList_Click(sender, e);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView4, "到货接驳");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            string proc = "QSP_GET_SENDSHORTCONN_CANCEL";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", begDate.DateTime));
                list.Add(new SqlPara("t2", endDate.DateTime));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("WebName", endweb.Text.Trim() == "全部" ? "%%" : endweb.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                myGridControl5.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void esite_TextChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(endweb, esite.Text, true);
            endweb.SelectedIndex = 0;
        }

        /// <summary>
        /// 确认到车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string CarNo = "", DepartureBatch = "";
            CarNo = myGridView1.GetRowCellValue(rowhandle, "CarNO").ToString();
            DepartureBatch = myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();

            string EndSite = myGridView1.GetRowCellValue(rowhandle, "EndSite").ToString();
            if (EndSite.IndexOf(",") > 0)
            {
                MsgBox.ShowOK("多地车请点击查看清单，选中此网点货物，单票点到、不允许整车点到");
                    return;
            }
            if (MsgBox.ShowYesNo(string.Format("该车将设置成到车状态，该状态会立即传递给始发站。\r\n车号:{0}   批次：{1}", CarNo, DepartureBatch)) != DialogResult.Yes) return;
            string departureBatch = myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
            try
            {
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", myGridView1.GetRowCellValue(rowhandle, "DepartureBatch")));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    /*
                    #region //跟踪节点信息同步接口 (到货)
                    {
                        
                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("DepartureBatch", myGridView1.GetRowCellValue(rowhandle, "DepartureBatch")));
                        SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "USP_SET_BILLDEPARTURE_ARRIVED_OK_KT_BillNo", lists);
                        DataSet dds = SqlHelper.GetDataSet(spsa);
                        DataRow ddr = dds.Tables[0].Rows[0];
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("BillNo", ddr["BillNO"]));
                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo1", list1);
                        DataSet ds1 = SqlHelper.GetDataSet(sps1);
                        DataRow dr1 = ds1.Tables[0].Rows[0];

                        if (dr1["BegWeb"].ToString() == "三方")
                        {
                            Dictionary<string, object> hashMap1 = new Dictionary<string, object>();
                            hashMap1.Add("carriageSn", dr1["BillNo"].ToString());
                            hashMap1.Add("orderStatusCode", Convert.ToInt32(2010));
                            hashMap1.Add("traceRemarks", "干线运输中");
                            string json1 = JsonConvert.SerializeObject(hashMap1);
                            string url1 = "http://120.76.141.227:9882/umsv2.biz/open/track/record_trunk_order_status";
                            try
                            {
                                HttpHelper.HttpPostJava(json1, url1);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    #endregion
                     * */
                    CommonClass.SetOperLog(DepartureBatch, "", "", CommonClass.UserInfo.UserName, "到车确认", "在途车辆到车确认操作");
                    myGridView1.DeleteRow(rowhandle);
                    List<SqlPara> listTip = new List<SqlPara>();
                    listTip.Add(new SqlPara("DepartureBatch", departureBatch));
                    SqlParasEntity spsTip = new SqlParasEntity(OperType.Query, "QSP_GET_ACCOUNTBALANCE", listTip);
                    DataSet ds = SqlHelper.GetDataSet(spsTip);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        decimal accountBalance = Convert.ToDecimal(ds.Tables[0].Rows[0]["AccountBalance"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["AccountBalance"].ToString());
                        string accountName = ds.Tables[0].Rows[0]["AccountName"].ToString();
                        if (accountBalance <= 2000 && accountBalance >= 1200)
                        {
                            MsgBox.ShowOK("你的账户【" + accountName + "】,余额已经低于" + accountBalance + "元，请及时充值，以免影响配载！");
                        }
                        //if (accountBalance < 950)
                        //{
                        //    MsgBox.ShowOK("你的账户【" + accountName + "】,余额为：" + accountBalance + "元，不足扣费，请先充值！");
                        //    return;
                        //}
                    }
                    // BillArrivalConfirmSyn(departureBatch);

                    if (departureBatch.Substring(0, 2) == "KP")
                    {
                        //CommonSyn.LMSSynZQTMS(list, "到货确认", "USP_SET_BILLDEPARTURE_ARRIVED_OK_LMSsSynZQTMS");
                    }
                    else
                    {
                        //CommonSyn.BillArrivalConfirmSyn(departureBatch, "", 0);
                    }
                    //CommonSyn.TimeOtherUptSyn("", departureBatch, "", CommonClass.UserInfo.WebName, "", "", CommonClass.UserInfo.WebName, "USP_SET_BILLDEPARTURE_ARRIVED_OK", "");//同步其他修改时效 LD 2018-4-27
                    //CommonSyn.TraceSyn(departureBatch, null, 7, "货物到达", 1, "货物到达", null);
                    //yzw  配载到货同步
                    CommonSyn.BILLDEPARTURE_ARRIVED_OK_SYN(departureBatch);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //private void BillArrivalConfirmSyn(string DepartureBatch)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNOStr", ""));
        //        listQuery.Add(new SqlPara("DepartureBatch", DepartureBatch));
        //        listQuery.Add(new SqlPara("Type",0));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ArrivalConfirmSyn", listQuery);
        //        DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
        //        if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(dsQuery);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //       // string url = "http://localhost:42936/KDLMSService/ZQTMSArrivalConfirmSyn";
        //        //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
        //        string url = HttpHelper.urlArrivalConfirmSyn;
        //        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //        if (model.State != "200")
        //        {
        //            MsgBox.ShowOK(model.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}


        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showdetail(myGridView1, 1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "在途车辆清单");
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnConfrm_Click(sender, null);
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnGetList_Click(sender, null);
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnFetch_Click(sender, null);
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            simpleButton7_Click(sender, null);
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView5.FocusedRowHandle;
            if (rowhandle < 0) return;
            string sendBatch = myGridView5.GetRowCellValue(rowhandle, "SendBatch").ToString();
            if (MsgBox.ShowYesNo("确定取消到货？") != DialogResult.Yes) return;
            #region 限制提货/送货不能取消到货 zb20190627
            if (GetUnitState(sendBatch, 1))
            {
                MsgBox.ShowError("不能取消该车。原因是该车部分运单已经提货或者送货。");
                return;
            }
            #endregion
            try
            {
            
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", myGridView5.GetRowCellValue(rowhandle, "SendBatch").ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_SENDSHORTCONN_CANCEL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView5.DeleteRow(rowhandle);
                    myGridView5.PostEditor();
                    myGridView5.UpdateCurrentRow();
                    myGridView5.UpdateSummary();
                    DataTable dt = myGridControl5.DataSource as DataTable;
                    dt.AcceptChanges();
                    //CommonSyn.SendToSiteConfirmCancelSyn(sendBatch); //zaj 2018-4-12 分拨同步
                    //yzw 新同步
                    CommonSyn.SendToSiteConfirmCancelSynNew(sendBatch);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView5, "接驳确认记录");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView5.FocusedRowHandle;
            if (rowhandle < 0) return;
            //查看明细
            MyGridView gv = (MyGridView)myGridControl5.MainView;
            if (gv.FocusedRowHandle < 0) return;
            frmSendDetailForConfirm ws = new frmSendDetailForConfirm();
            ws.gv = gv;
            ws.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
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

        /// <summary>
        /// 直营:确认到车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string CarNo = "", DepartureBatch = "";
            CarNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "CarNO");
            DepartureBatch = GridOper.GetRowCellValueString(myGridView1, rowhandle, "DepartureBatch");

            if (MsgBox.ShowYesNo(string.Format("该车将设置成到车状态，该状态会立即传递给始发站。\r\n车号:{0}   批次：{1}", CarNo, DepartureBatch)) != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", myGridView1.GetRowCellValue(rowhandle, "DepartureBatch")));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK_2", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    CommonClass.SetOperLog(DepartureBatch, "", "", CommonClass.UserInfo.UserName, "到车确认", "在途车辆到车确认操作");
                    myGridView1.DeleteRow(rowhandle);

                    CommonSyn.TimeOtherUptSyn("", DepartureBatch, "", "", "", "", CommonClass.UserInfo.WebName, "USP_SET_BILLDEPARTURE_ARRIVED_OK_2", "");//同步其他修改时效 LD 2018-4-27
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 直营:取消到车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string inoneflag = "", vehicleno = "";
            inoneflag = myGridView2.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
            vehicleno = myGridView2.GetRowCellValue(rowhandle, "CarNO").ToString();

            if (MsgBox.ShowYesNo("车号：" + vehicleno + " 发车批次：" + inoneflag + "\r\n该车将设置成在途状态，该状态会立即传递给始发站。\n\r确认取消后，本地库存减少。\r\n在途车辆会增加。") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", inoneflag));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_2", list);
            if (SqlHelper.ExecteNonQuery(sps) == 0) return;
            myGridView2.DeleteRow(rowhandle);
            MsgBox.ShowOK();
            CommonClass.SetOperLog(inoneflag, "", "", CommonClass.UserInfo.UserName, "到货确认取消到货", "整车取消到货操作");
        }

        private void simpleButton4_Click_1(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (MsgBox.ShowYesNo("确定完成卸货？") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", GridOper.GetRowCellValueString(myGridView2, rowhandle, "DepartureBatch")));
            if (GridOper.GetRowCellValueString(myGridView2, rowhandle, "DepartureBatch").Substring(0, 2) == "KP")
            {
                CommonSyn.LMSSynZQTMS(list, "完成卸货", "USP_SET_DEPARTURE_DOWN_GOODS_LMSSynZQTMS");
            }
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_DOWN_GOODS", list)) == 0) return;
            MsgBox.ShowOK("卸货成功");

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_SET_DEPARTURE_DOWN_GOODS",list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataRow dr = ds.Tables[0].Rows[0];

            string strBillNO = dr["BillNO"].ToString();
            fun(strBillNO);



        }
        private void fun(string strBillNO)
        {


            #region //跟踪节点信息同步接口 (卸货)
            {
                List<SqlPara> lists = new List<SqlPara>();
                lists.Add(new SqlPara("BillNo", strBillNO));
                SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo2", lists);
                DataTable dt = SqlHelper.GetDataTable(spsa);
                if (dt.Rows[0]["BegWeb"].ToString() == "三方")
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    hashMap.Add("carriageSn", strBillNO);
                    hashMap.Add("orderStatusCode", 2040);
                    hashMap.Add("traceRemarks", "正在卸货");
                    string json = JsonConvert.SerializeObject(hashMap);
                    string url = "http://120.76.141.227:9882/umsv2.biz/open/track/record_trunk_order_status";
                    try
                    {
                        HttpHelper.HttpPostJava(json, url);
                        MessageBox.Show("推送成功");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            #endregion
        }

        //车辆点到
        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //接收本车
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                string types = GridOper.GetRowCellValueString(myGridView1, "Type");//hj 2017-10-31
                string batchNo = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");//zxw 2016-12-26
                #region
                ////yzw 判断是否到过货
                //if (types == "短驳" || types == "转二级")
                //{
                //    List<SqlPara> list5 = new List<SqlPara>();
                //    list5.Add(new SqlPara("SCBatchNo", batchNo));
                //    if (types == "短驳")
                //    {
                //        list5.Add(new SqlPara("type", 0));
                //    }

                //    else if (types == "转二级")
                //    {
                //        list5.Add(new SqlPara("type", 1));
                //    }
                //    SqlParasEntity spe5 = new SqlParasEntity(OperType.Query, "QSP_GET_IsArriveStore", list5);
                //    DataSet ds5 = SqlHelper.GetDataSet(spe5);
                //    if (ds5 != null && ds5.Tables[0].Rows.Count != 0)
                //    {
                //        string isdaohuo = ds5.Tables[0].Rows[0]["isdaohuo"].ToString();
                //        if (isdaohuo == "已到货")
                //        {
                //            MsgBox.ShowOK("该批次已经到过货,无法再做到车(到车即到货)操作!");
                //            return;
                //        }
                //    }
                //}
                #endregion

                if (MsgBox.ShowYesNo("确定点到？") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("ID", GridOper.GetRowCellValueString(myGridView1, rowhandle, "ID")));

                string carNo = GridOper.GetRowCellValueString(myGridView1, "CarNO");
                string arriveSite = GridOper.GetRowCellValueString(myGridView1, "EndSite");
                string arriveWeb = GridOper.GetRowCellValueString(myGridView1, "EndWeb");

                list.Add(new SqlPara("DepartureBatch", batchNo));
                list.Add(new SqlPara("carNo", carNo));
                list.Add(new SqlPara("arriveSite", arriveSite));
                list.Add(new SqlPara("arriveWeb", arriveWeb));
                list.Add(new SqlPara("type", 1));//1表示接收本车,2表示取消接收
                #region
                //list.Add(new SqlPara("types", types));// hj 2017-10-31

                //yzw 到车即到货
                //短驳类型回单确认
                //#region 短驳回单
                //if (types == "短驳")
                //{

                //    List<SqlPara> list7 = new List<SqlPara>();
                //    list7.Add(new SqlPara("SCBatchNo", batchNo));
                //    SqlParasEntity spe7 = new SqlParasEntity(OperType.Query, "USP_GET_ReturnStock_BillNO", list7);
                //    DataSet ds7 = SqlHelper.GetDataSet(spe7);
                //    for (int i = 0; i < ds7.Tables[0].Rows.Count; i++)
                //    {
                //        BillNO += ds7.Tables[0].Rows[i]["BillNO"] + "@";
                //        ReceiptCondition += ds7.Tables[0].Rows[i]["ReceiptCondition"] + "@";
                //    }
                //    frmReturnStockDBCK frm = new frmReturnStockDBCK();
                //    frm.Billno = BillNO;
                //    frm.ReceiptCondition = ReceiptCondition;
                //    frm.type = "短驳接收";
                //    frm.ShowDialog();
                //    BillNO = "";
                //    HDBillno = frm.aa;
                //    bb = frm.bb;
                //    if (bb == "1")
                //    {
                //        return;
                //    }
                //}
                //#endregion

                //#region 配载回单
                //else if (types == "配载")
                //{

                //    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "USP_GET_billDepartureList_MiddleSite", new List<SqlPara>() { new SqlPara("DepartureBatch", batchNo) }));
                //    if (ds.Tables[0].Rows.Count > 1)
                //    {
                //        //下面做
                //    }
                //    else
                //    {
                //        if (MsgBox.ShowYesNo(string.Format("该车将设置成到车状态，该状态会立即传递给始发站。\r\n车号:{0}   批次：{1}", carNo, batchNo)) != DialogResult.Yes) return;
                //        try
                //        {
                //            List<SqlPara> list7 = new List<SqlPara>();
                //            list7.Add(new SqlPara("DepartureBatch", batchNo));
                //            SqlParasEntity spe7 = new SqlParasEntity(OperType.Query, "USP_GET_ReturnStock_BillNO2", list7);
                //            DataSet ds7 = SqlHelper.GetDataSet(spe7);
                //            for (int i = 0; i < ds7.Tables[0].Rows.Count; i++)
                //            {
                //                BillNO += ds7.Tables[0].Rows[i]["BillNO"] + "@";
                //                ReceiptCondition += ds7.Tables[0].Rows[i]["ReceiptCondition"] + "@";
                //            }
                //            frmReturnStockDBCK frm = new frmReturnStockDBCK();
                //            frm.Billno = BillNO;
                //            frm.ReceiptCondition = ReceiptCondition;
                //            frm.type = "干线到货";
                //            frm.ShowDialog();
                //            BillNO = "";//防止再次点击发车时 运单号重复
                //            HDBillno = frm.aa;
                //            bb = frm.bb;
                //            if (bb == "1")
                //            {
                //                return;
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            MsgBox.ShowException(ex);
                //        }

                //    }

                //}
                //#endregion

                //#region 转二级回单
                //else if (types == "转二级")
                //{

                //    try
                //    {
                //        List<SqlPara> list7 = new List<SqlPara>();
                //        list7.Add(new SqlPara("SendBatch", batchNo));
                //        SqlParasEntity spe7 = new SqlParasEntity(OperType.Query, "USP_GET_ReturnStock_BillNO3", list7);
                //        DataSet ds7 = SqlHelper.GetDataSet(spe7);
                //        for (int i = 0; i < ds7.Tables[0].Rows.Count; i++)
                //        {
                //            BillNO += ds7.Tables[0].Rows[i]["BillNO"] + "@";
                //            ReceiptCondition += ds7.Tables[0].Rows[i]["ReceiptCondition"] + "@";
                //        }
                //        frmReturnStockDBCK frm = new frmReturnStockDBCK();
                //        frm.Billno = BillNO;
                //        frm.ReceiptCondition = ReceiptCondition;
                //        frm.type = "转二级到货";
                //        frm.ShowDialog();
                //        BillNO = "";
                //        HDBillno = frm.aa;
                //        bb = frm.bb;
                //        if (bb == "1")
                //        {
                //            return;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MsgBox.ShowException(ex);
                //    }
                //}



                //#endregion
                #endregion

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "Upd_BILLVEHICLESTAR_Arrive_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                     getdata();//加载在途车辆 刷新页面  ZJF20181107
                    if (batchNo.Substring(0, 2) == "KP")//maohui20181107
                    {
                        list.Add(new SqlPara("types", "配载"));
                        //CommonSyn.LMSSynZQTMS(list, "车辆点到", "Upd_BILLVEHICLESTAR_Arrive_LMSSynZQTMS");
                        //yzw
                        
                    }
                    CommonSyn.VEHICLESTAR_Arrive_SYN("1", batchNo, arriveSite, arriveWeb, "配载");
                    MsgBox.ShowOK();
                    #region
                    //myGridView1.DeleteRow(rowhandle);
                    //CommonSyn.TraceSyn(batchNo, null, 0, "车辆到达", 1, types, null);

                    //#region 短驳自动到货
                    //if (types == "短驳")
                    //{
                    //    try
                    //    {
                    //        List<SqlPara> list2 = new List<SqlPara>();
                    //        list2.Add(new SqlPara("SCBatchNo", batchNo));
                    //        list2.Add(new SqlPara("SCAcceptMan", CommonClass.UserInfo.UserName));
                    //        list2.Add(new SqlPara("type", 1));//1表示接收本车
                    //        list2.Add(new SqlPara("HDBillno", HDBillno));
                    //        SqlParasEntity sps2 = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN", list2);
                    //        if (SqlHelper.ExecteNonQuery(sps2) > 0)
                    //        {
                    //            MsgBox.ShowOK();
                    //            myGridView1.DeleteRow(rowhandle);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MsgBox.ShowException(ex);
                    //    }

                    //}
                    //#endregion

                    //#region 配载自动到货

                    //else if (types == "配载")
                    //{
                    //    string endSites = "", destination = "";
                    //    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "USP_GET_billDepartureList_MiddleSite", new List<SqlPara>() { new SqlPara("DepartureBatch", batchNo) }));
                    //    if (ds.Tables[0].Rows.Count > 1)
                    //    {
                    //        frmUpdateEndSite frm = new frmUpdateEndSite();
                    //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //        {
                    //            endSites += ds.Tables[0].Rows[i]["MiddleSite"].ToString() + ",";
                    //            destination = ds.Tables[0].Rows[i]["EndSite"].ToString();//目的地
                    //        }

                    //        frm.gv = myGridView1;
                    //        frm.rowhandle = rowhandle;
                    //        frm.CarNo = carNo;
                    //        frm.endSites = endSites;
                    //        frm.batchNO = batchNo;
                    //        frm._destination = destination;
                    //        //frm.HDBillno = HDBillno;
                    //        frm.ShowDialog();
                    //    }
                    //    else
                    //    {
                    //        try
                    //        {
                    //            List<SqlPara> list3 = new List<SqlPara>();
                    //            list3.Add(new SqlPara("DepartureBatch", batchNo));
                    //            list3.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                    //            list3.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                    //            list3.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                    //            list3.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                    //            list3.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));
                    //            list3.Add(new SqlPara("HDBillno", HDBillno));//2018.4.24WBW

                    //            SqlParasEntity sps3 = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK", list3);
                    //            if (SqlHelper.ExecteNonQuery(sps3) > 0)
                    //            {
                    //                MsgBox.ShowOK();
                    //                CommonClass.SetOperLog(batchNo, "", "", CommonClass.UserInfo.UserName, "到车确认", "在途车辆到车确认操作");
                    //                myGridView1.DeleteRow(rowhandle);
                    //                CommonSyn.BillArrivalConfirmSyn(batchNo, null, 0, 0);
                    //                CommonSyn.TraceSyn(batchNo, null, 7, "货物到达", 1, "货物到达", null);
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            MsgBox.ShowException(ex);
                    //        }

                    //    }

                    //}
                    //#endregion

                    //#region 转二级自动到货
                    //else if (types == "转二级")
                    //{
                    //    try
                    //    {
                    //        List<SqlPara> list4 = new List<SqlPara>();
                    //        list4.Add(new SqlPara("SendBatch", batchNo));
                    //        list4.Add(new SqlPara("HDBillno", HDBillno));
                    //        SqlParasEntity sps4 = new SqlParasEntity(OperType.Execute, "USP_SET_SENDSHORTCONN_CONFIRM", list4);
                    //        if (SqlHelper.ExecteNonQuery(sps4) > 0)
                    //        {
                    //            MsgBox.ShowOK();
                    //            myGridView1.DeleteRow(rowhandle);
                    //            //DataTable dt = myGridControl4.DataSource as DataTable;
                    //            //dt.AcceptChanges();
                    //            //CommonSyn.SendToSiteSyn(batchNo, null, 0);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MsgBox.ShowException(ex);
                    //    }
                    //}
                    //#endregion
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton7_Click_1(object sender, EventArgs e)
        {
            int row = myGridView2.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("请选择一条信息！");
            //    return;
            //}
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView2, "DepartureBatch");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "装卸费-大车到货";
            frm.ShowDialog();
    }

        private void barButtonItem29_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView5.FocusedRowHandle;
            if (row < 0) return;
            //{
            //    MsgBox.ShowError("请选择一条信息！");
            //    return;
            //}
            string DepartureBatch = GridOper.GetRowCellValueString(myGridView5, "DepartureBatch");
            frmHandFeeAdd_KT frm = new frmHandFeeAdd_KT();
            frm.sDepartureBatch = DepartureBatch;
            frm.sFeeType = "装卸费-二级到货";
            frm.ShowDialog();
        }
        //取消车辆点到
        private void barButtonItem30_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            //zb20190809取消注释
            //if (GridOper.GetRowCellValueString(myGridView1, "CarStartState") == "在途中")
            //{
            //    MsgBox.ShowOK("未车辆点到不能取消点到！");
            //    return;
            //}

            if (MsgBox.ShowYesNo("确定取消点到？") != DialogResult.Yes) return;

            string batchNo = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            string carNo = GridOper.GetRowCellValueString(myGridView1, "CarNO");
            string arriveSite = GridOper.GetRowCellValueString(myGridView1, "EndSite");
            string arriveWeb = GridOper.GetRowCellValueString(myGridView1, "EndWeb");

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", batchNo));
            list.Add(new SqlPara("carNo", carNo));
            list.Add(new SqlPara("type", 2));//1表示接收本车,2表示取消接收
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "Upd_BILLVEHICLESTAR_Arrive_KT", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                getdata();//加载在途车辆 刷新页面  ZJF20181107
                MsgBox.ShowOK();
                //yzw 取消到车同步
                CommonSyn.VEHICLESTAR_Arrive_SYN("2", batchNo, arriveSite, arriveWeb, "配载");
            }
        }
    }
}