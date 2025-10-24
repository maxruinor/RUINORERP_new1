namespace RUINORERP.Server.Controls
{
    partial class ServerMonitorControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // 为性能指标添加的控件声明
        private System.Windows.Forms.Label lblConnectionUtilization;
        private System.Windows.Forms.Label lblConnectionUtilizationValue;
        private System.Windows.Forms.Label lblThroughput;
        private System.Windows.Forms.Label lblThroughputValue;
        private System.Windows.Forms.Label lblRequestsPerSecond;
        private System.Windows.Forms.Label lblRequestsPerSecondValue;
        private System.Windows.Forms.Label lblSystemHealth;
        private System.Windows.Forms.Label lblSystemHealthValue;
        // 添加SplitContainer和Panel控件声明
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        // 熔断器监控相关控件声明
        private System.Windows.Forms.GroupBox groupBox7; // 保持groupBox7名称不变
        private System.Windows.Forms.Label cbLblStatus;
        private System.Windows.Forms.Label cbLblStatusValue;
        private System.Windows.Forms.Label cbLblTotalRequests;
        private System.Windows.Forms.Label cbLblTotalRequestsValue;
        private System.Windows.Forms.Label cbLblSuccessRate;
        private System.Windows.Forms.Label cbLblSuccessRateValue;
        private System.Windows.Forms.Label cbLblFailedRequests;
        private System.Windows.Forms.Label cbLblFailedRequestsValue;
        private System.Windows.Forms.Label cbLblStateChanges;
        private System.Windows.Forms.Label cbLblStateChangesValue;
        private System.Windows.Forms.Label cbLblAvgResponseTime;
        private System.Windows.Forms.Label cbLblAvgResponseTimeValue;



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
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            panelBottom = new System.Windows.Forms.Panel();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            btnDiagnosticsReport = new System.Windows.Forms.Button();
            btnErrorReport = new System.Windows.Forms.Button();
            btnPerformanceReport = new System.Windows.Forms.Button();
            btnRefresh = new System.Windows.Forms.Button();
            btnResetStats = new System.Windows.Forms.Button();
            panelTop = new System.Windows.Forms.Panel();
            groupBox3 = new System.Windows.Forms.GroupBox();
            lblAvgProcessingTimeValue = new System.Windows.Forms.Label();
            lblAvgProcessingTime = new System.Windows.Forms.Label();
            lblRealTimeSuccessRateValue = new System.Windows.Forms.Label();
            lblRealTimeSuccessRate = new System.Windows.Forms.Label();
            lblCurrentProcessingValue = new System.Windows.Forms.Label();
            lblCurrentProcessing = new System.Windows.Forms.Label();
            lblActiveHandlersValue = new System.Windows.Forms.Label();
            lblActiveHandlers = new System.Windows.Forms.Label();
            lblTotalHandlersValue = new System.Windows.Forms.Label();
            lblTotalHandlers = new System.Windows.Forms.Label();
            lblMemoryUsageValue = new System.Windows.Forms.Label();
            lblMemoryUsage = new System.Windows.Forms.Label();
            lblUptimeValue = new System.Windows.Forms.Label();
            lblUptime = new System.Windows.Forms.Label();
            lblSystemTimeValue = new System.Windows.Forms.Label();
            lblSystemTime = new System.Windows.Forms.Label();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lblLastActivityValue = new System.Windows.Forms.Label();
            lblLastActivity = new System.Windows.Forms.Label();
            lblPeakConnectionsValue = new System.Windows.Forms.Label();
            lblPeakConnections = new System.Windows.Forms.Label();
            lblTotalConnectionsValue = new System.Windows.Forms.Label();
            lblTotalConnections = new System.Windows.Forms.Label();
            lblCurrentConnectionsValue = new System.Windows.Forms.Label();
            lblCurrentConnections = new System.Windows.Forms.Label();
            lblMaxConnectionsValue = new System.Windows.Forms.Label();
            lblMaxConnections = new System.Windows.Forms.Label();
            lblServerIpValue = new System.Windows.Forms.Label();
            lblServerIp = new System.Windows.Forms.Label();
            lblPortValue = new System.Windows.Forms.Label();
            lblPort = new System.Windows.Forms.Label();
            lblStatusValue = new System.Windows.Forms.Label();
            lblStatus = new System.Windows.Forms.Label();
            groupBox2 = new System.Windows.Forms.GroupBox();
            lblSystemHealthValue = new System.Windows.Forms.Label();
            lblSystemHealth = new System.Windows.Forms.Label();
            lblRequestsPerSecondValue = new System.Windows.Forms.Label();
            lblRequestsPerSecond = new System.Windows.Forms.Label();
            lblThroughputValue = new System.Windows.Forms.Label();
            lblThroughput = new System.Windows.Forms.Label();
            lblConnectionUtilizationValue = new System.Windows.Forms.Label();
            lblConnectionUtilization = new System.Windows.Forms.Label();
            lblLastHeartbeatCheckValue = new System.Windows.Forms.Label();
            lblLastHeartbeatCheck = new System.Windows.Forms.Label();
            lblLastCleanupValue = new System.Windows.Forms.Label();
            lblLastCleanup = new System.Windows.Forms.Label();
            lblHeartbeatFailuresValue = new System.Windows.Forms.Label();
            lblHeartbeatFailures = new System.Windows.Forms.Label();
            lblTimeoutSessionsValue = new System.Windows.Forms.Label();
            lblTimeoutSessions = new System.Windows.Forms.Label();
            lblPeakSessionsValue = new System.Windows.Forms.Label();
            lblPeakSessions = new System.Windows.Forms.Label();
            lblTotalSessionsValue = new System.Windows.Forms.Label();
            lblTotalSessions = new System.Windows.Forms.Label();
            lblActiveSessionsValue = new System.Windows.Forms.Label();
            lblActiveSessions = new System.Windows.Forms.Label();
            groupBox7 = new System.Windows.Forms.GroupBox();
            cbLblStatus = new System.Windows.Forms.Label();
            cbLblStatusValue = new System.Windows.Forms.Label();
            cbLblTotalRequests = new System.Windows.Forms.Label();
            cbLblTotalRequestsValue = new System.Windows.Forms.Label();
            cbLblSuccessRate = new System.Windows.Forms.Label();
            cbLblSuccessRateValue = new System.Windows.Forms.Label();
            cbLblFailedRequests = new System.Windows.Forms.Label();
            cbLblFailedRequestsValue = new System.Windows.Forms.Label();
            cbLblStateChanges = new System.Windows.Forms.Label();
            cbLblStateChangesValue = new System.Windows.Forms.Label();
            cbLblAvgResponseTime = new System.Windows.Forms.Label();
            cbLblAvgResponseTimeValue = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            groupBox6 = new System.Windows.Forms.GroupBox();
            lblTimeoutCommandsValue = new System.Windows.Forms.Label();
            lblTimeoutCommands = new System.Windows.Forms.Label();
            lblFailedCommandsValue = new System.Windows.Forms.Label();
            lblFailedCommands = new System.Windows.Forms.Label();
            lblTotalCommandsValue = new System.Windows.Forms.Label();
            lblTotalCommands = new System.Windows.Forms.Label();
            lblSuccessRateValue = new System.Windows.Forms.Label();
            lblSuccessRate = new System.Windows.Forms.Label();
            lblHealthStatusValue = new System.Windows.Forms.Label();
            lblHealthStatus = new System.Windows.Forms.Label();
            groupBox5 = new System.Windows.Forms.GroupBox();
            dgvHandlerStatistics = new System.Windows.Forms.DataGridView();
            HandlerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TotalCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            SuccessfulCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            FailedCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            TimeoutCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            CurrentProcessing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            AvgProcessingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            MaxProcessingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            groupBox4 = new System.Windows.Forms.GroupBox();
            lblHandlerCountValue = new System.Windows.Forms.Label();
            lblHandlerCount = new System.Windows.Forms.Label();
            lblDispatcherInitializedValue = new System.Windows.Forms.Label();
            lblDispatcherInitialized = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panelTop.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox7.SuspendLayout();
            tabPage2.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvHandlerStatistics).BeginInit();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Margin = new System.Windows.Forms.Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1167, 965);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panelBottom);
            tabPage1.Controls.Add(panelTop);
            tabPage1.Location = new System.Drawing.Point(4, 26);
            tabPage1.Margin = new System.Windows.Forms.Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(4);
            tabPage1.Size = new System.Drawing.Size(1159, 935);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "服务器状态";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(splitContainer1);
            panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            panelBottom.Location = new System.Drawing.Point(4, 584);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new System.Drawing.Size(1151, 347);
            panelBottom.TabIndex = 7;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnDiagnosticsReport);
            splitContainer1.Panel2.Controls.Add(btnErrorReport);
            splitContainer1.Panel2.Controls.Add(btnPerformanceReport);
            splitContainer1.Panel2.Controls.Add(btnRefresh);
            splitContainer1.Panel2.Controls.Add(btnResetStats);
            splitContainer1.Size = new System.Drawing.Size(1151, 347);
            splitContainer1.SplitterDistance = 750;
            splitContainer1.TabIndex = 0;
            // 
            // btnDiagnosticsReport
            // 
            btnDiagnosticsReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnDiagnosticsReport.Location = new System.Drawing.Point(84, 60);
            btnDiagnosticsReport.Margin = new System.Windows.Forms.Padding(4);
            btnDiagnosticsReport.Name = "btnDiagnosticsReport";
            btnDiagnosticsReport.Size = new System.Drawing.Size(88, 33);
            btnDiagnosticsReport.TabIndex = 5;
            btnDiagnosticsReport.Text = "诊断报告";
            btnDiagnosticsReport.UseVisualStyleBackColor = true;
            btnDiagnosticsReport.Click += btnDiagnosticsReport_Click;
            // 
            // btnErrorReport
            // 
            btnErrorReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnErrorReport.Location = new System.Drawing.Point(270, 60);
            btnErrorReport.Margin = new System.Windows.Forms.Padding(4);
            btnErrorReport.Name = "btnErrorReport";
            btnErrorReport.Size = new System.Drawing.Size(88, 33);
            btnErrorReport.TabIndex = 4;
            btnErrorReport.Text = "错误报告";
            btnErrorReport.UseVisualStyleBackColor = true;
            btnErrorReport.Click += btnErrorReport_Click;
            // 
            // btnPerformanceReport
            // 
            btnPerformanceReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnPerformanceReport.Location = new System.Drawing.Point(176, 60);
            btnPerformanceReport.Margin = new System.Windows.Forms.Padding(4);
            btnPerformanceReport.Name = "btnPerformanceReport";
            btnPerformanceReport.Size = new System.Drawing.Size(88, 33);
            btnPerformanceReport.TabIndex = 3;
            btnPerformanceReport.Text = "性能报告";
            btnPerformanceReport.UseVisualStyleBackColor = true;
            btnPerformanceReport.Click += btnPerformanceReport_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Location = new System.Drawing.Point(84, 116);
            btnRefresh.Margin = new System.Windows.Forms.Padding(4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(88, 33);
            btnRefresh.TabIndex = 2;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnResetStats
            // 
            btnResetStats.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnResetStats.Location = new System.Drawing.Point(180, 116);
            btnResetStats.Margin = new System.Windows.Forms.Padding(4);
            btnResetStats.Name = "btnResetStats";
            btnResetStats.Size = new System.Drawing.Size(88, 33);
            btnResetStats.TabIndex = 1;
            btnResetStats.Text = "重置统计";
            btnResetStats.UseVisualStyleBackColor = true;
            btnResetStats.Click += btnResetStats_Click;
            // 
            // panelTop
            // 
            panelTop.Controls.Add(groupBox3);
            panelTop.Controls.Add(groupBox1);
            panelTop.Controls.Add(groupBox7);
            panelTop.Controls.Add(groupBox2);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(4, 4);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(1151, 580);
            panelTop.TabIndex = 6;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lblAvgProcessingTimeValue);
            groupBox3.Controls.Add(lblAvgProcessingTime);
            groupBox3.Controls.Add(lblRealTimeSuccessRateValue);
            groupBox3.Controls.Add(lblRealTimeSuccessRate);
            groupBox3.Controls.Add(lblCurrentProcessingValue);
            groupBox3.Controls.Add(lblCurrentProcessing);
            groupBox3.Controls.Add(lblActiveHandlersValue);
            groupBox3.Controls.Add(lblActiveHandlers);
            groupBox3.Controls.Add(lblTotalHandlersValue);
            groupBox3.Controls.Add(lblTotalHandlers);
            groupBox3.Controls.Add(lblMemoryUsageValue);
            groupBox3.Controls.Add(lblMemoryUsage);
            groupBox3.Controls.Add(lblUptimeValue);
            groupBox3.Controls.Add(lblUptime);
            groupBox3.Controls.Add(lblSystemTimeValue);
            groupBox3.Controls.Add(lblSystemTime);
            groupBox3.Location = new System.Drawing.Point(479, 18);
            groupBox3.Margin = new System.Windows.Forms.Padding(4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4);
            groupBox3.Size = new System.Drawing.Size(461, 268);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "服务器运行信息";
            // 
            // lblAvgProcessingTimeValue
            // 
            lblAvgProcessingTimeValue.AutoSize = true;
            lblAvgProcessingTimeValue.Location = new System.Drawing.Point(117, 205);
            lblAvgProcessingTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblAvgProcessingTimeValue.Name = "lblAvgProcessingTimeValue";
            lblAvgProcessingTimeValue.Size = new System.Drawing.Size(31, 17);
            lblAvgProcessingTimeValue.TabIndex = 15;
            lblAvgProcessingTimeValue.Text = "N/A";
            // 
            // lblAvgProcessingTime
            // 
            lblAvgProcessingTime.AutoSize = true;
            lblAvgProcessingTime.Location = new System.Drawing.Point(23, 205);
            lblAvgProcessingTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblAvgProcessingTime.Name = "lblAvgProcessingTime";
            lblAvgProcessingTime.Size = new System.Drawing.Size(92, 17);
            lblAvgProcessingTime.TabIndex = 14;
            lblAvgProcessingTime.Text = "平均处理时间：";
            // 
            // lblRealTimeSuccessRateValue
            // 
            lblRealTimeSuccessRateValue.AutoSize = true;
            lblRealTimeSuccessRateValue.Location = new System.Drawing.Point(117, 170);
            lblRealTimeSuccessRateValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRealTimeSuccessRateValue.Name = "lblRealTimeSuccessRateValue";
            lblRealTimeSuccessRateValue.Size = new System.Drawing.Size(31, 17);
            lblRealTimeSuccessRateValue.TabIndex = 13;
            lblRealTimeSuccessRateValue.Text = "N/A";
            // 
            // lblRealTimeSuccessRate
            // 
            lblRealTimeSuccessRate.AutoSize = true;
            lblRealTimeSuccessRate.Location = new System.Drawing.Point(23, 170);
            lblRealTimeSuccessRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRealTimeSuccessRate.Name = "lblRealTimeSuccessRate";
            lblRealTimeSuccessRate.Size = new System.Drawing.Size(80, 17);
            lblRealTimeSuccessRate.TabIndex = 12;
            lblRealTimeSuccessRate.Text = "实时成功率：";
            // 
            // lblCurrentProcessingValue
            // 
            lblCurrentProcessingValue.AutoSize = true;
            lblCurrentProcessingValue.Location = new System.Drawing.Point(117, 135);
            lblCurrentProcessingValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCurrentProcessingValue.Name = "lblCurrentProcessingValue";
            lblCurrentProcessingValue.Size = new System.Drawing.Size(31, 17);
            lblCurrentProcessingValue.TabIndex = 11;
            lblCurrentProcessingValue.Text = "N/A";
            // 
            // lblCurrentProcessing
            // 
            lblCurrentProcessing.AutoSize = true;
            lblCurrentProcessing.Location = new System.Drawing.Point(23, 135);
            lblCurrentProcessing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCurrentProcessing.Name = "lblCurrentProcessing";
            lblCurrentProcessing.Size = new System.Drawing.Size(80, 17);
            lblCurrentProcessing.TabIndex = 10;
            lblCurrentProcessing.Text = "当前处理数：";
            // 
            // lblActiveHandlersValue
            // 
            lblActiveHandlersValue.AutoSize = true;
            lblActiveHandlersValue.Location = new System.Drawing.Point(117, 99);
            lblActiveHandlersValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblActiveHandlersValue.Name = "lblActiveHandlersValue";
            lblActiveHandlersValue.Size = new System.Drawing.Size(31, 17);
            lblActiveHandlersValue.TabIndex = 9;
            lblActiveHandlersValue.Text = "N/A";
            // 
            // lblActiveHandlers
            // 
            lblActiveHandlers.AutoSize = true;
            lblActiveHandlers.Location = new System.Drawing.Point(23, 99);
            lblActiveHandlers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblActiveHandlers.Name = "lblActiveHandlers";
            lblActiveHandlers.Size = new System.Drawing.Size(80, 17);
            lblActiveHandlers.TabIndex = 8;
            lblActiveHandlers.Text = "活跃处理器：";
            // 
            // lblTotalHandlersValue
            // 
            lblTotalHandlersValue.AutoSize = true;
            lblTotalHandlersValue.Location = new System.Drawing.Point(117, 64);
            lblTotalHandlersValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalHandlersValue.Name = "lblTotalHandlersValue";
            lblTotalHandlersValue.Size = new System.Drawing.Size(31, 17);
            lblTotalHandlersValue.TabIndex = 7;
            lblTotalHandlersValue.Text = "N/A";
            // 
            // lblTotalHandlers
            // 
            lblTotalHandlers.AutoSize = true;
            lblTotalHandlers.Location = new System.Drawing.Point(23, 64);
            lblTotalHandlers.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalHandlers.Name = "lblTotalHandlers";
            lblTotalHandlers.Size = new System.Drawing.Size(80, 17);
            lblTotalHandlers.TabIndex = 6;
            lblTotalHandlers.Text = "处理器总数：";
            // 
            // lblMemoryUsageValue
            // 
            lblMemoryUsageValue.AutoSize = true;
            lblMemoryUsageValue.Location = new System.Drawing.Point(117, 241);
            lblMemoryUsageValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMemoryUsageValue.Name = "lblMemoryUsageValue";
            lblMemoryUsageValue.Size = new System.Drawing.Size(31, 17);
            lblMemoryUsageValue.TabIndex = 5;
            lblMemoryUsageValue.Text = "N/A";
            // 
            // lblMemoryUsage
            // 
            lblMemoryUsage.AutoSize = true;
            lblMemoryUsage.Location = new System.Drawing.Point(23, 241);
            lblMemoryUsage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMemoryUsage.Name = "lblMemoryUsage";
            lblMemoryUsage.Size = new System.Drawing.Size(68, 17);
            lblMemoryUsage.TabIndex = 4;
            lblMemoryUsage.Text = "内存使用：";
            // 
            // lblUptimeValue
            // 
            lblUptimeValue.AutoSize = true;
            lblUptimeValue.Location = new System.Drawing.Point(385, 28);
            lblUptimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUptimeValue.Name = "lblUptimeValue";
            lblUptimeValue.Size = new System.Drawing.Size(31, 17);
            lblUptimeValue.TabIndex = 3;
            lblUptimeValue.Text = "N/A";
            // 
            // lblUptime
            // 
            lblUptime.AutoSize = true;
            lblUptime.Location = new System.Drawing.Point(291, 28);
            lblUptime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUptime.Name = "lblUptime";
            lblUptime.Size = new System.Drawing.Size(68, 17);
            lblUptime.TabIndex = 2;
            lblUptime.Text = "运行时间：";
            // 
            // lblSystemTimeValue
            // 
            lblSystemTimeValue.AutoSize = true;
            lblSystemTimeValue.Location = new System.Drawing.Point(117, 28);
            lblSystemTimeValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSystemTimeValue.Name = "lblSystemTimeValue";
            lblSystemTimeValue.Size = new System.Drawing.Size(31, 17);
            lblSystemTimeValue.TabIndex = 1;
            lblSystemTimeValue.Text = "N/A";
            // 
            // lblSystemTime
            // 
            lblSystemTime.AutoSize = true;
            lblSystemTime.Location = new System.Drawing.Point(23, 28);
            lblSystemTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSystemTime.Name = "lblSystemTime";
            lblSystemTime.Size = new System.Drawing.Size(68, 17);
            lblSystemTime.TabIndex = 0;
            lblSystemTime.Text = "系统时间：";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblLastActivityValue);
            groupBox1.Controls.Add(lblLastActivity);
            groupBox1.Controls.Add(lblPeakConnectionsValue);
            groupBox1.Controls.Add(lblPeakConnections);
            groupBox1.Controls.Add(lblTotalConnectionsValue);
            groupBox1.Controls.Add(lblTotalConnections);
            groupBox1.Controls.Add(lblCurrentConnectionsValue);
            groupBox1.Controls.Add(lblCurrentConnections);
            groupBox1.Controls.Add(lblMaxConnectionsValue);
            groupBox1.Controls.Add(lblMaxConnections);
            groupBox1.Controls.Add(lblServerIpValue);
            groupBox1.Controls.Add(lblServerIp);
            groupBox1.Controls.Add(lblPortValue);
            groupBox1.Controls.Add(lblPort);
            groupBox1.Controls.Add(lblStatusValue);
            groupBox1.Controls.Add(lblStatus);
            groupBox1.Location = new System.Drawing.Point(7, 8);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(447, 278);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "基本服务器信息";
            // 
            // lblLastActivityValue
            // 
            lblLastActivityValue.AutoSize = true;
            lblLastActivityValue.Location = new System.Drawing.Point(117, 241);
            lblLastActivityValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastActivityValue.Name = "lblLastActivityValue";
            lblLastActivityValue.Size = new System.Drawing.Size(31, 17);
            lblLastActivityValue.TabIndex = 15;
            lblLastActivityValue.Text = "N/A";
            // 
            // lblLastActivity
            // 
            lblLastActivity.AutoSize = true;
            lblLastActivity.Location = new System.Drawing.Point(23, 241);
            lblLastActivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastActivity.Name = "lblLastActivity";
            lblLastActivity.Size = new System.Drawing.Size(92, 17);
            lblLastActivity.TabIndex = 14;
            lblLastActivity.Text = "最后活动时间：";
            // 
            // lblPeakConnectionsValue
            // 
            lblPeakConnectionsValue.AutoSize = true;
            lblPeakConnectionsValue.Location = new System.Drawing.Point(117, 205);
            lblPeakConnectionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPeakConnectionsValue.Name = "lblPeakConnectionsValue";
            lblPeakConnectionsValue.Size = new System.Drawing.Size(31, 17);
            lblPeakConnectionsValue.TabIndex = 13;
            lblPeakConnectionsValue.Text = "N/A";
            // 
            // lblPeakConnections
            // 
            lblPeakConnections.AutoSize = true;
            lblPeakConnections.Location = new System.Drawing.Point(23, 205);
            lblPeakConnections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPeakConnections.Name = "lblPeakConnections";
            lblPeakConnections.Size = new System.Drawing.Size(80, 17);
            lblPeakConnections.TabIndex = 12;
            lblPeakConnections.Text = "峰值连接数：";
            // 
            // lblTotalConnectionsValue
            // 
            lblTotalConnectionsValue.AutoSize = true;
            lblTotalConnectionsValue.Location = new System.Drawing.Point(117, 170);
            lblTotalConnectionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalConnectionsValue.Name = "lblTotalConnectionsValue";
            lblTotalConnectionsValue.Size = new System.Drawing.Size(31, 17);
            lblTotalConnectionsValue.TabIndex = 11;
            lblTotalConnectionsValue.Text = "N/A";
            // 
            // lblTotalConnections
            // 
            lblTotalConnections.AutoSize = true;
            lblTotalConnections.Location = new System.Drawing.Point(23, 170);
            lblTotalConnections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalConnections.Name = "lblTotalConnections";
            lblTotalConnections.Size = new System.Drawing.Size(80, 17);
            lblTotalConnections.TabIndex = 10;
            lblTotalConnections.Text = "总连接次数：";
            // 
            // lblCurrentConnectionsValue
            // 
            lblCurrentConnectionsValue.AutoSize = true;
            lblCurrentConnectionsValue.Location = new System.Drawing.Point(117, 135);
            lblCurrentConnectionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCurrentConnectionsValue.Name = "lblCurrentConnectionsValue";
            lblCurrentConnectionsValue.Size = new System.Drawing.Size(31, 17);
            lblCurrentConnectionsValue.TabIndex = 9;
            lblCurrentConnectionsValue.Text = "N/A";
            // 
            // lblCurrentConnections
            // 
            lblCurrentConnections.AutoSize = true;
            lblCurrentConnections.Location = new System.Drawing.Point(23, 135);
            lblCurrentConnections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblCurrentConnections.Name = "lblCurrentConnections";
            lblCurrentConnections.Size = new System.Drawing.Size(80, 17);
            lblCurrentConnections.TabIndex = 8;
            lblCurrentConnections.Text = "当前连接数：";
            // 
            // lblMaxConnectionsValue
            // 
            lblMaxConnectionsValue.AutoSize = true;
            lblMaxConnectionsValue.Location = new System.Drawing.Point(117, 99);
            lblMaxConnectionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMaxConnectionsValue.Name = "lblMaxConnectionsValue";
            lblMaxConnectionsValue.Size = new System.Drawing.Size(31, 17);
            lblMaxConnectionsValue.TabIndex = 7;
            lblMaxConnectionsValue.Text = "N/A";
            // 
            // lblMaxConnections
            // 
            lblMaxConnections.AutoSize = true;
            lblMaxConnections.Location = new System.Drawing.Point(23, 99);
            lblMaxConnections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblMaxConnections.Name = "lblMaxConnections";
            lblMaxConnections.Size = new System.Drawing.Size(80, 17);
            lblMaxConnections.TabIndex = 6;
            lblMaxConnections.Text = "最大连接数：";
            // 
            // lblServerIpValue
            // 
            lblServerIpValue.AutoSize = true;
            lblServerIpValue.Location = new System.Drawing.Point(117, 64);
            lblServerIpValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblServerIpValue.Name = "lblServerIpValue";
            lblServerIpValue.Size = new System.Drawing.Size(31, 17);
            lblServerIpValue.TabIndex = 5;
            lblServerIpValue.Text = "N/A";
            // 
            // lblServerIp
            // 
            lblServerIp.AutoSize = true;
            lblServerIp.Location = new System.Drawing.Point(23, 64);
            lblServerIp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblServerIp.Name = "lblServerIp";
            lblServerIp.Size = new System.Drawing.Size(55, 17);
            lblServerIp.TabIndex = 4;
            lblServerIp.Text = "服务器IP";
            // 
            // lblPortValue
            // 
            lblPortValue.AutoSize = true;
            lblPortValue.Location = new System.Drawing.Point(373, 28);
            lblPortValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPortValue.Name = "lblPortValue";
            lblPortValue.Size = new System.Drawing.Size(31, 17);
            lblPortValue.TabIndex = 3;
            lblPortValue.Text = "N/A";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new System.Drawing.Point(257, 28);
            lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPort.Name = "lblPort";
            lblPort.Size = new System.Drawing.Size(44, 17);
            lblPort.TabIndex = 2;
            lblPort.Text = "端口：";
            // 
            // lblStatusValue
            // 
            lblStatusValue.AutoSize = true;
            lblStatusValue.Location = new System.Drawing.Point(117, 28);
            lblStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatusValue.Name = "lblStatusValue";
            lblStatusValue.Size = new System.Drawing.Size(31, 17);
            lblStatusValue.TabIndex = 1;
            lblStatusValue.Text = "N/A";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new System.Drawing.Point(23, 28);
            lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(44, 17);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "状态：";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblSystemHealthValue);
            groupBox2.Controls.Add(lblSystemHealth);
            groupBox2.Controls.Add(lblRequestsPerSecondValue);
            groupBox2.Controls.Add(lblRequestsPerSecond);
            groupBox2.Controls.Add(lblThroughputValue);
            groupBox2.Controls.Add(lblThroughput);
            groupBox2.Controls.Add(lblConnectionUtilizationValue);
            groupBox2.Controls.Add(lblConnectionUtilization);
            groupBox2.Controls.Add(lblLastHeartbeatCheckValue);
            groupBox2.Controls.Add(lblLastHeartbeatCheck);
            groupBox2.Controls.Add(lblLastCleanupValue);
            groupBox2.Controls.Add(lblLastCleanup);
            groupBox2.Controls.Add(lblHeartbeatFailuresValue);
            groupBox2.Controls.Add(lblHeartbeatFailures);
            groupBox2.Controls.Add(lblTimeoutSessionsValue);
            groupBox2.Controls.Add(lblTimeoutSessions);
            groupBox2.Controls.Add(lblPeakSessionsValue);
            groupBox2.Controls.Add(lblPeakSessions);
            groupBox2.Controls.Add(lblTotalSessionsValue);
            groupBox2.Controls.Add(lblTotalSessions);
            groupBox2.Controls.Add(lblActiveSessionsValue);
            groupBox2.Controls.Add(lblActiveSessions);
            groupBox2.Location = new System.Drawing.Point(17, 294);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(437, 271);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "会话统计";
            // 
            // lblSystemHealthValue
            // 
            lblSystemHealthValue.AutoSize = true;
            lblSystemHealthValue.Location = new System.Drawing.Point(373, 277);
            lblSystemHealthValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSystemHealthValue.Name = "lblSystemHealthValue";
            lblSystemHealthValue.Size = new System.Drawing.Size(31, 17);
            lblSystemHealthValue.TabIndex = 23;
            lblSystemHealthValue.Text = "N/A";
            // 
            // lblSystemHealth
            // 
            lblSystemHealth.AutoSize = true;
            lblSystemHealth.Location = new System.Drawing.Point(257, 277);
            lblSystemHealth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSystemHealth.Name = "lblSystemHealth";
            lblSystemHealth.Size = new System.Drawing.Size(92, 17);
            lblSystemHealth.TabIndex = 22;
            lblSystemHealth.Text = "系统健康状态：";
            // 
            // lblRequestsPerSecondValue
            // 
            lblRequestsPerSecondValue.AutoSize = true;
            lblRequestsPerSecondValue.Location = new System.Drawing.Point(373, 241);
            lblRequestsPerSecondValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRequestsPerSecondValue.Name = "lblRequestsPerSecondValue";
            lblRequestsPerSecondValue.Size = new System.Drawing.Size(31, 17);
            lblRequestsPerSecondValue.TabIndex = 21;
            lblRequestsPerSecondValue.Text = "N/A";
            // 
            // lblRequestsPerSecond
            // 
            lblRequestsPerSecond.AutoSize = true;
            lblRequestsPerSecond.Location = new System.Drawing.Point(257, 241);
            lblRequestsPerSecond.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRequestsPerSecond.Name = "lblRequestsPerSecond";
            lblRequestsPerSecond.Size = new System.Drawing.Size(56, 17);
            lblRequestsPerSecond.TabIndex = 20;
            lblRequestsPerSecond.Text = "请求率：";
            // 
            // lblThroughputValue
            // 
            lblThroughputValue.AutoSize = true;
            lblThroughputValue.Location = new System.Drawing.Point(117, 277);
            lblThroughputValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblThroughputValue.Name = "lblThroughputValue";
            lblThroughputValue.Size = new System.Drawing.Size(31, 17);
            lblThroughputValue.TabIndex = 19;
            lblThroughputValue.Text = "N/A";
            // 
            // lblThroughput
            // 
            lblThroughput.AutoSize = true;
            lblThroughput.Location = new System.Drawing.Point(23, 277);
            lblThroughput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblThroughput.Name = "lblThroughput";
            lblThroughput.Size = new System.Drawing.Size(56, 17);
            lblThroughput.TabIndex = 18;
            lblThroughput.Text = "吞吐量：";
            // 
            // lblConnectionUtilizationValue
            // 
            lblConnectionUtilizationValue.AutoSize = true;
            lblConnectionUtilizationValue.Location = new System.Drawing.Point(117, 241);
            lblConnectionUtilizationValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblConnectionUtilizationValue.Name = "lblConnectionUtilizationValue";
            lblConnectionUtilizationValue.Size = new System.Drawing.Size(31, 17);
            lblConnectionUtilizationValue.TabIndex = 17;
            lblConnectionUtilizationValue.Text = "N/A";
            // 
            // lblConnectionUtilization
            // 
            lblConnectionUtilization.AutoSize = true;
            lblConnectionUtilization.Location = new System.Drawing.Point(23, 241);
            lblConnectionUtilization.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblConnectionUtilization.Name = "lblConnectionUtilization";
            lblConnectionUtilization.Size = new System.Drawing.Size(80, 17);
            lblConnectionUtilization.TabIndex = 16;
            lblConnectionUtilization.Text = "连接利用率：";
            // 
            // lblLastHeartbeatCheckValue
            // 
            lblLastHeartbeatCheckValue.AutoSize = true;
            lblLastHeartbeatCheckValue.Location = new System.Drawing.Point(140, 205);
            lblLastHeartbeatCheckValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastHeartbeatCheckValue.Name = "lblLastHeartbeatCheckValue";
            lblLastHeartbeatCheckValue.Size = new System.Drawing.Size(31, 17);
            lblLastHeartbeatCheckValue.TabIndex = 13;
            lblLastHeartbeatCheckValue.Text = "N/A";
            // 
            // lblLastHeartbeatCheck
            // 
            lblLastHeartbeatCheck.AutoSize = true;
            lblLastHeartbeatCheck.Location = new System.Drawing.Point(23, 205);
            lblLastHeartbeatCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastHeartbeatCheck.Name = "lblLastHeartbeatCheck";
            lblLastHeartbeatCheck.Size = new System.Drawing.Size(92, 17);
            lblLastHeartbeatCheck.TabIndex = 12;
            lblLastHeartbeatCheck.Text = "最后心跳检查：";
            // 
            // lblLastCleanupValue
            // 
            lblLastCleanupValue.AutoSize = true;
            lblLastCleanupValue.Location = new System.Drawing.Point(140, 170);
            lblLastCleanupValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastCleanupValue.Name = "lblLastCleanupValue";
            lblLastCleanupValue.Size = new System.Drawing.Size(31, 17);
            lblLastCleanupValue.TabIndex = 11;
            lblLastCleanupValue.Text = "N/A";
            // 
            // lblLastCleanup
            // 
            lblLastCleanup.AutoSize = true;
            lblLastCleanup.Location = new System.Drawing.Point(23, 170);
            lblLastCleanup.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblLastCleanup.Name = "lblLastCleanup";
            lblLastCleanup.Size = new System.Drawing.Size(92, 17);
            lblLastCleanup.TabIndex = 10;
            lblLastCleanup.Text = "最后清理时间：";
            // 
            // lblHeartbeatFailuresValue
            // 
            lblHeartbeatFailuresValue.AutoSize = true;
            lblHeartbeatFailuresValue.Location = new System.Drawing.Point(140, 135);
            lblHeartbeatFailuresValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHeartbeatFailuresValue.Name = "lblHeartbeatFailuresValue";
            lblHeartbeatFailuresValue.Size = new System.Drawing.Size(31, 17);
            lblHeartbeatFailuresValue.TabIndex = 9;
            lblHeartbeatFailuresValue.Text = "N/A";
            // 
            // lblHeartbeatFailures
            // 
            lblHeartbeatFailures.AutoSize = true;
            lblHeartbeatFailures.Location = new System.Drawing.Point(23, 135);
            lblHeartbeatFailures.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHeartbeatFailures.Name = "lblHeartbeatFailures";
            lblHeartbeatFailures.Size = new System.Drawing.Size(92, 17);
            lblHeartbeatFailures.TabIndex = 8;
            lblHeartbeatFailures.Text = "心跳失败次数：";
            // 
            // lblTimeoutSessionsValue
            // 
            lblTimeoutSessionsValue.AutoSize = true;
            lblTimeoutSessionsValue.Location = new System.Drawing.Point(140, 99);
            lblTimeoutSessionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTimeoutSessionsValue.Name = "lblTimeoutSessionsValue";
            lblTimeoutSessionsValue.Size = new System.Drawing.Size(31, 17);
            lblTimeoutSessionsValue.TabIndex = 7;
            lblTimeoutSessionsValue.Text = "N/A";
            // 
            // lblTimeoutSessions
            // 
            lblTimeoutSessions.AutoSize = true;
            lblTimeoutSessions.Location = new System.Drawing.Point(23, 99);
            lblTimeoutSessions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTimeoutSessions.Name = "lblTimeoutSessions";
            lblTimeoutSessions.Size = new System.Drawing.Size(92, 17);
            lblTimeoutSessions.TabIndex = 6;
            lblTimeoutSessions.Text = "超时会话数量：";
            // 
            // lblPeakSessionsValue
            // 
            lblPeakSessionsValue.AutoSize = true;
            lblPeakSessionsValue.Location = new System.Drawing.Point(140, 64);
            lblPeakSessionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPeakSessionsValue.Name = "lblPeakSessionsValue";
            lblPeakSessionsValue.Size = new System.Drawing.Size(31, 17);
            lblPeakSessionsValue.TabIndex = 5;
            lblPeakSessionsValue.Text = "N/A";
            // 
            // lblPeakSessions
            // 
            lblPeakSessions.AutoSize = true;
            lblPeakSessions.Location = new System.Drawing.Point(23, 64);
            lblPeakSessions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPeakSessions.Name = "lblPeakSessions";
            lblPeakSessions.Size = new System.Drawing.Size(92, 17);
            lblPeakSessions.TabIndex = 4;
            lblPeakSessions.Text = "峰值会话数量：";
            // 
            // lblTotalSessionsValue
            // 
            lblTotalSessionsValue.AutoSize = true;
            lblTotalSessionsValue.Location = new System.Drawing.Point(140, 28);
            lblTotalSessionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalSessionsValue.Name = "lblTotalSessionsValue";
            lblTotalSessionsValue.Size = new System.Drawing.Size(31, 17);
            lblTotalSessionsValue.TabIndex = 3;
            lblTotalSessionsValue.Text = "N/A";
            // 
            // lblTotalSessions
            // 
            lblTotalSessions.AutoSize = true;
            lblTotalSessions.Location = new System.Drawing.Point(23, 28);
            lblTotalSessions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalSessions.Name = "lblTotalSessions";
            lblTotalSessions.Size = new System.Drawing.Size(92, 17);
            lblTotalSessions.TabIndex = 2;
            lblTotalSessions.Text = "总会话连接数：";
            // 
            // lblActiveSessionsValue
            // 
            lblActiveSessionsValue.AutoSize = true;
            lblActiveSessionsValue.Location = new System.Drawing.Point(373, 28);
            lblActiveSessionsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblActiveSessionsValue.Name = "lblActiveSessionsValue";
            lblActiveSessionsValue.Size = new System.Drawing.Size(31, 17);
            lblActiveSessionsValue.TabIndex = 1;
            lblActiveSessionsValue.Text = "N/A";
            // 
            // lblActiveSessions
            // 
            lblActiveSessions.AutoSize = true;
            lblActiveSessions.Location = new System.Drawing.Point(257, 28);
            lblActiveSessions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblActiveSessions.Name = "lblActiveSessions";
            lblActiveSessions.Size = new System.Drawing.Size(92, 17);
            lblActiveSessions.TabIndex = 0;
            lblActiveSessions.Text = "活动会话数量：";
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(cbLblStatus);
            groupBox7.Controls.Add(cbLblStatusValue);
            groupBox7.Controls.Add(cbLblTotalRequests);
            groupBox7.Controls.Add(cbLblTotalRequestsValue);
            groupBox7.Controls.Add(cbLblSuccessRate);
            groupBox7.Controls.Add(cbLblSuccessRateValue);
            groupBox7.Controls.Add(cbLblFailedRequests);
            groupBox7.Controls.Add(cbLblFailedRequestsValue);
            groupBox7.Controls.Add(cbLblStateChanges);
            groupBox7.Controls.Add(cbLblStateChangesValue);
            groupBox7.Controls.Add(cbLblAvgResponseTime);
            groupBox7.Controls.Add(cbLblAvgResponseTimeValue);
            groupBox7.Location = new System.Drawing.Point(479, 306);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new System.Drawing.Size(642, 259);
            groupBox7.TabIndex = 25;
            groupBox7.TabStop = false;
            groupBox7.Text = "熔断器监控";
            // 
            // cbLblStatus
            // 
            cbLblStatus.AutoSize = true;
            cbLblStatus.Location = new System.Drawing.Point(10, 25);
            cbLblStatus.Name = "cbLblStatus";
            cbLblStatus.Size = new System.Drawing.Size(80, 17);
            cbLblStatus.TabIndex = 0;
            cbLblStatus.Text = "熔断器状态：";
            // 
            // cbLblStatusValue
            // 
            cbLblStatusValue.AutoSize = true;
            cbLblStatusValue.Location = new System.Drawing.Point(105, 25);
            cbLblStatusValue.Name = "cbLblStatusValue";
            cbLblStatusValue.Size = new System.Drawing.Size(32, 17);
            cbLblStatusValue.TabIndex = 1;
            cbLblStatusValue.Text = "关闭";
            // 
            // cbLblTotalRequests
            // 
            cbLblTotalRequests.AutoSize = true;
            cbLblTotalRequests.Location = new System.Drawing.Point(200, 25);
            cbLblTotalRequests.Name = "cbLblTotalRequests";
            cbLblTotalRequests.Size = new System.Drawing.Size(68, 17);
            cbLblTotalRequests.TabIndex = 2;
            cbLblTotalRequests.Text = "总请求数：";
            // 
            // cbLblTotalRequestsValue
            // 
            cbLblTotalRequestsValue.AutoSize = true;
            cbLblTotalRequestsValue.Location = new System.Drawing.Point(273, 25);
            cbLblTotalRequestsValue.Name = "cbLblTotalRequestsValue";
            cbLblTotalRequestsValue.Size = new System.Drawing.Size(15, 17);
            cbLblTotalRequestsValue.TabIndex = 3;
            cbLblTotalRequestsValue.Text = "0";
            // 
            // cbLblSuccessRate
            // 
            cbLblSuccessRate.AutoSize = true;
            cbLblSuccessRate.Location = new System.Drawing.Point(350, 25);
            cbLblSuccessRate.Name = "cbLblSuccessRate";
            cbLblSuccessRate.Size = new System.Drawing.Size(56, 17);
            cbLblSuccessRate.TabIndex = 4;
            cbLblSuccessRate.Text = "成功率：";
            // 
            // cbLblSuccessRateValue
            // 
            cbLblSuccessRateValue.AutoSize = true;
            cbLblSuccessRateValue.Location = new System.Drawing.Point(411, 25);
            cbLblSuccessRateValue.Name = "cbLblSuccessRateValue";
            cbLblSuccessRateValue.Size = new System.Drawing.Size(40, 17);
            cbLblSuccessRateValue.TabIndex = 5;
            cbLblSuccessRateValue.Text = "100%";
            // 
            // cbLblFailedRequests
            // 
            cbLblFailedRequests.AutoSize = true;
            cbLblFailedRequests.Location = new System.Drawing.Point(480, 25);
            cbLblFailedRequests.Name = "cbLblFailedRequests";
            cbLblFailedRequests.Size = new System.Drawing.Size(80, 17);
            cbLblFailedRequests.TabIndex = 6;
            cbLblFailedRequests.Text = "失败请求数：";
            // 
            // cbLblFailedRequestsValue
            // 
            cbLblFailedRequestsValue.AutoSize = true;
            cbLblFailedRequestsValue.Location = new System.Drawing.Point(575, 25);
            cbLblFailedRequestsValue.Name = "cbLblFailedRequestsValue";
            cbLblFailedRequestsValue.Size = new System.Drawing.Size(15, 17);
            cbLblFailedRequestsValue.TabIndex = 7;
            cbLblFailedRequestsValue.Text = "0";
            // 
            // cbLblStateChanges
            // 
            cbLblStateChanges.AutoSize = true;
            cbLblStateChanges.Location = new System.Drawing.Point(10, 60);
            cbLblStateChanges.Name = "cbLblStateChanges";
            cbLblStateChanges.Size = new System.Drawing.Size(92, 17);
            cbLblStateChanges.TabIndex = 8;
            cbLblStateChanges.Text = "状态变化次数：";
            // 
            // cbLblStateChangesValue
            // 
            cbLblStateChangesValue.AutoSize = true;
            cbLblStateChangesValue.Location = new System.Drawing.Point(129, 60);
            cbLblStateChangesValue.Name = "cbLblStateChangesValue";
            cbLblStateChangesValue.Size = new System.Drawing.Size(15, 17);
            cbLblStateChangesValue.TabIndex = 9;
            cbLblStateChangesValue.Text = "0";
            // 
            // cbLblAvgResponseTime
            // 
            cbLblAvgResponseTime.AutoSize = true;
            cbLblAvgResponseTime.Location = new System.Drawing.Point(200, 60);
            cbLblAvgResponseTime.Name = "cbLblAvgResponseTime";
            cbLblAvgResponseTime.Size = new System.Drawing.Size(92, 17);
            cbLblAvgResponseTime.TabIndex = 10;
            cbLblAvgResponseTime.Text = "平均响应时间：";
            // 
            // cbLblAvgResponseTimeValue
            // 
            cbLblAvgResponseTimeValue.AutoSize = true;
            cbLblAvgResponseTimeValue.Location = new System.Drawing.Point(295, 60);
            cbLblAvgResponseTimeValue.Name = "cbLblAvgResponseTimeValue";
            cbLblAvgResponseTimeValue.Size = new System.Drawing.Size(32, 17);
            cbLblAvgResponseTimeValue.TabIndex = 11;
            cbLblAvgResponseTimeValue.Text = "0ms";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(groupBox6);
            tabPage2.Controls.Add(groupBox5);
            tabPage2.Controls.Add(groupBox4);
            tabPage2.Location = new System.Drawing.Point(4, 26);
            tabPage2.Margin = new System.Windows.Forms.Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(4);
            tabPage2.Size = new System.Drawing.Size(1159, 935);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "指令处理状态";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(lblTimeoutCommandsValue);
            groupBox6.Controls.Add(lblTimeoutCommands);
            groupBox6.Controls.Add(lblFailedCommandsValue);
            groupBox6.Controls.Add(lblFailedCommands);
            groupBox6.Controls.Add(lblTotalCommandsValue);
            groupBox6.Controls.Add(lblTotalCommands);
            groupBox6.Controls.Add(lblSuccessRateValue);
            groupBox6.Controls.Add(lblSuccessRate);
            groupBox6.Controls.Add(lblHealthStatusValue);
            groupBox6.Controls.Add(lblHealthStatus);
            groupBox6.Location = new System.Drawing.Point(7, 790);
            groupBox6.Margin = new System.Windows.Forms.Padding(4);
            groupBox6.Name = "groupBox6";
            groupBox6.Padding = new System.Windows.Forms.Padding(4);
            groupBox6.Size = new System.Drawing.Size(1143, 142);
            groupBox6.TabIndex = 2;
            groupBox6.TabStop = false;
            groupBox6.Text = "系统健康状态";
            // 
            // lblTimeoutCommandsValue
            // 
            lblTimeoutCommandsValue.AutoSize = true;
            lblTimeoutCommandsValue.Location = new System.Drawing.Point(373, 99);
            lblTimeoutCommandsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTimeoutCommandsValue.Name = "lblTimeoutCommandsValue";
            lblTimeoutCommandsValue.Size = new System.Drawing.Size(31, 17);
            lblTimeoutCommandsValue.TabIndex = 9;
            lblTimeoutCommandsValue.Text = "N/A";
            // 
            // lblTimeoutCommands
            // 
            lblTimeoutCommands.AutoSize = true;
            lblTimeoutCommands.Location = new System.Drawing.Point(257, 99);
            lblTimeoutCommands.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTimeoutCommands.Name = "lblTimeoutCommands";
            lblTimeoutCommands.Size = new System.Drawing.Size(80, 17);
            lblTimeoutCommands.TabIndex = 8;
            lblTimeoutCommands.Text = "超时命令数：";
            // 
            // lblFailedCommandsValue
            // 
            lblFailedCommandsValue.AutoSize = true;
            lblFailedCommandsValue.Location = new System.Drawing.Point(373, 64);
            lblFailedCommandsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblFailedCommandsValue.Name = "lblFailedCommandsValue";
            lblFailedCommandsValue.Size = new System.Drawing.Size(31, 17);
            lblFailedCommandsValue.TabIndex = 7;
            lblFailedCommandsValue.Text = "N/A";
            // 
            // lblFailedCommands
            // 
            lblFailedCommands.AutoSize = true;
            lblFailedCommands.Location = new System.Drawing.Point(257, 64);
            lblFailedCommands.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblFailedCommands.Name = "lblFailedCommands";
            lblFailedCommands.Size = new System.Drawing.Size(80, 17);
            lblFailedCommands.TabIndex = 6;
            lblFailedCommands.Text = "失败命令数：";
            // 
            // lblTotalCommandsValue
            // 
            lblTotalCommandsValue.AutoSize = true;
            lblTotalCommandsValue.Location = new System.Drawing.Point(373, 28);
            lblTotalCommandsValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalCommandsValue.Name = "lblTotalCommandsValue";
            lblTotalCommandsValue.Size = new System.Drawing.Size(31, 17);
            lblTotalCommandsValue.TabIndex = 5;
            lblTotalCommandsValue.Text = "N/A";
            // 
            // lblTotalCommands
            // 
            lblTotalCommands.AutoSize = true;
            lblTotalCommands.Location = new System.Drawing.Point(257, 28);
            lblTotalCommands.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalCommands.Name = "lblTotalCommands";
            lblTotalCommands.Size = new System.Drawing.Size(68, 17);
            lblTotalCommands.TabIndex = 4;
            lblTotalCommands.Text = "总命令数：";
            // 
            // lblSuccessRateValue
            // 
            lblSuccessRateValue.AutoSize = true;
            lblSuccessRateValue.Location = new System.Drawing.Point(117, 99);
            lblSuccessRateValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSuccessRateValue.Name = "lblSuccessRateValue";
            lblSuccessRateValue.Size = new System.Drawing.Size(31, 17);
            lblSuccessRateValue.TabIndex = 3;
            lblSuccessRateValue.Text = "N/A";
            // 
            // lblSuccessRate
            // 
            lblSuccessRate.AutoSize = true;
            lblSuccessRate.Location = new System.Drawing.Point(23, 99);
            lblSuccessRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSuccessRate.Name = "lblSuccessRate";
            lblSuccessRate.Size = new System.Drawing.Size(56, 17);
            lblSuccessRate.TabIndex = 2;
            lblSuccessRate.Text = "成功率：";
            // 
            // lblHealthStatusValue
            // 
            lblHealthStatusValue.AutoSize = true;
            lblHealthStatusValue.Location = new System.Drawing.Point(117, 28);
            lblHealthStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHealthStatusValue.Name = "lblHealthStatusValue";
            lblHealthStatusValue.Size = new System.Drawing.Size(31, 17);
            lblHealthStatusValue.TabIndex = 1;
            lblHealthStatusValue.Text = "N/A";
            // 
            // lblHealthStatus
            // 
            lblHealthStatus.AutoSize = true;
            lblHealthStatus.Location = new System.Drawing.Point(23, 28);
            lblHealthStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHealthStatus.Name = "lblHealthStatus";
            lblHealthStatus.Size = new System.Drawing.Size(68, 17);
            lblHealthStatus.TabIndex = 0;
            lblHealthStatus.Text = "健康状态：";
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(dgvHandlerStatistics);
            groupBox5.Location = new System.Drawing.Point(7, 150);
            groupBox5.Margin = new System.Windows.Forms.Padding(4);
            groupBox5.Name = "groupBox5";
            groupBox5.Padding = new System.Windows.Forms.Padding(4);
            groupBox5.Size = new System.Drawing.Size(1143, 632);
            groupBox5.TabIndex = 1;
            groupBox5.TabStop = false;
            groupBox5.Text = "处理器统计信息";
            // 
            // dgvHandlerStatistics
            // 
            dgvHandlerStatistics.AllowUserToAddRows = false;
            dgvHandlerStatistics.AllowUserToDeleteRows = false;
            dgvHandlerStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHandlerStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { HandlerName, Status, Priority, TotalCommands, SuccessfulCommands, FailedCommands, TimeoutCount, CurrentProcessing, AvgProcessingTime, MaxProcessingTime });
            dgvHandlerStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvHandlerStatistics.Location = new System.Drawing.Point(4, 20);
            dgvHandlerStatistics.Margin = new System.Windows.Forms.Padding(4);
            dgvHandlerStatistics.Name = "dgvHandlerStatistics";
            dgvHandlerStatistics.ReadOnly = true;
            dgvHandlerStatistics.RowTemplate.Height = 23;
            dgvHandlerStatistics.Size = new System.Drawing.Size(1135, 608);
            dgvHandlerStatistics.TabIndex = 0;
            // 
            // HandlerName
            // 
            HandlerName.HeaderText = "处理器名称";
            HandlerName.Name = "HandlerName";
            HandlerName.ReadOnly = true;
            // 
            // Status
            // 
            Status.HeaderText = "状态";
            Status.Name = "Status";
            Status.ReadOnly = true;
            // 
            // Priority
            // 
            Priority.HeaderText = "优先级";
            Priority.Name = "Priority";
            Priority.ReadOnly = true;
            // 
            // TotalCommands
            // 
            TotalCommands.HeaderText = "总命令数";
            TotalCommands.Name = "TotalCommands";
            TotalCommands.ReadOnly = true;
            // 
            // SuccessfulCommands
            // 
            SuccessfulCommands.HeaderText = "成功命令数";
            SuccessfulCommands.Name = "SuccessfulCommands";
            SuccessfulCommands.ReadOnly = true;
            // 
            // FailedCommands
            // 
            FailedCommands.HeaderText = "失败命令数";
            FailedCommands.Name = "FailedCommands";
            FailedCommands.ReadOnly = true;
            // 
            // TimeoutCount
            // 
            TimeoutCount.HeaderText = "超时次数";
            TimeoutCount.Name = "TimeoutCount";
            TimeoutCount.ReadOnly = true;
            // 
            // CurrentProcessing
            // 
            CurrentProcessing.HeaderText = "当前处理数";
            CurrentProcessing.Name = "CurrentProcessing";
            CurrentProcessing.ReadOnly = true;
            // 
            // AvgProcessingTime
            // 
            AvgProcessingTime.HeaderText = "平均处理时间(ms)";
            AvgProcessingTime.Name = "AvgProcessingTime";
            AvgProcessingTime.ReadOnly = true;
            // 
            // MaxProcessingTime
            // 
            MaxProcessingTime.HeaderText = "最大处理时间(ms)";
            MaxProcessingTime.Name = "MaxProcessingTime";
            MaxProcessingTime.ReadOnly = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(lblHandlerCountValue);
            groupBox4.Controls.Add(lblHandlerCount);
            groupBox4.Controls.Add(lblDispatcherInitializedValue);
            groupBox4.Controls.Add(lblDispatcherInitialized);
            groupBox4.Location = new System.Drawing.Point(7, 8);
            groupBox4.Margin = new System.Windows.Forms.Padding(4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new System.Windows.Forms.Padding(4);
            groupBox4.Size = new System.Drawing.Size(1143, 124);
            groupBox4.TabIndex = 0;
            groupBox4.TabStop = false;
            groupBox4.Text = "命令调度器信息";
            // 
            // lblHandlerCountValue
            // 
            lblHandlerCountValue.AutoSize = true;
            lblHandlerCountValue.Location = new System.Drawing.Point(117, 64);
            lblHandlerCountValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHandlerCountValue.Name = "lblHandlerCountValue";
            lblHandlerCountValue.Size = new System.Drawing.Size(31, 17);
            lblHandlerCountValue.TabIndex = 3;
            lblHandlerCountValue.Text = "N/A";
            // 
            // lblHandlerCount
            // 
            lblHandlerCount.AutoSize = true;
            lblHandlerCount.Location = new System.Drawing.Point(23, 64);
            lblHandlerCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblHandlerCount.Name = "lblHandlerCount";
            lblHandlerCount.Size = new System.Drawing.Size(80, 17);
            lblHandlerCount.TabIndex = 2;
            lblHandlerCount.Text = "处理器数量：";
            // 
            // lblDispatcherInitializedValue
            // 
            lblDispatcherInitializedValue.AutoSize = true;
            lblDispatcherInitializedValue.Location = new System.Drawing.Point(117, 28);
            lblDispatcherInitializedValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDispatcherInitializedValue.Name = "lblDispatcherInitializedValue";
            lblDispatcherInitializedValue.Size = new System.Drawing.Size(31, 17);
            lblDispatcherInitializedValue.TabIndex = 1;
            lblDispatcherInitializedValue.Text = "N/A";
            // 
            // lblDispatcherInitialized
            // 
            lblDispatcherInitialized.AutoSize = true;
            lblDispatcherInitialized.Location = new System.Drawing.Point(23, 28);
            lblDispatcherInitialized.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblDispatcherInitialized.Name = "lblDispatcherInitialized";
            lblDispatcherInitialized.Size = new System.Drawing.Size(80, 17);
            lblDispatcherInitialized.TabIndex = 0;
            lblDispatcherInitialized.Text = "是否已初始化";
            // 
            // ServerMonitorControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "ServerMonitorControl";
            Size = new System.Drawing.Size(1167, 965);
            Load += ServerMonitorControl_Load;
            Disposed += ServerMonitorControl_Disposed;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panelTop.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            tabPage2.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox6.PerformLayout();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvHandlerStatistics).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblHealthStatusValue;
        private System.Windows.Forms.Label lblHealthStatus;
        private System.Windows.Forms.Label lblSuccessRateValue;
        private System.Windows.Forms.Label lblSuccessRate;
        private System.Windows.Forms.Label lblTotalCommandsValue;
        private System.Windows.Forms.Label lblTotalCommands;
        private System.Windows.Forms.Label lblFailedCommandsValue;
        private System.Windows.Forms.Label lblFailedCommands;
        private System.Windows.Forms.Label lblTimeoutCommandsValue;
        private System.Windows.Forms.Label lblTimeoutCommands;
        private System.Windows.Forms.Label lblTotalHandlersValue;
        private System.Windows.Forms.Label lblTotalHandlers;
        private System.Windows.Forms.Label lblActiveHandlersValue;
        private System.Windows.Forms.Label lblActiveHandlers;
        private System.Windows.Forms.Label lblCurrentProcessingValue;
        private System.Windows.Forms.Label lblCurrentProcessing;
        private System.Windows.Forms.Label lblRealTimeSuccessRateValue;
        private System.Windows.Forms.Label lblRealTimeSuccessRate;
        private System.Windows.Forms.Label lblAvgProcessingTimeValue;
        private System.Windows.Forms.Label lblAvgProcessingTime;
        private System.Windows.Forms.Button btnErrorReport;
        private System.Windows.Forms.Button btnPerformanceReport;
        private System.Windows.Forms.Button btnDiagnosticsReport;
       
    }
}
