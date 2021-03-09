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
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_yg_kp_add : BaseForm
    {
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;
        public string zwtype = "";

        public w_yg_kp_add()
        {
            InitializeComponent();
        }
        private void w_yg_add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);//工具
            try
            {
                int rowhandle1 = gv1.FocusedRowHandle;
                int rowhandle2 = gv2.FocusedRowHandle;
                if (oper == "MODIFY")
                {
                    edbh.EditValue = gv1.GetRowCellValue(rowhandle1, "bh");
                    edxm.EditValue = gv1.GetRowCellValue(rowhandle1, "xm");
                    edoldgs.EditValue = gv1.GetRowCellValue(rowhandle1, "fgs");
                    edoldbm.EditValue = gv1.GetRowCellValue(rowhandle1, "bm");
                    edoldzw.EditValue = gv1.GetRowCellValue(rowhandle1, "zw");
                    edbilldate.EditValue = gv2.GetRowCellValue(rowhandle2, "billdate");
                    editem.EditValue = gv2.GetRowCellValue(rowhandle2, "item");
                    editemresult.EditValue = gv2.GetRowCellValue(rowhandle2, "itemresult");
                    edaccount.EditValue = gv2.GetRowCellValue(rowhandle2, "account");
                    edappby.EditValue = gv2.GetRowCellValue(rowhandle2, "appby");
                    edremark.EditValue = gv2.GetRowCellValue(rowhandle2, "remark");
                    edcreateby.EditValue = gv2.GetRowCellValue(rowhandle2, "createby");
                }
                else
                {
                    edbh.EditValue = gv1.GetRowCellValue(rowhandle1, "bh");
                    edxm.EditValue = gv1.GetRowCellValue(rowhandle1, "xm");
                    edoldgs.EditValue = gv1.GetRowCellValue(rowhandle1, "fgs");
                    edoldbm.EditValue = gv1.GetRowCellValue(rowhandle1, "bm");
                    edoldzw.EditValue = gv1.GetRowCellValue(rowhandle1, "zw");
                    edbilldate.EditValue = CommonClass.gcdate;
                    edcreateby.EditValue = CommonClass.UserInfo.UserName;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            clear();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("item",editem.Text.Trim()));
                list.Add(new SqlPara("itemresult",editemresult.Text.Trim()));
                list.Add(new SqlPara("account",edaccount.Text.Trim()));
                list.Add(new SqlPara("remark",edremark.Text.Trim()));
                list.Add(new SqlPara("appby",edappby.Text.Trim()));
                list.Add(new SqlPara("createby",edcreateby.Text.Trim()));
                SqlParasEntity sps =  new SqlParasEntity(OperType.Query,"USP_ADD_YG_KP",list);
                SqlHelper.ExecteNonQuery(sps);
                MsgBox.ShowOK();
                oper = "NEW";
                clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                 
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

      

        void clear()
        {
            edbilldate.DateTime = CommonClass.gcdate;
            editem.Text = "";
            editemresult.Text = "";
            edaccount.Text = "";
            edappby.Text = "";
            edremark.Text = "";
            edcreateby.Text = CommonClass.UserInfo.UserName;
        }
    }
}