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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmBankAudit : BaseForm
    {
        public frmBankAudit()
        {
            InitializeComponent();
        }
        DataSet ds1 = new DataSet();
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MsgBox.ShowOK("已审核显示绿色，未审核显示白色");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barCheckItem2_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem3_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barCheckItem4_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //加载
        private void frmBankAudit_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            //设置网格
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            //dateEdit1.DateTime = CommonClass.gbdate;
            //dateEdit2.DateTime = CommonClass.gedate;
            dateEdit1.DateTime = CommonClass.gbdate;
            dateEdit2.DateTime = CommonClass.gedate;
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
            //设置颜色
            //GridOper.CreateStyleFormatCondition(myGridView1, "auditstatus", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "已审核", Color.LightGreen);
            //GridOper.CreateStyleFormatCondition(myGridView1, "auditstatus", DevExpress.XtraGrid.FormatConditionEnum.Equal, "未审核", Color.White);

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            JMfrmadd frmadd = new JMfrmadd();
            frmadd.Show();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            getData();
        }

        public void getData()
        {
            try
            {
                myGridView1.ClearColumnsFilter();
                List<SqlPara> list = new List<SqlPara>();
                
                list.Add(new SqlPara("Type", comboBoxEdit1.Text.Trim()));
                list.Add(new SqlPara("bdate", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("edate", dateEdit2.Text.Trim()));
                list.Add(new SqlPara("VerifyBatch", textBox1.Text.Trim() == "" ? "%%" : "%" + textBox1.Text.Trim() + "%"));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : "%" + CauseName.Text.Trim() + "%"));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : "%" + AreaName.Text.Trim() + "%"));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : "%" + WebName.Text.Trim() + "%"));
                
                list.Add(new SqlPara("VerifyState", auditstatus.Text.Trim() == "全部" ? "%%" : "%" + auditstatus.Text.Trim() + "%"));
                
                //list.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
                //list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                //list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_basBankAudit", list);
                ds1 = SqlHelper.GetDataSet(spe);

                myGridControl1.DataSource = ds1.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            myGridView1.ClearColumnsFilter();
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            
            //用GridOper获取该行的信息
            string ID = myGridView1.GetRowCellValue(rowHandle, "id").ToString();
            string entryname = GridOper.GetRowCellValueString(myGridView1, rowHandle, "entryname");
            string Senddate = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Senddate");
            string Amount = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Amount");
            string Registrant = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Registrant");
            string Redate = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Redate");
            string Redepartment = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Redepartment");
            string Abstract = GridOper.GetRowCellValueString(myGridView1, rowHandle, "abstract");
            string Remarks = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Remarks");
            string auditstatus = GridOper.GetRowCellValueString(myGridView1, rowHandle, "auditstatus");
            string CauseName = GridOper.GetRowCellValueString(myGridView1, rowHandle, "CauseName");
            string AreaName = GridOper.GetRowCellValueString(myGridView1, rowHandle, "AreaName");
            string WebName = GridOper.GetRowCellValueString(myGridView1, rowHandle, "WebName");

            if (auditstatus == "已审核") {
                MsgBox.ShowOK("已审核，不可修改");
                return;
            }

            //将获取的信息添加进去
            JMfrmadd jma = new JMfrmadd();
            jma.ismodify = 1;
            jma.ID1 = ID;
            jma.entryname1 = entryname;
            jma.Senddate1 = Senddate;
            jma.Amount1 = Amount;
            jma.Registrant1 = Registrant;
            jma.abstract1 = Abstract;
            jma.Remarks1 = Remarks;
            jma.Redate1 = Redate;
            jma.Redepartment1 = Redepartment;
            jma.CauseName1 = CauseName;
            jma.AreaName1 = AreaName;
            jma.WebName1 = WebName;
            jma.ShowDialog();


        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }

            string auditstatus = GridOper.GetRowCellValueString(myGridView1, rowHandle, "auditstatus");
            if (auditstatus == "已审核")
            {
                MsgBox.ShowOK("已审核，不可删除");
                return;
            }

            string id = myGridView1.GetRowCellValue(rowHandle, "id").ToString();
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id",id));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "JM_DELETE_YYWSR", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0) {
                MsgBox.ShowOK("删除成功");

                simpleButton12_Click(sender, e);
            }


        }
        //导出
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel((GridView)myGridControl1.MainView);
        }
        //审核
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0) {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            myGridView1.PostEditor();
            string VerifyBatch = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "VerifyState")) == "已审核")
                    {
                        MsgBox.ShowOK("存在已审核的数据，请重新选择！");
                        return;
                    }
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        VerifyBatch += myGridView1.GetRowCellValue(i, "VerifyBatch") + "@";
                    }
                }
                
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VerifyBatch", VerifyBatch));
            list.Add(new SqlPara("Type", "0"));
          
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_BankAudit_Update", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("审核成功！");
                getData();
            }
          
            
           

        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem3_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            string VerifyBatch = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "VerifyState")) != "已审核" || ConvertType.ToString(myGridView1.GetRowCellValue(i, "VerifyBatch")) == "")
                    {
                        MsgBox.ShowOK("存在未审核数据，请检查！");
                        return;
                    }
                    VerifyBatch += myGridView1.GetRowCellValue(i, "VerifyBatch") + "@";
                }
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VerifyBatch", VerifyBatch));
            list.Add(new SqlPara("Type", "1"));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_BankAudit_Update", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("反审核成功！");
                getData();
            }
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            frmFinancialDetail frm = new frmFinancialDetail();
            frm.DeclareBatch = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("VerifyBatch")); 
            frm.FeeType = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("FeeType")); 
            frm.CarNO = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("CarNO"));
            frm.SerialNumber = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("SerialNumber")); 
            frm.Show();

        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ds1 == null || ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0) return;
            frmPrintRuiLang fpr = new frmPrintRuiLang("银行信息.grf", ds1);
            fpr.ShowDialog(); 
        }

     

       

        







    }
}
