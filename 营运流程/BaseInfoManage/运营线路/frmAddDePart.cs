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
    public partial class frmAddPart : BaseForm
    {
        public DataRow dr = null;
        public frmAddPart()
        {
            InitializeComponent();
        }

        private void frmAddPart_Load(object sender, EventArgs e)
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

            AreaAll();
            if (dr != null)
            {
                // DepId.EditValue = dr["DepId"];
                DepArea.EditValue = dr["DepArea"];
                DepName.EditValue = dr["DepName"];
                DepCode.EditValue = dr["DepCode"];
                DepMan.EditValue = dr["DepMan"];
                DepPhone.EditValue = dr["DepPhone"];
                DepRemark.EditValue = dr["DepRemark"];
                ZQTMS.Common.CommonClass.SetSelectIndexByValue(dr["DepRight"].ToString().Trim(), DepRight);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepId", dr == null ? Guid.NewGuid() : dr["DepId"]));
                list.Add(new SqlPara("DepArea", DepArea.Text.Trim()));
                list.Add(new SqlPara("DepName", DepName.Text.Trim()));
                list.Add(new SqlPara("DepCode", DepCode.Text.Trim()));
                list.Add(new SqlPara("DepMan", DepMan.Text.Trim()));
                list.Add(new SqlPara("DepPhone", DepPhone.Text.Trim()));
                list.Add(new SqlPara("DepRemark", DepRemark.Text.Trim()));
                list.Add(new SqlPara("DepRight", DepRight.EditValue == null ? 0 : DepRight.EditValue));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDEPART", list);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AreaAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables.Count != 0)
                    {
                        DepArea.Properties.Items.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            DepArea.Properties.Items.Add(ds.Tables[0].Rows[i]["AreaName"].ToString().Trim());
                    }
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