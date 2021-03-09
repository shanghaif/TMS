using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmScanStatistics : BaseForm
    {
        public frmScanStatistics()
        {
            InitializeComponent();
        }

        public int type = 2; //1：短驳卸车统计  2：配载装车统计
        public string dtvehicleno = "";
        public string dtchauffer = "";
        public DateTime dtsenddate = CommonClass.gcdate;
        public string dtinoneflag = "";

        private void frmScanStatistics_Load(object sender, EventArgs e)
        {
            //CommonClass.FormSet(this, false, true);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);

            label2.Text = dtinoneflag;
            label4.Text = dtsenddate.ToString("MM-dd HH:mm");
            label6.Text = dtvehicleno;
            label8.Text = dtchauffer;
            if (type == 1)
            {
                this.Text = "短驳卸车扫描统计";
            }
            if (type == 2)
            {
                this.Text = "分单配载装车扫描统计";
                label1.Text = "发车批次：";
                label3.Text = "发车日期：";
                label5.Text = "车牌号码：";
                label7.Text = "司机姓名：";
            }
            getdata();
        }

        private void getdata()
        {
            try
            {//分单配载扫描统计
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_DEPARTURE_SCAN_STATISTICS", new List<SqlPara> { new SqlPara("DepartureBatch", dtinoneflag) }));
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
    }
}