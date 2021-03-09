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
    public partial class fmDepartureAllotFeeAdd : BaseForm
    {
        public DataRow dr = null;
        public fmDepartureAllotFeeAdd()
        {
            InitializeComponent();
        }

        private void fmDepartureAllotFeeAdd_Load(object sender, EventArgs e)
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
            CommonClass.SetWeb(BegWeb, true);
            if (BegWeb.Properties.Items.Contains("全部")) {
                BegWeb.Properties.Items.Remove("全部");
            }
            
            if (dr != null)
            {
                FromSite.EditValue = dr["FromSite"];
                BegWeb.EditValue = dr["BegWeb"];
                TransferSite.EditValue = dr["TransferSite"];
                HeavyPrice.EditValue = dr["HeavyPrice"];
                LightPrice.EditValue = dr["LightPrice"];
                ParcelPriceMin.EditValue = dr["ParcelPriceMin"];
                Remark.EditValue = dr["Remark"];
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureAllotFeeID", dr == null ? Guid.NewGuid() : dr["DepartureAllotFeeID"]));
                list.Add(new SqlPara("FromSite", FromSite.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
                list.Add(new SqlPara("HeavyPrice", ConvertType.ToDecimal(HeavyPrice.Text)));
                list.Add(new SqlPara("LightPrice", ConvertType.ToDecimal(LightPrice.Text)));
                list.Add(new SqlPara("ParcelPriceMin", ConvertType.ToDecimal(ParcelPriceMin.Text)));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

               // SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDEPARTUREALLOTFEE", list);
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDEPARTUREALLOTFEE_NEW", list);
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