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
using SuperSocket.Command;
using RUINORERP.Business;
using RUINORERP.Model;
using WorkflowCore.Services.DefinitionStorage;
using RUINORERP.Business.AutoMapper;
using SuperSocket.Server.Host;
using RUINORERP.Server.Comm;
using RUINORERP.Server.SmartReminder;
using WorkflowCore.Services;
using Autofac;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.SnowflakeIdHelper;
using RUINORERP.Business.Cache;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Business.CommService;

namespace RUINORERP.Server
{
    static class Program
    {
        /// <summary>
        /// 获取对象的私有字段值（用于调试）
        /// </summary>
        /// <typeparam name="TClass">对象类型</typeparam>
        /// <typeparam name="TField">字段类型</typeparam>
        /// <param name="obj">对象实例</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>字段值</returns>
        private static TField GetPrivateField<TClass, TField>(TClass obj, string fieldName)
        {
            if (obj == null) return default;
            
            var field = typeof(TClass).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field != null ? (TField)field.GetValue(obj) : default;
        }

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
        /// 服务集合
        /// </summary>
        static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
        public static string AppVersion { get; internal set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // 初始化雪花ID生成器
            new IdHelperBootstrapper().SetWorkderId(1).Boot();

#if DEBUG
            // 在DEBUG模式下，检查是否有特殊命令行参数来允许多实例运行
            bool allowMultipleInstances = Environment.GetCommandLineArgs().Contains("--allow-multiple-instances");
            if (!allowMultipleInstances && SingleInstanceChecker.IsAlreadyRunning())
#else
            if (SingleInstanceChecker.IsAlreadyRunning())
#endif
            {
                // 已有实例运行则退出
                Process instance = RunningInstance();
                if (instance != null)
                {
                    HandleRunningInstance(instance);
                }
                return;
            }
            try
            {
                // 启动服务UI
                await StartServerUI();
            }
            catch (Exception ex)
            {
                // 记录异常信息
                Console.Error.WriteLine($"启动服务时发生未处理异常: {ex}");
                // 可以添加日志记录
            }
            finally
            {
                SingleInstanceChecker.Release();
            }
        }

