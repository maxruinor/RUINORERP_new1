using SuperSocket;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using RUINORERP.Common.Log4Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using WorkflowCore.Interface;
using RUINORERP.Model.Context;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using RUINORERP.Server.Workflow;
using RUINORERP.Server.Workflow.Steps;
using log4net;
using log4net.Repository;
using log4net.Config;
using SuperSocket.Server;
using RUINORERP.Server.ServerSession;
using RUINORERP.Server.Commands;
using SuperSocket.Command;
using RUINORERP.Business;
using RUINORERP.Model;
using WorkflowCore.Services.DefinitionStorage;
using RUINORERP.Business.AutoMapper;
using SuperSocket.Server.Host;
using RUINORERP.Server.Comm;
using Mapster;
using RUINORERP.Server.SmartReminder;
using WorkflowCore.Services;
using Autofac;
using RUINORERP.Server.SmartReminder.Strategies.SafetyStockStrategies;

namespace RUINORERP.Server
{
    static class Program
    {

        public static IWorkflowHost WorkflowHost;

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


        static bool serviceStarted = false;
        /// <summary>
        ///  ��������
        /// </summary>
        static IServiceCollection Services { get; set; }
        /// <summary>
        /// ���������
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (SingleInstanceChecker.IsAlreadyRunning())
            {
                // �������д��ڲ��˳�
                BringExistingInstanceToFront();
                return;
            }
            try
            {
                // ������������
                StartServerUI();
            }
            finally
            {
                SingleInstanceChecker.Release();
            }
        }

        static async void StartServerUI()
        {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //log4netHelper��������ļ��Ĵ���
            //ILoggerRepository repository = LogManager.CreateRepository("erpServer");
            //XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            //Host..loggerRepository = repository;


#pragma warning disable CS0168 // �����˱���������δʹ�ù�
            try
            {

                #region ����csla  
                try
                {

                    //IMapper mapper = AutoMapperConfig.RegisterMappings().CreateMapper();
                    //tb_PurOrder purOrder= new tb_PurOrder();
                    //purOrder.PurOrderNo = "12312";
                    //var employeeDto = mapper.Map<tb_PurEntry>(purOrder);

                    Startup starter = new Startup();
                    IHost myhost = starter.CslaDIPort();
                    // IHostBuilder  myhost = starter.CslaDIPort();

                    IServiceProvider services = myhost.Services;

                    //https://github.com/autofac/Autofac.Extensions.DependencyInjection/releases
                    //�������ķ���Դ
                    Startup.ServiceProvider = services;
                    AppContextData.SetServiceProvider(services);
                    Startup.AutofacContainerScope = services.GetAutofacRoot();
                    AppContextData.SetAutofacContainerScope(Startup.AutofacContainerScope);
                    BusinessHelper.Instance.SetContext(AppContextData);

                    //Program.AppContextData.SetServiceProvider(services);
                    //Program.AppContextData.Status = "init";

                    #region  ��������������

                    var host = services.GetService<IWorkflowHost>();
                    host.OnStepError += Host_OnStepError;

                    //����jsonע�ᣬ���滹��һ����ͨ����������
                    // https://workflow-core.readthedocs.io/en/latest/json-yaml/
                    //var json = System.IO.File.ReadAllText("myflow.json");
                    //var loader = ServiceProvider.GetService<IDefinitionLoader>();
                    //loader.LoadDefinition(json, Deserializers.Json);
                    host.AddRegisterWorkflow();

                    //���ع���������
                    var loader = Startup.ServiceProvider.GetService<IDefinitionLoader>();
                    string jsonpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Workflow\\Json\\myflow.json");
                    var json = System.IO.File.ReadAllText(jsonpath);
                    loader.LoadDefinition(json, Deserializers.Json);


                    await SafetyStockWorkflowConfig.ScheduleDailySafetyStockCalculation(host);
                    await InventorySnapshotWorkflowConfig.ScheduleInventorySnapshot(host);

                    // ���host�����ˣ������ٴ���������û���жϷ���
                    if (!serviceStarted)
                    {
                        host.Start();
                        serviceStarted = true;
                    }
                    WorkflowHost = host;

                    // �������
                    //var reminderService = services.GetRequiredService<SmartReminderService_old>();
                    //Task.Run(() => reminderService.RunSystemAsync());


                    // ����workflow������
                    // host.StartWorkflow("HelloWorkflow", 1, data: null); //
                    //host.StartWorkflow("HelloWorkflow");//, 2, data: null, Ĭ�ϻ����ð汾�ߵ�

                    #endregion



                    Startup.AutofacContainerScope = services.GetAutofacRoot();

                    //ILogger<frmMain> logger = services.GetService<ILogger<frmMain>>();
                    //frmMain frmMain1 = new frmMain(logger);
                    //Application.Run(frmMain1);

                    var form1 = Startup.GetFromFac<frmMain>();
                    form1._ServiceProvider = services;
                    //starter.GetMultipleServerHost(Startup.Services).StartAsync();
                    Application.Run(form1);
                    //myhost.StartAsync();

                }
                catch (Exception ex)
                {
                    var s = ex.Message;
                    MessageBox.Show(s);
                    MessageBox.Show(ex.StackTrace);
                    Console.Write(ex.StackTrace);
                }

                // IHostBuilder ihostbuilder= starter.CslaDIPort();
                // ihostbuilder.Start();
                //ServiceProvider = Startup.ServiceProvider;
                //IServiceProvider services = myhost.Services;

                //var mainform = services.GetService<Form2>();

                // var mainform = Startup.GetFromFac<Form2>(); //��ȡ����Service1
                //var mainform = Startup.GetFromFac<MainForm>(); //��ȡ����Service1
                // Application.Run(mainform);




                #endregion


            }
            catch (Exception ex)
            {

            }
#pragma warning restore CS0168 // �����˱���������δʹ�ù�


            //Application.Run(new frmMain());
        }


