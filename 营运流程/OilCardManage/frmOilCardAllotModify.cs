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
using DevExpress.XtraGrid.Views.Grid;

namespace ZQTMS.UI
{
    public partial class frmOilCardAllotModify : ZQTMS.Tool.BaseForm
    {
        GridView _gv;

        public GridView Gv
        {
            get { return _gv; }
            set { _gv = value; }
        }

        public frmOilCardAllotModify()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            int rowhandle = _gv.FocusedRowHandle;
            if (rowhandle < 0) return;

            int id = ConvertType.ToInt32(_gv.GetRowCellValue(rowhandle, "id"));
            string chauffer = Chauffer.Text.Trim();
            if (chauffer == "")
            {
                MsgBox.ShowOK("请填写司机!");
                Chauffer.Focus();
                return;
            }
            string chaufferTel = ChaufferTel.Text.Trim();
            if (chaufferTel == "")
            {
                MsgBox.ShowOK("请填写司机电话!");
                ChaufferTel.Focus();
                return;
            }
            string vehicleNo = VehicleNo.Text.Trim();
            if (vehicleNo == "")
            {
                MsgBox.ShowOK("请填写车号!");
                VehicleNo.Focus();
                return;
            }

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id", id));
            list.Add(new SqlPara("Chauffer", chauffer));
            list.Add(new SqlPara("ChaufferTel", chaufferTel));
            list.Add(new SqlPara("VehicleNo", vehicleNo));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_MODIFY_OILCARD_ALLOT", list)) == 0) return;
            _gv.SetRowCellValue(rowhandle, "Chauffer", chauffer);
            _gv.SetRowCellValue(rowhandle, "ChaufferTel", chaufferTel);
            _gv.SetRowCellValue(rowhandle, "VehicleNo", vehicleNo);
            MsgBox.ShowYesNo("保存成功!");
            this.Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmOilCardAllotModify_Load(object sender, EventArgs e)
        {
            if (_gv == null || _gv.FocusedRowHandle < 0) return;
            OilCardNo.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "OilCardNo");
            Company.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "Company");
            Account.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "Account");
            Chauffer.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "Chauffer");
            ChaufferTel.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "ChaufferTel");
            VehicleNo.EditValue = _gv.GetRowCellValue(_gv.FocusedRowHandle, "VehicleNo");
        }
    }
}