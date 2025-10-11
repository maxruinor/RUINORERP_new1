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
            this.chkAutoLoadFiles = new System.Windows.Forms.CheckBox();
            this.btnLoadFiles = new System.Windows.Forms.Button();
            this.lblNewFile = new System.Windows.Forms.Label();
            this.lblOldFile = new System.Windows.Forms.Label();
            this.txtNewFilePath = new System.Windows.Forms.TextBox();
            this.txtOldFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectNewFile = new System.Windows.Forms.Button();
            this.btnSelectOldFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comparisonSettingsPanel = new System.Windows.Forms.Panel();
            this.chkShowSameData = new System.Windows.Forms.CheckBox();
            this.lblNewWorksheet = new System.Windows.Forms.Label();
            this.lblOldWorksheet = new System.Windows.Forms.Label();
            this.cmbNewWorksheet = new System.Windows.Forms.ComboBox();
            this.cmbOldWorksheet = new System.Windows.Forms.ComboBox();
            this.lblComparisonMode = new System.Windows.Forms.Label();
            this.cmbComparisonMode = new System.Windows.Forms.ComboBox();
            this.chkIgnoreSpaces = new System.Windows.Forms.CheckBox();
            this.chkCaseSensitive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.resultDisplayPanel = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.dgvComparisonResults = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.btnStartComparison = new System.Windows.Forms.Button();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.btnClearResults = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.columnMappingPanel = new System.Windows.Forms.Panel();
            this.splitContainerColumnMapping = new System.Windows.Forms.SplitContainer();
            this.splitContainerFilePreviews = new System.Windows.Forms.SplitContainer();
            this.dgvOldFilePreview = new System.Windows.Forms.DataGridView();
            this.lblOldFileSummary = new System.Windows.Forms.Label();
            this.lblOldFilePreview = new System.Windows.Forms.Label();
            this.dgvNewFilePreview = new System.Windows.Forms.DataGridView();
            this.lblNewFileSummary = new System.Windows.Forms.Label();
            this.lblNewFilePreview = new System.Windows.Forms.Label();
            this.panelMappingSettings = new System.Windows.Forms.Panel();
            this.btnLoadMapping = new System.Windows.Forms.Button();
            this.btnSaveMapping = new System.Windows.Forms.Button();
            this.btnAutoMatchColumns = new System.Windows.Forms.Button();
            this.btnAddCompareColumn = new System.Windows.Forms.Button();
            this.btnAddKeyColumn = new System.Windows.Forms.Button();
            this.lblCompareColumns = new System.Windows.Forms.Label();
            this.lblKeyColumns = new System.Windows.Forms.Label();
            this.lblNewColumns = new System.Windows.Forms.Label();
            this.lblOldColumns = new System.Windows.Forms.Label();
            this.lstCompareColumns = new System.Windows.Forms.ListBox();
            this.lstKeyColumns = new System.Windows.Forms.ListBox();
            this.lstNewColumns = new System.Windows.Forms.ListBox();
            this.lstOldColumns = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fileSelectionPanel.SuspendLayout();
            this.comparisonSettingsPanel.SuspendLayout();
            this.resultDisplayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparisonResults)).BeginInit();
            this.actionPanel.SuspendLayout();
            this.columnMappingPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerColumnMapping)).BeginInit();
            this.splitContainerColumnMapping.Panel1.SuspendLayout();
            this.splitContainerColumnMapping.Panel2.SuspendLayout();
            this.splitContainerColumnMapping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilePreviews)).BeginInit();
            this.splitContainerFilePreviews.Panel1.SuspendLayout();
            this.splitContainerFilePreviews.Panel2.SuspendLayout();
            this.splitContainerFilePreviews.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldFilePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFilePreview)).BeginInit();
            this.panelMappingSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSelectionPanel
            // 
            this.fileSelectionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileSelectionPanel.Controls.Add(this.chkAutoLoadFiles);
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
            this.fileSelectionPanel.Size = new System.Drawing.Size(1008, 92);
            this.fileSelectionPanel.TabIndex = 0;
            // 
            // chkAutoLoadFiles
            // 
            this.chkAutoLoadFiles.AutoSize = true;
            this.chkAutoLoadFiles.Location = new System.Drawing.Point(839, 28);
            this.chkAutoLoadFiles.Name = "chkAutoLoadFiles";
            this.chkAutoLoadFiles.Size = new System.Drawing.Size(108, 16);
            this.chkAutoLoadFiles.TabIndex = 8;
            this.chkAutoLoadFiles.Text = "启动时自动加载";
            this.chkAutoLoadFiles.UseVisualStyleBackColor = true;
            this.chkAutoLoadFiles.CheckedChanged += new System.EventHandler(this.chkAutoLoadFiles_CheckedChanged);
            // 
            // btnLoadFiles
            // 
            this.btnLoadFiles.Location = new System.Drawing.Point(735, 25);
            this.btnLoadFiles.Name = "btnLoadFiles";
            this.btnLoadFiles.Size = new System.Drawing.Size(75, 54);
            this.btnLoadFiles.TabIndex = 7;
            this.btnLoadFiles.Text = "加载文件";
            this.btnLoadFiles.UseVisualStyleBackColor = true;
            this.btnLoadFiles.Click += new System.EventHandler(this.btnLoadFiles_Click);
            // 
            // lblNewFile
            // 
            this.lblNewFile.AutoSize = true;
            this.lblNewFile.Location = new System.Drawing.Point(13, 61);
            this.lblNewFile.Name = "lblNewFile";
            this.lblNewFile.Size = new System.Drawing.Size(65, 12);
            this.lblNewFile.TabIndex = 6;
            this.lblNewFile.Text = "新数据文件";
            // 
            // lblOldFile
            // 
            this.lblOldFile.AutoSize = true;
            this.lblOldFile.Location = new System.Drawing.Point(13, 29);
            this.lblOldFile.Name = "lblOldFile";
            this.lblOldFile.Size = new System.Drawing.Size(65, 12);
            this.lblOldFile.TabIndex = 5;
            this.lblOldFile.Text = "旧数据文件";
            // 
            // txtNewFilePath
            // 
            this.txtNewFilePath.AllowDrop = true;
            this.txtNewFilePath.Location = new System.Drawing.Point(84, 58);
            this.txtNewFilePath.Name = "txtNewFilePath";
            this.txtNewFilePath.Size = new System.Drawing.Size(564, 21);
            this.txtNewFilePath.TabIndex = 4;
            this.txtNewFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNewFilePath_DragDrop);
            this.txtNewFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNewFilePath_DragEnter);
            // 
            // txtOldFilePath
            // 
            this.txtOldFilePath.AllowDrop = true;
            this.txtOldFilePath.Location = new System.Drawing.Point(84, 26);
            this.txtOldFilePath.Name = "txtOldFilePath";
            this.txtOldFilePath.Size = new System.Drawing.Size(564, 21);
            this.txtOldFilePath.TabIndex = 3;
            this.txtOldFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtOldFilePath_DragDrop);
            this.txtOldFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtOldFilePath_DragEnter);
            // 
            // btnSelectNewFile
            // 
            this.btnSelectNewFile.Location = new System.Drawing.Point(654, 58);
            this.btnSelectNewFile.Name = "btnSelectNewFile";
            this.btnSelectNewFile.Size = new System.Drawing.Size(75, 21);
            this.btnSelectNewFile.TabIndex = 2;
            this.btnSelectNewFile.Text = "选择文件";
            this.btnSelectNewFile.UseVisualStyleBackColor = true;
            this.btnSelectNewFile.Click += new System.EventHandler(this.btnSelectNewFile_Click);
            // 
            // btnSelectOldFile
            // 
            this.btnSelectOldFile.Location = new System.Drawing.Point(654, 26);
            this.btnSelectOldFile.Name = "btnSelectOldFile";
            this.btnSelectOldFile.Size = new System.Drawing.Size(75, 21);
            this.btnSelectOldFile.TabIndex = 1;
            this.btnSelectOldFile.Text = "选择文件";
            this.btnSelectOldFile.UseVisualStyleBackColor = true;
            this.btnSelectOldFile.Click += new System.EventHandler(this.btnSelectOldFile_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1006, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据文件选择";
            // 
            // comparisonSettingsPanel
            // 
            this.comparisonSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.comparisonSettingsPanel.Controls.Add(this.chkShowSameData);
            this.comparisonSettingsPanel.Controls.Add(this.lblNewWorksheet);
            this.comparisonSettingsPanel.Controls.Add(this.lblOldWorksheet);
            this.comparisonSettingsPanel.Controls.Add(this.cmbNewWorksheet);
            this.comparisonSettingsPanel.Controls.Add(this.cmbOldWorksheet);
            this.comparisonSettingsPanel.Controls.Add(this.lblComparisonMode);
            this.comparisonSettingsPanel.Controls.Add(this.cmbComparisonMode);
            this.comparisonSettingsPanel.Controls.Add(this.chkIgnoreSpaces);
            this.comparisonSettingsPanel.Controls.Add(this.chkCaseSensitive);
            this.comparisonSettingsPanel.Controls.Add(this.label3);
            this.comparisonSettingsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.comparisonSettingsPanel.Location = new System.Drawing.Point(0, 323);
            this.comparisonSettingsPanel.Name = "comparisonSettingsPanel";
            this.comparisonSettingsPanel.Size = new System.Drawing.Size(1008, 110);
            this.comparisonSettingsPanel.TabIndex = 2;
            // 
            // chkShowSameData
            // 
            this.chkShowSameData.AutoSize = true;
            this.chkShowSameData.Location = new System.Drawing.Point(16, 80);
            this.chkShowSameData.Name = "chkShowSameData";
            this.chkShowSameData.Size = new System.Drawing.Size(96, 16);
            this.chkShowSameData.TabIndex = 5;
            this.chkShowSameData.Text = "显示相同数据";
            this.chkShowSameData.UseVisualStyleBackColor = true;
            this.chkShowSameData.CheckedChanged += new System.EventHandler(this.chkShowSameData_CheckedChanged);
            // 
            // lblNewWorksheet
            // 
            this.lblNewWorksheet.AutoSize = true;
            this.lblNewWorksheet.Location = new System.Drawing.Point(340, 60);
            this.lblNewWorksheet.Name = "lblNewWorksheet";
            this.lblNewWorksheet.Size = new System.Drawing.Size(77, 12);
            this.lblNewWorksheet.TabIndex = 8;
            this.lblNewWorksheet.Text = "新数据工作表";
            // 
            // lblOldWorksheet
            // 
            this.lblOldWorksheet.AutoSize = true;
            this.lblOldWorksheet.Location = new System.Drawing.Point(340, 35);
            this.lblOldWorksheet.Name = "lblOldWorksheet";
            this.lblOldWorksheet.Size = new System.Drawing.Size(77, 12);
            this.lblOldWorksheet.TabIndex = 7;
            this.lblOldWorksheet.Text = "旧数据工作表";
            // 
            // cmbNewWorksheet
            // 
            this.cmbNewWorksheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewWorksheet.FormattingEnabled = true;
            this.cmbNewWorksheet.Location = new System.Drawing.Point(421, 57);
            this.cmbNewWorksheet.Name = "cmbNewWorksheet";
            this.cmbNewWorksheet.Size = new System.Drawing.Size(162, 20);
            this.cmbNewWorksheet.TabIndex = 6;
            // 
            // cmbOldWorksheet
            // 
            this.cmbOldWorksheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOldWorksheet.FormattingEnabled = true;
            this.cmbOldWorksheet.Location = new System.Drawing.Point(421, 32);
            this.cmbOldWorksheet.Name = "cmbOldWorksheet";
            this.cmbOldWorksheet.Size = new System.Drawing.Size(162, 20);
            this.cmbOldWorksheet.TabIndex = 5;
            // 
            // lblComparisonMode
            // 
            this.lblComparisonMode.AutoSize = true;
            this.lblComparisonMode.Location = new System.Drawing.Point(153, 20);
            this.lblComparisonMode.Name = "lblComparisonMode";
            this.lblComparisonMode.Size = new System.Drawing.Size(53, 12);
            this.lblComparisonMode.TabIndex = 4;
            this.lblComparisonMode.Text = "对比模式";
            // 
            // cmbComparisonMode
            // 
            this.cmbComparisonMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbComparisonMode.FormattingEnabled = true;
            this.cmbComparisonMode.Location = new System.Drawing.Point(156, 35);
            this.cmbComparisonMode.Name = "cmbComparisonMode";
            this.cmbComparisonMode.Size = new System.Drawing.Size(162, 20);
            this.cmbComparisonMode.TabIndex = 3;
            this.cmbComparisonMode.SelectedIndexChanged += new System.EventHandler(this.cmbComparisonMode_SelectedIndexChanged);
            // 
            // chkIgnoreSpaces
            // 
            this.chkIgnoreSpaces.AutoSize = true;
            this.chkIgnoreSpaces.Location = new System.Drawing.Point(16, 58);
            this.chkIgnoreSpaces.Name = "chkIgnoreSpaces";
            this.chkIgnoreSpaces.Size = new System.Drawing.Size(96, 16);
            this.chkIgnoreSpaces.TabIndex = 2;
            this.chkIgnoreSpaces.Text = "忽略前后空格";
            this.chkIgnoreSpaces.UseVisualStyleBackColor = true;
            this.chkIgnoreSpaces.CheckedChanged += new System.EventHandler(this.chkIgnoreSpaces_CheckedChanged);
            // 
            // chkCaseSensitive
            // 
            this.chkCaseSensitive.AutoSize = true;
            this.chkCaseSensitive.Location = new System.Drawing.Point(16, 37);
            this.chkCaseSensitive.Name = "chkCaseSensitive";
            this.chkCaseSensitive.Size = new System.Drawing.Size(84, 16);
            this.chkCaseSensitive.TabIndex = 1;
            this.chkCaseSensitive.Text = "区分大小写";
            this.chkCaseSensitive.UseVisualStyleBackColor = true;
            this.chkCaseSensitive.CheckedChanged += new System.EventHandler(this.chkCaseSensitive_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1006, 21);
            this.label3.TabIndex = 0;
            this.label3.Text = "对比设置";
            // 
            // resultDisplayPanel
            // 
            this.resultDisplayPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.resultDisplayPanel.Controls.Add(this.dgvComparisonResults);
            this.resultDisplayPanel.Controls.Add(this.progressBar);
            this.resultDisplayPanel.Controls.Add(this.label4);
            this.resultDisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultDisplayPanel.Location = new System.Drawing.Point(0, 433);
            this.resultDisplayPanel.Name = "resultDisplayPanel";
            this.resultDisplayPanel.Size = new System.Drawing.Size(1008, 177);
            this.resultDisplayPanel.TabIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 154);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1006, 21);
            this.progressBar.TabIndex = 2;
            // 
            // dgvComparisonResults
            // 
            this.dgvComparisonResults.AllowUserToAddRows = false;
            this.dgvComparisonResults.AllowUserToDeleteRows = false;
            this.dgvComparisonResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComparisonResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvComparisonResults.Location = new System.Drawing.Point(0, 21);
            this.dgvComparisonResults.Name = "dgvComparisonResults";
            this.dgvComparisonResults.Size = new System.Drawing.Size(1006, 133);
            this.dgvComparisonResults.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1006, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "对比结果";
            // 
            // actionPanel
            // 
            this.actionPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.actionPanel.Controls.Add(this.btnStartComparison);
            this.actionPanel.Controls.Add(this.btnExportResults);
            this.actionPanel.Controls.Add(this.btnClearResults);
            this.actionPanel.Controls.Add(this.label5);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Location = new System.Drawing.Point(0, 610);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Size = new System.Drawing.Size(1008, 46);
            this.actionPanel.TabIndex = 4;
            // 
            // btnStartComparison
            // 
            this.btnStartComparison.Location = new System.Drawing.Point(12, 24);
            this.btnStartComparison.Name = "btnStartComparison";
            this.btnStartComparison.Size = new System.Drawing.Size(75, 21);
            this.btnStartComparison.TabIndex = 1;
            this.btnStartComparison.Text = "开始对比";
            this.btnStartComparison.UseVisualStyleBackColor = true;
            this.btnStartComparison.Click += new System.EventHandler(this.btnStartComparison_Click);
            // 
            // btnExportResults
            // 
            this.btnExportResults.Location = new System.Drawing.Point(93, 24);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(75, 21);
            this.btnExportResults.TabIndex = 2;
            this.btnExportResults.Text = "导出结果";
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += new System.EventHandler(this.btnExportResults_Click);
            // 
            // btnClearResults
            // 
            this.btnClearResults.Location = new System.Drawing.Point(174, 24);
            this.btnClearResults.Name = "btnClearResults";
            this.btnClearResults.Size = new System.Drawing.Size(75, 21);
            this.btnClearResults.TabIndex = 3;
            this.btnClearResults.Text = "清除结果";
            this.btnClearResults.UseVisualStyleBackColor = true;
            this.btnClearResults.Click += new System.EventHandler(this.btnClearResults_Click);
            // 
            // label5
            // 
            this.label5.Dock = System.Windows.Forms.DockStyle.Top;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(1006, 21);
            this.label5.TabIndex = 0;
            this.label5.Text = "操作按钮";
            // 
            // columnMappingPanel
            // 
            this.columnMappingPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.columnMappingPanel.Controls.Add(this.splitContainerColumnMapping);
            this.columnMappingPanel.Controls.Add(this.label2);
            this.columnMappingPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.columnMappingPanel.Location = new System.Drawing.Point(0, 92);
            this.columnMappingPanel.Name = "columnMappingPanel";
            this.columnMappingPanel.Size = new System.Drawing.Size(1008, 231);
            this.columnMappingPanel.TabIndex = 1;
            // 
            // splitContainerColumnMapping
            // 
            this.splitContainerColumnMapping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerColumnMapping.Location = new System.Drawing.Point(0, 21);
            this.splitContainerColumnMapping.Name = "splitContainerColumnMapping";
            // 
            // splitContainerColumnMapping.Panel1
            // 
            this.splitContainerColumnMapping.Panel1.Controls.Add(this.splitContainerFilePreviews);
            // 
            // splitContainerColumnMapping.Panel2
            // 
            this.splitContainerColumnMapping.Panel2.Controls.Add(this.panelMappingSettings);
            this.splitContainerColumnMapping.Size = new System.Drawing.Size(1006, 208);
            this.splitContainerColumnMapping.SplitterDistance = 452;
            this.splitContainerColumnMapping.TabIndex = 1;
            // 
            // splitContainerFilePreviews
            // 
            this.splitContainerFilePreviews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFilePreviews.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFilePreviews.Name = "splitContainerFilePreviews";
            // 
            // splitContainerFilePreviews.Panel1
            // 
            this.splitContainerFilePreviews.Panel1.Controls.Add(this.dgvOldFilePreview);
            this.splitContainerFilePreviews.Panel1.Controls.Add(this.lblOldFileSummary);
            this.splitContainerFilePreviews.Panel1.Controls.Add(this.lblOldFilePreview);
            // 
            // splitContainerFilePreviews.Panel2
            // 
            this.splitContainerFilePreviews.Panel2.Controls.Add(this.dgvNewFilePreview);
            this.splitContainerFilePreviews.Panel2.Controls.Add(this.lblNewFileSummary);
            this.splitContainerFilePreviews.Panel2.Controls.Add(this.lblNewFilePreview);
            this.splitContainerFilePreviews.Size = new System.Drawing.Size(452, 208);
            this.splitContainerFilePreviews.SplitterDistance = 217;
            this.splitContainerFilePreviews.TabIndex = 0;
            // 
            // dgvOldFilePreview
            // 
            this.dgvOldFilePreview.AllowUserToAddRows = false;
            this.dgvOldFilePreview.AllowUserToDeleteRows = false;
            this.dgvOldFilePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOldFilePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOldFilePreview.Location = new System.Drawing.Point(0, 24);
            this.dgvOldFilePreview.Name = "dgvOldFilePreview";
            this.dgvOldFilePreview.ReadOnly = true;
            this.dgvOldFilePreview.RowHeadersVisible = false;
            this.dgvOldFilePreview.Size = new System.Drawing.Size(217, 184);
            this.dgvOldFilePreview.TabIndex = 3;
            // 
            // lblOldFileSummary
            // 
            this.lblOldFileSummary.AutoSize = true;
            this.lblOldFileSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOldFileSummary.Location = new System.Drawing.Point(0, 12);
            this.lblOldFileSummary.Name = "lblOldFileSummary";
            this.lblOldFileSummary.Size = new System.Drawing.Size(0, 12);
            this.lblOldFileSummary.TabIndex = 5;
            // 
            // lblOldFilePreview
            // 
            this.lblOldFilePreview.AutoSize = true;
            this.lblOldFilePreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOldFilePreview.Location = new System.Drawing.Point(0, 0);
            this.lblOldFilePreview.Name = "lblOldFilePreview";
            this.lblOldFilePreview.Size = new System.Drawing.Size(89, 12);
            this.lblOldFilePreview.TabIndex = 4;
            this.lblOldFilePreview.Text = "旧数据文件预览";
            // 
            // dgvNewFilePreview
            // 
            this.dgvNewFilePreview.AllowUserToAddRows = false;
            this.dgvNewFilePreview.AllowUserToDeleteRows = false;
            this.dgvNewFilePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNewFilePreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNewFilePreview.Location = new System.Drawing.Point(0, 24);
            this.dgvNewFilePreview.Name = "dgvNewFilePreview";
            this.dgvNewFilePreview.ReadOnly = true;
            this.dgvNewFilePreview.RowHeadersVisible = false;
            this.dgvNewFilePreview.Size = new System.Drawing.Size(231, 184);
            this.dgvNewFilePreview.TabIndex = 4;
            // 
            // lblNewFileSummary
            // 
            this.lblNewFileSummary.AutoSize = true;
            this.lblNewFileSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNewFileSummary.Location = new System.Drawing.Point(0, 12);
            this.lblNewFileSummary.Name = "lblNewFileSummary";
            this.lblNewFileSummary.Size = new System.Drawing.Size(0, 12);
            this.lblNewFileSummary.TabIndex = 6;
            // 
            // lblNewFilePreview
            // 
            this.lblNewFilePreview.AutoSize = true;
            this.lblNewFilePreview.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNewFilePreview.Location = new System.Drawing.Point(0, 0);
            this.lblNewFilePreview.Name = "lblNewFilePreview";
            this.lblNewFilePreview.Size = new System.Drawing.Size(89, 12);
            this.lblNewFilePreview.TabIndex = 5;
            this.lblNewFilePreview.Text = "新数据文件预览";
            // 
            // panelMappingSettings
            // 
            this.panelMappingSettings.Controls.Add(this.btnLoadMapping);
            this.panelMappingSettings.Controls.Add(this.btnSaveMapping);
            this.panelMappingSettings.Controls.Add(this.btnAutoMatchColumns);
            this.panelMappingSettings.Controls.Add(this.btnAddCompareColumn);
            this.panelMappingSettings.Controls.Add(this.btnAddKeyColumn);
            this.panelMappingSettings.Controls.Add(this.lblCompareColumns);
            this.panelMappingSettings.Controls.Add(this.lblKeyColumns);
            this.panelMappingSettings.Controls.Add(this.lblNewColumns);
            this.panelMappingSettings.Controls.Add(this.lblOldColumns);
            this.panelMappingSettings.Controls.Add(this.lstCompareColumns);
            this.panelMappingSettings.Controls.Add(this.lstKeyColumns);
            this.panelMappingSettings.Controls.Add(this.lstNewColumns);
            this.panelMappingSettings.Controls.Add(this.lstOldColumns);
            this.panelMappingSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMappingSettings.Location = new System.Drawing.Point(0, 0);
            this.panelMappingSettings.Name = "panelMappingSettings";
            this.panelMappingSettings.Size = new System.Drawing.Size(550, 208);
            this.panelMappingSettings.TabIndex = 0;
            // 
            // btnLoadMapping
            // 
            this.btnLoadMapping.Location = new System.Drawing.Point(464, 182);
            this.btnLoadMapping.Name = "btnLoadMapping";
            this.btnLoadMapping.Size = new System.Drawing.Size(75, 21);
            this.btnLoadMapping.TabIndex = 22;
            this.btnLoadMapping.Text = "加载配置";
            this.btnLoadMapping.UseVisualStyleBackColor = true;
            this.btnLoadMapping.Click += new System.EventHandler(this.btnLoadMapping_Click);
            // 
            // btnSaveMapping
            // 
            this.btnSaveMapping.Location = new System.Drawing.Point(383, 182);
            this.btnSaveMapping.Name = "btnSaveMapping";
            this.btnSaveMapping.Size = new System.Drawing.Size(75, 21);
            this.btnSaveMapping.TabIndex = 21;
            this.btnSaveMapping.Text = "保存配置";
            this.btnSaveMapping.UseVisualStyleBackColor = true;
            this.btnSaveMapping.Click += new System.EventHandler(this.btnSaveMapping_Click);
            // 
            // btnAutoMatchColumns
            // 
            this.btnAutoMatchColumns.Location = new System.Drawing.Point(279, 182);
            this.btnAutoMatchColumns.Name = "btnAutoMatchColumns";
            this.btnAutoMatchColumns.Size = new System.Drawing.Size(95, 21);
            this.btnAutoMatchColumns.TabIndex = 20;
            this.btnAutoMatchColumns.Text = "自动匹配列";
            this.btnAutoMatchColumns.UseVisualStyleBackColor = true;
            this.btnAutoMatchColumns.Click += new System.EventHandler(this.btnAutoMatchColumns_Click);
            // 
            // btnAddCompareColumn
            // 
            this.btnAddCompareColumn.Location = new System.Drawing.Point(145, 182);
            this.btnAddCompareColumn.Name = "btnAddCompareColumn";
            this.btnAddCompareColumn.Size = new System.Drawing.Size(95, 21);
            this.btnAddCompareColumn.TabIndex = 19;
            this.btnAddCompareColumn.Text = "添加比较列";
            this.btnAddCompareColumn.UseVisualStyleBackColor = true;
            this.btnAddCompareColumn.Click += new System.EventHandler(this.btnAddCompareColumn_Click);
            // 
            // btnAddKeyColumn
            // 
            this.btnAddKeyColumn.Location = new System.Drawing.Point(44, 182);
            this.btnAddKeyColumn.Name = "btnAddKeyColumn";
            this.btnAddKeyColumn.Size = new System.Drawing.Size(75, 21);
            this.btnAddKeyColumn.TabIndex = 18;
            this.btnAddKeyColumn.Text = "添加键列";
            this.btnAddKeyColumn.UseVisualStyleBackColor = true;
            this.btnAddKeyColumn.Click += new System.EventHandler(this.btnAddKeyColumn_Click);
            // 
            // lblCompareColumns
            // 
            this.lblCompareColumns.AutoSize = true;
            this.lblCompareColumns.Location = new System.Drawing.Point(286, 103);
            this.lblCompareColumns.Name = "lblCompareColumns";
            this.lblCompareColumns.Size = new System.Drawing.Size(65, 12);
            this.lblCompareColumns.TabIndex = 16;
            this.lblCompareColumns.Text = "比较列映射";
            // 
            // lblKeyColumns
            // 
            this.lblKeyColumns.AutoSize = true;
            this.lblKeyColumns.Location = new System.Drawing.Point(286, 11);
            this.lblKeyColumns.Name = "lblKeyColumns";
            this.lblKeyColumns.Size = new System.Drawing.Size(53, 12);
            this.lblKeyColumns.TabIndex = 15;
            this.lblKeyColumns.Text = "键列映射";
            // 
            // lblNewColumns
            // 
            this.lblNewColumns.AutoSize = true;
            this.lblNewColumns.Location = new System.Drawing.Point(142, 11);
            this.lblNewColumns.Name = "lblNewColumns";
            this.lblNewColumns.Size = new System.Drawing.Size(77, 12);
            this.lblNewColumns.TabIndex = 14;
            this.lblNewColumns.Text = "新数据列名称";
            // 
            // lblOldColumns
            // 
            this.lblOldColumns.AutoSize = true;
            this.lblOldColumns.Location = new System.Drawing.Point(11, 11);
            this.lblOldColumns.Name = "lblOldColumns";
            this.lblOldColumns.Size = new System.Drawing.Size(77, 12);
            this.lblOldColumns.TabIndex = 13;
            this.lblOldColumns.Text = "旧数据列名称";
            // 
            // lstCompareColumns
            // 
            this.lstCompareColumns.FormattingEnabled = true;
            this.lstCompareColumns.ItemHeight = 12;
            this.lstCompareColumns.Location = new System.Drawing.Point(289, 118);
            this.lstCompareColumns.Name = "lstCompareColumns";
            this.lstCompareColumns.Size = new System.Drawing.Size(250, 64);
            this.lstCompareColumns.TabIndex = 12;
            this.lstCompareColumns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstCompareColumns_MouseDoubleClick);
            // 
            // lstKeyColumns
            // 
            this.lstKeyColumns.FormattingEnabled = true;
            this.lstKeyColumns.ItemHeight = 12;
            this.lstKeyColumns.Location = new System.Drawing.Point(289, 26);
            this.lstKeyColumns.Name = "lstKeyColumns";
            this.lstKeyColumns.Size = new System.Drawing.Size(250, 64);
            this.lstKeyColumns.TabIndex = 11;
            this.lstKeyColumns.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstKeyColumns_MouseDoubleClick);
            // 
            // lstNewColumns
            // 
            this.lstNewColumns.FormattingEnabled = true;
            this.lstNewColumns.ItemHeight = 12;
            this.lstNewColumns.Location = new System.Drawing.Point(145, 26);
            this.lstNewColumns.Name = "lstNewColumns";
            this.lstNewColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstNewColumns.Size = new System.Drawing.Size(120, 148);
            this.lstNewColumns.TabIndex = 10;
            // 
            // lstOldColumns
            // 
            this.lstOldColumns.FormattingEnabled = true;
            this.lstOldColumns.ItemHeight = 12;
            this.lstOldColumns.Location = new System.Drawing.Point(14, 26);
            this.lstOldColumns.Name = "lstOldColumns";
            this.lstOldColumns.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstOldColumns.Size = new System.Drawing.Size(120, 148);
            this.lstOldColumns.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1006, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "列映射配置";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 656);
            this.Controls.Add(this.resultDisplayPanel);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.comparisonSettingsPanel);
            this.Controls.Add(this.columnMappingPanel);
            this.Controls.Add(this.fileSelectionPanel);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "办公助手 - Excel对比工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.fileSelectionPanel.ResumeLayout(false);
            this.fileSelectionPanel.PerformLayout();
            this.comparisonSettingsPanel.ResumeLayout(false);
            this.comparisonSettingsPanel.PerformLayout();
            this.resultDisplayPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvComparisonResults)).EndInit();
            this.actionPanel.ResumeLayout(false);
            this.columnMappingPanel.ResumeLayout(false);
            this.splitContainerColumnMapping.Panel1.ResumeLayout(false);
            this.splitContainerColumnMapping.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerColumnMapping)).EndInit();
            this.splitContainerColumnMapping.ResumeLayout(false);
            this.splitContainerFilePreviews.Panel1.ResumeLayout(false);
            this.splitContainerFilePreviews.Panel1.PerformLayout();
            this.splitContainerFilePreviews.Panel2.ResumeLayout(false);
            this.splitContainerFilePreviews.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFilePreviews)).EndInit();
            this.splitContainerFilePreviews.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldFilePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNewFilePreview)).EndInit();
            this.panelMappingSettings.ResumeLayout(false);
            this.panelMappingSettings.PerformLayout();
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
        private System.Windows.Forms.Label lblOldFileSummary;
        private System.Windows.Forms.Label lblNewFileSummary;
        private System.Windows.Forms.Label lblComparisonMode;
        private System.Windows.Forms.ComboBox cmbComparisonMode;
        private System.Windows.Forms.CheckBox chkIgnoreSpaces;
        private System.Windows.Forms.CheckBox chkCaseSensitive;
        private System.Windows.Forms.ComboBox cmbOldWorksheet;
        private System.Windows.Forms.ComboBox cmbNewWorksheet;
        private System.Windows.Forms.Label lblOldWorksheet;
        private System.Windows.Forms.Label lblNewWorksheet;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.DataGridView dgvComparisonResults;
        private System.Windows.Forms.Button btnClearResults;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.Button btnStartComparison;
        private System.Windows.Forms.SplitContainer splitContainerColumnMapping;
        private System.Windows.Forms.SplitContainer splitContainerFilePreviews;
        private System.Windows.Forms.Panel panelMappingSettings;
        private System.Windows.Forms.CheckBox chkAutoLoadFiles;
        private System.Windows.Forms.CheckBox chkShowSameData;
    }
}