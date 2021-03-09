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
    public partial class fmTransferFeeAdd : BaseForm
    {
        public DataRow dr = null;
        public fmTransferFeeAdd()
        {
            InitializeComponent();
        }

        private void fmTransferFeeAdd_Load(object sender, EventArgs e)
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
            //CommonClass.SetSite(FromSite, true);
            CommonClass.SetSite(FromSite, false);
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Province, "0");
            Province.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            City.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            if (dr != null)
            {
                FromSite.EditValue = dr["FromSite"];
                TransferSite.EditValue = dr["TransferSite"];
                HeavyPrice.EditValue = dr["HeavyPrice"];
                LightPrice.EditValue = dr["LightPrice"];
                ParcelPriceMin.EditValue = dr["ParcelPriceMin"];
                txtTransitMode.EditValue = dr["TransitMode"];
                Remark.EditValue = dr["Remark"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["ToProvince"].ToString().Trim(), Province);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["ToCity"].ToString().Trim(), City);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["ToArea"].ToString().Trim(), Area);
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

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(City, Province.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Area, City.EditValue);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TransferFeeID", dr == null ? Guid.NewGuid() : dr["TransferFeeID"]));
                list.Add(new SqlPara("FromSite", FromSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
                list.Add(new SqlPara("ToProvince", Province.Text.Trim()));
                list.Add(new SqlPara("ToCity", City.Text.Trim()));
                list.Add(new SqlPara("ToArea", Area.Text.Trim()));
                list.Add(new SqlPara("HeavyPrice", ConvertType.ToDecimal(HeavyPrice.Text)));
                list.Add(new SqlPara("LightPrice", ConvertType.ToDecimal(LightPrice.Text)));
                list.Add(new SqlPara("ParcelPriceMin", ConvertType.ToDecimal(ParcelPriceMin.Text)));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("TransitMode",txtTransitMode.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASTRANSFERFEE", list);
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
