using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Net;
using System.Management;
using System.Threading;
using System.Xml;

namespace ZQTMS.UI
{
    public partial class frmLogin : BaseForm
    {
        bool AutoLogin = false;  //是否自动登录

        string  CPU = "", HardDiskID = "", Mac = ""; //cpu编号和硬盘编号

        public frmLogin(string LoginUser, string LoginPwd, int LoginState,string LoginMac)
        {
            InitializeComponent();
            if (Companyid.Text.Trim() == "")
            {
                Companyid.Text = API.ReadINI("Login", "Companyid", "", "Login");
            }
            if (!string.IsNullOrEmpty(LoginUser))
            {
                UserAccount.Text = LoginUser;
            }
            if (!string.IsNullOrEmpty(LoginPwd))
            {
                UserPsw.Text = LoginPwd;
            }
            if (!string.IsNullOrEmpty(LoginMac))
            {
                Mac = LoginMac;
            }
            AutoLogin = LoginState == 1;
        }

        /// <summary>
        /// 自动更新工具的文件名
        /// <para>ZQTMSUpdate.exe</para>
        /// </summary>
        string UpdaterName = "ZQTMSUpdate.exe";
        string WebPicMD5 = "";//主界面图片MD5
        string LastLoginSite = "";//上次登录网点

        public bool CanLogin = false;
        DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
        private Point MousePoint = new Point(0, 0);
        private int mc = 0;

        DataSet dsLogin = new DataSet();//登录验证信息

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            MsgLabel lb = new MsgLabel(lbmsg);
            lbmsg.Visible = true;

            try
            {
                #region 验证账号
                if (!ValidateLogin(lb))
                {
                    UserAccount.Focus();
                    lbmsg.Text = "准备就绪...";
                    lbmsg.Update();
                    return;
                }
                #endregion

                if (!login()) return; //登录
               

                DateTime dt = SqlHelper.GetServerDate();

                if (!SetBaseArg()) return;//给参数赋值
                #region 检测更新
                if (!Debugger.IsAttached)
                {
                    string updateJson = "";
                    //if (!CheckUpdate(lb, ref updateJson)) return;
                    if (!string.IsNullOrEmpty(updateJson))
                    {
                        string ftpUser = "", ftpPassword = "";
                        string mainName = Assembly.GetExecutingAssembly().GetModules(false)[0].Name;//主程序名字
                        string param = string.Format("{0}@##*@{1}@##*@{2}@##*@{3}@##*@{4}@##*@{5}@##*@{6}", updateJson, ftpUser, ftpPassword, UserAccount.Text.Trim(), UserPsw.Text.Trim(), mainName, Mac);
                        Process process = new Process();
                        process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, UpdaterName);
                        process.StartInfo.Arguments = param;
                        process.StartInfo.UseShellExecute = false;
                        process.Start();

                        Environment.Exit(0);
                        return;
                    }
                }
                #endregion

                
                CommonClass.getLastLoginCompanyid();//zaj 2018-5-25 获取上次登录系统的公司id zaj 2018-5-25

                //CheckWebPic(lb);//检测主界面图片

                if (!AddCacheTable(lb)) return;  //创建本地缓存表

                //检查是否为初始密码
                if (UserPsw.Text.Trim() == "zhongqiang666")
                {
                    frmUpdUserPass frm = new frmUpdUserPass();
                    frm.MustUpdate = true;
                    DialogResult dl = frm.ShowDialog();
                    if (dl != DialogResult.OK)
                    {
                        this.CanLogin = false;
                        return;
                    }
                }
                #region  创建本地xml文件保存基本信息 zaj 2018-5-28
                string fileName = Application.StartupPath + "\\lastLoainCompanid.xml";
                string companyid = "";
                if (File.Exists(fileName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileName);
                    XmlNode xn = doc.SelectSingleNode("companyid");
                    companyid = xn.InnerText;
                    if (companyid != CommonClass.UserInfo.companyid)
                    {
                        xn.InnerText = CommonClass.UserInfo.companyid;
                    }
                    doc.Save(fileName);

                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    XmlNode node = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                    doc.AppendChild(node);
                    XmlNode root = doc.CreateElement("companyid");
                    doc.AppendChild(root);
                    root.InnerText = CommonClass.UserInfo.companyid;
                    doc.Save(fileName);

                }

