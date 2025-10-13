namespace RUINORERP.Server.Controls
{
    partial class BlacklistManagementControl
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
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.groupBoxBlacklistEntries = new System.Windows.Forms.GroupBox();
            this.dataGridViewBlacklist = new System.Windows.Forms.DataGridView();
            this.toolStripStatusLabelBlacklistCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBoxOperations = new System.Windows.Forms.GroupBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnAddIP = new System.Windows.Forms.Button();
            this.textBoxReason = new System.Windows.Forms.TextBox();
            this.labelReason = new System.Windows.Forms.Label();
            this.textBoxIPAddress = new System.Windows.Forms.TextBox();
            this.labelIPAddress = new System.Windows.Forms.Label();
            this.iPAddressDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reasonDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.banTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.groupBoxBlacklistEntries.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlacklist)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBoxOperations.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.groupBoxBlacklistEntries);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainerMain.Panel2.Controls.Add(this.groupBoxOperations);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 600);
            this.splitContainerMain.SplitterDistance = 400;
            this.splitContainerMain.TabIndex = 0;
            // 
            // groupBoxBlacklistEntries
            // 
            this.groupBoxBlacklistEntries.Controls.Add(this.dataGridViewBlacklist);
            this.groupBoxBlacklistEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxBlacklistEntries.Location = new System.Drawing.Point(0, 0);
            this.groupBoxBlacklistEntries.Name = "groupBoxBlacklistEntries";
            this.groupBoxBlacklistEntries.Size = new System.Drawing.Size(800, 400);
            this.groupBoxBlacklistEntries.TabIndex = 0;
            this.groupBoxBlacklistEntries.TabStop = false;
            this.groupBoxBlacklistEntries.Text = "黑名单列表";
            // 
            // dataGridViewBlacklist
            // 
            this.dataGridViewBlacklist.AllowUserToAddRows = false;
            this.dataGridViewBlacklist.AllowUserToDeleteRows = false;
            this.dataGridViewBlacklist.AutoGenerateColumns = false;
            this.dataGridViewBlacklist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBlacklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBlacklist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iPAddressDataGridViewTextBoxColumn,
            this.reasonDataGridViewTextBoxColumn,
            this.banTimeDataGridViewTextBoxColumn});
            this.dataGridViewBlacklist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBlacklist.Location = new System.Drawing.Point(3, 19);
            this.dataGridViewBlacklist.Name = "dataGridViewBlacklist";
            this.dataGridViewBlacklist.ReadOnly = true;
            this.dataGridViewBlacklist.Size = new System.Drawing.Size(794, 378);
            this.dataGridViewBlacklist.TabIndex = 0;
            this.dataGridViewBlacklist.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBlacklist_CellDoubleClick);
            // 
            // toolStripStatusLabelBlacklistCount
            // 
            this.toolStripStatusLabelBlacklistCount.Name = "toolStripStatusLabelBlacklistCount";
            this.toolStripStatusLabelBlacklistCount.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabelBlacklistCount.Text = "黑名单总数: 0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelBlacklistCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 174);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // groupBoxOperations
            // 
            this.groupBoxOperations.Controls.Add(this.btnExport);
            this.groupBoxOperations.Controls.Add(this.btnImport);
            this.groupBoxOperations.Controls.Add(this.btnClearAll);
            this.groupBoxOperations.Controls.Add(this.btnRemoveSelected);
            this.groupBoxOperations.Controls.Add(this.btnRefresh);
            this.groupBoxOperations.Controls.Add(this.btnAddIP);
            this.groupBoxOperations.Controls.Add(this.textBoxReason);
            this.groupBoxOperations.Controls.Add(this.labelReason);
            this.groupBoxOperations.Controls.Add(this.textBoxIPAddress);
            this.groupBoxOperations.Controls.Add(this.labelIPAddress);
            this.groupBoxOperations.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxOperations.Location = new System.Drawing.Point(0, 0);
            this.groupBoxOperations.Name = "groupBoxOperations";
            this.groupBoxOperations.Size = new System.Drawing.Size(800, 171);
            this.groupBoxOperations.TabIndex = 0;
            this.groupBoxOperations.TabStop = false;
            this.groupBoxOperations.Text = "操作";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(500, 120);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 9;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(400, 120);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(300, 120);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 23);
            this.btnClearAll.TabIndex = 7;
            this.btnClearAll.Text = "清空所有";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Location = new System.Drawing.Point(200, 120);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveSelected.TabIndex = 6;
            this.btnRemoveSelected.Text = "移除选中";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(100, 120);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnAddIP
            // 
            this.btnAddIP.Location = new System.Drawing.Point(15, 120);
            this.btnAddIP.Name = "btnAddIP";
            this.btnAddIP.Size = new System.Drawing.Size(75, 23);
            this.btnAddIP.TabIndex = 4;
            this.btnAddIP.Text = "添加IP";
            this.btnAddIP.UseVisualStyleBackColor = true;
            this.btnAddIP.Click += new System.EventHandler(this.btnAddIP_Click);
            // 
            // textBoxReason
            // 
            this.textBoxReason.Location = new System.Drawing.Point(75, 60);
            this.textBoxReason.Multiline = true;
            this.textBoxReason.Name = "textBoxReason";
            this.textBoxReason.Size = new System.Drawing.Size(300, 40);
            this.textBoxReason.TabIndex = 3;
            // 
            // labelReason
            // 
            this.labelReason.AutoSize = true;
            this.labelReason.Location = new System.Drawing.Point(15, 63);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(35, 12);
            this.labelReason.TabIndex = 2;
            this.labelReason.Text = "原因:";
            // 
            // textBoxIPAddress
            // 
            this.textBoxIPAddress.Location = new System.Drawing.Point(75, 30);
            this.textBoxIPAddress.Name = "textBoxIPAddress";
            this.textBoxIPAddress.Size = new System.Drawing.Size(150, 21);
            this.textBoxIPAddress.TabIndex = 1;
            // 
            // labelIPAddress
            // 
            this.labelIPAddress.AutoSize = true;
            this.labelIPAddress.Location = new System.Drawing.Point(15, 33);
            this.labelIPAddress.Name = "labelIPAddress";
            this.labelIPAddress.Size = new System.Drawing.Size(23, 12);
            this.labelIPAddress.TabIndex = 0;
            this.labelIPAddress.Text = "IP:";
            // 
            // iPAddressDataGridViewTextBoxColumn
            // 
            this.iPAddressDataGridViewTextBoxColumn.DataPropertyName = "IPAddress";
            this.iPAddressDataGridViewTextBoxColumn.HeaderText = "IP地址";
            this.iPAddressDataGridViewTextBoxColumn.Name = "iPAddressDataGridViewTextBoxColumn";
            this.iPAddressDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // reasonDataGridViewTextBoxColumn
            // 
            this.reasonDataGridViewTextBoxColumn.DataPropertyName = "Reason";
            this.reasonDataGridViewTextBoxColumn.HeaderText = "原因";
            this.reasonDataGridViewTextBoxColumn.Name = "reasonDataGridViewTextBoxColumn";
            this.reasonDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // banTimeDataGridViewTextBoxColumn
            // 
            this.banTimeDataGridViewTextBoxColumn.DataPropertyName = "BanTime";
            this.banTimeDataGridViewTextBoxColumn.HeaderText = "封禁时间";
            this.banTimeDataGridViewTextBoxColumn.Name = "banTimeDataGridViewTextBoxColumn";
            this.banTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // BlacklistManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerMain);
            this.Name = "BlacklistManagementControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.BlacklistManagementControl_Load);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.groupBoxBlacklistEntries.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBlacklist)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxOperations.ResumeLayout(false);
            this.groupBoxOperations.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxBlacklistEntries;
        private System.Windows.Forms.DataGridView dataGridViewBlacklist;
        private System.Windows.Forms.GroupBox groupBoxOperations;
        private System.Windows.Forms.TextBox textBoxReason;
        private System.Windows.Forms.Label labelReason;
        private System.Windows.Forms.TextBox textBoxIPAddress;
        private System.Windows.Forms.Label labelIPAddress;
        private System.Windows.Forms.Button btnAddIP;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnRemoveSelected;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelBlacklistCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn iPAddressDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reasonDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn banTimeDataGridViewTextBoxColumn;
    }
}