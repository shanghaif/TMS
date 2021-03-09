using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmConfirmWithTxt : BaseForm
    {
        public string Reson;
        public string Type;

        public frmConfirmWithTxt()
        {
            InitializeComponent();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            Reson = textBox1.Text.Trim();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Reson = textBox1.Text.Trim();
        } 

        private void frmConfirmWithTxt_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Type.Trim()))
            {
                this.Text = "确定是否进行【" + Type + "】操作？";
                label1.Text = "【" + Type + "】原因：";
            }
        }


    }
}
