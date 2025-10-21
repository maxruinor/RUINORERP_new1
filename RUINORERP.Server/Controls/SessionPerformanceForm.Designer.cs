namespace RUINORERP.Server.Controls
{
    partial class SessionPerformanceForm
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblClientVersion = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblClientInfo = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.picStatusIndicator = new System.Windows.Forms.PictureBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSessionId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblThroughput = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lblHeartbeatCount = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.lblConnectedDuration = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblLastActivity = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblConnectedTime = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblTotalBytesReceived = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblTotalBytesSent = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblReceivedPackets = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblSentPackets = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusIndicator)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
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
            this.tabPage1.Size = new System.Drawing.Size(768, 400);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "性能概览";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 400);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lblClientVersion);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblClientInfo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.picStatusIndicator);
            this.groupBox1.Controls.Add(this.lblStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblUserName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblSessionId);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(756, 134);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "会话基本信息";
            // 
            // lblClientVersion
            // 
            this.lblClientVersion.AutoSize = true;
            this.lblClientVersion.Location = new System.Drawing.Point(451, 90);
            this.lblClientVersion.Name = "lblClientVersion";
            this.lblClientVersion.Size = new System.Drawing.Size(65, 12);
            this.lblClientVersion.TabIndex = 10;
            this.lblClientVersion.Text = "未知版本";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(369, 90);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 9;
            this.label9.Text = "客户端版本：";
            // 
            // lblClientInfo
            // 
            this.lblClientInfo.AutoSize = true;
            this.lblClientInfo.Location = new System.Drawing.Point(82, 90);
            this.lblClientInfo.Name = "lblClientInfo";
            this.lblClientInfo.Size = new System.Drawing.Size(41, 12);
            this.lblClientInfo.TabIndex = 8;
            this.lblClientInfo.Text = "未知";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "客户端信息：";
            // 
            // picStatusIndicator
            // 
            this.picStatusIndicator.Location = new System.Drawing.Point(451, 23);
            this.picStatusIndicator.Name = "picStatusIndicator";
            this.picStatusIndicator.Size = new System.Drawing.Size(12, 12);
            this.picStatusIndicator.TabIndex = 6;
            this.picStatusIndicator.TabStop = false;
            this.picStatusIndicator.BackColor = System.Drawing.Color.Gray;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(82, 23);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "未知";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "会话状态：";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(82, 56);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(41, 12);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "未知";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "用户名：";
            // 
            // lblSessionId
            // 
            this.lblSessionId.AutoSize = true;
            this.lblSessionId.Location = new System.Drawing.Point(470, 23);
            this.lblSessionId.Name = "lblSessionId";
            this.lblSessionId.Size = new System.Drawing.Size(41, 12);
            this.lblSessionId.TabIndex = 1;
            this.lblSessionId.Text = "未知";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(369, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "会话ID：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.lblThroughput);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.lblHeartbeatCount);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.lblConnectedDuration);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.lblLastActivity);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.lblConnectedTime);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Location = new System.Drawing.Point(386, 146);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(376, 248);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "时间统计信息";
            // 
            // lblThroughput
            // 
            this.lblThroughput.AutoSize = true;
            this.lblThroughput.Location = new System.Drawing.Point(82, 193);
            this.lblThroughput.Name = "lblThroughput";
            this.lblThroughput.Size = new System.Drawing.Size(41, 12);
            this.lblThroughput.TabIndex = 9;
            this.lblThroughput.Text = "计算中";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 193);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 12);
            this.label19.TabIndex = 8;
            this.label19.Text = "平均吞吐量：";
            // 
            // lblHeartbeatCount
            // 
            this.lblHeartbeatCount.AutoSize = true;
            this.lblHeartbeatCount.Location = new System.Drawing.Point(82, 156);
            this.lblHeartbeatCount.Name = "lblHeartbeatCount";
            this.lblHeartbeatCount.Size = new System.Drawing.Size(11, 12);
            this.lblHeartbeatCount.TabIndex = 7;
            this.lblHeartbeatCount.Text = "0";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(10, 156);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 6;
            this.label17.Text = "心跳次数：";
            // 
            // lblConnectedDuration
            // 
            this.lblConnectedDuration.AutoSize = true;
            this.lblConnectedDuration.Location = new System.Drawing.Point(82, 119);
            this.lblConnectedDuration.Name = "lblConnectedDuration";
            this.lblConnectedDuration.Size = new System.Drawing.Size(41, 12);
            this.lblConnectedDuration.TabIndex = 5;
            this.lblConnectedDuration.Text = "未知";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 119);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(65, 12);
            this.label15.TabIndex = 4;
            this.label15.Text = "连接时长：";
            // 
            // lblLastActivity
            // 
            this.lblLastActivity.AutoSize = true;
            this.lblLastActivity.Location = new System.Drawing.Point(82, 82);
            this.lblLastActivity.Name = "lblLastActivity";
            this.lblLastActivity.Size = new System.Drawing.Size(41, 12);
            this.lblLastActivity.TabIndex = 3;
            this.lblLastActivity.Text = "未知";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 2;
            this.label13.Text = "最后活动：";
            // 
            // lblConnectedTime
            // 
            this.lblConnectedTime.AutoSize = true;
            this.lblConnectedTime.Location = new System.Drawing.Point(82, 45);
            this.lblConnectedTime.Name = "lblConnectedTime";
            this.lblConnectedTime.Size = new System.Drawing.Size(41, 12);
            this.lblConnectedTime.TabIndex = 1;
            this.lblConnectedTime.Text = "未知";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "连接时间：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lblTotalBytesReceived);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.lblTotalBytesSent);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.lblReceivedPackets);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.lblSentPackets);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Location = new System.Drawing.Point(6, 146);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(374, 248);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "网络传输统计";
            // 
            // lblTotalBytesReceived
            // 
            this.lblTotalBytesReceived.AutoSize = true;
            this.lblTotalBytesReceived.Location = new System.Drawing.Point(82, 119);
            this.lblTotalBytesReceived.Name = "lblTotalBytesReceived";
            this.lblTotalBytesReceived.Size = new System.Drawing.Size(11, 12);
            this.lblTotalBytesReceived.TabIndex = 7;
            this.lblTotalBytesReceived.Text = "0";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 119);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 6;
            this.label18.Text = "接收字节：";
            // 
            // lblTotalBytesSent
            // 
            this.lblTotalBytesSent.AutoSize = true;
            this.lblTotalBytesSent.Location = new System.Drawing.Point(82, 82);
            this.lblTotalBytesSent.Name = "lblTotalBytesSent";
            this.lblTotalBytesSent.Size = new System.Drawing.Size(11, 12);
            this.lblTotalBytesSent.TabIndex = 5;
            this.lblTotalBytesSent.Text = "0";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(10, 82);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 4;
            this.label16.Text = "发送字节：";
            // 
            // lblReceivedPackets
            // 
            this.lblReceivedPackets.AutoSize = true;
            this.lblReceivedPackets.Location = new System.Drawing.Point(82, 45);
            this.lblReceivedPackets.Name = "lblReceivedPackets";
            this.lblReceivedPackets.Size = new System.Drawing.Size(11, 12);
            this.lblReceivedPackets.TabIndex = 3;
            this.lblReceivedPackets.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 45);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 2;
            this.label14.Text = "接收数据包：";
            // 
            // lblSentPackets
            // 
            this.lblSentPackets.AutoSize = true;
            this.lblSentPackets.Location = new System.Drawing.Point(82, 193);
            this.lblSentPackets.Name = "lblSentPackets";
            this.lblSentPackets.Size = new System.Drawing.Size(11, 12);
            this.lblSentPackets.TabIndex = 1;
            this.lblSentPackets.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 193);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "发送数据包：";
            // 
            // SessionPerformanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "SessionPerformanceForm";
            this.Text = "会话性能详情";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionPerformanceForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusIndicator)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSessionId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox picStatusIndicator;
        private System.Windows.Forms.Label lblClientInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblClientVersion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblConnectedTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblConnectedDuration;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblLastActivity;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblHeartbeatCount;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblThroughput;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblTotalBytesReceived;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblTotalBytesSent;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblReceivedPackets;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblSentPackets;
        private System.Windows.Forms.Label label12;
    }
}