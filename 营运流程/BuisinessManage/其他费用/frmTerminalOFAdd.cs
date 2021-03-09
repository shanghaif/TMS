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
    public partial class frmTerminalOFAdd : BaseForm
    {

        public frmTerminalOFAdd()
        {
            InitializeComponent();
        }
        string sOtherState = "终端";
        public string sBillno = "";
        public string sOID = "";

        private void frmOtherFeeAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            string[] CustomTypeModeList = CommonClass.Arg.ProjectOend.Split(',');
            if (CustomTypeModeList.Length > 0)
            {
                for (int i = 0; i < CustomTypeModeList.Length; i++)
                {
                    Project.Properties.Items.Add(CustomTypeModeList[i]);
                }
                Project.SelectedIndex = 0;
            }
            btnSave.Enabled = false;
            if (sBillno != "")
            {
                serchBillNo.Text = sBillno;
                serchBillNo.Enabled = false;
                btnSearch.Enabled = false;
                getdata();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmBillSearch.ShowBillSearch(BillNo.Text.Trim());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (Amount.Text.Trim() == "" || Amount.Text.Trim() == "0") return;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OID", Guid.NewGuid()));
                list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
                list.Add(new SqlPara("OtherState", sOtherState));
                list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("SignDate", CommonClass.gcdate));
                list.Add(new SqlPara("SignMan", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("Project", Project.Text.Trim()));
                list.Add(new SqlPara("Amount", Amount.Text.Trim()));
                list.Add(new SqlPara("TSendBatch", textEdit1.Text.Trim()));
                if (Project.Text.Trim() == "终端装卸费")
                {
                    if (Discharger.Text.Trim() == "" || DischargerPhone.Text.Trim() == "")
                    {
                        MsgBox.ShowOK("必须填写装卸人和装卸电话!");
                        return;
                    }
                    if (HandNum.Text.Trim() == "" && CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
                    {
                        MsgBox.ShowOK("卸货件数必须填写!");
                        return;
                    }
                }
                list.Add(new SqlPara("Discharger", Discharger.Text.Trim()));
                list.Add(new SqlPara("DischargerPhone", DischargerPhone.Text.Trim()));
                list.Add(new SqlPara("ReMark", ReMark.Text.Trim()));
                list.Add(new SqlPara("HandNum", HandNum.Text.Trim()));
                if (btnSearch.Enabled)
                    list.Add(new SqlPara("isMod", 0));
                else
                    list.Add(new SqlPara("isMod", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLOTHERFEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    serchBillNo.Text = "";
                    BillNo.Text = "";
                    CusOderNo.Text = "";
                    StartSite.Text = "";
                    DestinationSite.Text = "";
                    TransferSite.Text = "";
                    BillDate.Text = "";
                    ConsigneeName.Text = "";
                    ConsignorName.Text = "";
                    PaymentMode.Text = "";
                    TransferMode.Text = "";
                    StorageFee.Text = "";
                    HandleFee.Text = "";
                    ForkliftFee.Text = "";
                    OtherFee.Text = "";
                    BillMan.Text = "";
                    BegWeb.Text = "";
                    LeftNum.EditValue = 0;
                    CustomsFee.Text = UpstairFee.Text = PackagFee.Text = "";
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message.Substring(ex.Message.Length - 13, 13));
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
                getdata();
            }
        }

        private void getdata()
        {
            //根据ID获取数据
            if (serchBillNo.Text.Trim() != "")
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    List<SqlPara> list2 = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", serchBillNo.Text.Trim()));
                    list2.Add(new SqlPara("BillNo", serchBillNo.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILLFOROTHERFEE", list);
                    SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_SENDBATCH", list2);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    DataSet ds2 = SqlHelper.GetDataSet(sps2);
                    if (ds2 != null && ds2.Tables.Count > 0) 
                    {
                        textEdit1.Properties.Items.Clear();
                        for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                        {
                            textEdit1.Properties.Items.Add(ds2.Tables[0].Rows[i][0]);
                        }
                        if (textEdit1.Properties.Items.Count > 0)
                            textEdit1.SelectedIndex = 0;
                    }

                    if (ds == null || ds.Tables.Count<0 ||ds.Tables[0].Rows.Count == 0)
                    {
                        BillNo.Text = "";
                        CusOderNo.Text = "";
                        StartSite.Text = "";
                        DestinationSite.Text = "";
                        TransferSite.Text = "";
                        BillDate.Text = "";
                        ConsigneeName.Text = "";
                        ConsignorName.Text = "";
                        PaymentMode.Text = "";
                        TransferMode.Text = "";
                        StorageFee.Text = "";
                        HandleFee.Text = "";
                        ForkliftFee.Text = "";
                        OtherFee.Text = "";
                        BillMan.Text = "";
                        BegWeb.Text = "";
                        LeftNum.EditValue = 0;
                        CustomsFee.Text = UpstairFee.Text = PackagFee.Text = "";
                        btnSave.Enabled = false;
                        return;
                    }
                    //展示数据
                    BillNo.Text = ds.Tables[0].Rows[0]["BillNo"].ToString();
                    CusOderNo.Text = ds.Tables[0].Rows[0]["CusOderNo"].ToString();
                    StartSite.Text = ds.Tables[0].Rows[0]["StartSite"].ToString();
                    DestinationSite.Text = ds.Tables[0].Rows[0]["DestinationSite"].ToString();
                    TransferSite.Text = ds.Tables[0].Rows[0]["TransferSite"].ToString();
                    BillDate.Text = ds.Tables[0].Rows[0]["BillDate"].ToString();
                    ConsigneeName.Text = ds.Tables[0].Rows[0]["ConsigneeName"].ToString();
                    ConsignorName.Text = ds.Tables[0].Rows[0]["ConsignorName"].ToString();
                    PaymentMode.Text = ds.Tables[0].Rows[0]["PaymentMode"].ToString();
                    TransferMode.Text = ds.Tables[0].Rows[0]["TransferMode"].ToString();
                    StorageFee.Text = ds.Tables[0].Rows[0]["StorageFee"].ToString();
                    HandleFee.Text = ds.Tables[0].Rows[0]["HandleFee"].ToString();
                    ForkliftFee.Text = ds.Tables[0].Rows[0]["ForkliftFee"].ToString();
                    OtherFee.Text = ds.Tables[0].Rows[0]["OtherFee"].ToString();
                    BillMan.Text = ds.Tables[0].Rows[0]["BillMan"].ToString();
                    BegWeb.Text = ds.Tables[0].Rows[0]["BegWeb"].ToString();
                    CustomsFee.EditValue = ds.Tables[0].Rows[0]["CustomsFee"];
                    UpstairFee.EditValue = ds.Tables[0].Rows[0]["UpstairFee"];
                    PackagFee.EditValue = ds.Tables[0].Rows[0]["PackagFee"];
                    LeftNum.EditValue = ds.Tables[0].Rows[0]["LeftNum"]; ;

                    if (BillNo.Text.Trim() != "")
                        btnSave.Enabled = true;
                    if (sBillno != "")
                    {
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("OID", sOID));

                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "QSP_GET_BILLOTHERFEE_ByID", list1);
                        DataSet ds1 = SqlHelper.GetDataSet(sps1);
                        Project.Text = ds1.Tables[0].Rows[0]["Project"].ToString();
                        Project.Enabled = false;
                        Amount.Text = ds1.Tables[0].Rows[0]["Amount"].ToString();
                        ReMark.Text = ds1.Tables[0].Rows[0]["ReMark"].ToString();
                        Discharger.Text = ds1.Tables[0].Rows[0]["Discharger"].ToString();
                        DischargerPhone.Text = ds1.Tables[0].Rows[0]["DischargerPhone"].ToString();
                        HandNum.Text = ds1.Tables[0].Rows[0]["HandNum"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void edacczx_c_EditValueChanged(object sender, EventArgs e)
        {
            decimal acczx = Convert.ToDecimal(Amount.Text.Trim() == "" ? "0" : Amount.Text.Trim());
            if (acczx > 0)
            {
                Discharger.Enabled = true;
                DischargerPhone.Enabled = true;
            }
            else
            {
                Discharger.Enabled = false;
                DischargerPhone.Enabled = false;
                Discharger.Text = "";
                DischargerPhone.Text = "";
            }
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            //获取送货件数
            if (textEdit1.Text.Trim() == "" || serchBillNo.Text.Trim() == "")
            {
                MsgBox.ShowOK("单号或者批次号为空！请输入！");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", serchBillNo.Text.Trim()));
            list.Add(new SqlPara("BatchNo", textEdit1.Text.Trim()));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDUNLOADNUM_BATCHNO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds != null && ds.Tables[0].Rows.Count > 0) 
            {
                HandNum.Enabled = true;
                HandNum.EditValue = ConvertType.ToString(ds.Tables[0].Rows[0][0]);
                HandNum.Enabled = false;
            }
        }
    }
}