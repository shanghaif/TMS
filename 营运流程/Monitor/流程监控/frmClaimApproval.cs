using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using System.IO;
using System.Net;
using ZQTMS.SqlDAL;
//using ZQTMS.UI.流程监控;

namespace ZQTMS.UI
{
    public partial class frmClaimApproval : BaseForm
    {
        public Boolean isClose = false;

        public frmClaimApproval()
        {
            InitializeComponent();
        }

        private void frmClaimApproval_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView1, false);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName,CauseName.Text, true);
            CommonClass.SetWeb(BegWeb, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            BegWeb.Text = CommonClass.UserInfo.WebName;

            GridOper.CreateStyleFormatCondition(myGridView1, "LinkMan", DevExpress.XtraGrid.FormatConditionEnum.Expression, "",
                Color.Red, true, "LinkMan = '" + CommonClass.UserInfo.UserName + "' or LinkMan = '" + CommonClass.UserInfo.WebName + "'");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // QSP_GET_ClaimInfo
            getdata();
        }

        private void getdata() 
        {
            try
            {
                //List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("bdate", bdate.DateTime));
                //list.Add(new SqlPara("edate", edate.DateTime));
                //list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                //list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                //list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));

                //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ClaimInfo", list);
                //DataSet ds = SqlHelper.GetDataSet(sps);

                string CauseName = this.CauseName.Text.Trim() == "全部" ? "%%" : this.CauseName.Text.Trim();
                string AreaName = this.AreaName.Text.Trim() == "全部" ? "%%" : this.AreaName.Text.Trim();
                string BegWeb = this.BegWeb.Text.Trim() == "全部" ? "%%" : this.BegWeb.Text.Trim();
                //调用ZQTMS理赔审批信息
                DataSet ds = CommonSyn.GetZQTMSClaimMessage("", bdate.DateTime.ToString(), edate.DateTime.ToString(), CauseName, AreaName, BegWeb, "QSP_GET_ClaimInfo");

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string billno = myGridView1.GetFocusedRowCellValue("BillNO").ToString();
            frmClaimDetail frm = new frmClaimDetail();
            frm.billNoStr = billno;
            frm.Owner = this;
            frm.ShowDialog();
            if (isClose)
            {
                getdata();
            }
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "理赔审批");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            if (MsgBox.ShowYesNo("确定要强制审批？") != DialogResult.Yes) return;
            string billno = myGridView1.GetFocusedRowCellValue("BillNO").ToString();

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billno));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USD_Claim_apply_QZ", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    getdata();
                }
                else
                {
                    MsgBox.ShowError("操作失败！");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        //修改审批人
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
           
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择数据");
                return;
            }
            else {

                //string ID = myGridView1.GetRowCellValue(rowhandle, "BillNO").ToString();
                //string LinkMan = myGridView1.GetRowCellValue(rowhandle, "LinkMan").ToString();
             
                //frmApprovalEdit frm = new frmApprovalEdit();
                //frm.id = ID;
                //frm.LinkMan = LinkMan;
                //frm.ShowDialog();
                //getdata();
            }

               
        }

      
    }
}
