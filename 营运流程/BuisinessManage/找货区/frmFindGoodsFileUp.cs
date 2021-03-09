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
    public partial class frmFindGoodsFileUp : BaseForm
    {
        private DataTable dt = new DataTable(); //数据源
        WebClient wc = new WebClient();
        string id = "";
        int itotal = 0; //上传文件总数
        int ifail = 0; //上传失败次数

        /// <summary>
        /// 无头件的编号
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        //传入参数
        //public string fileExt = "*.jpg;*.jpeg;*.gif;*.png";
        public string UpType = ""; //是否需要添加到数据库，默认不需要
        public int fileCount = 10;  //上传文件数量

        //返回参数
        public string paths = "";//图片服务器路径集合，多个用@分割，带路径和新生成的Guid文件名
        public string paths_ZQTMS = "";//maohui201807017
        string[] dirpath = null; //获取临时文件目录路径

        public frmFindGoodsFileUp()
        {
            InitializeComponent();

            dt.Columns.Add("rowid", typeof(int));
            dt.Columns.Add("filename", typeof(string));
            dt.Columns.Add("fileSize", typeof(string));

            DataColumn dc = new DataColumn("path", typeof(string));
            dc.Unique = true;//路径不能重复
            dt.Columns.Add(dc);

            dt.Columns.Add("state", typeof(string));

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            #region 基本信息检测
            gridView1.ClearColumnsFilter();
            if (string.IsNullOrEmpty(id))
            {
                if (MsgBox.ShowYesNo("没有编号信息，请重新打开本界面,现在关闭？") == DialogResult.Yes) this.Close();
                return;
            }
            if (gridView1.RowCount == 0)
            {
                MsgBox.ShowOK("没有需要上传的文件!");
                return;
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
                FileInfo fi;
                foreach (string path in dirpath)
                {
                    DataRow dr = dt.NewRow();
                    fi = new FileInfo(path);
                    dr["rowid"] = rowid++;
                    dr["fileSize"] = Math.Round(((double)fi.Length) / 1024, 2);
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
        public decimal Algorithm(decimal size)
        {
            string s = (size / 1024).ToString();
            return decimal.Parse(s.Substring(0, s.IndexOf('.') + 3));
        }
        private void UpLoadFiles()
        {
            DataRow[] drs = dt.Select("state<>'上传成功'");
            string filename = "", path = "", th_path = "", th_paths = "", filename_ZQTMS = "", path_ZQTMS = "", th_path_ZQTMS = "", th_paths_ZQTMS = "";  //maohui20180717
            int rowid = 1;
            paths = "";
            decimal fileSize = 0;
            foreach (DataRow dr in drs)
            {
                filename = ConvertType.ToString(dr["filename"]);
                path = ConvertType.ToString(dr["path"]);
                string pathNew = string.Format("/Files/{0}/{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path));
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("");
                        listLog.Items.Add("正在上传 第" + rowid + "行...");
                        lblstate.Text = "正在上传  " + filename + "       " + progressBarControl1.Text + "/" + itotal;
                    }));

                    fileSize = ConvertType.ToDecimal(dr["fileSize"]);
                    decimal size = Algorithm(fileSize);
                    if (size > 2)
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            listLog.Items.Add("    " + rowid + "上传失败：超过2M!");
                            dr["state"] = "上传失败";
                            ifail++;//上传失败计数
                            rowid++;//下一行
                        });
                        continue;
                    }

                    byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrl)), "POST", path);
                    string json = Encoding.UTF8.GetString(bt);
                    UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);

                    this.Invoke((EventHandler)(delegate
                    {
                        if (result.State == 1)
                        {
                            paths += pathNew + "@";
                            listLog.Items.Add("    " + rowid + "上传完毕 ...");
                            dr["state"] = "上传成功";
                        }
                        else
                        {
                            listLog.Items.Add("    " + rowid + "上传失败：" + result.Error);
                            dr["state"] = "上传失败";
                            ifail++;//上传失败计数
                        }
                        listLog.SetSelected(listLog.Items.Count - 1, true);
                        progressBarControl1.PerformStep();
                    }));

                    //生成并上传缩略图,大于50KB的才生成
                    if (fileSize > 50)
                    {
                        th_path = GenerateThumbnail(path);
                        //生成缩略图失败
                        if (th_path == "")
                        {
                            rowid++;//下一行
                            continue;
                        }

                        this.Invoke((EventHandler)(delegate
                        {
                            listLog.Items.Add("");
                            listLog.Items.Add("正在上传 第" + rowid + "行的缩略图...");
                        }));

                        //缩略图新路径
                        pathNew = string.Format("/Files/{0}/{1}.jpg", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString());
                        //上传
                        bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrl)), "POST", th_path);
                        json = Encoding.UTF8.GetString(bt);
                        result = JsonConvert.DeserializeObject<UploadResult>(json);

                        this.Invoke((EventHandler)(delegate
                        {
                            if (result.State == 1)
                            {
                                th_paths += pathNew + "@";
                                listLog.Items.Add("    " + rowid + " 行缩略图上传完毕 ...");
                            }
                            else
                            {
                                listLog.Items.Add("    " + rowid + " 行缩略图上传失败：" + result.Error);
                            }

                            listLog.SetSelected(listLog.Items.Count - 1, true);
                        }));
                    }
                    else
                    {
                        th_paths += "@";
                    }
                    rowid++;
                }
                catch (Exception ex)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("    " + rowid + "上传失败：" + ex.Message);
                        dr["state"] = "上传失败";
                        ifail++;//上传失败计数
                    }));
                }

                Thread.Sleep(10);
            }

            foreach (DataRow dr in drs)  //将图片上传到ZQTMS服务器上  maohui20180623
            {
                filename_ZQTMS = ConvertType.ToString(dr["filename"]);
                path_ZQTMS = ConvertType.ToString(dr["path"]);
                string pathNew = string.Format("/Files/{0}/{1}{2}", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString(), Path.GetExtension(path_ZQTMS));
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("");
                        listLog.Items.Add("正在上传 第" + rowid + "行...");
                        lblstate.Text = "正在上传  " + filename + "       " + progressBarControl1.Text + "/" + itotal;
                    }));

                    fileSize = ConvertType.ToDecimal(dr["fileSize"]);
                    decimal size = Algorithm(fileSize);
                    if (size > 2)
                    {
                        this.Invoke((EventHandler)delegate
                        {
                            listLog.Items.Add("    " + rowid + "上传失败：超过2M!");
                            dr["state"] = "上传失败";
                            ifail++;//上传失败计数
                            rowid++;//下一行
                        });
                        continue;
                    }

                    byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrlZQTMS)), "POST", path_ZQTMS);
                    string json = Encoding.UTF8.GetString(bt);
                    UploadResult result = JsonConvert.DeserializeObject<UploadResult>(json);

                    this.Invoke((EventHandler)(delegate
                    {
                        if (result.State == 1)
                        {
                            paths_ZQTMS += pathNew + "@";
                            listLog.Items.Add("    " + rowid + "上传完毕 ...");
                            dr["state"] = "上传成功";
                        }
                        else
                        {
                            listLog.Items.Add("    " + rowid + "上传失败：" + result.Error);
                            dr["state"] = "上传失败";
                            ifail++;//上传失败计数
                        }

                        listLog.SetSelected(listLog.Items.Count - 1, true);
                        progressBarControl1.PerformStep();
                    }));

                    //生成并上传缩略图,大于50KB的才生成
                    if (fileSize > 50)
                    {
                        th_path_ZQTMS = GenerateThumbnail(path);
                        //生成缩略图失败
                        if (th_path_ZQTMS == "")
                        {
                            rowid++;//下一行
                            continue;
                        }

                        this.Invoke((EventHandler)(delegate
                        {
                            listLog.Items.Add("");
                            listLog.Items.Add("正在上传 第" + rowid + "行的缩略图...");
                        }));

                        //缩略图新路径
                        pathNew = string.Format("/Files/{0}/{1}.jpg", DateTime.Now.ToString("yyyy-MM-dd"), Guid.NewGuid().ToString());
                        //上传
                        bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrlZQTMS)), "POST", th_path_ZQTMS);
                        json = Encoding.UTF8.GetString(bt);
                        result = JsonConvert.DeserializeObject<UploadResult>(json);

                        this.Invoke((EventHandler)(delegate
                        {
                            if (result.State == 1)
                            {
                                th_paths_ZQTMS += pathNew + "@";
                                listLog.Items.Add("    " + rowid + " 行缩略图上传完毕 ...");
                            }
                            else
                            {
                                listLog.Items.Add("    " + rowid + " 行缩略图上传失败：" + result.Error);
                            }

                            listLog.SetSelected(listLog.Items.Count - 1, true);
                        }));
                    }
                    else
                    {
                        th_paths_ZQTMS += "@";
                    }
                    rowid++;
                }
                catch (Exception ex)
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("    " + rowid + "上传失败：" + ex.Message);
                        dr["state"] = "上传失败";
                        ifail++;//上传失败计数
                    }));
                }

                Thread.Sleep(10);
            }

            this.Invoke((EventHandler)(delegate
            {
                if (paths == "")
                {
                    MsgBox.ShowOK("上传失败,请重试或稍后再试!");
                    return;
                }                
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_FINDGOODS_IMAGE", new List<SqlPara> { new SqlPara("Id", id), new SqlPara("PathStr", paths), new SqlPara("PathThStr", th_paths) })) == 0)
                    {
                        MsgBox.ShowOK("上传失败,请重试或稍后再试!");
                        return;
                    }
                    if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_ADD_FINDGOODS_IMAGE", new List<SqlPara> { new SqlPara("Id", id), new SqlPara("PathStr", paths_ZQTMS), new SqlPara("PathThStr", th_paths_ZQTMS) })) == 0)  //maohui20180623
                    {
                        MsgBox.ShowOK("上传失败,请重试或稍后再试!");
                        return;
                    }

                    lblstate.Text = "上传完成       " + progressBarControl1.Text + "/" + itotal;
                Clear();
                if (MsgBox.ShowYesNo(string.Format("上传完毕!\r\n共上传{0}个文件，上传成功：{1}个；上传失败：{2}个\r\n\r\n是否关闭本窗口？", itotal, itotal - ifail, ifail)) != DialogResult.Yes) return;
                this.Close();
            }));
        }

        /// <summary>
        /// 生成缩略图
        /// <param name="src">原文件</param>
        /// </summary>
        private string GenerateThumbnail(string src)
        {
            try
            {
                String dest = Application.StartupPath + "\\TempFile\\" + Guid.NewGuid() + "_th.jpg";//生成的缩略图图像文件的绝对路径
                int thumbWidth = 132;//要生成的缩略图的宽度
                System.Drawing.Image image = System.Drawing.Image.FromFile(src); //利用Image对象装载源图像
                //接着创建一个System.Drawing.Bitmap对象，并设置你希望的缩略图的宽度和高度。
                int srcWidth = image.Width;
                int srcHeight = image.Height;
                int thumbHeight = (int)((srcHeight * 1.0 / srcWidth) * thumbWidth);
                Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);
                //从Bitmap创建一个System.Drawing.Graphics对象，用来绘制高质量的缩小图。
                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                //设置 System.Drawing.Graphics对象的SmoothingMode属性为HighQuality
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //下面这个也设成高质量
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                //下面这个设成High
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                //把原始图像绘制成上面所设置宽高的缩小图
                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
                gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);
                //保存图像，大功告成！
                bmp.Save(dest);
                //最后别忘了释放资源
                bmp.Dispose();
                image.Dispose();
                //返回缩略图路径
                return dest;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                return "";
            }
        }

        private void Clear()
        {
            DataRow[] drs = dt.Select("state='上传成功'");
            for (int i = drs.Length - 1; i >= 0; i--)
            {
                dt.Rows.Remove(drs[i]);
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
            string filepath = GridOper.GetRowCellValueString(gridView1, rowhandle, "path");
            if (MsgBox.ShowYesNo("确定删除选中项吗?") != DialogResult.Yes) return;

            gridView1.DeleteRow(rowhandle);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "请选择图片文件(*.jpg;*.jpeg)|*.jpg;*.jpeg";
            op.Multiselect = true;
            if (op.ShowDialog() == DialogResult.OK)
            {
                dirpath = op.FileNames;
                GetFiles();
            }
        }

        private void frmFindGoodsFileUp_Load(object sender, EventArgs e)
        {
            wc.Proxy = null;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowLocalImg(gridView1, "path", true);// 打开本地文件,绝对路径
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