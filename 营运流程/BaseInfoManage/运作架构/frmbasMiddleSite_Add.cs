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
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.IO;
using DevExpress.XtraEditors.Controls;

namespace ZQTMS.UI
{
    public partial class frmbasMiddleSite_Add : BaseForm
    {
        public frmbasMiddleSite_Add()
        {
            InitializeComponent();
        }
        public DataRow dr = null;
        private DataSet dsarea = new DataSet();
        private DataTable dsStreet = null;
        private DataTable dsCity = null;
        private string siteName = "";
        private void frmOrgUnit_Web_Add_Load(object sender, EventArgs e)
        {
            GetCompanyId();
            if (Common.CommonClass.UserInfo.companyid != "101")
            {
                CompanyID.Enabled = false;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }
            else
            {
                CompanyID.Enabled = true;
                CompanyID.Text = Common.CommonClass.UserInfo.companyid;
            }

            SiteAll();
            getWeb();

            if (dr != null)
            {
                SiteName.EditValue = dr["SiteName"];
                MiddleStatus.EditValue = dr["MiddleStatus"];
            }
            dsarea = CommonClass.AreaManager.DsArea;
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(MiddleProvince, "0");
        }

        private DataTable GetAllRegion(int type)
        {
            DataSet ds = null;
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Type", type));
                //list.Add(new SqlPara("Ids", where));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEREGION_Comb", list);
                ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return null;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            return ds.Tables[0];
        }



        private void btnCreate_Click(object sender, EventArgs e)
        {
            //string whereCase = "";
            //int type = 0;
            //if (!string.IsNullOrEmpty(MiddleStreet.Text.Trim()))
            //{
            //    whereCase = string.Format("streetId in ({0})", MiddleStreet.EditValue);
            //    type = 3;
            //}
            //else if (!string.IsNullOrEmpty(MiddleArea.Text.Trim()))
            //{
            //    whereCase = string.Format("areaId in ({0})", MiddleArea.EditValue);
            //    type = 2;
            //}
            //else if (!string.IsNullOrEmpty(MiddleCity.Text.Trim()))
            //{
            //    whereCase = string.Format("cityId in ({0})", MiddleCity.EditValue);
            //    type = 1;
            //}
            if (string.IsNullOrEmpty(MiddleCity.Text.Trim()))
            {
                MsgBox.ShowOK("请选择城市");
                return;
            }
            if (string.IsNullOrEmpty(MiddleArea.Text.Trim()))
            {
                MsgBox.ShowOK("请选择区域");
                return;
            }
            if (string.IsNullOrEmpty(MiddleStreet.Text.Trim()))
            {
                MsgBox.ShowOK("请选择街道");
                return;
            }
            if (dsStreet == null)
            {
                dsStreet = GetAllRegion(3);
            }
            if (dsStreet == null) return;
            DataRow[] rows = dsStreet.Select(string.Format("streetId in ({0})", MiddleStreet.EditValue));
            DataTable dt = new DataTable("newDt");
            if (!dt.Columns.Contains("Destination"))
            {
                dt.Columns.Add("Destination");
            }
            if (!dt.Columns.Contains("MiddleProvince"))
            {
                dt.Columns.Add("MiddleProvince");
            }
            if (!dt.Columns.Contains("MiddleCity"))
            {
                dt.Columns.Add("MiddleCity");
            }
            if (!dt.Columns.Contains("MiddleArea"))
            {
                dt.Columns.Add("MiddleArea");
            }
            if (!dt.Columns.Contains("MiddleStreet"))
            {
                dt.Columns.Add("MiddleStreet");
            }
            if (!dt.Columns.Contains("iscancel"))
            {
                dt.Columns.Add("iscancel");
            }
            if (rows != null && rows.Length > 0)
            {
                DataRow newRow;
                foreach (DataRow row in rows)
                {
                    newRow = dt.NewRow();
                    newRow["MiddleProvince"] = row["province"];
                    newRow["MiddleCity"] = row["city"];
                    newRow["MiddleArea"] = row["area"];
                    newRow["MiddleStreet"] = row["street"];
                    newRow["iscancel"] = 0;
                    //if (type == 1)
                    //{
                    //    newRow["Destination"] = row["city"];
                    //}
                    //else if (type == 2)
                    //{
                    //    newRow["Destination"] = row["area"];
                    //}
                    //else if (type == 3)
                    //{
                    //    newRow["Destination"] = row["street"];
                    //}
                    newRow["Destination"] = row["area"];

                    dt.Rows.Add(newRow);
                }
            }
            myGridControl1.DataSource = dt;
        }

        private void btnConcel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //private void getArea()
        //{
        //    #region 行政区划

        //    try
        //    {

        //        SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASEREGION");
        //        DataSet ds = SqlHelper.GetDataSet(sps);

        //        if (ds == null || ds.Tables.Count == 0) return;

        //        dsarea = CommonClass.AreaManager.DsArea;
        //        if (dsarea == null || dsarea.Tables.Count == 0) return;
        //        DataRow[] dr = dsarea.Tables[0].Select("regionlevel=1");
        //        List<object> list = new List<object>();
        //        for (int i = 0; i < dr.Length; i++)
        //        {
        //            MiddleProvince.Properties.Items.Add(dr[i]["regionname"]);
        //            list.Add(dr[i]["RegionID"]);
        //        }
        //        MiddleProvince.Tag = list;
        //    }
        //    catch (Exception ex)
        //    {
        //        MsgBox.ShowException(ex);
        //    }
        //    #endregion
        //}

