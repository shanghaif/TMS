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
    public partial class frmAPSendFeeToNumInfo : BaseForm
    {
        public frmAPSendFeeToNumInfo()
        {
            InitializeComponent();
        }

        private void frmAPSendFeeToNumInfo_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("应付报价件数运费");//插入日志
            CommonClass.FormSet(this);//针对窗体通用设置
            CommonClass.GetGridViewColumns(myGridView1);//加载MyGridView的Columns信息
            GridOper.SetGridViewProperty(myGridView1);//设置网格MyGridView的相关属性
            BarMagagerOper.SetBarPropertity(bar3);//设置工具条基本属性
            updata();//加载数据源
        }

        //加载数据源
        public void updata() 
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasAPSendFeeToNum_All");//数据库查询操作
                DataSet ds = SqlHelper.GetDataSet(sps);//把sps传给数据集
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) //判断新增的行数是否等于0
                {
                    return;
                }
                myGridControl1.DataSource = ds.Tables[0];//把数据传给GridControl
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //刷新
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            updata();
        }

        //退出
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        //删除
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "numID").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BasAPSendFeeToNum_ByID", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.DeleteRow(rowhandle);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //新增
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAPSendFeeToNumInfoEdit frm = new frmAPSendFeeToNumInfoEdit();
            frm.Owner = this;
            frm.ShowDialog();
        }

        //修改
        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "numID").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasAPSendFeeToNum_ByID ", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmAPSendFeeToNumInfoEdit frm = new frmAPSendFeeToNumInfoEdit();
                frm.Owner = this;
                frm.dr = dr;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "应付报价件数运费档案");
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmAPSendFeeToNumImport frm = new frmAPSendFeeToNumImport();
            frm.Owner = this;
            frm.ShowDialog();
        }

    }
}
