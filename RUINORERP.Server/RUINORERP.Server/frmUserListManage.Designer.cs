namespace RUINORERP.Server
{
    partial class frmUserListManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserListManage));
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            listView1 = new System.Windows.Forms.ListView();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            tsbtn刷新 = new System.Windows.Forms.ToolStripButton();
            contextMenuStrip1.SuspendLayout();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6, 推送版本更新ToolStripMenuItem, 推送缓存数据ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(161, 158);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
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
            // listView1
            // 
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(0, 0);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(1295, 450);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            listView1.VirtualMode = true;
            listView1.MouseClick += listView1_MouseClick;
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsbtn刷新 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(1295, 25);
            toolStrip1.TabIndex = 2;
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
            // frmUserListManage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1295, 450);
            Controls.Add(toolStrip1);
            Controls.Add(listView1);
            Name = "frmUserListManage";
            Text = "用户管理";
            FormClosing += frmUserManage_FormClosing;
            Load += frmUserManage_Load;
            contextMenuStrip1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 推送版本更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn onlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn serverAuthenticationDataGridViewCheckBoxColumn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbtn刷新;
    }
}