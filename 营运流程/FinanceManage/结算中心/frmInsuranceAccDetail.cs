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
    public partial class frmInsuranceAccDetail : BaseForm
    {
        public frmInsuranceAccDetail()
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
            GetCompanyName(CommonClass.UserInfo.companyid);
        }



        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("companyName", Company.Text.Trim() == "全部" ? "%%" : Company.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_InsuranceAccountDetail", list);
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

        

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (Web.Properties.Items.Count != 0 || Web.Text!="全部")
                //{
                //    Web.Properties.Items.Clear();
                //    Web.Text = "全部";
                //}
                //string causename = Company.Text.Trim();
                //string areaname = Area.Text.Trim();
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("causename", causename == "全部" ? "%%" : causename));
                //list.Add(new SqlPara("areaname", areaname == "全部" ? "%%" : areaname));
                //DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "Get_basSettleCenterAccount",list));
                //if (ds == null || ds.Tables.Count == 0) return; 
                //for (int i = 0; i <  ds.Tables[0].Rows.Count; i++)
                //{
                //    Web.Properties.Items.Add(ds.Tables[0].Rows[i]["AccountName"]);
                //}
                //Web.Properties.Items.Add("全部");  //maohui20180503
             }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
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
            SetCompanyName(Company, ds);

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
                if (CommonClass.UserInfo.companyid == "101")
                {
                    cb.Properties.Items.Add("全部");
                }
                cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

    }
}
