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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;

namespace ZQTMS.UI
{
    public partial class frmBillDelivery : BaseForm
    {
        public frmBillDelivery()
        {
            InitializeComponent();
        }

        //菜单参数值，控制弹出窗体类型,MenuType=Apply(派车申请)，MenuType=dispatch(派车单)
        public string MenuType = "";
        public frmBillDelivery(string MenuType1)
        {
            InitializeComponent();
            MenuType = MenuType1;
            if (MenuType.Contains("apply"))
            {
                this.Text = "用车申请";
            }
            else
            {
                this.Text = "调度派车";
            }
        }

        private void frmBillDelivery_Load(object sender, EventArgs e)
        {
            if (MenuType.Contains("apply")) {
                CommonClass.InsertLog("用车申请"); //xj2019/5/30
            }
            else
            {
                CommonClass.InsertLog("调度派车"); //xj2019/5/30
            }
           
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fis = new FixColumn(myGridView1, barSubItem2);

            //设置颜色显示
            GridOper.CreateStyleFormatCondition(myGridView1, "VehiclesDemand", DevExpress.XtraGrid.FormatConditionEnum.Equal, "必提货", Color.Red);
            GridOper.CreateStyleFormatCondition(myGridView1, "VehiclesDemand", DevExpress.XtraGrid.FormatConditionEnum.Equal, "准时提", Color.Green);
            GridOper.CreateStyleFormatCondition(myGridView1, "VehiclesDemand", DevExpress.XtraGrid.FormatConditionEnum.Equal, "正常用车", Color.White);
            GridOper.CreateStyleFormatCondition(myGridView1, "DeliState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "已派车", Color.FromArgb(255, 255, 128));
            GridOper.CreateStyleFormatCondition(myGridView1, "DeliState", DevExpress.XtraGrid.FormatConditionEnum.Equal, "处理完成", Color.FromArgb(255, 255, 128));


            //if (MenuType.Contains("apply"))     //ZJF20181109
            //{
            //    barButtonItem10.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //    barButtonItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //    //barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //}
            //else
            //{
            //    barButtonItem2.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //    barButtonItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //    barButtonItem6.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            //}
            bdate.Text = CommonClass.gbdate.ToString("yyyy-MM-dd HH:mm");
            dateEdit1.Text = CommonClass.gedate.ToString("yyyy-MM-dd HH:mm");
            CommonClass.SetSite(cmbDepartCenter, true);
            cmbDepartCenter.Text = CommonClass.UserInfo.SiteName;

            string[] DeliveryStateList = CommonClass.Arg.DeliveryState.Split(',');
            if (DeliveryStateList.Length > 0)
            {
                for (int i = 0; i < DeliveryStateList.Length; i++)
                {
                    if (DeliveryStateList[i] == "作废" && this.Text == "调度派车")
                        continue;
                    comboBoxEdit1.Properties.Items.Add(DeliveryStateList[i]);
                }
                comboBoxEdit1.SelectedIndex = 0;
            }
            if (this.Text == "调度派车")
            {
                //depart.Visible = true;
                //depart.EditValue = CommonClass.UserInfo.DepartName;
                //label5.Left = label4.Left;
                //depart.Left = comboBoxEdit1.Left + 15;
                //label4.Left = label4.Left + label5.Width + depart.Width + 5;
                //comboBoxEdit1.Left = comboBoxEdit1.Left + label5.Width + depart.Width + 5;
                //btnClose.Left = btnClose.Left + label5.Width + depart.Width + 5;
                //btnRetrieve.Left = btnRetrieve.Left + label5.Width + depart.Width + 5;

            }
            else
            {
                //depart.Visible = false;
                //label5.Visible = false;
            }
            GetControlWeb();
        }

        private void GetControlWeb()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE_ToControlWeb");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count > 0)
                {
                    depart.Properties.Items.Add("全部");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        depart.Properties.Items.Add(row["ControlWeb"]);
                    }
                }
                depart.Text = "全部";
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 提取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("dFrom", bdate.Text));
                list.Add(new SqlPara("dTo", dateEdit1.Text));
                list.Add(new SqlPara("site", ConvertType.ToString(cmbDepartCenter.Text) == "全部" ? "%%" : ConvertType.ToString(cmbDepartCenter.Text)));
                list.Add(new SqlPara("state", ConvertType.ToString(comboBoxEdit1.Text) == "全部" ? "%%" : ConvertType.ToString(comboBoxEdit1.Text)));
                //if (depart.Visible)
                //{
                string departStr = ConvertType.ToString(depart.Text);
                list.Add(new SqlPara("depart", (string.IsNullOrEmpty(departStr) || departStr == "全部") ? "%%" : departStr));
                //}
                list.Add(new SqlPara("isControl", this.Text.Trim()));
                list.Add(new SqlPara("VehiclesDemand", VehiclesDemand.Text == "全部" ? "%%" : VehiclesDemand.Text));
                 string sPara = cmbCarOrGroup.SelectedIndex == 0 ? "QSP_GET_billdelivery" : "QSP_GET_billdelivery_Bill";
                 SqlParasEntity sps = new SqlParasEntity(OperType.Query, sPara, list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    gridDeliveryList.DataSource = ds.Tables[0];
                }
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

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBillDeliveryAdd fm = new frmBillDeliveryAdd();
            fm.Show();
            myGridView1_FocusedRowChanged(sender, null);
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
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (!ComeFrom3PL()) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DeliId", id));

