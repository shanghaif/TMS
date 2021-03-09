using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Reflection;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmDepartureModyCount : BaseForm
    {
        public frmDepartureModyCount()
        {
            InitializeComponent();
        }

        public string sDepartureBatch = "";
        public object _arriveDate = null;
        private int state = 0;//为1时不去获取接货单号
        GridColumn gcTransferMode;
        private bool isModify = false;
        public string DepartureBatch1;

        /// <summary>
        /// 表示是否修改
        /// </summary>
        public bool IsModify
        {
            get { return isModify; }
        }

        /// <summary>
        /// 编辑中转地的行
        /// </summary>
        List<int> editRowTransferSite = new List<int>();

        GridColumn gcTransferSite;// 中转地列

        private void frmDepartureModyCount_Load(object sender, EventArgs e)
        {
            //btnFetchData.Enabled = UserRight.GetRight("121586");//长途
            //simpleButton15.Enabled = UserRight.GetRight("121587");//整车
            //simpleButton16.Enabled = UserRight.GetRight("121588");//项目
            //simpleButton1.Enabled = UserRight.GetRight("121595");//审核
            //simpleButton4.Enabled = UserRight.GetRight("121596");//反审核

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);

            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView2, barSubItem2);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            gcTransferMode = GridOper.GetGridViewColumn(myGridView2, "TransferMode");

            gcTransferSite = GridOper.GetGridViewColumn(myGridView2, "TransferSite");
            RepositoryItemComboBox ricb = RepItemComboBox.CreateRepItemComboBox;
            gcTransferSite.ColumnEdit = ricb;
            CommonClass.SetSite(ricb, false);

            CommonClass.SetSite(UnloadAddr);
            CommonClass.SetSite(edesite1, false);
            CommonClass.SetSite(edesite2, false);
            CommonClass.SetSite(edesite3, false);

            if (edesite1.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite1.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            if (edesite2.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite2.Properties.Items.Remove(CommonClass.UserInfo.SiteName);
            if (edesite3.Properties.Items.Contains(CommonClass.UserInfo.SiteName))
                edesite3.Properties.Items.Remove(CommonClass.UserInfo.SiteName);

            CommonClass.SetWeb(ShtBargeDept, CommonClass.UserInfo.SiteName, false);
            CommonClass.SetWeb(TakeDept, CommonClass.UserInfo.SiteName, false);

            GridOper.CreateStyleFormatCondition(myGridView2, "NumEqually", FormatConditionEnum.Equal, "否", Color.Green);
            RepositoryItemComboBox rep = RepItemImageComboBox.CreateRepItemComboBox(myGridView2, "TransferMode");
            string[] CustomTagModeList = CommonClass.Arg.TransferMode.Split(',');
            if (CustomTagModeList.Length > 0)
            {
                for (int i = 0; i < CustomTagModeList.Length; i++)
                {
                    rep.Items.Add(CustomTagModeList[i]);
                }
            }
            //把司机直送的交接方式去掉 hj 20171222
            //rep.Items.Add("司机直送");
            //根据发车批次号  取得本次列车发车清单
            freshData();
            getDepartureInfo();
            string[] VehicleTypes = CommonClass.Arg.VehicleType.Split(',');
            for (int i = 0; i < VehicleTypes.Length; i++)
            {
                CarType.Properties.Items.Add(VehicleTypes[i]);
            }
            //if (CommonClass.UserInfo.UserDB == UserDB.ZQTMS3PL) 
            //{
            //    AccTakeCar.Text = "";
            //    AccCollectPremium.Text = "";
            //}
        }

        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_DEPARTUREBATCH", list);
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

        /// <summary>
        /// 获取
        /// </summary>
        private void getDepartureInfo()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH", list));
            if (ds == null || ds.Tables.Count == 0) return;
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];//取第0行
                ContractNO.EditValue = dr["ContractNO"]; labContractNO.Text = ContractNO.Text;
                DepartureBatch.EditValue = dr["DepartureBatch"]; labDepartureBatch.Text = DepartureBatch.Text;
                CarNO.EditValue = dr["CarNO"]; labCarNO.Text = dr["CarNO"].ToString();
                CarrNO.EditValue = dr["CarrNO"];
                DriverName.EditValue = dr["DriverName"]; labDriverName.Text = DriverName.Text;
                DriverPhone.EditValue = dr["DriverPhone"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolume.EditValue = dr["LoadVolume"];
                BeginSite.EditValue = dr["BeginSite"];
                EndSite.EditValue = dr["EndSite"];
                EndWeb.EditValue = dr["EndWeb"];//新增目的网点 hj20180417
                ActWeight.EditValue = dr["ActWeight"];
                ActVolume.EditValue = dr["ActVolume"];
                DepartureDate.EditValue = dr["DepartureDate"]; labDepartureDate.Text = DepartureDate.Text;
                ExpArriveDate.EditValue = dr["ExpArriveDate"];
                LoadPeoples.EditValue = dr["LoadPeoples"]; labLoadPeoples.Text = LoadPeoples.Text;
                Creator.EditValue = dr["Creator"]; lbcreateby.Text = Creator.Text;
                LoadingType.EditValue = dr["LoadingType"];
                CarType.EditValue = dr["CarType"];
                CarLength.EditValue = dr["CarLength"];
                CarWidth.EditValue = dr["CarWidth"];
                CarHeight.EditValue = dr["CarHeight"];
                NetWeight.EditValue = dr["NetWeight"];
                EndWebName.EditValue = dr["EndWebName"];
                BoxNO.EditValue = dr["BoxNO"];
                BigCarDescr.EditValue = dr["BigCarDescr"];
                AccLine.EditValue = dr["AccLine"];
                FecthSite.EditValue = dr["FecthSite"];
                AccBigCarFecth.EditValue = dr["AccBigCarFecth"];
                FecthBillNO.EditValue = dr["FecthBillNO"];
                AccBigCarSend.EditValue = dr["AccBigCarSend"];
                SendBillNO.EditValue = dr["SendBillNO"];
                SendAddr.EditValue = dr["SendAddr"];
                AccShtBarge.EditValue = dr["AccShtBarge"];
                ShtBargeDept.EditValue = dr["ShtBargeDept"];
                AccSomeLoad.EditValue = dr["AccSomeLoad"];
                UnloadAddr.EditValue = dr["UnloadAddr"];
                AccBigcarOther.EditValue = dr["AccBigcarOther"];
                TakeDept.EditValue = dr["TakeDept"];
                DriverTakePay.EditValue = dr["DriverTakePay"];
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccCollectPremium.EditValue = dr["AccCollectPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                ToPayDriver.EditValue = dr["ToPayDriver"];
                AccBigcarTotal.EditValue = dr["AccBigcarTotal"];//大车费合计
                DriverTakePay.EditValue = dr["DriverTakePay"];
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccCollectPremium.EditValue = dr["AccCollectPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                CarSoure.EditValue = dr["CarSoure"];
                edrepremark.EditValue = dr["BigCarDescr"];
                OilCardNo.EditValue = dr["OilCardNo"];
                OilCardFee.EditValue = dr["OilCardFee"];
                _arriveDate = dr["ArrivedDate"];

                //if (LoadingType.Text == "长途配载")
                //{
                //    btnFetchData.Enabled = true;
                //    simpleButton15.Enabled = simpleButton16.Enabled = false;
                //}
                //else if (LoadingType.Text == "整车配载")
                //{
                //    simpleButton15.Enabled = true;
                //    btnFetchData.Enabled = simpleButton16.Enabled = false;
                //}
                //else//中强项目
                //{
                //    simpleButton16.Enabled = true;
                //    btnFetchData.Enabled = simpleButton15.Enabled = false;
                //}
                BackPayDriver.EditValue = dr["BackPayDriver"];
                OilFee.EditValue = dr["OilFee"];
                oilVolume.EditValue = dr["oilVolume"];
                oilPrice.EditValue = dr["oilPrice"];
                try
                {
                    int isover = ConvertType.ToInt32(dr["isover"]);
                    SetButtonState(isover == 0);
                }
                catch { }
            }
            //取到付驾驶员
            if (ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0) return;
            try
            {
                edesite1.EditValue = ds.Tables[1].Rows[0][0];
                edesiteacc1.EditValue = ds.Tables[1].Rows[0][1];
                edesite2.EditValue = ds.Tables[1].Rows[1][0];
                edesiteacc2.EditValue = ds.Tables[1].Rows[1][1];
                edesite3.EditValue = ds.Tables[1].Rows[2][0];
                edesiteacc3.EditValue = ds.Tables[1].Rows[2][1];
            }
            catch { }

            if (myGridControl2.DataSource == null) return;
            DataTable dt = myGridControl2.DataSource as DataTable;
            object obj = dt.Compute("sum(FetchPay)", "TransferMode='司机直送' and PaymentMode='提付'");
            decimal FetchPay = obj == null || obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
            decimal driverTakePay = ConvertType.ToDecimal(DriverTakePay.EditValue);
            if (driverTakePay == 0 && ConvertType.ToDecimal(ds.Tables[0].Rows[0]["BackPayDriver"]) == 0)
            {
                DriverTakePay.EditValue = FetchPay;
            }
        }

        private void btnFetchData_Click(object sender, EventArgs e)
        {
            getdata(1);
        }

        private void getdata(int type)
        {
            //取得库存   121591:打印后加货权限
            if (!UserRight.GetRight("121591") && HtIsPrint(sDepartureBatch))
            {
                MsgBox.ShowOK("本车已打印，不能加货，如需加货请联系相关人员授权!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadType", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private bool HtIsPrint(string sDepartureBatch)
        {
            //判断发车合同是否已经打印
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return false;

                if (ConvertType.ToString(ds.Tables[0].Rows[0][0]) != "" && ConvertType.ToString(ds.Tables[0].Rows[0][1]) != "" && ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //加入库存
            int[] rows = myGridView1.GetSelectedRows();
            if (rows.Length == 0)
            {
                XtraMessageBox.Show("请单击选择要加入的运单!\r\n也可以按住Ctrl键,然后在要加入的运单上单击鼠标左键以选择多票!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //HJ 20180411
            DataSet ds = null;
            try
            {
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
                ds = SqlHelper.GetDataSet(sps);
                if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                {
                    MsgBox.ShowOK("本车已经打印司机运输协议，不能加入!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            if (XtraMessageBox.Show("确定将选中的运单加入到本车清单吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            try
            {
                string BillNos = "", factqtys = "";
                for (int i = 0; i < rows.Length; i++)
                {
                    BillNos += ConvertType.ToString(myGridView1.GetRowCellValue(rows[i], "BillNo")) + ",";
                    factqtys += ConvertType.ToInt32(myGridView1.GetRowCellValue(rows[i], "factqty")) + ",";
                }
                if (BillNos == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list.Add(new SqlPara("CarNO", labCarNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", labDriverName.Text.Trim()));
                list.Add(new SqlPara("WebDate", labDepartureDate.Text.Trim()));
                list.Add(new SqlPara("BillNOStr", BillNos));
                list.Add(new SqlPara("FactQtyStr", factqtys));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTURELIST", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteSelectedRows();

                    //CommonSyn.BillDepartureSyn(BillNos, sDepartureBatch);//zaj 2018-4-18 分拨同步
                    //yzw 新同步
                    CommonSyn.BillDepartureSynNew(BillNos, sDepartureBatch, factqtys);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //CommonSyn.TimeDepartUptSyn(BillNos.Replace(",", "@"), sDepartureBatch, "", ds.Tables[0].Rows[0]["LoadWeb"].ToString(), CommonClass.UserInfo.WebName, "USP_ADD_BILLDEPARTURELIST");//同步配载修改时效 LD 2018-4-27
                    }
                    //CommonSyn.TraceSyn(null, BillNos.Replace(",", "@"), 6, "车辆出发", 1, null, null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                //更新列表
                freshData();
            }
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, "ischecked", a);
            }
        }

        private void cbdeleteall_Click(object sender, EventArgs e)
        {
            //zaj 2018-4-12 去掉限制
            //if (!UserRight.GetRight("121591") && HtIsPrint(sDepartureBatch))
            //{
            //    MsgBox.ShowOK("本车已打印，不能整车作废，如需整车作废，请联系相关人员授权!");
            //    return;
            //}
            try
            {
                string billNos = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ToDate")) != "")
                    {
                        MsgBox.ShowOK("本车已经有到货,不能作废!\r\n如果确实需要作废,请联系本车到站负责人员取消本车到货!");
                        return;
                    }

                    if (CommonClass.QSP_LOCK_1(myGridView2.GetRowCellValue(i, "BillNO").ToString(), myGridView2.GetRowCellValue(i, "BillDate").ToString()))
                    {
                        MsgBox.ShowOK("本车已经有运单被锁账，不能整车作废!");
                        return;
                    }
                    billNos = billNos + myGridView2.GetRowCellValue(i, "BillNO").ToString() + "@";

                }

                if (MsgBox.ShowYesNo("确定作废本车？\r\n批次：" + sDepartureBatch) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定作废本车？请三思！！\r\n批次：" + sDepartureBatch) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTURE", list)) == 0) return;
                MsgBox.ShowOK("本车作废成功!");
               // cbcancelover.Enabled = true;
                this.Close();
                CommonSyn.DepartureDeleteSyn(sDepartureBatch, billNos, 0);//zaj 2018-4-11 分拨同步
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void cbdeletesingle_Click(object sender, EventArgs e)
        {
            //if (myGridView2.RowCount == 1)
            //{
            //    if (MsgBox.ShowYesNo("本车只剩下一票了,如果剔除当前运单,本车整个车辆也将作废!\r\n\r\n确定要继续吗?") == DialogResult.Yes)
            //    {
            //        cbdeleteall.PerformClick();
            //    }
            //    return;
            //}

            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                string billno = GridOper.GetRowCellValueString(myGridView2, "BillNO");
                string billdate = GridOper.GetRowCellValueString(myGridView2, "BillDate");
                string departureBatch = ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"));
                if (CommonClass.QSP_LOCK_1(billno, billdate))
                {
                    MsgBox.ShowOK("本运单被锁账，不能剔除!");
                    return;
                }
                //hj 20180411
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                {
                    MsgBox.ShowOK("本车已经打印司机运输协议，不能单票剔除!");
                    return;
                }


                if (ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "ToDate")) != "")
                {
                    MsgBox.ShowOK("本车已经到货,不能剔除!\r\n如果确实需要剔除,请联系本车到站负责人员取消本车到货!");
                    return;
                }

                if (MsgBox.ShowYesNo("确定要剔除本票？\r\n单号：" + billno) != DialogResult.Yes) return;
                if (MsgBox.ShowYesNo("确定要剔除本票？请三思！！\r\n单号：" + billno) != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                list.Add(new SqlPara("BillNo", billno));
                list.Add(new SqlPara("WebPCS", ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "WebPCS"))));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_DEPARTURE_SING", list)) == 0) return;
                myGridView2.DeleteRow(rowhandle);
                MsgBox.ShowOK();
                myGridView2.ClearColumnsFilter();
                CommonSyn.DepartureDeleteSyn(departureBatch, billno + "@", 1);//zaj 2018-4-11 分拨同步
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载明细清单");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载库存清单");
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            groupControl1.Visible = !groupControl1.Visible;
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            decimal accBigcarTotal = ConvertType.ToDecimal(AccBigcarTotal.Text);
            if (accBigcarTotal <= 0)
            {
                if (MsgBox.ShowYesNo("合同没保存运费，确定完成本车？") != DialogResult.Yes) return;
            }
            if (!SetBillDepartureState(1)) return;
            isModify = true;
            SetButtonState(false);
            if (!HtIsPrint(DepartureBatch.Text.Trim())) printcontract();
        }

        /// <summary>
        /// 保存大车状态
        /// </summary>
        /// <param name="state">0未完成,1已完成</param>
        private bool SetBillDepartureState(int state)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            list.Add(new SqlPara("state", state));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_BILLDEPARTURE_STATE", list)) == 0) return false;
            MsgBox.ShowOK();
            return true;
        }

        /// <summary>
        /// 设置按钮状态
        /// </summary>
        /// <param name="state">控件启用状态</param>
        private void SetButtonState(bool state)
        {
            //simpleButton12.Enabled = simpleButton18.Enabled = simpleButton11.Enabled = state;
            //cbcancelover.Enabled = !state;

            //btnAdd.Enabled = state && UserRight.GetRight("121589");//加货
           // simpleButton17.Enabled = state && UserRight.GetRight("121592");//强制装货
            //cbdeletesingle.Enabled = state && UserRight.GetRight("121593");//单票剔除
            //cbdeleteall.Enabled = state && UserRight.GetRight("ZQTMS.UI.frmDeparture#barBtnVehicleDel");//整车作废
            //simpleButton3.Enabled = state && UserRight.GetRight("121594");//保存合同
            //simpleButton19.Enabled = state && UserRight.GetRight("121594");//保存合同
        }

        private void cbcancelover_Click(object sender, EventArgs e)
        {
            if (!SetBillDepartureState(0)) return;
            isModify = true;
            SetButtonState(true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (CommonClass.QSP_LOCK_7(DepartureDate.Text))
            {
                return;
            }
            if (!UserRight.GetRight("121597") && HtIsPrint(sDepartureBatch))
            {
                MsgBox.ShowOK("本车已打印，不能修改合同信息，如需修改，请联系相关人员授权!");
                return;
            }

            string vehicleno = CarNO.Text.Trim();
            if (vehicleno == "")
            {
                MsgBox.ShowError("车牌号必须填写!");
                CarNO.Focus();
                return;
            }
            //hj20171213 取消车厢号限制
            //if (CarrNO.Text.Trim() == "")
            //{
            //    MsgBox.ShowError("车厢号必须填写!");
            //    CarNO.Focus();
            //    return;
            //}
            if (CarType.Text.Trim() == "")
            {
                MsgBox.ShowError("车型必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarLength.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车长必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarWidth.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车宽必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarHeight.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车高必须填写!");
                CarNO.Focus();
                return;
            }
            if (CarSoure.Text.Trim() == "")
            {
                MsgBox.ShowError("车源必须填写!");
                CarNO.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("请选择配载类型!");
                LoadingType.Focus();
                return;
            }
            decimal accLine = ConvertType.ToDecimal(AccLine.Text);
            if (accLine == 0)
            {
                MsgBox.ShowError("请填写干线运费!");
                AccLine.Focus();
                return;
            }
            decimal accBigCarFecth = ConvertType.ToDecimal(AccBigCarFecth.Text);
            string fecthBillNO = FecthBillNO.Text.Trim();
            if (accBigCarFecth != 0 && fecthBillNO == "")
            {
                MsgBox.ShowError("您填了大车接货费,必须填写接货单号!");
                FecthBillNO.Focus();
                return;
            }
            decimal accBigCarSend = ConvertType.ToDecimal(AccBigCarSend.Text);
            string sendBillNO = SendBillNO.Text.Trim();
            if (accBigCarSend != 0 && sendBillNO == "")
            {
                MsgBox.ShowError("您填了大车送货费,必须填写送货单号(多个送货单号用‘/’隔开)!");
                SendBillNO.Focus();
                return;
            }
            decimal accShtBarge = ConvertType.ToDecimal(AccShtBarge.Text);
            string shtBargeDept = ShtBargeDept.Text.Trim();
            if (accShtBarge != 0 && shtBargeDept == "")
            {
                MsgBox.ShowError("您填了短驳费,必须填写短驳费的承运部门!");
                ShtBargeDept.Focus();
                return;
            }
            decimal accBigcarOther = ConvertType.ToDecimal(AccBigcarOther.Text);
            string takeDept = TakeDept.Text.Trim();
            if (accBigcarOther != 0 && takeDept == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写大车其他费的承运部门!");
                TakeDept.Focus();
                return;
            }
            string bigcarOtherRemark = BigcarOtherRemark.Text.Trim();
            if (accBigcarOther != 0 && bigcarOtherRemark == "")
            {
                MsgBox.ShowError("您填了大车其他费,必须填写其他费备注!");
                BigcarOtherRemark.Focus();
                return;
            }
            decimal oilCardFee = ConvertType.ToDecimal(OilCardFee.Text);
            string oilCardNo = OilCardNo.Text.Trim();
            if (oilCardFee != 0 && oilCardNo == "")
            {
                MsgBox.ShowError("您填了油卡金额,必须填写油卡编号!");
                OilCardNo.Focus();
                return;
            }
            decimal driverTakePay = 0, accTakeCar = 0, accCollectPremium = 0, nowPayDriver = 0, toPayDriver = 0, backPayDriver = 0;
            driverTakePay = ConvertType.ToDecimal(DriverTakePay.Text);//司机代收
            accTakeCar = ConvertType.ToDecimal(AccTakeCar.Text);//代收派车费
            accCollectPremium = ConvertType.ToDecimal(AccCollectPremium.Text);//代收保险费
            nowPayDriver = ConvertType.ToDecimal(NowPayDriver.Text);
            toPayDriver = ConvertType.ToDecimal(ToPayDriver.Text);
            backPayDriver = ConvertType.ToDecimal(BackPayDriver.Text);
            if (ConvertType.ToDecimal(AccBigcarTotal.Text) != (driverTakePay + accTakeCar + accCollectPremium + nowPayDriver + toPayDriver + backPayDriver + oilCardFee))
            {
                MsgBox.ShowError("大车费合计=司机代收款+代收派车费+代收保险费+现付驾驶员+到付驾驶员+回付驾驶员+油卡金额,请检查!");
                return;
            }
            string esite1 = edesite1.Text.Trim();
            decimal eacc1 = esite1 == "" ? 0 : ConvertType.ToDecimal(edesiteacc1.Text);
            if (esite1 == "" && eacc1 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite1));
                edesiteacc1.Focus();
                return;
            }
            string esite2 = edesite2.Text.Trim();
            decimal eacc2 = esite2 == "" ? 0 : ConvertType.ToDecimal(edesiteacc2.Text);
            if (esite2 == "" && eacc2 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite2));
                edesiteacc2.Focus();
                return;
            }
            string esite3 = edesite3.Text.Trim();
            decimal eacc3 = esite3 == "" ? 0 : ConvertType.ToDecimal(edesiteacc3.Text);
            if (esite3 == "" && eacc3 != 0)
            {
                MsgBox.ShowError(string.Format("您填了到站点‘{0}’,必须填{0}的费用!", esite3));
                edesiteacc3.Focus();
                return;
            }

            if (myGridControl2.DataSource == null)
            {
                MsgBox.ShowError("发车明细列表没有数据!");
                return;
            }
            DataTable dt = myGridControl2.DataSource as DataTable;
            decimal sf = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ConvertType.ToString(dt.Rows[i]["TransferMode"]) == "司机直送" && ConvertType.ToString(dt.Rows[i]["PaymentMode"]) == "提付")
                    sf += ConvertType.ToDecimal(dt.Rows[i]["FetchPay"]);
            }
            //object obj = dt.Compute("sum(FetchPay)", "TransferMode='司机直送' and PaymentMode='提付'");
            //decimal FetchPay = obj == null || obj == DBNull.Value ? 0 : Convert.ToDecimal(obj);
            decimal FetchPay = sf;

            if (driverTakePay < FetchPay)
            {
                string msg = string.Format("本车司机直送的提付合计为{0}\r\n司机代收款不能低于此金额!", FetchPay);
                MsgBox.ShowOK(msg);
                return;
            }

            if (MsgBox.ShowYesNo("是否保存合同？") != DialogResult.Yes) return;

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
                list.Add(new SqlPara("EndSite", EndSite.Text.Trim()));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                //预到日期
                if (ExpArriveDate.Text.Trim() == "")
                    list.Add(new SqlPara("ExpArriveDate", DBNull.Value));
                else
                    list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));

                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", LoadPeoples.Text.Trim()));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", BoxNO.Text.Trim()));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                list.Add(new SqlPara("LoadingType", loadingType));
                list.Add(new SqlPara("AccLine", accLine));//干线运费
                list.Add(new SqlPara("FecthSite", FecthSite.Text.Trim()));//接货地
                list.Add(new SqlPara("AccBigCarFecth", accBigCarFecth));//大车接货费
                list.Add(new SqlPara("FecthBillNO", fecthBillNO));
                list.Add(new SqlPara("AccBigCarSend", accBigCarSend));
                list.Add(new SqlPara("SendBillNO", sendBillNO));
                list.Add(new SqlPara("SendAddr", SendAddr.Text.Trim()));
                list.Add(new SqlPara("AccShtBarge", ConvertType.ToDecimal(AccShtBarge.Text)));
                list.Add(new SqlPara("ShtBargeDept", ShtBargeDept.Text.Trim()));
                list.Add(new SqlPara("AccSomeLoad", ConvertType.ToDecimal(AccSomeLoad.Text)));
                list.Add(new SqlPara("UnloadAddr", UnloadAddr.Text));
                list.Add(new SqlPara("AccBigcarOther", accBigcarOther));
                list.Add(new SqlPara("TakeDept", takeDept));
                list.Add(new SqlPara("AccBigcarTotal", ConvertType.ToDecimal(AccBigcarTotal.Text)));
                list.Add(new SqlPara("DriverTakePay", driverTakePay));
                list.Add(new SqlPara("AccTakeCar", accTakeCar));
                list.Add(new SqlPara("AccCollectPremium", accCollectPremium));
                list.Add(new SqlPara("NowPayDriver", nowPayDriver));
                list.Add(new SqlPara("ToPayDriver", toPayDriver));
                list.Add(new SqlPara("BackPayDriver", backPayDriver));
                list.Add(new SqlPara("edesite1", esite1));
                list.Add(new SqlPara("edesiteacc1", eacc1));
                list.Add(new SqlPara("edesite2", esite2));
                list.Add(new SqlPara("edesiteacc2", eacc2));
                list.Add(new SqlPara("edesite3", esite3));
                list.Add(new SqlPara("edesiteacc3", eacc3));
                list.Add(new SqlPara("BigcarOtherRemark", bigcarOtherRemark));
                list.Add(new SqlPara("DeliCode", fecthBillNO));
                list.Add(new SqlPara("OilCardFee", oilCardFee));
                list.Add(new SqlPara("OilCardNo", oilCardNo));
                list.Add(new SqlPara("CarType", CarType.Text.Trim()));
                list.Add(new SqlPara("CarLength", CarLength.Text.Trim()));
                list.Add(new SqlPara("CarWidth", CarWidth.Text.Trim()));
                list.Add(new SqlPara("CarHeight", CarHeight.Text.Trim()));
                list.Add(new SqlPara("NetWeight", ConvertType.ToDecimal(NetWeight.Text)));
                list.Add(new SqlPara("CarSoure", CarSoure.Text.Trim()));
                list.Add(new SqlPara("oilPrice", ConvertType.ToDecimal(OilFee.Text)));
                list.Add(new SqlPara("oilId", oilId));
                list.Add(new SqlPara("oilVolume", ConvertType.ToDecimal(oilVolume.Text)));
                list.Add(new SqlPara("oilPrice1", ConvertType.ToDecimal(oilPrice.Text)));
                list.Add(new SqlPara("EndWeb", EndWeb.Text.Trim()));//新增目的网点 hj20180417
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLDEPARTURE", list)) == 0) return;
                isModify = true;
                MsgBox.ShowOK("合同保存成功!");
                if (!HtIsPrint(DepartureBatch.Text.Trim())) printcontract();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void printcontract() 
        {
            if (MsgBox.ShowYesNo("是否可以确认发车？（打印运输协议）") != DialogResult.Yes) return;
            if (sDepartureBatch == "") return;
            string middleSite = "";
            frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
                if (middleSite == "") continue;
                flag = false;
                for (int j = 0; j < fsp.checkedListBox1.Items.Count; j++)
                {
                    if (ConvertType.ToString(fsp.checkedListBox1.Items[j]) == middleSite) flag = true;
                }
                if (!flag) fsp.checkedListBox1.Items.Add(middleSite);
            }
            fsp.radioGroup2.SelectedIndex = 2;
            if (fsp.ShowDialog() != DialogResult.OK) return;

            if (fsp.printSite == "")
            {
                MsgBox.ShowOK("没有选择打印站点!");
                return;
            }
            //同步配载修改时效 LD 2018-5-24
            string strBillNo = string.Empty;
            List<SqlPara> lists = new List<SqlPara>();
            lists.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", lists);
            DataSet ds_check = SqlHelper.GetDataSet(sps);
            if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_check.Tables[0].Rows)
                {
                    strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
                }
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
            if (ds == null || ds.Tables.Count == 0) return;
            //同步配载修改时效 LD 2018-5-24
            if (!string.IsNullOrEmpty(strBillNo))
            {
                CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
            }

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
            string departList = CommonClass.UserInfo.DepartList == "" ? "配载清单" : CommonClass.UserInfo.DepartList;  //maohui20180315
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? "装车清单" : transprotocol), ds, CommonClass.UserInfo.gsqc);
            fpr.ShowDialog();
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || e.Value == null) return;
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            try
            {
                int leftNum = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "LeftNum"));
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

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void CalAccBigcarTotal(object sender, EventArgs e)
        {
            try
            {
                AccBigcarTotal.Text = (ConvertType.ToDecimal(AccLine.Text) + ConvertType.ToDecimal(AccBigCarFecth.Text) + ConvertType.ToDecimal(AccBigCarSend.Text) + ConvertType.ToDecimal(AccShtBarge.Text) + ConvertType.ToDecimal(AccSomeLoad.Text) + ConvertType.ToDecimal(AccBigcarOther.Text)).ToString();
            }
            catch (Exception ex)
            {
                AccBigcarTotal.Text = "";
                MsgBox.ShowError(ex.Message);
            }
        }

        /// <summary>
        /// 计算回付驾驶员
        /// </summary>
        private void CalAccBackPayDriver(object sender, EventArgs e)
        {
            BackPayDriver.Text = (ConvertType.ToDecimal(AccBigcarTotal.Text) - ConvertType.ToDecimal(DriverTakePay.Text) - ConvertType.ToDecimal(AccTakeCar.Text) - ConvertType.ToDecimal(AccCollectPremium.Text) - ConvertType.ToDecimal(NowPayDriver.Text) - ConvertType.ToDecimal(ToPayDriver.Text) - ConvertType.ToDecimal(OilCardFee.Text)).ToString();
        }

        private void edesiteacc1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite1.Text.Trim() == "")
                e.Cancel = true;
        }

        /// <summary>
        /// 计算到付司机
        /// </summary>
        private void CalToPayDriver(object sender, EventArgs e)
        {
            try
            {
                ToPayDriver.Text = (ConvertType.ToDecimal(edesiteacc1.Text) + ConvertType.ToDecimal(edesiteacc2.Text) + ConvertType.ToDecimal(edesiteacc3.Text)).ToString();
            }
            catch (Exception ex) { ToPayDriver.Text = ""; MsgBox.ShowError(ex.Message); }
        }

        private void frmDepartureModyCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!HtIsPrint(DepartureBatch.Text.Trim())) printcontract();
            //if (cbcancelover.Enabled) return;
            //if (MsgBox.ShowYesNo("本车还未完成，是否完成本车？") != DialogResult.Yes) return;
            //if (SetBillDepartureState(1)) isModify = true;
        }

        private void edesiteacc2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite2.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesiteacc3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (edesite3.Text.Trim() == "")
                e.Cancel = true;
        }

        private void edesite1_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite1.Text.Trim() == "")
            {
                edesiteacc1.EditValueChanging -= edesiteacc1_EditValueChanging;
                edesiteacc1.Text = "";
                edesiteacc1.EditValueChanging += edesiteacc1_EditValueChanging;
            }
        }

        private void edesite2_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite2.Text.Trim() == "")
            {
                edesiteacc2.EditValueChanging -= edesiteacc2_EditValueChanging;
                edesiteacc2.Text = "";
                edesiteacc2.EditValueChanging += edesiteacc2_EditValueChanging;
            }
        }

        private void edesite3_EditValueChanged(object sender, EventArgs e)
        {
            if (edesite3.Text.Trim() == "")
            {
                edesiteacc3.EditValueChanging -= edesiteacc3_EditValueChanging;
                edesiteacc3.Text = "";
                edesiteacc3.EditValueChanging += edesiteacc3_EditValueChanging;
            }
        }

        private void edesite1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite1.Text.Trim() == s || edesite3.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void edesite3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            string s = ConvertType.ToString(e.NewValue);
            if (s != "" && (edesite2.Text.Trim() == s || edesite1.Text.Trim() == s))
            {
                e.Cancel = true;
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            //发货人
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());
            sms.fcdsendsms(myGridView2, this, "1", senddate, ExpArriveDate.DateTime);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            DateTime senddate = Convert.ToDateTime(labDepartureDate.Text.ToString().Trim());

            sms.fcdsendsms_consignee(myGridView2, this, "1", senddate, labCarNO.Text.Trim());
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //货到前付
            if (_arriveDate == null)
            {
                if (XtraMessageBox.Show("本车未到货,确认发送货到前付短信通知?\r\n系统将默认到货时间为当前时间!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            sms.fukuansms(myGridView2, this, "1", _arriveDate == null ? CommonClass.gcdate : Convert.ToDateTime(_arriveDate));
        }

        private void cbprint_Click(object sender, EventArgs e)
        {
            if (sDepartureBatch == "") return;
            string middleSite = "";
            frmSelectPrintDepartureList fsp = new frmSelectPrintDepartureList();
            bool flag = false;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                middleSite = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransferSite"));
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

            //同步配载修改时效 LD 2018-5-24
            string strBillNo = string.Empty;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Check_BILLDEPARTURE", list);
            DataSet ds_check = SqlHelper.GetDataSet(sps);
            if (ds_check != null && ds_check.Tables.Count > 0 && ds_check.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds_check.Tables[0].Rows)
                {
                    strBillNo = strBillNo + (row["BillNO"].ToString() + "@");
                }
            }

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("MiddleSiteStr", fsp.printSite), new SqlPara("printType", fsp.printType) }));
            if (ds == null || ds.Tables.Count == 0) return;
            //同步配载修改时效 LD 2018-5-24
            if (!string.IsNullOrEmpty(strBillNo))
            {
                CommonSyn.TimeDepartUptSyn(strBillNo, sDepartureBatch, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CommonClass.UserInfo.WebName, CommonClass.UserInfo.WebName, "QSP_GET_DEPARTURELIST_BY_DEPARTUREBATCH_PRINT");
            }

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
            string departList = CommonClass.UserInfo.DepartList == "" ? "配载清单" : CommonClass.UserInfo.DepartList;  //maohui20180315
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : "司机运输协议"), ds);
            //frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? "配载清单" : fsp.printType == 1 ? "装车清单" : transprotocol), ds);
            frmPrintRuiLang fpr = new frmPrintRuiLang((fsp.printType == 0 ? departList : fsp.printType == 1 ? "装车清单" : transprotocol), ds, CommonClass.UserInfo.gsqc);

            fpr.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURELIST_PRINT", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("PrintMan", CommonClass.UserInfo.UserName) }));
            frmPrintRuiLang fpr = new frmPrintRuiLang("大车运输合同", ds);
            fpr.ShowDialog();
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (e.Page == tp2)
                freshData();
            if (e.Page != tp3) return;
            string BillNo = "", address = "", addressStr = "",
                fecthBillNO = FecthBillNO.Text.Trim(),
                sendBillNO = SendBillNO.Text.Trim();
            FecthBillNO.Properties.Items.Clear();
            SendBillNO.Properties.Items.Clear();
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView2.GetRowCellValue(i, GridOper.GetGridViewColumn(myGridView2, "TransferMode"))) == "司机直送")  //hj20180616 中心直送改为司机直送
                {
                    BillNo = GridOper.GetRowCellValueString(myGridView2, i, "BillNO");
                    address = GridOper.GetRowCellValueString(myGridView2, i, "ReceivAddress");
                    if (address != "") addressStr += address + ",";
                    if (BillNo != "" && !FecthBillNO.Properties.Items.Contains(BillNo))
                    {
                        FecthBillNO.Properties.Items.Add(BillNo);
                        SendBillNO.Properties.Items.Add(BillNo);
                    }
                }
            }
            FecthBillNO.Text = fecthBillNO;
            SendBillNO.Text = sendBillNO;
            SendAddr.Text = addressStr;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //审核
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATE", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("Man", CommonClass.UserInfo.UserName), new SqlPara("type", 1) })) == 0) return;
            MsgBox.ShowOK();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //反审核
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DEPARTURE_VERIFY_STATE", new List<SqlPara> { new SqlPara("DepartureBatch", sDepartureBatch), new SqlPara("type", 2) })) == 0) return;
            MsgBox.ShowOK();
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            getdata(2);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            getdata(3);
        }

        private void EndSite_EditValueChanged(object sender, EventArgs e)
        {
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string[] tmps = endSite.Split(',');
            if (tmps == null || tmps.Length == 0) return;

            edesite1.Text = edesite2.Text = edesite3.Text = "";
            try
            {
                edesite1.Text = tmps[0].Trim();
                edesite2.Text = tmps[1].Trim();
                edesite3.Text = tmps[2].Trim();
            }
            catch { }
            edesiteacc1.Text = edesite1.Text == "" ? "" : edesiteacc1.Text;
            edesiteacc2.Text = edesite2.Text == "" ? "" : edesiteacc2.Text;
            edesiteacc3.Text = edesite3.Text == "" ? "" : edesiteacc3.Text;

            //SetWeb();
        }

        public void SetWeb()
        {
            EndWebName.Properties.Items.Clear();
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string SiteName = "";
            foreach (string a in endSite.Split(','))
                SiteName += "'" + a + "',";
            SiteName = SiteName.TrimEnd(',');
            SiteName = "SiteName IN(" + SiteName + ")";
            DataRow[] drs = CommonClass.dsWeb.Tables[0].Select(SiteName);
            if (drs == null || drs.Length == 0) return;

            foreach (DataRow dr in drs)
            {
                endSite = ConvertType.ToString(dr["WebName"]);
                if (endSite != "" && !EndWebName.Properties.Items.Contains(endSite))
                    EndWebName.Properties.Items.Add(endSite);
            }
            EndWebName.Text = "";
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            if (simpleButton12.Text == "修正中转地")
            {
                simpleButton12.Text = "保存中转地";
                gcTransferSite.OptionsColumn.AllowEdit = gcTransferSite.OptionsColumn.AllowFocus = true;
                myGridView2.FocusedColumn = gcTransferSite;
            }
            else
            {
                myGridView2.PostEditor();
                simpleButton12.Text = "修正中转地";
                gcTransferSite.OptionsColumn.AllowEdit = gcTransferSite.OptionsColumn.AllowFocus = false;
                if (editRowTransferSite.Count == 0) return;

                string sbillno = "";
                string sTransferSite = "";

                for (int i = 0; i < editRowTransferSite.Count; i++)
                {
                    sbillno += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "BillNO")) + "@";
                    sTransferSite += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "TransferSite")) + "@";
                }
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", sbillno));
                    list.Add(new SqlPara("TransferSite", sTransferSite));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_WAYBILL_TransferSite", list);
                    if (SqlHelper.ExecteNonQuery(sps) == 0) return;
                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

        private void myGridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || editRowTransferSite.Contains(e.RowHandle)) return;
            editRowTransferSite.Add(e.RowHandle);
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            if (gcTransferMode == null) return;
            //hj20180411
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", ConvertType.ToString(myGridView2.GetRowCellValue(rowhandle, "DepartureBatch"))));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                {
                    MsgBox.ShowOK("本车已经打印司机运输协议，不能修改交接方式!");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

            if (simpleButton18.Text == "修正交接方式")
            {
                simpleButton18.Text = "保存交接方式";
                gcTransferMode.OptionsColumn.AllowEdit = gcTransferMode.OptionsColumn.AllowFocus = true;
                myGridView2.FocusedColumn = gcTransferMode;
            }
            else
            {
                myGridView2.PostEditor();
                simpleButton18.Text = "修正交接方式";
                gcTransferMode.OptionsColumn.AllowEdit = gcTransferMode.OptionsColumn.AllowFocus = false;
                if (editRowTransferSite.Count == 0) return;
                string sbillno = "";
                string sTransferMode = "";
                for (int i = 0; i < editRowTransferSite.Count; i++)
                {
                    sbillno += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "BillNO")) + ",";
                    sTransferMode += ConvertType.ToString(myGridView2.GetRowCellValue(editRowTransferSite[i], "TransferMode")) + ",";
                }
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", sbillno));
                    list.Add(new SqlPara("TransferMode", sTransferMode));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_WAYBILL_TransferMode", list);
                    if (SqlHelper.ExecteNonQuery(sps) == 0) return;

                    MsgBox.ShowOK();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            frmDepartureAddSingle fdas = new frmDepartureAddSingle();
            fdas.DepartureBatch = sDepartureBatch;
            fdas.CarNo = labCarNO.Text;
            fdas.DriverName = labDriverName.Text;
            fdas.DriverPhone = DriverPhone.Text.Trim();
            fdas.BeginSite = BeginSite.Text.Trim();
            fdas.EndSite = EndSite.Text.Trim();
            fdas.ShowDialog();

            freshData();//刷新本车清单
        }

        private void AccCollectPremium_Validated(object sender, EventArgs e)
        {
            //今天之后的才需要设置最低100
            //暂时去掉服务费100 zaj 2017-12-19
            //if (DepartureDate.DateTime > ConvertType.ToDateTime("2016-11-09 00:00"))
            //{
            //    if (ConvertType.ToDecimal(AccCollectPremium.Text) < 100)
            //        AccCollectPremium.Text = "100";
            //}
        }

        private Guid oilId = Guid.Empty;
        //查询油料记录
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            frmOilPlants_Select frm = new frmOilPlants_Select(CarNO.Text);
            frm.ShowDialog();
            if (!string.IsNullOrEmpty(frm.OilGuid))
            {
                oilId = new Guid(frm.OilGuid);
                OilFee.EditValue = frm.OilFee;
                oilVolume.EditValue = frm.OilVolume;
                oilPrice.EditValue = frm.OilPrice;
            }
        }
        //hj20180416
        public string ResponseSite { get; set; }
        public string ResponseWeb { get; set; }
        private void EndSite_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            frmDepartureEdit frmEdit = new frmDepartureEdit();
            frmEdit.RequestSite = EndSite.Text.Trim();
            frmEdit.RequestWeb = EndWeb.Text.Trim();
            frmEdit.Owner = this;
            //List<SqlPara> list1 = new List<SqlPara>();//毛慧20171030--运输合同审核过后，不能再做除目的网点以外的修改。
            //list1.Add(new SqlPara("DepartureBatch", DepartureBatch1));
            //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_aduitStateStr", list1);
            //DataSet ds = SqlHelper.GetDataSet(spe);
            //string a = ds.Tables[0].Rows[0]["aduitStateStr"].ToString();
            //frmEdit.state = a;
            frmEdit.ShowDialog();
            if (frmEdit.IsModify)
            {
                this.EndSite.Text = ResponseSite;
                this.EndWeb.Text = ResponseWeb;
            }
        }

        private void EndSite_EditValueChanged_1(object sender, EventArgs e)
        {
            string endSite = EndSite.Text.Trim();
            if (endSite == "") return;
            string[] tmps = endSite.Split(',');
            if (tmps == null || tmps.Length == 0) return;

            edesite1.Text = edesite2.Text = edesite3.Text = "";
            try
            {
                edesite1.Text = tmps[0].Trim();
                edesite2.Text = tmps[1].Trim();
                edesite3.Text = tmps[2].Trim();
            }
            catch { }
            edesiteacc1.Text = edesite1.Text == "" ? "" : edesiteacc1.Text;
            edesiteacc2.Text = edesite2.Text == "" ? "" : edesiteacc2.Text;
            edesiteacc3.Text = edesite3.Text == "" ? "" : edesiteacc3.Text;

        }

        private void edrepremark_EditValueChanged(object sender, EventArgs e)
        {

        }


    }
}