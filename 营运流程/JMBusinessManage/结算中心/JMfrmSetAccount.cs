using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraEditors.Controls;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfrmSetAccount :BaseForm
    {

        public DataRow dr = null;
        public JMfrmSetAccount()
        {
            InitializeComponent();
        }

        private void frmAddPart_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            AccountReserved.Text = dr["AccountReserved"].ToString(); ;
            NegwarnValue.Text = dr["NegwarnValue"].ToString();
            LoanWarnValue.Text = dr["LoanWarnValue"].ToString();

            AccountType.Text = dr["AccountType"].ToString();
            BankName.Text = dr["BankName"].ToString();
            BankAccountName.Text = dr["BankAccountName"].ToString();
            BankAccountNO.Text = dr["BankAccountNO"].ToString();



        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", new Guid(dr["ID"].ToString())));
                list.Add(new SqlPara("AccountReserved ", AccountReserved.Text.Trim()));
                list.Add(new SqlPara("NegwarnValue ", NegwarnValue.Text.Trim()));
                list.Add(new SqlPara("LoanWarnValue ", LoanWarnValue.Text.Trim()));

                list.Add(new SqlPara("AccountType ", AccountType.Text.Trim()));
                list.Add(new SqlPara("BankName ", BankName.Text.Trim()));
                list.Add(new SqlPara("BankAccountName ", BankAccountName.Text.Trim()));
                list.Add(new SqlPara("BankAccountNO ", BankAccountNO.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASSETTLECENTERACC_ACCOUNT", list);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
