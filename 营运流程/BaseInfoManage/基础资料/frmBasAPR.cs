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
    public partial class frmBasAPR : BaseForm
    {
        public frmBasAPR()
        {
            InitializeComponent();
        }

        

        private void frmBasAPR_Load(object sender, EventArgs e)
        {
            bdate.DateTime = CommonClass.gbdate.AddDays(+1).AddHours(-24);
            edate.DateTime = CommonClass.gedate.AddDays(+1).AddHours(-24);
            CommonClass.InsertLog("报价明细");//插入日志
            CommonClass.FormSet(this);//针对窗体通用设置
            CommonClass.GetGridViewColumns(myGridView1);//加载MyGridView的Columns信息
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView2);
           
        }

       
        private void showdetail()
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowError("请选择一条数据！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("tname", Convert.ToString(myGridView1.GetFocusedRowCellValue("TableName"))));
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasFeeDetail_All4", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;              

                myGridControl2.DataSource = ds.Tables[0];

                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);


                return;
            }
        }       

        //提取
        private void cbRetrieve_Click_1(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("ConsignorCompany", txtConsignorCompany.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasFeeDetail_All", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //单击grid view事件
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            
            showdetail();
        }

        //退出
        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
