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
using RUINORERP.Business;
using RUINORERP.Business.CommService;
using RUINORERP.Common.Helper;
using RUINORERP.Common.Log4Net;
using RUINORERP.Extensions;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TransInstruction;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

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

        public IServiceCollection _services { get; set; }
        public IServiceProvider _ServiceProvider { get; set; }

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
            _services = Startup.Services;

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

        frmUserManage frmusermange = Startup.GetFromFac<frmUserManage>();

        private async void frmMain_Load(object sender, EventArgs e)
        {

            _logger.Error("ErrorError2233");
            _logger.LogError("LogErrorLogError2233");


            // var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
            //logger.LogError($"{DateTime.Now} LogError 日志");

            _logger.LogError("启动了服务器123");
            this.IsMdiContainer = true; // 设置父窗体为MDI容器
            menuStrip1.MdiWindowListItem = 窗口ToolStripMenuItem;
            InitAll();

            //手动初始化
            BizCacheHelper.Instance = Startup.GetFromFac<BizCacheHelper>();
            BizCacheHelper.InitManager();

            IMemoryCache cache = Startup.GetFromFac<IMemoryCache>();
            cache.Set("test1", "test123");
            await InitConfig(false);

            //timer = new System.Timers.Timer(500);
            //timer.Elapsed += new System.Timers.ElapsedEventHandler((s, x) =>
            //{
            //    try
            //    {
            //        bindsourceUserList.DataSource = frmMain.Instance.sessionListBiz.Values;
            //    }
            //    catch (Exception)
            //    {

            //    }

            //});
            //timer.Enabled = true;
            //timer.Start();
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
                            PrintMsg($"{DateTime.Now} [SessionforBiz-主要程序] Session {session.RemoteEndPoint} closed: {reason}");
                            sessionListBiz.Remove(sg.SessionID, out sg);

                            if (sg != null)
                            {
                                frmusermange.userInfos.Remove(sg.User);
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
                        logging.AddProvider(new Log4NetProviderByCustomeDb("Log4net_db.config", conn, Program.AppContextData));
                        logging.AddLog4Net();
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

                }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
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


        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "工作流管理")
            {
                frmWorkFlowManage frm = Startup.GetFromFac<frmWorkFlowManage>();
                frm.MdiParent = this;
                frm.Show();
                frm.Activate();

            }
            if (e.ClickedItem.Text == "启动服务")
            {
                InitAll();
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
    }
}
