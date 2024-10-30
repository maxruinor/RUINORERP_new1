namespace RUINORERP.Server
{
    partial class frmUserManage
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
            dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            用户名DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            姓名DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            当前模块DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            当前窗体DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            登陆时间DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            心跳数DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            最后心跳时间DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            客户端版本DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            客户端IPDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            静止时间DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            userIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            onlineDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            serverAuthenticationDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            userInfoBindingSource = new System.Windows.Forms.BindingSource(components);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)userInfoBindingSource).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { dataGridViewTextBoxColumn1, 用户名DataGridViewTextBoxColumn, 姓名DataGridViewTextBoxColumn, 当前模块DataGridViewTextBoxColumn, 当前窗体DataGridViewTextBoxColumn, 登陆时间DataGridViewTextBoxColumn, 心跳数DataGridViewTextBoxColumn, 最后心跳时间DataGridViewTextBoxColumn, 客户端版本DataGridViewTextBoxColumn, 客户端IPDataGridViewTextBoxColumn, 静止时间DataGridViewTextBoxColumn, userIDDataGridViewTextBoxColumn, onlineDataGridViewCheckBoxColumn, serverAuthenticationDataGridViewCheckBoxColumn });
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.DataSource = userInfoBindingSource;
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1295, 450);
            dataGridView1.TabIndex = 0;
            dataGridView1.DataError += dataGridView1_DataError;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.DataPropertyName = "SessionId";
            dataGridViewTextBoxColumn1.HeaderText = "SessionId";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // 用户名DataGridViewTextBoxColumn
            // 
            用户名DataGridViewTextBoxColumn.DataPropertyName = "用户名";
            用户名DataGridViewTextBoxColumn.HeaderText = "用户名";
            用户名DataGridViewTextBoxColumn.Name = "用户名DataGridViewTextBoxColumn";
            // 
            // 姓名DataGridViewTextBoxColumn
            // 
            姓名DataGridViewTextBoxColumn.DataPropertyName = "姓名";
            姓名DataGridViewTextBoxColumn.HeaderText = "姓名";
            姓名DataGridViewTextBoxColumn.Name = "姓名DataGridViewTextBoxColumn";
            // 
            // 当前模块DataGridViewTextBoxColumn
            // 
            当前模块DataGridViewTextBoxColumn.DataPropertyName = "当前模块";
            当前模块DataGridViewTextBoxColumn.HeaderText = "当前模块";
            当前模块DataGridViewTextBoxColumn.Name = "当前模块DataGridViewTextBoxColumn";
            // 
            // 当前窗体DataGridViewTextBoxColumn
            // 
            当前窗体DataGridViewTextBoxColumn.DataPropertyName = "当前窗体";
            当前窗体DataGridViewTextBoxColumn.HeaderText = "当前窗体";
            当前窗体DataGridViewTextBoxColumn.Name = "当前窗体DataGridViewTextBoxColumn";
            // 
            // 登陆时间DataGridViewTextBoxColumn
            // 
            登陆时间DataGridViewTextBoxColumn.DataPropertyName = "登陆时间";
            登陆时间DataGridViewTextBoxColumn.HeaderText = "登陆时间";
            登陆时间DataGridViewTextBoxColumn.Name = "登陆时间DataGridViewTextBoxColumn";
            // 
            // 心跳数DataGridViewTextBoxColumn
            // 
            心跳数DataGridViewTextBoxColumn.DataPropertyName = "心跳数";
            心跳数DataGridViewTextBoxColumn.HeaderText = "心跳数";
            心跳数DataGridViewTextBoxColumn.Name = "心跳数DataGridViewTextBoxColumn";
            // 
            // 最后心跳时间DataGridViewTextBoxColumn
            // 
            最后心跳时间DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            最后心跳时间DataGridViewTextBoxColumn.DataPropertyName = "最后心跳时间";
            最后心跳时间DataGridViewTextBoxColumn.HeaderText = "最后心跳时间";
            最后心跳时间DataGridViewTextBoxColumn.Name = "最后心跳时间DataGridViewTextBoxColumn";
            // 
            // 客户端版本DataGridViewTextBoxColumn
            // 
            客户端版本DataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            客户端版本DataGridViewTextBoxColumn.DataPropertyName = "客户端版本";
            客户端版本DataGridViewTextBoxColumn.HeaderText = "客户端版本";
            客户端版本DataGridViewTextBoxColumn.Name = "客户端版本DataGridViewTextBoxColumn";
            // 
            // 客户端IPDataGridViewTextBoxColumn
            // 
            客户端IPDataGridViewTextBoxColumn.DataPropertyName = "客户端IP";
            客户端IPDataGridViewTextBoxColumn.HeaderText = "客户端IP";
            客户端IPDataGridViewTextBoxColumn.Name = "客户端IPDataGridViewTextBoxColumn";
            // 
            // 静止时间DataGridViewTextBoxColumn
            // 
            静止时间DataGridViewTextBoxColumn.DataPropertyName = "静止时间";
            静止时间DataGridViewTextBoxColumn.HeaderText = "静止时间";
            静止时间DataGridViewTextBoxColumn.Name = "静止时间DataGridViewTextBoxColumn";
            // 
            // userIDDataGridViewTextBoxColumn
            // 
            userIDDataGridViewTextBoxColumn.DataPropertyName = "UserID";
            userIDDataGridViewTextBoxColumn.HeaderText = "UserID";
            userIDDataGridViewTextBoxColumn.Name = "userIDDataGridViewTextBoxColumn";
            // 
            // onlineDataGridViewCheckBoxColumn
            // 
            onlineDataGridViewCheckBoxColumn.DataPropertyName = "Online";
            onlineDataGridViewCheckBoxColumn.HeaderText = "Online";
            onlineDataGridViewCheckBoxColumn.Name = "onlineDataGridViewCheckBoxColumn";
            // 
            // serverAuthenticationDataGridViewCheckBoxColumn
            // 
            serverAuthenticationDataGridViewCheckBoxColumn.DataPropertyName = "ServerAuthentication";
            serverAuthenticationDataGridViewCheckBoxColumn.HeaderText = "ServerAuthentication";
            serverAuthenticationDataGridViewCheckBoxColumn.Name = "serverAuthenticationDataGridViewCheckBoxColumn";
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
            // userInfoBindingSource
            // 
            userInfoBindingSource.DataSource = typeof(Model.CommonModel.UserInfo);
            // 
            // frmUserManage
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1295, 450);
            Controls.Add(dataGridView1);
            Name = "frmUserManage";
            Text = "用户管理";
            FormClosing += frmUserManage_FormClosing;
            Load += frmUserManage_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)userInfoBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.BindingSource userInfoBindingSource;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 推送版本更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn sessionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 用户名DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 姓名DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当前模块DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 当前窗体DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 登陆时间DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 心跳数DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最后心跳时间DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 客户端版本DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 客户端IPDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn 静止时间DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn onlineDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn serverAuthenticationDataGridViewCheckBoxColumn;
    }
}