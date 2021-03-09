using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmOilCardRechargeAdd : ZQTMS.Tool.BaseForm
    {
        static frmOilCardRechargeAdd foca;
        DataTable DtOilCardNo;

        public frmOilCardRechargeAdd()
        {
            InitializeComponent();
        }

        public static frmOilCardRechargeAdd Get_frmOilCardRechargeAdd { get { if (foca == null || foca.IsDisposed) foca = new frmOilCardRechargeAdd(); return foca; } }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardNo == "")
            {
                MsgBox.ShowOK("没有油卡编号,不能充值!");
                return;
            }

            decimal account = ConvertType.ToDecimal(Account.Text);
            if (account == 0)
            {
                MsgBox.ShowOK("充值金额不能等于0!");
                Account.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Company", Company.Text.Trim()));
            list.Add(new SqlPara("OilCardNo", oilCardNo));
            list.Add(new SqlPara("Account", account));

            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_OILCARD_RECHARGE", list)) == 0) return;

                Account.Text = Remark.Text = "";
                if (MsgBox.ShowYesNo("是否继充值？") != DialogResult.Yes) this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("CHECK")) MsgBox.ShowOK("充值失败，原因：油卡余额不能小于0！");
                else MsgBox.ShowOK("充值失败，原因如下\r\n" + ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOilCardRechargeAdd_Load(object sender, EventArgs e)
        {
            //获取所有油卡卡号
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ALL_OILCARDNO"));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DtOilCardNo = ds.Tables[0];

            string company = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                company = ConvertType.ToString(dr["Company"]);
                if (company != "" && !Company.Properties.Items.Contains(company))
                    Company.Properties.Items.Add(company);
            }
        }

        private void Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            OilCardNo.Text = Account.Text = "";
            OilCardNo.Properties.Items.Clear();
            if (DtOilCardNo == null || DtOilCardNo.Rows.Count == 0) return;

            DataRow[] drs = DtOilCardNo.Select("Company='" + Company.Text.Trim() + "'");
            if (drs == null || drs.Length == 0) return;
            string oilCard = "";
            foreach (DataRow dr in drs)
            {
                oilCard = ConvertType.ToString(dr["OilCardNo"]);
                if (oilCard != "" && !OilCardNo.Properties.Items.Contains(oilCard))
                    OilCardNo.Properties.Items.Add(oilCard);
            }
        }
    }
}