using Microsoft.Extensions.Logging;
using RUINORERP.Server.BizService;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.DataPortal;

namespace RUINORERP.Server
{
    public partial class frmUserOnline : Form
    {
        private ILogger _logger;
        public frmUserOnline(ILoggerFactory loggerFactory)
        {
            InitializeComponent();
            _logger = loggerFactory.CreateLogger<frmUserOnline>();
            this.Load += frmUserOnline_Load;
        }


        BindingSource bs = new BindingSource(frmMain.Instance.sessionListBiz, null);
        private void frmUserOnline_Load(object sender, EventArgs e)
        {

            LoadUserOnline();
            /*
            new Task(new Action(() =>
            {

                this.Invoke(new Action(() =>
                {
                    //需要执行的代码
                    if (this.listBoxForConn.Items.Count != frmMain.Instance.sessionListBiz.Count)
                    {
                        listBoxForConn.Items.Clear();
                        foreach (var item in frmMain.Instance.sessionListBiz)
                        {
                            listBoxForConn.Items.Add(item.Value);
                        }
                    }

                }));

            })).Start();
            */



        }

        private void LoadUserOnline()
        {
            Task.Run(async () =>
            {
                //Tools.ShowMsg("StartSendService Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                //https://www.cnblogs.com/doforfuture/p/6293926.html
                //LV
                //https://blog.csdn.net/qq_57798018/article/details/128122921
                while (true)
                {
                    try
                    {

                        //if (this.listViewForUser.Items.Count == frmMain.Instance.sessionListBiz.Count)
                        //{
                        //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                        listViewForUser.BeginUpdate();
                        if (frmMain.Instance.sessionListBiz.Count == 0)
                        {
                            listViewForUser.Items.Clear();
                        }

                        //掉线的移出,空的也移出，如果客户端断线，也会自动移出，如果他登陆，不点OK，可能产生两个连接。

                        foreach (var item in frmMain.Instance.sessionListBiz)
                        {
                            SessionforBiz SB = frmMain.Instance.sessionListBiz[item.Key];
                            TimeSpan timeSpan = System.DateTime.Now - SB.LastActiveTime;
                            Console.WriteLine("时间差: " + timeSpan.ToString());
                            //这里可以判断一下 这个IP是不是已经有正常连接  
                            //一个客户端 只能登陆一个账号，并且只同时登陆一个系统，不能双开？
                            //int count = frmMain.Instance.sessionListBiz.Where(s => s.Value.RemoteEndPoint.ToString().Contains("192.168.0.99")).Count();
                            //if (count > 1)
                            //{

                            //}
                            //如果时间差大于30秒，并且没有正常帐号信息。就断开。
                            //留30秒，如果没有正常帐号信息，就断开，这个时间客户机要提供帐号密码来验证。
                            //开始的连接是没有帐号信息的。这里要判断时间
                            if (timeSpan.TotalSeconds > 30 && SB.User.UserID == 0)
                            {
                                //if (SB.State == SuperSocket.SessionState.Connected)
                                //{
                                await SB.CloseAsync(SuperSocket.Channel.CloseReason.RemoteClosing);
                                frmMain.Instance.sessionListBiz.TryRemove(item.Key, out SB);
                                //}
                            }
                            if (timeSpan.TotalSeconds > 60)
                            {
                                //if (SB.State == SuperSocket.SessionState.Connected)
                                //{
                                await SB.CloseAsync(SuperSocket.Channel.CloseReason.RemoteClosing);
                                frmMain.Instance.sessionListBiz.TryRemove(item.Key, out SB);
                                //}
                            }

                        }

                        foreach (var item in frmMain.Instance.sessionListBiz)
                        {
                            if (item.Key == null)
                            {
                                continue;
                            }
                            bool exist = false;
                            if (item.Value != null && item.Value.User != null)
                            {
                                foreach (ListViewItem lvitem in listViewForUser.Items)
                                {
                                    if (lvitem.SubItems.ContainsKey(item.Key))
                                    {
                                        if (item.Value == null || item.Value.User.UserName == null)
                                        {
                                            continue;
                                        }
                                        //更新 ch账号, ch姓名, ch所在模块, ch当前窗体, ch登陆时间, ch心跳数, ch心跳时间, chVer,ch静止时间 
                                        exist = true;
                                        lvitem.SubItems[0].Text = item.Value.User.UserName.ToString();
                                        lvitem.SubItems[1].Text = item.Value.User.EmpName.ToString();
                                        lvitem.SubItems[2].Text = item.Value.User.ClientInfo.CurrentModule.ToString();
                                        lvitem.SubItems[3].Text = item.Value.User.ClientInfo.CurrentFormUI.ToString();
                                        lvitem.SubItems[4].Text = item.Value.User.ClientInfo.LoginTime.ToString();
                                        lvitem.SubItems[5].Text = item.Value.User.ClientInfo.BeatData.ToString();
                                        lvitem.SubItems[6].Text = item.Value.User.ClientInfo.LastBeatTime.ToString();
                                        lvitem.SubItems[7].Text = item.Value.User.ClientInfo.Version.ToString();
                                        lvitem.SubItems[8].Text = (item.Value.User.ClientInfo.ComputerFreeTime / 1000).ToString();
                                    }
                                    else
                                    {
                                        exist = false;
                                    }
                                }
                                //空的移出
                                for (int i = 0; i < listViewForUser.Items.Count; i++)
                                {
                                    if (string.IsNullOrEmpty(listViewForUser.Items[i].Text.Trim()))
                                    {
                                        listViewForUser.Items.Remove(listViewForUser.Items[i]);
                                    }
                                }



                                if (!exist && !listViewForUser.Items.ContainsKey(item.Key))
                                {
                                    #region 添加
                                    //实例化创建对象item
                                    ListViewItem itemLv = new ListViewItem();
                                    //向listView控件的项中添加第一个元素ID
                                    itemLv = listViewForUser.Items.Add(item.Value.User.UserName, item.Value.User.UserName, 0); //个人认为这句类似于数据库的添加主键元素,为了标明到底是哪一行,为后续操作做铺垫
                                    itemLv.Tag = item.Value;
                                    itemLv.Name = item.Key.ToString();//sessionid
                                    itemLv.SubItems.Add(item.Value.User.UserName);
                                    itemLv.SubItems.Add(item.Value.User.EmpName);         //这句操作会跟随上句类似添加主键操作后面,作为主键那行的第二个元素
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.CurrentModule.ToString());                  //原理同上,后面可以继续添加元素，如果你有需求且在winform中已设计好属性列个数
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.CurrentFormUI.ToString());
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.LoginTime.ToString());//4
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.BeatData.ToString());
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.LastBeatTime.ToString());
                                    itemLv.SubItems.Add(item.Value.User.ClientInfo.Version.ToString());
                                    itemLv.SubItems.Add((item.Value.User.ClientInfo.ComputerFreeTime / 1000).ToString());
                                    //item.BackColor = Color.Red;
                                    #endregion
                                }

                            }
                        }

