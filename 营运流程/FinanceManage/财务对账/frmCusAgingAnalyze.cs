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
using DevExpress.XtraGrid.Views.BandedGrid;

namespace ZQTMS.UI
{
    public partial class frmCusAgingAnalyze : BaseForm
    {
        public frmCusAgingAnalyze()
        {
            InitializeComponent();
        }
        DataSet ds = new DataSet();
        DataTable dsresult = new DataTable();
        private void frmCusAgingAnalyze_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            BarMagagerOper.SetBarPropertity(bar1);
            FixColumn fix = new FixColumn(bandedGridView2, barSubItem2);
            //FixColumn fix1 = new FixColumn(gridView2, barSubItem5);
            GridOper.RestoreGridLayout(bandedGridView2, this.Text);
            //dateEdit1.DateTime = CommonClass.gbdate.AddMonths(-1);
            //dateEdit4.EditValue = CommonClass.gbdate.AddMonths(-1);
            //dateEdit2.EditValue = CommonClass.gbdate.Year;
            //dateEdit3.EditValue = CommonClass.gbdate.Month;
            dateEdit2.EditValue = CommonClass.gbdate.Year.ToString();
            dateEdit3.EditValue = CommonClass.gbdate.Month.ToString();
            dateEdit4.EditValue = CommonClass.gbdate.AddMonths(-12);    //月份，chenxiang
            dateEdit1.EditValue = CommonClass.gbdate.AddYears(-1);      //年份,chenxiang

            int month = ConvertType.ToInt32(dateEdit3.Text.Trim());
            int count = ConvertType.ToInt32(Num.Text.Trim());
            gridBand13.Caption = dateEdit2.Text.Trim().ToString() + "年" + (month - count + 1) + "月之前";

