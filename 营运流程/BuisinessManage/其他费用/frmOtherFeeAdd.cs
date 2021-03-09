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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmOtherFeeAdd : BaseForm
    {

        public frmOtherFeeAdd()
        {
            InitializeComponent();
        }
        string sOtherState = "始发";
        public string sBillno = "";
        public string sOID = "";

        private void frmOtherFeeAdd_Load(object sender, EventArgs e)
        {
            //if (!commonclass.reptitle.Contains("明亮"))
            //{
            //    panel1.Visible = false;
            //}
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.FormSet(this);        
                //hj20180702
            CarNo.Enabled = false;
            DriverName.Enabled = false;
            DriverPhone.Enabled = false;

            //if (myGridView1.GetRowCellValue("") == "始发接货费")
            //{
            //    CarNo.Enabled = true;
            //    DriverName.Enabled = true;
            //    DriverPhone.Enabled = true;
            //}
            

            string[] CustomTypeModeList = CommonClass.Arg.ProjectOFHost.Split(',');
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

            try
            {
                //CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");
                myGridControl1.DataSource = CommonClass.dsCar.Tables[0];
            }
            catch { }
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
                if (CommonClass.QSP_LOCK_1(BillNo.Text.Trim(), BillDate.Text.Trim()))
                {
                    return;
                }
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
                list.Add(new SqlPara("Discharger", ""));
                list.Add(new SqlPara("DischargerPhone", ""));
                list.Add(new SqlPara("ReMark", ReMark.Text.Trim()));
                list.Add(new SqlPara("HandNum", HandNum.Text.Trim()));
                list.Add(new SqlPara("CarNo", CarNo.Text.Trim())); //HJ20180702
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));//HJ20180702
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));//HJ20180702
                if (btnSearch.Enabled)
                    list.Add(new SqlPara("isMod", 0));
                else
                    list.Add(new SqlPara("isMod", 1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLOTHERFEE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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
                    ReceivFee.Text = "";
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    list.Add(new SqlPara("BillNo", serchBillNo.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILLFOROTHERFEE", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count <0|| ds.Tables[0].Rows.Count == 0)
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
                        ReceivFee.Text = "";
                        HandleFee.Text = "";
                        ForkliftFee.Text = "";
                        OtherFee.Text = "";
                        BillMan.Text = "";
                        BegWeb.Text = "";
                        CustomsFee.Text = UpstairFee.Text = PackagFee.Text = "";
                        LeftNum.EditValue = 0;
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
                    ConsigneeName.Text = ds.Tables[0].Rows[0]["ConsignorName"].ToString();   //hz20180730
                    ConsignorName.Text = ds.Tables[0].Rows[0]["ConsigneeName"].ToString();   //hz20180730
                    PaymentMode.Text = ds.Tables[0].Rows[0]["PaymentMode"].ToString();
                    TransferMode.Text = ds.Tables[0].Rows[0]["TransferMode"].ToString();
                    ReceivFee.Text = ds.Tables[0].Rows[0]["ReceivFee"].ToString();
                    HandleFee.Text = ds.Tables[0].Rows[0]["HandleFee"].ToString();
                    ForkliftFee.Text = ds.Tables[0].Rows[0]["ForkliftFee"].ToString();
                    OtherFee.Text = ds.Tables[0].Rows[0]["OtherFee"].ToString();
                    BillMan.Text = ds.Tables[0].Rows[0]["BillMan"].ToString();
                    BegWeb.Text = ds.Tables[0].Rows[0]["BegWeb"].ToString();
                    CustomsFee.EditValue = ds.Tables[0].Rows[0]["CustomsFee"];
                    UpstairFee.EditValue = ds.Tables[0].Rows[0]["UpstairFee"];
                    PackagFee.EditValue = ds.Tables[0].Rows[0]["PackagFee"];
                    LeftNum.EditValue = ds.Tables[0].Rows[0]["LeftNum"];

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
                        HandNum.Text = ds1.Tables[0].Rows[0]["HandNum"].ToString();
                        CarNo.Text = ds1.Tables[0].Rows[0]["CarNo"].ToString();
                        DriverName.Text = ds1.Tables[0].Rows[0]["DriverName"].ToString();
                        DriverPhone.Text = ds1.Tables[0].Rows[0]["DriverPhone"].ToString();

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

        private void CarNo_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string value = e.NewValue.ToString();
            myGridView1.Columns["CarNo"].FilterInfo = new ColumnFilterInfo(
                    "[CarNo] LIKE " + "'%" + value + "%'"
                    + " OR [DriverName] LIKE" + "'%" + value + "%'"
                    + " OR [DriverPhone] LIKE" + "'%" + value + "%'",
                    "");
        }

        private void CarNo_Enter(object sender, EventArgs e)
        {
            //myGridControl1.Left = CarNo.Left;
            ////myGridControl1.Top = CarNo.Top + CarNo.Height + 2;
            //myGridControl1.Top = CarNo.Top + CarNo.Height;
            //myGridControl1.Visible = true;

            myGridControl1.Left = groupControl2.Left +  CarNo.Left;
            myGridControl1.Top = groupControl2.Top +  CarNo.Top + CarNo.Height + 2;
            myGridControl1.Visible = true;
        }

        private void CarNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                myGridControl1.Focus();
            //if (e.KeyCode == Keys.Escape)
            //{
            //    myGridControl1.Visible = false;
            //}

        }

        private void CarNo_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = myGridControl1.Focused;
        }

        private void myGridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetCarInfo();
            }
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            DataRow dr = myGridView1.GetDataRow(rowhandle);
            if (dr == null) return;

            CarNo.EditValue = dr["CarNo"];
            DriverName.EditValue = dr["DriverName"];
            DriverPhone.EditValue = dr["DriverPhone"];
            myGridControl1.Visible = false;
           
        }

        private void myGridControl1_Leave(object sender, EventArgs e)
        {
            myGridControl1.Visible = CarNo.Focused;
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void Project_EditValueChanged(object sender, EventArgs e)
        {
            if (Project.Text.Trim() == "始发接货费")
            {
                CarNo.Enabled = true;
                DriverName.Enabled = true;
                DriverPhone.Enabled = true;
            }
            else
            {
                CarNo.EditValue = "";
                DriverName.EditValue = "";
                DriverPhone.EditValue = "";
                CarNo.Enabled = false;
                DriverName.Enabled = false;
                DriverPhone.Enabled = false;
            }
        }
    }
}