using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmBuKouAdd : ZQTMS.Tool.BaseForm
    {
        public frmBuKouAdd()
        {
            InitializeComponent();
        }
        private void frmTiaoZhangAdd_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this, false, true);

            OperDate.DateTime = CommonClass.gcdate;
            OperMan.Text = CommonClass.UserInfo.UserName;
           
            //CommonClass.SetCause(FromMan, false);
            Project_SelectedIndexChanged(null, null);
            ToMan.EditValue = CommonClass.UserInfo.WebName;
            FromMan.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {


            if (BillNo.Text.Trim() == "" || !GetDataByBillNOBool(BillNo.Text.Trim()))
            {
                MsgBox.ShowOK("请输入正确的单号!");
                BillNo.Focus();
                return;
            }
            string project = Project.Text.Trim();
            if (project == "")
            {
                MsgBox.ShowOK("请选择汇总项目!");
                Project.Focus();
                return;
            }
            string feeType = FeeType.Text.Trim();
            if (feeType == "")
            {
                MsgBox.ShowOK("请选择费用类型!");
                FeeType.Focus();
                return;
            }
            string fromMan = FromMan.Text.Trim();
            if (fromMan == "")
            {
                MsgBox.ShowOK("请选择补扣事业部!");
                FromMan.Focus();
                return;
            }
            decimal account = ConvertType.ToDecimal(Account.Text);
            if (account <= 0)
            {
                MsgBox.ShowOK("转账金额必须大于0!");
                Account.Focus();
                return;
            }
            if (AreaName.Text.Trim() == "")
            {
                MsgBox.ShowOK("必须选择大区!");
                AreaName.Focus();
                return;
            }
            string toMan = ToMan.Text.Trim();
            if (toMan == "")
            {
                MsgBox.ShowOK("请选择补扣账户名称!");
                ToMan.Focus();
                return;
            }


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Project", project));
            list.Add(new SqlPara("FeeType", feeType));
            list.Add(new SqlPara("FromMan", fromMan));
            list.Add(new SqlPara("Account", account));
            list.Add(new SqlPara("ToMan", toMan));
            list.Add(new SqlPara("Remark", Remark.Text.Trim()));
            list.Add(new SqlPara("BillNo", BillNo.Text.Trim()));
            list.Add(new SqlPara("TjNo", CommonClass.gcdate.ToString("yyyyMMddHHmmsss") + new Random().Next(1000, 10000)));
            list.Add(new SqlPara("AreaName", AreaName.Text.Trim()));
            list.Add(new SqlPara("InOrOut", InOrOut.Text.Trim()));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_BUKOU", list)) > 0)
            {
                MsgBox.ShowOK();
                this.Close();
            }
        }

        private void Project_SelectedIndexChanged(object sender, EventArgs e)
        {
            FeeType.Properties.Items.Clear();
            FeeType.Text = "";
            string[] str=null;

            str = CommonClass.Arg.BuKouFeeType.Split(',');

            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    FeeType.Properties.Items.Add(str[i]);
                }
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FromMan_SelectedIndexChanged(object sender, EventArgs e)
        {
            //AreaName.Text = "";
            //CommonClass.SetArea(AreaName, FromMan.Text, false);
        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ToMan.Text = "";
            ////CommonClass.SetCauseWeb(ToMan, FromMan.Text, AreaName.Text, false);

            //try
            //{
            //    DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("BelongCause like '" + FromMan.Text + "' and BelongArea like '" + AreaName.Text + "' and (WebRole='加盟' or WebRole='合作') ");
            //    ToMan.Properties.Items.Clear();
            //    ToMan.Text = "";
            //    for (int i = 0; i < dr.Length; i++)
            //    {
            //        ToMan.Properties.Items.Add(dr[i]["WebName"]);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowOK(ex.Message);
            //}

        }



        private void BillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetDataByBillNO(BillNo.Text.Trim());
            }
        }
        private bool GetDataByBillNOBool(string BillNo) 
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", BillNo));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BUKOU_JIESUANFEE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
                return false;
            else
                return true;
        }
        private void GetDataByBillNO(string BillNo)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", BillNo));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BUKOU_JIESUANFEE", list);
            DataSet ds = SqlHelper.GetDataSet(sps);

            if (ds == null || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                MsgBox.ShowOK("请检查单号是否正确！");
                this.BillNo.Text = "";
                return;
            }
            MainlineFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["MainlineFee"]);
            TransferFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["TransferFee"]);
            DepartureOptFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["DepartureOptFee"]);
            TerminalOptFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["TerminalOptFee"]);
            TerminalAllotFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["TerminalAllotFee"]);
            UpstairFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["UpstairFee_C"]);
            SupportValue_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["SupportValue_C"]);
            Tax_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["Tax_C"]);
            ReceiptFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["ReceiptFee_C"]);
            AgentFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["AgentFee_C"]);
            HandleFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["HandleFee_C"]);
            ForkliftFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["ForkliftFee_C"]);
            StorageFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["StorageFee_C"]);
            CustomsFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["CustomsFee_C"]);
            PackagFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["CustomsFee_C"]);
            FrameFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["FrameFee_C"]);
            NoticeFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["NoticeFee_C"]);
            Expense_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["Expense_C"]);
            InformationFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["InformationFee_C"]);
            FuelFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["FuelFee_C"]);
            WarehouseFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["WarehouseFee_C"]);
            OtherFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["OtherFee_C"]);
            ChangeFee_C.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["ChangeFee_C"]);
            DeliveryFee.Text = ConvertType.ToString(ds.Tables[0].Rows[0]["DeliveryFee"]);
        }
    }
}