                #endregion

                GetBaseInfo(lb); //登录成功，加载信息
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
            finally
            {
                lbmsg.Visible = false;
            }
        }

        private bool AddCacheTable(IMsg splash) //创建本地缓存表
        {
            splash.UpdateMessage("正在创建本地缓存表...");
            SQLiteHelper helper = new SQLiteHelper();
            OperResult oper = helper.ExecuteNonQuery("create table if not exists sysLocalCacheTable(TableName varchar(200),ModifyDate datetime,CONSTRAINT [] PRIMARY KEY (TableName) ON CONFLICT REPLACE);");
            if (oper.State == 0)
            {
                MsgBox.ShowOK("本地缓存表创建失败：" + oper.Msg);
                return false;
            }
            return true;
        }

        private bool ValidateLogin(IMsg splash)
        {
            splash.UpdateMessage("正在验证用户名及密码...");
            if (Companyid.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入公司ID！");
                Companyid.Focus();
                return false;
            }
            if (UserAccount.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入账号！");
                UserAccount.Focus();
                return false;
            }
            if (UserPsw.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入密码！");
                UserPsw.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检测更新，返回需要更新的DataTable的json数据。如果为空，表示无更新
        /// </summary>
        /// <returns></returns>
        private bool CheckUpdate(IMsg splash, ref string updateJson)
        {
            splash.UpdateMessage("正在检测更新...");

            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FileUpdateFCD_Login", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                string filePath = "", fileName = "", appPath = "", md5 = "", fileUrl = "";

                DataTable dt = new DataTable();
                dt.Columns.Add("FILE_PATH", typeof(string)); //文件名
                dt.Columns.Add("AppPath", typeof(string)); //客户端存放目录
                dt.Columns.Add("DOWN_ADDRESS", typeof(string)); //下载url

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    fileName = ds.Tables[0].Rows[i]["FileName"].ToString();
                    appPath = ds.Tables[0].Rows[i]["AppPath"].ToString();
                    md5 = ds.Tables[0].Rows[i]["FileMD5"].ToString();
                    fileUrl = ds.Tables[0].Rows[i]["FileUrl"].ToString();

                    filePath = Path.Combine(Application.StartupPath, appPath) + "\\" + fileName;
                    if (!File.Exists(filePath)) //本地文件不存在：更新
                    {
                        dt.Rows.Add(fileName, appPath, fileUrl);
                        continue;
                    }
                    if (!Encrypt.MD5ForFile(filePath).Equals(md5, StringComparison.OrdinalIgnoreCase))
                    {
                        dt.Rows.Add(fileName, appPath, fileUrl);
                    }
                }
                dt.AcceptChanges();

                #region 更新ZQTMSUpdate.exe文件
                try
                {
                    DataRow[] drs = dt.Select(string.Format("FILE_PATH='{0}'", UpdaterName));
                    if (drs.Length > 0)
                    {
                        Process[] pros = Process.GetProcessesByName(UpdaterName.Replace(".exe", ""));
                        foreach (Process pro in pros)
                        {
                            pro.Kill();
                        }
                        splash.UpdateMessage("正在下载更新工具...");
                        using (WebClient wc = new WebClient())
                        {
                            wc.Proxy = null;
                            wc.DownloadFile(string.Format("http://8.129.7.49:8014/UpdateFile/{0}.rar", UpdaterName), UpdaterName + ".rar");
                            wc.Dispose();

                            string file = Path.Combine(Application.StartupPath, UpdaterName + ".rar");
                            CZip.DeCompress(file);

                            if (File.Exists(file))
                            {
                                File.Delete(file);
                            }
                        }

                        dt.Rows.Remove(drs[0]);
                        dt.AcceptChanges();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError("自动更新文件更新失败：" + ex.Message);
                    return false;
                }
                #endregion

                if (dt.Rows.Count > 0)
                {
                    string dtJson = JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, DateFormatString = "yyyy-MM-dd HH:mm:ss" });
                    byte[] bytes = Encoding.UTF8.GetBytes(dtJson);

                    updateJson = Convert.ToBase64String(bytes);
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("检测更新失败：" + ex.Message);
                return false;
            }
        }

