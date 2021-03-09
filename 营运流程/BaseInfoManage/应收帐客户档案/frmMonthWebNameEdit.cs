using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZQTMS.Tool;
using ZQTMS.Common;

namespace ZQTMS.UI
{
    public partial class frmMonthWebNameEdit : BaseForm
    {
        private String SiteName;
        private String webName;
        public Boolean isModify = false;

        public frmMonthWebNameEdit(String SiteName, String webName)
        {
            this.SiteName = SiteName;
            this.webName = webName;
            InitializeComponent();
            bindData(SiteName, "");
            if (webName != null && webName != "")
            {
                String[] s = webName.Split(',');
                for (int i = 0; i < s.Length; i++)
                {
                    //2018.3.26wbw
                    if (s[i] != "")
                    {
                        this.addListBox.Items.Add(s[i]);
                    }
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            bindData(SiteName, tb.Text);
            
        }

        private void bindData(String SiteName, String WebName) {
            DataRow[] drs = CommonClass.GetWebs(SiteName, WebName);
            if (drs == null) return;
            searchListBox.Items.Clear();
            for (int i = 0; i < drs.Length; i++)
            {
                searchListBox.Items.Add(drs[i]["WebName"]);
            }
        }

        private void searchListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            addData();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            addData();
        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            removeData();
        }

        private void addListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            removeData();
        }

        private void addData()
        {
            try
            {
                String WebName = this.searchListBox.SelectedItem.ToString();
                if (WebName == null || WebName == "" || addListBox.Items.Contains(WebName)) return;
                this.addListBox.Items.Add(WebName);
            } catch(Exception ex)
            {
                return;
            }
        }

        private void removeData() 
        {
            try
            {
                String WebName = this.addListBox.SelectedItem.ToString();
                if (WebName == null) return;
                this.addListBox.Items.Remove(WebName);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void completeBtn_Click(object sender, EventArgs e)
        {
            String webNames = "";
            for (int i = 0; i < this.addListBox.Items.Count; i++)
            {
                webNames += this.addListBox.Items[i].ToString() + ",";
            }
            var frm = (frmBasCustContract_detail)this.Owner;
            if (webNames.Length > 0)
            {
                webNames = webNames.Substring(0, webNames.Length - 1);
            }
            frm.webNames = webNames;
            this.isModify = true;
            this.Close();
        }


    }
}
