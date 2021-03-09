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
    public partial class w_yg_gz_add : BaseForm
    {

        public string bh = "";
        public int gzid = 0;
        public string oper = "NEW";
        public DevExpress.XtraGrid.Views.Grid.GridView gv;
        public w_yg_gz_add()
        {
            InitializeComponent();
        }

        private void w_yg_add_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);
            edyear.Text = CommonClass.gcdate.Year.ToString();
            edmonth.Text = CommonClass.gcdate.Month.ToString();
            
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_INFO");
                DataSet ds = new DataSet();
                ds = SqlHelper.GetDataSet(sps); if (ds == null) return;

                //绑定员工编号和姓名
                lookUpEdit1.Properties.DataSource = ds.Tables[0].DefaultView;
                lookUpEdit1.Properties.DisplayMember = "xm";
                lookUpEdit1.Properties.ValueMember = "bh";
                if (oper == "NEW")
                {
                    lookUpEdit1.ItemIndex = -1;
                    return;
                }

                if (oper == "MODIFY")
                {
                    lookUpEdit1.Properties.ReadOnly = true;
                    int row = gv.FocusedRowHandle;
                    textEdit1.Text = bh;
                    textEdit2.EditValue = gv.GetRowCellValue(row, "fgs");
                    textEdit3.EditValue = gv.GetRowCellValue(row, "bm");
                    edyear.EditValue = gv.GetRowCellValue(row, "gzyear");
                    edmonth.EditValue = gv.GetRowCellValue(row, "gzmonth");
                    gzori.EditValue = gv.GetRowCellValue(row, "gzori");
                    gzpro.EditValue = gv.GetRowCellValue(row, "gzpro");
                    gzqqj.EditValue = gv.GetRowCellValue(row, "gzqqj");
                    gzfb.EditValue = gv.GetRowCellValue(row, "gzfb");
                    gzhb.EditValue = gv.GetRowCellValue(row, "gzhb");
                    gzqita.EditValue = gv.GetRowCellValue(row, "gzqita");
                    gzqjdays.EditValue = gv.GetRowCellValue(row, "gzqjdays");
                    gzqyjreduce.EditValue = gv.GetRowCellValue(row, "gzqyjreduce");
                    gzjbdays.EditValue = gv.GetRowCellValue(row, "gzjbdays");

                    gzjb.EditValue = gv.GetRowCellValue(row, "gzjb");
                    gztel.EditValue = gv.GetRowCellValue(row, "gztel");
                    gzdf.EditValue = gv.GetRowCellValue(row, "gzdf");
                    gzkgdays.EditValue = gv.GetRowCellValue(row, "gzkgdays");
                    gzkgreduce.EditValue = gv.GetRowCellValue(row, "gzkgreduce");
                    gzborrow.EditValue = gv.GetRowCellValue(row, "gzborrow");
                    gzsuodeshui.EditValue = gv.GetRowCellValue(row, "gzsuodeshui");
                    gzyanglao.EditValue = gv.GetRowCellValue(row, "gzyanglao");
                    gzfactout.EditValue = gv.GetRowCellValue(row, "gzfactout");

                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int getgzid()
        {
            
            try
            {

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GZID_MAX");
                DataSet ds = new DataSet();
                ds = SqlHelper.GetDataSet(sps); if (ds == null) return 0;
                return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return 0;
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (lookUpEdit1.EditValue.ToString().Trim() == "")
                return;
            if (gzfactout.Text.Trim() == "0")
            {
                XtraMessageBox.Show("请填写发放金额!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            try
            {
 
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("operationtype",oper));
                list.Add(new SqlPara("bh",lookUpEdit1.EditValue));
                list.Add(new SqlPara("gzyear",edyear.Text));
                list.Add(new SqlPara("gzmonth",edmonth.Text));
                list.Add(new SqlPara("gzori",gzori.Text));
                list.Add(new SqlPara("gzpro",gzpro.Text));
                list.Add(new SqlPara("gzfb",gzfb.Text));
                list.Add(new SqlPara("gzhb",gzhb.Text));
                list.Add(new SqlPara("gzqita",gzqita.Text));
                list.Add(new SqlPara("gzqqj",gzqqj.Text));
                list.Add(new SqlPara("gzjbdays",gzjbdays.Text));
                list.Add(new SqlPara("gzjb",gzjb.Text));
                list.Add(new SqlPara("gztel",gztel.Text));
                list.Add(new SqlPara("gzdf",gzdf.Text));
                list.Add(new SqlPara("gzqjdays",gzqjdays.Text));
                list.Add(new SqlPara("gzqyjreduce",gzqyjreduce.Text));
                list.Add(new SqlPara("gzkgdays",gzkgdays.Text));
                list.Add(new SqlPara("gzkgreduce",gzkgreduce.Text));
                list.Add(new SqlPara("gzborrow",gzborrow.Text));
                list.Add(new SqlPara("gzyanglao",gzyanglao.Text));
                list.Add(new SqlPara("gzsuodeshui",gzsuodeshui.Text));
                list.Add(new SqlPara("gzfactout",gzfactout.Text));
                list.Add(new SqlPara("gzid",gzid));




                string km1 = "", km2 = "", km3 = "", km4 = "";
                //if (checkEdit1.Checked)
                //{
                //    cc.getkmforhexiao("工资发放", ref km1, ref km2, ref km3, ref km4);
                //    w_input_ticketno witck = new w_input_ticketno();
                //    witck.km1 = km1;
                //    witck.km2 = km2;
                //    witck.km3 = km3;
                //    witck.edcontent.Text = lookUpEdit1.Text.Trim();
                //    if (witck.ShowDialog() == DialogResult.Cancel)
                //        return;
                //}
                
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute,"USP_ADD_GZOUT",list);
                SqlHelper.ExecteNonQuery(sps);
                //if (checkEdit1.Checked)
                //{
                //    gzid = getgzid();
                //    if (!cc.AaddAccount(this, commonclass.xm, commonclass.km1, commonclass.km2, commonclass.km3, commonclass.km4, commonclass.gcdate, GetDouble(gzfactout.Text), "支出", "工资发放", gzid + "@", GetDouble(gzfactout.Text) + "@", "0@", "无@", 1))
                //    {
                //        return;
                //    }
                //    else
                //    {
                //        SqlCommand command = new SqlCommand("USP_UPDATE_YG_GZOUT_GZID");
                //        command.Parameters.Add(new SqlParameter("@gzid", SqlDbType.Int));
                //        command.Parameters[0].Value = gzid;
                //        command.CommandType = CommandType.StoredProcedure;
                //        cs.ENQ(command);
                //    }
                //}
                //commonclass.ShowOK();
                Clear();
                lookUpEdit1.Properties.ReadOnly = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                    
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void Clear()
        {
            foreach (Control c in panelControl1.Controls)
            {
                if (c.GetType() == typeof(DevExpress.XtraEditors.TextEdit) || c.GetType() == typeof(DevExpress.XtraEditors.LookUpEdit))
                {
                    c.Text = "";
                }
            }
        }

        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                textEdit1.EditValue = lookUpEdit1.EditValue;
                textEdit2.EditValue = lookUpEdit1.GetColumnValue("fgs");
                textEdit3.EditValue = lookUpEdit1.GetColumnValue("bm");

                get_gzori();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private decimal GetDouble(string str)
        {
            return str.Trim() == "" ? 0 : Math.Round(Convert.ToDecimal(str), 2);
        }

        private void getfactout(object sender, EventArgs e)
        {
            try
            {
                gzfactout.Text = Convert.ToString(GetDouble(gzori.Text) + GetDouble(gzpro.Text) + GetDouble(gzqqj.Text) + GetDouble(gzfb.Text) + GetDouble(gzhb.Text) + GetDouble(gzqita.Text) + GetDouble(gzjb.Text) + GetDouble(gztel.Text) - GetDouble(gzdf.Text) - GetDouble(gzqyjreduce.Text) - GetDouble(gzkgreduce.Text) - GetDouble(gzborrow.Text) - GetDouble(gzyanglao.Text) - GetDouble(gzsuodeshui.Text));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "蓝桥提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 获取基本工资
        /// </summary>
        private void get_gzori()
        {
            
            try
            {
                //SqlCommand da = new SqlCommand("QSP_GET_GZORI_BH");
                //da.CommandType = CommandType.StoredProcedure;
                //da.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar));
                //da.Parameters["@bh"].Value = lookUpEdit1.EditValue;
                //DataSet ds = new DataSet();
                //ds = cs.GetDataSet(da, null); if (ds == null) return;


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bh",lookUpEdit1.EditValue));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GZORI_BH", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    gzori.EditValue = ds.Tables[0].Rows[0][0];
                }
                else
                {
                    gzori.Text = "";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "蓝桥提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gzjb_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gzfactout.Text = Convert.ToString(GetDouble(gzori.Text) + GetDouble(gzpro.Text) + GetDouble(gzqqj.Text) + GetDouble(gzfb.Text) + GetDouble(gzhb.Text) + GetDouble(gzqita.Text) + GetDouble(gzjb.Text) + GetDouble(gztel.Text) - GetDouble(gzdf.Text) - GetDouble(gzqyjreduce.Text) - GetDouble(gzkgreduce.Text) - GetDouble(gzborrow.Text) - GetDouble(gzyanglao.Text) - GetDouble(gzsuodeshui.Text));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "蓝桥提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gzjbdays_TextChanged(object sender, EventArgs e)
        {

            if (edyear.Text.Trim() == "")
            {
                edyear.Text = CommonClass.gcdate.Year.ToString();
            }
            if (edmonth.Text.Trim() == "")
            {
                edmonth.Text = CommonClass.gcdate.Month.ToString();
            }

            int t = DateTime.DaysInMonth(Convert.ToInt32(edyear.Text.Trim()), Convert.ToInt32(edmonth.Text.Trim()));  //指定年和月的天数
            gzjb.Text = Convert.ToString(Math.Round(GetDouble(gzori.Text) * GetDouble(gzjbdays.Text) / t, 2));
        }

        private void gzqjdays_TextChanged(object sender, EventArgs e)
        {

            if (edyear.Text.Trim() == "")
            {
                edyear.Text = CommonClass.gcdate.Year.ToString();
            }
            if (edmonth.Text.Trim() == "")
            {
                edmonth.Text = CommonClass.gcdate.Month.ToString();
            }

            int t = DateTime.DaysInMonth(Convert.ToInt32(edyear.Text.Trim()), Convert.ToInt32(edmonth.Text.Trim()));  //指定年和月的天数
            gzqyjreduce.Text = Convert.ToString(Math.Round(GetDouble(gzori.Text) * GetDouble(gzqjdays.Text) / t, 2));
        }

        private void gzkgdays_TextChanged(object sender, EventArgs e)
        {

            if (edyear.Text.Trim() == "")
            {
                edyear.Text = CommonClass.gcdate.Year.ToString();
            }
            if (edmonth.Text.Trim() == "")
            {
                edmonth.Text = CommonClass.gcdate.Month.ToString();
            }

            int t = DateTime.DaysInMonth(Convert.ToInt32(edyear.Text.Trim()), Convert.ToInt32(edmonth.Text.Trim()));  //指定年和月的天数
            gzkgreduce.Text = Convert.ToString(Math.Round(GetDouble(gzori.Text) * GetDouble(gzkgdays.Text) / t, 2));
        }

    }
}