using Autofac;
using RUINORERP.Server.SmartReminder.ReminderRuleStrategy;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkflowCore.Interface;
using WorkflowCore.Services;

namespace RUINORERP.Server.SmartReminder
{
    // Autofac配置模块 SmartReminderModule.cs
    public class SmartReminderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // WorkflowCore集成
            builder.RegisterType<WorkflowHost>().As<IWorkflowHost>().SingleInstance();
          //  builder.RegisterType<WorkflowReminderService>().AsSelf();

            // 核心服务
          //  builder.RegisterType<CachedRuleEngineCenter>().As<IRuleEngineCenter>();
            builder.RegisterType<SmartReminderMonitor>().As<ISmartReminderMonitor>();
            builder.RegisterType<NotificationService>().As<INotificationService>();

            // 策略注册
            builder.RegisterType<SafetyStockStrategy>().As<IReminderStrategy>();
            builder.RegisterType<SalesTrendStrategy>().As<IReminderStrategy>();

            // Redis分布式缓存
            builder.Register(context =>
                ConnectionMultiplexer.Connect("localhost").GetDatabase()
            ).As<IDatabase>();
        }
    }
}