        private bool CheckWebPic(IMsg splash)
        {
            splash.UpdateMessage("正在检测主界面图片...");

            try
            {
                string file = Path.Combine(Application.StartupPath, "mainweb.jpg");
                string md5 = "";
                if (File.Exists(file))
                {
                    md5 = Encrypt.MD5ForFile(file);
                }
                if (md5.Equals(WebPicMD5, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                using (WebClient wc = new WebClient())
                {
                    wc.Proxy = null;
                    wc.DownloadFile("http://ZQTMS.dekuncn.com:7011/WebPic/mainweb.jpg", "mainweb.jpg");
                    wc.Dispose();
                }
            }
            catch (Exception)
            {
            }
            return true;
        }

        private void GetBaseInfo(IMsg splash)
        {
            splash.UpdateMessage("正在加载系统参数...");
            bool result = CommonClass.QSP_GET_ARG();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            splash.UpdateMessage("正在加载权限...");
            result = UserRight.GetUserRightDataSet(CommonClass.UserInfo.GRCode);
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            splash.UpdateMessage("正在加载菜单...");
            result = CommonClass.GetMainMenuData();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            //splash.UpdateMessage("正在加载结算送货费...");
            //result = CommonClass.P_GetSeriesProductDetail();
            //if (!result)
            //{
            //    Application.Restart();
            //    Environment.Exit(0);
            //    return;
            //}
            result = CommonClass.P_GetSeriesProductDetail_HK();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            //splash.UpdateMessage("正在加载结算干线费...");
            //result = CommonClass.QSP_GET_BASMAINLINEFEE();
            //if (!result)
            //{
            //    Application.Restart();
            //    Environment.Exit(0);
            //    return;
            //}
            //splash.UpdateMessage("正在加载结算始发操作费...");
            //result = CommonClass.QSP_GET_BASDEPARTUREOPTFEE();
            //if (!result)
            //{
            //    Application.Restart();
            //    Environment.Exit(0);
            //    return;
            //}

            //2019.4.29 wbw 
            splash.UpdateMessage("正在加载转分拨平台结算始发操作费...");
            result = CommonClass.QSP_GET_BASDEPARTUREOPTFEE_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载转分拨平台结算终端操作费...");
            result = CommonClass.QSP_GET_BASTERMINALOPTFEE_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载转分拨平台结算干线费...");
            result = CommonClass.QSP_GET_BASMAINLINEFEE_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载转分拨平台结算送货费分段...");
            result = CommonClass.P_GetSeriesProductDetail_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载转分拨平台结算中转费...");
            result = CommonClass.QSP_GET_BASTRANSFERFEE_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }






            splash.UpdateMessage("正在加载结算始发分拔费...");
            result = CommonClass.QSP_GET_BASDEPARTUREALLOTFEE();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            splash.UpdateMessage("正在加载转分拨平台结算始发分拔费...");
            result = CommonClass.QSP_GET_BASDEPARTUREALLOTFEE_PT();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            splash.UpdateMessage("正在加载支线始发分拨费...");
            result = CommonClass.QSP_GET_BASDEPARTUREALLOTFEE_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            //splash.UpdateMessage("正在加载结算中转费...");
            //result = CommonClass.QSP_GET_BASTRANSFERFEE();
            //if (!result)
            //{
            //    Application.Restart();
            //    Environment.Exit(0);
            //    return;
            //}
            //splash.UpdateMessage("正在加载结算终端操作费...");
            //result = CommonClass.QSP_GET_BASTERMINALOPTFEE();
            //if (!result)
            //{
            //    Application.Restart();
            //    Environment.Exit(0);
            //    return;
            //}
            splash.UpdateMessage("正在加载结算终端分拨费...");
            result = CommonClass.QSP_GET_BASTERMINALALLOTFEE();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            #region zaj 加载支线结算费用
            splash.UpdateMessage("正在加载支线结算终端干线费...");
            result = CommonClass.QSP_GET_BASMAINLINEFEE_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载支线结算始发操作费...");
            result = CommonClass.QSP_GET_BASDEPARTUREOPTFEE_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载支线结算中转费...");
            result = CommonClass.QSP_GET_BASTRANSFERFEE_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载支线结算终端操作费...");
            result = CommonClass.QSP_GET_BASTERMINALOPTFEE_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载支线结算送货费...");
            result = CommonClass.P_GetSeriesProductDetail_ZX();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            #endregion
            splash.UpdateMessage("正在加载站点资料...");
            result = CommonClass.SiteAll();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载网点资料...");
            result = CommonClass.getWeb();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载大区资料...");
            result = CommonClass.AreaAll();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载事业部资料...");
            result = CommonClass.CauseAll();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }

            splash.UpdateMessage("正在加载部门资料...");
            result = CommonClass.DepAll();
            if (!result)
            {
                Application.Restart();
                Environment.Exit(0);
                return;
            }
            splash.UpdateMessage("正在加载基本信息...");
            CommonClass.Init();
        }

