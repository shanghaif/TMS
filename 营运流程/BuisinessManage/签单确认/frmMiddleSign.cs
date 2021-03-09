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
    public partial class frmMiddleSign : BaseForm

    {
        public frmMiddleSign()
        {
            InitializeComponent();
        }
        DataRow dr = null;
        sysUserInfoInfo UserInfo = new sysUserInfoInfo();
        private void frmMiddleSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            //加载默认数据
            if (dr != null) 
            {
                BillNo.EditValue = dr["BillNO"];
                StartSite.EditValue = dr["StartSite"];
                DestinationSite.EditValue = dr["DestinationSite"];
                CusOderNo.EditValue = dr["CusOderNo"];
                BillDate.EditValue = dr["BillDate"];
                ConsignorName.EditValue = dr["ConsignorName"];
                ConsignorCellPhone.EditValue = dr["ConsignorCellPhone"];
                ConsignorCompany.EditValue = dr["ConsignorCompany"];
                ConsigneeName.EditValue = dr["ConsigneeName"];
                ConsigneeCellPhone.EditValue = dr["ConsigneeCellPhone"];
                ConsigneeCompany.EditValue = dr["ConsigneeCompany"];
                Varieties.EditValue = dr["Varieties"];
                Num.EditValue = dr["Num"];
                BearCompName.EditValue = dr["BearCompName"];
                MiddleDate.EditValue = dr["MiddleDate"];
                BearCompPhone.EditValue = dr["BearCompPhone"];
                BearToSitePhone.EditValue = dr["BearToSitePhone"];
                MiddleOperator.EditValue = dr["MiddleOperator"];
                AccMiddlePay.EditValue = dr["AccMiddlePay"];
                MiddlePayState.EditValue = dr["MiddlePayState"];
                OperateState.Checked = ConvertType.ToInt32(dr["OperateState"]) == 1;
                OperateType.EditValue = dr["OperateType"];
                OperateTime.EditValue = dr["OperateTime"];
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SignNO", Guid.NewGuid()));
                list.Add(new SqlPara("BillNO", BillNo.Text.Trim()));
                list.Add(new SqlPara("SignType", "提货签收"));
                list.Add(new SqlPara("SignMan", SignMan.Text.Trim()));
                list.Add(new SqlPara("SignManCardID", SignManCardID.Text.Trim()));
                list.Add(new SqlPara("AgentMan", ""));
                list.Add(new SqlPara("AgentCardId", ""));
                list.Add(new SqlPara("SignDate", SignDate.Text.Trim()));
                list.Add(new SqlPara("SignDesc", ""));
                list.Add(new SqlPara("SignOperator", UserInfo.UserName));
                list.Add(new SqlPara("SignSite", UserInfo.SiteName));
                list.Add(new SqlPara("SignWeb", UserInfo.WebName));
                list.Add(new SqlPara("SignContent", SignContent.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLSIGN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    //CommonSyn.TraceSyn(null, BillNo.Text.Trim()+"@", 14, "提货签收", 1, null, null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnSignUpload_Click(object sender, EventArgs e)
        {

        }
    }
}