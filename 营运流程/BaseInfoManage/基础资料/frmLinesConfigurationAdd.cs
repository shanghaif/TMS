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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmLinesConfigurationAdd : BaseForm
    {
        public frmLinesConfigurationAdd()
        {
            InitializeComponent();
        }
        public int operType = 0;
        public DataRow dr = null;

        private void frmLinesConfigurationAdd_Load(object sender, EventArgs e)
        {
            GetCompanys();
            if (operType == 1)
            {
                txtCompanyID.Enabled = false;
            }
            if (dr != null)
            {
            
                txtCompanyID.EditValue = dr["MainCompanyid"];
                txtCompanyID.Text = dr["MainCompanyname"].ToString();

                txtSubCompanyID.EditValue = dr["SubCompanyid"].ToString();
                txtSubCompanyID.Text = dr["SubCompanyname"].ToString();
               SetCheckedItems(dr["MainCompanyid"].ToString(),txtCompanyID);
                SetCheckedItems(dr["SubCompanyid"].ToString(),txtSubCompanyID);

               
            }
            
            txtOperMan.Text = CommonClass.UserInfo.UserName;
            txtOperWeb.Text = CommonClass.UserInfo.WebName;
            txtDate.EditValue = CommonClass.gcdate;
        }

        private void SetCheckedItems(string value, DevExpress.XtraEditors.CheckedComboBoxEdit control)
        {
            //foreach (CheckedListBoxItem item in txtSubCompanyID.Properties.Items)
            //{
            //    item.CheckState = CheckState.Unchecked;
            //}
           
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
            //control.RefreshEditValue();
        }

        private void GetCompanys()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANYS", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                txtCompanyID.Properties.Items.Clear();
                txtCompanyID.Properties.DataSource = ds.Tables[0];
                txtCompanyID.Properties.DisplayMember = "gsqc";
                txtCompanyID.Properties.ValueMember = "companyid";
                txtCompanyID.RefreshEditValue();

                txtSubCompanyID.Properties.Items.Clear();
                txtSubCompanyID.Properties.DataSource = ds.Tables[0];
                txtSubCompanyID.Properties.DisplayMember = "gsqc";
                txtSubCompanyID.Properties.ValueMember = "companyid";
                txtSubCompanyID.RefreshEditValue();

               
            }
            catch(Exception ex)
            { MsgBox.ShowException(ex); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string mainCompanyid = txtCompanyID.EditValue.ToString().Trim();
                 string mainCompanyname=txtCompanyID.Text.Trim();

                string subCompanyid = txtSubCompanyID.EditValue.ToString().Trim();
                string subCompanyname = txtSubCompanyID.Text.Trim();

                string operMan = txtOperMan.Text.Trim();
                string operWeb = txtOperWeb.Text.Trim();
                DateTime operDate =Convert.ToDateTime(txtDate.EditValue.ToString());
                if (mainCompanyname == "")
                {
                    MsgBox.ShowOK("公司ID不能为空！");
                    return;
                }
                if (mainCompanyname.Contains(","))
                {
                    MsgBox.ShowOK("公司名称只能选择一个!");
                    return;
                }
                if (subCompanyname == "")
                {
                    MsgBox.ShowOK("子公司ID不能为空！");
                    return;
                }
                if (subCompanyname.Contains(mainCompanyname))
                {
                    MsgBox.ShowOK("子公司选择错误，不能将公司自己作为自己的子公司！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("mainCompanyid", mainCompanyid));
                list.Add(new SqlPara("mainCompanyname", mainCompanyname));

                list.Add(new SqlPara("subCompanyid", subCompanyid));
                list.Add(new SqlPara("subCompanyname", subCompanyname));
            
                list.Add(new SqlPara("operMan", operMan));
                list.Add(new SqlPara("operWeb", operWeb));
                list.Add(new SqlPara("operDate", operDate));
                list.Add(new SqlPara("operType",operType));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_LINESCONFIGURATION", list);
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
    }
}