        private void SiteAll()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASSITE", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SiteName.Properties.Items.Add(ds.Tables[0].Rows[i]["SiteName"]);
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
        DataSet dsweb = new DataSet();
        private void getWeb()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BASWEB", list);
                dsweb = SqlHelper.GetDataSet(sps);
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }



        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void SetAddress(CheckedComboBoxEdit cb, string parentIds)
        {
            try
            {
                if (dsarea == null || dsarea.Tables.Count == 0) return;
                DataRow[] dr = dsarea.Tables[0].Select("ParentID in (" + parentIds + ")");
                for (int i = 0; i < dr.Length; i++)
                {
                    cb.Properties.Items.Add(dr[i]["RegionID"], dr[i]["RegionName"].ToString());
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void MiddleCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleArea.Properties.Items.Clear();
                MiddleArea.Text = "";
                if (MiddleCity.Text.Trim() == "")
                {
                    return;
                }
                SetAddress(MiddleArea, MiddleCity.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MiddleArea_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleStreet.Properties.Items.Clear();
                MiddleStreet.Text = "";

                if (MiddleArea.Text.Trim() == "")
                {
                    return;
                }
                SetAddress(MiddleStreet, MiddleArea.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MiddleProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MiddleCity.Properties.Items.Clear();
                MiddleCity.Text = "";
                if (MiddleProvince.Text.Trim() == "")
                {
                    return;
                }
                SetAddress(MiddleCity, MiddleProvince.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            myGridView1.DeleteRow(rowhandle);
            myGridView1.PostEditor();
            myGridView1.UpdateCurrentRow();
            myGridView1.UpdateSummary();
            DataTable dt = myGridControl1.DataSource as DataTable;
            dt.AcceptChanges();
        }

        private void btnDuibi_Click(object sender, EventArgs e)
        {
            DataTable dt = myGridControl1.DataSource as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                string[] remarkRows = Remark.Lines;
                int j = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (remarkRows != null && remarkRows.Length > 0)
                    {
                        foreach (string item in remarkRows)
                        {
                            if (row["MiddleStreet"].ToString() != item)
                            {
                                myGridView1.SetRowCellValue(j, "iscancel", 1);
                            }
                            else
                            {
                                myGridView1.SetRowCellValue(j, "iscancel", 0);
                                break;
                            }
                        }
                    }
                    j++;
                }
            }
            myGridView1.PostEditor();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SiteName.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择站点");
                    return;
                }
                if (string.IsNullOrEmpty(WebName.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择网点");
                    return;
                }
                if (string.IsNullOrEmpty(MiddleProvince.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择省份");
                    return;
                }
                if (string.IsNullOrEmpty(MiddleCity.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择城市");
                    return;
                }
                if (string.IsNullOrEmpty(MiddleArea.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择区域");
                    return;
                }
                if (string.IsNullOrEmpty(MiddleStreet.Text.Trim()))
                {
                    MsgBox.ShowOK("请选择街道");
                    return;
                }
                DataTable dt = myGridControl1.DataSource as DataTable;
                dt.Columns.Remove("iscancel");
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("WebName", WebName.Text.Trim()));
                list.Add(new SqlPara("SiteName", siteName));
                list.Add(new SqlPara("MiddleStatus", MiddleStatus.Checked ? 1 : 0));
                list.Add(new SqlPara("Type", Type.Text.Trim()));
                list.Add(new SqlPara("Tb", dt));
                list.Add(new SqlPara("companyid1", CompanyID.Text.Trim()));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_basMiddleSite_UPLOAD", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    this.Close();
                }
                else
                {
                    MsgBox.ShowOK("保存失败");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void SiteName_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(SiteName.Text))
            {
                return;
            }
            WebName.Text = "";
            WebName.Properties.Items.Clear();
            StringBuilder whereCase = new StringBuilder();
            string[] siteNameStrs = SiteName.Text.Split(',');
            for (int i = 0; i < siteNameStrs.Length; i++)
            {
                if (i > 0)
                {
                    whereCase.Append(" or ");
                }
                else
                {
                    siteName = siteNameStrs[i].Trim();
                }
                whereCase.AppendFormat("sitename='{0}'", siteNameStrs[i].Trim());
            }
            DataRow[] dr = dsweb.Tables[0].Select(whereCase.ToString());
            for (int i = 0; i < dr.Length; i++)
            {
                WebName.Properties.Items.Add(dr[i]["WebName"]);
            }

            MiddleProvince.Text = "";
            if (dsCity == null)
            {
                dsCity = GetAllRegion(1);
            }
            if (dsCity == null) return;
            DataRow[] rows = dsCity.Select(string.Format("city like '{0}%'", siteName));
            if (rows != null && rows.Length > 0)
            {
                ZQTMS.Common.CommonClass.SetSelectIndex(rows[0]["province"].ToString().Trim(), MiddleProvince);
            }
            if (!string.IsNullOrEmpty(MiddleProvince.EditValue.ToString()))
            {
                MiddleCity.Properties.Items.Clear();
                MiddleCity.Text = "";
                SetAddress(MiddleCity, MiddleProvince.EditValue.ToString());
                if (rows != null && rows.Length > 0)
                {
                    CommonClass.SetCheckedItems(rows[0]["cityid"].ToString(), MiddleCity);
                }
            }
        }

        public void GetCompanyId()
        {
            try
            {
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "P_USP_GET_companyid");
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0) return;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    CompanyID.Properties.Items.Add(dr[0]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}
