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

namespace ZQTMS.UI
{
    public partial class frmShowBigImg : BaseForm
    {
        public frmShowBigImg()
        {
            InitializeComponent();
        }
        public string url = "";
        public int isok = 0;
        public GoodsImage gi = null;

        private void frmShowBigImg_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromStream(System.Net.WebRequest.Create(url).GetResponse().GetResponseStream());
            this.Width = pictureBox1.Width + 4;
            this.Height = pictureBox1.Height + panel1.Height + 4;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string billNo = BillNo.Text.Trim().TrimEnd(',');
            if (billNo == "")
            {
                MsgBox.ShowOK("请输入认领运单号!");
                return;
            }
            if (MsgBox.ShowYesNo("确定认领？") != DialogResult.Yes) return;
            billNo += ",";
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Id", gi.GoodsId));
            list.Add(new SqlPara("BillNo", billNo));
            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_FindGoods_Claim", list)) == 0) return;

            if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_FindGoods_Claim", list)) == 0) return; //maohui20180719
            MsgBox.ShowOK("认领成功！");
            isok = 1;
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BillNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) simpleButton1.PerformClick();
        }
    }
}