using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RUINORERP.Model.Context;
using SimpleHttp;
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
using WorkflowCore.Interface;
using RUINORERP.WF.BizOperation;

namespace RUINORERP.WebServerConsole
{
  static  class Program
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
                        AppContextData.ClientInfo.Version = arg;
                        // Console.WriteLine(arg);
                        //MessageBox.Show(arg);
                    }
                }

               
 
                

 


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
                      
                        // 执行其他初始化任务
                        myhost.Run(); // 现在开始监听请求


                    }
                    catch (Exception ex)
                    {
                        var s = ex.Message;
                       
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
           Console.WriteLine("工作流", exception.Message);
        }
        
    }
}

