using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using gregn6Lib;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfrmPrintRuiLangWithCondition : BaseForm
    {
        public JMfrmPrintRuiLangWithCondition() { InitializeComponent(); }

        public JMfrmPrintRuiLangWithCondition(string reportType)
        {
            InitializeComponent();
            this.reportType = reportType;
            this.Text = GetWinTitle();
        }

        int ColumnCount = 0;
        GridppReport Report = new GridppReport();
        DataSet ds;//数据源
        DataSet dsKid;//子报表数据源
        GridppReport ReportKid = new GridppReport();
        DataTable dtsearch;
        string reportName = "";//报表文件名称
        frmRuiLangService frl = new frmRuiLangService();
        string reportType = "";//报表

        private void LoadColumn()
        {
            try
            {
                //绑定报表里的列到显示隐藏列的树
                TreeListNode t0 = treeList1.AppendNode(null, 0);
                t0.SetValue("ColumnName", "全部");
                t0.SetValue("tag", "全部");

                foreach (IGRColumn c in Report.DetailGrid.Columns)
                {
                    TreeListNode t1 = treeList1.AppendNode(null, t0);
                    t1.SetValue("ColumnName", c.TitleCell.Text);
                    t1.SetValue("tag", c.Name);
                    t1.Checked = c.Visible;
                    ColumnCount++;

                    try
                    {
                        #region..加载数据到筛选下拉框中..
                        cbcolumnName.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(c.TitleCell.Text, Report.FieldByName(c.ContentCell.DataField).DBFieldName));
                        edtbcolumn.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(c.TitleCell.Text, Report.FieldByName(c.ContentCell.DataField).DBFieldName));
                        #endregion
                    }
                    catch (Exception)
                    { }
                }
                treeList1.FocusedNode.Expanded = true;
                ValidParentNodeIsCanleSel(t0.Nodes[0]);
            }
            catch (Exception)
            { }
        }

        private void ReStart()
        {
            try
            {
                if (axGRDisplayViewer1.Report != null)
                    axGRDisplayViewer1.Stop();

                Report.LoadDataFromXML(ds.GetXml());
                axGRDisplayViewer1.Report = Report;

                axGRDisplayViewer1.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            groupControl2.Visible = false;
        }

        private void w_report_detail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar3);//如果有具体的工具条，就引用其实例

            dtsearch = frl.datatable();
            dtsearch.Rows.Clear();
            gridshow.DataSource = dtsearch;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            if (reportType == "1" || reportType == "21") 
            {
                bdate.DateTime = CommonClass.gbdate.AddHours(-16);
                edate.DateTime = CommonClass.gedate.AddHours(-16);
            }
            if (reportType == "2")
            {
                DateTime bdt = CommonClass.gbdate;
                bdt = bdt.AddDays(-2);
                bdt = bdt.AddHours(18);
                bdate.DateTime = bdt;
                DateTime edt = CommonClass.gedate;
                edt = edt.AddDays(-1);
                edt = edt.AddHours(17 - edt.Hour);
                edate.DateTime = edt;

            }

            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
            CommonClass.SetSite(true, StartSite, EndSiteName);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
            UserName.Properties.Items.Clear();
            UserName.Text = "全部";
            StartSite.Text = "全部";
            EndSiteName.Text = "全部";
            
            setUserEnable();
        }

        private void Init()
        {
            //加载锐浪报表文件
            if (!LoadReportFile()) return;
            try
            {
                LoadColumn();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 加载锐浪报表文件,放Reports下
        /// </summary>
        private bool LoadReportFile()
        {
            string reportpath = Application.StartupPath + "\\Reports\\" + reportName;
            if (!System.IO.File.Exists(reportpath))
            {
                if (reportName != "") MsgBox.ShowError("找不到相应的打印模板文件【" + reportName + "】");
                return false;
            }
            if (ds == null)
            {
                MsgBox.ShowError("没有数据,不能打印!");
                return false;
            }
            if (Report != null) axGRDisplayViewer1.Stop();

            Report.LoadFromFile(reportpath);
            Report.LoadDataFromXML(ds.GetXml());
            if (reportType == "14" || reportType == "15" || reportType == "16" || reportType == "17" || reportType == "1" || reportType == "2" || reportType == "3" || reportType == "19")
            {
                ReportKid = Report.ControlByName("SubReport1").AsSubReport.Report;
                ReportKid.LoadDataFromXML(dsKid.GetXml());
            }

            try
            {
                Report.ControlByName("txt").AsStaticBox.Text = "时间从 " + bdate.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "到" + edate.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "　区间从 " + StartSite.Text + " 到 " + EndSiteName.Text + " 事业部:" + CauseName.Text + "　大区:" + AreaName.Text + "　网点:" + WebName.Text;
            }
            catch { }

            Report.Printer.PrinterName = frmRuiLangService.GetPrinterName(reportName);
            axGRDisplayViewer1.Report = Report;
            axGRDisplayViewer1.Start();
            return true;
        }

        private void treeList1_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            try
            {
                if (e.Node == null) return;
                System.Data.DataRowView rov = treeList1.GetDataRecordByNode(e.Node) as System.Data.DataRowView;
                if (e.Node.CheckState == CheckState.Indeterminate)
                {
                    e.Node.CheckState = CheckState.Checked;
                }

                if (e.Node["ColumnName"].ToString() != "全部")
                {
                    if (e.Node.Checked)
                    {
                        Report.DetailGrid.Columns[e.Node["tag"].ToString()].Visible = true;
                    }
                    else
                    {
                        if (!CheckColumnCount())
                        {
                            e.Node.CheckState = CheckState.Checked;
                            MsgBox.ShowOK("最后一列不能隐藏。");
                            return;
                        }
                        Report.DetailGrid.Columns[e.Node["tag"].ToString()].Visible = false;
                    }
                }

                CheckNode(e.Node);

                //从根节点往下 查询 是否有打钩的子节点，如果有那么 父节点的 半选状态不变否则变为 不选择状态
                ValidParentNodeIsCanleSel(e.Node);

                ReStart();
            }
            catch (Exception)
            {

            }
        }

        private void CheckNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
            {
                if (cnode != null)
                {

                    cnode.Checked = node.Checked;
                    if (cnode.Checked)
                    {
                        Report.DetailGrid.Columns[cnode["tag"].ToString()].Visible = true;
                    }
                    else
                    {
                        if (!CheckColumnCount())
                        {
                            cnode.Checked = true;
                            MsgBox.ShowOK("最后一列不能隐藏。");
                            return;
                        }
                        Report.DetailGrid.Columns[cnode["tag"].ToString()].Visible = false;
                    }
                }
                if (cnode.HasChildren)
                {
                    CheckNode(cnode);
                }
            }
        }
        /// <summary>
        /// 校验隐藏列,必须显示一列
        /// </summary>
        /// <returns></returns>
        private bool CheckColumnCount()
        {
            try
            {
                int count = 0;
                foreach (IGRColumn c in Report.DetailGrid.Columns)
                {
                    if (c.Visible == false)
                    {
                        count++;
                    }
                    if (count == ColumnCount - 1)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        private void ValidParentNodeIsCanleSel(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            bool isSel = false;
            if (node.ParentNode != null)
            {
                //如果父节点的 状态为 半选 状态 这 更具父节点 判断子节点是否打钩
                isSel = ValidIsHasCheckChildNode(node.ParentNode);
                if (isSel == false)
                {//如果所有的 子节点 都没有 “选中”那么 父节点的状态 变为 非选中状态
                    node.ParentNode.CheckState = CheckState.Unchecked;
                }
                else
                {
                    node.ParentNode.CheckState = CheckState.Checked;
                }

                ValidParentNodeIsCanleSel(node.ParentNode);
            }
        }
        /// <summary>
        /// 判断 子节点 是否 有 状态为 “选中”状态 
        /// true 表示有 false 表示为 没有
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool ValidIsHasCheckChildNode(DevExpress.XtraTreeList.Nodes.TreeListNode node)
        {
            bool isCheck = true;
            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode cnode in node.Nodes)
            {
                if (cnode != null)
                {
                    if (cnode.CheckState == CheckState.Unchecked)
                    {
                        isCheck = false;
                        return isCheck;
                    }
                }
            }
            return isCheck;
        }

        private void gridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int row = gridView1.FocusedRowHandle;
                if (row < 0) return;
                if (e.KeyCode == Keys.Enter)
                {
                    string fieldName = gridView1.FocusedColumn.FieldName;
                    if (fieldName == "value" && row == gridView1.RowCount - 1)
                    {
                        gridView1.AddNewRow();
                        gridView1.FocusedRowHandle = gridView1.RowCount - 1;
                        gridView1.FocusedColumn = gridColumn1;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int rows = gridView1.RowCount;
                if (rows < 1) return;
                string columnName = "", operators = "", value = "";
                string result = "";
                DataTable dt = ds.Tables[0].Clone();
                DataSet dsnew = new DataSet();
                axGRDisplayViewer1.Stop();
                for (int i = 0; i < rows; i++)
                {
                    columnName = gridView1.GetRowCellValue(i, "columnName").ToString();
                    operators = gridView1.GetRowCellValue(i, "operators").ToString();
                    value = gridView1.GetRowCellValue(i, "value").ToString();
                    if (columnName == "" || operators == "" || value == "") continue;
                    if (value == "" && !frl.GetStr(operators).Contains("null")) continue;
                    value = frl.GetStr(operators).Contains("like") ? "'%" + value + "%'" : (frl.GetStr(operators).Contains("null") ? "" : "'" + value + "'");
                    result += columnName + " " + frl.GetStr(operators) + value + " AND ";
                }
                DataRow[] drs = ds.Tables[0].Select(result.Trim().Substring(0, result.Length - 4));
                foreach (DataRow dr in drs)
                {
                    DataRow dr2 = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr2[dc.ColumnName] = dr[dc.ColumnName];
                    }
                    dt.Rows.Add(dr2);
                }
                dsnew.Tables.Add(dt);
                Report.LoadDataFromXML(dsnew.GetXml());
                axGRDisplayViewer1.Report = Report;
                axGRDisplayViewer1.Start();
                //ReStart();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            axGRDisplayViewer1.Stop();
            ReStart();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            groupControl3.Visible = false;
        }

        Point p;
        private void groupControl3_MouseDown(object sender, MouseEventArgs e)
        {
            p = Cursor.Position;
        }

        private void groupControl3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int px = Cursor.Position.X - p.X;
                int py = Cursor.Position.Y - p.Y;
                groupControl3.Location = new Point(groupControl3.Location.X + px, groupControl3.Location.Y + py);

                p = Cursor.Position;
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            if (edtbcolumn.Text == "" || edoperators.Text.Trim() == "" || edvalue.Text.Trim() == "")
            {
                MsgBox.ShowOK("请将条件选择完整后在添加！", "系统提示！");
                return;
            }
            if (dtsearch == null)
            {
                return;
            }
            dtsearch.Rows.Add(new object[] { edtbcolumn.EditValue, edoperators.Text.Trim(), edvalue.Text.Trim() });
            edvalue.Text = "";
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0)
            {
                return;
            }
            gridView1.DeleteSelectedRows();
            gridView1.PostEditor();
        }

        /// <summary>
        /// 获取过程名称
        /// </summary>
        /// <returns></returns>
        private string GetProc()
        {
            switch (reportType)
            {
                case "1"://营业日报表
                    reportName = "营业额日报表.grf";
                    return "QSP_GET_WAYBILL_DailySales";
                case "2"://网点出库表
                    reportName = "网点出库表.grf";
                    return "QSP_GET_WAYBILL_WEBOUTBOUND";
                case "3"://账款收银确认报表
                    reportName = "账款收银确认报表.grf";
                    return "QSP_GET_WAYBILL_CASHIER";
                case "4":
                    reportName = "派车费核销报表.grf";
                    return "QSP_GET_Delivery_Report";
                case "5":
                    reportName = "中转费核销报表.grf";
                    return "QSP_GET_AccMiddlePay_Verify_Report";
                case "6":
                    reportName = "回扣费核销报表.grf";
                    return "QSP_GET_DiscountTransfer_Report";
                case "7":
                    reportName = "始发费核销报表.grf";
                    return "QSP_GET_OtherFee_SF_Verify_Report";
                case "8":
                    reportName = "终端费核销报表.grf";
                    return "QSP_GET_OtherFee_ZD_Verify_Report";
                case "9":
                    reportName = "送货费核销报表.grf";
                    return "QSP_GET_SendGoods_Verify_Report";
                case "10":
                    reportName = "短驳费核销报表.grf";
                    return "QSP_GET_ShortConn_Verify_Report";
                case "11":
                    reportName = "大车费核销报表.grf";
                    return "QSP_GET_Departure_Verify_Report";
                case "12":
                    reportName = "卸货费报销清单（按车）.grf";
                    return "QSP_GET_CARHANFEE_FOR_PRINT_REPORT";
                case "13":
                    reportName = "卸货费报销清单（单票）.grf";
                    return "QSP_GET_BILLHANFEE_FOR_PRINT_REPORT";
                case "14":
                    reportName = "现付审核报表.grf";
                    return "QSP_Get_WayBill_NOWPAYADUIT_Print";
                case "15":
                    reportName = "提付审核报表.grf";
                    return "QSP_Get_WayBill_FETCHPAYADUIT_Print";
                case "16":
                    reportName = "异动审核报表.grf";
                    return "QSP_Get_WayBill_TRANSPAYADUIT_Print";
                case "17":
                    reportName = "欠款审核报表.grf";
                    return "QSP_Get_WayBill_ArrConfirm_Print";
                case "18":
                    reportName = "终端中转应付日报表.grf";
                    return "QSP_GET_WAYBILLMIDDLE_CASHIER";
                case "19":
                    reportName = "营业改单日报表.grf";
                    return "QSP_GET_WAYBILLMODE_CASHIER";
                case "20":
                    reportName = "前台收银日报表.grf";
                    return "QSP_GET_WAYBILL_DailyInMoney";
                case "21":
                    reportName = "货物中转出库清单.grf";
                    return "QSP_GET_MIDDLEDAYREPORT";
                case "22":
                    reportName = "货款回收核销日报表.grf";
                    return "QSP_GET_CollectionPay_Report";

                default:
                    return "";
            }
        }
        /// <summary>
        /// 获取窗体名称
        /// </summary>
        /// <returns></returns>
        private string GetWinTitle()
        {
            string title = "打印清单--";
            switch (reportType)
            {
                case "1":
                    return title += "营业额日报表";
                case "2":
                    return title += "网点出库表";
                case "3":
                    return title += "账款确认报表";
                case "4":
                    return title += "派车费核销报表";
                case "5":
                    return title += "中转费核销报表";
                case "6":
                    return title += "回扣费核销报表";
                case "7":
                    return title += "始发费核销报表";
                case "8":
                    return title += "终端费核销报表";
                case "9":
                    return title += "送货费核销报表";
                case "10":
                    return title += "短驳费核销报表";
                case "11":
                    return title += "大车费核销报表";
                case "12":
                    return title += "单票卸货费报销报表";
                case "13":
                    return title += "整车卸货费报销报表";
                case "14":
                    return title += "现付审核报表";
                case "15":
                    return title += "提付审核报表";
                case "16":
                    return title += "异动审核报表";
                case "17":
                    return title += "欠款审核报表";
                case "18":
                    return title += "终端中转应付报表";
                case "19":
                    return title += "营业改单报表";
                case "20":
                    return title += "前台收银日报表";
                case "21":
                    return title += "货物中转出库清单";
                case "22":
                    return title += "货款回收核销日报表";
            }
            return title;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.PostColumnLayout();
                axGRDisplayViewer1.Report.PrintPreviewEx(GRPrintGenerateStyle.grpgsAll, true, true);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.PostColumnLayout();
                //if (axGRDisplayViewer1.Report.Printer.PrinterName != "" && frl.CheckPrinters(axGRDisplayViewer1.Report.Printer.PrinterName))
                //    axGRDisplayViewer1.Report.Print(false);
                //else
                axGRDisplayViewer1.Report.PrintEx(GRPrintGenerateStyle.grpgsAll, true);
                MsgBox.ShowOK("已打印");
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (groupControl2.Visible)
            {
                groupControl2.Visible = false;
            }
            else
            {
                groupControl2.Visible = true;
                groupControl2.Left = groupControl1.Width - groupControl2.Width;
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ds == null) return;
            if (ds.Tables[0].Rows.Count < 1) return;
            groupControl3.Visible = true;
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.Report.ExportDirect(GRExportType.gretXLS, (Report.Title + ".xls"), true, true);
            }
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bdate", bdate.DateTime));
            list.Add(new SqlPara("edate", edate.DateTime));
            list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
            list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
            list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
            list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
            if (reportType != "1" && reportType != "2" && reportType != "18" && reportType != "19" && reportType != "20") 
            {
                list.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
            }
            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("bdate", bdate.DateTime));
            list1.Add(new SqlPara("edate", edate.DateTime));
            list1.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
            list1.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
            list1.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
            list1.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
            list1.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
            if (reportType == "1" || reportType == "3" || reportType == "15" || reportType == "16" || reportType == "17") 
            {
                list1.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
            }

            ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, GetProc(), list));
            if (reportType == "14")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
            if (reportType == "15")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_FETCHPAYADUIT_Print_byTransferSite", list1));
            if (reportType == "16")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_TRANSPAYADUIT_Print_byTransferSite", list1));
            if (reportType == "17")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_ArrConfirm_Print_byTransferSite", list1));
            if (reportType == "1")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_DAYLYPAY_PRINT", list1));
            if (reportType == "2")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_WEBOUTBOUND_BY_TRANFERSITE", list1));
            if (reportType == "3")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_CASHIER_BY_STARTSITE", list1));
            if (reportType == "19")
                dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILLMODE_CASHIER_BY_STARTSITE", list1));
            if (reportType == "2") 
            {
                DataRow[] dr;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNoStr"]);

                    dr = ds.Tables[0].Select("BillNoStr='" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNoStr"])+"'");
                    if (dr.Length > 1)
                    {
                        for (int j = 1; j < dr.Length; j++)
                        {
                            if (ds.Tables[0].Columns.Contains("FetchPay")) dr[j]["FetchPay"] = DBNull.Value;
                            if (ds.Tables[0].Columns.Contains("NowPay")) dr[j]["NowPay"] = DBNull.Value;
                            if (ds.Tables[0].Columns.Contains("ShortOwePay")) dr[j]["ShortOwePay"] = DBNull.Value;
                            if (ds.Tables[0].Columns.Contains("MonthPay")) dr[j]["MonthPay"] = DBNull.Value;
                        }
                    }
                }
            }
            if (reportType == "15")
            {
                if (ds == null || ds.Tables.Count == 0) return;
                DataRow[] dr1;
                ///----ljp 修改为新的循环去除重复，由于数据里面重复数据删除导致行数变化，需要每次循环时检查行数是否等于循环数
                int index = 0;
                while (true)
                {
                    if (ds.Tables[0].Rows.Count <=index)
                    {
                        break;
                    }
                    string s = ConvertType.ToString(ds.Tables[0].Rows[index]["BillNo"]);

                    dr1 = ds.Tables[0].Select("BillNo= '" + s+"'");
                    if (dr1.Length > 1)
                    {
                        for (int j = 1; j < dr1.Length; j++)
                        {
                            ds.Tables[0].Rows.Remove(dr1[j]);
                        }
                    }
                    index++;

                }


                //for (int i = ds.Tables[0].Rows.Count - 1; i >= 0; i--)
                //{
                  
                //    string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]);

                //    dr1 = ds.Tables[0].Select("BillNo=" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNo"]));
                //    if (dr1.Length > 1)
                //    {
                //        for (int j = 1; j < dr1.Length; j++)
                //        {
                //            ds.Tables[0].Rows.Remove(dr1[j]);
                //            //if (ds.Tables[0].Columns.Contains("FetchPay")) dr[j]["FetchPay"] = DBNull.Value;
                //            //if (ds.Tables[0].Columns.Contains("NowPay")) dr[j]["NowPay"] = DBNull.Value;
                //            //if (ds.Tables[0].Columns.Contains("ShortOwePay")) dr[j]["ShortOwePay"] = DBNull.Value;
                //            //if (ds.Tables[0].Columns.Contains("MonthPay")) dr[j]["MonthPay"] = DBNull.Value;
                //        }
                //    }
                //}


            }
            getPrintUser();
            Init();//初始货
        }

        private void getPrintUser()
        {
            this.UserName.EditValueChanged -= new System.EventHandler(this.UserName_EditValueChanged);
            if (UserName.Text.Trim() != "全部")
            {
                this.UserName.EditValueChanged += new System.EventHandler(this.UserName_EditValueChanged);
                return;
            }
            
            if(getFieldName() == "")
            {
                UserName.Properties.Items.Clear();
                this.UserName.EditValueChanged += new System.EventHandler(this.UserName_EditValueChanged);
                return;
            }
            try
            {
                UserName.Properties.Items.Clear();
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if(UserName.Properties.Items.Contains(ds.Tables[0].Rows[i][getFieldName()])==false)
                    {
                        UserName.Properties.Items.Add(ds.Tables[0].Rows[i][getFieldName()]);
                    }
                }
                UserName.Properties.Items.Add("全部");
                this.UserName.EditValueChanged += new System.EventHandler(this.UserName_EditValueChanged);
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }
        private string getFieldName()
        {
            string title = "";
            switch (reportType)
            {
                case "1":
                    return title += "";//"营业额日报表";
                case "2":
                    return title += "";//"网点出库表";
                case "3":
                    return title += "ArrConfirmMan";
                case "4":
                    return title += "VerifMan";
                case "5":
                    return title += "OptMan";
                case "6":
                    return title += "OptMan";
                case "7":
                    return title += "AuditMan";
                case "8":
                    return title += "AuditMan";
                case "9":
                    return title += "OptMan";
                case "10":
                    return title += "OptMan";
                case "11":
                    return title += "OptMan";
                case "12":
                    return title += "";//"单票卸货费报销报表";
                case "13":
                    return title += "Discharger";//"整车卸货费报销报表";
                case "14":
                    return title += "AduitMan";
                case "15":
                    return title += "AduitMan";
                case "16":
                    return title += "AduitMan";
                case "17":
                    return title += "AduitMan";
                case "18":
                    return title += "";//"终端中转应付报表";
                case "19":
                    return title += "";//"营业改单报表";
                case "20":
                    return title += "BillMan";//"前台收银日报表";
                case "21":
                    return title += "";
                case "22":
                    return title += "OptMan";
            }
            return title;
        }
        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void axGRDisplayViewer1_ContentCellDblClick(object sender, Axgregn6Lib._IGRDisplayViewerEvents_ContentCellDblClickEvent e)
        {
            string[] sArray = ConvertType.ToString(Report.FieldByName("运单号").Value).Split('-');
            CommonClass.ShowBillSearch(sArray[0]);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 获取过程名称
        /// </summary>
        /// <returns></returns>
        private void setUserEnable() 
        {
            switch (reportType) 
            {
                case "1":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "2":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "3":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "4":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "5":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "6":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "7":
                    labUser.Text = "操作人";
                    UserName.Visible = true; ;
                    break;
                case "8":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "9":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "10":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "11":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "12":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "13":
                    labUser.Text = "卸货人";
                    UserName.Visible = true;
                    break;
                case "14":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "15":
                    labUser.Text = "操作人";
                    UserName.Visible = true
;
                    break;
                case "16":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "17":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "18":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "19":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "20":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "21":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "22":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
            }
            labUser.Location = new Point(label4.Location.X + label4.Size.Width - labUser.Size.Width, labUser.Location.Y);
        }

        private void WebName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(UserName,WebName.Text.Trim(),true);
        }

        private void UserName_EditValueChanged(object sender, EventArgs e)
        {
            simpleButton12_Click(null, null);
        }
    }
}