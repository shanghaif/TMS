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



namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmAddPlatAccount : BaseForm
    {
        public DataRow dr = null;
        int id;
        DataTable acNoDt = null;
        public frmAddPlatAccount()
        {
            InitializeComponent();
        }
   

        private void frmAddPlatAccount_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(bSite, false);
            CommonClass.SetSite(mSite, false);
            SetAccName();
            SetUserName();
            if (dr != null)
            {
                bSite.Text = dr["StartSite"].ToString();
                mSite.Text = dr["ToMiddle"].ToString();
                accName.Text = dr["AccountName"].ToString();
                accNo.Text = dr["AccountNo"].ToString();
                companyID.Text = dr["CompanyID"].ToString();
                id = Convert.ToInt32(dr["ID"]);
                userName.Text = dr["UserName"].ToString();
                if (dr["IsEnable"].ToString().Equals("1"))
                {
                    isEnable.Checked = true;
                }
                else
                {
                    isEnable.Checked = false;
                }
                if (dr["IsSettleFeeSub"].ToString().Equals("1"))
                {
                    isKKuan.Checked = true;
                }
                else
                {
                    isKKuan.Checked = false;
                }
            }
            else
            {
                companyID.EditValue = CommonClass.UserInfo.companyid;
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CheckEmptyValue())
            {
                MsgBox.ShowOK("加有特殊标识项必填，请检查!");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("sSite", bSite.Text.Trim()));
            list.Add(new SqlPara("mSite", mSite.Text.Trim()));
            list.Add(new SqlPara("accountName", accName.Text.Trim()));
            list.Add(new SqlPara("accountNo", accNo.Text.Trim()));
            list.Add(new SqlPara("a_companyId", companyID.Text.Trim()));
            list.Add(new SqlPara("Id", id));
            list.Add(new SqlPara("userName", userName.Text.Trim()));
            list.Add(new SqlPara("isKKuan", isKKuan.Checked == true ? 1 : 0));
            list.Add(new SqlPara("isEnable", isEnable.Checked == true ? 1 : 0));
            list.Add(new SqlPara("DBName", CommonClass.UserInfo.UserDB));
            list.Add(new SqlPara("UserAccount", userName.EditValue.ToString().Replace(" ", "")));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_Add_PlatAccountInfo", list);
            int result = SqlHelper.ExecteNonQuery(sps);
            if (result > 0)
            {
                MsgBox.ShowOK();
                
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheckEmptyValue()
        {
            bool note = false;
            if (accName.Text.Trim().Equals("") || accNo.Text.Trim().Equals("") || userName.Text.Trim().Equals(""))
            {
                note = true;
            }
            return note;
        }

        private void SetAccName()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_GET_accountName");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    acNoDt = ds.Tables[0];
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        accName.Properties.Items.Add(r[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SetUserName()
        {
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_UserNameTag");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                userName.Properties.Items.Clear();
                userName.Properties.DataSource = ds.Tables[0];
                userName.Properties.DisplayMember = "UserName";
                userName.Properties.ValueMember = "UserAccount";
                userName.RefreshEditValue();
            }
        }

        private void accName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string acName = accName.Text.Trim();
            if (acNoDt != null)
            {
                DataRow[] r = acNoDt.Select("AccountName = '" + acName + "'");
                if (r.Length > 0)
                {
                    accNo.EditValue = r[0]["AccountNO"].ToString();
                }
            }
        }
    }
}