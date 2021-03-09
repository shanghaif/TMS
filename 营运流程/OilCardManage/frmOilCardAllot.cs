using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmOilCardAllot : BaseForm
    {
        public frmOilCardAllot()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OILCARD_ALLOT_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 2000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("分配记录");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "油卡记录");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmOilCardAllotAdd foa = frmOilCardAllotAdd.Get_frmOilCardAllotAdd;
            foa.Show();
            foa.TopMost = true;
            foa.TopMost = false;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("只能修改分配的司机信息,是否继续？") != DialogResult.Yes) return;

            frmOilCardAllotModify foa = new frmOilCardAllotModify();
            foa.Gv = myGridView1;
            foa.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "VerifyState")) == 1)
            {
                MsgBox.ShowOK("此记录已审核,不能再审核!");
                return;
            }
            if (MsgBox.ShowYesNo("确定审核？") != DialogResult.Yes) return;

            //Type:1审核;2反审核
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_VERIFY_STATE", new List<SqlPara> { new SqlPara("id", ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "id"))), new SqlPara("Type", 1) })) == 0) return;

            myGridView1.SetRowCellValue(rowhandle, "VerifyState", 1);
            myGridView1.SetRowCellValue(rowhandle, "VerifyMan", CommonClass.UserInfo.UserName);
            myGridView1.SetRowCellValue(rowhandle, "VerifyDate", CommonClass.gcdate);
            MsgBox.ShowOK("审核成功!");
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            if (ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "VerifyState")) == 0)
            {
                MsgBox.ShowOK("此记录未审核,不能反审核!");
                return;
            }
            if (MsgBox.ShowYesNo("确定反审核？") != DialogResult.Yes) return;

            //Type:1审核;2反审核
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_VERIFY_STATE", new List<SqlPara> { new SqlPara("id", ConvertType.ToInt32(myGridView1.GetRowCellValue(rowhandle, "id"))), new SqlPara("Type", 2) })) == 0) return;

            myGridView1.SetRowCellValue(rowhandle, "VerifyState", 0);
            myGridView1.SetRowCellValue(rowhandle, "VerifyMan", "");
            myGridView1.SetRowCellValue(rowhandle, "VerifyDate", DBNull.Value);
            MsgBox.ShowOK("反审核成功!");
        }
    }
}