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
    public partial class frmCustSearchLog_Add : BaseForm
    {
        public DataRow dr;
        public string BillNO;

        private void frmCustSearchLog_Add_Load(object sender, EventArgs e)
        {
            QDate.EditValue = DateTime.Now.ToString();
            QCustServiceMan.EditValue = CommonClass.UserInfo.UserName;
            if (!string.IsNullOrEmpty(BillNO))
            {
                txtBillNO.Text = BillNO;
                txtBillNO.Enabled = true;
            }
            CommonClass.SetCause(ComplainCause,false);
            ComplainCause.EditValue = CommonClass.UserInfo.CauseName;
        }

        public frmCustSearchLog_Add()
        {
            InitializeComponent();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Qid", Guid.NewGuid()));
                list.Add(new SqlPara("QDate", QDate.Text.Trim()));
                list.Add(new SqlPara("QCustCompany", QCustCompany.Text.Trim()));
                list.Add(new SqlPara("QCustPhone", QCustPhone.Text.Trim()));
                list.Add(new SqlPara("QType", QType.Text.Trim()));
                list.Add(new SqlPara("QResult", QResult.Text.Trim()));
                list.Add(new SqlPara("QContent", QContent.Text.Trim()));
                list.Add(new SqlPara("QAnswerContent", QAnswerContent.Text.Trim()));
                list.Add(new SqlPara("QCustServiceMan", QCustServiceMan.Text.Trim()));
                list.Add(new SqlPara("BillNO", txtBillNO.Text.Trim()));
                list.Add(new SqlPara("ComplainDep", ComplainDep.Text.Trim()));
                list.Add(new SqlPara("ComplainType", ComplainType.Text.Trim()));
                list.Add(new SqlPara("ComplainLevel", ComplainLevel.Text.Trim()));
                list.Add(new SqlPara("ComplainArea", ComplainArea.Text.Trim()));
                list.Add(new SqlPara("ComplainCause", ComplainCause.Text.Trim()));
                list.Add(new SqlPara("IsQCust",cbIsQCust.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLCUSTQUERRYLOG", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    if (string.IsNullOrEmpty(BillNO)) return;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void ComplainArea_SelectedValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(ComplainArea, ComplainCause.Text.Trim(), false);
            ComplainArea.EditValue = CommonClass.UserInfo.AreaName;
        }

        private void ComplainArea_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetDep(ComplainDep, ComplainArea.Text, false);
            ComplainDep.EditValue = CommonClass.UserInfo.DepartName;
        }
    }
}