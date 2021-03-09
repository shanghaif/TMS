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

namespace ZQTMS.Tool
{
    public partial class frmPrintRuiLang : BaseForm
    {
        public frmPrintRuiLang(string reportName, DataTable dt)
        {
            InitializeComponent();
            this.reportName = (reportName.EndsWith(".grf") ? reportName : reportName + ".grf");
            ds = new DataSet();
            if (dt != null) ds.Tables.Add(dt.Copy());
        }

        public frmPrintRuiLang(string reportName, DataSet printds)
        {
            InitializeComponent();
            this.reportName = (reportName.EndsWith(".grf") ? reportName : reportName + ".grf");
            ds = printds;
        }

        public frmPrintRuiLang(string reportName, DataSet printds, DataSet printSubDs)
        {
            InitializeComponent();
            this.reportName = (reportName.EndsWith(".grf") ? reportName : reportName + ".grf");
            this.ds = printds;
            this.dsSub = printSubDs;
        }

        public frmPrintRuiLang(string reportName, DataSet printds, string companyName)
        {
            InitializeComponent();
            this.reportName = (reportName.EndsWith(".grf") ? reportName : reportName + ".grf");
            this.reportStrName = reportName;
            ds = printds;
            this.strCompanyName = companyName;
        }

        int ColumnCount = 0;
        GridppReport Report = new GridppReport();
        DataSet ds, dsSub;//数据源、子报表
        DataTable dtsearch;
        string reportName = "";//报表文件名称
        string reportStrName = "";//报表文件名称
        frmRuiLangService frl = new frmRuiLangService();
        string strCompanyName = string.Empty;//公司名称

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
                        edtbcolumn1.Properties.Items.Add(new DevExpress.XtraEditors.Controls.ImageComboBoxItem(c.Name, c.Name));//zaj

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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.Report.ExportDirect(GRExportType.gretXLS, (Report.Title + ".xls"), true, true);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //ReStart();//zaj 20180713
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.PostColumnLayout();
                if (axGRDisplayViewer1.Report.Printer.PrinterName != "" && frl.CheckPrinters(axGRDisplayViewer1.Report.Printer.PrinterName))
                    axGRDisplayViewer1.Report.Print(false);
                else
                    axGRDisplayViewer1.Report.PrintEx(GRPrintGenerateStyle.grpgsAll, true);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //ReStart();//zaj20180713
            if (axGRDisplayViewer1.Report != null)
            {
                axGRDisplayViewer1.PostColumnLayout();
                axGRDisplayViewer1.Report.PrintPreviewEx(GRPrintGenerateStyle.grpgsAll, true, true);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
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

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            groupControl2.Visible = false;
        }

        private void w_report_detail_Load(object sender, EventArgs e)
        {
            groupControl4.Visible = false;
            //加载锐浪报表文件
            if (!LoadReportFile())
            {
                this.Close();
                return;
            }
            try
            {
                LoadColumn();
                dtsearch = frl.datatable();
                dtsearch.Rows.Clear();
                gridshow.DataSource = dtsearch;

                // Report.LoadDataFromXML(ds.GetXml());很多地方传入的Report都已经加载数据了，在这里再次加载的话会重复
                axGRDisplayViewer1.Report = Report;
                axGRDisplayViewer1.Start();
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

            Report.LoadFromFile(reportpath);
            Report.LoadDataFromXML(ds.GetXml());
            Report.Printer.PrinterName = frmRuiLangService.GetPrinterName(reportName);
            return true;
        }

        private void cbClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
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

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            if (ds == null) return;
            if (ds.Tables[0].Rows.Count < 1) return;
            groupControl3.Visible = true;
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

        private void simpleButton5_Click_1(object sender, EventArgs e)
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

        private void simpleButton12_Click(object sender, EventArgs e)
        {

            string name = "";//reportName.EndsWith(".grf") ? reportName.Substring(0, reportName.IndexOf('.')) + "per.grf" : reportName + "per.grf"
            if (reportName.EndsWith(".grf"))
            {
                name = reportName.Substring(0, reportName.IndexOf('.'));
                if (!name.EndsWith("per"))
                {
                    name = name + "per.grf";
                }
                else
                {
                    name = name + ".grf";
                }
            }
            else
            {
                if (!name.EndsWith("per"))
                {
                    name = name + "per.grf";
                }
                else
                {
                    name = name + ".grf";
                }
            }
            string fileName = Application.StartupPath + "\\Reports\\"+name;
            if (!File.Exists(fileName))
            {
                //File.Create(fileName);
            }
            // Report.DetailGrid.Columns["运单号"].Width = 44;
            //Report.DetailGrid.Columns.ItemAt(3).Width = 7;
            Report.SaveToFile(fileName);
            MsgBox.ShowOK("报表外观保存成功！");
        }

        private void btnSetWith_Click(object sender, EventArgs e)
        {
            if (groupControl4.Visible)
            {
                groupControl4.Visible = false;
            }
            else
            {
                groupControl4.Visible = true;
                //groupControl2.Left = groupControl1.Width - groupControl2.Width;
            }
        }

        //zaj 设置列宽 20180713
        private void simpleButton13_Click(object sender, EventArgs e)
        {
            Report.DetailGrid.Columns[edtbcolumn1.Value.ToString()].Width = Convert.ToDouble(txtWidth.Text.ToString().Trim());
            ReStart();
        }

        private void edtbcolumn1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtWidth.Text = "";
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            groupControl4.Visible = false;
        }
    }
}
