using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmBasARFeeToVolume : BaseForm
    {
        public frmBasARFeeToVolume()
        {
            InitializeComponent();
        }

        private void frmBasARFeeToVolume_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("应收报价方数");
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            getdate();

        }

        //查询
        public void getdate()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasARFeeToVolume");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        
        }

        //删除
        private void delete()
        {
            try
            { 
                int rowhandle=myGridView1.FocusedRowHandle;
                if(rowhandle<0)return;
                string id = myGridView1.GetRowCellValue(rowhandle, "volumeID").ToString();
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list=new List<SqlPara>();
                list.Add(new SqlPara("vid", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BasARFeeToVolume_ByID", list);
                if(SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK("删除成功！");
                    //getdate();
                    myGridView1.DeleteRow(rowhandle);
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //修改
        private void update()
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "volumeID").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasARFeeToVolume_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                frmBasARFeeToVolumeAdd frm = new frmBasARFeeToVolumeAdd();
                frm.Owner = this;
                frm.dr = dr;
                frm.ShowDialog();      
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //新增
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBasARFeeToVolumeAdd frm = new frmBasARFeeToVolumeAdd();
            frm.Owner = this;
            frm.ShowDialog();
        }

        //删除
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            delete();
        }


        //修改
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            update();
        }

        
        //刷新
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdate();
        }

        
        //导入
        private void barButtonItem6_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            frmBasARFeeToVolumeImport1 frm = new frmBasARFeeToVolumeImport1();
            frm.Show();            
        }

        //导出
        private void barButtonItem7_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1,"应收报价方数");
        }


        //退出
        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }





    }
}
