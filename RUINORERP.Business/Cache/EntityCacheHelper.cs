using RUINORERP.Business.Cache;
using RUINORERP.Business.CommService;
using RUINORERP.PacketSpec.Models.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 实体缓存静态帮助类
    /// 提供静态方法直接访问实体缓存服务，避免在每个类中都需要注入服务实例
    /// 合并了UI层和Server层CacheManager的优点，提供统一的缓存访问入口
    /// </summary>
    public static class EntityCacheHelper
    {
        // 静态锁对象，用于线程安全操作
        private static readonly object _lock = new object();
        // 手动设置的服务实例
        private static IEntityCacheManager _manuallySetService;
        // 延迟加载的服务实例
        private static  Lazy<IEntityCacheManager> _lazyService = new Lazy<IEntityCacheManager>(() => GetServiceFromContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

        /// <summary>
        /// 设置当前使用的实体缓存服务实例
        /// 此方法应在应用程序启动时调用，通常在依赖注入容器配置完成后
        /// 手动设置的服务实例优先级高于从容器获取的实例
        /// </summary>
        /// <param name="service">实体缓存服务实例</param>
        public static void SetCurrent(IEntityCacheManager service)
        {
            lock (_lock)
            {
                _manuallySetService = service;
            }
        }

        /// <summary>
        /// 从依赖注入容器获取服务实例
        /// 尝试从不同项目的容器中获取服务
        /// </summary>
        /// <returns>实体缓存服务实例，如果无法获取则返回null</returns>
        private static IEntityCacheManager GetServiceFromContainer()
        {
            try
            {
                // 尝试从不同项目的容器中获取服务
                // 使用反射避免直接引用其他项目
                Type startupType = null;
                object startupInstance = null;

                // 尝试获取UI层的Startup
                try
                {
                    startupType = Type.GetType("RUINORERP.UI.Common.Startup");
                    if (startupType != null)
                    {
                        // 如果是静态类，直接调用静态方法
                        var method = startupType.GetMethod("GetFromFac", new[] { typeof(Type) });
                        if (method != null)
                        {
                            return method.Invoke(null, new[] { typeof(IEntityCacheManager) }) as IEntityCacheManager;
                        }
                    }
                }
                catch { }

                // 尝试获取Server层的Startup
                try
                {
                    startupType = Type.GetType("RUINORERP.Server.Startup");
                    if (startupType != null)
                    {
                        var method = startupType.GetMethod("GetFromFac", new[] { typeof(Type) });
                        if (method != null)
                        {
                            return method.Invoke(null, new[] { typeof(IEntityCacheManager) }) as IEntityCacheManager;
                        }
                    }
                }
                catch { }

                // 尝试获取Business层的Startup
                try
                {
                    startupType = Type.GetType("RUINORERP.Business.Startup");
                    if (startupType != null)
                    {
                        var method = startupType.GetMethod("GetFromFac", new[] { typeof(Type) });
                        if (method != null)
                        {
                            return method.Invoke(null, new[] { typeof(IEntityCacheManager) }) as IEntityCacheManager;
                        }
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                // 记录异常但不抛出，避免影响应用程序启动
                Console.WriteLine($"获取实体缓存服务实例时发生异常: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 获取当前实体缓存服务实例
        /// 优先级: 手动设置的实例 > 从容器获取的实例
        /// 如果都不可用，记录警告并返回null（增强的防御性编程）
        /// </summary>
        public static IEntityCacheManager Current
        {
            get
            {
                // 优先使用手动设置的实例
                if (_manuallySetService != null)
                {
                    return _manuallySetService;
                }
                if (_lazyService.Value == null)
                {
                    _lazyService = new Lazy<IEntityCacheManager>(() => GetServiceFromContainer(), LazyThreadSafetyMode.ExecutionAndPublication);

                }
                // 尝试从容器获取实例
                var service = _lazyService.Value;
                if (service != null)
                {
                    return service;
                }

                // 记录警告日志
                LogWarning("实体缓存服务未初始化，无法获取缓存服务实例。请确保在应用启动时调用SetCurrent方法或配置正确的依赖注入");
                return null;
            }
        }

        /// <summary>
        /// 记录警告日志
        /// 尝试通过不同方式记录日志，避免依赖特定日志系统
        /// </summary>
        /// <param name="message">警告信息</param>
        private static void LogWarning(string message)
        {
            try
            {
                // 尝试获取MainForm实例记录日志
                var mainFormType = Type.GetType("RUINORERP.UI.Common.MainForm");
                if (mainFormType != null)
                {
                    var instanceProperty = mainFormType.GetProperty("Instance");
                    if (instanceProperty != null)
                    {
                        var instance = instanceProperty.GetValue(null);
                        if (instance != null)
                        {
                            var loggerProperty = instance.GetType().GetProperty("logger");
                            if (loggerProperty != null)
                            {
                                var logger = loggerProperty.GetValue(instance);
                                if (logger != null)
                                {
                                    var logErrorMethod = logger.GetType().GetMethod("LogError", new[] { typeof(string) });
                                    if (logErrorMethod != null)
                                    {
                                        logErrorMethod.Invoke(logger, new object[] { message });
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                // 降级到控制台输出
                Console.WriteLine($"警告: {message}");
            }
            catch { }
        }

        /// <summary>
        /// 安全地执行缓存操作
        /// 如果服务实例为null，返回默认值而不是抛出异常
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">要执行的操作</param>
        /// <param name="defaultValue">默认返回值</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>操作结果或默认值</returns>
        private static T SafeExecute<T>(Func<IEntityCacheManager, T> action, T defaultValue = default, string errorMessage = null)
        {
            var service = Current;
            if (service == null)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    LogWarning(errorMessage);
                }
                return defaultValue;
            }

            try
            {
                return action(service);
            }
            catch (Exception ex)
            {
                LogWarning($"执行缓存操作时发生异常: {ex.Message}\n{errorMessage}");
                return defaultValue;
            }
        }

        /// <summary>
        /// 安全地执行无返回值的缓存操作
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="errorMessage">错误信息</param>
        private static void SafeExecute(Action<IEntityCacheManager> action, string errorMessage = null)
        {
            var service = Current;
            if (service == null)
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    LogWarning(errorMessage);
                }
                return;
            }

            try
            {
                action(service);
            }
            catch (Exception ex)
            {
                LogWarning($"执行缓存操作时发生异常: {ex.Message}\n{errorMessage}");
            }
        }

        #region 缓存键生成方法
        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="type">缓存类型</param>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValue">可选的主键值（用于实体和显示值缓存）</param>
        /// <returns>格式化的缓存键</returns>
        public static string GenerateCacheKey(IEntityCacheManager.CacheKeyType type, string tableName, object primaryKeyValue = null)
        {
            return SafeExecute(service => service.GenerateCacheKey(type, tableName, primaryKeyValue), $"{type}_{tableName}_{primaryKeyValue}", "无法生成缓存键");
        }
        #endregion

        #region 缓存查询方法
        /// <summary>
        /// 获取指定类型的实体列表
        /// </summary>
        public static List<T> GetEntityList<T>() where T : class
        {
            return SafeExecute(service => service.GetEntityList<T>(), new List<T>(), $"无法获取类型 {typeof(T).Name} 的缓存数据");
        }

        /// <summary>
        /// 根据表名获取指定类型的实体列表
        /// </summary>
        public static List<T> GetEntityList<T>(string tableName) where T : class
        {
            return SafeExecute(service => service.GetEntityList<T>(tableName), new List<T>(), $"无法获取表 {tableName} 的缓存数据");
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        public static T GetEntity<T>(object idValue) where T : class
        {
            return SafeExecute(service => service.GetEntity<T>(idValue), null, $"无法获取类型 {typeof(T).Name} 中ID为 {idValue} 的实体");
        }

        /// <summary>
        /// 根据表名和主键值获取实体
        /// </summary>
        public static object GetEntity(string tableName, object primaryKeyValue)
        {
            return SafeExecute(service => service.GetEntity(tableName, primaryKeyValue), null, $"无法获取表 {tableName} 中主键值为 {primaryKeyValue} 的实体");
        }

        /// <summary>
        /// 获取指定表名的显示值
        /// </summary>
        public static object GetDisplayValue(string tableName, object idValue)
        {
            return SafeExecute(service => service.GetDisplayValue(tableName, idValue), null, $"无法获取表 {tableName} 中ID为 {idValue} 的显示值");
        }

        /// <summary>
        /// 根据表名获取实体列表，返回强类型集合
        /// 无需传入泛型类型T，系统会自动根据表名解析对应的实体类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>实体列表，类型为表对应的强类型集合</returns>
        public static dynamic GetEntityListByTableName(string tableName)
        {
            return SafeExecute(service => service.GetEntityListByTableName(tableName), null, $"无法根据表名 {tableName} 获取实体列表");
        }
        #endregion

        #region 缓存更新方法
        /// <summary>
        /// 更新实体列表缓存
        /// </summary>
        public static void UpdateEntityList<T>(List<T> list) where T : class
        {
            SafeExecute(service => service.UpdateEntityList(list), $"无法更新类型 {typeof(T).Name} 的实体列表缓存");
        }

        /// <summary>
        /// 更新单个实体缓存
        /// </summary>
        public static void UpdateEntity<T>(T entity) where T : class
        {
            SafeExecute(service => service.UpdateEntity(entity), $"无法更新类型 {typeof(T).Name} 的实体缓存");
        }

        /// <summary>
        /// 根据表名更新缓存
        /// </summary>
        public static void UpdateEntityList(string tableName, object list)
        {
            SafeExecute(service => service.UpdateEntityList(tableName, list), $"无法更新表 {tableName} 的实体列表缓存");
        }

        /// <summary>
        /// 根据表名更新单个实体缓存
        /// </summary>
        public static void UpdateEntity(string tableName, object entity)
        {
            SafeExecute(service => service.UpdateEntity(tableName, entity), $"无法更新表 {tableName} 的实体缓存");
        }
        #endregion

        #region 缓存删除方法
        /// <summary>
        /// 删除指定ID的实体缓存
        /// </summary>
        public static void DeleteEntity<T>(object idValue) where T : class
        {
            SafeExecute(service => service.DeleteEntity<T>(idValue), $"无法删除类型 {typeof(T).Name} 中ID为 {idValue} 的实体缓存");
        }

        /// <summary>
        /// 删除实体列表缓存
        /// </summary>
        public static void DeleteEntityList<T>(List<T> entities) where T : class
        {
            SafeExecute(service => service.DeleteEntityList(entities), $"无法删除类型 {typeof(T).Name} 的实体列表缓存");
        }

        /// <summary>
        /// 删除指定表的整个实体列表缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        public static void DeleteEntityList(string tableName)
        {
            SafeExecute(service => service.DeleteEntityList(tableName), $"无法删除表 {tableName} 的实体列表缓存");
        }

        /// <summary>
        /// 根据表名和主键删除实体缓存
        /// </summary>
        public static void DeleteEntity(string tableName, object primaryKeyValue)
        {
            SafeExecute(service => service.DeleteEntity(tableName, primaryKeyValue), $"无法删除表 {tableName} 中主键值为 {primaryKeyValue} 的实体缓存");
        }

        /// <summary>
        /// 批量删除指定主键数组的实体缓存
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="idValues">主键值数组</param>
        public static void DeleteEntities<T>(object[] idValues) where T : class
        {
            SafeExecute(service => service.DeleteEntities<T>(idValues), $"无法批量删除类型 {typeof(T).Name} 的实体缓存");
        }

        /// <summary>
        /// 根据表名批量删除指定主键数组的实体缓存
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="primaryKeyValues">主键值数组</param>
        public static void DeleteEntities(string tableName, object[] primaryKeyValues)
        {
            SafeExecute(service => service.DeleteEntities(tableName, primaryKeyValues), $"无法批量删除表 {tableName} 的实体缓存");
        }
        #endregion

        #region 缓存统计方法
        /// <summary>
        /// 缓存命中次数
        /// </summary>
        public static long CacheHits => SafeExecute(service => service.CacheHits, 0L);

        /// <summary>
        /// 缓存未命中次数
        /// </summary>
        public static long CacheMisses => SafeExecute(service => service.CacheMisses, 0L);

        /// <summary>
        /// 缓存命中率
        /// </summary>
        public static double HitRatio => SafeExecute(service => service.HitRatio, 0.0);

        /// <summary>
        /// 缓存写入次数
        /// </summary>
        public static long CachePuts => SafeExecute(service => service.CachePuts, 0L);

        /// <summary>
        /// 缓存删除次数
        /// </summary>
        public static long CacheRemoves => SafeExecute(service => service.CacheRemoves, 0L);

        /// <summary>
        /// 缓存项总数
        /// </summary>
        public static int CacheItemCount => SafeExecute(service => service.CacheItemCount, 0);

        /// <summary>
        /// 缓存大小（估计值，单位：字节）
        /// </summary>
        public static long EstimatedCacheSize => SafeExecute(service => service.EstimatedCacheSize, 0L);

        /// <summary>
        /// 重置统计信息
        /// </summary>
        public static void ResetStatistics()
        {
            SafeExecute(service => service.ResetStatistics(), "无法重置缓存统计信息");
        }

        /// <summary>
        /// 获取缓存项统计详情
        /// </summary>
        public static Dictionary<string, CacheItemStatistics> GetCacheItemStatistics()
        {
            return SafeExecute(service => service.GetCacheItemStatistics(), new Dictionary<string, CacheItemStatistics>(), "无法获取缓存项统计详情");
        }

        /// <summary>
        /// 获取按表名分组的缓存统计
        /// </summary>
        public static Dictionary<string, TableCacheStatistics> GetTableCacheStatistics()
        {
            return SafeExecute(service => service.GetTableCacheStatistics(), new Dictionary<string, TableCacheStatistics>(), "无法获取按表名分组的缓存统计");
        }
        #endregion
    }
}