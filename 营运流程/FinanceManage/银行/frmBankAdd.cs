using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Collections;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using ZQTMS.Tool;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
{
    public partial class frmBankAdd : BaseForm
    {
        public frmBankAdd()
        {
            InitializeComponent();
        }

        public int id = -1;
        DataSet dsarea = new DataSet();
        public DataSet dsshipper = new DataSet();//汇款客户资料 打开银行信息平台就开始提取

        private void frmBankAdd_Load(object sender, EventArgs e)
        {
            edbilldate.DateTime = CommonClass.gcdate;

            #region 区划

            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edsheng, "0");

            #endregion

            if (id < 0)
            {
                clear();
            }
            else
            {
                fill();
            }
        }

        private void fill()
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("id", id));
                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BANK_ByID", list);

                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

                DataRow dr = ds.Tables[0].Rows[0];

                edbankman.EditValue = dr["bankman"];
                edbankcode.EditValue = dr["bankcode"];
                edbankname.EditValue = dr["bankname"];

                edbankchild.EditValue = dr["bankchild"];
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["sheng"].ToString().Trim(), edsheng);
                ZQTMS.Common.CommonClass.SetSelectIndex(dr["city"].ToString().Trim(), edcity);
                edopertype.EditValue = dr["opertype"];

                edaccout.EditValue = dr["accout"];
                edaccin.EditValue = dr["accin"];
                edbilldate.EditValue = dr["billdate"];

                edouttype.EditValue = dr["outtype"];
                edremark.EditValue = dr["remark"];
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item.GetType() == typeof(TextEdit) || item.GetType() == typeof(ComboBoxEdit))
                {
                    item.Text = "";
                }
            }
            edbilldate.DateTime = CommonClass.gcdate;
            id = -1;
        }

        private void comboBoxEdit2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                edcity.Properties.Items.Clear();
                edcity.Text = "";
                edcity.Tag = null;

                if (edsheng.Text.Trim() == "")
                {
                    edcity.Text = "";
                    return;
                }

                if (edsheng.SelectedIndex < 0) return;
                int id = Convert.ToInt32(((ArrayList)edsheng.Tag)[edsheng.SelectedIndex]);

                DataRow[] dr = dsarea.Tables[0].Select("parentid=" + id + "");
                ArrayList arr = new ArrayList();
                for (int i = 0; i < dr.Length; i++)
                {
                    edcity.Properties.Items.Add(dr[i]["city"]);
                    arr.Add(dr[i]["ID"]);
                }
                edcity.Tag = arr;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string bankman = edbankman.Text.Trim();
            if (bankman == "")
            {
                MsgBox.ShowOK("必须填写开户姓名!");
                edbankman.Focus();
                return;
            }

            string bankcode = edbankcode.Text.Trim();
            if (bankcode == "")
            {
                MsgBox.ShowOK("必须填写银行账号!");
                edbankcode.Focus();
                return;
            }
            string bankname = edbankname.Text.Trim();
            if (bankname == "")
            {
                MsgBox.ShowOK("必须填写银行名称!");
                edbankname.Focus();
                return;
            }

            string bankchild = edbankchild.Text.Trim();

            string sheng = edsheng.Text.Trim();
            if (sheng == "")
            {
                MsgBox.ShowOK("必须填写所属省份!");
                edsheng.Focus();
                return;
            }

            string city = edcity.Text.Trim();
            if (city == "")
            {
                MsgBox.ShowOK("必须填写所属用户城市!");
                edcity.Focus();
                return;
            }

            string opertype = edopertype.Text.Trim();
            if (opertype == "")
            {
                MsgBox.ShowOK("必须填写转账类型!");
                edopertype.Focus();
                return;
            }


            decimal accout = 0;
            decimal.TryParse(edaccout.Text.Trim(), out accout);

            decimal accin = 0;
            decimal.TryParse(edaccin.Text.Trim(), out accin);

            if (accout == 0 && accin == 0)
            {
                MsgBox.ShowOK("必须填写转账金额!");
                return;
            }

            string outtype = edouttype.Text.Trim();
            if (accout > 0 && outtype == "")
            {
                MsgBox.ShowOK("本单转出，必须填写转出类型!");
                edouttype.Focus();
                return;
            }

            DateTime billdate = edbilldate.DateTime;

            try
            {
                List<SqlPara> list = new List<SqlPara>();


                list.Add(new SqlPara("id", id));

                list.Add(new SqlPara("bankman", bankman));
                list.Add(new SqlPara("bankcode", bankcode));
                list.Add(new SqlPara("bankname", bankname));

                list.Add(new SqlPara("sheng", sheng));  //4
                list.Add(new SqlPara("city", city));
                list.Add(new SqlPara("opertype", opertype));
                list.Add(new SqlPara("accout", accout));

                list.Add(new SqlPara("accin", accin));  //8
                list.Add(new SqlPara("billdate", billdate));
                list.Add(new SqlPara("outtype", outtype));
                list.Add(new SqlPara("remark", edremark.Text.Trim()));

                list.Add(new SqlPara("createby", CommonClass.UserInfo.UserName));  //12
                list.Add(new SqlPara("bsite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("webid", CommonClass.UserInfo.WebName));

                list.Add(new SqlPara("bankchild", bankchild));


                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "USP_ADD_BANK_HK", list);
                SqlHelper.ExecteNonQuery(sps);
                MsgBox.ShowOK();
                clear();

            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dsshipper.Tables.Count > 0)
            {
                timer1.Enabled = false;
                gridControl1.DataSource = dsshipper.Tables[0];
                //设置某列的宽度
            }
        }

        private void edbankman_Enter(object sender, EventArgs e)
        {
            if (edbankman.Text.Trim() == "")
            {
                gridView1.ClearColumnsFilter();
            }
            gridControl1.Left = edbankman.Left;
            gridControl1.Top = edbankman.Top + edbankman.Height + 2;
            gridControl1.Visible = true;
            gridControl1.BringToFront();
        }

        private void edbankman_Leave(object sender, EventArgs e)
        {
            if (!gridControl1.Focused)
            {
                gridControl1.Visible = false;
            }
        }

        private void edbankman_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                gridControl1.Focus();
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void edbankman_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null)
            {
                gridView1.ClearColumnsFilter();
                gridView1.Columns["bankman"].FilterInfo = new ColumnFilterInfo("[bankman] LIKE " + "'%" + e.NewValue.ToString() + "%'", "");
            }
            else
            {
                gridView1.ClearColumnsFilter();
            }
        }

        private void SetValue()
        {
            int rowhandle = gridView1.FocusedRowHandle;
            if (rowhandle < 0) return;
            edbankman.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankman").ToString();
            edbankcode.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankcode").ToString();
            edbankname.Text = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankname").ToString();
            edbankchild.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "bankname");

            CommonClass.SetSelectIndex(ConvertType.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "sheng")), edsheng);
            CommonClass.SetSelectIndex(ConvertType.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "city")), edcity);
            edopertype.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "opertype");

            edouttype.EditValue = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "outtype");

            gridControl1.Visible = false;
            edbankcode.Focus();
        }

        private void gridControl1_DoubleClick(object sender, EventArgs e)
        {
            SetValue();
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetValue();
            }
            if (e.KeyCode == Keys.Up)
            {
                if (gridView1.FocusedRowHandle == 0)
                {
                    edbankman.Focus();
                }
            }
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void w_bank_add_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                gridControl1.Visible = false;
            }
        }

        private void edbankname_Leave(object sender, EventArgs e)
        {
            edbankchild.Text = edbankname.Text;
        }

        private void edsheng_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZQTMS.Common.CommonClass.AreaManager.FillCityToImageComBoxEdit(edcity, edsheng.EditValue);
        }

        private void edsheng_Enter(object sender, EventArgs e)
        {
            edsheng.ShowPopup();
        }

        private void edcity_Enter(object sender, EventArgs e)
        {

        }

        private void edopertype_Enter(object sender, EventArgs e)
        {

        }

        private void edouttype_Enter(object sender, EventArgs e)
        {
            edouttype.ShowPopup();
        }

        private void edbankname_Enter(object sender, EventArgs e)
        {
            edbankname.ShowPopup();
        }
    }
}