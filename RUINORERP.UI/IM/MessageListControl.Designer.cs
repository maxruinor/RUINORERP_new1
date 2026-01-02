using System;

namespace RUINORERP.UI.IM
{
    partial class MessageListControl
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
        /// 初始化组件（在设计器中自动生成）
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lstMessages = new System.Windows.Forms.ListView();
            this.colContent = new System.Windows.Forms.ColumnHeader();
            this.colTitle = new System.Windows.Forms.ColumnHeader();
            this.colTime = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStripMessages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuClear30Days = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear60Days = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear180Days = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnMarkAllRead = new System.Windows.Forms.Button();
            this.lblUnreadCount = new System.Windows.Forms.Label();
            this.contextMenuStripMessages.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            
            // lstMessages
            this.lstMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colContent,
            this.colTitle,
            this.colTime,
            this.colStatus,
            this.colType});
            this.lstMessages.ContextMenuStrip = this.contextMenuStripMessages;
            this.lstMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMessages.FullRowSelect = true;
            this.lstMessages.Location = new System.Drawing.Point(0, 40);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(800, 410);
            this.lstMessages.TabIndex = 0;
            this.lstMessages.UseCompatibleStateImageBehavior = false;
            this.lstMessages.View = System.Windows.Forms.View.Details;
            this.lstMessages.ItemActivate += new System.EventHandler(this.lstMessages_ItemClick);
            
            // colContent
            this.colContent.Text = "内容";
            this.colContent.Width = 300;
            
            // colTitle
            this.colTitle.Text = "标题";
            this.colTitle.Width = 150;
            
            // colTime
            this.colTime.Text = "时间";
            this.colTime.Width = 120;
            
            // colStatus
            this.colStatus.Text = "状态";
            this.colStatus.Width = 60;
            
            // colType
            this.colType.Text = "类型";
            this.colType.Width = 80;
            
            // contextMenuStripMessages
            this.contextMenuStripMessages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuClear30Days,
            this.menuClear60Days,
            this.menuClear180Days,
            this.toolStripSeparator1,
            this.menuClearAll});
            this.contextMenuStripMessages.Name = "contextMenuStripMessages";
            this.contextMenuStripMessages.Size = new System.Drawing.Size(181, 98);
            
            // menuClear30Days
            this.menuClear30Days.Name = "menuClear30Days";
            this.menuClear30Days.Size = new System.Drawing.Size(180, 22);
            this.menuClear30Days.Text = "清除30天前消息";
            this.menuClear30Days.Click += new System.EventHandler(this.menuClear30Days_Click);
            
            // menuClear60Days
            this.menuClear60Days.Name = "menuClear60Days";
            this.menuClear60Days.Size = new System.Drawing.Size(180, 22);
            this.menuClear60Days.Text = "清除60天前消息";
            this.menuClear60Days.Click += new System.EventHandler(this.menuClear60Days_Click);
            
            // menuClear180Days
            this.menuClear180Days.Name = "menuClear180Days";
            this.menuClear180Days.Size = new System.Drawing.Size(180, 22);
            this.menuClear180Days.Text = "清除180天前消息";
            this.menuClear180Days.Click += new System.EventHandler(this.menuClear180Days_Click);
            
            // toolStripSeparator1
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            
            // menuClearAll
            this.menuClearAll.Name = "menuClearAll";
            this.menuClearAll.Size = new System.Drawing.Size(180, 22);
            this.menuClearAll.Text = "清除所有消息";
            this.menuClearAll.Click += new System.EventHandler(this.menuClearAll_Click);
            
            // panelHeader
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.btnMarkAllRead);
            this.panelHeader.Controls.Add(this.lblUnreadCount);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(800, 40);
            this.panelHeader.TabIndex = 1;
            
            // btnRefresh
            this.btnRefresh.Location = new System.Drawing.Point(220, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            
            // btnMarkAllRead
            this.btnMarkAllRead.Location = new System.Drawing.Point(120, 10);
            this.btnMarkAllRead.Name = "btnMarkAllRead";
            this.btnMarkAllRead.Size = new System.Drawing.Size(90, 23);
            this.btnMarkAllRead.TabIndex = 1;
            this.btnMarkAllRead.Text = "全部标记已读";
            this.btnMarkAllRead.UseVisualStyleBackColor = true;
            this.btnMarkAllRead.Click += new System.EventHandler(this.btnbtnMarkAllRead_Click);
            
            // lblUnreadCount
            this.lblUnreadCount.AutoSize = true;
            this.lblUnreadCount.Location = new System.Drawing.Point(10, 15);
            this.lblUnreadCount.Name = "lblUnreadCount";
            this.lblUnreadCount.Size = new System.Drawing.Size(65, 12);
            this.lblUnreadCount.TabIndex = 0;
            this.lblUnreadCount.Text = "未读消息: 0";
            
            // MessageListControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstMessages);
            this.Controls.Add(this.panelHeader);
            this.Name = "MessageListControl";
            this.Size = new System.Drawing.Size(800, 450);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
        }


        #endregion

        private System.Windows.Forms.ListView lstMessages;
        private System.Windows.Forms.ColumnHeader colContent;
        private System.Windows.Forms.ColumnHeader colTitle;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMessages;
        private System.Windows.Forms.ToolStripMenuItem menuClear30Days;
        private System.Windows.Forms.ToolStripMenuItem menuClear60Days;
        private System.Windows.Forms.ToolStripMenuItem menuClear180Days;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuClearAll;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblUnreadCount;
        private System.Windows.Forms.Button btnMarkAllRead;
        private System.Windows.Forms.Button btnRefresh;
    }
}