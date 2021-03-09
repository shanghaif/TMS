using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmInternalType : BaseForm
    {
        public frmInternalType()
        {
            InitializeComponent();
        }

        private void frmInternalType_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
            getdata();
        }
        public void getdata()
        {
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_InternalType");
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                myGridControl1.DataSource = ds.Tables[0];
            }

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmTypeAdd frm = new frmTypeAdd();
            frm.type = 0;
            frm.ShowDialog();
            getdata();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            frmTypeAdd frm = new frmTypeAdd();
            frm.type = 1;
            frm.id = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            frm.insideType = myGridView1.GetRowCellValue(rowhandle, "InsideType").ToString();
            frm.one = myGridView1.GetRowCellValue(rowhandle, "One").ToString();
            frm.two = myGridView1.GetRowCellValue(rowhandle, "Two").ToString();
            frm.three = myGridView1.GetRowCellValue(rowhandle, "Three").ToString();
            frm.four = myGridView1.GetRowCellValue(rowhandle, "Four").ToString();
            frm.ShowDialog();
            getdata();

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string id = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ID", id));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DELInternalType", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                getdata();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}