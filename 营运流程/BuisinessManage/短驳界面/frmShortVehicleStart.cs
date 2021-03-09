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
using Newtonsoft.Json;
namespace ZQTMS.UI
{
    public partial class frmShortVehicleStart : BaseForm
    {
        public string batchNo { get; set; }
        public string SCDesSite { get; set; }
        public string SCDesWeb { get; set; }
        public string systemType = "";
        public string shortType = "";
        public frmShortVehicleStart()
        {
            InitializeComponent();
        }

        private void frmShortVehicleStart_Load(object sender, EventArgs e)
        {
            dateEdit1.DateTime = CommonClass.gcdate;
            dateEdit2.DateTime = CommonClass.gcdate;
            dateEdit3.DateTime = CommonClass.gcdate;
            label9.Text = batchNo;//短驳批次号
            label6.Text = SCDesSite;//短驳目的站点
            label7.Text = SCDesWeb;//短驳目的网点

            if (CommonClass.UserInfo.DepartName != "品质管理部")
            {
                dateEdit3.Enabled = false;
            }
        }

        //完成
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            try
            {
                list.Add(new SqlPara("ID", Guid.NewGuid()));
                list.Add(new SqlPara("batchNo", batchNo));
                list.Add(new SqlPara("scDesSite", SCDesSite));
                list.Add(new SqlPara("scDesWeb", SCDesWeb));
                list.Add(new SqlPara("dateEdit1", dateEdit1.DateTime));
                list.Add(new SqlPara("dateEdit2", dateEdit2.DateTime));
                list.Add(new SqlPara("dateEdit3", dateEdit3.DateTime));
                string json = CreateJson.GetShortJson(list, 6, "USP_ADD_ShortBILLVEHICLESTAR_ZQTMS", "", batchNo, CommonClass.UserInfo.companyid);
                string url = HttpHelper.urlLMSSysExecuteZQTMSCurrency;
                list.Add(new SqlPara("Json", json));
                list.Add(new SqlPara("URL", url));
                //list.Add(new SqlPara("ProcName", "USP_ADD_ShortBILLVEHICLESTAR_ZQTMS"));

                if (MsgBox.ShowYesNo("确定要点击完成？") != DialogResult.Yes) return;
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_ShortBILLVEHICLESTAR_KT", list)) > 0)
                {
                    MsgBox.ShowOK();

                    #region //跟踪节点信息同步接口 (发车)
                    {
                        List<SqlPara> list1 = new List<SqlPara>();
                        list1.Add(new SqlPara("batchNo", batchNo));
                        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_ADD_ShortBILLVEHICLESTAR_KT_BillNo", list1);
                        DataSet ds = SqlHelper.GetDataSet(sps);
                        DataRow dr = ds.Tables[0].Rows[0];

                        List<SqlPara> lists = new List<SqlPara>();
                        lists.Add(new SqlPara("BillNo", dr["BillNo"]));
                        SqlParasEntity spsa = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ByID_Auto_BillNo1", lists);
                        DataSet dds = SqlHelper.GetDataSet(spsa);
                        DataRow ddr = dds.Tables[0].Rows[0];

                        if (ddr["BegWeb"].ToString() == "三方")
                        {
                            Dictionary<string, object> hashMap1 = new Dictionary<string, object>();
                            hashMap1.Add("carriageSn", ddr["BillNo"].ToString());
                            hashMap1.Add("orderStatusCode", Convert.ToInt32(2020));
                            hashMap1.Add("traceRemarks", "干线运输中");
                            string json1 = JsonConvert.SerializeObject(hashMap1);
                            string url1 = "http://120.76.141.227:9882/umsv2.biz/open/track/record_trunk_order_status";
                            try
                            {
                                HttpHelper.HttpPostJava(json1, url1);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                    }
                    #endregion

                    if (shortType == "1")
                    {
                        CommonSyn.ShortReplaceLms(list, 4, "USP_ADD_ShortBILLVEHICLESTAR_ZQTMS", "", batchNo, "");
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}