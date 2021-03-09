using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAreaDivideBatchUpt : BaseForm
    {
        public frmAreaDivideBatchUpt()
        {
            InitializeComponent();
        }

        private void frmAreaDivideBatchUpt_Load(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleProvince, "0");
            CommonClass.SetWeb(sbjWebCbe, false);
            CommonClass.SetSite(SiteName, false);
        }

        /// <summary>
        /// 保存
        /// </summary>       
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
            string middleProvince = MiddleProvince.Text.Trim();
            //if (middleProvince == "")
            //{
            //    MsgBox.ShowOK("请选择省份!");
            //    MiddleProvince.Focus();
            //    return;
            //}
            if (SiteName.Text.Trim() == "" && middleProvince=="")
            {
                MsgBox.ShowOK("条件必须要有隶属站点或省份!");
                MiddleProvince.Focus();
                return;
            }
            //string middleCity = MiddleCity.Text.Trim();
            //if (middleCity == "")
            //{
            //    MsgBox.ShowOK("请选择城市!");
            //    MiddleCity.Focus();
            //    return;
            //}
            //string middleArea = MiddleArea.Text.Trim();
            //if (middleArea == "")
            //{
            //    MsgBox.ShowOK("请选择区县!");
            //    MiddleArea.Focus();
            //    return;
            //}
            //string middleStreet = MiddleStreet.Text.Trim();
            //if (middleStreet == "")
            //{
            //    MsgBox.ShowOK("请选择街道!");
            //    MiddleStreet.Focus();
            //    return;
            //}

            if (MsgBox.ShowYesNo("当前操作为批量修改不可逆，确定修改吗？") == DialogResult.No)
            {
                return;
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("SiteName", SiteName.Text.Trim());
            dic.Add("MiddleProvince", MiddleProvince.Text.Trim());
            dic.Add("MiddleCity", MiddleCity.Text.Trim());
            dic.Add("MiddleArea", MiddleArea.Text.Trim());
            dic.Add("MiddleStreet", MiddleStreet.Text.Trim());

            dic.Add("CoverageZone", coverageZoneTe.Text.Trim());
            dic.Add("OptCoverage", OptCoverageCbe.Text.Trim());
            dic.Add("SbjWeb", sbjWebCbe.Text.Trim());
            dic.Add("MiddlePartner", middlePartner.Text.Trim());
            dic.Add("Type", cb_Type.Text.Trim());

            string strSQL = string.Empty;
            frmOrgUnit frm = new frmOrgUnit();
            if (frm != null)
            {
                frm.GetCompanyId_By_Proc(CommonClass.UserInfo.companyid, "USP_BATCH_UPDATE_BASMIDDLE_BY_ADDRESS", dic, ref strSQL);
            }
            if (strSQL == "无效存储过程名称")
            {
                MsgBox.ShowOK(strSQL);
                return;
            }

            List<SqlPara> list = new List<SqlPara>();

            //list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
            //list.Add(new SqlPara("MiddleProvince", MiddleProvince.Text.Trim()));
            //list.Add(new SqlPara("MiddleCity", MiddleCity.Text.Trim()));
            //list.Add(new SqlPara("MiddleArea", MiddleArea.Text.Trim()));
            //list.Add(new SqlPara("MiddleStreet", MiddleStreet.Text.Trim()));

            //list.Add(new SqlPara("CoverageZone", coverageZoneTe.Text.Trim()));
            //list.Add(new SqlPara("OptCoverage", OptCoverageCbe.Text.Trim()));
            //list.Add(new SqlPara("SbjWeb", sbjWebCbe.Text.Trim()));
            //list.Add(new SqlPara("MiddlePartner", middlePartner.Text.Trim()));
            //list.Add(new SqlPara("Type", cb_Type.Text.Trim()));
            list.Add(new SqlPara("strSQL", strSQL));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_BASMIDDLESITE_OptionSQL", list)) == 0) return;
            MsgBox.ShowOK("保存成功");
        }
        /// <summary>
        /// 关闭
        /// </summary>      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 省份带出城市
        /// </summary>
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
        /// <summary>
        /// 城市带出区县
        /// </summary>
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
        /// <summary>
        /// 区县带出街道
        /// </summary>
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
    }
}