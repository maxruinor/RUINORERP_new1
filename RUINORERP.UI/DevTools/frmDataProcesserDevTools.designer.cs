namespace SMTAPI.ToolForm
{
    partial class frmDataProcesserDevTools
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel选择处理字段 = new System.Windows.Forms.TableLayoutPanel();
            this.gb选择处理方法 = new System.Windows.Forms.GroupBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.cmbColumns = new System.Windows.Forms.ComboBox();
            this.chk显示处理结果 = new System.Windows.Forms.CheckBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProcessTemplates = new System.Windows.Forms.ComboBox();
            this.panel4UC = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbtables = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSQL = new System.Windows.Forms.CheckBox();
            this.txtCSQL = new System.Windows.Forms.TextBox();
            this.chkTop = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnQuery = new System.Windows.Forms.Button();
            this.splitContainer数据表显示区 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看采集原始页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除选择数据行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.复制商品描述ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myRichTextBox1 = new HLH.WinControl.MyRichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdbMySQL = new System.Windows.Forms.RadioButton();
            this.rdbMSSQL = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel选择处理字段.SuspendLayout();
            this.gb选择处理方法.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer数据表显示区)).BeginInit();
            this.splitContainer数据表显示区.Panel1.SuspendLayout();
            this.splitContainer数据表显示区.Panel2.SuspendLayout();
            this.splitContainer数据表显示区.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer数据表显示区);
            this.splitContainer1.Size = new System.Drawing.Size(969, 666);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel选择处理字段);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(484, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(485, 256);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据处理操作";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tableLayoutPanel选择处理字段
            // 
            this.tableLayoutPanel选择处理字段.ColumnCount = 1;
            this.tableLayoutPanel选择处理字段.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel选择处理字段.Controls.Add(this.gb选择处理方法, 0, 0);
            this.tableLayoutPanel选择处理字段.Controls.Add(this.panel4UC, 0, 1);
            this.tableLayoutPanel选择处理字段.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel选择处理字段.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel选择处理字段.Name = "tableLayoutPanel选择处理字段";
            this.tableLayoutPanel选择处理字段.RowCount = 2;
            this.tableLayoutPanel选择处理字段.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel选择处理字段.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel选择处理字段.Size = new System.Drawing.Size(479, 236);
            this.tableLayoutPanel选择处理字段.TabIndex = 0;
            this.tableLayoutPanel选择处理字段.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel选择处理字段_Paint);
            // 
            // gb选择处理方法
            // 
            this.gb选择处理方法.Controls.Add(this.btnProcess);
            this.gb选择处理方法.Controls.Add(this.cmbColumns);
            this.gb选择处理方法.Controls.Add(this.chk显示处理结果);
            this.gb选择处理方法.Controls.Add(this.lbl1);
            this.gb选择处理方法.Controls.Add(this.chkSave);
            this.gb选择处理方法.Controls.Add(this.label1);
            this.gb选择处理方法.Controls.Add(this.cmbProcessTemplates);
            this.gb选择处理方法.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb选择处理方法.Location = new System.Drawing.Point(3, 3);
            this.gb选择处理方法.Name = "gb选择处理方法";
            this.gb选择处理方法.Size = new System.Drawing.Size(473, 59);
            this.gb选择处理方法.TabIndex = 95;
            this.gb选择处理方法.TabStop = false;
            this.gb选择处理方法.Enter += new System.EventHandler(this.gb选择处理方法_Enter);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(368, 15);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(46, 23);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "处理";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // cmbColumns
            // 
            this.cmbColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbColumns.FormattingEnabled = true;
            this.cmbColumns.Location = new System.Drawing.Point(116, 13);
            this.cmbColumns.Name = "cmbColumns";
            this.cmbColumns.Size = new System.Drawing.Size(144, 20);
            this.cmbColumns.TabIndex = 3;
            this.cmbColumns.SelectedIndexChanged += new System.EventHandler(this.cmbColumns_SelectedIndexChanged);
            // 
            // chk显示处理结果
            // 
            this.chk显示处理结果.AutoSize = true;
            this.chk显示处理结果.Checked = true;
            this.chk显示处理结果.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk显示处理结果.Location = new System.Drawing.Point(266, 15);
            this.chk显示处理结果.Name = "chk显示处理结果";
            this.chk显示处理结果.Size = new System.Drawing.Size(96, 16);
            this.chk显示处理结果.TabIndex = 62;
            this.chk显示处理结果.Text = "显示处理结果";
            this.chk显示处理结果.UseVisualStyleBackColor = true;
            this.chk显示处理结果.CheckedChanged += new System.EventHandler(this.chk显示处理结果_CheckedChanged);
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(9, 16);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(101, 12);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "选择要处理的字段";
            this.lbl1.Click += new System.EventHandler(this.lbl1_Click);
            // 
            // chkSave
            // 
            this.chkSave.AutoSize = true;
            this.chkSave.Location = new System.Drawing.Point(266, 37);
            this.chkSave.Name = "chkSave";
            this.chkSave.Size = new System.Drawing.Size(96, 16);
            this.chkSave.TabIndex = 61;
            this.chkSave.Text = "保存处理结果";
            this.chkSave.UseVisualStyleBackColor = true;
            this.chkSave.CheckedChanged += new System.EventHandler(this.chkSave_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择要处理的方法";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // cmbProcessTemplates
            // 
            this.cmbProcessTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcessTemplates.FormattingEnabled = true;
            this.cmbProcessTemplates.Location = new System.Drawing.Point(116, 35);
            this.cmbProcessTemplates.Name = "cmbProcessTemplates";
            this.cmbProcessTemplates.Size = new System.Drawing.Size(144, 20);
            this.cmbProcessTemplates.TabIndex = 4;
            this.cmbProcessTemplates.SelectedIndexChanged += new System.EventHandler(this.cmbProcessTemplates_SelectedIndexChanged);
            // 
            // panel4UC
            // 
            this.panel4UC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4UC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4UC.Location = new System.Drawing.Point(3, 68);
            this.panel4UC.Name = "panel4UC";
            this.panel4UC.Size = new System.Drawing.Size(473, 165);
            this.panel4UC.TabIndex = 0;
            this.panel4UC.Paint += new System.Windows.Forms.PaintEventHandler(this.panel4UC_Paint);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(474, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 256);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.cmbtables);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.chkSQL);
            this.groupBox1.Controls.Add(this.txtCSQL);
            this.groupBox1.Controls.Add(this.chkTop);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(474, 256);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据查询操作";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cmbtables
            // 
            this.cmbtables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtables.FormattingEnabled = true;
            this.cmbtables.Location = new System.Drawing.Point(117, 59);
            this.cmbtables.Name = "cmbtables";
            this.cmbtables.Size = new System.Drawing.Size(231, 20);
            this.cmbtables.TabIndex = 82;
            this.cmbtables.SelectedIndexChanged += new System.EventHandler(this.cmbtables_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 81;
            this.label3.Text = "选择要处理的表:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // chkSQL
            // 
            this.chkSQL.AutoSize = true;
            this.chkSQL.Location = new System.Drawing.Point(12, 161);
            this.chkSQL.Name = "chkSQL";
            this.chkSQL.Size = new System.Drawing.Size(78, 16);
            this.chkSQL.TabIndex = 69;
            this.chkSQL.Text = "自定义SQL";
            this.chkSQL.UseVisualStyleBackColor = true;
            this.chkSQL.CheckedChanged += new System.EventHandler(this.chkSQL_CheckedChanged);
            // 
            // txtCSQL
            // 
            this.txtCSQL.Location = new System.Drawing.Point(12, 183);
            this.txtCSQL.Multiline = true;
            this.txtCSQL.Name = "txtCSQL";
            this.txtCSQL.Size = new System.Drawing.Size(412, 63);
            this.txtCSQL.TabIndex = 68;
            this.txtCSQL.TextChanged += new System.EventHandler(this.txtCSQL_TextChanged);
            // 
            // chkTop
            // 
            this.chkTop.AutoSize = true;
            this.chkTop.Checked = true;
            this.chkTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTop.Location = new System.Drawing.Point(271, 103);
            this.chkTop.Name = "chkTop";
            this.chkTop.Size = new System.Drawing.Size(96, 16);
            this.chkTop.TabIndex = 39;
            this.chkTop.Text = "限定数据行数";
            this.chkTop.UseVisualStyleBackColor = true;
            this.chkTop.CheckedChanged += new System.EventHandler(this.chkTop_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(373, 99);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(51, 21);
            this.numericUpDown1.TabIndex = 38;
            this.numericUpDown1.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(366, 59);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // splitContainer数据表显示区
            // 
            this.splitContainer数据表显示区.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.splitContainer数据表显示区.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer数据表显示区.Location = new System.Drawing.Point(0, 0);
            this.splitContainer数据表显示区.Name = "splitContainer数据表显示区";
            this.splitContainer数据表显示区.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer数据表显示区.Panel1
            // 
            this.splitContainer数据表显示区.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer数据表显示区.Panel2
            // 
            this.splitContainer数据表显示区.Panel2.Controls.Add(this.myRichTextBox1);
            this.splitContainer数据表显示区.Size = new System.Drawing.Size(969, 406);
            this.splitContainer数据表显示区.SplitterDistance = 350;
            this.splitContainer数据表显示区.SplitterWidth = 6;
            this.splitContainer数据表显示区.TabIndex = 22;
            this.splitContainer数据表显示区.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer数据表显示区_SplitterMoved);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(969, 350);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridShow_CellPainting);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看采集原始页ToolStripMenuItem,
            this.删除选择数据行ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.复制商品描述ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 92);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 查看采集原始页ToolStripMenuItem
            // 
            this.查看采集原始页ToolStripMenuItem.Name = "查看采集原始页ToolStripMenuItem";
            this.查看采集原始页ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.查看采集原始页ToolStripMenuItem.Text = "查看采集原始页";
            this.查看采集原始页ToolStripMenuItem.Click += new System.EventHandler(this.查看采集原始页ToolStripMenuItem_Click);
            // 
            // 删除选择数据行ToolStripMenuItem
            // 
            this.删除选择数据行ToolStripMenuItem.Name = "删除选择数据行ToolStripMenuItem";
            this.删除选择数据行ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.删除选择数据行ToolStripMenuItem.Text = "删除选择数据行";
            this.删除选择数据行ToolStripMenuItem.Click += new System.EventHandler(this.删除选择数据行ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(161, 22);
            this.toolStripMenuItem2.Text = "-----------------";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 复制商品描述ToolStripMenuItem
            // 
            this.复制商品描述ToolStripMenuItem.Name = "复制商品描述ToolStripMenuItem";
            this.复制商品描述ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.复制商品描述ToolStripMenuItem.Text = "复制商品描述";
            this.复制商品描述ToolStripMenuItem.Click += new System.EventHandler(this.复制商品描述ToolStripMenuItem_Click);
            // 
            // myRichTextBox1
            // 
            this.myRichTextBox1.CurrentLine = 0;
            this.myRichTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myRichTextBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.myRichTextBox1.Location = new System.Drawing.Point(0, 0);
            this.myRichTextBox1.Name = "myRichTextBox1";
            this.myRichTextBox1.Size = new System.Drawing.Size(969, 50);
            this.myRichTextBox1.SummaryDescription = "1，带有显示行功能;2,从下到上的显示";
            this.myRichTextBox1.TabIndex = 21;
            this.myRichTextBox1.Load += new System.EventHandler(this.myRichTextBox1_Load);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdbMSSQL);
            this.groupBox3.Controls.Add(this.rdbMySQL);
            this.groupBox3.Location = new System.Drawing.Point(12, 17);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(265, 38);
            this.groupBox3.TabIndex = 83;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据库类型";
            // 
            // rdbMySQL
            // 
            this.rdbMySQL.AutoSize = true;
            this.rdbMySQL.Checked = true;
            this.rdbMySQL.Location = new System.Drawing.Point(28, 16);
            this.rdbMySQL.Name = "rdbMySQL";
            this.rdbMySQL.Size = new System.Drawing.Size(53, 16);
            this.rdbMySQL.TabIndex = 0;
            this.rdbMySQL.TabStop = true;
            this.rdbMySQL.Text = "MySQL";
            this.rdbMySQL.UseVisualStyleBackColor = true;
            this.rdbMySQL.CheckedChanged += new System.EventHandler(this.rdbMySQL_CheckedChanged);
            // 
            // rdbMSSQL
            // 
            this.rdbMSSQL.AutoSize = true;
            this.rdbMSSQL.Location = new System.Drawing.Point(153, 18);
            this.rdbMSSQL.Name = "rdbMSSQL";
            this.rdbMSSQL.Size = new System.Drawing.Size(53, 16);
            this.rdbMSSQL.TabIndex = 0;
            this.rdbMSSQL.Text = "MSSQL";
            this.rdbMSSQL.UseVisualStyleBackColor = true;
            this.rdbMSSQL.CheckedChanged += new System.EventHandler(this.rdbMSSQL_CheckedChanged);
            // 
            // frmDataProcesserDevTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 666);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmDataProcesserDevTools";
            this.Text = "frmDataProcesserDevTools";
            this.Load += new System.EventHandler(this.frmDataProcesser_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel选择处理字段.ResumeLayout(false);
            this.gb选择处理方法.ResumeLayout(false);
            this.gb选择处理方法.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.splitContainer数据表显示区.Panel1.ResumeLayout(false);
            this.splitContainer数据表显示区.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer数据表显示区)).EndInit();
            this.splitContainer数据表显示区.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnProcess;
        internal HLH.WinControl.MyRichTextBox myRichTextBox1;
        private System.Windows.Forms.Panel panel4UC;
        private System.Windows.Forms.ComboBox cmbColumns;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProcessTemplates;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.CheckBox chk显示处理结果;
        private System.Windows.Forms.SplitContainer splitContainer数据表显示区;
        private System.Windows.Forms.CheckBox chkTop;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox chkSQL;
        private System.Windows.Forms.TextBox txtCSQL;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 查看采集原始页ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除选择数据行ToolStripMenuItem;
        private System.Windows.Forms.ComboBox cmbtables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 复制商品描述ToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel选择处理字段;
        private System.Windows.Forms.GroupBox gb选择处理方法;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbMSSQL;
        private System.Windows.Forms.RadioButton rdbMySQL;
    }
}