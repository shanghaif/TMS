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
    public partial class frmBuKouRecord : BaseForm
    {
        public frmBuKouRecord()
        {
            InitializeComponent();
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                list.Add(new SqlPara("Type", Type.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BUKOU_RECORD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 2000) myGridView1.BestFitColumns();
            }
        }

        private void WayBillRecord_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

            GridOper.RestoreGridLayout(myGridView1);
            if (CommonClass.UserInfo.SiteName != "总部" && CommonClass.UserInfo.WebName.ToString().Contains("财务"))
            {
                CommonClass.SetWeb(WebName, CommonClass.UserInfo.SiteName, false);
            }
            else
            {
                CommonClass.SetWeb(WebName, "全部", true);
            }
            WebName.EditValue = CommonClass.UserInfo.WebName;
            GridOper.CreateStyleFormatCondition(myGridView1, "ApplyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "审核", Color.Green);//已通过 绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "ApplyState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "否决", Color.Red);//已否决，黄色
            
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "补扣记录");
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBuKouAdd frm = new frmBuKouAdd();
            frm.ShowDialog();
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("审核");
        }
        

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SH("否决");
        }

        private void SH(string state)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (MsgBox.ShowYesNo("是否" + state + "?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            try
            {
                Guid TjId = new Guid(myGridView1.GetRowCellValue(rowhandle, "TjId").ToString());

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("TjId", TjId));
                list.Add(new SqlPara("ApplyState", state));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_bukou_APPly", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridView1.SetRowCellValue(rowhandle, "ApplyState", state);
                    myGridView1.SetRowCellValue(rowhandle, "ApplyMan", CommonClass.UserInfo.UserName);
                    myGridView1.SetRowCellValue(rowhandle, "ApplyDate", CommonClass.gcdate);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //frmTiaoZhangUP frm = new frmTiaoZhangUP();
            //frm.ShowDialog();
            //cbRetrieve_Click(null,null);
        }
    }
}