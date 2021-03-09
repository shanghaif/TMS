using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmOilCardVerification : BaseForm
    {
        public frmOilCardVerification()
        {
            InitializeComponent();
        }
        public string str = null;
        public frmChoiceSubject form = null;
        public frmChoiceSubject2 form2 = null;
        public frmChoiceSubjectTX form3 = null;
        public int hasRemarks = 0;
        public string verifyType = "",  CardNo="";
        public decimal CardFee = 0;

        public void GetForm(frmChoiceSubject theform)
        {
            form = theform;
        }
        public void GetForm(frmChoiceSubjectTX theform)
        {
            form3 = theform;
        }
        public void GetForm(frmChoiceSubject2 theform,int hasRemarks)
        {
            form2 = theform;
            this.hasRemarks = hasRemarks;
        }

       // public string inOutType = "";
        private void frmOilCardChoose_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
          
            try
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OilCard"));
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
            if (verifyType == "大车费" && (Common.CommonClass.UserInfo.companyid == "155" || Common.CommonClass.UserInfo.companyid == "108"))
            {
                try
                {
                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_OilCard_CardNo", new List<SqlPara>() { new SqlPara("CardNo", CardNo) }));
                    if (ds.Tables.Count == 0 || ds == null || ds.Tables[0].Rows.Count == 0) return;
                    OilCardNo.Text = ds.Tables[0].Rows[0]["OilCardNo"].ToString();
                    OilCardFee.Text = CardFee.ToString();
                    Company.Text = ds.Tables[0].Rows[0]["Company"].ToString();
                    OilCardPsW.Text = ds.Tables[0].Rows[0]["OilCardPsW"].ToString();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
               
            }

        }


        private void OilCardNo_Enter(object sender, EventArgs e)
        {
            //myGridControl1.Left = groupControl2.Left + OilCardNo.Left;
            //myGridControl1.Top = groupControl2.Top + OilCardNo.Top + OilCardNo.Height + 2;
            myGridControl1.Left =  OilCardNo.Left;
            myGridControl1.Top =  OilCardNo.Top + OilCardNo.Height + 2;
            myGridControl1.Visible = true;
        }

        private void OilCardNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                myGridControl1.Focus();
        }

        private void OilCardNo_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = myGridControl1.Focused;
        }

        private void SetOilCard()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(rowhandle);
            if (dr == null) return;

            OilCardNo.EditValue = dr["OilCardNo"];
            OilCardFee.EditValue = dr["Balance"];
            Company.EditValue = dr["Company"];
            myGridControl1.Visible = false;
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetOilCard();
        }

        private void myGridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetOilCard();
            }
        }

        private void myGridControl1_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = OilCardNo.Focused;
        }

        private void cbsave_Click(object sender, EventArgs e)
        {
            if (OilCardNo.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写油卡编号!");
                return;
            }
            if (OilCardFee.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写油卡金额!");
                return;
            }
            if (Company.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写所属公司!");
                return;
            }
            //decimal OilCardFee = OilCardFee.text;
            if (hasRemarks == 1)
            {
                //decimal OilCardFee = OilCardFee.text;
                form2.OilCardNo = OilCardNo.Text.Trim();
                form2.OilCardFee = ConvertType.ToDecimal(OilCardFee.Text.Trim());
                form2.Company = Company.Text.Trim();
                this.Close();
            }
            else
            {
                form.OilCardNo = OilCardNo.Text.Trim();
                form.OilCardFee = ConvertType.ToDecimal(OilCardFee.Text.Trim());
                form.Company = Company.Text.Trim();
                this.Close();
            }

        }

        private void cbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }




        
    }
}
