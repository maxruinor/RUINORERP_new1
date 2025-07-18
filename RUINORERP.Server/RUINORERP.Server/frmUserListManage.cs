﻿using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RUINORERP.Server
{
    public partial class frmUserListManage : frmBase
    {
        private int _selectedIndex = -1; // 跟踪当前选中索引
        public frmUserListManage()
        {
            InitializeComponent();
            // 启用双缓冲
            EnableDoubleBuffering(listView1);
            // 每2000ms批量刷新一次
            _refreshTimer = new System.Threading.Timer(RefreshDirtyItems, null, 2000, 2000);

            // 初始化全选复选框
            InitializeHeaderCheckBox();
        }
        // 初始化列标题复选框
        private void InitializeHeaderCheckBox()
        {
            headerCheckBox = new CheckBox();
            headerCheckBox.Size = new Size(15, 15);
            headerCheckBox.Location = new Point(4, 2); // 调整位置以覆盖第一列标题
            headerCheckBox.CheckedChanged += HeaderCheckBox_CheckedChanged;

            // 将复选框添加到ListView的标题栏
            listView1.Controls.Add(headerCheckBox);

            // 设置ListView属性以显示复选框
            listView1.CheckBoxes = true;
            listView1.GridLines = true;
            listView1.View = View.Details;
            listView1.Scrollable = true;
            listView1.MultiSelect = true;
        }
        // 处理全选复选框状态变化
        private void HeaderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // 防止在ListView加载过程中触发此事件
            if (listView1.Items.Count == 0) return;

            // 开始更新，防止界面闪烁
            listView1.BeginUpdate();

            try
            {
                // 设置所有行的选中状态
                //foreach (ListViewItem item in listView1.Items)
                //{
                //    item.Checked = headerCheckBox.Checked;
                //}
                // 记录需要重绘的范围
                int startIndex = 0;
                bool isChecked = headerCheckBox.Checked;
                int endIndex = listView1.VirtualListSize - 1;

                // 设置所有行的选中状态
                for (int i = 0; i < listView1.VirtualListSize; i++)
                {
                    _checkedStates[i] = isChecked; // 更新选中状态字典
                    // 使用RedrawItems方法通知ListView某行需要重绘
                    // 实际的选中状态将在RetrieveVirtualItem事件中处理
                    _dirtyIndexes.Add(i);
                }

                // 批量重绘所有项
                SafeRedrawItems(startIndex, endIndex);

            }
            finally
            {
                // 结束更新
                listView1.EndUpdate();
            }
        }

        // 新增：存储选中状态的字典
        private Dictionary<int, bool> _checkedStates = new Dictionary<int, bool>();

        /// <summary>
        /// 获取所有选中的用户信息集合
        /// </summary>
        /// <returns>选中的用户信息列表</returns>
        public List<UserInfo> GetSelectedUsers()
        {
            var selectedUsers = new List<UserInfo>();

            // 遍历选中状态字典
            foreach (var pair in _checkedStates)
            {
                if (pair.Value && pair.Key < userInfos.Count) // 确保索引有效
                {
                    selectedUsers.Add(userInfos[pair.Key]);
                }
            }

            return selectedUsers;
        }



        // 重写ListView的OnHandleCreated方法，确保复选框位置正确
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // 确保在ListView句柄创建后调整复选框位置
            if (headerCheckBox != null && listView1.Columns.Count > 0)
            {
                // 调整复选框位置以适应列标题
                headerCheckBox.Location = new Point(4, 2);
            }
        }



        // 更新全选复选框状态
        private void UpdateHeaderCheckBoxState()
        {
            if (listView1.Items.Count == 0)
            {
                //headerCheckBox.Checked = false;
                headerCheckBox.CheckState = CheckState.Unchecked;
                return;
            }
            int checkedCount = _checkedStates.Count(p => p.Value);
            if (checkedCount == listView1.VirtualListSize)
            {
                headerCheckBox.CheckState = CheckState.Checked;
            }
            else if (checkedCount == 0)
            {
                headerCheckBox.CheckState = CheckState.Unchecked;
            }
            else
            {
                headerCheckBox.CheckState = CheckState.Indeterminate;
            }

            //int checkedCount = 0;
            //foreach (ListViewItem item in listView1.Items)
            //{
            //    if (item.Checked)
            //        checkedCount++;
            //}

            // 如果所有项都被选中，全选框为选中状态
            // 如果没有项被选中，全选框为未选中状态
            // 如果部分项被选中，全选框为不确定状态
            if (checkedCount == listView1.Items.Count)
            {
                headerCheckBox.Checked = true;
                headerCheckBox.CheckState = CheckState.Checked;
            }
            else if (checkedCount == 0)
            {
                headerCheckBox.Checked = false;
                headerCheckBox.CheckState = CheckState.Unchecked;
            }
            else
            {
                headerCheckBox.CheckState = CheckState.Indeterminate;
            }
        }

        private void RefreshDirtyItems(object state)
        {
            if (_dirtyIndexes.Count == 0) return;
            try
            {
                int min = _dirtyIndexes.Min();
                int max = _dirtyIndexes.Max();
                SafeRedrawItems(min, max);
            }
            catch (Exception)
            {


            }
            _dirtyIndexes.Clear();
        }



        // 通过反射启用双缓冲（系统默认未开放此属性）
        public static void EnableDoubleBuffering(System.Windows.Forms.ListView listView)
        {
            typeof(System.Windows.Forms.ListView)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(listView, true, null);
        }


        public ObservableCollection<UserInfo> userInfos = new ObservableCollection<UserInfo>();
        private void frmUserManage_Load(object sender, EventArgs e)
        {
            // 订阅CollectionChanged事件
            userInfos.CollectionChanged -= UserInfos_CollectionChanged;
            userInfos.CollectionChanged += UserInfos_CollectionChanged;

            listView1.RetrieveVirtualItem += ListView1_RetrieveVirtualItem;
            listView1.VirtualListSize = userInfos.Count;
            listView1.MultiSelect = false; // 是否允许多选（根据需求设置）
            listView1.ItemCheck+=ListView1_ItemCheck;
            listView1.ItemChecked += ListView1_ItemChecked;
            AddCols();
            AddUserList();

        }

        private void ListView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 更新选中状态字典
           // _checkedStates[e.Index] = e.NewValue == CheckState.Checked;
            // 检查所有项的状态，更新全选复选框状态
            UpdateHeaderCheckBoxState();
        }

        private void ListView1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
            // 更新选中状态字典
           // _checkedStates[e.Index] = e.CurrentValue.GetType().Checked;

            // 检查所有项的状态，更新全选复选框状态
            UpdateHeaderCheckBoxState();
        }
 

        private void AddUserList()
        {
            if (!this.IsHandleCreated)
            {
                return;
            }
            //如果sessionListBiz中的用户不存在于userInfos这里要添加一下
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(SyncUserInfosFromSessionList));
                    return;
                }
                else
                {
                    SyncUserInfosFromSessionList();
                }

                // [原有同步逻辑]
            }
            catch (InvalidOperationException ex)
            {
                frmMain.Instance.PrintMsg($"集合同步失败: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                frmMain.Instance.PrintMsg($"空引用异常: {ex.StackTrace}");
            }
            finally
            {
                // 确保界面最终刷新
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(SyncUserInfosFull));
                }
                else
                {
                    SyncUserInfosFull();
                }
            }
        }

        private void SyncUserInfosFull()
        {
            var validSessionIds = new HashSet<string>(
                frmMain.Instance.sessionListBiz.Keys
            );

            // 移除已失效的用户
            for (int i = userInfos.Count - 1; i >= 0; i--)
            {
                if (!validSessionIds.Contains(userInfos[i].SessionId))
                {
                    userInfos[i].PropertyChanged -= UserInfo_PropertyChanged;
                    userInfos.RemoveAt(i);
                }
            }

            // 添加新用户（原有逻辑）
            SyncUserInfosFromSessionList();
        }

        #region
        /// <summary>
        /// 将 sessionListBiz 中的用户同步到 userInfos
        /// </summary>
        private void SyncUserInfosFromSessionList()
        {
            // 确保在 UI 线程操作
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(SyncUserInfosFromSessionList));
                return;
            }

            // 获取现有用户的 SessionID 集合（用于快速查找）
            var existingSessionIds = new HashSet<string>(
                userInfos.Select(u => u.SessionId)
            );

            // 遍历 sessionListBiz 添加新用户
            foreach (var sessionPair in frmMain.Instance.sessionListBiz)
            {
                var sessionId = sessionPair.Key;
                var sessionBiz = sessionPair.Value;

                // 跳过已存在的用户
                if (existingSessionIds.Contains(sessionId)) continue;

                // 转换为 UserInfo 对象
                var userInfo = sessionBiz.User;

                // 添加新用户到集合
                userInfos.Add(userInfo);

                // 订阅属性变更事件
                userInfo.PropertyChanged += UserInfo_PropertyChanged;
            }
        }

        #endregion

        private void ListView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < userInfos.Count)
            {
                UserInfo user = userInfos[e.ItemIndex];
                //ListViewItem item = new ListViewItem(user.Employee_ID.ToString()); // 第一列为员工ID
                ListViewItem item = e.Item ?? new ListViewItem(); // 重用现有项（若存在）

                // 设置行的选中状态（根据需要）
                // item.Checked = user.IsSelected; // 如果UserInfo类有IsSelected属性
                // 清空所有子项（重要！确保不会重复添加）
                item.SubItems.Clear();
                // 按列顺序添加子项
                //item.SubItems[0].Text = ""; // 复选框列

                // 设置行的选中状态
                //bool isChecked;
                //if (_checkedStates.TryGetValue(e.ItemIndex, out isChecked))
                //{
                //    item.Checked = isChecked;
                //}
                // 设置复选框状态
                bool isChecked = _checkedStates.ContainsKey(e.ItemIndex) && _checkedStates[e.ItemIndex];
                item.Checked = isChecked;

                item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = (e.ItemIndex + 1).ToString() });
                //item.SubItems.Add(""); // 复选框列（第一列）
                // 按列顺序添加子项
                item.SubItems.Add(user.SessionId);                 // SessionId
                item.SubItems.Add(user.用户名);                     // 用户名
                item.SubItems.Add(user.姓名);                       // 姓名
                item.SubItems.Add(user.当前模块);                   // 当前模块
                item.SubItems.Add(user.当前窗体);                   // 当前窗体
                item.SubItems.Add(user.登陆时间.ToString("yyyy-MM-dd HH:mm:ss")); // 格式化时间
                item.SubItems.Add(user.心跳数.ToString());           // 心跳数
                item.SubItems.Add(user.最后心跳时间);                // 最后心跳时间
                item.SubItems.Add(user.客户端版本);                  // 客户端版本
                item.SubItems.Add(user.客户端IP);                    // 客户端IP
                item.SubItems.Add(user.静止时间.ToString());         // 静止时间
                item.SubItems.Add(user.UserID.ToString());          // 用户ID
                item.SubItems.Add(user.超级用户 ? "是" : "否");      // 布尔值转文本
                item.SubItems.Add(user.在线状态 ? "在线" : "离线");  // 布尔值转文本
                item.SubItems.Add(user.授权状态 ? "已授权" : "未授权"); // 布尔值转文本

                e.Item = item;
            }
            else
            {
                e.Item = new ListViewItem(); // 防止索引越界
                                             // 处理空项情况
                e.Item.SubItems.Clear();
                for (int i = 0; i < listView1.Columns.Count; i++)
                {
                    e.Item.SubItems.Add("");
                }
            }
        }
        private CheckBox headerCheckBox; // 声明列标题复选框

 
        private void AddCols()
        {
            listView1.Columns.Clear();
            // 调整第一列宽度，为复选框留出空间
            listView1.Columns.Add("", 30); // 复选框列
            listView1.Columns.Add("员工ID", 80);          // Employee_ID
            listView1.Columns.Add("SessionId", 150);     // SessionId
            listView1.Columns.Add("用户名", 100);         // 用户名
            listView1.Columns.Add("姓名", 100);           // 姓名
            listView1.Columns.Add("当前模块", 120);       // 当前模块
            listView1.Columns.Add("当前窗体", 120);       // 当前窗体
            listView1.Columns.Add("登陆时间", 150);       // 登陆时间
            listView1.Columns.Add("心跳数", 80);          // 心跳数
            listView1.Columns.Add("最后心跳时间", 150);   // 最后心跳时间
            listView1.Columns.Add("客户端版本", 100);     // 客户端版本
            listView1.Columns.Add("客户端IP", 120);       // 客户端IP
            listView1.Columns.Add("静止时间", 80);        // 静止时间
            listView1.Columns.Add("用户ID", 80);          // UserID
            listView1.Columns.Add("超级用户", 80);        // 超级用户
            listView1.Columns.Add("在线状态", 80);        // 在线状态
            listView1.Columns.Add("授权状态", 80);        // 授权状态
        }

        private System.Threading.Timer _refreshTimer;
        private HashSet<int> _dirtyIndexes = new HashSet<int>();

        public void UserInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(() => listView1.VirtualListSize = userInfos.Count);
                }
                else
                {
                    listView1.VirtualListSize = userInfos.Count;
                }

                // 在这里处理集合变化的逻辑
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        if (e.NewItems.Count == 1)
                        {
                            UserInfo newItem = e.NewItems[0] as UserInfo;
                            if (!userInfos.Contains(newItem))
                            {
                                userInfos.Add(newItem);                 // 处理删除元素的逻辑
                            }


                        }
                        listView1.VirtualListSize = userInfos.Count;

                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        if (e.OldItems.Count == 1)
                        {
                            UserInfo old = e.OldItems[0] as UserInfo;
                            if (userInfos.Contains(old))
                            {
                                userInfos.Remove(old);                 // 处理删除元素的逻辑
                            }
                        }
                        listView1.VirtualListSize = userInfos.Count;
                        break;
                    default:
                        // return;
                        break;
                        // 可以根据需要处理其他事件类型
                }

                //如果listview中的数据不存在于userinfos中。UI上也要移除

            }
            catch (Exception ex)
            {
                Console.WriteLine("UserInfos_CollectionChanged时出错" + ex.Message);

            }


        }



        private void RemoveUserFromListView(UserInfo user)
        {
            // 确保在 UI 线程执行
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => RemoveUserFromListView(user)));
                return;
            }

            // 从集合中移除用户
            if (userInfos.Contains(user))
            {
                userInfos.Remove(user);
                // 更新 ListView 的虚拟列表大小
                listView1.VirtualListSize = userInfos.Count;
                if (listView1.IsHandleCreated)
                {
                    listView1.Invoke(new Action(() =>
                    {
                        // 可选：取消订阅 PropertyChanged 事件
                        user.PropertyChanged -= UserInfo_PropertyChanged;
                    }));
                }

            }

            int index = userInfos.IndexOf(user);
            if (index != -1)
            {
                userInfos.RemoveAt(index);
                listView1.VirtualListSize = userInfos.Count;
                listView1.Invalidate(); // 强制重绘
                _selectedIndex = -1;    // 重置选中索引
            }

        }
        public void UserInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            try
            {
                if (sender != null && sender is UserInfo user)
                {
                    //高频
                    if (e.PropertyName == nameof(UserInfo.心跳数) || e.PropertyName == nameof(UserInfo.静止时间))
                    {
                        int index = userInfos.IndexOf(user);
                        if (index != -1)
                        {
                            //使用批量刷新法
                            _dirtyIndexes.Add(index);
                            // 使用局部刷新方法
                            //SafeRedrawItems(index, index);
                        }
                    }

                    //低频
                    if (e.PropertyName == nameof(UserInfo.当前模块) || e.PropertyName == nameof(UserInfo.当前窗体))
                    {
                        int index = userInfos.IndexOf(user);
                        if (index != -1)
                        {
                            // 使用局部刷新方法
                            SafeRedrawItems(index, index);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("UserInfo_PropertyChanged时出错" + ex.Message);
            }

        }

        private void SafeRedrawItems(int startIndex, int endIndex)
        {
            try
            {
                if (listView1.InvokeRequired)
                {
                    listView1.BeginInvoke(new Action(() => SafeRedrawItems(startIndex, endIndex)));
                    return;
                }
                listView1.RedrawItems(startIndex, endIndex, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SafeRedrawItems: {ex.Message}");
                Console.WriteLine($"userInfos: {userInfos.Count}");
                Console.WriteLine($"listView1.Items: {listView1.Items.Count}");
            }


        }

        private async void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (_selectedIndex == -1 || _selectedIndex >= userInfos.Count)
            {
                MessageBox.Show("请先选中一个用户！");
                return;
            }

            //if (_selectedIndex == -1 || _selectedIndex >= userInfos.Count) return;

            UserInfo user = userInfos[_selectedIndex];
            // 获取选中用户
            int index = listView1.SelectedIndices[0];
            UserInfo user1 = userInfos[index];

            // 根据菜单项执行操作
            switch (e.ClickedItem.Text)
            {
                case "断开连接":
                    await HandleDisconnect(user);
                    break;
                case "强制用户退出":
                    HandleForceLogout(user);
                    break;
                case "删除列配置文件":
                    HandleDeleteConfig(user);
                    break;
                case "发消息给客户端":
                    HandleSendMessage(user);
                    break;
                case "推送版本更新":
                    HandlePushUpdate(user);
                    break;
                case "更新全局配置":
                    HandlePushUpdateSysConfig(user);
                    break;
                case "推送缓存数据":
                    HandlePushCache(user);
                    break;
                case "关机":
                    HandleShutdown(user);
                    break;
                case "切换服务器":
                    HandleSwitchServer(user);
                    break;
                case "全部切换服务器":
                    HandleSwitchServer();
                    break;
                default:
                    break;
            }
        }

        private async void HandleSwitchServer(UserInfo user = null)
        {
            frmInput frmInput = new frmInput();
            frmInput.Text = "请输入服务器IP和端口，格式为 IP:端口";
            frmInput.txtInputContent.Text = "192.168.0.254:3001";
            if (frmInput.ShowDialog() == DialogResult.OK)
            {
                if (user == null)
                {
                    List<SessionforBiz> sbList = new List<SessionforBiz>();
                    sbList = frmMain.Instance.sessionListBiz.Values.ToList();
                    for (int i = 0; i < sbList.Count; i++)
                    {
                        if (sbList[i].State == SessionState.Connected)
                        {
                            await Task.Delay(500);
                            UserService.切换服务器(sbList[i], frmInput.InputContent);
                        }
                    }
                }
                else
                {
                    if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                    {
                        if (sb.State == SessionState.Connected)
                        {
                            UserService.切换服务器(sb, frmInput.InputContent);
                        }
                    }
                }

            }


        }

        //----------- 子方法实现 -----------
        private async Task HandleDisconnect(UserInfo user)
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

        private void HandleForceLogout(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz SB))
            {
                if (SB.State == SessionState.Connected)
                {
                    UserService.强制用户退出(SB);
                    RemoveUserFromListView(user); // 调用移除方法
                }
            }

        }

        private void HandleDeleteConfig(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
            {
                if (sb.State == SessionState.Connected)
                {
                    UserService.删除列配置文件(sb);
                }
            }
        }

        private void HandleSendMessage(UserInfo user)
        {
            frmMessager frm = new frmMessager();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
                {
                    if (sb.State == SessionState.Connected)
                    {
                        MessageModel message = new MessageModel
                        {
                            msg = frm.Message,
                        };
                        UserService.给客户端发消息实体(sb, message, frm.MustDisplay);
                    }
                }
            }
        }

        private void HandlePushUpdate(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
            {
                if (sb.State == SessionState.Connected)
                {
                    UserService.推送版本更新(sb);
                }
            }
        }


        private void HandlePushUpdateSysConfig(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
            {
                if (sb.State == SessionState.Connected)
                {
                    UserService.推送版本更新(sb);
                }
            }
        }

        private void HandlePushCache(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
            {
                if (sb.State == SessionState.Connected)
                {
                    foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
                    {
                        UserService.发送缓存数据列表(sb, tableName);
                    }
                }
            }
        }

        private void HandleShutdown(UserInfo user)
        {
            if (frmMain.Instance.sessionListBiz.TryGetValue(user.SessionId, out SessionforBiz sb))
            {
                if (sb.State == SessionState.Connected)
                {
                    UserService.强制用户关机(sb);
                }
            }
        }

        public void OnSessionClosed(string sessionId)
        {
            var user = userInfos.FirstOrDefault(u => u.SessionId == sessionId);
            if (user != null)
            {
                RemoveUserFromListView(user);
            }
        }

        private void CleanupInactiveUsers()
        {
            //var inactiveUsers = userInfos.Where(u => u.静止时间 > 300).ToList();
            //foreach (var user in inactiveUsers)
            //{
            //    RemoveUserFromListView(user);
            //}
        }

        private void frmUserManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // 获取鼠标位置下的项
                ListViewHitTestInfo hitTest = listView1.HitTest(e.X, e.Y);
                if (hitTest.Location == ListViewHitTestLocations.None)
                {
                    _selectedIndex = -1;
                    return;
                }
                if (hitTest.Item != null)
                {
                    _selectedIndex = hitTest.Item.Index;
                    // 取消其他选中项，仅选中右键点击的行
                    listView1.SelectedIndices.Clear();
                    listView1.SelectedIndices.Add(_selectedIndex); // 显式设置选中索引


                    hitTest.Item.Selected = true;
                }
                else
                {
                    _selectedIndex = -1;
                }
            }
        }

        private void tsbtn刷新_Click(object sender, EventArgs e)
        {
            AddUserList();
        }

  
    }
}



