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
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors.Repository;

namespace ZQTMS.UI
{
    public partial class frmBackAduit : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;
        BarEditItem barEditItem = new BarEditItem(); //生成站点下来框
        BarEditItem barEditItemWeb = new BarEditItem(); //生成站点下来框

        public frmBackAduit()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            try
            {
                dataset1.Clear();
                dataset3.Clear();
                List<SqlPara> list = new List<SqlPara>();
                string UserName = ConvertType.ToString(barEditUser.EditValue);
                string WebName = ConvertType.ToString(barEditWeb.EditValue);
                list.Add(new SqlPara("webname", WebName == "" ? CommonClass.UserInfo.WebName : WebName));
                list.Add(new SqlPara("UserName", UserName == "" ? CommonClass.UserInfo.UserName : UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_ADUITLIST_FOR_BACK", list);
                dataset1 = SqlHelper.GetDataSet(sps);
                if (dataset1 == null || dataset1.Tables.Count == 0) return;

                myGridControl8.DataSource = dataset1.Tables[0];
                dataset3 = dataset1.Clone();
                myGridControl1.DataSource = dataset3.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void w_package_load_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false);

            CommonClass.GetGridViewColumns(myGridView1, false, myGridView9);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            GridOper.RestoreGridLayout(myGridView9, myGridView1);

            //cc.Create_BarEditItem_Webid(barManager1, bar1, barEditItemWeb, ur.GetUserRightDetail888("ab12a0101"), commonclass.gbsite);
            //cc.Create_BarEditItem_Site(barManager1, bar1, barEditItem, ur.GetUserRightDetail888("ab12a0101"));


            //repositoryItemComboBox2.AutoHeight = false;
            //repositoryItemComboBox2.Sorted = true;
            //repositoryItemComboBox3.AutoHeight = false;
            //repositoryItemComboBox3.Sorted = true;
            try
            {
                if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
                string s = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + s + "'");
                repositoryItemComboBox2.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    repositoryItemComboBox2.Properties.Items.Add(dr[i]["WebName"]);
                }
                repositoryItemComboBox2.Properties.Items.Add("全部");
                barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
            barEditUser.EditValue = CommonClass.UserInfo.UserName;
            barButtonItem1.Enabled = UserRight.GetRight("13161781");
            barButtonItem11.Enabled = UserRight.GetRight("13161782");
            barButtonItem9.Enabled = UserRight.GetRight("13161783");

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

        private void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void gridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset3, dataset1);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView9, dataset1, dataset3);
        }

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset3, dataset1);
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView9, dataset1, dataset3);
        }

        private void gridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
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
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void gridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private decimal GetHexiaoAcc(ref decimal accall)
        {
            decimal acc = 0, sumacc = 0, acc1 = 0, sumacc1 = 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                acc = myGridView1.GetRowCellValue(i, "accnowleft") == DBNull.Value ? 0 : Convert.ToDecimal(myGridView1.GetRowCellValue(i, "accnowleft").ToString());
                sumacc += acc;

                acc1 = myGridView1.GetRowCellValue(i, "accnow") == DBNull.Value ? 0 : Convert.ToDecimal(myGridView1.GetRowCellValue(i, "accnow").ToString());
                sumacc1 += acc1;
            }
            accall = sumacc1;
            return sumacc;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView9);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView9);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView9.OptionsView.ShowAutoFilterRow = !myGridView9.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView9, dataset1, dataset3);
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView9.SelectAll();
            GridViewMove.Move(myGridView9, dataset1, dataset3);
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset3, dataset1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset3, dataset1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barEditItem1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView9.ClearColumnsFilter();
                myGridView9.Columns["BillNo"].FilterInfo = new ColumnFilterInfo("[BillNo] LIKE " + "'%" + szfilter + "%'");
            }
            else
                myGridView9.ClearColumnsFilter();
        }

        private void barEditItem1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView9.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView9.SelectRow(0);
            GridViewMove.Move(myGridView9, dataset1, dataset3);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView9.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, dataset3, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void gridView3_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //GridViewMove.v(gridView3, e, "accnow", 2);
        }

        private void gridView3_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            //cc.ValidateMessage(e);
        }

        private void splitContainerControl1_SplitterPositionChanged(object sender, EventArgs e)
        {
            //API.WriteINI("SplitterPosition", "hxPosition", splitContainerControl1.SplitterPosition.ToString(), "msconfig");
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            if (myGridView1.RowCount == 0) return;
            if (MsgBox.ShowYesNo("反核销不可逆！！！请谨慎操作！！") != DialogResult.Yes) return;
            try
            {
                string ArrBillNo = "";
                string AduitFetchBillNo = "";
                string AduitNowPayBillNo = "";
                string AduitTransBillNo = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "账款")
                        ArrBillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "提付"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "提付转正常"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "中转提付转正常"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "中转提付")
                        AduitFetchBillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "现付"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "现付转正常")
                        AduitNowPayBillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "提付异动"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "非提付异动"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "提付异动转正常"
                        || ConvertType.ToString(myGridView1.GetRowCellValue(i, "AduitBillState")) == "非提付异动转正常")
                        AduitTransBillNo += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ArrBillNo", ArrBillNo));
                list.Add(new SqlPara("AduitFetchBillNo", AduitFetchBillNo));
                list.Add(new SqlPara("AduitNowPayBillNo", AduitNowPayBillNo));
                list.Add(new SqlPara("AduitTransBillNo", AduitTransBillNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[USP_ADD_BACKADUIT]", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    dataset3.Clear();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || myGridView1.FocusedRowHandle < 0) return;
            try
            {
                float NowPayVerifBalance = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("ArrComfirmBalance"));
                float Amount = ConvertType.ToFloat(e.Value);
                if (Amount <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额必须大于0!";
                    return;
                }
                if (Amount > NowPayVerifBalance)
                {
                    e.Valid = false;
                    e.ErrorText = "审核金额不能大于余额!";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void barEditWeb_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (CommonClass.dsUsers == null || CommonClass.dsUsers.Tables.Count == 0) return;
                string s = barEditWeb.EditValue.ToString();
                if (s == "全部")
                    s = "%%";
                DataRow[] dr = CommonClass.dsUsers.Tables[0].Select("WebName like '" + s + "'");
                repositoryItemComboBox3.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    repositoryItemComboBox3.Properties.Items.Add(dr[i]["UserName"]);
                }
                repositoryItemComboBox3.Properties.Items.Add("全部");
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }

    }
}