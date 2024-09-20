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


namespace RUINORERP.UI
{
    static class Program
    {
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
        ///  服务容器
        /// </summary>
        //static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务管理者
        /// </summary>
        //public static IServiceProvider ServiceProvider { get; set; }
        /*
        static void Build()
        {
            // 创建服务容器
            Services = new ServiceCollection();
            #region 日志

            Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
            });

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

            //builder.RegisterType<BlogLogAOP>();//可以直接替换其他拦截器！一定要把拦截器进行注册



            ConfigureServices(Services);


            //Services.AddSingleton<IApiUserSession, ApiUserPrincipal>(); //CurrentPrincipal实现方式
            //Services.AddTransient<test>();
            // 添加服务注册
            //ConfigureServices(Services);

            //添加IApiUserSession实现类
            //Services.AddSingleton<IApiUserSession, ApiUserPrincipal>();

            //调用自定义的服务注册
            ConfigureRepository(Services);


            // 创建服务管理者
            ServiceProvider = Services.BuildServiceProvider();

            Services.AddSingleton(ServiceProvider);//注册到服务集合中,需要可以在Service中构造函数中注入使用
        }
        */
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

        /// 
        /// 配置依赖注入对象
        /// 
        /// 
        //       public static void ConfigureRepository1(IServiceCollection services)
        //       {
        //           #region 自动注入对应的服务接口
        //           //services.AddSingleton();//services.AddScoped();

        //           var baseType = typeof(**IDependency * *);
        //           var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        //           var getFiles = Directory.GetFiles(path, "*.dll").Where(Match);  //.Where(o=>o.Match())
        //           var referencedAssemblies = getFiles.Select(Assembly.LoadFrom).ToList();  //.Select(o=> Assembly.LoadFrom(o)) 

        //           var ss = referencedAssemblies.SelectMany(o => o.GetTypes());

        //           var types = referencedAssemblies
        //           .SelectMany(a => a.DefinedTypes)
        //           .Select(type => type.AsType())
        //           .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();
        //           var implementTypes = types.Where(x => x.IsClass).ToList();
        //           var interfaceTypes = types.Where(x => x.IsInterface).ToList();
        //           foreach (var implementType in implementTypes)
        //           {
        //               if (typeof(**IScopedDependency * *).IsAssignableFrom(implementType))
        //{
        //                   var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
        //                   if (interfaceType != null)
        //                       services.AddScoped(interfaceType, implementType);
        //               }
        //else if (typeof(**ISingletonDependency * *).IsAssignableFrom(implementType))
        //{
        //                   var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
        //                   if (interfaceType != null)
        //                       services.AddSingleton(interfaceType, implementType);
        //               }
        //else
        //               {
        //                   var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
        //                   if (interfaceType != null)
        //                       services.AddTransient(interfaceType, implementType);
        //               }
        //           }
        //           #endregion
        //       }

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
            // IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();

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
            services.AddSingleton(typeof(MainForm_test));
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

