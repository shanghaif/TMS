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
    public partial class w_update_jy_dj : BaseForm
    {
        public w_update_jy_dj()
        {
            InitializeComponent();
        }
        private string jyunit = string.Empty;
        private decimal oldmonay = 0;

        public w_update_jy_dj(string unit)
        {
            InitializeComponent();
            this.jyunit = unit;
        }

        private void w_update_jy_dj_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 
            this.madeby.Text = CommonClass.UserInfo.UserName;
            bindcomboboxedit();
            getdata();
        }

        private void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("jyunit", jyunit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_JY", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.jiayouno.Text = dt.Rows[0]["jyunit"].ToString();
            this.jiayoustation.Text = dt.Rows[0]["gyname"].ToString();
            this.vehicleno.Text = dt.Rows[0]["vehicleno"].ToString();
            this.youweight.Text = dt.Rows[0]["weight"].ToString();
            this.youprice.Text = dt.Rows[0]["price"].ToString();
            this.accnow.Text = dt.Rows[0]["accnow"].ToString();
            this.youmoney.Text = dt.Rows[0]["money"].ToString();
            this.yifu.Text = dt.Rows[0]["accyifu"].ToString();
            this.weifu.Text = dt.Rows[0]["accweifu"].ToString();
            this.ranyoutype.Text = dt.Rows[0]["rytype"].ToString();
            this.youkano.Text = dt.Rows[0]["serialnumber"].ToString();
            this.remark.Text = dt.Rows[0]["remark"].ToString();
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.jiayoudate.EditValue = dt.Rows[0]["jydate"].ToString();
            this.accnowzhanghu.Text = dt.Rows[0]["accnowno"].ToString();
            oldmonay = Convert.ToDecimal(dt.Rows[0]["price"].ToString()) * Convert.ToDecimal(dt.Rows[0]["weight"].ToString());
            
        }

        private void bindcomboboxedit()
        {
            bindvehicleno();
            bindjiayoustation();
            bindyoukano();
            bindranyoutype();
            bindrytype();
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
        private void bindjiayoustation()
        {
            this.jiayoustation.Properties.Items.Clear();


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("gytype", "加油站"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                this.jiayoustation.Properties.Items.Add(dt.Rows[j]["gyname"]);
            }
        }

        private void bindyoukano()
        {
            this.youkano.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_OIL");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                this.youkano.Properties.Items.Add(dt.Rows[k]["serialnumber"]);
            }
        }

        private void bindrytype()
        {
            this.ranyoutype.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_RYTYPE_LIST");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                this.ranyoutype.Properties.Items.Add(dt.Rows[k]["typename"]);
            }
        }

        private void bindranyoutype()
        {
            this.ranyoutype.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_RY_TYPE");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            for (int k = 0; k < dt.Rows.Count; k++)
            {
                this.ranyoutype.Properties.Items.Add(dt.Rows[k]["typename"]);
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmAddCars info = new frmAddCars();
            info.ShowDialog();
            bindvehicleno();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            w_gongying_info info = new w_gongying_info();
            info.ShowDialog();
            bindjiayoustation();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //w_youka_edit edit = new w_youka_edit();
            //edit.ShowDialog();
            //bindyoukano();
        }




        private void updatejydj()
        {
            try
            {
                string youmoney, accnow, yifu, weifu, price;
                if (this.youprice.Text.Trim() == "") price = "0"; else price = this.youprice.Text.Trim();
                if (this.youmoney.Text.Trim() == "") youmoney = "0"; else youmoney = this.youmoney.Text.Trim();
                if (this.accnow.Text.Trim() == "") accnow = "0"; else accnow = this.accnow.Text.Trim();
                if (this.yifu.Text.Trim() == "") yifu = "0"; else yifu = this.yifu.Text.Trim();
                if (this.weifu.Text.Trim() == "") weifu = "0"; else weifu = this.weifu.Text.Trim();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("jyunit", jiayouno.Text.Trim()));
                list.Add(new SqlPara("jydate", jiayoudate.DateTime));
                list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
                list.Add(new SqlPara("gyname", jiayoustation.Text.Trim()));
                list.Add(new SqlPara("serialnumber", youkano.Text.Trim()));
                list.Add(new SqlPara("rytype", ranyoutype.Text.Trim()));
                list.Add(new SqlPara("weight", youweight.Text.Trim()));
                list.Add(new SqlPara("price", price));
                list.Add(new SqlPara("money", youmoney));
                list.Add(new SqlPara("accnow", accnow));
                list.Add(new SqlPara("accnowno", accnowzhanghu.Text.Trim()));
                list.Add(new SqlPara("accyifu", yifu));
                list.Add(new SqlPara("accweifu", weifu));
                list.Add(new SqlPara("remark", remark.Text.Trim()));
                list.Add(new SqlPara("oldmoney", oldmonay));
                list.Add(new SqlPara("oldjyunit", jyunit));
                list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("updatedate", DateTime.Now));


  

                List<SqlPara> list2 = new List<SqlPara>();
                list2.Add(new SqlPara("serialnumber", this.youkano.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_NOWMONEY", list2);
                DataSet ds = SqlHelper.GetDataSet(sps);
                decimal nowmoney = Convert.ToDecimal(ds.Tables[0].Rows[0]["nowmoney"]);
                decimal money = Convert.ToDecimal(this.youweight.Text.Trim()) * Convert.ToDecimal(this.youprice.Text.Trim());
                if (nowmoney + oldmonay < money)
                {
                    XtraMessageBox.Show("对不起，该油卡余额还有 " + (nowmoney + oldmonay) + " 元可用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SqlParasEntity SPS1 = new SqlParasEntity(OperType.Query, "V_UPDATE_JY_DJ", list);
                int row = SqlHelper.ExecteNonQuery(SPS1);
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

        private bool check()
        {
            if (this.jiayouno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入加油单号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.vehicleno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择车号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.jiayoustation.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择加油站", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.ranyoutype.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择燃油种类", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.youweight.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入重量（升）", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.youweight.Text.Trim() != "" && !StringHelper.IsNumberId(this.youweight.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的重量", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.youprice.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入油价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.youprice.Text.Trim() != "" && !StringHelper.IsNumberId(this.youprice.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的油价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.youmoney.Text.Trim() != "" && !StringHelper.IsNumberId(this.youmoney.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的应付总价", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.yifu.Text.Trim() != "" && !StringHelper.IsNumberId(this.yifu.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的已付金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.weifu.Text.Trim() != "" && !StringHelper.IsNumberId(this.weifu.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的未付金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.jyunit != this.jiayouno.Text.Trim() && this.checkunit(this.jiayouno.Text.Trim()))
            {
                XtraMessageBox.Show("该加油单号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;


        }

        private bool checkunit(string jyunit)
        {
            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("jyunit", jyunit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_JY_DJ", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            int count = ConvertType.ToInt32(ds.Tables[0].Rows[0][0]);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            w_ry_typelist rylist = new w_ry_typelist();
            rylist.ShowDialog();
            bindrytype();
        }

        private void ranyoutype_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ranyoutype = this.ranyoutype.Text.Trim();
            if (ranyoutype != "")
            {
                List<SqlPara> list2 = new List<SqlPara>();
                list2.Add(new SqlPara("typename", ranyoutype));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_RYTYPE_PRICE", list2);
                DataSet ds = SqlHelper.GetDataSet(sps);
                decimal count = ConvertType.ToDecimal(ds.Tables[0].Rows[0][0]);

                this.youprice.Text = count.ToString();
          
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void saveupdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                updatejydj();
            }
        }

        private void youweight_EditValueChanged(object sender, EventArgs e)
        {
            summoney();
        }

        private void youprice_EditValueChanged(object sender, EventArgs e)
        {
            summoney();
        }

        private void summoney()
        {
            string weight = this.youweight.Text.Trim();
            string price = this.youprice.Text.Trim();
            if (StringHelper.IsNumberId(weight) && StringHelper.IsNumberId(price) && price != "" && weight != "")
            {
                this.youmoney.Text = (Convert.ToDecimal(weight) * Convert.ToDecimal(price)).ToString();
            }
            if (weight == "" || price == "")
            {
                this.youmoney.Text = "";
            }
        }

        private void yifu_EditValueChanged(object sender, EventArgs e)
        {
            string y = yifu.Text.Trim() == "" ? "0" : yifu.Text.Trim();
            string w = weifu.Text.Trim() == "" ? "0" : weifu.Text.Trim();
            string nowfu = youmoney.Text.Trim() == "" ? "0" : youmoney.Text.Trim();
            if (StringHelper.IsNumberId(y) && StringHelper.IsNumberId(w) && StringHelper.IsNumberId(nowfu) && nowfu != "0")
            {
                this.weifu.Text = (Convert.ToDecimal(nowfu) - Convert.ToDecimal(y)).ToString();
            }
            else
            {
                this.weifu.Text = "";
            }
        }
        private void weifu_EditValueChanged(object sender, EventArgs e)
        {
            string y = yifu.Text.Trim() == "" ? "0" : yifu.Text.Trim();
            string w = weifu.Text.Trim() == "" ? "0" : weifu.Text.Trim();
            string nowfu = youmoney.Text.Trim() == "" ? "0" : youmoney.Text.Trim();
            if (StringHelper.IsNumberId(y) && StringHelper.IsNumberId(w) && StringHelper.IsNumberId(nowfu) && nowfu != "0")
            {
                this.yifu.Text = (Convert.ToDecimal(nowfu) - Convert.ToDecimal(w)).ToString();
            }
            else
            {
                this.yifu.Text = "";
            }
        }
        
    }
}