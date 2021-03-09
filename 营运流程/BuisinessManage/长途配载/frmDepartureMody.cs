using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using System.IO;
using System.Text.RegularExpressions;
using ZQTMS.UI.其他费用;

namespace ZQTMS.UI
{
    public partial class frmDepartureMody : BaseForm
    {
        public frmDepartureMody()
        {
            InitializeComponent();
        }
        public decimal AccPZ;
        public string sDepartureBatch = "";
        public object _arriveDate = null;
        private int state = 0;//为1时不去获取接货单号
        GridColumn gcTransferMode;
        private bool isModify = false;
        public string DepartureBatch1;
        public string strPeiZaiType = string.Empty;//配载类型
        public int changeNum = 0;
        int flag = 0;
        DataRow dr = null;
        public decimal costrate = 0;
        //string Company = "";//hj 油卡所属公司

        /// <summary>
        /// 表示是否修改
        /// </summary>
        public bool IsModify
        {
            get { return isModify; }
        }

        /// <summary>
        /// 编辑中转地的行
        /// </summary>
        List<int> editRowTransferSite = new List<int>(); 

        GridColumn gcTransferSite;// 中转地列
        public string strCompanyId = string.Empty;
        public string strCompanyName = string.Empty;
        public string strToken = string.Empty;

        private void frmDepartureMody_Load(object sender, EventArgs e)
        {
            CommonClass.SetProvince(oilCardProvince);
            CommonClass.SetProvince(nowPayProvince);
            CommonClass.SetProvince(bankPayProvince);
            btnFetchData.Enabled = UserRight.GetRight("121581");//长途
            simpleButton15.Enabled = UserRight.GetRight("121582");//整车
            simpleButton16.Enabled = UserRight.GetRight("121583");//项目
            simpleButton1.Enabled = UserRight.GetRight("121586");//审核
            simpleButton4.Enabled = UserRight.GetRight("121587");//反审核

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4);

            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            gcTransferMode = GridOper.GetGridViewColumn(myGridView2, "TransferMode");

            gcTransferSite = GridOper.GetGridViewColumn(myGridView2, "TransferSite");
            RepositoryItemComboBox ricb = RepItemComboBox.CreateRepItemComboBox;
            gcTransferSite.ColumnEdit = ricb;
            CommonClass.SetSite(ricb, false);

            CommonClass.SetSite(UnloadAddr);
            CommonClass.SetSite(edesite1, false);
            CommonClass.SetSite(edesite2, false);
            CommonClass.SetSite(edesite3, false);

