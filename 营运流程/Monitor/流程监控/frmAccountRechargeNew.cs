using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Web;
using System.Globalization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZQTMS.UI
{
    public partial class frmAccountRechargeNew : BaseForm
    {
        public frmAccountRechargeNew()
        {
            InitializeComponent();
           
        }

        private string TFTtips = "", LLtips = "", YLtips = "";
        private void frmAccountRechargeNew_Load(object sender, EventArgs e)
        {
            

            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_InsuranceAccount_New");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        txtAccountName.Text = ds.Tables[0].Rows[i]["GSQC"].ToString();
                        txtAccountNo.Text = ds.Tables[0].Rows[i]["AccountID"].ToString();
                        txtBalance.Text = ds.Tables[0].Rows[i]["InsuranceBalance"].ToString();
                    }
                }
                else
                {
                    ds = new DataSet();
                }
                #region 获取提示
                SqlParasEntity sp = new SqlParasEntity(OperType.Query, "QSP_GET_SYSPARAMSETTING_By_ParamType", new List<SqlPara> {(new SqlPara("ParamType", "TFTtips,LLtips,YLtips,")) }); 
                 DataSet ds1 = SqlHelper.GetDataSet(sp);
                if (ds1 != null)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        if (ds1.Tables[0].Rows[i]["ParamType"].ToString().Equals("TFTtips")) {
                            TFTtips = ds1.Tables[0].Rows[i]["ParamDescription"].ToString();
                        }
                        if (ds1.Tables[0].Rows[i]["ParamType"].ToString().Equals("LLtips"))
                        {
                            LLtips = ds1.Tables[0].Rows[i]["ParamDescription"].ToString();
                        }
                        if (ds1.Tables[0].Rows[i]["ParamType"].ToString().Equals("YLtips"))
                        {
                            YLtips = ds1.Tables[0].Rows[i]["ParamDescription"].ToString();
                        }
                    }
                }
                this.label12.Text = TFTtips;
                this.label11.Text = LLtips;
                this.label13.Text = YLtips;
                #endregion

                this.radio_LLpay.Text = "连连";
                //dateflag();
                //this.comboBox3.Show();
                //this.comboBox5.Hide();
                this.comboBox1.Hide();
                this.label5.Hide();
                this.label7.Hide();
                this.label8.Hide();
                this.label9.Hide();
                this.label10.Hide();
                this.lbl_Ch.Hide();

                radio_chpay.Checked = true;
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //private bool dateflag()
        //{
        //    #region 每天23:00:00 后禁止使用杉德充值
        //    //开始时间
        //    DateTime begindate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:00:00");// 2008-09-04  23:00:00
        //    //当前时间
        //    DateTime newdate = Convert.ToDateTime(DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"));// 2008-09-04 20:12:12
        //    //截至时间
        //    DateTime enddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59");// 2008-09-04  23:59:59
            
        //    if (DateTime.Compare(begindate, newdate) <= 0 && DateTime.Compare(enddate, newdate) >= 0)
        //    {
        //        if (radio_sdpay.Checked)
        //        {
        //            this.label11.Show();
        //            this.label12.Show();
        //            this.radio_TFTpay.Checked = true;
        //            this.radio_sdpay.Checked = false;
        //            this.radio_sdpay.Enabled = false;
        //            this.label10.Hide();
        //            this.comboBox1.Hide();
        //            this.comboBox2.Hide();
        //            free();
        //            return true;
        //        }
        //        if (radio_LLpay.Checked) {
        //            this.label12.Hide();
        //            this.label10.Hide();
        //            this.comboBox1.Hide();
        //            this.comboBox2.Hide();
        //        }
        //        else
        //        {
        //            this.label11.Show();
        //            free();
        //        }
        //    }
        //    else
        //    {
        //        this.label11.Hide();
        //        if (radio_sdpay.Checked)
        //        {
        //            this.label12.Hide();
        //            this.label10.Show();
        //            this.comboBox1.Show();
                    
        //        }
        //        else
        //        {
        //            this.radio_sdpay.Enabled = true;
        //            if (radio_TFTpay.Checked)
        //            {
        //                this.label12.Show();

        //            }
        //            free();
        //        }
        //    }
        //    return false;
        //    #endregion
        //}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private static SqlResult HttpPost(string url, string data)
        {
            //string url = domain + UrlPage;
            SqlResult result = new SqlResult();
            try
            {
                string json = "";//返回json字符串

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 600000;


                byte[] btbody = Encoding.UTF8.GetBytes(data);
                req.ContentLength = btbody.Length;
                using (Stream st = req.GetRequestStream())
                {
                    st.Write(btbody, 0, btbody.Length);
                    st.Close();
                    st.Dispose();
                }
                WebResponse wr = (HttpWebResponse)req.GetResponse();
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
                wr.Close();
                result.State = 1;
                result.Result = json;
            }
            catch (WebException ex)
            {
                result.State = 0;
                result.Result = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.State = 0;
                result.Result = ex.Message;
            }
            return result;
        }


        private static SqlResult HttpPostJson(string url, string data)
        {
            //string url = domain + UrlPage;
            SqlResult result = new SqlResult();
            try
            {
                string json = "";//返回json字符串

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 600000;


                byte[] btbody = Encoding.UTF8.GetBytes(data);
                req.ContentLength = btbody.Length;
                using (Stream st = req.GetRequestStream())
                {
                    st.Write(btbody, 0, btbody.Length);
                    st.Close();
                    st.Dispose();
                }
                WebResponse wr = (HttpWebResponse)req.GetResponse();
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
                wr.Close();
                result = JsonConvert.DeserializeObject<SqlResult>(json);

                //result.State = 1;
                //result.Result = json;
            }
            catch (WebException ex)
            {
                result.State = 0;
                result.Result = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.State = 0;
                result.Result = ex.Message;
            }
            return result;
        }
        private void btnRecharge_Click(object sender, EventArgs e)
        {
            //if (radio_sdpay.Checked && dateflag())
            //{
            //    MsgBox.ShowOK("每日23:00—00:00为杉德系统维护时间！");
            //    return;
            //};
            //if (dateflag())
            //{
            //    MsgBox.ShowOK("每日23:00—00:00为杉德系统维护时间！");
            //    return;
            //};
            if (txtAccountName.Text=="无")
            {
                MsgBox.ShowOK("当前不存在可充值的账户！");
                return;
            }
            if (txtRechargeNum.Text == "0")
            {
                MsgBox.ShowOK("请输入充值金额！");
                return;
            }
            else if (txtRechargeNum.Text.Length > 8)
            {
                MsgBox.ShowError("金额位数不能超过8位！");
                return;
            }
            else if (Convert.ToDecimal(txtRechargeNum.Text)>(decimal)50000)
            {
                MsgBox.ShowOK("充值金额不能大于50000！");
                return;
            }
            
            if (decimal.Round(Convert.ToDecimal(txtRechargeNum.Text.Trim()), 2) < Convert.ToDecimal(0.01))
            {
                MsgBox.ShowError("充值金额必须大于0！");
                txtRechargeNum.Text = "0";
                return;
            }
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                MsgBox.ShowOK("手机号码不能为空！");
                return;
            }
            if (txtPhone.Text.Length != 11)
            {
                MsgBox.ShowOK("手机号码输入有误！");
                return;
            }

            if (MsgBox.ShowYesNo("您确定要充值么？") != DialogResult.Yes) return;

            if (radio_Unionpay.Checked)
            {
                int paytype = 0;
                if (this.comboBox1.Text.Equals("支付宝扫码")) {
                    paytype = 12;
                }
                if (this.comboBox1.Text.Equals("微信扫码"))
                {
                    paytype = 13;
                }
                UnionpayNew.UnionPay(paytype, txtAccountName.Text, txtAccountNo.Text, txtBalance.Text, txtRechargeNum.Text, txtPhone.Text);
                this.Close();
            }
            if (radio_chpay.Checked)
            {
                //支付类型14代表支付宝扫描，15微信扫码，16收银台
                int paytype = 0;
                if (this.comboBox1.Text.Equals("支付宝扫码"))
                {
                    paytype = 14;
                }
                if (this.comboBox1.Text.Equals("微信扫码"))
                {
                    paytype = 15;
                }
                if (this.comboBox1.Text.Equals("收银台"))
                {
                    paytype = 16;
                }
                CHPay.CHpay(paytype, txtAccountName.Text, txtAccountNo.Text, txtBalance.Text, txtRechargeNum.Text, txtPhone.Text);
                this.Close();
            }
            //if (radio_Walletpay.Checked)
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("WebName", txtAccountName.Text.Trim()));
            //    DataSet ds=   SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WalletAccountInfo_byWebName", list));
            //    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            //    {
                    
            //        if (MsgBox.ShowYesNo("您还没有注册支付钱包，是否立即注册？") == DialogResult.Yes)
            //        {
                        
            //            frmOpenAccount foa = new frmOpenAccount();
            //            foa.account_code = 1;
            //            foa.Show();
            //            this.Close();
                        
            //        }
            //    }
            //    else
            //    {
            //        string ext_user_id = ds.Tables[0].Rows[0]["UserID"].ToString();
            //        string user_id = ds.Tables[0].Rows[0]["user_id"].ToString();
            //        CHPay.CHWalletPay(ext_user_id, user_id, txtAccountName.Text, txtAccountNo.Text, txtBalance.Text, txtRechargeNum.Text, txtPhone.Text);
            //    }
               
            //}
        }

        public static string getbankcode(string bank) {
            switch (bank)
            {
                case "工商银行": return "01020000";
                case "建设银行": return "01050000";
                case "农业银行": return "01030000";
                case "招商银行": return "03080000";
                case "交通银行": return "03010000";
                case "中国银行": return "01040000";
                case "光大银行": return "03030000";
                case "民生银行": return "03050000";
                case "兴业银行": return "03090000";
                case "中信银行": return "03020000";
                case "广发银行": return "03060000";
                case "浦发银行": return "03100000";
                case "平安银行": return "03070000";
                case "华夏银行": return "03040000";
                case "宁波银行": return "04083320";
                case "东亚银行": return "03200000";
                case "上海银行": return "04012900";
                case "中国邮储银行": return "01000000";
                case "南京银行": return "04243010";
                case "上海农商行": return "65012900";
                case "渤海银行": return "03170000";
                case "成都银行": return "64296510";
                case "北京银行": return "04031000";
                case "徽商银行": return "64296511";
                case "天津银行": return "04341101";
            }
            return "1020000";
        }

        public static bool OpenBrowser(string url)
        {
            try
            {
                System.Diagnostics.Process.Start("iexplore.exe", url);
                return true;
            }
            catch (Exception ex)
            {
               // MessageBox.Show("充值打开浏览器失败"+ex);
              //  LogHelper.Error("财付通充值打开浏览器失败！", ex);
            }
            return false;
        }
        public static bool OpenBrowsers(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("充值打开浏览器失败" + ex);
                //  LogHelper.Error("财付通充值打开浏览器失败！", ex);
            }
            return false;
        }
        private void txtRechargeNum_TextChanged(object sender, EventArgs e)
        {
            free();
        }
        private void free()
        {
            decimal RechargeNum;
            if (txtRechargeNum.Text == "")
            {
                RechargeNum = 0;
                return;
            }
            else
            {
                RechargeNum = Convert.ToDecimal(txtRechargeNum.Text);//输入的充值金额
            }
            //腾付通手续费
            decimal handCharge = RechargeNum * (decimal)0.0025;//手续费
            //if (radio_sdpay.Checked)
            //{
            //    //杉德手续费
            //    if (comboBox1.Text.Equals("网银支付"))
            //    {
            //        handCharge = RechargeNum * (decimal)0.0020;
            //    }
            //    else
            //    {
            //        handCharge = RechargeNum * (decimal)0.0030;
            //    }
            //}
            if (radio_LLpay.Checked)
            {
                //连连手续费,最低每笔0.1元
                handCharge = RechargeNum * (decimal)0.0015;
                if (handCharge > (decimal)0&&handCharge < (decimal)0.1){
                    handCharge = (decimal)0.1;
                }
            }
            else if(radio_Unionpay.Checked)
            {
                //银联手续费,最低每笔0.1元
                handCharge = RechargeNum * (decimal)0.0028;
                //if (handCharge > (decimal)0 && handCharge < (decimal)0.1)
                //{
                //    handCharge = (decimal)0.1;
                //}
            }
            else if (radio_chpay.Checked)
            {
                if (comboBox1.Text == "微信扫码" || comboBox1.Text == "支付宝扫码")
                {
                    handCharge = RechargeNum * (decimal)0.002;
                }
                else
                {
                    handCharge = RechargeNum * (decimal)0.0015;
                }
            }
            else
            {
                if (handCharge > 10)
                {
                    handCharge = 10;
                }
            }

            decimal actualCharge = RechargeNum - handCharge;//实际充值金额=输入的充值金额-手续费
            label8.Text = decimal.Parse(handCharge.ToString(), NumberStyles.Any) + " 元";
            label9.Text = actualCharge + " 元";
        }

        //private void radio_sdpay_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.label12.Hide();
        //    this.label10.Show();
        //    this.comboBox1.Show();
        //    this.comboBox3.Hide();
        //    this.comboBox4.Hide();
        //    this.comboBox5.Hide();
        //    if (comboBox1.Text == "网银支付")
        //    {
        //        this.comboBox1.Size = new System.Drawing.Size(100, 22);
        //        this.comboBox2.Show();
        //    }
        //    else
        //    {
        //        this.comboBox1.Size = new System.Drawing.Size(194, 22);
        //        this.comboBox2.Hide();
        //    }
        //    free();
        //}

        private void radio_TFTpay_CheckedChanged(object sender, EventArgs e)
        {
            this.label12.Show();
            this.label13.Hide();
            this.label11.Hide();
            //this.comboBox1.Hide();
            //this.comboBox2.Hide();
            //this.comboBox3.Hide();
            //this.comboBox4.Hide();
            //this.comboBox5.Hide();
            this.label5.Show();
            this.label7.Show();
            this.label8.Show();
            this.label9.Show();
            this.comboBox1.Hide();
            this.label10.Hide();
            showorhide("",true);
            this.panel1.Hide();
            free();
        }

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    free();
        //    if (comboBox1.Text == "网银支付")
        //    {
        //        this.comboBox1.Size = new System.Drawing.Size(100, 22);
        //        this.comboBox2.Show();
        //    }
        //    else {
        //        this.comboBox1.Size = new System.Drawing.Size(194, 22);
        //        this.comboBox2.Hide();
        //    }
            
        //}

        private void txtRechargeNum_EditValueChanged(object sender, EventArgs e)
        {
            if (radio_LLpay.Checked)
            {
                if (txtRechargeNum.Text != "" && Convert.ToDecimal(txtRechargeNum.Text) < (decimal)0.1)
                {
                    MessageBox.Show("连连充值金额不能低于0.1元");
                    return;
                }
            }
            if (Convert.ToDecimal(txtRechargeNum.Text) > (decimal)50000)
            {
                MsgBox.ShowOK("充值金额不能大于50000！");
                return;
            }
            else if (decimal.Parse(txtRechargeNum.Text) < (decimal)0.01)
            {
                MsgBox.ShowOK("充值金额不能小于0.01！");
                return;
            }
        }

        private void txtPhone_EditValueChanged(object sender, EventArgs e)
        {
            if (txtPhone.Text.Length > 11)
            {
                MessageBox.Show("手机号码输入有误！");
            }
        }

        private void radio_Unionpay_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_Unionpay.Checked)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("支付宝扫码");
               // comboBox1.Items.Add("微信扫码");
                //comboBox1.Items.Add("转账充值");


                this.comboBox1.SelectedIndex = 0;
                this.label13.Show();
                this.label5.Show();
                this.label7.Show();
                this.label8.Show();
                this.label9.Show();
                this.comboBox1.Show();
                this.label10.Show();
                this.label11.Hide();
                this.label12.Hide();
                this.panel1.Hide();
                free();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox1.Text == "转账充值")
            {
                showorhide("",false);
                this.panel1.Show();
            }
            else {
                showorhide("",true);
                this.panel1.Hide();
            }
            if (radio_chpay.Checked)
            {
                free();
            }
        }

        private void radio_LLpay_CheckedChanged(object sender, EventArgs e)
        {
            this.label12.Hide();
            this.label13.Hide();
            this.label11.Show();
            //this.comboBox1.Hide();
            //this.comboBox2.Hide();
            //this.comboBox3.Show();
            //this.comboBox4.Show();
            //this.comboBox5.Show();
            this.label5.Hide();
            this.label7.Hide();
            this.label8.Hide();
            this.label9.Hide();
            this.comboBox1.Hide();
            this.label10.Hide();
            showorhide("LL",true);
            this.panel1.Hide();
            //free();
        }

        //private void comboBox4_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (this.comboBox4.Text== "借记卡") {
        //    this.comboBox3.Show();
        //    this.comboBox5.Hide();
        //}
        //    if (this.comboBox4.Text == "信用卡")
        //    {
        //    this.comboBox3.Hide();
        //    this.comboBox5.Show();
        //}
        //}
        public void showorhide(string flag,Boolean tf) {
            this.label1.Visible = tf;
            this.label2.Visible = tf;
            this.label3.Visible = tf;
            this.label4.Visible = tf;
            this.label6.Visible = tf;
            this.txtAccountName.Visible = tf;
            this.txtAccountNo.Visible = tf;
            this.txtBalance.Visible = tf;
            this.txtPhone.Visible = tf;
            this.btnRecharge.Visible = tf;
            this.btnCancel.Visible = tf;
            this.txtRechargeNum.Visible = tf;
            if (flag == "LL")
            {
                this.label5.Visible = false;
                this.label7.Visible = false;
                this.label8.Visible = false;
                this.label9.Visible = false;
            }
            else {
                this.label5.Visible = tf;
                this.label7.Visible = tf;
                this.label8.Visible = tf;
                this.label9.Visible = tf;
            }
        }

        private void radio_chpay_CheckedChanged(object sender, EventArgs e)
        {
            if (radio_chpay.Checked)
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("微信扫码");
                comboBox1.Items.Add("支付宝扫码");
               // comboBox1.Items.Add("收银台");
                this.comboBox1.SelectedIndex = 0;
                this.label13.Show();
                this.label5.Show();
                this.label7.Show();
                this.label8.Show();
                this.label9.Show();
                this.comboBox1.Show();
                this.label10.Show();
                this.label11.Hide();
                this.label12.Hide();
                this.panel1.Hide();
                this.lbl_Ch.Show();


            }
            else
            {
                this.lbl_Ch.Hide(); ;
            }
            free();
        }

        //private void simpleButton1_Click(object sender, EventArgs e)
        //{
        //    frmOpenAccount fra = new frmOpenAccount();
        //    fra.Show();
        //}

     
     }


    public class CHPay
    { 
         static string code = "";
         public static void CHpay(int paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone)
         {
             string url = "http://ZQTMS.dekuncn.com:8012/";
             //string url = "http://localhost:12345/";

             //string url = "http://localhost:8080/ZQTMSUnionPay/UnionQrPay"; //本地测试
             string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
             string strDate = System.DateTime.Now.ToString("yyyyMMdd");
             string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
             //将金额转为以分为单位格式
             string totalAmount = RechargeNum;// Convert.ToInt32(decimal.Round(decimal.Parse(RechargeNum), 2) * 100).ToString().ToString();
             Dictionary<string, object> dic = new Dictionary<string, object>();
             if (paytype == 14)
             {
                 //支付宝扫描
                 orderCode = orderCode + "3";
                 code = "1";
                 url = url + "MixturePayService/MixturePay";
             }
             else if (paytype == 15)
             {
                 //微信扫描扫描
                 orderCode = orderCode + "4";
                 code = "2";
                 url = url + "MixturePayService/MixturePay";

             }
             else if (paytype == 16)
             {
                 //传化收银台订单号
                 orderCode = orderCode + "5";
                 //修改收银台支付接口
                 //url = url + "CashierPayService/CashierPay";
                 url = url + "WalletPayService/WalletPay";
             }
             string appid = "3501001";//"3497001";//id
             if (paytype == 16)//新收银台接口新开辟了一个appid
             {
                 appid = "3645001";
             }
             string account_number = "8800011613171";//钱包新增参数
             string companyid = "485";
             string beneficiary = "深圳中强物流有限公司";
            // string dog_sk = "5O67GaYQ60w318JBN4b8";//密文
             //string toaccountnumber = "8800010082679";//账号
             if (paytype == 14 || paytype == 16)
             {
                 dic.Add("paytype", paytype);//支付类型14支付宝扫描15微信扫码16收银台
                 dic.Add("businessnumber", orderCode);//订单号
                 dic.Add("transactionamount", totalAmount);//金额
                 dic.Add("appid", appid);//商户号
                 dic.Add("account_number", account_number);//商户号
                 dic.Add("companyid", companyid);

                 //dic.Add("dog_sk", dog_sk);//商户号
                 //dic.Add("toaccountnumber", toaccountnumber);//商户号
                 string requestStr = JsonConvert.SerializeObject(dic);
                 RequestModel<string> request = new RequestModel<string>();
                 request.Request = requestStr;
                 request.OperType = 0;
                 string json = JsonConvert.SerializeObject(request);
                 ResponseModelClone<string> result = HttpHelper.HttpPost(json, url);
                 if (result.State == "200")
                 {

                     string htmldata = "";
                     if (paytype == 14 || paytype == 15)
                     {
                         JObject jo = (JObject)JsonConvert.DeserializeObject(result.Result);
                         JObject obj = null;
                         if (jo.Property("dataModel") != null)
                         {
                             obj = JsonConvert.DeserializeObject<JObject>(jo["dataModel"].ToString());
                         }

                         if (obj.Property("htmldata") != null)
                         {
                             htmldata = obj["htmldata"].ToString();
                         }
                         code = code + "|" + htmldata + "|" + "";

                         frmQrcode frm = new frmQrcode();
                         frm.StartPosition = FormStartPosition.CenterScreen;
                         frm.payWay = "ch";
                         frm.payCode = code;
                         frm.Show();
                     }
                     if (paytype == 16)
                     {
                         if (frmAccountRechargeNew.OpenBrowsers(result.Result) == false)
                         {
                             MessageBox.Show("打开浏览器失败！");
                             return;
                         }

                     }

                     //插入充值记录
                     List<SqlPara> list = new List<SqlPara>();
                     list.Add(new SqlPara("accountNo", accountno.Trim()));
                     list.Add(new SqlPara("accountName", accountname.Trim()));
                     list.Add(new SqlPara("custPhone", Phone.Trim()));
                     list.Add(new SqlPara("payMode", "保险充值"));
                     list.Add(new SqlPara("inkey", ""));//接口验证码
                     list.Add(new SqlPara("paySysDate", strDate));
                     list.Add(new SqlPara("serialNum", ""));
                     list.Add(new SqlPara("merNo", appid));//
                     list.Add(new SqlPara("merOrderId", orderCode));
                     list.Add(new SqlPara("merOrderName", accountname.Trim()));
                     list.Add(new SqlPara("merShortName", ""));
                     list.Add(new SqlPara("price", 0));
                     list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
                     list.Add(new SqlPara("merOrderCount", "1"));
                     list.Add(new SqlPara("payType", paytype));//支付类型
                     list.Add(new SqlPara("salePayAcct", ""));
                     list.Add(new SqlPara("custPayAcct", ""));
                     list.Add(new SqlPara("merOrderStatus", ""));
                     list.Add(new SqlPara("payUrl", htmldata));
                     list.Add(new SqlPara("Errorcode", ""));
                     list.Add(new SqlPara("account_number", account_number));
                     //list.Add(new SqlPara("paytype",paytype));

                     SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CHPAY", list));

                 }
                 else
                 {
                     // MsgBox.ShowError("支付异常:" + result.Message + ":" + result.Result);
                     //MessageBox.Show("支付异常:" + result.Message + ":" + result.Result, "系统提示", 
                     //   MessageBoxButtons.OK, MessageBoxIcon.Warning, 
                     //   MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                     XtraMessageBox.Show("支付异常:" + result.Message + ":" + result.Result, "系统提示",
                         MessageBoxButtons.OK, MessageBoxIcon.Error,
                         MessageBoxDefaultButton.Button1);

                 }
             }
             if (paytype == 15)
             {
                 //http://www.dekuncn.com/app/index.php?r=weipay.scancode&businessnumber=1213111&transactionamount=0.01&account_number=8800011613171&beneficiary=收款方
                 StringBuilder sb = new StringBuilder();//"&"
                 sb.Append("http://www.dekuncn.com/app/index.php?r=weipay.scancode");
                 sb.Append("&");
                 sb.Append("businessnumber=" + orderCode);
                 sb.Append("&");
                 sb.Append("transactionamount=" + totalAmount);
                 sb.Append("&");
                 sb.Append("account_number=" + account_number);
                 sb.Append("&");
                 sb.Append("beneficiary=" + beneficiary);
                 sb.Append("&");
                 sb.Append("companyid=" + companyid);
                 code = code + "|" + sb.ToString() + "|" + "";
                 frmQrcode frm = new frmQrcode();
                 frm.StartPosition = FormStartPosition.CenterScreen;
                 frm.payWay = "ch";
                 frm.payCode = code;
                 frm.Show();
                 //插入充值记录
                 List<SqlPara> list = new List<SqlPara>();
                 list.Add(new SqlPara("accountNo", accountno.Trim()));
                 list.Add(new SqlPara("accountName", accountname.Trim()));
                 list.Add(new SqlPara("custPhone", Phone.Trim()));
                 list.Add(new SqlPara("payMode", "保险充值"));
                 list.Add(new SqlPara("inkey", ""));//接口验证码
                 list.Add(new SqlPara("paySysDate", strDate));
                 list.Add(new SqlPara("serialNum", ""));
                 list.Add(new SqlPara("merNo", appid));//
                 list.Add(new SqlPara("merOrderId", orderCode));
                 list.Add(new SqlPara("merOrderName", accountname.Trim()));
                 list.Add(new SqlPara("merShortName", ""));
                 list.Add(new SqlPara("price", 0));
                 list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
                 list.Add(new SqlPara("merOrderCount", "1"));
                 list.Add(new SqlPara("payType", paytype));//支付类型
                 list.Add(new SqlPara("salePayAcct", ""));
                 list.Add(new SqlPara("custPayAcct", ""));
                 list.Add(new SqlPara("merOrderStatus", ""));
                 list.Add(new SqlPara("payUrl", sb.ToString()));
                 list.Add(new SqlPara("Errorcode", ""));
                 list.Add(new SqlPara("account_number", account_number));
                 //list.Add(new SqlPara("paytype",paytype));

                 SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CHPAY", list));
             }
         }

         public static void CHWalletPay(string ext_user_id,string user_id, string accountname, string accountno, string balance, string RechargeNum, string Phone)
         {
             string url = "http://ZQTMS.dekuncn.com:8012/WalletPayService/WalletPay";
         
             string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
             string strDate = System.DateTime.Now.ToString("yyyyMMdd");
             string orderCode = strDateTime + new Random().Next(100, 999).ToString()+"6";//订单号
             //将金额转为以分为单位格式
             string totalAmount = RechargeNum;
             Dictionary<string, object> dic = new Dictionary<string, object>();
             
             dic.Add("order_id", orderCode);//订单号
             dic.Add("ext_user_id", ext_user_id);//金额
             dic.Add("user_id", user_id);//商户号
             string requestStr = JsonConvert.SerializeObject(dic);
             RequestModel<string> request = new RequestModel<string>();
             request.Request = requestStr;
             request.OperType = 0;
             string json = JsonConvert.SerializeObject(request);
             ResponseModelClone<string> result = HttpHelper.HttpPost(json, url);
             //插入充值记录
             List<SqlPara> list = new List<SqlPara>();
             list.Add(new SqlPara("accountNo", accountno.Trim()));
             list.Add(new SqlPara("accountName", accountname.Trim()));
             list.Add(new SqlPara("custPhone", Phone.Trim()));
             list.Add(new SqlPara("payMode", "保险充值"));
             list.Add(new SqlPara("inkey", ""));//接口验证码
             list.Add(new SqlPara("paySysDate", strDate));
             list.Add(new SqlPara("serialNum", ""));
             list.Add(new SqlPara("merNo", "3497001"));//
             list.Add(new SqlPara("merOrderId", orderCode));
             list.Add(new SqlPara("merOrderName", accountname.Trim()));
             list.Add(new SqlPara("merShortName", ""));
             list.Add(new SqlPara("price", 0));
             list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
             list.Add(new SqlPara("merOrderCount", "1"));
             list.Add(new SqlPara("payType", "0"));//支付类型
             list.Add(new SqlPara("salePayAcct", ""));
             list.Add(new SqlPara("custPayAcct", ""));
             list.Add(new SqlPara("merOrderStatus", ""));
             list.Add(new SqlPara("payUrl", ""));
             list.Add(new SqlPara("Errorcode", ""));
             //list.Add(new SqlPara("paytype",paytype));
         }
    }
    public class UnionpayNew
    {
        static string code = "";
        public static void UnionPay(int paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone)
        {
            string url = "http://ZQTMS.dekuncn.com:8079/UnionQrPay";
            //string url = "http://localhost:8080/ZQTMSUnionPay/UnionQrPay"; //本地测试
            string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            string strDate = System.DateTime.Now.ToString("yyyyMMdd");
            string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
            //将金额转为以分为单位格式
            string totalAmount = Convert.ToInt32(decimal.Round(decimal.Parse(RechargeNum), 2) * 100).ToString().ToString();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (paytype == 12)
            {
                orderCode = orderCode + "1";
                code = "1";
            }
            else if (paytype == 13)
            {
                orderCode = orderCode + "2";
                code = "2";

            }
            dic.Add("paymenttypeid", paytype);
            dic.Add("subpaymenttypeid", paytype);
            dic.Add("body", "保险充值");
            dic.Add("amount", totalAmount);
            dic.Add("businesstype", "1001");
            dic.Add("ordercode", orderCode);
            //报文体json
            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
            string data = jsonSer.Serialize(dic);

            Logger.Logging("UnionPay", "待发送报文为：" + System.Web.HttpUtility.UrlEncode(data, System.Text.Encoding.UTF8), true);
            string responseString = HttpUtils.HttpPost(url, System.Web.HttpUtility.UrlEncode(data, System.Text.Encoding.UTF8), Encoding.UTF8);
            Logger.Logging("UnionPay", "响应报文为：" + responseString, true);
            JObject jo = (JObject)JsonConvert.DeserializeObject(responseString);
            string codes = jo["response"].ToString();
            string qrcode = "";
            if (codes.Equals("0000"))
            {
                string datas = jo["data"].ToString();
                if (datas == null || datas.Equals("")) { MessageBox.Show("充值异常"); return; }
                JObject obj = (JObject)JsonConvert.DeserializeObject(datas);
                qrcode = obj["qrcode"].ToString();
                code = code + "|" + qrcode + "|" + obj["qrcodeurl"].ToString();
                frmQrcode frm = new frmQrcode();
                frm.payCode = code;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
            }
            //插入充值记录
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("accountNo", accountno.Trim()));
            list.Add(new SqlPara("accountName", accountname.Trim()));
            list.Add(new SqlPara("custPhone", Phone.Trim()));
            list.Add(new SqlPara("payMode", "保险充值"));
            list.Add(new SqlPara("inkey", ""));//接口验证码
            list.Add(new SqlPara("paySysDate", strDate));
            list.Add(new SqlPara("serialNum", ""));
            list.Add(new SqlPara("merNo", "703530785"));//
            list.Add(new SqlPara("merOrderId", orderCode));
            list.Add(new SqlPara("merOrderName", accountname.Trim()));
            list.Add(new SqlPara("merShortName", ""));
            list.Add(new SqlPara("price", 0));
            list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
            list.Add(new SqlPara("merOrderCount", "1"));
            list.Add(new SqlPara("payType", paytype));//支付类型
            list.Add(new SqlPara("salePayAcct", ""));
            list.Add(new SqlPara("custPayAcct", ""));
            list.Add(new SqlPara("merOrderStatus", ""));
            list.Add(new SqlPara("payUrl", qrcode));
            list.Add(new SqlPara("Errorcode", ""));

            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_UNIONPAY", list));
        }
        public static String GetCode()
        {
            return code;
        }
    }
    //public class SDpay
    //{
    //    static string logHeader = "Program_";
    //    static String code = "";
    //    public static void SdJavaPay(string bankecode, string paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone)
    //    {
    //        string merId = string.Empty;

    //        //生成报文
    //        //默认扫码接口，否者在线支付接口
    //        string url = "http://ZQTMS.dekuncn.com:8077/SDOnlinePay";
    //        //url = "http://localhost:8080/ZQTMS/SDOnlinePay"; //本地测试
    //        string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
    //        string strDate = System.DateTime.Now.ToString("yyyyMMdd");
    //        string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
    //        int paytypes =1;
    //        switch (paytype)
    //        {
    //            case "网银支付":paytypes = 4;orderCode = orderCode+"4"; break;
    //            case "支付宝扫码": paytypes = 1; code = "1"; orderCode = orderCode + "1"; url = "http://ZQTMS.dekuncn.com:8077/SDQrPay"; break;
    //            //string url = "http://localhost:8080/ZQTMS/SDQrPay";break;//本地测试
    //            case "微信扫码": paytypes = 2; code = "2"; orderCode = orderCode + "2"; url = "http://ZQTMS.dekuncn.com:8077/SDWechatQrPay"; break;
    //                //url = "http://192.168.16.133:8080/ZQTMS/SDWechatQrPay"; break; //本地测试
    //            case "银联扫码": paytypes = 3; code = "3"; orderCode = orderCode + "3"; break;
    //        }
            
    //        //调用java接口进行支付
    //        string msgData = PostZQTMS(url,bankecode, merId, paytype, accountname, accountno, balance, RechargeNum, Phone, orderCode);
    //        if (msgData.Equals("error")) {
    //            MessageBox.Show("充值请求出错！");
    //            return ;
    //        } else if (!msgData.Contains("http") && !msgData.Contains("weixin")) {
    //            MessageBox.Show(msgData);
    //            return ;
    //        }
    //        if (!paytype.Equals("网银支付"))
    //        {
    //            code = code + "|" + msgData;
    //        }
    //        else
    //        {
    //            try
    //            {
    //                System.Diagnostics.Process.Start(msgData);
    //            }
    //            catch (Exception ex){
    //                MessageBox.Show("充值打开浏览器失败");
    //            }
    //        }

    //        //插入充值记录
    //        List<SqlPara> list = new List<SqlPara>();
    //        list.Add(new SqlPara("accountNo", accountno.Trim()));
    //        list.Add(new SqlPara("accountName", accountname.Trim()));
    //        list.Add(new SqlPara("custPhone", Phone.Trim()));
    //        list.Add(new SqlPara("payMode", "账户充值"));
    //        list.Add(new SqlPara("inkey", ""));//接口验证码
    //        list.Add(new SqlPara("paySysDate", strDate));
    //        list.Add(new SqlPara("serialNum", ""));
    //        list.Add(new SqlPara("merNo", "13703377"));//
    //        list.Add(new SqlPara("merOrderId", orderCode));
    //        list.Add(new SqlPara("merOrderName", accountname.Trim()));
    //        list.Add(new SqlPara("merShortName", ""));
    //        list.Add(new SqlPara("price", 0)); 
    //        list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
    //        list.Add(new SqlPara("merOrderCount", "1"));
    //        list.Add(new SqlPara("payType", paytypes));//支付类型
    //        list.Add(new SqlPara("salePayAcct", ""));
    //        list.Add(new SqlPara("custPayAcct", ""));
    //        list.Add(new SqlPara("merOrderStatus", ""));
    //        list.Add(new SqlPara("payUrl", msgData));
    //        list.Add(new SqlPara("Errorcode", ""));
            
    //        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SDPAY", list));
    //    }
    //    /// <summary>
    //    /// 未用到
    //    /// </summary>
    //    /// <param name="bankecode"></param>
    //    /// <param name="paytype"></param>
    //    /// <param name="accountname"></param>
    //    /// <param name="accountno"></param>
    //    /// <param name="balance"></param>
    //    /// <param name="RechargeNum"></param>
    //    /// <param name="Phone"></param>
    //    public static void SdNetPay(string bankecode, string paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone)
    //    {
    //        //string signType = string.Empty;
    //        //string charset = string.Empty;
    //        //string pfxFilePath = string.Empty;
    //        //string pfxPassword = string.Empty;
    //        //string cerFilePath = string.Empty;
    //        //string merId = string.Empty;
    //        ////读取配置文件
    //        //INIClass iniReader = new INIClass(System.IO.Directory.GetCurrentDirectory() + "\\config.ini");

    //        //pfxFilePath = iniReader.IniReadValue("Main", "pfxFilePath");
    //        //pfxPassword = iniReader.IniReadValue("Main", "pfxPassword");
    //        //cerFilePath = iniReader.IniReadValue("Main", "cerFilePath");
    //        //charset = iniReader.IniReadValue("Main", "charset");
    //        //signType = iniReader.IniReadValue("Main", "signType");
    //        //merId = iniReader.IniReadValue("Main", "merId");//100211701170003

    //        ////定义Json转换类
    //        //JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        //Dictionary<string, object> dicRslt;

    //        ////解密后的服务器返回
    //        //MessageWorker.trafficMessage resp;


    //        ////生成报文
    //        //string serverUrl = iniReader.IniReadValue("扫码支付_预下单", "serverURL");
    //        ////默认扫码接口，否者在线支付接口
    //        //string url = iniReader.IniReadValue("异步通知", "notityURL");
    //        //string urljsp = iniReader.IniReadValue("成功界面", "successJSP");
    //        //int paytypes = 1;
    //        //switch (paytype)
    //        //{
    //        //    case "网银支付":
    //        //        serverUrl = iniReader.IniReadValue("收银台支付", "serverURL"); paytypes = 4; break;
    //        //    case "支付宝扫码": paytypes = 1; code = "1"; break;
    //        //    case "微信扫码": paytypes = 2; code = "2"; break;
    //        //    case "银联扫码": paytypes = 3; code = "3"; break;
    //        //}
    //        //string strDateTime = System.DateTime.Now.ToString("yyyyMMddhhmmss");
    //        //string strDate = System.DateTime.Now.ToString("yyyyMMdd");
    //        //string orderCode = strDateTime + new Random().Next(100, 999).ToString();//订单号
    //        ////调用java接口进行支付
    //        ////string msgData = PostZQTMS(url, bankecode, merId, paytype, accountname, accountno, balance, RechargeNum, Phone, orderCode);

    //        ////系统内部进行支付
    //        //Dictionary<string, object> msgData = QrCode_PreCreate(bankecode, merId, paytype, accountname, accountno, balance, RechargeNum, Phone, orderCode);

    //        ////string serverUrl = iniReader.IniReadValue("扫码支付_统一下单并支付", "serverURL");
    //        ////Dictionary<string, object> msgData = qrCode_BarPay(merId);

    //        ////string serverUrl = iniReader.IniReadValue("扫码支付_订单查询", "serverURL");
    //        ////Dictionary<string, object> msgData = qrCode_Query(merId);

    //        //resp = SendMessageSample(
    //        //    pfxFilePath,
    //        //    pfxPassword,
    //        //    cerFilePath,
    //        //    serverUrl,
    //        //    signType,
    //        //    charset,
    //        //    msgData
    //        //    );
    //        //dicRslt = jsonSer.DeserializeObject(resp.data) as Dictionary<string, object>;

    //        //Dictionary<string, object> respHead = dicRslt["head"] as Dictionary<string, object>;
    //        //Dictionary<string, object> respBody = (Dictionary<string, object>)dicRslt["body"];
    //        ////获取异常
    //        //getError(resp.data, respHead["respCode"].ToString());
    //        //if (!paytype.Equals("网银支付"))
    //        //{
    //        //    if (((Dictionary<string, object>)msgData["head"])["method"].ToString() == "sandpay.trade.precreate")
    //        //    {//预下单使用qrcode生成二维码，其他的报文用这段代码会报错
    //        //        Dictionary<string, object> respData = dicRslt["body"] as Dictionary<string, object>;
    //        //        string qrCode = respData["qrCode"].ToString();
    //        //        string orderCodes = respData["orderCode"].ToString();
    //        //        //控制台输出二维码
    //        //        //QrCodes.qrCodeToConsole(msgData);
    //        //        //Console.Write(@"订单号：" + orderCodes);
    //        //        //保存二维码
    //        //        //qrCodeToImageFile(qrCode, orderCode + ".png", 8);
    //        //    }
    //        //    code = code + "|" + msgData;
    //        //    //Console.ReadKey();
    //        //}
    //        //else
    //        //{
    //        //    //    //返回参数不详，待补充
    //        //    //    string credential = System.Web.HttpUtility.UrlEncode(respBody["credential"].ToString());
    //        //    //    Logger.Logging(logHeader, "服务器返回为：" + resp.data, true);
    //        //    //    MsgBox.ShowOK(resp.data);
    //        //    //    return;
    //        //    //System.Diagnostics.Process.Start(respBody[""]);
    //        //}

    //        ////插入充值记录
    //        //List<SqlPara> list = new List<SqlPara>();
    //        //list.Add(new SqlPara("accountNo", accountno.Trim()));
    //        //list.Add(new SqlPara("accountName", accountname.Trim()));
    //        //list.Add(new SqlPara("custPhone", Phone.Trim()));
    //        //list.Add(new SqlPara("payMode", "账户充值"));
    //        //list.Add(new SqlPara("inkey", ""));//接口验证码
    //        //list.Add(new SqlPara("paySysDate", strDate));
    //        //list.Add(new SqlPara("serialNum", ""));
    //        //list.Add(new SqlPara("merNo", "13703377"));//
    //        //list.Add(new SqlPara("merOrderId", orderCode));
    //        //list.Add(new SqlPara("merOrderName", accountname.Trim()));
    //        //list.Add(new SqlPara("merShortName", ""));
    //        //list.Add(new SqlPara("price", 0));
    //        //list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
    //        //list.Add(new SqlPara("merOrderCount", "1"));
    //        //list.Add(new SqlPara("payType", paytypes));//支付类型
    //        //list.Add(new SqlPara("salePayAcct", ""));
    //        //list.Add(new SqlPara("custPayAcct", ""));
    //        //list.Add(new SqlPara("merOrderStatus", ""));
    //        //list.Add(new SqlPara("payUrl", msgData));
    //        //list.Add(new SqlPara("Errorcode", "000000"));

    //        //SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SDPAY", list));
    //    }

        
    //    public static void getError(string data,string code)
    //    {
    //        if (code.Equals("000001")) {
    //            Logger.Logging(logHeader, "服务器返回为：银行返回超时" + data, true);
    //            MsgBox.ShowOK("银行返回超时");
    //            return;
    //        }
    //        else if (code.Equals("000002"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：银行处理异常" + data, true);
    //            MsgBox.ShowOK("银行处理异常");
    //            return;
    //        }
    //        else if (code.Equals("000003"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：平台处理异常" + data, true);
    //            MsgBox.ShowOK("平台处理异常");
    //            return;
    //        }
    //        else if (code.Equals("010001"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：商户不存在" + data, true);
    //            MsgBox.ShowOK("商户不存在");
    //            return;
    //        }
    //        else if (code.Equals("010002"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：商户已停用" + data, true);
    //            MsgBox.ShowOK("商户已停用");
    //            return;
    //        }
    //        else if (code.Equals("010003"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：商户未开通此产品" + data, true);
    //            MsgBox.ShowOK("商户未开通此产品");
    //            return;
    //        }
    //        else if (code.Equals("010004"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：IP非法" + data, true);
    //            MsgBox.ShowOK("IP非法");
    //            return;
    //        }
    //        else if (code.Equals("020001"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：请求参数有误" + data, true);
    //            MsgBox.ShowOK("请求参数有误");
    //            return;
    //        }
    //        else if (code.Equals("020002"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：签名验证未通过" + data, true);
    //            MsgBox.ShowOK("签名验证未通过");
    //            return;
    //        }
    //        else if (code.Equals("030001"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：重复订单" + data, true);
    //            MsgBox.ShowOK("重复订单");
    //            return;
    //        }
    //        else if (code.Equals("030002"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：无可用渠道" + data, true);
    //            MsgBox.ShowOK("无可用渠道");
    //            return;
    //        }
    //        else if (code.Equals("030003"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：单笔金额超限" + data, true);
    //            MsgBox.ShowOK("单笔金额超限");
    //            return;
    //        }
    //        else if (code.Equals("030004"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：当日累计金额超限" + data, true);
    //            MsgBox.ShowOK("当日累计金额超限");
    //            return;
    //        }
    //        else if (code.Equals("030005"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：查询订单不存在" + data, true);
    //            MsgBox.ShowOK("查询订单不存在");
    //            return;
    //        }
    //        else if (code.Equals("030006"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：原订单不存在" + data, true);
    //            MsgBox.ShowOK("原订单不存在");
    //            return;
    //        }
    //        else if (code.Equals("030007"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：退款金额大于可退金额" + data, true);
    //            MsgBox.ShowOK("退款金额大于可退金额");
    //            return;
    //        }
    //        else if (code.Equals("030008"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：对账文件未生成" + data, true);
    //            MsgBox.ShowOK("对账文件未生成");
    //            return;
    //        }
    //        else if (code.Equals("030009"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：原支付订单未成功，无法退款" + data, true);
    //            MsgBox.ShowOK("原支付订单未成功，无法退款");
    //            return;
    //        }
    //        else if (code.Equals("030010"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：订单超过有效时间，失效" + data, true);
    //            MsgBox.ShowOK("订单超过有效时间，失效");
    //            return;
    //        }
    //        else if (code.Equals("030011"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：不支持当日退款" + data, true);
    //            MsgBox.ShowOK("不支持当日退款");
    //            return;
    //        }
    //        else if (code.Equals("030012"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：支付模式未开通" + data, true);
    //            MsgBox.ShowOK("支付模式未开通");
    //            return;
    //        }
    //        else if (code.Equals("999999"))
    //        {
    //            Logger.Logging(logHeader, "服务器返回为：未知错误" + data, true);
    //            MsgBox.ShowOK("未知错误");
    //            return;
    //        }
    //    }
    //    public static String GetCode()
    //    {
    //        return code;
    //    }
    //    private static MessageWorker.trafficMessage SendMessageSample(
    //        string pfxFilePath,
    //        string pfxPassword,
    //        string cerFilePath,
    //        string ServerUrl,
    //        string signType,
    //        string charset,
    //        Dictionary<string, object> msgData)
    //    {

    //        //报文结构体初始化
    //        MessageWorker.trafficMessage msgRequestSource = new MessageWorker.trafficMessage();
    //        //发送类实体化
    //        MessageWorker worker = new MessageWorker();


    //        worker.PFXFile = pfxFilePath; //商户pfx证书路径
    //        worker.PFXPassword = pfxPassword;  //商户pfx证书密码
    //        worker.CerFile = cerFilePath; //杉德cer证书路径


    //        msgRequestSource.charset = charset; //商户号
    //        msgRequestSource.signType = signType;        //交易代码
    //        msgRequestSource.extend = "";               //扩展域

    //        //报文体json
    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        msgRequestSource.data = jsonSer.Serialize(msgData);


    //        Logger.Logging(logHeader, "待发送报文为：" + msgRequestSource.data, true);

    //        MessageWorker.trafficMessage respMessage =  worker.postMessage(ServerUrl, msgRequestSource);

    //        Logger.Logging(logHeader, "服务器返回为：" + respMessage.data, true);
    //        return respMessage;
    //    }
    //    private static Dictionary<string, object> QrCode_PreCreate(string bankecode, string merid, string paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone, string orderCode)
    //    {
    //        string strDateTime = System.DateTime.Now.ToString("yyyyMMddhhmmss");
    //        string payTool = "";
    //        string productId = "";


    //        //if (paytype.Equals("银联扫码"))
    //        //{
    //        //    payTool = "0403";
    //        //    productId = "00000012";
    //        //}
    //        //else if (paytype.Equals("微信扫码"))
    //        //{
    //        //    payTool = "0402";
    //        //    productId = "00000005";
    //        //}
    //        //else if (paytype.Equals("支付宝扫码"))
    //        //{
    //        //    payTool = "0401";
    //        //    productId = "00000006";
    //        //} else if (paytype.Equals("网银支付"))
    //        //{
    //        //    productId = "00000007";
    //        //}
    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                productId = "00000007";
    //                break;
    //            case "支付宝扫码":
    //                payTool = "0401"; productId = "00000006";
    //                break;
    //            case "微信扫码":
    //                payTool = "0402"; productId = "00000005";
    //                break;
    //            case "银联扫码":
    //                payTool = "0403"; productId = "00000012";
    //                break;
    //        }
    //        //将金额转为000000000101格式
    //        string totalAmount = Convert.ToInt32(decimal.Round(decimal.Parse(RechargeNum), 2) * 100).ToString().ToString();
    //        int num = totalAmount.Length;
    //        for (int i = 0; i < 12 - num; i++)
    //        {
    //            totalAmount = "0" + totalAmount;
    //        }

    //        Dictionary<string, object> head = new Dictionary<string, object>();
    //        head.Add("accessType", "1");//接入类型
    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                head.Add("subMid", "");
    //                head["method"] = "sandpay.trade.pay";
    //                head.Add("productId", productId);//00000005微信扫码00000006支付宝扫码 00000012银联正扫 00000013银联反扫
    //                head.Add("mid", merid);
    //                head.Add("subMidName", "");
    //                //head.Add("plMid", merid);//接入类型为2时必填
    //                head.Add("channelType", "07");//渠道类型：07-互联网 08 - 移动端
    //                head.Add("reqTime", strDateTime);
    //                head.Add("subMidAddr", "");
    //                head.Add("version", "1.0");
    //                break;
    //            default:
    //                head.Add("method", "sandpay.trade.precreate");
    //                head.Add("productId", productId);//00000005微信扫码00000006支付宝扫码 00000012银联正扫 00000013银联反扫
    //                head.Add("mid", merid);
    //                head.Add("channelType", "07");//渠道类型：07-互联网 08 - 移动端
    //                head.Add("reqTime", strDateTime);
    //                head.Add("version", "1.0");
    //                break;
    //        }

    //        Dictionary<string, object> body = new Dictionary<string, object>();
    //        if (paytype.Equals("网银支付")) { body.Add("payMode", "bank_pc"); }
    //        body.Add("subject", "账户充值");
    //        if (paytype.Equals("网银支付")) { body.Add("frontUrl", "http://61.129.71.103:8003/jspsandpay/payReturn.jsp"); }
    //        body.Add("terminalId", "");
    //        body.Add("body", "账户充值");
    //        body.Add("storeId", "");
    //        body.Add("merchExtendParams", "");
    //        body.Add("extend", "");
    //        body.Add("totalAmount", totalAmount);//
    //        body.Add("txnTimeOut", DateTime.Now.AddMinutes(10).ToString("yyyyMMddhhmmss"));//表示十分钟后过期
    //        body.Add("bizExtendParams", "");
    //        if (paytype.Equals("网银支付")) { body.Add("clientIp", GetInternalIP());
    //            body.Add("payExtra", "{\"bankCode\":\"" + bankecode + "\",\"payType\":\"3\"}");//(Request.Form["payExtra"].Length == 0) ? new Dictionary<string, object>() : jsonSer.Deserialize<Dictionary<string, object>>(Request.Form["payExtra"]
    //        }
    //        body.Add("notifyUrl", "http://localhost:8080/ZQTMS/asyncNotify");
    //        body.Add("orderCode", orderCode);
    //        body.Add("operatorId", "");
    //        Console.WriteLine(DateTime.Now.AddMinutes(10).ToString("yyyyMMddhhmmss"));
            
    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                //head["method"] = "sandpay.trade.pay";
    //                //
    //                //body.Add("frontUrl", "http://61.129.71.103:8003/jspsandpay/payReturn.jsp");
    //                //body.Add("payExtra", "{\"bankCode\":\""+ bankecode + "\",\"payType\":\"1\"}");//(Request.Form["payExtra"].Length == 0) ? new Dictionary<string, object>() : jsonSer.Deserialize<Dictionary<string, object>>(Request.Form["payExtra"]
    //                //body.Add("payMode", "bank_pc");
    //                //body.Add("clearCycle", "");//清算模式0-T1(默认) 1-T0 2-D0
    //                //body.Add("riskRateInfo", "");//风控信息域JSON结构
    //                break;
    //            default:
    //                //head.Add("method", "sandpay.trade.precreate");
    //                body.Add("limitPay", "");//1-限定不能使用信用卡
    //                body.Add("payTool", payTool);//0401：支付宝扫码  0402：微信扫码 0403：银联扫码
    //                //body.Add("clearCycle", "");//清算模式0-T1(默认) 1-T0 2-D0
    //                //body.Add("riskRateInfo", "");//风控信息域JSON结构
    //                break;
    //        }

    //        Dictionary<string, object> dic = new Dictionary<string, object>();
    //        dic.Add("head", head);
    //        dic.Add("body", body);
    //        return dic;
    //    }

    //    //获取内网IP
    //    public static string GetInternalIP()
    //    {
    //          IPHostEntry host;
    //          string localIP = "?";
    //          host = Dns.GetHostEntry(Dns.GetHostName());
    //          foreach (IPAddress ip in host.AddressList)
    //         {
    //             if (ip.AddressFamily.ToString() == "InterNetwork")
    //             {
    //                 localIP = ip.ToString();
    //                 break;
    //             }
    //         }
    //         return localIP;
    //     }

    //    private static string PostZQTMS(string serverUrl,string bankecode, string merid, string paytype, string accountname, string accountno, string balance, string RechargeNum, string Phone, string orderCode)
    //    {
    //        string strDateTime = System.DateTime.Now.ToString("yyyyMMddhhmmss");
    //        string payTool = "";
    //        string productId = "";
    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                productId = "00000007";
    //                break;
    //            case "支付宝扫码":
    //                payTool = "0401"; productId = "00000006";
    //                break;
    //            case "微信扫码":
    //                payTool = "0402"; productId = "00000005";
    //                break;
    //            case "银联扫码":
    //                payTool = "0403"; productId = "00000012";
    //                break;
    //        }
    //        //将金额转为000000000101格式
    //        string totalAmount = Convert.ToInt32(decimal.Round(decimal.Parse(RechargeNum), 2) * 100).ToString().ToString();
    //        int num = totalAmount.Length;
    //        for (int i = 0; i < 12 - num; i++)
    //        {
    //            totalAmount = "0" + totalAmount;
    //        }

    //        Dictionary<string, object> head = new Dictionary<string, object>();
    //        head.Add("accessType", "1");//接入类型
    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                //head.Add("subMid", "");
    //                //head["method"] = "sandpay.trade.pay";
    //                //head.Add("productId", productId);//00000005微信扫码00000006支付宝扫码 00000012银联正扫 00000013银联反扫
    //                //head.Add("mid", merid);
    //                //head.Add("subMidName", "");
    //                //head.Add("plMid", merid);//接入类型为2时必填
    //                //head.Add("channelType", "07");//渠道类型：07-互联网 08 - 移动端
    //                //head.Add("reqTime", strDateTime);
    //                //head.Add("subMidAddr", "");
    //                head.Add("version", "1.0");//更新与java接口版本同步（每次更新必须更新该值）
    //                break;
    //            default:
    //                head.Add("method", "sandpay.trade.precreate");
    //                head.Add("productId", productId);//00000005微信扫码00000006支付宝扫码 00000012银联正扫 00000013银联反扫
    //                //head.Add("mid", merid);
    //                //head.Add("channelType", "07");//渠道类型：07-互联网 08 - 移动端
    //                head.Add("reqTime", strDateTime);
    //                head.Add("version", "1.0");//更新与java接口版本同步（每次更新必须更新该值）
    //                break;
    //        }

    //        Dictionary<string, object> body = new Dictionary<string, object>();
    //        body.Add("subject", "账户充值");
    //        body.Add("terminalId", "");
    //        body.Add("body", "账户充值");
    //        body.Add("storeId", "");
    //        body.Add("merchExtendParams", "");
    //        body.Add("extend", "");
    //        body.Add("totalAmount", totalAmount);//
    //        body.Add("txnTimeOut", DateTime.Now.AddMinutes(10).ToString("yyyyMMddhhmmss"));//表示十分钟后过期
    //        body.Add("bizExtendParams", "");
    //        body.Add("notifyUrl", "http://ZQTMS.dekuncn.com:8077/asyncNotify");
    //        body.Add("orderCode", orderCode);
    //        body.Add("operatorId", "");
    //        Console.WriteLine(DateTime.Now.AddMinutes(10).ToString("yyyyMMddhhmmss"));

    //        switch (paytype)
    //        {
    //            case "网银支付":
    //                //head["method"] = "sandpay.trade.pay";
    //                //body.Add("frontUrl", "http://61.129.71.103:8003/jspsandpay/payReturn.jsp");
    //                //body.Add("clearCycle", "");//清算模式0-T1(默认) 1-T0 2-D0
    //                //body.Add("riskRateInfo", "");//风控信息域JSON结构
    //                body.Add("payMode", "bank_pc");
    //                body.Add("frontUrl", "http://ZQTMS.dekuncn.com:8077/jsp/paysuccess.jsp");
    //                body.Add("clientIp", GetInternalIP());
    //                body.Add("payExtra", "{\"bankCode\":\"" + bankecode + "\",\"payType\":\"1\"}");
    //                break;
    //            default:
    //                body.Add("limitPay", "");//1-限定不能使用信用卡
    //                body.Add("payTool", payTool);//0401：支付宝扫码  0402：微信扫码 0403：银联扫码
    //                body.Add("clearCycle", "");//清算模式0-T1(默认) 1-T0 2-D0
    //                body.Add("riskRateInfo", "");//风控信息域JSON结构
    //                break;
    //        }

    //        Dictionary<string, object> dic = new Dictionary<string, object>();
    //        dic.Add("head", head);
    //        dic.Add("body", body);
    //        //报文结构体初始化
    //        MessageWorker.trafficMessage msgRequestSource = new MessageWorker.trafficMessage();
    //        //报文体json
    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        msgRequestSource.data = jsonSer.Serialize(dic);
            
    //        Logger.Logging(logHeader, "待发送报文为：" + msgRequestSource.data, true);
    //        string responseString = HttpUtils.HttpPost(serverUrl, msgRequestSource.data, Encoding.UTF8);
    //        return responseString;
    //    }
    //}
    //public class LLpay
    //{
    //    public static void LLPay( string accountname, string accountno, string balance, string RechargeNum, string Phone)
    //    {
    //        //默认扫码接口，否者在线支付接口
    //        string url = "http://ZQTMS.dekuncn.com:8078/LLOnlinePay";
    //        //string url = "http://localhost:8080/ZQTMSLLPay/LLOnlinePay";
    //        string strDateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
    //        string orderCode = strDateTime + new Random().Next(100, 999).ToString()+"1";//订单号1区分网银
    //        string strDate = System.DateTime.Now.ToString("yyyyMMdd");

    //        //将金额转为000000000101格式
    //        string totalAmount = decimal.Round(decimal.Parse(RechargeNum), 2).ToString().ToString();

    //        //Dictionary<string, object> body = new Dictionary<string, object>();
    //        //body.Add("version", "1.0");//更新与java接口版本同步（每次更新必须更新该值）
    //        //body.Add("totalAmount", totalAmount);//
    //        //body.Add("orderCode", orderCode);
    //        //body.Add("clientIp", SDpay.GetInternalIP());
    //        //body.Add("Userid", accountno);
    //        //body.Add("pay_type", paytype);
    //        //body.Add("bank_code", bankcode);
    //        //Console.WriteLine(DateTime.Now.AddMinutes(10).ToString("yyyyMMddhhmmss"));
    //        //Dictionary<string, object> dic = new Dictionary<string, object>();
    //        //dic.Add("body", body);
    //        ////报文结构体初始化
    //        //MessageWorker.trafficMessage msgRequestSource = new MessageWorker.trafficMessage();
    //        ////报文体json
    //        //JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        //msgRequestSource.data = jsonSer.Serialize(dic);

    //        //Logger.Logging("Program_LLpay", "待发送报文为：" + msgRequestSource.data, true);
    //        Dictionary<string, object> body = new Dictionary<string, object>();
    //        body.Add("version", "1.0");
    //        body.Add("totalAmount", totalAmount);
    //        body.Add("orderCode", orderCode);
    //        body.Add("clientIp", SDpay.GetInternalIP());
    //        body.Add("Userid", accountno);
    //        //body.Add("pay_type", paytype);
    //        //body.Add("bank_code", bankcode);
    //        body.Add("phone", Phone);
    //        body.Add("uname", CommonClass.UserInfo.UserName.Replace("Test", ""));
    //        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
    //        string data = jsonSer.Serialize(body);

    //        //插入充值记录
    //        List<SqlPara> list = new List<SqlPara>();
    //        list.Add(new SqlPara("accountNo", accountno.Trim()));
    //        list.Add(new SqlPara("accountName", accountname.Trim()));
    //        list.Add(new SqlPara("custPhone", Phone.Trim()));
    //        list.Add(new SqlPara("payMode", "账户充值"));
    //        list.Add(new SqlPara("inkey", ""));//接口验证码
    //        list.Add(new SqlPara("paySysDate", strDate));
    //        list.Add(new SqlPara("serialNum", ""));
    //        list.Add(new SqlPara("merNo", "201808070002105005"));//
    //        list.Add(new SqlPara("merOrderId", orderCode));
    //        list.Add(new SqlPara("merOrderName", accountname.Trim()));
    //        list.Add(new SqlPara("merShortName", ""));
    //        list.Add(new SqlPara("price", 0));
    //        list.Add(new SqlPara("merOrderAmt", Convert.ToDecimal(RechargeNum).ToString()));
    //        list.Add(new SqlPara("merOrderCount", "1"));
    //        list.Add(new SqlPara("payType", 1));//支付类型
    //        list.Add(new SqlPara("salePayAcct", ""));
    //        list.Add(new SqlPara("custPayAcct", ""));
    //        list.Add(new SqlPara("merOrderStatus", ""));
    //        list.Add(new SqlPara("payUrl", ""));
    //        list.Add(new SqlPara("Errorcode", ""));

    //        SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_LLPAY", list));

    //        //调用java接口进行支付
    //        if (frmAccountRechargeNew.OpenBrowser(url+ "?data="+ HttpUtility.UrlEncode(data)) == false)
    //        {
    //            if (frmAccountRechargeNew.OpenBrowsers(url + "?data=" + HttpUtility.UrlEncode(data)) == false)
    //            {
    //                MessageBox.Show("打开浏览器失败！");
    //                return;
    //            }
    //        }
    //        //string msgData = PostLLZQTMS(url, accountno, RechargeNum, orderCode);
    //        //if (msgData.Equals("error"))
    //        //{
    //        //    MessageBox.Show("充值请求出错！");
    //        //    return;
    //        //}
    //        //else if (!msgData.Contains("http") && !msgData.Contains("weixin"))
    //        //{
    //        //    MessageBox.Show(msgData);
    //        //    return;
    //        //}
    //        //if (!paytype.Equals("网银支付"))
    //        //{
    //        //    //code = code + "|" + msgData;
    //        //}
    //        //else
    //        //{
    //        //    try
    //        //    {
    //        //        System.Diagnostics.Process.Start(msgData);
    //        //    }
    //        //    catch (Exception ex)
    //        //    {
    //        //        MessageBox.Show("充值打开浏览器失败");
    //        //    }
    //        //}
          
    //    }
    //}
