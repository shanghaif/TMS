using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class driverCheckDetail : BaseForm
    {
        public driverCheckDetail()
        {
            InitializeComponent();
        }
        public string CarNo = "", Month="";
         

        private void driverCheckDetail_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            GridOper.SetGridViewProperty(myGridView1);
            CommonClass.GetGridViewColumns(myGridView1, false);
          //  BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            if (CarNo == "" || Month == "")
            {
                MsgBox.ShowOK("未获得参数!");
                this.Close();
            }
            getdata();
        }
        public void getdata()
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("CarNo", CarNo));
            list.Add(new SqlPara("Month", Month));
            SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GetDriverDetail", list);
            DataSet ds=SqlHelper.GetDataSet(spe);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                myGridControl1.DataSource = ds.Tables[0];
            }

        }
    }
}