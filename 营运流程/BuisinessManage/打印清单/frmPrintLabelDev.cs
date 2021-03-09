using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;
using DevExpress.XtraEditors;
using System.Drawing.Printing;
using System.Collections;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.IO;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmPrintLabelDev : BaseForm
    {
        frmRuiLangService frls = new frmRuiLangService();
        public XtraReport rpt = new XtraReport();
        string fileName = "";
        DataTable DtBillNo = new DataTable();

        public frmPrintLabelDev()
        {
            InitializeComponent();
        }

        public frmPrintLabelDev(DataTable DtBillNo)
        {
            InitializeComponent();
            this.DtBillNo = DtBillNo;
        }

        private void w_print_lavel_Load(object sender, EventArgs e)
        {
            if (DtBillNo == null || DtBillNo.Rows.Count == 0)
            {
                simpleButton1.Enabled = false;
                return;
            }
            lblBillNo.Text = ConvertType.ToString(DtBillNo.Rows[0]["BillNo"]);
              
            //fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelFileName;
            fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelNameNew;//zaj 可以给每一个公司设置自己的标签名 2017-1-4
            if (CommonClass.GetLabelNameNew == "川胜标签.repx")//当485公司时才获取客服电话的值
            {
                 List<SqlPara> list = new List<SqlPara>();
                 SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WebServiceTel", list);
                 DataTable  dt =SqlHelper.GetDataTable(sps) ;
                 if (dt!= null && dt.Rows.Count > 0)
                 {
                     DtBillNo.Rows[0]["BegWebTel"] = dt.Rows[0][0];
                 }
            }
            
            try
            {
                //fileName = Application.StartupPath + "\\Reports\\" + "同星标签.repx"; //所有公司公用一个模版打印
                edprinters.Properties.Items.Clear();
                ArrayList al = new ArrayList();
                frls.GetPrinters(al);
                for (int i = 0; i < al.Count; i++)
                {
                    edprinters.Properties.Items.Add(al[i]);
                }
                if (!File.Exists(fileName))
                {
                    XtraMessageBox.Show("标签文件不存在,无法打印标签!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    simpleButton1.Enabled = false;
                    return;
                }

                if (rpt.Report.Name == "")
                {
                    rpt.LoadLayout(fileName);
                }

                string printer = API.ReadINI("RuiLangPrint", "PrintLabel", "", frmRuiLangService.configFileName);
                if (al.Contains(printer))
                {
                    edprinters.Text = printer;
                }

                int Num = ConvertType.ToInt32(DtBillNo.Rows[0]["Num"]);
                spinEdit1.Properties.MaxValue = Num;
                spinEdit1.Properties.MinValue = 1;
                spinEdit1.Text = Num.ToString();

                spinEdit2.EditValue = 1;
                spinEdit2.Properties.MaxValue = Num;
                spinEdit2.Properties.MinValue = 1;

                spinEdit3.EditValue = 1;
                spinEdit3.Properties.MaxValue = Num;
                spinEdit3.Properties.MinValue = 1;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void print()
        {
            DtBillNo.Rows[0]["Num"] = DtBillNo.Rows[0]["Num"] + "件";
            DtBillNo.Rows[0]["TotalNum"] = "共" + DtBillNo.Rows[0]["TotalNum"] + "件";  //总件：第几件
            DtBillNo.Rows[0]["W_V"] = ConvertType.ToDecimal(DtBillNo.Rows[0]["FeeWeight"], "0") + "/" + ConvertType.ToDecimal(DtBillNo.Rows[0]["FeeVolume"], "0");
            int labelSeq = 0;
            if (DtBillNo.Columns.Contains("BillNo")) labelSeq = ConvertType.ToInt32(DtBillNo.Rows[0]["LabelSeq"]);
            try
            {
                #region 绑定条码控件
                for (int i = 0; i < rpt.Bands.Count; i++)
                {
                    if (rpt.Bands[i].GetType() == typeof(DetailBand))
                    {
                        for (int j = 0; j < rpt.Bands[i].Controls.Count; j++)
                        {
                            XRControl con = rpt.Bands[i].Controls[j];
                            if (con.GetType() == typeof(XRRichText) || con.GetType() == typeof(XRCheckBox) || con.GetType() == typeof(XRBarCode) || con.GetType() == typeof(XRZipCode))
                            {
                                for (int y = 0; y < DtBillNo.Columns.Count; y++)
                                {
                                    if (con.Name == DtBillNo.Columns[y].ColumnName || (DtBillNo.Columns[y].ColumnName == "code" && con.Name.Contains("code")))
                                    {
                                        con.DataBindings.Add("Text", DtBillNo, DtBillNo.Columns[y].ColumnName);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }

                // 获取qty列的索引
                //List<int> unitIndex = new List<int>();
                //for (int i = 0; i < DtBillNo.Columns.Count; i++)
                //{
                //    if (DtBillNo.Columns[i].ColumnName.Contains("条码") && DtBillNo.Columns[i].ColumnName.Contains("运单"))
                //    {
                //        unitIndex.Add(i);// 找到条码(运单号)列的索引
                //    }
                //}
                #endregion

                //int NumSub = int.Parse(spinEdit1.EditValue.ToString());
                //for (int i = 1; i <= DtBillNo.Rows.Count; i++)
                //{
                //    DtBillNo.Rows[0]["NumSub"] = "第" + (i) + "件";  //总件：第几件
                //}
                
                if (radioGroup1.SelectedIndex == 0)
                {
                    object[] dr = DtBillNo.Rows[0].ItemArray;
                    for (int i = 0; i < int.Parse(spinEdit1.EditValue.ToString()) - 1; i++)
                    {
                        DtBillNo.Rows.Add(dr);
                        DtBillNo.Rows[i]["NumSub"] = "第" + (i + 1) + "件";  //总件：第几件
                        if (i + 2 == int.Parse(spinEdit1.EditValue.ToString())) DtBillNo.Rows[i + 1]["NumSub"] = "第" + spinEdit1.EditValue.ToString() + "件";
                    }
                    for (int j = 0; j < DtBillNo.Columns.Count; j++)
                    {
                        if (DtBillNo.Columns[j].ColumnName.Contains("code") || DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                        {
                            for (int i = 0; i < DtBillNo.Rows.Count; i++)
                            {
                                if (DtBillNo.Columns[j].ColumnName.Contains("code"))
                                    DtBillNo.Rows[i][j] = ConvertType.ToString(DtBillNo.Rows[i]["BillNo"]) + "-" + (labelSeq + i + 1); //打印条码
                                if (DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                                   DtBillNo.Rows[i][j] = "第" + (i + 1) + "件"; 
                            }
                        }
                        
                    }
                }
                else
                {
                    int start = int.Parse(spinEdit2.EditValue.ToString());
                    int end = int.Parse(spinEdit3.EditValue.ToString());
                    object[] dr = DtBillNo.Rows[0].ItemArray;
                    for (int i = start; i < end; i++)
                    {
                        DtBillNo.Rows.Add(dr);
                    }

                    for (int j = 0; j < DtBillNo.Columns.Count; j++)
                    {
                        if (DtBillNo.Columns[j].ColumnName.Contains("code") || DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                        {
                            for (int i = 0; i < DtBillNo.Rows.Count; i++)
                            {
                                if (DtBillNo.Columns[j].ColumnName.Contains("code"))
                                    DtBillNo.Rows[i][j] = ConvertType.ToString(DtBillNo.Rows[i]["BillNo"]) + "-" + (labelSeq + start + i);
                                if (DtBillNo.Columns[j].ColumnName.Contains("rowid"))
                                    DtBillNo.Rows[i][j] = "第" + (labelSeq + start + i) + "件";
                            }
                        }
                    }
                }

                rpt.ShowPrintStatusDialog = false; //显示打印状态框
                rpt.ShowPrintMarginsWarning = false;
                rpt.DataSource = DtBillNo;
                rpt.DataMember = DtBillNo.TableName;
                printControl1.PrintingSystem = rpt.PrintingSystem;
                rpt.CreateDocument();
                printControl1.UpdatePageView();

                if (printControl1.PrintingSystem == null)
                {
                    return;
                }
                printControl1.PrintingSystem.Document.Name = "中强集团-标签打印";
                printControl1.PrintingSystem.PageSettings.PrinterName = edprinters.Text.Trim();
                printControl1.PrintingSystem.Print();
                //rpt.ExportToXls("d:\\1.xls");
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (edprinters.Text.Trim() == "")
                {
                    XtraMessageBox.Show("请选择打印机!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    edprinters.Focus();
                    return;
                }

                if (radioGroup1.SelectedIndex == 1 && int.Parse(spinEdit2.EditValue.ToString()) > int.Parse(spinEdit3.EditValue.ToString()))
                {
                    MsgBox.ShowOK("起止序号选择错误：\r\n截止序号必须大于起始序号!");
                    spinEdit3.Focus();
                }

                //保存标签序号
                if (DtBillNo.Columns.Contains("LabelSeq")) //ljp 2017-03-03去除为空判断&& ConvertType.ToString(DtBillNo.Rows[0]["LabelSeq"])!=""
                {
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BILL_LABELSEQ", new List<SqlPara> { new SqlPara("BillNo", DtBillNo.Rows[0]["BillNo"]), new SqlPara("LabelSeq", ConvertType.ToInt32(spinEdit1.Text)) })) == 0)
                    {
                        MsgBox.ShowOK("标签序号生成失败，请重试！", "系统提示");
                        return;
                    }
                }

                print();
                API.WriteINI("RuiLangPrint", "PrintLabel", edprinters.Text.Trim(), frmRuiLangService.configFileName);
                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            spinEdit1.Enabled = radioGroup1.SelectedIndex == 0 ? true : false;
            spinEdit2.Enabled = spinEdit3.Enabled = radioGroup1.SelectedIndex == 0 ? false : true;
        }
    }
}