namespace ZQTMS.UI
{
    partial class frmMonthWebNameEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchListBox = new System.Windows.Forms.ListBox();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.addListBox = new System.Windows.Forms.ListBox();
            this.completeBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(33, 38);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(120, 22);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // searchListBox
            // 
            this.searchListBox.FormattingEnabled = true;
            this.searchListBox.ItemHeight = 14;
            this.searchListBox.Location = new System.Drawing.Point(33, 57);
            this.searchListBox.Name = "searchListBox";
            this.searchListBox.Size = new System.Drawing.Size(120, 326);
            this.searchListBox.TabIndex = 1;
            this.searchListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.searchListBox_MouseDoubleClick);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(192, 113);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 2;
            this.addBtn.Text = "添加";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(192, 202);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(75, 23);
            this.removeBtn.TabIndex = 3;
            this.removeBtn.Text = "删除";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // addListBox
            // 
            this.addListBox.FormattingEnabled = true;
            this.addListBox.ItemHeight = 14;
            this.addListBox.Location = new System.Drawing.Point(301, 57);
            this.addListBox.Name = "addListBox";
            this.addListBox.Size = new System.Drawing.Size(120, 326);
            this.addListBox.TabIndex = 4;
            this.addListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.addListBox_MouseDoubleClick);
            // 
            // completeBtn
            // 
            this.completeBtn.Location = new System.Drawing.Point(192, 296);
            this.completeBtn.Name = "completeBtn";
            this.completeBtn.Size = new System.Drawing.Size(75, 23);
            this.completeBtn.TabIndex = 5;
            this.completeBtn.Text = "完成";
            this.completeBtn.UseVisualStyleBackColor = true;
            this.completeBtn.Click += new System.EventHandler(this.completeBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "已选：";
            // 
            // frmMonthWebNameEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 405);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.completeBtn);
            this.Controls.Add(this.addListBox);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.searchListBox);
            this.Controls.Add(this.searchTextBox);
            this.Name = "frmMonthWebNameEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "月结网点";
            //this.Load += new System.EventHandler(this.frmMonthWebNameEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.ListBox searchListBox;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
        private System.Windows.Forms.ListBox addListBox;
        private System.Windows.Forms.Button completeBtn;
        private System.Windows.Forms.Label label1;
    }
}