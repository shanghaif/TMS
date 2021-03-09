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

namespace ZQTMS.UI
{
    public partial class frmUpdUserPass : BaseForm
    {
        public bool MustUpdate = false;

        public frmUpdUserPass()
        {
            InitializeComponent();
        }

        private void frmUpdUserPass_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("密码修改");//xj/2019/5/29
            if (MustUpdate)
            {
                label1.Visible = true;
                this.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ps1 = textBox1.Text;
            string ps2 = textBox2.Text;

            if (ps1 != ps2)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MsgBox.ShowError("两次密码不一致请重新输入！");
                textBox1.Focus();
                return;
            }

            if (ps1.Trim() == "")
            {
                MsgBox.ShowError("密码不能为空！");
                textBox1.Text = "";
                textBox1.Focus();
                return;
            }

            if (ps2.Trim() == "")
            {
                MsgBox.ShowError("密码不能为空！");
                textBox2.Text = "";
                textBox2.Focus();
                return;
            }

            try
            {
                if (MsgBox.ShowYesNo("是否重置该用户密码？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("userid", CommonClass.UserInfo.UserId));
                list.Add(new SqlPara("userpsw", StringHelper.Md5Hash(ps1)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_RESET_USERPASS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.Cancel;
                MsgBox.ShowException(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MustUpdate)
            {
                this.DialogResult = DialogResult.Cancel;
                Application.Restart();
            }
            else
            {
                this.Close();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) textBox2.Focus();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}