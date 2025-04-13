namespace RUINORERP.Server
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            splitter1 = new System.Windows.Forms.Splitter();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            参数配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            层叠排列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            水平平铺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            垂直平铺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsBtnStartServer = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            toolStripddbtnDebug = new System.Windows.Forms.ToolStripDropDownButton();
            tsmY = new System.Windows.Forms.ToolStripMenuItem();
            tsmNo = new System.Windows.Forms.ToolStripMenuItem();
            tsbtnDataViewer = new System.Windows.Forms.ToolStripButton();
            tsbtn在线用户 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            系统注册ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            tslblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            黑名单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            menuStrip2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.BackColor = System.Drawing.Color.DarkGray;
            splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitter1.Location = new System.Drawing.Point(0, 322);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(800, 5);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            richTextBox1.Location = new System.Drawing.Point(0, 327);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(800, 101);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 参数配置ToolStripMenuItem, 窗口ToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 25);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(800, 25);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // 参数配置ToolStripMenuItem
            // 
            参数配置ToolStripMenuItem.Name = "参数配置ToolStripMenuItem";
            参数配置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            参数配置ToolStripMenuItem.Text = "参数配置";
            参数配置ToolStripMenuItem.Click += 参数配置ToolStripMenuItem_Click;
            // 
            // 窗口ToolStripMenuItem
            // 
            窗口ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 层叠排列ToolStripMenuItem, 水平平铺ToolStripMenuItem, 垂直平铺ToolStripMenuItem, 关闭ToolStripMenuItem });
            窗口ToolStripMenuItem.Name = "窗口ToolStripMenuItem";
            窗口ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            窗口ToolStripMenuItem.Text = "【窗口】";
            // 
            // 层叠排列ToolStripMenuItem
            // 
            层叠排列ToolStripMenuItem.Name = "层叠排列ToolStripMenuItem";
            层叠排列ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            层叠排列ToolStripMenuItem.Text = "层叠排列";
            层叠排列ToolStripMenuItem.Click += 层叠排列ToolStripMenuItem_Click;
            // 
            // 水平平铺ToolStripMenuItem
            // 
            水平平铺ToolStripMenuItem.Name = "水平平铺ToolStripMenuItem";
            水平平铺ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            水平平铺ToolStripMenuItem.Text = "水平平铺";
            水平平铺ToolStripMenuItem.Click += 水平平铺ToolStripMenuItem_Click;
            // 
            // 垂直平铺ToolStripMenuItem
            // 
            垂直平铺ToolStripMenuItem.Name = "垂直平铺ToolStripMenuItem";
            垂直平铺ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            垂直平铺ToolStripMenuItem.Text = "垂直平铺";
            垂直平铺ToolStripMenuItem.Click += 垂直平铺ToolStripMenuItem_Click;
            // 
            // 关闭ToolStripMenuItem
            // 
            关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            关闭ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            关闭ToolStripMenuItem.Text = "关闭";
            关闭ToolStripMenuItem.Click += 关闭ToolStripMenuItem_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsBtnStartServer, toolStripButton3, toolStripButton5, toolStripddbtnDebug, tsbtnDataViewer, tsbtn在线用户 });
            toolStrip1.Location = new System.Drawing.Point(0, 50);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(800, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            toolStrip1.ItemClicked += toolStrip1_ItemClicked;
            // 
            // tsBtnStartServer
            // 
            tsBtnStartServer.Image = Properties.Resources.foward2;
            tsBtnStartServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsBtnStartServer.Name = "tsBtnStartServer";
            tsBtnStartServer.Size = new System.Drawing.Size(88, 22);
            tsBtnStartServer.Text = "启动服务器";
            tsBtnStartServer.Click += tsBtnStartServer_Click;
            // 
            // toolStripButton3
            // 
            toolStripButton3.Image = (System.Drawing.Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new System.Drawing.Size(88, 22);
            toolStripButton3.Text = "工作流管理";
            toolStripButton3.Click += toolStripButton3_Click;
            // 
            // toolStripButton5
            // 
            toolStripButton5.Image = (System.Drawing.Image)resources.GetObject("toolStripButton5.Image");
            toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton5.Name = "toolStripButton5";
            toolStripButton5.Size = new System.Drawing.Size(76, 22);
            toolStripButton5.Text = "缓存管理";
            toolStripButton5.Click += toolStripButton5_Click;
            // 
            // toolStripddbtnDebug
            // 
            toolStripddbtnDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            toolStripddbtnDebug.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmY, tsmNo });
            toolStripddbtnDebug.Image = (System.Drawing.Image)resources.GetObject("toolStripddbtnDebug.Image");
            toolStripddbtnDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripddbtnDebug.Name = "toolStripddbtnDebug";
            toolStripddbtnDebug.Size = new System.Drawing.Size(85, 22);
            toolStripddbtnDebug.Text = "调式模式";
            // 
            // tsmY
            // 
            tsmY.Name = "tsmY";
            tsmY.Size = new System.Drawing.Size(88, 22);
            tsmY.Text = "是";
            tsmY.Click += tsmY_Click;
            // 
            // tsmNo
            // 
            tsmNo.Checked = true;
            tsmNo.CheckState = System.Windows.Forms.CheckState.Checked;
            tsmNo.Name = "tsmNo";
            tsmNo.Size = new System.Drawing.Size(88, 22);
            tsmNo.Text = "否";
            tsmNo.Click += tsmNo_Click;
            // 
            // tsbtnDataViewer
            // 
            tsbtnDataViewer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnDataViewer.Image = (System.Drawing.Image)resources.GetObject("tsbtnDataViewer.Image");
            tsbtnDataViewer.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnDataViewer.Name = "tsbtnDataViewer";
            tsbtnDataViewer.Size = new System.Drawing.Size(72, 22);
            tsbtnDataViewer.Text = "数据查看器";
            tsbtnDataViewer.Click += tsbtnDataViewer_Click;
            // 
            // tsbtn在线用户
            // 
            tsbtn在线用户.Image = (System.Drawing.Image)resources.GetObject("tsbtn在线用户.Image");
            tsbtn在线用户.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn在线用户.Name = "tsbtn在线用户";
            tsbtn在线用户.Size = new System.Drawing.Size(76, 22);
            tsbtn在线用户.Text = "在线用户";
            tsbtn在线用户.Click += tsbtn在线用户_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (System.Drawing.Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new System.Drawing.Size(23, 22);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // menuStrip2
            // 
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 系统注册ToolStripMenuItem, 黑名单ToolStripMenuItem });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new System.Drawing.Size(800, 25);
            menuStrip2.TabIndex = 7;
            menuStrip2.Text = "menuStrip2";
            // 
            // 系统注册ToolStripMenuItem
            // 
            系统注册ToolStripMenuItem.Name = "系统注册ToolStripMenuItem";
            系统注册ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            系统注册ToolStripMenuItem.Text = "系统注册";
            系统注册ToolStripMenuItem.Click += 系统注册ToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tslblStatus });
            statusStrip1.Location = new System.Drawing.Point(0, 428);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(800, 22);
            statusStrip1.TabIndex = 9;
            statusStrip1.Text = "statusStrip1";
            // 
            // tslblStatus
            // 
            tslblStatus.Name = "tslblStatus";
            tslblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // 黑名单ToolStripMenuItem
            // 
            黑名单ToolStripMenuItem.Name = "黑名单ToolStripMenuItem";
            黑名单ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            黑名单ToolStripMenuItem.Text = "黑名单";
            黑名单ToolStripMenuItem.Click += 黑名单ToolStripMenuItem_Click;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(toolStrip1);
            Controls.Add(splitter1);
            Controls.Add(richTextBox1);
            Controls.Add(menuStrip1);
            Controls.Add(menuStrip2);
            Controls.Add(statusStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MainMenuStrip = menuStrip2;
            Name = "frmMain";
            Text = "服务管理端2.1";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton tsBtnStartServer;
        private System.Windows.Forms.ToolStripMenuItem 窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 层叠排列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 水平平铺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 垂直平铺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripDropDownButton toolStripddbtnDebug;
        private System.Windows.Forms.ToolStripMenuItem tsmY;
        private System.Windows.Forms.ToolStripMenuItem tsmNo;
        private System.Windows.Forms.ToolStripButton tsbtnDataViewer;
        private System.Windows.Forms.ToolStripMenuItem 参数配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统注册ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslblStatus;
        private System.Windows.Forms.ToolStripButton tsbtn在线用户;
        private System.Windows.Forms.ToolStripMenuItem 黑名单ToolStripMenuItem;
    }
}

