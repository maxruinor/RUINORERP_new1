namespace RUINORERP.Server.Controls
{
    partial class FileManagementControl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            groupBox1 = new System.Windows.Forms.GroupBox();
            lblUsagePercentage = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            lblTotalFiles = new System.Windows.Forms.Label();
            lblFreeStorage = new System.Windows.Forms.Label();
            lblUsedStorage = new System.Windows.Forms.Label();
            lblTotalStorage = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            btnRefresh = new System.Windows.Forms.Button();
            btnCleanupFiles = new System.Windows.Forms.Button();
            btnViewDetails = new System.Windows.Forms.Button();
            btnMonitorDetails = new System.Windows.Forms.Button();
            btnViewDeletedFiles = new System.Windows.Forms.Button();
            listView1 = new System.Windows.Forms.ListView();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            lblLastUpdateTime = new System.Windows.Forms.ToolStripStatusLabel();
            picStatusIndicator = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picStatusIndicator).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Panel2.Controls.Add(listView1);
            splitContainer1.Size = new System.Drawing.Size(936, 869);
            splitContainer1.SplitterDistance = 217;
            splitContainer1.SplitterWidth = 6;
            splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lblUsagePercentage);
            groupBox1.Controls.Add(progressBar1);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(lblTotalFiles);
            groupBox1.Controls.Add(lblFreeStorage);
            groupBox1.Controls.Add(lblUsedStorage);
            groupBox1.Controls.Add(lblTotalStorage);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBox1.Location = new System.Drawing.Point(0, 0);
            groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            groupBox1.Size = new System.Drawing.Size(936, 217);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "存储使用概览";
            // 
            // lblUsagePercentage
            // 
            lblUsagePercentage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblUsagePercentage.AutoSize = true;
            lblUsagePercentage.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
            lblUsagePercentage.Location = new System.Drawing.Point(878, 163);
            lblUsagePercentage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUsagePercentage.Name = "lblUsagePercentage";
            lblUsagePercentage.Size = new System.Drawing.Size(26, 17);
            lblUsagePercentage.TabIndex = 10;
            lblUsagePercentage.Text = "0%";
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(12, 163);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(859, 24);
            progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            progressBar1.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(700, 64);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(68, 17);
            label5.TabIndex = 8;
            label5.Text = "文件总数：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(700, 26);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(68, 17);
            label4.TabIndex = 7;
            label4.Text = "空闲空间：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(350, 64);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(68, 17);
            label3.TabIndex = 6;
            label3.Text = "已用空间：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(350, 26);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(56, 17);
            label2.TabIndex = 5;
            label2.Text = "总空间：";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 113);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 17);
            label1.TabIndex = 4;
            label1.Text = "使用情况：";
            // 
            // lblTotalFiles
            // 
            lblTotalFiles.AutoSize = true;
            lblTotalFiles.Location = new System.Drawing.Point(782, 64);
            lblTotalFiles.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalFiles.Name = "lblTotalFiles";
            lblTotalFiles.Size = new System.Drawing.Size(15, 17);
            lblTotalFiles.TabIndex = 3;
            lblTotalFiles.Text = "0";
            // 
            // lblFreeStorage
            // 
            lblFreeStorage.AutoSize = true;
            lblFreeStorage.Location = new System.Drawing.Point(782, 26);
            lblFreeStorage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblFreeStorage.Name = "lblFreeStorage";
            lblFreeStorage.Size = new System.Drawing.Size(32, 17);
            lblFreeStorage.TabIndex = 2;
            lblFreeStorage.Text = "未知";
            // 
            // lblUsedStorage
            // 
            lblUsedStorage.AutoSize = true;
            lblUsedStorage.Location = new System.Drawing.Point(432, 64);
            lblUsedStorage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblUsedStorage.Name = "lblUsedStorage";
            lblUsedStorage.Size = new System.Drawing.Size(32, 17);
            lblUsedStorage.TabIndex = 1;
            lblUsedStorage.Text = "未知";
            // 
            // lblTotalStorage
            // 
            lblTotalStorage.AutoSize = true;
            lblTotalStorage.Location = new System.Drawing.Point(432, 26);
            lblTotalStorage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblTotalStorage.Name = "lblTotalStorage";
            lblTotalStorage.Size = new System.Drawing.Size(32, 17);
            lblTotalStorage.TabIndex = 0;
            lblTotalStorage.Text = "未知";
            // 
            // panel1
            // 
            panel1.Controls.Add(btnRefresh);
            panel1.Controls.Add(btnCleanupFiles);
            panel1.Controls.Add(btnViewDetails);
            panel1.Controls.Add(btnMonitorDetails);
            panel1.Controls.Add(btnViewDeletedFiles);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 569);
            panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(936, 77);
            panel1.TabIndex = 1;
            // 
            // btnRefresh
            //
            btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnRefresh.Location = new System.Drawing.Point(703, 8);
            btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new System.Drawing.Size(88, 33);
            btnRefresh.TabIndex = 1;
            btnRefresh.Text = "刷新数据";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            //
            // btnCleanupFiles
            //
            btnCleanupFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnCleanupFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            btnCleanupFiles.Location = new System.Drawing.Point(586, 8);
            btnCleanupFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnCleanupFiles.Name = "btnCleanupFiles";
            btnCleanupFiles.Size = new System.Drawing.Size(88, 33);
            btnCleanupFiles.TabIndex = 3;
            btnCleanupFiles.Text = "清理文件";
            btnCleanupFiles.UseVisualStyleBackColor = false;
            btnCleanupFiles.Click += new System.EventHandler(btnCleanupFiles_Click);
            //
            // btnViewDetails
            //
            btnViewDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnViewDetails.Location = new System.Drawing.Point(820, 8);
            btnViewDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnViewDetails.Name = "btnViewDetails";
            btnViewDetails.Size = new System.Drawing.Size(88, 33);
            btnViewDetails.TabIndex = 0;
            btnViewDetails.Text = "查看详情";
            btnViewDetails.UseVisualStyleBackColor = true;
            btnViewDetails.Click += btnViewDetails_Click;
            //
            // btnMonitorDetails
            //
            btnMonitorDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnMonitorDetails.Location = new System.Drawing.Point(703, 40);
            btnMonitorDetails.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnMonitorDetails.Name = "btnMonitorDetails";
            btnMonitorDetails.Size = new System.Drawing.Size(205, 33);
            btnMonitorDetails.TabIndex = 2;
            btnMonitorDetails.Text = "查看监控详情";
            btnMonitorDetails.UseVisualStyleBackColor = true;
            btnMonitorDetails.Click += new System.EventHandler(btnMonitorDetails_Click);
            //
            // btnViewDeletedFiles
            //
            btnViewDeletedFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnViewDeletedFiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(216)))), ((int)(((byte)(230)))));
            btnViewDeletedFiles.Location = new System.Drawing.Point(469, 8);
            btnViewDeletedFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            btnViewDeletedFiles.Name = "btnViewDeletedFiles";
            btnViewDeletedFiles.Size = new System.Drawing.Size(111, 33);
            btnViewDeletedFiles.TabIndex = 4;
            btnViewDeletedFiles.Text = "已删除文件";
            btnViewDeletedFiles.UseVisualStyleBackColor = false;
            btnViewDeletedFiles.Click += new System.EventHandler(btnViewDeletedFiles_Click);
            // 
            // listView1
            // 
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(936, 646);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripStatusLabel3, lblStatus, toolStripStatusLabel2, lblLastUpdateTime });
            statusStrip1.Location = new System.Drawing.Point(0, 847);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(936, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(44, 17);
            toolStripStatusLabel1.Text = "状态：";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new System.Drawing.Size(32, 17);
            lblStatus.Text = "未知";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new System.Drawing.Size(68, 17);
            toolStripStatusLabel2.Text = "最后更新：";
            // 
            // lblLastUpdateTime
            // 
            lblLastUpdateTime.Name = "lblLastUpdateTime";
            lblLastUpdateTime.Size = new System.Drawing.Size(13, 17);
            lblLastUpdateTime.Text = "-";
            // 
            // picStatusIndicator
            // 
            picStatusIndicator.BackColor = System.Drawing.Color.Gray;
            picStatusIndicator.Location = new System.Drawing.Point(55, 3);
            picStatusIndicator.Name = "picStatusIndicator";
            picStatusIndicator.Size = new System.Drawing.Size(12, 12);
            picStatusIndicator.TabIndex = 1;
            picStatusIndicator.TabStop = false;
            // 
            // FileManagementControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(statusStrip1);
            Controls.Add(splitContainer1);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "FileManagementControl";
            Size = new System.Drawing.Size(936, 869);
            Load += FileManagementControl_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picStatusIndicator).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsedStorage;
        private System.Windows.Forms.Label lblTotalStorage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTotalFiles;
        private System.Windows.Forms.Label lblFreeStorage;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblUsagePercentage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnCleanupFiles;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Button btnMonitorDetails;
        private System.Windows.Forms.Button btnViewDeletedFiles;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.PictureBox picStatusIndicator;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdateTime;
    }
}