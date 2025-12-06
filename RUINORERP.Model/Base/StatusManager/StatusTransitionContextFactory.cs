/**
 * 文件: StatusTransitionContextFactory.cs
 * 说明: 状态转换上下文工厂类 - 用于统一创建和管理StatusTransitionContext实例
 * 创建日期: 2024年
 * 更新日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换上下文工厂类
    /// 提供统一的方法来创建和管理StatusTransitionContext实例，避免重复代码
    /// 支持泛型创建、上下文缓存、克隆等增强功能
    /// </summary>
    public static class StatusTransitionContextFactory
    {
        /// <summary>
        /// 用于缓存状态上下文的字典，以实体ID和状态类型为键
        /// 使用ConcurrentDictionary确保线程安全
        /// </summary>
        private static readonly ConcurrentDictionary<string, StatusTransitionContext> _contextCache = 
            new ConcurrentDictionary<string, StatusTransitionContext>();
        
        /// <summary>
        /// 缓存清理间隔（毫秒），默认5分钟
        /// </summary>
        private static readonly int _cacheCleanupInterval = 5 * 60 * 1000;
        
        /// <summary>
        /// 上次清理缓存的时间
        /// </summary>
        private static DateTime _lastCacheCleanup = DateTime.Now;
        
        /// <summary>
        /// 最大缓存条目数，防止内存泄漏
        /// </summary>
        private static readonly int _maxCacheSize = 1000;
    
        /// <summary>
        /// 清理过期缓存项
        /// </summary>
        private static void CleanupCache()
        {
            // 检查是否需要清理缓存
            if ((DateTime.Now - _lastCacheCleanup).TotalMilliseconds < _cacheCleanupInterval && 
                _contextCache.Count <= _maxCacheSize)
            {
                return;
            }
            
            _lastCacheCleanup = DateTime.Now;
            
            // 如果缓存过大，则清理所有缓存项
            if (_contextCache.Count > _maxCacheSize)
            {
                _contextCache.Clear();
            }
        }
        
        /// <summary>
        /// 创建新的状态转换上下文实例
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>新创建的状态转换上下文</returns>
        private static StatusTransitionContext CreateNewContext(
            BaseEntity entity,
            Type statusType,
            object status)
        {
            // 简化创建方式，只传入必要的参数
            return new StatusTransitionContext(entity, statusType, status);
        }
        
        /// <summary>
        /// 创建数据状态转换上下文（支持缓存）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="useCache">是否使用缓存，默认为true</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateDataStatusContext(
            BaseEntity entity,
            DataStatus dataStatus,
            string reason = null,
            IServiceProvider serviceProvider = null,
            bool useCache = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 尝试从缓存获取
            if (useCache)
            {
                CleanupCache(); // 先清理缓存
                string cacheKey = GenerateCacheKey(entity, typeof(DataStatus));
                
                if (_contextCache.TryGetValue(cacheKey, out StatusTransitionContext cachedContext))
                {
                    // 创建新的上下文，而不是修改现有只读属性
                    StatusTransitionContext newContext = CreateNewContext(entity, typeof(DataStatus), dataStatus);
                    if (!string.IsNullOrEmpty(reason))
                    {
                        newContext.Reason = reason;
                    }
                    return newContext;
                }
            }

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(DataStatus),
                dataStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            // 缓存上下文
            if (useCache)
            {
                string cacheKey = GenerateCacheKey(entity, typeof(DataStatus));
                _contextCache.TryAdd(cacheKey, context);
            }

            return context;
        }
        
        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>生成的缓存键</returns>
        private static string GenerateCacheKey(BaseEntity entity, Type statusType)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
                
            // 获取实体ID
            return $"{entity.GetType().FullName}:{entity.PrimaryKeyID}:{statusType.FullName}";
        }

        /// <summary>
        /// 创建业务状态转换上下文（支持缓存）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="businessStatus">业务状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="useCache">是否使用缓存，默认为true</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateBusinessStatusContext<TBusinessStatus>(
            BaseEntity entity,
            TBusinessStatus businessStatus,
            string reason = null,
            IServiceProvider serviceProvider = null,
            bool useCache = true)
            where TBusinessStatus : struct, Enum
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 尝试从缓存获取
            if (useCache)
            {
                CleanupCache(); // 先清理缓存
                string cacheKey = GenerateCacheKey(entity, typeof(TBusinessStatus));
                
                if (_contextCache.TryGetValue(cacheKey, out StatusTransitionContext cachedContext))
                {
                    // 创建新的上下文，而不是修改现有只读属性
                    StatusTransitionContext newContext = CreateNewContext(entity, typeof(TBusinessStatus), businessStatus);
                    if (!string.IsNullOrEmpty(reason))
                    {
                        newContext.Reason = reason;
                    }
                    return newContext;
                }
            }

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(TBusinessStatus),
                businessStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            // 缓存上下文
            if (useCache)
            {
                string cacheKey = GenerateCacheKey(entity, typeof(TBusinessStatus));
                _contextCache.TryAdd(cacheKey, context);
            }

            return context;
        }

        /// <summary>
        /// 创建操作状态转换上下文（支持缓存）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="actionStatus">操作状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="useCache">是否使用缓存，默认为true</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateActionStatusContext(
            BaseEntity entity,
            ActionStatus actionStatus,
            string reason = null,
            IServiceProvider serviceProvider = null,
            bool useCache = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 尝试从缓存获取
            if (useCache)
            {
                CleanupCache(); // 先清理缓存
                string cacheKey = GenerateCacheKey(entity, typeof(ActionStatus));
                
                if (_contextCache.TryGetValue(cacheKey, out StatusTransitionContext cachedContext))
                {
                    // 创建新的上下文，而不是修改现有只读属性
                    StatusTransitionContext newContext = CreateNewContext(entity, typeof(ActionStatus), actionStatus);
                    if (!string.IsNullOrEmpty(reason))
                    {
                        newContext.Reason = reason;
                    }
                    return newContext;
                }
            }

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                typeof(ActionStatus),
                actionStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            // 缓存上下文
            if (useCache)
            {
                string cacheKey = GenerateCacheKey(entity, typeof(ActionStatus));
                _contextCache.TryAdd(cacheKey, context);
            }

            return context;
        }

        /// <summary>
        /// 创建UI状态更新上下文（用于UI状态更新，使用临时实体）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="useCache">是否使用缓存，默认为true</param>
        /// <returns>状态转换上下文</returns>
        public static StatusTransitionContext CreateUIUpdateContext(
            Type statusType,
            object currentStatus,
            string reason = null,
            IServiceProvider serviceProvider = null,
            bool useCache = true)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            // 尝试从缓存获取
            if (useCache)
            {
                CleanupCache(); // 先清理缓存
                string cacheKey = $"UI:{statusType.FullName}:{currentStatus}";
                
                if (_contextCache.TryGetValue(cacheKey, out StatusTransitionContext cachedContext))
                {
                    // 如果缓存中有，则更新原因后返回
                    if (!string.IsNullOrEmpty(reason))
                    {
                        cachedContext.Reason = reason;
                    }
                    return cachedContext;
                }
            }

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            // 创建临时实体用于UI更新
            var tempEntity = new BaseEntity();

            var context = new StatusTransitionContext(
                tempEntity,
                statusType,
                currentStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            // 缓存上下文
            if (useCache)
            {
                string cacheKey = $"UI:{statusType.FullName}:{currentStatus}";
                _contextCache.TryAdd(cacheKey, context);
            }

            return context;
        }

        /// <summary>
        /// 创建通用状态转换上下文（支持缓存）
        /// </summary>
        /// <param name="entity">实体对象，需要状态管理的实体</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态值，可以是任意类型的状态枚举值</param>
        /// <param name="reason">状态转换原因，可选</param>
        /// <param name="serviceProvider">服务提供程序，可选，用于获取服务</param>
        /// <param name="useCache">是否使用缓存，默认为true</param>
        /// <returns>创建的StatusTransitionContext实例</returns>
        public static StatusTransitionContext CreateContext(
            BaseEntity entity,
            Type statusType,
            object currentStatus,
            string reason = null,
            IServiceProvider serviceProvider = null,
            bool useCache = true)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            // 尝试从缓存获取
            if (useCache && currentStatus != null)
            {
                CleanupCache(); // 先清理缓存
                string cacheKey = GenerateCacheKey(entity, statusType);
                cacheKey += $":{currentStatus}";
                
                if (_contextCache.TryGetValue(cacheKey, out StatusTransitionContext cachedContext))
                {
                    // 创建新的上下文，而不是修改现有只读属性
                    StatusTransitionContext newContext = CreateNewContext(entity, statusType, currentStatus);
                    if (!string.IsNullOrEmpty(reason))
                    {
                        newContext.Reason = reason;
                    }
                    return newContext;
                }
            }

            var statusManager = serviceProvider?.GetService<IUnifiedStateManager>();
            var transitionEngine = serviceProvider?.GetService<IStatusTransitionEngine>();
            var logger = serviceProvider?.GetService<ILogger<StatusTransitionContext>>();

            var context = new StatusTransitionContext(
                entity,
                statusType,
                currentStatus,
                statusManager,
                transitionEngine,
                logger);

            if (!string.IsNullOrEmpty(reason))
            {
                context.Reason = reason;
            }

            // 缓存上下文
            if (useCache && currentStatus != null)
            {
                string cacheKey = GenerateCacheKey(entity, statusType);
                cacheKey += $":{currentStatus}";
                _contextCache.TryAdd(cacheKey, context);
            }

            return context;
        }
        
        
        /// <summary>
        /// 清除指定实体的所有缓存上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        public static void ClearEntityCache(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
                
            string entityKeyPart = $"{entity.GetType().FullName}:{entity.PrimaryKeyID}";
            
            // 找出所有与该实体相关的缓存键并移除
            var keysToRemove = new System.Collections.Generic.List<string>();
            foreach (var key in _contextCache.Keys)
            {
                if (key.StartsWith(entityKeyPart))
                {
                    keysToRemove.Add(key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _contextCache.TryRemove(key, out _);
            }
        }
        
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void ClearAllCache()
        {
            _contextCache.Clear();
            _lastCacheCleanup = DateTime.Now;
        }
        
        /// <summary>
        /// 获取当前缓存大小
        /// </summary>
        /// <returns>缓存中的条目数量</returns>
        public static int GetCacheSize()
        {
            return _contextCache.Count;
        }
    }
}