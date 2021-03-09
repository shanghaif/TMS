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
    public partial class frmAccountingAuditKeTong : BaseForm
    {
        public frmAccountingAuditKeTong()
        {
            InitializeComponent();
        }

        private void frmAccMiddlePay_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("出纳审核");//xj/2019/5/29
            GetAllWeb();
            GetAllOptMan();
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.CreateStyleFormatCondition(myGridView1, "aduitStateStr", DevExpress.XtraGrid.FormatConditionEnum.Equal, "出纳审核", Color.LightCoral);
            CommonClass.SetCause(Cause, true);
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);
           
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;

     
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("WebName", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("CauseName", Cause.Text.Trim() == "全部" ? "%%" : Cause.Text.Trim()));
                list.Add(new SqlPara("AreaName", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                list.Add(new SqlPara("direction", direction.Text.Trim() == "全部" ? "%%" : direction.Text.Trim()));
                list.Add(new SqlPara("verifyMan", verifyMan.Text.Trim() == "全部" ? "%%" : verifyMan.Text.Trim()));
                list.Add(new SqlPara("optState", optState.Text.Trim() == "全部" ? "%%" : optState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_Audit_CHUNA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", bdate.DateTime));
                list.Add(new SqlPara("t2", edate.DateTime));
                list.Add(new SqlPara("WebName", web.Text.Trim() == "全部" ? "%%" : web.Text.Trim()));
                list.Add(new SqlPara("direction", direction.Text.Trim() == "全部" ? "%%" : direction.Text.Trim()));
                list.Add(new SqlPara("verifyMan", verifyMan.Text.Trim() == "全部" ? "%%" : verifyMan.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLACCOUNT_Count_1", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                decimal a = ds.Tables[0].Rows[0][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0][0]);
                decimal b = ds.Tables[0].Rows[1][0] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[1][0]);
                edaccyestoday.Text = Math.Round(a, 2).ToString();
                edacctoday.Text = Math.Round(b, 2).ToString();
                decimal accyestoday = 0;
                if (edaccyestoday.Text.Trim() != "")
                {
                    accyestoday = Convert.ToDecimal(edaccyestoday.Text.Trim());
                }

                decimal acctoday = 0;
                if (edacctoday.Text.Trim() != "")
                {
                    acctoday = Convert.ToDecimal(edacctoday.Text.Trim());
                }
                edaccnow.Text = Math.Round(accyestoday + acctoday, 2).ToString();
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

        private void barCheckItem1_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void SelectAll(object sender, EventArgs e)
        {
            try { (sender as ComboBoxEdit).SelectAll(); }
            catch { }
        }

        private void checkBox_selectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_selectAll.Checked == true)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "ischecked", true);
                }
            }
            if (checkBox_selectAll.Checked == false)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, "ischecked", false);
                }
            }
        }


        private bool CheckSelect(ref DataTable SelectDt)
        {   //在未关闭编辑器的情况下,更新编辑器的值到数据源并改变RowState
            myGridView1.PostEditor();
            //初始化dt,ref修饰的变量，在使用前必须赋值
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return false;
            SelectDt = dt.Clone();      //Clone赋值表结构给Selectdt

            foreach (DataRow dr in dt.Rows)
            {
                if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                SelectDt.ImportRow(dr);           //将选择的行存到新表
            }
            if (SelectDt.Rows.Count == 0)
            {
                MsgBox.ShowOK("请选择数据!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 批量审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                DataTable dt = null;
                if (!CheckSelect(ref dt)) return;    //传值给dt
                if (dt == null) return;
                string  VoucherNos = "";
                //ywc              
                fmModifyTime frm = new fmModifyTime();
                frm.ShowDialog();
                DateTime shDate = frm.shdate;
                if (MsgBox.ShowYesNo("是否执行?\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                {
                    return;
                }
                //2019.3.2wbw
                string VerifyStatus = "";
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        VerifyStatus = myGridView1.GetRowCellValue(i, "VerifyStatus").ToString();
                        if (VerifyStatus == "取消")
                        {
                            MsgBox.ShowOK("存在已取消核销的单，不允许审核！");
                            return;
                        }
                    }
                }
                string VerifyOffTypes = "";
                foreach (DataRow dr in dt.Rows)
                {
                    VoucherNos += dr["VoucherNo"] + "@";
                    VerifyOffTypes += dr["VerifyOffType"] + "@";
                }
                if (string.IsNullOrEmpty(VoucherNos))
                {
                    MsgBox.ShowError("未找到需要审核的数据！");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNos", VoucherNos));
                list.Add(new SqlPara("VerifyOffTypes", VerifyOffTypes));
                list.Add(new SqlPara("shDate", shDate));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLACCOUNT_SHENHE_2", list)) > 0)
                {

                    MsgBox.ShowOK("审核成功");
                }
                else
                {
                    MsgBox.ShowOK("审核失败");
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message.ToString();
                MsgBox.ShowOK(errmsg.Replace("数据库访问异常：", ""));
            }        
        }

        /// <summary>
        /// 反审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            myGridView1.PostEditor();
            if (MsgBox.ShowYesNo("是否确认反审核？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string Area = GridOper.GetRowCellValueString(myGridView1, rowhandle, "AreaName");
            string AreaName = CommonClass.UserInfo.AreaName;
            //ywc20190420 取消反审核限制
            //if (Area != AreaName)
            //{
            //    MsgBox.ShowOK("只能反审核自己所属大区的单！");
            //    return;
            //}
            string VoucherNos = "", VerifyOffTypes="";

            if (rowhandle >= 0)
            {
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(myGridView1.GetRowCellValue(i, "ischecked")) > 0)
                    {
                        VoucherNos += myGridView1.GetRowCellValue(i, "VoucherNo") + ",";
                        VerifyOffTypes += myGridView1.GetRowCellValue(i, "VerifyOffType") + ",";
                        if (VoucherNos == "") return;
                    }
                }
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("VoucherNos", VoucherNos));
                list.Add(new SqlPara("VerifyOffTypes", VerifyOffTypes));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_BILLACCOUNT_FANSHENHE_1", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    cbRetrieve_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

      
        public void GetAllWeb()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                web.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    web.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
                web.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                
              XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void GetAllOptMan()
        {

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ALLOPTMAN", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    verifyMan.Properties.Items.Add(ds.Tables[0].Rows[i]["OptMan"]);
                }
                verifyMan.Properties.Items.Add("全部");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

           
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
            GridOper.ExportToExcel(myGridView1, this.Text);
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text, true);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int row = myGridView1.FocusedRowHandle;
            if (row < 0) return;
            string VoucherNo = GridOper.GetRowCellValueString(myGridView1, row, "VoucherNo");
            string VerifyOffType = GridOper.GetRowCellValueString(myGridView1, row, "VerifyOffType");
            frmAccountingAduit_List frm = new frmAccountingAduit_List();
            frm.VoucherNo = VoucherNo;
            frm.ShowDialog();
        }




       
    }
}