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
    public partial class fmSurchargeFeeAddNew : BaseForm
    {
        public DataRow dr = null;
        public fmSurchargeFeeAddNew()
        {
            InitializeComponent();
        }

        private void fmSurchargeFeeAdd_Load(object sender, EventArgs e)
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
            if (dr != null)
            {
                ProjectType.Text = dr["ProjectType"].ToString();
                JudgeColumn.Text = dr["JudgeColumn"].ToString();
                JudgeType.Text = dr["JudgeType"].ToString();
                ParcelWeight.Text = dr["ParcelWeight"].ToString();
                ParcelVolume.Text = dr["ParcelVolume"].ToString();
                ParcelPriceMin.Text = dr["ParcelPriceMin"].ToString();
                ParcelPriceMax.Text = dr["ParcelPriceMax"].ToString();
                FeeRate.Text = dr["FeeRate"].ToString();
                FeeColumn.Text = dr["FeeColumn"].ToString();
                Unit.Text = dr["Unit"].ToString();
                Remark.Text = dr["Remark"].ToString();


                OutLowest.Text = dr["OutLowest"].ToString();
                OutStandard.Text = dr["OutStandard"].ToString();
                OutStandardRemark.Text = dr["OutStandardRemark"].ToString();
                InnerLowest.Text = dr["InnerLowest"].ToString();
                InnerStandard.Text = dr["InnerStandard"].ToString();
                InnerStandardRemark.Text = dr["InnerStandardRemark"].ToString();
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
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SurchargeFeeID", dr == null ? Guid.NewGuid() : dr["SurchargeFeeID"]));
                list.Add(new SqlPara("ProjectType", ProjectType.Text.Trim()));
                list.Add(new SqlPara("JudgeColumn", JudgeColumn.Text.Trim()));
                list.Add(new SqlPara("JudgeType", JudgeType.Text.Trim()));
                list.Add(new SqlPara("ParcelWeight", ParcelWeight.Text.Trim()));
                list.Add(new SqlPara("ParcelVolume", ParcelVolume.Text.Trim()));
                list.Add(new SqlPara("ParcelPriceMin", ParcelPriceMin.Text.Trim()));
                list.Add(new SqlPara("ParcelPriceMax", ParcelPriceMax.Text.Trim()));
                list.Add(new SqlPara("FeeRate", FeeRate.Text.Trim()));
                list.Add(new SqlPara("FeeColumn", FeeColumn.Text.Trim()));
                list.Add(new SqlPara("Unit", Unit.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));

                list.Add(new SqlPara("OutLowest", ConvertType.ToDecimal(OutLowest.Text)));
                list.Add(new SqlPara("OutStandard", OutStandard.Text.Trim()));
                list.Add(new SqlPara("OutStandardRemark", OutStandardRemark.Text.Trim()));

                list.Add(new SqlPara("InnerLowest", ConvertType.ToDecimal(InnerLowest.Text)));
                list.Add(new SqlPara("InnerStandard", ConvertType.ToDecimal(InnerStandard.Text)));
                list.Add(new SqlPara("InnerStandardRemark", InnerStandardRemark.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASSURCHARGEFEE_GX", list);
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

        private void JudgeColumn_SelectedIndexChanged(object sender, EventArgs e)
        {
            JudgeType.Text = "";
            JudgeType.Properties.Items.Clear();
            string judgeColumn = JudgeColumn.Text.Trim();
            if (judgeColumn == "重量/体积")
            {
                JudgeType.Properties.Items.Add("小件");
                JudgeType.Properties.Items.Add("大件");
                JudgeType.Text = "小件";
            }
            else if (judgeColumn == "包装")
            {
                JudgeType.Text = "纸箱";
                JudgeType.Properties.Items.Add("纸箱");
                JudgeType.Properties.Items.Add("纤袋");
                JudgeType.Properties.Items.Add("木架");
                JudgeType.Properties.Items.Add("木箱");
                JudgeType.Properties.Items.Add("托盘");
                JudgeType.Properties.Items.Add("膜");
            }
            else if (judgeColumn == "运输方式")
            {
                JudgeType.Text = "零担";
                JudgeType.Properties.Items.Add("零担");
                JudgeType.Properties.Items.Add("整车");
            }
            else if (judgeColumn == "到站")
            {
                JudgeType.Text = "直达";
                JudgeType.Properties.Items.Add("直达");
                JudgeType.Properties.Items.Add("中转");
            }
        }

        private void ProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeeColumn.Text = "";
            FeeColumn.Properties.Items.Clear();
            string projectType = ProjectType.Text.Trim();
            if (projectType == "保价费")
            {
                FeeColumn.Text = "声明价值";
                FeeColumn.Properties.Items.Add("声明价值");
            }
            else if (projectType == "税金")
            {
                FeeColumn.Text = "总运费";
                FeeColumn.Properties.Items.Add("总运费");
            }
            else if (projectType == "代收手续费")
            {
                FeeColumn.Text = "代收货款";
                FeeColumn.Properties.Items.Add("代收货款");
            }
            else if (projectType == "上楼费" || projectType == "装卸费" || projectType == "叉车费")
            {
                FeeColumn.Text = "结算重量";
                FeeColumn.Properties.Items.Add("结算重量");
            }
            else if (projectType == "包装费" || projectType == "代打木架")
            {
                FeeColumn.Text = "购买数量";
                FeeColumn.Properties.Items.Add("购买数量");
            }
            else if (projectType == "信息费")
            {
                FeeColumn.Text = "信息费";
                FeeColumn.Properties.Items.Add("信息费");
            }
            else if (projectType == "工本费")
            {
                FeeColumn.Text = "工本费";
                FeeColumn.Properties.Items.Add("工本费");
            }
            else if (projectType == "燃油附加费")
            {
                FeeColumn.Text = "燃油附加费";
                FeeColumn.Properties.Items.Add("燃油附加费");
            }
            else if (projectType == "回单费")
            {
                FeeColumn.Text = "回单费";
                FeeColumn.Properties.Items.Add("回单费");
            }
            else if (projectType == "报关费")
            {
                FeeColumn.Text = "报关费";
                FeeColumn.Properties.Items.Add("报关费");
            }
            else if (projectType == "控货费")
            {
                FeeColumn.Text = "控货费";
                FeeColumn.Properties.Items.Add("控货费");
            }
            else if (projectType == "改单费")
            {
                FeeColumn.Text = "改单费";
                FeeColumn.Properties.Items.Add("改单费");
            }
            else if (projectType == "仓储费")
            {
                FeeColumn.Text = "仓储费";
                FeeColumn.Properties.Items.Add("仓储费");
            }
            else
            {
                FeeColumn.Text = "体积";
                FeeColumn.Properties.Items.Add("体积");
            }
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
