using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    partial class UCBasicDataCleanup
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
            this.components = new System.ComponentModel.Container();
            this.kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.splitContainerOutSite = new System.Windows.Forms.SplitContainer();
            this.treeViewTableList = new System.Windows.Forms.TreeView();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonGroupBoxConfig = new Krypton.Toolkit.KryptonGroupBox();
            this.klblDeleteMode = new Krypton.Toolkit.KryptonLabel();
            this.kcmbDeleteMode = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonGroupBoxPreview = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanelPreview = new Krypton.Toolkit.KryptonPanel();
            this.dgvDataPreview = new System.Windows.Forms.DataGridView();
            this.kryptonPanelExecuteButtons = new Krypton.Toolkit.KryptonPanel();
            this.kbtnDeleteSelected = new Krypton.Toolkit.KryptonButton();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnTestExecute = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectNone = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectInvert = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectAll = new Krypton.Toolkit.KryptonButton();
            this.kbtnRefresh = new Krypton.Toolkit.KryptonButton();
            this.kbtnPreview = new Krypton.Toolkit.KryptonButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabPageImages = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.ctxMenuLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.kcmbEntityType = new Krypton.Toolkit.KryptonComboBox();
            this.klblEntityType = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOutSite)).BeginInit();
            this.splitContainerOutSite.Panel1.SuspendLayout();
            this.splitContainerOutSite.Panel2.SuspendLayout();
            this.splitContainerOutSite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig.Panel)).BeginInit();
            this.kryptonGroupBoxConfig.Panel.SuspendLayout();
            this.kryptonGroupBoxConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDeleteMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxPreview.Panel)).BeginInit();
            this.kryptonGroupBoxPreview.Panel.SuspendLayout();
            this.kryptonGroupBoxPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelPreview)).BeginInit();
            this.kryptonPanelPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelExecuteButtons)).BeginInit();
            this.kryptonPanelExecuteButtons.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.ctxMenuLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbEntityType)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanelMain
            // 
            this.kryptonPanelMain.Controls.Add(this.tabControlMain);
            this.kryptonPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelMain.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelMain.Name = "kryptonPanelMain";
            this.kryptonPanelMain.Size = new System.Drawing.Size(1000, 700);
            this.kryptonPanelMain.TabIndex = 0;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageData);
            this.tabControlMain.Controls.Add(this.tabPageImages);
            this.tabControlMain.Controls.Add(this.tabPageLog);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1000, 700);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.splitContainerOutSite);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageData.Size = new System.Drawing.Size(992, 674);
            this.tabPageData.TabIndex = 0;
            this.tabPageData.Text = "数据清理";
            // 
            // splitContainerOutSite
            // 
            this.splitContainerOutSite.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOutSite.Location = new System.Drawing.Point(5, 5);
            this.splitContainerOutSite.Name = "splitContainerOutSite";
            // 
            // splitContainerOutSite.Panel1
            // 
            this.splitContainerOutSite.Panel1.Controls.Add(this.treeViewTableList);
            // 
            // splitContainerOutSite.Panel2
            // 
            this.splitContainerOutSite.Panel2.Controls.Add(this.kryptonSplitContainer1);
            this.splitContainerOutSite.Size = new System.Drawing.Size(982, 664);
            this.splitContainerOutSite.SplitterDistance = 185;
            this.splitContainerOutSite.TabIndex = 1;
            // 
            // treeViewTableList
            // 
            this.treeViewTableList.CheckBoxes = true;
            this.treeViewTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewTableList.Location = new System.Drawing.Point(0, 0);
            this.treeViewTableList.Name = "treeViewTableList";
            this.treeViewTableList.Size = new System.Drawing.Size(185, 664);
            this.treeViewTableList.TabIndex = 0;
            this.treeViewTableList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTableList_AfterCheck);
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonGroupBoxConfig);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.kryptonGroupBoxPreview);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(793, 664);
            this.kryptonSplitContainer1.SplitterDistance = 79;
            this.kryptonSplitContainer1.TabIndex = 0;
            // 
            // kryptonGroupBoxConfig
            // 
            this.kryptonGroupBoxConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBoxConfig.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBoxConfig.Name = "kryptonGroupBoxConfig";
            // 
            // kryptonGroupBoxConfig.Panel
            // 
            this.kryptonGroupBoxConfig.Panel.Controls.Add(this.klblDeleteMode);
            this.kryptonGroupBoxConfig.Panel.Controls.Add(this.kcmbDeleteMode);
            this.kryptonGroupBoxConfig.Size = new System.Drawing.Size(793, 79);
            this.kryptonGroupBoxConfig.TabIndex = 0;
            this.kryptonGroupBoxConfig.Values.Heading = "清理配置";
            // 
            // klblDeleteMode
            // 
            this.klblDeleteMode.Location = new System.Drawing.Point(10, 8);
            this.klblDeleteMode.Name = "klblDeleteMode";
            this.klblDeleteMode.Size = new System.Drawing.Size(75, 20);
            this.klblDeleteMode.TabIndex = 0;
            this.klblDeleteMode.Values.Text = "删除方式：";
            // 
            // kcmbDeleteMode
            // 
            this.kcmbDeleteMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbDeleteMode.DropDownWidth = 180;
            this.kcmbDeleteMode.IntegralHeight = false;
            this.kcmbDeleteMode.Location = new System.Drawing.Point(90, 8);
            this.kcmbDeleteMode.MaxDropDownItems = 10;
            this.kcmbDeleteMode.Name = "kcmbDeleteMode";
            this.kcmbDeleteMode.Size = new System.Drawing.Size(160, 21);
            this.kcmbDeleteMode.TabIndex = 1;
            // 
            // kryptonGroupBoxPreview
            // 
            this.kryptonGroupBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBoxPreview.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBoxPreview.Name = "kryptonGroupBoxPreview";
            // 
            // kryptonGroupBoxPreview.Panel
            // 
            this.kryptonGroupBoxPreview.Panel.Controls.Add(this.kryptonPanelPreview);
            this.kryptonGroupBoxPreview.Size = new System.Drawing.Size(793, 580);
            this.kryptonGroupBoxPreview.TabIndex = 2;
            this.kryptonGroupBoxPreview.Values.Heading = "数据预览";
            // 
            // kryptonPanelPreview
            // 
            this.kryptonPanelPreview.Controls.Add(this.dgvDataPreview);
            this.kryptonPanelPreview.Controls.Add(this.kryptonPanelExecuteButtons);
            this.kryptonPanelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelPreview.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelPreview.Name = "kryptonPanelPreview";
            this.kryptonPanelPreview.Size = new System.Drawing.Size(789, 556);
            this.kryptonPanelPreview.TabIndex = 0;
            // 
            // dgvDataPreview
            // 
            this.dgvDataPreview.AllowUserToAddRows = false;
            this.dgvDataPreview.AllowUserToDeleteRows = false;
            this.dgvDataPreview.BackgroundColor = System.Drawing.Color.White;
            this.dgvDataPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDataPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDataPreview.Location = new System.Drawing.Point(0, 0);
            this.dgvDataPreview.Name = "dgvDataPreview";
            this.dgvDataPreview.RowTemplate.Height = 23;
            this.dgvDataPreview.Size = new System.Drawing.Size(789, 517);
            this.dgvDataPreview.TabIndex = 1;
            // 
            // kryptonPanelExecuteButtons
            // 
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnDeleteSelected);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnCancel);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnTestExecute);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectNone);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectInvert);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectAll);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnRefresh);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnPreview);
            this.kryptonPanelExecuteButtons.Controls.Add(this.progressBar);
            this.kryptonPanelExecuteButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanelExecuteButtons.Location = new System.Drawing.Point(0, 517);
            this.kryptonPanelExecuteButtons.Name = "kryptonPanelExecuteButtons";
            this.kryptonPanelExecuteButtons.Size = new System.Drawing.Size(789, 39);
            this.kryptonPanelExecuteButtons.TabIndex = 0;
            // 
            // kbtnDeleteSelected
            // 
            this.kbtnDeleteSelected.Location = new System.Drawing.Point(440, 6);
            this.kbtnDeleteSelected.Name = "kbtnDeleteSelected";
            this.kbtnDeleteSelected.Size = new System.Drawing.Size(90, 28);
            this.kbtnDeleteSelected.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kbtnDeleteSelected.TabIndex = 7;
            this.kbtnDeleteSelected.Values.Text = "删除选中";
            this.kbtnDeleteSelected.Click += new System.EventHandler(this.KbtnDeleteSelected_Click);
            // 
            // kbtnCancel
            // 
            this.kbtnCancel.Enabled = false;
            this.kbtnCancel.Location = new System.Drawing.Point(540, 6);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(90, 28);
            this.kbtnCancel.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.kbtnCancel.TabIndex = 8;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.KbtnCancel_Click);
            // 
            // kbtnTestExecute
            // 
            this.kbtnTestExecute.Location = new System.Drawing.Point(333, 6);
            this.kbtnTestExecute.Name = "kbtnTestExecute";
            this.kbtnTestExecute.Size = new System.Drawing.Size(80, 28);
            this.kbtnTestExecute.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.kbtnTestExecute.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kbtnTestExecute.TabIndex = 1;
            this.kbtnTestExecute.Values.Text = "测试";
            this.kbtnTestExecute.Click += new System.EventHandler(this.KbtnTestExecute_Click);
            // 
            // kbtnSelectNone
            // 
            this.kbtnSelectNone.Location = new System.Drawing.Point(220, 6);
            this.kbtnSelectNone.Name = "kbtnSelectNone";
            this.kbtnSelectNone.Size = new System.Drawing.Size(80, 28);
            this.kbtnSelectNone.TabIndex = 6;
            this.kbtnSelectNone.Values.Text = "取消选择";
            this.kbtnSelectNone.Click += new System.EventHandler(this.KbtnSelectNone_Click);
            // 
            // kbtnSelectInvert
            // 
            this.kbtnSelectInvert.Location = new System.Drawing.Point(140, 6);
            this.kbtnSelectInvert.Name = "kbtnSelectInvert";
            this.kbtnSelectInvert.Size = new System.Drawing.Size(80, 28);
            this.kbtnSelectInvert.TabIndex = 5;
            this.kbtnSelectInvert.Values.Text = "反选";
            this.kbtnSelectInvert.Click += new System.EventHandler(this.KbtnSelectInvert_Click);
            // 
            // kbtnSelectAll
            // 
            this.kbtnSelectAll.Location = new System.Drawing.Point(60, 6);
            this.kbtnSelectAll.Name = "kbtnSelectAll";
            this.kbtnSelectAll.Size = new System.Drawing.Size(80, 28);
            this.kbtnSelectAll.TabIndex = 4;
            this.kbtnSelectAll.Values.Text = "全选";
            this.kbtnSelectAll.Click += new System.EventHandler(this.KbtnSelectAll_Click);
            // 
            // kbtnRefresh
            // 
            this.kbtnRefresh.Location = new System.Drawing.Point(10, 6);
            this.kbtnRefresh.Name = "kbtnRefresh";
            this.kbtnRefresh.Size = new System.Drawing.Size(40, 28);
            this.kbtnRefresh.TabIndex = 3;
            this.kbtnRefresh.Values.Text = "刷新";
            this.kbtnRefresh.Click += new System.EventHandler(this.KbtnRefresh_Click);
            // 
            // kbtnPreview
            // 
            this.kbtnPreview.Location = new System.Drawing.Point(676, 6);
            this.kbtnPreview.Name = "kbtnPreview";
            this.kbtnPreview.Size = new System.Drawing.Size(80, 28);
            this.kbtnPreview.TabIndex = 0;
            this.kbtnPreview.Values.Text = "预览";
            this.kbtnPreview.Click += new System.EventHandler(this.KbtnPreview_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(770, 10);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(150, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 9;
            this.progressBar.Visible = false;
            // 
            // tabPageImages
            // 
            this.tabPageImages.Location = new System.Drawing.Point(4, 22);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageImages.Size = new System.Drawing.Size(992, 674);
            this.tabPageImages.TabIndex = 1;
            this.tabPageImages.Text = "图片管理";
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.rtbLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageLog.Size = new System.Drawing.Size(992, 674);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "运行日志";
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.Black;
            this.rtbLog.ContextMenuStrip = this.ctxMenuLog;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbLog.ForeColor = System.Drawing.Color.Lime;
            this.rtbLog.Location = new System.Drawing.Point(5, 5);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(982, 664);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // ctxMenuLog
            // 
            this.ctxMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearLog});
            this.ctxMenuLog.Name = "ctxMenuLog";
            this.ctxMenuLog.Size = new System.Drawing.Size(125, 26);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(124, 22);
            this.tsmiClearLog.Text = "清除日志";
            this.tsmiClearLog.Click += new System.EventHandler(this.TsmiClearLog_Click);
            // 
            // kcmbEntityType
            // 
            this.kcmbEntityType.DropDownWidth = 121;
            this.kcmbEntityType.IntegralHeight = false;
            this.kcmbEntityType.Location = new System.Drawing.Point(0, 0);
            this.kcmbEntityType.Name = "kcmbEntityType";
            this.kcmbEntityType.Size = new System.Drawing.Size(121, 21);
            this.kcmbEntityType.TabIndex = 0;
            // 
            // klblEntityType
            // 
            this.klblEntityType.Location = new System.Drawing.Point(0, 0);
            this.klblEntityType.Name = "klblEntityType";
            this.klblEntityType.Size = new System.Drawing.Size(90, 25);
            this.klblEntityType.TabIndex = 0;
            // 
            // UCBasicDataCleanup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanelMain);
            this.Name = "UCBasicDataCleanup";
            this.Size = new System.Drawing.Size(1000, 700);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).EndInit();
            this.kryptonPanelMain.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.splitContainerOutSite.Panel1.ResumeLayout(false);
            this.splitContainerOutSite.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOutSite)).EndInit();
            this.splitContainerOutSite.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig.Panel)).EndInit();
            this.kryptonGroupBoxConfig.Panel.ResumeLayout(false);
            this.kryptonGroupBoxConfig.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig)).EndInit();
            this.kryptonGroupBoxConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kcmbDeleteMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxPreview.Panel)).EndInit();
            this.kryptonGroupBoxPreview.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxPreview)).EndInit();
            this.kryptonGroupBoxPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelPreview)).EndInit();
            this.kryptonPanelPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDataPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelExecuteButtons)).EndInit();
            this.kryptonPanelExecuteButtons.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.ctxMenuLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kcmbEntityType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanelMain;
        private TabControl tabControlMain;
        private TabPage tabPageData;
        private TabPage tabPageImages;
        private TabPage tabPageLog;
        private System.Windows.Forms.RichTextBox rtbLog;
        private Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxConfig;
        private Krypton.Toolkit.KryptonLabel klblEntityType;
        private Krypton.Toolkit.KryptonComboBox kcmbEntityType;
        private Krypton.Toolkit.KryptonLabel klblDeleteMode;
        private Krypton.Toolkit.KryptonComboBox kcmbDeleteMode;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxPreview;
        private Krypton.Toolkit.KryptonPanel kryptonPanelPreview;
        private System.Windows.Forms.DataGridView dgvDataPreview;
        private Krypton.Toolkit.KryptonPanel kryptonPanelExecuteButtons;
        private Krypton.Toolkit.KryptonButton kbtnDeleteSelected;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnSelectNone;
        private Krypton.Toolkit.KryptonButton kbtnSelectInvert;
        private Krypton.Toolkit.KryptonButton kbtnSelectAll;
        private Krypton.Toolkit.KryptonButton kbtnRefresh;
        private Krypton.Toolkit.KryptonButton kbtnTestExecute;
        private Krypton.Toolkit.KryptonButton kbtnPreview;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearLog;
        private SplitContainer splitContainerOutSite;
        private TreeView treeViewTableList;
    }
}
