using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using ZQTMS.SqlDAL;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmRPofDriverLead : BaseForm
    {
        public frmRPofDriverLead()
        {
            InitializeComponent();
        }

        private void frmRPofDriverLead_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
        }

        private void cbLeadIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
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
                    //ds.Tables[0].Columns.Add("balance_money_Auto", typeof(float));
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
                MsgBox.ShowException(ex, "加载司机奖罚记录失败");
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
                c["金额"].ColumnName = "Amount";
                c["发站"].ColumnName = "BSite";
                c["车牌"].ColumnName = "CarNO";
                c["发车批次号"].ColumnName = "DepartureBatch";
                c["司机姓名"].ColumnName = "DriverName";
                c["备注"].ColumnName = "RPContent";
                c["到站"].ColumnName = "ToSite";
                c["发车日期"].ColumnName = "WebDate";
                c["选项类型"].ColumnName = "cbMoneyType";
                //c["登记时间"].ColumnName = "RegisterDate";
                //c["登记人"].ColumnName = "RegisterMan";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private bool validate()
        {
             string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
             string negPattern = @"^\-[0-9]+(.[0-9]{1,2})?$";//验证负数正则
            string OutDepartureBatchs = "";//所有奖励支出发车批次
                string FineDepartureBatchs = "";//所以代扣罚款发车批次
                string dateDepartureBatchs = "";//所有时间格式不正确的发车批次
                string typeDepartureBatchs = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                
                string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, i, "DepartureBatch");//DepartureBatch
                //string WebDate = GridOper.GetRowCellValueString(myGridView1, i, "WebDate");
                string BSite = GridOper.GetRowCellValueString(myGridView1, i, "BSite");//发站
                string ToSite = GridOper.GetRowCellValueString(myGridView1, i, "ToSite");//到站
                string DriverName = GridOper.GetRowCellValueString(myGridView1, i, "DriverName");
                string CarNO = GridOper.GetRowCellValueString(myGridView1, i, "CarNO");//车牌
                string Amount = GridOper.GetRowCellValueString(myGridView1, i, "Amount");//金额
                string RPContent = GridOper.GetRowCellValueString(myGridView1, i, "RPContent");//备注
                string cbMoneyType = GridOper.GetRowCellValueString(myGridView1, i, "cbMoneyType");//项目类型
                if (cbMoneyType.Trim() != "奖励支出" && cbMoneyType.Trim() != "代扣罚款")
                {
                    typeDepartureBatchs += DepartureBatch + ",";
                }
                //发车时间
                if (!IsDateTime(GridOper.GetRowCellValueString(myGridView1, i, "WebDate")))
                {                   
                    dateDepartureBatchs += DepartureBatch + ",";
                }      
                if (cbMoneyType == "奖励支出" && !Regex.IsMatch(Amount, posPattern))
                {
                    OutDepartureBatchs+= DepartureBatch+",";                    
                }
                if (cbMoneyType == "代扣罚款" && !Regex.IsMatch(Amount, negPattern))
                {
                    FineDepartureBatchs += DepartureBatch+",";
                }
            }
            if (typeDepartureBatchs != "")
            {
                MsgBox.ShowOK("检测到批次号：【" + typeDepartureBatchs + "】,选项类型不正确\r\n当项目类型必须为“奖励支出“或“代扣罚款”，请检查！");
                return false;
            }
            if (OutDepartureBatchs != "")
            {
                MsgBox.ShowOK("检测到批次号：【"+OutDepartureBatchs+"】,金额格式不正确\r\n当项目类型为“奖励支出“时，金额必须为正数，请检查！");
                return false;
            }
            if (FineDepartureBatchs != "")
            {
                MsgBox.ShowOK("检测到批次号：【" + FineDepartureBatchs + "】,金额格式不正确\r\n当项目类型为“代扣罚款”时，金额必须为负数，请检查！");
                return false;
            }
            if (dateDepartureBatchs != "")
            {
                MsgBox.ShowOK("检测到批次号：【" + dateDepartureBatchs + "】,发车时间格式不正确，请检查！");
                return false;
            }
            return true;
           
        }
          
        //上传至服务器
        private void cbUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();           
            if (myGridView1.RowCount == 0)
            {
                return;
            }

            if (myGridView1.RowCount > 500)
            {
                XtraMessageBox.Show("数据导入不能超过500条！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {            
                DataTable newDt = new DataTable();
                newDt.Columns.Add("Amount", typeof(decimal));//金额
                newDt.Columns.Add("BSite", typeof(string));//发站
                newDt.Columns.Add("CarNO", typeof(string));//车牌
                newDt.Columns.Add("DepartureBatch", typeof(string));//发车批次号
                newDt.Columns.Add("DriverName", typeof(string));//司机姓名
                newDt.Columns.Add("RPContent", typeof(string));//备注
                newDt.Columns.Add("ToSite", typeof(string));//到站
                newDt.Columns.Add("WebDate", typeof(DateTime));//发车日期
                newDt.Columns.Add("cbMoneyType", typeof(string));//选项类型
                //newDt.Columns.Add("RegisterDate", typeof(DateTime));//登记时间 hj 20171208
                newDt.Columns.Add("RegisterMan", typeof(string));//登记人 hj 20171208
                string DepartureBatchs = "";
                //newDt.Columns.Add("BankName", typeof(string));
                if (validate())
                {
                    
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        #region 验证数据合法性

                        string DepartureBatch = GridOper.GetRowCellValueString(myGridView1, i, "DepartureBatch");//DepartureBatch
                        //string WebDate = GridOper.GetRowCellValueString(myGridView1, i, "WebDate");
                        string BSite = GridOper.GetRowCellValueString(myGridView1, i, "BSite");//发站
                        string ToSite = GridOper.GetRowCellValueString(myGridView1, i, "ToSite");//到站
                        string DriverName = GridOper.GetRowCellValueString(myGridView1, i, "DriverName");
                        string CarNO = GridOper.GetRowCellValueString(myGridView1, i, "CarNO");//车牌
                        string Amount = GridOper.GetRowCellValueString(myGridView1, i, "Amount");//金额
                        string RPContent = GridOper.GetRowCellValueString(myGridView1, i, "RPContent");//备注
                        string cbMoneyType = GridOper.GetRowCellValueString(myGridView1, i, "cbMoneyType");//项目类型
                        string RegisterMan = CommonClass.UserInfo.UserName; //登记人 hj
                        DateTime WebDate = new DateTime();
                        if (string.IsNullOrEmpty(GridOper.GetRowCellValueString(myGridView1, i, "WebDate")))
                        {
                            WebDate = DateTime.Now;
                        }
                        else
                        {
                            WebDate = ConvertType.ToDateTime(GridOper.GetRowCellValueString(myGridView1, i, "WebDate"));
                        }
                       
                        #endregion
                        DataRow dr = newDt.NewRow();
                        dr["Amount"] = Convert.ToDecimal(Amount);
                        dr["BSite"] = BSite;
                        dr["CarNO"] = CarNO;
                        dr["DepartureBatch"] = DepartureBatch;
                        dr["DriverName"] = DriverName;
                        dr["RPContent"] = RPContent;
                        dr["ToSite"] = ToSite;
                        dr["WebDate"] = WebDate;
                        dr["cbMoneyType"] = cbMoneyType;
                        //dr["RegisterDate"] = RegisterDate;
                        dr["RegisterMan"] = RegisterMan;
                        newDt.Rows.Add(dr);
                        DepartureBatchs += DepartureBatch+"@";
                    }
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("Tb", newDt));
                    list.Add(new SqlPara("DepartureBatchs", DepartureBatchs));
                    list.Add(new SqlPara("Ischeck", UserRight.GetRight("82") ? 0 : 1));

                    // list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                    //list.Add(new SqlPara("WebDate", WebDate));
                    //list.Add(new SqlPara("BSite", BSite));
                    // list.Add(new SqlPara("ToSite", ToSite));
                    //list.Add(new SqlPara("DriverName", DriverName));
                    //list.Add(new SqlPara("CarNO", CarNO));
                    //list.Add(new SqlPara("Amount", Amount));
                    // list.Add(new SqlPara("RPContent", RPContent));
                    // list.Add(new SqlPara("cbMoneyType", cbMoneyType));

                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_RPofDriver", list)) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                   
                }
               
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        
        /// <summary>
        /// 是否为时间类型的数据
        /// </summary>
        /// <param name="str">用于判断的目标字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(String str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime temp;

                return DateTime.TryParse(str, out temp);
            }
            else
            {
                return true;
            }
        }

        private void cbClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void cbClose_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbLeadIn_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "司机奖罚登记";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataSet ds = DsExecl(ofd.FileName);
                if (ds.Tables[0] == null)
                    return;
                int i = 1;
                foreach (DataColumn columns in ds.Tables[0].Columns)
                {
                    if (columns.ColumnName.IndexOf('-') > 0)
                    {
                        GridColumn column = new GridColumn();
                        column.Caption = columns.ColumnName;
                        column.FieldName = columns.ColumnName;
                        column.Name = "gridColumnDt" + i;
                        column.Visible = true;
                        column.VisibleIndex = 6 + i;
                        column.Width = 80;
                        myGridView1.Columns.Add(column);
                        i++;
                    }
                }
                if (ds != null)
                {
                    myGridControl1.DataSource = ds.Tables[0];

                }
            }
        }
    }
}
