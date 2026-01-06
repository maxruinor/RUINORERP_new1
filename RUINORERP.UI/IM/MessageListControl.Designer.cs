using Krypton.Toolkit;
using System;
using System.Windows.Forms;

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
            this.dgvMessages = new Krypton.Toolkit.KryptonDataGridView();
            this.colContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessageId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripMessages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRefreshMessages = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDeleteSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuClear30Days = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear60Days = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClear180Days = new System.Windows.Forms.ToolStripMenuItem();
            this.menuClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader = new Krypton.Toolkit.KryptonPanel();
            this.btnSettings = new Krypton.Toolkit.KryptonButton();
            this.btnRefresh = new Krypton.Toolkit.KryptonButton();
            this.btnMarkAllRead = new Krypton.Toolkit.KryptonButton();
            this.lblUnreadCount = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMessages)).BeginInit();
            this.contextMenuStripMessages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelHeader)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMessages
            // 
            this.dgvMessages.AllowUserToAddRows = false;
            this.dgvMessages.AllowUserToDeleteRows = false;
            this.dgvMessages.AllowUserToResizeRows = false;
            this.dgvMessages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvMessages.ColumnHeadersHeight = 30;
            this.dgvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContent,
            this.colTitle,
            this.colTime,
            this.colStatus,
            this.colType,
            this.colMessageId});
            this.dgvMessages.ContextMenuStrip = this.contextMenuStripMessages;
            this.dgvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMessages.Location = new System.Drawing.Point(0, 40);
            this.dgvMessages.MultiSelect = false;
            this.dgvMessages.Name = "dgvMessages";
            this.dgvMessages.ReadOnly = true;
            this.dgvMessages.RowHeadersVisible = false;
            this.dgvMessages.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMessages.Size = new System.Drawing.Size(800, 410);
            this.dgvMessages.TabIndex = 0;
            this.dgvMessages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMessages_CellDoubleClick);
            this.dgvMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvMessages_MouseDown);
            // 
            // colContent
            // 
            this.colContent.DataPropertyName = "Content";
            this.colContent.HeaderText = "消息内容";
            this.colContent.Name = "colContent";
            this.colContent.ReadOnly = true;
            this.colContent.Width = 145;
            this.colContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "标题";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colTime
            // 
            this.colTime.DataPropertyName = "SendTime";
            this.colTime.HeaderText = "发送时间";
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "IsRead";
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.DataPropertyName = "MessageType";
            this.colType.HeaderText = "消息类型";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colMessageId
            // 
            this.colMessageId.DataPropertyName = "MessageId";
            this.colMessageId.HeaderText = "MessageId";
            this.colMessageId.Name = "colMessageId";
            this.colMessageId.ReadOnly = true;
            this.colMessageId.Visible = false;
            // 
            // contextMenuStripMessages
            // 
            this.contextMenuStripMessages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRefreshMessages,
            this.toolStripSeparator1,
            this.menuDeleteSelected,
            this.toolStripSeparator2,
            this.menuClear30Days,
            this.menuClear60Days,
            this.menuClear180Days,
            this.menuClearAll});
            this.contextMenuStripMessages.Name = "contextMenuStripMessages";
            this.contextMenuStripMessages.Size = new System.Drawing.Size(170, 148);
            this.contextMenuStripMessages.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripMessages_Opening);
            // 
            // menuRefreshMessages
            // 
            this.menuRefreshMessages.Name = "menuRefreshMessages";
            this.menuRefreshMessages.Size = new System.Drawing.Size(169, 22);
            this.menuRefreshMessages.Text = "刷新消息";
            this.menuRefreshMessages.Click += new System.EventHandler(this.menuRefreshMessages_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
            // 
            // menuDeleteSelected
            // 
            this.menuDeleteSelected.Name = "menuDeleteSelected";
            this.menuDeleteSelected.Size = new System.Drawing.Size(169, 22);
            this.menuDeleteSelected.Text = "删除选中消息";
            this.menuDeleteSelected.Click += new System.EventHandler(this.menuDeleteSelected_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
            // 
            // menuClear30Days
            // 
            this.menuClear30Days.Name = "menuClear30Days";
            this.menuClear30Days.Size = new System.Drawing.Size(169, 22);
            this.menuClear30Days.Text = "清除30天前消息";
            this.menuClear30Days.Click += new System.EventHandler(this.menuClear30Days_Click);
            // 
            // menuClear60Days
            // 
            this.menuClear60Days.Name = "menuClear60Days";
            this.menuClear60Days.Size = new System.Drawing.Size(169, 22);
            this.menuClear60Days.Text = "清除60天前消息";
            this.menuClear60Days.Click += new System.EventHandler(this.menuClear60Days_Click);
            // 
            // menuClear180Days
            // 
            this.menuClear180Days.Name = "menuClear180Days";
            this.menuClear180Days.Size = new System.Drawing.Size(169, 22);
            this.menuClear180Days.Text = "清除180天前消息";
            this.menuClear180Days.Click += new System.EventHandler(this.menuClear180Days_Click);
            // 
            // menuClearAll
            // 
            this.menuClearAll.Name = "menuClearAll";
            this.menuClearAll.Size = new System.Drawing.Size(169, 22);
            this.menuClearAll.Text = "清除所有消息";
            this.menuClearAll.Click += new System.EventHandler(this.menuClearAll_Click);
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnSettings);
            this.panelHeader.Controls.Add(this.btnRefresh);
            this.panelHeader.Controls.Add(this.btnMarkAllRead);
            this.panelHeader.Controls.Add(this.lblUnreadCount);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.PanelAlternate;
            this.panelHeader.Size = new System.Drawing.Size(800, 40);
            this.panelHeader.TabIndex = 1;
            // 
            // btnSettings
            // 
            this.btnSettings.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
            this.btnSettings.Location = new System.Drawing.Point(210, 5);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(42, 30);
            this.btnSettings.TabIndex = 3;
            this.btnSettings.ToolTipValues.Description = "消息提醒设置";
            this.btnSettings.ToolTipValues.EnableToolTips = true;
            this.btnSettings.ToolTipValues.Heading = "";
            this.btnSettings.Values.Text = "设置";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
            this.btnRefresh.Location = new System.Drawing.Point(152, 5);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(42, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.ToolTipValues.Description = "刷新消息列表";
            this.btnRefresh.ToolTipValues.EnableToolTips = true;
            this.btnRefresh.ToolTipValues.Heading = "";
            this.btnRefresh.Values.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnMarkAllRead
            // 
            this.btnMarkAllRead.ButtonStyle = Krypton.Toolkit.ButtonStyle.Alternate;
            this.btnMarkAllRead.Location = new System.Drawing.Point(80, 5);
            this.btnMarkAllRead.Name = "btnMarkAllRead";
            this.btnMarkAllRead.Size = new System.Drawing.Size(66, 30);
            this.btnMarkAllRead.TabIndex = 1;
            this.btnMarkAllRead.ToolTipValues.Description = "标记所有消息为已读";
            this.btnMarkAllRead.ToolTipValues.EnableToolTips = true;
            this.btnMarkAllRead.ToolTipValues.Heading = "";
            this.btnMarkAllRead.Values.Text = "标记已读";
            this.btnMarkAllRead.Click += new System.EventHandler(this.btnbtnMarkAllRead_Click);
            // 
            // lblUnreadCount
            // 
            this.lblUnreadCount.AutoSize = false;
            this.lblUnreadCount.LabelStyle = Krypton.Toolkit.LabelStyle.NormalControl;
            this.lblUnreadCount.Location = new System.Drawing.Point(2, 5);
            this.lblUnreadCount.Name = "lblUnreadCount";
            this.lblUnreadCount.Size = new System.Drawing.Size(71, 30);
            this.lblUnreadCount.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.lblUnreadCount.StateCommon.ShortText.TextH = Krypton.Toolkit.PaletteRelativeAlign.Near;
            this.lblUnreadCount.StateCommon.ShortText.TextV = Krypton.Toolkit.PaletteRelativeAlign.Center;
            this.lblUnreadCount.TabIndex = 0;
            this.lblUnreadCount.Values.Text = "未读: 0";
            // 
            // MessageListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvMessages);
            this.Controls.Add(this.panelHeader);
            this.Name = "MessageListControl";
            this.Size = new System.Drawing.Size(800, 450);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMessages)).EndInit();
            this.contextMenuStripMessages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelHeader)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private Krypton.Toolkit.KryptonDataGridView dgvMessages;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessageId;
        private ContextMenuStrip contextMenuStripMessages;
        private System.Windows.Forms.ToolStripMenuItem menuRefreshMessages;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuDeleteSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuClear30Days;
        private System.Windows.Forms.ToolStripMenuItem menuClear60Days;
        private System.Windows.Forms.ToolStripMenuItem menuClear180Days;
        private System.Windows.Forms.ToolStripMenuItem menuClearAll;
        private Krypton.Toolkit.KryptonPanel panelHeader;
        private Krypton.Toolkit.KryptonLabel lblUnreadCount;
        private Krypton.Toolkit.KryptonButton btnMarkAllRead;
        private Krypton.Toolkit.KryptonButton btnRefresh;
        private Krypton.Toolkit.KryptonButton btnSettings;
    }
}