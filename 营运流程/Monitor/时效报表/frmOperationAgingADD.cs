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
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmOperationAgingADD : BaseForm
    {
        public string id1 = "", startweb1 = "", endweb1 = "", runtime1 = "", remarks1="";
        public string startSite = "", endSite = "";
        //2018.1.11wbw    
        public string StandardDepartureTime2 = "", Shift2 = "", Models2 = "", FlatStandardTime2 = "", FlatStandardArrivalTime2 = "", StandardArrivalTime2 = "", Kilometre2 = "";    

        public int flag;
        string strWebs = "";
        public frmOperationAgingADD()
        {
            InitializeComponent();
        }
       
        private void frmOperationAgingADD_Load(object sender, EventArgs e)
        {
            //CommonClass.SetCauseWeb(comboBoxEdit1, "全部", "全部", false);           
            //CommonClass.SetCauseWeb(comboBoxEdit2, "全部", "全部", false);
            CommonClass.SetSite(cbbStartSite, false);
            CommonClass.SetSite(cbbEndSite,false);
            strWebs = GetSites();
            if (cbbStartSite.Properties.Items.Contains("嘉定"))
                cbbStartSite.Properties.Items.Remove("嘉定");

            if (cbbEndSite.Properties.Items.Contains("嘉定"))
                cbbEndSite.Properties.Items.Remove("嘉定");

            if (cbbStartSite.Properties.Items.Contains("永康"))
                cbbStartSite.Properties.Items.Remove("永康");

            if (cbbEndSite.Properties.Items.Contains("永康"))
                cbbEndSite.Properties.Items.Remove("永康");

            
            
            if (!string.IsNullOrEmpty(id1))
            {
                cbbStartSite.Text = startSite;
                cbbEndSite.Text = endSite;
                cbbStartWeb.Text = startweb1;
                cbbEndWeb.Text = endweb1;
                textEdit3.Text = runtime1;
                textEdit1.Text = remarks1;
                //2018.1.11wbw
                StandardDepartureTime1.Text = StandardDepartureTime2;
                Shift1.Text = Shift2;
                Models1.Text = Models2;
                FlatStandardTime1.Text = FlatStandardTime2;
                FlatStandardArrivalTime1.Text = FlatStandardArrivalTime2;
                StandardArrivalTime1.Text = StandardArrivalTime2;
                Kilometre1.Text = Kilometre2;
                
              
            }



        }

        private string GetSites()
        {
            try
            {
                //获取所有指定的目的网点
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfo_All");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("没有任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                DataRow[] dr = ds.Tables[0].Select(string.Format("GRCode='{0}'", 205));
                if (dr == null || dr.Length == 0)
                {
                    XtraMessageBox.Show("没有找到任何指定目的网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return dr[0]["SiteNames"].ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string posPattern = @"^[0-9]+(.[0-9]{1,2})?$";//验证正数正则
            if (cbbStartSite.Text.Trim() == "")
            {
                MsgBox.ShowOK("始发站点不能为空！");
                return;
            }
            if (cbbEndSite.Text.Trim() == "")
            {
                MsgBox.ShowOK("目的站点不能为空！");
                return;
            }
            if (textEdit3.Text.Trim() == "")
            {
                MsgBox.ShowOK("标准运行时间不能为空！");
                return;
            }
            if (!Regex.IsMatch(textEdit3.Text.Trim(), posPattern))
            {
                MsgBox.ShowOK("标准运行时间格式不正确，必须是数值类型！");
                return;
            }
            if (flag == 1)
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("id", id1));
                    list.Add(new SqlPara("startweb", cbbStartWeb.Text.Trim()));
                    list.Add(new SqlPara("endweb", cbbEndWeb.Text.Trim()));
                    list.Add(new SqlPara("runtime", textEdit3.Text.Trim()));
                    list.Add(new SqlPara("remarks", textEdit1.Text.Trim()));
                    list.Add(new SqlPara("startSite", cbbStartSite.Text.Trim()));
                    list.Add(new SqlPara("endSite", cbbEndSite.Text.Trim()));
                    //2018.1.11wbw
                    list.Add(new SqlPara("StandardDepartureTime", StandardDepartureTime1.Text.Trim()));
                    list.Add(new SqlPara("Shift", Shift1.Text.Trim()));
                    list.Add(new SqlPara("Models", Models1.Text.Trim()));
                    list.Add(new SqlPara("FlatStandardTime", FlatStandardTime1.Text.Trim()));
                    list.Add(new SqlPara("FlatStandardArrivalTime", FlatStandardArrivalTime1.Text.Trim()));
                    list.Add(new SqlPara("StandardArrivalTime", StandardArrivalTime1.Text.Trim()));
                    list.Add(new SqlPara("Kilometre", Kilometre1.Text.Trim()));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_GX_YXSJB", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK("修改成功！");
                        this.Close();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }

            }
            else
            {
                try
                {
                    List<SqlPara> list = new List<SqlPara>();
                    list.Add(new SqlPara("ID", Guid.NewGuid().ToString()));
                    list.Add(new SqlPara("startweb", cbbStartWeb.Text.Trim()));
                    list.Add(new SqlPara("endweb", cbbEndWeb.Text.Trim()));
                    list.Add(new SqlPara("runtime", textEdit3.Text.Trim()));
                    list.Add(new SqlPara("remarks", textEdit1.Text.Trim()));
                    list.Add(new SqlPara("startSite",cbbStartSite.Text.Trim()));
                    list.Add(new SqlPara("endSite",cbbEndSite.Text.Trim()));
                    //2018.1.11wbw
                    list.Add(new SqlPara("StandardDepartureTime", StandardDepartureTime1.Text.Trim()));
                    list.Add(new SqlPara("Shift", Shift1.Text.Trim()));
                    list.Add(new SqlPara("Models", Models1.Text.Trim()));
                    list.Add(new SqlPara("FlatStandardTime", FlatStandardTime1.Text.Trim()));
                    list.Add(new SqlPara("FlatStandardArrivalTime", FlatStandardArrivalTime1.Text.Trim()));
                    list.Add(new SqlPara("StandardArrivalTime", StandardArrivalTime1.Text.Trim()));
                    list.Add(new SqlPara("Kilometre", Kilometre1.Text.Trim()));
                    SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_GX_YXSJB", list);
                    if (SqlHelper.ExecteNonQuery(spe) > 0)
                    {
                        MsgBox.ShowOK("保存成功");
                        this.Close();
                        return;
                    }


                }
                catch (Exception ex)
                {
                    MsgBox.ShowException(ex);
                }
            }
        }

        private void cbbStartSite_TextChanged(object sender, EventArgs e)
        {
            strWebs = GetSites2();
            //desitiWeb1.Properties.Items.Clear();
           //desitiWeb1.Text = "";
            cbbStartWeb.Properties.Items.Clear();
            cbbStartWeb.Text = "";

            string[] site = cbbStartSite.Text.Trim().Split(',');
            ComboBoxEdit cmb1 = new ComboBoxEdit();
            for (int i = 0; i < site.Length; i++)
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                for (int k = 0; k < dr.Length; k++)
                {
                    //desitiWeb1.Properties.Items.Add(dr[k]["WebName"]);
                    cmb1.Properties.Items.Add(dr[k]["WebName"]);
                }
            }
            if (cmb1.Properties.Items.Count > 0)
            {
                foreach (var item in cmb1.Properties.Items)
                {
                    if (strWebs.Contains(item.ToString()))
                    {
                        cbbStartWeb.Properties.Items.Add(item);
                    }
                }
            }
            else
            {
                //desitiWeb1.Text = destiSite1.Text.Trim();
            }
        }

        //2017.12.28wbw
        private string GetSites2()
        {
            try
            {
                //获取所有指定的始发网点
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_sysRoleDataInfoClone_All");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    XtraMessageBox.Show("没有任何指定始发网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                DataRow[] dr = ds.Tables[0].Select(string.Format("GRCode='{0}'", 206));
                if (dr == null || dr.Length == 0)
                {
                    XtraMessageBox.Show("没有找到任何指定始发网点", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return dr[0]["SiteNames"].ToString();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void cbbEndSite_TextChanged(object sender, EventArgs e)
        {
            strWebs = GetSites();
            //desitiWeb2.Properties.Items.Clear();
           // desitiWeb2.Text = "";
            cbbEndWeb.Properties.Items.Clear();
            cbbEndWeb.Text = "";
            string[] site = cbbEndSite.Text.Trim().Split(',');
            ComboBoxEdit cmb1 = new ComboBoxEdit();
            for (int i = 0; i < site.Length; i++)
            {
                DataRow[] dr = CommonClass.dsWeb.Tables[0].Select("SiteName='" + site[i] + "'");
                for (int k = 0; k < dr.Length; k++)
                {
                    //desitiWeb2.Properties.Items.Add(dr[k]["WebName"]);
                    cmb1.Properties.Items.Add(dr[k]["WebName"]);
                }
            }
            if (cmb1.Properties.Items.Count > 0)
            {
                foreach (var item in cmb1.Properties.Items)
                {
                    if (strWebs.Contains(item.ToString()))
                    {
                        cbbEndWeb.Properties.Items.Add(item);
                    }
                }
            }
            else
            {
                //desitiWeb2.Text = destiSite2.Text.Trim();
            }
        }
    }
}