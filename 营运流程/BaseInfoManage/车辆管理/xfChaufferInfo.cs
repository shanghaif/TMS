using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class xfChaufferInfo : BaseForm
    {
        public xfChaufferInfo()
        {
            InitializeComponent();
        }
        string strchaufferno = "";
        DataTable dt2 = new DataTable();
       // string chaufferid = "";
        private void xfChaufferInfo_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
             BarMagagerOper.SetBarPropertity(bar2); 
            bindcomboboxedit();
            strchaufferno = xfChauffer.strchaufferno;
            if (strchaufferno != "")
            {
                IninChauffer();
            }
            else
            {
                this.Width = 489;
                this.ruzhiTime.EditValue = DateTime.Now;
                this.sex.SelectedIndex=0;
                this.jiashiyuanjibie.SelectedIndex = 1;
                this.kaoping.SelectedIndex = 2;
                this.state.SelectedIndex = 0;
                this.jiazhengqixian.Text = DateTime.Now.ToShortDateString();
                this.baoxianqixian.Text = DateTime.Now.ToShortDateString();
                this.dateEdit1.Text = DateTime.Now.ToShortDateString();
                this.ruzhiTime.Text = DateTime.Now.ToShortDateString();
            }
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("chaufferid", strchaufferno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CHAUFFER_NOTE", list);
            DataSet ds  = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables.Count > 0)
                dt2 = ds.Tables[0];
            this.gridControl1.DataSource = dt2;
        }
        //绑定数据到车号
        private void bindcomboboxedit()
        {
           
            this.vehicleno.Properties.Items.Clear();
            List<SqlPara> list = new List<SqlPara>();
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "V_GET_VEHICLENO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    this.vehicleno.Properties.Items.Add(ds.Tables[0].Rows[i]["vehicleno"]);
                }
            }
        }

        private void IninChauffer()
        {


            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("chaufferno", strchaufferno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_CHAUFFER_BY_CHAUFFERNO", list);
            DataSet ds = SqlHelper.GetDataSet(sps);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            this.chaufferno.Visible = false;
            this.chaufferno.Text = "编号：" + dt.Rows[0]["chaufferno"];
            this.ruzhiTime.Text =dt.Rows[0]["ruzhiTime"].ToString();
            this.chauffername.Text = dt.Rows[0]["chauffername"].ToString();
            this.sex.SelectedIndex =Convert.ToInt32( dt.Rows[0]["sex"].ToString());
            this.age.Text = dt.Rows[0]["age"].ToString();
            this.shenfenzheng.Text = dt.Rows[0]["shenfenzheng"].ToString();
            this.jiashizheng.Text = dt.Rows[0]["jiashizheng"].ToString();
            this.jiashiyuanjibie.SelectedIndex = Convert.ToInt32( dt.Rows[0]["jiashiyuanjibie"].ToString());
            this.phone.Text = dt.Rows[0]["phone"].ToString();
            this.mobilephone.Text = dt.Rows[0]["mobilephone"].ToString();
            this.address.Text = dt.Rows[0]["address"].ToString();
            this.jialin.Text = dt.Rows[0]["jialin"].ToString();
            this.jiazhengjibie.Text = dt.Rows[0]["jiazhengjibie"].ToString();
            this.jiazhengqixian.Text = dt.Rows[0]["jiazhengqixian"].ToString();
            this.baoxianxinxi.Text = dt.Rows[0]["baoxianxinxi"].ToString();
            this.baoxianqixian.Text = dt.Rows[0]["baoxianqixian"].ToString();
            this.dateEdit1.Text = dt.Rows[0]["businessterm"].ToString();        //zhengjiafeng20181008
            this.kaoping.SelectedIndex =Convert.ToInt32( dt.Rows[0]["kaoping"].ToString());
            this.state.SelectedIndex = Convert.ToInt32(dt.Rows[0]["state"].ToString());
            this.vehicleno.Text = Convert.ToString(dt.Rows[0]["vehicleno"].ToString());
            //this.chaufferid = dt.Rows[0]["chaufferno"].ToString();
            
        }

        //private static SqlConnection conn = new SqlConnection("server=.,uid=sa,pwd=123");
        //public int ExecuteCommand(string storedProcedure, params SqlParameter[] values)
        //{
        //    try
        //    {
        //        conn.Open();
        //        SqlCommand cmd = new SqlCommand(storedProcedure, conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddRange(values);
        //        int result = cmd.ExecuteNonQuery();
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.chauffername.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入驾驶员姓名", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (this.age.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入年龄", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.age.Text.Trim() != "" && !StringHelper.IsNumberId(this.age.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效年龄", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.jialin.Text.Trim() == "")
            {
                XtraMessageBox.Show("请输入驾龄", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.jialin.Text.Trim() != "" && !StringHelper.IsNumberId(this.jialin.Text.Trim()))
            {
                XtraMessageBox.Show("请输入有效驾龄", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string chauffername=this.chauffername.Text.Trim();
            string sex = this.sex.EditValue.ToString();
            string age = this.age.Text.Trim();
            string shenfenzheng = this.shenfenzheng.Text.Trim();
            string jiashizheng = this.jiashizheng.Text.Trim();
            string jiashiyuanjibie = this.jiashiyuanjibie.EditValue.ToString();
            string phone = this.phone.Text.Trim();
            string mobilephone = this.mobilephone.Text.Trim();
            string address = this.address.Text.Trim();
            string jialin = this.jialin.Text.Trim();
            string jiazhengjibie = this.jiazhengjibie.Text.Trim();
            DateTime jiazhengqixian = this.jiazhengqixian.DateTime;
            string baoxianxinxi = this.baoxianxinxi.Text.Trim();
            DateTime baoxianqixian = this.baoxianqixian.DateTime;
            string kaoping = this.kaoping.EditValue.ToString();
            string state = this.state.EditValue.ToString();
            DateTime ruzhiTime = this.ruzhiTime.DateTime;
            string vehicle = this.vehicleno.Text.Trim();
            DateTime businessterm = this.dateEdit1.DateTime;

            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("chaufferno", strchaufferno));
            list.Add(new SqlPara("chauffername", chauffername));
            list.Add(new SqlPara("sex", sex));
            list.Add(new SqlPara("age", age));
            list.Add(new SqlPara("shenfenzheng", shenfenzheng));
            list.Add(new SqlPara("jiashizheng", jiashizheng));
            list.Add(new SqlPara("jiashiyuanjibie", jiashiyuanjibie));
            list.Add(new SqlPara("phone", phone));
            list.Add(new SqlPara("mobilephone", mobilephone));
            list.Add(new SqlPara("address", address));
            list.Add(new SqlPara("jialin", jialin));
            list.Add(new SqlPara("jiazhengjibie", jiazhengjibie));
            list.Add(new SqlPara("jiazhengqixian", jiazhengqixian));
            list.Add(new SqlPara("baoxianxinxi", baoxianxinxi));
            list.Add(new SqlPara("baoxianqixian", baoxianqixian));
            list.Add(new SqlPara("kaoping", kaoping));
            list.Add(new SqlPara("state", state));
            list.Add(new SqlPara("ruzhiTime", ruzhiTime));
            list.Add(new SqlPara("vehicleno", vehicle));
            list.Add(new SqlPara("businessterm", businessterm));
            
            string procname = "";
            if (strchaufferno != "")
            {
                procname = "V_UPDATE_CHAUFFER";
            }
            else
            {
                procname = "USP_ADD_CHAUFFER";
            }

             SqlParasEntity sps = new SqlParasEntity(OperType.Execute,procname, list);
            int n =SqlHelper.ExecteNonQuery(sps) ;
              MsgBox.ShowOK();
              if (strchaufferno == "")
              {
                  clear();
              }
              else {
                  saveproject(this.gridView1);
              }
        }

        private void clear()
        {
            this.ruzhiTime.Text = DateTime.Now.ToShortDateString();
            this.vehicleno.Text = "";
            this.chauffername.Text = "";
            this.age.Text = "";
            this.shenfenzheng.Text = "";
            this.phone.Text = "";
            this.jialin.Text = "";
            this.mobilephone.Text = "";
            this.jiashizheng.Text = "";
            this.jiazhengqixian.Text = "";
            this.baoxianxinxi.Text = "";
            this.baoxianqixian.Text = "";
            this.address.Text = "";
        }
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DataRow row = dt2.NewRow();
            row["inputtime"] = DateTime.Now;
            row["createby"] = CommonClass.UserInfo.UserName;
            dt2.Rows.Add(row);
            this.gridControl1.DataSource = dt2;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (gridView1.SelectedRowsCount == 0)
            {
                return;
            }
            this.gridView1.DeleteRow(this.gridView1.FocusedRowHandle);
        }

        private void saveproject(GridView gv)
        {
            List<SqlPara> list = new List<SqlPara>();
            list.Add(new SqlPara("chaufferno", strchaufferno));
            SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "DELETE_CHAUFFER_NOTE", list);
            SqlHelper.ExecteNonQuery(sps);
            if (this.gridView1.RowCount == 0)
            {
                return;
            }
            try
            {
                gv.PostEditor();
                gv.UpdateCurrentRow();
                for (int i = 0; i < gv.RowCount; i++)
                {
                    string inputtime = gv.GetRowCellValue(i, "inputtime").ToString();
                    string remark = gv.GetRowCellValue(i, "remark").ToString();
                    string score = gv.GetRowCellValue(i, "score").ToString();
                    string createby = gv.GetRowCellValue(i, "createby").ToString();
                    List<SqlPara> list1 = new List<SqlPara>();
                    list1.Add(new SqlPara("inputtime", inputtime));
                    list1.Add(new SqlPara("remark", remark));
                    list1.Add(new SqlPara("score", score));
                    list1.Add(new SqlPara("createby", createby));
                    list1.Add(new SqlPara("strchaufferno", strchaufferno));

                    SqlParasEntity sps1 = new SqlParasEntity(OperType.Execute, "V_ADD_CHAUFFER_NOTE", list1);
                    SqlHelper.ExecteNonQuery(sps1);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

    }
}