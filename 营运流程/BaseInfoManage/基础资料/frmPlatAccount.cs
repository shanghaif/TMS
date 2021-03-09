using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using System.Threading;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI.BaseInfoManage
{
    public partial class frmPlatAccount : BaseForm
    {
        public frmPlatAccount()
        {
            InitializeComponent();
        }

        private void frmPlatAccount_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            GridOper.CreateStyleFormatCondition(myGridView1, "IsRestart", DevExpress.XtraGrid.FormatConditionEnum.Equal, 1, Color.FromArgb(255, 255, 128));
            freshData();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddPlatAccount frm = new frmAddPlatAccount();
            frm.ShowDialog();
            freshData();
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            freshData();
        }

        private void freshData()
        {
            Thread th = new Thread(() =>
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PlatAccountInfo", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);

                    if (ds == null || ds.Tables.Count == 0) return;
                    if (!this.IsHandleCreated) return;
                    this.Invoke((MethodInvoker)delegate
                    {
                        myGridControl1.DataSource = ds.Tables[0];
                    });
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                finally
                {
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
                        });
                    }
                }
            });
            th.IsBackground = true;
            th.Start();

        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "网点信息");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                int id = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "ID"));
                string companyid = myGridView1.GetRowCellValue(rowhandle, "CompanyID").ToString();
                if (CommonClass.UserInfo.companyid != companyid)
                {
                    MsgBox.ShowOK("只能修改自己公司的账户信息");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Id", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_PlatAccountInfo_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                frmAddPlatAccount frm = new frmAddPlatAccount();
                frm.dr = dr;
                frm.ShowDialog();
                freshData();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("未选中行!");
                return;
            }
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") == DialogResult.No)
            {
                return;
            }
            try
            {
                int id = Convert.ToInt32(myGridView1.GetRowCellValue(rowhandle, "ID"));
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_basPlatAccount", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK();
                    freshData();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}