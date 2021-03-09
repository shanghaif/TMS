using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBaseArrivalConfirm : BaseForm
    {
        public frmBaseArrivalConfirm()
        {
            InitializeComponent();
        }
        private string departureBatch;

        public string DepartureBatch
        {
            get { return departureBatch; }
            set { departureBatch = value; }
        }
        private string carNo;

        public string CarNo
        {
            get { return carNo; }
            set { carNo = value; }
        }
        private string driverName;

        public string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }
        private string driverPhone;

        public string DriverPhone
        {
            get { return driverPhone; }
            set { driverPhone = value; }
        }
        private string beginSite;

        public string BeginSite
        {
            get { return beginSite; }
            set { beginSite = value; }
        }
        private string endSite;

        public string EndSite
        {
            get { return endSite; }
            set { endSite = value; }
        }

        private void frmBaseArrivalConfirm_Load(object sender, EventArgs e)
        {
            lBatchNo.Text = departureBatch;
            lCarNo.Text = carNo;
            lDriver.Text = driverName;
            lDriverPhone.Text = driverPhone;
            lStartSite.Text = beginSite;
            lEndDestination.Text = endSite;
            CommonClass.SetSite(cbbAgentSite, false);
            txtBillNo.Text = "";
            txtBillNo.Focus();
            txtBillNo.SelectAll();
        }

        private void btnQueryBill_Click(object sender, EventArgs e)
        {
            string BillNo = txtBillNo.Text.Trim();
            if (BillNo == "")
            {
                MsgBox.ShowOK("请输入要查询的运单号!");
                txtBillNo.Focus();
                return;
            }

            frmBillSearch.ShowBillSearch(txtBillNo.Text.Trim());
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cbbAgentSite.Text.Trim()))
            {
                MsgBox.ShowOK("请输入代理站点!");
                cbbAgentSite.Focus();
                return;
            }
            if (string.IsNullOrEmpty(cbbAgentWeb.Text.Trim()))
            {
                MsgBox.ShowOK("请输入代理网点!");
                cbbAgentWeb.Focus();
                return;
            }
            string BillNo = txtBillNo.Text.Trim();
            if (BillNo == "")
            {
                MsgBox.ShowOK("请输入要加入本车的运单号!");
                txtBillNo.Focus();
                return;
            }
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "USP_GET_billDepartureList_ByBillNo", new List<SqlPara>() { new SqlPara("BillNo", BillNo) }));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                frmArrivalInfo frm = new frmArrivalInfo();
                frm.ds = ds;
                frm.ShowDialog();
                if (frm.DialogResult != DialogResult.Yes) return;
            }

            string addReason = txtReason.Text.Trim();
            if (addReason == "")
            {
                MsgBox.ShowOK("请填写强装原因!");
                txtReason.Focus();
                return;
            }
            string num = txtBillNum.Text.Trim();
            bool isnum = Tool.StringHelper.IsNumberId(num);
            if (isnum == false)
            {
                MsgBox.ShowOK("请填写正确件数!");
                txtBillNum.Focus();
                return;
            }

            int inum = 0;
            int.TryParse(num, out inum);

            if (inum <= 0)
            {
                MsgBox.ShowOK("件数必须大于0或者是整数!");
                txtBillNum.Focus();
                return;
            }

            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BaseQZDEPARTURE_SINGLE", new List<SqlPara> { new SqlPara("DepartureBatch", departureBatch), new SqlPara("BillNo", BillNo), new SqlPara("AddReason", addReason), new SqlPara("DepartureNum", num), new SqlPara("AgentSite", cbbAgentSite.Text.Trim()), new SqlPara("AgentWeb", cbbAgentWeb.Text.Trim()) })) == 0) return;
                txtBillNo.Text = "";
                txtBillNum.Text = "";
                txtReason.Text = "";
                MsgBox.ShowOK("加入成功!");

                //同步配载修改时效 LD 2018-4-27
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
                DataSet ds_ture = SqlHelper.GetDataSet(sps);
                if (ds_ture != null && ds_ture.Tables.Count > 0 && ds_ture.Tables[0].Rows.Count > 0)
                {
                    CommonSyn.TimeDepartUptSyn((BillNo + "@"), departureBatch, ds_ture.Tables[0].Rows[0]["DepartureDate"].ToString(), ds_ture.Tables[0].Rows[0]["LoadWeb"].ToString(), CommonClass.UserInfo.WebName, "USP_ADD_BaseQZDEPARTURE_SINGLE");
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbAgentSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(cbbAgentWeb, cbbAgentSite.Text.Trim(), false);
        }
    }
}