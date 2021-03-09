using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBespokSend : BaseForm
    {
        public string BillNO;
        public frmBespokSend()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BespeakID", Guid.NewGuid()));
                list.Add(new SqlPara("BespeakDate", BespeakDate.Text.Trim()));
                list.Add(new SqlPara("BespeakRequir", BespeakRequir.Text.Trim()));
                list.Add(new SqlPara("Operator", Operator.Text.Trim()));
                list.Add(new SqlPara("BespeakDept", BespeakDept.Text.Trim()));
                list.Add(new SqlPara("BillNO", BillNO));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BLLBESPEAKSENDGOODS", list);

                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmBespokSend_Load(object sender, EventArgs e)
        {
            Operator.EditValue = CommonClass.UserInfo.UserName;
            CommonClass.SetDep(BespeakDept, "全部");
            BespeakDate.DateTime = DateTime.Now;
        }
    }
}
