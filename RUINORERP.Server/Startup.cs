using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business.BizMapperService;
using Microsoft.Extensions.Configuration;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Repository.Base;
using RUINORERP.IRepository.Base;
using Autofac.Extras.DynamicProxy;
using System.Text.RegularExpressions;
using RUINORERP.Common.DI;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.Extensions.AOP;
using RUINORERP.Business;
using RUINORERP.Common.CustomAttribute;
using SqlSugar;
using RUINORERP.Model;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using RUINORERP.Model.Context;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.ServerSession;
using System.Threading.Tasks;
using SuperSocket.Server;
using SuperSocket;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging.EventLog;
using SuperSocket.Command;
using RUINORERP.Server.Commands;
using Microsoft.Extensions.Options;
using RUINORERP.Global;
#pragma warning disable CS0105 // using 指令以前在此命名空间中出现过
using RUINORERP.Common.Log4Net;
#pragma warning restore CS0105 // using 指令以前在此命名空间中出现过
using RUINORERP.Business.Processor;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;
using RUINORERP.WF.WorkFlow;
using FluentValidation;
using RUINORERP.Server.BizService;
using Microsoft.Extensions.Caching.Memory;
using Autofac.Core;
using RUINORERP.Business.Security;
using RUINORERP.Model.ConfigModel;
using System.Configuration;
using RUINORERP.Server.SuperSocketServices;
using ZXing;
using RUINORERP.Server.SmartReminder;
using RUINORERP.Server.SmartReminder.ReminderRuleStrategy;
using Newtonsoft.Json;
using RUINORERP.Model.Base;
using RUINORERP.Business.CommService;
using System.Runtime.InteropServices.JavaScript;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;
using RUINORERP.Business.RowLevelAuthService;
// 引入各项目的DI配置类
using RUINORERP.Business.DI;
using RUINORERP.Services.DI;
using RUINORERP.Repository.DI;
using RUINORERP.IServices.DI;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.Server.Network.DI;
using RUINORERP.PacketSpec.DI;
// 添加缓存相关的using语句
using CacheManager.Core;

namespace RUINORERP.Server
{
    /// <summary>
    /// 应用程序启动配置类
    /// 负责配置依赖注入、服务注册等启动时需要的配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Autofac容器实例
        /// </summary>
        public static IContainer AutoFacContainer;

        /// <summary>
        /// 服务容器集合
        /// </summary>
        public static IServiceCollection services { get; set; }
        
        /// <summary>
        /// 服务提供者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Autofac生命周期容器作用域
        /// </summary>
        public static ILifetimeScope AutofacContainerScope { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Startup()
        {

        }

        /// <summary>
        /// 配置容器构建器
        /// </summary>
        /// <param name="bc">主机构建器上下文</param>
        /// <param name="builder">容器构建器</param>
        void cb(HostBuilderContext bc, ContainerBuilder builder)
        {
            #region 服务注册配置
            services = new ServiceCollection();

            // 注册当前程序集的所有类成员
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
                
            // 配置核心服务
            ConfigureServices(services);
            
            // 配置外部DLL依赖注入
            ConfigureContainerForDll(builder);
       

            // 注册特定类型和参数
            builder.RegisterType<AutoComplete>()
                .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);
                
            builder.RegisterType<BizCodeGenerator>(); // 注册拦截器
            
            // 注册AOP拦截器
            builder.RegisterType<BaseDataCacheAOP>(); // 注册拦截器
            
            builder.RegisterType<PersonBus>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
            
            builder.RegisterType<tb_DepartmentServices>().As<Itb_DepartmentServices>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .EnableInterfaceInterceptors().InterceptedBy(typeof(BaseDataCacheAOP));

            // 初始化应用程序上下文
            string conn = AppSettings.GetValue("ConnectString");
            Program.InitAppcontextValue(Program.AppContextData);
            
            // 配置日志服务
            services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                //引用的long4net.dll要版本一样。
                string key = "ruinor1234567890";
                string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);

                logBuilder.AddProvider(new Log4NetProviderByCustomeDb("Log4net_db.config", newconn, Program.AppContextData));
                
