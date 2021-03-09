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
    public partial class frmVehicleArrive : BaseForm
    {
        DataSet ds = new DataSet();
        //commonclass cc = new commonclass();
        //private userright ur = new userright();


        public frmVehicleArrive()
        {
            InitializeComponent();
        }

        private void frmVehicleArrive_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
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
                list.Add(new SqlPara("ssite", esite));
                list.Add(new SqlPara("sweb", webname));
                list.Add(new SqlPara("esite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("eweb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("hastime", 1));
                list.Add(new SqlPara("iscancel", 0));
                list.Add(new SqlPara("type", "%%"));
                list.Add(new SqlPara("isarrived", "%到车%"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLVEHICLESTAR_EX", list);
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
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ssite","%%"));
                    list.Add(new SqlPara("sweb","%%"));
                    list.Add(new SqlPara("esite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("eweb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("bdate", bdate.DateTime));
                    list.Add(new SqlPara("edate", edate.DateTime));
                    list.Add(new SqlPara("hastime", 0));
                    list.Add(new SqlPara("iscancel", 0));
                    list.Add(new SqlPara("type", "%%" ));
                    list.Add(new SqlPara("isarrived", "%未到%"));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLVEHICLESTAR", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    myGridControl1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
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

                if (MsgBox.ShowYesNo("确定点到？") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("ID", GridOper.GetRowCellValueString(myGridView1, rowhandle, "ID")));
                string batchNo = GridOper.GetRowCellValueString(myGridView1, "BatchNO");//zxw 2016-12-26
                string carNo = GridOper.GetRowCellValueString(myGridView1, "VehicleNO");
                string arriveSite = GridOper.GetRowCellValueString(myGridView1, "ArriveSite");
                list.Add(new SqlPara("DepartureBatch", batchNo));
                list.Add(new SqlPara("carNo",carNo));
                list.Add(new SqlPara("arriveSite", arriveSite));
                list.Add(new SqlPara("type", 1));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "Upd_BILLVEHICLESTAR_Arrive", list);
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
  
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmAddVehicleStart ws = new frmAddVehicleStart();
            ws.dr_ = myGridView2.GetDataRow(rowhandle);
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
                List<SqlPara> list = new List<SqlPara>();

                string batchNo = GridOper.GetRowCellValueString(myGridView2, "DepartureBatch");//zxw 2016-12-27
                string carNo = GridOper.GetRowCellValueString(myGridView2, "CarNO");
                list.Add(new SqlPara("DepartureBatch", batchNo));
                list.Add(new SqlPara("carNO", carNo));
                list.Add(new SqlPara("type", 2));//1表示接收本车,2表示取消接收

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "Upd_BILLVEHICLESTAR_Arrive", list);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "到车记录");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;  
            frmDepartureRemark frm = new frmDepartureRemark();
            frm.DepartureBatch = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BatchNO");
            frm.ShowDialog();
            btnRefresh_Click(sender,e);
            myGridView1.FocusedRowHandle = rowhandle;
        }
    }
}