using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.UI;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmAddRecCustData :BaseForm
    {
        public frmAddRecCustData()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RecievAddProv_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(RecievAddCity, RecievAddProv.EditValue);
            RecievAddCity.SelectedIndex = 0;
            
        }

        private void RecievAddCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(RecievAddArea, RecievAddCity.EditValue);
            RecievAddArea.SelectedIndex = 0;
        }

        private void RecievAddArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.AreaManager.FillCityToImageComBoxEdit(RecievAddStreet, RecievAddArea.EditValue);
            RecievAddStreet.SelectedIndex = 0;
        }

        private void frmAddRecCustData_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.SetSite(BelongSite,false);
            BelongSite.SelectedIndex = 0;
            CommonClass.SetSite(MidSite, false);
            MidSite.SelectedIndex = 0;
            CommonClass.SetSite(ArriveSite, false);
            ArriveSite.SelectedIndex = 0;
            CommonClass.SetArea(BelongArea,"全部",false);
            BelongArea.SelectedIndex = 0;
            CommonClass.SetWeb(DestinationWeb,"全部",false);
            DestinationWeb.SelectedIndex = 0;
            getCusData();
            CusID.SelectedIndex = 0;

            CommonClass.AreaManager.FillCityToImageComBoxEdit(RecievAddProv, "0");
            RecievAddProv.SelectedIndexChanged += new System.EventHandler(this.RecievAddProv_SelectedIndexChanged);
            RecievAddCity.SelectedIndexChanged += new System.EventHandler(this.RecievAddCity_SelectedIndexChanged);
            RecievAddArea.SelectedIndexChanged += new System.EventHandler(this.RecievAddArea_SelectedIndexChanged);
            RecievAddProv.SelectedIndex = 0;
            if (dr != null) 
            {
                //RCID.EditValue = dr["RCID"];
                CusID.EditValue = dr["CusID"];
                BelongSite.EditValue = dr["BelongSite"];
                BelongArea.EditValue = dr["BelongArea"];
                RCName.EditValue = dr["RCName"];
                ContactMan.EditValue = dr["ContactMan"];
                ContactPhone.EditValue = dr["ContactPhone"];
                ContactCellPhone.EditValue = dr["ContactCellPhone"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["RecievAddProv"].ToString().Trim(), RecievAddProv);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["RecievAddCity"].ToString().Trim(), RecievAddCity);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["RecievAddArea"].ToString().Trim(), RecievAddArea);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["RecievAddStreet"].ToString().Trim(), RecievAddStreet);
                RecievAddress.EditValue = dr["RecievAddress"];
                ArriveSite.EditValue = dr["ArriveSite"];
                MidSite.EditValue = dr["MidSite"];
                TransferMode.EditValue = dr["TransferMode"];
                DestinationWeb.EditValue = dr["DestinationWeb"];
                CooperateDate.EditValue = dr["CooperateDate"];
                BelongWeb.EditValue = dr["BelongWeb"];
                CusRemarkInfo.EditValue = dr["CusRemarkInfo"];
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("RCID", dr == null ? Guid.NewGuid() : dr["RCID"]));
                list.Add(new SqlPara("CusID", CusID.Text.Trim()));
                list.Add(new SqlPara("BelongSite", BelongSite.Text.Trim()));
                list.Add(new SqlPara("BelongArea", BelongArea.Text.Trim()));
                list.Add(new SqlPara("RCName", RCName.Text.Trim()));
                list.Add(new SqlPara("ContactMan", ContactMan.Text.Trim()));
                list.Add(new SqlPara("ContactPhone", ContactPhone.Text.Trim()));
                list.Add(new SqlPara("ContactCellPhone", ContactCellPhone.Text.Trim()));
                list.Add(new SqlPara("RecievAddProv", RecievAddProv.Text.Trim()));
                list.Add(new SqlPara("RecievAddCity", RecievAddCity.Text.Trim()));
                list.Add(new SqlPara("RecievAddArea", RecievAddArea.Text.Trim()));
                list.Add(new SqlPara("RecievAddStreet", RecievAddStreet.Text.Trim()));
                list.Add(new SqlPara("RecievAddress", RecievAddress.Text.Trim()));
                list.Add(new SqlPara("ArriveSite", ArriveSite.Text.Trim()));
                list.Add(new SqlPara("MidSite", MidSite.Text.Trim()));
                list.Add(new SqlPara("TransferMode", TransferMode.Text.Trim()));
                list.Add(new SqlPara("DestinationWeb", DestinationWeb.Text.Trim()));
                list.Add(new SqlPara("CooperateDate", CooperateDate.Text.Trim()));
                list.Add(new SqlPara("BelongWeb", BelongWeb.Text.Trim()));
                list.Add(new SqlPara("CusRemarkInfo", CusRemarkInfo.Text.Trim()));//收货人备注信息 hj20181107

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASRECEIVECUST", list);
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

        private void BelongSite_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(BelongWeb,BelongSite.Text.Trim(),false);
            BelongWeb.SelectedIndex = 0;
        }
        private void getCusData() 
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASCUST", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null && ds.Tables.Count != 0)
                {
                    if (ds != null && ds.Tables.Count != 0)
                    {
                        CusID.Properties.Items.Clear();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            CusID.Properties.Items.Add(ds.Tables[0].Rows[i]["ContactMan"].ToString().Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
