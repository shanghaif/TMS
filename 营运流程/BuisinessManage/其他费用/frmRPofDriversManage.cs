using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmRPofDriversManage : BaseForm
    {
        DataSet ds = new DataSet();
        public frmRPofDriversManage()
        {
            InitializeComponent();
        }

        private void frmRPofDriversManage_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(Tosite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            bsite.EditValue = CommonClass.UserInfo.SiteName;
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("Tosite", Tosite.Text.Trim() == "全部" ? "%%" : Tosite.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RPofDriverList", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //退出
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        //导出
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "司机奖罚登记");
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bcSearch_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        //锁定外观
        private void BBLock_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        //取消外观
        private void BBCancelLock_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        //过滤器
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        //删除
        private void BBDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            try
            {

                if (MsgBox.ShowYesNo("确定删除本条费用？") != DialogResult.Yes) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RPofdriverID", myGridView1.GetRowCellValue(rowhandle, "RPofdriverID").ToString()));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_DELETE_RPofDriverList", list)) == 0) return;
                myGridView1.DeleteSelectedRows();
                MsgBox.ShowOK("删除成功!");
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void BBUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        // 导入
        private void cbLeadin_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmRPofDriverLead leading = new frmRPofDriverLead();
            leading.ShowDialog();
        }

       
        private void BBUpdate_ItemClick_1(object sender, ItemClickEventArgs e)
        {
           
        }

        private void frmRPofDriversManage_Load_1(object sender, EventArgs e)
        {
            CommonClass.InsertLog("司机奖罚登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetSite(bsite, true);
            CommonClass.SetSite(Tosite, true);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            bsite.EditValue = CommonClass.UserInfo.SiteName;
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

        }

        private void myGridControl1_Click(object sender, EventArgs e)
        {

        }

        //提取
        private void cbRetrieve_Click_1(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            getDate();
        }
        public void getDate()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("bsite", bsite.Text.Trim() == "全部" ? "%%" : bsite.Text.Trim()));
                list.Add(new SqlPara("Tosite", Tosite.Text.Trim() == "全部" ? "%%" : Tosite.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RPofDriverList", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //退出
        private void cbClose_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }

        //自动筛选
        private void bcSearch_CheckedChanged_1(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void cbUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }

        //修改
        private void cbUpdate_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "RPofdriverID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RPofdriverID", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RPofDriverList_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmRPofDriversUpdate frm = new frmRPofDriversUpdate();
                frm.dr = dr;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
           
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("RPofdriverID", myGridView1.GetFocusedRowCellValue("RPofdriverID")));
            list.Add(new SqlPara("DepartureBatch", myGridView1.GetFocusedRowCellValue("DepartureBatch")));
            list.Add(new SqlPara("type", "0"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RPofDriverList_UpdateState", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("取消成功！");

            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("RPofdriverID", myGridView1.GetFocusedRowCellValue("RPofdriverID")));
            list.Add(new SqlPara("DepartureBatch", myGridView1.GetFocusedRowCellValue("DepartureBatch")));
            list.Add(new SqlPara("type", "1"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_UPDATE_RPofDriverList_UpdateState", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("确认成功！");
            }
        }
    }
}
