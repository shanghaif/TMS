using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.UI
{
    public partial class 
        frmAddUsers : BaseForm
    {
        public frmAddUsers()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(ComboBoxEdit) || item.GetType() == typeof(CheckedComboBoxEdit))
                    {
                        if (item.Text.Trim() == "" && item.Name != "txtPDAPwd")
                        {
                            MsgBox.ShowOK("每一项都必须填写!");
                            return;
                        }
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserId", dr == null ? Guid.NewGuid() : dr["UserId"]));
                list.Add(new SqlPara("UserAccount", UserAccount.Text.Trim()));
                list.Add(new SqlPara("UserName", UserName.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                list.Add(new SqlPara("DepartName", DepartName.Text.Trim()));
                list.Add(new SqlPara("GRCode", GRCode.EditValue.ToString().Replace(" ", "")));
                list.Add(new SqlPara("GRName", GRCode.Text.Trim().Replace(" ", "")));
                list.Add(new SqlPara("UserState", UserState.Checked ? 1 : 0));
                list.Add(new SqlPara("EmpNo", EmpNO.Text.Trim()));
                list.Add(new SqlPara("Position", position.Text.Trim()));
                list.Add(new SqlPara("UserRight", ConvertType.ToString(UserRight.EditValue)));
                list.Add(new SqlPara("isLoginApp", isLoginApp.Checked ? 1 : 0));
                list.Add(new SqlPara("EnableValidate", EnableValidate.Checked ? 1 : 0));
                list.Add(new SqlPara("PDApwd", txtPDAPwd.Text.Trim()));//zxw 2017-1-7 08:55
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));//公司ID

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SYSUSERINFO", list);
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

        private void frmAddUsers_Load(object sender, EventArgs e)
        {
            GetCompanyId();

            if (Common.CommonClass.UserInfo.companyid != "101")
            {
                CompanyID.Enabled = false;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            else
            {
                CompanyID.Enabled = true;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            CommonClass.FormSet(this);

            CommonClass.SetCause(CauseName, false);
            CommonClass.SetSite(SiteName, false);

            GetUserRightTag();

            if (this.dr != null)
            {
                //UserId.EditValue = dr["UserId"];
                UserAccount.EditValue = dr["UserAccount"];
                //UserPsw.EditValue = dr["UserPsw"];
                UserName.EditValue = dr["UserName"];
                SiteName.EditValue = dr["SiteName"];
                WebName.EditValue = dr["WebName"];
                CauseName.EditValue = dr["CauseName"];
                AreaName.EditValue = dr["AreaName"];
                DepartName.EditValue = dr["DepartName"];
                GRCode.EditValue = dr["GRCode"];
                GRCode.Text = dr["GRName"].ToString();
                UserState.Checked = ConvertType.ToInt32(dr["UserState"]) > 0;
                EmpNO.EditValue = dr["EmpNO"];
                position.EditValue = dr["Position"];
                isLoginApp.Checked = ConvertType.ToInt32(dr["isLoginApp"]) > 0;
                SetCheckedItems(dr["GRCode"].ToString());
                CommonClass.SetSelectIndexByValue(dr["UserRight"].ToString().Trim(), UserRight);
                EnableValidate.Checked = ConvertType.ToInt32(dr["EnableValidate"]) > 0;
                txtPDAPwd.EditValue = dr["PDApwd"];
                if (ZQTMS.Common.CommonClass.UserInfo.companyid != "101")
                {
                    CompanyID.Enabled = false;
                    CompanyID.EditValue = dr["companyid"];
                }
                else
                {
                    CompanyID.Enabled = true;
                    CompanyID.EditValue = dr["companyid"];
                }
            }
        }

        private void SetCheckedItems(string value)
        {
            string[] arr1 = value.Split(',');
            for (int i = 0; i < arr1.Length; i++)
            {
                foreach (CheckedListBoxItem item in GRCode.Properties.Items)
                {
                    if (item.Value.ToString() == arr1[i])
                    {
                        item.CheckState = CheckState.Checked;
                        continue;
                    }
                }
            }
        }

        private void CauseName_TextChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text.Trim(), false);
        }

        private void AreaName_TextChanged(object sender, EventArgs e)
        {
            CommonClass.SetDep(DepartName, AreaName.Text.Trim(), false);
        }

        private void EmpNO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string _empno = EmpNO.Text.Trim();
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("EmpNO", _empno));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEMPLOYEE_ByEmpNO", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                    DataRow udr = ds.Tables[0].Rows[0];

                    UserName.EditValue = udr["EmpName"];
                    SiteName.EditValue = udr["EmpSite"];
                    WebName.EditValue = udr["EmpWeb"];
                    CauseName.EditValue = udr["EmpCuase"];
                    AreaName.EditValue = udr["EmpArea"];
                    DepartName.EditValue = udr["EmpDept"];
                    position.EditValue = udr["EmpPosition"];
                    UserState.Checked = true;
                }
                catch
                {

                }
            }
        }

        private void GetUserRightTag()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserRightTag_KT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                GRCode.Properties.Items.Clear();

                GRCode.Properties.DataSource = ds.Tables[0];
                GRCode.Properties.DisplayMember = "GRName";
                GRCode.Properties.ValueMember = "GRCode";
                GRCode.RefreshEditValue();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void DepartName_TextChanged(object sender, EventArgs e)
        {
            WebName.Text = DepartName.Text.Trim();
        }

        private void DepartName_Enter(object sender, EventArgs e)
        {
            if (WebName.Text.Trim() == "")
            {
                WebName.Text = DepartName.Text.Trim();
            }
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
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    
    }
}