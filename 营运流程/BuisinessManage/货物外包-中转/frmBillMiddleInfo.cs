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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using System.IO;
using ZQTMS.UI.其他费用;
using Newtonsoft.Json;
namespace ZQTMS.UI
{
    public partial class frmBillMiddleInfo : BaseForm
    {
        int gettype;
        bool ismodify = false;// ismodify:修改状态
        string _BillNo = "";
        string _MiddleAduitState = ""; //zb20190611 中转费审核状态
        GridView _gv;
        string AccMiddlePayState = "";
        string CS_AccMiddlePay = ""; //hj20180724初始中转费
        private string MiddleSiteName = string.Empty; //修改中转站点
        private string MiddleWebName = string.Empty; //修改中转网点
        public string type;
        public decimal Freight = 0;
        public decimal costrate = 0;
        /// <summary>
        /// 中转类型
        /// </summary>
        public int Gettype
        {
            get { return gettype; }
            set { gettype = value; }
        }

        public bool Ismodify
        {
            get { return ismodify; }
            set { ismodify = value; }
        }

        public frmBillMiddleInfo()
        {
            InitializeComponent();
        }

        public string BillNo
        {
            get { return _BillNo; }
            set { _BillNo = value; }
        }
        public string CSAccMiddlePay
        {
            get { return CS_AccMiddlePay; }
            set { CS_AccMiddlePay = value; }
        }
        //zb20190611
        public string MiddleAduitState
        {
            get { return _MiddleAduitState; }
            set { _MiddleAduitState = value; }
        }
       

        /// <summary>
        /// 中转库存
        /// </summary>
        public GridView Gv
        {
            get { return _gv; }
            set { _gv = value; }
        }

