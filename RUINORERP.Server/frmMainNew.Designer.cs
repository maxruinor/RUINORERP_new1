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
            menuStripMain = new System.Windows.Forms.MenuStrip();
            systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            startServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            stopServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            reloadConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            managementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            userManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            cacheManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            workflowManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            blacklistManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            systemConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            registrationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dataViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            closeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            helpDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMain = new System.Windows.Forms.ToolStrip();
            toolStripButtonStartServer = new System.Windows.Forms.ToolStripButton();
            toolStripButtonStopServer = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripButtonRefreshData = new System.Windows.Forms.ToolStripButton();
            toolStripButtonUserManagement = new System.Windows.Forms.ToolStripButton();
            toolStripButtonCacheManagement = new System.Windows.Forms.ToolStripButton();
            toolStripButtonWorkflowTest = new System.Windows.Forms.ToolStripButton();
            toolStripButtonSystemConfig = new System.Windows.Forms.ToolStripButton();
            statusStripMain = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabelServerStatus = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelConnectionCount = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            panelNavigation = new System.Windows.Forms.Panel();
            buttonDataViewer = new System.Windows.Forms.Button();
            buttonSystemConfig = new System.Windows.Forms.Button();
            buttonBlacklist = new System.Windows.Forms.Button();
            buttonWorkflow = new System.Windows.Forms.Button();
            buttonCacheManage = new System.Windows.Forms.Button();
            buttonUserList = new System.Windows.Forms.Button();
            buttonServerMonitor = new System.Windows.Forms.Button();
            tabControlMain = new System.Windows.Forms.TabControl();
            tabPageServerMonitor = new System.Windows.Forms.TabPage();
            richTextBoxLog = new System.Windows.Forms.RichTextBox();
            splitterLog = new System.Windows.Forms.Splitter();
            menuStripMain.SuspendLayout();
            toolStripMain.SuspendLayout();
            statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            panelNavigation.SuspendLayout();
            tabControlMain.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripMain
            // 
            menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { systemToolStripMenuItem, managementToolStripMenuItem, configurationToolStripMenuItem, windowToolStripMenuItem, helpToolStripMenuItem });
            menuStripMain.Location = new System.Drawing.Point(0, 0);
            menuStripMain.Name = "menuStripMain";
            menuStripMain.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            menuStripMain.Size = new System.Drawing.Size(1167, 27);
            menuStripMain.TabIndex = 0;
            menuStripMain.Text = "menuStrip1";
            // 
            // systemToolStripMenuItem
            // 
            systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { startServerToolStripMenuItem, stopServerToolStripMenuItem, reloadConfigToolStripMenuItem, exitToolStripMenuItem });
            systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            systemToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            systemToolStripMenuItem.Text = "系统(&S)";
            // 
            // startServerToolStripMenuItem
            // 
            startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            startServerToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            startServerToolStripMenuItem.Text = "启动服务";
            startServerToolStripMenuItem.Click += new System.EventHandler(this.startServerToolStripMenuItem_Click);
            // 
            // stopServerToolStripMenuItem
            // 
            stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            stopServerToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            stopServerToolStripMenuItem.Text = "关闭服务";
            stopServerToolStripMenuItem.Click += new System.EventHandler(this.stopServerToolStripMenuItem_Click);
            // 
            // reloadConfigToolStripMenuItem
            // 
            reloadConfigToolStripMenuItem.Name = "reloadConfigToolStripMenuItem";
            reloadConfigToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            reloadConfigToolStripMenuItem.Text = "重新加载配置";
            reloadConfigToolStripMenuItem.Click += new System.EventHandler(this.reloadConfigToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            exitToolStripMenuItem.Text = "退出";
            exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // managementToolStripMenuItem
            // 
            managementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { userManagementToolStripMenuItem, cacheManagementToolStripMenuItem, workflowManagementToolStripMenuItem, blacklistManagementToolStripMenuItem });
            managementToolStripMenuItem.Name = "managementToolStripMenuItem";
            managementToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            managementToolStripMenuItem.Text = "管理(&M)";
            // 
            // userManagementToolStripMenuItem
            // 
            userManagementToolStripMenuItem.Name = "userManagementToolStripMenuItem";
            userManagementToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            userManagementToolStripMenuItem.Text = "用户管理";
            userManagementToolStripMenuItem.Click += new System.EventHandler(this.userManagementToolStripMenuItem_Click);
            // 
            // cacheManagementToolStripMenuItem
            // 
            cacheManagementToolStripMenuItem.Name = "cacheManagementToolStripMenuItem";
            cacheManagementToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            cacheManagementToolStripMenuItem.Text = "缓存管理";
            cacheManagementToolStripMenuItem.Click += new System.EventHandler(this.cacheManagementToolStripMenuItem_Click);
            // 
            // workflowManagementToolStripMenuItem
            // 
            workflowManagementToolStripMenuItem.Name = "workflowManagementToolStripMenuItem";
            workflowManagementToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            workflowManagementToolStripMenuItem.Text = "工作流管理";
            workflowManagementToolStripMenuItem.Click += new System.EventHandler(this.workflowManagementToolStripMenuItem_Click);
            // 
            // blacklistManagementToolStripMenuItem
            // 
            blacklistManagementToolStripMenuItem.Name = "blacklistManagementToolStripMenuItem";
            blacklistManagementToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            blacklistManagementToolStripMenuItem.Text = "黑名单管理";
            blacklistManagementToolStripMenuItem.Click += new System.EventHandler(this.blacklistManagementToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { systemConfigToolStripMenuItem, registrationInfoToolStripMenuItem, dataViewerToolStripMenuItem });
            configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            configurationToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            configurationToolStripMenuItem.Text = "配置(&C)";
            // 
            // systemConfigToolStripMenuItem
            // 
            systemConfigToolStripMenuItem.Name = "systemConfigToolStripMenuItem";
            systemConfigToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            systemConfigToolStripMenuItem.Text = "系统配置";
            systemConfigToolStripMenuItem.Click += new System.EventHandler(this.systemConfigToolStripMenuItem_Click);
            // 
            // registrationInfoToolStripMenuItem
            // 
            registrationInfoToolStripMenuItem.Name = "registrationInfoToolStripMenuItem";
            registrationInfoToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            registrationInfoToolStripMenuItem.Text = "注册信息";
            registrationInfoToolStripMenuItem.Click += new System.EventHandler(this.registrationInfoToolStripMenuItem_Click);
            // 
            // dataViewerToolStripMenuItem
            // 
            dataViewerToolStripMenuItem.Name = "dataViewerToolStripMenuItem";
            dataViewerToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            dataViewerToolStripMenuItem.Text = "数据查看";
            dataViewerToolStripMenuItem.Click += new System.EventHandler(this.dataViewerToolStripMenuItem_Click);
            // 
            // windowToolStripMenuItem
            // 
            windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { closeAllToolStripMenuItem });
            windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            windowToolStripMenuItem.Size = new System.Drawing.Size(64, 21);
            windowToolStripMenuItem.Text = "窗口(&W)";
            // 
            // closeAllToolStripMenuItem
            // 
            closeAllToolStripMenuItem.Name = "closeAllToolStripMenuItem";
            closeAllToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            closeAllToolStripMenuItem.Text = "关闭所有";
            closeAllToolStripMenuItem.Click += new System.EventHandler(this.closeAllToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { aboutToolStripMenuItem, helpDocumentationToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new System.Drawing.Size(61, 21);
            helpToolStripMenuItem.Text = "帮助(&H)";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            aboutToolStripMenuItem.Text = "关于";
            aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // helpDocumentationToolStripMenuItem
            // 
            helpDocumentationToolStripMenuItem.Name = "helpDocumentationToolStripMenuItem";
            helpDocumentationToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            helpDocumentationToolStripMenuItem.Text = "帮助文档";
            helpDocumentationToolStripMenuItem.Click += new System.EventHandler(this.helpDocumentationToolStripMenuItem_Click);
            // 
            // toolStripMain
            // 
            toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButtonStartServer, toolStripButtonStopServer, toolStripSeparator1, toolStripButtonRefreshData, toolStripButtonUserManagement, toolStripButtonCacheManagement, toolStripButtonWorkflowTest, toolStripButtonSystemConfig });
            toolStripMain.Location = new System.Drawing.Point(0, 27);
            toolStripMain.Name = "toolStripMain";
            toolStripMain.Size = new System.Drawing.Size(1167, 25);
            toolStripMain.TabIndex = 1;
            toolStripMain.Text = "toolStrip1";
            // 
            // toolStripButtonStartServer
            // 
            toolStripButtonStartServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonStartServer.Name = "toolStripButtonStartServer";
            toolStripButtonStartServer.Size = new System.Drawing.Size(72, 22);
            toolStripButtonStartServer.Text = "启动服务器";
            toolStripButtonStartServer.Click += new System.EventHandler(this.toolStripButtonStartServer_Click);
            // 
            // toolStripButtonStopServer
            // 
            toolStripButtonStopServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonStopServer.Name = "toolStripButtonStopServer";
            toolStripButtonStopServer.Size = new System.Drawing.Size(72, 22);
            toolStripButtonStopServer.Text = "停止服务器";
            toolStripButtonStopServer.Click += new System.EventHandler(this.toolStripButtonStopServer_Click);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonRefreshData
            // 
            toolStripButtonRefreshData.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonRefreshData.Name = "toolStripButtonRefreshData";
            toolStripButtonRefreshData.Size = new System.Drawing.Size(72, 22);
            toolStripButtonRefreshData.Text = "刷新数据";
            toolStripButtonRefreshData.Click += new System.EventHandler(this.toolStripButtonRefreshData_Click);
            // 
            // toolStripButtonUserManagement
            // 
            toolStripButtonUserManagement.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonUserManagement.Name = "toolStripButtonUserManagement";
            toolStripButtonUserManagement.Size = new System.Drawing.Size(72, 22);
            toolStripButtonUserManagement.Text = "用户管理";
            toolStripButtonUserManagement.Click += new System.EventHandler(this.toolStripButtonUserManagement_Click);
            // 
            // toolStripButtonCacheManagement
            // 
            toolStripButtonCacheManagement.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonCacheManagement.Name = "toolStripButtonCacheManagement";
            toolStripButtonCacheManagement.Size = new System.Drawing.Size(72, 22);
            toolStripButtonCacheManagement.Text = "缓存管理";
            toolStripButtonCacheManagement.Click += new System.EventHandler(this.toolStripButtonCacheManagement_Click);
            // 
            // toolStripButtonWorkflowTest
            // 
            toolStripButtonWorkflowTest.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonWorkflowTest.Name = "toolStripButtonWorkflowTest";
            toolStripButtonWorkflowTest.Size = new System.Drawing.Size(72, 22);
            toolStripButtonWorkflowTest.Text = "工作流测试";
            toolStripButtonWorkflowTest.Click += new System.EventHandler(this.toolStripButtonWorkflowTest_Click);
            // 
            // toolStripButtonSystemConfig
            // 
            toolStripButtonSystemConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButtonSystemConfig.Name = "toolStripButtonSystemConfig";
            toolStripButtonSystemConfig.Size = new System.Drawing.Size(72, 22);
            toolStripButtonSystemConfig.Text = "系统配置";
            toolStripButtonSystemConfig.Click += new System.EventHandler(this.toolStripButtonSystemConfig_Click);
            // 
            // statusStripMain
            // 
            statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabelServerStatus, toolStripStatusLabelConnectionCount, toolStripStatusLabelMessage });
            statusStripMain.Location = new System.Drawing.Point(0, 862);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStripMain.Size = new System.Drawing.Size(1167, 22);
            statusStripMain.TabIndex = 2;
            statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabelServerStatus
            // 
            toolStripStatusLabelServerStatus.Name = "toolStripStatusLabelServerStatus";
            toolStripStatusLabelServerStatus.Size = new System.Drawing.Size(68, 17);
            toolStripStatusLabelServerStatus.Text = "服务状态：";
            // 
            // toolStripStatusLabelConnectionCount
            // 
            toolStripStatusLabelConnectionCount.Name = "toolStripStatusLabelConnectionCount";
            toolStripStatusLabelConnectionCount.Size = new System.Drawing.Size(63, 17);
            toolStripStatusLabelConnectionCount.Text = "连接数：0";
            // 
            // toolStripStatusLabelMessage
            // 
            toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
            toolStripStatusLabelMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 52);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(panelNavigation);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(tabControlMain);
            splitContainerMain.Size = new System.Drawing.Size(1167, 810);
            splitContainerMain.SplitterDistance = 175;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 3;
            // 
            // panelNavigation
            // 
            panelNavigation.Controls.Add(buttonDataViewer);
            panelNavigation.Controls.Add(buttonSystemConfig);
            panelNavigation.Controls.Add(buttonBlacklist);
            panelNavigation.Controls.Add(buttonWorkflow);
            panelNavigation.Controls.Add(buttonCacheManage);
            panelNavigation.Controls.Add(buttonUserList);
            panelNavigation.Controls.Add(buttonServerMonitor);
            panelNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
            panelNavigation.Location = new System.Drawing.Point(0, 0);
            panelNavigation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panelNavigation.Name = "panelNavigation";
            panelNavigation.Size = new System.Drawing.Size(175, 810);
            panelNavigation.TabIndex = 0;
            // 
            // buttonDataViewer
            // 
            buttonDataViewer.Location = new System.Drawing.Point(14, 425);
            buttonDataViewer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonDataViewer.Name = "buttonDataViewer";
            buttonDataViewer.Size = new System.Drawing.Size(140, 42);
            buttonDataViewer.TabIndex = 6;
            buttonDataViewer.Text = "数据查看";
            buttonDataViewer.UseVisualStyleBackColor = true;
            buttonDataViewer.Click += new System.EventHandler(this.buttonDataViewer_Click);
            // 
            // buttonSystemConfig
            // 
            buttonSystemConfig.Location = new System.Drawing.Point(14, 354);
            buttonSystemConfig.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonSystemConfig.Name = "buttonSystemConfig";
            buttonSystemConfig.Size = new System.Drawing.Size(140, 42);
            buttonSystemConfig.TabIndex = 5;
            buttonSystemConfig.Text = "系统配置";
            buttonSystemConfig.UseVisualStyleBackColor = true;
            buttonSystemConfig.Click += new System.EventHandler(this.buttonSystemConfig_Click);
            // 
            // buttonBlacklist
            // 
            buttonBlacklist.Location = new System.Drawing.Point(14, 283);
            buttonBlacklist.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonBlacklist.Name = "buttonBlacklist";
            buttonBlacklist.Size = new System.Drawing.Size(140, 42);
            buttonBlacklist.TabIndex = 4;
            buttonBlacklist.Text = "黑名单管理";
            buttonBlacklist.UseVisualStyleBackColor = true;
            buttonBlacklist.Click += new System.EventHandler(this.buttonBlacklist_Click);
            // 
            // buttonWorkflow
            // 
            buttonWorkflow.Location = new System.Drawing.Point(14, 212);
            buttonWorkflow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonWorkflow.Name = "buttonWorkflow";
            buttonWorkflow.Size = new System.Drawing.Size(140, 42);
            buttonWorkflow.TabIndex = 3;
            buttonWorkflow.Text = "工作流管理";
            buttonWorkflow.UseVisualStyleBackColor = true;
            buttonWorkflow.Click += new System.EventHandler(this.buttonWorkflow_Click);
            // 
            // buttonCacheManage
            // 
            buttonCacheManage.Location = new System.Drawing.Point(14, 142);
            buttonCacheManage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonCacheManage.Name = "buttonCacheManage";
            buttonCacheManage.Size = new System.Drawing.Size(140, 42);
            buttonCacheManage.TabIndex = 2;
            buttonCacheManage.Text = "缓存管理";
            buttonCacheManage.UseVisualStyleBackColor = true;
            buttonCacheManage.Click += new System.EventHandler(this.buttonCacheManage_Click);
            // 
            // buttonUserList
            // 
            buttonUserList.Location = new System.Drawing.Point(14, 71);
            buttonUserList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonUserList.Name = "buttonUserList";
            buttonUserList.Size = new System.Drawing.Size(140, 42);
            buttonUserList.TabIndex = 1;
            buttonUserList.Text = "用户管理";
            buttonUserList.UseVisualStyleBackColor = true;
            buttonUserList.Click += new System.EventHandler(this.buttonUserList_Click);
            // 
            // buttonServerMonitor
            // 
            buttonServerMonitor.Location = new System.Drawing.Point(14, 14);
            buttonServerMonitor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            buttonServerMonitor.Name = "buttonServerMonitor";
            buttonServerMonitor.Size = new System.Drawing.Size(140, 42);
            buttonServerMonitor.TabIndex = 0;
            buttonServerMonitor.Text = "服务器监控";
            buttonServerMonitor.UseVisualStyleBackColor = true;
            buttonServerMonitor.Click += new System.EventHandler(this.buttonServerMonitor_Click);
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageServerMonitor);
            tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlMain.Location = new System.Drawing.Point(0, 0);
            tabControlMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new System.Drawing.Size(987, 810);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageServerMonitor
            // 
            tabPageServerMonitor.Location = new System.Drawing.Point(4, 26);
            tabPageServerMonitor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPageServerMonitor.Name = "tabPageServerMonitor";
            tabPageServerMonitor.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            tabPageServerMonitor.Size = new System.Drawing.Size(979, 780);
            tabPageServerMonitor.TabIndex = 0;
            tabPageServerMonitor.Text = "服务器监控";
            tabPageServerMonitor.UseVisualStyleBackColor = true;
            // 
            // richTextBoxLog
            // 
            richTextBoxLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            richTextBoxLog.Location = new System.Drawing.Point(0, 891);
            richTextBoxLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            richTextBoxLog.Name = "richTextBoxLog";
            richTextBoxLog.Size = new System.Drawing.Size(1167, 140);
            richTextBoxLog.TabIndex = 4;
            richTextBoxLog.Text = "";
            // 
            // splitterLog
            // 
            splitterLog.BackColor = System.Drawing.Color.DarkGray;
            splitterLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            splitterLog.Location = new System.Drawing.Point(0, 884);
            splitterLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splitterLog.Name = "splitterLog";
            splitterLog.Size = new System.Drawing.Size(1167, 7);
            splitterLog.TabIndex = 5;
            splitterLog.TabStop = false;
            // 
            // frmMainNew
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1167, 1031);
            Controls.Add(splitContainerMain);
            Controls.Add(statusStripMain);
            Controls.Add(toolStripMain);
            Controls.Add(menuStripMain);
            Controls.Add(splitterLog);
            Controls.Add(richTextBoxLog);
            MainMenuStrip = menuStripMain;
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "frmMainNew";
            Text = "RUINOR ERP 服务器管理端";
            FormClosing += frmMainNew_FormClosing;
            Load += frmMainNew_Load;
            menuStripMain.ResumeLayout(false);
            menuStripMain.PerformLayout();
            toolStripMain.ResumeLayout(false);
            toolStripMain.PerformLayout();
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            panelNavigation.ResumeLayout(false);
            tabControlMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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