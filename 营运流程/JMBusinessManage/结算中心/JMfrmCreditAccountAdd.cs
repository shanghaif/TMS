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
    public partial class JMfrmCreditAccountAdd : BaseForm
    {
        public DataRow dr = null;
        private DataTable dt = null;
        public JMfrmCreditAccountAdd()
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
                if (AccountName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请选择账户名称");
                    return;
                }
                if (CreditLimit.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请输入授信额度");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CreditAccountID", dr == null ? Guid.NewGuid() : dr["CreditAccountID"]));
                list.Add(new SqlPara("AccountName", AccountName.Text.Trim()));
                list.Add(new SqlPara("AccountNo", AccountNo.Text.Trim()));
                list.Add(new SqlPara("AccountType", AccountType.Text.Trim()));
                list.Add(new SqlPara("CreditLimit", CreditLimit.Text.Trim()));
                list.Add(new SqlPara("StartTime", StartTime.Text.Trim()));
                list.Add(new SqlPara("EndTime", EndTime.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_CREDITACCOUNT", list);
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

        private void JMfrmCreditAccountAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            LoadSettlement();
            StartTime.DateTime = CommonClass.gbdate;
            EndTime.DateTime = CommonClass.gedate;
            if (dr != null)
            {
                AccountNo.Text = dr["AccountNo"].ToString();
                AccountName.Text = dr["AccountName"].ToString();
                AccountType.Text = dr["AccountType"].ToString();
                CreditLimit.Text = dr["CreditLimit"].ToString();

                if (!string.IsNullOrEmpty(dr["StartTime"].ToString()))
                {
                    StartTime.DateTime = ConvertType.ToDateTime(dr["StartTime"]);
                }
                if (!string.IsNullOrEmpty(dr["EndTime"].ToString()))
                {
                    EndTime.DateTime = ConvertType.ToDateTime(dr["EndTime"]);
                }
            }
        }

        private void LoadSettlement()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSETTLECENTERACC_ByCredit");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            AccountName.Properties.Items.Add(row["AccountName"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void AccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select("AccountName='" + AccountName.Text.Trim() + "'");
                if (rows != null && rows.Length > 0)
                {
                    AccountNo.Text = rows[0]["AccountNO"].ToString();
                    AccountType.Text = rows[0]["AccountType"].ToString();
                }
            }
        }

    }
}
