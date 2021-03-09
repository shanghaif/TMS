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
    public partial class fmFileUploadClone : BaseForm
    {

        private DataTable dt = new DataTable(); //数据源
        string servicesTime = "";
        DataSet ds77 = new DataSet();

        int itotal = 0; //上传文件总数
        int ifail = 0; //上传失败次数 
        public int mustUploadFiles = 0;
        WebClient wc = new WebClient();

        //传入参数
        public string fileExt = ".jpg,.jpeg,.gif,.png";
        public string UpType = "upadd"; //是否需要添加到数据库，默认不需要
        public string UserName = CommonClass.UserInfo.UserName;  //用户名
        public string billNo = "";  //运单号

        //返回参数
        public string paths = "";//图片服务器路径集合，多个用@分割，带路径和新生成的Guid文件名
        public string billNos = ""; //运单号集合，多个用@分割
        string dirpath = Application.StartupPath + "\\TempFiles"; //获取临时文件目录路径

        public fmFileUploadClone()
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
      
            SqlParasEntity sp = new SqlParasEntity(OperType.Query, "QSP_GETSERVICESTIME");
            DataTable dt1 = SqlHelper.GetDataTable(sp);
            servicesTime = dt1.Rows[0]["ServicesTime"].ToString();

            string billnos = "";
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                billnos += gridView1.GetRowCellValue(i, "billno")+"@";

            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billnos", billnos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_get_huidanqueren", list);
                ds77 = SqlHelper.GetDataSet(spe);
                for (int j = 0; j < ds77.Tables[0].Rows.Count; j++)
                {
                    string huidanqueren = ds77.Tables[0].Rows[j]["huidanqueren"].ToString();
                    string BillNo = ds77.Tables[0].Rows[j]["BillNo"].ToString();
                    string IsReceiptFee = ds77.Tables[0].Rows[j]["IsReceiptFee"].ToString();
                    string ReceiptCondition = ds77.Tables[0].Rows[j]["ReceiptCondition"].ToString();
                    //if (ReceiptCondition != "签回单")
                    //{
                    //    MsgBox.ShowOK(BillNo + "没有开签回单，不允许上传回单图片，请去签单上传上传图片！");
                    //    return;
                    //}
                    //HJ20181009
                    if (ReceiptCondition == "" || ReceiptCondition == "附清单")
                    {
                        MsgBox.ShowOK(BillNo + "没有开签回单，不允许上传回单图片，请去签单上传上传图片！");
                        return;
                    }
                    if (huidanqueren == "已审核")
                    {
                        MsgBox.ShowOK("上传的单中"+BillNo+"已审核，请检查！");
                        return;
                    }
                    if (huidanqueren == "未审核")
                    {
                            //string BillNo2 = ds77.Tables[0].Rows[j]["BillNo"].ToString();
                            //string BillNo3 = "";
                            //int b = 0;
                            //for (int k = 0; k < gridView1.RowCount; k++)
                            //{
                            //    BillNo3 = gridView1.GetRowCellValue(k, "billno").ToString();
                            //    if (BillNo2 == BillNo3)
                            //    {
                            //        b++;
                            //    }
                            //}
                            //if ((b) > 5)
                            //{
                            //    MsgBox.ShowOK("已取消确定的单最多只能上传5张图片！");
                            //    return;
                            //}
                            //else
                            //{
                                List<SqlPara> list6 = new List<SqlPara>();
                                list6.Add(new SqlPara("BillNo", BillNo));
                                SqlParasEntity spe6 = new SqlParasEntity(OperType.Execute, "USP_delete_TBFILEINFO2", list6);
                                if (SqlHelper.ExecteNonQuery(spe6) > 0)
                                {
                                    //MsgBox.ShowOK();
                                }
                            //}
                     
                    }
                    //if (huidanqueren == "")
                    //{
                    //    try
                    //    {
                    //        List<SqlPara> list2 = new List<SqlPara>();
                    //        list2.Add(new SqlPara("BillNo", BillNo));
                    //        SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "USP_get_TBFILEINFO2", list2);
                    //        DataSet ds = SqlHelper.GetDataSet(spe2);
                    //        int a = ds.Tables[0].Rows.Count;
                    //        string BillNo2 = ds77.Tables[0].Rows[j]["BillNo"].ToString();
                    //        string BillNo3 = "";
                    //        int b = 0;
                    //        for (int k = 0; k < gridView1.RowCount; k++)
                    //        {
                    //            BillNo3 = gridView1.GetRowCellValue(k, "billno").ToString();
                    //            if (BillNo2 == BillNo3)
                    //            {
                    //                b++;
                    //            }
                    //        }
                    //        if ((a+b) > 5)
                    //        {
                    //            MsgBox.ShowOK("最多只能上传5张图片！");
                    //            return;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        MsgBox.ShowException(ex);
                    //    }
                    //}
                    if (huidanqueren == "否决")
                    {
                        //string BillNo2 = ds77.Tables[0].Rows[j]["BillNo"].ToString();
                        //string BillNo3 = "";
                        //int b = 0;
                        //for (int k = 0; k < gridView1.RowCount; k++)
                        //{
                        //    BillNo3 = gridView1.GetRowCellValue(k, "billno").ToString();
                        //    if (BillNo2 == BillNo3)
                        //    {
                        //        b++;
                        //    }
                        //}
                        //if ((b) > 5)
                        //{
                        //    MsgBox.ShowOK("已取消确定的单最多只能上传5张图片！");
                        //    return;
                        //}
                        //else
                        //{
                            List<SqlPara> list6 = new List<SqlPara>();
                            list6.Add(new SqlPara("BillNo", BillNo));
                            SqlParasEntity spe6 = new SqlParasEntity(OperType.Execute, "USP_delete_TBFILEINFO3", list6);
                            if (SqlHelper.ExecteNonQuery(spe6) > 0)
                            {
                                //MsgBox.ShowOK();
                            }
                        //}
                     

                    }

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


                gridView1.ClearColumnsFilter();
            if (gridView1.RowCount <= 0)
            {
                MsgBox.ShowOK("没有需要上传的文件!");
                return;
            }
            billNos="";
            paths = "";

            if (gridView1.RowCount > mustUploadFiles && mustUploadFiles != 0)
            {
                MsgBox.ShowOK("最多上传" + mustUploadFiles + "张图片!");
                return;
            }

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                string u = gridView1.GetRowCellValue(i, "billno") == DBNull.Value ? "" : gridView1.GetRowCellValue(i, "billno").ToString().Trim();
                if (string.IsNullOrEmpty(u))
                {
                    MsgBox.ShowOK("有些图片没有填写对应的运单号，请检查!");
                    gridView1.FocusedRowHandle = i;
                    return;
                }
                if (u.Length < 5)
                {
                    MsgBox.ShowOK("填写的单号不正确，请检查!");
                    gridView1.FocusedRowHandle = i;
                    return;
                }
                if (!StringHelper.IsNumberId(u))
                {
                    MsgBox.ShowOK("填写的单号必须为数字，请检查!");
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
                ////回单签收信息接收接口
                btn_Click(sender, e);
            }
            catch (Exception ex)
            {
                th.Abort();
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        //回单签收信息接收接口
        private void huidanqianshou()
        {
            //http://8.129.7.49:8014/Files/2021-02-05/0f8bbc7c-61fc-4f61-974e-9866a8dfcc69.jpg
                #region //回单签收信息接收接口
                {
                    int state = 0;
                    for (int i = 0; i < gridView1.RowCount; i++)
                    {
                        string strBillNo = gridView1.GetRowCellValue(i, "billno").ToString();
                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("BillNo", strBillNo));
                        SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo3", lists);
                        DataSet dds = SqlHelper.GetDataSet(spsa);
                        DataRow ddr = dds.Tables[0].Rows[0];

                        if (ddr["BegWeb"].ToString() == "三方")
                        {
                            Dictionary<string, object> hashMap1 = new Dictionary<string, object>();
                            hashMap1.Add("carriageSns", strBillNo);
                            hashMap1.Add("remark", "");
                            hashMap1.Add("fileName", "http://8.129.7.49:8014" + ddr["FilePath"]);
                            List<SqlPara> list = new List<SqlPara>();
                            list.Add(new SqlPara("BillNo", strBillNo));
                            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_BILLPETTYPAY_BYBILLNO", list);
                            if (SqlHelper.ExecteNonQuery(sps) > 0)
                            {
                                state = 1;
                            }

                            hashMap1.Add("receiptFlag", state);
                            string json1 = JsonConvert.SerializeObject(hashMap1);
                            string url1 = "http://120.76.141.227:9882/umsv2.biz/open/receipt_bill/trunk_receipt_bill";
                            try
                            {
                                MessageBox.Show("推送成功1");
                                HttpHelper.HttpPostJava(json1, url1);
                                MessageBox.Show("推送成功2");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                }
                #endregion
           
        }

        private void btn_Click(object sender, EventArgs e)
        {
            
            huidanqianshou();
          
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
            if (dt.Rows.Count > 0) dt.Clear();
            if (Directory.Exists(dirpath))
            {
                string[] files = Directory.GetFiles(dirpath, "*.*", SearchOption.AllDirectories);
                foreach (string path in files)
                {
                    string format = Path.GetExtension(path).ToLower();
                    if (fileExt.Contains(format))
                    {
                        DataRow dr = dt.NewRow();
                        if (!string.IsNullOrEmpty(billNo))
                        {
                            dr["billno"] = billNo;
                        }
                        else
                        {
                            dr["billno"] = Path.GetFileNameWithoutExtension(path);
                        }
                        dr["rowid"] = rowid++;
                        dr["filename"] = Path.GetFileName(path);
                        dr["path"] = path;
                        dr["state"] = "待上传";
                        dt.Rows.Add(dr);
                    }
                }
            }

            gridControl1.DataSource = dt;

            progressBarControl1.EditValue = 0;
            lblstate.Text = "";
        }

        private void SaveLog(string msg) //name：环节  xml：客户端提交过来的xml    dbname：哪家的
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, DateTime.Now.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, Guid.NewGuid().ToString() + ".txt");
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.Default))
                {
                    sw.WriteLine(msg);
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        private void UpLoadFiles()
        {
            string sumbillno = "";
            string sumState = "";
            string sumNewName = "";
            string sumoldName = "";

            //具体提示哪些运单哪些图片没有上传成功
            string msg = "";
            DataRow[] drs = dt.Select("state<>'上传成功'");
            foreach (DataRow dr in drs)
            {
                string filename = dr["filename"].ToString();
                string billno = dr["billno"].ToString();
                string path = dr["path"].ToString();
                string pathNew = string.Format("/Files/{0}/{1}{2}", servicesTime, Guid.NewGuid().ToString(), Path.GetExtension(path));
                try
                {
                    this.Invoke((EventHandler)(delegate
                    {
                        listLog.Items.Add("");
                        listLog.Items.Add("正在上传 单号为" + billno + "...");
                        lblstate.Text = "正在上传  " + filename + "       " + progressBarControl1.Text + "/" + itotal;
                    }));

                    string newFileName = "";
                    if (!PicDeal.SendSmallImage(path, ref newFileName, 1360, 1024))
                    {
                        continue;
                    }

                    byte[] bt = wc.UploadFile(new Uri(string.Format("{1}/FileUpLoad.ashx?Path={0}", pathNew, FileUpload.UpFileUrl)), "POST", newFileName);
                    string json = Encoding.UTF8.GetString(bt);
                    File.Delete(newFileName);
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

                            //2017-04-14 上传失败提示具体失败单号和图片
                            sumbillno = sumbillno + billno + "@";
                            sumState = sumState + "0" + "@";
                            sumNewName = sumNewName + pathNew + "@";
                            sumoldName = sumoldName + filename + "@";

                            msg = msg + "运单【" + billno + "】图片【" + filename + "】上传失败" + "\r\n";
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
                        msg += "运单【" + billno + "】图片【" + filename + "】上传失败" + "\r\n";
                    }));

                }

                Thread.Sleep(10);
            }

            if (billNos == "")
            {
                //记录失败日志
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", sumbillno));
                    list.Add(new SqlPara("ReceiptType", 0));
                    list.Add(new SqlPara("UpState", sumState));
                    list.Add(new SqlPara("sumNewName", sumNewName));
                    list.Add(new SqlPara("sumoldName", sumoldName));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ReceiptUpInfo", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowError(string.Format("上传完毕!\r\n共上传{0}个文件，上传成功：{1}个；上传失败：{2}个；\r\n失败详情：{3}", itotal, itotal - ifail, ifail, msg));
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                return;
            }
            this.Invoke((EventHandler)(delegate
            {
                if (UpType.Contains("upadd"))
                {
                    //需要添加到数据库
                    FileUpload.AddUpLoadMoreImg(7, billNos, paths, UserName);
                }

                lblstate.Text = "上传完成       " + progressBarControl1.Text + "/" + itotal;
                if (MsgBox.ShowYesNo(string.Format("上传完毕!\r\n共上传{0}个文件，上传成功：{1}个；上传失败：{2}个；\r\n失败详情：{3}", itotal, itotal - ifail, ifail, msg)) == DialogResult.Yes)
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
                dt.Rows.RemoveAt(i);
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
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            dialog.SelectedPath = dirpath;
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            dirpath = dialog.SelectedPath;

            GetFiles();
        }

        private void fmFileUploadClone_Load(object sender, EventArgs e)
        {
            this.Text += string.Format("【{0}】", CommonClass.UserInfo.UserName);
            wc.Proxy = null;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowLocalImg(gridView1);
        }


        /// <summary>
        /// 上传结果
        /// </summary>
        [Serializable]
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