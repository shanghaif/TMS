using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Data.SqlClient;
using System.Threading;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;
using Newtonsoft.Json;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class fmFileUploadS : BaseForm
    {
        private DataTable dt = new DataTable(); //数据源
        WebClient wc = new WebClient();
        int itotal = 0; //上传文件总数
        int ifail = 0; //上传失败次数
        public bool ishowdel = true;

        //传入参数
        //public string fileExt = "*.jpg;*.jpeg;*.gif;*.png";
        public string UpType = ""; //是否需要添加到数据库，默认不需要
        public string UserName = "";  //用户名
        public string billNo = "";  //运单号
        public int fileCount = 4;  //上传文件数量

        //返回参数
        public string paths = "";//图片服务器路径集合，多个用@分割，带路径和新生成的Guid文件名
        public string billNos = ""; //运单号集合，多个用@分割
        string[] dirpath = null; //获取临时文件目录路径

        public fmFileUploadS()
        {
            InitializeComponent();

            dt.Columns.Add("rowid", typeof(string));
            dt.Columns.Add("billno", typeof(string));
            dt.Columns.Add("filename", typeof(string));
            dt.Columns.Add("path", typeof(string));
            dt.Columns.Add("state", typeof(string));
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //清空，但不删除文件
            Clear(false);
        }

        private void btnDelFiles_Click(object sender, EventArgs e)
        {
            //清空，并删除文件
            string msg = "此操作将清空上传列表，并删除以下目录中的所有文件：\r\n" + dirpath + "\r\n是否继续？";
            if (MsgBox.ShowYesNo(msg) != DialogResult.Yes) return;
            Clear(true);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            #region 基本信息检测
            gridView1.ClearColumnsFilter();
            if (gridView1.RowCount <= 0)
            {
                MsgBox.ShowOK("没有需要上传的文件!");
                return;
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string u = gridView1.GetRowCellValue(i, "billno").ToString();
                if (string.IsNullOrEmpty(u))
                {
                    MsgBox.ShowOK("有些图片没有填写对应的运单号，请检查!");
                    gridView1.FocusedRowHandle = i;
                    return;
                }
            }
            #endregion
            Thread th = new Thread(new ThreadStart(UpLoadFiles));
            th.IsBackground = true;

            try
            {
                #region 配置进度条
                progressBarControl1.EditValue = 1;
                progressBarControl1.Properties.Maximum = dt.Rows.Count;
                progressBarControl1.Properties.Minimum = 1;
                progressBarControl1.Properties.Step = 1;
                #endregion //配置进度条

                listLog.Items.Add("----------------------------------------------------------------------------");
                listLog.Items.Add("");
                listLog.Items.Add("上传时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm") + " ...");

                itotal = dt.Rows.Count;
                ifail = 0;

                th.Start();
                if (th.ThreadState == ThreadState.Stopped)
                {
                    th.Abort();
                }
            }
            catch (Exception ex)
            {
                th.Abort();
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            GetFiles();
        }

        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="directory">文件夹</param>
        private void GetFiles()
        {
            int rowid = 1; //序号
            if (dirpath == null) return;
            if (dt.Rows.Count + dirpath.Length > fileCount)
            {
                MsgBox.ShowError("最多只能上传" + fileCount.ToString() + "张图片");
                return;
            }
            if (dirpath != null)
            {
                foreach (string path in dirpath)
                {
                    DataRow dr = dt.NewRow();
                    if (!string.IsNullOrEmpty(billNo))
                    {
                        dr["billno"] = billNo;
                    }
                    else
                    {
                        dr["billno"] = "";// Path.GetFileNameWithoutExtension(path);
                    }
                    dr["rowid"] = rowid++;
                    dr["filename"] = Path.GetFileName(path);
                    dr["path"] = path;
                    dr["state"] = "待上传";
                    dt.Rows.Add(dr);
                }
            }

            gridControl1.DataSource = dt;

            progressBarControl1.EditValue = 0;
            lblstate.Text = "";
        }

        private void UpLoadFiles()
        {
            DataRow[] drs = dt.Select("state<>'上传成功'");
            foreach (DataRow dr in drs)
            {
                string filename = dr["filename"].ToString();
                string billno = dr["billno"].ToString();
                string path = dr["path"].ToString();
                string pathNew = string.Format("/Files/{0}/{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path));
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("");
                        listLog.Items.Add("正在上传 单号为" + billno + "...");
                        lblstate.Text = "正在上传  " + filename + "       " + progressBarControl1.Text + "/" + itotal;
                    }));

                    byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrl)), "POST", path);
                    string json = Encoding.UTF8.GetString(bt);
                    UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);

                    this.Invoke((EventHandler)(delegate
                    {
                        if (result.State == 1)
                        {
                            billNos += billno + "@";
                            paths += pathNew + "@";
                            listLog.Items.Add("    " + billno + "上传完毕 ...");
                            dr["state"] = "上传成功";
                        }
                        else
                        {
                            listLog.Items.Add("    " + billno + "上传失败：" + result.Error);
                            dr["state"] = "上传失败";
                            ifail++;//上传失败计数
                        }

                        listLog.SetSelected(listLog.Items.Count - 1, true);
                        progressBarControl1.PerformStep();
                    }));
                }
                catch (Exception ex)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("    " + billno + "上传失败：" + ex.Message);
                        dr["state"] = "上传失败";
                        ifail++;//上传失败计数
                    }));

                }

                Thread.Sleep(10);
            }

            this.Invoke((EventHandler)(delegate
            {
                if (UpType.Contains("upadd"))
                {
                    //需要添加到数据库
                    FileUpload.AddUpLoadMoreImg(2, billNos, paths, UserName);
                }
                lblstate.Text = "上传完成       " + progressBarControl1.Text + "/" + itotal;
                if (MsgBox.ShowYesNo(string.Format("上传完毕!\r\n共上传{0}个文件，上传成功：{1}个；上传失败：{2}个\r\n\r\n是否删除已上传的文件？", itotal, itotal - ifail, ifail)) == DialogResult.Yes)
                {
                    Clear(true);
                }
            }));
        }

        private void Clear(bool DeleteFiles)
        {
            DataRow[] drs = dt.Select("state='上传成功'");
            for (int i = drs.Length - 1; i >= 0; i--)
            {
                string path = drs[i]["path"].ToString();
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            dt.AcceptChanges();
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            listLog.Items.Clear();
            listLog.Items.Add("上传记录...");
        }
        
        private void btnDelFile_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string filepath = gridView1.GetRowCellValue(rowhandle, "path").ToString();
            string billno = gridView1.GetRowCellValue(rowhandle, "billno").ToString();
            if (XtraMessageBox.Show("确定删除单号为[" + billno + "]图片文件吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (File.Exists(filepath))
                {
                    //File.Delete(filepath);
                    gridView1.DeleteRow(rowhandle);
                    dt.AcceptChanges();
                }
                else
                {
                    gridView1.DeleteRow(rowhandle);
                    dt.AcceptChanges();
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "请选择图片文件(*.jpg;*.jpeg;*.gif;*.png)|*.jpg;*.jpeg;*.gif;*.png";
            op.Multiselect = true;
            if (op.ShowDialog() == DialogResult.OK)
            {
                dirpath = op.FileNames;
            }
            GetFiles();
        }

        private void fmFileUploadM_Load(object sender, EventArgs e)
        {
            wc.Proxy = null;

            btnDelAllFiles.Visible = ishowdel;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowLocalImg(gridView1);
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