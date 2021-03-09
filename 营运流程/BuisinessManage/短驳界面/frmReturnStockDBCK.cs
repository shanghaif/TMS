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
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmReturnStockDBCK : BaseForm
    {

        public string Billno = "", ReceiptCondition="";
        public string type = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public string aa = "",bb="";
        public frmReturnStockDBCK()
        {
            InitializeComponent();
        }

        private void frmReturnStockDBCK_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            
            dt.Columns.Add(new DataColumn("BillNo", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("ReceiptCondition", Type.GetType("System.String")));
            
            string[] a = Billno.Split('@');
            string[] c = ReceiptCondition.Split('@');
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != "")
                {
                    DataRow dr = dt.NewRow();
                    dr["BillNo"] = ConvertType.ToString(a[i]).Trim();
                    dr["ReceiptCondition"] = ConvertType.ToString(c[i]).Trim();
                    dt.Rows.Add(dr);
                }
            }
            ds.Tables.Add(dt);
            myGridControl1.DataSource = ds.Tables[0];

        }

        //新增
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            textEdit1.Text = "";
        }

        //添加
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            //判断是否签回单
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", textEdit1.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ReceiptCondition", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                string ReceiptCondition = ds.Tables[0].Rows[0]["ReceiptCondition"].ToString();
                string HDState = ds.Tables[0].Rows[0]["HDState"].ToString();
                if (ReceiptCondition != "签回单")
                {
                    MsgBox.ShowOK("添加的运单号，不是签回单，不能添加！");
                    return;
                }
                if (HDState != "去")
                {
                    MsgBox.ShowOK("只有去的回单可以新增！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            DataRow dr = dt.NewRow();
            dr["BillNo"] = ConvertType.ToString(textEdit1.Text).Trim();
            dr["ReceiptCondition"] = ConvertType.ToString("签回单").Trim();
            dt.Rows.Add(dr);
            panel1.Visible = false;
           
            //生成问题件
            try
            {
                string billno=textEdit1.Text.Trim();
                List<SqlPara> list7 = new List<SqlPara>();
                list7.Add(new SqlPara("BillNo", textEdit1.Text));
                list7.Add(new SqlPara("type", type));
                list7.Add(new SqlPara("a", 1));
                SqlParasEntity spe7 = new SqlParasEntity(OperType.Execute, "QSP_ADD_ReturnStock_BILLNO", list7);
                if (SqlHelper.ExecteNonQuery(spe7) > 0)
                {
                    CommonSyn.HDProblemPartsSyn(billno, type, 1);//回单问题件同步 zaj 2018-5-11
                }
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
             textEdit1.Text = "";
        }

        //删除
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string b = myGridView1.GetRowCellValue(rowhandle, "BillNo").ToString();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string a = ds.Tables[0].Rows[i]["BillNo"].ToString();
                if (b == a)
                {
                    ds.Tables[0].Rows[i].Delete();
                }

            }
            //生成问题件
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", b));
                list.Add(new SqlPara("type", type));
                list.Add(new SqlPara("a", 2));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_ReturnStock_BILLNO", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    CommonSyn.HDProblemPartsSyn(b, type, 2);//回单问题件同步 zaj 2018-5-11
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        //保存
        private void simpleButton3_Click(object sender, EventArgs e)
        {
           
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                aa += myGridView1.GetRowCellValue(i, "BillNo").ToString()+"@";
            }
            this.Close();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            bb = "1";
            this.Close();
        }
    }
}