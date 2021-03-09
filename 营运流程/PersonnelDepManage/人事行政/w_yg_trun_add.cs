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
    public partial class w_yg_trun_add : BaseForm
    {
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;
        //commonclass cc = new commonclass();Cls_SqlHelper cs = new Cls_SqlHelper();

        public w_yg_trun_add()
        {
            InitializeComponent();
        }

        private void w_yg_add_Load(object sender, EventArgs e)
        {
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
                    ednewgs.EditValue = gv2.GetRowCellValue(rowhandle2, "newgs");
                    ednewbm.EditValue = gv2.GetRowCellValue(rowhandle2, "newbm");
                    ednewzw.EditValue = gv2.GetRowCellValue(rowhandle2, "newzw");
                    edreason.EditValue = gv2.GetRowCellValue(rowhandle2, "reason");
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

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            w_select_department wsd = new w_select_department();
            if (wsd.ShowDialog() == DialogResult.OK)
            {
                ednewgs.Text = CommonClass.UserInfo.DepartName;
                ednewbm.Text = CommonClass.UserInfo.DepartName;
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
                //SqlCommand Cmd = new SqlCommand("USP_ADD_YG_TRUN");
                //Cmd.CommandType = CommandType.StoredProcedure;
                //Cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Decimal)).Value = oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"));
                //Cmd.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar)).Value = edbh.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@billdate", SqlDbType.DateTime)).Value = edbilldate.DateTime;
                //Cmd.Parameters.Add(new SqlParameter("@oldgs", SqlDbType.VarChar)).Value = edoldgs.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@oldbm", SqlDbType.VarChar)).Value = edoldbm.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@oldzw", SqlDbType.VarChar)).Value = edoldzw.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@newgs", SqlDbType.VarChar)).Value = ednewgs.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@newbm", SqlDbType.VarChar)).Value = ednewbm.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@newzw", SqlDbType.VarChar)).Value = ednewzw.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@appby", SqlDbType.VarChar)).Value = edappby.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar)).Value = edreason.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar)).Value = edcreateby.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("oldgs",edoldgs.Text.Trim()));
                list.Add(new SqlPara("oldbm",edoldbm.Text.Trim()));
                list.Add(new SqlPara("oldzw",edoldzw.Text.Trim()));
                list.Add(new SqlPara("newgs",ednewgs.Text.Trim()));
                list.Add(new SqlPara("newbm",ednewbm.Text.Trim()));
                list.Add(new SqlPara("newzw",ednewzw.Text.Trim()));
                list.Add(new SqlPara("appby",edappby.Text.Trim()));
                list.Add(new SqlPara("reason",edreason.Text.Trim()));
                list.Add(new SqlPara("createby",edcreateby.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_YG_TRUN", list));
                //cs.ENQ(Cmd);
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
            ednewgs.Text = "";
            ednewbm.Text = "";
            ednewzw.Text = "";
            edreason.Text = "";
            edappby.Text = "";
            edcreateby.Text = CommonClass.UserInfo.UserName;
        }
    }
}