            if (edesite1.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite1.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            if (edesite2.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite2.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            if (edesite3.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite3.Properties.Items.Remove(CommonClass.UserInfo.SiteName);

            CommonClass.SetWeb(ShtBargeDept, CommonClass.UserInfo.SiteName, false);
            CommonClass.SetWeb(TakeDept, CommonClass.UserInfo.SiteName, false);

            GridOper.CreateStyleFormatCondition(myGridView2, "NumEqually", FormatConditionEnum.Equal, "否", Color.Green);
            RepositoryItemComboBox rep = RepItemImageComboBox.CreateRepItemComboBox(myGridView2, "TransferMode");
            string[] CustomTagModeList = CommonClass.Arg.TransferMode.Split(',');
            if (CustomTagModeList.Length > 0)
            {
                for (int i = 0; i < CustomTagModeList.Length; i++)
                {
                    rep.Items.Add(CustomTagModeList[i]);
                }
            }
            //根据发车批次号  取得本次列车发车清单
            freshData();
            getDepartureInfo();
            //设置信息费和保费为100；luohui
            if (AccPremium.Text.Trim() == ""||AccPremium.Text.Trim() == "0")
            {
                //AccPremium.EditValue = "100";
          

            }
            if (AccTakeCar.Text.Trim() == ""||AccTakeCar.Text.Trim() == "0")
            {
                //AccTakeCar.EditValue = "100";
            }
           
         
            getDeliCode();
           
            string[] VehicleTypes = CommonClass.Arg.VehicleType.Split(',');
            for (int i = 0; i < VehicleTypes.Length; i++)
            {
                CarType.Properties.Items.Add(VehicleTypes[i]);
            }
            //if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL) 
            //{
            //    AccTakeCar.Text = "";
            //    AccCollectPremium.Text = "";
            //}

            //hj20180707
            try
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OilCard" ));
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }

            if (strPeiZaiType == "ZQTMS") this.label152.Visible = true;
           
            

        }
        //获取派车单号luohui
        public void getDeliCode()
        {
            string carNO = CarNO.Text.Trim();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("carNo", carNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DeliCode_DepartMody", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            myGridControl5.DataSource = ds.Tables[0];
        }

        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_DEPARTUREBATCH", list);
               // SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_DepartureBatch_Test", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
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

        /// <summary>
        /// 获取
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0) return;
            if (ds.Tables[0].Rows.Count > 0)
            {
                 dr = ds.Tables[0].Rows[0];//取第0行
                ContractNO.EditValue = dr["ContractNO"]; labContractNO.Text = ContractNO.Text;
                DepartureBatch.EditValue = dr["DepartureBatch"]; labDepartureBatch.Text = DepartureBatch.Text;
                CarNO.EditValue = dr["CarNO"]; labCarNO.Text = dr["CarNO"].ToString();
                CarrNO.EditValue = dr["CarrNO"];
                DriverName.EditValue = dr["DriverName"]; labDriverName.Text = DriverName.Text;
                DriverPhone.EditValue = dr["DriverPhone"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolume.EditValue = dr["LoadVolume"];
                BeginSite.EditValue = dr["BeginSite"];
                EndSite.EditValue = dr["EndSite"];
                EndWeb.EditValue = dr["EndWeb"];//新增目的网点 hj20180417
                ActWeight.EditValue = dr["ActWeight"];
                ActVolume.EditValue = dr["ActVolume"];
                DepartureDate.EditValue = dr["DepartureDate"]; labDepartureDate.Text = DepartureDate.Text;
                ExpArriveDate.EditValue = dr["ExpArriveDate"];
                LoadPeoples.EditValue = dr["LoadPeoples"]; labLoadPeoples.Text = LoadPeoples.Text;
                Creator.EditValue = dr["Creator"]; lbcreateby.Text = Creator.Text;
                LoadingType.EditValue = dr["LoadingType"];
                CarType.EditValue = dr["CarType"];
                CarLength.EditValue = dr["CarLength"];
                CarWidth.EditValue = dr["CarWidth"];
                CarHeight.EditValue = dr["CarHeight"];
                NetWeight.EditValue = dr["NetWeight"];
                EndWebName.EditValue = dr["EndWebName"];
                BoxNO.EditValue = dr["BoxNO"];
                BigCarDescr.EditValue = dr["BigCarDescr"];
                AccLine.EditValue = dr["AccLine"];
                FecthSite.EditValue = dr["FecthSite"];
                AccBigCarFecth.EditValue = dr["AccBigCarFecth"];
                FecthBillNO.EditValue = dr["FecthBillNO"];
                AccBigCarSend.EditValue = dr["AccBigCarSend"];
                SendBillNO.EditValue = dr["SendBillNO"];
                SendAddr.EditValue = dr["SendAddr"];
                AccShtBarge.EditValue = dr["AccShtBarge"];
                ShtBargeDept.EditValue = dr["ShtBargeDept"];
                AccSomeLoad.EditValue = dr["AccSomeLoad"];
                UnloadAddr.EditValue = dr["UnloadAddr"];
                AccBigcarOther.EditValue = dr["AccBigcarOther"];
                TakeDept.EditValue = dr["TakeDept"];
                OilCardDeposit.EditValue = dr["OilCardDeposit"];
                DriverIDCardNo.EditValue = dr["DriverIDCardNo"];   //plh12
                //DriverTakePay.EditValue = dr["DriverTakePay"];
                //luohui 
                if (ds.Tables[3] != null && ds.Tables[3].Rows.Count != 0)
                {
                    if (ConvertType.ToString(dr["DriverTakePay"]) == "0.00")
                    {
                        DriverTakePay.EditValue = ds.Tables[3].Rows[0]["DriverTakePay"];
                    }
                    else
                    {
                        DriverTakePay.EditValue = dr["DriverTakePay"];
                    }
                }
                else
                {
                    DriverTakePay.EditValue = dr["DriverTakePay"];
                }
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccPremium.EditValue = dr["AccPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                ToPayDriver.EditValue = dr["ToPayDriver"];
                AccBigcarTotal.EditValue = dr["AccBigcarTotal"];//大车费合计
                AccTakeCar.EditValue = dr["AccTakeCar"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                CarSoure.EditValue = dr["CarSoure"];
                edrepremark.EditValue = dr["BigCarDescr"];
                OilCardNo.EditValue = dr["OilCardNo"];
                OilCardFee.EditValue = dr["OilCardFee"];
                _arriveDate = dr["ArrivedDate"];

                //if (LoadingType.Text == "长途配载")
                //{
                //    btnFetchData.Enabled = true;
                //    simpleButton15.Enabled = simpleButton16.Enabled = false;
                //}
                //else if (LoadingType.Text == "整车配载")
                //{
                //    simpleButton15.Enabled = true;
                //    btnFetchData.Enabled = simpleButton16.Enabled = false;
                //}
                //else//中强项目
                //{
                //    simpleButton16.Enabled = true;
                //    btnFetchData.Enabled = simpleButton15.Enabled = false;
                //}
                BackPayDriver.EditValue = dr["BackPayDriver"];
                OilFee.EditValue = dr["OilFee"];
                oilVolume.EditValue = dr["oilVolume"];
                oilPrice.EditValue = dr["oilPrice"];

                oilCardAccount.EditValue = dr["oilCardAccount"];
                oilCardBank.EditValue = dr["oilCardBank"];
                oilCardManName.EditValue = dr["oilCardManName"];
                oilCardProvince.EditValue = dr["oilCardProvince"];
                oilCardCity.EditValue = dr["oilCardCity"];
                nowPayProvince.EditValue = dr["nowPayProvince"];
                nowPayCity.EditValue = dr["nowPayCity"];
                bankPayProvince.EditValue = dr["bankPayProvince"];
                bankPayCity.EditValue = dr["bankPayCity"];
                txtnpAccountName.EditValue = dr["NowPayAccontName"];
                txtnpBank.EditValue = dr["NowPayBankName"];
                txtnpAccountNO.EditValue = dr["NowPayAccountNO"];
                txtbPAccountName.EditValue = dr["BackPayAccontName"];
                txtbPBank.EditValue = dr["BackPayBankName"];
                txtbpAccountNO.EditValue = dr["BackPayAccountNO"];
                txtnpAccountName.EditValue = dr["NowPayAccontName"];
                txtnpBank.EditValue = dr["NowPayBankName"];
                txtnpAccountNO.EditValue = dr["NowPayAccountNO"];
                txtbPAccountName.EditValue = dr["BackPayAccontName"];
                txtbPBank.EditValue = dr["BackPayBankName"];
                txtbpAccountNO.EditValue = dr["BackPayAccountNO"];


                #region -----汽运增减款登记运费信息加载   hs 2018-11-13-----
                _AccLine.EditValue = dr["AccLine"];
                _FecthSite.EditValue = dr["FecthSite"];
                _AccBigCarFecth.EditValue = dr["AccBigCarFecth"];
                _FecthBillNO.EditValue = dr["FecthBillNO"];
                _AccBigCarSend.EditValue = dr["AccBigCarSend"];
                //_SendBillNO.EditValue = dr["SendBillNO"];
                _SendBillNO.EditValue = SendBillNO.EditValue;//毛慧20170928
                _SendAddr.EditValue = dr["SendAddr"];
                _AccShtBarge.EditValue = dr["AccShtBarge"];
                _ShtBargeDept.EditValue = dr["ShtBargeDept"];
                _AccSomeLoad.EditValue = dr["AccSomeLoad"];
                _UnloadAddr.EditValue = dr["UnloadAddr"];
                _AccBigcarOther.EditValue = dr["AccBigcarOther"];
                _TakeDept.EditValue = dr["TakeDept"];
                _AccBigcarTotal.EditValue = dr["AccBigcarTotal"];
                _BigcarOtherRemark.EditValue = dr["BigcarOtherRemark"];
                _DriverTakePay.EditValue = dr["DriverTakePay"];
                _AccTakeCar.EditValue = dr["AccTakeCar"];
                _OilCardFee.EditValue = dr["OilCardFee"];
                _AccCollectPremium.EditValue = dr["AccCollectPremium"];
                _OilCardNo.EditValue = dr["OilCardNo"];
                _OilFee.EditValue = dr["OilFee"];
                _oilVolume.EditValue = dr["oilVolume"];
                _oilPrice.EditValue = dr["oilPrice"];
                _NowPayDriver.EditValue = dr["NowPayDriver"];
                _ToPayDriver.EditValue = dr["ToPayDriver"];
                _BackPayDriver.EditValue = dr["BackPayDriver"];
                #endregion
                try
                {
                    int isover = ConvertType.ToInt32(dr["isover"]);
                    SetButtonState(isover == 0);
                }
                catch { }

                if ((string.IsNullOrEmpty(dr["VerifyMan"].ToString()) && string.IsNullOrEmpty(dr["VerifyDate"].ToString())) || dr["LoadingType"].ToString() == "整车配载")
                {
                    #region 大车合同未审核过的就做不了大车增减款
                    txtLineFee.Enabled = false;
                    txtFetchFee.Enabled = false;
                    txtSendFee.Enabled = false;
                    txtLandingFee.Enabled = false;
                    txtOverWeightFee.Enabled = false;
                    txtPressNight.Enabled = false;
                    txtDeclare.Enabled = false;
                    txtOtherFee.Enabled = false;
                    txtDriverPay.Enabled = false;
                    txtCarSumFee.Enabled = false;
                    txtSumZjFee.Enabled = false;
                    txtFecthSite.Enabled = false;
                    ccbFetchNo.Enabled = false;
                    ccbSendGoodNo.Enabled = false;
                    txtSendSite.Enabled = false;
                    ccbLandingSite.Enabled = false;
                    ccbOverWeight.Enabled = false;
                    txtMark.Enabled = false;
                    txtDeclareNo.Enabled = false;
                    ZJBearDep.Enabled = false;
                    txtOtherFeeMark.Enabled = false;
                    ccbSendNo.Enabled = false;
                    #endregion

                }
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[2].Rows[0];//取第0行
                txtLineFee.EditValue = dr["ZJLineFee"];
                txtFetchFee.EditValue = dr["ZJBigCarFecthFee"];
                txtSendFee.EditValue = dr["ZJBigCarSendFee"];
                txtLandingFee.EditValue = dr["ZJLandingFee"];
                txtOverWeightFee.EditValue = dr["ZJOverWeightFee"];
                txtPressNight.EditValue = dr["ZJPressNightFee"];
                txtDeclare.EditValue = dr["ZJDeclareFee"];
                txtOtherFee.EditValue = dr["ZJOtherFee"];
                txtDriverPay.EditValue = dr["ZJDriverTakePay"];
                txtCarSumFee.EditValue = dr["ZJSumCarFee"];
                txtSumZjFee.EditValue = dr["ZJSumFee"];
                txtFecthSite.EditValue = dr["ZJDeliverySite"];
                ccbFetchNo.EditValue = dr["ZJDeliveryNo"];
                //ccbSendGoodNo.EditValue = dr["ZJSendGoodsNo"];
                txtSendSite.EditValue = dr["ZJSendGoodsSite"];
                ccbLandingSite.EditValue = dr["ZJLandingSite"];
                ccbOverWeight.EditValue = dr["ZJOverWeightNum"];
                txtMark.EditValue = dr["ZJMark"];
                txtDeclareNo.EditValue = dr["ZJDeclareBillNo"];
                ZJBearDep.EditValue = dr["ZJBearDep"];
                txtOtherFeeMark.EditValue = dr["ZJOtherFeeMark"];
                ccbSendNo.EditValue = dr["ZJSendGoodsFeeNo"];
            }
            else
            {
                flag = 1;
            }
           
            //取到付驾驶员
            if (ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0) return;
            try
            {
                edesite1.EditValue = ds.Tables[1].Rows[0][0];
                edesiteacc1.EditValue = ds.Tables[1].Rows[0][1];
                edesite2.EditValue = ds.Tables[1].Rows[1][0];
                edesiteacc2.EditValue = ds.Tables[1].Rows[1][1];
                edesite3.EditValue = ds.Tables[1].Rows[2][0];
                edesiteacc3.EditValue = ds.Tables[1].Rows[2][1];

            }
            catch { }

            if (myGridControl2.DataSource == null) return;
            DataTable dt = myGridControl2.DataSource as DataTable;
            object obj = dt.Compute("sum(FetchPay)", "TransferMode='司机直送' and PaymentMode='提付'");
            decimal FetchPay = obj == null || obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
            decimal driverTakePay = ConvertType.ToDecimal(DriverTakePay.EditValue);
            if (driverTakePay == 0 && ConvertType.ToDecimal(ds.Tables[0].Rows[0]["BackPayDriver"]) == 0)
            {
                DriverTakePay.EditValue = FetchPay;
            }
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            getdata(1);
        }

        private void getdata(int type)
        {
            //取得库存   121588:打印后加货权限
            if (!UserRight.GetRight("121588") && HtIsPrint(sDepartureBatch))
            {
                MsgBox.ShowOK("本车已打印，不能加货，如需加货请联系相关人员授权!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadType", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private bool HtIsPrint(string sDepartureBatch)
        {
            //判断发车合同是否已经打印
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;

                if (ConvertType.ToString(ds.Tables[0].Rows[0][0]) != "" && ConvertType.ToString(ds.Tables[0].Rows[0][1]) != "" && ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //加入库存
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                XtraMessageBox.Show("请单击选择要加入的运单!\r\n也可以按住Ctrl键,然后在要加入的运单上单击鼠标左键以选择多票!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //HJ 20180411
            DataSet ds = null;
            //try
            //{
            //    List<SqlPara> list1 = new List<SqlPara>();
            //    list1.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
            //    ds = SqlHelper.GetDataSet(sps);
            //    if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
            //    {
            //        MsgBox.ShowOK("本车已经打印司机运输协议，不能加入!");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
            string BillNos = "", factqtys = "";
            int a = 0; int b = 0;
            decimal ActualWeight = 0;
            decimal ActualVolume = 0;
            decimal FeeWeight2 = 0;
            decimal FeeVolume2 = 0;
            string ActualWeights = "", ActualVolumes = "";
            for (int i = 0; i < rows.Length; i++)
            {
                BillNos += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNo")) + ",";
                factqtys += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "factqty")) + ",";
                int factqty = ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "factqty"));
                int LeftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "LeftNum"));

                ActualWeight += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualWeight"));
                ActualVolume += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualVolume"));

                FeeWeight2 += ConvertType.ToDecimal( myGridView1.GetRowCellValue(rows[i], "FeeWeight2"));
                FeeVolume2 += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "FeeVolume2"));
            
                ActualWeights += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualWeight")) + ",";
                ActualVolumes += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualVolume")) + ",";
                try
                {
                    string billno = myGridView1.GetRowCellValue(rows[i], "BillNo").ToString();
                    DataSet ds1 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ActualWeight", new List<SqlPara> { new SqlPara("BillNo", billno) }));
                    decimal ActualWeight1 = ConvertType.ToDecimal(ds1.Tables[0].Rows[0]["ActualWeight"]);
                    decimal ActualVolume1 = ConvertType.ToDecimal(ds1.Tables[0].Rows[0]["ActualVolume"]);
                    if (ds1 == null || ds1.Tables.Count == 0) return;

                    int Num = ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "Num"));

                    int leftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "LeftNum"));
                    int factQty = ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "factqty"));

                    if (factQty <= 0)
                    {
                        MsgBox.ShowError("实发件数不能小于0!");
                        myGridView1.SetRowCellValue(rows[i], "factqty", leftNum);
                        return;
                    }
                    if (factQty > leftNum)
                    {
                        MsgBox.ShowError("实发件数不能大于剩余件数!");
                        myGridView1.SetRowCellValue(rows[i], "factqty", leftNum);
                        return;
                    }

                    decimal Value = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualWeight"));
                    decimal feeWeight2 = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "FeeWeight2"));
                    decimal acc1 = feeWeight2 - ActualWeight1;
                    decimal acc2 = acc1 - Value;
                    decimal acc3 = Math.Round(feeWeight2 * factqty / Num, 2);
                    if (Value <= 0 || Value > acc1)
                    {
                        MsgBox.ShowError("实发计费重量不能为0或者大于剩余计费重量!");
                        myGridView1.SetRowCellValue(rows[i], "ActualWeight", acc3);
                        return;
                    }
                    if (LeftNum - factqty != 0 && acc2 <= 0)
                    {
                        MsgBox.ShowError("还有剩余件数,实发计费重量不能大于等于剩余计费重量!");
                        myGridView1.SetRowCellValue(rows[i], "ActualWeight", acc3);
                        return;
                    }
                    if (LeftNum - factqty == 0 && acc2 != 0)
                    {
                        MsgBox.ShowError("剩余件数为0,实发计费重量必须等于剩余计费重量!");
                        myGridView1.SetRowCellValue(rows[i], "ActualWeight", acc1);
                        return;
                    }

                    decimal Value1 = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "ActualVolume"));
                    decimal feeVolume2 = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "FeeVolume2"));
                    decimal acc4 = feeVolume2 - ActualVolume1;
                    decimal acc5 = acc4 - Value1;
                    decimal acc6 = Math.Round(feeVolume2 * factqty / Num, 2);
                    if (Value1 <= 0 || Value1 > acc4)
                    {
                        MsgBox.ShowError("实发计费体积不能为0或者大于剩余计费体积!");
                        myGridView1.SetRowCellValue(rows[i], "ActualVolume", acc6);
                        return;
                    }
                    if (LeftNum - factqty != 0 && acc5 <= 0)
                    {
                        MsgBox.ShowError("还有剩余件数,实发计费体积不能大于等于剩余计费体积!");
                        myGridView1.SetRowCellValue(rows[i], "ActualVolume", acc6);
                        return;
                    }
                    if (LeftNum - factqty == 0 && acc5 != 0)
                    {
                        MsgBox.ShowError("剩余件数为0,实发计费体积必须等于剩余计费体积!");
                        myGridView1.SetRowCellValue(rows[i], "ActualVolume", acc4);
                        return;
                    }
                }



                catch (Exception ex)
                {
                    MsgBox.ShowError(ex.Message);
                }
               
               
            }
            if (BillNos == "") return;
            if (XtraMessageBox.Show("确定将选中的运单加入到本车清单吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            //检查分批配载的运单是否有做改单申请
            if (strPeiZaiType == "ZQTMS")
            {
                string strMessage = CheckBillPeiZaiModifyApply(BillNos);
                if (!string.IsNullOrEmpty(strMessage))
                {
                    MsgBox.ShowOK("以下运单号存在改单申请，还未执行，不能代配载至ZQTMS系统：\n" + strMessage);
                    return;
                }
            }
            //跨系统加入，需提醒目的网点做到货确认，如配载至ZQTMS系统
            DataTable dt2 = this.myGridControl2.DataSource as DataTable;
            string WebState = string.Empty;
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                DataRow[] dr = dt2.Select(" WebState = 1");
                if (dr.Length > 0)
                {
                    WebState = dr[0]["WebState"].ToString();
                }
            }
            if (strPeiZaiType == "ZQTMS" && WebState == "1")
            {
                if (MsgBox.ShowYesNo("当前操作是转配载至ZQTMS系统的批次号，加入后必须通知目的网点做到货确认，是否继续？") == DialogResult.No)
                {
                    return;
                }
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list.Add(new SqlPara("CarNO", labCarNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", labDriverName.Text.Trim()));
                list.Add(new SqlPara("WebDate", labDepartureDate.Text.Trim()));
                list.Add(new SqlPara("BillNOStr", BillNos));
                list.Add(new SqlPara("FactQtyStr", factqtys));

                list.Add(new SqlPara("ActualWeight", ActualWeight));
                list.Add(new SqlPara("ActualVolume", ActualVolume));
                list.Add(new SqlPara("ActualWeights", ActualWeights));
                list.Add(new SqlPara("ActualVolumes", ActualVolumes));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTURELIST", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteSelectedRows();

                    if (strPeiZaiType == "ZQTMS")
                    {
                        CommonSyn.LMSDepartureSysZQTMS(list, 3, "USP_ADD_BILLDEPARTURELIST_LMS,USP_ADD_PACKAGE_FB_TO_ZX_LMS,USP_ADD_WAYBILL_NEW_V3_LMS", BillNos, sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（多票加入）                        
                    }
                    else
                    {
                        //CommonSyn.BillDepartureSyn(BillNos, sDepartureBatch);//zaj 2018-4-18 分拨同步
                        CommonSyn.BillDepartureSynNew(BillNos, sDepartureBatch, factqtys);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            //CommonSyn.TimeDepartUptSyn(BillNos.Replace(",", "@"), sDepartureBatch, "", ds.Tables[0].Rows[0]["LoadWeb"].ToString(), CommonClass.UserInfo.WebName, "USP_ADD_BILLDEPARTURELIST");//同步配载修改时效 LD 2018-4-27
                        }
                        //CommonSyn.TraceSyn(null, BillNos.Replace(",", "@"), 6, "车辆出发", 1, null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                //更新列表
                freshData();
            }
        }

        //检查分批配载的运单是否有做改单申请
        private string CheckBillPeiZaiModifyApply(string strBillNos)
        {
            StringBuilder sb = new StringBuilder("");
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", strBillNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Check_BillApply_BY_BillNos", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sb.Append(row["BillNo"].ToString() + "\n");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return sb.ToString();
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", a);
            }
        }
        //整车作废
        private void cbdeleteall_Click(object sender, EventArgs e)
        {
            //zaj 2018-4-12 去掉限制
            //if (!UserRight.GetRight("121591") && HtIsPrint(sDepartureBatch))
            //{
            //    MsgBox.ShowOK("本车已打印，不能整车作废，如需整车作废，请联系相关人员授权!");
            //    return;
            //}


            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BatchNo", DepartureBatch.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_BY_BatchNo", list1));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowError("大车费已核销，如需整车作废，请先反核销！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            try
            {
                string billNos = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToDate")) != "")
                    {
                        MsgBox.ShowOK("本车已经有到货,不能作废!\r\n如果确实需要作废,请联系本车到站负责人员取消本车到货!");
                        return;
                    }

                    if (CommonClass.QSP_LOCK_1(myGridView2.GetRowCellValue(i, "BillNO").ToString(), myGridView2.GetRowCellValue(i, "BillDate").ToString()))
                    {
                        MsgBox.ShowOK("本车已经有运单被锁账，不能整车作废!");
                        return;
                    }
                    billNos = billNos + myGridView2.GetRowCellValue(i, "BillNO").ToString() + "@";

                }
                #region 如果已发车，需要取消本车，再整车作废 zb20190514
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list1.Add(new SqlPara("Type", "配载"));
                DataSet ds1 = new DataSet();
                ds1 =SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_DEPARTUREBATCH", list1));
               if(ds1.Tables[0].Rows.Count>0)
               {
                            MsgBox.ShowOK("本车已出发，请先取消本车，再操作!");
                            return;
                }
                #endregion
                if (MsgBox.ShowYesNo("确定作废本车？\r\n批次：" + sDepartureBatch) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定作废本车？请三思！！\r\n批次：" + sDepartureBatch) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTURE", list)) == 0) return;
                MsgBox.ShowOK("本车作废成功!");
                cbcancelover.Enabled = true;
                this.Close();

                if (strPeiZaiType == "ZQTMS")
                {
                    //CommonSyn.LMSDepartureSysZQTMS(list, 1, "USP_DELETE_BILLDEPARTURE", billNos, sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（整车作废）
                }
                else
                {
                    //CommonSyn.DepartureDeleteSyn(sDepartureBatch, billNos, 0);//zaj 2018-4-11 分拨同步
                }
                //yzw 同步新
                CommonSyn.CancelVecheil(billNos);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        //单票剔除
        private void cbdeletesingle_Click(object sender, EventArgs e)
        {
            //if (myGridView2.RowCount == 1)
            //{
            //    if (MsgBox.ShowYesNo("本车只剩下一票了,如果剔除当前运单,本车整个车辆也将作废!\r\n\r\n确定要继续吗?") == DialogResult.Yes)
            //    {
            //        cbdeleteall.PerformClick();
            //    }
            //    return;
            //}
            //if (myGridView2.RowCount == 1)
            //{
            //    MsgBox.ShowError("本车只剩下一票了,不能做单票剔除 只能整车作废！");
            //    return;
            //}
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_BY_BatchNo_dan", list1));
                if (ds.Tables[0].Rows.Count == 1)
                {
                    MsgBox.ShowError("本车只剩下一票了,不能做单票剔除 只能整车作废！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            //大车费核销过不能整车作废  除非反核销
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BatchNo", DepartureBatch.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_BY_BatchNo", list1));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowError("大车费已核销，如需整车作废，请先反核销！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }


            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string billno = GridOper.GetRowCellValueString(myGridView2, "BillNO");
                string billdate = GridOper.GetRowCellValueString(myGridView2, "BillDate");
                string departureBatch = ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"));
                if (CommonClass.QSP_LOCK_1(billno, billdate))
                {
                    MsgBox.ShowOK("本运单被锁账，不能剔除!");
                    return;
                }
                //hj 20180411
                //List<SqlPara> list1 = new List<SqlPara>();
                //list1.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
                //DataSet ds = SqlHelper.GetDataSet(sps);
                //if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                //{
                //    MsgBox.ShowOK("本车已经打印司机运输协议，不能单票剔除!");
                //    return;
                //}


                if (ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "ToDate")) != "")
                {
                    MsgBox.ShowOK("本车已经到货,不能剔除!\r\n如果确实需要剔除,请联系本车到站负责人员取消本车到货!");
                    return;
                }

                #region 判断是否已发车
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list1.Add(new SqlPara("Type", "配载"));
                DataSet ds =new DataSet();
               ds =SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query,"QSP_GET_DEPARTURE_BY_DEPARTUREBATCH",list1));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("本车已发车，请先取消发车!");
                    return;
                }
                #endregion

                ////限制只有发车状态才可以做此操作 zb20190520
                //string str = Convert.ToString(myGridView2.GetRowCellValue(rowhandle, "BillState"));
                //if (str !="5" && !str.Equals("6"))
                //{
                //    MsgBox.ShowOK("只有发车或又发状态才可以做此操作!");
                //    return;
                //}
                
                if (MsgBox.ShowYesNo("确定要剔除本票？\r\n单号：" + billno) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定要剔除本票？请三思！！\r\n单号：" + billno) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("WebPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "WebPCS"))));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_SING", list)) == 0) return;
                //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_SING_1", list)) == 0) return;         //whf20190809 USP_DELETE_DEPARTURE_SING 改为 USP_DELETE_DEPARTURE_SING_1

                myGridView2.DeleteRow(rowhandle);
                MsgBox.ShowOK();
                myGridView2.ClearColumnsFilter();

                if (strPeiZaiType == "ZQTMS")
                {
                    CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_DELETE_DEPARTURE_SING_LMS", billno, departureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（单票剔除）
                }
                CommonSyn.DepartureDeleteSyn(departureBatch, billno + "@", 1);//zaj 2018-4-11 分拨同步
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载明细清单");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载库存清单");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            groupControl1.Visible = !groupControl1.Visible;
        }
        //完成本车
        private void simpleButton11_Click(object sender, EventArgs e)
        {
            decimal accBigcarTotal = ConvertType.ToDecimal(AccBigcarTotal.Text);
            if (accBigcarTotal <= 0)
            {
                if (MsgBox.ShowYesNo("合同没保存运费，确定完成本车？") != DialogResult.Yes) return;
            }
            if (!SetBillDepartureState(1)) return;
            isModify = true;
            SetButtonState(false);
            if (!HtIsPrint(DepartureBatch.Text.Trim())) printcontract();
        }

        /// <summary>
        /// 保存大车状态
        /// </summary>
        /// <param name="state">0未完成,1已完成</param>
        private bool SetBillDepartureState(int state)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            list.Add(new SqlPara("state", state));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_STATE", list)) == 0) return false;

            if (strPeiZaiType == "ZQTMS")
            {
                CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_SET_BILLDEPARTURE_STATE", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（修改本车状态）                        
            }
            MsgBox.ShowOK();
            return true;
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="state">控件启用状态</param>
        private void SetButtonState(bool state)
        {
            simpleButton12.Enabled = simpleButton18.Enabled = simpleButton11.Enabled = state;
            cbcancelover.Enabled = !state;

            btnAdd.Enabled = state && UserRight.GetRight("121584");//加货
            simpleButton17.Enabled = state && UserRight.GetRight("121585");//强制装货
            cbdeletesingle.Enabled = state && UserRight.GetRight("121586");//单票剔除
            cbdeleteall.Enabled = state && UserRight.GetRight("ZQTMS.UI.frmDeparture#barBtnVehicleDel");//整车作废
            //simpleButton3.Enabled = state && UserRight.GetRight("121594");//保存合同
            simpleButton19.Enabled = state && UserRight.GetRight("121594");//保存合同
        }

        private void cbcancelover_Click(object sender, EventArgs e)
        {
            if (IsDispatch(DepartureBatch.Text.Trim().ToString()))
            {
                MsgBox.ShowError("该批次已经做过发车，请取消发车后再取消完成！");
                return;
            }//限制只有取消发车才能取消本车
            if (!SetBillDepartureState(0)) return;
            isModify = true;
            SetButtonState(true);
        }
        public bool IsDispatch(string batch)
        {

            bool ischeck = false;
            List<SqlPara> list_check = new List<SqlPara>();
            list_check.Add(new SqlPara("DepartureBatch", batch));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "GSP_Check_BILLVEHICLESTAR_Merge", list_check);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // MsgBox.ShowOK("该批次已经做过点击发车，不可重复操作！");
                ischeck = true;
            }
            return ischeck;


        }
        //保存合同
        private void simpleButton3_Click(object sender, EventArgs e)
        { 
            //zb20190627 lms4010 判断批次号是否执行过核销
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BatchNo", DepartureBatch.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_VerifyOffAccount_BY_BatchNo", list1));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowError("大车费已核销，如需修改，请先反核销！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }


            if (CommonClass.QSP_LOCK_7(DepartureDate.Text))
            {
                return;
            }
            if (!UserRight.GetRight("121597") && HtIsPrint(sDepartureBatch))
            {
                MsgBox.ShowOK("本车已打印，不能修改合同信息，如需修改，请联系相关人员授权!");
                return;
            }
            //zb20190814 限制车辆点到后，不能修改配载车辆信息
            string carSoure = ConvertType.ToString(CarSoure.Text.Trim());  //车源
            string vehicleno = ConvertType.ToString(CarNO.Text.Trim());  //车牌号
            string carrNO = ConvertType.ToString(CarrNO.Text.Trim());   //车厢号
            string driverName = ConvertType.ToString(DriverName.Text.Trim()); //司机姓名
            string driverPhone =ConvertType.ToString( DriverPhone.Text.Trim());  //司机电话
            DateTime expArriveDate =ConvertType.ToDateTime(ExpArriveDate.Text.Trim()); //预到日期
            string loadPeoples = ConvertType.ToString(LoadPeoples.Text.Trim());  //配载员
            decimal netWeight = ConvertType.ToDecimal(NetWeight.Text.Trim());   //净重
            string carType = ConvertType.ToString(CarType.Text.Trim()); //车型
            decimal carLength = ConvertType.ToDecimal(CarLength.Text.Trim());  //车长
            decimal carWidth = ConvertType.ToDecimal(CarWidth.Text.Trim()); //车宽
            decimal carHeight = ConvertType.ToDecimal(CarHeight.Text.Trim());  //车高
            string boxNO =ConvertType.ToString( BoxNO.Text.Trim()); //封箱编号
            string bigCarDescr = ConvertType.ToString(BigCarDescr.Text.Trim());  //大车备注
            string endSite = ConvertType.ToString(EndSite.Text.Trim());         //目的站点
            string endWeb = ConvertType.ToString(EndWeb.Text.Trim());           //目的网点
            string[] siteTmp = dr["EndSite"].ToString().Split(',');
            string[] endSiteTmp = endSite.Split(',');
            string[] webTmp = dr["EndWeb"].ToString().Split(',');
            string[] endWebTmp = endWeb.Split(',');

            List<SqlPara> lst = new List<SqlPara>();
            lst.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            try
            {
           
            DataSet dset = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", lst));
            string a = dset.Tables[4].Rows[0][0].ToString();
            if (a == "1")
            {
                if (dr["CarSoure"].ToString() != carSoure)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车源信息!");
                    return;
                }
                if (dr["CarNO"].ToString() != vehicleno)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车牌号信息!");
                    return;
                }
                if (dr["CarrNO"].ToString() != carrNO)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车厢信息!");
                    return;
                }
                if (dr["DriverName"].ToString() != driverName)
                {
                    MsgBox.ShowOK("本车已点到，不能修改司机姓名!");
                    return;
                }
                if (dr["DriverPhone"].ToString() != driverPhone)
                {
                    MsgBox.ShowOK("本车已点到，不能修改司机电话!");
                    return;
                }
                if (ConvertType.ToDateTime(dr["ExpArriveDate"]) != expArriveDate)
                {
                    MsgBox.ShowOK("本车已点到，不能修改预到日期!");
                    return;
                }
                if (dr["LoadPeoples"].ToString() != loadPeoples)
                {
                    MsgBox.ShowOK("本车已点到，不能修改配载员!");
                    return;
                }
                if (ConvertType.ToDecimal(dr["NetWeight"]) != netWeight)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车辆净重!");
                    return;
                }
                if (dr["CarType"].ToString() != carType)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车型!");
                    return;
                }
                if (ConvertType.ToDecimal(dr["CarLength"]) != carLength)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车长!");
                    return;
                }
                if (ConvertType.ToDecimal(dr["CarWidth"]) != carWidth)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车宽!");
                    return;
                }
                if (ConvertType.ToDecimal(dr["CarHeight"]) != carHeight)
                {
                    MsgBox.ShowOK("本车已点到，不能修改车高!");
                    return;
                }
                if (dr["BoxNO"].ToString() != boxNO)
                {
                    MsgBox.ShowOK("本车已点到，不能修改封箱编号!");
                    return;
                }
                if (dr["BigCarDescr"].ToString() != bigCarDescr)
                {
                    MsgBox.ShowOK("本车已点到，不能修改大车备注!");
                    return;
                }
                for (int i = 0; i < endSiteTmp.Length;i++ )
                {
                    if (siteTmp[i].ToString() != endSiteTmp[i].ToString())
                    {
                        MsgBox.ShowOK("本车已点到，不能修改修改目的站点!");
                        return;
                    }
                }
                for (int i = 0; i < endWebTmp.Length;i++ )
                {
                    if (webTmp[i].ToString() != endWebTmp[i].ToString())
                    {
                         MsgBox.ShowOK("本车已点到，不能修改修改目的网点!");
                    return;
                    }
                }
            }
            }
            catch (System.Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return;
            }
         


            if (vehicleno == "")
            {
                MsgBox.ShowError("车牌号必须填写!");
                CarNO.Focus();
                return;
            }
            //hj20171213 取消车厢号限制
            //if (CarrNO.Text.Trim() == "")
            //{
            //    MsgBox.ShowError("车厢号必须填写!");
            //    CarNO.Focus();
            //    return;
            //}
            if (CarType.Text.Trim() == "")
            {
                MsgBox.ShowError("车型必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarLength.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车长必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarWidth.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车宽必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarHeight.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车高必须填写!");
                CarNO.Focus();
                return;
            }
            if (CarSoure.Text.Trim() == "")
            {
                MsgBox.ShowError("车源必须填写!");
                CarNO.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("请选择配载类型!");
                LoadingType.Focus();
                return;
            }
            decimal accLine = ConvertType.ToDecimal(AccLine.Text);
            if (accLine == 0)
            {
                MsgBox.ShowError("请填写干线运费!");
                AccLine.Focus();
                return;
            }
            decimal accBigCarFecth = ConvertType.ToDecimal(AccBigCarFecth.Text);
            string fecthBillNO = FecthBillNO.Text.Trim();
            if (accBigCarFecth != 0 && fecthBillNO == "")
            {
                MsgBox.ShowError("您填了大车接货费,必须填写接货单号!");
                FecthBillNO.Focus();
                return;
            }
            decimal accBigCarSend = ConvertType.ToDecimal(AccBigCarSend.Text);
            string sendBillNO = SendBillNO.Text.Trim();
            if (accBigCarSend != 0 && sendBillNO == "")
            {
                MsgBox.ShowError("您填了大车送货费,必须填写送货单号(多个送货单号用‘/’隔开)!");
                SendBillNO.Focus();
                return;
            }
            decimal accShtBarge = ConvertType.ToDecimal(AccShtBarge.Text);
            string shtBargeDept = ShtBargeDept.Text.Trim();
            if (accShtBarge != 0 && shtBargeDept == "")
            {
                MsgBox.ShowError("您填了短驳费,必须填写短驳费的承运部门!");
                ShtBargeDept.Focus();
                return;
            }
            decimal accBigcarOther = ConvertType.ToDecimal(AccBigcarOther.Text);
            string takeDept = TakeDept.Text.Trim();
            if (accBigcarOther != 0 && takeDept == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写大车其他费的承运部门!");
                TakeDept.Focus();
                return;
            }
            string bigcarOtherRemark = BigcarOtherRemark.Text.Trim();
            if (accBigcarOther != 0 && bigcarOtherRemark == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写其他费备注!");
                BigcarOtherRemark.Focus();
                return;
            }
            decimal oilCardFee = ConvertType.ToDecimal(OilCardFee.Text);
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardFee != 0 && oilCardNo == "")
            {
                MsgBox.ShowError("您填了油卡金额,必须填写油卡编号!");
                OilCardNo.Focus();
                return;
            }
            decimal driverTakePay = 0, accTakeCar = 0, accPremium = 0, nowPayDriver = 0, toPayDriver = 0, backPayDriver = 0;
            driverTakePay = ConvertType.ToDecimal(DriverTakePay.Text);//司机代收
            accTakeCar = ConvertType.ToDecimal(AccTakeCar.Text);//代收派车费
            accPremium = ConvertType.ToDecimal(AccPremium.Text);//代收保险费
            nowPayDriver = ConvertType.ToDecimal(NowPayDriver.Text);
            toPayDriver = ConvertType.ToDecimal(ToPayDriver.Text);
            backPayDriver = ConvertType.ToDecimal(BackPayDriver.Text);
            if (ConvertType.ToDecimal(AccBigcarTotal.Text) != (driverTakePay + accTakeCar + accPremium + nowPayDriver + toPayDriver + backPayDriver + oilCardFee))
            {
                MsgBox.ShowError("大车费合计=司机代收款+代收派车费+代收保险费+现付驾驶员+到付驾驶员+回付驾驶员+油卡金额,请检查!");
                return;
            }
            string esite1 = edesite1.Text.Trim();
            decimal eacc1 = esite1 == "" ? 0 : ConvertType.ToDecimal(edesiteacc1.Text);
            if (esite1 == "" && eacc1 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite1));
                edesiteacc1.Focus();
                return;
            }
            string esite2 = edesite2.Text.Trim();
            decimal eacc2 = esite2 == "" ? 0 : ConvertType.ToDecimal(edesiteacc2.Text);
            if (esite2 == "" && eacc2 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite2));
                edesiteacc2.Focus();
                return;
            }
            string esite3 = edesite3.Text.Trim();
            decimal eacc3 = esite3 == "" ? 0 : ConvertType.ToDecimal(edesiteacc3.Text);
            if (esite3 == "" && eacc3 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite3));
                edesiteacc3.Focus();
                return;
            }

            if (myGridControl2.DataSource == null)
            {
                MsgBox.ShowError("发车明细列表没有数据!");
                return;
            }

            string DriverIDCardno = DriverIDCardNo.Text.Trim();
            if (DriverIDCardno == "")
            {
                MsgBox.ShowError("驾驶员身份证必填!");
                DriverIDCardNo.Focus();
                return;
            }  //plh

            if (DriverIDCardno.Length != 18)
            {
                MsgBox.ShowError("驾驶员身份证必须为18位!");
                DriverIDCardNo.Focus();
                return;
            }  //plh

            DataTable dt = myGridControl2.DataSource as DataTable;
            decimal sf = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ConvertType.ToString(dt.Rows[i]["TransferMode"]) == "司机直送" && ConvertType.ToString(dt.Rows[i]["PaymentMode"]) == "提付")
                    sf += ConvertType.ToDecimal(dt.Rows[i]["FetchPay"]);
            }
            //object obj = dt.Compute("sum(FetchPay)", "TransferMode='司机直送' and PaymentMode='提付'");
            //decimal FetchPay = obj == null || obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
            decimal FetchPay = sf;

            if (driverTakePay < FetchPay)
            {
                //string msg = string.Format("本车司机直送的提付合计为{0}\r\n司机代收款不能低于此金额!", FetchPay);
                //MsgBox.ShowOK(msg);
                //return;
                string msg = string.Format("本车司机直送的提付合计为{0}\r\n司机代收款是否低于此金额？", FetchPay);

                if (MsgBox.ShowYesNo(msg) != DialogResult.Yes) return;
            }
            if (dr["OilCardDeposit"].ToString() != this.OilCardDeposit.Text.ToString())
            {


                List<SqlPara> list_check = new List<SqlPara>();
                list_check.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "GSP_Check_BILLVEHICLESTAR_Merge", list_check);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("该批次已经做过点击发车，不可修改油卡押金！");
                    return;
                }

                List<SqlPara> list_check1 = new List<SqlPara>();
                list_check1.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.ToString()));
                DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OilCardDeposit_By_BatchNo_1", list_check1));
                if (dss.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowError("油卡押金已审核，如需修改，请先反审核！");
                    return;
                }

            }

            string oilCardAccountStr = oilCardAccount.Text.Trim().Replace(" ", "");
            string txtnpAccountNOStr = txtnpAccountNO.Text.Trim().Replace(" ", "");
            string txtbpAccountNOStr = txtbpAccountNO.Text.Trim().Replace(" ", "");
            string oilCardManNameStr = oilCardManName.Text.Trim();
            string txtnpAccountNameStr = txtnpAccountName.Text.Trim();
            string txtbPAccountNameStr = txtbPAccountName.Text.Trim();
            Regex r = new Regex(@"^\d{6,50}$");
            Regex r2 = new Regex(@"[\u4e00-\u9fbb]");

            //if (Convert.ToDecimal(OilCardFee.Text.Trim().ToString() == "" ? "0" : OilCardFee.Text.Trim().ToString()) > 0)
            //{
            //    if (!r.IsMatch(oilCardAccountStr))
            //    {
            //        MsgBox.ShowError("请输入正确的油卡账户!");
            //        return;
            //    }
            //    if (!r2.IsMatch(oilCardManNameStr))
            //    {
            //        MsgBox.ShowError("油卡户名只能输汉字");
            //        return;
            //    }
            //}
            //if (Convert.ToDecimal(NowPayDriver.Text.Trim().ToString() == "" ? "0" : NowPayDriver.Text.Trim().ToString()) > 0)
            //{
            //    if (!r.IsMatch(txtnpAccountNOStr))
            //    {
            //        MsgBox.ShowError("请输入正确的现付账户!");
            //        return;
            //    }
            //    if (!r2.IsMatch(txtnpAccountNameStr))
            //    {
            //        MsgBox.ShowError("现付户名只能输汉字");
            //        return;
            //    }
            //}
            //if (Convert.ToDecimal(BackPayDriver.Text.Trim().ToString() == "" ? "0" : BackPayDriver.Text.Trim().ToString()) > 0)
            //{
            //    if (!r.IsMatch(txtbpAccountNOStr))
            //    {
            //        MsgBox.ShowError("请输入正确的回付账户!");
            //        return;
            //    }
            //    if (!r2.IsMatch(txtbPAccountNameStr))
            //    {
            //        MsgBox.ShowError("回付户名只能输汉字");
            //        return;
            //    }
            //}
            //凡是”现付驾驶员“栏录入金额，”现付户名“”现付户名“”现付账号“必须填写，才能点击”保存合同“
            //if (Convert.ToDecimal(NowPayDriver.Text.Trim().ToString() == "" ? "0" : NowPayDriver.Text.Trim().ToString()) > 0)
            //{
            //    if (txtnpAccountName.Text.Trim() == "")
            //    {
            //        MsgBox.ShowOK("已输入现付驾驶员，现付户名不能为空！");
            //        this.txtnpAccountName.Focus();
            //        return;
            //    }
            //    if (txtnpBank.Text.Trim() == "")
            //    {
            //        MsgBox.ShowOK("已输入现付驾驶员，现付开户行不能为空！");
            //        this.txtnpBank.Focus();
            //        return;
            //    }
            //    if (txtnpAccountNO.Text.Trim() == "")
            //    {
            //        MsgBox.ShowOK("已输入现付驾驶员，现付账号不能为空！");
            //        this.txtnpAccountNO.Focus();
            //        return;
            //    }
            //    if (nowPayProvince.Text.Trim() == "")
            //    {
            //        MsgBox.ShowOK("已输入现付驾驶员，现付省份不能为空！");
            //        this.nowPayProvince.Focus();
            //        return;
            //    }
            //    if (nowPayCity.Text.Trim() == "")
            //    {
            //        MsgBox.ShowOK("已输入现付驾驶员，现付城市不能为空！");
            //        this.nowPayCity.Focus();
            //        return;
            //    }
            //}

            if (MsgBox.ShowYesNo("是否保存合同？") != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("BeginSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("EndSite", EndSite.Text.Trim()));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                //预到日期
                if (ExpArriveDate.Text.Trim() == "")
                    list.Add(new SqlPara("ExpArriveDate", DBNull.Value));
                else
                    list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));

                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", LoadPeoples.Text.Trim()));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", BoxNO.Text.Trim()));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                list.Add(new SqlPara("LoadingType", loadingType));
                list.Add(new SqlPara("AccLine", accLine));//干线运费
                list.Add(new SqlPara("FecthSite", FecthSite.Text.Trim()));//接货地
                list.Add(new SqlPara("AccBigCarFecth", accBigCarFecth));//大车接货费
                list.Add(new SqlPara("FecthBillNO", fecthBillNO));
                list.Add(new SqlPara("AccBigCarSend", accBigCarSend));
                list.Add(new SqlPara("SendBillNO", sendBillNO));
                list.Add(new SqlPara("SendAddr", SendAddr.Text.Trim()));
                list.Add(new SqlPara("AccShtBarge", ConvertType.ToDecimal(AccShtBarge.Text)));
                list.Add(new SqlPara("ShtBargeDept", ShtBargeDept.Text.Trim()));
                list.Add(new SqlPara("AccSomeLoad", ConvertType.ToDecimal(AccSomeLoad.Text)));
                list.Add(new SqlPara("UnloadAddr", UnloadAddr.Text));
                list.Add(new SqlPara("AccBigcarOther", accBigcarOther));
                list.Add(new SqlPara("TakeDept", takeDept));
                list.Add(new SqlPara("AccBigcarTotal", ConvertType.ToDecimal(AccBigcarTotal.Text)));
                list.Add(new SqlPara("DriverTakePay", driverTakePay));
                list.Add(new SqlPara("AccTakeCar", accTakeCar));
                list.Add(new SqlPara("AccPremium", accPremium));
                list.Add(new SqlPara("NowPayDriver", nowPayDriver));
                list.Add(new SqlPara("ToPayDriver", toPayDriver));
                list.Add(new SqlPara("BackPayDriver", backPayDriver));
                list.Add(new SqlPara("edesite1", esite1));
                list.Add(new SqlPara("edesiteacc1", eacc1));
                list.Add(new SqlPara("edesite2", esite2));
                list.Add(new SqlPara("edesiteacc2", eacc2));
                list.Add(new SqlPara("edesite3", esite3));
                list.Add(new SqlPara("edesiteacc3", eacc3));
                list.Add(new SqlPara("BigcarOtherRemark", bigcarOtherRemark));
                list.Add(new SqlPara("DeliCode", fecthBillNO));
                list.Add(new SqlPara("OilCardFee", oilCardFee));
                list.Add(new SqlPara("OilCardNo", oilCardNo));
                list.Add(new SqlPara("CarType", CarType.Text.Trim()));
                list.Add(new SqlPara("CarLength", CarLength.Text.Trim() == "" ? "0" : CarLength.Text.Trim()));
                list.Add(new SqlPara("CarWidth", CarWidth.Text.Trim() == "" ? "0" : CarWidth.Text.Trim()));
                list.Add(new SqlPara("CarHeight", CarHeight.Text.Trim() == "" ? "0" : CarHeight.Text.Trim()));
                list.Add(new SqlPara("NetWeight", ConvertType.ToDecimal(NetWeight.Text.Trim() == "" ? "0" : NetWeight.Text.Trim())));
                list.Add(new SqlPara("CarSoure", CarSoure.Text.Trim()));
                list.Add(new SqlPara("oilPrice", ConvertType.ToDecimal(OilFee.Text.Trim() == "" ? "0" : OilFee.Text.Trim())));
                list.Add(new SqlPara("oilId", oilId));
                list.Add(new SqlPara("oilVolume", ConvertType.ToDecimal(oilVolume.Text.Trim() == "" ? "0" : oilVolume.Text.Trim())));
                list.Add(new SqlPara("oilPrice1", ConvertType.ToDecimal(oilPrice.Text.Trim() == "" ? "0" : oilPrice.Text.Trim())));
                list.Add(new SqlPara("EndWeb", EndWeb.Text.Trim()));//新增目的网点 hj20180417

                //hs  2018-11-13
                list.Add(new SqlPara("NowPayAccontName", txtnpAccountName.Text.Trim()));
                list.Add(new SqlPara("NowPayBankName", txtnpBank.Text.Trim()));
                list.Add(new SqlPara("NowPayAccountNO", txtnpAccountNO.Text.Trim()));
                list.Add(new SqlPara("BackPayAccontName", txtbPAccountName.Text.Trim()));
                list.Add(new SqlPara("BackPayBankName", txtbPBank.Text.Trim()));
                list.Add(new SqlPara("BackPayAccountNO", txtbpAccountNO.Text.Trim()));


                list.Add(new SqlPara("oilCardAccount", oilCardAccount.Text.Trim()));
                list.Add(new SqlPara("oilCardBank", oilCardBank.Text.Trim()));
                list.Add(new SqlPara("oilCardManName", oilCardManName.Text.Trim()));
                list.Add(new SqlPara("oilCardProvince", oilCardProvince.Text.Trim()));
                list.Add(new SqlPara("oilCardCity", oilCardCity.Text.Trim()));
                list.Add(new SqlPara("nowPayCity", nowPayCity.Text.Trim()));
                list.Add(new SqlPara("nowPayProvince", nowPayProvince.Text.Trim()));
                list.Add(new SqlPara("bankPayCity", bankPayCity.Text.Trim()));
                list.Add(new SqlPara("bankPayProvince", bankPayProvince.Text.Trim()));
                list.Add(new SqlPara("OilCardDeposit", OilCardDeposit.Text.Trim()));  //gy
                list.Add(new SqlPara("DriverIDCardNo", DriverIDCardNo.Text.Trim()));  //plh

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLDEPARTURE", list)) == 0) return;
                isModify = true;
                MsgBox.ShowOK("合同保存成功!");

                if (strPeiZaiType == "ZQTMS")
                {
                    CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_UPDATE_BILLDEPARTURE", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（合同保存）                        
                }
                if (!HtIsPrint(DepartureBatch.Text.Trim())) printcontract();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void printcontract() 
        {
            //if (MsgBox.ShowYesNo("是否可以确认发车？（打印运输协议）") != DialogResult.Yes) return;
            //if (sDepartureBatch == "") return;
            //string middleSite = "";
            //frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            //bool flag = false;
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
            //    if (middleSite == "") continue;
            //    flag = false;
            //    for (int j = 0; j < fsp.checkedListBox1.Items.Count; j++)
            //    {
            //        if (ConvertType.ToString(fsp.checkedListBox1.Items[j]) == middleSite) flag = true;
            //    }
            //    if (!flag) fsp.checkedListBox1.Items.Add(middleSite);
            //}
            //fsp.radioGroup2.SelectedIndex = 2;
            //if (fsp.ShowDialog() != DialogResult.OK) return;

            //if (fsp.printSite == "")
            //{
            //    MsgBox.ShowOK("没有选择打印站点!");
            //    return;
            //}
            ////同步配载修改时效 LD 2018-5-24
            //string strBillNo = string.Empty;
            //List<SqlPara> lists = new List<SqlPara>();
            //lists.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", lists);
            //DataSet ds_check = SqlHelper.GetDataSet(sps);
            //if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
            //{
            //    foreach (DataRow row in ds_check.Tables[0].Rows)
            //    {
            //        strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
            //    }
            //}

            //DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
            //if (ds == null || ds.Tables.Count == 0) return;
            ////同步配载修改时效 LD 2018-5-24
            //if (!string.IsNullOrEmpty(strBillNo))
            //{
            //    CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
            //}

            //string tmps = "";
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    if (ds.Tables[0].Columns.Contains("NowCompany") && ds.Tables[0].Columns.Contains("NowCompanyStr"))
            //    {
            //        ds.Tables[0].Rows[i]["NowCompany"] = CommonClass.UserInfo.gsqc;
            //        ds.Tables[0].Rows[i]["NowCompanyStr"] = CommonClass.UserInfo.gsqc + "货物运输协议";
            //    }
            //    tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
            //    if (tmps == "") continue;
            //    try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
            //    catch { }
            //}
            ////zaj 2018-1-15 司机运输协议根据公司ID来加载
            //string transprotocol = CommonClass.UserInfo.Transprotocol == "" ? "司机运输协议" : CommonClass.UserInfo.Transprotocol;
            //if (File.Exists(Application.StartupPath + "\\Reports\\" + transprotocol + "per.grf"))//zaj 20180713保存外观的文件
            //{
            //    transprotocol = transprotocol + "per";
            //}
            //string departList = CommonClass.UserInfo.DepartList == "" ? "配载清单" : CommonClass.UserInfo.DepartList;  //maohui20180315
            //if (File.Exists(Application.StartupPath + "\\Reports\\" + departList + "per.grf"))
            //{
            //    departList = departList + "per";
            //}
            //string loadList = CommonClass.UserInfo.LoadList == "" ? "装车清单" : CommonClass.UserInfo.LoadList;//zaj 装车清单
            //if (File.Exists(Application.StartupPath + "\\Reports\\" + loadList + "per.grf"))
            //{
            //    loadList = loadList + "per";
            //}
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? loadList : transprotocol), ds);
            //fpr.ShowDialog();
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || e.Value == null) return;
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            
            try
            {
                //int leftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "LeftNum"));
                //int factQty = ConvertType.ToInt32(e.Value);

                //if (factQty <= 0)
                //{
                //    e.Valid = false;
                //    e.ErrorText = "实发件数必须大于0!";
                //}
                //else if (factQty > leftNum)
                //{
                //    e.Valid = false;
                //    e.ErrorText = "实发件数不能大于剩余件数!";
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void CalAccBigcarTotal(object sender, EventArgs e)
        {
            try
            {
                AccBigcarTotal.Text = (ConvertType.ToDecimal(AccLine.Text) + ConvertType.ToDecimal(AccBigCarFecth.Text) + ConvertType.ToDecimal(AccBigCarSend.Text) + ConvertType.ToDecimal(AccShtBarge.Text) + ConvertType.ToDecimal(AccSomeLoad.Text) + ConvertType.ToDecimal(AccBigcarOther.Text)).ToString();
            }
            catch (Exception ex)
            {
                AccBigcarTotal.Text = "";
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 计算回付驾驶员
        /// </summary>
        private void CalAccBackPayDriver(object sender, EventArgs e)
        {
            //大车费合计-司机代收款-信息费-保费-现付驾驶员-到付驾驶员-油卡金额 --zb20190518
            BackPayDriver.Text = (ConvertType.ToDecimal(AccBigcarTotal.Text) - ConvertType.ToDecimal(DriverTakePay.Text) - ConvertType.ToDecimal(AccTakeCar.Text) - ConvertType.ToDecimal(AccPremium.Text) - ConvertType.ToDecimal(NowPayDriver.Text) - ConvertType.ToDecimal(ToPayDriver.Text) - ConvertType.ToDecimal(OilCardFee.Text)).ToString();
            // _BackPayDriver.Text = (ConvertType.ToDecimal(AccBigcarTotal.Text) - ConvertType.ToDecimal(DriverTakePay.Text) - ConvertType.ToDecimal(AccTakeCar.Text) - ConvertType.ToDecimal(AccCollectPremium.Text) - ConvertType.ToDecimal(NowPayDriver.Text) - ConvertType.ToDecimal(ToPayDriver.Text) - ConvertType.ToDecimal(OilCardFee.Text)).ToString();//计算增减款中回付驾驶员--毛慧20170925
        }

        private void edesiteacc1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite1.Text.Trim() == "")
                e.Cancel = true;
        }

        /// <summary>
        /// 计算到付司机
        /// </summary>
        private void CalToPayDriver(object sender, EventArgs e)
        {
            try
            {
                ToPayDriver.Text = (ConvertType.ToDecimal(edesiteacc1.Text) + ConvertType.ToDecimal(edesiteacc2.Text) + ConvertType.ToDecimal(edesiteacc3.Text)).ToString();
            }
            catch (Exception ex) { ToPayDriver.Text = ""; MsgBox.ShowError(ex.Message); }
        }

        private void frmDepartureMody_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!cbcancelover.Enabled)
                if (MsgBox.ShowYesNo("本车还未完成，是否完成本车？") == DialogResult.Yes)
                { if (SetBillDepartureState(1)) isModify = true; }
            if (!HtIsPrint(DepartureBatch.Text.Trim()))printcontract();
        }

        private void edesiteacc2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite2.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesiteacc3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite3.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesite1_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite1.Text.Trim() == "")
            {
                edesiteacc1.EditValueChanging -= edesiteacc1_EditValueChanging;
                edesiteacc1.Text = "";
                edesiteacc1.EditValueChanging += edesiteacc1_EditValueChanging;
            }
        }

        private void edesite2_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite2.Text.Trim() == "")
            {
                edesiteacc2.EditValueChanging -= edesiteacc2_EditValueChanging;
                edesiteacc2.Text = "";
                edesiteacc2.EditValueChanging += edesiteacc2_EditValueChanging;
            }
        }

        private void edesite3_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite3.Text.Trim() == "")
            {
                edesiteacc3.EditValueChanging -= edesiteacc3_EditValueChanging;
                edesiteacc3.Text = "";
                edesiteacc3.EditValueChanging += edesiteacc3_EditValueChanging;
            }
        }

        private void edesite1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite1.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite1.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //发货人
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());
            sms.fcdsendsms(myGridView2, this, "1", senddate, ExpArriveDate.DateTime);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());

            sms.fcdsendsms_consignee(myGridView2, this, "1", senddate, labCarNO.Text.Trim());
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //货到前付
            if (_arriveDate == null)
            {
                if (XtraMessageBox.Show("本车未到货,确认发送货到前付短信通知?\r\n系统将默认到货时间为当前时间!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            sms.fukuansms(myGridView2, this, "1", _arriveDate == null ? CommonClass.gcdate : Convert.ToDateTime(_arriveDate));
        }


        public bool isDeficit(decimal bigfee, decimal pzfee)
        {
            try
            {
                bool ischeck = false;
                pzfee=(pzfee==0)?1:pzfee;
                costrate = Math.Round(bigfee / pzfee, 2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("StartSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", EndSite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_1", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    if (costrate > Convert.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"].ToString()))
                    {


                        ischeck = true;
                    }


                }
                return ischeck;
            }

            catch (Exception)
            {

                throw;
            }
        }

        //打印清单
        private void cbprint_Click(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.companyid == "485")
            {

                cbprint_485();

            }
            else
            {
                cbprint_486();
            }
        }

        public void cbprint_485()
        {
            if (sDepartureBatch == "") return;
            string middleSite = "";

            if (isDeficit(ConvertType.ToDecimal(this.AccBigcarTotal.Text.ToString().Trim()), AccPZ))
            {
                frmIsCostDeficits Cost = new frmIsCostDeficits();
                Cost.DepartureBatch = this.DepartureBatch.Text.Trim();
                Cost.StartSite = this.BeginSite.Text.Trim();
                Cost.DestinationSite = this.EndSite.Text.Trim();
                Cost.actual_rate = costrate;
                Cost.MenuType = "汽运亏损";
                Cost.ShowDialog();
                if (Cost.isprint == true)
                {
                    frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
                    bool flag = false;
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
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

                    //同步配载修改时效 LD 2018-5-24
                    string strBillNo = string.Empty;
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", list);
                    DataSet ds_check = SqlHelper.GetDataSet(sps);
                    if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds_check.Tables[0].Rows)
                        {
                            strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
                        }
                    }
                    SendHaoDuoCheBill(strCompanyId, strCompanyName, strToken); //好多车接口下单，打印推送订单ld

                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
                    if (ds == null || ds.Tables.Count == 0) return;

                    //LMS打印清单同步ZQTMS打印状态
                    if (strPeiZaiType == "ZQTMS")
                    {
                        CommonSyn.LMSDepartureSysZQTMS(list, 2, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT_LMS", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（打印清单）                        
                    }
                    else
                    {
                        //同步配载修改时效 LD 2018-5-24
                        if (!string.IsNullOrEmpty(strBillNo))
                        {
                            CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
                        }
                    }
                    string tmps = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows[i]["NowCompany"] = CommonClass.UserInfo.gsqc;
                        ds.Tables[0].Rows[i]["NowCompanyStr"] = CommonClass.UserInfo.gsqc + "货物运输协议";
                        tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                        if (tmps == "") continue;
                        try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                        catch { }
                    }
                    //zaj 2018-1-15 司机运输协议根据公司ID来加载
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
                    frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? loadList : transprotocol), ds);

                    fpr.ShowDialog();
                }
            }
            else
            {
                frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
                bool flag = false;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
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

                //同步配载修改时效 LD 2018-5-24
                string strBillNo = string.Empty;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds_check.Tables[0].Rows)
                    {
                        strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
                    }
                }
                SendHaoDuoCheBill(strCompanyId, strCompanyName, strToken); //好多车接口下单，打印推送订单ld

                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
                if (ds == null || ds.Tables.Count == 0) return;

                //LMS打印清单同步ZQTMS打印状态
                if (strPeiZaiType == "ZQTMS")
                {
                    CommonSyn.LMSDepartureSysZQTMS(list, 2, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT_LMS", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（打印清单）                        
                }
                else
                {
                    //同步配载修改时效 LD 2018-5-24
                    if (!string.IsNullOrEmpty(strBillNo))
                    {
                        CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
                    }
                }
                string tmps = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["NowCompany"] = CommonClass.UserInfo.gsqc;
                    ds.Tables[0].Rows[i]["NowCompanyStr"] = CommonClass.UserInfo.gsqc + "货物运输协议";
                    tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                    if (tmps == "") continue;
                    try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                    catch { }
                }
                //zaj 2018-1-15 司机运输协议根据公司ID来加载
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
                frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? loadList : transprotocol), ds);

                fpr.ShowDialog();


            }


        }

        public void cbprint_486()
        {
            if (sDepartureBatch == "") return;
            string middleSite = "";
            frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
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

            //同步配载修改时效 LD 2018-5-24
            string strBillNo = string.Empty;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", list);
            DataSet ds_check = SqlHelper.GetDataSet(sps);
            if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_check.Tables[0].Rows)
                {
                    strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
                }
            }
            SendHaoDuoCheBill(strCompanyId, strCompanyName, strToken); //好多车接口下单，打印推送订单ld

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
            if (ds == null || ds.Tables.Count == 0) return;

            //LMS打印清单同步ZQTMS打印状态
            if (strPeiZaiType == "ZQTMS")
            {
                CommonSyn.LMSDepartureSysZQTMS(list, 2, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT_LMS", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（打印清单）                        
            }
            else
            {
                //同步配载修改时效 LD 2018-5-24
                if (!string.IsNullOrEmpty(strBillNo))
                {
                    CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
                }
            }
            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i]["NowCompany"] = CommonClass.UserInfo.gsqc;
                ds.Tables[0].Rows[i]["NowCompanyStr"] = CommonClass.UserInfo.gsqc + "货物运输协议";
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }
            //zaj 2018-1-15 司机运输协议根据公司ID来加载
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
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? loadList : transprotocol), ds);

            fpr.ShowDialog();



        }
        //好多车接口下单，打印推送订单
        private void SendHaoDuoCheBill(string CompanyId, string CompanyName, string strToken)
        {
            //if (Convert.ToInt32(ds.Tables[0].Rows[0]["PrintNum"]) == 1)
            //{
            List<SqlPara> list_hd = new List<SqlPara>();
            list_hd.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            SqlParasEntity spe_hd = new SqlParasEntity(OperType.Query, "QSP_Get_Requirement_Data", list_hd);
            DataSet ds_hd = SqlHelper.GetDataSet(spe_hd);
            if (ds_hd != null && ds_hd.Tables.Count > 0 && ds_hd.Tables[0].Rows.Count > 0)
            {
                string strReturn = CommonSyn.SendHaoDuoCheOrder(ds_hd, CompanyId, CompanyName, strToken);
                if (strReturn != "成功")
                {
                    string[] strs = strReturn.Split(new char[] { '@' });
                    if (strs.Length > 1)
                    {
                        List<SqlPara> listLog = new List<SqlPara>();
                        listLog.Add(new SqlPara("Batch", sDepartureBatch));
                        listLog.Add(new SqlPara("FutionName", "SendHaoDuoCheOrder"));
                        listLog.Add(new SqlPara("FaceUrl", ""));
                        listLog.Add(new SqlPara("FaceJson", strs[1]));
                        listLog.Add(new SqlPara("ResultMessage", strs[0]));
                        listLog.Add(new SqlPara("FaceState", "失败"));
                        SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                        SqlHelper.ExecteNonQuery(spsLog);
                    }
                }
            }
            else
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", sDepartureBatch));
                listLog.Add(new SqlPara("FutionName", "SendHaoDuoCheOrder"));
                listLog.Add(new SqlPara("FaceUrl", ""));
                listLog.Add(new SqlPara("FaceJson", ""));
                listLog.Add(new SqlPara("ResultMessage", "QSP_Get_Requirement_Data：查询数据不满足推送条件！"));
                listLog.Add(new SqlPara("FaceState", "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            SendHaoDuoCheBill(strCompanyId, strCompanyName, strToken); //好多车接口下单，打印推送订单ld

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("PrintMan", CommonClass.UserInfo.UserName) }));
            frmPrintRuiLang fpr = new frmPrintRuiLang("大车运输合同", ds);
            fpr.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == tp2)
                freshData();
            if (e.Page != tp3) return;
            getDepartureInfo();
            string BillNo = "", address = "", addressStr = "",
                fecthBillNO = FecthBillNO.Text.Trim(),
                sendBillNO = SendBillNO.Text.Trim();
            FecthBillNO.Properties.Items.Clear();
            SendBillNO.Properties.Items.Clear();
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView2.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView2, "TransferMode"))) == "司机直送")  //hj20180616 中心直送改为司机直送
                {
                    BillNo = GridOper.GetRowCellValueString(myGridView2, i, "BillNO");
                    address = GridOper.GetRowCellValueString(myGridView2, i, "ReceivAddress");
                    if (address != "") addressStr += address + ",";
                    if (BillNo != "" && !FecthBillNO.Properties.Items.Contains(BillNo))
                    {
                        FecthBillNO.Properties.Items.Add(BillNo);
                        SendBillNO.Properties.Items.Add(BillNo);
                    }
                }
            }
            FecthBillNO.Text = fecthBillNO;
            SendBillNO.Text = sendBillNO;
            SendAddr.Text = addressStr;
        }

        //审核
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            list.Add(new SqlPara("Man", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("type", 1));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATE", list)) == 0) return;
            MsgBox.ShowOK();

            if (strPeiZaiType == "ZQTMS")
            {
                CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_SET_DEPARTURE_VERIFY_STATE", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（合同审核）                        
            }
        }

        //反审核
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            list.Add(new SqlPara("type", 2));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATE", list)) == 0) return;
            MsgBox.ShowOK();

            if (strPeiZaiType == "ZQTMS")
            {
                CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_SET_DEPARTURE_VERIFY_STATE", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（合同反审核）                        
            }
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            getdata(2);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            getdata(3);
        }

        private void EndSite_EditValueChanged(object sender, EventArgs e)
        {
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string[] tmps = endSite.Split(',');
            if (tmps == null || tmps.Length == 0) return;

            edesite1.Text = edesite2.Text = edesite3.Text = "";
            try
            {
                edesite1.Text = tmps[0].Trim();
                edesite2.Text = tmps[1].Trim();
                edesite3.Text = tmps[2].Trim();
            }
            catch { }
            edesiteacc1.Text = edesite1.Text == "" ? "" : edesiteacc1.Text;
            edesiteacc2.Text = edesite2.Text == "" ? "" : edesiteacc2.Text;
            edesiteacc3.Text = edesite3.Text == "" ? "" : edesiteacc3.Text;

            //SetWeb();
        }

        public void SetWeb()
        {
            EndWebName.Properties.Items.Clear();
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string SiteName = "";
            foreach (string a in endSite.Split(','))
                SiteName += "'" + a + "',";
            SiteName = SiteName.TrimEnd(',');
            SiteName = "SiteName IN(" + SiteName + ")";
            DataRow[] drs = CommonClass.dsWeb.Tables[0].Select(SiteName);
            if (drs == null || drs.Length == 0) return;

            foreach (DataRow dr in drs)
            {
                endSite = ConvertType.ToString(dr["WebName"]);
                if (endSite != "" && !EndWebName.Properties.Items.Contains(endSite))
                    EndWebName.Properties.Items.Add(endSite);
            }
            EndWebName.Text = "";
        }
        //修改中转地
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (simpleButton12.Text == "修正中转地")
            {
                simpleButton12.Text = "保存中转地";
                gcTransferSite.OptionsColumn.AllowEdit = gcTransferSite.OptionsColumn.AllowFocus = true;
                myGridView2.FocusedColumn = gcTransferSite;
            }
            else
            {
                myGridView2.PostEditor();
                simpleButton12.Text = "修正中转地";
                gcTransferSite.OptionsColumn.AllowEdit = gcTransferSite.OptionsColumn.AllowFocus = false;
                if (editRowTransferSite.Count == 0) return;

                string sbillno = "";
                string sTransferSite = "";

                for (int i = 0; i < editRowTransferSite.Count; i++)
                {
                    sbillno += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "BillNO")) + "@";
                    sTransferSite += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "TransferSite")) + "@";
                }
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", sbillno));
                    list.Add(new SqlPara("TransferSite", sTransferSite));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_WAYBILL_TransferSite", list);
                    if (SqlHelper.ExecteNonQuery(sps) == 0) return;

                
                    if (strPeiZaiType == "ZQTMS")
                    {
                        CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_UPDATE_WAYBILL_TransferSite", sbillno, sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（修改中转地）                        
                    }
                    //zb20190622
                    //else
                    //{
                    //    list = new List<SqlPara>();
                    //    list.Add(new SqlPara("BillNos", sbillno.Replace("@", ",")));
                    //    list.Add(new SqlPara("TransferSite", sTransferSite.Replace("@", ",")));
                    //    list.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
                    //    list.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
                    //    list.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
                    //    list.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
                    //    list.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
                    //    list.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
                    //    list.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));

                    //    CommonSyn.LMSSynZQTMS(list, "ZQTMS短驳至LMS，LMS自配在修改中转地", "USP_WayBill_Data_By_BillNo");
                    //}
                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

        private void myGridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRowTransferSite.Contains(e.RowHandle)) return;
            editRowTransferSite.Add(e.RowHandle);
        }
        //修改交接方式
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (gcTransferMode == null) return;
            //hj20180411
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                {
                    MsgBox.ShowOK("本车已经打印司机运输协议，不能修改交接方式!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            if (simpleButton18.Text == "修正交接方式")
            {
                simpleButton18.Text = "保存交接方式";
                gcTransferMode.OptionsColumn.AllowEdit = gcTransferMode.OptionsColumn.AllowFocus = true;
                myGridView2.FocusedColumn = gcTransferMode;
            }
            else
            {
                myGridView2.PostEditor();
                simpleButton18.Text = "修正交接方式";
                gcTransferMode.OptionsColumn.AllowEdit = gcTransferMode.OptionsColumn.AllowFocus = false;
                if (editRowTransferSite.Count == 0) return;
                string sbillno = "";
                string sTransferMode = "";
                for (int i = 0; i < editRowTransferSite.Count; i++)
                {
                    sbillno += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "BillNO")) + ",";
                    sTransferMode += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "TransferMode")) + ",";
                }
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", sbillno));
                    list.Add(new SqlPara("TransferMode", sTransferMode));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_WAYBILL_TransferMode", list);
                    if (SqlHelper.ExecteNonQuery(sps) == 0) return;

                    if (strPeiZaiType == "ZQTMS")
                    {
                        CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_UPDATE_WAYBILL_TransferMode", sbillno, sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（修改交接方式）                        
                    }
                    //zb20190622
                    //else
                    //{
                    //    list = new List<SqlPara>();
                    //    list.Add(new SqlPara("BillNos", sbillno));
                    //    list.Add(new SqlPara("TransferMode", sTransferMode));
                    //    list.Add(new SqlPara("LoginAreaName", CommonClass.UserInfo.AreaName));
                    //    list.Add(new SqlPara("LoginCauseName", CommonClass.UserInfo.CauseName));
                    //    list.Add(new SqlPara("LoginDepartName", CommonClass.UserInfo.DepartName));
                    //    list.Add(new SqlPara("LoginSiteName", CommonClass.UserInfo.SiteName));
                    //    list.Add(new SqlPara("LoginWebName", CommonClass.UserInfo.WebName));
                    //    list.Add(new SqlPara("LoginUserAccount", CommonClass.UserInfo.UserAccount));
                    //    list.Add(new SqlPara("LoginUserName", CommonClass.UserInfo.UserName));

                    //    CommonSyn.LMSSynZQTMS(list, "ZQTMS短驳至LMS，LMS自配在修改交接方式", "USP_WayBill_Data_By_BillNo");
                    //}
                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }
        //单票强装
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            string WebState = string.Empty;
            DataTable dt2 = this.myGridControl2.DataSource as DataTable;
            if (dt2 != null && dt2.Rows.Count > 0)
            {
                DataRow[] dr = dt2.Select(" WebState = 1");
                if (dr.Length > 0)
                {
                    WebState = dr[0]["WebState"].ToString();
                }
            }
            frmDepartureAddSingle fdas = new frmDepartureAddSingle();
            fdas.DepartureBatch = sDepartureBatch;
            fdas.CarNo = labCarNO.Text;
            fdas.DriverName = labDriverName.Text;
            fdas.DriverPhone = DriverPhone.Text.Trim();
            fdas.BeginSite = BeginSite.Text.Trim();
            fdas.EndSite = EndSite.Text.Trim();
            fdas.strPeiZaiType = strPeiZaiType;
            fdas.WebState = WebState;
            fdas.ShowDialog();

            freshData();//刷新本车清单
        }

        private void AccCollectPremium_Validated(object sender, EventArgs e)
        {
            //今天之后的才需要设置最低100
            //暂时去掉服务费100 zaj 2017-12-19
            //if (DepartureDate.DateTime > ConvertType.ToDateTime("2016-11-09 00:00"))
            //{
            //    if (ConvertType.ToDecimal(AccCollectPremium.Text) < 100)
            //        AccCollectPremium.Text = "100";
            //}
        }

        private Guid oilId = Guid.Empty;
        //查询油料记录
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            frmOilPlants_Select frm = new frmOilPlants_Select(CarNO.Text);
            frm.ShowDialog();
            if (!string.IsNullOrEmpty(frm.OilGuid))
            {
                oilId = new Guid(frm.OilGuid);
                OilFee.EditValue = frm.OilFee;
                oilVolume.EditValue = frm.OilVolume;
                oilPrice.EditValue = frm.OilPrice;
            }
        }
        //hj20180416
        public string ResponseSite { get; set; }
        public string ResponseWeb { get; set; }
        private void EndSite_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            frmDepartureEdit frmEdit = new frmDepartureEdit("LMS");
            frmEdit.RequestSite = EndSite.Text.Trim();
            frmEdit.RequestWeb = EndWeb.Text.Trim();
            frmEdit.Owner = this;
            //List<SqlPara> list1 = new List<SqlPara>();//毛慧20171030--运输合同审核过后，不能再做除目的网点以外的修改。
            //list1.Add(new SqlPara("DepartureBatch", DepartureBatch1));
            //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_aduitStateStr", list1);
            //DataSet ds = SqlHelper.GetDataSet(spe);
            //string a = ds.Tables[0].Rows[0]["aduitStateStr"].ToString();
            //frmEdit.state = a;
            frmEdit.ShowDialog();
            if (frmEdit.IsModify)
            {
                this.EndSite.Text = ResponseSite;
                this.EndWeb.Text = ResponseWeb;
            }
        }

        private void EndSite_EditValueChanged_1(object sender, EventArgs e)
        {
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string[] tmps = endSite.Split(',');
            if (tmps == null || tmps.Length == 0) return;

            edesite1.Text = edesite2.Text = edesite3.Text = "";
            try
            {
                edesite1.Text = tmps[0].Trim();
                edesite2.Text = tmps[1].Trim();
                edesite3.Text = tmps[2].Trim();
            }
            catch { }
            edesiteacc1.Text = edesite1.Text == "" ? "" : edesiteacc1.Text;
            edesiteacc2.Text = edesite2.Text == "" ? "" : edesiteacc2.Text;
            edesiteacc3.Text = edesite3.Text == "" ? "" : edesiteacc3.Text;

        }

        private void edrepremark_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void SetOilCard()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView3.GetDataRow(rowhandle);
            if (dr == null) return;

            OilCardNo.EditValue = dr["OilCardNo"];
            OilCardFee.EditValue = dr["Balance"];
            //Company = dr["Company"].ToString();
            myGridControl3.Visible = false;
        }


        private void myGridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetOilCard();
            }
        }

        private void myGridControl3_DoubleClick(object sender, EventArgs e)
        {
            SetOilCard();
        }

        private void myGridControl3_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = OilCardNo.Focused;
        }

        private void OilCardNo_Enter(object sender, EventArgs e)
        {
            myGridControl3.Left = groupControl2.Left + OilCardNo.Left;
            myGridControl3.Top = groupControl2.Top + OilCardNo.Top + OilCardNo.Height + 2;
            myGridControl3.Visible = true;
        }

        private void OilCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                myGridControl3.Focus();
        }

        private void OilCardNo_Leave(object sender, EventArgs e)
        {
            myGridControl3.Visible = myGridControl3.Focused;
        }

        private void OilCardNo_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            myGridView3.Columns["OilCardNo"].FilterInfo = new ColumnFilterInfo(
                    "[OilCardNo] LIKE " + "'%" + value + "%'"
                    + " OR [Balance] LIKE" + "'%" + value + "%'","");
        }

        private void ccbFetchNo_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void ccbSendNo_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }
        //确认合同
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            string batch = "";
            batch = DepartureBatch.Text.Trim();
            List<SqlPara> list1 = new List<SqlPara>();
            
            list1.Add(new SqlPara("DepartureBatch", batch));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_aduitStateStr", list1);
            DataSet ds = SqlHelper.GetDataSet(spe);
            string a = ds.Tables[0].Rows[0]["aduitStateStr"].ToString();
            string b = ds.Tables[0].Rows[0]["IsPrint"].ToString();
            if (a == "已审核")
            {
                MsgBox.ShowError("已审核合同，不能确认！");
                return;
            }
            if (b == "未打印")
            {
                MsgBox.ShowError("未打印合同，不能确认！");
                return;
            }
            if (CommonClass.QSP_LOCK_7(DepartureDate.Text))
            {
                return;
            }
            if (!UserRight.GetRight("121597") && HtIsPrint(sDepartureBatch))
            {
                MsgBox.ShowOK("本车已打印，不能修改合同信息，如需修改，请联系相关人员授权!");
                return;
            }

            string vehicleno = CarNO.Text.Trim();
            if (vehicleno == "")
            {
                MsgBox.ShowError("车牌号必须填写!");
                CarNO.Focus();
                return;
            }
           
            if (CarrNO.Text.Trim() == "")
            {
                MsgBox.ShowError("车厢号必须填写!");
                CarNO.Focus();
                return;
            }
            if (CarType.Text.Trim() == "")
            {
                MsgBox.ShowError("车型必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarLength.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车长必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarWidth.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车宽必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarHeight.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车高必须填写!");
                CarNO.Focus();
                return;
            }
            if (CarSoure.Text.Trim() == "")
            {
                MsgBox.ShowError("车源必须填写!");
                CarNO.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("请选择配载类型!");
                LoadingType.Focus();
                return;
            }
            decimal accLine = ConvertType.ToDecimal(AccLine.Text);
            if (accLine == 0)
            {
                MsgBox.ShowError("请填写干线运费!");
                AccLine.Focus();
                return;
            }
            decimal accBigCarFecth = ConvertType.ToDecimal(AccBigCarFecth.Text);
            string fecthBillNO = FecthBillNO.Text.Trim();
            if (accBigCarFecth != 0 && fecthBillNO == "")
            {
                MsgBox.ShowError("您填了大车接货费,必须填写接货单号!");
                FecthBillNO.Focus();
                return;
            }
            decimal accBigCarSend = ConvertType.ToDecimal(AccBigCarSend.Text);
            decimal TxtSendFee = Convert.ToDecimal(txtSendFee.Text);
            string sendBillNO = SendBillNO.Text.Trim();
            if ((accBigCarSend + TxtSendFee) != 0 && sendBillNO == "")//毛慧20170928
            {
                MsgBox.ShowError("您填了大车送货费,必须填写送货单号(多个送货单号用‘/’隔开)!");
                SendBillNO.Focus();
                return;
            }
            decimal accShtBarge = ConvertType.ToDecimal(AccShtBarge.Text);
            string shtBargeDept = ShtBargeDept.Text.Trim();
            if (accShtBarge != 0 && shtBargeDept == "")
            {
                MsgBox.ShowError("您填了短驳费,必须填写短驳费的承运部门!");
                ShtBargeDept.Focus();
                return;
            }
            decimal accBigcarOther = ConvertType.ToDecimal(AccBigcarOther.Text);
            string takeDept = TakeDept.Text.Trim();
            if (accBigcarOther != 0 && takeDept == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写大车其他费的承运部门!");
                TakeDept.Focus();
                return;
            }
            string bigcarOtherRemark = BigcarOtherRemark.Text.Trim();
            if (accBigcarOther != 0 && bigcarOtherRemark == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写其他费备注!");
                BigcarOtherRemark.Focus();
                return;
            }
            decimal oilCardFee = ConvertType.ToDecimal(OilCardFee.Text);
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardFee != 0 && oilCardNo == "")
            {
                MsgBox.ShowError("您填了油卡金额,必须填写油卡编号!");
                OilCardNo.Focus();
                return;
            }
            //hs
            decimal OilCard = ConvertType.ToDecimal(OilCardFee.Text);
            string OilNo = OilCardNo.Text.Trim();
            string ManName = oilCardManName.Text.Trim(); ;
            string Account = oilCardAccount.Text.Trim(); ;
            string Bank = oilCardBank.Text.Trim();
            string Province = oilCardProvince.Text.Trim();
            string City = oilCardCity.Text.Trim();
            //if (OilCard != 0)
            //{
            //    if (OilNo == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写油卡编号！");
            //        return;
            //    }
            //    if (ManName == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写油卡户名！");
            //        return;
            //    }
            //    if (Account == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写银行账户！");
            //        return;
            //    }
            //    if (Bank == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写银行名！");
            //        return;
            //    }
            //    if (Province == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写所属省份！");
            //        return;
            //    }
            //    if (City == "")
            //    {
            //        MsgBox.ShowError("您填写了油卡金额，必须填写所属城市！");
            //        return;
            //    }

            //}
            decimal BackPay = ConvertType.ToDecimal(BackPayDriver.Text);
            string bankManName = txtbPAccountName.Text.Trim(); ;
            string bankAccount = txtbpAccountNO.Text.Trim(); ;
            string bankBank = txtbPBank.Text.Trim();
            string bankProvince = bankPayProvince.Text.Trim();
            string bankCity = bankPayCity.Text.Trim();
            //if (BackPay != 0)
            //{

            //    if (bankManName == "")
            //    {
            //        MsgBox.ShowError("您填写了回付金额，必须填写回付户名！");
            //        return;
            //    }
            //    if (bankAccount == "")
            //    {
            //        MsgBox.ShowError("您填写了回付金额，必须填写回付账号！");
            //        return;
            //    }
            //    if (bankBank == "")
            //    {
            //        MsgBox.ShowError("您填写了回付金额，必须填写回付开户行！");
            //        return;
            //    }
            //    if (bankProvince == "")
            //    {
            //        MsgBox.ShowError("您填写了回付金额，必须填写所属省份！");
            //        return;
            //    }
            //    if (bankCity == "")
            //    {
            //        MsgBox.ShowError("您填写了回付金额，必须填写所属城市！");
            //        return;
            //    }

            //}

            string oilCardAccountStr = oilCardAccount.Text.Trim().Replace(" ", "");
            string txtnpAccountNOStr = txtnpAccountNO.Text.Trim().Replace(" ", "");
            string txtbpAccountNOStr = txtbpAccountNO.Text.Trim().Replace(" ", "");
            string oilCardManNameStr = oilCardManName.Text.Trim();
            string txtnpAccountNameStr = txtnpAccountName.Text.Trim();
            string txtbPAccountNameStr = txtbPAccountName.Text.Trim();
            Regex r = new Regex(@"^\d{9,50}$");
            Regex r2 = new Regex(@"[\u4e00-\u9fbb]");

            if (Convert.ToDecimal(OilCardFee.Text.Trim().ToString() == "" ? "0" : OilCardFee.Text.Trim().ToString()) > 0)
            {
                if (!r.IsMatch(oilCardAccountStr))
                {
                    MsgBox.ShowError("请输入正确的油卡账户!");
                    return;
                }
                if (!r2.IsMatch(oilCardManNameStr))
                {
                    MsgBox.ShowError("油卡户名只能输汉字");
                    return;
                }
            }
            if (Convert.ToDecimal(NowPayDriver.Text.Trim().ToString() == "" ? "0" : NowPayDriver.Text.Trim().ToString()) > 0)
            {
                if (!r.IsMatch(txtnpAccountNOStr))
                {
                    MsgBox.ShowError("请输入正确的现付账户!");
                    return;
                }
                if (!r2.IsMatch(txtnpAccountNameStr))
                {
                    MsgBox.ShowError("现付户名只能输汉字");
                    return;
                }
            }
            if (Convert.ToDecimal(BackPayDriver.Text.Trim().ToString() == "" ? "0" : BackPayDriver.Text.Trim().ToString()) > 0)
            {
                if (!r.IsMatch(txtbpAccountNOStr))
                {
                    MsgBox.ShowError("请输入正确的回付账户!");
                    return;
                }
                if (!r2.IsMatch(txtbPAccountNameStr))
                {
                    MsgBox.ShowError("回付户名只能输汉字");
                    return;
                }
            }
            //-----------------

            decimal driverTakePay = 0, accTakeCar = 0, accCollectPremium = 0, nowPayDriver = 0, toPayDriver = 0, backPayDriver = 0;
            driverTakePay = ConvertType.ToDecimal(DriverTakePay.Text);//司机代收
            accTakeCar = ConvertType.ToDecimal(AccTakeCar.Text);//代收派车费
            nowPayDriver = ConvertType.ToDecimal(NowPayDriver.Text);
            toPayDriver = ConvertType.ToDecimal(ToPayDriver.Text);
            backPayDriver = ConvertType.ToDecimal(BackPayDriver.Text);
            if (ConvertType.ToDecimal(AccBigcarTotal.Text) != (driverTakePay + accTakeCar + accCollectPremium + nowPayDriver + toPayDriver + backPayDriver + oilCardFee))
            {
                MsgBox.ShowError("大车费合计=司机代收款+代收派车费+代收保险费+现付驾驶员+到付驾驶员+回付驾驶员+油卡金额,请检查!");
                return;
            }
            string esite1 = edesite1.Text.Trim();
            decimal eacc1 = esite1 == "" ? 0 : ConvertType.ToDecimal(edesiteacc1.Text);
            if (esite1 == "" && eacc1 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite1));
                edesiteacc1.Focus();
                return;
            }
            string esite2 = edesite2.Text.Trim();
            decimal eacc2 = esite2 == "" ? 0 : ConvertType.ToDecimal(edesiteacc2.Text);
            if (esite2 == "" && eacc2 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite2));
                edesiteacc2.Focus();
                return;
            }
            string esite3 = edesite3.Text.Trim();
            decimal eacc3 = esite3 == "" ? 0 : ConvertType.ToDecimal(edesiteacc3.Text);
            if (esite3 == "" && eacc3 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite3));
                edesiteacc3.Focus();
                return;
            }

            if (myGridControl2.DataSource == null)
            {
                MsgBox.ShowError("发车明细列表没有数据!");
                return;
            }

            string DriverIDCardno = DriverIDCardNo.Text.Trim();
            if (DriverIDCardno == "")
            {
                MsgBox.ShowError("驾驶员身份证必填!");
                DriverIDCardNo.Focus();
                return;
            }  //plh

            if (DriverIDCardno.Length != 18)
            {
                MsgBox.ShowError("驾驶员身份证必须为18位!");
                DriverIDCardNo.Focus();
                return;
            }  //plh

            DataTable dt = myGridControl2.DataSource as DataTable;
            object obj = dt.Compute("sum(FetchPay)", "TransferMode='司机直送' and PaymentMode='提付'");
            decimal FetchPay = obj == null || obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
          

          
            if (MsgBox.ShowYesNo("是否确认合同？") != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("BeginSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("EndSite", EndSite.Text.Trim()));
                list.Add(new SqlPara("EndWeb", EndWeb.Text.Trim()));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                //预到日期
                if (ExpArriveDate.Text.Trim() == "")
                    list.Add(new SqlPara("ExpArriveDate", DBNull.Value));
                else
                    list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));

                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", LoadPeoples.Text.Trim()));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", BoxNO.Text.Trim()));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                list.Add(new SqlPara("LoadingType", loadingType));
                list.Add(new SqlPara("AccLine", accLine));//干线运费
                list.Add(new SqlPara("FecthSite", FecthSite.Text.Trim()));//接货地
                list.Add(new SqlPara("AccBigCarFecth", accBigCarFecth));//大车接货费
                list.Add(new SqlPara("FecthBillNO", fecthBillNO));
                list.Add(new SqlPara("AccBigCarSend", accBigCarSend));
                list.Add(new SqlPara("SendBillNO", sendBillNO));
                list.Add(new SqlPara("SendAddr", SendAddr.Text.Trim()));
                list.Add(new SqlPara("AccShtBarge", ConvertType.ToDecimal(AccShtBarge.Text)));
                list.Add(new SqlPara("ShtBargeDept", ShtBargeDept.Text.Trim()));
                list.Add(new SqlPara("AccSomeLoad", ConvertType.ToDecimal(AccSomeLoad.Text)));
                list.Add(new SqlPara("UnloadAddr", UnloadAddr.Text));
                list.Add(new SqlPara("AccBigcarOther", accBigcarOther));
                list.Add(new SqlPara("TakeDept", takeDept));
                list.Add(new SqlPara("AccBigcarTotal", ConvertType.ToDecimal(AccBigcarTotal.Text)));
                list.Add(new SqlPara("DriverTakePay", driverTakePay));
                list.Add(new SqlPara("AccTakeCar", accTakeCar));
                list.Add(new SqlPara("AccCollectPremium", accCollectPremium));
                list.Add(new SqlPara("NowPayDriver", nowPayDriver));
                list.Add(new SqlPara("ToPayDriver", toPayDriver));
                list.Add(new SqlPara("BackPayDriver", backPayDriver));
                list.Add(new SqlPara("edesite1", esite1));
                list.Add(new SqlPara("edesiteacc1", eacc1));
                list.Add(new SqlPara("edesite2", esite2));
                list.Add(new SqlPara("edesiteacc2", eacc2));
                list.Add(new SqlPara("edesite3", esite3));
                list.Add(new SqlPara("edesiteacc3", eacc3));
                list.Add(new SqlPara("BigcarOtherRemark", bigcarOtherRemark));
                list.Add(new SqlPara("DeliCode", fecthBillNO));
                list.Add(new SqlPara("OilCardFee", oilCardFee));
                list.Add(new SqlPara("OilCardNo", oilCardNo));
                list.Add(new SqlPara("CarType", CarType.Text.Trim()));
                list.Add(new SqlPara("CarLength", CarLength.Text.Trim()));
                list.Add(new SqlPara("CarWidth", CarWidth.Text.Trim()));
                list.Add(new SqlPara("CarHeight", CarHeight.Text.Trim()));
                list.Add(new SqlPara("NetWeight", ConvertType.ToDecimal(NetWeight.Text)));
                list.Add(new SqlPara("CarSoure", CarSoure.Text.Trim()));
                list.Add(new SqlPara("oilPrice", ConvertType.ToDecimal(OilFee.Text)));
                list.Add(new SqlPara("oilId", oilId));
                list.Add(new SqlPara("oilVolume", ConvertType.ToDecimal(oilVolume.Text)));
                list.Add(new SqlPara("oilPrice1", ConvertType.ToDecimal(oilPrice.Text)));
                list.Add(new SqlPara("NowPayAccontName", txtnpAccountName.Text.Trim()));
                list.Add(new SqlPara("NowPayBankName", txtnpBank.Text.Trim()));
                list.Add(new SqlPara("NowPayAccountNO", txtnpAccountNO.Text.Trim()));
                list.Add(new SqlPara("BackPayAccontName", txtbPAccountName.Text.Trim()));
                list.Add(new SqlPara("BackPayBankName", txtbPBank.Text.Trim()));
                list.Add(new SqlPara("BackPayAccountNO", txtbpAccountNO.Text.Trim()));
                list.Add(new SqlPara("ConfirmMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ConfirmTime", DateTime.Now));
                //list.Add(new SqlPara("oilCardAccount", oilCardAccount.Text.Trim()));
                //list.Add(new SqlPara("oilCardBank", oilCardBank.Text.Trim()));
                //list.Add(new SqlPara("oilCardManName", oilCardManName.Text.Trim()));
                //list.Add(new SqlPara("oilCardProvince", oilCardProvince.Text.Trim()));
                //list.Add(new SqlPara("oilCardCity", oilCardCity.Text.Trim()));
                //list.Add(new SqlPara("nowPayCity", nowPayCity.Text.Trim()));
                //list.Add(new SqlPara("nowPayProvince", nowPayProvince.Text.Trim()));
                //list.Add(new SqlPara("bankPayCity", bankPayCity.Text.Trim()));
                //list.Add(new SqlPara("bankPayProvince", bankPayProvince.Text.Trim()));
                list.Add(new SqlPara("DriverIDCardNo", DriverIDCardNo.Text.Trim()));  //plh20191225

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLDEPARTURE_Confirm", list)) == 0) return;
                isModify = true;
                simpleButton21.Enabled = false;
                MsgBox.ShowOK("合同确认成功!");

                if (strPeiZaiType == "ZQTMS")
                {
                    CommonSyn.LMSDepartureSysZQTMS(list, 2, "USP_UPDATE_BILLDEPARTURE_Confirm", "", sDepartureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（合同确认）                        
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void oilCardProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCity(oilCardProvince, oilCardCity);
        }

        private void nowPayProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCity(nowPayProvince, nowPayCity);
        }

        private void bankPayProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCity(bankPayProvince, bankPayCity);
        }

        private void btnPrintZjBill_Click(object sender, EventArgs e)
        {
            try
            {
                if (sDepartureBatch == "") return;
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ZJFeeRegister_Print", new List<SqlPara> { new SqlPara("batchNo", sDepartureBatch) }));
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有可打印的数据，请先保存！");
                    return;
                }
                frmPrintRuiLang fpr = new frmPrintRuiLang("大车费增减款清单", ds);
                fpr.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void oilCardManName_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string name = oilCardManName.Text.Trim();
            if (name != "" && this.myGridView5.DataRowCount > 0)
            {
                this.myGridView5.ClearColumnsFilter();
                myGridView5.Columns["bankman"].FilterInfo = new ColumnFilterInfo(
                    "[bankman] LIKE " + "'%" + name + "%'",
                    "");
            }
            else
            {
                this.myGridView5.ClearColumnsFilter();
            }
        }

        private void oilCardManName_Enter(object sender, EventArgs e)
        {
            this.myGridControl4.DataSource = CommonClass.dsBank_Loading.Tables[0];

            this.myGridControl4.Left = oilCardManName.Left + 476;
            myGridControl4.Top = oilCardManName.Top + oilCardManName.Height + 41;
            myGridControl4.Visible = true;
            myGridControl4.BringToFront();
        }

        private void oilCardManName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl4.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { myGridControl4.Visible = false; }
        }

        private void oilCardManName_Leave(object sender, EventArgs e)
        {
            changeNum = 1;
            if (!myGridControl4.Focused)
            {
                myGridControl4.Visible = false;
            }
        }

        private void txtnpAccountName_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string name = txtnpAccountName.Text.Trim();
            if (name != "" && this.myGridView5.DataRowCount > 0)
            {
                this.myGridView5.ClearColumnsFilter();
                myGridView5.Columns["bankman"].FilterInfo = new ColumnFilterInfo(
                    "[bankman] LIKE " + "'%" + name + "%'",
                    "");
            }
            else
            {
                this.myGridView5.ClearColumnsFilter();
            }
        }

        private void txtnpAccountName_Enter(object sender, EventArgs e)
        {
            this.myGridControl4.DataSource = CommonClass.dsBank_Loading.Tables[0];

            this.myGridControl4.Left = txtnpAccountName.Left + 476;
            myGridControl4.Top = txtnpAccountName.Top + txtnpAccountName.Height + 41;
            myGridControl4.Visible = true;
            myGridControl4.BringToFront();
        }

        private void txtnpAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl4.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { myGridControl4.Visible = false; }
        }

        private void txtnpAccountName_Leave(object sender, EventArgs e)
        {
            changeNum = 2;
            if (!myGridControl4.Focused)
            {
                myGridControl4.Visible = false;
            }
        }

        private void txtbPAccountName_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string name = txtbPAccountName.Text.Trim();
            if (name != "" && this.myGridView5.DataRowCount > 0)
            {
                this.myGridView5.ClearColumnsFilter();
                myGridView5.Columns["bankman"].FilterInfo = new ColumnFilterInfo(
                    "[bankman] LIKE " + "'%" + name + "%'",
                    "");
            }
            else
            {
                this.myGridView5.ClearColumnsFilter();
            }
        }

        private void txtbPAccountName_Enter(object sender, EventArgs e)
        {
            this.myGridControl4.DataSource = CommonClass.dsBank_Loading.Tables[0];

            this.myGridControl4.Left = txtbPAccountName.Left + 476;
            myGridControl4.Top = txtbPAccountName.Top + txtbPAccountName.Height + 41;
            myGridControl4.Visible = true;
            myGridControl4.BringToFront();
        }

        private void txtbPAccountName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl4.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { myGridControl4.Visible = false; }
        }

        private void txtbPAccountName_Leave(object sender, EventArgs e)
        {
            changeNum = 3;
            if (!myGridControl4.Focused)
            {
                myGridControl4.Visible = false;
            }
        }


        /// <summary>
        /// 增减款登记保存
        /// author：hs
        /// date:2018-11-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        decimal LineFee = 0, BigCarFecthFee = 0, BigCarSendFee = 0, LandingFee = 0, OverWeightFee = 0, PressNightFee = 0,
                    DeclareFee = 0, OtherFee = 0, sumCarFee = 0, sumFee = 0, DriverReplacePay = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CommonClass.QSP_LOCK_7(DepartureDate.Text))
            {
                return;
            }
            List<SqlPara> lst = new List<SqlPara>();
            try
            {
                
                decimal _txtFetchFee = ConvertType.ToDecimal(txtFetchFee.Text.Trim());
                if (_txtFetchFee != 0 && ccbFetchNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了大车接货费，必须填写接货单号！");
                    return;
                }
                if (_txtFetchFee != 0 && string.IsNullOrEmpty(txtFecthSite.Text.Trim()))
                {
                    MsgBox.ShowOK("您填了大车接货费，必须填写接货地！");
                    return;
                }

                decimal _txtSendFee = ConvertType.ToDecimal(txtSendFee.Text.Trim());
                if (_txtSendFee != 0 && ccbSendGoodNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了大车送货费，必须填写送货单号！");
                    return;
                }
                if (_txtSendFee != 0 && string.IsNullOrEmpty(txtSendSite.Text.Trim()))
                {
                    MsgBox.ShowOK("您填了大车送货费，必须填写送货地！");
                    return;
                }
                decimal _txtLandingFee = ConvertType.ToDecimal(txtLandingFee.Text.Trim());

                if (_txtLandingFee != 0 && ccbLandingSite.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了多地卸货费，必须填写卸货地！");
                    return;
                }
                decimal _txtOverWeightFee = ConvertType.ToDecimal(txtOverWeightFee.Text.Trim());
                decimal _ccbOverWeight = Convert.ToDecimal(ccbOverWeight.Text.Trim());
                if (_txtOverWeightFee != 0 && _ccbOverWeight == 0)
                {
                    MsgBox.ShowOK("您填了超重费，必须填写超重吨数！");
                    return;
                }
                decimal _txtPressNight = ConvertType.ToDecimal(txtPressNight.Text.Trim());
                if (_txtPressNight != 0 && txtMark.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了压夜费，必须填写压夜费备注！");
                    return;
                }

                decimal _txtDeclare = ConvertType.ToDecimal(txtDeclare.Text.Trim());
                if (_txtDeclare != 0 && txtDeclareNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了报关费，必须填写报关单号！");
                    return;
                }

                decimal _txtOtherFee = ConvertType.ToDecimal(txtOtherFee.Text.Trim());
                if (_txtOtherFee != 0 && txtOtherFeeMark.Text.Trim() == "")
                {
                    MsgBox.ShowOK("您填了大车其他费，必须填写其他费备注！");
                    return;
                }

                if (!StringHelper.IsDecimal(ccbOverWeight.Text.Trim()))
                {
                    XtraMessageBox.Show("超重吨数格式有误，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                decimal driverReplace = ConvertType.ToDecimal(txtDriverPay.Text.Trim());
                if (driverReplace != 0 && string.IsNullOrEmpty(ccbSendNo.Text.Trim()))
                {
                    MsgBox.ShowOK("您填了司机代收款，必须填写直送单号！");
                    return;
                }

                lst.Add(new SqlPara("ZJBatchNo", sDepartureBatch));
                lst.Add(new SqlPara("ZJLineFee", txtLineFee.Text.Trim()));
                lst.Add(new SqlPara("ZJBigCarFecthFee", txtFetchFee.Text.Trim()));
                lst.Add(new SqlPara("ZJBigCarSendFee", txtSendFee.Text.Trim()));
                lst.Add(new SqlPara("ZJLandingFee", txtLandingFee.Text.Trim()));

                lst.Add(new SqlPara("ZJOverWeightFee", txtOverWeightFee.Text.Trim()));
                lst.Add(new SqlPara("ZJPressNightFee", txtPressNight.Text.Trim()));
                lst.Add(new SqlPara("ZJDeclareFee", txtDeclare.Text.Trim()));
                lst.Add(new SqlPara("ZJOtherFee", txtOtherFee.Text.Trim()));
                lst.Add(new SqlPara("ZJDriverTakePay", txtDriverPay.Text.Trim() == "" ? "0" : txtDriverPay.Text.Trim()));

                lst.Add(new SqlPara("ZJSumCarFee", txtCarSumFee.Text.Trim()));
                lst.Add(new SqlPara("ZJSumFee", txtSumZjFee.Text.Trim() == "" ? "0" : txtSumZjFee.Text.Trim()));
                lst.Add(new SqlPara("ZJDeliverySite", string.IsNullOrEmpty(txtFecthSite.Text.Trim()) ? "" : txtFecthSite.Text.Trim()));
                lst.Add(new SqlPara("ZJDeliveryNo", string.IsNullOrEmpty(ccbFetchNo.Text.Trim()) ? "" : ccbFetchNo.Text.Trim()));
                lst.Add(new SqlPara("ZJSendGoodsNo", string.IsNullOrEmpty(ccbSendGoodNo.Text.Trim()) ? "" : ccbSendGoodNo.Text.Trim()));

                lst.Add(new SqlPara("ZJSendGoodsSite", string.IsNullOrEmpty(txtSendSite.Text.Trim()) ? "" : txtSendSite.Text.Trim()));
                lst.Add(new SqlPara("ZJLandingSite", string.IsNullOrEmpty(ccbLandingSite.Text.Trim()) ? "" : ccbLandingSite.Text.Trim()));
                lst.Add(new SqlPara("ZJOverWeightNum", ccbOverWeight.Text.Trim() == "" ? "0" : ccbOverWeight.Text.Trim()));
                lst.Add(new SqlPara("ZJMark", string.IsNullOrEmpty(txtMark.Text.Trim()) ? "" : txtMark.Text.Trim()));
                lst.Add(new SqlPara("ZJDeclareBillNo", string.IsNullOrEmpty(txtDeclareNo.Text.Trim()) ? "" : txtDeclareNo.Text.Trim()));

                lst.Add(new SqlPara("ZJBearDep", string.IsNullOrEmpty(ZJBearDep.Text.Trim()) ? "" : ZJBearDep.Text.Trim()));
                lst.Add(new SqlPara("ZJOtherFeeMark", string.IsNullOrEmpty(txtOtherFeeMark.Text.Trim()) ? "" : txtOtherFeeMark.Text.Trim()));
                lst.Add(new SqlPara("ZJSendGoodsFeeNo", string.IsNullOrEmpty(ccbSendNo.Text.Trim()) ? "" : ccbSendNo.Text.Trim()));

                lst.Add(new SqlPara("temp", flag));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_ZJFeeRegister", lst)) > 0)
                {
                    MsgBox.ShowOK();
                    this.btnSave.Enabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 增减款登记确认
        /// author：
        /// date:
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int temp = 0;
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            temp = 1;
            try
            {
                if (MsgBox.ShowYesNo("确定要完成确认吗？") == DialogResult.No) return;
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_ZJFeeRegister", new List<SqlPara>() { new SqlPara("ZJBatchNo", sDepartureBatch), new SqlPara("temp", temp) })) > 0)
                {
                    MsgBox.ShowOK();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnUnComfirm_Click(object sender, EventArgs e)
        {
            temp = 2;
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_ZJFeeRegister", new List<SqlPara>() { new SqlPara("ZJBatchNo", sDepartureBatch), new SqlPara("temp", temp) })) > 0)
                {
                    MsgBox.ShowOK();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       //luohui
        private void FecthBillNO_Enter(object sender, EventArgs e)
        {
            myGridControl5.Left = FecthBillNO.Left;

            myGridControl5.Top = FecthBillNO.Top + FecthBillNO.Height;
            myGridControl5.Visible = true;
            myGridControl5.BringToFront();
           
        }

        private void FecthBillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl5.Focus(); }
            if (e.KeyCode == Keys.Escape)
            { myGridControl5.Visible = false; }
        }

        private void FecthBillNO_Leave(object sender, EventArgs e)
        {
            if (!myGridControl5.Focused)
            {
                myGridControl5.Visible = false;
            }
        }
   
        private void myGridControl5_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rows = myGridView6.FocusedRowHandle;
                if (rows < 0) return;
                DataRow dr_ = myGridView6.GetDataRow(rows);
                AccBigCarFecth.Text = dr_["VehFare"].ToString();
                FecthBillNO.Text = dr_["DeliCode"].ToString();
                myGridControl5.Visible = false;
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridControl5_Leave(object sender, EventArgs e)
        {
            myGridControl5.Visible = myGridControl5.Focused;
        }

        private void myGridView4_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rows = this.myGridView4.FocusedRowHandle;
                if (rows < 0) return;
                if (changeNum == 1)
                {
                    oilCardManName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.oilCardManName_EditValueChanging);
                    oilCardManName.EditValue = myGridView4.GetRowCellValue(rows, "bankman").ToString();
                    oilCardAccount.EditValue = myGridView4.GetRowCellValue(rows, "bankcode").ToString();
                    oilCardBank.EditValue = myGridView4.GetRowCellValue(rows, "bankname").ToString();
                    oilCardProvince.EditValue = myGridView4.GetRowCellValue(rows, "sheng").ToString();
                    oilCardCity.EditValue = myGridView4.GetRowCellValue(rows, "city").ToString();
                }
                if (changeNum == 2)
                {
                    txtnpAccountName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtnpAccountName_EditValueChanging);
                    txtnpAccountName.EditValue = myGridView4.GetRowCellValue(rows, "bankman").ToString();
                    txtnpAccountNO.EditValue = myGridView4.GetRowCellValue(rows, "bankcode").ToString();
                    txtnpBank.EditValue = myGridView4.GetRowCellValue(rows, "bankname").ToString();
                    nowPayProvince.EditValue = myGridView4.GetRowCellValue(rows, "sheng").ToString();
                    nowPayCity.EditValue = myGridView4.GetRowCellValue(rows, "city").ToString();
                }
                if (changeNum == 3)
                {
                    txtbPAccountName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtbPAccountName_EditValueChanging);
                    txtbPAccountName.EditValue = myGridView4.GetRowCellValue(rows, "bankman").ToString();
                    txtbpAccountNO.EditValue = myGridView4.GetRowCellValue(rows, "bankcode").ToString();
                    txtbPBank.EditValue = myGridView4.GetRowCellValue(rows, "bankname").ToString();
                    bankPayProvince.EditValue = myGridView4.GetRowCellValue(rows, "sheng").ToString();
                    bankPayCity.EditValue = myGridView4.GetRowCellValue(rows, "city").ToString();
                }


                myGridControl4.Visible = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || e.Column.FieldName != "factqty") return;
            int factqty = ConvertType.ToInt32(e.Value);

            int Num = ConvertType.ToInt32(myGridView1.GetRowCellValue(e.RowHandle, "Num"));
            if (factqty <= 0 || factqty > Num) return;
            decimal feeWeight = ConvertType.ToDecimal(myGridView1.GetRowCellValue(e.RowHandle, "FeeWeight2"));//计费重量
            decimal feeVolume = ConvertType.ToDecimal(myGridView1.GetRowCellValue(e.RowHandle, "FeeVolume2"));//计费体积
            decimal actualFreight = ConvertType.ToDecimal(myGridView1.GetRowCellValue(e.RowHandle, "ActualFreight"));//实收运费Zb2190617

            myGridView1.SetRowCellValue(e.RowHandle, "FeeWeight", Math.Round(feeWeight * factqty / Num, 2));
            myGridView1.SetRowCellValue(e.RowHandle, "FeeVolume", Math.Round(feeVolume * factqty / Num, 2));
            myGridView1.SetRowCellValue(e.RowHandle, "ActualWeight", Math.Round(feeWeight * factqty / Num, 2));//实际计费重量
            myGridView1.SetRowCellValue(e.RowHandle, "ActualVolume", Math.Round(feeVolume * factqty / Num, 2));//实际计费体积
            myGridView1.SetRowCellValue(e.RowHandle, "PZ", Math.Round(actualFreight * factqty / Num, 2));//配载金额Zb2190617

        }

        //private void myGridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        //{
        //    int rowhandle = myGridView1.FocusedRowHandle;
        //    if (rowhandle < 0) return;

        //    try
        //    {
        //        string billno = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
        //        DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ActualWeight", new List<SqlPara> { new SqlPara("BillNo", billno) }));
        //        decimal ActualWeight = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["ActualWeight"]);
        //        decimal ActualVolume = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["ActualVolume"]);
        //        if (ds == null || ds.Tables.Count == 0) return;

        //        int Num = ConvertType.ToInt32(myGridView1.GetRowCellValue(e.RowHandle, "Num"));
        //        int factqty = ConvertType.ToInt32(myGridView1.GetRowCellValue(e.RowHandle, "factqty"));
        //        int LeftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(e.RowHandle, "LeftNum"));


        //        if (myGridView1.FocusedColumn.FieldName == "factqty")
        //        {
        //            if (e == null || myGridView1.GetFocusedRowCellValue("factqty") == null) return;
        //            try
        //            {
        //                int leftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "LeftNum"));
        //                int factQty = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "factqty"));

        //                if (factQty <= 0)
        //                {
        //                    MsgBox.ShowError("实发件数不能小于0!");
        //                    myGridView1.SetRowCellValue(rowhandle, "factqty", leftNum);
        //                    return;
        //                }
        //                else if (factQty > leftNum)
        //                {
        //                    MsgBox.ShowError("实发件数不能大于剩余件数!");
        //                    myGridView1.SetRowCellValue(rowhandle, "factqty", leftNum);
        //                    return;
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MsgBox.ShowError(ex.Message);
        //            }
        //        }
        //        else if (myGridView1.FocusedColumn.FieldName == "ActualWeight")
        //        {
        //            if (e == null || myGridView1.GetFocusedRowCellValue("ActualWeight") == null) return;

        //            decimal Value = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rowhandle, "ActualWeight"));
        //            decimal FeeWeight = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rowhandle, "FeeWeight"));
        //            decimal acc1 = FeeWeight - ActualWeight;
        //            decimal acc2 = acc1 - Value;
        //            decimal acc3 = Math.Round(acc1 * factqty / Num, 2);
        //            if (Value <= 0 || ActualWeight > acc1)
        //            {
        //                MsgBox.ShowError("实发计费重量不能为0或者大于剩余计费重量!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualWeight", acc3);
        //                return;
        //            }
        //            if (LeftNum - factqty != 0 && acc2 == 0)
        //            {
        //                MsgBox.ShowError("还有剩余件数,实发计费重量不能等于剩余计费重量!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualWeight", acc3);
        //                return;
        //            }
        //            if (LeftNum - factqty == 0 && acc2 != 0)
        //            {
        //                MsgBox.ShowError("剩余件数为0,实发计费重量必须等于剩余计费重量!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualWeight", acc1);
        //                return;
        //            }
        //        }
        //        else if (myGridView1.FocusedColumn.FieldName == "ActualVolume")
        //        {
        //            decimal Value = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rowhandle, "ActualVolume"));
        //            decimal feeVolume2 = ConvertType.ToDecimal(myGridView1.GetRowCellValue(rowhandle, "FeeVolume"));
        //            decimal acc1 = feeVolume2 - ActualVolume;
        //            decimal acc2 = acc1 - Value;
        //            decimal acc3 = Math.Round(acc1 * factqty / Num, 2);
        //            if (Value <= 0 || ActualVolume > acc1)
        //            {
        //                MsgBox.ShowError("实发计费体积不能为0或者大于剩余计费体积!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualVolume", acc3);
        //                return;
        //            }
        //            if (LeftNum - factqty != 0 && acc2 == 0)
        //            {
        //                MsgBox.ShowError("还有剩余件数,实发计费体积不能等于剩余计费体积!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualVolume", acc3);
        //                return;
        //            }
        //            if (LeftNum - factqty == 0 && acc2 != 0)
        //            {
        //                MsgBox.ShowError("剩余件数为0,实发计费体积必须等于剩余计费体积!");
        //                myGridView1.SetRowCellValue(rowhandle, "ActualVolume", acc1);
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowError(ex.Message);
        //    }
        //}

        private void myGridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRowTransferSite.Contains(e.RowHandle)) return;
            editRowTransferSite.Add(e.RowHandle);
        }
    }
}