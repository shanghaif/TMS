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
    public partial class w_tyre_dengji : BaseForm
    {
        public w_tyre_dengji()
        {
            InitializeComponent();
        }
        private void w_tyre_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例

            this.buydate.EditValue = DateTime.Now;
            this.baofeidate.EditValue = DateTime.Now;
            this.usedate.EditValue = DateTime.Now;
            bindvehicleno();
            bindprovider();
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
            else if (this.buymoney.Text.Trim() != "" && !StringHelper.IsNumberId(this.buymoney.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的购买金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.provider.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择供应商", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.checkunit(this.tyreno.Text.Trim()))
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
            if (ds !=null && ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                return true;
            }
            return false;
        }

        private void clear()
        {
            this.tyreno.Text = "";
            this.vehicleno.Text = "";
            this.tyreguige.Text = "";
            this.anzhuangweizhi.Text = "";
            this.buymoney.Text = "";
            this.provider.Text = "";
            this.shengchanchangjia.Text = "";
            this.remark.Text = "";
        }

        private void bindvehicleno()
        {
            this.vehicleno.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            DataTable dt = SqlHelper.GetDataSet(sps).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt.Rows[i]["vehicleno"]);
            }
        }
        private void bindprovider()
        {
            this.provider.Properties.Items.Clear();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("gytype", "配件供应商"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list);
            DataTable dt = SqlHelper.GetDataSet(sps).Tables[0];
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

      
        private void addtyre()
        {
            string state = this.tyrestate.Text.Trim();
            if (state == "正常") state = "0"; else state = "1";
            try
            {
                List<SqlPara> list = new List<SqlPara> ();
                list.Add(new SqlPara("tyreno", tyreno.Text.Trim()));
                list.Add(new SqlPara("tyreguige",tyreguige.Text.Trim()));
                list.Add(new SqlPara("azlocation",anzhuangweizhi.Text.Trim()));
                list.Add(new SqlPara("manufacturer",shengchanchangjia.Text.Trim()));
                list.Add(new SqlPara("vehicleno",vehicleno.Text.Trim()));
                list.Add(new SqlPara("gyname",provider.Text.Trim()));
                list.Add(new SqlPara("buydate",buydate.Text.Trim()));
                list.Add(new SqlPara("buymoney",buymoney.Text.Trim()));
                list.Add(new SqlPara("usedate",usedate.Text.Trim()));
                list.Add(new SqlPara("rejectdate",baofeidate.Text.Trim()));
                list.Add(new SqlPara("tyrestate", state));
                list.Add(new SqlPara("remark",remark.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_ADD_TYRE", list);
                int row = SqlHelper.ExecteNonQuery(sps);
                if (row > 0)
                {
                    MsgBox.ShowOK();
                }

                
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
                addtyre();
                clear();
            }
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

       

    }
}