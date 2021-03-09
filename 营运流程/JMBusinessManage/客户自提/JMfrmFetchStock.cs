using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class JMfrmFetchStock : DevExpress.XtraEditors.XtraForm
    {
        public JMfrmFetchStock()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            //提取数据
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLSIGN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                
                if(ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void JMfrmFetchStock_Load(object sender, EventArgs e)
        {
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }
    }
}