            CommonClass.SetCause(CauseName, true);              //chenxiang,02-21
            CommonClass.SetWeb(PartCompany, true);
            CauseName.Text=CommonClass.UserInfo.CauseName;
            CountObject.Text = "发货单位";                       
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            getdata();
        }
        private void getdata()
        {

            string t1 = dateEdit1.Text.ToString() + "-" + dateEdit4.Text.ToString() + "-01";//开始时间
            int monthdate = ConvertType.ToInt32(dateEdit3.Text.ToString()) + 1;
            int yeardate = ConvertType.ToInt32(dateEdit2.Text.ToString());
            if (monthdate > 12)
            {
                monthdate -= 12;
                yeardate++;
            }
            string t2 = yeardate + "-" + monthdate + "-01";//截止时间
            int year = ConvertType.ToInt32(dateEdit2.Text.Trim());
            int month = (ConvertType.ToInt32(dateEdit3.Text.Trim()) - ConvertType.ToInt32(Num.Text.Trim()));
            if (month <= 0)
            {
                year--;
                month += 12;
            }
            string t3 = year + "-" + month + "-01";//用作判断的时间
            if (ConvertType.ToDateTime(t1) > ConvertType.ToDateTime(t3))
            {
                MsgBox.ShowOK("开始时间不能大于截止时间减去账期的时间");
                return;
            }

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1", t1));//开始时间
                list.Add(new SqlPara("t2", t2));//截止时间
                list.Add(new SqlPara("CauseName", CauseName.Text.Trim() == "全部" ? "%%" : CauseName.Text.Trim()));
                list.Add(new SqlPara("PartCompany", PartCompany.Text.Trim() == "全部" ? "%%" : PartCompany.Text.Trim()));//分公司 网点
                list.Add(new SqlPara("CountObject", CountObject.Text.Trim() == "全部" ? "%%" : CountObject.Text.Trim()));//统计对象
                //list.Add(new SqlPara("Num", Num.Text.Trim()));//账期
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_CusAgingAnalyze", list);
                 ds = SqlHelper.GetDataSet(spe);
                 dsresult = ds.Tables[0].Clone();
                if (ds.Tables.Count == 0 || ds.Tables[0] == null) return;
                //gridControl1.DataSource = ds.Tables[0];
                BuildBands();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
                return;
            }
        }
        private void BuildBands()
        {
            //还原网格
            for (int i = 2; i < 13; i++)
            {
                bandedGridView2.Bands[i].Visible = false;
            }
            //某时间之前的网格名称
            int year1 = ConvertType.ToInt32(dateEdit1.Text.Trim());//开始年
            int month1 = ConvertType.ToInt32(dateEdit4.Text.Trim());//开始月
            int year = ConvertType.ToInt32(dateEdit2.Text.Trim()); //截止年
            int month = ConvertType.ToInt32(dateEdit3.Text.Trim());//截止月
            int count = ConvertType.ToInt32(Num.Text.Trim());//账期
            int check = month - count + 1;
            if (check < 1) 
            {
                year--;
                check = check + 12;
            }
            string time = year1 + "年" + month1 + "月到" + year + "年" + check + "月合计";
            gridBand13.Caption = time;
            //其余的网格名称
            for (int i = 1; i < count + 1; i++)
            {
                int showMonth = month - count + i;
                int showyear = ConvertType.ToInt32(dateEdit2.Text.Trim());
                if (showMonth < 1)
                {
                    showMonth = showMonth + 12;
                    showyear--;
                }
                string bandCaption = showyear + "年" + showMonth + "月";
                bandedGridView2.Bands[i + 1].Visible = true;
                bandedGridView2.Bands[i + 1].Caption = bandCaption;
                //string hh = "HappenAmount" + (i - 1);
                //string pp = "PayedMoney" + (i - 1);
                //string oo = "OweFee" + (i - 1);
                //dsresult.Columns.Add(hh, typeof(string));
                //dsresult.Columns.Add(pp, typeof(decimal));
                //dsresult.Columns.Add(oo, typeof(decimal));
            }

            DataTable newDt = new DataTable();//作为数据源
            newDt.Columns.Add("CusName", typeof(string));
            newDt.Columns.Add("HappenAmount", typeof(string));
            newDt.Columns.Add("PayedMoney", typeof(decimal));
            newDt.Columns.Add("OweFee", typeof(decimal));
            newDt.Columns.Add("HappenAmount0", typeof(string));
            newDt.Columns.Add("PayedMoney0", typeof(decimal));
            newDt.Columns.Add("OweFee0", typeof(decimal));
            newDt.Columns.Add("HappenAmount1", typeof(string));
            newDt.Columns.Add("PayedMoney1", typeof(decimal));
            newDt.Columns.Add("OweFee1", typeof(decimal));
            newDt.Columns.Add("HappenAmount2", typeof(string));
            newDt.Columns.Add("PayedMoney2", typeof(decimal));
            newDt.Columns.Add("OweFee2", typeof(decimal));
            newDt.Columns.Add("HappenAmount3", typeof(string));
            newDt.Columns.Add("PayedMoney3", typeof(decimal));
            newDt.Columns.Add("OweFee3", typeof(decimal));
            newDt.Columns.Add("HappenAmount4", typeof(string));
            newDt.Columns.Add("PayedMoney4", typeof(decimal));
            newDt.Columns.Add("OweFee4", typeof(decimal));
            newDt.Columns.Add("HappenAmount5", typeof(string));
            newDt.Columns.Add("PayedMoney5", typeof(decimal));
            newDt.Columns.Add("OweFee5", typeof(decimal));
            newDt.Columns.Add("HappenAmount6", typeof(string));
            newDt.Columns.Add("PayedMoney6", typeof(decimal));
            newDt.Columns.Add("OweFee6", typeof(decimal));
            newDt.Columns.Add("HappenAmount7", typeof(string));
            newDt.Columns.Add("PayedMoney7", typeof(decimal));
            newDt.Columns.Add("OweFee7", typeof(decimal));
            newDt.Columns.Add("HappenAmount8", typeof(string));
            newDt.Columns.Add("PayedMoney8", typeof(decimal));
            newDt.Columns.Add("OweFee8", typeof(decimal));
            newDt.Columns.Add("HappenAmount9", typeof(string));
            newDt.Columns.Add("PayedMoney9", typeof(decimal));
            newDt.Columns.Add("OweFee9", typeof(decimal));
            newDt.Columns.Add("HappenAmount10", typeof(string));
            newDt.Columns.Add("PayedMoney10", typeof(decimal));
            newDt.Columns.Add("OweFee10", typeof(decimal));
            newDt.Columns.Add("HappenAmount11", typeof(string));
            newDt.Columns.Add("PayedMoney11", typeof(decimal));
            newDt.Columns.Add("OweFee11", typeof(decimal));
            newDt.Columns.Add("HappenAmount12", typeof(string));
            newDt.Columns.Add("PayedMoney12", typeof(decimal));
            newDt.Columns.Add("OweFee12", typeof(decimal));

            string begindate = year1.ToString() + "-" + month1.ToString() + "-01 00:00:00";
            string enddate = year.ToString() + "-" + check.ToString() + "-01 00:00:00";
            
            
            //DataRow[] dr = ds.Tables[0].Select("BillDate > '" + begindate + "'and BillDate <'" + enddate + "'", "CusName");//总合计

            List<string> list = new List<string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (list.Contains(ConvertType.ToString(ds.Tables[0].Rows[i]["CusName"]).Trim())) continue;
                list.Add(ConvertType.ToString(ds.Tables[0].Rows[i]["CusName"]).Trim());
                DataRow[] dr = ds.Tables[0].Select("CusName = '" + ConvertType.ToString(ds.Tables[0].Rows[i]["CusName"]).Trim()+"'");
                
                DataRow dr1 = newDt.NewRow();
                dr1["CusName"] = ConvertType.ToString(ds.Tables[0].Rows[i]["CusName"]).Trim();
                for (int j = 0; j < dr.Length; j++)
                {
                    //合计
                    dr1["HappenAmount"] = ConvertType.ToDecimal(dr1["HappenAmount"]) + ConvertType.ToDecimal(dr[j]["HappenAmount"]);
                    dr1["PayedMoney"] = ConvertType.ToDecimal(dr1["PayedMoney"]) + ConvertType.ToDecimal(dr[j]["PayedMoney"]);
                    dr1["OweFee"] = ConvertType.ToDecimal(dr1["OweFee"]) + ConvertType.ToDecimal(dr[j]["OweFee"]);

                    //之前
                        decimal HappenAmount = 0, PayedMoney = 0, OweFee = 0;
                        if (ConvertType.ToDateTime(dr[j]["BillDate"]) > ConvertType.ToDateTime(year1.ToString() + "-" + month1.ToString() + "-01 00:00:00") && ConvertType.ToDateTime(dr[j]["BillDate"]) < ConvertType.ToDateTime(year.ToString() + "-" + check.ToString() + "-01 00:00:00"))
                        {
                        HappenAmount += ConvertType.ToDecimal(dr[j]["HappenAmount"]);
                        PayedMoney += ConvertType.ToDecimal(dr[j]["PayedMoney"]);
                        OweFee += ConvertType.ToDecimal(dr[j]["OweFee"]);
                        dr1["HappenAmount0"] =  ConvertType.ToDecimal(dr1["HappenAmount0"])+HappenAmount;
                        dr1["PayedMoney0"] = ConvertType.ToDecimal(dr1["PayedMoney0"]) + PayedMoney;
                        dr1["OweFee0"] = ConvertType.ToDecimal(dr1["OweFee0"]) + OweFee;

                        continue;
                        }
                        //每个月
                        string Happen = "", Payed = "", Owe = "";
                        string time1 = year.ToString() + "-" + check.ToString() + "-01 00:00:00";
                        int checknew = check;
                        int yearnew = year;
                        for (int n = 1; n < count+1; n++)
                        {
                            checknew++;
                            if (checknew > 12)
                            {
                                checknew -= 12;
                            }

                            if (checknew == 1)
                            {
                                yearnew++;
                            }
                            string time2 = yearnew.ToString() + "-" + checknew.ToString() + "-01 00:00:00";
                            Happen ="HappenAmount"+ n;
                            Payed = "PayedMoney"+ n;
                            Owe = "OweFee"+ n;

                            if (ConvertType.ToDateTime(dr[j]["BillDate"]) > ConvertType.ToDateTime(time1) && ConvertType.ToDateTime(dr[j]["BillDate"]) < ConvertType.ToDateTime(time2))
                            {
                                HappenAmount += ConvertType.ToDecimal(dr[j]["HappenAmount"]);
                                PayedMoney += ConvertType.ToDecimal(dr[j]["PayedMoney"]);
                                OweFee += ConvertType.ToDecimal(dr[j]["OweFee"]);
                                dr1[Happen] = ConvertType.ToDecimal(dr1[Happen]) + HappenAmount;
                                dr1[Payed] = ConvertType.ToDecimal(dr1[Payed]) + PayedMoney;
                                dr1[Owe] = ConvertType.ToDecimal(dr1[Owe]) + OweFee;
                            }
                            time1 = time2;
                        }
                    }
               // MsgBox.ShowOK(dr1["HappenAmount0"].ToString() + dr1["PayedMoney0"].ToString()+(dr1["OweFee0"].ToString()));
                //newDt.ImportRow(dr1);
                newDt.Rows.Add(dr1);
            }
            //DataTable dt1 = (DataTable)dsresult.Compute("sum(HappenAmount),sum(PayedMoney),sum(OweFee)", "CusName");

            gridControl1.DataSource = newDt;
            
           

            
            
           // ds.Tables[0].Compute("Sum(HappenAmount),Sum(PayedMoney),Sum(OweFee)", "BillDate > " + ConvertType.ToDateTime(begindate) + "and BillDate <" + ConvertType.ToDateTime(enddate));
            
        }
        

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(bandedGridView2, this.Text);
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(bandedGridView2, this.Text);
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(bandedGridView2);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(bandedGridView2);
        }

        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(bandedGridView2);
        }

        
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(gridView2);
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(gridView2, this.Text);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(gridView2);
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(gridView2, this.Text);
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(gridView2);
        }

        private void CauseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(PartCompany, PartCompany.Text);
        }

        private void CountObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CountObject.SelectedIndex = 0;
        }

       

       
    }
}
