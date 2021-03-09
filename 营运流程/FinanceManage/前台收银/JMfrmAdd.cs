using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class JMfrmadd : BaseForm
    {
        public string ID1="",entryname1 = "", Senddate1 = "", Amount1 = "", Registrant1 = "", Redate1 = "", Redepartment1 = "", abstract1 = "", Remarks1 = ""
            , CauseName1 = "", AreaName1 = "", WebName1 = "", FSWeb1="";
        public int ismodify = 0;
        public JMfrmadd()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
            if(check()){
             if(ismodify==1){
                 List<SqlPara> list = new List<SqlPara>();
                 list.Add(new SqlPara("id", ID1 == "" ? Guid.NewGuid().ToString() : ID1));
                 list.Add(new SqlPara("entryname", entryname.Text.Trim()));
                 list.Add(new SqlPara("Senddate", Senddate.Text.Trim()));
                 list.Add(new SqlPara("Amount", Amount.Text.Trim()));
                 list.Add(new SqlPara("Registrant", Registrant.Text.Trim()));
                 list.Add(new SqlPara("Redate", Redate.Text.Trim()));
                 list.Add(new SqlPara("Redepartment", Redepartment.Text.Trim()));
                 list.Add(new SqlPara("abstract", Abstract.Text.Trim()));
                 list.Add(new SqlPara("Remarks", Remarks.Text.Trim()));
                 list.Add(new SqlPara("CauseName",CauseName.Text.Trim()));
                 list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                 list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                 list.Add(new SqlPara("FSWeb", FSWeb.Text.Trim()));
                 SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "FM_MODIFYY_YYWSR", list);
                 if (SqlHelper.ExecteNonQuery(sps) > 0)
                 {
                     MsgBox.ShowOK("保存成功");
                     this.Close();
                 }
                 else
                 {
                     MsgBox.ShowOK("保存不成功");
                 }
                 return;
             }

            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("id", ID1 == "" ? Guid.NewGuid().ToString() : ID1));
            list1.Add(new SqlPara("entryname", entryname.Text.Trim()));
            list1.Add(new SqlPara("Senddate", Senddate.Text.Trim()));
            list1.Add(new SqlPara("Amount", Amount.Text.Trim()));
            list1.Add(new SqlPara("Registrant", Registrant.Text.Trim()));
            list1.Add(new SqlPara("Redate", Redate.Text.Trim()));
            list1.Add(new SqlPara("Redepartment", Redepartment.Text.Trim()));
            list1.Add(new SqlPara("abstract", Abstract.Text.Trim()));
            list1.Add(new SqlPara("Remarks", Remarks.Text.Trim()));
            list1.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
            list1.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
            list1.Add(new SqlPara("WebName", WebName.Text.Trim()));
            list1.Add(new SqlPara("auditstatus", "未审核"));
            list1.Add(new SqlPara("FSWeb", FSWeb.Text.Trim()));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "FM_ADD_YYWSR", list1);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK("保存成功");
                this.Close();
                
            }
            else
            {
                MsgBox.ShowOK("保存不成功");
            }
            }

        }
        private bool check()
        {
            if (this.entryname.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入项目名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Senddate.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入发送时间", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Amount.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Registrant.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入登记人", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Redate.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入登记日期", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Redepartment.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入登记部门", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.Abstract.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入摘要", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (this.FSWeb.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入发生部门", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //if (this.Remarks.Text.Trim() == "")
            //{
            //    XtraMessageBox.Show("请输入备注", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}

            return true;
        }

        private void JMfrmAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            Redate.Enabled = false;
            if(ismodify==1){
            if (!string.IsNullOrEmpty(ID1)) {
                entryname.Text = entryname1;
                Senddate.Text = Senddate1;
                Amount.Text = Amount1;
                Registrant.Text = Registrant1;
                Redate.Text = Redate1;
                Redepartment.Text = Redepartment1;
                Abstract.Text=abstract1;
                Remarks.Text = Remarks1;
                CauseName.Text = CauseName1;
                AreaName.Text = AreaName1;
                WebName.Text = WebName1;
                CommonClass.SetWeb(FSWeb, false);
                FSWeb.Text = FSWeb1;
               
            }
            return;
            }

            Registrant.Text=CommonClass.UserInfo.UserName;
            Redepartment.Text = CommonClass.UserInfo.DepartName;
            Redate.Text =(System.DateTime.Now).ToString();
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
            CommonClass.SetWeb(FSWeb, false);
        }

      
      
        

       
    }
}
