using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.IO;

namespace ZQTMS.Lib
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(System.Windows.Forms.ListBox))]
    public partial class FileList : UserControl
    {
        public FileList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否显示Caption
        /// </summary>
        [DefaultValue(true)]
        public bool ShowCaption
        {
            get 
            {
                return groupControl1.ShowCaption;
            }
            set
            {
                groupControl1.ShowCaption = value;
            }
        }

        public string CaptionText
        {
            get
            {
                return groupControl1.Text;
            }
            set
            {
                groupControl1.Text = value;
            }
        }

        [DefaultValue(DevExpress.Utils.Locations.Default)]
        public DevExpress.Utils.Locations CaptionLocation
        {
            get
            {
                return groupControl1.CaptionLocation;
            }
            set
            {
                groupControl1.CaptionLocation = value;
            }
        }

        [DefaultValue(typeof(Font), "Tahoma, 9pt")]
        public Font CaptionFont
        {
            get
            {
                return groupControl1.AppearanceCaption.Font;
            }
            set
            {
                groupControl1.AppearanceCaption.Font = value;
            }
        }

        [DefaultValue(typeof(Color), "Empty")]
        public Color CaptionForeColor
        {
            get
            {
                return groupControl1.AppearanceCaption.ForeColor;
            }
            set
            {
                groupControl1.AppearanceCaption.ForeColor = value;
            }
        }


        private List<ListItem> items = new List<ListItem>();

        [Browsable(false)]
        [TypeConverter(typeof(System.ComponentModel.CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<ListItem> Items
        {
            get
            {
                return items;
            }
        }

        private string filter = "图片文件(*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tif|所有文件(*.*)|*.*";
        /// <summary>
        /// 选择文件筛选字符
        /// </summary>
        public string FileFiter
        {
            get
            {
                return filter;
            }
            set
            {
                filter = value;
            }
        }

        public void AddFile(ListItem FileItem)
        {
            int count = items.Count;
            ButtonEdit button = FileItem.ButtonEdit;
            button.Size = new Size(groupControl1.Size.Width - 2, 13);
            button.Location = new Point(1, button.Height * count);
            FileItem.Index = items.Count;
            FileItem.ButtonEdit.ButtonClick += new ButtonPressedEventHandler(button_ButtonClick);
            items.Add(FileItem);
            panel1.Controls.Add(button);
        }

        private void button_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                ButtonEdit button = sender as ButtonEdit;
                ListItem item = items.Find(delegate(ListItem li) { if (li.ButtonEdit == button) { return true; } else { return false; } });
                items.Remove(item);
                button.Parent.Controls.Remove(button);
                if (item.Index < items.Count - 1)//不是最后一项
                {
                    for (int i = item.Index; i < items.Count; i++)
                    {
                        items[i].ButtonEdit.Location = new Point(1, button.Height * i);
                        items[i].Index--;
                    }
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = filter;
            if (filter.Trim() != "") of.FilterIndex = 1;
            of.Multiselect = true;
            if (of.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            
            foreach (string file in of.FileNames)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length > (long)1024 * (long)1024 * 4)
                {
                    MessageBox.Show(string.Format("文件太大，最大为4M，请检查：\r\n{0}", file));
                    return;
                }
                ListItem item = new ListItem(Path.GetFileName(file), file);
                AddFile(item);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].IsUpload)
                {
                    items.RemoveAt(i);
                }
            }
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ButtonEdit.Location = new Point(1, items[i].ButtonEdit.Height * i);
                items[i].Index = i;
            }
        }
    }

    public class ListItem
    {
        public ListItem(string FileName, string FilePath)
        {
            this.FileName = FileName;
            this.FilePath = FilePath;
            this.ButtonEdit = new ButtonEdit();
            
            this.ButtonEdit.Properties.Buttons.Clear();
            EditorButton ButtonOK = new EditorButton(ButtonPredefines.OK);
            ButtonOK.Visible = false;
            ButtonOK.ToolTip = "上传成功!";

            this.ButtonEdit.Properties.Buttons.AddRange(new EditorButton[] { ButtonOK, new EditorButton(ButtonPredefines.Delete) });
            this.ButtonEdit.Text = FileName;
            this.ButtonEdit.ToolTip = FilePath;
            this.ButtonEdit.Properties.ReadOnly = true;
            this.ButtonEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            this.ButtonEdit.BackColor = Color.White;
            this.ButtonEdit.Properties.AppearanceFocused.BackColor = Color.FromArgb(49, 106, 197);
            this.ButtonEdit.BorderStyle = BorderStyles.NoBorder;
            this.ButtonEdit.Properties.AllowFocused = false;
            this.ButtonEdit.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            this.ButtonEdit.AllowHtmlTextInToolTip = DevExpress.Utils.DefaultBoolean.True;
        }

        /// <summary>
        /// 文件名，不包含文件路径
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径，包含完整文件名
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        public DevExpress.XtraEditors.ButtonEdit ButtonEdit
        {
            get;
            set;
        }

        /// <summary>
        /// 文件索引
        /// </summary>
        public int Index
        {
            get;
            set;
        }

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool IsUpload
        {
            get;
            set;
        }
    }
}
