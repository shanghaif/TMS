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
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmAuditWayBill : BaseForm
    {
        public frmAuditWayBill()
        {
            InitializeComponent();
        }

        private void frmAuditWayBill_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("运单审核");//xj2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar5); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetUser(BillMan, BegWeb.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            AuditState.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            BillMan.Text = CommonClass.UserInfo.UserName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "AuditState", FormatConditionEnum.Equal, "已审核", Color.FromArgb(0, 255, 0));//颜色固定-蓝色
           // GridOper.CreateStyleFormatCondition(myGridView1, "AuditState", FormatConditionEnum.Equal, "未审核", Color.FromArgb(0, 255, 0));//颜色固定--绿色
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

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            myGridView1.ClearColumnsFilter();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("AuditState", AuditState.Text.Trim() == "全部" ? "%%" : AuditState.Text.Trim()));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("BillMan", BillMan.Text.Trim() == "全部" ? "%%" : BillMan.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_Audit", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
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
        //private void simpleButton1_Click_1(object sender, EventArgs e)
        //{
        //    int s = simpleButton1.Text.Trim().Equals("全选") ? 1 : 0;
        //    simpleButton1.Text = simpleButton1.Text == "全选" ? "全不选" : "全选";
        //    for (int i = 0; i < myGridView1.RowCount; i++)
        //    {
        //        myGridView1.SetRowCellValue(i, "ischecked", s);
        //    }
        //}
        //审核
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("未选中行！");
                return;
            }
            else
            {
                string auditState = myGridView1.GetRowCellValue(rowHandle, "AuditState").ToString();
                string billState = myGridView1.GetRowCellValue(rowHandle, "BillState").ToString();
                string billNo1 = myGridView1.GetRowCellValue(rowHandle, "BillNo").ToString() ;
                if (auditState == "未审核" && billState == "0")
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("BillNo", billNo1));
                        list.Add(new SqlPara("Type", 3));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_GET_AUDITWAYBILL", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            MsgBox.ShowOK();
                            GetData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }

                }
                else
                {
                    MsgBox.ShowOK("只有新开未审核的单才能进行审核！");
                    return;
                }
                return;
            }
            
        }
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
            foreach (DataRow dr1 in SelectDt.Rows)
            {
                dr1["ischecked"] = 1;//把勾选的行对应值重新赋值为：1便于后面判断
            }
            if (SelectDt.Rows.Count == 0)
            {
                MsgBox.ShowOK("请选择要审核的数据!");
                return false;
            }
            return true;

        }
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("未选中行！");
                return;
            }
            try
            {
                string auditState = myGridView1.GetRowCellValue(rowHandle, "AuditState").ToString();
                string billNo = myGridView1.GetRowCellValue(rowHandle,"BillNo").ToString();
                if (auditState == "未审核")
                {
                    MsgBox.ShowOK("未审核的单不能取消审核！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("Type",0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_GET_AUDITWAYBILL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    GetData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        //批量审核
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.MoveNext();
            myGridView1.Focus();
            myGridView1.MoveLast();
            try
            {
            //    for (int i = 0; i < myGridView1.RowCount; i++)
            //    {
            //        string ischecked = myGridView1.GetRowCellValue(i, "ischecked").ToString();
            //        if (Convert.ToInt32(ischecked) == 1)//如果当前的一列是被勾选的
            //        {
            //            string auditState1 = myGridView1.GetRowCellValue(i, "AuditState").ToString();
            //            string billState1 = myGridView1.GetRowCellValue(i, "BillState").ToString();

            //            if (auditState1 != "未审核" && billState1 != "0")//新开并且未审核的单可以进行审核
            //            {
            //                MsgBox.ShowOK("只有新开未审核的单可以进行审核！请检查选中的行！");
            //                return;
            //            }
            //        }
                   
            //    }
                //批量审核
                DataTable dt = null;
                if (!CheckSelect(ref dt)) return;
                if (dt == null) return;
                //for(int i = 0; i < dt.Rows.Count; i++)
                //{
                //    //string ischecked = myGridView1.GetRowCellValue(i, "ischecked").ToString();
                //    //if (Convert.ToInt32(ischecked) == 1)//如果当前的一列是被勾选的
                //   // {
                //        //string auditState1 = myGridView1.GetRowCellValue(i, "AuditState").ToString();
                //        string billState1 = myGridView1.GetRowCellValue(i, "BillState").ToString();
                //        if (!(auditState1 == "未审核" && billState1 == "0"))//新开并且未审核的单可以进行审核
                //        {
                //            MsgBox.ShowOK("只有新开未审核的单可以进行审核！请检查选中的行！");
                //            return;
                //        }
                //    //}
                //}
                string billNo = "";
                foreach (DataRow r in dt.Rows)
                {
                    string billState1 = r["BillState"].ToString();
                    string auditState1 = r["AuditState"].ToString();
                    if (!(auditState1 == "未审核" && billState1 == "0"))//新开并且未审核的单可以进行审核
                    {
                        MsgBox.ShowOK("只有新开未审核的单可以进行审核！请检查选中的行！");
                         return;
                    }
                    billNo += r["BillNo"] + "@";
                }
                if (string.IsNullOrEmpty(billNo))
                {
                    MsgBox.ShowError("未找到需要执行的数据！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("Type", 1));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_GET_AUDITWAYBILL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    GetData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            int s = 0;
            if (checkBox1.Checked)
            { 
                s = 1;
            }
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", s);//勾选全选
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)      //自动筛选
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)      //锁定外观
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)     //取消锁定
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)     //过滤器
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

    }
}
