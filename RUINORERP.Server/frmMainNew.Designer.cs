namespace RUINORERP.Server
{
    partial class frmMainNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMainNew));
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cacheManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workflowManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blacklistManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonStartServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStopServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRefreshData = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonUserManagement = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCacheManagement = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonWorkflowTest = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSystemConfig = new System.Windows.Forms.ToolStripButton();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelConnectionCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.buttonDataViewer = new System.Windows.Forms.Button();
            this.buttonSystemConfig = new System.Windows.Forms.Button();
            this.buttonBlacklist = new System.Windows.Forms.Button();
            this.buttonWorkflow = new System.Windows.Forms.Button();
            this.buttonCacheManage = new System.Windows.Forms.Button();
            this.buttonUserList = new System.Windows.Forms.Button();
            this.buttonServerMonitor = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageServerMonitor = new System.Windows.Forms.TabPage();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.splitterLog = new System.Windows.Forms.Splitter();
            this.menuStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem,
            this.managementToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.windowToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1000, 25);
            this.menuStripMain.TabIndex = 0;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServerToolStripMenuItem,
            this.stopServerToolStripMenuItem,
            this.reloadConfigToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.systemToolStripMenuItem.Text = "系统(&S)";
            // 
            // startServerToolStripMenuItem
            // 
            this.startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            this.startServerToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.startServerToolStripMenuItem.Text = "启动服务";
            // 
            // stopServerToolStripMenuItem
            // 
            this.stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            this.stopServerToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.stopServerToolStripMenuItem.Text = "关闭服务";
            // 
            // reloadConfigToolStripMenuItem
            // 
            this.reloadConfigToolStripMenuItem.Name = "reloadConfigToolStripMenuItem";
            this.reloadConfigToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.reloadConfigToolStripMenuItem.Text = "重新加载配置";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.exitToolStripMenuItem.Text = "退出";
            // 
            // managementToolStripMenuItem
            // 
            this.managementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManagementToolStripMenuItem,
            this.cacheManagementToolStripMenuItem,
            this.workflowManagementToolStripMenuItem,
            this.blacklistManagementToolStripMenuItem});
            this.managementToolStripMenuItem.Name = "managementToolStripMenuItem";
            this.managementToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.managementToolStripMenuItem.Text = "管理(&M)";
            // 
            // userManagementToolStripMenuItem
            // 
            this.userManagementToolStripMenuItem.Name = "userManagementToolStripMenuItem";
            this.userManagementToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.userManagementToolStripMenuItem.Text = "用户管理";
            // 
            // cacheManagementToolStripMenuItem
            // 
            this.cacheManagementToolStripMenuItem.Name = "cacheManagementToolStripMenuItem";
            this.cacheManagementToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.cacheManagementToolStripMenuItem.Text = "缓存管理";
            // 
            // workflowManagementToolStripMenuItem
            // 
            this.workflowManagementToolStripMenuItem.Name = "workflowManagementToolStripMenuItem";
            this.workflowManagementToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.workflowManagementToolStripMenuItem.Text = "工作流管理";
            // 
            // blacklistManagementToolStripMenuItem
            // 
            this.blacklistManagementToolStripMenuItem.Name = "blacklistManagementToolStripMenuItem";
            this.blacklistManagementToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.blacklistManagementToolStripMenuItem.Text = "黑名单管理";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemConfigToolStripMenuItem,
            this.registrationInfoToolStripMenuItem,
            this.dataViewerToolStripMenuItem});
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.configurationToolStripMenuItem.Text = "配置(&C)";
            // 
            // systemConfigToolStripMenuItem
            // 
            this.systemConfigToolStripMenuItem.Name = "systemConfigToolStripMenuItem";
            this.systemConfigToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.systemConfigToolStripMenuItem.Text = "系统配置";
            // 
            // registrationInfoToolStripMenuItem
            // 
            this.registrationInfoToolStripMenuItem.Name = "registrationInfoToolStripMenuItem";
            this.registrationInfoToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.registrationInfoToolStripMenuItem.Text = "注册信息";
            // 
            // dataViewerToolStripMenuItem
            // 
            this.dataViewerToolStripMenuItem.Name = "dataViewerToolStripMenuItem";
            this.dataViewerToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.dataViewerToolStripMenuItem.Text = "数据查看";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeAllToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            this.windowToolStripMenuItem.Text = "窗口(&W)";
            // 
            // closeAllToolStripMenuItem
            // 
            this.closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            this.closeAllToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.closeAllToolStripMenuItem.Text = "关闭所有";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.helpDocumentationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            this.helpToolStripMenuItem.Text = "帮助(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.aboutToolStripMenuItem.Text = "关于";
            // 
            // helpDocumentationToolStripMenuItem
            // 
            this.helpDocumentationToolStripMenuItem.Name = "helpDocumentationToolStripMenuItem";
            this.helpDocumentationToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.helpDocumentationToolStripMenuItem.Text = "帮助文档";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonStartServer,
            this.toolStripButtonStopServer,
            this.toolStripSeparator1,
            this.toolStripButtonRefreshData,
            this.toolStripButtonUserManagement,
            this.toolStripButtonCacheManagement,
            this.toolStripButtonWorkflowTest,
            this.toolStripButtonSystemConfig});
            this.toolStripMain.Location = new System.Drawing.Point(0, 25);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1000, 25);
            this.toolStripMain.TabIndex = 1;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonStartServer
            // 
            this.toolStripButtonStartServer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStartServer.Image")));
            this.toolStripButtonStartServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStartServer.Name = "toolStripButtonStartServer";
            this.toolStripButtonStartServer.Size = new System.Drawing.Size(88, 22);
            this.toolStripButtonStartServer.Text = "启动服务器";
            // 
            // toolStripButtonStopServer
            // 
            this.toolStripButtonStopServer.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStopServer.Image")));
            this.toolStripButtonStopServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStopServer.Name = "toolStripButtonStopServer";
            this.toolStripButtonStopServer.Size = new System.Drawing.Size(88, 22);
            this.toolStripButtonStopServer.Text = "停止服务器";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRefreshData
            // 
            this.toolStripButtonRefreshData.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefreshData.Image")));
            this.toolStripButtonRefreshData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefreshData.Name = "toolStripButtonRefreshData";
            this.toolStripButtonRefreshData.Size = new System.Drawing.Size(76, 22);
            this.toolStripButtonRefreshData.Text = "刷新数据";
            // 
            // toolStripButtonUserManagement
            // 
            this.toolStripButtonUserManagement.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUserManagement.Image")));
            this.toolStripButtonUserManagement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUserManagement.Name = "toolStripButtonUserManagement";
            this.toolStripButtonUserManagement.Size = new System.Drawing.Size(100, 22);
            this.toolStripButtonUserManagement.Text = "用户管理";
            // 
            // toolStripButtonCacheManagement
            // 
            this.toolStripButtonCacheManagement.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCacheManagement.Image")));
            this.toolStripButtonCacheManagement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCacheManagement.Name = "toolStripButtonCacheManagement";
            this.toolStripButtonCacheManagement.Size = new System.Drawing.Size(88, 22);
            this.toolStripButtonCacheManagement.Text = "缓存管理";
            // 
            // toolStripButtonWorkflowTest
            // 
            this.toolStripButtonWorkflowTest.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonWorkflowTest.Image")));
            this.toolStripButtonWorkflowTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonWorkflowTest.Name = "toolStripButtonWorkflowTest";
            this.toolStripButtonWorkflowTest.Size = new System.Drawing.Size(100, 22);
            this.toolStripButtonWorkflowTest.Text = "工作流测试";
            // 
            // toolStripButtonSystemConfig
            // 
            this.toolStripButtonSystemConfig.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSystemConfig.Image")));
            this.toolStripButtonSystemConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSystemConfig.Name = "toolStripButtonSystemConfig";
            this.toolStripButtonSystemConfig.Size = new System.Drawing.Size(88, 22);
            this.toolStripButtonSystemConfig.Text = "系统配置";
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelServerStatus,
            this.toolStripStatusLabelConnectionCount,
            this.toolStripStatusLabelMessage});
            this.statusStripMain.Location = new System.Drawing.Point(0, 658);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1000, 22);
            this.statusStripMain.TabIndex = 2;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelServerStatus
            // 
            this.toolStripStatusLabelServerStatus.Name = "toolStripStatusLabelServerStatus";
            this.toolStripStatusLabelServerStatus.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabelServerStatus.Text = "服务状态：";
            // 
            // toolStripStatusLabelConnectionCount
            // 
            this.toolStripStatusLabelConnectionCount.Name = "toolStripStatusLabelConnectionCount";
            this.toolStripStatusLabelConnectionCount.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabelConnectionCount.Text = "连接数：0";
            // 
            // toolStripStatusLabelMessage
            // 
            this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 50);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.panelNavigation);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControlMain);
            this.splitContainerMain.Size = new System.Drawing.Size(1000, 608);
            this.splitContainerMain.SplitterDistance = 150;
            this.splitContainerMain.TabIndex = 3;
            // 
            // panelNavigation
            // 
            this.panelNavigation.Controls.Add(this.buttonDataViewer);
            this.panelNavigation.Controls.Add(this.buttonSystemConfig);
            this.panelNavigation.Controls.Add(this.buttonBlacklist);
            this.panelNavigation.Controls.Add(this.buttonWorkflow);
            this.panelNavigation.Controls.Add(this.buttonCacheManage);
            this.panelNavigation.Controls.Add(this.buttonUserList);
            this.panelNavigation.Controls.Add(this.buttonServerMonitor);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelNavigation.Location = new System.Drawing.Point(0, 0);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(150, 608);
            this.panelNavigation.TabIndex = 0;
            // 
            // buttonDataViewer
            // 
            this.buttonDataViewer.Location = new System.Drawing.Point(12, 300);
            this.buttonDataViewer.Name = "buttonDataViewer";
            this.buttonDataViewer.Size = new System.Drawing.Size(120, 30);
            this.buttonDataViewer.TabIndex = 6;
            this.buttonDataViewer.Text = "数据查看";
            this.buttonDataViewer.UseVisualStyleBackColor = true;
            // 
            // buttonSystemConfig
            // 
            this.buttonSystemConfig.Location = new System.Drawing.Point(12, 250);
            this.buttonSystemConfig.Name = "buttonSystemConfig";
            this.buttonSystemConfig.Size = new System.Drawing.Size(120, 30);
            this.buttonSystemConfig.TabIndex = 5;
            this.buttonSystemConfig.Text = "系统配置";
            this.buttonSystemConfig.UseVisualStyleBackColor = true;
            // 
            // buttonBlacklist
            // 
            this.buttonBlacklist.Location = new System.Drawing.Point(12, 200);
            this.buttonBlacklist.Name = "buttonBlacklist";
            this.buttonBlacklist.Size = new System.Drawing.Size(120, 30);
            this.buttonBlacklist.TabIndex = 4;
            this.buttonBlacklist.Text = "黑名单管理";
            this.buttonBlacklist.UseVisualStyleBackColor = true;
            // 
            // buttonWorkflow
            // 
            this.buttonWorkflow.Location = new System.Drawing.Point(12, 150);
            this.buttonWorkflow.Name = "buttonWorkflow";
            this.buttonWorkflow.Size = new System.Drawing.Size(120, 30);
            this.buttonWorkflow.TabIndex = 3;
            this.buttonWorkflow.Text = "工作流管理";
            this.buttonWorkflow.UseVisualStyleBackColor = true;
            // 
            // buttonCacheManage
            // 
            this.buttonCacheManage.Location = new System.Drawing.Point(12, 100);
            this.buttonCacheManage.Name = "buttonCacheManage";
            this.buttonCacheManage.Size = new System.Drawing.Size(120, 30);
            this.buttonCacheManage.TabIndex = 2;
            this.buttonCacheManage.Text = "缓存管理";
            this.buttonCacheManage.UseVisualStyleBackColor = true;
            // 
            // buttonUserList
            // 
            this.buttonUserList.Location = new System.Drawing.Point(12, 50);
            this.buttonUserList.Name = "buttonUserList";
            this.buttonUserList.Size = new System.Drawing.Size(120, 30);
            this.buttonUserList.TabIndex = 1;
            this.buttonUserList.Text = "用户管理";
            this.buttonUserList.UseVisualStyleBackColor = true;
            // 
            // buttonServerMonitor
            // 
            this.buttonServerMonitor.Location = new System.Drawing.Point(12, 10);
            this.buttonServerMonitor.Name = "buttonServerMonitor";
            this.buttonServerMonitor.Size = new System.Drawing.Size(120, 30);
            this.buttonServerMonitor.TabIndex = 0;
            this.buttonServerMonitor.Text = "服务器监控";
            this.buttonServerMonitor.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageServerMonitor);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(846, 608);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageServerMonitor
            // 
            this.tabPageServerMonitor.Location = new System.Drawing.Point(4, 22);
            this.tabPageServerMonitor.Name = "tabPageServerMonitor";
            this.tabPageServerMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServerMonitor.Size = new System.Drawing.Size(838, 582);
            this.tabPageServerMonitor.TabIndex = 0;
            this.tabPageServerMonitor.Text = "服务器监控";
            this.tabPageServerMonitor.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBoxLog.Location = new System.Drawing.Point(0, 680);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.Size = new System.Drawing.Size(1000, 100);
            this.richTextBoxLog.TabIndex = 4;
            this.richTextBoxLog.Text = "";
            // 
            // splitterLog
            // 
            this.splitterLog.BackColor = System.Drawing.Color.DarkGray;
            this.splitterLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterLog.Location = new System.Drawing.Point(0, 675);
            this.splitterLog.Name = "splitterLog";
            this.splitterLog.Size = new System.Drawing.Size(1000, 5);
            this.splitterLog.TabIndex = 5;
            this.splitterLog.TabStop = false;
            // 
            // frmMainNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 780);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Controls.Add(this.splitterLog);
            this.Controls.Add(this.richTextBoxLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "frmMainNew";
            this.Text = "RUINOR ERP 服务器管理端";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cacheManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workflowManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blacklistManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registrationInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpDocumentationToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolStripButtonStartServer;
        private System.Windows.Forms.ToolStripButton toolStripButtonStopServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefreshData;
        private System.Windows.Forms.ToolStripButton toolStripButtonUserManagement;
        private System.Windows.Forms.ToolStripButton toolStripButtonCacheManagement;
        private System.Windows.Forms.ToolStripButton toolStripButtonWorkflowTest;
        private System.Windows.Forms.ToolStripButton toolStripButtonSystemConfig;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServerStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnectionCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Button buttonDataViewer;
        private System.Windows.Forms.Button buttonSystemConfig;
        private System.Windows.Forms.Button buttonBlacklist;
        private System.Windows.Forms.Button buttonWorkflow;
        private System.Windows.Forms.Button buttonCacheManage;
        private System.Windows.Forms.Button buttonUserList;
        private System.Windows.Forms.Button buttonServerMonitor;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageServerMonitor;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Splitter splitterLog;
    }
}