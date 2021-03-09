using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KMS.Tool;
using System.IO;
using System.Reflection;
using KMS.Common;
using KMS.SqlDAL;


namespace KMS.UI
{
    public partial class w_navigation_page : BaseForm
    {
        public w_navigation_page()
        {
            InitializeComponent();

        }

        private void w_navigation_page_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            DataSet ds = CommonClass.dsMainMenu;

            string[] str = new string[100];

            int k=0;

            foreach (Control control in this.panelControl1.Controls)
            {
                if (control is Button)
                {
                    str[k] = control.Name;
                    k++;
                }
            }
            foreach (Control control in this.panelControl2.Controls)
            {
                if (control is Button)
                {
                    str[k] = control.Name;
                    k++;
                }
            }

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["FormClass"].ToString() != "")
                {
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j] != null)
                        {
                            if (str[j].Contains(ds.Tables[0].Rows[i]["FormClass"].ToString()))
                            {
                                Controls.Find(str[j].ToString().Trim(), true)[0].Enabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MenuShowWindow("{根目录}\\Plugin\\*.dll","KMS.UI.BuisinessManage.dll","KMS.UI","WayBillRecord","",0,2);
            //if (!showchrild("WayBillRecord"))
            //{
            //    frm = new WayBillRecord();
            //    frm.MdiParent = this.MdiParent;
            //    frm.Dock = DockStyle.Fill;
            //    frm.Show();
            //    frm.Focus();
            //}  
        }


        private bool showchrild(string aa)
        {
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == aa)
                {
                    MdiParent.MdiChildren[i].Activate();
                    return true;
                }
            }
            return false;
        }

        private void showchrild1(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string name = button.Name;
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == button.Name)
                {
                    MdiParent.MdiChildren[i].Activate();
                    return;
                }
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("KMS.UI."+button.Name);
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }


        private void showchrild2(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string name = button.Name;
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == button.Name)
                {
                    MdiParent.MdiChildren[i].Activate();
                    return;
                }
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.FinanceManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("KMS.UI." + button.Name);
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (!showchrild("Receiptdell"))
            {
                Receiptdell frm = new Receiptdell();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.str = "回单寄出";
                frm.Show();
                frm.Focus();
             
            }  
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (!showchrild("Receiptdell"))
            {
                Receiptdell frm = new Receiptdell();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.str = "回单返回";
                frm.Show();
                frm.Focus();
               
            }  
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (!showchrild("Receiptdell"))
            {
                Receiptdell frm = new Receiptdell();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.str = "回单返厂";
                frm.Show();
                frm.Focus();
               
            }  
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (!showchrild("fmArrivalConfirm"))
            {
                fmArrivalConfirm frm = new fmArrivalConfirm();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (!showchrild("FrmBillMiddleList"))
            {
                FrmBillMiddleList frm = new FrmBillMiddleList();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmKickbackOrCollectionOrMatPayAudit"))
            {
                frmKickbackOrCollectionOrMatPayAudit frm = new frmKickbackOrCollectionOrMatPayAudit();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void button36_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == "frmDeparture")
                {
                    MdiParent.MdiChildren[i].Activate();
                    return;
                }
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\KMS.UI.FinanceManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("KMS.UI.frmDeparture" );
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }

        private void Receiptdell_Click(object sender, EventArgs e)
        {
            if (!showchrild("Receiptdell"))
            {
                Receiptdell frm = new Receiptdell();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void frmFetchSignList1_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmSignList"))
            {
                frmSignList frm = new frmSignList();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void frmFetchSignList2_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmSignList"))
            {
                frmSignList frm = new frmSignList();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

        private void WayBillRecord1_Click(object sender, EventArgs e)
        {
            if (!showchrild("WayBillRecord"))
            {
                WayBillRecord frm = new WayBillRecord();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }  
        }

    }   
}
