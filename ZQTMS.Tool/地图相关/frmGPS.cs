using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace ZQTMS.Tool
{
    public class location
    {
        private float _lat;

        public float lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        private float _lng;

        public float lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
    }

    public class result
    {
        private location _location;

        public location location
        {
            get { return _location; }
            set { _location = value; }
        }

        private int _precise;

        public int precise
        {
            get { return _precise; }
            set { _precise = value; }
        }
        private int _confidence;

        public int confidence
        {
            get { return _confidence; }
            set { _confidence = value; }
        }
        private string _level;

        public string level
        {
            get { return _level; }
            set { _level = value; }
        }
    }

    public class apimodel
    {
        private int _status;

        public int status
        {
            get { return _status; }
            set { _status = value; }
        }
        private result _result;

        public result result
        {
            get { return _result; }
            set { _result = value; }
        }
    }

    public class frmGPS
    {
        public frmGPS() { }

        public static void GetGPS(string address, ref float lat, ref float lng)
        {
            apimodel am = GetLSApi<apimodel>(address);
            if (am == null || am.result == null) return;

            lat = am.result.location.lat;
            lng = am.result.location.lng;
        }

        private static T GetLSApi<T>(string address)
        {
            string apiUrl = "http://api.map.baidu.com/geocoder/v2/";
            //string ak = "E4805d16520de693a3fe707cdc962045";
            string apiKey = "2ubeWNitmZNh6qB4N1CE4XSL";

            string param = string.Format("ak={0}&output=json&address={1}", apiKey, address);

            //初始化方案信息实体类。
            T info = default(T);
            string result = string.Empty;
            try
            {
                //以 Get 形式请求 Api 地址
                result = PostPage(apiUrl, param);
                //info = JsonHelper.FromJsonTo<T>(result);
                info = JsonConvert.DeserializeObject<T>(result);
            }
            catch
            {
                return default(T);
            }

            return info;
        }

        /// <summary>
        /// 发送 API 请求并返回方案信息。
        /// </summary>
        /// <returns></returns>
        private static T RequestApi<T>(string address)
        {
            string apiUrl = "http://api.map.baidu.com/geocoder/v2/";
            //string ak = "E4805d16520de693a3fe707cdc962045";
            string apiKey = "2ubeWNitmZNh6qB4N1CE4XSL";

            string param = string.Format("ak={0}&output=json&address={1}", apiKey);

            string result = string.Empty;

            //初始化方案信息实体类。
            T info = default(T);
            try
            {
                //以 Post 形式请求 Api 地址
                result = PostPage(apiUrl, param);
                //info = JsonHelper.FromJsonTo<T>(result);
                info = JsonConvert.DeserializeObject<T>(result);
            }
            catch (Exception)
            {
                info = default(T);
                throw;
            }

            return info;
        }

        private static string PostPage(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }
    }
}