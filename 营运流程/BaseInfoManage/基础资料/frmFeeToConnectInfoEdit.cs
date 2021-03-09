using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmFeeToConnectInfoEdit : BaseForm
    {
        public DataRow dr = null;
        public frmFeeToConnectInfoEdit()
        {
            InitializeComponent();
        }

        private void frmFeeToConnectInfoEdit_Load(object sender, EventArgs e)
        {
            GetModeOfTransport();
            if (dr != null)
            {
                txtTravellingTrader.EditValue = dr["TravellingTrader"];
                txtWayType.EditValue = dr["WayType"];
                txtOperator.EditValue = dr["Operator"];
                txtModeOfTransport.EditValue = dr["ModeOfTransport"];
                txtDeliveryMethod.EditValue = dr["DeliveryMethod"];
            }
        }

        public void GetModeOfTransport()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_ModeOfTransportID");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    txtModeOfTransport.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //保存
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtWayType.Text == "应收报价件数" || txtWayType.Text == "应付报价件数")
            {
                txtleixing.Text = "按件计";
            }
            else if (txtWayType.Text == "应收报价体积" || txtWayType.Text == "应付报价体积")
            {
                txtleixing.Text = "按体积计";
            }
            else
            {
                txtleixing.Text = "按重量计";
            }



            try
            {
                foreach (Control item in this.Controls)
                {
                    if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(ComboBoxEdit))
                    {
                        if (item.Text.Trim() == "")
                        {
                            MsgBox.ShowOK("每一项都必须填写!");
                            return;
                        }
                    }
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Cid", dr == null ? Guid.NewGuid() : dr["Cid"]));
                list.Add(new SqlPara("TravellingTrader", txtTravellingTrader.Text.Trim()));
                list.Add(new SqlPara("ModeOfTransport", txtModeOfTransport.Text.Trim()));
                list.Add(new SqlPara("WayType", txtWayType.Text.Trim()));
                list.Add(new SqlPara("WayType2", txtleixing.Text.Trim()));

                list.Add(new SqlPara("DeliveryMethod", txtDeliveryMethod.Text.Trim()));


                //类型对应表名

                //应收报价
                if (txtWayType.Text == "应收报价件数")
                {
                    list.Add(new SqlPara("TableName", "basARFeeToNum"));

                }
                if (txtWayType.Text == "应收报价体积")
                {
                    list.Add(new SqlPara("TableName", "basARFeeToVolume"));
                }
                if (txtWayType.Text == "应收报价重量")
                {
                    list.Add(new SqlPara("TableName", "basARFeeToWeight"));
                }
                //应付报价
                if (txtWayType.Text == "应付报价件数")
                {
                    list.Add(new SqlPara("TableName", "basAPFeeToNum"));
                }
                if (txtWayType.Text == "应付报价体积")
                {
                    list.Add(new SqlPara("TableName", "basAPFeeToVolume"));
                }
                if (txtWayType.Text == "应付报价重量")
                {
                    list.Add(new SqlPara("TableName", "basAPFeeToWeight"));
                }
                //应收报价运费
                if (txtWayType.Text == "应收报价件数运费")
                {
                    list.Add(new SqlPara("TableName", "basARSendFeeToNum"));
                }
                if (txtWayType.Text == "应收报价体积运费")
                {
                    list.Add(new SqlPara("TableName", "basARSendFeeToVolume"));
                }
                if (txtWayType.Text == "应收报价重量运费")
                {
                    list.Add(new SqlPara("TableName", "basARSendFeeToWeight"));
                }
                //应付报价运费
                if (txtWayType.Text == "应付报价件数运费")
                {
                    list.Add(new SqlPara("TableName", "basAPSendFeeToNum"));
                }
                if (txtWayType.Text == "应付报价体积运费")
                {
                    list.Add(new SqlPara("TableName", "basAPSendFeeToVolume"));
                }
                if (txtWayType.Text == "应付报价重量运费")
                {
                    list.Add(new SqlPara("TableName", "basAPSendFeeToWeight"));
                }
                list.Add(new SqlPara("Operator", txtOperator.Text.Trim()));
                txtOperationTime.Text = DateTime.Now.ToString();
                list.Add(new SqlPara("OperationTime ", txtOperationTime.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasFeeToConnect", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    frmFeeToConnectInfo ower = (frmFeeToConnectInfo)this.Owner;
                    ower.getdate();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //退出
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
