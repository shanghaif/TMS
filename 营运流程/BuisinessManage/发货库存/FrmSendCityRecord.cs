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
    public partial class FrmSendCityRecord : BaseForm
    {
        public FrmSendCityRecord()
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
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SENDCITY_RECORD", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            //barButtonItem8.Enabled = UserRight.GetRight("121486");//取消调拨

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "");
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
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string BillNoStr = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    BillNoStr += GridOper.GetRowCellValueString(myGridView1, i, "BillNo") + "@";
            }

            if (string.IsNullOrEmpty(BillNoStr)) return;
            if (MsgBox.ShowYesNo("确定打印选中单？") != DialogResult.Yes) return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BILLINFO_SendCity_PRINT_QSD", new List<SqlPara> { new SqlPara("BillNoStr", BillNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowError("没有找到选中的运单信息,打印失败,(请检查网络或运单是否已被删除)!");
                return;
            }
            //frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            //jl20181127
            if (CommonClass.UserInfo.WebName == "上海青浦操作部"
                || CommonClass.UserInfo.WebName == "上海青浦操作部1"
                || CommonClass.UserInfo.WebName == "杭州操作部"
                || CommonClass.UserInfo.WebName == "杭州操作部1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "宁波操作部"
                || CommonClass.UserInfo.WebName == "宁波操作部1"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心"
                || CommonClass.UserInfo.WebName == "济南二级分拨中心1"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心"
                || CommonClass.UserInfo.WebName == "无锡二级分拨中心1"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心"
                || CommonClass.UserInfo.WebName == "武汉二级分拨中心1"
                || CommonClass.UserInfo.WebName == "广州操作部"
                || CommonClass.UserInfo.WebName == "广州操作部1"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心"
                || CommonClass.UserInfo.WebName == "东莞大坪分拨中心1"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心"
                || CommonClass.UserInfo.WebName == "青岛二级分拨中心1")
            {
                frmRuiLangService.Print("提货单大坪", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
            else
            {
                frmRuiLangService.Print("提货单", ds.Tables[0], CommonClass.UserInfo.gsqc);
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;

            string BillNo = GridOper.GetRowCellValueString(myGridView1, rowhandle, "BillNo");
            if (BillNo == "") return;

            if (MsgBox.ShowYesNo("确定将选中运单取消调拨？") != DialogResult.Yes) return;

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_CANCEL_SENDTOCITY", new List<SqlPara> { new SqlPara("BillNo", BillNo) })) == 0) return;
            myGridView1.DeleteRow(rowhandle);
            MsgBox.ShowOK("取消调拨成功!");
        }
    }
}