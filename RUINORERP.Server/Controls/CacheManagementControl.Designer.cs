namespace RUINORERP.Server.Controls
{
    partial class CacheManagementControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton加载缓存 = new System.Windows.Forms.ToolStripButton();
            toolStripButton刷新缓存 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            cmbUser = new System.Windows.Forms.ToolStripComboBox();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPageCacheManagement = new System.Windows.Forms.TabPage();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            listBoxTableList = new System.Windows.Forms.ListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            加载缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            tabPageCacheStatistics = new System.Windows.Forms.TabPage();
            panel1 = new System.Windows.Forms.Panel();
            btnResetStatistics = new System.Windows.Forms.Button();
            btnRefreshStatistics = new System.Windows.Forms.Button();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            txtEstimatedSize = new System.Windows.Forms.TextBox();
            txtItemCount = new System.Windows.Forms.TextBox();
            txtRemoves = new System.Windows.Forms.TextBox();
            txtPuts = new System.Windows.Forms.TextBox();
            txtHitRatio = new System.Windows.Forms.TextBox();
            txtMisses = new System.Windows.Forms.TextBox();
            txtHits = new System.Windows.Forms.TextBox();
            tabControl2 = new System.Windows.Forms.TabControl();
            tabPageTableStats = new System.Windows.Forms.TabPage();
            dataGridViewTableStats = new System.Windows.Forms.DataGridView();
            tabPageItemStats = new System.Windows.Forms.TabPage();
            dataGridViewItemStats = new System.Windows.Forms.DataGridView();
            toolStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageCacheManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tabPageCacheStatistics.SuspendLayout();
            panel1.SuspendLayout();
            tabControl2.SuspendLayout();
            tabPageTableStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableStats).BeginInit();
            tabPageItemStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridViewItemStats).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton加载缓存, toolStripButton刷新缓存, toolStripSeparator1, toolStripLabel1, cmbUser, toolStripSeparator2, toolStripButton1 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton加载缓存
            // 
            toolStripButton加载缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton加载缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton加载缓存.Name = "toolStripButton加载缓存";
            toolStripButton加载缓存.Size = new System.Drawing.Size(60, 22);
            toolStripButton加载缓存.Text = "加载缓存";
            toolStripButton加载缓存.Click += toolStripButton加载缓存_Click;
            // 
            // toolStripButton刷新缓存
            // 
            toolStripButton刷新缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton刷新缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton刷新缓存.Name = "toolStripButton刷新缓存";
            toolStripButton刷新缓存.Size = new System.Drawing.Size(60, 22);
            toolStripButton刷新缓存.Text = "刷新缓存";
            toolStripButton刷新缓存.Click += toolStripButton刷新缓存_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new System.Drawing.Size(59, 22);
            toolStripLabel1.Text = "推送用户:";
            // 
            // cmbUser
            // 
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new System.Drawing.Size(140, 25);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new System.Drawing.Size(84, 22);
            toolStripButton1.Text = "推送缓存数据";
            toolStripButton1.Click += 推送缓存数据ToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageCacheManagement);
            tabControl1.Controls.Add(tabPageCacheStatistics);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 25);
            tabControl1.Margin = new System.Windows.Forms.Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(933, 683);
            tabControl1.TabIndex = 1;
            // 
            // tabPageCacheManagement
            // 
            tabPageCacheManagement.Controls.Add(splitContainer1);
            tabPageCacheManagement.Location = new System.Drawing.Point(4, 26);
            tabPageCacheManagement.Margin = new System.Windows.Forms.Padding(4);
            tabPageCacheManagement.Name = "tabPageCacheManagement";
            tabPageCacheManagement.Padding = new System.Windows.Forms.Padding(4);
            tabPageCacheManagement.Size = new System.Drawing.Size(925, 653);
            tabPageCacheManagement.TabIndex = 0;
            tabPageCacheManagement.Text = "缓存管理";
            tabPageCacheManagement.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(4, 4);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listBoxTableList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new System.Drawing.Size(917, 645);
            splitContainer1.SplitterDistance = 310;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 0;
            // 
            // listBoxTableList
            // 
            listBoxTableList.ContextMenuStrip = contextMenuStrip1;
            listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTableList.FormattingEnabled = true;
            listBoxTableList.ItemHeight = 17;
            listBoxTableList.Location = new System.Drawing.Point(0, 0);
            listBoxTableList.Margin = new System.Windows.Forms.Padding(4);
            listBoxTableList.Name = "listBoxTableList";
            listBoxTableList.Size = new System.Drawing.Size(310, 645);
            listBoxTableList.TabIndex = 0;
            listBoxTableList.SelectedIndexChanged += listBoxTableList_SelectedIndexChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 加载缓存数据ToolStripMenuItem, 推送缓存数据ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 加载缓存数据ToolStripMenuItem
            // 
            加载缓存数据ToolStripMenuItem.Name = "加载缓存数据ToolStripMenuItem";
            加载缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            加载缓存数据ToolStripMenuItem.Text = "加载缓存数据";
            加载缓存数据ToolStripMenuItem.Click += 加载缓存数据ToolStripMenuItem_Click;
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            推送缓存数据ToolStripMenuItem.Click += 推送缓存数据ToolStripMenuItem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 23;
            dataGridView1.Size = new System.Drawing.Size(602, 645);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            // 
            // tabPageCacheStatistics
            // 
            tabPageCacheStatistics.Controls.Add(tabControl2);
            tabPageCacheStatistics.Controls.Add(panel1);
            tabPageCacheStatistics.Location = new System.Drawing.Point(4, 26);
            tabPageCacheStatistics.Margin = new System.Windows.Forms.Padding(4);
            tabPageCacheStatistics.Name = "tabPageCacheStatistics";
            tabPageCacheStatistics.Padding = new System.Windows.Forms.Padding(4);
            tabPageCacheStatistics.Size = new System.Drawing.Size(925, 653);
            tabPageCacheStatistics.TabIndex = 1;
            tabPageCacheStatistics.Text = "缓存统计";
            tabPageCacheStatistics.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnResetStatistics);
            panel1.Controls.Add(btnRefreshStatistics);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(txtEstimatedSize);
            panel1.Controls.Add(txtItemCount);
            panel1.Controls.Add(txtRemoves);
            panel1.Controls.Add(txtPuts);
            panel1.Controls.Add(txtHitRatio);
            panel1.Controls.Add(txtMisses);
            panel1.Controls.Add(txtHits);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(4, 4);
            panel1.Margin = new System.Windows.Forms.Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(917, 162);
            panel1.TabIndex = 1;
            // 
            // btnResetStatistics
            // 
            btnResetStatistics.Location = new System.Drawing.Point(805, 113);
            btnResetStatistics.Margin = new System.Windows.Forms.Padding(4);
            btnResetStatistics.Name = "btnResetStatistics";
            btnResetStatistics.Size = new System.Drawing.Size(88, 33);
            btnResetStatistics.TabIndex = 16;
            btnResetStatistics.Text = "重置统计";
            btnResetStatistics.UseVisualStyleBackColor = true;
            btnResetStatistics.Click += btnResetStatistics_Click;
            // 
            // btnRefreshStatistics
            // 
            btnRefreshStatistics.Location = new System.Drawing.Point(805, 72);
            btnRefreshStatistics.Margin = new System.Windows.Forms.Padding(4);
            btnRefreshStatistics.Name = "btnRefreshStatistics";
            btnRefreshStatistics.Size = new System.Drawing.Size(88, 33);
            btnRefreshStatistics.TabIndex = 15;
            btnRefreshStatistics.Text = "刷新统计";
            btnRefreshStatistics.UseVisualStyleBackColor = true;
            btnRefreshStatistics.Click += btnRefreshStatistics_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(525, 120);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(83, 17);
            label8.TabIndex = 14;
            label8.Text = "缓存估计大小:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(525, 79);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(71, 17);
            label7.TabIndex = 13;
            label7.Text = "缓存项总数:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(525, 38);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(83, 17);
            label6.TabIndex = 12;
            label6.Text = "缓存删除次数:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(257, 120);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(83, 17);
            label5.TabIndex = 11;
            label5.Text = "缓存写入次数:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(257, 79);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(71, 17);
            label4.TabIndex = 10;
            label4.Text = "缓存命中率:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(257, 38);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(95, 17);
            label3.TabIndex = 9;
            label3.Text = "缓存未命中次数:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(18, 79);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 17);
            label2.TabIndex = 8;
            label2.Text = "统计信息";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(18, 38);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 17);
            label1.TabIndex = 7;
            label1.Text = "缓存命中次数:";
            // 
            // txtEstimatedSize
            // 
            txtEstimatedSize.Location = new System.Drawing.Point(622, 116);
            txtEstimatedSize.Margin = new System.Windows.Forms.Padding(4);
            txtEstimatedSize.Name = "txtEstimatedSize";
            txtEstimatedSize.ReadOnly = true;
            txtEstimatedSize.Size = new System.Drawing.Size(139, 23);
            txtEstimatedSize.TabIndex = 6;
            // 
            // txtItemCount
            // 
            txtItemCount.Location = new System.Drawing.Point(622, 75);
            txtItemCount.Margin = new System.Windows.Forms.Padding(4);
            txtItemCount.Name = "txtItemCount";
            txtItemCount.ReadOnly = true;
            txtItemCount.Size = new System.Drawing.Size(139, 23);
            txtItemCount.TabIndex = 5;
            // 
            // txtRemoves
            // 
            txtRemoves.Location = new System.Drawing.Point(622, 34);
            txtRemoves.Margin = new System.Windows.Forms.Padding(4);
            txtRemoves.Name = "txtRemoves";
            txtRemoves.ReadOnly = true;
            txtRemoves.Size = new System.Drawing.Size(139, 23);
            txtRemoves.TabIndex = 4;
            // 
            // txtPuts
            // 
            txtPuts.Location = new System.Drawing.Point(354, 116);
            txtPuts.Margin = new System.Windows.Forms.Padding(4);
            txtPuts.Name = "txtPuts";
            txtPuts.ReadOnly = true;
            txtPuts.Size = new System.Drawing.Size(139, 23);
            txtPuts.TabIndex = 3;
            // 
            // txtHitRatio
            // 
            txtHitRatio.Location = new System.Drawing.Point(354, 75);
            txtHitRatio.Margin = new System.Windows.Forms.Padding(4);
            txtHitRatio.Name = "txtHitRatio";
            txtHitRatio.ReadOnly = true;
            txtHitRatio.Size = new System.Drawing.Size(139, 23);
            txtHitRatio.TabIndex = 2;
            // 
            // txtMisses
            // 
            txtMisses.Location = new System.Drawing.Point(354, 34);
            txtMisses.Margin = new System.Windows.Forms.Padding(4);
            txtMisses.Name = "txtMisses";
            txtMisses.ReadOnly = true;
            txtMisses.Size = new System.Drawing.Size(139, 23);
            txtMisses.TabIndex = 1;
            // 
            // txtHits
            // 
            txtHits.Location = new System.Drawing.Point(114, 34);
            txtHits.Margin = new System.Windows.Forms.Padding(4);
            txtHits.Name = "txtHits";
            txtHits.ReadOnly = true;
            txtHits.Size = new System.Drawing.Size(139, 23);
            txtHits.TabIndex = 0;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(tabPageTableStats);
            tabControl2.Controls.Add(tabPageItemStats);
            tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl2.Location = new System.Drawing.Point(4, 166);
            tabControl2.Margin = new System.Windows.Forms.Padding(4);
            tabControl2.Name = "tabControl2";
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new System.Drawing.Size(917, 483);
            tabControl2.TabIndex = 2;
            // 
            // tabPageTableStats
            // 
            tabPageTableStats.Controls.Add(dataGridViewTableStats);
            tabPageTableStats.Location = new System.Drawing.Point(4, 26);
            tabPageTableStats.Margin = new System.Windows.Forms.Padding(4);
            tabPageTableStats.Name = "tabPageTableStats";
            tabPageTableStats.Padding = new System.Windows.Forms.Padding(4);
            tabPageTableStats.Size = new System.Drawing.Size(909, 445);
            tabPageTableStats.TabIndex = 0;
            tabPageTableStats.Text = "按表统计";
            tabPageTableStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTableStats
            // 
            dataGridViewTableStats.AllowUserToAddRows = false;
            dataGridViewTableStats.AllowUserToDeleteRows = false;
            dataGridViewTableStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewTableStats.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewTableStats.Location = new System.Drawing.Point(4, 4);
            dataGridViewTableStats.Margin = new System.Windows.Forms.Padding(4);
            dataGridViewTableStats.Name = "dataGridViewTableStats";
            dataGridViewTableStats.ReadOnly = true;
            dataGridViewTableStats.RowTemplate.Height = 23;
            dataGridViewTableStats.Size = new System.Drawing.Size(901, 437);
            dataGridViewTableStats.TabIndex = 0;
            // 
            // tabPageItemStats
            // 
            tabPageItemStats.Controls.Add(dataGridViewItemStats);
            tabPageItemStats.Location = new System.Drawing.Point(4, 26);
            tabPageItemStats.Margin = new System.Windows.Forms.Padding(4);
            tabPageItemStats.Name = "tabPageItemStats";
            tabPageItemStats.Padding = new System.Windows.Forms.Padding(4);
            tabPageItemStats.Size = new System.Drawing.Size(909, 453);
            tabPageItemStats.TabIndex = 1;
            tabPageItemStats.Text = "缓存项统计";
            tabPageItemStats.UseVisualStyleBackColor = true;
            // 
            // dataGridViewItemStats
            // 
            dataGridViewItemStats.AllowUserToAddRows = false;
            dataGridViewItemStats.AllowUserToDeleteRows = false;
            dataGridViewItemStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewItemStats.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewItemStats.Location = new System.Drawing.Point(4, 4);
            dataGridViewItemStats.Margin = new System.Windows.Forms.Padding(4);
            dataGridViewItemStats.Name = "dataGridViewItemStats";
            dataGridViewItemStats.ReadOnly = true;
            dataGridViewItemStats.RowTemplate.Height = 23;
            dataGridViewItemStats.Size = new System.Drawing.Size(901, 445);
            dataGridViewItemStats.TabIndex = 0;
            // 
            // CacheManagementControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "CacheManagementControl";
            Size = new System.Drawing.Size(933, 708);
            Load += CacheManagementControl_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageCacheManagement.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tabPageCacheStatistics.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabControl2.ResumeLayout(false);
            tabPageTableStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewTableStats).EndInit();
            tabPageItemStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridViewItemStats).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton加载缓存;
        private System.Windows.Forms.ToolStripButton toolStripButton刷新缓存;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBoxTableList;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 加载缓存数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCacheManagement;
        private System.Windows.Forms.TabPage tabPageCacheStatistics;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEstimatedSize;
        private System.Windows.Forms.TextBox txtItemCount;
        private System.Windows.Forms.TextBox txtRemoves;
        private System.Windows.Forms.TextBox txtPuts;
        private System.Windows.Forms.TextBox txtHitRatio;
        private System.Windows.Forms.TextBox txtMisses;
        private System.Windows.Forms.TextBox txtHits;
        private System.Windows.Forms.Button btnResetStatistics;
        private System.Windows.Forms.Button btnRefreshStatistics;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPageTableStats;
        private System.Windows.Forms.DataGridView dataGridViewTableStats;
        private System.Windows.Forms.TabPage tabPageItemStats;
        private System.Windows.Forms.DataGridView dataGridViewItemStats;
    }
}