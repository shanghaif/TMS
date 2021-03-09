using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using ZQTMS.UI.Properties;
using ZQTMS.Common;
using System.Threading;
using System.Reflection;
using DevExpress.XtraBars;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmMainToZQ :  RibbonForm
    {
        public frmMainToZQ()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 主页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        
        [STAThread]
        static void Main(string[] args)
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
                    //SaveLoginLog();
                    Application.Run(new frmMainToZQ());
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

        private void frmMainToZQ_Load(object sender, EventArgs e)
        {

            /*首页初始化渲染*/
            //ribbonMain.Minimized = true;
            this.LookAndFeel.UseDefaultLookAndFeel = true;
            defaultLookAndFeel.LookAndFeel.SkinName = "Black";
            DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint();

            /*去DLL找主页页面*/
            CommonClass.FormSet(this);
            ToChn.ConvertToChn();
            //Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.PersonnelDepManage.dll");
            //if (ass == null) return;
            //Type type = ass.GetType("ZQTMS.UI.w_navigation_page"); //hj更换主页面显示
            //if (type == null) return;
            //Form frm = (Form)Activator.CreateInstance(type);
            //if (frm == null) return;
            /*主页加载菜单*/
            if (CommonClass.dsMainMenu != null && CommonClass.dsMainMenu.Tables.Count > 0)
            {
                ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).BeginInit();
                this.SuspendLayout();

                LoadMenu(CommonClass.dsMainMenu.Tables[0]);

                ((System.ComponentModel.ISupportInitialize)(this.ribbonMain)).EndInit();
                this.ResumeLayout(false);

            }
            //CommonClass.ShowWindow(frm, this);

            /*底部ribbonStatusBar添加访问者基本信息*/
            barEnd.Caption = "当前用户:" + CommonClass.UserInfo.UserName + "|网点:" + CommonClass.UserInfo.WebName;
        }

        /// <summary>
        /// 应用程序线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception error = e.Exception;
            if (error != null && error.Message != "尝试读取或写入受保护的内存。这通常指示其他内存已损坏。")
            {
                MsgBox.ShowOK("捕捉到以下信息：\r\n" + error.Message);
            }
        }

        /// <summary>
        /// 主线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            if (error != null)
            {
                MsgBox.ShowOK("主线程异常：\r\n" + error.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ribbonGalleryBarItem_GalleryItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            //string caption = string.Empty;

            //if (ribbonGalleryBarItem.Gallery == null) return;

            //caption = ribbonGalleryBarItem.Gallery.GetCheckedItems()[0].Tag.ToString();//获取真实的名称，没有汉化的

            //caption = caption.Replace("主题：", "");

            //XmlDocument doc = new XmlDocument();

            //doc.Load("SkinInfo.xml");

            //XmlNodeList nodelist = doc.SelectSingleNode("SetSkin").ChildNodes;

            //foreach (XmlNode node in nodelist)
            //{

            //    XmlElement xe = (XmlElement)node;//将子节点类型转换为XmlElement类型

            //    if (xe.Name == "Skinstring")
            //    {

            //        xe.InnerText = caption;

            //    }

            //}

            //doc.Save("SkinInfo.xml");
        }

        /// <summary>
        /// 读取资源文件项目中的图片
        /// </summary>
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

        /// <summary>
        /// 加载菜单
        /// </summary>
        /// <param name="dt">数据源</param>
        private void LoadMenu(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }
            DataRow[] dr = dt.Select(" LevelID = 1");
            //DataRow[] dr1 = dt.Select(" LevelID = 2 "); //and  DllName = ''
            //DataRow[] dr2 = dt.Select(" LevelID = 3");
            //int i = 1;
            int j = 1;
            //int x = 1;

            /*便利LevelID = 1第一层的建立RibbonPage 主页头*/
            /*便利LevelID = 2第二层的建立RibbonGroup Page下的组*/
            /*便利LevelID = 3第三层的建立Button 按钮*/
            foreach (DataRow row in dr)
            { 
                BarSubItem bsi = null;
                BarButtonItem bbi = null; //按钮

                /*RibbonPage 根据一级菜单加载*/
                RibbonPage rp = new RibbonPage(); //头页
                if (!string.IsNullOrEmpty(row["IconIndex"].ToString()))
                    rp.ImageIndex = ConvertType.ToInt32(row["IconIndex"]);
                rp.Name = "ribbonPage" + row["ID"].ToString();
                rp.Text = row["MenuName"].ToString();

                /*RibbonPageGroup 根据 RibbonPage 一级父子关系 加载一个或多个  里面按钮基本属性RibbonStyle=All，Caption=菜单名
                 下方为总Group组放置容器
                 */
                RibbonPageGroup rpg = new RibbonPageGroup();  //子页
                this.ribbonMain.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rp });
                rpg.Name = "ribbonPageGroup" + row["ID"].ToString(); //RibbonPageGroup 控件名称 外面i++只有不超过10个
                rpg.Text = row["MenuName"].ToString(); //RibbonPageGroup 显示名称
                rp.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { rpg });

                /*便利数据源 LevelID = 2 New RibbonPageGroup() 
                 * LevelID = 3 In 2的RibbonPageGroup中 */
                DataRow[] drLevel2 = dt.Select(" ParentID =" + row["ID"].ToString());
                foreach (DataRow drLevel2Sub in drLevel2)
                {
                    if (string.IsNullOrEmpty(drLevel2Sub["DllName"].ToString()))
                    {

                        /*空的二级菜单在往下查3级菜单*/
                        DataRow[] drLevel3 = dt.Select(" ParentID =" + drLevel2Sub["ID"].ToString());
                        if (drLevel3.Length <= 0) continue;

                        /*RibbonPageGroup 根据 RibbonPage 一级父子关系 加载一个或多个  里面按钮基本属性RibbonStyle=All，Caption=菜单名*/
                        RibbonPageGroup rpgSub = new RibbonPageGroup();  //子页
                        //this.ribbonMain.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rp });
                        rpgSub.Name = "ribbonPageGroup" + drLevel2Sub["ID"].ToString() + drLevel2Sub["ID"].ToString(); //RibbonPageGroup 控件名称
                        rpgSub.Text = drLevel2Sub["MenuName"].ToString(); //RibbonPageGroup 显示名称
                        rp.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { rpgSub });

                        foreach (DataRow drLevel3Sub in drLevel3)
                        {
                            /*在RibbonPageGroup中加入BarButtonItem（需要便利当前数据源）*/
                            bbi = new BarButtonItem();
                            bbi.Caption = drLevel3Sub["MenuName"].ToString();
                            bbi.RibbonStyle = RibbonItemStyles.All;
                            bbi.Id = j;
                            if (!string.IsNullOrEmpty(drLevel3Sub["IconIndex"].ToString()))
                                bbi.ImageIndex = ConvertType.ToInt32(drLevel3Sub["IconIndex"]);
                            bbi.Name = "barButtonItem" + drLevel3Sub["ID"].ToString();
                            //窗体参数数组
                            string strLevel3Paras = drLevel3Sub["DllPath"].ToString() + "&" + drLevel3Sub["DllName"].ToString() + "&" + drLevel3Sub["FormNameSpace"].ToString() + "&" + drLevel3Sub["FormClass"].ToString() + "&" + drLevel3Sub["Paras"].ToString() + "&" +
                                drLevel3Sub["ParasTransferMode"].ToString() + "&" + drLevel3Sub["ShowType"].ToString();
                            bbi.Description = strLevel3Paras;

                            rpgSub.ItemLinks.Add(bbi);
                            this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });

                            j++;
                            continue;
                        }

                        j++;
                        continue;
                    }

                    /*在RibbonPageGroup中加入BarButtonItem（需要便利当前数据源）*/
                    bbi = new BarButtonItem();
                    bbi.Caption = drLevel2Sub["MenuName"].ToString();
                    bbi.Id = j;
                    if (!string.IsNullOrEmpty(drLevel2Sub["IconIndex"].ToString()))
                        bbi.ImageIndex = ConvertType.ToInt32(drLevel2Sub["IconIndex"]);
                    bbi.Name = "barButtonItem" + drLevel2Sub["ID"].ToString();
                    //窗体参数数组
                    string strParas = drLevel2Sub["DllPath"].ToString() + "&" + drLevel2Sub["DllName"].ToString() + "&" + drLevel2Sub["FormNameSpace"].ToString() + "&" + drLevel2Sub["FormClass"].ToString() + "&" + drLevel2Sub["Paras"].ToString() + "&" +
                        drLevel2Sub["ParasTransferMode"].ToString() + "&" + drLevel2Sub["ShowType"].ToString();
                    bbi.Description = strParas;

                    rpg.ItemLinks.Add(bbi);
                    this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });

                    j++;
                    continue;
                }


                //foreach (DataRow row1 in dr1)
                //{
                //    /*一级菜单下有2级菜单 将2级菜单New RibbonPageGroup()*/
                //    if (row["ID"].ToString() == row1["ParentID"].ToString()) //便利的时候判断如果dr1 LevelID=2里面有相同的时
                //    { 


                //        DataRow[] subDr = dt.Select(" ParentID =" + row1["ID"].ToString());
                //        if (subDr.Length > 0) /*1级菜单下如果有很多子菜单（二级）*/
                //        { 
                //            foreach (DataRow rowSub in subDr)
                //            {
                //                if (string.IsNullOrEmpty(rowSub["DllName"].ToString()))
                //                {
                //                    /*RibbonPageGroup 根据 RibbonPage 一级父子关系 加载一个或多个  里面按钮基本属性RibbonStyle=All，Caption=菜单名*/
                //                    RibbonPageGroup rpgSub = new RibbonPageGroup();  //子页
                //                    //this.ribbonMain.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { rp });
                //                    rpgSub.Name = "ribbonPageGroup" + i.ToString() + rowSub["ID"].ToString(); //RibbonPageGroup 控件名称
                //                    rpgSub.Text = rowSub["MenuName"].ToString(); //RibbonPageGroup 显示名称
                //                    rp.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { rpgSub });

                //                    j++;
                //                    continue;
                //                }

                //                /*在RibbonPageGroup中加入BarButtonItem（需要便利当前数据源）*/
                //                bbi = new BarButtonItem();
                //                bbi.Caption = rowSub["MenuName"].ToString();
                //                bbi.Id = j;
                //                bbi.ImageIndex = ConvertType.ToInt32(rowSub["IconIndex"]);
                //                bbi.Name = "barButtonItem" + x.ToString();
                //                //窗体参数数组
                //                string strParas = rowSub["DllPath"].ToString() + "&" + rowSub["DllName"].ToString() + "&" + rowSub["FormNameSpace"].ToString() + "&" + rowSub["FormClass"].ToString() + "&" + rowSub["Paras"].ToString() + "&" +
                //                    rowSub["ParasTransferMode"].ToString() + "&" + rowSub["ShowType"].ToString();
                //                bbi.Description = strParas;

                //                rpg.ItemLinks.Add(bbi);
                //                this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });

                //                j++;
                //                continue;
                //            }
                //            //bsi = new BarSubItem();
                //            //bsi.Caption = row1["MenuName"].ToString();
                //            //bsi.Id = j;
                //            ////bsi.ImageIndex = 7;
                //            //bsi.ImageIndex = ConvertType.ToInt32(row1["IconIndex"]);
                //            //bsi.Name = "barSubItem" + j.ToString();
                //            ////窗体参数数组
                //            //string strParas = row1["DllPath"].ToString() + "&" + row1["DllName"].ToString() + "&" + row1["FormNameSpace"].ToString() + "&" + row1["FormClass"].ToString() + "&" + row1["Paras"].ToString() + "&" +
                //            //    row1["ParasTransferMode"].ToString() + "&" + row1["ShowType"].ToString();
                //            //bsi.MenuCaption = strParas;

                //            //rpg.ItemLinks.Add(bsi);
                //            //this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bsi });
                //        }
                //        else
                //        {
                //            bbi = new BarButtonItem();
                //            bbi.Caption = row1["MenuName"].ToString();
                //            bbi.Id = j;
                //            //bbi.ImageIndex = 18;
                //            bbi.ImageIndex = ConvertType.ToInt32(row1["IconIndex"]);
                //            bbi.Name = "barButtonItem" + x.ToString();
                //            //窗体参数数组
                //            string strParas = row1["DllPath"].ToString() + "&" + row1["DllName"].ToString() + "&" + row1["FormNameSpace"].ToString() + "&" + row1["FormClass"].ToString() + "&" + row1["Paras"].ToString() + "&" +
                //                row1["ParasTransferMode"].ToString() + "&" + row1["ShowType"].ToString();
                //            bbi.Description = strParas;

                //            rpg.ItemLinks.Add(bbi);
                //            this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });

                //            j++;
                //            continue;
                //        }

                        //int count = dt.Select(" LevelID = 3 and ParentID =" + row1["ID"].ToString()).Length;
                        //DevExpress.XtraBars.LinkPersistInfo[] lpi = new DevExpress.XtraBars.LinkPersistInfo[count];
                        //foreach (DataRow row2 in dr2)
                        //{
                        //    if (row1["ID"].ToString() == row2["ParentID"].ToString())
                        //    {
                        //        bbi = new BarButtonItem();

                        //        bbi.Caption = row2["MenuName"].ToString();
                        //        bbi.Id = x;
                        //        //bbi.ImageIndex = 16;
                        //        bbi.ImageIndex = ConvertType.ToInt32(row2["IconIndex"]);
                        //        bbi.Name = "barButtonItem" + x.ToString();
                        //        //窗体参数数组
                        //        string strParas = row2["DllPath"].ToString() + "&" + row2["DllName"].ToString() + "&" + row2["FormNameSpace"].ToString() + "&" + row2["FormClass"].ToString() + "&" + row2["Paras"].ToString() + "&" +
                        //            row2["ParasTransferMode"].ToString() + "&" + row2["ShowType"].ToString();
                        //        bbi.Description = strParas;

                        //        lpi[count - 1] = new DevExpress.XtraBars.LinkPersistInfo(bbi);

                        //        this.ribbonMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] { bbi });
                        //        count--;
                        //        x++;
                        //    }
                        //}
                         
                        //if (lpi != null && lpi.Length > 0)
                        //{
                        //    bsi.LinksPersistInfo.AddRange(lpi);
                        //}
                        //j++;
                    //}
                //}

                //i++;
            }

            //this.ribbonMain.MaxItemId = 1000;
        } 

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

        /// <summary>
        /// 菜单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ribbonMain_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.Item.Name.Contains("barSubItem"))
            {
                return;
            }
            Cursor currentCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                DevExpress.XtraBars.BarButtonItem bbi = e.Item as DevExpress.XtraBars.BarButtonItem;
                string FormPara = string.Empty;
                if (bbi != null)
                {
                    FormPara = bbi.Description;
                }
                string[] strs = FormPara.Split(new char[] { '&' });
                if (strs.Length > 0)
                {
                    string DllPath = strs[0];
                    string DllName = strs[1];
                    string FormNameSpace = strs[2];
                    string FormClass = strs[3];
                    string Paras = strs[4];
                    int ParasTransferMode = Convert.ToInt32(strs[5]);  //参数传递方式(0不传参数； 1构造函数传参  2属性传参  3字段传参)
                    int ShowType = Convert.ToInt32(strs[6]); //界面打开方式(0 Show；  1 ShowDialog； 2 MDI)

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
                        this.FormClosing -= new FormClosingEventHandler(frmMainToZQ_FormClosing);
                        Application.Restart();
                        Application.Exit();
                    });
                }
            });
            th.IsBackground = true;
            th.Start();
            #endregion
        }

        /// <summary>
        /// 窗体正在关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMainToZQ_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// 选项卡鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xtraTabbedMdiManager_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void barExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.FormClosing += new FormClosingEventHandler(frmMainToZQ_FormClosing);
            //if (XtraMessageBox.Show("退出之前，请确认所有工作是否已保存!\n\n确定退出吗？", "请确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //{
            //    e.Cancel = false;
            //    this.Dispose(true);
                Application.ExitThread();
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        /// <summary>
        /// 快找事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barQuery_ItemClick(object sender, ItemClickEventArgs e)
        {
            CommonClass.ShowBillSearch();
        }

        private void edunit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edunit.SelectedIndex == 0) CommonClass.ShowBillSearch();
        }

        private void edunit_Click(object sender, EventArgs e)
        {
            edunit.SelectAll();
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

        private void barEditCom_ItemPress(object sender, ItemClickEventArgs e)
        {
            MsgBox.ShowOK("ss");
        }
    }
}
