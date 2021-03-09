using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using DevExpress.XtraGrid.Columns;
using System.Data.OleDb;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Net;

namespace ZQTMS.UI
{
    public partial class frmBasARSendFeeToWeightImport : BaseForm
    {
        public frmBasARSendFeeToWeightImport()
        {
            InitializeComponent();
        }

        DataSet dsNew = new DataSet();
        DataSet ds = new DataSet();


        //load
        private void frmBasARSendFeeToWeightImport_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("应收报价重量导入");
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar3);
        }


        //导入
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ImportExcel();
        }

        //导入方法
        private void ImportExcel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "选择应收报价重量文件";
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

                DataSet ds = DsExecl(ofd.FileName);

                if (ds != null)
                {
                    DataTable ddt = new DataTable();
                    ddt = ds.Tables[0];
                    RemoveEmpty(ddt);
                    myGridControl1.DataSource = ddt;
                }
            }
        }

        //将excel作为数据库，查数据并填充到数据源中
        private DataSet DsExecl(string filePath)
        {
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接可以兼容2003和2007
            //string str = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接必须要安装2007
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";//gxh
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

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
                MsgBox.ShowException(ex, "加载应收报价重量失败");
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        //设置列名
        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                c["运单号"].ColumnName = "BillNo";
                c["报价名称"].ColumnName = "OfferName";
                c["客商"].ColumnName = "Merchant";
                c["运费"].ColumnName = "FreightCharge";
                c["合同有效开始时间"].ColumnName = "ContractBegin";
                c["合同有效结束时间"].ColumnName = "ContractEnd";
                c["始发地"].ColumnName = "Origin";
                c["目的地"].ColumnName = "Destination";
                c["销售项目"].ColumnName = "SaleProject";
                c["最低收费"].ColumnName = "MinimumCharge";
                c["备注"].ColumnName = "Remark";
                c["区间一开始重量(KG)"].ColumnName = "BeginWeightOne";
                c["区间一结束重量(KG)"].ColumnName = "EndWeightOne";
                c["区间一价格(元/KG)"].ColumnName = "WeightOnePrice";
                c["区间二开始重量(KG)"].ColumnName = "BeginWeightTwo";
                c["区间二结束重量(KG)"].ColumnName = "EndWeightTwo";
                c["区间二价格(元/KG)"].ColumnName = "WeightTwoPrice";
                c["区间三开始重量(KG)"].ColumnName = "BeginWeightThree";
                c["区间三结束重量(KG)"].ColumnName = "EndWeightThree";
                c["区间三价格(元/KG)"].ColumnName = "WeightThreePrice";
                c["区间四开始重量(KG)"].ColumnName = "BeginWeightFour";
                c["区间四结束重量(KG)"].ColumnName = "EndWeightFour";
                c["区间四价格(元/KG)"].ColumnName = "WeightFourPrice";
                c["公司ID"].ColumnName = "CompanyID";
                c["操作人"].ColumnName = "Operator";

            }
            catch (Exception ex)
            {
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }


        //删除空行
        public DataTable RemoveEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool IsNull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        IsNull = false;
                    }
                }
                if (IsNull)
                {
                    removelist.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }

        //上传
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.myGridView1.RowCount == 0) return;

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (myGridView1.GetRowCellValue(i, "BillNo").ToString() == "")
                {
                    MsgBox.ShowOK("运单号不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "FreightCharge").ToString() == "")
                {
                    MsgBox.ShowOK("运费不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "Origin").ToString() == "")
                {
                    MsgBox.ShowOK("始发地不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "Destination").ToString() == "")
                {
                    MsgBox.ShowOK("目的地不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "OfferName").ToString() == "")
                {
                    MsgBox.ShowOK("报价名称不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "Merchant").ToString() == "")
                {
                    MsgBox.ShowOK("承运商不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "SaleProject").ToString() == "")
                {
                    MsgBox.ShowOK("销售项目不允许为空！");
                    return;
                }
            }

            DataTable dt = myGridControl1.DataSource as DataTable;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BasARSendFeeToWeight_Import", new List<SqlPara>() { new SqlPara("Tb", dt) })) > 0)
            {
                MsgBox.ShowOK("上传成功！");
                this.Close();
            }
            else
            {
                MsgBox.ShowOK("上传失败！");
            }
        }


        //下载模板
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<word> list = new List<word>();
            word wd = new word();
            wd.name = "应收报价模板";
            wd.path = "http://8.129.7.49/应收报价运费模板.xls";
            list.Add(wd);
            Download(list);
        }


        public class word
        {
            public string name;
            public string path;
        }

        //下载方法
        private static void Download(List<word> list)
        {
            try
            {

                WebClient client = new WebClient();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Microsoft Word文件(*.doc;*.docx)|*.doc;*.docx";//定义文件格式  
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.RestoreDirectory = true;//保存对话框是否记忆上次打开的目录  
                saveFileDialog.FileName = "应收报价模板.xls";
                saveFileDialog.Title = "导出Excel模板文件到";
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
                        localFilePath = FilePath + "\\" + list[i].name + ".xls";
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


        //退出
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


       

    }
}
