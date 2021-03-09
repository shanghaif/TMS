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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmStationBaseInfoAdd : BaseForm
    {
        public DataRow dr = null;
        public frmStationBaseInfoAdd()
        {
            InitializeComponent();
        }

        private void frmStationBaseInfoAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.SetCauseWeb(comboBoxEdit1,"全部", "全部");
            comboBoxEdit1.Properties.Items.Remove("全部");
            comboBoxEdit1.Text = "";
            if (dr != null)
            {
                comboBoxEdit1.EditValue = dr["StationName"];
                textEdit2.EditValue = dr["StationArea"];
                textEdit3.EditValue = dr["StationMonthRent"];
                textEdit4.EditValue = dr["WorkManNum"];
                textEdit5.EditValue = dr["WorkManMonthPay"];
                textEdit6.EditValue = dr["ForkliftNum"];
                textEdit7.EditValue = dr["StationSumCost"];
                textEdit8.EditValue = dr["MonthWaterElectric"];
                textEdit9.EditValue = dr["eForkliftMonthRent"];
                textEdit10.EditValue = dr["eForkliftNum"];
                textEdit11.EditValue = dr["ForkliftMonthOil"];
                textEdit12.EditValue = dr["ForkliftMonthRent"];
                dateEdit1.EditValue = dr["Year"];
                dateEdit2.EditValue = dr["Month"];
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", dr == null ? Guid.NewGuid() : dr["id"]));
                list.Add(new SqlPara("Year", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("Month", dateEdit2.Text.Trim()));
                list.Add(new SqlPara("StationName", comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("StationArea", textEdit2.Text.Trim()));
                list.Add(new SqlPara("StationMonthRent", textEdit3.Text.Trim()));
                list.Add(new SqlPara("WorkManNum", textEdit4.Text.Trim()));
                list.Add(new SqlPara("WorkManMonthPay", textEdit5.Text.Trim()));
                list.Add(new SqlPara("ForkliftNum", textEdit6.Text.Trim()));
                list.Add(new SqlPara("ForkliftMonthRent", textEdit12.Text.Trim()));
                list.Add(new SqlPara("ForkliftMonthOil", textEdit11.Text.Trim()));
                list.Add(new SqlPara("eForkliftNum", textEdit10.Text.Trim()));
                list.Add(new SqlPara("eForkliftMonthRent", textEdit9.Text.Trim()));
                list.Add(new SqlPara("MonthWaterElectric", textEdit8.Text.Trim()));
                list.Add(new SqlPara("StationSumCost", textEdit7.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_StationBaseInfo", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }

        private void AddSumCost()
        {
            decimal sumcost = 0;
            sumcost = Convert.ToDecimal(textEdit3.Text.Trim()) + Convert.ToDecimal(textEdit5.Text.Trim()) + Convert.ToDecimal(textEdit12.Text.Trim())
                      + Convert.ToDecimal(textEdit11.Text.Trim()) + Convert.ToDecimal(textEdit9.Text.Trim()) + Convert.ToDecimal(textEdit8.Text.Trim());
            textEdit7.Text = Convert.ToString(sumcost);
        }

        private void textEdit3_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }

        private void textEdit5_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }

        private void textEdit12_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }

        private void textEdit11_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }

        private void textEdit9_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }

        private void textEdit8_EditValueChanged(object sender, EventArgs e)
        {
            AddSumCost();
        }
    }
}
