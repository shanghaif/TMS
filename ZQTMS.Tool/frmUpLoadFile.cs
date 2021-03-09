using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using ZQTMS.Lib;
using Newtonsoft.Json;

namespace ZQTMS.Tool
{
    public partial class frmUpLoadFile : BaseForm
    {
        public frmUpLoadFile()
        {
            InitializeComponent();
        }

        WebClient wc = new WebClient();
        Queue<ListItem> fileQueue = new Queue<ListItem>();
        ListItem UploadingItem = null;//正在上传的项

        private void frmUpLoadFile_Load(object sender, EventArgs e)
        {
            wc.Proxy = null;
            wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
            wc.UploadFileCompleted += new UploadFileCompletedEventHandler(wc_UploadFileCompleted);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //OpenFileDialog of = new OpenFileDialog();
            //of.Filter = "图片文件(*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tif|所有文件(*.*)|*.*";
            //of.FilterIndex = 1;
            //of.Multiselect = true;
            //if (of.ShowDialog() != DialogResult.OK)
            //{
            //    return;
            //}
            //foreach (string file in of.FileNames)
            //{
            //    ListItem item = new ListItem(Path.GetFileName(file), file);
            //    fileList1.AddFile(item);
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (fileList1.Items.Count == 0)
            {
                MsgBox.ShowOK("请选择要上传的文件!");
                return;
            }
            foreach (ListItem item in fileList1.Items)
            {
                fileQueue.Enqueue(item);
            }

            progressBarControl1.Visible = true;
            labelControl1.Visible = true;

            Upload();
        }

        private void Upload()
        {
            try
            {
                if (fileQueue.Count == 0)
                {
                    progressBarControl1.Visible = false;
                    labelControl1.Visible = false;
                    wc.Dispose();
                    if (checkEdit1.Checked)
                    {
                        this.Close();
                    }
                    return;
                }

                UploadingItem = fileQueue.Dequeue();
                labelControl1.Text = string.Format("正在上传： {0} / {1}", UploadingItem.Index + 1, fileList1.Items.Count);
                wc.UploadFileAsync(new Uri("http://ZQTMS.dekuncn.com:7020/FileUpLoad.ashx?BillNo=12345&BillType=2"), "POST", UploadingItem.FilePath);
            }
            catch (Exception)
            {
            }
        }

        void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            progressBarControl1.Position = e.ProgressPercentage;
        }

        void wc_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);

            if (result.State == 1 && UploadingItem != null)
            {
                UploadingItem.IsUpload = true;
                UploadingItem.ButtonEdit.Properties.Buttons[0].Visible = true;//OK按钮
            }

            Upload();
        }

        /// <summary>
        /// 上传结果
        /// </summary>
        public class UploadResult
        {
            /// <summary>
            /// 保存结果：0失败  1成功
            /// </summary>
            public int State
            {
                get;
                set;
            }

            /// <summary>
            /// 失败原因
            /// </summary>
            public string Error
            {
                get;
                set;
            }
        }
    }
}
