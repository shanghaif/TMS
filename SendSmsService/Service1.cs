using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using ThisisLanQiaoSoft;

namespace SendSmsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        /// <summary>
        //短信账户和密码
        /// </summary>
        string smsid = "szhyhdk1076";
        string smsuserid = "szhyhdk1076";
        string smspassword = "szhyhdk1076";
        string smsCompanyName = "中强物流";//公司名称

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 3500000;//每3500秒执行一次
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour != 10) return;//每天10点才发送
            DataTable dt = SqlHelper.GetDataTable("QSP_GET_SMS_AUTO");
            if (dt == null || dt.Rows.Count == 0) return;

            string tel = "", content = "", IdStr = "", msg = "", msgStr = "", IdOkStr = "";
            bool isSend = true;
            foreach (DataRow dr in dt.Rows)
            {
                isSend = true;
                tel = ToString(dr["telephone"]);
                content = ToString(dr["content"]);

                if (tel == "")
                {
                    isSend = false;
                    msg = "没有手机号码,不能发送!";
                }
                else if (content == "")
                {
                    isSend = false;
                    msg = "没有短信内容,不能发送!";
                }
                else
                {
                    tel = CheckMb(tel);
                    if (tel == "")
                    {
                        isSend = false;
                        msg = "手机号码有误,不能发送!";
                    }
                }

                //验证不通过,跳过
                if (!isSend)
                {
                    IdStr += ToString(dr["Id"]) + "@";
                    msgStr += msg + "@";
                    continue;
                }

                try
                {
                    string result = SMS.Sendsms(smsCompanyName, "自动发送", "自动发送", smsuserid, smsid, smspassword, tel, content);
                    string[] Arr = result.Split('|');

                    if (Arr[0].Trim() == "1")
                    {
                        IdOkStr += ToString(dr["Id"]) + "@";//发送成功的要删掉
                    }
                    else//记录发送失败
                    {
                        IdStr += ToString(dr["Id"]) + "@";
                        msgStr += "发送返回信息:" + Arr[1].Replace('@', '_') + "@";
                    }
                }
                catch (Exception ex)
                {
                    IdStr += ToString(dr["Id"]) + "@";
                    msgStr += "发送异常信息:" + ex.Message.Replace('@', '_') + "@";
                }
            }

            //发送完删除已成功发送的记录
            if (IdStr == "" && IdOkStr == "") return;
            SqlHelper.ENQ("USP_SET_SMS_AUTO_COMPLETE", new SqlParameter[] { new SqlParameter("@IdStr", IdStr), new SqlParameter("@msgStr", msgStr), new SqlParameter("@IdOkStr", IdOkStr) });
        }

        /// <summary>
        /// 检测电话号码是否符合规则
        /// </summary>
        /// <param name="mb">手机</param>
        /// <param name="tel">电话</param>
        /// <returns></returns>
        string CheckMb(string mb)
        {
            string strMatch = "^1[3|4|5|8][0-9]\\d{8}$";
            if (Regex.IsMatch(mb, strMatch))
            {
                return mb;
            }
            else
            {
                return "";
            }
        }

        string ToString(object obj)
        {
            return (obj + "").Trim();
        }

        //protected override void OnStop()
        //{
        //}
    }
}
