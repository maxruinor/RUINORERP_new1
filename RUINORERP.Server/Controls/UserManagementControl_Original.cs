using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ToolsUI;
using SuperSocket.Server.Abstractions;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model.Base;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.Services;
using Microsoft.Extensions.Logging;
using System.Threading;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.Server.Controls;

namespace RUINORERP.Server.Controls
{
    /// <summary>
    /// 在线用户管理控件 - 重构版
    /// 基于SessionID进行用户管理，直接使用ISessionService
    /// 移除了冗余的ObservableCollection，简化了数据管理逻辑
    /// </summary>
    public partial class UserManagementControl : UserControl
    {
        #region 字段和属性

        private readonly ISessionService _sessionService;
        private readonly System.Windows.Forms.Timer _updateTimer;
        private readonly ServerMessageService _serverMessageService;
        
        // ListView项映射字典，以SessionID为键
        private readonly Dictionary<string, ListViewItem> _sessionItemMap = new Dictionary<string, ListViewItem>();
        
        // 上次完整刷新时间
        private DateTime _lastFullUpdate = DateTime.MinValue;
        
        // 是否需要完整刷新标志
        private bool _needsFullRefresh = false;

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        private const int SB_HORZ = 0;

        // 菜单项引用（与设计器关联）
        private System.Windows.Forms.ToolStripMenuItem 切换服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部切换服务器ToolStripMenuItem;

        #endregion

        public UserManagementControl()
        {
            InitializeComponent();
            InitializeListView();

            // 获取服务实例
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _serverMessageService = Program.ServiceProvider.GetRequiredService<ServerMessageService>();

            // 订阅会话服务事件
            _sessionService.SessionConnected += OnSessionConnected;
            _sessionService.SessionDisconnected += OnSessionDisconnected;
            _sessionService.SessionUpdated += OnSessionUpdated;

            // 初始化时加载所有现有会话
            LoadAllSessions();

            // 设置定时器用于UI刷新
            _updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;

            // 初始化列显示选项为选中状态
            InitializeColumnDisplayOptions();
        }

        private void InitializeListView()
        {
            // 启用双缓冲减少闪烁
            listView1.DoubleBuffered(true);
            listView1.VirtualMode = false;
            // 配置 ListView
            listView1.CheckBoxes = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            AddCols();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 加载所有现有会话
        /// </summary>
        private void LoadAllSessions()
        {
            try
            {
                var sessions = _sessionService.GetAllUserSessions().ToList();
                int loadedCount = 0;

                foreach (var session in sessions)
                {
                    AddOrUpdateSessionItem(session);
                    loadedCount++;
                }

                // 记录加载结果
                LogStatusChange(null, $"初始加载完成：加载 {loadedCount} 个会话");
                
                // 初始化统计信息
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                LogError($"加载现有会话时出错: {ex.Message}", ex);
            }
        }

        #endregion

        #region 列显示管理

        private void AddCols()
        {
            listView1.Columns.Clear();
            // 调整第一列宽度，为复选框留出空间
            listView1.Columns.Add("", 30); // 复选框列
            //listView1.Columns.Add("员工ID", 80);          // Employee_ID
            //listView1.Columns.Add("SessionId", 150);     // SessionId
            listView1.Columns.Add("用户名", 100);         // 用户名
            listView1.Columns.Add("姓名", 100);           // 姓名
            listView1.Columns.Add("当前模块", 120);       // 当前模块
            listView1.Columns.Add("当前窗体", 120);       // 当前窗体
            listView1.Columns.Add("登陆时间", 150);       // 登陆时间
            listView1.Columns.Add("心跳数", 80);          // 心跳数
            listView1.Columns.Add("最后心跳时间", 150);   // 最后心跳时间
            listView1.Columns.Add("客户端版本", 300);     // 客户端版本
            listView1.Columns.Add("客户端IP", 120);       // 客户端IP
            listView1.Columns.Add("静止时间", 80);        // 静止时间
            //listView1.Columns.Add("用户ID", 80);          // UserID
            listView1.Columns.Add("超级用户", 80);        // 超级用户
            listView1.Columns.Add("在线状态", 80);        // 在线状态
            listView1.Columns.Add("授权状态", 80);        // 授权状态
            listView1.Columns.Add("操作系统", 150);       // 操作系统
            listView1.Columns.Add("机器名", 100);         // 机器名
            listView1.Columns.Add("CPU信息", 150);        // CPU信息
            listView1.Columns.Add("内存大小", 100);       // 内存大小
        }

        #region 列显示选项

        private void InitializeColumnDisplayOptions()
        {
            // 初始化所有列显示选项为选中状态
            用户名列ToolStripMenuItem.Checked = true;
            姓名列ToolStripMenuItem.Checked = true;
            当前模块列ToolStripMenuItem.Checked = true;
            当前窗体列ToolStripMenuItem.Checked = true;
            登陆时间列ToolStripMenuItem.Checked = true;
            心跳数列ToolStripMenuItem.Checked = true;
            最后心跳时间列ToolStripMenuItem.Checked = true;
            客户端版本列ToolStripMenuItem.Checked = true;
            客户端IP列ToolStripMenuItem.Checked = true;
            静止时间列ToolStripMenuItem.Checked = true;
            超级用户列ToolStripMenuItem.Checked = true;
            在线状态列ToolStripMenuItem.Checked = true;
            授权状态列ToolStripMenuItem.Checked = true;
            操作系统列ToolStripMenuItem.Checked = true;
            机器名列ToolStripMenuItem.Checked = true;
            CPU信息列ToolStripMenuItem.Checked = true;
            内存大小列ToolStripMenuItem.Checked = true;
        }

        private void 列显示选项_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is ToolStripMenuItem menuItem)
                {
                    // 获取列名并找到对应的列索引
                    string columnName = menuItem.Text;
                    int columnIndex = FindColumnIndexByName(columnName) + 1; // +1 因为第一列是复选框

                    if (columnIndex > 0 && columnIndex < listView1.Columns.Count)
                    {
                        // 设置列的可见性
                        listView1.Columns[columnIndex].Width = menuItem.Checked ? listView1.Columns[columnIndex].Width : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("设置列显示选项时出错: " + ex.Message);
            }
        }

