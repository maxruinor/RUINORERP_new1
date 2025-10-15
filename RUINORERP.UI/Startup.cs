using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Business.AutoMapper;
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
using Krypton.Toolkit;
using WorkflowCore.Interface;
using RUINORERP.UI.WorkFlowTester;
using RUINORERP.Services;
using RUINORERP.IServices;
using RUINORERP.Extensions.AOP;
using RUINORERP.Business;
using RUINORERP.Common.CustomAttribute;
using SqlSugar;
using RUINORERP.Model;
using CacheManager.Core;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using RUINORERP.Common.SnowflakeIdHelper;
using RUINORERP.UI.ATechnologyStack.ServiceRegister;
using RUINORERP.Extensions.ServiceExtensions;
using RUINORERP.UI.BaseForm;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Global;
using RUINORERP.UI.Common;
using FluentValidation;
using RUINORERP.Business.Processor;
using WorkflowCore.Services;
using RUINORERP.UI.SysConfig;
using RUINORERP.Business.Security;
using Castle.Core.Logging;
using RUINORERP.Model.TransModel;
using RUINORERP.Model.ConfigModel;
using RUINORERP.UI.IM;
using System.Net.Mail;
using StackExchange.Redis;

using FastReport.DevComponents.DotNetBar;
using RUINORERP.UI.FM;
using RUINORERP.Global.EnumExt;
using RUINORERP.Business.CommService;
using Newtonsoft.Json;
using RUINORERP.UI.ATechnologyStack;
using RUINORERP.Model.Base;
using RUINORERP.UI.Monitoring.Auditing;
using RUINORERP.Model.CommonModel;
using RUINORERP.Business.BizMapperService;
using RUINORERP.UI.BusinessService.SmartMenuService;
using RUINORERP.UI.WorkFlowDesigner;
using RUINORERP.Business.LogicaService;
using AutoMapper.Internal;
using RUINORERP.Business.RowLevelAuthService;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using RUINORERP.PacketSpec.DI;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.DI;
using RUINORERP.Business.DI;
using RUINORERP.IServices.DI;
using RUINORERP.Services.DI;
using RUINORERP.Repository.DI;
using RUINORERP.UI.UserCenter.DataParts;
using RUINORERP.UI.Network.Authentication;
namespace RUINORERP.UI
{
    public class Startup
    {
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
            //要在第一行，里面有         Services = new ServiceCollection();
            MainRegister(bc, builder);
            // 配置基础服务
            ConfigureBaseServices(Services);

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
            // 配置日志（最先注册）
            ConfigureLogging(services);

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

