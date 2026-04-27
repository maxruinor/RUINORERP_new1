namespace RUINORERP.Server.Controls
{
    partial class InventorySnapshotManagementControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageConfig = new System.Windows.Forms.TabPage();
            this.groupBoxQuickTime = new System.Windows.Forms.GroupBox();
            this.btnSet04AM = new System.Windows.Forms.Button();
            this.btnSet03AM = new System.Windows.Forms.Button();
            this.btnSet02AM = new System.Windows.Forms.Button();
            this.btnSet01AM = new System.Windows.Forms.Button();
            this.groupBoxDebug = new System.Windows.Forms.GroupBox();
            this.btnSetDebugInterval = new System.Windows.Forms.Button();
            this.btnToggleDebugMode = new System.Windows.Forms.Button();
            this.lblDebugInterval = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblDebugMode = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lblNextRunTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblIntervalType = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblExecutionTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditTime = new System.Windows.Forms.Button();
            this.btnToggleEnabled = new System.Windows.Forms.Button();
            this.tabPageManual = new System.Windows.Forms.TabPage();
            this.btnTriggerNow = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPageHelp = new System.Windows.Forms.TabPage();
            this.btnHelp = new System.Windows.Forms.Button();
            this.richTextBoxHelp = new System.Windows.Forms.RichTextBox();
            this.dataGridViewTasks = new System.Windows.Forms.DataGridView();
            this.colTaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExecutionTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabControlMain.SuspendLayout();
            this.tabPageConfig.SuspendLayout();
            this.groupBoxQuickTime.SuspendLayout();
            this.groupBoxDebug.SuspendLayout();
            this.groupBoxStatus.SuspendLayout();
            this.tabPageManual.SuspendLayout();
            this.tabPageHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageConfig);
            this.tabControlMain.Controls.Add(this.tabPageManual);
            this.tabControlMain.Controls.Add(this.tabPageHelp);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(800, 600);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageConfig
            // 
            this.tabPageConfig.Controls.Add(this.groupBoxQuickTime);
            this.tabPageConfig.Controls.Add(this.groupBoxDebug);
            this.tabPageConfig.Controls.Add(this.groupBoxStatus);
            this.tabPageConfig.Controls.Add(this.btnEditTime);
            this.tabPageConfig.Controls.Add(this.btnToggleEnabled);
            this.tabPageConfig.Controls.Add(this.dataGridViewTasks);
            this.tabPageConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageConfig.Name = "tabPageConfig";
            this.tabPageConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageConfig.Size = new System.Drawing.Size(792, 574);
            this.tabPageConfig.TabIndex = 0;
            this.tabPageConfig.Text = "配置管理";
            this.tabPageConfig.UseVisualStyleBackColor = true;
            // 
            // groupBoxQuickTime
            // 
            this.groupBoxQuickTime.Controls.Add(this.btnSet04AM);
            this.groupBoxQuickTime.Controls.Add(this.btnSet03AM);
            this.groupBoxQuickTime.Controls.Add(this.btnSet02AM);
            this.groupBoxQuickTime.Controls.Add(this.btnSet01AM);
            this.groupBoxQuickTime.Location = new System.Drawing.Point(20, 340);
            this.groupBoxQuickTime.Name = "groupBoxQuickTime";
            this.groupBoxQuickTime.Size = new System.Drawing.Size(350, 120);
            this.groupBoxQuickTime.TabIndex = 5;
            this.groupBoxQuickTime.TabStop = false;
            this.groupBoxQuickTime.Text = "快捷时间设置（推荐夜间执行）";
            // 
            // btnSet04AM
            // 
            this.btnSet04AM.Location = new System.Drawing.Point(260, 40);
            this.btnSet04AM.Name = "btnSet04AM";
            this.btnSet04AM.Size = new System.Drawing.Size(75, 30);
            this.btnSet04AM.TabIndex = 3;
            this.btnSet04AM.Text = "凌晨4点";
            this.btnSet04AM.UseVisualStyleBackColor = true;
            this.btnSet04AM.Click += new System.EventHandler(this.SetTimeTo04AM);
            // 
            // btnSet03AM
            // 
            this.btnSet03AM.Location = new System.Drawing.Point(180, 40);
            this.btnSet03AM.Name = "btnSet03AM";
            this.btnSet03AM.Size = new System.Drawing.Size(75, 30);
            this.btnSet03AM.TabIndex = 2;
            this.btnSet03AM.Text = "凌晨3点";
            this.btnSet03AM.UseVisualStyleBackColor = true;
            this.btnSet03AM.Click += new System.EventHandler(this.SetTimeTo03AM);
            // 
            // btnSet02AM
            // 
            this.btnSet02AM.Location = new System.Drawing.Point(100, 40);
            this.btnSet02AM.Name = "btnSet02AM";
            this.btnSet02AM.Size = new System.Drawing.Size(75, 30);
            this.btnSet02AM.TabIndex = 1;
            this.btnSet02AM.Text = "凌晨2点";
            this.btnSet02AM.UseVisualStyleBackColor = true;
            this.btnSet02AM.Click += new System.EventHandler(this.SetTimeTo02AM);
            // 
            // btnSet01AM
            // 
            this.btnSet01AM.Location = new System.Drawing.Point(20, 40);
            this.btnSet01AM.Name = "btnSet01AM";
            this.btnSet01AM.Size = new System.Drawing.Size(75, 30);
            this.btnSet01AM.TabIndex = 0;
            this.btnSet01AM.Text = "凌晨1点";
            this.btnSet01AM.UseVisualStyleBackColor = true;
            this.btnSet01AM.Click += new System.EventHandler(this.SetTimeTo01AM);
            // 
            // groupBoxDebug
            // 
            this.groupBoxDebug.Controls.Add(this.btnSetDebugInterval);
            this.groupBoxDebug.Controls.Add(this.btnToggleDebugMode);
            this.groupBoxDebug.Controls.Add(this.lblDebugInterval);
            this.groupBoxDebug.Controls.Add(this.label8);
            this.groupBoxDebug.Controls.Add(this.lblDebugMode);
            this.groupBoxDebug.Controls.Add(this.label6);
            this.groupBoxDebug.Location = new System.Drawing.Point(400, 200);
            this.groupBoxDebug.Name = "groupBoxDebug";
            this.groupBoxDebug.Size = new System.Drawing.Size(350, 120);
            this.groupBoxDebug.TabIndex = 4;
            this.groupBoxDebug.TabStop = false;
            this.groupBoxDebug.Text = "调试模式";
            // 
            // btnSetDebugInterval
            // 
            this.btnSetDebugInterval.Location = new System.Drawing.Point(250, 70);
            this.btnSetDebugInterval.Name = "btnSetDebugInterval";
            this.btnSetDebugInterval.Size = new System.Drawing.Size(80, 30);
            this.btnSetDebugInterval.TabIndex = 5;
            this.btnSetDebugInterval.Text = "设置间隔";
            this.btnSetDebugInterval.UseVisualStyleBackColor = true;
            this.btnSetDebugInterval.Click += new System.EventHandler(this.btnSetDebugInterval_Click);
            // 
            // btnToggleDebugMode
            // 
            this.btnToggleDebugMode.Location = new System.Drawing.Point(250, 30);
            this.btnToggleDebugMode.Name = "btnToggleDebugMode";
            this.btnToggleDebugMode.Size = new System.Drawing.Size(80, 30);
            this.btnToggleDebugMode.TabIndex = 4;
            this.btnToggleDebugMode.Text = "切换";
            this.btnToggleDebugMode.UseVisualStyleBackColor = true;
            this.btnToggleDebugMode.Click += new System.EventHandler(this.btnToggleDebugMode_Click);
            // 
            // lblDebugInterval
            // 
            this.lblDebugInterval.AutoSize = true;
            this.lblDebugInterval.Location = new System.Drawing.Point(120, 75);
            this.lblDebugInterval.Name = "lblDebugInterval";
            this.lblDebugInterval.Size = new System.Drawing.Size(41, 12);
            this.lblDebugInterval.TabIndex = 3;
            this.lblDebugInterval.Text = "5 分钟";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "执行间隔：";
            // 
            // lblDebugMode
            // 
            this.lblDebugMode.AutoSize = true;
            this.lblDebugMode.Location = new System.Drawing.Point(120, 35);
            this.lblDebugMode.Name = "lblDebugMode";
            this.lblDebugMode.Size = new System.Drawing.Size(17, 12);
            this.lblDebugMode.TabIndex = 1;
            this.lblDebugMode.Text = "否";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "调试模式：";
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.lblNextRunTime);
            this.groupBoxStatus.Controls.Add(this.label5);
            this.groupBoxStatus.Controls.Add(this.lblIntervalType);
            this.groupBoxStatus.Controls.Add(this.label4);
            this.groupBoxStatus.Controls.Add(this.lblExecutionTime);
            this.groupBoxStatus.Controls.Add(this.label3);
            this.groupBoxStatus.Controls.Add(this.lblStatus);
            this.groupBoxStatus.Controls.Add(this.label1);
            this.groupBoxStatus.Location = new System.Drawing.Point(20, 200);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(350, 120);
            this.groupBoxStatus.TabIndex = 3;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "当前状态";
            // 
            // lblNextRunTime
            // 
            this.lblNextRunTime.AutoSize = true;
            this.lblNextRunTime.Location = new System.Drawing.Point(120, 90);
            this.lblNextRunTime.Name = "lblNextRunTime";
            this.lblNextRunTime.Size = new System.Drawing.Size(0, 12);
            this.lblNextRunTime.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "下次执行时间：";
            // 
            // lblIntervalType
            // 
            this.lblIntervalType.AutoSize = true;
            this.lblIntervalType.Location = new System.Drawing.Point(120, 65);
            this.lblIntervalType.Name = "lblIntervalType";
            this.lblIntervalType.Size = new System.Drawing.Size(41, 12);
            this.lblIntervalType.TabIndex = 5;
            this.lblIntervalType.Text = "每日执行";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "执行类型：";
            // 
            // lblExecutionTime
            // 
            this.lblExecutionTime.AutoSize = true;
            this.lblExecutionTime.Location = new System.Drawing.Point(120, 40);
            this.lblExecutionTime.Name = "lblExecutionTime";
            this.lblExecutionTime.Size = new System.Drawing.Size(65, 12);
            this.lblExecutionTime.TabIndex = 3;
            this.lblExecutionTime.Text = "01:00:00";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "执行时间：";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Green;
            this.lblStatus.Location = new System.Drawing.Point(120, 20);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(41, 12);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "已启用";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "状态：";
            // 
            // btnEditTime
            // 
            this.btnEditTime.Location = new System.Drawing.Point(400, 150);
            this.btnEditTime.Name = "btnEditTime";
            this.btnEditTime.Size = new System.Drawing.Size(100, 35);
            this.btnEditTime.TabIndex = 2;
            this.btnEditTime.Text = "自定义时间";
            this.btnEditTime.UseVisualStyleBackColor = true;
            this.btnEditTime.Click += new System.EventHandler(this.btnEditTime_Click);
            // 
            // btnToggleEnabled
            // 
            this.btnToggleEnabled.Location = new System.Drawing.Point(400, 100);
            this.btnToggleEnabled.Name = "btnToggleEnabled";
            this.btnToggleEnabled.Size = new System.Drawing.Size(100, 35);
            this.btnToggleEnabled.TabIndex = 1;
            this.btnToggleEnabled.Text = "启用/禁用";
            this.btnToggleEnabled.UseVisualStyleBackColor = true;
            this.btnToggleEnabled.Click += new System.EventHandler(this.btnToggleEnabled_Click);
            // 
            // dataGridViewTasks
            // 
            this.dataGridViewTasks.AllowUserToAddRows = false;
            this.dataGridViewTasks.AllowUserToDeleteRows = false;
            this.dataGridViewTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTasks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTaskName,
            this.colDescription,
            this.colExecutionTime,
            this.colEnabled});
            this.dataGridViewTasks.Location = new System.Drawing.Point(20, 20);
            this.dataGridViewTasks.Name = "dataGridViewTasks";
            this.dataGridViewTasks.RowTemplate.Height = 23;
            this.dataGridViewTasks.Size = new System.Drawing.Size(750, 70);
            this.dataGridViewTasks.TabIndex = 0;
            // 
            // colTaskName
            // 
            this.colTaskName.DataPropertyName = "Name";
            this.colTaskName.HeaderText = "任务名称";
            this.colTaskName.Name = "colTaskName";
            this.colTaskName.Width = 120;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "描述";
            this.colDescription.Name = "colDescription";
            this.colDescription.Width = 350;
            // 
            // colExecutionTime
            // 
            this.colExecutionTime.DataPropertyName = "ExecutionTime";
            this.colExecutionTime.HeaderText = "执行时间";
            this.colExecutionTime.Name = "colExecutionTime";
            this.colExecutionTime.Width = 100;
            // 
            // colEnabled
            // 
            this.colEnabled.DataPropertyName = "Enabled";
            this.colEnabled.HeaderText = "启用";
            this.colEnabled.Name = "colEnabled";
            this.colEnabled.Width = 60;
            // 
            // tabPageManual
            // 
            this.tabPageManual.Controls.Add(this.btnTriggerNow);
            this.tabPageManual.Controls.Add(this.label9);
            this.tabPageManual.Location = new System.Drawing.Point(4, 22);
            this.tabPageManual.Name = "tabPageManual";
            this.tabPageManual.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageManual.Size = new System.Drawing.Size(792, 574);
            this.tabPageManual.TabIndex = 1;
            this.tabPageManual.Text = "手动触发";
            this.tabPageManual.UseVisualStyleBackColor = true;
            // 
            // btnTriggerNow
            // 
            this.btnTriggerNow.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTriggerNow.Location = new System.Drawing.Point(300, 200);
            this.btnTriggerNow.Name = "btnTriggerNow";
            this.btnTriggerNow.Size = new System.Drawing.Size(200, 60);
            this.btnTriggerNow.TabIndex = 1;
            this.btnTriggerNow.Text = "立即执行库存快照";
            this.btnTriggerNow.UseVisualStyleBackColor = true;
            this.btnTriggerNow.Click += new System.EventHandler(this.btnTriggerNow_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(200, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(395, 24);
            this.label9.TabIndex = 0;
            this.label9.Text = "点击下方按钮可立即执行库存快照，用于测试和验证功能。\r\n执行结果请查看服务器日志。";
            // 
            // tabPageHelp
            // 
            this.tabPageHelp.Controls.Add(this.btnHelp);
            this.tabPageHelp.Controls.Add(this.richTextBoxHelp);
            this.tabPageHelp.Location = new System.Drawing.Point(4, 22);
            this.tabPageHelp.Name = "tabPageHelp";
            this.tabPageHelp.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHelp.Size = new System.Drawing.Size(792, 574);
            this.tabPageHelp.TabIndex = 2;
            this.tabPageHelp.Text = "帮助说明";
            this.tabPageHelp.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(350, 500);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(100, 35);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "显示帮助";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // richTextBoxHelp
            // 
            this.richTextBoxHelp.Location = new System.Drawing.Point(20, 20);
            this.richTextBoxHelp.Name = "richTextBoxHelp";
            this.richTextBoxHelp.ReadOnly = true;
            this.richTextBoxHelp.Size = new System.Drawing.Size(750, 450);
            this.richTextBoxHelp.TabIndex = 0;
            this.richTextBoxHelp.Text = "";
            // 
            // InventorySnapshotManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Name = "InventorySnapshotManagementControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.InventorySnapshotManagementControl_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageConfig.ResumeLayout(false);
            this.groupBoxQuickTime.ResumeLayout(false);
            this.groupBoxDebug.ResumeLayout(false);
            this.groupBoxDebug.PerformLayout();
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.tabPageManual.ResumeLayout(false);
            this.tabPageManual.PerformLayout();
            this.tabPageHelp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTasks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageConfig;
        private System.Windows.Forms.DataGridView dataGridViewTasks;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExecutionTime;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colEnabled;
        private System.Windows.Forms.Button btnToggleEnabled;
        private System.Windows.Forms.Button btnEditTime;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblExecutionTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblIntervalType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNextRunTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBoxDebug;
        private System.Windows.Forms.Label lblDebugMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblDebugInterval;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnToggleDebugMode;
        private System.Windows.Forms.Button btnSetDebugInterval;
        private System.Windows.Forms.GroupBox groupBoxQuickTime;
        private System.Windows.Forms.Button btnSet04AM;
        private System.Windows.Forms.Button btnSet03AM;
        private System.Windows.Forms.Button btnSet02AM;
        private System.Windows.Forms.Button btnSet01AM;
        private System.Windows.Forms.TabPage tabPageManual;
        private System.Windows.Forms.Button btnTriggerNow;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TabPage tabPageHelp;
        private System.Windows.Forms.RichTextBox richTextBoxHelp;
        private System.Windows.Forms.Button btnHelp;
    }
}
