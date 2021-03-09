using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ZQTMS.UpLoad
{
    public static class HttpHelper
    {
        static string url = "http://lms.dekuncn.com:8087/LeyService/CoreRun";
        //static string domaimNew = " http://lms.dekuncn.com:7016/";//WCF接口 zaj 2017-11-18
        static string domaimNew = "http://8.129.7.49:8013/";
        public static string UrlPageNew = "KDLMSService/Post_SystemRequestProt";//zaj 2017-11-18

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        public static SqlResult HttpGet(string data)
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
        //public static SqlResult HttpPost(string data)
        //{
        //    SqlResult result = new SqlResult();
        //    try
        //    {
        //        string json = "";//返回json字符串

        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //        req.Proxy = null;
        //        req.Method = "post";
        //        req.ContentType = "application/x-www-form-urlencoded";


        //        byte[] btbody = Encoding.UTF8.GetBytes(data);
        //        req.ContentLength = btbody.Length;
        //        using (Stream st = req.GetRequestStream())
        //        {
        //            st.Write(btbody, 0, btbody.Length);
        //            st.Close();
        //            st.Dispose();
        //        }

        //        WebResponse wr = (HttpWebResponse)req.GetResponse();
        //        using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
        //        {
        //            json = sr.ReadToEnd();
        //            sr.Close();
        //            sr.Dispose();
        //        }
        //        wr.Close();
        //        result = JsonConvert.DeserializeObject<SqlResult>(json);
        //    }
        //    catch (WebException ex)
        //    {
        //        result.State = 0;
        //        result.Result = "远程访问错误：\r\n" + ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.State = 0;
        //        result.Result = ex.Message;
        //    }
        //    return result;
        //}


        #region zaj 2017-11-17
        public static ResponseModelClone<string> HttpPost(string data)
        {
            string url = domaimNew + UrlPageNew;
            // string url = "http://localhost:42936/KDLMSService/Post_SystemRequestProt";
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
    }
}
