using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraEditors.Controls;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAddArea : BaseForm
    {
        public DataRow dr = null;
        public frmAddArea()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AreaID", dr == null ? Guid.NewGuid() : dr["AreaID"]));
                list.Add(new SqlPara("AreaCause", AreaCause.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                list.Add(new SqlPara("AreaCode", AreaCode.Text.Trim()));
                list.Add(new SqlPara("AreaMan", AreaMan.Text.Trim()));
                list.Add(new SqlPara("AreaPhone", AreaPhone.Text.Trim()));
                list.Add(new SqlPara("AreaRemark", AreaRemark.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASAREA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddArea_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            if (Common.CommonClass.UserInfo.companyid != "101")
            {
                CompanyID.Enabled = false;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            else
            {
                CompanyID.Enabled = true;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }

            CommonClass.FormSet(this);
            CauseAll();
            if (dr != null)
            {
                //AreaID.EditValue = dr["AreaID"];
                AreaCause.EditValue = dr["AreaCause"];
                AreaName.EditValue = dr["AreaName"];
                AreaCode.EditValue = dr["AreaCode"];
                AreaMan.EditValue = dr["AreaMan"];
                AreaPhone.EditValue = dr["AreaPhone"];
                AreaRemark.EditValue = dr["AreaRemark"];
                if (ZQTMS.Common.CommonClass.UserInfo.companyid != "101")
                {
                    CompanyID.Enabled = false;
                    CompanyID.EditValue = dr["companyid"];
                }
                else
                {
                    CompanyID.Enabled = true;
                    CompanyID.EditValue = dr["companyid"];
                }
            }
        }
        private void CauseAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    AreaCause.Properties.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                        AreaCause.Properties.Items.Add(ds.Tables[0].Rows[i]["CauseName"].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
