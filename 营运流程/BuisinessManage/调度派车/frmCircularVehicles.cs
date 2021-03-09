using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmCircularVehicles : BaseForm
    {
        public string lonAndLat = "";
        public string vehiclesNo = "";  //车号
        public frmCircularVehicles()
        {
            InitializeComponent();
        }

        private void frmCircularVehicles_Load(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1);
            try
            {
                if (lonAndLat != null)
                {
                    string[] lonLatArray = lonAndLat.Split('|');
                    List<VehicleModel> list = E6GPS.GetCircularVehicles(ConvertType.ToDecimal(lonLatArray[0]), ConvertType.ToDecimal(lonLatArray[1]), 10);
                    if (list != null && list.Count > 0)
                    {
                        myGridControl1.DataSource = list;
                    }
                }
            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            vehiclesNo = ConvertType.ToString(myGridView1.GetRowCellValue(rowhandle, "Vehicle"));
            this.Close();
        }
    }
}
