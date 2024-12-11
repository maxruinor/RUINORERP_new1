namespace RUINORERP.UI.SysConfig
{
    partial class UCCacheManage
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.listBoxTableList = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.请求缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonSplitContainer2 = new Krypton.Toolkit.KryptonSplitContainer();
            this.dataGridView1 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.btnRefreshCache = new Krypton.Toolkit.KryptonButton();
            this.chkALL = new Krypton.Toolkit.KryptonCheckBox();
            this.btnSave = new Krypton.Toolkit.KryptonButton();
            this.bindingSourceList = new System.Windows.Forms.BindingSource(this.components);
            this.清空选中缓存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).BeginInit();
            this.kryptonSplitContainer2.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).BeginInit();
            this.kryptonSplitContainer2.Panel2.SuspendLayout();
            this.kryptonSplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.listBoxTableList);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonSplitContainer2);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(770, 615);
            this.kryptonSplitContainer1.SplitterDistance = 179;
            this.kryptonSplitContainer1.TabIndex = 1;
            // 
            // listBoxTableList
            // 
            this.listBoxTableList.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxTableList.FormattingEnabled = true;
            this.listBoxTableList.ItemHeight = 12;
            this.listBoxTableList.Location = new System.Drawing.Point(0, 0);
            this.listBoxTableList.Name = "listBoxTableList";
            this.listBoxTableList.Size = new System.Drawing.Size(179, 615);
            this.listBoxTableList.TabIndex = 0;
            this.listBoxTableList.SelectedIndexChanged += new System.EventHandler(this.listBoxTableList_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.请求缓存ToolStripMenuItem,
            this.清空选中缓存ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // 请求缓存ToolStripMenuItem
            // 
            this.请求缓存ToolStripMenuItem.Name = "请求缓存ToolStripMenuItem";
            this.请求缓存ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.请求缓存ToolStripMenuItem.Text = "请求缓存";
            this.请求缓存ToolStripMenuItem.Click += new System.EventHandler(this.请求缓存ToolStripMenuItem_Click);
            // 
            // kryptonSplitContainer2
            // 
            this.kryptonSplitContainer2.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer2.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer2.Name = "kryptonSplitContainer2";
            this.kryptonSplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer2.Panel1
            // 
            this.kryptonSplitContainer2.Panel1.Controls.Add(this.dataGridView1);
            // 
            // kryptonSplitContainer2.Panel2
            // 
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.btnRefreshCache);
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.chkALL);
            this.kryptonSplitContainer2.Panel2.Controls.Add(this.btnSave);
            this.kryptonSplitContainer2.Panel2.StateNormal.Color1 = System.Drawing.SystemColors.ActiveCaption;
            this.kryptonSplitContainer2.Size = new System.Drawing.Size(586, 615);
            this.kryptonSplitContainer2.SplitterDistance = 493;
            this.kryptonSplitContainer2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Beige;
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.CustomRowNo = false;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.FieldNameList = null;
            this.dataGridView1.IsShowSumRow = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(586, 493);
            this.dataGridView1.SumColumns = null;
            this.dataGridView1.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView1.SumRowCellFormat = "N2";
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.UseCustomColumnDisplay = true;
            this.dataGridView1.UseSelectedColumn = false;
            this.dataGridView1.Use是否使用内置右键功能 = true;
            this.dataGridView1.XmlFileName = "";
            // 
            // btnRefreshCache
            // 
            this.btnRefreshCache.Location = new System.Drawing.Point(457, 7);
            this.btnRefreshCache.Name = "btnRefreshCache";
            this.btnRefreshCache.Size = new System.Drawing.Size(99, 35);
            this.btnRefreshCache.TabIndex = 2;
            this.btnRefreshCache.Values.Text = "刷新缓存";
            this.btnRefreshCache.Click += new System.EventHandler(this.btnRefreshCache_Click);
            // 
            // chkALL
            // 
            this.chkALL.Location = new System.Drawing.Point(135, 22);
            this.chkALL.Name = "chkALL";
            this.chkALL.Size = new System.Drawing.Size(49, 20);
            this.chkALL.TabIndex = 1;
            this.chkALL.Values.Text = "全选";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(278, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 35);
            this.btnSave.TabIndex = 0;
            this.btnSave.Values.Text = "保存";
            // 
            // 清空选中缓存ToolStripMenuItem
            // 
            this.清空选中缓存ToolStripMenuItem.Name = "清空选中缓存ToolStripMenuItem";
            this.清空选中缓存ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.清空选中缓存ToolStripMenuItem.Text = "清空选中缓存";
            this.清空选中缓存ToolStripMenuItem.Click += new System.EventHandler(this.清空选中缓存ToolStripMenuItem_Click);
            // 
            // UCCacheManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCCacheManage";
            this.Size = new System.Drawing.Size(770, 615);
            this.Load += new System.EventHandler(this.UCCacheManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel1)).EndInit();
            this.kryptonSplitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2.Panel2)).EndInit();
            this.kryptonSplitContainer2.Panel2.ResumeLayout(false);
            this.kryptonSplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer2)).EndInit();
            this.kryptonSplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer2;
        internal UControls.NewSumDataGridView dataGridView1;
        private Krypton.Toolkit.KryptonCheckBox chkALL;
        private Krypton.Toolkit.KryptonButton btnSave;
        internal System.Windows.Forms.BindingSource bindingSourceList;
        private System.Windows.Forms.ListBox listBoxTableList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 请求缓存ToolStripMenuItem;
        private Krypton.Toolkit.KryptonButton btnRefreshCache;
        private System.Windows.Forms.ToolStripMenuItem 清空选中缓存ToolStripMenuItem;
    }
}
