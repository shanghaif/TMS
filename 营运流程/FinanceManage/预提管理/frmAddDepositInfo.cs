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
    public partial class frmAddDepositInfo : BaseForm
    {
        #region 修改字段
        public string id { get; set; }
        public string cause { get; set; }
        public string area { get; set; }
        public string web { get; set; }
        public string proType { get; set; }
        public string proSub { get; set; }
        public string mark { get; set; }
        public decimal proAmount { get; set; }
        public string flowID { get; set; }
        #endregion
        public frmAddDepositInfo()
        {
            InitializeComponent();
        }

        private void frmAddDepositInfo_Load(object sender, EventArgs e)
        {
            CommonClass.SetCause(txtCause, false);
            CommonClass.SetArea(txtArea, txtCause.Text, false);
            txtCause.Text = CommonClass.UserInfo.CauseName;
            txtArea.Text = CommonClass.UserInfo.AreaName;
            txtWeb.Text = CommonClass.UserInfo.DepartName;
            if (!string.IsNullOrEmpty(id))
            {
                init();
            }
        }

        public void init()
        {
            txtCause.Text = cause;
            txtArea.Text = area;
            txtWeb.Text = web;
            //txtDepositType.Text = proType;
            txtDepositSub.Text = proSub;
            txtDepositAmount.Text = proAmount.ToString();
            txtRemark.Text = mark;
            txtFlowID.Text = flowID;
        }

        private void txtCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWeb.Text.Trim()))
            {
                txtWeb.Text = "";
            }
            CommonClass.SetArea(txtArea,txtCause.Text.Trim(),false);
            //CommonClass.SetCauseWeb(txtWeb,txtCause.Text.Trim(),txtArea.Text.Trim(),false);
            CommonClass.SetDep(txtWeb, txtArea.Text.Trim(), false);
        }

        private void txtArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWeb.Text.Trim()))
            {
                txtWeb.Text = "";
            }
            //CommonClass.SetCauseWeb(txtWeb, txtCause.Text.Trim(), txtArea.Text.Trim(), false);
            CommonClass.SetDep(txtWeb, txtArea.Text.Trim(), false);
        }



        ///// <summary>
        ///// 预提类型
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void txtDepositType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string DepositType = txtDepositType.Text.Trim();
        //    #region 预提类型--预提科目
        //    switch (DepositType)
        //    {
        //        case "成本类":
        //            if (txtDepositSub.Properties.Items.Count > 0)
        //            {
        //                txtDepositSub.Properties.Items.Clear();
        //                txtDepositSub.Text = "";
        //            }
        //            txtDepositSub.Properties.Items.Add("车辆加油费");
        //            txtDepositSub.Properties.Items.Add("车辆维修费");
        //            txtDepositSub.Properties.Items.Add("车辆路桥费");
        //            txtDepositSub.Properties.Items.Add("车辆年审费");
        //            break;
        //        case "费用类":
        //            if (txtDepositSub.Properties.Items.Count > 0)
        //            {
        //                txtDepositSub.Properties.Items.Clear();
        //                txtDepositSub.Text = "";
        //            }
        //            txtDepositSub.Properties.Items.Add("生活费");
        //            txtDepositSub.Properties.Items.Add("社保费");
        //            txtDepositSub.Properties.Items.Add("保险费");
        //            txtDepositSub.Properties.Items.Add("档口房租");
        //            txtDepositSub.Properties.Items.Add("宿舍房租");
        //            txtDepositSub.Properties.Items.Add("水电费");

        //            txtDepositSub.Properties.Items.Add("办公费");
        //            txtDepositSub.Properties.Items.Add("快递费");
        //            txtDepositSub.Properties.Items.Add("垫板费");
        //            txtDepositSub.Properties.Items.Add("通讯费");
        //            txtDepositSub.Properties.Items.Add("差旅费");
        //            txtDepositSub.Properties.Items.Add("会议费");

        //            txtDepositSub.Properties.Items.Add("培训费");
        //            txtDepositSub.Properties.Items.Add("业务费");
        //            txtDepositSub.Properties.Items.Add("劳务费");
        //            txtDepositSub.Properties.Items.Add("招聘费");
        //            txtDepositSub.Properties.Items.Add("工程/资产维修费");
        //            txtDepositSub.Properties.Items.Add("装修费");

        //            txtDepositSub.Properties.Items.Add("叉车费");
        //            txtDepositSub.Properties.Items.Add("广告费");
        //            txtDepositSub.Properties.Items.Add("福利费");
        //            txtDepositSub.Properties.Items.Add("工资（非部门）");
        //            txtDepositSub.Properties.Items.Add("奖金");
        //            txtDepositSub.Properties.Items.Add("货量折扣");
        //            txtDepositSub.Properties.Items.Add("交通费");
        //            break;
        //        case "其他类":
        //            if (txtDepositSub.Properties.Items.Count > 0)
        //            {
        //                txtDepositSub.Properties.Items.Clear();
        //                txtDepositSub.Text = "";
        //            }
        //            txtDepositSub.Properties.Items.Add("财务费用");
        //            txtDepositSub.Properties.Items.Add("管理费用");
        //            txtDepositSub.Properties.Items.Add("税金");
        //            txtDepositSub.Properties.Items.Add("营业外支出");
        //            break;
        //    #endregion
        //    }
        //}
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal depositAmount = 0;
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                if (string.IsNullOrEmpty(txtCause.Text.Trim()) || string.IsNullOrEmpty(txtArea.Text.Trim()) || string.IsNullOrEmpty(txtWeb.Text.Trim()) //|| string.IsNullOrEmpty(txtDepositType.Text.Trim())
                    || string.IsNullOrEmpty(txtDepositSub.Text.Trim()) || string.IsNullOrEmpty(txtDepositAmount.Text.Trim()) || string.IsNullOrEmpty(txtFlowID.Text.Trim()) || string.IsNullOrEmpty(txtRemark.Text.Trim()))
                {
                    XtraMessageBox.Show("信息填写不完整，请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!StringHelper.IsDecimal(txtDepositAmount.Text.Trim()))
                {
                    MsgBox.ShowOK("预提金额格式不正确！");
                    return;
                }
                depositAmount = ConvertType.ToDecimal(txtDepositAmount.Text.Trim());
                if (depositAmount<=0)
                {
                    MsgBox.ShowOK("预提金额要大于0");
                    return;
                }
                
                list.Add(new SqlPara("ID", string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id));
                list.Add(new SqlPara("CauseName",txtCause.Text.Trim()));
                list.Add(new SqlPara("AreaName",txtArea.Text .Trim()));
                list.Add(new SqlPara("WebName",txtWeb.Text.Trim()));
                //list.Add(new SqlPara("DepositType",txtDepositType.Text.Trim()));
                list.Add(new SqlPara("DepositType", ""));
                list.Add(new SqlPara("DepositSub",txtDepositSub.Text.Trim()));
                list.Add(new SqlPara("DepositAmount",txtDepositAmount.Text.Trim()));
                list.Add(new SqlPara("flowId",txtFlowID.Text.Trim()));
                list.Add(new SqlPara("remark",txtRemark.Text.Trim()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_ADD_YAJIN_Info", list)) > 0)
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
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}