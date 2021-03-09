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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using System.Data.SqlClient;
using Newtonsoft.Json;
using DevExpress.XtraGrid;

namespace ZQTMS.UI
{
    public partial class frmDepartureAdd : BaseForm
    {
        private DataSet dataset1 = new DataSet();
        private DataSet dataset2 = new DataSet();
        GridHitInfo hitInfo = null;

        public frmDepartureAdd()
        {
            InitializeComponent();
            barEditItem3.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem3_EditValueChanging);
            barEditItem3.Edit.KeyDown += new KeyEventHandler(barEditItem3_KeyDown);
            barEditItem2.Edit.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(barEditItem2_EditValueChanging);
            barEditItem2.Edit.KeyDown += new KeyEventHandler(barEditItem2_KeyDown);
        }

        /// <summary>
        /// 提取库存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barBtnSearch_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "长途配载";
            getdata(1);
        }

        private void getdata(int type)
        {
            try
            {
                dataset1.Clear();
                //dataset2.Clear();
                myGridView1.ClearColumnsFilter();
                myGridView2.ClearColumnsFilter();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("SiteName", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webName", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadType", type));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_STOWAGE_LOAD", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                dataset1 = ds;
                if (dataset2 == null || dataset2.Tables.Count == 0) dataset2 = dataset1.Clone();
                myGridControl1.DataSource = dataset1.Tables[0];
                myGridControl2.DataSource = dataset2.Tables[0];

                if (dataset1 == null || dataset1.Tables.Count == 0 || dataset1.Tables[0].Rows.Count == 0 || dataset2 == null || dataset2.Tables.Count == 0 || dataset2.Tables[0].Rows.Count == 0) return;
                string billNo = "";
                for (int i = 0; i < dataset1.Tables[0].Rows.Count; i++)
                {
                    billNo = ConvertType.ToString(dataset1.Tables[0].Rows[i]["BillNo"]);
                    if (billNo == "") continue;
                    DataRow[] dr = dataset2.Tables[0].Select("BillNo='" + billNo + "'");
                    if (dr == null || dr.Length == 0) continue;
                    dataset1.Tables[0].Rows.RemoveAt(i);
                    i--;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmDepartureAdd_Load(object sender, EventArgs e)
        {
            if (!CommonClass.FormSet(this, false, true)) return;
            CommonClass.GetGridViewColumns(myGridView1, false, myGridView2, myGridView3);
            GridOper.SetGridViewProperty(myGridView2, myGridView2, myGridView3);
            BarMagagerOper.SetBarPropertity(bar2, bar3);
            // CommonClass.SetSite(EndSite);
            GridOper.RestoreGridLayout(myGridView1, myGridView2);
            GridOper.GetGridViewColumn(myGridView2, "factqty").AppearanceCell.BackColor = Color.Yellow;//实发件数背景为黄色
            GridOper.GetGridViewColumn(myGridView2, "ActualWeight").AppearanceCell.BackColor = Color.Yellow;//实际计费重量为黄色 hj20180706
            GridOper.GetGridViewColumn(myGridView2, "ActualVolume").AppearanceCell.BackColor = Color.Yellow;//实际计费体积为黄色 hj20180706
            GridOper.CreateStyleFormatCondition(myGridView1, "PreciousGoods", DevExpress.XtraGrid.FormatConditionEnum.Equal, "1", Color.FromArgb(192, 255, 192));

            myGridControl3.DataSource = CommonClass.dsCar.Tables[0];
            Creator.Text = LoadPeoples.Text = CommonClass.UserInfo.UserName;
            BeginSite.Text = CommonClass.UserInfo.SiteName;
            DepartureDate.DateTime = CommonClass.gcdate;

            //barBtnSearch.Enabled = UserRight.GetRight("121586");//长途
            //barButtonItem11.Enabled = UserRight.GetRight("121587");//整车
            barButtonItem12.Enabled = UserRight.GetRight("121588");//项目
            barButtonItem15.Enabled = UserRight.GetRight("121590");//整站库存

            GetSiteName();//获取左边线路
            string[] VehicleTypes = CommonClass.Arg.VehicleType.Split(',');
            for (int i = 0; i < VehicleTypes.Length; i++)
            {
                CarType.Properties.Items.Add(VehicleTypes[i]);
            }
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 0, Color.FromArgb(255, 255, 255));//颜色固定--白色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Equal, 1, Color.FromArgb(193, 255, 193));//颜色固定--绿色
            GridOper.CreateStyleFormatCondition(myGridView1, "LckDate", FormatConditionEnum.Greater, 1, Color.LightBlue);//颜色固定--浅蓝色
            ////plh20191029
        }

        /// <summary>
        /// 获取线路
        /// </summary>
        private void GetSiteName()
        {
            if (CommonClass.dsSite == null || CommonClass.dsSite.Tables.Count == 0) return;
            string site = "";
            foreach (DataRow dr in CommonClass.dsSite.Tables[0].Rows)
            {
                site = ConvertType.ToString(dr["SiteName"]);
                if (site != CommonClass.UserInfo.SiteName)
                    treeView1.Nodes.Add(site);
            }
            treeView1.Nodes.Add("全部");
        }

        public void GetWeightAndVolume()
        {
            decimal actVolume = 0;
            decimal actWeight = 0;
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                //actVolume += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Volume")) * ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "factqty")) / ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "Num")), 2);
                //actWeight += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "Weight")) * ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "factqty")) / ConvertType.ToInt32(myGridView2.GetRowCellValue(i, "Num")), 2);
                actVolume += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ActualVolume")), 2);//hj20180705
                actWeight += Math.Round(ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "ActualWeight")), 2);//hj20180705
            }
            ActVolume.Text = actVolume.ToString();
            ActWeight.Text = actWeight.ToString();
        }


        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void myGridControl1_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl2_DragDrop(object sender, DragEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnLeftOne_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnLeftMore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.SelectAll();
            GridViewMove.Move(myGridView1, dataset1, dataset2);
        }

        private void barBtnRightOne_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void barBtnRightMore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView2.SelectAll();
            GridViewMove.Move(myGridView2, dataset2, dataset1);
        }

        private void myGridControl1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl2_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void myGridControl1_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView1.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void myGridControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
           hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl1.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (hitInfo == null) return;
            Rectangle dragRect = new Rectangle(new Point(
            hitInfo.HitPoint.X - SystemInformation.DragSize.Width / 2,
            hitInfo.HitPoint.Y - SystemInformation.DragSize.Height / 2), SystemInformation.DragSize);
            if (!dragRect.Contains(new Point(e.X, e.Y)))
            {
                if (hitInfo.InRowCell)
                    myGridControl2.DoDragDrop("我要过去了....", DragDropEffects.All);
            }
        }

        private void myGridControl2_MouseDown(object sender, MouseEventArgs e)
        {
            hitInfo = myGridView2.CalcHitInfo(new Point(e.X, e.Y));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (myGridView2.RowCount == 0)
            {
                XtraMessageBox.Show("没有发现任何配载的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //限制长途配载必须填写目的网点zb20190529
            if (this.LoadingType.Text.Trim().ToString() == "长途配载")
            {
                if (string.IsNullOrEmpty(this.EndWeb.Text.Trim().ToString()))
                {
                    MsgBox.ShowOK("目的网点不能为空");
                    return;
                }

            }
            string vehicleno = CarNO.Text.Trim();
            if (vehicleno == "")
            {
                MsgBox.ShowError("车牌号必须填写!");
                CarNO.Focus();
                return;
            }
            //if (CarrNO.Text.Trim() == "")
            //{
            //    MsgBox.ShowError("车厢号必须填写!");
            //    CarNO.Focus();
            //    return;
            //}
            if (CarType.Text.Trim() == "")
            {
                MsgBox.ShowError("车型必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarLength.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车长必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarWidth.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车宽必须填写!");
                CarNO.Focus();
                return;
            }
            if (ConvertType.ToDecimal(CarHeight.Text.Trim()) == 0)
            {
                MsgBox.ShowError("车高必须填写!");
                CarNO.Focus();
                return;
            }
            if (CarSoure.Text.Trim() == "")
            {
                MsgBox.ShowError("车源必须填写!");
                CarNO.Focus();
                return;
            }
            //string endSite = EndSite.Text.Trim();
            //if (endSite == "")
            //{
            //    MsgBox.ShowError("请填写目的地!");
            //    EndSite.Focus();
            //    return;
            //}
            //hj20180416
            string endSite = txtEndSite.Text.Trim();
            string loadPeople = LoadPeoples.Text.Trim();
            if (loadPeople == "")
            {
                MsgBox.ShowError("配载员必填!");
                LoadPeoples.Focus();
                return;
            }
            if (ExpArriveDate.Text.Trim() == "")
            {
                MsgBox.ShowError("请填写预到日期!");
                ExpArriveDate.Focus();
                return;
            }
            string loadingType = LoadingType.Text.Trim();
            if (loadingType == "")
            {
                MsgBox.ShowError("请选择配载类型!");
                LoadingType.Focus();
                return;
            }
            string boxNo = BoxNO.Text.Trim();
            if (loadingType == "长途配载" && boxNo == "")
            {
                if (CarType.Text.Trim() != "平板")
                {
                    MsgBox.ShowError("请填写封箱编号!");
                    BoxNO.Focus();
                    return;
                }
            }
            string cartype = CarType.Text.Trim();
            if (cartype == "")
            {
                MsgBox.ShowError("请选择车辆类型!");
                CarType.Focus();
                return;
            }
            if (ExpArriveDate.DateTime < DepartureDate.DateTime)
            {
                MsgBox.ShowError("预计到达时间选择错误,请检查!\r\n不能早于发车时间!");
                return;
            }
            //if (endSite.Split(',').Length > 2)
            //{
            //    MsgBox.ShowOK("目的地不能超过2个!");
            //    EndSite.Focus();
            //    return;
            //}
            //hj20180416
            //if (endSite.Split(',').Length > 3)
            //{
            //    MsgBox.ShowOK("目的地不能超过3个!");
            //    txtEndSite.Focus();
            //    return;
            //}
            if (endSite.Split(',').Length > 4)
            {
                MsgBox.ShowOK("目的地不能超过4个!");
                txtEndSite.Focus();
                return;
            }
            string DriverIDCardno = DriverIDCardNo.Text.Trim();
            if (DriverIDCardno == "")
            {
                MsgBox.ShowError("驾驶员身份证必填!");
                DriverIDCardNo.Focus();
                return;
            }  //plh 20191225

            if (DriverIDCardno.Length != 18)
            {
                MsgBox.ShowError("驾驶员身份证必须为18位!");
                DriverIDCardNo.Focus();
                return;
            }  //plh 20191225

            #region 检查运输方式 注释
            /*
            string transitMode = "";
            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                transitMode = ConvertType.ToString(myGridView2.GetRowCellValue(i, "TransitMode"));
                if (loadingType == "整车配载" && transitMode != "中强整车")
                {
                    MsgBox.ShowError("您选择的配载类型是整车配载,必须选择运输方式为中强整车的运单!");
                    return;
                }
                if (loadingType == "长途配载" && (transitMode == "中强城际" || transitMode == "中强整车"))
                {
                    MsgBox.ShowError("您选择的配载类型是长途配载,选择运单的运输方式不能为 中强城际或中强整车!");
                    return;
                }
            }
             */
            #endregion

            ContractNO.Text = DepartureBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
            if (DepartureBatch.Text == "")
            {
                MsgBox.ShowError("获取发车批次失败,请重试!");
                return;
            }

            //string strType = string.Empty;//配载类型20181116ld
            //SimpleButton sb = sender as SimpleButton;
            //if (sb != null && sb.Name == "sb_ZQTMSPeiZai")
            //{
            //    strType = "ZQTMS";
            //    string strMessage = CheckBillPeiZaiNum(dataset2.Tables[0]);
            //    if (!string.IsNullOrEmpty(strMessage))
            //    {
            //        MsgBox.ShowOK("以下运单号为分批配载，不能代配载至ZQTMS：\n" + strMessage);
            //        return;
            //    }
            //}
            //else
            //{
            //    strType = "LMS";
            //}

            for (int i = 0; i < myGridView2.RowCount; i++)
            {
                if (CommonClass.QSP_LOCK_1(myGridView2.GetRowCellValue(i, "BillNo").ToString(), myGridView2.GetRowCellValue(i, "BillDate").ToString()))
                {
                    return;
                }
            }

            if (MsgBox.ShowYesNo("确定完成配载？") != DialogResult.Yes) return;
            try
            {
                string billNoStr = "";
                string billState = "";
                string numStr = "", ActualWeight = "", ActualVolume = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    billNoStr += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                    numStr += myGridView2.GetRowCellValue(i, "factqty") + ",";
                    ActualWeight += myGridView2.GetRowCellValue(i, "ActualWeight") + ",";
                    ActualVolume += myGridView2.GetRowCellValue(i, "ActualVolume") + ",";
                    billState += "5,";
                }
                //if (strType == "ZQTMS")
                //{
                //    string strMessage = CheckBillPeiZaiModifyApply(billNoStr);
                //    if (!string.IsNullOrEmpty(strMessage))
                //    {
                //        MsgBox.ShowOK("以下运单号存在改单申请，还未执行，不能代配载至ZQTMS系统：\n" + strMessage);
                //        return;
                //    }
                //}
                #region 判断账户余额当少于2000时给提示 zaj
                //List<SqlPara> listTip = new List<SqlPara>();
                //listTip.Add(new SqlPara("BillNOStr",billNoStr));
                //SqlParasEntity spsTip = new SqlParasEntity(OperType.Query, "QSP_GET_ACCOUNTBALANCE", listTip);
                //DataSet ds = SqlHelper.GetDataSet(spsTip);
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    decimal accountBalance = Convert.ToDecimal(ds.Tables[0].Rows[0]["AccountBalance"].ToString() == "" ? "0" : ds.Tables[0].Rows[0]["AccountBalance"].ToString());
                //    string accountName = ds.Tables[0].Rows[0]["AccountName"].ToString();
                //    if (accountBalance <= 2000 && accountBalance>=950)
                //    {
                //        MsgBox.ShowOK("你的账户【" + accountName + "】,余额已经低于"+accountBalance+"元，请及时充值，以免影响配载！");                       
                //    }
                //    //if (accountBalance < 950)
                //    //{
                //    //    MsgBox.ShowOK("你的账户【" + accountName + "】,余额为：" + accountBalance + "元，不足扣费，请先充值！");
                //    //    return;
                //    //}
                //}
                #endregion

                // this.DepartureBatch.Text = "WH201805-0002";
                string departureBatch = DepartureBatch.Text.Trim();
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("ContractNO", ContractNO.Text.Trim()));
                list.Add(new SqlPara("DepartureBatch", DepartureBatch.Text.Trim()));
                list.Add(new SqlPara("CarNO", CarNO.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("BeginSite", BeginSite.Text.Trim()));
                list.Add(new SqlPara("DepartureDate", DepartureDate.DateTime));
                list.Add(new SqlPara("ExpArriveDate", ExpArriveDate.DateTime));
                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolume", ConvertType.ToDecimal(LoadVolume.Text)));
                list.Add(new SqlPara("ActWeight", ConvertType.ToDecimal(ActWeight.Text)));
                list.Add(new SqlPara("ActVolume", ConvertType.ToDecimal(ActVolume.Text)));
                list.Add(new SqlPara("LoadPeoples", loadPeople));
                list.Add(new SqlPara("Creator", Creator.Text.Trim()));
                list.Add(new SqlPara("BoxNO", boxNo));
                list.Add(new SqlPara("BigCarDescr", BigCarDescr.Text.Trim()));
                list.Add(new SqlPara("EndSite", endSite));
                list.Add(new SqlPara("EndWeb", EndWeb.Text.Trim()));//hj 20180416 新增目的网点字段
                // string billNoStr = "";
                //string numStr = "";

                //for (int i = 0; i < myGridView2.RowCount; i++)
                //{
                //    billNoStr += myGridView2.GetRowCellValue(i, "BillNo") + ",";
                //    numStr += myGridView2.GetRowCellValue(i, "factqty") + ",";
                //}

                list.Add(new SqlPara("BillNOStr", billNoStr));
                list.Add(new SqlPara("NumStr", numStr));
                list.Add(new SqlPara("ActualWeight", ActualWeight));//hj2018075
                list.Add(new SqlPara("ActualVolume", ActualVolume));//hj2018075

                list.Add(new SqlPara("LoadSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("LoadWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("LoadBusiDept", CommonClass.UserInfo.CauseName));
                list.Add(new SqlPara("LoadArea", CommonClass.UserInfo.AreaName));
                list.Add(new SqlPara("LoadDept", CommonClass.UserInfo.DepartName));
                list.Add(new SqlPara("LoadingType", loadingType));
                list.Add(new SqlPara("CarType", CarType.Text.Trim()));
                list.Add(new SqlPara("CarLength", CarLength.Text.Trim()));
                list.Add(new SqlPara("CarWidth", CarWidth.Text.Trim()));
                list.Add(new SqlPara("CarHeight", CarHeight.Text.Trim()));
                list.Add(new SqlPara("NetWeight", ConvertType.ToDecimal(NetWeight.Text)));
                list.Add(new SqlPara("CarSoure", CarSoure.Text.Trim()));


                list.Add(new SqlPara("DriverIDCardNo", DriverIDCardNo.Text.Trim()));  //plh20191225
                //list.Add(new SqlPara("PeiZaiType", strType));//配载类型

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLDEPARTURE", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    XtraMessageBox.Show("配载完成。\r\n\r\n现在将关闭本窗口。\r\n\r\n返回后，可以打印清单或者调整本车配载及相关运输合同信息。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    #region 注释代码
                    // List<SqlPara> listQuery = new List<SqlPara>();
                    // listQuery.Add(new SqlPara("BillNOStr", billNoStr));
                    // SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureSysInfo", listQuery);
                    // DataSet ds = SqlHelper.GetDataSet(spsQuery);
                    //// if (ds == null || ds.Tables[0].Rows.Count == 0) return;
                    // if (ds != null)
                    // {
                    //     if (ds.Tables[0].Rows.Count > 0)
                    //     {
                    //         string dsJson = JsonConvert.SerializeObject(ds);
                    //         RequestModel<string> request = new RequestModel<string>();
                    //         request.Request = dsJson;
                    //         request.OperType = 0;
                    //         string json = JsonConvert.SerializeObject(request);
                    //         string url = "http://localhost:42936/KDLMSService/ZQTMSDepartureSys";
                    //         // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSDepartureSys";


                    //         ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
                    //         if (model.State != "200")
                    //         {
                    //             MsgBox.ShowOK(model.Message);
                    //         }
                    //     }
                    // }
                    #endregion
                    //BillDepartureSyn(billNoStr,departureBatch);
                    string strBillNo = billNoStr.Replace(",", "@");



                    CommonSyn.BillDepartureSynNew(billNoStr, departureBatch, numStr);//yzw

                    SendHaoDuoCheBill(CommonClass.UserInfo.companyid, CommonClass.UserInfo.gsqc, CommonClass.UserInfo.token); //好多车接口下单，打印推送订单/ld
                }

                showDeteil();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //检查是否有分批配载的运单
        private string CheckBillPeiZaiNum(DataTable dt)
        {
            StringBuilder sb = new StringBuilder("");
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    //运单件数不等于实发件数
                    if (Convert.ToInt32(row["Num"]) != Convert.ToInt32(row["factqty"]))
                    {
                        sb.Append(row["BillNo"].ToString() + "\n");
                    }
                }
            }
            return sb.ToString();
        }

        //检查分批配载的运单是否有做改单申请
        private string CheckBillPeiZaiModifyApply(string strBillNos)
        {
            StringBuilder sb = new StringBuilder("");
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNos", strBillNos));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_Check_BillApply_BY_BillNos", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append(row["BillNo"].ToString() + "\n");
                }
            }
            return sb.ToString();
        }

        //好多车接口下单，打印推送订单
        private void SendHaoDuoCheBill(string CompanyId, string CompanyName, string strToken)
        {
            //if (Convert.ToInt32(ds.Tables[0].Rows[0]["PrintNum"]) == 1)
            //{
            List<SqlPara> list_hd = new List<SqlPara>();
            list_hd.Add(new SqlPara("DepartureBatch", ContractNO.Text.Trim()));
            SqlParasEntity spe_hd = new SqlParasEntity(OperType.Query, "QSP_Get_Requirement_Data", list_hd);
            DataSet ds_hd = SqlHelper.GetDataSet(spe_hd);
            if (ds_hd != null && ds_hd.Tables.Count > 0 && ds_hd.Tables[0].Rows.Count > 0)
            {
                string strReturn = CommonSyn.SendHaoDuoCheOrder(ds_hd, CompanyId, CompanyName, strToken);
                if (strReturn != "成功")
                {
                    string[] strs = strReturn.Split(new char[] { '@' });
                    if (strs.Length > 1)
                    {
                        List<SqlPara> listLog = new List<SqlPara>();
                        listLog.Add(new SqlPara("Batch", ContractNO.Text.Trim()));
                        listLog.Add(new SqlPara("FutionName", "SendHaoDuoCheOrder"));
                        listLog.Add(new SqlPara("FaceUrl", ""));
                        listLog.Add(new SqlPara("FaceJson", strs[1]));
                        listLog.Add(new SqlPara("ResultMessage", strs[0]));
                        listLog.Add(new SqlPara("FaceState", "失败"));
                        SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                        SqlHelper.ExecteNonQuery(spsLog);
                    }
                }
            }
            else
            {
                List<SqlPara> listLog = new List<SqlPara>();
                listLog.Add(new SqlPara("Batch", ContractNO.Text.Trim()));
                listLog.Add(new SqlPara("FutionName", "SendHaoDuoCheOrder"));
                listLog.Add(new SqlPara("FaceUrl", ""));
                listLog.Add(new SqlPara("FaceJson", ""));
                listLog.Add(new SqlPara("ResultMessage", "QSP_Get_Requirement_Data：查询数据不满足推送条件！"));
                listLog.Add(new SqlPara("FaceState", "失败"));
                SqlParasEntity spsLog = new SqlParasEntity(OperType.Execute, "USP_ADD_InterFace_Log", listLog);
                SqlHelper.ExecteNonQuery(spsLog);
            }
            //}
        }

        //private void BillDepartureSyn(string billNoStr,string departureBatch)
        //{
        //    try
        //    {
        //        List<SqlPara> listQuery = new List<SqlPara>();
        //        listQuery.Add(new SqlPara("BillNOStr", billNoStr));
        //        listQuery.Add(new SqlPara("DepartureBatch", departureBatch));
        //        SqlParasEntity spsQuery = new SqlParasEntity(OperType.Query, "QSP_GET_DepartureSysInfo", listQuery);
        //        DataSet ds = SqlHelper.GetDataSet(spsQuery);
        //        // if (ds == null || ds.Tables[0].Rows.Count == 0) return;
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                string dsJson = JsonConvert.SerializeObject(ds);
        //                RequestModel<string> request = new RequestModel<string>();
        //                request.Request = dsJson;
        //                request.OperType = 0;
        //                string json = JsonConvert.SerializeObject(request);
        //                //string url = "http://localhost:42936/KDLMSService/ZQTMSDepartureSys";
        //                // string url = "http://lms.dekuncn.com:7016/KDLMSService/ZQTMSDepartureSys";
        //                string url = HttpHelper.urlBilldepartureSyn;
        //                ResponseModelClone<string> model = HttpHelper.HttpPost(json, url);
        //                if (model.State != "200")
        //                {
        //                    MsgBox.ShowOK(model.Message);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}

        private void showDeteil()
        {
            frmDepartureMody frm = new frmDepartureMody();
            frm.sDepartureBatch = DepartureBatch.Text;
            frm.strPeiZaiType = this.ck_ZQTMS.Checked ? "ZQTMS" : "LMS";
            frm.strCompanyId = CommonClass.UserInfo.companyid;
            frm.strCompanyName = CommonClass.UserInfo.gsqc;
            frm.strToken = CommonClass.UserInfo.token;
            frm.xtraTabControl1.SelectedTabPage = frm.tp3;
            frm.ShowDialog();
        }

        private void btnCanccel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        ////////////////////////////////////////
        private void barEditItem3_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView1.ClearColumnsFilter();
                GridOper.GetGridViewColumn(myGridView1, "BillNo").FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView1.ClearColumnsFilter();
        }

        private void barEditItem3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView1.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView1.SelectRow(0);
            GridViewMove.Move(myGridView1, dataset1, dataset2);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView1.ClearColumnsFilter();
            e.Handled = true;
        }

        private void barEditItem2_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            string szfilter = e.NewValue.ToString().Trim();
            if (szfilter != "")
            {
                myGridView2.ClearColumnsFilter();
                GridOper.GetGridViewColumn(myGridView2, "BillNo").FilterInfo = new ColumnFilterInfo("[BillNo] LIKE" + "'%" + szfilter + "%'", "");
            }
            else
                myGridView2.ClearColumnsFilter();
        }

        private void barEditItem2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || ((DevExpress.XtraEditors.TextEdit)sender).Text == "" || myGridView2.RowCount > 1)
            {
                e.Handled = true;
                return;
            }
            myGridView2.SelectRow(0);
            GridViewMove.Move(myGridView2, dataset2, dataset1);
            ((DevExpress.XtraEditors.TextEdit)sender).Text = "";

            myGridView2.ClearColumnsFilter();
            e.Handled = true;
        }

        private void edCarNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                myGridControl3.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
        }

        private void edCarNO_Leave(object sender, EventArgs e)
        {
            if (!myGridControl3.Focused)
            {
                myGridControl3.Visible = false;
            }
        }

        private void edCarNO_Enter(object sender, EventArgs e)
        {
            if (CarNO.Text.Trim() == "")
            {
                myGridView3.ClearColumnsFilter();
            }
            if (myGridView3.RowCount == 0) return;
            myGridControl3.BringToFront();
            myGridControl3.Left = CarNO.Left;
            myGridControl3.Top = CarNO.Top + CarNO.Height;
            myGridControl3.Visible = true;
        }

        private void edCarNO_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue == null) return;
            myGridView3.Columns["CarNo"].FilterInfo = new ColumnFilterInfo("[CarNo] LIKE " + "'%" + e.NewValue.ToString() + "%'" + " OR [CarNo] LIKE" + "'%" + e.NewValue.ToString() + "%'", "");
        }

        private void myGridControl3_DoubleClick(object sender, EventArgs e)
        {
            SetCarInfo();
        }

        private void SetCarInfo()
        {
            int rowhandle = myGridView3.FocusedRowHandle;
            if (rowhandle < 0) return;
            DataRow dr = myGridView3.GetDataRow(rowhandle);
            try
            {
                CarNO.EditValue = dr["CarNo"];
                DriverName.EditValue = dr["DriverName"];
                DriverPhone.EditValue = dr["DriverPhone"];
                CarrNO.EditValue = dr["CarrNO"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolume.EditValue = dr["LoadVolum"];
                CarType.EditValue = dr["CarType"];
                CarLength.EditValue = dr["CarLength"];
                CarWidth.EditValue = dr["CarWidth"];
                CarHeight.EditValue = dr["CarHeight"];
                DriverIDCardNo.EditValue = dr["DriverIDCardNo"];   //plh20191225

            }
            catch { }
            myGridControl3.Visible = false;
        }

        public string GetMaxInOneVehicleFlag(string bsite)
        {
            DataSet dsflag = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", bsite));
                list.Add(new SqlPara("bcode", CommonClass.UserInfo.LoginSiteCode));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_INONEVEHICLEFLAG", list);
                dsflag = SqlHelper.GetDataSet(sps);

                return ConvertType.ToString(dsflag.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("产生发车批次失败：\r\n" + ex.Message);
                return "";
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                try
                {
                    string billno = myGridView2.GetRowCellValue(rowhandle, "BillNo").ToString();
                    DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_ActualWeight", new List<SqlPara> { new SqlPara("BillNo", billno) }));
                    decimal ActualWeight = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["ActualWeight"]);
                    decimal ActualVolume = ConvertType.ToDecimal(ds.Tables[0].Rows[0]["ActualVolume"]);
                    if (ds == null || ds.Tables.Count == 0) return;

                    int Num = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "Num"));
                    int factqty = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "factqty"));
                    int LeftNum = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "LeftNum"));

                    if (myGridView2.FocusedColumn != null)
                    {

                        if (e == null || myGridView2.GetFocusedRowCellValue("factqty") == null) return;

                        int leftNum = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "LeftNum"));
                        int factQty = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "factqty"));

                        if (factQty <= 0)
                        {
                            MsgBox.ShowError("实发件数不能小于0!");
                            myGridView2.SetRowCellValue(rowhandle, "factqty", leftNum);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }
                        else if (factQty > leftNum)
                        {
                            MsgBox.ShowError("实发件数不能大于剩余件数!");
                            myGridView2.SetRowCellValue(rowhandle, "factqty", leftNum);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }

                        if (e == null || myGridView2.GetFocusedRowCellValue("ActualWeight") == null) return;
                        //decimal feeWeight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "FeeWeight"));//计费重量
                        decimal Value = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "ActualWeight"));
                        decimal feeWeight2 = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "FeeWeight2"));
                        decimal acc1 = feeWeight2 - ActualWeight;
                        decimal acc2 = acc1 - Value;
                        decimal acc3 = Math.Round(feeWeight2 * factqty / Num, 2);
                        if (Value <= 0 || Value > acc1)
                        {
                            MsgBox.ShowError("实发计费重量不能为0或者大于剩余计费重量!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualWeight", acc3);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;

                        }
                        if (LeftNum - factqty != 0 && acc2 <= 0)
                        {
                            MsgBox.ShowError("还有剩余件数,实发计费重量不能大于等于剩余计费重量!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualWeight", acc3);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }
                        if (LeftNum - factqty == 0 && acc2 != 0)
                        {
                            MsgBox.ShowError("剩余件数为0,实发计费重量必须等于剩余计费重量!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualWeight", acc1);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }


                        //decimal feeVolume = ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "FeeVolume"));//计费体积
                        decimal feeVolume2 = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "FeeVolume2"));
                        decimal Value1 = ConvertType.ToDecimal(myGridView2.GetRowCellValue(rowhandle, "ActualVolume"));
                        decimal acc4 = feeVolume2 - ActualVolume;
                        decimal acc5 = acc4 - Value1;
                        decimal acc6 = Math.Round(feeVolume2 * factqty / Num, 2);
                        if (Value1 <= 0 || Value1 > acc4)
                        {
                            MsgBox.ShowError("实发计费体积不能为0或者大于剩余计费体积!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualVolume", acc6);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }
                        if (LeftNum - factqty != 0 && acc5 <= 0)
                        {
                            MsgBox.ShowError("还有剩余件数,实发计费体积不能大于等于剩余计费体积!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualVolume", acc6);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }
                        if (LeftNum - factqty == 0 && acc5 != 0)
                        {
                            MsgBox.ShowError("剩余件数为0,实发计费体积必须等于剩余计费体积!");
                            myGridView2.SetRowCellValue(rowhandle, "ActualVolume", acc4);
                            xtraTabControl1.SelectedTabPage = xtraTabPage1;
                            return;
                        }

                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
                string endSite = txtEndSite.Text.Trim();
                //zb20190527  485 目的站和目的网点可编辑
                if (CommonClass.UserInfo.companyid == "485")
                {
                    //判断有无数据
                    if (myGridView2.RowCount == 0)
                    {
                        XtraMessageBox.Show("没有发现任何配载的清单，请先在第①步中构建清单。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //判断清单中有2条数据时,始发站是否一致
                    if (myGridView2.RowCount > 0 &&  this.LoadingType.Text.Trim().ToString() == "整车配载")
                    {
                        for (int i = 0; i < myGridView2.RowCount; i++)
                        {
                            if (i > 0)
                            {
                                if (GridOper.GetRowCellValueString(myGridView2, i, "StartSite") != GridOper.GetRowCellValueString(myGridView2, i - 1, "StartSite"))
                                {
                                    MsgBox.ShowOK("不同始发站不能一起配载!");
                                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                                    return;
                                }
                            }
                              this.BeginSite.Text = GridOper.GetRowCellValueString(myGridView2, 0, "StartSite");
                        }
                      
                    }
                }
                if (DepartureBatch.Text.Trim() == "")
                {
                    DepartureBatch.Text = GetMaxInOneVehicleFlag(CommonClass.UserInfo.SiteName);
                    if (ContractNO.Text.Trim() == "")
                        ContractNO.Text = DepartureBatch.Text;
                }

                GetWeightAndVolume();

                string s = "", tmp = "";
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    tmp = GridOper.GetRowCellValueString(myGridView2, i, "TransferSite");
                    if (tmp != "" && !s.Contains(tmp))
                        s += tmp + ",";
                }
                //EndSite.Text = s.TrimEnd(',');
                txtEndSite.Text = s.TrimEnd(',');
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView2);
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView2.Guid.ToString());
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView2);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "配载库存清单");
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView2, "配载库存挑选清单");
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void myGridView2_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || e.Value == null) return;
            int rowhandle = myGridView2.FocusedRowHandle;
            if (rowhandle < 0) return;

            //try
            //{
            //    int leftNum = ConvertType.ToInt32(myGridView2.GetRowCellValue(rowhandle, "LeftNum"));
            //    int factQty = ConvertType.ToInt32(e.Value);

            //    if (factQty <= 0)
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "实发件数必须大于0!";
            //    }
            //    else if (factQty > leftNum)
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "实发件数不能大于剩余件数!";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError(ex.Message);
            //}
        }

        private void myGridView2_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "整车配载";
            getdata(2);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "项目配载";
            getdata(3);
        }

        private void ExpArriveDate_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //if (e == null || ConvertType.ToString(e.NewValue) == "") return;
            //DateTime dt = ConvertType.ToDateTime(e.NewValue);
            //if (dt.Hour != 0) e.Cancel = true;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (dataset1 == null || dataset1.Tables.Count == 0) return;
            if (treeView1.SelectedNode == null) return;
            string site = treeView1.SelectedNode.Text.Trim();
            if (site == "全部")
                myGridView1.ClearColumnsFilter();
            else
                GridOper.GetGridViewColumn(myGridView1, "TransferSite").FilterInfo = new ColumnFilterInfo("TransferSite='" + site + "'");
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            LoadingType.Text = "长途配载";
            getdata(4);
        }

        private void myGridControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl3.Visible = false;
            }
            if (e.KeyCode == Keys.Enter)
            {
                SetCarInfo();
            }
        }

        private void myGridView2_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e == null || e.RowHandle < 0 || e.Column.FieldName != "factqty") return;
            int factqty = ConvertType.ToInt32(e.Value);

            int Num = ConvertType.ToInt32(myGridView2.GetRowCellValue(e.RowHandle, "Num"));
            if (factqty <= 0 || factqty > Num) return;
            decimal feeWeight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "FeeWeight2"));//计费重量
            decimal feeVolume = ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "FeeVolume2"));//计费体积
            decimal actualFreight = ConvertType.ToDecimal(myGridView2.GetRowCellValue(e.RowHandle, "ActualFreight"));//实收运费Zb2190617

            myGridView2.SetRowCellValue(e.RowHandle, "FeeWeight", Math.Round(feeWeight * factqty / Num, 2));
            myGridView2.SetRowCellValue(e.RowHandle, "FeeVolume", Math.Round(feeVolume * factqty / Num, 2));
            myGridView2.SetRowCellValue(e.RowHandle, "ActualWeight", Math.Round(feeWeight * factqty / Num, 2));//实际计费重量
            myGridView2.SetRowCellValue(e.RowHandle, "ActualVolume", Math.Round(feeVolume * factqty / Num, 2));//实际计费体积
            myGridView2.SetRowCellValue(e.RowHandle, "PZ", Math.Round(actualFreight * factqty / Num, 2));//配载金额Zb2190617
        }

        //hj20180416
        public string ResponseSite { get; set; }
        public string ResponseWeb { get; set; }
        private void buttonEdit1_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            string SystemType = "LMS";
            if (this.ck_ZQTMS.Checked)
            {
                SystemType = "ZQTMS";
            }
            frmDepartureEdit frmEdit = new frmDepartureEdit(SystemType);
            frmEdit.RequestSite = txtEndSite.Text.Trim();
            frmEdit.RequestWeb = EndWeb.Text.Trim();
            frmEdit.Owner = this;
            frmEdit.loadType = LoadingType.Text.Trim();
            //frmEdit.strTransitMode = strTransitMode;
            frmEdit.LoadingType = this.LoadingType.Text.Trim(); //zb20190710
            frmEdit.ShowDialog();
            if (frmEdit.IsModify)
            {
                this.txtEndSite.Text = ResponseSite;
                this.EndWeb.Text = ResponseWeb;
            }
        }

        //hj20180706
        private void myGridView2_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {

        }

        //代配载
        private void ck_ZQTMS_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ck_ZQTMS.Checked)
            {
                //LoadZQTMSCar();

                //this.CarNO.Text = string.Empty;
                //this.txtEndSite.Text = string.Empty;
                //this.EndWeb.Text = string.Empty;
                this.sb_ZQTMSPeiZai.Visible = true;
                this.btnAdd.Visible = false;
                if (MsgBox.ShowYesNo("勾选代配载后货物将转配载至ZQTMS系统，请注意切勿操作错误！！是否继续？") == DialogResult.No)
                {
                    this.ck_ZQTMS.Checked = false;
                }
            }
            else
            {
                //myGridControl3.DataSource = CommonClass.dsCar.Tables[0];

                //this.CarNO.Text = string.Empty;
                //this.txtEndSite.Text = string.Empty;
                //this.EndWeb.Text = string.Empty;
                this.sb_ZQTMSPeiZai.Visible = false;
                this.btnAdd.Visible = true;
            }
            //xtraTabControl1_SelectedPageChanged(null, null);
            this.txtEndSite.Text = string.Empty;
            this.EndWeb.Text = string.Empty;
        }

        //加载ZQTMS车辆信息
        private void LoadZQTMSCar()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CAR", list);
                DataSet ds = SqlHelper.GetDataSet_ZQTMS(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                myGridControl3.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowError("加载LMS车辆信息：" + ex.Message);
            }
        }

        private void myGridView2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            
        }
    }
}
