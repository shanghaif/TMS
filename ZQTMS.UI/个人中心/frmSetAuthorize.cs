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
    public partial class frmSetAuthorize : BaseForm
    {
        public frmSetAuthorize()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_AUTHORIZE", new List<SqlPara> { new SqlPara("UserAccount", CommonClass.UserInfo.UserAccount), new SqlPara("Authorize", Authorize.Text.Trim()) })) == 0) return;

            MsgBox.ShowOK("设置成功");
        }

        private void frmSetAuthorize_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("授权码设置");//xj/2019/5/29
            UserAccount.Text = CommonClass.UserInfo.UserAccount;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_USER_AUTHORIZE"));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            Authorize.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["Authorize"]);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}