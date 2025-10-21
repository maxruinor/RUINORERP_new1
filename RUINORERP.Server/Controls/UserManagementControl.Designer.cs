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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserManagementControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSelectedAll = new System.Windows.Forms.ToolStripButton();
            this.btnNotAllSelected = new System.Windows.Forms.ToolStripButton();
            this.btnReverseSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtn发送消息 = new System.Windows.Forms.ToolStripButton();
            this.tsbtn刷新 = new System.Windows.Forms.ToolStripButton();
            this.tsbtn推送更新 = new System.Windows.Forms.ToolStripButton();
            this.tsbtn推送系统配置 = new System.Windows.Forms.ToolStripButton();
            this.tsbtn推送缓存 = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.断开连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.强制用户退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除列配置文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发消息给客户端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.推送版本更新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新全局配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.推送缓存数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关机ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部切换服务器ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectedAll,
            this.btnNotAllSelected,
            this.btnReverseSelected,
            this.toolStripSeparator1,
            this.tsbtn发送消息,
            this.tsbtn刷新,
            this.tsbtn推送更新,
            this.tsbtn推送系统配置,
            this.tsbtn推送缓存});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSelectedAll
            // 
            this.btnSelectedAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelectedAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectedAll.Image")));
            this.btnSelectedAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectedAll.Name = "btnSelectedAll";
            this.btnSelectedAll.Size = new System.Drawing.Size(60, 22);
            this.btnSelectedAll.Text = "全部选择";
            this.btnSelectedAll.Click += new System.EventHandler(this.btnSelectedAll_Click);
            // 
            // btnNotAllSelected
            // 
            this.btnNotAllSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNotAllSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnNotAllSelected.Image")));
            this.btnNotAllSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNotAllSelected.Name = "btnNotAllSelected";
            this.btnNotAllSelected.Size = new System.Drawing.Size(60, 22);
            this.btnNotAllSelected.Text = "全部取消";
            this.btnNotAllSelected.Click += new System.EventHandler(this.btnNotAllSelected_Click);
            // 
            // btnReverseSelected
            // 
            this.btnReverseSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReverseSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnReverseSelected.Image")));
            this.btnReverseSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReverseSelected.Name = "btnReverseSelected";
            this.btnReverseSelected.Size = new System.Drawing.Size(60, 22);
            this.btnReverseSelected.Text = "反向选择";
            this.btnReverseSelected.Click += new System.EventHandler(this.btnReverseSelected_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtn发送消息
            // 
            this.tsbtn发送消息.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtn发送消息.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn发送消息.Image")));
            this.tsbtn发送消息.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn发送消息.Name = "tsbtn发送消息";
            this.tsbtn发送消息.Size = new System.Drawing.Size(60, 22);
            this.tsbtn发送消息.Text = "发送消息";
            this.tsbtn发送消息.Click += new System.EventHandler(this.tsbtn发送消息_Click);
            // 
            // tsbtn刷新
            // 
            this.tsbtn刷新.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtn刷新.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn刷新.Image")));
            this.tsbtn刷新.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn刷新.Name = "tsbtn刷新";
            this.tsbtn刷新.Size = new System.Drawing.Size(36, 22);
            this.tsbtn刷新.Text = "刷新";
            this.tsbtn刷新.Click += new System.EventHandler(this.tsbtn刷新_Click);
            // 
            // tsbtn推送更新
            // 
            this.tsbtn推送更新.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtn推送更新.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn推送更新.Image")));
            this.tsbtn推送更新.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn推送更新.Name = "tsbtn推送更新";
            this.tsbtn推送更新.Size = new System.Drawing.Size(60, 22);
            this.tsbtn推送更新.Text = "推送更新";
            this.tsbtn推送更新.Click += new System.EventHandler(this.tsbtn推送更新_Click);
            // 
            // tsbtn推送系统配置
            // 
            this.tsbtn推送系统配置.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtn推送系统配置.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn推送系统配置.Image")));
            this.tsbtn推送系统配置.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn推送系统配置.Name = "tsbtn推送系统配置";
            this.tsbtn推送系统配置.Size = new System.Drawing.Size(84, 22);
            this.tsbtn推送系统配置.Text = "推送系统配置";
            this.tsbtn推送系统配置.Click += new System.EventHandler(this.tsbtn推送系统配置_Click);
            // 
            // tsbtn推送缓存
            // 
            this.tsbtn推送缓存.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtn推送缓存.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn推送缓存.Image")));
            this.tsbtn推送缓存.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn推送缓存.Name = "tsbtn推送缓存";
            this.tsbtn推送缓存.Size = new System.Drawing.Size(60, 22);
            this.tsbtn推送缓存.Text = "推送缓存";
            this.tsbtn推送缓存.Click += new System.EventHandler(this.tsbtn推送缓存_Click);
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(800, 475);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.断开连接ToolStripMenuItem,
            this.强制用户退出ToolStripMenuItem,
            this.toolStripSeparator2,
            this.列显示选项ToolStripMenuItem,
            this.删除列配置文件ToolStripMenuItem,
            this.toolStripSeparator3,
            this.发消息给客户端ToolStripMenuItem,
            this.推送版本更新ToolStripMenuItem,
            this.更新全局配置ToolStripMenuItem,
            this.推送缓存数据ToolStripMenuItem,
            this.toolStripSeparator4,
            this.关机ToolStripMenuItem,
            this.切换服务器ToolStripMenuItem,
            this.全部切换服务器ToolStripMenuItem});
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(157, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(157, 6);
            // 
            // 列显示选项ToolStripMenuItem
            // 
            this.列显示选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.用户名列ToolStripMenuItem,
            this.姓名列ToolStripMenuItem,
            this.当前模块列ToolStripMenuItem,
            this.当前窗体列ToolStripMenuItem,
            this.登陆时间列ToolStripMenuItem,
            this.心跳数列ToolStripMenuItem,
            this.最后心跳时间列ToolStripMenuItem,
            this.客户端版本列ToolStripMenuItem,
            this.客户端IP列ToolStripMenuItem,
            this.静止时间列ToolStripMenuItem,
            this.超级用户列ToolStripMenuItem,
            this.在线状态列ToolStripMenuItem,
            this.授权状态列ToolStripMenuItem});
            this.列显示选项ToolStripMenuItem.Name = "列显示选项ToolStripMenuItem";
            this.列显示选项ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.列显示选项ToolStripMenuItem.Text = "列显示选项";
            // 
            // 用户名列ToolStripMenuItem
            // 
            this.用户名列ToolStripMenuItem.CheckOnClick = true;
            this.用户名列ToolStripMenuItem.Name = "用户名列ToolStripMenuItem";
            this.用户名列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.用户名列ToolStripMenuItem.Text = "用户名";
            this.用户名列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 姓名列ToolStripMenuItem
            // 
            this.姓名列ToolStripMenuItem.CheckOnClick = true;
            this.姓名列ToolStripMenuItem.Name = "姓名列ToolStripMenuItem";
            this.姓名列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.姓名列ToolStripMenuItem.Text = "姓名";
            this.姓名列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 当前模块列ToolStripMenuItem
            // 
            this.当前模块列ToolStripMenuItem.CheckOnClick = true;
            this.当前模块列ToolStripMenuItem.Name = "当前模块列ToolStripMenuItem";
            this.当前模块列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.当前模块列ToolStripMenuItem.Text = "当前模块";
            this.当前模块列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 当前窗体列ToolStripMenuItem
            // 
            this.当前窗体列ToolStripMenuItem.CheckOnClick = true;
            this.当前窗体列ToolStripMenuItem.Name = "当前窗体列ToolStripMenuItem";
            this.当前窗体列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.当前窗体列ToolStripMenuItem.Text = "当前窗体";
            this.当前窗体列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 登陆时间列ToolStripMenuItem
            // 
            this.登陆时间列ToolStripMenuItem.CheckOnClick = true;
            this.登陆时间列ToolStripMenuItem.Name = "登陆时间列ToolStripMenuItem";
            this.登陆时间列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.登陆时间列ToolStripMenuItem.Text = "登陆时间";
            this.登陆时间列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 心跳数列ToolStripMenuItem
            // 
            this.心跳数列ToolStripMenuItem.CheckOnClick = true;
            this.心跳数列ToolStripMenuItem.Name = "心跳数列ToolStripMenuItem";
            this.心跳数列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.心跳数列ToolStripMenuItem.Text = "心跳数";
            this.心跳数列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 最后心跳时间列ToolStripMenuItem
            // 
            this.最后心跳时间列ToolStripMenuItem.CheckOnClick = true;
            this.最后心跳时间列ToolStripMenuItem.Name = "最后心跳时间列ToolStripMenuItem";
            this.最后心跳时间列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.最后心跳时间列ToolStripMenuItem.Text = "最后心跳时间";
            this.最后心跳时间列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 客户端版本列ToolStripMenuItem
            // 
            this.客户端版本列ToolStripMenuItem.CheckOnClick = true;
            this.客户端版本列ToolStripMenuItem.Name = "客户端版本列ToolStripMenuItem";
            this.客户端版本列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.客户端版本列ToolStripMenuItem.Text = "客户端版本";
            this.客户端版本列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 客户端IP列ToolStripMenuItem
            // 
            this.客户端IP列ToolStripMenuItem.CheckOnClick = true;
            this.客户端IP列ToolStripMenuItem.Name = "客户端IP列ToolStripMenuItem";
            this.客户端IP列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.客户端IP列ToolStripMenuItem.Text = "客户端IP";
            this.客户端IP列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 静止时间列ToolStripMenuItem
            // 
            this.静止时间列ToolStripMenuItem.CheckOnClick = true;
            this.静止时间列ToolStripMenuItem.Name = "静止时间列ToolStripMenuItem";
            this.静止时间列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.静止时间列ToolStripMenuItem.Text = "静止时间";
            this.静止时间列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 超级用户列ToolStripMenuItem
            // 
            this.超级用户列ToolStripMenuItem.CheckOnClick = true;
            this.超级用户列ToolStripMenuItem.Name = "超级用户列ToolStripMenuItem";
            this.超级用户列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.超级用户列ToolStripMenuItem.Text = "超级用户";
            this.超级用户列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 在线状态列ToolStripMenuItem
            // 
            this.在线状态列ToolStripMenuItem.CheckOnClick = true;
            this.在线状态列ToolStripMenuItem.Name = "在线状态列ToolStripMenuItem";
            this.在线状态列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.在线状态列ToolStripMenuItem.Text = "在线状态";
            this.在线状态列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            // 
            // 授权状态列ToolStripMenuItem
            // 
            this.授权状态列ToolStripMenuItem.CheckOnClick = true;
            this.授权状态列ToolStripMenuItem.Name = "授权状态列ToolStripMenuItem";
            this.授权状态列ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.授权状态列ToolStripMenuItem.Text = "授权状态";
            this.授权状态列ToolStripMenuItem.CheckedChanged += new System.EventHandler(this.列显示选项_CheckedChanged);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(161, 224);
            // 
            // 断开连接ToolStripMenuItem
            // 
            this.断开连接ToolStripMenuItem.Name = "断开连接ToolStripMenuItem";
            this.断开连接ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.断开连接ToolStripMenuItem.Text = "断开连接";
            // 
            // 强制用户退出ToolStripMenuItem
            // 
            this.强制用户退出ToolStripMenuItem.Name = "强制用户退出ToolStripMenuItem";
            this.强制用户退出ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.强制用户退出ToolStripMenuItem.Text = "强制用户退出";
            // 
            // 删除列配置文件ToolStripMenuItem
            // 
            this.删除列配置文件ToolStripMenuItem.Name = "删除列配置文件ToolStripMenuItem";
            this.删除列配置文件ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.删除列配置文件ToolStripMenuItem.Text = "删除列配置文件";
            // 
            // 发消息给客户端ToolStripMenuItem
            // 
            this.发消息给客户端ToolStripMenuItem.Name = "发消息给客户端ToolStripMenuItem";
            this.发消息给客户端ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.发消息给客户端ToolStripMenuItem.Text = "发消息给客户端";
            // 
            // 推送版本更新ToolStripMenuItem
            // 
            this.推送版本更新ToolStripMenuItem.Name = "推送版本更新ToolStripMenuItem";
            this.推送版本更新ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.推送版本更新ToolStripMenuItem.Text = "推送版本更新";
            // 
            // 更新全局配置ToolStripMenuItem
            // 
            this.更新全局配置ToolStripMenuItem.Name = "更新全局配置ToolStripMenuItem";
            this.更新全局配置ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.更新全局配置ToolStripMenuItem.Text = "更新全局配置";
            // 
            // 推送缓存数据ToolStripMenuItem
            // 
            this.推送缓存数据ToolStripMenuItem.Name = "推送缓存数据ToolStripMenuItem";
            this.推送缓存数据ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.推送缓存数据ToolStripMenuItem.Text = "推送缓存数据";
            // 
            // 关机ToolStripMenuItem
            // 
            this.关机ToolStripMenuItem.Name = "关机ToolStripMenuItem";
            this.关机ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.关机ToolStripMenuItem.Text = "关机";
            // 
            // 切换服务器ToolStripMenuItem
            // 
            this.切换服务器ToolStripMenuItem.Name = "切换服务器ToolStripMenuItem";
            this.切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.切换服务器ToolStripMenuItem.Text = "切换服务器";
            // 
            // 全部切换服务器ToolStripMenuItem
            // 
            this.全部切换服务器ToolStripMenuItem.Name = "全部切换服务器ToolStripMenuItem";
            this.全部切换服务器ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.全部切换服务器ToolStripMenuItem.Text = "全部切换服务器";
            // 
            // UserManagementControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UserManagementControl";
            this.Size = new System.Drawing.Size(800, 500);
            this.Disposed += new System.EventHandler(this.UserManagementControl_Disposed);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}