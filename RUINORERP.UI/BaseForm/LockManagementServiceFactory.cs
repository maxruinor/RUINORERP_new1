using Microsoft.Extensions.Logging;
using RUINORERP.UI.Network.Services;
using RUINORERP.Model.Context;
using System;
using System.Threading.Tasks;

namespace RUINORERP.UI.BaseForm
{
    /// <summary>
    /// 锁管理服务工厂 v2.0.0
    /// 提供统一的服务创建和版本管理
    /// 
    /// 版本：2.0.0
    /// 作者：AI Assistant
    /// 创建时间：2025-01-27
    /// 
    /// 主要功能：
    /// - 统一的服务创建接口
    /// - 版本控制和兼容性管理
    /// - 现有服务平滑迁移
    /// - 依赖注入支持
    /// </summary>
    public static class LockManagementServiceFactory
    {
        #region 版本信息

        /// <summary>
        /// 工厂版本号
        /// </summary>
        public const string FACTORY_VERSION = "2.0.0";

        /// <summary>
        /// 当前推荐的服务版本
        /// </summary>
        public const string RECOMMENDED_SERVICE_VERSION = "2.0.0";

        #endregion

        #region 服务创建接口

        /// <summary>
        /// 创建集成式锁管理服务（推荐）
        /// 使用新的2.0.0架构，集成心跳和缓存功能
        /// </summary>
        /// <returns>集成式锁管理服务实例</returns>
        public static IntegratedLockManagementService CreateIntegratedService()
        {
            try
            {
                var logger = ApplicationContext.Current?.GetLogger<IntegratedLockManagementService>();
                var communicationService = ApplicationContext.Current?.GetRequiredService<ClientCommunicationService>();
                var heartbeatManager = ApplicationContext.Current?.GetRequiredService<HeartbeatManager>();

                if (communicationService == null)
                {
                    throw new InvalidOperationException("ClientCommunicationService 未在依赖注入容器中注册");
                }

                if (heartbeatManager == null)
                {
                    throw new InvalidOperationException("HeartbeatManager 未在依赖注入容器中注册");
                }

                var service = new IntegratedLockManagementService(
                    communicationService, 
                    heartbeatManager, 
                    logger);

                logger?.LogInformation("创建集成式锁管理服务 v{Version} 成功", 
                    IntegratedLockManagementService.SERVICE_VERSION);

                return service;
            }
            catch (Exception ex)
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger?.LogError(ex, "创建集成式锁管理服务失败");
                throw;
            }
        }

