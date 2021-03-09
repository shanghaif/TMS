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
using Newtonsoft.Json;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace ZQTMS.UI
{
    public partial class frmBillAccount : BaseForm
    {
        public frmBillAccount()
        {
            InitializeComponent();
        }

        private void frmAccMiddlePay_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("财务日记账");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1,false);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetCause(Cause, true);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.CreateStyleFormatCondition(myGridView1, "VerifyStatus", DevExpress.XtraGrid.FormatConditionEnum.Equal, "取消", Color.Red);
            web.Text = CommonClass.UserInfo.WebName;

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, this.Text);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmItemsInput f = new frmItemsInput("");
            f.type = "新增";
            f.Show();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("WebName", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("Voucher", comboBoxEdit1.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_cs", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
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

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("WebName", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_Count", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                decimal a = ds.Tables[0].Rows[0][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                decimal b = ds.Tables[0].Rows[1][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[1][0]);
                //decimal b = 0;
                edaccyestoday.Text = Math.Round(a, 2).ToString();
                //edacctoday.Text = Math.Round(b, 2).ToString();
                //if (ds.Tables.Count > )
                //{
                //    if (ds.Tables[1].Rows.Count > 0)
                //    {
                //        b = ConvertType.ToDecimal(ds.Tables[1].Rows[0]["acc"]);
                //    }
                //}
                edacctoday.Text = Math.Round(b, 2).ToString();

                decimal accyestoday = 0;
                if (edaccyestoday.Text.Trim() != "")
                {
                    accyestoday = Convert.ToDecimal(edaccyestoday.Text.Trim());
                }

                decimal acctoday = 0;
                if (edacctoday.Text.Trim() != "")
                {
                    acctoday = Convert.ToDecimal(edacctoday.Text.Trim());
                }
                edaccnow.Text = Math.Round(accyestoday + acctoday, 2).ToString();
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

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text, true);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            //hj20180711
            string BillType = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillType");
            if (BillType == "油卡"&&(Common.CommonClass.UserInfo.companyid == "155"||Common.CommonClass.UserInfo.companyid == "108"))
            {
                MsgBox.ShowError("不能修改油卡金额!");
                return;
            }
            frmItemsInput f = new frmItemsInput(GridOper.GetRowCellValueString(myGridView1, rowhandle, "VerifyOffAccountID"));
            f.ComeFrom = GridOper.GetRowCellValueString(myGridView1, rowhandle, "ComeFrom");//手工录入的凭证号可以修改核销日期gy20190731
            f.ShowDialog();
        }

        private void SelectAll(object sender, EventArgs e)
        {
            try { (sender as ComboBoxEdit).SelectAll(); }
            catch { }
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem8.PerformClick();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmVerifyDirection frm = new FrmVerifyDirection();
            frm.ShowDialog();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            Guid ID = new Guid(myGridView1.GetRowCellValue(rowhandle, "VerifyOffAccountID").ToString());
            int state = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "registrationState"));
            if (MsgBox.ShowYesNo("确定删除吗？") != DialogResult.Yes) return;
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLACCOUNT", new List<SqlPara> { new SqlPara("VerifyOffAccountID", ID), new SqlPara("registrationState", state) });
            if (SqlHelper.ExecteNonQuery(sps)>0)
            {
                MsgBox.ShowOK();
                cbRetrieve_Click(sender,e);
            }
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridView gv = new GridView();
            gv = myGridControl1.MainView as GridView;

            if (gv == null || gv.FocusedRowHandle < 0) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
            list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
            list.Add(new SqlPara("WebName", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));   

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_PRINT",list));
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

            decimal acc = ConvertType.ToDecimal(ds.Tables[1].Rows[0]["acc"]);
            decimal acc1 = ConvertType.ToDecimal(ds.Tables[1].Rows[1]["acc1"]);

            decimal acc2 = acc + acc1;

            //for (int i = 0; i < ds.Tables[1].Rows.Count;i++ )
            //{
            //    if (ConvertType.ToDecimal(ds.Tables[1].Rows[i]["acc"]) != 0)
            //    { 
            //     acc=ConvertType.ToDecimal(ds.Tables[1].Rows[i]["acc"]);
            //    }
            //    if (ConvertType.ToDecimal(ds.Tables[1].Rows[i]["acc1"]) != 0)
            //    {
            //         acc1 = ConvertType.ToDecimal(ds.Tables[1].Rows[i]["acc1"]);
            //    }

            //}
           
             dt1 = ds.Tables[0];
             //dt2 = ds.Tables[1];

             dt2.Columns.Add("acc",typeof(decimal));
             dt2.Columns.Add("acc1", typeof(decimal));
             dt2.Columns.Add("acc2", typeof(decimal));
             dt1.Merge(dt2);
             
             for (int i = 0; i < dt1.Rows.Count; i++)
             {
                 //dt2.Rows.Add(moneyacc = acc, acc1, acc2);
                 dt1.Rows[i]["acc"] = acc;
                 dt1.Rows[i]["acc1"] = acc1;
                 dt1.Rows[i]["acc2"] = acc2;
             }                
            ds = dt1.DataSet;
            string Accountlisting = "手工日记账清单";
            if (File.Exists(Application.StartupPath + "\\Reports\\" + Accountlisting + "per.grf"))//zaj 20180713保存外观的文件
            {
                Accountlisting = Accountlisting + "per";
            }
            frmPrintRuiLang fpr = new frmPrintRuiLang(Accountlisting, ds, CommonClass.UserInfo.gsqc);
            fpr.ShowDialog();
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;
            string VoucherNo = GridOper.GetRowCellValueString(myGridView1, row, "VoucherNo");
            string VerifyOffType = GridOper.GetRowCellValueString(myGridView1, row, "VerifyOffType");
            if (comboBoxEdit1.Text.Trim() == "业务明细") return;
            frmAccountingAduit_List frm = new frmAccountingAduit_List();
            frm.VoucherNo = VoucherNo;
            frm.VerifyOffType = VerifyOffType;
            frm.ShowDialog();
        }



    }
}