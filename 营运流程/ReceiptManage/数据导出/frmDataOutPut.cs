using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;
using System.Threading;
using System.Data.SqlClient;
using System.IO;
using KMS.Tool;
using KMS.SqlDAL;
using KMS.Common;
using DevExpress.XtraGrid.Columns;
using System.Net;

namespace KMS.UI
{
    public partial class frmDataOutPut : BaseForm
    {
       
        public frmDataOutPut()
        {
            InitializeComponent();
        }

        public frmDataOutPut(string atype)
        {
            InitializeComponent();
         
        }

        private void frmApplyList_Load(object sender, EventArgs e)
        {
          

            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            bdate.DateTime = CommonClass.gbdate.AddDays(-1);
            edate.DateTime = CommonClass.gedate;

            
        }

     

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
           getdate();
          
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//
        {
            FrmDataApplyMain dam = new FrmDataApplyMain();
            dam.ShowDialog();
        }

        public class word
        {
            public string name;
            public string path;
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//下载 
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (string.IsNullOrEmpty(dr["Annex"].ToString()))
            {
                MsgBox.ShowOK("数据为零,请调整筛选条件重新申请!");
                return;
            }
            if (dr["applyState"].ToString() != "已完成")
            {
                MsgBox.ShowOK("状态为未完成,不能下载!");
                return;
            }

            //string file = "/file/" + dr["Annex"].ToString(); //测试
            string file = "http://120.78.229.221:8091/DateExport/" + dr["Annex"].ToString();  //正式
            //string url = HttpHelper.domaimNew+file;
           string url =file;
            FolderBrowserDialog sfd = new FolderBrowserDialog();
            sfd.Description = "另存为";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            try
            {
                List<word> list = new List<word>();
                word wd = new word();
                wd.name = "受理开单";
                wd.path = url;
                list.Add(wd);
                Download(list);
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        public static void Download(List<word> list)
        {
            try
            {

                WebClient client = new WebClient();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录 
               // saveFileDialog.Filter = "Microsoft Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx";//定义文件格式  
               saveFileDialog.Filter = "winRAR(*.rar;*.zip)|*.rar;*.zip";//定义文件格式  
                //点了保存按钮进入  
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        //saveFileDialog.FileName = "matternam"+i;
                        string URL = list[i].path;
                        string localFilePath = System.IO.Path.GetFullPath(saveFileDialog.FileName);//获得文件路径，含文件名  
                        string fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);//获取文件名，不带路径  
                        string FilePath = localFilePath.Substring(0, localFilePath.LastIndexOf("\\"));//获取文件路径，不带文件名  
                        //string newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt; //给文件名前加上时间  
                        //saveFileDialog.FileName.Insert(1, "dameng");//在文件名里加字符  
                        //localFilePath = FilePath + "\\" + list[i].name + ".rar";
                        localFilePath = FilePath + "\\" + fileNameExt;
                        client.DownloadFile(URL, localFilePath);
                    }
                    MessageBox.Show("已下载完成！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("文件下载异常：\r\n" + ex.Message);
            }
        } 
          

           
          
     
    

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//执行
        {
            
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//取消
        {
          
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//否决
        {
          
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "改单记录");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem5_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);
            if (MsgBox.ShowYesNo("是否确认删除？" ) != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id", dr["exportApplyID"].ToString()));
            try
            {
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_Del_ExportDataApply", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK("操作成功!");
                    getdate();
                    return;
                }
                else
                {
                    MsgBox.ShowOK("操作失败!");
                    getdate();
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void getdate()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExportDataApply", list);
            myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
        }

    }
}