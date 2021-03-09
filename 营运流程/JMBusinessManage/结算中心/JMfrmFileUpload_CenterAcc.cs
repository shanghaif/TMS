using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using System.IO;
using System.Net;
using ZQTMS.Common;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class JMfrmFileUpload_CenterAcc : BaseForm
    {
        string dirpath = Application.StartupPath + "\\TempFiles";
        WebClient wc = new WebClient();

        public string ResultPath = "";

        public JMfrmFileUpload_CenterAcc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="directory">文件夹</param>
        private void GetFiles()
        {

            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                labelControl3.Text = this.openFileDialog1.FileName;
            }
        }


        private void UpLoadFiles()
        {
            string path = labelControl3.Text.Trim();
            if (path == "未选择")
            {
                MsgBox.ShowError("请选择一个文件！");
                return;
            }


            ResultPath = string.Format("/Files/{0}/{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path));
            try
            {
                this.Invoke((EventHandler)(delegate
                {
                    labelControl4.Text = "文件上传中，请稍后";
                    simpleButton1.Enabled = false;
                    simpleButton2.Enabled = false;
                    simpleButton3.Enabled = false;
                    timer1.Enabled = true;
                }));

                byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", ResultPath, FileUpload.UpFileUrl)), "POST", path);
                string json = Encoding.UTF8.GetString(bt);
                ZQTMS.Tool.frmUpLoadFile.UploadResult result = JsonConvert.DeserializeObject<ZQTMS.Tool.frmUpLoadFile.UploadResult>(json);

                this.Invoke((EventHandler)(delegate
                {
                    if (result.State == 1)
                    {
                        labelControl4.Text = "上传成功!";
                    }
                    else
                    {
                        labelControl4.Text = "上传失败!";
                    }

                    timer1.Enabled = false;
                    simpleButton1.Enabled = true;
                    simpleButton2.Enabled = true;
                    simpleButton3.Enabled = true;
                }));
            }
            catch (Exception)
            {
                this.Invoke((EventHandler)(delegate
                {
                    labelControl4.Text = "上传失败";
                }));
                timer1.Enabled = false;
                simpleButton1.Enabled = true;
                simpleButton2.Enabled = true;
                simpleButton3.Enabled = true;
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            UpLoadFiles();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GetFiles();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (labelControl4.Text.Trim().Length <= 20)
            {
                labelControl4.Text += ".";
            }
            else
                labelControl4.Text = "文件上传中，请稍后";

        }

        private void JMfrmFileUpload_CenterAcc_Load(object sender, EventArgs e)
        {

        }


    }
}
