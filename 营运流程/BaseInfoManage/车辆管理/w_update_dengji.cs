﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_update_dengji : BaseForm
    {
        private DataTable dt1 = null;
        private DataTable dt2 = new DataTable();
        bool isclick = false;
        private string unit = string.Empty;

        public w_update_dengji()
        {
            InitializeComponent();
        }

        public w_update_dengji(string wxunit)
        {
            InitializeComponent();
            this.unit = wxunit;
        }

        private void getwxinfo()
        {
            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("wxunit", unit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_WX_INFO", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            this.wxunit.Text = dt.Rows[0]["wxunit"].ToString();
            this.repairdate.EditValue = Convert.ToDateTime(dt.Rows[0]["wxdate"]);
            this.repairno.Text = dt.Rows[0]["vehicleno"].ToString();
            this.repairchang.Text = dt.Rows[0]["gyname"].ToString();
            this.accpeijian.Text = dt.Rows[0]["accpj"].ToString();
            this.accweifu.Text = dt.Rows[0]["accweifu"].ToString();
            this.accman.Text = dt.Rows[0]["accman"].ToString();
            this.accnow.Text = dt.Rows[0]["accnow"].ToString();
            this.accnowzhanghu.Text = dt.Rows[0]["accnowzh"].ToString();
            this.accyifu.Text = dt.Rows[0]["accyifu"].ToString();
            this.accweifu.Text = dt.Rows[0]["accweifu"].ToString();
            this.remark.Text = dt.Rows[0]["remark"].ToString();
            this.madeby.Text = CommonClass.UserInfo.UserName;
            this.repairman.Text = dt.Rows[0]["wxman"].ToString();

        }

        private void createcolumn()
        {
            dt2.Columns.Clear();
            DataColumn dc1 = new DataColumn("pjn", typeof(string));
            DataColumn dc2 = new DataColumn("count", typeof(int));
            DataColumn dc3 = new DataColumn("price", typeof(decimal));
            DataColumn dc4 = new DataColumn("money", typeof(decimal));
            dt2.Columns.Add(dc1);
            dt2.Columns.Add(dc2);
            dt2.Columns.Add(dc3);
            dt2.Columns.Add(dc4);
        }


        private void getwxdetail()
        {

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("wxunit", unit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_WX_DETAIL", list2);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = ds.Tables[0];
            try
            {
                createcolumn();
                dt2.Rows.Clear();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow newRow = dt2.NewRow();
                    newRow["pjn"] = dt.Rows[j]["pjname"];
                    newRow["count"] = dt.Rows[j]["count"];
                    newRow["price"] = dt.Rows[j]["price"];
                    newRow["money"] = dt.Rows[j]["money"];
                    dt2.Rows.Add(newRow);
                }
                this.gcwxdetaillist.DataSource = dt2;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void w_update_dengji_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar2); 

            getwxinfo();
            getwxdetail();
            bindcomboboxedit();
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmAddCars info = new frmAddCars();
            info.ShowDialog();
            bindcomboboxedit();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            w_gongying_info info = new w_gongying_info();
            info.ShowDialog();
            bindcomboboxedit();
        }

        private void exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //绑定数据到车号，修理厂列表框
        private void bindcomboboxedit()
        {
            this.repairno.Properties.Items.Clear();
            this.repairchang.Properties.Items.Clear();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO");
            DataSet ds1 = SqlHelper.GetDataSet(sps);

            List<SqlPara> list2 = new List<SqlPara>();
            list2.Add(new SqlPara("gytype", "修理厂"));
            SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "V_GET_GYTYPE", list2);
            DataSet ds2 = SqlHelper.GetDataSet(sps1);


            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@gytype", "修理厂") };
            DataTable dt1 = ds1.Tables[0];
            DataTable dt2 = ds2.Tables[0];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                this.repairno.Properties.Items.Add(dt1.Rows[i]["vehicleno"]);
            }
            for (int j = 0; j < dt2.Rows.Count; j++)
            {
                this.repairchang.Properties.Items.Add(dt2.Rows[j]["gyname"]);
            }
        }

        private void saveadd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (check())
            {
                save();
            }
        }
        private void save()
        {
            string peijians = string.Empty;
            try
            {

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    peijians = peijians + dt2.Rows[i]["pjn"].ToString();
                    if (i + 1 != dt2.Rows.Count)
                    {
                        peijians = peijians + "，";
                    }
                }
                string accnow, yifu, weifu,accrengong;
                if (this.accnow.Text.Trim() == "") accnow = "0"; else accnow = this.accnow.Text.Trim();
                if (this.accyifu.Text.Trim() == "") yifu = "0"; else yifu = this.accyifu.Text.Trim();
                if (this.accweifu.Text.Trim() == "") weifu = "0"; else weifu = this.accweifu.Text.Trim();
                if (this.accman.Text.Trim() == "") accrengong = "0"; else accrengong = this.accman.Text.Trim();

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("wxunit", wxunit.Text.Trim()));
                list.Add(new SqlPara("wxdate", repairdate.DateTime));
                list.Add(new SqlPara("vehicleno", repairno.Text.Trim()));
                list.Add(new SqlPara("gyname", repairchang.Text.Trim()));
                list.Add(new SqlPara("wxpj", peijians));
                list.Add(new SqlPara("accpj", accpeijian.Text.Trim()));
                list.Add(new SqlPara("accman", accrengong));
                list.Add(new SqlPara("accnow", accnow));
                list.Add(new SqlPara("accnowzh", accnowzhanghu.Text.Trim()));
                list.Add(new SqlPara("accyifu", yifu));
                list.Add(new SqlPara("accweifu", weifu));
                list.Add(new SqlPara("wxman", repairman.Text.Trim()));
                list.Add(new SqlPara("remark", remark.Text.Trim()));
                list.Add(new SqlPara("oldwxunit", unit));
                list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("updatedate", DateTime.Now));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_UPDATE_WX_DJ", list);
                int row = SqlHelper.ExecteNonQuery(sps);
                int row2 = 0;
                for (int i = 0; i < dt2.Rows.Count; i++)
                {

                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("wxunit", wxunit.Text.Trim()));
                    list1.Add(new SqlPara("pjguige", dt2.Rows[i]["pjn"]));
                    list1.Add(new SqlPara("count",dt2.Rows[i]["count"]));
                    list1.Add(new SqlPara("price", dt2.Rows[i]["price"]));
                    list1.Add(new SqlPara("money", dt2.Rows[i]["money"]));
                  
                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "V_ADD_WX_DETAIL", list1);
                     row2 = SqlHelper.ExecteNonQuery(sps1);
                }
                if (row2 > 0)
                {
                    MsgBox.ShowOK();
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool check()
        {
            if (this.wxunit.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入维修单号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            else if (this.repairno.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择车号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.repairchang.Text.Trim() == "")
            {
                XtraMessageBox.Show("请选择维修厂", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.repairdate.DateTime > DateTime.Now)
            {
                XtraMessageBox.Show("维修时间不能大于当前时间", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.accpeijian.Text.Trim() != "" && !StringHelper.IsDecimal(accpeijian.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的配件费用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            else if (this.accman.Text.Trim() != "" && !StringHelper.IsDecimal(accman.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的人工费用", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.repairman.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入维修人员", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            else if (this.accnow.Text.Trim() != "" && !StringHelper.IsDecimal(accnow.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的本次现付", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            else if (this.accyifu.Text.Trim() != "" && !StringHelper.IsDecimal(accyifu.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的已付金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.accweifu.Text.Trim() != "" && !StringHelper.IsDecimal(accweifu.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效的未付金额", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (dt2 == null || dt2.Rows.Count == 0)
            {
                XtraMessageBox.Show("请添加维修明细", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (this.wxunit.Text.Trim() != unit && this.checkunit(this.wxunit.Text.Trim()))
            {
                XtraMessageBox.Show("该维修单号已存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;

        }

        private bool checkunit(string wxunit)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("wxunit", wxunit));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_CHECK_WXUNIT", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            
            if (ds!=null&&ConvertType.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
            {
                return true;
            }
            return false;
        }

        private void getData()
        {
            dt1 = new DataTable();
           DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "V_GET_ALL_PJ"));
           dt1 = ds.Tables[0];
            this.gcpjlist.DataSource = dt1;
        }

        private void MMove(DevExpress.XtraGrid.Views.Grid.GridView gv)
        {
            // 从一个网格移动数据行到另外的网格
            // gv  源gridview
            // ds  源dataset
            // ds2 目的dataset
            // tbid dataset 中的表格顺序
            // createby duanzhiyu
            try
            {
                if (!isclick)
                {
                    dt2.Rows.Clear();
                }
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j].RowState != DataRowState.Deleted)
                    {
                        for (int k = 0; k < dt2.Rows.Count; k++)
                        {
                            if (dt2.Rows[k]["pjn"].ToString() == dt1.Rows[j]["pjname"].ToString())
                            {
                                return;
                            }
                        }
                        DataRow newRow = dt2.NewRow();
                        newRow["pjn"] = dt1.Rows[j]["pjname"];
                        newRow["count"] = 1;
                        newRow["price"] = dt1.Rows[j]["jhprice"];
                        newRow["money"] = Convert.ToInt32(newRow["count"]) * Convert.ToDecimal(newRow["price"]);
                        dt2.Rows.Add(newRow);
                        dt1.Rows[j]["nowcount"] = Convert.ToInt32(dt1.Rows[j]["nowcount"]) - 1;
                    }
                }
                gv.DeleteSelectedRows();
                isclick = true;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void MMove(DevExpress.XtraGrid.Views.Grid.GridView gv, string pjname)
        {
            // 从一个网格移动数据行到另外的网格
            // gv  源gridview
            // ds  源dataset
            // ds2 目的dataset
            // tbid dataset 中的表格顺序
            // createby duanzhiyu
            try
            {
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j].RowState != DataRowState.Deleted)
                    {
                        if (dt1.Rows[j]["pjname"].ToString() == pjname)
                        {
                            for (int k = 0; k < dt2.Rows.Count; k++)
                            {
                                if (dt2.Rows[k]["pjn"].ToString() == pjname)
                                {
                                    //DataRow newRow = dt2.NewRow();
                                    //newRow["pjn"] = pjname;
                                    //newRow["count"] = (Convert.ToInt32(dt2.Rows[k]["count"]) + 1)as object;
                                    //newRow["price"] = dt2.Rows[j]["price"];
                                    //newRow["money"] = Convert.ToInt32(newRow["count"]) * Convert.ToDecimal(newRow["price"]);
                                    //dt2.Rows.Add(newRow);
                                    //this.gridView2.DeleteRow(k);
                                    return;
                                }
                            }
                            DataRow newRow2 = dt2.NewRow();
                            newRow2["pjn"] = pjname;
                            newRow2["count"] = 1;
                            newRow2["price"] = dt1.Rows[j]["jhprice"];
                            newRow2["money"] = Convert.ToInt32(newRow2["count"]) * Convert.ToDecimal(newRow2["price"]);
                            dt2.Rows.Add(newRow2);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

     
        private void move()
        {
            int[] r = this.gridView1.GetSelectedRows();
            if (r.Length == 0) return;
            for (int i = 0; i < r.Length; i++)
            {
                string pjname = this.gridView1.GetRowCellValue(r[i], "pjname").ToString();
                this.MMove(this.gridView1, pjname);
            }

            getaccpeijian();
            isclick = true;
            gridView1.DeleteSelectedRows();
        }

        private void gcpjlist_DragDrop(object sender, DragEventArgs e)
        {
            move();
        }

        private void getmoney()
        {
            int no = this.gridView2.FocusedRowHandle;
            if (no < 0) return;
            decimal money = Convert.ToInt32(this.gridView2.GetRowCellValue(no, "count").ToString())
            *
            Convert.ToDecimal(this.gridView2.GetRowCellValue(no, "price").ToString());
            this.gridView2.SetRowCellValue(no, "money", (object)money);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            w_add_pj addpj = new w_add_pj();
            addpj.ShowDialog();
            getData();
        }

        private void barButtonItem34_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gridView1.SelectAll();
            this.MMove(gridView1);
            getaccpeijian();
        }

        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            move();
        }
        private void gcpjlist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            move();
        }

        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isclick = false;
            getData();
        }

        private void barButtonItem31_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            w_add_pj addpj = new w_add_pj();
            addpj.ShowDialog();
            getData();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            int no = this.gridView1.FocusedRowHandle;
            string pjname = this.gridView1.GetRowCellValue(no, "pjname").ToString();
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("pjname", pjname));
            DataTable dt = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "V_GET_PJINFO", list)).Tables[0];
            w_update_pj updatepj = new w_update_pj(dt.Rows[0]["pjname"].ToString(), dt.Rows[0]["units"].ToString()
                , Convert.ToDecimal(dt.Rows[0]["jhprice"]), Convert.ToInt32(dt.Rows[0]["nowcount"]));
            updatepj.ShowDialog();
            getData();
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            if (XtraMessageBox.Show("确定要删除吗？", "系统提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                int no = this.gridView1.FocusedRowHandle;
                string pjname = this.gridView1.GetRowCellValue(no, "pjname").ToString();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("pjname", pjname));
                int row = SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "V_DELETE_PJ", list));
                this.gridView1.DeleteRow(no);
            }
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gridView2.SelectedRowsCount == 0)
            {
                return;
            }
            int no = this.gridView2.FocusedRowHandle;
            this.gridView2.DeleteRow(no);
            getaccpeijian();
        }

        private void getaccpeijian()
        {
            decimal accpeijian = 0;
            for (int i = 0; i < gridView2.RowCount; i++)
            {
                accpeijian = accpeijian + Convert.ToDecimal(this.gridView2.GetRowCellValue(i, "money"));
            }
            this.accpeijian.Text = accpeijian.ToString();
        }

        private void accpeijian_EditValueChanged(object sender, EventArgs e)
        {
            getaccnow();
        }

        private void getaccnow()
        {
            this.accnow.Text = "";
            string rengong = accman.Text.Trim() == "" ? "0" : accman.Text.Trim();
            string peijian = accpeijian.Text.Trim() == "" ? "0" : accpeijian.Text.Trim();
            if (StringHelper.IsDecimal(rengong) && StringHelper.IsDecimal(peijian))
            {
                this.accnow.Text = (Convert.ToDecimal(rengong) + Convert.ToDecimal(peijian)).ToString();
            }
        }

        private void accman_EditValueChanged(object sender, EventArgs e)
        {
            getaccnow();
        }

        private void accyifu_EditValueChanged(object sender, EventArgs e)
        {
            this.accweifu.EditValueChanged -= new System.EventHandler(this.accweifu_EditValueChanged);
            string yifu = accyifu.Text.Trim() == "" ? "0" : accyifu.Text.Trim();
            string weifu = accweifu.Text.Trim() == "" ? "0" : accweifu.Text.Trim();
            string nowfu = accnow.Text.Trim() == "" ? "0" : accnow.Text.Trim();
            if (ConvertType.ToDecimal(nowfu) - ConvertType.ToDecimal(yifu) < 0)
            {
                this.accyifu.EditValueChanged -= new System.EventHandler(this.accyifu_EditValueChanged);
                accyifu.Text = nowfu;
                accyifu.Refresh();
                accweifu.Text = "";
                this.accyifu.EditValueChanged += new System.EventHandler(this.accyifu_EditValueChanged);
                this.accweifu.EditValueChanged += new System.EventHandler(this.accweifu_EditValueChanged);
                return;

            }
            if (StringHelper.IsDecimal(yifu) && StringHelper.IsDecimal(weifu) && StringHelper.IsDecimal(nowfu) && nowfu != "0")
            {
                this.accweifu.Text = (ConvertType.ToDecimal(nowfu) - ConvertType.ToDecimal(yifu)).ToString();
            }
            else
            {
                this.accweifu.Text = "";
            }
            this.accweifu.EditValueChanged += new System.EventHandler(this.accweifu_EditValueChanged);
        }
        private void accweifu_EditValueChanged(object sender, EventArgs e)
        {
            this.accyifu.EditValueChanged -= new System.EventHandler(this.accyifu_EditValueChanged);
            string yifu = accyifu.Text.Trim() == "" ? "0" : accyifu.Text.Trim();
            string weifu = accweifu.Text.Trim() == "" ? "0" : accweifu.Text.Trim();
            string nowfu = accnow.Text.Trim() == "" ? "0" : accnow.Text.Trim();
            if (Convert.ToDecimal(nowfu) - Convert.ToDecimal(weifu) < 0)
            {
                accweifu.Text = nowfu;
                accyifu.Text = "";
                accweifu.Refresh();
                this.accyifu.EditValueChanged += new System.EventHandler(this.accyifu_EditValueChanged); ;
                return;

            }
            if (StringHelper.IsDecimal(yifu) && StringHelper.IsDecimal(weifu) && StringHelper.IsDecimal(nowfu) && nowfu != "0")
            {
                this.accyifu.Text = (Convert.ToDecimal(nowfu) - Convert.ToDecimal(weifu)).ToString();
            }
            else
            {
                this.accyifu.Text = "";
            }
            this.accyifu.EditValueChanged += new System.EventHandler(this.accyifu_EditValueChanged);
        }

        private void gridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            gridView2.UpdateCurrentRow();
            gridView2.PostEditor();
            try
            {
                int rowhandle = gridView2.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (gridView2.FocusedColumn.FieldName == "count")
                {
                    int count = (e.Value == DBNull.Value || e.Value == null) ? 0 : Convert.ToInt32(e.Value);
                    decimal price = Convert.ToDecimal(gridView2.GetRowCellValue(rowhandle, "price"));
                    if (count == 0)
                    {
                        e.Valid = false;
                        e.ErrorText = "请输入大于0的数量";
                        XtraMessageBox.Show(e.ErrorText, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        gridView2.SetRowCellValue(rowhandle, money, count * price);
                    }
                    getaccpeijian();
                }
            }
            catch (DevExpress.Utils.HideException ee)
            {
                XtraMessageBox.Show(ee.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}