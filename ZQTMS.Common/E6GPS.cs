using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using ZQTMS.Tool;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace ZQTMS.Common
{

    public static class E6GPS
    {
        static string url = "http://api.e6gps.com/public/v3/";
        static string appkey = "4875cd75-62eb-49e5-8695-9462a4a24be5";//公钥
        static string appsecret = "4f9795c9-200a-4d60-9d92-1edceecdd707"; //密钥
        static string format = "json";
        //测试车号：赣CB2800

        /// <summary>
        /// 获取车辆位置
        /// </summary>
        /// <param name="Vno">需要查询的车牌,支持多车查询，车牌之间用英文逗号分割，-1表示全部车辆</param>
        public static List<VehicleModel> GetVehiclePosition(string Vno)
        {
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add("method", "GetVehcileInfo");
            list.Add("appkey", appkey);
            list.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            list.Add("format", format);
            list.Add("vehicle", Vno);//-1：全部车辆
            list.Add("sessionid", "");
            list.Add("isoffsetlonlat", "0");
            string sign = GetSign(list);
            list.Add("sign", sign);

            HttpResult result = HttpGet("Inface/Call", list);
            if (result.Code == 0)
            {
                MsgBox.ShowOK(result.Message);
                return null;
            }
            E6VehiclePosition pos = JsonConvert.DeserializeObject<E6VehiclePosition>(result.Json);
            if (pos == null)
            {
                return null;
            }
            if (pos.result.code != 1)
            {
                MsgBox.ShowError(pos.result.message);
                return null;
            }
            List<VehicleModel> listResult = pos.result.data;

            return listResult;
        }

        /// <summary>
        /// 地图显示获取车辆位置
        /// </summary>
        /// <param name="Vno">需要查询的车牌,支持多车查询，车牌之间用英文逗号分割，-1表示全部车辆</param>
        public static void GetVehiclePositionMap(string Vno)
        {
            try
            {
                SortedList<string, string> list = new SortedList<string, string>();
                list.Add("appkey", appkey);
                list.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                list.Add("vehicle", Vno);//-1：全部车辆
                string sign = GetSign(list);
                list.Remove("vehicle");
                list.Add("vehicle", System.Web.HttpUtility.UrlEncode(Vno));

                list.Add("sign", sign);

                string data = GetParaStr(list);
                string mapurl = string.Format("{0}MapServices/Orientation.aspx?{1}", url, data);

                System.Diagnostics.Process.Start(mapurl);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 指定目的地周围X公里范围内的车辆
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <param name="distance">公里数</param>
        /// <returns></returns>
        public static List<VehicleModel> GetCircularVehicles(decimal lon, decimal lat, int distance)
        {
            distance = distance * 1000;
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add("method", "GetVehcileInfoWithPoint");
            list.Add("appkey", appkey);
            list.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            list.Add("format", format);
            list.Add("lon", lon.ToString()); //经度
            list.Add("lat", lat.ToString()); //纬度
            list.Add("distance", distance.ToString());//公里范围内
            string sign = GetSign(list);
            list.Add("sign", sign);

            HttpResult result = HttpGet("Inface/Call", list);
            if (result.Code == 0)
            {
                MsgBox.ShowOK(result.Message);
                return null;
            }
            CircularVehicles pos = JsonConvert.DeserializeObject<CircularVehicles>(result.Json);
            if (pos == null)
            {
                return null;
            }
            if (pos.result.code != 1)
            {
                //MsgBox.ShowError(pos.result.message);
                return null;
            }
            List<VehicleModel> listResult = pos.result.data;
            
            return listResult;
        }

        /// <summary>
        /// 返回两个地址之间的距离和耗时(秒)
        /// </summary>
        /// <param name="addr1">起始地址</param>
        /// <param name="addr2">目的地址</param>
        /// <param name="Distance">返回参数：距离(km)</param>
        /// <param name="Duration">返回参数：预计耗时(s)</param>
        /// <returns></returns>
        public static bool GetDistance(string addr1, string addr2, ref decimal Distance, ref Int64 Duration)
        {
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add("method", "GetDriveInfoByPlaceName");
            list.Add("appkey", appkey);
            list.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            list.Add("format", format);
            list.Add("startplacename", addr1);
            list.Add("endplacename", addr2);
            string sign = GetSign(list);
            list.Add("sign", sign);

            HttpResult result = HttpGet("StatisticsReport/Call", list);
            if (result.Code == 0)
            {
                MsgBox.ShowOK(result.Message);
                return false;
            }
            // {"result":{"code":"1","message":"","data":[{"SLon":114.347696,"SLat":22.657462,"ELon":114.347696,"ELat":22.657462,"Distance":0.0,"Duration":0}]}}
            JObject obj = JsonConvert.DeserializeObject(result.Json) as JObject;
            int code = Convert.ToInt32(obj.SelectToken("result.code"));
            if (code != 1)
            {
                object msg = obj.SelectToken("result.message");
                if (msg != null)
                {
                    MsgBox.ShowError(msg.ToString());
                }
                return false;
            }

            JArray arr = obj.SelectToken("result.data") as JArray;
            if (arr.Count == 0)
            {
                return false;
            }
            Distance = Convert.ToDecimal(arr[0].SelectToken("Distance"));
            Duration = Convert.ToInt64(arr[0].SelectToken("Duration"));
            return true;
        }

        //http://login.e6gps.com/Home/LoginWithToken?sid=unc80sDeNsc60szdbdf1+mN7dnvgxFEh85xp4H\/T2hE\/yctqC636z8RWU1\/ZgfEfBXP34bcyAUE=&url=http://webgis.e6gps.com/Setting/E6User/SelE6User
        public static string GetLoginStr()
        {
            SortedList<string, string> list = new SortedList<string, string>();
            list.Add("method", "GetLoginStr");
            list.Add("appkey", appkey);
            list.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            list.Add("format", format);
            string sign = GetSign(list);
            list.Add("sign", sign);

            HttpResult result = HttpGet("Inface/Call", list);
            if (result.Code == 0)
            {
                MsgBox.ShowOK(result.Message);
                return "";
            }
            JObject obj = JsonConvert.DeserializeObject(result.Json) as JObject;
            int code = Convert.ToInt32(obj.SelectToken("result.code"));
            if (code != 1)
            {
                object msg = obj.SelectToken("result.message");
                if (msg != null)
                {
                    MsgBox.ShowError(msg.ToString());
                }
                return "";
            }

            JArray arr = obj.SelectToken("result.data") as JArray;
            if (arr.Count == 0)
            {
                return "";
            }
            string LoginKey = arr[0].SelectToken("LoginKey").ToString();
            return LoginKey;
        }

        private static void ListSort(List<VehicleModel> list, string field, string rule)
        {
            if (string.IsNullOrEmpty(rule) || !rule.ToLower().Equals("desc") || !rule.ToLower().Equals("asc"))
            {
                return;
            }
            try
            {
                list.Sort(
                    delegate(VehicleModel info1, VehicleModel info2)
                    {
                        Type t = typeof(VehicleModel);
                        PropertyInfo pro = t.GetProperty(field);
                        return rule.ToLower().Equals("asc") ?
                            pro.GetValue(info1, null).ToString().CompareTo(pro.GetValue(info2, null).ToString()) :
                            pro.GetValue(info2, null).ToString().CompareTo(pro.GetValue(info1, null).ToString());
                    });
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("车辆资料排序失败：" + ex.Message);
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="data">键值对，比如：json="...."</param>
        /// <returns></returns>
        private static HttpResult HttpGet(string fun, SortedList<string, string> list)
        {
            HttpResult result = new HttpResult();
            try
            {
                string json = "";//返回json字符串
                string data = GetParaStr(list);

                WebResponse wr = null;
                HttpWebRequest req = null;
                req = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}?{2}", url, fun, data));
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

                result.Code = 1;
                result.Json = json;
            }
            catch (WebException ex)
            {
                result.Code = 0;
                result.Message = "GPS接口访问错误：\r\n" + ex.Message;
            }
            catch (Exception ex)
            {
                result.Code = 0;
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 根据传入参数名称（签名sign除外）将所有请求参数按照首字母先后顺序排序，md5（密钥+除密钥外排好序的参数串+密钥）后转换为大写字母
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string GetSign(SortedList<string, string> list)
        {
            string sign = "";
            foreach (KeyValuePair<string, string> item in list)
            {
                sign += string.Format("{0}{1}", item.Key, item.Value);
            }
            string md5 = EncryptUtils.MD5Encrypt(appsecret + sign + appsecret);
            return md5;
        }

        private static string GetParaStr(SortedList<string, string> list)
        {
            string data = "";
            foreach (KeyValuePair<string, string> item in list)
            {
                data += string.Format("{0}={1}&", item.Key, item.Value);
            }
            return data.TrimEnd('&');
        }

        private static string GetMsgByCode(int code)
        {
            string msg = "";
            switch (code)
            {
                case 1:
                    msg = "正确结果";
                    break;
                case 5:
                    msg = "参数都正确，没有数据";
                    break;
                case 11:
                    msg = "用户没有访问api的权限";
                    break;
                case 21:
                    msg = "用户请求未传入method（请求的api名称）参数";
                    break;
                case 22:
                    msg = "用户请求的method找不到相应的api与之对应";
                    break;
                case 24:
                    msg = "用户请求未传入sign（请求签名）参数";
                    break;
                case 25:
                    msg = "用户请求传入的sign参数验证错误。服务端用传入的参数按照约定方法签名得到的值与传入的sign参数不一样";
                    break;
                case 28:
                    msg = "用户请求未传入appkey（应用标记key）参数";
                    break;
                case 29:
                    msg = "用户请求传入的appkey找不到对应的应用";
                    break;
                case 30:
                    msg = "用户请求未传入timestamp（时间戳）参数";
                    break;
                case 31:
                    msg = "用户请求传入的timestamp参数不是合法的格式";
                    break;
                case 32:
                    msg = "时间戳不在规定范围内";
                    break;
                case 40:
                    msg = "少必传参数XXX（XXX为参数名字）";
                    break;
                case 41:
                    msg = "传入的参数格式错误";
                    break;
                case 42:
                    msg = "";
                    break;
                case 52:
                    msg = "服务端执行异常";
                    break;
                case 503:
                    msg = "当前ip并发请求数过高造成服务器ddos防攻击控制报错。";
                    break;
                default:
                    break;
            }

            return "错误代码：" + code + "，错误内容：" + msg;
        }
    }

    #region 车辆位置实体
    /// <summary>
    /// 车辆实体对象
    /// </summary>
    public class VehicleModel
    {
        /// <summary>
        /// 车牌
        /// </summary>
        public string Vehicle { get; set; }

        /// <summary>
        /// 定位时间
        /// </summary>
        public string GPSTime { get; set; }

        /// <summary>
        /// 车速
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 里程
        /// </summary>
        public double Odometer { get; set; }

        /// <summary>
        /// 纬度(WGS-84坐标系)
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 经度(WGS-84坐标系)
        /// </summary>
        public double Lon { get; set; }

        /// <summary>
        /// 车头方向
        /// </summary>
        public int Direction { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 车辆位置
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Provice { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 路名信息
        /// </summary>
        public string RoadName { get; set; }

        ///// <summary>
        ///// 车辆ID
        ///// </summary>
        //public int VehicleID { get; set; }

        //public string T1 { get; set; }
        //public string T2 { get; set; }
        //public string T3 { get; set; }
        //public string T4 { get; set; }
        //public double Lat02 { get; set; }
        //public double Lon02 { get; set; }

        /// <summary>
        /// 地标名称
        /// </summary>
        public string AreaName { get; set; }
    }

    /// <summary>
    /// 结果集实体
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 调用接口返回结果1:成功 非1:错误
        /// </summary>
        public int code { get; set; }

        public string message { get; set; }

        public List<VehicleModel> data { get; set; }
    }

    /// <summary>
    /// 车辆位置信息返回实体
    /// </summary>
    public class E6VehiclePosition
    {
        public ResultModel result { get; set; }
    }
    #endregion

    /// <summary>
    /// 圆形范围内的车辆
    /// </summary>
    public class CircularVehicles
    {
        public ResultModel result { get; set; }
    }

    /// <summary>
    /// HTTP访问结果
    /// </summary>
    public class HttpResult
    {
        /// <summary>
        /// 是否正常访问：0异常  1正常
        /// </summary>
        public int Code
        {
            get;
            set;
        }

        /// <summary>
        /// code=0的时候，返回异常原因
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// code=1的时候返回调用结果的json
        /// </summary>
        public string Json
        {
            get;
            set;
        }
    }
}
