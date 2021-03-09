using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.IO;
using System.Net;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;


namespace ZQTMS.UI
{
    public partial class frmExpenseBudget : BaseForm
    {
        public frmExpenseBudget()
        {
            InitializeComponent();
        }

        int NO;
        GridColumn gcIsseleckedMode;
        //提取按钮
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            //if (registrant.Text != "")
            //{
            //    try
            //    {
            //        List<SqlPara> list1 = new List<SqlPara>();
            //        list1.Add(new SqlPara("registrant", registrant.Text.Trim()));
            //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBudget_BYREG", list1);//提取存储过程
            //        DataSet ds = SqlHelper.GetDataSet(sps);

            //        if (ds == null || ds.Tables.Count == 0) return;
            //        myGridControl1.DataSource = ds.Tables[0];
            //    }
            //    catch (Exception ex)
            //    {
            //        MsgBox.ShowException(ex);
            //    }
            //}
            //else
            //{
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("t1", bdate.DateTime));
                    list.Add(new SqlPara("t2", edate.DateTime));
                    list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                    list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                    list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                    list.Add(new SqlPara("RegisterMan", registrant.Text.Trim() == "全部" ? "%%" : registrant.Text.Trim()));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBudget", list);

                    DataSet ds = SqlHelper.GetDataSet(sps);
                    myGridControl1.DataSource = ds.Tables[0];
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                }
            
        }
        //窗体加载
        private void ExpenseBudget_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("费用预算");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例


            
            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetWeb(web, Area.Text);
            CommonClass.SetUser(registrant, web.Text);

            //web.Text = CommonClass.UserInfo.WebName;

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            registrant.Text = CommonClass.UserInfo.UserName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.CreateStyleFormatCondition(myGridView1, "ConfirmMan", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Yellow);//已确认的颜色变黄

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }
        //退出按钮
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //事业部下拉
        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }
        //大区下拉
        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }
        private void web_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(registrant, web.Text);
        }
        //锁定外观
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        //删除外观
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        //过滤器
        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //导出Excel
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "费用预算");
        }
        //自动筛选
        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        //下面退出按钮
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }
        //新增按钮
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmNewBudget frm = new frmNewBudget();
            frm.type = 0;
            frm.Show();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            myGridView1.UpdateCurrentRow();
            int rowhandle = myGridView1.FocusedRowHandle;

            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择数据");
                return;
            }
            if ((myGridView1.GetRowCellValue(rowhandle, "ConfirmMan").ToString() != "") && myGridView1.GetRowCellValue(rowhandle, "UnConfirmMan").ToString() == "")
            {
                MsgBox.ShowOK("该预算已确定，不能修改!");
                return;
            }
            else
            {

                frmNewBudget frm = new frmNewBudget();

                frm.Show();
                frm.id = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
                frm.type = 1;
                frm.getdata();
                
              
            }


        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)

        {

            myGridView1.PostEditor();
            myGridView1.UpdateCurrentRow(); 
            int rowhandle = myGridView1.FocusedRowHandle;

            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择数据");
                return;
            }
           if(MsgBox.ShowYesNo("确定删除该条数据吗？该过程不可逆") != DialogResult.Yes){
               return;
          
           }
           if ((myGridView1.GetRowCellValue(rowhandle, "ConfirmMan").ToString() != "" && myGridView1.GetRowCellValue(rowhandle, "UnConfirmMan").ToString() == ""))
           {
               MsgBox.ShowOK("该预算已确定，不能删除!");
               return;
           }
             string ID= myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
             if (ID == "") return;

             SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_DELETE_ExpenseBudget_BY_ID", new List<SqlPara> { new SqlPara("ID", ID) });
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
            }

             
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             int num = 0;
            myGridView1.PostEditor();
            //myGridView1.UpdateCurrentRow();
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    num++;
                }
            }
            if (num == 0)
            {
                MsgBox.ShowOK("未选择任何数据！");
                return;
            }

            string sShowOK = "确认数据数：" + num
                     + "\r\n是否继续？";
            if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            {
                num = 0;
                return;
            }


            string IDs = "";
            //string  NOs = "";


            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    if (myGridView1.GetRowCellValue(i, "ConfirmMan").ToString()!="")
                    {
                        MsgBox.ShowOK("选中的数据中有已确认的数据，不能重复确认！请重新选择。");
                        return;
                    }
                    else
                    {
                        IDs += myGridView1.GetRowCellValue(i, "ID") + "@";
                       // NOs += myGridView1.GetRowCellValue(i, "NO") + "@";
                    }

                }
            }
            NO = 0;

            try
            {

                string[] str = IDs.Split('@');
                //string[] str1 = NOs.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("IDs", IDs));
                list0.Add(new SqlPara("NO", NO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_ExpenseBudget_BY_ID", list0);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, e);
                    
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            //myGridView1.PostEditor();
            //myGridView1.UpdateCurrentRow();
            //int rowhandle = myGridView1.FocusedRowHandle;

            //if (rowhandle < 0)
            //{
            //    MsgBox.ShowOK("请选择数据");
            //    return;
            //}
            //if (MsgBox.ShowYesNo("是否确定该条数据？") != DialogResult.Yes)
            //{
            //    return;

            //}
            // if (myGridView1.GetRowCellValue(rowhandle, "ConfirmMan").ToString() != "")
            // {
            // MsgBox.ShowOK("该预算已确定，不能重复确认!");
            //   return;
            // }
            //NO = 0;
            //string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //if (ID == "") return;

            //SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_ExpenseBudget_BY_ID", new List<SqlPara> { new SqlPara("ID", ID), new SqlPara("NO", NO) });
            //if (SqlHelper.ExecteNonQuery(sps) > 0)
            //{
            //    MsgBox.ShowOK();
            //}


        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int num = 0;
            myGridView1.PostEditor();
            //myGridView1.UpdateCurrentRow();
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    num++;
                }
            }
            if (num == 0)
            {
                MsgBox.ShowOK("未选择任何数据！");
                return;
            }

            string sShowOK = "取消确认数据数：" + num
                     + "\r\n是否继续？";
            if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
            {
                num = 0;
                return;
            }


            string IDs = "";
            //string  NOs = "";


            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    if (myGridView1.GetRowCellValue(i, "UnConfirmMan").ToString() != "")
                    {
                        MsgBox.ShowOK("选中的数据中有已取消确认的数据，不能重复取消确认！请重新选择。");
                        return;
                    }
                    else
                    {
                        IDs += myGridView1.GetRowCellValue(i, "ID") + "@";
                        // NOs += myGridView1.GetRowCellValue(i, "NO") + "@";
                    }

                }
            }
            NO = 1;

            try
            {

                string[] str = IDs.Split('@');
                //string[] str1 = NOs.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("IDs", IDs));
                list0.Add(new SqlPara("NO", NO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_ExpenseBudget_BY_ID", list0);

                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(sender, e);

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            //myGridView1.PostEditor();
            //    myGridView1.UpdateCurrentRow();
            //int rowhandle = myGridView1.FocusedRowHandle;

            //if (rowhandle < 0)
            //{
            //    MsgBox.ShowOK("请选择数据");
            //    return;
            //}
            //if (MsgBox.ShowYesNo("是否取消确定该条数据？") != DialogResult.Yes)
            //{
            //    return;

            //}
            //if (myGridView1.GetRowCellValue(rowhandle, "ConfirmMan").ToString() == "") 
            //{
            //    MsgBox.ShowOK("该数据未确认不能取消确认!");
            //    return;
            //}
            //if (myGridView1.GetRowCellValue(rowhandle, "UnConfirmMan").ToString() != "")
            //{
            //    MsgBox.ShowOK("已经取消确认，不能重复取消!");
            //    return;
            //}
            //NO = 1;
            //string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //if (ID == "") return;

            //SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_ExpenseBudget_BY_ID", new List<SqlPara> { new SqlPara("ID", ID), new SqlPara("NO", NO) });
            //if (SqlHelper.ExecteNonQuery(sps) > 0)
            //{
            //    MsgBox.ShowOK();
            //}
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmExpenseBudgetUploading fm = new frmExpenseBudgetUploading();
            fm.Show();
            //getdata();
        }
        public void getdata() 
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBudget", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
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

       
    }
}