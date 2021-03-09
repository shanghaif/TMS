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
    public partial class frmShortlineSiteAdd : BaseForm
    {
        public DataRow dr = null;
        public frmShortlineSiteAdd()
        {
            InitializeComponent();
        }

        private void fmMainlineFeeAdd_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(SiteProvince, "0");
            SiteProvince.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            CommonClass.FormSet(this);
            companyid.Text = CommonClass.UserInfo.companyid;

            if (dr != null)
            {
                SiteName.EditValue = dr["SiteName"];
                LineCode.EditValue = dr["LineCode"];
                //SiteProvince.EditValue = dr["SiteProvince"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["SiteProvince"].ToString(), SiteProvince);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["SiteCity"].ToString(), SiteCity);
                SiteRemark.EditValue = dr["SiteRemark"];
                SiteAddress.EditValue = dr["SiteAddress"];
                companyid.EditValue = dr["companyid"];
          
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", dr == null ? Guid.NewGuid() : dr["ID"]));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("LineCode", LineCode.Text.Trim()));
                list.Add(new SqlPara("SiteProvince", SiteProvince.Text.Trim()));
                list.Add(new SqlPara("SiteCity", SiteCity.Text.Trim()));
                list.Add(new SqlPara("SiteRemark", SiteRemark.Text.Trim()));
                list.Add(new SqlPara("SiteAddress", SiteAddress.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basShortlineSite", list);
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
                    companyid.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(SiteCity, SiteProvince.EditValue);
        }
    }
}