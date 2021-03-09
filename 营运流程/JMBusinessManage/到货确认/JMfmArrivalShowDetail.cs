using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class JMfmArrivalShowDetail : BaseForm
    {
        string _departureBatch;
        int _hitorder;
        DateTime _arriveddate;
        GridColumn gcBespeakContent;
        List<int> editRows = new List<int>();

        public int Hitorder
        {
            get { return _hitorder; }
            set { _hitorder = value; }
        }

        public string DepartureBatch
        {
            get { return _departureBatch; }
            set { _departureBatch = value; }
        }

        public DateTime Arriveddate
        {
            get { return _arriveddate; }
            set { _arriveddate = value; }
        }

        public JMfmArrivalShowDetail()
        {
            InitializeComponent();
        }

        private void fmArrivalShowDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            gcBespeakContent = myGridView1.Columns["BespeakContent"];

            if (_hitorder == 1)
            {
                //simpleButton1.Visible = simpleButton7.Visible = false;

                getbilnos_inroad();
                this.Text = "在途清单";
            }
            if (_hitorder == 2)
            {
                getbilnos_arrived();
            }

            getDepartureInfo();
        }

        /// <summary>
        /// 提取在途明细
        /// </summary>
        private void getbilnos_inroad()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("site", CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_INROAD_FCD", list);
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

        /// <summary>
        /// 提取到货明细
        /// </summary>
        private void getbilnos_arrived()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD", list);
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 7
                   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 8)
                {
                    MsgBox.ShowOK("该单已经出库或未到货，无法取消到货！");
                    return;
                }
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("确定取消到货？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
            list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
            list.Add(new SqlPara("BillNoStr", billnostr));
            list.Add(new SqlPara("man", ""));
            list.Add(new SqlPara("type", 2));

            //提前获取到轨迹信息
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", null));
            lists.Add(new SqlPara("BillNO", billnostr));
            lists.Add(new SqlPara("tracetype", "货物到达"));
            lists.Add(new SqlPara("num", 7));
            DataSet dss = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_TraceSyn", lists));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_UPDATE_BILL_ARRIVED_OK", list)) == 0) return;
            CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "取消到货", "到货清单取消到货操作");

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            MsgBox.ShowOK();
            CommonSyn.TraceSyn(null, billnostr, 7, "货物到达", 2, null,dss);
            #region 原有的单票取消到货
            /*
            string billno = "";
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                int state = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));
                if (state != 7 && state != 8)
                {
                    MsgBox.ShowOK("该单已经出库或未到货，无法取消到货!\r\nr如果确实需要取消到货,请先取消出库!");
                    return;
                }
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));

                DialogResult dl = XtraMessageBox.Show("单号：" + billno + " 将设置成在途状态,确认吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dl == DialogResult.Yes)
                {
                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                        list.Add(new SqlPara("BillNo", billno));
                        list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                        list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_CANCEL_SINGLE", list);
                        if (SqlHelper.ExecteNonQuery(sps) > 0)
                        {
                            if (_hitorder == 1) getbilnos_inroad();
                            if (_hitorder == 2) getbilnos_arrived();
                            MsgBox.ShowOK();
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowException(ex);
                    }
                }
            }
            */
            #endregion
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            string billnostr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) == 0) continue;
                billnostr += GridOper.GetRowCellValueString(myGridView1, i, "BillNO") + "@";

                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 5
                   && ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "BillState")) != 6)
                {
                    MsgBox.ShowOK("运单未在途，不需要到货！");
                    return;
                }
            }
            if (billnostr == "") return;
            if (MsgBox.ShowYesNo("确定将选中的单到货？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            list.Add(new SqlPara("BillNoStr", billnostr));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_OK_SING", list)) == 0) return;

            if (_hitorder == 1) getbilnos_inroad();
            if (_hitorder == 2) getbilnos_arrived();
            MsgBox.ShowOK();
            CommonSyn.TraceSyn(null, billnostr, 7, "货物到达", 1, null, null);
            #region 原有的单票到货
            /*
            int state = 0;
            string billno = "";

            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                billno = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNO"));
                state = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "BillState"));

                if (state >= 7)
                {
                    XtraMessageBox.Show("单号为：" + billno + "不能设置到货状态,这票货已经是到货的状态!", "系统提示", MessageBoxButtons.OK);
                    return;
                }

                if (XtraMessageBox.Show("单号为：" + billno + "将设置为到货状态，确认吗？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", DepartureBatch));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("AcceptWebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("DepName", CommonClass.UserInfo.DepartName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_ARRIVED_STATE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    if (_hitorder == 1) getbilnos_inroad();
                    if (_hitorder == 2) getbilnos_arrived();
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
            */
            #endregion
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 获取
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", _departureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            DataRow dr = ds.Tables[0].Rows[0];//取第0行
            ContractNO.EditValue = dr["ContractNO"];
            DepartureBatchs.EditValue = dr["DepartureBatch"];
            CarNO.EditValue = dr["CarNO"];
            CarrNO.EditValue = dr["CarrNO"];
            DriverName.EditValue = dr["DriverName"];
            DriverPhone.EditValue = dr["DriverPhone"];
            LoadWeight.EditValue = dr["LoadWeight"];
            LoadVolume.EditValue = dr["LoadVolume"];
            BeginSite.EditValue = dr["BeginSite"];
            EndSite.EditValue = dr["EndSite"];
            ActWeight.EditValue = dr["ActWeight"];
            ActVolume.EditValue = dr["ActVolume"];
            DepartureDate.EditValue = dr["DepartureDate"];
            ExpArriveDate.EditValue = dr["ExpArriveDate"];
            LoadPeoples.EditValue = dr["LoadPeoples"];
            Creator.EditValue = dr["Creator"];
            LoadingType.EditValue = dr["LoadingType"];
            BoxNO.EditValue = dr["BoxNO"];
            BigCarDescr.EditValue = dr["BigCarDescr"];
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(myGridView1.FocusedRowHandle);

            if (dr == null) return;

            JMfrmAppointmentSend frm = new JMfrmAppointmentSend();
            frm.crrBillNO = dr["BillNO"].ToString();
            frm.ShowDialog();

            //billnostr
            //if (simpleButton2.Text == "预约送货")
            //{
            //    simpleButton2.Text = "保存信息";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = true;
            //    myGridView1.FocusedColumn = gcBespeakContent;
            //}
            //else
            //{
            //    simpleButton2.Text = "预约送货";
            //    gcBespeakContent.OptionsColumn.AllowEdit = gcBespeakContent.OptionsColumn.AllowFocus = false;
            //    //保存预约送货信息
            //    string billnostr = "", bespeakContentstr = "";
            //    for (int i = 0; i < editRows.Count; i++)
            //    {
            //        billnostr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], "BillNO")) + "@";
            //        bespeakContentstr += ConvertType.ToString(myGridView1.GetRowCellValue(editRows[i], gcBespeakContent)).Replace('@', '_') + "@";
            //    }
            //    editRows.Clear();
            //    if (billnostr == "") return;

            //    List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("BillNoStr", billnostr));
            //    list.Add(new SqlPara("BespeakContentStr", bespeakContentstr));
            //    if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_SEND_BESPEAKCONTENT", list)) == 0) return;
            //    CommonClass.SetOperLog(_departureBatch, "", "", CommonClass.UserInfo.UserName, "预约送货", "到货清单预约送货操作");
            //    MsgBox.ShowOK();
            //}
        }

        private void myGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRows.Contains(e.RowHandle)) return;
            editRows.Add(e.RowHandle);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //sms.nowsendsms_to_shipper(myGridView1, _arriveddate);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //sms.nowsendsms(myGridView1, this, "1");
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            if (DepartureBatch == "") return;
            string middleSite = "";
            JMfrmSelectPrintDepartureList fsp = new JMfrmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView1.GetRowCellValue(i, "TransferSite"));
                if (middleSite == "") continue;
                flag = false;
                for (int j = 0; j < fsp.checkedListBox1.Items.Count; j++)
                {
                    if (ConvertType.ToString(fsp.checkedListBox1.Items[j]) == middleSite) flag = true;
                }
                if (!flag) fsp.checkedListBox1.Items.Add(middleSite);
            }
            if (fsp.ShowDialog() != DialogResult.OK) return;

            if (fsp.printSite == "")
            {
                MsgBox.ShowOK("没有选择打印站点!");
                return;
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite) }));
            if (ds == null || ds.Tables.Count == 0) return;

            string tmps = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                tmps = ConvertType.ToString(ds.Tables[0].Rows[i]["DestinationSite"]);
                if (tmps == "") continue;
                try { ds.Tables[0].Rows[i]["DestinationSite"] = tmps.Split(' ')[1]; }
                catch { }
            }

            //zaj 2018-1-15 司机运输协议根据公司ID来加载
            string transprotocol = CommonClass.UserInfo.Transprotocol == "" ? "司机运输协议" : CommonClass.UserInfo.Transprotocol;
           // frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : "司机运输协议"), ds);
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : transprotocol), ds);

            fpr.ShowDialog();

            //if (string.IsNullOrEmpty(DepartureBatch)) return;
            //frmPrintRuiLang fprl = new frmPrintRuiLang("到货清单", SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ARRIVED_FCD_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", DepartureBatch) })));
            //fprl.ShowDialog();
        }

        private void barbtnPrintQSD_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                MsgBox.ShowOK("请选择要打印的运单!");
                return;
            }
            string str = "";
            for (int i = 0; i < rows.Length; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNO")) + "@";
            }
            PrintQSD(str);
        }

        private void barbtnPrintGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount == 0)
            {
                MsgBox.ShowOK("没有运单，不能打印!");
                return;
            }
            string str = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                str += ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNO")) + "@";
            }
            PrintQSD(str);
        }

        private void PrintQSD(string BillNoStr)
        {
            if (string.IsNullOrEmpty(BillNoStr)) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr), new SqlPara("DepartureBatch", DepartureBatch) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //DataTable dt = ds.Tables[0].Clone();
            //frmPrintRuiLang fprl;
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    dt.ImportRow(ds.Tables[0].Rows[i]);
            //    //fprl = new frmPrintRuiLang("提货单", dt);
            //    //fprl.ShowDialog();
            //}
            //frmRuiLangService.Print("提货单", ds.Tables[0]);
            //jl20181127
            if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                || CommonClass.UserInfo.WebName == "杭州操作部"
                || CommonClass.UserInfo.WebName == "杭州操作部1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "宁波操作部"
                || CommonClass.UserInfo.WebName == "宁波操作部1"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                || CommonClass.UserInfo.WebName == "广州操作部"
                || CommonClass.UserInfo.WebName == "广州操作部1"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
            {
                frmRuiLangService.Print("提货单大坪", ds.Tables[0]);
            }
            else
            {
                frmRuiLangService.Print("提货单", ds.Tables[0]);
            }
        }

        private void ddbtnPrintQSD_Click(object sender, EventArgs e)
        {
            ddbtnPrintQSD.ShowDropDown();
        }
    }
}