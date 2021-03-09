using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmSmsMange_Auto : BaseForm
    {

        public frmSmsMange_Auto()
        {
            InitializeComponent();
        }

        //提取数据
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("smsstate", smsstate.Text.Trim() == "全部" ? "3" : smsstate.Text.Trim() == "发送失败" ? "1" : "0"));
                list.Add(new SqlPara("telephone", telephone.Text.Trim()));
                list.Add(new SqlPara("billno", billno.Text.Trim()));
                list.Add(new SqlPara("RecordCount", 20));
                //list.Add("RecordCount", ).Direction = ParameterDirection.Output;
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_SMSManage", list);
                
                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
       
        }
        //导出明细
        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3);
        }
        ////退出
        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //自动筛选
        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }
        //锁定外观
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3);
        }
        //取消锁定
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3.Guid.ToString());
        }
        //过滤器
        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void frmSmsMange_Auto_Load_1(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView3);
            GridOper.SetGridViewProperty(myGridView3);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView3);

            bdate.DateTime = Convert.ToDateTime(CommonClass.gcdate.AddDays(-7).ToShortDateString());
            edate.DateTime = CommonClass.gedate;
        }
  
        private void barModify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (myGridView3.FocusedRowHandle < 0)
                {
                    MsgBox.ShowOK("请选择一条信息！");
                    return;
                }
                string billNo = myGridView3.GetRowCellValue(myGridView3.FocusedRowHandle, "billno").ToString();
                string SWTelNum = myGridView3.GetRowCellValue(myGridView3.FocusedRowHandle, "telephone").ToString();
                string oldId = myGridView3.GetRowCellValue(myGridView3.FocusedRowHandle, "Id").ToString();
                FrmupdateSMS frm = new FrmupdateSMS();
                frm.billNo = billNo;
                frm.telephone = SWTelNum;
                frm.oldId = oldId;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        ////按手机号删除
        //private void FrmDelByTel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    SMSDelByBillorTel frm = new SMSDelByBillorTel();
        //    frm.flag = 1;
        //    frm.ShowDialog();
        //}


        ////按单号删除
        //private void FrmDelByBillNo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        //{
        //    SMSDelByBillorTel frm = new SMSDelByBillorTel();
        //    frm.flag = 2;
        //    frm.ShowDialog();
        //}

        //选定删除
        private void FrmDelbyFocus_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView3.GetRowCellValue(rowhandle, "Id").ToString());

                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("FocusId", id));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_FOCUSID", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    myGridView3.DeleteRow(rowhandle);
                    myGridView3.PostEditor();
                    myGridView3.UpdateCurrentRow();
                    myGridView3.UpdateSummary();
                    DataTable dt = myGridControl3.DataSource as DataTable;
                    dt.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
