using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class FrmWalletAlterPwd : BaseForm
    {
        public string PWD = "";
        public string NewPwd="";
        public FrmWalletAlterPwd()
        {
            InitializeComponent();
        }

        private void btn_alter_Click(object sender, EventArgs e)
        {
            string oldPwd = txtOldPwd.Text.Trim();
            string newPwd = txtNewPwd.Text.Trim();
            string pwdConfrim = txtPwdConfirm.Text.Trim();
            if (StringHelper.Md5Hash(oldPwd) != PWD)
            {
                MsgBox.ShowOK("原始密码错误!");
                return;

            }
            if (newPwd == "")
            {
                MsgBox.ShowOK("新密码不能为空!");
                return;
            }
            if (newPwd != pwdConfrim)
            {
                MsgBox.ShowOK("两次密码不一致,请重新输入!");
                return;
            }
            if (MsgBox.ShowYesNo("确认修改密码?") == DialogResult.Yes)
            {
                NewPwd = newPwd;
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}