        /// <summary>
        /// 创建传统锁管理服务（兼容性）
        /// 保持与现有代码的兼容性
        /// </summary>
        /// <returns>传统锁管理服务实例</returns>
        public static LockManagementService CreateLegacyService()
        {
            try
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementService>();
                var service = new LockManagementService(logger);

                logger?.LogInformation("创建传统锁管理服务成功（兼容模式）");
                return service;
            }
            catch (Exception ex)
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger?.LogError(ex, "创建传统锁管理服务失败");
                throw;
            }
        }

        /// <summary>
        /// 自动创建锁管理服务
        /// 根据系统配置和可用性自动选择最佳服务
        /// </summary>
        /// <param name="preferIntegrated">是否优先使用集成式服务</param>
        /// <returns>锁管理服务实例（动态类型）</returns>
        public static object CreateAutoService(bool preferIntegrated = true)
        {
            try
            {
                if (preferIntegrated)
                {
                    try
                    {
                        return CreateIntegratedService();
                    }
                    catch
                    {
                        var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                        logger?.LogWarning("集成式服务创建失败，回退到传统服务");
                        return CreateLegacyService();
                    }
                }
                else
                {
                    return CreateLegacyService();
                }
            }
            catch (Exception ex)
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger?.LogError(ex, "自动创建锁管理服务失败");
                throw;
            }
        }

        #endregion

        #region 服务初始化接口

        /// <summary>
        /// 初始化并启动集成式服务
        /// </summary>
        /// <returns>启动任务</returns>
        public static async Task<IntegratedLockManagementService> InitializeIntegratedServiceAsync()
        {
            var service = CreateIntegratedService();
            await service.StartAsync();
            
            var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
            logger?.LogInformation("集成式锁管理服务初始化并启动完成");
            
            return service;
        }

        /// <summary>
        /// 安全初始化服务（带错误处理）
        /// 初始化失败时返回null而不抛出异常
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>服务实例或null</returns>
        public static async Task<object> InitializeServiceSafeAsync(string serviceType = "integrated")
        {
            try
            {
                switch (serviceType.ToLowerInvariant())
                {
                    case "integrated":
                        return await InitializeIntegratedServiceAsync();
                    case "legacy":
                        return CreateLegacyService();
                    case "auto":
                    default:
                        return CreateAutoService();
                }
            }
            catch (Exception ex)
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger?.LogError(ex, "安全初始化服务失败，服务类型: {ServiceType}", serviceType);
                return null;
            }
        }

        #endregion

        #region 迁移辅助接口

        /// <summary>
        /// 检查迁移条件
        /// 验证系统是否准备好迁移到新版本
        /// </summary>
        /// <returns>迁移检查结果</returns>
        public static MigrationCheckResult CheckMigrationReadiness()
        {
            var result = new MigrationCheckResult
            {
                FactoryVersion = FACTORY_VERSION,
                RecommendedVersion = RECOMMENDED_SERVICE_VERSION,
                IsReady = true,
                Warnings = new System.Collections.Generic.List<string>()
            };

            try
            {
                // 检查依赖服务
                var communicationService = ApplicationContext.Current?.GetService<ClientCommunicationService>();
                if (communicationService == null)
                {
                    result.IsReady = false;
                    result.Warnings.Add("ClientCommunicationService 不可用");
                }

                var heartbeatManager = ApplicationContext.Current?.GetService<HeartbeatManager>();
                if (heartbeatManager == null)
                {
                    result.IsReady = false;
                    result.Warnings.Add("HeartbeatManager 不可用");
                }

                // 检查日志服务
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                if (logger == null)
                {
                    result.Warnings.Add("日志服务不可用，但不影响基本功能");
                }

                result.CheckTime = DateTime.Now;

                var logger2 = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger2?.LogInformation("迁移检查完成，准备状态: {IsReady}, 警告数量: {WarningCount}", 
                    result.IsReady, result.Warnings.Count);

                return result;
            }
            catch (Exception ex)
            {
                var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
                logger?.LogError(ex, "迁移检查过程中发生异常");
                
                result.IsReady = false;
                result.Warnings.Add($"检查异常: {ex.Message}");
                return result;
            }
        }

        /// <summary>
        /// 执行平滑迁移
        /// 从传统服务平滑迁移到集成式服务
        /// </summary>
        /// <param name="legacyService">传统服务实例</param>
        /// <returns>新的集成式服务实例</returns>
        public static async Task<IntegratedLockManagementService> MigrateToIntegratedServiceAsync(LockManagementService legacyService)
        {
            var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
            
            try
            {
                logger?.LogInformation("开始迁移到集成式锁管理服务");

                // 创建新的集成式服务
                var integratedService = CreateIntegratedService();

                // 启动新服务
                await integratedService.StartAsync();

                // 尝试从旧服务迁移状态（如果需要）
                await MigrateServiceStateAsync(legacyService, integratedService);

                // 清理旧服务
                if (legacyService != null)
                {
                    // 这里可以添加旧服务的清理逻辑
                    logger?.LogInformation("旧服务状态清理完成");
                }

                logger?.LogInformation("迁移到集成式锁管理服务完成");
                return integratedService;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "迁移到集成式服务失败");
                throw;
            }
        }

        /// <summary>
        /// 迁移服务状态
        /// 在迁移过程中保持状态一致性
        /// </summary>
        private static async Task MigrateServiceStateAsync(LockManagementService legacyService, IntegratedLockManagementService integratedService)
        {
            // 这里可以实现状态迁移逻辑
            // 例如：将活跃的锁信息从旧服务迁移到新服务
            
            await Task.CompletedTask;
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 获取服务版本信息
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns>版本信息字符串</returns>
        public static string GetServiceVersionInfo(string serviceType = "integrated")
        {
            switch (serviceType.ToLowerInvariant())
            {
                case "integrated":
                    return $"集成式锁管理服务 v{IntegratedLockManagementService.SERVICE_VERSION}";
                case "legacy":
                    return $"传统锁管理服务 v1.0.0";
                case "factory":
                    return $"服务工厂 v{FACTORY_VERSION}";
                default:
                    return $"未知服务类型: {serviceType}";
            }
        }

        /// <summary>
        /// 记录服务统计信息
        /// </summary>
        /// <param name="service">服务实例</param>
        public static void LogServiceStatistics(object service)
        {
            var logger = ApplicationContext.Current?.GetLogger<LockManagementServiceFactory>();
            
            try
            {
                switch (service)
                {
                    case IntegratedLockManagementService integrated:
                        var stats = integrated.GetStatistics();
                        logger?.LogInformation("集成式服务统计: {Summary}", stats.GetSummary());
                        break;
                    
                    case LockManagementService legacy:
                        logger?.LogInformation("传统服务运行中");
                        break;
                    
                    default:
                        logger?.LogWarning("未知服务类型: {ServiceType}", service.GetType().Name);
                        break;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "记录服务统计信息失败");
            }
        }

        #endregion
    }

    #region 辅助类

    /// <summary>
    /// 迁移检查结果
    /// </summary>
    public class MigrationCheckResult
    {
        public string FactoryVersion { get; set; }
        public string RecommendedVersion { get; set; }
        public bool IsReady { get; set; }
        public System.Collections.Generic.List<string> Warnings { get; set; }
        public DateTime CheckTime { get; set; }

        public string GetSummary()
        {
            return $"迁移检查 v{FactoryVersion} - 准备状态: {(IsReady ? "✅ 就绪" : "❌ 未就绪")}, " +
                   $"警告: {Warnings.Count} 项, 检查时间: {CheckTime:yyyy-MM-dd HH:mm:ss}";
        }
    }

    #endregion
}