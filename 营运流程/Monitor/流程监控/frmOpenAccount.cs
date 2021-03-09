using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ZQTMS.UI
{
    public partial class frmOpenAccount : BaseForm
    {
        public frmOpenAccount()
        {
            InitializeComponent();
        }
        public int account_code = 0;
        private void frmOpenAccount_Load(object sender, EventArgs e)
        {
            txt_UserID.Text = DateTime.Now.ToString("yyyyMMddHHmmss");
            txt_accountType.Properties.ReadOnly = true;
            if (account_code == 1)
            {
                txt_accountType.SelectedIndex = 1;
                xtraTabControl1.SelectedTabPage = tp3;
                txt_WebName.Text = CommonClass.UserInfo.WebName;
                txt_UserType.SelectedIndex = 0;
                txt_UserType.Properties.ReadOnly = false;
            }
            else
            {
                txt_accountType.SelectedIndex = 0;
                txt_UserType.SelectedIndex =1;
                txt_UserType.Properties.ReadOnly = true;
            }
            xtraTabControl1.TabPages.Remove(tp3);
            xtraTabControl1.TabPages.Remove(tp4);

            //tp3.Parent = null;

           /// tp4.Parent = null;
        }


        private void btn_register_Click(object sender, EventArgs e)
        {
            try
            {
                string accountType = txt_accountType.Text.Trim();
                string UserID = txt_UserID.Text.Trim();
                string mobile = txt_Mobile.Text.Trim();
                string LegalPerson = txt_Name.Text.Trim();
                string id_no = txt_ID.Text.Trim();
                string id_expiry_date = txt_IDexpiryDate.Text.Trim();
                string link_man = txt_LinkMan.Text.Trim();
                string enterprise_name = txt_CompanyName.Text.Trim();
                string address = txt_Address.Text.Trim();
                string license_type = txt_LicType.Text.Trim();
                string business_license = txt_LicName.Text.Trim();
                string province = txt_Province.Text.Trim();
                string license_expiry_date = txt_LicExpiryDate.Text.Trim();
                string city = txt_City.Text.Trim();
                string district = txt_District.Text.Trim();
                string user_type = txt_UserType.Text.Trim();
                string merchant_type = txt_MerchantType.Text.Trim();
                string webName = txt_WebName.Text.Trim();

                if (string.IsNullOrEmpty(accountType))
                {

                    MsgBox.ShowOK("账户类型不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(UserID))
                {
                    MsgBox.ShowOK("用户编号不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(mobile))
                {
                    MsgBox.ShowOK("手机号不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(LegalPerson))
                {
                    MsgBox.ShowOK("姓名/法人,不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(id_no))
                {
                    MsgBox.ShowOK("证件号码,不能为空!");
                    return;
                }
                if (string.IsNullOrEmpty(user_type))
                {
                    MsgBox.ShowOK("会员类型不能为空!");
                    return;
                }
                if (user_type == "企业")
                {
                    if (string.IsNullOrEmpty(business_license))
                    {
                        MsgBox.ShowOK("营业执照号，不能为空!");
                        return;
                    }

                    if (string.IsNullOrEmpty(merchant_type))
                    {
                        MsgBox.ShowOK("商户注册类型，不能为空!");
                        return;
                    }
                }
                if (accountType == "网点支付账户")
                {
                    if (webName == "")
                    {
                        MsgBox.ShowOK("网点名称不能为空!");
                        return;
                    }
                }



                string license_type_code = "";
                if (license_type == "普通营业执照")
                {
                    license_type_code = "01";
                }
                else if (license_type == "多证合一营业执照（存在独立的组织机构代码证）(合证不合号)")
                {
                    license_type_code = "02";
                }
                else if (license_type == "多证合一营业执照（不存在独立的组织机构代码证） (合证合号)")
                {
                    license_type_code = "03";
                }
                string user_type_code = "";
                user_type_code = user_type == "个人" ? "01" : "02";
                string merchant_type_code = "";
                switch (merchant_type)
                {
                    case "法人": merchant_type_code = "01"; break;
                    case "其他组织": merchant_type_code = "02"; break;
                    case "个体工商户": merchant_type_code = "03"; break;
                    case "自然人": merchant_type_code = "04"; break;
                    default: merchant_type_code = ""; break;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", webName));
                list.Add(new SqlPara("UserID", UserID));
                list.Add(new SqlPara("mobile", mobile));
                list.Add(new SqlPara("LegalPerson", LegalPerson));
                list.Add(new SqlPara("id_no", id_no));
                list.Add(new SqlPara("id_expiry_date", id_expiry_date == "" ? "1900-01-01 00:00:00" : id_expiry_date));
                list.Add(new SqlPara("link_man", link_man));
                list.Add(new SqlPara("enterprise_name", enterprise_name));
                list.Add(new SqlPara("address", address));
                list.Add(new SqlPara("license_type", license_type_code));
                list.Add(new SqlPara("business_license", business_license));
                list.Add(new SqlPara("province", province));
                list.Add(new SqlPara("license_expiry_date", license_expiry_date==""?"1900-01-01 00:00:00":license_expiry_date));
                list.Add(new SqlPara("city", city));
                list.Add(new SqlPara("district", district));
                list.Add(new SqlPara("user_type", user_type_code));
                list.Add(new SqlPara("merchant_type", merchant_type_code));
                list.Add(new SqlPara("AccountType", accountType));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WalletAccountInfo", list);
                int i = SqlHelper.ExecteNonQuery_ZQTMS(sps);
                if (i <= 0)
                {
                    MsgBox.ShowOK("此证件号已经注册过!");
                    return;
                }

                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("appid", "3501001");//支付类型14支付宝扫描15微信扫码16收银台
                dic.Add("ext_user_id", UserID);
                dic.Add("mobile", mobile);
                dic.Add("name", LegalPerson);
                dic.Add("id_no", id_no);
                dic.Add("user_type", user_type_code);
                dic.Add("id_expiry_date", id_expiry_date);
                dic.Add("link_man", link_man);
                dic.Add("enterprise_name", enterprise_name);
                dic.Add("address", address);
                dic.Add("license_type", license_type_code);
                dic.Add("business_license", business_license);
                dic.Add("province", province);
                dic.Add("license_expiry_date", license_expiry_date);
                dic.Add("city", city);
                dic.Add("district", district);
                dic.Add("merchant_type", merchant_type_code);
                dic.Add("DBName", "");//数据库名，为当前系统数据库名ZQTMSTEST20180803
                dic.Add("DBInfo", "");//数据库连接字符串，为当前系统连接字符串server=192.168.3.200;database=ZQTMSTEST20180803;uid=sa;pwd=lq123!@#;Pooling=true;Min Pool Size=0;Max Pool Size=500
                string requestStr = JsonConvert.SerializeObject(dic);


                RequestModel<string> request = new RequestModel<string>();
                request.Request = requestStr;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://ZQTMS.dekuncn.com:8012/WalletPayService/WalletIndex");
                if (result.State == "200")
                {
                    if (OpenBrowsers(result.Result) == false)
                    {
                        MessageBox.Show("打开浏览器失败！");
                        return;
                    }
                }
                if (result.State == "201")
                {
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("UserID", UserID));
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "USP_ADD_WalletAccountInfo", list);
                    int j = SqlHelper.ExecteNonQuery_ZQTMS(sps1);
                }
                //if (result.State == "200")
                //{

                //    List<SqlPara> list = new List<SqlPara>();


                //    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_AreaNew", list);
                //    if (SqlHelper.ExecteNonQuery(sps) > 0)
                //    {
                //        MsgBox.ShowOK();
                //        this.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {

                MsgBox.ShowOK(ex.Message);
            }
        }

        public static bool OpenBrowsers(string url)
        {
            try
            {
                System.Diagnostics.Process.Start(url);
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("充值打开浏览器失败" + ex);
                //  LogHelper.Error("财付通充值打开浏览器失败！", ex);
            }
            return false;
        }

        private void txt_accountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_accountType.SelectedIndex == 0)
            {
                txt_WebName.Text = "";
                txt_WebName.Properties.ReadOnly = true;
            }
            if (txt_accountType.SelectedIndex == 1)
            {
                txt_WebName.Properties.ReadOnly = false;
            }
        }

        private void txt_UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_UserType.Text.Trim() == "个人")
            {
                txt_CompanyName.Text = "";
                txt_CompanyName.Enabled = false;
                txt_LinkMan.Text = "";
                txt_LinkMan.Enabled = false;

                txt_LicName.Text = "";
                txt_LicName.Enabled = false;
                txt_LicType.Text = "";
                txt_LicType.Enabled = false;
                txt_LicExpiryDate.Text = "";
                txt_LicExpiryDate.Enabled = false;
                txt_Province.Text = "";
                txt_Province.Enabled = false;
                txt_City.Text = "";
                txt_City.Enabled = false;
                txt_District.Text = "";
                txt_District.Enabled = false;
                txt_MerchantType.Text = "";
                txt_MerchantType.Enabled = false;
                txt_Address.Text = "";
                txt_Address.Enabled = false;

            }
            else
            {
                txt_CompanyName.Text = "";
                txt_CompanyName.Enabled = true;
                txt_LinkMan.Text = "";
                txt_LinkMan.Enabled = true;

                txt_LicName.Text = "";
                txt_LicName.Enabled = true;
                txt_LicType.Text = "";
                txt_LicType.Enabled = true;
                txt_LicExpiryDate.Text = "";
                txt_LicExpiryDate.Enabled = true;
                txt_Province.Text = "";
                txt_Province.Enabled = true;
                txt_City.Text = "";
                txt_City.Enabled = true;
                txt_District.Text = "";
                txt_District.Enabled = true;
                txt_MerchantType.Text = "";
                txt_MerchantType.Enabled = true;
                txt_Address.Text = "";
                txt_Address.Enabled = true;

            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string id_no = txtID.Text.Trim();
            string pwd=txtPWD.Text.Trim();
            string company = txtCompany.Text.Trim();//公司名称
            if (name == "")
            {
                MsgBox.ShowOK("姓名/法人不能为空!");
                return;
            }
            if (phone == "")
            {
                //MsgBox.ShowOK("电话号码不能为空!");
                //return;
            }
            if (id_no == "")
            {
                MsgBox.ShowOK("证件号不能为空!");
                return;
            }
            if (company == "")
            {
                MsgBox.ShowOK("公司名称不能为空!");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("LegalPerson",name));
                list.Add(new SqlPara("mobile", phone));
                list.Add(new SqlPara("id_no", id_no));
                //list.Add(new SqlPara("Pwd",StringHelper.Md5Hash(pwd)));
                list.Add(new SqlPara("enterprise_name", company));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WalletAccountInfo_byPhone", list);
                DataSet ds = SqlHelper.GetDataSet_ZQTMS(sps);
                string ext_user_id = "";
                string mobile = "";
                //string name = "";
                string user_type = "";
                string id_expiry_date = "";
                string link_man = "";
                string enterprise_name = "";
                string address = "";
                string license_type = "";
                string business_license = "";
                string province = "";
                string city = "";
                string district = "";
                string merchant_type = "", license_expiry_date="";
                string dbPwd = "";


               
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("账户不存在，请先注册!");
                    return;
                }
                else
                { 
                    DataRow dr=ds.Tables[0].Rows[0];
                    ext_user_id = dr["UserID"].ToString();
                    mobile = dr["mobile"].ToString();
                    user_type = dr["user_type"].ToString();
                    id_expiry_date = dr["id_expiry_date"].ToString();
                    link_man = dr["link_man"].ToString();
                    enterprise_name = dr["enterprise_name"].ToString();
                    address = dr["address"].ToString();
                    license_type = dr["license_type"].ToString();
                    business_license = dr["business_license"].ToString();
                    province = dr["province"].ToString();
                    city = dr["city"].ToString();
                    district = dr["district"].ToString();
                    license_expiry_date = dr["license_expiry_date"].ToString();
                    merchant_type = dr["merchant_type"].ToString();
                    dbPwd = dr["Pwd"].ToString();
                    if(dbPwd!=StringHelper.Md5Hash(pwd))
                    {
                        MsgBox.ShowOK("密码不正确!");
                        return;
                    }
                    this.txtName.Text = "";
                    this.txtPhone.Text = "";
                    this.txtID.Text = "";
                    this.txtPWD.Text = "";
                
                }


                Dictionary<string, object> dic = new Dictionary<string, object>();

                dic.Add("appid", "3501001");//支付类型14支付宝扫描15微信扫码16收银台//3497001
                dic.Add("ext_user_id", ext_user_id);
                dic.Add("mobile", mobile==""?"123":mobile);//登陆时时间号不是必须的，所有随意给个默认值
                dic.Add("name", name);
                dic.Add("id_no", id_no);
                dic.Add("user_type", user_type);
                dic.Add("id_expiry_date", id_expiry_date);
                dic.Add("link_man", link_man);
                dic.Add("enterprise_name", enterprise_name);
                dic.Add("address", address);
                dic.Add("license_type", license_type);
                dic.Add("business_license", business_license);
                dic.Add("province", province);
                dic.Add("license_expiry_date", license_expiry_date);
                dic.Add("city", city);
                dic.Add("district", district);
                dic.Add("merchant_type", merchant_type);
                dic.Add("DBName", "ZQTMSTEST20180803");//数据库名，为当前系统数据库名
                dic.Add("DBInfo", "server=192.168.3.200;database=ZQTMSTEST20180803;uid=sa;pwd=lq123!@#;Pooling=true;Min Pool Size=0;Max Pool Size=500");//数据库连接字符串，为当前系统连接字符串
                string requestStr = JsonConvert.SerializeObject(dic);


                RequestModel<string> request = new RequestModel<string>();
                request.Request = requestStr;
                request.OperType = 0;
                string json = JsonConvert.SerializeObject(request);
                //ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://localhost:12345/WalletPayService/WalletIndex");
                ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://ZQTMS.dekuncn.com:8012/WalletPayService/WalletIndex");
                if (result.State == "200")
                {
                    if (OpenBrowsers(result.Result) == false)
                    {
                        MessageBox.Show("打开浏览器失败！");
                        return;
                    }
                }
                if (result.State != "200")
                { 
                  MsgBox.ShowOK(result.Message+":"+result.Result);
                }
               
            }
            catch(Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btn_quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string user_id = txtQNO.Text.Trim();
            string id_no = txtQid_no.Text.Trim();
            //string company = txtQCompany.Text.Trim();
            if (string.IsNullOrEmpty(user_id) && string.IsNullOrEmpty(id_no))
            {
                MsgBox.ShowOK("用户编号和证件号至少填一项！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("UserID",user_id));
                list.Add(new SqlPara("id_no", id_no));
                //list.Add(new SqlPara("enterprise_name", company));//公司名称
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WalletAccountInfo_byID", list);
                DataSet ds = SqlHelper.GetDataSet_ZQTMS(sps);
                string userID = "";
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("查无此账户！");
                    return;
                }
                else
                {
                    userID = ds.Tables[0].Rows[0]["UserID"].ToString();
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("ext_user_id", userID);
                    string requestStr = JsonConvert.SerializeObject(dic);
                    RequestModel<string> request = new RequestModel<string>();
                    request.Request = requestStr;
                    request.OperType = 0;
                    string json = JsonConvert.SerializeObject(request);
                    ResponseModelClone<string> result = HttpHelper.HttpPost(json, "http://ZQTMS.dekuncn.com:8012/WalletPayService/UserInfoQuery");
                    if (result.State == "200")
                    {
                        string respStr = result.Result;
                        if (!string.IsNullOrEmpty(respStr))
                        {
                            JObject joResp = JsonConvert.DeserializeObject<JObject>(respStr);
                            string data = "";
                            if (joResp.Property("data") != null)
                            {
                                data = joResp["data"].ToString();
                            }
                            if (data != "")
                            {
                                JObject joData = JsonConvert.DeserializeObject<JObject>(data);
                                string account_number = "";
                                if (joData.Property("account_number") != null)
                                {
                                    account_number = joData["account_number"].ToString();
                                }
                                string user_id_inter = "";
                                if (joData.Property("user_id") != null)
                                {
                                    user_id_inter = joData["user_id"].ToString();
                                }
                                string create_time = "";
                                if (joData.Property("create_time") != null)
                                {
                                    create_time = joData["create_time"].ToString();
                                }
                                string name = "";
                                if (joData.Property("name") != null)
                                {
                                    name = joData["name"].ToString();
                                }
                                txtQAccount_number.Text = account_number;
                                txtQUser_id.Text = user_id_inter;
                                txtQCreate_time.Text = create_time;
                                txtQName.Text = name;
                                txtQstuts.Text = "成功";
                            }
                        }

                    }//
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void btnAlterPwd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string id_no = txtID.Text.Trim();
            string company = txtCompany.Text.Trim();
            if (name == "")
            {
                MsgBox.ShowOK("姓名/法人不能为空!");
                return;
            }
            if (phone == "")
            {
                //MsgBox.ShowOK("电话号码不能为空!");
                //return;
            }
            if (id_no == "")
            {
                MsgBox.ShowOK("证件号不能为空!");
                return;
            }
            if (company == "")
            {
                MsgBox.ShowOK("公司名不能为空!");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("LegalPerson", name));
                list.Add(new SqlPara("mobile", phone));
                list.Add(new SqlPara("id_no", id_no));
                list.Add(new SqlPara("enterprise_name", company));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WalletAccountInfo_byPhone", list);
                DataSet ds = SqlHelper.GetDataSet_ZQTMS(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("账户不存在,无法修改密码!");
                    return;
                }
                else
                {
                    string pwd = ds.Tables[0].Rows[0]["Pwd"].ToString();
                    string newPwd = "";
                    FrmWalletAlterPwd wap = new FrmWalletAlterPwd();
                    wap.PWD = pwd;
                    if (wap.ShowDialog() == DialogResult.Yes)
                    {
                        newPwd = wap.NewPwd;
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("LegalPerson", name));
                        list1.Add(new SqlPara("mobile", phone));
                        list1.Add(new SqlPara("id_no", id_no));
                        list1.Add(new SqlPara("Pwd", StringHelper.Md5Hash(newPwd)));
                        list1.Add(new SqlPara("enterprise_name", company));
                        SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "USP_WalletAccountInfo", list1);
                        int i = SqlHelper.ExecteNonQuery_ZQTMS(sps1);
                        if (i > 0)
                        {
                            MsgBox.ShowOK();
                        }
                    }

                    

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }


    }
}

