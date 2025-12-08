/**
 * 文件: StateManagerExtensions.cs
 * 版本: V3 - 状态管理器扩展方法
 * 说明: 状态管理器扩展方法 - 提供依赖注入注册和快捷操作方法
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 支持Microsoft DI和Autofac双容器注册
 * 公共代码: 状态管理扩展方法，所有版本通用
 */

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global;
using Autofac;

namespace RUINORERP.UI
{
    /// <summary>
    /// 状态管理器扩展方法类
    /// </summary>
    public static class StateManagerExtensions
    {
        /// <summary>
        /// 向依赖注入容器注册状态管理服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddStateManager(this IServiceCollection services)
        {
            // 注册简化的缓存管理器
            services.AddSingleton<SimpleCacheManager>();
            
            // 注册状态管理核心服务 - 使用内部实现
            services.AddSingleton<IUnifiedStateManager, UnifiedStateManager>();
            
            return services;
        }

        /// <summary>
        /// 向Autofac容器注册状态管理服务
        /// </summary>
        /// <param name="builder">容器构建器</param>
        /// <returns>容器构建器</returns>
        public static Autofac.ContainerBuilder AddStateManager(this Autofac.ContainerBuilder builder)
        {
            // 注册简化的缓存管理器
            builder.RegisterType<SimpleCacheManager>().SingleInstance();
            
            // 注册状态管理核心服务 - 使用内部实现
            builder.RegisterType<UnifiedStateManager>()
                   .As<IUnifiedStateManager>()
                   .SingleInstance();
                   
           
            
            return builder;
        }

         
    }
}