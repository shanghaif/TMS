using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmInventory : BaseForm
    {
        public frmInventory()
        {
            InitializeComponent();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("盘点清仓");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            CommonClass.SetSite(txtSiteName, true);
            txtSiteName.Text = CommonClass.UserInfo.SiteName;
            txtWebName.Text = CommonClass.UserInfo.WebName;

            txtInventory.DateTime = CommonClass.gbdate;
            txtInventoryEnd.DateTime = CommonClass.gedate;
            invstatus.Text = "全部";

        }

        private void txtSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(txtWebName, txtSiteName.Text);
        }

        //自动筛选
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        //锁定外观
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }
        //取消外观
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }
        //过滤器
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //退出
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //关闭
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        //导出
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "盘点清仓");
        }
        //安排盘点
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //int rowHandle = myGridView1.FocusedRowHandle;
            //if (rowHandle<0)
            //{
            //    MsgBox.ShowOK("请选择一条盘点信息！");
            //    return;
            //}
            frmAddInventory fm = new frmAddInventory();
            fm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate",txtInventory.DateTime));
                list.Add(new SqlPara("edate",txtInventoryEnd.DateTime));
                list.Add(new SqlPara("siteName", txtSiteName.Text.Trim() == "全部" ? "%%" : txtSiteName.Text.Trim()));
                list.Add(new SqlPara("webName", txtWebName.Text.Trim() == "全部" ? "%%" : txtWebName.Text.Trim()));
                list.Add(new SqlPara("invstatus", invstatus.Text.Trim() == "全部" ? "%%" : invstatus.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_InventoryInfo",list));
                if (ds == null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowOK("没有可提取的盘点信息！");
                    return;
                }
                else
                {
                    myGridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //查看盘点明细
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle<0)
            {
                MsgBox.ShowOK("请选择一条盘点信息！");
                return;
            }
            string PDBatchNo = myGridView1.GetRowCellValue(rowHandle, "invBatchNo").ToString();
            frmInventoryDetail frm = new frmInventoryDetail();
            frm.PDBathStr = PDBatchNo;
            frm.Show();
        }
        string operType;//操作类型
        //审核
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle<0)
            {
                MsgBox.ShowOK("请选择一条盘点信息！");
                return;
            }
            operType = "审核";
            string batchNo = myGridView1.GetRowCellValue(rowhandle, "invBatchNo").ToString();
            if (MsgBox.ShowYesNo("确定要审核完成？") != DialogResult.Yes) return;
            UpdatePDState(operType, batchNo);
        }
        //完成
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条盘点信息！");
                return;
            }
            operType = "完成";
            string batchNo = myGridView1.GetRowCellValue(rowhandle, "invBatchNo").ToString();
            if (MsgBox.ShowYesNo("确定要完成盘点？") != DialogResult.Yes) return;
            UpdatePDState(operType, batchNo);
        }
        //更新盘点状态
        public void UpdatePDState(string operType, string batchNo)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("invBatchNo", batchNo));
                list.Add(new SqlPara("operType", operType));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_UPDATE_InventoryState", list)) == 0) return;
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条盘点信息！");
                    return;
                }
                string InvBatchNo = myGridView1.GetRowCellValue(rowHandle, "invBatchNo").ToString();
                if (MsgBox.ShowYesNo("确定要作废当前盘点信息？") != DialogResult.Yes) return;
                operType = "作废";
                UpdatePDState(operType, InvBatchNo);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}