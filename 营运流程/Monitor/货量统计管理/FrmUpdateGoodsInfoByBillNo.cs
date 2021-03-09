using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class FrmUpdateGoodsInfoByBillNo : BaseForm
    {
        public string billNo = "", operMan = "", carLoding = "",operType="",operWeb="";
        public FrmUpdateGoodsInfoByBillNo()
        {
            InitializeComponent();
        }
        public FrmUpdateGoodsInfoByBillNo(string billno, string operman, string carloding,string opertype,string operweb)
        {
            this.billNo = billno;
            this.operMan = operman;
            this.carLoding = carloding;
            this.operType = opertype;
            this.operWeb = operweb;
        }
        private void btnSure_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNewer.Text.Trim()))
                {
                    MsgBox.ShowOK("新操作人不能为空！");
                    return;
                }
                if (string.IsNullOrEmpty(txtCarLode.Text.Trim()))
                {
                    MsgBox.ShowOK("车位不能为空！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", LBillNo.Text.Trim()));
                list.Add(new SqlPara("older", Lolder.Text.Trim()));
                list.Add(new SqlPara("newer", txtNewer.Text.Trim()));
                list.Add(new SqlPara("carLoding", txtCarLode.Text.Trim()));
                list.Add(new SqlPara("operType", operType));
                list.Add(new SqlPara("operWeb", operWeb));
                if (MsgBox.ShowYesNo("确定要修改运单号为：" + "【" + LBillNo.Text.Trim() + "】" + "的信息？") == DialogResult.No) return;
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Update_GoodsCountByBillNo", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    this.Close();
                }
                else
                {
                    MsgBox.ShowOK("修改出现错误,请重试！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void FrmUpdateGoodsInfoByBillNo_Load(object sender, EventArgs e)
        {
            LBillNo.Text = billNo;
            Lolder.Text = operMan;
            txtCarLode.Text = carLoding;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}