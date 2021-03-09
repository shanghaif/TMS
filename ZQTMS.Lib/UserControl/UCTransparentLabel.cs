using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.Lib
{
    public partial class UCTransparentLabel : Control
    {
        private readonly Timer refresher;
        private Image _image;

        public UCTransparentLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Size = new Size(100, 54);
            BackColor = Color.Transparent;
            refresher = new Timer();
            refresher.Tick += TimerOnTick;
            refresher.Interval = 50;
            refresher.Enabled = true;
            refresher.Start();
            this.BringToFront();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (_image != null)
            {
                e.Graphics.DrawImage(_image, (Width / 2) - (_image.Width / 2), (Height / 2) - (_image.Height / 2));
                return;
            }

            if (!string.IsNullOrEmpty(this.Text))
            {
                string txt = this.Text.Trim();
                Graphics g = e.Graphics;
                Font font = new Font("黑体", 25, FontStyle.Bold, GraphicsUnit.Pixel);

                float angle = -30; //旋转角度
                g.ResetTransform();
                Pen pen = new Pen(Color.Red, 3);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                SizeF size = g.MeasureString(txt, font);

                g.TranslateTransform((this.Size.Width - size.Width) / 2 - 2, (this.Size.Height - size.Height) / 2 + 22);
                g.RotateTransform(angle); //旋转
                g.DrawString(txt, font, Brushes.Red, new PointF(1, 4));
                g.DrawRectangle(pen, new Rectangle(new Point(1, 1), size.ToSize()));
                this.BringToFront();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do not paint background
        }

        public void Redraw()
        {
            RecreateHandle();
        }

        private void TimerOnTick(object source, EventArgs e)
        {
            RecreateHandle();
            refresher.Stop();
        }

        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                RecreateHandle();
            }
        }
    }
}
