using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using DevExpress.XtraGrid.Columns;
using ZQTMS.SqlDAL;
using ZQTMS.Common;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmVehicleWithholdLead : BaseForm
    {
        public frmVehicleWithholdLead()
        {
            InitializeComponent();
        }

        private void barbtn_import_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "车辆代扣款记录导入";
            ofd.Filter = "Microsoft Execl文件|*.xls;*.xlsx";
            ofd.FilterIndex = 1;
            ofd.DefaultExt = "xls";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!ofd.SafeFileName.EndsWith(".xls") && !ofd.SafeFileName.EndsWith(".xlsx"))
                {
                    XtraMessageBox.Show("请选择Excel文件！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!ofd.CheckFileExists)
                {
                    XtraMessageBox.Show("文件不存在，请重新选择！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DataSet ds = DsExecl(ofd.FileName);
                if (ds.Tables[0] == null)
                    return;
                int i = 1;
                foreach (DataColumn columns in ds.Tables[0].Columns)
                {

                    if (columns.ColumnName.IndexOf('-') > 0)
                    {
                        GridColumn column = new GridColumn();
                        column.Caption = columns.ColumnName;
                        column.FieldName = columns.ColumnName;
                        column.Name = "gridColumnDt" + i;
                        column.Visible = true;
                        column.VisibleIndex = 6 + i;
                        column.Width = 80;
                        myGridView1.Columns.Add(column);
                        i++;
                    }
                }
                if (ds != null)
                {
                    myGridControl1.DataSource = ds.Tables[0];

                }

            }
        }

        private DataSet DsExecl(string filePath)
        {
            //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接可以兼容2003和2007
            //string str = "Provider = Microsoft.Jet.OLEDB.12.0; Data Source = " + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\""; //此连接必须要安装2007
            string str = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\""; //此连接智能读取2003格式
            OleDbConnection Conn = new OleDbConnection(str);
            try
            {
                Conn.Open();
                System.Data.DataTable dt = Conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                string tablename = "", sql = "";
                w_import_select_table wi = new w_import_select_table();
                wi.dt = dt;
                if (wi.ShowDialog() != DialogResult.Yes)
                { return null; }
                tablename = wi.listBoxControl1.Text.Trim();
                sql = "select * from [" + tablename + "]";

                OleDbDataAdapter da = new OleDbDataAdapter(sql, Conn);
                DataSet ds = new DataSet();
                da.Fill(ds, tablename);

                try
                {
                    //ds.Tables[0].Columns.Add("balance_money_Auto", typeof(float));
                    SetColumnName(ds.Tables[0].Columns);
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex, "转换失败!\r\n请检查EXCEL列头是否与模板一致！");
                    return null;
                }
                return ds;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "加载司机奖罚记录失败");
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open) Conn.Close();
            }
        }

        private void SetColumnName(DataColumnCollection c)
        {
            try
            {
                foreach (DataColumn dc in c)
                {
                    dc.ColumnName = dc.ColumnName.Trim();
                }

                c["车牌号"].ColumnName = "CarNo";
                c["司机姓名"].ColumnName = "DriverName";
                c["所属月份"].ColumnName = "belongMonth";
                c["司机电话"].ColumnName = "DriverPhone";
                c["代扣类型"].ColumnName = "WithholdType";
                c["代扣金额"].ColumnName = "WithholdMoney";
                c["返款户名"].ColumnName = "RefundAccountName";
                c["返款账号"].ColumnName = "AccountNO";
                c["返款开户行"].ColumnName = "BankName";
                c["摘要"].ColumnName = "Remark";
                c["车辆用途"].ColumnName = "VehicleUse";
                c["加油时间"].ColumnName = "oilDate";
                c["申报方式"].ColumnName = "useFee";
                c["付款时间"].ColumnName = "BusinessDate";//jl20180911
                c["省份"].ColumnName = "Province";
                c["城市"].ColumnName = "City";

                //c["发车日期"].ColumnName = "WebDate";
                //c["选项类型"].ColumnName = "cbMoneyType";
            }
            catch (Exception ex)
            {
                //MsgBox.ShowException(ex);
                MsgBox.ShowError("请检查自己导入的文档是否为标准格式");
            }
        }

        //验证
        private bool validate()
        {
            for (int i = 0; i < myGridView1.RowCount; i++)
            {


                string WithholdType = GridOper.GetRowCellValueString(myGridView1, i, "WithholdType");
                string WithholdMoney = GridOper.GetRowCellValueString(myGridView1, i, "WithholdMoney");
                string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
                string negPattern = @"^\-[0-9]+(.[0-9]{1,2})?$";//验证负数正则
                string carNo = GridOper.GetRowCellValueString(myGridView1, i, "CarNo");//车号
                string DriverName = GridOper.GetRowCellValueString(myGridView1, i, "DriverName");//司机
                string DriverPhone = GridOper.GetRowCellValueString(myGridView1, i, "DriverPhone");//司机
                string belongMonth = GridOper.GetRowCellValueString(myGridView1, i, "belongMonth");
                // string WithholdType = GridOper.GetRowCellValueString(myGridView1, i, "WithholdType");//司机
                //string WithholdMoney = GridOper.GetRowCellValueString(myGridView1, i, "WithholdMoney");//司机
                string RefundAccountName = GridOper.GetRowCellValueString(myGridView1, i, "RefundAccountName");//
                string AccountNO = GridOper.GetRowCellValueString(myGridView1, i, "AccountNO");//
                string BankName = GridOper.GetRowCellValueString(myGridView1, i, "BankName");//
                string Remark = GridOper.GetRowCellValueString(myGridView1, i, "Remark");//司机
                string VehicleUse = GridOper.GetRowCellValueString(myGridView1, i, "VehicleUse");//车辆用途
                string BusinessDate = GridOper.GetRowCellValueString(myGridView1, i, "BusinessDate");//业务发生日期jl20180911
                string useFee = GridOper.GetRowCellValueString(myGridView1, i, "useFee");//车辆用途
                string Province = GridOper.GetRowCellValueString(myGridView1, i, "Province");//车辆用途
                string City = GridOper.GetRowCellValueString(myGridView1, i, "City");//车辆用途

                //2018-10-11 hs
                if (useFee != "返款" && useFee !="抵账")
                {
                    MessageBox.Show("申报方式错误，请检查！");
                    return false;
                }

                if (WithholdType == "" || WithholdMoney == "" || carNo == "" || DriverName == "" || DriverPhone == "" || Remark == "" || VehicleUse == "")
                {
                    MessageBox.Show("车牌号，司机姓名，司机电话，代扣类型，代扣金额，\r\n返款户名，返款账号，返款开户行，摘要，车辆用途\r\n不能为空请检查！", "错误");
                    return false;
                }
                if (VehicleUse.Trim() != "长途" && VehicleUse.Trim() != "短途")
                {
                    MessageBox.Show("车辆用途只能是长途和短途，请检查！", "错误");
                    return false;
                }
                if ((RefundAccountName == ""
                  || AccountNO == "" || BankName == "") && useFee == "返款")
                {
                    MessageBox.Show("申报方式为返款，返款户名，返款账号，返款开户行不能为空！", "错误");
                    return false;
                }


                //判断状态
                if (WithholdType == "奖励支出" && !Regex.IsMatch(WithholdMoney, negPattern))
                {
                    MessageBox.Show("奖励支出必须为负数", "错误");
                    return false;
                }
                if (WithholdType != "奖励支出" && !Regex.IsMatch(WithholdMoney, posPattern))
                {
                    MessageBox.Show("非奖励支出类型必须为正数", "错误");
                    return false;
                }
                if (!Regex.IsMatch(WithholdMoney, negPattern) && !Regex.IsMatch(WithholdMoney, posPattern))
                {
                    MessageBox.Show("奖励金额格式不正确，必须输入数字类型", "错误");
                    return false;
                }
                if (WithholdType == "油料费")
                {
                    MessageBox.Show("检测到有代扣类型为【油料费】的数据，不允许在此模块导入该类型的数据！", "错误");
                    return false;
                }
                //string WithholdType = GridOper.GetRowCellValueString(myGridView1, i, "WithholdType");
                if (WithholdType != "车贷" && WithholdType != "挂靠费" && WithholdType != "社保费" && WithholdType != "保险费" && WithholdType != "油料费" && WithholdType != "油卡" && WithholdType != "GPS服务费" && WithholdType != "车身广告费"
                    && WithholdType != "年审费" && WithholdType != "罚款" && WithholdType != "代办证件费" && WithholdType != "ETC费" && WithholdType != "修理费" && WithholdType != "其他代扣费" && WithholdType != "代扣罚款" && WithholdType != "奖励支出"
                    && WithholdType != "轮胎费" && WithholdType != "管理费")
                {
                    MessageBox.Show("代扣类型不正确，请输入正确的代扣类型！", "错误");
                    return false;
                }



            }
            return true;
        }

        private void barbtn_Upload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            myGridView1.PostEditor();
            // int flag = 0;
            if (myGridView1.RowCount == 0)
            {
                return;
            }

            if (myGridView1.RowCount > 500)
            {
                XtraMessageBox.Show("数据导入不能超过500条！", "文件导入失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //yzw限制导入格式
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                string a = myGridView1.GetRowCellValue(i, "belongMonth").ToString();
                if (a.Length != 7)
                {
                    MsgBox.ShowOK("月份格式不正确!格式:2018-07");
                    return;
                }
                if (!(a.Contains("-")))
                {
                    MsgBox.ShowOK("月份格式不正确!格式:2018-07");
                    return;

                }
                int b = ConvertType.ToInt32(a.Substring(5, 2));
                if (b > 12)
                {
                    MsgBox.ShowOK("月份有误!");
                    return;
                }
            }
            if (validate())
            {
                DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
                if (dt.Columns.Contains("序号"))
                {
                    dt.Columns.Remove("序号");
                    dt.AcceptChanges();
                }
                string a = DateTime.Now.ToString("yyyyMMddHHmmss");
                dt.Columns.Add("CertificateNo", typeof(string),a);
                DataRow dr = dt.NewRow();
                string msg = "";
                if (CommonClass.BasUpload(dt, "USP_ADD_VEHICLEWITHOLD_mult2", out msg))
                {

                    MsgBox.ShowOK(msg);
                    this.Close();
                }
                else
                {
                    MsgBox.ShowError(msg);
                }
            }






            //try
            //{
            //    if (validate())
            //    {
            //        DataTable dt = ((System.Data.DataView)(myGridView1.DataSource)).Table;
            //        //dt.Columns.Remove("序号");
            //        dt.AcceptChanges();
            //        DataTable newDt = new DataTable();
            //        newDt.Columns.Add("CarNo", typeof(string));
            //        newDt.Columns.Add("DriverName", typeof(string));
            //        newDt.Columns.Add("DriverPhone", typeof(string));
            //        newDt.Columns.Add("belongMonth", typeof(string));
            //        newDt.Columns.Add("WithholdType", typeof(string));
            //        newDt.Columns.Add("WithholdMoney", typeof(decimal));
            //        newDt.Columns.Add("Remark", typeof(string));
            //        newDt.Columns.Add("CertificateNo", typeof(string));
            //        newDt.Columns.Add("RefundAccountName", typeof(string));
            //        newDt.Columns.Add("AccountNO", typeof(string));
            //        newDt.Columns.Add("BankName", typeof(string));
            //        newDt.Columns.Add("VehicleUse", typeof(string));
            //        newDt.Columns.Add("oilDate", typeof(DateTime));

            //        newDt.Columns.Add("useFee", typeof(string));
            //        newDt.Columns.Add("BusinessDate", typeof(DateTime));
            //        newDt.Columns.Add("Province", typeof(string));
            //        newDt.Columns.Add("City", typeof(string));
            //        for (int i = 0; i < dt.Rows.Count; i++)
            //        {
            //            DataRow dr = newDt.NewRow();
            //            // List<SqlPara> list = new List<SqlPara>();
            //            dr["CarNo"] = GridOper.GetRowCellValueString(myGridView1, i, "CarNo");
            //            dr["DriverName"] = GridOper.GetRowCellValueString(myGridView1, i, "DriverName");
            //            dr["belongMonth"] = GridOper.GetRowCellValueString(myGridView1, i, "belongMonth");
            //            dr["DriverPhone"] = GridOper.GetRowCellValueString(myGridView1, i, "DriverPhone");
            //            dr["WithholdType"] = GridOper.GetRowCellValueString(myGridView1, i, "WithholdType");
            //            dr["WithholdMoney"] = Convert.ToDecimal(GridOper.GetRowCellValueString(myGridView1, i, "WithholdMoney"));
            //            dr["Remark"] = GridOper.GetRowCellValueString(myGridView1, i, "Remark");
            //            dr["RefundAccountName"] = GridOper.GetRowCellValueString(myGridView1, i, "RefundAccountName");
            //            dr["AccountNO"] = GridOper.GetRowCellValueString(myGridView1, i, "AccountNO");
            //            dr["BankName"] = GridOper.GetRowCellValueString(myGridView1, i, "BankName");
            //            dr["CertificateNo"] = GridOper.GetRowCellValueString(myGridView1, i, DateTime.Now.ToString());
            //            dr["VehicleUse"] = GridOper.GetRowCellValueString(myGridView1, i, "VehicleUse");
            //            dr["oilDate"] = DBNull.Value;
            //            dr["useFee"] = GridOper.GetRowCellValueString(myGridView1, i, "useFee");
            //            dr["BusinessDate"] = DateTime.Now;

            //            dr["Province"] = GridOper.GetRowCellValueString(myGridView1, i, "Province");
            //            dr["City"] = GridOper.GetRowCellValueString(myGridView1, i, "City");


            //            newDt.Rows.Add(dr);
            //            // list.Add(new SqlPara("CarNo", CarNo));
            //            // list.Add(new SqlPara("DriverName", DriverName));
            //            // list.Add(new SqlPara("DriverPhone", DriverPhone));
            //            // list.Add(new SqlPara("WithholdType", WithholdType));
            //            // list.Add(new SqlPara("WithholdMoney", WithholdMoney));
            //            // list.Add(new SqlPara("Remark", Remark));
            //            //list.Add(new SqlPara("CauseName", CommonClass.UserInfo.CauseName));
            //            //list.Add(new SqlPara("AreaName", CommonClass.UserInfo.AreaName));
            //            //list.Add(new SqlPara("WebName", CommonClass.UserInfo.WebName));
            //            //list.Add(new SqlPara("RegisterMan", CommonClass.UserInfo.UserName));
            //            // list.Add(new SqlPara("OperDate", DateTime.Now));
            //            // list.Add(new SqlPara("CertificateNo", DateTime.Now.ToString("yyyyMMddHHmmss")));
            //            //if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_VEHICLEWITHOLD", list)) > 0)
            //            //{
            //            //    flag = 1;
            //            //}
            //        }
            //        string msg = "";
            //        List<SqlPara> list = new List<SqlPara>();
            //        list.Add(new SqlPara("Tb", newDt));
            //        SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_VEHICLEWITHOLD_mult", list);
            //        int k = SqlHelper.ExecteNonQuery(sps);
            //        if (k > 0)
            //        {

            //            MsgBox.ShowOK("批量上传成功");
            //            this.Close();
            //        }
            //    }
                // if (flag == 1)
                // {
                // MsgBox.ShowOK();
                // this.Close();
                //  }
            //}
            //catch (Exception ex)
            //{
            //    MsgBox.ShowException(ex);
            //}
        }

        /// <summary>
        /// 是否为时间类型的数据
        /// </summary>
        /// <param name="str">用于判断的目标字符串</param>
        /// <returns></returns>
        public static bool IsDateTime(String str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                DateTime temp;

                return DateTime.TryParse(str, out temp);
            }
            else
            {
                return false;
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void frmVehicleWithholdLead_Load(object sender, EventArgs e)
        {
            CommonClass.GetGridViewColumns(myGridView1, false);
            GridOper.SetGridViewProperty(myGridView1);
        }
    }
}
