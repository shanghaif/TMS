using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Lib;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class fmbasTerminalOutFeeAdd : BaseForm
    {
        public Guid id = Guid.Empty;

        public fmbasTerminalOutFeeAdd()
        {
            InitializeComponent();
        }

        public fmbasTerminalOutFeeAdd(Guid _id) 
        {
            InitializeComponent();
            id = _id;
        }

        private void fmbasTerminalOutFeeAdd_Load(object sender, EventArgs e)
        {
            CommonClass.SetSite(FromSite, false);
            CommonClass.SetSite(ToSite, false);

            if (id != Guid.Empty)
            {
                try
                {
                    List<SqlPara> lst = new List<SqlPara>();
                    lst.Add(new SqlPara("basTerminalOutFeeID", id));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_basTerminalOutFee_byid", lst);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count==0)
                    {
                        return;
                    }
                    DataRow dr = ds.Tables[0].Rows[0];
                    FromSite.EditValue = dr["FromSite"];
                    ToSite.EditValue = dr["ToSite"];
                    CompanyName.EditValue = dr["CompanyName"];
                    OpName.EditValue = dr["OpName"];
                    CompanyAdd.EditValue = dr["CompanyAdd"];
                    phone.EditValue = dr["phone"];
                    minPrice.EditValue = dr["minPrice"];
                    price.EditValue = dr["price"];
                    SendFee.EditValue = dr["SendFee"];
                    UpstairFee.EditValue = dr["UpstairFee"];
                    isSign.EditValue = dr["isSign"];
                    AgentFee.EditValue = dr["AgentFee"];
                    evaluate.EditValue = dr["evaluate"];
                    ArrivalFee.EditValue = dr["ArrivalFee"];
                    DepartureDate.EditValue = dr["DepartureDate"];
                    ArrivalDate.EditValue = dr["ArrivalDate"];
                    Remark.EditValue = dr["Remark"];
                    RunTime.EditValue = dr["RunTime"];
                    getFeeSpeed.EditValue = dr["getFeeSpeed"];
                    weightPrice.EditValue = dr["weightPrice"];
                    volumePrice.EditValue = dr["volumePrice"];
                }
                catch (Exception EX)
                {
                    MsgBox.ShowException(EX);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> lst = new List<SqlPara>();
                lst.Add(new SqlPara("basTerminalOutFeeID", id == Guid.Empty ? Guid.NewGuid() : id));
                lst.Add(new SqlPara("FromSite", FromSite.Text.Trim()));
                lst.Add(new SqlPara("ToSite", ToSite.Text.Trim()));
                lst.Add(new SqlPara("CompanyName", CompanyName.Text.Trim()));
                lst.Add(new SqlPara("OpName", OpName.Text.Trim()));
                lst.Add(new SqlPara("CompanyAdd", CompanyAdd.Text.Trim()));
                lst.Add(new SqlPara("phone", phone.Text.Trim()));
                lst.Add(new SqlPara("minPrice", ConvertType.ToDecimal(minPrice.Text)));
                lst.Add(new SqlPara("price", ConvertType.ToDecimal(price.Text)));
                lst.Add(new SqlPara("SendFee", ConvertType.ToDecimal(SendFee.Text)));
                lst.Add(new SqlPara("UpstairFee", ConvertType.ToDecimal(UpstairFee.Text)));
                lst.Add(new SqlPara("isSign", isSign.Text));
                lst.Add(new SqlPara("AgentFee", ConvertType.ToDecimal(AgentFee.Text)));
                lst.Add(new SqlPara("evaluate", evaluate.Text.Trim()));
                lst.Add(new SqlPara("ArrivalFee", ConvertType.ToDecimal(ArrivalFee.Text)));
                lst.Add(new SqlPara("DepartureDate", DepartureDate.Text.Trim()));
                lst.Add(new SqlPara("ArrivalDate", ArrivalDate.Text.Trim()));
                lst.Add(new SqlPara("Remark", Remark.Text.Trim()));
                lst.Add(new SqlPara("RunTime", ConvertType.ToDecimal(RunTime.Text)));
                lst.Add(new SqlPara("getFeeSpeed", getFeeSpeed.Text.Trim()));
                lst.Add(new SqlPara("weightPrice", ConvertType.ToDecimal(weightPrice.Text)));
                lst.Add(new SqlPara("volumePrice", ConvertType.ToDecimal(volumePrice.Text)));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "QSP_update_basTerminalOutFee", lst);
                if(SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

    }
}
