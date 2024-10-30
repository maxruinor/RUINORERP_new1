namespace RUINORERP.Server
{
    partial class frmUserOnline
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
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            listViewForUser = new System.Windows.Forms.ListView();
            ch账号 = new System.Windows.Forms.ColumnHeader();
            ch姓名 = new System.Windows.Forms.ColumnHeader();
            ch所在模块 = new System.Windows.Forms.ColumnHeader();
            ch当前窗体 = new System.Windows.Forms.ColumnHeader();
            ch登陆时间 = new System.Windows.Forms.ColumnHeader();
            ch心跳数 = new System.Windows.Forms.ColumnHeader();
            ch心跳时间 = new System.Windows.Forms.ColumnHeader();
            chVer = new System.Windows.Forms.ColumnHeader();
            ch静止时间 = new System.Windows.Forms.ColumnHeader();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3, toolStripMenuItem4, toolStripMenuItem5, toolStripMenuItem6, 推送版本更新ToolStripMenuItem, 推送缓存数据ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(193, 180);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem1.Text = "断开连接";
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem2.Text = "强制用户退出";
            // 
            // toolStripMenuItem3
            // 
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            toolStripMenuItem3.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem3.Text = "toolStripMenuItem3";
            // 
            // toolStripMenuItem4
            // 
            toolStripMenuItem4.Name = "toolStripMenuItem4";
            toolStripMenuItem4.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem4.Text = "删除列配置文件";
            // 
            // toolStripMenuItem5
            // 
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            toolStripMenuItem5.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem5.Text = "发消息给客户端";
            // 
            // toolStripMenuItem6
            // 
            toolStripMenuItem6.Name = "toolStripMenuItem6";
            toolStripMenuItem6.Size = new System.Drawing.Size(192, 22);
            toolStripMenuItem6.Text = "关机";
            // 
            // 推送版本更新ToolStripMenuItem
            // 
            推送版本更新ToolStripMenuItem.Name = "推送版本更新ToolStripMenuItem";
            推送版本更新ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            推送版本更新ToolStripMenuItem.Text = "推送版本更新";
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            // 
            // listViewForUser
            // 
            listViewForUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { ch账号, ch姓名, ch所在模块, ch当前窗体, ch登陆时间, ch心跳数, ch心跳时间, chVer, ch静止时间 });
            listViewForUser.ContextMenuStrip = contextMenuStrip1;
            listViewForUser.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewForUser.Location = new System.Drawing.Point(0, 0);
            listViewForUser.Name = "listViewForUser";
            listViewForUser.Size = new System.Drawing.Size(890, 514);
            listViewForUser.TabIndex = 0;
            listViewForUser.UseCompatibleStateImageBehavior = false;
            listViewForUser.View = System.Windows.Forms.View.Details;
            // 
            // ch账号
            // 
            ch账号.Name = "ch账号";
            ch账号.Text = "账号";
            // 
            // ch姓名
            // 
            ch姓名.Name = "ch姓名";
            ch姓名.Text = "姓名";
            // 
            // ch所在模块
            // 
            ch所在模块.Name = "ch所在模块";
            ch所在模块.Text = "所在模块";
            ch所在模块.Width = 80;
            // 
            // ch当前窗体
            // 
            ch当前窗体.Name = "ch当前窗体";
            ch当前窗体.Text = "当前窗体";
            ch当前窗体.Width = 80;
            // 
            // ch登陆时间
            // 
            ch登陆时间.Text = "登陆时间";
            ch登陆时间.Width = 120;
            // 
            // ch心跳数
            // 
            ch心跳数.Name = "ch心跳数";
            ch心跳数.Text = "心跳数";
            // 
            // ch心跳时间
            // 
            ch心跳时间.Name = "ch心跳时间";
            ch心跳时间.Text = "心跳时间";
            ch心跳时间.Width = 120;
            // 
            // chVer
            // 
            chVer.Text = "版本";
            chVer.Width = 100;
            // 
            // ch静止时间
            // 
            ch静止时间.Name = "ch静止时间";
            ch静止时间.Text = "ch静止时间";
            ch静止时间.Width = 120;
            // 
            // frmUserOnline
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(890, 514);
            Controls.Add(listViewForUser);
            Name = "frmUserOnline";
            Text = "frmUserOnline";
            Load += frmUserOnline_Load;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.ListView listViewForUser;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ColumnHeader ch账号;
        private System.Windows.Forms.ColumnHeader ch姓名;
        private System.Windows.Forms.ColumnHeader ch所在模块;
        private System.Windows.Forms.ColumnHeader ch当前窗体;
        private System.Windows.Forms.ColumnHeader ch心跳数;
        private System.Windows.Forms.ColumnHeader ch心跳时间;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ColumnHeader ch登陆时间;
        private System.Windows.Forms.ColumnHeader ch静止时间;
        private System.Windows.Forms.ColumnHeader chVer;
        private System.Windows.Forms.ToolStripMenuItem 推送版本更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
    }
}