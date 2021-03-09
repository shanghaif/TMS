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
    public partial class frmIncomeExpendMonthDetail : BaseForm
    {
        public DataSet ds;
        //public DataSet ds
        //{
        //    get { return _ds; }
        //    set { _ds = value; }
        //}
        public frmIncomeExpendMonthDetail()
        {
            InitializeComponent();
        }

        private void frmIncomeExpendMonthDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.RestoreGridLayout(myGridView1, this.Text);
            myGridControl1.DataSource = ds.Tables[0];
        }
    }
}