        static async Task StartServerUI()
        {

            System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.ThreadException += Application_ThreadException;
            System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;


#pragma warning disable CS0168 // 声明了变量但从未使用
            try
            {

                #region 初始化csla  
                try
                {



                    Startup starter = new Startup();
                    IHost myhost = starter.CslaDIPort();
                    // IHostBuilder  myhost = starter.CslaDIPort();

                    IServiceProvider services = myhost.Services;

                    //https://github.com/autofac/Autofac.Extensions.DependencyInjection/releases
                    //设置服务的提供者
                    Startup.ServiceProvider = services;
                    Program.ServiceProvider = services; // 同时给Program.ServiceProvider赋值

                    // 初始化配置同步服务
                    Startup.InitializeConfigSync(services);
                    AppContextData.SetServiceProvider(services);
                    Startup.AutofacContainerScope = services.GetAutofacRoot();
                    AppContextData.SetAutofacContainerScope(Startup.AutofacContainerScope);
                    BusinessHelper.Instance.SetContext(AppContextData);

                    // 首先初始化表结构信息
                    try
                    {
                        // 强制使用Autofac容器获取服务，确保一致性
                        if (Startup.AutofacContainerScope == null)
                        {
                            throw new InvalidOperationException("Autofac容器尚未初始化，无法获取服务");
                        }
                        
                        // 使用同一个Autofac容器获取所有服务
                        var tableSchemaManager = Startup.AutofacContainerScope.Resolve<ITableSchemaManager>();
                        var cacheInitService = Startup.AutofacContainerScope.Resolve<EntityCacheInitializationService>();
                        
                        System.Diagnostics.Debug.WriteLine($"使用Autofac容器获取服务");
                        System.Diagnostics.Debug.WriteLine($"TableSchemaManager实例ID: {tableSchemaManager.GetHashCode()}");
                        
                        // 验证EntityCacheInitializationService使用的是同一个TableSchemaManager实例
                        var cacheInitServiceTableSchemaManager = GetPrivateField<EntityCacheInitializationService, ITableSchemaManager>(cacheInitService, "_tableSchemaManager");
                        System.Diagnostics.Debug.WriteLine($"cacheInitService中的TableSchemaManager实例ID: {cacheInitServiceTableSchemaManager?.GetHashCode()}");
                        System.Diagnostics.Debug.WriteLine($"两个实例是否相同: {ReferenceEquals(tableSchemaManager, cacheInitServiceTableSchemaManager)}");
                        
                        // 如果不是同一个实例，说明EntityCacheInitializationService使用的是不同的实例
                        if (!ReferenceEquals(tableSchemaManager, cacheInitServiceTableSchemaManager))
                        {
                            System.Diagnostics.Debug.WriteLine("警告：EntityCacheInitializationService使用了不同的TableSchemaManager实例");
                            // 创建一个新的EntityCacheInitializationService实例，使用正确的TableSchemaManager
                            var unitOfWorkManage = Startup.AutofacContainerScope.Resolve<IUnitOfWorkManage>();
                            var cacheManager = Startup.AutofacContainerScope.Resolve<IEntityCacheManager>();
                            var loggerCache = Startup.AutofacContainerScope.Resolve<Microsoft.Extensions.Logging.ILogger<EntityCacheInitializationService>>();
                            var cacheSyncMetadata = Startup.AutofacContainerScope.Resolve<ICacheSyncMetadata>();
                            var queryHelper = Startup.AutofacContainerScope.Resolve<DynamicQueryHelper>();
                            
                            cacheInitService = new EntityCacheInitializationService(
                                unitOfWorkManage,
                                cacheManager,
                                cacheSyncMetadata,
                                queryHelper,
                                tableSchemaManager,
                                loggerCache);
                                
                            // 验证新实例使用的是正确的TableSchemaManager
                            var newCacheInitServiceTableSchemaManager = GetPrivateField<EntityCacheInitializationService, ITableSchemaManager>(cacheInitService, "_tableSchemaManager");
                            System.Diagnostics.Debug.WriteLine($"新EntityCacheInitializationService中的TableSchemaManager实例ID: {newCacheInitServiceTableSchemaManager?.GetHashCode()}");
                            System.Diagnostics.Debug.WriteLine($"新实例与原始TableSchemaManager是否相同: {ReferenceEquals(tableSchemaManager, newCacheInitServiceTableSchemaManager)}");
                        }
                        
                        // 打印初始化前的状态
                        System.Diagnostics.Debug.WriteLine($"初始化前 TableCount: {tableSchemaManager.GetAllTableNames().Count}");
                        System.Diagnostics.Debug.WriteLine($"初始化前 IsInitialized: {tableSchemaManager.IsInitialized}");
                        
                        // 再次验证cacheInitService使用的TableSchemaManager实例
                        var beforeInitCacheInitServiceTableSchemaManager = GetPrivateField<EntityCacheInitializationService, ITableSchemaManager>(cacheInitService, "_tableSchemaManager");
                        System.Diagnostics.Debug.WriteLine($"初始化前cacheInitService中的TableSchemaManager实例ID: {beforeInitCacheInitServiceTableSchemaManager?.GetHashCode()}");
                        System.Diagnostics.Debug.WriteLine($"初始化前两个实例是否相同: {ReferenceEquals(tableSchemaManager, beforeInitCacheInitServiceTableSchemaManager)}");
                                                        
                        // 同步初始化表结构，确保在后续代码执行前完成
                        cacheInitService.InitializeAllTableSchemas();
                        
                        // 再次验证cacheInitService使用的TableSchemaManager实例
                        var afterInitCacheInitServiceTableSchemaManager = GetPrivateField<EntityCacheInitializationService, ITableSchemaManager>(cacheInitService, "_tableSchemaManager");
                        System.Diagnostics.Debug.WriteLine($"初始化后cacheInitService中的TableSchemaManager实例ID: {afterInitCacheInitServiceTableSchemaManager?.GetHashCode()}");
                        System.Diagnostics.Debug.WriteLine($"初始化后两个实例是否相同: {ReferenceEquals(tableSchemaManager, afterInitCacheInitServiceTableSchemaManager)}");
                                                        
                        // 打印初始化后的状态
                        System.Diagnostics.Debug.WriteLine($"初始化后 TableCount: {tableSchemaManager.GetAllTableNames().Count}");
                        System.Diagnostics.Debug.WriteLine($"初始化后 IsInitialized: {tableSchemaManager.IsInitialized}");
                            
                        if (!tableSchemaManager.IsInitialized)
                        {
                            System.Diagnostics.Debug.WriteLine("警告：表结构初始化可能未完成，当前表数量为0");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine($"表结构初始化成功，共注册了 {tableSchemaManager.GetAllTableNames().Count} 个表");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"初始化表结构时发生错误: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"异常详情: {ex}");
                        throw; // 重新抛出异常，以便在调试时能看到完整的堆栈跟踪
                    }


                    //Program.AppContextData.SetServiceProvider(services);
                    //Program.AppContextData.Status = "init";

                    #region 工作流相关配置

                    var host = services.GetService<IWorkflowHost>();
                    host.OnStepError += Host_OnStepError;

                    //加载json注册，这里使用一个通用的定义加载器
                    // https://workflow-core.readthedocs.io/en/latest/json-yaml/
                    //var json = System.IO.File.ReadAllText("myflow.json");
                    //var loader = ServiceProvider.GetService<IDefinitionLoader>();
                    //loader.LoadDefinition(json, Deserializers.Json);
                    host.AddRegisterWorkflow();

                    //加载自定义工作流
                    var loader = Startup.ServiceProvider.GetService<IDefinitionLoader>();
                    string jsonpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workflow\\Json\\myflow.json");
                    var json = System.IO.File.ReadAllText(jsonpath);
                    loader.LoadDefinition(json, Deserializers.Json);

                    // 启动host服务，避免重复启动
                    if (!serviceStarted)
                    {
                        host.Start();
                        serviceStarted = true;
                    }
                    WorkflowHost = host;

                    // 提醒服务
                    //var reminderService = services.GetRequiredService<SmartReminderService_old>();
                    //Task.Run(() => reminderService.RunSystemAsync());


                    // 启动workflow流程
                    // host.StartWorkflow("HelloWorkflow", 1, data: null); //
                    //host.StartWorkflow("HelloWorkflow");//, 2, data: null, 默认会创建版本高的

                    #endregion


                    Startup.AutofacContainerScope = services.GetAutofacRoot();

                    //ILogger<frmMain> logger = services.GetService<ILogger<frmMain>>();
                    //frmMain frmMain1 = new frmMain(logger);
                    //System.Windows.Forms.Application.Run(frmMain1);

                    // 使用新的Tab形式主窗体
                    var logger = services.GetService<ILogger<frmMainNew>>();
                    var workflowHost = services.GetService<IWorkflowHost>();
                    var config = services.GetService<IOptionsMonitor<SystemGlobalConfig>>();

                    IEntityMappingService entityMappingService = Startup.GetFromFac<IEntityMappingService>();
                    // 在应用程序启动时设置当前实体映射服务
                    // 这通常在依赖注入容器配置完成后调用
                    EntityMappingHelper.SetCurrent(entityMappingService);

                    /// 初始化实体映射服务
                    EntityMappingHelper.Initialize();

                    var form1 = Startup.GetFromFac<frmMainNew>();
                    System.Windows.Forms.Application.Run(form1);
                    // var newMainForm = new frmMainNew(logger, workflowHost, config);
                    // System.Windows.Forms.Application.Run(newMainForm);

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
                // System.Windows.Forms.Application.Run(mainform);



                #endregion


            }
            catch (Exception ex)
            {
                // 记录异常信息
                Console.Error.WriteLine($"启动服务UI时发生未处理异常: {ex}");
                // 可以添加日志记录
            }
#pragma warning restore CS0168 // 声明了变量但从未使用


            //System.Windows.Forms.Application.Run(new frmMain());
        }




        #region 防止程序重复运行，检测到重复运行时切换到前一个实例
        #region 判断系统中是否已经存在实例
        // 确保应用程序只有一个实例
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
                    // 无权限访问该进程信息时跳过
                    continue;
                }
            }
            return null; // 没有找到实例
        }
        #endregion


