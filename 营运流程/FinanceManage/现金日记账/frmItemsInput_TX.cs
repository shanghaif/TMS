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
    public partial class frmItemsInput_TX : BaseForm
    {
        public frmItemsInput_TX()
        {
            InitializeComponent();
        }
        public string VoucherNo1 = "", OpType = "";

        private void frmItemsInput_TX_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            WebName.Text = CommonClass.UserInfo.WebName;
            edcreateby.Text = CommonClass.UserInfo.UserName;
            if (VoucherNo1 != "")
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", VoucherNo1));
                SqlParasEntity spst = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_ByVoucherNo", list);
                DataSet dt = SqlHelper.GetDataSet(spst);
                if (dt == null || dt.Tables[0] == null || dt.Tables[0].Rows.Count == 0) return;
                WebName.Text = dt.Tables[0].Rows[0]["WebName"].ToString();
                edcreateby  .Text = dt.Tables[0].Rows[0]["OptMan"].ToString();
                edinouttype.Text = dt.Tables[0].Rows[0]["InOutType"].ToString();
                edbilltype.Text = dt.Tables[0].Rows[0]["BillType"].ToString();
                edaccount.Text = dt.Tables[0].Rows[0]["Money"].ToString();
                edTheBillType.Text = dt.Tables[0].Rows[0]["TheBillType"].ToString();
                Type.Text = dt.Tables[0].Rows[0]["zzlx"].ToString();
                txtSummary.Text = dt.Tables[0].Rows[0]["Summary"].ToString();
                edsjno.Text = dt.Tables[0].Rows[0]["sjno"].ToString();
                edfpno.Text = dt.Tables[0].Rows[0]["fpno"].ToString();
                edzpno.Text = dt.Tables[0].Rows[0]["zpno"].ToString();
                hm.Text = dt.Tables[0].Rows[0]["hm"].ToString();
                zh.Text = dt.Tables[0].Rows[0]["zh"].ToString();
                khh.Text = dt.Tables[0].Rows[0]["khh"].ToString();
                szs.Text = dt.Tables[0].Rows[0]["szs"].ToString();
                szshi.Text = dt.Tables[0].Rows[0]["szshi"].ToString();
            }

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
            if (OpType == "新增")
            {
                Random r1 = new Random();
                int a1 = r1.Next(10, 100);
                VoucherNo1 = (edinouttype.Text == "支出" ? "O" : "I") + CommonClass.gcdate.ToString("yyyyMMddHHmmss")+a1;
            }
            decimal account = ConvertType.ToDecimal(edaccount.Text);
            if (account <= 0)
            {
                MsgBox.ShowOK("请填写发生金额!");
                edaccount.Focus();
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VoucherNo", VoucherNo1));
            list.Add(new SqlPara("OpType", OpType));
            list.Add(new SqlPara("InoutType", edinouttype.Text.Trim()));
            list.Add(new SqlPara("OptTime", CommonClass.gcdate));
            list.Add(new SqlPara("BillType", edbilltype.Text.Trim()));
            list.Add(new SqlPara("TheBillType", edTheBillType.Text.Trim()));
            list.Add(new SqlPara("Money", account));
            list.Add(new SqlPara("Summary", txtSummary.Text.Trim()));
            list.Add(new SqlPara("sjno", edsjno.Text.Trim()));
            list.Add(new SqlPara("fpno", edfpno.Text.Trim()));
            list.Add(new SqlPara("zpno", edzpno.Text.Trim()));
            list.Add(new SqlPara("szs", szs.Text.Trim()));
            list.Add(new SqlPara("szshi", szshi.Text.Trim()));
            list.Add(new SqlPara("zzlx", Type.Text.Trim()));
            list.Add(new SqlPara("hm", hm.Text.Trim()));
            list.Add(new SqlPara("zh", zh.Text.Trim()));
            list.Add(new SqlPara("khh", khh.Text.Trim())); 
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BILLACCOUNT_2", list)) == 0) return;
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