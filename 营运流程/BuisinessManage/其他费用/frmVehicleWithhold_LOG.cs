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

namespace ZQTMS.UI
{
    public partial class frmVehicleWithhold_LOG :BaseForm
    {
        public frmVehicleWithhold_LOG()
        {
            InitializeComponent();
        }

        public string ID = "", CarNo = "", Month = "";
      

        private void frmVehicleWithhold_LOG_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView1, false);
            //BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);

            if (ID == "" && (CarNo == "" && Month == ""))
            {
                MsgBox.ShowOK("未获取到参数!");
                this.Close();
            }

            getdata();
        }
        public void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("ID", ID));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "Get_VehicleWithhold_LOG", list);
            DataSet ds = SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                myGridControl1.DataSource = ds.Tables[0];
            }
        }
    }
}