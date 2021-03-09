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

namespace ZQTMS.UI.BaseInfoManage.运作架构
{
    public partial class frmOrgUnitShort_Manage : BaseForm
    {
        public frmOrgUnitShort_Manage()
        {
            InitializeComponent();
        }
        public string Id = "";
        public GridView gv;
        //加载
        private void frmOrgUnitShort_Manage_Load(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleProvince, "0");
            GetCompanyId();
            CommonClass.SetSite(false, SiteName);
            CommonClass.SetWeb(WebName, "%%", false);

            //放最后
            if (Id == "")
            {
                this.Text = "新增短线路由";
                return;
            }
            else
            {
                this.Text = "修改短线路由";
            }
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BASMIDDLESITESHORT_ByID", new List<SqlPara> { new SqlPara("MiddleSiteId", Id) }));
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
            Ascription.EditValue = dr["Ascription"];

            CommonClass.SetWeb(SbjWeb, dr["SiteName"] as string);

            SbjWeb.EditValue = dr["SbjWeb"];
            MiddleStatus.Checked = ConvertType.ToInt32(dr["MiddleStatus"]) > 0;

            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleProvince"]), MiddleProvince);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleCity"]), MiddleCity);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleArea"]), MiddleArea);
            CommonClass.SetSelectIndexByValue(ConvertType.ToString(dr["MiddleStreet"]), MiddleStreet);

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

        //获取公司ID
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

        //关闭
        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //保存
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
            //if (middleStreet == "")
            //{
            //    MsgBox.ShowOK("请选择街道!");
            //    MiddleStreet.Focus();
            //    return;
            //}
            string webName = WebName.Text.Trim();
            if (webName == "")
            {
                MsgBox.ShowOK("请选择网点名称!");
                WebName.Focus();
                return;
            }
            string type = Type.Text.Trim();
            if (type == "")
            {
                MsgBox.ShowOK("请选择服务类型!");
                Type.Focus();
                return;
            }
            string companyID = CompanyID.Text.Trim();
            if (companyID == "")
            {
                MsgBox.ShowOK("请选择公司ID!");
                CompanyID.Focus();
                return;
            }
            string ascription = Ascription.Text.Trim();
            if (ascription == "")
            {
                MsgBox.ShowOK("请输入短线归属地!");
                Ascription.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SiteName", siteName));
            list.Add(new SqlPara("Destination", destination));
            list.Add(new SqlPara("MiddleProvince", middleProvince));
            list.Add(new SqlPara("MiddleCity", middleCity));
            list.Add(new SqlPara("MiddleArea", middleArea));
            list.Add(new SqlPara("MiddleStreet", middleStreet));
            list.Add(new SqlPara("WebName", webName));
            list.Add(new SqlPara("Type", Type.Text));
            list.Add(new SqlPara("MiddleLon", 0));
            list.Add(new SqlPara("MiddleLat", 0));
            list.Add(new SqlPara("FetchStorageLoca", ""));
            list.Add(new SqlPara("SendStorageLoca", ""));
            list.Add(new SqlPara("Ascription", Ascription.Text.Trim()));
            list.Add(new SqlPara("ShengStore", ""));
            list.Add(new SqlPara("AreaStore", ""));
            list.Add(new SqlPara("MiddleStatus", MiddleStatus.Checked ? 1 : 0));
            list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
            list.Add(new SqlPara("SbjWeb", SbjWeb.Text.Trim()));//隶属分拨中心

            if (Id == "")
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BASMIDDLESITESHORT", list)) == 0) return;
            }
            else
            {
                list.Add(new SqlPara("MiddleSiteId", Id));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASMIDDLESITESHORT", list)) == 0) return;
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
                    //gv.SetRowCellValue(rowhandle, "MiddleLon", ConvertType.ToFloat(MiddleLon.Text));
                    //gv.SetRowCellValue(rowhandle, "MiddleLat", ConvertType.ToFloat(MiddleLat.Text));
                    //gv.SetRowCellValue(rowhandle, "FetchStorageLoca", FetchStorageLoca.Text.Trim());
                    //gv.SetRowCellValue(rowhandle, "SendStorageLoca", SendStorageLoca.Text.Trim());
                    //gv.SetRowCellValue(rowhandle, "Ascription", Ascription.Text.Trim());
                    //gv.SetRowCellValue(rowhandle, "ShengStore", ShengStore.Text.Trim());
                    //gv.SetRowCellValue(rowhandle, "AreaStore", AreaStore.Text.Trim());
                    gv.SetRowCellValue(rowhandle, "MiddleStatus", MiddleStatus.Checked ? 1 : 0);
                    gv.SetRowCellValue(rowhandle, "companyid", CompanyID.Text.Trim());
                }
            }
            
            MsgBox.ShowOK("保存成功");
            Clear();//清空文本框
        }

        //清空
        private void Clear() 
        {
            this.SiteName.Text = string.Empty;
            this.Destination.Text = string.Empty;
            this.MiddleProvince.Text = string.Empty;
            this.MiddleCity.Text = string.Empty;
            this.MiddleArea.Text = string.Empty;
            this.MiddleStreet.Text = string.Empty;
            this.WebName.Text = string.Empty;
            this.Type.SelectedIndex = 0;
            this.CompanyID.Text = string.Empty;
            this.SbjWeb.Text = string.Empty;
            this.Ascription.Text = string.Empty;
        }

        private void SiteName_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.SiteName.Text.Trim()))
            {
                CommonClass.SetWeb(SbjWeb, this.SiteName.Text.Trim());
            }
        }
    }
}
