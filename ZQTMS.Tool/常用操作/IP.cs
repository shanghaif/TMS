using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Management;

namespace ZQTMS.Tool
{
    public static class IP
    {
        private static string _webIP;

        /// <summary>
        /// 终端用户外网IP
        /// </summary>
        public static string WebIP
        {
            get 
            {
                if (string.IsNullOrEmpty(_webIP))
                {
                    GetPulicAddress();
                }
                return _webIP;
            }
            set
            {
                _webIP = value;
            }
        }

        private static string _webAddr;
        /// <summary>
        /// 终端用户外网地址
        /// </summary>
        public static string WebAddr
        {
            get
            {
                if (string.IsNullOrEmpty(_webAddr))
                {
                    GetPulicAddress();
                }
                return _webAddr;
            }
            set
            {
                _webAddr = value;
            }
        }

        private static string _hostName;

        /// <summary>
        /// 终端用户电脑名称
        /// </summary>
        public static string HostName
        {
            get
            {
                if (string.IsNullOrEmpty(_hostName))
                {
                    GetLocalIp();
                }
                return _hostName;
            }
            set
            {
                _hostName = value;
            }
        }

        private static string _mac;

        /// <summary>
        /// 终端用户电脑MAC
        /// </summary>
        public static string Mac
        {
            get
            {
                if (string.IsNullOrEmpty(_mac))
                {
                    GetLocalIp();
                }
                return _mac;
            }
            set
            {
                _mac = value;
            }
        }

        private static string _localIP;

        /// <summary>
        /// 终端用户电脑局域网IP
        /// </summary>
        public static string LocalIP
        {
            get
            {
                if (string.IsNullOrEmpty(_localIP))
                {
                    GetLocalIp();
                }
                return _localIP;
            }
            set
            {
                _localIP = value;
            }
        }

        /// <summary>
        /// 获取客户端外网IP
        /// </summary>
        /// <returns></returns>
        private static void GetPulicAddress()
        {
            //http://www.3322.org/dyndns/getip
            string s = "";
            try
            {
                string url = "http://www.ip138.com/ips138.asp";

                WebClient wc = new WebClient();
                s = wc.DownloadString(url);
                string findstring = "您的IP地址是：[";
                int p1 = s.IndexOf(findstring);
                s = s.Substring(p1 + findstring.Length, 100);

                findstring = "<br/>";
                p1 = s.IndexOf(findstring);
                s = s.Substring(0, p1);
                s = s.Replace("]", "");
                s = s.Replace(" 来自：", "|");
                string[] arr = s.Split('|');

                _webIP = arr[0];
                if (arr.Length > 1)
                {
                    _webAddr = arr[1];
                }
            }
            catch (Exception)
            {
            }
        }


        private static void GetLocalIp()
        {
            try
            {
                _hostName = Dns.GetHostName().ToString();
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection queryCollection = query.Get();
                string[] ips;
                string sname = "";
                foreach (ManagementObject mo in queryCollection)
                {
                    if (!(bool)mo["IPEnabled"]) continue;
                    sname = mo["ServiceName"] as string;

                    if (sname.ToLower().Contains("vmnetadapter") || sname.ToLower().Contains("ppoe") || sname.ToLower().Contains("bthpan") || sname.ToLower().Contains("tapvpn") || sname.ToLower().Contains("ndisip") || sname.ToLower().Contains("sinforvnic"))
                    { 
                        continue; 
                    }

                    ips = mo["IPAddress"] as string[];
                    IPAddress addr;
                    foreach (string item in ips)
                    {
                        if (item.StartsWith("127")) continue;
                        addr = IPAddress.Parse(item);
                        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            _localIP = item;
                            _mac = mo["MacAddress"].ToString();
                            return;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
