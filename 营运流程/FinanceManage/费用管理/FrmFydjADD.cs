using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using KMS.Tool;
using KMS.SqlDAL;
using KMS.Common;
using System.Text.RegularExpressions;

namespace KMS.UI
{
    public partial class FrmFydjADD : BaseForm
    {

        public string pingzheng1 = "", pici1 = "", khxm1 = "", yhzh1 = "", khyh1 = "", ID1 = "", hexiao1 = "",ApplyDate1="",ApplyCause1="",ApplyDept1="",
            ApplyArea1="",ApplyDeptE1="",FeeType1="",FeeProject1="",AssumeDept1="",BelongMonth1="",BelongYear1="",
     Money1 = "", ApplyMan1 = "", BankSubbranch1 = "", BankProvince1 = "", BankCity1 = "", 
     TransferType1 = "", ExpendType1 = "", PayDate1 = "", Remark1 = "";
        public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取
        DataSet dsFeeType = new DataSet();

        public FrmFydjADD()
        {
            InitializeComponent();
        }

        private void FrmFydjADD_Load(object sender, EventArgs e)
        {
            CommonClass.SetCause(CauseName, false);
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_hkzl", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            myGridControl1.DataSource = ds.Tables[0];
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);  
            if (ID1 == "")
            {  
                CauseName.EditValue = CommonClass.UserInfo.CauseName;
                AreaName.EditValue = CommonClass.UserInfo.AreaName;
                ApplyDept.EditValue = CommonClass.UserInfo.WebName;
                CommonClass.SetWeb(ApplyDept, false);
                CommonClass.SetWeb(AssumeDept, false);
                AssumeDept.Properties.Items.Add("");
                AssumeDept.EditValue = CommonClass.UserInfo.WebName;
                CommonClass.SetUser(ApplyMan, ApplyDept.Text, false);
                ApplyDate.EditValue = CommonClass.gcdate;
                ApplyMan.Text = CommonClass.UserInfo.UserName;
                dsFeeType = GetFeeType();
                if (dsFeeType != null && dsFeeType.Tables.Count > 0 && dsFeeType.Tables[0].Rows.Count > 0)
                {
                    DataRow[] drs = dsFeeType.Tables[0].Select("ParentID=0");
                    if (drs.Length > 0)
                    {
                        FeeType.Properties.Items.Clear();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            FeeType.Properties.Items.Add(drs[i]["TypeName"].ToString());
                        }
                    }


                }
                BelongYear.Properties.Items.Clear();
                BelongYear.Properties.Items.Add(DateTime.Now.Year - 1);
                BelongYear.Properties.Items.Add(DateTime.Now.Year);
                BelongYear.Properties.Items.Add(DateTime.Now.Year + 1);

                BelongYear.Text = DateTime.Now.Year.ToString();


