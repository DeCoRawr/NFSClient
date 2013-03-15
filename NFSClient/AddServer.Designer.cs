namespace NFSClient
{
    partial class AddServer
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
            this.justConnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.defServerAdr = new System.Windows.Forms.TextBox();
            this.saveServer = new System.Windows.Forms.Button();
            this.newProfileName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nfsProto = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.GidConnect = new System.Windows.Forms.TextBox();
            this.securePort = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.uidConnect = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // justConnect
            // 
            this.justConnect.Location = new System.Drawing.Point(191, 223);
            this.justConnect.Name = "justConnect";
            this.justConnect.Size = new System.Drawing.Size(103, 31);
            this.justConnect.TabIndex = 46;
            this.justConnect.Text = "One time Connect";
            this.justConnect.UseVisualStyleBackColor = true;
            this.justConnect.Click += new System.EventHandler(this.justConnect_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Server adress:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 28);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 13);
            this.label14.TabIndex = 44;
            this.label14.Text = "Server name:";
            // 
            // defServerAdr
            // 
            this.defServerAdr.Location = new System.Drawing.Point(128, 54);
            this.defServerAdr.Name = "defServerAdr";
            this.defServerAdr.Size = new System.Drawing.Size(254, 20);
            this.defServerAdr.TabIndex = 32;
            // 
            // saveServer
            // 
            this.saveServer.Location = new System.Drawing.Point(116, 223);
            this.saveServer.Name = "saveServer";
            this.saveServer.Size = new System.Drawing.Size(69, 31);
            this.saveServer.TabIndex = 43;
            this.saveServer.Text = "Save";
            this.saveServer.UseVisualStyleBackColor = true;
            this.saveServer.Click += new System.EventHandler(this.saveServer_Click);
            // 
            // newProfileName
            // 
            this.newProfileName.Location = new System.Drawing.Point(127, 21);
            this.newProfileName.Name = "newProfileName";
            this.newProfileName.Size = new System.Drawing.Size(255, 20);
            this.newProfileName.TabIndex = 42;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "NFS Protocol:";
            // 
            // nfsProto
            // 
            this.nfsProto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nfsProto.FormattingEnabled = true;
            this.nfsProto.Items.AddRange(new object[] {
            "v2",
            "v3",
            "v4.1"});
            this.nfsProto.Location = new System.Drawing.Point(130, 86);
            this.nfsProto.Name = "nfsProto";
            this.nfsProto.Size = new System.Drawing.Size(58, 21);
            this.nfsProto.TabIndex = 34;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Use secure port:";
            // 
            // GidConnect
            // 
            this.GidConnect.Location = new System.Drawing.Point(131, 172);
            this.GidConnect.Name = "GidConnect";
            this.GidConnect.Size = new System.Drawing.Size(47, 20);
            this.GidConnect.TabIndex = 40;
            this.GidConnect.Text = "0";
            // 
            // securePort
            // 
            this.securePort.AutoSize = true;
            this.securePort.Checked = true;
            this.securePort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.securePort.Location = new System.Drawing.Point(130, 115);
            this.securePort.Name = "securePort";
            this.securePort.Size = new System.Drawing.Size(15, 14);
            this.securePort.TabIndex = 36;
            this.securePort.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 179);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(116, 13);
            this.label11.TabIndex = 39;
            this.label11.Text = "Group id to connect to:";
            // 
            // uidConnect
            // 
            this.uidConnect.Location = new System.Drawing.Point(130, 139);
            this.uidConnect.Name = "uidConnect";
            this.uidConnect.Size = new System.Drawing.Size(47, 20);
            this.uidConnect.TabIndex = 38;
            this.uidConnect.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 146);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(109, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "User id to connect to:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(239, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Use local port from 665 to 1023 ( keep checked )";
            // 
            // AddServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(399, 277);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.justConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.defServerAdr);
            this.Controls.Add(this.saveServer);
            this.Controls.Add(this.newProfileName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nfsProto);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.GidConnect);
            this.Controls.Add(this.securePort);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.uidConnect);
            this.Controls.Add(this.label10);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddServer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddServer";
            this.Load += new System.EventHandler(this.AddServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button justConnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox defServerAdr;
        private System.Windows.Forms.Button saveServer;
        private System.Windows.Forms.TextBox newProfileName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox nfsProto;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox GidConnect;
        private System.Windows.Forms.CheckBox securePort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox uidConnect;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
    }
}