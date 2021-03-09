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
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_update_fy_dj : BaseForm
    {
        private string funit = string.Empty;
        public w_update_fy_dj()
        {
            InitializeComponent();
        }

        public w_update_fy_dj(string fyunit)
        {
            InitializeComponent();
            this.funit = fyunit;
        }

        private void w_update_fy_dj_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 
            bindvehicleno();
            bindproject();
            getdata();
        }

        private void getdata()
        {

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("unit", funit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.unit.Text = dt.Rows[0]["unit"].ToString();
            this.vehicleno.Text = dt.Rows[0]["vehicleno"].ToString();
            this.banlidate.Text = dt.Rows[0]["bldate"].ToString();
            this.remark.Text = dt.Rows[0]["remark"].ToString();
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.money.Text = dt.Rows[0]["money"].ToString();
            this.zhifuzhanghu.Text = dt.Rows[0]["zfaccount"].ToString();
            this.feiyongproject.Text = dt.Rows[0]["fyproject"].ToString();
        }



        private void simpleButton3_Click(object sender, EventArgs e)
        {
            frmAddCars info = new frmAddCars();
            info.ShowDialog();
            bindvehicleno();
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            w_fy_project_list list = new w_fy_project_list();
            list.ShowDialog();
            bindproject();
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                updatefy();
            }
        }

        private void updatefy()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("unit", unit.Text.Trim()));
            list.Add(new SqlPara("bldate", banlidate.DateTime));
            list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
            list.Add(new SqlPara("fyproject", feiyongproject.Text.Trim()));
            list.Add(new SqlPara("money", money.Text.Trim()));
            list.Add(new SqlPara("zfaccount", zhifuzhanghu.Text.Trim()));
            list.Add(new SqlPara("remark", remark.Text.Trim()));
            list.Add(new SqlPara("oldunit", funit));
            list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("updatedate", DateTime.Now));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_UPDATE_FY", list);
            int row = SqlHelper.ExecteNonQuery(sps);
            MsgBox.ShowOK();
            this.Close();
        }

      

        private bool check()
        {
            if (this.unit.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入单据编号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.vehicleno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择车号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.feiyongproject.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择费用项目", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.money.Text.Trim() != "" && !StringHelper.IsDecimal(this.money.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (funit != this.unit.Text.Trim() && this.checkunit(this.unit.Text.Trim()))
            {
                XtraMessageBox.Show("该单据编号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }


        private bool checkunit(string unit)
        {
            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("unit", unit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_FY", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            int count = ConvertType.ToInt32(ds.Tables[0].Rows[0][0]);
            if (count > 0)
            {
                return true;
            }
            return false;
        }


        private void bindvehicleno()
        {

            List<SqlPara> list2 = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            this.vehicleno.Properties.Items.Clear();
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt.Rows[i]["vehicleno"]);
            }
        }

        private void bindproject()
        {
            List<SqlPara> list2 = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY_PROJECT", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            this.feiyongproject.Properties.Items.Clear();
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.feiyongproject.Properties.Items.Add(dt.Rows[i]["projectname"]);
            }
        }

    }
}