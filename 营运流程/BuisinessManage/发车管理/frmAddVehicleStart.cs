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
    public partial class frmAddVehicleStart : BaseForm
    {
        public DataRow dr_;
        public DataSet ds_;
        public frmAddVehicleStart()
        {
            InitializeComponent();
        }

        private void frmAddVehicleStart_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            // CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);

            StartTime.DateTime = CommonClass.gcdate;
            LastStartTime.DateTime = CommonClass.gcdate;
            EstArriveTime.DateTime = CommonClass.gcdate;

            CommonClass.SetSite(StartSite, false);
            CommonClass.SetSite(ArriveSite, false);

            StartSite.EditValue = CommonClass.UserInfo.SiteName;
            StartWeb.EditValue = CommonClass.UserInfo.WebName;

            if (dr_ != null)
            {
                try
                {
                    BatchNO.EditValue = dr_["BatchNO"];
                    Driver.EditValue = dr_["Driver"];
                    DriverPhone.EditValue = dr_["DriverPhone"];
                    VehicleNO.EditValue = dr_["VehicleNO"];
                    ArriveSite.EditValue = dr_["ArriveSite"];
                    ArriveWeb.EditValue = dr_["ArriveWeb"];
                    Type.EditValue = dr_["Type"];
                    EstArriveTime.EditValue = dr_["EstArriveTime"];
                    LastStartTime.EditValue = dr_["LastStartTime"];
                    StartTime.EditValue = dr_["StartTime"];
                    StartSite.EditValue = dr_["StartSite"];
                    StartWeb.EditValue = dr_["StartWeb"];
                }
                catch
                {

                }
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLVEHICLESTAR_DRIVER", list);
                ds_ = SqlHelper.GetDataSet(sps);
            }
            catch
            {
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(BatchNO.Text.Trim()))
                {
                    MsgBox.ShowError("批次号不能为空！");
                    BatchNO.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(Driver.Text.Trim()))
                {
                    MsgBox.ShowError("驾驶人不能为空！");
                    Driver.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(DriverPhone.Text.Trim()))
                {
                    MsgBox.ShowError("联系电话不能为空！");
                    DriverPhone.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(VehicleNO.Text.Trim()))
                {
                    MsgBox.ShowError("承运车号不能为空！");
                    VehicleNO.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(ArriveSite.Text.Trim()))
                {
                    MsgBox.ShowError("到车站点不能为空！");
                    ArriveSite.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(ArriveWeb.Text.Trim()))
                {
                    MsgBox.ShowError("到车网点不能为空！");
                    ArriveWeb.Focus();
                    return;
                }

                if (MsgBox.ShowYesNo("是否保存？请确认！") != DialogResult.Yes)
                {
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BatchNO", BatchNO.Text.Trim()));
                list.Add(new SqlPara("Type", Type.Text.Trim()));
                list.Add(new SqlPara("ID", (dr_ == null || string.IsNullOrEmpty(dr_["ID"] + "") ? Guid.NewGuid() : dr_["ID"])));
                list.Add(new SqlPara("VehicleNO", VehicleNO.Text.Trim()));
                list.Add(new SqlPara("Driver", Driver.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("StartTime", StartTime.Text.Trim()));
                list.Add(new SqlPara("StartSite", StartSite.Text.Trim()));
                list.Add(new SqlPara("StartWeb", StartWeb.Text.Trim()));
                list.Add(new SqlPara("ArriveSite", ArriveSite.Text.Trim()));
                list.Add(new SqlPara("ArriveWeb", ArriveWeb.Text.Trim()));
                list.Add(new SqlPara("BillMan", (dr_ == null || string.IsNullOrEmpty(dr_["BillMan"] + "") ? CommonClass.UserInfo.UserName : dr_["BillMan"])));
                list.Add(new SqlPara("EstArriveTime", EstArriveTime.Text.Trim()));
                list.Add(new SqlPara("LastStartTime", LastStartTime.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLVEHICLESTAR", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string[] arr = ex.Message.Split('：');
                MsgBox.ShowError(arr[arr.Length - 1]);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StartSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(StartWeb, StartSite.Text.Trim(), false);
        }

        private void ArriveSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetWeb(ArriveWeb, ArriveSite.Text.Trim(), false);
        }

        private void BatchNO_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void myGridControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (myGridView1.FocusedRowHandle < 0) return;

            this.BatchNO.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "BatchNO"));
            this.StartTime.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SDT"));
            this.StartSite.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SSite"));
            this.StartWeb.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "SWeb"));
            this.VehicleNO.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "CarNo"));
            this.Driver.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Driver"));
            this.DriverPhone.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "DriverPhone"));
            this.ArriveSite.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "ESite"));
            this.ArriveWeb.EditValue = ConvertType.ToString(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "EWeb"));
            myGridControl1.Visible = false;
        }

        private void BatchNO_Click(object sender, EventArgs e)
        {

        }

        private void frmAddVehicleStart_Click(object sender, EventArgs e)
        {
            myGridControl1.Visible = false;
        }

        private void ArriveSite_TextChanged(object sender, EventArgs e)
        {
            ArriveWeb.Properties.Items.Clear();
            string[] site = ArriveSite.Text.Trim().Split(',');
            for (int i = 0; i < site.Length; i++)
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                for (int k = 0; k < dr.Length; k++)
                {
                    ArriveWeb.Properties.Items.Add(dr[k]["WebName"]);
                }
            }
        }

        private void Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dss = ds_.Tables[0].Clone();

            DataRow[] drs = ds_.Tables[0].Select("Type = '" + Type.Text.Trim() + "'");

            for (int i = 0; i < drs.Length; i++)
            {
                dss.ImportRow(drs[i]);
            }

            myGridControl1.DataSource = dss;
            //myGridControl1.Visible = true;
        }

        private void BatchNO_Enter(object sender, EventArgs e)
        {
            myGridControl1.Left = Type.Left;
            myGridControl1.Top = Type.Top + Type.Height;
            myGridControl1.Visible = true;
            myGridControl1.BringToFront();
        }

        private void BatchNO_Leave(object sender, EventArgs e)
        {
            if (!myGridControl1.Focused)
            {
                myGridControl1.Visible = false;
            }
        }
    }
}