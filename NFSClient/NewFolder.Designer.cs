namespace NFSClient
{
    partial class NewFolder
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
            this.label1 = new System.Windows.Forms.Label();
            this.folderName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.UsercheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.GroupcheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.OthercheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.NewFolderCancelbut = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // folderName
            // 
            this.folderName.Location = new System.Drawing.Point(12, 25);
            this.folderName.Name = "folderName";
            this.folderName.Size = new System.Drawing.Size(184, 20);
            this.folderName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Permisions:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(205, 23);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 31);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // UsercheckedListBox
            // 
            this.UsercheckedListBox.FormattingEnabled = true;
            this.UsercheckedListBox.Items.AddRange(new object[] {
            "Read",
            "Write",
            "Execute"});
            this.UsercheckedListBox.Location = new System.Drawing.Point(12, 104);
            this.UsercheckedListBox.Name = "UsercheckedListBox";
            this.UsercheckedListBox.Size = new System.Drawing.Size(74, 49);
            this.UsercheckedListBox.TabIndex = 5;
            // 
            // GroupcheckedListBox
            // 
            this.GroupcheckedListBox.FormattingEnabled = true;
            this.GroupcheckedListBox.Items.AddRange(new object[] {
            "Read",
            "Write",
            "Execute"});
            this.GroupcheckedListBox.Location = new System.Drawing.Point(92, 104);
            this.GroupcheckedListBox.Name = "GroupcheckedListBox";
            this.GroupcheckedListBox.Size = new System.Drawing.Size(74, 49);
            this.GroupcheckedListBox.TabIndex = 6;
            // 
            // OthercheckedListBox
            // 
            this.OthercheckedListBox.FormattingEnabled = true;
            this.OthercheckedListBox.Items.AddRange(new object[] {
            "Read",
            "Write",
            "Execute"});
            this.OthercheckedListBox.Location = new System.Drawing.Point(172, 104);
            this.OthercheckedListBox.Name = "OthercheckedListBox";
            this.OthercheckedListBox.Size = new System.Drawing.Size(74, 49);
            this.OthercheckedListBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "User";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(89, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Group";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(169, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Other";
            // 
            // NewFolderCancelbut
            // 
            this.NewFolderCancelbut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.NewFolderCancelbut.Location = new System.Drawing.Point(205, 60);
            this.NewFolderCancelbut.Name = "NewFolderCancelbut";
            this.NewFolderCancelbut.Size = new System.Drawing.Size(75, 24);
            this.NewFolderCancelbut.TabIndex = 11;
            this.NewFolderCancelbut.Text = "Cancel";
            this.NewFolderCancelbut.UseVisualStyleBackColor = true;
            // 
            // NewFolder
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.NewFolderCancelbut;
            this.ClientSize = new System.Drawing.Size(292, 165);
            this.ControlBox = false;
            this.Controls.Add(this.NewFolderCancelbut);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OthercheckedListBox);
            this.Controls.Add(this.GroupcheckedListBox);
            this.Controls.Add(this.UsercheckedListBox);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.folderName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "NewFolder";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewFolder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox folderName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.CheckedListBox UsercheckedListBox;
        private System.Windows.Forms.CheckedListBox GroupcheckedListBox;
        private System.Windows.Forms.CheckedListBox OthercheckedListBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button NewFolderCancelbut;
    }
}