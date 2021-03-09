using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBankInfo : BaseForm
    {
        DataSet dsshipper = new DataSet();//汇款客户资料

        public frmBankInfo()
        {
            InitializeComponent();
        }

        private void frmBankInfo_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(gridView1, "银行客户资料");
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANKINFO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                gridshow.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "银行客户资料");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("银行客户资料");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBankInfoAdd wv = new frmBankInfoAdd();
            wv.Show();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)gridshow.MainView);
            // cc.ExportToExcel((GridView)gridshow.MainView); 
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));
            frmBankInfoAdd wv = new frmBankInfoAdd();
            wv.id = id;
            wv.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;


            if (MsgBox.ShowYesNo("确定要删除当前记录？\r\n请注意：系统会记录操作日志!") == DialogResult.No) return;

            int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "DELETE_BANKINFO", list);
                if (SqlHelper.ExecteNonQuery(sps) == 1)
                {
                    gridView1.DeleteRow(rowhandle);

                    string bankman = gridView1.GetRowCellValue(rowhandle, "bankman").ToString();
                    string bankcode = gridView1.GetRowCellValue(rowhandle, "bankcode").ToString();
                    string opertype = gridView1.GetRowCellValue(rowhandle, "opertype").ToString();

                    decimal accin = Convert.ToDecimal(gridView1.GetRowCellValue(rowhandle, "accin"));
                    decimal accout = Convert.ToDecimal(gridView1.GetRowCellValue(rowhandle, "accout"));

                    //cc.Log(0, accin + accout, "删除银行汇款", string.Format("开户名：{0} 账号：{1} 转账类型：{2}", bankman, bankcode, opertype));
                    gridView1.DeleteRow(rowhandle);
                    MsgBox.ShowOK();
                }
                else
                    MsgBox.ShowError("操作失败！");

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

    }
}




