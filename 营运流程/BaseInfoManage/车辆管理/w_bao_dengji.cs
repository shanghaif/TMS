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
    public partial class w_bao_dengji : BaseForm
    {
        public w_bao_dengji()
        {
            InitializeComponent();
        }
        private void w_bao_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            if (1 == 1)
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("tablename", "保养"));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_V_UNIT", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    this.byunit.Text = ConvertType.ToString(ds.Tables[0].Rows[0][0].ToString());
                    vehicleno.Select();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.baoyangdate.EditValue = DateTime.Now;
            bindvehicleno();
            bindbyproject();
            bindcomboboxedit();
        }

        //绑定数据修理厂列表框
        private void bindcomboboxedit()
        {
            this.repairchang.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("gytype", "修理厂"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.repairchang.Properties.Items.Add(ds.Tables[0].Rows[j]["gyname"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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

        private void bindbyproject()
        {
            this.baoyangproject.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_BY_PROJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.baoyangproject.Properties.Items.Add(ds.Tables[0].Rows[j]["projectname"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private bool check()
        {
            if (this.byunit.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入保养单号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.vehicleno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择车号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.baoyangproject.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择保养项目", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.baoyangmoney.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入保养金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.baoyangmoney.Text.Trim() != "" && !StringHelper.IsDecimal(this.baoyangmoney.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的保养金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.checkbyunit(this.byunit.Text.Trim()))
            {
                XtraMessageBox.Show("该保养单号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool checkbyunit(string byunit)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("byunit", byunit));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_BY_UNIT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds !=null && ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
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

        private void clear()
        {
            this.byunit.Text = "";
            this.vehicleno.Text = "";
            this.baoyangproject.Text = "";
            this.baoyangmoney.Text = "";
            this.remark.Text = "";
            this.repairchang.Text = "";
        }

        private void addby()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("byunit", byunit.Text.Trim()));
                list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
                list.Add(new SqlPara("bydate", baoyangdate.Text.Trim()));
                list.Add(new SqlPara("byproject", baoyangproject.Text.Trim()));
                list.Add(new SqlPara("bymoney", baoyangmoney.Text.Trim()));
                list.Add(new SqlPara("zfaccount", zhifuzhanghu.Text.Trim()));
                list.Add(new SqlPara("madeby", ConvertType.ToString(madeby.Text)));
                list.Add(new SqlPara("remark", ConvertType.ToString(remark.Text)));
                list.Add(new SqlPara("gyname", ConvertType.ToString(repairchang.Text)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_ADD_BY_INFO", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            w_by_project_list plist = new w_by_project_list();
            plist.ShowDialog();
            bindbyproject();

        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                this.addby();
                this.clear();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            w_gongying_info info = new w_gongying_info();
            info.ShowDialog();
            bindcomboboxedit();
        }
    }
}