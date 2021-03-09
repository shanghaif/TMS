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
    public partial class frmBasSite_Add : BaseForm
    {
        public frmBasSite_Add()
        {
            InitializeComponent();
        }
        public string SiteId = "";
        private void frmOrgUnit_Site_Add_Load(object sender, EventArgs e)
        {
            CommonClass.GetCompanyId(this.CompanyID);
            CommonClass.GetCompanyId(this.txtAllocateCompanyID);
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

            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(SiteProvince, "0");
            SiteProvince.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            CommonClass.SetSite(StartSiteRange);
            //CommonClass.SetWeb(ControlWeb, "全部");
            if (SiteId != "")
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteId", SiteId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);                
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                SiteCode.EditValue = dr["SiteCode"];
                SiteName.EditValue = dr["SiteName"];
                LineCode.EditValue = dr["LineCode"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["SiteProvince"].ToString(), SiteProvince);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["SiteCity"].ToString(), SiteCity);

                SiteAddress.EditValue = dr["SiteAddress"];
                SiteRemark.EditValue = dr["SiteRemark"];
                ControlWeb.Text = ConvertType.ToString(dr["ControlWeb"]);
                SiteStatus.Checked = Convert.ToInt32(dr["SiteStatus"].ToString() == "" ? "0" : dr["SiteStatus"].ToString()) > 0;
                StartSiteRange.EditValue = dr["EndSiteRang"];
                txtAllocateCompanyID.EditValue = dr["AllocateCompanyID"];
                CompanyID.EditValue = dr["BelongToCompany"];
                if (ZQTMS.Common.CommonClass.UserInfo.companyid != "101")
                {
                    CompanyID.Enabled = false;
                    //CompanyID.EditValue = dr["companyid"];
                }
                else
                {
                    CompanyID.Enabled = true;
                   // CompanyID.EditValue = dr["companyid"];
                }
            }

        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(SiteCity, SiteProvince.EditValue);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ControlWeb.Text.Trim()))
                {
                    MsgBox.ShowError("请输入调车方向");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteId", SiteId == "" ? Guid.NewGuid() : new Guid(SiteId)));
                list.Add(new SqlPara("SiteCode", SiteCode.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("LineCode", LineCode.Text.Trim()));
                list.Add(new SqlPara("SiteProvince", SiteProvince.Text.Trim()));
                list.Add(new SqlPara("SiteCity", SiteCity.Text.Trim()));
                list.Add(new SqlPara("SiteAddress", SiteAddress.Text.Trim()));
                list.Add(new SqlPara("SiteRemark", SiteRemark.Text.Trim()));
                list.Add(new SqlPara("SiteStatus", SiteStatus.Checked ? 1 : 0));
                list.Add(new SqlPara("ControlWeb", ControlWeb.Text.Trim()));
                list.Add(new SqlPara("EndSiteRang",StartSiteRange.Text.Trim()));
                list.Add(new SqlPara("companyid1", "101"));  //毛慧20171212
                //EndSiteRang

                //增加分拨公司参数 zaj 2017-11-20
                list.Add(new SqlPara("AllocateCompanyID", txtAllocateCompanyID.Text.Trim()));
                list.Add(new SqlPara("BelongToCompany", CompanyID.Text.Trim()));//毛慧20171212

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASSITE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    if (SiteId.ToString() != "")
                    {
                        this.Close();
                    }
                    SiteCode.EditValue = "";
                    SiteName.EditValue = "";
                    LineCode.EditValue = "";
                    SiteProvince.EditValue = DBNull.Value;
                    SiteCity.EditValue = DBNull.Value;
                    SiteAddress.EditValue = "";
                    SiteRemark.EditValue = "";
                    SiteStatus.Checked = true;
                }
                else
                {
                    MsgBox.ShowOK("保存失败");
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

        public void GetCompanyId(DevExpress.XtraEditors.CheckedComboBoxEdit com)
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    com.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


    }
}
