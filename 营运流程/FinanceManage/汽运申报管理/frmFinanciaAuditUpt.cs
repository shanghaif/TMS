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
    public partial class frmFinanciaAuditUpt : BaseForm
    {
        public string BbusinessDate = "", DepartureBatch = "", CarNO = "", DriverName = "", LineName = "", FeeType = "", Province = "",City="",
            BankMan = "", BankCode = "", BankName = "", AID = "";
        public frmFinanciaAuditUpt()
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
            string oilCardAccountStr = textEdit17.Text.Trim();
            if (!r.IsMatch(oilCardAccountStr))
            {
                MsgBox.ShowError("请输入正确的银行账号!");
                return;
            }
             List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AID", AID));
                list.Add(new SqlPara("BankMan", textEdit1.Text.Trim()));
                
                list.Add(new SqlPara("BankCode", edbankname.Text.Trim()));
                list.Add(new SqlPara("BankName", textEdit17.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("FeeType", FeeType));
                list.Add(new SqlPara("Province", oilCardProvince.Text));
                list.Add(new SqlPara("City", oilCardCity.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_FinancialAudit_bank", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    Close();
                }

        }
        private bool check()
        {
            return true;
        }

        private void frmFinanciaAuditUpt_Load(object sender, EventArgs e)
        {
            textEdit5.Text = DepartureBatch;
            textEdit5.Enabled = false;
            Registrant.Text = BbusinessDate;
            Registrant.Enabled = false;
            textEdit2.Text = CarNO;
            textEdit2.Enabled = false;
            textEdit3.Text = DriverName;
            textEdit3.Enabled = false;
            textEdit4.Text = LineName;
            textEdit4.Enabled = false;
            textEdit6.Text = FeeType;
            textEdit6.Enabled = false;

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
