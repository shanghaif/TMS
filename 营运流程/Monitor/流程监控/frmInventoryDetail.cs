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
    public partial class frmInventoryDetail : BaseForm
    {
        public string PDBathStr;

        public string PDBatchNo
        {
            get { return PDBathStr; }
            set { PDBathStr = value; }
        }

        public frmInventoryDetail()
        {
            InitializeComponent();
        }

        private void frmInventoryDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar2); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            GetInventoryDetail();
            GridOper.CreateStyleFormatCondition(myGridView1, "PDCompare", DevExpress.XtraGrid.FormatConditionEnum.Greater, 0, Color.LightGoldenrodYellow);
            GridOper.CreateStyleFormatCondition(myGridView1, "PDCompare", DevExpress.XtraGrid.FormatConditionEnum.Less, 0, Color.GreenYellow);
            GridOper.CreateStyleFormatCondition(myGridView1, "ExceptionState", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, null, Color.Red);

        }

        public void GetInventoryDetail()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("PDBatchNo", PDBatchNo));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_InventoryDetail", list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "盘点明细");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}