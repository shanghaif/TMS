using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class frmCollectionCommission : BaseForm
    {
        DataSet ds = new DataSet();
        DataSet dsNew = new DataSet();
        public frmCollectionCommission()
        {
            InitializeComponent();
        }

        private void frmCollectionCommission_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("代收代扣款登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            GridOper.CreateStyleFormatCondition(myGridView1, "states", DevExpress.XtraGrid.FormatConditionEnum. Equal, "执行", Color.Green);//执行的颜色变绿
           
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            getdata();



        }

        //2017-10-16wbw数据提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }
        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.EditValue));
                list.Add(new SqlPara("edate", edate.EditValue));
                list.Add(new SqlPara("Registrant", Registrant.Text == "全部" ? "%%" : Registrant.Text.Trim()));
                list.Add(new SqlPara("states", states.Text == "全部" ? "%%" : states.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_CollectionCommission", list);
                ds = SqlHelper.GetDataSet(spe);
                dsNew = ds.Clone();
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

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        //导出
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "代收代扣款明细");
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //新增
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCollectionCommissionADD fm = new frmCollectionCommissionADD();
            fm.ShowDialog();
            getdata();
        }

        //修改
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            string states = GridOper.GetRowCellValueString(myGridView1, rowHandle, "states");
            if (states != "申请")
            {
                MsgBox.ShowOK("只有审核前可以进行修改！");
                return;
            }
            string Registrant = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Registrant");
            if (Registrant != CommonClass.UserInfo.UserName)
            {
                MsgBox.ShowOK("只能修改自己的单！");
                return;
            }
            frmCollectionCommissionADD fm = new frmCollectionCommissionADD();
            fm.type = "1";
            string ID = GridOper.GetRowCellValueString(myGridView1,rowHandle,"ID");
            fm.ID = ID;
            fm.ShowDialog();
            getdata();
            cbRetrieve_Click(null, null);
        }

        //删除
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            string states = GridOper.GetRowCellValueString(myGridView1, rowHandle, "states");
            if (states != "申请")
            {
                MsgBox.ShowOK("只有申请状态下可以删除！");
                return;
            }
            string Registrant = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Registrant");
            if (Registrant != CommonClass.UserInfo.UserName)
            {
                MsgBox.ShowOK("只能删除自己的单！");
                return;
            }
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string ID = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ID");
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DELETE_CollectionCommission_ID", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(null,null);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }



        }

      
       


        //批量确认
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否审核?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            myGridView1.PostEditor();
            //int rowhandle = myGridView1.RowCount;
            //for (int i = 0; i < myGridView1.RowCount; i++)
            //{
            //    System.Threading.Thread.Sleep(300);
            //    if (myGridView1.RowCount >= 80)
            //    {
            //        DataTable table = ds.Tables[0];
            //        DataTable table2 = dsNew.Tables[0];
            //        table2.Rows.Clear();
            //        for (int k = 0; k < 80; k++)
            //        {
            //            table2.Rows.Add(table.Rows[k].ItemArray);
            //        }
            //        table2.AcceptChanges();
            //        myGridControl2.DataSource = dsNew.Tables[0];
            //        senddata(myGridView2,dsNew);

            //        if (dsNew.Tables[0].Rows.Count == 0)
            //        {
            //            for (int m = 0; m < 80; m++)
            //            {
            //                ds.Tables[0].Rows[m].Delete();
            //            }
            //            ds.AcceptChanges();
            //        }

            //        if (ds.Tables[0].Rows.Count== 0 || myGridView1.RowCount==0) return;
            //        if (myGridView1.RowCount < 80 && myGridView1.RowCount > 0)
            //        {
            //            senddata(myGridView1, ds);
            //            cbRetrieve_Click(null,null);
            //            return;
            //        }


            //    }
            //    if (myGridView1.RowCount > 0 && myGridView1.RowCount < 80)
            //    {
            //        senddata(myGridView1, ds);
            //        cbRetrieve_Click(null,null);
            //        return;
            //    }

            //}
            //2017.12.14wbw
            int rowhandle = myGridView1.RowCount;
             string IDS = "";
             for (int i = 0; i < rowhandle; i++)
             {
                 if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                 {
                     IDS += myGridView1.GetRowCellValue(i, "ID") + "@";
                 }
             }
             List<SqlPara> list = new List<SqlPara>();
             list.Add(new SqlPara("IDS", IDS));
             list.Add(new SqlPara("Auditor", CommonClass.UserInfo.UserName));
             list.Add(new SqlPara("AuditorDate", DateTime.Now.ToString()));
             SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly", list);
             if (SqlHelper.ExecteNonQuery(spe) > 0)
             {
                 MsgBox.ShowOK();
                 cbRetrieve_Click(null, null);
             }


        }

        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            int o = checkAll.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", o);
            }
        }

        private void senddata(MyGridView myGridView2,DataSet ds1)
        {
            try
            {
                myGridView1.PostEditor();
                int rowhandle = myGridView2.RowCount;
                string IDS = "";
                for (int i = 0; i < rowhandle; i++)
                {
                    if (ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        IDS += myGridView2.GetRowCellValue(i, "ID") + "@";
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDS",IDS));
                list.Add(new SqlPara("Auditor", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("AuditorDate", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    ds1.Clear();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void senddata2(MyGridView myGridView2, DataSet ds1)
        {

            try
            {
                myGridView1.PostEditor();
                int rowhandle = myGridView2.RowCount;
                string IDS = "";
                for (int i = 0; i < rowhandle; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        IDS += myGridView1.GetRowCellValue(i, "ID") + "@";
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDS", IDS));
                list.Add(new SqlPara("Executor", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("executiontime", DateTime.Now.ToString()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly2", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    ds1.Clear();
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        //批量执行
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否执行?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            //int rowhandle = myGridView1.RowCount;
            //for (int i = 0; i < rowhandle; i++)
            //{
            //    System.Threading.Thread.Sleep(300);
            //    if (myGridView1.RowCount >= 80)
            //    {
            //        DataTable table = ds.Tables[0];
            //        DataTable table2 = dsNew.Tables[0];
            //        table2.Rows.Clear();
            //        for (int k = 0; k < 80; k++)
            //        {
            //            table2.Rows.Add(table.Rows[k].ItemArray);
            //        }
            //        table2.AcceptChanges();
            //        myGridControl2.DataSource = dsNew.Tables[0];
            //        senddata2(myGridView2, dsNew);

            //        if (dsNew.Tables[0].Rows.Count == 0)
            //        {
            //            for (int m = 0; m < 80; m++)
            //            {
            //                ds.Tables[0].Rows[m].Delete();
            //            }
            //            ds.AcceptChanges();
            //        }

            //        if (ds.Tables[0].Rows.Count == 0 || myGridView1.RowCount == 0) return;
            //        if (myGridView1.RowCount < 80 && myGridView1.RowCount > 0)
            //        {
            //            senddata2(myGridView1, ds);
            //            cbRetrieve_Click(null, null);
            //            return;
            //        }


            //    }
            //    if (myGridView1.RowCount > 0 && myGridView1.RowCount < 80)
            //    {
            //        senddata2(myGridView1, ds);
            //        cbRetrieve_Click(null, null);
            //        return;
            //    }

            //}

            //2017.12.15wbw
            myGridView1.PostEditor();
            int rowhandle = myGridView1.RowCount;
            string IDS = "";
            for (int i = 0; i < rowhandle; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                {
                    IDS += myGridView1.GetRowCellValue(i, "ID") + "@";
                }
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("IDS", IDS));
            list.Add(new SqlPara("Executor", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("executiontime", DateTime.Now.ToString()));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly2", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                cbRetrieve_Click(null, null);
            }

        }

        //否决  可批量
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataTable dt = null;
                if (!CheckSelect(ref dt)) return;
                if (dt == null) return;
                string IDS = "";
                if (MsgBox.ShowYesNo("是否执行?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }

                foreach (DataRow dr in dt.Rows)
                {
                    IDS += dr["ID"] + "@";
                }

                if (string.IsNullOrEmpty(IDS))
                {
                    MsgBox.ShowError("未找到需要否决的数据！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("IDS", IDS));
                list.Add(new SqlPara("VetoPerson", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("VetoTime", DateTime.Now.ToString()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly3", list)) > 0)
                {
                    MsgBox.ShowOK("否决成功！");
                    getdata();
                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }





            //单条否决
            //int rowhandle = myGridView1.FocusedRowHandle;
            //if (rowhandle < 0) {
            //    MsgBox.ShowOK("请选择一条信息!");
            //    return; }
            //string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            //List<SqlPara> list = new List<SqlPara>();
            //list.Add(new SqlPara("ID",ID));
            //list.Add(new SqlPara("VetoPerson", CommonClass.UserInfo.UserName));
            //list.Add(new SqlPara("VetoTime", DateTime.Now.ToString()));
            //SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_CollectionCommission_APPly3", list);
            //if (SqlHelper.ExecteNonQuery(spe) > 0)
            //{
            //    MsgBox.ShowOK("否决成功！");
            //cbRetrieve_Click(null, null);
            //}

        }

        //导入
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCollectionCommissionUP fm = new frmCollectionCommissionUP();
            fm.ShowDialog();

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }


        //检测是否钩选
        private bool CheckSelect(ref DataTable SelectDt)
        {
            myGridView1.PostEditor();
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return false;
            SelectDt = dt.Clone();


            foreach (DataRow dr in dt.Rows)
            {
                if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                SelectDt.ImportRow(dr);//将选择的行存到新表
            }
            if (SelectDt.Rows.Count == 0)
            {
                MsgBox.ShowOK("请选择数据!");
                return false;
            }
            return true;

        }       


    }
}