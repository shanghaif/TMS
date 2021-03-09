using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KMS.Tool;
using KMS.SqlDAL;
using KMS.Common;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid;

namespace KMS.UI.核销界面
{
    public partial class frmOilCardDeposit : KMS.Tool.BaseForm
    {
        public frmOilCardDeposit()
        {
            InitializeComponent();
        }
        DataSet ds;
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {




                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string VerifState = myGridView1.GetRowCellValue(rowhandle, "OilCardDepositVerifState").ToString();
                string batch = myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
                decimal CurrentVerifyFee=Convert.ToDecimal(myGridView1.GetRowCellValue(rowhandle, "CurrentVerifyFee").ToString());
                //DataSet dsnew =new DataSet() ;
                //DataRow[] dr = ds.Tables[0].Select("DepartureBatch= '" + batch.Trim() + "' ");
                //if (dr != null && dr.Length > 0)
                //{
                //    DataTable  dt=ToDataTable(dr);
                //    dsnew.Tables.Add(dt);
                //}



                if (VerifState == "已核销")
                {
                    MsgBox.ShowError("该批次已审核，请选择未审核的批次！"); return;
                }
                else
                {
                    if (MsgBox.ShowYesNo("确定要审核该批次吗?") != DialogResult.Yes)
                    {
                        return;
                    }
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("DepartureBatch", batch.Trim()));
                    list.Add(new SqlPara("CurrentVerifyFee", CurrentVerifyFee));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_OilCardDeposit_AddVerify", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        GetData();
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        //private DataTable ToDataTable(DataRow[] rows)
        //{
        //    if (rows == null || rows.Length == 0) return null;
        //    DataTable tmp = rows[0].Table.Clone(); // 复制DataRow的表结构
        //    foreach (DataRow row in rows)
        //    {

        //        tmp.ImportRow(row); // 将DataRow添加到DataTable中
        //    }
        //    return tmp;
        //}

        private void frmOilCardDeposit_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("油卡押金核销");//xj/2019/5/29
            CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(myGridView1);
            //GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, Cause.Text, true);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);


            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            bsite.Text = CommonClass.UserInfo.SiteName;
            esite.Text = "全部";

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);

            GridOper.CreateStyleFormatCondition(myGridView1, "OilCardDepositVerifState", FormatConditionEnum.Equal, "已核销", Color.LightGreen);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OilCardDeposit_Verify", list);
                ds = SqlHelper.GetDataSet(sps);
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
        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }


        private void myGridView1_CustomDrawRowPreview(object sender, DevExpress.XtraGrid.Views.Base.RowObjectCustomDrawEventArgs e)
        {


        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;


                string verifyStatus = myGridView1.GetRowCellValue(rowhandle, "OilCardDepositVerifState").ToString();
                if (verifyStatus == "未核销")
                {
                    MsgBox.ShowError("该批次是未审核状态,不能反审核！");
                    return;
                }
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "VerifyOffAccountID").ToString());
                string  BatchNo=myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
                if (MsgBox.ShowYesNo("确定要反审核该凭证吗?") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VerifyOffAccountID", id));
                list.Add(new SqlPara("BatchNo", BatchNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_OilCardDeposit_UpdateVerify", list);
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
    }
}
