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
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace ZQTMS.UI.流程监控
{
    public partial class frmTransferWhite : ZQTMS.Tool.BaseForm
    {
        public frmTransferWhite()
        {
            InitializeComponent();
        }
        private static string AccountNO = "";
        private void cbRetrieve_Click(object sender, EventArgs e)
        {  try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("AccountName", AccountName.Text.Trim() == "全部" ? "%%" : AccountName.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_TransferWhite", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        


        }

        private void cbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransferWhite_Load(object sender, EventArgs e)
        {

            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            CommonClass.InsertLog("转账充值白名单"); //20190606

            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            CommonClass.SetWeb(AccountName, true);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_RechargeAccount");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds != null)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AccountNO = ds.Tables[0].Rows[i]["AccountNO"].ToString();
                        AccountName.Text = ds.Tables[0].Rows[i]["AccountName"].ToString();
                    }
                }
                else
                {
                    ds = new DataSet();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }

        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmAddWhiteList fwl = new frmAddWhiteList();
            fwl.AccountName = AccountName.Text;
            fwl.AccountNO = AccountNO;
            fwl.ShowDialog();
            if (fwl.DialogResult == DialogResult.OK)
            {
                cbRetrieve_Click(sender, e);
            }

        }
        
      

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "转账充值白名单");
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem19_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            if (myGridView1.FocusedRowHandle < 0) return;
            string rbankaccount = myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "rbankaccount").ToString();
            string url = "http://ZQTMS.dekuncn.com:8079/UnionDelTransferWhiteList";
            //string url = "http://127.0.0.1:8080/ZQTMSUnionPay/UnionDelTransferWhiteList";
            Dictionary<string, object> dics = new Dictionary<string, object>();
            dics.Add("rbankaccount", rbankaccount);
            //JavaScriptSerializer jsonSer = new JavaScriptSerializer();//把这个换成jsonconvert是dll引用会报错，版本冲突gy
            //string data = jsonSer.Serialize(dics);
            string data = JsonConvert.SerializeObject(dics);
            string responseString = HttpHelper.HttpPost(url, System.Web.HttpUtility.UrlEncode(data, System.Text.Encoding.UTF8), Encoding.UTF8);
            JObject jo = (JObject)JsonConvert.DeserializeObject(responseString);
            if (jo["response"].ToString().Equals("0000"))
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("rcompanycode", ""));
                list.Add(new SqlPara("rcompanyname", ""));
                list.Add(new SqlPara("raccountname", ""));
                list.Add(new SqlPara("rbankaccount", rbankaccount));
                list.Add(new SqlPara("updateman", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("perflag", 1));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_UPD_TransferWhite", list)) > 0)
                {
                    MsgBox.ShowOK("删除成功!");
                    cbRetrieve_Click(sender, e);
                }
            }
            else
            {
                MsgBox.ShowOK(jo["message"].ToString());
            }

        }

     
    }
}