                // 设置日志级别过滤规则
                logBuilder.AddFilter((provider, category, logLevel) =>
                {
                    // 所有RUINORERP开头的命名空间使用Error级别
                    if (category.StartsWith("RUINORERP"))
                    {
                        return logLevel >= LogLevel.Error;
                    }
                    if (category.StartsWith("WorkflowCore"))
                    {
                        return logLevel >= LogLevel.Error;
                    }

                    // 保留原有其他过滤规则
                    else if (category == "Microsoft.Hosting.Lifetime" ||
                             category.StartsWith("Microsoft") ||
                             category.StartsWith("System.Net.Http.HttpClient"))
                    {
                        return logLevel >= LogLevel.Error;
                    }
                    // 其他日志正常记录  
                    //所有的都变为错误才记录
                    return logLevel >= LogLevel.Error;
                });
            });

            // 注册工作流相关服务（单例模式）
            builder.RegisterType(typeof(WorkflowRegisterService))
                 .AsImplementedInterfaces()
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()
                 .PropertiesAutowired()
                 .SingleInstance();

            builder.RegisterType(typeof(RUINORERP.Business.CommService.BillConverterFactory))
                 .AsImplementedInterfaces()
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()
                 .PropertiesAutowired()
                 .SingleInstance();
            #endregion

            #region 智能提醒服务配置
            builder.RegisterType<WorkflowReminderService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterType<NotificationService>().As<INotificationService>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterType<SmartReminderMonitor>().As<ISmartReminderMonitor>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterType<SafetyStockStrategy>().As<IReminderStrategy>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces().AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            // 注册智能提醒模块
            builder.RegisterModule<SmartReminderModule>();
            #endregion

            #region 网络服务配置
            // 配置网络服务容器
            builder.ConfigureNetworkServicesContainer();
            #endregion

            // 将Microsoft.Extensions.DependencyInjection服务注入到Autofac容器中
            builder.Populate(services);
        }

        /// <summary>
        /// 配置CSLA依赖注入端口
        /// </summary>
        /// <returns>主机实例</returns>
        public IHost CslaDIPort()
        {
            var hostBuilder = new HostBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>(new Action<HostBuilderContext, ContainerBuilder>(cb))
                .ConfigureServices((context, services) =>
                {
                    services.AddAutofac();
                    SafetyStockWorkflowConfig.RegisterWorkflow(services);
                })
                .Build();

            return hostBuilder;
        }


        #region 核心服务配置
        /// <summary>
        /// 配置核心服务
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            // 注册旧系统CommandDispatcher（临时保留，逐步迁移）
            services.AddSingleton<CommandDispatcher>();


            #region 配置文件创建
            // 配置文件所在的目录
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles");
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }

            // 检查并生成 SystemGlobalconfig 配置文件
            string systemGlobalConfigPath = Path.Combine(configDirectory, nameof(SystemGlobalconfig) + ".json");
            if (!File.Exists(systemGlobalConfigPath))
            {
                var systemGlobalConfig = new SystemGlobalconfig();
                string systemGlobalConfigJson = JsonConvert.SerializeObject(new { SystemGlobalconfig = systemGlobalConfig }, Formatting.Indented);
                File.WriteAllText(systemGlobalConfigPath, systemGlobalConfigJson);
            }

            // 检查并生成 GlobalValidatorConfig 配置文件
            string globalValidatorConfigPath = Path.Combine(configDirectory, nameof(GlobalValidatorConfig) + ".json");
            if (!File.Exists(globalValidatorConfigPath))
            {
                var globalValidatorConfig = new GlobalValidatorConfig();
                string globalValidatorConfigJson = JsonConvert.SerializeObject(globalValidatorConfig, Formatting.Indented);
                File.WriteAllText(globalValidatorConfigPath, globalValidatorConfigJson);
            }
            #endregion

            // 读取自定义的 JSON 配置文件
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "SysConfigFiles"))
                .AddJsonFile(nameof(SystemGlobalconfig) + ".json", optional: false, reloadOnChange: true)
                .AddJsonFile(nameof(GlobalValidatorConfig) + ".json", optional: false, reloadOnChange: true)
                .Build();

            services.Configure<SystemGlobalconfig>(builder.GetSection(nameof(SystemGlobalconfig)));
            services.Configure<GlobalValidatorConfig>(builder.GetSection(nameof(GlobalValidatorConfig)));

            services.AddSingleton(typeof(frmMain));//MDI最大。才开一次才能单例

            #region 工作流服务配置
            // 这是新增加工作流的服务
            services.AddWorkflowCoreServicesNew();

            // 添加新的SuperSocket服务
            IConfigurationBuilder configurationBuilder2 = new ConfigurationBuilder();
            var cfgBuilder2 = configurationBuilder2.AddJsonFile("appsettings.json");
            IConfiguration configuration2 = cfgBuilder2.Build();
            #endregion

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // 注入仓储
            services.AddTransient<IUnitOfWorkManage, UnitOfWorkManage>(); // 注入工作单元

            //Add Memory Cache
            services.AddOptions();
            services.AddMemoryCache();
            services.AddMemoryCacheSetup();
            services.AddDistributedMemoryCache();

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

           

            // 注册统一缓存管理器
            // services.AddSingleton<UnifiedCacheManager>();
            // services.AddSingleton<CacheSyncService>();
            // services.AddSingleton<CacheEventPublisher>();

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);
            services.AddSqlsugarSetup(Program.AppContextData, newconn);
            services.AddAppContext(Program.AppContextData);

            services.AddSingleton(typeof(Comm.AutoMapperConfig));
            IMapper mapper = Comm.AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddScoped<IMapper, Mapper>();
            services.AddSingleton<IMapper>(mapper);

            // 注册审计日志服务
            services.AddSingleton<IFMAuditLogService, FMAuditLogService>();

            // 注册各项目的服务（使用各项目自己的DI配置）
            services.AddNetworkServices();      // 网络服务
            services.AddPacketSpecServices(configuration);   // PacketSpec服务
        }
        #endregion

        #region 外部DLL依赖注入配置
        /// <summary>
        /// 配置外部DLL依赖注入
        /// </summary>
        /// <param name="builder">容器构建器</param>
        public static void ConfigureContainerForDll(ContainerBuilder builder)
        {

            #region Extensions程序集依赖注入
            var dalAssemble_Extensions = System.Reflection.Assembly.LoadFrom("RUINORERP.Extensions.dll");
            builder.RegisterAssemblyTypes(dalAssemble_Extensions)
                  .AsImplementedInterfaces().AsSelf()
                  .InstancePerDependency()
                  .PropertiesAutowired();
            #endregion

            #region Model程序集依赖注入
            var dalAssemble = System.Reflection.Assembly.LoadFrom("RUINORERP.Model.dll");
            builder.RegisterAssemblyTypes(dalAssemble)
             .AsImplementedInterfaces().AsSelf()
             .InstancePerDependency()
             .PropertiesAutowired();

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
                    if (tempModelTypes[i].BaseType == typeof(BaseConfig))
                    {
                        Type type = Assembly.LoadFrom("RUINORERP.Model.dll").GetType(tempModelTypes[i].FullName);
                        builder.Register(c => Activator.CreateInstance(type)).Named<BaseConfig>(tempModelTypes[i].Name);
                    }
                }
            }
            #endregion

            #region 使用各项目的DI配置类
            // 配置各项目的依赖注入
            BusinessDIConfig.ConfigureContainer(builder);      // Business项目
            ServicesDIConfig.ConfigureContainer(builder);      // Services项目
            RepositoryDIConfig.ConfigureContainer(builder);    // Repository项目
            IServicesDIConfig.ConfigureContainer(builder);     // IServices项目
            #endregion

            #region 网络服务配置
            // 配置SuperSocket服务Autofac容器
            builder.ConfigureNetworkServicesContainer();

            builder.RegisterModule<SmartReminderModule>();
            #endregion
        }
        #endregion

        /// <summary>
        /// 要扫描的程序集名称
        /// 默认为[^Shop.Utils|^Shop.]多个使用|分隔
        /// </summary>
        public static string MatchAssemblies = "^RUINORERP.|^TETS.";

        /// <summary>
        /// 批量服务注册
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>更新后的服务集合</returns>
        public IServiceCollection BatchServiceRegister(IServiceCollection services)
        {
            #region 依赖注入
            var baseType = typeof(IDependency);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;

            var getFiles = Directory.GetFiles(path, "*.dll").Where(o => o.ToLower().StartsWith(@path.ToLower() + "ruinor"));//路径筛选

            List<string> dlls = new List<string>();
            foreach (var file in getFiles)
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Name.ToLower().Contains("ruinor"))
                {
                    dlls.Add(file);
                }
            }
            var referencedAssemblies = getFiles.Select(Assembly.LoadFrom).ToList();
            var ss = referencedAssemblies.SelectMany(o => o.GetTypes());
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x))
                .ToList();

            var implementTypes = types.Where(x => x.IsClass).ToList();
            var interfaceTypes = types.Where(x => x.IsInterface).ToList();

            foreach (var implementType in implementTypes)
            {
                if (typeof(IDependencyService).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddScoped(interfaceType, implementType);
                }
                else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddSingleton(interfaceType, implementType);
                }
                else
                {
                    var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                    if (interfaceType != null)
                        services.AddTransient(interfaceType, implementType);
                }
            }
            #endregion
            return services;
        }
 

        /// <summary>
        /// 从Autofac容器获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>服务实例</returns>
        public static T GetFromFac<T>()
        {
            return AutofacContainerScope.Resolve<T>();
        }

        /// <summary>
        /// 根据名称从Autofac容器获取服务实例
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="className">类名称</param>
        /// <returns>服务实例</returns>
        public static T GetFromFacByName<T>(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return default(T);
            }

            return AutofacContainerScope.ResolveNamed<T>(className);
        }
    }
}