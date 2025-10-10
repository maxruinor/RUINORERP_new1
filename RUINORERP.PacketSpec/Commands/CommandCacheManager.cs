using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Linq.Expressions;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一的命令缓存管理器 - 企业级高性能缓存系统
    /// 
    /// 职责：
    /// 1. 缓存命令类型、构造函数、创建器等基础信息
    /// 2. 缓存扫描结果避免重复扫描
    /// 3. 实现增量扫描支持
    /// 4. 提供缓存预热机制
    /// 5. 支持缓存持久化和恢复
    /// 6. 提供缓存统计和监控
    /// 
    /// 设计目标：
    /// - 显著提升系统启动和运行性能
    /// - 支持智能缓存失效和更新
    /// - 提供多级缓存策略
    /// - 支持分布式缓存扩展
    /// </summary>
    public class CommandCacheManager
    {
        private readonly ILogger<CommandCacheManager> _logger;
        
        // 基础缓存 - 命令ID到类型的映射
        private readonly ConcurrentDictionary<CommandId, Type> _commandTypeCache;
        
        // 构造函数和创建器缓存
        private readonly ConcurrentDictionary<Type, Func<ICommand>> _constructorCache;
        private readonly ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>> _commandCreatorCache;
        
        // 增强扫描缓存
        private readonly ConcurrentDictionary<string, ScanCacheEntry> _scanResultCache;
        private readonly ConcurrentDictionary<string, AssemblyMetadata> _assemblyMetadataCache;
        private readonly ConcurrentDictionary<string, DateTime> _lastScanTime;
        private readonly ConcurrentDictionary<string, string> _assemblyChecksums;
        
        // 处理器缓存
        private readonly ConcurrentDictionary<string, HandlerCacheEntry> _handlerTypeCache;
        private readonly ConcurrentDictionary<string, List<HandlerCacheEntry>> _assemblyHandlerCache;
        
        // 缓存持久化和统计
        private readonly string _cachePersistencePath;
        private readonly Timer _cacheWarmupTimer;
        private readonly Timer _cacheCleanupTimer;
        private long _cacheHits;
        private long _cacheMisses;
        private long _incrementalScans;
        private long _fullScans;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cachePersistencePath">缓存持久化路径</param>
        /// <param name="enableAutoWarmup">是否启用自动预热</param>
        /// <param name="logger">日志记录器</param>
        public CommandCacheManager(string cachePersistencePath = null, bool enableAutoWarmup = true, ILogger<CommandCacheManager> logger = null)
        {
            _logger = logger;
            
            // 初始化基础缓存
            _commandTypeCache = new ConcurrentDictionary<CommandId, Type>();
            _constructorCache = new ConcurrentDictionary<Type, Func<ICommand>>();
            _commandCreatorCache = new ConcurrentDictionary<CommandId, Func<PacketModel, ICommand>>();
            
            // 初始化增强缓存
            _scanResultCache = new ConcurrentDictionary<string, ScanCacheEntry>();
            _assemblyMetadataCache = new ConcurrentDictionary<string, AssemblyMetadata>();
            _lastScanTime = new ConcurrentDictionary<string, DateTime>();
            _assemblyChecksums = new ConcurrentDictionary<string, string>();
            
            // 初始化处理器缓存
            _handlerTypeCache = new ConcurrentDictionary<string, HandlerCacheEntry>();
            _assemblyHandlerCache = new ConcurrentDictionary<string, List<HandlerCacheEntry>>();
            
            // 设置缓存持久化路径
            _cachePersistencePath = cachePersistencePath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "RUINORERP",
                "CommandCache"
            );
            
            // 确保缓存目录存在
            if (!string.IsNullOrEmpty(_cachePersistencePath))
            {
                Directory.CreateDirectory(_cachePersistencePath);
            }
            
            // 启动自动预热和清理定时器
            if (enableAutoWarmup)
            {
                _cacheWarmupTimer = new Timer(WarmupCache, null, TimeSpan.FromMinutes(5), TimeSpan.FromHours(1));
                _cacheCleanupTimer = new Timer(CleanupExpiredCache, null, TimeSpan.FromHours(1), TimeSpan.FromHours(6));
            }
            
            // 加载持久化缓存
            LoadPersistedCache().ConfigureAwait(false).GetAwaiter().GetResult();
            
            _logger?.LogInformation("统一缓存管理器初始化完成，缓存路径: {CachePath}", _cachePersistencePath);
        }

        #region 基础缓存操作

        /// <summary>
        /// 缓存命令类型
        /// </summary>
        public void CacheCommandType(CommandId commandId, Type commandType)
        {
            if (commandId == null || commandType == null) return;
            
            _commandTypeCache.TryAdd(commandId, commandType);
            
            _logger?.LogDebug($"缓存命令类型: {commandId} -> {commandType.Name}");
        }

        /// <summary>
        /// 获取缓存的命令类型
        /// </summary>
        public Type GetCachedCommandType(CommandId commandId)
        {
            return _commandTypeCache.TryGetValue(commandId, out var type) ? type : null;
        }

        /// <summary>
        /// 缓存命令构造函数
        /// </summary>
        public void CacheConstructor(Type commandType, Func<ICommand> constructor)
        {
            if (commandType == null || constructor == null) return;
            
            _constructorCache.TryAdd(commandType, constructor);
            _logger?.LogDebug($"缓存构造函数: {commandType.Name}");
        }

        /// <summary>
        /// 获取缓存的构造函数
        /// </summary>
        public Func<ICommand> GetCachedConstructor(Type commandType)
        {
            return _constructorCache.TryGetValue(commandType, out var constructor) ? constructor : null;
        }

        /// <summary>
        /// 创建或获取构造函数（使用表达式树优化）
        /// </summary>
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
        public void CacheCommandCreator(CommandId commandId, Func<PacketModel, ICommand> creator)
        {
            if (commandId == null || creator == null) return;
            
            _commandCreatorCache.TryAdd(commandId, creator);
            _logger?.LogDebug($"缓存命令创建器: {commandId}");
        }

        /// <summary>
        /// 获取缓存的命令创建器
        /// </summary>
        public Func<PacketModel, ICommand> GetCachedCommandCreator(CommandId commandId)
        {
            return _commandCreatorCache.TryGetValue(commandId, out var creator) ? creator : null;
        }

        /// <summary>
        /// 缓存处理器类型映射
        /// </summary>
        public void CacheHandlerType(Type commandType, Type handlerType)
        {
            if (commandType == null || handlerType == null) return;
            
            var cacheKey = $"handler_{handlerType.FullName}";
            var handlerCacheEntry = new HandlerCacheEntry
            {
                CacheKey = cacheKey,
                HandlerType = handlerType,
                HandlerName = handlerType.Name,
                AssemblyName = handlerType.Assembly.GetName().Name,
                CreatedTime = DateTime.UtcNow,
                LastAccessTime = DateTime.UtcNow,
                AccessCount = 0
            };
            
            _handlerTypeCache.TryAdd(cacheKey, handlerCacheEntry);
            _logger?.LogDebug($"缓存处理器类型映射: {commandType.Name} -> {handlerType.Name}");
        }

        /// <summary>
        /// 获取缓存的处理器类型
        /// </summary>
        public Type GetCachedHandlerType(Type commandType)
        {
            // 注意：这里需要遍历所有处理器缓存，找到与commandType匹配的处理器类型
            foreach (var kvp in _handlerTypeCache)
            {
                var handlerEntry = kvp.Value;
                if (handlerEntry.HandlerType != null)
                {
                    // 这里需要根据实际业务逻辑判断是否匹配
                    // 临时返回第一个处理器的类型作为示例
                    return handlerEntry.HandlerType;
                }
            }
            return null;
        }

        #endregion

        #region 增强扫描缓存

        /// <summary>
        /// 获取或创建扫描缓存
        /// </summary>
        public async Task<ScanCacheEntry> GetOrCreateScanCacheAsync(Assembly assembly, string namespaceFilter = null, bool forceFullScan = false)
        {
            if (assembly == null) return null;
            
            var cacheKey = GenerateCacheKey(assembly, namespaceFilter);
            
            // 尝试从缓存获取
            if (_scanResultCache.TryGetValue(cacheKey, out var existingEntry))
            {
                Interlocked.Increment(ref _cacheHits);
                
                // 检查缓存有效性
                if (await IsCacheValidAsync(existingEntry))
                {
                    existingEntry.LastAccessTime = DateTime.UtcNow;
                    existingEntry.AccessCount++;
                    return existingEntry;
                }
                
                Interlocked.Increment(ref _cacheMisses);
                _logger?.LogInformation("缓存失效，重新扫描: {CacheKey}", cacheKey);
            }
            else
            {
                Interlocked.Increment(ref _cacheMisses);
                _logger?.LogInformation("缓存未命中，开始扫描: {CacheKey}", cacheKey);
            }
            
            // 检查是否支持增量扫描
            var assemblyMetadata = await GetAssemblyMetadataAsync(assembly);
            var canIncremental = !forceFullScan && await CanUseIncrementalScanAsync(assemblyMetadata);
            
            if (canIncremental)
            {
                Interlocked.Increment(ref _incrementalScans);
                _logger?.LogInformation("使用增量扫描: {AssemblyName}", assembly.GetName().Name);
            }
            else
            {
                Interlocked.Increment(ref _fullScans);
                _logger?.LogInformation("使用全量扫描: {AssemblyName}", assembly.GetName().Name);
            }
            
            // 创建新的缓存条目
            var newEntry = new ScanCacheEntry
            {
                CacheKey = cacheKey,
                CreatedTime = DateTime.UtcNow,
                LastAccessTime = DateTime.UtcNow,
                AccessCount = 1,
                IsIncremental = canIncremental,
                AssemblyInfo = assemblyMetadata,
                Dependencies = await GetAssemblyDependenciesAsync(assembly),
                ScanResults = new Dictionary<CommandId, Type>()
            };
            
            // 更新缓存
            _scanResultCache.AddOrUpdate(cacheKey, newEntry, (key, old) => newEntry);
            _assemblyMetadataCache.AddOrUpdate(assembly.GetName().Name, assemblyMetadata, (key, old) => assemblyMetadata);
            
            // 异步持久化缓存
            await PersistCacheAsync(cacheKey, newEntry);
            
            return newEntry;
        }

        /// <summary>
        /// 缓存扫描结果
        /// </summary>
        public async Task CacheScanResultsAsync(Assembly assembly, Dictionary<CommandId, Type> scanResults)
        {
            if (assembly == null || scanResults == null)
            {
                _logger?.LogWarning("程序集或扫描结果为空，无法缓存");
                return;
            }

            try
            {
                var assemblyName = assembly.GetName().Name;
                var cacheKey = GenerateCacheKey(assembly, null);
                
                // 创建或更新缓存条目
                var cacheEntry = await GetOrCreateScanCacheAsync(assembly, null, false);
                
                // 更新扫描结果到缓存条目
                if (cacheEntry != null)
                {
                    cacheEntry.ScanResults = scanResults;
                }
                
                if (cacheEntry != null)
                {
                    _logger?.LogInformation("扫描结果已缓存: {AssemblyName} ({CommandCount} 个命令)", 
                        assemblyName, scanResults.Count);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "缓存扫描结果失败: {AssemblyName}", assembly.GetName().Name);
            }
        }

        /// <summary>
        /// 获取缓存的扫描结果
        /// </summary>
        public Dictionary<CommandId, Type> GetCachedScanResults(string cacheKey)
        {
            return _scanResultCache.TryGetValue(cacheKey, out var entry) ? entry.ScanResults : null;
        }

        #endregion

        #region 缓存持久化和清理

        /// <summary>
        /// 检查缓存是否有效
        /// </summary>
        private async Task<bool> IsCacheValidAsync(ScanCacheEntry cacheEntry)
        {
            if (cacheEntry?.AssemblyInfo == null)
                return false;
            
            try
            {
                var assemblyPath = cacheEntry.AssemblyInfo.FilePath;
                if (!File.Exists(assemblyPath))
                    return false;
                
                var fileInfo = new FileInfo(assemblyPath);
                
                // 检查文件大小和修改时间
                if (fileInfo.Length != cacheEntry.AssemblyInfo.FileSize ||
                    fileInfo.LastWriteTimeUtc != cacheEntry.AssemblyInfo.LastModifiedTime)
                {
                    return false;
                }
                
                // 检查校验和
                var currentChecksum = await CalculateFileChecksumAsync(assemblyPath);
                if (currentChecksum != cacheEntry.AssemblyInfo.Checksum)
                    return false;
                
                // 检查依赖项
                if (cacheEntry.Dependencies != null)
                {
                    foreach (var dependency in cacheEntry.Dependencies)
                    {
                        if (!await IsDependencyValidAsync(dependency))
                            return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "检查缓存有效性失败: {CacheKey}", cacheEntry?.CacheKey);
                return false;
            }
        }

        /// <summary>
        /// 加载持久化缓存
        /// </summary>
        private Task LoadPersistedCache()
        {
            if (string.IsNullOrEmpty(_cachePersistencePath) || !Directory.Exists(_cachePersistencePath))
                return Task.CompletedTask;
            
            try
            {
                var cacheFiles = Directory.GetFiles(_cachePersistencePath, "*.cache");
                var loadTasks = cacheFiles.Select(LoadCacheFileAsync).ToArray();
                Task.WaitAll(loadTasks);
                
                _logger?.LogInformation("加载持久化缓存完成，共加载 {Count} 个缓存文件", cacheFiles.Length);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "加载持久化缓存失败");
            }
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步加载缓存文件
        /// </summary>
        private async Task LoadCacheFileAsync(string cacheFile)
        {
            try
            {
                // 使用Task.Run包装同步方法
                var json = await Task.Run(() => File.ReadAllText(cacheFile));
                var cacheData = JsonConvert.DeserializeObject<dynamic>(json);
                
                var cacheKey = cacheData.CacheKey.ToString();
                var createdTime = DateTime.Parse(cacheData.CreatedTime.ToString());
                var assemblyInfo = JsonConvert.DeserializeObject<AssemblyMetadata>(cacheData.AssemblyInfo.ToString());
                var dependencies = JsonConvert.DeserializeObject<List<AssemblyDependency>>(cacheData.Dependencies.ToString());
                
                var scanResults = new Dictionary<CommandId, Type>();
                if (cacheData.ScanResults != null)
                {
                    foreach (var kvp in cacheData.ScanResults)
                    {
                        var commandId = CommandId.FromUInt16(ushort.Parse(kvp.Name));
                        var typeName = kvp.Value.ToString();
                        var type = Type.GetType(typeName);
                        if (type != null)
                        {
                            scanResults[commandId] = type;
                        }
                    }
                }
                
                var cacheEntry = new ScanCacheEntry
                {
                    CacheKey = cacheKey,
                    CreatedTime = createdTime,
                    LastAccessTime = DateTime.UtcNow,
                    AccessCount = 0,
                    IsIncremental = bool.Parse(cacheData.IsIncremental.ToString()),
                    AssemblyInfo = assemblyInfo,
                    Dependencies = dependencies,
                    ScanResults = scanResults
                };
                
                _scanResultCache.TryAdd(cacheKey, cacheEntry);
                _assemblyMetadataCache.TryAdd(assemblyInfo.Name, assemblyInfo);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "加载缓存文件失败: {CacheFile}", cacheFile);
            }
        }

        /// <summary>
        /// 异步持久化缓存
        /// </summary>
        private async Task PersistCacheAsync(string cacheKey, ScanCacheEntry cacheEntry)
        {
            if (string.IsNullOrEmpty(_cachePersistencePath)) return;
            
            try
            {
                var cacheFile = Path.Combine(_cachePersistencePath, $"{cacheKey}.cache");
                var cacheData = new
                {
                    CacheKey = cacheEntry.CacheKey,
                    CreatedTime = cacheEntry.CreatedTime,
                    AssemblyInfo = cacheEntry.AssemblyInfo,
                    Dependencies = cacheEntry.Dependencies,
                    IsIncremental = cacheEntry.IsIncremental,
                    ScanResults = cacheEntry.ScanResults.ToDictionary(kvp => kvp.Key.ToString(), kvp => kvp.Value.AssemblyQualifiedName)
                };
                
                var json = JsonConvert.SerializeObject(cacheData, Formatting.Indented);
                // 使用Task.Run包装同步方法
                await Task.Run(() => File.WriteAllText(cacheFile, json));
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "持久化缓存失败: {CacheKey}", cacheKey);
            }
        }

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private void CleanupExpiredCache(object state)
        {
            try
            {
                var expiredKeys = new List<string>();
                var cutoffTime = DateTime.UtcNow.AddHours(-24); // 24小时未访问的缓存
                
                foreach (var kvp in _scanResultCache)
                {
                    if (kvp.Value.LastAccessTime < cutoffTime)
                    {
                        expiredKeys.Add(kvp.Key);
                    }
                }
                
                // 移除过期缓存
                foreach (var key in expiredKeys)
                {
                    _scanResultCache.TryRemove(key, out _);
                    
                    // 删除持久化文件
                    var cacheFile = Path.Combine(_cachePersistencePath, $"{key}.cache");
                    if (File.Exists(cacheFile))
                    {
                        File.Delete(cacheFile);
                    }
                }
                
                if (expiredKeys.Count > 0)
                {
                    _logger?.LogInformation("清理过期缓存完成: 清理 {Count} 个条目", expiredKeys.Count);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理过期缓存失败");
            }
        }

        /// <summary>
        /// 缓存命令处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task CacheCommandHandlerAsync(Type handlerType, CancellationToken cancellationToken = default)
        {
            if (handlerType == null)
            {
                _logger?.LogWarning("尝试缓存null处理器类型");
                return;
            }

            try
            {
                var cacheKey = $"handler_{handlerType.FullName}";
                var assembly = handlerType.Assembly;
                var assemblyName = assembly.GetName().Name;
                var version = assembly.GetName().Version?.ToString() ?? "unknown";
                
                // 创建处理器缓存条目
                var handlerCacheEntry = new HandlerCacheEntry
                {
                    CacheKey = cacheKey,
                    HandlerType = handlerType,
                    HandlerName = handlerType.Name,
                    AssemblyName = assemblyName,
                    CreatedTime = DateTime.UtcNow,
                    LastAccessTime = DateTime.UtcNow,
                    AccessCount = 0
                };

                // 缓存处理器类型
                _handlerTypeCache.AddOrUpdate(cacheKey, handlerCacheEntry, (key, existing) => handlerCacheEntry);
                
                // 同时缓存到程序集级别的处理器列表
                var assemblyHandlersKey = $"assembly_handlers_{assemblyName}_{version}";
                _assemblyHandlerCache.AddOrUpdate(assemblyHandlersKey, 
                    new List<HandlerCacheEntry> { handlerCacheEntry },
                    (key, existingList) =>
                    {
                        if (!existingList.Any(h => h.HandlerType == handlerType))
                        {
                            existingList.Add(handlerCacheEntry);
                        }
                        return existingList;
                    });

                _logger?.LogDebug("处理器类型 {HandlerType} 已缓存", handlerType.Name);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "缓存处理器类型 {HandlerType} 失败", handlerType.Name);
            }
        }

        /// <summary>
        /// 获取缓存的处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>处理器缓存条目</returns>
        public HandlerCacheEntry GetCachedHandler(Type handlerType)
        {
            if (handlerType == null) return null;
            
            var cacheKey = $"handler_{handlerType.FullName}";
            if (_handlerTypeCache.TryGetValue(cacheKey, out var cachedHandler))
            {
                cachedHandler.LastAccessTime = DateTime.UtcNow;
                cachedHandler.AccessCount++;
                return cachedHandler;
            }
            return null;
        }

        /// <summary>
        /// 获取程序集的所有缓存处理器
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>处理器缓存条目列表</returns>
        public List<HandlerCacheEntry> GetAssemblyCachedHandlers(Assembly assembly)
        {
            if (assembly == null) return new List<HandlerCacheEntry>();
            
            var assemblyName = assembly.GetName().Name;
            var version = assembly.GetName().Version?.ToString() ?? "unknown";
            var assemblyHandlersKey = $"assembly_handlers_{assemblyName}_{version}";
            
            if (_assemblyHandlerCache.TryGetValue(assemblyHandlersKey, out var handlers))
            {
                foreach (var handler in handlers)
                {
                    handler.LastAccessTime = DateTime.UtcNow;
                    handler.AccessCount++;
                }
                return handlers;
            }
            return new List<HandlerCacheEntry>();
        }

        /// <summary>
        /// 缓存预热
        /// </summary>
        private void WarmupCache(object state)
        {
            try
            {
                _logger?.LogInformation("开始缓存预热...");
                
                // 预热常用程序集的缓存
                var commonAssemblies = new[]
                {
                    typeof(CommandCacheManager).Assembly,
                    typeof(ICommand).Assembly
                };
                
                foreach (var assembly in commonAssemblies)
                {
                    Task.Run(async () =>
                    {
                        await GetOrCreateScanCacheAsync(assembly, null, false);
                    });
                }
                
                _logger?.LogInformation("缓存预热完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "缓存预热失败");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成缓存键
        /// </summary>
        private string GenerateCacheKey(Assembly assembly, string namespaceFilter)
        {
            var assemblyName = assembly.GetName().Name;
            var version = assembly.GetName().Version?.ToString() ?? "unknown";
            var nsPart = string.IsNullOrEmpty(namespaceFilter) ? "all" : namespaceFilter.Replace('.', '_');
            return $"{assemblyName}_{version}_{nsPart}";
        }

        /// <summary>
        /// 获取程序集元数据
        /// </summary>
        private async Task<AssemblyMetadata> GetAssemblyMetadataAsync(Assembly assembly)
        {
            var name = assembly.GetName();
            var location = assembly.Location;
            
            var fileInfo = new FileInfo(location);
            var checksum = await CalculateFileChecksumAsync(location);
            
            return new AssemblyMetadata
            {
                Name = name.Name,
                Version = name.Version?.ToString(),
                FilePath = location,
                FileSize = fileInfo.Length,
                LastModifiedTime = fileInfo.LastWriteTimeUtc,
                Checksum = checksum,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// 计算文件校验和
        /// </summary>
        private async Task<string> CalculateFileChecksumAsync(string filePath)
        {
            try
            {
                using (var sha256 = SHA256.Create())
                using (var stream = File.OpenRead(filePath))
                {
                    // 使用同步方法计算哈希，因为SHA256没有ComputeHashAsync
                    var hash = await Task.Run(() => sha256.ComputeHash(stream));
                    return Convert.ToBase64String(hash);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 检查是否可以使用增量扫描
        /// </summary>
        private async Task<bool> CanUseIncrementalScanAsync(AssemblyMetadata metadata)
        {
            if (metadata == null) return false;
            
            // 检查是否有之前的扫描记录
            if (!_lastScanTime.TryGetValue(metadata.Name, out var lastScanTime))
                return false;
            
            // 检查程序集是否被修改
            var assemblyFile = new FileInfo(metadata.FilePath);
            if (assemblyFile.LastWriteTimeUtc > lastScanTime)
                return false;
            
            // 检查依赖项是否被修改
            if (_assemblyMetadataCache.TryGetValue(metadata.Name, out var cachedMetadata))
            {
                if (cachedMetadata.LastModifiedTime != metadata.LastModifiedTime)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// 获取程序集依赖项
        /// </summary>
        private async Task<List<AssemblyDependency>> GetAssemblyDependenciesAsync(Assembly assembly)
        {
            var dependencies = new List<AssemblyDependency>();
            
            try
            {
                var referencedAssemblies = assembly.GetReferencedAssemblies();
                foreach (var referencedAssembly in referencedAssemblies)
                {
                    try
                    {
                        var depAssembly = Assembly.Load(referencedAssembly);
                        var depMetadata = await GetAssemblyMetadataAsync(depAssembly);
                        
                        dependencies.Add(new AssemblyDependency
                        {
                            Name = referencedAssembly.Name,
                            Version = referencedAssembly.Version?.ToString(),
                            FilePath = depMetadata.FilePath,
                            Checksum = depMetadata.Checksum,
                            LastModifiedTime = depMetadata.LastModifiedTime
                        });
                    }
                    catch
                    {
                        // 忽略无法加载的依赖项
                    }
                }
            }
            catch
            {
                // 忽略获取依赖项时的错误
            }
            
            return dependencies;
        }

        /// <summary>
        /// 检查依赖项是否有效
        /// </summary>
        private async Task<bool> IsDependencyValidAsync(AssemblyDependency dependency)
        {
            if (dependency == null) return false;
            
            try
            {
                if (!File.Exists(dependency.FilePath))
                    return false;
                
                var fileInfo = new FileInfo(dependency.FilePath);
                if (fileInfo.LastWriteTimeUtc != dependency.LastModifiedTime)
                    return false;
                
                var currentChecksum = await CalculateFileChecksumAsync(dependency.FilePath);
                if (currentChecksum != dependency.Checksum)
                    return false;
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 计算总缓存大小
        /// </summary>
        private long CalculateTotalCacheSize()
        {
            return _commandTypeCache.Count + 
                   _constructorCache.Count + _commandCreatorCache.Count + 
                   _handlerTypeCache.Count + _scanResultCache.Count + 
                   _assemblyMetadataCache.Count;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void ClearAllCaches()
        {
            // 清空基础缓存
            _commandTypeCache.Clear();
            _constructorCache.Clear();
            _commandCreatorCache.Clear();
            _handlerTypeCache.Clear();
            
            // 清空增强缓存
            _scanResultCache.Clear();
            _assemblyMetadataCache.Clear();
            _lastScanTime.Clear();
            _assemblyChecksums.Clear();
            
            // 重置统计
            _cacheHits = 0;
            _cacheMisses = 0;
            _incrementalScans = 0;
            _fullScans = 0;
            
            _logger?.LogInformation("已清空所有命令缓存");
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public UnifiedCacheStatistics GetCacheStatistics()
        {
            var totalAccesses = _cacheHits + _cacheMisses;
            
            return new UnifiedCacheStatistics
            {
                // 基础统计
                CommandTypeCacheCount = _commandTypeCache.Count,
                ConstructorCacheCount = _constructorCache.Count,
                CommandCreatorCacheCount = _commandCreatorCache.Count,
                HandlerTypeCacheCount = _handlerTypeCache.Count,
                
                // 增强统计
                ScanResultCacheCount = _scanResultCache.Count,
                AssemblyMetadataCacheCount = _assemblyMetadataCache.Count,
                CacheHits = _cacheHits,
                CacheMisses = _cacheMisses,
                CacheHitRate = totalAccesses > 0 ? (double)_cacheHits / totalAccesses : 0,
                IncrementalScans = _incrementalScans,
                FullScans = _fullScans,
                LastScanTime = _lastScanTime.Values.Any() ? _lastScanTime.Values.Max() : DateTime.MinValue,
                CachePersistencePath = _cachePersistencePath,
                TotalCacheSize = CalculateTotalCacheSize(),
                LastUpdateTime = DateTime.UtcNow
            };
        }

        #endregion

        #region 内部类

        /// <summary>
        /// 扫描缓存条目
        /// </summary>
        public class ScanCacheEntry
        {
            public string CacheKey { get; set; }
            public Dictionary<CommandId, Type> ScanResults { get; set; }
            public AssemblyMetadata AssemblyInfo { get; set; }
            public DateTime CreatedTime { get; set; }
            public DateTime LastAccessTime { get; set; }
            public long AccessCount { get; set; }
            public bool IsIncremental { get; set; }
            public List<AssemblyDependency> Dependencies { get; set; }
        }

        /// <summary>
        /// 处理器缓存条目
        /// </summary>
        public class HandlerCacheEntry
        {
            public string CacheKey { get; set; }
            public Type HandlerType { get; set; }
            public string HandlerName { get; set; }
            public string AssemblyName { get; set; }
            public DateTime CreatedTime { get; set; }
            public DateTime LastAccessTime { get; set; }
            public long AccessCount { get; set; }
        }

        /// <summary>
        /// 程序集元数据
        /// </summary>
        public class AssemblyMetadata
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string FilePath { get; set; }
            public long FileSize { get; set; }
            public DateTime LastModifiedTime { get; set; }
            public string Checksum { get; set; }
            public DateTime Timestamp { get; set; }
        }

        /// <summary>
        /// 程序集依赖项
        /// </summary>
        public class AssemblyDependency
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string FilePath { get; set; }
            public string Checksum { get; set; }
            public DateTime LastModifiedTime { get; set; }
        }

        /// <summary>
        /// 统一缓存统计信息
        /// </summary>
        public class UnifiedCacheStatistics
        {
            // 基础统计
            public int CommandTypeCacheCount { get; set; }
            public int CommandTypeByNameCacheCount { get; set; }
            public int ConstructorCacheCount { get; set; }
            public int CommandCreatorCacheCount { get; set; }
            public int HandlerTypeCacheCount { get; set; }
            
            // 增强统计
            public int ScanResultCacheCount { get; set; }
            public int AssemblyMetadataCacheCount { get; set; }
            public long CacheHits { get; set; }
            public long CacheMisses { get; set; }
            public double CacheHitRate { get; set; }
            public long IncrementalScans { get; set; }
            public long FullScans { get; set; }
            public DateTime LastScanTime { get; set; }
            public string CachePersistencePath { get; set; }
            public long TotalCacheSize { get; set; }
            public DateTime LastUpdateTime { get; set; }
        }

        #endregion
    }
}
