namespace RUINORERP.ManagementServer
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
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.启动服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.停止服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.服务器实例ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.授权管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.用户状态ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.配置管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsOnlineCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsTotalCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpServerInstances = new System.Windows.Forms.TabPage();
            this.dgvServerInstances = new System.Windows.Forms.DataGridView();
            this.tpAuthorization = new System.Windows.Forms.TabPage();
            this.dgvAuthorization = new System.Windows.Forms.DataGridView();
            this.tpUsers = new System.Windows.Forms.TabPage();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.tpConfigurations = new System.Windows.Forms.TabPage();
            this.grpConfigActions = new System.Windows.Forms.GroupBox();
            this.btnDeleteConfig = new System.Windows.Forms.Button();
            this.btnUpdateConfig = new System.Windows.Forms.Button();
            this.btnAddConfig = new System.Windows.Forms.Button();
            this.dgvConfigurations = new System.Windows.Forms.DataGridView();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpServerInstances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerInstances)).BeginInit();
            this.tpAuthorization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthorization)).BeginInit();
            this.tpUsers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.tpConfigurations.SuspendLayout();
            this.grpConfigActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfigurations)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统ToolStripMenuItem,
            this.查看ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.启动服务器ToolStripMenuItem,
            this.停止服务器ToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出ToolStripMenuItem});
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.系统ToolStripMenuItem.Text = "系统";
            // 
            // 启动服务器ToolStripMenuItem
            // 
            this.启动服务器ToolStripMenuItem.Name = "启动服务器ToolStripMenuItem";
            this.启动服务器ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.启动服务器ToolStripMenuItem.Text = "启动服务器";
            this.启动服务器ToolStripMenuItem.Click += new System.EventHandler(this.启动服务器ToolStripMenuItem_Click);
            // 
            // 停止服务器ToolStripMenuItem
            // 
            this.停止服务器ToolStripMenuItem.Name = "停止服务器ToolStripMenuItem";
            this.停止服务器ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.停止服务器ToolStripMenuItem.Text = "停止服务器";
            this.停止服务器ToolStripMenuItem.Click += new System.EventHandler(this.停止服务器ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 查看ToolStripMenuItem
            // 
            this.查看ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.服务器实例ToolStripMenuItem,
            this.授权管理ToolStripMenuItem,
            this.用户状态ToolStripMenuItem,
            this.配置管理ToolStripMenuItem});
            this.查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            this.查看ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.查看ToolStripMenuItem.Text = "查看";
            // 
            // 服务器实例ToolStripMenuItem
            // 
            this.服务器实例ToolStripMenuItem.Name = "服务器实例ToolStripMenuItem";
            this.服务器实例ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.服务器实例ToolStripMenuItem.Text = "服务器实例";
            this.服务器实例ToolStripMenuItem.Click += new System.EventHandler(this.服务器实例ToolStripMenuItem_Click);
            // 
            // 授权管理ToolStripMenuItem
            // 
            this.授权管理ToolStripMenuItem.Name = "授权管理ToolStripMenuItem";
            this.授权管理ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.授权管理ToolStripMenuItem.Text = "授权管理";
            this.授权管理ToolStripMenuItem.Click += new System.EventHandler(this.授权管理ToolStripMenuItem_Click);
            // 
            // 用户状态ToolStripMenuItem
            // 
            this.用户状态ToolStripMenuItem.Name = "用户状态ToolStripMenuItem";
            this.用户状态ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.用户状态ToolStripMenuItem.Text = "用户状态";
            this.用户状态ToolStripMenuItem.Click += new System.EventHandler(this.用户状态ToolStripMenuItem_Click);
            // 
            // 配置管理ToolStripMenuItem
            // 
            this.配置管理ToolStripMenuItem.Name = "配置管理ToolStripMenuItem";
            this.配置管理ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.配置管理ToolStripMenuItem.Text = "配置管理";
            this.配置管理ToolStripMenuItem.Click += new System.EventHandler(this.配置管理ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatus,
            this.toolStripStatusLabel1,
            this.tsOnlineCount,
            this.toolStripStatusLabel3,
            this.tsTotalCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsStatus
            // 
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(53, 17);
            this.tsStatus.Text = "就绪";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel1.Text = "在线实例: ";
            // 
            // tsOnlineCount
            // 
            this.tsOnlineCount.Name = "tsOnlineCount";
            this.tsOnlineCount.Size = new System.Drawing.Size(13, 17);
            this.tsOnlineCount.Text = "0";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(73, 17);
            this.toolStripStatusLabel3.Text = "总实例数: ";
            // 
            // tsTotalCount
            // 
            this.tsTotalCount.Name = "tsTotalCount";
            this.tsTotalCount.Size = new System.Drawing.Size(13, 17);
            this.tsTotalCount.Text = "0";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpServerInstances);
            this.tabControl1.Controls.Add(this.tpAuthorization);
            this.tabControl1.Controls.Add(this.tpUsers);
            this.tabControl1.Controls.Add(this.tpConfigurations);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 401);
            this.tabControl1.TabIndex = 2;
            // 
            // tpServerInstances
            // 
            this.tpServerInstances.Controls.Add(this.dgvServerInstances);
            this.tpServerInstances.Location = new System.Drawing.Point(4, 22);
            this.tpServerInstances.Name = "tpServerInstances";
            this.tpServerInstances.Padding = new System.Windows.Forms.Padding(3);
            this.tpServerInstances.Size = new System.Drawing.Size(792, 375);
            this.tpServerInstances.TabIndex = 0;
            this.tpServerInstances.Text = "服务器实例";
            this.tpServerInstances.UseVisualStyleBackColor = true;
            // 
            // dgvServerInstances
            // 
            this.dgvServerInstances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvServerInstances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServerInstances.Location = new System.Drawing.Point(6, 6);
            this.dgvServerInstances.Name = "dgvServerInstances";
            this.dgvServerInstances.Size = new System.Drawing.Size(780, 363);
            this.dgvServerInstances.TabIndex = 0;
            // 
            // tpAuthorization
            // 
            this.tpAuthorization.Controls.Add(this.dgvAuthorization);
            this.tpAuthorization.Location = new System.Drawing.Point(4, 22);
            this.tpAuthorization.Name = "tpAuthorization";
            this.tpAuthorization.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuthorization.Size = new System.Drawing.Size(792, 375);
            this.tpAuthorization.TabIndex = 1;
            this.tpAuthorization.Text = "授权管理";
            this.tpAuthorization.UseVisualStyleBackColor = true;
            // 
            // dgvAuthorization
            // 
            this.dgvAuthorization.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAuthorization.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAuthorization.Location = new System.Drawing.Point(6, 6);
            this.dgvAuthorization.Name = "dgvAuthorization";
            this.dgvAuthorization.Size = new System.Drawing.Size(780, 363);
            this.dgvAuthorization.TabIndex = 0;
            // 
            // tpUsers
            // 
            this.tpUsers.Controls.Add(this.dgvUsers);
            this.tpUsers.Location = new System.Drawing.Point(4, 22);
            this.tpUsers.Name = "tpUsers";
            this.tpUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsers.Size = new System.Drawing.Size(792, 375);
            this.tpUsers.TabIndex = 2;
            this.tpUsers.Text = "用户状态";
            this.tpUsers.UseVisualStyleBackColor = true;
            // 
            // dgvUsers
            // 
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(6, 6);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(780, 363);
            this.dgvUsers.TabIndex = 0;
            // 
            // tpConfigurations
            // 
            this.tpConfigurations.Controls.Add(this.grpConfigActions);
            this.tpConfigurations.Controls.Add(this.dgvConfigurations);
            this.tpConfigurations.Location = new System.Drawing.Point(4, 22);
            this.tpConfigurations.Name = "tpConfigurations";
            this.tpConfigurations.Padding = new System.Windows.Forms.Padding(3);
            this.tpConfigurations.Size = new System.Drawing.Size(792, 375);
            this.tpConfigurations.TabIndex = 3;
            this.tpConfigurations.Text = "配置管理";
            this.tpConfigurations.UseVisualStyleBackColor = true;
            // 
            // grpConfigActions
            // 
            this.grpConfigActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpConfigActions.Controls.Add(this.btnDeleteConfig);
            this.grpConfigActions.Controls.Add(this.btnUpdateConfig);
            this.grpConfigActions.Controls.Add(this.btnAddConfig);
            this.grpConfigActions.Location = new System.Drawing.Point(6, 335);
            this.grpConfigActions.Name = "grpConfigActions";
            this.grpConfigActions.Size = new System.Drawing.Size(780, 54);
            this.grpConfigActions.TabIndex = 1;
            this.grpConfigActions.TabStop = false;
            this.grpConfigActions.Text = "配置操作";
            // 
            // btnDeleteConfig
            // 
            this.btnDeleteConfig.Location = new System.Drawing.Point(242, 19);
            this.btnDeleteConfig.Name = "btnDeleteConfig";
            this.btnDeleteConfig.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteConfig.TabIndex = 2;
            this.btnDeleteConfig.Text = "删除";
            this.btnDeleteConfig.UseVisualStyleBackColor = true;
            // 
            // btnUpdateConfig
            // 
            this.btnUpdateConfig.Location = new System.Drawing.Point(124, 19);
            this.btnUpdateConfig.Name = "btnUpdateConfig";
            this.btnUpdateConfig.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateConfig.TabIndex = 1;
            this.btnUpdateConfig.Text = "更新";
            this.btnUpdateConfig.UseVisualStyleBackColor = true;
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Location = new System.Drawing.Point(6, 19);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(75, 23);
            this.btnAddConfig.TabIndex = 0;
            this.btnAddConfig.Text = "添加";
            this.btnAddConfig.UseVisualStyleBackColor = true;
            // 
            // dgvConfigurations
            // 
            this.dgvConfigurations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConfigurations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfigurations.Location = new System.Drawing.Point(6, 6);
            this.dgvConfigurations.Name = "dgvConfigurations";
            this.dgvConfigurations.Size = new System.Drawing.Size(780, 323);
            this.dgvConfigurations.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "RUINOR ERP 顶级管理服务器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpServerInstances.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServerInstances)).EndInit();
            this.tpAuthorization.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAuthorization)).EndInit();
            this.tpUsers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.tpConfigurations.ResumeLayout(false);
            this.grpConfigActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfigurations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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