using System;
using System.Collections.Generic;
using System.Text;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using System.Net;
using System.IO;
using ZQTMS.Tool;

namespace ZQTMS.Common
{
    public static class FileUpload
    {
        public static string UpFileUrl = "http://8.129.7.49:8014";//lms图片服务器
        public static string UpFileUrlZQTMS = "http://ZQTMS.dekuncn.com:7020";//ZQTMS图片服务器
        public static string UpFileUrlTX = "http://8.129.7.49:8014"; // 同星图片服务器（暂时APP在用）
        /// <summary>
        /// 公共保存图片
        /// </summary>
        /// <param name="billType">类型 0回单上传 1异常登记</param>
        /// <param name="billNos"></param>
        /// <param name="filePaths"></param>
        public static void AddUpLoadMoreImg(int billType, string billNos, string filePaths, string userName)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BillType", billType));
                list.Add(new SqlPara("FilePaths", filePaths));
                list.Add(new SqlPara("UpFileMan", userName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_TBFILEINFO", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (billType == 1)
                    {
                        List<SqlPara> listZQTMS = new List<SqlPara>();  //maohui20180724
                        listZQTMS.Add(new SqlPara("BillNos", billNos));
                        listZQTMS.Add(new SqlPara("BillType", billType));
                        listZQTMS.Add(new SqlPara("FilePaths", filePaths));
                        listZQTMS.Add(new SqlPara("UpFileMan", userName));
                        SqlParasEntity spsZQTMS = new SqlParasEntity(OperType.Execute, "USP_ADD_TBFILEINFO", listZQTMS);
                        SqlHelper.ExecteNonQuery_ZQTMS(spsZQTMS);
                    }
                    MsgBox.ShowOK();
                    CommonSyn.ReceiptUploadSyn(billType, billNos, filePaths, userName);//zaj 回单上传同步
                    CommonSyn.TimeOtherUptSyn(billNos, "", "", "", "", "", CommonClass.UserInfo.WebName, "USP_ADD_TBFILEINFO", "");//同步其他修改时效 LD 2018-4-27
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        public static void ShowImg(GridView gridView)
        {
            try
            {
                int rowhandle = gridView.FocusedRowHandle;
                if (rowhandle < 0) return;
                string isSynData = gridView.GetRowCellValue(rowhandle, "IsSynData").ToString();//是否同步过来的数据 zaj 20108-6-2
                string filename = gridView.GetRowCellValue(rowhandle, "FilePath").ToString();
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    string loadUrl = isSynData == "1" ? FileUpload.UpFileUrlZQTMS + filename : FileUpload.UpFileUrl + filename;//是否同步过来的数据 zaj 20108-6-2
                    //wc.DownloadFile(UpFileUrl + filename, bdFileName);
                    wc.DownloadFile(loadUrl, bdFileName);

                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();

                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = bdFileName;
                //process.StartInfo.Verb = "Open";
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                //process.Start();
            }
            catch (Exception ee)
            {
                MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!\r\n" + ee.Message);
            }
        }

        public static void ShowImg(string filename)
        {
            try
            {
                string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                if (!File.Exists(bdFileName))
                {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(UpFileUrl + filename, bdFileName);
                }

                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();

                //System.Diagnostics.Process process = new System.Diagnostics.Process();
                //process.StartInfo.FileName = bdFileName;
                //process.StartInfo.Verb = "Open";
                //process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                //process.Start();
            }
            catch (Exception ee)
            {
                MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!\r\n" + ee.Message);
            }
        }

        public static void ShowLocalImg(string bdFileName)
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = bdFileName;
                process.StartInfo.Verb = "Open";
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.Start();
            }
            catch
            {
                frmShowPic frm = new frmShowPic();
                frm.imgPath = bdFileName;
                frm.Show();
            }
        }

        public static void ShowLocalImg(GridView gridView)
        {
            ShowLocalImg(gridView, "path");
        }

        public static void ShowLocalImg(GridView gridView, string fieldName)
        {
            ShowLocalImg(gridView, fieldName, false);
        }

        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="gridView">网格</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="isAbs">是否为绝对路径</param>
        public static void ShowLocalImg(GridView gridView, string fieldName, bool isAbs)
        {
            int rowhandle = gridView.FocusedRowHandle;
            if (rowhandle < 0) return;
            string filename = gridView.GetRowCellValue(rowhandle, fieldName).ToString();

            string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
            if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
            //如果是本地就直接用网格里的路径,否则就用临时文件夹里的图片
            string bdFileName = isAbs ? filename : (bdPath + filename.Substring(filename.LastIndexOf("/")));

            if (!File.Exists(bdFileName))
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(UpFileUrl + filename, bdFileName);
            }
            ShowLocalImg(bdFileName);
        }
    }
}
