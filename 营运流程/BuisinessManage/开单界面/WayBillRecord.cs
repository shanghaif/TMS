using System;
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
using System.IO;
using System.Threading;

namespace ZQTMS.UI
{
    public partial class WayBillRecord : BaseForm
    {
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        public WayBillRecord()
        {
            InitializeComponent();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmWayBillAdd fwb = frmWayBillAdd.Get_frmWayBillAdd 
            //zaj 2017-11-15 将开单界面从frmWayBillAdd_JMGX换成frmWayBillAdd_JMGXClone
            // frmWayBillAdd_JMGX fwb = new frmWayBillAdd_JMGX();
            //if (CommonClass.UserInfo.companyid != "105" && CommonClass.UserInfo.companyid != "102" && CommonClass.UserInfo.companyid != "103" && CommonClass.UserInfo.companyid != "104" && CommonClass.UserInfo.companyid != "107" && CommonClass.UserInfo.companyid != "109")
            //{
            //    frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
            //    fwb.rpt = rpt;
            //    fwb.Show();

            //}
            //else
            //{
            //    frmWayBillAdd_JMGXClone fwb = new frmWayBillAdd_JMGXClone();
            //    fwb.rpt = rpt;
            //    fwb.Show();
            //}

            frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
            fwb.rpt = rpt;
            fwb.Show();

        }

