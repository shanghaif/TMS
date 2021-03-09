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

namespace ZQTMS.UI
{
    public partial class FrmDeleteByBillNoOrBatchNo : BaseForm
    {
        public int flag = 0;
        public string operWebName = "";
        public FrmDeleteByBillNoOrBatchNo()
        {
            InitializeComponent();
        }
        public FrmDeleteByBillNoOrBatchNo(string webname)
        {
            this.operWebName = webname;
        }

        private void FrmDeleteByBillNoOrBatchNo_Load(object sender, EventArgs e)
        {
            FrmDeleteByBillNoOrBatchNo frm = new FrmDeleteByBillNoOrBatchNo();
            switch (flag)
            {
                case 1:
                    Text = "按批次号删除";
                    label1.Text = "批次号";
                    break;
                case 2:
                    Text = "按单号删除";
                    label1.Text = "单  号";
                    break;
                default: break;
            }
        }

        public void DeleteByBatchNo()
        {
            try
            {
                if (string.IsNullOrEmpty(txtNo.Text))
                {
                    MsgBox.ShowOK("请填写批次号！");
                    return;
                }
                if (string.IsNullOrEmpty(txtOperType.Text))
                {
                    MsgBox.ShowOK("请选择操作类型！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要删除批次号为：" + "【" + txtNo.Text + "】" + "的数据？") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("batchNo", txtNo.Text));
                list.Add(new SqlPara("operType", txtOperType.Text));
                list.Add(new SqlPara("operWeb", operWebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_GoodsCountByBatchNo", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        public void DeleteByBillNo() {
            try
            {
                if (string.IsNullOrEmpty(txtNo.Text))
                {
                    MsgBox.ShowOK("请填写单号！");
                    return;
                }
                if (string.IsNullOrEmpty(txtOperType.Text))
                {
                    MsgBox.ShowOK("请选择操作类型！");
                    return;
                }
                if (MsgBox.ShowYesNo("确定要删除单号为：" + "【" + txtNo.Text + "】" + "的数据？") != DialogResult.Yes) return;
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("billNo", txtNo.Text));
                list.Add(new SqlPara("operType", txtOperType.Text));
                list.Add(new SqlPara("operWeb", operWebName));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_DELETE_GoodsCountByBillNo", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            switch (flag) {
                case 1: DeleteByBatchNo(); break;
                case 2: DeleteByBillNo(); break;
            }
        }
        
    }
}