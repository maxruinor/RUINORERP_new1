using Autofac;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 内存泄漏诊断服务的 Autofac 模块
    /// 统一注册内存泄漏诊断相关服务
    /// </summary>
    public class MemoryLeakDiagnosticsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 注册内存泄漏诊断服务（单例，确保全局唯一实例）
            builder.RegisterType<MemoryLeakDiagnosticsService>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired(); // 启用属性注入
            
            // 注意：MemoryLeakDiagnosticsService 的依赖已在其他地方注册：
            // - ILogger<MemoryLeakDiagnosticsService>: 由 Microsoft.Extensions.Logging 自动提供
            
            // 可选：如果需要通过接口访问，可以定义并注册接口
            // builder.RegisterType<MemoryLeakDiagnosticsService>()
            //     .As<IMemoryLeakDiagnosticsService>()
            //     .SingleInstance()
            //     .PropertiesAutowired();
        }
    }
}
