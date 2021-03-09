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
    public partial class frmTypeAdd : BaseForm
    {
        public frmTypeAdd()
        {
            InitializeComponent();
        }
        public int type = 0;
        public string id = "", insideType="",one="",two="",three="",four="";

        private void frmTypeAdd_Load(object sender, EventArgs e)
        {
            if (type ==1)
            {
                InsideType.Text = insideType;
                One.Text = one;
                Two.Text = two;
                Three.Text = three;
                Four.Text = four;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (InsideType.Text.Trim() == "")
            {
                MsgBox.ShowOK("请输入内部往来类型");
                return;
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ID",id));
            list.Add(new SqlPara("InsideType", InsideType.Text.Trim()));
            list.Add(new SqlPara("One", One.Text.Trim()));
            list.Add(new SqlPara("Two", Two.Text.Trim()));
            list.Add(new SqlPara("Three", Three.Text.Trim()));
            list.Add(new SqlPara("Four", Four.Text.Trim()));
            list.Add(new SqlPara("Type", type));
            SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_InternalType", list);
            if (SqlHelper.ExecteNonQuery(spe) > 0)
            {
                MsgBox.ShowOK();
                this.Close();
            }
        }

    }
}