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
    public partial class fmDenominatedFeeAddNew : BaseForm
    {
        public DataRow dr = null;
        public fmDenominatedFeeAddNew()
        {
            InitializeComponent();
        }

        private void fmTransferFeeAdd_Load(object sender, EventArgs e)
        {
            //GetCompanyId();
            //if (Common.CommonClass.UserInfo.companyid != "101")
            //{
            //    CompanyID.Enabled = false;
            //    CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            //}
            //else
            //{
            //    CompanyID.Enabled = true;
            //    CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            //}
            CommonClass.FormSet(this);
            CommonClass.SetSite(StartSite, true);
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Province, "0");
            Province.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            City.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            if (dr != null)
            {
                CusName.EditValue = dr["CusName"];
                StartSite.EditValue = dr["StartSite"];
                WeightMin.EditValue = dr["WeightMin"];
                WeightMax.EditValue = dr["WeightMax"];
                HeavyPrice.EditValue = dr["HeavyPrice"];
                HeavyPriceMin.EditValue = dr["HeavyPriceMin"];
                VolumeMin.EditValue = dr["VolumeMin"];
                VolumeMax.EditValue = dr["VolumeMax"];
                LightPrice.EditValue = dr["LightPrice"];
                LightPriceMin.EditValue = dr["LightPriceMin"];
                DeliFee.EditValue = dr["DeliFee"];
                ReceivFee.EditValue = dr["ReceivFee"];
                NumPrice.EditValue = dr["NumPrice"];
                Varieties.EditValue = dr["Varieties"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Province"].ToString().Trim(), Province);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["City"].ToString().Trim(), City);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Area"].ToString().Trim(), Area);
                //if (ZQTMS.Common.CommonClass.UserInfo.companyid != "101")
                //{
                //    CompanyID.Enabled = false;
                //    CompanyID.EditValue = dr["companyid"];
                //}
                //else
                //{
                //    CompanyID.Enabled = true;
                //    CompanyID.EditValue = dr["companyid"];
                //}

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //public void GetCompanyId()
        //{
        //    try
        //    {
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
        //        DataSet ds = SqlHelper.GetDataSet(sps);
        //        if (ds == null || ds.Tables.Count == 0) return;

        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            CompanyID.Properties.Items.Add(dr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DenominatedFeeID", dr == null ? Guid.NewGuid() : dr["DenominatedFeeID"]));
                list.Add(new SqlPara("CusName", CusName.Text.Trim()));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));

                list.Add(new SqlPara("Province", Province.Text.Trim()));
                list.Add(new SqlPara("City", City.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim()));
                list.Add(new SqlPara("WeightMin", ConvertType.ToDecimal(WeightMin.Text)));
                list.Add(new SqlPara("WeightMax", ConvertType.ToDecimal(WeightMax.Text)));
                list.Add(new SqlPara("HeavyPrice", ConvertType.ToDecimal(HeavyPrice.Text)));
                list.Add(new SqlPara("HeavyPriceMin", ConvertType.ToDecimal(HeavyPriceMin.Text)));
                list.Add(new SqlPara("VolumeMin", ConvertType.ToDecimal(VolumeMin.Text)));
                list.Add(new SqlPara("VolumeMax", ConvertType.ToDecimal(VolumeMax.Text)));
                list.Add(new SqlPara("LightPrice", ConvertType.ToDecimal(LightPrice.Text)));
                list.Add(new SqlPara("LightPriceMin", ConvertType.ToDecimal(LightPriceMin.Text)));
                list.Add(new SqlPara("DeliFee", ConvertType.ToDecimal(DeliFee.Text)));
                list.Add(new SqlPara("ReceivFee", ConvertType.ToDecimal(ReceivFee.Text)));
                list.Add(new SqlPara("NumPrice", ConvertType.ToDecimal(NumPrice.Text)));  //zb20191024 lms-5295
                list.Add(new SqlPara("Varieties", Varieties.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basDenominatedFee_GX", list);
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
