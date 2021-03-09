using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KMS.Tool;
using KMS.Common;
using KMS.SqlDAL;
using DevExpress.XtraGrid.Columns;
using KMS.UI;

namespace KMS.UI
{
    public partial class FrmFydengJi : BaseForm
    {
        public FrmFydengJi()
        {
            InitializeComponent();
        }
        //页面加载
        private void FrmFydengJi_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.SetCause(CauseName, false);
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            BegWeb.Text = CommonClass.UserInfo.WebName;

            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        //提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Fydengji", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        //新增
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmFydjADD ADD = new FrmFydjADD();
            ADD.ShowDialog();
            shuaxin();
        }
        //修改
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            } 
            string ID = myGridView1.GetRowCellValue(rows, "ID").ToString();
            string hexiao = myGridView1.GetRowCellValue(rows, "hexiao").ToString();
            string pici = myGridView1.GetRowCellValue(rows, "pici").ToString();
            string khxm = myGridView1.GetRowCellValue(rows, "khxm").ToString();
            string yhzh = myGridView1.GetRowCellValue(rows, "yhzh").ToString();
            string khyh = myGridView1.GetRowCellValue(rows, "khyh").ToString();
            string ApplyDate = myGridView1.GetRowCellValue(rows, "ApplyDate").ToString();
            string ApplyCause = myGridView1.GetRowCellValue(rows, "ApplyCause").ToString();
            string ApplyArea = myGridView1.GetRowCellValue(rows, "ApplyArea").ToString();
            string ApplyDept = myGridView1.GetRowCellValue(rows, "ApplyDept").ToString();
            string FeeType = myGridView1.GetRowCellValue(rows, "FeeType").ToString();
            string FeeProject = myGridView1.GetRowCellValue(rows, "FeeProject").ToString();
            string AssumeDept = myGridView1.GetRowCellValue(rows, "AssumeDept").ToString();
            string BelongMonth = myGridView1.GetRowCellValue(rows, "BelongMonth").ToString();
            string BelongYear = myGridView1.GetRowCellValue(rows, "BelongYear").ToString();
            string Money = myGridView1.GetRowCellValue(rows, "Money").ToString();
            string ApplyMan = myGridView1.GetRowCellValue(rows, "ApplyMan").ToString();
            string BankSubbranch = myGridView1.GetRowCellValue(rows, "BankSubbranch").ToString();
            string BankProvince = myGridView1.GetRowCellValue(rows, "BankProvince").ToString();
            string BankCity = myGridView1.GetRowCellValue(rows, "BankCity").ToString();
            string TransferType = myGridView1.GetRowCellValue(rows, "TransferType").ToString();
            string ExpendType = myGridView1.GetRowCellValue(rows, "ExpendType").ToString();
            string PayDate = myGridView1.GetRowCellValue(rows, "PayDate").ToString();
            string Remark = myGridView1.GetRowCellValue(rows, "Remark").ToString();
            if (hexiao.Equals("已核销"))//
            {
                MsgBox.ShowError("当前运单为以核销状态，不能删除！");
                return;
            }
            FrmFydjADD frm = new FrmFydjADD();
            frm.ID1 = ID;
            frm.pici1 = pici;
            frm.khxm1 = khxm;
            frm.yhzh1 = yhzh;
            frm.khyh1 = khyh;
            frm.ApplyDate1 = ApplyDate;
            frm.ApplyCause1 = ApplyCause;
            frm.ApplyArea1 = ApplyArea;
            frm.ApplyDept1 = ApplyDept;
            frm.FeeType1 =FeeType ;
            frm.FeeProject1 =FeeProject ;
            frm.AssumeDept1 =AssumeDept ;
            frm.BelongMonth1 = BelongMonth;
            frm.BelongYear1 = BelongYear;
            frm.Money1 =Money ;
            frm.ApplyMan1 =ApplyMan ;
            frm.BankSubbranch1 = BankSubbranch;
            frm.BankProvince1 = BankProvince;
            frm.BankCity1 = BankCity;
            frm.TransferType1 = TransferType;
            frm.ExpendType1 = ExpendType;
            frm.PayDate1 = PayDate;
            frm.Remark1 = Remark;
            frm.ShowDialog();
            shuaxin();
                  
        }
        //删除
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            string hexiao = myGridView1.GetRowCellValue(rowhandle, "hexiao").ToString();
            try
            {
                if (hexiao.Equals("已核销"))//
                {
                    MsgBox.ShowError("当前运单为以核销状态，不能删除！");
                    return;
                }
                if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_DEL_FYDJ", list);
                
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            shuaxin();
        }
         //核销
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            string hexiao = myGridView1.GetRowCellValue(rowhandle, "hexiao").ToString();
            string pici = myGridView1.GetRowCellValue(rowhandle, "pici").ToString();
            string pingzheng= "FY" + CommonClass.gcdate.ToString("yyyyMMddHHmmss");
            string Remark = myGridView1.GetRowCellValue(rowhandle, "Remark").ToString();
            string FeeType = myGridView1.GetRowCellValue(rowhandle, "FeeType").ToString();
            string Money = myGridView1.GetRowCellValue(rowhandle, "Money").ToString();
            string ApplyCause = myGridView1.GetRowCellValue(rowhandle, "ApplyCause").ToString();
            string ApplyArea = myGridView1.GetRowCellValue(rowhandle, "ApplyArea").ToString();
            string ApplyDept = myGridView1.GetRowCellValue(rowhandle, "ApplyDept").ToString();
            try
            {
                if (hexiao.Equals("已核销"))//
                {
                    MsgBox.ShowError("当前运单为以核销状态，不能重复核销！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("pici", pici));
                list.Add(new SqlPara("pingzheng", pingzheng));
                list.Add(new SqlPara("Remark", Remark));
                list.Add(new SqlPara("FeeType", FeeType));
                list.Add(new SqlPara("Money", Money));
                list.Add(new SqlPara("ApplyCause", ApplyCause));
                list.Add(new SqlPara("ApplyArea", ApplyArea));
                list.Add(new SqlPara("ApplyDept", ApplyDept));

                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_HX_Fydj", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0) 
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            shuaxin();
        }
         //反核销
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            string hexiao = myGridView1.GetRowCellValue(rowhandle, "hexiao").ToString();
            string pici = myGridView1.GetRowCellValue(rowhandle, "pici").ToString();
            string pingzheng = myGridView1.GetRowCellValue(rowhandle, "pingzheng").ToString();
            try
            {
                if (hexiao == "未核销")
                {
                    MsgBox.ShowError("当前运单为未核销状态，不能反核销！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", ID));
                list.Add(new SqlPara("hexiao", hexiao));
                list.Add(new SqlPara("pici", pici));
                list.Add(new SqlPara("pingzheng", pingzheng));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_FHX_Fydj", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            shuaxin();
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(AreaName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(BegWeb, CauseName.Text, AreaName.Text);
        }
        public void shuaxin() 
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));

                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", BegWeb.Text.Trim() == "全部" ? "%%" : BegWeb.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_Fydengji", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

     }
}

