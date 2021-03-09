using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KMS.Tool;
using KMS.Common;
using KMS.SqlDAL;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using DevExpress.XtraGrid.Views.Grid;
using KMS.UI.Properties;

namespace KMS.UI
{
    public partial class frmMain : BaseForm
    {
        protected static DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();

        public frmMain()
        {
            InitializeComponent();
        }
        //yzw 菜单筛选
        public DataTable dt1;

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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                string LoginUser = string.Empty;
                string LoginPwd = string.Empty;
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
                    LoginState = 1;
                }

                Login(LoginUser, LoginPwd, LoginState);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("初始化失败：\r\n" + ex.Message);
            }
        }

        private static void Login(string LoginUser, string LoginPwd, int LoginState)
        {
            try
            {
                frmLogin lf = new frmLogin(LoginUser, LoginPwd, LoginState);
                lf.ShowDialog();
                if (lf.CanLogin == true)
                {
                    //保存日志
                    SaveLoginLog();
                    Application.Run(new frmMain());
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

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.LookAndFeel.UseDefaultLookAndFeel = true;
            frmMain.defaultLookAndFeel.LookAndFeel.SkinName = "Office 2007 Blue";
            DevExpress.Utils.Paint.XPaint.ForceGDIPlusPaint();

            #region 右侧DockPannel高度计算
            int height = this.ClientSize.Height - 30;
            groupControl1.Height = height / 2;
            #endregion

            CommonClass.FormSet(this);
            ToChn.ConvertToChn();
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.PersonnelDepManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("KMS.UI.w_navigation_page"); //hj更换主页面显示
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            
            CommonClass.ShowWindow(frm, this);

            GetIcon();
            if (CommonClass.dsMainMenu != null && CommonClass.dsMainMenu.Tables.Count > 0)
                treeList1.DataSource = CommonClass.dsMainMenu.Tables[0];
            //SetImageIndex(treeList1, null, 1, 3);
            SetChildImage(treeList1, null, 0, 0);
            //yzw 菜单信息赋值
            dt1 = CommonClass.dsMainMenu.Tables[0];

            this.Text = "德坤-干线信息系统（" + CommonClass.UserInfo.gsqc + "：" + CommonClass.UserInfo.companyid + "）";
            //this.Text = "德坤集团信息系统/" + CommonClass.UserInfo.SiteName + "/" + CommonClass.UserInfo.WebName + "/" + CommonClass.UserInfo.UserName;
            label3.Text = CommonClass.UserInfo.UserName;
            label8.Text = CommonClass.UserInfo.WebName;

            GetOA_Msg();

            GetOrderFromHYH();
        }

        #region 右侧DockPannel

        private void GetOA_Msg()
        {
            Thread th = new Thread(delegate()
            {
                while (true)
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Message_By_LoginName", list); //取OA系统的公文
                        DataSet ds = SqlHelper.GetDataSet(sps, 88);
                        if (ds == null || ds.Tables.Count == 0) return;

                        this.Invoke((MethodInvoker)delegate
                        {
                            gridControl1.DataSource = ds.Tables[0];
                            gridControl2.DataSource = ds.Tables[1];
                        });
                        Thread.Sleep(10 * 60 * 1000);//10分钟一次
                    }
                    catch (Exception)
                    {
                        break;
                        //MsgBox.ShowOK(ex.Message);
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            GridView gv = sender as GridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = gv.CalcHitInfo(new Point(e.X, e.Y));
            if (hi.Column == null || hi.RowHandle < 0) return;
            if (hi.Column.ColumnEdit != null && hi.Column.FieldName == "read")
            {
                string guid = gv.GetRowCellValue(hi.RowHandle, "GUID") == DBNull.Value ? "" : gv.GetRowCellValue(hi.RowHandle, "GUID").ToString().Trim();
                if (guid == "") return;
                string fileid = gv.GetRowCellValue(hi.RowHandle, "fileid").ToString();

                string url = "http://oa.dekuncn.com/ESBPM/LoginHandler.ashx?GUID={0}&url=OldMainPage/person.aspx&LinkMessageId={1}";
                url = string.Format(url, guid, fileid);

                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch (Win32Exception) //没有设置默认浏览器，就用IE打开
                {
                    System.Diagnostics.Process.Start("iexplore.exe", url);
                }
            }
        }

        private void gridView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            GridView gv = sender as GridView;
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = gv.CalcHitInfo(new Point(e.X, e.Y));
            if (hi.Column == null || hi.RowHandle < 0) return;
            if (hi.Column.ColumnEdit != null && hi.Column.FieldName == "read")
            {
                string guid = gv.GetRowCellValue(hi.RowHandle, "GUID") == DBNull.Value ? "" : gv.GetRowCellValue(hi.RowHandle, "GUID").ToString().Trim();
                if (guid == "") return;
                string fileid = gv.GetRowCellValue(hi.RowHandle, "fileid").ToString();

                string url = "http://oa.dekuncn.com/ESBPM/LoginHandler.ashx?GUID={0}&url=Forms/NewsRead.aspx?type=info_flowid={1}";
                url = string.Format(url, guid, fileid);

                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch (Win32Exception) //没有设置默认浏览器，就用IE打开
                {
                    System.Diagnostics.Process.Start("iexplore.exe", url);
                }
            }
        }

        private void GetOrderFromHYH() //检测来自好友汇的订单数
        {
            if (!UserRight.GetRight("KMS.UI.frmPtOrder")) return;
            Thread th = new Thread(delegate()
                {
                    while (true)
                    {
                        try
                        {
                            Thread.Sleep(1 * 60 * 1000);
                            List<SqlPara> list = new List<SqlPara>();
                            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TYD_ZTD_For_Main", list);
                            DataSet ds = SqlHelper.GetDataSet(sps);
                            if (ds == null || ds.Tables.Count == 0) return;
                            this.Invoke((MethodInvoker)delegate
                            {
                                try
                                {
                                    lbOrderCount.Text = ds.Tables[0].Rows[0]["num"].ToString();//平台未处理订单
                                    lblXmOrderCount.Text = ConvertType.ToInt32(ds.Tables[1].Rows[0]["num"]).ToString();//项目未处理订单
                                }
                                catch { }
                            });
                            Thread.Sleep(9 * 60 * 1000);//10分钟一次
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                });
            th.IsBackground = true;
            th.Start();
        }

        private void lbOrderCount_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = (sender as Label).Text.Trim();
                int count = 0;
                if (!int.TryParse(txt, out count) || count <= 0) return;

                Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.BuisinessManage.dll");
                if (ass == null) return;
                Type type = ass.GetType("KMS.UI.frmPtOrder");
                if (type == null) return;
                Form frm = (Form)Activator.CreateInstance(type);
                if (frm == null) return;
                CommonClass.ShowWindow(frm, this);
            }
            catch (Exception)
            {
            }

        }
        #endregion

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

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            Cursor currentCursor = Cursor.Current;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                TreeListHitInfo info = treeList1.CalcHitInfo(new Point(e.X, e.Y));
                if (info.HitInfoType != HitInfoType.Cell) return;

                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;
                if (node.HasChildren) return;//有子节点，不产生点击动作

                string DllPath = node.GetValue("DllPath").ToString();
                string DllName = node.GetValue("DllName").ToString();
                string FormNameSpace = node.GetValue("FormNameSpace").ToString();
                string FormClass = node.GetValue("FormClass").ToString();
                string Paras = node.GetValue("Paras").ToString();
                int ParasTransferMode = Convert.ToInt32(node.GetValue("ParasTransferMode"));  //参数传递方式(0不传参数； 1构造函数传参  2属性传参  3字段传参)
                int ShowType = Convert.ToInt32(node.GetValue("ShowType")); //界面打开方式(0 Show；  1 ShowDialog； 2 MDI)


                FormClassManage.GetNewFormClass(ref DllPath, ref DllName, ref FormNameSpace, ref FormClass, ref Paras);

                MenuShowWindow(DllPath, DllName, FormNameSpace, FormClass, Paras, ParasTransferMode, ShowType);
            }
            catch (Exception)
            {
            }
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

        private void treeList1_NodeChanged(object sender, NodeChangedEventArgs e)
        {
            //if (e.ChangeType == NodeChangeTypeEnum.Expanded)
            //{
            //    e.Node.StateImageIndex = e.Node.ImageIndex = e.Node.Expanded ? 2 : 3;
            //}
        }

        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hi = treeList1.CalcHitInfo(new Point(e.X, e.Y));
            if (hi == null) return;
            if (hi.HitInfoType == HitInfoType.Button || hi.HitInfoType == HitInfoType.NodeCheckBox || hi.HitInfoType == HitInfoType.SelectImage || hi.HitInfoType == HitInfoType.StateImage)
            {
                treeList1.Cursor = Cursors.Hand;
            }
            else
            {
                treeList1.Cursor = Cursors.Default;
            }
        }

        private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.Level != 0) return;
            if (e.Column.FieldName == "MenuName" && e.CellText == "费用标准")
            {
                e.Appearance.ForeColor = Color.Red;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "http://oa.dekuncn.com/ESBPM/LoginHandler.ashx?GUID={0}&url=NewMainPage/Index.aspx";
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Guid_By_LoginName", list);
                DataSet ds = SqlHelper.GetDataSet(sps, 88);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("当前账号不能登录OA!");
                    return;
                }
                string guid = ds.Tables[0].Rows[0]["GUID"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["GUID"].ToString().Trim();
                if (guid == "")
                {
                    MsgBox.ShowOK("当前账号不能登录OA!");
                    return;
                }
                url = string.Format(url, guid);
                try
                {
                    System.Diagnostics.Process.Start(url);
                }
                catch (Win32Exception) //没有设置默认浏览器，就用IE打开
                {
                    System.Diagnostics.Process.Start("iexplore.exe", url);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            CommonClass.ShowBillSearch();
            //MenuShowWindow("{根目录}\\Plugin\\*.dll", "KMS.UI.BuisinessManage.dll", "KMS.UI", "frmBillSearch", "", 0, 1);
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
                            if (frm.GetType().ToString() == "KMS.UI.w_navigation_page")
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (barButtonItem1.Tag == null) return;
                Form frm = barButtonItem1.Tag as Form;
                if (frm.GetType().ToString() == "KMS.UI.w_navigation_page")
                {
                    MsgBox.ShowOK("此界面不允许关闭！");
                    return;
                }
                frm.Close();
            }
            catch { }
        }

        private void MdiCloseOthers(DevExpress.XtraTabbedMdi.XtraMdiTabPageCollection pages, IntPtr handle)
        {
            for (int i = pages.Count - 1; i >= 0; i--)
            {
                if (pages[i].MdiChild.GetType().ToString() == "KMS.UI.w_navigation_page")
                {
                    continue;
                }
                if (pages[i].MdiChild.Handle != handle)
                {
                    pages[i].MdiChild.Close();
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form f = barButtonItem2.Tag as Form;
            MdiCloseOthers(xtraTabbedMdiManager1.Pages, f.Handle);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            for (int i = xtraTabbedMdiManager1.Pages.Count - 1; i > 0; i--)
            {
                xtraTabbedMdiManager1.Pages[i].MdiChild.Close();
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

        private void lblXmOrderCount_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = (sender as Label).Text.Trim();
                int count = 0;
                if (!int.TryParse(txt, out count) || count <= 0) return;

                Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.BuisinessManage.dll");
                if (ass == null) return;
                Type type = ass.GetType("KMS.UI.frmWayBillOrder");
                if (type == null) return;
                Form frm = (Form)Activator.CreateInstance(type);
                if (frm == null) return;
                CommonClass.ShowWindow(frm, this);
            }
            catch { }
        }

        private void dockPanel1_Resize(object sender, EventArgs e)
        {
            treeListColumn1.Width = dockPanel1.Width - 24;
        }

        private void textEdit1_Click(object sender, EventArgs e)
        {
            textEdit1.SelectAll();
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string menuName = textEdit1.Text.Trim();
                //为空则显示全部有权限的菜单
                if (menuName == "")
                {
                    treeList1.DataSource = dt1;

                    SetChildImage(treeList1, null, 0, 0);


                }
                //不为空则显示相应的菜单
                else
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("GRCode", CommonClass.UserInfo.GRCode.Replace(',', '@') + "@"));
                        list.Add(new SqlPara("MenuName", menuName));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMENU_MainForm_Select", list);//基础菜单
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                        {
                            string ParentIDall = "";//父级菜单合集

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                DataRow dr2 = ds.Tables[0].Rows[i];
                                ParentIDall += dr2["ParentID"].ToString() + "@";
                                if (dr2["ParentID"].ToString() != "0")
                                {

                                    string ParentID = dr2["ParentID"].ToString();
                                    List<SqlPara> list3 = new List<SqlPara>();
                                    list3.Add(new SqlPara("ParentID", ParentID));
                                    SqlParasEntity sps3 = new SqlParasEntity(OperType.Query, "QSP_GET_BASMENU_MainForm_SecMenu", list3);//二级菜单
                                    DataSet ds3 = SqlHelper.GetDataSet(sps3);
                                    if (ds3 != null && ds3.Tables.Count != 0 && ds3.Tables[0].Rows.Count != 0)
                                    {
                                        for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
                                        {
                                            DataRow dr3 = ds3.Tables[0].Rows[j];
                                            if (dr3["ParentID"].ToString() != "0")
                                            {
                                                ParentIDall += dr3["ParentID"].ToString() + "@";
                                            }



                                        }
                                    }

                                }
                            }
                            List<SqlPara> list4 = new List<SqlPara>();
                            list4.Add(new SqlPara("GRCode", CommonClass.UserInfo.GRCode.Replace(',', '@') + "@"));
                            list4.Add(new SqlPara("MenuName", menuName));
                            list4.Add(new SqlPara("ParentIDall", ParentIDall));
                            SqlParasEntity sps4 = new SqlParasEntity(OperType.Query, "QSP_GET_BASMENU_MainForm_selectAll", list4);
                            DataSet ds4 = SqlHelper.GetDataSet(sps4);
                            if (ds4 != null && ds4.Tables.Count != 0 && ds4.Tables[0].Rows.Count != 0)
                            {
                                GetIcon();
                                treeList1.DataSource = ds4.Tables[0];

                                SetChildImage(treeList1, null, 0, 0);
                            }

                        }

                        else
                        {
                            MsgBox.ShowOK("不存在该菜单请核对!");
                            textEdit1.Focus();
                            textEdit1.SelectAll();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        // MsgBox.ShowException(ex);
                        MsgBox.ShowOK("模糊查询【" + menuName + "】与父级菜单冲突,请输入完整菜单名重试!");
                    }





                }
            }
        }

        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            if (textEdit1.Text.ToString() == "")
            {
                treeList1.DataSource = dt1;
                SetChildImage(treeList1, null, 0, 0);
                textEdit1.Text = "输入菜单名称后回车";
            }
        }

    }
}