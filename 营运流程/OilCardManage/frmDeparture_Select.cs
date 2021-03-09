using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Common;
using ZQTMS.SqlDAL;
using ZQTMS.Tool;

namespace ZQTMS.UI
{
    public partial class frmDeparture_Select : BaseForm
    {
        public string car = "";
        public int flag = 0;

        public frmDeparture_Select()
        {
            InitializeComponent();
        }

        private void frmDeparture_Select_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            bdate.DateTime = CommonClass.gbdate.AddHours(-16);
            edate.DateTime = CommonClass.gedate.AddHours(-16);
            
            CommonClass.SetSite(bSite, true);
            CommonClass.SetSite(eSite, true);
            CommonClass.SetCause(Cause, true);

            bSite.EditValue = CommonClass.UserInfo.SiteName;
            Cause.Text = CommonClass.UserInfo.CauseName;
            Area.Text = CommonClass.UserInfo.AreaName;
            web.Text = CommonClass.UserInfo.WebName;
            eSite.EditValue = "全部";
        }


        private void freshData()
        {
            string causeName = Cause.Text.Trim() == "全部" ? "%%" : Cause.Text;
            string areaName = Area.Text.Trim() == "全部" ? "%%" : Area.Text;
            string bsite = bSite.Text.Trim() == "全部" ? "%%" : bSite.Text;
            string esite = eSite.Text.Trim() == "全部" ? "%%" : eSite.Text;
            string webName = web.Text.Trim() == "全部" ? "%%" : web.Text;
            string carName = car == "" ? "%%" : car;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate", bdate.DateTime));
                list.Add(new SqlPara("edate", edate.DateTime));
                list.Add(new SqlPara("CauseName", causeName));
                list.Add(new SqlPara("AreaName", areaName));
                list.Add(new SqlPara("bSite", bsite));
                list.Add(new SqlPara("eSite", esite));
                list.Add(new SqlPara("webName", webName));
                list.Add(new SqlPara("LoadingType", LoadingType.SelectedIndex));
                list.Add(new SqlPara("CarNo", carName));
                DataSet ds = new DataSet();
                if (flag == 1)
                {
                    SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE2", list);
                    ds = SqlHelper.GetDataSet(sps);
                }
                else if (flag == 2)
                {
                    SqlParasEntity _sps = new SqlParasEntity(OperType.Query, "QSP_GET_BILLDEPARTURE3", list);
                    ds = SqlHelper.GetDataSet(_sps);
                }
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

        string Batchno = "";

        public string Batchno1
        {
            get { return Batchno; }
        }

        string CarNO = "";

        public string CarNO1
        {
            get { return CarNO; }
        }

        string OilFee = "";
        public string OilFee1
        {
            get { return OilFee; }
        }

        string oilVolume = "";
        public string oilVolume1
        {
            get { return oilVolume; }
        }

        string oilPrice = "";
        public string oilPrice1
        {
            get { return oilPrice; }
        }
        private void myGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (myGridView1.FocusedRowHandle < 0) return;
            Batchno = GridOper.GetRowCellValueString(myGridView1, "DepartureBatch");
            CarNO = GridOper.GetRowCellValueString(myGridView1, "CarNO");
            OilFee = GridOper.GetRowCellValueString(myGridView1, "OilFee");
            oilVolume = GridOper.GetRowCellValueString(myGridView1, "oilVolume");
            oilPrice = GridOper.GetRowCellValueString(myGridView1, "oilPrice");
            this.Close();
        }

        private void btnRetrieve_Click_1(object sender, EventArgs e)
        {
            freshData();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Cause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(Area, Cause.Text);
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

        private void Area_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(web, Cause.Text, Area.Text);
        }

    }
}
