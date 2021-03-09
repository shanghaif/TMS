using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_pt_company_add : BaseForm
    {
        public DevExpress.XtraGrid.Views.Grid.GridView gv;
        public DataSet ds = new DataSet();
        public string id = "";
        public int i;  //毛慧20171204

        public w_pt_company_add()
        {
            InitializeComponent();
        }

        private void clear()
        {
            edgsqc.Text = "";

            edgsjc.Text = "";

            edgsfr.Text = "";
            edfrtel.Text = "";

            edgstel.Text = "";
            edemail.Text = "";

            edgsaddr.Text = "";
            txtFeeSubNode.Text = "";

            id = "";
            gv = null;
            edgscode.Enabled = true;
            edgscode.Text = "";
            edgsqc.Focus();
            textEdit1.Text = "";
        }

        private void save()
        {
            string gsqc = edgsqc.Text.Trim();

            string gsjc = edgsjc.Text.Trim();
            string gscode = edgscode.Text.Trim();

            string gstype = edgstype.Text.Trim();
            string gsarea = edgscause.Text.Trim();

            string gsfr = edgsfr.Text.Trim();
            string frtel = edfrtel.Text.Trim();

            string gstel = edgstel.Text.Trim();
            string email = edemail.Text.Trim();
            string property = Property.Text.Trim(); //zb20190508

            string gsaddr = edgsaddr.Text.Trim();

            if (gsqc == "" || gsjc == "" || gscode == "")
            {
                XtraMessageBox.Show("公司全称、简称和公司代码必须填写!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ds.Tables.Count > 0 && gv == null)
            {
                DataRow[] drs = ds.Tables[0].Select(string.Format("gsqc='{0}' or gsjc='{1}'", gsqc, gsjc));
                if (drs.Length > 0)
                {
                    MsgBox.ShowOK("您填写的公司全称或简称已存在，请核查!");
                    return;
                }
            }

            if (gsarea == "")
            {
                XtraMessageBox.Show("必须选择该公司所在的操作中心名称!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                edgscause.ShowPopup();
                return;
            }
            //zb20190508
            //if (string.IsNullOrEmpty(property))
            //{
            //    MsgBox.ShowOK("属性为必选项,请选择");
            //    return;
            //}
            ////20190508 判断公司属于线路还是平台
            //if (gscode == "239" || gscode == "488")
            //{
            //    if (property !="平台")
            //    {
            //        MsgBox.ShowOK("该公司属性为平台，请重新选择");
            //        return;
            //    }
            //}
            //else
            //{
            //    if (property !="线路")
            //    {
            //        MsgBox.ShowOK("该公司属性为线路,请重新选择");
            //        return;
            //    }
            //}


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("id", id == "" ? Guid.NewGuid() : new Guid(id)));
            list.Add(new SqlPara("gsqc", edgsqc.Text.Trim()));
            list.Add(new SqlPara("gsjc", edgsjc.Text.Trim()));
            list.Add(new SqlPara("gscode", edgscode.Text.Trim()));
            list.Add(new SqlPara("gstype", edgstype.Text.Trim()));
            list.Add(new SqlPara("gscause", edgscause.Text.Trim()));

            list.Add(new SqlPara("gsfr", edgsfr.Text.Trim()));
            list.Add(new SqlPara("frtel", edfrtel.Text.Trim()));
            list.Add(new SqlPara("gstel", edgstel.Text.Trim()));
            list.Add(new SqlPara("email", edemail.Text.Trim()));
            list.Add(new SqlPara("gsaddr", edgsaddr.Text.Trim()));
            list.Add(new SqlPara("gslabel", edgslabel.Text.Trim()));//公司标签 
            list.Add(new SqlPara("gsenvelope", edgsenvelope.Text.Trim()));//公司信封
            list.Add(new SqlPara("gsState", edgsState.Checked ? 1 : 0));//是否自动生成运单 
            list.Add(new SqlPara("gsControl", edgsControl.Checked ? 1 : 0));//是否对月结客户管控

            list.Add(new SqlPara("Transprotocol", txtTransprotocol.Text.Trim()));
            list.Add(new SqlPara("DepartList", txtDepartList.Text.Trim()));
            list.Add(new SqlPara("LoadList", txtLoadList.Text.Trim()));
            list.Add(new SqlPara("ShortConList", txtShortConList.Text.Trim()));
            list.Add(new SqlPara("BookNote", txtBookNote.Text.Trim()));
            list.Add(new SqlPara("FeeSubNode", txtFeeSubNode.Text.Trim()));//HJ扣费节点 20180322
            list.Add(new SqlPara("MiddleList", textEdit1.Text.Trim()));
            list.Add(new SqlPara("IsAutomaticFangHuo", ck_IsAutomaticFangHuo.Checked == true ? 1 : 0));//是否控货转放货自动审核
            list.Add(new SqlPara("IsAutomaticGaiDan", IsAutomaticGaiDan.Checked == true ? 1 : 0));//是否改单申请自动执行
            list.Add(new SqlPara("IsAutomaticzhixing", IsAutomaticzhixing.Checked == true ? 1 : 0));//是否费用异动申请自动审批和执行
            //list.Add(new SqlPara("IsAutomaticSign", IsAutomaticSign.Checked == true ? 1 : 0));//是否回单寄出自动作货物签收

            list.Add(new SqlPara("IsReceiptPayChecked", IsReceiptPay.Checked == true ? 1 : 0));//是否勾选回单付管控
            list.Add(new SqlPara("IsLmsBasics", 1));//是否LMS干线基础系统录入
            list.Add(new SqlPara("gsNotStreet", gsNotStreet.Checked ? 1 : 0));//是否取消街道必填限制hj20180904
            list.Add(new SqlPara("gsDenominatedFee", gsDenominatedFee.Checked ? 1 : 0));//是否对客户计价管控hj20181031
            list.Add(new SqlPara("IsGsNotStreet_self", gsNotStreet_self.Checked ? 1 : 0));//是否对客户计价管控hj20181031
            list.Add(new SqlPara("IsGsNotStreet_send", gsNotStreet_send.Checked ? 1 : 0));//是否对客户计价管控hj20181031
            list.Add(new SqlPara("token", this.txt_token.Text.Trim()));
            list.Add(new SqlPara("Property", this.Property.Text.Trim()));  //zb20190507 属性

            list.Add(new SqlPara("isPlatform", IsPlatform.Text.Trim() == "是" ? "1" : "0"));
            list.Add(new SqlPara("platformName", PlatformName.Text.Trim()));
            list.Add(new SqlPara("IsAutomaticGaiDanSP", IsAutomaticGaiDanSP.Checked == true ? 1 : 0));//是否改单一键审核、审批
            list.Add(new SqlPara("IsAutomaticGaiDanZX", IsAutomaticGaiDanZX.Checked == true ? 1 : 0));//是否改单审批后自动执行

            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_COMPANY", list);
            if (SqlHelper.ExecteNonQuery(sps) > 0)
            {
                clear();
                MsgBox.ShowOK();
            }
        }

        private void w_company_add_Load_1(object sender, EventArgs e)
        {
            BarMagagerOper.SetBarPropertity(bar1);
            CommonClass.SetCause(edgscause, false);
            if (gv == null)
            {
                try
                {
                    //List<SqlPara> list = new List<SqlPara>();
                    //SqlParasEntity spe = new SqlParasEntity(OperType.Query, "USP_GET_COMPANYID", list);
                    //DataSet ds = SqlHelper.GetDataSet(spe);
                    //edgscode.Text = ds.Tables[0].Rows[0]["companyid"].ToString();
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
            if (gv != null)
            {
                int rowhandle = gv.FocusedRowHandle;
                if (rowhandle >= 0)
                {
                    edgscode.Enabled = false;

                    edgsqc.EditValue = gv.GetRowCellValue(rowhandle, "gsqc");

                    edgsjc.EditValue = gv.GetRowCellValue(rowhandle, "gsjc");
                    edgscode.EditValue = gv.GetRowCellValue(rowhandle, "companyid");

                    edgstype.EditValue = gv.GetRowCellValue(rowhandle, "gstype");
                    edgscause.EditValue = gv.GetRowCellValue(rowhandle, "gscause");

                    edgsfr.EditValue = gv.GetRowCellValue(rowhandle, "gsfr");
                    edfrtel.EditValue = gv.GetRowCellValue(rowhandle, "frtel");

                    edgstel.EditValue = gv.GetRowCellValue(rowhandle, "gstel");
                    edemail.EditValue = gv.GetRowCellValue(rowhandle, "email");

                    edgsaddr.EditValue = gv.GetRowCellValue(rowhandle, "gsaddr");
                    edgslabel.EditValue = gv.GetRowCellValue(rowhandle, "gslabel");
                    edgsenvelope.EditValue = gv.GetRowCellValue(rowhandle, "gsenvelope");
                    edgsState.Checked = gv.GetRowCellValue(rowhandle, "gsState").ToString() == "是" ? edgsState.Checked = true : edgsState.Checked = false;
                    edgsControl.Checked = gv.GetRowCellValue(rowhandle, "gsControl").ToString() == "是" ? edgsControl.Checked = true : edgsControl.Checked = false;

                    txtTransprotocol.EditValue = gv.GetRowCellValue(rowhandle, "Transprotocol");
                    txtDepartList.EditValue = gv.GetRowCellValue(rowhandle, "DepartList");
                    txtLoadList.EditValue = gv.GetRowCellValue(rowhandle, "LoadList");
                    txtShortConList.EditValue = gv.GetRowCellValue(rowhandle, "ShortConList");
                    txtBookNote.EditValue = gv.GetRowCellValue(rowhandle, "BookNote");
                    txtFeeSubNode.EditValue = gv.GetRowCellValue(rowhandle, "FeeSubNode");
                    textEdit1.EditValue = gv.GetRowCellValue(rowhandle, "MiddleList"); //maohui20180324
                    IsAutomaticGaiDan.Checked = gv.GetRowCellValue(rowhandle, "IsAutomaticGaiDan").ToString() == "是" ? IsAutomaticGaiDan.Checked = true : IsAutomaticGaiDan.Checked = false;
                    ck_IsAutomaticFangHuo.Checked = gv.GetRowCellValue(rowhandle, "IsAutomaticFangHuo").ToString() == "是" ? ck_IsAutomaticFangHuo.Checked = true : ck_IsAutomaticFangHuo.Checked = false;
                    gsNotStreet.Checked = gv.GetRowCellValue(rowhandle, "gsNotStreet").ToString() == "是" ? gsNotStreet.Checked = true : gsNotStreet.Checked = false;
                    IsAutomaticzhixing.Checked = gv.GetRowCellValue(rowhandle, "IsAutomaticzhixing").ToString() == "是" ? IsAutomaticzhixing.Checked = true : IsAutomaticzhixing.Checked = false;//tuxin20180927
                    IsReceiptPay.Checked = gv.GetRowCellValue(rowhandle, "IsReceiptPayChecked").ToString() == "是" ? IsReceiptPay.Checked = true : IsReceiptPay.Checked = false;
                   
                    gsDenominatedFee.Checked = gv.GetRowCellValue(rowhandle, "gsDenominatedFee").ToString() == "是" ? gsDenominatedFee.Checked = true : gsDenominatedFee.Checked = false;//hj20181031
                    gsNotStreet_self.Checked = gv.GetRowCellValue(rowhandle, "IsGsNotStreet_self").ToString() == "是" ? gsNotStreet_self.Checked = true : gsNotStreet_self.Checked = false;//hj20181031
                    gsNotStreet_send.Checked = gv.GetRowCellValue(rowhandle, "IsGsNotStreet_send").ToString() == "是" ? gsNotStreet_send.Checked = true : gsNotStreet_send.Checked = false;//hj20181031
                    IsAutomaticGaiDanSP.Checked = gv.GetRowCellValue(rowhandle, "IsAutomaticGaiDanSP").ToString() == "是" ? IsAutomaticGaiDanSP.Checked = true : IsAutomaticGaiDanSP.Checked = false;
                    IsAutomaticGaiDanZX.Checked = gv.GetRowCellValue(rowhandle, "IsAutomaticGaiDanZX").ToString() == "是" ? IsAutomaticGaiDanZX.Checked = true : IsAutomaticGaiDanZX.Checked = false;
                    this.txt_token.EditValue = gv.GetRowCellValue(rowhandle, "token");
                    string isplat = gv.GetRowCellValue(rowhandle, "IsPlatform").ToString() == "是" ? "是" : "否";
                    IsPlatform.EditValue = isplat;
                    PlatformName.EditValue = gv.GetRowCellValue(rowhandle, "PlatformName");
                }
            }
            if (i == 1)   //毛慧20171204
            {
                simpleButton1.Visible = false;
            }
          
            
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            clear();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            save();
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)  //毛慧20171204
        {
            try
            {
                if (MsgBox.ShowYesNo("是否同步权限？\r\r请确认！") != DialogResult.Yes)
                {
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("gscode", edgscode.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_Company_RightMenu_New", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}