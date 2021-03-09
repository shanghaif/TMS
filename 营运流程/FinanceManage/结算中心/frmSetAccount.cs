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
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmSetAccount :BaseForm
    {

        public DataRow dr = null;
        public frmSetAccount()
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
            txtAccountelephone.Text = dr["Accountelephone"].ToString();


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
                if (!IsMobile(txtAccountelephone.Text.Trim()))
                {
                    MsgBox.ShowOK("电话格式不正确！");
                    return;
                }
                list.Add(new SqlPara("Accountelephone",txtAccountelephone.Text.Trim()));

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


        public bool IsMobile(String str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Regex mobileRegx = new Regex("^(\\+[0-9]{1,4}\\-){0,1}1(3|4|6|2|7|9|5|8)[0-9]{9}$");
                return mobileRegx.IsMatch(str);
            }
            else
            {
                return false;
            }
        }
    }
}
