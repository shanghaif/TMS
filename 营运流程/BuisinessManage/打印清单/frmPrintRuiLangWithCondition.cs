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
    public partial class frmPrintRuiLangWithCondition : BaseForm
    {
        public frmPrintRuiLangWithCondition() { InitializeComponent(); }

        public frmPrintRuiLangWithCondition(string reportType)
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

         DataSet dsKid2;//子报表数据源2
        GridppReport ReportKid2 = new GridppReport();
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
                    t1.SetValue("ColumnName", c.Name);
                    t1.SetValue("tag", c.Name);
                    t1.Checked = c.Visible;
                    ColumnCount++;

                    try
                    {
                        #region..加载数据到筛选下拉框中..
                        cbcolumnName.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(c.Name, Report.FieldByName(c.Name).DBFieldName));
                        edtbcolumn.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(c.Name, Report.FieldByName(c.Name).DBFieldName));
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
                if (reportType == "69")
                {
                    ReportKid.LoadDataFromXML(dsKid.GetXml());
                }
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
            //记录界面点击次数xj/2019/5/29
            switch (reportType)
            {

                case "9":
                    CommonClass.InsertLog("送货费核销日报表");
                    break;
                case "5":
                    CommonClass.InsertLog("中转费核销日报表");
                    break;
                case "11":
                    CommonClass.InsertLog("大车费核销日报表");
                    break;
                case "7":
                    CommonClass.InsertLog("始发其它费核销报表");
                    break;
                case "8":
                    CommonClass.InsertLog("终端其它费核销报表");
                    break;
                case "10":
                    CommonClass.InsertLog("短驳费核销报表");
                    break;
                case "4":
                    CommonClass.InsertLog("派车费核销报表");
                    break;
                case "27":
                    CommonClass.InsertLog("转送费核销报表");
                    break;
                case "44":
                    CommonClass.InsertLog("现付核销报表");
                    break;
                case "45":
                    CommonClass.InsertLog("提付核销报表");
                    break;
                case "50":
                    CommonClass.InsertLog("欠付款核销报表");
                    break;
                case "51":
                    CommonClass.InsertLog("异动款核销报表");
                    break;
                case"52":
                    CommonClass.InsertLog("提付转欠核销报表");
                    break;
                case "53":
                    CommonClass.InsertLog("整车装卸费报表");
                    break;
                case "65":
                    CommonClass.InsertLog("垫付费核销日报表");
                    break;
                case"67":
                    CommonClass.InsertLog("车辆代扣款核销报表");
                    break;
                case"6":
                    CommonClass.InsertLog("回扣核销日报表");
                    break;
                case"22":
                    CommonClass.InsertLog("代收货款核销日报表");
                    break;
                case"66":
                    CommonClass.InsertLog("营业外收入审核报表");
                    break;
                case "69":
                    CommonClass.InsertLog("长途配载发运汇总表");
                    break;
                case"13":
                    CommonClass.InsertLog("单票卸货费报销清单");
                    break;
                case "12":
                    CommonClass.InsertLog("整车卸货费报销清单");
                    break;
                case "39":
                    CommonClass.InsertLog("其他费用报销清单");
                    break;
                case "30":
                    CommonClass.InsertLog("送货费返款审核报表");
                    break;
                case "40":
                    CommonClass.InsertLog("终端分拨费返款审核报表");
                    break;
                case "32":
                    CommonClass.InsertLog("调账审核报表");
                    break;
                case "29":
                    CommonClass.InsertLog("加盟结算收入日报表");
                    break;
                case "36":
                    CommonClass.InsertLog("付提付款日报表");
                    break;
                case "1":
                    CommonClass.InsertLog("营业额日报表");
                    break;
                case "2":
                    CommonClass.InsertLog("网点出库表");
                    break;
                case "18":
                    CommonClass.InsertLog("终端中转应付报表");
                    break;
                case "3":
                    CommonClass.InsertLog("账款收银确认报表");
                    break;
                case"19":
                    CommonClass.InsertLog("营业改单日报表");
                    break;
                case "21":
                    CommonClass.InsertLog("中转出库日报表");
                    break;
                case "55":
                    CommonClass.InsertLog("提付转欠款报表");
                    break;
                case "62":
                    CommonClass.InsertLog("干线毛利报表");
                    break;
                case"63":
                    CommonClass.InsertLog("操作毛利报表");
                    break;
                case "64":
                    CommonClass.InsertLog("终端毛利报表");
                    break;
                case "20":
                    CommonClass.InsertLog("前台收银合计");
                    break;
                default:
                    break;
            }
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar3);//如果有具体的工具条，就引用其实例

            if (reportType == "45") //ZJF20181124  提付核销增加按出库网点筛选条件
            {
                
                GetOutWeb(OutWeb);
                OutWeb.Text = "全部";
                WebName.Visible = false;
                label6.Visible = false;
            }
            if (reportType != "45")
            {
                label14.Visible = false;
                OutWeb.Visible = false;
                
            }
          
            dtsearch = frl.datatable();
            dtsearch.Rows.Clear();
            gridshow.DataSource = dtsearch;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            bdate2.DateTime = CommonClass.gbdate;
            edate2.DateTime = CommonClass.gedate;
            if (reportType == "1")
            {
                //485公司修改默认显示时间  zb20190603
                if (CommonClass.UserInfo.companyid == "485")
                {
                    bdate.DateTime = CommonClass.gbdate.AddHours(-24);
                    edate.DateTime = CommonClass.gedate.AddHours(-24);
                }
                else
                {
                    bdate.DateTime = CommonClass.gbdate.AddHours(-16);
                    edate.DateTime = CommonClass.gedate.AddHours(-16);
                }
                
            }
            if (reportType == "21")
            {
                bdate.DateTime = CommonClass.gbdate.AddHours(-6);
                edate.DateTime = CommonClass.gedate.AddHours(-6);
            }
            if (reportType == "2")
            {
                //DateTime bdt = CommonClass.gbdate;
                //bdt = bdt.AddDays(-2);
                //bdt = bdt.AddHours(18);
                //bdate.DateTime = bdt;
                //DateTime edt = CommonClass.gedate;
                //edt = edt.AddDays(-1);
                //edt = edt.AddHours(17 - edt.Hour);
                //edate.DateTime = edt;
                //485公司默认时间修改zb20190603
                if (CommonClass.UserInfo.companyid == "485")
                {
                    bdate.DateTime = CommonClass.gbdate.AddHours(-24);
                    edate.DateTime = CommonClass.gedate.AddHours(-24);
                }
                else
                {
                    bdate.DateTime = CommonClass.gbdate.AddHours(-6);
                    edate.DateTime = CommonClass.gedate.AddHours(-6);
                }
            
                if (CommonClass.UserInfo.companyid == "101" || CommonClass.UserInfo.companyid == "266" || CommonClass.UserInfo.companyid == "268" || CommonClass.UserInfo.companyid == "288")
                {
                    label11.Visible = true;
                    bdate2.Visible = true;
                    edate2.Visible = true;
                    label12.Visible = true;
                }
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
            condition.Properties.Items.Add("按\"并且\"过滤");  //zb20191011
            condition.Properties.Items.Add("按\"或者\"过滤");

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                //CauseName.Enabled = false;
            }

            if (reportType == "61" || reportType == "62" || reportType == "63" || reportType == "64")  //maohui20180814
            {
                label10.Visible = false;
                StartSite.Visible = false;
                label9.Visible = false;
                EndSiteName.Visible = false;
                labUser.Visible = false;
                UserName.Visible = false;
                if (CommonClass.UserInfo.companyid == "110" || CommonClass.UserInfo.companyid == "101")
                {
                    CauseName.Enabled = true;
                }
            }
            if (reportType == "63")   //maohui20180814
            {
                bdate.Properties.EditMask = "yyyy-MM-dd";
                edate.Properties.EditMask = "yyyy-MM-dd";
                bdate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";
                edate.Properties.DisplayFormat.FormatString = "yyyy-MM-dd";

            }
            //tuxin20181022
            if (UserName.Visible == true)
            {
                CommonClass.SetUser(UserName, WebName.Text.Trim(), true);
            }
        }
        //ZJF20181124
        public void GetOutWeb(DevExpress.XtraEditors.ComboBoxEdit OutWeb)
        {

            List<SqlPara> list = new List<SqlPara>();
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OutWeb", list));
            if (ds == null || ds.Tables.Count == 0) return;
            OutWeb.EditValue = "";
            OutWeb.Properties.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow dr = ds.Tables[0].Rows[i];
                OutWeb.Properties.Items.AddRange(new object[] { dr["OutWeb"] });

            }
            OutWeb.Properties.Items.AddRange(new object[] { "全部" });

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

            ds.Tables[0].Columns.Add("NowCompany");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["NowCompany"] = CommonClass.UserInfo.gsqc;
            }

            Report.LoadFromFile(reportpath);
            Report.LoadDataFromXML(ds.GetXml());
            if (reportType == "42" || reportType == "41" || reportType == "20" || reportType == "40" || reportType == "37" || reportType == "36" || reportType == "35" || reportType == "28" || reportType == "29" || reportType == "14" || reportType == "15" || reportType == "16" || reportType == "17" || reportType == "1" || reportType == "2" || reportType == "3" || reportType == "19" || reportType == "23" || reportType == "24" || reportType == "25" || reportType == "26" || reportType == "30" || reportType == "31" || reportType == "32" || reportType == "33" || reportType == "34" || reportType == "44" || reportType == "45" || reportType == "46" || reportType == "47" || reportType == "48" || reportType == "49" || reportType == "50" || reportType == "52"
                || reportType == "61" || reportType == "62" || reportType == "64" || reportType == "66" || reportType == "68" || reportType == "69")  //maohui20180819
            {
                ReportKid = Report.ControlByName("SubReport1").AsSubReport.Report;
                ReportKid.LoadDataFromXML(dsKid.GetXml());
            }
            if (reportType == "68")
            {
                ReportKid2 = Report.ControlByName("SubReport2").AsSubReport.Report;
                ReportKid2.LoadDataFromXML(dsKid2.GetXml());
            }
            try
            {
                //Report.ControlByName("txt").AsStaticBox.Text = "时间从 " + bdate.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "到" + edate.DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "　区间从 " + StartSite.Text + " 到 " + EndSiteName.Text + " 事业部:" + CauseName.Text + "　大区:" + AreaName.Text + "　网点:" + WebName.Text;
            }
            catch (Exception ex) { string str = ex.Message; }

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
            string filterType = "AND";
            try
            {
                int rows = gridView1.RowCount;
                if (rows < 1) return;
                string columnName = "", operators = "", value = "";
                string result = "";
                DataTable dt = ds.Tables[0].Clone();
                DataSet dsnew = new DataSet();
                axGRDisplayViewer1.Stop();
                if (condition.SelectedIndex==0)
                {
                    filterType = " AND ";
                }
                else
                {
                    filterType = " OR ";
                }


                for (int i = 0; i < rows; i++)
                {
                    columnName = gridView1.GetRowCellValue(i, "columnName").ToString();
                    operators = gridView1.GetRowCellValue(i, "operators").ToString();
                    value = gridView1.GetRowCellValue(i, "value").ToString();
                    if (columnName == "" || operators == "" || value == "") continue;
                    if (value == "" && !frl.GetStr(operators).Contains("null")) continue;
                    value = frl.GetStr(operators).Contains("like") ? "'%" + value + "%'" : (frl.GetStr(operators).Contains("null") ? "" : "'" + value + "'");
                    result += columnName + " " + frl.GetStr(operators) + value + filterType;
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
                    return "QSP_GET_CARHANFEE_FOR_PRINT_REPORT_KT";
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
                    reportName = "中转出库报表.grf";
                    //return "QSP_GET_MIDDLEDAYREPORT
                    return "QSP_GET_MIDDLEDAYREPORT_Line";
                case "22":
                    reportName = "货款回收核销日报表.grf";
                    return "QSP_GET_CollectionPay_Report";
                case "23":
                    reportName = "提现审核报表.grf";
                    return "QSP_GET_BILLWITHDRAWCASHAPPLY_PINT";
                case "24":
                    reportName = "充值审核报表.grf";
                    return "QSP_GET_BILLCHARGEAPPLY_ADUIT_PRINT";
                case "25":
                    reportName = "营业额日报表.grf";
                    return "QSP_GET_WAYBILL_DailySales_JM";
                case "26":
                    reportName = "网点出库表.grf";
                    return "QSP_GET_WAYBILL_WEBOUTBOUND_JM";
                case "27":
                    reportName = "转送费核销报表.grf";
                    return "QSP_GET_MiddlSendFee_Verify_Report";
                case "28":
                    reportName = "加盟结算收入汇总报表.grf";
                    return "QSP_GET_ACCTOTAL_report";
                case "29":
                    reportName = "加盟结算收入日报表.grf";
                    return "QSP_GET_ACCDAYLY_report";
                case "30":
                    reportName = "送货费返款审核报表.grf";
                    return "QSP_GET_DeliveryFee_Report";
                case "31":
                    reportName = "提付返款审核报表.grf";
                    return "QSP_GET_FetchFee_Aduit_Report";
                case "32":
                    reportName = "调账审核报表.grf";
                    return "QSP_GET_TIAOZHANG_Report";
                case "33":
                    reportName = "终端操作费审核报表.grf";
                    return "QSP_GET_TerminalOptFee_Report";
                case "34":
                    reportName = "终端分拨费审核报表.grf";
                    return "QSP_GET_ZDFBF_report";
                case "35":
                    reportName = "加盟改单调整报表.grf";
                    return "QSP_GET_ACCPAY_DETAIL_Report";
                case "36":
                    reportName = "付提付款日报表.grf";
                    return "QSP_GET_ACCFETCHPAY_DETAIL_Report";
                case "37":
                    reportName = "中转费返款审核报表.grf";
                    return "QSP_GET_TransferFee_report";
                case "38":
                    reportName = "加盟账户汇总日报表.grf";
                    return "QSP_GET_ACCCENTER_REPORT";
                case "39":
                    reportName = "其他费报销清单（单票）.grf";
                    return "QSP_GET_BillOhterFee_FOR_PRINT_REPORT";
                case "40":
                    reportName = "终端分拨费返款.grf";
                    return "QSP_GET_FenBoFee_Report";
                case "41":
                    reportName = "结算送货费调整审核报表.grf";
                    return "QSP_GET_Tz_DeliveryFee_Report";
                case "42":
                    reportName = "终端加盟提付审核报表.grf";
                    return "QSP_GET_DZFETCHPAY_Report";
                case "43":                                //毛慧20180103
                    reportName = "加盟单车毛利日报表.grf";
                    return "QSP_GET_GrossProfit_By_DEPARTURE";
                case "44":                                //lhd20180628
                    reportName = "现付核销报表.grf";
                    return "QSP_GETNOWPAY_InFee_Report";
                case "45":                                //lhd20180628
                    reportName = "提付核销报表.grf";
                    return "QSP_Get_WayBill_FETCHPAYADUIT_Print_Report_ZQ";
                //case "46":                                //lhd20180628
                //    reportName = "月结核销报表.grf";
                //    return "QSP_Get_WayBill_MONTHPAYADUIT_Print_Report";
                //case "47":                                //hj20180629
                //    reportName = "回单付核销报表.grf";
                //    return "QSP_Get_WayBill_ReceiptPay_Print";
                //case "48":                                //hj20180629
                //    reportName = "欠付核销报表.grf";
                //    return "QSP_Get_WayBill_OwePay_Print";
                //case "49":                                //hj20180629
                //    reportName = "货到前付核销报表.grf";
                //    return "QSP_Get_WayBill_BefArrivalPay_Print";
                case "50":                                //hj20180629
                    reportName = "欠付款核销报表.grf";
                    return "QSP_Get_WayBill_OwePay_Print_1"; //hj20180811
                case "51":                                //hj20180629
                    reportName = "异动款核销报表.grf";
                    return "QSP_Get_WayBill_FreightChanges_Print_1"; //hj20180811
                case "52":
                    reportName = "提付转欠核销报表.grf";
                    return "QSP_Get_WayBill_FetchToOwePay_Print_NEW";//hj20180811
                case "53":
                    reportName = "整车装卸费报表.grf";
                    return "QSP_GET_Departure_Verify_Print_KT";//hj20180811
                case "54":                                //hj20180629
                    reportName = "欠款确认报表.grf";
                    return "QSP_GET_OweMoney_CONFIRM_Line";
                case "55":                                //hj20180629
                    reportName = "提付转欠款报表.grf";
                    return "QSP_GET_BILLAPPLY_Line";
                case "56":                                //hj20180629
                    reportName = "回扣核销报表.grf";
                    return "QSP_GET_DiscountTransfer_Print";
                case "60":                                //hj20180629
                    reportName = "运单改动报表.grf";
                    return "QSP_GET_MODIFIED_WAYBILL_Print";//hj20180811
                case "61":
                    reportName = "营业毛利报表.grf";   //maohui20180814
                    return "QSP_GET_WAYBILL_Profit_Print";
                case "62":
                    reportName = "干线毛利报表.grf";  //maohui20180814
                    return "QSP_GET_BillDeparture_Profit_Print";
                case "63":
                    reportName = "操作毛利报表.grf";  //maohui20180816
                    return "QSP_GET_Operation_Profit_Print";
                case "64":
                    reportName = "终端毛利报表.grf";  //maohui20180816
                    return "QSP_GET_Terminal_Profit_Print";
                case "65":                                 //tuxin20181010
                    reportName = "垫付费核销报表.grf";
                    return "QSP_GET_MatPay_Report";
                case "66":
                    reportName = "营业外收入审核报表.grf";
                    return "QSP_Get_qt_income_Print";
                case "67":
                    reportName = "车辆代扣款核销报表.grf";
                    return "QSP_Get_vehicleWithold_Print";
                case "68":// hs 2018-11-14
                    reportName = "汽运成本申报汇总表.grf";
                    return "QSP_GET_FinancialAudit_Report";
                case "69"://
                    reportName = "长途配载发运汇总表.grf";
                    return "QSP_GET_LONGDepartureInfo";
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
                    return title += "整车卸货费报销报表";
                case "13":
                    return title += "单票卸货费报销报表";
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
                    //return title += "货物中转出库清单
                    return title += "中转出库报表";
                case "22":
                    return title += "货款回收核销日报表";
                case "23":
                    return title += "提现审核报表";
                case "24":
                    return title += "充值审核报表";
                case "25":
                    return title += "营业额日报表";
                case "26":
                    return title += "网点出库报表";
                case "27":
                    return title += "转送费核销报表";
                case "28":
                    return title += "加盟结算收入汇总报表";
                case "29":
                    return title += "加盟结算收入日报表";
                case "30":
                    return title += "送货费返款审核报表";
                case "31":
                    return title += "提付返款审核报表";
                case "32":
                    return title += "调整审核报表";
                case "33":
                    return title += "终端操作费审核报表";
                case "34":
                    return title += "终端分拨费审核报表";
                case "35":
                    return title += "加盟改单调整报表";
                case "36":
                    return title += "付提付款日报表";
                case "37":
                    return title += "中转费返款审核报表";
                case "38":
                    return title += "加盟账户汇总日报表";
                case "39":
                    return title += "其他非报销清单";
                case "40":
                    return title += "终端分拨费返款报表";
                case "41":
                    return title += "结算送货费调整审核报表";
                case "42":
                    return title += "终端加盟提付审核报表";
                case "43":                              //毛慧20180103
                    return title += "加盟单车毛利日报表";
                case "44":                              //lhd20180628
                    return title += "现付核销报表";
                case "45":                              //lhd20180628
                    return title += "提付核销报表";
                //case "46":                              //lhd20180628
                //    return title += "月结核销报表";
                //case "47":                              //hj20180629
                //    return title += "回单付核销报表";
                //case "48":                              //hj20180629
                //    return title += "欠付核销报表";
                //case "49":                              //hj20180629
                //    return title += "货到前付核销报表";
                case "50":
                    return title += "欠付款核销报表"; //hj20180811
                case "51":
                    return title += "异动款核销报表"; //hj20180812
                case "52":
                    return title += "提付转欠核销报表"; //hj20180812
                case "53":
                    return title += "整车装卸费报表"; //hj20180814
                case "54":                            
                    return title += "欠款确认报表";
                case "55":                              //hj20180629
                    return title += "提付转欠款报表";
                case "56":
                    return title += "回扣核销报表";//hj20180814
                case "60":
                    return title += "运单改动报表";//hj20180814
                case "61":
                    return title += "营业毛利报表";//maohui20180814
                case "62":
                    return title += "干线毛利报表";//maohui20180814
                case "63":
                    return title += "操作毛利报表";//maohui20180816
                case "64":
                    return title += "终端毛利报表";//maohui20180816
                case"65":
                    return title += "垫付费核销报表";//tuxin20181010
                case "66":
                    return title += "营业外收入审核报表";
                case "67":
                    return title += "车辆代扣款核销报表";
                case "68":
                    return title += "申报汇总表";
                case "69":
                    return title += "长途配载发运汇总表";
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
                try
                {
                    axGRDisplayViewer1.PostColumnLayout();
                    //if (axGRDisplayViewer1.Report.Printer.PrinterName != "" && frl.CheckPrinters(axGRDisplayViewer1.Report.Printer.PrinterName))
                    //    axGRDisplayViewer1.Report.Print(false);
                    //else
                    axGRDisplayViewer1.Report.PrintEx(GRPrintGenerateStyle.grpgsAll, true);
                    //MsgBox.ShowOK("已打印");
                }
                catch (System.Runtime.InteropServices.SEHException)
                {
                    MsgBox.ShowYesNo("外部组件发生异常!\r\n\r\n发生此问题，有两种可能：\r\n1、您的系统中没有安装打印机。解决方法：安装打印机驱动。\r\n2、打印服务没有开启。解决方法：按顺序打开“操作系统控制面板-管理工具-服务”，找到Print Spooler服务，将其启动，并设为自动启动。\r\n\r\n执行以上方法之后重新启动蓝桥物流平台!");
                }
                catch (Win32Exception ex)
                {
                    MsgBox.ShowError("检测到Win32异常：\r\n" + ex.Message);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
               
                if (reportType != "2" && reportType != "18" && reportType != "19" && reportType != "43")
                {
                    list.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
                }
                if (reportType == "45")
                {
                    list.Add(new SqlPara("OutWeb", OutWeb.Text.Trim() == "全部" ? "%%" : OutWeb.Text));    
                }
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("bdate", bdate.DateTime));
                list1.Add(new SqlPara("edate", edate.DateTime));
                list1.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list1.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list1.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list1.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list1.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
                if (reportType == "42" || reportType == "41" || reportType == "1" || reportType == "20" || reportType == "40" || reportType == "39" || reportType == "39" || reportType == "39"
                    || reportType == "37" || reportType == "36" || reportType == "35" || reportType == "27" || reportType == "12"
                    || reportType == "1" || reportType == "3" || reportType == "15" || reportType == "16" || reportType == "17" ||
                    reportType == "23" || reportType == "24" || reportType == "25" || reportType == "26" || reportType == "30" || reportType == "31"
                    || reportType == "32" || reportType == "33" || reportType == "34")
                {
                    list1.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
                }
                if (CommonClass.UserInfo.companyid == "101" || CommonClass.UserInfo.companyid == "266" || CommonClass.UserInfo.companyid == "268" || CommonClass.UserInfo.companyid == "288")
                {
                    if (reportType == "2")
                    {
                        list.Add(new SqlPara("bdate2", bdate2.DateTime));
                        list.Add(new SqlPara("edate2", edate2.DateTime));
                        list1.Add(new SqlPara("bdate2", bdate2.DateTime));
                        list1.Add(new SqlPara("edate2", edate2.DateTime));
                    }
                }
                if (reportType != "68")
                {
                    ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, GetProc(), list));
                }
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
                if (reportType == "23")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLWITHDRAWCASHAPPLY_PINT_BY_WEBNAME", list1));
                if (reportType == "24")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLCHARGEAPPLY_ADUIT_PRINT_BY_WEBNAME", list1));
                if (reportType == "25")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_DAYLYPAY_PRINT_JM", list1));
                if (reportType == "26")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_WEBOUTBOUND_BY_TRANFERSITE_JM", list1));
                if (reportType == "28")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ACCTOTAL_report_Transfersite", list1));
                if (reportType == "29")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ACCDAYLY_report_TransFerSite", list1));
                if (reportType == "30")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DeliveryFee_Report_Total", list1));
                if (reportType == "31")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FetchFee_Aduit_Report_Total", list1));
                if (reportType == "32")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TIAOZHANG_Report_Total", list1));
                if (reportType == "33")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TerminalOptFee_Report_Total", list1));
                if (reportType == "34")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ZDFBF_report_Total", list1));
                if (reportType == "35")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ACCPAY_DETAIL_Report_PAYSITe", list1));
                if (reportType == "36")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ACCFETCHPAY_DETAIL_Report_paysite", list1));
                if (reportType == "37")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TransferFee_report_Total", list1));
                if (reportType == "40")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FenBoFee_Report_Total", list1));
                if (reportType == "20")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_DailyInMoney_Total", list1));
                if (reportType == "41")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_Tz_DeliveryFee_Report_ToTal", list1));
                if (reportType == "42")
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DZFETCHPAY_Report_ToTal", list1));
                if (reportType == "61")   //maohui20180819
                {
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Report_ToTal", list1));
                }
                if (reportType == "62")   //maohui20180819
                {
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BillDeparture_Profit_Total", list1));
                }
                if (reportType == "64")  //maohui20180819
                {
                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_Terminal_Profit_Total", list1));
                }
                if (reportType == "44")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds != null || ds.Tables.Count != 0 || ds.Tables[0].Rows.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("SumNowPay", Type.GetType("System.String")));
                        List<string> list2 = new List<string>();
                        decimal SumNowPay = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            SumNowPay = 0;
                            if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim())) continue;
                            list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim());
                            DataRow dr = dt.NewRow();
                            dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim();
                            for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                            {
                                if (ds.Tables[0].Rows[s]["StartSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                                {
                                    SumNowPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["NowPay"]);
                                }
                            }
                            dr["SumNowPay"] = SumNowPay;
                            dt.Rows.Add(dr);
                        }
                        dsKid = new DataSet();
                        dsKid.Tables.Add(dt);
                    }

                }
                if (reportType == "45")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds != null || ds.Tables.Count != 0 || ds.Tables[0].Rows.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("SumFetchPay", Type.GetType("System.String")));
                        List<string> list2 = new List<string>();
                        decimal SumFetchPay = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            SumFetchPay = 0;
                            if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim())) continue;
                            list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim());
                            DataRow dr = dt.NewRow();
                            dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim();
                            for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                            {
                                if (ds.Tables[0].Rows[s]["TransferSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                                {
                                    SumFetchPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["FetchPay"]);
                                }
                            }
                            dr["SumFetchPay"] = SumFetchPay;
                            dt.Rows.Add(dr);
                        }
                        dsKid = new DataSet();
                        dsKid.Tables.Add(dt);
                    }

                }
                if (reportType == "46")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds != null || ds.Tables.Count != 0 || ds.Tables[0].Rows.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("SumMonthPay", Type.GetType("System.String")));
                        List<string> list2 = new List<string>();
                        decimal SumMonthPay = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            SumMonthPay = 0;
                            if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim())) continue;
                            list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim());
                            DataRow dr = dt.NewRow();
                            dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim();
                            for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                            {
                                if (ds.Tables[0].Rows[s]["StartSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                                {
                                    SumMonthPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["MonthPay"]);
                                }
                            }
                            dr["SumMonthPay"] = SumMonthPay;
                            dt.Rows.Add(dr);
                        }
                        dsKid = new DataSet();
                        dsKid.Tables.Add(dt);
                    }
                }
                //hj20180629
                if (reportType == "47")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumReceiptPay", Type.GetType("System.String")));
                    List<string> list2 = new List<string>();
                    decimal SumReceiptPay = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SumReceiptPay = 0;
                        if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim())) continue;
                        list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim());
                        DataRow dr = dt.NewRow();
                        dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim();
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            if (ds.Tables[0].Rows[s]["StartSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                            {
                                SumReceiptPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["ReceiptPay"]);
                            }
                        }
                        dr["SumReceiptPay"] = SumReceiptPay;
                        dt.Rows.Add(dr);
                    }
                    dsKid = new DataSet();
                    dsKid.Tables.Add(dt);

                }
                //2018 11-30 hs
                if (reportType == "69")
                {
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("loadWeb", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("loadAmount", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccLine", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccBigCarFecth", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccBigCarSend", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("otherFee", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccBigcarTotal", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("NowPayDriver", Type.GetType("System.String")));

                    dt.Columns.Add(new DataColumn("OilCardFee", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccCollectPremium", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("AccTakeCar", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("DriverTakePay", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("backPay", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("BackPayDriver", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("maoli", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("chengbenRate", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumMoney", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumChengbenRate", Type.GetType("System.String")));


                    List<string> list2 = new List<string>();
                    decimal loadAmount = 0, AccLine = 0, AccBigCarFecth = 0, AccBigCarSend = 0, otherFee = 0, AccBigcarTotal = 0, NowPayDriver = 0
                        , OilCardFee = 0, AccCollectPremium = 0, AccTakeCar = 0, DriverTakePay = 0, backPay = 0, BackPayDriver = 0, maoli = 0, chengbenRate = 0, sumMoney = 0
                        , sumLoadAmount = 0, sumAccBigcar = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["LoadWeb"]).Trim())) continue;
                        list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["LoadWeb"]).Trim());
                        DataRow dr = dt.NewRow();
                        dr["LoadWeb"] = ConvertType.ToString(ds.Tables[0].Rows[i]["LoadWeb"]).Trim();
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            sumLoadAmount += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccPZ"]);
                            sumAccBigcar += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccBigcarTotal"]);
                            if (ds.Tables[0].Rows[s]["LoadWeb"].ToString().Trim() == dr["LoadWeb"].ToString().Trim())
                            {
                                loadAmount += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccPZ"]);
                                AccLine += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccLine"]);
                                AccBigCarFecth += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccBigCarFecth"]);
                                AccBigCarSend += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccBigCarSend"]);
                                otherFee += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["otherFee"]);
                                AccBigcarTotal += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccBigcarTotal"]);
                                NowPayDriver += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["NowPayDriver"]);
                                OilCardFee += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["OilCardFee"]);
                                AccCollectPremium += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccCollectPremium"]);
                                AccTakeCar += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["AccTakeCar"]);
                                DriverTakePay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["DriverTakePay"]);
                                backPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["backPay"]);
                                BackPayDriver += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["BackPayDriver"]);
                                maoli += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["maoli"]);
                                //chengbenRate += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["chengbenRate"]);
                                if (loadAmount == 0)
                                {
                                    chengbenRate = 0;
                                }
                                else
                                {
                                    chengbenRate = AccBigcarTotal / loadAmount;
                                }
                            }
                        }
                        sumMoney = loadAmount + AccLine + AccBigCarFecth + AccBigCarSend + otherFee + AccBigcarTotal + NowPayDriver + OilCardFee + AccCollectPremium + AccTakeCar
                            + DriverTakePay + backPay + BackPayDriver + maoli + chengbenRate;
                        dr["loadAmount"] = loadAmount;
                        dr["AccLine"] = AccLine;
                        dr["AccBigCarFecth"] = AccBigCarFecth;
                        dr["AccBigCarSend"] = AccBigCarSend;
                        dr["otherFee"] = otherFee;
                        dr["AccBigcarTotal"] = AccBigcarTotal;
                        dr["NowPayDriver"] = NowPayDriver;
                        dr["OilCardFee"] = OilCardFee;
                        dr["AccCollectPremium"] = AccCollectPremium;
                        dr["AccTakeCar"] = AccTakeCar;
                        dr["DriverTakePay"] = DriverTakePay;
                        dr["backPay"] = backPay;
                        dr["BackPayDriver"] = BackPayDriver;
                        dr["maoli"] = maoli;
                        dr["chengbenRate"] = chengbenRate;
                        dr["SumMoney"] = sumMoney;
                        dr["SumChengbenRate"] = sumAccBigcar / sumLoadAmount;
                        dt.Rows.Add(dr);
                        loadAmount = 0; AccLine = 0; AccBigCarFecth = 0; AccBigCarSend = 0; otherFee = 0; sumMoney = 0;

                        AccBigcarTotal = 0; NowPayDriver = 0; OilCardFee = 0; AccCollectPremium = 0; AccTakeCar = 0; DriverTakePay = 0; backPay = 0; BackPayDriver = 0; maoli = 0; chengbenRate = 0;
                    }
                    dsKid = new DataSet();
                    dsKid.Tables.Add(dt);

                }
                //hj20180629
                if (reportType == "48")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumOwePay", Type.GetType("System.String")));
                    List<string> list2 = new List<string>();
                    decimal SumOwePay = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SumOwePay = 0;
                        if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim())) continue;
                        list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim());
                        DataRow dr = dt.NewRow();
                        dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim();
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            if (ds.Tables[0].Rows[s]["StartSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                            {
                                SumOwePay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["OwePay"]);
                            }
                        }
                        dr["SumOwePay"] = SumOwePay;
                        dt.Rows.Add(dr);
                    }
                    dsKid = new DataSet();
                    dsKid.Tables.Add(dt);

                }
                //hj20180629
                if (reportType == "49")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumBefArrivalPay", Type.GetType("System.String")));
                    List<string> list2 = new List<string>();
                    decimal SumBefArrivalPay = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SumBefArrivalPay = 0;
                        if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim())) continue;
                        list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim());
                        DataRow dr = dt.NewRow();
                        dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["StartSite"]).Trim();
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            if (ds.Tables[0].Rows[s]["StartSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                            {
                                SumBefArrivalPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["BefArrivalPay"]);
                            }
                        }
                        dr["SumBefArrivalPay"] = SumBefArrivalPay;
                        dt.Rows.Add(dr);
                    }
                    dsKid = new DataSet();
                    dsKid.Tables.Add(dt);
                }

                //hj20180629
                if (reportType == "50" || reportType == "52")
                {
                    //dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_NOWPAYADUIT_Print_byTransferSite", list1));
                    if (ds != null || ds.Tables.Count != 0 || ds.Tables[0].Rows.Count != 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add(new DataColumn("TransferSite", Type.GetType("System.String")));
                        dt.Columns.Add(new DataColumn("SumPay", Type.GetType("System.String")));
                        List<string> list2 = new List<string>();
                        decimal SumPay = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            SumPay = 0;
                            if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim())) continue;
                            list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim());
                            DataRow dr = dt.NewRow();
                            dr["TransferSite"] = ConvertType.ToString(ds.Tables[0].Rows[i]["TransferSite"]).Trim();
                            for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                            {
                                if (ds.Tables[0].Rows[s]["TransferSite"].ToString().Trim() == dr["TransferSite"].ToString().Trim())
                                {
                                    SumPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["SumPay"]);
                                }
                            }
                            dr["SumPay"] = SumPay;
                            dt.Rows.Add(dr);
                        }
                        dsKid = new DataSet();
                        dsKid.Tables.Add(dt);
                    }

                }
                if (reportType == "68")//hs 2018-11-14
                {
                    List<SqlPara> list2 = new List<SqlPara>();
                    List<SqlPara> list3 = new List<SqlPara>();
                    List<SqlPara> list4 = new List<SqlPara>();
                    list2.Add(new SqlPara("bdate", bdate.DateTime));
                    list2.Add(new SqlPara("edate", edate.DateTime));

                    list2.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text.Trim()));
                    list2.ForEach(i => list3.Add(i));
                    list2.ForEach(i => list4.Add(i));
                    ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, GetProc(), list2));

                    dsKid = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_GasStation_Report", list3));
                    dsKid2 = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ZJFee_Report", list4));


                    //dsKid = new DataSet();
                    //dsKid.Tables.Add(dt);
                }
                if (reportType == "66")
                {
                    //if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    DataTable dt = new DataTable();
                    dt.Columns.Add(new DataColumn("entryname", Type.GetType("System.String")));
                    dt.Columns.Add(new DataColumn("SumPayVerif", Type.GetType("System.String")));
                    List<string> list2 = new List<string>();
                    decimal SumNowPay = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SumNowPay = 0;
                        if (list2.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["entryname"]).Trim())) continue;
                        list2.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["entryname"]).Trim());
                        DataRow dr = dt.NewRow();
                        dr["entryname"] = ConvertType.ToString(ds.Tables[0].Rows[i]["entryname"]).Trim();
                        for (int s = 0; s < ds.Tables[0].Rows.Count; s++)
                        {
                            if (ds.Tables[0].Rows[s]["entryname"].ToString().Trim() == dr["entryname"].ToString().Trim())
                            {
                                SumNowPay += ConvertType.ToDecimal(ds.Tables[0].Rows[s]["PayVerif"]);
                            }
                        }
                        dr["SumPayVerif"] = SumNowPay;
                        dt.Rows.Add(dr);
                    }
                    dsKid = new DataSet();
                    dsKid.Tables.Add(dt);
                }

                if (reportType == "2")
                {
                    DataRow[] dr;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string s = ConvertType.ToString(ds.Tables[0].Rows[i]["BillNoStr"]);

                        dr = ds.Tables[0].Select("BillNoStr='" + ConvertType.ToString(ds.Tables[0].Rows[i]["BillNoStr"]) + "'");
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
                        if (ds.Tables[0].Rows.Count <= index)
                        {
                            break;
                        }
                        string s = ConvertType.ToString(ds.Tables[0].Rows[index]["BillNo"]);

                        dr1 = ds.Tables[0].Select("BillNo= '" + s + "'");
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
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private void getPrintUser()
        {
            this.UserName.EditValueChanged -= new System.EventHandler(this.UserName_EditValueChanged);
            if (UserName.Text.Trim() != "全部")
            {
                this.UserName.EditValueChanged += new System.EventHandler(this.UserName_EditValueChanged);
                return;
            }

            string strFileName = getFieldName();
            if (strFileName == "")
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
                    if (ds.Tables[0].Columns.Contains(strFileName) && UserName.Properties.Items.Contains(ds.Tables[0].Rows[i][strFileName]) == false)
                    {
                        UserName.Properties.Items.Add(ds.Tables[0].Rows[i][strFileName]);
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
                    return title += "BillMan";//"营业额日报表";
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
                    return title += "Discharger";//"单票卸货费报销报表";
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
                case "23":
                    return title += "AuditingMan";
                case "24":
                    return title += "AuditingMan";
                case "25":
                    return title += "";//"营业额日报表";
                case "26":
                    return title += "";//"网点出库表";
                case "27":
                    return title += "OptMan";//"网点出库表"
                case "28":
                    return title += "";//"网点出库表"
                case "29":
                    return title += "";//"网点出库表"
                case "30":
                    return title += "OptMan";//"网点出库表"
                case "31":
                    return title += "OptMan";//"网点出库表"
                case "32":
                    return title += "OptMan";//"网点出库表"
                case "33":
                    return title += "OptMan";//"网点出库表"
                case "34":
                    return title += "OptMan";//"网点出库表"
                case "35":
                    return title += "OptMan";//"网点出库表"
                case "36":
                    return title += "OptMan";//"网点出库表"
                case "37":
                    return title += "OptMan";//"网点出库表"
                case "38":
                    return title += "";//""
                case "39":
                    return title += "Project";//""
                case "40":
                    return title += "OptMan";//""
                case "41":
                    return title += "DeliveryFee_CMan";//""
                case "42":
                    return title += "ZDFetchPayVerifMan";//""

                case "44":                                  
                    return title += "OptMan";  //zhengjiafeng20180727
                case "45":
                    return title += "AduitMan";
                case "46":
                    return title += "AduitMan";
                case "50":
                    return title += "OptMan";
                case "51":
                    return title += "OptMan";
                case "52":
                    return title += "OptMan";
                case "53":
                    return title += "OptMan";
                case "55":
                    return title += "OptMan";
                case "66":
                    return title += "AduitMan";
                case "67":
                    return title += "AduitMan";
                case "69":
                    return title += "VerifyMan";
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
            frmBillSearchControl.ShowBillSearch(sArray[0]);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 设置是否显示用户名框
        /// </summary>
        /// <returns></returns>
        private void setUserEnable()
        {
            switch (reportType)
            {
                case "1":
                    labUser.Text = "制单人";
                    UserName.Visible = true;
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
                    labUser.Text = "卸货人";
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
                    labUser.Text = "收银员";
                    UserName.Visible = true;
                    break;
                case "21":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "22":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "23":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "24":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "25":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "26":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "27":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "28":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "29":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "30":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "31":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "32":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "33":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "34":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "35":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "36":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "37":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "38":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "39":
                    labUser.Text = "费用类型";
                    UserName.Visible = true;
                    break;
                case "40":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "41":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "42":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "43":                 //毛慧20180104
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "44":                 //lhd20180628
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "45":                 //lhd20180628
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "46":                 //lhd20180628
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "50":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "51":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "52":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "53":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "54":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "55":
                    labUser.Text = "";
                    UserName.Visible = false;
                    break;
                case "56":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "65":
                    labUser.Text = "操作人";
                    UserName.Visible = true;
                    break;
                case "66":
                    labUser.Text = "登记人";
                    UserName.Visible = true;
                    StartSite.Visible = false;
                    EndSiteName.Visible = false;
                    label10.Visible = false;
                    label9.Visible = false;
                    break;
                case "67":
                    labUser.Text = "登记人";
                    UserName.Visible = true;
                    StartSite.Visible = false;
                    EndSiteName.Visible = false;
                    label10.Visible = false;
                    label9.Visible = false;
                    break;
                case "68":
                    labUser.Text = "审核人";
                    UserName.Visible = true;
                    StartSite.Visible = false;
                    EndSiteName.Visible = false;
                    label10.Visible = false;
                    label9.Visible = false;
                    break;
                case "69":
                    labUser.Text = "审核人";
                    UserName.Visible = true;
                    StartSite.Visible = false;
                    EndSiteName.Visible = false;
                    label10.Visible = false;
                    label9.Visible = false;
                    break;
            }
           // labUser.Location = new Point(label4.Location.X + label4.Size.Width - labUser.Size.Width, labUser.Location.Y);
        }

        private void WebName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(UserName, WebName.Text.Trim(), true);
        }

        private void UserName_EditValueChanged(object sender, EventArgs e)
        {
            simpleButton12_Click(null, null);
        }

        private void UserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportType == "44")
            {
                ds.Clear();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
                list.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GETNOWPAY_InFee_Report", list);
                ds = SqlHelper.GetDataSet(spe);
            }

            if(reportType == "45")
            {
                 ds.Clear();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
                list.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_FETCHPAYADUIT_Print_Report_ZQ", list);
                ds = SqlHelper.GetDataSet(spe);
            }

            if (reportType == "46")
            {
                ds.Clear();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text));
                list.Add(new SqlPara("esite", EndSiteName.Text.Trim() == "全部" ? "%%" : EndSiteName.Text));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text));
                list.Add(new SqlPara("UserName", UserName.Text.Trim() == "全部" ? "%%" : UserName.Text));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_MONTHPAYADUIT_Print_Report", list);
                ds = SqlHelper.GetDataSet(spe);
            }

            }

        }
    }
