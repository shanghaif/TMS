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
    public partial class fmDeliveryFeeAddNew : BaseForm
    {
        public DataRow dr = null;
        public fmDeliveryFeeAddNew()
        {
            InitializeComponent();
        }

        private void fmDeliveryFeeAdd_Load(object sender, EventArgs e)
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
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Province, "0");
            Province.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            City.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            Area.SelectedIndexChanged += new System.EventHandler(this.edarea_SelectedIndexChanged);
            if (dr != null)
            {
                bsitename.EditValue = dr["sitename"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Province"].ToString().Trim(), Province);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["City"].ToString().Trim(), City);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Area"].ToString().Trim(), Area);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["Street"].ToString().Trim(), Street);
                CenterKilometres.EditValue = dr["CenterKilometres"];
                textEdit1.EditValue = dr["TransferMode"];
                w0_300.EditValue = dr["w0_300"];
                w300_1000.EditValue = dr["w300_1000"];
                w1000_3000.EditValue = dr["w1000_3000"];
                w3000_5000.EditValue = dr["w3000_5000"];
                w5000_8000.EditValue = dr["w5000_8000"];
                w8000_.EditValue = dr["w8000_"];
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

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(City, Province.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Area, City.EditValue);
        }

        private void edarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Street, Area.EditValue);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (w0_300.Text.Trim() == "")
            {
                MsgBox.ShowOK("300以下的标准必须要填写!");
                return;
            }
            if (w300_1000.Text.Trim() == "")
            {
                MsgBox.ShowOK("300-1000的标准必须要填写!");
                return;
            }
            if (w1000_3000.Text.Trim() == "")
            {
                MsgBox.ShowOK("1000-3000的标准必须要填写!");
                return;
            }
            if (w3000_5000.Text.Trim() == "")
            {
                MsgBox.ShowOK("3000-5000的标准必须要填写!");
                return;
            }
            if (w5000_8000.Text.Trim() == "")
            {
                MsgBox.ShowOK("5000-8000的标准必须要填写!");
                return;
            }
            if (w8000_.Text.Trim() == "")
            {
                MsgBox.ShowOK("8000以上的标准必须要填写!");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeliveryFeeID", dr == null ? Guid.NewGuid() : dr["DeliveryFeeID"]));
                list.Add(new SqlPara("Province", Province.Text.Trim()));
                list.Add(new SqlPara("City", City.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim()));
                list.Add(new SqlPara("Street", Street.Text.Trim()));
                list.Add(new SqlPara("CenterKilometres", ConvertType.ToDecimal(CenterKilometres.Text)));
                list.Add(new SqlPara("TransferMode", textEdit1.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("bsitename", bsitename.Text.Trim()));
                list.Add(new SqlPara("w0_300", w0_300.Text.Trim()));
                list.Add(new SqlPara("w300_1000", w300_1000.Text.Trim()));
                list.Add(new SqlPara("w1000_3000", w1000_3000.Text.Trim()));
                list.Add(new SqlPara("w3000_5000", w3000_5000.Text.Trim()));
                list.Add(new SqlPara("w5000_8000", w5000_8000.Text.Trim()));
                list.Add(new SqlPara("w8000_", w8000_.Text.Trim()));
                list.Add(new SqlPara("CompanyID1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASDELIVERYFEE_GX", list);
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