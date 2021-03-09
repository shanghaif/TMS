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
    public partial class frmwithdrowCashApplyAdd : BaseForm
    {
        public frmwithdrowCashApplyAdd()
        {
            InitializeComponent();
        }

        private void frmChargeApplyAdd_Load(object sender, EventArgs e)
        {
            SerialNO.EditValue = "GZ" + CommonClass.gcdate.ToString("yyyyMMddHHmmss") + "-" + (new Random().Next(9999));
            ApplyDate.DateTime = CommonClass.gcdate;
            ShowPage();
        } 

        protected void ShowPage() 
        {
            try
            {
                string WebName = CommonClass.UserInfo.WebName;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webname", WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SettleCenterAccByWebName", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                AccountNO.EditValue = dr["AccountNo"];
                AccountName.EditValue = dr["AccountName"];
                AccountBanlace.EditValue = string.IsNullOrEmpty(dr["AccountBalance"].ToString()) ? "0" : dr["AccountBalance"];
                AccountReserved.EditValue = string.IsNullOrEmpty(dr["AccountReserved"].ToString()) ? "0" : dr["AccountReserved"];
                CanCashQuota.EditValue = decimal.Parse(AccountBanlace.Text.Trim()) - decimal.Parse(AccountReserved.Text.Trim());
 
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)//上传
        {
            frmFileUpload_CenterAcc frm = new frmFileUpload_CenterAcc();
            frm.ShowDialog();

            //upLoadImgText.Text = frm.ResultPath.Trim();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string _AccountBanlace = AccountBanlace.Text.Trim();
            string _AccountNO = AccountNO.Text.Trim();
            string _AccountName = AccountName.Text.Trim();

            string _SerialNO = SerialNO.Text.Trim();
            string _ApplyDate = ApplyDate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string _BankAccount = BankAccount.Text.Trim();
            string _BankSubbranch = BankSubbranch.Text.Trim();
            string _BankName = BankName.Text.Trim();
            string _Remark = Remark.Text.Trim();
            string _BankAccountName = BankAccountName.Text.Trim();
            string _AccountReserved = AccountReserved.Text.Trim();
            string _CanCashQuota = CanCashQuota.Text.Trim();
            string _ApplyQuota = ApplyQuota.Text.Trim();

            if (string.IsNullOrEmpty(_AccountBanlace) || string.IsNullOrEmpty(_AccountNO) || string.IsNullOrEmpty(_AccountName) ||
                string.IsNullOrEmpty(_SerialNO) || string.IsNullOrEmpty(_ApplyDate) || string.IsNullOrEmpty(_BankAccount) ||
                string.IsNullOrEmpty(_BankSubbranch) || string.IsNullOrEmpty(_BankName) || string.IsNullOrEmpty(_Remark) ||
                string.IsNullOrEmpty(_BankAccountName) || string.IsNullOrEmpty(_AccountReserved) ||  string.IsNullOrEmpty(_CanCashQuota) || 
                string.IsNullOrEmpty(_ApplyQuota) 
                )
            {
                MsgBox.ShowError("所有数据都不能为空！");
                return;
            }

            try
            {
                decimal.Parse(_AccountReserved);
                decimal.Parse(_CanCashQuota);
                decimal.Parse(_ApplyQuota); 
            }
            catch
            { 
                MsgBox.ShowError("金额不合法！");
                return;
            }

            if (decimal.Parse(_AccountReserved) < 0)
            {
                AccountReserved.Text = "";
                AccountReserved.Focus();
                MsgBox.ShowError("金额不能小于0！");
                return;
            }

            if (decimal.Parse(_CanCashQuota) < 0)
            {
                CanCashQuota.Text = "";
                CanCashQuota.Focus();
                MsgBox.ShowError("金额不能小于0！");
                return;
            }

            if (decimal.Parse(_ApplyQuota) < 0)
            {
                ApplyQuota.Text = "";
                ApplyQuota.Focus();
                MsgBox.ShowError("金额不能小于0！");
                return;
            }


            if (decimal.Parse(_ApplyQuota) > decimal.Parse(_CanCashQuota))
            {
                ApplyQuota.Text = "";
                ApplyQuota.Focus();
                MsgBox.ShowError("申请额度不能大于可提现额度！");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("AccountBalance", decimal.Parse(_AccountBanlace)));
                list.Add(new SqlPara("AccountName", _AccountName));
                list.Add(new SqlPara("AccountNO", _AccountNO));

                list.Add(new SqlPara("BankName", _BankName));
                list.Add(new SqlPara("BankAccount", _BankAccount));
                list.Add(new SqlPara("AccountReserved", decimal.Parse(_AccountReserved)));
                list.Add(new SqlPara("CanCashQuota", decimal.Parse(_CanCashQuota)));
                list.Add(new SqlPara("ApplyQuota", decimal.Parse(_ApplyQuota)));

                list.Add(new SqlPara("BankSubbranch", _BankSubbranch));
                list.Add(new SqlPara("BankAccountName", _BankAccountName));
                list.Add(new SqlPara("Remark", _Remark));

                list.Add(new SqlPara("SerialNO", _SerialNO));
                list.Add(new SqlPara("ApplyDate", CommonClass.gcdate));

                list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ApplyState", "新增"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLWITHDRAWCASHAPPLY", list);
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