        private int FindColumnIndexByName(string columnName)
        {
            // 跳过第一列（复选框列）
            for (int i = 1; i < listView1.Columns.Count; i++)
            {
                if (listView1.Columns[i].Text == columnName)
                {
                    return i - 1; // 返回相对于数据列的索引（不包括复选框列）
                }
            }
            return -1;
        }

        #endregion

        #region 事件处理

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var hitTest = listView1.HitTest(e.Location);
                if (hitTest.Item != null && hitTest.Item.Tag is SessionInfo sessionInfo)
                {
                    // 创建并显示会话管理详情窗体
                    var sessionManagementForm = new SessionManagementForm(sessionInfo, _sessionService);
                    sessionManagementForm.Show();
                }
            }
            catch (Exception ex)
            {
                LogError("显示会话性能详情时出错", ex);
            }
        }

        #region 列头右键菜单实现

        private void SelectAllItems()
        {
            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
            listView1.EndUpdate();
        }

        private void InvertSelection()
        {
            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = !item.Checked;
            }
            listView1.EndUpdate();
        }

        private void DeselectAllItems()
        {
            listView1.BeginUpdate();
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
            listView1.EndUpdate();
        }

        #endregion

        #region 会话数据管理

        /// <summary>
        /// 添加或更新会话项 - 基于SessionID进行管理
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        private void AddOrUpdateSessionItem(SessionInfo sessionInfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddOrUpdateSessionItem(sessionInfo)));
                return;
            }

            // 以SessionID为唯一标识
            if (string.IsNullOrEmpty(sessionInfo.SessionID))
            {
                LogError("会话信息SessionID为空，跳过添加或更新");
                return;
            }

            if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
            {
                // 会话已存在，更新现有项
                UpdateSessionItem(existingItem, sessionInfo);
            }
            else
            {
                // 新会话，创建新项
                var newItem = CreateSessionItem(sessionInfo);
                listView1.Items.Add(newItem);
                _sessionItemMap[sessionInfo.SessionID] = newItem;
            }
        }

        /// <summary>
        /// 移除会话项
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        private void RemoveSessionItem(string sessionId)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => RemoveSessionItem(sessionId)));
                return;
            }

            if (_sessionItemMap.TryGetValue(sessionId, out var item))
            {
                listView1.Items.Remove(item);
                _sessionItemMap.Remove(sessionId);
            }
        }

        /// <summary>
        /// 创建会话列表项
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>ListView项</returns>
        private ListViewItem CreateSessionItem(SessionInfo sessionInfo)
        {
            var item = new ListViewItem
            {
                Tag = sessionInfo,
                Text = "", // 第一列（复选框列）
            };

            item.ToolTipText = $"SessionID: {sessionInfo.SessionID}\n用户名: {sessionInfo.UserName}\n客户端IP: {sessionInfo.ClientIp}";
            
            // 添加各列数据
            item.SubItems.Add(GetDisplayName(sessionInfo.UserName)); // 用户名
            item.SubItems.Add(sessionInfo.DisplayName ?? ""); // 姓名
            item.SubItems.Add(sessionInfo.CurrentModule ?? ""); // 当前模块
            item.SubItems.Add(sessionInfo.CurrentForm ?? ""); // 当前窗体
            item.SubItems.Add(sessionInfo.ConnectTime.ToString("yy-MM-dd HH:mm:ss")); // 连接时间
            item.SubItems.Add(sessionInfo.HeartbeatCount.ToString()); // 心跳数
            item.SubItems.Add(sessionInfo.LastHeartbeatTime.ToString("yy-MM-dd HH:mm:ss")); // 最后心跳时间
            item.SubItems.Add(sessionInfo.ClientVersion ?? ""); // 客户端版本
            item.SubItems.Add(sessionInfo.ClientIp); // 客户端IP
            item.SubItems.Add(FormatIdleTime(DateTime.Now - sessionInfo.LastHeartbeatTime)); // 静止时间
            item.SubItems.Add(sessionInfo.IsSuperUser ? "是" : "否"); // 超级用户
            item.SubItems.Add(sessionInfo.IsConnected ? "在线" : "离线"); // 在线状态
            item.SubItems.Add(sessionInfo.IsAuthenticated ? "已授权" : "未授权"); // 授权状态
            item.SubItems.Add(sessionInfo.OperatingSystem ?? ""); // 操作系统
            item.SubItems.Add(sessionInfo.MachineName ?? ""); // 机器名
            item.SubItems.Add(sessionInfo.CpuInfo ?? ""); // CPU信息
            item.SubItems.Add(sessionInfo.MemoryInfo ?? ""); // 内存大小

            SetSessionItemStyle(item, sessionInfo);
            return item;
        }

        /// <summary>
        /// 更新会话列表项
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="sessionInfo">会话信息</param>
        private void UpdateSessionItem(ListViewItem item, SessionInfo sessionInfo)
        {
            if (item.SubItems.Count < 18) return;

            // 更新所有列的数据
            item.SubItems[1].Text = GetDisplayName(sessionInfo.UserName);
            item.SubItems[2].Text = sessionInfo.DisplayName ?? "";
            item.SubItems[3].Text = sessionInfo.CurrentModule ?? "";
            item.SubItems[4].Text = sessionInfo.CurrentForm ?? "";
            item.SubItems[5].Text = sessionInfo.ConnectTime.ToString("yy-MM-dd HH:mm:ss");
            item.SubItems[6].Text = sessionInfo.HeartbeatCount.ToString();
            item.SubItems[7].Text = sessionInfo.LastHeartbeatTime.ToString("yy-MM-dd HH:mm:ss");
            item.SubItems[8].Text = sessionInfo.ClientVersion ?? "";
            item.SubItems[9].Text = sessionInfo.ClientIp;
            item.SubItems[10].Text = FormatIdleTime(DateTime.Now - sessionInfo.LastHeartbeatTime);
            item.SubItems[11].Text = sessionInfo.IsSuperUser ? "是" : "否";
            item.SubItems[12].Text = sessionInfo.IsConnected ? "在线" : "离线";
            item.SubItems[13].Text = sessionInfo.IsAuthenticated ? "已授权" : "未授权";
            item.SubItems[14].Text = sessionInfo.OperatingSystem ?? "";
            item.SubItems[15].Text = sessionInfo.MachineName ?? "";
            item.SubItems[16].Text = sessionInfo.CpuInfo ?? "";
            item.SubItems[17].Text = sessionInfo.MemoryInfo ?? "";

            SetSessionItemStyle(item, sessionInfo);
        }

        /// <summary>
        /// 设置会话项的视觉样式
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="sessionInfo">会话信息</param>
        private void SetSessionItemStyle(ListViewItem item, SessionInfo sessionInfo)
        {
            try
            {
                // 状态判断逻辑
                if (!sessionInfo.IsConnected)
                {
                    // 离线状态：灰色文字，浅灰色背景
                    item.ForeColor = Color.Gray;
                    item.BackColor = Color.LightGray;
                    item.Font = new Font(item.Font, FontStyle.Regular);
                }
                else if (sessionInfo.IsConnected && !sessionInfo.IsAuthenticated)
                {
                    // 在线但未授权：橙色文字，浅黄色背景，加粗显示
                    item.ForeColor = Color.DarkOrange;
                    item.BackColor = Color.LightYellow;
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
                else if (sessionInfo.IsConnected && sessionInfo.IsAuthenticated)
                {
                    // 在线且已授权：绿色文字，白色背景
                    item.ForeColor = Color.Green;
                    item.BackColor = Color.White;
                    item.Font = new Font(item.Font, FontStyle.Regular);
                }
                else
                {
                    // 未知状态：默认样式
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.White;
                    item.Font = new Font(item.Font, FontStyle.Regular);
                }

                // 超级用户特殊标识
                if (sessionInfo.IsSuperUser)
                {
                    if (item.SubItems.Count > 0)
                    {
                        item.SubItems[0].BackColor = Color.LightBlue;
                    }
                }

                // 心跳异常检测
                var timeSinceHeartbeat = DateTime.Now - sessionInfo.LastHeartbeatTime;
                if (timeSinceHeartbeat.TotalMinutes > 5 && sessionInfo.IsConnected)
                {
                    item.ForeColor = Color.Red;
                    item.BackColor = Color.MistyRose;
                    item.ToolTipText = $"心跳异常 - 超过{timeSinceHeartbeat.TotalMinutes:F0}分钟无响应\n{item.ToolTipText}";
                }
            }
            catch (Exception ex)
            {
                LogError($"设置会话项样式时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取显示用户名
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>显示用户名</returns>
        private string GetDisplayName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return "未认证用户";
            }
            return userName;
        }

        #endregion

        #endregion

        #endregion

        #region 定时器和刷新逻辑

        /// <summary>
        /// 定时器刷新 - 简化版：基于事件驱动和必要轮询
        /// </summary>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var now = DateTime.Now;
                
                // 1. 统计信息：每5秒更新一次
                if (now.Second % 5 == 0)
                {
                    UpdateStatistics();
                }

                // 2. 心跳和空闲时间更新：每5秒更新可见项
                if (now.Second % 5 == 0)
                {
                    UpdateVisibleSessionsHeartbeatAndIdleTime();
                }

                // 3. 会话同步和清理：每30秒执行一次
                if (now.Second % 30 == 0)
                {
                    SyncWithSessionService();
                }

                // 4. 完整刷新：每60秒执行一次（仅在需要时）
                if (now.Second % 60 == 0 && _needsFullRefresh)
                {
                    FullRefreshFromSessions();
                    _needsFullRefresh = false;
                }
            }
            catch (Exception ex)
            {
                LogError("定时器更新时出错", ex);
            }
        }

        /// <summary>
        /// 标记需要完整刷新
        /// </summary>
        private void MarkForFullRefresh()
        {
            _needsFullRefresh = true;
        }

        /// <summary>
        /// 更新可见会话的心跳和空闲时间 - 性能优化版
        /// </summary>
        private void UpdateVisibleSessionsHeartbeatAndIdleTime()
        {
            try
            {
                if (listView1.Items.Count == 0) return;

                foreach (ListViewItem item in listView1.Items)
                {
                    // 只更新可见项
                    if (listView1.ClientRectangle.IntersectsWith(item.Bounds) && item.Tag is SessionInfo sessionInfo)
                    {
                        UpdateSessionIdleTime(item, sessionInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("更新可见会话心跳时间显示时出错", ex);
            }
        }

        /// <summary>
        /// 更新会话项的空闲时间
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="sessionInfo">会话信息</param>
        private void UpdateSessionIdleTime(ListViewItem item, SessionInfo sessionInfo)
        {
            try
            {
                if (item.SubItems.Count > 10)
                {
                    var idleTime = DateTime.Now - sessionInfo.LastHeartbeatTime;
                    item.SubItems[10].Text = FormatIdleTime(idleTime);
                }
            }
            catch (Exception ex)
            {
                LogError($"更新会话空闲时间时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 格式化静止时间显示
        /// </summary>
        /// <param name="idleTime">静止时间间隔</param>
        /// <returns>格式化的时间字符串</returns>
        private string FormatIdleTime(TimeSpan idleTime)
        {
            if (idleTime.TotalDays >= 1)
                return $"{idleTime.TotalDays:F1}天";
            else if (idleTime.TotalHours >= 1)
                return $"{idleTime.TotalHours:F1}小时";
            else if (idleTime.TotalMinutes >= 1)
                return $"{idleTime.TotalMinutes:F1}分钟";
            else
                return $"{idleTime.TotalSeconds:F0}秒";
        }

        #endregion

        #region 统计和同步

        /// <summary>
        /// 更新用户统计信息
        /// </summary>
        private void UpdateStatistics()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(UpdateStatistics));
                    return;
                }

                // 获取会话统计信息
                var statistics = _sessionService.GetStatistics();

                // 获取所有用户会话
                var allSessions = _sessionService.GetAllUserSessions().ToList();
                var authenticatedSessions = allSessions.Where(s => s.IsAuthenticated).ToList();

                // 更新标签文本
                lbl在线用户数.Text = $"在线用户: {allSessions.Count}";
                lbl总会话数.Text = $"总会话: {statistics.TotalConnections}";
                lbl已认证用户数.Text = $"已认证用户: {authenticatedSessions.Count}";
            }
            catch (Exception ex)
            {
                LogError($"更新统计信息时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 从会话服务完整刷新会话列表
        /// </summary>
        private void FullRefreshFromSessions()
        {
            try
            {
                // 获取所有当前会话
                var currentSessions = _sessionService.GetAllUserSessions().ToList();
                var currentSessionIds = new HashSet<string>(currentSessions.Select(s => s.SessionID));
                
                int newCount = 0;
                int removedCount = 0;

                // 添加新会话或更新现有会话
                foreach (var sessionInfo in currentSessions)
                {
                    AddOrUpdateSessionItem(sessionInfo);
                }

                // 移除不存在的会话
                var sessionsToRemove = _sessionItemMap.Keys.Where(sessionId => !currentSessionIds.Contains(sessionId)).ToList();
                foreach (var sessionId in sessionsToRemove)
                {
                    RemoveSessionItem(sessionId);
                    removedCount++;
                }

                // 只在有变化时记录日志
                if (newCount > 0 || removedCount > 0)
                {
                    LogStatusChange(null, $"完整刷新完成：新增 {newCount} 个会话，移除 {removedCount} 个会话，当前显示 {currentSessions.Count} 个会话");
                }
            }
            catch (Exception ex)
            {
                LogError("完整刷新会话列表时出错", ex);
            }
        }

        /// <summary>
        /// 同步会话服务状态
        /// </summary>
        private void SyncWithSessionService()
        {
            try
            {
                // 获取当前所有会话
                var currentSessions = _sessionService.GetAllUserSessions().ToList();
                int updatedCount = 0;

                foreach (var sessionInfo in currentSessions)
                {
                    if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                    {
                        // 检查是否需要更新
                        UpdateSessionItem(existingItem, sessionInfo);
                        updatedCount++;
                    }
                }

                if (updatedCount > 0)
                {
                    LogStatusChange(null, $"同步完成，更新 {updatedCount} 个会话状态");
                }
            }
            catch (Exception ex)
            {
                LogError($"同步会话服务状态时出错: {ex.Message}");
            }
        }

        #endregion

        #region 会话事件处理

        /// <summary>
        /// 会话连接事件处理
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        private void OnSessionConnected(SessionInfo sessionInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<SessionInfo>(OnSessionConnected), sessionInfo);
                return;
            }

            if (sessionInfo == null) return;

            try
            {
                // 添加或更新会话项
                AddOrUpdateSessionItem(sessionInfo);

                // 记录新连接
                string statusDescription = GetStatusDescription(sessionInfo.IsConnected, sessionInfo.IsAuthenticated);
                LogStatusChange(sessionInfo, $"新会话连接 - {statusDescription}");

                // 立即更新统计信息
                UpdateStatistics();

                // 刷新显示
                this.Refresh();
            }
            catch (Exception ex)
            {
                LogError($"处理会话连接事件时出错: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// 会话断开事件处理
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        private void OnSessionDisconnected(SessionInfo sessionInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<SessionInfo>(OnSessionDisconnected), sessionInfo);
                return;
            }

            if (sessionInfo == null) return;

            try
            {
                // 更新会话状态或移除会话项
                if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                {
                    // 更新会话信息并更新UI
                    UpdateSessionItem(existingItem, sessionInfo);
                    LogStatusChange(sessionInfo, "会话断开");
                }
                else
                {
                    // 如果找不到，创建一个断开的会话记录
                    var disconnectedSession = new SessionInfo
                    {
                        SessionID = sessionInfo.SessionID,
                        UserName = $"已断开用户_{sessionInfo.SessionID.Substring(0, Math.Min(8, sessionInfo.SessionID.Length))}",
                        ClientIp = sessionInfo.ClientIp ?? "未知IP",
                        IsConnected = false,
                        IsAuthenticated = false,
                        LastHeartbeatTime = DateTime.Now,
                        ConnectTime = sessionInfo.ConnectTime
                    };
                    
                    AddOrUpdateSessionItem(disconnectedSession);
                    LogStatusChange(disconnectedSession, "会话断开 - 创建断开记录");
                }

                // 立即更新统计信息
                UpdateStatistics();
                this.Refresh();
                
                // 标记需要完整刷新
                MarkForFullRefresh();
            }
            catch (Exception ex)
            {
                LogError($"处理会话断开事件时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 会话更新事件处理
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        private void OnSessionUpdated(SessionInfo sessionInfo)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<SessionInfo>(OnSessionUpdated), sessionInfo);
                return;
            }

            if (sessionInfo == null) return;

            try
            {
                // 更新或创建会话项
                if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                {
                    // 记录关键状态变化
                    var oldSessionInfo = existingItem.Tag as SessionInfo;
                    bool statusChanged = oldSessionInfo?.IsConnected != sessionInfo.IsConnected || 
                                       oldSessionInfo?.IsAuthenticated != sessionInfo.IsAuthenticated;
                    bool heartbeatChanged = Math.Abs((oldSessionInfo?.LastHeartbeatTime ?? DateTime.MinValue - sessionInfo.LastHeartbeatTime).TotalSeconds) > 5;
                    bool moduleChanged = oldSessionInfo?.CurrentModule != sessionInfo.CurrentModule;
                    bool formChanged = oldSessionInfo?.CurrentForm != sessionInfo.CurrentForm;

                    // 更新会话信息
                    UpdateSessionItem(existingItem, sessionInfo);
                    existingItem.Tag = sessionInfo; // 更新引用

                    // 只在有关键变化时记录日志
                    if (statusChanged)
                    {
                        LogStatusChange(sessionInfo, $"状态变化 - 连接:{sessionInfo.IsConnected}, 认证:{sessionInfo.IsAuthenticated}");
                    }
                    else if (moduleChanged)
                    {
                        LogStatusChange(sessionInfo, $"模块变化: {sessionInfo.CurrentModule}");
                    }
                    else if (formChanged)
                    {
                        LogStatusChange(sessionInfo, $"窗体变化: {sessionInfo.CurrentForm}");
                    }
                    else if (heartbeatChanged)
                    {
                        // 心跳变化不记录详细日志，避免日志过多
                    }
                }
                else
                {
                    // 新会话，添加到列表
                    AddOrUpdateSessionItem(sessionInfo);
                    LogStatusChange(sessionInfo, "新会话更新");
                }

                // 在重要状态变化时更新统计信息
                if (sessionInfo.IsAuthenticated || !sessionInfo.IsConnected)
                {
                    UpdateStatistics();
                }
            }
            catch (Exception ex)
            {
                LogError($"处理会话更新事件时出错: {ex.Message}", ex);
            }
        }

                    // 立即更新UI显示
                    AddOrUpdateUser(existingUser);

                    // 记录重要状态变化（详细日志）
                    var changeDescriptions = new List<string>();
                    
                    if (onlineStatusChanged)
                    {
                        changeDescriptions.Add($"在线状态: {(!sessionInfo.IsConnected ? "离线" : "在线")}");
                        MarkForFullRefresh(); // 重要状态变化
                    }
                    
                    if (authStatusChanged)
                    {
                        changeDescriptions.Add($"授权状态: {(sessionInfo.IsAuthenticated ? "已授权" : "未授权")}");
                        MarkForFullRefresh(); // 重要状态变化
                    }
                    
                    if (moduleChanged)
                    {
                        changeDescriptions.Add($"模块切换: {existingUser.当前模块} -> {currentModule}");
                    }
                    
                    if (formChanged)
                    {
                        changeDescriptions.Add($"窗体切换: {existingUser.当前窗体} -> {currentForm}");
                    }
                    
                    if (heartbeatChanged)
                    {
                        changeDescriptions.Add($"心跳更新: {sessionInfo.LastActivityTime:HH:mm:ss}");
                    }

                    // 只在有重要变化时记录日志
                    if (changeDescriptions.Count > 0)
                    {
                        LogStatusChange(existingUser, $"会话更新 - {string.Join(", ", changeDescriptions)}");
                    }
                }
                else
                {
                    // 新会话，添加到列表
                    var newUserInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                    AddOrUpdateUser(newUserInfo);
                    
                    string statusDescription = GetStatusDescription(sessionInfo.IsConnected, sessionInfo.IsAuthenticated);
                    LogStatusChange(newUserInfo, $"新会话更新 - {statusDescription}");
                    
                    MarkForFullRefresh(); // 新会话可能需要完整刷新
                }

                // 关键时刻：立即更新统计信息和UI
                UpdateStatistics();
                
                // 如果有重要状态变化，立即刷新显示
                if (existingUser != null && (existingUser.授权状态 != sessionInfo.IsAuthenticated || 
                    existingUser.在线状态 != sessionInfo.IsConnected))
                {
                    this.Refresh();
                }
            }
            catch (Exception ex)
            {
                LogError($"处理会话更新事件时出错: {ex.Message}", ex);
            }
        }



        #endregion

        #endregion

        /// <summary>
        /// 获取选中的会话列表
        /// </summary>
        /// <returns>选中的会话列表</returns>
        private List<SessionInfo> SelectSessions()
        {
            var selectedSessions = new List<SessionInfo>();

            // 先检查是否有复选框选中的会话
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                if (item.Tag is SessionInfo sessionInfo)
                {
                    selectedSessions.Add(sessionInfo);
                }
            }

            // 如果没有复选框选中的会话，则使用单选的会话
            if (selectedSessions.Count == 0 && listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is SessionInfo sessionInfo)
                {
                    selectedSessions.Add(sessionInfo);
                }
            }

            return selectedSessions;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selectedUsers = SelectUser();

            // 显示选中的用户
            MessageBox.Show($"选中了 {selectedUsers.Count} 个用户", "信息",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #region 用户信息

        private void RemoveUserFromListView(UserInfo user)
        {
            // 确保在 UI 线程执行
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => RemoveUserFromListView(user)));
                return;
            }

            // 只从UI中移除用户，不从UserInfos集合中移除
            // 因为这个方法是在UserInfos集合变更后调用的
            RemoveUser(user);
        }

        #endregion

        private void UserManagementControl_Disposed(object sender, EventArgs e)
        {
            // 清理资源
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
                _updateTimer.Dispose();
            }

            // 取消订阅事件
            if (_sessionService != null)
            {
                _sessionService.SessionConnected -= OnSessionConnected;
                _sessionService.SessionDisconnected -= OnSessionDisconnected;
                _sessionService.SessionUpdated -= OnSessionUpdated;
            }
        }

        #region 右键指令

        private async void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var users = SelectUser();
            if (users.Count == 0)
            {
                MessageBox.Show("请先选中一个用户！");
                return;
            }

            // 根据菜单项执行操作
            switch (e.ClickedItem.Text)
            {
                case "断开连接":
                    await HandleDisconnect(users);
                    break;
                case "强制用户退出":
                    HandleForceLogout(users);
                    break;
                case "删除列配置文件":
                    HandleDeleteConfig(users);
                    break;
                case "发消息给客户端":
                    HandleSendMessage(users);
                    break;
                case "推送版本更新":
                    HandlePushUpdate(users);
                    break;
                case "更新全局配置":
                    HandlePushUpdateSysConfig(users);
                    break;
                case "推送缓存数据":
                    HandlePushCache(users);
                    break;
                case "关机":
                    HandleShutdown(users);
                    break;
                case "切换服务器":
                    HandleSwitchServer(users);
                    break;
                case "全部切换服务器":
                    HandleSwitchAllServers(users);
                    break;
                default:
                    break;
            }
        }

        private void HandleSwitchServer(List<UserInfo> users)
        {
            string newServerAddress = string.Empty;
            var frmInput = new frmInput();
            frmInput.Text = "请输入新的服务器地址";
            if (frmInput.ShowDialog() == DialogResult.OK)
            {
                // 获取输入的内容
                newServerAddress = frmInput.InputContent;
            }

            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送切换服务器命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "SWITCH_SERVER",
                            ServerAddress = newServerAddress
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送切换服务器命令: {newServerAddress}");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送切换服务器命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"切换用户 {user.用户名} 服务器失败: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 处理全部切换服务器命令
        /// </summary>
        /// <param name="users">用户列表</param>
        private void HandleSwitchAllServers(List<UserInfo> users)
        {
            string newServerAddress = string.Empty;
            var frmInput = new frmInput();
            frmInput.Text = "请输入新的服务器地址";
            if (frmInput.ShowDialog() == DialogResult.OK)
            {
                // 获取输入的内容
                newServerAddress = frmInput.InputContent;
            }

            // 对所有在线用户执行切换服务器操作，而不仅仅是选中的用户
            var allUsers = UserInfos.ToList();

            foreach (var user in allUsers)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送切换服务器命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "SWITCH_SERVER",
                            ServerAddress = newServerAddress
                        };

                        var request = new MessageRequest(MessageType.Message, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送切换服务器命令: {newServerAddress}");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送切换服务器命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"切换用户 {user.用户名} 服务器失败: {ex.Message}");
                }
            }
        }

        //----------- 子方法实现 -----------
        private async Task HandleDisconnect(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService断开会话
                    await _sessionService.DisconnectSessionAsync(user.SessionId);
                    frmMainNew.Instance.PrintInfoLog($"已断开用户 {user.用户名} 的连接");
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"断开用户 {user.用户名} 连接失败: {ex.Message}");
                }
            }
        }

        private void HandleForceLogout(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送强制退出命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "FORCE_LOGOUT"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送强制退出命令");
                            RemoveUserFromListView(user); // 调用移除方法
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送强制退出命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"强制用户 {user.用户名} 退出失败: {ex.Message}");
                }
            }
        }

        private void HandleDeleteConfig(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送删除配置文件命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "DELETE_CONFIG"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送删除配置文件命令");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送删除配置文件命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"删除用户 {user.用户名} 配置文件失败: {ex.Message}");
                }
            }
        }

        private async void HandleSendMessage(List<UserInfo> users)
        {
            frmMessager frmMessager = new frmMessager();
            frmMessager.MustDisplay = true;
            if (frmMessager.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            string message = frmMessager.Message;
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息

                    #region
                    var response = await _serverMessageService.SendPopupMessageAsync(
                        user.用户名,
                        message,
                        "系统通知",
                        30000, // 30秒超时
                        CancellationToken.None);
                    #endregion

                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送消息失败: {ex.Message}");
                }
            }
        }

        private void HandlePushCache(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送缓存推送命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_CACHE"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送缓存推送命令");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送缓存推送命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送缓存推送命令失败: {ex.Message}");
                }
            }
        }

        private void HandlePushUpdate(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送更新推送命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_UPDATE"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送更新推送命令");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送更新推送命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送更新推送命令失败: {ex.Message}");
                }
            }
        }

        private void HandlePushUpdateSysConfig(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送系统配置推送命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_SYS_CONFIG"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送系统配置推送命令");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送系统配置推送命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送系统配置推送命令失败: {ex.Message}");
                }
            }
        }

        private void tsbtn推送缓存_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的用户
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要推送缓存的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is UserInfo userInfo)
                {
                    // 获取会话信息
                    var session = _sessionService.GetSession(userInfo.SessionId);
                    if (session == null)
                    {
                        MessageBox.Show($"用户 {userInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 确认推送缓存
                    var result = MessageBox.Show($"确定要向用户 {userInfo.用户名} 推送缓存数据吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // 发送推送缓存命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_CACHE"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {userInfo.用户名} 推送缓存数据");
                            MessageBox.Show("缓存数据推送成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("缓存数据推送失败，请检查用户连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"推送缓存数据时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void HandleShutdown(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 发送关机命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "SHUTDOWN"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送关机命令");
                        }
                        else
                        {
                            frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送关机命令失败");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送关机命令失败: {ex.Message}");
                }
            }
        }

        public void OnSessionClosed(List<UserInfo> users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                RemoveUserFromListView(user);
            }
        }

        /// <summary>
        /// 记录状态变化日志
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="changeDescription">变化描述</param>
        private void LogStatusChange(SessionInfo sessionInfo, string changeDescription)
        {
            try
            {
                // 添加空值检查，防止sessionInfo为null导致错误
                if (sessionInfo == null)
                {
                    string logMessagenull = $"[会话状态变化] 会话信息为空, {changeDescription}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    frmMainNew.Instance.PrintInfoLog(logMessagenull);
                    return;
                }

                string userDisplayName = GetDisplayName(sessionInfo.UserName);
                string userRealName = sessionInfo.DisplayName ?? "未知姓名";
                string logMessage = $"[会话状态变化] 用户: {userDisplayName} ({userRealName}), {changeDescription}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                frmMainNew.Instance.PrintInfoLog(logMessage);
            }
            catch (Exception ex)
            {
                LogError($"记录状态变化日志时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取状态描述字符串
        /// </summary>
        /// <param name="isConnected">是否连接</param>
        /// <param name="isAuthenticated">是否已认证</param>
        /// <returns>状态描述</returns>
        private string GetStatusDescription(bool isConnected, bool isAuthenticated)
        {
            if (!isConnected)
                return "已断开";
            else if (isAuthenticated)
                return "已连接且已授权";
            else
                return "已连接但未授权";
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="ex">异常对象</param>
        private void LogError(string message, Exception ex = null)
        {
            try
            {
                string logMessage = $"[用户管理错误] {message}";
                if (ex != null)
                {
                    logMessage += $", 异常: {ex.Message}";
                }
                logMessage += $", 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                frmMainNew.Instance.PrintErrorLog(logMessage);
            }
            catch (Exception logEx)
            {
                Console.WriteLine($"记录错误日志时出错: {logEx.Message}");
            }
        }

        /// <summary>
        /// 与会话服务同步状态
        /// 检测会话服务中的变化并同步到用户管理界面
        /// </summary>
        private void SyncWithSessionService()
        {
            try
            {
                var sessionSessions = _sessionService.GetAllUserSessions().ToList();
                var sessionDict = sessionSessions.ToDictionary(s => s.SessionID, s => s);
                
                int syncChanges = 0;
                
                // 检查现有用户的状态变化
                foreach (var userInfo in UserInfos.ToList())
                {
                    if (sessionDict.TryGetValue(userInfo.SessionId, out var sessionInfo))
                    {
                        // 检查关键状态是否有变化
                        bool statusChanged = userInfo.在线状态 != sessionInfo.IsConnected ||
                                            userInfo.授权状态 != sessionInfo.IsAuthenticated ||
                                            Math.Abs((userInfo.最后心跳时间.ObjToDate() - sessionInfo.LastActivityTime).TotalSeconds) > 5;
                        
                        if (statusChanged)
                        {
                            var updatedUserInfo = ConvertSessionInfoToUserInfo(sessionInfo, false);
                            UpdateUserInfoProperties(userInfo, updatedUserInfo);
                            AddOrUpdateUser(userInfo);
                            syncChanges++;
                        }
                    }
                    else
                    {
                        // 会话不存在，标记为离线
                        if (userInfo.在线状态)
                        {
                            userInfo.在线状态 = false;
                            userInfo.最后心跳时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            AddOrUpdateUser(userInfo);
                            syncChanges++;
                        }
                    }
                }

                // 发现新会话
                foreach (var sessionInfo in sessionSessions)
                {
                    if (!UserInfos.Any(u => u.SessionId == sessionInfo.SessionID))
                    {
                        var newUserInfo = ConvertSessionInfoToUserInfo(sessionInfo, false);
                        AddOrUpdateUser(newUserInfo);
                        syncChanges++;
                    }
                }

                if (syncChanges > 0)
                {
                    LogStatusChange(null, $"会话服务同步完成，检测到 {syncChanges} 个变化");
                }
            }
            catch (Exception ex)
            {
                LogError("与会话服务同步时出错", ex);
            }
        }

        /// <summary>
        /// 记录性能统计信息
        /// </summary>
        private void LogPerformanceStatistics()
        {
            try
            {
                var stats = _sessionService.GetStatistics();
                var onlineUsers = UserInfos.Count(u => u.在线状态);
                var authenticatedUsers = UserInfos.Count(u => u.授权状态);
                
                string performanceInfo = $"[性能统计] 在线用户: {onlineUsers}, 已认证: {authenticatedUsers}, " +
                                       $"总会话: {stats.TotalConnections}, 峰值连接: {stats.MaxConnections}, " +
                                       $"用户管理界面项数: {listView1.Items.Count}";
                
                frmMainNew.Instance.PrintInfoLog(performanceInfo);
            }
            catch (Exception ex)
            {
                LogError("记录性能统计时出错", ex);
            }
        }

        /// <summary>
        /// 清理离线超时用户（30分钟）
        /// </summary>
        private void CleanupInactiveUsers()
        {
            // 计算30分钟前的时间点
            DateTime threshold = DateTime.Now.AddMinutes(-30);

            // 找出需要移除的用户（心跳时间为空或超过30分钟）
            var inactiveUsers = UserInfos
                .Where(u => !string.IsNullOrEmpty(u.最后心跳时间) && (u.最后心跳时间.ObjToDate()) < threshold)
                .ToList();

            if (inactiveUsers.Any())
            {
                listView1.BeginUpdate();
                // 从ListView中移除
                foreach (var user in inactiveUsers)
                {
                    LogStatusChange(user, "离线超时清理（30分钟）");
                    RemoveUser(user);
                    user.PropertyChanged -= UserInfo_PropertyChanged;
                }
                listView1.EndUpdate();

                frmMainNew.Instance.PrintInfoLog($"清理了 {inactiveUsers.Count} 个离线超时用户");
            }
        }

        #endregion

        private void btnSelectedAll_Click(object sender, EventArgs e)
        {
            SelectAllItems();
        }

        private void btnNotAllSelected_Click(object sender, EventArgs e)
        {
            DeselectAllItems();
        }

        private void btnReverseSelected_Click(object sender, EventArgs e)
        {
            InvertSelection();
        }

        private void tsbtn发送消息_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的用户
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要发送消息的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is UserInfo userInfo)
                {
                    // 获取会话信息
                    var session = _sessionService.GetSession(userInfo.SessionId);
                    if (session == null)
                    {
                        MessageBox.Show($"用户 {userInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    var userList = new List<UserInfo>();
                    userList.Add(userInfo);
                    HandleSendMessage(userList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送消息时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtn刷新_Click(object sender, EventArgs e)
        {
            try
            {
                // 清空现有列表
                listView1.Items.Clear();
                _itemMap.Clear();
                UserInfos.Clear();

                // 重新加载所有会话
                var sessions = _sessionService.GetAllUserSessions().ToList();
                var sessionCount = sessions.Count;
                var newUsers = 0;
                var updatedUsers = 0;

                frmMainNew.Instance.PrintInfoLog($"服务器用户数量：{sessionCount}");

                // 添加所有用户（带重复检查）
                foreach (var session in sessions)
                {
                    // 检查是否已存在相同会话ID的用户
                    var existingUser = UserInfos.FirstOrDefault(u => u.SessionId == session.SessionID);

                    // 如果用户名空，检查是否已存在相同客户端IP的未认证用户
                    if (existingUser == null && string.IsNullOrEmpty(session.UserName))
                    {
                        existingUser = UserInfos.FirstOrDefault(u =>
                            u.用户名.StartsWith("未认证用户_") && u.客户端IP == session.ClientIp);
                    }

                    var userInfo = ConvertSessionInfoToUserInfo(session);

                    if (existingUser != null)
                    {
                        // 更新现有用户
                        existingUser.用户名 = userInfo.用户名;
                        existingUser.客户端IP = userInfo.客户端IP;
                        existingUser.登陆时间 = userInfo.登陆时间;
                        existingUser.最后心跳时间 = userInfo.最后心跳时间;
                        existingUser.在线状态 = userInfo.在线状态;
                        existingUser.授权状态 = userInfo.授权状态;
                        existingUser.UserID = userInfo.UserID;

                        // 更新ListView显示 - 使用现有的AddOrUpdateUser方法
                        AddOrUpdateUser(existingUser);
                        updatedUsers++;
                    }
                    else
                    {
                        // 添加新用户
                        frmMainNew.Instance.PrintInfoLog($"添加用户：{userInfo.用户名}");
                        AddOrUpdateUser(userInfo);
                        newUsers++;
                    }
                }

                // 更新统计信息
                UpdateStatistics();

                frmMainNew.Instance.PrintInfoLog($"刷新完成：新增 {newUsers} 个用户，更新 {updatedUsers} 个用户");
            }
            catch (Exception ex)
            {
                LogError($"刷新用户列表时出错: {ex.Message}");
                MessageBox.Show($"刷新用户列表时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtn推送更新_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的用户
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要推送更新的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is UserInfo userInfo)
                {
                    // 获取会话信息
                    var session = _sessionService.GetSession(userInfo.SessionId);
                    if (session == null)
                    {
                        MessageBox.Show($"用户 {userInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 确认推送更新
                    var result = MessageBox.Show($"确定要向用户 {userInfo.用户名} 推送更新吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // 发送推送更新命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_UPDATE"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {userInfo.用户名} 推送更新");
                            MessageBox.Show("更新推送成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("更新推送失败，请检查用户连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"推送更新时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsbtn推送系统配置_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中的用户
                if (listView1.SelectedItems.Count == 0)
                {
                    MessageBox.Show("请先选择要推送系统配置的用户", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is UserInfo userInfo)
                {
                    // 获取会话信息
                    var session = _sessionService.GetSession(userInfo.SessionId);
                    if (session == null)
                    {
                        MessageBox.Show($"用户 {userInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 确认推送系统配置
                    var result = MessageBox.Show($"确定要向用户 {userInfo.用户名} 推送系统配置吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // 发送推送系统配置命令 - 使用新的发送方法
                        var messageData = new
                        {
                            Command = "PUSH_SYS_CONFIG"
                        };

                        var request = new MessageRequest(MessageType.Unknown, messageData);
                        var success = _sessionService.SendCommandAsync(
                            session.SessionID,
                            MessageCommands.SendMessageToUser,
                            request).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                        if (success)
                        {
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {userInfo.用户名} 推送系统配置");
                            MessageBox.Show("系统配置推送成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("系统配置推送失败，请检查用户连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"推送系统配置时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 发消息给客户端ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    // 扩展方法：启用ListView的双缓冲
    public static class ControlExtensions
    {
        public static void DoubleBuffered(this Control control, bool enabled)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty(
                "DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            doubleBufferPropertyInfo?.SetValue(control, enabled, null);
        }
    }
}
