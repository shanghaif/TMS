using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class JMfrmAddShortconnect : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset3 = new DataSet();
        GridHitInfo hitInfo = null;

        public JMfrmAddShortconnect()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("begSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("begWeb", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PACKAGE_LOAD_FOR_CONNECT", list);
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
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void w_package_load_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar1, bar2);
            barEditItem1.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem1_EditValueChanging);
            barEditItem1.Edit.KeyDown += new KeyEventHandler(barEditItem1_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
            //加载车辆信息

            CommonClass.SetSite(SCDesSite, false);
            //SCDesSite.SelectedIndex = 0;
            SCMan.Text = CommonClass.UserInfo.UserName;
            SCSite.Text = CommonClass.UserInfo.SiteName;
            SCDate.DateTime = CommonClass.gcdate;
            SCDesSite.Text = CommonClass.UserInfo.SiteName;
            gridControl3.DataSource = CommonClass.dsCar.Tables[0];
            getdelvCusData();

            //合作形式
            //string[] CarCooperationModeList = CommonClass.Arg.CarCooperation.Split(',');
            //if (CarCooperationModeList.Length > 0)
            //{
            //    for (int i = 0; i < CarCooperationModeList.Length; i++)
            //    {
            //        Cooperation.Properties.Items.Add(CarCooperationModeList[i]);
            //    }
            //    Cooperation.SelectedIndex = 0;
            //}
        }

        private void gridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //cc.Moveback(gridView2, dataset3, dataset1, 0);
        }

        private void gridControl2_DragDrop(object sender, DragEventArgs e)
        {
            //cc.Move(gridView1, dataset1, dataset3, 0);
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

        private void gridControl3_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_DragDrop(object sender, DragEventArgs e)
        {
            //cc.Moveback(gridView2, dataset3, dataset1, 0);
        }

        private void gridControl3_DragDrop(object sender, DragEventArgs e)
        {
            //cc.Moveback(gridView2, dataset3, dataset1, 0);
        }

        private void gridControl2_MouseDown_1(object sender, MouseEventArgs e)
        {
            //hitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void finished()
        {
            string deliCode = DeliCode.Text.Trim();
            if (CarManageDep.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择是否外派！！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CarManageDep.Text.Trim() == "否" && SCCarNo.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入申请车辆编号！！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (CarManageDep.Text.Trim() == "是" && deliCode == "")
            {
                XtraMessageBox.Show("请输入派车申请编号！！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            myGridView2.ClearColumnsFilter();
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何短驳的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string vehicleno = SCCarNo.Text.Trim();
            string chauffer = SCDriver.Text.Trim();
            string tosite = SCDesSite.Text.Trim();
            string towebid = SCDesWeb.Text.Trim();

            DateTime dt = SCDate.DateTime;
            string madeby = SCMan.Text.Trim();
            if (vehicleno == "")
            {
                XtraMessageBox.Show("请选择短驳车号！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SCCarNo.Focus();
                return;
            }
            if (chauffer == "")
            {
                XtraMessageBox.Show("请填写短驳司机！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SCDriver.Focus();
                return;
            }
            if (tosite == "")
            {
                XtraMessageBox.Show("请选择目的地！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SCDesSite.Focus();
                return;
            }
            if (towebid == "")
            {
                XtraMessageBox.Show("请选择目的网点！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SCDesWeb.Focus();
                return;
            }
            /*
            if (ShortBargeType.Text.Trim() == "")
            {
                MsgBox.ShowError("请选择短驳类型！");
                ShortBargeType.Focus();
                return;
            }

            //检查运输方式
            if (ShortBargeType.Text == "城际")
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransitMode")) != "中强城际")
                    {
                        MsgBox.ShowError("您选择的短驳类型是城际,必须选择运输方式为中强城际的运单!");
                        return;
                    }
                }
            }
            else
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransitMode")) == "中强城际")
                    {
                        MsgBox.ShowError("您选择的短驳类型不是城际,不能选择运输方式为中强城际的运单!");
                        return;
                    }
                }
            }
             */
            
            if (XtraMessageBox.Show(string.Format("确定将货物短驳到{0}({1})？", tosite, towebid), "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            if (!getdtinoneflag()) return;

            if (SCBatchNo.Text.Trim() == "")
            {
                MsgBox.ShowOK("没有取得短驳批次!请重新保存!");
                return;
            }
            try
            {
                string BillNo = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    BillNo += myGridView2.GetRowCellValue(i, "BillNo").ToString().Trim() + "@";
                }
                GetWebAcc(BillNo);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SCId", Guid.NewGuid()));
                list.Add(new SqlPara("SCBatchNo", SCBatchNo.Text.Trim()));
                list.Add(new SqlPara("SCDate", SCDate.Text.Trim()));
                list.Add(new SqlPara("SCSite", SCSite.Text.Trim()));

                list.Add(new SqlPara("SCWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("SCCarNo", SCCarNo.Text.Trim()));
                list.Add(new SqlPara("SCDriver", SCDriver.Text.Trim()));
                list.Add(new SqlPara("SCDriverPhone", SCDriverPhone.Text.Trim()));
                list.Add(new SqlPara("SCDesSite", SCDesSite.Text.Trim()));

                list.Add(new SqlPara("SCDesWeb", SCDesWeb.Text.Trim()));
                list.Add(new SqlPara("SCMan", SCMan.Text.Trim()));
                list.Add(new SqlPara("SCFee", SCFee.Text.Trim()));
                list.Add(new SqlPara("SCVerifState", 0));

                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("Cooperation", Cooperation.Text.Trim()));
                list.Add(new SqlPara("ShortBargeType", ShortBargeType.Text.Trim()));
                list.Add(new SqlPara("DeliCode", deliCode));
                list.Add(new SqlPara("CarManageDep", CarManageDep.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SHORTCONN", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    CommonClass.SetOperLog(SCBatchNo.Text.Trim(), "", "", CommonClass.UserInfo.UserName, "短途接驳", "短途接驳新增操作");
                    dataset3.Clear();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("写入数据失败：\r\n" + ex.Message);
                this.Close();
            }
        }


        public  void  GetWebAcc(string billno)
        {
            try
            {
                if (CommonClass.UserInfo.WebRole != "加盟") return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("billno", billno));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_Get_WebAcc_Ex", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    decimal acc = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["AccountBalance"]);
                    decimal total = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["total"]);
                    if (total > acc) 
                    {
                        MsgBox.ShowOK("账户余额已不足扣减此车结算费用，操作中心将无法接收本车，为保证走货时效，请及时充值！");
                    }
                    else if (acc < 3000)
                    {
                        MsgBox.ShowOK("账户余额已低于警戒值3000元，请及时充值！");
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset3);
        }

        private void gridView3_DoubleClick(object sender, EventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset3, dataset1);
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void gridControl1_DragDrop_1(object sender, DragEventArgs e)
        {

        }

        private void gridControl1_DragOver_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gridControl1_MouseMove_1(object sender, MouseEventArgs e)
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

        private void gridControl1_MouseDown_1(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "短驳库存列表");
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            myGridView1.OptionsView.ShowAutoFilterRow = !myGridView1.OptionsView.ShowAutoFilterRow;
            myGridView2.OptionsView.ShowAutoFilterRow = !myGridView2.OptionsView.ShowAutoFilterRow;
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
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
            GridOper.SaveGridLayout(myGridView2, myGridView2.Guid.ToString());
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

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "短驳库存配载列表");
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
            GridViewMove.Move(myGridView2, dataset3, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            finished();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edtosite_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetWeb();
        }

        private void gridControl3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            setvehicle();
        }

        private void gridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                setvehicle();
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl3.Visible = false;
            }
        }

        private void setvehicle()
        {
            int rowhandle = gridView3.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = gridView3.GetDataRow(rowhandle);
            SCDriver.EditValue = dr["DriverName"];
            SCDriverPhone.EditValue = dr["DriverPhone"];
            SCCarNo.EditValue = dr["CarNo"];
            Cooperation.EditValue = dr["Cooperation"];
            gridControl3.Visible = false;
        }

        private void edvehicleno_Enter(object sender, EventArgs e)
        {
            if (SCCarNo.Text.Trim() == "")
            {
                gridView3.ClearColumnsFilter();
            }
            if (gridView3.RowCount == 0) return;
            gridControl3.Left = SCCarNo.Left;
            gridControl3.Top = SCCarNo.Top + SCCarNo.Height;
            gridControl3.Visible = true;
            gridControl3.BringToFront();
        }

        private void edvehicleno_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            gridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo("[CarNo] LIKE " + "'%" + e.NewValue.ToString() + "%'" + " OR [CarNo] LIKE" + "'%" + e.NewValue.ToString() + "%'", "");
        }

        private void edvehicleno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControl3.Focus();
            }
        }

        private void edvehicleno_Leave(object sender, EventArgs e)
        {
            if (!gridControl3.Focused)
            {
                gridControl3.Visible = false;
            }
        }

        private bool getdtinoneflag()
        {

            //string s1 = "", s2 = "";
            //eddtinoneflag.EditValue = commonclass.GetSiteCode(commonclass.gbsite, ref s1, ref s2) + commonclass.gcdate.ToString("MMddHHmmss") + (new Random()).Next(11, 99).ToString();
            return true;
            //SqlConnection connection = cc.GetConn();
            //try
            //{
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    SqlCommand sq = new SqlCommand("QSP_GET_DT_INONEFLAG", connection);
            //    sq.CommandType = CommandType.StoredProcedure;
            //    sq.Parameters.Add(new SqlParameter("@bsite", SqlDbType.VarChar));
            //    sq.Parameters.Add(new SqlParameter("@webid", SqlDbType.VarChar));
            //    sq.Parameters.Add(new SqlParameter("@bcode", SqlDbType.VarChar));
            //    sq.Parameters.Add(new SqlParameter("@webcode", SqlDbType.VarChar));

            //    string s1 = "", s2 = "";

            //    sq.Parameters[0].Value = commonclass.gbsite;
            //    sq.Parameters[1].Value = commonclass.webid;
            //    sq.Parameters[2].Value = commonclass.GetSiteCode(commonclass.gbsite, ref s1, ref s2);
            //    sq.Parameters[3].Value = commonclass.webcode;

            //    da.SelectCommand = sq;

            //    DataSet ds = new DataSet();

            //    da.Fill(ds);
            //    if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            //    {
            //        commonclass.MsgBox.ShowOK("没有取得短驳批次!请重新保存!");
            //        return false;
            //    }
            //    eddtinoneflag.EditValue = ds.Tables[0].Rows[0][0];
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("没有取得短驳批次!请重新保存：\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            //return true;
        }

        private void CarManageDep_EditValueChanged(object sender, EventArgs e)
        {
            if (CarManageDep.EditValue.ToString().Trim() == "否")
            {
                DeliCode.Enabled = false;
                SCCarNo.Enabled = true;
                DeliCode.Text = Cooperation.Text = "";
            }
            else
            {
                SCCarNo.Enabled = false;
                DeliCode.Enabled = true;
                Cooperation.Text = "外请";
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                Random ran = new Random();
                SCBatchNo.Text = "SC" + DateTime.Now.ToString("yyyyMMddHHmmss") + ran.Next(10, 99);
            }
        }
        private void CarAppNo_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            GridOper.GetGridViewColumn(gridView1, "DeliCode").FilterInfo = new ColumnFilterInfo("[DeliCode] LIKE" + "'%" + e.NewValue.ToString() + "%'");
        }

        private void CarAppNo_Enter(object sender, EventArgs e)
        {
            if (DeliCode.Text.Trim() == "")
            {
                gridView1.ClearColumnsFilter();
            }
            if (gridView1.RowCount == 0) return;
            gridControl1.Left = DeliCode.Left;
            gridControl1.Top = DeliCode.Top + DeliCode.Height;
            gridControl1.Visible = true;
            gridControl1.BringToFront();
        }

        private void CarAppNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
        }

        private void CarAppNo_Leave(object sender, EventArgs e)
        {
            if (!gridControl1.Focused)
            {
                gridControl1.Visible = false;
            }
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int rowhandle = gridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                SCDriver.EditValue = gridView1.GetRowCellValue(rowhandle, "DriverName");
                SCDriverPhone.EditValue = gridView1.GetRowCellValue(rowhandle, "DriverPhone");
                SCCarNo.EditValue = gridView1.GetRowCellValue(rowhandle, "VehicleNum");
                Cooperation.EditValue = gridView1.GetRowCellValue(rowhandle, "Cooperation");
                gridControl1.Visible = false;
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void gridControl1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = gridView1.GetDataRow(rowhandle);
            SCDriver.EditValue = dr["DriverName"];
            SCDriverPhone.EditValue = dr["DriverPhone"];
            SCCarNo.EditValue = dr["VehicleNum"];
            DeliCode.EditValue = dr["DeliCode"];
            //Cooperation.EditValue = dr["Cooperation"];//固定为外请车
            gridControl1.Visible = false;
        }

        private void getdelvCusData()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELIVERY_FORSHORTCONN", null);
                dataset1 = SqlHelper.GetDataSet(sps);

                if (dataset1 == null || dataset1.Tables.Count == 0) return;
                gridControl1.DataSource = dataset1.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SetWeb()
        {
            SCDesWeb.Properties.Items.Clear();
            SCDesWeb.Text = "";
            string sCDesSite = SCDesSite.Text.Trim();
            if (sCDesSite == "") return;

            DataRow[] drs = CommonClass.dsWeb.Tables[0].Select("SiteName='" + sCDesSite + "' AND IsAcceptShort=1");
            if (drs == null || drs.Length == 0) return;

            foreach (DataRow dr in drs)
            {
                sCDesSite = ConvertType.ToString(dr["WebName"]);
                if (sCDesSite != "" && !SCDesWeb.Properties.Items.Contains(sCDesSite))
                    SCDesWeb.Properties.Items.Add(sCDesSite);
            }
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }
    }
}