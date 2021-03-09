using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.UI.Properties;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraBars;


namespace ZQTMS.UI
{
    public partial class frmMainNew : BaseForm
    {
        protected static DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
        public frmMainNew()
        {
            InitializeComponent();
        }

        //[STAThread]
        //static void Main(string[] args)
        private void frmMainNew11(string[] args)
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                //处理UI线程异常
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                //处理非UI线程异常
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                DevExpress.UserSkins.OfficeSkins.Register();
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.Skins.SkinManager.EnableMdiFormSkins();
                DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();

                //遍历Icon替换
                //WindowHooker hooker = new WindowHooker();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainNew));
                //hooker.OnHookControl += (o, e) =>
                //{
                //    Console.WriteLine(e.Control.GetType().ToString());
                //    var frm = e.Control as Form;
                //    frm.Icon = System.Drawing.Icon.ExtractAssociatedIcon("favicon.ico");//GetResAsIcon("favicon.ico");
                //};
                //hooker.Hook();

                string LoginUser = string.Empty;
                string LoginPwd = string.Empty;
                string LoginMac = string.Empty;
                int LoginState = 0;
                if (args.Length > 0)
                {
                    string msg = args[0];
                    string[] arr = msg.Split(new string[] { "@##*@" }, StringSplitOptions.None);
                    if (arr.Length > 0)
                    {
                        LoginUser = arr[0];
                    }
                    if (arr.Length > 1)
                    {
                        LoginPwd = arr[1];
                    }
                    if (arr.Length > 3)
                    {
                        LoginMac = arr[3];
                    }
                    LoginState = 1;
                }

