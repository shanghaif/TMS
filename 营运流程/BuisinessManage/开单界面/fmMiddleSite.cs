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
using System.Threading;

namespace ZQTMS.UI
{
    public partial class fmMiddleSite : BaseForm
    {
        public fmMiddleSite()
        {
            InitializeComponent();

        }

        private void fmDeliveryFee_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            //CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1); 
            LoadData();
        }

        private void LoadData()
        {
            //Thread th = new Thread(() =>
            //{
            //    try
            //    {
            //        List<SqlPara> list = new List<SqlPara>();

            //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASMIDDLESITE", list);
            //        DataSet ds = SqlHelper.GetDataSet(sps);

            //        if (ds == null || ds.Tables.Count == 0) return;
            //        if (!this.IsHandleCreated) return;
            //        this.Invoke((MethodInvoker)delegate
            //        {
            //            myGridControl1.DataSource = ds.Tables[0];
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        MsgBox.ShowException(ex);
            //    }
            //    finally
            //    {
            //        if (this.IsHandleCreated)
            //        {
            //            this.Invoke((MethodInvoker)delegate
            //            {
            //                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            //            });
            //        }
            //    }
            //});
            //th.IsBackground = true;
            //th.Start();

            try
            {
                myGridControl1.DataSource = CommonClass.dsMiddleSite.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void fmDeliveryFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }


 

    }
}
