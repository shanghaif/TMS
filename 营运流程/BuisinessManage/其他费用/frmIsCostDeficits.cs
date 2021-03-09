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
    public partial class frmIsCostDeficits : ZQTMS.Tool.BaseForm
    {


        public string DepartureBatch;
        public string StartSite="";
        public string DestinationSite="";
        public string MenuType = "";
        public  string SendWebName="";
        public  string MiddleWebName="";
        public decimal actual_rate = 0;
        public int middlecount=0;//批量中转单数
        public bool isprint = false;
        string randCode;
        DataTable dt = new DataTable();
        TimeSpan ts = new TimeSpan();
        public frmIsCostDeficits()
        {
            InitializeComponent();
            
          
        }
        SqlParasEntity sps;
        private void frmIsCostDeficits_Load(object sender, EventArgs e)
        {
            if (MenuType.Equals("汽运亏损"))
            {
                this.Text = "汽运亏损";
                this.label_rate.Text = "汽运成本率";
                this.label10.Text = "目标差异";
                this.label_pc.Text = "发车批次";
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("StartSite", StartSite.Trim()));
                list.Add(new SqlPara("DestinationSite", DestinationSite.Trim()));
                sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_1", list);
              
                

            }
            else  if (MenuType.Equals("送货亏损"))
            {
                this.Text = "送货亏损";
                this.label_rate.Text = "送货成本率";
                this.label10.Text = "目标差异";
                this.label_pc.Text = "送货批次";
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendWebName", SendWebName.Trim()));
                sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_2", list);
            }

            else if (MenuType.Equals("单票中转亏损"))
            {
                this.Text = "单票中转亏损";
                this.label_rate.Text = "中转成本率";
                this.label10.Text = "目标差异";
                this.label_pc.Text = "中转单号";
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleWebName", MiddleWebName.Trim()));
                sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_3", list);
              
            }
            else if (MenuType.Equals("批量中转亏损"))
            {
                this.Text = "批量中转亏损";
                this.label_rate.Text = "中转成本率";
                this.label10.Text = "目标差异";
                this.label_pc.Visible = false;
                this.billno.Visible = false;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleWebName", MiddleWebName.Trim()));
                 sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_3", list);

            }
            this.billno.Text = DepartureBatch;
            //this.actually_rate.Text = string.Format("{0:0.00%}", actual_rate); 
            this.actually_rate.Text = actual_rate.ToString("P0"); 
            DataSet ds_check = SqlHelper.GetDataSet(sps);
            if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
            {

                TargetcostRate.Text = ConvertType.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"]).ToString("P0");
                TelPhone.Text = ds_check.Tables[0].Rows[0]["TelPhone"].ToString();
                ChargePerson.Text = ds_check.Tables[0].Rows[0]["ChargePerson"].ToString();
                costrate.Text = (actual_rate - ConvertType.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"])).ToString("P0");
              


            }



        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ts = new TimeSpan(0, 2,59);
            string billno = this.billno.Text;
            string strMsg="";
            string content;
            string mb;
            timer1.Enabled = true;
            timer1.Interval = 1000;
            this.Countdown.Visible = true;
            timer1.Start();
            send.Enabled = false;
            string json="";
            try
            {

                //生成验证码
                Random rd = new Random();
                randCode = rd.Next(100000, 999999).ToString();
                List<SqlPara> list_code = new List<SqlPara>();
                list_code.Add(new SqlPara("TransferCode", randCode));
                list_code.Add(new SqlPara("BillNos", billno));
                list_code.Add(new SqlPara("Remark", "成本管控验证码"));
                list_code.Add(new SqlPara("CodeType", 0));
                SqlParasEntity sps_code = new SqlParasEntity(OperType.Execute, "USP_ADD_TransferCode_1", list_code);
                mb = sms.CheckMb("", this.TelPhone.Text.Trim());//验证电话号码格式是否正确
                sms.LimitSMS("成本管控验证码");
                if (SqlHelper.ExecteNonQuery(sps_code) > 0)
                {
                    //定义短信格式
                    //	汽运成本短信内容：你好，XXX,发车批次XX,汽运成本率XX，目标成本率XX，差额XX，验证码：XX				
                    if (MenuType.Equals("汽运亏损"))
                    {
                    strMsg = "您好!"+ ChargePerson.Text + "," + label_pc.Text.Trim() + "," + this.billno.Text.Trim() + ";" + this.label_rate.Text.Trim() + "为"+ this.actually_rate.Text.Trim()+"，目标成本率为" + this.TargetcostRate.Text.Trim() + "，差额为" + this.costrate.Text + "，验证码为：" + randCode;
                    Dictionary<string, string> hashMap = new Dictionary<string, string>();
                    hashMap.Add("a", ChargePerson.Text.Trim());//shipper
                    hashMap.Add("b", billno.ToString());
                    hashMap.Add("c", this.actually_rate.Text.Trim());
                    hashMap.Add("d", this.TargetcostRate.Text.Trim());
                    hashMap.Add("e", this.costrate.Text.Trim());
                    hashMap.Add("g", randCode);
                    hashMap.Add("templateNumber", "43");
                    hashMap.Add("phone", mb);
                    hashMap.Add("signName", "中强物流"); 
                    json = JsonConvert.SerializeObject(hashMap);
    
                    }
                    else if (MenuType.Equals("送货亏损"))
                    {
                        strMsg = "您好!" + ChargePerson.Text + "," + label_pc.Text.Trim() + "," + this.billno.Text.Trim() + ";" + this.label_rate.Text.Trim() + "为" + this.actually_rate.Text.Trim() + "，目标成本率为" + this.TargetcostRate.Text.Trim() + "，差额为" + this.costrate.Text + "，验证码为：" + randCode;
                        Dictionary<string, string> hashMap = new Dictionary<string, string>();
                        hashMap.Add("a", ChargePerson.Text.Trim());//shipper
                        hashMap.Add("b", billno.ToString());
                        hashMap.Add("c", this.actually_rate.Text.Trim());
                        hashMap.Add("d", this.TargetcostRate.Text.Trim());
                        hashMap.Add("e", this.costrate.Text.Trim());
                        hashMap.Add("g", randCode);
                        hashMap.Add("templateNumber", "44");
                        hashMap.Add("phone", mb);
                        hashMap.Add("signName", "中强物流"); 
                        json = JsonConvert.SerializeObject(hashMap);

                    }
                    else if (MenuType.Equals("批量中转亏损"))
                    {
                        ///你好，XXX,批量中转X单，中转成本率XX，目标成本XX，差额XX，验证码：XXX
                        strMsg = "您好!" + ChargePerson.Text + ",批量中转" + middlecount + "单;" + this.label_rate.Text.Trim() + "为" + this.actually_rate.Text.Trim() + "，目标成本率为" + this.TargetcostRate.Text.Trim() + "，差额为" + this.costrate.Text + "，验证码为：" + randCode;
                        Dictionary<string, string> hashMap = new Dictionary<string, string>();
                        hashMap.Add("a", ChargePerson.Text.Trim());//shipper
                        hashMap.Add("b", middlecount.ToString());
                        hashMap.Add("c", this.actually_rate.Text.Trim());
                        hashMap.Add("d", this.TargetcostRate.Text.Trim());
                        hashMap.Add("e", this.costrate.Text.Trim());
                        hashMap.Add("g", randCode);
                        hashMap.Add("templateNumber", "45");
                        hashMap.Add("phone", mb);
                        hashMap.Add("signName", "中强物流");
                        json = JsonConvert.SerializeObject(hashMap);
                    }
                    else if (MenuType.Equals("单票中转亏损"))
                    {
                        ///你好，XXX,单号XX，中转成本率XX，目标成本XX，差额XX，验证码：XXX
                        strMsg = "您好!" + ChargePerson.Text + ",单号为" + this.billno.Text.Trim() + ";" + this.label_rate.Text.Trim() + "为" + this.actually_rate.Text.Trim() + "，目标成本率为" + this.TargetcostRate.Text.Trim() + "，差额为" + this.costrate.Text + "，验证码为：" + randCode;
                        Dictionary<string, string> hashMap = new Dictionary<string, string>();
                        hashMap.Add("a", ChargePerson.Text.Trim());//shipper
                        hashMap.Add("b", billno.ToString());
                        hashMap.Add("c", this.actually_rate.Text.Trim());
                        hashMap.Add("d", this.TargetcostRate.Text.Trim());
                        hashMap.Add("e", this.costrate.Text.Trim());
                        hashMap.Add("g", randCode);
                        hashMap.Add("templateNumber", "46");
                        hashMap.Add("phone", mb);
                        hashMap.Add("signName", "中强物流"); //lms-6669
                        json = JsonConvert.SerializeObject(hashMap);
                    }
                    
                    timer1.Enabled = true;
                    timer1.Start();
                    send.Enabled = false;

                    //发送短信
                    if (sms.sengsmsALi(json, mb, strMsg))
                    {
                        //保存短信
                        sms.SaveSMSS("11", "短信中心发送", TelPhone.Text.Trim(), strMsg, CommonClass.gcdate, this.billno.Text.ToString());
                        MsgBox.ShowOK("发送短信成功");

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
            catch (System.Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
         /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(randomcode.Text.Trim()))
            {
                MsgBox.ShowOK("验证码不能为空");
                return;
            }
          
            if (randomcode.Text.Trim().Length>6)
            {
                MsgBox.ShowOK("验证码不正确");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billno.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basTransferCode_BiLLNO_1", list);//gy20200725
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0) return  ;
                DateTime dtime =ConvertType.ToDateTime( ds.Tables[0].Rows[0]["CreateTime"].ToString());
                //计算发送验证码日期和当前日期差值
                TimeSpan ts = DateTime.Now - dtime;
                if (ts.TotalSeconds > 180)
                {
                    MsgBox.ShowOK("验证码过期.请重新获取!");
                    return;
                }
                string vCode = ds.Tables[0].Rows[0]["TransferCode"].ToString();
                if (vCode != randomcode.Text.Trim())
                {
                    MsgBox.ShowOK("验证码不正确,请重新输入!");
                    return;
                }
                else
                {
                    this.timer1.Stop();
                    this.Close();
                    isprint=true;
                   
                  
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Save();

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int str = ConvertType.ToInt32(ts.Seconds.ToString());
            int str1 = ConvertType.ToInt32(ts.Minutes.ToString());
            if (str1 > 0&&str1==2)
            {
                str +=120;
            }
            else   if (str1 > 0&&str1==1)
            {
                str +=60;
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

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Close();
        }
    }
}
