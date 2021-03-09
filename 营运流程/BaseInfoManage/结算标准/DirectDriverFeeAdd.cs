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
    public partial class DirectDriverFeeAdd : BaseForm
    {
        public DataRow dr = null;
        public DirectDriverFeeAdd()
        {
            InitializeComponent();
        }

        private void fmDirectSendFeeAdd_Load(object sender, EventArgs e)
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
            CommonClass.SetSite(SiteName, false);
            CommonClass.FormSet(this);
            if (dr != null)
            {
                WebName.EditValue = dr["WebName"];
                DirAddress.EditValue = dr["DirAddress"];
                Lng.EditValue = dr["Lng"];
                Lat.EditValue = dr["Lat"];
                KmPrice.EditValue = dr["KmPrice"];
                Remark.EditValue = dr["Remark"];
                SiteName.EditValue = dr["SiteName"];
                LowPrice.EditValue = dr["LowPrice"];
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DirectDriverID", dr == null ? Guid.NewGuid() : dr["DirectDriverID"]));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                list.Add(new SqlPara("DirAddress", DirAddress.Text.Trim()));
                list.Add(new SqlPara("Lng", Lng.Text.Trim()));
                list.Add(new SqlPara("Lat", Lat.Text.Trim()));
                list.Add(new SqlPara("KmPrice", ConvertType.ToDecimal(KmPrice.Text)));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("LowPrice", ConvertType.ToDecimal(ConvertType.ToDecimal(LowPrice.Text))));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_DIRECTDRIVERFEE", list);
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

        private void CenterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = CommonClass.dsWeb;
            if (ds != null && ds.Tables[0] != null)
            {
                DataRow[] rows = ds.Tables[0].Select(string.Format("WebName='{0}'", WebName.Text.Trim()));
                if (rows != null && rows.Length > 0)
                {
                    DirAddress.Text = rows[0]["WebAddress"].ToString();
                    Lng.Text = rows[0]["WebLng"].ToString();
                    Lat.Text = rows[0]["WebLat"].ToString();
                }
            }
        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, SiteName.Text.Trim(), false);
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