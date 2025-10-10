using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Linq.Expressions;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一的命令缓存管理器 - 集中管理所有命令相关的缓存
    /// 提供类型缓存、构造函数缓存、创建器缓存等功能
    /// </summary>
    public class CommandCacheManager
    {
        private readonly ILogger<CommandCacheManager> _logger;
        
        // 类型缓存 - 命令ID到类型的映射
        private readonly ConcurrentDictionary<CommandId, Type> _commandTypeCache;
        
        // 类型名称缓存 - 类型名称到类型的映射
        private readonly ConcurrentDictionary<string, Type> _commandTypeByNameCache;
        
        // 构造函数缓存 - 类型到构造函数委托的映射
        private readonly ConcurrentDictionary<Type, Func<ICommand>> _constructorCache;
        
        // 命令创建器缓存 - 命令ID到创建器函数的映射
        private readonly ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>> _commandCreatorCache;
        
        // 扫描结果缓存 - 避免重复扫描
        private readonly ConcurrentDictionary<string, Dictionary<CommandId, Type>> _scanResultCache;
        
        // 处理器类型缓存 - 命令类型到处理器类型的映射
        private readonly ConcurrentDictionary<Type, Type> _handlerTypeCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public CommandCacheManager(ILogger<CommandCacheManager> logger = null)
        {
            _logger = logger;
            _commandTypeCache = new ConcurrentDictionary<CommandId, Type>();
            _commandTypeByNameCache = new ConcurrentDictionary<string, Type>();
            _constructorCache = new ConcurrentDictionary<Type, Func<ICommand>>();
            _commandCreatorCache = new ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>>();
            _scanResultCache = new ConcurrentDictionary<string, Dictionary<CommandId, Type>>();
            _handlerTypeCache = new ConcurrentDictionary<Type, Type>();
        }

        /// <summary>
        /// 缓存命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="commandType">命令类型</param>
        public void CacheCommandType(CommandId commandId, Type commandType)
        {
            if (commandId == null || commandType == null) return;
            
            _commandTypeCache.TryAdd(commandId, commandType);
            _commandTypeByNameCache.TryAdd(commandType.FullName, commandType);
            _commandTypeByNameCache.TryAdd(commandType.Name, commandType);
            
            _logger?.LogDebug($"缓存命令类型: {commandId} -> {commandType.Name}");
        }

        /// <summary>
        /// 获取缓存的命令类型
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令类型，未找到返回null</returns>
        public Type GetCachedCommandType(CommandId commandId)
        {
            return _commandTypeCache.TryGetValue(commandId, out var type) ? type : null;
        }

        /// <summary>
        /// 根据类型名称获取缓存的命令类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>命令类型，未找到返回null</returns>
        public Type GetCachedCommandTypeByName(string typeName)
        {
            return _commandTypeByNameCache.TryGetValue(typeName, out var type) ? type : null;
        }

        /// <summary>
        /// 缓存命令构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="constructor">构造函数委托</param>
        public void CacheConstructor(Type commandType, Func<ICommand> constructor)
        {
            if (commandType == null || constructor == null) return;
            
            _constructorCache.TryAdd(commandType, constructor);
            _logger?.LogDebug($"缓存构造函数: {commandType.Name}");
        }

        /// <summary>
        /// 获取缓存的构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>构造函数委托，未找到返回null</returns>
        public Func<ICommand> GetCachedConstructor(Type commandType)
        {
            return _constructorCache.TryGetValue(commandType, out var constructor) ? constructor : null;
        }

        /// <summary>
        /// 创建或获取构造函数（使用表达式树优化）
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>构造函数委托</returns>
        public Func<ICommand> GetOrCreateConstructor(Type commandType)
        {
            if (commandType == null) return null;

            // 先尝试从缓存获取
            var cachedConstructor = GetCachedConstructor(commandType);
            if (cachedConstructor != null) return cachedConstructor;

            try
            {
                // 使用表达式树创建构造函数
                var constructor = commandType.GetConstructor(Type.EmptyTypes);
                if (constructor == null)
                {
                    _logger?.LogWarning($"类型 {commandType.Name} 没有无参构造函数");
                    return null;
                }

                var lambda = Expression.Lambda<Func<ICommand>>(Expression.New(constructor));
                var compiledConstructor = lambda.Compile();

                // 缓存构造函数
                CacheConstructor(commandType, compiledConstructor);
                return compiledConstructor;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"创建构造函数失败: {commandType.Name}");
                return null;
            }
        }

        /// <summary>
        /// 缓存命令创建器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <param name="creator">创建器函数</param>
        public void CacheCommandCreator(CommandId commandId, Func<PacketModel, ICommand> creator)
        {
            if (commandId == null || creator == null) return;
            
            _commandCreatorCache.TryAdd(commandId, creator);
            _logger?.LogDebug($"缓存命令创建器: {commandId}");
        }

        /// <summary>
        /// 获取缓存的命令创建器
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>创建器函数，未找到返回null</returns>
        public Func<PacketModel, ICommand> GetCachedCommandCreator(CommandId commandId)
        {
            return _commandCreatorCache.TryGetValue(commandId, out var creator) ? creator : null;
        }

        /// <summary>
        /// 缓存扫描结果
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="scanResults">扫描结果</param>
        public void CacheScanResults(string cacheKey, Dictionary<CommandId, Type> scanResults)
        {
            if (string.IsNullOrEmpty(cacheKey) || scanResults == null) return;
            
            _scanResultCache.TryAdd(cacheKey, scanResults);
            _logger?.LogDebug($"缓存扫描结果: {cacheKey}, 包含 {scanResults.Count} 个类型");
        }

        /// <summary>
        /// 获取缓存的扫描结果
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns>扫描结果，未找到返回null</returns>
        public Dictionary<CommandId, Type> GetCachedScanResults(string cacheKey)
        {
            return _scanResultCache.TryGetValue(cacheKey, out var results) ? results : null;
        }

        /// <summary>
        /// 缓存处理器类型映射
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="handlerType">处理器类型</param>
        public void CacheHandlerType(Type commandType, Type handlerType)
        {
            if (commandType == null || handlerType == null) return;
            
            _handlerTypeCache.TryAdd(commandType, handlerType);
            _logger?.LogDebug($"缓存处理器类型映射: {commandType.Name} -> {handlerType.Name}");
        }

        /// <summary>
        /// 获取缓存的处理器类型
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>处理器类型，未找到返回null</returns>
        public Type GetCachedHandlerType(Type commandType)
        {
            return _handlerTypeCache.TryGetValue(commandType, out var handlerType) ? handlerType : null;
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void ClearAllCaches()
        {
            _commandTypeCache.Clear();
            _commandTypeByNameCache.Clear();
            _constructorCache.Clear();
            _commandCreatorCache.Clear();
            _scanResultCache.Clear();
            _handlerTypeCache.Clear();
            
            _logger?.LogInformation("已清空所有命令缓存");
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public CacheStatistics GetCacheStatistics()
        {
            return new CacheStatistics
            {
                CommandTypeCacheCount = _commandTypeCache.Count,
                CommandTypeByNameCacheCount = _commandTypeByNameCache.Count,
                ConstructorCacheCount = _constructorCache.Count,
                CommandCreatorCacheCount = _commandCreatorCache.Count,
                ScanResultCacheCount = _scanResultCache.Count,
                HandlerTypeCacheCount = _handlerTypeCache.Count,
                TotalCacheEntries = _commandTypeCache.Count + _commandTypeByNameCache.Count + 
                                   _constructorCache.Count + _commandCreatorCache.Count + 
                                   _scanResultCache.Count + _handlerTypeCache.Count
            };
        }
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        public int CommandTypeCacheCount { get; set; }
        public int CommandTypeByNameCacheCount { get; set; }
        public int ConstructorCacheCount { get; set; }
        public int CommandCreatorCacheCount { get; set; }
        public int ScanResultCacheCount { get; set; }
        public int HandlerTypeCacheCount { get; set; }
        public int TotalCacheEntries { get; set; }
    }
}
