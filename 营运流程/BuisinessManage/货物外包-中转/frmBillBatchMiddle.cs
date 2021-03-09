using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using ZQTMS.UI.其他费用;

namespace ZQTMS.UI
{
    public partial class frmBillBatchMiddle : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        public DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;
        public int gettype = 0;
        int account_type = -1;//费用分摊方式
        public DevExpress.XtraGrid.Views.Grid.GridView gv;
        static frmBillBatchMiddle fbbm, fbbm2;
        public decimal costrate = 0;

        /// <summary>
        /// 获取当前窗体对象
        /// </summary>
        public static frmBillBatchMiddle Get_frmBillBatchMiddle(int type)
        {
            if ((fbbm == null || fbbm.IsDisposed) && type == 0)
            {
                fbbm = new frmBillBatchMiddle();
                fbbm.gettype = type;
                return fbbm;
            }
            if ((fbbm2 == null || fbbm2.IsDisposed) && type == 1)
            {
                fbbm2 = new frmBillBatchMiddle();
                fbbm2.gettype = type;
                return fbbm2;
            }
            return type == 0 ? fbbm : fbbm2;
        }

        public frmBillBatchMiddle()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            dataset1.Tables.Clear();
            dataset3.Tables.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("gettype", gettype));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_LOAD_Agent", list);
                dataset1 = SqlHelper.GetDataSet(sps);

                if (dataset1 == null || dataset1.Tables.Count == 0) return;
                dataset3 = dataset1.Clone();
                myGridControl1.DataSource = dataset1.Tables[0];
                myGridControl2.DataSource = dataset3.Tables[0];
              
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void w_package_load_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 0, Color.FromArgb(255, 255, 255));//颜色固定--白色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 1, Color.FromArgb(193, 255, 193));//颜色固定--绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Greater, 1, Color.LightBlue);//颜色固定--浅蓝色
            ////plh20191016

            MiddleDate.DateTime = CommonClass.gcdate;

            MiddleBatch.Text = GetMaxInOneVehicleFlag();
            MiddleOperator.Text = CommonClass.UserInfo.UserName;
            if (gettype == 0)
            {
                this.Text = "本地代理—" + this.Text;
            }
            else
            {
                this.Text = "终端代理—" + this.Text;
            }
            getCarrier();
            getDelivery();//获取派车单号
            if (CommonClass.UserInfo.companyid == "486")
            {
                this.AccMiddlePay.Properties.ReadOnly = true;
            }

        }

        private void getCarrier()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCARRIERUNIT_Ex", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void getDelivery()
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_DELIVERY", new List<SqlPara> { new SqlPara("SiteName", CommonClass.UserInfo.SiteName), new SqlPara("WebName", CommonClass.UserInfo.WebName) }));
            if (ds == null || ds.Tables.Count == 0) return;
            gcjiehuodanhao.DataSource = ds.Tables[0];
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void myGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl3_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                // if (hitInfo.InRowCell)
                //   gridControl3.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
            hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl2.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void gridControl3_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        ////////////////////////////////////////
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, dataset1, dataset3);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView2.ClearColumnsFilter();
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView2.ClearColumnsFilter();
        }

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView2.SelectRow(0);
            GridViewMove.Move(myGridView2, dataset3, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        public bool isDeficit(decimal Freight, decimal AccMiddlePay)
        {
            try
            {
                bool ischeck = false;
                Freight = (Freight == 0) ? 1 : Freight;
                costrate = Math.Round(AccMiddlePay / Freight, 2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("MiddleWebName", CommonClass.UserInfo.WebName.ToString().Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CostControlsList_rate_3", list);
                DataSet ds_check = SqlHelper.GetDataSet(sps);
                if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
                {
                    if (costrate > ConvertType.ToDecimal(ds_check.Tables[0].Rows[0]["TargetcostRate"].ToString()))
                    {


                        ischeck = true;
                    }


                }
                return ischeck;
            }

            catch (Exception)
            {

                throw;
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {

            if (CommonClass.UserInfo.companyid == "485")
            {
                Decimal FreightSum = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    FreightSum += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Freight"));

                }
                if (isDeficit(FreightSum, ConvertType.ToDecimal(AccMiddlePay.Text)))
                {
                    frmIsCostDeficits Cost = new frmIsCostDeficits();
                    Cost.DepartureBatch = "";//myGridView2.RowCount.ToString()
                    Cost.middlecount = myGridView2.RowCount;
                    Cost.MiddleWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                    Cost.actual_rate = costrate;
                    Cost.MenuType = "批量中转亏损";
                    Cost.ShowDialog();
                    if (Cost.isprint == true)
                    {
                        save(false);
                    }
                }

                else
                {
                    save(false);
                }

            }
            else
            {
                save(false);
            }


        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            if (CommonClass.UserInfo.companyid == "485")
            {
                Decimal FreightSum = 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    FreightSum += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Freight"));

                }
                if (isDeficit(FreightSum, ConvertType.ToDecimal(AccMiddlePay.Text)))
                {
                    frmIsCostDeficits Cost = new frmIsCostDeficits();
                    Cost.DepartureBatch = "";//myGridView2.RowCount.ToString()
                    Cost.middlecount = myGridView2.RowCount;
                    Cost.MiddleWebName = CommonClass.UserInfo.WebName.ToString().Trim();
                    Cost.actual_rate = costrate;
                    Cost.MenuType = "批量中转亏损";
                    Cost.ShowDialog();
                    if (Cost.isprint == true)
                    {
                        save(true);
                    }

                }
                else
                {
                    save(true);
                }
            }
            else
            {
                save(true);
            }
        }

        private void save(bool print)
        {
            if (MiddleCarrier.Text.Trim() == "")
            {
                XtraMessageBox.Show("请填写承运单位!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MiddleCarrier.Focus();
                return;
            }

            decimal MiddleOtherFeess = ConvertType.ToDecimal(MiddleOtherFee.Text);
            if (MiddleOtherFeess != 0 && MiddleOtherReason.Text.Trim() == "")
            {
                MsgBox.ShowError("您填了其他费,必须填写其他费说明!");
                MiddleOtherReason.Focus();
                return;
            }

            if (popupContainerEdit1.Text.Contains("选择"))
            {
                XtraMessageBox.Show("请选择费用分摊方式!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                popupContainerEdit1.Focus();
                popupContainerEdit1.ShowPopup();
                return;
            }
            myGridView2.PostEditor();
            myGridView2.UpdateCurrentRow();
            myGridView2.UpdateSummary();

            try
            {
                if (checkBox1.Checked && dataset3.Tables[0].Select("ischecked=1").Length == 0)
                {
                    XtraMessageBox.Show("您选择了短信通知,却没有勾选要需要通知的运单!\r\n\r\n请在第一步批量清单中勾选!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                decimal acc_tifu = Convert.ToDecimal(myGridView2.Columns["FetchPay"].SummaryItem.SummaryValue); //提付的运费合计

                if ((MiddleRebate.Text.Trim() == "" ? 0 : ConvertType.ToDecimal(MiddleRebate.Text)) > acc_tifu)
                {
                    XtraMessageBox.Show("填写的收中转回扣金额不合理,请检查!\r\n\r\n中转回扣金额大于承运单位代收提付款总金额!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (Math.Abs((CommonClass.gcdate - MiddleDate.DateTime).TotalDays) > 30)
            {
                if (MsgBox.ShowYesNo("中转时间跟当前时间超过30天,确定保存？") != DialogResult.Yes) return;
            }

            if (XtraMessageBox.Show("确认保存吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            string BillNos = "", AccMiddlePays = "", MiddleFeePaymentStates = "", MiddlePackageFees = "", MiddleHandleFees = "", MiddleForkliftFees = "", MiddleOtherFees = "", MiddleBillnos = "", MiddleSendFees = "", MiddleDeliCodes = "", MiddleCarNos = "", MiddleChauffers = "", MiddleChaufferPhones = "", MiddleBackFees = "", MiddleFreights = "", MiddleDeliFees = "";
            decimal accMiddlePay = 0, middleFreight = 0, middlePackageFee = 0, middleHandleFee = 0, middleForkliftFee = 0, middleDeliFee = 0, middleOtherFee = 0;

            try
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    accMiddlePay = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccMiddlePay"));
                    middleFreight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleFreight"));
                    middlePackageFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddlePackageFee"));
                    middleHandleFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleHandleFee"));
                    middleForkliftFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleForkliftFee"));
                    middleDeliFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleDeliFee"));
                    middleOtherFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleOtherFee"));

                    if (accMiddlePay <= 0)
                    {
                        accMiddlePay = middleFreight + middlePackageFee + middleHandleFee + middleForkliftFee + middleDeliFee + middleOtherFee;
                    }

                    BillNos += ConvertType.ToLong(myGridView2.GetRowCellValue(i, "BillNo")) + "@";
                    MiddleBillnos += ConvertType.ToString(myGridView2.GetRowCellValue(i, "MiddleBillNo")) + "@";
                    AccMiddlePays += accMiddlePay + "@";
                    MiddleFeePaymentStates += ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "MiddleFeePaymentState")) + "@";
                    MiddlePackageFees += middlePackageFee + "@";
                    MiddleHandleFees += middleHandleFee + "@";
                    MiddleForkliftFees += middleForkliftFee + "@";
                    MiddleOtherFees += middleOtherFee + "@";
                    MiddleSendFees += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleSendFee")) + "@";
                    MiddleDeliCodes += GridOper.GetRowCellValueString(myGridView2, i, "MiddleDeliCode") + "@";
                    MiddleCarNos += GridOper.GetRowCellValueString(myGridView2, i, "MiddleCarNo") + "@";
                    MiddleChauffers += GridOper.GetRowCellValueString(myGridView2, i, "MiddleChauffer") + "@";
                    MiddleChaufferPhones += GridOper.GetRowCellValueString(myGridView2, i, "MiddleChaufferPhone") + "@";
                    MiddleBackFees += ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "MiddleBackFee")) + "@";
                    MiddleFreights += middleFreight + "@";
                    MiddleDeliFees += middleDeliFee + "@";
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", BillNos));
                list.Add(new SqlPara("MiddleDate", MiddleDate.DateTime));
                list.Add(new SqlPara("MiddleCarrier", MiddleCarrier.Text.Trim()));
                list.Add(new SqlPara("MiddleStartSitePhone", MiddleStartSitePhone.Text.Trim()));
                list.Add(new SqlPara("MiddleEndSitePhone", MiddleEndSitePhone.Text.Trim()));
                list.Add(new SqlPara("MiddleBillNos", MiddleBillnos));
                list.Add(new SqlPara("MiddleSiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("MiddleWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("MiddleType", gettype));
                list.Add(new SqlPara("AccMiddlePays", AccMiddlePays));
                list.Add(new SqlPara("MiddleFeePaymentStates", MiddleFeePaymentStates));
                list.Add(new SqlPara("MiddleFetchAddress", MiddleFetchAddress.Text.Trim()));
                list.Add(new SqlPara("MiddleOperator", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("MiddleRemark", MiddleRemark.Text.Trim()));
                list.Add(new SqlPara("MiddlePackageFees", MiddlePackageFees));
                list.Add(new SqlPara("MiddleHandleFees", MiddleHandleFees));
                list.Add(new SqlPara("MiddleForkliftFees", MiddleForkliftFees));
                list.Add(new SqlPara("MiddleOtherFees", MiddleOtherFees));
                list.Add(new SqlPara("MiddleFreights", MiddleFreights));
                list.Add(new SqlPara("MiddleOtherReason", MiddleOtherReason.Text.Trim()));
                string middleBatch = GetMaxInOneVehicleFlag();
                if (middleBatch == "")
                {
                    MsgBox.ShowOK("没有获取到中转批次,请重试或稍后再试");
                    return;
                }
                list.Add(new SqlPara("MiddleBatch", middleBatch));
                list.Add(new SqlPara("MiddleCauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("MiddleAreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("MiddleDepName", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("MiddleSendFees", MiddleSendFees));
                list.Add(new SqlPara("MiddleDeliCodes", MiddleDeliCodes));
                list.Add(new SqlPara("MiddleCarNos", MiddleCarNos));
                list.Add(new SqlPara("MiddleChauffers", MiddleChauffers));
                list.Add(new SqlPara("MiddleChaufferPhones", MiddleChaufferPhones));
                list.Add(new SqlPara("MiddleBackFees", MiddleBackFees));
                list.Add(new SqlPara("MiddleDeliFees", MiddleDeliFees));
                list.Add(new SqlPara("opertype", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MIDDLE_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) == 0) return;

                XtraMessageBox.Show("中转完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (checkBox1.Checked)
                {
                    //sms.outsendsms_pl(myGridView2, edoutdate.DateTime, edcygs.Text.Trim(), edtel.Text.Trim());
                }
                if (print)
                {
                    if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL)
                    {
                        frmPrintRuiLang fpr = new frmPrintRuiLang("中转清单(项目部)", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT", new List<SqlPara> { new SqlPara("MiddleBatch", middleBatch) })));
                        fpr.ShowDialog();
                    }
                    else if (CommonClass.UserInfo.companyid == "163") //hj20180719
                    {
                        //string middlelist = CommonClass.UserInfo.MiddleList == "中转清单";  //maohui20180324
                        frmPrintRuiLang fpr = new frmPrintRuiLang("中转清单", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT_TX", new List<SqlPara> { new SqlPara("MiddleBatch", middleBatch) })));
                        fpr.ShowDialog();
                    }
                    else
                    {
                        string middlelist = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;  //maohui20180324
                        //DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT", new List<SqlPara> { new SqlPara("MiddleBatch", middleBatch) }));
                        DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT_TX", new List<SqlPara> { new SqlPara("MiddleBatch", middleBatch) }));
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            row["NowCompany"] = CommonClass.UserInfo.gsqc;
                        }
                        frmPrintRuiLang fpr = new frmPrintRuiLang(middlelist, ds);
                        fpr.ShowDialog();
                    }
                }
                MiddleBatch.Text = GetMaxInOneVehicleFlag();
                popupContainerEdit1.Text = "请选择费用分摊方式";
                popupContainerEdit1.EditValue = "请选择费用分摊方式";
                //dataset3.Clear();
                if (gv != null)
                {
                    for (int i = 0; i < dataset3.Tables[0].Rows.Count; i++)
                    {
                        for (int j = gv.RowCount - 1; j >= 0; j--)
                        {
                            if (GridOper.GetRowCellValueString(gv, j, "BillNo") == ConvertType.ToString(dataset3.Tables[0].Rows[i]["BillNo"]))
                            {
                                gv.DeleteRow(j);
                                break;
                            }
                        }
                    }
                }
                //List<SqlPara> listQuery = new List<SqlPara>();
                //listQuery.Add(new SqlPara("BillNos",BillNos));
                //SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ZQTMSMiddleSYS", listQuery);
                //DataSet ds = SqlHelper.GetDataSet(spsQuery);
                //if (ds == null || ds.Tables[0].Rows.Count == 0) return;
                //string dsJson = JsonConvert.SerializeObject(ds);
                //RequestModel<string> request = new RequestModel<string>();
                //request.Request = dsJson;
                //request.OperType = 0;
                //string json = JsonConvert.SerializeObject(request);
                ////string url = "http://localhost:42936/KDLMSService/ZQTMSMiddleSys";
                //string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSMiddleSys";

            
                //ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                //if (model.State != "200")
                //{
                //    MsgBox.ShowOK(model.Message);
                //}
               // MiddleSyn(BillNos);
                this.Close();
                string gettype1 = gettype.ToString();
                CommonSyn.MIDDLE_NEW_SYN(BillNos, MiddleBillnos, gettype1);//yzw
                //CommonSyn.MiddleSyn(BillNos);
                //CommonSyn.TraceSyn(null, BillNos, gettype==0?3:8, gettype == 0 ? "本地中转" : "终端中转", 1, null, null);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //中转同步 2018-4-9 zaj
        //private void MiddleSyn(string BillNos)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNos", BillNos));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_ZQTMSMiddleSYS", listQuery);
        //        DataSet ds = SqlHelper.GetDataSet(spsQuery);
        //        if (ds == null || ds.Tables[0].Rows.Count == 0) return;
        //        string dsJson = JsonConvert.SerializeObject(ds);
        //        RequestModel<string> request = new RequestModel<string>();
        //        request.Request = dsJson;
        //        request.OperType = 0;
        //        string json = JsonConvert.SerializeObject(request);
        //        //string url = "http://localhost:42936/KDLMSService/ZQTMSMiddleSys";
        //       // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSMiddleSys";
        //        string url = HttpHelper.urlMiddleSyn;


        //        ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //        if (model.State != "200")
        //        {
        //            MsgBox.ShowOK(model.Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        private bool HasApply(string sBillNo)
        {
            bool bl = false;
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("BillNO", sBillNo));
                list1.Add(new SqlPara("ApplyType", "控货/放货"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLAPPLY_HasApplying", list1);
                DataSet ds = SqlHelper.GetDataSet(sps1);
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    bl = false;
                else
                    bl = true;
            }
            catch
            {
                bl = false;

            }
            return bl;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edcygs_Enter(object sender, EventArgs e)
        {
            try
            {
                if (MiddleCarrier.Text.Trim() == "")
                {
                    gridView2.ClearColumnsFilter();
                }
                if (gridView2.RowCount == 0) return;
                gridControl3.Parent.Controls.Remove(gridControl3);
                MiddleCarrier.Parent.Parent.Controls.Add(gridControl3);
                gridControl3.BringToFront();
                gridControl3.Left = MiddleCarrier.Left + MiddleCarrier.Parent.Left;
                gridControl3.Top = MiddleCarrier.Top + MiddleCarrier.Height + MiddleCarrier.Parent.Top;
                gridControl3.Visible = true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void edcygs_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView2.ClearColumnsFilter();
                gridView2.Columns["CompanyName"].FilterInfo = new ColumnFilterInfo("[CompanyName] LIKE " + "'%" + e.NewValue.ToString() + "%'", "");
            }
            else
            {
                gridView2.ClearColumnsFilter();
            }
        }

        private void edcygs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControl3.Focus();
            }
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                gridControl3.Visible = false;
            }
        }

        private void edcygs_Leave(object sender, EventArgs e)
        {
            if (!gridControl3.Focused)
            {
                gridControl3.Visible = false;
            }
        }

        private void gridControl3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                setcygs();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void setcygs()
        {
            try
            {
                if (gridView2.FocusedRowHandle < 0) return;

                DataRow cdr = gridView2.GetDataRow(gridView2.FocusedRowHandle);

                MiddleCarrier.EditValue = cdr["CompanyName"];
                MiddleStartSitePhone.EditValue = cdr["SalesCellPhone"];
                MiddleEndSitePhone.EditValue = cdr["SalesPhone"];
                MiddleFetchAddress.EditValue = cdr["ArriveAddress"];
                gridControl3.Visible = false;
                MiddleStartSitePhone.Focus();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    setcygs();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message);
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                if (gridView2.FocusedRowHandle == 0)
                {
                    MiddleCarrier.Focus();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl3.Visible = false;
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
                {
                    myGridView2.PostEditor();
                    edweight.EditValue = myGridView2.Columns["Weight"].SummaryItem.SummaryValue;
                    edvolumn.EditValue = myGridView2.Columns["Volume"].SummaryItem.SummaryValue;
                    MiddleBackFee.EditValue = GridOper.GetGridViewColumn(myGridView2, "MiddleBackFee").SummaryItem.SummaryValue;

                    //统计已收未收 已付未付
                    int accoperoutstate = 0;
                    decimal b1 = 0, b2 = 0, outaccnow = 0;
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        accoperoutstate = ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "MiddleFeePaymentState"));
                        outaccnow = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "AccMiddlePay"));

                        b1 += accoperoutstate == 1 ? outaccnow : 0;
                        b2 += accoperoutstate == 0 ? outaccnow : 0;
                    }
                    textEdit6.EditValue = Math.Round(b1, 2);
                    textEdit7.EditValue = Math.Round(b2, 2);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 获取中转批次
        /// </summary>
        /// <returns></returns>
        private string GetMaxInOneVehicleFlag()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginWebCode));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_BATCH", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return "";
            return ConvertType.ToString(ds.Tables[0].Rows[0][0]);
        }

        private void SetColumnState(bool state)
        {
            List<string> list = new List<string>(new string[] { "AccMiddlePay", "MiddlePackageFee", "MiddleHandleFee", "MiddleForkliftFee", "MiddleOtherFee", "MiddleFeePaymentState", "MiddleSendFee", "MiddleDeliCode", "MiddleCarNo", "MiddleChauffer", "MiddleChaufferPhone", "MiddleFreight", "MiddleDeliFee" });

            foreach (string s in list)
            {
                GridColumn gc = myGridView2.Columns[s];
                if (gc == null) continue;
                gc.OptionsColumn.AllowEdit = gc.OptionsColumn.AllowFocus = state;
                if (state) gc.AppearanceCell.BackColor = Color.Yellow;
                else gc.AppearanceCell.BackColor = Color.White;
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            myGridView2.ClearColumnsFilter();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show(this, "请先到第①步挑选要批量中转的运单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
                return;
            }
            GridColumn gcout = myGridView2.Columns["AccMiddlePay"];
            GridColumn gcpackage = myGridView2.Columns["MiddlePackageFee"];
            GridColumn gczx = myGridView2.Columns["MiddleHandleFee"];
            GridColumn gccc = myGridView2.Columns["MiddleForkliftFee"];
            GridColumn gcother = myGridView2.Columns["MiddleOtherFee"];
            GridColumn gcMiddleSendFee = GridOper.GetGridViewColumn(myGridView2, "MiddleSendFee");//转送费
            GridColumn gcMiddleFreight = GridOper.GetGridViewColumn(myGridView2, "MiddleFreight");//中转运费
            GridColumn gcMiddleDeliFee = GridOper.GetGridViewColumn(myGridView2, "MiddleDeliFee");//中转送货费

            decimal acc = 0;//当前行所需的值
            try
            {
                int type = radioGroup1.SelectedIndex;

                if (type == 4)
                {
                    popupContainerEdit1.Text = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description;
                    if (XtraMessageBox.Show(this, "您选择了手工填写每一票的费用，请到第①步的批量中转清单中填写相关费用!\r\n\r\n现在开始手工分摊?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    }
                    SetColumnState(true);
                    account_type = type;
                    myGridView2.FocusedRowHandle = 0;
                    return;
                }
                decimal outacc = ConvertType.ToDecimal(AccMiddlePay.Text);
                decimal outaccpackage = ConvertType.ToDecimal(MiddlePackageFee.Text);
                decimal outacczx = ConvertType.ToDecimal(MiddleHandleFee.Text);
                decimal outacccc = ConvertType.ToDecimal(MiddleForkliftFee.Text);
                decimal accreduce = ConvertType.ToDecimal(MiddleOtherFee.Text);
                decimal middleSendFee = ConvertType.ToDecimal(MiddleSendFee.Text);
                decimal middleFreight = ConvertType.ToDecimal(MiddleFreight.Text);
                decimal middleDeliFee = ConvertType.ToDecimal(MiddleDeliFee.Text);
                if (outacc + outaccpackage + outacczx + outacccc + accreduce + middleSendFee + middleFreight + middleDeliFee == 0)
                {
                    XtraMessageBox.Show(this, "没有输入费用金额,无需分摊!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SetColumnState(false);

                if (type == 0)
                {//按件数分摊
                    decimal qtytotal = ConvertType.ToDecimal(myGridView2.Columns["Num"].SummaryItem.SummaryValue);
                    if (qtytotal == 0)
                    {
                        MsgBox.ShowOK("没有件数，不能按件数分摊!");
                        return;
                    }
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        acc = ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "Num"));
                        myGridView2.SetRowCellValue(i, gcout, Math.Round(outacc * acc / qtytotal, 2));//中转费
                        myGridView2.SetRowCellValue(i, gcpackage, Math.Round(outaccpackage * acc / qtytotal, 2));//中转包装费
                        myGridView2.SetRowCellValue(i, gczx, Math.Round(outacczx * acc / qtytotal, 2));//中转装卸费
                        myGridView2.SetRowCellValue(i, gccc, Math.Round(outacccc * acc / qtytotal, 2));//中转叉车费
                        myGridView2.SetRowCellValue(i, gcother, Math.Round(accreduce * acc / qtytotal, 2));//中转其他费
                        myGridView2.SetRowCellValue(i, gcMiddleSendFee, Math.Round(middleSendFee * acc / qtytotal, 2));//转送费
                        myGridView2.SetRowCellValue(i, gcMiddleFreight, Math.Round(middleFreight * acc / qtytotal, 2));//中转运费
                        myGridView2.SetRowCellValue(i, gcMiddleDeliFee, Math.Round(middleDeliFee * acc / qtytotal, 2));//中送货费
                    }
                }
                else if (type == 1)
                {//按基本运费
                    decimal acctotal = ConvertType.ToDecimal(myGridView2.Columns["Freight"].SummaryItem.SummaryValue); //总运费
                    if (acctotal == 0)
                    {
                        MsgBox.ShowOK("没有运费，不能按运费分摊!");
                        return;
                    }
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        acc = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Freight"));
                        myGridView2.SetRowCellValue(i, gcout, Math.Round(outacc * acc / acctotal, 2));//中转费
                        myGridView2.SetRowCellValue(i, gcpackage, Math.Round(outaccpackage * acc / acctotal, 2));//中转包装费
                        myGridView2.SetRowCellValue(i, gczx, Math.Round(outacczx * acc / acctotal, 2));//中转装卸费
                        myGridView2.SetRowCellValue(i, gccc, Math.Round(outacccc * acc / acctotal, 2));//中转叉车费
                        myGridView2.SetRowCellValue(i, gcother, Math.Round(accreduce * acc / acctotal, 2));//中转其他费
                        myGridView2.SetRowCellValue(i, gcMiddleSendFee, Math.Round(middleSendFee * acc / acctotal, 2));//转送费
                        myGridView2.SetRowCellValue(i, gcMiddleFreight, Math.Round(middleFreight * acc / acctotal, 2));//中转运费
                        myGridView2.SetRowCellValue(i, gcMiddleDeliFee, Math.Round(middleDeliFee * acc / acctotal, 2));//中送货费
                    }
                }
                else if (type == 2)
                {//按票均分 
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        myGridView2.SetRowCellValue(i, gcout, Math.Round(outacc / myGridView2.RowCount, 2));//中转费
                        myGridView2.SetRowCellValue(i, gcpackage, Math.Round(outaccpackage / myGridView2.RowCount, 2));//中转包装费
                        myGridView2.SetRowCellValue(i, gczx, Math.Round(outacczx / myGridView2.RowCount, 2));//中转装卸费
                        myGridView2.SetRowCellValue(i, gccc, Math.Round(outacccc / myGridView2.RowCount, 2));//中转叉车费
                        myGridView2.SetRowCellValue(i, gcother, Math.Round(accreduce / myGridView2.RowCount, 2));//中转其他费
                        myGridView2.SetRowCellValue(i, gcMiddleSendFee, Math.Round(middleSendFee / myGridView2.RowCount, 2));//转送费
                        myGridView2.SetRowCellValue(i, gcMiddleFreight, Math.Round(middleFreight / myGridView2.RowCount, 2));//中转运费
                        myGridView2.SetRowCellValue(i, gcMiddleDeliFee, Math.Round(middleDeliFee / myGridView2.RowCount, 2));//中转送货费
                    }
                }
                else
                {//放在第一票 type == 3 
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        myGridView2.SetRowCellValue(i, gcout, 0);
                        myGridView2.SetRowCellValue(i, gcpackage, 0);
                        myGridView2.SetRowCellValue(i, gczx, 0);
                        myGridView2.SetRowCellValue(i, gccc, 0);
                        myGridView2.SetRowCellValue(i, gcother, 0);
                        myGridView2.SetRowCellValue(i, gcMiddleSendFee, 0);
                        myGridView2.SetRowCellValue(i, gcMiddleFreight, 0);
                        myGridView2.SetRowCellValue(i, gcMiddleDeliFee, 0);
                    }
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        myGridView2.SetRowCellValue(0, gcout, Math.Round(outacc, 2));//中转费
                        myGridView2.SetRowCellValue(0, gcpackage, Math.Round(outaccpackage, 2));//中转包装费
                        myGridView2.SetRowCellValue(0, gczx, Math.Round(outacczx, 2));//中转装卸费
                        myGridView2.SetRowCellValue(0, gccc, Math.Round(outacccc, 2));//中转叉车费
                        myGridView2.SetRowCellValue(0, gcother, Math.Round(accreduce, 2));//中转其他费
                        myGridView2.SetRowCellValue(0, gcMiddleSendFee, Math.Round(middleSendFee, 2));//转送费
                        myGridView2.SetRowCellValue(0, gcMiddleFreight, Math.Round(middleFreight, 2));//中转运费
                        myGridView2.SetRowCellValue(0, gcMiddleDeliFee, Math.Round(middleDeliFee, 2));//中送货费
                    }
                }
                popupContainerEdit1.Text = radioGroup1.Properties.Items[radioGroup1.SelectedIndex].Description;
                myGridView2.PostEditor();
                myGridView2.UpdateSummary();
                account_type = type;

                CalculateMiddleBackFee();//计算转送回扣

                XtraMessageBox.Show(this, "分摊成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(this, ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMiddleBackFee()
        {
            try
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    myGridView2.SetRowCellValue(i, "MiddleBackFee", ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "FetchPay")) - ConvertType.ToDecimal(gridView2.GetRowCellValue(i, "AccMiddleFee")));
                }
                myGridView2.PostEditor();
                MiddleBackFee.EditValue = GridOper.GetGridViewColumn(myGridView2, "MiddleBackFee").SummaryItem.SummaryValue;
            }
            catch { }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "MiddleRebateFee", 0);
                myGridView2.SetRowCellValue(i, "AccMiddlePay", 0);
                myGridView2.SetRowCellValue(i, "MiddlePackageFee", 0);
                myGridView2.SetRowCellValue(i, "MiddleHandleFee", 0);
                myGridView2.SetRowCellValue(i, "MiddleForkliftFee", 0);
                myGridView2.SetRowCellValue(i, "MiddleOtherFee", 0);
            }
            SetColumnState(false);
            popupContainerEdit1.EditValue = "请选择费用分摊方式";
            popupContainerEdit1.Text = "请选择费用分摊方式";
            XtraMessageBox.Show("取消分摊成功!中转相关费用已清零!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Redo(object sender, EventArgs e)
        {
            if (account_type == 4) return;
            popupContainerEdit1.Text = "请选择费用分摊方式";
            popupContainerEdit1.EditValue = "请选择费用分摊方式";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int state = checkBox1.Checked ? 1 : 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    myGridView2.SetRowCellValue(i, "ischecked", state);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        private void MiddleDeliCode_Enter(object sender, EventArgs e)
        {
            gcjiehuodanhao.Left = groupControl2.Left + groupBox1.Left + MiddleDeliCode.Left;
            gcjiehuodanhao.Top = groupControl2.Top + groupBox1.Top + MiddleDeliCode.Top + MiddleDeliCode.Height + 2;
            gcjiehuodanhao.Visible = true;
        }

        private void MiddleDeliCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) gcjiehuodanhao.Focus();
        }

        private void MiddleDeliCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e == null) return;
            string s = ConvertType.ToString(e.NewValue);
            if (s == "") gridView5.ClearColumnsFilter();
            else gridColumn7.FilterInfo = new ColumnFilterInfo("DeliCode LIKE '%" + e.NewValue + "%'");
        }

        private void gcjiehuodanhao_Leave(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = MiddleDeliCode.Focused;
        }

        private void MiddleDeliCode_Leave(object sender, EventArgs e)
        {
            gcjiehuodanhao.Visible = gcjiehuodanhao.Focused;
        }

        private void gridView5_DoubleClick(object sender, EventArgs e)
        {
            SetDeliCode();
        }

        private void SetDeliCode()
        {
            int rowhandle = gridView5.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = gridView5.GetDataRow(rowhandle);
            if (dr == null) return;
            MiddleDeliCode.EditValue = dr["DeliCode"];
            MiddleSendFee.EditValue = dr["VehFare"];
            MiddleCarNo.EditValue = dr["VehicleNum"];
            MiddleChauffer.EditValue = dr["DriverName"];
            MiddleChaufferPhone.EditValue = dr["DeliCusPhone"];
            gcjiehuodanhao.Visible = false;
        }

        private void gcjiehuodanhao_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) SetDeliCode();
        }

        private void myGridView2_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || e.Column.FieldName != "AccMiddlePay") return;

            try
            {
                myGridView2.SetRowCellValue(e.RowHandle, "MiddleBackFee", ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "FetchPay")) - ConvertType.ToDecimal(e.Value));
            }
            catch { }
        }

        private void CalculateMiddleFee(object sender, EventArgs e)
        {
            AccMiddlePay.Text = (ConvertType.ToDecimal(MiddleFreight.Text) + ConvertType.ToDecimal(MiddlePackageFee.Text) + ConvertType.ToDecimal(MiddleHandleFee.Text) + ConvertType.ToDecimal(MiddleForkliftFee.Text) + ConvertType.ToDecimal(MiddleDeliFee.Text) + ConvertType.ToDecimal(MiddleOtherFee.Text)).ToString();
        }

        private void myGridView2_RowCountChanged(object sender, EventArgs e)
        {
            popupContainerEdit1.Text = "请选择费用分摊方式";
            popupContainerEdit1.EditValue = "请选择费用分摊方式";

            int rowhandle = myGridView2.RowCount - 1;
            if (rowhandle < 0) return;
            if (GridOper.GetRowCellValueString(myGridView2, rowhandle, "MiddleDeliCode") != "") return;

            myGridView2.SetRowCellValue(rowhandle, "MiddleDeliCode", MiddleDeliCode.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "MiddleCarNo", MiddleCarNo.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "MiddleChauffer", MiddleChauffer.Text.Trim());
            myGridView2.SetRowCellValue(rowhandle, "MiddleChaufferPhone", MiddleChaufferPhone.Text.Trim());
        }

        private void SetGridViewMiddleInfo(object sender, EventArgs e)
        {
            if (radioGroup1.SelectedIndex == 4 || myGridView2.RowCount == 0) return;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "MiddleDeliCode", MiddleDeliCode.Text.Trim());
                myGridView2.SetRowCellValue(i, "MiddleCarNo", MiddleCarNo.Text.Trim());
                myGridView2.SetRowCellValue(i, "MiddleChauffer", MiddleChauffer.Text.Trim());
                myGridView2.SetRowCellValue(i, "MiddleChaufferPhone", MiddleChaufferPhone.Text.Trim());
            }
        }
    }
}