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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmFinancialAudit : BaseForm
    {
        public frmFinancialAudit()
        {
            InitializeComponent();
        }
        GridColumn gcIsseleckedMode;

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
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barCheckItem4_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        //加载
        private void frmFinancialAudit_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);

            //设置网格
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            dateEdit1.DateTime = CommonClass.gbdate.AddHours(-16);
            dateEdit2.DateTime = CommonClass.gedate.AddHours(-16);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
            //设置颜色
            //GridOper.CreateStyleFormatCondition(myGridView1, "auditstatus", DevExpress.XtraGrid.FormatConditionEnum.NotEqual, "已审核", Color.LightGreen);
            //GridOper.CreateStyleFormatCondition(myGridView1, "auditstatus", DevExpress.XtraGrid.FormatConditionEnum.Equal, "未审核", Color.White);

            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            CommonClass.GetServerDate();
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
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
                string ConfirmState = "";
                if (auditstatus.Text.Trim()=="全部")
                {
                    ConfirmState = "%%";
                }
                else if (auditstatus.Text.Trim() == "未确认")
                {
                    ConfirmState = "";
                }
                else
                {
                    ConfirmState = auditstatus.Text.Trim();
                }
                list.Add(new SqlPara("ConfirmState", ConfirmState));
                list.Add(new SqlPara("bdate", dateEdit1.Text.Trim()));
                list.Add(new SqlPara("edate", dateEdit2.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : "%" + CauseName.Text.Trim() + "%"));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : "%" + AreaName.Text.Trim() + "%"));
                list.Add(new SqlPara("WebName", WebName.Text.Trim() == "全部" ? "%%" : "%" + WebName.Text.Trim() + "%"));
                list.Add(new SqlPara("Type", comboBoxEdit1.Text.Trim()));
                //list.Add(new SqlPara("CauseName", CauseName.Text.Trim()));
                //list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                //list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_basfinancialAudit", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];

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
            string AID = "";
            string BbusinessDate = "";
            //string CarNO = "";
            //string DepartureBatch = "";
            //string Money = "";
            double Money = 0.0;
            string FeeType = "";
            string FeeTypeOne = "";
            //FeeType.Substring();
            

            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                {
                    FeeTypeOne = ConvertType.ToString(myGridView1.GetRowCellValue(i, "FeeType"));
                    if ((ConvertType.ToString(myGridView1.GetRowCellValue(i, "BankMan")) == "" || 
                        ConvertType.ToString(myGridView1.GetRowCellValue(i, "BankCode")) == "")
                        && (FeeTypeOne == "大车费现付"
                        || FeeTypeOne == "大车油卡费"))
                    {
                        if(Convert.ToDecimal(myGridView1.GetRowCellValue(i, "money"))<=0)
                        {
                            MsgBox.ShowOK("金额不能小于零：" + myGridView1.GetRowCellValue(i, "DepartureBatch"));
                            return;
                        }
                        MsgBox.ShowOK("银行信息不能为空：" + myGridView1.GetRowCellValue(i, "DepartureBatch"));
                        return;
                    }
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConfirmState")) == "已确认")
                    {
                        MsgBox.ShowOK("存在已确认数据！：" + myGridView1.GetRowCellValue(i, "DepartureBatch"));
                        return;
                    }
                    //if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "FeeType")) == "车辆代扣" && ConvertType.ToDecimal(myGridView1.GetRowCellValue(i, "money")) <= 0 && ConvertType.ToString(myGridView1.GetRowCellValue(i, "BankCode")) != "" && ConvertType.ToString(myGridView1.GetRowCellValue(i, "BankMan")) != "")
                    //{
                    //    MsgBox.ShowOK("金额不能小于零：" + myGridView1.GetRowCellValue(i, "DepartureBatch"));
                    //    return;
                    //}
                    //if (FeeTypeOne == "大车费回付" || FeeTypeOne == "大车油料费" || FeeTypeOne == "车辆代扣"
                    //    || FeeTypeOne == "大车增减款" || FeeTypeOne == "大车费代收" || FeeTypeOne == "大车司机奖罚费")
                    //{
                       
                       
                    //}
                    
                    AID += myGridView1.GetRowCellValue(i, "AID") + "@";
                    BbusinessDate += myGridView1.GetRowCellValue(i, "BbusinessDate") + "@";
                    //CarNO += myGridView1.GetRowCellValue(i, "CarNO") + "@";
                    //DepartureBatch += myGridView1.GetRowCellValue(i, "DepartureBatch") + "@";
                    //Money += Convert.ToDouble(myGridView1.GetRowCellValue(i, "money"));
                    FeeType += FeeTypeOne + "@";

                }
            }

            try
            {
                string SerialNumber = "";
                List<SqlPara> list2 = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_FinancialAuditBatch", list2);
                DataSet ds = SqlHelper.GetDataSet(spe);
               SerialNumber= ds.Tables[0].Rows[0][0].ToString();


                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AID", AID));
                list.Add(new SqlPara("BbusinessDate", ""));
                list.Add(new SqlPara("CarNO", ""));
                list.Add(new SqlPara("DepartureBatch", BbusinessDate));
                list.Add(new SqlPara("money", Money));
                list.Add(new SqlPara("FeeType", FeeType));
                list.Add(new SqlPara("SerialNumber", SerialNumber));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FinancialAudit", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    List<SqlPara> list3 = new List<SqlPara>();
                    list3.Add(new SqlPara("SerialNumber", SerialNumber));
                    SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "QSP_GET_FinancialAuditSumMoney", list3);
                    DataSet ds2 = SqlHelper.GetDataSet(spe2);

                    MsgBox.ShowOK("本次确认："+ds2.Tables[0].Rows[0][0].ToString());
                    getData();
                }
            }
            catch(Exception ex)
            {
                //string strEx=ex.ToString();

                //strEx = strEx.Substring(strEx.IndexOf("50000，") + 6, strEx.Length);
                //strEx = strEx.Substring(strEx.IndexOf("50000，") + 6, strEx.IndexOf("\r\n ") - 1);
                //MsgBox.ShowOK(strEx);
                //return;
                MsgBox.ShowException(ex);
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
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息");
                return;
            }
            myGridView1.PostEditor();
            string VerifyBatch = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1")
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConfirmState")) != "已确认" || ConvertType.ToString(myGridView1.GetRowCellValue(i, "DeclareBatch")) == "")
                    {
                        MsgBox.ShowOK("存在未确认的数据！");
                        return;
                    }
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConfirmState")) == "已确认" && ConvertType.ToString(myGridView1.GetRowCellValue(i, "DeclareBatch")) != "")
                    {
                        VerifyBatch += myGridView1.GetRowCellValue(i, "DeclareBatch") + "@";

                    }
                }
              
            }
            if (VerifyBatch == "" || VerifyBatch=="@")
            {
                MsgBox.ShowOK("存在未确认的数据！");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("VerifyBatch", VerifyBatch));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_CHECK_FinancialAudit_Update", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                MsgBox.ShowOK("取消成功！");
                getData();
            }
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //tring BbusinessDate = "", DepartureBatch = "", CarNO = "", DriverName = "", LineName = "", FeeType = "", BankMan = "", BankCode = "", BankName = "";
            //Convert.ToString(myGridView1.GetRowCellValue(i, "FeeType")
            int rowHandle = myGridView1.FocusedRowHandle;
            if (rowHandle<0)
            {
                MsgBox.ShowOK("请选择一条数据！");
                return;
            }
            frmFinanciaAuditUpt frm = new frmFinanciaAuditUpt();
            frm.AID = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "AID"));
            frm.BbusinessDate = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "BbusinessDate"));
            frm.DepartureBatch = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "DepartureBatch"));
            frm.CarNO = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "CarNO"));
            frm.DriverName = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "DriverName"));
            frm.LineName = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "LineName"));
            frm.FeeType = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "FeeType"));
            frm.BankMan = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "BankMan"));
            frm.BankCode = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "BankCode"));
            frm.BankName = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "BankName"));
            frm.Province = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "Province"));
            frm.City = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "City"));
            frm.Show();
        }

        private void myGridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
           
        }

        private void myGridView1_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int rowHandle = myGridView1.FocusedRowHandle;

            if (rowHandle >= 0 && Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "ischecked")) == "0" && Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "FeeType")) == "大车费回付")
            {
                myGridView1.PostEditor();
                string feetype = "";
                string carNo = Convert.ToString(myGridView1.GetRowCellValue(rowHandle, "CarNO"));
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    feetype = Convert.ToString(myGridView1.GetRowCellValue(i, "FeeType"));
                    if (Convert.ToString(myGridView1.GetRowCellValue(i, "CarNO")) == carNo &&
                        ( feetype == "大车增减款"
                        || feetype == "大车费代收" || feetype == "大车司机奖罚费"
                        || feetype == "大车油料费" ||
                        (feetype == "车辆代扣" && Convert.ToString(myGridView1.GetRowCellValue(i, "BankCode")) == "")))
                    {
                        myGridView1.SetRowCellValue(i, "ischecked", 1);
                    }
                }
            }
           
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmFinanciaLoad frm = new frmFinanciaLoad();
            frm.Show();
        }

       

        







    }
}
