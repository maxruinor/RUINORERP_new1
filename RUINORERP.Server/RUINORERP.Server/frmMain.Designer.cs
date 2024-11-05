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
            窗口ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            层叠排列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            水平平铺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            垂直平铺ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            toolStripddbtnDebug = new System.Windows.Forms.ToolStripDropDownButton();
            tsmY = new System.Windows.Forms.ToolStripMenuItem();
            tsmNo = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            menuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitter1
            // 
            splitter1.BackColor = System.Drawing.Color.DarkGray;
            splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitter1.Location = new System.Drawing.Point(0, 349);
            splitter1.Name = "splitter1";
            splitter1.Size = new System.Drawing.Size(800, 5);
            splitter1.TabIndex = 0;
            splitter1.TabStop = false;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            richTextBox1.Location = new System.Drawing.Point(0, 354);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(800, 96);
            richTextBox1.TabIndex = 1;
            richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 窗口ToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(800, 25);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
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
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton4, toolStripButton1, toolStripButton3, toolStripButton5, toolStripddbtnDebug });
            toolStrip1.Location = new System.Drawing.Point(0, 25);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(800, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            toolStrip1.ItemClicked += toolStrip1_ItemClicked;
            // 
            // toolStripButton4
            // 
            toolStripButton4.Image = (System.Drawing.Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new System.Drawing.Size(76, 22);
            toolStripButton4.Text = "启动服务";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(84, 22);
            toolStripButton1.Text = "在线用户管理";
            toolStripButton1.Click += toolStripButton1_Click;
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
            tsmY.Size = new System.Drawing.Size(180, 22);
            tsmY.Text = "是";
            tsmY.Click += tsmY_Click;
            // 
            // tsmNo
            // 
            tsmNo.Checked = true;
            tsmNo.CheckState = System.Windows.Forms.CheckState.Checked;
            tsmNo.Name = "tsmNo";
            tsmNo.Size = new System.Drawing.Size(180, 22);
            tsmNo.Text = "否";
            tsmNo.Click += tsmNo_Click;
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
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(toolStrip1);
            Controls.Add(splitter1);
            Controls.Add(richTextBox1);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            Name = "frmMain";
            Text = "服务管理端";
            FormClosing += frmMain_FormClosing;
            Load += frmMain_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem 窗口ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 层叠排列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 水平平铺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 垂直平铺ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripDropDownButton toolStripddbtnDebug;
        private System.Windows.Forms.ToolStripMenuItem tsmY;
        private System.Windows.Forms.ToolStripMenuItem tsmNo;
    }
}

