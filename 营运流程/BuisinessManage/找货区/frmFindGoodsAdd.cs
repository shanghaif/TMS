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
    public partial class frmFindGoodsAdd : ZQTMS.Tool.BaseForm
    {
        static frmFindGoodsAdd foca;
        public int type;  //maohui20180622
        public string id;

        ///// <summary>
        ///// 编号
        ///// </summary>
        //public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}

        public frmFindGoodsAdd()
        {
            InitializeComponent();
        }

        public static frmFindGoodsAdd Get_frmFindGoodsAdd { get { if (foca == null || foca.IsDisposed) foca = new frmFindGoodsAdd(); return foca; } }

        private void frmFindGoodsAdd_Load(object sender, EventArgs e)
        {
            FindDate.DateTime = CommonClass.gcdate;
            CommonClass.SetCause(FromCause, false);
        }

        public void getdata()
        {
            if (id == "") return;
            DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_FINDGOODS_BY_ID", new List<SqlPara> { new SqlPara("Id", id) }));

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            FromCause.EditValue = ds.Tables[0].Rows[0]["FromCause"];
            FromArea.EditValue = ds.Tables[0].Rows[0]["FromArea"];
            FromWeb.EditValue = ds.Tables[0].Rows[0]["FromWeb"];
            FindMan.EditValue = ds.Tables[0].Rows[0]["FindMan"];
            FindDate.EditValue = ds.Tables[0].Rows[0]["FindDate"];
            Varieties.EditValue = ds.Tables[0].Rows[0]["Varieties"];
            Package.EditValue = ds.Tables[0].Rows[0]["Package"];
            Num.EditValue = ds.Tables[0].Rows[0]["Num"];
            Chauffer.EditValue = ds.Tables[0].Rows[0]["Chauffer"];
            VehicleNo.EditValue = ds.Tables[0].Rows[0]["VehicleNo"];
            Describe.EditValue = ds.Tables[0].Rows[0]["Describe"];
            Remark.EditValue = ds.Tables[0].Rows[0]["Remark"];
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string findMan = FindMan.Text.Trim();
            if (findMan == "")
            {
                MsgBox.ShowOK("请填写发现人!");
                FindMan.Focus();
                return;
            }
            if (type == 0)  //maohui20180622
            {
                id = Convert.ToString(Guid.NewGuid());
            }

            if (MsgBox.ShowYesNo("确定保存？") != DialogResult.Yes) return;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("Id", id));
            list.Add(new SqlPara("type", type));
            list.Add(new SqlPara("FindMan", findMan));
            list.Add(new SqlPara("FindDate", FindDate.DateTime));
            list.Add(new SqlPara("FromCause", FromCause.Text.Trim()));
            list.Add(new SqlPara("FromArea", FromArea.Text.Trim()));
            list.Add(new SqlPara("FromWeb", FromWeb.Text.Trim()));
            list.Add(new SqlPara("Varieties", Varieties.Text.Trim()));
            list.Add(new SqlPara("Package", Package.Text.Trim()));
            list.Add(new SqlPara("Num", ConvertType.ToInt32(Num.Text)));
            list.Add(new SqlPara("Chauffer", Chauffer.Text.Trim()));
            list.Add(new SqlPara("VehicleNo", VehicleNo.Text.Trim()));
            list.Add(new SqlPara("Describe", Describe.Text.Trim()));
            list.Add(new SqlPara("Remark", Remark.Text.Trim()));

            if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_FINDGOODS", list)) == 0)
            {
                return;
            }
            else  //maohui2180623
            {
                List<SqlPara> list_ZQTMS = new List<SqlPara>();
                list_ZQTMS.Add(new SqlPara("Id", id));
                list_ZQTMS.Add(new SqlPara("type", type));
                list_ZQTMS.Add(new SqlPara("FindMan", findMan));
                list_ZQTMS.Add(new SqlPara("FindDate", FindDate.DateTime));
                list_ZQTMS.Add(new SqlPara("FromCause", FromCause.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("FromArea", FromArea.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("FromWeb", FromWeb.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("Varieties", Varieties.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("Package", Package.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("Num", ConvertType.ToInt32(Num.Text)));
                list_ZQTMS.Add(new SqlPara("Chauffer", Chauffer.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("VehicleNo", VehicleNo.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("Describe", Describe.Text.Trim()));
                list_ZQTMS.Add(new SqlPara("Remark", Remark.Text.Trim()));
                if (SqlHelper.ExecteNonQuery_ZQTMS(new SqlParasEntity(OperType.Execute, "USP_ADD_FINDGOODS", list_ZQTMS)) == 0) return;
                FindMan.Text = Varieties.Text = Package.Text = Num.Text = Chauffer.Text = VehicleNo.Text = Describe.Text = Remark.Text = "";
                FindDate.DateTime = CommonClass.gcdate;
                MsgBox.ShowOK();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FromCause_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetArea(FromArea, FromCause.Text, false);
            CommonClass.SetCauseWeb(FromWeb, FromCause.Text, FromArea.Text, false);
        }

        private void FromArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetCauseWeb(FromWeb, FromCause.Text, FromArea.Text, false);
        }
    }
}