namespace RUINORERP.Server.Controls
{
    partial class SystemManagementControl
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
            this.tabControlSystemManagement = new System.Windows.Forms.TabControl();
            this.tabPageSystemConfig = new System.Windows.Forms.TabPage();
            this.groupBoxLoggingConfig = new System.Windows.Forms.GroupBox();
            this.textBoxLogLevel = new System.Windows.Forms.TextBox();
            this.labelLogLevel = new System.Windows.Forms.Label();
            this.checkBoxEnableLogging = new System.Windows.Forms.CheckBox();
            this.groupBoxCacheConfig = new System.Windows.Forms.GroupBox();
            this.textBoxCacheConnectionString = new System.Windows.Forms.TextBox();
            this.labelCacheConnectionString = new System.Windows.Forms.Label();
            this.textBoxCacheType = new System.Windows.Forms.TextBox();
            this.labelCacheType = new System.Windows.Forms.Label();
            this.groupBoxDatabaseConfig = new System.Windows.Forms.GroupBox();
            this.textBoxDbType = new System.Windows.Forms.TextBox();
            this.labelDbType = new System.Windows.Forms.Label();
            this.textBoxDbConnectionString = new System.Windows.Forms.TextBox();
            this.labelDbConnectionString = new System.Windows.Forms.Label();
            this.groupBoxBasicConfig = new System.Windows.Forms.GroupBox();
            this.textBoxSomeSetting = new System.Windows.Forms.TextBox();
            this.labelSomeSetting = new System.Windows.Forms.Label();
            this.textBoxHeartbeatInterval = new System.Windows.Forms.TextBox();
            this.labelHeartbeatInterval = new System.Windows.Forms.Label();
            this.textBoxMaxConnections = new System.Windows.Forms.TextBox();
            this.labelMaxConnections = new System.Windows.Forms.Label();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.textBoxServerName = new System.Windows.Forms.TextBox();
            this.labelServerName = new System.Windows.Forms.Label();
            this.panelConfigButtons = new System.Windows.Forms.Panel();
            this.btnResetConfig = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnLoadConfig = new System.Windows.Forms.Button();
            this.tabPageRegistration = new System.Windows.Forms.TabPage();
            this.groupBoxRegistrationInfo = new System.Windows.Forms.GroupBox();
            this.dateTimePickerExpirationDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerRegistrationDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerPurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.checkBoxIsRegistered = new System.Windows.Forms.CheckBox();
            this.labelExpirationDate = new System.Windows.Forms.Label();
            this.labelRegistrationDate = new System.Windows.Forms.Label();
            this.labelPurchaseDate = new System.Windows.Forms.Label();
            this.textBoxRegistrationCode = new System.Windows.Forms.TextBox();
            this.labelRegistrationCode = new System.Windows.Forms.Label();
            this.textBoxMachineCode = new System.Windows.Forms.TextBox();
            this.labelMachineCode = new System.Windows.Forms.Label();
            this.btnGenerateMachineCode = new System.Windows.Forms.Button();
            this.textBoxFunctionModule = new System.Windows.Forms.TextBox();
            this.labelFunctionModule = new System.Windows.Forms.Label();
            this.textBoxLicenseType = new System.Windows.Forms.TextBox();
            this.labelLicenseType = new System.Windows.Forms.Label();
            this.textBoxProductVersion = new System.Windows.Forms.TextBox();
            this.labelProductVersion = new System.Windows.Forms.Label();
            this.textBoxConcurrentUsers = new System.Windows.Forms.TextBox();
            this.labelConcurrentUsers = new System.Windows.Forms.Label();
            this.textBoxPhoneNumber = new System.Windows.Forms.TextBox();
            this.labelPhoneNumber = new System.Windows.Forms.Label();
            this.textBoxContactName = new System.Windows.Forms.TextBox();
            this.labelContactName = new System.Windows.Forms.Label();
            this.textBoxCompanyName = new System.Windows.Forms.TextBox();
            this.labelCompanyName = new System.Windows.Forms.Label();
            this.panelRegistrationButtons = new System.Windows.Forms.Panel();
            this.btnValidateRegistration = new System.Windows.Forms.Button();
            this.btnSaveRegistration = new System.Windows.Forms.Button();
            this.btnLoadRegistration = new System.Windows.Forms.Button();
            this.tabControlSystemManagement.SuspendLayout();
            this.tabPageSystemConfig.SuspendLayout();
            this.groupBoxLoggingConfig.SuspendLayout();
            this.groupBoxCacheConfig.SuspendLayout();
            this.groupBoxDatabaseConfig.SuspendLayout();
            this.groupBoxBasicConfig.SuspendLayout();
            this.panelConfigButtons.SuspendLayout();
            this.tabPageRegistration.SuspendLayout();
            this.groupBoxRegistrationInfo.SuspendLayout();
            this.panelRegistrationButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlSystemManagement
            // 
            this.tabControlSystemManagement.Controls.Add(this.tabPageSystemConfig);
            this.tabControlSystemManagement.Controls.Add(this.tabPageRegistration);
            this.tabControlSystemManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSystemManagement.Location = new System.Drawing.Point(0, 0);
            this.tabControlSystemManagement.Name = "tabControlSystemManagement";
            this.tabControlSystemManagement.SelectedIndex = 0;
            this.tabControlSystemManagement.Size = new System.Drawing.Size(800, 600);
            this.tabControlSystemManagement.TabIndex = 0;
            // 
            // tabPageSystemConfig
            // 
            this.tabPageSystemConfig.Controls.Add(this.groupBoxLoggingConfig);
            this.tabPageSystemConfig.Controls.Add(this.groupBoxCacheConfig);
            this.tabPageSystemConfig.Controls.Add(this.groupBoxDatabaseConfig);
            this.tabPageSystemConfig.Controls.Add(this.groupBoxBasicConfig);
            this.tabPageSystemConfig.Controls.Add(this.panelConfigButtons);
            this.tabPageSystemConfig.Location = new System.Drawing.Point(4, 22);
            this.tabPageSystemConfig.Name = "tabPageSystemConfig";
            this.tabPageSystemConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystemConfig.Size = new System.Drawing.Size(792, 574);
            this.tabPageSystemConfig.TabIndex = 0;
            this.tabPageSystemConfig.Text = "系统配置";
            this.tabPageSystemConfig.UseVisualStyleBackColor = true;
            // 
            // groupBoxLoggingConfig
            // 
            this.groupBoxLoggingConfig.Controls.Add(this.textBoxLogLevel);
            this.groupBoxLoggingConfig.Controls.Add(this.labelLogLevel);
            this.groupBoxLoggingConfig.Controls.Add(this.checkBoxEnableLogging);
            this.groupBoxLoggingConfig.Location = new System.Drawing.Point(15, 420);
            this.groupBoxLoggingConfig.Name = "groupBoxLoggingConfig";
            this.groupBoxLoggingConfig.Size = new System.Drawing.Size(760, 80);
            this.groupBoxLoggingConfig.TabIndex = 4;
            this.groupBoxLoggingConfig.TabStop = false;
            this.groupBoxLoggingConfig.Text = "日志配置";
            // 
            // textBoxLogLevel
            // 
            this.textBoxLogLevel.Location = new System.Drawing.Point(180, 35);
            this.textBoxLogLevel.Name = "textBoxLogLevel";
            this.textBoxLogLevel.Size = new System.Drawing.Size(150, 21);
            this.textBoxLogLevel.TabIndex = 2;
            // 
            // labelLogLevel
            // 
            this.labelLogLevel.AutoSize = true;
            this.labelLogLevel.Location = new System.Drawing.Point(120, 38);
            this.labelLogLevel.Name = "labelLogLevel";
            this.labelLogLevel.Size = new System.Drawing.Size(59, 12);
            this.labelLogLevel.TabIndex = 1;
            this.labelLogLevel.Text = "日志级别:";
            // 
            // checkBoxEnableLogging
            // 
            this.checkBoxEnableLogging.AutoSize = true;
            this.checkBoxEnableLogging.Location = new System.Drawing.Point(20, 37);
            this.checkBoxEnableLogging.Name = "checkBoxEnableLogging";
            this.checkBoxEnableLogging.Size = new System.Drawing.Size(72, 16);
            this.checkBoxEnableLogging.TabIndex = 0;
            this.checkBoxEnableLogging.Text = "启用日志";
            this.checkBoxEnableLogging.UseVisualStyleBackColor = true;
            // 
            // groupBoxCacheConfig
            // 
            this.groupBoxCacheConfig.Controls.Add(this.textBoxCacheConnectionString);
            this.groupBoxCacheConfig.Controls.Add(this.labelCacheConnectionString);
            this.groupBoxCacheConfig.Controls.Add(this.textBoxCacheType);
            this.groupBoxCacheConfig.Controls.Add(this.labelCacheType);
            this.groupBoxCacheConfig.Location = new System.Drawing.Point(15, 320);
            this.groupBoxCacheConfig.Name = "groupBoxCacheConfig";
            this.groupBoxCacheConfig.Size = new System.Drawing.Size(760, 80);
            this.groupBoxCacheConfig.TabIndex = 3;
            this.groupBoxCacheConfig.TabStop = false;
            this.groupBoxCacheConfig.Text = "缓存配置";
            // 
            // textBoxCacheConnectionString
            // 
            this.textBoxCacheConnectionString.Location = new System.Drawing.Point(320, 35);
            this.textBoxCacheConnectionString.Name = "textBoxCacheConnectionString";
            this.textBoxCacheConnectionString.Size = new System.Drawing.Size(250, 21);
            this.textBoxCacheConnectionString.TabIndex = 3;
            // 
            // labelCacheConnectionString
            // 
            this.labelCacheConnectionString.AutoSize = true;
            this.labelCacheConnectionString.Location = new System.Drawing.Point(260, 38);
            this.labelCacheConnectionString.Name = "labelCacheConnectionString";
            this.labelCacheConnectionString.Size = new System.Drawing.Size(59, 12);
            this.labelCacheConnectionString.TabIndex = 2;
            this.labelCacheConnectionString.Text = "连接字符串:";
            // 
            // textBoxCacheType
            // 
            this.textBoxCacheType.Location = new System.Drawing.Point(80, 35);
            this.textBoxCacheType.Name = "textBoxCacheType";
            this.textBoxCacheType.Size = new System.Drawing.Size(150, 21);
            this.textBoxCacheType.TabIndex = 1;
            // 
            // labelCacheType
            // 
            this.labelCacheType.AutoSize = true;
            this.labelCacheType.Location = new System.Drawing.Point(20, 38);
            this.labelCacheType.Name = "labelCacheType";
            this.labelCacheType.Size = new System.Drawing.Size(59, 12);
            this.labelCacheType.TabIndex = 0;
            this.labelCacheType.Text = "缓存类型:";
            // 
            // groupBoxDatabaseConfig
            // 
            this.groupBoxDatabaseConfig.Controls.Add(this.textBoxDbType);
            this.groupBoxDatabaseConfig.Controls.Add(this.labelDbType);
            this.groupBoxDatabaseConfig.Controls.Add(this.textBoxDbConnectionString);
            this.groupBoxDatabaseConfig.Controls.Add(this.labelDbConnectionString);
            this.groupBoxDatabaseConfig.Location = new System.Drawing.Point(15, 220);
            this.groupBoxDatabaseConfig.Name = "groupBoxDatabaseConfig";
            this.groupBoxDatabaseConfig.Size = new System.Drawing.Size(760, 80);
            this.groupBoxDatabaseConfig.TabIndex = 2;
            this.groupBoxDatabaseConfig.TabStop = false;
            this.groupBoxDatabaseConfig.Text = "数据库配置";
            // 
            // textBoxDbType
            // 
            this.textBoxDbType.Location = new System.Drawing.Point(80, 35);
            this.textBoxDbType.Name = "textBoxDbType";
            this.textBoxDbType.Size = new System.Drawing.Size(150, 21);
            this.textBoxDbType.TabIndex = 1;
            // 
            // labelDbType
            // 
            this.labelDbType.AutoSize = true;
            this.labelDbType.Location = new System.Drawing.Point(20, 38);
            this.labelDbType.Name = "labelDbType";
            this.labelDbType.Size = new System.Drawing.Size(59, 12);
            this.labelDbType.TabIndex = 0;
            this.labelDbType.Text = "数据库类型:";
            // 
            // textBoxDbConnectionString
            // 
            this.textBoxDbConnectionString.Location = new System.Drawing.Point(320, 35);
            this.textBoxDbConnectionString.Name = "textBoxDbConnectionString";
            this.textBoxDbConnectionString.Size = new System.Drawing.Size(420, 21);
            this.textBoxDbConnectionString.TabIndex = 3;
            // 
            // labelDbConnectionString
            // 
            this.labelDbConnectionString.AutoSize = true;
            this.labelDbConnectionString.Location = new System.Drawing.Point(260, 38);
            this.labelDbConnectionString.Name = "labelDbConnectionString";
            this.labelDbConnectionString.Size = new System.Drawing.Size(59, 12);
            this.labelDbConnectionString.TabIndex = 2;
            this.labelDbConnectionString.Text = "连接字符串:";
            // 
            // groupBoxBasicConfig
            // 
            this.groupBoxBasicConfig.Controls.Add(this.textBoxSomeSetting);
            this.groupBoxBasicConfig.Controls.Add(this.labelSomeSetting);
            this.groupBoxBasicConfig.Controls.Add(this.textBoxHeartbeatInterval);
            this.groupBoxBasicConfig.Controls.Add(this.labelHeartbeatInterval);
            this.groupBoxBasicConfig.Controls.Add(this.textBoxMaxConnections);
            this.groupBoxBasicConfig.Controls.Add(this.labelMaxConnections);
            this.groupBoxBasicConfig.Controls.Add(this.textBoxServerPort);
            this.groupBoxBasicConfig.Controls.Add(this.labelServerPort);
            this.groupBoxBasicConfig.Controls.Add(this.textBoxServerName);
            this.groupBoxBasicConfig.Controls.Add(this.labelServerName);
            this.groupBoxBasicConfig.Location = new System.Drawing.Point(15, 20);
            this.groupBoxBasicConfig.Name = "groupBoxBasicConfig";
            this.groupBoxBasicConfig.Size = new System.Drawing.Size(760, 180);
            this.groupBoxBasicConfig.TabIndex = 1;
            this.groupBoxBasicConfig.TabStop = false;
            this.groupBoxBasicConfig.Text = "基本配置";
            // 
            // textBoxSomeSetting
            // 
            this.textBoxSomeSetting.Location = new System.Drawing.Point(120, 135);
            this.textBoxSomeSetting.Name = "textBoxSomeSetting";
            this.textBoxSomeSetting.Size = new System.Drawing.Size(250, 21);
            this.textBoxSomeSetting.TabIndex = 9;
            // 
            // labelSomeSetting
            // 
            this.labelSomeSetting.AutoSize = true;
            this.labelSomeSetting.Location = new System.Drawing.Point(20, 138);
            this.labelSomeSetting.Name = "labelSomeSetting";
            this.labelSomeSetting.Size = new System.Drawing.Size(95, 12);
            this.labelSomeSetting.TabIndex = 8;
            this.labelSomeSetting.Text = "其他配置项名称:";
            // 
            // textBoxHeartbeatInterval
            // 
            this.textBoxHeartbeatInterval.Location = new System.Drawing.Point(120, 100);
            this.textBoxHeartbeatInterval.Name = "textBoxHeartbeatInterval";
            this.textBoxHeartbeatInterval.Size = new System.Drawing.Size(150, 21);
            this.textBoxHeartbeatInterval.TabIndex = 7;
            // 
            // labelHeartbeatInterval
            // 
            this.labelHeartbeatInterval.AutoSize = true;
            this.labelHeartbeatInterval.Location = new System.Drawing.Point(20, 103);
            this.labelHeartbeatInterval.Name = "labelHeartbeatInterval";
            this.labelHeartbeatInterval.Size = new System.Drawing.Size(83, 12);
            this.labelHeartbeatInterval.TabIndex = 6;
            this.labelHeartbeatInterval.Text = "心跳间隔(秒):";
            // 
            // textBoxMaxConnections
            // 
            this.textBoxMaxConnections.Location = new System.Drawing.Point(120, 65);
            this.textBoxMaxConnections.Name = "textBoxMaxConnections";
            this.textBoxMaxConnections.Size = new System.Drawing.Size(150, 21);
            this.textBoxMaxConnections.TabIndex = 5;
            // 
            // labelMaxConnections
            // 
            this.labelMaxConnections.AutoSize = true;
            this.labelMaxConnections.Location = new System.Drawing.Point(20, 68);
            this.labelMaxConnections.Name = "labelMaxConnections";
            this.labelMaxConnections.Size = new System.Drawing.Size(71, 12);
            this.labelMaxConnections.TabIndex = 4;
            this.labelMaxConnections.Text = "最大连接数:";
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(350, 30);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(100, 21);
            this.textBoxServerPort.TabIndex = 3;
            // 
            // labelServerPort
            // 
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.Location = new System.Drawing.Point(290, 33);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(59, 12);
            this.labelServerPort.TabIndex = 2;
            this.labelServerPort.Text = "服务器端口:";
            // 
            // textBoxServerName
            // 
            this.textBoxServerName.Location = new System.Drawing.Point(120, 30);
            this.textBoxServerName.Name = "textBoxServerName";
            this.textBoxServerName.Size = new System.Drawing.Size(150, 21);
            this.textBoxServerName.TabIndex = 1;
            // 
            // labelServerName
            // 
            this.labelServerName.AutoSize = true;
            this.labelServerName.Location = new System.Drawing.Point(20, 33);
            this.labelServerName.Name = "labelServerName";
            this.labelServerName.Size = new System.Drawing.Size(71, 12);
            this.labelServerName.TabIndex = 0;
            this.labelServerName.Text = "服务器名称:";
            // 
            // panelConfigButtons
            // 
            this.panelConfigButtons.Controls.Add(this.btnResetConfig);
            this.panelConfigButtons.Controls.Add(this.btnSaveConfig);
            this.panelConfigButtons.Controls.Add(this.btnLoadConfig);
            this.panelConfigButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelConfigButtons.Location = new System.Drawing.Point(3, 524);
            this.panelConfigButtons.Name = "panelConfigButtons";
            this.panelConfigButtons.Size = new System.Drawing.Size(786, 47);
            this.panelConfigButtons.TabIndex = 0;
            // 
            // btnResetConfig
            // 
            this.btnResetConfig.Location = new System.Drawing.Point(200, 12);
            this.btnResetConfig.Name = "btnResetConfig";
            this.btnResetConfig.Size = new System.Drawing.Size(75, 23);
            this.btnResetConfig.TabIndex = 2;
            this.btnResetConfig.Text = "重置配置";
            this.btnResetConfig.UseVisualStyleBackColor = true;
            this.btnResetConfig.Click += new System.EventHandler(this.btnResetConfig_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(100, 12);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 1;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnLoadConfig
            // 
            this.btnLoadConfig.Location = new System.Drawing.Point(15, 12);
            this.btnLoadConfig.Name = "btnLoadConfig";
            this.btnLoadConfig.Size = new System.Drawing.Size(75, 23);
            this.btnLoadConfig.TabIndex = 0;
            this.btnLoadConfig.Text = "重新加载";
            this.btnLoadConfig.UseVisualStyleBackColor = true;
            this.btnLoadConfig.Click += new System.EventHandler(this.btnLoadConfig_Click);
            // 
            // tabPageRegistration
            // 
            this.tabPageRegistration.Controls.Add(this.groupBoxRegistrationInfo);
            this.tabPageRegistration.Controls.Add(this.panelRegistrationButtons);
            this.tabPageRegistration.Location = new System.Drawing.Point(4, 22);
            this.tabPageRegistration.Name = "tabPageRegistration";
            this.tabPageRegistration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRegistration.Size = new System.Drawing.Size(792, 574);
            this.tabPageRegistration.TabIndex = 1;
            this.tabPageRegistration.Text = "注册信息";
            this.tabPageRegistration.UseVisualStyleBackColor = true;
            // 
            // groupBoxRegistrationInfo
            // 
            this.groupBoxRegistrationInfo.Controls.Add(this.dateTimePickerExpirationDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.dateTimePickerRegistrationDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.dateTimePickerPurchaseDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.checkBoxIsRegistered);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelExpirationDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelRegistrationDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelPurchaseDate);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxRegistrationCode);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelRegistrationCode);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxMachineCode);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelMachineCode);
            this.groupBoxRegistrationInfo.Controls.Add(this.btnGenerateMachineCode);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxFunctionModule);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelFunctionModule);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxLicenseType);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelLicenseType);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxProductVersion);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelProductVersion);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxConcurrentUsers);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelConcurrentUsers);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxPhoneNumber);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelPhoneNumber);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxContactName);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelContactName);
            this.groupBoxRegistrationInfo.Controls.Add(this.textBoxCompanyName);
            this.groupBoxRegistrationInfo.Controls.Add(this.labelCompanyName);
            this.groupBoxRegistrationInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRegistrationInfo.Location = new System.Drawing.Point(3, 3);
            this.groupBoxRegistrationInfo.Name = "groupBoxRegistrationInfo";
            this.groupBoxRegistrationInfo.Size = new System.Drawing.Size(786, 521);
            this.groupBoxRegistrationInfo.TabIndex = 1;
            this.groupBoxRegistrationInfo.TabStop = false;
            this.groupBoxRegistrationInfo.Text = "注册信息";
            // 
            // dateTimePickerExpirationDate
            // 
            this.dateTimePickerExpirationDate.Location = new System.Drawing.Point(120, 345);
            this.dateTimePickerExpirationDate.Name = "dateTimePickerExpirationDate";
            this.dateTimePickerExpirationDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePickerExpirationDate.TabIndex = 25;
            // 
            // dateTimePickerRegistrationDate
            // 
            this.dateTimePickerRegistrationDate.Location = new System.Drawing.Point(120, 310);
            this.dateTimePickerRegistrationDate.Name = "dateTimePickerRegistrationDate";
            this.dateTimePickerRegistrationDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePickerRegistrationDate.TabIndex = 24;
            // 
            // dateTimePickerPurchaseDate
            // 
            this.dateTimePickerPurchaseDate.Location = new System.Drawing.Point(120, 275);
            this.dateTimePickerPurchaseDate.Name = "dateTimePickerPurchaseDate";
            this.dateTimePickerPurchaseDate.Size = new System.Drawing.Size(200, 21);
            this.dateTimePickerPurchaseDate.TabIndex = 23;
            // 
            // checkBoxIsRegistered
            // 
            this.checkBoxIsRegistered.AutoSize = true;
            this.checkBoxIsRegistered.Location = new System.Drawing.Point(120, 245);
            this.checkBoxIsRegistered.Name = "checkBoxIsRegistered";
            this.checkBoxIsRegistered.Size = new System.Drawing.Size(72, 16);
            this.checkBoxIsRegistered.TabIndex = 22;
            this.checkBoxIsRegistered.Text = "已注册";
            this.checkBoxIsRegistered.UseVisualStyleBackColor = true;
            // 
            // labelExpirationDate
            // 
            this.labelExpirationDate.AutoSize = true;
            this.labelExpirationDate.Location = new System.Drawing.Point(20, 350);
            this.labelExpirationDate.Name = "labelExpirationDate";
            this.labelExpirationDate.Size = new System.Drawing.Size(59, 12);
            this.labelExpirationDate.TabIndex = 21;
            this.labelExpirationDate.Text = "过期时间:";
            // 
            // labelRegistrationDate
            // 
            this.labelRegistrationDate.AutoSize = true;
            this.labelRegistrationDate.Location = new System.Drawing.Point(20, 315);
            this.labelRegistrationDate.Name = "labelRegistrationDate";
            this.labelRegistrationDate.Size = new System.Drawing.Size(59, 12);
            this.labelRegistrationDate.TabIndex = 20;
            this.labelRegistrationDate.Text = "注册时间:";
            // 
            // labelPurchaseDate
            // 
            this.labelPurchaseDate.AutoSize = true;
            this.labelPurchaseDate.Location = new System.Drawing.Point(20, 280);
            this.labelPurchaseDate.Name = "labelPurchaseDate";
            this.labelPurchaseDate.Size = new System.Drawing.Size(59, 12);
            this.labelPurchaseDate.TabIndex = 19;
            this.labelPurchaseDate.Text = "购买时间:";
            // 
            // textBoxRegistrationCode
            // 
            this.textBoxRegistrationCode.Location = new System.Drawing.Point(120, 210);
            this.textBoxRegistrationCode.Name = "textBoxRegistrationCode";
            this.textBoxRegistrationCode.Size = new System.Drawing.Size(400, 21);
            this.textBoxRegistrationCode.TabIndex = 18;
            // 
            // labelRegistrationCode
            // 
            this.labelRegistrationCode.AutoSize = true;
            this.labelRegistrationCode.Location = new System.Drawing.Point(20, 215);
            this.labelRegistrationCode.Name = "labelRegistrationCode";
            this.labelRegistrationCode.Size = new System.Drawing.Size(59, 12);
            this.labelRegistrationCode.TabIndex = 17;
            this.labelRegistrationCode.Text = "注册码:";
            // 
            // textBoxMachineCode
            // 
            this.textBoxMachineCode.Location = new System.Drawing.Point(120, 175);
            this.textBoxMachineCode.Name = "textBoxMachineCode";
            this.textBoxMachineCode.Size = new System.Drawing.Size(400, 21);
            this.textBoxMachineCode.TabIndex = 16;
            // 
            // labelMachineCode
            // 
            this.labelMachineCode.AutoSize = true;
            this.labelMachineCode.Location = new System.Drawing.Point(20, 180);
            this.labelMachineCode.Name = "labelMachineCode";
            this.labelMachineCode.Size = new System.Drawing.Size(59, 12);
            this.labelMachineCode.TabIndex = 15;
            this.labelMachineCode.Text = "机器码:";
            // 
            // btnGenerateMachineCode
            // 
            this.btnGenerateMachineCode.Location = new System.Drawing.Point(530, 173);
            this.btnGenerateMachineCode.Name = "btnGenerateMachineCode";
            this.btnGenerateMachineCode.Size = new System.Drawing.Size(80, 23);
            this.btnGenerateMachineCode.TabIndex = 14;
            this.btnGenerateMachineCode.Text = "生成机器码";
            this.btnGenerateMachineCode.UseVisualStyleBackColor = true;
            this.btnGenerateMachineCode.Click += new System.EventHandler(this.btnGenerateMachineCode_Click);
            // 
            // textBoxFunctionModule
            // 
            this.textBoxFunctionModule.Location = new System.Drawing.Point(120, 140);
            this.textBoxFunctionModule.Name = "textBoxFunctionModule";
            this.textBoxFunctionModule.Size = new System.Drawing.Size(400, 21);
            this.textBoxFunctionModule.TabIndex = 13;
            // 
            // labelFunctionModule
            // 
            this.labelFunctionModule.AutoSize = true;
            this.labelFunctionModule.Location = new System.Drawing.Point(20, 145);
            this.labelFunctionModule.Name = "labelFunctionModule";
            this.labelFunctionModule.Size = new System.Drawing.Size(71, 12);
            this.labelFunctionModule.TabIndex = 12;
            this.labelFunctionModule.Text = "功能模块:";
            // 
            // textBoxLicenseType
            // 
            this.textBoxLicenseType.Location = new System.Drawing.Point(370, 105);
            this.textBoxLicenseType.Name = "textBoxLicenseType";
            this.textBoxLicenseType.Size = new System.Drawing.Size(150, 21);
            this.textBoxLicenseType.TabIndex = 11;
            // 
            // labelLicenseType
            // 
            this.labelLicenseType.AutoSize = true;
            this.labelLicenseType.Location = new System.Drawing.Point(270, 110);
            this.labelLicenseType.Name = "labelLicenseType";
            this.labelLicenseType.Size = new System.Drawing.Size(71, 12);
            this.labelLicenseType.TabIndex = 10;
            this.labelLicenseType.Text = "许可证类型:";
            // 
            // textBoxProductVersion
            // 
            this.textBoxProductVersion.Location = new System.Drawing.Point(120, 105);
            this.textBoxProductVersion.Name = "textBoxProductVersion";
            this.textBoxProductVersion.Size = new System.Drawing.Size(130, 21);
            this.textBoxProductVersion.TabIndex = 9;
            // 
            // labelProductVersion
            // 
            this.labelProductVersion.AutoSize = true;
            this.labelProductVersion.Location = new System.Drawing.Point(20, 110);
            this.labelProductVersion.Name = "labelProductVersion";
            this.labelProductVersion.Size = new System.Drawing.Size(59, 12);
            this.labelProductVersion.TabIndex = 8;
            this.labelProductVersion.Text = "产品版本:";
            // 
            // textBoxConcurrentUsers
            // 
            this.textBoxConcurrentUsers.Location = new System.Drawing.Point(370, 70);
            this.textBoxConcurrentUsers.Name = "textBoxConcurrentUsers";
            this.textBoxConcurrentUsers.Size = new System.Drawing.Size(150, 21);
            this.textBoxConcurrentUsers.TabIndex = 7;
            // 
            // labelConcurrentUsers
            // 
            this.labelConcurrentUsers.AutoSize = true;
            this.labelConcurrentUsers.Location = new System.Drawing.Point(270, 75);
            this.labelConcurrentUsers.Name = "labelConcurrentUsers";
            this.labelConcurrentUsers.Size = new System.Drawing.Size(83, 12);
            this.labelConcurrentUsers.TabIndex = 6;
            this.labelConcurrentUsers.Text = "并发用户数:";
            // 
            // textBoxPhoneNumber
            // 
            this.textBoxPhoneNumber.Location = new System.Drawing.Point(120, 70);
            this.textBoxPhoneNumber.Name = "textBoxPhoneNumber";
            this.textBoxPhoneNumber.Size = new System.Drawing.Size(130, 21);
            this.textBoxPhoneNumber.TabIndex = 5;
            // 
            // labelPhoneNumber
            // 
            this.labelPhoneNumber.AutoSize = true;
            this.labelPhoneNumber.Location = new System.Drawing.Point(20, 75);
            this.labelPhoneNumber.Name = "labelPhoneNumber";
            this.labelPhoneNumber.Size = new System.Drawing.Size(59, 12);
            this.labelPhoneNumber.TabIndex = 4;
            this.labelPhoneNumber.Text = "电话号码:";
            // 
            // textBoxContactName
            // 
            this.textBoxContactName.Location = new System.Drawing.Point(370, 35);
            this.textBoxContactName.Name = "textBoxContactName";
            this.textBoxContactName.Size = new System.Drawing.Size(150, 21);
            this.textBoxContactName.TabIndex = 3;
            // 
            // labelContactName
            // 
            this.labelContactName.AutoSize = true;
            this.labelContactName.Location = new System.Drawing.Point(270, 40);
            this.labelContactName.Name = "labelContactName";
            this.labelContactName.Size = new System.Drawing.Size(47, 12);
            this.labelContactName.TabIndex = 2;
            this.labelContactName.Text = "联系人:";
            // 
            // textBoxCompanyName
            // 
            this.textBoxCompanyName.Location = new System.Drawing.Point(120, 35);
            this.textBoxCompanyName.Name = "textBoxCompanyName";
            this.textBoxCompanyName.Size = new System.Drawing.Size(130, 21);
            this.textBoxCompanyName.TabIndex = 1;
            // 
            // labelCompanyName
            // 
            this.labelCompanyName.AutoSize = true;
            this.labelCompanyName.Location = new System.Drawing.Point(20, 40);
            this.labelCompanyName.Name = "labelCompanyName";
            this.labelCompanyName.Size = new System.Drawing.Size(59, 12);
            this.labelCompanyName.TabIndex = 0;
            this.labelCompanyName.Text = "公司名称:";
            // 
            // panelRegistrationButtons
            // 
            this.panelRegistrationButtons.Controls.Add(this.btnValidateRegistration);
            this.panelRegistrationButtons.Controls.Add(this.btnSaveRegistration);
            this.panelRegistrationButtons.Controls.Add(this.btnLoadRegistration);
            this.panelRegistrationButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRegistrationButtons.Location = new System.Drawing.Point(3, 524);
            this.panelRegistrationButtons.Name = "panelRegistrationButtons";
            this.panelRegistrationButtons.Size = new System.Drawing.Size(786, 47);
            this.panelRegistrationButtons.TabIndex = 0;
            // 
            // btnValidateRegistration
            // 
            this.btnValidateRegistration.Location = new System.Drawing.Point(200, 12);
            this.btnValidateRegistration.Name = "btnValidateRegistration";
            this.btnValidateRegistration.Size = new System.Drawing.Size(85, 23);
            this.btnValidateRegistration.TabIndex = 2;
            this.btnValidateRegistration.Text = "验证注册";
            this.btnValidateRegistration.UseVisualStyleBackColor = true;
            this.btnValidateRegistration.Click += new System.EventHandler(this.btnValidateRegistration_Click);
            // 
            // btnSaveRegistration
            // 
            this.btnSaveRegistration.Location = new System.Drawing.Point(100, 12);
            this.btnSaveRegistration.Name = "btnSaveRegistration";
            this.btnSaveRegistration.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRegistration.TabIndex = 1;
            this.btnSaveRegistration.Text = "保存注册";
            this.btnSaveRegistration.UseVisualStyleBackColor = true;
            this.btnSaveRegistration.Click += new System.EventHandler(this.btnSaveRegistration_Click);
            // 
            // btnLoadRegistration
            // 
            this.btnLoadRegistration.Location = new System.Drawing.Point(15, 12);
            this.btnLoadRegistration.Name = "btnLoadRegistration";
            this.btnLoadRegistration.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRegistration.TabIndex = 0;
            this.btnLoadRegistration.Text = "重新加载";
            this.btnLoadRegistration.UseVisualStyleBackColor = true;
            this.btnLoadRegistration.Click += new System.EventHandler(this.btnLoadRegistration_Click);
            // 
            // SystemManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlSystemManagement);
            this.Name = "SystemManagementControl";
            this.Size = new System.Drawing.Size(800, 600);
            this.Load += new System.EventHandler(this.SystemManagementControl_Load);
            this.tabControlSystemManagement.ResumeLayout(false);
            this.tabPageSystemConfig.ResumeLayout(false);
            this.groupBoxLoggingConfig.ResumeLayout(false);
            this.groupBoxLoggingConfig.PerformLayout();
            this.groupBoxCacheConfig.ResumeLayout(false);
            this.groupBoxCacheConfig.PerformLayout();
            this.groupBoxDatabaseConfig.ResumeLayout(false);
            this.groupBoxDatabaseConfig.PerformLayout();
            this.groupBoxBasicConfig.ResumeLayout(false);
            this.groupBoxBasicConfig.PerformLayout();
            this.panelConfigButtons.ResumeLayout(false);
            this.tabPageRegistration.ResumeLayout(false);
            this.groupBoxRegistrationInfo.ResumeLayout(false);
            this.groupBoxRegistrationInfo.PerformLayout();
            this.panelRegistrationButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlSystemManagement;
        private System.Windows.Forms.TabPage tabPageSystemConfig;
        private System.Windows.Forms.TabPage tabPageRegistration;
        private System.Windows.Forms.Panel panelConfigButtons;
        private System.Windows.Forms.Button btnResetConfig;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Button btnLoadConfig;
        private System.Windows.Forms.GroupBox groupBoxBasicConfig;
        private System.Windows.Forms.TextBox textBoxServerName;
        private System.Windows.Forms.Label labelServerName;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox textBoxMaxConnections;
        private System.Windows.Forms.Label labelMaxConnections;
        private System.Windows.Forms.TextBox textBoxHeartbeatInterval;
        private System.Windows.Forms.Label labelHeartbeatInterval;
        private System.Windows.Forms.TextBox textBoxSomeSetting;
        private System.Windows.Forms.Label labelSomeSetting;
        private System.Windows.Forms.GroupBox groupBoxDatabaseConfig;
        private System.Windows.Forms.TextBox textBoxDbType;
        private System.Windows.Forms.Label labelDbType;
        private System.Windows.Forms.TextBox textBoxDbConnectionString;
        private System.Windows.Forms.Label labelDbConnectionString;
        private System.Windows.Forms.GroupBox groupBoxCacheConfig;
        private System.Windows.Forms.TextBox textBoxCacheType;
        private System.Windows.Forms.Label labelCacheType;
        private System.Windows.Forms.TextBox textBoxCacheConnectionString;
        private System.Windows.Forms.Label labelCacheConnectionString;
        private System.Windows.Forms.GroupBox groupBoxLoggingConfig;
        private System.Windows.Forms.CheckBox checkBoxEnableLogging;
        private System.Windows.Forms.TextBox textBoxLogLevel;
        private System.Windows.Forms.Label labelLogLevel;
        private System.Windows.Forms.Panel panelRegistrationButtons;
        private System.Windows.Forms.Button btnValidateRegistration;
        private System.Windows.Forms.Button btnSaveRegistration;
        private System.Windows.Forms.Button btnLoadRegistration;
        private System.Windows.Forms.GroupBox groupBoxRegistrationInfo;
        private System.Windows.Forms.TextBox textBoxCompanyName;
        private System.Windows.Forms.Label labelCompanyName;
        private System.Windows.Forms.TextBox textBoxContactName;
        private System.Windows.Forms.Label labelContactName;
        private System.Windows.Forms.TextBox textBoxPhoneNumber;
        private System.Windows.Forms.Label labelPhoneNumber;
        private System.Windows.Forms.TextBox textBoxConcurrentUsers;
        private System.Windows.Forms.Label labelConcurrentUsers;
        private System.Windows.Forms.TextBox textBoxProductVersion;
        private System.Windows.Forms.Label labelProductVersion;
        private System.Windows.Forms.TextBox textBoxLicenseType;
        private System.Windows.Forms.Label labelLicenseType;
        private System.Windows.Forms.TextBox textBoxFunctionModule;
        private System.Windows.Forms.Label labelFunctionModule;
        private System.Windows.Forms.Button btnGenerateMachineCode;
        private System.Windows.Forms.TextBox textBoxMachineCode;
        private System.Windows.Forms.Label labelMachineCode;
        private System.Windows.Forms.TextBox textBoxRegistrationCode;
        private System.Windows.Forms.Label labelRegistrationCode;
        private System.Windows.Forms.Label labelExpirationDate;
        private System.Windows.Forms.Label labelRegistrationDate;
        private System.Windows.Forms.Label labelPurchaseDate;
        private System.Windows.Forms.CheckBox checkBoxIsRegistered;
        private System.Windows.Forms.DateTimePicker dateTimePickerExpirationDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerRegistrationDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerPurchaseDate;
    }
}