using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmFindGoodsImage : ZQTMS.Tool.BaseForm
    {
        string id = "";
        static frmFindGoodsImage foca;

        /// <summary>
        /// 编号
        /// </summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public frmFindGoodsImage()
        {
            InitializeComponent();
        }

        public static frmFindGoodsImage Get_frmFindGoodsImage { get { if (foca == null || foca.IsDisposed) foca = new frmFindGoodsImage(); return foca; } }

        public void getdata()
        {
            if (id == "") return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FINDGOODSIMAGE_BY_ID", new List<SqlPara> { new SqlPara("Id", id) }));

            if (ds == null || ds.Tables.Count == 0) return;
            gridControl1.DataSource = ds.Tables[0];
        }

        private void frmFindGoodsImage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) getdata();
            if (e.KeyCode == Keys.Escape) this.Close();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            FileUpload.ShowLocalImg(gridView1, "FilePath");
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || e.Column.FieldName != "rowid") return;
            e.Value = e.RowHandle + 1;
        }

        private void frmFindGoodsImage_Load(object sender, EventArgs e)
        {

        }
    }
}