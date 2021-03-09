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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmExpenseAudit : BaseForm
    {
        public frmExpenseAudit()
        {
            InitializeComponent();
        }
        DataSet dsshipper = new DataSet();//汇款客户资料
        string auditIds = "";
        GridColumn gcIsseleckedMode;
        private void frmExpenseAudit_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("费用审核");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1,myGridView2);
            GridOper.SetGridViewProperty(myGridView1,myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetWeb(BegWeb,true);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);           
           // CommonClass.SetUser(BillMan, BegWeb.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;     
          
            BegWeb.Text = CommonClass.UserInfo.WebName;
           // BillMan.Text = CommonClass.UserInfo.UserName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView1, "AuditMan", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Yellow);
            GridOper.CreateStyleFormatCondition(myGridView1, "PushMan", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Orange);

            GridOper.CreateStyleFormatCondition(myGridView1, "appby", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.PaleGreen);
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
            backgroundWorker1.RunWorkerAsync();//汇款客户资料


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

        private void BegWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetUser(BillMan, BegWeb.Text);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("Batch", txtBatch.Text.Trim() == "" ? "%%" : txtBatch.Text.Trim()));
                //list.Add(new SqlPara("RegistMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseAudit", list);
                
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
           
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExpenseReimbursementAdd add = new ExpenseReimbursementAdd();
            //add.dsshipper = dsshipper;
            add.ShowDialog();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;
            string aduitid = myGridView1.GetRowCellValue(rows, "ID").ToString();
            string aduitMan = myGridView1.GetRowCellValue(rows, "AuditMan").ToString();
            if (aduitMan != "")
            {
                MsgBox.ShowOK("此单已经核销,不能修改！");
                return;
            }
            ExpenseBankInfo edit = new ExpenseBankInfo();
            edit.aduitId = aduitid;
            edit.dsshipper = dsshipper;
            edit.pageType = "audit";
            edit.ShowDialog();


        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int rows = myGridView1.FocusedRowHandle;
            //if (rows < 0) return;
            myGridView1.PostEditor();
            string Ids = "";
            string confirmMan = "";
            for (int i = 0; i < myGridView1.RowCount;i++ )
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    Ids = Ids + myGridView1.GetRowCellValue(i, "ID").ToString() + "@";
                    confirmMan = myGridView1.GetRowCellValue(i, "ConfirmMan").ToString();
                    if (confirmMan != "")
                    {
                        MsgBox.ShowOK("存在已确认的单，不能删除！");
                        return;
                    }
                }
                
            }
            //string id = myGridView1.GetRowCellValue(rows, "ID").ToString();
            if (Ids == "")
            {
                MsgBox.ShowOK("请选择要删除的单！");
                return;
            }
           if (MsgBox.ShowYesNo("此操作不可逆转，确定删除吗？")!=DialogResult.Yes) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",Ids));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DEL_Expensereimbursement_ByID", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("删除成功!");
                    //myGridView1.DeleteRow(rows);
                    cbRetrieve.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnConfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
   
            string auditMan = "";

            string batchs = "";
            string feeTypes = "";
            


            
            for (int i = 0; i < myGridView1.RowCount;i++ )
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    batchs = batchs + myGridView1.GetRowCellValue(i, "Batch").ToString() + "@";
                    feeTypes = feeTypes + myGridView1.GetRowCellValue(i, "FeeType").ToString() + "@";
                    auditIds = auditIds + myGridView1.GetRowCellValue(i, "ID").ToString() + "@";
                    auditMan = myGridView1.GetRowCellValue(i, "AuditMan").ToString();
                    if (auditMan != "")
                    {
                        MsgBox.ShowOK("存在核销的单，请重新选择！");
                        return;
                    }
                }
            }

            if (batchs == "")
            {
                MsgBox.ShowOK("请选择要核销的单!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batch", batchs));
                list.Add(new SqlPara("FeeTypes",feeTypes));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Expensereimbursement_ByBatch", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count < 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
                barButtonItem5.Enabled = true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        int times = 0; //汇款客户资料取3次
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANK_CUSTOMER");
                DataSet ds = SqlHelper.GetDataSet(sps);
                e.Result = new object[] { 1, ds };
            }
            catch (Exception ex)
            {
                e.Result = new object[] { 0, ex.Message };
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] obj = e.Result as object[];
            if (Convert.ToInt32(obj[0]) == 0)
            {
                if (times < 3)
                {
                    times++;
                    backgroundWorker1.RunWorkerAsync();
                    return;
                }
                return;
            }
            dsshipper = obj[1] as DataSet;
        }

        private void btnUnconfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int rows = myGridView1.FocusedRowHandle;
           // if (rows < 0) return;
            myGridView1.PostEditor();
            //string Ids = "";
            string batchs = "";
            string auditMan = "";
            string pushMan = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                   // Ids = Ids + myGridView1.GetRowCellValue(i, "ID").ToString() + "@";
                    batchs = batchs + myGridView1.GetRowCellValue(i, "Batch").ToString() + "@";
                    auditMan = myGridView1.GetRowCellValue(i, "AuditMan").ToString();
                    pushMan = myGridView1.GetRowCellValue(i, "PushMan").ToString();
                    if (auditMan.Trim() == "")
                    {
                        MsgBox.ShowOK("存在未核销的单，请重新选择！");
                        return;
                    }
                    if (pushMan.Trim() != "")
                    {
                        MsgBox.ShowOK("您选择的单已经转付款不能反核销");
                           return;
                    }
                }
            }
           
            //string id = myGridView1.GetRowCellValue(rows, "ID").ToString();
            if (batchs == "") return;
            try
            {
                //List<SqlPara> listQuery = new List<SqlPara>();
                //listQuery.Add(new SqlPara("Batchs", batchs));
                //SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_Bank_ByBatch", listQuery);
                //DataSet ds = SqlHelper.GetDataSet(spsQuery);
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    string batchMsg = "";
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        batchMsg = batchMsg + ds.Tables[0].Rows[i]["FeeBatch"].ToString() + ",";
                    
                //    }
                //    MsgBox.ShowOK("批次号:"+batchMsg+"已经付款，不能反审核!");
                //    return;
                //}



                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batchs", batchs));
                list.Add(new SqlPara("OperType", 2));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ExpenseAudit", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("反核销成功!");
                    cbRetrieve.PerformClick();
                    //myGridView1.DeleteRow(rows);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;
            string batch = myGridView1.GetRowCellValue(row, "Batch").ToString();
            string feeTypes = myGridView1.GetRowCellValue(row, "Batch").ToString();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batch",batch+"@"));
                list.Add(new SqlPara("FeeTypes", feeTypes + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Expensereimbursement_ByBatch", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count < 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
                barButtonItem5.Enabled = false;
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int j=myGridView2.RowCount;
            for(int i=0;i<j;i++)
            {
                myGridView2.DeleteRow(0);
            }
            
        }
             //完成审核
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView2.PostEditor();
            string ids = "";
            string batchs = "";
            string auditMoneys = "";
            string feeTypes = "";
            
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                decimal all =ConvertType.ToDecimal( myGridView2.GetRowCellValue(i, "Money").ToString());
                decimal thitime = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AuditMoney").ToString());
                if (thitime > all)
                {
                    MsgBox.ShowOK("第" + (i + 1) + "行支出金额不能大于申报金额,请核对!");
                    return;
                }
                ids = ids + myGridView2.GetRowCellValue(i, "ID").ToString() + "@";
                batchs = batchs + myGridView2.GetRowCellValue(i, "Batch").ToString() + "@";
                auditMoneys = auditMoneys + myGridView2.GetRowCellValue(i, "AuditMoney").ToString() + "@";
                feeTypes = feeTypes + myGridView2.GetRowCellValue(i, "FeeType").ToString() + "@";
            }
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDs",ids));
                list.Add(new SqlPara("Batchs", batchs));
                list.Add(new SqlPara("AuditMoneys", auditMoneys));
                list.Add(new SqlPara("OperType",1));
                list.Add(new SqlPara("AuditIds",auditIds));
                list.Add(new SqlPara("FeeTypes",feeTypes));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ExpenseAudit", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("核销成功!");
                    int j = myGridView2.RowCount;
                    for (int k = 0; k < j; k++)
                    {
                        myGridView2.DeleteRow(0);
                    }
                }


            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnExist_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

  

        private void btnReceive_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             myGridView1.PostEditor();
            string acceptMan = "";
            string ids = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
        
                    ids = auditIds + myGridView1.GetRowCellValue(i, "ID").ToString() + "@";
                    acceptMan = myGridView1.GetRowCellValue(i, "AcceptMan").ToString();
                    if (acceptMan != "")
                    {
                        MsgBox.ShowOK("存在已接收过的单，请重新选择!");
                        return;
                    }
                }
            }
            if (ids == "")
            {
                MsgBox.ShowOK("请选择要接收的单!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDs",ids));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_GET_ExpenseAccept", list);
                if(SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnPay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string batchCheck = "";
            string feeTypes = "";
            string auditMan = "";
            
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    batchCheck = batchCheck + myGridView1.GetRowCellValue(i, "Batch").ToString()+"@";
                    feeTypes=feeTypes+myGridView1.GetRowCellValue(i,"FeeType").ToString()+"@";
                    auditMan = myGridView1.GetRowCellValue(i, "AuditMan").ToString();
                    if (auditMan.Trim() == "")
                    {
                        MsgBox.ShowOK("您选择了未核销的单，请重新选择!");
                        return;
                    }
                }
            }
            if (batchCheck == "")
            {
                MsgBox.ShowOK("请选择要转付款的单!");
                return;
            }
            for (int j = 0; j < myGridView1.RowCount; j++)
            {
                if (Convert.ToInt32(myGridView1.GetRowCellValue(j, "ischecked")) == 1) continue;
                string batch = myGridView1.GetRowCellValue(j, "Batch").ToString();
                if (batchCheck.Contains(batch))
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (Convert.ToInt32(myGridView1.GetRowCellValue(j, "ischecked")) == 1) continue;
                        string batch1 = myGridView1.GetRowCellValue(j, "Batch").ToString();
                        if (batch1 == batch)
                        {
                            myGridView1.SetRowCellValue(i, "ischecked", 1);
                            batchCheck += myGridView1.GetRowCellValue(i, "Batch").ToString() + "@";
                            feeTypes += myGridView1.GetRowCellValue(i, "FeeType").ToString() + "@";
                        }
                    }
                    //MsgBox.ShowOK("批次号："+batch+"只选中了部分，同一个批次号，只能一起转付款，请重新选择!");
                    //return;
                }
            
            }
         

            try 
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batchs",batchCheck));
                list.Add(new SqlPara("FeeTypes",feeTypes));
                list.Add(new SqlPara("OperType",1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute,"USP_ADD_ExpensePush",list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                } 
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnUnPay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string batchCheck = "";
            string feeTypes = "";
            string pushMan = "";
            string appby = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    batchCheck = batchCheck + myGridView1.GetRowCellValue(i, "Batch").ToString() + "@";
                    feeTypes = feeTypes + myGridView1.GetRowCellValue(i, "FeeType").ToString() + "@";
                    pushMan = myGridView1.GetRowCellValue(i, "PushMan").ToString();
                    appby = myGridView1.GetRowCellValue(i, "appby").ToString();
                    if (pushMan.Trim() == "")
                    {
                        MsgBox.ShowOK("您选择了未转付的单，请重新选择!");
                        return;
                    }
                    if (appby.Trim() != "")
                    {
                        MsgBox.ShowOK("您选择的单已实际付款,不能取消转付");
                        return;
                    }
                }
            }
            try 
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batchs", batchCheck));
               // list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("OperType", 2));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ExpensePush", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                } 
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                GridOper.ExportToExcel(myGridView1);
            }
            else
            {
                GridOper.ExportToExcel(myGridView2);
            }
        }

        private void btnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }
    }
}
