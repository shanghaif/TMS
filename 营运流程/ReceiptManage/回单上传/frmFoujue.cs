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
    public partial class frmFoujue : BaseForm
    {
        public string BillNo = "";
        public int a = 0;
        public frmFoujue()
        {
            InitializeComponent();
        }

        private void frmFoujue_Load(object sender, EventArgs e)
        {

            BillStat.Text = BillNo;
            if (a == 1)
            {
                labelControl1.Text = "取消原因";
            }
            if (a == 2)
            {
                labelControl1.Text = "取消原因";
            }

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //否决
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (foujueyuanyin.Text == "")
            {
                MsgBox.ShowOK("原因不能为空！");
                return;
            }
            if (a == 1)
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("huidanqueren", "未审核"));
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("quxiaofoujue", foujueyuanyin.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "USP_UPDATE_TBFILEINFO_QX", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            if (a == 2)
            {
                List<SqlPara> list3 = new List<SqlPara>();
                list3.Add(new SqlPara("huidanqueren", "未审核"));
                list3.Add(new SqlPara("BillNo", BillNo));
                list3.Add(new SqlPara("quxiaoqueding", foujueyuanyin.Text.Trim()));
                SqlParasEntity spe3 = new SqlParasEntity(OperType.Execute, "USP_UPDATE_TBFILEINFO_QXQD", list3);
                if (SqlHelper.ExecteNonQuery(spe3) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
            if(a==3)
            {
                List<SqlPara> list2 = new List<SqlPara>();
                list2.Add(new SqlPara("huidanqueren", "否决"));
                list2.Add(new SqlPara("BillNo", BillNo));
                list2.Add(new SqlPara("foujueyuanyin", foujueyuanyin.Text.Trim()));
                SqlParasEntity spe2 = new SqlParasEntity(OperType.Execute, "USP_UPDATE_TBFILEINFO3", list2);
                if (SqlHelper.ExecteNonQuery(spe2) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
            }
        }

       
    }
}