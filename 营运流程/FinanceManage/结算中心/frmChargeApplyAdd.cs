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
    public partial class frmChargeApplyAdd : BaseForm
    {
        private string AccountNo;

        public frmChargeApplyAdd()
        {
            InitializeComponent();
        }

        private void frmChargeApplyAdd_Load(object sender, EventArgs e)
        {
            SerialNO.EditValue = "GZ" + CommonClass.gcdate.ToString("yyyyMMddHHmmss") + "-" + (new Random().Next(9999));
            chargedate.DateTime = CommonClass.gcdate;
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

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 ||
                    ds.Tables[0].Rows[0]["AccountNo"] == null || string.IsNullOrEmpty(ds.Tables[0].Rows[0]["AccountNo"].ToString()) ) 
                {
                    MsgBox.ShowError("获取当前网点信息失败，请与管理员联系！");
                    this.Close();
                }
                AccountNo = ds.Tables[0].Rows[0]["AccountNo"].ToString(); 
            }
            catch
            {
                MsgBox.ShowError("获取当前网点信息失败，请与管理员联系！");
                this.Close();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)//上传
        {
            frmFileUpload_CenterAcc frm = new frmFileUpload_CenterAcc();
            frm.ShowDialog();

            upLoadImgText.Text = frm.ResultPath.Trim();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string _bankname = bankname.Text.Trim();
            string _bankAccount = bankAccount.Text.Trim();
            string _bankaccountname = bankaccountname.Text.Trim();

            string _SerialNO = SerialNO.Text.Trim();
            string _chargeDate = chargedate.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string _chargebankaccount = chargebankaccount.Text.Trim();
            string _chargebankaccountname = chargebankaccountname.Text.Trim();
            string _chargebankname = chargebankname.Text.Trim();
            string _chargefee = chargefee.Text.Trim();
            string _chargeway = chargeway.Text.Trim();
            string filepath = upLoadImgText.Text.Trim();

            if (string.IsNullOrEmpty(_bankname) || string.IsNullOrEmpty(_bankAccount) || string.IsNullOrEmpty(_bankaccountname) ||
                string.IsNullOrEmpty(_SerialNO) || string.IsNullOrEmpty(_chargeDate) || string.IsNullOrEmpty(_chargebankaccount) ||
                string.IsNullOrEmpty(_chargebankaccountname) || string.IsNullOrEmpty(_chargebankname) || string.IsNullOrEmpty(_chargefee) ||
                string.IsNullOrEmpty(_chargeway) || string.IsNullOrEmpty(filepath)
                )
            {
                MsgBox.ShowError("所有数据都不能为空！");
                return;
            }

            try
            {
                decimal.Parse(_chargefee);
            }
            catch
            {
                chargefee.Text = "";
                chargefee.Focus();
                MsgBox.ShowError("金额不合法！");
                return;
            }


            if (decimal.Parse(_chargefee) < 0)
            {
                chargefee.Text = "";
                chargefee.Focus();
                MsgBox.ShowError("金额不能小于0！");
                return;
            }

            try
            { 
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("AccountNo", AccountNo));//网点账户
                list.Add(new SqlPara("SCBankName", _bankname));
                list.Add(new SqlPara("SCBankAccount", _bankAccount));
                list.Add(new SqlPara("SCBankAccountName", _bankaccountname));

                list.Add(new SqlPara("SerialNO", _SerialNO));
                list.Add(new SqlPara("ChargeDate", _chargeDate));
                list.Add(new SqlPara("BankName", _chargebankname));
                list.Add(new SqlPara("BankAccount", _chargebankaccount));

                list.Add(new SqlPara("BankAccountName", _chargebankaccountname));
                list.Add(new SqlPara("ChargeFee", _chargefee));
                list.Add(new SqlPara("ChargeWay", _chargeway));
                list.Add(new SqlPara("ApplyDate", CommonClass.gcdate));

                list.Add(new SqlPara("ApplyMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ApplyState", "新增"));
                list.Add(new SqlPara("VoucherImg", filepath));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLCHARGEAPPLY", list);
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
