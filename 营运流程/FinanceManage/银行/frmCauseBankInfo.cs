using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmCauseBankInfo : BaseForm
    {
        public frmCauseBankInfo()
        {
            InitializeComponent();
        }
        DataSet dsshipper = new DataSet();//汇款客户资料

        private void frmCauseBankInfo_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("银行客户");//xj/2019/5/29
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(gridView1, "银行客户资料");
            getdata();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANKINFO_Cause", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                gridshow.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "银行客户资料");
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("银行客户资料");
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCauseBankInfoAdd wv = new frmCauseBankInfoAdd();
            wv.Show();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));
            frmCauseBankInfoAdd wv = new frmCauseBankInfoAdd();
            wv.id = id;
            wv.ShowDialog();
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;


            if (MsgBox.ShowYesNo("确定要删除当前记录？\r\n请注意：系统会记录操作日志!") == DialogResult.No) return;

            int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "DELETE_CAUSEBANKINFO", list);
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

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)gridshow.MainView);
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}