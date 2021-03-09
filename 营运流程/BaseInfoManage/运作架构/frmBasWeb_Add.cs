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
    public partial class frmBasWeb_Add : BaseForm
    {
        public frmBasWeb_Add()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        string WebId = "";
        private DataSet dsBASAREA = new DataSet();

        private void frmOrgUnit_Web_Add_Load(object sender, EventArgs e)
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
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebProvince, "0");
            WebProvince.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            WebCity.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            WebArea.SelectedIndexChanged += new System.EventHandler(this.edarea_SelectedIndexChanged);

            CommonClass.SetSite(StartSiteRange);
            CommonClass.SetDep(WebName, "%%", false);
            CommonClass.SetWeb(WebCheckScope, "%%", false);

            SiteAll();
            freshCause();
            freshArea();
            if (dr != null)
            {
                try
                {
                    WebId = dr["WebId"].ToString();
                    WebCode.EditValue = dr["WebCode"];
                    InternalCode.EditValue = dr["InternalCode"];
                    SiteName.EditValue = dr["SiteName"];
                    WebName.EditValue = dr["WebName"];
                    WebAddress.EditValue = dr["WebAddress"];
                    ZQTMS.Common.CommonClass.SetSelectIndex(dr["WebProvince"].ToString().Trim(), WebProvince);
                    ZQTMS.Common.CommonClass.SetSelectIndex(dr["WebCity"].ToString().Trim(), WebCity);
                    ZQTMS.Common.CommonClass.SetSelectIndex(dr["WebArea"].ToString().Trim(), WebArea);
                    ZQTMS.Common.CommonClass.SetSelectIndex(dr["WebStreet"].ToString().Trim(), WebStreet);
                    WebMan.EditValue = dr["WebMan"];
                    WebServiceTel.EditValue = dr["WebServiceTel"];
                    WebFetchTel.EditValue = dr["WebFetchTel"];
                    WebSendTel.EditValue = dr["WebSendTel"];
                    WebLng.EditValue = dr["WebLng"];
                    WebLat.EditValue = dr["WebLat"];
                    WebType.EditValue = dr["WebType"];
                    WebLevel.EditValue = dr["WebLevel"];
                    WebRole.EditValue = dr["WebRole"];
                    WebSubName.EditValue = dr["WebSubName"];
                    WebMiddleSite.EditValue = dr["WebMiddleSite"];
                    WebRemark.EditValue = dr["WebRemark"];
                    BelongCause.EditValue = dr["BelongCause"];
                    IsHasDirect.EditValue = dr["IsHasDirect"];
                    CommonClass.SetCheckedItems(dr["DirectToSites"].ToString(), DirectToSites);
                    WebStatus.Checked = ConvertType.ToInt32(dr["WebStatus"]) > 0;
                    BelongArea.EditValue = dr["BelongArea"];
                    ZQTMS.Common.CommonClass.SetSelectIndexByValue(dr["WebRight"].ToString().Trim(), WebRight);
                    IsSendVehicle.Checked = ConvertType.ToString(dr["IsSendVehicle"]) != "是" ? false : true;
                    IsAcceptShort.Checked = ConvertType.ToInt32(dr["IsAcceptShort"]) > 0;
                    StartSiteRange.EditValue = dr["StartSiteRange"];
                    WeightLevel.Text = ConvertType.ToString(dr["WeightLevel"]);
                    IsJoinLine.Checked = ConvertType.ToString(dr["IsJoinLine"]) != "1" ? false : true;
                    WebCheckScope.EditValue = dr["WebCheckScope"];
                    txtAllocateCompanyID.EditValue = dr["AllocateCompanyID"];
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
                catch (Exception ex)
                {
                    MsgBox.ShowError(ex.Message);
                }
            }
        }

        private void SiteAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SiteName.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                    DirectToSites.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void getWeb()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    WebSubName.Properties.Items.Add(ds.Tables[0].Rows[i]["WebSubName"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void freshCause()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BelongCause.Properties.Items.Add(ds.Tables[0].Rows[i]["CauseName"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void freshArea()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                dsBASAREA = SqlHelper.GetDataSet(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebCity, WebProvince.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebArea, WebCity.EditValue);
        }

        private void edarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebStreet, WebArea.EditValue);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebId", WebId == "" ? Guid.NewGuid() : new Guid(WebId)));
                list.Add(new SqlPara("WebCode", WebCode.Text.Trim()));
                list.Add(new SqlPara("InternalCode", InternalCode.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                list.Add(new SqlPara("WebProvince", WebProvince.Text.Trim()));
                list.Add(new SqlPara("WebCity", WebCity.Text.Trim()));
                list.Add(new SqlPara("WebArea", WebArea.Text.Trim()));
                list.Add(new SqlPara("WebStreet", WebStreet.Text.Trim()));
                list.Add(new SqlPara("WebAddress", WebAddress.Text.Trim()));
                list.Add(new SqlPara("WebMan", WebMan.Text.Trim()));
                list.Add(new SqlPara("WebServiceTel", WebServiceTel.Text.Trim()));
                list.Add(new SqlPara("WebFetchTel", WebFetchTel.Text.Trim()));
                list.Add(new SqlPara("WebSendTel", WebSendTel.Text.Trim()));
                list.Add(new SqlPara("WebLng", WebLng.Text.Trim()));
                list.Add(new SqlPara("WebLat", WebLat.Text.Trim()));
                list.Add(new SqlPara("WebType", WebType.Text.Trim()));
                list.Add(new SqlPara("WebLevel", WebLevel.Text.Trim()));
                list.Add(new SqlPara("WebRole", WebRole.Text.Trim()));
                list.Add(new SqlPara("WebSubName", WebSubName.Text.Trim()));
                list.Add(new SqlPara("WebMiddleSite", WebMiddleSite.Text.Trim()));
                list.Add(new SqlPara("WebRemark", WebRemark.Text.Trim()));
                list.Add(new SqlPara("WebStatus", WebStatus.Checked ? 1 : 0));
                list.Add(new SqlPara("BelongCause", BelongCause.Text.Trim()));
                list.Add(new SqlPara("BelongArea", BelongArea.Text.Trim()));
                list.Add(new SqlPara("IsHasDirect", IsHasDirect.Text.Trim()));
                list.Add(new SqlPara("DirectToSites", DirectToSites.Text.Trim()));
                list.Add(new SqlPara("WebRight", WebRight.EditValue == null ? 0 : WebRight.EditValue));

                list.Add(new SqlPara("IsSendVehicle", IsSendVehicle.Checked ? "是" : "否"));
                list.Add(new SqlPara("IsAcceptShort", IsAcceptShort.Checked ? 1 : 0));
                list.Add(new SqlPara("StartSiteRange", StartSiteRange.Text.Trim()));
                list.Add(new SqlPara("WeightLevel", ConvertType.ToInt32(WeightLevel.Text)));
                list.Add(new SqlPara("WebCheckScope", WebCheckScope.Text.Trim()));
                list.Add(new SqlPara("IsJoinLine", IsJoinLine.Checked ? 1 : 0));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
                //增加分拨公司参数 zaj 2017-11-20
                list.Add(new SqlPara("AllocateCompanyID", txtAllocateCompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASWEB_2", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    WebCode.EditValue = "";
                    InternalCode.EditValue = "";
                    SiteName.EditValue = "";
                    WebName.EditValue = "";
                    ZQTMS.Common.CommonClass.SetSelectIndex("", WebProvince);
                    ZQTMS.Common.CommonClass.SetSelectIndex("", WebCity);
                    ZQTMS.Common.CommonClass.SetSelectIndex("", WebArea);
                    ZQTMS.Common.CommonClass.SetSelectIndex("", WebStreet);

                    WebAddress.EditValue = "";
                    WebMan.EditValue = "";
                    WebServiceTel.EditValue = "";
                    WebFetchTel.EditValue = "";
                    WebSendTel.EditValue = "";
                    WebLng.EditValue = "";
                    WebLat.EditValue = "";
                    WebType.EditValue = "";
                    WebLevel.EditValue = "";
                    WebRole.EditValue = "";
                    WebSubName.EditValue = "";
                    WebMiddleSite.EditValue = "";
                    WebRemark.EditValue = "";
                    WebStatus.Checked = true;
                    BelongCause.EditValue = "";
                    BelongArea.EditValue = "";
                    IsHasDirect.EditValue = "";
                    DirectToSites.EditValue = "";
                    WebRight.EditValue = "";
                    IsAcceptShort.Checked = false;
                    StartSiteRange.Text = "";
                    WeightLevel.Text = "";
                    this.Close();
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

        private void BelongCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dsBASAREA
            BelongArea.Properties.Items.Clear();
            DataRow[] dr = dsBASAREA.Tables[0].Select("AreaCause='" + BelongCause.Text.Trim() + "'");
            for (int i = 0; i < dr.Length; i++)
            {
                BelongArea.Properties.Items.Add(dr[i]["AreaName"]);
            }
        }

    }
}