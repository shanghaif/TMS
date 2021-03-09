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
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfrmScanBillMan : BaseForm
    {
        public int opertype = 2;//1：短驳卸货  2配载扫描上车  3全部(暂时不用)
        static JMfrmScanBillMan fsb;

        public JMfrmScanBillMan()
        {
            InitializeComponent();
        }

        public static JMfrmScanBillMan Get_frmScanBillMan { get { if (fsb == null || fsb.IsDisposed) fsb = new JMfrmScanBillMan(); return fsb; } }

        private void Form2_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            backgroundWorker1.RunWorkerAsync();

            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string createby = edcreateby.Text.Trim() == "全部" ? "%%" : edcreateby.Text.Trim();

            try
            {
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_SCAN_MAN_STATISTICS", new List<SqlPara> { new SqlPara("t1", bdate.DateTime), new SqlPara("t2", edate.DateTime), new SqlPara("createby", createby) }));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //获取装卸扫描人
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_UPDOWN_SCAN_MAN", new List<SqlPara> { new SqlPara("opertype", 1) }));
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
                MsgBox.ShowError(obj[1].ToString());
                return;
            }
            DataSet ds = obj[1] as DataSet;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                edcreateby.Properties.Items.Add(ds.Tables[0].Rows[i]["upman"]);
            }
            edcreateby.Properties.Items.Add("全部");
            edcreateby.Text = "全部";
        }
    }
}