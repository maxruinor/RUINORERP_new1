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
        public frmUserListManage()
        {
            InitializeComponent();
        }
        public ObservableCollection<UserInfo> userInfos = new ObservableCollection<UserInfo>();
        private void frmUserManage_Load(object sender, EventArgs e)
        {
          
            // 订阅CollectionChanged事件
            userInfos.CollectionChanged -= UserInfos_CollectionChanged;
            userInfos.CollectionChanged += UserInfos_CollectionChanged;

            listView1.RetrieveVirtualItem += ListView1_RetrieveVirtualItem;
            listView1.VirtualListSize = userInfos.Count;

            AddCols();
            RefreshData();
        }
        private void ListView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (e.ItemIndex >= 0 && e.ItemIndex < userInfos.Count)
            {
                UserInfo user = userInfos[e.ItemIndex];
                ListViewItem item = new ListViewItem(user.Employee_ID.ToString()); // 第一列为员工ID

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
            }
        }

        private void AddCols()
        {
            listView1.Columns.Clear();
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

        // 刷新数据的方法
        private void RefreshData()
        {
            // 这里你可以执行数据更新的逻辑，例如从数据库重新读取数据
            // 然后重新绑定数据源到DataGridView
            userInfoBindingSource.DataSource = userInfos;
            ////dataGridView1.DataSource = null;
            //dataGridView1.DataSource = userInfoBindingSource;

            //DataGridViewColumn LastbeatTime = dataGridView1.Columns["最后心跳时间" + "DataGridViewTextBoxColumn"];

            //// 获取SessionID列
            //DataGridViewColumn sessionIDColumn = dataGridView1.Columns["sessionIDDataGridViewTextBoxColumn"];

            //// 如果找到了该列，将其设置为不可见
            //if (sessionIDColumn != null)
            //{
            //    // sessionIDColumn.Visible = false;
            //}
        }


        public void UserInfos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
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
                            if (!userInfoBindingSource.Contains(newItem))
                            {
                                userInfoBindingSource.Add(newItem);
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
                            if (userInfoBindingSource.Contains(old))
                            {
                                userInfoBindingSource.Remove(old);
                            }

                        }
                        listView1.VirtualListSize = userInfos.Count;
                        break;
                    default:
                        // return;
                        break;
                        // 可以根据需要处理其他事件类型
                }
                //if (userInfos.Count == 0)
                //{
                //   // dataGridView1.Rows.Clear();
                //}
                RefreshDataGridView();
            }
            catch (Exception ex)
            {
                Console.WriteLine("UserInfos_CollectionChanged时出错" + ex.Message);

            }


        }


        /// <summary>
        /// 刷新DataGridView控件
        /// 如果当前线程不是UI线程，它会使用Invoke来调用自己。如果当前线程是UI线程，它将直接更新UI。
        /// </summary>
        private void RefreshDataGridView()
        {
            try
            {
                if (this.listView1.InvokeRequired)
                {
                    // 如果当前线程不是UI线程，则使用Invoke来更新UI
                    this.listView1.Invoke(new MethodInvoker(RefreshDataGridView));
                }
                else
                {
                    // 如果当前线程是UI线程，则直接更新UI
                    this.userInfoBindingSource.ResetBindings(false);
                    this.listView1.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RefreshDataGridView时出错" + ex.Message);

            }

        }

        public void UserInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {


            try
            {
                if (sender != null && sender is UserInfo user)
                {
                    int index = userInfos.IndexOf(user);
                    if (index >= 0)
                    {
                        listView1.Invalidate(); // 触发重绘
                                                // 或通过 CacheVirtualItems 事件局部更新
                    }
                    // 当用户信息发生变化时，刷新数据
                    if (frmMain.Instance.sessionListBiz.ContainsKey(user.SessionId))
                    {
                        SessionforBiz biz = frmMain.Instance.sessionListBiz[user.SessionId];
                        if (this.listView1.IsHandleCreated)
                        {
                            listView1.Invoke(new Action(() =>
                            {
                                //for (int i = 0; i < listView1.Rows.Count; i++)
                                //{
                                //    if (listView1.Rows[i].DataBoundItem is UserInfo userInfo)
                                //    {
                                //        if (userInfo.SessionId == biz.SessionID)
                                //        {
                                //            foreach (DataGridViewColumn dc in dataGridView1.Columns)
                                //            {
                                //                if (dc.DataPropertyName == e.PropertyName)
                                //                {
                                //                    //dataGridView1.Rows[i].Cells[dc.Name].Value = HLH.Lib.Helper.ReflectionHelper.GetPropertyValue(info, e.PropertyName);
                                //                    //dataGridView1.Refresh();
                                //                    // dataGridView1.PerformLayout();
                                //                    break;
                                //                }
                                //            }
                                //        }
                                //    }
                                //}
                            }));
                        }
                        
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("UserInfo_PropertyChanged时出错" + ex.Message);
            }

        }


        private  void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            /*
            if (e.ClickedItem.Text == "断开连接")
            {
                if (listView1.CurrentRow != null)
                {
                    if (listView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Closed)
                        {
                            SessionforBiz biz = new SessionforBiz();
                            frmMain.Instance.sessionListBiz.TryRemove(SB.SessionID, out biz);
                            if (biz != null)
                            {
                                UserInfo user = userInfos.FirstOrDefault(c => c.SessionId == biz.SessionID);
                                userInfos.Remove(user);
                            }

                        }
                        if (SB.State == SessionState.Connected)
                        {
                            await SB.CloseAsync(SuperSocket.Connection.CloseReason.RemoteClosing);
                            SessionforBiz biz = new SessionforBiz();
                            frmMain.Instance.sessionListBiz.TryRemove(SB.SessionID, out biz);
                            if (biz != null)
                            {
                                UserInfo user = userInfos.FirstOrDefault(c => c.SessionId == biz.SessionID);
                                userInfos.Remove(user);
                            }
                        }
                    }
                }
            }
            if (e.ClickedItem.Text == "强制用户退出")
            {

                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Connected)
                        {
                            //这里是强制用户退出，让客户端自动断开服务器。
                            UserService.强制用户退出(SB);
                        }
                    }
                }
            }

            if (e.ClickedItem.Text == "删除列配置文件")
            {
                if (dataGridView1.SelectedRows != null)
                {
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        if (item.DataBoundItem is UserInfo userInfo)
                        {
                            SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                            if (SB.State == SessionState.Connected)
                            {
                                UserService.删除列配置文件(SB);
                            }


                        }
                    }
                }
            }


            if (e.ClickedItem.Text == "发消息给客户端")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Connected)
                        {
                            frmMessager frm = new frmMessager();
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                MessageModel messageModel = new MessageModel();
                                messageModel.msg = frm.Message;
                                UserService.给客户端发消息实体(SB, messageModel, frm.MustDisplay);
                            }

                        }

                    }
                }
            }

            if (e.ClickedItem.Text == "推送版本更新")
            {

                if (dataGridView1.SelectedRows != null)
                {
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        if (item.DataBoundItem is UserInfo userInfo)
                        {
                            SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                            if (SB.State == SessionState.Connected)
                            {
                                UserService.推送版本更新(SB);
                            }

                        }
                    }

                }
            }


            if (e.ClickedItem.Text == "推送缓存数据")
            {
                if (dataGridView1.SelectedRows != null)
                {
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        if (item.DataBoundItem is UserInfo userInfo)
                        {
                            SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                            if (SB.State == SessionState.Connected)
                            {
                                foreach (var tableName in BizCacheHelper.Manager.NewTableList.Keys)
                                {
                                    UserService.发送缓存数据列表(SB, tableName);
                                }

                            }
                        }
                    }
                }
            }

            if (e.ClickedItem.Text == "关机")
            {
                if (dataGridView1.CurrentRow != null)
                {

                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Connected)
                        {
                            UserService.强制用户关机(SB);
                        }
                    }
                }

            }

            */
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void frmUserManage_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }




        private void toolStripMenuItem5_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem5_Click_2(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }
    }
}



