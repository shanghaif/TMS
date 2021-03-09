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
using DevExpress.XtraBars.Alerter;
using System.Threading;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using System.Reflection;

namespace ZQTMS.UI
{
    public partial class w_yg_total : BaseForm
    {
        DataSet ds = new DataSet();
        //string exception = "";

        public w_yg_total()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_YGDA_TOTAL");
                //sq.CommandType = CommandType.StoredProcedure;
                
                //ds.Clear();
                //ds = cs.GetDataSet(sq,gridControl1);


                //List<SqlPara> list = new List<SqlPara>();
                ////list.Add(new SqlPara("CarId", CarId));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_TOTAL");
                DataSet ds = SqlHelper.GetDataSet(sps);
                gridControl1.DataSource = ds.Tables[0];
                if (ds == null || ds.Tables.Count == 0) return;
                DateTime t = CommonClass.gcdate;
                string s = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTime birthday = ds.Tables[0].Rows[i]["birthday"] == DBNull.Value ? t : Convert.ToDateTime(ds.Tables[0].Rows[i]["birthday"]);
                    string state = ds.Tables[0].Rows[i]["state"].ToString();
                    if ((birthday.AddYears(t.Year - birthday.Year) - t).TotalDays >= 0 && (birthday.AddYears(t.Year - birthday.Year) - t).TotalDays <= 10)
                    {
                        if (state == "在职" || state == "新聘" || state == "待岗")
                        {
                            s += ds.Tables[0].Rows[i]["xm"].ToString() + ",";
                        }
                    }
                }
                s = s.TrimEnd(',');
                if (s.Trim() != "")
                {
                    s = "以下人员即将过生日：\r\n" + s;

                    AlertControl alertControl1 = new AlertControl();
                    alertControl1.AutoFormDelay = 10000;
                    alertControl1.Show(null, "", s);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                 
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //commonclass.SetBarPropertity(bar3);
            //cc.RestoreGridLayout(gridControl1, "员工档案登记");
            CommonClass.InsertLog("员工总表");
            getdata();
        }

        private void modify()
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {

                w_yg_add wa = new w_yg_add();
                wa.bh = gridView1.GetRowCellValue(rowhandle, "bh") == DBNull.Value ? "" : gridView1.GetRowCellValue(rowhandle, "bh").ToString();
                wa.ShowDialog();
            }
        }
        
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            modify();
        }

        private void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //cc.GenSeq(e);
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "员工档案登记");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, "员工档案登记");
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            w_yg_add wya = new w_yg_add();
            wya.Show();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            modify();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle >= 0)
            {
                string bh = gridView1.GetRowCellValue(rowhandle, "bh").ToString();
                if (XtraMessageBox.Show("确认要删除该笔记录吗? 编号是：" + bh + "\r\n\r\n删除该员工资料后，该员工的其他资料也将被删除!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {

                    //SqlCommand sq = new SqlCommand("USP_DELETE_YGDA_BH");
                    //sq.CommandType = CommandType.StoredProcedure;
                    //sq.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar));
                    //sq.Parameters[0].Value = bh;
                    //cs.ENQ(sq);
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("bh", bh));
                    SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_YGDA_BH", list));
                    gridView1.DeleteRow(rowhandle);
                }
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\LQSoft.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("LQSoft.UI.frmBillSearch");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            frm.Show();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            //if (e.RowHandle < 0) return;
            //DateTime t = commonclass.gcdate;
            //DateTime birthday = gridView1.GetRowCellValue(e.RowHandle, "birthday") == DBNull.Value ? t : Convert.ToDateTime(gridView1.GetRowCellValue(e.RowHandle, "birthday"));
            //string state = gridView1.GetRowCellValue(e.RowHandle, "state").ToString();
            //if ((birthday.AddYears(t.Year - birthday.Year) - t).TotalDays >= 0 && (birthday.AddYears(t.Year - birthday.Year) - t).TotalDays <= 10)
            //{
            //    if (state == "在职" || state == "新聘" || state == "待岗")
            //    {
            //        e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
            //    }
            //}
        }
    }
}