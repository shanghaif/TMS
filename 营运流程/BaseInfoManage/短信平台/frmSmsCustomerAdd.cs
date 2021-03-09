using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmSmsCustomerAdd : BaseForm
    {

        public string inxm = "";
        public frmSmsCustomerAdd()
        {
            InitializeComponent();
        }

        private void clear()
        {
            man.Text = "";
            tel.Text = "";
            mb.Text ="";
            address.Text = "";
            dtype.Text = "";
            goodat.Text = "";
            gsname.Text ="";
            sex.Text = "";
            birthday.EditValue = CommonClass.gcdate;
            man.Focus();
        }

        private void save()
        {
            if (man.Text.Trim() == "")
            {
                XtraMessageBox.Show("要输入联系人名称!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (mb.Text.Trim() == "")
            {
                XtraMessageBox.Show("要输入联系人的手机号码!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dtype.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择客户类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("mProject", man.Text.Trim()));
                list.Add(new SqlPara("mSex", sex.Text.Trim()));
                list.Add(new SqlPara("mBirthday", birthday.EditValue));
                list.Add(new SqlPara("mCellphone", mb.Text.Trim()));
                list.Add(new SqlPara("mTelephone", tel.Text.Trim()));
                list.Add(new SqlPara("mAddress", address.Text.Trim()));
                list.Add(new SqlPara("mCompanyName", gsname.Text.Trim()));
                list.Add(new SqlPara("mType", dtype.Text.Trim()));
                list.Add(new SqlPara("mHobby", goodat.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SMS_CUSTOMER");
                sps.ParaList = list;
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    clear();
                } 

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void getsmsgroup()
        {
            //dtype.Properties.Items.Clear();

            //DataSet dataset = new DataSet();
            //SqlConnection connection = cc.GetConn();
            //try
            //{
            //    SqlDataAdapter da = new SqlDataAdapter();
            //    SqlCommand sq = new SqlCommand(string.Format("select * from smsgroups where companyid='{0}'", commonclass.companyid), connection);

            //    da.SelectCommand = sq;
            //    da.Fill(dataset, "smsgroups");
            //    connection.Close();

            //    for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            //    {
            //        dtype.Properties.Items.Add(dataset.Tables[0].Rows[i][0].ToString());
            //    }
            //}
            //catch (Exception)
            //{
            //    connection.Close();
            //}
            //finally
            //{
            //    connection.Close();
            //}
        }

        private void fill(string mProject)
        {
            try
            {


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("mProject", mProject));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUSTOMERSMS_ByID", list);
                DataSet dataset = SqlHelper.GetDataSet(sps);
                if (dataset == null || dataset.Tables.Count == 0) return;




                //--------------------------------------------------------------------------------------------------
                man.Text = dataset.Tables[0].Rows[0]["xm"].ToString();


                dtype.Text = dataset.Tables[0].Rows[0]["dtype"].ToString();
                goodat.Text = dataset.Tables[0].Rows[0]["goodat"].ToString();
                address.Text = dataset.Tables[0].Rows[0]["address"].ToString();
                gsname.Text = dataset.Tables[0].Rows[0]["gsname"].ToString();
                sex.Text = dataset.Tables[0].Rows[0]["sex"].ToString();
                mb.Text = dataset.Tables[0].Rows[0]["mb"].ToString();
                tel.Text = dataset.Tables[0].Rows[0]["tel"].ToString();

                birthday.EditValue = Convert.ToDateTime(dataset.Tables[0].Rows[0]["birthday"]);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void w_add_vehicle_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);
            if (inxm == "")
            {
                clear();
                getsmsgroup();
            }
            else
            {
                fill(inxm);
            }
            string[] CustomTypeModeList = CommonClass.Arg.CustomType.Split(',');
            if (CustomTypeModeList.Length > 0)
            {
                for (int i = 0; i < CustomTypeModeList.Length; i++)
                {
                    dtype.Properties.Items.Add(CustomTypeModeList[i]);
                }
                dtype.SelectedIndex = 0;
            }

            //commonclass.SetBarPropertity(bar1);//工具
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            clear();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            save();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void w_sms_customer_add_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                save();
            }
        }
    }
}