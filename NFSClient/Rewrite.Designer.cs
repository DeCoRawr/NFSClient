namespace NFSClient
{
    partial class Rewrite
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
            this.question = new System.Windows.Forms.Label();
            this.buttonqYes = new System.Windows.Forms.Button();
            this.buttonQall = new System.Windows.Forms.Button();
            this.buttonqNo = new System.Windows.Forms.Button();
            this.buttonqnotoal = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // question
            // 
            this.question.AutoSize = true;
            this.question.Location = new System.Drawing.Point(24, 27);
            this.question.MaximumSize = new System.Drawing.Size(200, 0);
            this.question.Name = "question";
            this.question.Size = new System.Drawing.Size(49, 13);
            this.question.TabIndex = 0;
            this.question.Text = "Question";
            // 
            // buttonqYes
            // 
            this.buttonqYes.Location = new System.Drawing.Point(27, 84);
            this.buttonqYes.Name = "buttonqYes";
            this.buttonqYes.Size = new System.Drawing.Size(57, 35);
            this.buttonqYes.TabIndex = 1;
            this.buttonqYes.Text = "Yes";
            this.buttonqYes.UseVisualStyleBackColor = true;
            this.buttonqYes.Click += new System.EventHandler(this.buttonqYes_Click);
            // 
            // buttonQall
            // 
            this.buttonQall.Location = new System.Drawing.Point(90, 84);
            this.buttonQall.Name = "buttonQall";
            this.buttonQall.Size = new System.Drawing.Size(60, 35);
            this.buttonQall.TabIndex = 2;
            this.buttonQall.Text = "Yes to all";
            this.buttonQall.UseVisualStyleBackColor = true;
            this.buttonQall.Click += new System.EventHandler(this.buttonQall_Click);
            // 
            // buttonqNo
            // 
            this.buttonqNo.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonqNo.Location = new System.Drawing.Point(223, 84);
            this.buttonqNo.Name = "buttonqNo";
            this.buttonqNo.Size = new System.Drawing.Size(54, 35);
            this.buttonqNo.TabIndex = 3;
            this.buttonqNo.Text = "No";
            this.buttonqNo.UseVisualStyleBackColor = true;
            this.buttonqNo.Click += new System.EventHandler(this.buttonqNo_Click);
            // 
            // buttonqnotoal
            // 
            this.buttonqnotoal.Location = new System.Drawing.Point(156, 84);
            this.buttonqnotoal.Name = "buttonqnotoal";
            this.buttonqnotoal.Size = new System.Drawing.Size(61, 35);
            this.buttonqnotoal.TabIndex = 4;
            this.buttonqnotoal.Text = "No to all";
            this.buttonqnotoal.UseVisualStyleBackColor = true;
            this.buttonqnotoal.Click += new System.EventHandler(this.buttonqnotoal_Click);
            // 
            // Rewrite
            // 
            this.AcceptButton = this.buttonqYes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonqNo;
            this.ClientSize = new System.Drawing.Size(302, 131);
            this.ControlBox = false;
            this.Controls.Add(this.buttonqnotoal);
            this.Controls.Add(this.buttonqNo);
            this.Controls.Add(this.buttonQall);
            this.Controls.Add(this.buttonqYes);
            this.Controls.Add(this.question);
            this.Name = "Rewrite";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rewrite";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label question;
        private System.Windows.Forms.Button buttonqYes;
        private System.Windows.Forms.Button buttonQall;
        private System.Windows.Forms.Button buttonqNo;
        private System.Windows.Forms.Button buttonqnotoal;
    }
}