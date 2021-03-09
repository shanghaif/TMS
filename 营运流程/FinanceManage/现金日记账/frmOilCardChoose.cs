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
    public partial class frmOilCardChoose : BaseForm
    {
        public frmOilCardChoose()
        {
            InitializeComponent();
        }
        public string str = null;
        public frmItemsInput form = null;

        public void GetForm(frmItemsInput theform)
        {
            form = theform;
        }


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
            OilCardPsw.EditValue = dr["OilCardPsw"];
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
            //if (OilCardNo.Text.Trim() == "")
            //{
            //    MsgBox.ShowError("请填写油卡编号！");
            //    return;
            //}
            if (ConvertType.ToDecimal(OilCardFee.Text.Trim()) == 0)
            {
                MsgBox.ShowError("请输入油卡金额！");
                return;
            }
             form.edaccount.Text = OilCardFee.Text;
             form.OilCardNo = OilCardNo.Text.Trim().ToString();
             form.OilCardPsw = OilCardPsw.Text.Trim().ToString();
             form.Company = Company.Text.Trim();
            this.Close();

        }

        private void cbclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
