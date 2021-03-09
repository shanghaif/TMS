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
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class w_考核项目定义 : BaseForm
    {
        public w_考核项目定义()
        {
            InitializeComponent();
        }

        DataSet ds = new DataSet();
        private void w_hx_方向_Load(object sender, EventArgs e)
        {
            //gridControl1.DataSource = commonclass.GetHXdirection().Tables[0];
            //cc.LoadScheme(gridControl1);
            getdata();
        }

        private void getdata()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("select * from b_kh_xm",con);
                //SqlDataAdapter ada = new SqlDataAdapter(sq);
                //ada.Fill(ds);
                //gridControl1.DataSource = ds.Tables[0];
                //SqlCommand sq = new SqlCommand("QSP_GET_B_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@bsiteName ", SqlDbType.VarChar).Value = CommonClass.UserInfo.UserName;
                ////DataSet ds = new DataSet();
                ////ada.Fill(ds);
                //ds = cs.GetDataSet(sq, gridControl1);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsiteName", CommonClass.UserInfo.UserName));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_B_KH_XM", list));
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
                    XtraMessageBox.Show("已存在这个考核项目!请检查!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textEdit1.Focus();
                    return;
                }
            }
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand("insert into b_kh_xm(xm) values('" + textEdit1.Text.Trim() + "')", Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                // SqlCommand sq = new SqlCommand("USP_ADD_B_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;

                //sq.Parameters.Add("@xm", SqlDbType.VarChar).Value = textEdit1.Text.Trim();
               
                //cs.ENQ(sq);

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("xm", textEdit1.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_B_KH_XM", list));
                getdata();
                XtraMessageBox.Show("添加成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (gridView1.FocusedRowHandle < 0) return;
                textEdit1.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridColumn1).ToString();
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
                XtraMessageBox.Show("请选择要删除的考核项目!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                return;
            }
            string xm = gridView1.GetRowCellValue(rowhandle, "xm").ToString();
            if (XtraMessageBox.Show("确定删除 " + xm + " 吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            //SqlConnection Conn = cc.GetConn();
            try
            {
                //SqlCommand Cmd = new SqlCommand("delete from b_kh_xm where xm='" + xm + "'", Conn);
                //Conn.Open();
                //Cmd.ExecuteNonQuery();
                //SqlCommand sq = new SqlCommand("USP_DEL_B_KH_XM");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@xm", SqlDbType.VarChar)).Value = xm;
                //cs.ENQ(sq);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("xm", textEdit1.Text.Trim()));
                SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DEL_B_KH_XM", list));
                int a = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                {
                    if (ds.Tables[0].Rows[i]["xm"].ToString() == textEdit1.Text.Trim())
                    {
                        ds.Tables[0].Rows.RemoveAt(i);
                        a++;
                        break;
                    }
                }
                if (a == 0)
                {
                    XtraMessageBox.Show("系统中不存在该考核项目,请检查!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                getdata();
                XtraMessageBox.Show("删除成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (gridView1.FocusedRowHandle < 0) return;
                textEdit1.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridColumn1).ToString();
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
            textEdit1.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, gridColumn1).ToString();
        }
    }
}
