using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class ExpenseBankInfo : BaseForm
    {
        public ExpenseBankInfo()
        {
            InitializeComponent();
        }

        public int id = -1;
        DataSet dsarea = new DataSet();
        public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取
        public decimal money = 0;
        public string applyDept = "";//申报部门
      //public string projectType = "";
          public string feeType = "";
          public string applyCause = "";
          public string applyArea = "";
          public string aduitId = "";//从审核界面传过来

          public string pageType = "";

        public string Gid = "";
        public string remarkInfo = "";
        private void ExpenseBankInfo_Load(object sender, EventArgs e)
        {
            ck_DaiDiZhang.Checked = true;
            #region 区划

            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");

            #endregion
            if (Gid != "")
            {
                textwd.Text = applyDept;
                textproject.Text = feeType;
                edaccout.Text = money.ToString();
                edremark.Text = remarkInfo;
                SetControl();
               // txtBatch.Text = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                txtBatch.Text = GetMaxInOneVehicleFlag();
            }
            DataSet ds = new DataSet();
            if (aduitId != "")
            {
                try
                {
                    
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara ("ID",aduitId));
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseAudit_ById", list);
                     ds = SqlHelper.GetDataSet(sps);
                    if (ds == null || ds.Tables.Count < 0 || ds.Tables[0].Rows.Count < 0) return;
                    ck_DaiDiZhang.Checked = ds.Tables[0].Rows[0]["PayType"].ToString() == "抵账" ? true : false;
                    ck_DaiFuKuan.Checked = ds.Tables[0].Rows[0]["PayType"].ToString() == "付款" ? true : false;
                    edbankman.Text = ds.Tables[0].Rows[0]["BankMan"].ToString();
                    edbankcode.Text = ds.Tables[0].Rows[0]["BankAccount"].ToString();
                    edbankname.Text = ds.Tables[0].Rows[0]["BankName"].ToString();
                    edbankchild.Text = ds.Tables[0].Rows[0]["BankSubbranch"].ToString();
                    
                    edopertype.Text = ds.Tables[0].Rows[0]["TransferType"].ToString();
                    edaccout.Text = ds.Tables[0].Rows[0]["ApplyMoney"].ToString();
                    edouttype.Text = ds.Tables[0].Rows[0]["ExpendType"].ToString();
                    textwd.Text = ds.Tables[0].Rows[0]["ApplyDept"].ToString();
                    textproject.Text = ds.Tables[0].Rows[0]["FeeType"].ToString();
                    txtBatch.Text = ds.Tables[0].Rows[0]["Batch"].ToString();                    
                    edbilldate.EditValue = ds.Tables[0].Rows[0]["PayDate"].ToString();
                    edremark.Text = ds.Tables[0].Rows[0]["BankRemark"].ToString();
                    //edsheng.Text = ds.Tables[0].Rows[0]["BankProvince"].ToString();
                    //edcity.Text = ds.Tables[0].Rows[0]["BankCity"].ToString();
                    //edsheng.EditValue = ds.Tables[0].Rows[0]["BankProvince"].ToString();
                    //edcity.EditValue = ds.Tables[0].Rows[0]["BankCity"].ToString();
                    CommonClass.SetSelectIndex(ConvertType.ToString(ds.Tables[0].Rows[0]["BankProvince"].ToString()), edsheng);
                    CommonClass.SetSelectIndex(ConvertType.ToString(ds.Tables[0].Rows[0]["BankCity"].ToString()), edcity);

                    edbankman.Enabled = true;
                    edbankcode.Properties.ReadOnly = false;
                    edbankname.Properties.ReadOnly = false;
                    edbankchild.Properties.ReadOnly = false;
                    edsheng.Properties.ReadOnly = false;
                    edcity.Properties.ReadOnly = false;
                    edopertype.Properties.ReadOnly = false;
                    edouttype.Properties.ReadOnly = false;
                    edbilldate.Properties.ReadOnly = false;
                    gridControl1.Visible = false;

                    ck_DaiDiZhang.Enabled = false;
                    ck_DaiFuKuan.Enabled = false;

                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (id < 0)
            {
                //clear();
            }
            else
            {
                //fill();
            }
           
            edbilldate.DateTime = CommonClass.gcdate;
           
           
        }


        private void SetControl()
        {
            if (Gid != "")
            {
                if (ck_DaiDiZhang.Checked == true)
                {
                    edbankman.Enabled = false;
                    edbankcode.Properties.ReadOnly = true;
                    edbankname.Properties.ReadOnly = true;
                    edbankchild.Properties.ReadOnly = true;
                    edsheng.Properties.ReadOnly = true;
                    edcity.Properties.ReadOnly = true;
                    edopertype.Properties.ReadOnly = true;
                    edouttype.Properties.ReadOnly = true;
                    edbilldate.Properties.ReadOnly = true;
                    edbankman.Text = "";
                    edbankcode.Text = "";
                    edbankname.Text = "";
                    edbankchild.Text = "";
                    edsheng.Text = "";
                    edcity.Text = "";
                    edopertype.Text = "";
                    edouttype.Text = "";
                    edbilldate.Text = "";
                    edremark.Text = "";
                }
                else
                {
                    edbankman.Enabled = true;
                    edbankcode.Properties.ReadOnly = false;
                    edbankname.Properties.ReadOnly = false;
                    edbankchild.Properties.ReadOnly = false;
                    edsheng.Properties.ReadOnly = false;
                    edcity.Properties.ReadOnly = false;
                    edopertype.Properties.ReadOnly = false;
                    edouttype.Properties.ReadOnly = false;
                    edbilldate.Properties.ReadOnly = false;
                }
                if (ck_DaiFuKuan.Checked)
                {
                    edbankman.Enabled = true;
                    edbankcode.Properties.ReadOnly = false;
                    edbankname.Properties.ReadOnly = false;
                    edbankchild.Properties.ReadOnly = false;
                    edsheng.Properties.ReadOnly = false;
                    edcity.Properties.ReadOnly = false;
                    edopertype.Properties.ReadOnly = false;
                    edouttype.Properties.ReadOnly = false;
                    edbilldate.Properties.ReadOnly = false;
                }
                else
                {
                    edbankman.Enabled = false;
                    edbankcode.Properties.ReadOnly = true;
                    edbankname.Properties.ReadOnly = true;
                    edbankchild.Properties.ReadOnly = true;
                    edsheng.Properties.ReadOnly = true;
                    edcity.Properties.ReadOnly = true;
                    edopertype.Properties.ReadOnly = true;
                    edouttype.Properties.ReadOnly = true;
                    edbilldate.Properties.ReadOnly = true;
                    edbankman.Text = "";
                    edbankcode.Text = "";
                    edbankname.Text = "";
                    edbankchild.Text = "";
                    edsheng.Text = "";
                    edcity.Text = "";
                    edopertype.Text = "";
                    edouttype.Text = "";
                    edbilldate.Text = "";
                    edremark.Text = "";
                }
            }
        }

        private void fill()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANK_ByID", list);

                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds.Tables.Count == 0) return;
                if (ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                edbankman.EditValue = dr["bankman"];
                edbankcode.EditValue = dr["bankcode"];
                edbankname.EditValue = dr["bankname"];

                edbankchild.EditValue = dr["bankchild"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["sheng"].ToString().Trim(), edsheng);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["city"].ToString().Trim(), edcity);
                edopertype.EditValue = dr["opertype"];

                edaccout.EditValue = dr["accout"];
                edaccin.EditValue = dr["accin"];
                edbilldate.EditValue = dr["billdate"];

                edouttype.EditValue = dr["outtype"];
                edremark.EditValue = dr["remark"];

                textwd.EditValue = dr["reportnetwork"];
                textproject.EditValue = dr["project"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(ComboBoxEdit))
                {
                    item.Text = "";
                }
            }
            edbilldate.DateTime = CommonClass.gcdate;
            id = -1;
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                edcity.Properties.Items.Clear();
                edcity.Text = "";
                edcity.Tag = null;

                if (edsheng.Text.Trim() == "")
                {
                    edcity.Text = "";
                    return;
                }

                if (edsheng.SelectedIndex < 0) return;
                int id = Convert.ToInt32(((ArrayList)edsheng.Tag)[edsheng.SelectedIndex]);

                DataRow[] dr = dsarea.Tables[0].Select("parentid=" + id + "");
                ArrayList arr = new ArrayList();
                for (int i = 0; i < dr.Length; i++)
                {
                    edcity.Properties.Items.Add(dr[i]["city"]);
                    arr.Add(dr[i]["ID"]);
                }
                edcity.Tag = arr;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string bankman = edbankman.Text.Trim();
            string bankcode = edbankcode.Text.Trim();
            string bankname = edbankname.Text.Trim();
            string bankchild = edbankchild.Text.Trim();
            string sheng = edsheng.Text.Trim();
            string city = edcity.Text.Trim();
            string opertype = edopertype.Text.Trim();
            string outtype = edouttype.Text.Trim();
            decimal accout = 0;
            decimal.TryParse(edaccout.Text.Trim(), out accout);
            if (ck_DaiFuKuan.Checked)
            {
                if (bankman == "")
                {
                    MsgBox.ShowOK("必须填写开户姓名!");
                    edbankman.Focus();
                    return;
                }
                if (bankcode == "")
                {
                    MsgBox.ShowOK("必须填写银行账号!");
                    edbankcode.Focus();
                    return;
                }
                if (bankname == "")
                {
                    MsgBox.ShowOK("必须填写银行名称!");
                    edbankname.Focus();
                    return;
                }
                if (sheng == "")
                {
                    MsgBox.ShowOK("必须填写所属省份!");
                    edsheng.Focus();
                    return;
                }
                if (city == "")
                {
                    MsgBox.ShowOK("必须填写所属用户城市!");
                    edcity.Focus();
                    return;
                }
                if (opertype == "")
                {
                    MsgBox.ShowOK("必须填写转账类型!");
                    edopertype.Focus();
                    return;
                }
                
                if (accout > 0 && outtype == "")
                {
                    MsgBox.ShowOK("本单转出，必须填写转出类型!");
                    edouttype.Focus();
                    return;
                }
            }

            string reportnetwork = textwd.Text.Trim();
            if (reportnetwork=="")
            {
                MsgBox.ShowOK("必须填写申报部门!");
                textwd.Focus();
                return;
            }

            string project = textproject.Text.Trim();
            //if (project == "")
            //{
            //    MsgBox.ShowOK("必须填写项目!");
            //    textproject.Focus();
            //    return;
            //}

            

            decimal accin = 0;
            decimal.TryParse(edaccin.Text.Trim(), out accin);

            if (accout == 0 && accin == 0)
            {
                MsgBox.ShowOK("必须填写转账金额!");
                return;
            }

          
            DateTime billdate = edbilldate.DateTime;

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                if (pageType == "confirm")
                {

                    list.Add(new SqlPara("ID", Gid));
                }
                if (pageType == "audit")
                {
                    list.Add(new SqlPara("ID", aduitId));
                }

                list.Add(new SqlPara("BankMan", bankman));
                list.Add(new SqlPara("BankAccount", bankcode));
                list.Add(new SqlPara("BankName", bankname));

                list.Add(new SqlPara("BankProvince", sheng));  //4
                list.Add(new SqlPara("BankCity", city));
                list.Add(new SqlPara("TransferType", opertype));
                //list.Add(new SqlPara("accout", accout));

               // list.Add(new SqlPara("accin", accin));  //8
                list.Add(new SqlPara("PayDate", billdate));
                list.Add(new SqlPara("ExpendType", outtype));
                list.Add(new SqlPara("BankRemark", edremark.Text.Trim()));

                //list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));  //12
                //list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
               // list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));

                list.Add(new SqlPara("BankSubbranch", bankchild));
                if (pageType == "confirm")
                {
                    list.Add(new SqlPara("OperType", 1));
                }
                if (pageType == "audit")
                {
                    list.Add(new SqlPara("OperType", 3));
                }
                
                list.Add(new SqlPara("Batch",txtBatch.Text.Trim()));
                list.Add(new SqlPara("ApplyMoney", Convert.ToDecimal(edaccout.Text.Trim())));
                list.Add(new SqlPara("ApplyCause",applyCause));
                list.Add(new SqlPara("ApplyArea", applyArea));
                list.Add(new SqlPara("PayType", ck_DaiDiZhang.Checked?"抵账":"付款"));

                list.Add(new SqlPara("ApplyDept", reportnetwork));
                list.Add(new SqlPara("FeeType", project));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_Expensereimbursement_Confirm", list);

                edbankman.Text = "";
                edbankcode.Text = "";
                edbankname.Text = "";
                edbankchild.Text = "";
                edcity.Text = "";
                edsheng.Text = "";
                edopertype.Text = "";
                edaccout.Text = "";
                edouttype.Text = "";
                textwd.Text = "";
                textproject.Text = "";
                edremark.Text = "";
                edbilldate.Text = DateTime.Now.ToString();//显示当前操作时间
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK("成功!");
                }
                MsgBox.ShowOK("操作成功!");//数据库调试，受影响的行数>0 到这里显示结果显示为：0
                    edbankman.Focus();
                    this.Close();
                //Close();
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dsshipper.Tables.Count > 0)
            {
                timer1.Enabled = false;
                gridControl1.DataSource = dsshipper.Tables[0];
                //设置某列的宽度
            }
        }

        private void edbankman_Enter(object sender, EventArgs e)
        {
            if (edbankman.Text.Trim() == "")
            {
                gridView1.ClearColumnsFilter();
            }
            gridControl1.Left = edbankman.Left;
            gridControl1.Top = edbankman.Top + edbankman.Height + 2;
            gridControl1.Visible = true;
            gridControl1.BringToFront();
        }

        private void edbankman_Leave(object sender, EventArgs e)
        {
            if (!gridControl1.Focused)
            {
                gridControl1.Visible = false;
            }
        }


        public string GetMaxInOneVehicleFlag()
        {
            DataSet dsflag = new DataSet();
            try
            {
                List<SqlPara> list = new List<SqlPara>();


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ExpenseBatch", list);
                dsflag = SqlHelper.GetDataSet(sps);

                return ConvertType.ToString(dsflag.Tables[0].Rows[0][0]);
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK("产生发车批次失败：\r\n" + ex.Message);
                return "";
            }
        }

        private void edbankman_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void edbankman_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView1.ClearColumnsFilter();
                gridView1.Columns["bankman"].FilterInfo = new ColumnFilterInfo("[bankman] LIKE " + "'%" + e.NewValue.ToString() + "%'", "");
            }
            else
            {
                gridView1.ClearColumnsFilter();
            }
        }

        private void SetValue()
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            edbankman.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankman").ToString();
            edbankcode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankcode").ToString();
            edbankname.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankname").ToString();
            edbankchild.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankname");

            CommonClass.SetSelectIndex(ConvertType.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sheng")), edsheng);
            CommonClass.SetSelectIndex(ConvertType.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "city")), edcity);
            edopertype.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "opertype");

            edouttype.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "outtype");

            gridControl1.Visible = false;
            edbankcode.Focus();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetValue();
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue();
            }
            if (e.KeyCode == Keys.Up)
            {
                if (gridView1.FocusedRowHandle == 0)
                {
                    edbankman.Focus();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void w_bank_add_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void edbankname_Leave(object sender, EventArgs e)
        {
            edbankchild.Text = edbankname.Text;
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edcity, edsheng.EditValue);
        }

        private void edsheng_Enter(object sender, EventArgs e)
        {
            edsheng.ShowPopup();
        }

        private void edcity_Enter(object sender, EventArgs e)
        {

        }

        private void edopertype_Enter(object sender, EventArgs e)
        {

        }

        private void edouttype_Enter(object sender, EventArgs e)
        {
            edouttype.ShowPopup();
        }

        private void edbankname_Enter(object sender, EventArgs e)
        {
            edbankname.ShowPopup();
        }

        private void ck_DaiDiZhang_CheckedChanged(object sender, EventArgs e)
        {
            if (ck_DaiDiZhang.Checked)
            {
                ck_DaiFuKuan.Checked = false;

            }
            else
            {
                ck_DaiFuKuan.Checked = true;
            }
            SetControl();
        }

        private void ck_DaiFuKuan_CheckedChanged(object sender, EventArgs e)
        {
            if (ck_DaiFuKuan.Checked)
            {
                ck_DaiDiZhang.Checked = false;
                edremark.Text = remarkInfo;
            }
            else
            {
                ck_DaiDiZhang.Checked = true;
            }
            SetControl();
        }
    }
}