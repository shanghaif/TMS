using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmAccountAduitModify : BaseForm
    {
        public frmAccountAduitModify()
        {
            InitializeComponent();
        }
        public string VoucherNo1 = "", VerifyOffType1 = "", BillType1 = "", hm1 = "", zh1 = "", khh1 = "";

        private void frmAccountAduitModify_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            VoucherNo.EditValue = VoucherNo1;
            textEdit1.EditValue = VerifyOffType1;
            edbilltype.EditValue = BillType1;
            hm.EditValue = hm1;
            zh.EditValue = zh1;
            khh.EditValue = khh1;

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSPARAMSETTING_1");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            string[] str = ds.Tables[0].Rows[0]["ParamValue"].ToString().Split(',');
            if (str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    edbilltype.Properties.Items.Add(str[i]);
                }
            }
        }
        //保存
        private void cbsave_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VoucherNo", VoucherNo1));
            list.Add(new SqlPara("VerifyOffType", VerifyOffType1));
            list.Add(new SqlPara("BillType", edbilltype.Text.Trim()));
            list.Add(new SqlPara("hm", hm.Text.Trim()));
            list.Add(new SqlPara("zh", zh.Text.Trim()));
            list.Add(new SqlPara("khh", khh.Text.Trim()));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_UPDATE_BILLACCOUNT_Audit_BankInfo", list)) == 0) return;
            MsgBox.ShowOK("保存成功");
            this.Close();
        }
        //关闭
        private void cbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}