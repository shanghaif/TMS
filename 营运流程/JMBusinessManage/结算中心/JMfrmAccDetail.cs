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
    public partial class JMfrmAccDetail : BaseForm
    {
        public JMfrmAccDetail()
        {
            InitializeComponent();
        }

        private void frmChargeApply_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            //getAccontWeb();
            //PaySite.SelectedIndex = PaySite.Properties.Items.Count - 1;
            setweb();
        }
        private void setweb()
        {
            for (int i = 0; i < CommonClass.dsCause.Tables[0].Rows.Count; i++)
            {
                SettlementAcc.Properties.Items.Add(CommonClass.dsCause.Tables[0].Rows[i]["CauseName"]);
            }
            for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
            {
                SettlementAcc.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
            }
            if (CommonClass.UserInfo.SiteName != "总部")
            {
                SettlementAcc.Enabled = false;
                SettlementAcc.Text = CommonClass.UserInfo.WebName;
            }
            else
            {
                SettlementAcc.Properties.Items.Add("全部");
                SettlementAcc.Text = "全部";
            }
        }


        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("SettlementAcc", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));
                list.Add(new SqlPara("PaySite", PaySite.Text.Trim() == "全部" ? "%%" : PaySite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCDETAIL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
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
                    PaySite.Properties.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                        PaySite.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"].ToString().Trim());
                }
                PaySite.Properties.Items.Add("全部");

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SettlementAcc_EditValueChanged(object sender, EventArgs e)
        {
            PaySite.Properties.Items.Clear();
            DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("BelongCause='" + SettlementAcc.Text.Trim() + "'");
            if (dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    PaySite.Properties.Items.Add(dr[i]["WebName"]);
                }
                PaySite.Properties.Items.Add("全部");
                PaySite.Text = "全部";
            }
            else
            {
                PaySite.Properties.Items.Add(SettlementAcc.Text.Trim());
                PaySite.Properties.Items.Add("全部");
                PaySite.Text = "全部";
            }
        }

        
    }
}
