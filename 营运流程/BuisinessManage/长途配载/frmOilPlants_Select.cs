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

namespace ZQTMS.UI
{
    public partial class frmOilPlants_Select : BaseForm
    {
        public string CarNO = "";
        decimal oilFee;

        public decimal OilFee
        {
            get { return oilFee; }
        }
        string oilGuid;

        public string OilGuid
        {
            get { return oilGuid; }
        }

        decimal oilVolume;
        public decimal OilVolume
        {
            get { return oilVolume; }
        }

        decimal oilPrice;
        public decimal OilPrice
        {
            get { return oilPrice; }
        }

        public frmOilPlants_Select()
        {
            InitializeComponent();
        }

        public frmOilPlants_Select(string _CarNO)
        {
            InitializeComponent();
            CarNO = _CarNO;
        }

        private void frmOilPlants_Select_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);

            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("CarNO", CarNO));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_OilplantsRegister_byCarNo", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                else
                {
                    myGridControl1.DataSource = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridControl1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            oilGuid = GridOper.GetRowCellValueString(myGridView1, "oilId");
            oilFee = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, "sumAccount"));
            oilPrice = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, "oilPrice"));
            oilVolume = ConvertType.ToDecimal(GridOper.GetRowCellValueString(myGridView1, "oilVolume"));
            this.Close();
        }
    }
}
