namespace IDS
{
    partial class MainFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MnuAddPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuEditPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuWhiteListAPerson = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuActivateIDS = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuMinimize = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.defaultCameraMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.hiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helloToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MnuTrain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.MnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnmenu = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnedit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btntrain = new System.Windows.Forms.Button();
            this.btnactivate = new System.Windows.Forms.Button();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MnuAddPerson,
            this.MnuEditPerson,
            this.toolStripMenuItem1,
            this.MnuWhiteListAPerson,
            this.toolStripMenuItem2,
            this.MnuActivateIDS,
            this.MnuMinimize,
            this.toolStripMenuItem3,
            this.defaultCameraMenu,
            this.MnuTrain,
            this.toolStripMenuItem4,
            this.MnuExit});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(200, 204);
            // 
            // MnuAddPerson
            // 
            this.MnuAddPerson.Name = "MnuAddPerson";
            this.MnuAddPerson.Size = new System.Drawing.Size(199, 22);
            this.MnuAddPerson.Text = "Add Student";
            this.MnuAddPerson.Click += new System.EventHandler(this.MnuAddPerson_Click);
            // 
            // MnuEditPerson
            // 
            this.MnuEditPerson.Name = "MnuEditPerson";
            this.MnuEditPerson.Size = new System.Drawing.Size(199, 22);
            this.MnuEditPerson.Text = "Edit Student Details";
            this.MnuEditPerson.Click += new System.EventHandler(this.MnuEditPerson_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(196, 6);
            // 
            // MnuWhiteListAPerson
            // 
            this.MnuWhiteListAPerson.Name = "MnuWhiteListAPerson";
            this.MnuWhiteListAPerson.Size = new System.Drawing.Size(199, 22);
            this.MnuWhiteListAPerson.Text = "White Or Black List";
            this.MnuWhiteListAPerson.Click += new System.EventHandler(this.MnuWhiteListAPerson_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 6);
            // 
            // MnuActivateIDS
            // 
            this.MnuActivateIDS.Name = "MnuActivateIDS";
            this.MnuActivateIDS.Size = new System.Drawing.Size(199, 22);
            this.MnuActivateIDS.Text = "Activate Camera";
            this.MnuActivateIDS.Click += new System.EventHandler(this.MnuActivateIDS_Click);
            // 
            // MnuMinimize
            // 
            this.MnuMinimize.Name = "MnuMinimize";
            this.MnuMinimize.Size = new System.Drawing.Size(199, 22);
            this.MnuMinimize.Text = "Minimize Camera";
            this.MnuMinimize.Click += new System.EventHandler(this.MnuMinimize_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(196, 6);
            // 
            // defaultCameraMenu
            // 
            this.defaultCameraMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hiToolStripMenuItem,
            this.helloToolStripMenuItem});
            this.defaultCameraMenu.Name = "defaultCameraMenu";
            this.defaultCameraMenu.Size = new System.Drawing.Size(199, 22);
            this.defaultCameraMenu.Text = "Choose Default Camera";
            // 
            // hiToolStripMenuItem
            // 
            this.hiToolStripMenuItem.Name = "hiToolStripMenuItem";
            this.hiToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hiToolStripMenuItem.Text = "Hi";
            // 
            // helloToolStripMenuItem
            // 
            this.helloToolStripMenuItem.Name = "helloToolStripMenuItem";
            this.helloToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.helloToolStripMenuItem.Text = "Hello";
            // 
            // MnuTrain
            // 
            this.MnuTrain.Name = "MnuTrain";
            this.MnuTrain.Size = new System.Drawing.Size(199, 22);
            this.MnuTrain.Text = "Capture Image";
            this.MnuTrain.Click += new System.EventHandler(this.MnuTrain_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(196, 6);
            // 
            // MnuExit
            // 
            this.MnuExit.Name = "MnuExit";
            this.MnuExit.Size = new System.Drawing.Size(199, 22);
            this.MnuExit.Text = "Exit";
            this.MnuExit.Click += new System.EventHandler(this.MnuExit_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.BalloonTipText = "Intrusion Detection System";
            this.notifyIcon.BalloonTipTitle = "Intrusion Detection System";
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.BalloonTipClosed += new System.EventHandler(this.notifyIcon_BalloonTipClosed);
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            this.notifyIcon.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseMove);
            // 
            // btnmenu
            // 
            this.btnmenu.Location = new System.Drawing.Point(12, 12);
            this.btnmenu.Name = "btnmenu";
            this.btnmenu.Size = new System.Drawing.Size(190, 27);
            this.btnmenu.TabIndex = 1;
            this.btnmenu.Text = "Menu";
            this.btnmenu.UseVisualStyleBackColor = true;
            this.btnmenu.Click += new System.EventHandler(this.btnmenu_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImage = global::IDS.Properties.Resources.face3;
            this.groupBox1.Location = new System.Drawing.Point(208, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(698, 432);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // btnedit
            // 
            this.btnedit.Location = new System.Drawing.Point(12, 110);
            this.btnedit.Name = "btnedit";
            this.btnedit.Size = new System.Drawing.Size(190, 27);
            this.btnedit.TabIndex = 3;
            this.btnedit.Text = "Edit Student Details";
            this.btnedit.UseVisualStyleBackColor = true;
            this.btnedit.Click += new System.EventHandler(this.btnedit_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(12, 60);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(190, 27);
            this.btnNew.TabIndex = 4;
            this.btnNew.Text = "Add New Student";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btntrain
            // 
            this.btntrain.Location = new System.Drawing.Point(12, 157);
            this.btntrain.Name = "btntrain";
            this.btntrain.Size = new System.Drawing.Size(190, 27);
            this.btntrain.TabIndex = 5;
            this.btntrain.Text = "Capture image";
            this.btntrain.UseVisualStyleBackColor = true;
            this.btntrain.Click += new System.EventHandler(this.btntrain_Click);
            // 
            // btnactivate
            // 
            this.btnactivate.Location = new System.Drawing.Point(12, 203);
            this.btnactivate.Name = "btnactivate";
            this.btnactivate.Size = new System.Drawing.Size(190, 27);
            this.btnactivate.TabIndex = 6;
            this.btnactivate.Text = "Activate Camera";
            this.btnactivate.UseVisualStyleBackColor = true;
            this.btnactivate.Click += new System.EventHandler(this.btnactivate_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(918, 447);
            this.Controls.Add(this.btnactivate);
            this.Controls.Add(this.btntrain);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnedit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnmenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrm";
            this.ShowInTaskbar = false;
            this.Text = "MainFrm";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainFrm_MouseClick);
            this.MouseEnter += new System.EventHandler(this.MainFrm_MouseEnter);
            this.MouseHover += new System.EventHandler(this.MainFrm_MouseHover);
            this.Resize += new System.EventHandler(this.MainFrm_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem MnuAddPerson;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MnuEditPerson;
        private System.Windows.Forms.ToolStripMenuItem MnuWhiteListAPerson;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MnuActivateIDS;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem MnuExit;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem MnuMinimize;
        private System.Windows.Forms.ToolStripMenuItem MnuTrain;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem defaultCameraMenu;
        private System.Windows.Forms.ToolStripMenuItem hiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helloToolStripMenuItem;
        private System.Windows.Forms.Button btnmenu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnedit;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btntrain;
        private System.Windows.Forms.Button btnactivate;
    }
}