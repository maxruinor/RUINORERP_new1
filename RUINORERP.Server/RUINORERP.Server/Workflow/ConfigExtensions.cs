using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using RUINORERP.Model;
using RUINORERP.Model.TransModel;
using RUINORERP.Server.Workflow.Steps;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFPush;
using RUINORERP.Server.Workflow.WFReminder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;


namespace RUINORERP.Server.Workflow
{
    public static class ConfigExtensions
    {

        /// <summary>
        /// 配置workflow
        /// </summary>
        /// <returns></returns>
        public static void AddWorkflowCoreServices(this IServiceCollection services)
        {
            services.AddLogging();
            //services.AddWorkflow();
            services.AddWorkflow(x => x.UseSqlServer(@"Server=.;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));
            //services.AddWorkflow(x => x.UseMySQL(@"Server=127.0.0.1;Database=workflow;User=root;Password=password;", true, true));
            services.AddWorkflowDSL();

            // 这些个构造函数带参数的，需要添加到transient中
            // 可能没构造函数的 自动添加
            /*
            services.AddTransient<loopWork>();
            services.AddTransient<MyNameClass>();
            services.AddTransient<NextWorker>();
            services.AddTransient<worker>();
            services.AddTransient<WorkWorkflow>();
            services.AddTransient<WorkWorkflow2>();
            */
            // 这些个构造函数带参数的，需要添加到transient中
            // services.AddTransient<HelloWorld>();
            //  services.AddTransient<GoodbyeWorld>();
            // services.AddTransient<SleepStep>();

            var serviceProvider = services.BuildServiceProvider();

            //config logging
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new DebugLoggerProvider());

            // return serviceProvider;
        }

        public static void AddWorkflowCoreServicesNew(this IServiceCollection services)
        {

            // 这些个构造函数带参数的，需要添加到transient中
            services.AddTransient<StepError>();
            services.AddTransient<SayGoodbye>();
            services.AddTransient<SayHello>();
            services.AddTransient<PrintMessage>();
            services.AddTransient<HelloWorld>();
            //services.AddTransient<GetBaseInfo>();
            services.AddWorkflow();
            //services.AddWorkflow(x => x.UseSqlServer(@"Server=192.168.0.254;Database=Workflowdb;UID=sa;Password=SA!@#123sa;", true, true));

            //Json格式
            services.AddWorkflowDSL();
            //https://www.qubcedu.com/postdetail/3a14f314-b844-4092-2c0e-04a755d5ef76/1
            //services.AddWorkflowMiddleware


            //services.AddWorkflow(x => x.UseSqlite(@"Data Source=database2.db;", true));            
            // services.AddTransient<frmMain>();
            //services.AddSingleton(typeof(frmMain));//MDI最大。才开一次才能单例
            //var serviceProvider = services.BuildServiceProvider();
            //config logging
            //  var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            //loggerFactory.AddConsole(LogLevel.Debug);
            // return serviceProvider;
        }

        public static void AddRegisterWorkflow(this IWorkflowHost host)
        {
            host.Registry.GetDefinition("HelloWorkflow");
            host.RegisterWorkflow<IfWorkflow, MyData>();
            host.RegisterWorkflow<ScheduledlWorkflow, ApprovalWFData>();
            host.RegisterWorkflow<ApprovalWorkflow, ApprovalWFData>();

            host.RegisterWorkflow<ReminderWorkflow, ServerReminderData>();

            host.RegisterWorkflow<WFPush.PushBaseInfoWorkflow, PushData>();


        }
    }
}
