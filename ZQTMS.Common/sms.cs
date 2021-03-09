using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Text.RegularExpressions;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Collections.Generic;
using ThisisLanQiaoSoft;
using ZQTMS.Tool;
using Newtonsoft.Json;
//20101216
namespace ZQTMS.Common
{
    public class sms
    {
        public sms()
        {

        }

        static string hyh = "【中强物流】";
        static string smsAisleLQ = "";

        /// <summary>
        /// 要发送短信的项目环节
        /// <para>短信环节都要从这里读取</para>
        /// <para>好处在于：入口统一，以免遗漏</para>
        /// </summary>
        public enum SmsXM
        {
            开单,
            配载完成,
            在途跟踪,
            中转完成,
            中转跟踪,
            到货通知自提,
            到货通知送货,
            到货等通知放货,
            提货签收,
            送货完成,
            送货签收,

            回单寄出,
            回单返回,

            货款回收,
            货款到账,
            货款发放
        }

        /// <summary>
        /// 短信接收对象
        /// </summary>
        public enum SendTo
        {
            发货人,
            收货人
        }

        public static bool checksmsid()
        {
            if (CommonClass.UserInfo.UserName == "")
            {
                XtraMessageBox.Show("没有短信企业ID,无法发送短信!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (CommonClass.smsid == "")
            {
                XtraMessageBox.Show("没有短信账号,无法发送短信!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (CommonClass.smspassword == "")
            {
                XtraMessageBox.Show("没有短信密码,无法发送短信!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        public static bool smschecked(GridView gridView1)
        {
            int a = 0;
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (Convert.ToInt32(gridView1.GetRowCellValue(i, "ischecked")) == 1)
                {
                    a++;
                    break;
                }
            }
            if (a == 0)
            {
                XtraMessageBox.Show("发送失败!\r\n\r\n原因：没有勾选要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        public static bool smschecked(DataTable dt)
        {
            if (dt.Select("ischecked=1").Length == 0)
            {
                XtraMessageBox.Show("短信发送失败!\r\n\r\n原因：没有勾选要发送短信的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 短信限制
        /// </summary>
        public static void LimitSMS_100()
        {
            List<SqlPara> list_code = new List<SqlPara>();
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_LIMIT_SMS_100", list_code));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                MsgBox.ShowOK("短信条数不足100，请及时充值");
            }
        }


        public static void LimitSMS()
        {
            List<SqlPara> list_code = new List<SqlPara>();
            SqlParasEntity sps_code = new SqlParasEntity(OperType.Query, "QSP_LIMIT_SMS", list_code);
            SqlHelper.ExecteNonQuery(sps_code);
        }

        //阿里云发短信方法
        public static bool sengsmsALi(String json, String mb, String content)
        {
            try
            {
                //http://localhost:8080/DeKunInterface/ZQTMS/sms
                string resultStr = HttpHelper.HttpPostJava(json, "http://ZQTMS.dekuncn.com:8075/sms/sendSms");//http://localhost:8000/sms/sendSms
                //string resultStr = HttpHelper.HttpPostJava(json, "http://localhost:8075/sms/sendSms");//http://localhost:8000/sms/sendSms
                Root root = JsonConvert.DeserializeObject<Root>(resultStr);
                if (root.msg == "ok")
                {
                    smsAisleLQ = "阿里云";
                    return true;
                }
                else
                {
                    if (smslanqiao(mb, content))
                    {
                        smsAisleLQ = "蓝桥";
                        return true;
                    }
                    else if (sendsmsNew(mb, content))
                    {
                        smsAisleLQ = "虞姿";
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (smslanqiao(mb, content))
                {
                    smsAisleLQ = "蓝桥";
                    return true;
                }
            }
            return false;
        }
        public static bool sendsms(string mb, string content) //目前用的是北京那家
        { // 注意：换短信的时候要修改这个值：int per = 100;//一次提交的手机号码数量  翼锋100          
            try
            {
                LimitSMS();
                LimitSMS_100();
                if (!checksmsid())
                    return false;

                string result = SMS.Sendsms(CommonClass.smsCompanyName, CommonClass.UserInfo.UserId.ToString(), CommonClass.UserInfo.UserName, CommonClass.smsuserid, CommonClass.smsid, CommonClass.smspassword, mb, content);
                string[] Arr = result.Split('|');
                if (Arr[0].Trim() == "1")
                {
                    return true;
                }
                else
                {
                    MsgBox.ShowOK(Arr[1]); //Arr[0]=0
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 短信限制
        /// </summary>
        public static void LimitSMS(string msg)
        {
            List<SqlPara> list_code = new List<SqlPara>();
            list_code.Add(new SqlPara("msg", msg));
            SqlParasEntity sps_code = new SqlParasEntity(OperType.Query, "QSP_LIMIT_SMS", list_code);
            SqlHelper.ExecteNonQuery(sps_code);
        }
        //送货发送短信 给发货人
        /// <summary>
        /// 新增发送短信
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool sendsmsNew(string mb, string content) //zb20191224
        {
            try
            {
                SmsResult<List<SendSmsModel>> result = SendSms("200015", "123456", mb, hyh + content);
                if (result.status == "0")
                {
                  MsgBox.ShowOK("发送成功");
                    return true;
                }
                else
                {
                    return smslanqiao(mb, content);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return smslanqiao(mb, content);
            }
        }
        /// <summary>
        /// 蓝桥短信
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool smslanqiao(string mb, string content)//目前用的是北京那家
        {
            //注意：换短信的时候要修改这个值：int per = 100;//一次提交的手机号码数量  翼锋100          
            try
            {
                if (!checksmsid())
                    return false;

                string result = SMS.Sendsms(CommonClass.smsCompanyName, CommonClass.UserInfo.UserId.ToString(), CommonClass.UserInfo.UserName, CommonClass.smsuserid, CommonClass.smsid, CommonClass.smspassword, mb, content);
                string[] Arr = result.Split('|');
                if (Arr[0].Trim() == "1")
                {
                    smsAisleLQ = "蓝桥";
                    sms.SaveSMSS("0", CommonClass.UserInfo.UserName, mb, content, CommonClass.gcdate, "0", "其他");
                    return true;
                }
                else
                {
                    MsgBox.ShowOK(Arr[1]); //Arr[0]=0
                    return false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="account">用户账号</param>
        /// <param name="password">密码</param>
        /// <param name="mobile">全部被叫号码,发信发送的目的号码.多个号码之间用半角逗号隔开,最多500个号码</param>
        /// <param name="content">发送内容</param>
        /// <returns>返回请求结果</returns>
        public static SmsResult<List<SendSmsModel>> SendSms(string account, string password, string mobile, string content)
        {
            string data = "action=send&account=" + account + "&password=" + password +
                "&mobile=" + mobile + "&content=" + content + "&extno=10690225&rt=json";//10690212
            SmsResult<List<SendSmsModel>> result = new SmsResult<List<SendSmsModel>>();
            try
            {
                string resultStr = HttpGet(data);
                result = JsonConvert.DeserializeObject<SmsResult<List<SendSmsModel>>>(resultStr);
            }
            catch (Exception ex)
            {
                result.status = "99";
                result.msg = "请求失败：" + ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        private static string HttpGet(string data)
        {
            string result = "";
            try
            {
                WebResponse wr = null;
                HttpWebRequest req = null;
                req = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", "http://47.99.144.3:7862/sms", data));
                req.Proxy = null;
                req.Method = "get";
                req.Timeout = 600000;

                wr = req.GetResponse();
                using (Stream ReceiveStream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(ReceiveStream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    ReceiveStream.Close();
                    ReceiveStream.Dispose();
                }
                wr.Close();
            }
            catch (WebException ex)
            {
                result = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// hj20180420 
        /// </summary>
        /// <param name="xm"></param>
        /// <param name="sendto"></param>
        /// <param name="content"></param>
        /// <param name="showtip"></param>
        /// <returns></returns>
        public static bool CheckSmsContent1(SmsXM xm, SendTo sendto, ref string content, bool showtip,string companyid1)//showtip：是否弹出提示框
        {
            try
            {
                string field = "forShipper";
                if (sendto == SendTo.收货人)
                    field = "forConsignee";

                int flag = 0;
                //if (CommonClass.dsmsg.Tables.Count == 0 || CommonClass.dsmsg.Tables[0].Rows.Count == 0)
                //hj20180420 106,122公司都有到武汉落货，导致了122公司也能发106公司的短信，不使用缓存表的数据
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MSG_1", new List<SqlPara>() { new SqlPara("companyid1", companyid1) }));
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    flag = 1;
                // DataRow[] dr = CommonClass.dsmsg.Tables[0].Select(string.Format("project='{0}' and {1}<>''", xm, field));
                DataRow[] dr = ds.Tables[0].Select(string.Format("project='{0}' and {1}<>''", xm, field));
                if (dr.Length == 0)
                    flag = 1;
                if (flag == 1)
                {
                    if (showtip)
                    {
                        MsgBox.ShowOK(string.Format("发送失败!{0}环节没有定义给{1}的短信内容!\r\n\r\n如果您确认已经定义了,请重新启动登录软件!", xm, sendto));
                    }
                    content = "";
                    return false;
                }

                content = dr[0][field] == DBNull.Value ? "" : dr[0][field].ToString();
                return true;
            }
            catch (Exception ex)
            {
                if (showtip)
                {
                    MsgBox.ShowOK(ex.Message);
                }
                content = "";
                return false;
            }
        }


        public static bool CheckSmsContent(SmsXM xm, SendTo sendto, ref string content, bool showtip)//showtip：是否弹出提示框
        {
            try
            {
                string field = "forShipper";
                if (sendto == SendTo.收货人)
                    field = "forConsignee";

                int flag = 0;
                if (CommonClass.dsmsg.Tables.Count == 0 || CommonClass.dsmsg.Tables[0].Rows.Count == 0)
                //DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MSG"));
                //if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    flag = 1;
                 DataRow[] dr = CommonClass.dsmsg.Tables[0].Select(string.Format("project='{0}' and {1}<>''", xm, field));
                //DataRow[] dr = ds.Tables[0].Select(string.Format("project='{0}' and {1}<>''", xm, field));
                if (dr.Length == 0)
                    flag = 1;
                if (flag == 1)
                {
                    if (showtip)
                    {
                        MsgBox.ShowOK(string.Format("发送失败!{0}环节没有定义给{1}的短信内容!\r\n\r\n如果您确认已经定义了,请重新启动登录软件!", xm, sendto));
                    }
                    content = "";
                    return false;
                }

                content = dr[0][field] == DBNull.Value ? "" : dr[0][field].ToString();
                return true;
            }
            catch (Exception ex)
            {
                if (showtip)
                {
                    MsgBox.ShowOK(ex.Message);
                }
                content = "";
                return false;
            }
        }

        /// <summary>
        /// 检测电话号码是否符合规则
        /// </summary>
        /// <param name="mb">手机</param>
        /// <param name="tel">电话</param>
        /// <returns></returns>
        public static string CheckMb(string mb, string tel)
        {
            string strMatch = "^1[3|4|5|8][0-9]\\d{8}$";
            if (Regex.IsMatch(mb, strMatch))
            {
                return mb;
            }
            else
            {
                if (Regex.IsMatch(tel, strMatch))
                {
                    return tel;
                }
                return "";
            }
        }

        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="smsstate">发送短信的环节</param>
        /// <param name="fshipper">接收人</param>
        /// <param name="telephone">电话</param>
        /// <param name="content">短信内容</param>
        /// <param name="senddate">发送日期</param>
        /// <param name="unit">运单号</param>
        public static bool SaveSMSS(string smsstate, string fshipper, string telephone, string content, DateTime senddate, string billno)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("smsstate", smsstate));
                list.Add(new SqlPara("fshipper", fshipper));
                list.Add(new SqlPara("telephone", telephone));
                list.Add(new SqlPara("content", content));

                list.Add(new SqlPara("senddate", senddate));
                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("billno", billno));
                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));

                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("mbcount", telephone.Split(',').Length));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.WebName));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SMSS");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }
        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="smsstate">发送短信的环节</param>
        /// <param name="fshipper">接收人</param>
        /// <param name="telephone">电话</param>
        /// <param name="content">短信内容</param>
        /// <param name="senddate">发送日期</param>
        /// <param name="unit">运单号</param>
        public static bool SaveSMSS(string smsstate, string fshipper, string telephone, string content, DateTime senddate, string billno, string smsNode)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("smsstate", smsstate));
                list.Add(new SqlPara("fshipper", fshipper));
                list.Add(new SqlPara("telephone", telephone));
                list.Add(new SqlPara("content", content));

                list.Add(new SqlPara("senddate", senddate));
                list.Add(new SqlPara("billno", billno));
                list.Add(new SqlPara("mbcount", telephone.Split(',').Length));
                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));

                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));

                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));

                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("smsNode", smsNode));
                if (smsAisleLQ == "")
                {
                    list.Add(new SqlPara("smsAisle", "阿里云"));
                }
                else
                {
                    list.Add(new SqlPara("smsAisle", smsAisleLQ));
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SMSS_NEW");
                sps.ParaList = list;
                smsAisleLQ = "";
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                   // MsgBox.ShowOK();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }




        public static bool SaveSMSS2(string smsstate, string fshipper, string telephone, string content, DateTime senddate, string billno)
        {
            //MsgBox.ShowOK(string.Format("对象={0}\r\n电话={1}\r\n内容={2}", fshipper, telephone, content));
            //try
            //{
            //    SqlCommand cmd = new SqlCommand("USP_ADD_SMSS", Conn);
            //    cmd.CommandType = CommandType.StoredProcedure;

            //    cmd.Parameters.Add(new SqlParameter("@smsstate", SqlDbType.Int));
            //    cmd.Parameters.Add(new SqlParameter("@fshipper", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@telephone", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@senddate", SqlDbType.DateTime));
            //    cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@unit", SqlDbType.Int));

            //    cmd.Parameters.Add(new SqlParameter("@bsite", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@webid", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@companyid", SqlDbType.VarChar));
            //    cmd.Parameters.Add(new SqlParameter("@mbcount", SqlDbType.Int));

            //    cmd.Parameters["@smsstate"].Value = smsstate;
            //    cmd.Parameters["@fshipper"].Value = fshipper;
            //    cmd.Parameters["@telephone"].Value = telephone;
            //    cmd.Parameters["@content"].Value = content;
            //    cmd.Parameters["@senddate"].Value = senddate;
            //    cmd.Parameters["@createby"].Value = CommonClass.UserInfo.UserName;
            //    cmd.Parameters["@unit"].Value = unit;

            //    cmd.Parameters["@bsite"].Value = CommonClass.b;
            //    cmd.Parameters["@webid"].Value = CommonClass.UserInfo.WebName;
            //    cmd.Parameters["@companyid"].Value = CommonClass.UserInfo.companyid;
            //    cmd.Parameters["@mbcount"].Value = telephone.Split(',').Length;

            //    Conn.Open();
            //    cmd.ExecuteNonQuery();
            //    Conn.Close();

            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("保存短信失败：\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}


            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("smsstate", smsstate));
                list.Add(new SqlPara("fshipper", fshipper));
                list.Add(new SqlPara("telephone", telephone));
                list.Add(new SqlPara("content", content));

                list.Add(new SqlPara("senddate", senddate));
                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("billno", billno));
                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));

                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("mbcount", telephone.Split(',').Length));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.WebName));



                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SMSS");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }


        //开单发送短信  定义了发货人短信，就给发货人。定义了收货人短信，就给收货人
        public static void kdsendsms(int unit, string billno, string shipper, string consignee, DateTime billdate, string bsite, string middlesite, string esite, int qty, string product, string consigneemb, string consigneetel, string webid, decimal accarrived, decimal accdaishou, string shippertel, string shippermb)
        {
            if (!checksmsid())
                return;

            #region 取短信格式
            string temp1 = "", temp2 = ""; //temp1发货人格式   temp1收货人格式 

            SmsXM xm = SmsXM.开单;
            CheckSmsContent(xm, SendTo.发货人, ref temp1, false);
            CheckSmsContent(xm, SendTo.收货人, ref temp2, false);
            if (temp1 == "" && temp2 == "")
            {
                MsgBox.ShowOK(string.Format("发送失败!{0}环节没有定义短信内容!\r\n\r\n如果您确认已经定义了,请重新启动登录软件!", xm));
                return;
            }


            string mb1 = "", mb2 = "";
            if (temp1 != "")
            {
                mb1 = CheckMb(shippermb, shippertel);
            }
            if (temp2 != "")
            {
                mb2 = CheckMb(consigneemb, consigneetel);
            }
            if (mb1 == "" && mb2 == "")
            {
                MsgBox.ShowOK("手机号码不符合规则，短信未发送!");
                return;
            }

            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.开单))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion
            string content1 = temp1, content2 = temp2;

            DateTime datenow = CommonClass.ServerDate;

            #region 发货人
            content1 = content1.Replace("[a]", shipper);
            content1 = content1.Replace("[b]", consignee);
            content1 = content1.Replace("[c]", bsite);
            content1 = content1.Replace("[d]", esite);
            content1 = content1.Replace("[l]", middlesite);
            content1 = content1.Replace("[e]", unit.ToString());
            content1 = content1.Replace("[f]", billno);
            content1 = content1.Replace("[g]", product);
            content1 = content1.Replace("[h]", qty.ToString());
            content1 = content1.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
            content1 = content1.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
            content1 = content1.Replace("[we]", webid);//开单网点
            content1 = content1.Replace("[T1]", tel); //开单查询电话

            content1 = content1.Replace("[i]", CommonClass.smsCompanyName);

            content1 = content1.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
            content1 = content1.Replace("[yy]", billdate.Year.ToString());
            content1 = content1.Replace("[nn]", billdate.Month.ToString());
            content1 = content1.Replace("[dd]", billdate.Day.ToString());
            content1 = content1.Replace("[hh]", billdate.Hour.ToString());
            content1 = content1.Replace("[mm]", billdate.Minute.ToString());

            content1 = content1.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
            content1 = content1.Replace("[YY]", datenow.Year.ToString());
            content1 = content1.Replace("[NN]", datenow.Month.ToString());
            content1 = content1.Replace("[DD]", datenow.Day.ToString());
            content1 = content1.Replace("[HH]", datenow.Hour.ToString());
            content1 = content1.Replace("[MM]", datenow.Minute.ToString());

            content1 = content1.Replace("[gs]", gs);
            #endregion


            #region 收货人
            content2 = content2.Replace("[a]", shipper);
            content2 = content2.Replace("[b]", consignee);
            content2 = content2.Replace("[c]", bsite);
            content2 = content2.Replace("[d]", esite);
            content2 = content2.Replace("[l]", middlesite);
            content2 = content2.Replace("[e]", unit.ToString());
            content2 = content2.Replace("[f]", billno);
            content2 = content2.Replace("[g]", product);
            content2 = content2.Replace("[h]", qty.ToString());
            content2 = content2.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
            content2 = content2.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
            content2 = content2.Replace("[we]", webid);//开单网点
            content2 = content2.Replace("[T1]", tel); //开单查询电话

            content2 = content2.Replace("[i]", CommonClass.smsCompanyName);

            content2 = content2.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
            content2 = content2.Replace("[yy]", billdate.Year.ToString());
            content2 = content2.Replace("[nn]", billdate.Month.ToString());
            content2 = content2.Replace("[dd]", billdate.Day.ToString());
            content2 = content2.Replace("[hh]", billdate.Hour.ToString());
            content2 = content2.Replace("[mm]", billdate.Minute.ToString());

            content2 = content2.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
            content2 = content2.Replace("[YY]", datenow.Year.ToString());
            content2 = content2.Replace("[NN]", datenow.Month.ToString());
            content2 = content2.Replace("[DD]", datenow.Day.ToString());
            content2 = content2.Replace("[HH]", datenow.Hour.ToString());
            content2 = content2.Replace("[MM]", datenow.Minute.ToString());

            content2 = content2.Replace("[gs]", gs);
            #endregion

            if (mb1 != "")
            {
                if (!SaveSMSS("0", shipper, mb1, content1, billdate, billno)) return;
                if (!sendsms(mb1, content1)) return; ;
            }
            if (mb2 != "")
            {
                if (!SaveSMSS("0", consignee, mb2, content2, billdate, billno)) return;
                if (!sendsms(mb2, content2)) return;
            }
            XtraMessageBox.Show("通知短信发送完毕!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //中转发送短信
        public static void outsendsms(SendTo sendto, int isfromsf, int unit, string billno, string shipper, string consignee, DateTime billdate, string bsite, string esite, int qty, string product, string shippermb, string shippertel, DateTime outdate, string outcygs, string outcygstel, string consigneemb, string consigneetel, string outbillno, string outremotetel, int gettype, decimal accarrived, decimal accdaishou)
        {
            if (!checksmsid())
                return;

            string acceptmb = shippermb, accepttel = shippertel;

            SmsXM xm = SmsXM.中转完成;
            if (sendto == SendTo.收货人)
            {
                acceptmb = consigneemb;
                accepttel = consigneetel;
            }

            string mb = CheckMb(acceptmb, accepttel);
            if (mb == "")
            {
                MsgBox.ShowOK(string.Format("{0}环节{1}手机号码不符合规则，短信未发送!", xm, sendto));
                return;
            }

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            string content = temp;
            string acc = accarrived + accdaishou > 0 ? "及运费" + Math.Round(accarrived + accdaishou, 2).ToString() + "元" : "";

            content = content.Replace("[a]", shipper);
            content = content.Replace("[b]", consignee);
            content = content.Replace("[c]", bsite);
            content = content.Replace("[d]", esite);
            content = content.Replace("[e]", unit.ToString());
            content = content.Replace("[f]", billno);
            content = content.Replace("[g]", product);
            content = content.Replace("[h]", qty.ToString());

            content = content.Replace("[i]", CommonClass.smsCompanyName);

            content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
            content = content.Replace("[yy]", billdate.Year.ToString());
            content = content.Replace("[nn]", billdate.Month.ToString());
            content = content.Replace("[dd]", billdate.Day.ToString());
            content = content.Replace("[hh]", billdate.Hour.ToString());
            content = content.Replace("[mm]", billdate.Minute.ToString());

            content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
            content = content.Replace("[YY]", datenow.Year.ToString());
            content = content.Replace("[NN]", datenow.Month.ToString());
            content = content.Replace("[DD]", datenow.Day.ToString());
            content = content.Replace("[HH]", datenow.Hour.ToString());
            content = content.Replace("[MM]", datenow.Minute.ToString());

            content = content.Replace("[ii]", outcygs);
            content = content.Replace("[$$$]", acc);
            content = content.Replace("[ee]", outbillno);
            content = content.Replace("[bj]", outcygstel);
            content = content.Replace("[ej]", outremotetel);

            content = content.Replace("[eh]", outdate.ToString("yyyy年MM月dd日")); //中转日期
            content = content.Replace("[ey]", outdate.Year.ToString());
            content = content.Replace("[en]", outdate.Month.ToString());
            content = content.Replace("[ed]", outdate.Day.ToString());
            content = content.Replace("[eh]", outdate.Hour.ToString());
            content = content.Replace("[em]", outdate.Minute.ToString());

            content = content.Replace("[T4]", tel); //中转查询电话

            gs = isfromsf == 1 ? hyh : gs;
            content = content.Replace("[gs]", gs);

            if (!SaveSMSS("10", (sendto == SendTo.发货人 ? shipper : consignee), mb, content, CommonClass.gcdate, billno)) return;
            if (!sendsms(mb, content)) return;
            XtraMessageBox.Show("通知短信发送完毕!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //中转发送短信 批量
        public static void outsendsms_pl(GridView gridView3, DateTime outdate, string outcygs, string outcygstel)
        {
            if (!checksmsid())
                return;
            if (gridView3.RowCount == 0)
                return;
            string unit = "", billno = "", shipper = "", consignee = "", mb = "", bsite = "", middlesite = "", esite = "", qty = "", product = "", outbillno = "", outremotetel = "", content = "";
            DateTime billdate;

            SmsXM xm = SmsXM.中转完成;
            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(xm, SendTo.发货人, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            int isfromsf = 0;

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;
                mb = CheckMb(gridView3.GetRowCellValue(i, "shippermb").ToString().Trim(), gridView3.GetRowCellValue(i, "shippertel").ToString().Trim());
                if (mb == "")
                    continue;

                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "billdate"));

                outbillno = gridView3.GetRowCellValue(i, "outbillno") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "outbillno").ToString();
                outremotetel = gridView3.GetRowCellValue(i, "outremotetel") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "outremotetel").ToString();

                isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty.ToString());

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[ii]", outcygs);
                content = content.Replace("[ee]", outbillno);
                content = content.Replace("[bj]", outcygstel);
                content = content.Replace("[ej]", outremotetel);

                content = content.Replace("[eh]", outdate.ToString("yyyy年MM月dd日")); //中转日期
                content = content.Replace("[ey]", outdate.Year.ToString());
                content = content.Replace("[en]", outdate.Month.ToString());
                content = content.Replace("[ed]", outdate.Day.ToString());
                content = content.Replace("[eh]", outdate.Hour.ToString());
                content = content.Replace("[em]", outdate.Minute.ToString());

                content = content.Replace("[T4]", tel); //中转查询电话

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("10", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                k++;
            }

            XtraMessageBox.Show("通知短信发送完毕,本次发送了" + k.ToString() + "条!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //发车 给发货人
        public static void fcdsendsms(GridView gridView3, Form wmain, string range, DateTime senddate, DateTime willbedate) //willbedate：预计到达
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", product = "";
            string consignee = "", shipper = "", acctype = "", webid = "";
            decimal accdaishou = 0, accarrived = 0;


            #region 取短信格式
            string temp = "";
            SmsXM xm = SmsXM.配载完成;
            SendTo sendto = SendTo.发货人;
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked == 1)
                {
                    mb = CheckMb(gridView3.GetRowCellValue(i, "ConsignorCellPhone").ToString().Trim(), gridView3.GetRowCellValue(i, "ConsignorPhone").ToString().Trim());
                    if (mb == "")
                    {
                        continue;
                    }

                    //qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                    //bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                    // middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                    //billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                    //okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                    // consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                    //shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                    //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                    //product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                    //DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                    //bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                    //acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode"));
                    //webid = gridView3.GetRowCellValue(i, "webid") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "webid").ToString();
                    // accdaishou = gridView3.GetRowCellValue(i, "CollectionPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                    //   accarrived = gridView3.GetRowCellValue(i, "FetchPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));
                    if (CommonClass.dsWeb.Tables.Count > 0 && CommonClass.dsWeb.Tables[0].Rows.Count > 0)
                    {
                        tel = CommonClass.dsWeb.Tables[0].Select(string.Format("WebName='{0}'", ConvertType.ToString(gridView3.GetRowCellValue(i, "BegWeb"))))[0]["WebServiceTel"].ToString();
                        //gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
                    }

                    qty = ConvertType.ToString(gridView3.GetRowCellValue(i, "WebPCS"));
                    bsite = ConvertType.ToString(gridView3.GetRowCellValue(i, "StartSite"));
                    esite = ConvertType.ToString(gridView3.GetRowCellValue(i, "DestinationSite"));
                    middlesite = ConvertType.ToString(gridView3.GetRowCellValue(i, "TransferSite"));
                    billno = ConvertType.ToString(gridView3.GetRowCellValue(i, "BillNo"));
                    okprocess = ConvertType.ToString(gridView3.GetRowCellValue(i, "TransferMode"));
                    consignee = ConvertType.ToString(gridView3.GetRowCellValue(i, "ConsigneeName"));
                    shipper = ConvertType.ToString(gridView3.GetRowCellValue(i, "ConsignorName"));
                    //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                    webid = gridView3.GetRowCellValue(i, "BegWeb") == DBNull.Value ? "" : ConvertType.ToString(gridView3.GetRowCellValue(i, "BegWeb"));
                    product = ConvertType.ToString(gridView3.GetRowCellValue(i, "Varieties"));
                    DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                    acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode")); ;


                    //address = gridView3.GetRowCellValue(i, "ReceivAddress") == DBNull.Value || gridView3.GetRowCellValue(i, "ReceivAddress") == null ? "" : gridView3.GetRowCellValue(i, "ReceivAddress").ToString();

                    accdaishou = gridView3.GetRowCellValue(i, "CollectionPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                    accarrived = gridView3.GetRowCellValue(i, "FetchPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));



                    //isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                    content = temp;

                    content = content.Replace("[a]", shipper);
                    content = content.Replace("[b]", consignee);
                    content = content.Replace("[c]", bsite);
                    content = content.Replace("[d]", esite);
                    content = content.Replace("[l]", middlesite);
                    content = content.Replace("[e]", unit.ToString());
                    content = content.Replace("[f]", billno);
                    content = content.Replace("[g]", product);
                    content = content.Replace("[h]", qty);
                    content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                    content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
                    content = content.Replace("[we]", webid);//开单网点

                    content = content.Replace("[i]", CommonClass.smsCompanyName);

                    content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[yy]", billdate.Year.ToString());
                    content = content.Replace("[nn]", billdate.Month.ToString());
                    content = content.Replace("[dd]", billdate.Day.ToString());
                    content = content.Replace("[hh]", billdate.Hour.ToString());
                    content = content.Replace("[mm]", billdate.Minute.ToString());

                    content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[YY]", datenow.Year.ToString());
                    content = content.Replace("[NN]", datenow.Month.ToString());
                    content = content.Replace("[DD]", datenow.Day.ToString());
                    content = content.Replace("[HH]", datenow.Hour.ToString());
                    content = content.Replace("[MM]", datenow.Minute.ToString());

                    content = content.Replace("[po]", senddate.ToString("yyyy年MM月dd日")); //配载日期
                    content = content.Replace("[py]", senddate.Year.ToString());
                    content = content.Replace("[pn]", senddate.Month.ToString());
                    content = content.Replace("[pd]", senddate.Day.ToString());
                    content = content.Replace("[ph]", senddate.Hour.ToString());
                    content = content.Replace("[pm]", senddate.Minute.ToString());

                    content = content.Replace("[pt]", acctype == "货到前付" ? "为了不延误货物交接，请及时付款，" : ""); //货到前付提示
                    content = content.Replace("[T2]", tel); //配载完成查询电话

                    content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                    if (!SaveSMSS("1", shipper, mb, content, CommonClass.gcdate, billno)) return;
                    if (!sendsms(mb, content)) return;

                    gridView3.FocusedRowHandle = i;
                    gridView3.SetRowCellValue(i, "ischecked", 2);
                    k++;
                }
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //发车 给收货人
        public static void fcdsendsms_consignee(GridView gridView3, Form wmain, string range, DateTime senddate, string vehicleno) //vehicleno：车号
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", product = "", webtel = "", webaddress = "";
            string consignee = "", shipper = "", acctype = "", webid = "";
            decimal accdaishou = 0, accarrived = 0;

            #region 取短信格式
            string temp = "";
            SmsXM xm = SmsXM.配载完成;
            SendTo sendto = SendTo.收货人;
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            #endregion


            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked == 1)
                {
                    mb = CheckMb(gridView3.GetRowCellValue(i, "ConsigneeCellPhone").ToString().Trim(), gridView3.GetRowCellValue(i, "ConsigneePhone").ToString().Trim());
                    if (mb == "")
                    {
                        continue;
                    }
                    if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
                    {
                        tel = CommonClass.dsWeb.Tables[0].Select(string.Format("WebName='{0}'", ConvertType.ToString(gridView3.GetRowCellValue(i, "BegWeb"))))[0]["WebServiceTel"].ToString();
                        gs = "";
                    }

                    qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                    bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                    //CommonClass.GetSiteCode(esite, ref webtel, ref webaddress);
                    middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                    billno = gridView3.GetRowCellValue(i, "BillNO").ToString();
                    okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                    consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                    shipper = gridView3.GetRowCellValue(i, "ConsignorName").ToString();
                    //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                    product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                    DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                    bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();

                    acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode")); ;
                    webid = gridView3.GetRowCellValue(i, "BegWeb") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "BegWeb").ToString();
                    accdaishou = gridView3.GetRowCellValue(i, "CollectionPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                    accarrived = gridView3.GetRowCellValue(i, "FetchPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));

                    //isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                    content = temp;

                    content = content.Replace("[a]", shipper);
                    content = content.Replace("[b]", consignee);
                    content = content.Replace("[c]", bsite);
                    content = content.Replace("[d]", esite);
                    content = content.Replace("[l]", middlesite);
                    content = content.Replace("[e]", unit.ToString());
                    content = content.Replace("[f]", billno);
                    content = content.Replace("[g]", product);
                    content = content.Replace("[h]", qty);
                    content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                    content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
                    content = content.Replace("[we]", webid);//开单网点

                    content = content.Replace("[i]", CommonClass.smsCompanyName);

                    content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[yy]", billdate.Year.ToString());
                    content = content.Replace("[nn]", billdate.Month.ToString());
                    content = content.Replace("[dd]", billdate.Day.ToString());
                    content = content.Replace("[hh]", billdate.Hour.ToString());
                    content = content.Replace("[mm]", billdate.Minute.ToString());

                    content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[YY]", datenow.Year.ToString());
                    content = content.Replace("[NN]", datenow.Month.ToString());
                    content = content.Replace("[DD]", datenow.Day.ToString());
                    content = content.Replace("[HH]", datenow.Hour.ToString());
                    content = content.Replace("[MM]", datenow.Minute.ToString());

                    content = content.Replace("[po]", senddate.ToString("yyyy年MM月dd日")); //配载日期
                    content = content.Replace("[py]", senddate.Year.ToString());
                    content = content.Replace("[pn]", senddate.Month.ToString());
                    content = content.Replace("[pd]", senddate.Day.ToString());
                    content = content.Replace("[ph]", senddate.Hour.ToString());
                    content = content.Replace("[pm]", senddate.Minute.ToString());

                    content = content.Replace("[pt]", acctype == "货到前付" ? "为了不延误货物交接，请及时付款，" : ""); //货到前付提示
                    content = content.Replace("[T2]", tel); //配载完成查询电话

                    content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                    if (!SaveSMSS("1", consignee, mb, content, CommonClass.gcdate, billno)) return;
                    if (!sendsms(mb, content)) return;
                    gridView3.FocusedRowHandle = i;
                    gridView3.SetRowCellValue(i, "ischecked", 2);
                    k++;
                }

            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //专线在途情况
        public static void errorsendsms(GridView gridView3, Form wmain, string range, string content1, string people)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;

            string field2 = "ConsignorCellPhone", field3 = "ConsignorPhone";
            SmsXM xm = SmsXM.在途跟踪;
            SendTo sendto = SendTo.发货人;
            if (people == "收货人")
            {
                sendto = SendTo.收货人;
                field2 = "ConsigneeCellPhone";
                field3 = "ConsigneePhone";
            }

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            string mb = "", middlesite = "", billno = "", okprocess = "", content = "";
            string qty = "", bsite = "", esite = "", product = "";
            string consignee = "", shipper = "", acctype = "", webid = "";
            decimal accdaishou = 0, accarrived = 0;

            DateTime billdate = DateTime.Now;

            int k = 0, ischecked = 0;
            string unit = "";

            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));
                if (ischecked != 1)
                    continue;

