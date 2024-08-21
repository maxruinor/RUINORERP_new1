using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;
using RUINORERP.Business;
using RUINORERP.Model;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Commands;
using RUINORERP.Server.ServerSession;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server
{
    public partial class frmMain : Form
    {
        /// <summary>
        /// 保存启动的工作流队列 2023-11-18
        /// 暂时用的是通过C端传的单号来找到对应的流程号。实际不科学
        /// </summary>
        public ConcurrentDictionary<string, string> workflowlist = new ConcurrentDictionary<string, string>();
        public ILogger<frmMain> _logger { get; set; }



        private static frmMain _main;
        public static frmMain Instance
        {
            get { return _main; }
        }
        public frmMain(ILogger<frmMain> logger)

        {
            InitializeComponent();
            _main = this;
            _logger = logger;
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
            toolStripButton4.Enabled = false;
            ServerStart = true;
            try
            {
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
        private void frmMain_Load(object sender, EventArgs e)
        {
            InitAll();
        }

        private void InitAll()
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

        }

        IHost _host = null;
        async Task StartServer()
        {
            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
            //logger.LogError($"{DateTime.Now} LogError 日志");

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
                         sessionListBiz.TryAdd(session.SessionID, session as SessionforBiz);
                         PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序] Session connected: {session.RemoteEndPoint}");
                         await Task.Delay(0);
                     }, async (session, reason) =>
                     {
                         SessionforBiz sg = session as SessionforBiz;
                         //if (sg.player != null && sg.player.Online)
                         //{
                         //   // SephirothServer.CommandServer.RoleService.角色退出(sg);
                         //}
                         PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序] Session {session.RemoteEndPoint} closed: {reason}");
                         sessionListBiz.Remove(sg.SessionID, out sg);
                         //SessionListGame.Remove(session as SessionforBiz);
                         await Task.Delay(0);
                     })
                             //.ConfigureServices((context, services) =>
                             //{
                             //    services = _services;
                             //    //services.AddSingleton<RoomService>();
                             //})
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
                         logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
                     }
                     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                     logging.AddConsole();
                     logging.AddDebug();
                     if (isWindows)
                     {
                         // Add the EventLogLoggerProvider on windows machines
                         //logging.AddEventLog();//这个写到了事件查看器中。没有必要
                         logging.AddFile();
                         //logging.AddLog4Net();
                     }
                 }).UseLog4Net()

             .Build();

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
                _host.StopAsync().GetAwaiter().GetResult();
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
                        await session.CloseAsync(SuperSocket.Channel.CloseReason.ServerShutdown);
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

        private void PrintMsg(string t)//这个就是我们的函数，我们把要对控件进行的操作放在这里
        {
            if (!richTextBox1.InvokeRequired)//判断是否需要进行唤醒的请求，如果控件与主线程在一个线程内，可以写成 if(!InvokeRequired)
            {
                // MessageBox.Show("同一线程内");
                PrintInfoLog(t);
            }
            else
            {
                // MessageBox.Show("不是同一个线程");
                printMsg a1 = new printMsg(PrintMsg);
                Invoke(a1, new object[] { t });//执行唤醒操作
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
            Instance.PrintInfoLog(sql);
        }


        public void PrintInfoLog(string msg)
        {
            if (!System.Diagnostics.Process.GetCurrentProcess().MainModule.ToString().ToLower().Contains("iis"))
            {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
                try
                {
                    if (IsDisposed || !frmMain.Instance.IsHandleCreated) return;
                    frmMain.Instance.Invoke(new EventHandler(delegate
                    {
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;
                        frmMain.Instance.richTextBox1.AppendText(msg);
                        frmMain.Instance.richTextBox1.AppendText("\r\n");
                        frmMain.Instance.richTextBox1.SelectionColor = Color.Black;

                    }
                    ));
                }
                catch (Exception ex)
                {

                }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            }


        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "工作流管理")
            {
                frmWorkFlowManage frm = Startup.GetFromFac<frmWorkFlowManage>();
                frm.MdiParent = this;
                frm.Show();

            }
            if (e.ClickedItem.Text == "启动服务")
            {
                InitAll();
            }

            if (e.ClickedItem.Text == "在线用户管理")
            {
                frmUserOnline frmuser = Startup.GetFromFac<frmUserOnline>();
                frmuser.MdiParent = this;
                frmuser.Show();
            }



        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Shutdown();
        }
    }
}
