using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.UI
{
    public partial class DataStatisticsRignt_add : BaseForm
    {
        public DataRow dr = null;
        public int isModify=0;
        public DataStatisticsRignt_add()
        {
            InitializeComponent();
        }
        private DataSet ds = null;
        private DataSet dsUserName = null;


        private void DataStatisticsRignt_add_Load(object sender, EventArgs e)
        {
            //CommonClass.SetSite(txtUserAccount, false);
            //CommonClass.SetSite(txtUserName, false);
            GetCompanys();
            GetUserName();
            //SetAccountName();//加载账户名称
            txtCompanyID.Text = Common.CommonClass.UserInfo.companyid;
            string sql="companyid='"+Common.CommonClass.UserInfo.companyid+"'";
            txtCompanyName.Text = ds.Tables[0].Select(sql)[0]["gsqc"].ToString();

            if (dr != null)
            {
                txtUserAccount.Enabled = false;
                txtUserName.Enabled = false;
                txtUserAccount.EditValue = dr["Account"];
                txtUserName.EditValue = dr["UserName"];
                //txtCompanyName.EditValue = dr["AccountName"];
               // txtCompanyID.EditValue = dr["CompanyID"];
                //AccountNO.EditValue = dr["AccountNO"];
                //UserName.EditValue = dr["UserName"];
                txtCompanyIdRange.EditValue = dr["companyidRange"].ToString();
                txtCompanyIdRange.Text = dr["CompanyNameRange"].ToString();
                txtRemark.Text = dr["Remark"].ToString();
                SetCheckedItems(dr["companyidRange"].ToString(), txtCompanyIdRange);
            }

        }

        private void GetCompanys()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_COMPANYS", list);
                 ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return;
                //txtCompanyName.Properties.Items.Clear();
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{

                //    txtCompanyName.Properties.Items.Add(ds.Tables[0].Rows[i]["gsqc"].ToString());
                //}

                txtCompanyIdRange.Properties.Items.Clear();
                txtCompanyIdRange.Properties.DataSource = ds.Tables[0];
                txtCompanyIdRange.Properties.DisplayMember = "gsqc";
                txtCompanyIdRange.Properties.ValueMember = "companyid";
                txtCompanyIdRange.RefreshEditValue();


            }
            catch (Exception ex)
            { MsgBox.ShowException(ex); }
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

                    if (item.Value.ToString() == arr1[i].Trim())
                    {
                        item.CheckState = CheckState.Checked;
                        continue;
                    }
                }
            }
            //control.RefreshEditValue();
        }

        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (txtUserAccount.Text == "")
            {
                MsgBox.ShowOK("工号必须填写！");
                return;
            }

            if (txtUserName.Text == "")
            {
                MsgBox.ShowOK("用户名必须填写！");
                return;
            }

            if (txtCompanyName.Text == "")
            {
                MsgBox.ShowOK("公司名称必须填写！");
                return;
            }
            if (txtCompanyIdRange.Text.Trim() == "")
            { 
               MsgBox.ShowOK("权限范围必须填写！");
               return;
            }

            //if (UserName.Text == "")
            //{
            //    MsgBox.ShowOK("用户姓名不能为空！");
            //    return;
            //}

         try
            {
                string companyidRange = txtCompanyIdRange.EditValue.ToString().Trim();
                string CompanyNameRange = txtCompanyIdRange.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CompanyName",txtCompanyName.Text.Trim()));                
                list.Add(new SqlPara("Account", txtUserAccount.Text.Trim()));
                list.Add(new SqlPara("UserName", txtUserName.Text.Trim()));
                list.Add(new SqlPara("companyidRange", companyidRange));
                list.Add(new SqlPara("CompanyNameRange", CompanyNameRange));
                list.Add(new SqlPara("Remark", txtRemark.Text.Trim()));
                list.Add(new SqlPara("isModify", isModify));



                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_DataStatisticsRignt", list);
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

        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //private void SetAccountName()
        //{
        //    try
        //    {
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_GET_accountName");
        //        DataSet ds = SqlHelper.GetDataSet(sps);
        //        if (ds == null || ds.Tables.Count == 0) return;

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            txtCompanyName.Properties.Items.Add(dr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}


        private void AccountName_SelectedValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", txtCompanyName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_GET_accountNo",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
              //  AccountNO.Text = ds.Tables[0].Rows[0]["AccountNO"].ToString();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void GetUserName()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserNameTag", list);
                 dsUserName = SqlHelper.GetDataSet(sps);
                if(dsUserName==null ||dsUserName.Tables.Count==0||dsUserName.Tables[0].Rows.Count==0) return;
                txtUserName.Properties.Items.Clear();
                for (int i = 0; i < dsUserName.Tables[0].Rows.Count; i++)
                {
                    txtUserName.Properties.Items.Add(dsUserName.Tables[0].Rows[i]["UserName"].ToString());
                }
                   
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void txtCompanyName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return;
            string sql="gsqc='"+txtCompanyName.Text+"'";
            txtCompanyID.Text = ds.Tables[0].Select(sql)[0]["companyid"].ToString();
        }

        private void txtCompanyID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("companyid",txtCompanyID.Text.ToString().Trim()));
                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserNameTag", list);
                //dsUserName = SqlHelper.GetDataSet(sps);
                //if (dsUserName == null || dsUserName.Tables.Count == 0 || dsUserName.Tables[0].Rows.Count == 0) return;
                //txtUserName.Properties.Items.Clear();
                //for (int i = 0; i < dsUserName.Tables[0].Rows.Count; i++)
                //{
                //    txtUserName.Properties.Items.Add(dsUserName.Tables[0].Rows[i]["UserAccount"].ToString());
                //}
            }
            catch(Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void txtUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dsUserName == null || dsUserName.Tables.Count == 0 || dsUserName.Tables[0].Rows.Count == 0) return;
            string sql = "UserName='"+txtUserName.Text.Trim()+"'";
            DataRow[] drs = dsUserName.Tables[0].Select(sql);
            txtUserAccount.Properties.Items.Clear();
            for (int i = 0; i < drs.Length;i++ )
            {
                txtUserAccount.Properties.Items.Add(drs[i]["UserAccount"].ToString());
            }
            txtUserAccount.Text = drs[0]["UserAccount"].ToString();
        }

    }
}
