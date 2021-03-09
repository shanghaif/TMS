using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;


namespace ZQTMS.UI
{
    public partial class frmArrMessage : BaseForm
    {
        public frmArrMessage()
        {
            InitializeComponent();
        }
        GridColumn gcIsseleckedMode;
        private void frmShortConnOutbound_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("催款短信");//xj/2019/5/29
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, myGridView2);
            GridOper.SetGridViewProperty(myGridView1,myGridView2);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            FixColumn fix = new FixColumn(myGridView1, barSubItem4);
            bdate.DateTime = CommonClass.gbdate; ;
            CommonClass.SetCause(CauseName, true);
            CauseName.EditValue = CommonClass.UserInfo.CauseName;
            AreaName.EditValue = CommonClass.UserInfo.AreaName;
            WebName.EditValue = CommonClass.UserInfo.WebName;
            gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView2, "ischecked");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getData();
        }
        public void getData()
        {
            if(Payment.Text.Trim() == "月结")
                myGridControl1.MainView = myGridView2;
            else
                myGridControl1.MainView = myGridView1;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("cause", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("area", AreaName.Text.Trim() == "全部" ? "%%" : AreaName.Text.Trim()));
                list.Add(new SqlPara("web", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                list.Add(new SqlPara("PayMent ", Payment.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_ARRMESSAGE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);

                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            finally
            {
                if (myGridView1.RowCount < 1000) myGridView1.BestFitColumns();
            }
        }
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1, myGridView2);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView2);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1, myGridView2);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString(), myGridView2.Guid.ToString());
        }
        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(Payment.Text == "月结")
                GridOper.ExportToExcel(myGridView2);
            else
                GridOper.ExportToExcel(myGridView1);
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void btnAduit_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (myGridView1.RowCount > 0) 
            {
                for (int i = 0; i < myGridView1.RowCount ; i++ )
                {
                    if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "ischecked")) == "1") 
                    {
                        string chauffermb = ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorCellPhone")) == "" ? ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorPhone")) : ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorCellPhone"));
                        string chauffer = ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorName")) == "" ? ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorCompany"))  : ConvertType.ToString(myGridView1.GetRowCellValue(i, "ConsignorName"));
                        string Amount = "";
                        Amount = ConvertType.ToString(myGridView1.GetRowCellValue(i, "UnpaidAccounts"));
                        if (ConvertType.ToString(myGridView1.GetRowCellValue(i, "UnpaidAccounts")) == "" || ConvertType.ToDecimal(myGridView1.GetRowCellValue(i, "UnpaidAccounts")) == 0) 
                        {
                            Amount = ConvertType.ToString(myGridView1.GetRowCellValue(i, "AccountsPayable"));
                        }
                        string billno = ConvertType.ToString(myGridView1.GetRowCellValue(i, "BillNo"));
                        string content = "";
                        if(Payment.Text.Trim() == "月结")
                            content = "尊敬的" + chauffer + "：截止2016年9月30日，您有" + Amount + "元运费尚未支付给我司，为保障双方权益，请您收到短信后尽快安排运费支付事宜，谢谢！ 【中强集团】";
                        else
                            content = "尊敬的" + chauffer + "：截止2016年9月30日，您有" + billno + "运费" + Amount + "元运费尚未支付给我司，为保障双方权益，请您收到短信后尽快安排运费支付事宜，谢谢！ 【中强集团】";
                        if (chauffermb != "")
                        {
                            if (chauffermb.Substring(0, 1) == "1")
                            {
                                if (sms.SaveSMSS("0", chauffer, chauffermb, content, CommonClass.gcdate, "0"))
                                {
                                    sms.sendsms(chauffermb, content);
                                }
                            }
                        }
                        
                    }
                }
            }
            if (myGridView2.RowCount > 0)
            {
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "ischecked")) == "1")
                    {
                        string chauffermb = ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorCellPhone")) == "" ? ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorPhone")) : ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorCellPhone"));
                        string chauffer = ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorName")) == "" ? ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorCompany")) : ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorName"));
                        string Amount = "";
                        Amount = ConvertType.ToString(myGridView2.GetRowCellValue(i, "UnpaidAccounts"));
                        if (ConvertType.ToString(myGridView2.GetRowCellValue(i, "UnpaidAccounts")) == "" || ConvertType.ToDecimal(myGridView2.GetRowCellValue(i, "UnpaidAccounts")) == 0)
                        {
                            Amount = ConvertType.ToString(myGridView2.GetRowCellValue(i, "AccountsPayable"));
                        }
                        string billno = ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo"));
                        string content = "";
                        if (Payment.Text.Trim() == "月结")
                            content = "尊敬的" + chauffer + "：截止2016年9月30日，您有" + Amount + "元运费尚未支付给我司，为保障双方权益，请您收到短信后尽快安排运费支付事宜，谢谢！ 【中强集团】";
                        else
                            content = "尊敬的" + chauffer + "：截止2016年9月30日，您有" + billno + "运费" + Amount + "元运费尚未支付给我司，为保障双方权益，请您收到短信后尽快安排运费支付事宜，谢谢！ 【中强集团】";
                        if (chauffermb != "")
                        {
                            if (chauffermb.Substring(0, 1) == "1")
                            {
                                if (sms.SaveSMSS("0", chauffer, chauffermb, content, CommonClass.gcdate, "0"))
                                {
                                    if (sms.sendsms(chauffermb, content))
                                        SaveMsgTime(ConvertType.ToString(myGridView2.GetRowCellValue(i, "BillNo")),
                                            ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorName")),
                                            ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorCellPhone")),
                                            ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorPhone")),
                                            ConvertType.ToString(myGridView2.GetRowCellValue(i, "ConsignorCompany")),
                                            ConvertType.ToString(myGridView2.GetRowCellValue(i, "BegWeb")));
                                }
                            }
                        }

                    }
                }
            }
           
        }
        private void SaveMsgTime(string BillNo,string ConsignorName,string ConsignorCellPhone,string ConsignorPhone,string ConsignorCompany,string BegWeb)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", BillNo));
                list.Add(new SqlPara("ConsignorName", ConsignorName));
                list.Add(new SqlPara("ConsignorCellPhone", ConsignorCellPhone));
                list.Add(new SqlPara("ConsignorPhone", ConsignorPhone));
                list.Add(new SqlPara("BegWeb", BegWeb));
                list.Add(new SqlPara("ConsignorCompany", ConsignorCompany));

                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_BILLARRMESSAGERECORD", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        private void CauseName_EditValueChanged(object sender, EventArgs e)
        {
            //CommonClass.SetArea(AreaName, CauseName.EditValue.ToString()); 
            //AreaName.EditValue = CommonClass.UserInfo.AreaName;               
            CommonClass.SetArea(AreaName, CauseName.Text);          
            CommonClass.SetCauseWeb(WebName, CauseName.Text, AreaName.Text);
        }

        private void AreaName_EditValueChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(WebName, CauseName.Text.Trim(), AreaName.Text.Trim());
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //if (e == null || myGridView1.FocusedRowHandle < 0) return;
            //try
            //{
            //    float FetchPayVerifBalance = ConvertType.ToFloat(myGridView1.GetFocusedRowCellValue("FetchPayVerifBalance"));
            //    float Amount = ConvertType.ToFloat(e.Value);
            //    if (Amount <= 0)
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "审核金额必须大于0!";
            //        return;
            //    }
            //    if (Amount > FetchPayVerifBalance)
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "审核金额不能大于余额!";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowError(ex.Message);
            //}
        }

        private void myGridView1_InvalidValueException(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            MsgBox.ShowError(e.ErrorText);
        }
        private void chkALL_CheckedChanged(object sender, EventArgs e)
        {
            if (Payment.Text.Trim() == "月结")
            {
                int a = chkALL.Checked == true ? 1 : 0;
                for (int i = 0; i < myGridView2.RowCount; i++)
                {
                    myGridView2.SetRowCellValue(i, gcIsseleckedMode, a);
                }
            }
            else 
            {
                int a = chkALL.Checked == true ? 1 : 0;
                for (int i = 0; i < myGridView1.RowCount; i++)
                {
                    myGridView1.SetRowCellValue(i, gcIsseleckedMode, a);
                }
            }
        }

        private void Payment_EditValueChanged(object sender, EventArgs e)
        {
            if(Payment.Text.Trim() == "月结")
                gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView2, "ischecked");
            else
                gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
        }
    }
}