                string DeliState = GetDeliState(id);

                if (ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "BillNo")) != "")
                {
                    XtraMessageBox.Show("本单已关联运单，不可作废！");
                    return;
                }
                else
                {
                    if (JudgeIsCancel(id))//限制已经作废的单不能再次作废
                    {
                        MsgBox.ShowError("该单已作废，不可再作废！");
                        return;
                    }
                    else
                    {


                        if (MsgBox.ShowYesNo("是否作废？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
                        {
                            return;
                        }
                        else
                        {
                            //if (barButtonItem3.Caption.ToString() == "作废")
                            //{
                            list.Add(new SqlPara("DelType", 1));
                            //}
                            //else 
                            //{
                            //    list.Add(new SqlPara("DelType", 0));
                            //}
                            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_BILLDELIVERY", list);
                            if (SqlHelper.ExecteNonQuery(sps) > 0)
                            {
                                MsgBox.ShowOK();

                                //myGridView1.DeleteRow(rowhandle);
                                //myGridView1.PostEditor();
                                //myGridView1.UpdateCurrentRow();
                                //myGridView1.UpdateSummary();
                                //DataTable dt = gridDeliveryList.DataSource as DataTable;
                                //dt.AcceptChanges();
                                //myGridView1_FocusedRowChanged(sender, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


        private bool JudgeIsCancel(Guid DeliveryID)
        {
             List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DeliId", DeliveryID));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELIVERY_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                if (dr["DeliState"].ToString() == "作废")
                {
                    return true;
                }
                else return false;

            }
        }

        /// <summary>
        /// 返回派车状态，未派车，已派车，处理完成
        /// </summary>
        /// <param name="DeliveryID"></param>
        /// <returns></returns>
        string GetDeliState(Guid DeliveryID)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DeliId", DeliveryID));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELIVERY_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DeliVehType = dr["DeliVehType"].ToString();
                return dr["DeliState"].ToString();
            }
        }
        //zhengjiafeng20180905 根据id取核销状态
        string GetVehFareVerifyState(Guid DeliveryID)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("DeliId", DeliveryID));

            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDELIVERY_ByID", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return "";
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                DeliVehType = dr["DeliVehType"].ToString();
                return dr["VehFareVerifyState"].ToString();
            }
        }

        /// <summary>
        /// 双击数据行查看数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());

                BillDeliveryDetail fm = new BillDeliveryDetail();
                if (MenuType.Contains("apply"))
                {
                    fm.Text = "用车申请";
                    label3.Text = "分公司";
                }
                else
                {
                    fm.Text = "调度派车";
                }
                fm.DeliveryID = id;
                fm.OperateType = "OnlyView";
                fm.isCancel = myGridView1.GetRowCellValue(rowhandle, "DeliState").ToString();
                fm.Show();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 修改备注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (!ComeFrom3PL()) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());

                string DeliState = GetDeliState(id);

                if (DeliState.Equals("处理完成"))
                {
                    XtraMessageBox.Show("本单已处理完成，不可修改！");
                    return;
                }
                else
                {
                    BillDeliveryDetail fm = new BillDeliveryDetail();
                    fm.OperateType = "EditRemark";
                    fm.DeliveryID = id;
                    fm.Show();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 调度派车
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());
               
                string DeliState = GetDeliState(id);
                if (DeliState == "作废")
                {
                    XtraMessageBox.Show("该派车单号已作废，不可调度派车！"); //zhengjiafeng20180905
                    return;
                }
                string VehFareVerifyState = GetVehFareVerifyState(id); //zhengjiafeng20180905

                if (!VehFareVerifyState.Equals("1"))
                {
                    if (DeliState.Equals("未派车"))
                    {
                        BillDeliveryDetail fm = new BillDeliveryDetail();
                        fm.OperateType = "dispatch";
                        fm.DeliveryID = id;
                        fm.Show();
                    }
                    else if ((DeliState.Equals("已派车")) || (DeliState.Equals("处理完成")))
                    {
                        if (DeliState.Equals("已派车"))
                        {

                            if (MsgBox.ShowYesNo("本单已派车，是否需要修改派车信息？") != DialogResult.Yes)
                            {
                                return;
                            }
                            else
                            {
                                BillDeliveryDetail fm = new BillDeliveryDetail();
                                fm.OperateType = "dispatch";
                                fm.DeliveryID = id;
                                fm.Show();
                            }
                        }
                        else if (DeliState.Equals("处理完成"))
                        {
                            if (MsgBox.ShowYesNo("本单已派车完成，是否需要修改派车信息？") != DialogResult.Yes)
                            {
                                return;
                            }
                            else
                            {
                                BillDeliveryDetail fm = new BillDeliveryDetail();
                                fm.OperateType = "dispatch";
                                fm.DeliveryID = id;
                                fm.Show();
                            }
                        }
                    }
                    //else   zhengjiafeng20180905
                    //{
                    //    XtraMessageBox.Show("本单已处理完成，不可派车！");
                    //    return;
                    //}
                }
                else
                {
                    XtraMessageBox.Show("本单已核销，不可派车！"); //zhengjiafeng20180905
                    return;
                }


            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }

        /// <summary>
        /// 派车完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;
                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());
                string DeliState = GetDeliState(id);

                if (DeliState.Equals("已派车"))
                {
                    if (MsgBox.ShowYesNo("是否确定完成该单？") != DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("DeliId", id));
                        list.Add(new SqlPara("DeliState", "处理完成"));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_update_BILLDELIVERY_DeliState", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                    }
                }
                else
                {
                    XtraMessageBox.Show("本单状态必须为已派车状态，否则不可标记完成！");
                    return;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 关联运单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string DeliVehType = "";
        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (!ComeFrom3PL()) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());

                string DeliState = GetDeliState(id);
                if (DeliState.Equals("未派车"))
                {
                    XtraMessageBox.Show("本单未派车，不可关联运单！");
                    return;
                }
                else
                {
                    if (JudgeIsCancel(id))//限制已经作废的单不能再次作废
                    {
                        MsgBox.ShowError("该单已作废，不可关联运单！");
                        return;
                    }
                    else
                    {
                        //3)派车类型为“接货”才须关联运单
                        if (DeliVehType.Equals("接货") || DeliVehType.Equals("二次送货"))
                        {
                            BillDeliveryDetail fm = new BillDeliveryDetail();
                            fm.OperateType = "Relation";
                            fm.DeliveryID = id;
                            fm.Show();
                        }
                        else
                        {
                            XtraMessageBox.Show("本单派车类型为" + DeliVehType + "，无须关联运单");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView1.FocusedRowHandle;
                if (rowhandle < 0) return;

                if (!ComeFrom3PL()) return;

                Guid id = new Guid(myGridView1.GetRowCellValue(rowhandle, "DeliId").ToString());               

                string DeliState = GetDeliState(id);
                if (DeliState == "作废")
                {
                    XtraMessageBox.Show("本单已作废，不可修改！");
                    return;
                }
                if (DeliState.Equals("已派车"))
                {
                    if (MsgBox.ShowYesNo("本单已派车只支持修改备注，是否继续？") != DialogResult.Yes)
                    {
                        return;
                    }
                    else
                    {
                        BillDeliveryDetail fm = new BillDeliveryDetail();
                        fm.isCancel = myGridView1.GetRowCellValue(rowhandle, "DeliState").ToString();
                        fm.OperateType = "EditRemark";
                        fm.DeliveryID = id;
                        fm.Show();
                    }
                }
                else if (DeliState.Equals("处理完成"))
                {
                    XtraMessageBox.Show("本单已处理完成，不可修改！");
                    return;
                }
                else
                {
                    BillDeliveryDetail fm = new BillDeliveryDetail();
                    fm.OperateType = "update";
                    fm.DeliveryID = id;
                    fm.Show();
                }
                myGridView1_FocusedRowChanged(sender, null);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            string DeliCode = ConvertType.ToString(myGridView1.GetFocusedRowCellValue("DeliCode"));
            if (DeliCode == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DELIVERY_BY_DELICODE", new List<SqlPara> { new SqlPara("DeliCode", DeliCode) }));
            if (ds == null || ds.Tables.Count == 0) return;

            frmPrintRuiLang fpr = new frmPrintRuiLang("派车单.grf", ds);
            fpr.ShowDialog();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void cmbDepartCenter_EditValueChanged(object sender, EventArgs e)
        {
            //depart.Properties.Items.Clear();
            //DataSet ds = CommonClass.dsSite;
            //if (ds != null && ds.Tables.Count > 0)
            //{
            //    DataRow[] rows = ds.Tables[0].Select("SiteName='" + cmbDepartCenter.Text.Trim() + "'");
            //    if (rows != null && rows.Length > 0)
            //    {
            //        string departStr = ConvertType.ToString(rows[0]["ControlWeb"]);
            //        if (!string.IsNullOrEmpty(departStr))
            //        {
            //            depart.Properties.Items.Add(departStr);
            //        }
            //    }
            //}
            //depart.Properties.Items.Add("全部");
            //depart.Text = "全部";

            //if (cmbDepartCenter.Text == "深圳")
            //{
            //    depart.Enabled = true;
            //    if (depart.Properties.Items.Count < 1)
            //    {
            //        depart.Properties.Items.Add("深圳金华伦");
            //        depart.Properties.Items.Add("深圳广源");
            //        depart.Properties.Items.Add("全部");
            //        depart.Text = "全部";
            //    }
            //}
            //else
            //{
            //    depart.Text = "";
            //    depart.Enabled = false;
            //    //label4.Left = label4.Left - label5.Width - depart.Width - 5;
            //    //comboBoxEdit1.Left = comboBoxEdit1.Left - label5.Width - depart.Width - 5;
            //    //btnClose.Left = btnClose.Left - label5.Width - depart.Width - 5;
            //    //btnRetrieve.Left = btnRetrieve.Left - label5.Width - depart.Width - 5;

            //}
        }

        private void myGridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (ConvertType.ToString(myGridView1.GetFocusedRowCellValue("DeliVehType")) == "接货")
            //{
            //    barButtonItem3.Caption = "作废";

            //}
            //else 
            //{
            //    barButtonItem3.Caption = "删除";
            //}
        }

        /// <summary>
        /// 筛选条件框的双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCondition_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (sender as ComboBoxEdit).SelectAll();
            }
            catch { }
        }

        private bool ComeFrom3PL()
        {
            string ComeFrom = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ComeFrom") == DBNull.Value ? "" : myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ComeFrom").ToString();
            if (ComeFrom == "项目" && CommonClass.UserInfo.UserDB == UserDB.ZQTMS20160713)
            {
                XtraMessageBox.Show("来自项目的记录，不可专线不能操作！");
                return false;
            }
            return true;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            MsgBox.ShowOK("按车：车费=实际调车车费" + "\n" + "按票：车费=实际调车车费*分摊方式（如：单票车费=实际调车费*单票件数/车辆总件数）" + "\n" + "派车单界面接货费分摊逻辑:关联的件数除以开单总件数乘以派车费");

        }
    }
}