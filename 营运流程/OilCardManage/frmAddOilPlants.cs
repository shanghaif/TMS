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
    public partial class frmAddOilPlants : BaseForm
    {
        public int flag = 0;
        public string carNo = "", gasStation = "", gasType = "", oilCardNo = "", oilDate = "", oilVolume = "", oilPrice = "", sumAccount = "", Mark = "", oilId = "", Batchno="";
        public frmAddOilPlants()
        {
            InitializeComponent();
        }

        public frmAddOilPlants(int flag, string carNo, string gasStation, string gasType, string oilCardNo, string oilDate, string oilVolume, string oilPrice, string sumAccount, string Mark, string oilId, string Batchno)
        {
            this.flag = flag;
            this.carNo = carNo;
            this.gasStation = gasStation;
            this.gasType = gasType;
            this.oilCardNo = oilCardNo;
            this.oilDate = oilDate;
            this.oilVolume = oilVolume;
            this.oilPrice = oilPrice;
            this.sumAccount = sumAccount;
            this.Mark = Mark;
            this.oilId = oilId;
            this.Batchno = Batchno;
        }

        public void init() {
            txtCarNo.Text = carNo;
            txtoildate.Text = oilDate;
            txtoilName.Text = gasStation;
            txtoilCardNo.Text = oilCardNo;
            txtoilVolume.Text = oilVolume;
            txtoilPerPrice.Text = oilPrice;
            txtoilType.Text = gasType;
            txtoilAccount.Text = sumAccount;
            txtMark.Text = Mark;
            textEdit1.Text = Batchno;
        }

        private void frmAddOilPlants_Load(object sender, EventArgs e)
        {
            txtoildate.DateTime = CommonClass.gcdate;
            bindcomboboxedit();
            if (txtoilType.Text == "油卡")
            {
                txtoilCardNo.Enabled = true;
            }
            if (flag==2)
            {
                init();
            }
        }

        //绑定数据到车号
        private void bindcomboboxedit()
        {

            this.txtCarNo.Properties.Items.Clear();
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["vehicleno"]=="")
                    {
                        continue;
                    }
                    this.txtCarNo.Properties.Items.Add(ds.Tables[0].Rows[i]["vehicleno"]);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oilType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtoilType.Text == "油卡")
            {
                txtoilCardNo.Enabled = true;
            }
            else
            {
                txtoilCardNo.Enabled = false;
                txtoilCardNo.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCarNo.Text.Trim()))
            {
                MsgBox.ShowOK("请输入车牌号！");
                return;
            }
            if (string.IsNullOrEmpty(txtoilVolume.Text.Trim()) || Convert.ToDecimal(txtoilVolume.Text.Trim())==0)
            {
                MsgBox.ShowOK("请输入加油量！");
                return;
            }
            if (string.IsNullOrEmpty(txtoilAccount.Text.Trim()) || Convert.ToDecimal(txtoilAccount.Text.Trim()) == 0)
            {
                MsgBox.ShowOK("请输入总金额！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                string _oilId = "";
                if (flag == 1)
                {
                    _oilId = Guid.NewGuid().ToString();
                }
                else
                {
                    _oilId = oilId;
                }
               
                list.Add(new SqlPara("carNo", txtCarNo.Text.Trim()));
                list.Add(new SqlPara("oildate",txtoildate.DateTime));
                list.Add(new SqlPara("oilType", txtoilType.Text.Trim()));
                list.Add(new SqlPara("oilCardNo",txtoilCardNo.Text.Trim()));
                list.Add(new SqlPara("gasStation", txtoilName.Text.Trim()));
                list.Add(new SqlPara("oilVolume", txtoilVolume.Text.Trim()));
                list.Add(new SqlPara("oilPerPrice", txtoilPerPrice.Text.Trim()));
                list.Add(new SqlPara("oilAccount", txtoilAccount.Text.Trim()));
                list.Add(new SqlPara("oilID", _oilId));
                list.Add(new SqlPara("mark",txtMark.Text.Trim()));
                list.Add(new SqlPara("flag",flag));
                list.Add(new SqlPara("Batchno", textEdit1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_OilPlants", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void txtoilPerPrice_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtoilPerPrice.Text.Trim())||Convert.ToDecimal(txtoilPerPrice.Text.Trim())==0)
            {
                txtoilPerPrice.Text = "0";
                return;
            }
            else
            {
                txtoilAccount.Text = (Convert.ToDecimal(txtoilVolume.Text.Trim()) * Convert.ToDecimal(txtoilPerPrice.Text.Trim())).ToString();
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmDeparture_Select frm = new frmDeparture_Select();
            frm.car = txtCarNo.Text;
            frm.flag = 1;
            frm.ShowDialog();
            textEdit1.EditValue = frm.Batchno1;
            txtCarNo.EditValue = frm.CarNO1;
        }

        private void simpleButton2_Click_1(object sender, EventArgs e)
        {
            frmDeparture_Select frm = new frmDeparture_Select();
            frm.car = txtCarNo.Text;
            frm.flag = 2;
            frm.ShowDialog();
            textEdit1.EditValue = frm.Batchno1;
            txtCarNo.EditValue = frm.CarNO1;
            txtoilVolume.EditValue = frm.oilVolume1;
            txtoilPerPrice.EditValue = frm.oilPrice1;
            txtoilAccount.EditValue = frm.OilFee1;
        }

    }
}