        private static void CreatSocketServer(IHost host)
        {

            frmMain.Instance.PrintInfoLog("StartServer Thread Id =" + System.Threading.Thread.CurrentThread.ManagedThreadId);
            //var logger = new LoggerFactory().AddLog4Net().CreateLogger("logs");
            //logger.LogError($"{DateTime.Now} LogError ��־");

            var _host = MultipleServerHostBuilder.Create()


            //��½��
            .AddServer<ServiceforLander<LanderPackageInfo>, LanderPackageInfo, LanderCommandLinePipelineFilter>(builder =>
               {
                   builder.ConfigureServerOptions((ctx, config) =>
                   {
                       //��ȡ��������
                       return config.GetSection("ServiceforLander");
                   })
               .UseSession<SessionforLander>()
               //ע�����ڴ������ӡ��رյ�Session������
               .UseSessionHandler(async (session) =>
               {
                   // sessionListLander.Add(session as SessionforLander);
                   // PrintMsg($"{DateTime.Now} [SessionforLander] Session-��½�� connected: {session.RemoteEndPoint}");
                   await Task.Delay(0);
               }, async (session, reason) =>
               {
                   //sessionListLander.Remove(session as SessionforLander);
                   // PrintMsg($"{DateTime.Now} [SessionforLander] Session-��½�� {session.RemoteEndPoint} closed: {reason}");
                   await Task.Delay(0);
               })
            //.ConfigureServices((context, services) =>
            //{


            //})

            .UseCommand(commandOptions =>
            {
                commandOptions.AddCommand<BaseCommand>();
                commandOptions.AddCommand<getmsgCommand>();
                commandOptions.AddCommand<loginCommand>();
                commandOptions.AddCommand<LanderCommand>();
            });


               })


                   /*

                           //һ��
                           .AddServer<ServiceforBiz<BizPackageInfo>, BizPackageInfo, BizPipelineFilter>(builder =>
                           {
                               builder.ConfigureServerOptions((ctx, config) =>
                               {
                                   //��ȡ��������
                                   // ReSharper disable once ConvertToLambdaExpression
                                   return config.GetSection("ServiceforBiz");
                               })
                               .UsePackageDecoder<MyPackageDecoder>()//ע���Զ�������
                               .UseSession<SessionforBiz>()
                           //ע�����ڴ������ӡ��رյ�Session������
                           .UseSessionHandler(async (session) =>
                           {
                               sessionListBiz.TryAdd(session.SessionID, session as SessionforBiz);
                               PrintMsg($"{DateTime.Now} [SessionforBiz-��Ҫ����] Session connected: {session.RemoteEndPoint}");
                               await Task.Delay(0);
                           }, async (session, reason) =>
                           {
                               SessionforBiz sg = session as SessionforBiz;
                               //if (sg.player != null && sg.player.Online)
                               //{
                               //   // SephirothServer.CommandServer.RoleService.��ɫ�˳�(sg);
                               //}
                               PrintMsg($"{DateTime.Now} [SessionforBiz-��Ҫ����] Session {session.RemoteEndPoint} closed: {reason}");
                               sessionListBiz.Remove(sg.SessionID, out sg);
                               //SessionListGame.Remove(session as SessionforBiz);
                               await Task.Delay(0);
                           })
                           .ConfigureServices((context, services) =>
                           {
                               services = _services;
                               //services.AddSingleton<RoomService>();
                           })
                                   .UseCommand(commandOptions =>
                                   {
                                       commandOptions.AddCommand<BizCommand>();
                                       commandOptions.AddCommand<XTCommand>();
                                   });
                           })

                       .ConfigureLogging((hostingContext, logging) =>
                       {
                           logging.ClearProviders(); //ȥ��Ĭ����ӵ���־�ṩ����
                           var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
                           // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
                           // the defaults be overridden by the configuration.
                           if (isWindows)
                           {
                               // Default the EventLogLoggerProvider to warning or above
                               logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Information);
                           }
                           logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                           logging.AddConsole();
                           logging.AddDebug();
                           if (isWindows)
                           {
                               // Add the EventLogLoggerProvider on windows machines
                               //logging.AddEventLog();//���д�����¼��鿴���С�û�б�Ҫ
                               logging.AddFile();
                               //logging.AddLog4Net();
                           }
                       }).UseLog4Net()
                   */
                   .Build();

            //try
            //{
            //    await _host.RunAsync();
            //}
            //catch (Exception e)
            //{
            //    frmMain.Instance.PrintInfoLog("_host.RunAsync()" + e.Message);
            //    _logger.LogError("socket _host RunAsync", e.Message);
            //}

        }



