using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class frmBackAduitList : BaseForm
    {
        public frmBackAduitList()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("webname", WebName.Text.Trim()));
                list.Add(new SqlPara("UserName", UserName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_BACKADUITLIST", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) (myGridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
            }
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBackAduit wsl = new frmBackAduit();
            wsl.MdiParent = this.MdiParent;
            wsl.Show();
            wsl.Focus();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridControl1.MainView as MyGridView);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MyGridView gv = myGridControl1.MainView as MyGridView;
            if (gv == null || gv.FocusedRowHandle < 0) return;

            string billno = ConvertType.ToString(gv.GetFocusedRowCellValue("BillNo"));
            frmBillSearch.ShowBillSearch(billno);
        }

        private void 安排送货ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            barButtonItem3.PerformClick();
        }

        private void gridView2_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //送货调整
            MyGridView gv = (MyGridView)myGridControl1.MainView;
            if (gv.FocusedRowHandle < 0) return;
            frmSendDetail ws = new frmSendDetail();
            ws.gv = gv;
            ws.ShowDialog();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DevExpress.XtraGrid.Views.Grid.GridView gv = (DevExpress.XtraGrid.Views.Grid.GridView)gridControl1.MainView;
            //if (gv.FocusedRowHandle < 0) return;
            //string sendvehicleno = gv.GetRowCellValue(gv.FocusedRowHandle, "sendvehicleno") == DBNull.Value ? "" : gv.GetRowCellValue(gv.FocusedRowHandle, "sendvehicleno").ToString();
            //if (sendvehicleno == "")
            //{
            //    commonclass.MsgBox.ShowOK("没有取得车号,无法定位!");
            //    return;
            //}
            //WebServiceHelper.GetPos(sendvehicleno);
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            //foreach (Form form in this.MdiParent.MdiChildren)
            //{
            //    if (form.GetType() == typeof(w_md_accept_record_cancel))
            //    {
            //        form.Focus();
            //        return;
            //    }
            //}

            //w_md_accept_record_cancel fm = new w_md_accept_record_cancel();
            //fm.MdiParent = this.MdiParent;
            //fm.Dock = DockStyle.Fill;
            //fm.Show();
        }

        private void frmSendRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("反审核记录");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetWeb(WebName, "全部", true);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2); ;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            UserName.Text = CommonClass.UserInfo.SiteName;
            WebName.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();
        }

        private void edbsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetWeb(WebName, edbsite.Text.Trim());
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem8.PerformClick();
        }

        private void WebName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetUser(UserName, WebName.Text.Trim(), true);
        }
    }
}