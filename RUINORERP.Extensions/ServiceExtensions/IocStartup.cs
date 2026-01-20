using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions;
using Microsoft.Extensions.Logging;
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


namespace RUINORERP.Extensions
{
    public class IocStartup
    {
        public static IContainer AutoFacContainer;
        /// <summary>
        ///  服务容器
        /// </summary>
        public static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务管理者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        //private IConfiguration Configuration { get; }
        //public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        //{
        //    //Configuration = configuration;
        //    //WebHostEnvironment = webHostEnvironment;
        //    var builder = new ContainerBuilder();
        //    ConfigureContainer(builder);
        //    AutoFacContainer = builder.Build();
        //}
        public IocStartup(ServiceCollection _services, ContainerBuilder _builder)
        {
            //Configuration = configuration;
            //WebHostEnvironment = webHostEnvironment;
            // 创建服务容器
            //Services = new ServiceCollection();
            //var builder = new ContainerBuilder();


            //BatchServiceRegister(Services);
            ConfigureServices(_services);



            //注册当前程序集的所有类成员
            _builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().AsSelf();

            //覆盖上面自动注册的？说是最后的才是
            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
           // builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
            //.AsImplementedInterfaces().AsSelf();

            // 注册依赖
            //builder.RegisterType<LogInterceptor>(); // 注册拦截器
           // builder.RegisterType<Person>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
            ConfigureContainer(_builder);
            _builder.Populate(_services);//将自带的也注入到autofac
            AutoFacContainer = _builder.Build();
        }


        /// <summary>
        /// 自带框架注入
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton(typeof(MainForm_test));//MDI最大。才开一次才能单例
            // services.AddSingleton(new AppSettings(WebHostEnvironment));
            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton(Configuration);
            //services.AddLogging();
            #region 日志

   

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
            services.AddSingleton(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // 注入仓储
            services.AddSingleton<IUnitOfWorkManage, UnitOfWorkManage>(); // 注入工作单元


            // services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            // services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddMemoryCacheSetup();
            //services.AddRedisCacheSetup();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
            string conn = AppSettings.GetValue("ConnectString");
          //  services.AddSqlsugarSetup(configuration);
            //  services.AddDbSetup();
            //services.AddAutoMapper();
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

            // 创建服务管理者
            ServiceProvider = services.BuildServiceProvider();

            services.AddSingleton(ServiceProvider);//注册到服务集合中,需要可以在Service中构造函数中注入使用
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            //var dalAssemble_common = System.Reflection.Assembly.LoadFrom("RUINORERP.Common.dll");
            //builder.RegisterAssemblyTypes(dalAssemble_common)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入

            builder.RegisterType<Extensions.Filter.GlobalExceptionsFilter>();

            var dalAssemble_Extensions = AssemblyLoader.LoadAssembly("RUINORERP.Extensions");
            if (dalAssemble_Extensions != null)
            {
                builder.RegisterAssemblyTypes(dalAssemble_Extensions)
                      .AsImplementedInterfaces().AsSelf()
                      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                      .PropertiesAutowired();//允许属性注入
            }

            var dalAssemble = AssemblyLoader.LoadAssembly("RUINORERP.Model");
            if (dalAssemble != null)
            {
                builder.RegisterAssemblyTypes(dalAssemble)
                      .AsImplementedInterfaces().AsSelf()
                      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                      .PropertiesAutowired();//允许属性注入
            }


            // 获取所有待注入服务类
            //var dependencyService = typeof(IDependencyService);
            // var dependencyServiceArray = GetAllTypes(alldlls).ToArray();//  GlobalData.FxAllTypes
            //    .Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();
            var dalAssemble_Iservice = AssemblyLoader.LoadAssembly("RUINORERP.IServices");
            if (dalAssemble_Iservice != null)
            {
                builder.RegisterAssemblyTypes(dalAssemble_Iservice)
              .AsClosedTypesOf(typeof(IServices.BASE.IBaseServices<>));
            }


            // builder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerRequest();


            var dalAssemble_service = AssemblyLoader.LoadAssembly("RUINORERP.Services");
            if (dalAssemble_service != null)
            {
                builder.RegisterTypes(dalAssemble_service.GetTypes())
                    .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired()
                    .InstancePerDependency();
            }

            var dalAssemble_Business = AssemblyLoader.LoadAssembly("RUINORERP.Business");
            if (dalAssemble_Business != null)
            {
                builder.RegisterTypes(dalAssemble_Business.GetTypes())
                     .AsImplementedInterfaces().AsSelf()
                    .PropertiesAutowired()
                    .InstancePerDependency();
            }
            //.AsImplementedInterfaces()
            //.PropertiesAutowired()
            //.InstancePerDependency();



            //.EnableInterfaceInterceptors()
            // .InterceptedBy(cacheType.ToArray());

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
            builder.RegisterModule(new AutofacRegister());
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
            var referencedAssemblies = getFiles.Select(AssemblyLoader.LoadFromPath).Where(a => a != null).ToList();
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
            return AutoFacContainer.Resolve<T>();
        }
        public static object GetFromFacByName(string TypeName, Type t)
        {
            return AutoFacContainer.ResolveNamed(TypeName, t);
        }


    }
}
