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
using System.Text.RegularExpressions;
namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmCostManageAdd : ZQTMS.Tool.BaseForm
    {
        public frmCostManageAdd()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        public string ControlId = "";
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ProjectName.Text))
                {
                    MsgBox.ShowOK("项目类型不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(SiteName.Text)&&this.ProjectName.SelectedIndex==0)
                {

                    MsgBox.ShowOK("选择了汽运成本类型始发站不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(this.DestinationSite.Text) && this.ProjectName.SelectedIndex ==0)
                {
                    MsgBox.ShowOK("选择了汽运成本类型目的站不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(this.SendWebname.Text)&&this.ProjectName.SelectedIndex==1)
                {
                    MsgBox.ShowOK("选择了送货成本类型送货网点不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(this.MiddleWebName.Text)&&this.ProjectName.SelectedIndex==2)
                {
                    MsgBox.ShowOK("选择了中转成本类型中转网点不能为空");
                    return;
                }
                if (Convert.ToDecimal(this.TargetcostRate.Text.Trim().ToString()) < 0 || Convert.ToDecimal(this.TargetcostRate.Text.ToString().Trim()) > 1)
                {
                    MsgBox.ShowOK("请输入正确的成本率");
                    return;
                }
                if(!IsPhoneNo(this.TelPhone.Text.Trim()))
                {
                     MsgBox.ShowOK("请输入正确手机号");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",dr == null ? Guid.NewGuid() : dr["ID"] ));
                list.Add(new SqlPara("ProjectType", this.ProjectName.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.ToString()));
                list.Add(new SqlPara("DestinationSite", DestinationSite.Text.ToString()));
                list.Add(new SqlPara("SendWebname", SendWebname.Text.ToString()));
                list.Add(new SqlPara("MiddleWebName", MiddleWebName.Text.Trim()));
                list.Add(new SqlPara("TargetcostRate", TargetcostRate.Text.ToString()));
                list.Add(new SqlPara("ChargePerson", ChargePerson.Text.ToString()));
                list.Add(new SqlPara("TelPhone", TelPhone.Text.ToString()));
                list.Add(new SqlPara("Remark", this.richTextBox1.Text.ToString()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_CostControls", list);//保存
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static bool IsPhoneNo(string phone)
        {
            return Regex.IsMatch(phone, "^(13[0-9]|14[579]|15[0-3,5-9]|16[6]|17[0135678]|18[0-9]|19[89])\\d{8}$");
        }
        private void ProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectName.SelectedIndex == 0)
            {
                this.SendWebname.Enabled = false;
                this.MiddleWebName.Enabled = false;
                this.SiteName.Enabled = true;
                this.DestinationSite.Enabled = true;
            

            }
            else if (this.ProjectName.SelectedIndex == 1)
            {
                this.SiteName.Enabled = false;
                this.DestinationSite.Enabled = false;
                this.MiddleWebName.Enabled =false;
                this.SendWebname.Enabled = true;
             


            }
            else if (this.ProjectName.SelectedIndex == 2)
            {

                this.SiteName.Enabled = false;
                this.DestinationSite.Enabled = false;
                this.SendWebname.Enabled = false;
                this.MiddleWebName.Enabled = true;
            }
        }

        private void TargetcostRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar!=8&&!Char.IsDigit(e.KeyChar)&&e.KeyChar!='.')
　　     {
　　           e.Handled = true;
　    　}
        }

        private void frmCostManageAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.SetSite(SiteName,false);
            CommonClass.SetSite(DestinationSite, false);
            CommonClass.SetWeb(SendWebname, false);
            CommonClass.SetWeb(MiddleWebName, false);


            SiteName.Text = CommonClass.UserInfo.SiteName;
            DestinationSite.Text = CommonClass.UserInfo.SiteName;
            SendWebname.Text = CommonClass.UserInfo.WebName;
            MiddleWebName.Text = CommonClass.UserInfo.WebName;

            if (dr != null)
            {
                this.ProjectName.EditValue = dr["ProjectType"];
                this.SiteName.EditValue = dr["StartSite"];
                this.DestinationSite.EditValue = dr["DestinationSite"];
                this.SendWebname.EditValue = dr["SendWebname"];
                this.MiddleWebName.Text = dr["MiddleWebName"].ToString();
                this.TargetcostRate.EditValue = dr["TargetcostRate"];
                this.ChargePerson.Text = dr["ChargePerson"].ToString();
                this.TelPhone.Text= dr["TelPhone"].ToString();
                this.richTextBox1.Text = dr["Remark"].ToString();
                //this.ProjectName.Enabled = false;



            }
        }

        private void TelPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

       
    }
}
