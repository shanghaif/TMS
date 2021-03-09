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
using DevExpress.XtraGrid.Columns;

namespace ZQTMS.UI
{
    public partial class driverCheckAccount : BaseForm
    {
        public driverCheckAccount()
        {
            InitializeComponent();
        }
        private string state = string.Empty;
        GridColumn gcIsseleckedMode;

        private void frmVehicleWithhold_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int smallmonth=month-1;
            bdate.EditValue = year.ToString() +"-"+ smallmonth.ToString();
            edate.EditValue = year.ToString() + "-" + month.ToString();
            for (int i = month; i <= 12; i++)
            {
                edate.Properties.Items.AddRange(new object[] {
                (year-1)+"-"+i
                });
              
            }
            for (int i = 1; i <= month; i++)
            {
                edate.Properties.Items.AddRange(new object[] {
                   
                year+"-"+i
                });


            }
            for (int i = month; i <= 12; i++)
            {
                bdate.Properties.Items.AddRange(new object[] {
                (year-1)+"-"+i
                });

            }
            for (int i = 1; i <= month; i++)
            {
                bdate.Properties.Items.AddRange(new object[] {
                   
                year+"-"+i
                });


            }

           

                gcIsseleckedMode = GridOper.GetGridViewColumn(myGridView1, "ischecked");
                GridOper.CreateStyleFormatCondition(myGridView1, "State", DevExpress.XtraGrid.FormatConditionEnum.Equal, "核销", Color.Yellow);
                // GridOper.CreateStyleFormatCondition(myGridView1, "State", DevExpress.XtraGrid.FormatConditionEnum.Equal, "确认", Color.Yellow);


            
        }

        private void cbRetrieve_Click(object sender, EventArgs e)
        {
            select();

        }
        private void select()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
              
                //list.Add(new SqlPara("DriverName", txt_Driver.Text.Trim() == "" ? "%%" : "%" + txt_Driver.Text.Trim() + "%"));
                // list.Add(new SqlPara("esite", esite.Text.Trim() == "全部" ? "%%" : esite.Text.Trim()));
                //string bedate = "";
                //string enddate = "";
              
                //int bdateYear= Convert.ToDateTime(bdate.Text.Trim()).Year;
                //int bdateMonth = Convert.ToDateTime(bdate.Text.Trim()).Month;
                //bedate = bdateYear.ToString() + bdateMonth.ToString();

                //int edateYear = Convert.ToDateTime(edate.Text.Trim()).Year;
                //int edateMonth = Convert.ToDateTime(edate.Text.Trim()).Month;
                //enddate = edateYear.ToString() + edateMonth.ToString();
              int year=  Convert.ToDateTime(bdate.Text.Trim()).Year;
              int month = Convert.ToDateTime(bdate.Text.Trim()).Month;
              string bedate = year.ToString() +"-"+ month.ToString()+"-01 0:00";

                
              int eyear = Convert.ToDateTime(edate.Text.Trim()).Year;
              int emonth = Convert.ToDateTime(edate.Text.Trim()).Month;
              string enddate = "";
              if (emonth == 1 || emonth == 5 || emonth == 3 || emonth == 7 || emonth == 8 || emonth == 10 || emonth == 12) {
                   enddate = eyear.ToString() + "-" + emonth.ToString() + "-31 23:59";

              }
              else if (eyear % 4 == 0 && emonth == 2)
              {
                  enddate = eyear.ToString() + "-" + emonth.ToString() + "-29 23:59";

              }
              else  if (eyear % 4 != 0 && emonth == 2)
              {
                  enddate = eyear.ToString() + "-" + emonth.ToString() + "-28 23:59";

              }
              else {
                  enddate = eyear.ToString() + "-" + emonth.ToString() + "-30 23:59";

              }
            

              list.Add(new SqlPara("bdate", bedate));
              list.Add(new SqlPara("edate", enddate));
                list.Add(new SqlPara("carNo", carNo.Text.Trim()==""?"%%":carNo.Text.Trim()));
                list.Add(new SqlPara("cbbType", cbbType.Text.Trim() == "全部" ? "%%" : cbbType.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_DRIVERCHECKACCOUNT", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barbtn_Add_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVehicleWithholdAdd add = new frmVehicleWithholdAdd();
            add.oper = "1";
            add.ShowDialog();
        }

      


        private void barbtn_Export_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "车辆待扣款登记信息");
        }

        private void barbtn_exit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barbtn_look_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

  


        private void barbtn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmVehicleWithholdLead lead = new frmVehicleWithholdLead();
            lead.ShowDialog();

        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }


        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem21_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1, myGridView1.Guid.ToString());
        }

        private void barButtonItem22_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void cbbCause_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cbbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

     

        private void barButtonItem23_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void export_Click(object sender, EventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "司机对账");
        }

        private void edate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            string CarNo = myGridView1.GetRowCellValue(rowhandle, "CarNo").ToString();
            string Month = myGridView1.GetRowCellValue(rowhandle, "belongMonth").ToString();
            driverCheckDetail frm = new driverCheckDetail();
            frm.CarNo = CarNo;
            frm.Month = Month;
            
            frm.ShowDialog();
             
        }

    }
}
