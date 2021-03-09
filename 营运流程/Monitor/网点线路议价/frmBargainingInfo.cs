using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBargainingInfo : BaseForm
    {
        public frmBargainingInfo()
        {
            InitializeComponent();
        }

        private void frmBargainingInfo_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem2);

            CommonClass.SetCause(Cause, true);
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetDep(WebName, Area.Text);
            
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            WebName.Text = CommonClass.UserInfo.WebName;
           // GridOper.GetGridViewColumn(myGridView1, "confirmState").AppearanceCell.BackColor = Color.Yellow;
            GridOper.CreateStyleFormatCondition(myGridView1, "confirmState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "否决", Color.Red);//zb20190715
            GridOper.CreateStyleFormatCondition(myGridView1, "confirmState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已执行", Color.LightGreen);//zb20190715
           
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBargainingAdd frm = new frmBargainingAdd();
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
                string id = "", billno = "", site = "", web = "", transLine = "", transMode = "", varieties = "", num = "", mainFee = "", mainWeight = "", perPrice = "", newMainfee = "",
                confirmState = "", actualFee = "", transiteSite = "", remark = "", deliveryFee = "", newDeliveryFee = "", deliveryFeePer="", acceptCompany="";
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0) return;
                confirmState = GridOper.GetRowCellValueString(myGridView1, rowHandle, "confirmState");
                if (confirmState !="待执行")
                {
                    MsgBox.ShowOK("数据已经否决/执行，不能修改！");
                    return;
                }
                billno = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillNo");
                site = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillSite");
                web = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillWeb");
                transLine = GridOper.GetRowCellValueString(myGridView1, rowHandle, "TransitLines");
                transMode = GridOper.GetRowCellValueString(myGridView1, rowHandle, "TransitMode");
                varieties = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Varieties");
                num = GridOper.GetRowCellValueString(myGridView1, rowHandle, "Num");
                mainFee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "mainLineFee");
                mainWeight = GridOper.GetRowCellValueString(myGridView1, rowHandle, "OperationWeight");
                perPrice = GridOper.GetRowCellValueString(myGridView1, rowHandle, "mainLinePer");
                newMainfee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "newMainLinefee");
                id = GridOper.GetRowCellValueString(myGridView1, rowHandle, "priceId");
                actualFee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ActualFreight");
                transiteSite = GridOper.GetRowCellValueString(myGridView1, rowHandle, "TransferSite");
                remark = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BargainingMark");
                deliveryFee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "DeliveryFee");
                newDeliveryFee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "NewDeliveryFee");
                deliveryFeePer = GridOper.GetRowCellValueString(myGridView1, rowHandle, "DeliveryFeePer");
                acceptCompany = GridOper.GetRowCellValueString(myGridView1, rowHandle, "AcceptCompany");


                frmBargainingAdd frm = new frmBargainingAdd();
                frm.site = site;
                frm.web = web;
                frm.transLine = transLine;
                frm.transMode = transMode;
                frm.varieties = varieties;
                frm.num = num;
                frm.mainFee = mainFee;
                frm.mainWeight = mainWeight;
                frm.perPrice = perPrice;
                frm.newMainfee = newMainfee;
                frm.billno = billno;
                frm.id = id;
                frm.actualFee = actualFee;
                frm.transitSite = transiteSite;
                frm.remark = remark;
                frm.deliveryFee = deliveryFee;
                frm.newDeliveryFee = newDeliveryFee;
                frm.deliveryFeePer = deliveryFeePer;
                frm.acceptCompany = acceptCompany;

                frm.statu = "1";
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.Yes)
                {
                    btnSearch_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            List<SqlPara> lst = new List<SqlPara>();
            try
            {
                lst.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                lst.Add(new SqlPara("bdate",bdate.DateTime));
                lst.Add(new SqlPara("edate",edate.DateTime));
                lst.Add(new SqlPara("Cause", Cause.Text.Trim()=="全部"?"%%" : Cause.Text.Trim()));
                lst.Add(new SqlPara("Area", Area.Text.Trim() == "全部" ? "%%" : Area.Text.Trim()));
                lst.Add(new SqlPara("webName", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_BargainingInfo_New", lst));
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
               
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0) return;
                string confirmState = GridOper.GetRowCellValueString(myGridView1, rowHandle, "confirmState");
                string id = GridOper.GetRowCellValueString(myGridView1, rowHandle, "priceId");
                 string billno = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillNo")+",";
                string applywebname = GridOper.GetRowCellValueString(myGridView1, rowHandle, "ApplyWebName");
                string acceptCompany = GridOper.GetRowCellValueString(myGridView1, rowHandle, "AcceptCompany");
                if (applywebname!=CommonClass.UserInfo.WebName)
                {
                    MsgBox.ShowOK("只有申请网点才可以取消申请！");
                    return;
                }
                if (confirmState !="待执行")
                {
                    MsgBox.ShowOK("数据已经/否决/执行，不能取消申请！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要删除吗？") == DialogResult.No) return;

                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_DEL_BargainingInfo", new List<SqlPara>() { new SqlPara("ID",id)}))>0)
                {
                    MsgBox.ShowOK();
                    btnSearch_Click(sender, e);
                    #region 239公司执行异步同步
                    if (acceptCompany == "239")
                    {
                        CommonSyn.DelBargainAPPLYYDSYN(id, billno);
                    }
                    #endregion
                    
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            setState(1);
            btnSearch_Click(sender, e);
        }
        /// <summary>
        /// 否决
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            setState(2);
            btnSearch_Click(sender, e);
        }

        private void setState(int flag)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0) return;
                string id = GridOper.GetRowCellValueString(myGridView1, rowHandle, "priceId");
                string newMainfee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "newMainLinefee");
                string newDeliveryFee = GridOper.GetRowCellValueString(myGridView1, rowHandle, "NewDeliveryFee");
                string billno = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillNo");
                string confirmState = GridOper.GetRowCellValueString(myGridView1, rowHandle, "confirmState");
                string applyWebName = GridOper.GetRowCellValueString(myGridView1, rowHandle, "BillWeb");
                string inputSerialNumber = CommonClass.gcdate.ToString("yyyyMMddHHmmsss") + new Random().Next(1000, 10000);
                if (flag == 1)
                {
                    
                    if (confirmState == "否决")
                    {
                        MsgBox.ShowOK("已否决，不能再审核！");
                        return;
                    }
                    if (confirmState == "已执行")
                    {
                        MsgBox.ShowOK("已执行，不能再审核！");
                        return;
                    }
                    if (MsgBox.ShowYesNo("确定完成审核吗？") == DialogResult.No) return;
                }
                else if (flag==2)
                {
                    if (confirmState == "否决")
                    {
                        MsgBox.ShowOK("已否决，无需再否决！");
                        return;
                    }
                    if (confirmState == "已执行")
                    {
                        MsgBox.ShowOK("已执行，不能再否决！");
                        return;
                    }
                    frmBargainingFJ frm = new frmBargainingFJ();
                    frm.id = id;
                    frm.newMainfee = newMainfee;
                    frm.newDeliveryFee = newDeliveryFee;
                    frm.billno = billno;
                    frm.confirmState = confirmState;
                    frm.applyWebName = applyWebName;
                    frm.inputSerialNumber = inputSerialNumber;
                    frm.ShowDialog();
                }
                if (flag == 3)
                {
                    if (confirmState == "否决")
                    {
                        MsgBox.ShowOK("已否决，不能再执行！");
                        return;
                    }
                    if (confirmState == "已执行")
                    {
                        MsgBox.ShowOK("已执行，不能再执行！");
                        return;
                    }
                    if (MsgBox.ShowYesNo("确定完成执行吗？") == DialogResult.No) return;
                }
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "QSP_CONFIRM_BargainingInfo", new List<SqlPara>() { new SqlPara("ID", id), new SqlPara("flag", flag), new SqlPara("newMainfee", newMainfee), new SqlPara("newDeliveryFee", newDeliveryFee), new SqlPara("billno", billno), new SqlPara("applyWeb", applyWebName), new SqlPara("inputSerialNumber", inputSerialNumber) })) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }
        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }
        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "网点线路议价信息");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(WebName, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, Cause.Text, Area.Text);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            setState(3);
            btnSearch_Click(sender, e);
        } 
     }
}