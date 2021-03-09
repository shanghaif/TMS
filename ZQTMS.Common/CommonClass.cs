using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.Lib;
using ZQTMS.Tool;
using System.Data;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using ZQTMS.SqlDAL;
using System.Threading;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraBars;
using System.Reflection;
using System.ComponentModel;
using DevExpress.XtraGrid.Views.Grid;
using System.Net;
using DevExpress.XtraGrid;
using System.Drawing;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml;
using System.Security.Cryptography;
using System.Web.Security;
using System.Collections;
using ZQTMS.UI;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace ZQTMS.Common
{
    public static class CommonClass
    {
        /// <summary>
        /// 不需要设置权限的工具栏菜单
        /// </summary>
        static List<string> listLink = new List<string>() { "关闭", "退出", "刷新", "快找", "外观设置", "锁定外观", "过滤器", "3", "4", "7", "8", "下一步", "全选", "定义库存天数" };

        /// <summary>
        /// 针对窗体通用设置
        /// </summary>
        /// <param name="frm">调用界面(this)</param>
        /// <returns></returns>
        public static bool FormSet(Form frm)
        {
            #region 检测权限
            if (frm.MdiParent == null) //这个判断，主要是控制核销按钮，通过核销按钮点击出来的界面，不控制工具栏的权限
            {
                return true;
            }
            List<Bar> list = new List<Bar>();
            GetBars(frm, list);
            string fullName = frm.GetType().FullName;
            foreach (Bar bar in list)
            {
                foreach (LinkPersistInfo link in bar.LinksPersistInfo)
                {
                    if (listLink.Contains(link.Item.Caption.Trim())) continue;
                    if (link.Item.GetType() == typeof(BarStaticItem) || link.Item.GetType() == typeof(BarEditItem)) continue;
                    link.Item.Enabled = UserRight.GetRight(fullName + "#" + link.Item.Name);
                }
            }
            return true;
            #endregion
        }

        /// <summary>
        /// 针对窗体通用设置
        /// </summary>
        /// <param name="frm">调用界面(this)</param>
        /// <param name="CheckRight">是否检测工具栏权限</param>
        /// <returns></returns>
        public static bool FormSet(Form frm, bool CheckRight)
        {
            if (!CheckRight)
            {
                return true;
            }
            return FormSet(frm);
        }

        /// <summary>
        /// 针对窗体通用设置
        /// </summary>
        /// <param name="frm">调用界面(this)</param>
        /// <param name="CheckRight">是否检测工具栏权限</param>
        /// <param name="CheckData">检查数据</param>
        /// <returns></returns>
        public static bool FormSet(Form frm, bool CheckRight, bool CheckData)
        {
            //检查数据是否下载完毕
            if (CheckData)
            {
                if (string.IsNullOrEmpty(UserInfo.UserName)
                    || dsSite == null || dsSite.Tables.Count == 0
                    || dsWeb == null || dsWeb.Tables.Count == 0
                    || dsCar == null || dsCar.Tables.Count == 0
                    || dsMiddleSite == null || dsMiddleSite.Tables.Count == 0)
                {
                    MsgBox.ShowOK("正在加载数据,请稍后...");
                    try { frm.Close(); }
                    catch
                    {
                        frm.Shown += new EventHandler(frm_Shown);
                    }
                    return false;
                }
            }

            if (!CheckRight)
            {
                return true;
            }
            return FormSet(frm);
        }

        /// <summary>
        /// 如果窗体在加载时关闭失败,就再这里关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void frm_Shown(object sender, EventArgs e)
        {
            try
            {
                (sender as Form).Close();
            }
            catch { }
        }

        private static void GetBars(Control con, List<Bar> listBar)
        {
            List<BarManager> list = new List<BarManager>();
            GetBarManager(con, list);
            foreach (BarManager manager in list)
            {
                if (manager == null) continue;
                foreach (Bar bar in manager.Bars)
                {
                    listBar.Add(bar);
                }
            }
        }

        private static void GetBarManager(Control con, List<BarManager> list)
        {
            foreach (Control ctl in con.Controls)
            {
                if (ctl.GetType() == typeof(BarDockControl))
                {
                    BarManager bm = (ctl as BarDockControl).Manager;
                    if (!list.Contains(bm))
                    {
                        list.Add(bm);
                    }
                }
                GetBarManager(ctl, list);
            }
        }

        /// <summary>
        /// 加载MyGridView的Columns信息
        /// </summary>
        /// <param name="gridView">要加载的MyGridView</param>
        /// <param name="myGridViews"></param>
        public static void GetGridViewColumns(MyGridView gridView, params MyGridView[] myGridViews)
        {
            GetGridViewColumns(gridView, true, myGridViews);
        }

        private static DataRow[] GetHiddenField(MyGridView GridView)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

            if (GridView.MenuName.Length > 0)
            {
                list.Add(new SqlPara("MenuName", GridView.MenuName));
            }
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_HiddenFieldConfig", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                return dt.Select(string.Format("GridViewID='{0}'", GridView.Guid.ToString()));
            }
            return null;
        }

        /// <summary>
        /// 加载MyGridView的Columns信息
        /// </summary>
        /// <param name="gridView">要加载的MyGridView</param>
        /// <param name="addDoubleClick">是否要为MyGridView添加双击打开快找事件</param>
        /// <param name="myGridViews">要加载的MyGridView</param>
        public static void GetGridViewColumns(MyGridView gridView, bool addDoubleClick, params MyGridView[] myGridViews)
        {
            List<string> listGuid = new List<string>() { gridView.Guid.ToString() };
            foreach (MyGridView view in myGridViews)
            {
                listGuid.Add(view.Guid.ToString());
            }
            string guids = string.Join("@", listGuid.ToArray()) + "@";

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("GridViewID", guids));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GridViewCol_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return;

            if (ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("指定的GridViewGuid不存在，或者没有做对应关系!");
                return;
            }

            int SumFlag = 0;
            List<MyGridView> listView = new List<MyGridView>() { gridView };
            foreach (MyGridView view in myGridViews)
            {
                listView.Add(view);
            }

            DataRow[] drs;
            foreach (MyGridView view in listView)
            {
                int k = 0;
                drs = ds.Tables[0].Select(string.Format("GridViewID='{0}'", view.Guid.ToString()));

                DataRow[] Query = GetHiddenField(view);//根据GridViewID 获取对应的屏蔽字段
                string[] ColFiledNames = null;
                if (Query != null && Query.Length > 0)
                {
                    ColFiledNames = Query[0]["field"].ToString().Replace(" ", "").Split(',');
                }
                Dictionary<string, object> dic = new Dictionary<string, object>();

                view.OptionsBehavior.Editable = true;
                foreach (DataRow item in drs)
                {
                    GridViewColumn gvc = DataRowToModel.FillModel<GridViewColumn>(item);
                    GridColumn col = new GridColumn();
                    col.FieldName = gvc.ColName;
                    col.Caption = gvc.ColCaption;
                    col.OptionsColumn.AllowEdit = col.OptionsColumn.AllowFocus = gvc.AllowEdit == 1;
                    //col.Visible = gvc.Visible == 1;
                    bool vi = true;
                    vi = gvc.Visible == 1 ? true : false;
                    col.Visible = vi;//== 1;

                    if (gvc.AllowSummary == 3)
                    {//Count
                        col.SummaryItem.SetSummary((SummaryItemType)gvc.AllowSummary, "共{0}笔");
                        //else col.SummaryItem.SummaryType = (SummaryItemType)gvc.AllowSummary;//根据选择
                        SumFlag++;
                    }

                    if (gvc.Visible == 1)
                    {
                        col.VisibleIndex = k++;
                    }

                    //设置类型
                    switch (gvc.ColType)
                    {
                        //时间
                        case "datetime":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                            col.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
                            SumFlag++;
                            break;
                        //整数
                        case "int":
                        case "bigint":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.DisplayFormat.FormatString = "{0:#}";
                            col.SummaryItem.SummaryType = SummaryItemType.Sum;
                            col.SummaryItem.DisplayFormat = "{0:#}";
                            SumFlag++;
                            break;
                        //带小数
                        case "decimal":
                        case "float":
                        case "money":
                        case "real":
                            col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                            col.DisplayFormat.FormatString = "{0:#.##}";
                            col.SummaryItem.SummaryType = SummaryItemType.Sum;
                            col.SummaryItem.DisplayFormat = "{0:#.##}";
                            SumFlag++;
                            break;
                        default:
                            if (gvc.AllowSummary != 3)
                            {
                                col.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;
                                col.DisplayFormat.FormatString = "";
                                col.SummaryItem.SummaryType = SummaryItemType.None;
                                col.SummaryItem.DisplayFormat = "";
                            }
                            break;
                    }
                    if (col.Caption == "开单网点" && view.WebControlBindFindName.Length == 0)
                    {
                        view.WebControlBindFindName = col.FieldName;
                    }
                    if (ColFiledNames != null && ColFiledNames.Length > 0)
                    {
                        HidenFiledSetValue(ColFiledNames, gvc, dic);
                    }

                    SetColumnEdit(col);
                    view.Columns.Add(col);
                }
                if (dic.Count > 0)
                {
                    view.DataSourceChanged += new EventHandler(DataSourceChanged);//为MyGridView添加更新屏蔽字段事件
                    view.HiddenFiledDic = dic;
                }
                if (SumFlag > 0)
                {
                    view.OptionsView.ShowFooter = true;
                }
                if (addDoubleClick)
                    AddBillSearchEvent(view);//为MyGridView添加双击打开快找事件
            }
        }

        private static void DataSourceChanged(object sender, EventArgs e)
        {
            MyGridView gridview = (sender as MyGridView);
            DataTable dt = (gridview.DataSource as DataView).Table;
            if (dt == null) { return; }
            int rowCount = gridview.DataRowCount;
            int rowHandle;
            string BindWebFileName = string.Empty;
            gridview.BeginDataUpdate();
            try
            {
                for (rowHandle = rowCount - 1; rowHandle >= 0; rowHandle--)
                {
                    int ColumnsCount = gridview.Columns.Count;
                    DataRow dr = gridview.GetDataRow(rowHandle);
                    BindWebFileName = gridview.WebControlBindFindName;
                    if (BindWebFileName.Length > 0 && dr[BindWebFileName].ToString() == UserInfo.WebName) { continue; }
                    foreach (var dicinfo in gridview.HiddenFiledDic)
                    {
                        if (dr.Table.Columns.Contains(dicinfo.Key))
                        {
                            dr[dicinfo.Key] = dicinfo.Value;
                        }
                    }
                }
            }
            finally
            {
                gridview.EndDataUpdate();
            }
        }

        /// <summary>
        /// 设置屏蔽单元格的值
        /// </summary>
        /// <param name="ColFiledNames"></param>
        /// <param name="gvc"></param>
        /// <param name="dic"></param>
        private static void HidenFiledSetValue(string[] ColFiledNames, GridViewColumn gvc, Dictionary<string, object> dic)
        {
            bool queryRusult = false;
            foreach (string ColFiledName in ColFiledNames)
            {
                if (ColFiledName.Equals(gvc.ColName))
                {
                    queryRusult = true;
                }
            }

            if (queryRusult)
            {
                switch (gvc.ColType)
                {
                    case "int":
                    case "bigint":
                    case "decimal":
                    case "float":
                    case "money":
                    case "real":
                        dic.Add(gvc.ColName, 0);
                        break;
                    case "datetime":
                        dic.Add(gvc.ColName, DBNull.Value);
                        break;
                    default:
                        dic.Add(gvc.ColName, "******");
                        break;
                }
            }
        }

        /// <summary>
        /// 设置GridColumn的ColumnEdit
        /// </summary>
        private static void SetColumnEdit(GridColumn gc)
        {
            if (gc == null) return;
            bool flag = false;
            //先将字段转成小写再判断
            switch (gc.FieldName.ToLower())
            {
                //创建复选框
                case "isgoods"://是否发货
                case "isarrivestation"://是否到站
                case "issendgoods"://是否送货
                case "issign"://是否正常签收
                case "ischecked"://选择
                case "noticestate"://是否控货
                case "aliengoods"://异形货物
                case "goodsvoucher"://货物单据
                case "preciousgoods"://贵重货物
                case "isinvoice"://是否开发票
                case "middlefeepaymentstate"://中转费付款状态
                case "isacceptshort"://网点可接收短驳
                case "isreceiptfee"://是否回单
                case "issupportvalue"://是否保价
                case "isagentfee"://是否代收
                case "ispackagfee"://是否包装
                case "isotherfee"://是否收其它费
                case "ishandlefee"://是否装卸
                case "isstoragefee"://是否进仓
                case "iswarehousefee"://是否仓储
                case "isforkliftfee"://是否叉车
                case "ischangefee"://是否改单
                case "isupstairfee"://是否上楼
                case "iscustomsfee"://是否报关
                case "isframefee"://是否代打木架
                case "isrestart"://是否需要重启
                case "isadd"://是否补货
                case "ismatpay"://是否垫付 hj20180928
                case "isrebates"://回扣已返 hj20180928
                    RepItemCheckEdit.GetRepositoryItemCheckEdit(gc);
                    flag = true;
                    break;
                case "billstate"://运单状态
                case "middlestate"://中转前状态
                    RepItemImageComboBox.GetUnitState(gc);
                    flag = true;
                    break;
                case "devicesource"://设备来源
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 100, new Dictionary<int, string>() { { 0, "CS端" }, { 1, "BS端" } });
                    flag = true;
                    break;
                case "isbackfetchfee"://提付返款审核状态
                case "isbackdeliveryfee"://送货费返款状态
                case "isbackdepartureoptfee"://始发分拨费返款状态
                case "isterminaloptfee"://终端分拨费返款状态
                case "isbacktransferfee"://中转费返款状态
                case "fenbofeeaduitstate"://分拨费返款状态
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 100, new Dictionary<int, string>() { { 0, "未返款" }, { 1, "已返款" } });
                    flag = true;
                    break;

                case "tooaapp"://中转费返款状态
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 100, new Dictionary<int, string>() { { 0, "未付款" }, { 1, "已付款" } });
                    flag = true;
                    break;
                case "isloginenable"://用户账号是否禁止登录
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 100, new Dictionary<int, string>() { { 0, "正常" }, { 1, "禁止" } });
                    flag = true;
                    break;

                //核销状态 0未核销,1已核销,2部分核销
                case "scverifstate"://短驳费核销状态
                case "nowpayverifstate"://现付司机核销状态/现付核销状态
                case "topayverifstate"://到付司机核销状态
                case "backpayverifstate"://回付司机核销状态
                case "fuelcardverifstate"://油卡核销状态
                case "sendverifstate"://送货费核销状态
                case "accmiddlepaystate"://中转费核销状态
                case "fetchpayverifstate"://提付核销状态
                case "monthpayverifstate"://月结核销状态
                case "befarrivalpayverifstate"://货到前付核销状态
                case "distranverifstate"://折扣折让核销状态
                case "collectionpaybackstate"://货款回收状态
                case "collectionpaypoststate"://货款汇款状态
                case "shortowepayverifstate"://短欠核销状态
                case "collectionpaystate"://代收货款状态
                case "middlesendfeestate"://转送费核销状态
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 1, null);
                    flag = true;
                    break;
                //1是/0否
                case "middletracestate"://中转跟踪状态
                case "arrivedstate"://到货状态(运单表)
                case "isprint"://是否打印
                case "isdelete"://是否剔除
                case "isjoinline"://是否加盟干线
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 2, null);
                    flag = true;
                    break;
                //1启用/未启用
                case "userstate":
                case "sitestatus":
                case "webstatus":
                case "middlestatus":
                case "enablevalidate"://用户账号是否启用登录验证
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 3, null);
                    flag = true;
                    break;
                //中转类型
                case "middletype":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 4, null);
                    flag = true;
                    break;
                //装车完毕 0未完毕/1已完毕
                case "isover":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 5, null);
                    flag = true;
                    break;
                //审核状态 0未审核/1已审核
                case "auditingstate":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 6, null);
                    flag = true;
                    break;
                //审批状态 0未审批/1已审批
                case "approvalstate":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 7, null);
                    flag = true;
                    break;
                //执行状态 0未执行/1已执行
                case "excutestate":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 8, null);
                    flag = true;
                    break;
                //短驳/送货 性质 0直营/1加盟
                case "sendtotype":
                case "shorttotype":
                    RepItemImageComboBox.GetRepItemImageComboBox(gc, 9, null);
                    flag = true;
                    break;
                default:
                    flag = false;
                    break;
            }
            if (flag)
            {
                gc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.None;
                gc.DisplayFormat.FormatString = "";
                gc.SummaryItem.SetSummary(SummaryItemType.None, "");
            }
        }

        /// <summary>
        /// 添加双击快找事件
        /// </summary>
        /// <param name="gv"></param>
        private static void AddBillSearchEvent(MyGridView gv)
        {
            if (gv == null) return;
            bool flag = false;
            foreach (GridColumn gc in gv.Columns)
            {
                if (gc.FieldName.ToLower() == "billno")
                {
                    flag = true;
                    break;
                }
            }
            if (flag /*&& IsBindEvent(gv.GetType(), gv, "DoubleClick")*/)
                gv.DoubleClick += new EventHandler(gv_DoubleClick);
        }

        static void gv_DoubleClick(object sender, EventArgs e)
        {
            MyGridView gv = sender as MyGridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;
            GridColumn gc1 = null;
            foreach (GridColumn gc in gv.Columns)
            {
                if (gc.FieldName.ToLower() == "billno")
                {
                    gc1 = gc;
                    break;
                }
            }
            if (gc1 == null) return;
            ShowBillSearch(ConvertType.ToString(gv.GetFocusedRowCellValue(gc1)));
        }

        /// <summary>
        /// 打开快找
        /// </summary>
        public static void ShowBillSearch()
        {
            ShowBillSearch(null);
        }

        /// <summary>
        /// 打开快找并传运单号
        /// </summary>
        /// <param name="BillNo"></param>
        public static void ShowBillSearch(string BillNo)
        {
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type, BillNo);
            if (frm == null) return;
            frm.Show();


        }

        //是否已经绑定了事件
        private static bool IsBindEvent(Type type, MyGridView con, string eventName)
        {
            /*
            bool isBind = false;

            PropertyInfo pi = type.GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);   //获取type类定义的所有事件的信息
            EventHandlerList ehl = (EventHandlerList)pi.GetValue(con, null);    //获取con对象的事件处理程序列表
            FieldInfo fieldInfo = (typeof(Control)).GetField("EventText", BindingFlags.Static | BindingFlags.NonPublic); //获取Control类Click事件的字段信息
            Delegate d = ehl[fieldInfo.GetValue("DoubleClick")];
            if (d == null)
            {
                return isBind;
            }
            foreach (Delegate del in d.GetInvocationList())
            {
                if (del.Method.Name == eventName)
                {
                    isBind = true;
                    break;
                }
            }
            return isBind;
            */
            return false;
        }

        /// <summary>
        /// 在Mdi窗体中显示新窗体
        /// </summary>
        /// <param name="frm">要显示的窗体</param>
        /// <param name="MdiForm">Mdi窗体</param>
        public static void ShowWindow(Form frm, Form MdiForm)
        {
            foreach (Form form in MdiForm.MdiChildren)
            {
                if (form.GetType() == frm.GetType() && form.Text == frm.Text)
                {
                    form.Activate();
                    return;
                }
            }
            frm.MdiParent = MdiForm;
            frm.Show();
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        public static sysUserInfoInfo UserInfo = new sysUserInfoInfo();
        /// <summary>
        /// 系统参数
        /// </summary>
        public static SysArgs Arg = new SysArgs();
        /// <summary>
        /// 二级中转市县
        /// </summary>
        public static DataSet dsMiddleSite = new DataSet();
        /// <summary>
        /// 同星开单页面加载二级市县 
        /// </summary>
        public static DataSet dsMiddleSite_WayBill = new DataSet();
        /// <summary>
        /// 站点数据集
        /// </summary>
        public static DataSet dsSite = new DataSet();
        /// <summary>
        /// 网点数据集
        /// </summary>
        public static DataSet dsWeb = new DataSet();
        /// <summary>
        /// 事业部数据集
        /// </summary>
        public static DataSet dsCause = new DataSet();
        /// <summary>
        /// 大区数据集
        /// </summary>
        public static DataSet dsArea = new DataSet();
        /// <summary>
        /// 部门数据集
        /// </summary>
        public static DataSet dsDep = new DataSet();

        /// <summary>
        /// 车辆信息
        /// </summary>
        public static DataSet dsCar = new DataSet();

        /// <summary>
        /// 发货资料
        /// </summary>
        public static DataSet dsBasCust = new DataSet();

        /// <summary>
        /// 收货资料
        /// </summary>
        public static DataSet dsBasReceiveCust = new DataSet();

        /// <summary>
        /// 操作员
        /// </summary>
        public static DataSet dsUsers = new DataSet();

        /// <summary>
        /// 承运单位
        /// </summary>
        public static DataSet dsRRIERUNIT = new DataSet();

        /// <summary>
        /// 用户权限
        /// <para>字段：GRTag,GRFlag</para>
        /// </summary>
        public static DataSet dsUserRight = new DataSet();

        /// <summary>
        /// 主界面左侧菜单
        /// </summary>
        public static DataSet dsMainMenu = new DataSet();

        /// <summary>
        //短信账户和密码
        /// </summary>
        public static DataSet dsSms = new DataSet();

        /// <summary>
        /// 配载银行信息
        /// </summary>
        public static DataSet dsBank_Loading = new DataSet(); 

        private static string _OAUserID = "";

        /// <summary>
        /// 登录的账号，在OA系统中对应的UserID
        /// </summary>
        public static string OAUserID
        {
            get
            {
                if (_OAUserID == "")
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserID_By_LoginName", list);
                    DataSet ds = SqlHelper.GetDataSet(sps, 88);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    {
                        MsgBox.ShowOK("当前账号在OA系统中不存在!");
                        return "";
                    }
                    string UserID = ds.Tables[0].Rows[0]["UserID"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["UserID"].ToString().Trim();
                    if (UserID == "")
                    {
                        MsgBox.ShowOK("当前账号在OA系统中的ID为空!");
                        return "";
                    }
                    _OAUserID = UserID;
                }
                return _OAUserID;
            }
        }

        public static DataSet dsmsg = new DataSet(); //短信格式表
        public static DataSet dsmsgtel = new DataSet(); //短信环节电话
        public static DataSet dsSiteZQTMS = new DataSet();//maohui20181101
        public static DataSet dsWebZQTMS = new DataSet();//maohui20181101
        // public static DataSet dsCompanyID = new DataSet();//公司ID hj

        /// <summary>
        /// 初始化基础数据
        /// </summary>
        public static void Init()
        {
            Thread tt = new Thread(new ThreadStart(delegate() { while (true) { ServerDate = ServerDate.AddSeconds(10); Thread.Sleep(10000); } }));
            tt.IsBackground = true;
            tt.Start();

            Thread thAll = new Thread(new ThreadStart(delegate()
            {
                #region 组织架构
                MiddleSiteALL();
                 MiddleSiteALL_WayBill();
                //SiteAll();
                //getWeb();
                //AreaAll();
                //CauseAll();
                //DepAll();
                #endregion

                #region 结算体系
                //QSP_GET_BASDELIVERYFEE();//暂时不用了
                //////P_GetSeriesProductDetail();
                //////QSP_GET_BASMAINLINEFEE();
                //////QSP_GET_BASDEPARTUREOPTFEE();
                //////QSP_GET_BASDEPARTUREALLOTFEE();
                //////QSP_GET_BASTRANSFERFEE();
                //////QSP_GET_BASTERMINALOPTFEE();
                //////QSP_GET_BASTERMINALALLOTFEE();
                //QSP_GET_basFreightFee();
                #endregion

                #region 客户档案
                QSP_GET_BASCUST_KD();
                QSP_GET_BASRECEIVECUST_KD();
                #endregion

                #region 用户资料
                QSP_GET_USERNAME();
                #endregion

                #region 短信模板
                QSP_GET_MSG();
                QSP_GET_MSG_TEL_SITE();
                GetCompanySms();
                #endregion

                #region 承运单位
                QSP_GET_BASCARRIERUNIT_Ex();
                #endregion

                #region 车辆档案
                CarAll();
                #endregion

                #region 行政区域
                QSP_GET_BASDIRECTSENDFEE();
                #endregion

                #region 获取ZQTMS和LMS所有的站点
                GetSitaNameZQTMS();
                #endregion

                #region 获取ZQTMS和LMS所有的网点
                GetWebNameZQTMS();
                #endregion  

                //银行信息
                QSP_GET_Bank();

                Thread SurchargeFee_TH = new Thread(new ThreadStart(SurchargeFee));
                SurchargeFee_TH.Priority = ThreadPriority.Highest;
                SurchargeFee_TH.IsBackground = true;
                SurchargeFee_TH.Start();

                Thread DirectDriverFee_TH = new Thread(new ThreadStart(DirectDriverFee));
                DirectDriverFee_TH.Priority = ThreadPriority.Highest;
                DirectDriverFee_TH.IsBackground = true;
                DirectDriverFee_TH.Start();

                //zaj 2018-4-14 支线附加费
                Thread dsSurchargeFee_ZX_TH = new Thread(new ThreadStart(SurchargeFee_ZX));
                dsSurchargeFee_ZX_TH.Priority = ThreadPriority.Highest;
                dsSurchargeFee_ZX_TH.IsBackground = true;
                dsSurchargeFee_ZX_TH.Start();
            }));
            thAll.IsBackground = true;
            thAll.Start();

            return;/////////////////////////////////在前边一个线程加载

            #region 注释,不用线程加载了
            /*
            #region 组织架构
            //Thread MiddleSiteALLTH = new Thread(new ThreadStart(MiddleSiteALL));
            //MiddleSiteALLTH.Priority = ThreadPriority.Highest;
            //MiddleSiteALLTH.IsBackground = true;
            //MiddleSiteALLTH.Start();

            //Thread SiteAllTH = new Thread(new ThreadStart(SiteAll));
            //SiteAllTH.Priority = ThreadPriority.Highest;
            //SiteAllTH.IsBackground = true;
            //SiteAllTH.Start();

            //Thread getWebTH = new Thread(new ThreadStart(getWeb));
            //getWebTH.Priority = ThreadPriority.Highest;
            //getWebTH.IsBackground = true;
            //getWebTH.Start();


            //Thread AreaAllTH = new Thread(new ThreadStart(AreaAll));
            //AreaAllTH.Priority = ThreadPriority.Highest;
            //AreaAllTH.IsBackground = true;
            //AreaAllTH.Start();

            //Thread CauseAllTH = new Thread(new ThreadStart(CauseAll));
            //CauseAllTH.Priority = ThreadPriority.Highest;
            //CauseAllTH.IsBackground = true;
            //CauseAllTH.Start();

            //Thread DepAllTH = new Thread(new ThreadStart(DepAll));
            //DepAllTH.Priority = ThreadPriority.Highest;
            //DepAllTH.IsBackground = true;
            //DepAllTH.Start();
            #endregion

            #region 结算体系
            //Thread QSP_GET_BASDELIVERYFEE_TH = new Thread(new ThreadStart(QSP_GET_BASDELIVERYFEE));
            //QSP_GET_BASDELIVERYFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASDELIVERYFEE_TH.IsBackground = true;
            //QSP_GET_BASDELIVERYFEE_TH.Start();

            //Thread P_GetSeriesProductDetail_TH = new Thread(new ThreadStart(P_GetSeriesProductDetail));
            //P_GetSeriesProductDetail_TH.IsBackground = true;
            //P_GetSeriesProductDetail_TH.Start();


            //Thread QSP_GET_BASMAINLINEFEE_TH = new Thread(new ThreadStart(QSP_GET_BASMAINLINEFEE));
            //QSP_GET_BASMAINLINEFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASMAINLINEFEE_TH.IsBackground = true;
            //QSP_GET_BASMAINLINEFEE_TH.Start();


            //Thread QSP_GET_BASDEPARTUREOPTFEE_TH = new Thread(new ThreadStart(QSP_GET_BASDEPARTUREOPTFEE));
            //QSP_GET_BASDEPARTUREOPTFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASDEPARTUREOPTFEE_TH.IsBackground = true;
            //QSP_GET_BASDEPARTUREOPTFEE_TH.Start();


            //Thread QSP_GET_BASDEPARTUREALLOTFEE_TH = new Thread(new ThreadStart(QSP_GET_BASDEPARTUREALLOTFEE));
            //QSP_GET_BASDEPARTUREALLOTFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASDEPARTUREALLOTFEE_TH.IsBackground = true;
            //QSP_GET_BASDEPARTUREALLOTFEE_TH.Start();

            //Thread QSP_GET_BASTRANSFERFEE_TH = new Thread(new ThreadStart(QSP_GET_BASTRANSFERFEE));
            //QSP_GET_BASTRANSFERFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASTRANSFERFEE_TH.IsBackground = true;
            //QSP_GET_BASTRANSFERFEE_TH.Start();


            //Thread QSP_GET_BASTERMINALOPTFEE_TH = new Thread(new ThreadStart(QSP_GET_BASTERMINALOPTFEE));
            //QSP_GET_BASTERMINALOPTFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASTERMINALOPTFEE_TH.IsBackground = true;
            //QSP_GET_BASTERMINALOPTFEE_TH.Start();

            //Thread QSP_GET_BASTERMINALALLOTFEE_TH = new Thread(new ThreadStart(QSP_GET_BASTERMINALALLOTFEE));
            //QSP_GET_BASTERMINALALLOTFEE_TH.Priority = ThreadPriority.Highest;
            //QSP_GET_BASTERMINALALLOTFEE_TH.IsBackground = true;
            //QSP_GET_BASTERMINALALLOTFEE_TH.Start();


            #endregion

            #region 客户档案
            Thread QSP_GET_BASCUST_KDTH = new Thread(new ThreadStart(QSP_GET_BASCUST_KD));
            QSP_GET_BASCUST_KDTH.Priority = ThreadPriority.Highest;
            QSP_GET_BASCUST_KDTH.IsBackground = true;
            QSP_GET_BASCUST_KDTH.Start();

            Thread QSP_GET_BASRECEIVECUST_KDTH = new Thread(new ThreadStart(QSP_GET_BASRECEIVECUST_KD));
            QSP_GET_BASRECEIVECUST_KDTH.Priority = ThreadPriority.Highest;
            QSP_GET_BASRECEIVECUST_KDTH.IsBackground = true;
            QSP_GET_BASRECEIVECUST_KDTH.Start();
            #endregion

            #region 操作员
            Thread QSP_GET_USERNAME_TH = new Thread(new ThreadStart(QSP_GET_USERNAME));
            QSP_GET_USERNAME_TH.Priority = ThreadPriority.Highest;
            QSP_GET_USERNAME_TH.IsBackground = true;
            QSP_GET_USERNAME_TH.Start();
            #endregion

            #region 短信模板
            Thread QSP_GET_MSG_TH = new Thread(new ThreadStart(QSP_GET_MSG));
            QSP_GET_MSG_TH.Priority = ThreadPriority.Highest;
            QSP_GET_MSG_TH.IsBackground = true;
            QSP_GET_MSG_TH.Start();


            Thread QSP_GET_MSG_TEL_SITE_TH = new Thread(new ThreadStart(QSP_GET_MSG_TEL_SITE));
            QSP_GET_MSG_TEL_SITE_TH.Priority = ThreadPriority.Highest;
            QSP_GET_MSG_TEL_SITE_TH.IsBackground = true;
            QSP_GET_MSG_TEL_SITE_TH.Start();
            #endregion

            #region 承运单位
            Thread QSP_GET_BASCARRIERUNIT_Ex_TH = new Thread(new ThreadStart(QSP_GET_BASCARRIERUNIT_Ex));
            QSP_GET_BASCARRIERUNIT_Ex_TH.Priority = ThreadPriority.Highest;
            QSP_GET_BASCARRIERUNIT_Ex_TH.IsBackground = true;
            QSP_GET_BASCARRIERUNIT_Ex_TH.Start();
            #endregion

            #region 车辆档案
            Thread CarAllTH = new Thread(new ThreadStart(CarAll));
            CarAllTH.Priority = ThreadPriority.Highest;
            CarAllTH.IsBackground = true;
            CarAllTH.Start();
            #endregion

            #region 行政区域
            Thread QSP_GET_BASDIRECTSENDFEE_TH = new Thread(new ThreadStart(QSP_GET_BASDIRECTSENDFEE));
            QSP_GET_BASDIRECTSENDFEE_TH.IsBackground = true;
            QSP_GET_BASDIRECTSENDFEE_TH.Start();
            #endregion
            */
            #endregion
        }

        /// <summary>
        /// 大车直送费
        /// </summary>
        public static DataSet dsDirectDriverFee = new DataSet();
        public static void DirectDriverFee()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DIRECTDRIVERFEE", list);
            dsDirectDriverFee = SqlHelper.GetDataSet(sps);
        }

        /// <summary>
        /// 附加费
        /// </summary>
        public static DataSet dsSurchargeFee = new DataSet();
        public static void SurchargeFee()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSURCHARGEFEE", list);
            dsSurchargeFee = SqlHelper.GetDataSet(sps);
        }

        /// <summary>
        /// 支线附加费
        /// </summary>
        public static DataSet dsSurchargeFee_ZX = new DataSet();
        public static void SurchargeFee_ZX()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSURCHARGEFEE_ZX", list);
            dsSurchargeFee_ZX = SqlHelper.GetDataSet(sps);
        }

        /// <summary>
        /// 中心直送费
        /// </summary>
        public static DataSet dsDirectSendFee = new DataSet(); //

        /// <summary>
        /// 结算送货费分段
        /// </summary>
        public static DataSet dsSendPrice = new DataSet(); //

        /// <summary>
        /// 香港结算送货费
        /// </summary>
        public static DataSet dsSendPriceHK = new DataSet(); //

        /// <summary>
        /// 获取主界面左侧菜单
        /// </summary>
        /// <returns></returns>
        public static bool GetMainMenuData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GRCode", CommonClass.UserInfo.GRCode.Replace(',', '@') + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMENU_MainForm_KT", list);
                dsMainMenu = SqlHelper.GetDataSet(sps);

                if (dsMainMenu == null || dsMainMenu.Tables.Count == 0 || dsMainMenu.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowError("菜单加载失败，没有加载到任何菜单!");
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("菜单加载失败：\r\n" + ex.Message);
                return false;
            }
        }

        #region 组织架构本地化

        public static void MiddleSiteALL()
        {
            //GetbasTableInfo("basMiddleSite", "MiddleSiteId", "QSP_GET_BASMIDDLESITE_OPEN", ref dsMiddleSite);

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("strProc", "QSP_GET_BASMIDDLESITE_OPEN"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_baseProcedure", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    string strSQL = ds.Tables[0].Rows[0]["ProcSql"].ToString();
                    if (UserInfo.companyid == "101")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_JMZX");
                    }
                    else if (UserInfo.companyid == "124")
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_HD");
                    }
                    else
                    {
                        strSQL = strSQL.Replace("@tabName", "basMiddleSite_" + UserInfo.companyid);
                    }
                    strSQL = strSQL.Replace("@LoginSiteName", "'" + UserInfo.SiteName + "'").Replace("@companyid", "'" + UserInfo.companyid + "'");

                    GetbasTableInfo("basMiddleSite", "MiddleSiteId", strSQL, ref dsMiddleSite);
                }
                else
                {
                    GetbasTableInfo("basMiddleSite", "MiddleSiteId", "QSP_GET_BASMIDDLESITE_OPEN", ref dsMiddleSite);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
        public static void MiddleSiteALL_WayBill()
        {
            string procname = "QSP_GET_BASMIDDLESITE_" + UserInfo.companyid;
            string basMiddleSite = "basMiddleSite_" + UserInfo.companyid;
            try
            {
                GetbasTableInfo(basMiddleSite, "MiddleSiteId", procname, ref dsMiddleSite_WayBill);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }

        }





        public static bool SiteAll()
        {
            return GetbasTableInfo("basSite", "SiteId", "QSP_GET_BASSITE", ref dsSite);
        }

        public static bool getWeb()
        {
            return GetbasTableInfo("basWeb", "WebId", "QSP_GET_BASWEB_STATUS", ref dsWeb);
        }

        public static bool AreaAll()
        {
            return GetbasTableInfo("basArea", "AreaID", "QSP_GET_BASAREA", ref dsArea);
        }

        public static bool CauseAll()
        {
            return GetbasTableInfo("basCause", "CauseID", "QSP_GET_BASCAUSE", ref dsCause);
        }

        public static bool DepAll()
        {
            return GetbasTableInfo("basDepart", "DepId", "QSP_GET_BASDEPART", ref dsDep);
        }
        #endregion
        #region 获取上次登录系统的公司id zaj 2018-5-25
        public static bool isSameCompanyid = true;
        public static void getLastLoginCompanyid()
        {
            // bool isSameCompanyid = true;
            string fileName = Application.StartupPath + "\\lastLoainCompanid.xml";
            string companyid = "";
            if (File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode xn = doc.SelectSingleNode("companyid");
                companyid = xn.InnerText;
                if (companyid == CommonClass.UserInfo.companyid)
                {
                    isSameCompanyid = true;
                }
                else
                {
                    isSameCompanyid = false;
                }
            }
            else
            {
                isSameCompanyid = false;
            }

        }
        #endregion


        #region 结算体系本地化

        /// <summary>
        /// 取结算价通用过程
        /// </summary>
        /// <param name="tableName">本地缓存表名</param>
        /// <param name="primaryKey">本地缓存表主键</param>
        /// <param name="procedureName">要访问的存储过程名称</param>
        /// <param name="targetDataset">目的数据集</param>
        private static bool GetbasTableInfo(string tableName, string primaryKey, string procedureName, ref DataSet targetDataset, params List<SqlPara>[] listParas)
        {
            try
            {
                //bool isSameCompanyid=true;
                //string fileName=Application.StartupPath + "\\lastLoainCompanid.xml";
                //string companyid="";
                //if (File.Exists(fileName))
                //{
                //    XmlDocument doc = new XmlDocument();
                //    doc.Load(fileName);
                //    XmlNode xn = doc.SelectSingleNode("companyid");
                //    companyid = xn.InnerText;
                //    if (companyid == CommonClass.UserInfo.companyid)
                //    {
                //        isSameCompanyid = true;
                //    }
                //    else
                //    {
                //        isSameCompanyid = false;
                //    }
                //}
                //else
                //{
                //    isSameCompanyid = false;
                //}
                DataSet dstemp = new DataSet();//本地表中的数据

                DateTime modifydate = Convert.ToDateTime("1950/01/01 0:00");
                int tcount = 0;//是否存在Sqlite表  0不存在 1存在
                DataTable tcols = new DataTable(); //如果有这个表，提取表架构 （cid,name,type,notnull...）
                SQLiteHelper helper = new SQLiteHelper();

                //检测对应的表的本地缓存时间，以及本地表是否存在：
                string sql = string.Format("select ModifyDate from sysLocalCacheTable where TableName='{0}';select count(*) as tcount from sqlite_master where type='table' and name='{1}';pragma table_info ('{2}');", tableName, tableName, tableName);
                OperResult oper = helper.GetDataSet(sql);//登录时已创建表
                if (oper.State == 1 && oper.DataSet != null && oper.DataSet.Tables.Count > 0)
                {
                    if (oper.DataSet.Tables[0].Rows.Count > 0)
                    {
                        modifydate = Convert.ToDateTime(oper.DataSet.Tables[0].Rows[0]["ModifyDate"]).AddSeconds(1);
                    }
                    tcount = Convert.ToInt32(oper.DataSet.Tables[1].Rows[0]["tcount"]);
                    if (tcount == 1) //没有表的时候取不到架构
                    {
                        tcols = oper.DataSet.Tables[2];
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TableName", tableName));
                list.Add(new SqlPara("ModifyDate", modifydate));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_CHECK_CacheTable", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                int result = ds == null || ds.Tables[0].Rows.Count == 0 ? 0 : Convert.ToInt32(ds.Tables[0].Rows[0]["result"]);//0无需更新，1需要更新
                if (!isSameCompanyid)
                {
                    result = 1;
                }

            lb_reload:
                if (result == 1)
                {
                    list = listParas == null || listParas.Length == 0 ? new List<SqlPara>() : listParas[0];
                    //中转二级市县表根据公司ID加载,LD20180812
                    if (tableName == "basMiddleSite" && primaryKey.Contains("MiddleSiteId") && procedureName.Contains("w0_200"))
                    {
                        list.Add(new SqlPara("strSQL", procedureName));
                        sps = new SqlParasEntity(OperType.Query, "USP_BASMIDDLESITE_OptionSQL", list);
                    }
                    else
                    {
                        sps = new SqlParasEntity(OperType.Query, procedureName, list);
                    }
                    targetDataset = SqlHelper.GetDataSet(sps);

                    //创建表，保存内容到本地
                    if (targetDataset != null && targetDataset.Tables.Count > 0)
                    {
                        if (tcount == 1) //本地有表，检测字段是否一致，不一致就删除重建
                        {
                            #region 检测字段是否一致
                            List<string> listCol1 = new List<string>();
                            List<string> listCol2 = new List<string>();
                            for (int i = 0; i < tcols.Rows.Count; i++)
                            {
                                listCol1.Add(tcols.Rows[i]["name"].ToString());
                            }
                            for (int i = 0; i < targetDataset.Tables[0].Columns.Count; i++)
                            {
                                listCol2.Add(targetDataset.Tables[0].Columns[i].ColumnName);
                            }
                            listCol1.Sort();
                            listCol2.Sort();

                            if (string.Join("", listCol1.ToArray()) != string.Join("", listCol2.ToArray())) //字段不相符
                            {
                                helper.ExecuteNonQuery(string.Format("drop table if exists {0}", tableName));
                                tcount = 0;//此时本地没有了
                            }

                            #endregion
                        }
                        if (tcount == 0) //本地没有表，则创建
                        {
                            #region 创建本地缓存表
                            string msg = string.Format("create table if not exists {0} (\r\n", tableName);
                            for (int i = 0; i < targetDataset.Tables[0].Columns.Count; i++)
                            {
                                msg += string.Format("{0} {1},\r\n", targetDataset.Tables[0].Columns[i].ColumnName, helper.DataColumnTypeToSqlite_Type(targetDataset.Tables[0].Columns[i]));
                            }
                            if (primaryKey == "")
                            {
                                msg = msg.Remove(msg.LastIndexOf(",\r\n"), 3);
                                msg += ")";
                            }
                            else
                            {
                                msg += string.Format("CONSTRAINT [] PRIMARY KEY ({0}) ON CONFLICT REPLACE)", primaryKey);
                            }
                            oper = helper.ExecuteNonQuery(msg);

                            if (oper.State == 0)
                            {
                                throw new Exception("本地缓存表创建失败：" + oper.Msg);
                            }
                            #endregion
                        }
                        #region 保存缓存到本地
                        DataTable dt = targetDataset.Tables[0].Clone();
                        for (int i = targetDataset.Tables[0].Rows.Count - 1; i >= 0; i--)
                        {
                            dt.Rows.Add(targetDataset.Tables[0].Rows[i].ItemArray);
                        }
                        helper.ExecuteNonQuery(string.Format("delete from {0}", tableName));
                        oper = helper.SaveDataTable(dt, tableName);
                        if (oper.State == 1)
                        {
                            oper = helper.ExecuteNonQuery(string.Format("insert or replace into sysLocalCacheTable(TableName,ModifyDate) values('{0}','{1}')", tableName, gcdate.ToString("s")));
                        }

                        #endregion
                    }
                }
                else //取本地的数据
                {
                    oper = helper.GetDataSet(string.Format("select * from {0}", tableName));
                    if (oper.State == 1 && oper.DataSet != null)
                    {
                        targetDataset = oper.DataSet;
                    }
                    else
                    {
                        result = 1;
                        goto lb_reload;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }

        ///// <summary>
        ///// 结算送货费
        ///// </summary>
        //public static DataSet dsDeliveryFee = new DataSet();
        //private static void QSP_GET_BASDELIVERYFEE()
        //{
        //    //GetbasTableInfo("basDeliveryFee", "DeliveryFeeID", "QSP_GET_BASDELIVERYFEE", ref dsDeliveryFee); //改用分段的了
        //}


        /// <summary>
        /// 结算送货费分段
        /// </summary>
        /// <returns></returns>
        public static bool P_GetSeriesProductDetail()
        {
            return GetbasTableInfo("basDeliveryFee", "", "P_GetSeriesProductDetail", ref dsSendPrice);
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_GetSeriesProductDetail", list);
            //    dsSendPrice = SqlHelper.GetDataSet(sps);

            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError("结算送货费分段：\r\n" + ex.Message);
            //}
        }

        /// <summary>
        /// 转分拨平台结算送货费分段
        /// </summary>
        /// <returns></returns>
        public static DataSet dsSendPrice_PT = new DataSet();
        public static bool P_GetSeriesProductDetail_PT()
        {
            return GetbasTableInfo("basDeliveryFee", "", "P_GetSeriesProductDetail_PT", ref dsSendPrice_PT);
          
        }

        /// <summary>
        /// 转分拨平台香港结算送货费分段
        /// </summary>
        /// <returns></returns>
        public static DataSet dsSendPrice_PT_HK = new DataSet();
        public static bool P_GetSeriesProductDetail_HKPT() 
        {
            return GetbasTableInfo("basDeliveryFee", "", "P_GetSeriesProductDetail_HK_PT", ref dsSendPrice_PT_HK);

        }


        //ZAJ 2017-12-8
        /// <summary>
        /// 专线结算送货费分段
        /// </summary>
        public static DataSet dsSendPrice_ZX = new DataSet();
        /// <summary>
        /// 专线结算送货费分段
        /// </summary>
        /// <returns></returns>
        public static bool P_GetSeriesProductDetail_ZX()
        {
            return GetbasTableInfo("basDeliveryFee_New", "", "P_GetSeriesProductDetail_ZX_ZQ", ref dsSendPrice_ZX);
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_GetSeriesProductDetail", list);
            //    dsSendPrice = SqlHelper.GetDataSet(sps);

            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError("结算送货费分段：\r\n" + ex.Message);
            //}
        }

        /// <summary>
        /// 香港结算送货费
        /// </summary>
        /// <returns></returns>
        public static bool P_GetSeriesProductDetail_HK()
        {
            return GetbasTableInfo("basDeliveryFeeHK", "", "P_GetSeriesProductDetail_HK", ref dsSendPriceHK);
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();
            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_GetSeriesProductDetail", list);
            //    dsSendPrice = SqlHelper.GetDataSet(sps);

            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError("结算送货费分段：\r\n" + ex.Message);
            //}
        }

        /// <summary>
        /// 结算干线费
        /// </summary>
        public static DataSet dsMainlineFee = new DataSet();
        public static bool QSP_GET_BASMAINLINEFEE()
        {
            return GetbasTableInfo("basMainlineFee", "MainlineFeeID", "QSP_GET_BASMAINLINEFEE", ref  dsMainlineFee);
        }

        /// <summary>
        /// 转分拨平台结算干线费2019.4.29wbw
        /// </summary>
        public static DataSet dsMainlineFeePT = new DataSet();
        public static bool QSP_GET_BASMAINLINEFEE_PT()
        {
            return GetbasTableInfo("basMainlineFee", "MainlineFeeID", "QSP_GET_BASMAINLINEFEE_PT", ref  dsMainlineFeePT);
        }

        //zaj 2017-12-8
        /// <summary>
        /// 专线结算干线费
        /// </summary>
        public static DataSet dsMainlineFee_ZX = new DataSet();
        public static bool QSP_GET_BASMAINLINEFEE_ZX()
        {
            return GetbasTableInfo("basMainlineFee_New", "MainlineFeeID", "QSP_GET_BASMAINLINEFEE_ZX", ref  dsMainlineFee_ZX);
        }

        /// <summary>
        /// 结算始发操作费
        /// </summary>
        public static DataSet dsDepartureOptFee = new DataSet();
        public static bool QSP_GET_BASDEPARTUREOPTFEE()
        {
            return GetbasTableInfo("basDepartureOptFee", "DepartureOptFeeSetID", "QSP_GET_BASDEPARTUREOPTFEE", ref  dsDepartureOptFee);
        }

        /// <summary>
        /// 转分拨平台结算始发操作费
        /// </summary>
        public static DataSet dsDepartureOptFee_PT = new DataSet();
        public static bool QSP_GET_BASDEPARTUREOPTFEE_PT()
        {
            return GetbasTableInfo("basDepartureOptFee", "DepartureOptFeeSetID", "QSP_GET_BASDEPARTUREOPTFEE_PT", ref  dsDepartureOptFee_PT);
        }

        //ZAJ 2017-12-8
        /// <summary>
        /// 专线结算始发操作费
        /// </summary>
        public static DataSet dsDepartureOptFee_ZX = new DataSet();
        public static bool QSP_GET_BASDEPARTUREOPTFEE_ZX()
        {
            return GetbasTableInfo("basDepartureOptFee_New", "DepartureOptFeeSetID", "QSP_GET_BASDEPARTUREOPTFEE_ZX", ref  dsDepartureOptFee_ZX);
        }

        /// <summary>
        /// 结算始发分拔费
        /// </summary>
        public static DataSet dsDepartureAllotFee = new DataSet();
        public static bool QSP_GET_BASDEPARTUREALLOTFEE()
        {
            return GetbasTableInfo("basDepartureAllotFee", "DepartureAllotFeeID", "QSP_GET_BASDEPARTUREALLOTFEE", ref  dsDepartureAllotFee);
        }

        /// <summary>
        /// 平台转分拨结算始发分拔费 2019.5.4
        /// </summary>
        public static DataSet dsDepartureAllotFee_PT = new DataSet();
        public static bool QSP_GET_BASDEPARTUREALLOTFEE_PT()
        {
            return GetbasTableInfo("basDepartureAllotFee", "DepartureAllotFeeID", "QSP_GET_BASDEPARTUREALLOTFEE_PT", ref  dsDepartureAllotFee_PT);
        }

        /// <summary>
        /// 专线结算始发分拨费
        /// </summary>
        public static DataSet dsDepartureAllotFee_ZX = new DataSet();
        public static bool QSP_GET_BASDEPARTUREALLOTFEE_ZX()
        {
            return GetbasTableInfo("basDepartureAllotFeeNew", "DepartureAllotFeeID", "QSP_GET_BASDEPARTUREALLOTFEE_ZX", ref  dsDepartureAllotFee_ZX);
        }


        /// <summary>
        /// 结算中转费
        /// </summary>
        public static DataSet dsTransferFee = new DataSet();
        public static bool QSP_GET_BASTRANSFERFEE()
        {
            return GetbasTableInfo("basTransferFee", "TransferFeeID", "QSP_GET_BASTRANSFERFEE", ref  dsTransferFee);
        }

        /// <summary>
        /// 转分拨平台结算中转费
        /// </summary>
        public static DataSet dsTransferFee_PT = new DataSet();
        public static bool QSP_GET_BASTRANSFERFEE_PT()
        {
            return GetbasTableInfo("basTransferFee", "TransferFeeID", "QSP_GET_BASTRANSFERFEE_PT", ref  dsTransferFee_PT);
        }


        #region 2017-12-6
        /// <summary>
        /// 专线结算中转费
        /// </summary>
        public static DataSet dsTransferFee_ZX = new DataSet();
        public static bool QSP_GET_BASTRANSFERFEE_ZX()
        {
            return GetbasTableInfo("basTransferFee_New", "TransferFeeID", "QSP_GET_BASTRANSFERFEE_ZX", ref  dsTransferFee_ZX);
        }
        #endregion

        /// <summary>
        /// 结算终端操作费
        /// </summary>
        public static DataSet dsTerminalOptFee = new DataSet();
        public static bool QSP_GET_BASTERMINALOPTFEE()
        {
            return GetbasTableInfo("basTerminalOptFee", "TerminalOptFeeID", "QSP_GET_BASTERMINALOPTFEE", ref dsTerminalOptFee);
        }

        /// <summary>
        /// 转分拨平台结算终端操作费 wbw
        /// </summary>
        public static DataSet dsTerminalOptFee_PT = new DataSet();
        public static bool QSP_GET_BASTERMINALOPTFEE_PT()
        {
            return GetbasTableInfo("basTerminalOptFee", "TerminalOptFeeID", "QSP_GET_BASTERMINALOPTFEE_PT", ref dsTerminalOptFee_PT);
        }


        //ZAJ 2017-12-8
        /// <summary>
        /// 专线结算终端操作费
        /// </summary>
        public static DataSet dsTerminalOptFee_ZX = new DataSet();
        public static bool QSP_GET_BASTERMINALOPTFEE_ZX()
        {
            return GetbasTableInfo("basTerminalOptFee_New", "TerminalOptFeeID", "QSP_GET_BASTERMINALOPTFEE_ZX", ref dsTerminalOptFee_ZX);
        }

        /// <summary>
        /// 结算终端分拨费
        /// </summary>
        public static DataSet dsTerminalAllotFee = new DataSet();
        public static bool QSP_GET_BASTERMINALALLOTFEE()
        {
            return GetbasTableInfo("basTerminalAllotFee", "TerminalAllotFeeID", "QSP_GET_BASTERMINALALLOTFEE", ref dsTerminalAllotFee);
        }



        /// <summary>
        /// 最低价格标准
        /// </summary>
        public static DataSet dsFreightFee = new DataSet();
        public static void QSP_GET_basFreightFee()
        {
            GetbasTableInfo("basFreightFee", "FreightId", "QSP_GET_basFreightFee", ref dsFreightFee);
        }

        #endregion

        #region 客户档案本地化
        /// <summary>
        /// 开单提取发货人资料
        /// <para>初始字段：</para>
        /// <para>CustID,CustNo,CusName,CusType,CusTag,ContactMan,ContactPhone,ContactCellPhone,CusEmail,CusAddress,PayWay,CooperateDate,</para>
        /// <para>BankName,BankUserName,BankAdd,BankAccount,BelongSite,BelongWeb,BelongArea,AlwaysSend,GoodsPackag,LoadRequir,SendRequir,</para>
        /// <para>MidRequir,Salesman,CusRemark,CusFeeType,CusDiscount,ModifyDate,IsDelete</para>
        /// </summary>
        private static void QSP_GET_BASCUST_KD()
        {
            try
            {
            xxx:
                DataSet dstemp = new DataSet();//本地表中的数据

                DateTime modifydate = Convert.ToDateTime("1950-1-1");
                int tcount = 0;//是否存在Sqlite表
                DataTable tcols = new DataTable(); //如果有这个表，提取表架构 （cid,name,type,notnull...）
                string tableName = "basCust";//本地缓存表名

                #region SQLite检测
                SQLiteHelper helper = new SQLiteHelper();
                int times = 0;//检索次数，超过3次终止
            lb_tableinfo:
                OperResult oper = helper.GetDataSet(string.Format("select count(*) as tcount from sqlite_master where type='table' and name='{0}';pragma table_info ('{1}');", tableName, tableName));
                if (oper.State == 1 && oper.DataSet != null && oper.DataSet.Tables.Count > 0)
                {
                    tcount = Convert.ToInt32(oper.DataSet.Tables[0].Rows[0]["tcount"]);
                    if (tcount > 0) //说明有basCust表
                    {
                        tcols = oper.DataSet.Tables[1];

                        times = 0;
                    lb_data:
                        oper = helper.GetDataSet(string.Format("select * from {0} where companyid={1}", tableName, CommonClass.UserInfo.companyid));
                        if (oper.DataSet != null && oper.DataSet.Tables.Count > 0 && oper.State == 1)
                        {
                            dstemp = oper.DataSet;
                            if (dstemp.Tables[0].Columns.Contains("ModifyDate"))
                            {
                                object maxdate = dstemp.Tables[0].Compute("max(ModifyDate)", "");
                                if (maxdate != DBNull.Value)
                                {
                                    modifydate = Convert.ToDateTime(maxdate).AddSeconds(1);
                                }
                            }
                        }
                        else
                        {
                            if (times < 3)
                            {
                                times++;
                                goto lb_data;
                            }
                            throw new Exception("读取发货人缓存失败：" + oper.Msg);
                        }
                    }
                }
                else
                {
                    if (times < 3)
                    {
                        times++;
                        goto lb_tableinfo;
                    }
                    throw new Exception("检测发货人缓存失败：" + oper.Msg);
                }
                #endregion

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", UserInfo.SiteName));
                list.Add(new SqlPara("web", UserInfo.WebName));
                list.Add(new SqlPara("ModifyDate", modifydate));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUST_KD", list);
                dsBasCust = SqlHelper.GetDataSet(sps);
                //if (dsBasCust == null || dsBasCust.Tables.Count == 0 || dsBasCust.Tables[0].Rows.Count == 0) return;

                #region Sqlite
                if (dsBasCust != null && dsBasCust.Tables.Count > 0)
                {
                    if (tcount == 0)
                    {
                        #region 创建本地缓存表
                        string msg = string.Format("create table if not exists {0} (\r\n", tableName);
                        for (int i = 0; i < dsBasCust.Tables[0].Columns.Count; i++)
                        {
                            msg += string.Format("{0} {1},\r\n", dsBasCust.Tables[0].Columns[i].ColumnName, helper.DataColumnTypeToSqlite_Type(dsBasCust.Tables[0].Columns[i]));
                        }
                        msg += "CONSTRAINT [] PRIMARY KEY (CustID) ON CONFLICT REPLACE)";

                        oper = helper.ExecuteNonQuery(msg);
                        if (oper.State == 0)
                        {
                            throw new Exception("本地缓存表创建失败：" + oper.Msg);
                        }
                        #endregion
                    }
                    else //有表，检测字段是否一致
                    {
                        #region 检测字段是否一致
                        List<string> listCol1 = new List<string>();
                        List<string> listCol2 = new List<string>();
                        for (int i = 0; i < tcols.Rows.Count; i++)
                        {
                            listCol1.Add(tcols.Rows[i]["name"].ToString());
                        }
                        for (int i = 0; i < dsBasCust.Tables[0].Columns.Count; i++)
                        {
                            listCol2.Add(dsBasCust.Tables[0].Columns[i].ColumnName);
                        }
                        listCol1.Sort();
                        listCol2.Sort();
                        if (string.Join("", listCol1.ToArray()) != string.Join("", listCol2.ToArray())) //字段不相符
                        {
                            helper.ExecuteNonQuery(string.Format("drop table if exists {0}", tableName));
                            goto xxx;
                        }
                        #endregion
                    }

                    #region 针对服务端已删除的记录，在本地缓存表中删除
                    List<string> listDel = new List<string>();//删除的记录
                    DataTable dt = dsBasCust.Tables[0].Clone();
                    for (int i = dsBasCust.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToInt32(dsBasCust.Tables[0].Rows[i]["IsDelete"]) == 1)
                        {
                            listDel.Add(string.Format("delete from {0} where CustID='{1}'", tableName, dsBasCust.Tables[0].Rows[i]["CustID"]));
                            dsBasCust.Tables[0].Rows.RemoveAt(i);
                        }
                        else
                        {
                            dt.Rows.Add(dsBasCust.Tables[0].Rows[i].ItemArray);
                        }
                    }
                    dsBasCust.Tables[0].AcceptChanges();
                    if (listDel.Count > 0)
                    {
                        helper.ExecuteNonQuery(listDel);
                    }
                    oper = helper.SaveDataTable(dt, tableName);
                    dt.Dispose();
                    #endregion
                }
                #endregion
                if (dstemp.Tables.Count > 0)
                {
                    dsBasCust.Tables[0].Merge(dstemp.Tables[0]);
                }
                dstemp.Dispose();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 开单提取收货人资料
        /// <para>初始字段：</para>
        /// <para>RCID,CusID,BelongSite,BelongArea,RCName,ContactMan,ContactPhone,ContactCellPhone,RecievAddProv,</para>
        /// <para>RecievAddCity,RecievAddArea,RecievAddStreet,RecievAddress,ArriveSite,MidSite,TransferMode,</para>
        /// <para>DestinationWeb,CooperateDate,BelongWeb,ModifyDate,IsDelete</para>
        /// </summary>
        private static void QSP_GET_BASRECEIVECUST_KD()
        {
            try
            {
            xxx:
                DataSet dstemp = new DataSet();//本地表中的数据

                DateTime modifydate = Convert.ToDateTime("1950-1-1");
                int tcount = 0;//是否存在Sqlite表
                DataTable tcols = new DataTable(); //如果有这个表，提取表架构 （cid,name,type,notnull...）
                string tableName = "basReceiveCust";//本地缓存表名

                #region SQLite检测
                SQLiteHelper helper = new SQLiteHelper();
                int times = 0;//检索次数，超过3次终止
            lb_tableinfo:
                OperResult oper = helper.GetDataSet(string.Format("select count(*) as tcount from sqlite_master where type='table' and name='{0}';pragma table_info ('{1}');", tableName, tableName));
                if (oper.State == 1 && oper.DataSet != null && oper.DataSet.Tables.Count > 0)
                {
                    tcount = Convert.ToInt32(oper.DataSet.Tables[0].Rows[0]["tcount"]);
                    if (tcount > 0) //说明有basReceiveCust表
                    {
                        tcols = oper.DataSet.Tables[1];

                        times = 0;
                    lb_data:
                        oper = helper.GetDataSet(string.Format("select * from {0} where companyid={1}", tableName,CommonClass.UserInfo.companyid));
                        if (oper.DataSet != null && oper.DataSet.Tables.Count > 0 && oper.State == 1  )
                        {
                            dstemp = oper.DataSet;
                            if (dstemp.Tables[0].Columns.Contains("ModifyDate"))
                            {
                                object maxdate = dstemp.Tables[0].Compute("max(ModifyDate)", "");
                                if (maxdate != DBNull.Value)
                                {
                                    modifydate = Convert.ToDateTime(maxdate).AddSeconds(1);
                                }
                            }
                        }
                        else
                        {
                            if (times < 3)
                            {
                                times++;
                                goto lb_data;
                            }
                            throw new Exception("读取收货人缓存失败：" + oper.Msg);
                        }
                    }
                }
                else
                {
                    if (times < 3)
                    {
                        times++;
                        goto lb_tableinfo;
                    }
                    throw new Exception("检测收货人缓存失败：" + oper.Msg);
                }
                #endregion

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", UserInfo.SiteName));
                list.Add(new SqlPara("web", UserInfo.WebName));
                list.Add(new SqlPara("ModifyDate", modifydate));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASRECEIVECUST_KD", list);
                dsBasReceiveCust = SqlHelper.GetDataSet(sps);
                // if (dsBasReceiveCust == null || dsBasReceiveCust.Tables.Count == 0 || dsBasReceiveCust.Tables[0].Rows.Count == 0) return;

                #region Sqlite
                if (dsBasReceiveCust != null && dsBasReceiveCust.Tables.Count > 0)
                {
                    if (tcount == 0)
                    {
                        #region 创建本地缓存表
                        string msg = string.Format("create table if not exists {0} (\r\n", tableName);
                        for (int i = 0; i < dsBasReceiveCust.Tables[0].Columns.Count; i++)
                        {
                            msg += string.Format("{0} {1},\r\n", dsBasReceiveCust.Tables[0].Columns[i].ColumnName, helper.DataColumnTypeToSqlite_Type(dsBasReceiveCust.Tables[0].Columns[i]));
                        }
                        msg += "CONSTRAINT [] PRIMARY KEY (RCID) ON CONFLICT REPLACE)";

                        oper = helper.ExecuteNonQuery(msg);
                        if (oper.State == 0)
                        {
                            throw new Exception("本地缓存表创建失败：" + oper.Msg);
                        }
                        #endregion
                    }
                    else //有表，检测字段是否一致
                    {
                        #region 检测字段是否一致
                        List<string> listCol1 = new List<string>();
                        List<string> listCol2 = new List<string>();
                        for (int i = 0; i < tcols.Rows.Count; i++)
                        {
                            listCol1.Add(tcols.Rows[i]["name"].ToString());
                        }
                        for (int i = 0; i < dsBasReceiveCust.Tables[0].Columns.Count; i++)
                        {
                            listCol2.Add(dsBasReceiveCust.Tables[0].Columns[i].ColumnName);
                        }
                        listCol1.Sort();
                        listCol2.Sort();
                        if (string.Join("", listCol1.ToArray()) != string.Join("", listCol2.ToArray())) //字段不相符
                        {
                            helper.ExecuteNonQuery(string.Format("drop table {0}", tableName));
                            goto xxx;
                        }
                        #endregion
                    }

                    #region 针对服务端已删除的记录，在本地缓存表中删除
                    List<string> listDel = new List<string>();//删除的记录
                    DataTable dt = dsBasReceiveCust.Tables[0].Clone();
                    for (int i = dsBasReceiveCust.Tables[0].Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToInt32(dsBasReceiveCust.Tables[0].Rows[i]["IsDelete"]) == 1)
                        {
                            listDel.Add(string.Format("delete from {0} where RCID='{1}'", tableName, dsBasReceiveCust.Tables[0].Rows[i]["RCID"]));
                            dsBasReceiveCust.Tables[0].Rows.RemoveAt(i);
                        }
                        else
                        {
                            dt.Rows.Add(dsBasReceiveCust.Tables[0].Rows[i].ItemArray);
                        }
                    }
                    dsBasReceiveCust.Tables[0].AcceptChanges();
                    if (listDel.Count > 0)
                    {
                        helper.ExecuteNonQuery(listDel);
                    }
                    oper = helper.SaveDataTable(dt, tableName);
                    dt.Dispose();
                    #endregion
                }
                #endregion

                if (dstemp.Tables.Count > 0)
                {
                    dsBasReceiveCust.Tables[0].Merge(dstemp.Tables[0]);
                }
                dstemp.Dispose();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        #endregion

        #region 操作员信息本地化
        private static void QSP_GET_USERNAME()
        {
            GetbasTableInfo("sysUserInfo", "UserId", "QSP_GET_USERNAME", ref dsUsers);
        }
        #endregion

        #region 短信模板本地化
        private static void QSP_GET_MSG()
        {
            GetbasTableInfo("basSmsFormat", "id", "QSP_GET_MSG", ref dsmsg);
        }

        private static void QSP_GET_MSG_TEL_SITE()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("site", UserInfo.SiteName));
            GetbasTableInfo("basSmsTel", "InfoId", "QSP_GET_MSG_TEL_SITE", ref dsmsgtel, list);
        }
        #endregion

        #region 承运单位本地化
        private static void QSP_GET_BASCARRIERUNIT_Ex()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("CUSite", UserInfo.SiteName));
            GetbasTableInfo("BasCarrierUnit", "CUId", "QSP_GET_BASCARRIERUNIT_Ex", ref dsRRIERUNIT, list);
        }
        #endregion

        #region 车辆档案本地化

        private static void CarAll()
        {
            GetbasTableInfo("basCar", "CarId", "QSP_GET_CAR", ref dsCar);
        }

        #endregion

        #region 行政区域本地化
        /// <summary>
        /// 行政区域
        /// </summary>
        /// <returns></returns>
        public static void QSP_GET_BASDIRECTSENDFEE()
        {
            GetbasTableInfo("basDirectSendFee", "DirectSendID", "QSP_GET_BASDIRECTSENDFEE", ref dsDirectSendFee);
        }
        #endregion


        private static void QSP_GET_Bank()
        {
            GetbasTableInfo("b_bank_Loading", "id", "QSP_GET_BANK_BILLDEPARTURE", ref dsBank_Loading);
        }
        /// <summary>
        /// 获取运单实时状态
        /// <para>BillNo</para>
        /// </summary>
        public static int GetBillState(string BillNo)
        {
            int BillState = 100;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BillState", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return BillState;
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    BillState = ConvertType.ToInt32(ds.Tables[0].Rows[0]["BillState"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return BillState;
        }

        /// <summary>
        /// 获取运单审核状态
        /// <para>BillNo</para>
        /// </summary>
        public static int GetAduitState(string BillNo)
        {
            int num = 0;
            try
            {
                List<SqlPara> list_1 = new List<SqlPara>();
                list_1.Add(new SqlPara("billno", BillNo));
                SqlParasEntity sps_1 = new SqlParasEntity(OperType.Query, "QSP_Check_Aduit", list_1);
                DataSet ds = SqlHelper.GetDataSet(sps_1);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return num;
                num = ConvertType.ToInt32(ds.Tables[0].Rows[0]["num"]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return num;
        }

        public static void SetProvince(DevExpress.XtraEditors.ComboBoxEdit province)
        {

            List<SqlPara> list = new List<SqlPara>();

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PROVINCE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                province.Properties.Items.AddRange(new object[] {
                 dr["RegionName"]
                });
            }

        }

        public static void SetCity(DevExpress.XtraEditors.ComboBoxEdit province, DevExpress.XtraEditors.ComboBoxEdit city)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("province", province.Text.Trim().ToString()));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CITY", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            city.EditValue = "";
            city.Properties.Items.Clear();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                DataRow dr = ds.Tables[0].Rows[i];
                city.Properties.Items.AddRange(new object[] {
                  dr["RegionName"]
                 });
            }

        }

        /// <summary>
        /// 获取运单出库状态
        /// <para>BillNo</para>
        /// </summary>
        public static int GetOutState(string BillNo)
        {
            int num = 0;
            try
            {
                List<SqlPara> list_1 = new List<SqlPara>();
                list_1.Add(new SqlPara("billno", BillNo));
                SqlParasEntity sps_1 = new SqlParasEntity(OperType.Query, "QSP_Check_OutState", list_1);
                DataSet ds = SqlHelper.GetDataSet(sps_1);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return num;
                num = ConvertType.ToInt32(ds.Tables[0].Rows[0]["num"]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return num;
        }
        /// <summary>
        /// 获取运单核销状态
        /// <para>BillNo</para>
        /// </summary>
        public static int GetVerifyStatus(string BillNo)  //maohui20180202
        {
            int num = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity sp = new SqlParasEntity(OperType.Query, "QSP_Check_VerifyStatus", list);
                DataSet ds = SqlHelper.GetDataSet(sp);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return num;
                num = ConvertType.ToInt32(ds.Tables[0].Rows[0]["num"]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return num;
        }
        /// <summary>
        /// 验证数据输入的数据是否合法
        /// <para>oper：1 分单配载    2 核销</para>
        /// </summary>
        /// <param name="gv">数据网格</param>
        /// <param name="e">要验证的单元格数据</param>
        /// <param name="filed">要用来比较的字段</param>
        /// <param name="oper">操作类型</param>
        public static void ValidateGridView(MyGridView gv, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e, string filed, int oper)
        {
            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle < 0) return;

            try
            {
                int oldvalue = Convert.ToInt32(gv.GetRowCellValue(rowhandle, filed));
                int newvalue = Convert.ToInt32(e.Value);
                if (newvalue == 0)
                {
                    e.Valid = true;
                    if (oper == 1)
                    {
                        e.Valid = false;
                        e.ErrorText = "实发件数必须大于0";
                    }
                    return;
                }
                if (newvalue > oldvalue)
                {
                    e.Valid = false;
                    if (oper == 1) e.ErrorText = "实发件数不能大于本票剩余件数!\r\n\r\n本票当前库存为：" + oldvalue.ToString();
                    if (oper == 2) e.ErrorText = "核销余额不能大于当前总金额：" + oldvalue.ToString();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 数据网格验证失败后弹出提示
        /// </summary>
        /// <param name="e"></param>
        public static void ValidateMessage(DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            if (e.Value.ToString().Trim() == "0" || e.Value.ToString().Trim() == "")
            {
                return;
            }
            XtraMessageBox.Show(e.ErrorText, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 获取系统参数 登录界面加载
        /// </summary>
        /// <returns></returns>
        public static bool QSP_GET_ARG()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARG", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowError("获取系统参数信息失败!");
                    return false;
                }
                SysArgs arg = new SysArgs();
                #region 参数赋值

                arg.Packag = GetArgValue(ds, "Packag");
                arg.PaymentMode = GetArgValue(ds, "PaymentMode");
                arg.TransferMode = GetArgValue(ds, "TransferMode");
                arg.TransitMode = GetArgValue(ds, "TransitMode");
                arg.Varieties = GetArgValue(ds, "Varieties");
                arg.IsRent = GetArgValue(ds, "IsRent");
                arg.AcceptDepart = GetArgValue(ds, "AcceptDepart");
                arg.SettleState = GetArgValue(ds, "SettleState");
                arg.VehicleType = GetArgValue(ds, "VehicleType");
                arg.VehicleLength = GetArgValue(ds, "VehicleLength");
                arg.OperateState = GetArgValue(ds, "OperateState");
                arg.ReceiptBackState = GetArgValue(ds, "ReceiptBackState");
                arg.ReceiptSelectState = GetArgValue(ds, "ReceiptSelectState");
                arg.ReceiptCancelSelectType = GetArgValue(ds, "ReceiptCancelSelectType");
                arg.DeliveryState = GetArgValue(ds, "DeliveryState");
                arg.ReceivingWay = GetArgValue(ds, "ReceivingWay");
                arg.ReceiptRequir = GetArgValue(ds, "ReceiptRequir");
                arg.ReducedWeight = GetArgValue(ds, "ReducedWeight");
                arg.GoodsType = GetArgValue(ds, "GoodsType");
                arg.CustomType = GetArgValue(ds, "CustomType");
                arg.DenominatedType = GetArgValue(ds, "DenominatedType");
                arg.OfenVarieties = GetArgValue(ds, "OfenVarieties");
                arg.StowagePlan = GetArgValue(ds, "StowagePlan");
                arg.SendRequir = GetArgValue(ds, "SendRequir");
                arg.MiddleRequir = GetArgValue(ds, "MiddleRequir");
                arg.CarCooperation = GetArgValue(ds, "CarCooperation");
                arg.ProjectOFHost = GetArgValue(ds, "ProjectOFHost");
                arg.ProjectOend = GetArgValue(ds, "ProjectOend");
                arg.PayMode = GetArgValue(ds, "PayMode");
                arg.CustomTag = GetArgValue(ds, "CustomTag");
                arg.InOutType = GetArgValue(ds, "InOutType");
                arg.LockDate = GetArgValue(ds, "LockDate");
                arg.TotalOtherAcc = GetArgValue(ds, "TotalOtherAcc");
                arg.TotalTransaction = GetArgValue(ds, "TotalTransaction");
                arg.TotalOtherAccOut = GetArgValue(ds, "TotalOtherAccOut");
                arg.TotalTransactionOut = GetArgValue(ds, "TotalTransactionOut");
                arg.BackFeeLessPay = GetArgValue(ds, "BackFeeLessPay");
                arg.BackFeeRate = GetArgValue(ds, "BackFeeRate");
                arg.ContractCheck = GetArgValue(ds, "ContractCheck");
                arg.BuKouFeeType = GetArgValue(ds, "BuKouFeeType");
                arg.gasStation = GetArgValue(ds, "gasStation");
                arg.VerifyDirection = GetArgValue(ds, "VerifyDirection");
                arg.PickUpFreeWeb = GetArgValue(ds, "PickUpFreeWeb");//zaj 2018-5-9
                arg.qysj = GetArgValue(ds, "qysj");
                arg.ChangeBillControl = GetArgValue(ds, "ChangeBillControl");
                arg.CommissionRate = GetArgValue(ds, "CommissionRate");
                arg.SupportValueRate = GetArgValue(ds, "SupportValueRate"); //zb20190828 lms-4769
                arg.SupportValueLowest = GetArgValue(ds, "SupportValueLowest"); //zb20190828 lms-4769
                arg.NotStreet = GetArgValue(ds, "NotStreet");
                #endregion

                CommonClass.Arg = arg;
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("系统参数加载失败：" + ex.Message);
                return false;
            }
        }

        private static string GetArgValue(DataSet ds, string field)
        {
            try
            {
                DataRow[] drs = ds.Tables[0].Select(string.Format("ParamType='{0}'", field));
                if (drs.Length > 0)
                {
                    return drs[0]["ParamValue"].ToString();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("系统参数加载失败：" + ex.Message);
            }
            return null;
        }

        public static void SetSite(ComboBoxEdit cb, bool isall)
        {
            if (dsSite == null || dsSite.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsSite.Tables[0].Rows[i]["SiteName"].ToString()) && !cb.Properties.Items.Contains(dsSite.Tables[0].Rows[i]["SiteName"].ToString()))
                    {
                        cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                    }
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetSite(bool isall, params ComboBoxEdit[] cbs)
        {
            if (dsSite == null || dsSite.Tables.Count == 0 || cbs == null || cbs.Length == 0) return;
            try
            {
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    foreach (ComboBoxEdit cb in cbs)
                        cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                }
                if (isall)
                {
                    foreach (ComboBoxEdit cb in cbs)
                        cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetSite(params CheckedComboBoxEdit[] cbs)
        {
            if (dsSite == null || dsSite.Tables.Count == 0 || cbs == null || cbs.Length == 0) return;
            try
            {
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    foreach (CheckedComboBoxEdit cb in cbs)
                    {
                        if (!string.IsNullOrEmpty(dsSite.Tables[0].Rows[i]["SiteName"].ToString()) && !cb.Properties.Items.Contains(dsSite.Tables[0].Rows[i]["SiteName"].ToString()))
                        {
                            cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static DataRow[] GetWebs(String SiteName, String WebName)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return null;
            DataRow[] dr = null;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and WebName like '%" + WebName + "%'");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
            return dr;
        }


        public static void SetWeb(ComboBoxEdit cb, string SiteName)
        {
            SetWeb(cb, SiteName, true);
        }

        public static void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        /// <summary>
        /// yzw 事业部到网点
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="CauserName"></param>
        /// <param name="isall"></param>
        public static void SetWeb_Cause(ComboBoxEdit cb, string CauserName, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (CauserName == "全部") CauserName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("BelongCause like '" + CauserName + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetCause(ComboBoxEdit cb, bool isall)
        {
            if (dsCause == null || dsCause.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsCause.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsCause.Tables[0].Rows[i]["CauseName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        /// <summary>
        /// zxw 2017-2-15
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="isall"></param>
        public static void SetWeb(ComboBoxEdit cb, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsWeb.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsWeb.Tables[0].Rows[i]["WebName"].ToString()) && !cb.Properties.Items.Contains(dsWeb.Tables[0].Rows[i]["WebName"].ToString()))
                    {
                        cb.Properties.Items.Add(dsWeb.Tables[0].Rows[i]["WebName"]);
                    }
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //LD 2017-12-14
        public static void SetWeb(ComboBoxEdit cb, DataTable dt, bool isall)
        {
            if (dt == null) return;
            try
            {
                if (dt.Rows.Count == 0)
                {
                    cb.Properties.Items.Clear();
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(row["WebName"].ToString()) && !cb.Properties.Items.Contains(row["WebName"].ToString()))
                        {
                            cb.Properties.Items.Add(row["WebName"]);
                        }
                    }
                    if (isall)
                    {
                        cb.Properties.Items.Add("全部");
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetCause(RepositoryItemComboBox cb, bool isall)
        {
            if (dsCause == null || dsCause.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsCause.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsCause.Tables[0].Rows[i]["CauseName"]);
                   
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetArea(ComboBoxEdit cb, string AreaCause)
        {
            SetArea(cb, AreaCause, true);
        }

        public static void SetArea(ComboBoxEdit cb, string AreaCause, bool isall)
        {
            if (dsArea == null || dsArea.Tables.Count == 0) return;
            try
            {
                if (AreaCause == "全部") AreaCause = "%%";
                DataRow[] dr = dsArea.Tables[0].Select("AreaCause like '" + AreaCause + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["AreaName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
                else
                {
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetArea(RepositoryItemComboBox cb, string AreaCause, bool isall)
        {
            if (dsArea == null || dsArea.Tables.Count == 0) return;
            try
            {
                if (AreaCause == "全部") AreaCause = "%%";
                DataRow[] dr = dsArea.Tables[0].Select("AreaCause like '" + AreaCause + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["AreaName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetDep(ComboBoxEdit cb, string DepArea)
        {
            SetDep(cb, DepArea, true);
        }

        public static void SetDep(ComboBoxEdit cb, string DepArea, bool isall)
        {
            if (dsDep == null || dsDep.Tables.Count == 0) return;
            try
            {
                if (DepArea == "全部") DepArea = "%%";
                DataRow[] dr = dsDep.Tables[0].Select("DepArea like '" + DepArea + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["DepName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetUser(ComboBoxEdit cb, string WebName)
        {
            SetUser(cb, WebName, true);
        }

        public static void SetUser(ComboBoxEdit cb, string WebName, bool isall)
        {
            if (dsUsers == null || dsUsers.Tables.Count == 0) return;
            try
            {
                if (WebName == "全部") WebName = "%%";
                DataRow[] dr = dsUsers.Tables[0].Select("WebName like '" + WebName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["UserName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        /// <summary>
        /// 多选下拉框（网点职员）zxw 2017-2-13
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="WebName"></param>
        /// <param name="isall"></param>
        public static void SetUser(CheckedComboBoxEdit cb, string WebName, bool isall)
        {
            if (dsUsers == null || dsUsers.Tables.Count == 0) return;
            try
            {
                if (WebName == "全部") WebName = "%%";
                DataRow[] dr = dsUsers.Tables[0].Select("WebName like '" + WebName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["UserName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetSite(RepositoryItemComboBox cb, bool isall)
        {
            if (dsSite == null || dsSite.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetWeb(RepositoryItemComboBox cb, string SiteName)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                cb.Properties.Items.Add("全部");
                //cb.Text = "全部";
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetWeb(RepositoryItemComboBox cb, string SiteName, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    //cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetWeb(CheckedComboBoxEdit cb, string SiteName, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        /// <summary>
        /// 根据item.Description选中ImageComboBoxEdit
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cbo"></param>
        public static void SetCheckedItems(string value, CheckedComboBoxEdit cc)
        {
            string[] arr1 = value.Split(',');
            foreach (CheckedListBoxItem item in cc.Properties.Items)
            {
                for (int i = 0; i < arr1.Length; i++)
                {
                    if (item.Value.ToString().Trim() == arr1[i].Trim())
                    {
                        item.CheckState = CheckState.Checked;
                        break;
                    }
                }
            }
        }

        public static void SetCauseWeb(ComboBoxEdit cb, string BelongCause, string BelongArea)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (BelongCause == "全部") BelongCause = "%%";
                if (BelongArea == "全部") BelongArea = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("BelongCause like '" + BelongCause + "' and BelongArea like '" + BelongArea + "' ");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                cb.Properties.Items.Add("全部");
                cb.Text = "全部";
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetCauseWeb(RepositoryItemComboBox cb, string BelongCause, string BelongArea)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (BelongCause == "全部") BelongCause = "%%";
                if (BelongArea == "全部") BelongArea = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("BelongCause like '" + BelongCause + "' and BelongArea like '" + BelongArea + "' ");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                cb.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetCauseWeb(ComboBoxEdit cb, string BelongCause, string BelongArea, bool isall)
        {
            if (dsWeb == null || dsWeb.Tables.Count == 0) return;
            try
            {
                if (BelongCause == "全部") BelongCause = "%%";
                if (BelongArea == "全部") BelongArea = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("BelongCause like '" + BelongCause + "' and BelongArea like '" + BelongArea + "' ");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
                else
                {
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static bool BasUpload(DataTable dt, string procName, out string msg)
        {
            bool result = false;
            msg = "上传失败";

            try
            {
                if (procName != "USP_ADD_BASDELIVERYFEE_UPLOAD")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (string.IsNullOrEmpty(dt.Rows[i][0].ToString()) && string.IsNullOrEmpty(dt.Rows[i][1].ToString()))
                            {
                                dt.Rows.Remove(dt.Rows[i]);
                            }
                        }
                    }
                }
                //datatable重新排序
                if (procName == "USP_ADD_BASDELIVERYFEE_UPLOAD")
                {
                    //dt.Columns["Province"].SetOrdinal(0);
                    //dt.Columns["City"].SetOrdinal(1);
                    //dt.Columns["Area"].SetOrdinal(2);
                    //dt.Columns["Street"].SetOrdinal(3);
                    //dt.Columns["MinWeight"].SetOrdinal(4);
                    //dt.Columns["MaxWeight"].SetOrdinal(5);
                    //dt.Columns["MinVolume"].SetOrdinal(6);
                    //dt.Columns["MaxVolume"].SetOrdinal(7);
                    //dt.Columns["DeliveryFee"].SetOrdinal(8);
                    //dt.Columns["TransferMode"].SetOrdinal(9);
                    //dt.Columns["Remark"].SetOrdinal(10);
                }
                //上传二级中转市县
                else if (procName == "USP_ADD_basMiddleSite_UPLOAD")
                {
                    dt.Columns["SiteName"].SetOrdinal(0);
                    dt.Columns["Destination"].SetOrdinal(1);
                    dt.Columns["MiddleProvince"].SetOrdinal(2);
                    dt.Columns["MiddleCity"].SetOrdinal(3);
                    dt.Columns["MiddleArea"].SetOrdinal(4);
                    dt.Columns["MiddleStreet"].SetOrdinal(5);
                    dt.Columns["WebName"].SetOrdinal(6);
                    dt.Columns["MiddleStatus"].SetOrdinal(7);
                    dt.Columns["Type"].SetOrdinal(8);
                    dt.Columns["MiddleLon"].SetOrdinal(9);
                    dt.Columns["MiddleLat"].SetOrdinal(10);
                    dt.Columns["FetchStorageLoca"].SetOrdinal(11);
                    dt.Columns["SendStorageLoca"].SetOrdinal(12);
                    dt.Columns["Ascription"].SetOrdinal(13);
                    dt.Columns["ShengStore"].SetOrdinal(14);
                    dt.Columns["AreaStore"].SetOrdinal(15);
                }
                else if (procName == "USP_ADD_BASFREIGHTFEE_UPLOAD")
                {
                    dt.Columns["StartSite"].SetOrdinal(0);
                    dt.Columns["Province"].SetOrdinal(1);
                    dt.Columns["City"].SetOrdinal(2);
                    dt.Columns["Area"].SetOrdinal(3);
                    dt.Columns["TransferSite"].SetOrdinal(4);
                    dt.Columns["ParcelPriceMin"].SetOrdinal(5);
                    dt.Columns["HeavyPrice"].SetOrdinal(6);
                    dt.Columns["LightPrice"].SetOrdinal(7);
                    dt.Columns["TransitMode"].SetOrdinal(8);
                    dt.Columns["Prescription"].SetOrdinal(9);
                    dt.Columns["Remark"].SetOrdinal(10);
                }
                else if (procName == "USP_ADD_BASCUSTCONTRACT_UPLOAD")
                {
                    dt.Columns["ShortName"].SetOrdinal(0);
                    dt.Columns["FullName"].SetOrdinal(1);
                    dt.Columns["CpyLegalPerson"].SetOrdinal(2);
                    dt.Columns["RegistCapital"].SetOrdinal(3);
                    dt.Columns["RegistAdd"].SetOrdinal(4);
                    dt.Columns["RunAdd"].SetOrdinal(5);
                    dt.Columns["CustLinkName"].SetOrdinal(6);
                    dt.Columns["SendCustTel"].SetOrdinal(7);
                    dt.Columns["SendCustMobile"].SetOrdinal(8);
                    dt.Columns["CheckBillLinkName"].SetOrdinal(9);
                    dt.Columns["CheckBillTel"].SetOrdinal(10);
                    dt.Columns["ContractNo"].SetOrdinal(11);
                    dt.Columns["BeginDate"].SetOrdinal(12);
                    dt.Columns["EndDate"].SetOrdinal(13);
                    dt.Columns["ContractDate"].SetOrdinal(14);
                    dt.Columns["crDate"].SetOrdinal(15);
                    dt.Columns["ApplyName"].SetOrdinal(16);
                    dt.Columns["UnitDeptName"].SetOrdinal(17);
                    dt.Columns["CreditDays"].SetOrdinal(18);
                    dt.Columns["CreditLimit"].SetOrdinal(19);
                    dt.Columns["PayCycle"].SetOrdinal(20);
                    dt.Columns["MonthlyDelayDays"].SetOrdinal(21);
                    dt.Columns["MonthlyDelayLimit"].SetOrdinal(22);
                    dt.Columns["ReturnBillDelayDays"].SetOrdinal(23);
                    dt.Columns["Operator"].SetOrdinal(24);
                    dt.Columns["CustTypeValue"].SetOrdinal(25);
                    dt.Columns["MonthSiteName"].SetOrdinal(26);
                    dt.Columns["MonthWebName"].SetOrdinal(27);

                    dt.Columns["CheckBillLinkQQ"].SetOrdinal(28);
                    dt.Columns["CheckBillLinkWeChat"].SetOrdinal(29);
                    dt.Columns["CheckBillLinkEmail"].SetOrdinal(30);
                    dt.Columns["Tax"].SetOrdinal(31);
                    dt.Columns["Punctuate"].SetOrdinal(32);
                    dt.Columns["Paymentcontact"].SetOrdinal(33);
                    dt.Columns["Paymentcontactphone"].SetOrdinal(34);
                    dt.Columns["Serviceresponsibler"].SetOrdinal(35);
                    dt.Columns["Returnrequest"].SetOrdinal(36);
                    dt.Columns["Invoice"].SetOrdinal(37);
                }
                else if (procName == "USP_ADD_basDeliveryFeeHK")
                {
                    dt.Columns["Province"].SetOrdinal(0);
                    dt.Columns["City"].SetOrdinal(1);
                    dt.Columns["Area"].SetOrdinal(2);
                    dt.Columns["Street"].SetOrdinal(3);
                    dt.Columns["Remark"].SetOrdinal(4);
                    dt.Columns["ExpressionName"].SetOrdinal(5);
                    dt.Columns["Additional"].SetOrdinal(6);
                    dt.Columns["kilometre"].SetOrdinal(7);
                }
                else if (procName == "USP_ADD_TIAOZHANG_UPLOAD")
                {
                    //dt.Columns["BillNo"].DataType = Type.GetType("Systerm.String");
                    dt.Columns["InOrOut"].SetOrdinal(0);
                    dt.Columns["Project"].SetOrdinal(1);
                    dt.Columns["FeeType"].SetOrdinal(2);
                    dt.Columns["Account"].SetOrdinal(3);
                    dt.Columns["ToMan"].SetOrdinal(4);
                    dt.Columns["Remark"].SetOrdinal(5);
                    dt.Columns["BillNo"].SetOrdinal(6);
                }
                else
                {
                    if (!dt.Columns.Contains("ToSite"))
                    {
                        dt.Columns.Add("ToSite", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToProvince"))
                    {
                        dt.Columns.Add("ToProvince", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToCity"))
                    {
                        dt.Columns.Add("ToCity", typeof(string));
                    }
                    if (!dt.Columns.Contains("ToArea"))
                    {
                        dt.Columns.Add("ToArea", typeof(string));
                    }
                    if (!dt.Columns.Contains("TransportMode"))
                    {
                        dt.Columns.Add("TransportMode", typeof(string));
                    }
                    if (!dt.Columns.Contains("TransferSite"))
                    {
                        dt.Columns.Add("TransferSite", typeof(string));
                    }
                    if (!dt.Columns.Contains("FromSite"))
                    {
                        dt.Columns.Add("FromSite", typeof(string));
                    }
                    dt.Columns["FromSite"].SetOrdinal(0);
                    dt.Columns["TransferSite"].SetOrdinal(1);
                    dt.Columns["ToSite"].SetOrdinal(2);
                    dt.Columns["TransportMode"].SetOrdinal(3);
                    dt.Columns["HeavyPrice"].SetOrdinal(4);
                    dt.Columns["LightPrice"].SetOrdinal(5);
                    dt.Columns["ParcelPriceMin"].SetOrdinal(6);
                    dt.Columns["Remark"].SetOrdinal(7);
                    dt.Columns["ToProvince"].SetOrdinal(8);
                    dt.Columns["ToCity"].SetOrdinal(9);
                    dt.Columns["ToArea"].SetOrdinal(10);
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Tb", dt));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, procName, list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    msg = "上传成功";
                    result = true;
                }
            }
            catch (Exception e)
            {
                msg = "上传失败," + e.Message;
            }
            return result;
        }

        public static void Create_BarEditItem_Web(BarManager barManager1, Bar bar1, BarEditItem barEditItem)
        {
            try
            {
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();

                barManager1.BeginInit();
                repositoryItemComboBox.BeginInit();

                barEditItem.Caption = "网点";
                barEditItem.Edit = repositoryItemComboBox;

                repositoryItemComboBox.AutoHeight = false;
                repositoryItemComboBox.Sorted = true;
                repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                repositoryItemComboBox.DropDownRows = 10;
                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

                bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

                if (barManager1.Items.Equals(barEditItem))
                    barManager1.Items.Remove(barEditItem);
                else
                    barManager1.Items.Add(barEditItem);

                barManager1.RepositoryItems.Add(repositoryItemComboBox);

                barManager1.EndInit();
                repositoryItemComboBox.EndInit();

                //FillRepWebByParent(repositoryItemComboBox, site, true);
            }
            catch (Exception)
            {
                //commonclass.MsgBox.ShowYesNo(ex.Message);
            }
            finally
            {
                barEditItem.EditValue = "全部";
            }
        }

        public static void Create_BarEditItem_Area(BarManager barManager1, Bar bar1, BarEditItem barEditItem)
        {
            try
            {
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();

                barManager1.BeginInit();
                repositoryItemComboBox.BeginInit();

                barEditItem.Caption = "大区";
                barEditItem.Edit = repositoryItemComboBox;

                repositoryItemComboBox.AutoHeight = false;
                repositoryItemComboBox.Sorted = true;
                repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                repositoryItemComboBox.DropDownRows = 10;
                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

                bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

                if (barManager1.Items.Equals(barEditItem))
                    barManager1.Items.Remove(barEditItem);
                else
                    barManager1.Items.Add(barEditItem);

                barManager1.RepositoryItems.Add(repositoryItemComboBox);

                barManager1.EndInit();
                repositoryItemComboBox.EndInit();

                //FillRepWebByParent(repositoryItemComboBox, site, true);
            }
            catch (Exception)
            {
                //commonclass.MsgBox.ShowYesNo(ex.Message);
            }
            finally
            {
                barEditItem.EditValue = "全部";
            }
        }
        public static void Create_BarEditItem_WhoFee(BarManager barManager1, Bar bar1, BarEditItem barEditItem)
        {
          
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();

                barManager1.BeginInit();
                repositoryItemComboBox.BeginInit();

                barEditItem.Caption = "付款方";
                barEditItem.Edit = repositoryItemComboBox;

                repositoryItemComboBox.AutoHeight = false;
                repositoryItemComboBox.Sorted = true;
                repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                repositoryItemComboBox.DropDownRows = 10;
                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

                bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

                if (barManager1.Items.Equals(barEditItem))
                { barManager1.Items.Remove(barEditItem); }
                else
                {
                    barManager1.Items.Add(barEditItem);

                    barManager1.RepositoryItems.Add(repositoryItemComboBox);

                    barManager1.EndInit();
                    repositoryItemComboBox.EndInit();
                }
                repositoryItemComboBox.Properties.Items.Add("发货方");
                repositoryItemComboBox.Properties.Items.Add("收货方");
                repositoryItemComboBox.Properties.Items.Add("全部");
                barEditItem.EditValue = "全部";
               
            
            
        }
        public static void Create_BarEditItem_Cause(BarManager barManager1, Bar bar1, BarEditItem barEditItem)
        {
            try
            {
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();

                barManager1.BeginInit();
                repositoryItemComboBox.BeginInit();

                barEditItem.Caption = "事业部";
                barEditItem.Edit = repositoryItemComboBox;

                repositoryItemComboBox.AutoHeight = false;
                repositoryItemComboBox.Sorted = true;
                repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                repositoryItemComboBox.DropDownRows = 10;
                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

                bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

                if (barManager1.Items.Equals(barEditItem))
                    barManager1.Items.Remove(barEditItem);
                else
                    barManager1.Items.Add(barEditItem);

                barManager1.RepositoryItems.Add(repositoryItemComboBox);

                barManager1.EndInit();
                repositoryItemComboBox.EndInit();
                SetCause(repositoryItemComboBox, true);
            }
            catch (Exception)
            {
                //commonclass.MsgBox.ShowYesNo(ex.Message);
            }
            finally
            {
                barEditItem.EditValue = "全部";
            }
        }
        public static void Create_BarEditItem_TransferSite(BarManager barManager1, Bar bar1, BarEditItem barEditItem)  //maohui20180306
        {
            try
            {
                RepositoryItemComboBox repositoryItemComboBox = new RepositoryItemComboBox();

                barManager1.BeginInit();
                repositoryItemComboBox.BeginInit();

                barEditItem.Caption = "中转地";
                barEditItem.Edit = repositoryItemComboBox;

                repositoryItemComboBox.AutoHeight = false;
                repositoryItemComboBox.Sorted = true;
                repositoryItemComboBox.Buttons.Add(new EditorButton(ButtonPredefines.Combo));
                repositoryItemComboBox.DropDownRows = 10;
                repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;

                bar1.LinksPersistInfo.Insert(0, new LinkPersistInfo(((BarLinkUserDefines)((BarLinkUserDefines.PaintStyle | BarLinkUserDefines.Width))), barEditItem, "", false, true, true, 88, null, BarItemPaintStyle.CaptionGlyph));

                if (barManager1.Items.Equals(barEditItem))
                    barManager1.Items.Remove(barEditItem);
                else
                    barManager1.Items.Add(barEditItem);

                barManager1.RepositoryItems.Add(repositoryItemComboBox);

                barManager1.EndInit();
                repositoryItemComboBox.EndInit();
                SetSite(repositoryItemComboBox, true);
            }
            catch (Exception)
            {
                //commonclass.MsgBox.ShowYesNo(ex.Message);
            }
            finally
            {
                barEditItem.EditValue = "全部";
            }
        }
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        public static void GetServerDate()
        {
            Thread tt = new Thread(new ThreadStart(delegate() { ServerDate = SqlHelper.GetServerDate(); }));
            tt.IsBackground = true;
            tt.Start();
        }

        /// <summary>
        /// 0:00:00
        /// </summary>
        public static DateTime gbdate
        {
            get { return ServerDate.Date; }
        }
        /// <summary>
        /// 23:59:59
        /// </summary>
        public static DateTime gedate
        {
            get
            {
                return ServerDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            }
        }

        public static DateTime fetchBDate
        {
            get
            {
                return ServerDate.Date.AddDays(-1).AddHours(18).AddMinutes(00).AddSeconds(00);
            }
        }
        public static DateTime fetchEDate
        {
            get
            {
                return ServerDate.Date.AddHours(17).AddMinutes(59).AddSeconds(59);
            }
        }
        public static DateTime fetchTXDate
        {
            get
            {
                return ServerDate.Date.AddHours(7).AddMinutes(59).AddSeconds(59);
            }
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        public static DateTime gcdate
        {
            get
            {
                return ServerDate;
            }
        }

        private static DateTime _ServerDate;
        /// <summary>
        /// 服务器时间
        /// </summary>
        public static DateTime ServerDate
        {
            get
            {
                try
                {
                    if (_ServerDate.Year == 1)
                    {
                        return DateTime.Now;
                    }
                    return _ServerDate;
                }
                catch (Exception) { return DateTime.Now; }
            }
            set
            {
                try
                {
                    _ServerDate = value;
                }
                catch (Exception) { _ServerDate = DateTime.Now; }
            }
        }

        /// <summary>
        /// 行政区划管理类
        /// </summary>
        public static class AreaManager
        {
            private static DataSet dsArea;

            /// <summary>
            /// 行政区划
            /// </summary>
            public static DataSet DsArea
            {
                get
                {
                    if (dsArea == null)
                    {
                        try
                        {

                            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEREGION");
                            DataSet ds = SqlHelper.GetDataSet(sps);
                            dsArea = ds;
                        }
                        catch (Exception ex)
                        {
                            MsgBox.ShowException(ex);
                        }
                    }
                    return dsArea;
                }
                set { dsArea = value; }
            }

            /// <summary>
            /// 绑定行政区(四级地址)
            /// </summary>
            /// <param name="edit">ImageComboBoxEdit控件</param>
            /// <param name="prentid">Parent.Properties.Items[MiddleProvince.SelectedIndex].Value 填充省:传0</param>
            public static void FillCityToImageComBoxEdit(ImageComboBoxEdit edit, object prentid)
            {
                if (prentid == null || prentid.ToString() == "")
                {
                    prentid = "-1";
                }
                edit.SelectedIndex = -1;
                if (DsArea != null && dsArea.Tables.Count > 0)
                {
                    edit.Properties.Items.Clear();
                    DataRow[] drs = DsArea.Tables[0].Select("ParentID=" + prentid.ToString() + "");
                    ImageComboBoxItem item;
                    List<ImageComboBoxItem> list = new List<ImageComboBoxItem>();
                    for (int i = 0; i < drs.Length; i++)
                    {
                        item = new ImageComboBoxItem(drs[i]["RegionName"].ToString(), drs[i]["RegionID"]);
                        if (!list.Contains(item))
                        {
                            list.Add(item);
                        }
                    }
                    edit.Properties.Items.AddRange(list);
                }
            }
        }

        /// <summary>
        /// 根据item.Description选中ImageComboBoxEdit
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cbo"></param>
        public static void SetSelectIndex(string value, ImageComboBoxEdit cbo)
        {
            foreach (ImageComboBoxItem item in cbo.Properties.Items)
            {
                if (item.Description == value)
                {
                    cbo.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// 根据item.Description选中ImageComboBoxEdit
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cbo"></param>
        public static void SetSelectIndexByValue(string value, ImageComboBoxEdit cbo)
        {
            foreach (ImageComboBoxItem item in cbo.Properties.Items)
            {
                if (item.Description == value || item.Value.ToString() == value)
                {
                    cbo.SelectedItem = item;
                    return;
                }
            }
        }

        /// <summary>
        /// 加入修改日志
        /// </summary>
        public static void SetOperLog(string sBatch, string sBillNo, string sAccount, string sOperMan, string sOperType, string sContent)
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("Batch", sBatch));
                //list.Add(new SqlPara("BillNo", sBillNo));
                //list.Add(new SqlPara("Account", ConvertType.ToFloat(sAccount)));
                ////list.Add(new SqlPara("OperDate", OperDate.));
                //list.Add(new SqlPara("OperMan", sOperMan));
                //list.Add(new SqlPara("OperType", sOperType));
                //list.Add(new SqlPara("Content", sContent));

                //SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_OPERLOG", list);
                //if (SqlHelper.ExecteNonQuery(sps) > 0)
                //{
                //    MsgBox.ShowOK();
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 检查控货,返回true表示存在控货
        /// </summary>
        /// <param name="gv">要检查的风格</param>
        /// <param name="type">1检查整个网格;2检查选中行</param>
        /// <returns></returns>
        public static bool CheckKongHuo(GridView gv, int type)
        {
            if (gv == null) return true;
            int i = 0;
            if (type == 1)
            {
                for (i = 0; i < gv.RowCount; i++)
                {
                    if (ConvertType.ToInt32(gv.GetRowCellValue(i, "NoticeState")) == 1) return true;
                }
            }
            else
            {
                int[] rows = gv.GetSelectedRows();
                for (i = 0; i < rows.Length; i++)
                {
                    if (ConvertType.ToInt32(gv.GetRowCellValue(rows[i], "NoticeState")) == 1) return true;
                }
            }
            return false;
        }

        #region 取客户IP
        public static void GetLocalIp(ref string hostname, ref string localip, ref string mac)
        {
            // 此方法获取的地址也可能是包含虚拟机的IP
            #region
            //IPAddress[] AddressList = Dns.GetHostAddresses(Dns.GetHostName());
            //foreach (IPAddress ip in AddressList)
            //{
            //    //根据AddressFamily判断是否为ipv4,如果是InterNetWorkV6则为ipv6
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //        localip = ip.ToString();
            //}
            #endregion

            hostname = Dns.GetHostName().ToString();

            try
            {
                System.Management.ManagementObjectSearcher query = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                System.Management.ManagementObjectCollection queryCollection = query.Get();
                string[] ips;
                string sname = "";
                foreach (System.Management.ManagementObject mo in queryCollection)
                {
                    if (!(bool)mo["IPEnabled"]) continue;
                    sname = mo["ServiceName"] as string;

                    if (sname.ToLower().Contains("vmnetadapter") || sname.ToLower().Contains("ppoe") || sname.ToLower().Contains("bthpan") || sname.ToLower().Contains("tapvpn") || sname.ToLower().Contains("ndisip") || sname.ToLower().Contains("sinforvnic"))
                    { continue; }

                    ips = mo["IPAddress"] as string[];
                    IPAddress addr;
                    foreach (string item in ips)
                    {
                        addr = IPAddress.Parse(item);
                        if (addr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localip = item;
                            mac = mo["MacAddress"].ToString();
                            return;
                        }
                    }
                }
            }
            catch (Exception) { }
        }
        #endregion

        #region  取客户路由器地址
        public static string GetPulicAddress()
        {
            string ip = "", html = "";
            //try
            //{
            //    using (WebClient web = new WebClient())
            //    {
            //        html = web.DownloadString("https://ipip.yy.com/get_ip_info.php");
            //        string str = "var returnInfo = {\"cip\":\"";
            //        if (html.Contains(str))
            //        {
            //            html = html.Replace(str, "");
            //            int index = html.IndexOf("\"");
            //            if (index >= 0)
            //            {
            //                ip = html.Substring(0, index);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string s = ex.Message;
            //}
            return ip;
        }
        #endregion

        /// <summary>
        /// 根据登录数据库，连接指定URL
        /// </summary>
        /// <returns></returns>
        public static DataTable GetDatabaseInfo()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("db", typeof(string)); //要连接的数据库环境
            dt.Columns.Add("url", typeof(string));//接口页面
            dt.Columns.Add("localdb", typeof(string));//本地数据库文件名

            dt.Rows.Add("ZQTMS20160713", "LeyService/CoreRun", "Client.dll");
            dt.Rows.Add("ZQTMSLEY", "LeyService/CoreRun", "Client.dll");

            return dt;
        }

        /// <summary>
        /// QSP_LOCK_1
        /// <para>BillNo</para>
        /// </summary>
        public static bool QSP_LOCK_1(string BillNo, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);
                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// QSP_LOCK_2
        /// <para>inoneflag</para>
        /// </summary>
        public static bool QSP_LOCK_2(string inoneflag, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);

                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// QSP_LOCK_3
        /// <para>DeliCode</para>
        /// </summary>
        public static bool QSP_LOCK_3(string DeliCode, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);

                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// QSP_LOCK_4
        /// <para>BillNO</para>
        /// </summary>
        public static bool QSP_LOCK_4(string BillNO, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);

                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// QSP_LOCK_5
        /// <para>BillNO</para>
        /// </summary>
        public static bool QSP_LOCK_5(string BillNO, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);

                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// QSP_LOCK_6
        /// <para>BillNO</para>
        /// </summary>
        public static bool QSP_LOCK_6(string BillNO, string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("81")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.LockDate);
                DateTime date2 = Convert.ToDateTime(BillDate);

                if (date1 > date2)
                {
                    MsgBox.ShowOK("本运单已锁定，无法做任何修改");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// 汽运锁账
        /// <para>BillNO</para>
        /// </summary>
        public static bool QSP_LOCK_7(string BillDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("82")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.qysj);
                DateTime date2 = Convert.ToDateTime(BillDate);
                if (date1 > date2)
                {
                    MsgBox.ShowOK("财务已锁账，如需调整，请联系相关人员解锁!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// 改单锁定
        /// lhd 20180604
        /// </summary>
        /// <param name="billDate"></param>
        /// <returns></returns>
        public static bool QSP_LOCK_8(string billDate)
        {
            bool locks = false;
            try
            {
                if (UserRight.GetRight("83")) return false;
                DateTime date1 = Convert.ToDateTime(CommonClass.Arg.ChangeBillControl);
                DateTime date2 = Convert.ToDateTime(billDate);
                if (date2 < date1)
                {
                    MsgBox.ShowOK("运单已锁定，无法改单!");
                    return true;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return locks;
        }

        /// <summary>
        /// 检测账户余额
        /// </summary>
        public static decimal GetWebAcc(string AccountName)
        {
            decimal acc = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", AccountName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_WebAcc", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return acc;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    acc = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["AccountBalance"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return acc;
        }

        /// <summary>
        /// 获取标签文件名
        /// </summary>
        public static string GetLabelFileName
        {
            get
            {
                switch (UserInfo.WebName)
                {
                    case "麦克维尔客户部":
                        return "麦克维尔客户部标签.repx";
                    default:
                        return "标签.repx";
                }
            }
        }
        //zaj 2017-11-23
        /// <summary>
        /// 获取标签文件名（新）
        /// </summary>
        public static string GetLabelNameNew
        {
            get
            {
                if (UserInfo.LabelName == "")
                {
                    return "标签.repx";
                }
                else
                {
                    return UserInfo.LabelName + ".repx";
                }

            }

        }

        /// <summary>
        /// 根据网格列生成过程参数
        /// </summary>
        /// <param name="gv">网格</param>
        /// <returns></returns>
        public static List<SqlPara> GetParaList(GridView gv)
        {
            gv.PostEditor();
            List<SqlPara> list = new List<SqlPara>();
            if (gv == null || gv.Columns.Count == 0 || gv.RowCount == 0) return list;

            Dictionary<string, string> dicCol = new Dictionary<string, string>();
            //将列添加到字典中
            foreach (GridColumn gc in gv.Columns)
            {
                if (gc.FieldName == "rowid") continue;
                if (!dicCol.ContainsKey(gc.FieldName)) dicCol.Add(gc.FieldName, "");
            }
            if (dicCol.Count == 0) return list;

            for (int i = 0; i < gv.RowCount; i++)
            {
                foreach (GridColumn col in gv.Columns)
                {
                    dicCol[col.FieldName] += GridOper.GetRowCellValueString(gv, i, col.FieldName).Replace('@', '_') + "@";
                }
            }
            foreach (KeyValuePair<string, string> item in dicCol)
            {
                list.Add(new SqlPara(item.Key, item.Value));
            }
            return list;
        }

        /// <summary>
        /// 检查空值,并且只有一行,如果是就删除
        /// </summary>
        /// <returns></returns>
        public static DataSet CheckDataSetNullRow(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return ds;
            bool flag = false;
            foreach (DataTable dt in ds.Tables)
            {
                if (dt == null || dt.Rows.Count != 1) continue;
                flag = false;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    //如果不为空就跳过
                    if (dt.Rows[0][i] != DBNull.Value)
                    {
                        flag = true;
                        break;//跳出,没必要继续走
                    }
                }
                if (!flag)
                {
                    dt.Rows.RemoveAt(0);
                }
            }
            return ds;
        }

        /// <summary>
        /// 加载所有公司ID
        /// </summary>
        /// <param name="com"></param>
        public static void GetCompanyId(DevExpress.XtraEditors.PopupBaseEdit com)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit cbe = null;
                if (com is DevExpress.XtraEditors.ComboBoxEdit)
                {
                    cbe = com as DevExpress.XtraEditors.ComboBoxEdit;
                }
                DevExpress.XtraEditors.CheckedComboBoxEdit ckCbe = null;
                if (com is DevExpress.XtraEditors.CheckedComboBoxEdit)
                {
                    ckCbe = com as DevExpress.XtraEditors.CheckedComboBoxEdit;
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (cbe != null)
                    {
                        cbe.Properties.Items.Add(dr[0]);
                    }
                    else if (ckCbe != null)
                    {
                        ckCbe.Properties.Items.Add(dr[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        public static void SetSite(TextEdit StartSite, bool p)
        {
            throw new NotImplementedException();
        }

        public static void GetGridViewColumns(GridView gridView1)
        {
            throw new NotImplementedException();
        }

        public static string smsid = "";
        public static string smsuserid = "";
        public static string smspassword = "";
        public static string smsCompanyName = "";
        /// <summary>
        /// 加载所有公司短信发送账号密码  hj20181102
        /// </summary>
        /// <param name="com"></param>
        public static void GetCompanySms()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SmsInfo");
                dsSms = SqlHelper.GetDataSet(sps);
                if (dsSms == null || dsSms.Tables.Count == 0 || dsSms.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                else
                {
                    smsid = Convert.ToString(dsSms.Tables[0].Rows[0]["smsid"]);
                    smsuserid = Convert.ToString(dsSms.Tables[0].Rows[0]["smsuserid"]);
                    smspassword = Convert.ToString(dsSms.Tables[0].Rows[0]["smspassword"]);
                    smsCompanyName = Convert.ToString(dsSms.Tables[0].Rows[0]["smsCompanyName"]);//公司名称
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// DES数据加密
        /// </summary>
        /// <param name="targetValue">目标值</param>
        /// <param name="key">密钥</param>
        /// <returns>加密值</returns>
        public static string Encrypt(string targetValue, string key)
        {
            if (string.IsNullOrEmpty(targetValue))
            {
                return string.Empty;
            }

            var returnValue = new StringBuilder();
            var des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(targetValue);
            // 通过两次哈希密码设置对称算法的初始化向量   
            des.Key = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
                                                    (FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5").
                                                        Substring(0, 8), "sha1").Substring(0, 8));
            // 通过两次哈希密码设置算法的机密密钥   
            des.IV = Encoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile
                                                    (FormsAuthentication.HashPasswordForStoringInConfigFile(key, "md5")
                                                        .Substring(0, 8), "md5").Substring(0, 8));
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            foreach (byte b in ms.ToArray())
            {
                returnValue.AppendFormat("{0:X2}", b);
            }
            return returnValue.ToString();
        }

        //获取ZQTMS和LMS所有的站点
        public static void GetSitaNameZQTMS()//maohui20181101
        {
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_SitaNameZQTMS");
            dsSiteZQTMS = SqlHelper.GetDataSet(spe);
        }

        //获取ZQTMS和LMS所有的网点
        public static void GetWebNameZQTMS()//maohui20181101
        {
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_ShortWebZQTMS");
            dsWebZQTMS = SqlHelper.GetDataSet(spe);
        }

        public static void SetSiteZQTMS(ComboBoxEdit cb, bool isall)//maohui20181101
        {
            if (dsSiteZQTMS == null || dsSiteZQTMS.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < dsSiteZQTMS.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsSiteZQTMS.Tables[0].Rows[i]["SiteName"].ToString()) && !cb.Properties.Items.Contains(dsSiteZQTMS.Tables[0].Rows[i]["SiteName"].ToString()))
                    {
                        cb.Properties.Items.Add(dsSiteZQTMS.Tables[0].Rows[i]["SiteName"]);
                    }
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="password">待加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();
        }

        //是否是战区公司
        public static bool IsZhanQuCompanyId(string strCompanyid) 
        {
            if ("266@268@288@299@300@301@".Contains(strCompanyid))
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 打开每个界面插入日志
        /// </summary>
        /// <param name="MenuName"></param>
        public static void InsertLog(string MenuName)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MenuName", MenuName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_InsertLog", list);
                SqlHelper.ExecteNonQuery(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 获取短驳之前总的结算费用
        /// </summary>  zb20191224
        /// <param name="webname"></param>
        /// <returns></returns>
        public static decimal GetTotalMoney(string webname)
        {
            decimal totalmoney = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webname",webname));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCOUNT_BIllSTATE_ByWebName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0) return 0;
                totalmoney= ConvertType.ToDecimal(ds.Tables[0].Rows[0]["heji"].ToString());
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            return totalmoney;
        }

        /// <summary>
        /// 检测账户余额 zb20191224
        /// </summary>
        public static decimal GetWebAcc_ByWebName(string AccountName)
        {
            decimal acc = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", AccountName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_WebAcc_By_WebName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return acc;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    acc = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["AccountBalance"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return acc;
        }

       
        /// <summary>
        /// 检查窗口是否已经打开 gy20200306
        /// </summary>
        /// <param name="asFormName">窗口名称</param>
        /// <returns></returns>
        public static bool CheckFormIsOpen(string asFormName)
        {
            bool bResult = false;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == asFormName)
                {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }

    }
}