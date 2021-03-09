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
    public partial class JMfrmSendCostInfo : BaseForm
    {
        public JMfrmSendCostInfo()
        {
            InitializeComponent();
        }

        private void JMfrmSendCostInfo_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("加盟商送货成本表");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetSite(txtSiteName,true);
            CommonClass.SetWeb(txtWebName,true);
            txtSiteName.Text = CommonClass.UserInfo.SiteName;
            txtWebName.Text = CommonClass.UserInfo.WebName;
        }
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("t1",bdate.DateTime));
                list.Add(new SqlPara("t2",edate.DateTime));
                list.Add(new SqlPara("siteName",txtSiteName.Text.Trim()=="全部"?"%%":txtSiteName.Text.Trim()));
                list.Add(new SqlPara("webName", txtWebName.Text.Trim() == "全部" ? "%%" : txtWebName.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "JMQSP_FETCH_SendCost_Info",list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }
        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1,"加盟商送货成本表");
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(txtWebName,txtSiteName.Text.Trim(),true);
        }

        
    }
}