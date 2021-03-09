using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace ZQTMS.Tool
{
    public partial class w_import_select_table : BaseForm
    {
        public w_import_select_table()
        {
            InitializeComponent();
        }

        public DataTable dt;
        private void w_import_select_table_Load(object sender, EventArgs e)
        {
            if (dt.Columns.Count == 0) return;
            listBoxControl1.DataSource = dt;
            listBoxControl1.DisplayMember = dt.Columns[2].ColumnName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (listBoxControl1.SelectedItems.Count == 1)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("请选择表格!");
            }
        }

        private void listBoxControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick();
        }
    }
}