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
    public partial class frmAddSettlementAccount : BaseForm
    {
        public DataRow dr = null;
        public frmAddSettlementAccount()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SettlementID", dr["SettlementID"]));
                list.Add(new SqlPara("BankName", BankName.Text.Trim()));
                list.Add(new SqlPara("BankAccount", BankAccount.Text.Trim()));
                list.Add(new SqlPara("BankAccountName", BankAccountName.Text.Trim()));
                list.Add(new SqlPara("BankAddress", BankAddress.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SETTLEMENTACCOUNT", list);
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

        private void frmAddSettlementAccount_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
             
            if (dr != null)
            {
                CauseName.Text = dr["CauseName"].ToString();
                BankName.Text = dr["BankName"].ToString();
                BankAccount.Text = dr["BankAccount"].ToString();
                BankAccountName.Text = dr["BankAccountName"].ToString();
                BankAddress.Text = dr["BankAddress"].ToString();
            }
        }
    }
}
