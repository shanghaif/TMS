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
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
//luohui
namespace ZQTMS.UI
{
    public partial class frmInternalWithhold : BaseForm
    {
        public frmInternalWithhold()
        {
            InitializeComponent();
        }
        int NO;
        GridColumn gcIsseleckedMode;
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WITHHOLDING", list);
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

        private void InternalWithhold_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("内部代扣");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.CreateStyleFormatCondition(myGridView1, "Status", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已审核", Color.Yellow);//已确认的颜色变黄

            GridOper.CreateStyleFormatCondition(myGridView1, "Executor", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Green);//执行的颜色变绿
            GridOper.CreateStyleFormatCondition(myGridView1, "VetoPerson", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "", Color.Red);//否决的颜色变绿

            CommonClass.SetCause(Cause, true);
            web.Text = CommonClass.UserInfo.WebName;

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
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
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "网点对账");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

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


        // luohui 新增
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            frmInternalWithholdAdd frm = new frmInternalWithholdAdd();
            frm.type = 0;
            frm.Show();
        }

        //修改
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            string auditor = myGridView1.GetRowCellValue(rowHandle, "Auditor").ToString();
            if (auditor.Trim() == "")
            {
                MsgBox.ShowOK("已审核不能修改！");
                return;
            
            }

            string executor = myGridView1.GetRowCellValue(rowHandle, "Executor").ToString();
            if (executor != "")
            {
                MsgBox.ShowOK("已执行不能修改！");
                return;
            }

            frmInternalWithholdAdd frm = new frmInternalWithholdAdd();
           
