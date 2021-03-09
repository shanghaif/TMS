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
    public partial class frmWebAdd_NEW : BaseForm
    {
        public frmWebAdd_NEW()
        {
            InitializeComponent();
        }
        public DataRow dr = null; 
        string WebId = "";
        private DataSet dsBASAREA = new DataSet();
        string sProvince = "";
        string sCity = "";
        string sArea = "";
        string sStreet = "";

        private void frmOrgUnit_Web_Add_Load(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(WebProvince, "0");
            WebProvince.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            WebCity.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            WebArea.SelectedIndexChanged += new System.EventHandler(this.edarea_SelectedIndexChanged);
            SiteAll();

            CommonClass.SetCause(BelongCause, false);
            BelongCause.Text = CommonClass.UserInfo.CauseName;
            //freshCause();//只能添加当前事业部的网点
            //freshArea();
            if (CommonClass.UserInfo.SiteName.Contains("总部")) BelongCause.Enabled = true;

            if (dr != null)
            {
                //不给修改
                WebCode.Enabled = InternalCode.Enabled = SiteName.Enabled = WebName.Enabled =
                    WebRole.Enabled = BelongCause.Enabled = BelongArea.Enabled = false;
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
                    WebType.EditValue = dr["WebType"];
                    WebLevel.EditValue = dr["WebLevel"];
                    WebRole.EditValue = dr["WebRole"];
                    WebSubName.EditValue = dr["WebSubName"];
                    WebRemark.EditValue = dr["WebRemark"];
                    BelongCause.EditValue = dr["BelongCause"];
                    BelongArea.EditValue = dr["BelongArea"];
                    txtAllocateCompanyID.EditValue = dr["AllocateCompanyID"];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowError(ex.Message);
                }
                if (CommonClass.UserInfo.SiteName.Contains("总部") == false && ConvertType.ToString(dr["BelongCause"]) != CommonClass.UserInfo.CauseName && dr != null)
                {
                    simpleButton1.Enabled = false;
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
            sProvince = WebProvince.Text;
            WebAddress.Text = sProvince + sCity + sArea + sStreet;
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebArea, WebCity.EditValue);
            sCity = WebCity.Text;
            WebAddress.Text = sProvince + sCity + sArea + sStreet;
        }

        private void edarea_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(WebStreet, WebArea.EditValue);
            sArea = WebArea.Text;
            WebAddress.Text = sProvince + sCity + sArea + sStreet;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (WebCode.Text.Trim() == "" || WebName.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写网点代码和名字！");
                return;
            }
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
                list.Add(new SqlPara("WebType", WebType.Text.Trim()));
                list.Add(new SqlPara("WebLevel", WebLevel.Text.Trim()));
                list.Add(new SqlPara("WebRole", WebRole.Text.Trim()));
                list.Add(new SqlPara("WebSubName", WebSubName.Text.Trim()));
                list.Add(new SqlPara("WebRemark", WebRemark.Text.Trim()));
                list.Add(new SqlPara("BelongCause", BelongCause.Text.Trim()));
                list.Add(new SqlPara("BelongArea", BelongArea.Text.Trim()));
                //增加分拨公司参数 zaj 2017-11-20
                list.Add(new SqlPara("AllocateCompanyID",txtAllocateCompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASWEB_CONTROL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    WebCode.EditValue = "";
                    InternalCode.EditValue = "";
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
                    WebType.EditValue = "";
                    WebLevel.EditValue = "";
                    WebSubName.EditValue = "";
                    WebRemark.EditValue = "";
                    txtAllocateCompanyID.EditValue = "";

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

        private void BelongArea_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetDep(WebName, BelongArea.EditValue.ToString(), false);
        }

        private void WebStreet_SelectedIndexChanged(object sender, EventArgs e)
        {
            sStreet = WebStreet.Text;
            WebAddress.Text = sProvince + sCity + sArea + sStreet;
        }

        private void WebAddress_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void WebAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back && WebAddress.Text == sProvince + sCity + sArea + sStreet)
            {
                e.Handled = true;
            }
        }

        private void BelongCause_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(BelongArea, BelongCause.Text, false);

            //BelongArea.Properties.Items.Clear();
            //DataRow[] dr = dsBASAREA.Tables[0].Select("AreaCause='" + BelongCause.Text.Trim() + "'");
            //for (int i = 0; i < dr.Length; i++)
            //{
            //    BelongArea.Properties.Items.Add(dr[i]["AreaName"]);
            //}
        }

        private void buttonEdit1_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }
    }
}