using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;
using System.IO;

namespace ZQTMS.UI
{
    public partial class fmArrivalShowDetail : BaseForm
    {
        string _departureBatch;
        int _hitorder;
        DateTime _arriveddate;
        GridColumn gcBespeakContent;
        GridColumn gcIsseleckedMode;
        List<int> editRows = new List<int>();

        public int Hitorder
        {
            get { return _hitorder; }
            set { _hitorder = value; }
        }

        public string DepartureBatch
        {
            get { return _departureBatch; }
            set { _departureBatch = value; }
        }

        public DateTime Arriveddate
        {
            get { return _arriveddate; }
            set { _arriveddate = value; }
        }

        public 
            fmArrivalShowDetail()
        {
            InitializeComponent();
        }

        private void fmArrivalShowDetail_Load(object sender, EventArgs e)
        {
            //if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
            //    simpleButton1.Visible = simpleButton7.Visible = true;
            //else simpleButton4.Visible = simpleButton5.Visible = true;

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            //鸿达到货确认限制套打签收单 2018-3-23 zaj 
            if(CommonClass.UserInfo.companyid=="106" && CommonClass.UserInfo.SiteName != "武汉")  //鸿达武汉线放开打印限制
            {
                ddbtnPrintQSD.Enabled = false;
            }
            //barButtonItem4.Enabled = UserRight.GetRight("121781");
            //barButtonItem10.Enabled = UserRight.GetRight("121782");
            //barSubItem3.Visibility = barButtonItem4.Enabled || barButtonItem10.Enabled ? BarItemVisibility.Always : BarItemVisibility.Never;

            gcBespeakContent = myGridView1.Columns["BespeakContent"];
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");

            
            if (_hitorder == 1)
            {
                //simpleButton1.Visible = simpleButton7.Visible = false;

                getbilnos_inroad();
                this.Text = "在途清单";
            }
            if (_hitorder == 2)
            {
                getbilnos_arrived();
            }
            if (_hitorder == 3)
            {
                getbilnos_inroad();
                this.Text = "在途清单";
                this.simpleButton7.Enabled = false;
                this.simpleButton1.Enabled = false;
                this.barSubItem3.Enabled=false;
            }

            getDepartureInfo();
            if (CommonClass.UserInfo.companyid == "486")  //鸿达武汉线放开打印限制
            {
                ddbtnPrintQSD.Text = "打印托运单";
            }
        }

        /// <summary>
        /// 提取在途明细
        /// </summary>
        private void getbilnos_inroad()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("site", CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_INROAD_FCD", list);
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

        /// <summary>
        /// 提取到货明细
        /// </summary>
        private void getbilnos_arrived()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD", list);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 7
                   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 8)
                {
                    MsgBox.ShowOK("该单已经出库或未到货，无法取消到货！");
                    return;
                }
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("确定取消到货？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("BillNoStr", billnostr));
            list.Add(new SqlPara("man", ""));
            list.Add(new SqlPara("type", 2));

            //提前获取到轨迹信息
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", null));
            lists.Add(new SqlPara("BillNO", billnostr));
            lists.Add(new SqlPara("tracetype", "货物到达"));
            lists.Add(new SqlPara("num", 7));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_UPDATE_BILL_ARRIVED_OK", list)) == 0) return;
            CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "取消到货", "到货清单取消到货操作");

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            if (_hitorder == 3) getbilnos_inroad();
            MsgBox.ShowOK();

