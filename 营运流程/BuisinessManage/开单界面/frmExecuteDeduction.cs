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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmExecuteDeduction : BaseForm
    {
        public frmExecuteDeduction()
        {
            InitializeComponent();
        }
        public string billno;
        public decimal amount = 0;
        public string feeWeight;
        public string feeVolume;
        public int num = 0;
        public string middleSite;
        string randCode;

        DataTable dt = new DataTable();
        TimeSpan ts = new TimeSpan();

        private void frmExecuteDeduction_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            BillnoStr.Text = billno;
            AmountMoney.Text = amount.ToString();
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ByCompanyID_488", list);
             dt =SqlHelper.GetDataTable(sps);
            if (dt == null || dt.Rows.Count == 0) return;
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    WebName.Properties.Items.Add(dt.Rows[i]["WebName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void WebName_SelectedValueChanged(object sender, EventArgs e)
        {
            string webname =WebName.Text.Trim();
            Principal.Properties.Items.Clear();
            PrincipalPhone.Properties.Items.Clear();
            DataRow[] dr = dt.Select("WebName like '" + webname + "'");
          
            for (int i = 0; i < dr.Length;i++ )
            {
                string a = dr[i]["WebMan"].ToString();
                Principal.Properties.Items.Add(dr[i]["WebMan"]);
                PrincipalPhone.Properties.Items.Add(dr[i]["WebServiceTel"]);
            }
            Principal.SelectedIndex = 0;
            PrincipalPhone.SelectedIndex = 0;
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            ts = new TimeSpan(0, 1, 59);
            string webname = WebName.Text;
            string billno = BillnoStr.Text;
            string strMsg;
            string content;
            string mb;
            Decimal total_acc = 0;
            Decimal AccountBalance = 0;
            try
            {
                total_acc = CommonClass.GetTotalMoney(webname);
                AccountBalance = CommonClass.GetWebAcc_ByWebName(webname);
                if ((AccountBalance - total_acc) < 2000)
                {
                    MsgBox.ShowOK("账户余额不足2000元，请充值");
                    return;
                }
                //生成验证码
                Random rd = new Random();
                randCode = rd.Next(100000, 999999).ToString();
                List<SqlPara> list_code = new List<SqlPara>();
                list_code.Add(new SqlPara("TransferCode", randCode));
                list_code.Add(new SqlPara("BillNos", billno));
                list_code.Add(new SqlPara("Remark", "共享消费执行扣款验证码"));
                list_code.Add(new SqlPara("CodeType", 0));
                SqlParasEntity sps_code = new SqlParasEntity(OperType.Execute, "USP_ADD_TransferCode", list_code);

                mb = sms.CheckMb("",PrincipalPhone.Text.Trim());
                Dictionary<string, string> hashMap = new Dictionary<string, string>();
                hashMap.Add("a", middleSite.ToString());//shipper
                hashMap.Add("b", billno.ToString());
                hashMap.Add("c", num.ToString());
                hashMap.Add("d", feeWeight.ToString());
                hashMap.Add("e", feeVolume.ToString());
                hashMap.Add("f", AmountMoney.Text.ToString());
                hashMap.Add("g", randCode);
                hashMap.Add("templateNumber", "31");
                hashMap.Add("phone", mb);
                hashMap.Add("signName", "中强物流"); //lms-6669
                string json = JsonConvert.SerializeObject(hashMap);
                sms.LimitSMS("共享消费执行扣款验证码");

                if (SqlHelper.ExecteNonQuery(sps_code) > 0)
                {
                    //定义短信格式
                    strMsg = "您发往" + middleSite + "的货物，单号：" + billno + "，" + num.ToString() + "件，" + feeWeight.ToString() + "公斤，" + feeVolume.ToString() + "立方，扣费金额：" + AmountMoney.Text + "元，验证码：" + randCode + "，请及时验证！";
                    timer1.Enabled = true;
                    timer1.Start();
                    simpleButton1.Enabled = false;
                  
                    //发送短信
                    if (sms.sengsmsALi(json, mb, strMsg))
                    {
                        //保存短信
                        sms.SaveSMSS("11", "短信中心发送", PrincipalPhone.Text.Trim(), strMsg, CommonClass.gcdate, BillnoStr.Text.ToString(), "共享消费执行扣款验证码");
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

        private void timer1_Tick(object sender, EventArgs e)
        {
          int str =ConvertType.ToInt32(ts.Seconds.ToString());
          int str1 =ConvertType.ToInt32( ts.Minutes.ToString());
            if (str1>0)
            {
                str += 60;
            }
            Countdown.Text="验证码剩余时间:"+str.ToString()+"秒";
            ts=ts.Subtract(new TimeSpan(0,0,1));
            if (ts.TotalSeconds<0.0)
            {
                timer1.Enabled=false;
                simpleButton1.Text = "重新发送";
                simpleButton1.Enabled = true;
                MsgBox.ShowOK("验证码超时!");
                return ;
            }
            
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (string.IsNullOrEmpty(VerificationCode.Text.Trim()))
            {
                MsgBox.ShowOK("验证码不能为空");
                return;
            }
            if (string.IsNullOrEmpty(WebName.Text.Trim()))
            {
                MsgBox.ShowOK("扣费网点不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Principal.Text.Trim()))
            {
                MsgBox.ShowOK("网点负责人不能为空");
                return;
            }
            if (string.IsNullOrEmpty(PrincipalPhone.Text.Trim()))
            {
                MsgBox.ShowOK("负责人电话不能为空");
                return;
            }
            if (VerificationCode.Text.Trim().Length>6)
            {
                MsgBox.ShowOK("验证码不正确");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billno));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basTransferCode_BiLLNO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0) return  ;
                DateTime dtime =ConvertType.ToDateTime( ds.Tables[0].Rows[0]["CreateTime"].ToString());
                //计算发送验证码日期和当前日期差值
                TimeSpan ts = DateTime.Now - dtime;
                if (ts.TotalSeconds > 120)
                {
                    MsgBox.ShowOK("验证码过期.请重新获取!");
                    return;
                }
                string vCode = ds.Tables[0].Rows[0]["TransferCode"].ToString();
                if (vCode != VerificationCode.Text.Trim())
                {
                    MsgBox.ShowOK("验证码不正确,请重新输入!");
                    return;
                }
                List<SqlPara> lst = new List<SqlPara>();
                lst.Add(new SqlPara("BillNo", billno));
                lst.Add(new SqlPara("WebName", WebName.Text.Trim()));
                lst.Add(new SqlPara("Principal", Principal.Text.Trim()));
                lst.Add(new SqlPara("PrincipalPhone", PrincipalPhone.Text.Trim()));
                lst.Add(new SqlPara("Amount", amount));
                SqlParasEntity spa = new SqlParasEntity(OperType.Execute, "USP_ADD_SharedConsumption", lst);
                if (SqlHelper.ExecteNonQuery(spa) > 0)
                {
                    MsgBox.ShowOK("扣款成功!");
                    this.timer1.Stop();
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}