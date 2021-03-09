using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Data.OleDb;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmSendDetailPre : BaseForm
    {
        public frmSendDetailPre()
        {
            InitializeComponent();
        }
        public GridView gv;
        List<int> EditRows = new List<int>();

        private void w_send_detail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);

            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            getdata();
        }

        private void getdata()
        {
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string sendbatch = "";
            try
            {
                //填写送货信息
                sendbatch = ConvertType.ToString(gv.GetFocusedRowCellValue("SendBatch"));
                edinoneflag.Text = sendbatch;
                edvehicleno.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendCarNO"));
                edsendman.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriver"));
                edsendmantel.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriverPhone"));
                edsenddate.DateTime = ConvertType.ToDateTime(gv.GetFocusedRowCellValue("SendDate"));
                edsendtosite.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToSite"));
                edsendtoweb.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToWeb"));
                edsendmadeby.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendOperator"));
                edsendremark.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDesc"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendbatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAILPRE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "送货清单");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_LOADPRE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "送货库存");
        }

        private void cbadd_Click(object sender, EventArgs e)
        {
            if (CommonClass.CheckKongHuo(myGridView2, 2))
            {
                MsgBox.ShowOK("选择的清单包含控货的运单,不能送货!");
                return;
            }
            if (XtraMessageBox.Show("确定将选中的运单加入到本车清单吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            myGridView1.PostEditor();
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0) return;

            string billnos = "", accsends = "", sendpcss = "", sendtosite = "", sendtoweb = "", IdStr = "";
            sendtosite = edsendtosite.Text.Trim();
            sendtoweb = edsendtoweb.Text.Trim();
            if (CheckInOneVehicle(ref billnos))
            {
                XtraMessageBox.Show("本车中已含有该票，不能同一票拆开，然后又装入同一车中。\r\n请先在本车中剔除该票，然后重新调入库存，再加入本车。\r\n本车中已存在的运单：\r\n" + billnos, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            billnos = "";
            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] < 0) continue;
                if (sendtosite == "" && ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "TransferMode")) == "自提") continue;
                billnos += GridOper.GetRowCellValueString(myGridView1, rows[i], "BillNo") + ",";
                accsends += ConvertType.ToDecimal(myGridView1.GetRowCellValue(rows[i], "AccSend")) + ",";
                sendpcss += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "sendqty")) + ",";
                IdStr += GridOper.GetRowCellValueString(myGridView1, rows[i], "Id") + ",";
            }
            if (billnos == "")
            {
                if (sendtosite == "")
                {
                    MsgBox.ShowOK("本车是送货上门的，请注意选择加货的运单不能是自提的!");
                }
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
            list.Add(new SqlPara("SendDate", edsenddate.DateTime));
            list.Add(new SqlPara("billnos", billnos));
            list.Add(new SqlPara("AccSends", accsends));
            list.Add(new SqlPara("SendPCSs", sendpcss));
            list.Add(new SqlPara("type", 1));
            list.Add(new SqlPara("IdStr", IdStr));
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, sendtosite == "" ? "USP_ADD_SENDPRE" : "USP_ADD_SEND_TOSITEPRE", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                getdata();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private bool CheckInOneVehicle(ref string units)
        {//gridView2清单
            int[] rows = myGridView1.GetSelectedRows();
            string billno = "";
            for (int i = 0; i < rows.Length; i++)
            {
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "Billno"));
                for (int j = 0; j < myGridView2.RowCount; j++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(j, "Billno")) == billno)
                    {
                        units += billno.ToString() + ",";
                    }
                }
            }
            units = units.TrimEnd(',');
            return units == "" ? false : true;
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private bool CheckHexiao(ref string result)
        {
            return false;
        }

        private void cbdeletesingle_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string billno = GridOper.GetRowCellValueString(myGridView2, rowhandle, "BillNo");
            if (CommonClass.QSP_LOCK_4(billno, edsenddate.Text.Trim()))
            {
                return;
            }
            if (MsgBox.ShowYesNo("确定剔除选中项?") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", billno));
            list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
            list.Add(new SqlPara("SendPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "SendPCS"))));
            list.Add(new SqlPara("SendDepartureListNo", GridOper.GetRowCellValueString(myGridView2, rowhandle, "SendDepartureListNo")));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_SENDPRE", list)) == 0) return;
            myGridView2.DeleteRow(rowhandle);
            MsgBox.ShowOK();
        }

        private void cbdeleteall_Click(object sender, EventArgs e)
        {
            string BillNo = "", IdStr = "", SendPCSStr = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                BillNo += GridOper.GetRowCellValueString(myGridView2, i, "BillNo") + "@";
                IdStr += GridOper.GetRowCellValueString(myGridView2, i, "SendDepartureListNo") + "@";
                SendPCSStr += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "SendPCS")) + "@";
            }
            if (BillNo == "") return;
            if (MsgBox.ShowYesNo("确定取消整车？") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNoStr", BillNo));
            list.Add(new SqlPara("SendBatch", edinoneflag.Text.Trim()));
            list.Add(new SqlPara("IdStr", IdStr));
            list.Add(new SqlPara("SendPCSStr", SendPCSStr));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_SENDPRE_ALL", list)) == 0) return;
            gv.DeleteSelectedRows();//删除选中行
            MsgBox.ShowOK("整车作废成功!");
            this.Close();
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            string sendBatch = edinoneflag.Text.Trim();
            if (sendBatch == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINTPre", new List<SqlPara> { new SqlPara("SendBatch", sendBatch) }));
            if (ds == null || ds.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("预送货清单.grf", ds);
            fpr.ShowDialog();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string result = "";
            if (CheckHexiao(ref result))
            {
                XtraMessageBox.Show("本车有些运单实际送货费已经经过财务销账，不能再调整实际送货费。必须经过财务反核销，然后才能调整!\r\n\r\n已经销账的运单号为：\r\n" + result, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            simpleButton7.Enabled = true;
            GridColumn gc = myGridView2.Columns["AccSend"];
            gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = true;
            myGridView2.FocusedColumn = gc;
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            myGridView2.PostEditor();
            if (EditRows.Count == 0)
            {
                MsgBox.ShowOK("没有修改送货费!");
                return;
            }

            string billnos = "", AccSends = "";
            try
            {
                GridColumn gc = myGridView2.Columns["AccSend"];
                for (int i = 0; i < EditRows.Count; i++)
                {
                    billnos += ConvertType.ToString(myGridView2.GetRowCellValue(EditRows[i], "BillNo")) + ",";
                    AccSends += ConvertType.ToDecimal(myGridView2.GetRowCellValue(EditRows[i], gc)) + ",";

                    if (CommonClass.QSP_LOCK_4(myGridView2.GetRowCellValue(EditRows[i], "BillNo").ToString(), edsenddate.Text.Trim()))
                    {
                        return;
                    }

                }
                if (billnos == "") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("sendbatch", edinoneflag.Text.Trim()));
                list.Add(new SqlPara("billnos", billnos));
                list.Add(new SqlPara("accsends", AccSends));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFIED_ACCSEND_BY_BATCH", list));
                gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = false;
                EditRows.Clear();
                simpleButton7.Enabled = false;
                MsgBox.ShowOK("保存成功");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (textEdit3.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入整车送货费!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textEdit3.Focus();
                return;
            }

            try
            {
                int qtytotal = Convert.ToInt32(myGridView2.Columns["qty"].SummaryItem.SummaryValue);
                int type = radioGroup1.SelectedIndex;
                int acctotal = Convert.ToInt32(textEdit3.Text.Trim());

                if (type == 0)
                {//按件数分摊
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {

                    }
                    //barEditItem4.EditValue = "按件数";
                }
                else if (type == 1)
                {
                    int a = 0;

                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {

                    }
                    //barEditItem4.EditValue = "按运费";
                }
                else
                {
                    decimal avg = Math.Floor(acctotal / Convert.ToDecimal(myGridView2.RowCount));
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        if (i == myGridView2.RowCount - 1)
                        {
                            myGridView2.SetRowCellValue(i, "accsendout", acctotal - avg * (myGridView2.RowCount - 1));
                        }
                        else
                        {
                            myGridView2.SetRowCellValue(i, "accsendout", avg);
                        }
                    }
                    //barEditItem4.EditValue = "按票";
                }
                //gridColumn92.OptionsColumn.AllowEdit = false;
                //gridColumn92.OptionsColumn.AllowFocus = false;
                myGridView2.UpdateSummary();
                panelControl3.Visible = false;
                XtraMessageBox.Show("分摊成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "accsendout", 0);
            }
            //gridColumn92.OptionsColumn.AllowEdit = false;
            //gridColumn92.OptionsColumn.AllowFocus = false;
            panelControl3.Visible = false;
            simpleButton7.Enabled = false;
            XtraMessageBox.Show("取消分摊成功!实际送货费已清零!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            panelControl3.Visible = false;
        }

        private void myGridControl2_DoubleClick(object sender, EventArgs e)
        {

        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {

        }

        private void panelControl3_Leave(object sender, EventArgs e)
        {
            textEdit3.Focus();
            if (!panelControl3.Focused)
            {
                panelControl3.Visible = false;
            }
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            try
            {
                GridColumn gc = ((DevExpress.XtraGrid.Views.Grid.GridView)sender).FocusedColumn;
                if (e == null || gc == null || gc.FieldName != "sendqty") return;
                int oldvalue = ConvertType.ToInt32(myGridView1.GetFocusedRowCellValue("sendremainqty"));//库存件数
                int newvalue = ConvertType.ToInt32(e.Value);//填的件数
                if (newvalue <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "实发件数必须大于0！";
                }
                else if (newvalue > oldvalue)
                {
                    e.Valid = false;
                    e.ErrorText = string.Format("实发件数不能大于库存件数(当前库存{0}件)！", oldvalue);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void barbtnPrintAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            PrintQSD(str);
            //cc.RegReport();
            //SqlConnection Conn = cc.GetConn();
            //int isprint = 0; //当为1时打印全部，0时停止所有打印
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    try
            //    {
            //        DataSet dsPrint = new DataSet();
            //        //grproLib.GridppReport Report = new grproLib.GridppReport();
            //        int iunit = Convert.ToInt32(myGridView2.GetRowCellValue(i, "unit"));

            //        SqlCommand sqlcmd = new SqlCommand("QSP_GET_FETCH_FOR_PRINT_RL", Conn);
            //        sqlcmd.CommandType = CommandType.StoredProcedure;
            //        sqlcmd.Parameters.AddWithValue("@unit", iunit);
            //        sqlcmd.Parameters.AddWithValue("@webid", commonclass.webid);
            //        //SqlDataAdapter sqlda = new SqlDataAdapter(sqlcmd);
            //        sqlda.Fill(dsPrint);

            //        Report.LoadFromFile(Application.StartupPath + "\\提货单(套打).grf");
            //        //Report.Printer.PaperLength = commonclass.thdh;
            //        //Report.Printer.PaperWidth = commonclass.thdw;
            //        //Report.Printer.PaperSize = 256;
            //        //Report.Title = commonclass.reptitle + "客户签收单";
            //        Report.LoadDataFromXML(dsPrint.GetXml());
            //        if (isprint == 0) isprint = Report.Printer.PrintDialog() ? 1 : 0;
            //        if (isprint == 1)
            //        {
            //            Report.Print(false);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        break;
            //    }
            //}
        }

        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }

        private void myGridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || EditRows.Contains(e.RowHandle)) return;
            EditRows.Add(e.RowHandle);
        }

        private void barbtnPrintQSD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] rows = myGridView2.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(rows[i], "BillNo")) + "@";
            }
            PrintQSD(str);
        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            DataTable dt = ds.Tables[0].Clone();
            frmPrintRuiLang fprl;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dt.ImportRow(ds.Tables[0].Rows[i]);
                fprl = new frmPrintRuiLang("套打签收单", dt);
                fprl.ShowDialog();
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            string sendBatch = edinoneflag.Text.Trim();
            if (sendBatch == "") return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SendBatch", sendBatch));
            list.Add(new SqlPara("SendCarNO", edvehicleno.Text.Trim()));
            list.Add(new SqlPara("SendDriver", edsendman.Text.Trim()));
            list.Add(new SqlPara("SendDriverPhone", edsendmantel.Text.Trim()));
            list.Add(new SqlPara("SendDesc", edsendremark.Text.Trim()));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLSENDGOODSPRE", list)) == 0) return;
            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                gv.SetRowCellValue(rowhandle, "SendCarNO", edvehicleno.Text.Trim());
                gv.SetRowCellValue(rowhandle, "SendDriver", edsendman.Text.Trim());
                gv.SetRowCellValue(rowhandle, "SendDriverPhone", edsendmantel.Text.Trim());
                gv.SetRowCellValue(rowhandle, "SendDesc", edsendremark.Text.Trim());
            }
            MsgBox.ShowOK("保存成功!");
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}