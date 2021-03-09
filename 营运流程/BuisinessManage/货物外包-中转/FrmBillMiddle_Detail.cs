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
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class FrmBillMiddle_Detail : BaseForm
    {
        int gettype;
        /// <summary>
        /// 中转类型
        /// </summary>
        public int Gettype
        {
            get { return gettype; }
            set { gettype = value; }
        }

        static FrmBillMiddle_Detail fbm, fbm2;

        public FrmBillMiddle_Detail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取对象
        /// <param name="type">中转类型</param>
        /// </summary>
        public static FrmBillMiddle_Detail Get_FrmBillMiddle_Detail(int type)
        {
            if ((fbm == null || fbm.IsDisposed) && type == 0)
            {
                fbm = new FrmBillMiddle_Detail();
                fbm.gettype = type;
                return fbm;
            }
            if ((fbm2 == null || fbm2.IsDisposed) && type == 1)
            {
                fbm2 = new FrmBillMiddle_Detail();
                fbm2.gettype = type;
                return fbm2;
            }
            return type == 0 ? fbm : fbm2;
        }

        public void FrmBillMiddle_Detail_load(object sender, EventArgs e)
        {
            this.Text = gettype == 0 ? "中转分流-本地中转" : "中转分流-终端中转";

            CommonClass.FormSet(this, false);
            CommonClass.GetGridViewColumns(myGridView3, false);
            GridOper.SetGridViewProperty(myGridView3);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView3);

            FixColumn fix = new FixColumn(myGridView3, barSubItem4);
            barButtonItem20.PerformClick();
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDate", FormatConditionEnum.Equal, 0, Color.FromArgb(255, 255, 255));//颜色固定--白色
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDate", FormatConditionEnum.Equal, 1, Color.FromArgb(193, 255, 193));//颜色固定--绿色
            GridOper.CreateStyleFormatCondition(myGridView3, "LckDate", FormatConditionEnum.Greater, 1, Color.LightBlue);//颜色固定--浅蓝色
            //plh20191029
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3.Guid.ToString());
        }

        private void barCheckItem4_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "中转库存");
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView3.FocusedRowHandle < 0) return;
            try
            {
                frmBillMiddleInfo fbm = new frmBillMiddleInfo();
                fbm.Gettype = gettype;
                fbm.Gv = this.myGridView3;
                fbm.BillNo = ConvertType.ToString(myGridView3.GetFocusedRowCellValue("BillNo"));
                fbm.Freight = ConvertType.ToDecimal(myGridView3.GetFocusedRowCellValue("ActualFreight"));
                fbm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            }
        }

        private void barButtonItem25_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBillBatchMiddle fbbm = frmBillBatchMiddle.Get_frmBillBatchMiddle(gettype);
            fbbm.MdiParent = this.MdiParent;
            fbbm.Dock = DockStyle.Fill;
            fbbm.gv = myGridView3;
            fbbm.Show();
            fbbm.Focus();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridControl3.DataSource = null;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("siteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("gettype", gettype));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MIDDLE_LOAD_Agent", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView3.RowCount < 1000) myGridView3.BestFitColumns();
            }
        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            barButtonItem21.PerformClick();
        }
    }
}
