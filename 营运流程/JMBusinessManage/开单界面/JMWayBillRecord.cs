﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.IO;

namespace ZQTMS.UI
{
    public partial class JMWayBillRecord : BaseForm
    {
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        public JMWayBillRecord()
        {
            InitializeComponent();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
            {
                JMfrmWayBillAdd fwb = JMfrmWayBillAdd.Get_JMfrmWayBillAdd;
                fwb.rpt = this.rpt;
                fwb.Show();
                fwb.TopMost = true;
                fwb.TopMost = false;
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("DepName", DepName.Text.Trim() == "全部" ? "%%" : DepName.Text.Trim()));

                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("BillMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void JMWayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetDep(DepName, AreaName.Text);
            CommonClass.SetUser(BillMan, BegWeb.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            DepName.Text = CommonClass.UserInfo.DepartName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            BillMan.Text = CommonClass.UserInfo.UserName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "BillState", DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual, 5, Color.FromArgb(128, 255, 128));
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            //加载标签
            Thread tt = new Thread(load_rpt);
            tt.IsBackground = true;
            tt.Start();
        }

        private void load_rpt()
        {
            //加载标签
            string fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelFileName;
            if (File.Exists(fileName))
            {
                rpt.LoadLayout(fileName);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            string BillNo = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            if (CommonClass.GetBillState(BillNo) > 0)
            {
                MsgBox.ShowOK("运单不在库存，不能修改运单！");
                return;
            }
            if (CommonClass.GetAduitState(BillNo) > 0)
            {
                MsgBox.ShowOK("运单已经审核，不能修改！");
                return;
            }
            if (QSP_GET_BILLISACC(BillNo) > 0)
            {
                MsgBox.ShowOK("运单已出库并扣款，不能修改！");
                return;
            }
            if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
            {
                JMfrmWayBillAdd fwb = new JMfrmWayBillAdd();
                fwb.rpt = this.rpt;
                fwb.isModify = 1;
                fwb.BillNO = BillNo;
                fwb.ShowDialog();
            }
        }


        public static int QSP_GET_BILLISACC(string BillNo)
        {
            int num = 0;
            try
            {
                List<SqlPara> list_1 = new List<SqlPara>();
                list_1.Add(new SqlPara("billno", BillNo));
                SqlParasEntity sps_1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLISACC", list_1);
                DataSet ds = SqlHelper.GetDataSet(sps_1);
                num = ConvertType.ToInt32(ds.Tables[0].Rows[0]["num"]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return num;
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void Depart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void web_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(BillMan, BegWeb.Text);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            if (MsgBox.ShowYesNo("是否删除运单？") != DialogResult.Yes) return;
            string BillNO = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNO", BillNO));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_WAYBILL", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
                myGridView1.DeleteRow(rows);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billno = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            CommonClass.ShowBillSearch(billno);
        }

        private void tsmiPrintEnvelope_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;

            frmRuiLangService.Print("信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
        }

        private void tsmiPrintLabel_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            //DEV打印标签
            JMfrmPrintLabelDev fpld = new JMfrmPrintLabelDev(ds.Tables[0]);
            fpld.rpt = rpt;
            fpld.ShowDialog();

            //锐浪打印
            //JMfrmPrintLabel fpl = new JMfrmPrintLabel(billNo, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            //fpl.ShowDialog();
        }

        private void myGridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //contextMenuStrip1.Show();
                //contextMenuStrip1.Top = e.Y + contextMenuStrip1.Height;
                //contextMenuStrip1.Left = e.X;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //int rows = myGridView1.FocusedRowHandle;
            //if (rows < 0) return;
            //string ReceiptCondition = myGridView1.GetRowCellValue(rows, "ReceiptCondition").ToString();
            //tsmiPrintEnvelope.Enabled = ReceiptCondition != "" ? true : false;
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            string ReceiptCondition = myGridView1.GetRowCellValue(rows, "ReceiptCondition").ToString();
            barButtonItem18.Enabled = ReceiptCondition != "" ? true : false;
        }

        private void tsmiPrintBill_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;

            frmRuiLangService.Print("托运单", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tsmiPrintLabel_Click(sender, e);
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            tsmiPrintEnvelope_Click(sender, e);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billno = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            CommonClass.ShowBillSearch(billno);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string billNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillNo");
            if (billNo == "") return;

            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState")) > 4)
            {
                MsgBox.ShowOK("本单已发车,不能作废!");
                return;
            }

            if (MsgBox.ShowYesNo("确定作废运单：" + billNo + "？") != DialogResult.Yes) return;

            //Type：1表示作废;2表示恢复
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_WAYBILL_ZUOFEI", new List<SqlPara> { new SqlPara("BillNo", billNo) })) == 0) return;

            myGridView1.SetRowCellValue(rowhandle, "BillState", 100);
            myGridView1.SetRowCellValue(rowhandle, "ZuoFeiMan", CommonClass.UserInfo.UserName);
            myGridView1.SetRowCellValue(rowhandle, "ZuoFeiDate", CommonClass.gcdate);
            MsgBox.ShowOK("作废成功!");
        }
    }
}