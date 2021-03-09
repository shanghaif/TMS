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
    public partial class frmBasARSendFeeToVolumeAdd : BaseForm
    {
        public frmBasARSendFeeToVolumeAdd()
        {
            InitializeComponent();
        }

        public DataRow dr = null;       

        //Load
        private void frmBasARSendFeeToVolumeAdd_Load(object sender, EventArgs e)
        {
            //GetTravellingTrader();
            if(dr!=null)
            {
                txtBillNo.EditValue = dr["BillNo"];
                offerName.EditValue = dr["offerName"];
                travellingTrader.EditValue = dr["travellingTrader"];
                cost.EditValue = dr["FreightCharge"];
                startTime.EditValue = dr["startTime"];
                endTime.EditValue = dr["endTime"];
                Lindisfarne.EditValue = dr["Lindisfarne"];
                Destination.EditValue = dr["Destination"];
                salesProject.EditValue = dr["salesProject"];
                minimumCharge.EditValue = dr["minimumCharge"];
                Remark.Text=dr["Remark"].ToString();
                sOneStartVolume.EditValue = dr["sOneStartVolume"];
                sOneEndVolume.EditValue = dr["sOneEndVolume"];
                sOnePrice.EditValue = dr["sOnePrice"];
                sTwoStartVolume.EditValue = dr["sTwoStartVolume"];
                sTwoEndVolume.EditValue = dr["sTwoEndVolume"];
                sTwoPrice.EditValue = dr["sTwoPrice"];
                sThreeStartVolume.EditValue = dr["sThreeStartVolume"];
                sThreeEndVolume.EditValue = dr["sThreeEndVolume"];
                sThreePrice.EditValue = dr["sThreePrice"];
                sFourStartVolume.EditValue = dr["sFourStartVolume"];
                sFourEndVolume.EditValue = dr["sFourEndVolume"];
                sFourPrice.EditValue = dr["sFourPrice"];
                operationPeople.EditValue = dr["operationPeople"];  
            }
        }


        //取消
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //保存
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try 
            { 
                //foreach(Control item in this.Controls)
                //{
                //    if(item.GetType()==typeof(TextEdit) || item.GetType()==typeof(ComboBoxEdit) || item.GetType()==typeof(RichTextBox) || item.GetType()==typeof(DateTimePicker))
                //    {
                //        if(item.Text.Trim()=="")
                //        {
                //            MsgBox.ShowOK("每一项都必须填写！");
                //            return;
                //        }
                //    }
                //}
                if (txtBillNo.Text.Trim() == "")
                {
                    MsgBox.ShowOK("运单号不可以为空！");
                    return;
                }
                if(offerName.Text.Trim()=="")
                {
                    MsgBox.ShowOK("报价名称不可以为空！");
                    return;
                }
                
                List<SqlPara> list = new List<SqlPara>();

                list.Add(new SqlPara("volumeID", dr == null ? Guid.NewGuid() : dr["volumeID"]));
                list.Add(new SqlPara("BillNo", txtBillNo.Text.Trim()));
                list.Add(new SqlPara("offerName", offerName.Text.Trim()));
                list.Add(new SqlPara("travellingTrader", travellingTrader.Text.Trim()));
                list.Add(new SqlPara("FreightCharge", cost.Text.Trim()));
                list.Add(new SqlPara("startTime", startTime.DateTime));                
                list.Add(new SqlPara("endTime", endTime.DateTime));
                list.Add(new SqlPara("Lindisfarne", Lindisfarne.Text.Trim()));
                list.Add(new SqlPara("Destination", Destination.Text.Trim()));
                list.Add(new SqlPara("salesProject", salesProject.Text.Trim()));
                list.Add(new SqlPara("minimumCharge", minimumCharge.Text.Trim()));
                list.Add(new SqlPara("Remark", Remark.Text.Trim()));
                list.Add(new SqlPara("sOneStartVolume", sOneStartVolume.Text.Trim()));
                list.Add(new SqlPara("sOneEndVolume", sOneEndVolume.Text.Trim()));
                list.Add(new SqlPara("sOnePrice", sOnePrice.Text.Trim()));
                list.Add(new SqlPara("sTwoStartVolume", sTwoStartVolume.Text.Trim()));
                list.Add(new SqlPara("sTwoEndVolume", sTwoEndVolume.Text.Trim()));
                list.Add(new SqlPara("sTwoPrice", sTwoPrice.Text.Trim()));
                list.Add(new SqlPara("sThreeStartVolume", sThreeStartVolume.Text.Trim()));
                list.Add(new SqlPara("sThreeEndVolume", sThreeEndVolume.Text.Trim()));
                list.Add(new SqlPara("sThreePrice", sThreePrice.Text.Trim()));
                list.Add(new SqlPara("sFourStartVolume", sFourStartVolume.Text.Trim()));
                list.Add(new SqlPara("sFourEndVolume", sFourEndVolume.Text.Trim()));
                list.Add(new SqlPara("sFourPrice", sFourPrice.Text.Trim()));
                list.Add(new SqlPara("operationPeople", operationPeople.Text.Trim()));
                operateTime.Text = DateTime.Now.ToString();;
                list.Add(new SqlPara("operateTime", operateTime.Text));                
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BasARSendFeeToVolume", list);
                if(SqlHelper.ExecteNonQuery(sps)>0)
                {
                    MsgBox.ShowOK("保存成功！");
                    frmBasARSendFeeToVolume frm = (frmBasARSendFeeToVolume)this.Owner;
                    frm.getdate();
                    this.Close();
                    return;
                }            
            }
            catch(Exception ex)
            {
                MsgBox.ShowException(ex);
            }


        }

        //public void GetTravellingTrader()
        //{
        //    try
        //    {
        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_traderid");
        //        DataSet ds = SqlHelper.GetDataSet(sps);
        //        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        //        {
        //            return;
        //        }
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            travellingTrader.Properties.Items.Add(dr[0]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //}


    

    }
}
