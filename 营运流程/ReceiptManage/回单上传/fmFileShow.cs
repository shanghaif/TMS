using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class fmFileShow : BaseForm
    {
        public fmFileShow()
        {
            InitializeComponent();
        }
        public string billNo;
        public int billType;
        private void fmFileShow_Load(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("BillType", billType));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TBFILEINFO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowImg(myGridView1);
        }
    }
}