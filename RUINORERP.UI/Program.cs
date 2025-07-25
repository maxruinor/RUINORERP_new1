﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using RUINORERP.Common.Log4Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Common;
using System.Reflection;
using log4net;
using RUINORERP.Model;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using WorkflowCore.Services.DefinitionStorage;
using Microsoft.Extensions.Hosting;
using Autofac;
using log4net.Repository;
using System.IO;
using log4net.Config;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Business;
using WorkflowCore.Services;
using RUINORERP.WF.BizOperation;
using RUINORERP.Common.Extensions;
using System.Threading;
using SuperSocket.ClientEngine;
using RUINORERP.UI.SuperSocketClient;
using RUINORERP.Business.Security;
using AutoUpdate;
using System.Xml.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Caching.Memory;
using log4net.Repository.Hierarchy;
using System.Text.RegularExpressions;
using RUINORERP.UI.Common;
using SqlSugar;


namespace RUINORERP.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的版本信息
        /// </summary>
        public static string ERPVersion { get; set; }

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

        /// <summary>
        /// 配置依赖注入对象
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepository(IServiceCollection services)
        {
            //手动注册 测试过程。这里只是实现批量一次注入
            //services.AddScoped(typeof(IUnitRepository), typeof(UnitRepository)); // 注入仓储
            //services.AddTransient<IUnitService, UnitService>();
            //services.AddScoped<Tb_Unit>();
            //Services.AddTransient<UnitController>();

            //services.AddSingleton<ISqlSugarClient>(sqlSugar); // 单例注册
            //services.AddScoped(typeof(SqlSugarRepository<>)); // 仓储注册
            //services.AddUnitOfWork<SqlSugarUnitOfWork>(); // 事务与工作单元注册




            #region 自动注入对应的服务接口
            //services.AddSingleton<IDictDataService, DictDataService>();//services.AddScoped<IUserService, UserService>();
            /*
            var baseType = typeof(IDependencyRepository);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            //            var getFiles = Directory.GetFiles(path, "*.dll").Where(Match);  //.Where(o=>o.Match())
            var getFiles = Directory.GetFiles(path, "*.dll").Where(o => o.ToLower().StartsWith(@path.ToLower() + "ruinor"));//路径筛选

            List<string> dlls = new List<string>();
            foreach (var file in getFiles)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Name.ToLower().Contains("ruinor"))
                {
                    // dlls.Add(file);
                }
            }*/
            //Process.GetCurrentProcess().ProcessName
            //dlls.Add(System.IO.Path.Combine(Application.StartupPath, System.AppDomain.CurrentDomain.FriendlyName));
            //List<string> dlls = new List<string>();
            // dlls.Add(System.IO.Path.Combine(Application.StartupPath, "RUINORERP.Entity.dll"));

            // var referencedAssemblies = dlls.ToArray().Select(Assembly.LoadFrom).ToList();  //.Select(o=> Assembly.LoadFrom(o))         

            // var ss = referencedAssemblies.SelectMany(o => o.GetTypes());


            //  var dependencyService = typeof(IDependencyService);
            //var ServiceTypes = ss.Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();

            // var dependencyRepository = typeof(IDependencyRepository);
            // var RepositoryTypes = ss.Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyRepository).ToArray();




            //var types = referencedAssemblies
            //    .SelectMany(a => a.DefinedTypes)
            //    .Select(type => type.AsType())
            //    .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();
            //var implementTypes = types.Where(x => x.IsClass).ToList();
            //var interfaceTypes = types.Where(x => x.IsInterface).ToList();


            //foreach (var implementType in ServiceTypes)
            //{
            //if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //{
            //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //    if (interfaceType != null)
            //        services.AddScoped(interfaceType, implementType);
            //}
            //else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //{
            //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //    if (interfaceType != null)
            //        services.AddSingleton(interfaceType, implementType);
            //}
            //else
            //{
            //    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //    if (interfaceType != null)
            //        services.AddTransient(interfaceType, implementType);
            //}
            //}

            //foreach (var implementType in implementTypes)
            //{
            //    if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddScoped(interfaceType, implementType);
            //    }
            //    else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddSingleton(interfaceType, implementType);
            //    }
            //    else
            //    {
            //        var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
            //        if (interfaceType != null)
            //            services.AddTransient(interfaceType, implementType);
            //    }
            //}
            #endregion
        }


        /// <summary>
        /// 注入服务
        /// </summary>
        /// <param name="services"></param>
        static void ConfigureServices(IServiceCollection services)
        {

            //services.AddScoped<ICurrentUser, CurrentUser>();

            ////注入配置文件，各种配置在这里先定义
            ////register configuration
            //IConfigurationBuilder cfgBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            ////.AddJsonFile("appsettings.json")
            ////2.重新添加json配置文件
            //.AddJsonFile("appsettings.json", false, false) //3.最后一个参数就是是否热更新的布尔值
            //    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true, reloadOnChange: false)
            //    ;
            //IConfiguration configuration = cfgBuilder.Build();
            //services.AddSingleton<IConfiguration>(configuration);


            // 基于接口的注册
            //services.AddSingleton<IUserService, UserService>();
            //services.AddSingleton<IUserRepository, UserReposotory>();
            // 注入日志

            //var conf =  ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // services.AddSqlsugarSetup(new AddSingleton(typeof(ILogFactory<>), typeof(LogFactory<>));
            //services.AddLogging();
            //services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());



            //services.AddScoped(typeof(IBaseRepository<>), typeof(SqlSugarBaseRepository<>)); // 注入仓储
            //Services.AddScoped(typeof(IUnitRepository)<>),typeof(RUINORERP.Repository.UnitRepository<>)); // 注入仓储

            //services.AddTransient<IUnitOfWork, UnitOfWork>(); // 注入工作单元

            //AutoMapper.IConfigurationProvider config = new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile<MappingProfile>();
            //});
            //services.AddSingleton(config);
            // if (services.IsNull()) throw new ArgumentNullException(nameof(services));

            //  services.AddSingleton(typeof(AutoMapperConfig));
            // 

            // services.AddScoped<IMapper, Mapper>();
            //  services.AddSingleton<IMapper>(mapper);

            //var sqlSugarScope = new SqlSugarScope(new ConnectionConfig
            //{
            //    ConnectionString = "Server=192.168.0.250;Database=erp;UID=sa;Password=sa",
            //    DbType = DbType.SqlServer,
            //    IsAutoCloseConnection = true,
            //});
            //services.AddSingleton<ISqlSugarClient>(sqlSugarScope); // SqlSugar 官网推荐用单例模式注入
            //  services.AddSqlsugarSetup();
            // services.AddSingleton(typeof(MainForm_test));
            // services.AddTransient(typeof(Form2));

            // 注入窗体
            //RegisterForm();
            // 注入IniHelper
            // services.AddScoped<IIniHelper, IniHelper>();


        }



        static void CreateConfig()
        {
            //这里可以弄一个ICONFIG
            IConfigurationBuilder configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            var builder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfigurationRoot configuration = builder.Build();
            string configValue = configuration.GetSection("AllowedHosts").Value;
        }

        private static Mutex _mutex;
        private const string AppGuid = "{11-22-33-44}"; // 替换为你自己的GUID

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {


            // 生成唯一的Mutex名称（使用GUID确保唯一性）
            string mutexName = $"Global\\{AppGuid}";

            // 尝试创建Mutex
            bool createdNew;
            _mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                // 如果已有实例运行，激活它并退出
                ActivateExistingInstance();
                return;
            }

            try
            {
                // 注册应用程序退出事件
                Application.ApplicationExit += OnApplicationExit;
                StartProgram(args);
            }
            finally
            {
                ReleaseMutex();
            }
        }



        private static void ActivateExistingInstance()
        {
            // 获取当前进程名称（不带扩展名）
            string processName = Process.GetCurrentProcess().ProcessName;

            // 查找同名的运行中进程
            Process[] processes = Process.GetProcessesByName(processName);

            foreach (Process process in processes)
            {
                // 跳过当前进程（尚未启动的进程）
                if (process.Id == Process.GetCurrentProcess().Id)
                    continue;

                // 激活已有窗口
                IntPtr handle = process.MainWindowHandle;
                if (handle != IntPtr.Zero)
                {
                    ShowWindow(handle, SW_RESTORE); // 恢复窗口
                    SetForegroundWindow(handle);    // 置顶窗口
                }

                MessageBox.Show("程序已经在运行中", "提示",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                break;
            }
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            ReleaseMutex();
        }

        private static void ReleaseMutex()
        {
            try
            {
                if (_mutex != null)
                {
                    _mutex.ReleaseMutex();
                    _mutex.Dispose();
                    _mutex = null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"释放Mutex时出错: {ex.Message}");
            }
        }



        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_RESTORE = 9;


        //if (SingleInstanceChecker.IsAlreadyRunning())
        //{
        //    // 激活已有窗口并退出
        //    BringExistingInstanceToFront();
        //    return;
        //}
        //try
        //{
        //    // 正常启动程序
        //    StartProgram(args);
        //}
        //finally
        //{
        //    SingleInstanceChecker.Release();
        //}



        private static void StartProgram(string[] args)
        {
            // 如果需要处理命令行参数，可以在这里进行
            // 例如，打印所有参数
            if (args.Length > 0)
            {
                Console.WriteLine("接收到的命令行参数如下：");
                foreach (var arg in args)
                {
                    ERPVersion = arg;
                }
            }
            else
            {
                //自己读取配置文件中的版本号
                string localXmlFile = Application.StartupPath + "\\AutoUpdaterList.xml";
                try
                {
                    //从本地读取更新配置文件信息

                    XDocument doc = XDocument.Load(localXmlFile);
                    // 查找 <Version> 元素
                    var versionElement = doc.Descendants("Version").FirstOrDefault();
                    if (versionElement != null)
                    {
                        ERPVersion = versionElement.Value;
                    }

                }
                catch (Exception ex)
                {

                }
            }

            try
            {
                PreCheckMustOverrideBaseClassAttribute.CheckAll(Assembly.GetExecutingAssembly());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            // 处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //UnhandledException 处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //公共类中的 先要执行



            #region clsa 可用
            /*
            var csservices = new ServiceCollection();
            csservices.AddCsla(options => options.AddWindowsForms());
            csservices.AddSingleton<Form2>();
            csservices.AddTransient<tb_LocationTypeList>();
            csservices.AddTransient<tb_LocationTypeInfo>();
            csservices.AddTransient<tb_LocationType>();
            csservices.AddTransient<tb_LocationTypeEdit>();
            csservices.AddTransient<tb_LocationTypeEditBindingList>();
            //.AddTransient<Pages.PersonListPage>()
            //.AddTransient<tb_UnitEntity>()

            // register other services here
            csservices.AddTransient<Itb_LocationTypeDal, tb_LocationTypeDal>();

            var provider = csservices.BuildServiceProvider();
            ApplicationContext = provider.GetRequiredService<Csla.ApplicationContext>();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2());
            */

            /*


                   var host1 = new HostBuilder()
                .ConfigureServices((hostContext, services) => services
                         // register window and page types here
                         //       .AddSingleton<MainForm>()
                         //            .AddSingleton<Form2>()
                         .AddSingleton<Form2>()
                  .AddTransient<tb_LocationTypeList>()
                   .AddTransient<tb_LocationTypeInfo>()
                  .AddTransient<tb_LocationType>()
                  .AddTransient<tb_LocationTypeEdit>()
                  .AddTransient<tb_LocationTypeEditBindingList>()
                   //.AddTransient<Pages.PersonListPage>()
                   //.AddTransient<tb_UnitEntity>()

                   // register other services here
                   .AddTransient<Itb_LocationTypeDal, tb_LocationTypeDal>()

                  .AddCsla(options => options.AddWindowsForms())
                  .AddLogging(configure => configure.AddConsole())

              ).Build();


                   IServiceProvider services;

                   using (var serviceScope = host1.Services.CreateScope())
                   {

                       services = serviceScope.ServiceProvider;

                       try
                       {
                           var form1 = services.GetRequiredService<Form2>();
                           Application.Run(form1);

                           Console.WriteLine("Success");
                       }
       #pragma warning disable CA1031 // Do not catch general exception types
                       catch (Exception ex)
                       {
                           Console.WriteLine("Error Occurred " + ex.Message);
                       }
       #pragma warning restore CA1031 // Do not catch general exception types
                   }
            */
            #endregion


            try
            {

                #region 用了csla  
                try
                {
                    //先定义上下文

                    Startup starter = new Startup(true);
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

                    /*
                    services.AddLogging(logBuilder =>
                    {
                        logBuilder.ClearProviders();
                        //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));

                        logBuilder.AddProvider(new Log4NetProviderByCustomeDb("log4net.config", Program.AppContextData));
                    });
                    */

                    #region  启动工作流主机


                    #region WF批量注册

                    IWorkflowRegistry _workflowRegistry = Startup.GetFromFac<IWorkflowRegistry>();
                    // var assembly = Assembly.GetExecutingAssembly();
                    var assembly = System.Reflection.Assembly.LoadFrom("RUINORERP.WF.dll");
                    var workflowTypes = assembly.GetTypes()
                        .Where(t => typeof(IWorkflowMarker).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                    foreach (var workflowType in workflowTypes)
                    {
                        try
                        {
                            if (typeof(IWorkflow<BizOperationData>).IsAssignableFrom(workflowType))
                            {
                                var workflow = (IWorkflow<BizOperationData>)Activator.CreateInstance(workflowType);
                                _workflowRegistry.RegisterWorkflow(workflow);
                            }
                            //参数要一样。不然要多种参数都 注册一下。
                            //if (typeof(IWorkflow<SOProcessData>).IsAssignableFrom(workflowType))
                            //{
                            //    var workflow = (IWorkflow<SOProcessData>)Activator.CreateInstance(workflowType);
                            //    _workflowRegistry.RegisterWorkflow(workflow);
                            //}

                        }
                        catch (Exception ex)
                        {
                            // 处理异常，例如记录日志
                            Console.WriteLine($"Failed to register workflow of type {workflowType.FullName}: {ex.Message}");
                        }
                    }

                    #endregion

                    var host = Startup.GetFromFac<IWorkflowHost>();
                    host.OnStepError += Host_OnStepError;

                    //// 这里json注册，后面还是一样的通过名称启动
                    // https://workflow-core.readthedocs.io/en/latest/json-yaml/
                    // var json = System.IO.File.ReadAllText("myflow.json");

                    //  services.AddWorkflowDSL();前面加了这个才可以取
                    var loader = Startup.ServiceProvider.GetService<IDefinitionLoader>();
                    //loader = Startup.GetFromFac<IDefinitionLoader>();
                    // loader.LoadDefinition(json, Deserializers.Json);
                    AppContextData.definitionLoader = loader;
                    //host.Registry.GetDefinition("HelloWorkflow");
                    host.RegisterWorkflow<WorkFlowTester.WorkWorkflow>();
                    host.RegisterWorkflow<WorkFlowTester.WorkWorkflow2, MyNameClass>();
                    //host.RegisterWorkflow<WFSO, WFProcessData>();//手动单个注册

                    // 如果host启动了，不能再次启动，但没有判断方法
                    host.Start();

                    AppContextData.workflowHost = host;


                    #endregion


                    var form1 = Startup.ServiceProvider.GetService<MainForm>();
                    Application.Run(form1);

                    //ILogger<MainForm> logger = null;
                    // MainForm form1 = new MainForm(logger,null);
                    //Application.Run(form1);

                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                    MessageBox.Show(s);
                    MessageBox.Show(ex.StackTrace);
                    Console.Write(ex.StackTrace);
                }

                /*

                IServiceProvider services;
                using (var serviceScope = myhost.Services.CreateScope())
                {
                    services = serviceScope.ServiceProvider;
                    try
                    {
                        serviceScope.ServiceProvider.get
                        var form1 = services.GetRequiredService<Form2>();
                        Application.Run(form1);
                        Console.WriteLine("Success");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error Occurred " + ex.Message);
                    }

                }
                */

                // IHostBuilder ihostbuilder= starter.CslaDIPort();
                // ihostbuilder.Start();
                //ServiceProvider = Startup.ServiceProvider;
                //IServiceProvider services = myhost.Services;

                //var mainform = services.GetService<Form2>();

                // var mainform = Startup.GetFromFac<Form2>(); //获取服务Service1
                //var mainform = Startup.GetFromFac<MainForm>(); //获取服务Service1
                // Application.Run(mainform);



                return;
                #endregion

            }
            catch (Exception ex)
            {

            }







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
            AppContextData.log.IP = HLH.Lib.Net.IpAddressHelper.GetLocIP();
            AppContextData.SysConfig = new tb_SystemConfig();
        }

        private static void Host_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            //MainForm.Instance.logger.LogError("Host_OnStepError", exception);
            MainForm.Instance.uclog.AddLog("工作流", exception.Message);
        }


        private static bool HandleUniqueConstraintException(Exception ex)
        {
            string errorMsg = ex.Message.ToLower();

            // 判断是否为唯一约束冲突（中文/英文消息兼容）
            if (errorMsg.Contains("unique key") || errorMsg.Contains("重复键"))
            {
                // 提取重复的订单编号（示例：从消息中匹配括号内的内容）
                string orderNo = Regex.Match(errorMsg, @"\((.*?)\)").Groups[1].Value;

                MessageBox.Show(
                    $"编号【{orderNo}】已存在，请检查后重试！",
                    "唯一性错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return true;
            }


            errorMsg = ex.Message;
            // 新增：处理外键约束冲突
            if (errorMsg.Contains("REFERENCE 约束") || errorMsg.Contains("reference constraint") || errorMsg.Contains("外键约束"))
            {
                // 提取约束名称和表名
                string constraintName = Regex.Match(errorMsg, @"约束""([^""]+)""").Groups[1].Value;
                string relatedTable = Regex.Match(errorMsg, @"表""([^""]+)""").Groups[1].Value;

                // 简化表名显示（去除dbo.前缀）
                if (relatedTable.StartsWith("dbo."))
                    relatedTable = relatedTable.Substring(4);

                // 尝试从表名推断实体类型名称（移除tb_前缀并处理复数）
                string entityTypeName = relatedTable;
           

                // 通过反射获取实体类的Description特性值
                string entityDescription = GetEntityDescriptionFromTableName(entityTypeName);

                MessageBox.Show(
                    $"无法删除当前记录，因为【{entityDescription}】中存在关联数据。\n请先删除或修改相关联的业务单据。",
                    "关联数据冲突",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return true;
            }

            return false;
        }



        private static readonly MemoryCache _errorCache = new MemoryCache(new MemoryCacheOptions());
        private static readonly MemoryCacheEntryOptions _cacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30) // 30分钟内无访问自动过期
        };
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            List<string> IgnoreExceptionMsglist = new List<string>
            {
                "执行 CreateHandle() 时无法调用值 Dispose()",
                "所请求的剪贴板操作失败",
                    "GDI+ 中发生一般性错误",
            };

            if (e.Exception != null)
            {
                //特殊的几个异常暂时屏蔽掉
                foreach (var item in IgnoreExceptionMsglist)
                {
                    if (e.Exception.Message.Contains(item))
                    {
                        string errorStrIgnore = string.Empty;
                        Exception errorIgnore = e.Exception as Exception;
                        if (errorIgnore != null)
                        {
                            errorStrIgnore = string.Format("异常类型：{0}\r\n异常消息：{1}\r\n{2}\r\n",
                               errorIgnore.GetType().Name, errorIgnore.Message, errorIgnore.StackTrace);
                        }
                        else
                        {
                            errorStrIgnore = string.Format("应用程序线程错误:{0}", e);
                        }

                        MainForm.Instance.logger.LogError("出现应用程序未处理的异常，客户端没显示！\r\n" + errorStrIgnore, errorIgnore);
                        return;
                    }
                }
            }

            if (HandleUniqueConstraintException(e.Exception)) return;


            string str = "";
            string strDateInfo = "\r\n\r\n出现应用程序未处理的异常,请更新到最新版本，如果无法解决，请联系管理员!" + DateTime.Now.ToString() + "\r\n";
            //Exception error = e.Exception as Exception;
            //if (error != null)
            //{
            //    str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n{2}\r\n",
            //    error.GetType().Name, error.Message, error.StackTrace);
            //}
            //else
            //{
            //    str = string.Format("应用程序线程错误:{0}", e);
            //}
            SystemOptimizerService.异常信息发送(e.Exception.Message, e.Exception);
            //MainForm.Instance.uclog.AddLog("线程", str);
            string errorHash = e.Exception.StackTrace?.GetHashCode().ToString(); // 更稳定的哈希
            if (_errorCache.TryGetValue(errorHash, out _)) return;
            _errorCache.Set(errorHash, DateTime.Now, _cacheOptions);
            MainForm.Instance.logger.LogError(e.Exception, e.Exception.Message);
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = "";
            Exception error = e.ExceptionObject as Exception;
            string strDateInfo = "出现应用程序未处理的异常，请更新到最新版本，如果无法解决，请联系管理员!" + DateTime.Now.ToString() + "\r\n";
            if (error != null)
            {
                str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }
            SystemOptimizerService.异常信息发送(error.Message, error);
            MainForm.Instance.uclog.AddLog("应用域", str);
            MainForm.Instance.logger.LogError("当前域_未处理异常2,请更新到新版本，如果无法解决，请联系管理员", error);
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 根据表名获取对应的实体类描述
        /// </summary>
        private static string GetEntityDescriptionFromTableName(string tableName)
        {
            try
            {
                // 假设实体类都在当前程序集的某个命名空间下
        

                // 尝试查找对应的实体类型
                var assembly = Assembly.GetExecutingAssembly();
                Type entityType = assembly.GetTypes()
                    .FirstOrDefault(t =>
                        t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase) ||
                        (t.GetCustomAttributes(typeof(SugarTable), true)
                            .FirstOrDefault() as SugarTable)?.TableName == tableName);

                entityType = Assembly.LoadFrom(Global.GlobalConstants.ModelDLL_NAME).GetType(Global.GlobalConstants.Model_NAME + "." + tableName);

                if (entityType != null)
                {
                    return UIHelper.GetEntityDescription(entityType);
                }

                // 如果找不到对应的实体类型，返回表名本身
                return tableName;
            }
            catch
            {
                // 发生异常时返回原始表名
                return tableName;
            }
        }


    }
}

