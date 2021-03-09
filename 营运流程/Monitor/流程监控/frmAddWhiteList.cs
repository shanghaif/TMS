using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace ZQTMS.UI.流程监控
{
    public partial class frmAddWhiteList : ZQTMS.Tool.BaseForm
    {
        public frmAddWhiteList()
        {
            InitializeComponent();
        }
        public string AccountName;
        public string AccountNO;
        private void frmAddWhiteList_Load(object sender, EventArgs e)
        {
            this.textBox2.Text = AccountName;
            this.textBox1.Text = AccountNO;
        }


    
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox3.Text == "")
            {
                MsgBox.ShowError("开户人不能为空");
                return;
            }
            if (this.textBox4.Text == "")
            {
                MsgBox.ShowError("卡号不能为空");
                return;
            }
            string url = "http://lms.dekuncn.com:8079/UnionTransferWhiteList";
            //string url = "http://127.0.0.1:8080/ZQTMSUnionPay/UnionTransferWhiteList";
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("rcompanycode", "LMS" + this.textBox1.Text.Trim()));
            list.Add(new SqlPara("rcompanyname", this.textBox2.Text.Trim()));
            list.Add(new SqlPara("raccountname", this.textBox3.Text.Trim()));
            list.Add(new SqlPara("rbankaccount", this.textBox4.Text.Trim()));
            list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
            list.Add(new SqlPara("perflag", 0));
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("rcompanycode", "LMS" + this.textBox1.Text.Trim());
            dic.Add("rcompanyname", this.textBox2.Text.Trim());
            dic.Add("raccountname", this.textBox3.Text.Trim());
            dic.Add("rbankaccount", this.textBox4.Text.Trim());
            ////报文体json
            //JavaScriptSerializer jsonSer = new JavaScriptSerializer();//把这个换成jsonconvert是dll引用会报错，版本冲突gy
            //string data = jsonSer.Serialize(dic);
            string data = JsonConvert.SerializeObject(dic);
            string responseString = HttpHelper.HttpPost(url, System.Web.HttpUtility.UrlEncode(data, System.Text.Encoding.UTF8), Encoding.UTF8);
            JObject jo = (JObject)JsonConvert.DeserializeObject(responseString);
            if (jo["response"].ToString().Equals("0000"))
            {
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPD_TransferWhite", list));
                this.Close();
                MsgBox.ShowOK("添加成功！");
            }
            else
            {
                MsgBox.ShowOK(jo["message"].ToString());
            }
            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        


        }
    }

