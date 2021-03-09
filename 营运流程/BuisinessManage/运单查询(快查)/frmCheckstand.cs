using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using Newtonsoft.Json;
using ZQTMS.Common;
using Newtonsoft.Json.Linq;
using ZQTMS.SqlDAL;
using System.Collections;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ZQTMS.UI
{
    public partial class frmCheckstand : BaseForm
    {
        private ScanerHook listener = new ScanerHook();
        private string code = "";
        public int type = 0;//1运单收款0其他收款
        public string billNo = "";//运单号
        public string paymentAmout = "";//应收金额
        bool flag = false;
        public frmCheckstand()
        {
            InitializeComponent();
            listener.ScanerEvent += Listener_ScanerEvent;  
        }
     

        private void frmCheckstand_Load(object sender, EventArgs e)
        {
            listener.Start();
            txtBSumMoney.Properties.ReadOnly = true;
            txtBOperMan.Properties.ReadOnly = true;
            txtBWebName.Properties.ReadOnly = true;
            txtBAreaName.Properties.ReadOnly = true;

            txtOSummoney.Properties.ReadOnly = true;
            txtOOperman.Properties.ReadOnly = true;
            txtOWebName.Properties.ReadOnly = true;
            txtOAreaName.Properties.ReadOnly = true;

            txtBOperMan.Text = CommonClass.UserInfo.UserName;
            txtBWebName.Text = CommonClass.UserInfo.WebName;
            txtBAreaName.Text = CommonClass.UserInfo.AreaName;

            txtOOperman.Text = CommonClass.UserInfo.UserName;
            txtOWebName.Text = CommonClass.UserInfo.WebName;
            txtOAreaName.Text = CommonClass.UserInfo.AreaName;
            if (type == 1 )
            {
                xtraTabControl1.SelectedTabPage = tpByBillNo;
                
                txtBBillno.Properties.ReadOnly = true;
               


                txtBBillno.Text = billNo;
                txtBSumMoney.Text = paymentAmout;
                txtBAccMoney.Focus();
                this.ActiveControl = txtBAccMoney;
               

                xtraTabControl1.TabPages.Remove(tpByOther);
            }
            if (type == 0 )
            {
                xtraTabControl1.SelectedTabPage =tpByOther;
                if (xtraTabControl1.SelectedTabPage == tpByOther)
                {
                    txtBBillno.Properties.ReadOnly = true;
                    txtOCompany.Properties.ReadOnly = false;
                    txtBBillno.Text = "";
                    txtOCompany.Focus();
                    this.ActiveControl = txtOCompany;
                    //txtOSummoney.Properties.ReadOnly = true;
                    //txtOOperman.Properties.ReadOnly = true;
                    //txtOWebName.Properties.ReadOnly = true;
                    //txtOAreaName.Properties.ReadOnly = true;
                }
                if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                {
                    txtBBillno.Properties.ReadOnly = false;
                       txtOCompany.Properties.ReadOnly=true;
                       txtOCompany.Text = "";
                       txtBBillno.Focus();
                       this.ActiveControl = txtBBillno;
                   
                }
            
            }
            txtBCode.Properties.ReadOnly = true;
            txtOCode.Properties.ReadOnly = true;
        }
        private void Listener_ScanerEvent(ScanerHook.ScanerCodes codes)
        {
            code = codes.Result;
            if (xtraTabControl1.SelectedTabPage == tpByBillNo)
            {

                txtBCode.Focus();
                this.ActiveControl = txtBCode;
                txtBCode.Text = code;

            }
            if (xtraTabControl1.SelectedTabPage == tpByOther)
            {
                txtOCode.Focus();
                this.ActiveControl = txtOCode;
                txtOCode.Text = code;

            }
            if (code != "" && code!="ac")
            {
                if (!flag)
                {
                    //lblBMsg.Text = "输入金额后，请按回车!";
                    showMsg("输入金额后，请按回车!");
                    return;
                }
                try
                {
                    string billno = "";
                    string sumMoney = "";
                    string accMoney = "";
                    string operMan = "";
                    string webName = "";
                    string areaName = "";
                    string companyName = "";
                    if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                    {
                        billno = txtBBillno.Text.Trim();
                        sumMoney = txtBSumMoney.Text.Trim();
                        accMoney = txtBAccMoney.Text.Trim();
                        operMan = txtBOperMan.Text.Trim();
                        webName = txtBWebName.Text.Trim();
                        areaName = txtBAreaName.Text.Trim();
                        if (billno == "")
                        {
                            //MsgBox.ShowOK("运单号不能为空!");
                            //lblBMsg.Text = "运单号不能为空!";
                            showMsg("运单号不能为空!");
                            return;
                        }
                        if (sumMoney == "")
                        {
                            //lblBMsg.Text = "应收金额不能为空!";
                            showMsg("应收金额不能为空!");
                            return;

                        }
                    }
                    if (xtraTabControl1.SelectedTabPage == tpByOther)
                    {
                        companyName = txtOCompany.Text.Trim();
                        sumMoney = txtOSummoney.Text.Trim();
                        accMoney = txtOAccMoney.Text.Trim();
                        operMan = txtOOperman.Text.Trim();
                        webName = txtOWebName.Text.Trim();
                        areaName = txtOAreaName.Text.Trim();
                        if (companyName == "")
                        {
                            //lblBMsg.Text = "公司/客户名不能为空!";
                            showMsg("付款说明不能为空!");
                            return;
                        }
                    }


                    if (Convert.ToDecimal(accMoney) > 50000)
                    {
                        // MsgBox.ShowError("付款金额不能大于50000！");
                        // lblBMsg.Text = "付款金额不能大于50000!";
                        showMsg("付款金额不能大于50000!");
                        return;
                    }
                    if (decimal.Round(Convert.ToDecimal(accMoney), 2) < (decimal)0.02)
                    {
                        //  MsgBox.ShowError("付款金额必须大于0！");
                        lblBMsg.Text = "付款金额必须大于0.01！";
                        return;
                    }
                    //if (type == 1)
                    //{                       
                    //}
                    //if (type == 0)
                    //{                                           
                    //}
                    int paytype = 0,subpaytype = 0;;
                    string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
                    string strDate = System.DateTime.Now.ToString("yyyyMMdd");//20190706
                    string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
                    string startCode = code.Substring(0, 2);
                    string totalAmount = accMoney; //decimal.Round(decimal.Parse(accMoney), 2).ToString().ToString();
                    string[] weixinScodes = { "10", "11", "12", "13", "14", "15" };//微信
                    string[] zhifubaoScodes = { "25", "26", "27", "28", "29", "30" };//支付宝
                    string serialNum = "";//20190706
                    //微信条码规则：用户付款码条形码规则：18位纯数字，以10、11、12、13、14、15开头
                    //支付宝条码规则：付款码将由原来的28开头扩充到25-30开头，长度由原来的16-18位扩充到16-24位。
                    if (((IList)weixinScodes).Contains(startCode))
                    {
                        if (code.Length != 18)
                        {

                            // MsgBox.ShowOK("条码不正确，请重新扫描!");
                            // lblBMsg.Text = "条码不正确，请重新扫描!";
                            showMsg("条码不正确，请重新扫描!");
                            return;
                        }
                        //20190705 判断银联支付和快捷支付
                        if ((xtraTabControl1.SelectedTabPage == tpByBillNo && txtBPayMode.Text.Trim() == "银联支付") || (xtraTabControl1.SelectedTabPage == tpByOther && txtOPayMode.Text.Trim() == "银联支付"))
                        {
                            paytype = 13;
                            subpaytype = 17;
                        }

                        if ((xtraTabControl1.SelectedTabPage == tpByBillNo && txtBPayMode.Text.Trim() == "快捷支付") || (xtraTabControl1.SelectedTabPage == tpByOther && txtOPayMode.Text.Trim() == "快捷支付"))
                        {
                            paytype = 17;//微信条码
                        }
                        
                        //paytype = 17;//微信条码
                    }
                    if (((IList)zhifubaoScodes).Contains(startCode))
                    {
                        if (code.Length < 18 || code.Length > 24)
                        {
                            // MsgBox.ShowOK("条码不正确，请重新扫描!");
                            // lblBMsg.Text = "条码不正确，请重新扫描!";
                            showMsg("条码不正确，请重新扫描!");
                            return;
                        }
                        if ((xtraTabControl1.SelectedTabPage == tpByBillNo && txtBPayMode.Text.Trim() == "银联支付") || (xtraTabControl1.SelectedTabPage == tpByOther && txtOPayMode.Text.Trim() == "银联支付"))
                        {
                            paytype = 12;
                            subpaytype = 16;
                        }
                        if ((xtraTabControl1.SelectedTabPage == tpByBillNo && txtBPayMode.Text.Trim() == "快捷支付") || (xtraTabControl1.SelectedTabPage == tpByOther && txtOPayMode.Text.Trim() == "快捷支付"))
                        {
                            paytype = 18;//支付宝条码
                        }
                        //paytype = 18;//支付宝条码
                    }
                    if (paytype == 0)
                    {
                        // MsgBox.ShowOK("条码不正确，请重新扫描!");
                        //lblBMsg.Text = "条码不正确，请重新扫描!";
                        showMsg("条码不正确，请重新扫描!");
                        return;

                    }
                    //20170705
                    if (paytype == 12)
                    {
                        orderCode = orderCode + "1";
                    }
                    else if (paytype == 13)
                    {
                        orderCode = orderCode + "2";
                    }
                    if (paytype == 17)
                    {
                        orderCode = orderCode + "6";
                    }
                    if (paytype == 18)
                    {
                        orderCode = orderCode + "7";
                    }
                    string url = "";
                    if (paytype == 12 || paytype == 13)//银联的单
                    {
                        url = "http://localhost:8090/UnionQrPay"; //本地测试
                        //将金额转为以分为单位格式
                         totalAmount = Convert.ToInt32(decimal.Round(decimal.Parse(accMoney), 2) * 100).ToString().ToString();
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("paymenttypeid", paytype);
                        dic.Add("subpaymenttypeid", subpaytype);
                        dic.Add("authcode", codes.Result.Trim());
                        dic.Add("body", "运单收款");
                        dic.Add("amount", totalAmount);
                        dic.Add("businesstype", "1001");
                        dic.Add("ordercode", orderCode);
                        dic.Add("belongarea", "西南分区");
                        //报文体json

                        string data = JsonConvert.SerializeObject(dic);
                        string responseString = HttpPost(url, System.Web.HttpUtility.UrlEncode(data, System.Text.Encoding.UTF8), Encoding.UTF8);
                        JObject jo = (JObject)JsonConvert.DeserializeObject(responseString);
                        if (jo["response"].ToString().Equals("0000"))
                        {
                            string datas = jo["data"].ToString();
                            if (datas == null || datas.Equals("")) { MsgBox.ShowError("收款异常"); return; }
                            JObject obj = (JObject)JsonConvert.DeserializeObject(datas);
                            string respcode = obj["respcode"].ToString();
                            serialNum = obj["payorderid"].ToString();
                            string businesstime = obj["businesstime"].ToString();//交易时间
                            if (respcode.Equals("2"))
                            {
                                MsgBox.ShowOK("付款成功！");
                            }
                            else
                            {
                                MsgBox.ShowOK("支付中，查看支付记录！");
                            }
                            //插入充值记录
                            List<SqlPara> list = new List<SqlPara>();
                            list.Add(new SqlPara("accountNo", ""));
                            list.Add(new SqlPara("accountName", webName));
                            list.Add(new SqlPara("custPhone", ""));
                            list.Add(new SqlPara("payMode", "运单收款"));
                            list.Add(new SqlPara("inkey", ""));//接口验证码
                            list.Add(new SqlPara("paySysDate", strDate));
                            list.Add(new SqlPara("serialNum", serialNum));
                            list.Add(new SqlPara("merNo", "703530785"));//
                            list.Add(new SqlPara("merOrderId", orderCode));
                            list.Add(new SqlPara("merOrderName", ""));
                            list.Add(new SqlPara("merShortName", ""));
                            list.Add(new SqlPara("price", 0));
                            list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(accMoney).ToString()));
                            list.Add(new SqlPara("merOrderCount", "1"));
                            list.Add(new SqlPara("payType", subpaytype));//支付类型
                            list.Add(new SqlPara("salePayAcct", ""));
                            list.Add(new SqlPara("custPayAcct", ""));
                            list.Add(new SqlPara("merOrderStatus", respcode));
                            list.Add(new SqlPara("payUrl", code));
                            list.Add(new SqlPara("Errorcode", billno));

                            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_UNIONPAY", list));
                        }
                        else
                        {
                            MsgBox.ShowError(jo["message"].ToString());
                        }


                    }
                    if (paytype == 17 || paytype == 18)//传化的单
                    {
                         //url = "http://localhost:12345/BarCodePayService/BarCodePay";
                        url = "http://ZQTMS.dekuncn.com:8012/BarCodePayService/BarCodePay";
                         totalAmount = accMoney;
                         string payType = "";
                            if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                            {
                                payType = "运单收款";

                            }
                            else
                            {
                                payType = "其他收款";
                            }
                            string subject = "";
                        if(payType=="运单收款")
                        {
                            subject = CommonClass.UserInfo.WebName+ ":运单收款:" + billno+":"+paytype+":"+CommonClass.UserInfo.companyid;
                        }
                        if(payType=="其他收款")
                        {
                            subject = CommonClass.UserInfo.WebName + ":其他收款" + companyName+":"+paytype+":"+CommonClass.UserInfo.companyid;
                        }
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        if (CommonClass.UserInfo.companyid == "485")
                        {
                            string account_number = "8800011713565";//"8800011619379";//钱包新增参数，此参数不同系统对应不同参数，系统会根据这个参数给操作不同数据库
                            //string dog_sk = "5O67GaYQ60w318JBN4b8";//密文
                            //string toaccountnumber = "8800010082679";//账号
                            //dic.Add("paytype", paytype);//支付类型14支付宝扫描15微信扫码16收银台
                            dic.Add("businessnumber", orderCode);//订单号
                            dic.Add("transactionamount", totalAmount);//金额
                            //dic.Add("appid", appid);//商户号
                            dic.Add("account_number", account_number);//商户号
                            dic.Add("authcode", code);
                            dic.Add("paytype", paytype);
                            dic.Add("companyid", "485");
                            dic.Add("subject", subject);//运单收款需要按照此规则传值，以便回调时根据运单号回写数据 zaj 2019-11-12
                         
                        }

                        else if (CommonClass.UserInfo.companyid == "486")
                        {
                            string account_number = "8800012015693";//"8800011619379";//钱包新增参数，此参数不同系统对应不同参数，系统会根据这个参数给操作不同数据库
                            //string dog_sk = "5O67GaYQ60w318JBN4b8";//密文
                            //string toaccountnumber = "8800010082679";//账号
                            //dic.Add("paytype", paytype);//支付类型14支付宝扫描15微信扫码16收银台
                            dic.Add("businessnumber", orderCode);//订单号
                            dic.Add("transactionamount", totalAmount);//金额
                            //dic.Add("appid", appid);//商户号
                            dic.Add("account_number", account_number);//商户号
                            dic.Add("authcode", code);
                            dic.Add("paytype", paytype);
                            dic.Add("companyid", "486");
                            dic.Add("subject", subject);//运单收款需要按照此规则传值，以便回调时根据运单号回写数据 zaj 2019-11-12


                        }
                        string requestStr = JsonConvert.SerializeObject(dic);
                        RequestModel<string> request = new RequestModel<string>();
                        request.Request = requestStr;
                        request.OperType = 0;
                        string json = JsonConvert.SerializeObject(request);
                        ResponseModelClone<string> result = HttpHelper.HttpPost(json, url);
                        if (result.State == "200")
                        {
                            JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                            string biz_code = "";
                            if (jo.Property("biz_code") != null)
                            {
                                biz_code = jo["biz_code"].ToString();
                            }
                            string Repcode = "";
                            if (jo.Property("code") != null)
                            {
                                Repcode = jo["code"].ToString();
                            }

                           
                            string businessrecordnumber = "";
                            if (biz_code == "GPBIZ_00" && Repcode == "GP_00")
                            {
                                string data = "";
                                if (jo.Property("data") != null)
                                {
                                    data = jo["data"].ToString();
                                }
                                if (!string.IsNullOrEmpty(data))
                                {
                                    JObject joData = JsonConvert.DeserializeObject<JObject>(data);

                                    if (joData.Property("businessrecordnumber") != null)
                                    {
                                        businessrecordnumber = joData["businessrecordnumber"].ToString();
                                    }

                                }
                                string errorcode = "";
                                if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                                {
                                    errorcode = billno;
                                }
                                else
                                {
                                    errorcode = companyName;
                                }
                                #region  test code
                                int times=0;
                                do
                                {
                                    //int a = 1;
                                    Dictionary<string, object> dicQ = new Dictionary<string, object>();
                                    dicQ.Add("businessnumber", orderCode);//订单号
                                    string requestQstr= JsonConvert.SerializeObject(dicQ);
                                    RequestModel<string> requestQ = new RequestModel<string>();
                                    requestQ.Request = requestQstr;
                                    requestQ.OperType = 0;
                                    string jsonQ = JsonConvert.SerializeObject(requestQ);
                                    string urlQ = "http://ZQTMS.dekuncn.com:8012/BarCodePayService/BarCodeShareOrderQuery";
                                   // string urlQ = "http://localhost:12345/BarCodePayService/BarCodeShareOrderQuery";

                                    ResponseModelClone<string> resultQ = HttpHelper.HttpPost(jsonQ, urlQ);
                                    JObject joOne = (JObject)JsonConvert.DeserializeObject(resultQ.Result);
                                    string biz_codeOne = "";
                                    if (joOne.Property("biz_code") != null)
                                    {
                                        biz_codeOne = joOne["biz_code"].ToString();
                                    }
                                    string RepcodeOne = "";
                                    if (joOne.Property("code") != null)
                                    {
                                        RepcodeOne = joOne["code"].ToString();
                                    }
                                    if (biz_codeOne == "GPBIZ_00" && RepcodeOne =="GP_00")
                                    {
                                        string dataOne = "";
                                        if (joOne.Property("data") != null)
                                        {
                                            dataOne = joOne["data"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(dataOne))
                                        {
                                            JObject joDataOne = JsonConvert.DeserializeObject<JObject>(dataOne);

                                            string statusOne = "";
                                            if (joDataOne.Property("status") != null)
                                            {
                                                statusOne = joDataOne["status"].ToString();
                                            }
                                            string sharelist = "";
                                            if (joDataOne.Property("sharelist") != null)
                                            {
                                                sharelist = joDataOne["sharelist"].ToString();
                                            }
                                            if (statusOne == "成功" && sharelist.Length>3)
                                            {
                                                List<SqlPara> list = new List<SqlPara>();
                                                list.Add(new SqlPara("accountNo", ""));
                                                list.Add(new SqlPara("accountName", webName));
                                                list.Add(new SqlPara("custPhone", ""));
                                                list.Add(new SqlPara("payMode", "扫码付款"));
                                                list.Add(new SqlPara("inkey", ""));//接口验证码
                                                list.Add(new SqlPara("paySysDate", System.DateTime.Now.ToString("yyyyMMdd")));
                                                list.Add(new SqlPara("serialNum", businessrecordnumber));
                                                list.Add(new SqlPara("merNo", "3501001"));//
                                                list.Add(new SqlPara("merOrderId", orderCode));
                                                list.Add(new SqlPara("merOrderName", ""));
                                                list.Add(new SqlPara("merShortName", ""));
                                                list.Add(new SqlPara("price", 0));
                                                list.Add(new SqlPara("merOrderAmt", totalAmount));
                                                list.Add(new SqlPara("merOrderCount", "1"));
                                                list.Add(new SqlPara("payType", paytype));//支付类型
                                                list.Add(new SqlPara("salePayAcct", ""));
                                                list.Add(new SqlPara("custPayAcct", ""));
                                                list.Add(new SqlPara("merOrderStatus", "成功"));
                                                list.Add(new SqlPara("payUrl", code));
                                                list.Add(new SqlPara("Errorcode", errorcode));
                                                list.Add(new SqlPara("flag", payType));

                                                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CHCODEPAY", list));
                                                if (MessageBox.Show("付款成功，是否关闭付款窗口?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification) == DialogResult.Yes)
                                                {
                                                    this.Close();
                                                }
                                                else
                                                { this.Close(); }
                                                break;
                                            }
                                            if(statusOne=="失败")
                                            {
                                                MsgBox.ShowOK("交易失败!");
                                                break;
                                            }

                                        }
                                    }
                                    Thread.Sleep(600);
                                    times++;
                                }
                                while (times < 100);
                                #endregion
                               
                             

                                //if( MessageBox.Show("付款成功，是否关闭付款窗口?")==DialogResult.Yes)
                                //{
                                //    //this.Close();
                                //}
                                //this.Close();
                                // Thread.Sleep(1000 * 60 * 30);

                            }



                        }
                        else
                        {
                            MsgBox.ShowError(result.Message);
                        }
                    }

                  
                  

                    //code = "";
                   // txtBCode.Text = "";
                    txtBBillno.Text = "";
                    txtOCompany.Text = "";
                    txtBSumMoney.Text = "";
                    txtOSummoney.Text = "";
                    txtBAccMoney.Text = "0";
                    txtOAccMoney.Text = "0";
                }
                catch (Exception ex)
                {
                    //txtBCode.Text = "";
                   // code = "";
                    MsgBox.ShowOK(ex.Message);
                }
            }
          
        }

        private void showMsg(string msg)
        {
            if (xtraTabControl1.SelectedTabPage == tpByBillNo)
            {
                lblBMsg.Text = msg;
                txtBCode.Text = "";
            }
            if (xtraTabControl1.SelectedTabPage == tpByOther)
            {
                lblOMsg.Text = msg;
                txtOCode.Text = "";
            }
            
            //lbl.Text = msg;
        }

        private void txtBAuthcode_EditValueChanged(object sender, EventArgs e)
        {
           // if (txtOCode.Text.Trim() == "" || txtBCode.Text.Trim()=="")
           // {
           //     return;
           // }
           // if (!flag)
           // {               
           //     //lblBMsg.Text = "输入金额后，请按回车!";
           //     showMsg("输入金额后，请按回车!");
           //     return;
           // }
           //// string aa = "bb";
           // if (txtBCode.Text.Trim() == code && txtBCode.Text.Trim()!="")
           // {
           //     try
           //     {
           //         string billno = "";
           //         string sumMoney = "";
           //         string accMoney = "";
           //         string operMan = "";
           //         string webName = "";
           //         string areaName = "";
           //         string companyName="";
           //         if (xtraTabControl1.SelectedTabPage == tpByBillNo)
           //         {
           //             billno = txtBBillno.Text.Trim();
           //             sumMoney = txtBSumMoney.Text.Trim();
           //             accMoney = txtBAccMoney.Text.Trim();
           //             operMan = txtBOperMan.Text.Trim();
           //             webName = txtBWebName.Text.Trim();
           //             areaName = txtBAreaName.Text.Trim();
           //             if (billno == "")
           //             {
           //                 //MsgBox.ShowOK("运单号不能为空!");
           //                 //lblBMsg.Text = "运单号不能为空!";
           //                 showMsg("运单号不能为空!");
           //                 return;
           //             }
           //             if (sumMoney == "")
           //             {
           //                 //lblBMsg.Text = "应收金额不能为空!";
           //                 showMsg("应收金额不能为空!");
           //                 return;

           //             }
           //         }
           //         if (xtraTabControl1.SelectedTabPage == tpByOther)
           //         {
           //             companyName = txtOCompany.Text.Trim();
           //             sumMoney = txtOSummoney.Text.Trim();
           //             accMoney = txtOAccMoney.Text.Trim();
           //             operMan = txtOOperman.Text.Trim();
           //             webName = txtOWebName.Text.Trim();
           //             areaName = txtOAreaName.Text.Trim();
           //             if (companyName == "")
           //             {
           //                 //lblBMsg.Text = "公司/客户名不能为空!";
           //                 showMsg("公司/客户名不能为空!");
           //                 return;
           //             }
           //         }
                    
                  
           //         if (Convert.ToDecimal(accMoney) > 50000)
           //         {
           //             // MsgBox.ShowError("付款金额不能大于50000！");
           //            // lblBMsg.Text = "付款金额不能大于50000!";
           //             showMsg("付款金额不能大于50000!");
           //             return;
           //         }
           //         if (decimal.Round(Convert.ToDecimal(accMoney), 2) < (decimal)0.01)
           //         {
           //             //  MsgBox.ShowError("付款金额必须大于0！");
           //             lblBMsg.Text = "付款金额必须大于0！";
           //             return;
           //         }
           //         //if (type == 1)
           //         //{                       
           //         //}
           //         //if (type == 0)
           //         //{                                           
           //         //}
           //         int paytype = 0;
           //         string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
           //         string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
           //         string startCode = code.Substring(0, 2);
           //         string totalAmount = accMoney; //decimal.Round(decimal.Parse(accMoney), 2).ToString().ToString();
           //         string[] weixinScodes = { "10", "11", "12", "13", "14", "15" };//微信
           //         string[] zhifubaoScodes = { "25", "26", "27", "28", "29", "30" };//支付宝
           //         //微信条码规则：用户付款码条形码规则：18位纯数字，以10、11、12、13、14、15开头
           //         //支付宝条码规则：付款码将由原来的28开头扩充到25-30开头，长度由原来的16-18位扩充到16-24位。
           //         if (((IList)weixinScodes).Contains(startCode))
           //         {
           //             if (code.Length != 18)
           //             {

           //                // MsgBox.ShowOK("条码不正确，请重新扫描!");
           //                // lblBMsg.Text = "条码不正确，请重新扫描!";
           //                 showMsg("条码不正确，请重新扫描!");
           //                 return;
           //             }
           //             paytype = 17;//微信条码
           //         }
           //         if (((IList)zhifubaoScodes).Contains(startCode))
           //         {
           //             if (code.Length < 18 || code.Length > 24)
           //             {
           //                // MsgBox.ShowOK("条码不正确，请重新扫描!");
           //                // lblBMsg.Text = "条码不正确，请重新扫描!";
           //                 showMsg("条码不正确，请重新扫描!");
           //                 return;
           //             }
           //             paytype = 18;//支付宝条码
           //         }
           //         if (paytype == 0)
           //         {
           //            // MsgBox.ShowOK("条码不正确，请重新扫描!");
           //             //lblBMsg.Text = "条码不正确，请重新扫描!";
           //             showMsg("条码不正确，请重新扫描!");
           //             return;

           //         }
           //         if (paytype == 17)
           //         {
           //             orderCode = orderCode + "6";
           //         }
           //         if (paytype == 18)
           //         {
           //             orderCode = orderCode + "7";
           //         }

           //         string url = "http://localhost:12345/BarCodePayService/BarCodePay";
           //         Dictionary<string, object> dic = new Dictionary<string, object>();
           //         string account_number = "8800011605627";//钱包新增参数，此参数不同系统对应不同参数，系统会根据这个参数给操作不同数据库
           //         //string dog_sk = "5O67GaYQ60w318JBN4b8";//密文
           //         //string toaccountnumber = "8800010082679";//账号
           //         //dic.Add("paytype", paytype);//支付类型14支付宝扫描15微信扫码16收银台
           //         dic.Add("businessnumber", orderCode);//订单号
           //         dic.Add("transactionamount", totalAmount);//金额
           //         //dic.Add("appid", appid);//商户号
           //         dic.Add("account_number", account_number);//商户号
           //         dic.Add("authcode", code);
           //         dic.Add("paytype", paytype);
           //         //http://localhost:12345/BarCodePayService
           //         string requestStr = JsonConvert.SerializeObject(dic);
           //         RequestModel<string> request = new RequestModel<string>();
           //         request.Request = requestStr;
           //         request.OperType = 0;
           //         string json = JsonConvert.SerializeObject(request);
           //         ResponseModelClone<string> result = HttpHelper.HttpPost(json, url);
           //         if (result.State == "200")
           //         {
           //             JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
           //             string biz_code = "";
           //             if (jo.Property("biz_code") != null)
           //             {
           //                 biz_code = jo["biz_code"].ToString();
           //             }
           //             string Repcode = "";
           //             if (jo.Property("code") != null)
           //             {
           //                 Repcode = jo["code"].ToString();
           //             }

           //             string payType = "";
           //             if (xtraTabControl1.SelectedTabPage == tpByBillNo)
           //             {
           //                 payType = "运单收款";

           //             }
           //             else
           //             {
           //                 payType = "其他收款";
           //             }
           //             string businessrecordnumber = "";
           //             if (biz_code == "GPBIZ_00" && Repcode == "GP_00")
           //             {
           //                 string data = "";
           //                 if (jo.Property("data") != null)
           //                 {
           //                     data = jo["data"].ToString();
           //                 }
           //                 if (!string.IsNullOrEmpty(data))
           //                 {
           //                     JObject joData = JsonConvert.DeserializeObject<JObject>(data);

           //                     if (joData.Property("businessrecordnumber") != null)
           //                     {
           //                         businessrecordnumber = joData["businessrecordnumber"].ToString();
           //                     }

           //                 }
           //                 List<SqlPara> list = new List<SqlPara>();
           //                 list.Add(new SqlPara("accountNo", ""));
           //                 list.Add(new SqlPara("accountName", webName));
           //                 list.Add(new SqlPara("custPhone", ""));
           //                 list.Add(new SqlPara("payMode", "扫码付款"));
           //                 list.Add(new SqlPara("inkey", ""));//接口验证码
           //                 list.Add(new SqlPara("paySysDate", System.DateTime.Now.ToString("yyyyMMdd")));
           //                 list.Add(new SqlPara("serialNum", businessrecordnumber));
           //                 list.Add(new SqlPara("merNo", "3501001"));//
           //                 list.Add(new SqlPara("merOrderId", orderCode));
           //                 list.Add(new SqlPara("merOrderName", ""));
           //                 list.Add(new SqlPara("merShortName", ""));
           //                 list.Add(new SqlPara("price", 0));
           //                 list.Add(new SqlPara("merOrderAmt", totalAmount));
           //                 list.Add(new SqlPara("merOrderCount", "1"));
           //                 list.Add(new SqlPara("payType", paytype));//支付类型
           //                 list.Add(new SqlPara("salePayAcct", ""));
           //                 list.Add(new SqlPara("custPayAcct", ""));
           //                 list.Add(new SqlPara("merOrderStatus", "成功"));
           //                 list.Add(new SqlPara("payUrl", code));
           //                 list.Add(new SqlPara("Errorcode", billno));
           //                 list.Add(new SqlPara("flag", payType));

           //                 SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CHCODEPAY", list));
           //             }

           //             MsgBox.ShowOK();

           //         }
           //         else
           //         {
           //             MsgBox.ShowError(result.Message);
           //         }

           //         code = "";
           //         txtBCode.Text = "";
           //     }
           //     catch (Exception ex)
           //     {
           //         txtBCode.Text = "";
           //         code = "";
           //         MsgBox.ShowOK(ex.Message);
           //     }
            //}
        }

        private void frmCheckstand_FormClosed(object sender, FormClosedEventArgs e)
        {
            listener.Stop();  
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //string aa = code;
            this.Close();
        }

        private void txtBAccMoney_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                {
                    txtBCode.Focus();
                    this.ActiveControl = txtBCode;
                }
                else
                {
                    txtOCode.Focus();
                    this.ActiveControl = txtOCode;
                }
                flag = true;
            }
        }

        private void txtBAccMoney_Enter(object sender, EventArgs e)
        {
            flag = false;
        }

        private void btnOCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (type == 0)
            {
                //xtraTabControl1.SelectedTabPage = tpByOther;
                if (xtraTabControl1.SelectedTabPage == tpByOther)
                {
                    txtBBillno.Properties.ReadOnly = true;
                    txtOCompany.Properties.ReadOnly = false;
                    txtBBillno.Text = "";
                    txtBAccMoney.Text = "0";
                    txtBSumMoney.Text = "";
                    txtBCode.Text = "";
                    //txtOSummoney.Properties.ReadOnly = true;
                    //txtOOperman.Properties.ReadOnly = true;
                    //txtOWebName.Properties.ReadOnly = true;
                    //txtOAreaName.Properties.ReadOnly = true;
                }
                if (xtraTabControl1.SelectedTabPage == tpByBillNo)
                {
                    txtBBillno.Properties.ReadOnly = false;
                    txtOCompany.Properties.ReadOnly = true;
                    txtOCompany.Text = "";
                    txtOAccMoney.Text = "0";
                    txtOSummoney.Text = "";
                    txtOCode.Text = "";

                }

            }
        }

        private void txtBBillno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                vide(txtBBillno.Text.Trim());
            }
        }

        public void vide(string billno)
        {
            if (billno.Trim().Equals(""))
            {
                return;
            }
            if (xtraTabControl1.SelectedTabPage==tpByBillNo)//运单收款
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNO", billno));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_CH", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
                {
                    MsgBox.ShowError("该运单不存在！");

                }
                else
                {
                  txtBSumMoney.Text = ds.Tables[0].Rows[0]["PaymentAmout"].ToString();
                }
            }
        }

        private void txtBBillno_Leave(object sender, EventArgs e)
        {
            vide(txtBBillno.Text.Trim());
        }


        public static string HttpPost(string postUrl, string paramData, Encoding EncodingName)
        {
            string postDataStr = paramData;
            byte[] buff = EncodingName.GetBytes(postDataStr);

            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (postUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(postUrl) as HttpWebRequest;
                request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(postUrl) as HttpWebRequest;
            }

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(buff, 0, buff.Length);
            myRequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, EncodingName);
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            return retString;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}