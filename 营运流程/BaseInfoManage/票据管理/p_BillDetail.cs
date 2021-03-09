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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class p_BillDetail : BaseForm
    {
        public p_BillDetail()
        {
            InitializeComponent();
        }
        private DevExpress.XtraGrid.Views.Grid.GridView pgv;

        //commonclass cc = new commonclass();
        //userright ur = new userright();
        private void billDetails_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("票据使用明细");//xj/2019/5/29
            this.cobWebid.SelectedIndex = 1;
            this.cobWebid.Properties.Items.Clear();
            GetDetailsWebid();
            this.dateEdit1.EditValue = CommonClass.gbdate;
            this.dateEdit2.EditValue = CommonClass.gedate;
            GridOper.RestoreGridLayout(gvBillDetails, "票据使用明细");
            FixColumn fix = new FixColumn(gvBillDetails, barSubItem2);
            //cc.LoadScheme(gcMain); 
            BarMagagerOper.SetBarPropertity(bar3);
            //barButtonItem5.Enabled = barButtonItem8.Enabled = ur.GetUserRightDetail("d31002");
            //barButtonItem6.Enabled = ur.GetUserRightDetail("d31003");
        }

        public void GetDetailsWebid()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                cobWebid.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    cobWebid.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["SiteName"]);
                }
                cobWebid.Properties.Items.Add("全部");
                cobWebid.Text = "全部";
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
            string webid = cobWebid.Text.Trim() == "全部" ? "%%" : cobWebid.Text.Trim();
            //int state = 0;
            //if (comboBoxEdit1.Text == "全部")
            //{
            //    state = -1;
            //}
            //else if (comboBoxEdit1.Text == "未使用")
            //{
            //    state = -2;
            //}
            //else
            //{
            //    state = comboBoxEdit1.SelectedIndex;
            //}

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
                list.Add(new SqlPara("webid", webid));
                list.Add(new SqlPara("state", this.comboBoxEdit1.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BillnoByWebid", list);
                gcMain.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gvBillDetails);
        }

        private void barCheckItem2_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gvBillDetails);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gvBillDetails);
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gvBillDetails, "票据使用明细");
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("票据使用明细");
        }

        private void gvBillDetails_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            int status = Convert.ToInt32(gvBillDetails.GetRowCellValue(e.RowHandle, gvBillDetails.Columns["state"]));
            if (status == 0)
            {

            }
            if (status == 1)
            {
                e.Appearance.BackColor = Color.FromArgb(178, 226, 240);
            }
            if (status == 2)
            {
                e.Appearance.BackColor = Color.LimeGreen;
            }
            if (status == 3)
            {
                e.Appearance.BackColor = Color.Red;
            }
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = gvBillDetails.GetSelectedRows();
            if (rows.Length == 0)
            {
                XtraMessageBox.Show("请选择要销单的托运单!\r\n您可以单票销单,也可以按住Ctrl键(或Shift键)并单击鼠标左键选择多票同时销单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (XtraMessageBox.Show("确定要销已选择的单据吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("hxstate", 1));
                    list.Add(new SqlPara("hxdate", CommonClass.gcdate));
                    list.Add(new SqlPara("hxcreateby", CommonClass.UserInfo.UserName));

                    list.Add(new SqlPara("unit", Convert.ToInt32(gvBillDetails.GetRowCellValue(rows[i], "unit")).ToString()));
                    list.Add(new SqlPara("billtype", gvBillDetails.GetRowCellValue(rows[i], "billtype").ToString()));
                    list.Add(new SqlPara("billno", gvBillDetails.GetRowCellValue(rows[i], "billno").ToString()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_PIAOJU_HEXIAO", list);
                    SqlHelper.ExecteNonQuery(sps);
                }
                XtraMessageBox.Show("销单完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            int[] rows = gvBillDetails.GetSelectedRows();
            if (rows.Length == 0)
            {
                XtraMessageBox.Show("请选择要销单的托运单!\r\n您可以单票销单,也可以按住Ctrl键(或Shift键)并单击鼠标左键选择多票同时销单!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //zhengjiafeng20181009 
            int rowhandle = gvBillDetails.FocusedRowHandle;
            if (rowhandle < 0) return;
            int hxstate = Convert.ToInt32(gvBillDetails.GetRowCellValue(rowhandle, "hxstate"));
            if (hxstate == 0) //0未核销  1核销
            {
                MsgBox.ShowOK("此运单还没核销，不需要反核销");
                return;
            }
            if (XtraMessageBox.Show("确定要反销已选择的单据吗?", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                for (int i = 0; i < rows.Length; i++)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("hxstate", 0));
                    list.Add(new SqlPara("hxdate", gvBillDetails.GetRowCellValue(rows[i], "hxdate")));
                    list.Add(new SqlPara("hxcreateby", gvBillDetails.GetRowCellValue(rows[i], "hxcreateby")));

                    list.Add(new SqlPara("unit", Convert.ToInt32(gvBillDetails.GetRowCellValue(rows[i], "unit"))));
                    list.Add(new SqlPara("billtype", gvBillDetails.GetRowCellValue(rows[i], "billtype").ToString()));
                    list.Add(new SqlPara("billno", gvBillDetails.GetRowCellValue(rows[i], "billno").ToString()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_PIAOJU_FANHEXIAO", list);
                    SqlHelper.ExecteNonQuery(sps);


                    gvBillDetails.SetRowCellValue(rows[i], "hxstate", 0);
                    gvBillDetails.SetRowCellValue(rows[i], "hxdate", null);
                    gvBillDetails.SetRowCellValue(rows[i], "hxcreateby", null);
                }
                XtraMessageBox.Show("反销单完成!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gcMain_MouseUp(object sender, MouseEventArgs e)
        {
            //cc.ShowPopupMenu(gvBillDetails, e, popupMenu1);
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hi = gvBillDetails.CalcHitInfo(e.Location);
            if (hi.InRowCell == true && e.Button == MouseButtons.Right)
            {
                barButtonItem7.Tag = hi.Column.FieldName;
            }
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = gvBillDetails.FocusedRowHandle;
                if (rowhandle < 0) return;
                Clipboard.SetText(gvBillDetails.GetRowCellValue(rowhandle, barButtonItem7.Tag.ToString()).ToString());
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            int rowhandle = gvBillDetails.FocusedRowHandle;
            if (rowhandle < 0) return;
            int state = Convert.ToInt32(gvBillDetails.GetRowCellValue(rowhandle, "state"));
            if (state > 1) //0入库 1出库 2使用 3作废
            {
                MsgBox.ShowOK("只能作废未使用的托运单!");
                return;
            }
            if (XtraMessageBox.Show("确定要作废已选择的单据吗?\r\n请注意，作废后将不可恢复!", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("hxdate", CommonClass.gcdate));
                list.Add(new SqlPara("hxcreateby", CommonClass.UserInfo.UserName));

                list.Add(new SqlPara("billtype", gvBillDetails.GetRowCellValue(rowhandle, "billtype")));
                list.Add(new SqlPara("billno", gvBillDetails.GetRowCellValue(rowhandle, "billno")));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_PIAOJU_HEXIAO_zuofei", list);
                SqlHelper.ExecteNonQuery(sps);

                gvBillDetails.SetRowCellValue(rowhandle, "state", 3);
                gvBillDetails.SetRowCellValue(rowhandle, "hxstate", 1);
                gvBillDetails.SetRowCellValue(rowhandle, "hxdate", CommonClass.gcdate);
                gvBillDetails.SetRowCellValue(rowhandle, "hxcreateby", CommonClass.UserInfo.UserName);

                XtraMessageBox.Show("作废成功!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}