using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmFinancialDetail : BaseForm
    {
        public frmFinancialDetail()
        {
            InitializeComponent();
        }

        public string DeclareBatch = "";
        public string FeeType = "";
        public string CarNO = "";
        public string SerialNumber = "";

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MsgBox.ShowOK("已确认显示绿色，未审核显示白色");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, "");
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("");
        }

        private void barCheckItem4_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //加载
        private void frmFinancialDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            //设置网格
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);

            getData();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
      

        public void getData()
        {
            try
            {
                myGridView1.ClearColumnsFilter();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeclareBatch", DeclareBatch));
                list.Add(new SqlPara("FeeType", FeeType));
                list.Add(new SqlPara("CarNO", CarNO));
                list.Add(new SqlPara("SerialNumber", SerialNumber));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_basfinancialAudit_DeclareBatch2", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

      

       
        //导出
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)myGridControl1.MainView);
        }
        
    }
}