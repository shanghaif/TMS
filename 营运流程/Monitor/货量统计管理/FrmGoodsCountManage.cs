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
    public partial class FrmGoodsCountManage : BaseForm
    {
        public FrmGoodsCountManage()
        {
            InitializeComponent();
        }

        private void FrmGoodsCountManage_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("货量统计管理");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            CommonClass.SetWeb(cbbWebName, true);
            cbbWebName.Text = CommonClass.UserInfo.WebName;
            string webName = CommonClass.UserInfo.WebName;
            CommonClass.SetUser(cbbOperMan, webName, true);
            //FixColumn fix = new FixColumn(myGridView1, barSubItem1);

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate.AddDays(7);
            if (CommonClass.UserInfo.SiteName != "总部")
            {
                cbbWebName.Text = CommonClass.UserInfo.WebName;
                cbbWebName.Enabled = false;
            }
            else
            {
                cbbWebName.Enabled = true;
            }

        }

        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }
        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
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

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmGoodsCountInput frm = new FrmGoodsCountInput();
            frm.ShowDialog();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("searchCondition", txtSearchType.Text.Trim() == "" ? "%%" : txtSearchType.Text.Trim()));
                list.Add(new SqlPara("WebName", cbbWebName.Text.Trim() == "全部" ? "%%" : cbbWebName.Text.Trim()));
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("operMan", cbbOperMan.Text.Trim() == "全部" ? "%%" : cbbOperMan.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GoodsCountManage", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 按批次号删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelBybatch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmDeleteByBillNoOrBatchNo frm = new FrmDeleteByBillNoOrBatchNo();
            frm.flag = 1;
            frm.operWebName = cbbWebName.Text;
            frm.ShowDialog();
        }
        /// <summary>
        /// 按单号删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void DelByBillNo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmDeleteByBillNoOrBatchNo frm = new FrmDeleteByBillNoOrBatchNo();
            frm.flag = 2;
            frm.operWebName = cbbWebName.Text;
            frm.ShowDialog();
        }

        private void UptByBillNo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条信息！");
                    return;
                }
                string opertype = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "OperType").ToString();
                string billNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BillNo").ToString();
                string operMan = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "operman").ToString();
                string carLoding = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "CarLoding").ToString();
                string operWeb = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "operWebName").ToString();
                FrmUpdateGoodsInfoByBillNo frm = new FrmUpdateGoodsInfoByBillNo();
                frm.operType = opertype;
                frm.billNo = billNo;
                frm.operMan = operMan;
                frm.carLoding = carLoding;
                frm.operWeb = operWeb;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmGoodsManageLog frm = new FrmGoodsManageLog();
            frm.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmAddOperMan frm = new FrmAddOperMan();
            frm.ShowDialog();
        }

    }
}