using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using RUINORERP.Common.Log4Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Common;
using System.Reflection;
using log4net;
using RUINORERP.Model;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using SqlSugar;
using RUINORERP.Common.DI;
using RUINORERP.Common.Global;
using Autofac.Extras.DynamicProxy;
using RUINORERP.IRepository.Base;
using RUINORERP.Extensions;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Repository.Base;
using RUINORERP.Common.Helper;

namespace RUINORERP.Assistant
{
    static class Program
    {

        /// <summary>
        ///  服务容器
        /// </summary>
        static IServiceCollection Services { get; set; }
        /// <summary>
        /// 服务管理者
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        public static ContainerBuilder builder { get; set; }


        #region 注入窗体-开始
        private static void RegisterForm()
        {
            Type[]? types = System.Reflection.Assembly.GetExecutingAssembly()?.GetExportedTypes();
            if (types != null)
            {
                var descType = typeof(FormMarkAttribute);
                var form = typeof(Form);
                foreach (Type type in types)
                {
                    // 类型是否为窗体，否则跳过，进入下一个循环
                    //if (type.GetTypeInfo != form)
                    //    continue;

                    // 是否为自定义特性，否则跳过，进入下一个循环
                    if (!type.IsDefined(descType, false))
                        continue;
                    // 强制为自定义特性
                    FormMarkAttribute? attribute = type.GetCustomAttribute(descType, false) as FormMarkAttribute;
                    // 如果强制失败或者不需要注入的窗体跳过，进入下一个循环
                    if (attribute == null || !attribute.IsIOC)
                        continue;
                    // 域注入
                    //Services.AddScoped(type);
                    Console.WriteLine($"注入：{attribute.FormType.Namespace}.{attribute.FormType.Name},{attribute.Describe}");
                }
            }
        }
        #endregion 注入窗体-结束


        public static IContainer AutoFacContainer;

        public static T GetFromFac<T>()
        {
            return AutoFacContainer.Resolve<T>();
        }
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //UnhandledException 处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //buildTypes();
            var _builder = new ContainerBuilder();
            var _Services = new ServiceCollection();
            //注册当前程序集的所有类成员
            _builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().AsSelf();
            IocStartup start = new IocStartup(_Services, _builder);
            AutoFacContainer = IocStartup.AutoFacContainer;



            var mainform = GetFromFac<MainForm>(); //获取服务Service1
            Application.Run(mainform);

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
            
            

            var _logFactory = new LoggerFactory();
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

