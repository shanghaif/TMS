using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_ygkq_modified : BaseForm
    {
        public w_ygkq_modified()
        {
            InitializeComponent();
        }

        private void w_ygkq_modified_Load(object sender, EventArgs e)
        {
            dateEdit1.EditValue = CommonClass.gcdate;
        }
    }
}