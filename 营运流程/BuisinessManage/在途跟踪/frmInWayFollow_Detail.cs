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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmInWayFollow_Detail : BaseForm
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public string sDepartureBatch;
        GridColumn gcIsseleckedMode;
        public frmInWayFollow_Detail()
        {
            InitializeComponent();
        }
        private void frmInWayFollow_Detail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1); 
            GetWindowData();
            FollowDate.DateTime = CommonClass.gcdate;
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }

        /// <summary>
        /// 获取窗体数据
        /// </summary>
        private void GetWindowData()
        {
            if (string.IsNullOrEmpty(sDepartureBatch)) return;

            freshData();
            getContractByDBatch();
        }

        private void getData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_sDepartureBatch", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                //dataset2 = ds;
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void getContractByDBatch()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                DataRow dr = ds.Tables[0].Rows[0];
                lbht.Text = dr["ContractNO"] + "";
                lbvehicle.Text = dr["CarNO"] + "";
                lbchauffer.Text = dr["DriverName"] + "";
                lbinonevehicleflag.Text = dr["DepartureBatch"] + "";
                lbbilldate.Text = dr["DepartureDate"] + "";
                lbmadeby.Text = dr["LoadPeoples"] + "";
                lbcreateby.Text = dr["Creator"] + "";
                ContractNO.EditValue = dr["ContractNO"];
                DepartureBatch.EditValue = dr["DepartureBatch"];
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
                BackPayDriver.EditValue = dr["BackPayDriver"];
                AccBigcarTotal.EditValue = dr["AccBigcarTotal"];//大车费合计
                DriverTakePay.EditValue = dr["DriverTakePay"];
                AccTakeCar.EditValue = dr["AccTakeCar"];
                AccCollectPremium.EditValue = dr["AccCollectPremium"];
                NowPayDriver.EditValue = dr["NowPayDriver"];
                BackPayDriver.EditValue = dr["BackPayDriver"];
                AccLine.EditValue = dr["AccLine"];
                DriverTakePay.EditValue = dr["DriverTakePay"];
                BigCarDescr.EditValue = dr["BigCarDescr"];
                getDepartureList();
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
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void getDepartureList()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURELIST_BY_DEPARTUREBATCH", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTUREFOLLOW");
                sps.ParaList = list;

                DataSet ds = SqlHelper.GetDataSet(sps);


                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (FollowContent.Text.Trim() == "") return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebFollowNO", Guid.NewGuid()));
                list.Add(new SqlPara("DepartureBatch", sDepartureBatch));
                list.Add(new SqlPara("FollowContent", FollowContent.Text.Trim()));
                list.Add(new SqlPara("FollowDate", FollowDate.DateTime.ToString()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTUREFOLLOW");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    FollowContent.Text = "";
                    freshData();
                    CommonSyn.TraceSyn(sDepartureBatch, null, 6, "大车跟踪", 1, "配载", null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView2.GetRowCellValue(rowhandle, "WebFollowNO").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebFollowNO", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDEPARTUREFOLLOW");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView2.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            FollowContent.Text = "";
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnSendSms_Click(object sender, EventArgs e)
        {
            string content = edcontent.Text.ToString();
            string people = edpeople.Text;
            if (content == "")
            {
                MsgBox.ShowError("请填写跟踪内容!");
                return;
            }
            sms.errorsendsms(myGridView1, this, "1", content, people);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "大车跟踪明细");
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            string vno = lbvehicle.Text.Trim();
            if (vno == "")
            {
                MsgBox.ShowOK("没有车牌号，无法定位!");
                return;
            }
            List<VehicleModel> list = E6GPS.GetVehiclePosition(vno);
            if (list == null || list.Count == 0) return;

            FollowContent.Text = list[0].Provice + list[0].City + list[0].District + list[0].RoadName;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string vno = lbvehicle.Text.Trim();
            if (vno == "")
            {
                MsgBox.ShowOK("没有取车车牌号，无法定位!");
                return;
            }
            E6GPS.GetVehiclePositionMap(vno);
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            int a = checkEdit1.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
    }
}