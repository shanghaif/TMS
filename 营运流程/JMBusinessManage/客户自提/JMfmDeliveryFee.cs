using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfmDeliveryFee : BaseForm
    {
        public JMfmDeliveryFee()
        {
            InitializeComponent();

        }

        private void JMfmDeliveryFee_Load(object sender, EventArgs e)
        { 
            //CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1); 
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                myGridControl1.DataSource = CommonClass.dsSendPrice.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void JMfmDeliveryFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


 

    }
}
