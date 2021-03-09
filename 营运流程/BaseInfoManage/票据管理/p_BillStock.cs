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
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class p_BillStock : BaseForm
    {
        public p_BillStock()
        {
            InitializeComponent();
        }

        private void RepertoryForm_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("票据库存");//xj/2019/5/29
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

        private void sBtnSearch_Click(object sender, EventArgs e)
        {
            string billtype = cobeBillType.Text.Trim();
            billtype = billtype == "全部" ? "%%" : billtype;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billtype", billtype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OUT_GetBillKCByType", list);
                gcRepertory.DataSource = SqlHelper.GetDataTable(sps);

                list.Clear();
                list.Add(new SqlPara("billtype", billtype));
                list.Add(new SqlPara("webid", "%%"));
                SqlParasEntity sps1 = new SqlParasEntity(OperType.Query, "P_USP_GET_Total_By_Webid", list);
                gridControl1.DataSource = SqlHelper.GetDataTable(sps1);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void sBtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gvRepertory_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0)
                {
                    int sykc = Convert.ToInt32(gvRepertory.GetRowCellDisplayText(e.RowHandle, gvRepertory.Columns["sykc"]));
                    int zrk = Convert.ToInt32(gvRepertory.GetRowCellDisplayText(e.RowHandle, gvRepertory.Columns["zrk"]));
                    int yck = Convert.ToInt32(gvRepertory.GetRowCellDisplayText(e.RowHandle, gvRepertory.Columns["yck"]));
                    if (zrk == sykc)
                    {
                        e.Appearance.BackColor = Color.White;
                        return;
                    }
                    if (sykc == 0)
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#FF8080");//红
                        return;
                    }
                    if (yck > 0)
                    {
                        e.Appearance.BackColor = ColorTranslator.FromHtml("#C0FFC0");//green
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}