            //CommonSyn.ArrivalConfirmCancelSyn(_departureBatch, billnostr, 1);//zaj 2018-4-11 分拨同步
            if (_departureBatch.Substring(0,2) == "KP")//maohui20181108
            {
                CommonSyn.LMSSynZQTMS(list, "取消到货", "USP_SET_UPDATE_BILL_ARRIVED_OK_LMSSynZQTMS");
            }
            CommonSyn.TimeCancelSyn(billnostr, "", "", "USP_SET_UPDATE_BILL_ARRIVED_OK");//时效取消同步 LD 2018-4-27
            CommonSyn.TraceSyn(null, billnostr, 7, "货物到达", 2, null,dss);
            #region 原有的单票取消到货
            /*
            string billno = "";
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                int state = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));
                if (state != 7 && state != 8)
                {
                    MsgBox.ShowOK("该单已经出库或未到货，无法取消到货!\r\nr如果确实需要取消到货,请先取消出库!");
                    return;
                }
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));

                DialogResult dl = XtraMessageBox.Show("单号：" + billno + " 将设置成在途状态,确认吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dl == DialogResult.Yes)
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                        list.Add(new SqlPara("BillNo", billno));
                        list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                        list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_SINGLE", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            if (_hitorder == 1) getbilnos_inroad();
                            if (_hitorder == 2) getbilnos_arrived();
                            MsgBox.ShowOK();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
            }
            */
            #endregion
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                //if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 5
                //   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 6)
                //{
                //    MsgBox.ShowOK("运单未在途，不需要到货！");
                //    return;
                //}
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("确定将选中的单到货？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("BillNoStr", billnostr));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK_SING", list)) == 0) return;

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            if (_hitorder == 3) getbilnos_inroad();
            MsgBox.ShowOK();
            //BillArrivalConfirmSyn(billnostr);
            //CommonSyn.BillArrivalConfirmSyn(DepartureBatch, billnostr, 1);
            if (_departureBatch.Substring(0,2) == "KP")//maohui20181108
            {
                CommonSyn.LMSSynZQTMS(list, "单票到货", "USP_SET_BILLDEPARTURE_ARRIVED_OK_SING_LMSSynZQTMS");
            }
            CommonSyn.TimeOtherUptSyn(billnostr, "", "", "", "", "", CommonClass.UserInfo.WebName, "USP_SET_BILLDEPARTURE_ARRIVED_OK_SING", "");//同步其他修改时效 LD 2018-4-27
            CommonSyn.TraceSyn(null, billnostr, 7, "货物到达", 1, null, null);
            #region 原有的单票到货
            /*
            int state = 0;
            string billno = "";

            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));
                state = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));

                if (state >= 7)
                {
                    XtraMessageBox.Show("单号为：" + billno + "不能设置到货状态,这票货已经是到货的状态!", "系统提示", MessageBoxButtons.OK);
                    return;
                }

                if (XtraMessageBox.Show("单号为：" + billno + "将设置为到货状态，确认吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_STATE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (_hitorder == 1) getbilnos_inroad();
                    if (_hitorder == 2) getbilnos_arrived();
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            */
            #endregion
        }
        //private void BillArrivalConfirmSyn(string billnostr)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNOStr", billnostr));
        //        listQuery.Add(new SqlPara("DepartureBatch", ""));
        //        listQuery.Add(new SqlPara("Type", 1));
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DataRow dr = ds.Tables[0].Rows[0];//取第0行
            ContractNO.EditValue = dr["ContractNO"];
            DepartureBatchs.EditValue = dr["DepartureBatch"];
            CarNO.EditValue = dr["CarNO"];
            CarrNO.EditValue = dr["CarrNO"];
            DriverName.EditValue = dr["DriverName"];
            DriverPhone.EditValue = dr["DriverPhone"];
            LoadWeight.EditValue = dr["LoadWeight"];
            LoadVolume.EditValue = dr["LoadVolume"];
            BeginSite.EditValue = dr["BeginSite"];
            EndSite.EditValue = dr["EndSite"];
            ActWeight.EditValue = dr["ActWeight"];
            ActVolume.EditValue = dr["ActVolume"];
            DepartureDate.EditValue = dr["DepartureDate"];
            ExpArriveDate.EditValue = dr["ExpArriveDate"];
            LoadPeoples.EditValue = dr["LoadPeoples"];
            Creator.EditValue = dr["Creator"];
            LoadingType.EditValue = dr["LoadingType"];
            BoxNO.EditValue = dr["BoxNO"];
            BigCarDescr.EditValue = dr["BigCarDescr"];
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null) return;

            frmAppointmentSend frm = new frmAppointmentSend();
            frm.crrBillNO = dr["BillNO"].ToString();
            frm.ShowDialog();

            //billnostr
            //if (simpleButton2.Text == "预约送货")
            //{
            //    simpleButton2.Text = "保存信息";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = true;
            //    myGridView1.FocusedColumn = gcBespeakContent;
            //}
            //else
            //{
            //    simpleButton2.Text = "预约送货";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = false;
            //    //保存预约送货信息
            //    string billnostr = "", bespeakContentstr = "";
            //    for (int i = 0; i < editRows.Count; i++)
            //    {
            //        billnostr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], "BillNO")) + "@";
            //        bespeakContentstr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], gcBespeakContent)).Replace('@', '_') + "@";
            //    }
            //    editRows.Clear();
            //    if (billnostr == "") return;

            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("BillNoStr", billnostr));
            //    list.Add(new SqlPara("BespeakContentStr", bespeakContentstr));
            //    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_SEND_BESPEAKCONTENT", list)) == 0) return;
            //    CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "预约送货", "到货清单预约送货操作");
            //    MsgBox.ShowOK();
            //}
        }

        private void myGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRows.Contains(e.RowHandle)) return;
            editRows.Add(e.RowHandle);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            ////hj 20180420
            //for (int i = 0; i < myGridView1.RowCount; i++)
            //{
            //    billnoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

            //}
            //DataSet dsCompanyid1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MSG_companyid_1", new List<SqlPara>() { new SqlPara("billnoStr", billnoStr) }));
            //if (dsCompanyid1 == null || dsCompanyid1.Tables.Count == 0 || dsCompanyid1.Tables[0].Rows.Count == 0) return;
            //string msg = "";
            //for (int i = 0; i < dsCompanyid1.Tables[0].Rows.Count; i++)
            //{
            //    if (dsCompanyid1.Tables[0].Rows[i]["forShipper"].ToString() == "" || dsCompanyid1.Tables[0].Rows[i]["forConsignee"].ToString() == "")
            //    {
            //        msg += dsCompanyid1.Tables[0].Rows[i]["BillNo"].ToString() + ",";
            //    }
            //}
            //if (msg != "")
            //{
            //    MsgBox.ShowOK("运单号" + msg + "所属的公司暂时没开通短信到货通知功能！");
            //    return;
            //}
            //hj20180420
            string billno = GridOper.GetRowCellValueString(myGridView1, 0, "BillNO");

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_waybill_companyid", new List<SqlPara>() { new SqlPara("billno", billno) }));
            string companyid1 = ds.Tables[0].Rows[0]["companyid"].ToString();

            sms.nowsendsms_to_shipper(myGridView1, _arriveddate, companyid1);

        }


        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            ////hj 20180420
            //for (int i = 0; i < myGridView1.RowCount; i++)
            //{
            //    billnoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

            //}
            //DataSet dsCompanyid1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MSG_companyid_2", new List<SqlPara>() { new SqlPara("billnoStr", billnoStr) }));
            //if (dsCompanyid1 == null || dsCompanyid1.Tables.Count == 0 || dsCompanyid1.Tables[0].Rows.Count == 0) return;
            //string msg = "";
            //for (int i = 0; i < dsCompanyid1.Tables[0].Rows.Count; i++)
            //{
            //    if (dsCompanyid1.Tables[0].Rows[i]["forShipper"].ToString() == "" || dsCompanyid1.Tables[0].Rows[i]["forConsignee"].ToString() == "")
            //    {
            //        msg += dsCompanyid1.Tables[0].Rows[i]["BillNo"].ToString() + ",";
            //    }
            //}
            //if (msg != "")
            //{
            //    MsgBox.ShowOK("运单号" + msg + "所属的公司暂时没开通短信到货通知功能！");
            //    return;
            //}
            string billno = GridOper.GetRowCellValueString(myGridView1, 0, "BillNO");
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_waybill_companyid", new List<SqlPara>() { new SqlPara("billno", billno) }));
            string companyid1 = ds.Tables[0].Rows[0]["companyid"].ToString();

            sms.nowsendsms(myGridView1, this, "1", companyid1);
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            if (DepartureBatch == "") return;
            string middleSite = "";
            frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView1.GetRowCellValue(i, "TransferSite"));
                if (middleSite == "") continue;
                flag = false;
                for (int j = 0; j < fsp.checkedListBox1.Items.Count; j++)
                {
                    if (ConvertType.ToString(fsp.checkedListBox1.Items[j]) == middleSite) flag = true;
                }
                if (!flag) fsp.checkedListBox1.Items.Add(middleSite);
            }
            if (fsp.ShowDialog() != DialogResult.OK) return;

            if (fsp.printSite == "")
            {
                MsgBox.ShowOK("没有选择打印站点!");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }
            //zaj 2018-1-15 司机运输协议根据公司id来加载
            string transprotocol = CommonClass.UserInfo.Transprotocol == "" ? "司机运输协议" : CommonClass.UserInfo.Transprotocol;
            if (File.Exists(Application.StartupPath + "\\Reports\\" + transprotocol + "per.grf"))//zaj 20180713保存外观的文件
            {
                transprotocol = transprotocol + "per";
            }
            string departList = CommonClass.UserInfo.DepartList == "" ? "配载清单" : CommonClass.UserInfo.DepartList;  //maohui20180315
            if (File.Exists(Application.StartupPath + "\\Reports\\" + departList + "per.grf"))
            {
                departList = departList + "per";
            }
            string loadList = CommonClass.UserInfo.LoadList == "" ? "装车清单" : CommonClass.UserInfo.LoadList;//zaj 装车清单
            if (File.Exists(Application.StartupPath + "\\Reports\\" + loadList + "per.grf"))
            {
                loadList = loadList + "per";
            }
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : "司机运输协议"), ds);
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : transprotocol), ds);
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? loadList : transprotocol), ds, CommonClass.UserInfo.gsqc);

            fpr.ShowDialog();


            //if (string.IsNullOrEmpty(DepartureBatch)) return;
            //frmPrintRuiLang fprl = new frmPrintRuiLang("到货清单", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch) })));
            //fprl.ShowDialog();
        }

        private void barbtnPrintQSD_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    str += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";
            }
            if (CommonClass.UserInfo.companyid == "485")
            {
                PrintQSD(str);
            }
            else
            {
                string name = "";
                //int rowhandle = myGridView1.FocusedRowHandle;
                //if (rowhandle < 0) return;
                //string BillNo = myGridView1.GetRowCellValue(rowhandle, "BillNO").ToString();
                if (CommonClass.UserInfo.BookNote == "")
                {


                    name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                }
                else
                {

                    if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                    {
                        name = "宝虹广州项目部托运单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" &&  CommonClass.UserInfo.WebName == "宝虹武汉东西湖营业部")
                    {
                        name = "宝虹广州总部配送部_签收单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                    {
                        name = "武汉遵义营业部托运单";
                    }

                    else
                    {
                        name = CommonClass.UserInfo.BookNote;
                    }


                   

                }
                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", str) })));

            }
        
        }

        private void barbtnPrintGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
            }

            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //string BillNo = myGridView1.GetRowCellValue(rowhandle, "BillNO").ToString();
            if (CommonClass.UserInfo.companyid == "485")
            {
                PrintQSD(str);
            }
            else
            {
                string name = "";
                if (CommonClass.UserInfo.BookNote == "")
                {


                    name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
                }
                else
                {
                    if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                    {
                        name = "宝虹广州项目部托运单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹武汉东西湖营业部")
                    {
                        name = "宝虹广州总部配送部_签收单";
                    }
                    else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                    {
                        name = "武汉遵义营业部托运单";
                    }
                   else
                    {
                        name = CommonClass.UserInfo.BookNote;
                    }




                }
                frmRuiLangService.Print(name, SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_1", new List<SqlPara> { new SqlPara("BillNoStr", str) })), "");
            }
        }




        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("DepartureBatch", DepartureBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    //fprl = new frmPrintRuiLang("提货单", dt);
            //    //fprl.ShowDialog();
            //}
            //frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            //jl20181127
            if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                || CommonClass.UserInfo.WebName == "杭州操作部"
                || CommonClass.UserInfo.WebName == "杭州操作部1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "宁波操作部"
                || CommonClass.UserInfo.WebName == "宁波操作部1"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                || CommonClass.UserInfo.WebName == "广州操作部"
                || CommonClass.UserInfo.WebName == "广州操作部1"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
            {
                frmRuiLangService.Print("提货单大坪", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
            else
            {
                frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
        }

        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }

        /// <summary>
        /// 直营:单票到货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int state = 0;
            string billno = "";

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    state = ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState"));
                    if (state != 5 && state != 6) continue;//不是在途状态,不能确认到货
                    billno += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
                }
            }

            try
            {
                if (XtraMessageBox.Show("单号为：" + billno + "将设置为到货状态，确认吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("BillNoStr", billno));
                list.Add(new SqlPara("type", 1));//1确认到货,2取消到货

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_UPDATE_BILL_ARRIVED_OK_2", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (_hitorder == 1) getbilnos_inroad();
                    if (_hitorder == 2) getbilnos_arrived();
                    if (_hitorder == 3) getbilnos_inroad();
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 直营:单票取消到货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int state = 0;
            string billno = "";

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    state = ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState"));
                    if (state != 7 && state != 8) continue;//不是到货状态,不能取消到货
                    billno += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
                }
            }

            try
            {
                if (XtraMessageBox.Show("单号为：" + billno + "取消到货，确认吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("BillNoStr", billno));
                list.Add(new SqlPara("type", 2));//1确认到货,2取消到货

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_UPDATE_BILL_ARRIVED_OK_2", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (_hitorder == 1) getbilnos_inroad();
                    if (_hitorder == 2) getbilnos_arrived();
                    if (_hitorder == 3) getbilnos_inroad();
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmArrivalConfirmSingle fdas = new frmArrivalConfirmSingle();
            frmBaseArrivalConfirm fbac = new frmBaseArrivalConfirm();
            if (CommonClass.UserInfo.SiteName == "总部")
            {
                fbac.DepartureBatch = DepartureBatchs.Text.Trim();
                fbac.CarNo = CarNO.Text.Trim();
                fbac.DriverName = DriverName.Text.Trim();
                fbac.DriverPhone = DriverPhone.Text.Trim();
                fbac.BeginSite = BeginSite.Text.Trim();
                fbac.EndSite = EndSite.Text.Trim();
                fbac.ShowDialog();
            }
            else
            {
                fdas.DepartureBatch = DepartureBatchs.Text.Trim();
                fdas.CarNo = CarNO.Text.Trim();
                fdas.DriverName = DriverName.Text.Trim();
                fdas.DriverPhone = DriverPhone.Text.Trim();
                fdas.BeginSite = BeginSite.Text.Trim();
                fdas.EndSite = EndSite.Text.Trim();
                fdas.ShowDialog();
            }
            getDepartureInfo();
            //   freshData();//刷新本车清单

            //string BillNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");
            //if (BillNo == "") return;

            //frmArrivalSelectSite fss = new frmArrivalSelectSite();
            //if (fss.ShowDialog() != DialogResult.OK) return;

            //if (MsgBox.ShowYesNo("确定将选中的运单设置为到货状态？\r\n接收信息如下\r\n站点：" + fss.SiteName + "  网点：" + fss.WebName) != DialogResult.Yes) return;

            //List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("DepartureBatch", DepartureBatch));
            //list.Add(new SqlPara("BillNo", BillNo));
            //list.Add(new SqlPara("SiteName", fss.SiteName));
            //list.Add(new SqlPara("WebName", fss.WebName));
            //list.Add(new SqlPara("AddReason", fss.AddReason));

            //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILL_DEPARTURE_COERCE_ARRIVED_SINGLE", list)) == 0) return;
            //MsgBox.ShowOK("成功到货!");
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string BillNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");
            //if (BillNo == "") return;

            //if (MsgBox.ShowYesNo("确定将选中的运单取消到货？") != DialogResult.Yes) return;

            //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILL_DEPARTURE_COERCE_CANCEL_ARRIVED_SINGLE", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch), new SqlPara("BillNo", BillNo) })) == 0)
            //{
            //    MsgBox.ShowOK("提示：本次执行影响行数为0！");
            //    return;
            //}
            //MsgBox.ShowOK("成功取消到货!");
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            int a = checkEdit1.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

       
    }
}