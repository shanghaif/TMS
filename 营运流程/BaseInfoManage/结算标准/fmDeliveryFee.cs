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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class fmDeliveryFee : BaseForm
    {
        public fmDeliveryFee()
        {
            InitializeComponent();

        }
        GridColumn gcIsseleckedMode;
        private void fmDeliveryFee_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("结算送货费");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例  
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FillProvince();
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        private void FillProvince()
        {
            DataSet ds = ZQTMS.Common.CommonClass.AreaManager.DsArea;

            if (ds == null || ds.Tables.Count == 0)
            {
                return;
            }
            repositoryItemImageComboBox2.Items.Clear();
            DataRow[] drs = ds.Tables[0].Select("RegionLevel=1");
            ImageComboBoxItem item;
            List<ImageComboBoxItem> list = new List<ImageComboBoxItem>();
            for (int i = 0; i < drs.Length; i++)
            {
                item = new ImageComboBoxItem(drs[i]["RegionName"].ToString(), drs[i]["RegionID"], -1);
                list.Add(item);
            }
            list.Insert(0, new ImageComboBoxItem("全部", -1));
            repositoryItemImageComboBox2.Items.AddRange(list);
            barEditItem1.EditValue = "全部";
        }

        private void LoadData()
        {
            try
            {
                DataSet dset = ZQTMS.Common.CommonClass.AreaManager.DsArea;

                if (dset == null || dset.Tables.Count == 0)
                {
                    return;
                }

                string Province = barEditItem1.EditValue == null ? "全部" : barEditItem1.EditValue.ToString();
                string City = barEditItem2.EditValue == null ? "全部" : barEditItem2.EditValue.ToString();
                if (Province != "全部") //此处取到的竟然是ID
                {
                    DataRow[] drs = dset.Tables[0].Select("RegionID=" + Province);
                    if (drs.Length > 0)
                    {
                        Province = drs[0]["RegionName"].ToString();
                    }
                }
                if (City != "全部") //此处取到的竟然是ID
                {
                    DataRow[] drs = dset.Tables[0].Select("RegionID=" + City);
                    if (drs.Length > 0)
                    {
                        City = drs[0]["RegionName"].ToString();
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Province", Province));
                list.Add(new SqlPara("City", City));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDELIVERYFEE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmDeliveryFeeAdd frm = new fmDeliveryFeeAdd();
            frm.ShowDialog();
        }

        private void barBtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliveryFeeID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeliveryFeeID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDELIVERYFEE_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                fmDeliveryFeeAdd frm = new fmDeliveryFeeAdd();
                frm.dr = dr;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                myGridView1.PostEditor();
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliveryFeeID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string DeliveryFeeIDs = "";
                string companyids = "";
                if (rowhandle >= 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            DeliveryFeeIDs += myGridView1.GetRowCellValue(i, "DeliveryFeeID").ToString() + "@";
                            companyids = companyids + myGridView1.GetRowCellValue(i, "companyid").ToString() + "@";
                            
                        }
                    }
                }
                if (DeliveryFeeIDs == "") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeliveryFeeID", DeliveryFeeIDs));
                list.Add(new SqlPara("companyids", companyids));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BASDELIVERYFEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    myGridView1.PostEditor();
                    myGridView1.UpdateCurrentRow();
                    myGridView1.UpdateSummary();
                    DataTable dt = myGridControl1.DataSource as DataTable;
                    dt.AcceptChanges();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barBtnFilter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadData();
        }

        private void barBtnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barBtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barBtnImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            fmDeliveryFeeUp up = new fmDeliveryFeeUp();
            up.ShowDialog();
        }

        private void repositoryItemImageComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = ZQTMS.Common.CommonClass.AreaManager.DsArea;

            if (ds == null || ds.Tables.Count == 0)
            {
                return;
            }

            ImageComboBoxEdit edit = sender as ImageComboBoxEdit;
            string filter = "";
            if (edit.Text != "全部")
            {
                filter = "ParentID=" + edit.Value;
            }
            
            repositoryItemImageComboBox3.Items.Clear();
            DataRow[] drs = ds.Tables[0].Select(filter);
            ImageComboBoxItem item;
            List<ImageComboBoxItem> list = new List<ImageComboBoxItem>();
            for (int i = 0; i < drs.Length; i++)
            {
                item = new ImageComboBoxItem(drs[i]["RegionName"].ToString(), drs[i]["RegionID"], -1);
                list.Add(item);
            }
            list.Insert(0, new ImageComboBoxItem("全部", -1));
            repositoryItemImageComboBox3.Items.AddRange(list);
            barEditItem2.EditValue = "全部";
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

    }
}
