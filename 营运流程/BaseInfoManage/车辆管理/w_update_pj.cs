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
    public partial class w_update_pj : BaseForm
    {
        public w_update_pj()
        {
            InitializeComponent();
        }
        private string peijianname = string.Empty;
        private string pjn,dw;
        private decimal pr;
        private int num;

        public w_update_pj(string pjname, string units, decimal price, int count)
        {
            InitializeComponent();
            this.pjn = pjname;
            this.dw = units;
            this.pr = price;
            this.num = count;
            this.peijianname = pjname;
        }

        private void w_update_pj_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 
            this.pjname.Text = pjn;
            this.danwei.Text = dw;
            this.jhprice.Text = pr.ToString();
            this.count.Text = num.ToString();
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
                XtraMessageBox.Show("请输入单价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.jhprice.Text.Trim() != "" && !StringHelper.IsDecimal(this.jhprice.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的单价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.pjname.Text.Trim()!=this.peijianname && this.checkpjname(this.pjname.Text.Trim()))
            {
                XtraMessageBox.Show("该配件名称已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;

        }

        private void update()
        {
            try
            {
                string count = this.count.Text.Trim() == "" ? "0" : this.count.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("pjname", pjname.Text.Trim()));
                list.Add(new SqlPara("oldpjname", peijianname));
                list.Add(new SqlPara("units", danwei.Text.Trim()));
                list.Add(new SqlPara("pjprice", jhprice.Text.Trim()));
                list.Add(new SqlPara("nowcount", count));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_PJ_GUIGE", list);
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

        private bool checkpjname(string pjname)
        {

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("pjname", "pjname"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_PJNAME", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds!=null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.check())
            {
                this.update();
            }
            this.Close();
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

    }
}