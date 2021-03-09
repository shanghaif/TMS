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

namespace ZQTMS.UI
{
    public partial class frmReceiptDD : BaseForm
    {
        DataSet ds = new DataSet();
        string xx = "";
        public frmReceiptDD()
        {
            InitializeComponent();
        }

        private void frmReceiptDD_Load(object sender, EventArgs e)
        {
            CommonClass.FormSet(this);
            CommonClass.GetGridViewColumns(myGridView1);
            GridOper.SetGridViewProperty(myGridView1);
            GridOper.RestoreGridLayout(myGridView1, myGridView1.Guid.ToString());

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //提取数据
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void getdata()
        {
            try
            {
                if (HDPCH.Text == "" && HDBillNo.Text == "" && CourierNumber.Text=="")
                {
                    MsgBox.ShowOK("请输入批次号或者内部带货单号或者快递单号！");
                    return;
                }
                if (HDPCH.Text != "" && HDBillNo.Text != "")
                {
                    MsgBox.ShowOK("批次号和内部带货单号只能输入一个，谢谢！");
                    return;
                }
                if (HDPCH.Text != "" && CourierNumber.Text != "")
                {
                    MsgBox.ShowOK("批次号和快递单号只能输入一个，谢谢！");
                    return;
                }
                if (HDBillNo.Text != "" && CourierNumber.Text != "")
                {
                    MsgBox.ShowOK("快递单号和内部带货单号只能输入一个，谢谢！");
                    return;
                }
                List<SqlPara> list = new List<SqlPara>();
                if (HDPCH.Text != "")
                {
                    list.Add(new SqlPara("HDPCH", HDPCH.Text.Trim()));
                }
                if (HDBillNo.Text != "")
                {
                    list.Add(new SqlPara("HDBillNo", HDBillNo.Text.Trim()));
                }
                if (CourierNumber.Text != "")
                {
                    list.Add(new SqlPara("CourierNumber", CourierNumber.Text.Trim()));
                }
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_Receipt_HDDD", list);
                ds = SqlHelper.GetDataSet(spe);
                myGridControl1.DataSource = ds.Tables[0];
                xx = HDPCH.Text;
                
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //新增
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (myGridView1.RowCount < 1)
            {
                MsgBox.ShowOK("请先提取数据！");
                return;
            }
            panel2.Visible = true;


        }

        //删除
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int rowhandle = myGridView1.FocusedRowHandle;
            if (rowhandle < 0)
            {
                MsgBox.ShowOK("请选择一条信息！");
                return;
            }
            if (MsgBox.ShowYesNo("是否删除？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string BillNO = myGridView1.GetRowCellValue(rowhandle, "BillNO").ToString();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string a = ds.Tables[0].Rows[i]["BillNo"].ToString();
                if (BillNO == a)
                {
                    ds.Tables[0].Rows[i].Delete();
                }

            }
            //给回单刷新批次号
            setHDPCH(2);
            //生成问题件
            addProblemParts(2);
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            textEdit1.Text = "";
        }

        //添加
        private void simpleButton7_Click(object sender, EventArgs e)
        {

            //判断是否签回单
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", textEdit1.Text.Trim()));
                SqlParasEntity spe = new SqlParasEntity(OperType.Query, "QSP_GET_WAYBILL_ReceiptCondition", list);
                DataSet ds2 = SqlHelper.GetDataSet(spe);
                if (ds2.Tables[0].Rows.Count == 0)
                {
                    return;
                }
                string ReceiptCondition = ds2.Tables[0].Rows[0]["ReceiptCondition"].ToString();
                if (ReceiptCondition == "" || ReceiptCondition == "附清单") //HJ20181009
                {
                    MsgBox.ShowOK("添加的运单号，没有签回单，不能添加！");
                    return;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
            //给回单刷新批次号
            setHDPCH(1);
            getdata();
            //生成问题件
            addProblemParts(1);
        }

        private void addProblemParts(int A)
        {
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", textEdit1.Text.Trim()));
                list.Add(new SqlPara("OperateWeb", myGridView1.GetRowCellValue(0, "OperateWeb").ToString()));
                if (A == 1)
                {
                    list.Add(new SqlPara("ProblemDescription", "有回单，回单批次号里没有。"));
                }
                if (A == 1)
                {
                    list.Add(new SqlPara("ProblemDescription", "无回单，回单批次号里有。"));
                }
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_ADD_ProblemParts_HDDD", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        private void setHDPCH(int a)
        {
            try
            {
                
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNo", textEdit1.Text.Trim()));
                if (a == 1)
                {
                    list.Add(new SqlPara("HDPCH", xx));
                }
                if (a == 2)
                {
                    list.Add(new SqlPara("HDPCH", ""));
                }
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_Receipt_HDPCH", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {

                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //确定
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否确定？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            string BillNos = "";
            for (int i = 0; i < myGridView1.RowCount; i++)
            {
                BillNos += myGridView1.GetRowCellValue(i, "BillNO").ToString() + "@";
            }
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("BillNos", BillNos));
                SqlParasEntity spe = new SqlParasEntity(OperType.Execute, "QSP_UPDATE_WAYBILL_ReceiptState", list);
                if (SqlHelper.ExecteNonQuery(spe) > 0)
                {
                    myGridControl1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }

        //回单返回
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            if (MsgBox.ShowYesNo("是否返回？\r\r此操作不可逆，请确认！") != DialogResult.Yes)
            {
                return;
            }
            
            try
            {
                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("OperateState", "正常回单"));
                list.Add(new SqlPara("Operator", CommonClass.UserInfo.UserName));
                list.Add(new SqlPara("OperateTime", DateTime.Now.ToString()));
                string allBillNo = "";
                string begweb = "";
                string DepName = "";
                if (myGridView1.RowCount > 0)
                {
                    for (int i = 0; i < myGridView1.RowCount; i++)
                    {
                        allBillNo += myGridView1.GetRowCellValue(i, "BillNO") + ",";
                        begweb = myGridView1.GetRowCellValue(i, "begweb") .ToString();
                        DepName = myGridView1.GetRowCellValue(i, "DepName").ToString();
                        if (begweb != CommonClass.UserInfo.WebName)
                        {
                            if (CommonClass.UserInfo.WebName != DepName)
                            {
                                MsgBox.ShowOK("只有开单网点才可以做回单返回！");
                                return;
                            }
                        }
                    }
                }
                list.Add(new SqlPara("allBillNo", allBillNo.Trim()));
                list.Add(new SqlPara("OperateSite", CommonClass.UserInfo.SiteName));
                list.Add(new SqlPara("OperateWeb", CommonClass.UserInfo.WebName));
                list.Add(new SqlPara("RecBatch", Guid.NewGuid().ToString()));
                list.Add(new SqlPara("OperateRemark", ""));
                list.Add(new SqlPara("ToSite", ""));
                list.Add(new SqlPara("ToWeb", ""));
                list.Add(new SqlPara("SendNum", ""));
                list.Add(new SqlPara("ReceiptState", "回单返回"));
                list.Add(new SqlPara("LinkTel", ""));
                SqlParasEntity sps = new SqlParasEntity(OperType.Execute, "USP_ADD_Receipt_NEW", list);
                if (SqlHelper.ExecteNonQuery(sps) > 0)
                {
                    MsgBox.ShowOK();
                    myGridControl1.DataSource = null;
                }

            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex);
            }
        }
    }
}