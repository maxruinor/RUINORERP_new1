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
            this.kryptonPanelMain = new Krypton.Toolkit.KryptonPanel();
            this.tabControlMain = new TabControl();
            this.tabPageData = new  TabPage();
            this.kryptonSplitContainer1 = new Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonGroupBoxConfig = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanelConfig = new Krypton.Toolkit.KryptonPanel();
            this.kcmbEntityType = new Krypton.Toolkit.KryptonComboBox();
            this.ktbSearchEntity = new Krypton.Toolkit.KryptonTextBox();
            this.klblSearch = new Krypton.Toolkit.KryptonLabel();
            this.klblEntityType = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBoxPreview = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanelPreview = new Krypton.Toolkit.KryptonPanel();
            this.dgvDataPreview = new System.Windows.Forms.DataGridView();
            this.kryptonPanelExecuteButtons = new Krypton.Toolkit.KryptonPanel();
            this.kbtnDeleteSelected = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectNone = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectInvert = new Krypton.Toolkit.KryptonButton();
            this.kbtnSelectAll = new Krypton.Toolkit.KryptonButton();
            this.kbtnRefresh = new Krypton.Toolkit.KryptonButton();
            this.kbtnTestExecute = new Krypton.Toolkit.KryptonButton();
            this.kbtnPreview = new Krypton.Toolkit.KryptonButton();
            this.tabPageImages = new TabPage();
            this.tabPageLog = new TabPage();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.ctxMenuLog = new System.Windows.Forms.ContextMenuStrip();
            this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelMain)).BeginInit();
            this.kryptonPanelMain.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig.Panel)).BeginInit();
            this.kryptonGroupBoxConfig.Panel.SuspendLayout();
            this.kryptonGroupBoxConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelConfig)).BeginInit();
            this.kryptonPanelConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbEntityType)).BeginInit();
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
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1000, 700);
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabPages.AddRange(new TabPage[] {
            this.tabPageData,
            this.tabPageImages,
            this.tabPageLog});
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.kryptonSplitContainer1);
            this.tabPageData.Location = new System.Drawing.Point(5, 24);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageData.Size = new System.Drawing.Size(990, 671);
            this.tabPageData.Text = "数据清理";
            // 
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(5, 5);
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
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(980, 661);
            this.kryptonSplitContainer1.SplitterDistance = 80;
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
            this.kryptonGroupBoxConfig.Panel.Controls.Add(this.kryptonPanelConfig);
            this.kryptonGroupBoxConfig.Size = new System.Drawing.Size(1000, 80);
            this.kryptonGroupBoxConfig.TabIndex = 0;
            this.kryptonGroupBoxConfig.Values.Heading = "清理配置";
            // 
            // kryptonPanelConfig
            // 
            this.kryptonPanelConfig.Controls.Add(this.kcmbEntityType);
            this.kryptonPanelConfig.Controls.Add(this.ktbSearchEntity);
            this.kryptonPanelConfig.Controls.Add(this.klblSearch);
            this.kryptonPanelConfig.Controls.Add(this.klblEntityType);
            this.kryptonPanelConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanelConfig.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanelConfig.Name = "kryptonPanelConfig";
            this.kryptonPanelConfig.Size = new System.Drawing.Size(996, 56);
            this.kryptonPanelConfig.TabIndex = 0;
            // 
            // kcmbEntityType
            // 
            this.kcmbEntityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcmbEntityType.DropDownWidth = 400;
            this.kcmbEntityType.IntegralHeight = false;
            this.kcmbEntityType.Location = new System.Drawing.Point(90, 15);
            this.kcmbEntityType.MaxDropDownItems = 20;
            this.kcmbEntityType.Name = "kcmbEntityType";
            this.kcmbEntityType.Size = new System.Drawing.Size(405, 21);
            this.kcmbEntityType.TabIndex = 1;
            this.kcmbEntityType.SelectedIndexChanged += new System.EventHandler(this.KcmbEntityType_SelectedIndexChanged);
            // 
            // ktbSearchEntity
            // 
            this.ktbSearchEntity.Location = new System.Drawing.Point(590, 15);
            this.ktbSearchEntity.Name = "ktbSearchEntity";
            this.ktbSearchEntity.Size = new System.Drawing.Size(300, 23);
            this.ktbSearchEntity.TabIndex = 3;
            this.ktbSearchEntity.TextChanged += new System.EventHandler(this.KtbSearchEntity_TextChanged);
            // 
            // klblSearch
            // 
            this.klblSearch.Location = new System.Drawing.Point(520, 18);
            this.klblSearch.Name = "klblSearch";
            this.klblSearch.Size = new System.Drawing.Size(65, 20);
            this.klblSearch.TabIndex = 2;
            this.klblSearch.Values.Text = "快速搜索:";
            // 
            // klblEntityType
            // 
            this.klblEntityType.Location = new System.Drawing.Point(20, 18);
            this.klblEntityType.Name = "klblEntityType";
            this.klblEntityType.Size = new System.Drawing.Size(65, 20);
            this.klblEntityType.TabIndex = 0;
            this.klblEntityType.Values.Text = "实体类型:";
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
            this.kryptonGroupBoxPreview.Size = new System.Drawing.Size(1000, 615);
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
            this.kryptonPanelPreview.Size = new System.Drawing.Size(996, 591);
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
            this.dgvDataPreview.Size = new System.Drawing.Size(996, 552);
            this.dgvDataPreview.TabIndex = 1;
            // 
            // kryptonPanelExecuteButtons
            // 
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnDeleteSelected);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectNone);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectInvert);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnSelectAll);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnRefresh);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnTestExecute);
            this.kryptonPanelExecuteButtons.Controls.Add(this.kbtnPreview);
            this.kryptonPanelExecuteButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanelExecuteButtons.Location = new System.Drawing.Point(0, 552);
            this.kryptonPanelExecuteButtons.Name = "kryptonPanelExecuteButtons";
            this.kryptonPanelExecuteButtons.Size = new System.Drawing.Size(996, 39);
            this.kryptonPanelExecuteButtons.TabIndex = 0;
            // 
            // kbtnDeleteSelected
            // 
            this.kbtnDeleteSelected.Location = new System.Drawing.Point(839, 6);
            this.kbtnDeleteSelected.Name = "kbtnDeleteSelected";
            this.kbtnDeleteSelected.Size = new System.Drawing.Size(90, 28);
            this.kbtnDeleteSelected.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.kbtnDeleteSelected.TabIndex = 7;
            this.kbtnDeleteSelected.Values.Text = "删除选中";
            this.kbtnDeleteSelected.Click += new System.EventHandler(this.KbtnDeleteSelected_Click);
            // 
            // kbtnSelectNone
            // 
            this.kbtnSelectNone.Location = new System.Drawing.Point(513, 6);
            this.kbtnSelectNone.Name = "kbtnSelectNone";
            this.kbtnSelectNone.Size = new System.Drawing.Size(90, 28);
            this.kbtnSelectNone.TabIndex = 6;
            this.kbtnSelectNone.Values.Text = "取消选择";
            this.kbtnSelectNone.Click += new System.EventHandler(this.KbtnSelectNone_Click);
            // 
            // kbtnSelectInvert
            // 
            this.kbtnSelectInvert.Location = new System.Drawing.Point(395, 6);
            this.kbtnSelectInvert.Name = "kbtnSelectInvert";
            this.kbtnSelectInvert.Size = new System.Drawing.Size(90, 28);
            this.kbtnSelectInvert.TabIndex = 5;
            this.kbtnSelectInvert.Values.Text = "反选";
            this.kbtnSelectInvert.Click += new System.EventHandler(this.KbtnSelectInvert_Click);
            // 
            // kbtnSelectAll
            // 
            this.kbtnSelectAll.Location = new System.Drawing.Point(285, 6);
            this.kbtnSelectAll.Name = "kbtnSelectAll";
            this.kbtnSelectAll.Size = new System.Drawing.Size(90, 28);
            this.kbtnSelectAll.TabIndex = 4;
            this.kbtnSelectAll.Values.Text = "全选";
            this.kbtnSelectAll.Click += new System.EventHandler(this.KbtnSelectAll_Click);
            // 
            // kbtnRefresh
            // 
            this.kbtnRefresh.Location = new System.Drawing.Point(30, 6);
            this.kbtnRefresh.Name = "kbtnRefresh";
            this.kbtnRefresh.Size = new System.Drawing.Size(90, 28);
            this.kbtnRefresh.TabIndex = 3;
            this.kbtnRefresh.Values.Text = "刷新";
            this.kbtnRefresh.Click += new System.EventHandler(this.KbtnRefresh_Click);
            // 
            // kbtnTestExecute
            // 
            this.kbtnTestExecute.Location = new System.Drawing.Point(669, 6);
            this.kbtnTestExecute.Name = "kbtnTestExecute";
            this.kbtnTestExecute.Size = new System.Drawing.Size(90, 28);
            this.kbtnTestExecute.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.kbtnTestExecute.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.kbtnTestExecute.TabIndex = 1;
            this.kbtnTestExecute.Values.Text = "测试执行";
            this.kbtnTestExecute.Click += new System.EventHandler(this.KbtnTestExecute_Click);
            // 
            // kbtnPreview
            // 
            this.kbtnPreview.Location = new System.Drawing.Point(140, 6);
            this.kbtnPreview.Name = "kbtnPreview";
            this.kbtnPreview.Size = new System.Drawing.Size(90, 28);
            this.kbtnPreview.TabIndex = 0;
            this.kbtnPreview.Values.Text = "数据预览";
            this.kbtnPreview.Click += new System.EventHandler(this.KbtnPreview_Click);
            // 
            // tabPageImages
            // 
            this.tabPageImages.Location = new System.Drawing.Point(5, 24);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageImages.Size = new System.Drawing.Size(990, 671);
            this.tabPageImages.Text = "图片管理";
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.rtbLog);
            this.tabPageLog.Location = new System.Drawing.Point(5, 24);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(5);
            this.tabPageLog.Size = new System.Drawing.Size(990, 671);
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
            this.rtbLog.Size = new System.Drawing.Size(980, 661);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // ctxMenuLog
            // 
            this.ctxMenuLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearLog});
            this.ctxMenuLog.Name = "ctxMenuLog";
            this.ctxMenuLog.Size = new System.Drawing.Size(101, 26);
            // 
            // tsmiClearLog
            // 
            this.tsmiClearLog.Name = "tsmiClearLog";
            this.tsmiClearLog.Size = new System.Drawing.Size(100, 22);
            this.tsmiClearLog.Text = "清除日志";
            this.tsmiClearLog.Click += new System.EventHandler(this.TsmiClearLog_Click);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig.Panel)).EndInit();
            this.kryptonGroupBoxConfig.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBoxConfig)).EndInit();
            this.kryptonGroupBoxConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelConfig)).EndInit();
            this.kryptonPanelConfig.ResumeLayout(false);
            this.kryptonPanelConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kcmbEntityType)).EndInit();
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
        private Krypton.Toolkit.KryptonPanel kryptonPanelConfig;
        private Krypton.Toolkit.KryptonLabel klblEntityType;
        private Krypton.Toolkit.KryptonComboBox kcmbEntityType;
        private Krypton.Toolkit.KryptonTextBox ktbSearchEntity;
        private Krypton.Toolkit.KryptonLabel klblSearch;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBoxPreview;
        private Krypton.Toolkit.KryptonPanel kryptonPanelPreview;
        private System.Windows.Forms.DataGridView dgvDataPreview;
        private Krypton.Toolkit.KryptonPanel kryptonPanelExecuteButtons;
        private Krypton.Toolkit.KryptonButton kbtnDeleteSelected;
        private Krypton.Toolkit.KryptonButton kbtnSelectNone;
        private Krypton.Toolkit.KryptonButton kbtnSelectInvert;
        private Krypton.Toolkit.KryptonButton kbtnSelectAll;
        private Krypton.Toolkit.KryptonButton kbtnRefresh;
        private Krypton.Toolkit.KryptonButton kbtnTestExecute;
        private Krypton.Toolkit.KryptonButton kbtnPreview;
        private System.Windows.Forms.ContextMenuStrip ctxMenuLog;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearLog;
    }
}
