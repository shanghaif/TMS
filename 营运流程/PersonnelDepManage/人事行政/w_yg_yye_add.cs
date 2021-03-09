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
    public partial class w_yg_yye_add : BaseForm
    {
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv1, gv2;
        public w_yg_yye_add()
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
                    edaccount.EditValue = gv2.GetRowCellValue(rowhandle2, "account"); 
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
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand(oper == "NEW" ? insert() : update(), Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                //Conn.Close();
                //commonclass.ShowOK();
                //oper = "NEW";
                //clear();


                //SqlCommand Cmd = new SqlCommand("USP_ADD_B_YG_YYE");
                //Cmd.CommandType = CommandType.StoredProcedure;
                //Cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Decimal)).Value = oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"));
                //Cmd.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar)).Value = edbh.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@billdate", SqlDbType.DateTime)).Value = edbilldate.DateTime;
                //if (edaccount.Text.Trim() == "")
                //    Cmd.Parameters.Add(new SqlParameter("@account", SqlDbType.Float)).Value = 0.0;
                //else
                //    Cmd.Parameters.Add(new SqlParameter("@account", SqlDbType.Float)).Value = float.Parse(edaccount.Text.Trim());
                //Cmd.Parameters.Add(new SqlParameter("@remark", SqlDbType.VarChar)).Value = edremark.Text.Trim();
                //Cmd.Parameters.Add(new SqlParameter("@createby", SqlDbType.VarChar)).Value = edcreateby.Text.Trim();
                //cs.ENQ(Cmd);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id",oper == "NEW" ? 0 : Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id"))));
                list.Add(new SqlPara("bh",edbh.Text.Trim()));
                list.Add(new SqlPara("billdate",edbilldate.DateTime));
                list.Add(new SqlPara("account",ConvertType.ToDecimal(edaccount.Text.Trim())));
                list.Add(new SqlPara("remark",edremark.Text.Trim()));
                list.Add(new SqlPara("createby",edcreateby.Text.Trim()));
                MsgBox.ShowOK();
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_B_YG_YYE", list));
                oper = "NEW";
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

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private string update()
        {
            string sql = "update b_yg_yye set billdate='" + edbilldate.DateTime + "',account=" + edaccount.Text.Trim() + ",";
            sql += "remark='" + edremark.Text.Trim() + "',createby='" + edcreateby.Text.Trim() + "' where bh='" + edbh.Text.Trim() + "' and id=" + Convert.ToDecimal(gv2.GetRowCellValue(gv2.FocusedRowHandle, "id")) + "";
            return sql;
        }

        private string insert()
        {
            string sql = "insert b_yg_yye(bh,billdate,account, remark,createby) values('" + edbh.Text.Trim() + "','" + edbilldate.DateTime + "',";
            sql += "" + edaccount.Text.Trim() + ",'" + edremark.Text.Trim() + "','" + edcreateby.Text.Trim() + "')";
            return sql;
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