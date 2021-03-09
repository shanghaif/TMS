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
using Newtonsoft.Json;

namespace ZQTMS.UI
{
    public partial class frmAddCarrierUnit : BaseForm
    {
        public frmAddCarrierUnit()
        {
            InitializeComponent();
        }

        public DataRow dr = null;
        private void frmAddCarrierUnit_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            SiteAll();
            if (dr != null) 
            {
                //CUId.EditValue = dr["CUId"];
                CompanyName.EditValue = dr["CompanyName"];
                CUSite.EditValue = dr["CUSite"];
                CUWeb.EditValue = dr["CUWeb"];
                Mainline.EditValue = dr["Mainline"];
                CUArriveSite.EditValue = dr["CUArriveSite"];
                MinimumBill.EditValue = dr["MinimumBill"];
                HeavyPrice.EditValue = dr["HeavyPrice"];
                LightPrice.EditValue = dr["LightPrice"];
                DeliFee.EditValue = dr["DeliFee"];
                AriiveMan.EditValue = dr["AriiveMan"];
                ArrivePhone.EditValue = dr["ArrivePhone"];
                ArriveCellPhone.EditValue = dr["ArriveCellPhone"];
                ArriveAddress.EditValue = dr["ArriveAddress"];
                Salesman.EditValue = dr["Salesman"];
                SalesPhone.EditValue = dr["SalesPhone"];
                SalesCellPhone.EditValue = dr["SalesCellPhone"];
                SalesAddress.EditValue = dr["SalesAddress"];
                CreditLevel.EditValue = dr["CreditLevel"];
                IsSigned.EditValue = dr["IsSigned"];
                LegalPerson.EditValue = dr["LegalPerson"];
                Deposit.EditValue = dr["Deposit"];
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CUId", dr == null ? Guid.NewGuid() : dr["CUId"]));
                list.Add(new SqlPara("CompanyName", CompanyName.Text.Trim()));
                list.Add(new SqlPara("CUSite", CUSite.Text.Trim()));
                list.Add(new SqlPara("CUWeb", CUWeb.Text.Trim()));
                list.Add(new SqlPara("Mainline", Mainline.Text.Trim()));
                list.Add(new SqlPara("CUArriveSite", CUArriveSite.Text.Trim()));
                list.Add(new SqlPara("MinimumBill", MinimumBill.Text.Trim()));
                list.Add(new SqlPara("HeavyPrice", HeavyPrice.Text.Trim()));
                list.Add(new SqlPara("LightPrice", LightPrice.Text.Trim()));
                list.Add(new SqlPara("DeliFee", DeliFee.Text.Trim()));
                list.Add(new SqlPara("AriiveMan", AriiveMan.Text.Trim()));
                list.Add(new SqlPara("ArrivePhone", ArrivePhone.Text.Trim()));
                list.Add(new SqlPara("ArriveCellPhone", ArriveCellPhone.Text.Trim()));
                list.Add(new SqlPara("ArriveAddress", ArriveAddress.Text.Trim()));
                list.Add(new SqlPara("Salesman", Salesman.Text.Trim()));
                list.Add(new SqlPara("SalesPhone", SalesPhone.Text.Trim()));
                list.Add(new SqlPara("SalesCellPhone", SalesCellPhone.Text.Trim()));
                list.Add(new SqlPara("SalesAddress", SalesAddress.Text.Trim()));
                list.Add(new SqlPara("CreditLevel", CreditLevel.Text.Trim()));
                list.Add(new SqlPara("IsSigned", IsSigned.Text.Trim()));
                list.Add(new SqlPara("LegalPerson", LegalPerson.Text.Trim()));
                list.Add(new SqlPara("Deposit", Deposit.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BASCARRIERUNIT", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();

                    #region //承运商/线路接口
                    Rootobject obj = new Rootobject();
                    Stations sta = new Stations();
                    obj.carrierName = CompanyName.Text.Trim();//承运商名称
                    obj.carrierContacts = Salesman.Text.Trim();//联系人
                    obj.carrierMobile = SalesPhone.Text.Trim();//联系电话
                    obj.carrierAddress = SalesAddress.Text.Trim();//地址
                    obj.remarks = "";//备注
                    sta.fzStationName = CUSite.Text.Trim();//发站线路(站点)名称
                    sta.fzContacts = "";//发站联系人
                    sta.fzMobile = "";//发站电话
                    sta.fzAddress = "";
                    sta.fzRemarks = "";
                    sta.fzArea = new[] { "", "" };
                    sta.dzStationName = CUArriveSite.Text.Trim();//到站站点
                    sta.dzContacts = AriiveMan.Text.Trim();//到站联系人
                    sta.dzMobile = ArriveCellPhone.Text.Trim();//到站电话
                    sta.dzAddress = ArriveAddress.Text.Trim();//到站地址
                    sta.dzRemarks = "";
                    sta.dzArea = new[] { "", "" };
                    obj.stations = sta;

                    Dictionary<string, object> hashMap = new Dictionary<string, object>();
                    hashMap.Add("carrierName", obj.carrierName); //现在承运商的数据拿的是错的
                    hashMap.Add("carrierContacts", obj.carrierContacts);
                    hashMap.Add("carrierMobile", obj.carrierMobile);
                    hashMap.Add("carrierAddress", obj.carrierAddress);
                    hashMap.Add("remarks", obj.remarks);
                    hashMap.Add("stations", obj.stations);

                    string json = JsonConvert.SerializeObject(hashMap);
                    string url = "http://120.76.141.227:9882/umsv2.biz/open/customer/add_trunk_carrier";
                    string resultStr = string.Empty;
                    try
                    {
                        HttpHelper.HttpPostJava(json, url);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    #endregion

                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                CUSite.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CUSite.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void getWeb(string SiteName)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                DataRow[] dtRows = ds.Tables[0].Select("SiteName='" + SiteName + "'");
                CUWeb.Properties.Items.Clear();
                for (int i = 0; i < dtRows.Length; i++)
                {
                    CUWeb.Properties.Items.Add(dtRows[i]["WebName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void CUSite_TextChanged(object sender, EventArgs e)
        {
            getWeb(CUSite.Text.Trim());
        }
    }
}


public class Rootobject 
{
    public string carrierName { get; set; }
    public string carrierContacts { get; set; }
    public string carrierMobile { get; set; }
    public string carrierAddress { get; set; }
    public string remarks { get; set; }
    public Stations stations { get; set; }
}

public class Stations
{
    public string fzStationName { get; set; }
    public string[] fzArea { get; set; }
    public string fzContacts { get; set; }
    public string fzMobile { get; set; }
    public string fzAddress { get; set; }
    public string fzRemarks { get; set; }
    public string dzStationName { get; set; }
    public string[] dzArea { get; set; }
    public string dzContacts { get; set; }
    public string dzMobile { get; set; }
    public string dzAddress { get; set; }
    public string dzRemarks { get; set; }
}
