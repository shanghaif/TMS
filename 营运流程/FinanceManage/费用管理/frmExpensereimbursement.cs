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
    public partial class frmExpensereimbursement : BaseForm
    {
        public frmExpensereimbursement()
        {
            InitializeComponent();
        }
        DataSet dsshipper = new DataSet();//汇款客户资料
        GridColumn gcIsseleckedMode;
        private void frmExpensereimbursement_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("费用报销登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetWeb(BegWeb,true);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);           
            CommonClass.SetUser(BillMan, BegWeb.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;     
          
            BegWeb.Text = CommonClass.UserInfo.WebName;
            BillMan.Text = CommonClass.UserInfo.UserName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView1, "ConfirmMan", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Yellow);
            backgroundWorker1.RunWorkerAsync();//汇款客户资料
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");

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
            CommonClass.SetUser(BillMan, BegWeb.Text);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                //List<SqlPara> listbuget = new List<SqlPara>();
                //listbuget.Add(new SqlPara("t1", bdate.DateTime));
                //listbuget.Add(new SqlPara("t2", edate.DateTime));

                //listbuget.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                //listbuget.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                //listbuget.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                ////listbuget.Add(new SqlPara("RegistMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));
                //SqlParasEntity spsbuget = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBudgetBalance", listbuget);

                //DataSet dsbuget = SqlHelper.GetDataSet(spsbuget);



                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("RegistMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Expensereimbursement", list);
                
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0) return;
                myGridControl1.DataSource = ds.Tables[0];
                //for (int i = 0; i < myGridView1.RowCount; i++)
                //{ 
                   
                
                //}
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
            string id = myGridView1.GetRowCellValue(rows, "ID").ToString();
            string confirmMan = myGridView1.GetRowCellValue(rows, "ConfirmMan").ToString();
            if (confirmMan != "")
            {
                MsgBox.ShowOK("此单已经确认不能修改!");
                return;
            }
            ExpenseReimbursementAdd edit = new ExpenseReimbursementAdd();
            edit.id = id;
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
           //int rows = myGridView1.FocusedRowHandle;
          //if (rows < 0) return;
            myGridView1.PostEditor();
            string Ids = "";
            decimal moneySum = 0;
            string applyDept = "";
            string feeType = "";//费用类型
            string feeTypeTmp = "";
            string applyCause = "";
            string applyArea = "";
            string confirmMan = "";
            string applyDeptTmp = "";
            string remarkInfo = "";//备注信息
            string belongMonth = "";//所属月份
            
            for (int i = 0; i < myGridView1.RowCount;i++ )
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    Ids = Ids + myGridView1.GetRowCellValue(i, "ID").ToString()+"@";
                    moneySum = moneySum + Convert.ToDecimal(myGridView1.GetRowCellValue(i, "Money").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "Money").ToString());
                    applyDeptTmp = myGridView1.GetRowCellValue(i, "ApplyDept").ToString();
                    confirmMan = myGridView1.GetRowCellValue(i, "ConfirmMan").ToString();
                    feeTypeTmp = myGridView1.GetRowCellValue(i, "FeeType").ToString();
                    belongMonth = myGridView1.GetRowCellValue(i, "BelongMonth").ToString();
                    remarkInfo += "支付" + applyDeptTmp +belongMonth + "-" + feeTypeTmp + ",";//拼接备注内容
                    if (confirmMan != "")
                    {
                        MsgBox.ShowOK("存在已经确认的单，请重新选择！");
                        return;
                    }
                    if (applyDept != "")
                    {
                        if (applyDept != applyDeptTmp)
                        {
                            MsgBox.ShowOK("只能选择相同申报部门的记录确认！");
                            return;
                        }
                    }
                    else
                    {
                        applyDept = myGridView1.GetRowCellValue(i, "ApplyDept").ToString();
                    }
                    //if (feeType != "")
                    //{
                    //    if (feeTypeTmp != feeType)
                    //    {
                    //        MsgBox.ShowOK("只能选择相同费用类型的记录确认!");
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    feeType = myGridView1.GetRowCellValue(i, "FeeType").ToString();
                    //}
                    if (applyCause == "")
                    {
                        applyCause = myGridView1.GetRowCellValue(i, "ApplyCause").ToString();
                    }
                    if (applyArea == "")
                    {
                        applyArea = myGridView1.GetRowCellValue(i, "ApplyArea").ToString();
                    }
                }
            }
            //decimal money = Convert.ToDecimal( myGridView1.GetRowCellValue(rows, "Money"));
            //string applyDept = myGridView1.GetRowCellValue(rows, "ApplyDept").ToString();
            //string projectType = myGridView1.GetRowCellValue(rows, "FeeProject").ToString();
          //  string id = myGridView1.GetRowCellValue(rows, "ID").ToString();
            if (Ids == "")
            {
                MsgBox.ShowOK("请选择要确认的单!");
                return;
            }

            ExpenseBankInfo info = new ExpenseBankInfo();
            info.dsshipper = dsshipper;
            info.money = moneySum;
            info.applyDept = applyDept;
            info.feeType = feeType;
            info.applyCause = applyCause;
            info.applyArea = applyArea;
            info.Gid = Ids;
            info.pageType = "confirm";
            info.remarkInfo = remarkInfo;
            info.ShowDialog();
            cbRetrieve.PerformClick();
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
            string Ids = "";
            string confirmMan = "";
            string auditMan = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    Ids = Ids + myGridView1.GetRowCellValue(i, "ID").ToString() + "@";
                    confirmMan = myGridView1.GetRowCellValue(i, "ConfirmMan").ToString();
                    auditMan = myGridView1.GetRowCellValue(i, "AuditMan").ToString();
                    if (confirmMan.Trim() == "")
                    {
                        MsgBox.ShowOK("存在未确认的单，请重新选择！");
                        return;
                    }
                    if(auditMan!="")
                    {
                        MsgBox.ShowOK("存在已经审核的单，请重新选择！");
                        return;
                    }
                }
            }
           
            //string id = myGridView1.GetRowCellValue(rows, "ID").ToString();
            if (Ids == "") return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", Ids));
                list.Add(new SqlPara("OperType", 2));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Expensereimbursement_Confirm", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("取消确认成功!");
                    cbRetrieve.PerformClick();
                    //myGridView1.DeleteRow(rows);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnExist_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ExpenseUp upload = new ExpenseUp();
            upload.Show();
        }

        //导出
        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
    }
}
