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
    public partial class JMfrmShortConnInbound : BaseForm
    {
        DataSet ds = new DataSet();

        public JMfrmShortConnInbound()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(endSite, true);
            endSite.EditValue = CommonClass.UserInfo.SiteName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
            getdata();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string esite = endSite.Text == "全部" ? "%%" : endSite.Text;
            string webname = WebName.Text == "全部" ? "%%" : WebName.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDesSite", esite));
                list.Add(new SqlPara("SCDESWeb", webname));
                list.Add(new SqlPara("begDate", bdate.DateTime));
                list.Add(new SqlPara("endDate", edate.DateTime));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_ARRIVED", list);
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

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
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

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            //获取本车明细
            if (myGridView1.FocusedRowHandle < 0) return;
            JMfrmRecShortConn frm = new JMfrmRecShortConn();
            frm.SCId = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId"));
            frm.U_SCBatchNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo"));
            frm.U_SCDate = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate"));
            frm.U_SCSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite"));
            frm.U_SCWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb"));
            frm.U_SCCarNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo"));
            frm.U_SCDriver = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver"));
            frm.U_SCDesSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite"));
            frm.U_SCDesWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb"));
            frm.U_SCDContolMan = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDContolMan"));
            frm.ShowDialog();
        }

        private void endSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, endSite.EditValue.ToString());
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCDesSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("SCDESWeb", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_INROAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //接收本车
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "SCBatchNo");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("type", 1));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            #region 直接接收车辆,不需要看明细了
            /*
            if (ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCState")) != "未接")
                return;
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            frmRecShortConn frm = new frmRecShortConn();
            frm.SCId = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCId"));
            frm.U_SCBatchNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCBatchNo"));
            frm.U_SCDate = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDate"));
            frm.U_SCSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCSite"));
            frm.U_SCWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCWeb"));
            frm.U_SCCarNo = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCCarNo"));
            frm.U_SCDriver = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDriver"));
            frm.U_SCDesSite = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesSite"));
            frm.U_SCDesWeb = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDesWeb"));
            frm.U_SCDContolMan = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SCDContolMan"));
            frm.isMod = true;
            frm.SCId = myGridView1.GetRowCellValue(rowhandle, "SCId").ToString();
            frm.ShowDialog();
            getdata();
            */
            #endregion
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView2.FocusedRowHandle < 0) return;
            JMfrmRecShortConn ws = new JMfrmRecShortConn();
            ws.U_SCBatchNo = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCBatchNo").ToString();
            ws.U_SCDate = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDate").ToString();
            ws.U_SCSite = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCSite").ToString();
            ws.U_SCWeb = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCWeb").ToString();
            ws.U_SCCarNo = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCCarNo").ToString();
            ws.U_SCDriver = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDriver").ToString();
            ws.U_SCDesSite = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDesSite").ToString();
            ws.U_SCDesWeb = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCDesWeb").ToString();
            ws.U_SCDContolMan = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCMan").ToString();
            ws.SCId = myGridView2.GetRowCellValue(myGridView2.FocusedRowHandle, "SCId").ToString();
            ws.isArrivedInBound = true;
            ws.ShowDialog();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短驳在途车辆");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("确定取消接收本车？") != DialogResult.Yes) return;
            //取消接收
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                string SCBatchNo = GridOper.GetRowCellValueString(myGridView2, rowhandle, "SCBatchNo");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCBatchNo", SCBatchNo));
                list.Add(new SqlPara("SCAcceptMan", ""));
                list.Add(new SqlPara("type", 2));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RECSHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    myGridView2.DeleteRow(rowhandle);
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        private void barButtonItem20_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}