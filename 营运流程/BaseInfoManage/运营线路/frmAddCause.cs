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

namespace ZQTMS.UI
{
    public partial class frmAddCause : BaseForm
    {
        public DataRow dr = null;
        public frmAddCause()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (userDB.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请选择登录环境!");
                    userDB.ShowPopup();
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CauseID", dr == null ? Guid.NewGuid() : dr["CauseID"]));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
                list.Add(new SqlPara("CauseCode", CauseCode.Text.Trim()));
                list.Add(new SqlPara("CauseMan", CauseMan.Text.Trim()));
                list.Add(new SqlPara("CausePhone", CausePhone.Text.Trim()));
                list.Add(new SqlPara("CauseRemark", CauseRemark.Text.Trim()));
                list.Add(new SqlPara("UserDB", userDB.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASCAUSE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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

        private void frmAddCause_Load(object sender, EventArgs e)
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
            DataTable dt = CommonClass.GetDatabaseInfo();
            foreach (DataRow row in dt.Rows)
            {
                userDB.Properties.Items.Add(row["db"].ToString());
            }
            if (dr != null)
            {
                //CauseID.EditValue = dr["CauseID"];
                CauseName.EditValue = dr["CauseName"];
                CauseCode.EditValue = dr["CauseCode"];
                CauseMan.EditValue = dr["CauseMan"];
                CausePhone.EditValue = dr["CausePhone"];
                CauseRemark.EditValue = dr["CauseRemark"];
                userDB.EditValue = dr["UserDB"];
                if (Common.CommonClass.UserInfo.companyid != "101")
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
