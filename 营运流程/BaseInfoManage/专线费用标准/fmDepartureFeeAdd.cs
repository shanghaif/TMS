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
    public partial class fmDepartureFeeAdd : BaseForm
    {
        public DataRow dr = null;

        public delegate void deteype(string type);
        public event deteype type;
        private string t = "";
        public fmDepartureFeeAdd()
        {
            InitializeComponent();
        }

        public fmDepartureFeeAdd(string type)
        {
            InitializeComponent();
            t = type;
        }

        private void fmDepartureFeeAdd_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
           // CommonClass.SetSite(FromSite, true);
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Province, "0");
            Province.SelectedIndexChanged += new System.EventHandler(this.edsheng_SelectedIndexChanged);
            City.SelectedIndexChanged += new System.EventHandler(this.edcity_SelectedIndexChanged);
            companyid.Text = CommonClass.UserInfo.companyid;
            if (dr != null)
            {
                FeeType.EditValue = dr["FeeType"];
                Price.EditValue = dr["Price"];
                Unit.EditValue = dr["Unit"];
                Descript.EditValue = dr["Descript"];
            }

        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(City, Province.EditValue);
        }

        private void edcity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(Area, City.EditValue);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Price.Text.Trim() == "")
            {
                MsgBox.ShowOK("价格不能为空!");
                return;
            }
            try
            {
                if (t == "add") //新增
                {
                    List<SqlPara> list = new List<SqlPara>();
                    //list.Add(new SqlPara("ID", id));
                    list.Add(new SqlPara("FeeType", FeeType.Text.Trim()));
                    list.Add(new SqlPara("Price", Price.Text.Trim()));
                    list.Add(new SqlPara("Descript", Descript.Text.Trim()));
                    list.Add(new SqlPara("Unit", Unit.Text.Trim()));
                    //list.Add(new SqlPara("companyid", companyid.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basDepartureFee", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                }
                else  //修改
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ID", ConvertType.ToInt32(t)));
                    list.Add(new SqlPara("FeeType", FeeType.Text.Trim()));
                    list.Add(new SqlPara("Price", Price.Text.Trim()));
                    list.Add(new SqlPara("Descript", Descript.Text.Trim()));
                    list.Add(new SqlPara("Unit", Unit.Text.Trim()));
                    //list.Add(new SqlPara("companyid", companyid.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_update_basDepartureFee", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                
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
        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    companyid.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }




    }
}