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
    public partial class frmBasAPFeeToVolumeEdit : BaseForm
    {
        public DataRow dr=null;

        public frmBasAPFeeToVolumeEdit()
        {
            InitializeComponent();
        }

        //load
        private void frmBasAPFeeToVolumeEdit_Load(object sender, EventArgs e)
        {
           
            if(dr!=null)
            {
                txtBillNo.EditValue = dr["BillNo"];
                offerName.EditValue = dr["offerName"];
                affiliatedMerchants.EditValue = dr["Carrier"];
                costAccount.EditValue = dr["TransferDelieryCharge"];
                startTime.EditValue = dr["startTime"];
                endTime.EditValue = dr["endTime"];
                Lindisfarne.EditValue = dr["Lindisfarne"];
                Destination.EditValue = dr["Destination"];
                salesProject.EditValue = dr["salesProject"];
                Combien.EditValue = dr["Combien"];
                affiliatedMerchants.EditValue = dr["affiliatedMerchants"];
                sOneStartVolume.EditValue = dr["sOneStartVolume"];
                sOneEndVolume.EditValue = dr["sOneEndVolume"];
                sOnePrice.EditValue = dr["sOnePrice"];
                sTwoStartVolume.EditValue = dr["sTwoStartVolume"];
                sTwoEndVolume.EditValue = dr["sTwoEndVolume"];
                sTwoPrice.EditValue = dr["sTwoPrice"];
                sThreeStartVolume.EditValue = dr["sThreeStartVolume"];
                sThreeEndVolume.EditValue = dr["sThreeEndVolume"];
                sThreePrice.EditValue = dr["sThreePrice"];
                mainNumber.EditValue = dr["mainNumber"];
                singleNumber.EditValue = dr["singleNumber"];
                operationPeople.EditValue = dr["operationPeople"];
            }
        }


        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (txtBillNo.Text.Trim() == "")
            {
                MsgBox.ShowOK("运单号不可以为空！");
                return;
            }
            if (offerName.Text.Trim() == "")
            {
                MsgBox.ShowOK("报价名称不可以为空！");
                return;
            }
            try{
                List<SqlPara> list = new List<SqlPara>();

                list.Add(new SqlPara("volumeID", dr == null ? Guid.NewGuid() : dr["volumeID"]));
                list.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                list.Add(new SqlPara("offerName", offerName.Text.Trim()));
                list.Add(new SqlPara("Carrier", affiliatedMerchants.Text.Trim()));
                list.Add(new SqlPara("TransferDelieryCharge", costAccount.Text.Trim()));
                list.Add(new SqlPara("startTime", startTime.DateTime));                
                list.Add(new SqlPara("endTime", endTime.DateTime));
                list.Add(new SqlPara("Lindisfarne", Lindisfarne.Text.Trim()));
                list.Add(new SqlPara("Destination", Destination.Text.Trim()));
                list.Add(new SqlPara("salesProject", salesProject.Text.Trim()));
                list.Add(new SqlPara("Combien", Combien.Text.Trim()));
                list.Add(new SqlPara("affiliatedMerchants", affiliatedMerchants.Text.Trim()));
                list.Add(new SqlPara("sOneStartVolume", sOneStartVolume.Text.Trim()));
                list.Add(new SqlPara("sOneEndVolume", sOneEndVolume.Text.Trim()));
                list.Add(new SqlPara("sOnePrice", sOnePrice.Text.Trim()));
                list.Add(new SqlPara("sTwoStartVolume", sTwoStartVolume.Text.Trim()));
                list.Add(new SqlPara("sTwoEndVolume", sTwoEndVolume.Text.Trim()));
                list.Add(new SqlPara("sTwoPrice", sTwoPrice.Text.Trim()));
                list.Add(new SqlPara("sThreeStartVolume", sThreeStartVolume.Text.Trim()));
                list.Add(new SqlPara("sThreeEndVolume", sThreeEndVolume.Text.Trim()));
                list.Add(new SqlPara("sThreePrice", sThreePrice.Text.Trim()));
                list.Add(new SqlPara("mainNumber", mainNumber.Text.Trim()));
                list.Add(new SqlPara("singleNumber", singleNumber.Text.Trim()));                
                list.Add(new SqlPara("operationPeople", operationPeople.Text.Trim()));
                operateTime.Text = DateTime.Now.ToString();;
                list.Add(new SqlPara("operateTime", operateTime.Text));                
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasAPFeeToVolume", list);
                if(SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK("保存成功！");
                    frmBasAPFeeToVolume ower = (frmBasAPFeeToVolume)this.Owner;
                    ower.getdate();
                    this.Close();
                }
               
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

       


        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
