namespace RUINORERP.Server.Controls
{
    partial class SequenceManagementControl
    {
        /// <summary>
        /// 必需的设计器变量
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false</param>
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
        /// 使用代码编辑器修改此方法的内容
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnRefresh = new System.Windows.Forms.ToolStripButton();
            btnResetSelected = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            txtBusinessTypeFilter = new System.Windows.Forms.ToolStripTextBox();
            btnFilterByType = new System.Windows.Forms.ToolStripButton();
            btnClearFilter = new System.Windows.Forms.ToolStripButton();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            dgvSequences = new System.Windows.Forms.DataGridView();
            colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSequenceKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCurrentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colResetType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colFormatMask = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBusinessType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colLastUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            toolStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSequences).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnRefresh, btnResetSelected, toolStripSeparator1, toolStripLabel1, txtBusinessTypeFilter, btnFilterByType, btnClearFilter });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnRefresh
            // 
            btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(36, 22);
            btnRefresh.Text = "刷新";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnResetSelected
            // 
            btnResetSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnResetSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnResetSelected.Name = "btnResetSelected";
            btnResetSelected.Size = new System.Drawing.Size(60, 22);
            btnResetSelected.Text = "重置选中";
            btnResetSelected.Click += btnResetSelected_Click;
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
            toolStripLabel1.Text = "业务类型:";
            // 
            // txtBusinessTypeFilter
            // 
            txtBusinessTypeFilter.Name = "txtBusinessTypeFilter";
            txtBusinessTypeFilter.Size = new System.Drawing.Size(116, 25);
            // 
            // btnFilterByType
            // 
            btnFilterByType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnFilterByType.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnFilterByType.Name = "btnFilterByType";
            btnFilterByType.Size = new System.Drawing.Size(36, 22);
            btnFilterByType.Text = "筛选";
            btnFilterByType.Click += btnFilterByType_Click;
            // 
            // btnClearFilter
            // 
            btnClearFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnClearFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnClearFilter.Name = "btnClearFilter";
            btnClearFilter.Size = new System.Drawing.Size(36, 22);
            btnClearFilter.Text = "清空";
            btnClearFilter.Click += btnClearFilter_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 25);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(933, 683);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgvSequences);
            tabPage1.Location = new System.Drawing.Point(4, 26);
            tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPage1.Size = new System.Drawing.Size(925, 653);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "序号管理";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dgvSequences
            // 
            dgvSequences.AllowUserToAddRows = false;
            dgvSequences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSequences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { colId, colSequenceKey, colCurrentValue, colResetType, colFormatMask, colBusinessType, colDescription, colCreatedAt, colLastUpdated });
            dgvSequences.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSequences.Location = new System.Drawing.Point(4, 4);
            dgvSequences.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            dgvSequences.Name = "dgvSequences";
            dgvSequences.RowTemplate.Height = 23;
            dgvSequences.Size = new System.Drawing.Size(917, 645);
            dgvSequences.TabIndex = 0;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 60;
            // 
            // colSequenceKey
            // 
            colSequenceKey.DataPropertyName = "SequenceKey";
            colSequenceKey.HeaderText = "序列键";
            colSequenceKey.Name = "colSequenceKey";
            colSequenceKey.ReadOnly = true;
            colSequenceKey.Width = 200;
            // 
            // colCurrentValue
            // 
            colCurrentValue.DataPropertyName = "CurrentValue";
            colCurrentValue.HeaderText = "当前值";
            colCurrentValue.Name = "colCurrentValue";
            colCurrentValue.ReadOnly = true;
            colCurrentValue.Width = 80;
            // 
            // colResetType
            // 
            colResetType.DataPropertyName = "ResetType";
            colResetType.HeaderText = "重置类型";
            colResetType.Name = "colResetType";
            colResetType.ReadOnly = true;
            colResetType.Width = 80;
            // 
            // colFormatMask
            // 
            colFormatMask.DataPropertyName = "FormatMask";
            colFormatMask.HeaderText = "格式掩码";
            colFormatMask.Name = "colFormatMask";
            colFormatMask.ReadOnly = true;
            colFormatMask.Width = 80;
            // 
            // colBusinessType
            // 
            colBusinessType.DataPropertyName = "BusinessType";
            colBusinessType.HeaderText = "业务类型";
            colBusinessType.Name = "colBusinessType";
            colBusinessType.ReadOnly = true;
            // 
            // colDescription
            // 
            colDescription.DataPropertyName = "Description";
            colDescription.HeaderText = "描述";
            colDescription.Name = "colDescription";
            colDescription.ReadOnly = true;
            colDescription.Width = 120;
            // 
            // colCreatedAt
            // 
            colCreatedAt.DataPropertyName = "CreatedAt";
            colCreatedAt.HeaderText = "创建时间";
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.ReadOnly = true;
            colCreatedAt.Width = 120;
            // 
            // colLastUpdated
            // 
            colLastUpdated.DataPropertyName = "LastUpdated";
            colLastUpdated.HeaderText = "最后更新";
            colLastUpdated.Name = "colLastUpdated";
            colLastUpdated.ReadOnly = true;
            colLastUpdated.Width = 120;
            // 
            // SequenceManagementControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "SequenceManagementControl";
            Size = new System.Drawing.Size(933, 708);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSequences).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripButton btnResetSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtBusinessTypeFilter;
        private System.Windows.Forms.ToolStripButton btnFilterByType;
        private System.Windows.Forms.ToolStripButton btnClearFilter;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvSequences;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSequenceKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFormatMask;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBusinessType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastUpdated;
    }
}