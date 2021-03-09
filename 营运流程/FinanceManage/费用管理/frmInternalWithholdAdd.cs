using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.Text.RegularExpressions;
namespace ZQTMS.UI
{
    //luohui 201808015

    public partial class frmInternalWithholdAdd : BaseForm
    {

        public int type;
        public string id="";
        public string billno { get; set; }
        public string item { get; set; }
        public string feeType { get; set; }
        public string feeMoth { get; set; }
        public string money { get; set; }
        public string responsibleDepartment { get; set; }
        public string revenueDepartement { get; set; }
        public string remark { get; set; }
        public frmInternalWithholdAdd()
        {
            InitializeComponent();
        }
        DataSet dsFeeType = new DataSet();

        private DataSet GetFeeType()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_FeeType", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                return ds;

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return null;
            }
        }

        private void frmInternalWithholdAdd_Load(object sender, EventArgs e)
        {
            textEdit7.Properties.Items.Clear();
            CommonClass.SetWeb(textEdit7, false);
            textEdit7.Properties.Items.Add("深圳中强物流");
            //dsFeeType = GetFeeType();
            //if (dsFeeType != null && dsFeeType.Tables.Count > 0 && dsFeeType.Tables[0].Rows.Count > 0)
            //{
            //    DataRow[] drs = dsFeeType.Tables[0].Select("ParentID=0");
            //    if (drs.Length > 0)
            //    {
            //        textFeeType.Properties.Items.Clear();
            //        for (int i = 0; i < drs.Length; i++)
            //        {
            //            textFeeType.Properties.Items.Add(drs[i]["TypeName"].ToString());
            //        }
            //    }


            //}
            //List<SqlPara> list = new List<SqlPara>();

            //SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASDEPART", list);
            //DataSet ds = SqlHelper.GetDataSet(sps);
            //if (ds != null && ds.Tables.Count != 0)
            //{
            //    if (ds != null && ds.Tables.Count != 0)
            //    {
            //        textCDDpartement.Properties.Items.Clear();
            //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)

            //            textCDDpartement.Properties.Items.Add(ds.Tables[0].Rows[i]["DepName"].ToString().Trim());
            //    }
            //}
            textCDDpartement.Properties.Items.Clear();
            CommonClass.SetWeb(textCDDpartement, false);
            textCDDpartement.Properties.Items.Add("深圳中强物流");

            //zaj
            BelongYear.Properties.Items.Clear();
            BelongYear.Properties.Items.Add(DateTime.Now.Year - 1);
            BelongYear.Properties.Items.Add(DateTime.Now.Year);
            BelongYear.Properties.Items.Add(DateTime.Now.Year + 1);

            BelongYear.Text = DateTime.Now.Year.ToString();

            int mon = DateTime.Now.Month;
            if (mon < 10) { textMonth.Text = "0" + mon.ToString(); }
            else { textMonth.Text = mon.ToString(); }
        }
        //确定
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            //if (textBillNo.Text.Trim() == "")
            //{
            //    MsgBox.ShowOK("请输入运单号");
            //    return;
            //}

            if (textItem.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入项目");
                return;
            }

            if (textReamrk.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入摘要");
                return;
            }

            if (textMoney.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入金额");
                return;
            }

            if (textMonth.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入月份");
                return;
            }
            if (textFeeType.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入费用类型");
                return;
            }

            if (textCDDpartement.Text.Trim() == "")
            {
                MsgBox.ShowOK("承担部门不能为空");
                return;
            }

            if (textEdit7.Text.Trim() == "")
            {
                MsgBox.ShowOK("收益部门不能为空");
                return;
            }
            if (!Regex.IsMatch(textMoney.Text, posPattern))
            {
                MsgBox.ShowOK("输入金额格式不正确!");
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("BillNo", textBillNo.Text.Trim()));
            list.Add(new SqlPara("Item", textItem.Text.Trim()));
            list.Add(new SqlPara("FeeType", textFeeType.Text.Trim()));
            list.Add(new SqlPara("FeeMonth",BelongYear.Text.Trim()+ textMonth.Text.Trim()));
            list.Add(new SqlPara("Remark", textReamrk.Text.Trim()));
            list.Add(new SqlPara("Money", textMoney.Text.Trim()));
            list.Add(new SqlPara("ResponsibleDepartment", textCDDpartement.Text.Trim()));
            list.Add(new SqlPara("RevenueDepartement", textEdit7.Text.Trim()));
            list.Add(new SqlPara("type", type));
            list.Add(new SqlPara("ID", id));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_WITHHOLDING", list);

            if(SqlHelper.ExecteNonQuery(sps)>0)
            {
                MsgBox.ShowOK();

                Close();

            }
        }
        //关闭
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void getdata()
        {
            if (id == "") return;

            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WithHolding_BY_ID", new List<SqlPara> { new SqlPara("ID", id) }));

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            textBillNo.EditValue = ds.Tables[0].Rows[0]["BillNo"];
            textItem.EditValue = ds.Tables[0].Rows[0]["Item"];
            textReamrk.EditValue = ds.Tables[0].Rows[0]["Remark"];
            textMoney.Text = ds.Tables[0].Rows[0]["Money"].ToString();
            textMonth.Text = ds.Tables[0].Rows[0]["FeeMonth"].ToString().Substring(4,2);
            textFeeType.Text = ds.Tables[0].Rows[0]["FeeType"].ToString();
            textCDDpartement.Text = ds.Tables[0].Rows[0]["ResponsibleDepartment"].ToString();
            textEdit7.Text = ds.Tables[0].Rows[0]["RevenueDepartement"].ToString();
            BelongYear.Text = ds.Tables[0].Rows[0]["FeeMonth"].ToString().Substring(0,4);

        
        }

        private void textFeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string feeType = textFeeType.Text.Trim();
            //string sql = "TypeName='" + feeType + "' and ParentID=0";

            //DataRow[] drs = dsFeeType.Tables[0].Select(sql);
            //int id = 0;
            //if (drs.Length > 0)
            //{
            //    id = Convert.ToInt32(drs[0]["FeeID"]);
            //}
            //textItem.Text = "";
            //textItem.Properties.Items.Clear();
            //string sqlstr = "ParentID=" + id;
            //DataRow[] drs1 = dsFeeType.Tables[0].Select(sqlstr);
            //if (drs1.Length > 0)
            //{
            //    for (int i = 0; i < drs1.Length; i++)
            //    {
            //        textItem.Properties.Items.Add(drs1[i]["TypeName"].ToString());
            //    }
            //}
        }

        private void textCDDpartement_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetArea(textCDDpartement, textEdit7.Text);

            //CommonClass.SetArea(Area, Cause.Text);
            //CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
          
        }


    }
}