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
    public partial class frmAddCars : BaseForm
    {
        public frmAddCars()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SiteName.Text.Trim() == "")
            {
                MsgBox.ShowOK("所属站点必须要填写！");
                return;
            }
            if (AreaName.Text.Trim() == "")
            {
                MsgBox.ShowOK("所属区域必须要填写！");
                return;
            }
            if (DepName.Text.Trim() == "")
            {
                MsgBox.ShowOK("所属操作单位必须要填写！");
                return;
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CarId", dr == null ? Guid.NewGuid() : dr["CarId"]));
                list.Add(new SqlPara("CarNo", CarNo.Text.Trim()));
                list.Add(new SqlPara("UsType", UsType.Text.Trim()));
                list.Add(new SqlPara("Cooperation", Cooperation.Text.Trim()));
                list.Add(new SqlPara("RecordDate", RecordDate.Text.Trim()));
                list.Add(new SqlPara("CarType", CarType.Text.Trim()));
                list.Add(new SqlPara("CarLength", ConvertType.ToDecimal(CarLength.Text)));
                list.Add(new SqlPara("CarWidth", ConvertType.ToDecimal(CarWidth.Text)));
                list.Add(new SqlPara("CarHeight", ConvertType.ToDecimal(CarHeight.Text)));
                list.Add(new SqlPara("EngineNo", EngineNo.Text.Trim()));
                list.Add(new SqlPara("CarrNO", CarrNO.Text.Trim()));
                list.Add(new SqlPara("BusinessCardNo", BusinessCardNo.Text.Trim()));
                list.Add(new SqlPara("CarColor", CarColor.Text.Trim()));
                list.Add(new SqlPara("BuyDate", BuyDate.Text.Trim()));
                list.Add(new SqlPara("CarWeight", ConvertType.ToDecimal(CarWeight.Text)));
                list.Add(new SqlPara("LoadWeight", ConvertType.ToDecimal(LoadWeight.Text)));
                list.Add(new SqlPara("LoadVolum", ConvertType.ToDecimal(LoadVolum.Text)));
                list.Add(new SqlPara("JqxBeginDate", JqxBeginDate.Text.Trim()));
                list.Add(new SqlPara("JqxEndDate", JqxEndDate.Text.Trim()));
                list.Add(new SqlPara("JqxFee", ConvertType.ToDecimal(JqxFee.Text)));
                list.Add(new SqlPara("SyBeginDate", SyBeginDate.Text.Trim()));
                list.Add(new SqlPara("SyEndDate", SyEndDate.Text.Trim()));
                list.Add(new SqlPara("SyNo", SyNo.Text.Trim()));
                list.Add(new SqlPara("SyFee", ConvertType.ToDecimal(SyFee.Text)));
                list.Add(new SqlPara("InsuredUnits", InsuredUnits.Text.Trim()));
                list.Add(new SqlPara("ScrapDate", ScrapDate.Text.Trim()));
                list.Add(new SqlPara("GPSNo", GPSNo.Text.Trim()));
                list.Add(new SqlPara("CarOwner", CarOwner.Text.Trim()));
                list.Add(new SqlPara("OwnerIDCardNo", OwnerIDCardNo.Text.Trim()));
                list.Add(new SqlPara("OwnerPhone", OwnerPhone.Text.Trim()));
                list.Add(new SqlPara("OwnerAddress", OwnerAddress.Text.Trim()));
                list.Add(new SqlPara("OwnerUnit", OwnerUnit.Text.Trim()));
                list.Add(new SqlPara("OwnerUnitAdd", OwnerUnitAdd.Text.Trim()));
                list.Add(new SqlPara("DriverName", DriverName.Text.Trim()));
                list.Add(new SqlPara("DriverIDCardNo", DriverIDCardNo.Text.Trim()));
                list.Add(new SqlPara("DriverPhone", DriverPhone.Text.Trim()));
                list.Add(new SqlPara("DriverAddress", DriverAddress.Text.Trim()));
                list.Add(new SqlPara("SiteName", SiteName.Text.Trim()));
                list.Add(new SqlPara("DepName", DepName.Text.Trim()));
                list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
                list.Add(new SqlPara("CarRemark", ""));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASCAR", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void frmVehicleFiles_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            SiteAll();
            AreaAll();

            SiteName.Text = CommonClass.UserInfo.SiteName;
            AreaName.Text = CommonClass.UserInfo.AreaName;
            DepName.Text = CommonClass.UserInfo.WebName;

            if (dr != null)
            {
                //CarId.EditValue = dr["CarId"];
                CarNo.EditValue = dr["CarNo"];
                UsType.EditValue = dr["UsType"];
                Cooperation.EditValue = dr["Cooperation"];
                RecordDate.EditValue = dr["RecordDate"];
                CarType.EditValue = dr["CarType"];
                CarLength.EditValue = dr["CarLength"];
                CarWidth.EditValue = dr["CarWidth"];
                CarHeight.EditValue = dr["CarHeight"];
                EngineNo.EditValue = dr["EngineNo"];
                CarrNO.EditValue = dr["CarrNO"];
                BusinessCardNo.EditValue = dr["BusinessCardNo"];
                CarColor.EditValue = dr["CarColor"];
                BuyDate.EditValue = dr["BuyDate"];
                CarWeight.EditValue = dr["CarWeight"];
                LoadWeight.EditValue = dr["LoadWeight"];
                LoadVolum.EditValue = dr["LoadVolum"];
                JqxBeginDate.EditValue = dr["JqxBeginDate"];
                JqxEndDate.EditValue = dr["JqxEndDate"];
                JqxFee.EditValue = dr["JqxFee"];
                SyBeginDate.EditValue = dr["SyBeginDate"];
                SyEndDate.EditValue = dr["SyEndDate"];
                SyNo.EditValue = dr["SyNo"];
                SyFee.EditValue = dr["SyFee"];
                InsuredUnits.EditValue = dr["InsuredUnits"];
                ScrapDate.EditValue = dr["ScrapDate"];
                GPSNo.EditValue = dr["GPSNo"];
                CarOwner.EditValue = dr["CarOwner"];
                OwnerIDCardNo.EditValue = dr["OwnerIDCardNo"];
                OwnerPhone.EditValue = dr["OwnerPhone"];
                OwnerAddress.EditValue = dr["OwnerAddress"];
                OwnerUnit.EditValue = dr["OwnerUnit"];
                OwnerUnitAdd.EditValue = dr["OwnerUnitAdd"];
                DriverName.EditValue = dr["DriverName"];
                DriverIDCardNo.EditValue = dr["DriverIDCardNo"];
                DriverPhone.EditValue = dr["DriverPhone"];
                DriverAddress.EditValue = dr["DriverAddress"];
                SiteName.EditValue = dr["SiteName"];
                DepName.EditValue = dr["DepName"];
                AreaName.EditValue = dr["AreaName"];
                //CarRemark.EditValue = dr["CarRemark"];

            }

        }

        private void SiteAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                SiteName.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SiteName.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void DepAll(string DepArea)
        {
            try
            {

                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    DataRow[] dtRows = ds.Tables[0].Select("DepArea='" + DepArea + "'");
                    DepName.Properties.Items.Clear();
                    for (int i = 0; i < dtRows.Length; i++)

                        DepName.Properties.Items.Add(dtRows[i]["DepName"].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void AreaAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASAREA", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables.Count != 0)
                    {
                        AreaName.Properties.Items.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

                            AreaName.Properties.Items.Add(ds.Tables[0].Rows[i]["AreaName"].ToString().Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void AreaName_TextChanged(object sender, EventArgs e)
        {
            DepAll(AreaName.Text.Trim());
        }
    }
}
