using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Threading;


namespace ZQTMS.UI
{
    public partial class frmNotCusData : BaseForm
    {
        public frmNotCusData()
        {
            InitializeComponent();
        }

        private void frmNotCusData_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("最近未发货客户");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.SetSite(Site, true);
            Site.Text = CommonClass.UserInfo.SiteName;
        
        }
        
        
        //提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try {
              
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Site", Site.Text.Trim() == "全部" ? "%%" : Site.Text.Trim()));
                list.Add(new SqlPara("Type", Type.Text.Trim() == "全部" ? "%%" : Type.Text.Trim()));
                list.Add(new SqlPara("Days", Days.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_NotCusData", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
              GridOper.ExportToExcel(myGridView1);
        }
        }
    
    
    }

