using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmDriverUserADD : BaseForm
    {
        public string ID = "", username1 ="",usermb1="", cyzgno1 ="", vehicleno1 ="", CarAscription1 = "", Province1 = "",settle1="",chauffeur1="";
        DataSet ds = new DataSet();
        public frmDriverUserADD()
        {
            InitializeComponent();
        }
        
         public frmDriverUserADD(string textName)
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(textName) & textName == "新增")
            {
                chauffeur.Properties.Items.Add("临时外请");
                chauffeur.Properties.Items.Add("挂靠");
                chauffeur.Properties.Items.Add("合同");
                chauffeur.Properties.Items.Add("长期外请");
                //this.CarAscription.Text = CommonClass.UserInfo.WebName;
                this.Text = "新增";
            }
            else 
            {
                this.Text = "修改";
            }
            
        }

        private void frmDriverUserADD_Load(object sender, EventArgs e)
        {
            if(this.Text=="修改")
            {
                //getdata();
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    Province.Properties.Items.Add(ds.Tables[0].Rows[i]["Province"]);
                //}
                //Province.Text = "";
                if (ID != "")
                {
                    username.Text = username1;
                    usermb.Text = usermb1;
                    cyzgno.Text = cyzgno1;
                    vehicleno.Text = vehicleno1;
                    //CarAscription.Text = CarAscription1;
                    //Province.Text = Province1;
                    settle.Text = settle1;
                    chauffeur.Text = chauffeur1;
                    chauffeur.Properties.Items.Add("临时外请");
                    chauffeur.Properties.Items.Add("挂靠");
                    chauffeur.Properties.Items.Add("合同");
                    chauffeur.Properties.Items.Add("长期外请");
                }
            }
            
        }

        //private void getdata()
        //{
        //    try
        //    {
        //        List<SqlPara> list = new List<SqlPara>();
        //        SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_GET_CarAscription", list);
        //        ds = SqlHelper.GetDataSet(spe);

        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (settle.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写结算周期!");
                settle.Focus();
                return;
            }
            if (username.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写司机姓名!");
                username.Focus();
                return;
            }
            if (usermb.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写司机手机号!");
                usermb.Focus();
                return;
            }
            if (vehicleno.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写司机车牌号!");
                vehicleno.Focus();
                return;
            }
            if (chauffeur.Text.Trim() == "")
            {
                MsgBox.ShowOK("请填写从业资料证号!");
                chauffeur.Focus();
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                
                list.Add(new SqlPara("chauffeurxz",chauffeur.Text.Trim()));
                list.Add(new SqlPara("settleinterval",settle.Text.Trim()));
                list.Add(new SqlPara("username", username.Text.Trim()));
                list.Add(new SqlPara("usermb", usermb.Text.Trim()));
                list.Add(new SqlPara("cyzgno", cyzgno.Text.Trim()));
                list.Add(new SqlPara("vehicleno", vehicleno.Text.Trim()));
                list.Add(new SqlPara("CarAscription", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("Province", ""));
                SqlParasEntity spe = null;
                if (this.Text == "新增")
                {
                    list.Add(new SqlPara("usertype", 0));//0:司机 1:货主 2:物流公司 3:家装
                    spe = new SqlParasEntity(OperType.Execute, "USP_ADD_userinfo", list);
                }
                else 
                {
                    list.Add(new SqlPara("userid", ID));
                    spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_userinfo_byid", list);
                }
                
                if (SqlHelper.ExecteNonQuery(spe) > 0)
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

        private void Province_SelectedValueChanged(object sender, EventArgs e)
        {

            //CarAscription.Properties.Items.Clear();
            
            //DataRow[] clgs = ds.Tables[0].Select("Province='" + Province.Text.Trim() + "'");
            //string a = clgs[0]["CarAscription"].ToString();
            //string[] b = a.Split(',');
            //for (int i = 0; i < b.Length; i++)
            //{
            //    CarAscription.Properties.Items.Add(b[i]);
            //}
        }

        private void Province_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //CarAscription.Properties.Items.Clear();
            //if (ID == "")
            //{
            //    CarAscription.Text = "";
            //}
            //DataRow[] clgs = ds.Tables[0].Select("Province='" + Province.Text.Trim() + "'");
            //string a = clgs[0]["CarAscription"].ToString();
            //string[] b = a.Split(',');
            //for (int i = 0; i < b.Length; i++)
            //{
            //    CarAscription.Properties.Items.Add(b[i]);
            //}
        }
    }
}