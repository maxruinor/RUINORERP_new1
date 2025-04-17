namespace RUINORERP.Server
{
    partial class frmBlacklist
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
            components = new System.ComponentModel.Container();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            解除IPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            添加IPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(800, 450);
            dataGridView1.TabIndex = 1;
            dataGridView1.DataError += dataGridView1_DataError;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 刷新ToolStripMenuItem, 解除IPToolStripMenuItem, 添加IPToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(112, 70);
            // 
            // 刷新ToolStripMenuItem
            // 
            刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            刷新ToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            刷新ToolStripMenuItem.Text = "刷新";
            刷新ToolStripMenuItem.Click += 刷新ToolStripMenuItem_Click;
            // 
            // 解除IPToolStripMenuItem
            // 
            解除IPToolStripMenuItem.Name = "解除IPToolStripMenuItem";
            解除IPToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            解除IPToolStripMenuItem.Text = "解除IP";
            解除IPToolStripMenuItem.Click += 解除IPToolStripMenuItem_Click;
            // 
            // 添加IPToolStripMenuItem
            // 
            添加IPToolStripMenuItem.Name = "添加IPToolStripMenuItem";
            添加IPToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            添加IPToolStripMenuItem.Text = "添加IP";
            添加IPToolStripMenuItem.Click += 添加IPToolStripMenuItem_Click;
            // 
            // frmBlacklist
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(dataGridView1);
            Name = "frmBlacklist";
            Text = "frmBlacklist";
            Load += frmBlacklist_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 解除IPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加IPToolStripMenuItem;
    }
}