using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_gongying_record : BaseForm
    {
        public w_gongying_record()
        {
            InitializeComponent();
        }


        private void w_gongying_record_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(gridView1, barSubItem2);
            GridOper.RestoreGridLayout(gridView1, this.Text);
            getdata();
        }

        private void getdata()
        {
            SqlParasEntity  sps  = new SqlParasEntity(OperType.Query,"QSP_GET_GONGYING");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if(ds != null)
            gridControl1.DataSource = ds.Tables[0];
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, this.Text);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1,this.Text);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_gongying_info wgi = new w_gongying_info();
            wgi.Show();
            wgi.TopMost = true;
            wgi.TopMost = false;
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            if (row < 0) return;

            if (XtraMessageBox.Show("确定删除？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("gyid", ConvertType.ToInt32(gridView1.GetRowCellValue(row, "gyid"))));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_GONGYING_BY_GYID", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                gridView1.DeleteRow(row);
                MsgBox.ShowOK();
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = gridView1.FocusedRowHandle;
            if (row < 0) return;
            w_gongying_info wgi = new w_gongying_info();
            wgi.sgyid = ConvertType.ToString(gridView1.GetRowCellValue(row, "gyid"));
            wgi.sgyname = ConvertType.ToString(gridView1.GetRowCellValue(row, "gyname"));
              wgi.sgytype =  ConvertType.ToString(gridView1.GetRowCellValue(row, "gyid"));
              wgi.sgongyingname = ConvertType.ToString(gridView1.GetRowCellValue(row, "gongyingname"));
              wgi.slinkman = ConvertType.ToString(gridView1.GetRowCellValue(row, "linkman"));
              wgi.slinkmantel = ConvertType.ToString(gridView1.GetRowCellValue(row, "linkmantel"));
              wgi.slinkmanaddr = ConvertType.ToString(gridView1.GetRowCellValue(row, "linkmanaddr"));
              wgi.sswdjno = ConvertType.ToString(gridView1.GetRowCellValue(row, "swdjno"));
              wgi.skhbank = ConvertType.ToString(gridView1.GetRowCellValue(row, "khbank"));
              wgi.sbankno = ConvertType.ToString(gridView1.GetRowCellValue(row, "bankno"));
              wgi.sremark = ConvertType.ToString(gridView1.GetRowCellValue(row, "remark"));
            wgi.Show();
            wgi.TopMost = true;
            wgi.TopMost = false;
        }
    }
}