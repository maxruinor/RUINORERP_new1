namespace RUINORERP.Server.Controls
{
    partial class ServerMonitorControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblMemoryUsageValue = new System.Windows.Forms.Label();
            this.lblMemoryUsage = new System.Windows.Forms.Label();
            this.lblUptimeValue = new System.Windows.Forms.Label();
            this.lblUptime = new System.Windows.Forms.Label();
            this.lblSystemTimeValue = new System.Windows.Forms.Label();
            this.lblSystemTime = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblLastHeartbeatCheckValue = new System.Windows.Forms.Label();
            this.lblLastHeartbeatCheck = new System.Windows.Forms.Label();
            this.lblLastCleanupValue = new System.Windows.Forms.Label();
            this.lblLastCleanup = new System.Windows.Forms.Label();
            this.lblHeartbeatFailuresValue = new System.Windows.Forms.Label();
            this.lblHeartbeatFailures = new System.Windows.Forms.Label();
            this.lblTimeoutSessionsValue = new System.Windows.Forms.Label();
            this.lblTimeoutSessions = new System.Windows.Forms.Label();
            this.lblPeakSessionsValue = new System.Windows.Forms.Label();
            this.lblPeakSessions = new System.Windows.Forms.Label();
            this.lblTotalSessionsValue = new System.Windows.Forms.Label();
            this.lblTotalSessions = new System.Windows.Forms.Label();
            this.lblActiveSessionsValue = new System.Windows.Forms.Label();
            this.lblActiveSessions = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLastActivityValue = new System.Windows.Forms.Label();
            this.lblLastActivity = new System.Windows.Forms.Label();
            this.lblPeakConnectionsValue = new System.Windows.Forms.Label();
            this.lblPeakConnections = new System.Windows.Forms.Label();
            this.lblTotalConnectionsValue = new System.Windows.Forms.Label();
            this.lblTotalConnections = new System.Windows.Forms.Label();
            this.lblCurrentConnectionsValue = new System.Windows.Forms.Label();
            this.lblCurrentConnections = new System.Windows.Forms.Label();
            this.lblMaxConnectionsValue = new System.Windows.Forms.Label();
            this.lblMaxConnections = new System.Windows.Forms.Label();
            this.lblServerIpValue = new System.Windows.Forms.Label();
            this.lblServerIp = new System.Windows.Forms.Label();
            this.lblPortValue = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvHandlerStatistics = new System.Windows.Forms.DataGridView();
            this.HandlerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SuccessfulCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FailedCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeoutCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurrentProcessing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvgProcessingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxProcessingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblHandlerCountValue = new System.Windows.Forms.Label();
            this.lblHandlerCount = new System.Windows.Forms.Label();
            this.lblDispatcherInitializedValue = new System.Windows.Forms.Label();
            this.lblDispatcherInitialized = new System.Windows.Forms.Label();
            this.btnResetStats = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandlerStatistics)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1000, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(992, 574);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "服务器状态";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblMemoryUsageValue);
            this.groupBox3.Controls.Add(this.lblMemoryUsage);
            this.groupBox3.Controls.Add(this.lblUptimeValue);
            this.groupBox3.Controls.Add(this.lblUptime);
            this.groupBox3.Controls.Add(this.lblSystemTimeValue);
            this.groupBox3.Controls.Add(this.lblSystemTime);
            this.groupBox3.Location = new System.Drawing.Point(6, 423);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(980, 145);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "服务器运行信息";
            // 
            // lblMemoryUsageValue
            // 
            this.lblMemoryUsageValue.AutoSize = true;
            this.lblMemoryUsageValue.Location = new System.Drawing.Point(100, 70);
            this.lblMemoryUsageValue.Name = "lblMemoryUsageValue";
            this.lblMemoryUsageValue.Size = new System.Drawing.Size(29, 12);
            this.lblMemoryUsageValue.TabIndex = 5;
            this.lblMemoryUsageValue.Text = "N/A";
            // 
            // lblMemoryUsage
            // 
            this.lblMemoryUsage.AutoSize = true;
            this.lblMemoryUsage.Location = new System.Drawing.Point(20, 70);
            this.lblMemoryUsage.Name = "lblMemoryUsage";
            this.lblMemoryUsage.Size = new System.Drawing.Size(65, 12);
            this.lblMemoryUsage.TabIndex = 4;
            this.lblMemoryUsage.Text = "内存使用：";
            // 
            // lblUptimeValue
            // 
            this.lblUptimeValue.AutoSize = true;
            this.lblUptimeValue.Location = new System.Drawing.Point(100, 45);
            this.lblUptimeValue.Name = "lblUptimeValue";
            this.lblUptimeValue.Size = new System.Drawing.Size(29, 12);
            this.lblUptimeValue.TabIndex = 3;
            this.lblUptimeValue.Text = "N/A";
            // 
            // lblUptime
            // 
            this.lblUptime.AutoSize = true;
            this.lblUptime.Location = new System.Drawing.Point(20, 45);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(65, 12);
            this.lblUptime.TabIndex = 2;
            this.lblUptime.Text = "运行时间：";
            // 
            // lblSystemTimeValue
            // 
            this.lblSystemTimeValue.AutoSize = true;
            this.lblSystemTimeValue.Location = new System.Drawing.Point(100, 20);
            this.lblSystemTimeValue.Name = "lblSystemTimeValue";
            this.lblSystemTimeValue.Size = new System.Drawing.Size(29, 12);
            this.lblSystemTimeValue.TabIndex = 1;
            this.lblSystemTimeValue.Text = "N/A";
            // 
            // lblSystemTime
            // 
            this.lblSystemTime.AutoSize = true;
            this.lblSystemTime.Location = new System.Drawing.Point(20, 20);
            this.lblSystemTime.Name = "lblSystemTime";
            this.lblSystemTime.Size = new System.Drawing.Size(65, 12);
            this.lblSystemTime.TabIndex = 0;
            this.lblSystemTime.Text = "系统时间：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblLastHeartbeatCheckValue);
            this.groupBox2.Controls.Add(this.lblLastHeartbeatCheck);
            this.groupBox2.Controls.Add(this.lblLastCleanupValue);
            this.groupBox2.Controls.Add(this.lblLastCleanup);
            this.groupBox2.Controls.Add(this.lblHeartbeatFailuresValue);
            this.groupBox2.Controls.Add(this.lblHeartbeatFailures);
            this.groupBox2.Controls.Add(this.lblTimeoutSessionsValue);
            this.groupBox2.Controls.Add(this.lblTimeoutSessions);
            this.groupBox2.Controls.Add(this.lblPeakSessionsValue);
            this.groupBox2.Controls.Add(this.lblPeakSessions);
            this.groupBox2.Controls.Add(this.lblTotalSessionsValue);
            this.groupBox2.Controls.Add(this.lblTotalSessions);
            this.groupBox2.Controls.Add(this.lblActiveSessionsValue);
            this.groupBox2.Controls.Add(this.lblActiveSessions);
            this.groupBox2.Location = new System.Drawing.Point(6, 223);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(980, 194);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "会话统计";
            // 
            // lblLastHeartbeatCheckValue
            // 
            this.lblLastHeartbeatCheckValue.AutoSize = true;
            this.lblLastHeartbeatCheckValue.Location = new System.Drawing.Point(120, 145);
            this.lblLastHeartbeatCheckValue.Name = "lblLastHeartbeatCheckValue";
            this.lblLastHeartbeatCheckValue.Size = new System.Drawing.Size(29, 12);
            this.lblLastHeartbeatCheckValue.TabIndex = 13;
            this.lblLastHeartbeatCheckValue.Text = "N/A";
            // 
            // lblLastHeartbeatCheck
            // 
            this.lblLastHeartbeatCheck.AutoSize = true;
            this.lblLastHeartbeatCheck.Location = new System.Drawing.Point(20, 145);
            this.lblLastHeartbeatCheck.Name = "lblLastHeartbeatCheck";
            this.lblLastHeartbeatCheck.Size = new System.Drawing.Size(89, 12);
            this.lblLastHeartbeatCheck.TabIndex = 12;
            this.lblLastHeartbeatCheck.Text = "最后心跳检查：";
            // 
            // lblLastCleanupValue
            // 
            this.lblLastCleanupValue.AutoSize = true;
            this.lblLastCleanupValue.Location = new System.Drawing.Point(120, 120);
            this.lblLastCleanupValue.Name = "lblLastCleanupValue";
            this.lblLastCleanupValue.Size = new System.Drawing.Size(29, 12);
            this.lblLastCleanupValue.TabIndex = 11;
            this.lblLastCleanupValue.Text = "N/A";
            // 
            // lblLastCleanup
            // 
            this.lblLastCleanup.AutoSize = true;
            this.lblLastCleanup.Location = new System.Drawing.Point(20, 120);
            this.lblLastCleanup.Name = "lblLastCleanup";
            this.lblLastCleanup.Size = new System.Drawing.Size(89, 12);
            this.lblLastCleanup.TabIndex = 10;
            this.lblLastCleanup.Text = "最后清理时间：";
            // 
            // lblHeartbeatFailuresValue
            // 
            this.lblHeartbeatFailuresValue.AutoSize = true;
            this.lblHeartbeatFailuresValue.Location = new System.Drawing.Point(120, 95);
            this.lblHeartbeatFailuresValue.Name = "lblHeartbeatFailuresValue";
            this.lblHeartbeatFailuresValue.Size = new System.Drawing.Size(29, 12);
            this.lblHeartbeatFailuresValue.TabIndex = 9;
            this.lblHeartbeatFailuresValue.Text = "N/A";
            // 
            // lblHeartbeatFailures
            // 
            this.lblHeartbeatFailures.AutoSize = true;
            this.lblHeartbeatFailures.Location = new System.Drawing.Point(20, 95);
            this.lblHeartbeatFailures.Name = "lblHeartbeatFailures";
            this.lblHeartbeatFailures.Size = new System.Drawing.Size(89, 12);
            this.lblHeartbeatFailures.TabIndex = 8;
            this.lblHeartbeatFailures.Text = "心跳失败次数：";
            // 
            // lblTimeoutSessionsValue
            // 
            this.lblTimeoutSessionsValue.AutoSize = true;
            this.lblTimeoutSessionsValue.Location = new System.Drawing.Point(120, 70);
            this.lblTimeoutSessionsValue.Name = "lblTimeoutSessionsValue";
            this.lblTimeoutSessionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblTimeoutSessionsValue.TabIndex = 7;
            this.lblTimeoutSessionsValue.Text = "N/A";
            // 
            // lblTimeoutSessions
            // 
            this.lblTimeoutSessions.AutoSize = true;
            this.lblTimeoutSessions.Location = new System.Drawing.Point(20, 70);
            this.lblTimeoutSessions.Name = "lblTimeoutSessions";
            this.lblTimeoutSessions.Size = new System.Drawing.Size(89, 12);
            this.lblTimeoutSessions.TabIndex = 6;
            this.lblTimeoutSessions.Text = "超时会话数量：";
            // 
            // lblPeakSessionsValue
            // 
            this.lblPeakSessionsValue.AutoSize = true;
            this.lblPeakSessionsValue.Location = new System.Drawing.Point(120, 45);
            this.lblPeakSessionsValue.Name = "lblPeakSessionsValue";
            this.lblPeakSessionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblPeakSessionsValue.TabIndex = 5;
            this.lblPeakSessionsValue.Text = "N/A";
            // 
            // lblPeakSessions
            // 
            this.lblPeakSessions.AutoSize = true;
            this.lblPeakSessions.Location = new System.Drawing.Point(20, 45);
            this.lblPeakSessions.Name = "lblPeakSessions";
            this.lblPeakSessions.Size = new System.Drawing.Size(89, 12);
            this.lblPeakSessions.TabIndex = 4;
            this.lblPeakSessions.Text = "峰值会话数量：";
            // 
            // lblTotalSessionsValue
            // 
            this.lblTotalSessionsValue.AutoSize = true;
            this.lblTotalSessionsValue.Location = new System.Drawing.Point(120, 20);
            this.lblTotalSessionsValue.Name = "lblTotalSessionsValue";
            this.lblTotalSessionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblTotalSessionsValue.TabIndex = 3;
            this.lblTotalSessionsValue.Text = "N/A";
            // 
            // lblTotalSessions
            // 
            this.lblTotalSessions.AutoSize = true;
            this.lblTotalSessions.Location = new System.Drawing.Point(20, 20);
            this.lblTotalSessions.Name = "lblTotalSessions";
            this.lblTotalSessions.Size = new System.Drawing.Size(89, 12);
            this.lblTotalSessions.TabIndex = 2;
            this.lblTotalSessions.Text = "总会话连接数：";
            // 
            // lblActiveSessionsValue
            // 
            this.lblActiveSessionsValue.AutoSize = true;
            this.lblActiveSessionsValue.Location = new System.Drawing.Point(320, 20);
            this.lblActiveSessionsValue.Name = "lblActiveSessionsValue";
            this.lblActiveSessionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblActiveSessionsValue.TabIndex = 1;
            this.lblActiveSessionsValue.Text = "N/A";
            // 
            // lblActiveSessions
            // 
            this.lblActiveSessions.AutoSize = true;
            this.lblActiveSessions.Location = new System.Drawing.Point(220, 20);
            this.lblActiveSessions.Name = "lblActiveSessions";
            this.lblActiveSessions.Size = new System.Drawing.Size(89, 12);
            this.lblActiveSessions.TabIndex = 0;
            this.lblActiveSessions.Text = "活动会话数量：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblLastActivityValue);
            this.groupBox1.Controls.Add(this.lblLastActivity);
            this.groupBox1.Controls.Add(this.lblPeakConnectionsValue);
            this.groupBox1.Controls.Add(this.lblPeakConnections);
            this.groupBox1.Controls.Add(this.lblTotalConnectionsValue);
            this.groupBox1.Controls.Add(this.lblTotalConnections);
            this.groupBox1.Controls.Add(this.lblCurrentConnectionsValue);
            this.groupBox1.Controls.Add(this.lblCurrentConnections);
            this.groupBox1.Controls.Add(this.lblMaxConnectionsValue);
            this.groupBox1.Controls.Add(this.lblMaxConnections);
            this.groupBox1.Controls.Add(this.lblServerIpValue);
            this.groupBox1.Controls.Add(this.lblServerIp);
            this.groupBox1.Controls.Add(this.lblPortValue);
            this.groupBox1.Controls.Add(this.lblPort);
            this.groupBox1.Controls.Add(this.lblStatusValue);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(980, 211);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "基本服务器信息";
            // 
            // lblLastActivityValue
            // 
            this.lblLastActivityValue.AutoSize = true;
            this.lblLastActivityValue.Location = new System.Drawing.Point(100, 170);
            this.lblLastActivityValue.Name = "lblLastActivityValue";
            this.lblLastActivityValue.Size = new System.Drawing.Size(29, 12);
            this.lblLastActivityValue.TabIndex = 15;
            this.lblLastActivityValue.Text = "N/A";
            // 
            // lblLastActivity
            // 
            this.lblLastActivity.AutoSize = true;
            this.lblLastActivity.Location = new System.Drawing.Point(20, 170);
            this.lblLastActivity.Name = "lblLastActivity";
            this.lblLastActivity.Size = new System.Drawing.Size(89, 12);
            this.lblLastActivity.TabIndex = 14;
            this.lblLastActivity.Text = "最后活动时间：";
            // 
            // lblPeakConnectionsValue
            // 
            this.lblPeakConnectionsValue.AutoSize = true;
            this.lblPeakConnectionsValue.Location = new System.Drawing.Point(100, 145);
            this.lblPeakConnectionsValue.Name = "lblPeakConnectionsValue";
            this.lblPeakConnectionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblPeakConnectionsValue.TabIndex = 13;
            this.lblPeakConnectionsValue.Text = "N/A";
            // 
            // lblPeakConnections
            // 
            this.lblPeakConnections.AutoSize = true;
            this.lblPeakConnections.Location = new System.Drawing.Point(20, 145);
            this.lblPeakConnections.Name = "lblPeakConnections";
            this.lblPeakConnections.Size = new System.Drawing.Size(77, 12);
            this.lblPeakConnections.TabIndex = 12;
            this.lblPeakConnections.Text = "峰值连接数：";
            // 
            // lblTotalConnectionsValue
            // 
            this.lblTotalConnectionsValue.AutoSize = true;
            this.lblTotalConnectionsValue.Location = new System.Drawing.Point(100, 120);
            this.lblTotalConnectionsValue.Name = "lblTotalConnectionsValue";
            this.lblTotalConnectionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblTotalConnectionsValue.TabIndex = 11;
            this.lblTotalConnectionsValue.Text = "N/A";
            // 
            // lblTotalConnections
            // 
            this.lblTotalConnections.AutoSize = true;
            this.lblTotalConnections.Location = new System.Drawing.Point(20, 120);
            this.lblTotalConnections.Name = "lblTotalConnections";
            this.lblTotalConnections.Size = new System.Drawing.Size(77, 12);
            this.lblTotalConnections.TabIndex = 10;
            this.lblTotalConnections.Text = "总连接次数：";
            // 
            // lblCurrentConnectionsValue
            // 
            this.lblCurrentConnectionsValue.AutoSize = true;
            this.lblCurrentConnectionsValue.Location = new System.Drawing.Point(100, 95);
            this.lblCurrentConnectionsValue.Name = "lblCurrentConnectionsValue";
            this.lblCurrentConnectionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblCurrentConnectionsValue.TabIndex = 9;
            this.lblCurrentConnectionsValue.Text = "N/A";
            // 
            // lblCurrentConnections
            // 
            this.lblCurrentConnections.AutoSize = true;
            this.lblCurrentConnections.Location = new System.Drawing.Point(20, 95);
            this.lblCurrentConnections.Name = "lblCurrentConnections";
            this.lblCurrentConnections.Size = new System.Drawing.Size(77, 12);
            this.lblCurrentConnections.TabIndex = 8;
            this.lblCurrentConnections.Text = "当前连接数：";
            // 
            // lblMaxConnectionsValue
            // 
            this.lblMaxConnectionsValue.AutoSize = true;
            this.lblMaxConnectionsValue.Location = new System.Drawing.Point(100, 70);
            this.lblMaxConnectionsValue.Name = "lblMaxConnectionsValue";
            this.lblMaxConnectionsValue.Size = new System.Drawing.Size(29, 12);
            this.lblMaxConnectionsValue.TabIndex = 7;
            this.lblMaxConnectionsValue.Text = "N/A";
            // 
            // lblMaxConnections
            // 
            this.lblMaxConnections.AutoSize = true;
            this.lblMaxConnections.Location = new System.Drawing.Point(20, 70);
            this.lblMaxConnections.Name = "lblMaxConnections";
            this.lblMaxConnections.Size = new System.Drawing.Size(77, 12);
            this.lblMaxConnections.TabIndex = 6;
            this.lblMaxConnections.Text = "最大连接数：";
            // 
            // lblServerIpValue
            // 
            this.lblServerIpValue.AutoSize = true;
            this.lblServerIpValue.Location = new System.Drawing.Point(100, 45);
            this.lblServerIpValue.Name = "lblServerIpValue";
            this.lblServerIpValue.Size = new System.Drawing.Size(29, 12);
            this.lblServerIpValue.TabIndex = 5;
            this.lblServerIpValue.Text = "N/A";
            // 
            // lblServerIp
            // 
            this.lblServerIp.AutoSize = true;
            this.lblServerIp.Location = new System.Drawing.Point(20, 45);
            this.lblServerIp.Name = "lblServerIp";
            this.lblServerIp.Size = new System.Drawing.Size(53, 12);
            this.lblServerIp.TabIndex = 4;
            this.lblServerIp.Text = "服务器IP";
            // 
            // lblPortValue
            // 
            this.lblPortValue.AutoSize = true;
            this.lblPortValue.Location = new System.Drawing.Point(320, 20);
            this.lblPortValue.Name = "lblPortValue";
            this.lblPortValue.Size = new System.Drawing.Size(29, 12);
            this.lblPortValue.TabIndex = 3;
            this.lblPortValue.Text = "N/A";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(220, 20);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(41, 12);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "端口：";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Location = new System.Drawing.Point(100, 20);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(29, 12);
            this.lblStatusValue.TabIndex = 1;
            this.lblStatusValue.Text = "N/A";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(20, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "状态：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(992, 574);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "指令处理状态";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvHandlerStatistics);
            this.groupBox5.Location = new System.Drawing.Point(6, 106);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(980, 462);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "处理器统计信息";
            // 
            // dgvHandlerStatistics
            // 
            this.dgvHandlerStatistics.AllowUserToAddRows = false;
            this.dgvHandlerStatistics.AllowUserToDeleteRows = false;
            this.dgvHandlerStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHandlerStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.HandlerName,
            this.Status,
            this.Priority,
            this.TotalCommands,
            this.SuccessfulCommands,
            this.FailedCommands,
            this.TimeoutCount,
            this.CurrentProcessing,
            this.AvgProcessingTime,
            this.MaxProcessingTime});
            this.dgvHandlerStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHandlerStatistics.Location = new System.Drawing.Point(3, 17);
            this.dgvHandlerStatistics.Name = "dgvHandlerStatistics";
            this.dgvHandlerStatistics.ReadOnly = true;
            this.dgvHandlerStatistics.RowTemplate.Height = 23;
            this.dgvHandlerStatistics.Size = new System.Drawing.Size(974, 442);
            this.dgvHandlerStatistics.TabIndex = 0;
            // 
            // HandlerName
            // 
            this.HandlerName.HeaderText = "处理器名称";
            this.HandlerName.Name = "HandlerName";
            this.HandlerName.ReadOnly = true;
            // 
            // Status
            // 
            this.Status.HeaderText = "状态";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // Priority
            // 
            this.Priority.HeaderText = "优先级";
            this.Priority.Name = "Priority";
            this.Priority.ReadOnly = true;
            // 
            // TotalCommands
            // 
            this.TotalCommands.HeaderText = "总命令数";
            this.TotalCommands.Name = "TotalCommands";
            this.TotalCommands.ReadOnly = true;
            // 
            // SuccessfulCommands
            // 
            this.SuccessfulCommands.HeaderText = "成功命令数";
            this.SuccessfulCommands.Name = "SuccessfulCommands";
            this.SuccessfulCommands.ReadOnly = true;
            // 
            // FailedCommands
            // 
            this.FailedCommands.HeaderText = "失败命令数";
            this.FailedCommands.Name = "FailedCommands";
            this.FailedCommands.ReadOnly = true;
            // 
            // TimeoutCount
            // 
            this.TimeoutCount.HeaderText = "超时次数";
            this.TimeoutCount.Name = "TimeoutCount";
            this.TimeoutCount.ReadOnly = true;
            // 
            // CurrentProcessing
            // 
            this.CurrentProcessing.HeaderText = "当前处理数";
            this.CurrentProcessing.Name = "CurrentProcessing";
            this.CurrentProcessing.ReadOnly = true;
            // 
            // AvgProcessingTime
            // 
            this.AvgProcessingTime.HeaderText = "平均处理时间(ms)";
            this.AvgProcessingTime.Name = "AvgProcessingTime";
            this.AvgProcessingTime.ReadOnly = true;
            // 
            // MaxProcessingTime
            // 
            this.MaxProcessingTime.HeaderText = "最大处理时间(ms)";
            this.MaxProcessingTime.Name = "MaxProcessingTime";
            this.MaxProcessingTime.ReadOnly = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblHandlerCountValue);
            this.groupBox4.Controls.Add(this.lblHandlerCount);
            this.groupBox4.Controls.Add(this.lblDispatcherInitializedValue);
            this.groupBox4.Controls.Add(this.lblDispatcherInitialized);
            this.groupBox4.Location = new System.Drawing.Point(6, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(980, 94);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "命令调度器信息";
            // 
            // lblHandlerCountValue
            // 
            this.lblHandlerCountValue.AutoSize = true;
            this.lblHandlerCountValue.Location = new System.Drawing.Point(100, 45);
            this.lblHandlerCountValue.Name = "lblHandlerCountValue";
            this.lblHandlerCountValue.Size = new System.Drawing.Size(29, 12);
            this.lblHandlerCountValue.TabIndex = 3;
            this.lblHandlerCountValue.Text = "N/A";
            // 
            // lblHandlerCount
            // 
            this.lblHandlerCount.AutoSize = true;
            this.lblHandlerCount.Location = new System.Drawing.Point(20, 45);
            this.lblHandlerCount.Name = "lblHandlerCount";
            this.lblHandlerCount.Size = new System.Drawing.Size(77, 12);
            this.lblHandlerCount.TabIndex = 2;
            this.lblHandlerCount.Text = "处理器数量：";
            // 
            // lblDispatcherInitializedValue
            // 
            this.lblDispatcherInitializedValue.AutoSize = true;
            this.lblDispatcherInitializedValue.Location = new System.Drawing.Point(100, 20);
            this.lblDispatcherInitializedValue.Name = "lblDispatcherInitializedValue";
            this.lblDispatcherInitializedValue.Size = new System.Drawing.Size(29, 12);
            this.lblDispatcherInitializedValue.TabIndex = 1;
            this.lblDispatcherInitializedValue.Text = "N/A";
            // 
            // lblDispatcherInitialized
            // 
            this.lblDispatcherInitialized.AutoSize = true;
            this.lblDispatcherInitialized.Location = new System.Drawing.Point(20, 20);
            this.lblDispatcherInitialized.Name = "lblDispatcherInitialized";
            this.lblDispatcherInitialized.Size = new System.Drawing.Size(77, 12);
            this.lblDispatcherInitialized.TabIndex = 0;
            this.lblDispatcherInitialized.Text = "是否已初始化";
            // 
            // btnResetStats
            // 
            this.btnResetStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetStats.Location = new System.Drawing.Point(913, 606);
            this.btnResetStats.Name = "btnResetStats";
            this.btnResetStats.Size = new System.Drawing.Size(75, 23);
            this.btnResetStats.TabIndex = 1;
            this.btnResetStats.Text = "重置统计";
            this.btnResetStats.UseVisualStyleBackColor = true;
            this.btnResetStats.Click += new System.EventHandler(this.btnResetStats_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(832, 606);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // ServerMonitorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnResetStats);
            this.Controls.Add(this.tabControl1);
            this.Name = "ServerMonitorControl";
            this.Size = new System.Drawing.Size(1000, 635);
            this.Load += new System.EventHandler(this.ServerMonitorControl_Load);
            this.Disposed += new System.EventHandler(this.ServerMonitorControl_Disposed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHandlerStatistics)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblPortValue;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblServerIpValue;
        private System.Windows.Forms.Label lblServerIp;
        private System.Windows.Forms.Label lblMaxConnectionsValue;
        private System.Windows.Forms.Label lblMaxConnections;
        private System.Windows.Forms.Label lblCurrentConnectionsValue;
        private System.Windows.Forms.Label lblCurrentConnections;
        private System.Windows.Forms.Label lblTotalConnectionsValue;
        private System.Windows.Forms.Label lblTotalConnections;
        private System.Windows.Forms.Label lblPeakConnectionsValue;
        private System.Windows.Forms.Label lblPeakConnections;
        private System.Windows.Forms.Label lblLastActivityValue;
        private System.Windows.Forms.Label lblLastActivity;
        private System.Windows.Forms.Label lblActiveSessionsValue;
        private System.Windows.Forms.Label lblActiveSessions;
        private System.Windows.Forms.Label lblTotalSessionsValue;
        private System.Windows.Forms.Label lblTotalSessions;
        private System.Windows.Forms.Label lblPeakSessionsValue;
        private System.Windows.Forms.Label lblPeakSessions;
        private System.Windows.Forms.Label lblTimeoutSessionsValue;
        private System.Windows.Forms.Label lblTimeoutSessions;
        private System.Windows.Forms.Label lblHeartbeatFailuresValue;
        private System.Windows.Forms.Label lblHeartbeatFailures;
        private System.Windows.Forms.Label lblLastCleanupValue;
        private System.Windows.Forms.Label lblLastCleanup;
        private System.Windows.Forms.Label lblLastHeartbeatCheckValue;
        private System.Windows.Forms.Label lblLastHeartbeatCheck;
        private System.Windows.Forms.Label lblSystemTimeValue;
        private System.Windows.Forms.Label lblSystemTime;
        private System.Windows.Forms.Label lblUptimeValue;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Label lblMemoryUsageValue;
        private System.Windows.Forms.Label lblMemoryUsage;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblDispatcherInitializedValue;
        private System.Windows.Forms.Label lblDispatcherInitialized;
        private System.Windows.Forms.Label lblHandlerCountValue;
        private System.Windows.Forms.Label lblHandlerCount;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dgvHandlerStatistics;
        private System.Windows.Forms.DataGridViewTextBoxColumn HandlerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalCommands;
        private System.Windows.Forms.DataGridViewTextBoxColumn SuccessfulCommands;
        private System.Windows.Forms.DataGridViewTextBoxColumn FailedCommands;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeoutCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurrentProcessing;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvgProcessingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxProcessingTime;
        private System.Windows.Forms.Button btnResetStats;
        private System.Windows.Forms.Button btnRefresh;
    }
}