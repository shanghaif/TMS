using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class FrmVerifyDirection : BaseForm
    {
        public FrmVerifyDirection()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable();
        bool flag = true;
        private void FrmVerifyDirection_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("direction");
            gridControl1.DataSource = dt;
            dt.AcceptChanges();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SYSPARAMSETTING_1");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                flag = false;
                return;
            }
            DataTable dt_temp = ds.Tables[0];
          


            

            string[] arrStr = dt_temp.Rows[0]["ParamValue"].ToString().Split(',');
                foreach (string str in arrStr)
                {
                    DataRow row = dt.NewRow();
                    row["direction"] = str;
                    dt.Rows.Add(row);
                }
            
            gridControl1.DataSource = dt;
            dt.AcceptChanges();

        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
           //StringBuilder strBuilder = new StringBuilder();
            
            try
            {
                if (VerifyDirection.Text.Trim() == "")
                {
                    MsgBox.ShowOK("核销方向不能为空！");
                    return;
                }
                if (flag == true)
                {
                    DataRow row = dt.NewRow();
                    row["direction"] = this.VerifyDirection.Text.Trim();
                    dt.Rows.Add(row);
                    dt.AcceptChanges();
                    string strResult = string.Empty;
                    if (dt != null)
                    {
                        foreach (DataRow row1 in dt.Rows)
                        {
                            strResult += row1["direction"].ToString() + ",";
                        }
                    }
                    strResult = strResult.Trim(',');
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ParamType", "VerifyDirection"));
                    list.Add(new SqlPara("paramValue", strResult));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_SAVE_SYSPARAMSETTING_1", list);

                    int num = SqlHelper.ExecteNonQuery(sps);

                    if (num > 0)
                    {
                        MsgBox.ShowOK();
                        this.gridControl1.DataSource = dt;
                    }
                    else
                        MsgBox.ShowOK("操作失败！");
                }
                else
                {
                    MsgBox.ShowError("该公司没有该参数设置，请到参数设置页面增加！");
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //StringBuilder strBuilder = new StringBuilder();

            try
            {
                if (this.gridView1.FocusedRowHandle < 0)
                {
                    MsgBox.ShowOK("请选中一行再做删除操作!");
                    return;
                }
                if (MsgBox.ShowYesNo("是否确定删除?") != DialogResult.Yes) return;
                this.gridView1.DeleteRow(this.gridView1.FocusedRowHandle);
                dt.AcceptChanges();
                string strResult = string.Empty;
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        strResult += row["direction"].ToString() + ",";
                    }
                }
                strResult = strResult.Trim(',');

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ParamType", "VerifyDirection"));
                list.Add(new SqlPara("paramValue", strResult));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_SAVE_SYSPARAMSETTING_1", list);

                int num = SqlHelper.ExecteNonQuery(sps);

                if (num > 0)
                {
                    MsgBox.ShowOK();
                    this.gridControl1.DataSource = dt;
                }
                else
                    MsgBox.ShowOK("操作失败！");

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

   
    
    
    
    
    
    
    }
}
