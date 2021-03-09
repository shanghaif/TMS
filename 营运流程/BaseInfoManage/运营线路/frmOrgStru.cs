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
using System.Threading;

namespace ZQTMS.UI
{
    public partial class frmOrgStru : BaseForm
    {
        public frmOrgStru()
        {
            InitializeComponent();
        }

        private void frmAdmOrgStru_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("行政规划");//xj2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
            CommonClass.GetGridViewColumns(myGridView3);
            CommonClass.GetGridViewColumns(myGridView4);
            CommonClass.GetGridViewColumns(myGridView5);
            CommonClass.GetGridViewColumns(myGridView6);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6);
            BarMagagerOper.SetBarPropertity(bar11); //如果有具体的工具条，就引用其实例
            BarMagagerOper.SetBarPropertity(bar12);
            BarMagagerOper.SetBarPropertity(bar13);
            BarMagagerOper.SetBarPropertity(bar14);
            BarMagagerOper.SetBarPropertity(bar15);
            BarMagagerOper.SetBarPropertity(bar16);
            freshCause();
        }

        #region 大区
        private void barBtnAddArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddArea frm = new frmAddArea();
            frm.ShowDialog();
            freshArea();
        }

        private void barBtnDelArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView2.GetRowCellValue(rowhandle, "AreaID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AreaID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASAREA", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView2.DeleteRow(rowhandle);
                    myGridView2.PostEditor();
                    myGridView2.UpdateCurrentRow();
                    myGridView2.UpdateSummary();
                    DataTable dt = myGridControl2.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnModArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView2.GetRowCellValue(rowhandle, "AreaID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AreaID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmAddArea frm = new frmAddArea();
                frm.dr = dr;
                frm.ShowDialog();
                freshArea();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnFreshArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshArea();
        }

        private void barBtnAreaExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshArea()
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

               if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        #endregion

        #region 大事业部
        private void barBtnCauseAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddCause frm = new frmAddCause();
            frm.ShowDialog();
            freshCause();
        }
        private void barBtnCauseMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "CauseID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CauseID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmAddCause frm = new frmAddCause();
                frm.dr = dr;
                frm.ShowDialog();
                freshCause();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void barBtnCauseDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "CauseID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CauseID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASCAUSE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnCauseFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshCause();
        }

        private void barBtnCauseExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshCause()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCAUSE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                               if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        #endregion

        #region 部门
        private void barBtnDepartAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddPart frm = new frmAddPart();
            frm.ShowDialog();
            freshDepart();
        }

        private void barBtnDepartMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView3.GetRowCellValue(rowhandle, "DepId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddPart frm = new frmAddPart();
                frm.dr = dr;
                frm.ShowDialog();
                freshDepart();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnDepartDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView3.GetRowCellValue(rowhandle, "DepId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASDEPART", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView3.DeleteRow(rowhandle);
                    myGridView3.PostEditor();
                    myGridView3.UpdateCurrentRow();
                    myGridView3.UpdateSummary();
                    DataTable dt = myGridControl3.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnDepartFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshDepart();
        }

        private void barBtnDepartExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void freshDepart()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                               if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        #endregion

        #region 岗位
        private void barBtnJobAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddJob frm = new frmAddJob();
            frm.ShowDialog();
            freshJob();
        }

        private void barBtnJobMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView4.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView4.GetRowCellValue(rowhandle, "JobId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("JobId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASJOBS_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddJob frm = new frmAddJob();
                frm.dr = dr;
                frm.ShowDialog();
                freshJob();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnJobDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView4.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView4.GetRowCellValue(rowhandle, "JobId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("JobId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASJOBS", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView4.DeleteRow(rowhandle);
                    myGridView4.PostEditor();
                    myGridView4.UpdateCurrentRow();
                    myGridView4.UpdateSummary();
                    DataTable dt = myGridControl4.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnJobFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView4);
        }

        private void barBtnJobFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshJob();
        }

        private void barBtnJobExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView4, "岗位");
        }

        private void barBtnJobExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void freshJob()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASJOBS", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                               if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl4.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        #endregion

        #region 职称
        private void barBtnTitAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddTitle frm = new frmAddTitle();
            frm.ShowDialog();
            freshTitle();
        }

        private void barBtnTitMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView5.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView5.GetRowCellValue(rowhandle, "TitId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TitId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASTITLE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddTitle frm = new frmAddTitle();
                frm.dr = dr;
                frm.ShowDialog();
                freshTitle();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnTitDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView5.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView5.GetRowCellValue(rowhandle, "TitId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TitId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASTITLE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView5.DeleteRow(rowhandle);
                    myGridView5.PostEditor();
                    myGridView5.UpdateCurrentRow();
                    myGridView5.UpdateSummary();
                    DataTable dt = myGridControl5.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnTitFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView5);
        }

        private void barBtnTitFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshTitle();
        }

        private void barBtnTitExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView5, "职称");
        }

        private void barBtnTitExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void freshTitle()
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASTITLE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                               if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl5.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        #endregion

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            switch (xtraTabControl1.SelectedTabPageIndex)
            {
                case 0: freshCause();
                    break;
                case 1: freshArea();
                    break;
                case 2: freshDepart();
                    break;
                case 3: freshJob();
                    break;
                case 4: freshTitle();
                    break;
                case 5: freshEmployee();
                    break;
                default: break;
            }
        }

        #region 员工基础资料

        private void barBtnUserAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddEmployee frm = new frmAddEmployee();
            frm.ShowDialog();
            freshEmployee();
        }

        private void barBtnUserMod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView6.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView6.GetRowCellValue(rowhandle, "EmpID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("EmpID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEMPLOYEE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];
                frmAddEmployee frm = new frmAddEmployee();
                frm.dr = dr;
                frm.ShowDialog();
                freshEmployee();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnUserDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView6.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView6.GetRowCellValue(rowhandle, "EmpID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("EmpID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASEMPLOYEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView6.DeleteRow(rowhandle);
                    myGridView6.PostEditor();
                    myGridView6.UpdateCurrentRow();
                    myGridView6.UpdateSummary();
                    DataTable dt = myGridControl6.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnUserFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView6);
        }

        private void barBtnUserFresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshEmployee();
        }

        private void barBtnUserExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView6, "员工资料");
        }

        private void barBtnUserExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void freshEmployee()
        {
            bar16.Visible = false;
            panel1.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEMPLOYEE", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                                   if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl6.DataSource = ds.Tables[0];
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            bar16.Visible = true;
                            panel1.Visible = false;
                            if (myGridView6.RowCount < 1000) myGridView6.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }
        #endregion

        private void barBtnCauseFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barBtnCauseExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "事业部");
        }

        private void barBtnAreaFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barBtnExportArea_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "大区");
        }

        private void barBtnDepartFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barBtnDepartExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "部门");
        }
    }
}