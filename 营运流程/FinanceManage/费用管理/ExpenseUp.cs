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
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid.Columns;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class ExpenseUp : BaseForm
    {
        public ExpenseUp()
        {
            InitializeComponent();
        }
        static System.Windows.Forms.Timer mytimer = new System.Windows.Forms.Timer();
        public delegate void OnUpLoadEventHandler(int step);
        public event OnUpLoadEventHandler OnUpLoad;
        public static bool DataUploadOK = false;
        public delegate void MyInvoke(int rownum);
        DataSet ds = null;
        string cygscompanyid;

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InportExcel();
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                return;
            }
            try
            {
                DataTable dt = (DataTable)myGridControl1.DataSource;
                if (dt == null  && dt.Rows.Count <= 0)
                {
                    return;
                }
                //dt.Columns.Contains
               // dt.Columns["ApplyDate"].SetOrdinal(0);
                //dt.Columns["ApplyCause"].SetOrdinal(0);
                //dt.Columns["ApplyArea"].SetOrdinal(1);
                //dt.Columns["ApplyDept"].SetOrdinal(2);
                //dt.Columns["Registrant"].SetOrdinal(3);
                //dt.Columns["FeeProject"].SetOrdinal(4);
                //dt.Columns["FeeType"].SetOrdinal(5);
                //dt.Columns["BelongMonth"].SetOrdinal(6);
                //dt.Columns["Remark"].SetOrdinal(7);
                //dt.Columns["Money"].SetOrdinal(8);
                //dt.Columns["AssumeDept"].SetOrdinal(9);
                string patten = @"^[2][0][12][0-9](0[1-9]|1[0-2])$";//201001-202912
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    if (!Regex.IsMatch(dt.Rows[i]["BelongMonth"].ToString(), patten))
                    {
                        MsgBox.ShowOK("所属月份格式不正确!");
                        return;
                    }
                    if(dt.Rows[i]["FeeType"].ToString()!="招聘费"&&dt.Rows[i]["FeeType"].ToString()!="奖金及提成"&&dt.Rows[i]["FeeType"].ToString()!="车辆油料成本"&&dt.Rows[i]["FeeType"].ToString()!="税务管理成本"&&
                        dt.Rows[i]["FeeType"].ToString()!="其他自有车辆成本"&&dt.Rows[i]["FeeType"].ToString()!="业务费"&&dt.Rows[i]["FeeType"].ToString()!="其他费用"&&dt.Rows[i]["FeeType"].ToString()!="差旅费"&&
                        dt.Rows[i]["FeeType"].ToString()!="办公物料费"&&dt.Rows[i]["FeeType"].ToString()!="车辆修理成本"&&dt.Rows[i]["FeeType"].ToString()!="外请劳务"&&dt.Rows[i]["FeeType"].ToString()!="生活费"&&
                        dt.Rows[i]["FeeType"].ToString()!="培训费"&&dt.Rows[i]["FeeType"].ToString()!="社保"&&dt.Rows[i]["FeeType"].ToString()!="维修装修费"&&dt.Rows[i]["FeeType"].ToString()!="财务费用"&&
                        dt.Rows[i]["FeeType"].ToString()!="广告费"&&dt.Rows[i]["FeeType"].ToString()!="水电费"&&dt.Rows[i]["FeeType"].ToString()!="通讯费"&&dt.Rows[i]["FeeType"].ToString()!="福利费"&&
                        dt.Rows[i]["FeeType"].ToString()!="叉车费"&&dt.Rows[i]["FeeType"].ToString()!="房租费"&&dt.Rows[i]["FeeType"].ToString()!="会议费"&&dt.Rows[i]["FeeType"].ToString()!="路桥停车成本"&&
                        dt.Rows[i]["FeeType"].ToString()!="其他营业外支出"){

                            MsgBox.ShowOK("费用类型不正确!");
                            return;
                    }

                }

                try
                {
                   
                        DataTable dt0 = ((System.Data.DataView)(myGridView1.DataSource)).Table;
                        //dt.Columns.Remove("序号");
                        dt0.AcceptChanges();
                        DataTable newDt = new DataTable();
                        newDt.Columns.Add("ApplyCause", typeof(string));
                        newDt.Columns.Add("ApplyArea", typeof(string));
                        newDt.Columns.Add("ApplyDept", typeof(string));
                        newDt.Columns.Add("Registrant", typeof(string));
                        newDt.Columns.Add("FeeProject", typeof(string));
                        newDt.Columns.Add("FeeType", typeof(string));
                        newDt.Columns.Add("BelongMonth", typeof(string));
                        newDt.Columns.Add("Remark", typeof(string));
                        newDt.Columns.Add("Money", typeof(string));
                        newDt.Columns.Add("AssumeDept", typeof(string));
                  
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = newDt.NewRow();
                            // List<SqlPara> list = new List<SqlPara>();
                            dr["ApplyCause"] = GridOper.GetRowCellValueString(myGridView1, i, "ApplyCause");
                            dr["ApplyArea"] = GridOper.GetRowCellValueString(myGridView1, i, "ApplyArea");
                            dr["ApplyDept"] = GridOper.GetRowCellValueString(myGridView1, i, "ApplyDept");
                            dr["Registrant"] = GridOper.GetRowCellValueString(myGridView1, i, "Registrant");
                            dr["FeeProject"] = GridOper.GetRowCellValueString(myGridView1, i, "FeeProject");

                            dr["FeeType"] = GridOper.GetRowCellValueString(myGridView1, i, "FeeType");
                            dr["BelongMonth"] = GridOper.GetRowCellValueString(myGridView1, i, "BelongMonth");
                            dr["Remark"] = GridOper.GetRowCellValueString(myGridView1, i, "Remark");

                            dr["Money"] = GridOper.GetRowCellValueString(myGridView1, i, "Money");
                            dr["AssumeDept"] = GridOper.GetRowCellValueString(myGridView1, i, "AssumeDept");


                            newDt.Rows.Add(dr);

                        }
                        string msg = "";
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("Tb", newDt));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Expensereimbursement_Upload", list);
                        int k = SqlHelper.ExecteNonQuery(sps);
                        if (k > 0)
                        {

                            MsgBox.ShowOK("批量上传成功");
                            this.Close();
                        }
                    
                    // if (flag == 1)
                    // {
                    // MsgBox.ShowOK();
                    // this.Close();
                    //  }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            //DataTable newDt = new DataTable();
            //newDt.Columns.Add("Province", Type.GetType("System.String"));
            //newDt.Columns.Add("City", Type.GetType("System.String"));
            //newDt.Columns.Add("Area", Type.GetType("System.String"));
            //newDt.Columns.Add("Street", Type.GetType("System.String"));
            //newDt.Columns.Add("CenterKilometres", Type.GetType("System.Double"));
            //newDt.Columns.Add("MinWeight", Type.GetType("System.Double"));
            //newDt.Columns.Add("MaxWeight", Type.GetType("System.Double"));
            //newDt.Columns.Add("DeliveryFee", Type.GetType("System.Double"));
            //newDt.Columns.Add("TransferMode", Type.GetType("System.String"));
            //newDt.Columns.Add("Remark", Type.GetType("System.String"));
            //newDt.Columns.Add("bsitename", Type.GetType("System.String"));
            //newDt.Columns.Add("HeavyPrice", Type.GetType("System.Double"));
            //newDt.Columns.Add("LightPrice", Type.GetType("System.Double"));
            ////newDt.Columns.Add("sitename", Type.GetType("System.String"));
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    DataRow newRow;
            //    string[] colNameArrays;
            //    foreach (DataRow row in dt.Rows)
            //    {
            //        if (!string.IsNullOrEmpty(row[0].ToString()) && !string.IsNullOrEmpty(row[1].ToString()))
            //        {
            //            foreach (DataColumn dc in dt.Columns)
            //            {
            //                if (dc.ColumnName.IndexOf('-') > 0)
            //                {
            //                    newRow = newDt.NewRow();
            //                    colNameArrays = dc.ColumnName.Split('-');
            //                    newRow["Province"] = row["Province"];
            //                    newRow["City"] = row["City"];
            //                    newRow["Area"] = row["Area"];
            //                    newRow["Street"] = row["Street"];
            //                    if (row["CenterKilometres"].ToString() == "未知")
            //                    {
            //                        newRow["CenterKilometres"] = 0;
            //                    }
            //                    else
            //                    {
            //                        newRow["CenterKilometres"] = row["CenterKilometres"];
            //                    }
            //                    newRow["MinWeight"] = colNameArrays[0];
            //                    newRow["MaxWeight"] = colNameArrays[1];
            //                    newRow["DeliveryFee"] = row[dc.ColumnName];
            //                    newRow["TransferMode"] = row["TransferMode"];
            //                    newRow["Remark"] = row["Remark"];
            //                    newRow["bsitename"] = row["bsitename"];
            //                    newRow["HeavyPrice"] = row["HeavyPrice"];
            //                    newRow["LightPrice"] = row["LightPrice"];
            //                    newDt.Rows.Add(newRow);
            //                }
            //            }
            //        }
            //    }
            //}
        }


        private void ShowInformation(string msg)
        {
            XtraMessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool ShowQuestion()
        {
            return DialogResult.Yes == XtraMessageBox.Show("停止上传？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private static void mytimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DataUploadOK == true)
                {
                    w_progressBar.CloseWindow = true;
                    DataUploadOK = false;
                    mytimer.Enabled = false;
                    mytimer.Tick -= new System.EventHandler(mytimer_Tick);
                }
            }
            catch (Exception)
            {
                w_progressBar.CloseWindow = true;
                w_progressBar.IsException = true;
                mytimer.Enabled = false;
            }
        }


        private void InportExcel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择结算费用文件";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataTable dt = NpoiOperExcel.ExcelToDataTable2(ofd.FileName, true);
                if (dt == null)
                    return;
                SetColumnName(dt.Columns);
                myGridControl1.DataSource = dt;
            }
        }

        private DataSet DsExecl(string filePath)
        {
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接可以兼容2003和2007
            //string str = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接必须要安装2007
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                System.Data.DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() != DialogResult.Yes)
                { return null; }
                tablename = wi.listBoxControl1.Text.Trim();
                sql = "select * from [" + tablename + "]";

                OleDbDataAdapter da = new OleDbDataAdapter(sql, Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);

                try
                {
                    SetColumnName(ds.Tables[0].Columns);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex, "转换失败!\r\n请检查EXCEL列头是否与模板一致！");
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "加载结算送货费失败");
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
             
                c["申报事业部"].ColumnName = "ApplyCause";
                c["申报大区"].ColumnName = "ApplyArea";
                c["申报部门"].ColumnName = "ApplyDept";
                c["登记人"].ColumnName = "Registrant";
                c["项目"].ColumnName = "FeeProject";
                c["费用类型"].ColumnName = "FeeType";
                c["所属月份"].ColumnName = "BelongMonth";
                c["摘要"].ColumnName = "Remark";
                c["金额"].ColumnName = "Money";
                c["承担部门"].ColumnName = "AssumeDept";               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void fmDeliveryFeeUp_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
           
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "送货费，中转费标准");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
        }

        private void barBtnDown_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string file = "\\结算送货费模板.xls";
                FolderBrowserDialog sfd = new FolderBrowserDialog();
                sfd.Description = "另存为";
                if (sfd.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }
                File.Copy(System.Windows.Forms.Application.StartupPath + file, sfd.SelectedPath + file, true);
                XtraMessageBox.Show("下载成功!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message + "\r保存失败，请重新下载!如果再次失败请关闭程序，再次下载!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //if (e != null && e.Column.FieldName == "yf_type")
            //{
            //    if ((e.Value + "").Trim() == "1")
            //        e.DisplayText = "快件";
            //    else
            //        e.DisplayText = "普件";
            //}
        }

        private void tsmiDeleteRow_Click(object sender, EventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            if (XtraMessageBox.Show("确定删除该行？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) return;
            myGridView1.DeleteRow(rowHandle);
        }
    }
}
