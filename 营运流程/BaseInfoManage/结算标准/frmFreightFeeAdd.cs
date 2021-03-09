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
    public partial class frmFreightFeeAdd : BaseForm
    {
        public DataRow dr = null;
        public frmFreightFeeAdd()
        {
            InitializeComponent();
        }

        private void fmDirectSendFeeAdd_Load(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Province, "0");
            Province.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            City.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            CommonClass.SetSite(StartSite, false);
            CommonClass.FormSet(this);
            if (dr != null)
            {
                StartSite.EditValue = dr["StartSite"];
                TransferSite.EditValue = dr["TransferSite"];
                ParcelPriceMin.EditValue = dr["ParcelPriceMin"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Province"].ToString().Trim(), Province);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["City"].ToString().Trim(), City);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Area"].ToString().Trim(), Area);
                HeavyPrice.EditValue = dr["HeavyPrice"];
                LightPrice.EditValue = dr["LightPrice"];
                TransitMode.EditValue = dr["TransitMode"];
                Prescription.EditValue = dr["Prescription"];
                ParcelPriceMin.EditValue = dr["ParcelPriceMin"];
                Remark.EditValue = dr["Remark"];
                //2017.7.6
                textEdit1.EditValue = dr["ParcelPriceMinDW"];
                textEdit2.EditValue = dr["HeavyPriceDW"];
                textEdit3.EditValue = dr["LightPriceDW"];
                cb_LatestDepartTime.EditValue = dr["LatestDepartTime"];
                smallerHeavyPrice.EditValue = dr["smallerHeavyPrice"];
                smallerLightPrice.EditValue = dr["smallerLightPrice"];
                smallerParcelPriceMin.EditValue = dr["smallerParcelPriceMin"];
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(City, Province.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Area, City.EditValue);
            TransferSite.Text = City.Text.Trim().Replace("市","");

        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FreightId", dr == null ? Guid.NewGuid() : dr["FreightId"]));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
                list.Add(new SqlPara("Province", Province.Text.Trim()));
                list.Add(new SqlPara("City", City.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim()));
                list.Add(new SqlPara("ParcelPriceMin", ConvertType.ToDecimal(ParcelPriceMin.Text)));
                list.Add(new SqlPara("HeavyPrice", Convert.ToDecimal(HeavyPrice.Text.Trim())));
                list.Add(new SqlPara("TransitMode", TransitMode.Text));
                list.Add(new SqlPara("Prescription", Prescription.Text));
                list.Add(new SqlPara("LightPrice", ConvertType.ToDecimal(LightPrice.Text)));
                list.Add(new SqlPara("Remark", ConvertType.ToString(Remark.Text)));
                //2017.7.6wwb
                list.Add(new SqlPara("ParcelPriceMinDW",textEdit1.Text.Trim()));
                list.Add(new SqlPara("HeavyPriceDW", textEdit2.Text.Trim()));
                list.Add(new SqlPara("LightPriceDW", textEdit3.Text.Trim()));
                list.Add(new SqlPara("LatestDepartTime", cb_LatestDepartTime.Text.Trim()));
                //2018.11.17gxh
                list.Add(new SqlPara("smallerHeavyPrice",smallerHeavyPrice.Text.Trim()));
                list.Add(new SqlPara("smallerLightPrice",smallerLightPrice.Text.Trim()));
                list.Add(new SqlPara("smallerParcelPriceMin", smallerParcelPriceMin.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASFREIGHTFEE", list);
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
    }
}