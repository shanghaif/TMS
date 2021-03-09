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
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmInsurance : BaseForm
    {
        public frmInsurance()
        {
            InitializeComponent();
        }

        private void frmInsurance_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView3, false);
            GridOper.SetGridViewProperty(myGridView3);
             BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
             GridOper.RestoreGridLayout(myGridView3);
            cbbState.Text = "全部";
            bdate.EditValue = CommonClass.gbdate;
            edate.EditValue = CommonClass.gedate;
            GridOper.RestoreGridLayout(myGridView3, myGridView3.Guid.ToString());
           // FixColumn fix = new FixColumn(myGridView3, barButtonItem6);
           // barBtnModify.Enabled = false;
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.Text.Trim()));
                list.Add(new SqlPara("edate", edate.Text.Trim()));
               // list.Add(new SqlPara("BillNO",billno.Text.Trim()==""?"%%":"%"+billno.Text.Trim()+"%"));
                list.Add(new SqlPara("state", cbbState.Text.Trim()));

               // list.Add(new SqlPara("bsite", cbSite.Text.Trim() == "全部" ? "%%" : "%" + cbSite.Text.Trim() + "%"));
               // list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
               // list.Add(new SqlPara("CauseName", cbCauseName.Text.Trim() == "全部" ? "%%" : "%" + cbCauseName.Text + "%"));
               // list.Add(new SqlPara("AreaName", cbAreaName.Text.Trim() == "全部" ? "%%" : "%" + cbAreaName.Text + "%"));
               // list.Add(new SqlPara("WebName", cbWeb.Text.Trim() == "全部" ? "%%" : "%" + cbWeb.Text.Trim() + "%"));
               // list.Add(new SqlPara("SignMan", txtSignManName.Text.Trim() == "" ? "%%" : "%" + txtSignManName.Text.Trim() + "%"));
               // list.Add(new SqlPara("BillNO", txtID.Text.Trim() == "" ? "%%" : "%" + txtID.Text.Trim() + "%"));
               // list.Add(new SqlPara("ExceptType", cbbExceptType.Text.Trim() == "全部" ? "%%" : "%" + cbbExceptType.Text.Trim() + "%"));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLINSURANCE_FAIL", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                gcInsurance.DataSource=ds.Tables[0];
               // gcErrorSign.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {

                if (myGridView3.RowCount < 1000) myGridView3.BestFitColumns();
            }
        }

        private void barModify_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {           
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0)
                {
                    MsgBox.ShowOK("请选择要修改的异常签收信息！");
                    return;
                }

                string insuranceNO = myGridView3.GetRowCellValue(rowhandle, "InsuranceNO").ToString();
                string responceState = myGridView3.GetRowCellValue(rowhandle, "ResponceState").ToString();
                frmInsuranceEdit edit = new frmInsuranceEdit();
                edit.insuranceNO = insuranceNO;
                edit.responceState = responceState;
                edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void myGridView3_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = myGridView3.FocusedRowHandle;
            if (rowHandle < 0) return;
            string responcePolicyGuid = myGridView3.GetRowCellValue(rowHandle, "ResponcePolicyGuid").ToString();
           // string url="http://service.5156bx.com:81/postPolicy/getPolicy";
            string remoteuser="8CB22A57-50E6-4B4C-9F65-BA45B5D56F9D";
            //string url = "http://localhost:42936/LKService/Get_InsureResult";//http://localhost:42936/  http://192.168.16.112:99
            string url = "http://192.168.16.112:99/LKService/Get_InsureResult";//http://localhost:42936/  http://192.168.16.112:99

           // string data = "remoteuser=" + remoteuser + "&SequenceCode=" + responcePolicyGuid;
            string data = "/" + remoteuser + "/" + responcePolicyGuid;

            ResponseModelClone<string> result = HttpHelper.HttpGetClone(url, data);
            if (result.State == "1")
            {
                LKInsuranceResponse model = JsonConvert.DeserializeObject<LKInsuranceResponse>(result.Result);
                string aa = model.SequenceCode;
            }
            

        }

        private void myGridView3_MouseUp(object sender, MouseEventArgs e)
        {
            PopMenu.ShowPopupMenu(myGridView3, e, popupMenu1);
            int rows = myGridView3.FocusedRowHandle;

            //20200317 zb
            if (rows < 0)
            {
                return;
            }
            string state = myGridView3.GetRowCellValue(rows, "State").ToString();      
            barInsuranceInfo.Enabled = state == "ok" ? true : false;
            if (state != "1")
            {
                barAlter.Enabled = false;
            }
            else
            {
                barAlter.Enabled = true;
            }
            barAlter.Enabled = false;
            
        }

        private void barBillDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView3.FocusedRowHandle;
            string billNo = myGridView3.GetRowCellValue(rows, "BillNo").ToString();
            CommonClass.ShowBillSearch(billNo);

        }

        private void barInsuranceInfo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rows = myGridView3.FocusedRowHandle;
            string insuranceNO = myGridView3.GetRowCellValue(rows, "InsuranceNO").ToString();
            string responceState = myGridView3.GetRowCellValue(rows, "ResponceState").ToString();
            string responcePolicyGuid = myGridView3.GetRowCellValue(rows, "ResponcePolicyGuid").ToString();
            frmInsuranceDetail edit = new frmInsuranceDetail();
            edit.insuranceNO = insuranceNO;
            edit.responceState = responceState;
            edit.PolicyGuid = responcePolicyGuid;
            edit.ShowDialog();
        }

        private void barAlter_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowhandle = myGridView3.FocusedRowHandle;
                if (rowhandle < 0)
                {
                    MsgBox.ShowOK("请选择要修改的异常签收信息！");
                    return;
                }

                string insuranceNO = myGridView3.GetRowCellValue(rowhandle, "InsuranceNO").ToString();
                string responceState = myGridView3.GetRowCellValue(rowhandle, "ResponceState").ToString();
                frmInsuranceEdit edit = new frmInsuranceEdit();
                edit.insuranceNO = insuranceNO;
                edit.responceState = responceState;
                edit.ShowDialog();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView3);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView3);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView3.Guid.ToString());
        }

   
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView3);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView3, "投保信息");
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }


    }
}
