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
using System.Reflection;
namespace ZQTMS.UI.财务统计
{
    public partial class SingalBillnoGrossProfit : ZQTMS.Tool.BaseForm
    {
        public SingalBillnoGrossProfit()
        {
            InitializeComponent();
        }

        private void cbRetrieve_1_Click(object sender, EventArgs e)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate_1.DateTime));
                list.Add(new SqlPara("t2", edate_1.DateTime));

                list.Add(new SqlPara("CauseName", CauseName_1.Text.Trim() == "全部" ? "%%" : CauseName_1.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName_1.Text.Trim() == "全部" ? "%%" : AreaName_1.Text.Trim()));

                list.Add(new SqlPara("StartSite", StartSite_1.Text.Trim() == "全部" ? "%%" : StartSite_1.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite_1.Text.Trim() == "全部" ? "%%" : TransferSite_1.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb_1.Text.Trim() == "全部" ? "%%" : BegWeb_1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELISTBYDEPARTUREBATCH_new_2", list);
                myGridControl2.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                // if (myGridView1.RowCount < 1000)  myGridView1.BestFitColumns();
                //GridOper.RestoreGridLayout(myGridView1); //zb20190429
            }

        }

        private void SingalBillnoGrossProfit_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            //CommonClass.GetGridViewColumns(bandedGridView2);
            BarMagagerOper.SetBarPropertity(bar9);
            bdate_1.DateTime = CommonClass.gbdate.AddDays(+1).AddHours(-24);
            edate_1.DateTime = CommonClass.gedate.AddDays(+1).AddHours(-24);

            CommonClass.SetSite(StartSite_1, true);
            CommonClass.SetSite(TransferSite_1, true);
            CommonClass.SetWeb(BegWeb_1, StartSite_1.Text);
            CommonClass.SetCause(CauseName_1, true);
            CommonClass.SetArea(AreaName_1, CauseName_1.Text);


            CauseName_1.Text = CommonClass.UserInfo.CauseName;
            AreaName_1.Text = CommonClass.UserInfo.AreaName;
            StartSite_1.Text = CommonClass.UserInfo.SiteName;
            TransferSite_1.Text = "全部";
            BegWeb_1.Text = CommonClass.UserInfo.WebName;
            CommonClass.GetServerDate();
        }

        private void CauseName_1_SelectedIndexChanged(object sender, EventArgs e)
        {

            CommonClass.SetArea(AreaName_1, CauseName_1.Text);
            CommonClass.SetCauseWeb(BegWeb_1, CauseName_1.Text, AreaName_1.Text);
        }

        private void AreaName_1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb_1, CauseName_1.Text, AreaName_1.Text);
        }

        private void myGridControl2_DoubleClick(object sender, EventArgs e)
        {
            
            int rows = bandedGridView2.FocusedRowHandle;
            string a = bandedGridView2.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Tag = a;
            frm.ShowDialog();
        
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            
            MsgBox.ShowOK("单票毛利=实收运费-单票接货费-单票短驳费-单票大车费-始发其他费-终端其他费-单票实际送货费-中转费");
        
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView2, "单票毛利");
        }
    }
}
