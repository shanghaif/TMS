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
using ZQTMS.Lib;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmErrorLine : BaseForm
    {
        public frmErrorLine()
        {
            InitializeComponent();
        }

        private void frmErrorLine_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("异常路由");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);

            CommonClass.SetSite(bSite, true);
            CommonClass.SetSite(eSite, true);
            CommonClass.SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
        }

        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            myGridView1.ClearColumnsFilter();
            string strBillNos = "";
            if (!string.IsNullOrEmpty(txtBillNos.Text))
            {
                string[] billNos = txtBillNos.Lines;
                if (billNos.Length > 20)
                {
                    MsgBox.ShowOK("最多只能输入20个单号！");
                    return;
                }
                for (int i = 0; i < billNos.Length; i++)
                {
                    strBillNos += billNos[i] + ",";
                }
            }
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            string state = cbbState.Text.Trim() == "全部" ? "%%" : cbbState.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("bSite", bsite));
                list.Add(new SqlPara("eSite", esite));
                list.Add(new SqlPara("webName", webName));
                //list.Add(new SqlPara("state", state));   状态和运单号已隐藏
                //list.Add(new SqlPara("BillNos", strBillNos));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ErrorLine_Detail", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                myGridControl1.DataSource = ds.Tables[0];
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal ActualFreight = 0, NewActualFreight = 0,
                            Num = 0, WebPCS = 0;
                    ActualFreight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "ActualFreight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewActualFreight = Math.Round(ActualFreight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "ActualFreight", NewActualFreight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal FeeWeight = 0, NewFeeWeight = 0,
                            Num = 0, WebPCS = 0;
                    FeeWeight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "FeeWeight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewFeeWeight = Math.Round(FeeWeight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "FeeWeight", NewFeeWeight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal FeeVolume = 0, NewFeeVolume = 0,
                            Num = 0, WebPCS = 0;
                    FeeVolume = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "FeeVolume"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewFeeVolume = Math.Round(FeeVolume * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "FeeVolume", NewFeeVolume);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal Weight = 0, NewWeight = 0,
                            Num = 0, WebPCS = 0;
                    Weight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Weight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewWeight = Math.Round(Weight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "Weight", NewWeight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal Volume = 0, NewVolume = 0,
                            Num = 0, WebPCS = 0;
                    Volume = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Volume"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewVolume = Math.Round(Volume * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "Volume", NewVolume);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal OperationWeight = 0, NewOperationWeight = 0,
                            Num = 0, WebPCS = 0;
                    OperationWeight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "OperationWeight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewOperationWeight = Math.Round(OperationWeight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "OperationWeight", NewOperationWeight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal Freight = 0, NewFreight = 0,
                            Num = 0, WebPCS = 0;
                    Freight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Freight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewFreight = Math.Round(Freight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "Freight", NewFreight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal OptWeight = 0, NewOptWeight = 0,
                            Num = 0, WebPCS = 0;
                    OptWeight = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "OptWeight"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewOptWeight = Math.Round(OptWeight * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "OptWeight", NewOptWeight);
                }
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    decimal MainlineFee = 0, NewMainlineFee = 0,
                            Num = 0, WebPCS = 0;
                    MainlineFee = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "MainlineFee"));
                    Num = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "Num"));
                    WebPCS = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WebPCS"));
                    NewMainlineFee = Math.Round(MainlineFee * (WebPCS / Num), 2);
                    myGridView1.SetRowCellValue(i, "MainlineFee", NewMainlineFee);
                }
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "异常路由");
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
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

    }
}
