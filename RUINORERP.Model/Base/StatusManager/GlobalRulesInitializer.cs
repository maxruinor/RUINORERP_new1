/**
 * 文件: GlobalRulesInitializer.cs
 * 版本: V1.0 - 全局规则初始化器
 * 说明: 提供全局状态规则管理器的初始化扩展方法
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 功能: 
 * 1. 提供应用启动时初始化全局规则的扩展方法
 * 2. 确保规则只初始化一次
 * 3. 提供清晰的初始化接口
 * 
 * 使用方法:
 * // 在应用启动时调用
 * GlobalRulesInitializer.InitializeGlobalRules();
 * 
 * // 或者使用依赖注入扩展方法
 * services.AddGlobalStateRules();
 */

using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 全局规则初始化器
    /// 提供全局状态规则管理器的初始化扩展方法
    /// </summary>
    public static class GlobalRulesInitializer
    {
        /// <summary>
        /// 初始化全局状态规则
        /// 确保规则只初始化一次，通常在应用启动时调用
        /// </summary>
        public static void InitializeGlobalRules()
        {
            try
            {
                // 获取全局规则管理器实例并初始化规则
                var rulesManager = GlobalStateRulesManager.Instance;
                rulesManager.InitializeAllRules();
            }
            catch (Exception)
            {
                // 忽略异常，避免应用启动失败
            }
        }

        /// <summary>
        /// 初始化全局状态规则（带服务提供者参数）
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        public static void InitializeGlobalRules(IServiceProvider serviceProvider)
        {
            try
            {
                // 获取全局规则管理器实例并初始化规则
                var rulesManager = GlobalStateRulesManager.Instance;
                rulesManager.InitializeAllRules();
            }
            catch (Exception)
            {
                // 忽略异常，避免应用启动失败
            }
        }

 

        /// <summary>
        /// 检查全局规则是否已初始化
        /// </summary>
        /// <returns>是否已初始化</returns>
        public static bool IsGlobalRulesInitialized()
        {
            return GlobalStateRulesManager.Instance.IsInitialized;
        }
    }


    /// <summary>
    /// Autofac ContainerBuilder扩展方法
    /// 提供Autofac容器的扩展方法
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// 添加全局状态规则服务
        /// 在Autofac容器中注册全局状态规则管理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        /// <returns>容器构建器</returns>
        public static Autofac.ContainerBuilder AddGlobalStateRules(this Autofac.ContainerBuilder builder)
        {
            // 注册全局规则管理器实例 - 使用Register方法而不是RegisterInstance
            builder.Register(c => GlobalStateRulesManager.Instance).SingleInstance();

            // 确保规则已初始化
            GlobalRulesInitializer.InitializeGlobalRules();

            return builder;
        }

        /// <summary>
        /// 添加状态管理服务（包括全局规则）
        /// 整合原有的状态管理服务和新的全局规则管理器
        /// </summary>
        /// <param name="builder">容器构建器</param>
        /// <returns>容器构建器</returns>
        public static Autofac.ContainerBuilder AddStateManagerWithGlobalRules(this Autofac.ContainerBuilder builder)
        {
            // 注册状态管理服务
            builder.RegisterType<StatusCacheManager>().SingleInstance();
            builder.RegisterType<UnifiedStateManager>()
                   .As<IUnifiedStateManager>()
                   .SingleInstance();

            // 注册全局状态规则管理器
            builder.AddGlobalStateRules();

            return builder;
        }
    }
}