        private static void BringExistingInstanceToFront()
        {
            Process current = Process.GetCurrentProcess();
            foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            {
                if (process.Id == current.Id) continue;
                SetForegroundWindow(process.MainWindowHandle);
                break;
            }
        }



        #region ��ֹ����������У����ظ�����ʱ������ǰ�Ľ���
        #region �ڽ����в����Ƿ��Ѿ���ʵ��������
        // ȷ������ֻ����һ��ʵ��
        public static Process RunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            string currentProcessPath = Assembly.GetExecutingAssembly().Location;
            currentProcessPath = Path.GetFullPath(currentProcessPath).Replace("/", "\\"); // �淶��·��

            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process process in processes)
            {
                if (process.Id == currentProcess.Id)
                    continue; // ������ǰ����

                try
                {
                    string processPath = process.MainModule.FileName;
                    processPath = Path.GetFullPath(processPath).Replace("/", "\\");

                    // �����ִ�Сд�Ƚ�·��
                    if (currentProcessPath.Equals(processPath, StringComparison.OrdinalIgnoreCase))
                    {
                        return process; // �ҵ���ͬ·����ʵ��
                    }
                }
                catch (Exception)
                {
                    // ��Ȩ�޷��ʸý�����Ϣ������
                    continue;
                }
            }
            return null; // ������ʵ������
        }
        #endregion


        #region ����Win32API,�������Ѿ���һ��ʵ��������,�����䴰�ڲ���ʾ����ǰ��
        private static void HandleRunningInstance(Process instance)
        {
            //MessageBox.Show("�Ѿ�������!", "��ʾ��Ϣ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ShowWindowAsync(instance.MainWindowHandle, SW_SHOWNOMAL);//����API����,������ʾ����
            SetForegroundWindow(instance.MainWindowHandle);//�����ڷ�������ǰ��  
        }
        #endregion

        /// <summary>
        /// �ú��������ɲ�ͬ�̲߳����Ĵ��ڵ���ʾ״̬  
        /// </summary>  
        /// <param name="hWnd">���ھ��</param>  
        /// <param name="cmdShow">ָ�����������ʾ���鿴����ֵ�б�</param>  
        /// <returns>�������ԭ���ɼ�������ֵΪ���㣻�������ԭ�������أ�����ֵΪ��</returns>                      
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        private const int SW_SHOWNOMAL = 1;
        /// <summary>  
        ///  �ú���������ָ�����ڵ��߳����õ�ǰ̨�����Ҽ���ô���
        ///  ϵͳ������ǰ̨���ڵ��̷߳����Ȩ���Ը��������̡߳�
        /// </summary>  
        /// <param name="hWnd">�������������ǰ̨�Ĵ��ھ��</param>  
        /// <returns>�������������ǰ̨������ֵΪ���㣻�������δ������ǰ̨������ֵΪ��</returns>  
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion


        /// <summary>
        /// ֹͣ������
        /// </summary>
        private static void StopWorkflow()
        {
            var host = ServiceProvider.GetService<IWorkflowHost>();
            host.Stop();
            serviceStarted = false;
        }


        /// <summary>
        /// ��������һЩ��ʼֵ
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
            AppContextData.log.IP = "server";
            AppContextData.log.MachineName = System.Environment.MachineName + "-" + System.Environment.UserName;
            AppContextData.SysConfig = new tb_SystemConfig();

        }


        static List<StepError> UnhandledStepErrors = new List<StepError>();
        private static void Host_OnStepError(WorkflowCore.Models.WorkflowInstance workflow, WorkflowCore.Models.WorkflowStep step, Exception exception)
        {
            UnhandledStepErrors.Add(new StepError
            {
                Exception = exception,
                Step = step,
                Workflow = workflow
            });


            frmMain.Instance.PrintInfoLog(workflow.Id + step.Id + exception.Message);
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            frmMain.Instance.PrintInfoLog("Application_ThreadException:" + e.Exception.Message);
            frmMain.Instance.PrintInfoLog(e.Exception.StackTrace);
            //log4netHelper.fatal("ϵͳ��Application_ThreadException", e.Exception);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            string errorMsg = "An application error occurred. Please contact the adminstrator " +
                              "with the following information:\n\n";
            if (e.IsTerminating)
            {
                frmMain.Instance.PrintInfoLog("����쳣���³�����ֹ");
            }
            else
            {
                frmMain.Instance.PrintInfoLog("CurrentDomain_UnhandledException:" + errorMsg);
            }
        }
    }
}
