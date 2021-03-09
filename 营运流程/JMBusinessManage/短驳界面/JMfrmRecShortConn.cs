using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.Threading;

namespace ZQTMS.UI
{
    public partial class JMfrmRecShortConn : BaseForm
    {
        public string SCId = "";
        public string U_SCBatchNo = "";
        public string U_SCDate = "";
        public string U_SCSite = "";
        public string U_SCWeb = "";
        public string U_SCCarNo = "";
        public string U_SCDriver = "";
        public string U_SCDesSite = "";
        public string U_SCDesWeb = "";
        public string U_SCDContolMan = "";
        public string SCAcceptMan = "";

        DataSet ds = new DataSet();

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("begSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("begWeb", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PACKAGE_LOAD_FOR_CONNECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        public JMfrmRecShortConn()
        {
            InitializeComponent();
        }

        /// <summary>
        /// isInBound:是否货物短驳到货车辆
        /// </summary>
        public bool isMod = true, isArrivedInBound = false;
        private void frmRecShortConn_Load(object sender, EventArgs e)
        {
            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            //barButtonItem8.Visibility = isMod ? BarItemVisibility.Always : BarItemVisibility.Never;
            //btnDelByCar.Visibility = isMod ? BarItemVisibility.Never : BarItemVisibility.Always;
            barButtonItem7.Visibility = isMod ? BarItemVisibility.Never : BarItemVisibility.Always;
            barEditItem1.Visibility = isMod ? BarItemVisibility.Never : BarItemVisibility.Always;
            xtraTabPage1.PageVisible = !isMod;

            //GridOper.CreateStyleFormatCondition(myGridView1, "AcceptBillMan", DevExpress.XtraGrid.FormatConditionEnum.Greater, "", Color.LightGreen);

            if (isArrivedInBound)
            {
                barEditItem1.Visibility = barButtonItem8.Visibility = BarItemVisibility.Never;
            }
            else
            {
                barButtonItem11.Visibility = barButtonItem13.Visibility = barButtonItem12.Visibility = BarItemVisibility.Never;
            }

            if (U_SCBatchNo != "")
                getShortCoonByID();
        }
        public void getShortCoonByID()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", U_SCBatchNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_BYCAR", list);
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

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "短驳取库存");
        }

        private void finished()
        {
            string BillNo = "";
            if (myGridView2.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("没有选择任何运单，请先选择运单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (myGridView2.IsRowSelected(i))
                {
                    BillNo += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                }
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDId", Guid.NewGuid()));
                list.Add(new SqlPara("SCBatchNo", U_SCBatchNo));
                list.Add(new SqlPara("SCDate", U_SCDate));
                list.Add(new SqlPara("SCSite", U_SCSite));
                list.Add(new SqlPara("SCWeb", U_SCWeb));
                list.Add(new SqlPara("SCCarNo", U_SCCarNo));
                list.Add(new SqlPara("SCDriver", U_SCDriver));
                list.Add(new SqlPara("SCDesSite", U_SCDesSite));
                list.Add(new SqlPara("SCDesWeb", U_SCDesWeb));
                list.Add(new SqlPara("SCDContolMan", U_SCDContolMan));
                list.Add(new SqlPara("BillNo", BillNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ShortConnDetail", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    CommonClass.SetOperLog(U_SCBatchNo, BillNo, "", CommonClass.UserInfo.UserName, "短驳明细", "当前短驳批次短驳明细新增一条");
                    myGridView2.DeleteSelectedRows();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
                return;
            }
        }

        private void cbadd_Click(object sender, EventArgs e)
        {
            finished();
            getShortCoonByID();
        }

        private void cancel()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                //if (SCAcceptMan != "")
                //{
                //    MsgBox.ShowOK("本车已接收，不能剔除！");
                //    return;
                //}

                //if (myGridView1.RowCount == 1)
                //{
                //    MsgBox.ShowOK("本车清单只剩下一票运单，不能剔除，只能整车作废");
                //    return;
                //}

                if (MsgBox.ShowYesNo("确定剔除？") != DialogResult.Yes) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "SCDId").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_ShortConnDetail", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    //CommonClass.SetOperLog(myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString(), myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString(), "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳单挑提出操作操作");
                    myGridView1.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(gridView2);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //接收本车
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("type", 1));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    CommonClass.SetOperLog(SCBatchNo, "", "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳接受本车操作");
                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.DeleteGridLayout(gridControl1, "短驳本车清单");
            //cc.DeleteGridLayout(gridControl2, "短驳本站库存");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(gridView2);
            //cc.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            cancel();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短驳明细");
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (U_SCBatchNo == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_BYCAR_PRINT", new List<SqlPara> { new SqlPara("SCBatchNo", U_SCBatchNo) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang("短驳清单", ds);
            fpr.ShowDialog();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            string BillNo = myGridView1.GetRowCellValue(0, "BillNo").ToString();
            Clipboard.SetText(BillNo);
            MsgBox.ShowOK();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SCBatchNo", U_SCBatchNo));
            list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
            string BillNoStr = "";
            try
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 1)
                        BillNoStr += ConvertType.ToString(myGridView1.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView1, "BillNo"))) + "@";
                }
                if (BillNoStr == "")
                {
                    MsgBox.ShowOK("请选择到货的运单!");
                    return;
                }
                if (MsgBox.ShowYesNo("确定选中的货已到货(忽略已到货的)？") != DialogResult.Yes) return;
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ACCEPT_BILL_RECSHORTCONN", list)) == 0) return;
                MsgBox.ShowOK();
                if (U_SCBatchNo != "")
                    getShortCoonByID();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //修改费用
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(SCId);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCId", id));
                list.Add(new SqlPara("SCFee", ConvertType.ToFloat(textEdit1.Text.Trim())));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_SHORTCONN_Fee", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    CommonClass.SetOperLog(SCId, "", textEdit1.Text.Trim(), CommonClass.UserInfo.UserName, "短途接驳", "短途接驳短波费修改操作");
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void btnLockStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void btnStyleCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void btnDelByCar_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (SCAcceptMan != "")
                {
                    MsgBox.ShowOK("本车已接收，不能剔除！");
                    return;
                }

                Guid id = new Guid(SCId);
                string SCBatchNo = U_SCBatchNo;

                if (MsgBox.ShowYesNo("确定作废本车？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCId", id));
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    //CommonClass.SetOperLog(U_SCBatchNo, "", "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳整车作废操作");
                    myGridView1.DeleteRow(rowhandle);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("确定取消接收本车？") != DialogResult.Yes) return;
            //取消接收
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", ""));
                list.Add(new SqlPara("type", 2));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    CommonClass.SetOperLog(SCBatchNo, "", "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳取消接收本车操作");
                    MsgBox.ShowOK();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SCBatchNo", U_SCBatchNo));
            string BillNoStr = "";
            try
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 1)
                        BillNoStr += ConvertType.ToString(myGridView1.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView1, "BillNo"))) + "@";
                }
                if (BillNoStr == "")
                {
                    MsgBox.ShowOK("请选择要取消到货的运单!");
                    return;
                }
                if (MsgBox.ShowYesNo("确定取消本车到货？") != DialogResult.Yes) return;
                list.Add(new SqlPara("BillNoStr", BillNoStr));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_CANCEL_ACCEPT_BILL_RECSHORTCONN", list)) == 0) return;
                MsgBox.ShowOK();
                if (U_SCBatchNo != "")
                    getShortCoonByID();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            getShortCoonByID();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            simpleButton3.Visible = e.Page == xtraTabPage2;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            simpleButton3.Enabled = false;
            Thread th = new Thread(() =>
            {
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;

                try
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dt.Rows[i]["ischecked"] = ConvertType.ToInt32(dt.Rows[i]["ischecked"]) == 0 ? 1 : 0;
                }
                catch { }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            simpleButton3.Enabled = true;
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }
    }
}