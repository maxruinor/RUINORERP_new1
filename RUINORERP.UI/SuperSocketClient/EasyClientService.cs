using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System.IO;
using System.Net;
using TransInstruction;
using System.Threading;
using System.Collections.Concurrent;
using System.ComponentModel;
using TransInstruction.DataPortal;
using Microsoft.Extensions.Logging;
using RUINORERP.UI.Common;
using System.Windows.Forms;
using RUINORERP.UI.IM;
using System.Diagnostics;
using SourceLibrary.Security;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.UI.SuperSocketClient
{
    public class EasyClientService
    {
        #region 当前用户信息
        private List<UserInfo> userInfos = new List<UserInfo>();
        public List<UserInfo> UserInfos { get => userInfos; set => userInfos = value; }

        private UserInfo _currentUser = new UserInfo();

        public UserInfo CurrentUser { get => _currentUser; set => _currentUser = value; }


        #endregion


        public delegate void ConnectClosed(bool isconect);

        [Browsable(true), Description("连接事件")]
        public event ConnectClosed OnConnectClosed;


        #region 

        #endregion 



        public ConcurrentQueue<byte[]> DataQueue = new ConcurrentQueue<byte[]>();
        /// <summary>
        /// 添加加密后的数据
        /// 通常这个用于广播。一次加密码好多次使用
        /// </summary>
        /// <param name="gde"></param>
        public void AddSendData(byte[] buffer)
        {
            DataQueue.Enqueue(buffer);
        }


        public void AddSendData(OriginalData od)
        {
            TransPackProcess tpp = new TransPackProcess();
            byte[] buffer = Tool4DataProcess.HexStrTobyte(tpp.ClientPackingAsHexString(od));
            DataQueue.Enqueue(buffer);
        }

        public void AddSendData(byte cmd, byte[] one, byte[] two)
        {
            //pp.WriteClientData((byte)SephirothInstruction.ServerCmd.ISMainMsg.主动10, null, tx.toByte());
            OriginalData od = new OriginalData();
            od.cmd = cmd;
            od.One = one;
            od.Two = two;

            TransPackProcess tpp = new TransPackProcess();
            byte[] buffer = Tool4DataProcess.HexStrTobyte(tpp.ClientPackingAsHexString(od));
            DataQueue.Enqueue(buffer);
        }

        public EasyClient<BizPackageInfo> client;
        private bool isConnected = false;

        public bool IsConnected { get => isConnected; set => isConnected = value; }

        /// <summary>
        /// socket验证成功后的结果
        /// 指示登陆的实际状态，登陆了才有心跳。因为心跳放到了连接成功后。所以在这里添加了一个属性来确认 by watson 2024-3-11
        /// </summary>
        public bool LoginStatus { get; set; } = false;
        /// <summary>
        /// 标识是否登陆成功过
        /// </summary>
        public bool LoginSuccessed { get; set; } = false;

        public string ServerIp { get => _ip; set => _ip = value; }
        public int Port { get => _port; set => _port = value; }

        private string _ip;
        private int _port;

        static System.Timers.Timer timer = null;
        public EasyClientService()
        {
            _stopThreadEvent = new AutoResetEvent(false);
            client = new EasyClient<BizPackageInfo>();
            client.Initialize(new BizPipelineFilter());
            client.Connected += OnClientConnected;
            client.NewPackageReceived += OnPackageReceived;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;
            string testMesg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId} 启动线程测试";
            MessageBox.Show(testMesg);
            //每10s发送一次心跳或尝试一次重连
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
            {
                //心跳包
                if (client.IsConnected && LoginStatus)
                {
                    try
                    {
                        if (MainForm.Instance.AppContext.CurUserInfo != null)
                        {
                            OriginalData beatData = HeartbeatCmdBuilder();
                            MainForm.Instance.ecs.AddSendData(beatData);
                        }
                    }
                    catch (Exception)
                    {


                    }
                }
                //断线重连

                //else if (!client.IsConnected)
                //{
                //    try
                //    {
                //        Stop();
                //    }
                //    catch (Exception)
                //    {
                //    }
                //    //client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
                //    MainForm.Instance.uclog.AddLog("正在重新连接...");
                //    Connect();
                //    //client.Connect(new IPEndPoint(IPAddress.Parse(_Ip), _Port));
                //}



            });
            timer.Enabled = true;
            timer.Start();


        }

        /// <summary>
        /// 客户端的心跳包
        /// </summary>
        /// <returns></returns>
        public static OriginalData HeartbeatCmdBuilder()
        {
            OriginalData gd = new OriginalData();
            try
            {
                if (MainForm.HeartbeatCounter == 256)
                {
                    MainForm.HeartbeatCounter = 0;
                }
                var tx = new ByteBuff(36);

                tx.PushInt16(MainForm.HeartbeatCounter++); //累加数
                                                           //tx.PushInt(PlayerSession._Player.v位置.x);
                                                           //tx.PushInt(PlayerSession._Player.v位置.y);
                                                           //  tx.PushInt(newx);
                                                           // tx.PushInt(newy);

                //可以添加，离开电脑没有动桌面的秒数
                tx.PushInt(0);
                // long SysStatmp = SephirothInstruction.Helper.CommonTimeStamp.GetTimeStampSeconds(System.DateTime.Now);
                // tx.PushInt((int)SysStatmp);//系统当前时间  这里打开的话就会有这种错误  Too Fast Client Time  OST 1656993523 NST 1656993564 DST 41 OCT 17 NCT 91 DCT 74
                tx.PushInt64(MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                tx.PushString(System.Environment.MachineName + ":" + MainForm.Instance.AppContext.OnlineUser.客户端版本);
                MainForm.Instance.AppContext.OnlineUser.静止时间 = MainForm.GetLastInputTime();
                tx.PushInt64(MainForm.Instance.AppContext.OnlineUser.静止时间);//电脑空闲时间
                tx.PushString(MainForm.Instance.AppContext.log.Path);
                tx.PushString(MainForm.Instance.AppContext.log.ModName);

                gd.cmd = (byte)ClientCmdEnum.客户端心跳包;
                gd.One = null;
                gd.Two = tx.toByte();
                //  RoleService.实时更新相关数据(PlayerSession);
            }
            catch (Exception)
            {

            }
            return gd;
        }


        public async Task<bool> Connect()
        {
            MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}正在连接...");
            var connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port));
            if (connected)
            {
                //如果重连次数大于，认为是通过重新连接到服务器的。这时将手动登陆信息传入
                if (reConnect > 1)
                {
                    try
                    {
                        ServerAuthorizer serverAuthorizer = new ServerAuthorizer();
                        await serverAuthorizer.LongRunningOperationAsync(this, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 3);
                        UITools.SuperSleep(800);
                        if (this.LoginStatus)
                        {
                            MainForm.Instance.logger.LogInformation("成功恢复与服务器的连接。");
                        }
                    }
                    catch (Exception)
                    {


                    }
                    reConnect = 0;
                }

                //连接成功
                IsConnected = true;
                _stopThreadEvent.Reset(); // 重置停止事件

                // 检查是否需要重新启动发送数据线程
                _threadSendDataWorker = new Thread(DoWork)
                {
                    IsBackground = true
                };

                if (_threadSendDataWorker.ThreadState == (System.Threading.ThreadState.Unstarted | System.Threading.ThreadState.Background))
                {
                    try
                    {
                        _threadSendDataWorker.Start();
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.logger.LogError(ex, "线程启动出错:" + ex.Message);
                    }
                }

            }
            else
            {
                //连接失败
                IsConnected = false;

            }
            return IsConnected;
        }

        public async void Stop()
        {
            await client.Close();
            _stopThreadEvent.Set();
            if (_threadSendDataWorker != null && _threadSendDataWorker.ThreadState == System.Threading.ThreadState.Running)
            {
                try
                {
                    _threadSendDataWorker.Join();
                    _threadSendDataWorker.Abort();
                }
                catch (ThreadInterruptedException)
                {
                    // 处理线程中断异常
                }
                catch (ThreadAbortException)
                {
                    // 处理线程中止异常
                }
                finally
                {
                    _threadSendDataWorker = null;
                }
            }
        }


        ///连接上之后断开必然会触发OnClosed事件;
        //private async void OnClientClosed(object sender, EventArgs e)
        //{
        //    IsConnected = false;
        //    if (OnConnectClosed != null)
        //    {
        //        OnConnectClosed(IsConnected);
        //    }
        //    userInfos = new List<OnlineUserInfo>();
        //    Thread.Sleep(10);
        //    _stopThreadEvent.Set();
        //    if (_threadSendDataWorker != null)
        //    {
        //        _threadSendDataWorker.Join();
        //        _threadSendDataWorker = null;
        //    }
        //    //重新连接
        //    //如果只执行连接，没有注册dowork
        //    //await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port));
        //    await Connect();
        //}


        private async void OnClientClosed(object sender, EventArgs e)
        {
            // 设置连接状态为断开
            IsConnected = false;

            // 触发连接关闭事件
            if (OnConnectClosed != null)
            {
                OnConnectClosed(IsConnected);
            }

            // 清空用户信息列表
            lock (userInfos)
            {
                userInfos.Clear();
            }

            // 通知发送数据线程停止
            _stopThreadEvent.Set();

            // 等待发送数据线程结束
            if (_threadSendDataWorker != null && _threadSendDataWorker.ThreadState == System.Threading.ThreadState.Running)
            {
                try
                {
                    await Task.Run(() => _threadSendDataWorker.Join());
                }
                catch (ThreadInterruptedException)
                {
                    // 处理线程中断异常
                }
                catch (ThreadAbortException)
                {
                    // 处理线程中止异常
                }
                finally
                {
                    _threadSendDataWorker = null;
                }
            }

            // 记录日志
            MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}连接已关闭，准备重新连接...");

            // 引入延迟重连策略
            await Task.Delay(2000); // 延迟1秒，可以根据实际情况调整

            // 尝试重新连接
            await Connect();
        }


        private async void OnPackageReceived(object sender, PackageEventArgs<BizPackageInfo> e)
        {
            if (e.Package.ecode == SpecialOrder.正常)
            {
                #region  

                try
                {
                    string rs = string.Empty;
                    ServerCmdEnum msg = (ServerCmdEnum)e.Package.od.cmd;
                    OriginalData od = e.Package.od;
                    switch (msg)
                    {
                        case ServerCmdEnum.发送缓存数据列表:
                            ClientService.接收缓存数据列表(od);
                            break;
                        case ServerCmdEnum.工作流数据推送:
                            WorkflowService.接收工作流数据(od);
                            break;

                        case ServerCmdEnum.删除列的配置文件:
                            MainForm.Instance.DeleteColumnsConfigFiles();
                            break;

                        case ServerCmdEnum.未知指令:
                            break;

                        case ServerCmdEnum.通知审批人审批:
                            //这里会弹出内容，没有实现具体功能前不生效
                            //WorkflowService.接收服务器审核通知(od);
                            break;
                        case ServerCmdEnum.通知相关人员审批完成:
                            WorkflowService.接收服务器审核完成通知(od);
                            break;
                        case ServerCmdEnum.用户登陆回复:
                            bool Successed = ClientService.用户登陆回复(od);
                            LoginStatus = Successed;
                            LoginSuccessed = Successed;
                            break;
                        case ServerCmdEnum.发送在线列表:
                            //try
                            //{
                            //    MainForm.Instance.Invoke(new Action(() =>
                            //    {
                            //        IM.UCMessager.Instance.listboxForUsers.Items.Clear();
                            //    }));

                            //}
                            //catch (Exception)
                            //{

                            //}
                            //ClientService.接收在线用户列表(od);
                            break;
                        case ServerCmdEnum.转发消息:
                            //别人发消息过来了
                            #region 
                            //IM.UCMessager.Instance.接收消息(od);
                            #endregion

                            break;
                        case ServerCmdEnum.关机:
                            //执行关机代码
                            System.Diagnostics.Process.Start("shutdown", "/s /t 20");
                            MainForm.Instance.uclog.AddLog($"关机关机关机关机");
                            break;
                        case ServerCmdEnum.推送版本更新:
                            await MainForm.Instance.UpdateSys(false);
                            break;
                        case ServerCmdEnum.强制用户退出:
                            //加个倒计时？
                            // MainForm.Instance.Close();
                            //System.Windows.Forms.Application.Exit();
                            System.Environment.Exit(0);
                            Process.GetCurrentProcess().Kill();
                            break;

                        case ServerCmdEnum.给客户端发提示消息:
                            //string cmsg = IM.UCMessager.Instance.接收消息(od);
                            //MainForm.Instance.ShowMsg(cmsg);
                            break;
                        case ServerCmdEnum.心跳回复:
                            break;
                        default:
                            break;
                    }
                    //ise.ClientActionDefault
                    if (TransService.ServerActionList.Count > 0)
                    {
                        rs += TransService.PorcessServerMsg(msg, TransService.ServerActionList[msg], od);
                        rs += msg.ToString() + "|";
                        MainForm.Instance.PrintInfoLog("【收到服务器数据】" + rs.ToString());
                    }



                }
                catch (Exception ex)
                {
                    MainForm.Instance.PrintInfoLog("Server", ex);
                }

                #endregion
            }
        }


        int reConnect = 1;

        /// <summary>
        /// 没有连接上的时候尝试连接失败，可能只会触发OnError事件;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnClientError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            MainForm.Instance.uclog.AddLog("与服务器连接异常:" + e.Exception.Message);
            MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}准备重新连接...");
            IsConnected = false;
            userInfos = new List<UserInfo>();
            Thread.Sleep(2000);
            reConnect++;
            if (reConnect > 30)
            {
                MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}系统连接超时...一分钟后强制退出，请保存数据。");
                this.LoginStatus = false;
                if (reConnect > 90)
                {
                    System.Windows.Forms.Application.Exit();
                }

            }
            //重新连接
            //如果只执行连接，没有注册dowork
            //await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port));
            await Connect();
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            IsConnected = true;
            //如果验证通过才是登陆成功
            if (true)
            {
                this.LoginStatus= false;
            }
            //连上就需要做一些动作，如果登陆成功过的。
            /*
            if (this.LoginStatus == false)
            {
                try
                {
                    ServerAuthorizer serverAuthorizer = new ServerAuthorizer();
                    await serverAuthorizer.LongRunningOperationAsync(this, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 5);
                    UITools.SuperSleep(3000);
                    if (this.LoginStatus)
                    {
                        MainForm.Instance.logger.LogInformation("成功登陆服务器");
                    }
                    else
                    {
                        MainForm.Instance.logger.LogInformation("验证失败或超时请重试");
                    }
                }
                catch (Exception)
                {


                }
            }
            //获取用户列表
            */
            MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}连接成功{LoginStatus},{this.client.LocalEndPoint}");
        }

        #region 线程

        /// <summary>
        /// 发送数据线程
        /// </summary>
        private Thread _threadSendDataWorker;
        private readonly AutoResetEvent _stopThreadEvent;

        #endregion


        private void DoWork()
        {
            var random = new Random(DateTime.Now.Millisecond);
            while (true)
            {
                Thread.Sleep(300);
                //var data = GetRandomData(random);
                while (DataQueue.Count > 0)
                {
                    byte[] sendData;
                    bool rs = DataQueue.TryDequeue(out sendData);
                    if (sendData != null && rs)
                    {
                        if (sendData.Length > 0)
                        {
                            client.Send(sendData);
                        }
                    }
                }

                //10s发送一次数据
                if (_stopThreadEvent.WaitOne(100))
                {
                    break;
                }
            }

            // Console.WriteLine($@"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} DoWork exit.");
        }



    }
}
