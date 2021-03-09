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
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using System.Reflection;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DevExpress.XtraTreeList;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmRightGroup : BaseForm
    {
        public frmRightGroup()
        {
            InitializeComponent();
        }
        DataSet dsmenu = new DataSet();

        private void frmRightGroupAdd_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("权限管理");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar1);
            GetGroupData();
        }

        private bool GetGroupData()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RightGroup_KT", list);
                dsmenu = SqlHelper.GetDataSet(sps);
                
                if (dsmenu == null || dsmenu.Tables.Count == 0) return true;
                myGridControl1.DataSource = dsmenu.Tables[0];
                return true;
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("菜单加载失败：\r\n" + ex.Message);
                return false;
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e) //新增
        {
            frmRightGroupAdd frm = new frmRightGroupAdd();
            frm.ShowDialog();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e) //修改
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                string GRCode = myGridView1.GetRowCellValue(rowhandle, "GRCode").ToString();
                string GRName = myGridView1.GetRowCellValue(rowhandle, "GRName").ToString();
                string GRRemark = myGridView1.GetRowCellValue(rowhandle, "GRRemark").ToString();
                using (frmRightGroupAdd frm = new frmRightGroupAdd())
                {
                    frm.grCode = GRCode;
                    frm.grName = GRName;
                    frm.grRemark = GRRemark;
                    frm.oper = 2;
                    frm.ShowDialog();
                    frm.Dispose(); 
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("权限初始化：" + ex.Message);
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                string GRCode = myGridView1.GetRowCellValue(rowhandle, "GRCode").ToString();

                if (MsgBox.ShowYesNo("是否删除权限组？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("GRCode", GRCode));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_RightGroup_KT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView1.DeleteRow(rowhandle);
                    dsmenu.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetGroupData();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                string GRCode = myGridView1.GetRowCellValue(rowhandle, "GRCode").ToString();
                string GRName = myGridView1.GetRowCellValue(rowhandle, "GRName").ToString();
                string GRRemark = myGridView1.GetRowCellValue(rowhandle, "GRRemark").ToString();
                frmRightGroupAdd frm = new frmRightGroupAdd();
                frm.grCode = GRCode;
                frm.grName = GRName;
                frm.grRemark = GRRemark;
                frm.oper = 3;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}