        ///// <summary>
        ///// var obj = await App.GetDataPortal<personedit>.FetchAsync(42);
        ///// </summary>
        ///// <typeparam name="t"></typeparam>
        ///// <returns></returns>
        //public static IDataPortal<t> DataPortal<t>()
        //{
        //    return AppContext.GetRequiredService<IDataPortal<t>>();
        //}





        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // 创建一个命名 Mutex
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "Global\\" + Assembly.GetExecutingAssembly().GetName().Name, out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("已经有一个实例在运行,不允许同时打开多个系统。", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 如果需要处理命令行参数，可以在这里进行
                // 例如，打印所有参数
                if (args.Length > 0)
                {
                    Console.WriteLine("接收到的命令行参数如下：");
                    foreach (var arg in args)
                    {
                        AppContextData.ClientInfo.Version = arg;
                        // Console.WriteLine(arg);
                        //MessageBox.Show(arg);
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
                //Application.EnableVisualStyles();可能会让其他电脑布局有问题？？
                Application.SetCompatibleTextRenderingDefault(false);
                //公共类中的 先要执行
                //  ILoggerRepository repository = LogManager.CreateRepository("erpComm");
                //  XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
                //  Log4NetRepository.loggerRepository = repository;

                //Application.Run(new Form2());
                //return;
                #region csla前
                /*
                Startup starter = new Startup();
                ServiceProvider = Startup.ServiceProvider;
                var mainform = Startup.GetFromFac<MainForm>(); //获取服务Service1
                Application.Run(mainform);
                */


                #endregion


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


                ///=====----

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


                        // var form1=Startup.ServiceProvider.GetService<MainForm>();
                        var form1 = Startup.GetFromFac<MainForm>();


                        //   MainForm form1 = new MainForm(services, aa);
                        Application.Run(form1);
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






                /*
                using (MainForm f1 = ServiceProvider.GetRequiredService<MainForm>())
                {
                    // f1.ShowDialog();
                    //Application.Run(new Form1());
                    frmBase.BaseServiceProvider = ServiceProvider;
                    var db = ServiceProvider.GetRequiredService<SqlSugar.ISqlSugarClient>();
                    var list = db.SqlQueryable<Model.tb_Unit>("select * from tb_Unit").OrderBy("id asc");
                    MessageBox.Show(list.Count().ToString());
                    Application.Run(f1);
                }
    */

                /*
                 * ok
                MainForm _mainform = new MainForm();
                var sqlSugarScope = new SqlSugar.SqlSugarScope(new SqlSugar.ConnectionConfig
                {
                    ConnectionString = "Server=192.168.0.250;Database=erp;UID=sa;Password=sa",
                    DbType = SqlSugar.DbType.SqlServer,
                    IsAutoCloseConnection = true,
                });



                // _logFactory = new LoggerFactory();
                var loggerFactory = (ILoggerFactory)new LoggerFactory();
                loggerFactory.AddProvider(new Log4NetProvider("log4net.config"));
                var logger = loggerFactory.CreateLogger<Repository.UnitOfWorks.UnitOfWorkManage>();
                IRepository.Base.IBaseRepository<tb_Supplier> rr = new Repository.Base.BaseRepository<tb_Supplier>(new Repository.UnitOfWorks.UnitOfWorkManage(sqlSugarScope, logger));
                IMapper mapper = Model.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
               IServices.ISupplierServices us = new Services.SupplierServices(mapper, rr);

               // var mylist = us.QueryTest();
                Model.tb_Supplier dto = new Model.tb_Supplier();
                dto.ID = 1;
                dto.Name = "dt0";
                // us.SaveRole(dto);
                //var list = sqlSugarScope.SqlQueryable<Model.tb_Unit>("select * from tb_Unit").OrderBy("id asc");
                //MessageBox.Show(mylist.ToString());
                Application.Run(_mainform);

                */

            }
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
            AppContextData.log.IP = HLH.Lib.Net.IpAddressHelper.GetLocIP();
            AppContextData.SysConfig = new tb_SystemConfig();
        }

        private static void Host_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            //MainForm.Instance.logger.LogError("Host_OnStepError", exception);
            MainForm.Instance.uclog.AddLog("工作流", exception.Message);
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = "";
            string strDateInfo = "\r\n\r\n出现应用程序未处理的异常,请更新到最新版本，如果无法解决，请联系管理员!" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n{2}\r\n",
                error.GetType().Name, error.Message, error.StackTrace);
            }
            else
            {
                str = string.Format("应用程序线程错误:{0}", e);
            }
            MainForm.Instance.uclog.AddLog("线程", str);
            MainForm.Instance.logger.LogError("出现应用程序未处理的异常,请更新到新版本，如果无法解决，请联系管理员！\r\n" + error.Message, error);
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
            MainForm.Instance.uclog.AddLog("应用域", str);
            MainForm.Instance.logger.LogError("出现应用程序未处理的异常2,请更新到新版本，如果无法解决，请联系管理员", error);
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

