using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmArrivalConfirmSingle : ZQTMS.Tool.BaseForm
    {
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

        public frmArrivalConfirmSingle()
        {
            InitializeComponent();
        }

        private void frmArrivalConfirmSingle_Load(object sender, EventArgs e)
        {
            lblDepartureBatch.Text = "发车批次：" + departureBatch;
            lblCarNo.Text = "车号：" + carNo;
            lblDriverName.Text = "司机：" + driverName;
            lblDriverPhone.Text = "司机电话：" + driverPhone;
            lblBeginSite.Text = "启运站：" + beginSite;
            lblEndSite.Text = "目的地：" + endSite;
            txtBillNo.Text = "";
            txtBillNo.Focus();
            txtBillNo.SelectAll();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
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

            string addReason = MeAddReason.Text.Trim();
            if (addReason == "")
            {
                MsgBox.ShowOK("请填写强装原因!");
                MeAddReason.Focus();
                return;
            }
            string num = txtnum.Text.Trim();
            bool isnum = Tool.StringHelper.IsNumberId(num);
            if (isnum == false)
            {
                MsgBox.ShowOK("请填写正确件数!");
                txtnum.Focus();
                return;
            }

            int inum = 0;
            int.TryParse(num, out inum);

            if (inum <= 0)
            {
                MsgBox.ShowOK("件数必须大于0或者是整数!");
                txtnum.Focus();
                return;
            }
            try
            {
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_QZDEPARTURE_SINGLE", new List<SqlPara> { new SqlPara("DepartureBatch", departureBatch), new SqlPara("BillNo", BillNo), new SqlPara("AddReason", addReason), new SqlPara("DepartureNum", num) })) == 0) return;
                txtBillNo.Text = "";
                txtnum.Text = "";
                MeAddReason.Text = "";
                MsgBox.ShowOK("加入成功!");

                //同步配载修改时效 LD 2018-4-27
                List<SqlPara> list1 = new List<SqlPara>();
                list1.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list1);
                DataSet ds_ture = SqlHelper.GetDataSet(sps);
                if (ds_ture != null && ds_ture.Tables.Count > 0 && ds_ture.Tables[0].Rows.Count > 0)
                {
                    CommonSyn.TimeDepartUptSyn((BillNo + "@"), departureBatch, ds_ture.Tables[0].Rows[0]["DepartureDate"].ToString(), ds_ture.Tables[0].Rows[0]["LoadWeb"].ToString(), CommonClass.UserInfo.WebName, "USP_ADD_QZDEPARTURE_SINGLE");
                }
                if (departureBatch.Substring(0, 2) == "KP")
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("DepartureBatch", departureBatch));
                    list.Add(new SqlPara("BillNo", BillNo));
                    list.Add(new SqlPara("AddReason", addReason));
                    list.Add(new SqlPara("DepartureNum", num));
                    CommonSyn.LMSSynZQTMS(list, "单票强制点到", "USP_ADD_QZDEPARTURE_SINGLE_ZQTMSSynLMS");
                }
                CommonSyn.TraceSyn(null, BillNo + "@", 7, "货物到达", 1, null, null);
                this.Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }


        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
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
    }
}
