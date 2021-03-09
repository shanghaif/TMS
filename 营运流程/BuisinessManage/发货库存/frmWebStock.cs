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
    public partial class frmWebStock : BaseForm
    {
        public frmWebStock()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cbbSiteName.Text.Trim()))
                {
                    MsgBox.ShowOK("请填写站点名称！");
                    return;
                }
                if (string.IsNullOrEmpty(ccbWebName.Text.Trim()))
                {
                    MsgBox.ShowOK("请填写网点名称！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("type", type.Text.Trim()));
                list.Add(new SqlPara("siteName", cbbSiteName.Text.Trim()));
                list.Add(new SqlPara("webName", ccbWebName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WebStock", list);
                myGridControl1.DataSource = SqlHelper.GetDataTable(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
            CommonClass.SetSite(cbbSiteName, false);
            cbbSiteName.Text = CommonClass.UserInfo.SiteName;
            ccbWebName.Text = CommonClass.UserInfo.WebName;
            if (CommonClass.UserInfo.SiteName != "总部")
            {
                cbbSiteName.Enabled = false;
                ccbWebName.Enabled = false;
            }
            else
            {
                cbbSiteName.Enabled = true;
                ccbWebName.Enabled = true;
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //List<string> lt = new List<string>();
            //lt.Add("目的网点");
            //lt.Add("运单号");
            //lt.Add("件数");
            //lt.Add("剩余件数");
            //lt.Add("开单日期");
            //lt.Add("运单状态");
            //lt.Add("包装");
            //lt.Add("品名");
            //lt.Add("体积");
            //lt.Add("实盘件数");
            //lt.Add("盘点备注");

            //List<string> vlt = new List<string>();

            //for (int index = 0; index < myGridView1.Columns.Count; index++)
            //{

            //    if (lt.Contains(myGridView1.Columns[index].Caption.ToString()))
            //    {
            //        myGridView1.Columns[index].Visible = true;

            //    }
            //    else
            //    {
            //        if (myGridView1.Columns[index].Visible == true)
            //        {
            //            vlt.Add(myGridView1.Columns[index].Caption.ToString());
            //            myGridView1.Columns[index].Visible = false;
            //        }
            //    }

            //}
            GridOper.ExportToExcel(myGridView1, "发货库存");


            //for (int index = 0; index < myGridView1.Columns.Count; index++)
            //{
            //    if (vlt.Contains(myGridView1.Columns[index].Caption.ToString()))
            //    {
            //        myGridView1.Columns[index].Visible = true;

            //    }
            //}



            //frmExcelPortPro fep = new frmExcelPortPro()



        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (myGridView1.DataRowCount > 0)
            {
                DataTable dt = ((DataTable)myGridControl1.DataSource).Clone();


                dt.Columns.Add("xh");

                int xh = 0;
                for (int index = 0; index < myGridView1.RowCount; index++)
                {

                    dt.Rows.Add(myGridView1.GetDataRow(index).ItemArray);

                    dt.Rows[index]["xh"] = xh;
                    xh += 1;
                }
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);


                frmPrintRuiLang fpr = new frmPrintRuiLang("盘点清单", ds);
                fpr.ShowDialog();
            }
            else
            {
                MessageBox.Show("没有数据可打印!");

            }

            //List<string> lt = new List<string>();
            //lt.Add("目的网点");
            //lt.Add("运单号");
            //lt.Add("件数");
            //lt.Add("剩余件数");
            //lt.Add("开单日期");
            //lt.Add("运单状态");
            //lt.Add("包装");
            //lt.Add("品名");
            //lt.Add("体积");
            //lt.Add("实盘件数");
            //lt.Add("盘点备注");

            //List<string> vlt = new List<string>();

            //for (int index = 0; index < myGridView1.Columns.Count; index++)
            //{

            //    if (lt.Contains(myGridView1.Columns[index].Caption.ToString()))
            //    {
            //        myGridView1.Columns[index].Visible = true;

            //    }
            //    else
            //    {
            //        if (myGridView1.Columns[index].Visible == true)
            //        {
            //            vlt.Add(myGridView1.Columns[index].Caption.ToString());
            //            myGridView1.Columns[index].Visible = false;
            //        }
            //    }

            //}
            //GridOper.ExportToExcel(myGridView1, "发货库存");


            //for (int index = 0; index < myGridView1.Columns.Count; index++)
            //{
            //    if (vlt.Contains(myGridView1.Columns[index].Caption.ToString()))
            //    {
            //        myGridView1.Columns[index].Visible = true;

            //    }
            //}


        }

        private void cbbSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(ccbWebName, cbbSiteName.Text.Trim(), false);
        }
    }
}