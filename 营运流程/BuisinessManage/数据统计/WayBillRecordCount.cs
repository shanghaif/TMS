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
    public partial class WayBillRecordCount : BaseForm
    {
        XtraReport rpt = new XtraReport();//为了加快打印标签的显示速度

        public WayBillRecordCount()
        {
            InitializeComponent();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmWayBillAdd fwb = frmWayBillAdd.Get_frmWayBillAdd 
            //zaj 2017-11-15 将开单界面从frmWayBillAdd_JMGX换成frmWayBillAdd_JMGXClone
           // frmWayBillAdd_JMGX fwb = new frmWayBillAdd_JMGX();
            if (CommonClass.UserInfo.companyid != "105" && CommonClass.UserInfo.companyid != "102" && CommonClass.UserInfo.companyid != "103" && CommonClass.UserInfo.companyid != "104" && CommonClass.UserInfo.companyid != "107" && CommonClass.UserInfo.companyid != "109")
            {
                frmWayBillAdd_JMGX_HD fwb = new frmWayBillAdd_JMGX_HD();
                fwb.rpt = rpt;
                fwb.Show();

            }
            else
            {
                frmWayBillAdd_JMGXClone fwb = new frmWayBillAdd_JMGXClone();
                fwb.rpt = rpt;
                fwb.Show();
            }
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

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ForDataStatistics", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
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

        private void WayBillRecordCount_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            //暂时给某个网点测试
            //if (CommonClass.UserInfo.WebName != "深圳华通源营业一部") barButtonItem19.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            //CommonClass.SetSite(StartSite, true);
            //CommonClass.SetSite(TransferSite, true);
            //CommonClass.SetWeb(BegWeb, StartSite.Text);
            //CommonClass.SetCause(CauseName, true);
            //CommonClass.SetArea(AreaName, CauseName.Text);
            //CommonClass.SetDep(DepName, AreaName.Text);
            //CommonClass.SetUser(BillMan, BegWeb.Text);
            SetSite(StartSite, true);
            SetSite(TransferSite, true);
            SetWeb(BegWeb, StartSite.Text,true);
            SetCause(CauseName, true);
            SetArea(AreaName, CauseName.Text, true);
            SetDep(DepName, AreaName.Text, true);
            SetUser(BillMan, BegWeb.Text, true);

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

        private  void SetSite(ComboBoxEdit cb, bool isall)
        {

           // 
            try
            {
                List<SqlPara> list=new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_ForDataStatistics", list);

                DataSet dsSite = SqlHelper.GetDataSet(sps);
                if (dsSite == null || dsSite.Tables.Count == 0) return;
                for (int i = 0; i < dsSite.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsSite.Tables[0].Rows[i]["SiteName"].ToString()) && !cb.Properties.Items.Contains(dsSite.Tables[0].Rows[i]["SiteName"].ToString()))
                    {
                        cb.Properties.Items.Add(dsSite.Tables[0].Rows[i]["SiteName"]);
                    }
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private  void SetCause(ComboBoxEdit cb, bool isall)
        {            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE_ForDataStatistics",list);
                DataSet dsCause = SqlHelper.GetDataSet(sps);
                if (dsCause == null || dsCause.Tables.Count == 0) return;
                for (int i = 0; i < dsCause.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(dsCause.Tables[0].Rows[i]["CauseName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void SetArea(ComboBoxEdit cb, string AreaCause, bool isall)
        {
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA__ForDataStatistics", list);
                DataSet dsArea = SqlHelper.GetDataSet(sps);
                if (dsArea == null || dsArea.Tables.Count == 0) return;
                if (AreaCause == "全部") AreaCause = "%%";
                DataRow[] dr = dsArea.Tables[0].Select("AreaCause like '" + AreaCause + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["AreaName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
                else
                {
                    cb.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private  void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
           
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB_ForDataStatistics", list);
                DataSet dsWeb = SqlHelper.GetDataSet(sps);
                if (dsWeb == null || dsWeb.Tables.Count == 0) return;
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private  void SetDep(ComboBoxEdit cb, string DepArea, bool isall)
        {
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART_ForDataStatistics", list);
                DataSet dsDep = SqlHelper.GetDataSet(sps);
                if (dsDep == null || dsDep.Tables.Count == 0) return;
                if (DepArea == "全部") DepArea = "%%";
                DataRow[] dr = dsDep.Tables[0].Select("DepArea like '" + DepArea + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["DepName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        public static void SetUser(ComboBoxEdit cb, string WebName, bool isall)
        {
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_USERNAME_ForDataStatistics", list);
                DataSet dsUsers = SqlHelper.GetDataSet(sps);
                if (dsUsers == null || dsUsers.Tables.Count == 0) return;
                if (WebName == "全部") WebName = "%%";
                DataRow[] dr = dsUsers.Tables[0].Select("WebName like '" + WebName + "'");
                cb.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["UserName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
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
            if (CommonClass.UserInfo.companyid != "105" && CommonClass.UserInfo.companyid != "102" && CommonClass.UserInfo.companyid != "103" && CommonClass.UserInfo.companyid != "104" && CommonClass.UserInfo.companyid != "107" && CommonClass.UserInfo.companyid != "109")
            {
                frmWayBillAdd_JMGX_HD fwb = new frmWayBillAdd_JMGX_HD();
                fwb.rpt = rpt;
                fwb.isModify = 1;
                fwb.BillNO = BillNo;
                fwb.ShowDialog();
            }
            else
            {
                frmWayBillAdd_JMGXClone fwb = new frmWayBillAdd_JMGXClone();
                fwb.rpt = rpt;
                fwb.isModify = 1;
                fwb.BillNO = BillNo;
                fwb.ShowDialog();
            }
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
            frmBillSearch.ShowBillSearch(myGridView1, "BillNo");
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
            barButtonItem18.Enabled = ReceiptCondition != "" ? true : false;
        }

        private void tsmiPrintBill_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "") return;
            string name="";
            if (CommonClass.UserInfo.BookNote == "")
            {
                name = CommonClass.UserInfo.IsAutoBill == false ? "托运单" : "托运单(打印条码)";
            }
            else
            {
                name = CommonClass.UserInfo.BookNote;
            }
             
            frmRuiLangService.Print(name, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
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
            frmBillSearch.ShowBillSearch(myGridView1, "BillNo");
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

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
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
    }
}