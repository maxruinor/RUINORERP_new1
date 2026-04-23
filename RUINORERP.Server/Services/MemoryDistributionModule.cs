using Autofac;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.Cache;
using RUINORERP.Server.Comm;
using RUINORERP.Server.Network.Interfaces.Services;
using RUINORERP.Server.Network.Services;
using RUINORERP.Server.SmartReminder;
using StackExchange.Redis;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存分布统计服务的Autofac模块
    /// 统一注册所有相关依赖和服务
    /// </summary>
    public class MemoryDistributionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册内存分布统计服务（单例，确保全局唯一实例）
            builder.RegisterType<MemoryDistributionService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired(); // 启用属性注入

            // 注意：MemoryDistributionService 的以下依赖已在其他地方注册：
            // - ILogger<MemoryDistributionService>: 由 Microsoft.Extensions.Logging 自动提供
            // - ISessionService: 在 NetworkServicesDependencyInjection 中注册
            // - ServerLockManager: 在 Startup.cs 中注册
            // - SmartReminderMonitor: 在 SmartReminderModule 中注册
            // - IEntityCacheManager: 在 BusinessDIConfig 中注册
            // - IStockCacheService: 在 Startup.cs 中注册
            // - IMemoryCache: 由 Microsoft.Extensions.Caching.Memory 自动提供
            // - IRedisCacheService: 在 Startup.cs 中注册
            // - ImageCacheServiceBase: 在 Startup.cs 中注册
            // - FileStorageMonitorService: 在 NetworkServicesDependencyInjection 中注册
            // - PerformanceDataStorageService: 在 Startup.cs 中注册
            // - CachedRuleEngineCenter: ⚠️ 需要确认是否已注册，如未注册则使用 RuleEngineCenter
            
            // 可选：如果需要通过接口访问，可以定义并注册接口
            // builder.RegisterType<MemoryDistributionService>()
            //     .As<IMemoryDistributionService>()
            //     .SingleInstance()
            //     .PropertiesAutowired();
        }
    }
}
