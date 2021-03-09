using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;


namespace ZQTMS.UI
{
    public partial class frmOtherFeeList : BaseForm
    {
        DataSet ds = new DataSet();
        //commonclass cc = new commonclass();
        //private userright ur = new userright();
        string sOtherState = "始发";

        public frmOtherFeeList()
        {
            InitializeComponent();
        }

        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("始发其他费登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            CommonClass.SetSite(endSite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            endSite.EditValue = CommonClass.UserInfo.SiteName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("OtherState", sOtherState));
                list.Add(new SqlPara("SiteName", endSite.Text.Trim() == "全部" ? "%%" : endSite.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLOTHERFEE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            endSite.Text = e.Node.Text;
        }

        private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            endSite.Text = e.Node.Text;
        }

        private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        {
            endSite.ClosePopup();
        }

        private void tvesite_MouseClick(object sender, MouseEventArgs e)
        {
            endSite.ClosePopup();
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {


        }

        private void cancel()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                if ((myGridView1.GetRowCellValue(rowhandle, "SCWeb").ToString()) != CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowOK("请在短驳网点进行此操作！");
                }
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "SCId").ToString());
                string SCBatchNo = (myGridView1.GetRowCellValue(rowhandle, "SCBatchNo").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
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

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.SaveGridLayout(gridshow, "短途接驳记录", true);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAddShortconnect wpc = new frmAddShortconnect();
            wpc.ShowDialog();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            cancel();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "始发其他费用明细");
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = gridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;

            //w_扫描统计 ws = new w_扫描统计();
            //ws.dtvehicleno = gridView1.GetRowCellValue(rowhandle, "dtvehicleno").ToString();
            //ws.dtchauffer = gridView1.GetRowCellValue(rowhandle, "dtchauffer").ToString();
            //ws.dtsenddate = Convert.ToDateTime(gridView1.GetRowCellValue(rowhandle, "dtsenddate"));
            //ws.dtinoneflag = gridView1.GetRowCellValue(rowhandle, "dtinoneflag") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "dtinoneflag").ToString();
            //ws.type = 1;
            //ws.ShowDialog();
        }
        private void btnLockStyle_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void btnStyleCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            //w_扫描人统计 wv = new w_扫描人统计();
            //wv.Text = "短驳卸货按扫描人统计";

            //foreach (Form form in this.MdiParent.MdiChildren)
            //{
            //    if (form.GetType() == typeof(w_扫描人统计) && form.Text == wv.Text)
            //    {
            //        form.Focus();
            //        return;
            //    }
            //}

            //wv.MdiParent = this.MdiParent;
            //wv.Dock = DockStyle.Fill;
            //wv.opertype = 1;
            //wv.Show();
        }

        private void endSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, endSite.EditValue.ToString());
        }

        private void btnAddOtherFee_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmOtherFeeAdd frm = new frmOtherFeeAdd();
            frm.ShowDialog();
        }

        private void barButtonItem11_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "AuditState")) == 1)
            {
                MsgBox.ShowOK("费用已核销!不能修改!");
                return;
            }
            frmOtherFeeAdd frm = new frmOtherFeeAdd();
            frm.sBillno = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
            frm.sOID = myGridView1.GetRowCellValue(rowhandle, "OID").ToString();
            frm.ShowDialog();
        }

        private void barButtonItem15_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "AuditState")) == 1)
                {
                    MsgBox.ShowOK("费用已核销!不能修改!");
                    return;
                }

                if (MsgBox.ShowYesNo("确定删除本条费用？") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OID", myGridView1.GetRowCellValue(rowhandle, "OID").ToString()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLOTHERFEE", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("删除成功!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }
    }
}