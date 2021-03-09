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
    public partial class w_update_by_dj : BaseForm
    {
        public w_update_by_dj()
        {
            InitializeComponent();
        }
        private string unit = string.Empty;
        public w_update_by_dj(string byunit)
        {
            InitializeComponent();
            this.unit = byunit;
        }

        private void w_update_by_dj_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 

            bindvehicleno();
            bindbyproject();
            getdata();
            bindcomboboxedit();
        }

        //绑定数据修理厂列表框
        private void bindcomboboxedit()
        {

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("gytype", "修理厂"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            this.repairchang.Properties.Items.Clear();
            DataTable dt2 = ds.Tables[0];
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                this.repairchang.Properties.Items.Add(dt2.Rows[j]["gyname"]);
            }
        }

        private void getdata()
        {

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("byunit", unit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_BY_INFO", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.byunit.Text = dt.Rows[0]["byunit"].ToString();
            this.vehicleno.Text = dt.Rows[0]["vehicleno"].ToString();
            this.baoyangdate.Text = dt.Rows[0]["bydate"].ToString();

            this.zhifuzhanghu.Text = dt.Rows[0]["zfaccount"].ToString();
            this.baoyangproject.Text = dt.Rows[0]["byproject"].ToString();
            this.baoyangmoney.Text = dt.Rows[0]["bymoney"].ToString();
            this.repairchang.Text = dt.Rows[0]["gyname"].ToString();
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.remark.Text = dt.Rows[0]["remark"].ToString();
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
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt.Rows[i]["vehicleno"]);
            }
        }

        private void bindbyproject()
        {
            this.baoyangproject.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_BY_PROJECT");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0] ;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.baoyangproject.Properties.Items.Add(dt.Rows[i]["projectname"]);
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
            else if (this.byunit.Text.Trim() != this.unit && this.checkbyunit(this.byunit.Text.Trim()))
            {
                XtraMessageBox.Show("该保养单号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool checkbyunit(string byunit)
        {
            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("byunit", byunit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_BY_UNIT", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            int count = ConvertType.ToInt32(ds.Tables[0].Rows[0][0]);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        private void updateby()
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
                list.Add(new SqlPara("madeby", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("remark", remark.Text.Trim()));
                list.Add(new SqlPara("oldbyunit", unit));
                list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("updatedate", DateTime.Now));
                list.Add(new SqlPara("gyname", repairchang.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_UPDATE_BY_INFO", list);
                int row = SqlHelper.ExecteNonQuery(sps);
                    MsgBox.ShowOK();
                    this.Close();
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show(ex.Message);
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
                this.updateby();
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