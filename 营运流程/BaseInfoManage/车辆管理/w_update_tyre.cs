using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
namespace ZQTMS.UI
{
    public partial class w_update_tyre : BaseForm
    {
        public w_update_tyre()
        {
            InitializeComponent();
        }
        private string number = string.Empty;

        public w_update_tyre(string tyreno)
        {
            InitializeComponent();
            this.number = tyreno;
        }
        private void w_update_tyre_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 
            bindvehicleno();
            bindprovider();
            getdata();
        }
        private void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("tyreno", number));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_TYRE_INFO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.tyreno.Text = dt.Rows[0]["tyreno"].ToString();
            this.vehicleno.Text = dt.Rows[0]["vehicleno"].ToString();
            this.tyreguige.Text = dt.Rows[0]["tyreguige"].ToString();
            this.anzhuangweizhi.Text = dt.Rows[0]["azlocation"].ToString();
            this.buymoney.Text = dt.Rows[0]["buymoney"].ToString();
            this.provider.Text = dt.Rows[0]["gyname"].ToString();
            this.buydate.Text = dt.Rows[0]["buydate"].ToString();
            this.shengchanchangjia.Text = dt.Rows[0]["manufacturer"].ToString();
            this.usedate.EditValue = dt.Rows[0]["usedate"].ToString();
            this.baofeidate.EditValue = dt.Rows[0]["rejectdate"].ToString();
            this.tyrestate.EditValue = dt.Rows[0]["tyrestate"].ToString();
            this.tyrestate.Text = dt.Rows[0]["tyrestate"].ToString() == "0" ? "正常" : "报废";
            this.remark.Text = dt.Rows[0]["remark"].ToString();
            this.updateman.Text = CommonClass.UserInfo.UserName;
        }



        private bool check()
        {
            if (this.tyreno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入轮胎编号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.vehicleno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择车号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.buymoney.Text.Trim() != "" && !StringHelper.IsDecimal(this.buymoney.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的购买金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.provider.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择供应商", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.tyreno.Text.Trim()!=number && this.checkunit(this.tyreno.Text.Trim()))
            {
                XtraMessageBox.Show("该轮胎编号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool checkunit(string tyreno)
        {

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("tyreno", tyreno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_TYRE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds!= null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }


        private void bindvehicleno()
        {
            this.vehicleno.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt.Rows[i]["vehicleno"]);
            }
        }
        private void bindprovider()
        {
           

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("gytype", "配件供应商"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                this.provider.Properties.Items.Add(dt.Rows[j]["gyname"]);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            frmAddCars che = new frmAddCars();
            che.ShowDialog();
            bindvehicleno();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            w_gongying_info info = new w_gongying_info();
            info.ShowDialog();
            bindprovider();
        }


        private void updatetyre()
        {
            string state = this.tyrestate.Text.Trim();
            if (state == "正常") state = "0"; else state = "1";
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tyreno", tyreno.Text.Trim()));
                list.Add(new SqlPara("tyreguige", tyreguige.Text.Trim()));
                list.Add(new SqlPara("azlocation", anzhuangweizhi.Text.Trim()));
                list.Add(new SqlPara("manufacturer", shengchanchangjia.Text.Trim()));
                list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
                list.Add(new SqlPara("gyname", provider.Text.Trim()));
                list.Add(new SqlPara("buydate", buydate.Text.Trim()));
                list.Add(new SqlPara("buymoney", buymoney.Text.Trim()));
                list.Add(new SqlPara("usedate", usedate.Text.Trim()));
                list.Add(new SqlPara("rejectdate", baofeidate.Text.Trim()));
                list.Add(new SqlPara("tyrestate", state));
                list.Add(new SqlPara("remark", remark.Text.Trim()));
                list.Add(new SqlPara("oldtyreno", number));
                list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("updatedate", DateTime.Now));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_TYRE", list);
                int row = SqlHelper.ExecteNonQuery(sps);
                MsgBox.ShowOK();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        
        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                updatetyre();
            }
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}