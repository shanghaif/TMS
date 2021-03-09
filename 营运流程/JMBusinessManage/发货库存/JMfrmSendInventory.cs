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

namespace ZQTMS.UI
{
    public partial class JMfrmSendInventory : BaseForm
    {
        public JMfrmSendInventory()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));

                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("TransferSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("BegWeb", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_SendInventory", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
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

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);
            CommonClass.SetCause(Cause, true);

            bsite.Text = CommonClass.UserInfo.SiteName;
            esite.Text = "全部";
            web.Text = CommonClass.UserInfo.WebName;

            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
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
            GridOper.ExportToExcel(myGridView1, "发货库存");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                myGridView1.PostEditor();
                myGridView1.UpdateCurrentRow();
                string BillNo = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0 && myGridView1.GetRowCellValue(i, "TransitMode").ToString() == "中强城际")
                    {
                        BillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    }

                    if (CommonClass.UserInfo.WebName != "深圳广源省际操作部")
                    {
                        if (myGridView1.GetRowCellValue(i, "BegWeb").ToString() != CommonClass.UserInfo.WebName
                            && myGridView1.GetRowCellValue(i, "SCDesWeb").ToString() != CommonClass.UserInfo.WebName)
                        {
                            MsgBox.ShowOK("只能调拨当前网点库存的运单！");
                            return; 
                        }
                    }

                }
                if (BillNo == "")
                {
                    MsgBox.ShowOK("请选择需要调拨的城际运单！");
                    return; 
                }
                JMfrmSendCity fs = new JMfrmSendCity();
                DialogResult dr= fs.ShowDialog();
                if (dr != DialogResult.Yes) return;
                if (fs.SiteName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择目的站点");
                    return;
                }
                if (fs.WebName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择目的网点");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("AcceptSiteName", fs.SiteName.Text.Trim()));
                list.Add(new SqlPara("AcceptWebName", fs.WebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SNEDTOCITY", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    for (int i = myGridView1.RowCount - 1; i >= 0; i--)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            myGridView1.DeleteRow(i);
                        }
                    }
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                myGridView1.PostEditor();
                myGridView1.UpdateCurrentRow();
                string BillNo = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0 && myGridView1.GetRowCellValue(i, "TransitMode").ToString() != "中强城际")
                    {
                        BillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    }
                }
                if (BillNo == "")
                {
                    MsgBox.ShowOK("请选择需要调拨的省际运单！");
                    return;
                }

                JMfrmSendDeparture fs = new JMfrmSendDeparture();
                DialogResult dr = fs.ShowDialog();
                if (dr != DialogResult.Yes) return;
                if (fs.DepartureBatch.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须发车批次");
                    return;
                }
                if (fs.SiteName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择目的站点");
                    return;
                }
                if (fs.WebName.Text.Trim() == "")
                {
                    MsgBox.ShowOK("必须选择目的网点");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("AcceptSiteName", fs.SiteName.Text.Trim()));
                list.Add(new SqlPara("AcceptWebName", fs.WebName.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", fs.DepartureBatch.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SNEDDeparture", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    for (int i = myGridView1.RowCount - 1; i >= 0; i--)
                    {
                        if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                        {
                            myGridView1.DeleteRow(i);
                        }
                    }
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            JMfrmSendCityRecord fs = new JMfrmSendCityRecord();
            fs.ShowDialog();
        }
    }
}