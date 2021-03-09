using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.UI
{
    public partial class frmTransactionLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;

        public frmTransactionLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();

                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                CauseName = CauseName == "全部" ? "%%" : CauseName;
                AreaName = AreaName == "全部" ? "%%" : AreaName;
                WebName = WebName == "全部" ? "%%" : WebName;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("WebName", WebName));
                list.Add(new SqlPara("transactionType", barEditItem3.EditValue.ToString()));//按异动类型提取

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_TRANSACTIONFORADUIT_Load_NEW", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmBatchFetchSign_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;

            repositoryItemComboBox1.Properties.Items.Add("提付异动");
            repositoryItemComboBox1.Properties.Items.Add("非提付异动");
            repositoryItemComboBox1.Properties.Items.Add("全部");

            splitContainerControl1.Horizontal = false;
            splitContainerControl1.SplitterPosition = 480;
        }

        BarEditItem barEditCause = new BarEditItem(); //生成事业部
        BarEditItem barEditArea = new BarEditItem(); //生成大区
        BarEditItem barEditWeb = new BarEditItem(); //生成网点

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditArea.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetArea(repositoryItemComboBox, barEditCause.EditValue + "", true);
            barEditArea.EditValue = "全部";
        }
        private void barEditItem1_EditValueChanged_2(object sender, EventArgs e)
        {
            RepositoryItemComboBox repositoryItemComboBox = (RepositoryItemComboBox)barEditWeb.Edit;
            repositoryItemComboBox.Items.Clear();
            CommonClass.SetCauseWeb(repositoryItemComboBox, barEditCause.EditValue + "", barEditArea.EditValue + "");
            barEditWeb.EditValue = "全部";
        }

        #region 移动网格数据相关
        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void gridControl1_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl2_MouseMove(object sender, MouseEventArgs e)
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

        private void gridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }
        #endregion

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
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds, ds1);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds1, ds);
        }

        private void btnClose_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        ////////////////////////////////////////
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                GridOper.GetGridViewColumn(myGridView1, "BillNo").FilterInfo = new ColumnFilterInfo("BillNO LIKE '%" + szfilter + "%'", "");
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
            GridViewMove.Move(myGridView1, ds, ds1);
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
                myGridView2.Columns["BillNO"].FilterInfo = new ColumnFilterInfo("BillNO LIKE " + "'%" + szfilter + "%'", "");
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
            GridViewMove.Move(myGridView2, ds1, ds);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //非提付异动
                string sBillNo = "";//ID串
                string sAmount = "";//本次审核金额串
                string sNowPayVerifBalance = "";//核销余额串
                string BillNos = "";//运单号串
                DateTime credentialsTime = CommonClass.gcdate;
                float sumAmount = 0;//核销总金额
                int num = 0;
                //提付异动
                string sBillNo1 = "";//ID串
                string sAmount1 = "";//本次审核金额串
                string sFetchPayVerifBalance1 = "";//核销余额串
                string BillNos1 = "";
                DateTime credentialsTime1 = CommonClass.gcdate;
                float sumAmount1 = 0;//运单号串
                int num1 = 0;


                if (myGridView2.RowCount > 0)
                {
                    for (int i = 0; i < myGridView2.RowCount; i++)
                    {
                        if (myGridView2.GetRowCellValue(i, "Type").ToString() == "非提付异动")
                        {
                            if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent")) == 0)
                            {
                                MsgBox.ShowOK("第"+ i +"行本次核销金额不能为0!");
                                return;
                            }
                            if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent")) > ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "virefBalance")))
                            {
                                MsgBox.ShowOK("第"+ i +"行本次核销金额不能为大于核销余额!");
                            }
                            //sumAmount = sumAmount + ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "Amount"));
                            sumAmount = sumAmount + ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent"));
                            sBillNo = sBillNo + myGridView2.GetRowCellValue(i, "ID") + "@";
                            BillNos = BillNos + myGridView2.GetRowCellValue(i, "BillNo") + "@";
                            //sAmount = sAmount + myGridView2.GetRowCellValue(i, "Amount") + "@"; 
                            sAmount = sAmount + myGridView2.GetRowCellValue(i, "VirefCurrent") + "@";
                            sNowPayVerifBalance = sNowPayVerifBalance + myGridView2.GetRowCellValue(i, "FetchPayVerifBalance") + "@";
                            credentialsTime = Convert.ToDateTime(myGridView2.GetRowCellValue(i, "credentialsTime"));//hj 20180309可修改的核销日期
                            num++;
                        }
                        else
                        {
                            if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent")) == 0)
                            {
                                MsgBox.ShowOK("第" + i + "行本次核销金额不能为0!");
                                return;
                            }
                            if (ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent")) > ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "virefBalance")))
                            {
                                MsgBox.ShowOK("第" + i + "行本次核销金额不能为大于核销余额!");
                            }
                            //sumAmount1 = sumAmount1 + ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "Amount"));
                            sumAmount1 = sumAmount1 + ConvertType.ToFloat(myGridView2.GetRowCellValue(i, "VirefCurrent"));
                            sBillNo1 = sBillNo1 + myGridView2.GetRowCellValue(i, "ID") + "@";
                            BillNos1 = BillNos1 + myGridView2.GetRowCellValue(i, "BillNo") + "@";
                          //  sAmount1 = sAmount1 + myGridView2.GetRowCellValue(i, "Amount") + "@";
                            sAmount1 = sAmount1 + myGridView2.GetRowCellValue(i, "VirefCurrent") + "@";

                            sFetchPayVerifBalance1 = sFetchPayVerifBalance1 + myGridView2.GetRowCellValue(i, "FetchPayVerifBalance") + "@";
                            credentialsTime1 = Convert.ToDateTime(myGridView2.GetRowCellValue(i, "credentialsTime"));//hj 20180309可修改的核销日期
                            num1++;
                        }
                    }

                }
                else
                    return;

                string sShowOK = "非提付异动核销总票数：" + num
                         + "\r\n非提付异动核销总金额：" + ConvertType.ToString(sumAmount) + "\r\n提付异动核销总票数：" + num1
                         + "\r\n提付异动核销总金额：" + ConvertType.ToString(sumAmount1) + "\r\n异动核销人：" + CommonClass.UserInfo.UserName + "\r\n是否继续？";

                if (MsgBox.ShowYesNo(sShowOK) != DialogResult.Yes)
                {
                    return;
                }

                if (num == 0 && num1 == 0) return;

                if (num != 0)
                {

                    string subjectOne = "";
                    string subjectTwo = "";
                    string subjectThree = "";
                    string Verifydirection = "";
                    string summary = "";
                    string remarks = "";
                    if (CommonClass.UserInfo.companyid == "124")
                    {
                        frmChoiceSubject2 frm = new frmChoiceSubject2();
                        frm.xm = "非提付异动";
                        DialogResult result = frm.ShowDialog();
                        if (result == DialogResult.Cancel) return;
                        subjectOne = frm.SubjectOne;
                        subjectTwo = frm.SubjectTwo;
                        subjectThree = frm.SubjectThree;
                        Verifydirection = frm.Verifydirection;
                        summary = frm.Summary;
                        remarks = frm.Remarks;
                    }
                    else
                    {
                        frmChoiceSubject frm = new frmChoiceSubject();
                        frm.xm = "非提付异动";
                        DialogResult result = frm.ShowDialog();
                        if (result == DialogResult.Cancel) return;
                        subjectOne = frm.SubjectOne;
                        subjectTwo = frm.SubjectTwo;
                        subjectThree = frm.SubjectThree;
                        Verifydirection = frm.Verifydirection;
                        summary = frm.Summary;
                    }

                    string voucherNo = "I" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billNo", sBillNo));
                    list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                    list.Add(new SqlPara("Amount", sAmount));
                    list.Add(new SqlPara("AduitBillState", "非提付异动"));
                    list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                    list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                    list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                    list.Add(new SqlPara("NowPayVerifBalance", sNowPayVerifBalance));
                    list.Add(new SqlPara("subjectOne", subjectOne));
                    list.Add(new SqlPara("subjectTwo", subjectTwo));
                    list.Add(new SqlPara("subjectThree", subjectThree));
                    list.Add(new SqlPara("Verifydirection", Verifydirection));
                    list.Add(new SqlPara("summary", summary));
                    list.Add(new SqlPara("Remarks", remarks));
                    list.Add(new SqlPara("voucherNo", voucherNo));
                    list.Add(new SqlPara("BillNos", BillNos));
                    list.Add(new SqlPara("credentialsTime", credentialsTime));//HJ20180331
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_TRANSACTIONADUIT_NOWPAY_GXSJ", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        ds1.Clear();
                    }
                }
                if (num1 != 0)
                {
                    string subjectOne1 = "";
                    string subjectTwo1 = "";
                    string subjectThree1 = "";
                    string Verifydirection1 = "";
                    string summary1 = "";
                    string remarks1 = "";
                    if (CommonClass.UserInfo.companyid == "124")
                    {
                        frmChoiceSubject2 frm = new frmChoiceSubject2();
                        frm.xm = "提付异动";
                        DialogResult result = frm.ShowDialog();
                        if (result == DialogResult.Cancel) return;
                        subjectOne1 = frm.SubjectOne;
                        subjectTwo1 = frm.SubjectTwo;
                        subjectThree1 = frm.SubjectThree;
                        Verifydirection1 = frm.Verifydirection;
                        summary1 = frm.Summary;
                        remarks1 = frm.Remarks;
                    }
                    else
                    {
                        frmChoiceSubject frm = new frmChoiceSubject();
                        frm.xm = "提付异动";
                        DialogResult result = frm.ShowDialog();
                        if (result == DialogResult.Cancel) return;
                        subjectOne1 = frm.SubjectOne;
                        subjectTwo1 = frm.SubjectTwo;
                        subjectThree1 = frm.SubjectThree;
                        Verifydirection1 = frm.Verifydirection;
                        summary1 = frm.Summary;
                    }

                    string voucherNo = "I" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billNo", sBillNo1));
                    list.Add(new SqlPara("AduitDate", CommonClass.gcdate));
                    list.Add(new SqlPara("Amount", sAmount1));
                    list.Add(new SqlPara("AduitBillState", "提付异动"));
                    list.Add(new SqlPara("AduitMan", CommonClass.UserInfo.UserName));
                    list.Add(new SqlPara("AduitSite", CommonClass.UserInfo.SiteName));
                    list.Add(new SqlPara("AduitWeb", CommonClass.UserInfo.WebName));
                    list.Add(new SqlPara("AduitCause", CommonClass.UserInfo.CauseName));
                    list.Add(new SqlPara("AduitArea", CommonClass.UserInfo.AreaName));
                    list.Add(new SqlPara("FetchPayVerifBalance", sFetchPayVerifBalance1));
                    list.Add(new SqlPara("subjectOne", subjectOne1));
                    list.Add(new SqlPara("subjectTwo", subjectTwo1));
                    list.Add(new SqlPara("subjectThree", subjectThree1));
                    list.Add(new SqlPara("Verifydirection", Verifydirection1));
                    list.Add(new SqlPara("summary", summary1));
                    list.Add(new SqlPara("Remarks", remarks1));
                    list.Add(new SqlPara("voucherNo", voucherNo));
                    list.Add(new SqlPara("BillNos", BillNos1));
                    list.Add(new SqlPara("credentialsTime", credentialsTime1));//HJ20180331
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_TRANSACTIONADUIT_FETCHPAY_GXSJ", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        ds1.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);

            }
            //VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();
            //voaDeel.SubmitVerify(myGridView2, ds1, VerifyType.送货费.ToString(), "BillNO", "SendBatch", "CurrentVerifyFee", "AccSendLast", "支出");
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem15_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem16_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem17_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem18_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }


        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (splitContainerControl1.Horizontal == true)
            {
                splitContainerControl1.Horizontal = false;
                splitContainerControl1.SplitterPosition = 480;
                return;
            }
            if (splitContainerControl1.Horizontal == false)
            {
                splitContainerControl1.Horizontal = true;
                splitContainerControl1.SplitterPosition = 800;
                return;
            }
        }
    }
}