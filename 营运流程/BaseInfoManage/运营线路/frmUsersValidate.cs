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
    public partial class frmUsersValidate : BaseForm
    {
        public frmUsersValidate()
        {
            InitializeComponent();
        }

        public string userAccount = "";

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string info = ValidationInfo.Text.Trim();
                info = info.Replace("，", ",").Replace("。", ",").Replace("、", ",").Replace("|", ",");

                if (info == "")
                {
                    if (MsgBox.ShowYesNo("没有填写验证信息，将清空登录验证信息。是否继续？") == DialogResult.No) return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserAccount", userAccount));
                list.Add(new SqlPara("ValidationInfo", info));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_User_ValidateInfo", list);
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
            CommonClass.FormSet(this);
           
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_User_ValidateInfo", list);
                list.Add(new SqlPara("UserAccount", userAccount));

                DataSet ds = SqlHelper.GetDataSet(sps);
                UserAccount.Text = userAccount;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                ValidationInfo.EditValue = ds.Tables[0].Rows[0]["ValidationInfo"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}