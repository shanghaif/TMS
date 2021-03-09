using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Lib;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class fmbasTerminalOutFee : BaseForm
    {
        public fmbasTerminalOutFee()
        {
            InitializeComponent();
        }

        private void fmbasTerminalOutFee_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);

            BarMagagerOper.SetBarPropertity(bar1);

            getData();
        }

        private void myGridControl1_Click(object sender, EventArgs e)
        {

        }

        private void getData()
        {
            try
            {
                List<SqlPara> lst = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basTerminalOutFee", lst);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// xin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmbasTerminalOutFeeAdd frm = new fmbasTerminalOutFeeAdd();
            frm.id = Guid.Empty;
            frm.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int row = myGridView1.FocusedRowHandle;
                if (row < 0)
                {
                    return;
                }

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }

                Guid id = new Guid(myGridView1.GetRowCellValue(row, "basTerminalOutFeeID").ToString());
                List<SqlPara> lst = new List<SqlPara>();
                lst.Add(new SqlPara("basTerminalOutFeeID", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_basTerminalOutFee", lst);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(row);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }

            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int selectRow = myGridView1.FocusedRowHandle;
                if (selectRow < 0)
                {
                    return;
                }

                Guid id = new Guid(myGridView1.GetRowCellDisplayText(selectRow, "basTerminalOutFeeID").ToString());

                fmbasTerminalOutFeeAdd frm = new fmbasTerminalOutFeeAdd();
                frm.id = id;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getData();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


    }
}
