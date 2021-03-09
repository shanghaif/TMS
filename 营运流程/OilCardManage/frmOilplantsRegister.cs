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
    public partial class frmOilplantsRegister : BaseForm
    {
        public frmOilplantsRegister()
        {
            InitializeComponent();
        }

        private void frmOilplantsRegister_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("油料登记");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate.AddDays(7);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate",bdate.DateTime));
                list.Add(new SqlPara("edate",edate.DateTime));
                list.Add(new SqlPara("oiltype",txtoilType.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OilplantsRegister",list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
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
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "油料登记信息");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnRegister_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddOilPlants frm = new frmAddOilPlants();
            frm.flag = 1;
            frm.Show();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条要修改的数据！");
                return;
            }
            string Batchno = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Batchno").ToString();
            if (!string.IsNullOrEmpty(Batchno))
            {
                MsgBox.ShowOK("已关联配载记录不可修改！");
                return;
            }
            string _carNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "carNo").ToString();
            string _gasStation = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "gasStation").ToString();
            string _gasType = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "gasType").ToString();
            string _oilCardNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilCardNo").ToString();
            string _oilDate = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilDate").ToString();
            string _oilVolume = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilVolume").ToString();
            string _oilPrice = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilPrice").ToString();
            string _sumAccount = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "sumAccount").ToString();
            string _Mark = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Mark").ToString();
            string oilId = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilId").ToString();
            
            frmAddOilPlants frm = new frmAddOilPlants();
            frm.carNo = _carNo;
            frm.gasStation = _gasStation;
            frm.gasType = _gasType;
            frm.oilCardNo = _oilCardNo;
            frm.oilDate = _oilDate;
            frm.oilVolume = _oilVolume;
            frm.oilPrice = _oilPrice;
            frm.sumAccount = _sumAccount;
            frm.Mark = _Mark;
            frm.oilId = oilId;
            frm.Batchno = Batchno;
            frm.flag = 2;
            frm.Show();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.FocusedRowHandle<0)
                {
                    MsgBox.ShowOK("请选择你要删除的信息！");
                    return;
                }

                string Batchno = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Batchno").ToString();
                if (!string.IsNullOrEmpty(Batchno)) 
                {
                    MsgBox.ShowOK("已关联配载记录不可删除！");
                    return;
                }

                if (MsgBox.ShowYesNo("确定要删除这条信息？") != DialogResult.Yes) return;

                string id = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "oilId").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_OilPlantsById",list);

                if (SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}