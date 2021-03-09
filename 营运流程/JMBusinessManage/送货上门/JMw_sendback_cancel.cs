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


namespace ZQTMS
{
    public partial class JMw_sendback_cancel : BaseForm
    {

        public JMw_sendback_cancel()
        {
            InitializeComponent();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            uncheck();
        }

        private void uncheck()
        {
            if (edunit.Text.Trim() == "")
            {
                edunit.Focus();
                return;
            }
            else
            {
                
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int unit = -1;
            if (edunit.Text.Trim() == "") 
            {
                XtraMessageBox.Show("请输入货号或者运单号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
            }

            if (comboBoxEdit1.SelectedIndex == 1)
            {
                //cc.ShowBillDetail(edunit.Text.Trim());
            }
            else
            {
                unit = Convert.ToInt32(edunit.Text.Trim());
                //cc.ShowBillDetail(unit);
            }
        }
        
    }
}
