using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using DevExpress.XtraReports.UI;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace ZQTMS.UI
{
    public partial class frmBillOrderXCXUpDate : BaseForm
    {
        public frmBillOrderXCXUpDate()
        {
            InitializeComponent();
        }
        private string url = "";
        public DataRow dr = null;
        private void BespeakSendGoods_Load(object sender, EventArgs e)
        {
            if (dr != null)
            {
               orderSn.Text=dr["orderSn"].ToString();
               num.Text = dr["qty"].ToString();
               Weight.Text = dr["Weight"].ToString();
               Volume.Text = dr["Volume"].ToString();
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("orderSn", dr["orderSn"].ToString()));
                list.Add(new SqlPara("num", num.Text));
                list.Add(new SqlPara("Weight", Weight.Text));
                list.Add(new SqlPara("Volume", Volume.Text));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Update_SmallProgramIDOrder", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    Close();
                }
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }


       

    }
}