                mb = CheckMb(gridView3.GetRowCellValue(i, field2).ToString().Trim(), gridView3.GetRowCellValue(i, field3).ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

               // qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                bsite = gridView3.GetRowCellValue(i, "DestinationSite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNO").ToString();
                okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                shipper = gridView3.GetRowCellValue(i, "ConsignorPhone").ToString();
                //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode")); ;
                webid = gridView3.GetRowCellValue(i, "BegWeb") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "BegWeb").ToString();
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "WebDate"));
                accdaishou = gridView3.GetRowCellValue(i, "CollectionPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                accarrived = gridView3.GetRowCellValue(i, "FetchPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));

                //isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[l]", middlesite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);
                content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
                content = content.Replace("[we]", webid);//开单网点

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[T3]", tel); //专线在途跟踪查询电话
                content = content.Replace("[GZ]", content1); //操作员填写的跟踪内容

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("11", (sendto == SendTo.发货人 ? shipper : consignee), mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;

                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //确认到货短信 给发货人
        public static void nowsendsms_to_shipper(GridView gridView3, DateTime arriveddate, string companyid1)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "", temp1 = "";
            if (!CheckSmsContent1(SmsXM.到货通知自提, SendTo.发货人, ref temp, true,companyid1))
                return;
            DataView dv = (DataView)gridView3.DataSource;
            bool isdfyifan = dv.Table.Columns.Contains("NoticeState");
            if (isdfyifan)
            {
                if (dv.Table.Select("dfyifan=1").Length > 0)
                {
                    if (!CheckSmsContent(SmsXM.到货等通知放货, SendTo.收货人, ref temp1, true))
                        return;
                }
            }

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.到货通知自提))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0;
            string unit = "";

            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                if (ConvertType.ToInt32(gridView3.GetRowCellValue(i, "ischecked")) == 0) continue;
                mb = CheckMb(GridOper.GetRowCellValueString(gridView3, i, "ConsignorCellPhone"), GridOper.GetRowCellValueString(gridView3, i, "ConsignorPhone"));
                if (mb == "")
                {
                    continue;
                }

                qty = GridOper.GetRowCellValueString(gridView3, i, "SendPCS");
                bsite = GridOper.GetRowCellValueString(gridView3, i, "StartSite");
                esite = GridOper.GetRowCellValueString(gridView3, i, "DestinationSite");
                middlesite = GridOper.GetRowCellValueString(gridView3, i, "TransferSite");
                billno = GridOper.GetRowCellValueString(gridView3, i, "BillNO");
                okprocess = GridOper.GetRowCellValueString(gridView3, i, "TransferMode");
                consignee = GridOper.GetRowCellValueString(gridView3, i, "ConsigneeName");
                shipper = GridOper.GetRowCellValueString(gridView3, i, "ConsignorName");
                product = GridOper.GetRowCellValueString(gridView3, i, "Varieties");
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                acctype = GridOper.GetRowCellValueString(gridView3, i, "PaymentMode");

                if (isdfyifan && GridOper.GetRowCellValueString(gridView3, i, "NoticeState") == "1")
                {
                    content = temp1;
                }
                else
                {
                    content = temp;
                }

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[l]", middlesite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[T6]", tel); //到货通知查询电话
                content = content.Replace("[gb]", CommonClass.UserInfo.SiteName);
                content = content.Replace("[k]", webaddress);

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("2", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;

                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //确认到货短信 给收货人
        public static void nowsendsms(GridView gridView3, Form wmain, string range,string companyid1)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webaddress = "", product = "", pickgoodsite = "";
            string consignee = "", shipper = "";
            string webPhone = "";
            string acctype = "";
            string address = "";
            decimal accdaishou = 0, accarrived = 0;

            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp1 = "", temp2 = "", temp3 = "";

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                if (GridOper.GetRowCellValueString(gridView3, i, "TransferMode") == "自提") //有自提的
                {
                    if (!CheckSmsContent1(SmsXM.到货通知自提, SendTo.收货人, ref temp1, true, companyid1))
                        return;
                    break;
                }
            }
            for (int i = 0; i < gridView3.RowCount; i++)
            {
                if (GridOper.GetRowCellValueString(gridView3, i, "TransferMode") != "自提") //有不是自提的
                {
                    if (!CheckSmsContent1(SmsXM.到货通知送货, SendTo.收货人, ref temp2, true, companyid1))
                        return;
                    break;
                }
            }

            DataView dv = (DataView)gridView3.DataSource;
            bool isdfyifan = dv.Table.Columns.Contains("NoticeState");
            if (isdfyifan)
            {
                if (dv.Table.Select("dfyifan=1").Length > 0)
                {
                    if (!CheckSmsContent(SmsXM.到货等通知放货, SendTo.收货人, ref temp3, true))
                        return;
                }
            }

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.到货通知自提))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                if (ConvertType.ToInt32(gridView3.GetRowCellValue(i, "ischecked")) == 0) continue;

                mb = CheckMb(GridOper.GetRowCellValueString(gridView3, i, "ConsigneeCellPhone"), GridOper.GetRowCellValueString(gridView3, i, "ConsigneePhone"));
                if (mb == "")
                {
                    continue;
                }

                qty = GridOper.GetRowCellValueString(gridView3, i, "WebPCS");
                bsite = GridOper.GetRowCellValueString(gridView3, i, "StartSite");
                esite = GridOper.GetRowCellValueString(gridView3, i, "DestinationSite");
                middlesite = GridOper.GetRowCellValueString(gridView3, i, "TransferSite");
                billno = GridOper.GetRowCellValueString(gridView3, i, "BillNO");
                unit = GridOper.GetRowCellValueString(gridView3, i, "CusOderNo");
                okprocess = GridOper.GetRowCellValueString(gridView3, i, "TransferMode");
                consignee = GridOper.GetRowCellValueString(gridView3, i, "ConsigneeName");
                shipper = GridOper.GetRowCellValueString(gridView3, i, "ConsignorName");
                product = GridOper.GetRowCellValueString(gridView3, i, "Varieties");
                billdate = ConvertType.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                acctype = GridOper.GetRowCellValueString(gridView3, i, "PaymentMode");
                // pickgoodsite = GridOper.GetRowCellValueString(gridView3, i, "WebAddress");
                pickgoodsite = GridOper.GetRowCellValueString(gridView3, i, "WebAddress");
                webPhone = GridOper.GetRowCellValueString(gridView3, i, "WebFetchTel");
                address = GridOper.GetRowCellValueString(gridView3, i, "ReceivAddress");
                accdaishou = ConvertType.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                accarrived = ConvertType.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));
                if (accdaishou == 0)
                {
                    accdaishou = ConvertType.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay"));
                }
                if (accarrived == 0)
                {
                    accarrived = ConvertType.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));
                }
                string acc = "0";
                acc = accarrived > 0 ? "及运费" + Math.Round(accarrived, 2).ToString() + "元" : "";
                acc += accarrived > 0 ? (accdaishou > 0 ? "和货款" + Math.Round(accdaishou, 2).ToString() + "元" : "") : (accdaishou > 0 ? "及货款" + Math.Round(accdaishou, 2).ToString() + "元" : "");

                if (isdfyifan && GridOper.GetRowCellValueString(gridView3, i, "NoticeState") == "1") //控货的
                {
                    content = temp3;
                    if (okprocess == "自提")
                    {
                        #region 自提

                        content = content.Replace("[a]", shipper);
                        content = content.Replace("[b]", consignee);
                        content = content.Replace("[c]", bsite);
                        content = content.Replace("[d]", esite);
                        content = content.Replace("[l]", middlesite);
                        content = content.Replace("[e]", billno);
                        content = content.Replace("[f]", unit.ToString());
                        content = content.Replace("[g]", product);
                        content = content.Replace("[h]", qty);
                        content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                        content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付

                        content = content.Replace("[i]", CommonClass.smsCompanyName);

                        content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[yy]", billdate.Year.ToString());
                        content = content.Replace("[nn]", billdate.Month.ToString());
                        content = content.Replace("[dd]", billdate.Day.ToString());
                        content = content.Replace("[hh]", billdate.Hour.ToString());
                        content = content.Replace("[mm]", billdate.Minute.ToString());

                        content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[YY]", datenow.Year.ToString());
                        content = content.Replace("[NN]", datenow.Month.ToString());
                        content = content.Replace("[DD]", datenow.Day.ToString());
                        content = content.Replace("[HH]", datenow.Hour.ToString());
                        content = content.Replace("[MM]", datenow.Minute.ToString());

                        content = content.Replace("[T6]", tel); //到货通知查询电话
                        content = content.Replace("[gb]", CommonClass.UserInfo.SiteName);
                        content = content.Replace("[tf]", acc);
                        content = content.Replace("[k]", webaddress);
                        content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);
                        content = content.Replace("[pg]", pickgoodsite);

                        #endregion
                    }

                    if (okprocess != "自提")
                    {
                        #region 送货

                        content = content.Replace("[a]", shipper);
                        content = content.Replace("[b]", consignee);
                        content = content.Replace("[c]", bsite);
                        content = content.Replace("[d]", esite);
                        content = content.Replace("[l]", middlesite);
                        content = content.Replace("[e]", billno);
                        content = content.Replace("[f]", unit.ToString());
                        content = content.Replace("[g]", product);
                        content = content.Replace("[h]", qty);
                        content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                        content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付

                        content = content.Replace("[i]", CommonClass.smsCompanyName);

                        content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[yy]", billdate.Year.ToString());
                        content = content.Replace("[nn]", billdate.Month.ToString());
                        content = content.Replace("[dd]", billdate.Day.ToString());
                        content = content.Replace("[hh]", billdate.Hour.ToString());
                        content = content.Replace("[mm]", billdate.Minute.ToString());

                        content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[YY]", datenow.Year.ToString());
                        content = content.Replace("[NN]", datenow.Month.ToString());
                        content = content.Replace("[DD]", datenow.Day.ToString());
                        content = content.Replace("[HH]", datenow.Hour.ToString());
                        content = content.Replace("[MM]", datenow.Minute.ToString());

                        content = content.Replace("[T6]", tel); //到货通知查询电话
                        content = content.Replace("[gb]", CommonClass.UserInfo.SiteName);
                        content = content.Replace("[tf]", acc);
                        content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);
                        content = content.Replace("[pg]", pickgoodsite);

                        #endregion
                    }
                }
                else
                {
                    if (okprocess == "自提")
                    {
                        #region 自提
                        content = temp1;

                        content = content.Replace("[a]", shipper);
                        content = content.Replace("[b]", consignee);
                        content = content.Replace("[c]", bsite);
                        content = content.Replace("[d]", esite);
                        content = content.Replace("[l]", middlesite);
                        content = content.Replace("[f]", unit.ToString());
                        content = content.Replace("[e]", billno);
                        content = content.Replace("[g]", product);
                        content = content.Replace("[h]", qty);
                        content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                        content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付

                        content = content.Replace("[i]", CommonClass.smsCompanyName);

                        content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[yy]", billdate.Year.ToString());
                        content = content.Replace("[nn]", billdate.Month.ToString());
                        content = content.Replace("[dd]", billdate.Day.ToString());
                        content = content.Replace("[hh]", billdate.Hour.ToString());
                        content = content.Replace("[mm]", billdate.Minute.ToString());

                        content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[YY]", datenow.Year.ToString());
                        content = content.Replace("[NN]", datenow.Month.ToString());
                        content = content.Replace("[DD]", datenow.Day.ToString());
                        content = content.Replace("[HH]", datenow.Hour.ToString());
                        content = content.Replace("[MM]", datenow.Minute.ToString());

                        content = content.Replace("[T6]", webPhone); //到货通知查询电话
                        content = content.Replace("[gb]", CommonClass.UserInfo.SiteName);
                        content = content.Replace("[tf]", acc);
                        content = content.Replace("[k]", webaddress);
                        content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);
                        content = content.Replace("[pg]", pickgoodsite);

                        #endregion
                    }

                    if (okprocess != "自提")
                    {
                        #region 送货
                        content = temp2;

                        content = content.Replace("[a]", shipper);
                        content = content.Replace("[b]", consignee);
                        content = content.Replace("[c]", bsite);
                        content = content.Replace("[d]", esite);
                        content = content.Replace("[l]", middlesite);
                        content = content.Replace("[e]", billno);
                        content = content.Replace("[f]", unit.ToString());
                        content = content.Replace("[g]", product);
                        content = content.Replace("[h]", qty);
                        content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                        content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付

                        content = content.Replace("[i]", CommonClass.smsCompanyName);

                        content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[yy]", billdate.Year.ToString());
                        content = content.Replace("[nn]", billdate.Month.ToString());
                        content = content.Replace("[dd]", billdate.Day.ToString());
                        content = content.Replace("[hh]", billdate.Hour.ToString());
                        content = content.Replace("[mm]", billdate.Minute.ToString());

                        content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                        content = content.Replace("[YY]", datenow.Year.ToString());
                        content = content.Replace("[NN]", datenow.Month.ToString());
                        content = content.Replace("[DD]", datenow.Day.ToString());
                        content = content.Replace("[HH]", datenow.Hour.ToString());
                        content = content.Replace("[MM]", datenow.Minute.ToString());

                        content = content.Replace("[T6]", tel); //到货通知查询电话
                        content = content.Replace("[gb]", CommonClass.UserInfo.SiteName);
                        content = content.Replace("[tf]", acc);
                        content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);
                        content = content.Replace("[pg]", pickgoodsite);

                        #endregion
                    }
                }

                #region 发送
                if (!SaveSMSS("2", consignee, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;

                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
                #endregion
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //提货发送短信 给发货人
        public static bool fetchsendsms(int isfromsf, int unit, string billno, string shipper, string consignee, DateTime billdate, string bsite, string esite, int qty, string product, string shippermb, string shippertel, DateTime fetchdate, string fetchman)
        {
            if (!checksmsid())
                return false;

            string mb = CheckMb(shippermb, shippertel);
            if (mb == "")
            {
                MsgBox.ShowOK(string.Format("{0}环节{1}手机号码不符合规则，短信未发送!", SmsXM.提货签收, SendTo.发货人));
                return false;
            }

            #region 取短信格式
            string temp = "";
            SmsXM xm = SmsXM.提货签收;
            SendTo sendto = SendTo.发货人;
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return false;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion
            string content = temp;

            content = content.Replace("[a]", shipper);
            content = content.Replace("[b]", consignee);
            content = content.Replace("[c]", bsite);
            content = content.Replace("[d]", esite);
            content = content.Replace("[e]", unit.ToString());
            content = content.Replace("[f]", billno);
            content = content.Replace("[g]", product);
            content = content.Replace("[h]", qty.ToString());
            content = content.Replace("[T7]", tel); //提货完成查询电话

            content = content.Replace("[i]", CommonClass.smsCompanyName);

            content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
            content = content.Replace("[yy]", billdate.Year.ToString());
            content = content.Replace("[nn]", billdate.Month.ToString());
            content = content.Replace("[dd]", billdate.Day.ToString());
            content = content.Replace("[hh]", billdate.Hour.ToString());
            content = content.Replace("[mm]", billdate.Minute.ToString());

            content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
            content = content.Replace("[YY]", datenow.Year.ToString());
            content = content.Replace("[NN]", datenow.Month.ToString());
            content = content.Replace("[DD]", datenow.Day.ToString());
            content = content.Replace("[HH]", datenow.Hour.ToString());
            content = content.Replace("[MM]", datenow.Minute.ToString());

            content = content.Replace("[to]", fetchdate.ToString("yyyy年MM月dd日"));//提货日期
            content = content.Replace("[ty]", fetchdate.Year.ToString());
            content = content.Replace("[tn]", fetchdate.Month.ToString());
            content = content.Replace("[td]", fetchdate.Day.ToString());
            content = content.Replace("[th]", fetchdate.Hour.ToString());
            content = content.Replace("[tm]", fetchdate.Minute.ToString());

            content = content.Replace("[tr]", fetchman);

            content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

            if (!SaveSMSS("5", shipper, mb, content, CommonClass.gcdate, billno)) return false;
            if (!sendsms(mb, content)) return false;

            MsgBox.ShowOK("短信发送完毕!");
            return true;
        }

        //送货签收的时候发送短信
        public static void sendoksms(int isfromsf, int unit, string billno, string shipper, string consignee, DateTime billdate, string bsite, string esite, int qty, string product, string shippermb, string shippertel, DateTime senddate, string fetchman)
        {
            if (!checksmsid())
                return;
            string mb = CheckMb(shippermb, shippertel);
            if (mb == "")
            {
                MsgBox.ShowOK(string.Format("{0}环节{1}手机号码不符合规则，短信未发送!", SmsXM.送货签收, SendTo.发货人));
                return;
            }

            #region 取短信格式
            string temp = "";
            SmsXM xm = SmsXM.送货签收;
            SendTo sendto = SendTo.发货人;
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion
            string content = temp;

            content = content.Replace("[a]", shipper);
            content = content.Replace("[b]", consignee);
            content = content.Replace("[c]", bsite);
            content = content.Replace("[d]", esite);
            content = content.Replace("[e]", unit.ToString());
            content = content.Replace("[f]", billno);
            content = content.Replace("[g]", product);
            content = content.Replace("[h]", qty.ToString());
            content = content.Replace("[T9]", tel); //送货签收查询电话

            content = content.Replace("[i]", CommonClass.smsCompanyName);

            content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
            content = content.Replace("[yy]", billdate.Year.ToString());
            content = content.Replace("[nn]", billdate.Month.ToString());
            content = content.Replace("[dd]", billdate.Day.ToString());
            content = content.Replace("[hh]", billdate.Hour.ToString());
            content = content.Replace("[mm]", billdate.Minute.ToString());

            content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
            content = content.Replace("[YY]", datenow.Year.ToString());
            content = content.Replace("[NN]", datenow.Month.ToString());
            content = content.Replace("[DD]", datenow.Day.ToString());
            content = content.Replace("[HH]", datenow.Hour.ToString());
            content = content.Replace("[MM]", datenow.Minute.ToString());

            content = content.Replace("[qs]", senddate.ToString("yyyy年MM月dd日"));//送货签收日期
            content = content.Replace("[sr]", fetchman); //送货签收人

            content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

            if (!SaveSMSS("6", shipper, mb, content, CommonClass.gcdate, billno)) return;
            if (!sendsms(mb, content)) return;

            XtraMessageBox.Show("通知短信发送完毕!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //送货发送短信 给发货人
        public static void send_shipper(GridView gridView3, Form wmain, DateTime senddate, string sendman, string sendmantel)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", bsite = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", esite = "", product = "";
            string consignee = "", shipper = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.送货完成, SendTo.发货人, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.送货完成))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;
                mb = CheckMb(gridView3.GetRowCellValue(i, "ConsignorCellPhone").ToString().Trim(), gridView3.GetRowCellValue(i, "ConsignorPhone").ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                //isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));
                qty = gridView3.GetRowCellValue(i, "sendqty").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                esite = gridView3.GetRowCellValue(i, "DestinationSite").ToString();
                middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                shipper = gridView3.GetRowCellValue(i, "ConsignorName").ToString();
                //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[l]", middlesite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[SqlHelper]", senddate.ToString("yyyy年MM月dd日")); //送货日期
                content = content.Replace("[sy]", senddate.Year.ToString());
                content = content.Replace("[sn]", senddate.Month.ToString());
                content = content.Replace("[sd]", senddate.Day.ToString());
                content = content.Replace("[sh]", senddate.Hour.ToString());
                content = content.Replace("[sm]", senddate.Minute.ToString());

                content = content.Replace("[T8]", tel); //完成送货查询电话

                content = content.Replace("[sc]", sendman);
                content = content.Replace("[T15]", sendmantel);

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("6", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //送货发送短信 给收货人
        public static void send_consignee(GridView gridView3, Form wmain, DateTime senddate, string sendman, string sendmantel)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", bsite = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", esite = "", product = "";
            string consignee = "", shipper = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.送货完成, SendTo.收货人, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.送货完成))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }





            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;
                mb = CheckMb(gridView3.GetRowCellValue(i, "ConsigneeCellPhone").ToString().Trim(), gridView3.GetRowCellValue(i, "ConsigneePhone").ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                qty = gridView3.GetRowCellValue(i, "sendqty").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                esite = gridView3.GetRowCellValue(i, "DestinationSite").ToString();
                middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                shipper = gridView3.GetRowCellValue(i, "ConsignorName").ToString();
                //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));

                //isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[l]", middlesite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[SqlHelper]", senddate.ToString("yyyy年MM月dd日")); //送货日期
                content = content.Replace("[sy]", senddate.Year.ToString());
                content = content.Replace("[sn]", senddate.Month.ToString());
                content = content.Replace("[sd]", senddate.Day.ToString());
                content = content.Replace("[sh]", senddate.Hour.ToString());
                content = content.Replace("[sm]", senddate.Minute.ToString());

                content = content.Replace("[T8]", tel); //完成送货查询电话

                content = content.Replace("[sc]", sendman);
                content = content.Replace("[T15]", sendmantel);

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("6", consignee, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //回单寄出
        public static void backonesms(DataTable dt)
        {
            if (!checksmsid())
                return;
            if (!smschecked(dt))
                return;
            if (dt.Rows.Count == 0)
                return;
            string mb = "", content = "", qty = "", bsite = "", esite = "", billno = "", product = "";
            string consignee = "", shipper = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.回单寄出, SendTo.发货人, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.回单寄出))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ischecked = Convert.ToInt32(dt.Rows[i]["ischecked"]);

                if (ischecked != 1)
                    continue;
                mb = CheckMb(dt.Rows[i]["shippermb"].ToString().Trim(), dt.Rows[i]["shippertel"].ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                qty = dt.Rows[i]["qty"].ToString();
                esite = dt.Rows[i]["esite"].ToString();
                billno = dt.Rows[i]["billno"].ToString();
                consignee = dt.Rows[i]["consignee"].ToString();
                shipper = dt.Rows[i]["shipper"].ToString();
                unit = dt.Rows[i]["unit"].ToString();
                product = dt.Rows[i]["product"].ToString();
                billdate = Convert.ToDateTime(dt.Rows[i]["billdate"]);
                bsite = dt.Rows[i]["bsite"].ToString();

                isfromsf = dt.Rows[i]["isfromsf"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["isfromsf"]);

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[T11]", tel); //回单寄出查询电话

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("5", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //回单返回批量发送
        public static void backtwosms(DataTable dt)
        {
            if (!checksmsid())
                return;
            if (!smschecked(dt))
                return;
            if (dt.Rows.Count == 0)
                return;
            string mb = "", content = "", bsite = "", middlesite = "", billno = "";
            string qty = "", esite = "", product = "";
            string consignee = "", shipper = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.回单返回, SendTo.发货人, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.回单返回))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ischecked = Convert.ToInt32(dt.Rows[i]["ischecked"]);

                if (ischecked != 1)
                    continue;
                mb = CheckMb(dt.Rows[i]["shippermb"].ToString().Trim(), dt.Rows[i]["shippertel"].ToString().Trim());
                if (mb == "")
                    continue;

                qty = dt.Rows[i]["qty"].ToString();
                esite = dt.Rows[i]["esite"].ToString();
                middlesite = dt.Rows[i]["middlesite"].ToString();
                billno = dt.Rows[i]["billno"].ToString();
                consignee = dt.Rows[i]["consignee"].ToString();
                shipper = dt.Rows[i]["shipper"].ToString();
                unit = dt.Rows[i]["unit"].ToString();
                product = dt.Rows[i]["product"].ToString();
                billdate = Convert.ToDateTime(dt.Rows[i]["billdate"]);
                bsite = dt.Rows[i]["bsite"].ToString();

                isfromsf = dt.Rows[i]["isfromsf"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[i]["isfromsf"]);

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[T12]", tel); //回单返回查询电话

                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("15", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                k++;


            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //提取单个发送
        public static void onesendsms(string shipper, string mb, string content, string createby)
        {
            if (!checksmsid())
                return;
            mb = CheckMb(mb, mb);
            if (mb == "")
            {
                MsgBox.ShowOK("手机号码不符合规则，短信未发送!");
                return;
            }
            if (!SaveSMSS("12", shipper, mb, content, CommonClass.gcdate, "0")) return;
            if (!sendsms(mb, content)) return;
        }

        //货到前付催款短信
        public static void dunningsms(GridView gridView3, Form wmain, string range, int state)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            string bsite = "", esite = "", product = "", billno = "", consignee = "", shipper = "",
                  content = "", unit = "", qty = "", acctype = "", okprocess = "", mb = "", acctotal = "", package = "";

            if (gridView3.RowCount == 0)
                return;
            int k = 0, ischecked = 0;

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;
                mb = CheckMb(gridView3.GetRowCellValue(i, "shippermb").ToString().Trim(), gridView3.GetRowCellValue(i, "shippertel").ToString().Trim());
                if (mb == "")
                    continue;

                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                package = gridView3.GetRowCellValue(i, "package").ToString();
                acctotal = gridView3.GetRowCellValue(i, "acctotal").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode")); ;

                content = shipper + ",您好！您" + billdate.Month.ToString() + "月" + billdate.Day.ToString() + "日发" + esite + qty + product + "," + acctype + "运费" + acctotal + "元,请及时到我司付款，以方便营运操作!【" + CommonClass.smsCompanyName + "】";

                if (content.Trim() == "")
                {
                    XtraMessageBox.Show("本环节没有短信内容,不可发送!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!SaveSMSS("13", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //货款回收
        public static void hkin(GridView gridView3, SendTo sendto, DateTime billdate)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webtel = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            string acceptman = "", fieldtel = "shippertel", fieldmb = "shippermb";
            string unit = "", hkfactin = "";

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.货款回收, sendto, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.货款回收))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;

            int isfromsf = 0;//来自三方的运单1

            if (sendto == SendTo.收货人)
            {
                fieldtel = "consigneetel";
                fieldmb = "consigneemb";
            }

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;

                mb = CheckMb(gridView3.GetRowCellValue(i, fieldmb).ToString().Trim(), gridView3.GetRowCellValue(i, fieldtel).ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                hkfactin = gridView3.GetRowCellValue(i, "hkfactin").ToString();

                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "billdate"));

                isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit);
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[$$]", hkfactin);

                content = content.Replace("[T13]", tel); //回收通知查询电话
                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("21", sendto == SendTo.发货人 ? shipper : consignee, mb, content, datenow, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //货款到账
        public static void hkarrived(GridView gridView3, SendTo sendto, DateTime billdate)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webtel = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            string acceptman = "", fieldtel = "shippertel", fieldmb = "shippermb";
            string unit = "", hkfactin = "";

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.货款到账, sendto, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.货款到账))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;

            int isfromsf = 0;//来自三方的运单1

            if (sendto == SendTo.收货人)
            {
                fieldtel = "consigneetel";
                fieldmb = "consigneemb";
            }

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;

                mb = CheckMb(gridView3.GetRowCellValue(i, fieldmb).ToString().Trim(), gridView3.GetRowCellValue(i, fieldtel).ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                hkfactin = gridView3.GetRowCellValue(i, "hkfactin").ToString();

                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "billdate"));

                isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit);
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[$$]", hkfactin);
                content = content.Replace("[T10]", tel); //到账通知查询电话
                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("22", sendto == SendTo.发货人 ? shipper : consignee, mb, content, datenow, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;


            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //货款发放
        public static void hkout(GridView gridView3, SendTo sendto, DateTime billdate)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webtel = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            string acceptman = "", fieldtel = "shippertel", fieldmb = "shippermb";
            string unit = "", hkfactout = "";

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.货款发放, sendto, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.货款发放))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;

            int isfromsf = 0;//来自三方的运单1

            if (sendto == SendTo.收货人)
            {
                fieldtel = "consigneetel";
                fieldmb = "consigneemb";
            }

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;

                mb = CheckMb(gridView3.GetRowCellValue(i, fieldmb).ToString().Trim(), gridView3.GetRowCellValue(i, fieldtel).ToString().Trim());
                if (mb == "")
                {
                    continue;
                }

                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                hkfactout = gridView3.GetRowCellValue(i, "hkfactout").ToString();

                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "billdate"));

                isfromsf = gridView3.GetRowCellValue(i, "isfromsf") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "isfromsf"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[e]", unit);
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[$$]", hkfactout);//实发货款

                content = content.Replace("[T14]", tel); //发放通知查询电话
                content = content.Replace("[gs]", isfromsf == 1 ? hyh : gs);

                if (!SaveSMSS("23", sendto == SendTo.发货人 ? shipper : consignee, mb, content, datenow, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //发车界面通知货到前付付款
        public static void fukuansms(GridView gridView3, Form wmain, string range, DateTime arriveddate)
        {
            if (!checksmsid()) return;
            if (!smschecked(gridView3)) return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", esite = "", acctrans = "", webtel = "", webaddress = "", product = "";
            string consignee = "", shipper = "";
            string acctype = "", accback = "";
            if (gridView3.RowCount == 0) return;
            int k = 0, ischecked = 0;
            string unit = "";

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked == 1)
                {
                    mb = gridView3.GetRowCellValue(i, "ConsignorCellPhone").ToString().Trim();

                    if (mb == "" || mb.Substring(0, 1) != "1")
                    {
                        mb = gridView3.GetRowCellValue(i, "ConsignorPhone").ToString().Trim();
                        if (mb != "")
                            if (mb.Substring(0, 1) != "1") continue;
                    }

                    qty = gridView3.GetRowCellValue(i, "WebPCS").ToString();
                    string bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                    middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                    //CommonClass.GetSiteCode(esite, ref webtel, ref webaddress);
                    acctrans = gridView3.GetRowCellValue(i, "BefArrivalPay").ToString();
                    billno = gridView3.GetRowCellValue(i, "BillNO").ToString();
                    okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                    consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                    shipper = gridView3.GetRowCellValue(i, "ConsignorName").ToString();
                    //unit = gridView3.GetRowCellValue(i, "unit").ToString();
                    product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                    DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                    //string bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                    //accback = gridView3.GetRowCellValue(i, "accback").ToString();
                    acctype = gridView3.GetRowCellValue(i, "PaymentMode").ToString();



                    if (content.Trim() == "")
                    {
                        XtraMessageBox.Show("本环节没有短信内容,不可发送!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (mb != "")
                    {
                        if (!SaveSMSS("14", shipper, mb, content, CommonClass.gcdate, billno)) return;
                        if (!sendsms(mb, content)) return;
                        gridView3.FocusedRowHandle = i;
                        gridView3.SetRowCellValue(i, "ischecked", 2);
                        k++;
                    }

                }
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region 三方短信
        //在途跟踪界面 发车短信 给发货人
        public static void sffcdsms_shipper(GridView gridView3)
        {
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", product = "";
            string consignee = "", shipper = "", acctype = "", webid = "";
            decimal accdaishou = 0, accarrived = 0;
            DateTime senddate = DateTime.Now;
            int state = 0;


            #region 取短信格式
            string temp = "";
            SmsXM xm = SmsXM.配载完成;
            SendTo sendto = SendTo.发货人;
            if (!CheckSmsContent(xm, sendto, ref temp, true))
                return;
            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", xm))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked == 1)
                {
                    mb = CheckMb(gridView3.GetRowCellValue(i, "shippermb").ToString().Trim(), gridView3.GetRowCellValue(i, "shippertel").ToString().Trim());
                    if (mb == "")
                    {
                        continue;
                    }
                    state = Convert.ToInt32(gridView3.GetRowCellValue(i, "state").ToString());
                    if (state != 1) continue;

                    qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                    bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                    middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                    billno = gridView3.GetRowCellValue(i, "BillNo").ToString();
                    okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                    consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                    shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                    unit = gridView3.GetRowCellValue(i, "unit").ToString();
                    product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                    DateTime billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                    bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                    acctype = ConvertType.ToString(gridView3.GetRowCellValue(i, "PaymentMode")); ;
                    webid = gridView3.GetRowCellValue(i, "webid") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "webid").ToString();
                    // accdaishou = gridView3.GetRowCellValue(i, "CollectionPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "CollectionPay")); //没提取
                    accarrived = gridView3.GetRowCellValue(i, "FetchPay") == DBNull.Value ? 0 : Convert.ToDecimal(gridView3.GetRowCellValue(i, "FetchPay"));
                    senddate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "senddate"));

                    content = temp;

                    content = content.Replace("[a]", shipper);
                    content = content.Replace("[b]", consignee);
                    content = content.Replace("[c]", bsite);
                    content = content.Replace("[d]", esite);
                    content = content.Replace("[l]", middlesite);
                    content = content.Replace("[e]", unit.ToString());
                    content = content.Replace("[f]", billno);
                    content = content.Replace("[g]", product);
                    content = content.Replace("[h]", qty);
                    content = content.Replace("[$$]", (Math.Round(accdaishou, 2)).ToString()); //代收款
                    content = content.Replace("[$$$]", (Math.Round(accarrived, 2)).ToString());//提付
                    content = content.Replace("[we]", webid);//开单网点

                    content = content.Replace("[i]", CommonClass.smsCompanyName);

                    content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[yy]", billdate.Year.ToString());
                    content = content.Replace("[nn]", billdate.Month.ToString());
                    content = content.Replace("[dd]", billdate.Day.ToString());
                    content = content.Replace("[hh]", billdate.Hour.ToString());
                    content = content.Replace("[mm]", billdate.Minute.ToString());

                    content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                    content = content.Replace("[YY]", datenow.Year.ToString());
                    content = content.Replace("[NN]", datenow.Month.ToString());
                    content = content.Replace("[DD]", datenow.Day.ToString());
                    content = content.Replace("[HH]", datenow.Hour.ToString());
                    content = content.Replace("[MM]", datenow.Minute.ToString());

                    content = content.Replace("[po]", senddate.ToString("yyyy年MM月dd日")); //配载日期
                    content = content.Replace("[py]", senddate.Year.ToString());
                    content = content.Replace("[pn]", senddate.Month.ToString());
                    content = content.Replace("[pd]", senddate.Day.ToString());
                    content = content.Replace("[ph]", senddate.Hour.ToString());
                    content = content.Replace("[pm]", senddate.Minute.ToString());

                    content = content.Replace("[pt]", acctype == "货到前付" ? "为了不延误货物交接，请及时付款，" : ""); //货到前付提示
                    content = content.Replace("[T2]", tel); //配载完成查询电话
                    content = content.Replace("[gs]", gs);

                    if (!SaveSMSS("1", shipper, mb, content, CommonClass.gcdate, billno)) return;
                    if (!sendsms(mb, content)) return;

                    gridView3.FocusedRowHandle = i;
                    gridView3.SetRowCellValue(i, "ischecked", 2);
                    k++;
                }
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //送货签收发给发货人的
        public int sendSignsms_to_shipper(GridView gridView3, DateTime arriveddate, string companyid1, string signMan, string signManTel, string companyName)
        {
            if (!checksmsid())       
                return 0;
            //if (!smschecked(gridView3))
            //    return;
            if (gridView3.RowCount == 0)
                return 0;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            string num = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "", temp1 = "";
            if (!CheckSmsContent1(SmsXM.送货签收, SendTo.发货人, ref temp, true, companyid1))
                return 0;
            DataView dv = (DataView)gridView3.DataSource;


            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("companyId0", companyid1));
            list.Add(new SqlPara("project", "送货签收"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SMSCONTENT", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0) return 0;
            content = ds.Tables[0].Rows[0]["shipper"].ToString();

            #endregion

            int k = 0;
            string unit = "";

            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                content = ds.Tables[0].Rows[0]["shipper"].ToString();
                mb = CheckMb(GridOper.GetRowCellValueString(gridView3, i, "ConsignorCellPhone"), "");
                if (mb == "")
                {
                    continue;
                }

                qty = GridOper.GetRowCellValueString(gridView3, i, "SendPCS");
                bsite = GridOper.GetRowCellValueString(gridView3, i, "StartSite");
                esite = GridOper.GetRowCellValueString(gridView3, i, "DestinationSite");
                middlesite = GridOper.GetRowCellValueString(gridView3, i, "TransferSite");
                billno = GridOper.GetRowCellValueString(gridView3, i, "BillNo");
                num = GridOper.GetRowCellValueString(gridView3, i, "Num");
                consignee = GridOper.GetRowCellValueString(gridView3, i, "ConsigneeName");
                shipper = GridOper.GetRowCellValueString(gridView3, i, "ConsignorName");
                product = GridOper.GetRowCellValueString(gridView3, i, "Varieties");
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                acctype = GridOper.GetRowCellValueString(gridView3, i, "PaymentMode");
                content = content.Replace("[sr]", signMan);
                content = content.Replace("[T9]", signManTel);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[a]", shipper);
                content = content.Replace("[d]", esite);
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", num);
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[gs]", companyName);

                if (!SaveSMSS2("2", shipper, mb, content, CommonClass.gcdate, billno)) return 0;
                if (!sendsms(mb, content)) return 0;
                k++;
                ;

            }
            return k;

        }

        //送货签收发给收货人的

        public int sendSignsms_to_shipper2(GridView gridView3, DateTime arriveddate, string companyid1, string signMan, string signManTel, string companyName)
        {
            if (!checksmsid())
                return 0;
            //if (!smschecked(gridView3))
            //    return;
            if (gridView3.RowCount == 0)
                return 0;
            string mb = "", content = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", bsite = "", esite = "", webaddress = "", product = "";
            string consignee = "", shipper = "", acctype = "";
            string num = "";
            DateTime billdate = DateTime.Now;

            #region 取短信格式
            string temp = "", temp1 = "";
            if (!CheckSmsContent1(SmsXM.送货签收, SendTo.发货人, ref temp, true, companyid1))
                return 0;
            DataView dv = (DataView)gridView3.DataSource;


            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("companyId0", companyid1));
            list.Add(new SqlPara("project", "送货签收"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SMSCONTENT", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0) return 0;
            content = ds.Tables[0].Rows[0]["forConsignee"].ToString();
            #endregion

            int k = 0;
            string unit = "";

            int isfromsf = 0;//来自三方的运单1

            for (int i = 0; i < gridView3.RowCount; i++)
            {
                content = ds.Tables[0].Rows[0]["forConsignee"].ToString();
                mb = CheckMb(GridOper.GetRowCellValueString(gridView3, i, "ConsigneeCellPhone"), "");
                if (mb == "")
                {
                    continue;
                }

                qty = GridOper.GetRowCellValueString(gridView3, i, "SendPCS");
                bsite = GridOper.GetRowCellValueString(gridView3, i, "StartSite");
                esite = GridOper.GetRowCellValueString(gridView3, i, "DestinationSite");
                middlesite = GridOper.GetRowCellValueString(gridView3, i, "TransferSite");
                billno = GridOper.GetRowCellValueString(gridView3, i, "BillNo");
                num = GridOper.GetRowCellValueString(gridView3, i, "Num");
                consignee = GridOper.GetRowCellValueString(gridView3, i, "ConsigneeName");
                shipper = GridOper.GetRowCellValueString(gridView3, i, "ConsignorName");
                product = GridOper.GetRowCellValueString(gridView3, i, "Varieties");
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "BillDate"));
                acctype = GridOper.GetRowCellValueString(gridView3, i, "PaymentMode");
                content = content.Replace("[sr]", signMan);
                content = content.Replace("[T9]", signManTel);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[d]", esite);
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", num);
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[gs]", companyName);
                if (!SaveSMSS2("2", shipper, mb, content, CommonClass.gcdate, billno)) return 0;
                if (!sendsms(mb, content)) return 0;

                ;
                k++;
            }
            return k;

        }

        //送货发送短信 给发货人
        public static void sfsend_shipper(GridView gridView3)
        {
           
            if (!checksmsid())
                return;
            if (!smschecked(gridView3))
                return;
            if (gridView3.RowCount == 0)
                return;
            string mb = "", content = "", bsite = "", middlesite = "", billno = "", okprocess = "";
            string qty = "", esite = "", product = "";
            string consignee = "", shipper = "";
            DateTime billdate = DateTime.Now;
            DateTime senddate = DateTime.Now;
            DateTime fetchdate = DateTime.Now;
            int state = 0;
            string fetchman = "";
            //string sendman="", string sendmantel=""

            #region 取短信格式
            string temp = "";
            if (!CheckSmsContent(SmsXM.送货完成, SendTo.发货人, ref temp, true))
                return;

            DateTime datenow = CommonClass.ServerDate;
            string tel = "", gs = "";
            if (CommonClass.dsmsgtel.Tables.Count > 0 && CommonClass.dsmsgtel.Tables[0].Rows.Count > 0)
            {
                tel = CommonClass.dsmsgtel.Tables[0].Select(string.Format("xm='{0}'", SmsXM.送货完成))[0]["tel"].ToString();
                gs = CommonClass.dsmsgtel.Tables[0].Select("xm='公司签名'")[0]["tel"].ToString();
            }
            #endregion

            int k = 0, ischecked = 0;
            string unit = "";
            for (int i = 0; i < gridView3.RowCount; i++)
            {
                ischecked = gridView3.GetRowCellValue(i, "ischecked") == DBNull.Value ? 0 : Convert.ToInt32(gridView3.GetRowCellValue(i, "ischecked"));

                if (ischecked != 1)
                    continue;
                mb = CheckMb(gridView3.GetRowCellValue(i, "shippermb").ToString().Trim(), gridView3.GetRowCellValue(i, "shippertel").ToString().Trim());
                if (mb == "")
                {
                    continue;
                }
                state = Convert.ToInt32(gridView3.GetRowCellValue(i, "state"));
                if (state != 5 && state != 6) continue;

                qty = gridView3.GetRowCellValue(i, "SendPCS").ToString();
                bsite = gridView3.GetRowCellValue(i, "bsite").ToString();
                bsite = gridView3.GetRowCellValue(i, "StartSite").ToString();
                middlesite = gridView3.GetRowCellValue(i, "TransferSite").ToString();
                billno = gridView3.GetRowCellValue(i, "BillNO").ToString();
                okprocess = gridView3.GetRowCellValue(i, "TransferMode").ToString();
                consignee = gridView3.GetRowCellValue(i, "ConsigneeName").ToString();
                shipper = gridView3.GetRowCellValue(i, "shipper").ToString();
                unit = gridView3.GetRowCellValue(i, "unit").ToString();
                product = gridView3.GetRowCellValue(i, "Varieties").ToString();
                billdate = Convert.ToDateTime(gridView3.GetRowCellValue(i, "billdate"));

                fetchman = gridView3.GetRowCellValue(i, "fetchman") == DBNull.Value ? "" : gridView3.GetRowCellValue(i, "fetchman").ToString();
                fetchdate = gridView3.GetRowCellValue(i, "fetchdate") == DBNull.Value ? DateTime.Now : Convert.ToDateTime(gridView3.GetRowCellValue(i, "fetchdate"));

                content = temp;

                content = content.Replace("[a]", shipper);
                content = content.Replace("[b]", consignee);
                content = content.Replace("[c]", bsite);
                content = content.Replace("[d]", esite);
                content = content.Replace("[l]", middlesite);
                content = content.Replace("[e]", unit.ToString());
                content = content.Replace("[f]", billno);
                content = content.Replace("[g]", product);
                content = content.Replace("[h]", qty);

                content = content.Replace("[i]", CommonClass.smsCompanyName);

                content = content.Replace("[o]", billdate.ToString("yyyy年MM月dd日"));
                content = content.Replace("[yy]", billdate.Year.ToString());
                content = content.Replace("[nn]", billdate.Month.ToString());
                content = content.Replace("[dd]", billdate.Day.ToString());
                content = content.Replace("[hh]", billdate.Hour.ToString());
                content = content.Replace("[mm]", billdate.Minute.ToString());

                content = content.Replace("[p]", datenow.ToString("yyyy年MM月dd日"));
                content = content.Replace("[YY]", datenow.Year.ToString());
                content = content.Replace("[NN]", datenow.Month.ToString());
                content = content.Replace("[DD]", datenow.Day.ToString());
                content = content.Replace("[HH]", datenow.Hour.ToString());
                content = content.Replace("[MM]", datenow.Minute.ToString());

                content = content.Replace("[tr]", fetchman);
                content = content.Replace("[to]", fetchdate.ToString("yyyy年MM月dd日"));//提货日期
                content = content.Replace("[ty]", fetchdate.Year.ToString());
                content = content.Replace("[tn]", fetchdate.Month.ToString());
                content = content.Replace("[td]", fetchdate.Day.ToString());
                content = content.Replace("[th]", fetchdate.Hour.ToString());
                content = content.Replace("[tm]", fetchdate.Minute.ToString());
                content = content.Replace("[T7]", tel); //提货完成查询电话

                content = content.Replace("[SqlHelper]", senddate.ToString("yyyy年MM月dd日")); //送货日期
                content = content.Replace("[sy]", senddate.Year.ToString());
                content = content.Replace("[sn]", senddate.Month.ToString());
                content = content.Replace("[sd]", senddate.Day.ToString());
                content = content.Replace("[sh]", senddate.Hour.ToString());
                content = content.Replace("[sm]", senddate.Minute.ToString());

                content = content.Replace("[T8]", tel); //完成送货查询电话

                //content = content.Replace("[sc]", sendman);
                //content = content.Replace("[T15]", sendmantel);

                content = content.Replace("[gs]", gs);

                if (!SaveSMSS("6", shipper, mb, content, CommonClass.gcdate, billno)) return;
                if (!sendsms(mb, content)) return;
                gridView3.FocusedRowHandle = i;
                gridView3.SetRowCellValue(i, "ischecked", 2);
                k++;
            }
            XtraMessageBox.Show("通知短信发送完毕，本次发送了" + k.ToString() + "条", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }

    public class SmsResult<T>
    {
        public string status { get; set; }

        public string balance { get; set; }

        public T list { get; set; }

        public string chargeType { get; set; }

        public string msg { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string send_qty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
    }
    public class SendSmsModel
    {
        public string mid { get; set; }

        public string mobile { get; set; }

        public string result { get; set; }

    }

}