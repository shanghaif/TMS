using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Net;

namespace ZQTMS.UI
{
    public partial class w_oa_file : BaseForm
    {
        public w_oa_file()
        {
            InitializeComponent();
        }

        int pagecount = 20; //每页行数
        int page = 1; //当前页数
        int totalpage = 0;//总页数
        int rowcount = 0; //总行数
        private void b_oa_file_Load(object sender, EventArgs e)
        {
            //CommonClass.FormSet(this);
            bdate.EditValue = CommonClass.gbdate.AddDays(-15);
            edate.EditValue = CommonClass.gedate.Date.Add(new TimeSpan(23, 59, 59));
            backgroundWorker1.RunWorkerAsync();
            //设计冻结列    
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(gridView1, "OA文件管理1");
            GridOper.RestoreGridLayout(gridView2, "OA文件管理2");

            //barButtonItem4.Visibility = ur.GetUserRightDetail888("f801") ? BarItemVisibility.Always : BarItemVisibility.Never;//新增
            //barButtonItem6.Visibility = ur.GetUserRightDetail888("f802") ? BarItemVisibility.Always : BarItemVisibility.Never;//删除
            //barButtonItem7.Visibility = ur.GetUserRightDetail888("f803") ? BarItemVisibility.Always : BarItemVisibility.Never;//导出
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1, gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "OA文件管理1", gridView2, "OA文件管理2");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "OA文件管理1");
            GridOper.DeleteGridLayout(gridView2, "OA文件管理2");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_oa_file_add wo = new w_oa_file_add();
            wo.ShowDialog();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (XtraMessageBox.Show("确定删除吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                int id = Convert.ToInt32(gridView2.GetRowCellValue(rowhandle, "fileid"));
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("fileid", id));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_OA_FILE", list)) > 0)
                {
                    XtraMessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridView2.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("操作失败!\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2, this.Text);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
                cbRetrieve.Enabled = false;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("userid", CommonClass.UserInfo.UserAccount));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OA_FILE_Sum", list));
                e.Result = new object[] { 1, ds };
            }
            catch (Exception ex)
            {
                e.Result = new object[] { 0, ex.Message };
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] obj = e.Result as object[];
                if (Convert.ToInt32(obj[0]) == 0)
                {
                    throw new Exception(obj[1].ToString());
                }
                DataSet ds = obj[1] as DataSet;
                if (ds.Tables.Count == 0) return;

                #region
                DataRow[] drs = ds.Tables[0].Select("filetype like '%,%'");
                string[] arr;
                DataTable dt = ds.Tables[0].Clone();
                foreach (DataRow item in drs)
                {
                    dt.Rows.Add(item.ItemArray);
                    ds.Tables[0].Rows.Remove(item);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arr = dt.Rows[i]["filetype"].ToString().Split(',');
                    foreach (string s in arr)
                    {
                        DataRow[] rows = ds.Tables[0].Select(string.Format("filetype='{0}'", s.Trim()));
                        if (rows.Length > 0)
                        {
                            rows[0]["total"] = Convert.ToInt32(rows[0]["total"]) + Convert.ToInt32(dt.Rows[i]["total"]);
                        }
                        else
                        {
                            ds.Tables[0].Rows.Add(new object[] { s.Trim(), Convert.ToInt32(dt.Rows[i]["total"]) });
                        }
                    }
                }
                #endregion

                #region  通知通报类提前
                dt.Clear();
                DataRow[] ss = ds.Tables[0].Select("filetype not like '%通%'");
                foreach (DataRow item in ss)
                {
                    dt.Rows.Add(item.ItemArray);
                    ds.Tables[0].Rows.Remove(item);
                }

                foreach (DataRow item in dt.Rows)
                {
                    ds.Tables[0].Rows.Add(item.ItemArray);
                }
                #endregion

                gridView1.ClearColumnsFilter();
                gridView1.ClearSorting();
                gridControl1.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    backgroundWorker2.RunWorkerAsync(ds.Tables[0].Rows[0]["filetype"].ToString());

                    totalpage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(ds.Tables[0].Rows[0]["total"]) / (decimal)pagecount));
                    rowcount = Convert.ToInt32(ds.Tables[0].Rows[0]["total"]);

                    label2.Text = string.Format("当前第 {0} 页 共 {1} 页 {2}条记录", page, totalpage, rowcount);
                    comboBoxEdit1.Properties.Items.Clear();
                    for (int i = 1; i <= totalpage; i++)
                    {
                        comboBoxEdit1.Properties.Items.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
            finally
            {
                cbRetrieve.Enabled = true;
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            page = 1;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();

            page = 1;
            totalpage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(gridView1.GetRowCellValue(rowhandle, "total")) / (decimal)pagecount));
            rowcount = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "total"));

            label2.Text = string.Format("当前第 {0} 页 共 {1} 页 {2}条记录", page, totalpage, rowcount);

            if (!backgroundWorker2.IsBusy)
            {
                backgroundWorker2.RunWorkerAsync(filetype);
            }
            comboBoxEdit1.Properties.Items.Clear();
            for (int i = 1; i <= totalpage; i++)
            {
                comboBoxEdit1.Properties.Items.Add(i);
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string filetype = e.Argument as string;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("userid", CommonClass.UserInfo.UserAccount));
                list.Add(new SqlPara("filetype", filetype));
                list.Add(new SqlPara("pagecount", pagecount));
                list.Add(new SqlPara("page", page));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OA_FILE", list));


                e.Result = new object[] { 1, ds };
            }
            catch (Exception ex)
            {
                e.Result = new object[] { 0, ex.Message };
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                object[] obj = e.Result as object[];
                if (Convert.ToInt32(obj[0]) == 0)
                {
                    throw new Exception(obj[1].ToString());
                }
                DataSet ds = obj[1] as DataSet;
                if (ds.Tables.Count == 0) return;

                DataTable dt = ds.Tables[0].Clone();
                DataRow[] ss = ds.Tables[0].Select("filetype like '%通%'");
                foreach (DataRow item in ss)
                {
                    dt.Rows.Add(item.ItemArray);
                    ds.Tables[0].Rows.Remove(item);
                }

                dt.DefaultView.Sort = "billdate desc";
                DataTable table = dt.DefaultView.ToTable();

                foreach (DataRow item in table.Rows)
                {
                    ds.Tables[0].Rows.Add(item.ItemArray);
                }

                gridView2.ClearColumnsFilter();
                gridView2.ClearSorting();
                gridControl2.DataSource = ds.Tables[0];
                if (totalpage == 1)
                {
                    simpleButton1.Enabled = false;
                    simpleButton2.Enabled = false;
                    simpleButton3.Enabled = false;
                    simpleButton4.Enabled = false;
                }
                if (totalpage > 1 && page == 1)
                {
                    simpleButton1.Enabled = false;
                    simpleButton2.Enabled = false;
                    simpleButton3.Enabled = true;
                    simpleButton4.Enabled = true;
                }
                if (totalpage > 1 && page == totalpage)
                {
                    simpleButton1.Enabled = true;
                    simpleButton2.Enabled = true;
                    simpleButton3.Enabled = false;
                    simpleButton4.Enabled = false;
                }
                if (totalpage > 1 && page > 1 && page < totalpage)
                {
                    simpleButton1.Enabled = true;
                    simpleButton2.Enabled = true;
                    simpleButton3.Enabled = true;
                    simpleButton4.Enabled = true;
                }
                label2.Text = string.Format("当前第 {0} 页 共 {1} 页 {2}条记录", page, totalpage, rowcount);
                comboBoxEdit1.Text = page.ToString();


                DataRow[] dr = ds.Tables[0].Select("isread=0");
                if (dr.Length > 0)
                {
                    MsgBox.ShowOK("您有" + dr.Length + "条公文未查看，请及时查看！");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            e.Value = e.RowHandle + 1;
        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        #region
        [DllImport("dsoframer.ocx")]
        public static extern int DllRegisterServer();//注册时用

        [DllImport("dsoframer.ocx")]
        public static extern int DllUnregisterServer();
        #endregion

        private void RegOCX()
        {
            try
            {
                DllRegisterServer();
                //if (a >= 0)
                //{
                //    commonclass.MsgBox.ShowOK("注册成功");
                //}
                //else
                //{
                //    commonclass.MsgBox.ShowOK("注册失败");
                //}
                //Process Process1 = new Process();
                //Process1.StartInfo.FileName = "cmd.exe";
                //Process1.StartInfo.CreateNoWindow = true;
                //Process1.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //Process1.StartInfo.Arguments = string.Format(" RegSvr32.exe \"{0}\\dsoframer.ocx\"", Application.StartupPath);
                //Process1.Start();
                //Process1.WaitForExit(2000);
                //if (Process1.HasExited)
                //{
                //    int ExitCode = Process1.ExitCode;
                //    Process1.Close();
                //    if (ExitCode != 0)
                //    {
                //        Process1.Close();
                //        return;
                //    }
                //}
            }
            catch (Exception)
            { }
        }

        private void gridView2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = gridView2.CalcHitInfo(new Point(e.X, e.Y));
            if (hi.Column == null || hi.RowHandle == -1) return;
            if (hi.Column.ColumnEdit != null && hi.Column.FieldName == "read")
            {

                try
                {
                    int rowhandle = gridView2.FocusedRowHandle;

                    if (rowhandle < 0) return;
                    string filename = gridView2.GetRowCellValue(rowhandle, "filedata").ToString();
                    string bdPath = System.Windows.Forms.Application.StartupPath + "/TempFile";
                    if (!Directory.Exists(bdPath)) Directory.CreateDirectory(bdPath);
                    string bdFileName = bdPath + filename.Substring(filename.LastIndexOf("/"));

                    if (!File.Exists(bdFileName))
                    {
                        WebClient wc = new WebClient();
                        wc.DownloadFile(FileUpload.UpFileUrl + filename, bdFileName);
                    }
                    System.Diagnostics.Process.Start(bdFileName);

                    int id = Convert.ToInt32(gridView2.GetRowCellValue(rowhandle, "id"));
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("id", id));
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_OA_ReciverREAD", list)) > 0)
                    {
                        gridView2.SetRowCellValue(rowcount, "isread", 1);
                    }
                }
                catch (Exception ee)
                {
                    MsgBox.ShowOK("打开失败。您的系统中没有合适的程序打开该文件!\r\n" + ee.Message);

                }
            }
        }

        #region 页码
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            page = 1;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();
            backgroundWorker2.RunWorkerAsync(filetype);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (page == 1) return;
            page--;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();
            backgroundWorker2.RunWorkerAsync(filetype);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (page == totalpage) return;
            page++;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();
            backgroundWorker2.RunWorkerAsync(filetype);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            page = totalpage;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();
            backgroundWorker2.RunWorkerAsync(filetype);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            page = comboBoxEdit1.SelectedIndex + 1;
            string filetype = gridView1.GetRowCellValue(rowhandle, "filetype").ToString();
            backgroundWorker2.RunWorkerAsync(filetype);
        }

        #endregion

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //刷新
        {
            if (!backgroundWorker1.IsBusy) backgroundWorker1.RunWorkerAsync();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
