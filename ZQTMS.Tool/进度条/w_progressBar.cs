using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public partial class w_progressBar : Form
    {
        public w_progressBar()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 进度条
        /// </summary>
        /// <param name="count">执行的步书，一般为循环的次数</param>
        /// <param name="title">显示的提示信息</param>
        /// <param name="bf">form窗体，注册OnUpLoad事件用，主要用于form窗体循环过程中更新进度条信息</param>
        public w_progressBar(int count, string title, BaseForm bf)
        {
            InitializeComponent();
        }

        public w_progressBar(int count, string title)
        {
            InitializeComponent();
            _count = count;
            step = maximum / _count;
        }

        bool close = false;

        private int _count = 0;
        private float step = 10;
        private int maximum = 10000;
        public delegate void MyInvoke(string str);
        public delegate void MyInvoke1(int num);

        /// <summary>
        /// 提取过程是否出现异常
        /// </summary>
        public static bool IsException = false;  //提取过程是否出现异常
        public static bool CloseWindow = false;  //关闭本窗体

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (IsException)
            {
                timer1.Enabled = false;
                this.Close();
                return;
            }
            if (CloseWindow)
            {
                timer1.Enabled = false;
                this.Close();
                return;
            }
        }

        private void w_progress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && (e.KeyCode == Keys.F4))
            {
                close = true;
            }
        }

        private void w_progress_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetlblStep("100");
            e.Cancel = close;
            close = false;
        }

        private void w_progress_Load(object sender, EventArgs e)
        {
            //设置一个最小值
            progressBarControl1.Properties.Minimum = 0;
            //设置一个最大值
            progressBarControl1.Properties.Maximum = maximum;
            //设置步长，即每次增加的数
            progressBarControl1.Properties.Step = Convert.ToInt32(step);
            IsException = false;
            CloseWindow = false;
            timer1.Enabled = true;
        }

        public void progressBarStep(int step)
        {
            string stepNum;
            if (progressBarControl1.InvokeRequired)
            {
                MyInvoke1 _myinvoke = new MyInvoke1(progressBarStep);
                progressBarControl1.Invoke(_myinvoke, new object[] { step });
            }
            else
            {
                if (_count != 0)
                {
                    progressBarControl1.PerformStep();
                    stepNum = (Convert.ToDouble(step) * 100 / Convert.ToDouble(_count)).ToString("f1");
                    SetlblStep(stepNum);
                }
            }
        }

        private void SetlblStep(string stepNum)
        {
            //在线程里以安全方式调用控件
            if (lblStep.InvokeRequired)
            {
                MyInvoke _myinvoke = new MyInvoke(SetlblStep);
                lblStep.Invoke(_myinvoke, new object[] { stepNum });
            }
            else
            {
                lblStep.Text = stepNum + "%";
            }
        } 
    }
}
