using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.Data.SqlClient;

namespace ZQTMS.UI
{
    public partial class frmDepartureAddPre : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset2 = new DataSet();
        GridHitInfo hitInfo = null;
        public frmDepartureAddPre()
        {
            InitializeComponent();
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }
        /// <summary>
        /// 提取库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "长途配载";
            getdata(1);
        }

        private void getdata(int type)
        {
            try
            {
                dataset1.Clear();
                dataset2.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadType", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOADPRE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                dataset1 = ds;
                dataset2 = dataset1.Clone();
                myGridControl1.DataSource = dataset1.Tables[0];
                myGridControl2.DataSource = dataset2.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmDepartureAdd_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView2, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar2, bar3);
            CommonClass.SetSite(EndSite);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            GridOper.GetGridViewColumn(myGridView2, "factqty").AppearanceCell.BackColor = Color.Yellow;//实发件数背景为黄色

            myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            Creator.Text = LoadPeoples.Text = CommonClass.UserInfo.UserName;
            BeginSite.Text = CommonClass.UserInfo.SiteName;
            DepartureDate.DateTime = CommonClass.gcdate;

            barBtnSearch.Enabled = UserRight.GetRight("126081");//长途
            barButtonItem11.Enabled = UserRight.GetRight("126082");//整车
            barButtonItem12.Enabled = UserRight.GetRight("126083");//项目

            GetSiteName();
        }

        /// <summary>
        /// 获取线路
        /// </summary>
        private void GetSiteName()
        {
            if (CommonClass.dsSite == null || CommonClass.dsSite.Tables.Count == 0) return;
            string site = "";
            foreach (DataRow dr in CommonClass.dsSite.Tables[0].Rows)
            {
                site = ConvertType.ToString(dr["SiteName"]);
                if (site != CommonClass.UserInfo.SiteName)
                    treeView1.Nodes.Add(site);
            }
            treeView1.Nodes.Add("全部");
        }

        public void GetWeightAndVolume()
        {
            decimal actVolume = 0;
            decimal actWeight = 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                actVolume += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Volume")) * ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "factqty")) / ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "Num")), 2);
                actWeight += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Weight")) * ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "factqty")) / ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "Num")), 2);
            }
            ActVolume.Text = actVolume.ToString();
            ActWeight.Text = actWeight.ToString();
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnLeftOne_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnLeftMore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnRightOne_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void barBtnRightMore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何配载的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ContractNO.Text = DepartureBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
            if (DepartureBatch.Text == "")
            {
                MsgBox.ShowError("获取发车批次失败,请重试!");
                return;
            }
            string vehicleno = CarNO.Text.Trim();
            if (vehicleno == "")
            {
                MsgBox.ShowError("车牌号必须填写!");
                CarNO.Focus();
                return;
            }
            string endSite = EndSite.Text.Trim();
            if (endSite == "")
            {
                MsgBox.ShowError("请填写目的地!");
                EndSite.Focus();
                return;
            }
            string loadPeople = LoadPeoples.Text.Trim();
            if (loadPeople == "")
            {
                MsgBox.ShowError("配载员必填!");
                LoadPeoples.Focus();
                return;
            }
            if (ExpArriveDate.Text.Trim() == "")
            {
                MsgBox.ShowError("请填写预到日期!");
                ExpArriveDate.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("请选择配载类型!");
                LoadingType.Focus();
                return;
            }
            if (endSite.Split(',').Length > 2)
            {
                MsgBox.ShowOK("目的地不能超过2个!");
                EndSite.Focus();
                return;
            }

            //检查运输方式
            string transitMode = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                transitMode = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransitMode"));
                if (loadingType == "整车配载" && transitMode != "中强整车")
                {
                    MsgBox.ShowError("您选择的配载类型是整车配载,必须选择运输方式为中强整车的运单!");
                    return;
                }
                if (loadingType == "长途配载" && (transitMode == "中强城际" || transitMode == "中强整车"))
                {
                    MsgBox.ShowError("您选择的配载类型是长途配载,选择运单的运输方式不能为 中强城际或中强整车!");
                    return;
                }
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("BeginSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("EndSite", endSite));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));
                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", loadPeople));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", BoxNO.Text.Trim()));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                string billNoStr = "";
                string numStr = "";

                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    billNoStr += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                    numStr += myGridView2.GetRowCellValue(i, "factqty") + ",";
                }

                list.Add(new SqlPara("BillNOStr", billNoStr));
                list.Add(new SqlPara("NumStr", numStr));

                list.Add(new SqlPara("LoadSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("LoadWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadBusiDept", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("LoadArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("LoadDept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("LoadingType", loadingType));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTUREPRE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("配载完成。\r\n\r\n现在将关闭本窗口。\r\n\r\n返回后，可以打印清单或者调整本车配载及相关运输合同信息。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                showDeteil();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void showDeteil()
        {
            frmDepartureMody frm = new frmDepartureMody();
            frm.sDepartureBatch = DepartureBatch.Text;
            frm.xtraTabControl1.SelectedTabPage = frm.tp3;
            frm.ShowDialog();
        }

        private void btnCanccel_Click(object sender, EventArgs e)
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
                myGridView1.Columns["billno"].FilterInfo = new ColumnFilterInfo("[billno] LIKE" + "'%" + szfilter + "%'", "");
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
            GridViewMove.Move(myGridView1, dataset1, dataset2);
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
            GridViewMove.Move(myGridView2, dataset2, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void edCarNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl3.Focus();
            }
        }

        private void edCarNO_Leave(object sender, EventArgs e)
        {
            if (!myGridControl3.Focused)
            {
                myGridControl3.Visible = false;
            }
        }

        private void edCarNO_Enter(object sender, EventArgs e)
        {
            if (CarNO.Text.Trim() == "")
            {
                myGridView3.ClearColumnsFilter();
            }
            if (myGridView3.RowCount == 0) return;
            myGridControl3.BringToFront();
            myGridControl3.Left = CarNO.Left;
            myGridControl3.Top = CarNO.Top + CarNO.Height;
            myGridControl3.Visible = true;
        }

        private void edCarNO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo("[CarNo] LIKE " + "'%" + e.NewValue.ToString() + "%'" + " OR [CarNo] LIKE" + "'%" + e.NewValue.ToString() + "%'", "");
        }

        private void myGridControl3_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView3.GetDataRow(rowhandle);
            try
            {
                CarNO.EditValue = dr["CarNo"];
                DriverName.EditValue = dr["DriverName"];
                DriverPhone.EditValue = dr["DriverPhone"];
                CarrNO.EditValue = dr["CarrNO"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolume.EditValue = dr["LoadVolum"];
            }
            catch { }
            myGridControl3.Visible = false;
        }
        public string GetMaxInOneVehicleFlag(string bsite)
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet dsflag = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", bsite));
                list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_INONEVEHICLEFLAGpre", list);
                dsflag = SqlHelper.GetDataSet(sps);

                return ConvertType.ToString(dsflag.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("产生发车批次失败：\r\n" + ex.Message);
                return "";
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                /*
                DepartureDate.EditValue = CommonClass.gcdate;
                ExpArriveDate.EditValue = CommonClass.gcdate;
                string esite = "", esite_t = "", middlesite_t = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    middlesite_t = myGridView2.GetRowCellValue(i, "TransferSite").ToString().Trim();
                    if (middlesite_t == "")
                    {
                        esite_t = myGridView2.GetRowCellValue(i, "DestinationSite").ToString().Trim();
                        if (!esite.Contains(esite_t))
                        {
                            esite += esite_t + ",";
                        }
                    }
                    else
                    {
                        middlesite_t = myGridView2.GetRowCellValue(i, "TransferSite").ToString().Trim();
                        if (!esite.Contains(middlesite_t))
                        {
                            esite += middlesite_t + ",";
                        }
                    }
                }
                esite = esite.TrimEnd(',');
                EndSite.Text = esite;*/

                if (DepartureBatch.Text.Trim() == "")
                {
                    DepartureBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                    if (ContractNO.Text.Trim() == "")
                        ContractNO.Text = DepartureBatch.Text;
                }

                GetWeightAndVolume();

                string s = "", tmp = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    tmp = GridOper.GetRowCellValueString(myGridView2, i, "TransferSite");
                    if (tmp != "" && !s.Contains(tmp))
                        s += tmp + ",";
                }
                EndSite.Text = s.TrimEnd(',');
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "配载库存清单");
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载库存挑选清单");
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void myGridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || e.Value == null) return;
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            try
            {
                int leftNum = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "LeftNum"));
                int factQty = ConvertType.ToInt32(e.Value);

                if (factQty <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "实发件数必须大于0!";
                }
                else if (factQty > leftNum)
                {
                    e.Valid = false;
                    e.ErrorText = "实发件数不能大于剩余件数!";
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void myGridView2_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dataset1 == null || dataset1.Tables.Count == 0) return;
            if (treeView1.SelectedNode == null) return;
            string site = treeView1.SelectedNode.Text.Trim();
            if (site == "全部")
                myGridView1.ClearColumnsFilter();
            else
                GridOper.GetGridViewColumn(myGridView1, "TransferSite").FilterInfo = new ColumnFilterInfo("TransferSite='" + site + "'");
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "整车配载";
            getdata(2);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "项目配载";
            getdata(3);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "长途配载";
            getdata(4);
        }
    }
}