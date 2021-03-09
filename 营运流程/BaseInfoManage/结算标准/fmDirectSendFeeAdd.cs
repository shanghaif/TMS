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
    public partial class fmDirectSendFeeAdd : BaseForm
    {
        public DataRow dr = null;
        public fmDirectSendFeeAdd()
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
                CenterName.EditValue = dr["CenterName"];
                DeliveryAddress.EditValue = dr["DeliveryAddress"];
                GPSLong.EditValue = dr["GPSLng"];
                GPSLat.EditValue = dr["GPSLat"];
                Price.EditValue = dr["Price"];
                TransferMode.EditValue = dr["TransferMode"];
                Remark.EditValue = dr["Remark"];
                SiteName.EditValue = dr["SiteName"];
                OperationWeight.EditValue = dr["OperationWeight"];
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
                list.Add(new SqlPara("DirectSendID", dr == null ? Guid.NewGuid() : dr["DirectSendID"]));
                list.Add(new SqlPara("CenterName", CenterName.Text.Trim()));
                list.Add(new SqlPara("DeliveryAddress", DeliveryAddress.Text.Trim()));
                list.Add(new SqlPara("GPSLng", GPSLong.Text.Trim()));
                list.Add(new SqlPara("GPSLat", GPSLat.Text.Trim()));
                list.Add(new SqlPara("Price", ConvertType.ToDecimal(Price.Text)));
                list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("OperationWeight", ConvertType.ToDecimal(OperationWeight.Text)));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDIRECTSENDFEE", list);
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
                DataRow[] rows = ds.Tables[0].Select(string.Format("WebName='{0}'", CenterName.Text.Trim()));
                if (rows != null && rows.Length > 0)
                {
                    DeliveryAddress.Text = rows[0]["WebAddress"].ToString();
                    GPSLong.Text = rows[0]["WebLng"].ToString();
                    GPSLat.Text = rows[0]["WebLat"].ToString();
                }
            }
        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(CenterName, SiteName.Text.Trim(), false);
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