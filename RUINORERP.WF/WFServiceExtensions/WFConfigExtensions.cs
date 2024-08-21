using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using RUINORERP.Model;
using RUINORERP.WF.WFApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;


namespace RUINORERP.WF.WFServiceExtensions
{
    public static class WFConfigExtensions
    {

        /// <summary>
        /// 配置workflow
        /// </summary>
        /// <returns></returns>
        private static void AddWorkflowCoreServices(this IServiceCollection services)
        {
            services.AddLogging();
            //services.AddWorkflow();
            services.AddWorkflow(x => x.UseSqlServer(@"Server=.;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            //services.AddWorkflow(x => x.UseMongoDB(@"mongodb://localhost:27017", "workflow"));

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


        public static void AddRegisterWorkflow(this IWorkflowHost host)
        {

            host.Registry.GetDefinition("HelloWorkflow");
            // host.RegisterWorkflow<ActivityWorkflow, MyData>();
            host.RegisterWorkflow<ApprovalWorkflowTest, WFApproval.ApprovalWFData>();
            host.RegisterWorkflow<ApprovalWorkflow, tb_Stocktake>();
            // host.RegisterWorkflow<WorkWorkflow2, MyNameClass>();
            // host.RegisterWorkflow

        }
    }
}