        private void load_rpt()
        {
            //加载标签
            //string fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelFileName;
            string fileName = Application.StartupPath + "\\Reports\\" + CommonClass.GetLabelNameNew;//zaj 可以给每一个公司设置自己的标签名 2017-1-4

            if (File.Exists(fileName))
            {
                rpt.LoadLayout(fileName);
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            myGridView1.ClearColumnsFilter();
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
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                // if (myGridView1.RowCount < 1000)  myGridView1.BestFitColumns();
                //GridOper.RestoreGridLayout(myGridView1); //zb20190429
            }


        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("受理开单");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            //暂时给某个网点测试
            //if (CommonClass.UserInfo.WebName != "深圳华通源营业一部") barButtonItem19.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            bdate.DateTime = CommonClass.gbdate.AddDays(+1).AddHours(-24);
            edate.DateTime = CommonClass.gedate.AddDays(+1).AddHours(-24);

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
            GridOper.CreateStyleFormatCondition(myGridView1, "BillState", DevExpress.XtraGrid.FormatConditionEnum.Equal, 100, Color.Red);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            if (!UserRight.GetRight("121188"))
            {
                barButtonItem21.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            //加载标签
            Thread tt = new Thread(load_rpt);
            tt.IsBackground = true;
            tt.Start();
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
                MsgBox.ShowOK("运单已经审核，不能执行！");
                return;
            }
            if (CommonClass.GetVerifyStatus(BillNo) > 0)  //maohui20180202
            {
                MsgBox.ShowOK("运单应收费用已经核销，不能修改！");
                return;
            }
            #region 判断是否为加盟商，然后通过运单ID判断是否执行过短驳接收   zb20190505
            if (CommonClass.UserInfo.WebRole == "加盟")
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "[QSP_GET_LOG_BYBILLNO]", list));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MsgBox.ShowOK("该单已扣结算费用，如需修改请走改单申请");
                    return;
                }
            }
            #endregion

            //if (CommonClass.UserInfo.companyid != "105" && CommonClass.UserInfo.companyid != "102" && CommonClass.UserInfo.companyid != "103" && CommonClass.UserInfo.companyid != "104" && CommonClass.UserInfo.companyid != "107" && CommonClass.UserInfo.companyid != "109")
            //{
            //    frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
            //    fwb.rpt = rpt;
            //    fwb.isModify = 1;
            //    fwb.BillNO = BillNo;
            //    fwb.ShowDialog();
            //}
            //else
            //{
            //    frmWayBillAdd_JMGXClone fwb = new frmWayBillAdd_JMGXClone();
            //    fwb.rpt = rpt;
            //    fwb.isModify = 1;
            //    fwb.BillNO = BillNo;
            //    fwb.ShowDialog();
            //}
            frmWayBillAdd_JMGX_Upgrade fwb = new frmWayBillAdd_JMGX_Upgrade();
            fwb.rpt = rpt;
            fwb.isModify = 1;
            fwb.BillNO = BillNo;
            fwb.ShowDialog();
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
            if (SqlHelper.GetDataSet(sps).Tables[0].Rows.Count > 0)
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
            frmBillSearchControl.ShowBillSearch(myGridView1, "BillNo");

        }

        private void tsmiPrintEnvelope_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;
            if (CommonClass.UserInfo.companyid == "240")
            {
                frmRuiLangService.Print("富成达信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "159")
            {
                frmRuiLangService.Print("金邦物流信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "167")  //maohui20180630
            {
                frmRuiLangService.Print("佳安信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "155")  //hj20180705
            {
                frmRuiLangService.Print("东尚信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "210")
            {
                frmRuiLangService.Print("苏州复兴信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }    //HZ20180829
            if (CommonClass.UserInfo.companyid == "216")
            {
                frmRuiLangService.Print("上海天陆物流信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }    //HZ20180829
            if (CommonClass.UserInfo.companyid == "231")
            {
                frmRuiLangService.Print("江苏远中信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "239")
            {
                frmRuiLangService.Print("可通信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            if (CommonClass.UserInfo.companyid == "488" || CommonClass.UserInfo.companyid == "132")
            {
                frmRuiLangService.Print("488信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            //whf20190601
            if (CommonClass.UserInfo.companyid == "484")
            {
                frmRuiLangService.Print("鑫乐信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            //zb20190601
            if (CommonClass.UserInfo.companyid == "485")
            {
                frmRuiLangService.Print("川胜集团信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            //whf201906018
            if (CommonClass.UserInfo.companyid == "483")
            {
                frmRuiLangService.Print("凯宇信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE_KY", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
            //HZ20180901 
            if (CommonClass.UserInfo.companyid != "159" && CommonClass.UserInfo.companyid != "240" && CommonClass.UserInfo.companyid != "167" && CommonClass.UserInfo.companyid != "155" && CommonClass.UserInfo.companyid != "210" && CommonClass.UserInfo.companyid != "216" && CommonClass.UserInfo.companyid != "231" && CommonClass.UserInfo.companyid != "239" && CommonClass.UserInfo.companyid != "488" && CommonClass.UserInfo.companyid != "132" && CommonClass.UserInfo.companyid != "485" && CommonClass.UserInfo.companyid != "484"&& CommonClass.UserInfo.companyid !="483")
            {
                frmRuiLangService.Print("信封", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTENVELOPE", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            }
        }

        private void tsmiPrintLabel_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            //锐浪打印标签
            frmPrintLabel fpl = new frmPrintLabel(billNo, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            fpl.ShowDialog();
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

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPrintLable_Pre frm = new frmPrintLable_Pre();
            frm.ShowDialog();
        }
        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            string ReceiptCondition = myGridView1.GetRowCellValue(rows, "ReceiptCondition").ToString();
            if (CommonClass.UserInfo.companyid != "167" || CommonClass.UserInfo.companyid != "239")  //maohui20180702   ZJF20181026
            {
                barButtonItem18.Enabled = true;
            }
        }

        private void tsmiPrintBill_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;
            string name = "";
            if (CommonClass.UserInfo.BookNote == "")
            {
                

                name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
            }
            else
            {

                if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹广州项目部")
                {
                    name = "宝虹广州项目部托运单";
                }
                else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "宝虹长沙星沙营业部")
                {
                    name = "宝虹长沙星沙营业部托运单";
                }
                else if (CommonClass.UserInfo.companyid == "486" && CommonClass.UserInfo.WebName == "武汉遵义营业部")
                {
                    name = "武汉遵义营业部托运单";
                }
                else
                {
                    name = CommonClass.UserInfo.BookNote;
                }
                
              //  name = CommonClass.UserInfo.BookNote;
            }

            frmRuiLangService.Print(name, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
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

            frmBillSearchControl.ShowBillSearch(myGridView1, "BillNo");

        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV2", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
            fpld.rpt = rpt;
            fpld.ShowDialog();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string billNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillNo");
            if (billNo == "") return;

            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState")) > 0)
            {
                MsgBox.ShowOK("只有新开单才可以作废!");
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

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            string BillNo = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            frmWayBillAdd_WB fwb = new frmWayBillAdd_WB();
            fwb.rpt = rpt;
            fwb.isModify = 1;
            fwb.BillNO = BillNo;
            fwb.ShowDialog();
        }

        //运单作废恢复
        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DataRow row = this.myGridView1.GetFocusedDataRow();
            //if (row != null)
            //{
            //    if (row["BillState"].ToString() != "100")
            //    {
            //        return;
            //    }
            //    try
            //    {
            //        List<SqlPara> list = new List<SqlPara>();
            //        list.Add(new SqlPara("BillNo", row["BillNo"].ToString()));
            //        SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_WAYBILL_ZUOFEI_Recovery", list);
            //        int num = SqlHelper.ExecteNonQuery(spe);
            //        if (num > 0)
            //        {
            //            MsgBox.ShowOK("运单已恢复，开单时间更改为当前时间！");
            //        }
            //        else
            //        {
            //            MsgBox.ShowOK("恢复失败！");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MsgBox.ShowError(ex.Message);
            //    }
            //}
            //else
            //{
            //    MsgBox.ShowOK("请选中行！");
            //}
            frmZuoFeiCancel frm = new frmZuoFeiCancel(); //hj20181119
            frm.ShowDialog();
        }
        //打印回扣凭证
        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTVOUCHERS", new List<SqlPara> { new SqlPara("BillNo", billNo) });
            DataSet ds = SqlHelper.GetDataSet(sps1);
            ds.Tables[0].Columns.Add("NowCompany");
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["NowCompany"] = CommonClass.UserInfo.gsqc;
            }
            frmRuiLangService.Print("回扣凭证", ds);

        }

        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWayBillRecordZQUP frm = new frmWayBillRecordZQUP();
            frm.Show();
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmExecuteDeduction ed = new frmExecuteDeduction();
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            decimal PaymentAmout = ConvertType.ToDecimal(myGridView1.GetFocusedRowCellValue("ActualFreight"));
            string billstate = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillState"));
            string begweb = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BegWeb"));
            string middlesite = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("TransferSite"));
            string feeWeight = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("FeeWeight"));
            string feeVolume = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("FeeVolume"));
            int num = ConvertType.ToInt32(myGridView1.GetFocusedRowCellValue("Num"));
            ed.billno = billNo;
            ed.middleSite = middlesite;
            ed.amount = PaymentAmout;
            ed.feeWeight = feeWeight;
            ed.feeVolume = feeVolume;
            ed.num = num;
            ed.ShowDialog();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MsgBox.ShowOK("实收运费:总运费-代收货款-垫付款-折扣折让"+ "\n" + "发车时间：批次点击发车的时间");
        }

    }
}