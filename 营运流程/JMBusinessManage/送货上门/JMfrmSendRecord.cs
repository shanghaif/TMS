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
    public partial class JMfrmSendRecord : BaseForm
    {
        public JMfrmSendRecord()
        {
            InitializeComponent();
        }
        private void getdata()
        {
            string proc = comboBoxEdit1.SelectedIndex == 0 ? "QSP_GET_SEND_DEPARTURE" : "QSP_GET_SEND_BILL";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", edbsite.Text.Trim() == "全部" ? "%%" : edbsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", edesite.Text.Trim() == "全部" ? "%%" : edesite.Text.Trim()));
                list.Add(new SqlPara("WebName", edwebid.Text.Trim() == "全部" ? "%%" : edwebid.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) (myGridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
            }

            PrePrintItems();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            myGridControl1.MainView = comboBoxEdit1.SelectedIndex == 0 ? myGridView2 : myGridView1; //0 按车  1 按票
            getdata();
        }

        private void PrePrintItems()
        {
            //toolStripButton1
            int have = 0;
            string sendinoneflag = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                sendinoneflag = ConvertType.ToString(myGridView1.GetRowCellValue(i, "SendBatch"));

                if (barSubItem3.ItemLinks.Count > 0)
                {
                    have = 0;
                    for (int j = 0; j < barSubItem3.ItemLinks.Count; j++)
                        if (barSubItem3.ItemLinks[j].Caption == sendinoneflag)
                        { have = 1; }
                }

                if (have == 0)
                {
                    DevExpress.XtraBars.BarButtonItem item = new DevExpress.XtraBars.BarButtonItem();
                    item.Caption = sendinoneflag;
                    item.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(item_ItemClick);
                    barSubItem3.ItemLinks.Add(item);
                }
            }
        }

        private void item_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            JMfrmSendLoad wsl = JMfrmSendLoad.Get_JMfrmSendLoad;
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
            CommonClass.ShowBillSearch(billno);
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
            JMfrmSendDetail ws = new JMfrmSendDetail();
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

        private void JMfrmSendRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetSite(edbsite, true);
            CommonClass.SetSite(edesite, true);
            CommonClass.SetWeb(edwebid, CommonClass.UserInfo.SiteName);

            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            FixColumn fix2 = new FixColumn(myGridView2, barSubItem4);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            edesite.Text = CommonClass.UserInfo.SiteName;
            edwebid.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();
        }

        private void edbsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(edwebid, edbsite.Text.Trim());
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem8.PerformClick();
        }
    }
}