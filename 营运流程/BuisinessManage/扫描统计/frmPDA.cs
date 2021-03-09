using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmPDA : BaseForm
    {
        public frmPDA()
        {
            InitializeComponent();
        }

        public int type = 1; //1：短驳卸车统计  2：配载装车统计
        public string dtvehicleno = "";
        public string dtchauffer = "";
        public DateTime dtsenddate = CommonClass.gcdate;
        public string dtinoneflag = "";

        private void frmPDA_Load(object sender, EventArgs e)
        {
            GridOper.AddGridViewID(gridView1);
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
            {
                string proc = "";
                List<SqlPara> list = new List<SqlPara>();
                if (type == 1)
                {
                    proc = "短驳卸货扫描统计";
                    list.Add(new SqlPara("dtinoneflag", dtinoneflag));
                }
                if (type == 2)
                {
                    proc = "分单配载扫描统计";
                    list.Add(new SqlPara("inonevehicleflag", dtinoneflag));
                }

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
    }
}
