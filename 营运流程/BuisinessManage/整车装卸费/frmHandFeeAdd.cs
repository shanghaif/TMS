using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmHandFeeAdd : BaseForm
    {

        public frmHandFeeAdd()
        {
            InitializeComponent();
        }
        public string sOtherState = "始发";
        public string sDepartureBatch = "";
        public string sid = "";

        private void frmOtherFeeAdd_Load(object sender, EventArgs e)
        {
            //if (!commonclass.reptitle.Contains("明亮"))
            //{
            //    panel1.Visible = false;
            //}
            CommonClass.FormSet(this);
            //string[] CustomTypeModeList = CommonClass.Arg.ProjectOFHost.Split(',');
            //if (CustomTypeModeList.Length > 0)
            //{
            //    for (int i = 0; i < CustomTypeModeList.Length; i++)
            //    {
            //        Project.Properties.Items.Add(CustomTypeModeList[i]);
            //    }
            //    Project.SelectedIndex = 0;
            //}
            if (sOtherState == "始发")
            {
                this.Text = "始发整车装卸费登记";
            }
            else 
            {
                this.Text = "终端整车装卸费登记";
            }
            btnSave.Enabled = false;
            if (sDepartureBatch != "")
            {
                serchBillNo.Text = sDepartureBatch;
                serchBillNo.Enabled = false;
                btnSearch.Enabled = false;
                getDepartureInfo();
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (serchBillNo.Text.Trim() == "") return;

            if (!getunit()) return;
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Amount.Text.Trim() == "" || Amount.Text.Trim() == "0") return;
            try
            {
                if (sOtherState != "始发")
                {
                    if (CommonClass.QSP_LOCK_2(DepartureBatch.Text.Trim(), DepartureDate.Text.Trim()))
                    {
                        return;
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("OID", Guid.NewGuid()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("Type", sOtherState));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CreateTime", CommonClass.gcdate));
                list.Add(new SqlPara("CreatelMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("HandFee", Amount.Text.Trim())); 
                list.Add(new SqlPara("ReMark", ReMark.Text.Trim()));
                if (Discharger.Text.Trim() == "" || DischargerPhone.Text.Trim() == "")
                {
                    MsgBox.ShowOK("装货人姓名和电话必须填写!");
                    return;
                }
                list.Add(new SqlPara("HandMan", Discharger.Text.Trim()));
                list.Add(new SqlPara("HandManPhone", DischargerPhone.Text.Trim()));
                if (btnSearch.Enabled)
                    list.Add(new SqlPara("isMod", 0));
                else
                    list.Add(new SqlPara("isMod", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLHANDFEEBYCAR", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(TextEdit))
                {
                    item.Text = "";
                }
            }
        }

        private bool getunit()
        {
            return false;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void edunit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getDepartureInfo();
            }
        }
         string sDepartureBatchAdd = "";
        private void getDepartureInfo()
        {
            sDepartureBatchAdd = serchBillNo.Text.Trim();
            if (sDepartureBatchAdd == "") 
            {
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", sDepartureBatchAdd));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH_FOR_HANDFEE", list));
                if (ds == null || ds.Tables.Count == 0) return;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];//取第0行
                    ContractNO.EditValue = dr["ContractNO"];
                    DepartureBatch.EditValue = dr["DepartureBatch"];
                    CarNO.EditValue = dr["CarNO"];
                    CarrNO.EditValue = dr["CarrNO"];
                    //DriverName.EditValue = dr["DriverName"]; labDriverName.Text = DriverName.Text;
                    DriverPhone.EditValue = dr["DriverPhone"];
                    LoadWeight.EditValue = dr["LoadWeight"];
                    LoadVolume.EditValue = dr["LoadVolume"];
                    BeginSite.EditValue = dr["BeginSite"];
                    EndSite.EditValue = dr["EndSite"];
                    ActWeight.EditValue = dr["ActWeight"];
                    ActVolume.EditValue = dr["ActVolume"];
                    DepartureDate.EditValue = dr["DepartureDate"];
                    ExpArriveDate.EditValue = dr["ExpArriveDate"];
                    //LoadPeoples.EditValue = dr["LoadPeoples"]; labLoadPeoples.Text = LoadPeoples.Text;
                    //Creator.EditValue = dr["Creator"]; lbcreateby.Text = Creator.Text;
                    LoadingType.EditValue = dr["LoadingType"];
                    //BoxNO.EditValue = dr["BoxNO"];
                    //BigCarDescr.EditValue = dr["BigCarDescr"];
                    //AccLine.EditValue = dr["AccLine"];
                    //FecthSite.EditValue = dr["FecthSite"];
                    //AccBigCarFecth.EditValue = dr["AccBigCarFecth"];
                    //FecthBillNO.EditValue = dr["FecthBillNO"];
                    //AccBigCarSend.EditValue = dr["AccBigCarSend"];
                    //SendBillNO.EditValue = dr["SendBillNO"];
                    //SendAddr.EditValue = dr["SendAddr"];
                    //AccShtBarge.EditValue = dr["AccShtBarge"];
                    //ShtBargeDept.EditValue = dr["ShtBargeDept"];
                    //AccSomeLoad.EditValue = dr["AccSomeLoad"];
                    //UnloadAddr.EditValue = dr["UnloadAddr"];
                    //AccBigcarOther.EditValue = dr["AccBigcarOther"];
                    //TakeDept.EditValue = dr["TakeDept"];
                    //DriverTakePay.EditValue = dr["DriverTakePay"];
                    //AccTakeCar.EditValue = dr["AccTakeCar"];
                    //AccCollectPremium.EditValue = dr["AccCollectPremium"];
                    //NowPayDriver.EditValue = dr["NowPayDriver"];
                    //ToPayDriver.EditValue = dr["ToPayDriver"];
                    //BackPayDriver.EditValue = dr["BackPayDriver"];
                    //AccBigcarTotal.EditValue = dr["AccBigcarTotal"];//大车费合计
                    //DriverTakePay.EditValue = dr["DriverTakePay"];
                    //AccTakeCar.EditValue = dr["AccTakeCar"];
                    //AccCollectPremium.EditValue = dr["AccCollectPremium"];
                    //NowPayDriver.EditValue = dr["NowPayDriver"];
                    //BackPayDriver.EditValue = dr["BackPayDriver"];
                    //edrepremark.EditValue = dr["BigCarDescr"];

                    //try
                    //{
                    //    int isover = ConvertType.ToInt32(dr["isover"]);
                    //    SetButtonState(isover == 0);
                    //}
                    //catch { }
                }
                //取到付驾驶员
                //if (ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0) return;
                //try
                //{
                //    edesite1.EditValue = ds.Tables[1].Rows[0][0];
                //    edesiteacc1.EditValue = ds.Tables[1].Rows[0][1];
                //    edesite2.EditValue = ds.Tables[1].Rows[1][0];
                //    edesiteacc2.EditValue = ds.Tables[1].Rows[1][1];
                //    edesite3.EditValue = ds.Tables[1].Rows[2][0];
                //    edesiteacc3.EditValue = ds.Tables[1].Rows[2][1];
                //}
                //catch { }
                if (DepartureBatch.Text.Trim() != "")
                {
                    btnSave.Enabled = true;
                }
                if (sDepartureBatch != "")
                {
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("id", sid));

                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLHANDFEEBYID", list1);
                    DataSet ds1 = SqlHelper.GetDataSet(sps1);
                    Amount.Text = ds1.Tables[0].Rows[0]["HandFee"].ToString();
                    ReMark.Text = ds1.Tables[0].Rows[0]["ReMark"].ToString();
                    Discharger.Text = ds1.Tables[0].Rows[0]["HandMan"].ToString();
                    DischargerPhone.Text = ds1.Tables[0].Rows[0]["HandManPhone"].ToString();

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message.Substring(ex.Message.Length - 13, 13));
            }
        }


        private void simpleButton4_Click(object sender, EventArgs e)
        {
            getDepartureInfo();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edacczx_c_EditValueChanged(object sender, EventArgs e)
        {
            //decimal acczx = Convert.ToDecimal(Amount.Text.Trim() == "" ? "0" : Amount.Text.Trim());
            //if (acczx > 0)
            //{
            //    Discharger.Enabled = true;
            //    DischargerPhone.Enabled = true;
            //}
            //else
            //{
            //    Discharger.Enabled = false;
            //    DischargerPhone.Enabled = false;
            //    Discharger.Text = "";
            //    DischargerPhone.Text = "";
            //}
        }


    }
}