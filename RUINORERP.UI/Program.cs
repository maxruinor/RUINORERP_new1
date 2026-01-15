using System;
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
using RUINORERP.UI.Network.DI;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.Cache;
using RUINORERP.Business.RowLevelAuthService;


namespace RUINORERP.UI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的版本信息
        /// </summary>
        public static string ERPVersion { get; set; }

        /// <summary>
        /// 全局配置对象
        /// </summary>
        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 初始化全局配置
        /// </summary>
        private static void InitializeConfiguration()
        {
            // 创建配置目录和文件
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            // 读取配置
            var builder = new ConfigurationBuilder()
                .SetBasePath(configDirectory)
                .AddJsonFile("SystemGlobalConfig.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        private static ApplicationContext _AppContextData;
        public static ApplicationContext AppContextData
        {
            get => _AppContextData ??= InitializeApplicationContext();
            set => _AppContextData = value;
        }




        /// <summary>
        /// 初始化应用程序上下文
        /// </summary>
        private static ApplicationContext InitializeApplicationContext()
        {
            ApplicationContextManagerAsyncLocal applicationContextManagerAsyncLocal = new ApplicationContextManagerAsyncLocal();
            applicationContextManagerAsyncLocal.Flag = "test" + DateTime.Now.ToString();
            ApplicationContextAccessor applicationContextAccessor = new ApplicationContextAccessor(applicationContextManagerAsyncLocal);
            return new ApplicationContext(applicationContextAccessor);
        }

        /// <summary>
        /// 初始化日志系统
        /// </summary>
        private static void InitializeLogging()
        {
            // 先初始化配置
            InitializeConfiguration();

            try
            {
                // 配置log4net（使用基础配置，主要日志配置由Startup中的ConfigureLogging方法处理）
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                BasicConfigurator.Configure(logRepository);

                System.Diagnostics.Debug.WriteLine("应用程序启动 - 基础日志系统初始化完成");
            }
            catch (Exception ex)
            {
                // 如果日志配置失败，至少确保有控制台输出
                System.Diagnostics.Debug.WriteLine($"日志初始化失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否已有实例运行
        /// </summary>
        private static bool CheckSingleInstance()
        {
            string mutexName = $"Global\\{AppGuid}";
            bool createdNew;
            _mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                ActivateExistingInstance();
                return false;
            }
            return true;
        }


        /// <summary>
        /// 配置依赖注入对象
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRepository(IServiceCollection services)
        {



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
            //}
            //}
            #endregion
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
        private const string AppGuid = "{11-22-33-4400}"; // 替换为你自己的GUID

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 初始化基础日志系统（最先执行）
            InitializeLogging();

            // 单实例检查
            if (!CheckSingleInstance())
                return;

            try
            {
                // 注册应用程序退出事件
                Application.ApplicationExit += OnApplicationExit;
                MainAsync(args).GetAwaiter().GetResult();
            }
            finally
            {
                ReleaseMutex();
            }

        }

        /// <summary>
        /// 异步主程序入口
        /// </summary>
        static async Task MainAsync(string[] args)
        {
            await StartProgramAsync(args);
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





        private static async Task StartProgramAsync(string[] args)
        {
            // 初始化帮助系统
            InitializeHelpSystem();

            // 如果需要处理命令行参数，可以在这里进行
            // 例如，打印所有参数
            if (args.Length > 0)
            {
                System.Diagnostics.Debug.WriteLine("接收到的命令行参数如下：");
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
                System.Diagnostics.Debug.WriteLine(e);
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

            try
            {

                #region 注册DI
                try
                {
                    //先定义上下文
                    Startup starter = new Startup();
                    IHost myhost = starter.SartUpDIPort();


                    IServiceProvider services = myhost.Services;
                    //获取配置对象并初始化AppSettings
                    IConfiguration configuration = services.GetRequiredService<IConfiguration>();
                    RUINORERP.Common.Helper.AppSettings.Initialize(configuration);

                    //https://github.com/autofac/Autofac.Extensions.DependencyInjection/releases
                    //给上下文服务源
                    Startup.ServiceProvider = services;
                    AppContextData.SetServiceProvider(services);
                    Startup.AutofacContainerScope = services.GetAutofacRoot();
                    AppContextData.SetAutofacContainerScope(Startup.AutofacContainerScope);
                    BusinessHelper.Instance.SetContext(AppContextData);

                    // 设置ApplicationContextAccessor的ServiceProvider，确保ApplicationContext.Current能正常工作
                    var contextAccessor = AppContextData.GetApplicationContextAccessor();
                    if (contextAccessor != null)
                    {
                        contextAccessor.ServiceProvider = services;
                    }


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
                            System.Diagnostics.Debug.WriteLine($"Failed to register workflow of type {workflowType.FullName}: {ex.Message}");
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

                    // 在应用程序启动时调用此方法进行验证
                    ValidateLoggingConfiguration();


                    IEntityCacheManager entityCacheManager = Startup.GetFromFac<IEntityCacheManager>();
                    // 验证EntityCacheManager是否正确注册

                    // 验证TableSchemaManager是否正确注册为单例
                    try
                    {
                        var tableSchemaManager1 = Startup.GetFromFac<ITableSchemaManager>();
                        var tableSchemaManager2 = Startup.GetFromFac<ITableSchemaManager>();

                        if (!ReferenceEquals(tableSchemaManager1, tableSchemaManager2))
                        {
                            System.Diagnostics.Debug.WriteLine("[严重错误] Program.cs中的TableSchemaManager未正确注册为单例!");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("[验证通过] Program.cs中的TableSchemaManager正确注册为单例");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"验证TableSchemaManager注册时出错: {ex.Message}");
                    }


                    IEntityMappingService entityMappingService = Startup.GetFromFac<IEntityMappingService>();
                    // 在应用程序启动时设置当前实体映射服务
                    // 这通常在依赖注入容器配置完成后调用
                    EntityMappingHelper.SetCurrent(entityMappingService);

                    /// 初始化实体映射服务
                    EntityMappingHelper.Initialize();

                    var form1 = Startup.ServiceProvider.GetService<MainForm>();

                    // 加载行级权限策略（在主线程上异步加载）
                    Task.Run(async () =>
                    {
                        try
                        {
                            var rowAuthPolicyLoaderService = Startup.GetFromFac<IRowAuthPolicyLoaderService>();
                            if (rowAuthPolicyLoaderService != null)
                            {
                                await rowAuthPolicyLoaderService.LoadAllPoliciesAsync();
                                System.Diagnostics.Debug.WriteLine("[行级权限] 策略加载成功");
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("[警告] 未找到IRowAuthPolicyLoaderService服务，行级权限功能将不可用");
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[错误] 加载行级权限策略失败: {ex.Message}");
                        }
                    });

                    Application.Run(form1);

                }
                catch (Exception ex)
                {
                    var s = ex.Message + "\r\n" + ex.StackTrace;
                    if (ex.InnerException != null)
                    {
                        s += "\r\n";
                        s += ex.InnerException.Message + "\r\n" + ex.InnerException.StackTrace;
                    }
                    MessageBox.Show(s);
                    System.Diagnostics.Debug.WriteLine(s);
                }


                return;
                #endregion

            }
            catch (Exception ex)
            {

            }

        }


        private static void ValidateLoggingConfiguration()
        {
            var logRepository = LogManager.GetRepository();
            var appenders = logRepository.GetAppenders();

            System.Diagnostics.Debug.WriteLine($"当前配置的 Appender 数量: {appenders.Length}");
            foreach (var appender in appenders)
            {
                System.Diagnostics.Debug.WriteLine($"Appender: {appender.Name}, Type: {appender.GetType().Name}");
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
            if (e.Exception == null)
            {
                return;
            }

            // 特殊异常处理列表
            List<string> IgnoreExceptionMsglist = new List<string>
            {
                "执行 CreateHandle() 时无法调用值 Dispose()",
                "所请求的剪贴板操作失败",
                "GDI+ 中发生一般性错误"
            };

            // 检查是否需要忽略的异常
            bool isIgnored = false;
            foreach (var item in IgnoreExceptionMsglist)
            {
                if (e.Exception.Message.Contains(item))
                {
                    isIgnored = true;
                    break;
                }
            }

            // 记录所有异常，包括被忽略的异常
            string errorDetails = string.Format("异常类型：{0}\r\n异常消息：{1}\r\n堆栈信息：{2}\r\n",
                e.Exception.GetType().Name, e.Exception.Message, e.Exception.StackTrace);

            if (isIgnored)
            {
                // 被忽略的异常仍然记录日志，但不显示给用户
                return;
            }

            // 处理特定类型的业务异常
            if (HandleUniqueConstraintException(e.Exception))
            {
                return;
            }

            // 改进缓存键生成策略，结合异常类型、消息和堆栈跟踪
            string errorHash = $"{e.Exception.GetType().FullName}:{e.Exception.Message}:{e.Exception.StackTrace}".GetHashCode().ToString();
            if (_errorCache.TryGetValue(errorHash, out _))
            {
                return;
            }

            // 添加到缓存，设置过期时间
            _errorCache.Set(errorHash, DateTime.Now, _cacheOptions);

            // 发送错误信息并记录日志
            try
            {
                //TODO list 请实现将用户客户端报出来的异常发送到服务器。转发给管理员。
            }
            catch (Exception ex)
            {
                // 确保异常上报失败不会影响主流程
            }

            // 记录详细错误日志

            // 显示错误信息给用户
            string userMessage = string.Format("系统发生错误：{0}\r\n\r\n请更新到最新版本，如果无法解决，请联系管理员！\r\n时间：{1}",
                e.Exception.Message, DateTime.Now.ToString());

            // 使用线程安全的方式显示消息框
            if (MainForm.Instance != null && MainForm.Instance.InvokeRequired)
            {
                MainForm.Instance.Invoke(new Action(() =>
                {
                    MessageBox.Show(userMessage, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            }
            else
            {
                MessageBox.Show(userMessage, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            //    //TODO list 请实现将用户客户端报出来的异常发送到服务器。转发给管理员。
            MainForm.Instance.uclog.AddLog("应用域", str);
            MainForm.Instance.logger.LogError("当前域_未处理异常2,请更新到新版本，如果无法解决，请联系管理员", error);
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// 初始化帮助系统
        /// </summary>
        private static void InitializeHelpSystem()
        {
            try
            {
                // 首先尝试在应用程序目录中查找帮助文件
                string helpFilePath = Path.Combine(Application.StartupPath, "help.chm");

                // 如果在应用程序目录中找不到，尝试在Helper目录中查找
                if (!File.Exists(helpFilePath))
                {
                    helpFilePath = Path.Combine(Application.StartupPath, "..\\RUINORERP.Helper\\help.chm");
                    if (!File.Exists(helpFilePath))
                    {
                        helpFilePath = Path.Combine(Application.StartupPath, "..\\..\\RUINORERP.Helper\\help.chm");
                    }
                }

                //// 如果找到了帮助文件，则初始化帮助系统
                //if (File.Exists(helpFilePath))
                //{
                //    RUINORERP.UI.HelpSystem.HelpManager.Initialize(helpFilePath);
                //}
                //else
                //{
                //    Debug.WriteLine("帮助文件未找到: " + helpFilePath);
                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"初始化帮助系统时出错: {ex.Message}");
            }
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

