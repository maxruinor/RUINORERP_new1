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
using System.Drawing;

namespace RUINORERP.UI.SuperSocketClient
{
    public class EasyClientService
    {



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

        /// <summary>
        /// 添加加密前的原始数据，经过加密后添加到队列中
        /// </summary>
        /// <param name="od"></param>
        public void AddSendData(OriginalData od)
        {
            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(od);
            DataQueue.Enqueue(buffer);
        }

        /// <summary>
        /// 添加加密前的原始数据，经过加密后添加到队列中
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        public void AddSendData(byte cmd, byte[] one, byte[] two)
        {
            OriginalData od = new OriginalData();
            od.cmd = cmd;
            od.One = one;
            od.Two = two;
            byte[] buffer = CryptoProtocol.EncryptClientPackToServer(od);
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
                else
                {

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
                tx.PushInt(0);
                tx.PushInt64(MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID);
                if (MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee != null)
                {
                    tx.PushString(MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name);
                }
                else
                {
                    tx.PushString("");
                }
                tx.PushString(System.Environment.MachineName + ":" + MainForm.Instance.AppContext.OnlineUser.客户端版本);
                MainForm.Instance.AppContext.OnlineUser.静止时间 = MainForm.GetLastInputTime();
                tx.PushInt64(MainForm.Instance.AppContext.OnlineUser.静止时间);//电脑空闲时间
                tx.PushString(MainForm.Instance.AppContext.log.Path);
                tx.PushString(MainForm.Instance.AppContext.log.ModName);
                tx.PushBool(MainForm.Instance.AppContext.OnlineUser.在线状态);
                tx.PushBool(MainForm.Instance.AppContext.OnlineUser.授权状态);
                tx.PushString(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));//客户端时间，用来对比服务器的时间，如果多个客户端时间与服务器不一样。则服务器有问题。
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

        private Task _connectTask;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private volatile bool _isConnecting = false;
        private int _reconnectAttempts = 0;
        private const int MaxReconnectAttempts = 10;
        private const int ReconnectInterval = 5000; // 重连间隔时间，单位为毫秒
        public async Task<bool> Connect()
        {
            if (_isConnecting) // 如果已经在连接中，直接返回
            {
                return false;
            }
            _cancellationTokenSource.Cancel(); // 取消之前的连接尝试
            _cancellationTokenSource = new CancellationTokenSource(); // 创建新的CancellationTokenSource
            _isConnecting = true;
            try
            {
                _cancellationTokenSource.Token.ThrowIfCancellationRequested(); // 如果已经被取消，抛出异常

                MainForm.Instance.uclog.AddLog($"{System.Threading.Thread.CurrentThread.ManagedThreadId}正在连接...");
                //var connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port));
                var connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port), _cancellationTokenSource.Token);
                if (connected)
                {
                    await ConnectSuccessed();
                }
                else
                {
                    //连接失败
                    IsConnected = false;

                }
                return IsConnected;
            }
            catch (OperationCanceledException)
            {
                // 连接被取消
                MainForm.Instance.uclog.AddLog("连接尝试被取消。");
            }
            finally
            {
                _isConnecting = false; // 完成连接尝试，无论成功与否
            }
            return IsConnected;
        }


        public async Task<bool> ConnectSuccessed()
        {
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

            //如果重连次数大于1，认为是通过重新连接到服务器的。
            //这时将手动登陆信息传入 TODO实际要看之前是不是登陆成功的状态。
            if (_reconnectAttempts > 1)
            {
                try
                {
                    ServerAuthorizer serverAuthorizer = new ServerAuthorizer();
                    bool result = await serverAuthorizer.loginRunningOperationAsync(this, UserGlobalConfig.Instance.UseName, UserGlobalConfig.Instance.PassWord, 3);
                    if (result)
                    {
                        MainForm.Instance.logger.LogInformation("成功恢复与服务器的连接。");
                    }
                }
                catch (Exception)
                {


                }

            }


            return IsConnected;
        }


