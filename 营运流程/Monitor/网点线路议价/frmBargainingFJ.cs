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
    public partial class frmBargainingFJ : BaseForm
    {
        public string id = "", newMainfee = "", billno = "", confirmState = "", applyWebName = "", inputSerialNumber = "", newDeliveryFee="";
        public int flag = 2;
        public frmBargainingFJ()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBargainingFJ_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (foujueyuanyin.Text == "")
            {
                MsgBox.ShowOK("否决原因不能为空");
                return;
            }
            if (MsgBox.ShowYesNo("确定完成否决吗？") == DialogResult.No) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("flag", flag));
                list.Add(new SqlPara("newMainfee", newMainfee));
                list.Add(new SqlPara("newDeliveryFee", newDeliveryFee));
                list.Add(new SqlPara("billno", billno));
                list.Add(new SqlPara("applyWeb", applyWebName));
                list.Add(new SqlPara("inputSerialNumber", inputSerialNumber));
                list.Add(new SqlPara("foujueyuanyin", foujueyuanyin.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_CONFIRM_BargainingInfo", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
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