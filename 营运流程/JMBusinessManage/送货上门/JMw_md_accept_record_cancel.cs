using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.Collections;
using ZQTMS.Tool;


namespace ZQTMS
{
    public partial class JMw_md_accept_record_cancel : BaseForm
    {
        DataSet ds = new DataSet();

        public int type = 0;

        public JMw_md_accept_record_cancel()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

             
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (bdate.DateTime.Date > edate.DateTime.Date) 
            {
            
            }
           
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tvbsite_AfterSelect(object sender, TreeViewEventArgs e)
        {
 
        }

        private void tvesite_AfterSelect(object sender, TreeViewEventArgs e)
        {
 
        }

        private void tvbsite_MouseClick(object sender, MouseEventArgs e)
        { 
        }

        private void tvesite_MouseClick(object sender, MouseEventArgs e)
        { 
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            //cc.ShowBillDetail(gridView1);
        }

        private void barCheckItem3_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.AllowAutoFilter(gridView1);
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.SaveGridLayout(gridshow, "门店到站分流退货记录", true);
        }

        private void barCheckItem4_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            //cc.ShowAutoFilterRow(gridView1);
        }

        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.DeleteGridLayout(gridshow, "门店到站分流退货记录");
        }

        private void barButtonItem12_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.ExportToExcel(gridView1);
        }

        private void barButtonItem13_ItemClick(object sender, ItemClickEventArgs e)
        {
            //cc.QuickSearch();
        }

        private void barButtonItem14_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Close();
        }
          
       
         
    }
}