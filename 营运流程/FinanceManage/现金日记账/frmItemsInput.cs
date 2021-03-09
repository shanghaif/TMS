using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmItemsInput : ZQTMS.Tool.BaseForm
    {
        DataSet dsxm;
        string VerifyOffAccountID = "";
        string optState = "";
        public string OilCardNo = "", Company = "", OilCardPsw="";
        public string type = "";
        public string ComeFrom;
        public int isadd = 0;
        public frmItemsInput(string VerifyOffAccountID)
        {
            InitializeComponent();
            this.VerifyOffAccountID = VerifyOffAccountID;
        }

        private void frmItemsInput_Load(object sender, EventArgs e)
        {
          
            CommonClass.FormSet(this);
            CommonClass.GetServerDate();
            edbilldate.DateTime = CommonClass.gcdate;
            WebName.Text = CommonClass.UserInfo.WebName;
            edcreateby.Text = CommonClass.UserInfo.UserName;
            VoucherNo.Text = (edinouttype.Text == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss");
            getparent();
            //hj 20171229
            //if (CommonClass.Arg.VerifyDirection != null)
            //{
            //    string[] str = CommonClass.Arg.VerifyDirection.Split(',');
               
            //    if (str.Length > 0)
            //    {
            //        for (int i = 0; i < str.Length; i++)
            //        {
            //            edbilltype.Properties.Items.Add(str[i]);
            //        }
            //    }
            //}
            //ywc 20190403
            if (type == "新增")
            {
                label2.Visible = false;
                VoucherNo.Visible = false;
                isadd = 1;

            }
            //hj 20180102
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
                    edbilltype.Properties.Items.Add(str[i]);
                }
            }


            if (VerifyOffAccountID != "")
            {
                DataTable dt = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_BYID", new List<SqlPara> { new SqlPara("VerifyOffAccountID", VerifyOffAccountID) }));
                if (dt == null || dt.Rows.Count == 0) return;

                DataRow dr = dt.Rows[0];
                WebName.EditValue = dr["WebName"];
                VoucherNo.EditValue = dr["VoucherNo"];
                edinouttype.EditValue = dr["InoutType"];
                //edbilldate.EditValue = dr["OptTime"];

                //
                if(string.IsNullOrEmpty(dr["credentialsTime"].ToString()))
                {
                  edbilldate.EditValue=CommonClass.ServerDate;
                }
                else
                {
                     edbilldate.EditValue = dr["credentialsTime"];//hj 20180309
                }
                edbilltype.EditValue = dr["BillType"];
                edSubjectOne.EditValue = dr["OneSubject"];
                edSubjectTwo.EditValue = dr["TwoSubject"];
                edSubjectThree.EditValue = dr["ThreeSubject"];
                edTheBillType.EditValue = dr["TheBillType"];
                edaccount.EditValue = dr["Money"];
                txtSummary.EditValue = dr["Summary"];
                edsjno.EditValue = dr["sjno"];
                edfpno.EditValue = dr["fpno"];
                edzpno.EditValue = dr["zpno"];
                edsgpz.EditValue = dr["sgpz"];
                edcreateby.EditValue = dr["OptMan"];
                optState=dr["optState"].ToString();
                hm.EditValue = dr["hm"];
                zh.EditValue = dr["zh"];
                khh.EditValue = dr["khh"];
            }
            //tc201808-28
            if (CommonClass.UserInfo.companyid == "124" || (CommonClass.UserInfo.companyid == "485" && ComeFrom == "手工录入") || (CommonClass.UserInfo.companyid == "485" && isadd==1))
            {
                edbilldate.Properties.ReadOnly = false;
            }
        }

        private void getparent()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSUBJECT", list);
                dsxm = SqlHelper.GetDataSet(sps);

                if (dsxm == null || dsxm.Tables.Count == 0) return;
                edSubjectOne.Properties.Items.Clear();

                foreach (DataRow row in dsxm.Tables[0].Rows)
                {
                    if (ConvertType.ToInt32(row["SubLevel"]) == 0)
                    {
                        edSubjectOne.Properties.Items.Add(row["SubjectName"].ToString());
                    }
                }
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

        private void cbsave_Click(object sender, EventArgs e)
        {
            string subjectOne = edSubjectOne.Text.Trim();
            if (subjectOne == "")
            {
                MsgBox.ShowOK("请选择一级科目!");
                edSubjectOne.Focus();
                return;
            }
            string subjectTwo = edSubjectTwo.Text.Trim();
            //if (subjectTwo == "")
            //{
            //    MsgBox.ShowOK("请选择二级科目!");
            //    edSubjectTwo.Focus();
            //    return;
            //}
            string subjectThree = edSubjectThree.Text.Trim();
            //if (subjectThree == "")
            //{
            //    MsgBox.ShowOK("请选择三级科目!");
            //    edSubjectThree.Focus();
            //    return;
            //}
            decimal account = ConvertType.ToDecimal(edaccount.Text);
            if (account <= 0)
            {
                MsgBox.ShowOK("请填写发生金额!");
                edaccount.Focus();
                return;
            }
            string voucherNo = "";
            Random r1 = new Random();
            int a1 = r1.Next(10, 100);
            if (VerifyOffAccountID == "") voucherNo = (edinouttype.Text == "支出" ? "O" : "I") + CommonClass.gcdate.ToString("yyyyMMddHHmmss")+a1;
            else voucherNo = VoucherNo.Text;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VerifyOffAccountID", VerifyOffAccountID));
            list.Add(new SqlPara("VoucherNo", voucherNo));
            list.Add(new SqlPara("InoutType", edinouttype.Text));
            list.Add(new SqlPara("OptTime", CommonClass.gcdate));
            list.Add(new SqlPara("BillType", edbilltype.Text));
            list.Add(new SqlPara("SubjectOne", subjectOne));
            list.Add(new SqlPara("SubjectTwo", subjectTwo));
            list.Add(new SqlPara("SubjectThree", subjectThree));
            list.Add(new SqlPara("TheBillType", edTheBillType.Text));
            list.Add(new SqlPara("Money", account));
            list.Add(new SqlPara("Summary", txtSummary.Text.Trim()));
            list.Add(new SqlPara("sjno", edsjno.Text.Trim()));
            list.Add(new SqlPara("fpno", edfpno.Text.Trim()));
            list.Add(new SqlPara("zpno", edzpno.Text.Trim()));
            list.Add(new SqlPara("sgpz", edsgpz.Text.Trim()));
            list.Add(new SqlPara("optState", optState));//hj 审核状态20180106
            list.Add(new SqlPara("OilCardNo", OilCardNo));//hj油卡编号20180709
            list.Add(new SqlPara("Company", Company));//hj油卡所属公司
            list.Add(new SqlPara("OilCardPsw", OilCardPsw));
            list.Add(new SqlPara("credentialsTime", edbilldate.DateTime)); //maohui20180904核销时间（可修改）
            list.Add(new SqlPara("hm",hm.Text.Trim()));
            list.Add(new SqlPara("zh", zh.Text.Trim()));
            list.Add(new SqlPara("khh", khh.Text.Trim())); //ywc20190329 户名开户行账号
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BILLACCOUNT_1", list)) == 0) return;

            //清空
            VoucherNo.Text = (edinouttype.Text == "支出" ? "O" : "I") + CommonClass.gcdate.ToString("yyyyMMddHHmmss");
            edbilldate.DateTime = CommonClass.gcdate;
            edSubjectOne.Text = edSubjectTwo.Text = edSubjectThree.Text = edaccount.Text = txtSummary.Text = edsjno.Text = edfpno.Text = edzpno.Text = edsgpz.Text = "";
            VerifyOffAccountID = "";

            MsgBox.ShowOK("保存成功");
        }

        private void cbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edbilltype_EditValueChanged(object sender, EventArgs e)
        {
            if (edbilltype.Text.Trim() == "油卡" && (Common.CommonClass.UserInfo.companyid == "155" || Common.CommonClass.UserInfo.companyid == "108"))
            {
                frmOilCardChoose frm = new frmOilCardChoose();
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.GetForm(this);         
                frm.ShowDialog();
            }
        }
    }
}