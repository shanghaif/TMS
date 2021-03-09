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
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmBillVehicleStart : BaseForm
    {
        DataSet ds = new DataSet();

        public frmBillVehicleStart()
        {
            InitializeComponent();
        }

        private void frmBillVehicleStart_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("发车管理"); //xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            bsite.EditValue = CommonClass.UserInfo.SiteName;
            bweb.EditValue = CommonClass.UserInfo.WebName;
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {  
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmAddVehicleStart ws = new frmAddVehicleStart();
            ws.dr_ = myGridView1.GetDataRow(rowhandle);
            ws.ShowDialog();
 
            cbRetrieve.PerformClick();
        }

        private void cancel()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                if ((myGridView1.GetRowCellValue(rowhandle, "vehiclestarMan").ToString()) != CommonClass.UserInfo.UserName)
                {
                    MsgBox.ShowOK("只能删除自己的单据！");
                    return;
                }
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "ID").ToString());
                string BatchNO = (myGridView1.GetRowCellValue(rowhandle, "BatchNO").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLVEHICLESTAR", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();

                    //取消好多车司机打卡
                    CommonSyn.CancleHaoDuoCheSendDaKa(BatchNO, CommonClass.UserInfo.token);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAddVehicleStart wpc = new frmAddVehicleStart();
            wpc.ShowDialog();
            cbRetrieve_Click(null, null);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            cancel();
        }


        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView gv = myGridControl1.MainView as GridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string SCBatchNo = ConvertType.ToString(gv.GetFocusedRowCellValue("SCBatchNo"));
            if (SCBatchNo == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SHORTCONN_BYCAR_PRINT", new List<SqlPara> { new SqlPara("SCBatchNo", SCBatchNo) }));
            if (ds == null || ds.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("短驳清单", ds);
            fpr.ShowDialog();
        }


        private void btnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //GridOper.ShowAutoFilterRow(myGridView1);
            frmShortConnDelList frm = new frmShortConnDelList();
            frm.Show();

        }

        private void bsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(bweb, bsite.Text.Trim(), true);
        }

        private void esite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(eweb, esite.Text.Trim(), true);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "发车管理清单");
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime > edate.DateTime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ssite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("sweb", bweb.Text.Trim() == "全部" ? "%%" : bweb.Text.Trim()));
                list.Add(new SqlPara("eweb", eweb.Text.Trim() == "全部" ? "%%" : eweb.Text.Trim()));
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("hastime", 1));
                list.Add(new SqlPara("iscancel", 0));
                list.Add(new SqlPara("type", type.Text.Trim() == "全部" ? "%%" : type.Text));
                list.Add(new SqlPara("isarrived", isarrived.Text.Trim() == "全部" ? "%%" : ("%" + isarrived.Text + "%")));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLVEHICLESTAR_TWO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}