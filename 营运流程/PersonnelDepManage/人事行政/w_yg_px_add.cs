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
    public partial class w_yg_px_add : BaseForm
    {

        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;
        public w_yg_px_add()
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
                    edbilldate2.EditValue = gv2.GetRowCellValue(rowhandle2, "billdate2");
                    edcontent.EditValue = gv2.GetRowCellValue(rowhandle2, "content");
                    edaccount.EditValue = gv2.GetRowCellValue(rowhandle2, "account");
                    edtecher.EditValue = gv2.GetRowCellValue(rowhandle2, "techer");
                    edtechsite.EditValue = gv2.GetRowCellValue(rowhandle2, "techsite");
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
                    edbilldate2.EditValue = CommonClass.gcdate;
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
                //SqlCommand Cmd = new SqlCommand("USP_ADD_YG_PX");
                //Cmd.CommandType = CommandType.StoredProcedure;
                //Cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Decimal)).Value = oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"));
                //Cmd.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar)).Value = edbh.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@billdate", SqlDbType.DateTime)).Value = edbilldate.DateTime;
                //Cmd.Parameters.Add(new SqlParameter("@billdate2", SqlDbType.DateTime)).Value = edbilldate2.DateTime;
                //Cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarChar)).Value = edcontent.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@account", SqlDbType.VarChar)).Value = edaccount.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@techer", SqlDbType.VarChar)).Value = edtecher.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@techsite", SqlDbType.VarChar)).Value = edtechsite.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@remark", SqlDbType.VarChar)).Value = edremark.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar)).Value = edcreateby.Text.Trim();
                //cs.ENQ(Cmd);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("billdate2",edbilldate2.DateTime));
                list.Add(new SqlPara("content",edcontent.Text.Trim()));
                list.Add(new SqlPara("account", edaccount.Text.Trim()));
                list.Add(new SqlPara("techer",edtecher.Text.Trim()));
                list.Add(new SqlPara("techsite", edtechsite.Text.Trim()));
                list.Add(new SqlPara("remark", edremark.Text.Trim()));
                list.Add(new SqlPara("createby", edcreateby.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_YG_PX", list);
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
            edbilldate2.DateTime = CommonClass.gcdate;
            edcontent.Text = "";
            edaccount.Text = "";
            edtecher.Text = "";
            edtechsite.Text = "";
            edremark.Text = "";
            edcreateby.Text = CommonClass.UserInfo.UserName;
        }
    }
}