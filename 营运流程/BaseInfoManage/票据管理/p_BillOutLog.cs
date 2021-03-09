using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class p_BillOutLog : BaseForm
    {
        public p_BillOutLog()
        {
            InitializeComponent();
        }

        private void BillOutBboundForm_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("票据出库");//xj/2019/5/29
            deBeginTime.EditValue = CommonClass.gbdate;
            deEndTime.EditValue = CommonClass.gedate;
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(gvOutBbound, "票据管理出库");
            FixColumn fix = new FixColumn(gvOutBbound, barSubItem2);
        }

        private void sbtnSearch_Click(object sender, EventArgs e)
        {
            if (deBeginTime.DateTime.Date > deEndTime.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("btime", deBeginTime.DateTime.ToString()));
                list.Add(new SqlPara("etime", deEndTime.DateTime.ToString()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OUT_ALLBillCreateBy", list);
                gcOutBbound.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void d_ve(string str) { }

        //添加
        private void barbtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            p_BillOut bif = new p_BillOut();
            bif.type = 1;
            bif.ShowDialog();
        }

        //修改
        private void barbtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gvOutBbound.FocusedRowHandle;
                if (rowhandle < 0) return;
                string flag = gvOutBbound.GetRowCellValue(rowhandle, "oBatch").ToString();
                string bno = gvOutBbound.GetRowCellValue(rowhandle, "oBBno").ToString();
                string eno = gvOutBbound.GetRowCellValue(rowhandle, "oBEno").ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //导出Excel
        private void barbtnDerived_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gvOutBbound);
            //Common.CommonClass.ExportToExcel(gvOutBbound);
        }

        private void sbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barbtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gvOutBbound);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gvOutBbound);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gvOutBbound, "票据管理出库");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("票据管理出库");
        }

        private void barbtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraMessageBox.Show("不可删除", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gvOutBbound.FocusedRowHandle;
                if (rowhandle < 0)
                {
                    MsgBox.ShowOK("请选择需要调拨的批次!");
                    return;
                }
                string flag = gvOutBbound.GetRowCellValue(rowhandle, "oBatch").ToString();
                string bno = gvOutBbound.GetRowCellValue(rowhandle, "oBBno").ToString();
                string eno = gvOutBbound.GetRowCellValue(rowhandle, "oBEno").ToString();
                string webid = gvOutBbound.GetRowCellValue(rowhandle, "webid").ToString();
                string lingdangren = gvOutBbound.GetRowCellValue(rowhandle, "lingdangren").ToString();
                string billtype = gvOutBbound.GetRowCellValue(rowhandle, "oBillType").ToString();

                p_BillOut pp = new p_BillOut();
                pp.flag = flag;
                pp.bno = bno;
                pp.eno = eno;
                pp.web = webid;
                pp.lingdan = lingdangren;
                pp.bill = billtype;

                pp.type = 2;
                pp.ShowDialog();
                gvOutBbound.SetRowCellValue(rowhandle, "webid", pp.web);
                gvOutBbound.SetRowCellValue(rowhandle, "lingdangren", pp.lingdan);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            p_BillAllot_ByBill wp = new p_BillAllot_ByBill();
            wp.ShowDialog();
        }
    }
}