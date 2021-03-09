using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using ZQTMS.Tool;
using Sybase.DataWindow;
using ZQTMS.SqlDAL;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class w_fkpz_add : BaseForm
    {

        public string bh = "";
    
        public string oper = "NEW";
       



        public w_fkpz_add()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dw_1.Reset();
            dw_1.InsertRow();
        }



        private void w_yg_add_Load(object sender, EventArgs e)
        {

            dw_1.LibraryList = "reports.pbl";
            dw_1.DataWindowObject = "宏浩付款凭证录入";

            dw_2.LibraryList = "reports.pbl";
            dw_2.DataWindowObject = "宏浩付款凭证打印";

            if (oper == "MODIFY")
            {

                

                try
                {

                    
                    //AdoTransaction adTrans = new Sybase.DataWindow.AdoTransaction(conn);
                    //adTrans.BindConnection();
                    //dw_1.SetTransaction(adTrans);
                    //dw_1.Retrieve(bh);
                    

                }
                catch (Exception ex)
                {

                    XtraMessageBox.Show(ex.ToString() + "\n\nNo changes made to the database.", "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    
                }
            }
            else
            {




                //AdoTransaction adTrans = new Sybase.DataWindow.AdoTransaction(conn);
                //adTrans.BindConnection();
                //dw_1.SetTransaction(adTrans);
                //dw_1.Reset();
                //dw_1.InsertRow();

                //dw_1.SetItemSqlDateTime(1, "billdate", System.DateTime.Now.Date);
                //dw_1.SetItemSqlString(1, "createby", CommonClass.UserInfo.UserName);

                

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            

            try
            {

                
                //AdoTransaction adTrans = new Sybase.DataWindow.AdoTransaction(conn);
                //adTrans.BindConnection();
                //dw_1.SetTransaction(adTrans);
                //dw_1.AcceptText();
                //dw_1.UpdateData();

                //dw_1.ResetUpdateStatus();
                
                //commonclass.ShowOK();
                //dw_1.Reset();
                //dw_1.InsertRow();
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show(ex.ToString() + "\n\nNo changes made to the database.", "Unexpected Exception", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dw_1_ItemChanged(object sender, Sybase.DataWindow.ItemChangedEventArgs e)
        {
            if (e.ColumnName.ToString() == "account")
            {
                try
                {
                    dw_1.AcceptText();
                    decimal account = dw_1.GetItemDecimal(1, "account");
                    string dx = Rmb.GetChinaMoney(account);

                    dw_1.Modify("dx.text=" + "'" + dx + "'");
                }
                catch(Exception)
                {
                    dw_1.AcceptText();
                   // int unit = dw_1.GetItemDecimal(1, "unit");


                }
            }

            if (e.ColumnName.ToString() == "unit")
            {
              　
                try
                {
                    dw_1.AcceptText();
                    string szunit = string.Empty;
                    szunit=dw_1.GetItemString(1, "unit");
                    int unit = Convert.ToInt32(szunit);
                    DataSet ds = GetData(unit);
                    if (ds.Tables.Count > 0)
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dw_1.SetItemString(1, "customer", ds.Tables[0].Rows[0]["consignee"].ToString());
                            dw_1.SetItemString(1, "pici", ds.Tables[0].Rows[0]["product"].ToString());
                            dw_1.SetItemString(1, "billno", ds.Tables[0].Rows[0]["billno"].ToString());
                            dw_1.SetItemSqlDecimal(1, "account", ds.Tables[0].Rows[0]["accdaidian"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["accdaidian"]));
                            dw_1.SetItemString(1, "consignee", ds.Tables[0].Rows[0]["shipper"].ToString());
                            dw_1.SetItemString(1, "tel", ds.Tables[0].Rows[0]["shippertel"].ToString());

                            decimal account = ds.Tables[0].Rows[0]["accdaidian"] == DBNull.Value ? 0 : Convert.ToDecimal(ds.Tables[0].Rows[0]["accdaidian"]);

                            if (account == 0)
                                XtraMessageBox.Show("系统提示", "此单无货款", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            string dx = Rmb.GetChinaMoney(account);

                            dw_1.Modify("dx.text=" + "'" + dx + "'");
                        
                        }
                }
                catch (Exception)
                { }
          }
        }

        private void dw_1_EndRetrieve(object sender, EndRetrieveEventArgs e)
        {
            try
            {
                dw_1.AcceptText();
                decimal account = dw_1.GetItemDecimal(1, "account");
                string dx = Rmb.GetChinaMoney(account);

                dw_1.Modify("dx.text=" + "'" + dx + "'");
            }
            catch (Exception)
            {

            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
             
            dw_1.Print();
        }


        private DataSet GetData(int unit)
        {
            DataSet dataset = new DataSet();
            
            try
            {
               
            
                //SqlCommand sq = new SqlCommand("QSP_GET_HONGHAO_FK");
                //sq.CommandType = CommandType.StoredProcedure;
                //sq.Parameters.Add(new SqlParameter("@unit", SqlDbType.Int));

                //sq.Parameters[0].Value = unit;

                List<SqlPara> list = new List<SqlPara>();
                list.Add(new SqlPara("unit", unit));

                
                dataset.Clear();
                //dataset = cs.GetDataSet(sq,null);
                dataset = SqlHelper.GetDataSet(new SqlParasEntity(OperType.Query, "QSP_GET_HONGHAO_FK", list));
                return dataset;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                  
            
            }
            return dataset;
        }
    }
    }