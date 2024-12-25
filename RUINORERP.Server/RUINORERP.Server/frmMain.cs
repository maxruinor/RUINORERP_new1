using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Dm.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.Extensions.Options;
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Log4Net;
using RUINORERP.Extensions;
using RUINORERP.Extensions.Middlewares;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.BizService;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Commands;
using RUINORERP.Server.ServerService;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Workflow.WFReminder;
using SharpYaml.Tokens;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Abstractions.Session;
using SuperSocket.Server.Host;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using TransInstruction.CommandService;
using WorkflowCore.Interface;
using WorkflowCore.Primitives;
using WorkflowCore.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace RUINORERP.Server
{
    public partial class frmMain : Form
    {

        /// <summary>
        /// 系统保护数据
        /// </summary>
        public SystemProtectionData protectionData = new SystemProtectionData();

        /// <summary>
        /// 可配置性全局参数
        /// </summary>
        public readonly IOptionsMonitor<SystemGlobalconfig> Globalconfig;


        //保存要更新 分发的配置数据，（客户不在线时） 是不是 类似的情况。通知都是这样。比方管理员发布一个通知 在线吗上收到。没在线的 上线后收到
        public ConcurrentDictionary<long, BaseConfig> UpdateConfigDataList = new ConcurrentDictionary<long, BaseConfig>();



        /// <summary>
        /// 保存服务器的一些缓存信息。让客户端可以根据一些机制来获取。得到最新的信息
        /// 将来要考虑放到还有其它信息时，key中换为实例。查找时添加类型? 将这个数据保存到缓存中，不过期。定时更新
        /// </summary>
        // public ConcurrentDictionary<string, CacheInfo> CacheInfoList = new ConcurrentDictionary<string, CacheInfo>();
        //MyCacheManager Cache 代替上面的集合


        //保存系统所有提醒的业务数据配置,系统每分钟检测。
        public ConcurrentDictionary<long, ReminderData> ReminderBizDataList = new ConcurrentDictionary<long, ReminderData>();

        /// <summary>
        /// 保存启动的工作流队列 2023-11-18
        /// 暂时用的是通过C端传的单号来找到对应的流程号。实际不科学
        /// </summary>
        public ConcurrentDictionary<string, string> workflowlist = new ConcurrentDictionary<string, string>();
        public ILogger<frmMain> _logger { get; set; }


        //一个消息缓存列表，有处理过的。未处理的。未看的。临时性还是固定到表的？

        public Queue<ReminderData> MessageList = new Queue<ReminderData>();
        public IServiceCollection _services { get; set; }
        public IServiceProvider _ServiceProvider { get; set; }

        public bool IsDebug { get; set; } = false;

        private static frmMain _main;
        public static frmMain Instance
        {
            get { return _main; }
        }

        public IWorkflowHost host;
        public frmMain(ILogger<frmMain> logger, IWorkflowHost workflowHost, IOptionsMonitor<SystemGlobalconfig> config)
        {
            InitializeComponent();
            _main = this;
            _logger = logger;
            _services = Startup.Services;
            host = workflowHost;


            Globalconfig = config;
            // 监听配置变化
            Globalconfig.OnChange(updatedConfig =>
            {
                Console.WriteLine($"Configuration has changed: {updatedConfig.SomeSetting}");
            });


            //ILoggerFactory loggerFactory = LoggerFactory.Create(logbuilder => logbuilder
            // .AddFilter("Microsoft", LogLevel.Debug)
            // .AddFilter("System", LogLevel.Debug)
            // .AddFilter("Namespace.Class", LogLevel.Debug)
            // //.AddFile()//成功写入到文件
            // .AddLog4Net()
            //);
            //_logger = loggerFactory.CreateLogger<frmMain>();


            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }


        List<SessionforLander> sessionListLander = new List<SessionforLander>();
        internal ConcurrentDictionary<string, SessionforBiz> sessionListBiz = new ConcurrentDictionary<string, SessionforBiz>();

        async private void StartServerUI()
        {
            Application.DoEvents();
            // timerServerInfo.Start();
            //btnStartServer.Enabled = false;
            tsBtnStartServer.Enabled = false;
            ServerStart = true;
            try
            {
                _logger.Error("ErrorError11");
                _logger.LogError("LogErrorLogError11");
                PrintInfoLog("开始启动服务器");
                _logger.LogInformation("开始启动socket服务器");
                // BaseKXGame.Instance.Initinal();
                //log4netHelper.info("开始启动服务器");
                //Tools.ShowMsg("RoleService Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                //准备工作 准备数据

                //启动socket
                frmMain.Instance.PrintInfoLog("StartServerUI Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
                StartSendData();
                await StartServer();

            }
            catch (Exception ex)
            {
                frmMain.Instance._logger.LogError("StartServerUI", ex);
                //log4netHelper.error("StartServer总异常", ex);
                _logger.LogInformation(ex, "StartServer总异常");
                Console.WriteLine("StartServer总异常" + ex.Message);
                //throw;
            }
        }


        private void StartSendData()
        {
            Task.Run(() => { StartSendService(); });
        }

        public Task StartSendService()
        {
            frmMain.Instance.PrintInfoLog("StartSendService Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //https://www.cnblogs.com/doforfuture/p/6293926.html
            while (true)
            {
                try
                {
                    Thread.Sleep(10);
                    if (frmMain.Instance.sessionListBiz.Count > 0)
                    {
                        foreach (var item in frmMain.Instance.sessionListBiz)
                        {
                            // SessionforGame sg;
                            while (item.Value.DataQueue.Count > 0)
                            {
                                byte[] sendData;
                                bool rs = item.Value.DataQueue.TryDequeue(out sendData);
                                if (sendData != null && rs)
                                {
                                    if (sendData.Length > 338420)
                                    {

                                    }
                                    if (sendData.Length > 0)
                                    {
                                        item.Value.Send(sendData);
                                    }

                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    frmMain.Instance.PrintInfoLog(ex.Message + "StartSendService!#");
                    _logger.LogError("数据发送服务出错:Task StartSendService", ex);
                }
            }

        }

        bool ServerStart = false;
        static System.Timers.Timer timer = null;

        static System.Timers.Timer ReminderTimer = null;

        frmUserManage frmusermange = Startup.GetFromFac<frmUserManage>();

        private async void frmMain_Load(object sender, EventArgs e)
        {
            protectionData = new SystemProtectionData();
            _logger.Error("ErrorError2233");
            _logger.LogError("LogErrorLogError2233");

            // var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
            //logger.LogError($"{DateTime.Now} LogError 日志");

            _logger.LogError("启动了服务器123");
            this.IsMdiContainer = true; // 设置父窗体为MDI容器
            menuStrip1.MdiWindowListItem = 窗口ToolStripMenuItem;
            //InitAll();

            //手动初始化
            BizCacheHelper.Instance = Startup.GetFromFac<BizCacheHelper>();
            BizCacheHelper.InitManager();

            IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
            cache.Set("test1", "test123");
            await InitConfig(false);

            MyCacheManager.Instance.CacheInfoList.OnAdd += CacheInfoList_OnAdd;
            MyCacheManager.Instance.CacheInfoList.OnClear += CacheInfoList_OnClear;
            MyCacheManager.Instance.CacheInfoList.OnUpdate += CacheInfoList_OnUpdate;
            MyCacheManager.Instance.CacheEntityList.OnRemove += CacheEntityList_OnRemove;
            //1分钟检查一次
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(() =>

                    {
                        CheckSystemProtection();
                        CheckCacheList();
                        CheckReminderBizDataList();
                    }



                    ));
                }
                else
                {
                    CheckSystemProtection();
                    CheckCacheList();
                    CheckReminderBizDataList();
                }
            });
            timer.Enabled = true;




            //1分钟检查一次
            //ReminderTimer = new System.Timers.Timer(60000);
            //ReminderTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
            //{
            //    if (this.InvokeRequired)
            //    {
            //        this.Invoke(new MethodInvoker(() => CheckReminderBizDataList()));
            //    }
            //    else
            //    {
            //        CheckReminderBizDataList();
            //    }
            //});
            //ReminderTimer.Enabled = true;
            //ReminderTimer.Start();


        }

        private void CacheInfoList_OnUpdate(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
            {
                BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheInfoList_OnClear(object sender, CacheManager.Core.Internal.CacheClearEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
            {
                BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheInfoList_OnAdd(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
            {
                BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }

        private void CacheEntityList_OnRemove(object sender, CacheManager.Core.Internal.CacheActionEventArgs e)
        {
            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
            {
                BizService.UserService.发送缓存信息列表(PlayerSession);
            }
        }


        private bool CheckSystemProtection()
        {
            bool result = false;
            try
            {
                if (protectionData != null)
                {
                    if (!protectionData.IsProtectionEnabled)
                    {
                        return false;
                    }
                    else
                    {
                        //如果当前时间小于限制时间。就退出
                        if (protectionData.ExpirationDate.Date <= protectionData.GetLocalTime().Date)
                        {
                            result= true;
                            //断开所有链接
                            Shutdown();
                            Application.Exit();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                frmMain.Instance.PrintInfoLog($"CheckSystemProtection：{ex.Message} ");
            }
            return result;
        }

        private async void CheckReminderBizDataList()
        {
            #region 根据ReminderBizDataList检查各种提醒的业务是不是要启动工作流了。
            try
            {
                foreach (var item in ReminderBizDataList)
                {
                    ReminderData BizData = item.Value;
                    if (BizData != null)
                    {
                        //这里要判断规则，目前暂时todo 写死,提前一天启动提醒
                        //如果启动时间 提前1天
                        if (System.DateTime.Now > BizData.StartTime.AddDays(-1) && System.DateTime.Now < BizData.EndTime && string.IsNullOrEmpty(BizData.WorkflowId))
                        {
                            //启动
                            var workflowId = await host.StartWorkflow("ReminderWorkflow", 1, BizData);
                            BizData.WorkflowId = workflowId;
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"启动{BizData.BizType} ，成功启动提醒工作流{workflowId}。");
                            }
                            continue;
                        }
                    }
                    else
                    {
                        if (frmMain.Instance.IsDebug)
                        {
                            frmMain.Instance.PrintInfoLog($"提醒业务数据为空：{item.Key}。");
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                frmMain.Instance.PrintInfoLog($"根据CheckReminderBizDataList检测数据启动工作流时出错：{exc.Message} ");
            }
            #endregion

        }

        private void CheckCacheList()
        {
            #region 根据CacheInfoList检查更新过期的缓存。
            try
            {

                foreach (var item in BizCacheHelper.Manager.NewTableList)
                {
                    CacheInfo cacheInfo = MyCacheManager.Instance.CacheInfoList.Get(item.Key) as CacheInfo;
                    if (cacheInfo != null)
                    {
                        if (!MyCacheManager.Instance.CacheEntityList.Exists(item.Key))
                        {
                            BizCacheHelper.Instance.SetDictDataSource(item.Key, true);
                            if (frmMain.Instance.IsDebug)
                            {
                                frmMain.Instance.PrintInfoLog($"检查更新过期的缓存 ，成功添加{item.Key}。");
                            }
                            //只有缓存概率有变化就发到客户端。客户端再根据这个与他本地实际的缓存数据行对比来请求真正的缓存数据
                            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
                            {
                                BizService.UserService.发送缓存信息列表(PlayerSession);
                            }
                        }
                    }
                    else
                    {
                        //if (frmMain.Instance.IsDebug)
                        //{
                        //    frmMain.Instance.PrintInfoLog($"检查更新过期的缓存时没有找到{item.Key}概览信息。");
                        //}
                    }
                }
            }
            catch (Exception exc)
            {
                frmMain.Instance.PrintInfoLog($"根据CacheInfoList检查更新过期的缓存出错：{exc.Message} ");
            }
            #endregion

        }

        /// <summary>
        /// 定时器会可能停止 可能是线程退出了
        /// </summary>
        /// <param name="state"></param>
        private void CheckAndRemoveExpiredSessions(object state)
        {
            try
            {
                var currentTime = DateTime.Now;
                var keysToRemove = new ConcurrentBag<string>();

                // 遍历会话集合，检查每个会话的LastActiveTime
                foreach (var kvp in sessionListBiz)
                {
                    if ((currentTime - kvp.Value.LastActiveTime).TotalMinutes > 1)
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // 删除过期的会话
                foreach (var key in keysToRemove)
                {
                    if (sessionListBiz.TryRemove(key, out var removedSession))
                    {
                        Console.WriteLine($"移除过期会话: {key}");
                    }
                }


            }
            catch (Exception ex)
            {
                //线程
                //frmMain.Instance.PrintInfoLog($"CheckAndRemoveExpiredSessions{ex.Message}。");
                Console.WriteLine($"CheckAndRemoveExpiredSessions: {ex.Message}");
            }
        }

        public async Task InitConfig(bool LoadData = true)
        {
            BizCacheHelper cacheHelper = Startup.GetFromFac<BizCacheHelper>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            cacheHelper.InitDict(LoadData);
            stopwatch.Stop();
            frmMain.Instance.PrintInfoLog($"InitConfig总执行时间：{stopwatch.ElapsedMilliseconds} 毫秒");
            await Task.Delay(0);
        }

        private async void InitAll()
        {
            if (!ServerStart)
            {
                DbInit();
                StartServerUI();
                RUINORERP.Extensions.SqlsugarSetup.CheckEvent += SqlsugarSetup_CheckEvent;
            }
            else
            {
                frmMain.Instance.PrintInfoLog("服务器已经启动");
            }


            // 每120秒（120000毫秒）执行一次检查
            System.Threading.Timer timerStatus = new System.Threading.Timer(CheckAndRemoveExpiredSessions, null, 0, 1000);

            //加载提醒数据

            DataServiceChannel loadService = Startup.GetFromFac<DataServiceChannel>();
            loadService.LoadCRMFollowUpPlansData(ReminderBizDataList);

            //启动每天要执行的定时任务
            //启动
            var DailyworkflowId = await host.StartWorkflow("DailyTaskWorkflow", 1, null);

            frmMain.Instance.PrintInfoLog($"每日任务启动{DailyworkflowId}。");
        }

        IHost _host = null;
        async Task StartServer()
        {
            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);


            try
            {
                _host = MultipleServerHostBuilder.Create()

                    /*
                             //登陆器
                             .AddServer<ServiceforLander<LanderPackageInfo>, LanderPackageInfo, LanderCommandLinePipelineFilter>(builder =>
                             {
                                 builder.ConfigureServerOptions((ctx, config) =>
                                 {
                                     //获取服务配置
                                     return config.GetSection("ServiceforLander");
                                 })
                             .UseSession<SessionforLander>()
                             //注册用于处理连接、关闭的Session处理器
                             .UseSessionHandler(async (session) =>
                             {
                                 sessionListLander.Add(session as SessionforLander);
                                 PrintMsg($"{DateTime.Now} [SessionforLander] Session-登陆器 connected: {session.RemoteEndPoint}");
                                 await Task.Delay(0);
                             }, async (session, reason) =>
                             {
                                 sessionListLander.Remove(session as SessionforLander);
                                 PrintMsg($"{DateTime.Now} [SessionforLander] Session-登陆器 {session.RemoteEndPoint} closed: {reason}");
                                 await Task.Delay(0);
                             })
                          //.ConfigureServices((context, services) =>
                          //{

                          //})
                          .UseCommand(commandOptions =>
                              {
                                  commandOptions.AddCommand<BaseCommand>();
                                  commandOptions.AddCommand<getmsgCommand>();
                                  commandOptions.AddCommand<loginCommand>();
                                  commandOptions.AddCommand<LanderCommand>();
                              });
                             })
                    */
                    //一线
                    .AddServer<ServiceforBiz<BizPackageInfo>, BizPackageInfo, BizPipelineFilter>(builder =>
                    {
                        builder.ConfigureServerOptions((ctx, config) =>
                        {
                            //获取服务配置
                            // ReSharper disable once ConvertToLambdaExpression
                            return config.GetSection("ServiceforBiz");
                        })
                        .UsePackageDecoder<MyPackageDecoder>()//注册自定义解包器
                        .UseSession<SessionforBiz>()

                    //注册用于处理连接、关闭的Session处理器
                    .UseSessionHandler(async (session) =>
                    {
                        SessionforBiz sessionforBiz = session as SessionforBiz;
                        sessionforBiz.User.PropertyChanged -= frmusermange.UserInfo_PropertyChanged;
                        sessionforBiz.User.PropertyChanged += frmusermange.UserInfo_PropertyChanged;
                        sessionListBiz.TryAdd(session.SessionID, session as SessionforBiz);
                        frmusermange.userInfos.Add(sessionforBiz.User);
                        if (sessionforBiz.User != null)
                        {
                            while (MessageList.Count > 0 && sessionforBiz.User.超级用户)
                            {
                                ReminderData MessageInfo = MessageList.Dequeue();
                                SystemService.process请求协助处理(sessionforBiz, MessageInfo);
                            }

                        }
                        //广播出去
                        foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
                        {
                            BizService.UserService.发送在线列表(PlayerSession);
                        }

                        PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序] Session connected: {session.RemoteEndPoint}");
                        await Task.Delay(0);
                    }, async (session, reason) =>
                    {
                        try
                        {
                            SessionforBiz sg = session as SessionforBiz;
                            //if (sg.player != null && sg.player.Online)
                            //{
                            //   // SephirothServer.CommandServer.RoleService.角色退出(sg);
                            //}
                            PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序]  {session.RemoteEndPoint} closed，原因：: {reason.Reason}");
                            sessionListBiz.Remove(sg.SessionID, out sg);
                            if (sg != null)
                            {
                                PrintMsg(sg.User.用户名 + "断开连接");
                                frmusermange.userInfos.Remove(sg.User);
                            }

                            //谁突然掉线或退出。服务器主动将他的锁在别人电脑上的单据释放
                            SystemService.process断开连接锁定释放(sg.User.UserID);

                            //广播出去
                            foreach (SessionforBiz PlayerSession in sessionListBiz.Values)
                            {
                                BizService.UserService.发送在线列表(PlayerSession);
                            }

                        }
                        catch (Exception quitex)
                        {
                            frmMain.Instance._logger.LogError("客户端退出", quitex);
                        }

                        await Task.Delay(0);
                    })
                            .ConfigureServices((context, services) =>
                            {
                                IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
                                // services = Startup.Services;
                                //services.AddMemoryCache();
                                services.AddMemoryCacheSetupWithInstance(cache);

                                //services.AddSingleton<CommandDispatcher>();
                                //services.AddTransient<ICommandHandler, LoginCommandHandler>();

                                foreach (var service in Startup.Services)
                                {
                                    // 假设 service 是一个 ServiceDescriptor 对象
                                    // 将 service 注册添加到 OtherServices 中
                                    services.Add(service);
                                }

                            })

                            .UseCommand(commandOptions =>
                            {

                                commandOptions.AddCommand<BizCommand>();
                                commandOptions.AddCommand<XTCommand>();
                            });
                    })

                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders(); //去掉默认添加的日志提供程序
                    var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                    // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                    // the defaults be overridden by the configuration.
                    if (isWindows)
                    {
                        // Default the EventLogLoggerProvider to warning or above
                        logging.AddFilter<EventLogLoggerProvider>(level => level >= Microsoft.Extensions.Logging.LogLevel.Information);
                    }
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    if (isWindows)
                    {
                        // Add the EventLogLoggerProvider on windows machines
                        //logging.AddEventLog();//这个写到了事件查看器中。没有必要
                        logging.ClearProviders();
                        //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
                        //引用的long4net.dll要版本一样。
                        string conn = AppSettings.GetValue("ConnectString");
                        logging.AddLog4Net();
                        logging.AddProvider(new Log4NetProviderByCustomeDb("Log4net_db.config", conn, Program.AppContextData));

                    }
                })//.UseLog4Net()

            .Build();

            }
            catch (Exception hostex)
            {
                frmMain.Instance._logger.LogError("hostex", hostex);
            }


            try
            {
                await _host.RunAsync();
            }
            catch (Exception e)
            {
                frmMain.Instance.PrintInfoLog("_host.RunAsync()" + e.Message);
                _logger.LogError("socket _host RunAsync", e.Message);
            }

        }



        public void StartupServer()
        {
            try
            {
                _host.StartAsync().GetAwaiter();
            }
            catch (Exception e)
            {
                Instance.PrintInfoLog($"启动SocketServer失败：{e.Message}.");
            }
        }

        async public void Shutdown()
        {
            try
            {
                await DrainAllServers();
                // 记得在程序结束时清理定时器资源
                timer.Dispose();
                ReminderTimer.Dispose();
            }
            catch (Exception e)
            {
                Instance.PrintInfoLog($"关闭SocketServer失败：{e.Message}.");
            }
            finally
            {
                _host.Dispose();
                //关闭Socket服务器时，如果不对IHost对象调用Dispose方法，SuperSocket不会真正关闭，导致WPF进程无法正常退出。
            }
        }

        public async Task DrainAllServers()
        {
            if (_host == null)
            {
                return;
            }
            //_host.StopAsync();
            foreach (var service in _host.Services.GetServices<IHostedService>())
            {
                if (service is IServer server)
                {
                    await DrainServer(server);
                    await service.StopAsync(CancellationToken.None);
                }
            }

            _host.WaitForShutdownAsync().GetAwaiter();
        }

        private async Task DrainServer(IServer server)
        {
            var container = server.GetSessionContainer();
            if (container != null)
            {
                var serverSessions = container.GetSessions();
                foreach (var session in serverSessions)
                {
                    try
                    {
                        await session.CloseAsync(SuperSocket.Connection.CloseReason.ServerShutdown);
                    }
                    catch (Exception ex)
                    {
                        // _logger.LogError(ex.Message, ex);
                        Instance.PrintInfoLog(ex.Message);
                    }
                }
            }
        }

        /*
         * 控制台-可以在控制台查看日志输出
        调试-vs工具 -》开始调试-》输出窗口进行查看日志输出
        EventSource-可使用PerfView 实用工具收集和查看日志
        EventLog-》仅在windows系统下可以使用事件查看器查看日志
        TraceSource
        AzureAppServicesFile
        AzureAppServicesBlob
        ApplicationInsights
         * **/

        #region show UI

        delegate void printMsg(string s);//创建一个代理

        public void PrintMsg(string t)//这个就是我们的函数，我们把要对控件进行的操作放在这里
        {
            if (!this.IsHandleCreated)
            {
                return;
            }
            if (!richTextBox1.InvokeRequired)//判断是否需要进行唤醒的请求，如果控件与主线程在一个线程内，可以写成 if(!InvokeRequired)
            {
                // MessageBox.Show("同一线程内");
                PrintInfoLog(t);
            }
            else
            {
                // MessageBox.Show("不是同一个线程");
                printMsg a1 = new printMsg(PrintMsg);
                if (this.IsHandleCreated)
                {
                    Invoke(a1, new object[] { t });//执行唤醒操作
                }

            }
        }

        #endregion

        private void DbInit()
        {
            var sut = new ServiceCollection()
            .AddMemoryCache()
            .AddDistributedMemoryCache() // 为了测试，我们就不使用redis之类的东西了，用个内存实现模拟就好
            .AddSingleton<ICacheAdapter, MemoryCacheAdapter>()  // 添加缓存适配器
            .AddSingleton<ICacheAdapter>(i => new DistributedCacheAdapter(i.GetRequiredService<IDistributedCache>(), "distribute"))
            .BuildServiceProvider();
            IDistributedCache cache = sut.GetService<IDistributedCache>() as IDistributedCache;

            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("configuration.json");
            var configuration = builder.Build();
            var collections = configuration.AsEnumerable();
            foreach (var item in collections)
            {
                Console.WriteLine("{0}={1}", item.Key, item.Value);
            }

            //ILoggerFactory loggerFactory = LoggerFactory.Create(logbuilder => logbuilder
            // .AddFilter("Microsoft", LogLevel.Debug)
            // .AddFilter("System", LogLevel.Debug)
            // .AddFilter("Namespace.Class", LogLevel.Debug)
            // //.AddFile()//成功写入到文件
            // .AddLog4Net()
            //);
            //var logger = loggerFactory.CreateLogger("kxServerlogs");

            //PSqlHelper.Initialization(cache, configuration.GetSection("es_BLL_ITEM_CACHE"), configuration["ConnectionStrings:Npgsql"], null, logger);
        }

        private void SqlsugarSetup_CheckEvent(string sql)
        {
            if (IsDebug)
            {
                Instance.PrintInfoLog(sql);
            }
        }


        public void PrintInfoLog(string msg)
        {
            // Console.WriteLine(msg);
            if (!System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis"))
            {
                try
                {
                    if (IsDisposed || !frmMain.Instance.IsHandleCreated) return;

                    // 确保最多只有1000行
                    EnsureMaxLines(frmMain.Instance.richTextBox1, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {msg}\r\n";
                    if (frmMain.Instance.InvokeRequired)
                    {

                    }
                    frmMain.Instance.Invoke(new EventHandler(delegate
                    {
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;
                        frmMain.Instance.richTextBox1.AppendText(formattedMsg);
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;
                        frmMain.Instance.richTextBox1.ScrollToCaret(); // 滚动到最新的消息

                    }
                    ));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintInfoLog时出错" + ex.Message);
                }

            }


        }

        private void EnsureMaxLines(RichTextBox rtb, int maxLines)
        {
            // 计算当前的行数
            int currentLines = rtb.GetLineFromCharIndex(rtb.Text.Length) + 1;

            // 如果行数超过了最大限制，则删除旧的行
            if (currentLines > maxLines)
            {
                int linesToRemove = currentLines - maxLines;
                int start = rtb.GetFirstCharIndexFromLine(0);
                int end = rtb.GetFirstCharIndexFromLine(linesToRemove);

                // 确保richTextBox1控件在主线程中被操作
                frmMain.Instance.Invoke(new MethodInvoker(() =>
                {
                    rtb.Text = rtb.Text.Remove(start, end - start);
                }));
            }
        }

        public frmWFManage frmWF = Startup.GetFromFac<frmWFManage>();

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "工作流管理")
            {
                if (frmWF == null)
                {
                    frmWF = Startup.GetFromFac<frmWFManage>();
                }
                frmWF.RefreshData();//ReminderBizDataList
                frmWF.MdiParent = this;
                frmWF.Show();
                frmWF.Activate();

            }
            if (e.ClickedItem.Text == "启动服务")
            {
                //Load时就启动了。不重复启动了。
                // InitAll();
            }

            if (e.ClickedItem.Text == "在线用户管理")
            {
                //frmUserOnline frmuser = Startup.GetFromFac<frmUserOnline>();
                //frmuser.MdiParent = this;
                //frmuser.Show();
                if (frmusermange == null)
                {
                    frmusermange = Startup.GetFromFac<frmUserManage>();
                }
                //frmUserManage 
                frmusermange.MdiParent = this;
                frmusermange.Show();
                frmusermange.Activate();
            }

            if (e.ClickedItem.Text == "缓存管理")
            {
                frmCacheManage frmCahe = Startup.GetFromFac<frmCacheManage>();
                frmCahe.MdiParent = this;
                frmCahe.Show();
                frmCahe.Activate();
            }

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }
        private void 层叠排列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void 水平平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void 垂直平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void tsmY_Click(object sender, EventArgs e)
        {
            tsmY.Checked = true;
            tsmNo.Checked = !tsmY.Checked;
            IsDebug = tsmY.Checked;
            PrintMsg($"当前调式模式:{IsDebug}");
        }

        private void tsmNo_Click(object sender, EventArgs e)
        {
            tsmY.Checked = false;
            tsmNo.Checked = !tsmY.Checked;
            IsDebug = !tsmNo.Checked;
            PrintMsg($"当前调式模式:{IsDebug}");
        }

        private void tsbtnDataViewer_Click(object sender, EventArgs e)
        {
            frmMemoryDataViewer frm = Startup.GetFromFac<frmMemoryDataViewer>();
            frm.MdiParent = this;
            frm.Show();
            frm.Activate();
        }

        private void 参数配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGlobalConfig frm = Startup.GetFromFac<frmGlobalConfig>();
            frm.MdiParent = this;
            frm.Show();
            frm.Activate();
        }

        private void tsBtnStartServer_Click(object sender, EventArgs e)
        {
            InitAll();
            timer.Start();
            tsBtnStartServer.Enabled = false;
        }

        private void 系统注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //注册成功后。才可能启动服务器
        }
    }
}
