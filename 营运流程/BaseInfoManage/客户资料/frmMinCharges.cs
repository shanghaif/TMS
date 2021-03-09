using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
namespace ZQTMS.UI.BaseInfoManage.客户资料
{
    public partial class frmMinCharges : ZQTMS.Tool.BaseForm
    {
        GridColumn gcIsseleckedMode;

        public frmMinCharges()
        {
            InitializeComponent();
        }

        public void Getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MinCostChargeList", list);
            myGridControl1.DataSource = SqlHelper.GetDataTable(sps);


        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Getdata();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMinChargeAdd cost = new frmMinChargeAdd();
            cost.ShowDialog();
        }

        private void frmMinCharges_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("最低收费管控");//xj/2019/5/28
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView2);
            GridOper.SetGridViewProperty(myGridView2);
            BarMagagerOper.SetBarPropertity(bar9); //如果有具体的工具条，就引用其实例
            Getdata();
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView2, "ischecked");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView2.FocusedRowHandle;
                if (rowhandle < 0) return;
                string ControlId = myGridView2.GetRowCellValue(rowhandle, "ID").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ControlId));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_MinCostChargeList_ByID", list);//根据id获取其他字段
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                frmMinChargeAdd frm = new frmMinChargeAdd();
                frm.dr = dr;
                frm.ShowDialog();


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView2.RowCount == 0) return;

                myGridView2.PostEditor();

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string Ids = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        Ids += myGridView2.GetRowCellValue(i, "ID") + "@";//单号
                    }
                }
                if (Ids == "") return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ControlId", Ids));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_MinCostChargeList", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    Getdata();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2);
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmMinCostChargeUp frm = new frmMinCostChargeUp();
            frm.ShowDialog();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView2);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                myGridView2.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }
    }
}
