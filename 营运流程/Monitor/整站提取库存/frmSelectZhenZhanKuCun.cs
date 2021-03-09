using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmSelectZhenZhanKuCun : BaseForm
    {
        public frmSelectZhenZhanKuCun()
        {
            InitializeComponent();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "整站库存");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            if (StartSite.Text == "全部" && TransferSite.Text == "全部")
            {
                MsgBox.ShowOK("始发站和中转站不可同时选择全部！");
                return;
            }
            myGridView1.ClearColumnsFilter();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadType", 4));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOADPRE_hefei", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                //hj20180423 提取ZQTMS整站库存
                //List<SqlPara> list_ZQTMS = new List<SqlPara>();
                //list_ZQTMS.Add(new SqlPara("bdate", bdate.DateTime));
                //list_ZQTMS.Add(new SqlPara("edate", edate.DateTime));
                //list_ZQTMS.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                //list_ZQTMS.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                //list_ZQTMS.Add(new SqlPara("LoadType", 4));
                //list_ZQTMS.Add(new SqlPara("StartSite", StartSite.Text.Trim() == "全部" ? "%%" : StartSite.Text.Trim()));
                //list_ZQTMS.Add(new SqlPara("TransferSite", TransferSite.Text.Trim() == "全部" ? "%%" : TransferSite.Text.Trim()));
                //SqlParasEntity sps_ZQTMS = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOADPRE_hefei_ZQTMS", list_ZQTMS);
                //DataSet ds_ZQTMS = SqlHelper.GetDataSet_ZQTMS(sps_ZQTMS);

                //if (ds == null || ds.Tables.Count == 0 || ds_ZQTMS == null || ds_ZQTMS.Tables.Count==0) return;
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                //myGridControl1.DataSource = ds.Tables[0];
                //myGridControl1.DataSource = ds_ZQTMS.Tables[0];
                DataTable newDataTable = ds.Tables[0].Clone();

                object[] obj = new object[newDataTable.Columns.Count];
                //添加DataTable1的数据
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i].ItemArray.CopyTo(obj, 0);
                    newDataTable.Rows.Add(obj);
                }
                //添加DataTable2的数据
                //for (int i = 0; i < ds_ZQTMS.Tables[0].Rows.Count; i++)
                //{
                //    ds_ZQTMS.Tables[0].Rows[i].ItemArray.CopyTo(obj, 0);
                //    newDataTable.Rows.Add(obj);
                //}
                myGridControl1.DataSource = newDataTable;
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




        private void frmSelectZhenZhanKuCun_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);
            CommonClass.SetSite(TransferSite, true);
            CommonClass.SetSite(StartSite, true);
            StartSite.Text = CommonClass.UserInfo.SiteName;
            TransferSite.Text = CommonClass.UserInfo.SiteName;
            CommonClass.GetServerDate();

            //this.StartSite.SelectedItem = "深圳";
        }

        private void StartSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StartSite.Text.Trim() == "全部")
            {
                CommonClass.SetSite(TransferSite, false);
            }
            else
            {
                CommonClass.SetSite(TransferSite, true);
            }
        }

        private void TransferSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TransferSite.Text.Trim() == "全部")
            {
                CommonClass.SetSite(StartSite, false);
            }
            else
            {
                CommonClass.SetSite(StartSite, true);
            }
        }
    }
}