        private void frmBillMiddleInfo_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            this.MiddleCarrier.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.MiddleCarrier_EditValueChanged);
            getCarrier();
            getDelivery();
            modify();
            if (Issign(BillNo))
            {
                MsgBox.ShowOK("已签收的运单不允许修改部分中转信息！");
                MiddleDate.Enabled = false;
                MiddleCarrier.Enabled = false;
                MiddleStartSitePhone.Enabled = false;
                MiddleEndSitePhone.Enabled = false;
                MiddleFetchAddress.Enabled = false;
                MiddleBillNo.Enabled = false;
                MiddleCarNo.Enabled = false;
                MiddleChauffer.Enabled = false;
                MiddleChaufferPhone.Enabled = false;
                MiddleRemark.Enabled = false;
                MiddleDeliCode.Enabled = false;

            }
            if (CommonClass.UserInfo.companyid == "486")
            {
                this.AccMiddlePay.Properties.ReadOnly = true;
            }




        }

        private void getDelivery()
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_DELIVERY", new List<SqlPara> { new SqlPara("SiteName", CommonClass.UserInfo.SiteName), new SqlPara("WebName", CommonClass.UserInfo.WebName) }));
            if (ds == null || ds.Tables.Count == 0) return;
            gcjiehuodanhao.DataSource = ds.Tables[0];
        }

        private void modify()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", _BillNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                AccMiddlePayState = ConvertType.ToString(dr["AccMiddlePayState"]);
                BillDate.EditValue = ConvertType.ToDateTime(dr["BillDate"]);
                BillNo1.EditValue = ConvertType.ToString(dr["BillNo"]);
                StartSite.EditValue = ConvertType.ToString(dr["StartSite"]);
                DestinationSite.EditValue = ConvertType.ToString(dr["DestinationSite"]);
                ConsignorCompany.EditValue = ConvertType.ToString(dr["ConsignorCompany"]);
                ConsignorName.EditValue = ConvertType.ToString(dr["ConsignorName"]);
                ConsignorCellPhone.EditValue = ConvertType.ToString(dr["ConsignorCellPhone"]);
                ConsignorPhone.EditValue = ConvertType.ToString(dr["ConsignorPhone"]);
                ConsigneeCompany.EditValue = ConvertType.ToString(dr["ConsigneeCompany"]);
                ConsigneeName.EditValue = ConvertType.ToString(dr["ConsigneeName"]);
                ConsigneeCellPhone.EditValue = ConvertType.ToString(dr["ConsigneeCellPhone"]);
                ConsigneePhone.EditValue = ConvertType.ToString(dr["ConsigneePhone"]);
                Varieties.EditValue = ConvertType.ToString(dr["Varieties"]);
                Package.EditValue = ConvertType.ToString(dr["Package"]);
                Num.EditValue = ConvertType.ToInt32(dr["Num"]);
                Weight.EditValue = ConvertType.ToDecimal(dr["Weight"]);
                Volume.EditValue = ConvertType.ToDecimal(dr["Volume"]);
                NowPay.EditValue = ConvertType.ToDecimal(dr["NowPay"]);
                FetchPay.EditValue = ConvertType.ToDecimal(dr["FetchPay"]);
                ReceiptPay.EditValue = ConvertType.ToDecimal(dr["ReceiptPay"]);
                MonthPay.EditValue = ConvertType.ToDecimal(dr["MonthPay"]);
                SupportValue.EditValue = ConvertType.ToDecimal(dr["SupportValue"]);
                CollectionPay.EditValue = ConvertType.ToDecimal(dr["CollectionPay"]);
                DiscountTransfer.EditValue = ConvertType.ToDecimal(dr["DiscountTransfer"]);
                ReceiptCondition.EditValue = ConvertType.ToString(dr["ReceiptCondition"]);
                BillRemark.EditValue = ConvertType.ToString(dr["BillRemark"]);
                MiddleDate.DateTime = CommonClass.gcdate;
                DeliFee.EditValue = ConvertType.ToDecimal(dr["DeliFee"]); //ljp 补上送货费信息
                CusOderNo.EditValue = ConvertType.ToString(dr["CusOderNo"]);
                ShortOwePay.EditValue = ConvertType.ToDecimal(dr["ShortOwePay"]);
                BefArrivalPay.EditValue = ConvertType.ToDecimal(dr["BefArrivalPay"]);

                if (ismodify)
                {
                    MiddleDate.EditValue = ConvertType.ToDateTime(dr["MiddleDate"]);
                    MiddleCarrier.EditValue = ConvertType.ToString(dr["MiddleCarrier"]);
                    MiddleStartSitePhone.EditValue = ConvertType.ToString(dr["MiddleStartSitePhone"]);
                    MiddleEndSitePhone.EditValue = ConvertType.ToString(dr["MiddleEndSitePhone"]);
                    MiddleFetchAddress.EditValue = ConvertType.ToString(dr["MiddleFetchAddress"]);
                    MiddleBillNo.EditValue = ConvertType.ToString(dr["MiddleBillNo"]);
                    if (ConvertType.ToInt32(dr["MiddleFeePaymentState"]) > 0) radioButton1.Select();
                    else radioButton2.Select();
                    MiddlePackageFee.EditValue = ConvertType.ToDecimal(dr["MiddlePackageFee"]);
                    MiddleHandleFee.EditValue = ConvertType.ToDecimal(dr["MiddleHandleFee"]);
                    MiddleForkliftFee.EditValue = ConvertType.ToDecimal(dr["MiddleForkliftFee"]);
                    MiddleOtherFee.EditValue = ConvertType.ToDecimal(dr["MiddleOtherFee"]);
                    MiddleFreight.EditValue = ConvertType.ToDecimal(dr["MiddleFreight"]);
                    MiddleOtherReason.EditValue = ConvertType.ToString(dr["MiddleOtherReason"]);
                    MiddleRemark.EditValue = ConvertType.ToString(dr["MiddleRemark"]);
                    MiddleBatch.EditValue = ConvertType.ToString(dr["MiddleBatch"]);
                    MiddleDeliFee.EditValue = ConvertType.ToDecimal(dr["MiddleDeliFee"]);
                    MiddleSendFee.EditValue = ConvertType.ToDecimal(dr["MiddleSendFee"]);
                    MiddleDeliCode.EditValue = dr["MiddleDeliCode"];
                    MiddleCarNo.EditValue = dr["MiddleCarNo"];
                    MiddleChauffer.EditValue = dr["MiddleChauffer"];
                    MiddleChaufferPhone.EditValue = dr["MiddleChaufferPhone"];
                    MiddleSiteName = ConvertType.ToString(dr["MiddleSiteName"]);//中转站点
                    MiddleWebName = ConvertType.ToString(dr["MiddleWebName"]);//中转网点
                    AccMiddlePay.EditValue = ConvertType.ToDecimal(dr["AccMiddlePay"]);
                    CusOderNo.EditValue = ConvertType.ToString(dr["CusOderNo"]);
                }
                else
                {
                    MiddleDate.DateTime = CommonClass.gcdate;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        public bool isDeficit(decimal Freight, decimal AccMiddlePay)
        {
            try
            {
                bool ischeck = false;
                Freight = (Freight == 0) ? 1 : Freight;
                costrate = Math.Round(AccMiddlePay/Freight, 2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleWebName", CommonClass.UserInfo.WebName.ToString().Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_3", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    if (costrate > ConvertType.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"].ToString()))
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



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.companyid == "485")
            {

                if (isDeficit(Freight, ConvertType.ToDecimal(AccMiddlePay.Text)))
                {
                    frmIsCostDeficits Cost = new frmIsCostDeficits();
                    Cost.DepartureBatch = BillNo1.Text.Trim();
                    Cost.MiddleWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                    Cost.actual_rate = costrate;
                    Cost.MenuType = "单票中转亏损";
                    Cost.ShowDialog();
                    if (Cost.isprint == true)
                    {
                        save(false);
                    }

                }
                else
                {
                    save(false);
                }
            }

            else
            {
                save(false);
            }
        }


        private bool Issign(string billno)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO", list);
            DataSet ds1 = SqlHelper.GetDataSet(sps);
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return false;
            DataRow dr = ds1.Tables[0].Rows[0];
            if (dr["BillState"].ToString() == "16")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="canprint">是否打印</param>
        private void save(bool canprint)
        {
            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("BillNo", _BillNo));

            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO", list1);
            DataSet ds1 = SqlHelper.GetDataSet(sps1);
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;

            DataRow dr = ds1.Tables[0].Rows[0];

            decimal middleOtherFee = ConvertType.ToDecimal(MiddleOtherFee.Text.Trim().ToString());       //其他费zb20190611
            decimal middleFreight = ConvertType.ToDecimal(MiddleFreight.Text.Trim().ToString());  //运费
            decimal middleHandleFee = ConvertType.ToDecimal(MiddleHandleFee.Text.Trim().ToString());  //装卸费
            decimal middleDeliFee = ConvertType.ToDecimal(MiddleDeliFee.Text.Trim().ToString());       //送货费
            decimal accMiddlePay = ConvertType.ToDecimal(AccMiddlePay.Text.Trim().ToString());     //应付中转费
            decimal middleSendFee = ConvertType.ToDecimal(MiddleSendFee.Text.Trim().ToString());  //转送费
            
            //zb20190611
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")
            {
                if (middleOtherFee != ConvertType.ToDecimal(dr["MiddleOtherFee"]) || middleHandleFee != ConvertType.ToDecimal(dr["MiddleHandleFee"]) || middleFreight != ConvertType.ToDecimal(dr["MiddleFreight"])
                    || middleDeliFee != ConvertType.ToDecimal(dr["MiddleDeliFee"]) || accMiddlePay != ConvertType.ToDecimal(dr["AccMiddlePay"]) || middleSendFee != ConvertType.ToDecimal(dr["MiddleSendFee"]))
                {
                    if (_MiddleAduitState == "已审核")
                    {
                        MsgBox.ShowOK("已审核不可取消单票，如需修改请先反审核！");
                        return;
                    }
                }
            }

            string middleCarrier = MiddleCarrier.Text.Trim();

            if (AccMiddlePayState == "1")
            {
                MsgBox.ShowOK("中转费已核销!不允许修改!");
                return;
            }
            if (middleCarrier == "")
            {
                MsgBox.ShowOK("请填写承运公司!");
                MiddleCarrier.Focus();
                return;
            }

           
            decimal mSendFee = ConvertType.ToDecimal(MiddleSendFee.Text);//转送费
            if (mSendFee > 0)
            {
                if (MiddleCarNo.Text.Trim() == "" || MiddleChauffer.Text.Trim() == "")
                {
                    MsgBox.ShowOK("填写了转送费，必须填写车号和司机!");
                    MiddleCarNo.Focus();
                    return;
                }
            }
           
            if (Math.Abs((CommonClass.gcdate - MiddleDate.DateTime).TotalDays) > 30)
            {
                if (MsgBox.ShowYesNo("中转时间跟当前时间超过30天,确定保存？") != DialogResult.Yes) return;
            }

            if (MsgBox.ShowYesNo("确定保存吗？") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNos", BillNo + "@"));//0
            list.Add(new SqlPara("MiddleDate", MiddleDate.DateTime));
            list.Add(new SqlPara("MiddleCarrier", MiddleCarrier.Text.Trim()));
            list.Add(new SqlPara("MiddleStartSitePhone", MiddleStartSitePhone.Text.Trim()));
            list.Add(new SqlPara("MiddleEndSitePhone", MiddleEndSitePhone.Text.Trim()));
            list.Add(new SqlPara("MiddleBillNos", MiddleBillNo.Text.Trim() + "@"));//5
            if (CommonClass.UserInfo.SiteName.Equals("总部"))
            {
                if (string.IsNullOrEmpty(MiddleSiteName) || MiddleSiteName == "")
                {
                    list.Add(new SqlPara("MiddleSiteName", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("MiddleWebName", CommonClass.UserInfo.WebName));
                }
                else
                {
                    list.Add(new SqlPara("MiddleSiteName", MiddleSiteName));
                    list.Add(new SqlPara("MiddleWebName", MiddleWebName));
                }
            }
            else
            {
                list.Add(new SqlPara("MiddleSiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("MiddleWebName", CommonClass.UserInfo.WebName));
            }

            list.Add(new SqlPara("MiddleType", gettype));
            list.Add(new SqlPara("AccMiddlePays", ConvertType.ToDecimal(AccMiddlePay.Text) + "@"));
            list.Add(new SqlPara("MiddleFeePaymentStates", (radioButton1.Checked ? 1 : 0) + "@"));//10
            list.Add(new SqlPara("MiddleFetchAddress", MiddleFetchAddress.Text.Trim()));
            list.Add(new SqlPara("MiddleOperator", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("MiddleRemark", MiddleRemark.Text.Trim()) );
            list.Add(new SqlPara("MiddlePackageFees", ConvertType.ToDecimal(MiddlePackageFee.Text) + "@"));
            list.Add(new SqlPara("MiddleHandleFees", ConvertType.ToDecimal(MiddleHandleFee.Text) + "@"));//15
            list.Add(new SqlPara("MiddleForkliftFees", ConvertType.ToDecimal(MiddleForkliftFee.Text) + "@"));
            list.Add(new SqlPara("MiddleOtherFees", ConvertType.ToDecimal(MiddleOtherFee.Text) + "@"));
            list.Add(new SqlPara("MiddleFreights", ConvertType.ToDecimal(MiddleFreight.Text) + "@"));
            list.Add(new SqlPara("MiddleOtherReason", MiddleOtherReason.Text.Trim()));
            list.Add(new SqlPara("MiddleBatch", MiddleBatch.Text.Trim()));//20
            list.Add(new SqlPara("MiddleCauseName", CommonClass.UserInfo.CauseName));
            list.Add(new SqlPara("MiddleAreaName", CommonClass.UserInfo.AreaName));
            list.Add(new SqlPara("MiddleDepName", CommonClass.UserInfo.DepartName));
            list.Add(new SqlPara("MiddleSendFees", ConvertType.ToDecimal(MiddleSendFee.Text) + "@"));
            list.Add(new SqlPara("MiddleDeliFees", ConvertType.ToDecimal(MiddleDeliFee.Text) + "@"));
            list.Add(new SqlPara("MiddleDeliCodes", MiddleDeliCode.Text.Trim() + "@"));//25
            list.Add(new SqlPara("MiddleCarNos", MiddleCarNo.Text.Trim() + "@"));
            list.Add(new SqlPara("MiddleChauffers", MiddleChauffer.Text.Trim() + "@"));
            list.Add(new SqlPara("MiddleChaufferPhones", MiddleChaufferPhone.Text.Trim() + "@"));
            list.Add(new SqlPara("MiddleBackFees", ConvertType.ToDecimal(MiddleBackFee.Text) + "@"));//29
            list.Add(new SqlPara("opertype", ismodify ? 2 : 1));//1新增  2修改
            list.Add(new SqlPara("CS_AccMiddlePay", CS_AccMiddlePay));//hj20180724初始中转费 用来比较修改的值

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MIDDLE_NEW", list);
            if (SqlHelper.ExecteNonQuery(sps) == 0) return;
            if (_gv != null) _gv.DeleteSelectedRows();
            if (canprint)
            {
                if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
                {
                    frmPrintRuiLang fpr = new frmPrintRuiLang("中转清单(项目部)", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT", new List<SqlPara> { new SqlPara("MiddleBatch", BillNo1.Text.Trim()) })));
                    fpr.ShowDialog();
                }
                else
                {
                    string middlelist = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;  //maohui20180324
                    if (File.Exists(Application.StartupPath + "\\Reports\\" + middlelist + "per.grf"))
                    {
                        middlelist = middlelist + "per";
                    }
                    frmPrintRuiLang fpr = new frmPrintRuiLang(middlelist, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT_TX_DP", new List<SqlPara> { new SqlPara("MiddleBatch", BillNo1.Text.Trim()) })));
                    fpr.ShowDialog();
                }
            }

            MsgBox.ShowOK("中转完成!");
            //List<SqlPara> listQuery = new List<SqlPara>();
            //listQuery.Add(new SqlPara("BillNos", BillNo+"@"));
            //SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ZQTMSMiddleSYS", listQuery);
            //DataSet ds = SqlHelper.GetDataSet(spsQuery);
            //if (ds == null || ds.Tables[0].Rows.Count == 0) return;
            //string dsJson = JsonConvert.SerializeObject(ds);
            //RequestModel<string> request = new RequestModel<string>();
            //request.Request = dsJson;
            //request.OperType = 0;
            //string json = JsonConvert.SerializeObject(request);
            ////string url = "http://localhost:42936/KDLMSService/ZQTMSMiddleSys";
            //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSMiddleSys";

            //ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
            //if (model.State != "200")
            //{
            //    MsgBox.ShowOK(model.Message);
            //}
            //MiddleSyn(BillNo + "@");

            #region //跟踪节点信息同步接口 (中转)
            {
                List<SqlPara> lists = new List<SqlPara>();
                lists.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo1", lists);
                DataSet dds = SqlHelper.GetDataSet(spsa);
                DataRow ddr = dds.Tables[0].Rows[0];

                if (ddr["BegWeb"].ToString() == "三方")
                {
                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    hashMap.Add("carriageSn", _BillNo);
                    hashMap.Add("orderStatusCode", Convert.ToInt32(9));
                    hashMap.Add("traceRemarks", "中转");
                    string json = JsonConvert.SerializeObject(hashMap);
                    string url = "http://120.76.141.227:9882/umsv2.biz/open/track/record_trunk_order_status";
                    try
                    {
                        HttpHelper.HttpPostJava(json, url);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            #endregion                           


            this.Close();
           // CommonSyn.MiddleSyn(BillNo+"@");//zaj 2018-4-10
            string type1 = gettype.ToString();
            CommonSyn.MIDDLE_NEW_SYN(BillNo + "@", MiddleBillNo.Text.Trim() + "@", type1);//yzw 中转同步新
           // CommonSyn.TimeOtherUptSyn((BillNo + "@"), "", CommonClass.UserInfo.WebName, "", "", "", CommonClass.UserInfo.WebName, "USP_ADD_MIDDLE_NEW", "");//同步其他修改时效 LD 2018-4-27
           //CommonSyn.TraceSyn(null, BillNo + "@", gettype == 0 ? 3 : 8, gettype == 0 ? "本地中转" : "终端中转", 1, null, null);
            
        }

        //中转同步 2018-4-9 zaj
        //private void MiddleSyn(string BillNos)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNos", BillNos));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ZQTMSMiddleSYS", listQuery);
        //        DataSet ds = SqlHelper.GetDataSet(spsQuery);
        //        if (ds == null || ds.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(ds);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //        //string url = "http://localhost:42936/KDLMSService/ZQTMSMiddleSys";
        //       // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSMiddleSys";
        //        string url = HttpHelper.urlMiddleSyn;


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

        private void MiddleCarrier_Enter(object sender, EventArgs e)
        {
            myGridControl1.Visible = true;
            myGridControl1.Location = new Point(224, 86);
        }

        private bool HasApply(string sBillNo)
        {
            bool bl = false;
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", sBillNo));
                list1.Add(new SqlPara("ApplyType", "控货/放货"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    bl = false;
                else
                    bl = true;
            }
            catch
            {
                bl = false;
            }
            return bl;
        }
        private void MiddleCarrier_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl1.Focus();
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
            }
        }

        private void MiddleCarrier_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = myGridControl1.Focus();
        }

        private void myGridControl1_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = MiddleCarrier.Focused;
        }

        private void myGridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SetBasCarrierUnit();
        }

        private void SetBasCarrierUnit()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(rowhandle);
            MiddleCarrier.EditValue = dr["CompanyName"];
            MiddleStartSitePhone.EditValue = dr["SalesCellPhone"];
            MiddleEndSitePhone.EditValue = dr["SalesPhone"];
            MiddleFetchAddress.EditValue = dr["ArriveAddress"];
            myGridControl1.Visible = false;
        }

        private void CalculateMiddleFee(object sender, EventArgs e)
        {
            AccMiddlePay.Text = (ConvertType.ToDecimal(MiddleFreight.Text) + ConvertType.ToDecimal(MiddleDeliFee.Text) + ConvertType.ToDecimal(MiddleOtherFee.Text) + ConvertType.ToDecimal(MiddleHandleFee.Text)).ToString();
        }

        private void getCarrier()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCARRIERUNIT_Ex", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetBasCarrierUnit();
        }

        private void MiddleCarrier_EditValueChanged(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["CompanyName"].FilterInfo = new ColumnFilterInfo("[CompanyName] LIKE " + "'%" + e.NewValue.ToString() + "%'", "");
            }
            else
            {
                myGridView1.ClearColumnsFilter();
            }
        }

        private void MiddleDeliCode_Enter(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = true;
            gcjiehuodanhao.Location = new Point(170, 77);
        }

        private void MiddleDeliCode_Leave(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = gcjiehuodanhao.Focused;
        }

        private void MiddleDeliCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e == null) return;
            string s = ConvertType.ToString(e.NewValue);
            if (s == "") gridView5.ClearColumnsFilter();
            else gridColumn7.FilterInfo = new ColumnFilterInfo("DeliCode LIKE '%" + e.NewValue + "%'");
        }

        private void MiddleDeliCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) gcjiehuodanhao.Focus();
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            SetDeliCode();
        }

        private void SetDeliCode()
        {
            int rowhandle = gridView5.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = gridView5.GetDataRow(rowhandle);
            if (dr == null) return;
            MiddleDeliCode.EditValue = dr["DeliCode"];
            MiddleSendFee.EditValue = dr["VehFare"];
            MiddleCarNo.EditValue = dr["VehicleNum"];
            MiddleChauffer.EditValue = dr["DriverName"];
            MiddleChaufferPhone.EditValue = dr["DeliCusPhone"];
            gcjiehuodanhao.Visible = false;
        }

        private void gcjiehuodanhao_Leave(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = MiddleDeliCode.Focused;
        }

        private void AccMiddlePay_EditValueChanged(object sender, EventArgs e)
        {
            MiddleBackFee.Text = (ConvertType.ToDecimal(FetchPay.Text) - ConvertType.ToDecimal(AccMiddlePay.Text)).ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.companyid == "485")
            {

                if (isDeficit(Freight, ConvertType.ToDecimal(AccMiddlePay.Text)))
                {
                    frmIsCostDeficits Cost = new frmIsCostDeficits();
                    Cost.DepartureBatch = BillNo1.Text.Trim();
                    Cost.MiddleWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                    Cost.actual_rate = costrate;
                    Cost.MenuType = "单票中转亏损";
                    Cost.ShowDialog();
                    if (Cost.isprint == true)
                    {
                        save(true);
                    }

                }
                else
                {
                    save(true);
                }
            }
            else
            {
                save(true);
            }
        }
    }
}