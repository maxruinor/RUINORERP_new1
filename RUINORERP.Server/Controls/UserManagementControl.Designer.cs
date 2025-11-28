namespace RUINORERP.Server.Controls
{
    partial class UserManagementControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            btnSelectedAll = new System.Windows.Forms.ToolStripButton();
            btnNotAllSelected = new System.Windows.Forms.ToolStripButton();
            btnReverseSelected = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsbtn发送消息 = new System.Windows.Forms.ToolStripButton();
            tsbtn刷新 = new System.Windows.Forms.ToolStripButton();
            tsbtn推送更新 = new System.Windows.Forms.ToolStripButton();
            tsbtn推送系统配置 = new System.Windows.Forms.ToolStripButton();
            tsbtn推送缓存 = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            lbl统计信息 = new System.Windows.Forms.ToolStripLabel();
            lbl在线用户数 = new System.Windows.Forms.ToolStripLabel();
            lbl总会话数 = new System.Windows.Forms.ToolStripLabel();
            lbl已认证用户数 = new System.Windows.Forms.ToolStripLabel();
            listView1 = new System.Windows.Forms.ListView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            断开连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            强制用户退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            列显示选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            用户名列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            姓名列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            当前模块列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            当前窗体列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            登陆时间列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            心跳数列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            最后心跳时间列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            客户端版本列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            客户端IP列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            静止时间列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            超级用户列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            在线状态列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            授权状态列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            删除列配置文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            发消息给客户端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            更新全局配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            关机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            全部切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            操作系统列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            机器名列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            CPU信息列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            内存大小列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnSelectedAll, btnNotAllSelected, btnReverseSelected, toolStripSeparator1, tsbtn发送消息, tsbtn刷新, tsbtn推送更新, tsbtn推送系统配置, tsbtn推送缓存, toolStripSeparator5, lbl统计信息, lbl在线用户数, lbl总会话数, lbl已认证用户数 });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new System.Drawing.Size(933, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // btnSelectedAll
            // 
            btnSelectedAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnSelectedAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSelectedAll.Name = "btnSelectedAll";
            btnSelectedAll.Size = new System.Drawing.Size(60, 22);
            btnSelectedAll.Text = "全部选择";
            btnSelectedAll.Click += btnSelectedAll_Click;
            // 
            // btnNotAllSelected
            // 
            btnNotAllSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnNotAllSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnNotAllSelected.Name = "btnNotAllSelected";
            btnNotAllSelected.Size = new System.Drawing.Size(60, 22);
            btnNotAllSelected.Text = "全部取消";
            btnNotAllSelected.Click += btnNotAllSelected_Click;
            // 
            // btnReverseSelected
            // 
            btnReverseSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            btnReverseSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnReverseSelected.Name = "btnReverseSelected";
            btnReverseSelected.Size = new System.Drawing.Size(60, 22);
            btnReverseSelected.Text = "反向选择";
            btnReverseSelected.Click += btnReverseSelected_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtn发送消息
            // 
            tsbtn发送消息.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn发送消息.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn发送消息.Name = "tsbtn发送消息";
            tsbtn发送消息.Size = new System.Drawing.Size(60, 22);
            tsbtn发送消息.Text = "发送消息";
            tsbtn发送消息.Click += tsbtn发送消息_Click;
            // 
            // tsbtn刷新
            // 
            tsbtn刷新.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn刷新.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn刷新.Name = "tsbtn刷新";
            tsbtn刷新.Size = new System.Drawing.Size(36, 22);
            tsbtn刷新.Text = "刷新";
            tsbtn刷新.Click += tsbtn刷新_Click;
            // 
            // tsbtn推送更新
            // 
            tsbtn推送更新.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn推送更新.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn推送更新.Name = "tsbtn推送更新";
            tsbtn推送更新.Size = new System.Drawing.Size(60, 22);
            tsbtn推送更新.Text = "推送更新";
            tsbtn推送更新.Click += tsbtn推送更新_Click;
            // 
            // tsbtn推送系统配置
            // 
            tsbtn推送系统配置.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn推送系统配置.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn推送系统配置.Name = "tsbtn推送系统配置";
            tsbtn推送系统配置.Size = new System.Drawing.Size(84, 22);
            tsbtn推送系统配置.Text = "推送系统配置";
            tsbtn推送系统配置.Click += tsbtn推送系统配置_Click;
            // 
            // tsbtn推送缓存
            // 
            tsbtn推送缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbtn推送缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbtn推送缓存.Name = "tsbtn推送缓存";
            tsbtn推送缓存.Size = new System.Drawing.Size(60, 22);
            tsbtn推送缓存.Text = "推送缓存";
            tsbtn推送缓存.Click += tsbtn推送缓存_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // lbl统计信息
            // 
            lbl统计信息.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
            lbl统计信息.Name = "lbl统计信息";
            lbl统计信息.Size = new System.Drawing.Size(59, 22);
            lbl统计信息.Text = "统计信息:";
            // 
            // lbl在线用户数
            // 
            lbl在线用户数.Name = "lbl在线用户数";
            lbl在线用户数.Size = new System.Drawing.Size(70, 22);
            lbl在线用户数.Text = "在线用户: 0";
            // 
            // lbl总会话数
            // 
            lbl总会话数.Name = "lbl总会话数";
            lbl总会话数.Size = new System.Drawing.Size(58, 22);
            lbl总会话数.Text = "总会话: 0";
            // 
            // lbl已认证用户数
            // 
            lbl已认证用户数.Name = "lbl已认证用户数";
            lbl已认证用户数.Size = new System.Drawing.Size(82, 22);
            lbl已认证用户数.Text = "已认证用户: 0";
            // 
            // listView1
            // 
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            listView1.FullRowSelect = true;
            listView1.Location = new System.Drawing.Point(0, 25);
            listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            listView1.Name = "listView1";
            listView1.Size = new System.Drawing.Size(933, 683);
            listView1.TabIndex = 1;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { 断开连接ToolStripMenuItem, 强制用户退出ToolStripMenuItem, toolStripSeparator2, 列显示选项ToolStripMenuItem, 删除列配置文件ToolStripMenuItem, toolStripSeparator3, 发消息给客户端ToolStripMenuItem, 推送版本更新ToolStripMenuItem, 更新全局配置ToolStripMenuItem, 推送缓存数据ToolStripMenuItem, toolStripSeparator4, 关机ToolStripMenuItem, 切换服务器ToolStripMenuItem, 全部切换服务器ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(181, 286);
            // 
            // 断开连接ToolStripMenuItem
            // 
            断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            断开连接ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            断开连接ToolStripMenuItem.Text = "断开连接";
            // 
            // 强制用户退出ToolStripMenuItem
            // 
            强制用户退出ToolStripMenuItem.Name = "强制用户退出ToolStripMenuItem";
            强制用户退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            强制用户退出ToolStripMenuItem.Text = "强制用户退出";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // 列显示选项ToolStripMenuItem
            // 
            列显示选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { 用户名列ToolStripMenuItem, 姓名列ToolStripMenuItem, 当前模块列ToolStripMenuItem, 当前窗体列ToolStripMenuItem, 登陆时间列ToolStripMenuItem, 心跳数列ToolStripMenuItem, 最后心跳时间列ToolStripMenuItem, 客户端版本列ToolStripMenuItem, 客户端IP列ToolStripMenuItem, 静止时间列ToolStripMenuItem, 超级用户列ToolStripMenuItem, 在线状态列ToolStripMenuItem, 授权状态列ToolStripMenuItem });
            列显示选项ToolStripMenuItem.Name = "列显示选项ToolStripMenuItem";
            列显示选项ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            列显示选项ToolStripMenuItem.Text = "列显示选项";
            // 
            // 用户名列ToolStripMenuItem
            // 
            用户名列ToolStripMenuItem.CheckOnClick = true;
            用户名列ToolStripMenuItem.Name = "用户名列ToolStripMenuItem";
            用户名列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            用户名列ToolStripMenuItem.Text = "用户名";
            用户名列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 姓名列ToolStripMenuItem
            // 
            姓名列ToolStripMenuItem.CheckOnClick = true;
            姓名列ToolStripMenuItem.Name = "姓名列ToolStripMenuItem";
            姓名列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            姓名列ToolStripMenuItem.Text = "姓名";
            姓名列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 当前模块列ToolStripMenuItem
            // 
            当前模块列ToolStripMenuItem.CheckOnClick = true;
            当前模块列ToolStripMenuItem.Name = "当前模块列ToolStripMenuItem";
            当前模块列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            当前模块列ToolStripMenuItem.Text = "当前模块";
            当前模块列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 当前窗体列ToolStripMenuItem
            // 
            当前窗体列ToolStripMenuItem.CheckOnClick = true;
            当前窗体列ToolStripMenuItem.Name = "当前窗体列ToolStripMenuItem";
            当前窗体列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            当前窗体列ToolStripMenuItem.Text = "当前窗体";
            当前窗体列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 登陆时间列ToolStripMenuItem
            // 
            登陆时间列ToolStripMenuItem.CheckOnClick = true;
            登陆时间列ToolStripMenuItem.Name = "登陆时间列ToolStripMenuItem";
            登陆时间列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            登陆时间列ToolStripMenuItem.Text = "登陆时间";
            登陆时间列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 心跳数列ToolStripMenuItem
            // 
            心跳数列ToolStripMenuItem.CheckOnClick = true;
            心跳数列ToolStripMenuItem.Name = "心跳数列ToolStripMenuItem";
            心跳数列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            心跳数列ToolStripMenuItem.Text = "心跳数";
            心跳数列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 最后心跳时间列ToolStripMenuItem
            // 
            最后心跳时间列ToolStripMenuItem.CheckOnClick = true;
            最后心跳时间列ToolStripMenuItem.Name = "最后心跳时间列ToolStripMenuItem";
            最后心跳时间列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            最后心跳时间列ToolStripMenuItem.Text = "最后心跳时间";
            最后心跳时间列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 客户端版本列ToolStripMenuItem
            // 
            客户端版本列ToolStripMenuItem.CheckOnClick = true;
            客户端版本列ToolStripMenuItem.Name = "客户端版本列ToolStripMenuItem";
            客户端版本列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            客户端版本列ToolStripMenuItem.Text = "客户端版本";
            客户端版本列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 客户端IP列ToolStripMenuItem
            // 
            客户端IP列ToolStripMenuItem.CheckOnClick = true;
            客户端IP列ToolStripMenuItem.Name = "客户端IP列ToolStripMenuItem";
            客户端IP列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            客户端IP列ToolStripMenuItem.Text = "客户端IP";
            客户端IP列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 静止时间列ToolStripMenuItem
            // 
            静止时间列ToolStripMenuItem.CheckOnClick = true;
            静止时间列ToolStripMenuItem.Name = "静止时间列ToolStripMenuItem";
            静止时间列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            静止时间列ToolStripMenuItem.Text = "静止时间";
            静止时间列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 超级用户列ToolStripMenuItem
            // 
            超级用户列ToolStripMenuItem.CheckOnClick = true;
            超级用户列ToolStripMenuItem.Name = "超级用户列ToolStripMenuItem";
            超级用户列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            超级用户列ToolStripMenuItem.Text = "超级用户";
            超级用户列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 在线状态列ToolStripMenuItem
            // 
            在线状态列ToolStripMenuItem.CheckOnClick = true;
            在线状态列ToolStripMenuItem.Name = "在线状态列ToolStripMenuItem";
            在线状态列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            在线状态列ToolStripMenuItem.Text = "在线状态";
            在线状态列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 授权状态列ToolStripMenuItem
            // 
            授权状态列ToolStripMenuItem.CheckOnClick = true;
            授权状态列ToolStripMenuItem.Name = "授权状态列ToolStripMenuItem";
            授权状态列ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            授权状态列ToolStripMenuItem.Text = "授权状态";
            授权状态列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 删除列配置文件ToolStripMenuItem
            // 
            删除列配置文件ToolStripMenuItem.Name = "删除列配置文件ToolStripMenuItem";
            删除列配置文件ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            删除列配置文件ToolStripMenuItem.Text = "删除列配置文件";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // 发消息给客户端ToolStripMenuItem
            // 
            发消息给客户端ToolStripMenuItem.Name = "发消息给客户端ToolStripMenuItem";
            发消息给客户端ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            发消息给客户端ToolStripMenuItem.Text = "发消息给客户端";
            发消息给客户端ToolStripMenuItem.Click += 发消息给客户端ToolStripMenuItem_Click;
            // 
            // 推送版本更新ToolStripMenuItem
            // 
            推送版本更新ToolStripMenuItem.Name = "推送版本更新ToolStripMenuItem";
            推送版本更新ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            推送版本更新ToolStripMenuItem.Text = "推送版本更新";
            // 
            // 更新全局配置ToolStripMenuItem
            // 
            更新全局配置ToolStripMenuItem.Name = "更新全局配置ToolStripMenuItem";
            更新全局配置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            更新全局配置ToolStripMenuItem.Text = "更新全局配置";
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // 关机ToolStripMenuItem
            // 
            关机ToolStripMenuItem.Name = "关机ToolStripMenuItem";
            关机ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            关机ToolStripMenuItem.Text = "关机";
            // 
            // 切换服务器ToolStripMenuItem
            // 
            切换服务器ToolStripMenuItem.Name = "切换服务器ToolStripMenuItem";
            切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            切换服务器ToolStripMenuItem.Text = "切换服务器";
            // 
            // 全部切换服务器ToolStripMenuItem
            // 
            全部切换服务器ToolStripMenuItem.Name = "全部切换服务器ToolStripMenuItem";
            全部切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            全部切换服务器ToolStripMenuItem.Text = "全部切换服务器";
            // 
            // 操作系统列ToolStripMenuItem
            // 
            操作系统列ToolStripMenuItem.CheckOnClick = true;
            操作系统列ToolStripMenuItem.Name = "操作系统列ToolStripMenuItem";
            操作系统列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            操作系统列ToolStripMenuItem.Text = "操作系统";
            操作系统列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 机器名列ToolStripMenuItem
            // 
            机器名列ToolStripMenuItem.CheckOnClick = true;
            机器名列ToolStripMenuItem.Name = "机器名列ToolStripMenuItem";
            机器名列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            机器名列ToolStripMenuItem.Text = "机器名";
            机器名列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // CPU信息列ToolStripMenuItem
            // 
            CPU信息列ToolStripMenuItem.CheckOnClick = true;
            CPU信息列ToolStripMenuItem.Name = "CPU信息列ToolStripMenuItem";
            CPU信息列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            CPU信息列ToolStripMenuItem.Text = "CPU信息";
            CPU信息列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // 内存大小列ToolStripMenuItem
            // 
            内存大小列ToolStripMenuItem.CheckOnClick = true;
            内存大小列ToolStripMenuItem.Name = "内存大小列ToolStripMenuItem";
            内存大小列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            内存大小列ToolStripMenuItem.Text = "内存大小";
            内存大小列ToolStripMenuItem.CheckedChanged += 列显示选项_CheckedChanged;
            // 
            // UserManagementControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(listView1);
            Controls.Add(toolStrip1);
            Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            Name = "UserManagementControl";
            Size = new System.Drawing.Size(933, 708);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSelectedAll;
        private System.Windows.Forms.ToolStripButton btnNotAllSelected;
        private System.Windows.Forms.ToolStripButton btnReverseSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtn发送消息;
        private System.Windows.Forms.ToolStripButton tsbtn刷新;
        private System.Windows.Forms.ToolStripButton tsbtn推送更新;
        private System.Windows.Forms.ToolStripButton tsbtn推送系统配置;
        private System.Windows.Forms.ToolStripButton tsbtn推送缓存;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 断开连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 强制用户退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除列配置文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发消息给客户端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送版本更新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新全局配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 推送缓存数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关机ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 列显示选项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户名列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 姓名列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 当前模块列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 当前窗体列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 登陆时间列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 心跳数列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最后心跳时间列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户端版本列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 客户端IP列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 静止时间列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 超级用户列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 在线状态列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 授权状态列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作系统列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机器名列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CPU信息列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 内存大小列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 切换服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部切换服务器ToolStripMenuItem;

        // 会话统计标签
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel lbl统计信息;
        private System.Windows.Forms.ToolStripLabel lbl在线用户数;
        private System.Windows.Forms.ToolStripLabel lbl总会话数;
        private System.Windows.Forms.ToolStripLabel lbl已认证用户数;
    }
}