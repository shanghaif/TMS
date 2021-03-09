using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;

namespace ZQTMS
{
    public partial class JMw_sendback_load : BaseForm
    {

        private DataSet dataset1 = new DataSet();


        public JMw_sendback_load()
        {
            InitializeComponent();
        }

        private void getdata()
        {

        }

        private void w_package_load_Load(object sender, EventArgs e)
        {



        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            NowFetch();
        }

        private void NowFetch()
        {

        }

        private void 查看运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 打印运单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            NowFetch();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.SaveGridLayout(gridControl1, "送货签收", true);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.DeleteGridLayout(gridControl1, "送货签收");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.ExportToExcel(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void repositoryItemTextEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["billno"].FilterInfo = new ColumnFilterInfo("[unit] LIKE " + "'%" + szfilter + "%'" + " OR [billno] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void repositoryItemTextEdit1_Enter(object sender, EventArgs e)
        {
        }

        private void repositoryItemTextEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            NowFetch();
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            (barEditItem1.Links[0] as BarEditItemLink).ShowEditor();
            if (barManager1.ActiveEditor != null)
                barManager1.ActiveEditor.Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void repositoryItemTextEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.KeyChar = cc.NumChange(e.KeyChar);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //userright ur = new userright();
            //if (!ur.GetUserRightDetail888("a108"))
            //{
            //    commonclass.MsgBox.ShowOK("没有取消签收权限!");
            //    barButtonItem9.Enabled = false;
            //    return;
            //}
            JMw_sendback_cancel ws = new JMw_sendback_cancel();
            ws.Show();
        }

        private void barEditItem2_EditValueChanged(object sender, EventArgs e)
        {
            //cc.FillRepWebByParent(repositoryItemComboBox2, barEditItem2.EditValue.ToString(), true);
        }
    }
}