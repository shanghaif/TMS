using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KMS.Tool;


namespace KMS.UI
{
    public partial class FrmDataApplyMain :BaseForm
    {
            FrmDataApply da = new FrmDataApply();
          string modelname ="";
        public FrmDataApplyMain()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            modelname= simpleButton1.Text.Trim().ToString();
            da.modelName = modelname;
            da.ShowDialog();
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            modelname = simpleButton2.Text.Trim().ToString();
            da.modelName = modelname;
            da.Show();
            this.Close();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            modelname = simpleButton3.Text.Trim().ToString();
            da.modelName = modelname;
            da.Show();
            this.Close();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            modelname = simpleButton4.Text.Trim().ToString();
            da.modelName = modelname;
            da.Show();
            this.Close();
        }
    }
}
