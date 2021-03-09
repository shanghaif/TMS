using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmDepositManagement : BaseForm
    {
        public frmDepositManagement()
        {
            InitializeComponent();
        }

        private void frmDepositManagement_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("预提信息管理");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.SetCause(txtCauseName, true);
            CommonClass.SetArea(txtAreaName, txtCauseName.Text, true);
            GridOper.CreateStyleFormatCondition(myGridView1, "qrState", FormatConditionEnum.Equal, "已确认", Color.Yellow);
            txtCauseName.Text = CommonClass.UserInfo.CauseName;
            txtAreaName.Text = CommonClass.UserInfo.AreaName;
            txtWebName.Text = CommonClass.UserInfo.WebName;
        }

        /// <summary>
        /// 设置确认状态
        /// </summary>
        /// <param name="flag">1确认,2取消确认</param>
        public void SetYuTiQueRenState(int flag)
        {
            //if (xtraTabControl1.SelectedTabPage != xtraTabPage2) return;
            myGridView1.PostEditor();
            string keyId = "";
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return;
            dt.AcceptChanges();
            int rowHandle = myGridView1.FocusedRowHandle;
            int checkType = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowHandle, "ischecked").ToString());
            if (checkType == 0)
            {
                keyId += myGridView1.GetRowCellValue(rowHandle, "Id").ToString() + "@";
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                    if (flag == 1 && dr["qrState"].ToString() == "已确认") continue;
                    if (flag == 2 && dr["qrState"].ToString() == "未确认") continue;
                    keyId += dr["Id"] + "@";
                }
            }
                    
            if (keyId=="")
            {
                MsgBox.ShowOK(flag == 1 ? "请选择要确认的行" : "请选择要取消确认的行");
                return;
            }
            if (MsgBox.ShowYesNo(flag == 1 ? "要操作确认么？" : "要操作取消确认么？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id",keyId));
            list.Add(new SqlPara("Type",flag));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_DepositQueRen_STATE", list)) > 0)
            {
                MsgBox.ShowOK();
                return;
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddDepositInfo frm = new frmAddDepositInfo();
            frm.ShowDialog();
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle<0)
                {
                    MsgBox.ShowOK("请选择你要修改的信息！");
                    return;
                }
                #region 要修改的列值
                string id = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Id");
                string cause = GridOper.GetRowCellValueString(myGridView1, rowHandle, "CDCauseName");
                string area = GridOper.GetRowCellValueString(myGridView1, rowHandle, "CDAreaName");
                string web = GridOper.GetRowCellValueString(myGridView1, rowHandle, "CDWebName");
                string proType = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ProvisionType");
                string proSub = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ProvisionSub");
                string mark = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ReMark");
                decimal proAmount = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, rowHandle, "ProvisionAmount"));
                string flowID = GridOper.GetRowCellValueString(myGridView1, rowHandle, "FlowId");
                frmAddDepositInfo frm = new frmAddDepositInfo();
                frm.id = id;
                frm.cause = cause;
                frm.area = area;
                frm.web = web;
                frm.proType = proType;
                frm.proSub = proSub;
                frm.mark = mark;
                frm.proAmount = proAmount;
                frm.flowID = flowID;
                frm.ShowDialog();
                #endregion
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            
        }
        /// <summary>
        /// 删除
        /// 单条删除或批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                myGridView1.PostEditor();
                string keyId = "";
                DataTable dt = myGridControl1.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0) return;
                dt.AcceptChanges();
                int rowHandle = myGridView1.FocusedRowHandle;
                int checkType = ConvertType.ToInt32(myGridView1.GetRowCellValue(rowHandle, "ischecked").ToString());

                if (checkType == 0)
                {
                    keyId += myGridView1.GetRowCellValue(rowHandle, "Id").ToString() + "@";
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                        keyId += dr["Id"] + "@";
                    }
                }
                if (string.IsNullOrEmpty(keyId))
                {
                    MsgBox.ShowOK("请选择你要删除的数据！");
                    return;
                }
                //string id = GridOper.GetRowCellValueString(myGridView1,rowHandle,"Id");
                if (MsgBox.ShowYesNo("确定要删除么？") != DialogResult.Yes) return;
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_DEL_YUTI_Info", new List<SqlPara>() { new SqlPara("ID", keyId) })) > 0)
                {
                    MsgBox.ShowOK();
                    btnGetInfo_Click(sender, e);
                }
                else
                {
                    MsgBox.ShowOK("删除失败");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmYTImport frm = new frmYTImport();
            frm.ShowDialog();
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1,"预提信息记录");
        }
        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetYuTiQueRenState(1);
        }
        /// <summary>
        /// 取消确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetYuTiQueRenState(2);
        }
        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("t1",bdate.DateTime));
                list.Add(new SqlPara("t2",edate.DateTime));
                list.Add(new SqlPara("CauseName", txtCauseName.Text.Trim() == "全部" ? "%%" : txtCauseName.Text.Trim()));
                list.Add(new SqlPara("AreaName", txtAreaName.Text.Trim() == "全部" ? "%%" : txtAreaName.Text.Trim()));
                list.Add(new SqlPara("WebName", txtWebName.Text.Trim() == "全部" ? "%%" : txtWebName.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_YAJIN_Info",list));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
                //myGridControl2.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 事业部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(txtAreaName, txtCauseName.Text.Trim(), true);
            CommonClass.SetDep(txtWebName, txtAreaName.Text.Trim(), true);
        }
        /// <summary>
        /// 大区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetDep(txtWebName, txtAreaName.Text.Trim(), true);
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAll_CheckedChanged(object sender, EventArgs e)
        {
            int o = checkAll.Checked == true ? 1 : 0;
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                myGridView1.SetRowCellValue(i, "ischecked", o);
            }
        }

       
    }
}