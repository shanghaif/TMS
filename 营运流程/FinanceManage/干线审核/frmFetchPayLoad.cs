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
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class frmFetchPayLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo;
        public string bdate;
        public string edate;

        public frmFetchPayLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void getdata()
        {
            myGridControl1.DataSource = myGridControl2.DataSource = null;
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();
                string strProcName = "QSP_GETFETCHPAY_InFee_Load_KETONG";
                if (CommonClass.IsZhanQuCompanyId(CommonClass.UserInfo.companyid))//战区查询
                {
                    strProcName = "QSP_GETFETCHPAY_InFee_Load_ZhanQu";
                }

                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                string TransferSite = barEditTransferSite == null || barEditTransferSite == null ? CommonClass.UserInfo.SiteName : barEditTransferSite.EditValue.ToString();
                CauseName = CauseName == "全部" ? "%%" : CauseName;
                AreaName = AreaName == "全部" ? "%%" : AreaName;
                WebName = WebName == "全部" ? "%%" : WebName;
                TransferSite = TransferSite == "全部" ? "%%" : TransferSite;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("TransferSite", TransferSite));

                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));
                list.Add(new SqlPara("IsOut", "已出库"));
                list.Add(new SqlPara("FetchPayType", barEditItem6.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, strProcName, list);
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

            CommonClass.Create_BarEditItem_TransferSite(barManager1, bar1, barEditTransferSite);
            CommonClass.Create_BarEditItem_Web(barManager1, bar1, barEditWeb);
            CommonClass.Create_BarEditItem_Area(barManager1, bar1, barEditArea);
            CommonClass.Create_BarEditItem_Cause(barManager1, bar1, barEditCause);
            barEditCause.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);
            barEditArea.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged_2);
            barEditCause.EditValue = CommonClass.UserInfo.CauseName;
            barEditArea.EditValue = CommonClass.UserInfo.AreaName;
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            barEditTransferSite.EditValue = CommonClass.UserInfo.SiteName;
            barEditItem3.EditValue = bdate;       //ywc20190315 
            barEditItem4.EditValue = edate;  //ywc20190315 
            if (CommonClass.UserInfo.companyid == "309" || CommonClass.UserInfo.companyid == "490")  //zb20190521
            {
                repositoryItemComboBox1.Items.Add("已出库");   //zb20190521
            }
            else
            {
                repositoryItemComboBox1.Items.Add("全部");      //zb20190430
                repositoryItemComboBox1.Items.Add("已出库");   //zb20190430
                repositoryItemComboBox1.Items.Add("未出库");   //zb20190430
            }
            repositoryItemComboBox3.Items.Add("全部");
            repositoryItemComboBox3.Items.Add("提付");
            repositoryItemComboBox3.Items.Add("中转提付");
            this.panelControl1.Hide();//PK2019-3-27


            splitContainerControl1.Horizontal = false;
            splitContainerControl1.SplitterPosition = 480;
        }

        BarEditItem barEditCause = new BarEditItem(); //生成事业部
        BarEditItem barEditArea = new BarEditItem(); //生成大区
        BarEditItem barEditWeb = new BarEditItem(); //生成网点
        BarEditItem barEditTransferSite = new BarEditItem();//生成中转地--maohui20180306

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
            //BillAccountDeel voaDeel = new BillAccountDeel();
            //voaDeel.SubmitVerify(myGridView2, ds1, BillVerifyType.提付.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");
            #region 限制未执行改单，不能核销 zb20190507
            try
            {
                StringBuilder sb = new StringBuilder();
                List<SqlPara> list = new List<SqlPara>();
                //获取批量运单号
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    sb.Append(myGridView2.GetRowCellValue(i, "BillNo"));
                    sb.Append("@");
                }
                list.Add(new SqlPara("BillNos", sb));
                //list.Add(new SqlPara("BillNos", "4407588@4385539@4460360@14254065@")); 测试
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_ApplyStateByBillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                sb.Remove(0, sb.Length);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string BillNO = ds.Tables[0].Rows[i]["BillNO"].ToString();
                        sb.Append(BillNO);
                        sb.Append(",");
                        MsgBox.ShowOK("单号:" + sb + "未执行改单/提付转欠款/提付转月结申请,不能进行核销，请先执行!");
                        return;

                    }
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
            #endregion
            VerifyOffAccountDeel voaDeel = new VerifyOffAccountDeel();
            voaDeel.SubmitVerify(myGridView2, ds1, VerifyType.提付.ToString(), "BillNo", null, "CurrentVerifyFee", "AmountLeft", "收入");

           
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

        private void barButtonItem23_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridControl1.DataSource = myGridControl2.DataSource = null;
            try
            {
                ds.Clear();
                ds1.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();

                List<SqlPara> list = new List<SqlPara>();

                string TransferSite = barEditTransferSite == null || barEditTransferSite == null ? CommonClass.UserInfo.SiteName : barEditTransferSite.EditValue.ToString();
                TransferSite = TransferSite == "全部" ? "%%" : TransferSite;
                if (barEditItem8.EditValue == null)
                {
                    MsgBox.ShowOK("请输入单号！");
                    return;
                }
                string BillNo = barEditItem8.EditValue.ToString();
                list.Add(new SqlPara("TransferSite", TransferSite));
                list.Add(new SqlPara("BillNo", BillNo));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GETFETCHPAY_InFee_Load_ByBillNo", list);
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
        //打开批量提取
        private void barButtonItem24_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.panelControl1.Show();
        }
        //退出
        private void button2_Click(object sender, EventArgs e)
        {
            this.txtBillNos.Text = "";
            this.panelControl1.Hide();
        }
        //检索
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBillNos.Text))
                {
                    XtraMessageBox.Show("请先输入单号或批次号。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string sb = txtBillNos.Text.Trim().Replace("\r\n", "@");
                string[] aa = sb.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                int rowHandle = myGridView2.RowCount;
                for (int i = 0; i < rowHandle; i++)
                {
                    string sb2 = GridOper.GetRowCellValueString(myGridView2, i, "SendBatch");
                    string sb3 = GridOper.GetRowCellValueString(myGridView2, i, "BillNo");
                    for (int j = 0; j < aa.Length; j++)
                    {
                        string sb4 = aa[j];
                        if (sb4 == sb2)
                        {
                            txtBillNos.Text = "";
                            MsgBox.ShowOK("输入的批次号已提取");
                            return;
                        }
                        if (sb4 == sb3)
                        {
                            txtBillNos.Text = "";
                            MsgBox.ShowOK("输入的单号已提取");
                            return;
                        }
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                string CauseName = barEditCause == null || barEditCause.EditValue == null ? CommonClass.UserInfo.CauseName : barEditCause.EditValue.ToString();
                string AreaName = barEditArea == null || barEditArea.EditValue == null ? CommonClass.UserInfo.AreaName : barEditArea.EditValue.ToString();
                string WebName = barEditWeb == null || barEditWeb.EditValue == null ? CommonClass.UserInfo.WebName : barEditWeb.EditValue.ToString();
                string TransferSite = barEditTransferSite == null || barEditTransferSite == null ? CommonClass.UserInfo.SiteName : barEditTransferSite.EditValue.ToString();
                CauseName = CauseName == "全部" ? "%%" : CauseName;
                AreaName = AreaName == "全部" ? "%%" : AreaName;
                WebName = WebName == "全部" ? "%%" : WebName;
                TransferSite = TransferSite == "全部" ? "%%" : TransferSite;
                list.Add(new SqlPara("CauseName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("BegWeb", WebName));
                list.Add(new SqlPara("TransferSite", TransferSite));

                list.Add(new SqlPara("t1", barEditItem3.EditValue));
                list.Add(new SqlPara("t2", barEditItem4.EditValue));
                list.Add(new SqlPara("IsOut", "已出库"));
                list.Add(new SqlPara("FetchPayType", barEditItem6.EditValue));
                list.Add(new SqlPara("BillNos", sb.ToString() + "@"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GETFETCHPAY_InFee_Load_KETONG_pilian", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("查无此单", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ds == null || ds.Tables.Count == 0) return;
                ds1 = ds.Clone();
                myGridControl1.DataSource = ds.Tables[0];
                myGridControl2.DataSource = ds1.Tables[0];

                string retrunStr = string.Empty;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    retrunStr = sb.Replace(row["BillNo"].ToString(), "");
                    sb = retrunStr;
                }
                string[] spStr = retrunStr.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                retrunStr = string.Join("@", spStr);
                txtBillNos.Text = retrunStr.Replace("@", "\r\n");
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem25_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            string a = myGridView1.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Tag = a;
            frm.ShowDialog();
        }    //plh20200521 LMS--6539


      


        private void myGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView1, e, popupMenu1);
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0) return;

        }  //plh20200521 LMS--6539


    }
}