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
using DevExpress.XtraGrid.Views.Grid;
using System.Collections;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_yg_work : BaseForm
    {
        //commonclass cc = new commonclass();
        //Cls_SqlHelper cs = new Cls_SqlHelper();
        public w_yg_work()
        {
            InitializeComponent();
        }
        public int id = 2; //从2开始 根据gridview序号

        GridView gv = new GridView(); //MainView
        string layout = "";//主表样式以及标题
        string proc = ""; //明细过程
        string table = "";

        DataSet zwds = new DataSet();

        private void getdata()
        {
            try
            {
                DataSet ds = new DataSet();

                //SqlCommand sq = new SqlCommand("QSP_GET_YGDA_TOTAL_EX");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@site", SqlDbType.VarChar).Value = comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim();
                //ds = cs.GetDataSet(sq, gridControl1);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("site", comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim()));
                ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_TOTAL_EX", list));
                if (ds == null || ds.Tables.Count == 0) return;
                gridControl1.DataSource = ds.Tables[0];
                // gridControl1.DataSource = FilterDs(ds);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 没有权限的将不会显示
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private DataTable FilterDs(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0) return null;
            if (zwds == null || zwds.Tables.Count == 0) return ds.Tables[0];

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string zw = ConvertType.ToString(ds.Tables[0].Rows[i]["zw"]);
                int k = 0;
                for (int j = 0; j < zwds.Tables[0].Rows.Count; j++)
                {
                    string xm = zwds.Tables[0].Rows[j]["xm"].ToString();
                    if (xm == zw)
                    {
                        k += 1;
                    }
                }

                if (k == 0)
                {
                    ds.Tables[0].Rows.RemoveAt(i);
                    i--;
                }
                k = 0;
            }

            return ds.Tables[0];
        }

        private void initial()
        {
            switch (id)
            {
                case 2:
                    gv = gridView2;
                    proc = "QSP_GET_YG_WORK";
                    layout = "工作经历";
                    table = "b_yg_work";
                    break;
                case 3:
                    gv = gridView3;
                    proc = "QSP_GET_YG_QJ";
                    layout = "员工考勤";
                    table = "b_yg_qj";
                    break;
                case 4:
                    gv = gridView4;
                    proc = "QSP_GET_YG_PX";
                    layout = "培训登记";
                    table = "b_yg_px";
                    break;
                case 5:
                    gv = gridView5;
                    proc = "QSP_GET_YG_GS";
                    layout = "工伤申报";
                    table = "b_yg_gs";
                    break;
                case 6:
                    gv = gridView6;
                    proc = "QSP_GET_YG_TRUN";
                    layout = "晋级调动";
                    table = "b_yg_trun";
                    break;
                case 7:
                    gv = gridView7;
                    proc = "QSP_GET_YG_JC";
                    layout = "奖惩登记";
                    table = "b_yg_jc";
                    break;
                case 8:
                    gv = gridView8;
                    proc = "QSP_GET_YG_KP";
                    layout = "绩效考评";
                    table = "b_yg_kp";
                    getdatadetail();


                    break;
                case 9:
                    gv = gridView9;
                    proc = "QSP_GET_YG_YYE";
                    layout = "营业目标";
                    table = "b_yg_yye";
                    break;
            }
        }


        private void getdatadetail()
        {
            //SqlConnection con = cc.GetConn();
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_USER_ZW");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add("@username", SqlDbType.VarChar).Value = commonclass.username;
                //SqlDataAdapter ada = new SqlDataAdapter(sq);
                //ada.Fill(zwds);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("username",CommonClass.UserInfo.UserName));
                //zwds = cs.GetDataSet(sq, new DevExpress.XtraGrid.GridControl());
                zwds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_USER_ZW", list));
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("工作记录");//xj/2019/5/29
            initial();
            gridControl2.MainView = gv;
            this.Text = layout;
            gv.GroupPanelText = layout + "记录";
            //fixcolumn fc = new fixcolumn(gridView1, barSubItem2);//动结列
            //cc.LoadScheme(gridControl1);//网格外观
            //cc.LoadScheme(gridControl2);
            BarMagagerOper.SetBarPropertity(bar3);//工具
            GridOper.RestoreGridLayout(gridView1, layout);

            dateEdit1.EditValue = CommonClass.gbdate;
            dateEdit2.EditValue = CommonClass.gedate;

            //cc.FillRepCombox(comboBoxEdit2);
            CommonClass.SetSite(comboBoxEdit2,true);
            //comboBoxEdit2.Properties.Items.Add("全部");
            comboBoxEdit2.Text = CommonClass.UserInfo.SiteName;

            //getdata();
            setright();
        }

        private void setright()
        {
            //userright ur = new userright();


            //switch (id)
            //{
            //    case 2:

            //        //layout = "员工经历";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10201");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10202");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10203");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10205");
            //        break;
            //    case 3:
            //        //layout = "员工请假";
            //        //barButtonItem3.Enabled = ur.GetUserRightDetail999("d10201");
            //        //barButtonItem8.Enabled = ur.GetUserRightDetail999("d10202");
            //        //barButtonItem4.Enabled = ur.GetUserRightDetail999("d10203");
            //        //comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10205");
            //        break;
            //    case 4:

            //        //layout = "员工培训";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10401");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10402");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10403");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10405");
            //        break;
            //    case 5:
            //        //layout = "员工工伤";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10501");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10502");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10503");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10505");
            //        break;
            //    case 6:
            //        //layout = "员工调动";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10601");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10602");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10603");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10605");
            //        break;
            //    case 7:
            //        //layout = "员工奖惩";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10701");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10702");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10703");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10705");
            //        break;
            //    case 8:
            //        //layout = "员工考评";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10801");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10802");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10803");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10805");
            //        break;
            //    case 9:
            //        //layout = "营业额目标";
            //        barButtonItem3.Enabled = ur.GetUserRightDetail999("d10901");
            //        barButtonItem8.Enabled = ur.GetUserRightDetail999("d10902");
            //        barButtonItem4.Enabled = ur.GetUserRightDetail999("d10903");
            //        comboBoxEdit2.Enabled = ur.GetUserRightDetail999("d10905");
            //        break;
            //}
        }



        private void oper2(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_work_add wa = new w_yg_work_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
            
        }

        private void oper3(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_qj_add wa = new w_yg_qj_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void oper4(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_px_add wa = new w_yg_px_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void oper5(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_gs_add wa = new w_yg_gs_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void oper6(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_trun_add wa = new w_yg_trun_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void oper7(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_jc_add wa = new w_yg_jc_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void oper8(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }

            string zw = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "zw").ToString();
            string zwtype = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "type").ToString();
            int k = 0;
            for (int i = 0; i < zwds.Tables[0].Rows.Count; i++)
            {
                string xm = zwds.Tables[0].Rows[i]["xm"].ToString();
                if (xm == zw)
                {
                    k += 1;
                }
            }

            //if (k == 0)
            //{
            //    commonclass.MsgBox.ShowOK("没有权限做" + zw + "的考核");
            //    return;
            //}


            w_yg_kp_add wa = new w_yg_kp_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.zwtype = zwtype;

            wa.ShowDialog();
        }

        private void oper9(int type) //0新增 1修改
        {
            if (type == 1)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle < 0) return;
            }
            w_yg_yye_add wa = new w_yg_yye_add();
            wa.gv1 = gridView1;
            wa.gv2 = gv;
            wa.oper = type == 0 ? "NEW" : "MODIFY";
            wa.ShowDialog();
        }

        private void GetDetail(string bh)
        {
            //SqlConnection connection = cc.GetConn();
            try
            {
                //if (connection.State == ConnectionState.Closed) connection.Open();
                //DataSet dataset = new DataSet();
                //SqlDataAdapter da = new SqlDataAdapter();
                //SqlCommand sq = new SqlCommand(proc, connection);
                //sq.Parameters.Add(new SqlParameter("@bh", SqlDbType.VarChar));
                //sq.Parameters[0].Value = bh;
                //sq.Parameters.Add("@t1", SqlDbType.DateTime).Value = dateEdit1.DateTime;
                //sq.Parameters.Add("@t2", SqlDbType.DateTime).Value = dateEdit2.DateTime;

                //sq.CommandType = CommandType.StoredProcedure;
                ////da.SelectCommand = sq;
                ////dataset.Clear();
                ////da.Fill(dataset);
                ////if (connection.State == ConnectionState.Open) connection.Close();
                ////gridControl2.DataSource = dataset;
                ////gridControl2.DataMember = dataset.Tables[0].ToString();
                //dataset = cs.GetDataSet(sq, gridControl2);
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bh",bh));
                list.Add(new SqlPara("t1", dateEdit1.DateTime));
                list.Add(new SqlPara("t2", dateEdit2.DateTime));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query,proc,list));
                gridControl2.DataSource = ds.Tables[0];

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_Click(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;

            if (rowhandle >= 0)
            {
                string bh = gridView1.GetRowCellValue(rowhandle, "bh").ToString();
                string xm = gridView1.GetRowCellValue(rowhandle, "xm").ToString();
                gv.GroupPanelText = layout + "记录  【" + xm + "】";
                GetDetail(bh);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (id == 2)
            {
                oper2(0);
                gridView1_Click(null,null);
                return;
            }
            if (id == 3)
            {
                oper3(0);
                gridView1_Click(null, null);
                return;
            }
            if (id == 4)
            {
                oper4(0);
                gridView1_Click(null, null);
                return;
            }
            if (id == 5)
            {
                oper5(0);
                gridView1_Click(null, null);
                return;
            }
            if (id == 6)
            {
                oper6(0);
                gridView1_Click(null, null);
                return;
            }
            if (id == 7)
            {
                oper7(0);
                gridView1_Click(null, null);
                return;
            }
            if (id == 8)
            {
                oper8(0);
                gridView1_Click(null,null);
                return;
            }
            if (id == 9)
            {
                oper9(0);
                gridView1_Click(null, null);
                return;
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (id == 2)
            {
                oper2(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 3)
            {
                oper3(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 4)
            {
                oper4(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 5)
            {
                oper5(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 6)
            {
                oper6(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 7)
            {
                oper7(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 8)
            {
                oper8(1);
                gridView1_Click(null, null);
                return;
            }
            if (id == 9)
            {
                oper9(1);
                gridView1_Click(null, null);
                return;
            }
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gv.FocusedRowHandle;
            //SqlConnection connection = cc.GetConn();
            if (rowhandle >= 0)
            {
                decimal id = Convert.ToDecimal(gv.GetRowCellValue(rowhandle, "id"));
                if (XtraMessageBox.Show("确认要删除该笔记录吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    //SqlCommand sq = new SqlCommand("USP_DELETE_YGDA_LAYOUT");
                    //sq.CommandType = CommandType.StoredProcedure;
                    //sq.Parameters.Add(new SqlParameter("@id", SqlDbType.Decimal));
                    //sq.Parameters.Add(new SqlParameter("@layout", SqlDbType.VarChar));
                    //sq.Parameters[0].Value = id;
                    //sq.Parameters[1].Value = layout;
                    //cs.ENQ(sq);
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("id",id));
                    list.Add(new SqlPara("layout", layout));
                    SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_DELETE_YGDA_LAYOUT", list));

                    gv.DeleteRow(rowhandle);
                }
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, layout);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, layout);
        }

        private void barCheckItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barCheckItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBoxEdit1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit1.Text.Trim() == "工作经历")
                id = 2;
            if (comboBoxEdit1.Text.Trim() == "员工考勤")
                id = 3;
            if (comboBoxEdit1.Text.Trim() == "培训登记")
                id = 4;
            if (comboBoxEdit1.Text.Trim() == "工伤申报")
                id = 5;
            if (comboBoxEdit1.Text.Trim() == "晋级调动")
                id = 6;
            if (comboBoxEdit1.Text.Trim() == "奖惩登记")
                id = 7;
            if (comboBoxEdit1.Text.Trim() == "绩效考评")
                id = 8;
            if (comboBoxEdit1.Text.Trim() == "营业目标")
                id = 9;
            initial();
            this.Text = layout;
            gridControl2.MainView = gv;
            gv.GroupPanelText = layout + "记录";
        }
    }

}