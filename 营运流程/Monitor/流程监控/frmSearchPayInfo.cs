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
    public partial class frmSearchPayInfo : BaseForm
    {
        public frmSearchPayInfo()
        {
            InitializeComponent();
        }

        private void frmSearchPayInfo_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("在线充值信息");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetCause(txtCauseName,true);
            CommonClass.SetArea(txtAreaName, txtCauseName.Text);
            CommonClass.SetWeb(txtWebName,txtAreaName.Text);

            indateState.DateTime = Convert.ToDateTime(CommonClass.gcdate.AddDays(-7).ToShortDateString());
            indateEnd.DateTime = CommonClass.gedate;

            txtCauseName.Text = CommonClass.UserInfo.CauseName;
            txtAreaName.Text = CommonClass.UserInfo.AreaName;
            txtWebName.Text = CommonClass.UserInfo.WebName;
        }

        private void txtCauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(txtAreaName, txtCauseName.Text.Trim(), true);
        }

        private void txtAreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(txtWebName, txtCauseName.Text.Trim(), txtAreaName.Text.Trim());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CauseName", txtCauseName.Text.Trim() == "全部" ? "%%" : txtCauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", txtAreaName.Text.Trim() == "全部" ? "%%" : txtAreaName.Text.Trim()));
                list.Add(new SqlPara("webName", txtWebName.Text.Trim() == "全部" ? "%%" : txtWebName.Text.Trim()));
                list.Add(new SqlPara("indateState", indateState.DateTime));
                list.Add(new SqlPara("indateEnd", indateEnd.DateTime));
                list.Add(new SqlPara("TopupState", TopupState.Text.Trim() == "全部" ? "%%" : TopupState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OnLinePayInfo", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string paystate = ds.Tables[0].Rows[i]["paystatus"].ToString();
                    if (paystate == "1" || paystate == "2")
                    {
                        GridOper.CreateStyleFormatCondition(myGridView1, "paystatus", DevExpress.XtraGrid.FormatConditionEnum.Equal, paystate, Color.GreenYellow);// 绿色
                    }
                }
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1); 
        }
    }
}