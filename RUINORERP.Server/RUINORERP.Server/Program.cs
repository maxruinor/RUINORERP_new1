using SuperSocket;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using RUINORERP.Common.Log4Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using WorkflowCore.Interface;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.Steps;
using log4net;
using log4net.Repository;
using log4net.Config;
using SuperSocket.Server;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Commands;
using SuperSocket.Command;
using RUINORERP.Business;
using RUINORERP.Model;
using WorkflowCore.Services.DefinitionStorage;
using RUINORERP.Business.AutoMapper;
using SuperSocket.Server.Host;
using RUINORERP.Server.Comm;
using Mapster;
using RUINORERP.Server.SmartReminder;
using WorkflowCore.Services;
using Autofac;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;

namespace RUINORERP.Server
{
    static class Program
    {

        public static IWorkflowHost WorkflowHost;

        private static ApplicationContext _AppContextData;
        public static ApplicationContext AppContextData
        {
            get
            {
                if (_AppContextData == null)
                {
                    ApplicationContextManagerAsyncLocal applicationContextManagerAsyncLocal = new ApplicationContextManagerAsyncLocal();
                    applicationContextManagerAsyncLocal.Flag = "test" + System.DateTime.Now.ToString();
                    ApplicationContextAccessor applicationContextAccessor = new ApplicationContextAccessor(applicationContextManagerAsyncLocal);
                    _AppContextData = new ApplicationContext(applicationContextAccessor);
                }
                return _AppContextData;
            }
            set
            {
                _AppContextData = value;
            }
        }


        static bool serviceStarted = false;
        /// <summary>
        ///  服务容器
        /// </summary>
        static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务管理者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (SingleInstanceChecker.IsAlreadyRunning())
            {
                // 激活已有窗口并退出
                BringExistingInstanceToFront();
                return;
            }
            try
            {
                // 正常启动程序
                StartServerUI();
            }
            finally
            {
                SingleInstanceChecker.Release();
            }
        }

        static async void StartServerUI()
        {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //log4netHelper配置这个文件的代码
            //ILoggerRepository repository = LogManager.CreateRepository("erpServer");
            //XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            //Host..loggerRepository = repository;


#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {

                #region 用了csla  
                try
                {

                    //IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    //tb_PurOrder purOrder= new tb_PurOrder();
                    //purOrder.PurOrderNo = "12312";
                    //var employeeDto = mapper.Map<tb_PurEntry>(purOrder);

                    Startup starter = new Startup();
                    IHost myhost = starter.CslaDIPort();
                    // IHostBuilder  myhost = starter.CslaDIPort();

                    IServiceProvider services = myhost.Services;

                    //https://github.com/autofac/Autofac.Extensions.DependencyInjection/releases
                    //给上下文服务源
                    Startup.ServiceProvider = services;
                    AppContextData.SetServiceProvider(services);
                    Startup.AutofacContainerScope = services.GetAutofacRoot();
                    AppContextData.SetAutofacContainerScope(Startup.AutofacContainerScope);
                    BusinessHelper.Instance.SetContext(AppContextData);

                    //Program.AppContextData.SetServiceProvider(services);
                    //Program.AppContextData.Status = "init";

                    #region  启动工作流主机

                    var host = services.GetService<IWorkflowHost>();
                    host.OnStepError += Host_OnStepError;

                    //这里json注册，后面还是一样的通过名称启动
                    // https://workflow-core.readthedocs.io/en/latest/json-yaml/
                    //var json = System.IO.File.ReadAllText("myflow.json");
                    //var loader = ServiceProvider.GetService<IDefinitionLoader>();
                    //loader.LoadDefinition(json, Deserializers.Json);
                    host.AddRegisterWorkflow();

                    //加载工作流配置
                    var loader = Startup.ServiceProvider.GetService<IDefinitionLoader>();
                    string jsonpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workflow\\Json\\myflow.json");
                    var json = System.IO.File.ReadAllText(jsonpath);
                    loader.LoadDefinition(json, Deserializers.Json);


                    await SafetyStockWorkflowConfig.ScheduleDailySafetyStockCalculation(host);
                    await InventorySnapshotWorkflowConfig.ScheduleInventorySnapshot(host);

                    // 如果host启动了，不能再次启动，但没有判断方法
                    if (!serviceStarted)
                    {
                        host.Start();
                        serviceStarted = true;
                    }
                    WorkflowHost = host;

                    // 启动监控
                    //var reminderService = services.GetRequiredService<SmartReminderService_old>();
                    //Task.Run(() => reminderService.RunSystemAsync());


                    // 启动workflow工作流
                    // host.StartWorkflow("HelloWorkflow", 1, data: null); //
                    //host.StartWorkflow("HelloWorkflow");//, 2, data: null, 默认会启用版本高的

                    #endregion



                    Startup.AutofacContainerScope = services.GetAutofacRoot();

                    //ILogger<frmMain> logger = services.GetService<ILogger<frmMain>>();
                    //frmMain frmMain1 = new frmMain(logger);
                    //Application.Run(frmMain1);

                    var form1 = Startup.GetFromFac<frmMain>();
                    form1._ServiceProvider = services;
                    //starter.GetMultipleServerHost(Startup.Services).StartAsync();
                    Application.Run(form1);
                    //myhost.StartAsync();

                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                    MessageBox.Show(s);
                    MessageBox.Show(ex.StackTrace);
                    Console.Write(ex.StackTrace);
                }

                // IHostBuilder ihostbuilder= starter.CslaDIPort();
                // ihostbuilder.Start();
                //ServiceProvider = Startup.ServiceProvider;
                //IServiceProvider services = myhost.Services;

                //var mainform = services.GetService<Form2>();

                // var mainform = Startup.GetFromFac<Form2>(); //获取服务Service1
                //var mainform = Startup.GetFromFac<MainForm>(); //获取服务Service1
                // Application.Run(mainform);




                #endregion


            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过


            //Application.Run(new frmMain());
        }


