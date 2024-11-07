using RUINORERP.Business.CommService;
using RUINORERP.Common.Extensions;
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

namespace RUINORERP.Server
{
    public partial class frmUserManage : frmBase
    {
        public frmUserManage()
        {
            InitializeComponent();
        }
        public ObservableCollection<UserInfo> userInfos = new ObservableCollection<UserInfo>();
        private void frmUserManage_Load(object sender, EventArgs e)
        {
            dataGridView1.VirtualMode = false;
            // 订阅CollectionChanged事件
            userInfos.CollectionChanged -= UserInfos_CollectionChanged;
            userInfos.CollectionChanged += UserInfos_CollectionChanged;

            RefreshData();


        }

        // 刷新数据的方法
        private void RefreshData()
        {
            // 这里你可以执行数据更新的逻辑，例如从数据库重新读取数据
            // 然后重新绑定数据源到DataGridView
            userInfoBindingSource.DataSource = userInfos;
            //dataGridView1.DataSource = null;
            dataGridView1.DataSource = userInfoBindingSource;

            DataGridViewColumn LastbeatTime = dataGridView1.Columns["最后心跳时间" + "DataGridViewTextBoxColumn"];

            // 获取SessionID列
            DataGridViewColumn sessionIDColumn = dataGridView1.Columns["sessionIDDataGridViewTextBoxColumn"];

            // 如果找到了该列，将其设置为不可见
            if (sessionIDColumn != null)
            {
                // sessionIDColumn.Visible = false;
            }
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

                        //SessionforBiz biz = frmMain.Instance.sessionListBiz[((UserInfo)e.NewItems[e.NewStartingIndex]).SessionId];
                        //userInfoBindingSource.Add(((UserInfo)e.NewItems[e.NewStartingIndex]));
                        // 处理添加元素的逻辑
                        //userInfoBindingSource.DataSource = frmMain.Instance.userInfos;
                        //dataGridView1.DataSource = null;
                        //dataGridView1.DataSource = userInfoBindingSource;

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
                if (this.dataGridView1.InvokeRequired)
                {
                    // 如果当前线程不是UI线程，则使用Invoke来更新UI
                    this.dataGridView1.Invoke(new MethodInvoker(RefreshDataGridView));
                }
                else
                {
                    // 如果当前线程是UI线程，则直接更新UI
                    this.userInfoBindingSource.ResetBindings(false);
                    this.dataGridView1.Refresh();
                }
            }
            catch (Exception ex)
            {


            }

        }

        public void UserInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            try
            {
                if (sender != null && sender is UserInfo info)
                {
                    // 当用户信息发生变化时，刷新数据
                    if (frmMain.Instance.sessionListBiz.ContainsKey(info.SessionId))
                    {
                        SessionforBiz biz = frmMain.Instance.sessionListBiz[info.SessionId];
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (dataGridView1.Rows[i].DataBoundItem is UserInfo userInfo)
                            {
                                if (userInfo.SessionId == biz.SessionID)
                                {
                                    foreach (DataGridViewColumn dc in dataGridView1.Columns)
                                    {
                                        if (dc.DataPropertyName == e.PropertyName)
                                        {
                                            dataGridView1.Rows[i].Cells[dc.Name].Value = HLH.Lib.Helper.ReflectionHelper.GetPropertyValue(info, e.PropertyName);
                                            dataGridView1.Refresh();
                                            // dataGridView1.PerformLayout();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        //RefreshData();
                    }

                }

            }
            catch (Exception ex)
            {


            }
        }


        private async void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "断开连接")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
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
                            UserService.强制用户退出(SB);
                        }
                    }
                }
            }

            if (e.ClickedItem.Text == "删除列配置文件")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Connected)
                        {
                            UserService.删除列配置文件(SB);
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
                                UserService.给客户端发消息(SB, frm.Message, frm.MustDisplay);
                            }

                        }

                    }
                }
            }

            if (e.ClickedItem.Text == "推送版本更新")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
                    {
                        SessionforBiz SB = frmMain.Instance.sessionListBiz[userInfo.SessionId];
                        if (SB.State == SessionState.Connected)
                        {
                            UserService.推送版本更新(SB);
                        }

                    }
                }
            }


            if (e.ClickedItem.Text == "推送缓存数据")
            {
                if (dataGridView1.CurrentRow != null)
                {
                    if (dataGridView1.CurrentRow.DataBoundItem is UserInfo userInfo)
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
    }
}



