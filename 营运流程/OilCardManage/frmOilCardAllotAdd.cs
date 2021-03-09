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
    public partial class frmOilCardAllotAdd : ZQTMS.Tool.BaseForm
    {
        static frmOilCardAllotAdd foca;
        DataTable DtOilCardNo;

        public frmOilCardAllotAdd()
        {
            InitializeComponent();
        }

        public static frmOilCardAllotAdd Get_frmOilCardAllotAdd { get { if (foca == null || foca.IsDisposed) foca = new frmOilCardAllotAdd(); return foca; } }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardNo == "")
            {
                MsgBox.ShowOK("没有油卡卡号,请重试!");
                return;
            }
            decimal account = ConvertType.ToDecimal(Account.Text);
            if (account == 0)
            {
                MsgBox.ShowOK("分配金额不能等于0!");
                Account.Focus();
                return;
            }
            string chauffer = Chauffer.Text.Trim();
            if (chauffer == "")
            {
                MsgBox.ShowOK("请填写司机!");
                Chauffer.Focus();
                return;
            }
            string chaufferTel = ChaufferTel.Text.Trim();
            if (chaufferTel == "")
            {
                MsgBox.ShowOK("请填写司机电话!");
                ChaufferTel.Focus();
                return;
            }
            string vehicleNo = VehicleNo.Text.Trim();
            if (vehicleNo == "")
            {
                MsgBox.ShowOK("请填写车号!");
                VehicleNo.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("OilCardNo", oilCardNo));
            list.Add(new SqlPara("Company", Company.Text.Trim()));
            list.Add(new SqlPara("Chauffer", chauffer));
            list.Add(new SqlPara("ChaufferTel", chaufferTel));
            list.Add(new SqlPara("VehicleNo", vehicleNo));
            list.Add(new SqlPara("Account", account));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_OILCARD_ALLOT", list)) == 0) return;
            OilCardNo.Text = Chauffer.Text = ChaufferTel.Text = VehicleNo.Text = Account.Text = "";
            if (MsgBox.ShowYesNo("保存成功,是否继续分配？") != DialogResult.Yes)
            {
                this.Close();
                return;
            }
            //删除已分配的油卡
            DataRow[] drs = DtOilCardNo.Select("OilCardNo='" + oilCardNo + "'");
            if (drs == null || drs.Length == 0) return;
            DtOilCardNo.Rows.Remove(drs[0]);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOilCardAllotAdd_Load(object sender, EventArgs e)
        {
            //获取所有油卡卡号
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ALL_OILCARDNO_ALLOT"));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            //添加所属公司
            DtOilCardNo = ds.Tables[0];
            string company = "";
            Company.Properties.Items.Add("");
            foreach (DataRow dr in DtOilCardNo.Rows)
            {
                company = ConvertType.ToString(dr["Company"]);
                if (company != "" && !Company.Properties.Items.Contains(company))
                    Company.Properties.Items.Add(company);
            }
        }

        private void OilCardNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //获取油卡余额
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardNo == "") return;

            DataRow[] drs = DtOilCardNo.Select("OilCardNo='" + oilCardNo + "'");
            if (drs == null || drs.Length == 0) return;
            Account.EditValue = drs[0]["Balance"];
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