using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using ZQTMS.Common;
using ZQTMS.Tool;
using ZQTMS.SqlDAL;
using System.Text.RegularExpressions;

namespace ZQTMS.UI
{
    public partial class frmMinusStock : BaseForm
    {
        public frmMinusStock()
        {
            InitializeComponent();
        }

        private void frmMinusStock_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            BarMagagerOper.SetBarPropertity(bar4); //如果有具体的工具条，就引用其实例
            GridOper.RestoreGridLayout(myGridView1);
            FixColumn fix = new FixColumn(myGridView1, barSubItem1);

            CommonClass.SetSite(SiteName, true);
            SiteName.Text = CommonClass.UserInfo.SiteName;
            WebName.Text = CommonClass.UserInfo.WebName;
            txtStockNum.Enabled = false;
            txtStockNum2.Enabled = false;
        }

        private void SiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            CommonClass.SetWeb(WebName, SiteName.Text);
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 提取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(wayBillNum.Text.Trim()))
                {
                    MsgBox.ShowError("请输入运单号！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("Site", SiteName.Text.Trim() == "全部" ? "%%" : SiteName.Text.Trim()));
                list.Add(new SqlPara("Web", WebName.Text.Trim() == "全部" ? "%%" : WebName.Text.Trim()));
                list.Add(new SqlPara("BillNo", wayBillNum.Text.Trim()));
                DataSet ds = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_DELETEKC_SINGLE", list));
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowError("该单号没有数据，请重新输入！");
                    return;
                } 
                myGridControl1.DataSource = ds.Tables[0];
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
        /// <summary>
        /// myGridView行单机事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        string opttype = "";//操作类型
        private void myGridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (rabMinus.Checked == true)
            {
                txtStockNum.Text = GridOper.GetRowCellValueString(myGridView1, "LeftNum");
                txtStockNum.Enabled = true;
                txtStockNum2.Text = "0";
                opttype = "减少";
            }
        }

        /// <summary>
        /// 自动筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.AllowAutoFilter(myGridView1);
        }

        /// <summary>
        /// 锁定外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.SaveGridLayout(myGridView1);
        }

        /// <summary>
        /// 取消外观
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.DeleteGridLayout(myGridView1.Guid.ToString());
        }

        /// <summary>
        /// 过滤器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GridOper.ShowAutoFilterRow(myGridView1);
        }

        /// <summary>
        /// 是否全是正整数
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static bool IsInteger(string Input, bool Plus)
        {
            if (Input == null)
            {
                return false;
            }
            else
            {
                string pattern = "^-?[0-9]+$";
                if (Plus)
                    pattern = "^[0-9]+$";
                if (Regex.Match(Input, pattern, RegexOptions.Compiled).Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 检测是否整数型数据
        /// </summary>
        /// <param name="Num">待检查数据</param>
        /// <returns></returns>
        public static bool IsInteger(string Input)
        {
            if (Input == null)
            {
                return false;
            }
            else
            {
                return IsInteger(Input, true);
            }
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveStock_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                int rowHandle = myGridView1.FocusedRowHandle;
                if (rowHandle < 0) return;
                int deleteNum = 0, backNum = 0;
                
                string sType = myGridView1.GetRowCellValue(rowHandle, "Type").ToString();
                string startSite = myGridView1.GetRowCellValue(rowHandle, "StartSite").ToString();
                string begWeb = myGridView1.GetRowCellValue(rowHandle, "BegWeb").ToString();
                string billNo = myGridView1.GetRowCellValue(rowHandle, "BillNo").ToString();
                string deleteId = myGridView1.GetRowCellValue(rowHandle, "kcid").ToString();

                List<SqlPara> list = new List<SqlPara>();
                int num = Convert.ToInt32(myGridView1.GetRowCellValue(rowHandle, "Num"));//件数
                int leftNum = Convert.ToInt32(myGridView1.GetRowCellValue(rowHandle, "LeftNum"));//库存剩余件数
                if (!IsNumeric(txtStockNum.Text.Trim()))
                {
                    MsgBox.ShowError("请输入数字！");
                    txtStockNum.EditValue = 0;
                    return;
                }
                if (!IsInteger(txtStockNum.Text.Trim()))
                {
                    MsgBox.ShowError("件数不能小于0！");
                    txtStockNum.EditValue = 0;
                    return;
                }
                deleteNum = leftNum - Convert.ToInt32(txtStockNum.Text.Trim());//库存删减后件数
                if (!IsNumeric(txtStockNum2.Text.Trim()))
                {
                    MsgBox.ShowError("请输入数字！");
                    txtStockNum2.EditValue = 0;
                    return;
                }
                if (!IsInteger(txtStockNum2.Text.Trim()))
                {
                    MsgBox.ShowError("件数不能小于0！");
                    txtStockNum2.EditValue = 0;
                    return;
                }
                backNum = leftNum + Convert.ToInt32(txtStockNum2.Text.Trim());//还原后库存件数
                if (backNum > num)
                {
                    MsgBox.ShowError("还原后件数不能大于库存件数！");
                    return;
                }
                if (string.IsNullOrEmpty(Reson.Text.Trim()))
                {
                    MsgBox.ShowError("【减库存原因】不能为空！");
                    return;
                }
                string deleteReson = Reson.Text.Trim();

                list.Add(new SqlPara("SType", sType));
                list.Add(new SqlPara("Site", startSite));
                list.Add(new SqlPara("Web", begWeb));
                list.Add(new SqlPara("BillNo", billNo));
                list.Add(new SqlPara("AddReason", deleteReson));
                list.Add(new SqlPara("DeleteId", deleteId));
                list.Add(new SqlPara("DeleteNum", deleteNum));
                list.Add(new SqlPara("opttyp", opttype));
                list.Add(new SqlPara("backNum", backNum));
                if (SqlHelper.ExecteNonQuery(new SqlParasEntity(OperType.Execute, "USP_ADD_DELETEKC_SINGLE", list)) == 0) return;
                MsgBox.ShowOK();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 删减
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rabMinus_CheckedChanged(object sender, EventArgs e)
        {
            txtStockNum.Text = GridOper.GetRowCellValueString(myGridView1, "LeftNum");
            txtStockNum2.Enabled = false;
            txtStockNum.Enabled = true;
            txtStockNum2.Text = "0";
            opttype = "减少";
        }
        /// <summary>
        /// 还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rabAdd_CheckedChanged(object sender, EventArgs e)
        {
            txtStockNum2.Text = GridOper.GetRowCellValueString(myGridView1, "LeftNum");
            txtStockNum.Enabled = false;
            txtStockNum2.Enabled = true;
            txtStockNum.Text = "0";
            opttype = "还原";
        }
    }
}