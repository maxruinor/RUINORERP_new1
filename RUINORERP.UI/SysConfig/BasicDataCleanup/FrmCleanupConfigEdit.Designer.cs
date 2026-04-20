namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    partial class FrmCleanupConfigEdit
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kbtnCancel = new Krypton.Toolkit.KryptonButton();
            this.kbtnOK = new Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.knumMaxProcessCount = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel8 = new Krypton.Toolkit.KryptonLabel();
            this.kchkAllowTestMode = new Krypton.Toolkit.KryptonCheckBox();
            this.kchkEnableDetailedLog = new Krypton.Toolkit.KryptonCheckBox();
            this.ktxtBackupSuffix = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel7 = new Krypton.Toolkit.KryptonLabel();
            this.kchkEnableBackup = new Krypton.Toolkit.KryptonCheckBox();
            this.knumTransactionBatchSize = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel6 = new Krypton.Toolkit.KryptonLabel();
            this.kchkEnableTransaction = new Krypton.Toolkit.KryptonCheckBox();
            this.ktxtDescription = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.ktxtConfigName = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // kryptonPanel1
            //
            this.kryptonPanel1.Controls.Add(this.kbtnCancel);
            this.kryptonPanel1.Controls.Add(this.kbtnOK);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 420);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(550, 60);
            this.kryptonPanel1.TabIndex = 0;
            //
            // kbtnCancel
            //
            this.kbtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.kbtnCancel.Location = new System.Drawing.Point(280, 18);
            this.kbtnCancel.Name = "kbtnCancel";
            this.kbtnCancel.Size = new System.Drawing.Size(90, 28);
            this.kbtnCancel.TabIndex = 1;
            this.kbtnCancel.Values.Text = "取消";
            this.kbtnCancel.Click += new System.EventHandler(this.KbtnCancel_Click);
            //
            // kbtnOK
            //
            this.kbtnOK.Location = new System.Drawing.Point(180, 18);
            this.kbtnOK.Name = "kbtnOK";
            this.kbtnOK.Size = new System.Drawing.Size(90, 28);
            this.kbtnOK.TabIndex = 0;
            this.kbtnOK.Values.Text = "确定";
            this.kbtnOK.Click += new System.EventHandler(this.KbtnOK_Click);
            //
            // kryptonGroupBox1
            //
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            //
            // kryptonGroupBox1.Panel
            //
            this.kryptonGroupBox1.Panel.Controls.Add(this.knumMaxProcessCount);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel8);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkAllowTestMode);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkEnableDetailedLog);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtBackupSuffix);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel7);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkEnableBackup);
            this.kryptonGroupBox1.Panel.Controls.Add(this.knumTransactionBatchSize);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel6);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kchkEnableTransaction);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtDescription);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.ktxtConfigName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(550, 420);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "配置信息";
            //
            // knumMaxProcessCount
            //
            this.knumMaxProcessCount.Location = new System.Drawing.Point(130, 320);
            this.knumMaxProcessCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.knumMaxProcessCount.Name = "knumMaxProcessCount";
            this.knumMaxProcessCount.Size = new System.Drawing.Size(120, 22);
            this.knumMaxProcessCount.TabIndex = 13;
            this.knumMaxProcessCount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            //
            // kryptonLabel8
            //
            this.kryptonLabel8.Location = new System.Drawing.Point(20, 320);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(103, 20);
            this.kryptonLabel8.TabIndex = 12;
            this.kryptonLabel8.Values.Text = "最大处理记录数:";
            //
            // kchkAllowTestMode
            //
            this.kchkAllowTestMode.Checked = true;
            this.kchkAllowTestMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kchkAllowTestMode.Location = new System.Drawing.Point(20, 290);
            this.kchkAllowTestMode.Name = "kchkAllowTestMode";
            this.kchkAllowTestMode.Size = new System.Drawing.Size(123, 20);
            this.kchkAllowTestMode.TabIndex = 11;
            this.kchkAllowTestMode.Values.Text = "允许测试模式执行";
            //
            // kchkEnableDetailedLog
            //
            this.kchkEnableDetailedLog.Checked = true;
            this.kchkEnableDetailedLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kchkEnableDetailedLog.Location = new System.Drawing.Point(20, 260);
            this.kchkEnableDetailedLog.Name = "kchkEnableDetailedLog";
            this.kchkEnableDetailedLog.Size = new System.Drawing.Size(123, 20);
            this.kchkEnableDetailedLog.TabIndex = 10;
            this.kchkEnableDetailedLog.Values.Text = "启用详细执行日志";
            //
            // ktxtBackupSuffix
            //
            this.ktxtBackupSuffix.Location = new System.Drawing.Point(130, 225);
            this.ktxtBackupSuffix.Name = "ktxtBackupSuffix";
            this.ktxtBackupSuffix.Size = new System.Drawing.Size(200, 23);
            this.ktxtBackupSuffix.TabIndex = 9;
            this.ktxtBackupSuffix.Text = "_Backup_20240101";
            //
            // kryptonLabel7
            //
            this.kryptonLabel7.Location = new System.Drawing.Point(20, 225);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(90, 20);
            this.kryptonLabel7.TabIndex = 8;
            this.kryptonLabel7.Values.Text = "备份表后缀:";
            //
            // kchkEnableBackup
            //
            this.kchkEnableBackup.Checked = true;
            this.kchkEnableBackup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kchkEnableBackup.Location = new System.Drawing.Point(20, 195);
            this.kchkEnableBackup.Name = "kchkEnableBackup";
            this.kchkEnableBackup.Size = new System.Drawing.Size(123, 20);
            this.kchkEnableBackup.TabIndex = 7;
            this.kchkEnableBackup.Values.Text = "执行前备份数据";
            //
            // knumTransactionBatchSize
            //
            this.knumTransactionBatchSize.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.knumTransactionBatchSize.Location = new System.Drawing.Point(130, 160);
            this.knumTransactionBatchSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.knumTransactionBatchSize.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.knumTransactionBatchSize.Name = "knumTransactionBatchSize";
            this.knumTransactionBatchSize.Size = new System.Drawing.Size(120, 22);
            this.knumTransactionBatchSize.TabIndex = 6;
            this.knumTransactionBatchSize.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            //
            // kryptonLabel6
            //
            this.kryptonLabel6.Location = new System.Drawing.Point(20, 160);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(103, 20);
            this.kryptonLabel6.TabIndex = 5;
            this.kryptonLabel6.Values.Text = "事务批量大小:";
            //
            // kchkEnableTransaction
            //
            this.kchkEnableTransaction.Checked = true;
            this.kchkEnableTransaction.CheckState = System.Windows.Forms.CheckState.Checked;
            this.kchkEnableTransaction.Location = new System.Drawing.Point(20, 130);
            this.kchkEnableTransaction.Name = "kchkEnableTransaction";
            this.kchkEnableTransaction.Size = new System.Drawing.Size(98, 20);
            this.kchkEnableTransaction.TabIndex = 4;
            this.kchkEnableTransaction.Values.Text = "启用事务处理";
            //
            // ktxtDescription
            //
            this.ktxtDescription.Location = new System.Drawing.Point(90, 55);
            this.ktxtDescription.Multiline = true;
            this.ktxtDescription.Name = "ktxtDescription";
            this.ktxtDescription.Size = new System.Drawing.Size(420, 60);
            this.ktxtDescription.TabIndex = 3;
            //
            // kryptonLabel2
            //
            this.kryptonLabel2.Location = new System.Drawing.Point(20, 55);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "配置描述:";
            //
            // ktxtConfigName
            //
            this.ktxtConfigName.Location = new System.Drawing.Point(90, 20);
            this.ktxtConfigName.Name = "ktxtConfigName";
            this.ktxtConfigName.Size = new System.Drawing.Size(280, 23);
            this.ktxtConfigName.TabIndex = 1;
            //
            // kryptonLabel1
            //
            this.kryptonLabel1.Location = new System.Drawing.Point(20, 20);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(65, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "配置名称:";
            //
            // FrmCleanupConfigEdit
            //
            this.AcceptButton = this.kbtnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.kbtnCancel;
            this.ClientSize = new System.Drawing.Size(550, 480);
            this.Controls.Add(this.kryptonGroupBox1);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCleanupConfigEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "清理配置编辑";
            this.Load += new System.EventHandler(this.FrmCleanupConfigEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonButton kbtnCancel;
        private Krypton.Toolkit.KryptonButton kbtnOK;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonTextBox ktxtConfigName;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox ktxtDescription;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonCheckBox kchkEnableTransaction;
        private Krypton.Toolkit.KryptonNumericUpDown knumTransactionBatchSize;
        private Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private Krypton.Toolkit.KryptonCheckBox kchkEnableBackup;
        private Krypton.Toolkit.KryptonTextBox ktxtBackupSuffix;
        private Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private Krypton.Toolkit.KryptonCheckBox kchkEnableDetailedLog;
        private Krypton.Toolkit.KryptonCheckBox kchkAllowTestMode;
        private Krypton.Toolkit.KryptonNumericUpDown knumMaxProcessCount;
        private Krypton.Toolkit.KryptonLabel kryptonLabel8;
    }
}
