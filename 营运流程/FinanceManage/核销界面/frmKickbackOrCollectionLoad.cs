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
using System.Threading;
using ZQTMS.Lib;

namespace ZQTMS.UI
{
    public partial class frmKickbackOrCollectionLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        BarEditItem barEditCause = new BarEditItem(); //生成事业部
        BarEditItem barEditArea = new BarEditItem(); //生成大区
        BarEditItem barEditWeb = new BarEditItem(); //生成网点
        GridHitInfo hitInfo;
        public frmKickbackOrCollectionLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        //加载
        private void frmKickbackOrCollectionLoad_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);  //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView2);

            GridOper.GetGridViewColumn(myGridView1, "VerifMoney").AppearanceCell.BackColor = Color.Yellow;
            GridOper.GetGridViewColumn(myGridView1, "isIncom").AppearanceCell.BackColor = Color.Yellow;
            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;

            repositoryItemComboBox1.Properties.Items.Add("一个月");
            repositoryItemComboBox1.Properties.Items.Add("三个月");
            repositoryItemComboBox1.Properties.Items.Add("六个月");
            repositoryItemComboBox1.Properties.Items.Add("一年");

            repositoryItemComboBox3.Properties.Items.Add("回扣");
            repositoryItemComboBox3.Properties.Items.Add("代收货款");
        }

        int flag = 1;
        private void getdata()
        {
            try
            {
                //hj20180825
                string dateDiff = barEditItem3.EditValue.ToString();
           
                    switch (dateDiff)
                    {
                        case "一个月": flag = 1; break;
                        case "三个月": flag = 3; break;
                        case "六个月": flag = 6; break;
                        case "一年": flag = 12; break;
                    }
                
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
                //list.Add(new SqlPara("selectType", 1));
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("datediff", flag));
                list.Add(new SqlPara("VerifyOffType", barEditItem5.EditValue.ToString()));//核销类型

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DiscountTransfer_Or_CollectionPay_Load", new List<SqlPara>(list));
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];

                if (ds == null || ds.Tables.Count == 0) return;
                Thread th = new Thread(() =>
                {
                    list[0].ParaValue = 2;
                    DataTable dt = SqlHelper.GetDataTable(new SqlParasEntity(OperType.Query, "QSP_GET_DiscountTransfer_Or_CollectionPay_Load", list));

                    if (dt == null || dt.Rows.Count == 0) return;

                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        ds.Tables[0].Merge(dt.Copy());
                    });
                });
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

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

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.QuickSearch();
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
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
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
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'" + " OR [BillNo] LIKE" + "'%" + szfilter + "%'", "");
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
            if (this.myGridView2.DataRowCount == 0)
            {
                MsgBox.ShowOK("没有需要核销的数据！");
                return;
            }
            VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();
            if (this.barEditItem5.EditValue.ToString() == "回扣")
            {
                voaDeel.SubmitVerify(myGridView2, ds1, VerifyType.折扣费.ToString(), "BillNo", null, "CurrentVerifyFee", "VerifBalance", "支出");
                //if (!SubmitVerify(myGridView2, ds1, VerifyType.折扣费.ToString(), "BillNo", null, "CurrentVerifyFee", "VerifBalance"))
                //{
                //    return;
                //}
                //if (MsgBox.ShowYesNo("是否打印报表？") == DialogResult.Yes)
                //{
                //    frmPrintRuiLangWithCondition frm = new frmPrintRuiLangWithCondition("6");
                //    frm.Owner = this;
                //    frm.Show();
                //}
            }
            else
            {
                voaDeel.SubmitVerify(myGridView2, ds1, VerifyType.货款回收.ToString(), "BillNo", null, "CurrentVerifyFee", "VerifBalance", "支出");
                //if (!SubmitVerify(myGridView2, ds1, VerifyType.货款回收.ToString(), "BillNo", null, "CurrentVerifyFee", "VerifBalance"))
                //{
                //    return;
                //}
                //if (MsgBox.ShowYesNo("是否打印报表？") == DialogResult.Yes)
                //{
                //    frmPrintRuiLangWithCondition frm = new frmPrintRuiLangWithCondition("22");
                //    frm.Owner = this;
                //    frm.Show();
                //}
            }
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

        //int flag = 1;
        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            string dateDiff = barEditItem3.EditValue.ToString();
            try
            {
                switch (dateDiff)
                {
                    case "一个月": flag = 1; break;
                    case "三个月": flag = 3; break;
                    case "六个月": flag = 6; break;
                    case "一年": flag = 12; break;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 提交核销数据
        /// </summary>
        /// <param name="myGridView2"></param>
        /// <param name="ds1"></param>
        /// <param name="verifyType"></param>
        /// <param name="column1"></param>
        /// <param name="column2"></param>
        /// <param name="column3"></param>
        /// <param name="balanceName"></param>
        public bool SubmitVerify(MyGridView myGridView2, DataSet ds1, string verifyType, string column1, string column2, string column3, string balanceName)
        {
            myGridView2.PostEditor();
            int rowhandle = myGridView2.FocusedRowHandle;
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何需要核销的清单，请先在左侧列表构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                decimal balance = Convert.ToDecimal(myGridView2.GetRowCellValue(i, balanceName));
                decimal currentVerifyFee = Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee"));
                if (currentVerifyFee == 0)
                {
                    XtraMessageBox.Show("请输入本次核销金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee")) > 0 && Convert.ToString(myGridView2.GetRowCellValue(i, "FeeType")).Equals("大车油料费"))
                {
                    MsgBox.ShowError(Convert.ToString(myGridView2.GetRowCellValue(i, column2)) + "批次大车油料费核销金额不应大于零！");
                    return false;
                }
                if (Convert.ToString(myGridView2.GetRowCellValue(i, "FeeType")).Equals("大车司机奖罚费"))
                {
                    if ((Convert.ToDecimal(myGridView2.GetRowCellValue(i, "AccNowPayVerif")) < 0) && Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee")) > 0)
                    {
                        MsgBox.ShowError(Convert.ToString(myGridView2.GetRowCellValue(i, column2)) + "批次大车司机奖罚费核销金额不应大于零！");
                        return false;
                    }
                    if ((Convert.ToDecimal(myGridView2.GetRowCellValue(i, "AccNowPayVerif")) < 0) && Convert.ToDecimal(myGridView2.GetRowCellValue(i, "CurrentVerifyFee")) < 0)
                    {
                        if (Math.Abs(currentVerifyFee) > Math.Abs(balance))
                        {
                            XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    if (Convert.ToDecimal(myGridView2.GetRowCellValue(i, "AccNowPayVerif")) > 0)
                    {
                        if (currentVerifyFee > balance)
                        {
                            XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                if (!Convert.ToString(myGridView2.GetRowCellValue(i, "FeeType")).Equals("大车司机奖罚费"))
                {
                    if (currentVerifyFee > balance)
                    {
                        XtraMessageBox.Show("本次核销余额不能大于当前余额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            frmChoiceSubject frm = new frmChoiceSubject();
            frm.xm = verifyType;
            DialogResult result = frm.ShowDialog();
            if (result == DialogResult.Cancel) return false;
            string subjectOne = frm.SubjectOne;
            string subjectTwo = frm.SubjectTwo;
            string subjectThree = frm.SubjectThree;
            string summary = frm.Summary;
            if (!string.IsNullOrEmpty(subjectOne) && !string.IsNullOrEmpty(subjectTwo) && !string.IsNullOrEmpty(subjectThree))
            {
                string billNoStr = "";
                string batchNoStr = "";
                string moneyStr = "";
                string feeTypes = "";
                string WebNames = "";
                string HandNums = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (!string.IsNullOrEmpty(column1))
                    {
                        billNoStr += myGridView2.GetRowCellValue(i, column1) + "@";
                    }
                    if (!string.IsNullOrEmpty(column2))
                    {
                        batchNoStr += myGridView2.GetRowCellValue(i, column2) + "@";
                    }
                    moneyStr += myGridView2.GetRowCellValue(i, column3) + "@";
                    if (verifyType == "大车费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "FeeType") + "@";
                    }
                    if (verifyType == "始发其他费" || verifyType == "终端其他费")
                    {
                        feeTypes += myGridView2.GetRowCellValue(i, "Project") + "@";
                    }
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "大车终端装卸费")
                    {
                        WebNames += ConvertType.ToString(myGridView2.GetRowCellValue(i, "WebName")) + "@";
                    }
                    if ((ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "始发装卸费" || ConvertType.ToString(myGridView2.GetRowCellValue(i, "FeeType")) == "终端装卸费")
                             && CommonClass.UserInfo.UserDB != UserDB.ZQTMS3PL)
                    {
                        HandNums += ConvertType.ToString(myGridView2.GetRowCellValue(i, "HnadNum")) + "@";
                    }
                }

                AddVerifyOffAccount(subjectOne, subjectTwo, subjectThree, summary, verifyType, "支出", billNoStr, batchNoStr, moneyStr, feeTypes, WebNames, HandNums);
                ds1.Clear();
            }
            return true;
        }

        /// 修改核销现金日记账
        /// </summary>
        /// <param name="oneSubject">一级科目</param>
        /// <param name="twoSubject">二级科目</param>
        /// <param name="threeSubject">三级科目</param>
        /// <param name="summary">摘要</param>
        /// <param name="verifyType">核销费用类型</param>
        /// <param name="inOutType">记账类型 支出、收入</param>
        /// <param name="billNos">运单号字符串 多个用'@'隔开</param>
        /// <param name="moneys">金额字符串 多个用'@'隔开</param>
        public void AddVerifyOffAccount(string oneSubject, string twoSubject, string threeSubject, string summary, string verifyType, string inOutType, string billNos, string batchNos, string moneys, string feeTypes, string WebNames, string HandNums)
        {
            try
            {
                string voucherNo = (inOutType == "支出" ? "O" : "I") + DateTime.Now.ToString("yyyyMMddHHmmss");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNo", voucherNo));
                list.Add(new SqlPara("OneSubject", oneSubject));
                list.Add(new SqlPara("TwoSubject", twoSubject));
                list.Add(new SqlPara("ThreeSubject", threeSubject));
                list.Add(new SqlPara("Summary", summary));
                list.Add(new SqlPara("VerifyOffType", verifyType));
                list.Add(new SqlPara("InOutType", inOutType));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("OptMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("BillNos", billNos));
                list.Add(new SqlPara("BatchNos", batchNos));
                list.Add(new SqlPara("Moneys", moneys));
                list.Add(new SqlPara("FeeTypes", feeTypes));
                list.Add(new SqlPara("WebNames", WebNames));
                list.Add(new SqlPara("HandNums", HandNums));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPT_Automatic_VerifyOffAccount", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    #region 新增银行平台信息
                    if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                    {
                        string bankman = string.Empty;
                        string bankcode = string.Empty;
                        string bankname = string.Empty;
                        string sheng = string.Empty;
                        string city = string.Empty;
                        string opertype = string.Empty;
                        string accout = string.Empty;
                        //string billdate = string.Empty;
                        string outtype = string.Empty;
                        string bankchild = string.Empty;
                        string reportnetwork = string.Empty;
                        string project = string.Empty;
                        string phone = string.Empty;
                        int i = 0;
                        foreach (DataRow row in ds1.Tables[0].Rows)
                        {
                            if (row["BalanceFeeType"].ToString() == "抵账" || row["BalanceFeeType"].ToString() == "")//抵账不插入银行信息
                            {
                                continue;
                            }
                            bankman = bankman + row["BalanceName"].ToString() + "@";
                            bankcode = bankcode + row["BalanceAccount"].ToString() + "@";
                            bankname = bankname + row["BalanceBank"].ToString() + "@";
                            sheng = sheng + row["BalanceProvice"].ToString() + "@";
                            city = city + row["BalanceCity"].ToString() + "@";
                            opertype = opertype + row["BalanceType"].ToString() + "@";
                            accout = accout + row["VerifMoney"].ToString() + "@";
                            //billdate = billdate + row["BillDate"].ToString() + "@";
                            outtype = outtype + row["BalancePaymentType"].ToString() + "@";
                            bankchild = bankchild + row["BalanceBranch"].ToString() + "@";
                            reportnetwork = reportnetwork + row["BegWeb"].ToString() + "@";
                            project = project + row["BalanceProject"].ToString() + "@";
                            phone = phone + row["BalancePhone"].ToString() + "@";
                            i++;
                        }
                        if (i > 0)
                        {
                            List<SqlPara> list_bank = new List<SqlPara>();

                            list_bank.Add(new SqlPara("bankman", bankman));//开户姓名
                            list_bank.Add(new SqlPara("bankcode", bankcode));//银行账号
                            list_bank.Add(new SqlPara("bankname", bankname));//开户银行
                            list_bank.Add(new SqlPara("sheng", sheng));  //所属省份
                            list_bank.Add(new SqlPara("city", city));//所属城市
                            list_bank.Add(new SqlPara("opertype", opertype));//转账类型
                            list_bank.Add(new SqlPara("accout", accout));//支出金额
                            //list_bank.Add(new SqlPara("billdate", billdate));//付款日期
                            list_bank.Add(new SqlPara("outtype", outtype));//支出类型
                            list_bank.Add(new SqlPara("bankchild", bankchild));//开户支行
                            list_bank.Add(new SqlPara("reportnetwork", reportnetwork));//申报部门
                            list_bank.Add(new SqlPara("project", project));//项目
                            list_bank.Add(new SqlPara("phone", phone));//联系方式
                            list_bank.Add(new SqlPara("remark", "回扣/代收货款核销"));//备注
                            list_bank.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));  //创建人
                            list_bank.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));//创建站点
                            list_bank.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));//创建网点

                            sps = new SqlParasEntity(OperType.Query, "USP_ADD_BANK_ALL", list_bank);
                            SqlHelper.ExecteNonQuery(sps);
                        }
                    }
                    #endregion
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
