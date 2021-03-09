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
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using ZQTMS.Common;
using ZQTMS.UI.Properties;

namespace ZQTMS.UI
{
    public partial class frmMenu : BaseForm
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        DataSet dsmenu = new DataSet();

        private void frmMenu_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("菜单管理");//xj/2019/5/29
            BarMagagerOper.SetBarPropertity(bar1);
            GetMenuData();

            GetIcon();
            GetMenuToCompany();//毛慧20171205
        }

        private bool GetMenuData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMENU_Manager_KT", list);
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

        private void GetIcon()
        {
            ResourceMenu res = new ResourceMenu();
            PropertyInfo[] peoperInfo = res.GetType().GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            int i = 0;
            foreach (PropertyInfo pro in peoperInfo)
            {
                if (!pro.PropertyType.FullName.Equals("System.Drawing.Bitmap")) continue;

                imageList1.Images.Add(pro.Name, (Image)pro.GetValue(pro.Name, null));
                IconName.Properties.Items.Add(new ImageComboBoxItem(pro.Name, i));
                i++;
            }
            IconName.Properties.Items.Insert(0, new ImageComboBoxItem());
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

                int _MenuEnable = MenuEnable.Checked ? 1 : 0;

                int _ID = ID.Text.Trim() == "" ? 0 : Convert.ToInt32(ID.Text.Trim());
                if (_ID == 0)
                {
                    MsgBox.ShowOK("菜单编号值不能为空!");
                    ID.Focus();
                    return;
                }

                int _ParentID = ParentID.Text.Trim() == "" ? 0 : Convert.ToInt32(ParentID.Text.Trim());
                //if (_ParentID == "")
                //{
                //    MsgBox.ShowOK("父级菜单值不能为空!");
                //    return;
                //}

                int _LevelID = LevelID.Text.Trim() == "" ? 0 : Convert.ToInt32(LevelID.Text.Trim());
                if (_LevelID == 0)
                {
                    MsgBox.ShowOK("菜单级别值不能为空!");
                    LevelID.Focus();
                    return;
                }

                int _MenuOrder = MenuOrder.Text.Trim() == "" ? 0 : Convert.ToInt32(MenuOrder.Text.Trim());
                if (_MenuOrder == 0)
                {
                    MsgBox.ShowOK("菜单顺序值不能为空!");
                    MenuOrder.Focus();
                    return;
                }

                string _MenuName = MenuName.Text.Trim();
                if (_MenuName == "")
                {
                    MsgBox.ShowOK("菜单名称值不能为空!");
                    MenuName.Focus();
                    return;
                }

                string _DllPath = DllPath.Text.Trim();
                //if (_DllPath == "")
                //{
                //    MsgBox.ShowOK("菜单文件路径不能为空!");
                //    return;
                //}

                string _DllName = DllName.Text.Trim();
                //if (_DllName == "")
                //{
                //    MsgBox.ShowOK("程序集文件名不能为空!");
                //    DllName.Focus();
                //    return;
                //}

                string _FormNameSpace = FormNameSpace.Text.Trim();
                //if (_FormNameSpace == "")
                //{
                //    MsgBox.ShowOK("命名空间值不能为空!");
                //    FormNameSpace.Focus();
                //    return;
                //}

                string _FormClass = FormClass.Text.Trim();
                //if (_FormClass == "")
                //{
                //    MsgBox.ShowOK("窗体类名值不能为空!");
                //    FormClass.Focus();
                //    return;
                //}

                string _Paras = Paras.Text.Trim();
                //if (_Paras == "")
                //{
                //    MsgBox.ShowOK("参数值不能为空!");
                //    return;
                //}

                int _ParasTransferMode = ParasTransferMode.SelectedIndex;
                //if (_ParasTransferMode == "")
                //{
                //    MsgBox.ShowOK("参数传递方式值不能为空!");
                //    return;
                //}
                if (_Paras != "" && _ParasTransferMode == 0)
                {
                    MsgBox.ShowOK("为窗体指定了参数，却没有选择参数传递类型!");
                    Paras.Focus();
                    return;
                }

                if (_Paras == "" && _ParasTransferMode > 0)
                {
                    MsgBox.ShowOK("选择了参数传递类型，却没有窗体指定参数!");
                    Paras.Focus();
                    return;
                }

                int _ShowType = ShowType.SelectedIndex;
                //if (_ShowType == "")
                //{
                //    MsgBox.ShowOK("窗体显示方式值不能为空!");
                //    return;
                //}

                string _IconName = IconName.Text.Trim();
                //if (_IconName == "")
                //{
                //    MsgBox.ShowOK("图标文件名值不能为空!");
                //    return;
                //}

                if (!int.Equals(_LevelID * 2, _ID.ToString().Length))
                {
                    MsgBox.ShowOK("菜单ID位数和菜单级别不符：\r\n每一级菜单ID增加两位数!");
                    ID.Focus();
                    return;
                }
                if (MenuToCompany.Text.Trim() == "")  //毛慧20171205
                {
                    if (MsgBox.ShowYesNo("菜单同步为空，请确认是否仅保存到101公司！") != DialogResult.Yes)
                    {
                        return;
                    }
                }
                #endregion

                string mtc = MenuToCompany.Text.Trim();  //毛慧20171206 
                string[] newmtc = mtc.Split(',');
                List<string> arr = new List<string>();
                for (int i = 0; i < newmtc.Length; i++)
                {
                    newmtc[i] = newmtc[i].Trim();
                    arr.Add(newmtc[i]);
                }
                string[] array = arr.ToArray();
                mtc = string.Join("@",array);
                mtc = mtc + "@";
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MGuid", _MGuid));
                list.Add(new SqlPara("MenuEnable", _MenuEnable));
                list.Add(new SqlPara("ID", _ID));
                list.Add(new SqlPara("ParentID", _ParentID));
                list.Add(new SqlPara("LevelID", _LevelID));
                list.Add(new SqlPara("MenuOrder", _MenuOrder));
                list.Add(new SqlPara("MenuName", _MenuName));
                list.Add(new SqlPara("DllPath", _DllPath));
                list.Add(new SqlPara("DllName", _DllName));
                list.Add(new SqlPara("FormNameSpace", _FormNameSpace));
                list.Add(new SqlPara("FormClass", _FormClass));
                list.Add(new SqlPara("Paras", _Paras));
                list.Add(new SqlPara("ParasTransferMode", _ParasTransferMode));
                list.Add(new SqlPara("ShowType", _ShowType));
                list.Add(new SqlPara("IconName", _IconName));
                list.Add(new SqlPara("MenuToCompany", mtc.Trim())); //毛慧20171206

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASMENU_KT_Line", list);
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
                    AddRow(dr, "MenuEnable", _MenuEnable);
                    AddRow(dr, "ID", _ID);
                    AddRow(dr, "ParentID", _ParentID);
                    AddRow(dr, "LevelID", _LevelID);
                    AddRow(dr, "MenuOrder", _MenuOrder);
                    AddRow(dr, "MenuName", _MenuName);
                    AddRow(dr, "DllPath", _DllPath);
                    AddRow(dr, "DllName", _DllName);
                    AddRow(dr, "FormNameSpace", _FormNameSpace);
                    AddRow(dr, "FormClass", _FormClass);
                    AddRow(dr, "Paras", _Paras);
                    AddRow(dr, "ParasTransferMode", _ParasTransferMode);
                    AddRow(dr, "ShowType", _ShowType);
                    AddRow(dr, "IconName", _IconName);

                    if (rows.Length == 0)
                    {
                        dsmenu.Tables[0].Rows.Add(dr);
                    }
                    dsmenu.AcceptChanges();
                    Clear();
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

        private void Clear()
        {
            MGuid.Text = Guid.NewGuid().ToString();
            MenuEnable.Checked = true;
            ID.Text = "";
            //ParentID.Text = "";
            LevelID.Text = "";
            MenuOrder.Text = "";
            MenuName.Text = "";
            DllName.Text = "";
            FormNameSpace.Text = "";
            FormClass.Text = "";
            Paras.Text = "";
            ParasTransferMode.SelectedIndex = 0;
            IconName.Text = "";
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //添加一级
        {
            if (treeList1.DataSource == null)
            {
                MsgBox.ShowError("请加载菜单数据后再新增!");
                return;
            }
            MenuName.Focus();
            TreeListNode node = treeList1.Nodes.LastNode;
            MGuid.Text = Guid.NewGuid().ToString();
            MenuName.Text = "";
            imageComboBoxEdit1.Text = "";
            ParentID.Text = "";
            LevelID.EditValue = 1;
            imageComboBoxEdit2.SelectedIndex = -1;
            FormNameSpace.Text = "";
            FormClass.Text = "";
            Paras.Text = "";
            ParasTransferMode.SelectedIndex = 0;
            IconName.Text = "";

            int id = 11;
            int order = 1;

            DataTable dt = treeList1.DataSource as DataTable;
            object obj = dt.Compute("max(ID)", "LevelID=1");
            if (obj != DBNull.Value)
            {
                id = Convert.ToInt32(obj);
            }

            obj = dt.Compute("max(MenuOrder)", "LevelID=1");
            if (obj != DBNull.Value)
            {
                order = Convert.ToInt32(obj);
            }

            ID.EditValue = id + 1;
            MenuOrder.EditValue = order + 1;

            //if (node == null)
            //{
            //    ID.EditValue = 11;
            //    MenuOrder.EditValue = 1;
            //    return;
            //}
            //ID.EditValue = Convert.ToInt32(node.GetValue("ID")) + 1;
            //MenuOrder.EditValue = Convert.ToInt32(node.GetValue("MenuOrder")) + 1;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//添加同级
        {
            MenuName.Focus();
            TreeListNode node = treeList1.FocusedNode;
            if (node == null)
            {
                MsgBox.ShowOK("没有选择菜单，无法添加同级菜单!");
                return;
            }
            MGuid.Text = Guid.NewGuid().ToString();
            MenuName.Text = "";
            if (node.ParentNode != null)
            {
                imageComboBoxEdit1.Properties.Items.Clear();
                imageComboBoxEdit1.Properties.Items.Add(new ImageComboBoxItem(node.ParentNode.GetValue("MenuName").ToString(), node.ParentNode.GetValue("ID").ToString()));
                ID.EditValue = Convert.ToInt32(node.ParentNode.LastNode.GetValue("ID")) + 1;
                imageComboBoxEdit1.SelectedIndex = 0;
            }
            else//没有父级，说明此时是根菜单
            {
                ID.EditValue = Convert.ToInt32(treeList1.Nodes.LastNode.GetValue("ID")) + 1;
                imageComboBoxEdit1.Text = "";
            }
            //ParentID.Text = "";
            LevelID.EditValue = node.GetValue("LevelID");
            imageComboBoxEdit2.SelectedIndex = -1;
            FormNameSpace.Text = "";
            FormClass.Text = "";
            Paras.Text = "";
            ParasTransferMode.SelectedIndex = 0;
            IconName.Text = "";

            MenuOrder.EditValue = Convert.ToInt32(node.GetValue("MenuOrder")) + 1;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //添加下级
        {
            MenuName.Focus();
            TreeListNode node = treeList1.FocusedNode;
            if (node == null)
            {
                MsgBox.ShowOK("没有选择菜单，无法添加下级菜单!");
                return;
            }
            MGuid.Text = Guid.NewGuid().ToString();
            MenuName.Text = "";

            imageComboBoxEdit1.Properties.Items.Clear();
            imageComboBoxEdit1.Properties.Items.Add(new ImageComboBoxItem(node.GetValue("MenuName").ToString(), node.GetValue("ID").ToString()));

            if (node.HasChildren)
            {
                ID.EditValue = Convert.ToInt32(node.LastNode.GetValue("ID")) + 1;
            }
            else
            {
                ID.EditValue = Convert.ToInt32(node.GetValue("ID") + "10") + 1;
            }
            imageComboBoxEdit1.SelectedIndex = 0;

            //ParentID.Text = "";
            LevelID.EditValue = Convert.ToInt32(node.GetValue("LevelID")) + 1;
            imageComboBoxEdit2.SelectedIndex = -1;
            FormNameSpace.Text = "";
            FormClass.Text = "";
            Paras.Text = "";
            ParasTransferMode.SelectedIndex = 0;
            IconName.Text = "";

            if (node.HasChildren)
            {
                MenuOrder.EditValue = Convert.ToInt32(node.Nodes.LastNode.GetValue("MenuOrder")) + 1;
            }
            else
            {
                MenuOrder.EditValue = 1;
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                DataRowView dr = treeList1.GetDataRecordByNode(node) as DataRowView;

                if (node.ParentNode != null)
                {
                    imageComboBoxEdit1.Properties.Items.Clear();
                    imageComboBoxEdit1.Properties.Items.Add(new ImageComboBoxItem(node.ParentNode.GetValue("MenuName").ToString(), node.ParentNode.GetValue("ID").ToString()));
                    imageComboBoxEdit1.SelectedIndex = 0;
                }
                MGuid.EditValue = dr["MGuid"];
                MenuEnable.Checked = Convert.ToInt32(dr["MenuEnable"]) == 1;
                ID.EditValue = dr["ID"];
                ParentID.EditValue = dr["ParentID"];
                LevelID.EditValue = dr["LevelID"];
                MenuOrder.EditValue = dr["MenuOrder"];
                MenuName.EditValue = dr["MenuName"];
                DllPath.EditValue = dr["DllPath"];
                DllName.EditValue = dr["DllName"];
                FormNameSpace.EditValue = dr["FormNameSpace"];
                FormClass.EditValue = dr["FormClass"];
                Paras.EditValue = dr["Paras"];
                ParasTransferMode.SelectedIndex = Convert.ToInt32(dr["ParasTransferMode"]);
                ShowType.SelectedIndex = Convert.ToInt32(dr["ShowType"]);
                IconName.EditValue = dr["IconName"];

                List<SqlPara> list = new List<SqlPara>();  //毛慧20171206
                list.Add(new SqlPara("id", ID.EditValue.ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Companyname_By_Menuid_KT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                DataTable dt = ds.Tables[0];
                string str = "";
                for (int i=0; i < dt.Rows.Count; i++)
                {
                    str = str + dt.Rows[i]["gsqc"].ToString() + ",";
                }
                str = str.Substring(0, str.Length - 1);
                MenuToCompany.Text = str;
                SetCheckedItems(str);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                TreeListNode node = treeList1.FocusedNode;
                if (node == null) return;

                if (MsgBox.ShowYesNo("是否删除？同时将删除对应的子菜单！\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                Guid guid = new Guid(node.GetValue("MGuid").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MGuid", guid));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASMENU_KT", list);
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

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GetMenuData();
        }

        private void imageComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit1.SelectedItem == null) return;
            ParentID.EditValue = ((ImageComboBoxItem)imageComboBoxEdit1.SelectedItem).Value;
        }

        private void DllPath_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = DllPath.SelectedIndex;
            string path = Application.StartupPath;
            DllName.Properties.Items.Clear();
            DllName.Text = "";

            if (index == 0) //此时表示该菜单有子菜单
            {
            }
            else if (index == 1)   //{根目录}\*.dll
            {
                string[] files = Directory.GetFiles(path, "ZQTMS.*.dll", SearchOption.TopDirectoryOnly);
                foreach (string item in files)
                {
                    DllName.Properties.Items.Add(Path.GetFileName(item));
                }
            }
            else if (index == 2)  //{根目录}\Plugin\*.dll
            {
                path = Path.Combine(path, "Plugin");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                string[] files = Directory.GetFiles(path, "ZQTMS.*.dll", SearchOption.TopDirectoryOnly);
                foreach (string item in files)
                {
                    DllName.Properties.Items.Add(Path.GetFileName(item));
                }
            }
            else if (index == 3)  //{根目录}\主程序.exe
            {
                DllName.Properties.Items.Add(Assembly.GetExecutingAssembly().GetModules(false)[0].Name);
            }
        }

        private void DllName_TextChanged(object sender, EventArgs e)
        {
            imageComboBoxEdit2.Properties.Items.Clear();
            imageComboBoxEdit2.SelectedIndex = -1;
            FormText.Text = "";
            if (DllName.Text.Trim() == "")
            {
                return;
            }

            string path = Application.StartupPath;

            if (DllPath.SelectedIndex == 2)  //{根目录}\Plugin\*.dll
            {
                path = Path.Combine(path, "Plugin");
            }

            Assembly ass = Assembly.LoadFile(Path.Combine(path, DllName.Text.Trim()));
            Type[] types = ass.GetExportedTypes();
            foreach (Type t in types)
            {
                if (t.FullName != "ZQTMS.Tool.frmPrintRuiLang" && t.BaseType == typeof(BaseForm))
                {
                    try
                    {
                        Form frm = (Form)Activator.CreateInstance(t);
                        imageComboBoxEdit2.Properties.Items.Add(new ImageComboBoxItem(frm.Name, frm));
                    }
                    catch { }
                }
            }
        }

        private void imageComboBoxEdit2_TextChanged(object sender, EventArgs e)
        {
            if (imageComboBoxEdit2.SelectedItem == null)
            {
                FormNameSpace.Text = FormClass.Text = FormText.Text = "";
                return;
            }
            Form frm = (Form)((ImageComboBoxItem)imageComboBoxEdit2.SelectedItem).Value;
            FormNameSpace.Text = frm.GetType().Namespace;
            FormClass.Text = frm.Name;
            FormText.Text = frm.Text;
        }

        private void GetMenuToCompany()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                MenuToCompany.Properties.Items.Clear();

                MenuToCompany.Properties.DataSource = ds.Tables[0];
                MenuToCompany.Properties.DisplayMember = "gsqc";
                MenuToCompany.Properties.ValueMember = "companyid";
                MenuToCompany.RefreshEditValue();
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SetCheckedItems(string value)
        {

            foreach (CheckedListBoxItem item in MenuToCompany.Properties.Items)
            {
                item.CheckState = CheckState.Unchecked;
                if(value.Contains(item.Description.ToString()))
                {
                    item.CheckState = CheckState.Checked;
                    continue;
                }
            }
        }
    }
}