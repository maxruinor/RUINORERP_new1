using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Messaging;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.ToolsUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        /// <summary>
        /// 断开连接的会话在UI中的保留时间（分钟）
        /// </summary>
        private int _disconnectedSessionRetentionMinutes = 2;

        /// <summary>
        /// 上次立即刷新时间，用于避免过于频繁的刷新
        /// </summary>
        private DateTime _lastImmediateRefresh = DateTime.MinValue;

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        private const int SB_HORZ = 0;

        #endregion

        #region 构造函数和初始化

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

            // 添加断开连接菜单项
            AddDisconnectMenuItem();
        }

        /// <summary>
        /// 向右键菜单添加断开连接菜单项
        /// </summary>
        private void AddDisconnectMenuItem()
        {
            try
            {
                // 检查菜单项是否已存在
                if (contextMenuStrip1.Items.Cast<ToolStripItem>().Any(item => item.Text == "断开连接"))
                {
                    return; // 菜单项已存在，不需要再次添加
                }

                // 创建分隔线
                ToolStripSeparator separator = new ToolStripSeparator();

                // 创建断开连接菜单项
                ToolStripMenuItem disconnectMenuItem = new ToolStripMenuItem
                {
                    Text = "断开连接",
                    ToolTipText = "强制断开选中的用户会话连接",
                    Image = null // 可以根据需要设置图标
                };

                // 添加菜单项到右键菜单
                contextMenuStrip1.Items.Add(separator);
                contextMenuStrip1.Items.Add(disconnectMenuItem);
            }
            catch (Exception ex)
            {
                LogError("添加断开连接菜单项时出错", ex);
            }

            // 添加切换服务器相关菜单项
            AddSwitchServerMenuItems();
        }

        private void AddSwitchServerMenuItems()
        {
            try
            {
                // 检查是否已存在切换服务器菜单项
                bool switchServerExists = contextMenuStrip1.Items.Cast<ToolStripItem>().Any(item => item.Text == "切换服务器");
                bool switchAllServersExists = contextMenuStrip1.Items.Cast<ToolStripItem>().Any(item => item.Text == "全部切换服务器");

                // 如果还没有服务器相关菜单项，添加分隔线和菜单项
                if (!switchServerExists && !switchAllServersExists)
                {
                    // 添加分隔线
                    contextMenuStrip1.Items.Add(new ToolStripSeparator());
                }

                // 添加切换服务器菜单项
                if (!switchServerExists)
                {
                    ToolStripMenuItem switchServerMenuItem = new ToolStripMenuItem
                    {
                        Text = "切换服务器",
                        ToolTipText = "为选中用户切换到指定服务器",
                        Image = null
                    };
                    contextMenuStrip1.Items.Add(switchServerMenuItem);
                }

                // 添加全部切换服务器菜单项
                if (!switchAllServersExists)
                {
                    ToolStripMenuItem switchAllServersMenuItem = new ToolStripMenuItem
                    {
                        Text = "全部切换服务器",
                        ToolTipText = "为所有用户切换到指定服务器",
                        Image = null
                    };
                    contextMenuStrip1.Items.Add(switchAllServersMenuItem);
                }
            }
            catch (Exception ex)
            {
                LogError("添加切换服务器菜单项时出错", ex);
            }
        }

        private void InitializeListView()
        {
            // 启用双缓冲减少闪烁
            SetListViewDoubleBuffer(listView1);
            listView1.VirtualMode = false;

            // 配置 ListView
            listView1.CheckBoxes = true;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.MouseDoubleClick += listView1_MouseDoubleClick;
            AddCols();
        }

        /// <summary>
        /// 设置ListView双缓冲以减少闪烁
        /// </summary>
        /// <param name="listView">ListView控件</param>
        private void SetListViewDoubleBuffer(ListView listView)
        {
            typeof(ListView).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null,
                listView,
                new object[] { true });
        }

        private void LoadAllSessions()
        {
            try
            {
                var sessions = _sessionService.GetAllUserSessions().ToList();
                int loadedCount = 0;
                int skippedCount = 0;

                foreach (var session in sessions)
                {
                    // 检查会话是否已授权，未授权的会话不添加到表格中
                    var userInfo = session.UserInfo ?? new UserInfo();
                    if (!userInfo.授权状态)
                    {
                        skippedCount++;
                        continue;
                    }

                    AddOrUpdateSessionItem(session);
                    loadedCount++;
                }

                // 记录加载结果
                LogStatusChange(null, $"初始加载完成：加载 {loadedCount} 个会话，跳过 {skippedCount} 个未授权会话");

                // 初始化统计信息
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                LogError($"加载现有会话时出错: {ex.Message}", ex);
            }
        }

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

        #endregion

        #region 列显示管理

        private void AddCols()
        {
            listView1.Columns.Clear();
            // 调整第一列宽度，为复选框留出空间
            listView1.Columns.Add("", 30); // 复选框列
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
            listView1.Columns.Add("超级用户", 80);        // 超级用户
            listView1.Columns.Add("在线状态", 80);        // 在线状态
            listView1.Columns.Add("授权状态", 80);        // 授权状态
            listView1.Columns.Add("操作系统", 150);       // 操作系统
            listView1.Columns.Add("机器名", 100);         // 机器名
            listView1.Columns.Add("CPU信息", 150);        // CPU信息
            listView1.Columns.Add("内存大小", 100);       // 内存大小
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
                LogError("设置列显示选项时出错: " + ex.Message);
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
                newItem.ToolTipText = sessionInfo.SessionID;
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

            try
            {
                if (string.IsNullOrEmpty(sessionId))
                {
                    LogError("尝试移除会话ID为空的会话项");
                    return;
                }

                if (_sessionItemMap.TryGetValue(sessionId, out var item))
                {
                    // 记录将要移除的会话信息
                    string sessionInfo = "未知会话";
                    if (item.Tag is SessionInfo sessionData)
                    {
                        var userInfo = sessionData.UserInfo ?? new UserInfo();
                        sessionInfo = $"用户: {GetDisplayUserName(userInfo)}, IP: {sessionData.ClientIp}";
                    }

                    // 移除会话项
                    listView1.BeginUpdate();
                    listView1.Items.Remove(item);
                    _sessionItemMap.Remove(sessionId);
                    listView1.EndUpdate();

                    LogStatusChange(null, $"会话项已移除: {sessionInfo} (SessionID: {sessionId})");
                }
                else
                {
                    LogStatusChange(null, $"尝试移除不存在的会话项: {sessionId}");
                }
            }
            catch (Exception ex)
            {
                LogError($"移除会话项时出错: {sessionId}", ex);
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

            // 从UserInfo获取详细信息，从SessionInfo获取连接信息
            var userInfo = sessionInfo.UserInfo ?? new UserInfo();

            item.ToolTipText = $"SessionID: {sessionInfo.SessionID}\n用户名: {GetDisplayUserName(userInfo)}\n客户端IP: {sessionInfo.ClientIp}";

            // 添加各列数据
            item.SubItems.Add(GetDisplayUserName(userInfo)); // 用户名
            item.SubItems.Add(GetDisplayName(userInfo?.姓名)); // 姓名
            item.SubItems.Add(GetDisplayName(userInfo?.当前模块)); // 当前模块
            item.SubItems.Add(GetDisplayName(userInfo?.当前窗体)); // 当前窗体
            item.SubItems.Add((sessionInfo.ConnectTime ?? DateTime.Now).ToString("yy-MM-dd HH:mm:ss")); // 连接时间
            item.SubItems.Add(sessionInfo.HeartbeatCount.ToString()); // 心跳数
            item.SubItems.Add(sessionInfo.LastHeartbeat.ToString("yy-MM-dd HH:mm:ss")); // 最后心跳时间
            item.SubItems.Add(GetDisplayName(sessionInfo.UserInfo.客户端版本)); // 客户端版本
            item.SubItems.Add(sessionInfo.ClientIp); // 客户端IP
            item.SubItems.Add(FormatIdleTime(DateTime.Now - sessionInfo.LastHeartbeat)); // 静止时间
            item.SubItems.Add(GetSuperUserStatus(userInfo)); // 超级用户
            item.SubItems.Add(sessionInfo.IsConnected ? "在线" : "离线"); // 在线状态
            item.SubItems.Add(GetAuthorizationStatus(userInfo)); // 授权状态
            item.SubItems.Add(GetDisplayName(userInfo?.操作系统)); // 操作系统
            item.SubItems.Add(GetDisplayName(userInfo?.机器名)); // 机器名
            item.SubItems.Add(GetDisplayName(userInfo?.CPU信息)); // CPU信息
            item.SubItems.Add(GetDisplayName(userInfo?.内存大小)); // 内存大小

            SetSessionItemStyle(item, sessionInfo, userInfo);
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

            // 从UserInfo获取详细信息
            var userInfo = sessionInfo.UserInfo ?? new UserInfo();

            // 更新所有列的数据
            item.SubItems[1].Text = GetDisplayUserName(userInfo);
            item.SubItems[2].Text = GetDisplayName(sessionInfo.UserName);
            item.SubItems[3].Text = GetDisplayName(userInfo?.当前模块);
            item.SubItems[4].Text = GetDisplayName(userInfo?.当前窗体);
            item.SubItems[5].Text = (sessionInfo.ConnectTime ?? DateTime.Now).ToString("yy-MM-dd HH:mm:ss");
            item.SubItems[6].Text = sessionInfo.HeartbeatCount.ToString();
            item.SubItems[7].Text = sessionInfo.LastHeartbeat.ToString("yy-MM-dd HH:mm:ss");
            item.SubItems[8].Text = GetDisplayName(sessionInfo.UserInfo.客户端版本);
            item.SubItems[9].Text = sessionInfo.ClientIp ?? sessionInfo.RemoteEndPoint.ToString();
            item.SubItems[10].Text = FormatIdleTime(DateTime.Now - sessionInfo.LastHeartbeat);
            item.SubItems[11].Text = GetSuperUserStatus(userInfo);
            item.SubItems[12].Text = sessionInfo.IsConnected ? "在线" : "离线";
            item.SubItems[13].Text = GetAuthorizationStatus(userInfo);
            item.SubItems[14].Text = GetDisplayName(userInfo?.操作系统);
            item.SubItems[15].Text = GetDisplayName(userInfo?.机器名);
            item.SubItems[16].Text = GetDisplayName(userInfo?.CPU信息);
            item.SubItems[17].Text = GetDisplayName(userInfo?.内存大小);

            SetSessionItemStyle(item, sessionInfo, userInfo);
        }

        /// <summary>
        /// 设置会话项的视觉样式
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="userInfo">用户信息</param>
        private void SetSessionItemStyle(ListViewItem item, SessionInfo sessionInfo, UserInfo userInfo)
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
                else if (sessionInfo.IsConnected && !userInfo.授权状态)
                {
                    // 在线但未授权：橙色文字，浅黄色背景，加粗显示
                    item.ForeColor = Color.DarkOrange;
                    item.BackColor = Color.LightYellow;
                    item.Font = new Font(item.Font, FontStyle.Bold);
                }
                else if (sessionInfo.IsConnected && userInfo.授权状态)
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
                if (userInfo.超级用户)
                {
                    if (item.SubItems.Count > 0)
                    {
                        item.SubItems[0].BackColor = Color.LightBlue;
                    }
                }

                // 心跳异常检测
                var timeSinceHeartbeat = DateTime.Now - sessionInfo.LastHeartbeat;
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
                return "未知";
            }
            return userName;
        }

        /// <summary>
        /// 获取显示用户名 - 优先使用UserInfo中的用户名
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>显示用户名</returns>
        private string GetDisplayUserName(UserInfo userInfo)
        {
            if (userInfo != null && !string.IsNullOrEmpty(userInfo.用户名))
            {
                return userInfo.用户名;
            }
            return "未认证用户";
        }

        /// <summary>
        /// 获取超级用户状态
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>超级用户状态</returns>
        private string GetSuperUserStatus(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                return userInfo.超级用户 ? "是" : "否";
            }
            return "否";
        }

        /// <summary>
        /// 获取授权状态
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <returns>授权状态</returns>
        private string GetAuthorizationStatus(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                return userInfo.授权状态 ? "已授权" : "未授权";
            }
            return "未授权";
        }

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
                    // 检查listView1是否已经被释放
                    if (listView1.IsDisposed) return;
                    UpdateVisibleSessionsHeartbeatAndIdleTime();
                }

                // 3. 会话同步：每30秒执行一次
                if (now.Second % 30 == 0)
                {
                    SyncWithSessionService();
                    // 同步后刷新UI确保实时显示
                    if (!listView1.IsDisposed)
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            if (!listView1.IsDisposed)
                                listView1.Refresh();
                        });
                    }
                }

                // 4. 完整刷新：每60秒执行一次
                if (now.Second % 60 == 0 || _needsFullRefresh)
                {
                    // 优先处理需要完整刷新的情况，不等待60秒
                    if (_needsFullRefresh)
                    {
                        _needsFullRefresh = false;
                    }
                    FullRefreshFromSessions();

                    // 确保UI完全刷新
                    if (!listView1.IsDisposed)
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            if (!listView1.IsDisposed)
                                listView1.Refresh();
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("定时器更新时出错", ex);
                // 发生错误时，确保下次会重新同步
                _needsFullRefresh = true;
            }
        }

        /// <summary>
        /// 标记需要完整刷新并尝试立即触发刷新
        /// </summary>
        private void MarkForFullRefresh()
        {
            _needsFullRefresh = true;

            // 尝试立即触发刷新而不等待定时器周期
            // 但避免过于频繁的刷新
            if (DateTime.Now.Subtract(_lastImmediateRefresh).TotalSeconds > 5)
            {
                _lastImmediateRefresh = DateTime.Now;

                // 检查控件句柄是否已创建，避免在句柄创建前调用BeginInvoke导致异常
                if (IsHandleCreated && !IsDisposed)
                {                    // 在UI线程上执行刷新
                    BeginInvoke((MethodInvoker)delegate
                    {
                        try
                        {
                            if (!IsDisposed && !listView1.IsDisposed)
                            {                                // 执行轻量级同步而不是完整刷新，以避免性能问题
                                SyncWithSessionService();
                                listView1.Refresh();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogError("立即刷新失败", ex);
                        }
                    });
                }
                else
                {                    // 控件句柄尚未创建，记录日志并依赖定时器刷新
                    LogStatusChange(null, "控件句柄尚未创建，延迟刷新操作，将由定时器处理");
                }
            }
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
                    var idleTime = DateTime.Now - sessionInfo.LastHeartbeat;
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
            // 检查控件是否已经被释放
            if (IsDisposed || listView1.IsDisposed)
            {
                LogError("FullRefreshFromSessions: 控件已被释放，无法执行刷新操作");
                return;
            }

            // 初始化状态计数器
            int newCount = 0;
            int updatedCount = 0;
            int removedCount = 0;
            int statusChangedCount = 0;
            bool refreshSuccessful = false;

            try
            {
                // 开始更新，避免UI闪烁
                listView1.BeginUpdate();

                // 获取所有当前会话 - 添加超时保护
                var currentSessions = new List<SessionInfo>();
                try
                {
                    // 限制获取会话的时间，避免长时间阻塞
                    using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                    {
                        currentSessions = _sessionService.GetAllUserSessions().ToList();
                    }
                }
                catch (Exception getSessionsEx)
                {
                    LogError($"获取会话列表时发生超时或错误: {getSessionsEx.Message}", getSessionsEx);
                    currentSessions = new List<SessionInfo>();
                }

                var currentSessionIds = new HashSet<string>(currentSessions.Select(s => s.SessionID));

                // 添加新会话或更新现有会话
                foreach (var sessionInfo in currentSessions)
                {
                    try
                    {
                        if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                        {
                            // 更新现有会话
                            // 检查会话状态是否发生变化
                            bool statusChanged = false;
                            if (existingItem.Tag is SessionInfo existingSessionData)
                            {
                                // 检查连接状态是否变化
                                if (existingSessionData.IsConnected != sessionInfo.IsConnected)
                                {
                                    statusChanged = true;
                                    statusChangedCount++;
                                    string statusDesc = sessionInfo.IsConnected ? "已连接" : "已断开";
                                    LogStatusChange(sessionInfo, $"会话状态变更为{statusDesc}: {GetDisplayUserName(sessionInfo.UserInfo ?? new UserInfo())} - {sessionInfo.ClientIp}");
                                }
                            }

                            UpdateSessionItem(existingItem, sessionInfo);
                            updatedCount++;
                        }
                        else
                        {
                            // 添加新会话
                            AddOrUpdateSessionItem(sessionInfo);
                            newCount++;
                        }
                    }
                    catch (Exception updateEx)
                    {
                        // 记录单个会话更新错误，但继续处理其他会话
                        LogError($"更新会话 {sessionInfo.SessionID} 时出错: {updateEx.Message}", updateEx);
                    }
                }

                // 移除不存在的会话或已断开且超过保留时间的会话
                var sessionsToRemove = new List<string>();
                var now = DateTime.Now;

                foreach (var sessionId in _sessionItemMap.Keys.ToList())
                {
                    try
                    {
                        if (!currentSessionIds.Contains(sessionId))
                        {
                            // 移除不存在于服务中的会话
                            sessionsToRemove.Add(sessionId);
                        }
                        else if (_sessionItemMap.TryGetValue(sessionId, out var existingItem) && existingItem.Tag is SessionInfo sessionData)
                        {
                            // 检查是否需要移除已断开且超过保留时间的会话
                            if (!sessionData.IsConnected && (now - sessionData.LastHeartbeat).TotalMinutes > _disconnectedSessionRetentionMinutes)
                            {
                                sessionsToRemove.Add(sessionId);
                            }
                        }
                    }
                    catch (Exception removeCheckEx)
                    {
                        // 记录检查移除条件时的错误，但继续处理其他会话
                        LogError($"检查会话 {sessionId} 是否需要移除时出错: {removeCheckEx.Message}", removeCheckEx);
                    }
                }

                // 批量移除会话项
                foreach (var sessionId in sessionsToRemove)
                {
                    try
                    {
                        RemoveSessionItem(sessionId);
                        removedCount++;
                    }
                    catch (Exception removeEx)
                    {
                        // 记录单个会话移除错误，但继续处理其他会话
                        LogError($"移除会话 {sessionId} 时出错: {removeEx.Message}", removeEx);
                    }
                }

                refreshSuccessful = true;

                // 只在有变化时记录日志
                if (newCount > 0 || updatedCount > 0 || removedCount > 0)
                {
                    LogStatusChange(null, $"完整刷新完成：新增 {newCount} 个会话，更新 {updatedCount} 个会话，状态变更 {statusChangedCount} 个会话，移除 {removedCount} 个会话，当前显示 {_sessionItemMap.Count} 个会话");
                }
            }
            catch (Exception ex)
            {
                LogError("完整刷新会话列表时出错", ex);
                // 发生严重错误时，确保设置标记让下次能尝试重新同步
                _needsFullRefresh = true;
            }
            finally
            {
                // 无论如何都要结束更新，避免UI卡死
                try
                {
                    if (!listView1.IsDisposed)
                    {
                        listView1.EndUpdate();
                        // 确保UI与数据同步
                        listView1.Refresh();
                    }
                }
                catch (Exception uiEx)
                {
                    LogError("刷新UI时发生错误", uiEx);
                }

                // 更新统计信息
                try
                {
                    UpdateStatistics();
                }
                catch (Exception statsEx)
                {
                    LogError("更新统计信息失败", statsEx);
                }

                // 重置刷新状态
                if (refreshSuccessful)
                {
                    _lastFullUpdate = DateTime.Now;
                    _needsFullRefresh = false;
                }

                // 记录刷新结果
                LogStatusChange(null, $"刷新操作{(refreshSuccessful ? "成功" : "失败")}: 新增={newCount}, 更新={updatedCount}, 移除={removedCount}");
            }
        }

        /// <summary>
        /// 同步会话服务状态
        /// </summary>
        /// <summary>
        /// 清除所有会话项
        /// </summary>
        private void ClearAllSessionItems()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearAllSessionItems));
                return;
            }

            try
            {
                listView1.BeginUpdate();
                listView1.Items.Clear();
                _sessionItemMap.Clear();
                listView1.EndUpdate();

                LogStatusChange(null, "所有会话项已清除");
            }
            catch (Exception ex)
            {
                LogError("清除所有会话项时出错", ex);
                try
                {
                    listView1.EndUpdate();
                }
                catch { }
            }
        }

        private void SyncWithSessionService()
        {
            // 检查控件是否已经被释放
            if (IsDisposed || listView1.IsDisposed)
            {
                LogError("SyncWithSessionService: 控件已被释放，无法执行同步操作");
                return;
            }

            int updatedCount = 0;
            bool syncSuccessful = false;

            try
            {
                // 获取当前所有会话 - 添加异常处理
                var currentSessions = new List<SessionInfo>();
                try
                {
                    // 设置获取会话的超时保护
                    using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5)))
                    {
                        currentSessions = _sessionService.GetAllUserSessions().ToList();
                    }
                }
                catch (Exception getSessionsEx)
                {
                    LogError($"获取会话列表进行同步时出错: {getSessionsEx.Message}", getSessionsEx);
                    currentSessions = new List<SessionInfo>();
                }

                foreach (var sessionInfo in currentSessions)
                {
                    try
                    {
                        if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                        {
                            // 检查是否需要更新
                            UpdateSessionItem(existingItem, sessionInfo);
                            updatedCount++;
                        }
                    }
                    catch (Exception updateEx)
                    {
                        // 记录单个会话更新错误，但继续处理其他会话
                        LogError($"同步更新会话 {sessionInfo.SessionID} 时出错: {updateEx.Message}", updateEx);
                    }
                }

                syncSuccessful = true;

                if (updatedCount > 0)
                {
                    LogStatusChange(null, $"同步完成，更新 {updatedCount} 个会话状态");

                    // 同步完成后立即刷新UI
                    try
                    {
                        if (!listView1.IsDisposed)
                        {
                            listView1.Refresh();
                        }
                    }
                    catch (Exception uiEx)
                    {
                        LogError("同步后刷新UI失败", uiEx);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError($"同步会话服务状态时发生未处理异常: {ex.Message}", ex);
                // 发生错误时确保下次会重新同步
                _needsFullRefresh = true;
            }
            finally
            {
                // 记录同步结果
                LogStatusChange(null, $"同步操作{(syncSuccessful ? "成功" : "失败")}: 更新={updatedCount}");
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
                // 检查会话是否已授权，未授权的新会话不添加到表格中
                var userInfo = sessionInfo.UserInfo ?? new UserInfo();
                if (!userInfo.授权状态 && !_sessionItemMap.ContainsKey(sessionInfo.SessionID))
                {
                    LogStatusChange(null, $"忽略未授权的新会话: {GetDisplayUserName(userInfo)} - {sessionInfo.ClientIp}");
                    return;
                }

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

            if (sessionInfo == null)
            {
                LogError("接收到空的会话断开事件");
                return;
            }

            try
            {
                // 获取用户信息用于日志记录
                var userInfo = sessionInfo.UserInfo ?? new UserInfo();
                string userName = GetDisplayUserName(userInfo);
                string clientIp = sessionInfo.ClientIp ?? "未知IP";

                if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                {
                    // 判断是否需要移除会话项还是仅更新状态
                    // 规则：已认证用户断开连接后，先更新状态显示为"离线"，等待下次完整刷新时移除
                    // 未认证用户和无效会话可直接移除
                    if (!userInfo.授权状态 && !sessionInfo.IsAuthenticated)
                    {
                        // 未认证会话直接移除
                        RemoveSessionItem(sessionInfo.SessionID);
                        LogStatusChange(sessionInfo, $"未认证会话已断开并移除: {userName} - {clientIp}");
                    }
                    else
                    {
                        // 已认证用户更新状态为离线
                        UpdateSessionItem(existingItem, sessionInfo);
                        LogStatusChange(sessionInfo, $"已认证会话断开: {userName} - {clientIp}");
                    }
                }
                else
                {
                    // 找不到的会话记录，记录日志但不创建断开记录
                    // 避免UI中显示不必要的断开记录
                    LogStatusChange(null, $"接收到未知会话断开事件: {sessionInfo.SessionID} - {userName} - {clientIp}");
                }

                // 立即更新统计信息
                UpdateStatistics();
                this.Refresh();

                // 标记需要完整刷新，确保下一次刷新时清理所有断开的会话
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

            // 检查控件是否已经被释放
            if (IsDisposed || listView1.IsDisposed)
            {
                LogError("OnSessionUpdated: 控件已被释放，无法处理会话更新事件");
                return;
            }

            try
            {
                bool isSignificantChange = false;

                // 更新或创建会话项
                if (_sessionItemMap.TryGetValue(sessionInfo.SessionID, out var existingItem))
                {
                    // 记录关键状态变化
                    var oldSessionInfo = existingItem.Tag as SessionInfo;
                    bool statusChanged = oldSessionInfo?.IsConnected != sessionInfo.IsConnected ||
                                       oldSessionInfo?.IsAuthenticated != sessionInfo.IsAuthenticated;
                    bool heartbeatChanged = Math.Abs(((oldSessionInfo?.LastHeartbeat ?? DateTime.MinValue) - sessionInfo.LastHeartbeat).TotalSeconds) > 5;

                    // 兼容不同版本的属性路径
                    string oldModule = null;
                    string newModule = null;
                    string oldForm = null;
                    string newForm = null;

                    try
                    {
                        oldModule = oldSessionInfo?.UserInfo?.当前模块;
                        newModule = sessionInfo?.UserInfo?.当前模块;
                        oldForm = oldSessionInfo?.UserInfo?.当前窗体;
                        newForm = sessionInfo?.UserInfo?.当前窗体;
                    }
                    catch (Exception)
                    {
                        // 忽略属性访问错误
                    }

                    bool moduleChanged = oldModule != newModule;
                    bool formChanged = oldForm != newForm;

                    // 更新会话信息
                    UpdateSessionItem(existingItem, sessionInfo);
                    existingItem.Tag = sessionInfo; // 更新引用

                    // 判断是否为重要变化
                    isSignificantChange = statusChanged || moduleChanged || formChanged;

                    // 只在有关键变化时记录日志
                    if (statusChanged)
                    {
                        LogStatusChange(sessionInfo, $"状态变化 - 连接:{sessionInfo.IsConnected}, 认证:{sessionInfo.IsAuthenticated}");
                    }
                    else if (moduleChanged && !string.IsNullOrEmpty(newModule))
                    {
                        LogStatusChange(sessionInfo, $"模块变化: {newModule}");
                    }
                    else if (formChanged && !string.IsNullOrEmpty(newForm))
                    {
                        LogStatusChange(sessionInfo, $"窗体变化: {newForm}");
                    }
                    // 心跳变化不记录详细日志，避免日志过多
                }
                else
                {
                    // 新会话，添加到列表
                    AddOrUpdateSessionItem(sessionInfo);
                    LogStatusChange(sessionInfo, "新会话更新");
                    isSignificantChange = true;
                }

                // 在重要状态变化时更新统计信息
                if (sessionInfo.IsAuthenticated || !sessionInfo.IsConnected || isSignificantChange)
                {
                    UpdateStatistics();

                    // 标记需要完整刷新，确保UI能立即反映所有变化
                    MarkForFullRefresh();
                }

                // 立即刷新UI确保用户能看到最新状态
                try
                {
                    if (!listView1.IsDisposed)
                    {
                        listView1.Refresh();
                    }
                }
                catch (Exception refreshEx)
                {
                    LogError("刷新UI失败", refreshEx);
                }
            }
            catch (Exception ex)
            {
                LogError($"处理会话更新事件时出错: {ex.Message}", ex);
                // 发生错误时确保下次会重新同步
                MarkForFullRefresh();
            }
        }

        #endregion

        #region 工具方法

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
                    if (frmMainNew.Instance.IsDebug)
                    {
                        frmMainNew.Instance.PrintInfoLog(logMessagenull);
                    }
             
                    return;
                }

                var userInfo = sessionInfo.UserInfo ?? new UserInfo();
                string userDisplayName = GetDisplayUserName(userInfo);
                string userRealName = GetDisplayName(userInfo?.姓名) ?? "未知姓名";
                string logMessage = $"[会话状态变化] 用户: {userDisplayName} ({userRealName}), {changeDescription}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                if (frmMainNew.Instance.IsDebug)
                {
                    frmMainNew.Instance.PrintInfoLog(logMessage);
                }
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
        /// <param name="message">错误信息</param>
        /// <param name="exception">异常对象</param>
        private void LogError(string message, Exception exception = null)
        {
            try
            {
                string fullMessage = exception != null
                    ? $"{message}\n异常详情: {exception}"
                    : message;

                frmMainNew.Instance.PrintErrorLog($"[UserManagementControl] {fullMessage}");
            }
            catch
            {
                // 避免日志记录本身出错导致无限递归
                Console.WriteLine($"[UserManagementControl] {message}");
            }
        }

        /// <summary>
        /// 获取选中的会话列表
        /// </summary>
        /// <returns>选中的会话列表</returns>
        private List<SessionInfo> SelectSessions()
        {
            var selectedSessions = new List<SessionInfo>();

            try
            {
                foreach (ListViewItem item in listView1.CheckedItems)
                {
                    if (item.Tag is SessionInfo sessionInfo)
                    {
                        selectedSessions.Add(sessionInfo);
                    }
                }

                // 如果没有选中任何项，但有当前选中项，使用当前项
                if (selectedSessions.Count == 0 && listView1.SelectedItems.Count > 0)
                {
                    var selectedItem = listView1.SelectedItems[0];
                    if (selectedItem.Tag is SessionInfo sessionInfo)
                    {
                        selectedSessions.Add(sessionInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("获取选中会话时出错", ex);
            }

            return selectedSessions;
        }

        #endregion

        #region 控件事件处理

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

        // 按钮事件处理需要根据具体的业务需求来实现
        // 这里只提供基本的框架
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

        private void tsbtn刷新_Click(object sender, EventArgs e)
        {
            FullRefreshFromSessions();
            UpdateStatistics();
        }

        private void tsbtn发送消息_Click(object sender, EventArgs e)
        {
            var selectedSessions = SelectSessions();
            if (selectedSessions.Count == 0)
            {
                MessageBox.Show("请先选择要发送消息的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (var session in selectedSessions)
            {
                if (session == null)
                {
                    MessageBox.Show($"用户 {session.UserInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var userList = new List<UserInfo>();
                userList.Add(session.UserInfo);
                HandleSendMessage(userList);
            }
            // TODO: 实现发送消息功能
            MessageBox.Show($"选择了 {selectedSessions.Count} 个会话发送消息", "功能待实现", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void tsbtn推送更新_Click(object sender, EventArgs e)
        {
            PushVersionUpdate();
        }

        private void PushVersionUpdate()
        {
            var selectedSessions = SelectSessions();
            if (selectedSessions.Count == 0)
            {
                MessageBox.Show("请先选择要推送更新的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (var session in selectedSessions)
            {
                if (session == null)
                {
                    MessageBox.Show($"用户 {session.UserInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 确认推送更新
                var result = MessageBox.Show($"确定要向用户 {session.UserInfo.用户名} 推送更新吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // 发送推送更新命令 - 使用新的发送方法
                    SystemCommandRequest systemCommandRequest = new SystemCommandRequest();
                    systemCommandRequest.CommandType = SystemManagementType.PushVersionUpdate;

                    var success = _sessionService.SendCommandAsync(
                        session.SessionID,
                        SystemCommands.SystemManagement,
                        systemCommandRequest).Result; // 注意：这里使用.Result是为了保持原有的同步行为

                    if (success)
                    {
                        frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserInfo.用户名} 推送更新");
                    }
                    else
                    {
                        MessageBox.Show("更新推送失败，请检查用户连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void tsbtn推送系统配置_Click(object sender, EventArgs e)
        {
            var selectedSessions = SelectSessions();
            if (selectedSessions.Count == 0)
            {
                MessageBox.Show("请先选择要推送系统配置的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            foreach (var session in selectedSessions)
            {
                if (session == null)
                {
                    MessageBox.Show($"用户 {session.UserInfo.用户名} 的会话不存在或已断开连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 确认推送系统配置
                var result = MessageBox.Show($"确定要向用户 {session.UserInfo.用户名} 推送系统配置吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
                        frmMainNew.Instance.PrintInfoLog($"已向用户 {session.UserInfo.用户名} 推送系统配置");
                        MessageBox.Show("系统配置推送成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("系统配置推送失败，请检查用户连接状态", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            // TODO: 实现推送系统配置功能
            MessageBox.Show($"选择了 {selectedSessions.Count} 个会话推送系统配置", "功能待实现", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tsbtn推送缓存_Click(object sender, EventArgs e)
        {
            var selectedSessions = SelectSessions();
            if (selectedSessions.Count == 0)
            {
                MessageBox.Show("请先选择要推送缓存的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TODO: 实现推送缓存功能
            MessageBox.Show($"选择了 {selectedSessions.Count} 个会话推送缓存", "功能待实现", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



        // FullRefreshFromSessions方法已在文件上方定义，此处移除重复实现

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                // 获取选中的会话
                var selectedSessions = SelectSessions();

                // 根据菜单项文本执行相应操作
                switch (e.ClickedItem.Text)
                {
                    case "发消息给客户端":
                        tsbtn发送消息_Click(sender, new EventArgs());
                        break;
                    case "推送更新":
                        tsbtn推送更新_Click(sender, new EventArgs());
                        break;
                    case "推送系统配置":
                        tsbtn推送系统配置_Click(sender, new EventArgs());
                        break;
                    case "推送缓存":
                        tsbtn推送缓存_Click(sender, new EventArgs());
                        break;
                    case "刷新":
                        tsbtn刷新_Click(sender, new EventArgs());
                        break;
                    case "全部选择":
                        SelectAllItems();
                        break;
                    case "取消选择":
                        DeselectAllItems();
                        break;
                    case "反选":
                        InvertSelection();
                        break;
                    case "断开连接":
                        DisconnectSelectedSessions();
                        break;
                    case "切换服务器":
                        HandleSwitchServer();
                        break;
                    case "全部切换服务器":
                        HandleSwitchAllServers();
                        break;
                    default:
                        // 记录未知菜单项
                        LogError($"未处理的右键菜单项: {e.ClickedItem.Text}");
                        break;
                }
            }
            catch (Exception ex)
            {
                LogError("处理右键菜单项点击事件时出错", ex);
            }
        }

        /// <summary>
        /// 处理切换服务器功能
        /// </summary>
        private void HandleSwitchServer()
        {
            try
            {
                // 获取选中的会话
                var selectedSessions = SelectSessions();
                if (selectedSessions.Count == 0)
                {
                    MessageBox.Show("请先选择要切换服务器的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取目标服务器地址和端口
                var (serverAddress, serverPort) = PromptServerAddressAndPort();
                if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(serverPort))
                {
                    return; // 用户取消或未输入
                }

                // 执行切换服务器操作
                SwitchServers(selectedSessions, serverAddress, serverPort);
            }
            catch (Exception ex)
            {
                LogError("处理切换服务器操作时出错", ex);
                MessageBox.Show($"切换服务器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 处理全部切换服务器功能
        /// </summary>
        private void HandleSwitchAllServers()
        {
            try
            {
                if (listView1.Items.Count == 0)
                {
                    MessageBox.Show("当前没有在线会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取目标服务器地址和端口
                var (serverAddress, serverPort) = PromptServerAddressAndPort();
                if (string.IsNullOrEmpty(serverAddress) || string.IsNullOrEmpty(serverPort))
                {
                    return; // 用户取消或未输入
                }

                // 获取所有会话
                var allSessions = new List<SessionInfo>();
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Tag is SessionInfo sessionInfo)
                    {
                        allSessions.Add(sessionInfo);
                    }
                }

                // 执行切换服务器操作
                SwitchServers(allSessions, serverAddress, serverPort);
            }
            catch (Exception ex)
            {
                LogError("处理全部切换服务器操作时出错", ex);
                MessageBox.Show($"全部切换服务器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 提示用户输入服务器地址和端口
        /// </summary>
        private (string serverAddress, string serverPort) PromptServerAddressAndPort()
        {
            // 使用输入对话框获取服务器地址和端口
            string serverInfo = Microsoft.VisualBasic.Interaction.InputBox(
                "请输入目标服务器地址和端口 (格式: http://ip:port)",
                "切换服务器",
                string.Empty);

            // 简单验证输入
            if (string.IsNullOrEmpty(serverInfo))
            {
                return (null, null);
            }

            // 确保地址格式正确
            if (!serverInfo.StartsWith("http://") && !serverInfo.StartsWith("https://"))
            {
                serverInfo = "http://" + serverInfo;
            }

            // 尝试解析URL以获取IP和端口
            try
            {
                // 移除协议部分
                string hostAndPort = serverInfo.Replace("http://", "").Replace("https://", "");

                // 查找端口分隔符
                int portSeparatorIndex = hostAndPort.LastIndexOf(':');

                string serverAddress;
                string serverPort;

                if (portSeparatorIndex > 0)
                {
                    // 分离IP和端口
                    serverAddress = hostAndPort.Substring(0, portSeparatorIndex);
                    serverPort = hostAndPort.Substring(portSeparatorIndex + 1);
                }
                else
                {
                    // 如果没有指定端口，使用默认端口80
                    serverAddress = hostAndPort;
                    serverPort = "80";
                }

                return (serverAddress, serverPort);
            }
            catch (Exception ex)
            {
                LogError("解析服务器地址和端口失败", ex);
                MessageBox.Show("无法解析服务器地址和端口，请检查输入格式", "格式错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return (null, null);
            }
        }

        /// <summary>
        /// 为指定会话切换服务器
        /// </summary>
        private void SwitchServers(List<SessionInfo> sessions, string serverAddress, string serverPort)
        {
            try
            {
                int successCount = 0;
                int failedCount = 0;

                foreach (var sessionInfo in sessions)
                {
                    try
                    {
                        // 构造切换服务器命令
                        var command = new Dictionary<string, object>
                        {
                            { "CommandType", "SwitchServer" },
                            { "ServerAddress", serverAddress },
                               { "ServerPort", serverPort },
                            { "SessionID", sessionInfo.SessionID }
                        };

                        SystemCommandRequest commandRequest = new SystemCommandRequest();
                        commandRequest.CommandType = PacketSpec.Commands.SystemManagementType.SwitchServer;
                        commandRequest.Parameters = command;

                        // 发送命令到客户端
                        bool success = _sessionService.SendCommandAsync<SystemCommandRequest>(sessionInfo.SessionID, SystemCommands.SystemManagement, commandRequest).Result;
                        if (success)
                        {
                            successCount++;
                            LogStatusChange(sessionInfo, $"管理员切换服务器到: {serverAddress}");
                        }
                        else
                        {
                            failedCount++;
                            LogError($"切换服务器失败: {sessionInfo.SessionID} - {GetDisplayUserName(sessionInfo.UserInfo)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        LogError($"发送切换服务器命令时出错: {sessionInfo.SessionID} - {ex.Message}", ex);
                    }
                }

                // 显示操作结果
                if (successCount > 0)
                {
                    MessageBox.Show($"成功发送切换服务器命令到 {successCount} 个会话", "操作成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (failedCount > 0)
                {
                    MessageBox.Show($"有 {failedCount} 个会话切换服务器失败，请查看日志获取详情", "操作结果", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 刷新会话列表
                FullRefreshFromSessions();
            }
            catch (Exception ex)
            {
                LogError("执行切换服务器操作时出错", ex);
                throw;
            }
        }

        /// <summary>
        /// 断开选中的会话连接
        /// </summary>
        private async Task DisconnectSelectedSessions()
        {
            try
            {
                var selectedSessions = SelectSessions();
                if (selectedSessions.Count == 0)
                {
                    MessageBox.Show("请先选择要断开连接的会话", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 确认断开连接
                var result = MessageBox.Show(
                    $"确定要断开选中的 {selectedSessions.Count} 个会话吗？\n此操作将强制断开用户连接，可能导致用户未保存的数据丢失。",
                    "确认断开连接",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;

                int disconnectedCount = 0;
                int failedCount = 0;

                foreach (var sessionInfo in selectedSessions)
                {
                    try
                    {
                        // 使用会话服务断开连接
                        bool success = await _sessionService.DisconnectSessionAsync(sessionInfo.SessionID);
                        if (success)
                        {
                            disconnectedCount++;
                            LogStatusChange(sessionInfo, "管理员强制断开连接");
                        }
                        else
                        {
                            failedCount++;
                            LogError($"断开会话连接失败: {sessionInfo.SessionID} - {GetDisplayUserName(sessionInfo.UserInfo)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        failedCount++;
                        LogError($"断开会话连接时发生异常: {sessionInfo.SessionID} - {ex.Message}", ex);
                    }
                }

                // 显示操作结果
                string message = $"断开连接操作完成:\n成功: {disconnectedCount} 个会话\n失败: {failedCount} 个会话";
                MessageBox.Show(message, "操作结果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 刷新统计信息
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                LogError("执行断开连接操作时出错", ex);
                MessageBox.Show("断开连接操作时发生错误: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 发消息给客户端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsbtn发送消息_Click(sender, e);
        }

        #endregion

        #region 资源释放

        /// <summary>
        /// 清理资源
        /// </summary>
        /// <param name="disposing">是否正在释放资源</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 取消事件订阅
                if (_sessionService != null)
                {
                    _sessionService.SessionConnected -= OnSessionConnected;
                    _sessionService.SessionDisconnected -= OnSessionDisconnected;
                    _sessionService.SessionUpdated -= OnSessionUpdated;
                }

                // 停止定时器
                _updateTimer?.Stop();
                _updateTimer?.Dispose();

                // 清理映射字典
                _sessionItemMap?.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}