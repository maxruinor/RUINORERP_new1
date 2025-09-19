using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Business.CommService;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.ToolsUI;
using SuperSocket.Server.Abstractions;
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

namespace RUINORERP.Server
{
    public partial class frmUserList : frmBase
    {
        private readonly Timer _updateTimer;
        private readonly Random _random = new Random();
        public ObservableCollection<UserInfo> UserInfos { get; } = new ObservableCollection<UserInfo>();
        private readonly Dictionary<string, ListViewItem> _itemMap = new Dictionary<string, ListViewItem>();
        private DateTime _lastFullUpdate = DateTime.MinValue;
        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        private const int SB_HORZ = 0;

        public frmUserList()
        {
            InitializeComponent();
            InitializeListView();


            InitializeDataBinding();

            // 设置定时器用于UI刷新
            _updateTimer = new Timer { Interval = 1000 };
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();

            contextMenuStrip1.ItemClicked += contextMenuStrip1_ItemClicked;
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

        //private void FullRefreshListView()
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new Action(FullRefreshListView));
        //        return;
        //    }

        //    listView1.BeginUpdate();
        //    listView1.Items.Clear();
        //    _itemMap.Clear();

        //    foreach (var user in UserInfos)
        //    {
        //        var item = CreateListViewItem(user);
        //        listView1.Items.Add(item);
        //        _itemMap[user.SessionId] = item;
        //    }

        //    listView1.EndUpdate();
        //}

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
                        if (e.OldItems.Count == 1)
                        {
                            UserInfo old = e.OldItems[0] as UserInfo;
                            if (UserInfos.Contains(old))
                            {
                                UserInfos.Remove(old);                 // 处理删除元素的逻辑
                            }
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

            // 从集合中移除用户
            if (UserInfos.Contains(user))
            {
                UserInfos.Remove(user);
                // 更新 ListView 的虚拟列表大小
                listView1.VirtualListSize = UserInfos.Count;
                if (listView1.IsHandleCreated)
                {
                    listView1.Invoke(new Action(() =>
                    {
                        // 可选：取消订阅 PropertyChanged 事件
                        user.PropertyChanged -= UserInfo_PropertyChanged;
                    }));
                }

            }

            int index = UserInfos.IndexOf(user);
            if (index != -1)
            {
                UserInfos.RemoveAt(index);
                listView1.VirtualListSize = UserInfos.Count;
                listView1.Invalidate(); // 强制重绘

            }

        }


        #endregion




        private void frmUserList_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
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
                for (int i = 0; i < users.Count; i++)
                {
                    var user = users[i];
                    if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                    {
                        if (sb.State == SessionState.Connected)
                        {
                            //UserService.切换服务器(sb, frmInput.InputContent);
                        }
                    }
                }
            }
        }

        //----------- 子方法实现 -----------
        private async Task HandleDisconnect(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        await sb.CloseAsync(SuperSocket.Connection.CloseReason.RemoteClosing);
                        frmMain.Instance.sessionListBiz.TryRemove(sb.SessionID, out _);
                        RemoveUserFromListView(user);
                    }
                }
            }
        }

        private void HandleForceLogout(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz SB))
                {
                    if (SB.State == SessionState.Connected)
                    {
                        //UserService.强制用户退出(SB);
                        RemoveUserFromListView(user); // 调用移除方法
                    }
                }
            }
        }

        private void HandleDeleteConfig(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        //UserService.删除列配置文件(sb);
                    }
                }
            }
        }

        private void HandleSendMessage(List<UserInfo> users)
        {
            frmMessager frm = new frmMessager();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                foreach (var user in users)
                {
                    if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                    {
                        if (sb.State == SessionState.Connected)
                        {
                            MessageModel message = new MessageModel
                            {
                                msg = frm.Message,
                            };
                            //UserService.给客户端发消息实体(sb, message, frm.MustDisplay);
                        }
                    }
                }
            }
        }

        private void HandlePushUpdate(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        //UserService.推送版本更新(sb);
                    }
                }
            }
        }


        private void HandlePushUpdateSysConfig(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        //UserService.更新全局配置(sb);
                    }
                }
            }
        }

        private void HandlePushCache(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
                        {
                            //UserService.发送缓存数据列表(sb, tableName);
                        }
                    }
                }
            }
        }

        private void HandleShutdown(List<UserInfo> users)
        {
            foreach (var user in users)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        //UserService.强制用户关机(sb);
                    }
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

        private void tsbtn刷新_Click(object sender, EventArgs e)
        {
            var UserCounter = frmMain.Instance.sessionListBiz.Count;
            frmMain.Instance.PrintInfoLog($"服务器用户数量：{UserCounter}");
            if (UserCounter != listView1.Items.Count)
            {
                //重新全部加载一次？
                var s = _itemMap.Count;
                if (listView1.Items.Count < UserCounter)
                {
                    //添加
                    foreach (var item in frmMain.Instance.sessionListBiz)
                    {
                        if (!_itemMap.ContainsKey(item.Key))
                        {
                            frmMain.Instance.PrintInfoLog($"add用户：{item.Value.User.用户名}");
                            AddOrUpdateUser(item.Value.User);
                        }
                    }

                }
                if (listView1.Items.Count > UserCounter)
                {
                    //删除
                    for (int i = 0; i < listView1.Items.Count; i++)
                    {
                        var item = listView1.Items[i];
                        if (item is ListViewItem lvitem)
                        {
                            var userinfo = lvitem.Tag as UserInfo;
                            if (!_itemMap.ContainsKey(userinfo.SessionId))
                            {
                                frmMain.Instance.PrintInfoLog($"remove用户：{userinfo.用户名}");
                                RemoveUser(userinfo);
                            }
                        }
                    }
                }
                //AddOrUpdateUser()
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
