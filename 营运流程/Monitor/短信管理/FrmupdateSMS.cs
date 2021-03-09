using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class FrmupdateSMS : BaseForm
    {
        public string billNo = "", telephone = "", oldId = "";
        public FrmupdateSMS()
        {
            InitializeComponent();

        }
        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", SWBillNo.Text.Trim()));
                list.Add(new SqlPara("OldTel", SWTelNum.Text.Trim()));
                list.Add(new SqlPara("NewTel", TextNewTel.Text.Trim()));
                list.Add(new SqlPara("oldId", oldId.Trim()));
                if (MsgBox.ShowYesNo("确定要修改手机号为：" + "【" + TextNewTel.Text.Trim() + "】" ) == DialogResult.No) return;
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Update_SMSTel", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
                else
                {
                    MsgBox.ShowOK("修改出现错误,请重试！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void FrmupdateSMS_Load(object sender, EventArgs e)
        {
            SWBillNo.Text = billNo;
            SWTelNum.Text = telephone;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
