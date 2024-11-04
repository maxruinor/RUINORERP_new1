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

namespace RUINORERP.Server
{
    public class Startup
    {

        /// <summary>
        /// 没使用用csla时使用
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


        public Startup()
        {

        }


        void cb(HostBuilderContext bc, ContainerBuilder builder)
        {

            #region  注册
            Services = new ServiceCollection();

        

            //注册当前程序集的所有类成员
            //builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            //    .AsImplementedInterfaces().AsSelf();
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
            .AsImplementedInterfaces()
            .AsSelf()
            .SingleInstance();
            //覆盖上面自动注册的？说是最后的才是
            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
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
                                                      //builder.RegisterType<Person>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
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

            ConfigureServices(Services);

            string conn = AppSettings.GetValue("ConnectString");
            Program.InitAppcontextValue(Program.AppContextData);
            Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
          //引用的long4net.dll要版本一样。
                logBuilder.AddProvider(new Log4NetProviderByCustomeDb("Log4net_db.config", conn, Program.AppContextData));
                logBuilder.AddLog4Net();
            });



            //覆盖上面自动dll批量注入的方法，因为要用单例模式
            builder.RegisterType(typeof(WorkflowRegisterService))
                 .AsImplementedInterfaces() //加上这一行，会出错
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()//打开AOP类的虚方法注入
                 .PropertiesAutowired()//指定属性注入
                 .SingleInstance();

            ////覆盖上面自动dll批量注入的方法，因为要用单例模式
            builder.RegisterType(typeof(RUINORERP.Business.CommService.BillConverterFactory))
                 .AsImplementedInterfaces() //加上这一行，会出错
                 .EnableInterfaceInterceptors()
                 .EnableClassInterceptors()//打开AOP类的虚方法注入
                 .PropertiesAutowired()//指定属性注入
                 .SingleInstance();
            //_containerBuilder = builder;
            //AutoFacContainer = builder.Build();
            #endregion

            //ILifetimeScope autofacRoot;//= app.ApplicationServices.GetAutofacRoot();


            // var repository = autofacRoot.Resolve<>();
            //public interface IAutofacConfiguration
            //{
            //    ILifetimeScope LifetimeScope { get; }
            //    ILifetimeScope RegisterTypes(Action<ContainerBuilder> configurationAction);
            //}

