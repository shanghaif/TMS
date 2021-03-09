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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class frmVehicleWithhold : BaseForm
    {
        public frmVehicleWithhold()
        {
            InitializeComponent();
        }
        private string state = string.Empty;
        GridColumn gcIsseleckedMode;
      
        private void frmVehicleWithhold_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("车辆代扣款登记");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            BarMagagerOper.SetBarPropertity(barbtn_cancelCon);

            CommonClass.SetArea(cbbArea, "全部", true);
            CommonClass.SetWeb(cbbWeb, true);
            CommonClass.SetCause(cbbCause, true);
            cbbState.Text = "全部";
            
            cbbCause.Text = CommonClass.UserInfo.CauseName;
            cbbArea.Text = CommonClass.UserInfo.AreaName;
            cbbWeb.Text = CommonClass.UserInfo.WebName;
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;           
            cbb_Type.EditValue = "全部";
            cmbUse.Text = "全部";
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
            GridOper.CreateStyleFormatCondition(myGridView1, "State3", DevExpress.XtraGrid.FormatConditionEnum.Equal, "核销", Color.Yellow);
            GridOper.CreateStyleFormatCondition(myGridView1, "State3", DevExpress.XtraGrid.FormatConditionEnum.Equal, "部分核销", Color.FromArgb(100, 149, 237));
            FixColumn fix = new FixColumn(myGridView1, barSubItem6);
        }
        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            select();
        }
        private void select()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("begDate", bdate.Text.Trim()));
                list.Add(new SqlPara("endDate", edate.Text.Trim()));
                //list.Add(new SqlPara("DriverName", txt_Driver.Text.Trim() == "" ? "%%" : "%" + txt_Driver.Text.Trim() + "%"));
                // list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                list.Add(new SqlPara("VehicleUse", cmbUse.Text.Trim() == "全部" ? "%%" : cmbUse.Text.Trim()));
                list.Add(new SqlPara("WithholdType", cbb_Type.Text.Trim() == "全部" ? "%%" : cbb_Type.Text.Trim()));
                list.Add(new SqlPara("CauseName", cbbCause.Text.Trim() == "全部" ? "%%" :cbbCause.Text.Trim() ));
                list.Add(new SqlPara("AreaName", cbbArea.Text.Trim() == "全部" ? "%%" :cbbArea.Text.Trim() ));
                list.Add(new SqlPara("WebName", cbbWeb.Text.Trim() == "全部" ? "%%" : cbbWeb.Text.Trim()));
                list.Add(new SqlPara("State", cbbState.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_VehicleWitholdByCondition", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0||ds.Tables[0].Rows.Count==0) return;
                DataRow dr=ds.Tables[0].Rows[0];
           //  oriMoney= ConvertType.ToDecimal(  dr["thisTimeMoney"].ToString());
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barbtn_Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVehicleWithholdAdd add = new frmVehicleWithholdAdd();
            add.oper = "1";
            add.ShowDialog();
        }

        //删除
        private void barbtn_del_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            //int rowhandle = myGridView1.FocusedRowHandle;
            //string id = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
           
            //if (rowhandle < 0)
            //{
            //    MsgBox.ShowOK("请选择要删除的信息！");
            //    return;
            //}
            //string RegisterMan = myGridView1.GetRowCellValue(rowhandle, "RegisterMan").ToString();
            //if (CommonClass.UserInfo.WebName.IndexOf("财务") <= 0)
            //{
            //    if (RegisterMan != CommonClass.UserInfo.UserName)
            //    {
            //        MsgBox.ShowOK("只能删除自己登记的数据");
            //        return;
            //    }

            //}
            //string hState = myGridView1.GetRowCellValue(rowhandle, "State").ToString();
            //if (hState == "确认" || hState == "核销" || hState == "反核销")
            //{
            //    MsgBox.ShowOK("此记录不允许删除！");
            //    return;
            //}
            //else 
            //{ 
            //   if(MsgBox.ShowYesNo("确认要删除吗？")==DialogResult.Yes)
            //   {
            //            state = "0";
            //            excuteOper(id);
            //            //myGridView1.UpdateCurrentRow();
            //            select();                        
            //   }
            //}  
            try
            {
                string id = "";
                state = "0";
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1)
                    {
                        string RegisterMan = dr["RegisterMan"].ToString();
                        string CarNo = dr["CarNo"].ToString();
                       // if (CommonClass.UserInfo.WebName.IndexOf("财务") <= 0)
                        //{
                            if (RegisterMan != CommonClass.UserInfo.UserName)
                            {
                                MsgBox.ShowOK("车牌号为：【"+CarNo+"】的记录不是操作人登记的记录，不能做删除操作，只能删除自己登记的数据！");
                                return;
                            }

                       // }
                        if (hState == "确认" || hState == "核销" || hState == "反核销")
                        {
                            MsgBox.ShowOK("车牌号为：【" + CarNo + "】的记录已经确认，不能做删除操作！");
                            return;
                        }
                        else
                        {
                            
                                id += dr["ID"].ToString() + "@";
                                //state = "0";
                                //excuteOper(id);
                                ////myGridView1.UpdateCurrentRow();
                                //select();
                            
                        }
                    }
                    //if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "新开代扣款" || hState == "取消确认" || hState == "核销"))
                    //{
                    //    MsgBox.ShowOK("您选中的单里面有不能做确认操作的单，请重新选择！");
                    //    return;

                    //}
                    //if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "确认" || hState == "反核销"))
                    //{
                    //    id += dr["ID"].ToString() + "@";
                    //}
                }
                if (id == "")
                {
                    MsgBox.ShowOK("请选择要删除的单！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要批量删除吗？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量删除成功！");
                    select();
                }
            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }

         
        }

 

        //取消确认
        private void barbtn_DisCon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {   
            try
            {
                string id = "";
                state = "3";
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "新开代扣款" || hState == "取消确认" || hState == "核销"))
                    {
                        MsgBox.ShowOK("您选中的单里面有不能做确认操作的单，请重新选择！");
                        return;

                    }
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "确认" || hState == "反核销"))
                    {
                        id += dr["ID"].ToString() + "@";
                    }
                }
                if (id == "")
                {
                    MsgBox.ShowOK("请选择要确认的单！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要批量取消确认吗？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量取消确认确认操作成功！");                   
                    select();
                }
            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }
        }


        //反核销
        private void barbtn_DisCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string id = "";
                state = "5";
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState!="核销"))
                    {
                        MsgBox.ShowOK("您选中的单中有未核销的单，请重新选择！");
                        return;
                    }

                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "核销"))
                    {
                        id += dr["ID"].ToString() + "@";

                    }
                }
                if (id == "")
                {
                    MsgBox.ShowOK("请选择要反审核的单！");
                    return;
                }
                if (MsgBox.ShowYesNo("确认批量反核销核销？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量反核销操作成功！");
                    select();
                }
            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }     
        }
        //修改
        private void barbtn_modify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            string hState = myGridView1.GetRowCellValue(rowhandle, "State2").ToString();
            string id = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            string certificateNo = myGridView1.GetRowCellValue(rowhandle, "CertificateNo").ToString();
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择要修改的信息！");
                return;
            }
            string WithholdType = myGridView1.GetRowCellValue(rowhandle, "WithholdType").ToString();

            string RegisterMan = myGridView1.GetRowCellValue(rowhandle, "RegisterMan").ToString();
            if (CommonClass.UserInfo.WebName.IndexOf("财务") <= 0)
            {
                if (RegisterMan != CommonClass.UserInfo.UserName)
                {
                    MsgBox.ShowOK("只能修改自己登记的数据");
                    return;
                }
            }
            if (WithholdType == "油料费")
            {
                MsgBox.ShowOK("代扣类型为油料费时不允许在此模块修改，请到油料登记模块进行修改！");
                return;
            }
            if (hState == "确认" || hState == "核销" || hState == "反核销")
            {
                MsgBox.ShowOK("此记录不允许修改！");
                return;
            }
            else
            {
                frmVehicleWithholdAdd edit = new frmVehicleWithholdAdd();
                edit.id = id;
                edit.oper = "2";               
                edit.ShowDialog();
            }
        }

        private void excuteOper(string id)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = null;
                if (Validate())
                {
                    list.Add(new SqlPara("state", state));
                    list.Add(new SqlPara("id", id));
                    list.Add(new SqlPara("dept",CommonClass.UserInfo.DepartName));
                    list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                    sps = new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK("操作成功！");
                        //this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void barbtn_Export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "车辆待扣款登记信息");
        }

        private void barbtn_exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barbtn_look_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    

        //批量确认
        private void barbtn_mulconfirm_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           // myGridView1.FocusedRowHandle += 1;
            try
            {
                string id = "";
                state = "2";
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "确认" || hState == "核销" || hState == "反核销"))
                    {
                        MsgBox.ShowOK("您选中的单里面有不能做确认操作的单，请重新选择！");
                        return;

                    }


                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "新开代扣款" || hState == "取消确认"))
                    {
                        id += dr["ID"].ToString() + "@";

                    }
                }
                if (id == "")
                {
                    MsgBox.ShowOK("请选择要确认的单！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要批量确认吗？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));


                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量确认操作成功！");
                    select();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //批量审核
        private void barbtn_mulCheck_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            try
            {
                string id = "";
                state = "4";
                int count = 0;
                float sumMoney = 0f;
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                string money = "";
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "新开代扣款" || hState == "取消确认" ))
                    {

                        MsgBox.ShowOK("您选中的单中有没有做确认或已核销的单，请重新选择！");
                        return;

                    }
                    if (ConvertType.ToDecimal(dr["thisTimeMoney"].ToString())==0)
                
                    {

                        MsgBox.ShowOK("请输入核销金额");
                        return;

                    }


                    if (ConvertType.ToInt32(dr["ischecked"]) == 1 && (hState == "确认" || hState == "反核销"|| hState == "核销"))
                    {

                        if (dr["State2"].ToString().Equals("核销") && Convert.ToDecimal(dr["thisTimeMoney"].ToString()) <= Convert.ToDecimal(dr["unVerifyMoney"].ToString()) ||

                            !dr["State2"].ToString().Equals("核销") && Convert.ToDecimal(dr["thisTimeMoney"].ToString()) <= Convert.ToDecimal(dr["WithholdMoney"].ToString())||
                    dr["State2"].ToString().Equals("核销") && Convert.ToDecimal(dr["unVerifyMoney"].ToString()) > 0 && Convert.ToDecimal(dr["thisTimeMoney"].ToString()) <= Convert.ToDecimal(dr["unVerifyMoney"].ToString())
                            )
                        {
                            id += dr["ID"].ToString() + "@";
                            money += dr["thisTimeMoney"].ToString() + "@";
                            count++;
                            sumMoney += float.Parse(dr["thisTimeMoney"].ToString());

                        }
                        else
                        {

                            MsgBox.ShowError("输入的金额不正确或者已经核销");
                            return;
                        }
                    }
                  
                }

                if (id == "")
                {
                    MsgBox.ShowOK("请选择要审核的单！");
                    return;
                }
               
                if (MsgBox.ShowYesNo("本次核销共" + count + "笔，核销金额：" + sumMoney + "元，确认核销？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("money", money));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));


                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量核销操作成功！");
                    select();
                }
            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }

        }     

        private void barbtn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVehicleWithholdLead lead = new frmVehicleWithholdLead();
            lead.ShowDialog();

        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }


        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1,myGridView1.Guid.ToString());
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1,myGridView1.Guid.ToString());
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void cbbCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(cbbArea, cbbCause.Text);
            CommonClass.SetCauseWeb(cbbWeb, cbbCause.Text, cbbArea.Text);
        }

        private void cbbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(cbbWeb, cbbCause.Text, cbbArea.Text);
        }

        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            int a = chkALL.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
            }
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string id = "";
                state = "0";
                myGridView1.PostEditor();
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                foreach (DataRow dr in dt.Rows)
                {
                    string hState = dr["State2"].ToString();
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (ConvertType.ToInt32(dr["ischecked"]) == 1)
                    {
                        string RegisterMan = dr["RegisterMan"].ToString();
                        string CarNo = dr["CarNo"].ToString();
                        //if (CommonClass.UserInfo.WebName.IndexOf("财务") <= 0)
                        //{
                        //    if (RegisterMan != CommonClass.UserInfo.UserName)
                        //    {
                        //        MsgBox.ShowOK("车牌号为：【" + CarNo + "】的记录不是操作人登记的记录，不能做删除操作，只能删除自己登记的数据！");
                        //        return;
                        //    }

                        //}
                        if (hState == "确认" || hState == "核销" || hState == "反核销")
                        {
                            MsgBox.ShowOK("车牌号为：【" + CarNo + "】的记录已经确认，不能做删除操作！");
                            return;
                        }
                        else
                        {
                            id += dr["ID"].ToString() + "@";  
                        }
                    }
            
                }
                if (id == "")
                {
                    MsgBox.ShowOK("请选择要删除的单！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要批量删除吗？") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ID", id));
                list.Add(new SqlPara("State", state));
                list.Add(new SqlPara("dept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("man", CommonClass.UserInfo.UserName));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_vehicleWithold_mult", list)) > 0)
                {
                    MsgBox.ShowOK("批量删除操作成功！");
                    select();
                }
            }
            catch (Exception ex)
            {

                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            driverCheckAccount frm = new driverCheckAccount();
            frm.Show();
        }
        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string ID = myGridView1.GetRowCellValue(rowhandle, "ID").ToString();
            frmVehicleWithhold_LOG frm = new frmVehicleWithhold_LOG();
            frm.ID = ID;
         
            frm.ShowDialog();
             
        }

    }
}
