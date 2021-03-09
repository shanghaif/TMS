using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;

namespace ZQTMS.UI
{
    public partial class JMfrmReceiptCancel : BaseForm
    {
        public JMfrmReceiptCancel()
        {
            InitializeComponent();
        }

        string MenuType = "";
        public JMfrmReceiptCancel(string _MenuType)
        {
            InitializeComponent();
            MenuType = _MenuType;
            if (MenuType.Contains("CancelSend"))
            {
                label1.Text = "取消寄出：";
                button2.Text = "取消寄出";
            }
            else if (MenuType.Contains("CancelBack"))
            {
                label1.Text = "取消返回：";
                button2.Text = "取消返回";
            }
            else
            {
                label1.Text = "取消返厂：";
                button2.Text = "取消返厂";
            }
        }

        private void frmReceiptCancel_Load(object sender, EventArgs e)
        {
            string[] SelectList = CommonClass.Arg.ReceiptCancelSelectType.Split(',');
            if (SelectList.Length > 0)
            {
                for (int i = 0; i < SelectList.Length; i++)
                {
                    SelectType.Properties.Items.Add(SelectList[i]);
                }
                SelectType.SelectedIndex = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                CommonClass.ShowBillSearch(textBox1.Text);
            }
            else 
            {
                XtraMessageBox.Show("运单号不可为空，请确认！");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                bool CanCancel = false;
                if (MenuType.Contains("CancelSend"))
                {
                    if (OrderReceiptState.Equals("回单寄出"))
                    {
                        CanCancel = true;
                    }
                    else 
                    {
                        CanCancel = false;
                    }
                }
                else if (MenuType.Contains("CancelBack"))
                {
                    if (OrderReceiptState.Equals("回单返回"))
                    {
                        CanCancel = true;
                    }
                    else
                    {
                        CanCancel = false;
                    }
                }
                else
                {
                    if (OrderReceiptState.Equals("回单返厂"))
                    {
                        CanCancel = true;
                    }
                    else
                    {
                        CanCancel = false;
                    }
                }

                if (CanCancel)
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("BillNo", textBox1.Text));
                    list.Add(new SqlPara("OperationType", MenuType));

                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_cancel_receipt", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        XtraMessageBox.Show("取消成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else 
                {
                    XtraMessageBox.Show("运单当前状态不支持该操作！");
                    return;
                }
            }
            else 
            {
                XtraMessageBox.Show("运单号不可为空，请确认！");
                return;
            }
        }

        private string OrderReceiptState = "";

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", textBox1.Text));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("您输入的运单号不存在，请确认！");
                    if (!textBox1.Focused)
                    {
                        textBox1.Focus();
                    }
                    return;
                }
                else 
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    OrderReceiptState = dr["ReceiptState"].ToString();
                }
            }
        }

    }
}
