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
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class JMfrmShortConnDelList : BaseForm
    {
        public JMfrmShortConnDelList()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            string proc = "QSP_GET_BILLSHORTCONNDELLIST";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("type", type.Text.Trim() == "全部"  ? "%%" : type.Text.Trim()));
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) (myGridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSendRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        private void barCheckItem1_CheckedChanged_1(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "到货货量统计");
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barbtnPrintQSD_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null) return;

            JMfrmAppointmentSend frm = new JMfrmAppointmentSend();
            frm.crrBillNO = dr["BillNO"].ToString();
            frm.ShowDialog();
        }
    }
}