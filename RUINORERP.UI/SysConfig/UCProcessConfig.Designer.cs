namespace RUINORERP.UI.SysConfig
{
    partial class UCProcessConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCWorkCenterConfig));
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.TreeView1 = new RUINOR.WinFormsUI.TreeViewThreeState.ThreeStateTreeView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonHeaderGroup2 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.kryptonCheckedListBox数据概览 = new Krypton.Toolkit.KryptonCheckedListBox();
            this.kryptonHeaderGroup1 = new Krypton.Toolkit.KryptonHeaderGroup();
            this.kryptonCheckedListBox待办事项 = new Krypton.Toolkit.KryptonCheckedListBox();
            this.BaseToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.bindingSourceList = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip4InitData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemInitBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInitField = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2.Panel)).BeginInit();
            this.kryptonHeaderGroup2.Panel.SuspendLayout();
            this.kryptonHeaderGroup2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).BeginInit();
            this.kryptonHeaderGroup1.Panel.SuspendLayout();
            this.kryptonHeaderGroup1.SuspendLayout();
            this.BaseToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip4InitData.SuspendLayout();
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
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.TreeView1);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.BaseToolStrip);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(799, 606);
            this.kryptonSplitContainer1.SplitterDistance = 260;
            this.kryptonSplitContainer1.TabIndex = 1;
            // 
            // TreeView1
            // 
            this.TreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeView1.Location = new System.Drawing.Point(0, 0);
            this.TreeView1.Name = "TreeView1";
            this.TreeView1.Size = new System.Drawing.Size(260, 606);
            this.TreeView1.SummaryDescription = "1，添加和级联状态变化到相关的节点，即子节点和或父节点，以及有选择的能力 ";
            this.TreeView1.TabIndex = 1;
            this.TreeView1.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeView1_DrawNode);
            this.TreeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView1_AfterSelect);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 168F));
            this.tableLayoutPanel1.Controls.Add(this.kryptonHeaderGroup2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.kryptonHeaderGroup1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(534, 581);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // kryptonHeaderGroup2
            // 
            this.kryptonHeaderGroup2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup2.Location = new System.Drawing.Point(186, 3);
            this.kryptonHeaderGroup2.Name = "kryptonHeaderGroup2";
            // 
            // kryptonHeaderGroup2.Panel
            // 
            this.kryptonHeaderGroup2.Panel.Controls.Add(this.kryptonCheckedListBox数据概览);
            this.kryptonHeaderGroup2.Size = new System.Drawing.Size(177, 575);
            this.kryptonHeaderGroup2.TabIndex = 1;
            this.kryptonHeaderGroup2.ValuesPrimary.Heading = "数据概览";
            // 
            // kryptonCheckedListBox数据概览
            // 
            this.kryptonCheckedListBox数据概览.ContextMenuStrip = this.contextMenuStrip1;
            this.kryptonCheckedListBox数据概览.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonCheckedListBox数据概览.Location = new System.Drawing.Point(0, 0);
            this.kryptonCheckedListBox数据概览.Name = "kryptonCheckedListBox数据概览";
            this.kryptonCheckedListBox数据概览.Size = new System.Drawing.Size(175, 522);
            this.kryptonCheckedListBox数据概览.TabIndex = 1;
            // 
            // kryptonHeaderGroup1
            // 
            this.kryptonHeaderGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonHeaderGroup1.Location = new System.Drawing.Point(3, 3);
            this.kryptonHeaderGroup1.Name = "kryptonHeaderGroup1";
            // 
            // kryptonHeaderGroup1.Panel
            // 
            this.kryptonHeaderGroup1.Panel.Controls.Add(this.kryptonCheckedListBox待办事项);
            this.kryptonHeaderGroup1.Size = new System.Drawing.Size(177, 575);
            this.kryptonHeaderGroup1.TabIndex = 0;
            this.kryptonHeaderGroup1.ValuesPrimary.Heading = "待办事项";
            // 
            // kryptonCheckedListBox待办事项
            // 
            this.kryptonCheckedListBox待办事项.ContextMenuStrip = this.contextMenuStrip1;
            this.kryptonCheckedListBox待办事项.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonCheckedListBox待办事项.Location = new System.Drawing.Point(0, 0);
            this.kryptonCheckedListBox待办事项.Name = "kryptonCheckedListBox待办事项";
            this.kryptonCheckedListBox待办事项.Size = new System.Drawing.Size(175, 522);
            this.kryptonCheckedListBox待办事项.TabIndex = 0;
            // 
            // BaseToolStrip
            // 
            this.BaseToolStrip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.BaseToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButtonSave,
            this.toolStripSeparator2,
            this.toolStripSeparator1,
            this.toolStripButton12});
            this.BaseToolStrip.Location = new System.Drawing.Point(0, 0);
            this.BaseToolStrip.Name = "BaseToolStrip";
            this.BaseToolStrip.Size = new System.Drawing.Size(534, 25);
            this.BaseToolStrip.TabIndex = 2;
            this.BaseToolStrip.Text = "toolStrip1";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton3.Text = "修改";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonSave.Text = "保存";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(53, 22);
            this.toolStripButton12.Text = "关闭";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAll,
            this.selectNoAll});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            // 
            // selectAll
            // 
            this.selectAll.Name = "selectAll";
            this.selectAll.Size = new System.Drawing.Size(180, 22);
            this.selectAll.Text = "全选";
            this.selectAll.Click += new System.EventHandler(this.selectAll_Click);
            // 
            // selectNoAll
            // 
            this.selectNoAll.Name = "selectNoAll";
            this.selectNoAll.Size = new System.Drawing.Size(180, 22);
            this.selectNoAll.Text = "全不选";
            this.selectNoAll.Click += new System.EventHandler(this.selectNoAll_Click);
            // 
            // contextMenuStrip4InitData
            // 
            this.contextMenuStrip4InitData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip4InitData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemInitBtn,
            this.toolStripMenuItemInitField});
            this.contextMenuStrip4InitData.Name = "contextMenuStrip4InitData";
            this.contextMenuStrip4InitData.Size = new System.Drawing.Size(127, 48);
            // 
            // toolStripMenuItemInitBtn
            // 
            this.toolStripMenuItemInitBtn.Name = "toolStripMenuItemInitBtn";
            this.toolStripMenuItemInitBtn.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItemInitBtn.Text = "添加按钮";
            // 
            // toolStripMenuItemInitField
            // 
            this.toolStripMenuItemInitField.Name = "toolStripMenuItemInitField";
            this.toolStripMenuItemInitField.Size = new System.Drawing.Size(126, 22);
            this.toolStripMenuItemInitField.Text = "添加字段";
            // 
            // UCWorkCenterConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Name = "UCWorkCenterConfig";
            this.Size = new System.Drawing.Size(799, 606);
            this.Load += new System.EventHandler(this.UCWorkCenterConfig_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            this.kryptonSplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2.Panel)).EndInit();
            this.kryptonHeaderGroup2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup2)).EndInit();
            this.kryptonHeaderGroup2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1.Panel)).EndInit();
            this.kryptonHeaderGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonHeaderGroup1)).EndInit();
            this.kryptonHeaderGroup1.ResumeLayout(false);
            this.BaseToolStrip.ResumeLayout(false);
            this.BaseToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceList)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip4InitData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        internal System.Windows.Forms.BindingSource bindingSourceList;
        private RUINOR.WinFormsUI.TreeViewThreeState.ThreeStateTreeView TreeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAll;
        private System.Windows.Forms.ToolStripMenuItem selectNoAll;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip4InitData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInitBtn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemInitField;
        internal System.Windows.Forms.ToolStrip BaseToolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup1;
        private Krypton.Toolkit.KryptonHeaderGroup kryptonHeaderGroup2;
        private Krypton.Toolkit.KryptonCheckedListBox kryptonCheckedListBox数据概览;
        private Krypton.Toolkit.KryptonCheckedListBox kryptonCheckedListBox待办事项;
    }
}
