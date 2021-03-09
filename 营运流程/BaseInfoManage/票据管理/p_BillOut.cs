using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class p_BillOut : BaseForm
    {
        public p_BillOut()
        {
            InitializeComponent();
        }

        public int type = 1;//1新增 2修改

        //供修改使用
        public string flag = "";
        public string bno = "", eno = "", web = "", lingdan = "", bill = "";

        private void AddBillOutForm_Load(object sender, EventArgs e)
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
            try
            {
                BarMagagerOper.SetBarPropertity(bar1);
                cobeBillType.SelectedValueChanged -= new EventHandler(cobeBillType_SelectedValueChanged);
                if (type != null)
                {

                    GetAllWebId();
                    List<string> listType = GetAllBillType();
                    for (int i = 0; i < listType.Count; i++)
                    {
                        if (!cobeBillType.Properties.Items.Contains(listType[i].ToString()))
                        {
                            cobeBillType.Properties.Items.Add(listType[i].ToString());
                        }
                    }

                    if (type == 1)
                    {
                        this.Text = "票据出库";
                        this.groupBox1.Text = "票据出库";
                        this.teoRecord.Text = CommonClass.UserInfo.UserName;
                        this.teoBatch.Text = GetOBatch();
                        //this.teoBBno.Text = GetBillNo(this.cobeBillType.Text.Trim());
                        //this.teoBEno.Text = GetBillNo(this.cobeBillType.Text.Trim());
                        this.teoTime.DateTime = CommonClass.ServerDate;
                    }
                    else
                    {
                        this.Text = "票据出库修改";
                        foreach (Control Con in groupBox1.Controls)
                        {
                            if (Con.GetType() != typeof(DevExpress.XtraEditors.LabelControl))
                            {
                                Con.Enabled = false;
                            }
                        }
                        cbwebid.Enabled = true;
                        cblingdanren.Enabled = true;
                        teoBatch.Text = flag;
                        teoBBno.Text = bno;
                        teoBEno.Text = eno;
                        cbwebid.Text = web;
                        cblingdanren.Text = lingdan;
                        cobeBillType.Text = bill;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cobeBillType.SelectedValueChanged += new EventHandler(cobeBillType_SelectedValueChanged);
            }
        }

        public void GetAllWebId()
        {
            try
            {
                if (CommonClass.dsWeb.Tables.Count == 0) return;
                cbwebid.Properties.Items.Clear();
                for (int i = 0; i < CommonClass.dsWeb.Tables[0].Rows.Count; i++)
                {
                    cbwebid.Properties.Items.Add(CommonClass.dsWeb.Tables[0].Rows[i]["WebName"]);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public List<string> GetAllBillType()
        {
            List<string> listTypes = new List<string>();
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OUT_ALLBillType");
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return listTypes;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    listTypes.Add(dr[0].ToString());
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return listTypes;
        }

        public string GetOBatch()
        {
            string batch = null;
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_OBatchNum");
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return batch;
                batch = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return batch;
        }

        /// <summary>
        /// 库存票数
        /// </summary>
        /// <param name="billtype"></param>
        /// <returns></returns>
        public int GetBillNo(string billtype, ref string billno)
        {
            int count = 0;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillType", billtype));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_OUT_BillNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return count;
                count = Convert.ToInt32(ds.Tables[0].Rows[0]["num"]);
                billno = ds.Tables[0].Rows[0]["billno"] == DBNull.Value ? "" : ds.Tables[0].Rows[0]["billno"].ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return count;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (type == 2)
            {
                Modify();
            }
            else
            {
                Add();
            }
        }
        #region Save
        private void Add()
        {
            try
            {
                string oBatch = teoBatch.Text.Trim();
                string oBBno = teoBBno.Text.Trim();
                string oBEno = teoBEno.Text.Trim();
                int oBundle = Convert.ToInt32(teoBundle.Text.Trim());
                string oCreateBy = teoCreateBy.Text.Trim();
                DateTime oTime = Convert.ToDateTime(teoTime.Text.Trim());
                string oRecord = teoRecord.Text.Trim();
                string oRemark = teoRemark.Text.Trim();
                string billtype = cobeBillType.Text.Trim();
                string webid = cbwebid.Text.Trim();
                string lingdanren = cblingdanren.Text.Trim();

                if (billtype == "")
                {
                    XtraMessageBox.Show("请选择票据类型!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cobeBillType.Focus();
                    return;
                }
                if (webid == "")
                {
                    XtraMessageBox.Show("请输入站点!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (oBBno == "")
                {
                    MsgBox.ShowOK("起始单号不能为空白!");
                    teoBBno.Focus();
                    return;
                }
                if (oBEno == "")
                {
                    MsgBox.ShowOK("结束单号不能为空白!");
                    teoBEno.Focus();
                    return;
                }
                if (lingdanren == "")
                {
                    XtraMessageBox.Show("请输入领单人!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Int64 bno = 0, eno = 0;
                if (!Int64.TryParse(oBBno, out bno) || bno <= 0)
                {
                    MsgBox.ShowOK("起始单号错误!");
                    teoBBno.Focus();
                    return;
                }

                if (!Int64.TryParse(oBEno, out eno) || eno <= 0)
                {
                    MsgBox.ShowOK("截止单号错误!");
                    teoBEno.Focus();
                    return;
                }

                if (bno.ToString().Length != eno.ToString().Length)
                {
                    MsgBox.ShowOK("单号长度不一致,请检查!");
                    teoBBno.Focus();
                    return;
                }

                if (bno > eno)
                {
                    MsgBox.ShowOK("截止单号不能小于起始单号,请检查!");
                    teoBBno.Focus();
                    return;
                }
                Int64 count = eno - bno + 1;

                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = null;
                try
                {

                    list.Add(new SqlPara("bno", bno));
                    list.Add(new SqlPara("eno", eno));
                    list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
                    sps = new SqlParasEntity(OperType.Query, "P_QSP_CHECK_STATE", list);
                    DataSet ds = SqlHelper.GetDataSet(sps);
                    int state = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    if (state == 1) //起止单号内有使用过
                    {
                        XtraMessageBox.Show("您填写的起止单号中只有 " + ds.Tables[0].Rows[0][1].ToString() + " 张可出库(其他已出库),或者当前库存不足,请检查!\r\n\r\n温馨提示：您可以在“票据使用明细”中查询起止单状态!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("检测库存失败!\r\n原因：" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                list = new List<SqlPara>();
                list.Add(new SqlPara("Batch", oBatch));
                list.Add(new SqlPara("BillType", billtype));
                list.Add(new SqlPara("BBno", bno));
                list.Add(new SqlPara("BEno", eno));
                list.Add(new SqlPara("Bundle", oBundle));
                list.Add(new SqlPara("CreateBy", oCreateBy));
                list.Add(new SqlPara("Time", oTime));
                list.Add(new SqlPara("Record", oRecord));
                list.Add(new SqlPara("Remark", oRemark));
                list.Add(new SqlPara("webid", webid));
                list.Add(new SqlPara("lingdanren", lingdanren));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                sps = new SqlParasEntity(OperType.Query, "P_USP_OUT_Bill", list);
                int row = int.Parse(SqlHelper.GetDataSet(sps).Tables[0].Rows[0][0].ToString());

                if (row == 0)
                {
                    XtraMessageBox.Show("操作成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    string msg = String.Format("操作失败！库存只剩下{0}张票据", row);
                    XtraMessageBox.Show(msg, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("填写有误！\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void Modify()
        {
            try
            {
                string oBatch = teoBatch.Text.Trim();
                string webid = cbwebid.Text.Trim();
                string lingdanren = cblingdanren.Text.Trim();

                if (oBatch == "")
                {
                    XtraMessageBox.Show("没有出库批次!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    teoBatch.Focus();
                    return;
                }
                if (webid == "")
                {
                    XtraMessageBox.Show("请选择拨入网点!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //if (lingdanren == "")
                //{
                //    XtraMessageBox.Show("请输入领单人!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Batch", oBatch));
                list.Add(new SqlPara("webid", cbwebid.Text.Trim()));
                list.Add(new SqlPara("lingdanren", cblingdanren.Text.Trim()));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "P_USP_MODIFY_PIAOJU_OUT", list);
                int result = SqlHelper.ExecteNonQuery(sps);
                if (result > 0)
                {
                    MsgBox.ShowOK("修改成功!");
                    web = cbwebid.Text.Trim();
                    lingdan = cblingdanren.Text.Trim();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("填写有误！\r\n" + ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }
        #endregion
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void teCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #region 判断出库起始单号是否存在
        public bool BillIsExists(int billno, string billtype)
        {
            bool flag = false;
            try
            {
                string i = string.Empty;
                string o = string.Empty;



                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bill", billno));
                list.Add(new SqlPara("type", billtype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_OBillIsExists", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                i = ds.Tables[0].Rows[0][0].ToString();
                o = ds.Tables[0].Rows[0][1].ToString();


                if (i == "0" && o == "0")
                {
                    XtraMessageBox.Show("起始单号不存在,请确认！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    flag = false;
                }
                if (i != "0" && o != "0")
                {
                    XtraMessageBox.Show("起始单号已使用,请确认！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    flag = false;
                }
                if (i != "0" && o == "0")
                {
                    flag = true;
                }

            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;

        }
        #endregion

        #region 根据类型取到最大单号
        public string GetMaxBillNoByTpye(string billtype)
        {
            string bno = null;

            try
            {

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillType", billtype));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_MaxTreasuryId", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                bno = ds.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return bno;
        }
        #endregion

        private void cobeBillType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cobeBillType.Text == "") return;
                string billtype = cobeBillType.Text.Trim();
                string billno = "";
                int count = GetBillNo(billtype, ref billno);
                if (count == 0)
                {
                    XtraMessageBox.Show("该类型库存不足！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.cobeBillType.Text = "";
                    return;
                }
                teoBBno.Text = billno;//库存中的最小单号
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void teCount_EditValueChanged(object sender, EventArgs e)
        {
            Int64 count = 0;
            try
            {
                if (teCount.Text.Trim().Length == 0)
                    return;
                count = Convert.ToInt64(teCount.Text.Trim());
                if (count >= 0)
                {
                    Int64 bno = 0;
                    if (teoBBno.Text.Trim().Length == 0)
                        return;
                    bno = Convert.ToInt64(teoBBno.Text.Trim());
                    count = count + bno - 1;
                    teoBEno.Text = "" + count;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void AddBillOutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                if (type == 2)
                {
                    Modify();
                }
                else
                {
                    Add();
                }
            }
        }

        private void cbwebid_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cc.FillCreatebyBySiteAndWeb(cblingdanren, CommonClass.dsSite, cbwebid.Text.Trim(), false); 
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
