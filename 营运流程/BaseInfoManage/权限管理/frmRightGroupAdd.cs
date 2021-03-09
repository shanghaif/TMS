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
using DevExpress.XtraTreeList.Nodes.Operations;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmRightGroupAdd : XtraForm
    {
        public frmRightGroupAdd()
        {
            InitializeComponent();
        }
        DataSet dsmenu = new DataSet();

        public int oper = 1;//1新增 2修改 3复制
        public string grCode = "", grName = "", grRemark = "";

        /// <summary>
        /// 不需要设置权限的工具栏菜单
        /// </summary>
        List<string> listLink = new List<string>() { "关闭", "退出", "刷新", "快找", "外观设置", "过滤器", "3", "4", "7", "8", "下一步" };

        private void frmRightGroupAdd_Load(object sender, EventArgs e)
        {
            try
            {
                BarMagagerOper.SetBarPropertity(bar1);
                if (oper == 2)
                {
                    GRCode.Enabled = false;
                    GRCode.Text = grCode;
                    GRName.Text = grName;
                    GRRemark.Text = grRemark;
                }
                if (oper == 3)
                {
                    GRCode.Enabled = true;
                }
                List<int> list = new List<int>();
                for (int i = 100; i < 300; i++)
                {
                    list.Add(i);
                }
                GRCode.Properties.Items.AddRange(list);

                GetAllInfo();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("加载错误：" + ex.Message);
            }
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
            if (dsmenu.Tables.Count > 1 && dsmenu.Tables[1].Rows.Count > 0)
                dsmenu.Tables[0].Merge(dsmenu.Tables[1]);

            if (oper == 1) return;
            GetCheckState(treeList1.Nodes);
        }

        /// <summary>
        /// 加载勾选信息
        /// </summary>
        /// <param name="node"></param>
        private void GetCheckState(TreeListNodes node)
        {
            if (dsmenu.Tables.Count < 3) return;
            foreach (TreeListNode item in node)
            {
                DataRow[] drs = dsmenu.Tables[2].Select(string.Format("MenuID={0}", item.GetValue("ID")));
                if (drs.Length > 0)
                {
                    item.Checked = Convert.ToInt32(drs[0]["GRFlag"]) == 1;
                }

                GetCheckState(item.Nodes);
            }
        }

        private bool GetMenuData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GRCode", grCode));

                //SqlParasEntity sps = new SqlParasEntity(OperType.QueryThreeTable, "QSP_GET_RigthGroup_Manager", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RigthGroup_Manager_KT", list);//zaj 2017-11-17

                dsmenu = SqlHelper.GetDataSet(sps);

                if (dsmenu == null || dsmenu.Tables.Count == 0) return true;
                treeList1.DataSource = dsmenu.Tables[0];

                dsmenu.Tables[0].Columns.Add("ButtonItemName", typeof(string));//用于保存工具栏按钮的名称
                dsmenu.Tables[1].Columns.Add("ButtonItemName", typeof(string));
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

                    #region 加提取功能
                    DataRow dr0 = dt.NewRow();
                    dr0["MGuid"] = Guid.NewGuid().ToString();
                    dr0["ID"] = dv["ID"].ToString() + "00";
                    dr0["ParentID"] = dv["ID"];
                    dr0["MenuName"] = "提取";
                    dr0["ComeFrom"] = 4; //表示为每个界面增加的提取权限，不起实际作用，只用于辅助父级勾选
                    dt.Rows.Add(dr0);
                    #endregion

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
                            dr["ButtonItemName"] = link.Item.Name;
                            dt.Rows.Add(dr);
                            id++;
                            link.Item.Dispose();
                        }

                        if (fb.Bar != null && fb.Bar.Manager != null)
                        {
                            fb.Bar.Manager.Dispose();
                        }
                        if (fb.Bar != null)
                        {
                            fb.Bar.Dispose();
                        }
                        if (fb.TabPage != null)
                        {
                            fb.TabPage.Dispose();
                        }
                    }
                    if (frm != null)
                    {
                        frm.Close();
                        frm.Dispose();
                    }
                }
                dt.AcceptChanges();
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

        public class TreeListOperationChecked : TreeListOperation
        {
            public override void Execute(DevExpress.XtraTreeList.Nodes.TreeListNode node)
            {
                node.Checked = true;
            }
        }

        public class TreeListOperationUnChecked : TreeListOperation
        {
            public override void Execute(DevExpress.XtraTreeList.Nodes.TreeListNode node)
            {
                node.Checked = false;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeList1.ExpandAll();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            treeList1.CollapseAll();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeListOperation oper = new TreeListOperationChecked();
            treeList1.NodesIterator.DoOperation(oper);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            TreeListOperation oper = new TreeListOperationUnChecked();
            treeList1.NodesIterator.DoOperation(oper);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                string grName = GRName.Text.Trim();
                string grCode = GRCode.Text.Trim();
                string grRemark = GRRemark.Text.Trim();

                if (grCode == "")
                {
                    MsgBox.ShowOK("请填写权限组编号!");
                    return;
                }
                if (grName == "")
                {
                    MsgBox.ShowOK("请填写权限组名称!");
                    return;
                }

                DataTable table = new DataTable();
                table.Columns.Add("MenuID", typeof(int));
                table.Columns.Add("GRTag", typeof(string));
                table.Columns.Add("GRFlag", typeof(int));

                GetAllNodes(treeList1.Nodes, table);

                string MenuIdStr = "", GRTagStr = "", GRFlagStr = "";
                foreach (DataRow dr in table.Rows)
                {
                    MenuIdStr += dr["MenuId"] + "@";
                    GRTagStr += dr["GRTag"] + "@";
                    GRFlagStr += dr["GRFlag"] + "@";
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("oper", oper));
                list.Add(new SqlPara("GRName", grName));
                list.Add(new SqlPara("GRCode", grCode));
                list.Add(new SqlPara("GRRemark", grRemark));
                list.Add(new SqlPara("MenuIdStr", MenuIdStr));
                list.Add(new SqlPara("GRTagStr", GRTagStr));
                list.Add(new SqlPara("GRFlagStr", GRFlagStr));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_RightGroup_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    treeList1.PostEditor();
                    if (GRCode.SelectedIndex >= 0)
                    {
                        GRCode.SelectedIndex++;
                    }
                    GRName.Text = "";
                    GRRemark.Text = "";

                    barButtonItem7_ItemClick(null, null);
                }

                if (oper == 2)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node == null) return;
            e.Appearance.ForeColor = e.Node.CheckState == CheckState.Checked ? Color.Red : (e.Node.CheckState == CheckState.Unchecked ? Color.Black : Color.Blue); //CheckState.Indeterminate：Blue
        }

        private void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
            }
        }

        private void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode == null) return;
            #region 任意一个子节点勾选，如果同级节点只要有一个勾选，则父级为勾选状态
            bool b = false;
            CheckState state;
            for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
            {
                state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                if (!check.Equals(state))
                {
                    b = !b;
                    break;
                }
            }
            node.ParentNode.CheckState = b ? CheckState.Checked : check;
            #endregion

            SetCheckedParentNodes(node.ParentNode, check);
        }

        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
        }

        private void treeList1_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        private void treeList1_MouseMove(object sender, MouseEventArgs e)
        {
            TreeListHitInfo hi = treeList1.CalcHitInfo(new Point(e.X, e.Y));
            if (hi == null) return;
            if (hi.HitInfoType == HitInfoType.Button || hi.HitInfoType == HitInfoType.NodeCheckBox) //|| hi.HitInfoType == HitInfoType.SelectImage || hi.HitInfoType == HitInfoType.StateImage
            {
                treeList1.Cursor = Cursors.Hand;
            }
            else
            {
                treeList1.Cursor = Cursors.Default;
            }
        }

        private void GetAllNodes(TreeListNodes node, DataTable dt)
        {
            try
            {
                foreach (TreeListNode item in node)
                {
                    DataRowView dv = treeList1.GetDataRecordByNode(item) as DataRowView;

                    DataRow dr = dt.NewRow();
                    dr["MenuID"] = dv["ID"];
                    if (dv["ButtonItemName"] != DBNull.Value)
                    {

                        dr["GRTag"] = string.Format("{0}.{1}#{2}", item.ParentNode.GetValue("FormNameSpace"), item.ParentNode.GetValue("FormClass"), dv["ButtonItemName"]);
                    }
                    else
                    {
                        dr["GRTag"] = dv["FormNameSpace"] == DBNull.Value || dv["FormNameSpace"].ToString() == "" ? "" : dv["FormNameSpace"] + "." + dv["FormClass"];
                    }
                    dr["GRFlag"] = item.Checked ? 1 : 0;
                    dt.Rows.Add(dr);

                    GetAllNodes(item.Nodes, dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("收集权限明细失败：" + ex.Message);
            }
        }
    }
}