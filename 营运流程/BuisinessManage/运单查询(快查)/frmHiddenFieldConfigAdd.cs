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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmHiddenFieldConfigAdd : BaseForm
    {
        public frmHiddenFieldConfigAdd()
        {
            InitializeComponent();
        }
        public int operType = 0;
        public DataRow dr = null;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        delegate void Mydel(string FiledName);
        private void frmLinesConfigurationAdd_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            GetCompanys();
            getMenuComboBox();
            CompanyID.SelectedIndex = 0;
            GetAllWebId();
            if (dr != null)
            {

                CompanyID.EditValue = dr["CompanyID"].ToString();
                txtOperMan.Text = dr["OptMan"].ToString();
                cbwebid.Text = dr["WebName"].ToString();
                txtDate.EditValue = dr["OptTime"].ToString();
                //FieldName.Text = dr["FieldName"].ToString();

                FieldName.EditValue = dr["FieldName"].ToString();
                FieldName.Text = dr["FieldName"].ToString();
                SetCheckedItems(dr["Field"].ToString(), FieldName);

                FieldName.Properties.Items.Clear();
                MenuComboBox.SelectedItem = dr["MenuName"].ToString();
                comboBoxEdit3.SelectedItem = dr["SetNode"].ToString();
                comboBoxEdit1.SelectedItem = dr["hiddenWeb"].ToString();
                MenuComboBox.Properties.ReadOnly = true;
                cbwebid.Properties.ReadOnly = true;
                CompanyID.Properties.ReadOnly = true;
            }
            txtOperMan.Text = CommonClass.UserInfo.UserName;
            txtDate.EditValue = CommonClass.gcdate;
            cbSetwebid.Text = CommonClass.UserInfo.WebName;
            setCompanyID.Text = CommonClass.UserInfo.companyid;
            CompanyID.Properties.ReadOnly = true;
        }

        private void getMenuComboBox()
        {
            foreach (MenuID r in Enum.GetValues(typeof(MenuID)))
            {
                MenuComboBox.Properties.Items.Add(r.ToString());

                System.Reflection.FieldInfo fieldInfo = r.GetType().GetField(r.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                if (attribArray.Length > 0)
                {
                    dic.Add(r.ToString(), (attribArray[0] as DescriptionAttribute).Description);
                }
            }
        }

        private void SetCheckedItems(string value, DevExpress.XtraEditors.CheckedComboBoxEdit control)
        {
            string[] arr1 = value.Split(',');
            for (int i = 0; i < arr1.Length; i++)
            {
                foreach (CheckedListBoxItem item in control.Properties.Items)
                {
                    if (item.Value.ToString() == arr1[i].Trim() )
                    {
                        item.CheckState = CheckState.Checked;
                        continue;
                    }
                }
            }
        }

        private void GetCompanys()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Waybill_FieldName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                
                //string[] strarr = str.Split(',');
                ////string[] strarr1 = str1.Split(',');

                FieldName.Properties.Items.Clear();
                FieldName.Properties.DataSource = ds.Tables[0];
                FieldName.Properties.DisplayMember = "FieldName".ToString().Trim();
                FieldName.Properties.ValueMember = "Field".ToString().Trim();
              
                FieldName.RefreshEditValue();
            }
            catch(Exception ex)
            { MsgBox.ShowException(ex); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Field = FieldName.EditValue.ToString().Trim();
                string fieldName = FieldName.Text.Trim();

                string operMan = txtOperMan.Text.Trim();
                string OptTime = txtDate.Text.Trim();
                string locations = "";

                //foreach (DataColumn item in ds.Tables[0].Columns)
                //{  
                //        str +=  item.ColumnName + ",";
                //}
                //foreach (DataRow item in ds.Tables[1].Rows)
                //{
                //    locations += item["location"].ToString() + ",";
                //}
                //if (subCompanyname == "")
                //{
                //    MsgBox.ShowOK("子公司ID不能为空！");
                //    return;
                //}

                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("mainCompanyid", mainCompanyid));
                //list.Add(new SqlPara("mainCompanyname", mainCompanyname));
                if (cbwebid.Text.Trim() == "" && this.CompanyID.Text.Trim().Substring(0, 3).Equals(this.setCompanyID.Text))
                {
                    //MessageBox.Show("选择网点不可为空！");
                    MsgBox.ShowError("选择网点不可为空！");
                    return;
                }
                if (String.IsNullOrEmpty(MenuComboBox.Text.Trim()))
                {
                    //MessageBox.Show("涉及界面不可已为空！");
                    MsgBox.ShowError("涉及界面不可已为空！");
                    return;
                }
                if (String.IsNullOrEmpty(FieldName.Text.Trim()))
                {
                    //MessageBox.Show("选择隐藏字段不可为空！");
                    MsgBox.ShowError("选择隐藏字段不可为空！");
                    return;
                }

                list.Add(new SqlPara("ID",dr == null ? Guid.NewGuid() : dr["ID"]));
                list.Add(new SqlPara("companyid1", this.CompanyID.Text.Trim().Substring(0, 3)));
                list.Add(new SqlPara("SetCompanyid", setCompanyID.Text));
                list.Add(new SqlPara("WebName", cbwebid.Text.Trim()));
                list.Add(new SqlPara("GridViewID", dic.ContainsKey(MenuComboBox.Text) ? dic[MenuComboBox.Text] : ""));
                list.Add(new SqlPara("MenuName", MenuComboBox.Text));//涉及界面
                list.Add(new SqlPara("SetWebName", cbSetwebid.Text));//设置网点
                list.Add(new SqlPara("SetNode", comboBoxEdit3.Text));//设置节点
                list.Add(new SqlPara("OptMan", operMan));
                //list.Add(new SqlPara("OptTime", OptTime));
                list.Add(new SqlPara("FieldName", fieldName));
                list.Add(new SqlPara("Field", Field));
                list.Add(new SqlPara("operType", operType));
                list.Add(new SqlPara("hiddenWeb", comboBoxEdit1.Text));//是否隐藏本网点，取值：是/否
                //list.Add(new SqlPara("location", locations));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_HiddenFieldConfig", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr[0].ToString().Equals(CommonClass.UserInfo.companyid))
                    CompanyID.Properties.Items.Add(dr[0] + " - " + dr[1]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CompanyID_EditValueChanged(object sender, EventArgs e)
        {
            GetAllWebId();
        }

        public void GetAllWebId()
        {
            try
            {
                cbwebid.Text = "";
                cbwebid.Properties.Items.Clear();
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_STATUS_Companyid", new List<SqlPara>() { new SqlPara("companyid1", this.CompanyID.Text.Trim().Substring(0, 3)) }));
                if (ds == null || ds.Tables[0].Rows.Count == 0 || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cbwebid.Properties.Items.Add(ds.Tables[0].Rows[i]["WebName"]);
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MenuComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            if (MenuComboBox.EditValue.ToString() == "快找界面")
            {
                comboBoxEdit3.Properties.ReadOnly = false;
                GetCompanys();
                if (dr != null)
                {
                    SetCheckedItems(dr["field"].ToString(), FieldName);
                }
            }
            else if (dic.ContainsKey(MenuComboBox.EditValue.ToString()))
            {
                comboBoxEdit3.Text = "";
                comboBoxEdit3.Properties.ReadOnly = true;
                list.Add(new SqlPara("ID", dic[MenuComboBox.EditValue.ToString()]));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_HiddenField", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                DataRow[] drlist = ds.Tables[0].Select("ColCaption <>'运单号' and ColCaption <>'运单状态' and ColCaption<>'发车批次'  and ColCaption<>'送货批次'");
                FieldName.Text = string.Empty;
                FieldName.Properties.DisplayMember = "ColCaption".ToString().Trim();
                FieldName.Properties.ValueMember = "ColName".ToString().Trim();

                DataTable tmp;
                if (drlist == null || drlist.Length == 0)
                {
                    tmp = null;
                }
                else
                {
                    tmp = drlist[0].Table.Clone();  // 复制DataRow的表结构  
                    foreach (DataRow row in drlist)
                        tmp.Rows.Add(row.ItemArray);  // 将DataRow添加到DataTable中
                }

                FieldName.Properties.DataSource = tmp;
                FieldName.RefreshEditValue();
                if (dr != null)
                {
                    SetCheckedItems(dr["field"].ToString(), FieldName);
                }
            }
        }
    }
}
