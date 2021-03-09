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
    public partial class frmBasAccount_add : BaseForm
    {
        public DataRow dr = null;
        public frmBasAccount_add()
        {
            InitializeComponent();
        }



        private void frmBasAccount_add_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(StartSite, false);
            CommonClass.SetSite(ToMiddle, false);
            GetUserName();
            SetAccountName();//加载账户名称
            CompanyID.Text = Common.CommonClass.UserInfo.companyid;

            if (dr != null)
            {
                StartSite.EditValue = dr["StartSite"];
                ToMiddle.EditValue = dr["ToMiddle"];
                AccountName.EditValue = dr["AccountName"];
                CompanyID.EditValue = dr["CompanyID"];
                AccountNO.EditValue = dr["AccountNO"];
                UserName.EditValue = dr["UserName"];
                feeDedu.Checked = dr["IsSettleFeeSub"].ToString() == "1" ? true : false;
                ckIsEnable.Checked = dr["IsEnable"].ToString() == "0" ? true : false;
                SetCheckedItems(dr["UserName"].ToString());
            }

        }

        private void SetCheckedItems(string value)
        {
            string[] arr1 = value.Split(',');
            for (int i = 0; i < arr1.Length; i++)
            {
                foreach (CheckedListBoxItem item in UserName.Properties.Items)
                {
                   // if (item.Value.ToString() == arr1[i])
                    if (item.ToString() == arr1[i])
                    {
                        item.CheckState = CheckState.Checked;
                        continue;
                    }
                }
            }
        }

        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (StartSite.Text == "")
            {
                MsgBox.ShowOK("始发站必须填写！");
                return;
            }

            if (ToMiddle.Text == "")
            {
                MsgBox.ShowOK("中转地必须填写！");
                return;
            }

            if (AccountName.Text == "")
            {
                MsgBox.ShowOK("账户名称必须填写！");
                return;
            }

            if (UserName.Text=="")
            {
                MsgBox.ShowOK("用户姓名不能为空！");
                return;
            }

         try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountID", dr == null ? Guid.NewGuid() : dr["AccountID"]));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
                list.Add(new SqlPara("ToMiddle", ToMiddle.Text.Trim()));
                list.Add(new SqlPara("AccountName", AccountName.Text.Trim()));
                //list.Add(new SqlPara("CompanyID", CompanyID.Text.Trim()));
                list.Add(new SqlPara("AccountNO", AccountNO.Text.Trim()));
                list.Add(new SqlPara("UserName", UserName.Text.Trim().Replace(" ", "")));
                list.Add(new SqlPara("UserAccount", UserName.EditValue.ToString().Replace(" ", "")));
                list.Add(new SqlPara("isFeeDeduction", feeDedu.Checked ? 1 : 0));
                list.Add(new SqlPara("IsEnable",ckIsEnable.Checked?0:1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASACCOUNT", list);
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


        private void SetAccountName()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_GET_accountName");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    AccountName.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void AccountName_SelectedValueChanged_1(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", AccountName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_GET_accountNo",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                AccountNO.Text = ds.Tables[0].Rows[0]["AccountNO"].ToString();

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
                DataSet ds = SqlHelper.GetDataSet(sps);
                UserName.Properties.Items.Clear();

                UserName.Properties.DataSource = ds.Tables[0];
                UserName.Properties.DisplayMember = "UserName";
                UserName.Properties.ValueMember = "UserAccount";
                UserName.RefreshEditValue();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
