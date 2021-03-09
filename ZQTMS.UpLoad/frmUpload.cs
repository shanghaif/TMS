using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace ZQTMS.UpLoad
{
    public partial class frmUpload : Form
    {
        public frmUpload()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 文件列表
        /// <para>FileName 文件名，含后缀，无路径</para>
        /// <para>UpState 上传结果 1：上传成功</para>
        /// <para>AppPath 客户端路径</para>
        /// <para>FileLen 压缩前的字节数</para>
        /// <para>FullPath 完整路径</para>
        /// </summary>
        DataTable dt = new DataTable();

        WebClient wc = new WebClient();

        int CurrentIndex = 0;//正在上传第x个文件
        string zipPath = "";//正在上传的压缩文件路径

        Queue<DataRow> fileQueue = new Queue<DataRow>();

        private void frmUpload_Load(object sender, EventArgs e)
        {
            myGridView1.CustomUnboundColumnData += (ss, ee) => { if (ee.Column.FieldName == "rowid") { ee.Value = (object)(ee.RowHandle + 1); } };
            dt.Columns.Add("FileName", typeof(string)); //文件名，含后缀，无路径
            dt.Columns.Add("UpState", typeof(int));   //上传结果
            dt.Columns.Add("AppPath", typeof(string));  //客户端路径
            dt.Columns.Add("FileLen", typeof(int));   //字节数
            dt.Columns.Add("FullPath", typeof(string));   //完整路径
            myGridControl1.DataSource = dt;

            wc.Proxy = null;
            wc.UploadProgressChanged += new UploadProgressChangedEventHandler(wc_UploadProgressChanged);
            wc.UploadFileCompleted += new UploadFileCompletedEventHandler(wc_UploadFileCompleted);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = true;
            DialogResult dl = of.ShowDialog();
            if (dl == DialogResult.OK)
            {
                string[] files = of.FileNames;
                foreach (string item in files)
                {
                    FileInfo fi = new FileInfo(item);

                    DataRow dr = dt.NewRow();
                    dr["FileName"] = Path.GetFileName(item);
                    dr["UpState"] = 0;
                    dr["FileLen"] = fi.Length;
                    dr["FullPath"] = item;

                    dt.Rows.Add(dr);
                }
                dt.AcceptChanges();
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            myGridView1.DeleteRow(rowhandle);
            dt.AcceptChanges();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            #region 基本信息检测
            myGridView1.ClearColumnsFilter();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("没有要添加要上传的文件!");
                return;
            }
            if (dt.Select("UpState=0").Length == 0)
            {
                MessageBox.Show("文件已上传完毕，没有未上传的文件!");
                return;
            }

            if (dt.Select("len(AppPath)=0 or AppPath is null").Length > 0)
            {
                MessageBox.Show("必须为上传的文件选择客户端目录!");
                return;
            }
            if (MessageBox.Show("确定上传？\r\n请确认目录是否指定正确？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            #endregion

            CurrentIndex = 0;
            fileQueue.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                fileQueue.Enqueue(dr);
            }

            progressBarControl1.Visible = true;
            labelControl1.Visible = true;
            SetState(false);

            Upload();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetState(bool flag)
        {
            myGridControl1.Enabled = flag;
            simpleButton1.Enabled = simpleButton2.Enabled = simpleButton3.Enabled = simpleButton4.Enabled = flag;
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
                    SetState(true);
                    return;
                }

                DataRow dr = fileQueue.Dequeue();
                string FullPath = dr["FullPath"].ToString();
                zipPath = CZip.Compress(FullPath);

                labelControl1.Text = string.Format("正在上传： {0} / {1}", CurrentIndex + 1, dt.Rows.Count);
                wc.UploadFileAsync(new Uri("http://8.129.7.49:8014/KMSUpdate.ashx"), "POST", zipPath);
            }
            catch (Exception)
            {
            }
        }

        private void wc_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            progressBarControl1.Position = e.ProgressPercentage;
        }

        private void wc_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);
            Application.DoEvents();
            if (result.State == 1)
            {
                try
                {
                    DataRow dr = dt.Rows[CurrentIndex];
                    string path = dr["FullPath"].ToString();

                    FileInfo fi = new FileInfo(zipPath);

                    #region 基本信息监测
                    string _FileName = dr["FileName"].ToString();
                    string _FileNameZip = Path.GetFileName(zipPath);
                    string _AppPath = dr["AppPath"].ToString().Trim();
                    string _FileMD5 = GetFileMD5(path);
                    long _FileLen = fi.Length;

                    if (File.Exists(zipPath))
                    {
                        File.Delete(zipPath);
                    }
                    #endregion

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("FileName", _FileName));
                    list.Add(new SqlPara("FileNameZip", _FileNameZip));
                    list.Add(new SqlPara("AppPath", _AppPath));
                    list.Add(new SqlPara("FileMD5", _FileMD5));
                    list.Add(new SqlPara("FileLen", _FileLen));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_FileUpdateGX", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        dt.Rows[CurrentIndex]["UpState"] = 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("上传记录保存失败：\r\n" + ex.Message, "错误提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            CurrentIndex++;
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