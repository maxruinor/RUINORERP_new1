namespace RUINORERP.Server
{
    partial class frmGlobalConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlobalConfig));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeView1 = new System.Windows.Forms.TreeView();
            propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtnSave = new System.Windows.Forms.ToolStripButton();
            tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            tsbtnUndoButton = new System.Windows.Forms.ToolStripButton();
            tsbtnRedoButton = new System.Windows.Forms.ToolStripButton();
            textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(propertyGrid1);
            splitContainer1.Size = new System.Drawing.Size(800, 454);
            splitContainer1.SplitterDistance = 167;
            splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView1.Location = new System.Drawing.Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new System.Drawing.Size(167, 454);
            treeView1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            propertyGrid1.Location = new System.Drawing.Point(0, 0);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(629, 454);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.SelectedObjectsChanged += propertyGrid1_SelectedObjectsChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtnSave, tsbtnRefresh, tsbtnUndoButton, tsbtnRedoButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(800, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtnSave
            // 
            tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnSave.Image = (System.Drawing.Image)resources.GetObject("tsbtnSave.Image");
            tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnSave.Name = "tsbtnSave";
            tsbtnSave.Size = new System.Drawing.Size(36, 22);
            tsbtnSave.Text = "保存";
            tsbtnSave.Click += tsbtnSave_Click;
            // 
            // tsbtnRefresh
            // 
            tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnRefresh.Image = (System.Drawing.Image)resources.GetObject("tsbtnRefresh.Image");
            tsbtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRefresh.Name = "tsbtnRefresh";
            tsbtnRefresh.Size = new System.Drawing.Size(36, 22);
            tsbtnRefresh.Text = "刷新";
            tsbtnRefresh.Click += tsbtnRefresh_Click;
            // 
            // tsbtnUndoButton
            // 
            tsbtnUndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnUndoButton.Image = (System.Drawing.Image)resources.GetObject("tsbtnUndoButton.Image");
            tsbtnUndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnUndoButton.Name = "tsbtnUndoButton";
            tsbtnUndoButton.Size = new System.Drawing.Size(36, 22);
            tsbtnUndoButton.Text = "撤销";
            tsbtnUndoButton.Click += tsbtnUndoButton_Click;
            // 
            // tsbtnRedoButton
            // 
            tsbtnRedoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtnRedoButton.Image = (System.Drawing.Image)resources.GetObject("tsbtnRedoButton.Image");
            tsbtnRedoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtnRedoButton.Name = "tsbtnRedoButton";
            tsbtnRedoButton.Size = new System.Drawing.Size(36, 22);
            tsbtnRedoButton.Text = "重做";
            tsbtnRedoButton.Click += tsbtnRedoButton_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            textBox1.Location = new System.Drawing.Point(0, 479);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(800, 69);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // frmGlobalConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 548);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Controls.Add(textBox1);
            Name = "frmGlobalConfig";
            Text = "全局配置中心";
            Load += frmGlobalConfig_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripButton tsbtnRefresh;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripButton tsbtnUndoButton;
        private System.Windows.Forms.ToolStripButton tsbtnRedoButton;
    }
}