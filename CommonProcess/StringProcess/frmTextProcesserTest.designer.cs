namespace CommonProcess.StringProcess
{
    partial class frmTextProcesserTest
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
            this.btn模拟批处理 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btn处理外部事件 = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProcessTemplates = new System.Windows.Forms.ComboBox();
            this.panel4UC = new System.Windows.Forms.Panel();
            this.chklist动作队列 = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除选中运作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除全部动作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新选中动作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnUpdateAction = new System.Windows.Forms.Button();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox源 = new System.Windows.Forms.RichTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.richTextBox结果 = new System.Windows.Forms.RichTextBox();
            this.myRichMsg = new HLH.WinControl.MyRichTextBox();
            this.chk解析JSON到对象 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel选择处理字段.SuspendLayout();
            this.gb选择处理方法.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1180, 818);
            this.splitContainer1.SplitterDistance = 380;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel选择处理字段);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1180, 380);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据处理操作";
            // 
            // tableLayoutPanel选择处理字段
            // 
            this.tableLayoutPanel选择处理字段.ColumnCount = 2;
            this.tableLayoutPanel选择处理字段.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel选择处理字段.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 195F));
            this.tableLayoutPanel选择处理字段.Controls.Add(this.gb选择处理方法, 0, 0);
            this.tableLayoutPanel选择处理字段.Controls.Add(this.panel4UC, 0, 1);
            this.tableLayoutPanel选择处理字段.Controls.Add(this.chklist动作队列, 1, 1);
            this.tableLayoutPanel选择处理字段.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel选择处理字段.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel选择处理字段.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel选择处理字段.Name = "tableLayoutPanel选择处理字段";
            this.tableLayoutPanel选择处理字段.RowCount = 2;
            this.tableLayoutPanel选择处理字段.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel选择处理字段.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel选择处理字段.Size = new System.Drawing.Size(1174, 360);
            this.tableLayoutPanel选择处理字段.TabIndex = 0;
            // 
            // gb选择处理方法
            // 
            this.gb选择处理方法.Controls.Add(this.chk解析JSON到对象);
            this.gb选择处理方法.Controls.Add(this.btn模拟批处理);
            this.gb选择处理方法.Controls.Add(this.btnSave);
            this.gb选择处理方法.Controls.Add(this.btn处理外部事件);
            this.gb选择处理方法.Controls.Add(this.btnProcess);
            this.gb选择处理方法.Controls.Add(this.label1);
            this.gb选择处理方法.Controls.Add(this.cmbProcessTemplates);
            this.gb选择处理方法.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb选择处理方法.Location = new System.Drawing.Point(3, 3);
            this.gb选择处理方法.Name = "gb选择处理方法";
            this.gb选择处理方法.Size = new System.Drawing.Size(973, 59);
            this.gb选择处理方法.TabIndex = 95;
            this.gb选择处理方法.TabStop = false;
            // 
            // btn模拟批处理
            // 
            this.btn模拟批处理.Location = new System.Drawing.Point(673, 19);
            this.btn模拟批处理.Name = "btn模拟批处理";
            this.btn模拟批处理.Size = new System.Drawing.Size(88, 32);
            this.btn模拟批处理.TabIndex = 7;
            this.btn模拟批处理.Text = "模拟批处理";
            this.btn模拟批处理.UseVisualStyleBackColor = true;
            this.btn模拟批处理.Click += new System.EventHandler(this.btn模拟批处理_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(888, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "保存配置";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btn处理外部事件
            // 
            this.btn处理外部事件.Location = new System.Drawing.Point(559, 19);
            this.btn处理外部事件.Name = "btn处理外部事件";
            this.btn处理外部事件.Size = new System.Drawing.Size(97, 34);
            this.btn处理外部事件.TabIndex = 5;
            this.btn处理外部事件.Text = "处理外部数据";
            this.btn处理外部事件.UseVisualStyleBackColor = true;
            this.btn处理外部事件.Click += new System.EventHandler(this.btn处理外部事件_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(435, 19);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(76, 34);
            this.btnProcess.TabIndex = 2;
            this.btnProcess.Text = "测试处理";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择要处理的方法";
            // 
            // cmbProcessTemplates
            // 
            this.cmbProcessTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProcessTemplates.FormattingEnabled = true;
            this.cmbProcessTemplates.Location = new System.Drawing.Point(115, 27);
            this.cmbProcessTemplates.Name = "cmbProcessTemplates";
            this.cmbProcessTemplates.Size = new System.Drawing.Size(213, 20);
            this.cmbProcessTemplates.TabIndex = 4;
            this.cmbProcessTemplates.SelectedIndexChanged += new System.EventHandler(this.cmbProcessTemplates_SelectedIndexChanged);
            // 
            // panel4UC
            // 
            this.panel4UC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4UC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4UC.Location = new System.Drawing.Point(3, 68);
            this.panel4UC.Name = "panel4UC";
            this.panel4UC.Size = new System.Drawing.Size(973, 289);
            this.panel4UC.TabIndex = 0;
            // 
            // chklist动作队列
            // 
            this.chklist动作队列.AllowDrop = true;
            this.chklist动作队列.ContextMenuStrip = this.contextMenuStrip1;
            this.chklist动作队列.FormattingEnabled = true;
            this.chklist动作队列.Location = new System.Drawing.Point(982, 68);
            this.chklist动作队列.Name = "chklist动作队列";
            this.chklist动作队列.Size = new System.Drawing.Size(189, 276);
            this.chklist动作队列.TabIndex = 113;
            this.chklist动作队列.SelectedIndexChanged += new System.EventHandler(this.chklist动作队列_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除选中运作ToolStripMenuItem,
            this.删除全部动作ToolStripMenuItem,
            this.更新选中动作ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 70);
            // 
            // 删除选中运作ToolStripMenuItem
            // 
            this.删除选中运作ToolStripMenuItem.Name = "删除选中运作ToolStripMenuItem";
            this.删除选中运作ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除选中运作ToolStripMenuItem.Text = "删除选中动作";
            this.删除选中运作ToolStripMenuItem.Click += new System.EventHandler(this.删除选中运作ToolStripMenuItem_Click);
            // 
            // 删除全部动作ToolStripMenuItem
            // 
            this.删除全部动作ToolStripMenuItem.Name = "删除全部动作ToolStripMenuItem";
            this.删除全部动作ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.删除全部动作ToolStripMenuItem.Text = "删除全部动作";
            this.删除全部动作ToolStripMenuItem.Click += new System.EventHandler(this.删除全部动作ToolStripMenuItem_Click);
            // 
            // 更新选中动作ToolStripMenuItem
            // 
            this.更新选中动作ToolStripMenuItem.Name = "更新选中动作ToolStripMenuItem";
            this.更新选中动作ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.更新选中动作ToolStripMenuItem.Text = "更新选中动作";
            this.更新选中动作ToolStripMenuItem.Click += new System.EventHandler(this.更新选中动作ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnUpdateAction);
            this.panel1.Controls.Add(this.btnAddAction);
            this.panel1.Location = new System.Drawing.Point(982, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 59);
            this.panel1.TabIndex = 114;
            // 
            // btnUpdateAction
            // 
            this.btnUpdateAction.Location = new System.Drawing.Point(108, 4);
            this.btnUpdateAction.Name = "btnUpdateAction";
            this.btnUpdateAction.Size = new System.Drawing.Size(75, 29);
            this.btnUpdateAction.TabIndex = 1;
            this.btnUpdateAction.Text = "更新动作";
            this.btnUpdateAction.UseVisualStyleBackColor = true;
            this.btnUpdateAction.Click += new System.EventHandler(this.btnUpdateAction_Click);
            // 
            // btnAddAction
            // 
            this.btnAddAction.Location = new System.Drawing.Point(4, 4);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(75, 29);
            this.btnAddAction.TabIndex = 0;
            this.btnAddAction.Text = "添加动作";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer2.Size = new System.Drawing.Size(1180, 434);
            this.splitContainer2.SplitterDistance = 523;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox源);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(523, 434);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "输入源";
            // 
            // richTextBox源
            // 
            this.richTextBox源.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox源.Location = new System.Drawing.Point(3, 17);
            this.richTextBox源.Name = "richTextBox源";
            this.richTextBox源.Size = new System.Drawing.Size(517, 414);
            this.richTextBox源.TabIndex = 0;
            this.richTextBox源.Text = "";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.splitContainer3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(653, 434);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "输入结果";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 17);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.richTextBox结果);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.myRichMsg);
            this.splitContainer3.Size = new System.Drawing.Size(647, 414);
            this.splitContainer3.SplitterDistance = 207;
            this.splitContainer3.TabIndex = 2;
            // 
            // richTextBox结果
            // 
            this.richTextBox结果.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox结果.Location = new System.Drawing.Point(0, 0);
            this.richTextBox结果.Name = "richTextBox结果";
            this.richTextBox结果.Size = new System.Drawing.Size(647, 207);
            this.richTextBox结果.TabIndex = 1;
            this.richTextBox结果.Text = "";
            // 
            // myRichMsg
            // 
            this.myRichMsg.CurrentLine = 0;
            this.myRichMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myRichMsg.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.myRichMsg.Location = new System.Drawing.Point(0, 0);
            this.myRichMsg.Name = "myRichMsg";
            this.myRichMsg.Size = new System.Drawing.Size(647, 203);
            this.myRichMsg.SummaryDescription = "1，带有显示行功能;2,从下到上的显示";
            this.myRichMsg.TabIndex = 115;
            // 
            // chk解析JSON到对象
            // 
            this.chk解析JSON到对象.AutoSize = true;
            this.chk解析JSON到对象.Location = new System.Drawing.Point(767, 28);
            this.chk解析JSON到对象.Name = "chk解析JSON到对象";
            this.chk解析JSON到对象.Size = new System.Drawing.Size(108, 16);
            this.chk解析JSON到对象.TabIndex = 8;
            this.chk解析JSON到对象.Text = "解析JSON到对象";
            this.chk解析JSON到对象.UseVisualStyleBackColor = true;
            // 
            // frmTextProcesserTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 818);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmTextProcesserTest";
            this.Text = "frmTextProcesserTest";
            this.Load += new System.EventHandler(this.frmTextProcesserTest_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel选择处理字段.ResumeLayout(false);
            this.gb选择处理方法.ResumeLayout(false);
            this.gb选择处理方法.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.RichTextBox richTextBox结果;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel选择处理字段;
        private System.Windows.Forms.GroupBox gb选择处理方法;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProcessTemplates;
        private System.Windows.Forms.Panel panel4UC;
        private System.Windows.Forms.Button btn处理外部事件;
        public System.Windows.Forms.RichTextBox richTextBox源;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btn模拟批处理;
        private System.Windows.Forms.CheckedListBox chklist动作队列;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除选中运作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除全部动作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新选中动作ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddAction;
        private System.Windows.Forms.Button btnUpdateAction;
        private System.Windows.Forms.SplitContainer splitContainer3;
        internal HLH.WinControl.MyRichTextBox myRichMsg;
        private System.Windows.Forms.CheckBox chk解析JSON到对象;
    }
}