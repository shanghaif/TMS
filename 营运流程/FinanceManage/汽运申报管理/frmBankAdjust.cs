using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmBankAdjust : BaseForm
    {
        public frmBankAdjust()
        {
            InitializeComponent();
        }
        DataSet ds1 = new DataSet();
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MsgBox.ShowOK("已审核显示绿色，未审核显示白色");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem4_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //加载
        private void frmBankAdjust_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            //设置网格
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();
            getData();
        }
       
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
        private void simpleButton12_Click(object sender, EventArgs e)
        {
            getData();
        }

        public void getData()
        {
            try
            {
                myGridView1.ClearColumnsFilter();
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_B_Bank_Bill", list);
                ds1 = SqlHelper.GetDataSet(spe);

                myGridControl1.DataSource = ds1.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

  
      
        //导出
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)myGridControl1.MainView);
        }
        

      
      
        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string id = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                {

                    id += myGridView1.GetRowCellValue(i, "id") + "@";
                }
            }
            if (id=="")
            {
                MsgBox.ShowOK("请选择需要删除的数据！");
                return;
            }
            if (MsgBox.ShowYesNo("确定要删除吗？") != DialogResult.Yes)
            {
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id", id));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Delete_B_Bank_Bill", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("删除成功！");
                getData();
            }
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条数据！");
                return;
            }
            frmBankAdjustUpt frm = new frmBankAdjustUpt();
            frm.BankMan = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "bankman"));
            frm.BankCode = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "bankcode"));
            frm.BankName = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "bankname"));
            frm.Province = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "sheng"));
            frm.City = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "city"));
            frm.id = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "id"));
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                getData();    //DataGridView控件的值
            }

        }

       

     

       

        







    }
}
