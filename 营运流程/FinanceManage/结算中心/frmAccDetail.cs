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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmAccDetail : BaseForm
    {
        public frmAccDetail()
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
            //setweb();

            CommonClass.SetCause(Cause, true);
            Cause.EditValue = CommonClass.UserInfo.CauseName;
            Area.EditValue = CommonClass.UserInfo.AreaName;
            Web.EditValue = CommonClass.UserInfo.WebName;

            //不是总部的财务，事业部固定
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                Cause.Enabled = false;
            }
        }

        //private void setweb()
        //{
        //    for (int i = 0; i < CommonClass.dsCause.Tables[0].Rows.Count; i++)
        //    {
        //        SettlementAcc.Properties.Items.Add(CommonClass.dsCause.Tables[0].Rows[i]["CauseName"]);
        //    }
        //    if (CommonClass.UserInfo.SiteName != "总部")
        //    {
        //        SettlementAcc.Enabled = false;
        //        SettlementAcc.Text = CommonClass.UserInfo.WebName;
        //    }
        //    else
        //    {
        //        SettlementAcc.Properties.Items.Add("全部");
        //        SettlementAcc.Text = "全部";
        //    }
        //}


        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("Cause", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("Area", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));

                //list.Add(new SqlPara("SettlementAcc", SettlementAcc.Text.Trim() == "全部" ? "%%" : SettlementAcc.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ACCDETAIL2", list);
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
            //try
            //{
            //    List<SqlPara> list = new List<SqlPara>();

            //    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSETTLECENTERACC_ACCOUNTNAME", list);
            //    DataSet ds = SqlHelper.GetDataSet(sps);
            //    if (ds != null && ds.Tables.Count != 0)
            //    {
            //        PaySite.Properties.Items.Clear();
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            //            PaySite.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"].ToString().Trim());
            //    }
            //    PaySite.Properties.Items.Add("全部");

            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }

        private void SettlementAcc_EditValueChanged(object sender, EventArgs e)
        {
            //PaySite.Properties.Items.Clear();
            //DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("BelongCause='" + SettlementAcc.Text.Trim() + "'");
            //if (dr.Length > 0)
            //{
            //    for (int i = 0; i < dr.Length; i++)
            //    {
            //        PaySite.Properties.Items.Add(dr[i]["WebName"]);
            //    }
            //    PaySite.Properties.Items.Add("全部");
            //    PaySite.Text = "全部";
            //}
            //else
            //{
            //    PaySite.Properties.Items.Add(SettlementAcc.Text.Trim());
            //    PaySite.Properties.Items.Add("全部");
            //    PaySite.Text = "全部";
            //}
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text,true);
            //CommonClass.SetCauseWeb(Web, Cause.Text, Area.Text, true);

        }
        

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Web.Properties.Items.Count != 0 || Web.Text!="全部")
                {
                    Web.Properties.Items.Clear();
                    Web.Text = "全部";
                }
                string causename = Cause.Text.Trim();
                string areaname = Area.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("causename", causename == "全部" ? "%%" : causename));
                list.Add(new SqlPara("areaname", areaname == "全部" ? "%%" : areaname));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "Get_basSettleCenterAccount",list));
                if (ds == null || ds.Tables.Count == 0) return; 
                for (int i = 0; i <  ds.Tables[0].Rows.Count; i++)
                {
                    Web.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"]);
                }
                Web.Properties.Items.Add("全部");  //maohui20180503
             }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        
    }
}
