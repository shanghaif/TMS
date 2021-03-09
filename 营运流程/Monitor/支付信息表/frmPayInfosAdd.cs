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
    public partial class frmPayInfosAdd : BaseForm
    {
        public DataRow dr = null;
        public frmPayInfosAdd()
        {
            InitializeComponent();
        }

        private void DAPINGcostStandardAdd_Load(object sender, EventArgs e)
        {
            opt_date.EditValue = CommonClass.gbdate;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                decimal a = 0;
                if (tx_type.Text == "莞通银联")
                {
                    a = 0;
                }
                if (tx_type.Text == "微信支付")
                {
                    a = 1;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("balance_money", balance_money.Text.Trim()));
                list.Add(new SqlPara("remark", remark.Text.Trim()));
                list.Add(new SqlPara("posId", posId.Text.Trim()));
                list.Add(new SqlPara("terminalNo", terminalNo.Text.Trim()));
                list.Add(new SqlPara("haccount_type", haccount_type.Text.Trim()));
                list.Add(new SqlPara("opt_date", opt_date.Text.Trim()));
                list.Add(new SqlPara("alipay_sn", alipay_sn.Text.Trim()));
                list.Add(new SqlPara("tx_type", a));
                list.Add(new SqlPara("billno", billno.Text.Trim()));
                list.Add(new SqlPara("MerchantNo", MerchantNo.Text.Trim()));
                list.Add(new SqlPara("procedureFee", procedureFee.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_PayInfos", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
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

      


    }
}