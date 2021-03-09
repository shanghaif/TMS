using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using System.Reflection;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmRightGroupTree : BaseForm
    {
        public frmRightGroupTree()
        {
            InitializeComponent();
        }
        DataSet dsmenu = new DataSet();

        /// <summary>
        /// 不需要设置权限的工具栏菜单
        /// </summary>
        List<string> listLink = new List<string>() { "关闭", "退出", "刷新", "快找", "外观设置", "过滤器", "3", "4", "7", "8" };

        private void frmRightGroupAdd_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("权限树管理");//xj/2019/5/29
            BarMagagerOper.SetBarPropertity(bar1);
            GetAllInfo();
        }

        private void GetAllInfo()
        {
            if (!GetMenuData())
            {
                return;
            }
            if (!GetSubMenu(treeList1.Nodes))
            {
                return;
            }
            dsmenu.Tables[0].Merge(dsmenu.Tables[1]);
        }

        private bool GetMenuData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                //SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_GET_RigthGroupTree", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RigthGroupTreeKT", list);//zaj

                dsmenu = SqlHelper.GetDataSet(sps);

                if (dsmenu == null || dsmenu.Tables.Count == 0) return true;
                treeList1.DataSource = dsmenu.Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("菜单加载失败：\r\n" + ex.Message);
                return false;
            }
        }

        private bool GetSubMenu(TreeListNodes nodes)
        {
            try
            {
                DataTable dt = treeList1.DataSource as DataTable;
                foreach (TreeListNode node in nodes)
                {
                    if (node.HasChildren)
                    {
                        GetSubMenu(node.Nodes);
                    }
                    string dllPath = node.GetValue("DllPath").ToString();
                    string dllName = node.GetValue("DllName").ToString();
                    string nameSpace = node.GetValue("FormNameSpace").ToString();
                    string assName = node.GetValue("FormClass").ToString();

                    #region 基本判断
                    if (nameSpace.Trim() == "")
                    {
                        continue;//说明该菜单有子目录
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
                        //MsgBox.ShowError("程序集文件不存在!");
                        //return;
                        continue;
                    }
                    Assembly ass = Assembly.LoadFrom(dllpath);
                    if (ass == null)
                    {
                        //MsgBox.ShowError("程序集文件加载失败！");
                        //return;
                        continue;
                    }

                    Type type = ass.GetType(string.Format("{0}.{1}", nameSpace, assName));
                    if (type == null)
                    {
                        //MsgBox.ShowError("程序类加载错误！");
                        //return;
                        continue;
                    }
                    #endregion

                    Form frm = null;
                    try
                    {
                        frm = (Form)Activator.CreateInstance(type);
                    }
                    catch (Exception)
                    {
                        //MsgBox.ShowError("页面打开失败：" + type.FullName);
                        continue;
                    }

                    List<FormBar> list = new List<FormBar>();
                    GetBars(frm, list);

                    DataRowView dv = treeList1.GetDataRecordByNode(node) as DataRowView;
                    int id = 11;
                    foreach (FormBar fb in list)
                    {
                        foreach (LinkPersistInfo link in fb.Bar.LinksPersistInfo)
                        {
                            if (listLink.Contains(link.Item.Caption.Trim())) continue;
                            if (link.Item.GetType() == typeof(BarStaticItem) || link.Item.GetType() == typeof(BarEditItem)) continue;
                            DataRow dr = dt.NewRow();
                            dr["MGuid"] = Guid.NewGuid().ToString();
                            dr["ID"] = dv["ID"].ToString() + id;
                            dr["ParentID"] = dv["ID"];
                            dr["MenuName"] = (fb.TabPage == null ? "" : fb.TabPage.Text.Trim() + "：") + link.Item.Caption;
                            dr["ComeFrom"] = 2;
                            dt.Rows.Add(dr);
                            id++;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("菜单工具栏加载失败：\r\n" + ex.Message);
                return false;
            }
        }

        private void GetBarManager(Control con, List<BarManager> list)
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

        private void GetBars(Control con, List<FormBar> listBar)
        {
            List<BarManager> list = new List<BarManager>();
            GetBarManager(con, list);
            foreach (BarManager manager in list)
            {
                if (manager == null) continue;
                XtraTabPage tab = manager.Form.GetType() == typeof(XtraTabPage) ? ((XtraTabPage)manager.Form) : null;
                foreach (Bar bar in manager.Bars)
                {
                    FormBar fb = new FormBar(tab, bar);
                    listBar.Add(fb);
                }
            }
        }

        private class FormBar
        {
            /// <summary>
            /// 工具栏(bar)所在的容器
            /// </summary>
            public XtraTabPage TabPage
            {
                get;
                set;
            }

            /// <summary>
            /// 工具栏
            /// </summary>
            public Bar Bar
            {
                get;
                set;
            }

            public FormBar(XtraTabPage TabPage, Bar Bar)
            {
                this.TabPage = TabPage;
                this.Bar = Bar;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeListNode node = treeList1.Nodes.LastNode;
            MGuid.Text = Guid.NewGuid().ToString();
            ucLabelBox1.Text = "";
            ParentID.Text = "";
            MenuName.Text = "";
            MenuName.Focus();
            //if (node == null)
            //{
            //    ID.EditValue = 11;
            //    return;
            //}
            //ID.EditValue = Convert.ToInt32(node.GetValue("ID")) + 1;
            ID.EditValue = 81;//防止中间添加菜单
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeListNode node = treeList1.FocusedNode;
            if (node == null)
            {
                MsgBox.ShowOK("没有选择菜单，无法添加下级菜单!");
                return;
            }
            int ComeFrom = 0; //1主菜单；2工具栏；3手工增加
            if (node.LastNode != null)
            {
                ComeFrom = ConvertType.ToInt32(node.LastNode.GetValue("ComeFrom"));
            }
            if (ComeFrom < 2) //1或2才能继续添加
            {
                MsgBox.ShowOK("只有页面主菜单才能单独添加特殊权限!");
                return;
            }
            MGuid.Text = Guid.NewGuid().ToString();
            ucLabelBox1.Text = node.GetValue("MenuName").ToString();
            ParentID.EditValue = node.GetValue("ID");

            if (node.HasChildren)
            {
                if (ComeFrom == 3)
                {
                    ID.EditValue = Convert.ToInt32(node.LastNode.GetValue("ID")) + 1;
                }
                else
                {
                    ID.EditValue = Convert.ToInt32(node.GetValue("ID") + "80") + 1;
                }
            }
            else
            {
                ID.EditValue = Convert.ToInt32(node.GetValue("ID") + "80") + 1;
            }
            MenuName.Text = "";
            MenuName.Focus();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                DataRowView dr = treeList1.GetDataRecordByNode(node) as DataRowView;

                int ComeFrom = Convert.ToInt32(dr["ComeFrom"]); //1主菜单；2工具栏；3手工增加
                if (ComeFrom < 3) //3才能修改
                {
                    MsgBox.ShowOK("只能修改手工添加的特殊权限!");
                    return;
                }

                MGuid.EditValue = dr["MGuid"];
                ucLabelBox1.Text = node.ParentNode == null ? "" : node.ParentNode.GetValue("MenuName").ToString();
                ParentID.EditValue = dr["ParentID"];
                ID.EditValue = dr["ID"];
                MenuName.EditValue = dr["MenuName"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                int ComeFrom = Convert.ToInt32(node.GetValue("ComeFrom")); //1主菜单；2工具栏；3手工增加
                if (ComeFrom != 3) //3才能删除
                {
                    MsgBox.ShowOK("只能删除手工添加的特殊权限!");
                    return;
                }

                if (MsgBox.ShowYesNo("是否删除？同时将删除对应的子菜单！\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                Guid guid = new Guid(node.GetValue("MGuid").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MGuid", guid));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_RightGroupTree", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    treeList1.PostEditor();
                    treeList1.DeleteNode(node);
                    dsmenu.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetAllInfo();
        }

        private void AddRow(DataRow dr, string field, object value)
        {
            if (dr.Table.Columns.Contains(field))
            {
                dr[field] = value;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                #region 基本信息监测
                string _MGuid = MGuid.Text.Trim();
                if (_MGuid == "")
                {
                    MsgBox.ShowOK("MGuid值不能为空!");
                    MGuid.Focus();
                    return;
                }

                int _ID = ID.Text.Trim() == "" ? 0 : Convert.ToInt32(ID.Text.Trim());
                if (_ID == 0)
                {
                    MsgBox.ShowOK("菜单编号值不能为空!");
                    ID.Focus();
                    return;
                }

                int _ParentID = ParentID.Text.Trim() == "" ? 0 : Convert.ToInt32(ParentID.Text.Trim());

                string _MenuName = MenuName.Text.Trim();
                if (_MenuName == "")
                {
                    MsgBox.ShowOK("菜单名称值不能为空!");
                    MenuName.Focus();
                    return;
                }

                int _ComeFrom = 3;
                #endregion

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MGuid", _MGuid));
                list.Add(new SqlPara("ID", _ID));
                list.Add(new SqlPara("ParentID", _ParentID));
                list.Add(new SqlPara("MenuName", _MenuName));
                list.Add(new SqlPara("MenuTag", ""));//暂时先保存空值
                list.Add(new SqlPara("ComeFrom", _ComeFrom));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_RIGHTGROUPTREEKT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    DataRow dr = dsmenu.Tables[0].NewRow();

                    DataRow[] rows = dsmenu.Tables[0].Select(string.Format("MGuid='{0}'", _MGuid));
                    if (rows.Length > 0)
                    {
                        dr = rows[0];
                    }

                    AddRow(dr, "MGuid", _MGuid);
                    AddRow(dr, "ID", _ID);
                    AddRow(dr, "ParentID", _ParentID);
                    AddRow(dr, "MenuName", _MenuName);
                    AddRow(dr, "ComeFrom", _ComeFrom);

                    if (rows.Length == 0)
                    {
                        dsmenu.Tables[0].Rows.Add(dr);
                    }
                    dsmenu.AcceptChanges();

                    MGuid.Text = "";
                    ID.Text = "";
                    ParentID.Text = "";
                    ucLabelBox1.Text = "";
                    MenuName.Text = "";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}