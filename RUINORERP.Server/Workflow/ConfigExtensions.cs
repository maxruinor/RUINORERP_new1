using Autofac.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Message;
using RUINORERP.Server.Workflow.Steps;
using RUINORERP.Server.Workflow.WFApproval;
using RUINORERP.Server.Workflow.WFPush;
using RUINORERP.Server.Workflow.WFReminder;
using RUINORERP.Server.Workflow.WFScheduled;
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
        #region 配置模型

        public enum WorkflowPersistenceType
        {
            Memory,
            SqlServer,
            SQLite
        }

        public class WorkflowCoreOptions
        {
            public WorkflowPersistenceType PersistenceType { get; set; } = WorkflowPersistenceType.Memory;
            public string ConnectionString { get; set; }
            public bool AutoCreateSchema { get; set; } = true;
            public bool AutoMigrateSchema { get; set; } = true;
        }

        #endregion

        #region 新的配置方法（推荐使用）

        public static void AddWorkflowCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection("WorkflowCore").Get<WorkflowCoreOptions>() ?? new WorkflowCoreOptions();

            ConfigureWorkflowPersistence(services, options);
            ConfigureWorkflowDSL(services);
            RegisterWorkflowSteps(services);
        }

        private static void ConfigureWorkflowPersistence(IServiceCollection services, WorkflowCoreOptions options)
        {
            try
            {
                switch (options.PersistenceType)
                {
                    case WorkflowPersistenceType.SqlServer:
                        ConfigureSqlServerPersistence(services, options);
                        break;

                    case WorkflowPersistenceType.SQLite:
                        ConfigureSQLitePersistence(services, options);
                        break;

                    case WorkflowPersistenceType.Memory:
                    default:
                        ConfigureMemoryPersistence(services);
                        break;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var logger = services.BuildServiceProvider().GetService<ILoggerFactory>()?.CreateLogger("RUINORERP.Server.Workflow.ConfigExtensions");
                    logger?.LogError(ex, "配置工作流持久化失败，降级到内存模式。类型: {PersistenceType}", options.PersistenceType);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine($"配置工作流持久化失败：{ex.GetType().Name} - {ex.Message}");
                }
                
                ConfigureMemoryPersistence(services);
            }
        }

        private static void ConfigureSqlServerPersistence(IServiceCollection services, WorkflowCoreOptions options)
        {
            var connStr = options.ConnectionString;
            if (string.IsNullOrWhiteSpace(connStr))
            {
                throw new InvalidOperationException("SQL Server 持久化模式需要配置 ConnectionString");
            }

            services.AddWorkflow(x => x.UseSqlServer(
                connStr,
                options.AutoCreateSchema,
                options.AutoMigrateSchema
            ));
        }

        private static void ConfigureSQLitePersistence(IServiceCollection services, WorkflowCoreOptions options)
        {
            var dbPath = options.ConnectionString ?? "workflow.db";

            services.AddWorkflow(x => x.UseSqlite(dbPath, options.AutoCreateSchema));
        }

        private static void ConfigureMemoryPersistence(IServiceCollection services)
        {
            services.AddWorkflow();
        }

        private static void ConfigureWorkflowDSL(IServiceCollection services)
        {
            services.AddWorkflowDSL();
        }

        private static void RegisterWorkflowSteps(IServiceCollection services)
        {
            services.AddTransient<StepError>();
            services.AddTransient<SayGoodbye>();
            services.AddTransient<SayHello>();
            services.AddTransient<PrintMessage>();
            services.AddTransient<HelloWorld>();
        }

        #endregion

        #region 旧的配置方法（已废弃，保留用于兼容性）

        [Obsolete("请使用 AddWorkflowCoreServices(services, configuration) 方法")]
        public static void AddWorkflowCoreServicesOld(this IServiceCollection services)
        {
            services.AddLogging();
            services.AddWorkflow(x => x.UseSqlServer(@"Server=.;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            services.AddWorkflowDSL();
        }

        [Obsolete("请使用 AddWorkflowCoreServices(services, configuration) 方法")]
        public static void AddWorkflowCoreServicesNew(this IServiceCollection services)
        {
            RegisterWorkflowSteps(services);
            services.AddWorkflow();
            services.AddWorkflowDSL();
        }

        #endregion

        public static void AddRegisterWorkflow(this IWorkflowHost host)
        {
            host.Registry.GetDefinition("HelloWorkflow");
            host.RegisterWorkflow<IfWorkflow, MyData>();
            host.RegisterWorkflow<ScheduledlWorkflow, ApprovalWFData>();
            host.RegisterWorkflow<NightlyWorkflow, GlobalScheduledData>();
            host.RegisterWorkflow<ApprovalWorkflow, ApprovalWFData>();
            host.RegisterWorkflow<DailyTaskWorkflow>();
            host.RegisterWorkflow<ReminderWorkflow, ReminderData>();
            host.RegisterWorkflow<WFPush.PushBaseInfoWorkflow, PushData>();
        }
    }
}
