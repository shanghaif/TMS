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

namespace ZQTMS.UI
{
    public partial class frmDepartureAddSingle : ZQTMS.Tool.BaseForm
    {
        private string departureBatch;
        private string carNo;
        private string driverName;
        private string driverPhone;
        private string beginSite;
        private string endSite;
        public string strPeiZaiType = string.Empty;//配载类型
        public string WebState = string.Empty;
        /// <summary>
        /// 发车批次
        /// </summary>
        public string DepartureBatch
        {
            get { return departureBatch; }
            set { departureBatch = value; }
        }

        /// <summary>
        /// 车号
        /// </summary>
        public string CarNo
        {
            get { return carNo; }
            set { carNo = value; }
        }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DriverName
        {
            get { return driverName; }
            set { driverName = value; }
        }

        /// <summary>
        /// 司机电话
        /// </summary>
        public string DriverPhone
        {
            get { return driverPhone; }
            set { driverPhone = value; }
        }

        /// <summary>
        /// 启运地
        /// </summary>
        public string BeginSite
        {
            get { return beginSite; }
            set { beginSite = value; }
        }

        /// <summary>
        /// 目的地
        /// </summary>
        public string EndSite
        {
            get { return endSite; }
            set { endSite = value; }
        }

        public frmDepartureAddSingle()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataSet ds = null;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE_ISPRINT", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ConvertType.ToString(ds.Tables[0].Rows[0][2]) != "")
                {
                    MsgBox.ShowOK("本车已经打印司机运输协议，不能单票强装!");
                    return;
                }
            }catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }


            string BillNo = txtBillNo.Text.Trim();
            if (BillNo == "")
            {
                MsgBox.ShowOK("请输入要加入本车的运单号!");
                txtBillNo.Focus();
                return;
            }
            string addReason = MeAddReason.Text.Trim();
            if (addReason == "")
            {
                MsgBox.ShowOK("请填写强装原因!");
                MeAddReason.Focus();
                return;
            }
            if (MsgBox.ShowYesNo("确定将运单：" + BillNo + "加入到本车？") != DialogResult.Yes) return;

            try
            {
                //检查分批配载的运单是否有做改单申请
                if (strPeiZaiType == "ZQTMS")
                {
                    string strMessage = CheckBillPeiZaiModifyApply(BillNo + ",");
                    if (!string.IsNullOrEmpty(strMessage))
                    {
                        MsgBox.ShowOK("以下运单号存在改单申请，还未执行，不能代配载至ZQTMS系统：\n" + strMessage);
                        return;
                    }
                }
                //跨系统加入，需提醒目的网点做到货确认，如配载至ZQTMS系统
                if (strPeiZaiType == "ZQTMS" && WebState == "1")
                {
                    if (MsgBox.ShowYesNo("当前操作是转配载至ZQTMS系统的批次号，加入后必须通知目的网点做到货确认，是否继续？") == DialogResult.No)
                    {
                        return;
                    }
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("DepartureBatch", departureBatch));
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("AaddReason", addReason));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_DEPARTURE_SINGLE", list)) == 0) return;
                //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_DEPARTURE_SINGLE_1", list)) == 0) return;    //whf20190809 USP_ADD_DEPARTURE_SINGLE 改为 USP_ADD_DEPARTURE_SINGLE_1

                txtBillNo.Text = "";
                MsgBox.ShowOK("加入成功!");

                if (strPeiZaiType == "ZQTMS")
                {
                    CommonSyn.LMSDepartureSysZQTMS(list, 3, "USP_ADD_DEPARTURE_SINGLE,USP_ADD_PACKAGE_FB_TO_ZX_LMS,USP_ADD_WAYBILL_NEW_V3_LMS", BillNo + ",", departureBatch, CommonClass.UserInfo.companyid);//LMS配载同步ZQTMS（单票强制加入）                        
                }
                else
                {
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        CommonSyn.TimeDepartUptSyn((BillNo + "@"), departureBatch, ds.Tables[0].Rows[0]["DepartureDate"].ToString(), ds.Tables[0].Rows[0]["LoadWeb"].ToString(), CommonClass.UserInfo.WebName, "USP_ADD_DEPARTURE_SINGLE");//同步配载修改时效 LD 2018-4-27
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //检查分批配载的运单是否有做改单申请
        private string CheckBillPeiZaiModifyApply(string strBillNos)
        {
            StringBuilder sb = new StringBuilder("");
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }
            return sb.ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            frmBillSearch.ShowBillSearch(txtBillNo.Text.Trim());
        }

        private void frmDepartureAddSingle_Load(object sender, EventArgs e)
        {
            lblDepartureBatch.Text = "发车批次：" + departureBatch;
            lblCarNo.Text = "车号：" + carNo;
            lblDriverName.Text = "司机：" + driverName;
            lblDriverPhone.Text = "司机电话：" + driverPhone;
            lblBeginSite.Text = "启运站：" + beginSite;
            lblEndSite.Text = "目的地：" + endSite;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}