        private bool login()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserCompanyId", Companyid.Text.Trim()));
                list.Add(new SqlPara("UserAccount", UserAccount.Text.Trim()));
                list.Add(new SqlPara("UserPsw", StringHelper.Md5Hash(UserPsw.Text.Trim())));
                //yzw mac
                list.Add(new SqlPara("Mac", Mac));
               // SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_LOGIN", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_LOGIN", list);//zaj

                dsLogin = SqlHelper.GetDataSet(sps);

                if (dsLogin == null)
                {
                    return false;
                }
                if (dsLogin.Tables.Count == 0 || dsLogin.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("账号或密码不正确！");
                    lbmsg.Text = "准备就绪...";
                    lbmsg.Update();
                    return false;
                }
                if (dsLogin.Tables[0].Columns.Count == 1) //此时登录的账号不存在，只有一个标识列
                {
                    MsgBox.ShowOK("账号不存在！");
                    lbmsg.Text = "准备就绪...";
                    lbmsg.Update();
                    return false;
                }

                if (dsLogin.Tables[0].Rows.Count == 0) //此时表示登录的账号存在，但是密码不对
                {
                    MsgBox.ShowOK("密码不正确！");
                    lbmsg.Text = "准备就绪...";
                    lbmsg.Update();
                    return false;
                }

                #region 登录验证信息
                if (!Debugger.IsAttached)
                {
                    int EnableValidate = dsLogin.Tables[0].Rows[0]["EnableValidate"] == DBNull.Value ? 0 : Convert.ToInt32(dsLogin.Tables[0].Rows[0]["EnableValidate"]);
                    if (EnableValidate == 1)
                    {
                        if (CPU == "" || HardDiskID == "")
                        {
                            MsgBox.ShowOK("本机验证信息没有获取完毕，请稍后10秒钟！");

                            #region 获取电脑信息
                            Thread th = new Thread(() =>
                            {
                                GetComputerInfo();
                            });
                            th.IsBackground = true;
                            th.Start();
                            #endregion

                            return false;
                        }

                        string info = dsLogin.Tables[0].Rows[0]["ValidationInfo"] == DBNull.Value ? "" : dsLogin.Tables[0].Rows[0]["ValidationInfo"].ToString();
                        string md5 = StringHelper.Md5Hash(CPU + HardDiskID).ToUpper();
                        info = info.ToUpper();
                        if (info == "")
                        {//说明数据库中没有验证信息，要展示出来，发给信息部

                            frmComputerInfo frm = new frmComputerInfo();
                            frm.md5 = md5;
                            frm.useraccount = UserAccount.Text.Trim();
                            frm.ShowDialog();

                            lbmsg.Text = "准备就绪...";
                            lbmsg.Update();
                            return false;
                        }
                        if (info != md5 && !info.Contains(md5))
                        {
                            MsgBox.ShowOK("您的登录信息和服务端不一致，无法登录，请联系信息部！");
                            frmComputerInfo frm = new frmComputerInfo();//ljp 登录信息不一致需要显示注册信息
                            frm.md5 = md5;
                            frm.useraccount = UserAccount.Text.Trim();
                            frm.ShowDialog();
                            lbmsg.Text = "准备就绪...";
                            lbmsg.Update();
                            return false;
                        }
                    }
                }

