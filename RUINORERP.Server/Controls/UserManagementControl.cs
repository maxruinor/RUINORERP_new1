using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
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
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model.Base;

namespace RUINORERP.Server.Controls
{
    public partial class UserManagementControl : UserControl
    {
        private readonly ISessionService _sessionService;
        private readonly Timer _updateTimer;
        private readonly Random _random = new Random();
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

            InitializeDataBinding();

            // 订阅会话服务事件
            _sessionService.SessionConnected += OnSessionConnected;
            _sessionService.SessionDisconnected += OnSessionDisconnected;
            _sessionService.SessionUpdated += OnSessionUpdated;

            // 初始化时加载所有现有会话
            LoadAllSessions();

            // 设置定时器用于UI刷新
            _updateTimer = new Timer { Interval = 1000 };
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
                foreach (var session in sessions)
                {
                    var userInfo = ConvertSessionInfoToUserInfo(session);
                    AddOrUpdateUser(userInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载现有会话时出错: {ex.Message}");
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
                        // 创建并显示会话性能详情窗体
                        var sessionPerformanceForm = new SessionPerformanceForm(session as SessionInfo);
                        sessionPerformanceForm.Show();
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

            SetItemColorByStatus(item, user.授权状态);
            return item;
        }

        private void UpdateListViewItem(ListViewItem item, UserInfo user)
        {
            if (item.SubItems.Count < 14) return;

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

            SetItemColorByStatus(item, user.授权状态);
        }

        private void SetItemColorByStatus(ListViewItem item, bool isOnline)
        {
            item.ForeColor = isOnline ? Color.Green : Color.Gray;
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
                else // 低频属性立即更新
                {
                    if (_itemMap.TryGetValue(user.SessionId, out var item))
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new Action(() => UpdateListViewItem(item, user)));
                        }
                        else
                        {
                            UpdateListViewItem(item, user);
                        }
                    }
                }
            }
        }

        public void UserInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(() => listView1.VirtualListSize = UserInfos.Count);
                }
                else
                {
                    listView1.VirtualListSize = UserInfos.Count;
                }

                // 在这里处理集合变化的逻辑
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count == 1)
                        {
                            UserInfo newItem = e.NewItems[0] as UserInfo;
                            if (!UserInfos.Contains(newItem))
                            {
                                UserInfos.Add(newItem);                 // 处理删除元素的逻辑
                            }
                        }
                        listView1.VirtualListSize = UserInfos.Count;
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        foreach (UserInfo oldUser in e.OldItems)
                        {
                            RemoveUser(oldUser);
                            oldUser.PropertyChanged -= UserInfo_PropertyChanged;
                        }
                        listView1.VirtualListSize = UserInfos.Count;
                        break;
                    default:
                        // return;
                        break;
                        // 可以根据需要处理其他事件类型
                }

                //如果listview中的数据不存在于UserInfos中。UI上也要移除

            }
            catch (Exception ex)
            {
                Console.WriteLine("UserInfos_CollectionChanged时出错" + ex.Message);
            }
        }

        #endregion

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // 每分钟执行一次完整刷新
            if (DateTime.Now.Minute % 1 == 0 && DateTime.Now.Second == 0)
            {
                //FullRefreshListView();
                CleanupInactiveUsers();
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

            if (sessionInfo != null && sessionInfo.IsAuthenticated)
            {
                var userInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                AddOrUpdateUser(userInfo);
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
                var userInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                // 直接从UI中移除用户
                RemoveUser(userInfo);
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

            if (sessionInfo != null && sessionInfo.IsAuthenticated)
            {
                var userInfo = ConvertSessionInfoToUserInfo(sessionInfo);
                AddOrUpdateUser(userInfo);
            }
        }



        /// <summary>
        /// 将SessionInfo转换为UserInfo
        /// </summary>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>用户信息</returns>
        private UserInfo ConvertSessionInfoToUserInfo(SessionInfo sessionInfo)
        {
            var userInfo = new UserInfo
            {
                SessionId = sessionInfo.SessionID,
                用户名 = sessionInfo.UserName,
                登陆时间 = sessionInfo.LoginTime ?? sessionInfo.ConnectedTime,
                最后心跳时间 = sessionInfo.LastActivityTime.ToString("yyyy-MM-dd HH:mm:ss"),
                客户端IP = sessionInfo.ClientIp,
                当前模块 = sessionInfo.Properties?.ContainsKey("CurrentModule") == true ?
                         sessionInfo.Properties["CurrentModule"]?.ToString() : "未知",
                客户端版本 = sessionInfo.ClientVersion ?? "未知",
                在线状态 = sessionInfo.IsConnected
            };

            // 如果有用户详细信息，则填充
            if (sessionInfo.UserInfo != null)
            {
                userInfo.姓名 = sessionInfo.UserInfo.姓名;
            }

            return userInfo;
        }

        #endregion

        private List<UserInfo> SelectUser()
        {
            // 获取选中的用户
            var selectedUsers = new List<UserInfo>();
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
            if (UserInfos.Count == 0)
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
                    HandleSwitchServer(users);
                    break;
                default:
                    break;
            }
        }

        private void HandleSwitchServer(List<UserInfo> users)
        {
            frmInput frmInput = new frmInput();
            frmInput.Text = "请输入服务器IP和端口，格式为 IP:端口";
            frmInput.txtInputContent.Text = "192.168.0.254:3001";
            if (frmInput.ShowDialog() == DialogResult.OK)
            {
                foreach (var user in users)
                {
                    try
                    {
                        // 使用新的SessionService获取会话信息
                        var session = _sessionService.GetSession(user.SessionId);
                        if (session != null)
                        {
                            // 发送切换服务器命令
                            var success = _sessionService.SendCommandToSession(session.SessionID, "SWITCH_SERVER", frmInput.InputContent);
                            if (success)
                            {
                                frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送切换服务器命令: {frmInput.InputContent}");
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
                        // 发送强制退出命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "FORCE_LOGOUT", null);
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
                        // 发送删除配置文件命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "DELETE_CONFIG", null);
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

        private void HandleSendMessage(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                try
                {
                    // 使用新的SessionService获取会话信息
                    var session = _sessionService.GetSession(user.SessionId);
                    if (session != null)
                    {
                        // 显示发送消息对话框
                        //using (var form = new frmMessager(user, _sessionService))
                        //{
                        //    if (form.ShowDialog() == DialogResult.OK)
                        //    {
                        //        frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送消息: {form.Message}");
                        //    }
                        //}
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送消息失败: {ex.Message}");
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
                        // 发送更新推送命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_UPDATE", null);
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
                        // 发送系统配置推送命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_SYS_CONFIG", null);
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
                        // 发送推送缓存命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_CACHE", null);
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

        /// <summary>
        /// 批量推送缓存数据
        /// </summary>
        /// <param name="users">用户列表</param>
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
                        // 发送缓存数据推送通知
                        foreach (var tableName in Business.Cache.TableSchemaManager.Instance.GetAllTableNames())
                        {
                            // 发送缓存数据列表
                            // MessageService.SendCacheDataList(session, tableName);
                            frmMainNew.Instance.PrintInfoLog($"已向用户 {user.用户名} 发送缓存表 {tableName} 的数据推送通知");
                        }
                    }
                    else
                    {
                        frmMainNew.Instance.PrintErrorLog($"用户 {user.用户名} 的会话不存在");
                    }
                }
                catch (Exception ex)
                {
                    frmMainNew.Instance.PrintErrorLog($"向用户 {user.用户名} 发送缓存数据推送通知失败: {ex.Message}");
                }
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
                        // 发送关机命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "SHUTDOWN", null);
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

        private void CleanupInactiveUsers()
        {
            // 计算1小时前的时间点
            DateTime threshold = DateTime.Now.AddMinutes(-20);

            // 找出需要移除的用户（心跳时间为空或超过1小时）
            var inactiveUsers = UserInfos
                .Where(u => !string.IsNullOrEmpty(u.最后心跳时间) && (u.最后心跳时间.ObjToDate()) < threshold)
                .ToList();

            if (inactiveUsers.Any())
            {
                listView1.BeginUpdate();
                // 从ListView中移除
                foreach (var user in inactiveUsers)
                {
                    RemoveUser(user);
                    user.PropertyChanged -= UserInfo_PropertyChanged;
                }
                listView1.EndUpdate();
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

                    // 显示发送消息对话框
                    //using (var form = new frmMessager(userInfo, _sessionService))
                    //{
                    //    if (form.ShowDialog() == DialogResult.OK)
                    //    {
                    //        frmMainNew.Instance.PrintInfoLog($"已向用户 {userInfo.用户名} 发送消息: {form.Message}");
                    //        MessageBox.Show("消息发送成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //}
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

                frmMainNew.Instance.PrintInfoLog($"服务器用户数量：{sessionCount}");

                // 添加所有用户
                foreach (var session in sessions)
                {
                    var userInfo = ConvertSessionInfoToUserInfo(session);
                    frmMainNew.Instance.PrintInfoLog($"添加用户：{userInfo.用户名}");
                    AddOrUpdateUser(userInfo);
                }
            }
            catch (Exception ex)
            {
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
                        // 发送推送更新命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_UPDATE", null);
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
                        // 发送推送系统配置命令
                        var success = _sessionService.SendCommandToSession(session.SessionID, "PUSH_SYS_CONFIG", null);
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