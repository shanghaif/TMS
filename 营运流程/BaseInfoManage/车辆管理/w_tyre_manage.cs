using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
namespace ZQTMS.UI
{
    public partial class w_tyre_manage : BaseForm
    {
        public w_tyre_manage()
        {
            InitializeComponent();
        }
        private void w_tyre_manage_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("轮胎管理");//xj/2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(gridView1,this.Text);
            this.bdate.EditValue = DateTime.Now;
            this.edate.EditValue = DateTime.Now;
            bindcomboboxedit();
        }

        private void bindcomboboxedit()
        {
            this.vehicleno.Properties.Items.Clear();

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt1 = ds.Tables[0];
           
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt1.Rows[i]["vehicleno"]);
            }
        }


        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_tyre_dengji dengji = new w_tyre_dengji();
            dengji.ShowDialog();
        }

        private void cbretrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            string number = (this.tyrenumber.Text.Trim() == "") ? "%%" : "%" + this.tyrenumber.Text.Trim() + "%";
            string vehicle = (this.vehicleno.Text.Trim() == "") ? "%%" : "%" + this.vehicleno.Text.Trim() + "%";
            List<SqlPara> list2 = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_TYRE_LIST", list2);
            list2.Add(new SqlPara("bdate",this.bdate.DateTime.Date));
            list2.Add(new SqlPara("edate",this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
            list2.Add(new SqlPara("tyreno",number));
            list2.Add(new SqlPara("vehicleno", vehicle));
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.gridControl1.DataSource = dt;
        }
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            string state = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "state").ToString();
            if (state == "2")
            {
                XtraMessageBox.Show("该条记录已被作废", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (XtraMessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                updatestate(2);
            }
        }

        private void updatestate(int state)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string number = this.gridView1.GetRowCellValue(no, "tyreno").ToString();



            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("state", state));
            list.Add(new SqlPara("tyreno", number));
            list.Add(new SqlPara("date", DateTime.Now));
            list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_UPDATE_TYRE_STATE", list);
            int row = SqlHelper.ExecteNonQuery(sps);
            getdata();
        }
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            string state = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "state").ToString();
            if (state == "2")
            {
                XtraMessageBox.Show("该条记录已被作废不能再修改", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            update();
        }
        private void update()
        {
            int no = this.gridView1.FocusedRowHandle;
            string number = this.gridView1.GetRowCellValue(no, "tyreno").ToString();
            w_update_tyre update = new w_update_tyre(number);
            update.ShowDialog();
            getdata();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, this.Text);
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void chexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            string state = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "state").ToString();
            if (state == "2")
            {
                XtraMessageBox.Show("该条记录已被作废", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (state == "1")
            {
                XtraMessageBox.Show("该条记录已被审核", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            updatestate(1);
            MsgBox.ShowOK();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            string state = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "state").ToString();
            if (state == "2")
            {
                XtraMessageBox.Show("该条记录已被作废", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (state == "0")
            {
                XtraMessageBox.Show("该记录还没有审核", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            updatestate(3);
            MsgBox.ShowOK();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1,this.Text);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void cbretrieve_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}