        public async Task<bool> Connect(CancellationToken cancellationToken)
        {
            string msg = string.Empty;
            if (_isConnecting)
            {
                return false;
            }
            _isConnecting = true;
            try
            {
                msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId}正在连接服务器{ServerIp}:{Port}...";
                MainForm.Instance.uclog.AddLog(msg);
                MainForm.Instance.lblServerInfo.Text = msg;
                var connected = await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port), cancellationToken);
                if (connected)
                {
                    // 连接成功
                    IsConnected = true;
                    // ... 连接成功后的操作 ...
                    await ConnectSuccessed();
                }
                else
                {
                    // 连接失败
                    IsConnected = false;
                }
                return connected;
            }
            catch (OperationCanceledException)
            {
                // 连接被取消
                MainForm.Instance.uclog.AddLog("连接尝试被取消。");
                msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId}连接尝试被取消{ServerIp}:{Port}...";
                MainForm.Instance.uclog.AddLog(msg);
                MainForm.Instance.lblServerInfo.Text = msg;
            }
            catch (Exception ex)
            {
                // 处理其他异常情况
                MainForm.Instance.uclog.AddLog($"连接异常: {ex.Message}");
            }
            finally
            {
                _isConnecting = false; // 完成连接尝试，无论成功与否
            }
            return IsConnected;
        }


        public async Task<bool> Stop()
        {
            if (_isConnecting)
            {
                _cancellationTokenSource.Cancel(); // 取消正在进行的连接尝试
            }
            bool rs = await client.Close(); // 关闭客户端连接
            if (rs)
            {
                isConnected = false;
            }
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
            return rs;
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

            //// 清空用户信息列表
            //lock (userInfos)
            //{
            //    userInfos.Clear();
            //}

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

            // 尝试重新连接
            //await Connect();
            //如何登陆成功过。则一直重新连接。否则只重新连接一次。需要点击连接按钮才会重新连接
            if (LoginStatus)
            {  // 引入延迟重连策略
                while (_reconnectAttempts < MaxReconnectAttempts)
                {
                    bool reconnectSuccess = await Reconnect();
                    if (reconnectSuccess)
                    {
                        break;
                    }
                }
                MainForm.Instance.LogLock();
            }
            else
            {
                await Reconnect();
            }
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
                        case ServerCmdEnum.工作流提醒推送:
                            ClientService.接收服务器工作流的提醒消息(od);
                            break;

                        case ServerCmdEnum.转发单据锁定:
                            //单个实例
                            ClientService.接收转发单据锁定(od);
                            break;
                        case ServerCmdEnum.转发单据锁定释放:
                            //单个实例
                            ClientService.接收转发单据锁定释放(od);
                            break;
                        case ServerCmdEnum.根据锁定用户释放:
                            //单个实例
                            ClientService.接收根据锁定用户释放(od);
                            break;
                        case ServerCmdEnum.转发更新缓存:
                            //单个实例
                            ClientService.接收转发更新缓存(od);
                            break;
                        case ServerCmdEnum.转发删除缓存:
                            //单个实例
                            ClientService.接收转发删除缓存(od);
                            break;
                        case ServerCmdEnum.发送缓存数据列表:
                            //实例集合
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
                            Program.AppContextData.IsOnline = Successed;
                            break;
                        case ServerCmdEnum.回复用户重复登陆:
                            Program.AppContextData.AlreadyLogged = ClientService.接收回复用户重复登陆(od);
                            break;
                        case ServerCmdEnum.发送在线列表:
                            ClientService.接收在线用户列表(od);
                            break;
                        case ServerCmdEnum.发送缓存信息列表:
                            ClientService.接收缓存信息列表(od);
                            break;
                        case ServerCmdEnum.转发弹窗消息:
                            //别人发消息过来了
                            #region 
                            ClientService.接收服务器弹窗消息(od);


                            #endregion

                            break;
                        case ServerCmdEnum.转发异常:
                            //服务转发异常来了。管理员看看啊。
                            #region 
                            ClientService.接收服务器转发异常消息(od);
                            #endregion
                            break;
                        case ServerCmdEnum.转发协助处理:
                            //服务转发协助处理:来了。管理员看看啊。
                            #region 
                            ClientService.接收服务器转发的协助处理请求(od);
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
                            try
                            {
                                // 保存当前工作
                                // SaveCurrentWork();

                                // 通知用户
                                // NotifyUserAboutForcedExit();

                                // 关闭所有资源
                                //CloseAllResources();
                                int index = 0;
                                ByteBuff bg = new ByteBuff(od.Two);
                                string msg1 = ByteDataAnalysis.GetString(od.Two, ref index);
                                string msg2 = ByteDataAnalysis.GetString(od.Two, ref index);
                                if (!string.IsNullOrEmpty(msg2))
                                {
                                    // MessageBox.Show(msg2, msg1, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //提示3秒后退出
                                    MainForm.Instance.ShowMsg(msg2, "系统5秒后退出");
                                    Thread.Sleep(5000);
                                }
                                // 安全退出
                                Environment.Exit(0);
                            }
                            catch (Exception ex)
                            {
                                // 异常处理
                                // LogError(ex, "强制用户退出时发生错误");
                            }
                            Process.GetCurrentProcess().Kill();
                            break;

                        case ServerCmdEnum.给客户端发提示消息:
                            ClientService.接收服务器弹窗消息(od);

                            //尝试找到销售订单：
                            //var ss = MainForm.Instance.kryptonDockableWorkspace1.ActiveCell.Pages.Count;

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
            string msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId},准备重新连接...";
            MainForm.Instance.uclog.AddLog(msg);
            MainForm.Instance.lblServerInfo.Text = msg;
            IsConnected = false;
            reConnect++;
            if (reConnect > 30)
            {
                msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId},系统连接超时...一分钟后强制退出，请保存数据。";
                MainForm.Instance.uclog.AddLog(msg);
                MainForm.Instance.lblServerInfo.Text = msg;
                this.LoginStatus = false;
                if (reConnect > 90)
                {
                    System.Windows.Forms.Application.Exit();
                }

            }
            //重新连接
            //如果只执行连接，没有注册dowork
            //await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(ServerIp), Port));
            //await Connect();
            await Reconnect();
        }

        private async Task<bool> Reconnect()
        {
            bool connected = false;
            if (_isConnecting || _reconnectAttempts >= MaxReconnectAttempts)
            {
                return false;
            }
            _reconnectAttempts++;
            try
            {
                _cancellationTokenSource.Cancel(); // 取消之前的重连尝试
                _cancellationTokenSource = new CancellationTokenSource(); // 创建新的CancellationTokenSource

                // 等待一段时间再尝试重连
                await Task.Delay(ReconnectInterval, _cancellationTokenSource.Token);
                string msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId}尝试第{_reconnectAttempts}次连接{ServerIp}:{Port}...";
                MainForm.Instance.uclog.AddLog(msg);
                MainForm.Instance.lblServerInfo.Text = msg;

                // 尝试重新连接
                connected = await Connect(_cancellationTokenSource.Token);
                if (connected)
                {
                    _reconnectAttempts = 0; // 重置重连尝试次数

                }
            }
            catch (OperationCanceledException)
            {
                // 重连尝试被取消
            }
            finally
            {
                _isConnecting = false; // 完成重连尝试，无论成功与否
            }
            return connected;
        }


        private void OnClientConnected(object sender, EventArgs e)
        {
            IsConnected = true;
            //如果验证通过才是登陆成功
            if (Program.AppContextData.IsOnline)
            {
                this.LoginStatus = true;
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
            string msg = $"{System.Threading.Thread.CurrentThread.ManagedThreadId}连接成功{LoginStatus},{this.client.LocalEndPoint}";
            MainForm.Instance.uclog.AddLog(msg);
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
