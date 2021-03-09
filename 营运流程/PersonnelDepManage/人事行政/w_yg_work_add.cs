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
    public partial class w_yg_work_add : BaseForm
    {
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;

        public w_yg_work_add()
        {
            InitializeComponent();
        }

        private void w_yg_add_Load(object sender, EventArgs e)
        {
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
                    eddw.EditValue = gv2.GetRowCellValue(rowhandle2, "dw");
                    edbm.EditValue = gv2.GetRowCellValue(rowhandle2, "bm");
                    edzw.EditValue = gv2.GetRowCellValue(rowhandle2, "zw");
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
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand(oper == "NEW" ? insert() : update(), Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                //Conn.Close();
                //commonclass.ShowOK();
                //SqlCommand Cmd = new SqlCommand("USP_ADD_B_YG_WORK");
                //Cmd.CommandType = CommandType.StoredProcedure;
                //Cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Decimal)).Value = oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"));
                //Cmd.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar)).Value = edbh.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@billdate", SqlDbType.DateTime)).Value = edbilldate.DateTime;
                //Cmd.Parameters.Add(new SqlParameter("@billdate2  ", SqlDbType.DateTime)).Value = edbilldate2.DateTime;
                //Cmd.Parameters.Add(new SqlParameter("@dw", SqlDbType.VarChar)).Value = eddw.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@bm", SqlDbType.VarChar)).Value = edbm.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@zw", SqlDbType.VarChar)).Value = edzw.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@remark", SqlDbType.VarChar)).Value = edremark.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar)).Value = edcreateby.Text.Trim();
                //cs.ENQ(Cmd);
                //commonclass.ShowOK();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("billdate2", edbilldate2.DateTime));
                list.Add(new SqlPara("dw",eddw.Text.Trim()));
                list.Add(new SqlPara("bm",edbm.Text.Trim()));
                list.Add(new SqlPara("zw",edzw.Text.Trim()));
                list.Add(new SqlPara("remark", edremark.Text.Trim()));
                list.Add(new SqlPara("createby", edcreateby.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_B_YG_WORK", list));
                //oper = "NEW";
                clear();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.ToString(), "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        private string update()
        {
            string sql = "update b_yg_work set billdate='" + edbilldate.DateTime + "',billdate2='" + edbilldate2.DateTime + "',dw='" + eddw.Text.Trim() + "',bm='" + edbm.Text.Trim() + "',";
            sql += "zw='" + edzw.Text.Trim() + "',remark='" + edremark.Text.Trim() + "',createby='" + edcreateby.Text.Trim() + "' where bh='" + edbh.Text.Trim() + "' and id=" + Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id")) + "";
            return sql;
        }

        private string insert()
        {
            string sql = "insert b_yg_work(bh,billdate,billdate2,dw,bm,zw,remark,createby) values('" + edbh.Text.Trim() + "','" + edbilldate.DateTime + "','" + edbilldate2.DateTime + "',";
            sql += "'" + eddw.Text.Trim() + "','" + edbm.Text.Trim() + "','" + edzw.Text.Trim() + "','" + edremark.Text.Trim() + "','" + edcreateby.Text.Trim() + "')";
            return sql;
        }

        void clear()
        {
            edbilldate.DateTime = CommonClass.gcdate;
            edbilldate2.DateTime = CommonClass.gcdate;
            eddw.Text = "";
            edbm.Text = "";
            edzw.Text = "";
            edremark.Text = "";
            edcreateby.Text = CommonClass.UserInfo.UserName;
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }
    }
}