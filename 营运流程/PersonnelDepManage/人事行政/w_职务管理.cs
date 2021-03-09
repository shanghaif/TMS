using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_职务管理 : BaseForm
    {
        public w_职务管理()
        {
            InitializeComponent();
        }
        DataSet ds = new DataSet();
        private void w_hx_方向_Load(object sender, EventArgs e)
        {
            //gridControl1.DataSource = commonclass.GetHXdirection().Tables[0];
           // OperGrid.LoadScheme(gridControl1);
            GridOper.LoadGridScheme(gridControl1);
            getdata();
        }

        private void getdata()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //ds.Clear();
                //SqlCommand sq = new SqlCommand("select * from b_zw", con);
                //SqlDataAdapter ada = new SqlDataAdapter(sq);
                //ada.Fill(ds);
                //gridControl1.DataSource = ds.Tables[0]; //SqlDataAdapter da = new SqlDataAdapter();
                //SqlCommand sq = new SqlCommand("QSP_GET_B_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsiteName ", SqlDbType.VarChar).Value = commonclass.username;
                //da.SelectCommand = sq;
                //ds.Clear();
                //da.Fill(ds);
                //gridControl1.DataSource = ds.Tables[0];
                //ds = cs.GetDataSet(sq, gridControl1);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsiteName",CommonClass.UserInfo.UserName));
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_B_ZW", list));
                if (ds != null && ds.Tables.Count > 0)
                    gridControl1.DataSource = ds.Tables[0];
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridView1.RowCount;i++ )
            {
                if (gridView1.GetRowCellValue(i, gridColumn1).ToString().Trim() == textEdit1.Text.Trim())
                {
                    XtraMessageBox.Show("已存在这个职务!请检查!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textEdit1.Focus();
                    return;
                }
            }
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand("insert into b_zw(zw,type) values('" + textEdit1.Text.Trim() + "','" + textEdit2.Text.Trim() + "')", Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                //getdata();
                //XtraMessageBox.Show("添加成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //textEdit1.Text = "";
                //textEdit2.SelectedIndex = 0;
                //SqlCommand sq = new SqlCommand("USP_ADD_B_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                
                //sq.Parameters.Add("@zw", SqlDbType.VarChar).Value = textEdit1.Text.Trim();
                //sq.Parameters.Add("@type", SqlDbType.VarChar).Value = textEdit2.Text.Trim();
                //cs.ENQ(sq);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("zw",textEdit1.Text.Trim()));
                list.Add(new SqlPara("type",textEdit2.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute,"USP_ADD_B_ZW",list));
                getdata();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                //Conn.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                XtraMessageBox.Show("请选择要删除的职务!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                return;
            }
            string xm = gridView1.GetRowCellValue(rowhandle, "zw").ToString();
            if (XtraMessageBox.Show("确定删除 " + xm + " 吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand("delete from b_zw where zw='" + xm + "'", Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                //SqlCommand sq = new SqlCommand("USP_DEL_B_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@zw", SqlDbType.VarChar)).Value = xm;
                //cs.ENQ(sq);

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("zw",xm));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DEL_B_ZW", list));

                XtraMessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textEdit1.Text = "";
                textEdit2.SelectedIndex = 0;

                getdata();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Conn.Close();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            if (gridView1.FocusedRowHandle < 0) return;
            textEdit1.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "zw").ToString();
            textEdit2.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "type").ToString();
        }
    }
}
