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
using Newtonsoft.Json;
namespace ZQTMS.UI.其他费用
{
    public partial class frmWaybillCostDeficits : ZQTMS.Tool.BaseForm
    {
        public   decimal PaymentAmout = 0;
        public decimal heji = 0;
        public bool issave = false;
        public string billno="";
        string randCode;
        public string ConsignorCompany = "";
        public string TransferSite = "";
        public string Nums="";
        public string Varietiess="";
        DataTable dt = new DataTable();
        TimeSpan ts = new TimeSpan();
        DateTime dt_now;

        public frmWaybillCostDeficits()
        {
            InitializeComponent();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            Saves();
        }

        private void frmWaybillCostDeficits_Load(object sender, EventArgs e)
        {
            this.PaymentAmouts.Text = PaymentAmout.ToString();
            this.pay_heji.Text = heji.ToString();
            this.cost.Text = Convert.ToString(PaymentAmout >= heji ? PaymentAmout - heji : heji - PaymentAmout);
        }


        private void send_Click(object sender, EventArgs e)
        {
            ts = new TimeSpan(0, 2, 59);
            string strMsg = "";
            string content;
            string mb;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            this.Countdown.Visible = true;
            timer1.Start();
            send.Enabled = false;
            string json = "";
            try
            {

                //生成验证码
                Random rd = new Random();
                randCode = rd.Next(100000, 999999).ToString();
                List<SqlPara> list = new List<SqlPara>();//
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART_name", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {

                    mb = sms.CheckMb("", ds.Tables[0].Rows[0]["DepPhone"].ToString());//验证电话号码格式是否正确 获取网点负责人电话

                    if (mb != "")
                    {
                        sms.LimitSMS("开单验证验证码");
                        //短信内容：马恭文（发货单位）发往广州（中转地）的100（件数）件配件（品名）实收1000（实收费用）元，结算合计1200元，差额200元，验证码：6666666，【川胜物流】
                        strMsg = "您好!" + ConsignorCompany + "发往" + TransferSite + "的" + Nums + "件" + Varietiess + "实收" + PaymentAmout + "元,结算合计为" + heji + "，差额为" + this.cost.Text + "，验证码为：" + randCode;
                        Dictionary<string, string> hashMap = new Dictionary<string, string>();
                        hashMap.Add("a", ConsignorCompany.Trim());//shipper
                        hashMap.Add("b", TransferSite.Trim());
                        hashMap.Add("c", Nums.Trim());
                        hashMap.Add("d", Varietiess.Trim());
                        hashMap.Add("e", PaymentAmout.ToString());
                        hashMap.Add("h", heji.ToString());
                        hashMap.Add("g", cost.Text.Trim());
                        hashMap.Add("k", randCode);
                        hashMap.Add("templateNumber", "47");
                        hashMap.Add("phone", mb);
                        hashMap.Add("signName", "中强物流");
                        json = JsonConvert.SerializeObject(hashMap);
                        timer1.Enabled = true;
                        timer1.Start();
                        send.Enabled = false;

                        //发送短信
                        if (sms.sengsmsALi(json, mb, strMsg))
                        {
                            //保存短信
                            sms.SaveSMSS("11", "短信中心发送", mb.Trim(), strMsg, CommonClass.gcdate, this.billno.ToString());
                            MsgBox.ShowOK("发送短信成功");
                            dt_now = DateTime.Now;

                        }
                        else
                        {
                            MsgBox.ShowError("验证码发送失败！");
                        }
                    }
                    else
                    {
                        MsgBox.ShowError("生成验证码失败！");
                    }


                }

            }
            catch (System.Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        private void Saves()
        {
            if (string.IsNullOrEmpty(randomcode.Text.Trim()))
            {
                MsgBox.ShowOK("验证码不能为空");
                return;
            }

            if (randomcode.Text.Trim().Length > 6)
            {
                MsgBox.ShowOK("验证码不正确");
                return;
            }
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("BillNo", billno.Trim()));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basTransferCode_BiLLNO_1", list);//gy20200725
                //DataSet ds = SqlHelper.GetDataSet(sps);
                //if (ds == null || ds.Tables[0].Rows.Count == 0) return;
                //计算发送验证码日期和当前日期差值
                TimeSpan ts = DateTime.Now - dt_now;
                if (ts.TotalSeconds > 120)
                {
                    MsgBox.ShowOK("验证码过期.请重新获取!");
                    return;
                }
                if (randCode != randomcode.Text.Trim())
                {
                    MsgBox.ShowOK("验证码不正确,请重新输入!");
                    this.randomcode.Focus();
                    return;

                }
                else
                {
                    this.timer1.Stop();
                    this.Close();
                    issave = true;


                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int str = ConvertType.ToInt32(ts.Seconds.ToString());
            int str1 = ConvertType.ToInt32(ts.Minutes.ToString());
           if (str1 > 0)
            {
                str += 60;
            }

            Countdown.Text = "验证码剩余时间:" + str.ToString() + "秒";
            ts = ts.Subtract(new TimeSpan(0, 0, 1));
            if (ts.TotalSeconds < 0.0)
            {
                timer1.Enabled = false;
                send.Text = "重新发送";
                send.Enabled = true;
                MsgBox.ShowOK("验证码超时!");
                return;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
