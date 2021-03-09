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
    public partial class frmMiddleRouteAdd : BaseForm
    {
        public int flag = 0;
        public string UseSite = "", UseWeb = "", EndSite = "",routeId="";
        public frmMiddleRouteAdd()
        {
            InitializeComponent();
        }
        public frmMiddleRouteAdd(int flag, string useSite, string useWeb, string endSite)
        {
            this.flag = flag;
            this.UseSite = useSite;
            this.UseWeb = useWeb;
            this.EndSite = endSite;
        }
       
        private void frmMiddleRouteAdd_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(cbbUseSite,false);
            CommonClass.SetSite(cbbEndSite,false);
            cbbUseSite.Text = UseSite;
            cbbUseWeb.Text = UseWeb;
            cbbEndSite.Text = EndSite;
            if (flag == 2)
            {
                cbbUseSite.Enabled = false;
                cbbUseWeb.Enabled = false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbbUseSite.Text.Trim()))
                {
                    MsgBox.ShowOK("请输入使用站点！");
                    return;
                }
                if (string.IsNullOrEmpty(cbbUseWeb.Text.Trim()))
                {
                    MsgBox.ShowOK("请输入使用网点！");
                    return;
                }
                if (string.IsNullOrEmpty(cbbEndSite.Text.Trim()))
                {
                    MsgBox.ShowOK("请输入目的站点！");
                    return;
                }
                if (cbbEndSite.Text.Trim()==cbbUseSite.Text.Trim())
                {
                    MsgBox.ShowOK("【使用站点】和【目的站点】不能一致！");
                    return;
                }
                string routeID = "";
                if (flag == 1)
                {
                    routeID = Guid.NewGuid().ToString();
                }
                else
                {
                    routeID = routeId;
                }
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Id", routeID));
                list.Add(new SqlPara("useSite",cbbUseSite.Text.Trim()));
                list.Add(new SqlPara("useWeb", cbbUseWeb.Text.Trim()));
                list.Add(new SqlPara("endSite", cbbEndSite.Text.Trim()));
                list.Add(new SqlPara("flag",flag));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_MiddleSendRoute",list);
                if (SqlHelper.ExecteNonQuery(sps)>0)
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbbUseSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(cbbUseWeb,cbbUseSite.Text.Trim(),false);
        }
    }
}