using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmBankAdjustUpt : BaseForm
    {
        public string Province = "",City="",
            BankMan = "", BankCode = "", BankName = "", id = "";
        public frmBankAdjustUpt()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Regex r = new Regex(@"^\d{6,50}$");
            Regex r2 = new Regex(@"[\u4e00-\u9fbb]");
            string oilCardAccountStr = textEdit17.Text.Trim().Replace(" ","");
            if (!r.IsMatch(oilCardAccountStr))
            {
                MsgBox.ShowError("请输入正确的银行账号!");
                return;
            }
             List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                list.Add(new SqlPara("bankman", textEdit1.Text.Trim()));

                list.Add(new SqlPara("bankcode", oilCardAccountStr));
                list.Add(new SqlPara("bankname", edbankname.Text.Trim()));
                list.Add(new SqlPara("sheng", oilCardProvince.Text));
                list.Add(new SqlPara("city", oilCardCity.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Update_B_Bank_Bill", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("修改成功！");
                    this.DialogResult = DialogResult.OK;
                    Close();
                }

        }
        private bool check()
        {
            return true;
        }

        private void frmBankAdjustUpt_Load(object sender, EventArgs e)
        {
            textEdit1.Text = BankMan;
            edbankname.Text = BankName;
            textEdit17.Text = BankCode;
            oilCardProvince.Text = Province;
            oilCardCity.Text = City;
            CommonClass.SetProvince(oilCardProvince);
            
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void oilCardProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCity(oilCardProvince, oilCardCity);
        }

      
      
        

       
    }
}
