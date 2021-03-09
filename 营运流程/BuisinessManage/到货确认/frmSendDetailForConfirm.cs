using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using System.Data.OleDb;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmSendDetailForConfirm : BaseForm
    {
        string _departureBatch;
        public frmSendDetailForConfirm()
        {
            InitializeComponent();
        }
        public string DepartureBatch
        {
            get { return _departureBatch; }
            set { _departureBatch = value; }
        }
        public GridView gv;
        List<int> EditRows = new List<int>();

        private void w_send_detail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView2);
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);

            getdata();
        }

        private void getdata()
        {
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string sendbatch = "";
            try
            {
                //填写送货信息
                sendbatch = ConvertType.ToString(gv.GetFocusedRowCellValue("SendBatch"));
                edinoneflag.Text = sendbatch;
                edvehicleno.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendCarNO"));
                edsendman.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriver"));
                edsendmantel.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDriverPhone"));
                edsenddate.DateTime = ConvertType.ToDateTime(gv.GetFocusedRowCellValue("SendDate"));
                edsendtosite.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToSite"));
                edsendtoweb.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendToWeb"));
                edsendmadeby.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendOperator"));
                edsendremark.Text = ConvertType.ToString(gv.GetFocusedRowCellValue("SendDesc"));

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", sendbatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "送货清单");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private bool CheckHexiao(ref string result)
        {
            return false;
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            string sendBatch = edinoneflag.Text.Trim();
            if (sendBatch == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SEND_DETAIL_PRINT", new List<SqlPara> { new SqlPara("SendBatch", sendBatch) }));
            if (ds == null || ds.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("送货清单.grf", ds);
            fpr.ShowDialog();
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("DepartureBatch", DepartureBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    //fprl = new frmPrintRuiLang("提货单", dt);
            //    //fprl.ShowDialog();
            //}
            //frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            //jl20181127
            if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                || CommonClass.UserInfo.WebName == "杭州操作部"
                || CommonClass.UserInfo.WebName == "杭州操作部1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "宁波操作部"
                || CommonClass.UserInfo.WebName == "宁波操作部1"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                || CommonClass.UserInfo.WebName == "广州操作部"
                || CommonClass.UserInfo.WebName == "广州操作部1"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
            {
                frmRuiLangService.Print("提货单大坪", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
            else
            {
                frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
        }

        private void barButtonItem9_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] rows = myGridView2.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(rows[i], "BillNo")) + "@";
            }
            PrintQSD(str);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
            }
            PrintQSD(str);
        }

        
    }
}