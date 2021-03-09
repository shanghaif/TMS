using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class p_BillInLog : BaseForm
    {
        public p_BillInLog()
        {
            InitializeComponent();
        }
        //commonclass cc = new commonclass();
        //加载
        private void BillListForm_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("票据入库");//xj/2019/5/29
            GridOper.RestoreGridLayout(gvBillList, "票据管理入库");
            BarMagagerOper.SetBarPropertity(bar3);
            FixColumn fix = new FixColumn(gvBillList, barSubItem3);
            dateEdit1.EditValue = CommonClass.gbdate;
            dateEdit2.EditValue = CommonClass.gedate;

            GetAllBillType();

            CommonClass.GetCompanyId(this.ckBox_Companyid);
            if (CommonClass.UserInfo.companyid != "101")
            {
                this.txt_Man.Visible = false;
            }
        }

        private void GetAllBillType()
        {
            try
            {
                cobeBillType.Properties.Items.Clear();
                cobeBillType.Properties.Items.Add("全部");

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OUT_ALLBillType");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cobeBillType.Properties.Items.Add(dr[0]);
                }

                cobeBillType.Text = "全部";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sbtnSearch_Click(object sender, EventArgs e)
        {
            DateTime btime = dateEdit1.DateTime;
            DateTime etime = dateEdit2.DateTime;
            string type = cobeBillType.Text.Trim() == "全部" ? "%%" : cobeBillType.Text.Trim();
            if (btime > etime)
            {
                XtraMessageBox.Show("开始日期不能大于结束日期", "日期选择错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("btime", btime.ToString()));
                list.Add(new SqlPara("etime", etime.ToString()));
                list.Add(new SqlPara("billType", type));
                list.Add(new SqlPara("inputMan", this.txt_Man.Text.Trim() == "" ? "%%" : this.txt_Man.Text.Trim()));
                list.Add(new SqlPara("companyids", this.ckBox_Companyid.Text.Trim() == ""?"": this.ckBox_Companyid.Text.Trim().Replace(" ","").Replace(',', '@') + "@"));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_TreasuryByTime", list);
                gcBillList.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            p_BillIn bif = new p_BillIn();
            bif.ShowDialog();
        }

        private void barbtnExit_Click(object sender, EventArgs e)
        {
            Close();
            Dispose();
        }

        private void d_ve(string str) { }

        public string type = "";
        private void barbtnAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            type = "add";
            p_BillIn bif = new p_BillIn(type);
            bif.type += new p_BillIn.deteype(d_ve);
            bif.ShowDialog();
            GetAllBillType();
        }

        private void UpdateBill()
        {
            try
            {
                int rowhandle = gvBillList.FocusedRowHandle;
                if (rowhandle < 0) return;
                if ((Convert.ToInt64(gvBillList.GetRowCellValue(rowhandle, "tBnum")) < Convert.ToInt64(GetBillNo(gvBillList.GetRowCellValue(rowhandle, "tBillType").ToString()))))
                {
                    XtraMessageBox.Show("该批次已有出库，不能再次修改！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                type = gvBillList.GetRowCellValue(rowhandle, "tId").ToString();
                p_BillIn bif = new p_BillIn(type);
                bif.type += new p_BillIn.deteype(d_ve);
                bif.ShowDialog();
                GetAllBillType();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void barbtnUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateBill();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            UpdateBill();
        }

        //删除
        private void barbtnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gvBillList.FocusedRowHandle;
                if (rowhandle < 0) return;
                if (Convert.ToInt64(gvBillList.GetFocusedRowCellValue("tEnum")) <= Convert.ToInt64(GetBillNo(gvBillList.GetFocusedRowCellValue("tBillType").ToString())))
                {
                    XtraMessageBox.Show("本批入库单据已经有出库，不能删除！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (XtraMessageBox.Show("是否确定要删除 ?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Int64 id = Convert.ToInt64(gvBillList.GetFocusedRowCellValue("tId"));
                    string companyid1 = gvBillList.GetFocusedRowCellValue("companyid").ToString();

                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("id", id));
                    list.Add(new SqlPara("companyid1", companyid1));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_DELETE_TreasuryById", list);
                   int k= SqlHelper.ExecteNonQuery(sps);
                   if (k > 0)
                   {
                       XtraMessageBox.Show("删除操作成功", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       gvBillList.DeleteRow(rowhandle);
                   }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //导出
        private void barbtnDerived_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gvBillList);
        }

        private void barbtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
            Dispose();
        }

        public Int64 GetBillNo(string billtype)
        {
            Int64 bno = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billType", billtype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_OUT_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                object result = ds.Tables[0].Rows[0][0];
                bno = result == DBNull.Value ? 0 : Convert.ToInt64(result);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return bno;
        }
        //控制行的颜色
        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                Int64 Bnum = Convert.ToInt64(gvBillList.GetRowCellDisplayText(e.RowHandle, gvBillList.Columns["tBnum"]));
                Int64 tEnum = Convert.ToInt64(gvBillList.GetRowCellDisplayText(e.RowHandle, gvBillList.Columns["tEnum"]));
                string billtype = gvBillList.GetRowCellDisplayText(e.RowHandle, gvBillList.Columns["tBillType"]).ToString();
                if (Bnum <= (GetBillNo(billtype) - 1) && (GetBillNo(billtype) - 1) < tEnum)
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#C0FFC0");//绿
                    return;
                }
                if ((GetBillNo(billtype) - 1) >= tEnum)
                {
                    e.Appearance.BackColor = ColorTranslator.FromHtml("#FF8080");//红
                    return;
                }
                if ((GetBillNo(billtype) - 1) <= Bnum)
                {
                    e.Appearance.BackColor = Color.White;
                    return;
                }
            }
        }

        private void sbtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gvBillList);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gvBillList);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gvBillList, "票据管理入库");
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("票据管理入库");
        }
    }
}