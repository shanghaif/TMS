using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.Lib
{
    [DefaultProperty("Text")]
    public partial class UCLabelBox : DevExpress.XtraEditors.XtraUserControl
    {
        public UCLabelBox()
        {
            InitializeComponent();
        }

        [Category("自定义属性"), Description("标签文本")]
        public string LabelText
        {
            get
            {
                return label.Text;
            }
            set
            {
                label.Text = value;
            }
        }

        [Category("自定义属性")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return textEdit1.Text;
            }
            set
            {
                textEdit1.Text = value;
            }
        }

        [Category("自定义属性")]
        public new int TabIndex
        {
            get
            {
                return textEdit1.TabIndex;
            }
            set
            {
                textEdit1.TabIndex = value;
            }
        }

        [Category("自定义属性")]
        public new bool Focused
        {
            get
            {
                return textEdit1.Focused;
            }
            set
            {
                if (value)
                {
                    textEdit1.Focus();
                }
            }
        }

        [Category("自定义属性")]
        public new bool Enabled
        {
            get
            {
                return textEdit1.Enabled;
            }
            set
            {
                textEdit1.Enabled = value;
            }
        }

        [Category("自定义属性"),DefaultValue(false)]
        public bool ReadOnly
        {
            get
            {
                return textEdit1.Properties.ReadOnly;
            }
            set
            {
                textEdit1.Properties.ReadOnly = value;
            }
        }

        [Category("自定义属性"), Description("LabelControl对象")]
        public LabelControl Label
        {
            get
            {
                return label;
            }
        }

        [Category("自定义属性"), Description("TextEdit对象")]
        public TextEdit TextBox
        {
            get
            {
                return textEdit1;
            }
        }

        [Category("自定义属性"), Description("按回车键是否移动到下一个控件(按TabIndex顺序)"), DefaultValue(false)]
        public bool EnterMoveNextControl
        {
            get
            {
                return textEdit1.EnterMoveNextControl;
            }
            set
            {
                textEdit1.EnterMoveNextControl = value;
            }
        }

        [Category("自定义属性"), DefaultValue(null)]
        public object EditValue
        {
            get
            {
                return textEdit1.EditValue;
            }
            set
            {
                textEdit1.EditValue = value;
            }
        }

        public override string ToString()
        {
            return textEdit1.Text;
        }

        public event EventHandler EditValueChanged;
        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (EditValueChanged != null)
            {
                EditValueChanged(sender, e);
            }
        }

        public event ChangingEventHandler EditValueChanging;
        private void textEdit1_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (EditValueChanging != null)
            {
                EditValueChanging(sender, e);
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler TextChanged;
        private void textEdit1_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }
        }

        private void UCLabelBox_Resize(object sender, EventArgs e)
        {
            int minHeight = 22, minWidth = 15;
            if (base.Size.Height < minHeight) base.Size = new Size(base.Size.Width, minHeight);
            if (textEdit1.Size.Width < minWidth) base.Size = new Size(label.Size.Width + minWidth, base.Size.Height);

            SizeF sizeF = new SizeF(0, 0);
            using (Graphics g = this.CreateGraphics())
            {
                g.PageUnit = GraphicsUnit.Pixel;
                StringFormat sf = new StringFormat();
                sf.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                sizeF = g.MeasureString(label.Text, label.Font, 500, sf);
                g.Dispose();
            }
            if (sizeF == new SizeF(0, 0)) return;

            label.Padding = new Padding(0, (int)((float)base.Size.Height - sizeF.Height) / 2, 2, 0);
        }

        private void label_Resize(object sender, EventArgs e)
        {
            int minWidth = 15;
            if (textEdit1.Size.Width < minWidth) base.Size = new Size(label.Size.Width + minWidth + 25, base.Size.Height);
        }
    }
}
