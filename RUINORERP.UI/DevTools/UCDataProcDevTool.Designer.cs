namespace RUINORERP.UI.DevTools
{
    partial class UCDataProcDevTool
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
            this.components = new System.ComponentModel.Container();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除选择数据行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查看采集原始页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制产品描述ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer数据表显示区 = new System.Windows.Forms.SplitContainer();
            this.cmbtables = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSQL = new System.Windows.Forms.CheckBox();
            this.txtCSQL = new System.Windows.Forms.TextBox();
            this.chkTop = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btnQuery = new System.Windows.Forms.Button();
            this.rdbMSSQL = new System.Windows.Forms.RadioButton();
            this.rdbMySQL = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel4UC = new System.Windows.Forms.Panel();
            this.btnProcess = new System.Windows.Forms.Button();
            this.cmbColumns = new System.Windows.Forms.ComboBox();
            this.chk显示处理结果 = new System.Windows.Forms.CheckBox();
            this.lbl1 = new System.Windows.Forms.Label();
            this.chkSave = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProcessTemplates = new System.Windows.Forms.ComboBox();
            this.gb选择处理方法 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel选择处理字段 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer数据表显示区)).BeginInit();
            this.splitContainer数据表显示区.Panel1.SuspendLayout();
            this.splitContainer数据表显示区.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.gb选择处理方法.SuspendLayout();
            this.tableLayoutPanel选择处理字段.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(159, 22);
            this.toolStripMenuItem2.Text = "-----------------";
            // 
            // 删除选择数据行ToolStripMenuItem
            // 
            this.删除选择数据行ToolStripMenuItem.Name = "删除选择数据行ToolStripMenuItem";
            this.删除选择数据行ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.删除选择数据行ToolStripMenuItem.Text = "删除选择数据行";
            this.删除选择数据行ToolStripMenuItem.Click += new System.EventHandler(this.删除选择数据行ToolStripMenuItem_Click);
            // 
            // 查看采集原始页ToolStripMenuItem
            // 
            this.查看采集原始页ToolStripMenuItem.Name = "查看采集原始页ToolStripMenuItem";
            this.查看采集原始页ToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.查看采集原始页ToolStripMenuItem.Text = "查看采集原始页";
            this.查看采集原始页ToolStripMenuItem.Click += new System.EventHandler(this.查看采集原始页ToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看采集原始页ToolStripMenuItem,
            this.删除选择数据行ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.复制产品描述ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 92);
            // 
            // 复制产品描述ToolStripMenuItem
            // 
            this.复制产品描述ToolStripMenuItem.Name = "复制产品描述ToolStripMenuItem";
            this.复制产品描述ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.复制产品描述ToolStripMenuItem.Text = "复制产品描述";
            this.复制产品描述ToolStripMenuItem.Click += new System.EventHandler(this.复制产品描述ToolStripMenuItem_Click);
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
            this.dataGridView1.Size = new System.Drawing.Size(1057, 409);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
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
            this.splitContainer数据表显示区.Size = new System.Drawing.Size(1057, 475);
            this.splitContainer数据表显示区.SplitterDistance = 409;
            this.splitContainer数据表显示区.SplitterWidth = 6;
            this.splitContainer数据表显示区.TabIndex = 22;
            // 
            // cmbtables
            // 
            this.cmbtables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtables.FormattingEnabled = true;
            this.cmbtables.Location = new System.Drawing.Point(117, 59);
            this.cmbtables.Name = "cmbtables";
            this.cmbtables.Size = new System.Drawing.Size(231, 20);
            this.cmbtables.TabIndex = 82;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 81;
            this.label3.Text = "选择要处理的表:";
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
            // 
            // txtCSQL
            // 
            this.txtCSQL.Location = new System.Drawing.Point(12, 183);
            this.txtCSQL.Multiline = true;
            this.txtCSQL.Name = "txtCSQL";
            this.txtCSQL.Size = new System.Drawing.Size(412, 63);
            this.txtCSQL.TabIndex = 68;
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
            // rdbMSSQL
            // 
            this.rdbMSSQL.AutoSize = true;
            this.rdbMSSQL.Location = new System.Drawing.Point(153, 18);
            this.rdbMSSQL.Name = "rdbMSSQL";
            this.rdbMSSQL.Size = new System.Drawing.Size(53, 16);
            this.rdbMSSQL.TabIndex = 0;
            this.rdbMSSQL.Text = "MSSQL";
            this.rdbMSSQL.UseVisualStyleBackColor = true;
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
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(474, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(10, 298);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
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
            this.groupBox1.Size = new System.Drawing.Size(474, 298);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据查询操作";
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
            // panel4UC
            // 
            this.panel4UC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4UC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4UC.Location = new System.Drawing.Point(3, 68);
            this.panel4UC.Name = "panel4UC";
            this.panel4UC.Size = new System.Drawing.Size(561, 207);
            this.panel4UC.TabIndex = 0;
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
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Location = new System.Drawing.Point(9, 16);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(101, 12);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "选择要处理的字段";
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
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择要处理的方法";
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
            this.gb选择处理方法.Size = new System.Drawing.Size(561, 59);
            this.gb选择处理方法.TabIndex = 95;
            this.gb选择处理方法.TabStop = false;
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
            this.tableLayoutPanel选择处理字段.Size = new System.Drawing.Size(567, 278);
            this.tableLayoutPanel选择处理字段.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel选择处理字段);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(484, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(573, 298);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据处理操作";
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
            this.splitContainer1.Size = new System.Drawing.Size(1057, 777);
            this.splitContainer1.SplitterDistance = 298;
            this.splitContainer1.TabIndex = 1;
            // 
            // UCDataProcDevTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "UCDataProcDevTool";
            this.Size = new System.Drawing.Size(1057, 777);
            this.Load += new System.EventHandler(this.UCDataProcDevTool_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer数据表显示区.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer数据表显示区)).EndInit();
            this.splitContainer数据表显示区.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.gb选择处理方法.ResumeLayout(false);
            this.gb选择处理方法.PerformLayout();
            this.tableLayoutPanel选择处理字段.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除选择数据行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查看采集原始页ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 复制产品描述ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer数据表显示区;
        private System.Windows.Forms.ComboBox cmbtables;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkSQL;
        private System.Windows.Forms.TextBox txtCSQL;
        private System.Windows.Forms.CheckBox chkTop;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.RadioButton rdbMSSQL;
        private System.Windows.Forms.RadioButton rdbMySQL;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel4UC;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.ComboBox cmbColumns;
        private System.Windows.Forms.CheckBox chk显示处理结果;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.CheckBox chkSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProcessTemplates;
        private System.Windows.Forms.GroupBox gb选择处理方法;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel选择处理字段;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
