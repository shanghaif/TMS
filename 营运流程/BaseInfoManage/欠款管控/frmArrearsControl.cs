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

namespace ZQTMS.UI.BaseInfoManage.欠款管控
{
    public partial class frmArrearsControl : ZQTMS.Tool.BaseForm
    {
        public frmArrearsControl()
        {
            InitializeComponent();
        }
        GridColumn getcheckcolumns;
        private void freshData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ArrearsControlAllStateList", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void frmArrearsControl_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            getcheckcolumns = GridOper.GetGridViewColumn(myGridView1, "ischecked");
            freshData();
          
            

        }

        private void btn_add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddfrmArrearsControll frm = new AddfrmArrearsControll();
            frm.ShowDialog();
        }
        /// <summary>
        /// 编辑欠款管控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_update_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                string ArrearsControlId = myGridView1.GetRowCellValue(rowhandle, "ArrearsControlId").ToString();
                AddfrmArrearsControll frm = new AddfrmArrearsControll();
                frm.ArrearsControlId = ArrearsControlId;
                frm.ShowDialog();
                freshData();

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btn_refersh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();

        }

        private void btn_delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView1.RowCount == 0) return;

                myGridView1.PostEditor();

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                string BusinessDepartGuidancePriceId = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        BusinessDepartGuidancePriceId += myGridView1.GetRowCellValue(i, "ArrearsControlId") + "@";
                    }
                }
                if (BusinessDepartGuidancePriceId == "") { MsgBox.ShowError("请勾选需要删除的相关信息"); return; }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ArrearsControlId", BusinessDepartGuidancePriceId));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_ArrearsControlById", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        /// <summary>
        /// 导出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            GridOper.ExportToExcel(myGridView1);
        }

        /// <summary>
        /// 退出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }




       
    }
}
