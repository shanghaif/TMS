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
using DevExpress.XtraEditors;
using System.IO;

namespace ZQTMS.UI
{
    public partial class FrmBillMiddleList : BaseForm
    {
        int gettype = 0;// 中转类型

        public FrmBillMiddleList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gettype">0本地代理,1终端代理</param>
        public FrmBillMiddleList(string gettype)
        {
            InitializeComponent();
            try
            {
                this.gettype = ConvertType.ToInt32(gettype);
            }
            catch { this.gettype = 0; }

            this.Text = (this.gettype == 0 ? "本地中转" : "终端中转") + "-中转记录列表";
        }

        private void FrmBillMiddleList_Load(object sender, EventArgs e)
        {
            if (this.gettype == 0)
            {
                CommonClass.InsertLog("本地中转");//xj2019/5/28
            }
            else {
                CommonClass.InsertLog("终端中转");//xj2019/5/28
            }
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1);

            bdate.DateTime = CommonClass.gbdate.AddDays(-2);
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(edbsite, true);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, CommonClass.UserInfo.CauseName);

            edbsite.Text = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            edWeb.Text = CommonClass.UserInfo.WebName;

       
            if (CommonClass.UserInfo.companyid != "309" && CommonClass.UserInfo.companyid != "490")
            {
                barButtonItem15.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                barButtonItem14.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
            else
            {
                GridOper.CreateStyleFormatCondition(myGridView1, "MiddleAduitState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已审核", Color.Red);  //zb20190617
            }

        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        //单票中转
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmBillMiddle_Detail detail = FrmBillMiddle_Detail.Get_FrmBillMiddle_Detail(gettype);
            detail.MdiParent = this.MdiParent;
            detail.Dock = DockStyle.Fill;
            detail.Show();
            detail.Focus();
        }
        //批量中转
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBillBatchMiddle fm = frmBillBatchMiddle.Get_frmBillBatchMiddle(gettype);
            fm.MdiParent = this.MdiParent;
            fm.Dock = DockStyle.Fill;
            fm.Show();
            fm.Focus();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (CommonClass.QSP_LOCK_6(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BillNo").ToString(),
                myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "MiddleDate").ToString()))
            {
                return;
            }
            string middleaduitstate = GridOper.GetRowCellValueString(myGridView1, rowhandle, "MiddleAduitState"); //zb20190611
            try
            {
                frmBillMiddleInfo fbm = new frmBillMiddleInfo();
                fbm.Gettype = gettype;
                fbm.MiddleAduitState = middleaduitstate;
                fbm.BillNo = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNo"));
                fbm.CSAccMiddlePay = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "AccMiddlePay"));//hj20180724
                fbm.Ismodify = true;
                fbm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "中转记录");
        }
        private void getdate()
        {
            string bsite = edbsite.Text.Trim();
            string web = edWeb.Text.Trim();
            string cause = Cause.Text.Trim();
            string area = Area.Text.Trim();
            bsite = bsite == "全部" ? "%%" : bsite;
            web = web == "全部" ? "%%" : web;
            cause = cause == "全部" ? "%%" : cause;
            area = area == "全部" ? "%%" : area;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("StartSite", bsite));
            list.Add(new SqlPara("datetype", comboBoxEdit1.SelectedIndex));
            list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
            list.Add(new SqlPara("gettype", gettype));
            list.Add(new SqlPara("WebName", web));
            list.Add(new SqlPara("CauseName", cause));
            list.Add(new SqlPara("AreaName", area));

            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_LIST", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdate();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
              int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            //309/490公司限制已审核不能做取消中转的操作
            string middleaduitstate = GridOper.GetRowCellValueString(myGridView1, rowhandle, "MiddleAduitState");
        
            if (CommonClass.QSP_LOCK_6(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BillNo").ToString(),
                myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "MiddleDate").ToString()))
            {
                return;
            }
            if (MsgBox.ShowYesNo("确定取消中转？") != DialogResult.Yes) return;
            try
            {
                string billNo=myGridView1.GetFocusedRowCellValue("BillNo").ToString();
                List<SqlPara> list = new List<SqlPara>(new SqlPara[] { new SqlPara("BillNo", ConvertType.ToLong(myGridView1.GetFocusedRowCellValue("BillNo"))) });
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_MIDDLE_RECORD_NEW", list)) == 0) return;
                myGridView1.DeleteRow(myGridView1.FocusedRowHandle);
                MsgBox.ShowOK();

                //CommonSyn.MiddleCancel(billNo);//zaj 2018-4-12 分拨同步
                //CommonSyn.TimeCancelSyn((billNo + "@"), "", "", "USP_DELETE_MIDDLE_RECORD_NEW");//时效取消同步 LD 2018-4-27
                //yzw 取消中转新
                CommonSyn.DELETE_MIDDLE_RECORD_NEW_SYN(billNo);
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
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

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string middleBatch = GridOper.GetRowCellValueString(myGridView1, "MiddleBatch");//中转批次
            string BillNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");//运单号

            middleBatch = middleBatch == "" ? BillNo : middleBatch;
            if (middleBatch == "") return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("MiddleBatchs", middleBatch + "@"));
            list.Add(new SqlPara("billnos", BillNo + "@"));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
         
            string MiddleList = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;//zaj 装车清单
            if (File.Exists(Application.StartupPath + "\\Reports\\" + MiddleList + "per.grf"))
            {
                MiddleList = MiddleList + "per";
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang(MiddleList, ds);
            fpr.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string MiddleCarrier = GridOper.GetRowCellValueString(myGridView1, "MiddleCarrier");//中转批次
            string BillNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");//运单号
            string bsite = edbsite.Text.Trim();
            string web = edWeb.Text.Trim();
            string cause = Cause.Text.Trim();
            string area = Area.Text.Trim();
            bsite = bsite == "全部" ? "%%" : bsite;
            web = web == "全部" ? "%%" : web;
            cause = cause == "全部" ? "%%" : cause;
            area = area == "全部" ? "%%" : area;

            MiddleCarrier = MiddleCarrier == "" ? BillNo : MiddleCarrier;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("StartSite", bsite));
            list.Add(new SqlPara("datetype", comboBoxEdit1.SelectedIndex));
            list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
            list.Add(new SqlPara("gettype", gettype));
            list.Add(new SqlPara("WebName", web));
            list.Add(new SqlPara("CauseName", cause));
            list.Add(new SqlPara("AreaName", area));
            list.Add(new SqlPara("MiddleCarrier", MiddleCarrier));
            if (MiddleCarrier == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT_CPY", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
          
            string MiddleList = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;//zaj 装车清单
            if (File.Exists(Application.StartupPath + "\\Reports\\" + MiddleList + "per.grf"))
            {
                MiddleList = MiddleList + "per";
            }
            frmPrintRuiLang fpr = new frmPrintRuiLang(MiddleList, ds);
            fpr.ShowDialog();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string allMiddleBatch = "";
            if (rowhandle >= 0)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        allMiddleBatch += myGridView1.GetRowCellValue(i, "BillNo") + ",";
                        if (allMiddleBatch == "") return;
                    }
                }
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("MiddleBatch", allMiddleBatch));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_BILL_ZhongSong_PRINT1", list);
            DataSet ds = SqlHelper.GetDataSet(spe);


            string MiddleList =  "中转清单" ;
            if (File.Exists(Application.StartupPath + "\\Reports\\" + MiddleList + "per.grf"))
            {
                MiddleList = MiddleList + "per";
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang(MiddleList, ds);
            fpr.ShowDialog();


        }

        private void myGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)//maohui20180605
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            if (this.gettype == 1)  //(终端中转)当字段“中转毛利”小于0时，则标注红色
            {
                if (e.RowHandle >= 0)
                {
                    if (e.Column.FieldName == "zzml")
                    {
                        if (Convert.ToDecimal(view.GetRowCellValue(e.RowHandle, "zzml")) < 0)
                        {
                            e.Appearance.BackColor = Color.Red;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
        }

        //审核
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)     
        {
            myGridView1.PostEditor();
            try
            {
                string AduitType = "中转费审核";
                string billnos = "";
                if (MsgBox.ShowYesNo("是否确定审核？") != DialogResult.Yes) return;
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (Convert.ToInt32(myGridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        string MiddleAduitState = myGridView1.GetRowCellValue(i, "MiddleAduitState").ToString();
                        if (MiddleAduitState == "已审核")
                        {
                            MsgBox.ShowError("存在单号已做过中转费审核，不能重复审核！");
                            return;
                        }
                        billnos += myGridView1.GetRowCellValue(i, "BillNo") + "@";
                    }
                }
              
                if (string.IsNullOrEmpty(billnos))
                {
                    MsgBox.ShowError("未找到需要审核的数据！");
                    return;
                }
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", edWeb.Text.Trim() == "全部" ? "%%" : edWeb.Text.Trim()));
                list.Add(new SqlPara("AduitType", AduitType));
                list.Add(new SqlPara("BillNos", billnos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_MIDDLEADUITSTATE_ByBillNos", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            getdate();
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            string AduitType = "中转费反审核";
            if (row < 0)
            {
                MsgBox.ShowError("请选择一条信息！");
                return;
            }
            if (MsgBox.ShowYesNo("是否确定反审核？") != DialogResult.Yes) return;
            string BillNo = GridOper.GetRowCellValueString(myGridView1, myGridView1.FocusedRowHandle, "BillNo");
            string MiddleAduitState = GridOper.GetRowCellValueString(myGridView1, "MiddleAduitState");
            if (MiddleAduitState == "未审核")
            {
                MsgBox.ShowError("该单号还未做审核！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", edWeb.Text.Trim() == "全部" ? "%%" : edWeb.Text.Trim()));
                list.Add(new SqlPara("AduitType", AduitType));
                list.Add(new SqlPara("BillNo", BillNo));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_MIDDLEADUITSTATE", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            getdate();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "isChecked", true);
                }
            }
            if (checkBox1.Checked == false)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "isChecked", false);
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            ///中转费运费与中转费区别：中转费比例  批量中转中转费字段取值逻辑
            MsgBox.ShowOK("中转毛利：“如果是本地代理，为实收运费-中转费；否则为结算送货费+结算中转费+ 终端结算费 -中转费”"
             + "\n" + "中转费比例：“如果实收运费为0，则为0，否则为(中转费/实收运费)*100%”"
             + "\n" + "摊分中转费：“等于中转费”"
              + "\n" + "转送费：“网点送货至中转公司的车费”"
              + "\n" + "转送回扣：“提付-中转费”"
              + "\n" + "批量中转中转费段取值逻辑：“批量中转/中转费为该批中转的所有单总中转费，按系统5种费用分摊方式进行分摊”");
        }
    }
}