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

namespace ZQTMS.UI
{
    public partial class frmAgentMiddleList : BaseForm
    {
        int type ;// 提取类型

        public frmAgentMiddleList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gettype">0本地代理,1终端代理</param>
        public frmAgentMiddleList(string gettype)
        {
            InitializeComponent();
            //try
            //{
            //    this.gettype = ConvertType.ToInt32(gettype);
            //}
            //catch { this.gettype = 0; }

            //this.Text = (this.gettype == 0 ? "本地中转" : "终端中转") + "-中转记录列表";
        }

        private void frmAgentMiddleList_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
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
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        //单票中转
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // FrmBillMiddle_Detail detail = FrmBillMiddle_Detail detail(gettype);
            //detail.MdiParent = this.MdiParent;
            //detail.Dock = DockStyle.Fill;
            //detail.Show();
            //detail.Focus();
            FrmBillMiddle_Detail detail = new FrmBillMiddle_Detail();
            detail.MdiParent = this.MdiParent;
            detail.Dock = DockStyle.Fill;
            detail.Show();
            detail.Focus();
        }
        //批量中转
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmBillBatchMiddle fm = frmBillBatchMiddle.Get_frmBillBatchMiddle(gettype);
            //fm.MdiParent = this.MdiParent;
            //fm.Dock = DockStyle.Fill;
            //fm.Show();
            //fm.Focus();
            frmBillBatchMiddle fm = new frmBillBatchMiddle();
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
        //修改
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            //判断运单是否锁定
            if (CommonClass.QSP_LOCK_6(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BillNo").ToString(),
                myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "MiddleDate").ToString()))
            {
                return;
            }

            try
            {
                frmBillMiddleInfo fbm = new frmBillMiddleInfo();
                //fbm.Gettype = gettype;
                fbm.BillNo = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNo"));
                fbm.CSAccMiddlePay = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "AccMiddlePay"));//hj20180724
                fbm.type = myGridView1.GetRowCellValue(rowhandle, "MiddleType").ToString();
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

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string bsite = edbsite.Text.Trim();
            string web = edWeb.Text.Trim();
            string cause = Cause.Text.Trim();
            string area = Area.Text.Trim();
            bsite = bsite == "全部" ? "%%" : bsite;
            web = web == "全部" ? "%%" : web;
            cause = cause == "全部" ? "%%" : cause;
            area = area == "全部" ? "%%" : area;
            if (inventory.Text.Trim().ToString() == "始发库存")
            {
                type = 0;
            }
            else if (inventory.Text.Trim().ToString() == "到货库存")
            {
                type = 1;
            }
            else
            {
                type = 2;
            }


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("t1", bdate.DateTime));
            list.Add(new SqlPara("t2", edate.DateTime));
            list.Add(new SqlPara("StartSite", bsite));
            list.Add(new SqlPara("datetype", comboBoxEdit1.SelectedIndex));
            list.Add(new SqlPara("WebType", comboBoxEdit2.SelectedIndex));
            list.Add(new SqlPara("type", type));//0本地 1终端 2全部
            list.Add(new SqlPara("WebName", web));
            list.Add(new SqlPara("CauseName", cause));
            list.Add(new SqlPara("AreaName", area));
            //list.Add(new SqlPara("inventory", inventory.Text.Trim() == "全部" ? "%%" : inventory.Text.Trim()));//库存类型

            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_AGENT_MIDDLE_LIST", list);
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

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(edWeb, Cause.Text, Area.Text);
        }
        //取消中转
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
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

                CommonSyn.MiddleCancel(billNo);//zaj 2018-4-12 分拨同步
                CommonSyn.TimeCancelSyn((billNo + "@"), "", "", "USP_DELETE_MIDDLE_RECORD_NEW");//时效取消同步 LD 2018-4-27
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

            string middlelist = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;  //maohui20180324
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT", new List<SqlPara> { new SqlPara("MiddleBatch", middleBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                row["NowCompany"] = CommonClass.UserInfo.gsqc;
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang(middlelist, ds);
            fpr.ShowDialog();
        }
        //打印中转清单（按公司）
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string MiddleCarrier = GridOper.GetRowCellValueString(myGridView1, "MiddleCarrier");//中转批次
            string BillNo = GridOper.GetRowCellValueString(myGridView1, "BillNo");//运单号
            string type1 = myGridView1.GetFocusedRowCellValue("MiddleType").ToString();
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
            //list.Add(new SqlPara("gettype", gettype));
            list.Add(new SqlPara("gettype", type1));
            list.Add(new SqlPara("WebName", web));
            list.Add(new SqlPara("CauseName", cause));
            list.Add(new SqlPara("AreaName", area));
            list.Add(new SqlPara("MiddleCarrier", MiddleCarrier));
            if (MiddleCarrier == "") return;

            string middlelist = CommonClass.UserInfo.MiddleList == "" ? "中转清单" : CommonClass.UserInfo.MiddleList;  //maohui20180324
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILL_MIDDLE_PRINT_CPY", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                 row["NowCompany"] = CommonClass.UserInfo.gsqc;
            }

            frmPrintRuiLang fpr = new frmPrintRuiLang(middlelist, ds);
            fpr.ShowDialog();
        }

        private void myGridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)//maohui20180605
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
           // if (this.gettype == 1)  //(终端中转)当字段“中转毛利”小于0时，则标注红色
            if (Convert.ToInt32(view.GetRowCellValue(e.RowHandle, "MiddleType"))== 1)  //(终端中转)当字段“中转毛利”小于0时，则标注红色
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
    }
}