using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using Newtonsoft.Json;
using System.IO;
using DevExpress.XtraBars;
using ZQTMS.Lib;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmPackageToZXDetail : ZQTMS.Tool.BaseForm
    {
        string _departureBatch;
        int _hitorder;
        DateTime _arriveddate;
        GridColumn gcBespeakContent;
        GridColumn gcIsseleckedMode;
        List<int> editRows = new List<int>();

        public int Hitorder
        {
            get { return _hitorder; }
            set { _hitorder = value; }
        }

        public string DepartureBatch
        {
            get { return _departureBatch; }
            set { _departureBatch = value; }
        }

        public DateTime Arriveddate
        {
            get { return _arriveddate; }
            set { _arriveddate = value; }
        }

        public frmPackageToZXDetail()
            : this("")
        {
            //InitializeComponent();
        }

        public frmPackageToZXDetail(string PID)
        {
            InitializeComponent();
            this.PID = PID;
            this.Batch=PID;
        }

        /// <summary>
        /// 分拨编号
        /// </summary>
        string PID;
        string Batch;

        private void frmPackageToZXDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false);
            CommonClass.GetGridViewColumns(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1);
            GridOper.RestoreGridLayout(myGridView1);
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");



            if (Batch != "") myGridControl1.DataSource = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_FB_DETAIL", new List<SqlPara> { new SqlPara("Batch", Batch) }));
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //zb20190509
        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }

        //zb20190509
        private void barbtnPrintGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNo")) + "@";
            }
            PrintQSD(str);
            
        }

        /// <summary>
        /// zb20190510
        /// </summary>
        /// <param name="BillNoStr"></param>
        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("DepartureBatch", DepartureBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            frmRuiLangService.Print("提货单分拨", ds.Tables[0], CommonClass.UserInfo.gsqc);
        }



        //zb20190509
        private void barbtnPrintQSD_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNo")) + "@";
            }
            PrintQSD(str);
        }
        //zb20190510

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            int a = checkEdit1.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
    }
}