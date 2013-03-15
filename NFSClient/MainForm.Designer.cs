namespace NFSClient
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.button_connect = new System.Windows.Forms.Button();
            this.cboxRemoteDevices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewRemote = new System.Windows.Forms.ListView();
            this.Namevcn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Sizenmn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ADate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rightClickRemote = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.listViewLocal = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rightClickLocal = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFolderToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tbLocalPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.altF4Button = new System.Windows.Forms.Button();
            this.f8DeleteButton = new System.Windows.Forms.Button();
            this.f7newfolderbutton = new System.Windows.Forms.Button();
            this.f6MoveButton = new System.Windows.Forms.Button();
            this.f5CopyButton = new System.Windows.Forms.Button();
            this.F4EditButton = new System.Windows.Forms.Button();
            this.F3ViewButton = new System.Windows.Forms.Button();
            this.StatusLog = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addServer = new System.Windows.Forms.Button();
            this.serverListCombo = new System.Windows.Forms.ComboBox();
            this.editServer = new System.Windows.Forms.Button();
            this.mountDrive = new System.Windows.Forms.Button();
            this.rightClickRemote.SuspendLayout();
            this.rightClickLocal.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(596, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Servers:";
            // 
            // button_connect
            // 
            this.button_connect.BackColor = System.Drawing.SystemColors.Control;
            this.button_connect.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_connect.Location = new System.Drawing.Point(898, 27);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 31);
            this.button_connect.TabIndex = 4;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_event);
            // 
            // cboxRemoteDevices
            // 
            this.cboxRemoteDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxRemoteDevices.FormattingEnabled = true;
            this.cboxRemoteDevices.Location = new System.Drawing.Point(470, 33);
            this.cboxRemoteDevices.Name = "cboxRemoteDevices";
            this.cboxRemoteDevices.Size = new System.Drawing.Size(120, 21);
            this.cboxRemoteDevices.TabIndex = 5;
            this.cboxRemoteDevices.Visible = false;
            this.cboxRemoteDevices.SelectedIndexChanged += new System.EventHandler(this.cboxRemoteDevices_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(391, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Remote point:";
            this.label2.Visible = false;
            // 
            // listViewRemote
            // 
            this.listViewRemote.AllowDrop = true;
            this.listViewRemote.BackColor = System.Drawing.Color.Black;
            this.listViewRemote.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Namevcn,
            this.Sizenmn,
            this.columnHeader4,
            this.MDate,
            this.ADate});
            this.listViewRemote.ContextMenuStrip = this.rightClickRemote;
            this.listViewRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewRemote.ForeColor = System.Drawing.Color.Lime;
            this.listViewRemote.HideSelection = false;
            this.listViewRemote.Location = new System.Drawing.Point(0, 0);
            this.listViewRemote.Name = "listViewRemote";
            this.listViewRemote.Size = new System.Drawing.Size(525, 411);
            this.listViewRemote.SmallImageList = this.imageList1;
            this.listViewRemote.TabIndex = 7;
            this.listViewRemote.UseCompatibleStateImageBehavior = false;
            this.listViewRemote.View = System.Windows.Forms.View.Details;
            this.listViewRemote.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewRemote_AfterLabelEdit);
            this.listViewRemote.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewRemote_ItemDrag);
            this.listViewRemote.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewRemote_DragDrop);
            this.listViewRemote.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewRemote_DragEnter);
            this.listViewRemote.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewRemote_DragOver);
            this.listViewRemote.DoubleClick += new System.EventHandler(this.listViewRemote_DoubleClick);
            this.listViewRemote.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewRemote_KeyDown);
            // 
            // Namevcn
            // 
            this.Namevcn.Text = "Name";
            this.Namevcn.Width = 179;
            // 
            // Sizenmn
            // 
            this.Sizenmn.Text = "Size (B)";
            this.Sizenmn.Width = 65;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Attributes";
            this.columnHeader4.Width = 78;
            // 
            // MDate
            // 
            this.MDate.Text = "Mdate";
            this.MDate.Width = 90;
            // 
            // ADate
            // 
            this.ADate.Text = "ADate";
            this.ADate.Width = 90;
            // 
            // rightClickRemote
            // 
            this.rightClickRemote.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFolderToolStripMenuItem,
            this.newFolderToolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.rightClickRemote.Name = "rightClickRemote";
            this.rightClickRemote.Size = new System.Drawing.Size(138, 70);
            // 
            // newFolderToolStripMenuItem
            // 
            this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
            this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.newFolderToolStripMenuItem.Text = "Select All";
            this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
            // 
            // newFolderToolStripMenuItem1
            // 
            this.newFolderToolStripMenuItem1.Name = "newFolderToolStripMenuItem1";
            this.newFolderToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.newFolderToolStripMenuItem1.Text = "New folder";
            this.newFolderToolStripMenuItem1.Click += new System.EventHandler(this.btnNewFolder_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "unknown.png");
            this.imageList1.Images.SetKeyName(1, "folder.gif");
            // 
            // listViewLocal
            // 
            this.listViewLocal.AllowDrop = true;
            this.listViewLocal.BackColor = System.Drawing.Color.Black;
            this.listViewLocal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listViewLocal.ContextMenuStrip = this.rightClickLocal;
            this.listViewLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLocal.ForeColor = System.Drawing.Color.Lime;
            this.listViewLocal.HideSelection = false;
            this.listViewLocal.Location = new System.Drawing.Point(0, 0);
            this.listViewLocal.Name = "listViewLocal";
            this.listViewLocal.Size = new System.Drawing.Size(477, 411);
            this.listViewLocal.SmallImageList = this.imageList1;
            this.listViewLocal.TabIndex = 8;
            this.listViewLocal.UseCompatibleStateImageBehavior = false;
            this.listViewLocal.View = System.Windows.Forms.View.Details;
            this.listViewLocal.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listViewLocal_AfterLabelEdit);
            this.listViewLocal.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewLocal_ItemDrag);
            this.listViewLocal.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewLocal_DragDrop);
            this.listViewLocal.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewLocal_DragEnter);
            this.listViewLocal.DragOver += new System.Windows.Forms.DragEventHandler(this.listViewLocal_DragOver);
            this.listViewLocal.DoubleClick += new System.EventHandler(this.listViewLocal_DoubleClick);
            this.listViewLocal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewLocal_KeyDown);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 240;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Size (B)";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Date";
            this.columnHeader3.Width = 115;
            // 
            // rightClickLocal
            // 
            this.rightClickLocal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.newFolderToolStripMenuItem2,
            this.refreshToolStripMenuItem1});
            this.rightClickLocal.Name = "rightClickLocal";
            this.rightClickLocal.Size = new System.Drawing.Size(137, 70);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.selectAllToolStripMenuItem.Text = "Select all";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // newFolderToolStripMenuItem2
            // 
            this.newFolderToolStripMenuItem2.Name = "newFolderToolStripMenuItem2";
            this.newFolderToolStripMenuItem2.Size = new System.Drawing.Size(136, 22);
            this.newFolderToolStripMenuItem2.Text = "NewFolder";
            this.newFolderToolStripMenuItem2.Click += new System.EventHandler(this.newFolderToolStripMenuItem2_Click);
            // 
            // refreshToolStripMenuItem1
            // 
            this.refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            this.refreshToolStripMenuItem1.Size = new System.Drawing.Size(136, 22);
            this.refreshToolStripMenuItem1.Text = "Refresh";
            this.refreshToolStripMenuItem1.Click += new System.EventHandler(this.refreshToolStripMenuItem1_Click);
            // 
            // tbLocalPath
            // 
            this.tbLocalPath.Location = new System.Drawing.Point(45, 32);
            this.tbLocalPath.Name = "tbLocalPath";
            this.tbLocalPath.Size = new System.Drawing.Size(268, 20);
            this.tbLocalPath.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Path:";
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(114, 135);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(331, 23);
            this.pb.TabIndex = 11;
            this.pb.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(319, 30);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(66, 23);
            this.btnBrowse.TabIndex = 14;
            this.btnBrowse.Text = "Browse....";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Location = new System.Drawing.Point(0, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1011, 414);
            this.panel1.TabIndex = 17;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(1, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewLocal);
            this.splitContainer1.Panel1.Controls.Add(this.pb);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listViewRemote);
            this.splitContainer1.Size = new System.Drawing.Size(1006, 411);
            this.splitContainer1.SplitterDistance = 477;
            this.splitContainer1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.altF4Button);
            this.panel2.Controls.Add(this.f8DeleteButton);
            this.panel2.Controls.Add(this.f7newfolderbutton);
            this.panel2.Controls.Add(this.f6MoveButton);
            this.panel2.Controls.Add(this.f5CopyButton);
            this.panel2.Controls.Add(this.F4EditButton);
            this.panel2.Controls.Add(this.F3ViewButton);
            this.panel2.Controls.Add(this.StatusLog);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 478);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1011, 71);
            this.panel2.TabIndex = 19;
            // 
            // altF4Button
            // 
            this.altF4Button.Location = new System.Drawing.Point(860, 3);
            this.altF4Button.Name = "altF4Button";
            this.altF4Button.Size = new System.Drawing.Size(135, 23);
            this.altF4Button.TabIndex = 7;
            this.altF4Button.Text = "Alt + F4 - Exit";
            this.altF4Button.UseVisualStyleBackColor = true;
            this.altF4Button.Click += new System.EventHandler(this.altf4Button);
            // 
            // f8DeleteButton
            // 
            this.f8DeleteButton.Location = new System.Drawing.Point(719, 3);
            this.f8DeleteButton.Name = "f8DeleteButton";
            this.f8DeleteButton.Size = new System.Drawing.Size(135, 23);
            this.f8DeleteButton.TabIndex = 6;
            this.f8DeleteButton.Text = "F8 - Delete";
            this.f8DeleteButton.UseVisualStyleBackColor = true;
            this.f8DeleteButton.Click += new System.EventHandler(this.f8DeleteButton_Click);
            // 
            // f7newfolderbutton
            // 
            this.f7newfolderbutton.Location = new System.Drawing.Point(579, 3);
            this.f7newfolderbutton.Name = "f7newfolderbutton";
            this.f7newfolderbutton.Size = new System.Drawing.Size(136, 23);
            this.f7newfolderbutton.TabIndex = 5;
            this.f7newfolderbutton.Text = "F7 - New folder";
            this.f7newfolderbutton.UseVisualStyleBackColor = true;
            this.f7newfolderbutton.Click += new System.EventHandler(this.newfolderf7_Click);
            // 
            // f6MoveButton
            // 
            this.f6MoveButton.Location = new System.Drawing.Point(438, 3);
            this.f6MoveButton.Name = "f6MoveButton";
            this.f6MoveButton.Size = new System.Drawing.Size(135, 23);
            this.f6MoveButton.TabIndex = 4;
            this.f6MoveButton.Text = "F6 - Move";
            this.f6MoveButton.UseVisualStyleBackColor = true;
            this.f6MoveButton.Click += new System.EventHandler(this.f6MoveButton_Click);
            // 
            // f5CopyButton
            // 
            this.f5CopyButton.Location = new System.Drawing.Point(297, 3);
            this.f5CopyButton.Name = "f5CopyButton";
            this.f5CopyButton.Size = new System.Drawing.Size(135, 23);
            this.f5CopyButton.TabIndex = 3;
            this.f5CopyButton.Text = "F5 - Copy";
            this.f5CopyButton.UseVisualStyleBackColor = true;
            this.f5CopyButton.Click += new System.EventHandler(this.f5CopyButton_Click);
            // 
            // F4EditButton
            // 
            this.F4EditButton.Location = new System.Drawing.Point(156, 3);
            this.F4EditButton.Name = "F4EditButton";
            this.F4EditButton.Size = new System.Drawing.Size(135, 23);
            this.F4EditButton.TabIndex = 2;
            this.F4EditButton.Text = "F4 - Edit";
            this.F4EditButton.UseVisualStyleBackColor = true;
            this.F4EditButton.Click += new System.EventHandler(this.F4EditButton_Click);
            // 
            // F3ViewButton
            // 
            this.F3ViewButton.Location = new System.Drawing.Point(15, 3);
            this.F3ViewButton.Name = "F3ViewButton";
            this.F3ViewButton.Size = new System.Drawing.Size(135, 23);
            this.F3ViewButton.TabIndex = 1;
            this.F3ViewButton.Text = "F3 - View";
            this.F3ViewButton.UseVisualStyleBackColor = true;
            this.F3ViewButton.Click += new System.EventHandler(this.F3ViewButton_Click);
            // 
            // StatusLog
            // 
            this.StatusLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StatusLog.BackColor = System.Drawing.Color.Black;
            this.StatusLog.ForeColor = System.Drawing.Color.Lime;
            this.StatusLog.FormattingEnabled = true;
            this.StatusLog.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.StatusLog.Location = new System.Drawing.Point(0, 28);
            this.StatusLog.Name = "StatusLog";
            this.StatusLog.Size = new System.Drawing.Size(1011, 43);
            this.StatusLog.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1011, 24);
            this.menuStrip1.TabIndex = 25;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.connectToolStripMenuItem.Text = "Connect / Disconnect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // addServer
            // 
            this.addServer.Location = new System.Drawing.Point(799, 27);
            this.addServer.Name = "addServer";
            this.addServer.Size = new System.Drawing.Size(43, 31);
            this.addServer.TabIndex = 26;
            this.addServer.Text = "Add";
            this.addServer.UseVisualStyleBackColor = true;
            this.addServer.Click += new System.EventHandler(this.addServer_Click);
            // 
            // serverListCombo
            // 
            this.serverListCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serverListCombo.FormattingEnabled = true;
            this.serverListCombo.Location = new System.Drawing.Point(648, 33);
            this.serverListCombo.Name = "serverListCombo";
            this.serverListCombo.Size = new System.Drawing.Size(145, 21);
            this.serverListCombo.TabIndex = 27;
            this.serverListCombo.SelectedIndexChanged += new System.EventHandler(this.serverListCombo_SelectedIndexChanged);
            // 
            // editServer
            // 
            this.editServer.Location = new System.Drawing.Point(848, 27);
            this.editServer.Name = "editServer";
            this.editServer.Size = new System.Drawing.Size(44, 31);
            this.editServer.TabIndex = 28;
            this.editServer.Text = "Edit";
            this.editServer.UseVisualStyleBackColor = true;
            this.editServer.Click += new System.EventHandler(this.editServer_Click);
            // 
            // mountDrive
            // 
            this.mountDrive.Location = new System.Drawing.Point(979, 27);
            this.mountDrive.Name = "mountDrive";
            this.mountDrive.Size = new System.Drawing.Size(28, 31);
            this.mountDrive.TabIndex = 29;
            this.mountDrive.Text = "Z:";
            this.mountDrive.UseVisualStyleBackColor = true;
            this.mountDrive.Click += new System.EventHandler(this.mountDrive_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 549);
            this.Controls.Add(this.mountDrive);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.editServer);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.addServer);
            this.Controls.Add(this.tbLocalPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverListCombo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboxRemoteDevices);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "NFS Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.rightClickRemote.ResumeLayout(false);
            this.rightClickLocal.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.ComboBox cboxRemoteDevices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView listViewRemote;
        private System.Windows.Forms.ListView listViewLocal;
        public System.Windows.Forms.TextBox tbLocalPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        public System.Windows.Forms.ColumnHeader Namevcn;
        private System.Windows.Forms.ColumnHeader Sizenmn;
        private System.Windows.Forms.ColumnHeader MDate;
        private System.Windows.Forms.ColumnHeader ADate;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ContextMenuStrip rightClickRemote;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip rightClickLocal;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox StatusLog;
        private System.Windows.Forms.Button F3ViewButton;
        private System.Windows.Forms.Button altF4Button;
        private System.Windows.Forms.Button f8DeleteButton;
        private System.Windows.Forms.Button f7newfolderbutton;
        private System.Windows.Forms.Button f6MoveButton;
        private System.Windows.Forms.Button f5CopyButton;
        private System.Windows.Forms.Button F4EditButton;
        private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Button addServer;
        public System.Windows.Forms.ComboBox serverListCombo;
        private System.Windows.Forms.Button editServer;
        private System.Windows.Forms.Button mountDrive;
    }
}

