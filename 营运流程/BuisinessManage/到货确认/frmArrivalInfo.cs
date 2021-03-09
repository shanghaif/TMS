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
    public partial class frmArrivalInfo : BaseForm
    {
        public frmArrivalInfo()
        {
            InitializeComponent();
        }
        public DataSet ds = new DataSet();

        private void frmArrivalInfo_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            myGridControl1.DataSource = ds.Tables[0];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("该单已经做过【强制点到】你确定要继续加入本车吗？") == DialogResult.No) return;
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }
    }
}