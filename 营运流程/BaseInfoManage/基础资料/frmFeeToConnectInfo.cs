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
    public partial class frmFeeToConnectInfo : BaseForm
    {
        public frmFeeToConnectInfo()
        {
            InitializeComponent();
        }

        private void frmFeeToConnectInfo_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("报价连接");//插入日志
            CommonClass.FormSet(this);//针对窗体通用设置
            CommonClass.GetGridViewColumns(myGridView1);//加载MyGridView的Columns信息
            GridOper.SetGridViewProperty(myGridView1);//设置网格MyGridView的相关属性
            BarMagagerOper.SetBarPropertity(bar3);//设置工具条基本属性
            getdate();//加载数据源
        }
        
        //加载数据源
        public void getdate()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasFeeToConnect_All");//数据库查询操作
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

        //删除
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "Cid").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Cid", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_DEL_BasFeeToConnect_ByID", list);
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

        //刷新
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdate();
        }

        //退出
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        //新增
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmFeeToConnectInfoEdit frm = new frmFeeToConnectInfoEdit();
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
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "Cid").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Cid", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BasFeeToConnect_ByID ", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmFeeToConnectInfoEdit frm = new frmFeeToConnectInfoEdit();
                frm.Owner = this;
                frm.dr = dr;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //
        


    }
}