        static void buildTypes()
        {
            var builder = new ContainerBuilder();
            // 创建服务容器
            var services = new ServiceCollection();
            //注册当前程序集的所有类成员
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces().AsSelf();

            #region 日志
            services.AddLogging(logBuilder =>
            {
                logBuilder.ClearProviders();
                logBuilder.AddProvider(new Log4NetProvider("log4net.config"));
            });
            #endregion

            #region db
            var sqlSugarScope = new SqlSugar.SqlSugarScope(new SqlSugar.ConnectionConfig
            {
                ConnectionString = "Server=192.168.0.250;Database=erp;UID=sa;Password=sa",
                DbType = SqlSugar.DbType.SqlServer,
                IsAutoCloseConnection = true,
            });
            // Services.AddSingleton<ISqlSugarClient>(sqlSugarScope);

            #endregion

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // 注入仓储
            services.AddTransient<IUnitOfWorkManage, UnitOfWorkManage>(); // 注入工作单元
            // services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            // services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.AddMemoryCacheSetup();
            //services.AddRedisCacheSetup();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            var cfgBuilder = configurationBuilder.AddJsonFile("appsettings.json");//默认读取：当前运行目录
            IConfiguration configuration = cfgBuilder.Build();
            AppSettings.Configuration = configuration;
            string conn = AppSettings.GetValue("ConnectString");
            services.AddSqlsugarSetup(configuration);
            //  services.AddDbSetup();
            //services.AddAutoMapperSetup();

            //覆盖上面自动注册的？说是最后的才是

            // var ss = Assembly.GetExecutingAssembly().GetReferencedAssemblies();



            var IServices = Assembly.Load("RUINORERP.IServices");
            var myServices = Assembly.Load("RUINORERP.Services");
            var IRepository = Assembly.Load("RUINORERP.Repository");
            //var Entitys = Assembly.Load("RUINORERP.Model");


            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(IServices, myServices)
              .Where(t => t.Name.EndsWith("Services"))
              .AsImplementedInterfaces();


            //根据名称约定（数据访问层的接口和实现均以Repository结尾），实现数据访问接口和数据访问实现的依赖
            //builder.RegisterAssemblyTypes(Entitys)
            //  .AsImplementedInterfaces();

            /*
            var businessLib = Assembly.Load("RUINORERP.Business");
            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            builder.RegisterAssemblyTypes(businessLib)
        //.AsImplementedInterfaces().AsSelf();
        .AsImplementedInterfaces()
        .PropertiesAutowired()
        .InstancePerDependency();
            // .EnableInterfaceInterceptors();
            */


            builder.RegisterModule(new AutofacRegister());


            // 获取所有待注入服务类
            var dependencyService = typeof(IDependencyService);
            var dependencyServiceArray = GlobalData.FxAllTypes
                .Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();
            builder.RegisterTypes(dependencyServiceArray)
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency()
                .EnableInterfaceInterceptors();
            //.InterceptedBy(cacheType.ToArray());


            // 获取所有待注入仓储类
            var dependencyRepository = typeof(IDependencyRepository);
            var dependencyRepositoryArray = GlobalData.FxAllTypes
                .Where(x => dependencyRepository.IsAssignableFrom(x) && x != dependencyRepository).ToArray();
            builder.RegisterTypes(dependencyRepositoryArray)
                .AsImplementedInterfaces()
                .InstancePerDependency();



            var dalAssemble = Assembly.LoadFrom("RUINORERP.Model.dll");
            builder.RegisterAssemblyTypes(dalAssemble)
                  .AsImplementedInterfaces().AsSelf()
                  .InstancePerDependency() //默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
                  .PropertiesAutowired();//允许属性注入
                                         // 获取所有待注入服务类

            //var dependencyService = typeof(IDependencyService);
            //var dependencyServiceArray = GlobalData.FxAllTypes
            //    .Where(x => dependencyService.IsAssignableFrom(x) && x != dependencyService).ToArray();
            //builder.RegisterTypes(dependencyServiceArray)
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired()
            //    .InstancePerDependency()
            //    .EnableInterfaceInterceptors()
            //    //.InterceptedBy(cacheType.ToArray());

            //builder.RegisterType<UserControl>().Named<UserControl>("MENU").InstancePerDependency();
            //builder.RegisterType<RUINORERP.UI.SS.MenuInit>().Named<UserControl>("MENU")
            //.AsImplementedInterfaces().AsSelf();

            // 注册依赖
            //builder.RegisterType<LogInterceptor>(); // 注册拦截器
            //builder.RegisterType<Person>().EnableClassInterceptors();  // 注册被拦截的类并启用类拦截

            // ConfigureContainer(builder);
            builder.Populate(services);//将自带的也注入到autofac

            AutoFacContainer = builder.Build();
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string str = "";
            string strDateInfo = "\r\n\r\n出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            Exception error = e.Exception as Exception;
            if (error != null)
            {
                string logInfo = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n", error.GetType().Name, error.Message, error.StackTrace);
                str = string.Format(strDateInfo + "异常类型：{0}\r\n异常消息：{1}\r\n",
                error.GetType().Name, error.Message);
            }
            else
            {
                str = string.Format("应用程序线程错误:{0}", e);
            }

            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string str = "";
            Exception error = e.ExceptionObject as Exception;
            string strDateInfo = "出现应用程序未处理的异常：" + DateTime.Now.ToString() + "\r\n";
            if (error != null)
            {
                string logInfo = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r堆栈信息:{1}", error.Message, error.StackTrace);
                str = string.Format(strDateInfo + "Application UnhandledException:{0};\n\r", error.Message);
            }
            else
            {
                str = string.Format("Application UnhandledError:{0}", e);
            }

            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