                Login(LoginUser, LoginPwd, LoginState, LoginMac);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("初始化失败：\r\n" + ex.Message);
            }
        }

        private static void Login(string LoginUser, string LoginPwd, int LoginState, string LoginMac)
        {
            try
            {
                frmLogin lf = new frmLogin(LoginUser, LoginPwd, LoginState, LoginMac);
                lf.ShowDialog();
                if (lf.CanLogin == true)
                {
                    //保存日志
                    SaveLoginLog();
                    Application.Run(new frmMainNew());
                }
            }
            catch (UriFormatException)
            {
                XtraMessageBox.Show("无法获取服务器!请检查网络!\r\n如果您已确认网络正常，请用垃圾清理软件清理您电脑中的恶意插件!清理之后再尝试!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Net.WebException)
            {
                XtraMessageBox.Show("无法连接到远程服务器!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存登录日志
        /// </summary>
        private static void SaveLoginLog()
        {
            string hostname = "", localip = "", mac = "";
            CommonClass.GetLocalIp(ref hostname, ref localip, ref mac);
            string publicip = CommonClass.GetPulicAddress();

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ConUser", CommonClass.UserInfo.UserAccount));
            list.Add(new SqlPara("ConTime", CommonClass.gcdate));
            list.Add(new SqlPara("ConHostName", hostname));
            list.Add(new SqlPara("ConMAc", mac));
            list.Add(new SqlPara("ConLANIp", localip));
            list.Add(new SqlPara("ConWANIp", publicip));
            list.Add(new SqlPara("ConAddress", ""));
            list.Add(new SqlPara("UserName", CommonClass.UserInfo.UserName));

            SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_CONLOG", list));
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception error = e.Exception;
            if (error != null && error.Message != "尝试读取或写入受保护的内存。这通常指示其他内存已损坏。")
            {
                MsgBox.ShowOK("捕捉到以下信息：\r\n" + error.Message);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            if (error != null)
            {
                MsgBox.ShowOK("主线程异常：\r\n" + error.Message);
            }
        }

        //加载
        private void frmMainNew_Load(object sender, EventArgs e)
        {
            ribbonControl1.Minimized = true;//HJ20181009
            this.LookAndFeel.UseDefaultLookAndFeel = true;
            defaultLookAndFeel.LookAndFeel.SkinName = "Black";
            DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint();

            CommonClass.FormSet(this);
            ToChn.ConvertToChn();
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.PersonnelDepManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.w_navigation_page"); //hj更换主页面显示
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;

            GetIcon();
            if (CommonClass.dsMainMenu != null && CommonClass.dsMainMenu.Tables.Count > 0)
            {
                ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
                this.SuspendLayout();

                LoadMenu(CommonClass.dsMainMenu.Tables[0]);

                ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
                this.ResumeLayout(false);
               
            }
            CommonClass.ShowWindow(frm, this);

            this.Text = "中强集团信息系统（" + CommonClass.UserInfo.gsqc + "：" + CommonClass.UserInfo.companyid + "）";
            label3.Text = CommonClass.UserInfo.UserName;
            label8.Text = CommonClass.UserInfo.WebName;

            string filePath =Application.StartupPath + "\\Views\\defaultLookAndFeel";
            string SkinName = "";
            if (File.Exists(filePath))
            {
                SkinName = File.ReadAllText(filePath);
                if (!String.IsNullOrEmpty(SkinName))
                {
                    defaultLookAndFeel.LookAndFeel.SkinName = SkinName;
                    comboBoxEdit1.EditValue = SkinName;
                }
            }

        }

        #region 左侧菜单相关

        /// <summary>
        /// 反射显示窗体；菜单使用
        /// </summary>
        /// <param name="dllName">程序集文件名(包含后缀，不含路径)</param>
        /// <param name="nameSpace">程序集命名空间</param>
        /// <param name="assName">窗体类名</param>
        /// <param name="paras">传递参数</param>
        /// <param name="parasTransferMode">参数传递方式(0不传参数； 1构造函数传参  2属性传参  3字段传参)</param>
        /// <param name="showType">窗体显示模式(0 Show  1 ShowDialog 2 MDI)</param>
        private void MenuShowWindow(string dllPath, string dllName, string nameSpace, string assName, string paras, int parasTransferMode, int showType)
        {
            try
            {
                #region 基本判断
                if (nameSpace.Trim() == "")
                {
                    return;//说明该菜单有子目录
                }
                string dllpath = "";
                if (dllPath == @"{根目录}\*.dll")
                {
                    dllpath = Path.Combine(Application.StartupPath, dllName);
                }
                else if (dllPath == @"{根目录}\Plugin\*.dll")
                {
                    dllpath = Path.Combine(Path.Combine(Application.StartupPath, "Plugin"), dllName);
                }
                else if (dllPath == @"{根目录}\主程序.exe")
                {
                    dllpath = Path.Combine(Application.StartupPath, Assembly.GetExecutingAssembly().GetModules(false)[0].Name);
                }
                if (!File.Exists(dllpath))
                {
                    MsgBox.ShowError("程序集文件不存在!");
                    return;
                }
                Assembly ass = Assembly.LoadFrom(dllpath);
                if (ass == null)
                {
                    MsgBox.ShowError("程序集文件加载失败！");
                    return;
                }

                Type type = ass.GetType(string.Format("{0}.{1}", nameSpace, assName));
                if (type == null)
                {
                    MsgBox.ShowError("程序类加载错误！");
                    return;
                }
                #endregion

                Form frm;
                #region 构建实例
                if (parasTransferMode != 1 || string.IsNullOrEmpty(paras))
                {
                    frm = (Form)Activator.CreateInstance(type);
                }
                else
                {
                    if (parasTransferMode == 1 && paras != null)
                    {
                        string[] arr = paras.Split(';');
                        List<object> list = new List<object>();
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string value = arr[i].Split('=')[1];
                            if (value.Contains("{username}"))
                            {
                                value = value.Replace("{username}", CommonClass.UserInfo.UserName);
                            }
                            list.Add(value);
                        }
                        try
                        {
                            frm = (Form)Activator.CreateInstance(type, list.ToArray());
                        }
                        catch (MissingMethodException)
                        {
                            MsgBox.ShowError("构造函数传递参数错误!");
                            return;
                        }
                    }
                    else
                    {
                        frm = (Form)Activator.CreateInstance(type);
                    }
                }
                #endregion

                #region 传参
                if (!string.IsNullOrEmpty(paras))
                {
                    string[] arr = paras.Split(';');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string value = arr[i].Split('=')[1];
                        if (value.Contains("{username}"))
                        {
                            value = value.Replace("{username}", CommonClass.UserInfo.UserName);
                        }
                        if (parasTransferMode == 2)
                        {
                            SetPropertyValue(frm, arr[i].Split('=')[0], value);
                        }
                        if (parasTransferMode == 3)
                        {
                            SetFieldValue(frm, arr[i].Split('=')[0], value);
                        }
                    }
                }

                #endregion

                #region 显示窗体
                if (showType == 0)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.Show();
                }
                else if (showType == 1)
                {
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    frm.ShowDialog();
                }
                else
                {
                    CommonClass.ShowWindow(frm, this);
                }
                #endregion
            }
            catch (MissingMethodException)
            {
                MsgBox.ShowError("实例化传递参数错误!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 设置相应字段的值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldValue">属性值</param>
        private void SetFieldValue(object entity, string fieldName, string fieldValue)
        {
            Type entityType = entity.GetType();

            FieldInfo fieldInfo = entityType.GetField(fieldName);
            if (fieldInfo == null) return;

            if (IsType(fieldInfo.FieldType, "System.String"))
            {
                fieldInfo.SetValue(entity, fieldValue);
                return;
            }

            if (IsType(fieldInfo.FieldType, "System.Boolean"))
            {
                fieldInfo.SetValue(entity, Boolean.Parse(fieldValue));
                return;
            }

            if (IsType(fieldInfo.FieldType, "System.Int32"))
            {
                if (fieldValue != "")
                    fieldInfo.SetValue(entity, int.Parse(fieldValue));
                else
                    fieldInfo.SetValue(entity, 0);
                return;
            }

            if (IsType(fieldInfo.FieldType, "System.Decimal"))
            {
                if (fieldValue != "")
                    fieldInfo.SetValue(entity, Decimal.Parse(fieldValue));
                else
                    fieldInfo.SetValue(entity, new Decimal(0));
                return;
            }

            if (IsType(fieldInfo.FieldType, "System.Nullable[System.DateTime]"))
            {
                if (fieldValue != "")
                {
                    try
                    {
                        fieldInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd HH:mm:ss", null));
                    }
                    catch
                    {
                        fieldInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd", null));
                    }
                }
                else
                    fieldInfo.SetValue(entity, null);
                return;
            }
        }

        /// <summary>
        /// 设置相应属性的值
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fieldName">属性名</param>
        /// <param name="fieldValue">属性值</param>
        private void SetPropertyValue(object entity, string fieldName, string fieldValue)
        {
            Type entityType = entity.GetType();

            PropertyInfo propertyInfo = entityType.GetProperty(fieldName);
            if (propertyInfo == null) return;

            if (IsType(propertyInfo.PropertyType, "System.String"))
            {
                propertyInfo.SetValue(entity, fieldValue, null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Boolean"))
            {
                propertyInfo.SetValue(entity, Boolean.Parse(fieldValue), null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Int32"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, int.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, 0, null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Decimal"))
            {
                if (fieldValue != "")
                    propertyInfo.SetValue(entity, Decimal.Parse(fieldValue), null);
                else
                    propertyInfo.SetValue(entity, new Decimal(0), null);
                return;
            }

            if (IsType(propertyInfo.PropertyType, "System.Nullable[System.DateTime]"))
            {
                if (fieldValue != "")
                {
                    try
                    {
                        propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd HH:mm:ss", null), null);
                    }
                    catch
                    {
                        propertyInfo.SetValue(entity, (DateTime?)DateTime.ParseExact(fieldValue, "yyyy-MM-dd", null), null);
                    }
                }
                else
                    propertyInfo.SetValue(entity, null, null);
                return;
            }
        }

        /// <summary>
        /// 类型匹配
        /// </summary>
        /// <param name="type"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private bool IsType(Type type, string typeName)
        {
            if (type.ToString() == typeName)
                return true;
            if (type.ToString() == "System.Object")
                return false;

            return IsType(type.BaseType, typeName);
        }

        public static void SetImageIndex(TreeList tl, TreeListNode node, int nodeIndex, int parentIndex)
        {
            if (node == null)
            {
                foreach (TreeListNode N in tl.Nodes)
                {
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
                }
            }
            else
            {
                if (node.HasChildren || node.ParentNode == null)
                {
                    node.StateImageIndex = parentIndex;
                    node.ImageIndex = parentIndex;
                }
                else
                {
                    node.StateImageIndex = nodeIndex;
                    node.ImageIndex = nodeIndex;
                }

                foreach (TreeListNode N in node.Nodes)
                {
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
                }
            }
        }

        public static void SetChildImage(TreeList tl, TreeListNode node, int nodeIndex, int parentIndex)
        {
            if (node == null)
            {
                foreach (TreeListNode N in tl.Nodes)
                {
                    SetChildImage(tl, N, nodeIndex, parentIndex);
                }
            }
            else
            {
                if (node.Level > 0)
                {
                    node.StateImageIndex = nodeIndex;
                }
                else
                {
                    string IconName = node.GetValue("IconName").ToString();
                    ImageList imgList = tl.StateImageList as ImageList;
                    if (imgList != null)
                    {
                        int index = imgList.Images.IndexOfKey(IconName);
                        if (index >= 0)
                        {
                            node.StateImageIndex = index;
                        }
                    }
                }

                foreach (TreeListNode N in node.Nodes)
                {
                    SetChildImage(tl, N, nodeIndex, parentIndex);
                }
            }
        }

        private void GetIcon()
        {
            ResourceMenu res = new ResourceMenu();
            PropertyInfo[] peoperInfo = res.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            int i = 0;
            foreach (PropertyInfo pro in peoperInfo)
            {
                if (!pro.PropertyType.FullName.Equals("System.Drawing.Bitmap")) continue;

                imageListTreeList.Images.Add(pro.Name, (Image)pro.GetValue(pro.Name, null));
                i++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CommonClass.ShowBillSearch();
            //MenuShowWindow("{根目录}\\Plugin\\*.dll", "ZQTMS.UI.BuisinessManage.dll", "ZQTMS.UI", "frmBillSearch", "", 0, 1);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (XtraMessageBox.Show("退出之前，请确认所有工作是否已保存!\n\n确定退出吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                e.Cancel = false;
                this.Dispose(true);
                Application.ExitThread();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private int clickTick = -1;
        private void xtraTabbedMdiManager1_MouseUp(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTab.ViewInfo.BaseTabHitInfo hi = xtraTabbedMdiManager1.CalcHitInfo(new Point(e.X, e.Y));
            if (hi.HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageHeader)
            {
                Form frm = (hi.Page as DevExpress.XtraTabbedMdi.XtraMdiTabPage).MdiChild;
                if (frm.GetType() == typeof(frmWeb))
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (clickTick == -1)
                    {
                        clickTick = System.Environment.TickCount;
                    }
                    else
                    {
                        if (System.Environment.TickCount - clickTick < SystemInformation.DoubleClickTime)
                        {
                            if (frm.GetType().ToString() == "ZQTMS.UI.w_navigation_page")
                            {
                                MsgBox.ShowOK("此界面不允许关闭！");
                                return;
                            }
                            frm.Close();
                            clickTick = -1;
                        }
                        else
                        {
                            clickTick = -1;
                        }
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    popupMenu2.ShowPopup(Cursor.Position);
                    barButtonItem1.Tag = barButtonItem2.Tag = frm;
                }
            }
        }

        private void MdiCloseOthers(DevExpress.XtraTabbedMdi.XtraMdiTabPageCollection pages, IntPtr handle)
        {
            for (int i = pages.Count - 1; i >= 0; i--)
            {
                if (pages[i].MdiChild.GetType().ToString() == "ZQTMS.UI.w_navigation_page")
                {
                    continue;
                }
                if (pages[i].MdiChild.Handle != handle)
                {
                    pages[i].MdiChild.Close();
                }
            }
        }

        private void edunit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string billno = edunit.Text.Trim();
                if (billno == "") return;
                CommonClass.ShowBillSearch(billno);
            }
        }

        private void edunit_Click(object sender, EventArgs e)
        {
            edunit.SelectAll();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            frmSetPrinter fsp = frmSetPrinter.Get_frmSetPrinter;
            fsp.Show();
            fsp.TopMost = true;
            fsp.TopMost = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmUpdUserPass frm = new frmUpdUserPass();
            frm.ShowDialog();
        }

        private void edunit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edunit.SelectedIndex == 0) button3.PerformClick();
        }

        private void timerts_Tick(object sender, EventArgs e)
        {
            if (ts.Left > -ts.Width)
            {
                ts.Left = ts.Left - 2;
            }
            else if (ts.Left <= -ts.Width)
            {
                ts.Left = panelControl6.Width - 2;
            }
        }

        //private void lblXmOrderCount_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string txt = (sender as Label).Text.Trim();
        //        int count = 0;
        //        if (!int.TryParse(txt, out count) || count <= 0) return;

        //        Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
        //        if (ass == null) return;
        //        Type type = ass.GetType("ZQTMS.UI.frmWayBillOrder");
        //        if (type == null) return;
        //        Form frm = (Form)Activator.CreateInstance(type);
        //        if (frm == null) return;
        //        CommonClass.ShowWindow(frm, this);
        //    }
        //    catch { }
        //}

        #endregion

        //右键菜单，关闭当前页
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barButtonItem1.Tag == null) return;
                Form frm = barButtonItem1.Tag as Form;
                if (frm.GetType().ToString() == "ZQTMS.UI.w_navigation_page")
                {
                    MsgBox.ShowOK("此界面不允许关闭！");
                    return;
                }
                frm.Close();
            }
            catch { }
        }
        //除此之外全部关闭
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = barButtonItem2.Tag as Form;
            MdiCloseOthers(xtraTabbedMdiManager1.Pages, f.Handle);
        }
        //全部关闭
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int i = xtraTabbedMdiManager1.Pages.Count - 1; i > 0; i--)
            {
                xtraTabbedMdiManager1.Pages[i].MdiChild.Close();
            }
        }

        //更改外观
        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            defaultLookAndFeel.LookAndFeel.SkinName = this.comboBoxEdit1.EditValue.ToString();
            string path = Application.StartupPath + "\\Views\\";
            string filePath = path + "defaultLookAndFeel";
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(filePath))
                {
                    FileStream fs = File.Create(filePath);
                    fs.Close();
                }
                DirFile.WriteText(filePath, comboBoxEdit1.EditValue.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //加载菜单
        private void LoadMenu(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            DataRow[] dr = dt.Select(" LevelID = 1");
            DataRow[] dr1 = dt.Select(" LevelID = 2");
            DataRow[] dr2 = dt.Select(" LevelID = 3");
            int i = 1;
            int j = 1;
            int x = 1;
            foreach (DataRow row in dr)
            {
                RibbonPage rp = null;
                RibbonPageGroup rpg = null;
                BarSubItem bsi = null;
                BarButtonItem bbi = null;

                rp = new RibbonPage();
                //rp.ImageIndex = 3;
                rp.ImageIndex = ConvertType.ToInt32(row["IconIndex"]);
                rp.Name = "ribbonPage" + i.ToString();
                rp.Text = row["MenuName"].ToString();
                //if (row["MenuName"].ToString().Contains("费用标准")) 
                    //rp.Category.Color = Color.Red;
                this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rp });
                rpg = new RibbonPageGroup();
                rpg.Name = "ribbonPageGroup" + i.ToString();
                rpg.Text = "";
                rp.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { rpg });

                foreach (DataRow row1 in dr1)
                {
                    if (row["ID"].ToString() == row1["ParentID"].ToString())
                    {
                        DataRow[] subDr = dt.Select(" ParentID =" + row1["ID"].ToString());
                        if (subDr.Length > 0)
                        {
                            bsi = new BarSubItem();
                            bsi.Caption = row1["MenuName"].ToString();
                            bsi.Id = j;
                            //bsi.ImageIndex = 7;
                            bsi.ImageIndex = ConvertType.ToInt32(row1["IconIndex"]);
                            bsi.Name = "barSubItem" + j.ToString();
                            //窗体参数数组
                            string strParas = row1["DllPath"].ToString() + "&" + row1["DllName"].ToString() + "&" + row1["FormNameSpace"].ToString() + "&" + row1["FormClass"].ToString() + "&" + row1["Paras"].ToString() + "&" +
                                row1["ParasTransferMode"].ToString() + "&" + row1["ShowType"].ToString();
                            bsi.MenuCaption = strParas;

                            rpg.ItemLinks.Add(bsi);
                            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bsi });
                        }
                        else
                        {
                            bbi = new BarButtonItem();
                            bbi.Caption = row1["MenuName"].ToString();
                            bbi.Id = j;
                            //bbi.ImageIndex = 18;
                            bbi.ImageIndex = ConvertType.ToInt32(row1["IconIndex"]);
                            bbi.Name = "barButtonItem" + x.ToString();
                            //窗体参数数组
                            string strParas = row1["DllPath"].ToString() + "&" + row1["DllName"].ToString() + "&" + row1["FormNameSpace"].ToString() + "&" + row1["FormClass"].ToString() + "&" + row1["Paras"].ToString() + "&" +
                                row1["ParasTransferMode"].ToString() + "&" + row1["ShowType"].ToString();
                            bbi.Description = strParas;

                            rpg.ItemLinks.Add(bbi);
                            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });

                            j++;
                            continue;
                        }

                        int count = dt.Select(" LevelID = 3 and ParentID =" + row1["ID"].ToString()).Length;
                        DevExpress.XtraBars.LinkPersistInfo[] lpi = new DevExpress.XtraBars.LinkPersistInfo[count];
                        foreach (DataRow row2 in dr2)
                        {
                            if (row1["ID"].ToString() == row2["ParentID"].ToString())
                            {
                                bbi = new BarButtonItem();

                                bbi.Caption = row2["MenuName"].ToString();
                                bbi.Id = x;
                                //bbi.ImageIndex = 16;
                                bbi.ImageIndex=ConvertType.ToInt32(row2["IconIndex"]);
                                bbi.Name = "barButtonItem" + x.ToString();
                                //窗体参数数组
                                string strParas = row2["DllPath"].ToString() + "&" + row2["DllName"].ToString() + "&" + row2["FormNameSpace"].ToString() + "&" + row2["FormClass"].ToString() + "&" + row2["Paras"].ToString() + "&" +
                                    row2["ParasTransferMode"].ToString() + "&" + row2["ShowType"].ToString();
                                bbi.Description = strParas;

                                lpi[count - 1] = new DevExpress.XtraBars.LinkPersistInfo(bbi);

                                this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });
                                count--;
                                x++;
                            }
                        }
                        #region 注释代码
                        //List<LinkPersistInfo> list = null;
                        //foreach (LinkPersistInfo item in lpi)
                        //{
                        //    if (item == null)
                        //    {
                        //        list = lpi.ToList();
                        //        for (int y = list.Count; y > 0; y--)
                        //        {
                        //            if (list[y - 1] == null)
                        //            {
                        //                list.RemoveAt(y - 1);
                        //            }
                        //        }
                        //        break;
                        //        //foreach (LinkPersistInfo item1 in list)
                        //        //{
                        //        //    list.Remove(item1);
                        //        //}
                        //    }
                        //}
                        //if (list != null && list.Count > 0)
                        //{
                        //    bsi.LinksPersistInfo.AddRange(list.ToArray());
                        //}
                        #endregion
                        if (lpi != null && lpi.Length > 0)
                        {
                            bsi.LinksPersistInfo.AddRange(lpi);
                        }
                        j++;
                    }
                }

                i++;
            }

            this.ribbonControl1.MaxItemId = 1000;
        }

        //打开界面
        private void ribbonControl1_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Name.Contains("barSubItem"))
            {
                return;
            }
            Cursor currentCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //TreeListHitInfo info = treeList1.CalcHitInfo(new Point(e.X, e.Y));
                //if (info.HitInfoType != HitInfoType.Cell) return;

                //TreeListNode node = treeList1.FocusedNode;
                //if (node == null) return;
                //if (node.HasChildren) return;//有子节点，不产生点击动作

                DevExpress.XtraBars.BarButtonItem bbi = e.Item as DevExpress.XtraBars.BarButtonItem;
                string FormPara = string.Empty;
                if (bbi != null)
                {
                    FormPara = bbi.Description;
                }
                string[] strs = FormPara.Split(new char[]{'&'});
                if (strs.Length > 0)
                {
                    string DllPath = strs[0];
                    string DllName = strs[1];
                    string FormNameSpace = strs[2];
                    string FormClass = strs[3];
                    string Paras = strs[4];
                    int ParasTransferMode = Convert.ToInt32(strs[5]);  //参数传递方式(0不传参数； 1构造函数传参  2属性传参  3字段传参)
                    int ShowType = Convert.ToInt32(strs[6]); //界面打开方式(0 Show；  1 ShowDialog； 2 MDI)

                    FormClassManage.GetNewFormClass(ref DllPath, ref DllName, ref FormNameSpace, ref FormClass, ref Paras);

                    MenuShowWindow(DllPath, DllName, FormNameSpace, FormClass, Paras, ParasTransferMode, ShowType);
                }
            }
            catch (Exception)
            { }
            Cursor.Current = currentCursor;

            #region 检测是否需要重启
            Thread th = new Thread(() =>
            {
                if (HttpHelper.HttpPostRestart())
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MsgBox.ShowOK("系统资料有更新，需要重新启动，点击确定后继续!");
                        this.FormClosing -= new FormClosingEventHandler(frmMain_FormClosing);
                        Application.Restart();
                        Application.Exit();
                    });
                }
            });
            th.IsBackground = true;
            th.Start();
            #endregion
        }
    }
}
