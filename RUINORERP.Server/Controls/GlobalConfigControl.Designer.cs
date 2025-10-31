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
            tsbtnPublish = new System.Windows.Forms.ToolStripButton();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            treeView2 = new System.Windows.Forms.TreeView();
            groupBoxConfigEdit = new System.Windows.Forms.GroupBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            groupBoxConfigEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // tsbtnPublish
            // 
            tsbtnPublish.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnPublish.Name = "tsbtnPublish";
            tsbtnPublish.Size = new System.Drawing.Size(48, 22);
            tsbtnPublish.Text = "发布";
            tsbtnPublish.Click += tsbtnPublish_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSave, tsbtnUndo, tsbtnRedo, toolStripSeparator1, tsbtnRefresh, tsbtnHistory, tsbtnPublish });
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
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 25);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(treeView2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(groupBoxConfigEdit);
            splitContainer2.Size = new System.Drawing.Size(933, 600);
            splitContainer2.SplitterDistance = 203;
            splitContainer2.TabIndex = 0;
            // 
            // treeView2
            // 
            treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView2.Location = new System.Drawing.Point(0, 0);
            treeView2.Name = "treeView2";
            treeView2.Size = new System.Drawing.Size(203, 600);
            treeView2.TabIndex = 0;
            treeView2.AfterSelect += treeView2_AfterSelect;
            // 
            // groupBoxConfigEdit
            // 
            groupBoxConfigEdit.Controls.Add(splitContainer1);
            groupBoxConfigEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxConfigEdit.Location = new System.Drawing.Point(0, 0);
            groupBoxConfigEdit.Margin = new System.Windows.Forms.Padding(4);
            groupBoxConfigEdit.Name = "groupBoxConfigEdit";
            groupBoxConfigEdit.Padding = new System.Windows.Forms.Padding(4);
            groupBoxConfigEdit.Size = new System.Drawing.Size(726, 600);
            groupBoxConfigEdit.TabIndex = 1;
            groupBoxConfigEdit.TabStop = false;
            groupBoxConfigEdit.Text = "配置项目编辑";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(4, 20);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(propertyGrid1);
            splitContainer1.Size = new System.Drawing.Size(718, 576);
            splitContainer1.SplitterDistance = 477;
            splitContainer1.TabIndex = 1;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(477, 576);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.PropertyValueChanged += propertyGrid1_PropertyValueChanged;
            // 
            // GlobalConfigControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainer2);
            Controls.Add(toolStrip1);
            Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "GlobalConfigControl";
            Size = new System.Drawing.Size(933, 625);
            Load += GlobalConfigControl_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            groupBoxConfigEdit.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton tsbtnPublish;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.GroupBox groupBoxConfigEdit;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}