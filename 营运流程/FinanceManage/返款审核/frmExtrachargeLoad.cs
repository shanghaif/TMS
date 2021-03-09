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
    public partial class frmExtrachargeLoad : BaseForm
    {
        private DataSet ds = new DataSet();
        private DataSet ds1 = new DataSet();
        GridHitInfo hitInfo = null;

        public frmExtrachargeLoad()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
            barEditItem5.Edit.KeyDown += new KeyEventHandler(barEditItem5_KeyDown);
        }


        private void barEditItem5_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "")
            //{
            //    e.Handled = true;
            //    return;
            //}
            //decimal bili = ConvertType.ToDecimal(barEditItem5.EditValue);
            //if (bili > 100 || bili<0)
            //{
            //    MsgBox.ShowOK("比例只能录入0-100的区间！");
            //    return;
            //}
            //for (int i = 0; i < myGridView2.RowCount; i++)
            //{
            //    decimal DeliveryFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "DeliveryFee"));
            //    decimal DeliveryFee_S=DeliveryFee * (bili / 100);
            //    decimal DeliveryFee_D = DeliveryFee - DeliveryFee_S;
            //    myGridView2.SetRowCellValue(i, "DeliveryFee_S", DeliveryFee_S);
            //    myGridView2.SetRowCellValue(i, "DeliveryFee_D", DeliveryFee_D);
            //}
            //myGridView2.PostEditor();
            //myGridView2.UpdateSummary();
            //e.Handled = true;
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
                list.Add(new SqlPara("causeName", CauseName));
                list.Add(new SqlPara("AreaName", AreaName));
                list.Add(new SqlPara("webName", WebName));
                list.Add(new SqlPara("bdate", barEditItem3.EditValue));
                list.Add(new SqlPara("edate", barEditItem4.EditValue));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BackExtracharge_LOAD", list);
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
            barEditItem3.EditValue = CommonClass.gbdate;
            barEditItem4.EditValue = CommonClass.gedate;

            if (repositoryItemComboBox2.Items.Count==0)
            {
                repositoryItemComboBox2.Items.Add("上楼费");
                repositoryItemComboBox2.Items.Add("装卸费");
                repositoryItemComboBox2.Items.Add("进仓费");
                repositoryItemComboBox2.Items.Add("仓储费");
                repositoryItemComboBox2.Items.Add("叉车费");
                repositoryItemComboBox2.Items.Add("控货费");
            }
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

        /// <summary>
        /// author:zxw
        /// date:2017-05-19 09:38
        /// 附加费返款核销操作
        /// </summary>
        //private void verifyOperate()
        //{
        //    int flag = 3;
        //    int count = ds1.Tables[0].Rows.Count;
        //    if (count == 0)
        //    {
        //        MsgBox.ShowOK("请在左侧选择行！");
        //        return;
        //    }
        //    DataSet dsBiLi = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FKBILI_BySignWeb", new List<SqlPara>() { new SqlPara("WebName", ds1.Tables[0].Rows[count - 1]["SignWeb"]), new SqlPara("flag", flag) }));

        //    #region ----结算上楼费----
        //    upBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["UpPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["UpPayRatio"].ToString()), 2);
        //    decimal UpstairFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "UpstairFee_C"));
        //    decimal UpstairFee_S = Math.Round(UpstairFee_C * (upBili / 100), 2);
        //    decimal UpstairFee_D = Math.Round(UpstairFee_C - UpstairFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "UpstairFee_S", UpstairFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "UpstairFee_D", UpstairFee_D);
        //    #endregion

        //    #region ----结算装卸费----
        //    zxBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["ZXPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["ZXPayRatio"].ToString()), 2);
        //    decimal HandleFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "HandleFee_C"));
        //    decimal HandleFee_S = Math.Round(HandleFee_C * (zxBili / 100), 2);
        //    decimal HandleFee_D = Math.Round(HandleFee_C - HandleFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "HandleFee_S", HandleFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "HandleFee_D", HandleFee_D);
        //    #endregion

        //    #region ----结算进仓费----
        //    jcBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["JCPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["JCPayRatio"].ToString()), 2);
        //    decimal StorageFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "StorageFee_C"));
        //    decimal StorageFee_S = Math.Round(StorageFee_C * (jcBili / 100), 2);
        //    decimal StorageFee_D = Math.Round(StorageFee_C - StorageFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "StorageFee_S", StorageFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "StorageFee_D", StorageFee_D);
        //    #endregion

        //    #region ----结算仓储费----
        //    ccBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["CCPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["CCPayRatio"].ToString()), 2);
        //    decimal WarehouseFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "WarehouseFee_C"));
        //    decimal WarehouseFee_S = Math.Round(WarehouseFee_C * (ccBili / 100), 2);
        //    decimal WarehouseFee_D = Math.Round(WarehouseFee_C - WarehouseFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "WarehouseFee_S", WarehouseFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "WarehouseFee_D", WarehouseFee_D);
        //    #endregion

        //    #region ----结算叉车费----
        //    chacheBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["ChaChePayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["ChaChePayRatio"].ToString()), 2);
        //    decimal ForkliftFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "ForkliftFee_C"));
        //    decimal ForkliftFee_S = Math.Round(ForkliftFee_C * (chacheBili / 100), 2);
        //    decimal ForkliftFee_D = Math.Round(ForkliftFee_C - ForkliftFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "ForkliftFee_S", ForkliftFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "ForkliftFee_D", ForkliftFee_D);
        //    #endregion

        //    #region ----结算控货费----
        //    khBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["KHPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["KHPayRatio"].ToString()), 2);
        //    decimal NoticeFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(count - 1, "NoticeFee_C"));
        //    decimal NoticeFee_S = Math.Round(NoticeFee_C * (khBili / 100), 2);
        //    decimal NoticeFee_D = Math.Round(NoticeFee_C - NoticeFee_S, 2);
        //    myGridView2.SetRowCellValue(count - 1, "NoticeFee_S", NoticeFee_S);
        //    myGridView2.SetRowCellValue(count - 1, "NoticeFee_D", NoticeFee_D);
        //    #endregion
        //    myGridView2.PostEditor();
        //    myGridView2.UpdateSummary();
        //}


        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
            //verifyOperate();
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
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //GridViewMove.QuickSearch();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, ds, ds1);
           // verifyOperate();
        }

        private decimal upBili = 0, zxBili = 0, jcBili = 0, ccBili = 0, chacheBili = 0, khBili = 0;
        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, ds, ds1);
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                int flag = 3;
                int count = ds1.Tables[0].Rows.Count;
                if (count == 0)
                {
                    MsgBox.ShowOK("请在左侧选择行！");
                    return;
                }
                
                DataSet dsBiLi = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FKBILI_BySignWeb", new List<SqlPara>() { new SqlPara("WebName", ds1.Tables[0].Rows[i]["SignWeb"]), new SqlPara("flag", flag) }));

                #region ----结算上楼费----
                upBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["UpPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["UpPayRatio"].ToString()), 2);
                decimal UpstairFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_C"));
                decimal UpstairFee_S = Math.Round(UpstairFee_C * (upBili / 100), 2);
                decimal UpstairFee_D = Math.Round(UpstairFee_C - UpstairFee_S, 2);
                myGridView2.SetRowCellValue(i, "UpstairFee_S", UpstairFee_S);
                myGridView2.SetRowCellValue(i, "UpstairFee_D", UpstairFee_D);
                #endregion

                #region ----结算装卸费----
                zxBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["ZXPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["ZXPayRatio"].ToString()), 2);
                decimal HandleFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_C"));
                decimal HandleFee_S = Math.Round(HandleFee_C * (zxBili / 100), 2);
                decimal HandleFee_D = Math.Round(HandleFee_C - HandleFee_S, 2);
                myGridView2.SetRowCellValue(i, "HandleFee_S", HandleFee_S);
                myGridView2.SetRowCellValue(i, "HandleFee_D", HandleFee_D);
                #endregion

                #region ----结算进仓费----
                jcBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["JCPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["JCPayRatio"].ToString()), 2);
                decimal StorageFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_C"));
                decimal StorageFee_S = Math.Round(StorageFee_C * (jcBili / 100), 2);
                decimal StorageFee_D = Math.Round(StorageFee_C - StorageFee_S, 2);
                myGridView2.SetRowCellValue(i, "StorageFee_S", StorageFee_S);
                myGridView2.SetRowCellValue(i, "StorageFee_D", StorageFee_D);
                #endregion

                #region ----结算仓储费----
                ccBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["CCPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["CCPayRatio"].ToString()), 2);
                decimal WarehouseFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_C"));
                decimal WarehouseFee_S = Math.Round(WarehouseFee_C * (ccBili / 100), 2);
                decimal WarehouseFee_D = Math.Round(WarehouseFee_C - WarehouseFee_S, 2);
                myGridView2.SetRowCellValue(i, "WarehouseFee_S", WarehouseFee_S);
                myGridView2.SetRowCellValue(i, "WarehouseFee_D", WarehouseFee_D);
                #endregion

                #region ----结算叉车费----
                chacheBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["ChaChePayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["ChaChePayRatio"].ToString()), 2);
                decimal ForkliftFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_C"));
                decimal ForkliftFee_S = Math.Round(ForkliftFee_C * (chacheBili / 100), 2);
                decimal ForkliftFee_D = Math.Round(ForkliftFee_C - ForkliftFee_S, 2);
                myGridView2.SetRowCellValue(i, "ForkliftFee_S", ForkliftFee_S);
                myGridView2.SetRowCellValue(i, "ForkliftFee_D", ForkliftFee_D);
                #endregion

                #region ----结算控货费----
                khBili = string.IsNullOrEmpty(dsBiLi.Tables[0].Rows[0]["KHPayRatio"].ToString()) ? 0 : Math.Round(Convert.ToDecimal(dsBiLi.Tables[0].Rows[0]["KHPayRatio"].ToString()), 2);
                decimal NoticeFee_C = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_C"));
                decimal NoticeFee_S = Math.Round(NoticeFee_C * (khBili / 100), 2);
                decimal NoticeFee_D = Math.Round(NoticeFee_C - NoticeFee_S, 2);
                myGridView2.SetRowCellValue(i, "NoticeFee_S", NoticeFee_S);
                myGridView2.SetRowCellValue(i, "NoticeFee_D", NoticeFee_D);
                #endregion
                myGridView2.PostEditor();
                myGridView2.UpdateSummary();
            }
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, ds1, ds);
            int count = myGridView1.RowCount;
            if (count==0)
            {
                MsgBox.ShowOK("请在右侧选择一条信息！");
                return;
            }
            #region ----结算上楼费----
            myGridView1.SetRowCellValue(count - 1, "UpstairFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "UpstairFee_D", 0);
            #endregion

            #region ----结算装卸费----
            myGridView1.SetRowCellValue(count - 1, "HandleFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "HandleFee_D", 0);
            #endregion

            #region ----结算进仓费----
            myGridView1.SetRowCellValue(count - 1, "StorageFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "StorageFee_D", 0);
            #endregion

            #region ----结算仓储费----
            myGridView1.SetRowCellValue(count - 1, "WarehouseFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "WarehouseFee_D", 0);
            #endregion

            #region ----结算叉车费----
            myGridView1.SetRowCellValue(count - 1, "ForkliftFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "ForkliftFee_D", 0);
            #endregion

            #region ----结算控货费----
            myGridView1.SetRowCellValue(count - 1, "NoticeFee_S", 0);
            myGridView1.SetRowCellValue(count - 1, "NoticeFee_D", 0);
            #endregion
            myGridView1.PostEditor();
            myGridView1.UpdateSummary();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, ds1, ds);
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                #region ----结算上楼费----
                myGridView1.SetRowCellValue(i, "UpstairFee_S", 0);
                myGridView1.SetRowCellValue(i, "UpstairFee_D", 0);
                #endregion

                #region ----结算装卸费----
                myGridView1.SetRowCellValue(i, "HandleFee_S", 0);
                myGridView1.SetRowCellValue(i, "HandleFee_D", 0);
                #endregion

                #region ----结算进仓费----
                myGridView1.SetRowCellValue(i, "StorageFee_S", 0);
                myGridView1.SetRowCellValue(i, "StorageFee_D", 0);
                #endregion

                #region ----结算仓储费----
                myGridView1.SetRowCellValue(i, "WarehouseFee_S", 0);
                myGridView1.SetRowCellValue(i, "WarehouseFee_D", 0);
                #endregion

                #region ----结算叉车费----
                myGridView1.SetRowCellValue(i, "ForkliftFee_S", 0);
                myGridView1.SetRowCellValue(i, "ForkliftFee_D", 0);
                #endregion

                #region ----结算控货费----
                myGridView1.SetRowCellValue(i, "NoticeFee_S", 0);
                myGridView1.SetRowCellValue(i, "NoticeFee_D", 0);
                #endregion
                myGridView1.PostEditor();
                myGridView1.UpdateSummary();
            }
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
                myGridView1.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
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
                myGridView2.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'", "");
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
            if (MsgBox.ShowYesNo("确定要完成核销？请三思！") != DialogResult.Yes) return;
            try
            {
                myGridView2.PostEditor();
                myGridView2.UpdateSummary();
                if (myGridView2.RowCount == 0) return;

                string SerialNumber = "", BillNo = "", SendCauseName = "", SendAreaName = "",
                       SendWebName = "", ArrivedCauseName = "", ArrivedAreaName = "", ArrivedWebName = "";

                //string editValue = barEditItem5.EditValue == null ? "" : barEditItem5.EditValue.ToString();
                //if (string.IsNullOrEmpty(editValue))
                //{
                //    XtraMessageBox.Show("请选择你要核销的费用类型！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                decimal _UpstairFee_C, _handleFee_C, _StorageFee_C, _WarehouseFee_C, _ForkliftFee_C, _NoticeFee_C;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    DataRow dr = myGridView2.GetDataRow(i);
                    _UpstairFee_C = dr["UpstairFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["UpstairFee_C"]);
                    _handleFee_C = dr["handleFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["handleFee_C"]);
                    _StorageFee_C = dr["StorageFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["StorageFee_C"]);
                    _WarehouseFee_C = dr["WarehouseFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["WarehouseFee_C"]);
                    _ForkliftFee_C = dr["ForkliftFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["ForkliftFee_C"]);
                    _NoticeFee_C = dr["NoticeFee_C"] == null ? 0 : ConvertType.ToDecimal(dr["NoticeFee_C"]);
                    if (_UpstairFee_C == 0 && _handleFee_C == 0 && _StorageFee_C == 0 && _WarehouseFee_C == 0 && _ForkliftFee_C == 0 && _NoticeFee_C == 0)
                    {
                        MsgBox.ShowOK("附加费有为空的，请检查！");
                        return;
                    }

                    BillNo += myGridView2.GetRowCellValue(i, "BillNo").ToString() + "@";
                    SendCauseName += myGridView2.GetRowCellValue(i, "CauseName").ToString() + "@";
                    SendAreaName += myGridView2.GetRowCellValue(i, "AreaName").ToString() + "@";
                    SendWebName += myGridView2.GetRowCellValue(i, "SignWeb").ToString() + "@";

                    ArrivedCauseName += myGridView2.GetRowCellValue(i, "DHCauseName").ToString() + "@";
                    ArrivedAreaName += myGridView2.GetRowCellValue(i, "DHAreaName").ToString() + "@";
                    ArrivedWebName += myGridView2.GetRowCellValue(i, "AcceptWebName").ToString() + "@";
                    #region
                    //switch (editValue)
                    //{
                    //    #region ----核销类型----
                    //    case "上楼费":
                    //        UpstairFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_S").ToString()), 2) + "@";
                    //        UpstairFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_D").ToString()), 2) + "@";
                    //        UpstairFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_S").ToString()), 2);
                    //        decimal UpstairFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UpstairFee_C").ToString()), 2);
                    //        if (UpstairFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算上楼费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    case "装卸费":
                    //        HandleFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_S").ToString()), 2) + "@";
                    //        HandleFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_D").ToString()), 2) + "@";
                    //        HandleFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_S").ToString()), 2);
                    //        decimal HandleFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "HandleFee_C").ToString()), 2);
                    //        if (HandleFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算装卸费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    case "进仓费":
                    //        StorageFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_S").ToString()), 2) + "@";
                    //        StorageFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_D").ToString()), 2) + "@";
                    //        StorageFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_S").ToString()), 2);
                    //        decimal StorageFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "StorageFee_C").ToString()), 2);
                    //        if (StorageFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算进仓费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    case "仓储费":
                    //        WarehouseFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_S").ToString()), 2) + "@";
                    //        WarehouseFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_D").ToString()), 2) + "@";
                    //        WarehouseFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_S").ToString()), 2);
                    //        decimal WarehouseFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "WarehouseFee_C").ToString()), 2);
                    //        if (WarehouseFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算仓储费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    case "叉车费":
                    //        ForkliftFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_S").ToString()), 2) + "@";
                    //        ForkliftFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_D").ToString()), 2) + "@";
                    //        ForkliftFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_S").ToString()), 2);
                    //        decimal ForkliftFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ForkliftFee_C").ToString()), 2);
                    //        if (ForkliftFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算叉车费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    case "控货费":
                    //        NoticeFee_S += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_S").ToString()), 2) + "@";
                    //        NoticeFee_D += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_D").ToString()), 2) + "@";
                    //        NoticeFee_C += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_C").ToString()), 2);
                    //        Extracharge_x += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_S").ToString()), 2);
                    //        decimal NoticeFee_X = Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "NoticeFee_C").ToString()), 2);
                    //        if (NoticeFee_X == 0)
                    //        {
                    //            XtraMessageBox.Show("第" + (i + 1) + "行【结算控货费】为0,无法核销,请检查！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        break;
                    //    #endregion
                    //}
                    #endregion
                }
                
                SerialNumber = CommonClass.gcdate.ToString("yyyyMMddHHmmsss") + new Random().Next(1000, 10000);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SerialNumber", SerialNumber));
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("SendCauseName", SendCauseName));
                list.Add(new SqlPara("SendAreaName", SendAreaName));
                list.Add(new SqlPara("SendWebName", SendWebName));

                list.Add(new SqlPara("ArrivedCauseName", ArrivedCauseName));
                list.Add(new SqlPara("ArrivedAreaName", ArrivedAreaName));
                list.Add(new SqlPara("ArrivedWebName", ArrivedWebName));



                #region
                //switch (editValue)
                //{
                //    #region ----不同核销类型传值----
                //    case "上楼费":
                //        list.Add(new SqlPara("BackFee_S", UpstairFee_S));
                //        list.Add(new SqlPara("BackFee_D", UpstairFee_D));
                //        list.Add(new SqlPara("BackFeeRate", upBili));
                //        list.Add(new SqlPara("verifyType", "上楼费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算上楼费:" + UpstairFee_C + "，审核百分比:" + upBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    case "装卸费":
                //        list.Add(new SqlPara("BackFee_S", HandleFee_S));
                //        list.Add(new SqlPara("BackFee_D", HandleFee_D));
                //        list.Add(new SqlPara("BackFeeRate", zxBili));
                //        list.Add(new SqlPara("verifyType", "装卸费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算装卸费:" + HandleFee_C + "，审核百分比:" + zxBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    case "进仓费":
                //        list.Add(new SqlPara("BackFee_S", StorageFee_S));
                //        list.Add(new SqlPara("BackFee_D", StorageFee_D));
                //        list.Add(new SqlPara("BackFeeRate", jcBili));
                //        list.Add(new SqlPara("verifyType", "进仓费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算进仓费:" + StorageFee_C + "，审核百分比:" + jcBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    case "仓储费":
                //        list.Add(new SqlPara("BackFee_S", WarehouseFee_S));
                //        list.Add(new SqlPara("BackFee_D", WarehouseFee_D));
                //        list.Add(new SqlPara("BackFeeRate", ccBili));
                //        list.Add(new SqlPara("verifyType", "仓储费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算仓储费:" + WarehouseFee_C + "，审核百分比:" + ccBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    case "叉车费":
                //        list.Add(new SqlPara("BackFee_S", ForkliftFee_S));
                //        list.Add(new SqlPara("BackFee_D", ForkliftFee_D));
                //        list.Add(new SqlPara("BackFeeRate", chacheBili));
                //        list.Add(new SqlPara("verifyType", "叉车费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算叉车费:" + ForkliftFee_C + "，审核百分比:" + chacheBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    case "控货费":
                //        list.Add(new SqlPara("BackFee_S", NoticeFee_S));
                //        list.Add(new SqlPara("BackFee_D", NoticeFee_D));
                //        list.Add(new SqlPara("BackFeeRate", khBili));
                //        list.Add(new SqlPara("verifyType", "控货费"));
                //        if (DialogResult.Yes != MsgBox.ShowYesNo("结算控货费:" + NoticeFee_C + "，审核百分比:" + khBili + "，实际审核金额:" + Extracharge_x + "   \r\n是否确认审核？")) return;
                //        break;
                //    #endregion
                //}
                #endregion

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_BACKExtracharge", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    ds1.Clear();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
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
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem19_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //setData();
        }

        private void setData()
        {
            try
            {
                //decimal bili = ConvertType.ToDecimal(barEditItem5.EditValue);
                //if (bili > 100 || bili < 0)
                //{
                //    MsgBox.ShowOK("比例只能录入0-100的区间！");
                //    return;
                //}
                //for (int i = 0; i < myGridView2.RowCount; i++)
                //{
                //    decimal DeliveryFee = ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "DeliveryFee"));
                //    decimal DeliveryFee_S =Math.Round(DeliveryFee * (bili / 100),2);
                //    decimal DeliveryFee_D = Math.Round(DeliveryFee - DeliveryFee_S,2);
                //    myGridView2.SetRowCellValue(i, "DeliveryFee_S", DeliveryFee_S);
                //    myGridView2.SetRowCellValue(i, "DeliveryFee_D", DeliveryFee_D);
                //}
                //myGridView2.PostEditor();
                //myGridView2.UpdateSummary();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


    }
}