using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmSendToSiteVehicleStart : BaseForm
    {
        public frmSendToSiteVehicleStart()
        {
            InitializeComponent();
        }
        public string sendToSite { get; set; }
        public string sendToWeb { get; set; }
        public string batchNo { get; set; }

        private void frmSendToSiteVehicleStart_Load(object sender, EventArgs e)
        {
           predictDate.DateTime = CommonClass.gcdate;
            lastDate.DateTime = CommonClass.gcdate;
            operDate.DateTime = CommonClass.gcdate;
            lblBatch.Text= batchNo;//送货批次号
            lblSite.Text = sendToSite;//转送到站点
            lblWeb.Text= sendToWeb;//转送到网点
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
               // list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("batchNo", batchNo));
                list.Add(new SqlPara("sendToSite", sendToSite));
                list.Add(new SqlPara("sendToWeb", sendToWeb));
                list.Add(new SqlPara("predictDate", predictDate.DateTime));
                list.Add(new SqlPara("lastDate", lastDate.DateTime));
                list.Add(new SqlPara("operDate", operDate.DateTime));
                if (MsgBox.ShowYesNo("确定要点击完成？") != DialogResult.Yes) return;
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_SendToSiteBillVehicleStart_KT", list)) > 0)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
