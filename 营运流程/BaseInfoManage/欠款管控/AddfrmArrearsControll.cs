using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;

namespace ZQTMS.UI.BaseInfoManage.欠款管控
{
    public partial class AddfrmArrearsControll : ZQTMS.Tool.BaseForm
    {
        public DataRow dr = null;
        public AddfrmArrearsControll()
        {
            InitializeComponent();
        }

       
        private void btn_save_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrEmpty(SiteName.Text))
                {
                    MsgBox.ShowOK("隶属站点不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(WebName.Text))
                {

                    MsgBox.ShowOK("网点名称不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(WebResponsiblePerson.Text))
                {
                    MsgBox.ShowOK("网点负责人不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(ArrearsAmount.Text))
                {
                    MsgBox.ShowOK("欠款额度不能为空");
                    return;
                }
                if (string.IsNullOrEmpty(ArrearsControlDate.Text))
                {
                    MsgBox.ShowOK("欠款管控时间不能为空");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ArrearsControlId", ArrearsControlId));
                list.Add(new SqlPara("ArrearsAmount", ArrearsAmount.Text.ToString()));
                list.Add(new SqlPara("SiteName", SiteName.Text.ToString()));
                list.Add(new SqlPara("WebName", WebName.Text.ToString()));
                list.Add(new SqlPara("WebResponsiblePerson", WebResponsiblePerson.Text.Trim()));
                list.Add(new SqlPara("ArrearsControlDate", ArrearsControlDate.Text.ToString()));
                list.Add(new SqlPara("OpenState", OpenState.Checked ? 1 : 0));
                list.Add(new SqlPara("BefArrivalPayState", BefArrivalPayState.Checked ? 1 : 0));
                list.Add(new SqlPara("OwePayState", OwePayState.Checked ? 1 : 0));
                list.Add(new SqlPara("ReceiptPayState", ReceiptPayState.Checked ? 1 : 0));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_ArrearsControl", list);//保存
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string ArrearsControlId = "";
        private void AddfrmArrearsControll_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(SiteName, false);//加载站点
            CommonClass.SetWeb(WebName, SiteName.Text, false);//加载网点
            SiteName.Text = CommonClass.UserInfo.SiteName;
            WebName.Text = CommonClass.UserInfo.WebName;

            if (ArrearsControlId != "")
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ArrearsControlId", ArrearsControlId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ArrearsControlBYId", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                SiteName.Text = ds.Tables[0].Rows[0]["SiteName"].ToString();
                WebName.Text = ds.Tables[0].Rows[0]["WebName"].ToString();
                WebResponsiblePerson.Text = ds.Tables[0].Rows[0]["WebResponsiblePerson"].ToString();
                ArrearsAmount.Text = ds.Tables[0].Rows[0]["ArrearsAmount"].ToString();
                ArrearsControlDate.Text = ds.Tables[0].Rows[0]["ArrearsControlDate"].ToString();

                BefArrivalPayState.Checked = ds.Tables[0].Rows[0]["BefArrivalPayState"].ToString() == "1" ? true : false;
                OpenState.Checked = ds.Tables[0].Rows[0]["OpenState"].ToString() == "1" ? true : false;
                OwePayState.Checked = ds.Tables[0].Rows[0]["OwePayState"].ToString() == "1" ? true : false;
                ReceiptPayState.Checked = ds.Tables[0].Rows[0]["ReceiptPayState"].ToString() == "1" ? true : false;

            }




        }

        private void WebName_SelectedIndexChanged(object sender, EventArgs e)
        {

            List<SqlPara> listuser = new List<SqlPara>();
            listuser.Add(new SqlPara("WebName", WebName.EditValue.ToString()));
            SqlParasEntity spsuser = new SqlParasEntity(OperType.Query, "QSP_GET_FindWebManByWebNameByWebName", listuser);
            DataSet dsuser = SqlHelper.GetDataSet(spsuser);
            if (dsuser == null || dsuser.Tables.Count == 0) return;
            WebResponsiblePerson.Text = "";
            WebResponsiblePerson.Text = dsuser.Tables[0].Rows[0]["WebMan"].ToString();
        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, SiteName.EditValue.ToString(), false);
            WebResponsiblePerson.Text = "";
        }
    }
}
