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
    public partial class frmBasARFeeToVolumeImport1 : BaseForm
    {
        public frmBasARFeeToVolumeImport1()
        {
            InitializeComponent();
        }

        DataSet dsNew = new DataSet();
        DataSet ds = new DataSet();        

        //Load
        private void frmBasARFeeToVolumeImport1_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("应收报价方数导入");
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
            ofd.Title = "选择应收报价方数文件";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if(ofd.ShowDialog()==DialogResult.OK)
            {
                if(!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excelwenjian","文件导入失败",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择!", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataSet ds = DsExecl(ofd.FileName);
                if(ds!=null)
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
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection conn = new OleDbConnection(str);
            try
            {
                conn.Open();
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() != DialogResult.Yes) return null;
                tablename = wi.listBoxControl1.Text.Trim();
                sql = "select * from [" + tablename + "]";
                OleDbDataAdapter dap = new OleDbDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                dap.Fill(ds, tablename);
                try 
                {
                    SetColumnName(ds.Tables[0].Columns);
                }
                catch(Exception ex)
                {
                    MsgBox.ShowException(ex, "转换失败!\r\n请检查EXCEL列头是否与模板一致！");
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "加载应收报价方数失败！");
                return null;
            }
            finally
            { 
                if(conn.State==ConnectionState.Open) conn.Close();
            }


        }

        //设置列名
        private void SetColumnName(DataColumnCollection c)
        {
            try
            { 
                foreach(DataColumn dc in c)     
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }
                c["运单号"].ColumnName = "BillNo";
                c["报价名称"].ColumnName = "offerName";
                c["客商"].ColumnName = "travellingTrader";
                c["送货费"].ColumnName = "DelieryCharge";
                c["合同有效开始时间"].ColumnName = "startTime";
                c["合同有效结束时间"].ColumnName = "endTime";
                c["始发地"].ColumnName = "lindisfarne";
                c["目的地"].ColumnName = "destination";
                c["销售项目"].ColumnName = "salesProject";
                c["最低收费"].ColumnName = "minimumCharge";
                c["备注"].ColumnName = "remark";
                c["区间一开始体积(方)"].ColumnName = "sOneStartVolume";
                c["区间一结束体积(方)"].ColumnName = "sOneEndVolume";
                c["区间一价格(元/方)"].ColumnName = "sOnePrice";
                c["区间二开始体积(方)"].ColumnName = "sTwoStartVolume";
                c["区间二结束体积(方)"].ColumnName = "sTwoEndVolume";
                c["区间二价格(元/方)"].ColumnName = "sTwoPrice";
                c["区间三开始体积(方)"].ColumnName = "sThreeStartVolume";
                c["区间三结束体积(方)"].ColumnName = "sThreeEndVolume";
                c["区间三价格(元/方)"].ColumnName = "sThreePrice";
                c["区间四开始体积(方)"].ColumnName = "sFourStartVolume";
                c["区间四结束体积(方)"].ColumnName = "sFourEndVolume";
                c["区间四价格(元/方)"].ColumnName = "sFourPrice";
                c["公司ID"].ColumnName = "companyID";
                c["操作人"].ColumnName = "operationPeople";
               
            }
            catch(Exception ex)
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
            for (int i = 0; i < this.myGridView1.RowCount; i++)
            {
                if (myGridView1.GetRowCellValue(i, "BillNo").ToString() == "")
                {
                    MsgBox.ShowOK("运单号不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "DelieryCharge").ToString() == "")
                {
                    MsgBox.ShowOK("送货费不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "lindisfarne").ToString() == "")
                {
                    MsgBox.ShowOK("始发地不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "destination").ToString() == "")
                {
                    MsgBox.ShowOK("目的地不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "offerName").ToString() == "")
                {
                    MsgBox.ShowOK("报价名称不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "travellingTrader").ToString() == "")
                {
                    MsgBox.ShowOK("客商不允许为空！");
                    return;
                }
                if (myGridView1.GetRowCellValue(i, "salesProject").ToString() == "")
                {
                    MsgBox.ShowOK("销售项目不允许为空！");
                    return;
                }

            }

            DataTable dt = myGridControl1.DataSource as DataTable;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BasARFeeToVolume_Import", new List<SqlPara>() { new SqlPara("Tb", dt) })) > 0)
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
            wd.path = "http://8.129.7.49/应收报价模板.xls";
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
                        //string newFileName = DateTime.Now.ToString("yyyyMMdd") + fileNameExt; //给文件名前加上时间  
                        //saveFileDialog.FileName.Insert(1, "dameng");//在文件名里加字符  
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
