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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmShortroute : BaseForm
    {
        public frmShortroute()
        {
            InitializeComponent(); 
        }

        private void frmShortroute_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            dateEdit1.DateTime = CommonClass.gbdate.AddDays(-15);
            dateEdit2.DateTime = CommonClass.gedate;
            //  加载站点
            CommonClass.SetSite(comboBoxEdit2, true);
        }
        //  提取数据
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bsitename", comboBoxEdit2.Text.Trim()=="全部"?"%%":comboBoxEdit2.Text.Trim()));
            list.Add(new SqlPara("webname", comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim()));
            list.Add(new SqlPara("centername", comboBoxEdit3.Text.Trim() == "全部" ? "%%" : comboBoxEdit3.Text.Trim()));
            list.Add(new SqlPara("bdate", dateEdit1.DateTime));
            list.Add(new SqlPara("edate", dateEdit2.DateTime));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SHORTROUTE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return;
            myGridControl1.DataSource = ds.Tables[0];

        }
        //  新增
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmShortrouteAdd fsra = new frmShortrouteAdd();
            fsra.Show();
        }
        //  修改
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) 
            {
                MsgBox.ShowOK("未选中行");
                return;
            }
            string RouteId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "RouteId").ToString();
            string webname = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "webname").ToString();
            string sitename = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "sitename").ToString();
            string centername = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "centername").ToString();
            string bsitename = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "bsitename").ToString();
            frmShortrouteAdd fsra = new frmShortrouteAdd();

            fsra.RouteId = RouteId;
            fsra.webname = webname;
            fsra.sitename = sitename;
            fsra.centername = centername;
            fsra.bsitename = bsitename;

            fsra.Show();

        }
        //  删除
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0)
            {
                MsgBox.ShowOK("未选中行");
                return;
            }
            try
            {
                string RouteId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "RouteId").ToString();
                if (MsgBox.ShowYesNo("确认要删除选中信息吗？") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RouteId", RouteId));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_SHORTROUTE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
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

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(comboBoxEdit1, comboBoxEdit2.Text.Trim(), true);
            CommonClass.SetWeb(comboBoxEdit3, comboBoxEdit2.Text.Trim(), true);
        }

    }
}