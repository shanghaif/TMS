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
    public partial class w_gongying_info : BaseForm
    {
        public w_gongying_info()
        {
            InitializeComponent();
        }
        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        public string sgyid = "";
        public string sgyname = "";
        public string sgytype = "";
        public string sgongyingname = "";
        public string slinkman = "";
        public string slinkmantel = "";
        public string slinkmanaddr = "";
        public string sswdjno = "";
        public string skhbank = "";
        public string sbankno = "";
        public string sremark = "";

        private void w_gongying_info_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("供应商维护");//xj/2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2);
            getgytype();
            if (sgyid != "") 
            {
                this.gysname.Text = sgyname;
                this.gongyingname.Text = sgongyingname;
                this.gytype.Text = sgyname;
                this.linkman.Text = slinkman;
                this.linktel.Text = slinkmantel;
                this.linkaddr.Text = slinkmanaddr;
                this.shuiwuno.Text = sswdjno;
                this.remark.Text = sremark;
                this.kaihubank.Text = skhbank;
                this.bankno.Text = sbankno;
            }
        }

        private void getgytype()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_GY_TYPE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    this.gytype.Properties.Items.Add(ds.Tables[0].Rows[i]["name"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void clear()
        {
            this.gysname.Text = "";
            this.gongyingname.Text = "";
            this.gytype.Text = "";
            this.linkman.Text = "";
            this.linktel.Text = "";
            this.linkaddr.Text = "";
            this.shuiwuno.Text = "";
            this.remark.Text = "";
            this.kaihubank.Text = "";
            this.bankno.Text = "";
        }

        private bool check()
        {
            if (gysname.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入供应商名称", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (gytype.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择供应商类别", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (linkman.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入联系人", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (kaihubank.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入开户银行", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (bankno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入银行账号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.checkgyname(gysname.Text.Trim()) && sgyid == "")
            {
                XtraMessageBox.Show("该供应商已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void addgy()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("sgyid", sgyid));
                list.Add(new SqlPara("gyname", gysname.Text.Trim()));
                list.Add(new SqlPara("gytype", gytype.Text.Trim()));
                list.Add(new SqlPara("gongyingname", gongyingname.Text.Trim()));
                list.Add(new SqlPara("linkman", linkman.Text.Trim()));
                list.Add(new SqlPara("linkmantel", linktel.Text.Trim()));
                list.Add(new SqlPara("linkmanaddr", linkaddr.Text.Trim()));
                list.Add(new SqlPara("swdjno", ConvertType.ToString(shuiwuno.Text)));
                list.Add(new SqlPara("khbank", ConvertType.ToString(kaihubank.Text)));
                list.Add(new SqlPara("bankno", ConvertType.ToString(bankno.Text)));
                list.Add(new SqlPara("remark", ConvertType.ToString(remark.Text)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_ADD_GYINFO", list);
                SqlHelper.ExecteNonQuery(sps);
               
                    MsgBox.ShowOK();
                    this.Close();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                addgy();
                clear();
            }
        }

        private bool checkgyname(string gyname)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("gyname", gyname));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_GYNAME", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
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
    }
}