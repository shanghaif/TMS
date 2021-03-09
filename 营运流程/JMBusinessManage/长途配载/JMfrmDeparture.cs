using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class JMfrmDeparture : BaseForm
    {
        public JMfrmDeparture()
        {
            InitializeComponent();
        }

        private void frmDeparture_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(bSite, true);
            CommonClass.SetSite(eSite, true);
            CommonClass.SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "isover", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.LightGreen);
        }

        private void freshData()
        {
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("bSite", bsite));
                list.Add(new SqlPara("eSite", esite));
                list.Add(new SqlPara("webName", webName));
                list.Add(new SqlPara("LoadingType", LoadingType.SelectedIndex));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                gridDepartureList.DataSource = ds.Tables[0];
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

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            JMfrmDepartureAdd wn = new JMfrmDepartureAdd();
            wn.Show();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            freshData();
        }

        private void barBtnMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            JMfrmDepartureMody frm = new JMfrmDepartureMody();
            frm.sDepartureBatch = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("DepartureBatch"));
            frm._arriveDate = myGridView1.GetFocusedRowCellValue("ArrivedDate") == DBNull.Value ? null : myGridView1.GetFocusedRowCellValue("ArrivedDate");
            frm.ShowDialog();
            btnRetrieve.PerformClick();
        }

        private void barBtnVehicleDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ArrivedDate")) != "")
                {
                    MsgBox.ShowOK("本车已经到货,不能作废!\r\n如果确实需要作废,请联系本车到站负责人员取消本车到货!");
                    return;
                }
                string departureBatch = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");

                if (MsgBox.ShowYesNo("确定作废本车？\r\n批次：" + departureBatch) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定作废本车？请三思！！\r\n批次：" + departureBatch) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTURE", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("本车作废成功!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void gridDepartureList_DoubleClick(object sender, EventArgs e)
        {
            if (barBtnMod.Enabled) barBtnMod.PerformClick();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void barBtnvExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "配载记录");
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

        private void barBtnLoadScan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            JMfrmScanStatistics fss = new JMfrmScanStatistics();
            fss.dtvehicleno = GridOper.GetRowCellValueString(myGridView1, "CarNo");
            fss.dtinoneflag = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            fss.dtchauffer = GridOper.GetRowCellValueString(myGridView1, "DriverName");
            fss.dtsenddate = ConvertType.ToDateTime(GridOper.GetRowCellValueString(myGridView1, "DepartureDate"));
            fss.Show();
        }

        private void barBtnLoadScanPeop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            JMfrmScanBillMan fsb = JMfrmScanBillMan.Get_frmScanBillMan;
            fsb.MdiParent = this.MdiParent;
            fsb.Dock = DockStyle.Fill;
            fsb.Show();
            fsb.Focus();
        }
    }
}