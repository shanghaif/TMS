using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using KMS.Tool;
using KMS.SqlDAL;
using KMS.Common;

namespace KMS.UI
{
    public partial class frmChooseMoneyAccount : BaseForm
    {
        private DataSet dsxm = new DataSet();
        public int Num =0;
        public decimal Money = 0;
        int state = 0;//1：点击确定按钮关闭的  0：点击右上角的×关闭的

        public decimal currVerifyFee=0;
        public string AccountType = "", MoneyAccount = "", SubjectID = "", SubjectName="";

        public frmChooseMoneyAccount()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 0)
            {
                AccountType = "现金";
                MoneyAccount = comboBoxEdit2.Text.Trim().ToString();
            }
            if (radioGroup1.SelectedIndex == 1)
            {
                AccountType = "银行";
                MoneyAccount = comboBoxEdit3.Text.Trim().ToString();
            }
                this.DialogResult = DialogResult.OK;
                state = 1;
                this.Close();
           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmChoiceSubject_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (state == 0)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }


        private void frmChoiceSubject_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            textBox2.Text = Num.ToString();
            textBox1.Text = Money.ToString();
            textBox3.Text = Money.ToString();
            getMoneyAccount();
            #region 
            textEdit1.Text = Money.ToString();
            textEdit1.Properties.ReadOnly = true;
            textEdit2.Text = "0";
            textEdit2.Properties.ReadOnly = true;
            comboBoxEdit2.Enabled = true;
            comboBoxEdit3.Enabled = false;
            #endregion           
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_basMoneyAccount_Verifi"));
            myGridControl1.DataSource = ds.Tables[0];

        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textEdit1.Text = "";
            textEdit2.Text = "";
            if (radioGroup1.SelectedIndex == 0)
            {               
                textEdit1.Text = Money.ToString();
                textEdit1.Properties.ReadOnly = true;
                textEdit2.Text = "0";
                textEdit2.Properties.ReadOnly = true;
                comboBoxEdit2.Enabled = true;
                comboBoxEdit3.Enabled = false;
            }
            if (radioGroup1.SelectedIndex == 1)
            {
                textEdit2.Text = Money.ToString();
                textEdit1.Properties.ReadOnly = true;
                textEdit1.Text = "0";
                textEdit2.Properties.ReadOnly = true;
                comboBoxEdit2.Enabled = false;
                comboBoxEdit3.Enabled = true;
            }
         
        }
        public void getMoneyAccount()
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_basMoneyAccount_XJ"));//提取账号类型为现金的户名
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    comboBoxEdit2.Properties.Items.Add(dr["AccountName"]);
                }
            }
        }

        private void comboBoxEdit3_Enter(object sender, EventArgs e)
        {
            myGridControl1.Left = comboBoxEdit3.Left ;
            //myGridControl1.Top = comboBoxEdit3.Top + comboBoxEdit3.Height + 66;
            myGridControl1.Top = comboBoxEdit3.Top+25;
            myGridControl1.Visible = true;
            myGridControl1.BringToFront();
        }

        private void comboBoxEdit3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl1.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
                //TransferSite.Focus();
            }
        }

        private void comboBoxEdit3_Leave(object sender, EventArgs e)
        {
            if (!myGridControl1.Focused)
            {
                myGridControl1.Visible = false;
            }
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetMoneyAccount();
        }

        private void myGridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetMoneyAccount();
            }
        }

        private void myGridControl1_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = comboBoxEdit3.Focused;
        }

        private void SetMoneyAccount()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView1.GetDataRow(rowhandle);
            string BankName = dr["BankName"].ToString();
            string BankAccount = dr["BankAccount"].ToString();
            string AccountName = dr["AccountName"].ToString();
            comboBoxEdit3.Text = AccountName + BankAccount + BankName;
             SubjectID = dr["SubjectID"].ToString();
             SubjectName = dr["SubjectName"].ToString();
            myGridControl1.Visible = false;
        }

        private void comboBoxEdit2_EditValueChanged(object sender, EventArgs e)
        {
            string AccountName = comboBoxEdit2.Text.Trim();
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_basMoneyAccount_XJ_AccountName",
                new List<SqlPara>() { new SqlPara("AccountName", AccountName) }));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                SubjectID = ds.Tables[0].Rows[0]["SubjectID"].ToString();
                SubjectName = ds.Tables[0].Rows[0]["SubjectName"].ToString();
            }
        }

    }
}
