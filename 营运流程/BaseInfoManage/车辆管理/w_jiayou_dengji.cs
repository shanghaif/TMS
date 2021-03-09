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
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_jiayou_dengji : BaseForm
    {
        public w_jiayou_dengji()
        {
            InitializeComponent();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //w_you_dengji you = new w_you_dengji();
            //you.ShowDialog();
        }
        private void w_jiayou_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(gridView1,this.Text);
            this.bdate.EditValue = DateTime.Now;
            this.edate.EditValue = DateTime.Now;
            bindcomboboxedit();
        }

        //绑定数据到车号，加油站列表框
        private void bindcomboboxedit()
        {
            this.vehicleno.Properties.Items.Clear();
            this.youzhan.Properties.Items.Clear();
            this.youka.Properties.Items.Clear();
            List<SqlPara> list1 = new List<SqlPara>();
            list1.Add(new SqlPara("gytype", "加油站"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list1);
            SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "V_GET_OIL");
            DataTable dt1 = SqlHelper.GetDataSet(sps).Tables[0];
            DataTable dt2 = SqlHelper.GetDataSet(sps1).Tables[0];
            DataTable dt3 = SqlHelper.GetDataSet(sps2).Tables[0];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(dt1.Rows[i]["vehicleno"]);
            }
            this.vehicleno.Properties.Items.Add("全部");
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                this.youzhan.Properties.Items.Add(dt2.Rows[j]["gyname"]);
            }
            this.youzhan.Properties.Items.Add("全部");
            for (int k = 0; k < dt3.Rows.Count; k++)
            {
                this.youka.Properties.Items.Add(dt3.Rows[k]["serialnumber"]);
            }
            this.youka.Properties.Items.Add("全部");
        }

         private void cbretrieve_Click(object sender, EventArgs e)
        {
            selectlist();
        }

        private void selectlist()
        {
            string vehicle = (vehicleno.Text.Trim() == "" || vehicleno.Text.Trim() == "全部") ? "%%" : vehicleno.Text.Trim();
            string yk = (youka.Text.Trim() == "" || youka.Text.Trim() == "全部") ? "%%" : youka.Text.Trim();
            string yz = (youzhan.Text.Trim() == "" || youzhan.Text.Trim() == "全部") ? "%%" : youzhan.Text.Trim();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", this.bdate.DateTime.Date));
                list.Add(new SqlPara("edate", this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
                list.Add(new SqlPara("vehicleno", vehicle));
                list.Add(new SqlPara("serialnumber", yk));
                list.Add(new SqlPara("gyname", yz));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_JY_LIST", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.gridControl1.DataSource = ds.Tables[0];
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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
            if (XtraMessageBox.Show("确定要作废吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                updatestate(2);
            }
        }

        private void updatestate(int state)
        {
            if (this.gridView1.SelectedRowsCount == 0) return;
            int rowhandle = this.gridView1.FocusedRowHandle;
            string unit = this.gridView1.GetRowCellValue(rowhandle, "jyunit").ToString();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("state", state));
                list.Add(new SqlPara("jyunit", unit));
                list.Add(new SqlPara("date", DateTime.Now));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_JY_STATE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    selectlist();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            int rowhandle = this.gridView1.FocusedRowHandle;
            string state = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "state").ToString();
            if (state == "2")
            {
                XtraMessageBox.Show("该条记录已被作废不能再修改", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string unit = this.gridView1.GetRowCellValue(rowhandle, "jyunit").ToString();
            w_update_jy_dj update = new w_update_jy_dj(unit);
            update.ShowDialog();
            selectlist();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1,this.Text);
        }

        private void cbretrieve_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }
    }
}