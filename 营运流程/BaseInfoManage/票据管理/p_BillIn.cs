using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class p_BillIn : BaseForm
    {
        public p_BillIn()
        {
            InitializeComponent();
        }
        //commonclass cc = new commonclass();

        public delegate void deteype(string type);
        public event deteype type;

        private string t = "";
        public p_BillIn(string type)
        {
            InitializeComponent();
            t = type;
        }
        private void BillForm_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            if (Common.CommonClass.UserInfo.companyid != "101")
            {
                CompanyID.Enabled = false;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            else
            {
                CompanyID.Enabled = true;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            BarMagagerOper.SetBarPropertity(bar1);
            if (type != null)
            {
                //cobeBillType.Properties.Items.Clear();
                GetAllBillType();
                if (t == "add")
                {
                    Text = "票据录入";
                    groupBox1.Text = "票据录入";
                    teBatch.Text = GetBatch();
                    teRecord.Text = CommonClass.UserInfo.UserName;
                    //teBnum.Text = GetBillNo(rowhandlecobeBillType.Text.Trim());
                    //teEnum.Text = GetBillNo(rowhandlecobeBillType.Text.Trim());
                    deEntryTime.EditValue = CommonClass.gcdate;
                    dePtime.EditValue = CommonClass.gcdate;
                }
                else
                {
                    Text = "票据修改";
                    groupBox1.Text = "票据修改";
                    teBnum.Properties.ReadOnly = true;
                    teBatch.Properties.ReadOnly = true;
                    tePage.Properties.ReadOnly = true;
                    cobeBillType.Properties.ReadOnly = true;
                    Int64 tid = Convert.ToInt64(t);
                    teEnum.Properties.ReadOnly = true;

                    try
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("id", tid));

                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_TreasuryById", list);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                        DataRow dr = ds.Tables[0].Rows[0];
                        teBatch.Text = Convert.ToString(dr["tBatch"]);
                        teProcurement.Text = dr["tProcurement"].ToString();
                        //dePtime.Text = dr["tPtime"].ToString().Substring(0, dr["tPtime"].ToString().Length - 8);
                        dePtime.EditValue = dr["tPtime"].ToString();
                        tePrice.Text = dr["tPrice"].ToString();
                        teTotal.Text = Convert.ToString(dr["tTotal"]);
                        teCreateBy.Text = dr["tCreateBy"].ToString();
                        //deEntryTime.Text = Convert.ToString(dr["tEntryTime"]).Substring(0, dr["tEntryTime"].ToString().Length - 8);
                        deEntryTime.EditValue = dr["tEntryTime"].ToString();
                        teRecord.Text = dr["tRecord"].ToString();
                        teRemark.Text = dr["tRemark"].ToString();
                        teBnum.Text = Convert.ToString(dr["tBnum"]);
                        teEnum.Text = Convert.ToString(dr["tEnum"]);
                        teBundle.Text = Convert.ToString(dr["tBundle"]);
                        tePage.Text = Convert.ToString(dr["tPage"]);
                        cobeBillType.Text = Convert.ToString(dr["tbilltype"]);
                        //rowhandletePage.Text = (Convert.ToInt32(teEnum.Text) + Convert.ToInt32(teBnum.Text)).ToString();
                    }
                    catch (Exception ex)
                    {
                        MsgBox.ShowOK(ex.ToString());
                    }
                }
            }
        }

        public void GetAllBillType()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OUT_ALLBillType");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (!dr[0].ToString().Contains("托运单"))
                    {
                        cobeBillType.Properties.Items.Add(dr[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cobeBillType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                string billtype = this.cobeBillType.Text.Trim();
                if (type != null)
                {
                    if (t == "add")
                    {
                        string billtype1 = string.IsNullOrEmpty(GetBillNo(billtype)) ? "0" : GetBillNo(billtype);

                        Int64 num = Convert.ToInt64(billtype1) + 1;
                        this.teBnum.Text = num.ToString();
                        this.teEnum.Text = num.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public string GetBatch()
        {
            string batch = string.Empty;
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BatchNum");
                DataSet ds = SqlHelper.GetDataSet(sps);
                batch = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return batch;
        }

        private void tePrice_KeyPress(object sender, KeyPressEventArgs e)
        {   //只能输入数字带小数点
            //if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 46) && e.KeyChar != 8)
            //{
            //    e.Handled = true;
            //}
            if (!(Char.IsNumber(e.KeyChar) || e.KeyChar == '\b' && e.KeyChar != 8))
            {
                e.Handled = true;
            }

            if ((e.KeyChar >= 48 && e.KeyChar <= 57) || e.KeyChar == 8 || e.KeyChar == 46)
            {
                if (sender != null && sender is TextBox && e.KeyChar == 46)
                {
                    if (((TextBox)sender).Text.IndexOf(".") >= 0)
                        e.Handled = true;
                    else
                        e.Handled = false;
                }
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void tePage_KeyPress(object sender, KeyPressEventArgs e)
        {   //只能输入数字
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            //if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        #region 判断起始单号是否已存在
        public bool BillIsExists(Int64 billno, string billtype)
        {
            bool flag = true;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bill", billno));
                list.Add(new SqlPara("type", billtype));
                //list.Add(new SqlPara("companyid1", companyid1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_BillIsExists", list);
                SqlHelper.GetDataSet(sps).Tables[0].Rows[0][0].ToString();

            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public bool BillIsExists1(Int64 billno, string billtype)
        {
            bool flag = true;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bill", billno));
                list.Add(new SqlPara("type", billtype));
                //list.Add(new SqlPara("companyid1", companyid1));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_BillIsExists1", list);
                SqlHelper.GetDataSet(sps).Tables[0].Rows[0][0].ToString();

            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;

        }
        #endregion

        public string GetBillNo(string billtype)
        {
            string bno = string.Empty;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billType", billtype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return bno;

                bno = SqlHelper.GetDataSet(sps).Tables[0].Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return bno;
        }

        //保存
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveBill();
        }

        private void SaveBill()
        {
            string batch = GetBatch();
            string billtype = this.cobeBillType.Text.Trim();//票据类型
            string procurement = teProcurement.Text.Trim();
            string time = ConvertType.ToString(dePtime.EditValue);
            double price = ConvertType.ToDouble(tePrice.Text.Trim());
            double total = ConvertType.ToDouble(teTotal.Text.Trim());
            string createby = teCreateBy.Text.Trim();
            string entrytime = ConvertType.ToString(deEntryTime.EditValue);//起始运单号
            string record = teRecord.Text.Trim();
            string remark = teRemark.Text.Trim();
            string bnum = ConvertType.ToString(teBnum.Text.Trim());
            string endnum = ConvertType.ToString(teEnum.Text.Trim());
            int bundle = ConvertType.ToInt32(teBundle.Text.Trim());
            int page = ConvertType.ToInt32(tePage.Text.Trim());
            string companyid1 = this.CompanyID.Text.Trim();//hj

            if (bnum.Length == 0)
            {
                XtraMessageBox.Show("请输入起始运单号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cobeBillType.Text.Trim() != "托运单")
            {
                XtraMessageBox.Show("票据类型输入有误!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (t == "add")
            {
                if (BillIsExists(Convert.ToInt64(bnum), billtype))
                {
                    XtraMessageBox.Show("起始运单号已存在,请确认!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (BillIsExists(Convert.ToInt64(endnum), billtype))
                {
                    XtraMessageBox.Show("起截止单号已存在,请确认!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (BillIsExists1(Convert.ToInt64(bnum), billtype))
                {
                    XtraMessageBox.Show("起始运单号已存在,请确认!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (BillIsExists1(Convert.ToInt64(endnum), billtype))
                {
                    XtraMessageBox.Show("起截止单号已存在,请确认!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }
            if (Convert.ToInt64(bnum) > Convert.ToInt64(endnum))
            {
                XtraMessageBox.Show("请认真检查您输入的单号!\r\n截止单号必须大于起始单号!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cobeBillType.Text.Trim() == "")
            {
                XtraMessageBox.Show("请填写您的单据类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (t == "add")
                {
                    #region 新增


                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("procurement", procurement));
                    list.Add(new SqlPara("batch", batch));
                    list.Add(new SqlPara("billtype", billtype));
                    list.Add(new SqlPara("time", time));
                    list.Add(new SqlPara("total", total));
                    list.Add(new SqlPara("price", price));
                    list.Add(new SqlPara("createby", createby));
                    list.Add(new SqlPara("entrytime", entrytime));
                    list.Add(new SqlPara("record", record));
                    list.Add(new SqlPara("remark", remark));
                    list.Add(new SqlPara("bnum", Convert.ToInt64(bnum)));
                    list.Add(new SqlPara("enum", Convert.ToInt64(endnum)));
                    list.Add(new SqlPara("bundle", bundle));
                    list.Add(new SqlPara("page", page));
                    list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_ADD_Treasury", list);

                    int row = SqlHelper.ExecteNonQuery(sps);
                    if (row > 0)
                    {
                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    #endregion
                }
                else
                {
                    #region 修改


                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("id", Convert.ToInt64(t)));
                    list.Add(new SqlPara("procurement", procurement));
                    list.Add(new SqlPara("batch", batch));
                    list.Add(new SqlPara("tBillType", billtype));
                    list.Add(new SqlPara("time", time));
                    list.Add(new SqlPara("total", total));
                    list.Add(new SqlPara("price", price));
                    list.Add(new SqlPara("createby", createby));
                    list.Add(new SqlPara("entrytime", entrytime));
                    list.Add(new SqlPara("record", record));
                    list.Add(new SqlPara("remark", remark));
                    list.Add(new SqlPara("bnum", bnum));
                    list.Add(new SqlPara("enum", endnum));
                    list.Add(new SqlPara("bundle", bundle));
                    list.Add(new SqlPara("page", page));
                    list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_UPDATE_TreasuryById", list);

                    int row = SqlHelper.ExecteNonQuery(sps);

                    if (row > 0)
                    {
                        XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tePrice_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                teTotal.Text = (Convert.ToDecimal(tePrice.Text.Trim()) * Convert.ToDecimal(teBundle.Text.Trim())).ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void teBundle_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                teTotal.Text = (Convert.ToDecimal(tePrice.Text.Trim()) * Convert.ToDecimal(teBundle.Text.Trim())).ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tePage_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                string page = tePage.Text.Trim();
                if (type != null)
                {
                    if (t == "add")
                    {
                        if (page.Length == 0)
                            return;
                        if (Convert.ToInt64(page) > 0)
                        {
                            Int64 count = 0;
                            string bnum = teBnum.Text.Trim();
                            if (bnum.Length == 0)
                                bnum = "0";
                            Int64 beginNO = Convert.ToInt64(bnum);
                            count = Convert.ToInt64(page) + beginNO - 1;
                            teEnum.EditValue = count;
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BillForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                SaveBill();
            }
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
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}