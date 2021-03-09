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
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class frmCountFetchToMonth : BaseForm
    {
        public frmCountFetchToMonth()
        {
            InitializeComponent();
        }
        public static string custname = "全部";
        public static string custtype = "发货人";
        public static string accstate = "";

        public static string acctype = "";

        string units = "";
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        private void w_count_customer_Load(object sender, EventArgs e)
        {
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            popupContainerEdit1.Text = CommonClass.UserInfo.SiteName;
            CommonClass.SetSite(popupContainerEdit1, true);

            BarMagagerOper.SetBarPropertity(bar3);

            GridOper.RestoreGridLayout(gridView2, "转月结对账-汇总");
            GridOper.RestoreGridLayout(gridView1, "转月结对账-明细");
        }
        private void getdatamain()
        {

            gridView2.ClearColumnsFilter();
            if (edacctype.Text == "全部") { acctype = "%%"; } else { acctype = edacctype.Text; }
            if (edaccstate.Text == "全部") { accstate = "全部"; } else { accstate = edaccstate.Text; }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("customertype", edcustomertype.Text.Trim()));

                list.Add(new SqlPara("bsite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("accstate", edaccstate.Text.Trim()));
                list.Add(new SqlPara("acctype", acctype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_FETCHTOMONTH_COUNT_MAIN", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                ////
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dc.DataType == typeof(decimal) || dc.DataType == typeof(float) || dc.DataType == typeof(double))
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ds.Tables[0].Rows[i][dc] = ds.Tables[0].Rows[i][dc] == DBNull.Value ? 0 : Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i][dc]), 2);
                        }
                    }
                }
                ////
                gridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getdata(string shipper)
        {
            gridView1.ClearColumnsFilter();
            if (edaccstate.Text == "全部") { accstate = "全部"; } else { accstate = edaccstate.Text; }
            if (edacctype.Text == "全部") { acctype = "%%"; } else { acctype = edacctype.Text; }
            try
            {
                custname = shipper;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("custname", custname));
                list.Add(new SqlPara("customertype", edcustomertype.Text.Trim()));

                list.Add(new SqlPara("bsite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("accstate", edaccstate.Text.Trim()));
                list.Add(new SqlPara("acctype", acctype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_FETCHTOMONTH_COUNT", list);
                ds1 = SqlHelper.GetDataSet(sps);
                if (ds1 == null || ds1.Tables.Count == 0) return;

                gridControl1.DataSource = ds1.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getdatamain();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void gridView2_Click(object sender, EventArgs e)
        {
            int handle = gridView2.FocusedRowHandle;
            if (handle >= 0)
            {
                string shipper = "";
                if(edcustomertype.Text.Trim() =="发货客户")
                {
                    shipper = gridView2.GetRowCellValue(handle, "shipper").ToString();
                    gridView1.GroupPanelText = "客户名称：" + shipper;
                }
                else
                {
                    shipper = gridView2.GetRowCellValue(handle, "webname").ToString();
                    gridView1.GroupPanelText = "申请网点：" + shipper;
                }
                getdata(shipper);
                units = "";
            }
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {
            popupContainerEdit1.Text = e.Node.Text;
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.AllowAutoFilter(gridView1);
            //cc.AllowAutoFilter(gridView2);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, "转月结对账-汇总", gridView1, "转月结对账-明细");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView2, "转月结对账-汇总");
            GridOper.DeleteGridLayout(gridView1, "转月结对账-明细");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1, gridView2);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("转月结对账(汇总).grf", ds);
            fpr.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("转月结对账(明细).grf", ds1);
            fpr.ShowDialog();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.ExportToExcel(gridView1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "转月结对账明细");
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2, "转月结对账汇总");
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //try
            //{
            //    int rowhandle = e.FocusedRowHandle;
            //    if (rowhandle < 0) return;
            //    DataTable dt = (DataTable)gridControl1.DataSource;
            //    if (!dt.Columns.Contains("ysyf")) return;
            //    decimal ysyf = gridView1.GetRowCellValue(rowhandle, "ysyf") == DBNull.Value ? 0 : Convert.ToDecimal(gridView1.GetRowCellValue(rowhandle, "ysyf"));

            //    gridColumn52.OptionsColumn.AllowEdit = ysyf == 0; //基本运费
            //    gridColumn52.OptionsColumn.AllowFocus = ysyf == 0;
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }



        private void edcustomertype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (edcustomertype.SelectedIndex == 1)
            {
                edacctype.Text = "全部";
            }
            else
            {
                edacctype.Text = "提付";
            }
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            int unit = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "unit"));
            units += unit.ToString() + ",";
            units = units.TrimEnd(',');
            gridView1.DeleteRow(rowhandle);
        }

        private void gridView1_MouseUp(object sender, MouseEventArgs e)
        {
            //cc.ShowPopupMenu(gridView1, e, popupMenu1);
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            CommonClass.ShowBillSearch(GridOper.GetRowCellValueString(gridView1, "billno"));
        }
    }
}