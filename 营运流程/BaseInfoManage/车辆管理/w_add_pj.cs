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
    public partial class w_add_pj : BaseForm
    {
        public w_add_pj()
        {
            InitializeComponent();
        }
        private bool check()
        {
            if (this.pjname.Text.Trim() == "")
            {

                XtraMessageBox.Show("请输入配件名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.danwei.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入单位", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.jhprice.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入价格价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.jhprice.Text.Trim() != "" && !StringHelper.IsNumberId(this.jhprice.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的价格价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            //else if (this.count.Text.Trim() == "")
            //{
            //    XtraMessageBox.Show("请输入当前数量", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            //else if (this.count.Text.Trim() != "" && !SqlHelper.IsNumber(count.Text.Trim()))
            //{
            //    XtraMessageBox.Show("请输入有效的数量", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            //else if (this.count.Text.Trim() != "" && Convert.ToInt32(count.Text.Trim()) == 0)
            //{
            //    XtraMessageBox.Show("当前数量不能为0", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return false;
            //}
            else if (this.checkpjname(this.pjname.Text.Trim()))
            {
                XtraMessageBox.Show("该配件名称已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;

        }

        private bool checkpjname(string pjname)
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("pjname", pjname));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_PJNAME", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds != null && ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return false;
            }
        }
        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                add();
                clear();
            }
        }

        private void clear()
        {
            this.pjname.Text = "";
            this.danwei.Text = "";
            this.jhprice.Text = "";
            this.count.Text = "";
        }

        private void add()
        {
            try
            {
                string count = this.count.Text.Trim() == "" ? "0" : this.count.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("pjname",pjname.Text.Trim()));
                list.Add(new SqlPara("units", danwei.Text.Trim()));
                list.Add(new SqlPara("pjprice", jhprice.Text.Trim()));
                list.Add(new SqlPara("nowcount", count));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_ADD_PJ_GUIGE", list);
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

        private void w_add_pj_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
        }
    }
}