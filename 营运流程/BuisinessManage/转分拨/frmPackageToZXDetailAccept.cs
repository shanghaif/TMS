using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmPackageToZXDetailAccept : ZQTMS.Tool.BaseForm
    {
        public frmPackageToZXDetailAccept()
            : this("")
        {
            //InitializeComponent();
        }

        public frmPackageToZXDetailAccept(string PID)
        {
            InitializeComponent();
            this.PID = PID;
            this.Batch = PID;
        }

        /// <summary>
        /// 分拨编号
        /// </summary>
        string PID;
        string Batch;

        private void frmPackageToZXDetailAccept_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false);
            CommonClass.GetGridViewColumns(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(myGridView1);

            getdata();
        }

        private void getdata()
        {
            if (Batch != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_DETAIL_ACCEPT", new List<SqlPara> { new SqlPara("Batch", Batch) }));
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string BillNoStr = "", AcceptWebNameStr="";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
               // if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                BillNoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
                AcceptWebNameStr += GridOper.GetRowCellValueString(myGridView1, i, "AcceptWebName") + "@";
            }
            //限制接收网点 只能当前接收网点接收 hj20180409
            string [] AcceptWebName=AcceptWebNameStr.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in AcceptWebName)
            {
                if (str != CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowOK("接收网点只能由当前接收网点接收!");
                    return;
                }
            }

            if (BillNoStr == "")
            {
                MsgBox.ShowOK("请选择要接收的运单！");
                return;
            }
            if (MsgBox.ShowYesNo("确定接收选择的货物？") != DialogResult.Yes) return;

            string BillNO = "";
            string ReceiptCondition = "";
            string HDBillno = "";
            string bb = "";
            try
            {
                List<SqlPara> list7 = new List<SqlPara>();
                list7.Add(new SqlPara("AllocateBatch", Batch));
                SqlParasEntity spe7 = new SqlParasEntity(OperType.Query, "USP_GET_FBReturnStock_BillNO", list7);
                DataSet ds7 = SqlHelper.GetDataSet(spe7);
                for (int i = 0; i < ds7.Tables[0].Rows.Count; i++)
                {
                    BillNO += ds7.Tables[0].Rows[i]["BillNO"] + "@";
                    ReceiptCondition += ds7.Tables[0].Rows[i]["ReceiptCondition"] + "@";
                }
                frmReturnStockDBCK frm = new frmReturnStockDBCK();
                frm.Billno = BillNO;
                frm.ReceiptCondition = ReceiptCondition;
                frm.type = "分拨接收";
                frm.ShowDialog();
                HDBillno = frm.aa;
                bb = frm.bb;
                if (bb == "1")
                {
                    return;
                }


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batch", Batch));
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                list.Add(new SqlPara("HDBillno",HDBillno));

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ACCEPT_FB_TO_ZX", list)) == 0) return;
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                    myGridView1.SetRowCellValue(i, "AcceptCause", CommonClass.UserInfo.CauseName);
                    myGridView1.SetRowCellValue(i, "AcceptArea", CommonClass.UserInfo.AreaName);
                    myGridView1.SetRowCellValue(i, "AcceptMan", CommonClass.UserInfo.UserName);
                    myGridView1.SetRowCellValue(i, "AcceptDate", CommonClass.gcdate);
                }
                MsgBox.ShowOK("接收成功！");
                // BillDepartureFBAcceptSyn(BillNoStr);
                CommonSyn.BillDepartureFBAcceptSynNew(BillNoStr, Batch);
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
            CommonSyn.TraceSyn(null, BillNoStr, 17, "拨入接收", 1, null, null);
        }


        //private void BillDepartureFBAcceptSyn(string billnos)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNOStr", billnos));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_BillDepartureFBAcceptSyn", listQuery);
        //        DataSet dsQuery = SqlHelper.GetDataSet(spsQuery);
        //        if (dsQuery == null || dsQuery.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(dsQuery);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //        // string url = "http://localhost:42936/KDLMSService/ZQTMSBillDepartueFBAcceptSyn";
        //        //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSBillSendGoodsSyn";
        //        string url = HttpHelper.urlAllocateAcceptSyn;
        //        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //        if (model.State != "200")
        //        {
        //            MsgBox.ShowOK(model.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}



        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string BillNoStr = "",AcceptWebNameStr="";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
               // if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                BillNoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
                AcceptWebNameStr += GridOper.GetRowCellValueString(myGridView1, i, "AcceptWebName") + "@";
            }
            //限制取消接收网点 只能当前接收网点取消 hj20180409
            string[] AcceptWebName = AcceptWebNameStr.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in AcceptWebName)
            {
                if (str != CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowOK("取消接收网点只能由当前接收网点取消!");
                    return;
                }
            }

            if (BillNoStr == "")
            {
                MsgBox.ShowOK("请选择要取消接收的运单！");
                return;
            }
            if (MsgBox.ShowYesNo("确定取消接收选择的货物？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Batch", Batch));
            list.Add(new SqlPara("BillNoStr", BillNoStr));

            //提前获取到轨迹信息
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", null));
            lists.Add(new SqlPara("BillNO", BillNoStr));
            lists.Add(new SqlPara("tracetype", "拨入接收"));
            lists.Add(new SqlPara("num", 17));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_CANCEL_ACCEPT_FB_TO_ZX", list)) == 0) return;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                myGridView1.SetRowCellValue(i, "AcceptCause", "");
                myGridView1.SetRowCellValue(i, "AcceptArea", "");
                myGridView1.SetRowCellValue(i, "AcceptMan", "");
                myGridView1.SetRowCellValue(i, "AcceptDate", DBNull.Value);
            }
            MsgBox.ShowOK("取消接收成功！");
            CommonSyn.QXBillDepartureFBAcceptSynNew(BillNoStr, Batch);//hj20180412
            //CommonSyn.TraceSyn(null, BillNoStr, 17, "拨入接收", 2, null,dss);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getdata();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", checkEdit1.Checked ? 1 : 0);
            }
        }
        //jl20180904
        private void dropDownButton1_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    str += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
            }
            PrintQSD(str);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
            }
            PrintQSD(str);
        }

        private void PrintQSD(string BillNoStr)
        {
            myGridView1.PostEditor();
            if (BillNoStr == "") return;

            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (dss == null || dss.Tables.Count == 0 || dss.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //frmRuiLangService.Print("提货单", dss.Tables[0]);
            //jl20181127
            if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                || CommonClass.UserInfo.WebName == "杭州操作部"
                || CommonClass.UserInfo.WebName == "杭州操作部1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "宁波操作部"
                || CommonClass.UserInfo.WebName == "宁波操作部1"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                || CommonClass.UserInfo.WebName == "广州操作部"
                || CommonClass.UserInfo.WebName == "广州操作部1"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
            {
                frmRuiLangService.Print("提货单大坪", dss.Tables[0], CommonClass.UserInfo.gsqc);
            }
            else
            {
                frmRuiLangService.Print("提货单", dss.Tables[0], CommonClass.UserInfo.gsqc);
            }
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

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billNo = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo"));
            if (billNo == "")
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV2_BRJS", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            frmPrintLabelDev fpld = new frmPrintLabelDev(ds.Tables[0]);
            //fpld.rpt = rpt;
            fpld.ShowDialog();
        }

        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            // string ReceiptCondition = myGridView1.GetRowCellValue(rows, "ReceiptCondition").ToString();
            //if (CommonClass.UserInfo.companyid != "167" || CommonClass.UserInfo.companyid != "239")  //maohui20180702   ZJF20181026
            //{
            //    barButtonItem18.Enabled = true;
            //}
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
                name = CommonClass.UserInfo.BookNote;
            }

            frmRuiLangService.Print(name, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTBIll_TX_BRJS ", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
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

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_BRJS", new List<SqlPara> { new SqlPara("BillNo", billNo) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            //锐浪打印标签
            frmPrintLabel fpl = new frmPrintLabel(billNo, SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_BRJS", new List<SqlPara> { new SqlPara("BillNo", billNo) })));
            fpl.ShowDialog();
        }
    }
}