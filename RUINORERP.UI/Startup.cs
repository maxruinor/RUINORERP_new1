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


namespace RUINORERP.UI
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


        public Startup(bool tset)
        {
            //雪花ID器
            new IdHelperBootstrapper().SetWorkderId(1).Boot();
            //IdHelper.GetLongId(); 用这个取值
            //上面是自定义雪花ID
            //--------------------
            // 下面是SqlSugar自带雪花ID是成熟算法，正确配置WORKID无一例重复反馈，标题4也可以用自定义雪花算法
            //程序启时动执行一次就行
            SnowFlakeSingle.WorkId = 1; //从配置文件读取一定要不一样
            //服务器时间修改一定也要修改WorkId

            //即使没有这里的定义，基础数据主键 类型是long的都会自动添加
            //  IServiceProvider serviceProvider = new ServiceCollection().BuildServiceProvider();
        }



        /*
/// <summary>
/// 用于csla前的方法
/// </summary>
public Startup()
{
    //Configuration = configuration;
    //WebHostEnvironment = webHostEnvironment;
    // 创建服务容器
    Services = new ServiceCollection();
    //BatchServiceRegister(Services);
    ConfigureServices(Services);

    var builder = new ContainerBuilder();


    //注册当前程序集的所有类成员
    builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
        .AsImplementedInterfaces().AsSelf();

    //覆盖上面自动注册的？说是最后的才是
    //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();


    builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
    .AsImplementedInterfaces().AsSelf();

    ConfigureContainer(builder);

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
    builder.Populate(Services);//将自带的也注入到autofac
    AutoFacContainer = builder.Build();
}







/// <summary>
/// 用于csla更新后的注册
/// </summary>
public IHost CslaDIPortBackup()
{
    var hostBuilder = new HostBuilder()
     .ConfigureAppConfiguration((context, config) =>
     {
         var env = context.HostingEnvironment;
         config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
         config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
     })
     .UseServiceProviderFactory(new AutofacServiceProviderFactory())
     .ConfigureContainer<ContainerBuilder>(builder =>
     {
         #region  注册
         Services = new ServiceCollection();
         //BatchServiceRegister(Services);
         ConfigureServices(Services);
         //注册当前程序集的所有类成员
         builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
             .AsImplementedInterfaces().AsSelf();

         //覆盖上面自动注册的？说是最后的才是
         //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();

         builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
         .AsImplementedInterfaces().AsSelf();

         ConfigureContainer(builder);

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
         builder.Populate(Services);//将自带的也注入到autofac

         //AutoFacContainer = builder.Build();
         #endregion

     })
     .ConfigureServices((context, services) =>
    {
        services.AddAutofac();
        //services.AddCsla(options => options.AddWindowsForms());
        services.AddLogging(configure => configure.AddConsole());
        //services.AddTransient<Business.Csla.Itb_LocationTypeDal, Business.Csla.tb_LocationTypeDal>();
        //.AddTransient<tb_UnitEntity>()
        // register other services here

        //测试服务 
        //services.AddHostedService<DemoService>();
    }).Build();
    return hostBuilder;
}

*/

        /// <summary>
        /// 注册过程 2023-10-08
        /// </summary>
        /// <param name="bc"></param>
        /// <param name="builder"></param>
        void cb(HostBuilderContext bc, ContainerBuilder builder)
        {
            #region  注册
            Services = new ServiceCollection();
            //BatchServiceRegister(Services);
            #region 模仿csla 为了上下文
            //为了上下文
            // ApplicationContext defaults
            // RegisterContextManager(Services);
            // Services.AddSingleton<Context.ApplicationContext>(Program.AppContextData);
            #endregion

            ConfigureServices(Services);
           
            //注册当前程序集的所有类成员
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().AsSelf();

            //覆盖上面自动注册的？说是最后的才是
            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
            //如果注册为名称的，需要这样操作
            builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
            .AsImplementedInterfaces().AsSelf();
            //如果注册为名称的，需要这样操作
            //builder.RegisterType<tb_UnitController>().Named<BaseController>("tb_UnitController")
            //.AsImplementedInterfaces().AsSelf();
            //可能 这样是错的
            // builder.RegisterGeneric(typeof(tb_UnitController<>)).Named<BaseController<T>>("tb_UnitController1")
            //           .AsImplementedInterfaces().AsSelf();


            //builder.RegisterGeneric(typeof(tb_UnitController<>))
            //        .Named("named", typeof(BaseController<>))
            //        .AsImplementedInterfaces()
            //        .SingleInstance()
            //        .PropertiesAutowired();



            ConfigureContainer(builder);

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

            Program.InitAppcontextValue(Program.AppContextData);
            Services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                //logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
                logBuilder.AddProvider(new Log4NetProviderByCustomeDb("log4net.config", conn, Program.AppContextData));
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
        }

        /// <summary>
        /// 使用csla有值
        /// </summary>
        public static ILifetimeScope AutofacContainerScope { get; set; }

        public IHost CslaDIPort()
        {

            var hostBuilder = new HostBuilder()
         /*
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


      }

  })

         */
         .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        //使用了后面cb方法注册的
        .ConfigureContainer<ContainerBuilder>(new Action<HostBuilderContext, ContainerBuilder>(cb))
         /*
        .ConfigureContainer<ContainerBuilder>(builder =>
         {
             #region  注册
             // Services = new ServiceCollection();
             //BatchServiceRegister(Services);
             //ConfigureServices(Services);



             builder.RegisterType<RuntimeInfo>().As<IRuntimeInfo>().AsImplementedInterfaces().AsSelf();
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
             builder.RegisterType<BizCodeGenerationHelper>(); // 注册拦截器
                                                              // 注册依赖
             builder.RegisterType<BaseDataCacheAOP>(); // 注册拦截器
                                                       //builder.RegisterType<LogInterceptor>(); // 注册拦截器
                                                       //builder.RegisterType<Person>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
             builder.RegisterType<Person>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
                                                                                                                //builder.RegisterType<AOPDllTest.PersonDLL>().InterceptedBy(typeof(BaseDataCacheAOP)).EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
             builder.RegisterType<PersonBus>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截
             builder.RegisterType<tb_DepartmentController>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截

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
             //builder.Populate(Services);//将自带的也注入到autofac

             AutoFacContainer = builder.Build();

             //var container = containerBuilder.Build();
             new AutofacServiceProvider(AutoFacContainer);

             //   _containerBuilder = builder;
             #endregion

         })
         */
         .ConfigureServices((context, services) =>
         {
             services.AddAutofac();

             //services.AddCsla(options => options.AddWindowsForms());
             //services.AddCsla(); //ApplicationContext applicationContext
             //  services.AddCsla(options => options.AddWindowsForms().RegisterContextManager<>
             //  .DataPortal().PropertyChangedMode(Csla.ApplicationContext.PropertyChangedModes.Windows)
             //  );
             //services.AddLogging(configure => configure.AddConsole());

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
            System.Reflection.Assembly Assemblyobj = System.Reflection.Assembly.GetExecutingAssembly();
            Type[]? types = Assemblyobj?.GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(MenuAttrAssemblyInfo);
                foreach (Type type in types)
                {
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

                    if (attribute.Describe.Contains("盘点单查询"))
                    {

                    }
                    if (type.BaseType.IsGenericType)
                    {

                        if (type.BaseType.Name.Contains("BaseBillEditGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseBillEdit>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }

                        if (type.BaseType.Name.Contains("BaseEditGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<KryptonForm>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }

                        if (type.BaseType.Name.Contains("BaseListGeneric"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseUControl>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        if (type.BaseType.Name.Contains("BaseBillQueryMC"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseQuery>(type.Name)
                          .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        if (type.BaseType.Name.Contains("BaseNavigatorAnalysis") || type.BaseType.Name.Contains("BaseNavigatorPages") || type.BaseType.Name.Contains("BaseNavigatorGeneric") || type.BaseType.Name.Contains("BaseBillQueryGeneric") || type.BaseType.Name.Contains("BaseBillQueryMC") || type.BaseType.Name.Contains("BaseMasterQueryWithCondition"))
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






                        Type[] paraTypes = type.BaseType.GetGenericArguments();
                        if (paraTypes.Length > 0)
                        {
                            // info.EntityName = paraTypes[0].Name;
                        }



                    }
                    else
                    {
                        if (type.BaseType.Name.Contains("KryptonForm"))
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<KryptonForm>(type.Name)
                             .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }
                        else
                        {
                            _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                           .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入
                        }



                    }

                    //if (type.BaseType == typeof(BaseUControl) || type.BaseType.FullName.Contains("BaseList") || type.BaseType.FullName.Contains("BaseBillEdit"))
                    //{
                    //    _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<BaseUControl>(type.Name)
                    //        .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入

                    //}


                    //if (type.BaseType == typeof(UserControl) || type.BaseType.FullName.Contains("BaseList") || type.BaseType.FullName.Contains("BaseBillEdit"))
                    //{
                    //    _builder.Register(c => Assemblyobj.CreateInstance(type.FullName)).Named<UserControl>(type.Name)
                    //   .PropertiesAutowired(new CustPropertyAutowiredSelector());//指定属性注入

                    //}

                    if (type.Name == "frmSaleOrderQuery")
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



        #endregion 注入窗体-结束
        /// <summary>
        /// 自带框架注入 2023-10-08
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(MainForm_test));//MDI最大。才开一次才能单例
            services.AddSingleton(typeof(MainForm));//MDI最大。才开一次才能单例
                                                    // 注册工作流定义
            services.AddSingleton<IWorkflowRegistry, WorkflowRegistry>();
            services.AddSingleton<IWorkflowHost, WorkflowHost>();

            services.AddScoped(typeof(UI.BaseForm.frmBase));
            services.AddScoped(typeof(UI.BaseForm.BaseUControl));
            services.AddScoped(typeof(UI.BaseForm.BaseQuery));

            services.AddScoped(typeof(UserControl));
            services.AddScoped(typeof(BaseListWithTree));



            // services.AddSingleton(new AppSettings(WebHostEnvironment));
            //services.AddScoped<ICurrentUser, CurrentUser>();
            //services.AddSingleton(Configuration);
            //services.AddLogging();

            #region 日志


            //  new IdHelperBootstrapper().SetWorkderId(1).Boot();

            //为了修改为DB添加字段，覆盖这前面的



            services.AddWorkflow();

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
            services.AddSqlsugarSetup(Program.AppContextData, configuration);



            //  services.AddSingleton(typeof(AutoMapperConfig));
            // IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
            //services.AddScoped<IMapper, Mapper>();
            //services.AddSingleton<IMapper>(mapper);
            //services.AddAutoMapperInstall();

            //services.AddSingleton(IDefinitionLoader);

            services.AddSingleton(typeof(RUINORERP.Business.AutoMapper.AutoMapperConfig));
            IMapper mapper = RUINORERP.Business.AutoMapper.AutoMapperConfig.RegisterMappings().CreateMapper();
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

        public static void ConfigureContainer(ContainerBuilder builder)
        {
            //var dalAssemble_common = System.Reflection.Assembly.LoadFrom("RUINORERP.Common.dll");
            //builder.RegisterAssemblyTypes(dalAssemble_common)
            //      .AsImplementedInterfaces().AsSelf()
            //      .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //      .PropertiesAutowired();//允许属性注入

            builder.RegisterType<Extensions.Filter.GlobalExceptionsFilter>();


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
                    //BaseControllerGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
                }

                if (tempTypes[i].BaseType.Name.Contains("Validator") && tempTypes[i].BaseType.IsGenericType)
                {
                    //泛型名称有一个尾巴，这里处理掉，但是总体要保持不能同时拥有同名的 泛型 和非泛型控制类
                    //否则就是调用解析时用加小尾巴
                    ValidatorGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name.Replace("`1", ""), tempTypes[i]));
                    //BaseControllerGenericlist.Add(new KeyValuePair<string, Type>(tempTypes[i].Name, tempTypes[i]));
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

    /*
    //http://cn.voidcc.com/question/p-pkpebief-bv.html
    public class TableNameParameter : Parameter
    {
        public override Boolean CanSupplyValue(
         ParameterInfo pi, IComponentContext context, out Func<Object> valueProvider)
        {
            valueProvider = null;

            if (pi.ParameterType != typeof(String) && pi.Name != "tableName")
                return false;

            valueProvider = () =>
            {
                ITableNameResolver tableNameResolver = context.Resolve<ITableNameResolver>();
                Type entityType = pi.Member.DeclaringType.GetGenericArguments()[0];
                String tableName = tableNameResolver.GetTableName(entityType);
                return tableName;
            };
            return true;
        }
    }*/

}
