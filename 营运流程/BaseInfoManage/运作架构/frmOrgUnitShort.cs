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

namespace ZQTMS.UI.BaseInfoManage.运作架构
{
    public partial class frmOrgUnitShort : BaseForm
    {
        public frmOrgUnitShort()
        {
            InitializeComponent();
        }
        bool flag = false;
        //加载
        private void frmOrgUnitShort_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            flag = UserRight.GetRight("161125");

            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar5); //如果有具体的工具条，就引用其实例
            //barButtonItem39.Enabled = false;
            getMiddleSite();
            //barButtonItem23.Enabled = true;

            //if (CommonClass.UserInfo.companyid != "101")
            //{

            //    xtraTabPage1.PageVisible = false;
            //    // xtraTabPage3.PageVisible = false;
            //}
        }

        //查询
        private void getMiddleSite()
        {
            //bar5.Visible = false;
            panel1.Visible = true;
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMIDDLESITESHORT", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl1.DataSource = ds.Tables[0];
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
                            bar5.Visible = true;
                            panel1.Visible = false;
                            if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();
        }

        //新增
        private void btn_Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOrgUnitShort_Manage fmsu = new frmOrgUnitShort_Manage();
            fmsu.gv = myGridView1;
            fmsu.ShowDialog();
        }

        //修改
        private void btn_Upt_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel1.Visible)
            {
                MsgBox.ShowOK("正在加载数据...\r\n请稍后再试");
                return;
            }
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            frmOrgUnitShort_Manage fmsu = new frmOrgUnitShort_Manage();
            fmsu.Id = GridOper.GetRowCellValueString(myGridView1, rowhandle, "MiddleSiteId");
            fmsu.gv = myGridView1;
            fmsu.ShowDialog();
        }

        //删除
        private void btn_Del_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "MiddleSiteId").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleSiteId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASMIDDLESITESHORT", list);
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

        //过滤
        private void btn_GuoLv_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        //刷新
        private void btn_Refresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getMiddleSite();
        }

        //导入
        private void btn_Import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        //导出
        private void btn_Export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短线路由");
        }

        //关闭
        private void btn_Close_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
