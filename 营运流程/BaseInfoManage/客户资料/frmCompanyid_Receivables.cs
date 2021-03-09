using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress;
using KMS.Tool;
using KMS.Common;
using KMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraGrid;

namespace KMS.UI.BaseInfoManage.客户资料
{
    public partial class frmCompanyid_Receivables : KMS.Tool.BaseForm
    {
        public frmCompanyid_Receivables()
        {
            InitializeComponent();
        }
        DateTime dt;
        private void btnRetrieve_Click(object sender, EventArgs e)
        {

            this.label1.Text = bdate.DateTime.Month.ToString() + "月";
            freshData();
        
        }

        private void freshData()
        {
            try
            {
                dt = ToDateTime(Convert.ToDateTime(bdate.EditValue)).AddDays(-1);
               
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("edate", bdate.EditValue));
                list.Add(new SqlPara("adate", dt));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CompanyidAccounts_Receivabled", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        private void frmCompanyid_Receivables_Load(object sender, EventArgs e)
        {

     
            bdate.DateTime = CommonClass.gbdate.AddDays(+1).AddHours(-24);
            this.label1.Text = bdate.DateTime.Month.ToString()+"月";
         

        }
        public  DateTime ToDateTime (DateTime  gedate)
        {
            return gedate.AddHours(23).AddMinutes(59).AddSeconds(59);
            
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView1);
        }
    }
}
