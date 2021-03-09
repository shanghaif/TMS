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
using DevExpress.XtraTreeList;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_gs_standard : BaseForm
    {
        DataSet ds_gz = new DataSet();
        private string fgs = "";
        
        public w_gs_standard()
        {
            InitializeComponent();
        }

        private void getdata()
        {
            
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_YGDA_GZ_ORI");
                //sq.CommandType = CommandType.StoredProcedure;
                
                //sq.Parameters.Add(new SqlParameter("@fgs", SqlDbType.VarChar));
                //sq.Parameters.Add(new SqlParameter("@gzyear", SqlDbType.Int));
                //sq.Parameters.Add(new SqlParameter("@gzmonth", SqlDbType.Int));


                //sq.Parameters[0].Value = "%" + fgs;
                //sq.Parameters[1].Value = Convert.ToInt32(edyear.Text);
                //sq.Parameters[2].Value = Convert.ToInt32(edmonth.Text);

                

                //ds_gz.Clear();
                //ds_gz = cs.GetDataSet(sq, null); if (ds_gz == null) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("fgs", "%" + fgs));
                list.Add(new SqlPara("gzyear", ConvertType.ToInt32(edyear.Text)));
                list.Add(new SqlPara("gzmonth", ConvertType.ToInt32(edmonth.Text)));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_YGDA_GZ_ORI", list);
                ds_gz.Clear();
                ds_gz = SqlHelper.GetDataSet(sps);

                if (ds_gz == null) return;


                if (!ds_gz.Tables[0].Columns.Contains("ischecked")) ds_gz.Tables[0].Columns.Add("ischecked");
                for (int i = 0; i < ds_gz.Tables[0].Rows.Count; i++)
                {
                    ds_gz.Tables[0].Rows[i]["ischecked"] = 0;
                }
                gridControl1.DataSource = ds_gz;
                gridControl1.DataMember = ds_gz.Tables[0].ToString();

            }
            catch (Exception ex)
            {
                 
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void w_cygs_Load(object sender, EventArgs e)
        {
            //fixcolumn fc = new fixcolumn(gridView1, barSubItem2);//动结列
            //cc.LoadScheme(gridControl1);//网格外观
            //commonclass.SetBarPropertity(bar3);//工具
            //cc.RestoreGridLayout(gridControl1, "员工工资标准");

            CommonClass.FormSet(this);
            GridOper.RestoreGridLayout(gridView1, this.Text);
            BarMagagerOper.SetBarPropertity(bar3); 
            edyear.Text = CommonClass.gcdate.Year.ToString();
            edmonth.Text = CommonClass.gcdate.Month.ToString();
            BuildTree();
            edgs.Text = CommonClass.UserInfo.SiteName;
        }
        
        private void modify()
        {
            int row = gridView1.FocusedRowHandle;
            if (row < 0) return;
            string bh = gridView1.GetRowCellValue(row, "bh").ToString();
            int gzid = Convert.ToInt32(gridView1.GetRowCellValue(row, "gzid"));
            int gzstate = Convert.ToInt32(gridView1.GetRowCellValue(row, "gzstate"));
            if (gzstate == 1)
            {
                XtraMessageBox.Show("已经核销，无法修改!\r\n\r\n如果确实需要修改，请先反核销!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            w_yg_gz_add wya = new w_yg_gz_add();
            wya.bh = bh;
            wya.gzid = gzid;
            wya.gv = gridView1;
            wya.oper = "MODIFY";
            wya.ShowDialog();
        }

        private void BuildTree()
        {
            treeList1.ClearNodes();
            
            try
            {
                //SqlCommand sq = new SqlCommand("QSP_GET_BM_BSITE");
                //sq.Parameters.Add(new SqlParameter("@bsite", SqlDbType.VarChar));
                //sq.Parameters[0].Value = commonclass.gbsite;
                //sq.CommandType = CommandType.StoredProcedure;
                //DataSet ds = new DataSet();
                //ds = cs.GetDataSet(sq, null); if (ds == null) return;
                //treeList1.DataSource = ds.Tables[0];

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", "%" + CommonClass.UserInfo.SiteName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BM_BSITE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if(ds==null || ds.Tables[0].Rows.Count == 0) return;
                treeList1.DataSource = ds_gz.Tables[0];



            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            modify();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            getdata();
        }        

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            GridOper.SaveGridLayout(gridView1, this.Text);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView1, this.Text);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            w_yg_gz_add wya = new w_yg_gz_add();
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
                try
                {
                    //工资和伙食补助可能分开发放，所以同一个月内，同一员工可能有两条或两条以上的记录，所以不能用bh,gzyear,gzmont做条件来删除
                    int gzid = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "gzid"));
                    string bh = gridView1.GetRowCellValue(rowhandle, "bh").ToString();
                    int gzstate = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "gzstate"));
                    if (gzstate == 1)
                    {
                        XtraMessageBox.Show("已经核销，无法删除!\r\n\r\n如果确实需要删除，请先反核销!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (XtraMessageBox.Show("确认要删除该笔记录吗?编号是：" + bh, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {

                        //SqlCommand sq = new SqlCommand("USP_DELETE_YG_GZOUT_GZID");
                        //sq.CommandType = CommandType.StoredProcedure;
                        //sq.Parameters.Add(new SqlParameter("@gzid", SqlDbType.Int));
                        //sq.Parameters[0].Value = gzid;
                        //cs.ENQ(sq);
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("gzid",gzid));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "", list);
                        SqlHelper.ExecteNonQuery(sps);
                         

                        gridView1.DeleteRow(rowhandle);
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            getdata();
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1,this.Text);
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void treeList1_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            if (e.Node.ParentNode == null) return;
            e.Appearance.ForeColor = Color.Blue;
        }

        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            if (treeList1.CalcHitInfo(new Point(e.X, e.Y)).HitInfoType != HitInfoType.Cell) return;
            DevExpress.XtraTreeList.Nodes.TreeListNode node = treeList1.FocusedNode;
            if (node == null) return;
            edgs.Text = node.GetDisplayText(0);
            edgs.ClosePopup();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            //int rowhandle = gridView1.FocusedRowHandle;
            //if (rowhandle < 0) return;
            
            
            ////SqlTransaction tran = connection.BeginTransaction();
            //try
            //{
            //    int gzid = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "gzid"));
            //    string bh = gridView1.GetRowCellValue(rowhandle, "bh").ToString();
            //    string xm = gridView1.GetRowCellValue(rowhandle, "xm").ToString();
            //    decimal gzfactout = Convert.ToDecimal(gridView1.GetRowCellValue(rowhandle, "gzfactout"));
            //    int gzstate = Convert.ToInt32(gridView1.GetRowCellValue(rowhandle, "gzstate"));
                
            //    if (gzstate == 1)
            //    {
            //        XtraMessageBox.Show("已经核销，无需再次核销!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    if (XtraMessageBox.Show("确认要核销该笔记录吗?编号是：" + bh, "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            //    {                    
            //        string km1 = "", km2 = "", km3 = "", km4 = "";
            //        cc.getkmforhexiao("工资发放", ref km1, ref km2, ref km3, ref km4);
            //        w_input_ticketno witck = new w_input_ticketno();
            //        witck.km1 = km1;
            //        witck.km2 = km2;
            //        witck.km3 = km3;
            //        witck.edcontent.Text = xm;
            //        if (witck.ShowDialog() == DialogResult.Cancel)
            //            return;

            //        if (!GridOper.AaddAccount(this, commonclass.xm, commonclass.km1, commonclass.km2, commonclass.km3, commonclass.km4, commonclass.gcdate, gzfactout, "支出", "工资发放", gzid + "@", gzfactout + "@", "0@", "无@", 1))
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            SqlCommand command = new SqlCommand("USP_UPDATE_YG_GZOUT_GZID");
            //            command.Parameters.Add(new SqlParameter("@gzid", SqlDbType.Int));
            //            command.Parameters[0].Value = gzid;
            //            command.CommandType = CommandType.StoredProcedure;
            //            cs.ENQ(command);
            //            gridView1.SetRowCellValue(rowhandle, "gzstate", 1);
            //        }
            //        commonclass.ShowOK();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
                 
            //}
        }

        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            ////打印全部
            //if (gridView1.RowCount == 0)
            //{
            //    XtraMessageBox.Show("请先提取数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //string gzid = "";
            //for (int i = 0; i < gridView1.RowCount; i++)
            //{
            //    gzid += gridView1.GetRowCellValue(i, "gzid").ToString() + ",";
            //}
            //w_ht_print ht = new w_ht_print();
            //ht.printtype = ht.Text = "打印工资条";
            //ht.gzid = gzid.TrimEnd(',');
            //ht.Show();
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        { //打印选择的
            //if (gridView1.RowCount == 0)
            //{
            //    XtraMessageBox.Show("请先提取数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //string gzid = "";
            //gridView1.PostEditor();
            //for (int i = 0; i < gridView1.RowCount; i++)
            //{
            //    if (Convert.ToInt32(gridView1.GetRowCellValue(i, "ischecked")) == 1)
            //    {
            //        gzid += gridView1.GetRowCellValue(i, "gzid").ToString() + ",";
            //    }
            //}
            //if (gzid.Trim() == "")
            //{
            //    XtraMessageBox.Show("没有选择任何记录。请先选择要打印的数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //w_ht_print ht = new w_ht_print();
            //ht.printtype = ht.Text = "打印工资条";
            //ht.gzid = gzid.TrimEnd(',');
            //ht.Show();
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            ////打印全部
            //if (gridView1.RowCount == 0)
            //{
            //    XtraMessageBox.Show("请先提取数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}
            //string gzid = "";
            //for (int i = 0; i < gridView1.RowCount; i++)
            //{
            //    gzid += gridView1.GetRowCellValue(i, "gzid").ToString() + ",";
            //}
            //w_ht_print ht = new w_ht_print();
            //ht.printtype = ht.Text = "打印工资表";
            //ht.gzid = gzid.TrimEnd(',');
            //ht.Show();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
        //    //打印选择的
        //    if (gridView1.RowCount == 0)
        //    {
        //        XtraMessageBox.Show("请先提取数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    string gzid = "";
        //    gridView1.PostEditor();
        //    for (int i = 0; i < gridView1.RowCount; i++)
        //    {
        //        if (Convert.ToInt32(gridView1.GetRowCellValue(i, "ischecked")) == 1)
        //        {
        //            gzid += gridView1.GetRowCellValue(i, "gzid").ToString() + ",";
        //        }
        //    }
        //    if (gzid.Trim() == "")
        //    {
        //        XtraMessageBox.Show("没有选择任何记录。请先选择要打印的数据!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //    w_ht_print ht = new w_ht_print();
        //    ht.printtype = ht.Text = "打印工资表";
        //    ht.gzid = gzid.TrimEnd(',');
        //    ht.Show();
        }

    }

}








