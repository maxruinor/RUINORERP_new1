namespace RUINORERP.TopServer
{
    partial class frmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            系统ToolStripMenuItem = new ToolStripMenuItem();
            启动服务器ToolStripMenuItem = new ToolStripMenuItem();
            停止服务器ToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            退出ToolStripMenuItem = new ToolStripMenuItem();
            查看ToolStripMenuItem = new ToolStripMenuItem();
            服务器实例ToolStripMenuItem = new ToolStripMenuItem();
            授权管理ToolStripMenuItem = new ToolStripMenuItem();
            用户状态ToolStripMenuItem = new ToolStripMenuItem();
            配置管理ToolStripMenuItem = new ToolStripMenuItem();
            帮助ToolStripMenuItem = new ToolStripMenuItem();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            tsStatus = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tsOnlineCount = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            tsTotalCount = new ToolStripStatusLabel();
            tabControl1 = new TabControl();
            tpServerInstances = new TabPage();
            dgvServerInstances = new DataGridView();
            tpAuthorization = new TabPage();
            dgvAuthorization = new DataGridView();
            tpUsers = new TabPage();
            dgvUsers = new DataGridView();
            tpConfigurations = new TabPage();
            grpConfigActions = new GroupBox();
            btnDeleteConfig = new Button();
            btnUpdateConfig = new Button();
            btnAddConfig = new Button();
            dgvConfigurations = new DataGridView();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tpServerInstances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvServerInstances).BeginInit();
            tpAuthorization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAuthorization).BeginInit();
            tpUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            tpConfigurations.SuspendLayout();
            grpConfigActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvConfigurations).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { 系统ToolStripMenuItem, 查看ToolStripMenuItem, 帮助ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 3, 0, 3);
            menuStrip1.Size = new Size(933, 27);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // 系统ToolStripMenuItem
            // 
            系统ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 启动服务器ToolStripMenuItem, 停止服务器ToolStripMenuItem, toolStripSeparator1, 退出ToolStripMenuItem });
            系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            系统ToolStripMenuItem.Size = new Size(44, 21);
            系统ToolStripMenuItem.Text = "系统";
            // 
            // 启动服务器ToolStripMenuItem
            // 
            启动服务器ToolStripMenuItem.Name = "启动服务器ToolStripMenuItem";
            启动服务器ToolStripMenuItem.Size = new Size(136, 22);
            启动服务器ToolStripMenuItem.Text = "启动服务器";
            启动服务器ToolStripMenuItem.Click += 启动服务器ToolStripMenuItem_Click;
            // 
            // 停止服务器ToolStripMenuItem
            // 
            停止服务器ToolStripMenuItem.Name = "停止服务器ToolStripMenuItem";
            停止服务器ToolStripMenuItem.Size = new Size(136, 22);
            停止服务器ToolStripMenuItem.Text = "停止服务器";
            停止服务器ToolStripMenuItem.Click += 停止服务器ToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(133, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            退出ToolStripMenuItem.Size = new Size(136, 22);
            退出ToolStripMenuItem.Text = "退出";
            退出ToolStripMenuItem.Click += 退出ToolStripMenuItem_Click;
            // 
            // 查看ToolStripMenuItem
            // 
            查看ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 服务器实例ToolStripMenuItem, 授权管理ToolStripMenuItem, 用户状态ToolStripMenuItem, 配置管理ToolStripMenuItem });
            查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            查看ToolStripMenuItem.Size = new Size(44, 21);
            查看ToolStripMenuItem.Text = "查看";
            // 
            // 服务器实例ToolStripMenuItem
            // 
            服务器实例ToolStripMenuItem.Name = "服务器实例ToolStripMenuItem";
            服务器实例ToolStripMenuItem.Size = new Size(136, 22);
            服务器实例ToolStripMenuItem.Text = "服务器实例";
            服务器实例ToolStripMenuItem.Click += 服务器实例ToolStripMenuItem_Click;
            // 
            // 授权管理ToolStripMenuItem
            // 
            授权管理ToolStripMenuItem.Name = "授权管理ToolStripMenuItem";
            授权管理ToolStripMenuItem.Size = new Size(136, 22);
            授权管理ToolStripMenuItem.Text = "授权管理";
            授权管理ToolStripMenuItem.Click += 授权管理ToolStripMenuItem_Click;
            // 
            // 用户状态ToolStripMenuItem
            // 
            用户状态ToolStripMenuItem.Name = "用户状态ToolStripMenuItem";
            用户状态ToolStripMenuItem.Size = new Size(136, 22);
            用户状态ToolStripMenuItem.Text = "用户状态";
            用户状态ToolStripMenuItem.Click += 用户状态ToolStripMenuItem_Click;
            // 
            // 配置管理ToolStripMenuItem
            // 
            配置管理ToolStripMenuItem.Name = "配置管理ToolStripMenuItem";
            配置管理ToolStripMenuItem.Size = new Size(136, 22);
            配置管理ToolStripMenuItem.Text = "配置管理";
            配置管理ToolStripMenuItem.Click += 配置管理ToolStripMenuItem_Click;
            // 
            // 帮助ToolStripMenuItem
            // 
            帮助ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 关于ToolStripMenuItem });
            帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            帮助ToolStripMenuItem.Size = new Size(44, 21);
            帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(100, 22);
            关于ToolStripMenuItem.Text = "关于";
            关于ToolStripMenuItem.Click += 关于ToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { tsStatus, toolStripStatusLabel1, tsOnlineCount, toolStripStatusLabel3, tsTotalCount });
            statusStrip1.Location = new Point(0, 616);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(933, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            tsStatus.Name = "tsStatus";
            tsStatus.Size = new Size(32, 17);
            tsStatus.Text = "就绪";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(63, 17);
            toolStripStatusLabel1.Text = "在线实例: ";
            // 
            // tsOnlineCount
            // 
            tsOnlineCount.Name = "tsOnlineCount";
            tsOnlineCount.Size = new Size(15, 17);
            tsOnlineCount.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(63, 17);
            toolStripStatusLabel3.Text = "总实例数: ";
            // 
            // tsTotalCount
            // 
            tsTotalCount.Name = "tsTotalCount";
            tsTotalCount.Size = new Size(15, 17);
            tsTotalCount.Text = "0";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tpServerInstances);
            tabControl1.Controls.Add(tpAuthorization);
            tabControl1.Controls.Add(tpUsers);
            tabControl1.Controls.Add(tpConfigurations);
            tabControl1.Location = new Point(0, 38);
            tabControl1.Margin = new Padding(4, 4, 4, 4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(933, 568);
            tabControl1.TabIndex = 2;
            // 
            // tpServerInstances
            // 
            tpServerInstances.Controls.Add(dgvServerInstances);
            tpServerInstances.Location = new Point(4, 26);
            tpServerInstances.Margin = new Padding(4, 4, 4, 4);
            tpServerInstances.Name = "tpServerInstances";
            tpServerInstances.Padding = new Padding(4, 4, 4, 4);
            tpServerInstances.Size = new Size(925, 538);
            tpServerInstances.TabIndex = 0;
            tpServerInstances.Text = "服务器实例";
            tpServerInstances.UseVisualStyleBackColor = true;
            // 
            // dgvServerInstances
            // 
            dgvServerInstances.AllowUserToAddRows = false;
            dgvServerInstances.AllowUserToDeleteRows = false;
            dgvServerInstances.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvServerInstances.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvServerInstances.Location = new Point(7, 8);
            dgvServerInstances.Margin = new Padding(4, 4, 4, 4);
            dgvServerInstances.Name = "dgvServerInstances";
            dgvServerInstances.ReadOnly = true;
            dgvServerInstances.Size = new Size(910, 514);
            dgvServerInstances.TabIndex = 0;
            // 
            // tpAuthorization
            // 
            tpAuthorization.Controls.Add(dgvAuthorization);
            tpAuthorization.Location = new Point(4, 26);
            tpAuthorization.Margin = new Padding(4, 4, 4, 4);
            tpAuthorization.Name = "tpAuthorization";
            tpAuthorization.Padding = new Padding(4, 4, 4, 4);
            tpAuthorization.Size = new Size(925, 538);
            tpAuthorization.TabIndex = 1;
            tpAuthorization.Text = "授权管理";
            tpAuthorization.UseVisualStyleBackColor = true;
            // 
            // dgvAuthorization
            // 
            dgvAuthorization.AllowUserToAddRows = false;
            dgvAuthorization.AllowUserToDeleteRows = false;
            dgvAuthorization.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAuthorization.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAuthorization.Location = new Point(7, 8);
            dgvAuthorization.Margin = new Padding(4, 4, 4, 4);
            dgvAuthorization.Name = "dgvAuthorization";
            dgvAuthorization.ReadOnly = true;
            dgvAuthorization.Size = new Size(910, 514);
            dgvAuthorization.TabIndex = 0;
            // 
            // tpUsers
            // 
            tpUsers.Controls.Add(dgvUsers);
            tpUsers.Location = new Point(4, 26);
            tpUsers.Margin = new Padding(4, 4, 4, 4);
            tpUsers.Name = "tpUsers";
            tpUsers.Padding = new Padding(4, 4, 4, 4);
            tpUsers.Size = new Size(925, 538);
            tpUsers.TabIndex = 2;
            tpUsers.Text = "用户状态";
            tpUsers.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Location = new Point(7, 8);
            dgvUsers.Margin = new Padding(4, 4, 4, 4);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.Size = new Size(910, 514);
            dgvUsers.TabIndex = 0;
            // 
            // tpConfigurations
            // 
            tpConfigurations.Controls.Add(grpConfigActions);
            tpConfigurations.Controls.Add(dgvConfigurations);
            tpConfigurations.Location = new Point(4, 26);
            tpConfigurations.Margin = new Padding(4, 4, 4, 4);
            tpConfigurations.Name = "tpConfigurations";
            tpConfigurations.Padding = new Padding(4, 4, 4, 4);
            tpConfigurations.Size = new Size(925, 538);
            tpConfigurations.TabIndex = 3;
            tpConfigurations.Text = "配置管理";
            tpConfigurations.UseVisualStyleBackColor = true;
            // 
            // grpConfigActions
            // 
            grpConfigActions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grpConfigActions.Controls.Add(btnDeleteConfig);
            grpConfigActions.Controls.Add(btnUpdateConfig);
            grpConfigActions.Controls.Add(btnAddConfig);
            grpConfigActions.Location = new Point(7, 475);
            grpConfigActions.Margin = new Padding(4, 4, 4, 4);
            grpConfigActions.Name = "grpConfigActions";
            grpConfigActions.Padding = new Padding(4, 4, 4, 4);
            grpConfigActions.Size = new Size(910, 76);
            grpConfigActions.TabIndex = 1;
            grpConfigActions.TabStop = false;
            grpConfigActions.Text = "配置操作";
            // 
            // btnDeleteConfig
            // 
            btnDeleteConfig.Location = new Point(282, 27);
            btnDeleteConfig.Margin = new Padding(4, 4, 4, 4);
            btnDeleteConfig.Name = "btnDeleteConfig";
            btnDeleteConfig.Size = new Size(88, 33);
            btnDeleteConfig.TabIndex = 2;
            btnDeleteConfig.Text = "删除";
            btnDeleteConfig.UseVisualStyleBackColor = true;
            // 
            // btnUpdateConfig
            // 
            btnUpdateConfig.Location = new Point(145, 27);
            btnUpdateConfig.Margin = new Padding(4, 4, 4, 4);
            btnUpdateConfig.Name = "btnUpdateConfig";
            btnUpdateConfig.Size = new Size(88, 33);
            btnUpdateConfig.TabIndex = 1;
            btnUpdateConfig.Text = "更新";
            btnUpdateConfig.UseVisualStyleBackColor = true;
            // 
            // btnAddConfig
            // 
            btnAddConfig.Location = new Point(7, 27);
            btnAddConfig.Margin = new Padding(4, 4, 4, 4);
            btnAddConfig.Name = "btnAddConfig";
            btnAddConfig.Size = new Size(88, 33);
            btnAddConfig.TabIndex = 0;
            btnAddConfig.Text = "添加";
            btnAddConfig.UseVisualStyleBackColor = true;
            // 
            // dgvConfigurations
            // 
            dgvConfigurations.AllowUserToAddRows = false;
            dgvConfigurations.AllowUserToDeleteRows = false;
            dgvConfigurations.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvConfigurations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvConfigurations.Location = new Point(7, 8);
            dgvConfigurations.Margin = new Padding(4, 4, 4, 4);
            dgvConfigurations.Name = "dgvConfigurations";
            dgvConfigurations.ReadOnly = true;
            dgvConfigurations.Size = new Size(910, 458);
            dgvConfigurations.TabIndex = 0;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 638);
            Controls.Add(tabControl1);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 4, 4, 4);
            Name = "frmMain";
            Text = "RUINOR ERP 顶级管理服务器";
            FormClosing += frmMain_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tpServerInstances.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvServerInstances).EndInit();
            tpAuthorization.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAuthorization).EndInit();
            tpUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            tpConfigurations.ResumeLayout(false);
            grpConfigActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvConfigurations).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 启动服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 停止服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 服务器实例ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 授权管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户状态ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 配置管理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsOnlineCount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsTotalCount;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpServerInstances;
        private System.Windows.Forms.DataGridView dgvServerInstances;
        private System.Windows.Forms.TabPage tpAuthorization;
        private System.Windows.Forms.DataGridView dgvAuthorization;
        private System.Windows.Forms.TabPage tpUsers;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.TabPage tpConfigurations;
        private System.Windows.Forms.GroupBox grpConfigActions;
        private System.Windows.Forms.Button btnDeleteConfig;
        private System.Windows.Forms.Button btnUpdateConfig;
        private System.Windows.Forms.Button btnAddConfig;
        private System.Windows.Forms.DataGridView dgvConfigurations;
    }
}