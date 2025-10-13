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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CacheManagementControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton加载缓存 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton刷新缓存 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbUser = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBoxTableList = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.加载缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton加载缓存,
            this.toolStripButton刷新缓存,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.cmbUser,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton加载缓存
            // 
            this.toolStripButton加载缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton加载缓存.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton加载缓存.Image")));
            this.toolStripButton加载缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton加载缓存.Name = "toolStripButton加载缓存";
            this.toolStripButton加载缓存.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton加载缓存.Text = "加载缓存";
            this.toolStripButton加载缓存.Click += new System.EventHandler(this.toolStripButton加载缓存_Click);
            // 
            // toolStripButton刷新缓存
            // 
            this.toolStripButton刷新缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton刷新缓存.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton刷新缓存.Image")));
            this.toolStripButton刷新缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton刷新缓存.Name = "toolStripButton刷新缓存";
            this.toolStripButton刷新缓存.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton刷新缓存.Text = "刷新缓存";
            this.toolStripButton刷新缓存.Click += new System.EventHandler(this.toolStripButton刷新缓存_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel1.Text = "推送用户:";
            // 
            // cmbUser
            // 
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton1.Text = "推送缓存数据";
            this.toolStripButton1.Click += new System.EventHandler(this.推送缓存数据ToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxTableList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 475);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 1;
            // 
            // listBoxTableList
            // 
            this.listBoxTableList.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTableList.FormattingEnabled = true;
            this.listBoxTableList.ItemHeight = 12;
            this.listBoxTableList.Location = new System.Drawing.Point(0, 0);
            this.listBoxTableList.Name = "listBoxTableList";
            this.listBoxTableList.Size = new System.Drawing.Size(266, 475);
            this.listBoxTableList.TabIndex = 0;
            this.listBoxTableList.SelectedIndexChanged += new System.EventHandler(this.listBoxTableList_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载缓存数据ToolStripMenuItem,
            this.推送缓存数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 加载缓存数据ToolStripMenuItem
            // 
            this.加载缓存数据ToolStripMenuItem.Name = "加载缓存数据ToolStripMenuItem";
            this.加载缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.加载缓存数据ToolStripMenuItem.Text = "加载缓存数据";
            this.加载缓存数据ToolStripMenuItem.Click += new System.EventHandler(this.加载缓存数据ToolStripMenuItem_Click);
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            this.推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            this.推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            this.推送缓存数据ToolStripMenuItem.Click += new System.EventHandler(this.推送缓存数据ToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(530, 475);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridView1_CellPainting);
            // 
            // CacheManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CacheManagementControl";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.CacheManagementControl_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}