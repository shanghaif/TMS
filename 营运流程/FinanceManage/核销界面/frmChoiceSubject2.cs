using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmChoiceSubject2 : BaseForm
    {
        private DataSet dsxm = new DataSet();
        public string SubjectOne = "", SubjectTwo = "", SubjectThree = "",Verifydirection = "", Summary = "",Remarks = "";
        int state = 0;//1：点击确定按钮关闭的  0：点击右上角的×关闭的

        public string xm = "", inOutType = "";
        public string OilCardNo = "", Company="";
        public decimal OilCardFee = 0, currVerifyFee=0,CardFee=0;

        public frmChoiceSubject2()
        {
            InitializeComponent();
        }

        private void getparent()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSUBJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                dsxm.Clear();
                dsxm = ds;
                edSubjectOne.Properties.Items.Clear();
                if (dsxm.Tables.Count > 0)
                {
                    foreach (DataRow row in dsxm.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(row["SubLevel"]) == 0)
                        {
                            edSubjectOne.Properties.Items.Add(row["SubjectName"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void getXm()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("xm", xm));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_HXXM", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count <= 0) return;
                edSubjectOne.Text = ds.Tables[0].Rows[0]["km1"].ToString();
                edSubjectTwo.Text = ds.Tables[0].Rows[0]["km2"].ToString();
                edSubjectThree.Text = ds.Tables[0].Rows[0]["km3"].ToString();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private int getkmid(string kmmc)
        {
            foreach (DataRow row in dsxm.Tables[0].Rows)
            {
                if (row["SubjectName"].ToString().Trim() == kmmc.Trim())
                {
                    return Convert.ToInt32(row["SubjectID"]);
                }
            }
            return -1;
        }

        private void getchild(int parentid, DevExpress.XtraEditors.ComboBoxEdit cbe)
        {
            cbe.Properties.Items.Clear();
            cbe.Text = "";
            foreach (DataRow row in dsxm.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["ParentId"]) == parentid)
                {
                    cbe.Properties.Items.Add(row["SubjectName"].ToString());
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SubjectOne = edSubjectOne.Text.Trim();
            SubjectTwo = edSubjectTwo.Text.Trim();
            SubjectThree = edSubjectThree.Text.Trim();
            Verifydirection = VerifyDirections.Text.Trim();//hj 20180104
            Summary = txtSummary.Text.Trim();
            Remarks = txtRemarks.Text.Trim();//jilei 20180831
            if (edSubjectOne.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择一级科目");
                edSubjectOne.Focus();
                return;
            }
            if (edSubjectTwo.Text.Trim() == "")
            {
                MsgBox.ShowOK("请选择二级科目");
                edSubjectTwo.Focus();
                return;
            }
            //if (edSubjectThree.Text.Trim() == "")
            //{
            //    MsgBox.ShowOK("请选择三级科目");
            //    edSubjectThree.Focus();
            //    return;
            //}
            //hj20180710
            if (Common.CommonClass.UserInfo.companyid == "155" || Common.CommonClass.UserInfo.companyid == "108")
            {
                if (VerifyDirections.Text.Trim() == "油卡")
                {
                    if (OilCardFee != currVerifyFee)
                    {
                        MsgBox.ShowError("您输入的油卡金额要和本次核销金额相等！");
                        return;
                    }
                }
                if (xm != "大车费")
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("OilCardNo", OilCardNo));
                    list.Add(new SqlPara("OilCardFee", OilCardFee));
                    list.Add(new SqlPara("inOutType", inOutType));
                    list.Add(new SqlPara("Company", Company));
                    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATA_OilCard", list)) == 0)
                    {
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        state = 1;
                        this.Close();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    state = 1;
                    this.Close();
                }
               
            }
            else
            {

                this.DialogResult = DialogResult.OK;
                state = 1;
                this.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmChoiceSubject2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (state == 0)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void edSubjectOne_SelectedIndexChanged(object sender, EventArgs e)
        {
            edSubjectTwo.Text = "";
            edSubjectThree.Text = "";
            getchild(getkmid(edSubjectOne.Text.Trim()), edSubjectTwo);
        }

        private void edSubjectTwo_SelectedIndexChanged(object sender, EventArgs e)
        {
            edSubjectThree.Text = "";
            getchild(getkmid(edSubjectTwo.Text.ToString()), edSubjectThree);
        }

        private void frmChoiceSubject2_Load(object sender, EventArgs e)
        {
            edSubjectOne.Text = SubjectOne;
            edSubjectTwo.Text = SubjectTwo;
            edSubjectThree.Text = SubjectThree;
            VerifyDirections.Text = Verifydirection;
            getparent();
            getXm();

            //hj 20180104
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSPARAMSETTING_1");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return;
            }
            string[] str = ds.Tables[0].Rows[0]["ParamValue"].ToString().Split(',');
            if (str.Length > 0)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    VerifyDirections.Properties.Items.Add(str[i]);
                }
            }

        }
        //核销方向
        private void VerifyDirections_EditValueChanged(object sender, EventArgs e)
        {
            if (VerifyDirections.Text.Trim() == "油卡" && (Common.CommonClass.UserInfo.companyid == "155" || Common.CommonClass.UserInfo.companyid == "108"))
            {
                frmOilCardVerification frm = new frmOilCardVerification();
                frm.CardNo = OilCardNo;
                frm.verifyType = xm;
                frm.CardFee = CardFee;
                frm.GetForm(this, 1);//后一个参数是判断通过哪个界面传进去的。0-frmChoiceSubject or 1-frmChoiceSubject2
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }



    }
}
