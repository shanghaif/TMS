using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZQTMS.Tool
{
    public partial class MsgForm : DevExpress.XtraEditors.XtraForm
    {
        public MsgForm()
        {
            InitializeComponent();

            lbTitle.Text = this.Title;
            lbContent.Text = this.Content;
        }

        /// <summary>
        /// 显示在弹框上的提示标题
        /// </summary>
        public string Title
        {
            get 
            { 
                return lbTitle.Text.Trim(); 
            }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    return; 
                }
                lbTitle.Text = value; 
            }
        }

        /// <summary>
        /// 显示在弹框上的提示内容
        /// </summary>
        public string Content
        {
            get 
            {
                return lbContent.Text.Trim(); 
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                lbContent.Text = value;
            }
        }

        public MsgButtons MsgButton
        {
            set
            {
                switch (value)
                {
                    case MsgButtons.OK:
                        btn1.Visible = btn3.Visible = false;
                        btn2.Visible = true;
                        btn2.Text = "确定(&O)";
                        break;
                    case MsgButtons.OKCancel:
                        break;
                    case MsgButtons.YesNo:
                        break;
                    case MsgButtons.YesNoCancel:
                        break;
                    default:
                        this.Content += "\r\n按钮类型创建错误!";
                        break;
                }
                
            }
        }

        public enum MsgButtons
        {
            OK,
            OKCancel,
            YesNo,
            YesNoCancel
        }

        private void MsgForm_Load(object sender, EventArgs e)
        {
            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lbContent.Text.Trim() == "") return;
            Clipboard.SetText(lbContent.Text.Trim());
        }
    }
}