        #region 调用Win32API,如果发现已经有一个实例运行，就将它显示到前台
        private static void HandleRunningInstance(Process instance)
        {
            //MessageBox.Show("已经有实例在运行！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//调用API函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle);//将窗口放置到前台  
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
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口。
        /// 系统给创建前台窗口的线程分配的权限稍高于其他线程。
        /// </summary>  
        /// <param name="hWnd">将被激活并被调入前台窗口的句柄</param>  
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
        /// 初始化一些上下文值
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


            frmMainNew.Instance.PrintInfoLog(workflow.Id + step.Id + exception.Message);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            string errorMessage = "应用程序未处理的线程异常: " + e.Exception.Message;
            frmMainNew.Instance.PrintInfoLog(errorMessage);
            frmMainNew.Instance.PrintInfoLog(e.Exception.StackTrace);

            // 使用frmMain中的logger记录异常（这与项目原有日志系统兼容）
            try
            {
                if (frmMainNew.Instance != null && frmMainNew.Instance._logger != null)
                {
                    frmMainNew.Instance._logger.LogError("应用程序未处理的线程异常", e.Exception);
                }
            }
            catch (Exception logEx)
            {
                // 如果日志记录也失败，确保有基本的错误输出
                System.Diagnostics.Debug.WriteLine("记录线程异常日志失败: " + logEx.Message);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string errorMsg = "应用程序发生未处理的异常。请联系管理员并提供以下信息:\n\n";

            try
            {
                if (e.IsTerminating)
                {
                    string terminatingMsg = "严重异常导致程序终止: " + ex.Message;
                    frmMainNew.Instance.PrintInfoLog(terminatingMsg);
                }
                else
                {
                    frmMainNew.Instance.PrintInfoLog("CurrentDomain_UnhandledException:" + errorMsg + ex.Message);
                }

                // 使用frmMain中的logger记录异常（这与项目原有日志系统兼容）
                if (frmMainNew.Instance != null && frmMainNew.Instance._logger != null)
                {
                    frmMainNew.Instance._logger.LogError("应用程序域未处理的异常", ex);
                }
            }
            catch (Exception logEx)
            {
                // 如果日志记录也失败，确保有基本的错误输出
                System.Diagnostics.Debug.WriteLine("记录应用程序域异常日志失败: " + logEx.Message);
            }
        }
    }
}