                #endregion

                #region 保存登录信息
                API.WriteINI("Login", "Companyid", Companyid.Text.Trim(), "Login");
                API.WriteINI("Login", "UserAccount", UserAccount.Text.Trim(), "Login");
                if (Debugger.IsAttached)
                {
                    API.WriteINI("Login", "UserPsw", UserPsw.Text.Trim(), "Login");
                }
                API.WriteINI("Login", "LastLoginSite", dsLogin.Tables[0].Rows[0]["SiteName"].ToString(), "Login");
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 给系统参数赋值
        /// </summary>
        /// <returns></returns>
        private bool SetBaseArg()
        {
            try
            {
                int UserState = dsLogin.Tables[0].Rows[0]["UserState"] == DBNull.Value ? 0 : Convert.ToInt32(dsLogin.Tables[0].Rows[0]["UserState"]);
                if (UserState == 0)
                {
                    MsgBox.ShowOK("当前账号已禁用，请联系信息管理中心！");
                    lbmsg.Text = "准备就绪...";
                    lbmsg.Update();
                    return false;
                }

                sysUserInfoInfo UserInfo = new sysUserInfoInfo();
                UserInfo.UserId = new Guid(dsLogin.Tables[0].Rows[0]["UserId"].ToString());
                UserInfo.UserAccount = dsLogin.Tables[0].Rows[0]["UserAccount"].ToString();
                UserInfo.UserPsw = dsLogin.Tables[0].Rows[0]["UserPsw"].ToString();
                UserInfo.UserName = dsLogin.Tables[0].Rows[0]["UserName"].ToString();
                UserInfo.SiteName = dsLogin.Tables[0].Rows[0]["SiteName"].ToString();
                UserInfo.WebName = dsLogin.Tables[0].Rows[0]["WebName"].ToString();
                UserInfo.CauseName = dsLogin.Tables[0].Rows[0]["CauseName"].ToString();
                UserInfo.AreaName = dsLogin.Tables[0].Rows[0]["AreaName"].ToString();
                UserInfo.DepartName = dsLogin.Tables[0].Rows[0]["DepartName"].ToString();
                UserInfo.GRCode = dsLogin.Tables[0].Rows[0]["GRCode"].ToString();
                UserInfo.GRName = dsLogin.Tables[0].Rows[0]["GRName"].ToString();
                UserInfo.LoginSiteCode = dsLogin.Tables[0].Rows[0]["LoginSiteCode"].ToString();
                UserInfo.LoginWebCode = dsLogin.Tables[0].Rows[0]["LoginWebCode"].ToString();
                UserInfo.WebRole = dsLogin.Tables[0].Rows[0]["WebRole"].ToString();
                UserInfo.UserDB = (UserDB)Enum.Parse(typeof(UserDB), dsLogin.Tables[0].Rows[0]["UserDB"].ToString());
                UserInfo.Discount = ConvertType.ToDecimal(dsLogin.Tables[0].Rows[0]["Discount"].ToString());
                UserInfo.companyid = ConvertType.ToString(dsLogin.Tables[0].Rows[0]["companyid"]);
                UserInfo.gsqc = dsLogin.Tables[0].Rows[0]["gsjc"].ToString();
                //zaj 2017-11-23
                UserInfo.LabelName = dsLogin.Tables[0].Rows[0]["LabelName"].ToString();
                UserInfo.EnvelopeName = dsLogin.Tables[0].Rows[0]["EnvelopeName"].ToString();
                UserInfo.IsAutoBill = Convert.ToInt32(dsLogin.Tables[0].Rows[0]["IsAutoBill"].ToString()) == 0 ? false : true;
                UserInfo.IsLimitMonthPay = Convert.ToInt32(dsLogin.Tables[0].Rows[0]["IsLimitMonthPay"].ToString())==0?false:true;

                UserInfo.Transprotocol = dsLogin.Tables[0].Rows[0]["Transprotocol"].ToString();
                UserInfo.DepartList = dsLogin.Tables[0].Rows[0]["DepartList"].ToString();
                UserInfo.LoadList = dsLogin.Tables[0].Rows[0]["LoadList"].ToString();
                UserInfo.ShortConList = dsLogin.Tables[0].Rows[0]["ShortConList"].ToString();
                UserInfo.BookNote = dsLogin.Tables[0].Rows[0]["BookNote"].ToString();
                UserInfo.MiddleList = dsLogin.Tables[0].Rows[0]["MiddleList"].ToString();//maohui20180324
                UserInfo.token = dsLogin.Tables[0].Rows[0]["token"].ToString();//E3PLtoken

                CommonClass.UserInfo = UserInfo;

                CommonClass.ServerDate = Convert.ToDateTime(dsLogin.Tables[0].Rows[0]["sdate"]);

                DataRow[] rows = CommonClass.GetDatabaseInfo().Select(string.Format("db='{0}'", dsLogin.Tables[0].Rows[0]["UserDB"]));
                if (rows.Length > 0)
                {
                    HttpHelper.UrlPage = rows[0]["url"].ToString();
                    SQLiteHelper.LocalDB = rows[0]["localdb"].ToString();
                }
                WebPicMD5 = dsLogin.Tables[0].Rows[0]["WebPicMD5"] == DBNull.Value ? "" : dsLogin.Tables[0].Rows[0]["WebPicMD5"].ToString();

                #region 检测本次登录的站点和上次登录的站点是否一致
                try
                {
                    if (!LastLoginSite.Equals(UserInfo.SiteName)) //本次登录的站点和上次登录的站点不一致，就删除本地的缓存数据库
                    {
                        string file1 = Path.Combine(Application.StartupPath, "Client.dll");
                        string file2 = Path.Combine(Application.StartupPath, "Client3PL.dll");
                        if (File.Exists(file1))
                        {
                            File.Delete(file1);
                        }
                        if (File.Exists(file2))
                        {
                            File.Delete(file2);
                        }
                    }
                }
                catch (Exception)
                {
                }
                #endregion

                CanLogin = true;
                this.Close();
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("系统参数赋值错误：" + ex.Message);
                return false;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UserPsw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) cbRetrieve.PerformClick();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            #region 获取电脑信息
            Thread th = new Thread(() =>
                {
                    GetComputerInfo();
                });
            th.IsBackground = true;
            th.Start();
            #endregion

            defaultLookAndFeel.LookAndFeel.SkinName = "Office 2007 Blue";
            if (Companyid.Text.Trim() == "")
            {
                Companyid.Text = API.ReadINI("Login", "Companyid", "", "Login");
            }
            if (UserAccount.Text == "")
            {
                UserAccount.Text = API.ReadINI("Login", "UserAccount", "", "Login");
                LastLoginSite = API.ReadINI("Login", "LastLoginSite", "", "Login");
            }
            if (Debugger.IsAttached)
            {
                UserPsw.Text = API.ReadINI("Login", "UserPsw", "", "Login");
            }

            if (AutoLogin)
            {
                simpleButton1_Click(null, null);
                return;
            }

            try
            {
                frmPrintRuiLang w = new frmPrintRuiLang("", new DataTable());
                w.Dispose();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("COM"))
                {
                    MsgBox.ShowOK("系统检测到本机尚未注册打印组件，将自动进行注册\r\n win7、win8需要以系统管理员权限运行系统才能正常注册！");
                    string batPath = Path.Combine(Application.StartupPath, "RegSvr.bat");
                    if (!File.Exists(batPath))
                    {
                        MsgBox.ShowOK("未检测到报表打印组件，请联系系统管理员为您处理！");
                        return;
                    }

                    System.Diagnostics.Process pro = new System.Diagnostics.Process();
                    FileInfo file = new FileInfo(batPath);
                    pro.StartInfo.WorkingDirectory = file.Directory.FullName;
                    pro.StartInfo.FileName = batPath;
                    pro.StartInfo.CreateNoWindow = false;
                    pro.StartInfo.UseShellExecute = false;
                    pro.Start();
                    pro.WaitForExit();
                }
                else
                {
                    MsgBox.ShowError(ex.Message);
                }
            }
        }

        private void frmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MousePoint = new Point(e.X, e.Y);
                mc = 1;
                this.Cursor = Cursors.SizeAll;
            }
        }

        private void frmLogin_MouseMove(object sender, MouseEventArgs e)
        {
            if (mc == 1)
            {
                this.Left = this.Left + (e.X - MousePoint.X);
                this.Top = this.Top + (e.Y - MousePoint.Y);
            }
        }

        private void frmLogin_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            mc = 0;
        }
        private string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_Processor");
                string sCPUSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    sCPUSerialNumber = mo["ProcessorId"].ToString().Trim();
                }
                return sCPUSerialNumber;
            }
            catch
            {
                return "cpu123";
            }
        }


        private string GetSignature()
        {
            try
            {
                ManagementObjectSearcher wmiSearcher = new ManagementObjectSearcher();
                wmiSearcher.Query = new SelectQuery("Win32_DiskDrive", "", new string[] { "PNPDeviceID", "signature" });
                ManagementObjectCollection myCollection = wmiSearcher.Get();
                ManagementObjectCollection.ManagementObjectEnumerator em =
                myCollection.GetEnumerator();
                em.MoveNext();
                ManagementBaseObject mo = em.Current;
                string id = mo.Properties["signature"].Value.ToString().Trim();
                return id;
            }
            catch
            {
                return "yp456";
            }
        }

        private void GetComputerInfo()
        {
            try
            {

                CPU = GetCPUSerialNumber();
                HardDiskID = GetSignature();
                Mac = GetMacAddress();
                //ManagementClass mc = new ManagementClass("Win32_Processor");
                //if (mc == null)
                //{
                //    throw new Exception("第一步检测：可能Windows Management Instrumentation服务没有启动!");
                //}
                //ManagementObjectCollection moc = mc.GetInstances();
                //foreach (ManagementObject mo in moc)
                //{
                //    CPU = mo.Properties["ProcessorId"].Value.ToString();
                //    break;
                //}

                //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                //if (searcher == null)
                //{
                //    throw new Exception("第二步检测：可能Windows Management Instrumentation服务没有启动!");
                //}
                //foreach (ManagementObject mo in searcher.Get())
                //{
                //    HardDiskID = mo["SerialNumber"].ToString().Trim();
                //    break;
                //}
            }
            catch (Exception ex)
            {
                //this.Invoke((MethodInvoker)delegate
                //{
                //    MsgBox.ShowOK("获取本机验证信息失败：\r\n" + ex.Message);
                //});
            }

        }
        /// <summary>  
        /// 获取本机MAC地址  
        /// </summary>  
        /// <returns>本机MAC地址</returns>  
        public string GetMacAddress()
        {
            try
            {
                string strMac = string.Empty;
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        strMac = mo["MacAddress"].ToString();
                    }
                }
                moc = null;
                mc = null;
                string mac = strMac.Replace(":", "-");
                return mac;
            }
            catch
            {
                return "unknown";
            }
        }
    }
}