namespace RUINORERP.Server.Controls
{
    partial class GlobalConfigControl
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
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnSave = new System.Windows.Forms.ToolStripButton();
            tsbtnUndo = new System.Windows.Forms.ToolStripButton();
            tsbtnRedo = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            tsbtnHistory = new System.Windows.Forms.ToolStripButton();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPageBasic = new System.Windows.Forms.TabPage();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            treeView2 = new System.Windows.Forms.TreeView();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeView1 = new System.Windows.Forms.TreeView();
            propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            tabPageAdvanced = new System.Windows.Forms.TabPage();
            groupBox1 = new System.Windows.Forms.GroupBox();
            textBox1 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            tabPageHistory = new System.Windows.Forms.TabPage();
            listViewHistory = new System.Windows.Forms.ListView();
            columnHeader1 = new System.Windows.Forms.ColumnHeader();
            columnHeader2 = new System.Windows.Forms.ColumnHeader();
            columnHeader3 = new System.Windows.Forms.ColumnHeader();
            columnHeader4 = new System.Windows.Forms.ColumnHeader();
            toolStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPageBasic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            tabPageAdvanced.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPageHistory.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSave, tsbtnUndo, tsbtnRedo, toolStripSeparator1, tsbtnRefresh, tsbtnHistory });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            tsbtnSave.Image = Properties.Resources.save;
            tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnSave.Name = "tsbtnSave";
            tsbtnSave.Size = new System.Drawing.Size(52, 22);
            tsbtnSave.Text = "保存";
            tsbtnSave.Click += tsbtnSave_Click;
            // 
            // tsbtnUndo
            // 
            tsbtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnUndo.Name = "tsbtnUndo";
            tsbtnUndo.Size = new System.Drawing.Size(36, 22);
            tsbtnUndo.Text = "撤销";
            tsbtnUndo.Click += tsbtnUndo_Click;
            // 
            // tsbtnRedo
            // 
            tsbtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRedo.Name = "tsbtnRedo";
            tsbtnRedo.Size = new System.Drawing.Size(36, 22);
            tsbtnRedo.Text = "重做";
            tsbtnRedo.Click += tsbtnRedo_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnRefresh
            // 
            tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRefresh.Name = "tsbtnRefresh";
            tsbtnRefresh.Size = new System.Drawing.Size(36, 22);
            tsbtnRefresh.Text = "刷新";
            tsbtnRefresh.Click += tsbtnRefresh_Click;
            // 
            // tsbtnHistory
            // 
            tsbtnHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnHistory.Name = "tsbtnHistory";
            tsbtnHistory.Size = new System.Drawing.Size(60, 22);
            tsbtnHistory.Text = "历史记录";
            tsbtnHistory.Click += tsbtnHistory_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageBasic);
            tabControl1.Controls.Add(tabPageAdvanced);
            tabControl1.Controls.Add(tabPageHistory);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Location = new System.Drawing.Point(0, 25);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(933, 600);
            tabControl1.TabIndex = 1;
            // 
            // tabPageBasic
            // 
            tabPageBasic.Controls.Add(splitContainer2);
            tabPageBasic.Location = new System.Drawing.Point(4, 26);
            tabPageBasic.Name = "tabPageBasic";
            tabPageBasic.Padding = new System.Windows.Forms.Padding(3);
            tabPageBasic.Size = new System.Drawing.Size(925, 570);
            tabPageBasic.TabIndex = 0;
            tabPageBasic.Text = "基本配置";
            tabPageBasic.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(3, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(treeView2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(splitContainer1);
            splitContainer2.Size = new System.Drawing.Size(919, 564);
            splitContainer2.SplitterDistance = 200;
            splitContainer2.TabIndex = 0;
            // 
            // treeView2
            // 
            treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView2.Location = new System.Drawing.Point(0, 0);
            treeView2.Name = "treeView2";
            treeView2.Size = new System.Drawing.Size(200, 564);
            treeView2.TabIndex = 0;
            treeView2.AfterSelect += treeView2_AfterSelect;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(propertyGrid1);
            splitContainer1.Size = new System.Drawing.Size(715, 564);
            splitContainer1.SplitterDistance = 175;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(175, 564);
            treeView1.TabIndex = 0;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(536, 564);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.PropertyValueChanged += propertyGrid1_PropertyValueChanged;
            // 
            // tabPageAdvanced
            // 
            tabPageAdvanced.Controls.Add(groupBox1);
            tabPageAdvanced.Location = new System.Drawing.Point(4, 26);
            tabPageAdvanced.Name = "tabPageAdvanced";
            tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            tabPageAdvanced.Size = new System.Drawing.Size(925, 570);
            tabPageAdvanced.TabIndex = 1;
            tabPageAdvanced.Text = "高级配置";
            tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox1.Location = new System.Drawing.Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(919, 100);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "属性编辑";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(88, 42);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(300, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 45);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(56, 17);
            label1.TabIndex = 0;
            label1.Text = "属性值：";
            // 
            // tabPageHistory
            // 
            tabPageHistory.Controls.Add(listViewHistory);
            tabPageHistory.Location = new System.Drawing.Point(4, 26);
            tabPageHistory.Name = "tabPageHistory";
            tabPageHistory.Padding = new System.Windows.Forms.Padding(3);
            tabPageHistory.Size = new System.Drawing.Size(925, 570);
            tabPageHistory.TabIndex = 2;
            tabPageHistory.Text = "历史记录";
            tabPageHistory.UseVisualStyleBackColor = true;
            // 
            // listViewHistory
            // 
            listViewHistory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4 });
            listViewHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewHistory.FullRowSelect = true;
            listViewHistory.GridLines = true;
            listViewHistory.Location = new System.Drawing.Point(3, 3);
            listViewHistory.Name = "listViewHistory";
            listViewHistory.Size = new System.Drawing.Size(919, 564);
            listViewHistory.TabIndex = 0;
            listViewHistory.UseCompatibleStateImageBehavior = false;
            listViewHistory.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "时间";
            columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "操作";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "描述";
            columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "用户";
            columnHeader4.Width = 150;
            // 
            // GlobalConfigControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Controls.Add(toolStrip1);
            Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "GlobalConfigControl";
            Size = new System.Drawing.Size(933, 625);
            Load += GlobalConfigControl_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPageBasic.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            tabPageAdvanced.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPageHistory.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripButton tsbtnUndo;
        private System.Windows.Forms.ToolStripButton tsbtnRedo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtnRefresh;
        private System.Windows.Forms.ToolStripButton tsbtnHistory;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageBasic;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageHistory;
        private System.Windows.Forms.ListView listViewHistory;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}