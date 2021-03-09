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
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class FrmShortDetailStatusWeb : BaseForm
    {
        #region 界面方法
        public static void SetWeb(ComboBoxEdit cb, string SiteName, bool isall)
        {
            try
            {
                if (CommonClass.dsWeb == null || CommonClass.dsWeb.Tables.Count == 0) return;
                if (SiteName == "全部") SiteName = "%%";
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName like '" + SiteName + "' and WebRole='加盟' and belongcause<>'测试事业部'");
                cb.Properties.Items.Clear();
                cb.Text = "";
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["WebName"]);
                }
                if (isall)
                {
                    cb.Properties.Items.Add("全部");
                    cb.Text = "全部";
                }
            }
            catch (Exception)
            {
                MsgBox.ShowOK("正在加载基础资料，请稍等！");
            }
        }
        #endregion

        public FrmShortDetailStatusWeb()
        {
            InitializeComponent();
        }

        private void FrmShortDetailStatusWeb_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView3,true);
            GridOper.SetGridViewProperty(myGridView3);
            GridOper.RestoreGridLayout(myGridView3);
            BarMagagerOper.SetBarPropertity(bar8); //如果有具体的工具条，就引用其实例
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            SetWeb(BegWeb, "全部", true);
        }

        //提取数据
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                //QSP_GET_BillStatusCount QSP_GET_BillStatusWeb

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ShortDetailStatusWeb", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //导出明细
        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3);
        }
        ////退出
        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //自动筛选
        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }
        //锁定外观
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3);
        }
        //取消锁定
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3.Guid.ToString());
        }
        //过滤器
        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        
    }
}