//public class INIClass
//    {
//        public string inipath;
//        [System.Runtime.InteropServices.DllImport("kernel32")]
//        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
//        [System.Runtime.InteropServices.DllImport("kernel32")]
//        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
//        /// <summary>
//        /// 构造方法
//        /// </summary>
//        /// <param name="INIPath">文件路径</param>
//        public INIClass(string INIPath)
//        {
//            inipath = INIPath;
//        }
//        /// <summary>
//        /// 写入INI文件
//        /// </summary>
//        /// <param name="Section">项目名称(如 [TypeName] )</param>
//        /// <param name="Key">键</param>
//        /// <param name="Value">值</param>
//        public void IniWriteValue(string Section, string Key, string Value)
//        {
//            WritePrivateProfileString(Section, Key, Value, this.inipath);
//        }
//        /// <summary>
//        /// 读出INI文件
//        /// </summary>
//        /// <param name="Section">项目名称(如 [TypeName] )</param>
//        /// <param name="Key">键</param>
//        public string IniReadValue(string Section, string Key)
//        {
//            StringBuilder temp = new StringBuilder(500);
//            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
//            return temp.ToString();
//        }
//        /// <summary>
//        /// 验证文件是否存在
//        /// </summary>
//        /// <returns>布尔值</returns>
//        public bool ExistINIFile()
//        {
//            return File.Exists(inipath);
//        }
//    }

    //public class MessageWorker
    //{
    //    string loggerHeader = "MessageWorker_";
    //    public struct trafficMessage
    //    {
    //        public string charset;  //合作商户ID 杉德系统分配，唯一标识
    //        public string signType;// 加密后的AES秘钥 公钥加密(RSA/ECB/PKCS1Padding)，加密结果采用base64编码
    //        public string data; //加密后的请求/应答报文 AES加密(AES/ECB/PKCS5Padding)，加密结果采用base64编码
    //        public string sign;//    签名 对encryptData对应的明文进行签名(SHA1WithRSA)，签名结果采用base64编码
    //        public string extend;//  扩展域 暂时不用
    //    }

    //    private Encoding encodeCode = Encoding.UTF8;


    //    private string pfxFilePath = string.Empty;
    //    public string PFXFile
    //    {
    //        get
    //        {
    //            return pfxFilePath;
    //        }
    //        set
    //        {
    //            pfxFilePath = value;
    //        }
    //    }

    //    private string pfxPassword = string.Empty;
    //    public string PFXPassword
    //    {
    //        get
    //        {
    //            return pfxPassword;
    //        }
    //        set
    //        {
    //            pfxPassword = value;
    //        }
    //    }


    //    private string cerFilePath = string.Empty;
    //    public string CerFile
    //    {
    //        get
    //        {
    //            return cerFilePath;
    //        }
    //        set
    //        {
    //            cerFilePath = value;
    //        }
    //    }
    //    public trafficMessage UrlDecodeMessage(string msgResponse)
    //    {
    //        trafficMessage msgEncrypt = new trafficMessage();
    //        string[] EncryptBody = HttpUtility.UrlDecode(msgResponse).Split('&');
    //        for (int i = 0; i < EncryptBody.Length; i++)
    //        {
    //            string[] tmp = EncryptBody[i].Split('=');
    //            switch (tmp[0])
    //            {
    //                //需要添加引用System.Web，用于url转码，处理base64产生的+/=
    //                case "charset": msgEncrypt.charset = EncryptBody[i].Replace("charset=", "").Trim('"'); break;
    //                case "signType": msgEncrypt.signType = EncryptBody[i].Replace("signType=", "").Trim('"'); break;
    //                case "data": msgEncrypt.data = EncryptBody[i].Replace("data=", "").Trim('"'); break;
    //                case "sign": msgEncrypt.sign = EncryptBody[i].Replace("sign=", "").Trim('"'); break;
    //                case "extend": msgEncrypt.extend = EncryptBody[i].Replace("extend=", "").Trim('"'); break;
    //            }
    //        }
    //        return msgEncrypt;
    //    }
    //    public string UrlEncodeMessage(trafficMessage msgRequest)
    //    {
    //        //需要添加引用System.Web，用于url转码，处理base64产生的+/=
    //        return "charset=" + System.Web.HttpUtility.UrlEncode(msgRequest.charset) + "&" +
    //             "signType=" + System.Web.HttpUtility.UrlEncode(msgRequest.signType) + "&" +
    //              "data=" + System.Web.HttpUtility.UrlEncode(msgRequest.data) + "&" +
    //               "sign=" + System.Web.HttpUtility.UrlEncode(msgRequest.sign) + "&" +
    //               "extend=" + System.Web.HttpUtility.UrlEncode(msgRequest.extend);
    //    }



    //    public trafficMessage SignMessageBeforePost(trafficMessage msgSource)
    //    {
    //        trafficMessage msgEncrypt = new trafficMessage();


    //        //获取报文字符集
    //        this.encodeCode = Encoding.GetEncoding(msgSource.charset);
    //        Logger.Logging(loggerHeader, "Message Charset is [" + msgSource.charset + "]", true);

    //        msgEncrypt.charset = msgSource.charset;
    //        msgEncrypt.signType = msgSource.signType;
    //        msgEncrypt.extend = msgSource.extend;
    //        msgEncrypt.data = msgSource.data;

    //        //报文签名
    //        msgEncrypt.sign = CryptUtils.Base64Encoder(CryptUtils.CreateSignWithPrivateKey(CryptUtils.getBytesFromString(msgSource.data, encodeCode),
    //            CryptUtils.getPrivateKeyXmlFromPFX(pfxFilePath, pfxPassword)));

    //        Logger.Logging(loggerHeader, "sign[" + msgEncrypt.sign + "]", true);

    //        return msgEncrypt;

    //    }
    //    public trafficMessage CheckSignMessageAfterResponse(trafficMessage msgEncrypt)
    //    {
    //        trafficMessage msgSource = new trafficMessage();

    //        //获取报文字符集
    //        this.encodeCode = Encoding.GetEncoding(msgEncrypt.charset);
    //        Logger.Logging(loggerHeader, "Message Charset is [" + msgEncrypt.charset + "]", true);

    //        msgSource.charset = msgEncrypt.charset;
    //        msgSource.signType = msgEncrypt.signType;
    //        msgSource.extend = msgEncrypt.extend;
    //        msgSource.data = msgEncrypt.data;

    //        msgSource.sign = CryptUtils.VerifySignWithPublicKey(
    //                            (CryptUtils.getBytesFromString(msgEncrypt.data, encodeCode)),
    //                            CryptUtils.getPublicKeyXmlFromCer(cerFilePath),
    //                            CryptUtils.Base64Decoder(msgEncrypt.sign)
    //                            ).ToString();

    //        Logger.Logging(loggerHeader, "sign[" + msgSource.sign + "][" + msgEncrypt.sign + "]", true);
    //        return msgSource;
    //    }


    //    public trafficMessage postMessage(string serverUrl, trafficMessage requestSourceMessage)
    //    {
    //        trafficMessage responseMessage = new trafficMessage();
    //        // try
    //        {
    //            string requestString = UrlEncodeMessage(SignMessageBeforePost(requestSourceMessage));
    //            //Console.WriteLine("url:" + serverUrl);
    //            Logger.Logging(loggerHeader, "request  ==>[" + requestString + "]", true);

    //            string responseString = HttpUtils.HttpPost(serverUrl, requestString, encodeCode);
    //            Logger.Logging(loggerHeader, "response <==[" + responseString + "]", true);
    //            responseMessage = CheckSignMessageAfterResponse(UrlDecodeMessage(responseString));
    //        }
    //        //  catch (Exception er)
    //        {
    //            //    Console.WriteLine(er.ToString());
    //        }
    //        return responseMessage;
    //    }
    //}

    //public class HttpUtils
    //{
    //    public static string HttpPost(string postUrl, string paramData, Encoding EncodingName)
    //    {
    //        string postDataStr = paramData;
    //        byte[] buff = EncodingName.GetBytes(postDataStr);

    //        HttpWebRequest request = null;
    //        //如果是发送HTTPS请求  
    //        if (postUrl.StartsWith("https", StringComparison.OrdinalIgnoreCase))
    //        {
    //            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
    //            request = WebRequest.Create(postUrl) as HttpWebRequest;
    //            request.ProtocolVersion = HttpVersion.Version10;
    //        }
    //        else
    //        {
    //            request = WebRequest.Create(postUrl) as HttpWebRequest;
    //        }

    //        request.Method = "POST";
    //        request.ContentType = "application/x-www-form-urlencoded";

    //        Stream myRequestStream = request.GetRequestStream();
    //        myRequestStream.Write(buff, 0, buff.Length);
    //        myRequestStream.Close();

    //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    //        Stream myResponseStream = response.GetResponseStream();
    //        StreamReader myStreamReader = new StreamReader(myResponseStream, EncodingName);
    //        string retString = myStreamReader.ReadToEnd();
    //        myStreamReader.Close();
    //        myResponseStream.Close();
    //        return retString;
    //    }

    //    private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
    //    {
    //        return true; //总是接受  
    //    }
    //}
    //public class CryptUtils
    //{
    //    public static byte[] AESEncrypt(byte[] Data, string Key)
    //    {
    //        RijndaelManaged rm = new RijndaelManaged
    //        {
    //            Key = Encoding.UTF8.GetBytes(Key),
    //            Mode = CipherMode.ECB,
    //            Padding = PaddingMode.PKCS7
    //        };

    //        ICryptoTransform cTransform = rm.CreateEncryptor();
    //        byte[] resultArray = cTransform.TransformFinalBlock(Data, 0, Data.Length);

    //        return resultArray;
    //    }

    //    public static byte[] AESDecrypt(byte[] Data, string Key)
    //    {

    //        RijndaelManaged rm = new RijndaelManaged
    //        {
    //            Key = Encoding.UTF8.GetBytes(Key),
    //            Mode = CipherMode.ECB,
    //            Padding = PaddingMode.PKCS7
    //        };

    //        ICryptoTransform cTransform = rm.CreateDecryptor();
    //        byte[] resultArray = cTransform.TransformFinalBlock(Data, 0, Data.Length);

    //        return resultArray;
    //    }


    //    public static string GuidTo16String()
    //    {
    //        string dic = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    //        int keylen = dic.Length;
    //        long x = 1;
    //        foreach (byte b in Guid.NewGuid().ToByteArray())
    //            x *= ((int)b + 1);
    //        string value = string.Empty;
    //        Random ra = new Random((int)(x & 0xffffffffL) | (int)(x >> 32));
    //        for (int i = 0; i < 16; i++)
    //        {
    //            value += dic[ra.Next() % keylen];
    //        }
    //        return value;
    //    }

    //    public static string getStringFromBytes(byte[] hexbytes, Encoding enc)
    //    {
    //        return enc.GetString(hexbytes);
    //    }

    //    public static byte[] getBytesFromString(string str, Encoding enc)
    //    {
    //        return enc.GetBytes(str);
    //    }

    //    public static byte[] asc2hex(string hexString)
    //    {

    //        byte[] returnBytes = new byte[hexString.Length / 2];
    //        for (int i = 0; i < returnBytes.Length; i++)
    //            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
    //        return returnBytes;
    //    }

    //    public static string hex2asc(byte[] hexbytes)
    //    {
    //        return BitConverter.ToString(hexbytes).Replace("-", string.Empty);

    //    }
    //    /// <summary>   
    //    /// RSA解密   要加密较长的数据，则可以采用分段加解密的方式
    //    /// </summary>   
    //    /// <param name="xmlPrivateKey"></param>   
    //    /// <param name="EncryptedBytes"></param>   
    //    /// <returns></returns>   
    //    public static byte[] RSADecrypt(string xmlPrivateKey, byte[] EncryptedBytes)
    //    {

    //        using (RSACryptoServiceProvider RSACryptography = new RSACryptoServiceProvider())
    //        {
    //            RSACryptography.FromXmlString(xmlPrivateKey);


    //            int MaxBlockSize = RSACryptography.KeySize / 8;    //解密块最大长度限制

    //            if (EncryptedBytes.Length <= MaxBlockSize)
    //                return RSACryptography.Decrypt(EncryptedBytes, false);

    //            using (MemoryStream CrypStream = new MemoryStream(EncryptedBytes))
    //            using (MemoryStream PlaiStream = new MemoryStream())
    //            {
    //                Byte[] Buffer = new Byte[MaxBlockSize];
    //                int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);

    //                while (BlockSize > 0)
    //                {
    //                    Byte[] ToDecrypt = new Byte[BlockSize];
    //                    Array.Copy(Buffer, 0, ToDecrypt, 0, BlockSize);

    //                    Byte[] Plaintext = RSACryptography.Decrypt(ToDecrypt, false);
    //                    PlaiStream.Write(Plaintext, 0, Plaintext.Length);

    //                    BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
    //                }

    //                return PlaiStream.ToArray();
    //            }
    //        }
    //    }


    //    /// <summary>   
    //    /// RSA加密   要加密较长的数据，则可以采用分段加解密的方式
    //    /// </summary>   
    //    /// <param name="xmlPublicKey"></param>   
    //    /// <param name="SourceBytes"></param>   
    //    /// <returns></returns>   
    //    public static byte[] RSAEncrypt(string xmlPublicKey, byte[] SourceBytes)
    //    {
    //        using (RSACryptoServiceProvider RSACryptography = new RSACryptoServiceProvider())
    //        {
    //            RSACryptography.FromXmlString(xmlPublicKey);

    //            int MaxBlockSize = RSACryptography.KeySize / 8 - 11;    //加密块最大长度限制

    //            if (SourceBytes.Length <= MaxBlockSize)
    //                return RSACryptography.Encrypt(SourceBytes, false);

    //            using (MemoryStream PlaiStream = new MemoryStream(SourceBytes))
    //            using (MemoryStream CrypStream = new MemoryStream())
    //            {
    //                Byte[] Buffer = new Byte[MaxBlockSize];
    //                int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);

    //                while (BlockSize > 0)
    //                {
    //                    Byte[] ToEncrypt = new Byte[BlockSize];
    //                    Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

    //                    Byte[] Cryptograph = RSACryptography.Encrypt(ToEncrypt, false);
    //                    CrypStream.Write(Cryptograph, 0, Cryptograph.Length);

    //                    BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
    //                }

    //                return CrypStream.ToArray();
    //            }
    //        }
    //    }


    //    public static X509Certificate2 getPrivateKeyXmlFromPFX(string PFX_file, string password)
    //    {
    //        //return new X509Certificate2(PFX_file, password, X509KeyStorageFlags.Exportable);X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
    //        X509Certificate2 pfx = new X509Certificate2(PFX_file, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
    //        return pfx;
    //    }

    //    public static X509Certificate2 getPublicKeyXmlFromCer(string Cer_file)
    //    {
    //        return new X509Certificate2(Cer_file);
    //    }


    //    public static byte[] CreateSignWithPrivateKey(byte[] msgin, X509Certificate2 pfx)
    //    {
    //        HashAlgorithm SHA1 = HashAlgorithm.Create("sha1");
    //        byte[] hashdata = SHA1.ComputeHash(msgin);//求数字指纹

    //        try
    //        {
    //            AsymmetricAlgorithm prikey = pfx.PrivateKey;
    //        }
    //        catch (Exception er)
    //        {
    //            Console.WriteLine(er.ToString());
    //        }

    //        RSAPKCS1SignatureFormatter signCrt = new RSAPKCS1SignatureFormatter();
    //        signCrt.SetKey(pfx.PrivateKey);
    //        signCrt.SetHashAlgorithm("sha1");
    //        return signCrt.CreateSignature(hashdata);
    //    }

    //    public static bool VerifySignWithPublicKey(byte[] msgin, X509Certificate2 cer, byte[] sign)
    //    {
    //        HashAlgorithm SHA1 = HashAlgorithm.Create("sha1");
    //        byte[] hashdata = SHA1.ComputeHash(msgin);//求数字指纹

    //        RSACryptoServiceProvider signV = new RSACryptoServiceProvider();
    //        signV.FromXmlString(cer.PublicKey.Key.ToXmlString(false));
    //        return signV.VerifyData(msgin, CryptoConfig.MapNameToOID("SHA1"), sign);
    //    }

    //    public static string Base64Encoder(byte[] b)
    //    {
    //        return Convert.ToBase64String(b);
    //    }

    //    public static byte[] Base64Decoder(string base64String)
    //    {
    //        return Convert.FromBase64String(base64String);
    //    }

    //}

    /// <summary>
    /// Logger随便写的，会有并发问题，不能投产哦，建议使用log4net替换掉
    /// </summary>
    //public static class Logger
    //{
    //    public static void Logging(string LogFileHead, string message, bool filewrite)
    //    {
    //        Console.WriteLine(DateTime.Now.ToString().PadRight(20) + message);
    //        //if (filewrite)
    //        //{
    //        //    StreamWriter sw = new StreamWriter(LogFileHead + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", true, Encoding.GetEncoding("gb2312"));
    //        //    sw.WriteLine(DateTime.Now.ToString().PadRight(20) + message);
    //        //    sw.Close();
    //        //}
    //    }
    //}

}