using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KMS.Tool;
using KMS.Common;
using KMS.SqlDAL;

namespace KMS.UI
{
    public partial class FrmhkzlADD : BaseForm
    {
        public string ID = "", hm = "", zh = "", khh = "", szs = "", szshi = "";
        public FrmhkzlADD()
        {
            InitializeComponent();
        }
        //退出
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //保存
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ID == "")
            {
                try
                {
                    if (LAB1.Text != "" && LAB2.Text != "" )
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("hm", LAB1.Text.Trim()));
                        list.Add(new SqlPara("zh", LAB2.Text.Trim()));
                        list.Add(new SqlPara("khh", LAB3.Text.Trim()));
                        list.Add(new SqlPara("szs", LAB4.Text.Trim()));
                        list.Add(new SqlPara("szshi", LAB5.Text.Trim()));
                        SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_INS_hkzl", list);
                        if (SqlHelper.ExecteNonQuery(spe) > 0)
                        {
                            MsgBox.ShowOK();
                            this.Close();
                        } 
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

            if (ID != "")
            {
                try
                {
                    if (LAB1.Text != "" && LAB2.Text != "" && LAB3.Text != "" && LAB4.Text != "" && LAB5.Text != "")
                    {
                        List<SqlPara> list = new List<SqlPara>();
                        list.Add(new SqlPara("hm", LAB1.Text.Trim()));
                        list.Add(new SqlPara("zh", LAB2.Text.Trim()));
                        list.Add(new SqlPara("khh", LAB3.Text.Trim()));
                        list.Add(new SqlPara("szs", LAB4.Text.Trim()));
                        list.Add(new SqlPara("szshi", LAB5.Text.Trim()));
                        list.Add(new SqlPara("ID", ID));
                        SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_hkzl", list);
                        if (SqlHelper.ExecteNonQuery(spe) > 0)
                        {
                            MsgBox.ShowOK();
                            this.Close();
                        }
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

        }
        private void FrmhkzlADD_Load(object sender, EventArgs e)
        {
            if (ID != "")
            {
                LAB1.Text = hm;
                LAB2.Text = zh;
                LAB3.Text = khh;
                LAB4.Text = szs;
                LAB5.Text = szshi;
            }
        }
    }
}
