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
    public partial class w_weixiu_dengji : BaseForm
    {
        public w_weixiu_dengji()
        {
            InitializeComponent();
        }
       
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void w_weixiu_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("车辆维护");//xj/2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar4);
            GridOper.RestoreGridLayout(gridView1,this.Text);

            this.bdate.EditValue = DateTime.Now;
            this.edate.EditValue = DateTime.Now;
            this.bindcomboboxedit();
        }

        private void cbretrieve_Click(object sender, EventArgs e)
        {
            selectlist();
        }

        private void selectlist()
        {
            
            string vehicle = (this.vehicleno.Text.Trim() == "" || this.vehicleno.Text.Trim() == "全部") ? "%%" : this.vehicleno.Text.Trim();
            string pj = (this.peijian.Text.Trim() == "" || this.peijian.Text.Trim() == "全部") ? "%%" : "%" + this.peijian.Text.Trim() + "%";
            string wxg = (this.repairman.Text.Trim() == "" || this.repairman.Text.Trim() == "全部") ? "%%" : this.repairman.Text.Trim() + "%";
            string wxc = (this.repairchang.Text.Trim() == "" || this.repairchang.Text.Trim() == "全部") ? "%%" : this.repairchang.Text.Trim();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("bdate", this.bdate.DateTime.Date));
             list.Add(new SqlPara("edate",this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));
             list.Add(new SqlPara("vehicleno",vehicle));
             list.Add(new SqlPara("wxpj",pj));
             list.Add(new SqlPara("wxman",wxg));
             list.Add(new SqlPara("gyname",wxc));
             SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_WX_LIST", list);
             DataSet ds = SqlHelper.GetDataSet(sps);
            this.gridControl1.DataSource = ds.Tables[0];
        }
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_xiu_dengji xiu = new w_xiu_dengji();
            xiu.ShowDialog();
        }

        //绑定数据到车号，配件，修理厂列表框
        private void bindcomboboxedit()
        {
            this.vehicleno.Properties.Items.Clear();
            this.peijian.Properties.Items.Clear();
            this.repairchang.Properties.Items.Clear();
            this.repairman.Properties.Items.Clear();
            List<SqlPara> list1= new List<SqlPara>();
            List<SqlPara> list2= new List<SqlPara>();
            List<SqlPara> list3 = new List<SqlPara>();
            List<SqlPara> list4 = new List<SqlPara>();
            list3.Add(new SqlPara("gytype", "修理厂"));
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list1);
            DataSet ds1 = SqlHelper.GetDataSet(sps1);
            SqlParasEntity sps2 = new SqlParasEntity(OperType.Query, "V_GET_PJ", list2);
            DataSet ds2 = SqlHelper.GetDataSet(sps2);
            SqlParasEntity sps3 = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list3);
            DataSet ds3 = SqlHelper.GetDataSet(sps3);
            
            SqlParasEntity sps4 = new SqlParasEntity(OperType.Query, "V_GET_REP_MAN", list4);   //zjf20181012
            DataSet ds4 = SqlHelper.GetDataSet(sps4);

            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                this.vehicleno.Properties.Items.Add(ds1.Tables[0].Rows[i]["vehicleno"]);
            }
            this.vehicleno.Properties.Items.Add("全部");
            for (int k = 0; k < ds2.Tables[0].Rows.Count; k++)
            {
                this.peijian.Properties.Items.Add(ds2.Tables[0].Rows[k]["pjname"]);
            }
            this.peijian.Properties.Items.Add("全部");
            for (int j = 0; j < ds3.Tables[0].Rows.Count; j++)
            {
                this.repairchang.Properties.Items.Add(ds3.Tables[0].Rows[j]["gyname"]);//
            }
            this.repairchang.Properties.Items.Add("全部");
            for (int y = 0; y < ds4.Tables[0].Rows.Count; y++)
            {
                this.repairman.Properties.Items.Add(ds4.Tables[0].Rows[y]["wxman"]);
            }
            this.repairman.Properties.Items.Add("全部");
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
            string unit = this.gridView1.GetRowCellValue(no, "wxunit").ToString();
            w_update_dengji update = new w_update_dengji(unit);
            update.ShowDialog();
            selectlist();
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
                string unit = this.gridView1.GetRowCellValue(no, "wxunit").ToString();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("state",state));
                list.Add(new SqlPara("wxunit", unit));
                list.Add(new SqlPara("date", DateTime.Now));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDATE_WX_STATE", list);
                int row = SqlHelper.ExecteNonQuery(sps);
                selectlist();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, this.Text);
        }

        private void chexit_Click(object sender, EventArgs e)
        {
            this.Close();
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
            if (state == "1")
            {
                XtraMessageBox.Show("该条记录已被审核", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            updatestate(1);
            MsgBox.ShowOK();
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
         
        }
    }
}