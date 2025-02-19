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
                components.Dispose();
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
            this.btnPreview = new System.Windows.Forms.Button();
            this.chk文件比较 = new System.Windows.Forms.CheckBox();
            this.button_save_config = new System.Windows.Forms.Button();
            this.prbProd = new System.Windows.Forms.ProgressBar();
            this.btnrelease = new System.Windows.Forms.Button();
            this.btnProduce = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbpBase = new System.Windows.Forms.TabPage();
            this.btnBaseDir = new System.Windows.Forms.Button();
            this.txtBaseDir = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCompareSource = new System.Windows.Forms.Button();
            this.txtCompareSource = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.richTxtLog = new System.Windows.Forms.RichTextBox();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExpt = new System.Windows.Forms.Button();
            this.txtExpt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDest = new System.Windows.Forms.Button();
            this.txtAutoUpdateXmlSavePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSrc = new System.Windows.Forms.Button();
            this.txtMainExePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbpOpt = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtUpdatedFiles = new System.Windows.Forms.TextBox();
            this.txtBaseVerNo = new System.Windows.Forms.TextBox();
            this.chk目录转为明细 = new System.Windows.Forms.CheckBox();
            this.chkAppend = new System.Windows.Forms.CheckBox();
            this.txtVerNo = new System.Windows.Forms.TextBox();
            this.chkCustomVer = new System.Windows.Forms.CheckBox();
            this.tbpDiffList = new System.Windows.Forms.TabPage();
            this.txtDiff = new System.Windows.Forms.RichTextBox();
            this.tbpLastXml = new System.Windows.Forms.TabPage();
            this.txtLastXml = new System.Windows.Forms.RichTextBox();
            this.ofdSrc = new System.Windows.Forms.OpenFileDialog();
            this.sfdDest = new System.Windows.Forms.SaveFileDialog();
            this.ofdExpt = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialogCompareSource = new System.Windows.Forms.FolderBrowserDialog();
            this.chkUseBaseVersion = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbpBase.SuspendLayout();
            this.tbpOpt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tbpDiffList.SuspendLayout();
            this.tbpLastXml.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 53);
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
            this.panel2.Controls.Add(this.btnPreview);
            this.panel2.Controls.Add(this.chk文件比较);
            this.panel2.Controls.Add(this.button_save_config);
            this.panel2.Controls.Add(this.prbProd);
            this.panel2.Controls.Add(this.btnrelease);
            this.panel2.Controls.Add(this.btnProduce);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 651);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 40);
            this.panel2.TabIndex = 1;
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPreview.Location = new System.Drawing.Point(315, 10);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 7;
            this.btnPreview.Text = "1)预览";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // chk文件比较
            // 
            this.chk文件比较.AutoSize = true;
            this.chk文件比较.Checked = true;
            this.chk文件比较.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk文件比较.Location = new System.Drawing.Point(408, 14);
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
            this.button_save_config.Location = new System.Drawing.Point(879, 11);
            this.button_save_config.Name = "button_save_config";
            this.button_save_config.Size = new System.Drawing.Size(101, 23);
            this.button_save_config.TabIndex = 3;
            this.button_save_config.Text = "保存配置(&B)";
            this.button_save_config.UseVisualStyleBackColor = true;
            this.button_save_config.Click += new System.EventHandler(this.button_save_config_Click);
            // 
            // prbProd
            // 
            this.prbProd.Location = new System.Drawing.Point(7, 5);
            this.prbProd.Name = "prbProd";
            this.prbProd.Size = new System.Drawing.Size(266, 23);
            this.prbProd.TabIndex = 2;
            this.prbProd.Visible = false;
            // 
            // btnrelease
            // 
            this.btnrelease.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnrelease.Location = new System.Drawing.Point(672, 10);
            this.btnrelease.Name = "btnrelease";
            this.btnrelease.Size = new System.Drawing.Size(113, 23);
            this.btnrelease.TabIndex = 1;
            this.btnrelease.Text = "3)发布到保存位置";
            this.btnrelease.UseVisualStyleBackColor = true;
            this.btnrelease.Click += new System.EventHandler(this.btnrelease_Click);
            // 
            // btnProduce
            // 
            this.btnProduce.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProduce.Location = new System.Drawing.Point(509, 11);
            this.btnProduce.Name = "btnProduce";
            this.btnProduce.Size = new System.Drawing.Size(75, 23);
            this.btnProduce.TabIndex = 0;
            this.btnProduce.Text = "2)生成(&G)";
            this.btnProduce.UseVisualStyleBackColor = true;
            this.btnProduce.Click += new System.EventHandler(this.btnProduce_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 598);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbpBase);
            this.tabControl1.Controls.Add(this.tbpOpt);
            this.tabControl1.Controls.Add(this.tbpDiffList);
            this.tabControl1.Controls.Add(this.tbpLastXml);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1002, 578);
            this.tabControl1.TabIndex = 0;
            // 
            // tbpBase
            // 
            this.tbpBase.Controls.Add(this.btnBaseDir);
            this.tbpBase.Controls.Add(this.txtBaseDir);
            this.tbpBase.Controls.Add(this.label7);
            this.tbpBase.Controls.Add(this.btnCompareSource);
            this.tbpBase.Controls.Add(this.txtCompareSource);
            this.tbpBase.Controls.Add(this.label6);
            this.tbpBase.Controls.Add(this.richTxtLog);
            this.tbpBase.Controls.Add(this.txtUrl);
            this.tbpBase.Controls.Add(this.label5);
            this.tbpBase.Controls.Add(this.btnExpt);
            this.tbpBase.Controls.Add(this.txtExpt);
            this.tbpBase.Controls.Add(this.label4);
            this.tbpBase.Controls.Add(this.btnDest);
            this.tbpBase.Controls.Add(this.txtAutoUpdateXmlSavePath);
            this.tbpBase.Controls.Add(this.label3);
            this.tbpBase.Controls.Add(this.btnSrc);
            this.tbpBase.Controls.Add(this.txtMainExePath);
            this.tbpBase.Controls.Add(this.label2);
            this.tbpBase.Location = new System.Drawing.Point(4, 22);
            this.tbpBase.Name = "tbpBase";
            this.tbpBase.Padding = new System.Windows.Forms.Padding(3);
            this.tbpBase.Size = new System.Drawing.Size(994, 538);
            this.tbpBase.TabIndex = 0;
            this.tbpBase.Text = "※基本信息";
            this.tbpBase.UseVisualStyleBackColor = true;
            // 
            // btnBaseDir
            // 
            this.btnBaseDir.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBaseDir.Location = new System.Drawing.Point(683, 41);
            this.btnBaseDir.Name = "btnBaseDir";
            this.btnBaseDir.Size = new System.Drawing.Size(58, 21);
            this.btnBaseDir.TabIndex = 17;
            this.btnBaseDir.Text = "选择(&S)";
            this.btnBaseDir.UseVisualStyleBackColor = true;
            this.btnBaseDir.Click += new System.EventHandler(this.btnBaseDir_Click);
            // 
            // txtBaseDir
            // 
            this.txtBaseDir.AllowDrop = true;
            this.txtBaseDir.Location = new System.Drawing.Point(70, 40);
            this.txtBaseDir.Name = "txtBaseDir";
            this.txtBaseDir.Size = new System.Drawing.Size(607, 21);
            this.txtBaseDir.TabIndex = 16;
            this.txtBaseDir.Text = "暂时没有完成，用哈希值比较，以上面主程序为基准用这个debug输出比较。有新的就加入";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 15;
            this.label7.Text = "原始目录:";
            // 
            // btnCompareSource
            // 
            this.btnCompareSource.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCompareSource.Location = new System.Drawing.Point(683, 73);
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
            this.txtCompareSource.Location = new System.Drawing.Point(70, 72);
            this.txtCompareSource.Name = "txtCompareSource";
            this.txtCompareSource.Size = new System.Drawing.Size(607, 21);
            this.txtCompareSource.TabIndex = 13;
            this.txtCompareSource.Text = "暂时没有完成，用哈希值比较，以上面主程序为基准用这个debug输出比较。有新的就加入";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "更新来源:";
            // 
            // richTxtLog
            // 
            this.richTxtLog.Location = new System.Drawing.Point(70, 387);
            this.richTxtLog.Name = "richTxtLog";
            this.richTxtLog.Size = new System.Drawing.Size(468, 115);
            this.richTxtLog.TabIndex = 11;
            this.richTxtLog.Text = "";
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(70, 96);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(607, 21);
            this.txtUrl.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "更新网址:";
            // 
            // btnExpt
            // 
            this.btnExpt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExpt.Location = new System.Drawing.Point(882, 122);
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
            this.txtExpt.Location = new System.Drawing.Point(70, 122);
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
            this.label4.Location = new System.Drawing.Point(7, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "排除文件:";
            // 
            // btnDest
            // 
            this.btnDest.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDest.Location = new System.Drawing.Point(552, 364);
            this.btnDest.Name = "btnDest";
            this.btnDest.Size = new System.Drawing.Size(58, 21);
            this.btnDest.TabIndex = 5;
            this.btnDest.Text = "选择(&S)";
            this.btnDest.UseVisualStyleBackColor = true;
            this.btnDest.Click += new System.EventHandler(this.btnSearDes_Click);
            // 
            // txtAutoUpdateXmlSavePath
            // 
            this.txtAutoUpdateXmlSavePath.Location = new System.Drawing.Point(70, 360);
            this.txtAutoUpdateXmlSavePath.Name = "txtAutoUpdateXmlSavePath";
            this.txtAutoUpdateXmlSavePath.Size = new System.Drawing.Size(468, 21);
            this.txtAutoUpdateXmlSavePath.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 364);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "保存位置:";
            // 
            // btnSrc
            // 
            this.btnSrc.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSrc.Location = new System.Drawing.Point(683, 12);
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
            this.txtMainExePath.Location = new System.Drawing.Point(70, 13);
            this.txtMainExePath.Name = "txtMainExePath";
            this.txtMainExePath.Size = new System.Drawing.Size(607, 21);
            this.txtMainExePath.TabIndex = 1;
            this.txtMainExePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSrc_DragDrop);
            this.txtMainExePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSrc_DragEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "主程序:";
            // 
            // tbpOpt
            // 
            this.tbpOpt.Controls.Add(this.splitContainer1);
            this.tbpOpt.Location = new System.Drawing.Point(4, 22);
            this.tbpOpt.Name = "tbpOpt";
            this.tbpOpt.Padding = new System.Windows.Forms.Padding(3);
            this.tbpOpt.Size = new System.Drawing.Size(994, 552);
            this.tbpOpt.TabIndex = 1;
            this.tbpOpt.Text = "※选项";
            this.tbpOpt.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtUpdatedFiles);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chkUseBaseVersion);
            this.splitContainer1.Panel2.Controls.Add(this.txtBaseVerNo);
            this.splitContainer1.Panel2.Controls.Add(this.chk目录转为明细);
            this.splitContainer1.Panel2.Controls.Add(this.chkAppend);
            this.splitContainer1.Panel2.Controls.Add(this.txtVerNo);
            this.splitContainer1.Panel2.Controls.Add(this.chkCustomVer);
            this.splitContainer1.Size = new System.Drawing.Size(988, 546);
            this.splitContainer1.SplitterDistance = 750;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtUpdatedFiles
            // 
            this.txtUpdatedFiles.AllowDrop = true;
            this.txtUpdatedFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUpdatedFiles.Location = new System.Drawing.Point(0, 0);
            this.txtUpdatedFiles.Multiline = true;
            this.txtUpdatedFiles.Name = "txtUpdatedFiles";
            this.txtUpdatedFiles.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtUpdatedFiles.Size = new System.Drawing.Size(750, 546);
            this.txtUpdatedFiles.TabIndex = 0;
            this.txtUpdatedFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtUpdatedFiles_DragDrop);
            this.txtUpdatedFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtUpdatedFiles_DragEnter);
            // 
            // txtBaseVerNo
            // 
            this.txtBaseVerNo.Location = new System.Drawing.Point(19, 179);
            this.txtBaseVerNo.Name = "txtBaseVerNo";
            this.txtBaseVerNo.Size = new System.Drawing.Size(119, 21);
            this.txtBaseVerNo.TabIndex = 4;
            this.txtBaseVerNo.Text = "1.0.0.1";
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
            // txtVerNo
            // 
            this.txtVerNo.Location = new System.Drawing.Point(97, 294);
            this.txtVerNo.Name = "txtVerNo";
            this.txtVerNo.Size = new System.Drawing.Size(119, 21);
            this.txtVerNo.TabIndex = 1;
            // 
            // chkCustomVer
            // 
            this.chkCustomVer.AutoSize = true;
            this.chkCustomVer.Location = new System.Drawing.Point(6, 296);
            this.chkCustomVer.Name = "chkCustomVer";
            this.chkCustomVer.Size = new System.Drawing.Size(96, 16);
            this.chkCustomVer.TabIndex = 0;
            this.chkCustomVer.Text = "自定义版本号";
            this.chkCustomVer.UseVisualStyleBackColor = true;
            this.chkCustomVer.CheckedChanged += new System.EventHandler(this.chkCustomVer_CheckedChanged);
            // 
            // tbpDiffList
            // 
            this.tbpDiffList.Controls.Add(this.txtDiff);
            this.tbpDiffList.Location = new System.Drawing.Point(4, 22);
            this.tbpDiffList.Name = "tbpDiffList";
            this.tbpDiffList.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDiffList.Size = new System.Drawing.Size(994, 538);
            this.tbpDiffList.TabIndex = 2;
            this.tbpDiffList.Text = "※差异文件列表";
            this.tbpDiffList.UseVisualStyleBackColor = true;
            // 
            // txtDiff
            // 
            this.txtDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiff.Location = new System.Drawing.Point(3, 3);
            this.txtDiff.Name = "txtDiff";
            this.txtDiff.Size = new System.Drawing.Size(988, 532);
            this.txtDiff.TabIndex = 1;
            this.txtDiff.Text = "";
            // 
            // tbpLastXml
            // 
            this.tbpLastXml.Controls.Add(this.txtLastXml);
            this.tbpLastXml.Location = new System.Drawing.Point(4, 22);
            this.tbpLastXml.Name = "tbpLastXml";
            this.tbpLastXml.Padding = new System.Windows.Forms.Padding(3);
            this.tbpLastXml.Size = new System.Drawing.Size(994, 538);
            this.tbpLastXml.TabIndex = 3;
            this.tbpLastXml.Text = "※结果";
            this.tbpLastXml.UseVisualStyleBackColor = true;
            // 
            // txtLastXml
            // 
            this.txtLastXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLastXml.Location = new System.Drawing.Point(3, 3);
            this.txtLastXml.Name = "txtLastXml";
            this.txtLastXml.Size = new System.Drawing.Size(988, 532);
            this.txtLastXml.TabIndex = 0;
            this.txtLastXml.Text = "";
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
            // chkUseBaseVersion
            // 
            this.chkUseBaseVersion.AutoSize = true;
            this.chkUseBaseVersion.Location = new System.Drawing.Point(19, 145);
            this.chkUseBaseVersion.Name = "chkUseBaseVersion";
            this.chkUseBaseVersion.Size = new System.Drawing.Size(204, 16);
            this.chkUseBaseVersion.TabIndex = 6;
            this.chkUseBaseVersion.Text = "指定版本【不从文件属性中获取】";
            this.chkUseBaseVersion.UseVisualStyleBackColor = true;
            // 
            // frmAULWriter
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 691);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1024, 900);
            this.Name = "frmAULWriter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AULWriter for AutoUpdater";
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
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tbpDiffList.ResumeLayout(false);
            this.tbpLastXml.ResumeLayout(false);
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
        private System.Windows.Forms.Button btnProduce;
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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkCustomVer;
        private System.Windows.Forms.TextBox txtVerNo;
        private System.Windows.Forms.TextBox txtUpdatedFiles;
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
        private System.Windows.Forms.TextBox txtBaseDir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnBaseDir;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.TextBox txtBaseVerNo;
        private System.Windows.Forms.CheckBox chkUseBaseVersion;
    }
}

