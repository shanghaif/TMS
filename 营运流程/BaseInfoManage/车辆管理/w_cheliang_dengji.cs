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
    public partial class w_cheliang_dengji : BaseForm
    {
        public w_cheliang_dengji()
        {
            InitializeComponent();
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_feiyong_dengji feiyong = new w_feiyong_dengji();
            feiyong.ShowDialog();
        }

        private void w_cheliang_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("固定费用");//xj/2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(gridView1,this.Text);
            this.bdate.EditValue = DateTime.Now;
            this.edate.EditValue = DateTime.Now;
            bindvehicleno();
            bindproject();
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

        private void bindproject()
        {
            this.feiyongproject.Properties.Items.Clear();
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY_PROJECT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    this.feiyongproject.Properties.Items.Add(ds.Tables[0].Rows[j]["projectname"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            this.feiyongproject.Properties.Items.Add("全部");
        }

        private void getdata()
        {
           
            string vehicle = (this.vehicleno.Text.Trim() == "" || this.vehicleno.Text.Trim() == "全部") ? "%%" : this.vehicleno.Text.Trim();
            string project = (this.feiyongproject.Text.Trim() == "" || this.feiyongproject.Text.Trim() == "全部") ? "%%" : this.feiyongproject.Text.Trim();
             try
            {
                this.gridControl1.DataSource = null;    //zhengjiafeng20191006
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", this.bdate.DateTime.Date));
                list.Add(new SqlPara("edate", this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
                list.Add(new SqlPara("vehicleno", vehicle));
                list.Add(new SqlPara("fyproject", project));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_FY_LIST", list);
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
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string unit = this.gridView1.GetRowCellValue(no, "unit").ToString();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("state", state));
                list.Add(new SqlPara("unit", unit));
                list.Add(new SqlPara("date", DateTime.Now));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));


                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_FY_STATE", list);
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
            string unit = this.gridView1.GetRowCellValue(no, "unit").ToString();
            w_update_fy_dj update = new w_update_fy_dj(unit);
            update.ShowDialog();
            getdata();
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

        private void cbretrieve_Click(object sender, EventArgs e)
        {
            getdata();
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