        /// <summary>
        /// 配置日志服务
        /// </summary>
        private static void ConfigureLogging(IServiceCollection services)
        {
            try
            {
                // 获取解密后的数据库连接字符串
                string connectionString = null;

                // 尝试通过CryptoHelper获取（这是标准方式）
                try
                {
                    connectionString = CryptoHelper.GetDecryptedConnectionString();
                    Console.WriteLine("通过CryptoHelper成功获取解密的连接字符串");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("警告：通过CryptoHelper获取连接字符串失败: " + ex.Message);

                    // 备选方案：直接从应用设置和硬编码密钥获取
                    try
                    {
                        string conn = AppSettings.GetValue("ConnectString");
                        if (!string.IsNullOrEmpty(conn))
                        {
                            string key = "ruinor1234567890";
                            connectionString = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);
                            Console.WriteLine("通过备选方式成功获取解密的连接字符串");
                        }
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("警告：备选方式获取连接字符串也失败: " + ex2.Message);
                    }
                }

                // 注册日志服务
                services.AddLogging(logBuilder =>
                {
                    // 清除所有现有提供者，避免冲突
                    logBuilder.ClearProviders();

                    // 添加自定义的数据库日志提供者
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        Console.WriteLine("注册Log4NetProviderByCustomeDb，使用log4net.config配置数据库日志");
                        logBuilder.AddProvider(new Log4NetProviderByCustomeDb("log4net.config", connectionString, Program.AppContextData));
                    }
                    else
                    {
                        // 如果没有有效的连接字符串，添加控制台日志作为备用
                        Console.WriteLine("警告：没有有效的数据库连接字符串，使用控制台日志作为备用");
                        logBuilder.AddConsole();
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("配置日志服务时出错: " + ex.Message);
                // 即使出错，也确保有基础日志功能
                services.AddLogging(logBuilder =>
                {
                    logBuilder.AddConsole();
                });
            }
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
                .AddJsonFile(nameof(SystemGlobalconfig) + ".json", optional: false, reloadOnChange: true)
                .AddJsonFile(nameof(GlobalValidatorConfig) + ".json", optional: false, reloadOnChange: true)
                .Build();

            services.Configure<SystemGlobalconfig>(builder.GetSection(nameof(SystemGlobalconfig)));
            services.Configure<GlobalValidatorConfig>(builder.GetSection(nameof(GlobalValidatorConfig)));

            services.AddSingleton(typeof(ConfigManager));
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
            string systemGlobalConfigPath = Path.Combine(configDirectory, nameof(SystemGlobalconfig) + ".json");
            if (!File.Exists(systemGlobalConfigPath))
            {
                var systemGlobalConfig = new SystemGlobalconfig();
                string systemGlobalConfigJson = JsonConvert.SerializeObject(new { SystemGlobalconfig = systemGlobalConfig }, Formatting.Indented);
                File.WriteAllText(systemGlobalConfigPath, systemGlobalConfigJson);
            }

            // 创建全局验证器配置
            string globalValidatorConfigPath = Path.Combine(configDirectory, nameof(GlobalValidatorConfig) + ".json");
            if (!File.Exists(globalValidatorConfigPath))
            {
                var globalValidatorConfig = new GlobalValidatorConfig();
                string globalValidatorConfigJson = JsonConvert.SerializeObject(globalValidatorConfig, Formatting.Indented);
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
            // 注册网络通信服务
            services.AddNetworkServices();

      

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

            // 注册当前程序集 - 排除不需要注册的类型
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(type => !typeof(IExcludeFromRegistration).IsAssignableFrom(type))
                .AsImplementedInterfaces()
                .AsSelf();
        }

        /// <summary>
        /// 注册各项目模块服务 - UI层只需引用其他项目DI目录中的注册服务类
        /// </summary>
        private static void RegisterProjectModuleServices(ContainerBuilder builder)
        {
            try
            {
                // 配置各项目的依赖注入
                BusinessDIConfig.ConfigureContainer(builder);      // Business项目
                ServicesDIConfig.ConfigureContainer(builder);      // Services项目
                RepositoryDIConfig.ConfigureContainer(builder);    // Repository项目
                IServicesDIConfig.ConfigureContainer(builder);     // IServices项目
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("警告：使用模块DI配置类注册服务失败，回退到传统方式注册。错误信息：" + ex.Message);

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
                Console.WriteLine(string.Format("注册UCTodoList失败: {0}", ex.Message));
            }
        }

        /// <summary>
        /// 配置AOP
        /// </summary>
        private static void ConfigureAOP(ContainerBuilder builder)
        {
            builder.RegisterType<AutoComplete>()
                .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);

            builder.RegisterType<BizCodeGenerator>();
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

            builder.RegisterModule(new AutofacServiceRegister());
        }

        /// <summary>
        /// 注册程序集类型
        /// </summary>
        private static void RegisterAssemblyTypes(ContainerBuilder builder, string assemblyName)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyName);
                builder.RegisterAssemblyTypes(assembly)
                    .AsImplementedInterfaces()
                    .AsSelf()
                    .InstancePerDependency()
                    .PropertiesAutowired();
            }
            catch (Exception ex)
            {
                // 记录程序集加载失败日志
                Console.WriteLine($"加载程序集 {assemblyName} 失败: {ex.Message}");
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
            Services = new ServiceCollection();

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

            //直接实现。或排除
            //builder.Register(c => new ClientEventManager()).As<IClientEventManager>();
            //注册当前程序集的所有类成员
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            //    .AsImplementedInterfaces().AsSelf();

            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            //.Where(type => type != typeof(ClientEventManager)) // 排除指定类
            //.AsImplementedInterfaces()
            //.AsSelf();


            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            .Where(type => !typeof(IExcludeFromRegistration).IsAssignableFrom(type)) // 排除实现了 IExcludeFromRegistration 接口的类
            .AsImplementedInterfaces()
            .AsSelf();




            //覆盖上面自动注册的？说是最后的才是

            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
            //如果注册为名称的，需要这样操作
            builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
            .AsImplementedInterfaces().AsSelf();



            ConfigureContainerForDll(builder);

            RegisterForm(builder);

            //将配置添加到ConfigurationBuilder
            //var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            //config.AddJsonFile("autofac.json");
            //config.AddJsonFile来自Microsoft.Extensions.Configuration.Json
            //config.AddXmlFile来自Microsoft.Extensions.Configuration.Xml

            //用Autofac注册ConfigurationModule
            //var module = new ConfigurationModule(config.Build());

            //builder.RegisterModule(module);
            //var Configuration = AutofacCore.GetFromFac<IConfiguration>();
            //AppId = Configuration["AppSettings:AppId"];
            //AppSecret = Configuration["AppSettings:AppSecret"];

            // builder.Register(c => new BI.UCLocation()).Named<UserControl>("UCLocation");
            //builder.Register(c => System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("RUINORERP.UI.BI.UCLocation")).Named<UserControl>("UCLocation");

            //builder.RegisterType<ProductEAV.UCProductEdit>();


            //builder.RegisterType<Ean13>()
            //      //选择类型默认最多的，这里用无参的，实际没有的构造函数,如果类型为别的类型，就typeof(别的类型，和构造函数保持一直即可)
            //      //实际是我在代码中直接实例化的，不需要注入，是不是可以做一个特性，标识不需要参与批量注入
            //      .UsingConstructor();

            builder.RegisterType<AutoComplete>()
            .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);
            builder.RegisterType<BizCodeGenerator>(); // 注册拦截器
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
            //builder.RegisterType<tb_DepartmentServices>().Named<Itb_DepartmentServices>(typeof(tb_DepartmentServices).Name).InstancePerLifetimeScope().EnableInterfaceInterceptors();
            //builder.RegisterType<FactoryTwo>().Named<IServiceFactory>(typeof(FactoryTwo).Name).InstancePerLifetimeScope().EnableClassInterceptors();

