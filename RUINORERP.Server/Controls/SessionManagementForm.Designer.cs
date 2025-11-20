namespace RUINORERP.Server.Controls
{
    partial class SessionManagementForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabBasicInfo = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.picSessionStatus = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSessionID = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblClientIP = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblClientPort = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblConnectedTime = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblConnectionDuration = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblClientVersion = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblIdleTime = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblHeartbeatCount = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblLastHeartbeat = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.lblThroughput = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.tabSystemInfo = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblIs64Bit = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.lblArchitecture = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.lblMachineName = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.lblOSName = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblCPUMaxSpeed = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.lblCPULogicalProcessors = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblCPUCores = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.lblCPUName = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblAvailableMemory = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.lblTotalMemory = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.lblMacAddress = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.tabPerformance = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lblReceivedPackets = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.lblSentPackets = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.lblTotalBytesReceived = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.lblTotalBytesSent = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.lblBandwidth = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.lblPacketLoss = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.lblLatency = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.lblRemoteIP = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.lblLocalIP = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.picNetworkStatus = new System.Windows.Forms.PictureBox();
            this.lblNetworkStatus = new System.Windows.Forms.Label();
            this.tabCache = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.lblCacheStatus = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.lblCacheHitRate = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.lblCacheSize = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.lblCacheEntries = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.txtMessageHistory = new System.Windows.Forms.RichTextBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.btnExportSessionInfo = new System.Windows.Forms.Button();
            this.btnForceSync = new System.Windows.Forms.Button();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.btnRestartClient = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1.SuspendLayout();
            this.tabBasicInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSessionStatus)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabSystemInfo.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabPerformance.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNetworkStatus)).BeginInit();
            this.tabCache.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabBasicInfo);
            this.tabControl1.Controls.Add(this.tabSystemInfo);
            this.tabControl1.Controls.Add(this.tabPerformance);
            this.tabControl1.Controls.Add(this.tabCache);
            this.tabControl1.Controls.Add(this.tabControl);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabBasicInfo
            // 
            this.tabBasicInfo.Controls.Add(this.groupBox2);
            this.tabBasicInfo.Controls.Add(this.groupBox1);
            this.tabBasicInfo.Location = new System.Drawing.Point(4, 22);
            this.tabBasicInfo.Name = "tabBasicInfo";
            this.tabBasicInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabBasicInfo.Size = new System.Drawing.Size(792, 574);
            this.tabBasicInfo.TabIndex = 0;
            this.tabBasicInfo.Text = "基本信息";
            this.tabBasicInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblIdleTime);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lblClientVersion);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.lblConnectionDuration);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.lblConnectedTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lblClientPort);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lblClientIP);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblSessionID);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblUserName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.picSessionStatus);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(776, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "会话基本信息";
            // 
            // picSessionStatus
            // 
            this.picSessionStatus.Location = new System.Drawing.Point(6, 20);
            this.picSessionStatus.Name = "picSessionStatus";
            this.picSessionStatus.Size = new System.Drawing.Size(16, 16);
            this.picSessionStatus.TabIndex = 0;
            this.picSessionStatus.TabStop = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(28, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户名：";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(70, 45);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(47, 13);
            this.lblUserName.TabIndex = 3;
            this.lblUserName.Text = "label3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "会话ID：";
            // 
            // lblSessionID
            // 
            this.lblSessionID.AutoSize = true;
            this.lblSessionID.Location = new System.Drawing.Point(70, 65);
            this.lblSessionID.Name = "lblSessionID";
            this.lblSessionID.Size = new System.Drawing.Size(47, 13);
            this.lblSessionID.TabIndex = 5;
            this.lblSessionID.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "客户端IP：";
            // 
            // lblClientIP
            // 
            this.lblClientIP.AutoSize = true;
            this.lblClientIP.Location = new System.Drawing.Point(70, 85);
            this.lblClientIP.Name = "lblClientIP";
            this.lblClientIP.Size = new System.Drawing.Size(47, 13);
            this.lblClientIP.TabIndex = 7;
            this.lblClientIP.Text = "label7";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "客户端端口：";
            // 
            // lblClientPort
            // 
            this.lblClientPort.AutoSize = true;
            this.lblClientPort.Location = new System.Drawing.Point(70, 105);
            this.lblClientPort.Name = "lblClientPort";
            this.lblClientPort.Size = new System.Drawing.Size(47, 13);
            this.lblClientPort.TabIndex = 9;
            this.lblClientPort.Text = "label9";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(300, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "连接时间：";
            // 
            // lblConnectedTime
            // 
            this.lblConnectedTime.AutoSize = true;
            this.lblConnectedTime.Location = new System.Drawing.Point(364, 45);
            this.lblConnectedTime.Name = "lblConnectedTime";
            this.lblConnectedTime.Size = new System.Drawing.Size(47, 13);
            this.lblConnectedTime.TabIndex = 11;
            this.lblConnectedTime.Text = "label11";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(300, 65);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "连接时长：";
            // 
            // lblConnectionDuration
            // 
            this.lblConnectionDuration.AutoSize = true;
            this.lblConnectionDuration.Location = new System.Drawing.Point(364, 65);
            this.lblConnectionDuration.Name = "lblConnectionDuration";
            this.lblConnectionDuration.Size = new System.Drawing.Size(47, 13);
            this.lblConnectionDuration.TabIndex = 13;
            this.lblConnectionDuration.Text = "label13";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(300, 85);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "客户端版本：";
            // 
            // lblClientVersion
            // 
            this.lblClientVersion.AutoSize = true;
            this.lblClientVersion.Location = new System.Drawing.Point(364, 85);
            this.lblClientVersion.Name = "lblClientVersion";
            this.lblClientVersion.Size = new System.Drawing.Size(47, 13);
            this.lblClientVersion.TabIndex = 15;
            this.lblClientVersion.Text = "label15";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(300, 105);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(58, 13);
            this.label16.TabIndex = 16;
            this.label16.Text = "空闲时间：";
            // 
            // lblIdleTime
            // 
            this.lblIdleTime.AutoSize = true;
            this.lblIdleTime.Location = new System.Drawing.Point(364, 105);
            this.lblIdleTime.Name = "lblIdleTime";
            this.lblIdleTime.Size = new System.Drawing.Size(47, 13);
            this.lblIdleTime.TabIndex = 17;
            this.lblIdleTime.Text = "label17";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblThroughput);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.lblLastHeartbeat);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.lblHeartbeatCount);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Location = new System.Drawing.Point(8, 162);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "连接统计";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 25);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(58, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "心跳计数：";
            // 
            // lblHeartbeatCount
            // 
            this.lblHeartbeatCount.AutoSize = true;
            this.lblHeartbeatCount.Location = new System.Drawing.Point(70, 25);
            this.lblHeartbeatCount.Name = "lblHeartbeatCount";
            this.lblHeartbeatCount.Size = new System.Drawing.Size(47, 13);
            this.lblHeartbeatCount.TabIndex = 1;
            this.lblHeartbeatCount.Text = "label19";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(300, 25);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(58, 13);
            this.label20.TabIndex = 2;
            this.label20.Text = "最后心跳：";
            // 
            // lblLastHeartbeat
            // 
            this.lblLastHeartbeat.AutoSize = true;
            this.lblLastHeartbeat.Location = new System.Drawing.Point(364, 25);
            this.lblLastHeartbeat.Name = "lblLastHeartbeat";
            this.lblLastHeartbeat.Size = new System.Drawing.Size(47, 13);
            this.lblLastHeartbeat.TabIndex = 3;
            this.lblLastHeartbeat.Text = "label21";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 50);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(58, 13);
            this.label22.TabIndex = 4;
            this.label22.Text = "实时吞吐量：";
            // 
            // lblThroughput
            // 
            this.lblThroughput.AutoSize = true;
            this.lblThroughput.Location = new System.Drawing.Point(70, 50);
            this.lblThroughput.Name = "lblThroughput";
            this.lblThroughput.Size = new System.Drawing.Size(47, 13);
            this.lblThroughput.TabIndex = 5;
            this.lblThroughput.Text = "label23";
            // 
            // tabSystemInfo
            // 
            this.tabSystemInfo.Controls.Add(this.groupBox5);
            this.tabSystemInfo.Controls.Add(this.groupBox4);
            this.tabSystemInfo.Controls.Add(this.groupBox3);
            this.tabSystemInfo.Location = new System.Drawing.Point(4, 22);
            this.tabSystemInfo.Name = "tabSystemInfo";
            this.tabSystemInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabSystemInfo.Size = new System.Drawing.Size(792, 574);
            this.tabSystemInfo.TabIndex = 1;
            this.tabSystemInfo.Text = "系统信息";
            this.tabSystemInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblIs64Bit);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Controls.Add(this.lblArchitecture);
            this.groupBox3.Controls.Add(this.label26);
            this.groupBox3.Controls.Add(this.lblMachineName);
            this.groupBox3.Controls.Add(this.label28);
            this.groupBox3.Controls.Add(this.lblOSName);
            this.groupBox3.Controls.Add(this.label30);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 120);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作系统信息";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 25);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(58, 13);
            this.label30.TabIndex = 0;
            this.label30.Text = "操作系统：";
            // 
            // lblOSName
            // 
            this.lblOSName.AutoSize = true;
            this.lblOSName.Location = new System.Drawing.Point(70, 25);
            this.lblOSName.Name = "lblOSName";
            this.lblOSName.Size = new System.Drawing.Size(47, 13);
            this.lblOSName.TabIndex = 1;
            this.lblOSName.Text = "label31";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 50);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(58, 13);
            this.label28.TabIndex = 2;
            this.label28.Text = "计算机名：";
            // 
            // lblMachineName
            // 
            this.lblMachineName.AutoSize = true;
            this.lblMachineName.Location = new System.Drawing.Point(70, 50);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new System.Drawing.Size(47, 13);
            this.lblMachineName.TabIndex = 3;
            this.lblMachineName.Text = "label29";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(300, 25);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(58, 13);
            this.label26.TabIndex = 4;
            this.label26.Text = "系统架构：";
            // 
            // lblArchitecture
            // 
            this.lblArchitecture.AutoSize = true;
            this.lblArchitecture.Location = new System.Drawing.Point(364, 25);
            this.lblArchitecture.Name = "lblArchitecture";
            this.lblArchitecture.Size = new System.Drawing.Size(47, 13);
            this.lblArchitecture.TabIndex = 5;
            this.lblArchitecture.Text = "label27";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(300, 50);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(58, 13);
            this.label24.TabIndex = 6;
            this.label24.Text = "64位系统：";
            // 
            // lblIs64Bit
            // 
            this.lblIs64Bit.AutoSize = true;
            this.lblIs64Bit.Location = new System.Drawing.Point(364, 50);
            this.lblIs64Bit.Name = "lblIs64Bit";
            this.lblIs64Bit.Size = new System.Drawing.Size(47, 13);
            this.lblIs64Bit.TabIndex = 7;
            this.lblIs64Bit.Text = "label25";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblCPUMaxSpeed);
            this.groupBox4.Controls.Add(this.label32);
            this.groupBox4.Controls.Add(this.lblCPULogicalProcessors);
            this.groupBox4.Controls.Add(this.label34);
            this.groupBox4.Controls.Add(this.lblCPUCores);
            this.groupBox4.Controls.Add(this.label36);
            this.groupBox4.Controls.Add(this.lblCPUName);
            this.groupBox4.Controls.Add(this.label38);
            this.groupBox4.Location = new System.Drawing.Point(8, 132);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(776, 120);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "处理器信息";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(6, 25);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(58, 13);
            this.label38.TabIndex = 0;
            this.label38.Text = "处理器：";
            // 
            // lblCPUName
            // 
            this.lblCPUName.AutoSize = true;
            this.lblCPUName.Location = new System.Drawing.Point(70, 25);
            this.lblCPUName.Name = "lblCPUName";
            this.lblCPUName.Size = new System.Drawing.Size(47, 13);
            this.lblCPUName.TabIndex = 1;
            this.lblCPUName.Text = "label39";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(6, 50);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(58, 13);
            this.label36.TabIndex = 2;
            this.label36.Text = "核心数：";
            // 
            // lblCPUCores
            // 
            this.lblCPUCores.AutoSize = true;
            this.lblCPUCores.Location = new System.Drawing.Point(70, 50);
            this.lblCPUCores.Name = "lblCPUCores";
            this.lblCPUCores.Size = new System.Drawing.Size(47, 13);
            this.lblCPUCores.TabIndex = 3;
            this.lblCPUCores.Text = "label37";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(300, 25);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(58, 13);
            this.label34.TabIndex = 4;
            this.label34.Text = "逻辑处理器：";
            // 
            // lblCPULogicalProcessors
            // 
            this.lblCPULogicalProcessors.AutoSize = true;
            this.lblCPULogicalProcessors.Location = new System.Drawing.Point(364, 25);
            this.lblCPULogicalProcessors.Name = "lblCPULogicalProcessors";
            this.lblCPULogicalProcessors.Size = new System.Drawing.Size(47, 13);
            this.lblCPULogicalProcessors.TabIndex = 5;
            this.lblCPULogicalProcessors.Text = "label35";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(300, 50);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(58, 13);
            this.label32.TabIndex = 6;
            this.label32.Text = "最大频率：";
            // 
            // lblCPUMaxSpeed
            // 
            this.lblCPUMaxSpeed.AutoSize = true;
            this.lblCPUMaxSpeed.Location = new System.Drawing.Point(364, 50);
            this.lblCPUMaxSpeed.Name = "lblCPUMaxSpeed";
            this.lblCPUMaxSpeed.Size = new System.Drawing.Size(47, 13);
            this.lblCPUMaxSpeed.TabIndex = 7;
            this.lblCPUMaxSpeed.Text = "label33";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblMacAddress);
            this.groupBox5.Controls.Add(this.label44);
            this.groupBox5.Controls.Add(this.lblAvailableMemory);
            this.groupBox5.Controls.Add(this.label40);
            this.groupBox5.Controls.Add(this.lblTotalMemory);
            this.groupBox5.Controls.Add(this.label42);
            this.groupBox5.Location = new System.Drawing.Point(8, 258);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(776, 80);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "内存信息";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(6, 25);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(58, 13);
            this.label42.TabIndex = 0;
            this.label42.Text = "总内存：";
            // 
            // lblTotalMemory
            // 
            this.lblTotalMemory.AutoSize = true;
            this.lblTotalMemory.Location = new System.Drawing.Point(70, 25);
            this.lblTotalMemory.Name = "lblTotalMemory";
            this.lblTotalMemory.Size = new System.Drawing.Size(47, 13);
            this.lblTotalMemory.TabIndex = 1;
            this.lblTotalMemory.Text = "label43";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(300, 25);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(58, 13);
            this.label40.TabIndex = 2;
            this.label40.Text = "可用内存：";
            // 
            // lblAvailableMemory
            // 
            this.lblAvailableMemory.AutoSize = true;
            this.lblAvailableMemory.Location = new System.Drawing.Point(364, 25);
            this.lblAvailableMemory.Name = "lblAvailableMemory";
            this.lblAvailableMemory.Size = new System.Drawing.Size(47, 13);
            this.lblAvailableMemory.TabIndex = 3;
            this.lblAvailableMemory.Text = "label41";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(6, 50);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(58, 13);
            this.label44.TabIndex = 4;
            this.label44.Text = "MAC地址：";
            // 
            // lblMacAddress
            // 
            this.lblMacAddress.AutoSize = true;
            this.lblMacAddress.Location = new System.Drawing.Point(70, 50);
            this.lblMacAddress.Name = "lblMacAddress";
            this.lblMacAddress.Size = new System.Drawing.Size(47, 13);
            this.lblMacAddress.TabIndex = 5;
            this.lblMacAddress.Text = "label45";
            // 
            // tabPerformance
            // 
            this.tabPerformance.Controls.Add(this.groupBox7);
            this.tabPerformance.Controls.Add(this.groupBox6);
            this.tabPerformance.Location = new System.Drawing.Point(4, 22);
            this.tabPerformance.Name = "tabPerformance";
            this.tabPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerformance.Size = new System.Drawing.Size(792, 574);
            this.tabPerformance.TabIndex = 2;
            this.tabPerformance.Text = "性能监控";
            this.tabPerformance.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lblTotalBytesSent);
            this.groupBox6.Controls.Add(this.label52);
            this.groupBox6.Controls.Add(this.lblTotalBytesReceived);
            this.groupBox6.Controls.Add(this.label50);
            this.groupBox6.Controls.Add(this.lblSentPackets);
            this.groupBox6.Controls.Add(this.label48);
            this.groupBox6.Controls.Add(this.lblReceivedPackets);
            this.groupBox6.Controls.Add(this.label46);
            this.groupBox6.Location = new System.Drawing.Point(8, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(776, 120);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "网络传输统计";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(6, 25);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(58, 13);
            this.label46.TabIndex = 0;
            this.label46.Text = "接收数据包：";
            // 
            // lblReceivedPackets
            // 
            this.lblReceivedPackets.AutoSize = true;
            this.lblReceivedPackets.Location = new System.Drawing.Point(70, 25);
            this.lblReceivedPackets.Name = "lblReceivedPackets";
            this.lblReceivedPackets.Size = new System.Drawing.Size(47, 13);
            this.lblReceivedPackets.TabIndex = 1;
            this.lblReceivedPackets.Text = "label47";
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(300, 25);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(58, 13);
            this.label48.TabIndex = 2;
            this.label48.Text = "发送数据包：";
            // 
            // lblSentPackets
            // 
            this.lblSentPackets.AutoSize = true;
            this.lblSentPackets.Location = new System.Drawing.Point(364, 25);
            this.lblSentPackets.Name = "lblSentPackets";
            this.lblSentPackets.Size = new System.Drawing.Size(47, 13);
            this.lblSentPackets.TabIndex = 3;
            this.lblSentPackets.Text = "label49";
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(6, 50);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(58, 13);
            this.label50.TabIndex = 4;
            this.label50.Text = "接收字节：";
            // 
            // lblTotalBytesReceived
            // 
            this.lblTotalBytesReceived.AutoSize = true;
            this.lblTotalBytesReceived.Location = new System.Drawing.Point(70, 50);
            this.lblTotalBytesReceived.Name = "lblTotalBytesReceived";
            this.lblTotalBytesReceived.Size = new System.Drawing.Size(47, 13);
            this.lblTotalBytesReceived.TabIndex = 5;
            this.lblTotalBytesReceived.Text = "label51";
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(300, 50);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(58, 13);
            this.label52.TabIndex = 6;
            this.label52.Text = "发送字节：";
            // 
            // lblTotalBytesSent
            // 
            this.lblTotalBytesSent.AutoSize = true;
            this.lblTotalBytesSent.Location = new System.Drawing.Point(364, 50);
            this.lblTotalBytesSent.Name = "lblTotalBytesSent";
            this.lblTotalBytesSent.Size = new System.Drawing.Size(47, 13);
            this.lblTotalBytesSent.TabIndex = 7;
            this.lblTotalBytesSent.Text = "label53";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.lblNetworkStatus);
            this.groupBox7.Controls.Add(this.picNetworkStatus);
            this.groupBox7.Controls.Add(this.lblLocalIP);
            this.groupBox7.Controls.Add(this.label66);
            this.groupBox7.Controls.Add(this.lblLocalPort);
            this.groupBox7.Controls.Add(this.label64);
            this.groupBox7.Controls.Add(this.lblRemoteIP);
            this.groupBox7.Controls.Add(this.label62);
            this.groupBox7.Controls.Add(this.lblRemotePort);
            this.groupBox7.Controls.Add(this.label60);
            this.groupBox7.Controls.Add(this.lblLatency);
            this.groupBox7.Controls.Add(this.label58);
            this.groupBox7.Controls.Add(this.lblPacketLoss);
            this.groupBox7.Controls.Add(this.label56);
            this.groupBox7.Controls.Add(this.lblBandwidth);
            this.groupBox7.Controls.Add(this.label54);
            this.groupBox7.Location = new System.Drawing.Point(8, 132);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(776, 150);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "网络质量监控";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Location = new System.Drawing.Point(6, 25);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(58, 13);
            this.label54.TabIndex = 0;
            this.label54.Text = "可用带宽：";
            // 
            // lblBandwidth
            // 
            this.lblBandwidth.AutoSize = true;
            this.lblBandwidth.Location = new System.Drawing.Point(70, 25);
            this.lblBandwidth.Name = "lblBandwidth";
            this.lblBandwidth.Size = new System.Drawing.Size(47, 13);
            this.lblBandwidth.TabIndex = 1;
            this.lblBandwidth.Text = "label55";
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Location = new System.Drawing.Point(300, 25);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(58, 13);
            this.label56.TabIndex = 2;
            this.label56.Text = "丢包率：";
            // 
            // lblPacketLoss
            // 
            this.lblPacketLoss.AutoSize = true;
            this.lblPacketLoss.Location = new System.Drawing.Point(364, 25);
            this.lblPacketLoss.Name = "lblPacketLoss";
            this.lblPacketLoss.Size = new System.Drawing.Size(47, 13);
            this.lblPacketLoss.TabIndex = 3;
            this.lblPacketLoss.Text = "label57";
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.Location = new System.Drawing.Point(6, 50);
            this.label58.Name = "label58";
            this.label18.TabIndex = 4;
            this.label58.Text = "延迟：";
            // 
            // lblLatency
            // 
            this.lblLatency.AutoSize = true;
            this.lblLatency.Location = new System.Drawing.Point(70, 50);
            this.lblLatency.Name = "lblLatency";
            this.lblLatency.Size = new System.Drawing.Size(47, 13);
            this.lblLatency.TabIndex = 5;
            this.lblLatency.Text = "label59";
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Location = new System.Drawing.Point(6, 75);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(58, 13);
            this.label60.TabIndex = 6;
            this.label60.Text = "远程端口：";
            // 
            // lblRemotePort
            // 
            this.lblRemotePort.AutoSize = true;
            this.lblRemotePort.Location = new System.Drawing.Point(70, 75);
            this.lblRemotePort.Name = "lblRemotePort";
            this.lblRemotePort.Size = new System.Drawing.Size(47, 13);
            this.lblRemotePort.TabIndex = 7;
            this.lblRemotePort.Text = "label61";
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.Location = new System.Drawing.Point(300, 75);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(58, 13);
            this.label62.TabIndex = 8;
            this.label62.Text = "远程IP：";
            // 
            // lblRemoteIP
            // 
            this.lblRemoteIP.AutoSize = true;
            this.lblRemoteIP.Location = new System.Drawing.Point(364, 75);
            this.lblRemoteIP.Name = "lblRemoteIP";
            this.lblRemoteIP.Size = new System.Drawing.Size(47, 13);
            this.lblRemoteIP.TabIndex = 9;
            this.lblRemoteIP.Text = "label63";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Location = new System.Drawing.Point(6, 100);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(58, 13);
            this.label64.TabIndex = 10;
            this.label64.Text = "本地端口：";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(70, 100);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(47, 13);
            this.lblLocalPort.TabIndex = 11;
            this.lblLocalPort.Text = "label65";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(300, 100);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(58, 13);
            this.label66.TabIndex = 12;
            this.label66.Text = "本地IP：";
            // 
            // lblLocalIP
            // 
            this.lblLocalIP.AutoSize = true;
            this.lblLocalIP.Location = new System.Drawing.Point(364, 100);
            this.lblLocalIP.Name = "lblLocalIP";
            this.lblLocalIP.Size = new System.Drawing.Size(47, 13);
            this.lblLocalIP.TabIndex = 13;
            this.lblLocalIP.Text = "label67";
            // 
            // picNetworkStatus
            // 
            this.picNetworkStatus.Location = new System.Drawing.Point(6, 125);
            this.picNetworkStatus.Name = "picNetworkStatus";
            this.picNetworkStatus.Size = new System.Drawing.Size(16, 16);
            this.picNetworkStatus.TabIndex = 14;
            this.picNetworkStatus.TabStop = false;
            // 
            // lblNetworkStatus
            // 
            this.lblNetworkStatus.AutoSize = true;
            this.lblNetworkStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNetworkStatus.Location = new System.Drawing.Point(28, 125);
            this.lblNetworkStatus.Name = "lblNetworkStatus";
            this.lblNetworkStatus.Size = new System.Drawing.Size(47, 15);
            this.lblNetworkStatus.TabIndex = 15;
            this.lblNetworkStatus.Text = "label1";
            // 
            // tabCache
            // 
            this.tabCache.Controls.Add(this.groupBox8);
            this.tabCache.Location = new System.Drawing.Point(4, 22);
            this.tabCache.Name = "tabCache";
            this.tabCache.Padding = new System.Windows.Forms.Padding(3);
            this.tabCache.Size = new System.Drawing.Size(792, 574);
            this.tabCache.TabIndex = 3;
            this.tabCache.Text = "缓存状态";
            this.tabCache.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.lblCacheStatus);
            this.groupBox8.Controls.Add(this.label68);
            this.groupBox8.Controls.Add(this.lblCacheHitRate);
            this.groupBox8.Controls.Add(this.label70);
            this.groupBox8.Controls.Add(this.lblCacheSize);
            this.groupBox8.Controls.Add(this.label72);
            this.groupBox8.Controls.Add(this.lblCacheEntries);
            this.groupBox8.Controls.Add(this.label74);
            this.groupBox8.Location = new System.Drawing.Point(8, 6);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(776, 120);
            this.groupBox8.TabIndex = 0;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "缓存统计";
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(6, 25);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(58, 13);
            this.label74.TabIndex = 0;
            this.label74.Text = "缓存条目：";
            // 
            // lblCacheEntries
            // 
            this.lblCacheEntries.AutoSize = true;
            this.lblCacheEntries.Location = new System.Drawing.Point(70, 25);
            this.lblCacheEntries.Name = "lblCacheEntries";
            this.lblCacheEntries.Size = new System.Drawing.Size(47, 13);
            this.lblCacheEntries.TabIndex = 1;
            this.lblCacheEntries.Text = "label75";
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(300, 25);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(58, 13);
            this.label72.TabIndex = 2;
            this.label72.Text = "缓存大小：";
            // 
            // lblCacheSize
            // 
            this.lblCacheSize.AutoSize = true;
            this.lblCacheSize.Location = new System.Drawing.Point(364, 25);
            this.lblCacheSize.Name = "lblCacheSize";
            this.lblCacheSize.Size = new System.Drawing.Size(47, 13);
            this.lblCacheSize.TabIndex = 3;
            this.lblCacheSize.Text = "label73";
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 50);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(58, 13);
            this.label70.TabIndex = 4;
            this.label70.Text = "缓存命中率：";
            // 
            // lblCacheHitRate
            // 
            this.lblCacheHitRate.AutoSize = true;
            this.lblCacheHitRate.Location = new System.Drawing.Point(70, 50);
            this.lblCacheHitRate.Name = "lblCacheHitRate";
            this.lblCacheHitRate.Size = new System.Drawing.Size(47, 13);
            this.lblCacheHitRate.TabIndex = 5;
            this.lblCacheHitRate.Text = "label71";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(300, 50);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(58, 13);
            this.label68.TabIndex = 6;
            this.label68.Text = "缓存状态：";
            // 
            // lblCacheStatus
            // 
            this.lblCacheStatus.AutoSize = true;
            this.lblCacheStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCacheStatus.Location = new System.Drawing.Point(364, 50);
            this.lblCacheStatus.Name = "lblCacheStatus";
            this.lblCacheStatus.Size = new System.Drawing.Size(47, 15);
            this.lblCacheStatus.TabIndex = 7;
            this.lblCacheStatus.Text = "label1";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.groupBox11);
            this.tabControl.Controls.Add(this.groupBox10);
            this.tabControl.Controls.Add(this.groupBox9);
            this.tabControl.Location = new System.Drawing.Point(4, 22);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Windows.Forms.Padding(3);
            this.tabControl.Size = new System.Drawing.Size(792, 574);
            this.tabControl.TabIndex = 4;
            this.tabControl.Text = "远程控制";
            this.tabControl.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.txtMessageHistory);
            this.groupBox9.Location = new System.Drawing.Point(8, 6);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(776, 200);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "消息历史";
            // 
            // txtMessageHistory
            // 
            this.txtMessageHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageHistory.Location = new System.Drawing.Point(3, 16);
            this.txtMessageHistory.Name = "txtMessageHistory";
            this.txtMessageHistory.ReadOnly = true;
            this.txtMessageHistory.Size = new System.Drawing.Size(770, 181);
            this.txtMessageHistory.TabIndex = 0;
            this.txtMessageHistory.Text = "";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.txtMessage);
            this.groupBox10.Controls.Add(this.btnSendMessage);
            this.groupBox10.Location = new System.Drawing.Point(8, 212);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(776, 80);
            this.groupBox10.TabIndex = 1;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "发送消息";
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Location = new System.Drawing.Point(695, 48);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(75, 23);
            this.btnSendMessage.TabIndex = 0;
            this.btnSendMessage.Text = "发送";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendMessage_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(6, 19);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(683, 52);
            this.txtMessage.TabIndex = 1;
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.btnShutdown);
            this.groupBox11.Controls.Add(this.btnRestartClient);
            this.groupBox11.Controls.Add(this.btnClearCache);
            this.groupBox11.Controls.Add(this.btnForceSync);
            this.groupBox11.Controls.Add(this.btnExportSessionInfo);
            this.groupBox11.Location = new System.Drawing.Point(8, 298);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(776, 120);
            this.groupBox11.TabIndex = 2;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "管理操作";
            // 
            // btnExportSessionInfo
            // 
            this.btnExportSessionInfo.Location = new System.Drawing.Point(6, 25);
            this.btnExportSessionInfo.Name = "btnExportSessionInfo";
            this.btnExportSessionInfo.Size = new System.Drawing.Size(120, 30);
            this.btnExportSessionInfo.TabIndex = 0;
            this.btnExportSessionInfo.Text = "导出会话信息";
            this.btnExportSessionInfo.UseVisualStyleBackColor = true;
            this.btnExportSessionInfo.Click += new System.EventHandler(this.btnExportSessionInfo_Click);
            // 
            // btnForceSync
            // 
            this.btnForceSync.Location = new System.Drawing.Point(132, 25);
            this.btnForceSync.Name = "btnForceSync";
            this.btnForceSync.Size = new System.Drawing.Size(120, 30);
            this.btnForceSync.TabIndex = 1;
            this.btnForceSync.Text = "强制同步缓存";
            this.btnForceSync.UseVisualStyleBackColor = true;
            this.btnForceSync.Click += new System.EventHandler(this.btnForceSync_Click);
            // 
            // btnClearCache
            // 
            this.btnClearCache.Location = new System.Drawing.Point(258, 25);
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.Size = new System.Drawing.Size(120, 30);
            this.btnClearCache.TabIndex = 2;
            this.btnClearCache.Text = "清理缓存";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // btnRestartClient
            // 
            this.btnRestartClient.Location = new System.Drawing.Point(384, 25);
            this.btnRestartClient.Name = "btnRestartClient";
            this.btnRestartClient.Size = new System.Drawing.Size(120, 30);
            this.btnRestartClient.TabIndex = 3;
            this.btnRestartClient.Text = "重启客户端";
            this.btnRestartClient.UseVisualStyleBackColor = true;
            this.btnRestartClient.Click += new System.EventHandler(this.btnRestartClient_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Location = new System.Drawing.Point(510, 25);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(120, 30);
            this.btnShutdown.TabIndex = 4;
            this.btnShutdown.Text = "关机";
            this.btnShutdown.UseVisualStyleBackColor = true;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 578);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusLabel1.Text = "就绪";
            // 
            // SessionManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SessionManagementForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "会话管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionManagementForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabBasicInfo.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSessionStatus)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabSystemInfo.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.tabPerformance.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picNetworkStatus)).EndInit();
            this.tabCache.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabBasicInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox picSessionStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSessionID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblClientIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblClientPort;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblConnectedTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblConnectionDuration;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblClientVersion;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblIdleTime;
        private System.Windows.Forms.Label lblThroughput;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label lblLastHeartbeat;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label lblHeartbeatCount;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tabSystemInfo;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lblMacAddress;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label lblAvailableMemory;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label lblTotalMemory;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblCPUMaxSpeed;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label lblCPULogicalProcessors;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label lblCPUCores;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label lblCPUName;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblIs64Bit;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblArchitecture;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label lblMachineName;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label lblOSName;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.PictureBox picNetworkStatus;
        private System.Windows.Forms.Label lblNetworkStatus;
        private System.Windows.Forms.Label lblLocalIP;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label lblRemoteIP;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label lblRemotePort;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Label lblLatency;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label lblPacketLoss;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Label lblBandwidth;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label lblTotalBytesSent;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label lblTotalBytesReceived;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label lblSentPackets;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label lblReceivedPackets;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TabPage tabCache;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label lblCacheStatus;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label lblCacheHitRate;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label lblCacheSize;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label lblCacheEntries;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.TabPage tabControl;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Button btnRestartClient;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.Button btnForceSync;
        private System.Windows.Forms.Button btnExportSessionInfo;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.RichTextBox txtMessageHistory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}