using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace ZQTMS.Common
{
    public static class HttpHelper
    {

        public static string urlSyn = "http://8.129.7.49:8022/"; //同步连接

        public static string UrlPage = "LeyService/CoreRun";
        public static string UrlPageNew = "KDLMSService/Post_SystemRequestProt";

        static string domain = "";//Java接口

        //static string domaimNew = "http://192.168.31.50:8013/";  //开发库连接
        static string domaimNew = "http://8.129.7.49:8013/";  //正式库连接
        //static string domaimNew = "http://192.168.1.234:8013/";  //正式库连接

        public static string WebPicUrl = ""; //主界面图片文件上传URL
        public static string RestartUrl = ""; //检测重启的URL

        public static string OAUrl = ""; //提交流程信息到OA
        public static string OAUrlImage = "";//提交图片到OA服务器

        public static string domaimZS = "h";//正式库URL lyj


        #region 同步正式库链接

        public static string urlWebApplySyn = urlSyn + "KDLMSService/LmsUpdateWayBill";//改单,网点费用异动同步url 测试库
        public static string urlAllocateToArtery = urlSyn+"KDLMSService/AllocateToArtery";//转分拨URL 测试库
        public static string urlCancelAllocate = urlSyn + "KDLMSService/CancelAllocateToArtery";//取消分拨Url 测试库
	   
        /// <summary>
        /// 签收同步
        /// </summary>
        public static string urlSignSyn = urlSyn+"KDLMSService/ZQTMSSignWayBill";

        /// <summary>
         ///中转同步URL
        /// </summary>
        public static string urlMiddleSyn = urlSyn+"KDLMSService/ZQTMSMiddleSys";

        /// <summary>
        /// 分拨接收同步URL
        /// </summary>
        public static string urlAllocateAcceptSyn = urlSyn + "KDLMSService/ZQTMSBillDepartueFBAcceptSyn";

        /// <summary>
        /// 分单配载同步URL
        /// </summary>
        public static string urlBilldepartureSyn = urlSyn + "KDLMSService/ZQTMSDepartureSys";

        /// <summary>
        /// 到货确认同步URL
        /// </summary>
        public static string urlArrivalConfirmSyn = urlSyn + "KDLMSService/ZQTMSArrivalConfirmSyn";

        /// <summary>
        /// 安排送货同步URL
        /// </summary>
        public static string urlBillSendSys = urlSyn + "KDLMSService/ZQTMSBillSendGoodsSyn";
		
        /// <summary>
        /// 转二级确认同步URL
        /// </summary>
        public static string urlSendToSiteConfirm = urlSyn + "KDLMSService/ZQTMSSendToSiteSyn";

        /// <summary>
        /// 取消配载同步URL
        /// </summary>
        public static string urlDepartureDeleteSyn = urlSyn + "KDLMSService/ZQTMSDepartureDeleteSyn";

        /// <summary>
        /// 取消到货同步URL
        /// </summary>
        public static string urlArrivalConfirmCancel = urlSyn + "KDLMSService/ZQTMSArrivalConfirmCancelSyn";

        /// <summary>
        /// 取消送货同步URL
        /// </summary>
        public static string urlBillSendCancelSyn = urlSyn + "KDLMSService/ZQTMSBillSendCancelSyn";

        /// <summary>
        /// 取消转二级到货URL
        /// </summary>
        public static string urlSendtoSiteConfirmCancel = urlSyn + "KDLMSService/ZQTMSSendToSiteConfirmCancel";

        /// <summary>
        /// 取消中转URL
        /// </summary>
        public static string urlMiddleCancel = urlSyn + "KDLMSService/ZQTMSMiddleCancel";

        /// <summary>
        /// 取消签收同步URL
        /// </summary>
        public static string urlSignCancelSyn = urlSyn + "KDLMSService/ZQTMSSignCancelSyn";

        /// <summary>
        /// 取消接受同步URL
        /// </summary>
        public static string urlAllocateCancelSyn = urlSyn + "KDLMSService/ZQTMSBillDepartueFBCancelSyn";

        /// <summary>
        /// 放货同步URL
        /// </summary>
        public static string urlReleaseSyn = urlSyn + "KDLMSService/LMSReleaseSyn";

        /// <summary>
        /// 签收同步时效URL
        /// </summary>
        public static string urlSignTimeSyn = urlSyn + "KDLMSService/ZQTMSSignTimeSyn";

        /// <summary>
        /// 同步取消时效URL
        /// </summary>
        public static string urlTimeCancelSyn = urlSyn + "KDLMSService/ZQTMSTimeCancelSyn";

        /// <summary>
        /// 同步配载修改时效URL
        /// </summary>
        public static string urlTimeDepartUptSyn = urlSyn + "KDLMSService/ZQTMSTimeDepartUptSyn";

        /// <summary>
        /// 同步其他修改时效URL
        /// </summary>
        public static string urlTimeOtherUptSyn = urlSyn + "KDLMSService/ZQTMSTimeOtherUptSyn";

        /// <summary>
        /// 同步送货修改时效URL
        /// </summary>
        public static string urlTimeSendUptSyn = urlSyn + "KDLMSService/ZQTMSTimeSendUptSyn";

        /// <summary>
        /// 备注同步URL
        /// </summary>
        public static string urlNoteSyn = urlSyn + "KDLMSService/LMSNoteSyn";

        /// <summary>
        /// 改单审批同步URL
        /// </summary>
        public static string urlReviewSyn = urlSyn + "KDLMSService/LMSReviewSyn";

        /// <summary>
        /// 轨迹同步
        /// </summary>
        public static string urlTraceSyn = urlSyn + "KDLMSService/LMSTraceSyn";

        /// <summary>
        /// 回单上传同步
        /// </summary>
        public static string urlReceiptUploadSyn = urlSyn + "KDLMSService/ZQTMSReceiptSyn"; 


        /// <summary>
        /// 拨入接收回单问题件同步
        /// </summary>
        public static string urlHDProblemPartsSyn = urlSyn + "KDLMSService/ZQTMSHDProblemPartsSyn";

        /// <summary>
        /// 回单取消同步
        /// </summary>
        public static string urlReceiptCancelSyn = urlSyn + "KDLMSService/ZQTMSReceiptCancelSyn";

        /// <summary>
        /// 回单寄出(新)同步
        /// </summary>
        public static string urlReceiptSendNewSyn = urlSyn + "KDLMSService/ZQTMSReceiptSendNewSyn"; 

        /// <summary>
        /// 异常登记同步URL
        /// </summary>
        public static string urlAbnormalSyn = urlSyn + "KDLMSService/ZQTMSAbnormalSyn";


        /// <summary>
        /// 撤销异常登记同步URL
        /// </summary>
        public static string urlUndoAbnormalSyn = urlSyn + "KDLMSService/ZQTMSUndoAbnormalSyn";

        /// <summary>
        /// LMS查询ZQTMS理赔审批信息
        /// </summary>
        public static string urlGetZQTMSClaimMessage = urlSyn + "KDLMSService/GetZQTMSClaimMessage";

        /// <summary>
        /// 费用异动执行同步URL maohui20180514
        /// </summary>
        public static string urlFreightChangesExcuteSyn = urlSyn + "KDLMSService/LMSFreightChangesExcuteSyn";

        /// <summary>
        /// 费用异动否决同步URL maohui20180514
        /// </summary>
        public static string urlFreightChangesVetoSyn = urlSyn + "KDLMSService/LMSFreightChangesVetoSyn";

        /// <summary>
        /// 费用异动新增同步URL maohui20180514
        /// </summary>
        public static string urlFreightChangesADDSyn = urlSyn + "KDLMSService/LMSFreightChangesADDSyn";

        /// <summary>
        /// 费用异动取消同步URL maohui20180514
        /// </summary>
        public static string urlFreightChangesCancelSyn = urlSyn + "KDLMSService/LMSFreightChangesCancelSyn";


        /// <summary>
        /// 中转提付确认同步URL zaj20180615
        /// </summary>
        public static string urlMidConfirmSyn = urlSyn + "KDLMSService/LMSMidConfirmSyn";

        public static string urlShortReplace = urlSyn + "KDLMSService/LMSShortConReplaceZQTMS";

        public static string urlShortCommon = urlSyn + "KDLMSService/LMSSysExecuteZQTMSCurrency";

        #endregion

        /// <summary>
        /// LMS增删改同步ZQTMS（通用）
        /// </summary>
        public static string urlLMSSysExecuteZQTMSCurrency = urlSyn + "KDLMSService/LMSSysExecuteZQTMSCurrency";

        /// <summary>
        /// LMS新增配载同步ZQTMS
        /// </summary>
        public static string urlLMSSysDeparttureZQTMS = urlSyn + "KDLMSService/LMSSysDeparttureZQTMS";


        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        public static SqlResult HttpGet(string data)
        {
            string url = domain + UrlPage;
            SqlResult result = new SqlResult();
            try
            {
                string json = "";//返回json字符串

                WebResponse wr = null;
                HttpWebRequest req = null;
                req = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", url, data));
                req.Proxy = null;
                req.Method = "get";
                req.Timeout = 600000;

                wr = req.GetResponse();
                using (Stream ReceiveStream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(ReceiveStream))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    ReceiveStream.Close();
                    ReceiveStream.Dispose();
                }
                wr.Close();

                result = JsonConvert.DeserializeObject<SqlResult>(json);
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

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        public static SqlResult HttpPost1(string data)
        {
            string url = domain + UrlPage;
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
        #region zaj 2017-11-17
        public static ResponseModelClone<string> HttpPost(string data)
        {
             string url = domaimNew + UrlPageNew;
            //string url = "http://localhost:42936/KDLMSService/Post_SystemRequestProt";
            //string url = "http://localhost:46663/TestService/Post_GetDataSet";

            ResponseModelClone<string> result = new ResponseModelClone<string>();
            try
            {


                string json = "";//返回json字符串

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/json";
                req.Timeout = 600000;
                //data = "{\"OperType\":2147483647,\"Request\":\"\"}";

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
                result = JsonConvert.DeserializeObject<ResponseModelClone<string>>(json);

            }
            catch (WebException ex)
            {
                result.State = "0";
                result.Result = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.State = "0";
                result.Result = ex.Message;
            }
            return result;
        }



        /// <summary>
        /// zaj 20180621
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ResponseModelClone<string> HttpGetClone(string url, string data)
        {
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            try
            {
                string json = "";//返回json字符串

                WebResponse wr = null;
                HttpWebRequest req = null;
                req = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", url, data));
                req.Proxy = null;
                req.Method = "get";
                req.Timeout = 600000;

                wr = req.GetResponse();
                using (Stream ReceiveStream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(ReceiveStream))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    ReceiveStream.Close();
                    ReceiveStream.Dispose();
                }
                wr.Close();

                result = JsonConvert.DeserializeObject<ResponseModelClone<string>>(json);
            }
            catch (WebException ex)
            {
                result.State = "0";
                result.Result = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.State = "0";
                result.Result = ex.Message;
            }
            return result;
        }
        #endregion

        #region zaj 2017-11-4
        public static ResponseModelClone<string> HttpPost(string data, string url)
        {
            //string url = domain + UrlPage;
            ResponseModelClone<string> result = new ResponseModelClone<string>();
            try
            {
                string json = "";//返回json字符串

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/json";
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
                result = JsonConvert.DeserializeObject<ResponseModelClone<string>>(json);
            }
            catch (WebException ex)
            {
                result.State = "0";
                result.Message = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.State = "0";
                result.Message = ex.Message;
            }
            return result;
        }
        #endregion 

        /// <summary>
        /// 检测是否客户端需要重启
        /// QSP_GET_ARG过程中改回重启状态IsRestart=null
        /// </summary>
        /// <returns></returns>
        public static bool HttpPostRestart()
        {
            string url = RestartUrl;
            SqlResult result = new SqlResult();
            try
            {
                string json = "";//返回json字符串

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 600000;

                string data = "companyid=" + CommonClass.UserInfo.companyid + "&userid=" + CommonClass.UserInfo.UserAccount;
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
            }
            catch (Exception ex)
            {
                result.State = 0;
                result.Result = ex.Message;
            }
            int IsRestart = 0;
            if (result.State == 1 && int.TryParse(result.Result, out IsRestart) && IsRestart == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 提交流程信息到OA
        /// </summary>
        /// <param name="data">XML</param>
        /// <returns></returns>
        public static string HttpPostOA(string data)
        {
            string json = "";//返回json字符串：  {msgid:160001154,msg:"操作成功"}
            string url = OAUrl;
            string msg = "";//返回结果
            PostOAResult result = new PostOAResult();
            HttpWebRequest req;
            HttpWebResponse wr;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = 600000;

                data = "arg1=" + data;
                byte[] btbody = Encoding.UTF8.GetBytes(data);
                req.ContentLength = btbody.Length;
                using (Stream st = req.GetRequestStream())
                {
                    st.Write(btbody, 0, btbody.Length);
                    st.Close();
                    st.Dispose();
                }

                wr = (HttpWebResponse)req.GetResponse();
                using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                {
                    json = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                }
                wr.Close();
                result = JsonConvert.DeserializeObject<PostOAResult>(json);
                if (result.msg.Contains("成功"))
                {
                    msg = "";
                }
                else
                {
                    msg = result.msg;
                }
            }
            catch (WebException ex)
            {
                wr = (HttpWebResponse)ex.Response;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static SqlResult HttpGet(string url, string data)
        {
            SqlResult result = new SqlResult();
            try
            {
                string json = "";//返回json字符串

                WebResponse wr = null;
                HttpWebRequest req = null;
                req = (HttpWebRequest)WebRequest.Create(string.Format("{0}?{1}", url, data));
                req.Proxy = null;
                req.Method = "get";
                req.Timeout = 600000;

                wr = req.GetResponse();
                using (Stream ReceiveStream = wr.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(ReceiveStream))
                    {
                        json = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    ReceiveStream.Close();
                    ReceiveStream.Dispose();
                }
                wr.Close();

                result = JsonConvert.DeserializeObject<SqlResult>(json);
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

        //#region hj20180423
        static string domain_ZQTMS = "http://8.129.7.49:8024/";//ZQTMS正式库连接推送给外省服务
        public static string UrlPage_ZQTMS = "kms.ashx";

      

 

        public static SqlResult HttpPost_ZQTMS(string data)
        {
            string url = domain_ZQTMS + UrlPage_ZQTMS;
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

       
        /// <summary>
        /// Post请求  maohui20180717
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        public static SqlResult HttpPost_ZQTMS_Execte(string data)
        {
            string url = domain_ZQTMS + UrlPage_ZQTMS;
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


        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        public static string HttpPostJava(string data, string url)
        {
            string json = "";//返回json字符串               
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Proxy = null;
                req.Method = "post";
                //req.ContentType = "application/x-www-form-urlencoded";
                req.ContentType = "application/json";
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
                //result = JsonConvert.DeserializeObject<ApiResult>(json);

            }
            catch (WebException ex)
            {
                json = "远程访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            return json;
        }

        //获取数据库连接地址
        public static void GetDomaim(ref string strDomaim)
        {
            strDomaim = domaimNew;
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
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

       
    }
}
