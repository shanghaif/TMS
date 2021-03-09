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
    public partial class xfChauffer : BaseForm
    {
        public xfChauffer()
        {
            InitializeComponent();
        }
        public static string strchaufferno = "";
        private void xfChauffer_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("驾驶员档案");//xj/2019/5/28
            CommonClass.FormSet(this);;
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            GridOper.RestoreGridLayout(gridView1,this.Text);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            GetChauffersall();
           
        }

        private void GetChauffersall()
        {
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CHAUFFER_ALL", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            this.gridControl1.DataSource = ds.Tables[0];
        }

        private void GetChauffers()
        {
            string chaufferno = this.txtChaufferno.Text.Trim() == "" ? "%%" : "%" + this.txtChaufferno.Text.Trim() + "%";



            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("chaufferno", chaufferno));
            list.Add(new SqlPara("bdate", bdate.DateTime.Date));
            list.Add(new SqlPara("edate", this.edate.DateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59)));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CHAUFFER_bytime", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            this.gridControl1.DataSource = ds.Tables[0];
            //for (int i = 0; i < this.gridControl1.Views[0].ro; i++)
            //{

            //    if (Convert.ToDateTime(this.gridView1.Columns["baoxianqixian"].Caption) > DateTime.Now.AddMonths(-1))
            //    {

            //    }
            //}
        }

        //private static SqlConnection conn = new SqlConnection("server=.,uid=sa,pwd=123");

        //private DataTable GetDataSet_EE(string procname, SqlParameter[] values)
        //{
        //    DataSet ds = new DataSet();
        //    SqlCommand cmd = new SqlCommand(procname, conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddRange(values);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(ds);
        //    return ds.Tables[0];
        //}

        private void cbretrieve_CheckedChanged(object sender, EventArgs e)
        {
            GetChauffers();
        }
        private void barButtonItem14_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            xfChaufferInfo sfc = new xfChaufferInfo();
            strchaufferno = "";
            sfc.ShowDialog();
            GetChauffersall();
        }

        private void barButtonItem15_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
             if (this.gridView1.RowCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string chaufferno = this.gridView1.GetRowCellValue(no, "chaufferno").ToString();
            xfChaufferInfo sfc = new xfChaufferInfo();
            strchaufferno = chaufferno;
            sfc.ShowDialog();
            GetChauffersall();
        }

        private void barButtonItem16_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.gridView1.RowCount == 0)
            {
                return;
            }
            if (XtraMessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                int no = this.gridView1.FocusedRowHandle;
                string chaufferno = this.gridView1.GetRowCellValue(no, "chaufferno").ToString();
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("chaufferno", chaufferno));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "V_UPDTAE_CHAUFFER_STATE", list);
                SqlHelper.ExecteNonQuery(sps);
                GetChauffers();
            }
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.gridView1.RowCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string chaufferno = this.gridView1.GetRowCellValue(no, "chaufferno").ToString();
            xfChaufferInfo sfc = new xfChaufferInfo();
            strchaufferno = chaufferno;
            sfc.ShowDialog();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //cc.LockGridLayout(this.gridControl1, "驾驶员");
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1,this.Text);
        }

        private void checkButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ImgeUplode_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int gridRows = gridView1.RowCount;
            if (gridRows==0)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            string no = GridOper.GetRowCellValueString(gridView1, rowhandle, "chaufferno");
            string name = GridOper.GetRowCellValueString(gridView1, rowhandle, "chauffername");
            frmDriverImageFileUp FDI = new frmDriverImageFileUp(no, name);
            //FDI.Id = GridOper.GetRowCellValueString(gridView1, rowhandle, "chaufferno");
            //FDI.Name = GridOper.GetRowCellValueString(gridView1, rowhandle, "chauffername");
            FDI.ShowDialog();
        }

        private void SearchLicence_Click(object sender, EventArgs e)
        {
            int gridRows = gridView1.RowCount;
            if (gridRows == 0)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
            string no = GridOper.GetRowCellValueString(gridView1, rowhandle, "chaufferno");
            frmDriverImgShow FIS = new frmDriverImgShow(no);
            FIS.ShowDialog();
        }
    }
}