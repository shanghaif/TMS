using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmSearchBillInfoByBillNo : BaseForm
    {
        public frmSearchBillInfoByBillNo()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textEdit1.Text.Trim() == "")
                {
                    MsgBox.ShowOK("运单号不能为空");
                    return;
                }
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("billno", textEdit1.Text.Trim()));
                    if (xtraTabControl1.SelectedTabPageIndex == 0)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillinfoByNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl1.DataSource = ds.Tables[0];
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 1)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ShortconninfoByNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl2.DataSource = ds.Tables[0];
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 2)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureinfoByBillNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl3.DataSource = ds.Tables[0];
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 3)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SendGoodsinfoByBillNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl4.DataSource = ds.Tables[0];
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 4)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_billSigninfoByBillNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl5.DataSource = ds.Tables[0];
                    }
                    else if (xtraTabControl1.SelectedTabPageIndex == 5)
                    {
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_fullBillinfoByBillNo", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0) return;
                        myGridControl6.DataSource = ds.Tables[0];
                    }
                }
            
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSearchBillInfoByBillNo_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6);
            GridOper.SetGridViewProperty(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6);
            GridOper.RestoreGridLayout(myGridView1, myGridView2, myGridView3, myGridView4, myGridView5, myGridView6);
        }

    }
}