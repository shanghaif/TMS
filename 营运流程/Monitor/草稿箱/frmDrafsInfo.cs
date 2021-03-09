using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
namespace ZQTMS.UI
{
    public partial class frmDrafsInfo : BaseForm
    {
        public frmDrafsInfo()
        {
            InitializeComponent();
        }
        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }



        public frmDrafsInfo(string billno, string varieties, string package, string num, string feevolume, string feeweight,  string volume, string weight)
        {
            InitializeComponent();

            txt_billno.Text = billno;
            txt_Varieties.Text = varieties;
            txt_Package.Text = package;
            txt_Num.Text = num;
            txt_FeeVolume.Text = feevolume;
            txt_FeeWeight.Text = feeweight;
            txt_Volume.Text = volume;
            txt_Weight.Text = weight;

       

        }

        private void frmDrafsInfo_Load(object sender, EventArgs e)
        {
          
        }

       private bool IsNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg1
                = new System.Text.RegularExpressions.Regex(@"^[-]?\d+[.]?\d*$");
            return reg1.IsMatch(str);
        }
       
        private void addgy()
        {
            try
            {
                if (!IsNumeric(txt_Num.Text.Trim()))
                {
                    MessageBox.Show("件数只能输入数字！");
                    txt_Num.SelectAll();
                    txt_Num.Focus();
                    return;
                }
                else if (!IsNumeric(txt_Volume.Text.Trim()))
                {
                    MessageBox.Show("体积只能输入数字！");
                    txt_Volume.SelectAll();
                    txt_Volume.Focus();
                    return;
                }
                else if (!IsNumeric(txt_Weight.Text.Trim()))
                {
                    MessageBox.Show("重量只能输入数字！");
                    txt_Weight.SelectAll();
                    txt_Weight.Focus();
                    return;

                }
                else if (!IsNumeric(txt_FeeVolume.Text.Trim()))
                {
                    MessageBox.Show("计费体积只能输入数字！");
                    txt_FeeVolume.SelectAll();
                    txt_FeeVolume.Focus();
                    return;
                }
                else if (!IsNumeric(txt_FeeWeight.Text.Trim()))
                {
                    MessageBox.Show("计费重量只能输入数字！");
                    txt_FeeWeight.SelectAll();
                    txt_FeeWeight.Focus();
                    return;
                }


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", txt_billno.Text.Trim()));
                list.Add(new SqlPara("Varieties", txt_Varieties.Text.Trim()));
                list.Add(new SqlPara("Package", txt_Package.Text.Trim()));
                list.Add(new SqlPara("FeeVolume", txt_FeeVolume.Text.Trim()));
                list.Add(new SqlPara("FeeWeight", txt_FeeWeight.Text.Trim()));
                list.Add(new SqlPara("Num", txt_Num.Text.Trim()));
                list.Add(new SqlPara("Volume", txt_Volume.Text.Trim()));
                list.Add(new SqlPara("Weight", txt_Weight.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_Update_Drafts", list);


                if (SqlHelper.ExecteNonQuery(sps) == 0) return;
                MsgBox.ShowOK();
                this.Close();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            addgy();

        }

    }
}