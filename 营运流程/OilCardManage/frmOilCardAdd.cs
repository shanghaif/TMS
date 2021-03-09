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
    public partial class frmOilCardAdd : ZQTMS.Tool.BaseForm
    {
        public string _OilCardNo = "";
        static frmOilCardAdd foca;

        public frmOilCardAdd()
        {
            InitializeComponent();
        }

        public static frmOilCardAdd Get_frmOilCardAdd { get { if (foca == null || foca.IsDisposed) foca = new frmOilCardAdd(); return foca; } }

        private void frmOilCardAdd_Load(object sender, EventArgs e)
        {
            OilCardNo.Properties.ReadOnly = Account.Properties.ReadOnly = false;
            BuyDate.DateTime = CommonClass.gcdate;
        }

        public void getdata()
        {
            if (_OilCardNo == "") return;
            OilCardNo.Properties.ReadOnly = Account.Properties.ReadOnly = true;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OILCARD_BY_NO", new List<SqlPara> { new SqlPara("OilCardNo", _OilCardNo) }));

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            OilCardNo.EditValue = ds.Tables[0].Rows[0]["OilCardNo"];
            Company.EditValue = ds.Tables[0].Rows[0]["Company"];
            Account.EditValue = ds.Tables[0].Rows[0]["Account"];
            BuyDate.EditValue = ds.Tables[0].Rows[0]["BuyDate"];
            OilCardPsw.EditValue = ds.Tables[0].Rows[0]["OilCardPsw"];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int type = 0;// 新增
            if (_OilCardNo != "") type = 1;
            if (_OilCardNo == "") _OilCardNo = OilCardNo.Text.Trim();
            if (_OilCardNo == "")
            {
                MsgBox.ShowOK("请填写油卡号或稍后再试!");
                OilCardNo.Focus();
                return;
            }

            decimal account = ConvertType.ToDecimal(Account.Text);
            if (type == 0 && account < 0)
            {
                MsgBox.ShowOK("油卡金额不能小于0!");
                Account.Focus();
                return;
            }
            if (BuyDate.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择购卡日期!");
                BuyDate.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("OilCardNo", _OilCardNo));
            list.Add(new SqlPara("Company", Company.Text.Trim()));
            list.Add(new SqlPara("Account", account));
            list.Add(new SqlPara("BuyDate", BuyDate.DateTime));
            list.Add(new SqlPara("type", type));
            list.Add(new SqlPara("OilCardPsw", OilCardPsw.Text.Trim()));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_OILCARD", list)) == 0) return;
            _OilCardNo = OilCardNo.Text = Account.Text = "";
            BuyDate.DateTime = CommonClass.gcdate;
            if (MsgBox.ShowYesNo("保存成功,是否继续添加卡？") != DialogResult.Yes) this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}