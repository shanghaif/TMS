using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.Update
{
    /// <summary>
    /// 进度条变化事件委托
    /// </summary>
    /// <param name="num">The number.</param>
    /// <param name="FullSize">The full size.</param>
    /// <param name="CurrentBytes">The current bytes.</param>
    public delegate void UpdateDelegate(int num, long FullSize, long CurrentBytes);

    /// <summary>
    /// Class HttpHelper.
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 委托事件
        /// </summary>
        public event UpdateDelegate UpdateStatusEvent;
        /// <summary>
        /// 变化值的控件
        /// </summary>
        private Control ParentFormRef;

        /// <summary>
        /// 构造 <see cref="HttpHelper"/> 类.
        /// </summary>
        /// <param name="formRef">值变化控件.</param>
        public HttpHelper(Control formRef)
        {
            ParentFormRef = formRef;
        }

        /// <summary>
        /// 委托变化事件
        /// </summary>
        /// <param name="num">位数</param>
        /// <param name="FullSize">总长度</param>
        /// <param name="CurrentBytes">当前内容字节</param>
        private void SendMessageToMainThread(int num, long FullSize, long CurrentBytes)
        {
            try
            {
                if (UpdateStatusEvent != null)
                {
                    object[] obArr = new object[] { num, FullSize, CurrentBytes };

                    if (ParentFormRef != null)
                    {
                        ParentFormRef.Invoke(UpdateStatusEvent, obArr);
                    }
                    else
                    {
                        UpdateStatusEvent(num, FullSize, CurrentBytes);
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// 传输方法
        /// </summary>
        /// <param name="url">下载路径</param>
        /// <param name="fs">文件流</param>
        /// <param name="n">位数</param>
        /// <param name="User">用户名</param>
        /// <param name="Pwd">密码</param>
        /// <returns><c>true</c> 如下载成功, <c>false</c> 如下载失败.</returns>
        /// <exception cref="System.Net.WebException">网络错误</exception>
        /// <exception cref="System.Exception">一般错误</exception>
        public bool TransData(string url, FileStream fs, int n, String User, String Pwd)
        {
            HttpWebRequest request = null;
            bool result = false;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "application/x-www-form-urlencoded";

                #region 加入用户名和密码
                if (!String.IsNullOrEmpty(User.Trim()))
                {
                    request.Credentials = CredentialCache.DefaultCredentials;

                    //获得用户名密码的Base64编码
                    string code = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", User, Pwd)));

                    //添加Authorization到HTTP头
                    request.Headers.Add("Authorization", "Basic " + code);
                }
                #endregion

                HttpWebResponse web = (HttpWebResponse)request.GetResponse();

                Stream strm = web.GetResponseStream();
                ///总长度
                long allSize = web.ContentLength;
                if (allSize == -1 || allSize == 0)
                {
                    allSize = 1;
                }
                int ArrSize = 10000;

                SendMessageToMainThread(n, allSize, 0);

                byte[] barr = new byte[ArrSize];
                long downSize = 0;
                while (true)
                {
                    int Result = strm.Read(barr, 0, ArrSize);
                    if (Result == -1 || Result == 0)
                    {
                        break;
                    }
                    fs.Write(barr, 0, Result);
                    downSize += long.Parse(Result.ToString());
                    SendMessageToMainThread(n, allSize, downSize);
                }
                result = true;
            }
            catch (WebException ex)
            {
                throw new WebException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            fs.Flush();
            fs.Close();
            return result;
        }
    }
}