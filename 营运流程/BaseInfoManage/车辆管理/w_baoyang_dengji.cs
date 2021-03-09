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
    public partial class w_baoyang_dengji : BaseForm
    {
        public w_baoyang_dengji()
        {
            InitializeComponent();
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_bao_dengji bao = new w_bao_dengji();
            bao.ShowDialog();
        }

        private void w_baoyang_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("保养记录");//xj/2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(gridView1, barSubItem2);
            GridOper.RestoreGridLayout(gridView1,"车辆保养记录");
            this.bdate.EditValue = DateTime.Now;
            this.edate.EditValue = DateTime.Now;
            bindvehicleno();
            bindbyproject();

        }

        private void cbretrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            string vehicleno = this.vehicleno.Text.Trim();
            string byproject = this.baoyangproject.Text.Trim();
            if (this.vehicleno.Text.Trim() == "" || this.vehicleno.Text.Trim() == "全部") vehicleno = "%%"; else vehicleno = "%" + this.vehicleno.Text.Trim() + "%";
            if (this.baoyangproject.Text.Trim() == "" || this.baoyangproject.Text.Trim() =="全部") byproject = "%%"; else byproject = "%" + this.baoyangproject.Text.Trim() + "%";
            try
            {
                this.gridControl1.DataSource = null;    //zhengjiafeng20191006
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", this.bdate.DateTime.Date));
                list.Add(new SqlPara("edate", this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
                list.Add(new SqlPara("vehicleno", vehicleno));
                list.Add(new SqlPara("byproject", byproject));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_BY_LIST", list);
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
        private void bindvehicleno()
        {

            this.vehicleno.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.vehicleno.Properties.Items.Add(ds.Tables[0].Rows[j]["vehicleno"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            this.vehicleno.Properties.Items.Add("全部");
        }

        private void bindbyproject()
        {
            this.baoyangproject.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_BY_PROJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.baoyangproject.Properties.Items.Add(ds.Tables[0].Rows[j]["projectname"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            this.baoyangproject.Properties.Items.Add("全部");
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
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string unit = this.gridView1.GetRowCellValue(no, "byunit").ToString();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("state", state));
                list.Add(new SqlPara("byunit", unit));
                list.Add(new SqlPara("date", DateTime.Now));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
              

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_BY_STATE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
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
            string unit = this.gridView1.GetRowCellValue(no, "byunit").ToString();
            w_update_by_dj bao = new w_update_by_dj(unit);
            bao.ShowDialog();
            getdata();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "车辆保养记录");
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
           // cc.GenSeq(e);
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
            GridOper.SaveGridLayout(gridView1,"车辆保养记录");
        }

        private void w_baoyang_dengji_FormClosing(object sender, FormClosingEventArgs e)
        {
            //cc.LockGridLayout(this.gridControl1, "保养记录");
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