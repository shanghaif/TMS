using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI    
{
    public partial class fmModifyTime : BaseForm
    {
        public fmModifyTime()
        {
            InitializeComponent();
        }
        public DateTime shdate ;


        private void fmModifyTime_Load(object sender, EventArgs e)
        {
            shTime.DateTime = CommonClass.gcdate;
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            shdate = shTime.DateTime;
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            shdate = shTime.DateTime;
            this.Close();
        }
       
    }

}