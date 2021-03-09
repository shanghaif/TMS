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
    public partial class frmChoiceSubjectTX : BaseForm
    {
        private DataSet dsxm = new DataSet();
        public string SubjectOne = "", SubjectTwo = "", SubjectThree = "", Verifydirection = "", Summary = "";
        int state = 0;//1：点击确定按钮关闭的  0：点击右上角的×关闭的
        public int Num = 0;
        public decimal Money = 0;

        public string xm = "", inOutType = "";
        public string OilCardNo = "", Company = "";
        public decimal OilCardFee = 0, currVerifyFee = 0, CardFee = 0;

        public string hm1 = "", zh1 = "", khh1 = "", szs1 = "", szshi1 = "", zzlx1 = "";

        public frmChoiceSubjectTX()
        {
            InitializeComponent();
        }

        private void frmChoiceSubjectTX_Load(object sender, EventArgs e)
        {
            CommonClass.GetGridViewColumns(myGridView1 );
            GridOper.SetGridViewProperty(myGridView1 );
            edSubjectOne.Text = SubjectOne;
            edSubjectTwo.Text = SubjectTwo;
            edSubjectThree.Text = SubjectThree;
            VerifyDirections.Text = Verifydirection;
            getparent();
            getXm();
            gethkzl();

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
            label4.Text = Num.ToString();
            label6.Text = Money.ToString();

            if (xm == "提付" || xm == "现付" || xm == "欠付" || xm == "异动"||xm=="短欠"||xm=="回单付"||xm=="货到前付"||xm=="月结"||xm=="提付异动"||xm=="非提付异动")
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                label8.Visible = false;
                hm.Visible = false;
                label9.Visible = false;
                zh.Visible = false;
                label10.Visible = false;
                khh.Visible = false;
                label12.Visible = false;
                szs.Visible = false;
                label14.Visible = false;
                szshi.Visible = false;
                label16.Visible = false;
                zzlx.Visible = false;
            }

        }

        private void gethkzl()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_GET_hkzl", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SubjectOne = edSubjectOne.Text.Trim();
            SubjectTwo = edSubjectTwo.Text.Trim();
            SubjectThree = edSubjectThree.Text.Trim();
            Verifydirection = VerifyDirections.Text.Trim();//hj 20180104
            Summary = txtSummary.Text.Trim();
            hm1 = hm.Text.Trim();
            zh1 = zh.Text.Trim().Replace(" ", "");
            khh1 = khh.Text.Trim();
            szs1 = szs.Text.Trim();
            szshi1 = szshi.Text.Trim();
            zzlx1 = zzlx.Text.Trim();

            //if (zzlx.Text.Trim() == "")
            //{
            //    MsgBox.ShowOK("请选择转账类型");
            //    return;
            //}
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
            //if (VerifyDirections.Text != "")
            //{
            //    if (VerifyDirections.Text != "微信" && VerifyDirections.Text != "支付宝" && VerifyDirections.Text != "油卡" && VerifyDirections.Text != "运满满" && VerifyDirections.Text != "现金")
            //    {
            //        if (hm.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入户名！");
            //            return;
            //        }
            //        if (zh.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入账号！");
            //            return;
            //        }
            //        if (khh.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入开户行！");
            //            return;
            //        }
            //        if (szs.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入所在省！");
            //            return;
            //        }
            //        if (szshi.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入所在市！");
            //            return;
            //        }
            //        if (zzlx.Text == "")
            //        {
            //            MsgBox.ShowOK("请输入转账类型！");
            //            return;
            //        }
            //    }
            //}

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

        private void edSubjectTwo_SelectedIndexChanged(object sender, EventArgs e)
        {
            edSubjectThree.Text = "";
            getchild(getkmid(edSubjectTwo.Text.ToString()), edSubjectThree);
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

        private void edSubjectOne_SelectedIndexChanged(object sender, EventArgs e)
        {
            edSubjectTwo.Text = "";
            edSubjectThree.Text = "";
            getchild(getkmid(edSubjectOne.Text.Trim()), edSubjectTwo);
        }

        private void VerifyDirections_EditValueChanged(object sender, EventArgs e)
        {
            if (VerifyDirections.Text.Trim() == "油卡" && (Common.CommonClass.UserInfo.companyid == "155" || Common.CommonClass.UserInfo.companyid == "108"))
            {
                frmOilCardVerification frm = new frmOilCardVerification();
                frm.CardNo = OilCardNo;
                frm.verifyType = xm;
                frm.CardFee = CardFee;
                frm.GetForm(this);
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.ShowDialog();
            }
        }

        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl1.Visible = true; }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
            }
        }

        private void textEdit1_KeyUP(object sender, KeyEventArgs e)
        {
            string str = zh.Text.Trim();
            str = str.Replace(" ", "");
            this.zh.Text = System.Text.RegularExpressions.Regex.Replace(str, @"(\w{4})", "$1 ").Trim(' ');
            this.zh.Select(this.zh.Text.Length, 0);
        }

        private void textEdit1_Leave(object sender, EventArgs e)
        {
            if (!myGridControl1.Focused)
            {
                myGridControl1.Visible = false;
            }
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            hm.Text = myGridView1.GetRowCellValue(rows,"hm").ToString();
            zh.Text = myGridView1.GetRowCellValue(rows, "zh").ToString();
            khh.Text = myGridView1.GetRowCellValue(rows, "khh").ToString();
            szs.Text = myGridView1.GetRowCellValue(rows, "szs").ToString();
            szshi.Text = myGridView1.GetRowCellValue(rows, "szshi").ToString();
            myGridControl1.Visible = false;
        }

        private void hm_Enter(object sender, EventArgs e)
        {
            myGridControl1.Left = hm.Left ;
            myGridControl1.Top = hm.Top + hm.Height;
            myGridControl1.Visible = true;
            myGridControl1.BringToFront();
        }


    }
}