            frm.Show();
            frm.id = myGridView1.GetRowCellValue(rowHandle, "ID").ToString();
            frm.type = 1;
            frm.getdata();



          
        }
        //删除

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;

            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择数据");
                return;
            }
            if (MsgBox.ShowYesNo("确定删除该条数据吗？该过程不可逆") != DialogResult.Yes)
            {
                return;

            }
            string status = myGridView1.GetRowCellValue(rowhandle, "Status").ToString();
            if (status == "已审核")
            {
                MsgBox.ShowOK("该条不能删除！");
                return;
            }

            //string vet1 = myGridView1.GetRowCellValue(rowhandle, "VetoPerson").ToString();
            //if (vet1!= "")
            //{
            //    MsgBox.ShowOK("否决的不能再删除");
            //    return;
            //}
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            if (ID == "") return;

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_DELETE_WithHolding_BY_ID", new List<SqlPara> { new SqlPara("ID", ID) });
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK();
            }

        }
        //审核 批量
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int num = 0;
            myGridView1.PostEditor();
            int rowhandle = myGridView1.FocusedRowHandle;

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

           
         
            string ID = "";

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    string vet = myGridView1.GetRowCellValue(i, "VetoPerson").ToString();
                    if (vet != "")
                    {
                        MsgBox.ShowOK("存在已否决的不能审核");
                        return;
                    }


                    string sts = myGridView1.GetRowCellValue(i, "Status").ToString();
                    if (sts == "已审核")
                    {
                        MsgBox.ShowOK("存在已审核不能再审核");
                        return;
                    }
                    if (myGridView1.GetRowCellValue(i, "Executor").ToString() != "")
                    {
                        MsgBox.ShowOK("选中的数据中有已执行的数据，不能审核！请重新选择。");
                        return;
                    }
                    else
                    {
                        ID += myGridView1.GetRowCellValue(i, "ID") + "@";
                       
                    }

                }
            }
            NO = 0;

            try
            {

                string[] str = ID.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("ID", ID));
                list0.Add(new SqlPara("NO", NO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", list0);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


           
          

            
        }


        private void unAudit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int num = 0;
            myGridView1.PostEditor();
            string ID = "";
            int rowhandle = myGridView1.FocusedRowHandle;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    string sts = myGridView1.GetRowCellValue(i, "Status").ToString();
                    if (sts == "执行")
                    {
                        MsgBox.ShowOK("存在已执行的单不能反审核");
                        return;
                    }
                    if (sts.Trim() == "")
                    {
                        MsgBox.ShowOK("存在未审核的单不能反审核");
                        return;
                    }
                    ID += myGridView1.GetRowCellValue(i, "ID") + "@";
                }
            }
            try
            {

                string[] str = ID.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("ID", ID));
                list0.Add(new SqlPara("NO", 3));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", list0);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //退出
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //执行
        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            int num = 0;
            myGridView1.PostEditor();
            int rowhandle = myGridView1.FocusedRowHandle;
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

            string executor = myGridView1.GetRowCellValue(rowhandle, "Executor").ToString();
            if (executor != "")
            {
                MsgBox.ShowOK("已执行不能再执行！");
                return;
            }

            string ID = "";

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    if (myGridView1.GetRowCellValue(i, "Executor").ToString() != "")
                    {
                        MsgBox.ShowOK("选中的数据中有已确认的数据，不能重复确认！请重新选择。");
                        return;
                    }
                    if (myGridView1.GetRowCellValue(i, "Auditor").ToString().Trim() == "")
                    {
                        MsgBox.ShowOK("选中的数据有未审核的数据，请重新选择！");
                        return;
                    }
                    else
                    {
                        ID += myGridView1.GetRowCellValue(i, "ID") + "@";

                    }

                }
            }
            NO = 1;

            try
            {

                string[] str = ID.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("ID", ID));
                list0.Add(new SqlPara("NO", NO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", list0);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


            //int rowhandle = myGridView1.FocusedRowHandle;

           // if (rowhandle < 0)
           // {
           //     MsgBox.ShowOK("请选择数据");
           //     return;
           // }
           // string exe=myGridView1.GetRowCellValue(rowhandle, "Status").ToString();
           //if (exe != "已审核")
           //{
           //    MsgBox.ShowOK("该条未审核不能执行"); 
           //    return;
           //}
           

           // if (MsgBox.ShowYesNo("是否执行该条数据？") != DialogResult.Yes)
           // {
           //     return;

           // }

            //string executor = myGridView1.GetRowCellValue(rowhandle, "Executor").ToString();
            //if (executor != "")
            //{
            //    MsgBox.ShowOK("已执行不能再执行！");
            //    return;
            //}
           // NO = 1;
           // string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
           // if (ID == "") return;

           // SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", new List<SqlPara> { new SqlPara("ID", ID), new SqlPara("NO", NO) });
           // if (SqlHelper.ExecteNonQuery(sps) > 0)
           // {
           //     MsgBox.ShowOK();
           // }
        }

        //否决
        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            int num = 0;
            myGridView1.PostEditor();
            int rowhandle = myGridView1.FocusedRowHandle;


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
            string exe = myGridView1.GetRowCellValue(rowhandle, "Status").ToString();
            if (exe == "执行")
            {
                MsgBox.ShowOK("该条已审核不能再否决");
                return;
            }

            string ID = "";

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1"))
                {
                    if (myGridView1.GetRowCellValue(i, "Executor").ToString() != "")
                    {
                        MsgBox.ShowOK("已执行，不能否决,请重新选择！");
                        return;
                    }
                    else
                    {
                        ID += myGridView1.GetRowCellValue(i, "ID") + "@";

                    }

                }
            }
            NO = 2;

            try
            {

                string[] str = ID.Split('@');
                List<SqlPara> list0 = new List<SqlPara>();
                list0.Add(new SqlPara("ID", ID));
                list0.Add(new SqlPara("NO", NO));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", list0);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            
            
            
            
            
            
            
            //int rowhandle = myGridView1.FocusedRowHandle;

            //if (rowhandle < 0)
            //{
            //    MsgBox.ShowOK("请选择数据");
            //    return;
            //}
            //string exe = myGridView1.GetRowCellValue(rowhandle, "Status").ToString();
            /**if (exe != "已审核")
            //{
            //    MsgBox.ShowOK("该条未审核不能执行");
            //    return;
            //**/

            //if(exe=="已审核")
            //{
            //    MsgBox.ShowOK("该条已审核不能再否决");
            //    return;
            //}


            //if (MsgBox.ShowYesNo("是否执行该条数据？") != DialogResult.Yes)
            //{
            //    return;

            //}


            //NO = 2;
            //string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //if (ID == "") return;

            //SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_COMFIRM_InternalWithhold_BY_ID", new List<SqlPara> { new SqlPara("ID", ID), new SqlPara("NO", NO) });
            //if (SqlHelper.ExecteNonQuery(sps) > 0)
            //{
            //    MsgBox.ShowOK();
            //}
        }

        //导入luohui
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            frmInternalWithholdUP frm = new frmInternalWithholdUP();
            frm.Show();
           
        }

        //导出luohui
        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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