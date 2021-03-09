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
    public partial class frmArrConfirmDetail : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;
        BarEditItem barEditItem = new BarEditItem(); //生成站点下来框
        BarEditItem barEditItemWeb = new BarEditItem(); //生成站点下来框

        public frmArrConfirmDetail()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        private void barEditItem1_EditValueChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox = (DevExpress.XtraEditors.Repository.RepositoryItemComboBox)barEditItemWeb.Edit;
            repositoryItemComboBox.Items.Clear();
            //cc.FillRepWebByParent(repositoryItemComboBox, barEditItem.EditValue + "", true);
            //barEditItemWeb.EditValue = "全部";
        }

        private void getdata()
        {
            try
            {
                dataset1.Clear();
                dataset3.Clear();
                List<SqlPara> list = new List<SqlPara>();
                string WebName = ConvertType.ToString(barEditWeb.EditValue);
                string siteName = ConvertType.ToString(barEditSite.EditValue);
                if (string.IsNullOrEmpty(siteName))
                {
                    MsgBox.ShowOK("请选择站点！");
                    return;
                }
                if (string.IsNullOrEmpty(WebName))
                {
                    MsgBox.ShowOK("请选择网点！");
                    return;
                }
                list.Add(new SqlPara("datediff", flag));
                list.Add(new SqlPara("webname", WebName == "" ? CommonClass.UserInfo.WebName : WebName));
                list.Add(new SqlPara("siteName", siteName == "" ? CommonClass.UserInfo.SiteName : siteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_WayBill_ArrConfirm_ED_Auto", list);
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
            CommonClass.FormSet(this);

            CommonClass.GetGridViewColumns(myGridView1, false, myGridView9);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            GridOper.RestoreGridLayout(myGridView9, myGridView1);

            //cc.Create_BarEditItem_Webid(barManager1, bar1, barEditItemWeb, ur.GetUserRightDetail888("ab12a0101"), commonclass.gbsite);
            //cc.Create_BarEditItem_Site(barManager1, bar1, barEditItem, ur.GetUserRightDetail888("ab12a0101"));

            barEditItem.EditValueChanged += new System.EventHandler(this.barEditItem1_EditValueChanged);

            //repositoryItemComboBox2.AutoHeight = false;
            //repositoryItemComboBox2.Sorted = true;
            //repositoryItemComboBox3.AutoHeight = false;
            //repositoryItemComboBox3.Sorted = true;
            try
            {
                //if (CommonClass.dsSite == null || CommonClass.dsSite.Tables.Count == 0) return;
                //string s = "%%";
                //DataRow[] dr = CommonClass.dsSite.Tables[0].Select("SiteName like '" + s + "'");
                //repositoryItemComboBox2.Properties.Items.Clear();
                //for (int i = 0; i < dr.Length; i++)
                //{
                //    repositoryItemComboBox2.Properties.Items.Add(dr[i]["SiteName"]);
                //}
                //repositoryItemComboBox2.Properties.Items.Add("全部");
                SetSite(barEditSite);
                barEditSite.EditValue = CommonClass.UserInfo.SiteName;
                barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
            barEditWeb.EditValue = CommonClass.UserInfo.WebName;
            barEditItem4.EditValue = CommonClass.gcdate;
            repositoryItemComboBox4.Properties.Items.Add("一个月");
            repositoryItemComboBox4.Properties.Items.Add("三个月");
            repositoryItemComboBox4.Properties.Items.Add("六个月");
            repositoryItemComboBox4.Properties.Items.Add("一年");

            if ( CommonClass.UserInfo.SiteName!="总部")
            {
                barEditSite.Enabled =false;
                barEditWeb.Enabled = false;
            }

        }

        public void SetWeb(BarEditItem cb, string SiteName,bool isall)
        {
            if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
            try
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + SiteName + "'");
                repositoryItemComboBox3.Properties.Items.Clear();
                for (int i = 0; i < dr.Length; i++)
                {
                    repositoryItemComboBox3.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    repositoryItemComboBox3.Properties.Items.Add("全部");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


        public void SetSite(BarEditItem cb)
        {
            if (CommonClass.dsSite == null || CommonClass.dsSite.Tables.Count == 0) return;
            try
            {
                for (int i = 0; i < CommonClass.dsSite.Tables[0].Rows.Count; i++)
                {
                    repositoryItemComboBox2.Properties.Items.Add(CommonClass.dsSite.Tables[0].Rows[i]["SiteName"]);
                }
                repositoryItemComboBox2.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
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
            if (MsgBox.ShowYesNo("是否确认金额？") != DialogResult.Yes) return;
            try
            {
                string unitstr = "";
                string accqkstr = "";
                string accqkleftstr = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    unitstr += myGridView1.GetRowCellValue(i, "BillNo").ToString() + "@";
                    decimal accqkleft = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "ArrComfirmBalance").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ArrComfirmBalance").ToString());
                    decimal accqk = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "ArrComfirmFee").ToString() == "" ? "0" : myGridView1.GetRowCellValue(i, "ArrComfirmFee").ToString());
                    accqkstr += accqk + "@";
                    accqkleftstr += (accqkleft - accqk) + "@";
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", unitstr));
                list.Add(new SqlPara("ArrConfirmArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("ArrConfirmCause", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("ArrConfirmSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("ArrConfirmWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("ArrConfirmMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("ArrConfirmFee", accqkleftstr));
                list.Add(new SqlPara("ArrConfirmFee1", accqkstr));
                list.Add(new SqlPara("ArrConfirmDate",barEditItem4.EditValue));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "[USP_ADD_ArrConfirm]", list);
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

        private void barEditSite_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                //if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
                //string s = barEditSite.EditValue.ToString();
                //DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + s + "'");
                //repositoryItemComboBox3.Properties.Items.Clear();
                //for (int i = 0; i < dr.Length; i++)
                //{
                //    repositoryItemComboBox3.Properties.Items.Add(dr[i]["WebName"]);
                //}
                //repositoryItemComboBox3.Properties.Items.Add("全部");
                barEditWeb.EditValue = "";
                SetWeb(barEditWeb, barEditSite.EditValue.ToString(),true);
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }

        int flag = 0;
        private void barEditItem3_EditValueChanged(object sender, EventArgs e)
        {
            string dateDiff = barEditItem3.EditValue.ToString();
            try
            {
                switch (dateDiff) {
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

    }
}