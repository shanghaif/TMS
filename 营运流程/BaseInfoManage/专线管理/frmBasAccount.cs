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
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmBasAccount : BaseForm
    {
        public frmBasAccount()
        {
            InitializeComponent();
        }



        private void frmBasAccount_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("专线账户管理");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            getdate();
        }



        private void getdate()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASACCOUNT");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



      


        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBasAccount_add frm = new frmBasAccount_add();
            frm.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "AccountID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASACCOUNT_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmBasAccount_add frm = new frmBasAccount_add();
                frm.dr = dr;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "AccountID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASACCOUNT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                    getdate();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdate();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem2.PerformClick();
        }
    }
}
