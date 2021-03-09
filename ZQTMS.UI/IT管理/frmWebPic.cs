using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using ZQTMS.Common;
using ZQTMS.UI.Properties;
using System.Net;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmWebPic : BaseForm
    {
        public frmWebPic()
        {
            InitializeComponent();
        }

        int len = 0;
        WebClient wc = new WebClient();

        private void frmWebPic_Load(object sender, EventArgs e)
        {
            wc.Proxy = null;
            wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
            wc.UploadFileCompleted += new UploadFileCompletedEventHandler(wc_UploadFileCompleted);
        }

        private void buttonEdit1_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Ellipsis)
            {
                OpenFileDialog of = new OpenFileDialog();
                of.Multiselect = false;
                of.Filter = "*.jpg|*.jpg";
                of.Title = "请选择jpg格式的图片!";
                DialogResult dl = of.ShowDialog();

                if (dl != DialogResult.OK)
                {
                    return;
                }
                buttonEdit1.Text = of.FileName;

                #region 文件信息
                try
                {
                    string path = buttonEdit1.Text;
                    if (!File.Exists(path)) return;

                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                    int byteLength = (int)fs.Length;
                    byte[] fileBytes = new byte[byteLength];
                    fs.Read(fileBytes, 0, byteLength);
                    fs.Close();
                    fs.Dispose();

                    Image img = Image.FromStream(new MemoryStream(fileBytes));
                    len = byteLength / 1024;

                    textEdit1.EditValue = string.Format("{0} × {1}", img.Size.Width, img.Size.Height);
                    textEdit2.EditValue = len;
                }
                catch (Exception)
                {
                }
                #endregion
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string path = buttonEdit1.Text.Trim();
                if (path == "")
                {
                    MsgBox.ShowOK("请选择要上传的文件!");
                    return;
                }
                if (!File.Exists(path))
                {
                    MsgBox.ShowOK("选择的文件已经不存在，请重新选择!");
                    return;
                }
                simpleButton2.Enabled = false;
                buttonEdit1.Enabled = false;
                if (len > 400)
                {
                    MsgBox.ShowOK("上传的图片文件太大!");
                    return;
                }
                progressBarControl1.Visible = true;
                wc.UploadFileAsync(new Uri(HttpHelper.WebPicUrl), "POST", path);

            }
            catch (Exception ex)
            {
                MsgBox.ShowError("上传图片异常。" + ex.Message);
            }
            finally
            {
                progressBarControl1.Visible = false;
                simpleButton2.Enabled = true;
                buttonEdit1.Enabled = true;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            progressBarControl1.Position = e.ProgressPercentage;
        }

        private void wc_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);

            if (result.State == 1)
            {
                try
                {
                    string path = buttonEdit1.Text.Trim();
                    FileInfo fi = new FileInfo(path);

                    string _FileName = Path.GetFileName(path);
                    string _FileMD5 = GetFileMD5(path);

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("FileName", _FileName));
                    list.Add(new SqlPara("FileMD5", _FileMD5));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WebPic", list);
                    SqlHelper.ExecteNonQuery(sps);
                    if (MsgBox.ShowYesNo("上传成功!是否在浏览器中预览刚刚上传的文件？") == DialogResult.Yes)
                    {
                        string url = "http://ZQTMS.dekuncn.com:7011/WebPic/mainweb.jpg";
                        System.Diagnostics.Process.Start(url);
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError("上传记录保存失败：\r\n" + ex.Message);
                }
            }
        }

        private string GetFileMD5(string filePath)
        {
            try
            {
                string temp = filePath + ".md5temp";
                if (File.Exists(temp))
                {
                    File.Delete(temp);
                }
                File.Copy(filePath, temp, true);

                byte[] retVal;
                using (FileStream file = new FileStream(temp, FileMode.Open))
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    retVal = md5.ComputeHash(file);
                    file.Close();
                    file.Dispose();
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                File.Delete(temp);

                return sb.ToString().ToUpper();
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件MD5失败：" + ex.Message);
            }
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