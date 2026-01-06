using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using AutoMapper.Internal;
using CacheManager.Core;
using Castle.Core.Logging;
using FastReport.DevComponents.DotNetBar;
using FluentValidation;
using Krypton.Toolkit;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RUINORERP.Business;
// 已删除Logger.cs，使用临时日志功能代替
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.Business.Config;
using RUINORERP.Business.DI;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business.Processor;
using RUINORERP.Business.RowLevelAuthService;
using RUINORERP.Business.Security;
using RUINORERP.Common;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Common.DI;
using RUINORERP.Common.Helper;
using RUINORERP.Common.SnowflakeIdHelper;
using RUINORERP.Extensions;
using RUINORERP.Extensions.AOP;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.IRepository.Base;
using RUINORERP.IServices;
using RUINORERP.IServices.DI;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.CommonModel;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.Context;

using RUINORERP.PacketSpec.DI;
using RUINORERP.Repository.Base;
using RUINORERP.Repository.DI;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Services;
using RUINORERP.Services.DI;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.BusinessService.SmartMenuService;
using RUINORERP.UI.Common;
using RUINORERP.UI.FM;
using RUINORERP.UI.IM;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Authentication;
using RUINORERP.UI.Network.DI;
using RUINORERP.UI.SysConfig;
using RUINORERP.UI.UserCenter.DataParts;
using RUINORERP.UI.WorkFlowDesigner;
using RUINORERP.UI.WorkFlowTester;
using SqlSugar;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WorkflowCore.Interface;
using WorkflowCore.Services;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using RUINORERP.Business.EntityLoadService;
using RUINORERP.PacketSpec.Models.Message;
namespace RUINORERP.UI
{
    public class Startup
    {
        // 初始化一个基础日志记录器，用于在完整日志系统初始化前使用
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 服务实例缓存，用于优化短时间内多次获取同一服务的性能
        /// </summary>
        private static readonly Dictionary<Type, object> _serviceInstanceCache = new Dictionary<Type, object>();
        
        /// <summary>
        /// 服务实例缓存锁，确保线程安全
        /// </summary>
        private static readonly object _cacheLock = new object();
        
        /// <summary>
        /// 缓存命中次数统计
        /// </summary>
        private static int _cacheHits = 0;
        
        /// <summary>
        /// 缓存未命中次数统计
        /// </summary>
        private static int _cacheMisses = 0;
        
        /// <summary>
        /// 最大缓存实例数量，防止内存泄漏
        /// </summary>
        private const int MaxCacheSize = 100;


        /// <summary>
        /// 没使用用csla时使用
        /// utofac容器
        /// </summary>
        public static IContainer AutoFacContainer;

        /// <summary>
        ///  服务容器
        /// </summary>
        public static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务管理者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }
        /// <summary>
        /// 使用csla有值
        /// </summary>
        public static ILifetimeScope AutofacContainerScope { get; set; }

        public Startup()
        {
            // 初始化ID生成器
            InitializeIdGenerators();
        }

        /// <summary>
        /// 初始化ID生成器
        /// </summary>
        private void InitializeIdGenerators()
        {
            // 雪花ID器
            new IdHelperBootstrapper().SetWorkderId(1).Boot();

            // SqlSugar自带雪花ID
            SnowFlakeSingle.WorkId = 1;
            //上面是自定义雪花ID
            //--------------------
            // 下面是SqlSugar自带雪花ID是成熟算法，正确配置WORKID无一例重复反馈，标题4也可以用自定义雪花算法
            //程序启时动执行一次就行
            //从配置文件读取一定要不一样
            //服务器时间修改一定也要修改WorkId
            //即使没有这里的定义，基础数据主键 类型是long的都会自动添加
            //  IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
            //IdHelper.GetLongId(); 用这个取值
        }
        /// <summary>
        /// Autofac容器配置回调
        /// </summary>
        void ConfigureAutofacContainer(HostBuilderContext bc, ContainerBuilder builder)
        {
            Services = new ServiceCollection();

            MainRegister(bc, builder);
            // 配置基础服务
            ConfigureBaseServices(Services);

            // 配置日志
            ConfigureLogger(Services);

            // 配置Autofac容器
            ConfigureContainer(builder);

            // 将服务集合注册到Autofac
            builder.Populate(Services);


        }
        /// <summary>
        /// 配置基础服务
        /// </summary>
        public static void ConfigureBaseServices(IServiceCollection services)
        {
            // 配置应用程序设置
            ConfigureAppSettings(services);

            // 配置数据库
            ConfigureDatabase(services);

      

            // 配置AutoMapper
            ConfigureAutoMapper(services);

            // 配置工作流
            ConfigureWorkflow(services);

            // 配置其他服务
            ConfigureOtherServices(services);
        }

        public static void ConfigureLogger(IServiceCollection services)
        {
            //日志优先 前面双份日志的问题，配置顺序不能乱。与DI注入有关
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);