            //var intermediateFactory = container.Resolve<Func<B, C>>();
            //Func<A, C> factory =
            //    a => intermediateFactory(container.Resolve(TypedParameter.From(a)));
            //var x = factory(new A());

            //注册是最后的覆盖前面的 ，AOP测试时，业务控制器中的方法不生效。与 ConfigureContainer(builder); 中注册的方式有关。可能参数不对。
            //后面需要研究
            //IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            // var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            // IConfiguration configuration = cfgBuilder.Build();
            /// AppSettings.Configuration = configuration;
            /// 
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);

            Program.InitAppcontextValue(Program.AppContextData);
            //日志要放在最前面，因为要使用
            Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
                //引用的long4net.dll要版本一样。
                logBuilder.AddProvider(new Log4NetProviderByCustomeDb("log4net.config", newconn, Program.AppContextData));
            });

            // by watson 2024-6-28

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

            ////覆盖上面自动dll批量注入的方法，因为要用单例模式
            builder.RegisterType(typeof(RUINORERP.Business.CommService.BillConverterFactory))
                 .AsImplementedInterfaces() //注册所有实现的接口
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()//打开AOP类的虚方法注入 同时启用接口和类拦截器可能导致冲突
                 .PropertiesAutowired()//指定属性注入
                 .SingleInstance();


            // 注册 AuditLogHelper 为可注入服务
            builder.RegisterType<AuditLogHelper>()
                   .AsSelf()
                   .InstancePerLifetimeScope() // 或根据需要使用 SingleInstance() 或根据需要选择生命周期
                  .PropertiesAutowired();


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
                 RUINORERP.Common.Log4Net.Logger.Error("配置应用程序设置失败", ex);
             }

         })
         .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //正确配置Autofac容器
        .ConfigureContainer<ContainerBuilder>(ConfigureAutofacContainer)
      /*
        .ConfigureContainer<ContainerBuilder>(builder =>
         {
             #region  注册
             // Services = new ServiceCollection();
             //BatchServiceRegister(Services);
             //ConfigureServices(Services);

             //注册当前程序集的所有类成员
             builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                 .AsImplementedInterfaces().AsSelf();

             //覆盖上面自动注册的？说是最后的才是
             //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();

             builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
             .AsImplementedInterfaces().AsSelf();

             ConfigureContainer(builder);

             RegisterForm(builder);


             // builder.Register(c => new BI.UCLocation()).Named<UserControl>("UCLocation");
             //builder.Register(c => System.Reflection.Assembly.GetExecutingAssembly().CreateInstance("RUINORERP.UI.BI.UCLocation")).Named<UserControl>("UCLocation");

             //builder.RegisterType<ProductEAV.UCProductEdit>();


             //builder.RegisterType<Ean13>()
             //      //选择类型默认最多的，这里用无参的，实际没有的构造函数,如果类型为别的类型，就typeof(别的类型，和构造函数保持一直即可)
             //      //实际是我在代码中直接实例化的，不需要注入，是不是可以做一个特性，标识不需要参与批量注入
             //      .UsingConstructor();

             builder.RegisterType<AutoComplete>()
             .WithParameter((pi, c) => pi.ParameterType == typeof(SearchType), (pi, c) => SearchType.Document);
                                                              // 注册依赖
             builder.RegisterType<BaseDataCacheAOP>(); // 注册拦截器
                                                       //builder.RegisterType<LogInterceptor>(); // 注册拦截器
                                                       //builder.RegisterType<Person>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
             builder.RegisterType<Person>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
                                                                                                                //builder.RegisterType<AOPDllTest.PersonDLL>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
             builder.RegisterType<PersonBus>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截

             builder.RegisterType<tb_DepartmentServices>().As<Itb_DepartmentServices>()
                 .AsImplementedInterfaces()
                 .InstancePerLifetimeScope()
                 .EnableInterfaceInterceptors().InterceptedBy(typeof(BaseDataCacheAOP));
             //builder.RegisterType<tb_DepartmentServices>().Named<Itb_DepartmentServices>(typeof(tb_DepartmentServices).Name).InstancePerLifetimeScope().EnableInterfaceInterceptors();
             //builder.RegisterType<FactoryTwo>().Named<IServiceFactory>(typeof(FactoryTwo).Name).InstancePerLifetimeScope().EnableClassInterceptors();

             //var intermediateFactory = container.Resolve<Func<B, C>>();
             //Func<A, C> factory =
             //    a => intermediateFactory(container.Resolve(TypedParameter.From(a)));
             //var x = factory(new A());

             builder.RegisterType<Form2>();

             //注册是最后的覆盖前面的 ，AOP测试时，业务控制器中的方法不生效。与 ConfigureContainer(builder); 中注册的方式有关。可能参数不对。
             //后面需要研究
             builder.Populate(Services);//将自带的也注入到autofac

             //AutoFacContainer = builder.Build();
             ////var container = containerBuilder.Build();
             //new AutofacServiceProvider(AutoFacContainer);

             //   _containerBuilder = builder;
             #endregion

         })
      */
         .ConfigureServices((context, services) =>
         {

       
             services.AddAutofac();
         }).Build();

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
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                             //.AsImplementedInterfaces().AsSelf() //加上这一行，会出错
                             // .EnableInterfaceInterceptors()
                             //.InstancePerDependency()//默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                             // .EnableClassInterceptors()//打开AOP类的虚方法注入
                             .PropertiesAutowired();//指定属性注入



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
                //builder.RegisterType(type)
                //          .As<BaseQuery>()
                //          //.WithMetadata("BillType", "Payment") // 添加额外元数据
                //          .PropertiesAutowired();

                _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseNavigator>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
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
            services.AddTransient<INotificationSender, EmailNotificationSender>();
            #region 创建配置文件-开始
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

            services.AddSingleton(typeof(ConfigManager));
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
            //services.AddLogging();

            #region 日志


            //  new IdHelperBootstrapper().SetWorkderId(1).Boot();

            //为了修改为DB添加字段，覆盖这前面的




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
            //services.AddTransient<WF.BizOperation.WFSO>();//手动注册
            //注入Log4Net
            //Services.AddLogging(cfg =>
            //{
            //    //cfg.AddLog4Net();
            //    //默认的配置文件路径是在根目录，且文件名为log4net.config
            //    //如果文件路径或名称有变化，需要重新设置其路径或名称
            //    //比如在项目根目录下创建一个名为cfg的文件夹，将log4net.config文件移入其中，并改名为log.config
            //    //则需要使用下面的代码来进行配置
            //    //cfg.AddLog4Net(new Log4NetProviderOptions()
            //    //{
            //    //    Log4NetConfigFileName = "cfg/log.config",
            //    //    Watch = true
            //    //});
            //});
            #endregion
            /*
            var sqlSugarScope = new SqlSugar.SqlSugarScope(new SqlSugar.ConnectionConfig
            {
                ConnectionString = "Server=192.168.0.250;Database=erp;UID=sa;Password=sa",
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
            });
            services.AddSingleton<SqlSugar.ISqlSugarClient>(sqlSugarScope); // SqlSugar 官网推荐用单例模式注入
            */


            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // 注入仓储
            /* by watson 2024-6-28
         services.AddTransient<IUnitOfWorkManage, UnitOfWorkManage>(); // 注入工作单元
         */

            // services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            // services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddMemoryCacheSetup();

            //这个缓存可能更好。暂时没有去实现，用了直接简单的方式
            //services.AddRedisCacheSetup();

            services.AddAppContext(Program.AppContextData);


            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
            string conn = AppSettings.GetValue("ConnectString");
            string key = "ruinor1234567890";
            string newconn = HLH.Lib.Security.EncryptionHelper.AesDecrypt(conn, key);
            services.AddSqlsugarSetup(Program.AppContextData, newconn);

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
                if (tempTypes[i].Name == "IWorkflowNotificationService")
                {
                    builder.RegisterType<WorkflowNotificationService>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
                if (tempTypes[i].Name == "IStatusMachine")
                {
                    builder.RegisterType<BusinessStatusMachine>()
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                    ;
                    continue;
                }
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
                 .AsImplementedInterfaces().AsSelf()
                 .PropertiesAutowired() //属性注入 如果没有这个  public Itb_LocationTypeServices _tb_LocationTypeServices { get; set; }  这个值会没有，所以实际后为null
                 .InstancePerDependency();

                    builder.RegisterType<QueryFilter>()
                  .AsImplementedInterfaces().AsSelf()
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
            .InstancePerDependency()//默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            .EnableInterfaceInterceptors();

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


            //如果多次注册以后最后准
            //https://blog.csdn.net/weixin_30778805/article/details/97148925

            //var dalAssemble_Repository = System.Reflection.Assembly.LoadFrom("RUINORERP.Repository.dll");
            //builder.RegisterTypes(dalAssemble_Repository.GetTypes())
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired()
            //    .InstancePerDependency();

            // builder.RegisterTypes(typeof(IBaseRepository<>), typeof(BaseRepository<>)).SingleInstance(); // 注入仓储
            // builder.RegisterType<UnitOfWorkManage>().As<IUnitOfWorkManage>().InstancePerDependency();
            //var dalAssembleCore = Assembly.LoadFrom("RUINOR.Framework.Core.dll");
            //builder.RegisterAssemblyTypes(dalAssembleCore)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入
            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton(typeof(AutoMapperConfig));
            //services.AddScoped<IMapper, Mapper>();
            //builder.RegisterType(ipmap)

            //     builder.RegisterType<TestService>()
            //.UsingConstructor(typeof(TransientService), typeof(SingletonService));


            //AutofacDependencyResolver.Current.RequestLifetimeScope.ResolveNamed<INewsHelper>("news");
            //模块化注入 - 使用服务注册契约统一管理服务注册
          

            // 显式注册GridViewRelated为单例，确保整个应用程序中使用同一个实例
            builder.RegisterType<GridViewRelated>().SingleInstance();

            // 注册ILogger和SqlSugarScope服务，用于SqlSugarRowLevelAuthFilter的依赖注入
            builder.RegisterInstance(Program.AppContextData.Db).As<SqlSugarScope>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>)).InstancePerDependency();
            builder.RegisterType<Log4NetLoggerByDb>().As<ILogger>().InstancePerDependency();

            // 注册SqlSugarRowLevelAuthFilter
            builder.RegisterType<SqlSugarRowLevelAuthFilter>().AsSelf().InstancePerDependency();

            builder.RegisterModule(new AutofacServiceRegister());
        }

        //public void Configure(IApplicationBuilder app, MyContext myContext,
        //    IQuartzNetService quartzNetService,
        //    ISchedulerCenterService schedulerCenter, ILoggerFactory loggerFactory)
        //{
        //    var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        //    if (locOptions != null) app.UseRequestLocalization(locOptions.Value);
        //    //获取远程真实ip,如果不是nginx代理部署可以不要
        //    app.UseMiddleware<RealIpMiddleware>();
        //    //IP限流
        //    app.UseIpLimitMiddleware();
        //    //日志
        //    loggerFactory.AddLog4Net();
        //    if (WebHostEnvironment.IsDevelopment())
        //    {
        //        app.UseDeveloperExceptionPage();
        //    }

        //    app.Use(next => context =>
        //    {
        //        context.Request.EnableBuffering();

        //        return next(context);
        //    });

        //    //autofac
        //    AutofacHelper.Container = app.ApplicationServices.GetAutofacRoot();
        //    //Swagger UI
        //    app.UseSwaggerMiddleware(() =>
        //        GetType().GetTypeInfo().Assembly.GetManifestResourceStream("ApeVolo.Api.index.html"));
        //    // CORS跨域
        //    app.UseCors(AppSettings.GetValue("Cors", "PolicyName"));
        //    //静态文件
        //    app.UseStaticFiles();
        //    //cookie
        //    app.UseCookiePolicy();
        //    //错误页
        //    app.UseStatusCodePages();
        //    app.UseRouting();

        //    app.UseCors("IpPolicy");
        //    // 认证
        //    app.UseAuthentication();
        //    // 授权
        //    app.UseAuthorization();
        //    //性能监控
        //    app.UseMiniProfilerMiddleware();
        //    app.UseEndpoints(endpoints =>
        //    {
        //        endpoints.MapControllerRoute(
        //            name: "default",
        //            pattern: "{controller=Home}/{action=Index}/{id?}");
        //    });
        //    app.UseHttpMethodOverride();

        //    app.UseDataSeederMiddleware(myContext);
        //    //作业调度
        //    app.UseQuartzNetJobMiddleware(quartzNetService, schedulerCenter);

        //    //雪花ID器
        //    new IdHelperBootstrapper().SetWorkderId(1).Boot();
        //    //事件总线配置订阅
        //    app.ConfigureEventBus();
        //    //List<object> items = new List<object>();
        //    //foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        //    //{
        //    //    items.Add(assembly);
        //    //}
        //}


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
                    RUINORERP.Common.Log4Net.Logger.Warn("未找到需要注册的RUINORERP相关DLL文件");

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
                        RUINORERP.Common.Log4Net.Logger.Error($"加载程序集 {Path.GetFileName(file)} 失败", ex);
                        // 继续处理其他程序集，不中断整体注册流程
                        continue;
                    }
                }

                // 去重，避免重复注册
                implementTypes = implementTypes.Distinct().ToList();
                interfaceTypes = interfaceTypes.Distinct().ToList();

                RUINORERP.Common.Log4Net.Logger.Info($"共发现 {implementTypes.Count} 个实现类和 {interfaceTypes.Count} 个接口需要注册");

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
                            RUINORERP.Common.Log4Net.Logger.Warn($"警告: 类 {0} 实现了IDependency接口但未实现任何已发现的具体接口{implementType.FullName}");
                            continue;
                        }

                        foreach (var interfaceType in implementedInterfaces)
                        {
                            // 根据不同的接口类型选择不同的生命周期
                            if (typeof(IDependencyService).IsAssignableFrom(implementType))
                            {
                                services.AddScoped(interfaceType, implementType);
                                RUINORERP.Common.Log4Net.Logger.Debug($"已注册 Scoped 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                            else if (typeof(IDependencyRepository).IsAssignableFrom(implementType))
                            {
                                services.AddSingleton(interfaceType, implementType);
                                RUINORERP.Common.Log4Net.Logger.Debug($"已注册 Singleton 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                            else
                            {
                                services.AddTransient(interfaceType, implementType);
                                RUINORERP.Common.Log4Net.Logger.Debug($"已注册 Transient 服务: {interfaceType.Name} -> {implementType.Name}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RUINORERP.Common.Log4Net.Logger.Error($"注册服务 {implementType.FullName} 失败", ex);
                        // 继续处理其他服务，不中断整体注册流程
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                RUINORERP.Common.Log4Net.Logger.Error("批量注册服务过程中发生严重错误", ex);
            }
            #endregion
            return services;
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
            try
            {
                // 检查容器是否初始化
                if (AutofacContainerScope == null)
                {
                    RUINORERP.Common.Log4Net.Logger.Warn($"警告: AutofacContainerScope尚未初始化，无法解析服务 {typeof(T).FullName}");
                    return default(T);
                }

                // 记录服务解析日志
                RUINORERP.Common.Log4Net.Logger.Debug($"正在从Autofac容器中解析服务: {typeof(T).FullName}");
                T service = AutofacContainerScope.Resolve<T>();
                RUINORERP.Common.Log4Net.Logger.Debug($"成功解析服务: {typeof(T).FullName}");
                return service;
            }
            catch (Exception ex)
            {
                RUINORERP.Common.Log4Net.Logger.Error($"解析服务失败 {typeof(T).FullName}", ex);
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
                    RUINORERP.Common.Log4Net.Logger.Warn("警告: className参数为空，无法按名称解析服务");
                    return default(T);
                }

                // 检查容器是否初始化
                if (AutofacContainerScope == null)
                {
                    RUINORERP.Common.Log4Net.Logger.Warn($"警告: AutofacContainerScope尚未初始化，无法按名称解析服务 {className}");
                    return default(T);
                }

                // 记录服务解析日志
                RUINORERP.Common.Log4Net.Logger.Debug($"正在从Autofac容器中按名称解析服务: {typeof(T).FullName}, 名称: {className}");
                T service = AutofacContainerScope.ResolveNamed<T>(className);
                RUINORERP.Common.Log4Net.Logger.Debug($"成功按名称解析服务: {typeof(T).FullName}, 名称: {className}");
                return service;
            }
            catch (Exception ex)
            {
                RUINORERP.Common.Log4Net.Logger.Error($"按名称解析服务失败 {typeof(T).FullName}, 名称: {className}", ex);
                // 错误处理：返回默认值而不是抛出异常，确保应用程序继续运行
                return default(T);
            }
        }
    }
}