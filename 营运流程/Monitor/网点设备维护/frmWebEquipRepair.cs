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
    public partial class frmWebEquipRepair : BaseForm
    {
        public frmWebEquipRepair()
        {
            InitializeComponent();
        }

        private void frmWebEquipRepair_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("设备维护");//xj/2019/5/28
            bdate.DateTime = CommonClass.gbdate;
            edate.DateTime = CommonClass.gedate;
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("t1",bdate.DateTime));
                list.Add(new SqlPara("t2",edate.DateTime));
                list.Add(new SqlPara("EquipNum",txtEquipNum.Text.Trim()));
                list.Add(new SqlPara("EquipName",txtEquipName.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_WebEquiPInfo", list));
                if (ds == null || ds.Tables.Count > 0 || ds.Tables[0].Rows.Count == 0) return;
                myGridControl1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}