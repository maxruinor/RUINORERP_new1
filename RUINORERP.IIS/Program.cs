using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RUINORERP.Model.Context;
using RUINORERP.SimpleHttp;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using RUINORERP.IRepository.Base;
using RUINORERP.Repository.Base;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.Model;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions;
using RUINORERP.Business.AutoMapper;
using RUINORERP.Business;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using RUINORERP.Global.CustomAttribute;
using System;
using System.Threading;
using System.Windows.Forms;


namespace RUINORERP.IIS
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
        /// 服务管理者
        /// </summary>
        public static IServiceProvider MyServiceProvider { get; set; }

        //public static void Main()
        //{
        //    //            var host = Startup.CreateHostBuilder(args).Build();
        //    var host = Startup.CreateHost();

        //        MyServiceProvider = host.Services;
        //    Startup.AppContextData.SetServiceProvider(MyServiceProvider);
        //    Startup.AutofacContainerScope = MyServiceProvider.GetAutofacRoot();
        //    Startup.AppContextData.SetAutofacContainerScope(MyServiceProvider.GetAutofacRoot());
        //   // host.Start(); // 启动主机但不开始监听请求
        //                  // 执行其他初始化任务
        //    host.Run(); // 现在开始监听请求
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
                    Console.WriteLine("已经有一个实例在运行,不允许同时打开多个系统。");
                    return;
                }

                // 如果需要处理命令行参数，可以在这里进行
                // 例如，打印所有参数
                if (args.Length > 0)
                {
                    Console.WriteLine("接收到的命令行参数如下：");
                    foreach (var arg in args)
                    {
                       // AppContextData. = arg;
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

                

                        //myhost.Start(); // 启动主机但不开始监听请求
                        //WebServer webserver = Startup.GetFromFac<WebServer>();
                        //webserver.RunWebServer();
                        // 执行其他初始化任务
                        // myhost.Run(); // 现在开始监听请求

                        // var form1=Startup.ServiceProvider.GetService<MainForm>();
                        var form1 = Startup.GetFromFac<frmMain>();

                        //   MainForm form1 = new MainForm(services, aa);
                        Application.Run(form1);
                    }
                    catch (Exception ex)
                    {
                        var s = ex.Message;
                        MessageBox.Show(s);
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
            frmMain.Instance.logViewer1.AddLog(error.Message);
            frmMain.Instance._logger.LogError("出现应用程序未处理的异常,请更新到新版本，如果无法解决，请联系管理员！\r\n" + error.Message, error);
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
            frmMain.Instance.logViewer1.AddLog(str);
            frmMain.Instance._logger.LogError("出现应用程序未处理的异常2,请更新到新版本，如果无法解决，请联系管理员", error);
            MessageBox.Show(str, "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}

