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
    public partial class w_yg_gs_add : BaseForm
    {
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;

        public w_yg_gs_add()
        {
            InitializeComponent();
        }
        private void w_yg_add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);
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
                    edreason.EditValue = gv2.GetRowCellValue(rowhandle2, "reason");
                    editemtype.EditValue = gv2.GetRowCellValue(rowhandle2, "itemtype");
                    edresult.EditValue = gv2.GetRowCellValue(rowhandle2, "result");
                    edappby.EditValue = gv2.GetRowCellValue(rowhandle2, "appby");
                    edcreateby.EditValue = gv2.GetRowCellValue(rowhandle2, "createby");
                }
                else
                {
                    edbh.EditValue = gv1.GetRowCellValue(rowhandle1, "bh");
                    edxm.EditValue = gv1.GetRowCellValue(rowhandle1, "xm");
                    edoldgs.EditValue = gv1.GetRowCellValue(rowhandle1, "fgs");
                    edoldbm.EditValue = gv1.GetRowCellValue(rowhandle1, "bm");
                    edoldzw.EditValue = gv1.GetRowCellValue(rowhandle1, "zw");
                    edbilldate.DateTime = CommonClass.gcdate;
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
                list.Add(new SqlPara("id", oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("item",editem.Text.Trim()));
                list.Add(new SqlPara("reason",edreason.Text.Trim()));
                list.Add(new SqlPara("itemtype",editemtype.Text.Trim()));
                list.Add(new SqlPara("result",edresult.Text.Trim()));
                list.Add(new SqlPara("appby",edappby.Text.Trim()));
                list.Add(new SqlPara("createby", edcreateby.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_YG_GS", list);
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
            edreason.Text = "";
            editemtype.Text = "";
            edresult.Text = "";
            edappby.Text = "";
            edcreateby.Text = CommonClass.UserInfo.UserName;
        }
    }
}