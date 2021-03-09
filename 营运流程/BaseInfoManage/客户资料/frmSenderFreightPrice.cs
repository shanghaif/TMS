using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraBars;
 
namespace ZQTMS.UI
{
    public partial class frmSenderFreightPrice : BaseForm
    {

        public string CustNo;
        DataSet dataset1 = new DataSet();

        public string shippername = "";

        public frmSenderFreightPrice()
        {
            InitializeComponent();
        }

        private void frmSenderFreightPrice_Load(object sender, EventArgs e)
        { 
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);  
            getdata();
            CommonClass.SetSite(repositoryItemComboBox1, false);
        }
        private void getdata()
        { 
            try
            { 
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CustNo", CustNo));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basSenderFreightPrice", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.ToString());
            }
        }

        private void save()
        {
            gridView1.PostEditor();
            gridView1.UpdateCurrentRow();
 
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CustNo", CustNo));
                list.Add(new SqlPara("Tb", gridControl1.DataSource as DataTable));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_SenderFreightPrice_UPLOAD", list);
                if (SqlHelper.ExecteNonQuery(sps)>0)   
                    MsgBox.ShowOK();
                else
                    MsgBox.ShowError("保存失败！"); 
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.Message);
            } 
        }
 
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridView1.AddNewRow();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
                gridView1.DeleteRow(rowhandle);

        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            save();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            gridView1.PostEditor();
            gridView1.UpdateCurrentRow();
        }
    }
}
