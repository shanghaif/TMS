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

namespace ZQTMS.UI
{
    public partial class frmSmsInfoAdd : BaseForm
    {
        public DataRow dr = null;
        public frmSmsInfoAdd()
        {
            InitializeComponent();
        }

        private void frmSmsInfoAdd_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            if (dr != null)
            {
                textEdit1.EditValue = dr["companyid"];
                textEdit2.EditValue = dr["smsuserid"];
                textEdit3.EditValue = dr["smspassword"];
                textEdit1.Enabled = false;
            }
        }

        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    textEdit1.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textEdit1.Text.Trim() == "")
                {
                    MsgBox.ShowError("公司id不允许为空！");
                    return;
                }
                if (textEdit2.Text.Trim() == "")
                {
                    MsgBox.ShowError("账户id不允许为空！");
                    return;
                }
                if (textEdit3.Text.Trim() == "")
                {
                    MsgBox.ShowError("账户密码不允许为空！");
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", dr == null ? Guid.NewGuid() : dr["id"]));
                list.Add(new SqlPara("companyidsms", textEdit1.Text.Trim()));
                list.Add(new SqlPara("smsuserid", textEdit2.Text.Trim()));
                list.Add(new SqlPara("smspassword", textEdit3.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SmsInfo_Companyid", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
