using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using ZQTMS.Tool;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmBasMiddleSiteUpdate : ZQTMS.Tool.BaseForm
    {
        public string Id = "";
        public GridView gv;

        public frmBasMiddleSiteUpdate()
        {
            InitializeComponent();
        }

        private void frmBasMiddleSiteUpdate_Load(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleProvince, "0");

            //放最后
            if (Id == "") return;

            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<SqlPara> list = new List<SqlPara>();
            dic.Add("MiddleSiteId", Id);

            string strSQL = string.Empty;
            frmOrgUnit frm = new frmOrgUnit();
            if (frm != null)
            {
                frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "QSP_GET_BASMIDDLESITE_ByID", dic, ref strSQL);
            }
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }
            list.Add(new SqlPara("strSQL", strSQL));

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "USP_BASMIDDLESITE_OptionSQL", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                Id = "";
                return;
            }
            DataRow dr = ds.Tables[0].Rows[0];

            SiteName.EditValue = dr["SiteName"];
            Destination.EditValue = dr["Destination"];
            WebName.EditValue = dr["WebName"];
            Type.EditValue = dr["Type"];
            MiddleLon.EditValue = dr["MiddleLon"];
            MiddleLat.EditValue = dr["MiddleLat"];
            FetchStorageLoca.EditValue = dr["FetchStorageLoca"];
            SendStorageLoca.EditValue = dr["SendStorageLoca"];
            Ascription.EditValue = dr["Ascription"];
            ShengStore.EditValue = dr["ShengStore"];
            AreaStore.EditValue = dr["AreaStore"];
            MiddleStatus.Checked = ConvertType.ToInt32(dr["MiddleStatus"]) > 0;

            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleProvince"]), MiddleProvince);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleCity"]), MiddleCity);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleArea"]), MiddleArea);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleStreet"]), MiddleStreet);

            GetCompanyId();
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

        private void MiddleProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleCity.Properties.Items.Clear();
                MiddleCity.Text = "";
                if (MiddleProvince.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleCity, MiddleProvince.Properties.Items[MiddleProvince.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void MiddleCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleArea.Properties.Items.Clear();
                MiddleArea.Text = "";
                if (MiddleCity.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleArea, MiddleCity.Properties.Items[MiddleCity.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void MiddleArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleStreet.Properties.Items.Clear();
                MiddleStreet.Text = "";
                if (MiddleArea.Text.Trim() == "")
                {
                    return;
                }
                CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleStreet, MiddleArea.Properties.Items[MiddleArea.SelectedIndex].Value);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message, "系统提示");
            }
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string siteName = SiteName.Text.Trim();
            if (siteName == "")
            {
                MsgBox.ShowOK("请选择隶属站点!");
                SiteName.Focus();
                return;
            }
            string destination = Destination.Text.Trim();
            if (destination == "")
            {
                MsgBox.ShowOK("请填写目的地!");
                Destination.Focus();
                return;
            }
            string middleProvince = MiddleProvince.Text.Trim();
            if (middleProvince == "")
            {
                MsgBox.ShowOK("请选择省份!");
                MiddleProvince.Focus();
                return;
            }
            string middleCity = MiddleCity.Text.Trim();
            if (middleCity == "")
            {
                MsgBox.ShowOK("请选择城市!");
                MiddleCity.Focus();
                return;
            }
            string middleArea = MiddleArea.Text.Trim();
            if (middleArea == "")
            {
                MsgBox.ShowOK("请选择区县!");
                MiddleArea.Focus();
                return;
            }
            string middleStreet = MiddleStreet.Text.Trim();
            if (middleStreet == "")
            {
                MsgBox.ShowOK("请选择街道!");
                MiddleStreet.Focus();
                return;
            }
            string webName = WebName.Text.Trim();
            if (webName == "")
            {
                MsgBox.ShowOK("请选择网点名称!");
                WebName.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("MiddleSiteId", Id);
            dic.Add("SiteName", siteName);
            dic.Add("Destination", destination);
            dic.Add("MiddleProvince", middleProvince);
            dic.Add("MiddleCity", middleCity);
            dic.Add("MiddleArea", middleArea);
            dic.Add("MiddleStreet", middleStreet);
            dic.Add("WebName", webName);
            dic.Add("Type", Type.Text);
            dic.Add("MiddleLon", MiddleLon.Text);
            dic.Add("MiddleLat", MiddleLat.Text);
            dic.Add("FetchStorageLoca", FetchStorageLoca.Text.Trim());
            dic.Add("SendStorageLoca", SendStorageLoca.Text.Trim());
            dic.Add("Ascription", Ascription.Text.Trim());
            dic.Add("ShengStore", ShengStore.Text.Trim());
            dic.Add("AreaStore", AreaStore.Text.Trim());
            dic.Add("MiddleStatus", (MiddleStatus.Checked ? 1 : 0).ToString());
            dic.Add("companyid1", CompanyID.Text.Trim());

            string strSQL = string.Empty;
            frmOrgUnit frm = new frmOrgUnit();
            if (frm != null)
            {
                frm.GetCompanyId_By_Proc(CompanyID.Text.Trim(), "USP_UPDATE_BASMIDDLE", dic, ref strSQL);
            }
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }

            list.Add(new SqlPara("strSQL", strSQL));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list)) == 0) return;

            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                gv.SetRowCellValue(rowhandle, "SiteName", siteName);
                gv.SetRowCellValue(rowhandle, "Destination", destination);
                gv.SetRowCellValue(rowhandle, "MiddleProvince", middleProvince);
                gv.SetRowCellValue(rowhandle, "MiddleCity", middleCity);
                gv.SetRowCellValue(rowhandle, "MiddleArea", middleArea);
                gv.SetRowCellValue(rowhandle, "MiddleStreet", middleStreet);
                gv.SetRowCellValue(rowhandle, "WebName", webName);
                gv.SetRowCellValue(rowhandle, "Type", Type.Text);
                gv.SetRowCellValue(rowhandle, "MiddleLon", ConvertType.ToFloat(MiddleLon.Text));
                gv.SetRowCellValue(rowhandle, "MiddleLat", ConvertType.ToFloat(MiddleLat.Text));
                gv.SetRowCellValue(rowhandle, "FetchStorageLoca", FetchStorageLoca.Text.Trim());
                gv.SetRowCellValue(rowhandle, "SendStorageLoca", SendStorageLoca.Text.Trim());
                gv.SetRowCellValue(rowhandle, "Ascription", Ascription.Text.Trim());
                gv.SetRowCellValue(rowhandle, "ShengStore", ShengStore.Text.Trim());
                gv.SetRowCellValue(rowhandle, "AreaStore", AreaStore.Text.Trim());
                gv.SetRowCellValue(rowhandle, "MiddleStatus", MiddleStatus.Checked ? 1 : 0);
                gv.SetRowCellValue(rowhandle, "companyid", CompanyID.Text.Trim());
            }
            MsgBox.ShowOK("保存成功");
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