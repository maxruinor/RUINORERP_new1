namespace RUINORERP.Server
{
    partial class frmUserList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserList));
            listView1 = new System.Windows.Forms.ListView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            全部切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            更新全局配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtn刷新 = new System.Windows.Forms.ToolStripButton();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            btnReverseSelected = new System.Windows.Forms.Button();
            btnNotAllSelected = new System.Windows.Forms.Button();
            btnSelectedAll = new System.Windows.Forms.Button();
            contextMenuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(911, 448);
            listView1.TabIndex = 3;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.VirtualMode = true;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6, 推送版本更新ToolStripMenuItem, 推送缓存数据ToolStripMenuItem, 切换服务器ToolStripMenuItem, 全部切换服务器ToolStripMenuItem, 更新全局配置ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(161, 224);
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItem1.Text = "断开连接";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItem2.Text = "强制用户退出";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItem4.Text = "删除列配置文件";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItem5.Text = "发消息给客户端";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(160, 22);
            toolStripMenuItem6.Text = "关机";
            // 
            // 推送版本更新ToolStripMenuItem
            // 
            推送版本更新ToolStripMenuItem.Name = "推送版本更新ToolStripMenuItem";
            推送版本更新ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            推送版本更新ToolStripMenuItem.Text = "推送版本更新";
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            // 
            // 切换服务器ToolStripMenuItem
            // 
            切换服务器ToolStripMenuItem.Name = "切换服务器ToolStripMenuItem";
            切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            切换服务器ToolStripMenuItem.Text = "切换服务器";
            // 
            // 全部切换服务器ToolStripMenuItem
            // 
            全部切换服务器ToolStripMenuItem.Name = "全部切换服务器ToolStripMenuItem";
            全部切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            全部切换服务器ToolStripMenuItem.Text = "全部切换服务器";
            // 
            // 更新全局配置ToolStripMenuItem
            // 
            更新全局配置ToolStripMenuItem.Name = "更新全局配置ToolStripMenuItem";
            更新全局配置ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            更新全局配置ToolStripMenuItem.Text = "更新全局配置";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtn刷新 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(911, 25);
            toolStrip1.TabIndex = 4;
            toolStrip1.Text = "toolStrip1";
            // 
            // tsbtn刷新
            // 
            tsbtn刷新.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn刷新.Image = (System.Drawing.Image)resources.GetObject("tsbtn刷新.Image");
            tsbtn刷新.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn刷新.Name = "tsbtn刷新";
            tsbtn刷新.Size = new System.Drawing.Size(36, 22);
            tsbtn刷新.Text = "刷新";
            tsbtn刷新.Click += tsbtn刷新_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 25);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(listView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnReverseSelected);
            splitContainer1.Panel2.Controls.Add(btnNotAllSelected);
            splitContainer1.Panel2.Controls.Add(btnSelectedAll);
            splitContainer1.Size = new System.Drawing.Size(911, 504);
            splitContainer1.SplitterDistance = 448;
            splitContainer1.TabIndex = 6;
            // 
            // btnReverseSelected
            // 
            btnReverseSelected.Location = new System.Drawing.Point(216, 14);
            btnReverseSelected.Name = "btnReverseSelected";
            btnReverseSelected.Size = new System.Drawing.Size(75, 26);
            btnReverseSelected.TabIndex = 8;
            btnReverseSelected.Text = "反选";
            btnReverseSelected.UseVisualStyleBackColor = true;
            btnReverseSelected.Click += btnReverseSelected_Click;
            // 
            // btnNotAllSelected
            // 
            btnNotAllSelected.Location = new System.Drawing.Point(117, 14);
            btnNotAllSelected.Name = "btnNotAllSelected";
            btnNotAllSelected.Size = new System.Drawing.Size(75, 26);
            btnNotAllSelected.TabIndex = 7;
            btnNotAllSelected.Text = "全不选";
            btnNotAllSelected.UseVisualStyleBackColor = true;
            btnNotAllSelected.Click += btnNotAllSelected_Click;
            // 
            // btnSelectedAll
            // 
            btnSelectedAll.Location = new System.Drawing.Point(20, 14);
            btnSelectedAll.Name = "btnSelectedAll";
            btnSelectedAll.Size = new System.Drawing.Size(75, 26);
            btnSelectedAll.TabIndex = 6;
            btnSelectedAll.Text = "全选";
            btnSelectedAll.UseVisualStyleBackColor = true;
            btnSelectedAll.Click += btnSelectedAll_Click;
            // 
            // frmUserList
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(911, 529);
            Controls.Add(splitContainer1);
            Controls.Add(toolStrip1);
            Name = "frmUserList";
            Text = "frmUserList";
            FormClosing += frmUserList_FormClosing;
            contextMenuStrip1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtn刷新;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 推送版本更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 切换服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部切换服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新全局配置ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnReverseSelected;
        private System.Windows.Forms.Button btnNotAllSelected;
        private System.Windows.Forms.Button btnSelectedAll;
    }
}