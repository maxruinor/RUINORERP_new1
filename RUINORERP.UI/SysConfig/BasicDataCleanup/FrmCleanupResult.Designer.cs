namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    partial class FrmCleanupResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnExport = new Krypton.Toolkit.KryptonButton();
            this.kbtnClose = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox4 = new Krypton.Toolkit.KryptonGroupBox();
            this.ktxtLog = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonGroupBox3 = new Krypton.Toolkit.KryptonGroupBox();
            this.dgvRuleResults = new System.Windows.Forms.DataGridView();
            this.colRuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRuleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSuccessCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailedCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colElapsedTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.kryptonPanel3 = new Krypton.Toolkit.KryptonPanel();
            this.klblElapsedTime = new Krypton.Toolkit.KryptonLabel();
            this.klblExecuteTime = new Krypton.Toolkit.KryptonLabel();
            this.klblExecuteResult = new Krypton.Toolkit.KryptonLabel();
            this.klblExecuteMode = new Krypton.Toolkit.KryptonLabel();
            this.klblConfigName = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).BeginInit();
            this.kryptonGroupBox4.Panel.SuspendLayout();
            this.kryptonGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).BeginInit();
            this.kryptonGroupBox3.Panel.SuspendLayout();
            this.kryptonGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).BeginInit();
            this.kryptonPanel3.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonPanel1
            //
            this.kryptonPanel1.Controls.Add(this.kbtnExport);
            this.kryptonPanel1.Controls.Add(this.kbtnClose);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 700);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(900, 60);
            this.kryptonPanel1.TabIndex = 0;
            //
            // kbtnExport
            //
            this.kbtnExport.Location = new System.Drawing.Point(350, 16);
            this.kbtnExport.Name = "kbtnExport";
            this.kbtnExport.Size = new System.Drawing.Size(90, 28);
            this.kbtnExport.TabIndex = 1;
            this.kbtnExport.Values.Text = "导出日志";
            this.kbtnExport.Click += new System.EventHandler(this.KbtnExport_Click);
            //
            // kbtnClose
            //
            this.kbtnClose.Location = new System.Drawing.Point(460, 16);
            this.kbtnClose.Name = "kbtnClose";
            this.kbtnClose.Size = new System.Drawing.Size(90, 28);
            this.kbtnClose.TabIndex = 0;
            this.kbtnClose.Values.Text = "关闭";
            this.kbtnClose.Click += new System.EventHandler(this.KbtnClose_Click);
            //
            // kryptonPanel2
            //
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox4);
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox3);
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel2.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(900, 700);
            this.kryptonPanel2.TabIndex = 1;
            //
            // kryptonGroupBox4
            //
            this.kryptonGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox4.Location = new System.Drawing.Point(0, 550);
            this.kryptonGroupBox4.Name = "kryptonGroupBox4";
            //
            // kryptonGroupBox4.Panel
            //
            this.kryptonGroupBox4.Panel.Controls.Add(this.ktxtLog);
            this.kryptonGroupBox4.Size = new System.Drawing.Size(900, 150);
            this.kryptonGroupBox4.TabIndex = 3;
            this.kryptonGroupBox4.Values.Heading = "执行日志";
            //
            // ktxtLog
            //
            this.ktxtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ktxtLog.Location = new System.Drawing.Point(0, 0);
            this.ktxtLog.Multiline = true;
            this.ktxtLog.Name = "ktxtLog";
            this.ktxtLog.ReadOnly = true;
            this.ktxtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ktxtLog.Size = new System.Drawing.Size(896, 126);
            this.ktxtLog.TabIndex = 0;
            //
            // kryptonGroupBox3
            //
            this.kryptonGroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox3.Location = new System.Drawing.Point(0, 350);
            this.kryptonGroupBox3.Name = "kryptonGroupBox3";
            //
            // kryptonGroupBox3.Panel
            //
            this.kryptonGroupBox3.Panel.Controls.Add(this.dgvRuleResults);
            this.kryptonGroupBox3.Size = new System.Drawing.Size(900, 200);
            this.kryptonGroupBox3.TabIndex = 2;
            this.kryptonGroupBox3.Values.Heading = "规则执行详情";
            //
            // dgvRuleResults
            //
            this.dgvRuleResults.AllowUserToAddRows = false;
            this.dgvRuleResults.AllowUserToDeleteRows = false;
            this.dgvRuleResults.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRuleResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRuleResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRuleResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colRuleName,
            this.colRuleType,
            this.colMatchedCount,
            this.colSuccessCount,
            this.colFailedCount,
            this.colStatus,
            this.colElapsedTime});
            this.dgvRuleResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRuleResults.Location = new System.Drawing.Point(0, 0);
            this.dgvRuleResults.Name = "dgvRuleResults";
            this.dgvRuleResults.ReadOnly = true;
            this.dgvRuleResults.RowTemplate.Height = 23;
            this.dgvRuleResults.Size = new System.Drawing.Size(896, 176);
            this.dgvRuleResults.TabIndex = 0;
            //
            // colRuleName
            //
            this.colRuleName.HeaderText = "规则名称";
            this.colRuleName.Name = "colRuleName";
            this.colRuleName.ReadOnly = true;
            this.colRuleName.Width = 150;
            //
            // colRuleType
            //
            this.colRuleType.HeaderText = "规则类型";
            this.colRuleType.Name = "colRuleType";
            this.colRuleType.ReadOnly = true;
            this.colRuleType.Width = 120;
            //
            // colMatchedCount
            //
            this.colMatchedCount.HeaderText = "匹配数";
            this.colMatchedCount.Name = "colMatchedCount";
            this.colMatchedCount.ReadOnly = true;
            this.colMatchedCount.Width = 80;
            //
            // colSuccessCount
            //
            this.colSuccessCount.HeaderText = "成功数";
            this.colSuccessCount.Name = "colSuccessCount";
            this.colSuccessCount.ReadOnly = true;
            this.colSuccessCount.Width = 80;
            //
            // colFailedCount
            //
            this.colFailedCount.HeaderText = "失败数";
            this.colFailedCount.Name = "colFailedCount";
            this.colFailedCount.ReadOnly = true;
            this.colFailedCount.Width = 80;
            //
            // colStatus
            //
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 80;
            //
            // colElapsedTime
            //
            this.colElapsedTime.HeaderText = "耗时";
            this.colElapsedTime.Name = "colElapsedTime";
            this.colElapsedTime.ReadOnly = true;
            this.colElapsedTime.Width = 100;
            //
            // kryptonGroupBox2
            //
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 150);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            //
            // kryptonGroupBox2.Panel
            //
            this.kryptonGroupBox2.Panel.Controls.Add(this.dgvStatistics);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(900, 200);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "执行统计";
            //
            // dgvStatistics
            //
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            this.dgvStatistics.BackgroundColor = System.Drawing.Color.White;
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatistics.Location = new System.Drawing.Point(0, 0);
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.ReadOnly = true;
            this.dgvStatistics.RowTemplate.Height = 23;
            this.dgvStatistics.Size = new System.Drawing.Size(896, 176);
            this.dgvStatistics.TabIndex = 0;
            //
            // kryptonGroupBox1
            //
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            //
            // kryptonGroupBox1.Panel
            //
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonPanel3);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(900, 150);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "基本信息";
            //
            // kryptonPanel3
            //
            this.kryptonPanel3.Controls.Add(this.klblElapsedTime);
            this.kryptonPanel3.Controls.Add(this.klblExecuteTime);
            this.kryptonPanel3.Controls.Add(this.klblExecuteResult);
            this.kryptonPanel3.Controls.Add(this.klblExecuteMode);
            this.kryptonPanel3.Controls.Add(this.klblConfigName);
            this.kryptonPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel3.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel3.Name = "kryptonPanel3";
            this.kryptonPanel3.Size = new System.Drawing.Size(896, 126);
            this.kryptonPanel3.TabIndex = 0;
            //
            // klblElapsedTime
            //
            this.klblElapsedTime.Location = new System.Drawing.Point(20, 95);
            this.klblElapsedTime.Name = "klblElapsedTime";
            this.klblElapsedTime.Size = new System.Drawing.Size(50, 20);
            this.klblElapsedTime.TabIndex = 4;
            this.klblElapsedTime.Values.Text = "耗时:";
            //
            // klblExecuteTime
            //
            this.klblExecuteTime.Location = new System.Drawing.Point(20, 75);
            this.klblExecuteTime.Name = "klblExecuteTime";
            this.klblExecuteTime.Size = new System.Drawing.Size(75, 20);
            this.klblExecuteTime.TabIndex = 3;
            this.klblExecuteTime.Values.Text = "执行时间:";
            //
            // klblExecuteResult
            //
            this.klblExecuteResult.Location = new System.Drawing.Point(350, 45);
            this.klblExecuteResult.Name = "klblExecuteResult";
            this.klblExecuteResult.Size = new System.Drawing.Size(75, 20);
            this.klblExecuteResult.TabIndex = 2;
            this.klblExecuteResult.Values.Text = "执行结果:";
            //
            // klblExecuteMode
            //
            this.klblExecuteMode.Location = new System.Drawing.Point(20, 45);
            this.klblExecuteMode.Name = "klblExecuteMode";
            this.klblExecuteMode.Size = new System.Drawing.Size(75, 20);
            this.klblExecuteMode.TabIndex = 1;
            this.klblExecuteMode.Values.Text = "执行模式:";
            //
            // klblConfigName
            //
            this.klblConfigName.Location = new System.Drawing.Point(20, 15);
            this.klblConfigName.Name = "klblConfigName";
            this.klblConfigName.Size = new System.Drawing.Size(75, 20);
            this.klblConfigName.TabIndex = 0;
            this.klblConfigName.Values.Text = "配置名称:";
            //
            // FrmCleanupResult
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 760);
            this.Controls.Add(this.kryptonPanel2);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCleanupResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据清理执行结果";
            this.Load += new System.EventHandler(this.FrmCleanupResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).EndInit();
            this.kryptonGroupBox4.Panel.ResumeLayout(false);
            this.kryptonGroupBox4.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).EndInit();
            this.kryptonGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).EndInit();
            this.kryptonGroupBox3.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).EndInit();
            this.kryptonGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRuleResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel3)).EndInit();
            this.kryptonPanel3.ResumeLayout(false);
            this.kryptonPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnClose;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel3;
        private Krypton.Toolkit.KryptonLabel klblConfigName;
        private Krypton.Toolkit.KryptonLabel klblExecuteMode;
        private Krypton.Toolkit.KryptonLabel klblExecuteResult;
        private Krypton.Toolkit.KryptonLabel klblExecuteTime;
        private Krypton.Toolkit.KryptonLabel klblElapsedTime;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private System.Windows.Forms.DataGridView dgvStatistics;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox3;
        private System.Windows.Forms.DataGridView dgvRuleResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRuleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMatchedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSuccessCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailedCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colElapsedTime;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox4;
        private Krypton.Toolkit.KryptonTextBox ktxtLog;
        private Krypton.Toolkit.KryptonButton kbtnExport;
    }
}
