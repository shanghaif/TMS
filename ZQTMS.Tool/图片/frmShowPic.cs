using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using DevExpress.XtraEditors;
using System.Threading;
using ZQTMS.Tool;
using System.IO;

namespace ZQTMS.Tool
{

   
    public partial class frmShowPic : BaseForm
    {
        //  记录x坐标: 
        private int xPos;
        //记录y坐标: 
        private int yPos;
        //记录是否按下鼠标: 
        private bool MoveFlag;

        public frmShowPic()
        {
            InitializeComponent();
        }

        public Image img = null;
        public string imgPath = "";
        public List<imgs> imgs = null;
        int imgIndex = 0;

        private void w_quicksearch_pic_Load(object sender, EventArgs e)
        {
            simpleButton4.Visible = false;
            simpleButton5.Visible = false;

            if (imgs == null) //查看一张图片
            {
                if (img != null)
                {
                    pictureBox1.Image = img;
                }
                else
                {
                    setImage(imgPath);
                }
            }
            else
            {
                if (imgs.Count > 1)
                {
                    simpleButton4.Visible = true;
                    simpleButton5.Visible = true;
                }
                //查看多张，默认打开第一张
                setImage(imgs[imgIndex].BdFileName);
            }
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            this.MouseWheel += Form1_MouseWheel;
            //设置图片在窗体居中
            pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);
            panelControl2.BringToFront();
            IamgeIdex();
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) //放大图片
            {
                pictureBox1.Size = new Size(pictureBox1.Width + 50, pictureBox1.Height + 50);
            }
            else
            {  //缩小图片
                pictureBox1.Size = new Size(pictureBox1.Width - 50, pictureBox1.Height - 50);
            }
            //设置图片在窗体居中
            pictureBox1.Location = new Point((this.Width - pictureBox1.Width) / 2, (this.Height - pictureBox1.Height) / 2);
            pictureBox1.Invalidate();
        }

        public void setImage(string _imgpath)
        {
            if (!string.IsNullOrEmpty(_imgpath))
            {
                FileStream fileStream = new FileStream(_imgpath, FileMode.Open, FileAccess.Read);

                int byteLength = (int)fileStream.Length;
                byte[] fileBytes = new byte[byteLength];
                fileStream.Read(fileBytes, 0, byteLength);

                //文件流关閉,文件解除锁定
                fileStream.Close();
                fileStream.Dispose();

                pictureBox1.Image = Image.FromStream(new MemoryStream(fileBytes));
            }
        }

        /// <summary>
        /// 上一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (imgIndex == 0)
            {
                imgIndex = imgs.Count - 1;
            }
            else
            {
                imgIndex = imgIndex - 1;
            }
            setImage(imgs[imgIndex].BdFileName);
            IamgeIdex();
        }

        /// <summary>
        /// 下一张
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            if (imgIndex == imgs.Count - 1)
            {
                imgIndex = 0;
            }
            else
            {
                imgIndex = imgIndex + 1;
            }
            setImage(imgs[imgIndex].BdFileName);
            IamgeIdex();
        }
        //yzw图片序号
        public void IamgeIdex()
        {
            if (imgs != null)
            {
                int all = imgs.Count;
                int now = imgIndex + 1;
                ImageIdx.Text = "第" + now + "/" + all + "张";

            }

            else
            {

                ImageIdx.Text = "第1/1张";

            }

        }

        private void 图片另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image == null)
                {
                    XtraMessageBox.Show("请先获取照片!", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "图片另存为";
                sfd.Filter = "图片文件|*.jpeg";
                sfd.DefaultExt = "jpeg";
                using (Bitmap bmp = new Bitmap(pictureBox1.Image))
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        bmp.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        MsgBox.ShowOK();
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowOK(ex.Message);
            }
        }

        //纵向打印
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                XtraMessageBox.Show("无打印对象，请先选择要打印的图片！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            PrintDialog MyPrintDg = new PrintDialog();
            MyPrintDg.Document = printDocument2;

            if (MyPrintDg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //设置支持纵向打印
                    PrinterSettings pss = new PrinterSettings();
                    printDocument2.PrinterSettings = pss;
                    pss.DefaultPageSettings.Landscape = false;


                    //隐藏取消打印对话框
                    PrintController printcontroller = new StandardPrintController();
                    printDocument2.PrintController = printcontroller;
                    printDocument2.Print();
                }
                catch
                {   //停止打印
                    printDocument2.PrintController.OnEndPrint(printDocument2, new System.Drawing.Printing.PrintEventArgs());
                }
            }
        }

        private void printDocument2_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0, 830, 1170);
        }

        //横向打印
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                XtraMessageBox.Show("无打印对象，请先选择要打印的图片！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PrintDialog MyPrintDg = new PrintDialog();
            MyPrintDg.Document = printDocument1;

            if (MyPrintDg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //设置支持横向打印
                    PrinterSettings pss = new PrinterSettings();
                    printDocument1.PrinterSettings = pss;
                    pss.DefaultPageSettings.Landscape = true;

                    //隐藏取消打印对话框
                    PrintController printcontroller = new StandardPrintController();
                    printDocument1.PrintController = printcontroller;
                    printDocument1.Print();
                }
                catch
                {   //停止打印
                    printDocument1.PrintController.OnEndPrint(printDocument1, new System.Drawing.Printing.PrintEventArgs());
                }
            }
        }
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0, 1180, 820);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            pictureBox1.Dispose();
            this.Close();
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetImage(pictureBox1.Image);
            }
            catch (Exception ex) { MsgBox.ShowOK("复制失败\r\n原因:" + ex.Message); }
        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            MoveFlag = true;//已经按下.
            xPos = e.X;//当前x坐标.
            yPos = e.Y;//当前y坐标.
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MoveFlag)
            {
                pictureBox1.Left += Convert.ToInt16(e.X - xPos);//设置x坐标.
                pictureBox1.Top += Convert.ToInt16(e.Y - yPos);//设置y坐标.
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            MoveFlag = false;
        }

    }

    public class imgs
    {
        private string _bdFileName;

        public string BdFileName
        {
            get { return _bdFileName; }
            set { _bdFileName = value; }
        }
    }
}