            //后面需要研究
            builder.Populate(Services);//将自带的也注入到autofac
        }

        /* 先注释掉，以后再研究
        public IServer GetMultipleServerHost(IServiceCollection _services)
        {
            var server = MultipleServerHostBuilder.Create()
                .ConfigureAppConfiguration((hostCtx, configApp) =>
                {
                    configApp.Sources.Clear();
                    configApp.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "serverOptions:name", "ServiceforBiz" },
                        { "serverOptions:listeners:0:ip", "Any" },
                        { "serverOptions:listeners:0:port", "3001" }
                    });
                })
                .AddServer<ServiceforBiz<BizPackageInfo>, BizPackageInfo, BizPipelineFilter>(builder =>
                {
                    builder
                           .UseCommand(commandOptions =>
                           {
                               commandOptions.AddCommand<BizCommand>();
                               // commandOptions.AddCommand<XTCommand>();
                           })
                      .UsePackageDecoder<MyPackageDecoder>()//注册自定义解包器
                     .UseSession<SessionforBiz>()
                     //注册用于处理连接、关闭的Session处理器
                     .UseSessionHandler(async (session) =>
                     {
                         frmMain.Instance.sessionListBiz.TryAdd(session.SessionID, session as SessionforBiz);

                         frmMain.Instance.PrintInfoLog($"{DateTime.Now} [SessionforBiz-主要程序] Session connected: {session.RemoteEndPoint}");
                         await Task.Delay(0);
                     }, async (session, reason) =>
                     {
                         SessionforBiz sg = session as SessionforBiz;

                         frmMain.Instance.PrintInfoLog($"{DateTime.Now} [SessionforBiz-主要程序] Session {session.RemoteEndPoint} closed: {reason}");
                         frmMain.Instance.sessionListBiz.Remove(sg.SessionID, out sg);
                         //SessionListGame.Remove(session as SessionforBiz);
                         await Task.Delay(0);
                     })

                   .ConfigureServices((hostingContext, services) =>
                   {
                       services = _services;
                       services.AddSingleton<SessionforBiz>();
                       services.AddSingleton<BizPackageInfo>();
                       services.AddSingleton<BizPipelineFilter>();
                       services.AddSingleton<BizCommand>();
                       services.AddSingleton<XTCommand>();
                       //services.AddSingleton<IAsyncCommand>(XTCommand);
                   })
             
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.ClearProviders(); //去掉默认添加的日志提供程序
                        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                        // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                        // the defaults be overridden by the configuration.
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                        logging.AddDebug();
                        if (isWindows)
                        {
                            // Add the EventLogLoggerProvider on windows machines
                            //logging.AddEventLog();//这个写到了事件查看器中。没有必要
                            //logging.AddFile();
                            logging.AddLog4Net();
                        }
                    }).UseLog4Net()
                    ;
                }).BuildAsServer();
            return server;
        }
        */


        /// <summary>
        /// 使用csla有值
        /// </summary>
        public static ILifetimeScope AutofacContainerScope { get; set; }

        public IHost CslaDIPort()
        {
            var hostBuilder = new HostBuilder()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
           .ConfigureContainer<ContainerBuilder>(new Action<HostBuilderContext, ContainerBuilder>(cb))
            .ConfigureServices((context, services) =>
            {
                services.AddAutofac();

                //services.AddLogging(configure => configure.AddLog4Net());

                //services.AddTransient<Business.UseCsla.Itb_LocationTypeDal, Business.UseCsla.tb_LocationTypeDal>();
                //services.AddTransient<Csla.Runtime.IRuntimeInfo, Csla.Runtime.RuntimeInfo>();
                //services.AddTransient<tb_LocationType>();
                //services.AddTransient<Form2>();
                //ConfigureServices(services);
                // register other services here
                //AutofacContainerScope = services.BuildServiceProvider(false).GetAutofacRoot();
                //AutofacContainerScope=services.BuildServiceProvider().CreateScope().ServiceProvider.GetAutofacRoot();
                //.Add<ServiceA>("key1")
                //.Add<ServiceB>("key2")
                //.Add<ServiceC>("key3")
                //.Build();
                //测试服务 
                //services.AddHostedService<DemoService>();

            }).Build();

            return hostBuilder;
        }


        #region 注入窗体-开始


        private static void RegisterForm(ContainerBuilder _builder)
        {

            /*
            System.Reflection.Assembly Assemblyobj = System.Reflection.Assembly.GetExecutingAssembly();
            Type[]? types = Assemblyobj?.GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                foreach (Type type in types)
                {
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

                    if (attribute.Describe.Contains("期初"))
                    {

                    }


                    //// 域注入
                    //Services.AddScoped(type);
                    //  _builder.RegisterType(attribute.FormType.GetType()).Named(attribute.FormType.Name, typeof(UserControl))
                    //       .AsImplementedInterfaces().AsSelf();
                    // _builder.Register(c => new BI.UCLocation()).Named<UserControl>("UCLocation");



                }
            }
            */
        }

        #endregion 注入窗体-结束
        /// <summary>
        /// 自带框架注入
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            //setup dependency injection
            //services.AddLogging();

            //services.AddLogging(logBuilder =>
            //{
            //    logBuilder.ClearProviders();
            //    logBuilder.AddLog4Net("ConfigFile/log4net.config", true);
            //});

            services.AddSingleton(typeof(frmMain));//MDI最大。才开一次才能单例
                                                   // services.AddSingleton(new AppSettings(WebHostEnvironment));
                                                   //services.AddScoped<ICurrentUser, CurrentUser>();
                                                   //services.AddSingleton(Configuration);
                                                   //services.AddLogging();

            #region 日志

            //services.AddLogging(logBuilder =>
            //{
            //    logBuilder.ClearProviders();
            //    //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
            //    logBuilder.AddLog4Net("ConfigFile/log4net.Config");//这里作用于工作流，配置文件都有，因为调试中看到 
            //    //logBuilder.AddLog4Net("log4net.config",true);
            //});



            //这是新增加工作流的服务
            services.AddWorkflowCoreServicesNew();




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
            services.AddTransient<IUnitOfWorkManage, UnitOfWorkManage>(); // 注入工作单元


            // services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            // services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            //Add Memory Cache
            services.AddOptions();
            services.AddMemoryCache();
            services.AddMemoryCacheSetup();
            services.AddDistributedMemoryCache();

            //services.AddRedisCacheSetup();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
           // string conn = AppSettings.GetValue("ConnectString");
            ///services.AddSqlsugarSetup(configuration);
            services.AddSqlsugarSetup(Program.AppContextData, configuration);

            services.AddAppContext(Program.AppContextData);
            /* by watson 2023-12-26
            services.AddSingleton(typeof(AutoMapperConfig));
            IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddScoped<IMapper, Mapper>();
            services.AddSingleton<IMapper>(mapper);
            services.AddAutoMapperInstall();
            */
            services.AddSingleton<UserService>();
            //映射暂时注释掉


            //services.AddSingleton(typeof(RUINORERP.Business.AutoMapper.AutoMapperConfig));
            //IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();

            services.AddSingleton(typeof(Comm.AutoMapperConfig));
            IMapper mapper = Comm.AutoMapperConfig.RegisterMappings().CreateMapper();
            services.AddScoped<IMapper, Mapper>();
            services.AddSingleton<IMapper>(mapper);
            //services.AddAutoMapperSetup();

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

        public static void ConfigureContainerForDll(ContainerBuilder builder)
        {

            // builder.RegisterType(typeof(OptionsManager<>)).As(typeof(IOptions<>)).SingleInstance();
            //// builder.RegisterType(typeof(OptionsManager<>)).As(typeof(IOptionsSnapshot<>));
            // builder.RegisterType(typeof(OptionsMonitor<>)).As(typeof(IOptionsMonitor<>)).SingleInstance();
            // builder.RegisterType(typeof(OptionsMonitor<>)).As(typeof(IOptionsFactory<>)).SingleInstance();
            // builder.RegisterType(typeof(OptionsFactory<>)).As(typeof(IOptionsMonitorCache<>)).SingleInstance();
            //builder.RegisterType<MemoryCache>().As<IMemoryCache>().SingleInstance();


            //var dalAssemble_common = System.Reflection.Assembly.LoadFrom("RUINORERP.Common.dll");
            //builder.RegisterAssemblyTypes(dalAssemble_common)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入

            builder.RegisterType<Extensions.Filter.GlobalExceptionsFilter>();

            //var Assembleycsla = System.Reflection.Assembly.LoadFrom("Csla.dll");
            //builder.RegisterAssemblyTypes(Assembleycsla)
            //     .Where(x => x.GetConstructors().Length > 0)//需要有构造函数
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency(); //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //.PropertiesAutowired();//允许属性注入




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

            /*
            var dalAssembleforwf = System.Reflection.Assembly.LoadFrom("RUINORERP.WF.dll");
            builder.RegisterAssemblyTypes(dalAssembleforwf)
             .AsImplementedInterfaces().AsSelf()
             .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
             .PropertiesAutowired();//允许属性注入
            */

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


                if (tempTypes[i].Name.Contains("UseCsla"))
                {
                    IOCCslaTypes.Add(tempTypes[i]);

                }
                else
                {
                    IOCTypes.Add(tempTypes[i]);
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

                /*
                // 强制为自定义特性
                MenuAttribute? attribute = tempTypes[i].GetCustomAttribute(NoWantIOCAttr, false) as MenuAttribute;
                // 如果强制失败就进入下一个循环
                if (attribute == null)
                    continue;
                */



            }
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
            //模块化注入
            //builder.RegisterModule<DefaultModule>();
            //属性注入控制器
            // builder.RegisterType<AutoDIController>().PropertiesAutowired();
            ////属性注入
            //builder.RegisterType<TestService>().As<ITestService>().PropertiesAutowired();
            // builder.RegisterModule(new AutofacRegister());

            builder.RegisterModule(new AutofacRegister());
        }


        /// <summary>
        /// 要扫描的程序集名称
        /// 默认为[^Shop.Utils|^Shop.]多个使用|分隔
        /// </summary>
        public static string MatchAssemblies = "^RUINORERP.|^TETS.";



        public IServiceCollection BatchServiceRegister(IServiceCollection services)
        {
            #region 依赖注入
            //services.AddScoped<IUserService, UserService>();           
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
            var referencedAssemblies = getFiles.Select(Assembly.LoadFrom).ToList();  //.Select(o=> Assembly.LoadFrom(o))         
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


        public static T GetFromFac<T>()
        {

            // return ServiceProvider.GetService<T>();
            //use csla
            return AutofacContainerScope.Resolve<T>();
            //原来模式
            // return AutoFacContainer.Resolve<T>();
        }

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
