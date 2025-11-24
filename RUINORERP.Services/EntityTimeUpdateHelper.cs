using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Services
{
    /// <summary>
    /// 实体时间更新帮助类
    /// 用于在代码中自动设置CreateTime和UpdateTime，替代数据库触发器
    /// </summary>
    public static class EntityTimeUpdateHelper
    {
        private static readonly Dictionary<Type, PropertyInfo> _createTimeProperties = new Dictionary<Type, PropertyInfo>();
        private static readonly Dictionary<Type, PropertyInfo> _updateTimeProperties = new Dictionary<Type, PropertyInfo>();
        private static readonly object _lock = new object();

        /// <summary>
        /// 为新增实体设置时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="logger">日志记录器</param>
        public static void SetCreateTimeForEntity<T>(T entity, ILogger logger = null) where T : class
        {
            if (entity == null) return;

            try
            {
                var entityType = typeof(T);
                var now = DateTime.Now;

                // 设置CreateTime
                var createTimeProp = GetOrCreateTimeProperty(entityType, "CreateTime");
                if (createTimeProp != null && createTimeProp.CanWrite)
                {
                    createTimeProp.SetValue(entity, now);
                }

                // 设置UpdateTime
                var updateTimeProp = GetOrCreateTimeProperty(entityType, "UpdateTime");
                if (updateTimeProp != null && updateTimeProp.CanWrite)
                {
                    updateTimeProp.SetValue(entity, now);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "设置实体创建时间失败: {EntityType}", typeof(T).Name);
            }
        }

        /// <summary>
        /// 为更新实体设置时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entity">实体实例</param>
        /// <param name="logger">日志记录器</param>
        public static void SetUpdateTimeForEntity<T>(T entity, ILogger logger = null) where T : class
        {
            if (entity == null) return;

            try
            {
                var entityType = typeof(T);
                var updateTimeProp = GetOrCreateTimeProperty(entityType, "UpdateTime");
                if (updateTimeProp != null && updateTimeProp.CanWrite)
                {
                    updateTimeProp.SetValue(entity, DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "设置实体更新时间失败: {EntityType}", typeof(T).Name);
            }
        }

        /// <summary>
        /// 为批量新增实体设置时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        /// <param name="logger">日志记录器</param>
        public static void SetCreateTimeForEntities<T>(IEnumerable<T> entities, ILogger logger = null) where T : class
        {
            if (entities == null) return;

            foreach (var entity in entities)
            {
                SetCreateTimeForEntity(entity, logger);
            }
        }

        /// <summary>
        /// 为批量更新实体设置时间
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        /// <param name="logger">日志记录器</param>
        public static void SetUpdateTimeForEntities<T>(IEnumerable<T> entities, ILogger logger = null) where T : class
        {
            if (entities == null) return;

            foreach (var entity in entities)
            {
                SetUpdateTimeForEntity(entity, logger);
            }
        }

        /// <summary>
        /// 检查实体是否有时间属性
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>是否包含时间属性</returns>
        public static bool HasTimeProperties<T>() where T : class
        {
            var entityType = typeof(T);
            return GetOrCreateTimeProperty(entityType, "CreateTime") != null ||
                   GetOrCreateTimeProperty(entityType, "UpdateTime") != null;
        }

        /// <summary>
        /// 获取或创建时间属性缓存
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性信息</returns>
        private static PropertyInfo GetOrCreateTimeProperty(Type entityType, string propertyName)
        {
            var cache = propertyName == "CreateTime" ? _createTimeProperties : _updateTimeProperties;
            var key = entityType;

            if (cache.TryGetValue(key, out var property))
            {
                return property;
            }

            lock (_lock)
            {
                if (cache.TryGetValue(key, out property))
                {
                    return property;
                }

                // 查找属性
                property = entityType.GetProperty(propertyName, typeof(DateTime));
                if (property == null)
                {
                    property = entityType.GetProperty(propertyName, typeof(DateTime?));
                }

                cache[key] = property;
                return property;
            }
        }

        /// <summary>
        /// 清除属性缓存（用于测试或特殊情况）
        /// </summary>
        public static void ClearCache()
        {
            lock (_lock)
            {
                _createTimeProperties.Clear();
                _updateTimeProperties.Clear();
            }
        }
    }

    /// <summary>
    /// 服务基类扩展方法
    /// 为现有的服务类提供时间更新支持
    /// </summary>
    public static class ServiceTimeUpdateExtensions
    {
        /// <summary>
        /// 保存前设置时间 - tb_ProcessNavigation专用
        /// </summary>
        /// <param name="processNavigation">流程导航图实体</param>
        /// <param name="logger">日志记录器</param>
        public static void SetTimeBeforeSave(this tb_ProcessNavigation processNavigation, ILogger logger = null)
        {
            if (processNavigation.ProcessNavID == 0)
            {
                // 新增
                EntityTimeUpdateHelper.SetCreateTimeForEntity(processNavigation, logger);
                if (processNavigation.Version == 0)
                {
                    processNavigation.Version = 1;
                }
                processNavigation.IsActive = true;
            }
            else
            {
                // 更新
                EntityTimeUpdateHelper.SetUpdateTimeForEntity(processNavigation, logger);
            }
        }

        /// <summary>
        /// 保存前设置时间 - tb_ProcessNavigationNode专用
        /// </summary>
        /// <param name="node">流程导航图节点实体</param>
        /// <param name="logger">日志记录器</param>
        public static void SetTimeBeforeSave(this tb_ProcessNavigationNode node, ILogger logger = null)
        {
            if (node.NodeID == 0)
            {
                // 新增
                EntityTimeUpdateHelper.SetCreateTimeForEntity(node, logger);
            }
            else
            {
                // 更新
                EntityTimeUpdateHelper.SetUpdateTimeForEntity(node, logger);
            }
        }

        /// <summary>
        /// 批量保存前设置时间 - tb_ProcessNavigationNode集合专用
        /// </summary>
        /// <param name="nodes">流程导航图节点集合</param>
        /// <param name="logger">日志记录器</param>
        public static void SetTimeBeforeSave(this IEnumerable<tb_ProcessNavigationNode> nodes, ILogger logger = null)
        {
            foreach (var node in nodes)
            {
                node.SetTimeBeforeSave(logger);
            }
        }
    }
}