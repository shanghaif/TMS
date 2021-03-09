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
using KMS.Tool;


namespace KMS.UI
{
    public partial class w_扫描人统计 : BaseForm
    {
        public int opertype = 1;//1：短驳卸货  2配载扫描上车  3全部(暂时不用)

        public w_扫描人统计()
        {
            InitializeComponent();
        }
        
        private void Form2_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            cc.RestoreGridLayout(gridControl1, "按扫描人统计");
            //cc.BuildSelectSiteTree(tvbsite, tvesite);
            //this.popupContainerEdit1.Text = commonclass.gbsite;
            //this.popupContainerEdit2.Text = "全部";
            bdate.EditValue = commonclass.gbdate;
            edate.EditValue = commonclass.gedate;
            //设计冻结列
            fixcolumn fc = new fixcolumn(gridView1, barSubItem2);
            cc.LoadScheme(gridControl1);
            commonclass.SetBarPropertity(bar3);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //string bsite = popupContainerEdit1.Text == "全部" ? "%%" : popupContainerEdit1.Text;
            //string esite = popupContainerEdit2.Text == "全部" ? "%%" : popupContainerEdit2.Text;

            //if (!ur.checkviewright(bsite, esite, "ac3", "ac4")) return;

            string createby = edcreateby.Text.Trim() == "全部" ? "%%" : edcreateby.Text.Trim();

            SqlConnection connection = cc.GetConn();
            try
            {
                string proc = "";
                if (opertype == 1)
                {
                    proc = "短驳卸货按扫描人统计";
                }
                if (opertype == 2)
                {
                    proc = "分单配载扫描按扫描人统计";
                }
                SqlCommand sq = new SqlCommand(proc, connection);
                sq.CommandType = CommandType.StoredProcedure;

                sq.Parameters.Add(new SqlParameter("@t1", SqlDbType.DateTime));
                sq.Parameters.Add(new SqlParameter("@t2", SqlDbType.DateTime));
                sq.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar));
                sq.Parameters.Add("@companyid", SqlDbType.VarChar).Value = commonclass.companyid;
                
                gridView1.ClearColumnsFilter();
                sq.Parameters[0].Value = bdate.DateTime;
                sq.Parameters[1].Value = edate.DateTime;
                sq.Parameters[2].Value = createby;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sq;

                DataSet ds = new DataSet();
                da.Fill(ds);
                connection.Close();
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region
        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            popupContainerEdit1.Text = e.Node.Text;
        }

        private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        {

            popupContainerEdit2.Text = e.Node.Text;
        }

        private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        {
            popupContainerEdit1.ClosePopup();
        }

        private void tvesite_MouseClick(object sender, MouseEventArgs e)
        {
            popupContainerEdit2.ClosePopup();
        }
        #endregion

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //w_show_connect_detail ws = new w_show_connect_detail();
            //ws.dttosite = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dttosite").ToString();
            //ws.dtvehicleno = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtvehicleno").ToString();
            //ws.dtchauffer = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtchauffer").ToString();
            //ws.dtsenddate = Convert.ToDateTime(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtsenddate"));
            //ws.dtmadeby = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtmadeby").ToString();
            //ws.dtinoneflag = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtinoneflag") == DBNull.Value ? "" : gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "dtinoneflag").ToString();
            //ws.ShowDialog();
            cc.ShowBillDetail(gridView1);
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            cc.AllowAutoFilter(gridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            cc.SaveGridLayout(gridControl1, "按扫描人统计", true);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            cc.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            cc.DeleteGridLayout(gridControl1, "按扫描人统计");
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            w_package_connect wpc = new w_package_connect();
            wpc.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            cc.ExportToExcel(gridView1);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            cc.QuickSearch();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            cc.GenSeq(e);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            SqlConnection Conn = cc.GetConn();
            try
            {
                SqlCommand Cmd = new SqlCommand("装卸货货物扫描人", Conn);
                Cmd.CommandType = CommandType.StoredProcedure;

                Cmd.Parameters.Add("@opertype", SqlDbType.Int).Value = opertype;
                Cmd.Parameters.Add("@companyid", SqlDbType.VarChar).Value = commonclass.companyid;

                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
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
                commonclass.MsgBox.ShowOKError(obj[1].ToString());
                return;
            }
            DataSet ds = obj[1] as DataSet;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                edcreateby.Properties.Items.Add(ds.Tables[0].Rows[i]["createby"]);
            }
            edcreateby.Properties.Items.Add("全部");
            edcreateby.Text = "全部";
        }
    }
}