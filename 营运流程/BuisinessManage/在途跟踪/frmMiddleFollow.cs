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

namespace ZQTMS.UI
{
    public partial class frmMiddleFollow : BaseForm
    {
        public frmMiddleFollow()
        {
            InitializeComponent();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmMiddleFollow_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("中转跟踪");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView1, "state", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已签收", Color.FromArgb(192, 255, 192));//已签收
            GridOper.CreateStyleFormatCondition(myGridView1, "state", DevExpress.XtraGrid.FormatConditionEnum.Equal, "跟踪结束", Color.LimeGreen);//跟踪结束
            
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            bsite.EditValue = CommonClass.UserInfo.SiteName;
            esite.EditValue = "全部";
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
            if (CommonClass.UserInfo.companyid == "486")
            {
                GridOper.CreateStyleFormatCondition(myGridView1, "TraceDate_state", DevExpress.XtraGrid.FormatConditionEnum.Equal, "1", Color.Yellow);//开始跟踪显示黄色
            }

        }

        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("MiddleTraceState", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_MiddleFollow");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
            
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                //自动列宽
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            freshData();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            frmMiddleFollow_ADD frm = new frmMiddleFollow_ADD();
            frm.billNo = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
            frm.sPossiblArrivalTime = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "PossiblArrivalTime"));
            frm.sArrivalTime = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ArrivalTime"));
            frm.sMiddleSendTime = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "MiddleSendTime"));
            frm.MiddleTraceState = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "MiddleTraceState"));
            frm.sConsignorCellPhone = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsignorCellPhone"));
            frm.sConsignorPhone = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsignorPhone"));
            frm.sConsignorName = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsignorName"));
            frm.sConsigneeCellPhone = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsigneeCellPhone"));
            frm.sConsigneePhone = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsigneePhone"));
            frm.sConsigneeName = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "ConsigneeName"));
            frm.Gv = this.myGridView1;
            frm.ShowDialog();
            //freshData();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem3_ItemClick(sender, null);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "中转在途跟踪");
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            frmBillSearchControl.ShowBillSearch(ConvertType.ToString(myGridView1.GetFocusedRowCellValue("BillNo")));
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }
    }
}