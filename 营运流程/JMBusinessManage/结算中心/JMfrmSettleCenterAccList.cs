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
    public partial class JMfrmSettleCenterAccList : BaseForm
    {
        public JMfrmSettleCenterAccList()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例


            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            getAccontWeb();
            AccountName.SelectedIndex = AccountName.Properties.Items.Count - 1;
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", AccountName.Text.Trim() == "全部" ? "%%" : AccountName.Text.Trim()));
                list.Add(new SqlPara("IsEnable", IsEnable.Text.Trim() == "全部" ? "%%" : IsEnable.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSETTLECENTERACC", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
                myGridView1_FocusedRowChanged(sender, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)//审核
        {
            
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //frmSetCreditLimit frm = new frmSetCreditLimit();
            //frm.sId = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //frm.sAccountName = myGridView1.GetRowCellValue(rowhandle, "AccountName").ToString();
            //frm.sAccountNO = myGridView1.GetRowCellValue(rowhandle, "AccountNO").ToString();
            //frm.sAccountType = myGridView1.GetRowCellValue(rowhandle, "AccountType").ToString();
            //frm.ShowDialog();
            //cbRetrieve_Click(sender, null);
            
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            GridOper.ExportToExcel(myGridView1); 
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            //frmSetAdjustLimit frm = new frmSetAdjustLimit();
            //frm.sId = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //frm.sUsingCredit = myGridView1.GetRowCellValue(rowhandle, "UsingCredit").ToString();
            //frm.sLeftCredit = myGridView1.GetRowCellValue(rowhandle, "LeftCredit").ToString();
            //frm.sCreditLimit = myGridView1.GetRowCellValue(rowhandle, "CreditLimit").ToString();
            //frm.ShowDialog();
            //cbRetrieve_Click(sender, null);
        }
        private void getAccontWeb()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSETTLECENTERACC_ACCOUNTNAME", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    AccountName.Properties.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                        AccountName.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"].ToString().Trim());
                }
                AccountName.Properties.Items.Add("全部");

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            JMfrmSetAccount frm = new JMfrmSetAccount();
            //frm.sId = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //frm.sLoanWarnValue = myGridView1.GetRowCellValue(rowhandle, "LoanWarnValue").ToString();
            //frm.sNegwarnValue = myGridView1.GetRowCellValue(rowhandle, "NegwarnValue").ToString();
            //frm.sAccountReserved = myGridView1.GetRowCellValue(rowhandle, "AccountReserved").ToString();
            frm.dr = myGridView1.GetDataRow(rowhandle);
            frm.ShowDialog();
            cbRetrieve_Click(sender, null);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "ID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                if (myGridView1.GetRowCellValue(rowhandle, "IsEnable").ToString() == "启用")
                {
                    if (MsgBox.ShowYesNo("是否冻结该账户？\r\r请确认！") != DialogResult.Yes)
                    {
                        return;
                    }
                    list.Add(new SqlPara("IsEnable", "冻结"));
                }
                else
                {
                    if (MsgBox.ShowYesNo("是否启用该账户？\r\r请确认！") != DialogResult.Yes)
                    {
                        return;
                    }
                    list.Add(new SqlPara("IsEnable", "启用"));
                }
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BASSETTLECENTERACC_ENABLED", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, null);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        private void myGridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (myGridView1.GetRowCellValue(rowhandle, "IsEnable").ToString() == "启用")
            {
                barButtonItem8.ImageIndex = 6;
                barButtonItem8.Caption = "冻结";
            }
            else
            {
                barButtonItem8.ImageIndex = 8;
                barButtonItem8.Caption = "启用";
            }

        }
    }
}
