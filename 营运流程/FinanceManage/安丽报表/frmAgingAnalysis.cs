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
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmAgingAnalysis : BaseForm
    {
        public frmAgingAnalysis()
        {
            InitializeComponent();
        }

        private void frmAuditWayBill_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("时效分析表");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar5); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;


            CommonClass.SetWeb(BegWeb, true);
            CommonClass.SetWeb(PickGoodsSite, true);

            BegWeb.Text = CommonClass.UserInfo.WebName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();

            
        }


        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            myGridView1.ClearColumnsFilter();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("DateType", BillDate.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                list.Add(new SqlPara("PickGoodsSite", (PickGoodsSite.Text.Trim() == "全部" || PickGoodsSite.Text.Trim() == "") ? "%%" : PickGoodsSite.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AgingAnalysis", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }
 
     

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}
