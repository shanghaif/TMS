using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmTiaoZhangAdd : ZQTMS.Tool.BaseForm
    {
        public frmTiaoZhangAdd()
        {
            InitializeComponent();
        }

        private void frmTiaoZhangAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false, true);

            OperDate.DateTime = CommonClass.gcdate;
            OperMan.Text = CommonClass.UserInfo.UserName;
            InOrOut.Text = "支出";
            
            CommonClass.SetCause(FromMan, false);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string project = Project.Text.Trim();
            if (project == "")
            {
                MsgBox.ShowOK("请选择汇总项目!");
                Project.Focus();
                return;
            }
            string feeType = FeeType.Text.Trim();
            if (feeType == "")
            {
                MsgBox.ShowOK("请选择费用类型!");
                FeeType.Focus();
                return;
            }
            string fromMan = FromMan.Text.Trim();
            if (fromMan == "")
            {
                MsgBox.ShowOK("请选择调账事业部!");
                FromMan.Focus();
                return;
            }
            decimal account = ConvertType.ToDecimal(Account.Text);
            if (account <= 0)
            {
                MsgBox.ShowOK("转账金额必须大于0!");
                Account.Focus();
                return;
            }
            if (AreaName.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须选择大区!");
                AreaName.Focus();
                return;
            }
            string toMan = ToMan.Text.Trim();
            if (toMan == "")
            {
                MsgBox.ShowOK("请选择调账账户名称!");
                ToMan.Focus();
                return;
            }


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Project", project));
            list.Add(new SqlPara("FeeType", feeType));
            list.Add(new SqlPara("FromMan", fromMan));
            list.Add(new SqlPara("Account", account));
            list.Add(new SqlPara("ToMan", toMan));
            list.Add(new SqlPara("Remark", Remark.Text.Trim()));
            list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
            list.Add(new SqlPara("TjNo", CommonClass.gcdate.ToString("yyyyMMddHHmmsss") + new Random().Next(1000, 10000)));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
            list.Add(new SqlPara("InOrOut", InOrOut.Text.Trim()));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_TIAOZHANG", list)) > 0)
            {
                MsgBox.ShowOK();
                this.Close();
            }
        }

        private void Project_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeeType.Properties.Items.Clear();
            FeeType.Text = "";
            string[] str=null;
            if (InOrOut.Text.Trim() == "收入")
            {
                if (Project.Text.Trim() == "扣返其他费")
                {
                    str = CommonClass.Arg.TotalOtherAcc.Split(',');
                }
                else if (Project.Text.Trim() == "异动款项")
                {
                    str = CommonClass.Arg.TotalTransaction.Split(',');
                }
            }
            else
            {
                if (Project.Text.Trim() == "扣返其他费")
                {
                    str = CommonClass.Arg.TotalOtherAccOut.Split(',');
                }
                else if (Project.Text.Trim() == "异动款项")
                {
                    str = CommonClass.Arg.TotalTransactionOut.Split(',');
                }
            }
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    FeeType.Properties.Items.Add(str[i]);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FromMan_SelectedIndexChanged(object sender, EventArgs e)
        {
            AreaName.Text = "";
            CommonClass.SetArea(AreaName, FromMan.Text, false);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToMan.Text = "";
            //CommonClass.SetCauseWeb(ToMan, FromMan.Text, AreaName.Text, false);

            try
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("BelongCause like '" + FromMan.Text + "' and BelongArea like '" + AreaName.Text + "' and (WebRole='加盟' or WebRole='合作') ");
                ToMan.Properties.Items.Clear();
                ToMan.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    ToMan.Properties.Items.Add(dr[i]["WebName"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }

        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Project.Text = "";
            FeeType.Text = "";
            Project.Properties.Items.Clear();
            if (InOrOut.Text.Trim() == "收入")
            {
                Project.Properties.Items.Add("扣返其他费");
            }
            else
            {
                Project.Properties.Items.Add("扣返其他费");
                Project.Properties.Items.Add("异动款项");
            }
        }
    }
}