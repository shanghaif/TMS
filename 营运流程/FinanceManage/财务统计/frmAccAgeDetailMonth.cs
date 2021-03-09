﻿using System;
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
    public partial class frmAccAgeDetailMonth : BaseForm
    {
        public frmAccAgeDetailMonth()
        {
            InitializeComponent();
        }

        private void frmAccAgeDetailMonth_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar3);
            GridOper.RestoreGridLayout(myGridView1);
            bdate.EditValue = CommonClass.gbdate.AddDays(-7);
            edate.EditValue = CommonClass.gedate;

            CommonClass.SetCauseWeb(Web, CommonClass.UserInfo.CauseName, CommonClass.UserInfo.AreaName, true);
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_billAccAgeDetailMonth", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    MsgBox.ShowOK();
                }
            }
            catch (Exception ex) 
            {
                MsgBox.ShowException(ex);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("bdate",bdate.Text));
                list.Add(new SqlPara("edate", edate.Text));
                list.Add(new SqlPara("Web", Web.Text.Trim() == "全部" ? "%%" : Web.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_billAccAgeDetailMonth", list);
                DataSet ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource=ds.Tables[0];

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ExportToExcel(myGridView1, "账龄表明细简表（未收）");
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}