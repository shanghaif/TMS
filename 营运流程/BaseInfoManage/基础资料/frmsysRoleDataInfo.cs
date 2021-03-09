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

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmsysRoleDataInfo : BaseForm
    {
        public frmsysRoleDataInfo()
        {
            InitializeComponent();
        }

        private void frmsysRoleDataInfo_Add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            Alldate();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmsysRoleDataInfo_And add = new frmsysRoleDataInfo_And();
            add.Show();
        }

        private void Alldate() {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sql = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_All", list);
                DataSet ds = SqlHelper.GetDataSet(sql);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "GUID").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GUID",id));
                SqlParasEntity sql = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_ById", list);
                DataSet ds=SqlHelper.GetDataSet(sql);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                DataRow dr = ds.Tables[0].Rows[0];
                frmsysRoleDataInfo_And update = new frmsysRoleDataInfo_And();
                update.dr = dr;
                update.ShowDialog();
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            
            

        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Alldate();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
    }
}
