using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using System.IO;
using System.Reflection;
using ZQTMS.Common;
using ZQTMS.SqlDAL;

namespace ZQTMS.UI
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

            int k = 0;

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
            foreach (Control control in this.panelControl3.Controls)
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


                            if (str[j].ToString() == "frmDeparture" || str[j].ToString() == "frmShortConnOutbound" || str[j].ToString() == "frmShortConnInbound")
                            {
                                if (str[j].Contains(ds.Tables[0].Rows[i]["FormClass"].ToString()) && ds.Tables[0].Rows[i]["DllName"].ToString() == "ZQTMS.UI.BuisinessManage.dll")
                                {
                                    Controls.Find(str[j].ToString().Trim(), true)[0].Enabled = true;
                                }
                            }
                            else if (str[j].ToString() == "frmApplyList")
                            {
                                if (ds.Tables[0].Rows[i]["FormClass"].ToString() == "frmApplyList" && ds.Tables[0].Rows[i]["MenuName"].ToString() == "改单审批")
                                {
                                    frmApplyList.Enabled = true;
                                }
                            }
                            else if (str[j].ToString() == "frmApplyList2")
                            {
                                if (ds.Tables[0].Rows[i]["FormClass"].ToString() == "frmApplyList" && ds.Tables[0].Rows[i]["MenuName"].ToString() == "放货控货审批")
                                {
                                    frmApplyList2.Enabled = true;
                                }
                            }
                            else if (str[j].ToString() == "frmDeparture1")
                            {
                                if (ds.Tables[0].Rows[i]["FormClass"].ToString() == "frmDeparture" && ds.Tables[0].Rows[i]["MenuName"].ToString() == "大车费核销")
                                {
                                    frmDeparture1.Enabled = true;
                                }
                            }
                            else
                                if (str[j].Contains(ds.Tables[0].Rows[i]["FormClass"].ToString()))
                                {
                                    Controls.Find(str[j].ToString().Trim(), true)[0].Enabled = true;
                                }

                        }
                    }
                }
            }
            int gLeft = this.Width / 2 - panelControl1.Width / 2;

            int gTop = this.Height / 2 - panelControl1.Height / 2;

            panelControl1.Location = new Point(gLeft, gTop);


            int gLeft2 = this.Width / 2 - panelControl2.Width / 2;

            int gTop2 = this.Height / 2 - panelControl2.Height / 2;

            panelControl2.Location = new Point(gLeft2, gTop2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //MenuShowWindow("{根目录}\\Plugin\\*.dll","ZQTMS.UI.BuisinessManage.dll","ZQTMS.UI","WayBillRecord","",0,2);
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

        private bool showchrildnew(string aa, string menutype)
        {
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == aa)
                {
                    if (MdiParent.MdiChildren[i].Text == menutype)
                    {
                        MdiParent.MdiChildren[i].Activate();
                        return true;
                    }
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

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI." + button.Name);
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

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.FinanceManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI." + button.Name);
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
                FrmBillMiddleList frm = new FrmBillMiddleList("1");
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
                if (this.MdiParent.MdiChildren[i].Name == "frmDeparture" && !this.MdiParent.MdiChildren[i].Text.Contains("发车"))
                {
                    MdiParent.MdiChildren[i].Activate();
                    return;
                }
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.FinanceManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI.frmDeparture");
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

        private void frmBillDelivery1_Click(object sender, EventArgs e)
        {
            if (!showchrildnew("frmBillDelivery", "调度派车"))
            {
                frmBillDelivery frm = new frmBillDelivery("dispatch");
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                //frm.MenuType = "dispatch";
                frm.Show();
                frm.Focus();
            }
        }

        private void FrmBillMiddleList_Click(object sender, EventArgs e)
        {
            if (!showchrild("FrmBillMiddleList"))
            {
                FrmBillMiddleList frm = new FrmBillMiddleList("0");
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }
        }

        private void frmBillDelivery_Click(object sender, EventArgs e)
        {
            if (!showchrildnew("frmBillDelivery", "用车申请"))
            {
                frmBillDelivery frm = new frmBillDelivery("apply");
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }
        }

        private void frmDeparture_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == "frmDeparture" && !this.MdiParent.MdiChildren[i].Text.Contains("核销"))
                {
                    MdiParent.MdiChildren[i].Activate();
                    return;
                }
            }

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI.frmDeparture");
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }

        private void fmArrivedStock_Click(object sender, EventArgs e)
        {
            if (!showchrild("fmArrivedStock"))
            {
                fmArrivedStock frm = new fmArrivedStock();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();

            }
        }
        private bool showchrild(string aa, string fromText)
        {
            for (int i = 0; i < this.MdiParent.MdiChildren.Length; i++)
            {
                if (this.MdiParent.MdiChildren[i].Name == aa && this.MdiParent.MdiChildren[i].Text.Contains(fromText))
                {
                    MdiParent.MdiChildren[i].Activate();
                    return true;
                }
            }
            return false;
        }

        private void fmArrivedStock1_Click(object sender, EventArgs e)
        {
            if (!showchrild("fmArrivedStock"))
            {
                fmArrivedStock frm = new fmArrivedStock();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();

            }
        }

        private void frmWayBillAdd_JMGX_Upgrade1_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmWayBillAdd_JMGX_Upgrade"))
            {

                frmWayBillAdd_JMGX_Upgrade frm = new frmWayBillAdd_JMGX_Upgrade("1");
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();

            }
        }

        private void frmApplyList_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmApplyList", "改单申请"))
            {
                frmApplyList frm = new frmApplyList("改单申请");
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }
        }

        private void frmToOweApplyList2_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmToOweApplyList"))
            {
                frmToOweApplyList frm = new frmToOweApplyList();
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();

            }
        }
        private void showchrild4(object sender, EventArgs e)
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

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.Monitor.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI." + button.Name);
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }
        private void showchrild3(object sender, EventArgs e)
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

            Assembly ass = Assembly.LoadFrom(Application.StartupPath + "\\Plugin\\ZQTMS.UI.BuisinessManage.dll");
            if (ass == null) return;
            Type t = ass.GetType("ZQTMS.UI." + button.Name);
            if (t == null) return;
            Form frm = (Form)Activator.CreateInstance(t);
            if (frm == null) return;
            frm.MdiParent = this.MdiParent;
            frm.Dock = DockStyle.Fill;
            frm.Show();
            frm.Focus();
        }

        private void frmApplyList2_Click(object sender, EventArgs e)
        {
            if (!showchrild("frmApplyList", "控货/放货"))
            {
                frmApplyList frm = new frmApplyList("控货/放货");
                frm.MdiParent = this.MdiParent;
                frm.Dock = DockStyle.Fill;
                frm.Show();
                frm.Focus();
            }
        }

    }
}
