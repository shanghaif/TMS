using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class FrmAddGrouper : BaseForm
    {
        public FrmAddGrouper()
        {
            InitializeComponent();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                if (string.IsNullOrEmpty(txtGrouperName.Text))
                {
                    MsgBox.ShowOK("请输入组员名！");
                    return;
                }
                if (string.IsNullOrEmpty(txtGroupJob.Text))
                {
                    MsgBox.ShowOK("请输入组员职位！");
                    return;
                }
                list.Add(new SqlPara("employeeName", txtGrouperName.Text));
                list.Add(new SqlPara("employeeJob", txtGroupJob.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Add_GrouperInfo",list);
                if (SqlHelper.ExecteNonQuery(sps) > 0) {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}