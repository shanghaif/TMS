using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmSelectPrintDepartureListPre : BaseForm
    {
        /// <summary>
        /// 要打印的站点
        /// </summary>
        public string printSite { get; set; }
        /// <summary>
        /// 打印的清单
        /// </summary>
        public int printType { get; set; }

        public frmSelectPrintDepartureListPre()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int count = checkedListBox1.CheckedItems.Count;
            if (count == 0)
            {
                XtraMessageBox.Show("至少选择一个站点打印!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            printType = radioGroup2.SelectedIndex;
            printSite = "";
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                printSite += checkedListBox1.CheckedItems[i] + "@";
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int a = radioGroup1.SelectedIndex;
            if (a == 0)
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemCheckState(i, CheckState.Checked);
                }
            }
            groupBox2.Enabled = a == 1;
        }
    }
}