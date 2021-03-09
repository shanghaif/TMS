using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmCauseBank : BaseForm
    {
        public frmCauseBank()
        {
            InitializeComponent();
        }
        DataSet dsshipper = new DataSet();//汇款客户资料
        private void frmCauseBank_Load(object sender, EventArgs e)
        {
            //CommonClass.(gridshow, "银行信息平台");
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;

            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar5);
            GridOper.RestoreGridLayout(gridView1, "银行信息平台");
            backgroundWorker1.RunWorkerAsync();//汇款客户资料
        }


        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "rowid")
                e.Value = (object)(e.RowHandle + 1);
        }

        int times = 0; //汇款客户资料取3次
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CAUSEBANK_CUSTOMER");
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                gridshow.MainView = gridView2;
            }
            else
            {
                gridshow.MainView = gridView1;
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("appstate", 0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANK_HK_Cause", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                gridshow.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem8_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "银行信息平台");
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("银行信息平台");
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCauseBankAdd wv = new frmCauseBankAdd();
            wv.dsshipper = dsshipper;
            wv.Show();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string singName = gridView1.GetRowCellValue(rowhandle, "createby").ToString();//登记人
            if (!string.IsNullOrEmpty(singName))
            {
                if (singName == CommonClass.UserInfo.UserName)
                {
                    string appby = gridView1.GetRowCellValue(rowhandle, "appby") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "appby").ToString();
                    if (appby != "")
                    {
                        MsgBox.ShowOK("当前记录已经审核，无法修改!");
                        return;
                    }

                    int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));
                    frmCauseBankAdd wv = new frmCauseBankAdd();
                    wv.id = id;
                    wv.Text = "银行付款信息编辑";
                    wv.ShowDialog();
                }
                else
                {
                    MsgBox.ShowYesNo("对不起，非当前用户不能操作！");
                }
            }
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            string appby = gridView1.GetRowCellValue(rowhandle, "appby") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "appby").ToString();
            if (appby != "")
            {
                MsgBox.ShowOK("当前记录已经审核，无法修改!");
                return;
            }
            string signName = gridView1.GetRowCellValue(rowhandle, "createby").ToString();//登记人
            if (!string.IsNullOrEmpty(signName))
            {
                if (signName == CommonClass.UserInfo.UserName)
                {
                    if (MsgBox.ShowYesNo("确定要删除当前记录？\r\n请注意：系统会记录操作日志!") == DialogResult.No) return;

                    int id = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "id"));

                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("id", id));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "DELETE_CAUSEBANK_HK", list);
                        if (SqlHelper.ExecteNonQuery(sps) == 1)
                        {
                            gridView1.DeleteRow(rowhandle);
                            MsgBox.ShowOK();
                        }
                        else
                            MsgBox.ShowError("操作失败！");
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowError(ex.Message);
                    }
                }
                else
                {
                    MsgBox.ShowYesNo("对不起，非当前用户不能操作！");
                }
            }
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int[] rows = gridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("没有选择");
                return;
            }

            if (MsgBox.ShowYesNo("确认审核选中的记录？已审核过将不再审核") != DialogResult.Yes) return;


            try
            {

                for (int i = 0; i < rows.Length; i++)
                {
                    string appby = gridView1.GetRowCellValue(rows[i], "appby").ToString();
                    if (appby == "")
                    {

                        int id = Convert.ToInt32(gridView1.GetRowCellValue(rows[i], "id"));
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("appby", CommonClass.UserInfo.UserName));
                        list.Add(new SqlPara("appdate", CommonClass.gcdate));
                        list.Add(new SqlPara("id", id));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "UPD_CAUSEBANK_HK_ByID", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            gridView1.SetRowCellValue(rows[i], "appby", CommonClass.UserInfo.UserName);
                            gridView1.SetRowCellValue(rows[i], "appdate", CommonClass.gcdate);
                            gridView1.SetRowCellValue(rows[i], "appstate", "已审核");
                            MsgBox.ShowOK();
                        }
                        else
                        {
                            MsgBox.ShowError("操作失败！");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)gridshow.MainView);
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}