namespace RUINORERP.UI.IM
{
    partial class frmIMSetting
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
            this.kryptonGroupBox4 = new Krypton.Toolkit.KryptonGroupBox();
            this.lblQuietTimeTo = new Krypton.Toolkit.KryptonLabel();
            this.dtpQuietEnd = new Krypton.Toolkit.KryptonDateTimePicker();
            this.dtpQuietStart = new Krypton.Toolkit.KryptonDateTimePicker();
            this.chkQuietTime = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonGroupBox3 = new Krypton.Toolkit.KryptonGroupBox();
            this.cmbReminderFrequency = new Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.kryptonGroupBox2 = new Krypton.Toolkit.KryptonGroupBox();
            this.chkAutoOpenDocument = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonGroupBox1 = new Krypton.Toolkit.KryptonGroupBox();
            this.lblVolumeValue = new Krypton.Toolkit.KryptonLabel();
            this.trackBarVolume = new Krypton.Toolkit.KryptonTrackBar();
            this.chkVoiceReminder = new Krypton.Toolkit.KryptonCheckBox();
            this.kryptonPanel2 = new Krypton.Toolkit.KryptonPanel();
            this.btnRestoreDefaults = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnSave = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).BeginInit();
            this.kryptonGroupBox4.Panel.SuspendLayout();
            this.kryptonGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).BeginInit();
            this.kryptonGroupBox3.Panel.SuspendLayout();
            this.kryptonGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox4);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox3);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(548, 430);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonGroupBox4
            // 
            this.kryptonGroupBox4.GroupBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonGroupBox4.Location = new System.Drawing.Point(12, 304);
            this.kryptonGroupBox4.Name = "kryptonGroupBox4";
            // 
            // kryptonGroupBox4.Panel
            // 
            this.kryptonGroupBox4.Panel.Controls.Add(this.lblQuietTimeTo);
            this.kryptonGroupBox4.Panel.Controls.Add(this.dtpQuietEnd);
            this.kryptonGroupBox4.Panel.Controls.Add(this.dtpQuietStart);
            this.kryptonGroupBox4.Panel.Controls.Add(this.chkQuietTime);
            this.kryptonGroupBox4.Size = new System.Drawing.Size(460, 116);
            this.kryptonGroupBox4.TabIndex = 3;
            this.kryptonGroupBox4.Values.Heading = "免打扰时段设置";
            // 
            // lblQuietTimeTo
            // 
            this.lblQuietTimeTo.Location = new System.Drawing.Point(270, 50);
            this.lblQuietTimeTo.Name = "lblQuietTimeTo";
            this.lblQuietTimeTo.Size = new System.Drawing.Size(23, 20);
            this.lblQuietTimeTo.TabIndex = 3;
            this.lblQuietTimeTo.Values.Text = "至";
            // 
            // dtpQuietEnd
            // 
            this.dtpQuietEnd.CustomFormat = "HH:mm";
            this.dtpQuietEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpQuietEnd.Location = new System.Drawing.Point(300, 45);
            this.dtpQuietEnd.Name = "dtpQuietEnd";
            this.dtpQuietEnd.ShowUpDown = true;
            this.dtpQuietEnd.Size = new System.Drawing.Size(80, 21);
            this.dtpQuietEnd.TabIndex = 2;
            // 
            // dtpQuietStart
            // 
            this.dtpQuietStart.CustomFormat = "HH:mm";
            this.dtpQuietStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpQuietStart.Location = new System.Drawing.Point(180, 45);
            this.dtpQuietStart.Name = "dtpQuietStart";
            this.dtpQuietStart.ShowUpDown = true;
            this.dtpQuietStart.Size = new System.Drawing.Size(80, 21);
            this.dtpQuietStart.TabIndex = 1;
            // 
            // chkQuietTime
            // 
            this.chkQuietTime.Location = new System.Drawing.Point(20, 15);
            this.chkQuietTime.Name = "chkQuietTime";
            this.chkQuietTime.Size = new System.Drawing.Size(88, 20);
            this.chkQuietTime.TabIndex = 0;
            this.chkQuietTime.Values.Text = "启用免打扰";
            this.chkQuietTime.CheckedChanged += new System.EventHandler(this.chkQuietTime_CheckedChanged);
            // 
            // kryptonGroupBox3
            // 
            this.kryptonGroupBox3.GroupBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonGroupBox3.Location = new System.Drawing.Point(12, 210);
            this.kryptonGroupBox3.Name = "kryptonGroupBox3";
            // 
            // kryptonGroupBox3.Panel
            // 
            this.kryptonGroupBox3.Panel.Controls.Add(this.cmbReminderFrequency);
            this.kryptonGroupBox3.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox3.Size = new System.Drawing.Size(460, 74);
            this.kryptonGroupBox3.TabIndex = 2;
            this.kryptonGroupBox3.Values.Heading = "提醒频率设置";
            // 
            // cmbReminderFrequency
            // 
            this.cmbReminderFrequency.DropDownWidth = 121;
            this.cmbReminderFrequency.IntegralHeight = false;
            this.cmbReminderFrequency.Items.AddRange(new object[] {
            "1",
            "3",
            "5",
            "10",
            "15",
            "30"});
            this.cmbReminderFrequency.Location = new System.Drawing.Point(160, 15);
            this.cmbReminderFrequency.Name = "cmbReminderFrequency";
            this.cmbReminderFrequency.Size = new System.Drawing.Size(121, 21);
            this.cmbReminderFrequency.TabIndex = 1;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(20, 15);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(117, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "提醒频率（分钟）:";
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.GroupBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(12, 119);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.chkAutoOpenDocument);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(460, 73);
            this.kryptonGroupBox2.TabIndex = 1;
            this.kryptonGroupBox2.Values.Heading = "操作设置";
            // 
            // chkAutoOpenDocument
            // 
            this.chkAutoOpenDocument.Location = new System.Drawing.Point(20, 15);
            this.chkAutoOpenDocument.Name = "chkAutoOpenDocument";
            this.chkAutoOpenDocument.Size = new System.Drawing.Size(205, 20);
            this.chkAutoOpenDocument.TabIndex = 0;
            this.chkAutoOpenDocument.Values.Text = "双击业务消息自动打开对应单据";
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.GroupBackStyle = Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.lblVolumeValue);
            this.kryptonGroupBox1.Panel.Controls.Add(this.trackBarVolume);
            this.kryptonGroupBox1.Panel.Controls.Add(this.chkVoiceReminder);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(460, 101);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "语音提醒设置";
            // 
            // lblVolumeValue
            // 
            this.lblVolumeValue.Location = new System.Drawing.Point(400, 45);
            this.lblVolumeValue.Name = "lblVolumeValue";
            this.lblVolumeValue.Size = new System.Drawing.Size(34, 20);
            this.lblVolumeValue.TabIndex = 2;
            this.lblVolumeValue.Values.Text = "80%";
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(160, 40);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(234, 27);
            this.trackBarVolume.TabIndex = 1;
            this.trackBarVolume.Value = 80;
            this.trackBarVolume.Scroll += new System.EventHandler(this.trackBarVolume_Scroll);
            // 
            // chkVoiceReminder
            // 
            this.chkVoiceReminder.Location = new System.Drawing.Point(20, 15);
            this.chkVoiceReminder.Name = "chkVoiceReminder";
            this.chkVoiceReminder.Size = new System.Drawing.Size(101, 20);
            this.chkVoiceReminder.TabIndex = 0;
            this.chkVoiceReminder.Values.Text = "启用语音提醒";
            this.chkVoiceReminder.CheckedChanged += new System.EventHandler(this.chkVoiceReminder_CheckedChanged);
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Controls.Add(this.btnRestoreDefaults);
            this.kryptonPanel2.Controls.Add(this.btnCancel);
            this.kryptonPanel2.Controls.Add(this.btnSave);
            this.kryptonPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.kryptonPanel2.Location = new System.Drawing.Point(0, 430);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(548, 50);
            this.kryptonPanel2.TabIndex = 1;
            // 
            // btnRestoreDefaults
            // 
            this.btnRestoreDefaults.Location = new System.Drawing.Point(12, 12);
            this.btnRestoreDefaults.Name = "btnRestoreDefaults";
            this.btnRestoreDefaults.Size = new System.Drawing.Size(90, 25);
            this.btnRestoreDefaults.TabIndex = 2;
            this.btnRestoreDefaults.Values.Text = "恢复默认";
            this.btnRestoreDefaults.Click += new System.EventHandler(this.btnRestoreDefaults_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(382, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(286, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Values.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmIMSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 480);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.kryptonPanel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIMSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "消息提醒设置";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4.Panel)).EndInit();
            this.kryptonGroupBox4.Panel.ResumeLayout(false);
            this.kryptonGroupBox4.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox4)).EndInit();
            this.kryptonGroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3.Panel)).EndInit();
            this.kryptonGroupBox3.Panel.ResumeLayout(false);
            this.kryptonGroupBox3.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox3)).EndInit();
            this.kryptonGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbReminderFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            this.kryptonGroupBox2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox4;
        private Krypton.Toolkit.KryptonLabel lblQuietTimeTo;
        private Krypton.Toolkit.KryptonDateTimePicker dtpQuietEnd;
        private Krypton.Toolkit.KryptonDateTimePicker dtpQuietStart;
        private Krypton.Toolkit.KryptonCheckBox chkQuietTime;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox3;
        private Krypton.Toolkit.KryptonComboBox cmbReminderFrequency;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private Krypton.Toolkit.KryptonCheckBox chkAutoOpenDocument;
        private Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private Krypton.Toolkit.KryptonLabel lblVolumeValue;
        private Krypton.Toolkit.KryptonTrackBar trackBarVolume;
        private Krypton.Toolkit.KryptonCheckBox chkVoiceReminder;
        private Krypton.Toolkit.KryptonPanel kryptonPanel2;
        private Krypton.Toolkit.KryptonButton btnRestoreDefaults;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnSave;
    }
}