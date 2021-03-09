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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmWebContrast : BaseForm
    {
        public frmWebContrast()
        {
            InitializeComponent();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //新增
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmWebConrastAdd add = new frmWebConrastAdd();
            add.ShowDialog();
        }

        //修改
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle=myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            string id = myGridView1.GetRowCellValue(rowHandle, "id").ToString();
            frmWebConrastAdd add = new frmWebConrastAdd();
            add.operType = 1;
            add.id = id;
            add.ShowDialog();
        }

        private void frmWebContrast_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            CommonClass.SetWeb(WebName, true);
        }

        //提取
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            try
            {

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WebContrast_byCondition",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count <= 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //删除
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) return;
            string id = myGridView1.GetRowCellValue(rowHandle, "id").ToString();
            try
            {
                if (MsgBox.ShowYesNo("确定删除数据？") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID",id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DEL_WebContrast_byID", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("删除成功！");
                    myGridView1.DeleteRow(rowHandle);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            
        }
        //导出
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "网点对照信息");
        }

       
    }
}
