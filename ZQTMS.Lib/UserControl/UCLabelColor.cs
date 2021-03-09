using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ZQTMS.Lib
{
    public partial class UCLabelColor : DevExpress.XtraEditors.XtraUserControl
    {
        public UCLabelColor()
        {
            InitializeComponent();
            lbText.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            lbColor.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            lbText.BackColor = Color.White;
            this.BorderStyle = BorderStyle.None;
        }

        public Color ItemColor
        {
            get { return lbColor.BackColor; }
            set { lbColor.BackColor = value; }
        }

        public string ItemText
        {
            get { return lbText.Text; }
            set { lbText.Text = " " + value; }
        }

        public override string ToString()
        {
            return lbText.Text;
        }
    }
}
