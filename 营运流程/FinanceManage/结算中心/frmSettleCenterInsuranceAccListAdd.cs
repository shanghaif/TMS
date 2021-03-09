using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using DevExpress.XtraEditors.Controls;
using ZQTMS.SqlDAL;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmSettleCenterInsuranceAccListAdd :BaseForm
    {
        int accountID = 1000;
        int state = 0;
        public frmSettleCenterInsuranceAccListAdd()
        {
            InitializeComponent();
        }

        private void frmAddPart_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            getAccountId();
            GetCompanyName(CommonClass.UserInfo.companyid);

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("gsqj ", gsqc.Text.Trim()));
                list.Add(new SqlPara("gsjc ", gsjc.Text.Trim()));
                list.Add(new SqlPara("accountId ", accountId.Text.Trim()));
                list.Add(new SqlPara("state ", checkEdit1.Checked==true?1:0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_InsuranceAccount", list);
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        ///获取账户编号 
        /// </summary>
        private void getAccountId()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_InsuranceAccount_AccountID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            accountId.Text = ds.Tables[0].Rows[0]["AccountID"].ToString();
        }


        private void checkEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (checkEdit1.Checked==true)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }

        }

        /// <summary>
        /// 获取公司名称
        /// </summary>
        /// <param name="companyId"></param>
        private void GetCompanyName(string companyId)
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_Get_ComapnyName", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            SetCompanyName(gsqc, ds);

        }

        /// <summary>
        /// 加载公司名称
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="ds"></param>
        /// <param name="isall"></param>
        public static void SetCompanyName(ComboBoxEdit cb, DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return;
            try
            {

                cb.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cb.Properties.Items.Add(ds.Tables[0].Rows[i]["gsqc"]);
                }
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}
