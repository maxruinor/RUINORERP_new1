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
    public partial class UserManagementControl : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly System.Windows.Forms.Timer _updateTimer;
        private readonly Random _random = new Random();

        private readonly ServerMessageService _serverMessageService;
        private readonly ILogger<MessageServiceUsageExample> _logger;

        public ObservableCollection<UserInfo> UserInfos { get; } = new ObservableCollection<UserInfo>();
        private readonly Dictionary<string, ListViewItem> _itemMap = new Dictionary<string, ListViewItem>();
        private DateTime _lastFullUpdate = DateTime.MinValue;
        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        private const int SB_HORZ = 0;
        // 菜单项引用（与设计器关联）
        private System.Windows.Forms.ToolStripMenuItem 切换服务器ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部切换服务器ToolStripMenuItem;

        public UserManagementControl()
        {
            InitializeComponent();
            InitializeListView();

            // 获取新的会话服务实例
            _sessionService = Program.ServiceProvider.GetRequiredService<ISessionService>();
            _serverMessageService = Program.ServiceProvider.GetRequiredService<ServerMessageService>();
            InitializeDataBinding();

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

        private void InitializeDataBinding()
        {
            UserInfos.CollectionChanged += (sender, e) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => HandleCollectionChanged(e)));
                }
                else
                {
                    HandleCollectionChanged(e);
                }
            };
        }

        /// <summary>
        /// 加载所有现有会话
        /// </summary>
        private void LoadAllSessions()
        {
            try
            {
                var sessions = _sessionService.GetAllUserSessions().ToList();
                int newUsersCount = 0;
                int existingUsersCount = 0;

                foreach (var session in sessions)
                {
                    // 检查是否已存在该会话的用户
                    var existingUser = UserInfos.FirstOrDefault(u => u.SessionId == session.SessionID);

                    // 如果通过SessionId找不到，尝试通过客户端IP查找未认证用户
                    if (existingUser == null && string.IsNullOrEmpty(session.UserName))
                    {
                        existingUser = UserInfos.FirstOrDefault(u =>
                            u.用户名.StartsWith("未认证用户_") &&
                            u.客户端IP == session.ClientIp);
                    }

                    if (existingUser != null)
                    {
                        // 用户已存在，更新信息
                        existingUsersCount++;
                        var userInfo = ConvertSessionInfoToUserInfo(session, false);
                        AddOrUpdateUser(userInfo);
                    }
                    else
                    {
                        // 新用户，添加到列表
                        newUsersCount++;
                        var userInfo = ConvertSessionInfoToUserInfo(session);
                        AddOrUpdateUser(userInfo);
                    }
                }

                // 记录加载结果
                if (newUsersCount > 0 || existingUsersCount > 0)
                {
                    LogStatusChange(null, $"初始加载完成：新增 {newUsersCount} 人，更新 {existingUsersCount} 人，当前用户列表 {UserInfos.Count} 人");
                }

                // 初始化统计信息
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                LogError($"加载现有会话时出错: {ex.Message}", ex);
            }
        }

        private void HandleCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        foreach (UserInfo newUser in e.NewItems)
                        {
                            AddOrUpdateUser(newUser);
                            newUser.PropertyChanged += UserInfo_PropertyChanged;
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach (UserInfo oldUser in e.OldItems)
                        {
                            RemoveUser(oldUser);
                            oldUser.PropertyChanged -= UserInfo_PropertyChanged;
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        //FullRefreshListView();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理集合变更时出错: {ex.Message}");
            }
        }

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

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var hitTest = listView1.HitTest(e.Location);
                if (hitTest.Item != null && hitTest.Item.Tag is UserInfo userInfo)
                {
                    // 查找对应的会话信息
                    var session = _sessionService.GetAppSession(userInfo.SessionId);
                    if (session != null)
                    {
                        // 创建并显示增强版会话管理详情窗体
                        var sessionManagementForm = new SessionManagementForm(session as SessionInfo, _sessionService);
                        sessionManagementForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("显示会话性能详情时出错: " + ex.Message);
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

        #region 用户数据管理

        private void AddOrUpdateUser(UserInfo user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddOrUpdateUser(user)));
                return;
            }

            if (_itemMap.TryGetValue(user.SessionId, out var existingItem))
            {
                // 更新现有项
                UpdateListViewItem(existingItem, user);
            }
            else
            {
                // 添加新项
                var newItem = CreateListViewItem(user);
                newItem.Tag = user;
                listView1.Items.Add(newItem);
                _itemMap[user.SessionId] = newItem;

                // 确保用户信息添加到UserInfos集合中
                if (!UserInfos.Any(u => u.SessionId == user.SessionId))
                {
                    UserInfos.Add(user);
                }
            }
        }

        private void RemoveUser(UserInfo user)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => RemoveUser(user)));
                return;
            }

            if (_itemMap.TryGetValue(user.SessionId, out var item))
            {
                listView1.Items.Remove(item);
                _itemMap.Remove(user.SessionId);
            }

            // 从UserInfos集合中移除用户
            var userToRemove = UserInfos.FirstOrDefault(u => u.SessionId == user.SessionId);
            if (userToRemove != null)
            {
                UserInfos.Remove(userToRemove);
            }
        }

        private ListViewItem CreateListViewItem(UserInfo user)
        {
            var item = new ListViewItem
            {
                Tag = user,
                Text = "", // 第一列
            };
            item.ToolTipText = $"Employee_ID: {user.Employee_ID}\nSessionId: {user.SessionId}\nUserID:{user.UserID}";
            //item.SubItems.Add(user.Employee_ID.ToString());
            //item.SubItems.Add(user.SessionId);
            item.SubItems.Add(user.用户名);
            item.SubItems.Add(user.姓名);
            item.SubItems.Add(user.当前模块);
            item.SubItems.Add(user.当前窗体);
            item.SubItems.Add(user.登陆时间.ToString("yy-MM-dd HH:mm:ss"));
            item.SubItems.Add(user.心跳数.ToString());
            item.SubItems.Add(user.最后心跳时间);
            item.SubItems.Add(user.客户端版本);
            item.SubItems.Add(user.客户端IP);
            item.SubItems.Add(user.静止时间.ToString());
            //item.SubItems.Add(user.UserID.ToString());
            item.SubItems.Add(user.超级用户 ? "是" : "否");
            item.SubItems.Add(user.在线状态 ? "在线" : "离线");
            item.SubItems.Add(user.授权状态 ? "已授权" : "未授权");
            item.SubItems.Add(user.操作系统);
            item.SubItems.Add(user.机器名);
            item.SubItems.Add(user.CPU信息);
            item.SubItems.Add(user.内存大小);

            SetItemColorByStatus(item, user.授权状态);
            return item;
        }

        private void UpdateListViewItem(ListViewItem item, UserInfo user)
        {
            if (item.SubItems.Count < 18) return;

            // 更新所有列的数据
            //item.SubItems[1].Text = user.Employee_ID.ToString();
            //item.SubItems[2].Text = user.SessionId;
            item.SubItems[1].Text = user.用户名;
            item.SubItems[2].Text = user.姓名;
            item.SubItems[3].Text = user.当前模块;
            item.SubItems[4].Text = user.当前窗体;
            item.SubItems[5].Text = user.登陆时间.ToString("yy-MM-dd HH:mm:ss");
            item.SubItems[6].Text = user.心跳数.ToString();
            item.SubItems[7].Text = user.最后心跳时间;
            item.SubItems[8].Text = user.客户端版本;
            item.SubItems[9].Text = user.客户端IP;
            item.SubItems[10].Text = user.静止时间.ToString();
            //item.SubItems[11].Text = user.UserID.ToString();
            item.SubItems[11].Text = user.超级用户 ? "是" : "否";
            item.SubItems[12].Text = user.在线状态 ? "在线" : "离线";
            item.SubItems[13].Text = user.授权状态 ? "已授权" : "未授权";
            item.SubItems[14].Text = user.操作系统;
            item.SubItems[15].Text = user.机器名;
            item.SubItems[16].Text = user.CPU信息;
            item.SubItems[17].Text = user.内存大小;

            // 根据综合状态设置颜色（使用在线状态参数，方法内部会重新获取用户信息）
            SetItemColorByStatus(item, user.在线状态);
        }

        /// <summary>
        /// 根据用户状态设置ListView项的视觉样式
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="isOnline">在线状态（此参数保留用于兼容现有调用）</param>
        private void SetItemColorByStatus(ListViewItem item, bool isOnline)
        {
            var userInfo = item.Tag as UserInfo;
            if (userInfo == null) return;

            // 综合状态判断逻辑
            if (!userInfo.在线状态)
            {
                // 离线状态：灰色文字，白色背景
                item.ForeColor = Color.Gray;
                item.BackColor = Color.White;
            }
            else if (userInfo.在线状态 && !userInfo.授权状态)
            {
                // 在线但未授权：橙色文字，浅黄色背景（突出显示需要关注的状态）
                item.ForeColor = Color.DarkOrange;
                item.BackColor = Color.LightYellow;
                item.Font = new Font(item.Font, FontStyle.Bold); // 加粗显示以引起注意
            }
            else if (userInfo.在线状态 && userInfo.授权状态)
            {
                // 在线且已授权：绿色文字，白色背景（正常状态）
                item.ForeColor = Color.Green;
                item.BackColor = Color.White;
                item.Font = new Font(item.Font, FontStyle.Regular);
            }
            else
            {
                // 其他情况：默认样式
                item.ForeColor = Color.Black;
                item.BackColor = Color.White;
                item.Font = new Font(item.Font, FontStyle.Regular);
            }
        }

        public void UserInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is UserInfo user)
            {
                // 高频更新属性使用延迟刷新
                if (e.PropertyName == nameof(UserInfo.心跳数) ||
                    e.PropertyName == nameof(UserInfo.静止时间))
                {
                    if ((DateTime.Now - _lastFullUpdate).TotalMilliseconds > 500)
                    {
                        _lastFullUpdate = DateTime.Now;
                        if (_itemMap.TryGetValue(user.SessionId, out var item))
                        {
                            UpdateListViewItem(item, user);
                        }
                    }
                }
                else // 低频属性使用增量更新
                {
                    if (_itemMap.TryGetValue(user.SessionId, out var item))
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => UpdateSpecificColumn(item, user, e.PropertyName)));
                        }
                        else
                        {
                            UpdateSpecificColumn(item, user, e.PropertyName);
                        }

                        // 如果是状态类属性变化，立即更新统计信息
                        if (e.PropertyName == nameof(UserInfo.在线状态) ||
                            e.PropertyName == nameof(UserInfo.授权状态))
                        {
                            UpdateStatistics();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 增量更新特定列
        /// </summary>
        /// <param name="item">ListView项</param>
        /// <param name="userInfo">用户信息</param>
        /// <param name="propertyName">变更的属性名</param>
        private void UpdateSpecificColumn(ListViewItem item, UserInfo userInfo, string propertyName)
        {
            try
            {
                // 根据属性名映射到对应的列索引并只更新该列
                switch (propertyName)
                {
                    case nameof(UserInfo.在线状态):
                        if (item.SubItems.Count > 0)
                            item.Text = userInfo.在线状态 ? "在线" : "离线";
                        // 更新行颜色 - 传入正确的在线状态参数
                        SetItemColorByStatus(item, userInfo.在线状态);
                        break;
                    case nameof(UserInfo.授权状态):
                        if (item.SubItems.Count > 1)
                            item.SubItems[1].Text = userInfo.授权状态 ? "已授权" : "未授权";
                        // 更新行颜色 - 传入正确的在线状态参数
                        SetItemColorByStatus(item, userInfo.在线状态);
                        break;
                    case nameof(UserInfo.用户名):
                        if (item.SubItems.Count > 2)
                            item.SubItems[2].Text = userInfo.用户名;
                        break;
                    case nameof(UserInfo.姓名):
                        if (item.SubItems.Count > 3)
                            item.SubItems[3].Text = userInfo.姓名;
                        break;
                    case nameof(UserInfo.最后心跳时间):
                        if (item.SubItems.Count > 7)
                            item.SubItems[7].Text = userInfo.最后心跳时间;
                        break;
                    case nameof(UserInfo.当前模块):
                        if (item.SubItems.Count > 8)
                            item.SubItems[8].Text = userInfo.当前模块;
                        break;
                    case nameof(UserInfo.超级用户):
                        if (item.SubItems.Count > 9)
                            item.SubItems[9].Text = userInfo.超级用户 ? "是" : "否";
                        break;
                    // 对于其他属性或未知属性，仍然使用完整更新作为后备
                    default:
                        UpdateListViewItem(item, userInfo);
                        break;
                }
            }
            catch (Exception ex)
            {
                LogError($"更新特定列时出错: {ex.Message}");
                // 出错时回退到完整更新
                UpdateListViewItem(item, userInfo);
            }
        }

        public void UserInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                // 确保在UI线程中更新
                if (InvokeRequired)
                {
                    Invoke(new Action<object, System.Collections.Specialized.NotifyCollectionChangedEventArgs>(UserInfos_CollectionChanged), sender, e);
                    return;
                }

                // 更新虚拟列表大小
                if (listView1.VirtualMode)
                {
                    listView1.VirtualListSize = UserInfos.Count;
                }

                // 优化集合变化处理，增强线程安全性
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (e.NewItems != null)
                        {
                            foreach (var newItem in e.NewItems)
                            {
                                if (newItem is UserInfo userInfo)
                                {
                                    // 避免重复添加和线程安全问题
                                    if (!UserInfos.Contains(userInfo))
                                    {
                                        // 这里应该通过AddOrUpdateUser来添加，而不是直接添加到集合
                                        // UserInfos.Add(userInfo); // 这行代码可能导致无限递归
                                        // 正确的做法是在AddOrUpdateUser中添加到集合
                                        AddOrUpdateUser(userInfo);
                                    }
                                    // 确保事件订阅
                                    userInfo.PropertyChanged += UserInfo_PropertyChanged;
                                }
                            }
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        if (e.OldItems != null)
                        {
                            foreach (var oldItem in e.OldItems)
                            {
                                if (oldItem is UserInfo userInfo)
                                {
                                    // 移除用户并取消事件订阅
                                    RemoveUser(userInfo);
                                    userInfo.PropertyChanged -= UserInfo_PropertyChanged;
                                }
                            }
                        }
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                        // 集合重置时需要清理UI并重新加载
                        ClearUserInterface();
                        // 重新添加所有用户
                        foreach (var userInfo in UserInfos)
                        {
                            AddOrUpdateUser(userInfo);
                            userInfo.PropertyChanged += UserInfo_PropertyChanged;
                        }
                        MarkForFullRefresh(); // 重置时标记需要完整刷新
                        break;
                }

                // 更新虚拟列表大小（确保正确反映）
                if (listView1.VirtualMode)
                {
                    listView1.VirtualListSize = UserInfos.Count;
                }

                // 集合变化时更新统计信息
                UpdateStatistics();

            }
            catch (Exception ex)
            {
                LogError("UserInfos_CollectionChanged时出错: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 清理用户界面，移除所有项
        /// </summary>
        private void ClearUserInterface()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearUserInterface));
                return;
            }

            // 清理列表视图
            if (listView1.VirtualMode)
            {
                listView1.VirtualListSize = 0;
            }
            else
            {
                listView1.Items.Clear();
            }

            // 清理映射字典
            _itemMap.Clear();
        }

        #endregion

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 优化更新策略: 减少不必要的轮询，增加事件驱动更新

                // 非关键统计信息降低到每2秒更新一次
                if (DateTime.Now.Second % 2 == 0)
                {
                    UpdateStatistics();
                }

                // 完整刷新和清理保持30秒，但减少全量刷新频率，更多依赖事件驱动
                if (DateTime.Now.Second % 30 == 0)
                {
                    CleanupInactiveUsers();
                    // 仅在需要时执行完整刷新，例如当检测到可能的数据不一致时
                    if (_needsFullRefresh)
                    {
                        FullRefreshFromSessions();
                        _needsFullRefresh = false; // 重置标志
                    }
                }

                // 用户状态检查保持10秒间隔
                if (DateTime.Now.Second % 10 == 0)
                {
                    UpdateUserStatuses();
                }

                // 心跳和空闲时间更新优化：只更新可见项，降低更新频率
                if (DateTime.Now.Second % 3 == 0) // 从每秒改为每3秒
                {
                    UpdateVisibleItemsHeartbeatAndIdleTime();
                }
            }
            catch (Exception ex)
            {
                LogError("定时器更新时出错", ex);
            }
        }

        // 标记是否需要完整刷新的标志
        private bool _needsFullRefresh = false;

        // 当检测到数据不一致或需要强制刷新时调用
        private void MarkForFullRefresh()
        {
            _needsFullRefresh = true;
        }

        /// <summary>
        /// 更新心跳时间和静止时间显示 - 优化版，只更新可见项
        /// </summary>
        private void UpdateHeartbeatAndIdleTime()
        {
            try
            {
                foreach (var item in listView1.Items)
                {
                    if (item is ListViewItem listItem && listItem.Tag is UserInfo userInfo)
                    {
                        // 更新静止时间显示
                        if (!string.IsNullOrEmpty(userInfo.最后心跳时间))
                        {
                            var lastHeartbeat = userInfo.最后心跳时间.ObjToDate();
                            var idleTime = DateTime.Now - lastHeartbeat;
                            listItem.SubItems[10].Text = FormatIdleTime(idleTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("更新心跳时间显示时出错", ex);
            }
        }

        /// <summary>
        /// 仅更新可见项的心跳和空闲时间 - 性能优化版
        /// </summary>
        private void UpdateVisibleItemsHeartbeatAndIdleTime()
        {
            try
            {
                if (listView1.Items.Count == 0) return;

                // 检查是否支持虚拟模式，如果支持则更新可见项
                if (listView1.VirtualMode)
                {
                    // 在虚拟模式下，只更新当前可见的项目范围
                    int firstVisible = listView1.TopItem?.Index ?? 0;
                    // 估算可见项数量，避免使用不存在的ItemHeight属性
                    int estimatedItemHeight = 24; // 假设每项高度为24像素
                    int visibleCount = Math.Min(listView1.DisplayRectangle.Height / estimatedItemHeight, listView1.VirtualListSize);
                    int lastVisible = Math.Min(firstVisible + visibleCount + 5, listView1.VirtualListSize - 1); // 多更新一些作为缓冲区

                    // 遍历可见范围内的项目
                    for (int i = firstVisible; i <= lastVisible; i++)
                    {
                        // 查找对应的用户信息并更新 - 优化查找逻辑
                        var userInfo = UserInfos.FirstOrDefault(u => u.SessionId != null && _itemMap.TryGetValue(u.SessionId, out var item) && listView1.Items.IndexOf(item) == i);
                        if (userInfo != null)
                        {
                            UpdateIdleTimeForUser(userInfo);
                        }
                    }
                }
                else
                {
                    // 非虚拟模式下，使用GetItemAt来获取可见项
                    foreach (ListViewItem item in listView1.Items)
                    {
                        // 检查项目是否可见
                        if (listView1.ClientRectangle.IntersectsWith(item.Bounds))
                        {
                            if (item.Tag is UserInfo userInfo && !string.IsNullOrEmpty(userInfo.最后心跳时间))
                            {
                                var lastHeartbeat = userInfo.最后心跳时间.ObjToDate();
                                var idleTime = DateTime.Now - lastHeartbeat;
                                item.SubItems[10].Text = FormatIdleTime(idleTime);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("更新可见项心跳时间显示时出错", ex);
            }
        }

        /// <summary>
        /// 为指定用户更新空闲时间 - 辅助方法
        /// </summary>
        private void UpdateIdleTimeForUser(UserInfo userInfo)
        {
            if (userInfo == null || string.IsNullOrEmpty(userInfo.最后心跳时间)) return;

            // 查找对应的ListViewItem
            if (userInfo.SessionId != null && _itemMap.TryGetValue(userInfo.SessionId, out var item))
            {
                if (item != null && item.SubItems.Count > 10)
                {
                    var lastHeartbeat = userInfo.最后心跳时间.ObjToDate();
                    var idleTime = DateTime.Now - lastHeartbeat;
                    item.SubItems[10].Text = FormatIdleTime(idleTime);
                }
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
                Console.WriteLine($"更新统计信息时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 从会话服务完整刷新用户列表
        /// </summary>
        private void FullRefreshFromSessions()
        {
            try
            {
                // 获取所有当前会话
                var currentSessions = _sessionService.GetAllUserSessions().ToList();
                var currentSessionDict = currentSessions.ToDictionary(s => s.SessionID, s => s);
                var processedSessionIds = new HashSet<string>();
                int newUsersCount = 0;
                int updatedUsersCount = 0;

                // 更新现有用户或添加新用户
                foreach (var sessionInfo in currentSessions)
                {
                    var userInfo = ConvertSessionInfoToUserInfo(sessionInfo, false); // 不记录添加日志

                    // 检查是否是新用户
                    bool isNewUser = !UserInfos.Any(u => u.SessionId == sessionInfo.SessionID);
                    AddOrUpdateUser(userInfo);

                    if (isNewUser)
                    {
                        newUsersCount++;
                        LogStatusChange(userInfo, $"完整刷新发现新用户 - 在线状态: {userInfo.在线状态}, 授权状态: {userInfo.授权状态}");
                    }
                    else
                    {
                        updatedUsersCount++;
                    }

                    processedSessionIds.Add(sessionInfo.SessionID);
                }

                // 处理已不存在的会话（标记为离线）
                int offlineUsersCount = 0;
                foreach (var userInfo in UserInfos.ToList())
                {
                    if (!processedSessionIds.Contains(userInfo.SessionId))
                    {
                        if (userInfo.在线状态)
                        {
                            userInfo.在线状态 = false;
                            userInfo.最后心跳时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            LogStatusChange(userInfo, "完整刷新 - 会话不存在，标记为离线");
                            offlineUsersCount++;
                        }
                    }
                }

                // 只在有变化时记录总结日志
                if (newUsersCount > 0 || offlineUsersCount > 0)
                {
                    LogStatusChange(null, $"完整刷新完成：新增 {newUsersCount} 人，更新 {updatedUsersCount} 人，标记离线 {offlineUsersCount} 人，当前用户列表 {UserInfos.Count} 人");
                }
            }
            catch (Exception ex)
            {
                LogError("完整刷新用户列表时出错", ex);
            }
        }

        /// <summary>
        /// 更新所有用户的状态信息
        /// </summary>
        private void UpdateUserStatuses()
        {
            try
            {
                // 获取所有当前会话的最新状态
                var currentSessions = _sessionService.GetAllUserSessions().ToList();
                var currentSessionDict = currentSessions.ToDictionary(s => s.SessionID, s => s);
                int statusChangesCount = 0;

                // 更新现有用户的显示状态
                foreach (var userInfo in UserInfos.ToList())
                {
                    if (currentSessionDict.TryGetValue(userInfo.SessionId, out var sessionInfo))
                    {
                        // 会话仍然存在，更新状态（不记录添加日志）
                        var updatedUserInfo = ConvertSessionInfoToUserInfo(sessionInfo, false);

                        // 检查是否有实际的状态变化
                        bool onlineStatusChanged = userInfo.在线状态 != updatedUserInfo.在线状态;
                        bool authStatusChanged = userInfo.授权状态 != updatedUserInfo.授权状态;
                        bool anyModuleChanged = userInfo.当前模块 != updatedUserInfo.当前模块;
                        bool anyFormChanged = userInfo.当前窗体 != updatedUserInfo.当前窗体;

                        if (onlineStatusChanged || authStatusChanged || anyModuleChanged || anyFormChanged)
                        {
                            AddOrUpdateUser(updatedUserInfo);
                            statusChangesCount++;

                            // 只在有实际变化时记录状态变化
                            if (onlineStatusChanged)
                            {
                                LogStatusChange(userInfo, $"在线状态: {userInfo.在线状态} -> {updatedUserInfo.在线状态}");
                            }
                            if (authStatusChanged)
                            {
                                LogStatusChange(userInfo, $"授权状态: {userInfo.授权状态} -> {updatedUserInfo.授权状态}");
                            }
                            if (anyModuleChanged)
                            {
                                LogStatusChange(userInfo, $"模块变化: {userInfo.当前模块} -> {updatedUserInfo.当前模块}");
                            }
                        }
                    }
                    else
                    {
                        // 会话已不存在，正确标记为离线状态而不是删除用户
                        if (userInfo.在线状态)
                        {
                            userInfo.在线状态 = false;
                            userInfo.最后心跳时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            LogStatusChange(userInfo, "会话断开，标记为离线");
                            statusChangesCount++;
                        }
                    }
                }

                // 只在有状态变化时记录总结
                if (statusChangesCount > 0)
                {
                    LogStatusChange(null, $"状态更新完成，共更新 {statusChangesCount} 个用户的状态");
                }
            }
            catch (Exception ex)
            {
                LogError($"更新用户状态时出错: {ex.Message}");
            }
        }

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

            if (sessionInfo != null)
            {
                // 首先检查是否已存在该用户，避免重复添加
                var existingUser = UserInfos.FirstOrDefault(u => u.SessionId == sessionInfo.SessionID);

                // 如果通过SessionId找不到，尝试通过客户端IP查找未认证用户
                if (existingUser == null && string.IsNullOrEmpty(sessionInfo.UserName))
                {
                    existingUser = UserInfos.FirstOrDefault(u =>
                        u.用户名.StartsWith("未认证用户_") &&
                        u.客户端IP == sessionInfo.ClientIp);
                }

                if (existingUser != null)
                {
                    // 用户已存在，更新状态而不是重新添加
                    existingUser.在线状态 = sessionInfo.IsConnected;
                    existingUser.最后心跳时间 = sessionInfo.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
                    existingUser.授权状态 = sessionInfo.IsAuthenticated; // 确保授权状态同步更新

                    // 记录连接状态更新
                    string statusDescription = sessionInfo.IsAuthenticated ? "已连接且已授权" : "已连接但未授权";
                    LogStatusChange(existingUser, $"会话重新连接 - {statusDescription}");
                }
                else
                {
                    // 新用户，添加到列表
                    var userInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                    AddOrUpdateUser(userInfo);

                    // 记录连接状态
                    string statusDescription = sessionInfo.IsAuthenticated ? "已连接且已授权" : "已连接但未授权";
                    LogStatusChange(userInfo, $"会话连接 - {statusDescription}");
                }

                // 立即更新统计信息，确保UI及时反映连接状态变化
                UpdateStatistics();

                // 在特定情况下标记需要完整刷新
                if (sessionInfo.IsConnected && sessionInfo.IsAuthenticated) // 重要状态变化时
                {
                    MarkForFullRefresh();
                }
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

            if (sessionInfo != null)
            {
                // 正确更新断开连接的用户状态，而不是删除用户
                var userToUpdate = UserInfos.FirstOrDefault(u => u.SessionId == sessionInfo.SessionID);
                if (userToUpdate != null)
                {
                    userToUpdate.在线状态 = false;
                    userToUpdate.最后心跳时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    LogStatusChange(userToUpdate, $"会话断开事件触发");
                }

                // 立即更新统计信息，确保UI及时反映断开状态变化
                UpdateStatistics();
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

            if (sessionInfo != null)
            {
                // 使用多种方式查找现有用户，避免键值不匹配导致的重复添加
                var existingUser = UserInfos.FirstOrDefault(u => u.SessionId == sessionInfo.SessionID);

                // 如果通过SessionId找不到，尝试通过客户端IP查找未认证用户
                if (existingUser == null && string.IsNullOrEmpty(sessionInfo.UserName))
                {
                    existingUser = UserInfos.FirstOrDefault(u =>
                        u.用户名.StartsWith("未认证用户_") &&
                        u.客户端IP == sessionInfo.ClientIp);
                }

                if (existingUser != null)
                {
                    // 记录状态变化
                    bool onlineStatusChanged = existingUser.在线状态 != sessionInfo.IsConnected;
                    bool authStatusChanged = existingUser.授权状态 != sessionInfo.IsAuthenticated;

                    // 更新用户信息
                    existingUser.最后心跳时间 = sessionInfo.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss");
                    existingUser.在线状态 = sessionInfo.IsConnected;
                    existingUser.授权状态 = sessionInfo.IsAuthenticated;
                    existingUser.当前模块 = sessionInfo.Properties?.ContainsKey("CurrentModule") == true ?
                                         sessionInfo.Properties["CurrentModule"]?.ToString() : existingUser.当前模块;

                    // 更新SessionId（如果需要）
                    if (existingUser.SessionId != sessionInfo.SessionID)
                    {
                        existingUser.SessionId = sessionInfo.SessionID;
                        // SessionId变更可能导致数据不一致，标记需要完整刷新
                        MarkForFullRefresh();
                    }

                    // 记录状态变化日志
                    if (onlineStatusChanged || authStatusChanged)
                    {
                        string changeType = string.Empty;
                        if (onlineStatusChanged) changeType += $"在线状态: {existingUser.在线状态} -> {sessionInfo.IsConnected}";
                        if (authStatusChanged) changeType += (changeType.Length > 0 ? ", " : "") + $"授权状态: {existingUser.授权状态} -> {sessionInfo.IsAuthenticated}";
                        LogStatusChange(existingUser, $"会话更新 - {changeType}");

                        // 重要状态变化时标记需要完整刷新
                        MarkForFullRefresh();
                    }
                }
                else
                {
                    // 新用户，无论是否认证都添加到列表（正确处理未授权状态）
                    var userInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                    AddOrUpdateUser(userInfo);

                    // 新用户加入可能导致数据不一致，标记需要检查
                    MarkForFullRefresh();
                }

                // 立即更新统计信息，确保UI及时反映状态变化
                UpdateStatistics();
            }
        }



        /// <summary>
        /// 将SessionInfo转换为UserInfo
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <param name="logAddition">是否记录添加日志，默认为true</param>
        /// <returns>用户信息</returns>
        private UserInfo ConvertSessionInfoToUserInfo(SessionInfo sessionInfo, bool logAddition = true)
        {
            var userInfo = new UserInfo
            {
                SessionId = sessionInfo.SessionID,
                用户名 = !string.IsNullOrEmpty(sessionInfo.UserName) ? sessionInfo.UserName : $"未认证用户_{sessionInfo.SessionID.Substring(0, 8)}",
                登陆时间 = sessionInfo.LoginTime ?? sessionInfo.ConnectedTime,
                最后心跳时间 = sessionInfo.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss"),
                客户端IP = sessionInfo.ClientIp,
                当前模块 = sessionInfo.Properties?.ContainsKey("CurrentModule") == true ?
                         sessionInfo.Properties["CurrentModule"]?.ToString() : "未知",
                客户端版本 = sessionInfo.ClientVersion ?? "未知",
                在线状态 = sessionInfo.IsConnected, // 使用会话的连接状态
                授权状态 = sessionInfo.IsAuthenticated // 使用会话的认证状态
            };

            // 如果有用户详细信息，则填充
            if (sessionInfo.UserInfo != null)
            {
                userInfo.姓名 = sessionInfo.UserInfo.姓名;
                userInfo.超级用户 = sessionInfo.UserInfo.超级用户;
                userInfo.UserID = sessionInfo.UserInfo.UserID;
                userInfo.Employee_ID = sessionInfo.UserInfo.Employee_ID;
            }

            // 如果有客户端系统信息，则填充相关字段
            if (sessionInfo.ClientSystemInfo != null)
            {
                userInfo.操作系统 = $"{sessionInfo.ClientSystemInfo.OSName} {sessionInfo.ClientSystemInfo.OSVersion}";
                userInfo.机器名 = sessionInfo.ClientSystemInfo.MachineName;
                userInfo.CPU信息 = sessionInfo.ClientSystemInfo.CPUInfo;
                userInfo.内存大小 = $"{sessionInfo.ClientSystemInfo.TotalMemory / (1024 * 1024 * 1024)}GB";
            }

            // 只在需要时记录新用户添加
            if (logAddition)
            {
                LogStatusChange(userInfo, $"用户添加 - 在线状态: {userInfo.在线状态}, 授权状态: {userInfo.授权状态}");
            }

            return userInfo;
        }

        #endregion

        private List<UserInfo> SelectUser()
        {
            // 获取选中的用户
            var selectedUsers = new List<UserInfo>();

            // 先检查是否有复选框选中的用户
            foreach (ListViewItem item in listView1.CheckedItems)
            {
                if (item.Tag is UserInfo userInfo)
                {
                    var user = UserInfos.FirstOrDefault(u => u.SessionId == userInfo.SessionId);
                    if (user != null)
                    {
                        selectedUsers.Add(user);
                    }
                }
            }

            // 如果没有复选框选中的用户，则使用单选的用户
            if (selectedUsers.Count == 0 && listView1.SelectedItems.Count > 0)
            {
                var selectedItem = listView1.SelectedItems[0];
                if (selectedItem.Tag is UserInfo userInfo)
                {
                    var user = UserInfos.FirstOrDefault(u => u.SessionId == userInfo.SessionId);
                    if (user != null)
                    {
                        selectedUsers.Add(user);
                    }
                }
            }

            return selectedUsers;
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
        /// <param name="userInfo">用户信息</param>
        /// <param name="changeDescription">变化描述</param>
        private void LogStatusChange(UserInfo userInfo, string changeDescription)
        {
            try
            {
                // 添加空值检查，防止userInfo为null导致错误
                if (userInfo == null)
                {
                    string logMessagenull = $"[用户状态变化] 用户信息为空, {changeDescription}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    frmMainNew.Instance.PrintInfoLog(logMessagenull);
                    return;
                }

                // 更新默认用户名处理逻辑，与ConvertSessionInfoToUserInfo保持一致
                string userDisplayName;
                if (string.IsNullOrEmpty(userInfo.用户名))
                {
                    userDisplayName = $"未认证用户_{userInfo.SessionId.Substring(0, 8)}";
                }
                else
                {
                    userDisplayName = userInfo.用户名;
                }

                string userRealName = !string.IsNullOrEmpty(userInfo.姓名) ? userInfo.姓名 : "未知姓名";
                string logMessage = $"[用户状态变化] 用户: {userDisplayName} ({userRealName}), {changeDescription}, 时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                frmMainNew.Instance.PrintInfoLog(logMessage);
            }
            catch (Exception ex)
            {
                LogError($"记录状态变化日志时出错: {ex.Message}", ex);
            }
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
