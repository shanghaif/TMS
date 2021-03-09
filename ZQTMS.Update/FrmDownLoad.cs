using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;

namespace ZQTMS.Update
{
    public partial class FrmDownLoad : Form
    {
        #region 局部变量

        /// <summary>
        /// The uptlibjson
        /// </summary>
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// 下载目录
        /// </summary>
        private string downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "Update");
        //AppDomain.CurrentDomain.BaseDirectory.ToString()

        /// <summary>
        /// 备份目录
        /// </summary>
        private string backUpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), "Backup");

        /// <summary>
        /// 装载升级数据集
        /// </summary>
        private DataTable dtFiles = null;

        /// <summary>
        /// 下载用户名
        /// </summary>
        private string DownUser = string.Empty;

        /// <summary>
        /// 下载密码
        /// </summary>
        private string DownPwd = string.Empty;

        /// <summary>
        /// 登录员工名
        /// </summary>
        private string SysUser = string.Empty;

        /// <summary>
        /// 登录员工密码
        /// </summary>
        private string SysPwd = string.Empty;

        /// <summary>
        /// 主程序名称，不含路径，包含扩展名
        /// </summary>
        private string SysMainName = string.Empty;

        public string Mac = string.Empty;

        /// <summary>
        /// 文件名称
        /// </summary>
        private string fileName = string.Empty;

        #endregion

        public FrmDownLoad()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造 <see cref="FrmDownload" /> 函数.
        /// </summary>
        /// <param name="Uptlibdata">需要更新DLL数据.</param>
        public FrmDownLoad(String Uptlibdata, String Httpuser, String Httppwd, String User, String Pwd, String MainName,String mac)
        {
            ////测试文件更新程序代码//
            //DataTable dtUpdateRecord = new DataTable("upt_table");
            //DataColumn dc = new DataColumn("FILE_PATH");
            //DataColumn dc1 = new DataColumn("AppPath");
            //DataColumn dc2 = new DataColumn("DOWN_ADDRESS");
            //dtUpdateRecord.Columns.Add(dc);
            //dtUpdateRecord.Columns.Add(dc1);
            //dtUpdateRecord.Columns.Add(dc2);

            //DataRow dr = dtUpdateRecord.NewRow();
            //dr["FILE_PATH"] = "DevExpress.XtraPivotGrid.v9.3.dll";
            //dr["AppPath"] = "";
            //dr["DOWN_ADDRESS"] = "http://ZQTMS.dekuncn.com:7020/UpdateFile/DevExpress.XtraPivotGrid.v9.3.dll.rar";
            //dtUpdateRecord.Rows.Add(dr);

            //DataRow dr1 = dtUpdateRecord.NewRow();
            //dr1["FILE_PATH"] = "DevExpress.XtraPrinting.v9.3.Design.dll";
            //dr1["AppPath"] = "Reports";
            //dr1["DOWN_ADDRESS"] = "http://ZQTMS.dekuncn.com:7020/UpdateFile/DevExpress.XtraPrinting.v9.3.Design.dll.rar";
            //dtUpdateRecord.Rows.Add(dr1);

            //DataRow dr2 = dtUpdateRecord.NewRow();
            //dr2["FILE_PATH"] = "DevExpress.XtraReports.v9.3.dll";
            //dr2["AppPath"] = "Reports";
            //dr2["DOWN_ADDRESS"] = "http://ZQTMS.dekuncn.com:7020/UpdateFile/DevExpress.XtraReports.v9.3.dll.rar";
            //dtUpdateRecord.Rows.Add(dr2);

            //string dtJson = JsonConvert.SerializeObject(dtUpdateRecord, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include, DateFormatString = "yyyy-MM-dd HH:mm:ss" });

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(dtJson);

            //string json = Convert.ToBase64String(bytes);

            //Uptlibdata = json;
            //*********************************************************************************************************************///

            DownUser = Httpuser;
            DownPwd = Httppwd;
            SysUser = User;
            SysPwd = Pwd;
            SysMainName = MainName;
            Mac = mac;
            if (!string.IsNullOrEmpty(Uptlibdata.Trim()))
            {
                sb.Append(Uptlibdata);
            }

            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FrmDownload_Load(object sender, EventArgs e)
        {
            //判断是否有需要更新的json数据
            if (sb.Length == 0)
            {
                MessageBox.Show("没有需要更新的升级数据，请确定后再更新！", "系统提示");
                StartMethod(1);
                Environment.Exit(0);
                return;
            }
            try
            {
                byte[] bytes = Convert.FromBase64String(sb.ToString());
                sb = null;
                string json = Encoding.UTF8.GetString(bytes);
                dtFiles = JsonConvert.DeserializeObject<DataTable>(json);
                json = string.Empty;
                if (dtFiles == null || dtFiles.Rows.Count == 0)
                {
                    MessageBox.Show("没有需要更新的升级数据，请确定后再更新！", "系统提示");
                    StartMethod(1);
                    Environment.Exit(0);
                    return;
                }

                #region 开始检测更新
            Retry:
                string mainPath = AppDomain.CurrentDomain.BaseDirectory + SysMainName;//主程序路径
                Process[] sameProcess = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(mainPath));

                foreach (Process item in sameProcess)
                {
                    if (item.MainModule.FileName.ToLower() != mainPath) //不在当前目录，不管
                    {
                        continue;
                    }
                    string msg = string.Format("以下路径的客户端没有关闭，请将其退出后点击“重试”，继续更新!\r\n{0}", mainPath);
                    DialogResult dl = MessageBox.Show(msg, "更新提示", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (dl == DialogResult.Cancel)
                    {
                        StartMethod(1);
                        Environment.Exit(0);
                        return;
                    }
                    if (dl == DialogResult.Retry)
                    {
                        goto Retry;
                    }
                }

                //清空下载目录和备份目录
                if (!Directory.Exists(downloadPath))
                {
                    Directory.CreateDirectory(downloadPath);
                }
                else
                {
                    Directory.Delete(downloadPath, true);
                    Directory.CreateDirectory(downloadPath);
                }

                if (!Directory.Exists(backUpPath))
                {
                    Directory.CreateDirectory(backUpPath);
                }
                else
                {
                    Directory.Delete(backUpPath, true);
                    Directory.CreateDirectory(backUpPath);
                }

                //启动线程进行下载
                Thread th = new Thread(Progress);
                th.IsBackground = true;
                th.Start();
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新程序发生报错，错误原因为：" + ex.Message, "系统提示");
                StartMethod(1);
                Environment.Exit(0);
            }
        }


        /// <summary>
        /// 下载处理进程.
        /// </summary>
        private void Progress()
        {
            for (int i = 0; i < dtFiles.Rows.Count; i++)
            {
                string[] item = new string[] { dtFiles.Rows[i]["FILE_PATH"].ToString(), "下载中....", "0%" };
                ListViewItem ListItem = new ListViewItem(item);
                ListItem.Name = dtFiles.Rows[i]["FILE_PATH"].ToString();
                lvLoadList.Items.Add(ListItem);
            }
            this.lvLoadList.Refresh();
            int errorCount = FileDown(dtFiles, downloadPath, DownUser, DownPwd);
            if (errorCount > 0) //错误项数大于0则有下载失败
            {
                if (MessageBox.Show(string.Format("{0}个文件下载失败，您是否重新升级更新？", errorCount), "提示信息", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    StartMethod(2);
                }
                else
                {
                    StartMethod(1);
                }
            }
            else
            {
                StartMethod(1);
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// 启动方法
        /// <param name="tag">1：启动主程序  2：再次启动更新</param>
        /// </summary>
        private void StartMethod(int tag)
        {
            if (tag == 1)//启动主程序
            {
                string msg = SysUser + "@##*@" + SysPwd + "@##*@" + tag + "@##*@" + Mac;
                string excutableFilePath = Path.Combine(Application.StartupPath, SysMainName);
                System.Diagnostics.Process.Start(excutableFilePath, msg);
            }
            if (tag == 2) //再次启动更新
            {
                Application.Run(new FrmDownLoad(sb.ToString(), DownUser, DownPwd, SysUser, SysPwd, SysMainName,Mac));
            }

        }


        #region 下载进程需要调用方法集合

        /// <summary>
        /// 进度变化事件
        /// </summary>
        /// <param name="num">位数.</param>
        /// <param name="FullSize">总长度.</param>
        /// <param name="CurrentBytes">当前内容字节.</param>
        private void _FileDownloader_UpdateStatusEvent(int num, long FullSize, long CurrentBytes)
        {
            try
            {
                string bar = (Math.Round((float)CurrentBytes / FullSize, 2) * 100).ToString() + "%";
                lvLoadList.Items[num].SubItems[2].Text = bar;
            }
            catch (Exception ex)
            {
                //MessageBox.Show()
                // LogHelper.Error("更新DLL程序的进度条变化事件报错！", ex);
            }
        }


        /// <summary>
        /// 文件下载方法
        /// </summary>
        /// <param name="UptData">需要更新的数据集.</param>
        /// <param name="FilePath">文件存放路径.</param>
        /// <param name="User">HTTP登录用户.</param>
        /// <param name="Pwd">HTTP密码.</param>
        /// <returns>返回错误项数.</returns>
        private int FileDown(DataTable UptData, string FilePath, string User, string Pwd)
        {
            this.pgBar.Maximum = 100;
            int index = 100 / dtFiles.Rows.Count;
            this.pgBar.Step = index;
            this.pgBar.Minimum = 0;
            int errorCount = 0;
            for (int i = 0; i < dtFiles.Rows.Count; i++)
            {
                //下载成功标识
                bool flg = false;
                string url = string.Empty;
                string appPath = string.Empty;//客户端存放目录

                url = dtFiles.Rows[i]["DOWN_ADDRESS"].ToString();
                appPath = dtFiles.Rows[i]["AppPath"].ToString().Trim();

                //从路径中取出文件名
                string[] urls = url.Split('/');
                fileName = urls[urls.Length - 1];

                /// <summary>
                /// 下载DLL
                /// </summary>
                FileStream output = new FileStream(Path.Combine(downloadPath, fileName), FileMode.Create);

                try
                {
                    Thread.Sleep(100);
                    HttpHelper http = new HttpHelper(this);
                    http.UpdateStatusEvent += new UpdateDelegate(_FileDownloader_UpdateStatusEvent);
                    flg = http.TransData(url, output, i, DownUser, DownPwd);
                    output.Close();


                }
                catch (Exception ex)
                {
                    //LogHelper.Error("更新程序的下载方法类(FileDown)报错", ex);
                    flg = false;
                    output.Close();
                    lvLoadList.Items[dtFiles.Rows[i]["FILE_PATH"].ToString()].SubItems[1].Text = "下载失败";
                    errorCount++;
                    continue;//进入下一个循环
                }

                if (flg)//下载成功
                {
                    bool blZip = UnMakeZipFile(downloadPath + "\\" + fileName, downloadPath);
                    if (blZip)//解压成功
                    {
                        if (File.Exists(downloadPath + "\\" + fileName))
                        {
                            File.Delete(downloadPath + "\\" + fileName); //解压成功，删除解压之前的压缩包
                        }
                        try
                        {
                            #region 转移文件目录
                            DirectoryInfo dir = new DirectoryInfo(downloadPath);
                            foreach (FileInfo fileitem in dir.GetFiles())
                            {
                                string item = fileitem.Name;

                                string excutePath = Path.Combine(Path.Combine(Application.StartupPath, appPath), item);
                                if (File.Exists(excutePath))
                                {
                                    File.Replace(Path.Combine(downloadPath, item), excutePath, Path.Combine(backUpPath, item));
                                }
                                else
                                {
                                    string temp = Path.Combine(downloadPath, item);
                                    if (!Directory.Exists(Path.GetDirectoryName(excutePath)))
                                    {
                                        Directory.CreateDirectory(Path.GetDirectoryName(excutePath));
                                    }
                                    File.Move(temp, excutePath);
                                }
                                File.Delete(fileitem.FullName);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            //LogHelper.Error("更新程序的替换目录里面的DLL报错", ex);
                            lvLoadList.Items[dtFiles.Rows[i]["FILE_PATH"].ToString()].SubItems[1].Text = "下载失败";
                            errorCount++;
                            continue;//进入下一个循环
                        }

                        lvLoadList.Items[dtFiles.Rows[i]["FILE_PATH"].ToString()].SubItems[1].Text = "下载成功";


                    }
                    else//解压失败
                    {
                        lvLoadList.Items[dtFiles.Rows[i]["FILE_PATH"].ToString()].SubItems[1].Text = "解压失败";
                        errorCount++;
                        continue;//进入下一个循环
                    }
                }
                else //下载失败
                {
                    lvLoadList.Items[dtFiles.Rows[i]["FILE_PATH"].ToString()].SubItems[1].Text = "下载失败";
                    errorCount++;
                    continue;//进入下一个循环

                }

                this.lvLoadList.Refresh();
                this.pgBar.PerformStep();

            }

            this.pgBar.PerformStep();
            this.lvLoadList.Items[this.lvLoadList.Items.Count - 1].Selected = true;
            return errorCount;

        }

        /// <summary>
        /// 实现解压操作
        /// </summary>
        /// <param name="zipfilename">要解压文件Zip(物理路径)</param>
        /// <param name="UnZipDir">解压目的路径(物理路径)</param>
        /// <returns>异常信息</returns>
        private bool UnMakeZipFile(string zipfilename, string UnZipDir)
        {
            bool isFlag = false;
            //判断待解压文件路径
            if (!File.Exists(zipfilename))
            {
                return false;
            }
            try
            {
                CZip.DeCompress(zipfilename);
                isFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "系统提示");

            }
            return isFlag;
        }

        #endregion

    }
}
