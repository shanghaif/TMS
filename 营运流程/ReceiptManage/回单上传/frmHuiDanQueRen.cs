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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Card.ViewInfo;
using ZQTMS.Lib;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

namespace ZQTMS.UI
{
    public partial class frmHuiDanQueRen : BaseForm
    {
        public frmHuiDanQueRen()
        {
            InitializeComponent();
        }

        private void frmHuiDanQueRen_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("回单确认");//xj2019/5/28
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例 
            GridOper.RestoreGridLayout(gridView1, "回单确认记录");

            CommonClass.SetSite(comboBoxEdit1, true);
            CommonClass.SetSite(comboBoxEdit2, true);
            dateEdit1.DateTime = CommonClass.gbdate;
            dateEdit2.DateTime = CommonClass.gedate;
            //CommonClass.SetWeb(comboBoxEdit3, true);
            CommonClass.SetCause(CauseName, true);
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetWeb(comboBoxEdit3, comboBoxEdit1.Text);
            //CommonClass.SetCauseWeb(comboBoxEdit3, CommonClass.UserInfo.CauseName, CommonClass.UserInfo.AreaName, true);
            cbState.Text = "已上传";

            comboBoxEdit1.Text = "全部";
            comboBoxEdit2.Text = "全部";
            CauseName.Text = CommonClass.UserInfo.CauseName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            comboBoxEdit3.Text = CommonClass.UserInfo.WebName;

        }

