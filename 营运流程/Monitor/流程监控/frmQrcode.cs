using ZQTMS.Tool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.UI
{
    public partial class frmQrcode : Form
    {
        public frmQrcode()
        {
            InitializeComponent();
        }

        public string payWay = "";//ch:传化字符
        public string payCode = "";
        private void frmQrcode_Load(object sender, EventArgs e)
        {
            string[] code = Unionpay.GetCode().ToString().Split('|');
            if (payWay == "ch")
            {
                code = payCode.Split('|');
            }
            string url = code[1].ToString();
            string info = code[0].ToString();
            switch (info)
            {
                case "1": label2.Text = "支付宝"; break;
                case "2": label2.Text = " 微信"; break;
                case "3": label2.Text = " 银联"; break;
            }
            if (code.Length == 3)
            {
                this.linkLabel1.Links.Add(0, 4, code[2]);
            }
            if (url != null && url.Length > 21)
            {
                //pictureBox1.Image = QrCodes.GenerateQRCode(url, Color.Black, Color.White, 8);
                pictureBox1.BackgroundImage = QrCodes.GenerateQRCode(url, Color.Black, Color.White, 8);
                pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;

            }
            else
            {
                label4.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.linkLabel1.Links[this.linkLabel1.Links.IndexOf(e.Link)].Visited = true;
            string targetUrl = e.Link.LinkData as string;
            if (string.IsNullOrEmpty(targetUrl))
                MessageBox.Show("请钉钉联系：中强IT");
            else
                System.Diagnostics.Process.Start(targetUrl);
        }
    }
}
