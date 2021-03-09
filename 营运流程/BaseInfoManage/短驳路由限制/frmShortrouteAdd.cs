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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmShortrouteAdd : BaseForm
    {
        public string RouteId = "", webname = "", sitename = "", centername = "", bsitename = "";
        
        public frmShortrouteAdd()
        {
            //  初始化窗口控件
            InitializeComponent();
        }

        public frmShortrouteAdd(string bsitename, string webname, string sitename, string centername, string RouteId) 
        {
            InitializeComponent();
            this.RouteId = RouteId;
            this.bsitename = bsitename;
            this.webname = webname;
            this.sitename = sitename;
            this.centername = centername;
        }

        private void frmShortrouteAdd_Load(object sender, EventArgs e)
        {
            comboBoxEdit1.Text = webname;
            comboBoxEdit2.Text = centername;
            comboBoxEdit3.Text = bsitename;
            comboBoxEdit4.Text = sitename;
            //  加载站点
            CommonClass.SetSite(comboBoxEdit3, false);
            CommonClass.SetSite(comboBoxEdit4, false);
        }

        //  根据站点加载网点
        private void comboBoxEdit3_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(comboBoxEdit1, comboBoxEdit3.Text.Trim(), false);
            CommonClass.SetWeb(comboBoxEdit2, comboBoxEdit3.Text.Trim(), false);
        }

        //  关闭
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //  保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string bsitename = comboBoxEdit3.Text.Trim();
                string webname = comboBoxEdit1.Text.Trim();
                string sitename = comboBoxEdit4.Text.Trim();
                string centername = comboBoxEdit2.Text.Trim();

                if (bsitename.ToString().Contains(sitename)) 
                {
                    MsgBox.ShowOK("始发站点与到站不能相同");
                    return; 
                }
                if (webname.ToString().Contains(centername)) 
                {
                    MsgBox.ShowOK("始发网点与分拨中心不能相同"); 
                    return; 
                }

                if (string.IsNullOrEmpty(bsitename) || string.IsNullOrEmpty(webname) || string.IsNullOrEmpty(sitename) || string.IsNullOrEmpty(centername))
                {
                    MsgBox.ShowOK("数据不能为空");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsitename", bsitename));
                list.Add(new SqlPara("webname", webname));
                list.Add(new SqlPara("sitename", sitename));
                list.Add(new SqlPara("centername", centername));
                list.Add(new SqlPara("RouteId", RouteId == "" ? Guid.NewGuid().ToString() : RouteId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SHORTROUTE", list);
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

    }
}