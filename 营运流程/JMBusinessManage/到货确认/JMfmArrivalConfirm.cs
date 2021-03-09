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
using ZQTMS.Lib;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class JMfmArrivalConfirm : BaseForm
    {
        public JMfmArrivalConfirm()
        {
            InitializeComponent();
        }

        private void fmArrivalConfirm_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5);

            if (CommonClass.UserInfo.SiteName == "总部")
            {
                comboBoxEdit1.Enabled = true;
                CommonClass.SetSite(comboBoxEdit1, true);
                comboBoxEdit1.Text = "全部";
            }
            else
            {
                comboBoxEdit1.Enabled = false;
                comboBoxEdit1.Text = CommonClass.UserInfo.SiteName;
            }

            BarMagagerOper.SetBarPropertity(bar1, bar2, bar3);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            CommonClass.SetSite(popupContainerEdit1, true);
            CommonClass.SetSite(popupContainerEdit2, true);
            CommonClass.SetCause(Cause, true);
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(esite, true);

            dateEdit3.DateTime = CommonClass.gbdate;
            dateEdit4.DateTime = CommonClass.gedate;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text =  CommonClass.UserInfo.WebName;
            popupContainerEdit1.Text = "全部";
            popupContainerEdit2.Text = CommonClass.UserInfo.SiteName;
            bsite.Text = "全部";
            esite.Text = CommonClass.UserInfo.SiteName;
            endweb.Text = CommonClass.UserInfo.WebName;
            begDate.DateTime = CommonClass.gbdate;
            endDate.DateTime = CommonClass.gedate;

            getdata();//打开加载在途车辆
            CommonClass.GetServerDate();
            btnFetch_Click(null, null);
        }

        /// <summary>
        /// 提取到货车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                string site = CommonClass.UserInfo.SiteName == "总部" ? comboBoxEdit1.Text.Trim() : CommonClass.UserInfo.SiteName;
                string web = CommonClass.UserInfo.SiteName == "总部" ? "全部" : CommonClass.UserInfo.WebName;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", site));
                list.Add(new SqlPara("dt1", bdate.DateTime));
                list.Add(new SqlPara("dt2", edate.DateTime));
                list.Add(new SqlPara("webid", web));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRIVED_BILLDEPARTURE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView2.RowCount < 1000) myGridView2.BestFitColumns();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Tool.GridOper.ExportToExcel(myGridView1, "在途车辆清单");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getdata()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", Common.CommonClass.UserInfo.SiteName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_INROAD_BILLDEPARTURE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
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

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "到货车辆");
        }

        /// <summary>
        /// 取消到货
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton14_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            string inoneflag = "", vehicleno = "";
            inoneflag = myGridView2.GetRowCellValue(rowhandle, "DepartureBatch").ToString();
            vehicleno = myGridView2.GetRowCellValue(rowhandle, "CarNO").ToString();

            if (GetUnitState(inoneflag))
            {
                MsgBox.ShowError("不能取消该车。原因是该车部分运单已经提货或者送货。");
                return;
            }
            if (MsgBox.ShowYesNo("车号：" + vehicleno + " 发车批次：" + inoneflag + "\r\n该车将设置成在途状态，该状态会立即传递给始发站。\n\r确认取消后，本地库存减少。\r\n在途车辆会增加。") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", inoneflag));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

            //提前获取到轨迹信息
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", inoneflag));
            lists.Add(new SqlPara("BillNO", null));
            lists.Add(new SqlPara("tracetype", "货物到达"));
            lists.Add(new SqlPara("num", 7));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL", list);
            if (SqlHelper.ExecteNonQuery(sps) == 0) return;
            myGridView2.DeleteRow(rowhandle);
            MsgBox.ShowOK();
            CommonClass.SetOperLog(inoneflag, "", "", CommonClass.UserInfo.UserName, "到货确认取消到货", "整车取消到货操作");
            CommonSyn.TraceSyn(inoneflag, null, 7, "货物到达", 2, "货物到达",dss);
        }

        private bool GetUnitState(string DepartureBatch)
        {
            DataSet ds = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_UNIT_STATE", list);
                ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return true;

                return ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return true;
        }

        /// <summary>
        /// 查看到货清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            showdetail(myGridView2, 2);
        }

        /// <summary>
        /// 查询所有在途车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton17_Click(object sender, EventArgs e)
        {
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", dateEdit3.DateTime));
                list.Add(new SqlPara("t2", dateEdit4.DateTime));
                list.Add(new SqlPara("bsite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("esite", popupContainerEdit1.Text.Trim() == "全部" ? "%%" : popupContainerEdit1.Text.Trim()));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("WebName", webName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_INROAD_BILLDEPARTURE_ALL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView3.RowCount < 1000) myGridView3.BestFitColumns();
            }
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "所有在途车辆");
        }

        private void showdetail(MyGridView gv, int hitoreder)
        {
            int rowhandle = gv.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                JMfmArrivalShowDetail wasd = new JMfmArrivalShowDetail();
                wasd.DepartureBatch = GridOper.GetRowCellValueString(gv, "DepartureBatch");

                wasd.Hitorder = hitoreder;
                wasd.Arriveddate = gv.GetRowCellValue(rowhandle, "ArrivedDate") == DBNull.Value ? DateTime.Now : Convert.ToDateTime(gv.GetRowCellValue(rowhandle, "ArrivedDate"));
                wasd.ShowDialog();
            }
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            showdetail(myGridView3, 1);
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView1, 1);
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView3, 1);
        }

        private void myGridView2_DoubleClick(object sender, EventArgs e)
        {
            showdetail(myGridView2, 2);
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        //到货短驳列表提取
        private void btnFetch_Click(object sender, EventArgs e)
        {
            string proc = "QSP_GET_SENDSHORTCONN_CONFIRM";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("t1", bdate.DateTime));
                //list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("StartSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("DestinationSite", ""));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl4.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnConfrm_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView4.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", myGridView4.GetRowCellValue(rowhandle, "SendBatch").ToString()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_SENDSHORTCONN_CONFIRM", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView4.DeleteRow(rowhandle);
                    myGridView4.PostEditor();
                    myGridView4.UpdateCurrentRow();
                    myGridView4.UpdateSummary();
                    DataTable dt = myGridControl4.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetList_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView4.FocusedRowHandle;
            if (rowhandle < 0) return;
            //查看明细
            MyGridView gv = (MyGridView)myGridControl4.MainView;
            if (gv.FocusedRowHandle < 0) return;
            JMfrmSendDetailForConfirm ws = new JMfrmSendDetailForConfirm();
            ws.DepartureBatch = GridOper.GetRowCellValueString(gv, "DepartureBatch");//zxw 2016-12-19
            ws.gv = gv;
            ws.ShowDialog();
        }

        private void myGridView4_DoubleClick(object sender, EventArgs e)
        {
            btnGetList_Click(sender, e);
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView4, "到货接驳");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            string proc = "QSP_GET_SENDSHORTCONN_CANCEL";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", begDate.DateTime));
                list.Add(new SqlPara("t2", endDate.DateTime));
                list.Add(new SqlPara("StartSite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("DestinationSite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("WebName", endweb.Text.Trim() == "全部" ? "%%" : endweb.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, proc, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl5.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void esite_TextChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(endweb, esite.Text, true);
            endweb.SelectedIndex = 0;
        }

        /// <summary>
        /// 确认到车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string CarNo = "", DepartureBatch = "";
            CarNo = myGridView1.GetRowCellValue(rowhandle, "CarNO").ToString();
            DepartureBatch = myGridView1.GetRowCellValue(rowhandle, "DepartureBatch").ToString();

            if (MsgBox.ShowYesNo(string.Format("该车将设置成到车状态，该状态会立即传递给始发站。\r\n车号:{0}   批次：{1}", CarNo, DepartureBatch)) != DialogResult.Yes) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", myGridView1.GetRowCellValue(rowhandle, "DepartureBatch")));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    CommonClass.SetOperLog(DepartureBatch, "", "", CommonClass.UserInfo.UserName, "到车确认", "在途车辆到车确认操作");
                    myGridView1.DeleteRow(rowhandle);
                    CommonSyn.TraceSyn(DepartureBatch, null, 7, "货物到达", 1, "货物到达", null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            showdetail(myGridView1, 1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            simpleButton3_Click(sender, null);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnRefresh_Click(sender, null);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnConfrm_Click(sender, null);
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnGetList_Click(sender, null);
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnFetch_Click(sender, null);
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            simpleButton7_Click(sender, null);
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView5.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SendBatch", myGridView5.GetRowCellValue(rowhandle, "SendBatch").ToString()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_SENDSHORTCONN_CANCEL", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView5.DeleteRow(rowhandle);
                    myGridView5.PostEditor();
                    myGridView5.UpdateCurrentRow();
                    myGridView5.UpdateSummary();
                    DataTable dt = myGridControl5.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView5, "接驳确认记录");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView5.FocusedRowHandle;
            if (rowhandle < 0) return;
            //查看明细
            MyGridView gv = (MyGridView)myGridControl5.MainView;
            if (gv.FocusedRowHandle < 0) return;
            JMfrmSendDetailForConfirm ws = new JMfrmSendDetailForConfirm();
            ws.gv = gv;
            ws.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}