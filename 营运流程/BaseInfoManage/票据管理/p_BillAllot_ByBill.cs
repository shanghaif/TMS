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

namespace ZQTMS.UI
{
    public partial class p_BillAllot_ByBill : BaseForm
    {
        public p_BillAllot_ByBill()
        {
            InitializeComponent();
        }

        private void AddBillOutForm_Load(object sender, EventArgs e)
        {
            try
            {
                BarMagagerOper.SetBarPropertity(bar1);
                //GetAllWebId();//获取所有网点
                CommonClass.SetWeb(cbwebid, "%%", false);
                List<string> listType = GetAllBillType();
                for (int i = 0; i < listType.Count; i++)
                {
                    if (!cobeBillType.Properties.Items.Contains(listType[i].ToString()))
                    {
                        cobeBillType.Properties.Items.Add(listType[i].ToString());
                    }
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

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            outbillSave();
        }
        #region Save
        private void outbillSave()
        {
            try
            {
                if (cobeBillType.Text.Trim() == "")
                {
                    MsgBox.ShowOK("请选择票据类型!");
                    cobeBillType.Focus();
                    return;
                }

                if (cbwebid.SelectedIndex < 0)
                {
                    MsgBox.ShowOK("请选择调拨网点!");
                    cbwebid.Focus();
                    return;
                }

                if (teoBBno.Text.Trim() == "")
                {
                    MsgBox.ShowOK("起始单号不能为空白!");
                    teoBBno.Focus();
                    return;
                }
                if (teoBEno.Text.Trim() == "")
                {
                    MsgBox.ShowOK("结束单号不能为空白!");
                    teoBEno.Focus();
                    return;
                }

                Int64 bno = 0, eno = 0;
                if (!Int64.TryParse(teoBBno.Text.Trim(), out bno) || bno <= 0)
                {
                    MsgBox.ShowOK("起始单号错误!");
                    teoBBno.Focus();
                    return;
                }

                if (!Int64.TryParse(teoBEno.Text.Trim(), out eno) || eno <= 0)
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

                string msg = string.Format("确定要调拨到：{0} ?\r\n本次调拨总票数：{1}\r\n调拨前，请确保输入的起止单号已存在，并且没有使用!", cbwebid.Text.Trim(), count);
                if (MsgBox.ShowYesNo(msg) == DialogResult.No) return;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("cbwebid", cbwebid.Text.Trim()));
                list.Add(new SqlPara("bno", bno));
                list.Add(new SqlPara("eno", eno));
                list.Add(new SqlPara("cobeBillType", cobeBillType.Text.Trim()));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_QSP_GET_p_bill_billnoInfo_tiaobo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) == 0)
                {
                    MsgBox.ShowOK("调拨失败：\r\n起止单号内的运单有些已经使用了，请检查!");
                    return;
                }
                else
                    MsgBox.ShowOK("调拨成功!");

                cbwebid.SelectedIndex = -1;
                teoBBno.Text = "";
                teoBEno.Text = "";
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }
        #endregion
        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void AddBillOutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                outbillSave();
            }
        }
    }
}