        //提取
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            string bsite = comboBoxEdit1.Text.Trim() == "全部" ? "%%" : comboBoxEdit1.Text.Trim();
            string esite = comboBoxEdit2.Text.Trim() == "全部" ? "%%" : comboBoxEdit2.Text.Trim();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", bsite));
                list.Add(new SqlPara("esite", esite));
                list.Add(new SqlPara("t1", dateEdit1.EditValue.ToString()));
                list.Add(new SqlPara("t2", dateEdit2.EditValue.ToString()));
                list.Add(new SqlPara("state", cbState.SelectedIndex));
                list.Add(new SqlPara("datetype", datetype.SelectedIndex));
                list.Add(new SqlPara("BegWeb", comboBoxEdit3.Text.Trim()=="全部"?"%%":comboBoxEdit3.Text.Trim()));
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_NOupload_ByReceipt2", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string huidanqueren = ds.Tables[0].Rows[i]["huidanqueren"].ToString();
                    if (huidanqueren == "")
                    {
                        ds.Tables[0].Rows[i]["huidanqueren"] = "未审核";
                    }
                }
                    //ds.Tables[0].Columns.Add("isChecked");
                    gridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView1, "回单确认记录");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout("回单确认记录");
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView1, "回单信息");
        }

      

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确认？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
       //     string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            string BegWeb = GridOper.GetRowCellValueString(gridView1, rowhandle, "BegWeb");
            string webname = CommonClass.UserInfo.WebName;
            if (BegWeb != webname)
            {
                MsgBox.ShowOK("自己部门只能确认自己部门的单！");
                return;
            }
            string billNos = "";
            if (rowhandle >= 0)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(gridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        billNos += gridView1.GetRowCellValue(i, "BillNo") + ",";
                        if (billNos == "") return;
                    }
                }
            }

            //try
            //{
            //    List<SqlPara> list2 = new List<SqlPara>();
            //    list2.Add(new SqlPara("BillNos", billNos));
            //    SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "USP_get_WAYBILL_huidanqueren", list2);
            //    DataSet ds2 = SqlHelper.GetDataSet(spe2);
            //    if (ds2.Tables[0].Rows.Count > 0)
            //    {
            //        string a = ds2.Tables[0].Rows[0]["huidanqueren"].ToString();
            //        if (a == "已审核")
            //        {
            //            MsgBox.ShowOK("已审核的单不能再确认！");
            //            return;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
            try
            {
                List<SqlPara> list = new List<SqlPara>();
            //    list.Add(new SqlPara("huidanqueren","已审核"));
                list.Add(new SqlPara("BillNos", billNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_HUIDAN_SHENHE", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    

                    
                    
                    cbRetrieve_Click(null,null);                   
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //取消
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否取消？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            string BegWeb = GridOper.GetRowCellValueString(gridView1, rowhandle, "BegWeb");
            string webname = CommonClass.UserInfo.WebName;
            if (BegWeb != webname)
            {
                MsgBox.ShowOK("只有开单网点可以取消！");
                return;
            }
            string billNos = "";
            int count = 0;
            if (rowhandle >= 0)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(gridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        count++;
                        billNos = gridView1.GetRowCellValue(i, "BillNo") + "";
                        if (billNos == "") return;
                    }
                }
            }
            if (count > 1)
            {
                MsgBox.ShowOK("此操作只能单条进行！");
                return;
            }
            if (billNos == null || billNos == "")
            {
                MsgBox.ShowOK("请先选择一条记录！");
                return;
            }
            try
            {
                List<SqlPara> list2 = new List<SqlPara>();
                list2.Add(new SqlPara("BillNo", billNos));
                SqlParasEntity spe2 = new SqlParasEntity(OperType.Query, "USP_get_WAYBILL_huidanqueren", list2);
                DataSet ds2 = SqlHelper.GetDataSet(spe2);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    string a = ds2.Tables[0].Rows[0]["huidanqueren"].ToString();
                    if (a != "已审核")
                    {
                        MsgBox.ShowOK("只有做了确认的单才能取消确认！");
                        return;
                    }
                    else
                    {
                        frmFoujue fm = new frmFoujue();
                        fm.BillNo = BillNo;
                        fm.a = 2;
                        fm.ShowDialog();
                        cbRetrieve_Click(null, null);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }


        private void repositoryItemHyperLinkEdit1_Click_1(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            fmFileShow fm = new fmFileShow();
            fm.billNo = BillNo;
            fm.billType = 7;
            fm.ShowDialog();
        }

        //hh 2018/12/30
        private void gridControl1_Click_1(object sender, EventArgs e)
        {
            int rowhandle = gridView1.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            fmFileShow fm = new fmFileShow();
            fm.billNo = BillNo;
            fm.billType = 3;
            fm.ShowDialog();
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(comboBoxEdit3, CauseName.Text, AreaName.Text);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(comboBoxEdit3, CauseName.Text, AreaName.Text);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        //2017.12.8wbw取消否决
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否取消否决？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            string BegWeb = GridOper.GetRowCellValueString(gridView1, rowhandle, "BegWeb");
            string webname = CommonClass.UserInfo.WebName;
            if (BegWeb != webname)
            {
                MsgBox.ShowOK("只有开单网点可以取消否决！");
                return;
            }
            string billNos = "";
            int count = 0;
            if (rowhandle >= 0)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(gridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        count++;
                        billNos = gridView1.GetRowCellValue(i, "BillNo") + "";
                        if (billNos == "") return;
                    }
                }
            }
            if (count > 1)
            {
                MsgBox.ShowOK("此操作只能单条进行！");
                return;
            }
            if (billNos == null || billNos == "")
            {
                MsgBox.ShowOK("请先选择一条记录！");
                return; 
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_get_WAYBILL_huidanqueren", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string a = ds.Tables[0].Rows[0]["huidanqueren"].ToString();
                    if (a != "否决" )
                    {
                        MsgBox.ShowOK("只有否决的单才能取消否决");
                    }
                    else
                    {
                        frmFoujue fm = new frmFoujue();
                        fm.BillNo = BillNo;
                        fm.a = 1;
                        fm.ShowDialog();
                        cbRetrieve_Click(null, null);
                    }

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        //2017.12.8wbw否决
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MsgBox.ShowYesNo("是否否决？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            int rowhandle = gridView1.FocusedRowHandle;
            string BillNo = GridOper.GetRowCellValueString(gridView1, rowhandle, "BillNo");
            string BegWeb = GridOper.GetRowCellValueString(gridView1, rowhandle, "BegWeb");
            string webname = CommonClass.UserInfo.WebName;
            if (BegWeb != webname)
            {
                MsgBox.ShowOK("只有开单网点可以否决！");
                return;
            }
            string billNos = "";
            int count = 0;
            if (rowhandle >= 0)
            {
                for (int i = 0; i < gridView1.RowCount; i++)
                {
                    if (ConvertType.ToInt32(gridView1.GetRowCellValue(i, "isChecked")) > 0)
                    {
                        count++;
                        billNos = gridView1.GetRowCellValue(i, "BillNo") + "";
                        if (billNos == "") return;
                    }
                }
            }
            if (count > 1)
            {
                MsgBox.ShowOK("此操作只能单条进行！");
                return;
            }
            if (billNos == null || billNos == "")
            {
                MsgBox.ShowOK("请先选择一条记录！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", billNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_get_WAYBILL_huidanqueren", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string a = ds.Tables[0].Rows[0]["huidanqueren"].ToString();
                    if (a != "未审核" && a != "")
                    {
                        MsgBox.ShowOK("只有未审核的单才能否决");
                    }
                    else
                    {
                        frmFoujue fm = new frmFoujue();
                        fm.BillNo = BillNo;
                        fm.a = 3;
                        fm.ShowDialog();
                        cbRetrieve_Click(null, null);
                    }

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }

        private void gridView1_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Column.Name.Equals("isChecked"))
            {
                string str= this.gridView1.GetFocusedRowCellValue("isChecked").ToString();
                if (str.Equals("1"))
                {
                    this.gridView1.SetFocusedRowCellValue("isChecked", "0");
                }
                else
                {
                    this.gridView1.SetFocusedRowCellValue("isChecked", "1");
                }
            }
        }

        //双击显示运单信息
        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int rows = gridView1.FocusedRowHandle;
            string a = gridView1.GetRowCellValue(rows, "BillNo").ToString();
            if (rows < 0) return;
            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type type = ass.GetType("ZQTMS.UI.frmBillSearchControl");
            if (type == null) return;
            Form frm = (Form)Activator.CreateInstance(type);
            if (frm == null) return;
            frm.StartPosition = FormStartPosition.CenterScreen;
            //string param = string.Format("{0} ", a);
            //System.Diagnostics.Process process = new Process();
            //process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\frmBillSearch.exe";
            //process.StartInfo.Arguments = param;
            //process.Start();

            //frm.crrBillNO = a;
            frm.Tag = a;
            frm.ShowDialog();
        }
    }
}