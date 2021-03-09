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
    public partial class w_feiyong_dengji : BaseForm
    {
        public w_feiyong_dengji()
        {
            InitializeComponent();
        }

        private void w_feiyong_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2);
            if (1 == 1)
            {

                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("tablename", "费用"));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_V_UNIT", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    this.unit.Text = ConvertType.ToString(ds.Tables[0].Rows[0][0].ToString());
                    vehicleno.Select();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.banlidate.EditValue = DateTime.Now;
            bindvehicleno();
            bindproject();
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
                addfy(); clear();
            }
        }

        private void addfy()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("unit", unit.Text.Trim()));
                list.Add(new SqlPara("bldate", banlidate.Text.Trim()));
                list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
                list.Add(new SqlPara("fyproject", feiyongproject.Text.Trim()));
                list.Add(new SqlPara("money", money.Text.Trim()));
                list.Add(new SqlPara("zfaccount", zhifuzhanghu.Text.Trim()));
                list.Add(new SqlPara("madeby", ConvertType.ToString(madeby.Text)));
                list.Add(new SqlPara("remark", ConvertType.ToString(remark.Text)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_ADD_FY", list);
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

        private void clear()
        {
            this.unit.Text = "";
            this.vehicleno.Text = "";
            this.feiyongproject.Text = "";
            this.money.Text = "";
            this.remark.Text = "";
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
            else if (this.checkunit(this.unit.Text.Trim()))
            {
                XtraMessageBox.Show("该单据编号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }


        private bool checkunit(string unit)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("unit", unit));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_FY", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds!=null && ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }


        private void bindvehicleno()
        {
            this.vehicleno.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.vehicleno.Properties.Items.Add(ds.Tables[0].Rows[j]["vehicleno"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void bindproject()
        {
            this.feiyongproject.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY_PROJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.feiyongproject.Properties.Items.Add(ds.Tables[0].Rows[j]["projectname"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
    
}