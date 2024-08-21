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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewForUser = new System.Windows.Forms.ListView();
            this.ch账号 = new System.Windows.Forms.ColumnHeader();
            this.ch姓名 = new System.Windows.Forms.ColumnHeader();
            this.ch所在模块 = new System.Windows.Forms.ColumnHeader();
            this.ch当前窗体 = new System.Windows.Forms.ColumnHeader();
            this.ch登陆时间 = new System.Windows.Forms.ColumnHeader();
            this.ch心跳数 = new System.Windows.Forms.ColumnHeader();
            this.ch心跳时间 = new System.Windows.Forms.ColumnHeader();
            this.chVer = new System.Windows.Forms.ColumnHeader();
            this.ch静止时间 = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.推送版本更新ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 158);
            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem1.Text = "断开连接";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem2.Text = "强制用户退出";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem3.Text = "toolStripMenuItem3";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem4.Text = "删除列配置文件";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem5.Text = "发消息给客户端";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem6.Text = "关机";
            // 
            // 推送版本更新ToolStripMenuItem
            // 
            this.推送版本更新ToolStripMenuItem.Name = "推送版本更新ToolStripMenuItem";
            this.推送版本更新ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.推送版本更新ToolStripMenuItem.Text = "推送版本更新";
            // 
            // listViewForUser
            // 
            this.listViewForUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch账号,
            this.ch姓名,
            this.ch所在模块,
            this.ch当前窗体,
            this.ch登陆时间,
            this.ch心跳数,
            this.ch心跳时间,
            this.chVer,
            this.ch静止时间});
            this.listViewForUser.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewForUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewForUser.HideSelection = false;
            this.listViewForUser.Location = new System.Drawing.Point(0, 0);
            this.listViewForUser.Name = "listViewForUser";
            this.listViewForUser.Size = new System.Drawing.Size(800, 450);
            this.listViewForUser.TabIndex = 0;
            this.listViewForUser.UseCompatibleStateImageBehavior = false;
            this.listViewForUser.View = System.Windows.Forms.View.Details;
            // 
            // ch账号
            // 
            this.ch账号.Name = "ch账号";
            this.ch账号.Text = "账号";
            // 
            // ch姓名
            // 
            this.ch姓名.Name = "ch姓名";
            this.ch姓名.Text = "姓名";
            // 
            // ch所在模块
            // 
            this.ch所在模块.Name = "ch所在模块";
            this.ch所在模块.Text = "所在模块";
            // 
            // ch当前窗体
            // 
            this.ch当前窗体.Name = "ch当前窗体";
            this.ch当前窗体.Text = "当前窗体";
            // 
            // ch登陆时间
            // 
            this.ch登陆时间.Text = "登陆时间";
            this.ch登陆时间.Width = 120;
            // 
            // ch心跳数
            // 
            this.ch心跳数.Name = "ch心跳数";
            this.ch心跳数.Text = "心跳数";
            // 
            // ch心跳时间
            // 
            this.ch心跳时间.Name = "ch心跳时间";
            this.ch心跳时间.Text = "心跳时间";
            this.ch心跳时间.Width = 120;
            // 
            // chVer
            // 
            this.chVer.Text = "版本";
            this.chVer.Width = 100;
            // 
            // ch静止时间
            // 
            this.ch静止时间.Name = "ch静止时间";
            this.ch静止时间.Text = "ch静止时间";
            this.ch静止时间.Width = 120;
            // 
            // frmUserOnline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listViewForUser);
            this.Name = "frmUserOnline";
            this.Text = "frmUserOnline";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}