                int mon = DateTime.Now.Month;
                if (mon < 10) { BelongMonth.Text = "0" + mon.ToString(); }
                else { BelongMonth.Text = mon.ToString(); }
                textEdit1.Text = "TX" + CommonClass.gcdate.ToString("yyyyMMddHHmmss");
            }
            if (ID1 != "")
            {
                edbankman.Text = khxm1;
                edbankcode.Text = yhzh1;
                edbankname.Text = khyh1;
                textEdit1.Text = pici1;
                 edbankman.Text=khxm1;
                 edbankcode.Text=yhzh1;
                edbankname.Text=khyh1;
                ApplyDate.Text=ApplyDate1;
                 CauseName.Text=ApplyCause1;
                 AreaName.Text=ApplyArea1;
                 ApplyDept.Text=ApplyDept1;
                 FeeType.Text=FeeType1;
                 FeeProject.Text=FeeProject1;
                 AssumeDept.Text=AssumeDept1;
                 BelongYear.Text = BelongYear1;
                 BelongMonth.Text = BelongMonth1;
                 Money.Text=Money1;
                 ApplyMan.Text=ApplyMan1;
                 edbankchild.Text=BankSubbranch1;
                 textEdit2.Text=BankProvince1;
                 textEdit3.Text=BankCity1;
                 edopertype.Text=TransferType1;
                 edouttype.Text=ExpendType1;
                 edbilldate.Text=PayDate1;
                 textEdit1.Text=pici1;
                 Remark.Text=Remark1;




            }
        }
        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (ID1 == "")
            {
              if (FeeType.Text.Trim() == "")
                {
                    MsgBox.ShowOK("费用类型必填!");
                    return;
                } 
                if (Money.Text.Trim() == "")
                {
                    MsgBox.ShowOK("金额必填!");
                    return;
                }
                if (edbankman.Text.Trim() == "")
                {
                    MsgBox.ShowOK("开户姓名!");
                    return;
                }
                if (edbankcode.Text.Trim() == "")
                {
                    MsgBox.ShowOK("银行账号!");
                    return;
                }
                if (edbankname.Text.Trim() == "")
                {
                    MsgBox.ShowOK("开户银行!");
                    return;
                }
                if (textEdit2.Text.Trim() == "")
                {
                    MsgBox.ShowOK("所属省分!");
                    return;
                }
                if (textEdit3.Text.Trim() == "")
                {
                    MsgBox.ShowOK("所属城市!");
                    return;
                }
                if (edopertype.Text.Trim() == "")
                {
                    MsgBox.ShowOK("转账类型!");
                    return;
                }
                if (Remark.Text.Trim() == "")
                {
                    MsgBox.ShowOK("摘要!");
                    return;
                }         
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("khxm", edbankman.Text.Trim()));
                    list.Add(new SqlPara("yhzh", edbankcode.Text.Trim()));
                    list.Add(new SqlPara("khyh", edbankname.Text.Trim()));
                    list.Add(new SqlPara("ApplyDate", ApplyDate.Text.Trim()));
                    list.Add(new SqlPara("ApplyCause", CauseName.Text.Trim()));
                    list.Add(new SqlPara("ApplyArea", AreaName.Text.Trim()));
                    list.Add(new SqlPara("ApplyDept", ApplyDept.Text.Trim()));
                    list.Add(new SqlPara("FeeType", FeeType.Text.Trim()));
                    list.Add(new SqlPara("FeeProject", FeeProject.Text.Trim()));
                    list.Add(new SqlPara("AssumeDept", AssumeDept.Text.Trim()));
                    list.Add(new SqlPara("BelongYear", BelongYear.Text.Trim()));
                    list.Add(new SqlPara("BelongMonth",BelongMonth.Text.Trim()));
                    list.Add(new SqlPara("Money", Money.Text.Trim()));
                    list.Add(new SqlPara("ApplyMan", ApplyMan.Text.Trim()));
                    list.Add(new SqlPara("BankSubbranch", edbankchild.Text.Trim()));
                    list.Add(new SqlPara("BankProvince", textEdit2.Text.Trim()));
                    list.Add(new SqlPara("BankCity", textEdit3.Text.Trim()));
                    list.Add(new SqlPara("TransferType", edopertype.Text.Trim()));
                    list.Add(new SqlPara("ExpendType", edouttype.Text.Trim()));
                    list.Add(new SqlPara("PayDate", edbilldate.Text.Trim()));
                    list.Add(new SqlPara("pici", textEdit1.Text.Trim()));
                    list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_INS_Fydj", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                    else
                    {
                        MsgBox.ShowError("请填写完整信息");
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (ID1 != "")
            {
                 if (FeeType.Text.Trim() == "")
                {
                    MsgBox.ShowOK("费用类型必填!");
                    return;
                } 
                if (Money.Text.Trim() == "")
                {
                    MsgBox.ShowOK("金额必填!");
                    return;
                }
                if (edbankman.Text.Trim() == "")
                {
                    MsgBox.ShowOK("开户姓名!");
                    return;
                }
                if (edbankcode.Text.Trim() == "")
                {
                    MsgBox.ShowOK("银行账号!");
                    return;
                }
                if (edbankname.Text.Trim() == "")
                {
                    MsgBox.ShowOK("开户银行!");
                    return;
                }
                if (textEdit2.Text.Trim() == "")
                {
                    MsgBox.ShowOK("所属省分!");
                    return;
                }
                if (textEdit3.Text.Trim() == "")
                {
                    MsgBox.ShowOK("所属城市!");
                    return;
                }
                if (edopertype.Text.Trim() == "")
                {
                    MsgBox.ShowOK("转账类型!");
                    return;
                }
                if (Remark.Text.Trim() == "")
                {
                    MsgBox.ShowOK("摘要!");
                    return;
                }         
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("khxm1", edbankman.Text.Trim()));
                    list.Add(new SqlPara("yhzh1", edbankcode.Text.Trim()));
                    list.Add(new SqlPara("khyh1", edbankname.Text.Trim()));
                    list.Add(new SqlPara("ApplyDate1", ApplyDate.Text.Trim()));
                    list.Add(new SqlPara("ApplyCause1", CauseName.Text.Trim()));
                    list.Add(new SqlPara("ApplyArea1", AreaName.Text.Trim()));
                    list.Add(new SqlPara("ApplyDept1", ApplyDept.Text.Trim()));
                    list.Add(new SqlPara("FeeType1", FeeType.Text.Trim()));
                    list.Add(new SqlPara("FeeProject1", FeeProject.Text.Trim()));
                    list.Add(new SqlPara("AssumeDept1", AssumeDept.Text.Trim()));
                    list.Add(new SqlPara("BelongYear1", BelongYear.Text.Trim()));
                    list.Add(new SqlPara("BelongMonth1", BelongMonth.Text.Trim()));
                    list.Add(new SqlPara("Money1", Money.Text.Trim()));
                    list.Add(new SqlPara("ApplyMan1", ApplyMan.Text.Trim()));
                    list.Add(new SqlPara("BankSubbranch1", edbankchild.Text.Trim()));
                    list.Add(new SqlPara("BankProvince1", textEdit2.Text.Trim()));
                    list.Add(new SqlPara("BankCity1", textEdit3.Text.Trim()));
                    list.Add(new SqlPara("TransferType1", edopertype.Text.Trim()));
                    list.Add(new SqlPara("ExpendType1", edouttype.Text.Trim()));
                    list.Add(new SqlPara("PayDate1", edbilldate.Text.Trim()));
                    list.Add(new SqlPara("pici1", textEdit1.Text.Trim()));
                    list.Add(new SqlPara("Remark1", Remark.Text.Trim()));
                    list.Add(new SqlPara("ID1", ID1));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_Fydj", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                    else 
                    {
                        MsgBox.ShowError("123");
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }


        private void label20_Click(object sender, EventArgs e)
        {

        }
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
        private void edbankman_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void edbankman_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            { myGridControl1.Visible = true; }
            if (e.KeyCode == Keys.Escape)
            {
                myGridControl1.Visible = false;
            }
        }
        private void edbankman_Leave(object sender, EventArgs e)
        {
            if (!myGridControl1.Focused)
            {
                myGridControl1.Visible = false;
            }
        }

        private void edbankman_Enter(object sender, EventArgs e)
        {
            myGridControl1.Left = edbankman.Left;
            myGridControl1.Top = edbankman.Top + edbankman.Height;
            myGridControl1.Visible = true;
            myGridControl1.BringToFront();
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            int rows = myGridView1.FocusedRowHandle;
            if (rows < 0)
            {
                return;
            }
            edbankman.Text = myGridView1.GetRowCellValue(rows, "hm").ToString();
            edbankcode.Text = myGridView1.GetRowCellValue(rows, "zh").ToString();
            edbankname.Text = myGridView1.GetRowCellValue(rows, "khh").ToString();
            textEdit2.Text = myGridView1.GetRowCellValue(rows, "szs").ToString();
            textEdit3.Text = myGridView1.GetRowCellValue(rows, "szshi").ToString();
            myGridControl1.Visible = false;
        }
        private void CauseName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CommonClass.SetArea(AreaName, CauseName.Text);
            CommonClass.SetCauseWeb(ApplyDept, CauseName.Text, AreaName.Text);
        }

        private void ApplyDept_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void FeeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string feeType = FeeType.Text.Trim();
            string sql = "TypeName='" + feeType + "' and ParentID=0";

            DataRow[] drs = dsFeeType.Tables[0].Select(sql);
            int id = 0;
            if (drs.Length > 0)
            {
                id = Convert.ToInt32(drs[0]["FeeID"]);
            }
            FeeProject.Text = "";
            FeeProject.Properties.Items.Clear();
            string sqlstr = "ParentID=" + id;
            DataRow[] drs1 = dsFeeType.Tables[0].Select(sqlstr);
            if (drs1.Length > 0)
            {
                for (int i = 0; i < drs1.Length; i++)
                {
                    FeeProject.Properties.Items.Add(drs1[i]["TypeName"].ToString());
                }
            }
        }

        private void FeeProject_Properties_Click(object sender, EventArgs e)
        {
            string feeType = FeeType.Text.Trim();
            if (string.IsNullOrEmpty(feeType))
            {
                MsgBox.ShowOK("请先选择科目类型!");
                return;
            }
        }

        private void FeeProject_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AreaName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(ApplyDept, CauseName.Text, AreaName.Text);
        }



    }
}

