namespace RUINORERP.Server
{
    partial class frmMemoryDataViewer
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMemoryDataViewer));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeViewDataType = new System.Windows.Forms.TreeView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            加载数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            contextMenuStripGrid = new System.Windows.Forms.ContextMenuStrip(components);
            发送提醒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            toolStrip1.SuspendLayout();
            contextMenuStripGrid.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeViewDataType);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new System.Drawing.Size(800, 425);
            splitContainer1.SplitterDistance = 169;
            splitContainer1.TabIndex = 0;
            // 
            // treeViewDataType
            // 
            treeViewDataType.ContextMenuStrip = contextMenuStrip1;
            treeViewDataType.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewDataType.Location = new System.Drawing.Point(0, 0);
            treeViewDataType.Name = "treeViewDataType";
            treeViewDataType.Size = new System.Drawing.Size(169, 425);
            treeViewDataType.TabIndex = 0;
            treeViewDataType.NodeMouseDoubleClick += treeViewDataType_NodeMouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 加载数据ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 加载数据ToolStripMenuItem
            // 
            加载数据ToolStripMenuItem.Name = "加载数据ToolStripMenuItem";
            加载数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            加载数据ToolStripMenuItem.Text = "加载数据";
            加载数据ToolStripMenuItem.Click += 加载数据ToolStripMenuItem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStripGrid;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(627, 425);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnRefresh });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(800, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnRefresh
            // 
            tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnRefresh.Image = (System.Drawing.Image)resources.GetObject("tsbtnRefresh.Image");
            tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRefresh.Name = "tsbtnRefresh";
            tsbtnRefresh.Size = new System.Drawing.Size(36, 22);
            tsbtnRefresh.Text = "刷新";
            tsbtnRefresh.Click += tsbtnRefresh_Click;
            // 
            // contextMenuStripGrid
            // 
            contextMenuStripGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 发送提醒ToolStripMenuItem });
            contextMenuStripGrid.Name = "contextMenuStripGrid";
            contextMenuStripGrid.Size = new System.Drawing.Size(181, 48);
            // 
            // 发送提醒ToolStripMenuItem
            // 
            发送提醒ToolStripMenuItem.Name = "发送提醒ToolStripMenuItem";
            发送提醒ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            发送提醒ToolStripMenuItem.Text = "发送提醒";
            发送提醒ToolStripMenuItem.Click += 发送提醒ToolStripMenuItem_Click;
            // 
            // frmMemoryDataViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Name = "frmMemoryDataViewer";
            Text = "frmMemoryDataViewer";
            Load += frmMemoryDataViewer_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            contextMenuStripGrid.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewDataType;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnRefresh;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 加载数据ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripGrid;
        private System.Windows.Forms.ToolStripMenuItem 发送提醒ToolStripMenuItem;
    }
}