        private static void CreatSocketServer(IHost host)
        {

            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
            //logger.LogError($"{DateTime.Now} LogError 日志");

            var _host = MultipleServerHostBuilder.Create()


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
                   // sessionListLander.Add(session as SessionforLander);
                   // PrintMsg($"{DateTime.Now} [SessionforLander] Session-登陆器 connected: {session.RemoteEndPoint}");
                   await Task.Delay(0);
               }, async (session, reason) =>
               {
                   //sessionListLander.Remove(session as SessionforLander);
                   // PrintMsg($"{DateTime.Now} [SessionforLander] Session-登陆器 {session.RemoteEndPoint} closed: {reason}");
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


                   /*

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
                           .ConfigureServices((context, services) =>
                           {
                               services = _services;
                               //services.AddSingleton<RoomService>();
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
                   */
                   .Build();

            //try
            //{
            //    await _host.RunAsync();
            //}
            //catch (Exception e)
            //{
            //    frmMain.Instance.PrintInfoLog("_host.RunAsync()" + e.Message);
            //    _logger.LogError("socket _host RunAsync", e.Message);
            //}

        }



        private static void BringExistingInstanceToFront()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id == current.Id) continue;
                SetForegroundWindow(process.MainWindowHandle);
                break;
            }
        }



        #region 禁止多个进程运行，当重复运行时激活以前的进程
        #region 在进程中查找是否已经有实例在运行
        // 确保程序只运行一个实例
        public static Process RunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string currentProcessPath = Assembly.GetExecutingAssembly().Location;
            currentProcessPath = Path.GetFullPath(currentProcessPath).Replace("/", "\\"); // 规范化路径

            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id == currentProcess.Id)
                    continue; // 跳过当前进程

                try
                {
                    string processPath = process.MainModule.FileName;
                    processPath = Path.GetFullPath(processPath).Replace("/", "\\");

                    // 不区分大小写比较路径
                    if (currentProcessPath.Equals(processPath, StringComparison.OrdinalIgnoreCase))
                    {
                        return process; // 找到相同路径的实例
                    }
                }
                catch (Exception)
                {
                    // 无权限访问该进程信息，忽略
                    continue;
                }
            }
            return null; // 无其他实例运行
        }
        #endregion


        #region 调用Win32API,进程中已经有一个实例在运行,激活其窗口并显示在最前端
        private static void HandleRunningInstance(Process instance)
        {
            //MessageBox.Show("已经在运行!", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//调用API函数,正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle);//将窗口放置在最前端  
        }
        #endregion

        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态  
        /// </summary>  
        /// <param name="hWnd">窗口句柄</param>  
        /// <param name="cmdShow">指定窗口如何显示。查看允许值列表</param>  
        /// <returns>如果窗口原来可见，返回值为非零；如果窗口原来被隐藏，返回值为零</returns>                      
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        private const int SW_SHOWNOMAL = 1;
        /// <summary>  
        ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口
        ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>  
        /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>  
        /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>  
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion


        /// <summary>
        /// 停止工作流
        /// </summary>
        private static void StopWorkflow()
        {
            var host = ServiceProvider.GetService<IWorkflowHost>();
            host.Stop();
            serviceStarted = false;
        }


        /// <summary>
        /// 给上下文一些初始值
        /// </summary>
        /// <param name="AppContextData"></param>
        /// <param name="services"></param>
        public static void InitAppcontextValue(ApplicationContext AppContextData)
        {

            AppContextData.Status = "init";
            if (AppContextData.log == null)
            {
                AppContextData.log = new Logs();
            }
            AppContextData.log.IP = "server";
            AppContextData.log.MachineName = System.Environment.MachineName + "-" + System.Environment.UserName;
            AppContextData.SysConfig = new tb_SystemConfig();

        }


        static List<StepError> UnhandledStepErrors = new List<StepError>();
        private static void Host_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            UnhandledStepErrors.Add(new StepError
            {
                Exception = exception,
                Step = step,
                Workflow = workflow
            });


            frmMain.Instance.PrintInfoLog(workflow.Id + step.Id + exception.Message);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            frmMain.Instance.PrintInfoLog("Application_ThreadException:" + e.Exception.Message);
            frmMain.Instance.PrintInfoLog(e.Exception.StackTrace);
            //log4netHelper.fatal("系统级Application_ThreadException", e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                              "with the following information:\n\n";
            if (e.IsTerminating)
            {
                frmMain.Instance.PrintInfoLog("这个异常导致程序终止");
            }
            else
            {
                frmMain.Instance.PrintInfoLog("CurrentDomain_UnhandledException:" + errorMsg);
            }
        }
    }
}
