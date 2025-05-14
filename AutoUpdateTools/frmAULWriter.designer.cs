namespace AULWriter
{
    partial class frmAULWriter
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
                _syncOldToNew?.Dispose();
                _syncNewToOld?.Dispose();
                components?.Dispose();
                //components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAULWriter));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDiff = new System.Windows.Forms.Button();
            this.btnLoadPreCurrentVer = new System.Windows.Forms.Button();
            this.chk文件比较 = new System.Windows.Forms.CheckBox();
            this.button_save_config = new System.Windows.Forms.Button();
            this.btnrelease = new System.Windows.Forms.Button();
            this.btnGenerateNewlist = new System.Windows.Forms.Button();
            this.richTxtLog = new System.Windows.Forms.RichTextBox();
            this.prbProd = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpBase = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAutoUpdateXmlSavePath = new System.Windows.Forms.TextBox();
            this.btnBaseDir = new System.Windows.Forms.Button();
            this.txtTargetDirectory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCompareSource = new System.Windows.Forms.Button();
            this.txtCompareSource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExpt = new System.Windows.Forms.Button();
            this.txtExpt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDest = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSrc = new System.Windows.Forms.Button();
            this.txtMainExePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbpOpt = new System.Windows.Forms.TabPage();
            this.splitContainerOption = new System.Windows.Forms.SplitContainer();
            this.txtPreVerUpdatedFiles = new System.Windows.Forms.TextBox();
            this.chkUseBaseVersion = new System.Windows.Forms.CheckBox();
            this.txtBaseExeVersion = new System.Windows.Forms.TextBox();
            this.chk目录转为明细 = new System.Windows.Forms.CheckBox();
            this.chkAppend = new System.Windows.Forms.CheckBox();
            this.tbpDiffList = new System.Windows.Forms.TabPage();
            this.txtDiff = new System.Windows.Forms.RichTextBox();
            this.tbpLastXml = new System.Windows.Forms.TabPage();
            this.txtLastXml = new System.Windows.Forms.RichTextBox();
            this.tabPageDiff = new System.Windows.Forms.TabPage();
            this.splitContainerDiff = new System.Windows.Forms.SplitContainer();
            this.rtbOld = new System.Windows.Forms.RichTextBox();
            this.rtbNew = new System.Windows.Forms.RichTextBox();
            this.tabUsedescribe = new System.Windows.Forms.TabPage();
            this.txt使用说明 = new System.Windows.Forms.TextBox();
            this.ofdSrc = new System.Windows.Forms.OpenFileDialog();
            this.sfdDest = new System.Windows.Forms.SaveFileDialog();
            this.ofdExpt = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogCompareSource = new System.Windows.Forms.FolderBrowserDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpBase.SuspendLayout();
            this.tbpOpt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOption)).BeginInit();
            this.splitContainerOption.Panel1.SuspendLayout();
            this.splitContainerOption.Panel2.SuspendLayout();
            this.splitContainerOption.SuspendLayout();
            this.tbpDiffList.SuspendLayout();
            this.tbpLastXml.SuspendLayout();
            this.tabPageDiff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiff)).BeginInit();
            this.splitContainerDiff.Panel1.SuspendLayout();
            this.splitContainerDiff.Panel2.SuspendLayout();
            this.splitContainerDiff.SuspendLayout();
            this.tabUsedescribe.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1061, 53);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(71, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "AutoUpdaterList Writer";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDiff);
            this.panel2.Controls.Add(this.btnLoadPreCurrentVer);
            this.panel2.Controls.Add(this.chk文件比较);
            this.panel2.Controls.Add(this.button_save_config);
            this.panel2.Controls.Add(this.btnrelease);
            this.panel2.Controls.Add(this.btnGenerateNewlist);
            this.panel2.Controls.Add(this.richTxtLog);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 519);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1061, 237);
            this.panel2.TabIndex = 1;
            // 
            // btnDiff
            // 
            this.btnDiff.Location = new System.Drawing.Point(978, 12);
            this.btnDiff.Name = "btnDiff";
            this.btnDiff.Size = new System.Drawing.Size(51, 23);
            this.btnDiff.TabIndex = 8;
            this.btnDiff.Text = "比较";
            this.btnDiff.UseVisualStyleBackColor = true;
            this.btnDiff.Click += new System.EventHandler(this.btnDiff_Click);
            // 
            // btnLoadPreCurrentVer
            // 
            this.btnLoadPreCurrentVer.Location = new System.Drawing.Point(12, 9);
            this.btnLoadPreCurrentVer.Name = "btnLoadPreCurrentVer";
            this.btnLoadPreCurrentVer.Size = new System.Drawing.Size(74, 23);
            this.btnLoadPreCurrentVer.TabIndex = 9;
            this.btnLoadPreCurrentVer.Text = "加载旧清单";
            this.btnLoadPreCurrentVer.UseVisualStyleBackColor = true;
            this.btnLoadPreCurrentVer.Click += new System.EventHandler(this.btnLoadCurrentVer_Click);
            // 
            // chk文件比较
            // 
            this.chk文件比较.AutoSize = true;
            this.chk文件比较.Checked = true;
            this.chk文件比较.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk文件比较.Location = new System.Drawing.Point(182, 12);
            this.chk文件比较.Name = "chk文件比较";
            this.chk文件比较.Size = new System.Drawing.Size(72, 16);
            this.chk文件比较.TabIndex = 4;
            this.chk文件比较.Text = "文件比较";
            this.chk文件比较.UseVisualStyleBackColor = true;
            this.chk文件比较.CheckedChanged += new System.EventHandler(this.chk哈希值比较_CheckedChanged);
            // 
            // button_save_config
            // 
            this.button_save_config.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_save_config.Location = new System.Drawing.Point(815, 11);
            this.button_save_config.Name = "button_save_config";
            this.button_save_config.Size = new System.Drawing.Size(101, 23);
            this.button_save_config.TabIndex = 3;
            this.button_save_config.Text = "保存配置(&B)";
            this.button_save_config.UseVisualStyleBackColor = true;
            this.button_save_config.Click += new System.EventHandler(this.button_save_config_Click);
            // 
            // btnrelease
            // 
            this.btnrelease.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnrelease.Location = new System.Drawing.Point(556, 10);
            this.btnrelease.Name = "btnrelease";
            this.btnrelease.Size = new System.Drawing.Size(113, 23);
            this.btnrelease.TabIndex = 1;
            this.btnrelease.Text = "2)发布到保存位置";
            this.btnrelease.UseVisualStyleBackColor = true;
            this.btnrelease.Click += new System.EventHandler(this.btnrelease_Click);
            // 
            // btnGenerateNewlist
            // 
            this.btnGenerateNewlist.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGenerateNewlist.Location = new System.Drawing.Point(326, 8);
            this.btnGenerateNewlist.Name = "btnGenerateNewlist";
            this.btnGenerateNewlist.Size = new System.Drawing.Size(111, 23);
            this.btnGenerateNewlist.TabIndex = 0;
            this.btnGenerateNewlist.Text = "1)生成最新清单(&G)";
            this.btnGenerateNewlist.UseVisualStyleBackColor = true;
            this.btnGenerateNewlist.Click += new System.EventHandler(this.btnProduce_Click);
            // 
            // richTxtLog
            // 
            this.richTxtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTxtLog.Location = new System.Drawing.Point(0, 49);
            this.richTxtLog.Name = "richTxtLog";
            this.richTxtLog.Size = new System.Drawing.Size(1061, 188);
            this.richTxtLog.TabIndex = 11;
            this.richTxtLog.Text = "";
            // 
            // prbProd
            // 
            this.prbProd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.prbProd.Location = new System.Drawing.Point(0, 496);
            this.prbProd.Name = "prbProd";
            this.prbProd.Size = new System.Drawing.Size(1061, 23);
            this.prbProd.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1061, 466);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpBase);
            this.tabControl1.Controls.Add(this.tbpOpt);
            this.tabControl1.Controls.Add(this.tbpDiffList);
            this.tabControl1.Controls.Add(this.tbpLastXml);
            this.tabControl1.Controls.Add(this.tabPageDiff);
            this.tabControl1.Controls.Add(this.tabUsedescribe);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1055, 446);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpBase
            // 
            this.tbpBase.Controls.Add(this.label8);
            this.tbpBase.Controls.Add(this.txtAutoUpdateXmlSavePath);
            this.tbpBase.Controls.Add(this.btnBaseDir);
            this.tbpBase.Controls.Add(this.txtTargetDirectory);
            this.tbpBase.Controls.Add(this.label7);
            this.tbpBase.Controls.Add(this.btnCompareSource);
            this.tbpBase.Controls.Add(this.txtCompareSource);
            this.tbpBase.Controls.Add(this.label6);
            this.tbpBase.Controls.Add(this.txtUrl);
            this.tbpBase.Controls.Add(this.label5);
            this.tbpBase.Controls.Add(this.btnExpt);
            this.tbpBase.Controls.Add(this.txtExpt);
            this.tbpBase.Controls.Add(this.label4);
            this.tbpBase.Controls.Add(this.btnDest);
            this.tbpBase.Controls.Add(this.label3);
            this.tbpBase.Controls.Add(this.btnSrc);
            this.tbpBase.Controls.Add(this.txtMainExePath);
            this.tbpBase.Controls.Add(this.label2);
            this.tbpBase.Location = new System.Drawing.Point(4, 22);
            this.tbpBase.Name = "tbpBase";
            this.tbpBase.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBase.Size = new System.Drawing.Size(1047, 420);
            this.tbpBase.TabIndex = 0;
            this.tbpBase.Text = "※基本信息";
            this.tbpBase.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(666, 366);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(263, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "生成后也会使用配置文件中 指定的更新网址端口";
            // 
            // txtAutoUpdateXmlSavePath
            // 
            this.txtAutoUpdateXmlSavePath.Location = new System.Drawing.Point(105, 360);
            this.txtAutoUpdateXmlSavePath.Name = "txtAutoUpdateXmlSavePath";
            this.txtAutoUpdateXmlSavePath.Size = new System.Drawing.Size(468, 21);
            this.txtAutoUpdateXmlSavePath.TabIndex = 4;
            // 
            // btnBaseDir
            // 
            this.btnBaseDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseDir.Location = new System.Drawing.Point(719, 41);
            this.btnBaseDir.Name = "btnBaseDir";
            this.btnBaseDir.Size = new System.Drawing.Size(58, 21);
            this.btnBaseDir.TabIndex = 17;
            this.btnBaseDir.Text = "选择(&S)";
            this.btnBaseDir.UseVisualStyleBackColor = true;
            this.btnBaseDir.Click += new System.EventHandler(this.btnBaseDir_Click);
            // 
            // txtTargetDirectory
            // 
            this.txtTargetDirectory.AllowDrop = true;
            this.txtTargetDirectory.Location = new System.Drawing.Point(106, 40);
            this.txtTargetDirectory.Name = "txtTargetDirectory";
            this.txtTargetDirectory.Size = new System.Drawing.Size(607, 21);
            this.txtTargetDirectory.TabIndex = 16;
            this.txtTargetDirectory.Text = "暂时没有完成，用哈希值比较，以上面主程序为基准用这个debug输出比较。有新的就加入";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "目标目录:";
            // 
            // btnCompareSource
            // 
            this.btnCompareSource.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCompareSource.Location = new System.Drawing.Point(719, 73);
            this.btnCompareSource.Name = "btnCompareSource";
            this.btnCompareSource.Size = new System.Drawing.Size(58, 21);
            this.btnCompareSource.TabIndex = 14;
            this.btnCompareSource.Text = "选择(&S)";
            this.btnCompareSource.UseVisualStyleBackColor = true;
            this.btnCompareSource.Click += new System.EventHandler(this.btnCompareSource_Click);
            // 
            // txtCompareSource
            // 
            this.txtCompareSource.AllowDrop = true;
            this.txtCompareSource.Location = new System.Drawing.Point(106, 72);
            this.txtCompareSource.Name = "txtCompareSource";
            this.txtCompareSource.Size = new System.Drawing.Size(607, 21);
            this.txtCompareSource.TabIndex = 13;
            this.txtCompareSource.Text = "暂时没有完成，用哈希值比较，以上面主程序为基准用这个debug输出比较。有新的就加入";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "来源目录:";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(106, 96);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(607, 21);
            this.txtUrl.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "更新网址:";
            // 
            // btnExpt
            // 
            this.btnExpt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExpt.Location = new System.Drawing.Point(917, 122);
            this.btnExpt.Name = "btnExpt";
            this.btnExpt.Size = new System.Drawing.Size(58, 21);
            this.btnExpt.TabIndex = 8;
            this.btnExpt.Text = "选择(&S)";
            this.btnExpt.UseVisualStyleBackColor = true;
            this.btnExpt.Click += new System.EventHandler(this.btnSearExpt_Click);
            // 
            // txtExpt
            // 
            this.txtExpt.AllowDrop = true;
            this.txtExpt.Location = new System.Drawing.Point(105, 122);
            this.txtExpt.Multiline = true;
            this.txtExpt.Name = "txtExpt";
            this.txtExpt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtExpt.Size = new System.Drawing.Size(806, 233);
            this.txtExpt.TabIndex = 7;
            this.txtExpt.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtExpt_DragDrop);
            this.txtExpt.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtExpt_DragEnter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "排除文件:";
            // 
            // btnDest
            // 
            this.btnDest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDest.Location = new System.Drawing.Point(587, 361);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(58, 21);
            this.btnDest.TabIndex = 5;
            this.btnDest.Text = "选择(&S)";
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnSearDes_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 363);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "目标清单保存位置:";
            // 
            // btnSrc
            // 
            this.btnSrc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrc.Location = new System.Drawing.Point(719, 12);
            this.btnSrc.Name = "btnSrc";
            this.btnSrc.Size = new System.Drawing.Size(58, 21);
            this.btnSrc.TabIndex = 2;
            this.btnSrc.Text = "选择(&S)";
            this.btnSrc.UseVisualStyleBackColor = true;
            this.btnSrc.Click += new System.EventHandler(this.btnSrc_Click);
            // 
            // txtMainExePath
            // 
            this.txtMainExePath.AllowDrop = true;
            this.txtMainExePath.Location = new System.Drawing.Point(106, 13);
            this.txtMainExePath.Name = "txtMainExePath";
            this.txtMainExePath.Size = new System.Drawing.Size(607, 21);
            this.txtMainExePath.TabIndex = 1;
            this.txtMainExePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSrc_DragDrop);
            this.txtMainExePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSrc_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "目标主程序:";
            // 
            // tbpOpt
            // 
            this.tbpOpt.Controls.Add(this.splitContainerOption);
            this.tbpOpt.Location = new System.Drawing.Point(4, 22);
            this.tbpOpt.Name = "tbpOpt";
            this.tbpOpt.Padding = new System.Windows.Forms.Padding(3);
            this.tbpOpt.Size = new System.Drawing.Size(1047, 420);
            this.tbpOpt.TabIndex = 1;
            this.tbpOpt.Text = "※将要生成的清单";
            this.tbpOpt.UseVisualStyleBackColor = true;
            // 
            // splitContainerOption
            // 
            this.splitContainerOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerOption.Location = new System.Drawing.Point(3, 3);
            this.splitContainerOption.Name = "splitContainerOption";
            // 
            // splitContainerOption.Panel1
            // 
            this.splitContainerOption.Panel1.Controls.Add(this.txtPreVerUpdatedFiles);
            // 
            // splitContainerOption.Panel2
            // 
            this.splitContainerOption.Panel2.Controls.Add(this.chkUseBaseVersion);
            this.splitContainerOption.Panel2.Controls.Add(this.txtBaseExeVersion);
            this.splitContainerOption.Panel2.Controls.Add(this.chk目录转为明细);
            this.splitContainerOption.Panel2.Controls.Add(this.chkAppend);
            this.splitContainerOption.Size = new System.Drawing.Size(1041, 414);
            this.splitContainerOption.SplitterDistance = 715;
            this.splitContainerOption.TabIndex = 0;
            // 
            // txtPreVerUpdatedFiles
            // 
            this.txtPreVerUpdatedFiles.AllowDrop = true;
            this.txtPreVerUpdatedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPreVerUpdatedFiles.Location = new System.Drawing.Point(0, 0);
            this.txtPreVerUpdatedFiles.Multiline = true;
            this.txtPreVerUpdatedFiles.Name = "txtPreVerUpdatedFiles";
            this.txtPreVerUpdatedFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPreVerUpdatedFiles.Size = new System.Drawing.Size(715, 414);
            this.txtPreVerUpdatedFiles.TabIndex = 0;
            this.txtPreVerUpdatedFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtUpdatedFiles_DragDrop);
            this.txtPreVerUpdatedFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtUpdatedFiles_DragEnter);
            // 
            // chkUseBaseVersion
            // 
            this.chkUseBaseVersion.AutoSize = true;
            this.chkUseBaseVersion.Location = new System.Drawing.Point(19, 145);
            this.chkUseBaseVersion.Name = "chkUseBaseVersion";
            this.chkUseBaseVersion.Size = new System.Drawing.Size(222, 16);
            this.chkUseBaseVersion.TabIndex = 6;
            this.chkUseBaseVersion.Text = "指定exe版本【不从文件属性中获取】";
            this.chkUseBaseVersion.UseVisualStyleBackColor = true;
            // 
            // txtBaseExeVersion
            // 
            this.txtBaseExeVersion.Location = new System.Drawing.Point(19, 179);
            this.txtBaseExeVersion.Name = "txtBaseExeVersion";
            this.txtBaseExeVersion.Size = new System.Drawing.Size(119, 21);
            this.txtBaseExeVersion.TabIndex = 4;
            this.txtBaseExeVersion.Text = "1.0.0.1";
            // 
            // chk目录转为明细
            // 
            this.chk目录转为明细.AutoSize = true;
            this.chk目录转为明细.Checked = true;
            this.chk目录转为明细.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk目录转为明细.Location = new System.Drawing.Point(9, 63);
            this.chk目录转为明细.Name = "chk目录转为明细";
            this.chk目录转为明细.Size = new System.Drawing.Size(96, 16);
            this.chk目录转为明细.TabIndex = 3;
            this.chk目录转为明细.Text = "目录转为明细";
            this.chk目录转为明细.UseVisualStyleBackColor = true;
            // 
            // chkAppend
            // 
            this.chkAppend.AutoSize = true;
            this.chkAppend.Checked = true;
            this.chkAppend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAppend.Location = new System.Drawing.Point(9, 15);
            this.chkAppend.Name = "chkAppend";
            this.chkAppend.Size = new System.Drawing.Size(72, 16);
            this.chkAppend.TabIndex = 2;
            this.chkAppend.Text = "追加模式";
            this.chkAppend.UseVisualStyleBackColor = true;
            // 
            // tbpDiffList
            // 
            this.tbpDiffList.Controls.Add(this.txtDiff);
            this.tbpDiffList.Location = new System.Drawing.Point(4, 22);
            this.tbpDiffList.Name = "tbpDiffList";
            this.tbpDiffList.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDiffList.Size = new System.Drawing.Size(1047, 420);
            this.tbpDiffList.TabIndex = 2;
            this.tbpDiffList.Text = "※差异文件列表";
            this.tbpDiffList.UseVisualStyleBackColor = true;
            // 
            // txtDiff
            // 
            this.txtDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiff.Location = new System.Drawing.Point(3, 3);
            this.txtDiff.Name = "txtDiff";
            this.txtDiff.Size = new System.Drawing.Size(1041, 414);
            this.txtDiff.TabIndex = 1;
            this.txtDiff.Text = "";
            // 
            // tbpLastXml
            // 
            this.tbpLastXml.Controls.Add(this.txtLastXml);
            this.tbpLastXml.Location = new System.Drawing.Point(4, 22);
            this.tbpLastXml.Name = "tbpLastXml";
            this.tbpLastXml.Padding = new System.Windows.Forms.Padding(3);
            this.tbpLastXml.Size = new System.Drawing.Size(1047, 420);
            this.tbpLastXml.TabIndex = 3;
            this.tbpLastXml.Text = "※结果";
            this.tbpLastXml.UseVisualStyleBackColor = true;
            // 
            // txtLastXml
            // 
            this.txtLastXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLastXml.Location = new System.Drawing.Point(3, 3);
            this.txtLastXml.Name = "txtLastXml";
            this.txtLastXml.ReadOnly = true;
            this.txtLastXml.Size = new System.Drawing.Size(1041, 414);
            this.txtLastXml.TabIndex = 0;
            this.txtLastXml.Text = "";
            // 
            // tabPageDiff
            // 
            this.tabPageDiff.Controls.Add(this.splitContainerDiff);
            this.tabPageDiff.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiff.Name = "tabPageDiff";
            this.tabPageDiff.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiff.Size = new System.Drawing.Size(1047, 420);
            this.tabPageDiff.TabIndex = 4;
            this.tabPageDiff.Text = "※新【右】-旧【左】版本差异比较";
            this.tabPageDiff.UseVisualStyleBackColor = true;
            // 
            // splitContainerDiff
            // 
            this.splitContainerDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerDiff.Location = new System.Drawing.Point(3, 3);
            this.splitContainerDiff.Name = "splitContainerDiff";
            // 
            // splitContainerDiff.Panel1
            // 
            this.splitContainerDiff.Panel1.Controls.Add(this.rtbOld);
            // 
            // splitContainerDiff.Panel2
            // 
            this.splitContainerDiff.Panel2.Controls.Add(this.rtbNew);
            this.splitContainerDiff.Size = new System.Drawing.Size(1041, 414);
            this.splitContainerDiff.SplitterDistance = 519;
            this.splitContainerDiff.TabIndex = 0;
            // 
            // rtbOld
            // 
            this.rtbOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOld.Location = new System.Drawing.Point(0, 0);
            this.rtbOld.Name = "rtbOld";
            this.rtbOld.Size = new System.Drawing.Size(519, 414);
            this.rtbOld.TabIndex = 0;
            this.rtbOld.Text = "";
            // 
            // rtbNew
            // 
            this.rtbNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbNew.Location = new System.Drawing.Point(0, 0);
            this.rtbNew.Name = "rtbNew";
            this.rtbNew.Size = new System.Drawing.Size(518, 414);
            this.rtbNew.TabIndex = 0;
            this.rtbNew.Text = "";
            // 
            // tabUsedescribe
            // 
            this.tabUsedescribe.Controls.Add(this.txt使用说明);
            this.tabUsedescribe.Location = new System.Drawing.Point(4, 22);
            this.tabUsedescribe.Name = "tabUsedescribe";
            this.tabUsedescribe.Size = new System.Drawing.Size(1047, 420);
            this.tabUsedescribe.TabIndex = 5;
            this.tabUsedescribe.Text = "使用说明";
            this.tabUsedescribe.UseVisualStyleBackColor = true;
            // 
            // txt使用说明
            // 
            this.txt使用说明.AllowDrop = true;
            this.txt使用说明.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt使用说明.Location = new System.Drawing.Point(0, 0);
            this.txt使用说明.Multiline = true;
            this.txt使用说明.Name = "txt使用说明";
            this.txt使用说明.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt使用说明.Size = new System.Drawing.Size(1047, 420);
            this.txt使用说明.TabIndex = 1;
            this.txt使用说明.Text = "1）如果没有旧版（旧版清单为空时），则以目标目录中的所有文件为标准全部更新。（执行排除清单）\r\n\r\n2）旧版清单中添加，实际目标中没有的。则版本号按前面规则（指定" +
    "初始版本或或文件版本本身为标准）添加\r\n\r\n3）文件比较勾选：按文件修改时间和哈希比较后产生变化的才加入到更新清单中。\r\n\r\n4）新旧清单中对应项目增加1，如果" +
    "没有对应的则按指定版本号或文件版本本身设置。\r\n\r\n";
            // 
            // ofdSrc
            // 
            this.ofdSrc.DefaultExt = "*.exe";
            this.ofdSrc.Filter = "程序文件(*.exe)|*.exe|所有文件(*.*)|*.*";
            this.ofdSrc.Title = "请选择主程序文件";
            this.ofdSrc.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdDest_FileOk);
            // 
            // sfdDest
            // 
            this.sfdDest.CheckPathExists = false;
            this.sfdDest.DefaultExt = "*.xml";
            this.sfdDest.FileName = "AutoUpdaterList.xml";
            this.sfdDest.Filter = "XML文件(*.xml)|*.xml";
            this.sfdDest.Title = "请选择AutoUpdaterList保存位置";
            this.sfdDest.FileOk += new System.ComponentModel.CancelEventHandler(this.sfdSrcPath_FileOk);
            // 
            // ofdExpt
            // 
            this.ofdExpt.DefaultExt = "*.*";
            this.ofdExpt.Filter = "所有文件(*.*)|*.*";
            this.ofdExpt.Multiselect = true;
            this.ofdExpt.Title = "请选择主程序文件";
            this.ofdExpt.FileOk += new System.ComponentModel.CancelEventHandler(this.ofdExpt_FileOk);
            // 
            // frmAULWriter
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 756);
            this.Controls.Add(this.prbProd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAULWriter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AULWriter for AutoUpdater【版本号增加才更新】";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAULWriter_FormClosing);
            this.Load += new System.EventHandler(this.frmAULWriter_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbpBase.ResumeLayout(false);
            this.tbpBase.PerformLayout();
            this.tbpOpt.ResumeLayout(false);
            this.splitContainerOption.Panel1.ResumeLayout(false);
            this.splitContainerOption.Panel1.PerformLayout();
            this.splitContainerOption.Panel2.ResumeLayout(false);
            this.splitContainerOption.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerOption)).EndInit();
            this.splitContainerOption.ResumeLayout(false);
            this.tbpDiffList.ResumeLayout(false);
            this.tbpLastXml.ResumeLayout(false);
            this.tabPageDiff.ResumeLayout(false);
            this.splitContainerDiff.Panel1.ResumeLayout(false);
            this.splitContainerDiff.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerDiff)).EndInit();
            this.splitContainerDiff.ResumeLayout(false);
            this.tabUsedescribe.ResumeLayout(false);
            this.tabUsedescribe.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbpBase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGenerateNewlist;
        private System.Windows.Forms.Button btnrelease;
        private System.Windows.Forms.TextBox txtMainExePath;
        private System.Windows.Forms.Button btnSrc;
        private System.Windows.Forms.Button btnDest;
        private System.Windows.Forms.TextBox txtAutoUpdateXmlSavePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog ofdSrc;
        private System.Windows.Forms.SaveFileDialog sfdDest;
        private System.Windows.Forms.TextBox txtExpt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnExpt;
        private System.Windows.Forms.OpenFileDialog ofdExpt;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tbpOpt;
        private System.Windows.Forms.ProgressBar prbProd;
        private System.Windows.Forms.Button button_save_config;
        private System.Windows.Forms.RichTextBox richTxtLog;
        private System.Windows.Forms.SplitContainer splitContainerOption;
        private System.Windows.Forms.TextBox txtPreVerUpdatedFiles;
        private System.Windows.Forms.CheckBox chkAppend;
        private System.Windows.Forms.CheckBox chk目录转为明细;
        private System.Windows.Forms.CheckBox chk文件比较;
        private System.Windows.Forms.Button btnCompareSource;
        private System.Windows.Forms.TextBox txtCompareSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tbpDiffList;
        private System.Windows.Forms.TabPage tbpLastXml;
        private System.Windows.Forms.RichTextBox txtLastXml;
        private System.Windows.Forms.RichTextBox txtDiff;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogCompareSource;
        private System.Windows.Forms.TextBox txtTargetDirectory;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBaseDir;
        private System.Windows.Forms.TextBox txtBaseExeVersion;
        private System.Windows.Forms.CheckBox chkUseBaseVersion;
        private System.Windows.Forms.TabPage tabPageDiff;
        private System.Windows.Forms.SplitContainer splitContainerDiff;
        private System.Windows.Forms.RichTextBox rtbOld;
        private System.Windows.Forms.RichTextBox rtbNew;
        private System.Windows.Forms.Button btnDiff;
        private System.Windows.Forms.Button btnLoadPreCurrentVer;
        private System.Windows.Forms.TabPage tabUsedescribe;
        private System.Windows.Forms.TextBox txt使用说明;
        private System.Windows.Forms.Label label8;
    }
}