                        //空的移出,掉线的移出
                        for (int i = 0; i < listViewForUser.Items.Count; i++)
                        {
                            var se = frmMain.Instance.sessionListBiz.Where(c => c.Key == listViewForUser.Items[i].Name.ToString()).FirstOrDefault();
                            if (se.Key == null)
                            {
                                listViewForUser.Items.Remove(listViewForUser.Items[i]);
                            }
                        }

                        //结束数据处理，UI界面一次性绘制。
                        listViewForUser.EndUpdate();
                        Thread.Sleep(2000);
                        // }

                    }
                    catch (Exception ex)
                    {
                        //Tools.ShowMsg(ex.Message + "StartSendService!#");
                        //log4netHelper.error("显示在线人数出错 ", ex);
                        _logger.LogError(ex, "显示在线人数出错");
                    }
                }
            });
        }



        private void updateUI()
        {

            Task task = new Task(() =>
            {
                int i = 0; while (++i < 100)
                {
                    Thread.Sleep(10);//模拟耗时操作
                    MethodInvoker mi = new MethodInvoker(() =>
                    {
                        // progressBar1.Value = i;
                        // this.label1.Text = i.ToString();
                    });
                    this.BeginInvoke(mi);
                }
            });

            task.Start(); task.ContinueWith(t =>
            {
                //progressBar1.Visible = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("test" + System.DateTime.Now);
            foreach (var item in frmMain.Instance.sessionListBiz)
            {
                try
                {

                    OriginalData exMsg = new OriginalData();
                    exMsg.cmd = (byte)ServerCmdEnum.给客户端发提示消息;
                    exMsg.One = null;
                    //这种可以写一个扩展方法
                    ByteBuff tx = new ByteBuff(100);
                    tx.PushString("给客户端发提示消息测试！");
                    exMsg.Two = tx.toByte();
                    item.Value.AddSendData(exMsg);

                    //ByteBuff tx = new ByteBuff(100);
                    //tx.PushInt(1);
                    //tx.PushString("11111服务器发给客户端MSG");
                    //tx.PushString("2222fg");

                    //item.Value.AddSendData((byte)ServerCmdEnum.转发消息, null, tx.toByte());


                }
                catch (Exception ex)
                {
                    frmMain.Instance.PrintInfoLog("用户登陆:" + ex.Message);
                }

            }
        }

        private async void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "断开连接")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Closed)
                            {
                                listViewForUser.Items.Remove(lvi);
                                SessionforBiz biz = new SessionforBiz();
                                frmMain.Instance.sessionListBiz.TryRemove(SB.SessionID, out biz);
                            }
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                await SB.CloseAsync(SuperSocket.Channel.CloseReason.RemoteClosing);
                                listViewForUser.Items.Remove(lvi);
                                SessionforBiz biz = new SessionforBiz();
                                frmMain.Instance.sessionListBiz.TryRemove(SB.SessionID, out biz);
                            }

                        }

                    }
                }
            }
            if (e.ClickedItem.Text == "强制用户退出")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                UserService.强制用户退出(SB);
                            }

                        }

                    }
                }
            }
            if (e.ClickedItem.Text == "删除列配置文件")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                UserService.删除列配置文件(SB);
                            }

                        }

                    }
                }
            }



            if (e.ClickedItem.Text == "发消息给客户端")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                UserService.发消息给客户端(SB);
                            }

                        }

                    }
                }
            }


            if (e.ClickedItem.Text == "推送版本更新")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                UserService.推送版本更新(SB);
                            }

                        }

                    }
                }
            }


            if (e.ClickedItem.Text == "关机")
            {
                if (listViewForUser.SelectedItems != null)
                {
                    if (listViewForUser.SelectedItems.Count == 1)
                    {
                        ListViewItem lvi = listViewForUser.SelectedItems[0];

                        if (lvi.Tag is SessionforBiz)
                        {
                            SessionforBiz SB = lvi.Tag as SessionforBiz;
                            if (SB.State == SuperSocket.SessionState.Connected)
                            {
                                UserService.强制用户关机(SB);
                            }

                        }

                    }
                }
            }
        }
    }
}
