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
using System.Reflection;
namespace ZQTMS.UI
{
    public partial class frmDrafts : BaseForm
    {
        public frmDrafts()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            myGridView1.ClearColumnsFilter();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));

                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                list.Add(new SqlPara("BegWeb", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Find_Drafts", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
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

        private void frmDrafts_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(StartSite, true);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetWeb(BegWeb, StartSite.Text);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);

            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = "全部";
            BegWeb.Text = CommonClass.UserInfo.WebName;
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();

            GridOper.CreateStyleFormatCondition(myGridView1, "BillState", DevExpress.XtraGrid.FormatConditionEnum.GreaterOrEqual, 5, Color.FromArgb(128, 255, 128));
            GridOper.CreateStyleFormatCondition(myGridView1, "BillState", DevExpress.XtraGrid.FormatConditionEnum.Equal, 100, Color.Red);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        ///<summary>
        /// 筛选条件框的双击事件
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string billno = GridOper.GetRowCellValueString(myGridView1, "BillNo");
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_BY_BILLNO_PRINTLABEL_DEV_Draft", new List<SqlPara> { new SqlPara("BillNo", billno) }));

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有找到要打印的运单,请稍后再试");
                return;
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmPrintLabelDev");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type, ds.Tables[0]);
            if (frm == null) return;
            frm.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;

            string _sgyid = GridOper.GetRowCellValueString(myGridView1, row, "BillNo");//运单号
            string _sgyname = GridOper.GetRowCellValueString(myGridView1, row, "Varieties");//品名
            string _sgongyingname = GridOper.GetRowCellValueString(myGridView1, row, "Package");//包装
            string _feevolume = GridOper.GetRowCellValueString(myGridView1, row, "FeeVolume");//计费体积
            string _feeweight = GridOper.GetRowCellValueString(myGridView1, row, "FeeWeight");//计费重量
            string _num = GridOper.GetRowCellValueString(myGridView1, row, "Num");//件数
            string _volume = GridOper.GetRowCellValueString(myGridView1, row, "Volume");//体积
            string _weight = GridOper.GetRowCellValueString(myGridView1, row, "Weight");//重量
            frmDrafsInfo fdi = new frmDrafsInfo(_sgyid, _sgyname, _sgongyingname, _num, _feevolume, _feeweight, _volume, _weight);
            fdi.Show();
        }
    }
}