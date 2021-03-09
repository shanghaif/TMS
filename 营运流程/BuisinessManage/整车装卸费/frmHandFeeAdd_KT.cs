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
    public partial class frmHandFeeAdd_KT : BaseForm
    {

        public frmHandFeeAdd_KT()
        {
            InitializeComponent();
        }
        //public string sOtherState = "始发";
        public string sDepartureBatch = "";
        public string sid = "";
        public string sFeeType = "", sDischarger = "",sExceDepart="", sDischargerPhone = "", sAmount = "", sReMark="";
        public int AddOrUpd = 0;

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
            //if (sOtherState == "始发")
            //{
            //    this.Text = "始发整车装卸费登记";
            //}
            //else 
            //{
            //    this.Text = "终端整车装卸费登记";
            //}
            btnSave.Enabled = false;
            if (sDepartureBatch != "")
            {
                serchBillNo.Text = sDepartureBatch;
                serchBillNo.Enabled = false;
                btnSearch.Enabled = false;
                getDepartureInfo();
                FeeType.Enabled = false;
                FeeType.Text = sFeeType;
                ExceDepart.Text = sExceDepart;
                Discharger.Text = sDischarger;
                Amount.Text = sAmount;
                DischargerPhone.Text = sDischargerPhone;
                ReMark.Text = sReMark;
            }
            FeeType.Text = sFeeType;
            ExceDepart.Text = CommonClass.UserInfo.WebName;
            GetAllWebId();

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (serchBillNo.Text.Trim() == "") return;

            if (!getunit()) return;
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Amount.Text.Trim() == "" || Amount.Text.Trim() == "0") return;
            if (FeeType.Text == "")           //whf20190805 begin 
            {
                MsgBox.ShowOK("费用类型不能为空！");
                return;
            }

            string strFeeType = FeeType.Text;
            string no = serchBillNo.Text;
            no = no.Substring(0, 2);

            List<string> startstring = new List<string>() { "LE", "LP", "LS" };

            if (!startstring.Contains(no))
            {
                MsgBox.ShowOK("改批次号有误，请重新核实批次号！");
                return;
            }
            if (no == "LE")
            {
                if (strFeeType != "装卸费-短驳出库" && strFeeType != "装卸费-短驳到货")
                {
                    MsgBox.ShowOK("该批次属于短驳批次，费用类型只能选择‘装卸费-短驳出库’或者‘装卸费-短驳到货’!");
                    return;
                }

            }

            if (no == "LP")
            {
                if (strFeeType != "装卸费-大车" && strFeeType != "装卸费-大车到货")
                {
                    MsgBox.ShowOK("该批次属于配载批次，费用类型只能选择‘装卸费-大车’或者‘装卸费-大车到货’!");
                    return;
                }
            }

            if (no == "LS")
            {
                if (strFeeType != "装卸费-二级到货" && strFeeType != "装卸费-转二级")
                {
                    MsgBox.ShowOK("该批次属于送货批次，费用类型只能选择‘装卸费-二级到货’或者‘装卸费-转二级’!");
                    return;
                }
            }                       //whf20190805 end 
            try
            {
                //if (sOtherState != "始发")
                //{
                //    if (CommonClass.QSP_LOCK_2(DepartureBatch.Text.Trim(), DepartureDate.Text.Trim()))
                //    {
                //        return;
                //    }
                //}
                List<SqlPara> list = new List<SqlPara>();
                //list.Add(new SqlPara("OID", Guid.NewGuid()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                //list.Add(new SqlPara("Type", sOtherState));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("CreateTime", CommonClass.gcdate));
                list.Add(new SqlPara("CreatelMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("HandFee", Amount.Text.Trim())); 
                list.Add(new SqlPara("ReMark", ReMark.Text.Trim()));
                list.Add(new SqlPara("ExceDepart", ExceDepart.Text.Trim()));
                if (Discharger.Text.Trim() == "" || DischargerPhone.Text.Trim() == "")
                {
                    MsgBox.ShowOK("装货人姓名和电话必须填写!");
                    return;
                }
                list.Add(new SqlPara("HandMan", Discharger.Text.Trim()));
                list.Add(new SqlPara("HandManPhone", DischargerPhone.Text.Trim()));
                list.Add(new SqlPara("FeeType ", FeeType.Text.Trim()));
                list.Add(new SqlPara("AddOrUpd", AddOrUpd));//1代表修改
                //if (btnSearch.Enabled)
                //    list.Add(new SqlPara("isMod", 0));
                //else
                //    list.Add(new SqlPara("isMod", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLHANDFEEBYCAR_KT", list);
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
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DEPARTURE_BY_BATCH_FOR_HANDFEE_KT", list));
                if (ds == null || ds.Tables.Count == 0) return;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];//取第0行
                    //ContractNO.EditValue = dr["ContractNO"];
                    DepartureBatch.EditValue = dr["DepartureBatch"];
                    CarNO.EditValue = dr["CarNO"];
                    CarrNO.EditValue = dr["CarrNO"];
                    DriverName.EditValue = dr["DriverName"]; 
                    DriverPhone.EditValue = dr["DriverPhone"];
                    //LoadWeight.EditValue = dr["LoadWeight"];
                    //LoadVolume.EditValue = dr["LoadVolume"];
                    //BeginSite.EditValue = dr["BeginSite"];
                    //EndSite.EditValue = dr["EndSite"];
                    ActWeight.EditValue = dr["ActWeight"];
                    ActVolume.EditValue = dr["ActVolume"];

                if (DepartureBatch.Text.Trim() != "")
                {
                    btnSave.Enabled = true;
                }
                //if (sDepartureBatch != "")
                //{
                //    List<SqlPara> list1 = new List<SqlPara>();
                //    list1.Add(new SqlPara("id", sid));

                //    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLHANDFEEBYID", list1);
                //    DataSet ds1 = SqlHelper.GetDataSet(sps1);
                //    Amount.Text = ds1.Tables[0].Rows[0]["HandFee"].ToString();
                //    ReMark.Text = ds1.Tables[0].Rows[0]["ReMark"].ToString();
                //    Discharger.Text = ds1.Tables[0].Rows[0]["HandMan"].ToString();
                //    DischargerPhone.Text = ds1.Tables[0].Rows[0]["HandManPhone"].ToString();

                //}
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


        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                ExceDepart.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    ExceDepart.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}