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
    public partial class frmInterchangeOrder : BaseForm
    {
        public frmInterchangeOrder()
        {
            InitializeComponent();
        }

        private void frmInterchangeOrder_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1, myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例

            CommonClass.SetSite(true, txtSite);
            txtSite.Text = CommonClass.UserInfo.SiteName;
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            myGridControl1.DataSource = null;
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            try
            {
                string site = txtSite.Text.Trim();
                string interType = txtInterType.Text.Trim();
                string webName = txtCarNo.Text.Trim();
                site = site == "全部" ? "%%" : site;
                interType = interType == "全部" ? "%%" : interType;
                webName = webName == "全部" ? "%%" : webName;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("interType", interType));
                list.Add(new SqlPara("interStatus", txtInterStatus.Text.Trim() == "全部" ? "%%" : txtInterStatus.Text.Trim()));
                list.Add(new SqlPara("SiteName", site));
                list.Add(new SqlPara("WebName", webName));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_InterchangeOrder", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (myGridView1.FocusedRowHandle < 0) return;
                string InterNo = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "InterNo").ToString();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_InterchangeOrderDetail", new List<SqlPara>() { new SqlPara("InterNo", InterNo) });
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl2.DataSource = ds.Tables[0];
                xtraTabControl1.SelectedTabPageIndex = 1;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1);
        }

        /// <summary>
        /// 检测钩选的项是否合法
        /// </summary>
        private bool CheckSelect(ref DataTable SelectDt)
        {
            myGridView1.PostEditor();
            string esite = "", vno = "";
            List<string> listEsite = new List<string>();

            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return false;
            SelectDt = dt.Clone();

            foreach (DataRow dr in dt.Rows)
            {
                if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                SelectDt.ImportRow(dr);//将选择的行存到新表

                esite = ConvertType.ToString(dr["Esite"]);

                if (ConvertType.ToString(dr["InterType"]) != "装车扫描")
                {
                    MsgBox.ShowOK("挑选的交接单必须是装车扫描的，请检查!");
                    return false;
                }
                if (ConvertType.ToString(dr["InterStatus"]) != "未配载")
                {
                    MsgBox.ShowOK("挑选的交接单必须是未配载的，请检查!");
                    return false;
                }

                //检测车号是否一致
                if (vno == "") vno = ConvertType.ToString(dr["CarNo"]);//表示第1次赋值
                else
                {
                    if (vno != ConvertType.ToString(dr["CarNo"]))
                    {
                        MsgBox.ShowOK("选择的交接单车号必须一致，请重新选择!");
                        return false;
                    }
                }
                if (!listEsite.Contains(esite)) listEsite.Add(esite);
                if (listEsite.Count > 2)
                {
                    MsgBox.ShowOK("选择的目的地不能超过2个，请重新选择!");
                    return false;
                }
            }
            if (SelectDt.Rows.Count == 0)
            {
                MsgBox.ShowOK("请选择要转配载的交接单!");
                return false;
            }
            return true;
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string InterNoStr = "", Chauffer = "", Chauffermb = "", CarNo = "", EsiteStr = "";
            DataTable dt = null;
            if (!CheckSelect(ref dt)) return;
            if (dt == null || dt.Rows.Count == 0) return;
            Chauffer = ConvertType.ToString(dt.Rows[0]["CarDervice"]);
            Chauffermb = ConvertType.ToString(dt.Rows[0]["CarPhone"]);
            CarNo = ConvertType.ToString(dt.Rows[0]["CarNo"]);

            foreach (DataRow dr in dt.Rows)
            {
                InterNoStr += ConvertType.ToString(dr["InterNo"]) + "@";
                EsiteStr += ConvertType.ToString(dr["Esite"]) + ",";
            }

            EsiteStr = EsiteStr.TrimEnd(',');
            if (InterNoStr == "" || EsiteStr == "") return;
            if (CarNo == "")
            {
                MsgBox.ShowOK("您选择的交接单没有车号，请检查!");
                return;
            }

            //检查选择的运单是否符合配载
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_CHECK_INTERORDER_SURE_PZ", new List<SqlPara> { new SqlPara("InterNoStr", InterNoStr) }));
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("没有获取到可以配载的运单!");
                return;
            }

            //得到可以配载的运单
            string BillNoStr = "", NumStr = "";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                BillNoStr += dr["BillNo"] + "@";
                NumStr += dr["Num"] + "@";
            }

            if (BillNoStr == "" || NumStr == "")
            {
                MsgBox.ShowOK("没有可配载的运单，请检查!");
                return;
            }
            string inoneflag = GetMaxInOneVehicleFlag();
            if (inoneflag == "")
            {
                MsgBox.ShowOK("没有获取到发车批次，请重新保存!");
                return;
            }
            if (MsgBox.ShowYesNo("确定转配载？") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("InterNo", InterNoStr));
            list.Add(new SqlPara("CarNO", CarNo));
            list.Add(new SqlPara("DepartureBatch", inoneflag));
            list.Add(new SqlPara("DriverName", Chauffer));
            list.Add(new SqlPara("DriverPhone", Chauffermb));
            list.Add(new SqlPara("EndSite", EsiteStr));
            list.Add(new SqlPara("BillNoStr", BillNoStr));
            list.Add(new SqlPara("NumStr", NumStr));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_INTERORDER_GEN_FCD_HT", list)) == 0) return;
            MsgBox.ShowOK("转配载成功!");
        }

        public string GetMaxInOneVehicleFlag()
        {
            DataSet dsflag = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
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

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SendLoad(true);//转二级
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SendLoad(false);//转送货
        }

        /// <summary>
        /// 转二级/转送货
        /// </summary>
        /// <param name="IsSendToSite"></param>
        private void SendLoad(bool IsSendToSite)
        {
            DataTable dt = null;
            if (!CheckSelect(ref dt)) return;
            if (dt == null) return;
            string InterNoStr = "", Chauffer = "", Chauffermb = "", CarNo = "";
            Chauffer = ConvertType.ToString(dt.Rows[0]["CarDervice"]);
            Chauffermb = ConvertType.ToString(dt.Rows[0]["CarPhone"]);
            CarNo = ConvertType.ToString(dt.Rows[0]["CarNo"]);

            foreach (DataRow dr in dt.Rows)
            {
                InterNoStr += dr["InterNo"] + "@";
            }

            if (InterNoStr == "") return;
            if (CarNo == "")
            {
                MsgBox.ShowOK("您选择的交接单没有车号，请检查!");
                return;
            }

            //检测选择的运单是否符合送货/转二级
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_CHECK_INTERORDER_SURE_SEND", new List<SqlPara> { new SqlPara("InterNoStr", InterNoStr) }));
            if (ds == null || ds.Tables.Count == 0)
            {
                MsgBox.ShowOK("没有获取到可以操作的交接单");
                return;
            }

            frmInterOrderToSend f = new frmInterOrderToSend();
            f.InterNoStr = InterNoStr;
            f.dtBillNo = ds.Tables[0];
            f.IsSendToSite = IsSendToSite;
            f.SendCarNO.Text = CarNo;
            f.SendDriver.Text = Chauffer;
            f.SendDriverPhone.Text = Chauffermb;
            f.ShowDialog();
        }

        private void txtSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(txtCarNo, txtSite.Text.Trim(), true);
        }

        /// <summary>
        /// 修改交接单状态
        /// </summary>
        /// <param name="type">1剔除、2恢复</param>
        private void SetInterOrderState(int type)
        {
            if (xtraTabControl1.SelectedTabPage != xtraTabPage2) return;
            myGridView2.PostEditor();

            string InterNoStr = "", BillNoStr = "";
            DataTable dt = myGridControl2.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0) return;
            dt.AcceptChanges();
            foreach (DataRow dr in dt.Rows)
            {
                if (ConvertType.ToInt32(dr["ischecked"]) == 0) continue;
                if (type == 1 && ConvertType.ToInt32(dr["IsDelete"]) == 1) continue;
                if (type == 2 && ConvertType.ToInt32(dr["IsDelete"]) == 0) continue;
                InterNoStr += dr["InterNo"] + "@";
                BillNoStr += dr["BillNo"] + "@";
            }

            if (InterNoStr == "")
            {
                MsgBox.ShowOK(type == 1 ? "请选择要剔除的运单!" : "请选择要恢复的运单!");
                return;
            }
            if (MsgBox.ShowYesNo(type == 1 ? "确定剔除选中单？" : "确定恢复选中单？") != DialogResult.Yes) return;
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("InterNo", InterNoStr));
            list.Add(new SqlPara("BillNo", BillNoStr));
            list.Add(new SqlPara("Type", type));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_SET_INTERCHANGEORDER_STATE", list)) == 0) return;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (ConvertType.ToInt32(dt.Rows[i]["ischecked"]) == 0) continue;
                dt.Rows[i]["IsDelete"] = type == 1 ? 1 : 0;
            }

            if (@type == 1) MsgBox.ShowOK("剔除成功!");
            else MsgBox.ShowOK("恢复成功!");
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInterOrderState(1);
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SetInterOrderState(2);
        }
    }
}