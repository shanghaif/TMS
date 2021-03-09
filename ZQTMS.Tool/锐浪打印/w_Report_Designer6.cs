using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using gregn6Lib;
using System.IO;
using ZQTMS.Common;
//using ZQTMS.Common;
namespace ZQTMS.Tool
{
    public partial class w_Report_Designer6 : BaseForm
    {
        public w_Report_Designer6()
        {
            InitializeComponent();
            axGRDesigner1.Report = Report;
            openFileDialog1.Filter = "模板文件(*.grf)|*.grf";
            openFileDialog1.InitialDirectory = Application.StartupPath;
        }
        public string grfpath;
        private GridppReport Report = new GridppReport();

        private void w_Report_Designer6_Load(object sender, EventArgs e)
        {
            CommonClass.InsertLog("报表设计");//xj2019/5/29
            if (!string.IsNullOrEmpty(grfpath))
            {
                if (!File.Exists(Application.StartupPath + @"\" + grfpath))
                {
                    return;
                }
                Report.LoadFromFile(Application.StartupPath + @"\" + grfpath);
                axGRDesigner1.Reload();
                Text = "模板设计【" + grfpath + "】";
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Report.LoadFromFile(openFileDialog1.FileName);
                axGRDesigner1.Reload();
            }
            Text = "模板设计【" + openFileDialog1.SafeFileName + "】";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName != "")
            {
                axGRDesigner1.Post();
                Report.SaveToFile(openFileDialog1.FileName);
                MsgBox.ShowOK("保存成功");
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = openFileDialog1.FileName;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                axGRDesigner1.Post();
                Report.SaveToFile(saveFileDialog1.FileName);
            }
        }

        private void axGRDesigner1_OpenReport(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Report.LoadFromFile(openFileDialog1.FileName);
                axGRDesigner1.Reload();
            }

            //将 DefaultAction 属性为假, 忽略掉设计器控件本身的打开行为
            axGRDesigner1.DefaultAction = false;
        }

        private void axGRDesigner1_SaveReport(object sender, EventArgs e)
        {
            bool ToSave = true;
            saveFileDialog1.FileName = openFileDialog1.FileName;
            if (saveFileDialog1.FileName == "")
                ToSave = saveFileDialog1.ShowDialog() == DialogResult.OK;

            if (ToSave)
            {
                axGRDesigner1.Post();
                Report.SaveToFile(saveFileDialog1.FileName);
            }

            //将 DefaultAction 属性为假, 忽略掉设计器控件本身的保存行为
            axGRDesigner1.DefaultAction = false;
        }

        private void w_Report_Designer6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                simpleButton2.PerformClick();
        }

        private void axGRDesigner1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                simpleButton2.PerformClick();
        }
    }
}