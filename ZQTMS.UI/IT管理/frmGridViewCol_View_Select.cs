using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmGridViewCol_View_Select : BaseForm
    {
        public frmGridViewCol_View_Select()
        {
            InitializeComponent();
        }

        public DataSet ds = new DataSet();
        public GridView gv;

        private void frmGridViewCol_View_Select_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            gridControl1.DataSource = ds.Tables[0];
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GetColumnName();
        }

        private void GetColumnName()
        {
            int row = gridView1.FocusedRowHandle;
            if (row < 0) return;

            int rowhandle = gv.FocusedRowHandle;
            gv.SetRowCellValue(rowhandle, "ColGuid", gridView1.GetRowCellValue(row, "ColGuid").ToString());
            gv.SetRowCellValue(rowhandle, "ColCaption", gridView1.GetRowCellValue(row, "ColCaption").ToString());
            gv.PostEditor();
            gv.UpdateCurrentRow();
            this.Close();
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) GetColumnName();
        }
    }
}
