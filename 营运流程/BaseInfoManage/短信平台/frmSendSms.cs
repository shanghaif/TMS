using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSendSms : BaseForm
    {

        public string shipper = "", content = "", createby = "", mb = "", billdate = "";
     
        public frmSendSms()
        {
            InitializeComponent();
        }

        private void sendsms_Load(object sender, EventArgs e)
        {
            edbilldate.Text = billdate;
            edshipper.Text = shipper;
            edmb.Text = mb;
            edcontent.Text = content;
            edcreateby.Text = createby;           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string shipper = edshipper.Text.ToString().Trim();
                string mb = edmb.Text.ToString().Trim();
                string content = edcontent.Text.ToString().Trim();
                string createby = edcreateby.Text.ToString().Trim();
                sms.onesendsms(shipper, mb, content, createby);
                XtraMessageBox.Show("发送成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edcontent_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            int len = edcontent.Text.Trim().Length;
            label8.Text = len.ToString() + " / 70  拆分条数：" + Math.Ceiling((decimal)len / (decimal)70).ToString();
        }

        private void edcontent_EditValueChanged(object sender, EventArgs e)
        {
            int len = edcontent.Text.Trim().Length;
            label8.Text = len.ToString() + " / 70  拆分条数：" + Math.Ceiling((decimal)len / (decimal)70).ToString();
        }
    }
}
