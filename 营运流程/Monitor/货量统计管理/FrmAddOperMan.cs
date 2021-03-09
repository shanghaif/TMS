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

namespace ZQTMS.UI
{
    public partial class FrmAddOperMan : BaseForm
    {
        public FrmAddOperMan()
        {
            InitializeComponent();
        }

        private void FrmAddOperMan_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            GetGrouperInfo();
           
        }

        public void GetGrouperInfo()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GrouperInfo");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmAddGrouper frm = new FrmAddGrouper();
            frm.ShowDialog();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条要删除的信息！");
                    return;
                }
                string subopetman = myGridView1.GetRowCellValue(rowHandle, "subopetman").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("subopetman", subopetman));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Del_GrouperInfo", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            GetGrouperInfo();
        }
    }
}