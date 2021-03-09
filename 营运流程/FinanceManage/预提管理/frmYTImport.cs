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

namespace ZQTMS.UI
{
    public partial class frmYTImport : BaseForm
    {
        public frmYTImport()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "预提信息记录";
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
                MsgBox.ShowException(ex, "加载预提信息失败");
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
                c["承担网点"].ColumnName = "CDWebName";
                c["预提类型"].ColumnName = "ProvisionType";
                c["预提科目"].ColumnName = "ProvisionSub";
                c["摘要"].ColumnName = "ReMark";
                c["预提金额"].ColumnName = "ProvisionAmount";
                c["流程号"].ColumnName = "FlowId";

            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式！");
            }

        }
        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.RowCount == 0)
                {
                    return;
                }
                DataTable dt = myGridControl1.DataSource as DataTable;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    #region 检测导入时承担网点、预提类型、预提科目是否正确、是否为空
                    //if (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) != "成本类" && ConvertType.ToString(dt.Rows[j]["ProvisionType"]) != "费用类"
                    //    && ConvertType.ToString(dt.Rows[j]["ProvisionType"]) != "其他" && ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "")
                    //{
                    //    XtraMessageBox.Show("请检查【预提类型】", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    if (string.IsNullOrEmpty(ConvertType.ToString(dt.Rows[j]["CDWebName"])))
                    {
                        MsgBox.ShowOK("网点为空跳过！");
                        continue;
                    }
                    if (ConvertType.ToDecimal(dt.Rows[j]["ProvisionAmount"]) <= 0)
                    {
                        MsgBox.ShowOK("导入的预提金额必须大于0,请检查！");
                        return;
                    }
                    //if ((ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "生活费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "社保费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "保险费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "档口房租")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "宿舍房租")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "水电费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "办公用品")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "快递费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "垫板费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "通讯费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "差旅费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "会议费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "培训费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "业务费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "劳务费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "招骋费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "工程/资产维修费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "装修费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "叉车费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "广告费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "福利费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "工资（非部门）")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "奖金")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "费用类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "货量折扣")

                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "其他类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "财务费用")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "其他类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "管理费用")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "其他类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "税金")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "其他类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "营业外支出")

                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "成本类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "车辆加油费")
                    //    && (ConvertType.ToString(dt.Rows[j]["ProvisionType"]) == "成本类" && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "车辆维修费")
                    //    && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) == "")
                    //{
                    //    XtraMessageBox.Show("请检查【预提科目】", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}

                    if ( ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "办公费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "木板费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "生活费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "房租费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "水电费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "通讯费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "差旅费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "业务费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "叉车费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "招骋费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "会议费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "培训费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "广告费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "福利费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "社保"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "货物运输险成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "维修费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "装修费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "外请劳务"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "财务费用"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "税务管理成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "奖金及提成"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "其他营业外支出"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "车辆油料成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "车辆修理成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "路桥停车成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "车辆保险成本"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "其他自有车辆成本"
                        &&  ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "咨询服务费"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) != "其他费用"
                        && ConvertType.ToString(dt.Rows[j]["ProvisionSub"]) == "")
                    {
                        XtraMessageBox.Show("请检查【预提科目】", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    int count = 0;
                    for (int i = 0; i < CommonClass.dsDep.Tables[0].Rows.Count; i++)
                    {
                        string dsDepName = CommonClass.dsDep.Tables[0].Rows[i]["DepName"].ToString();
                        if (dsDepName.Contains(dt.Rows[j]["CDWebName"].ToString()))
                        {
                            count++;
                        }
                    }
                    if (count == 0)
                    {
                        XtraMessageBox.Show("请检查【承担网点】", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    #endregion
                }
                dt.Columns.Remove("序号");
                dt.AcceptChanges();
                DataTable newDt = new DataTable();
                newDt.Columns.Add("CDWebName", typeof(string));
                newDt.Columns.Add("ProvisionType", typeof(string));
                newDt.Columns.Add("ProvisionSub", typeof(string));
                newDt.Columns.Add("ReMark", typeof(string));
                newDt.Columns.Add("ProvisionAmount", typeof(decimal));
                newDt.Columns.Add("FlowId", typeof(string));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(ConvertType.ToString(dt.Rows[i]["CDWebName"])))
                    {
                        //MsgBox.ShowOK("网点为空跳过！");
                        continue;
                    }
                    DataRow dr = newDt.NewRow();
                    dr["CDWebName"] = ConvertType.ToString(dt.Rows[i]["CDWebName"]);
                    dr["ProvisionType"] = ConvertType.ToString(dt.Rows[i]["ProvisionType"]);
                    dr["ProvisionSub"] = ConvertType.ToString(dt.Rows[i]["ProvisionSub"]);
                    dr["ReMark"] = ConvertType.ToString(dt.Rows[i]["ReMark"]);
                    dr["ProvisionAmount"] = ConvertType.ToString(dt.Rows[i]["ProvisionAmount"]);
                    dr["FlowId"] = ConvertType.ToString(dt.Rows[i]["FlowId"]);
                    newDt.Rows.Add(dr);
                }
                newDt.Columns["CDWebName"].SetOrdinal(0);
                newDt.Columns["ProvisionType"].SetOrdinal(1);
                newDt.Columns["ProvisionSub"].SetOrdinal(2);
                newDt.Columns["ReMark"].SetOrdinal(3);
                newDt.Columns["ProvisionAmount"].SetOrdinal(4);
                newDt.Columns["FlowId"].SetOrdinal(5);
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YT_UPLOAD", new List<SqlPara>() { new SqlPara("Tb", newDt) })) > 0) {
                    MsgBox.ShowOK("上传成功！");
                    this.Close();
                }
                //string msg = "";
                //if (CommonClass.BasUpload(newDt, "USP_ADD_YT_UPLOAD", out msg))
                //{

                //    MsgBox.ShowOK(msg);
                //    this.Close();
                //}
                //else
                //{
                //    MsgBox.ShowError(msg);
                //}
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1,"预提信息记录");
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmYTImport_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2);
        }
    }
}