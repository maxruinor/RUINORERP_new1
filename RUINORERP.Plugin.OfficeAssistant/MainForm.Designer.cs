namespace RUINORERP.Plugin.OfficeAssistant
{
    partial class MainForm
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
            this.fileSelectionPanel = new System.Windows.Forms.Panel();
            this.columnMappingPanel = new System.Windows.Forms.Panel();
            this.comparisonSettingsPanel = new System.Windows.Forms.Panel();
            this.resultDisplayPanel = new System.Windows.Forms.Panel();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSelectOldFile = new System.Windows.Forms.Button();
            this.btnSelectNewFile = new System.Windows.Forms.Button();
            this.txtOldFilePath = new System.Windows.Forms.TextBox();
            this.txtNewFilePath = new System.Windows.Forms.TextBox();
            this.lblOldFile = new System.Windows.Forms.Label();
            this.lblNewFile = new System.Windows.Forms.Label();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.dgvOldFilePreview = new System.Windows.Forms.DataGridView();
            this.dgvNewFilePreview = new System.Windows.Forms.DataGridView();
            this.lblOldFilePreview = new System.Windows.Forms.Label();
            this.lblNewFilePreview = new System.Windows.Forms.Label();
            this.lstOldColumns = new System.Windows.Forms.ListBox();
            this.lstNewColumns = new System.Windows.Forms.ListBox();
            this.lstKeyColumns = new System.Windows.Forms.ListBox();
            this.lstCompareColumns = new System.Windows.Forms.ListBox();
            this.lblOldColumns = new System.Windows.Forms.Label();
            this.lblNewColumns = new System.Windows.Forms.Label();
            this.lblKeyColumns = new System.Windows.Forms.Label();
            this.lblCompareColumns = new System.Windows.Forms.Label();
            this.btnAddKeyColumn = new System.Windows.Forms.Button();
            this.btnAddCompareColumn = new System.Windows.Forms.Button();
            this.btnAutoMatchColumns = new System.Windows.Forms.Button();
            this.btnSaveMapping = new System.Windows.Forms.Button();
            this.btnLoadMapping = new System.Windows.Forms.Button();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.chkIgnoreSpaces = new System.Windows.Forms.CheckBox();
            this.cmbComparisonMode = new System.Windows.Forms.ComboBox();
            this.lblComparisonMode = new System.Windows.Forms.Label();
            this.dgvComparisonResults = new System.Windows.Forms.DataGridView();
            this.btnStartComparison = new System.Windows.Forms.Button();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.btnClearResults = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.fileSelectionPanel.SuspendLayout();
            this.columnMappingPanel.SuspendLayout();
            this.comparisonSettingsPanel.SuspendLayout();
            this.resultDisplayPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldFilePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFilePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparisonResults)).BeginInit();
            this.SuspendLayout();
            // 
            // fileSelectionPanel
            // 
            this.fileSelectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileSelectionPanel.Controls.Add(this.btnLoadFiles);
            this.fileSelectionPanel.Controls.Add(this.lblNewFile);
            this.fileSelectionPanel.Controls.Add(this.lblOldFile);
            this.fileSelectionPanel.Controls.Add(this.txtNewFilePath);
            this.fileSelectionPanel.Controls.Add(this.txtOldFilePath);
            this.fileSelectionPanel.Controls.Add(this.btnSelectNewFile);
            this.fileSelectionPanel.Controls.Add(this.btnSelectOldFile);
            this.fileSelectionPanel.Controls.Add(this.label1);
            this.fileSelectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileSelectionPanel.Location = new System.Drawing.Point(0, 0);
            this.fileSelectionPanel.Name = "fileSelectionPanel";
            this.fileSelectionPanel.Size = new System.Drawing.Size(984, 100);
            this.fileSelectionPanel.TabIndex = 0;
            // 
            // columnMappingPanel
            // 
            this.columnMappingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.columnMappingPanel.Controls.Add(this.btnLoadMapping);
            this.columnMappingPanel.Controls.Add(this.btnSaveMapping);
            this.columnMappingPanel.Controls.Add(this.btnAutoMatchColumns);
            this.columnMappingPanel.Controls.Add(this.btnAddCompareColumn);
            this.columnMappingPanel.Controls.Add(this.btnAddKeyColumn);
            this.columnMappingPanel.Controls.Add(this.lblCompareColumns);
            this.columnMappingPanel.Controls.Add(this.lblKeyColumns);
            this.columnMappingPanel.Controls.Add(this.lblNewColumns);
            this.columnMappingPanel.Controls.Add(this.lblOldColumns);
            this.columnMappingPanel.Controls.Add(this.lstCompareColumns);
            this.columnMappingPanel.Controls.Add(this.lstKeyColumns);
            this.columnMappingPanel.Controls.Add(this.lstNewColumns);
            this.columnMappingPanel.Controls.Add(this.lstOldColumns);
            this.columnMappingPanel.Controls.Add(this.lblNewFilePreview);
            this.columnMappingPanel.Controls.Add(this.lblOldFilePreview);
            this.columnMappingPanel.Controls.Add(this.dgvNewFilePreview);
            this.columnMappingPanel.Controls.Add(this.dgvOldFilePreview);
            this.columnMappingPanel.Controls.Add(this.label2);
            this.columnMappingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.columnMappingPanel.Location = new System.Drawing.Point(0, 100);
            this.columnMappingPanel.Name = "columnMappingPanel";
            this.columnMappingPanel.Size = new System.Drawing.Size(984, 200);
            this.columnMappingPanel.TabIndex = 1;
            // 
            // comparisonSettingsPanel
            // 
            this.comparisonSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comparisonSettingsPanel.Controls.Add(this.lblComparisonMode);
            this.comparisonSettingsPanel.Controls.Add(this.cmbComparisonMode);
            this.comparisonSettingsPanel.Controls.Add(this.chkIgnoreSpaces);
            this.comparisonSettingsPanel.Controls.Add(this.chkCaseSensitive);
            this.comparisonSettingsPanel.Controls.Add(this.label3);
            this.comparisonSettingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.comparisonSettingsPanel.Location = new System.Drawing.Point(0, 300);
            this.comparisonSettingsPanel.Name = "comparisonSettingsPanel";
            this.comparisonSettingsPanel.Size = new System.Drawing.Size(984, 100);
            this.comparisonSettingsPanel.TabIndex = 2;
            // 
            // resultDisplayPanel
            // 
            this.resultDisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultDisplayPanel.Controls.Add(this.progressBar);
            this.resultDisplayPanel.Controls.Add(this.dgvComparisonResults);
            this.resultDisplayPanel.Controls.Add(this.label4);
            this.resultDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultDisplayPanel.Location = new System.Drawing.Point(0, 400);
            this.resultDisplayPanel.Name = "resultDisplayPanel";
            this.resultDisplayPanel.Size = new System.Drawing.Size(984, 211);
            this.resultDisplayPanel.TabIndex = 3;
            // 
            // actionPanel
            // 
            this.actionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionPanel.Controls.Add(this.btnClearResults);
            this.actionPanel.Controls.Add(this.btnExportResults);
            this.actionPanel.Controls.Add(this.btnStartComparison);
            this.actionPanel.Controls.Add(this.label5);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Location = new System.Drawing.Point(0, 611);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Size = new System.Drawing.Size(984, 50);
            this.actionPanel.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(982, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "文件选择";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(982, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "列映射配置";
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(982, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "对比设置";
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(982, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "结果展示";
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(982, 23);
            this.label5.TabIndex = 0;
            this.label5.Text = "操作区域";
            // 
            // btnSelectOldFile
            // 
            this.btnSelectOldFile.Location = new System.Drawing.Point(569, 26);
            this.btnSelectOldFile.Name = "btnSelectOldFile";
            this.btnSelectOldFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectOldFile.TabIndex = 1;
            this.btnSelectOldFile.Text = "选择文件";
            this.btnSelectOldFile.UseVisualStyleBackColor = true;
            // 
            // btnSelectNewFile
            // 
            this.btnSelectNewFile.Location = new System.Drawing.Point(569, 61);
            this.btnSelectNewFile.Name = "btnSelectNewFile";
            this.btnSelectNewFile.Size = new System.Drawing.Size(75, 23);
            this.btnSelectNewFile.TabIndex = 2;
            this.btnSelectNewFile.Text = "选择文件";
            this.btnSelectNewFile.UseVisualStyleBackColor = true;
            // 
            // txtOldFilePath
            // 
            this.txtOldFilePath.Location = new System.Drawing.Point(74, 28);
            this.txtOldFilePath.Name = "txtOldFilePath";
            this.txtOldFilePath.Size = new System.Drawing.Size(489, 20);
            this.txtOldFilePath.TabIndex = 3;
            // 
            // txtNewFilePath
            // 
            this.txtNewFilePath.Location = new System.Drawing.Point(74, 63);
            this.txtNewFilePath.Name = "txtNewFilePath";
            this.txtNewFilePath.Size = new System.Drawing.Size(489, 20);
            this.txtNewFilePath.TabIndex = 4;
            // 
            // lblOldFile
            // 
            this.lblOldFile.AutoSize = true;
            this.lblOldFile.Location = new System.Drawing.Point(13, 31);
            this.lblOldFile.Name = "lblOldFile";
            this.lblOldFile.Size = new System.Drawing.Size(43, 13);
            this.lblOldFile.TabIndex = 5;
            this.lblOldFile.Text = "旧文件:";
            // 
            // lblNewFile
            // 
            this.lblNewFile.AutoSize = true;
            this.lblNewFile.Location = new System.Drawing.Point(13, 66);
            this.lblNewFile.Name = "lblNewFile";
            this.lblNewFile.Size = new System.Drawing.Size(43, 13);
            this.lblNewFile.TabIndex = 6;
            this.lblNewFile.Text = "新文件:";
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(650, 26);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(75, 58);
            this.btnLoadFiles.TabIndex = 7;
            this.btnLoadFiles.Text = "加载文件";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            // 
            // dgvOldFilePreview
            // 
            this.dgvOldFilePreview.AllowUserToAddRows = false;
            this.dgvOldFilePreview.AllowUserToDeleteRows = false;
            this.dgvOldFilePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOldFilePreview.Location = new System.Drawing.Point(12, 43);
            this.dgvOldFilePreview.Name = "dgvOldFilePreview";
            this.dgvOldFilePreview.ReadOnly = true;
            this.dgvOldFilePreview.RowHeadersVisible = false;
            this.dgvOldFilePreview.Size = new System.Drawing.Size(200, 150);
            this.dgvOldFilePreview.TabIndex = 1;
            // 
            // dgvNewFilePreview
            // 
            this.dgvNewFilePreview.AllowUserToAddRows = false;
            this.dgvNewFilePreview.AllowUserToDeleteRows = false;
            this.dgvNewFilePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNewFilePreview.Location = new System.Drawing.Point(233, 43);
            this.dgvNewFilePreview.Name = "dgvNewFilePreview";
            this.dgvNewFilePreview.ReadOnly = true;
            this.dgvNewFilePreview.RowHeadersVisible = false;
            this.dgvNewFilePreview.Size = new System.Drawing.Size(200, 150);
            this.dgvNewFilePreview.TabIndex = 2;
            // 
            // lblOldFilePreview
            // 
            this.lblOldFilePreview.AutoSize = true;
            this.lblOldFilePreview.Location = new System.Drawing.Point(13, 27);
            this.lblOldFilePreview.Name = "lblOldFilePreview";
            this.lblOldFilePreview.Size = new System.Drawing.Size(55, 13);
            this.lblOldFilePreview.TabIndex = 3;
            this.lblOldFilePreview.Text = "旧文件预览";
            // 
            // lblNewFilePreview
            // 
            this.lblNewFilePreview.AutoSize = true;
            this.lblNewFilePreview.Location = new System.Drawing.Point(230, 27);
            this.lblNewFilePreview.Name = "lblNewFilePreview";
            this.lblNewFilePreview.Size = new System.Drawing.Size(55, 13);
            this.lblNewFilePreview.TabIndex = 4;
            this.lblNewFilePreview.Text = "新文件预览";
            // 
            // lstOldColumns
            // 
            this.lstOldColumns.FormattingEnabled = true;
            this.lstOldColumns.Location = new System.Drawing.Point(454, 43);
            this.lstOldColumns.Name = "lstOldColumns";
            this.lstOldColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstOldColumns.Size = new System.Drawing.Size(120, 56);
            this.lstOldColumns.TabIndex = 5;
            // 
            // lstNewColumns
            // 
            this.lstNewColumns.FormattingEnabled = true;
            this.lstNewColumns.Location = new System.Drawing.Point(595, 43);
            this.lstNewColumns.Name = "lstNewColumns";
            this.lstNewColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstNewColumns.Size = new System.Drawing.Size(120, 56);
            this.lstNewColumns.TabIndex = 6;
            // 
            // lstKeyColumns
            // 
            this.lstKeyColumns.FormattingEnabled = true;
            this.lstKeyColumns.Location = new System.Drawing.Point(454, 137);
            this.lstKeyColumns.Name = "lstKeyColumns";
            this.lstKeyColumns.Size = new System.Drawing.Size(120, 56);
            this.lstKeyColumns.TabIndex = 7;
            // 
            // lstCompareColumns
            // 
            this.lstCompareColumns.FormattingEnabled = true;
            this.lstCompareColumns.Location = new System.Drawing.Point(595, 137);
            this.lstCompareColumns.Name = "lstCompareColumns";
            this.lstCompareColumns.Size = new System.Drawing.Size(120, 56);
            this.lstCompareColumns.TabIndex = 8;
            // 
            // lblOldColumns
            // 
            this.lblOldColumns.AutoSize = true;
            this.lblOldColumns.Location = new System.Drawing.Point(451, 27);
            this.lblOldColumns.Name = "lblOldColumns";
            this.lblOldColumns.Size = new System.Drawing.Size(67, 13);
            this.lblOldColumns.TabIndex = 9;
            this.lblOldColumns.Text = "旧文件列名";
            // 
            // lblNewColumns
            // 
            this.lblNewColumns.AutoSize = true;
            this.lblNewColumns.Location = new System.Drawing.Point(592, 27);
            this.lblNewColumns.Name = "lblNewColumns";
            this.lblNewColumns.Size = new System.Drawing.Size(67, 13);
            this.lblNewColumns.TabIndex = 10;
            this.lblNewColumns.Text = "新文件列名";
            // 
            // lblKeyColumns
            // 
            this.lblKeyColumns.AutoSize = true;
            this.lblKeyColumns.Location = new System.Drawing.Point(451, 121);
            this.lblKeyColumns.Name = "lblKeyColumns";
            this.lblKeyColumns.Size = new System.Drawing.Size(43, 13);
            this.lblKeyColumns.TabIndex = 11;
            this.lblKeyColumns.Text = "键列名";
            // 
            // lblCompareColumns
            // 
            this.lblCompareColumns.AutoSize = true;
            this.lblCompareColumns.Location = new System.Drawing.Point(592, 121);
            this.lblCompareColumns.Name = "lblCompareColumns";
            this.lblCompareColumns.Size = new System.Drawing.Size(55, 13);
            this.lblCompareColumns.TabIndex = 12;
            this.lblCompareColumns.Text = "比较列名";
            // 
            // btnAddKeyColumn
            // 
            this.btnAddKeyColumn.Location = new System.Drawing.Point(736, 137);
            this.btnAddKeyColumn.Name = "btnAddKeyColumn";
            this.btnAddKeyColumn.Size = new System.Drawing.Size(75, 23);
            this.btnAddKeyColumn.TabIndex = 13;
            this.btnAddKeyColumn.Text = "添加键列";
            this.btnAddKeyColumn.UseVisualStyleBackColor = true;
            // 
            // btnAddCompareColumn
            // 
            this.btnAddCompareColumn.Location = new System.Drawing.Point(736, 170);
            this.btnAddCompareColumn.Name = "btnAddCompareColumn";
            this.btnAddCompareColumn.Size = new System.Drawing.Size(95, 23);
            this.btnAddCompareColumn.TabIndex = 14;
            this.btnAddCompareColumn.Text = "添加比较列";
            this.btnAddCompareColumn.UseVisualStyleBackColor = true;
            // 
            // btnAutoMatchColumns
            // 
            this.btnAutoMatchColumns.Location = new System.Drawing.Point(736, 43);
            this.btnAutoMatchColumns.Name = "btnAutoMatchColumns";
            this.btnAutoMatchColumns.Size = new System.Drawing.Size(95, 23);
            this.btnAutoMatchColumns.TabIndex = 15;
            this.btnAutoMatchColumns.Text = "自动匹配列";
            this.btnAutoMatchColumns.UseVisualStyleBackColor = true;
            // 
            // btnSaveMapping
            // 
            this.btnSaveMapping.Location = new System.Drawing.Point(837, 43);
            this.btnSaveMapping.Name = "btnSaveMapping";
            this.btnSaveMapping.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMapping.TabIndex = 16;
            this.btnSaveMapping.Text = "保存配置";
            this.btnSaveMapping.UseVisualStyleBackColor = true;
            // 
            // btnLoadMapping
            // 
            this.btnLoadMapping.Location = new System.Drawing.Point(837, 72);
            this.btnLoadMapping.Name = "btnLoadMapping";
            this.btnLoadMapping.Size = new System.Drawing.Size(75, 23);
            this.btnLoadMapping.TabIndex = 17;
            this.btnLoadMapping.Text = "加载配置";
            this.btnLoadMapping.UseVisualStyleBackColor = true;
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(16, 40);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(92, 17);
            this.chkCaseSensitive.TabIndex = 1;
            this.chkCaseSensitive.Text = "区分大小写";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreSpaces
            // 
            this.chkIgnoreSpaces.AutoSize = true;
            this.chkIgnoreSpaces.Location = new System.Drawing.Point(16, 63);
            this.chkIgnoreSpaces.Name = "chkIgnoreSpaces";
            this.chkIgnoreSpaces.Size = new System.Drawing.Size(110, 17);
            this.chkIgnoreSpaces.TabIndex = 2;
            this.chkIgnoreSpaces.Text = "忽略前后空格";
            this.chkIgnoreSpaces.UseVisualStyleBackColor = true;
            // 
            // cmbComparisonMode
            // 
            this.cmbComparisonMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComparisonMode.FormattingEnabled = true;
            this.cmbComparisonMode.Location = new System.Drawing.Point(156, 38);
            this.cmbComparisonMode.Name = "cmbComparisonMode";
            this.cmbComparisonMode.Size = new System.Drawing.Size(162, 21);
            this.cmbComparisonMode.TabIndex = 3;
            // 
            // lblComparisonMode
            // 
            this.lblComparisonMode.AutoSize = true;
            this.lblComparisonMode.Location = new System.Drawing.Point(153, 22);
            this.lblComparisonMode.Name = "lblComparisonMode";
            this.lblComparisonMode.Size = new System.Drawing.Size(55, 13);
            this.lblComparisonMode.TabIndex = 4;
            this.lblComparisonMode.Text = "对比模式";
            // 
            // dgvComparisonResults
            // 
            this.dgvComparisonResults.AllowUserToAddRows = false;
            this.dgvComparisonResults.AllowUserToDeleteRows = false;
            this.dgvComparisonResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvComparisonResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComparisonResults.Location = new System.Drawing.Point(12, 26);
            this.dgvComparisonResults.Name = "dgvComparisonResults";
            this.dgvComparisonResults.ReadOnly = true;
            this.dgvComparisonResults.RowHeadersVisible = false;
            this.dgvComparisonResults.Size = new System.Drawing.Size(960, 150);
            this.dgvComparisonResults.TabIndex = 1;
            // 
            // btnStartComparison
            // 
            this.btnStartComparison.Location = new System.Drawing.Point(12, 26);
            this.btnStartComparison.Name = "btnStartComparison";
            this.btnStartComparison.Size = new System.Drawing.Size(75, 23);
            this.btnStartComparison.TabIndex = 1;
            this.btnStartComparison.Text = "开始对比";
            this.btnStartComparison.UseVisualStyleBackColor = true;
            // 
            // btnExportResults
            // 
            this.btnExportResults.Location = new System.Drawing.Point(93, 26);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(75, 23);
            this.btnExportResults.TabIndex = 2;
            this.btnExportResults.Text = "导出结果";
            this.btnExportResults.UseVisualStyleBackColor = true;
            // 
            // btnClearResults
            // 
            this.btnClearResults.Location = new System.Drawing.Point(174, 26);
            this.btnClearResults.Name = "btnClearResults";
            this.btnClearResults.Size = new System.Drawing.Size(75, 23);
            this.btnClearResults.TabIndex = 3;
            this.btnClearResults.Text = "清除结果";
            this.btnClearResults.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 182);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(960, 23);
            this.progressBar.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.resultDisplayPanel);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.comparisonSettingsPanel);
            this.Controls.Add(this.columnMappingPanel);
            this.Controls.Add(this.fileSelectionPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "办公助手 - Excel对比工具";
            this.fileSelectionPanel.ResumeLayout(false);
            this.fileSelectionPanel.PerformLayout();
            this.columnMappingPanel.ResumeLayout(false);
            this.columnMappingPanel.PerformLayout();
            this.comparisonSettingsPanel.ResumeLayout(false);
            this.comparisonSettingsPanel.PerformLayout();
            this.resultDisplayPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldFilePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFilePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparisonResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel fileSelectionPanel;
        private System.Windows.Forms.Panel columnMappingPanel;
        private System.Windows.Forms.Panel comparisonSettingsPanel;
        private System.Windows.Forms.Panel resultDisplayPanel;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLoadFiles;
        private System.Windows.Forms.Label lblNewFile;
        private System.Windows.Forms.Label lblOldFile;
        private System.Windows.Forms.TextBox txtNewFilePath;
        private System.Windows.Forms.TextBox txtOldFilePath;
        private System.Windows.Forms.Button btnSelectNewFile;
        private System.Windows.Forms.Button btnSelectOldFile;
        private System.Windows.Forms.Button btnLoadMapping;
        private System.Windows.Forms.Button btnSaveMapping;
        private System.Windows.Forms.Button btnAutoMatchColumns;
        private System.Windows.Forms.Button btnAddCompareColumn;
        private System.Windows.Forms.Button btnAddKeyColumn;
        private System.Windows.Forms.Label lblCompareColumns;
        private System.Windows.Forms.Label lblKeyColumns;
        private System.Windows.Forms.Label lblNewColumns;
        private System.Windows.Forms.Label lblOldColumns;
        private System.Windows.Forms.ListBox lstCompareColumns;
        private System.Windows.Forms.ListBox lstKeyColumns;
        private System.Windows.Forms.ListBox lstNewColumns;
        private System.Windows.Forms.ListBox lstOldColumns;
        private System.Windows.Forms.Label lblNewFilePreview;
        private System.Windows.Forms.Label lblOldFilePreview;
        private System.Windows.Forms.DataGridView dgvNewFilePreview;
        private System.Windows.Forms.DataGridView dgvOldFilePreview;
        private System.Windows.Forms.Label lblComparisonMode;
        private System.Windows.Forms.ComboBox cmbComparisonMode;
        private System.Windows.Forms.CheckBox chkIgnoreSpaces;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.DataGridView dgvComparisonResults;
        private System.Windows.Forms.Button btnClearResults;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.Button btnStartComparison;
    }
}