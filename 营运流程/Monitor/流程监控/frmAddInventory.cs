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
    public partial class frmAddInventory : BaseForm
    {
        public frmAddInventory()
        {
            InitializeComponent();
        }

        private void frmAddInventory_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(txtInventorySite,false);
            txtInventoryDate.DateTime = CommonClass.gcdate;
            txtInventorySite.Text = CommonClass.UserInfo.SiteName;
            txtInventoryWeb.Text = CommonClass.UserInfo.WebName;
            string PDBatchNo = RandStr();
            label7.Text = PDBatchNo;//盘点批次号
            string loginSite = CommonClass.UserInfo.SiteName;
            if (loginSite!="总部")
            {
                txtInventorySite.Enabled = false;
                txtInventoryWeb.Enabled = false;
            }
        }

        private void txtInventorySite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(txtInventoryWeb,txtInventorySite.Text.Trim(),false);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 返回盘点批次
        /// </summary>
        /// <returns></returns>
        public static string RandStr()
        {
            return "PD" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Number(2, false);
        }
        /// <summary>
        /// 随机生成不重复的两位数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        /// <returns></returns>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep)
                System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtInventoryDate.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择预计盘点完成时间！");
                    return;
                }
                if (string.IsNullOrEmpty(txtInventorySite.Text.Trim()))
                {
                    MsgBox.ShowOK("盘点站点不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(txtInventoryWeb.Text.Trim()))
                {
                    MsgBox.ShowOK("盘点网点不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(txtInvCommond.Text.Trim()))
                {
                    MsgBox.ShowOK("请填写盘点要求！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("InvBatch",label7.Text.Trim()));
                list.Add(new SqlPara("invEndDate",txtInventoryDate.Text.Trim()));
                list.Add(new SqlPara("invRequire",txtInvCommond.Text.Trim()));
                list.Add(new SqlPara("invRemark",txtInvMark.Text.Trim()));
                list.Add(new SqlPara("sitename", txtInventorySite.Text.Trim()));
                list.Add(new SqlPara("webname",txtInventoryWeb.Text.Trim()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_InventoryProJect", list))==0) return;
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        } 
    }
}