using System;

namespace RUINORERP.Server
{
    partial class frmCacheManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCacheManage));
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            listBoxTableList = new System.Windows.Forms.ListBox();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            加载缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            bindingSource1 = new System.Windows.Forms.BindingSource(components);
            bindingSource2 = new System.Windows.Forms.BindingSource(components);
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            toolStripButton加载缓存 = new System.Windows.Forms.ToolStripButton();
            toolStripButton刷新缓存 = new System.Windows.Forms.ToolStripButton();
            cmbUser = new System.Windows.Forms.ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource2).BeginInit();
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
            splitContainer1.Panel1.Controls.Add(listBoxTableList);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(dataGridView1);
            splitContainer1.Size = new System.Drawing.Size(805, 623);
            splitContainer1.SplitterDistance = 219;
            splitContainer1.TabIndex = 0;
            // 
            // listBoxTableList
            // 
            listBoxTableList.ContextMenuStrip = contextMenuStrip1;
            listBoxTableList.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxTableList.FormattingEnabled = true;
            listBoxTableList.ItemHeight = 17;
            listBoxTableList.Location = new System.Drawing.Point(0, 0);
            listBoxTableList.Name = "listBoxTableList";
            listBoxTableList.Size = new System.Drawing.Size(219, 623);
            listBoxTableList.TabIndex = 0;
            listBoxTableList.SelectedIndexChanged += listBoxTableList_SelectedIndexChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 推送缓存数据ToolStripMenuItem, 加载缓存数据ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            推送缓存数据ToolStripMenuItem.Click += 推送缓存数据ToolStripMenuItem_Click;
            // 
            // 加载缓存数据ToolStripMenuItem
            // 
            加载缓存数据ToolStripMenuItem.Name = "加载缓存数据ToolStripMenuItem";
            加载缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            加载缓存数据ToolStripMenuItem.Text = "加载缓存数据";
            加载缓存数据ToolStripMenuItem.Click += 加载缓存数据ToolStripMenuItem_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(582, 623);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripButton加载缓存, toolStripButton刷新缓存, cmbUser });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(805, 25);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton加载缓存
            // 
            toolStripButton加载缓存.Image = (System.Drawing.Image)resources.GetObject("toolStripButton加载缓存.Image");
            toolStripButton加载缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton加载缓存.Name = "toolStripButton加载缓存";
            toolStripButton加载缓存.Size = new System.Drawing.Size(76, 22);
            toolStripButton加载缓存.Text = "加载缓存";
            toolStripButton加载缓存.Click += toolStripButton加载缓存_Click;
            // 
            // toolStripButton刷新缓存
            // 
            toolStripButton刷新缓存.Image = (System.Drawing.Image)resources.GetObject("toolStripButton刷新缓存.Image");
            toolStripButton刷新缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            toolStripButton刷新缓存.Name = "toolStripButton刷新缓存";
            toolStripButton刷新缓存.Size = new System.Drawing.Size(76, 22);
            toolStripButton刷新缓存.Text = "刷新缓存";
            toolStripButton刷新缓存.Click += toolStripButton刷新缓存_Click;
            // 
            // cmbUser
            // 
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new System.Drawing.Size(121, 25);
            // 
            // frmCacheManage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(805, 648);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Name = "frmCacheManage";
            Text = "缓存管理";
            Load += frmCacheManagement_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bindingSource2).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.ListBox listBoxTableList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton刷新缓存;
        private System.Windows.Forms.ToolStripButton toolStripButton加载缓存;
        private System.Windows.Forms.ToolStripComboBox cmbUser;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载缓存数据ToolStripMenuItem;
    }
}