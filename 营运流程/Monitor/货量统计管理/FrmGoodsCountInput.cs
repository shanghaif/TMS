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
using System.Collections;

namespace ZQTMS.UI
{
    public partial class FrmGoodsCountInput : BaseForm
    {
        public FrmGoodsCountInput()
        {
            InitializeComponent();
        }
        DataSet ds1 = new DataSet();
        int spiltLength = 0;
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOperType.Text))
            {
                MsgBox.ShowOK("操作类型不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(cbbOptCategory.Text))
            {
                MsgBox.ShowOK("操作类别不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(operMan.Text))
            {
                MsgBox.ShowOK("操作人不能为空！");
                return;
            }
            if (operMan.Text.Contains("，"))
            {
                MsgBox.ShowOK("操作人中有中文格式逗号，请切换成英文格式！");
                return;
            }
            if (string.IsNullOrEmpty(operDate.Text))
            {
                MsgBox.ShowOK("操作时间不能为空！");
                return;
            }
            if (string.IsNullOrEmpty(txtCarLoding.Text.Trim()))
            {
                MsgBox.ShowOK("请填写车位！");
                return;
            }
            spiltLength = operMan.Text.Split(',').Length;
            if (spiltLength > 6)
            {
                MsgBox.ShowOK("操作人数最多为6位！");
                return;
            }
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FrmGoodsCountInput_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1);
            string webName = CommonClass.UserInfo.WebName;
            //CommonClass.SetUser(operMan, webName, false);
            operDate.DateTime = CommonClass.gcdate;
            GridOper.GetGridViewColumn(myGridView1, "ActualOperNum").AppearanceCell.BackColor = Color.Yellow;
            //GridOper.CreateStyleFormatCondition(myGridView1,"ActualOperNum")
            if (txtOperType.Text.Trim() == "装车")
            {
                if (cbbQueryType.Properties.Items.Count > 0)
                {
                    cbbQueryType.Properties.Items.Clear();
                }
                cbbQueryType.Properties.Items.Add("运单号");
                cbbQueryType.Properties.Items.Add("配载批次号");
                cbbQueryType.Properties.Items.Add("送货批次号");
                cbbQueryType.Properties.Items.Add("接货派车单号");//20190416 zhongyf
                if (cbbOptCategory.Properties.Items.Count>0)
                {
                    cbbOptCategory.Properties.Items.Clear();
                }
                cbbOptCategory.Properties.Items.Add("厢车");
                cbbOptCategory.Properties.Items.Add("平板");
                cbbOptCategory.Properties.Items.Add("城际短驳");
                //cbbQueryType.Properties.Items.Add("接货派车单号");//20190416 zhongyf

            }
            SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_GrouperInfo");
            DataSet ds = SqlHelper.GetDataSet(sps);
            if (ds == null || ds.Tables.Count == 0) return;
            try
            {
                operMan.Properties.Items.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    operMan.Properties.Items.Add(ds.Tables[0].Rows[i]["subopetman"]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        /// <summary>
        /// 根据运单号或批次号查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string queryNums = "";
                if (!string.IsNullOrEmpty(txtNums.Text))
                {
                    txtNums.Text = txtNums.Text.Trim();
                    string[] nums = txtNums.Lines;

                    if (cbbQueryType.Text.Contains("运单") && nums.Length > 30)
                    {
                        MsgBox.ShowOK("运单号最多能有30个！");
                        return;
                    }
                    else if (cbbQueryType.Text.Contains("批次") && nums.Length > 1)
                    {
                        MsgBox.ShowOK("批次号最多只能有1个！");
                        return;
                    }
                    else if (cbbQueryType.Text.Contains("接货派车单号") && nums.Length > 1)
                    {
                        MsgBox.ShowError("派车单号最多只能有1个");
                        return;
                    }
                    for (int i = 0; i < nums.Length; i++)
                    {
                        if (cbbQueryType.Text.Contains("运单"))
                        {
                            queryNums += nums[i] + ",";
                        }
                        else
                        {
                            queryNums = nums[i];
                        }
                    }
                }
                else
                {
                    MsgBox.ShowOK("请填写相应的查询单号！");
                    return;
                }

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("searchType", cbbQueryType.Text.Trim()));
                list.Add(new SqlPara("opertype", txtOperType.Text.Trim()));
                list.Add(new SqlPara("searchCondition", queryNums));
                list.Add(new SqlPara("spiltLength", spiltLength));

                SqlParasEntity sps = new SqlParasEntity(OperType.Query, "QSP_GET_BillInfoById", list);
                DataSet ds = SqlHelper.GetDataSet(sps);
                if (ds == null || ds.Tables[0].Rows.Count == 0)
                {
                    MsgBox.ShowError("没有查到相关数据!");
                    return;
                }
                if (ds != null || ds.Tables[0].Rows.Count > 0)
                {
                    ds1 = ds;
                    myGridControl1.DataSource = ds.Tables[0];
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    decimal perOperWeight = Convert.ToDecimal(ds.Tables[0].Rows[i]["OptWeight"]) / Convert.ToInt16(ds.Tables[0].Rows[i]["Num"]) * Convert.ToDecimal(ds.Tables[0].Rows[i]["ActualOperNum"]) / spiltLength;
                    //    myGridView1.SetRowCellValue(i, "PerAvgOperWeight", Math.Round(perOperWeight, 2));
                    //}
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        /// <summary>
        /// 完成登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            List<SqlPara> list = new List<SqlPara>();
            string billno = "",actualWeight = "", actualNum = "", perWeight = "",batchNo="";
            try
            {
                if (myGridView1.RowCount > 0)
                {
                    if (txtOperType.Text.Trim() == "卸车")
                    {
                        if (cbbOptCategory.Text != "接货")
                        {
                            MsgBox.ShowError("派车类型不为接货,不能完成!");
                            return;
                        }
                    }



                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        decimal optWeight = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "OptWeight"));
                        int num = Convert.ToInt16(myGridView1.GetRowCellValue(i, "Num"));
                        decimal actualOperWeight = Convert.ToDecimal(myGridView1.GetRowCellValue(i, "ActualOperWeight"));
                        int actualOperNum = Convert.ToInt16(myGridView1.GetRowCellValue(i, "ActualOperNum"));

                        billno += myGridView1.GetRowCellValue(i, "BillNo") + "@";
                        
                        actualWeight += Math.Round(Convert.ToDecimal(myGridView1.GetRowCellValue(i, "ActualOperWeight")),2) + "@";
                        actualNum += Convert.ToInt16(myGridView1.GetRowCellValue(i, "ActualOperNum")) + "@";
                        perWeight += Math.Round(optWeight / num * actualOperNum / spiltLength, 2)+"@";
                    }

                    string operType = txtOperType.Text.Trim();

                    string man = operMan.Text.Trim();
                    string[] men = man.Split(',');
                    string newMen = "";
                    if (men.Length > 0)
                    {
                        for (int j = 0; j < men.Length; j++)
                        {
                            newMen += men[j].Trim() + ",";
                        }
                    }

                    if (cbbQueryType.Text.Contains("批次号"))
                    {
                        batchNo = txtNums.Text.Trim();
                        if (string.IsNullOrEmpty(batchNo))
                        {
                            MsgBox.ShowOK("批次号不能删除！");
                            return;
                        }
                    }
                    list.Add(new SqlPara("OperType", operType));
                    list.Add(new SqlPara("OptCategory",cbbOptCategory.Text.Trim()));
                    list.Add(new SqlPara("BillNo", billno));
                    list.Add(new SqlPara("ActualOperWeight", actualWeight));
                    list.Add(new SqlPara("ActualOperNum", actualNum));
                    list.Add(new SqlPara("OperMan", newMen));
                    list.Add(new SqlPara("CarLoding", txtCarLoding.Text.Trim()));
                    list.Add(new SqlPara("batchNo", batchNo));
                    list.Add(new SqlPara("PerAvgOperWeight", perWeight));
                    list.Add(new SqlPara("OperDate", operDate.Text));//zaj 2018-6-30 增加操作时间参数
                    SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_GoodsCountInfoTwo", list);
                    if (SqlHelper.ExecteNonQuery(sps) > 0)
                    {
                        MsgBox.ShowOK();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError(ex.ToString());
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int[] rowHandle = myGridView1.GetSelectedRows();
                if (rowHandle.Length==0)
                {
                    MsgBox.ShowOK("请选择要删除的行！");
                    return;
                }
                for (int i = 0; i < rowHandle.Length; i++)
                {
                    myGridView1.DeleteRow(rowHandle[i]);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void myGridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e == null || myGridView1.FocusedRowHandle < 0) return;
            try
            {
                int actualNum = Convert.ToInt16(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "LeftGoodsNum"));
                decimal operWeight = Convert.ToDecimal(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "OptWeight"));
                int num = Convert.ToInt16(myGridView1.GetRowCellValue(myGridView1.FocusedRowHandle, "Num"));
                int newActualNum = ConvertType.ToInt32(e.Value);
                if (newActualNum <= 0)
                {
                    e.Valid = false;
                    e.ErrorText = "实操件数必须大于0!";
                    return;
                }
                if (newActualNum > actualNum)
                {
                    e.Valid = false;
                    //e.ErrorText = "实操件数不能大于" + ActualNum;
                    MsgBox.ShowOK("实操件数不能大于" + actualNum);
                    return;
                }
                myGridView1.SetRowCellValue(myGridView1.FocusedRowHandle, "ActualOperWeight", Math.Round(operWeight / num * newActualNum, 2));
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex==1)
            {
                if (string.IsNullOrEmpty(txtOperType.Text))
                {
                    MsgBox.ShowOK("操作类型不能为空！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                if (string.IsNullOrEmpty(cbbOptCategory.Text))
                {
                    MsgBox.ShowOK("操作类别不能为空！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                if (string.IsNullOrEmpty(operMan.Text))
                {
                    MsgBox.ShowOK("操作人不能为空！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                if (operMan.Text.Contains("，"))
                {
                    MsgBox.ShowOK("操作人中有中文格式逗号，请切换成英文格式！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                if (string.IsNullOrEmpty(operDate.Text))
                {
                    MsgBox.ShowOK("操作时间不能为空！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                if (string.IsNullOrEmpty(txtCarLoding.Text.Trim()))
                {
                    MsgBox.ShowOK("请填写车位！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
                spiltLength = operMan.Text.Split(',').Length;
                if (spiltLength > 6)
                {
                    MsgBox.ShowOK("操作人数最多为6位！");
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                    return;
                }
            }

        }

        private void txtOperType_TextChanged(object sender, EventArgs e)
        {
            if (txtOperType.Text.Trim() == "装车")
            {
                if (cbbQueryType.Properties.Items.Count > 0)
                {
                    cbbQueryType.Properties.Items.Clear();
                }
                cbbQueryType.Properties.Items.Add("运单号");
                cbbQueryType.Properties.Items.Add("配载批次号");
                cbbQueryType.Properties.Items.Add("送货批次号");
                //cbbQueryType.Properties.Items.Add("接货派车单号");//20190416 zhongyf
            }
            else
            {
                if (cbbQueryType.Properties.Items.Count > 0)
                {
                    cbbQueryType.Properties.Items.Clear();
                }
                cbbQueryType.Properties.Items.Add("运单号");
                cbbQueryType.Properties.Items.Add("配载批次号");
                cbbQueryType.Properties.Items.Add("短驳批次号");
                //cbbQueryType.Properties.Items.Add("接货派车单号");//20190416 zhongyf
            }
        }

        private void txtOperType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtOperType.Text.Trim() == "装车")
            {
                if (cbbOptCategory.Properties.Items.Count > 0)
                {
                    cbbOptCategory.Properties.Items.Clear();
                    cbbOptCategory.Text = "";
                }
                cbbOptCategory.Properties.Items.Add("厢车");
                cbbOptCategory.Properties.Items.Add("平板");
                cbbOptCategory.Properties.Items.Add("城际短驳");
            }
            else
            {
                if (cbbOptCategory.Properties.Items.Count > 0)
                {
                    cbbOptCategory.Properties.Items.Clear();
                    cbbOptCategory.Text = "";
                }
                cbbOptCategory.Properties.Items.Add("接货");
                cbbOptCategory.Properties.Items.Add("短驳");
                cbbOptCategory.Properties.Items.Add("省际到达");
            }
        }
    }
}