            Program.InitAppcontextValue(Program.AppContextData);
            // by watson 2024-6-28
            services.AddLogging(logBuilder =>
            {
                // 清除所有现有提供者，避免冲突
                logBuilder.ClearProviders();

                // 添加日志过滤规则
                logBuilder.AddFilter((category, logLevel) =>
                {
                    // 从系统配置中获取日志级别设置
                    var configSection = Program.Configuration?.GetSection("SystemGlobalConfig");
                    var globalLogLevel = configSection?.GetValue<string>("LogLevel") ?? "Information";
                    LogLevel systemLogLevel;

                    // 尝试解析日志级别
                    if (!Enum.TryParse(globalLogLevel, true, out systemLogLevel))
                    {
                        systemLogLevel = LogLevel.Information; // 默认级别
                    }

                    // 为WorkflowCore设置特殊的日志级别控制
                    if (category.StartsWith("WorkflowCore"))
                    {
                        // 可以在这里专门为WorkflowCore设置不同的级别，或者使用系统统一级别
                        return logLevel >= systemLogLevel;
                    }

                    // 其他日志使用系统统一级别
                    return logLevel >= systemLogLevel;
                });

                // 添加自定义的数据库日志提供者
                if (!string.IsNullOrEmpty(newconn))
                {
                    //引用的long4net.dll要版本一样。
                    logBuilder.AddProvider(new RUINORERP.Common.Log4Net.Log4NetProviderByCustomeDb("log4net.config", newconn, Program.AppContextData));
                }
                else
                {
                    // 如果没有有效的连接字符串，添加控制台日志作为备用
                    logBuilder.AddConsole();
                }
            });

        }

        /// <summary>
        /// 配置应用程序设置
        /// </summary>
        private static void ConfigureAppSettings(IServiceCollection services)
        {
            // 创建配置目录和文件
            CreateConfigFiles();

            // 读取配置
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles"))
                .AddJsonFile(nameof(SystemGlobalConfig) + ".json", optional: false, reloadOnChange: true)
                .AddJsonFile(nameof(GlobalValidatorConfig) + ".json", optional: false, reloadOnChange: true)
                .Build();

            services.Configure<SystemGlobalConfig>(builder.GetSection(nameof(SystemGlobalConfig)));
            services.Configure<GlobalValidatorConfig>(builder.GetSection(nameof(GlobalValidatorConfig)));

            // 使用扩展方法注册配置管理相关服务
            services.AddConfigManagementServices();

            // 注册UIConfigManager为单例，并确保它能正确初始化
            services.AddSingleton<UIConfigManager>(provider =>
            {
                var configManager = new UIConfigManager();
                configManager.Initialize();
                return configManager;
            });
        }

        /// <summary>
        /// 创建配置文件
        /// </summary>
        private static void CreateConfigFiles()
        {
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            // 创建系统全局配置
            string systemGlobalConfigPath = Path.Combine(configDirectory, nameof(SystemGlobalConfig) + ".json");
            if (!File.Exists(systemGlobalConfigPath))
            {
                var systemGlobalConfig = new SystemGlobalConfig();
                string systemGlobalConfigJson = JsonConvert.SerializeObject(new { SystemGlobalConfig = systemGlobalConfig }, Formatting.Indented);
                File.WriteAllText(systemGlobalConfigPath, systemGlobalConfigJson);
            }

            // 创建全局验证器配置
            string globalValidatorConfigPath = Path.Combine(configDirectory, nameof(GlobalValidatorConfig) + ".json");
            if (!File.Exists(globalValidatorConfigPath))
            {
                var globalValidatorConfig = new GlobalValidatorConfig();
                string globalValidatorConfigJson = JsonConvert.SerializeObject(new { GlobalValidatorConfig = globalValidatorConfig }, Formatting.Indented);
                File.WriteAllText(globalValidatorConfigPath, globalValidatorConfigJson);
            }
        }

        /// <summary>
        /// 配置数据库
        /// </summary>
        private static void ConfigureDatabase(IServiceCollection services)
        {
            // 从配置读取连接字符串
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);

            // 配置SqlSugar
            services.AddSqlsugarSetup(Program.AppContextData, newconn);

            // 注册工作单元
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }

        /// <summary>
        /// 配置AutoMapper
        /// </summary>
        private static void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddSingleton(typeof(AutoMapperConfig));
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddScoped<IMapper, Mapper>();
            services.AddSingleton<IMapper>(mapper);
            services.AddAutoMapperSetup();
        }

        /// <summary>
        /// 配置工作流
        /// </summary>
        private static void ConfigureWorkflow(IServiceCollection services)
        {
            //默认就是内存模式?
            services.AddWorkflow();
            services.AddWorkflowDSL();

            // 注册工作流服务
            services.AddTransient<loopWork>();
            services.AddTransient<MyNameClass>();
            services.AddTransient<NextWorker>();
            services.AddTransient<worker>();
            services.AddTransient<WorkWorkflow>();
            services.AddTransient<WorkWorkflow2>();
        }

        /// <summary>
        /// 配置其他服务
        /// </summary>
        private static void ConfigureOtherServices(IServiceCollection services)
        {
            services.AddSingleton<HardwareInfo>();

            // 注册通知服务相关依赖
            services.AddSingleton<SmtpClient>();
            // 注册数据库连接
            services.AddScoped<IDbConnection>(provider =>
            {
                var sqlSugarClient = provider.GetRequiredService<ISqlSugarClient>();
                return sqlSugarClient.Ado.Connection;
            });
            // 注册语音提醒服务 - 简化版：只支持System.Speech或不支持语音
            services.AddSingleton<IVoiceReminder>(provider =>
            {
                // 检测系统是否支持语音功能
                bool isSpeechSupported = IsSpeechSynthesisAvailable();
                
                if (isSpeechSupported)
                {
                    Console.WriteLine("系统支持语音合成，使用System.Speech实现");
                    return new SystemSpeechVoiceReminder();
                }
                else
                {
                    Console.WriteLine("系统不支持语音合成，使用空实现");
                    return new NullVoiceReminder();
                }
            });
            
            // 保持原有注册以兼容现有代码
            services.AddSingleton<WindowsTtsVoiceReminder>();
            // 注册SystemSpeechVoiceReminder，方便直接使用
            services.AddSingleton<SystemSpeechVoiceReminder>();
            
            // 注册增强版消息管理器
            services.AddScoped<EnhancedMessageManager>();

            services.AddMemoryCacheSetup();
            services.AddAppContext(Program.AppContextData);

            // 添加CacheManager缓存服务
            services.AddSingleton<ICacheManager<object>>(provider =>
            {
                return CacheFactory.Build<object>(settings =>
                {
                    settings
                        .WithSystemRuntimeCacheHandle()
                        .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromMinutes(30));
                });
            });


            // 注册客户端缓存管理器
            // services.AddSingleton<EventDrivenCacheManager>();
            // services.AddSingleton<CacheSyncCommandHandler>();

            // 注册审计日志
            services.Configure<AuditLogOptions>(options =>
            {
                options.BatchSize = 5;
                options.FlushInterval = 5000;
                options.EnableAudit = true;
            });

            // 注册审计日志服务
            services.AddSingleton<IAuditLogService, AuditLogService>();
            services.AddSingleton<IFMAuditLogService, FMAuditLogService>();

            services.AddSingleton(typeof(MenuTracker));

            // 注册主窗体
            services.AddSingleton(typeof(MainForm));

            // 注册插件管理器
            services.AddSingleton(typeof(RUINORERP.Plugin.PluginManager));
        }





        /// <summary>
        /// 配置Autofac容器 - 集中管理服务注册，避免重复代码
        /// </summary>
        public static void ConfigureContainer(ContainerBuilder builder)
        {


            // 基础服务注册
            RegisterBaseServices(builder);

            // 配置网络服务的Autofac容器
            NetworkServicesDependencyInjection.ConfigureNetworkServicesContainer(builder);

            // 项目模块服务注册 - UI层只需引用其他项目DI目录中的注册服务类
            RegisterProjectModuleServices(builder);

            // 特定UI组件注册
            RegisterUIComponents(builder);

            // 注册AOP相关
            ConfigureAOP(builder);

            // 注册工作单元
            builder.RegisterType<UnitOfWorkManage>()
                .As<IUnitOfWorkManage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(BaseDataCacheAOP));
        }

        /// <summary>
        /// 注册基础服务
        /// </summary>
        private static void RegisterBaseServices(ContainerBuilder builder)
        {
            // 注册基础类型
            builder.RegisterGeneric(typeof(DbHelper<>))
                .As(typeof(DbHelper<>))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            // 注册状态管理服务
            builder.AddStateManagerWithGlobalRules();

        }

        /// <summary>
        /// 注册各项目模块服务 - UI层只需引用其他项目DI目录中的注册服务类
        /// </summary>
        private static void RegisterProjectModuleServices(ContainerBuilder builder)
        {
            try
            {
                    // 配置其他项目的依赖注入 - 通用注册放在前面
                ServicesDIConfig.ConfigureContainer(builder);      // Services项目
                RepositoryDIConfig.ConfigureContainer(builder);    // Repository项目
                IServicesDIConfig.ConfigureContainer(builder);     // IServices项目
                
                // 最后注册关键业务服务，确保特殊注册覆盖通用注册
                BusinessDIConfig.ConfigureContainer(builder);      // Business项目 - 最后执行，确保TableSchemaManager等关键服务为单例

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("警告：使用模块DI配置类注册服务失败，回退到传统方式注册。错误信息：" + ex.Message);

                // 回退到传统方式注册
                ConfigureContainerTodll(builder);
            }
        }

        /// <summary>
        /// 注册特定UI组件
        /// </summary>
        private static void RegisterUIComponents(ContainerBuilder builder)
        {
            // 注册菜单项
            builder.RegisterType<RUINORERP.UI.SS.MenuInit>()
                .Named<UserControl>("MENU")
                .AsImplementedInterfaces()
                .AsSelf();

            // 注册UCTodoList用户控件 - 由于没有MenuAttrAssemblyInfo特性，需要单独注册
            RegisterUCTodoList(builder);

            // 注册所有带MenuAttrAssemblyInfo特性的窗体
            RegisterForm(builder);
        }

        /// <summary>
        /// 注册UCTodoList用户控件
        /// </summary>
        private static void RegisterUCTodoList(ContainerBuilder builder)
        {
            try
            {
                builder.RegisterType<RUINORERP.UI.UserCenter.DataParts.UCTodoList>()
                    .AsSelf()
                    .PropertiesAutowired()
                    .InstancePerDependency();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("注册UCTodoList失败: {0}", ex.Message));
            }
        }

      

        /// <summary>
        /// 配置AOP
        /// </summary>
        private static void ConfigureAOP(ContainerBuilder builder)
        {
            builder.RegisterType<AutoComplete>()
                .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);

            //builder.RegisterType<BizCodeGenerator>();
            builder.RegisterType<BaseDataCacheAOP>();

            builder.RegisterType<Person>()
                .InterceptedBy(typeof(BaseDataCacheAOP))
                .EnableClassInterceptors();

            builder.RegisterType<PersonBus>()
                .EnableClassInterceptors();

            builder.RegisterType<tb_DepartmentServices>()
                .As<Itb_DepartmentServices>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(BaseDataCacheAOP));
        }

        /// <summary>
        /// 配置DLL程序集注册
        /// </summary>
        public static void ConfigureContainerTodll(ContainerBuilder builder)
        {
            // 注册各个DLL程序集
            RegisterAssemblyTypes(builder, "RUINORERP.WF.dll");
            RegisterAssemblyTypes(builder, "RUINORERP.Extensions.dll");
            RegisterAssemblyTypes(builder, "RUINORERP.Model.dll");
            RegisterAssemblyTypes(builder, "RUINORERP.IServices.dll");
            RegisterAssemblyTypes(builder, "RUINORERP.Services.dll");
            RegisterAssemblyTypes(builder, "RUINORERP.Business.dll");

            // 注册GridViewRelated为单例
            builder.RegisterType<GridViewRelated>().SingleInstance();

            // 最后注册AutofacServiceRegister模块，确保不会覆盖特定注册
           // builder.RegisterModule(new AutofacServiceRegister());
        }

        /// <summary>
        /// 注册程序集类型
        /// </summary>
        private static void RegisterAssemblyTypes(ContainerBuilder builder, string assemblyName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyName);

                // 分离BizTypeMapper和其他类型的注册
                var bizTypeMapperType = assembly.GetType("RUINORERP.Business.CommService.BizTypeMapper");

                if (bizTypeMapperType != null)
                {
                    // 对BizTypeMapper特殊处理：只注册为自身类型，避免接口拦截问题
                    builder.RegisterType(bizTypeMapperType)
                        .AsSelf()
                        .InstancePerDependency()
                        .PropertiesAutowired()
                        .PreserveExistingDefaults();

                    // 获取GlobalStateRulesManager类型（如果在当前程序集中）
                    var globalStateRulesManagerType = assembly.GetType("RUINORERP.Model.Base.StatusManager.GlobalStateRulesManager");
                    
                    // 获取TableSchemaManager类型（如果是Business.dll）
                    var tableSchemaManagerType = assemblyName == "RUINORERP.Business.dll" ? 
                        assembly.GetType("RUINORERP.Business.Cache.TableSchemaManager") : null;

                    // 注册其他类型（排除特殊处理的类型）
                    if (globalStateRulesManagerType != null)
                    {
                        // 如果包含GlobalStateRulesManager，排除它
                        builder.RegisterAssemblyTypes(assembly)
                            .Where(t => t != bizTypeMapperType && 
                                        t != globalStateRulesManagerType && 
                                        (tableSchemaManagerType == null || t != tableSchemaManagerType))
                            .AsImplementedInterfaces()
                            .AsSelf()
                            .InstancePerDependency()
                            .PropertiesAutowired();
                    }
                    else
                    {
                        // 不包含GlobalStateRulesManager的情况
                        builder.RegisterAssemblyTypes(assembly)
                            .Where(t => t != bizTypeMapperType && 
                                        (tableSchemaManagerType == null || t != tableSchemaManagerType))
                            .AsImplementedInterfaces()
                            .AsSelf()
                            .InstancePerDependency()
                            .PropertiesAutowired();
                    }
                }
                else
                {
                    // 获取GlobalStateRulesManager类型（如果在当前程序集中）
                    var globalStateRulesManagerType = assembly.GetType("RUINORERP.Model.Base.StatusManager.GlobalStateRulesManager");
                    
                    // 获取TableSchemaManager类型（如果是Business.dll）
                    var tableSchemaManagerType = assemblyName == "RUINORERP.Business.dll" ? 
                        assembly.GetType("RUINORERP.Business.Cache.TableSchemaManager") : null;
                    
                    if (globalStateRulesManagerType != null)
                    {
                        // 常规注册（排除GlobalStateRulesManager和TableSchemaManager）
                        builder.RegisterAssemblyTypes(assembly)
                            .Where(t => t != globalStateRulesManagerType && 
                                        (tableSchemaManagerType == null || t != tableSchemaManagerType))
                            .AsImplementedInterfaces()
                            .AsSelf()
                            .InstancePerDependency()
                            .PropertiesAutowired();
                    }
                    else
                    {
                        // 常规注册（排除TableSchemaManager）
                        if (tableSchemaManagerType != null)
                        {
                            builder.RegisterAssemblyTypes(assembly)
                                .Where(t => t != tableSchemaManagerType)
                                .AsImplementedInterfaces()
                                .AsSelf()
                                .InstancePerDependency()
                                .PropertiesAutowired();
                        }
                        else
                        {
                            // 常规注册（无特殊类型需要排除的程序集）
                            builder.RegisterAssemblyTypes(assembly)
                                .AsImplementedInterfaces()
                                .AsSelf()
                                .InstancePerDependency()
                                .PropertiesAutowired();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录程序集加载失败日志
                System.Diagnostics.Debug.WriteLine($"加载程序集 {assemblyName} 失败: {ex.Message}");
            }
        }





        /// <summary>
        /// 注册过程 2023-10-08
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="builder"></param>
        void MainRegister(HostBuilderContext bc, ContainerBuilder builder)
        {
            #region  注册


            #region 模仿csla 为了上下文
            //为了上下文
            // ApplicationContext defaults
            RegisterContextManager(Services);
            // Services.AddSingleton<Context.ApplicationContext>(Program.AppContextData);
            #endregion

            ConfigureServices(Services);


            builder.RegisterGeneric(typeof(DbHelper<>))
            .As(typeof(DbHelper<>))
                .AsImplementedInterfaces().AsSelf()
            //.EnableInterfaceInterceptors() // 如果需要 AOP 拦截的话
            //.EnableClassInterceptors() // 如果需要 AOP 拦截的话
            .PropertiesAutowired() // 指定属性注入
            .SingleInstance(); // 单例模式




            /// <summary>
            /// 通用注册放在前面，确保特殊注册可以覆盖它们
            /// 注意：TableSchemaManager的特殊注册在ConfigureContainer方法中处理，这里不重复注册
            /// </summary>
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .Where(type => !typeof(IExcludeFromRegistration).IsAssignableFrom(type) && type != typeof(RUINORERP.Business.Cache.TableSchemaManager)) // 排除TableSchemaManager，避免重复注册
                .AsImplementedInterfaces()
                .AsSelf();




            //覆盖上面自动注册的？说是最后的才是

            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
            //如果注册为名称的，需要这样操作
            builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
            .AsImplementedInterfaces().AsSelf();



            ConfigureContainerForDll(builder);

            RegisterForm(builder);

            builder.RegisterType<AutoComplete>()
            .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);
            // builder.RegisterType<BizCodeGenerator>(); // 注册拦截器
            // 注册依赖
            builder.RegisterType<BaseDataCacheAOP>(); // 注册拦截器
                                                      //builder.RegisterType<LogInterceptor>(); // 注册拦截器
                                                      //builder.RegisterType<Person>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
            builder.RegisterType<Person>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
                                                                                                               //builder.RegisterType<AOPDllTest.PersonDLL>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
            builder.RegisterType<PersonBus>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
            //builder.RegisterType<tb_DepartmentController>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截

            builder.RegisterType<tb_DepartmentServices>().As<Itb_DepartmentServices>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(BaseDataCacheAOP));


            //注入工作单元
            builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .EnableInterfaceInterceptors().InterceptedBy(typeof(BaseDataCacheAOP));


            builder.Populate(Services);//将自带的也注入到autofac

            ////覆盖上面自动dll批量注入的方法，因为要用单例模式
            builder.RegisterType(typeof(RUINORERP.WF.WorkFlow.WorkflowRegisterService))
                 .AsImplementedInterfaces() //加上这一行，会出错
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()//打开AOP类的虚方法注入
                 .PropertiesAutowired()//指定属性注入
                 .SingleInstance();




            // AuditLogHelper 已在 BusinessDIConfig.cs 中注册


            //_containerBuilder = builder;
            //AutoFacContainer = builder.Build();
            #endregion
        }



        public IHost SartUpDIPort()
        {
            var hostBuilder = new HostBuilder()
           .ConfigureAppConfiguration((context, config) =>
           {
               try
               {
                   var env = context.HostingEnvironment;
                   config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                   config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
               }
               catch (Exception ex)
               {
                   _logger.Error("配置应用程序设置失败", ex);
               }

           })
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
          //正确配置Autofac容器
          .ConfigureContainer<ContainerBuilder>(ConfigureAutofacContainer)

           .ConfigureServices((context, services) =>
           {
               services.AddAutofac();
           }).Build();

            // 初始化Autofac容器作用域，确保GetFromFac<T>()方法可以正常工作
            try
            {
                // 获取Autofac的根生命周期作用域
                var rootLifetimeScope = hostBuilder.Services.GetService<ILifetimeScope>();
                if (rootLifetimeScope != null)
                {
                    AutofacContainerScope = rootLifetimeScope;
                    _logger.Info("Autofac容器作用域已成功初始化");
                }
                else
                {
                    _logger.Warn("无法获取Autofac的根生命周期作用域");

                    // 备选方案：尝试从AutofacServiceProvider获取
                    var serviceProvider = hostBuilder.Services as AutofacServiceProvider;
                    if (serviceProvider != null)
                    {
                        AutofacContainerScope = serviceProvider.LifetimeScope;
                        _logger.Info("通过AutofacServiceProvider成功初始化容器作用域");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("初始化Autofac容器作用域失败", ex);
            }
            
            return hostBuilder;
        }

 


        #region 注入窗体-开始

        /// <summary>
        /// 这里主要 是注册了  菜单特性的窗体[MenuAttrAssemblyInfo("其他费用支出统计", ModuleMenuDefine.模块定义.财务管理, ModuleMenuDefine.财务管理.费用管理, BizType.其他费用统计)]
        /// </summary>
        /// <param name="_builder"></param>
        private static void RegisterForm(ContainerBuilder _builder)
        {
            System.Reflection.Assembly Assemblyobj = System.Reflection.Assembly.GetExecutingAssembly();
            Type[]? AllTypes = Assemblyobj?.GetExportedTypes();

            //主动排除指定接口的类
            Type[] types = AllTypes.ToList().Where(type => !typeof(IExcludeFromRegistration).IsAssignableFrom(type)).ToArray();

            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                foreach (Type type in types)
                {

                    // .Where(x => x.GetConstructors().Length > 0) //没有构造函数的排除


                    if (type.Name.Contains("ButtonInfo"))
                    {

                    }
                    // 类型是否为窗体，否则跳过，进入下一个循环
                    //if (type.GetTypeInfo != form)
                    //    continue;

                    // 是否为自定义特性，否则跳过，进入下一个循环
                    if (!type.IsDefined(descType, false))
                        continue;
                    // 强制为自定义特性
                    MenuAttrAssemblyInfo? attribute = type.GetCustomAttribute(descType, false) as MenuAttrAssemblyInfo;

                    // 如果强制失败就进入下一个循环
                    if (attribute == null)
                        continue;

                    if (type.Name.Contains("UCPrePaymentQuery"))
                    {

                    }

                    // 【新增】优先处理实现业务类型接口的窗体（收/付业务）
                    if (ISharedForm(type))
                    {
                        HandleSharedFormTypeRegistration(_builder, Assemblyobj, type);
                        continue; // 处理后跳过后续基类判断
                    }

                    if (type.BaseType.IsGenericType)
                    {

                        if (type.BaseType.Name.Contains("BaseBillEditGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseBillEdit>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        if (type.BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<KryptonForm>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        if (type.BaseType.Name.Contains("BaseListGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseUControl>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        //上面已经判断是泛型，则指定到他的基类，并指定属性注入。使用时也一样？
                        if (type.BaseType.Name.Contains("BaseListWithTree"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseListWithTree>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        if (type.BaseType.Name.Contains("BaseBillQueryMC"))
                        {
                            if (type.BaseType.IsGenericType)
                            {
                                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseQuery>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                            }

                        }
                        else
                        if (type.BaseType.Name.Contains("BaseNavigatorAnalysis") || type.BaseType.Name.Contains("BaseNavigatorPages") || type.BaseType.Name.Contains("BaseNavigatorGeneric") || type.BaseType.Name.Contains("BaseBillQueryMC") || type.BaseType.Name.Contains("BaseMasterQueryWithCondition"))
                        {
                            if (type.BaseType.Name.Contains("BaseNavigatorGeneric") && type.Name == "UCFinishedGoodsInvStatistics")
                            {

                            }
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                              //.AsSelf()
                              //.AsImplementedInterfaces().AsSelf() //加上这一行，会出错
                              // .EnableInterfaceInterceptors()
                              //.InstancePerDependency()//默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                              // .EnableClassInterceptors()//打开AOP类的虚方法注入
                              //.PropertiesAutowired();//指定属性注入
                              .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入


                            /*
                                        builder.RegisterTypes(IOCCslaTypes.ToArray())
                                        .Where(x => x.GetConstructors().Length > 0)
                                    //.AsClosedTypesOf(typeof(IServices.BASE.IBaseServices<>))
                                    .AsImplementedInterfaces().AsSelf()
                                    .PropertiesAutowired()
                                    .InstancePerDependency()//默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                                    .EnableInterfaceInterceptors();
                            */


                        }
                        else
                        {
                            //BY 2025-4-19 为了解决 合并表时 窗体类又要分开的情况 像 预收付单


                        }






                        Type[] paraTypes = type.BaseType.GetGenericArguments();
                        if (paraTypes.Length > 0)
                        {
                            // info.EntityName = paraTypes[0].Name;
                        }



                    }
                    else
                    {
                        //上面是泛型，这里处理非泛型
                        if (type.BaseType.Name.Contains("KryptonForm"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<KryptonForm>(type.Name)
                             .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else if (type.BaseType.Name.Contains("UCBaseClass"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UCBaseClass>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                           .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }


                        if (type.BaseType.Name.Contains("KryptonForm"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseQuery>(type.Name)
                        .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                    }


                    if (type.Name == "frmSaleOrderQuery")
                    {

                    }
                    if (type.Name == nameof(UCTodoList))
                    {

                    }

                    //// 域注入
                    //Services.AddScoped(type);
                    //  _builder.RegisterType(attribute.FormType.GetType()).Named(attribute.FormType.Name, typeof(UserControl))
                    //       .AsImplementedInterfaces().AsSelf();
                    // _builder.Register(c => new BI.UCLocation()).Named<UserControl>("UCLocation");

                }
            }
        }

        // 接口检测辅助方法
        private static bool ISharedForm(Type type)
        {
            // 确保是具体类，且实现了ISharedIdentification接口
            return type.IsClass && !type.IsAbstract &&
                   typeof(ISharedIdentification).IsAssignableFrom(type);
        }


        // 接口类型注册处理
        //这里处理的是BaseType.BaseType，提前处理的多一级基类。共享窗体也是加了一级子类所以按这个规律来处理了
        private static void HandleSharedFormTypeRegistration(ContainerBuilder _builder, System.Reflection.Assembly Assemblyobj, Type type)
        {
            // 注册为BaseQuery子类（假设基类是BaseQuery）
            //builder.Register(c => Activator.CreateInstance(type))
            //       .Named<BaseQuery>(type.Name) // 保持原有命名规则
            //       .PropertiesAutowired(new CustPropertyAutowiredSelector())
            //       .AsSelf() // 同时支持按自身类型解析
            //       .As(typeof(ISharedIdentification)); // 支持接口注入（可选）

            if (type.BaseType.BaseType.Name.Contains("BaseBillEditGeneric"))
            {
                //builder.RegisterType(type)
                //          .As<BaseBillEdit>()
                //          //.WithMetadata("BillType", "Payment") // 添加额外元数据
                //          .PropertiesAutowired();
                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseBillEdit>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
            }
            else if (type.BaseType.BaseType.Name.Contains("BaseBillQueryMC"))
            {
                //builder.RegisterType(type)
                //          .As<BaseQuery>()
                //          //.WithMetadata("BillType", "Payment") // 添加额外元数据
                //          .PropertiesAutowired();

                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseQuery>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
            }
            else if (type.BaseType.BaseType.Name.Contains("BaseNavigatorGeneric"))
            {
                if (type.BaseType.Name.Contains("BaseNavigatorGeneric") && type.Name == "UCFinishedGoodsInvStatistics")
                {

                }

                //builder.RegisterType(type)
                //          .As<BaseQuery>()
                //          //.WithMetadata("BillType", "Payment") // 添加额外元数据
                //          .PropertiesAutowired();

                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入

                //_builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseNavigator>(type.Name)
                //          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
            }
            else if (type.BaseType.BaseType.Name.Contains("BaseListGeneric"))
            {
                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseUControl>(type.Name)
              .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
            }


            // 【扩展】如果需要区分收/付业务，可添加额外元数据
            //var businessType = (BillBusinessType)type.GetProperty("BusinessType").GetValue(null);
            //builder.RegisterMetadata(registration =>
            //{
            //    registration.Add("BusinessType", businessType);
            //});
        }

        #endregion 注入窗体-结束





        /// <summary>
        /// 自带框架注入 2023-10-08
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            // 不再需要ClientTokenStorage，使用TokenManager代替
            services.AddSingleton<SmtpClient>(new SmtpClient("your.smtp.server"));
            #region 创建配置文件-开始
            // 配置文件所在的目录
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            // 检查并生成 SystemGlobalConfig 配置文件
            string systemGlobalConfigPath = Path.Combine(configDirectory, nameof(SystemGlobalConfig) + ".json");
            if (!File.Exists(systemGlobalConfigPath))
            {
                var systemGlobalConfig = new SystemGlobalConfig();
                string systemGlobalConfigJson = JsonConvert.SerializeObject(new { SystemGlobalConfig = systemGlobalConfig }, Formatting.Indented);
                File.WriteAllText(systemGlobalConfigPath, systemGlobalConfigJson);
            }

            // 检查并生成 GlobalValidatorConfig 配置文件
            string globalValidatorConfigPath = Path.Combine(configDirectory, nameof(GlobalValidatorConfig) + ".json");
            if (!File.Exists(globalValidatorConfigPath))
            {
                var globalValidatorConfig = new GlobalValidatorConfig();
                string globalValidatorConfigJson = JsonConvert.SerializeObject(new { GlobalValidatorConfig = globalValidatorConfig }, Formatting.Indented);
                File.WriteAllText(globalValidatorConfigPath, globalValidatorConfigJson);
            }
            #endregion

            // 读取自定义的 JSON 配置文件
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles"))
                .AddJsonFile(nameof(SystemGlobalConfig) + ".json", optional: false, reloadOnChange: true)
                .AddJsonFile(nameof(GlobalValidatorConfig) + ".json", optional: false, reloadOnChange: true)
                .Build();

            services.Configure<SystemGlobalConfig>(builder.GetSection(nameof(SystemGlobalConfig)));
            services.Configure<GlobalValidatorConfig>(builder.GetSection(nameof(GlobalValidatorConfig)));

            services.AddSingleton(typeof(UIConfigManager));
            //services.AddSingleton(typeof(MainForm_test));//MDI最大。才开一次才能单例
            services.AddSingleton(typeof(MainForm));//MDI最大。才开一次才能单例
                                                    // 注册工作流定义
            services.AddSingleton<IWorkflowRegistry, WorkflowRegistry>();
            services.AddSingleton<IWorkflowHost, WorkflowHost>();

            services.AddScoped(typeof(UI.BaseForm.frmBase));
            services.AddScoped(typeof(UI.BaseForm.BaseUControl));
            services.AddScoped(typeof(UI.BaseForm.BaseQuery));

            services.AddScoped(typeof(UserControl));
            services.AddScoped(typeof(BaseListWithTree));

            services.AddScoped(typeof(ReminderData));
            services.AddSingleton<ICacheService, SqlSugarMemoryCacheService>();



            // 添加其他服务...
            services.AddScoped<EntityLoader>();


            // services.AddSingleton(new AppSettings(WebHostEnvironment));
            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton(Configuration);

            #region 

            //  new IdHelperBootstrapper().SetWorkderId(1).Boot();

            //services.AddWorkflow(x => x.UseMySQL(@"Server=127.0.0.1;Database=workflow;User=root;Password=password;", true, true));

            //这是新增加的服务 后面才能实例 定义器
            services.AddWorkflowDSL();

            // 这些个构造函数带参数的，需要添加到transient中
            // 可能没构造函数的 自动添加
            services.AddTransient<loopWork>();
            services.AddTransient<MyNameClass>();
            services.AddTransient<NextWorker>();
            services.AddTransient<worker>();
            services.AddTransient<WorkWorkflow>();
            services.AddTransient<WorkWorkflow2>();

            #endregion


            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // 注入仓储


            // services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            // services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddMemoryCacheSetup();

            //这个缓存可能更好。暂时没有去实现，用了直接简单的方式
            //services.AddRedisCacheSetup();

            services.AddAppContext(Program.AppContextData);

            //已经有配置了
            //string conn = AppSettings.GetValue("ConnectString");
            //string key = "ruinor1234567890";
            //string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);
            //services.AddSqlsugarSetup(Program.AppContextData, newconn);


            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;



            // 注册PacketSpec服务
            services.AddPacketSpecServices(configuration);
            // 添加审计日志配置 （如果存在）
            services.Configure<AuditLogOptions>(configuration.GetSection("AuditLog"));

            // 使用 ConfigureOptions 手动覆盖特定值
            services.Configure<AuditLogOptions>(options =>
            {
                options.BatchSize = 5;
                options.FlushInterval = 5000;
                options.EnableAudit = true;
            });


            // 注册审计日志服务
            services.AddSingleton<IAuditLogService, AuditLogService>();
            // 注册审计日志服务
            services.AddSingleton<IFMAuditLogService, FMAuditLogService>();

            services.AddSingleton(typeof(MenuTracker)); // 菜单跟踪

            //services.AddSingleton<ApplicationContext>(Program.AppContextData);
            //services.AddTransient<BaseController, AuthorizeController>();

            //  services.AddSingleton(typeof(AutoMapperConfig));
            // 
            //services.AddScoped<IMapper, Mapper>();
            //services.AddSingleton<IMapper>(mapper);


            //services.AddSingleton(IDefinitionLoader);

            services.AddSingleton(typeof(AutoMapperConfig));
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddScoped<IMapper, Mapper>();
            services.AddSingleton<IMapper>(mapper);
            services.AddAutoMapperSetup();

 

            //services.AddCorsSetup();
            //services.AddMiniProfilerSetup();
            //services.AddSwaggerSetup();
            //services.AddQuartzNetJobSetup();
            //services.AddAuthorizationSetup();
            //services.AddSignalR().AddNewtonsoftJsonProtocol();
            //services.AddBrowserDetection();
            //services.AddRedisInitMqSetup();
            //services.AddIpStrategyRateLimitSetup(Configuration);
            //services.AddRabbitMQSetup();
            //services.AddEventBusSetup();
            //services.AddControllers(options =>
            //{
            //    // 异常过滤器
            //    options.Filters.Add(typeof(GlobalExceptionFilter));
            //    // 审计过滤器
            //    options.Filters.Add<AuditingFilter>();
            //})
            //    .AddControllersAsServices()
            //    .AddNewtonsoftJson(options =>
            //    {
            //    //全局忽略循环引用
            //    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //        options.SerializerSettings.ContractResolver = new CustomContractResolver();
            //    }
            //    );

            // 创建服务管理者  csla才注释掉
            //ServiceProvider = Services.BuildServiceProvider();
            //Services.AddSingleton(ServiceProvider);//注册到服务集合中,需要可以在Service中构造函数中注入使用
        }

        private static void RegisterContextManager(IServiceCollection services)
        {

            services.AddSingleton<ApplicationContextAccessor>();
            services.AddSingleton(typeof(IContextManager), typeof(ApplicationContextManagerAsyncLocal));
            var contextManagerType = typeof(IContextManager);
            // default to AsyncLocal context manager
            services.AddSingleton(contextManagerType, typeof(ApplicationContextManagerAsyncLocal));

            // var managerInit = services.Where(i => i.ServiceType.Equals(contextManagerType)).Any();
            // if (managerInit) return;

            // if (LoadContextManager(services, "RUINORERP.Model.ApplicationContextManager")) return;


        }

        private static bool LoadContextManager(IServiceCollection services, string managerTypeName)
        {
            var managerType = Type.GetType(managerTypeName, false);
            if (managerType != null)
            {
                services.AddSingleton(typeof(IContextManager), managerType);
                return true;
            }
            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void ConfigureContainerForDll(ContainerBuilder builder)
        {
            //var dalAssemble_common = System.Reflection.Assembly.LoadFrom("RUINORERP.Common.dll");
            //builder.RegisterAssemblyTypes(dalAssemble_common)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入


            /*
             Autofac 只会把 接口 IDuplicateCheckService 注册到容器；
DuplicateCheckService 这个 具体类 并不会被注册为可解析的 key。
因此你在解析时 只能用接口。
             */
            // 内存缓存配置


            // 纯内存缓存
            var cache = CacheFactory.Build<object>(c =>
            {
                c.WithSystemRuntimeCacheHandle("memHandle");
            });

            // 注册到 Autofac
            builder.RegisterInstance(cache)
                   .As<ICacheManager<object>>()
                   .SingleInstance();


            builder.RegisterType<DuplicateCheckService>()
           .AsSelf()                 // 允许按类解析 一般不注册自己？
           .As<IDuplicateCheckService>() // 允许按接口解析
           .SingleInstance();

            //// 1. 先建配置
            //var cacheCfg = ConfigurationBuilder.BuildConfiguration(cfg =>
            //{
            //    cfg.WithRedisConnection("redis", "127.0.0.1:6379") // 换成你的 Redis
            //       .WithRedisCacheHandle("redis", true);            // true = 作为回退
            //});




            var dalAssemble_WF = System.Reflection.Assembly.LoadFrom("RUINORERP.WF.dll");
            builder.RegisterAssemblyTypes(dalAssemble_WF)
                  .AsImplementedInterfaces().AsSelf()
                  .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                  .PropertiesAutowired();//允许属性注入



            var dalAssemble_Extensions = System.Reflection.Assembly.LoadFrom("RUINORERP.Extensions.dll");
            builder.RegisterAssemblyTypes(dalAssemble_Extensions)
                  .AsImplementedInterfaces().AsSelf()
                  .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                  .PropertiesAutowired();//允许属性注入


            var dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            builder.RegisterAssemblyTypes(dalAssemble)
             .AsImplementedInterfaces().AsSelf()
             .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
             .PropertiesAutowired();//允许属性注入


            Type[] tempModelTypes = dalAssemble.GetTypes();
            List<Type> ModelTypes = new List<Type>();
            List<Type> OtherModelTypes = new List<Type>();
            var suagrAttr = typeof(SugarTable);
            for (int i = 0; i < tempModelTypes.Length; i++)
            {

                if (!tempModelTypes[i].IsDefined(suagrAttr, false))
                {
                    //OtherModelTypes.Add(tempModelTypes[i]);
                }
                else
                {
                    if (tempModelTypes[i].BaseType == typeof(BaseEntity))
                    {
                        Type type = Assembly.LoadFrom("RUINORERP.Model.dll").GetType(tempModelTypes[i].FullName);
                        builder.Register(c => Activator.CreateInstance(type)).Named<BaseEntity>(tempModelTypes[i].Name);
                    }

                }
                // ModelTypes.Add(tempModelTypes[i]);
            }

            /*
            builder.RegisterTypes(OtherModelTypes.ToArray())
                  .AsImplementedInterfaces().AsSelf()
                  .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                  .PropertiesAutowired();//允许属性注入
            */

            // 获取所有待注入服务类
            //var dependencyService = typeof(IDependencyService);
            // var dependencyServiceArray = GetAllTypes(alldlls).ToArray();//  GlobalData.FxAllTypes
            //    .Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();
            var dalAssemble_Iservice = System.Reflection.Assembly.LoadFrom("RUINORERP.IServices.dll");
            builder.RegisterAssemblyTypes(dalAssemble_Iservice)
          .AsClosedTypesOf(typeof(IServices.BASE.IBaseServices<>));

            /*
            var asbly_service = System.Reflection.Assembly.LoadFrom("RUINORERP.Services.dll");
            var baseType = typeof(IDependencyService);
            //自动注入IDependency接口,支持AOP,生命周期为InstancePerDependency
            builder.RegisterTypes(asbly_service.GetTypes())
                .Where(x => baseType.IsAssignableFrom(x) && x != baseType)
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(BaseDataCacheAOP));
            */


            // builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerRequest();
            var dalAssemble_service = System.Reflection.Assembly.LoadFrom("RUINORERP.Services.dll");
            builder.RegisterTypes(dalAssemble_service.GetTypes())
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .InstancePerDependency();
            // .EnableInterfaceInterceptors();//打开AOP接口注入



            var dalAssemble_Business = System.Reflection.Assembly.LoadFrom("RUINORERP.Business.dll");


            Type[] tempTypes = dalAssemble_Business.GetTypes();


            List<KeyValuePair<string, Type>> ProcessorList = new List<KeyValuePair<string, Type>>();
            List<KeyValuePair<string, Type>> BaseControllerlist = new List<KeyValuePair<string, Type>>();
            List<KeyValuePair<string, Type>> BaseControllerGenericlist = new List<KeyValuePair<string, Type>>();
            //2023-12-22用名称注册验证器
            List<KeyValuePair<string, Type>> ValidatorGenericlist = new List<KeyValuePair<string, Type>>();


            //2024-9-04用名称注册验证器  新加了一个基类
            List<KeyValuePair<string, Type>> NewBaseValidatorGenericlist = new List<KeyValuePair<string, Type>>();


            List<Type> IOCTypes = new List<Type>();


            List<Type> IOCCslaTypes = new List<Type>();

            var NoWantIOCAttr = typeof(NoWantIOCAttribute);
            for (int i = 0; i < tempTypes.Length; i++)
            {
                // 是否为自定义特性，否则跳过，进入下一个循环
                //if (!tempTypes[i].IsDefined(NoWantIOCAttr, false))
                if (tempTypes[i].IsDefined(NoWantIOCAttr, false))
                    continue;
                if (tempTypes[i].Name == "Roles")
                {

                }
                //if (tempTypes[i].Name == "IWorkflowNotificationService")
                //{
                //    builder.RegisterType<WorkflowNotificationService>()
                //    .AsImplementedInterfaces().AsSelf()
                //    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                //    ;
                //    continue;
                //}
                //if (tempTypes[i].Name == "IStatusMachine")
                //{
                //    builder.RegisterType<BusinessStatusMachine>()
                //    .AsImplementedInterfaces().AsSelf()
                //    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                //    ;
                //    continue;
                //}
                if (tempTypes[i].Name == "IStatusHandler")
                {
                    builder.RegisterType<ProductionStatusHandler>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
                if (tempTypes[i].Name == "IAuditLogService")
                {
                    builder.RegisterType<AuditLogService>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
                if (tempTypes[i].Name == "IFMAuditLogService")
                {
                    builder.RegisterType<FMAuditLogService>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
                if (tempTypes[i].Name == "IAuthorizeController")
                {
                    builder.RegisterType<AuthorizeController>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
                if (tempTypes[i].Name == "QueryFilter")
                {
                    //单独处理这个类
                    builder.RegisterType(tempTypes[i]).Named("QueryFilter", typeof(QueryFilter))
                 .AsSelf() // 只注册为自身类型，不注册为实现的接口，避免接口拦截错误
                 .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                 .InstancePerDependency();

                    builder.RegisterType<QueryFilter>()
                  .AsSelf() // 只注册为自身类型，不注册为实现的接口，避免接口拦截错误
                  .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                  .InstancePerDependency();
                }

                if (tempTypes[i].Name.Contains("UseCsla"))
                {
                    IOCCslaTypes.Add(tempTypes[i]);
                }
                else
                {
                    IOCTypes.Add(tempTypes[i]);
                }
                if (tempTypes[i].BaseType == null)
                {
                    if (tempTypes[i].Name.Contains("tb_SysGlobalDynamicConfigController"))
                    {

                    }



                    continue;
                }
                if (tempTypes[i].BaseType == typeof(BaseProcessor))
                {
                    ProcessorList.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
                }


                //BaseController  继承这个的子类 用名称注册，为了baselist<T>中save统计处理
                if (tempTypes[i].BaseType == typeof(BaseController) && tempTypes[i].Name == "BaseController")
                {
                    BaseControllerlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
                }

                if (tempTypes[i].BaseType.Name.Contains("BaseController") && tempTypes[i].BaseType.IsGenericType)
                {
                    //泛型名称有一个尾巴，这里处理掉，但是总体要保持不能同时拥有同名的 泛型 和非泛型控制类
                    //否则就是调用解析时用加小尾巴
                    BaseControllerGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                }

                if (tempTypes[i].BaseType.Name.Contains("AbstractValidator")
                    && !tempTypes[i].Name.Contains("BaseValidatorGeneric")
                    && !tempTypes[i].BaseType.Name.Contains("BaseValidatorGeneric") && tempTypes[i].BaseType.IsGenericType)
                {
                    //泛型名称有一个尾巴，这里处理掉，但是总体要保持不能同时拥有同名的 泛型 和非泛型控制类
                    //否则就是调用解析时用加小尾巴
                    ValidatorGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                }

                //基类本身
                if (tempTypes[i].Name.Contains("BaseValidatorGeneric") && tempTypes[i].BaseType.IsGenericType)
                {
                    builder.RegisterGeneric(typeof(BaseValidatorGeneric<>));
                    builder.RegisterGeneric(typeof(AbstractValidator<>));
                }
                //子类
                if (tempTypes[i].BaseType.Name.Contains("BaseValidatorGeneric") && tempTypes[i].BaseType.IsGenericType)
                {
                    NewBaseValidatorGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                }

            }

            var regList = IOCTypes.Where(c => c.Name.Contains("SysGlobalDynamicConfigController")).ToList();
            if (regList.Count > 0)
            {

            }
            //加载RUINORERP.Business.dll并自动注册其中的服务
            var ExType = typeof(SearchType);
            builder.RegisterTypes(IOCTypes.ToArray())
            .Where(x => x.GetConstructors().Length > 0) //没有构造函数的排除
            .Where(x => x != ExType)//排除
            .Where(x => x != typeof(BizType))
            .Where(x => x.Namespace != "RUINORERP.Business.Cache") // 排除缓存架构命名空间，使用BusinessDIConfig.cs中的单个注册
             .AsImplementedInterfaces().AsSelf()
            .PropertiesAutowired()
            .InstancePerDependency();
            //==接口

            builder.RegisterTypes(IOCCslaTypes.ToArray())
                .Where(x => x.GetConstructors().Length > 0)
            //.AsClosedTypesOf(typeof(IServices.BASE.IBaseServices<>))
            .AsImplementedInterfaces().AsSelf()
            .PropertiesAutowired()
            .EnableClassInterceptors()//打开AOP类的虚方法注入
            .InstancePerDependency();//默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            // 移除EnableInterfaceInterceptors()，因为不是所有类都实现了公开可见的接口

            // .InterceptedBy(cacheType.ToArray());


            //用名称注册
            foreach (var item in ProcessorList)
            {
                builder.RegisterType(item.Value).Named(item.Key, typeof(BaseProcessor))
                .AsImplementedInterfaces().AsSelf()
                  .SingleInstance()
                .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                .InstancePerDependency();
                //.EnableInterfaceInterceptors();//不能启用， 是因为没有接口？
                //  .InterceptedBy(registerType.ToArray()); //以什么类型拦截
            }

            //用名称注册
            foreach (var item in BaseControllerlist)
            {
                builder.RegisterType(item.Value).Named(item.Key, typeof(BaseController))
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                .InstancePerDependency()
                .EnableInterfaceInterceptors();
                //  .InterceptedBy(registerType.ToArray()); //以什么类型拦截
            }

            //用名称注册泛型
            foreach (var item in BaseControllerGenericlist)
            {
                //builder.RegisterGeneric(typeof(BaseController<>));
                builder.RegisterGeneric(item.Value).Named(item.Key, typeof(BaseController<>))
                  .AsImplementedInterfaces().AsSelf()
                  .SingleInstance()
                  .PropertiesAutowired()
                 .InstancePerDependency();

                // .AsImplementedInterfaces().AsSelf()
                // .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                // .InstancePerDependency();
            }


            //用名称注册泛型
            foreach (var item in ValidatorGenericlist)
            {
                //builder.RegisterGeneric(typeof(BaseController<>));
                builder.RegisterType(item.Value)
               .AsImplementedInterfaces().AsSelf()
                  .SingleInstance()
                  .PropertiesAutowired()
                 .InstancePerDependency();
            }
            //用名称注册泛型
            foreach (var item in NewBaseValidatorGenericlist)
            {
                builder.RegisterType(item.Value)
               .AsImplementedInterfaces().AsSelf()
                  .SingleInstance()
                  .PropertiesAutowired()
                 .InstancePerDependency();
            }




            //AutofacDependencyResolver.Current.RequestLifetimeScope.ResolveNamed<INewsHelper>("news");
            //模块化注入 - 使用服务注册契约统一管理服务注册


            // 显式注册GridViewRelated为单例，确保整个应用程序中使用同一个实例
            builder.RegisterType<GridViewRelated>().SingleInstance();

            // 注册ILogger和SqlSugarScope服务，用于SqlSugarRowLevelAuthFilter的依赖注入
            //builder.RegisterInstance(Program.AppContextData.Db).As<SqlSugarScope>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerDependency();


            // 注册SqlSugarRowLevelAuthFilter
            builder.RegisterType<SqlSugarRowLevelAuthFilter>().AsSelf().InstancePerDependency();

            //builder.RegisterModule(new AutofacServiceRegister());
        }




        /// <summary>
        /// 要扫描的程序集名称
        /// 默认为[^Shop.Utils|^Shop.]多个使用|分隔
        /// </summary>
        public static string MatchAssemblies = "^RUINORERP.|^TEST.";

        /// <summary>
        /// 批量注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>更新后的服务集合</returns>
        public IServiceCollection BatchServiceRegister(IServiceCollection services)
        {
            #region 依赖注入
            try
            {
                var baseType = typeof(IDependency);
                var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

                // 安全检查路径
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentNullException("path", "无法获取应用程序目录路径");
                }

                // 获取RUINORERP相关的DLL文件
                var getFiles = Directory.GetFiles(path, "*.dll")
                    .Where(o => o.IndexOf("ruinor", StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                if (getFiles == null || getFiles.Count == 0)
                {
                    _logger.Warn("未找到需要注册的RUINORERP相关DLL文件");

                    return services;
                }

                var implementTypes = new List<Type>();
                var interfaceTypes = new List<Type>();

                // 遍历所有DLL文件，加载并分析其中的类型
                foreach (var file in getFiles)
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(file);
                        var types = assembly.GetTypes()
                            .Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface && baseType.IsAssignableFrom(t))
                            .ToList();

                        implementTypes.AddRange(types);

                        // 获取该程序集中的所有接口
                        var interfaces = assembly.GetTypes()
                            .Where(t => t.IsInterface && baseType.IsAssignableFrom(t))
                            .ToList();

                        interfaceTypes.AddRange(interfaces);
                    }
                    catch (Exception ex)
                    {
                        // 继续处理其他程序集，不中断整体注册流程
                        continue;
                    }
                }

                // 去重，避免重复注册
                implementTypes = implementTypes.Distinct().ToList();
                interfaceTypes = interfaceTypes.Distinct().ToList();


                // 注册所有实现类到对应的接口
                foreach (var implementType in implementTypes)
                {
                    try
                    {
                        // 获取该实现类实现的所有接口
                        var implementedInterfaces = implementType.GetInterfaces()
                            .Where(i => interfaceTypes.Contains(i))
                            .ToList();

                        if (implementedInterfaces.Count == 0)
                        {
                            _logger.Warn($"警告: 类实现了IDependency接口但未实现任何已发现的具体接口{implementType.FullName}");
                            continue;
                        }

                        foreach (var interfaceType in implementedInterfaces)
                        {
                            // 根据不同的接口类型选择不同的生命周期
                            if (typeof(IDependencyService).IsAssignableFrom(implementType))
                            {
                                services.AddScoped(interfaceType, implementType);
                                _logger.Debug($"已注册 Scoped 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                            else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
                            {
                                services.AddSingleton(interfaceType, implementType);
                                _logger.Debug($"已注册 Singleton 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                            else
                            {
                                services.AddTransient(interfaceType, implementType);
                                _logger.Debug($"已注册 Transient 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"注册服务 {implementType.FullName} 失败", ex);
                        // 继续处理其他服务，不中断整体注册流程
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("批量注册服务过程中发生严重错误", ex);
            }
            #endregion
            return services;
        }

        /// <summary>
        /// 检测系统是否支持语音合成功能
        /// </summary>
        /// <returns>是否支持语音合成</returns>
        private static bool IsSpeechSynthesisAvailable()
        {
            // 使用SystemSpeechVoiceReminder的静态方法检测系统支持
            return Common.SystemSpeechVoiceReminder.IsSystemSpeechSupported();
        }

        /// <summary>
        /// 获取服务实例缓存统计信息
        /// </summary>
        /// <returns>包含缓存命中率等统计信息的字符串</returns>
        public static string GetServiceCacheStatistics()
        {
            lock (_cacheLock)
            {
                int totalRequests = _cacheHits + _cacheMisses;
                double hitRate = totalRequests > 0 ? (double)_cacheHits / totalRequests * 100 : 0;
                
                return $"服务实例缓存统计: 缓存大小={_serviceInstanceCache.Count}/{MaxCacheSize}, " +
                       $"命中次数={_cacheHits}, 未命中次数={_cacheMisses}, " +
                       $"命中率={hitRate:F2}%";
            }
        }

        /// <summary>
        /// 清空服务实例缓存
        /// </summary>
        public static void ClearServiceCache()
        {
            lock (_cacheLock)
            {
                _serviceInstanceCache.Clear();
                _cacheHits = 0;
                _cacheMisses = 0;
                _logger.Info("服务实例缓存已清空");
            }
        }

        /// <summary>
        /// 程序集是否匹配
        /// </summary>
        public bool Match(string assemblyName)
        {
            assemblyName = System.IO.Path.GetFileName(assemblyName);
            if (assemblyName.StartsWith($"{AppDomain.CurrentDomain.FriendlyName}.Views"))
                return false;
            if (assemblyName.StartsWith($"{AppDomain.CurrentDomain.FriendlyName}.PrecompiledViews"))
                return false;
            return Regex.IsMatch(assemblyName, MatchAssemblies, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// 从Autofac容器中解析指定类型的服务
        /// </summary>
        /// <typeparam name="T">要解析的服务类型</typeparam>
        /// <returns>解析到的服务实例</returns>
        public static T GetFromFac<T>()
        {
            Type serviceType = typeof(T);
            
            try
            {
                // 检查容器是否初始化
                if (AutofacContainerScope == null)
                {
                    _logger.Warn($"警告: AutofacContainerScope尚未初始化，无法解析服务 {serviceType.FullName}");
                    return default(T);
                }

                // 直接从Autofac容器解析，让Autofac管理生命周期
                // SingleInstance服务：Autofac确保整个应用生命周期只创建一次
                // InstancePerDependency服务：每次都创建新实例
                // InstancePerLifetimeScope服务：在每个作用域内创建一次
                T service = AutofacContainerScope.Resolve<T>();
                
                _logger.Debug($"成功解析服务: {serviceType.FullName}");
                return service;
            }
            catch (Exception ex)
            {
                _logger.Error($"解析服务失败 {serviceType.FullName}", ex);
                // 错误处理：返回默认值而不是抛出异常，确保应用程序继续运行
                return default(T);
            }
        }

        /// <summary>
        /// 从Autofac容器中解析指定名称的服务
        /// </summary>
        /// <typeparam name="T">要解析的服务类型</typeparam>
        /// <param name="className">服务名称</param>
        /// <returns>解析到的服务实例</returns>
        public static T GetFromFacByName<T>(string className)
        {
            try
            {
                if (string.IsNullOrEmpty(className))
                {
                    _logger.Warn("警告: className参数为空，无法按名称解析服务");
                    return default(T);
                }

                // 检查容器是否初始化
                if (AutofacContainerScope == null)
                {
                    _logger.Warn($"警告: AutofacContainerScope尚未初始化，无法按名称解析服务 {className}");
                    return default(T);
                }

                // 记录服务解析日志
                _logger.Debug($"正在从Autofac容器中按名称解析服务: {typeof(T).FullName}, 名称: {className}");
                T service = AutofacContainerScope.ResolveNamed<T>(className);
                _logger.Debug($"成功按名称解析服务: {typeof(T).FullName}, 名称: {className}");
                return service;
            }
            catch (Exception ex)
            {
                _logger.Error($"按名称解析服务失败 {typeof(T).FullName}, 名称: {className}", ex);
                // 错误处理：返回默认值而不是抛出异常，确保应用程序继续运行
                return default(T);
            }
        }
    }
}