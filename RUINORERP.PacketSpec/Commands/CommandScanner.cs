using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 文件级别注释：
    /// 统一命令扫描器 - 企业级命令和处理器扫描管理器（第二阶段优化版）
    /// 
    /// 职责（优化后）：
    /// 1. 专注于类型发现 - 扫描程序集中的命令类型（ICommand）和命令处理器（ICommandHandler）
    /// 2. 提供高效的扫描结果缓存和查询功能
    /// 3. 支持扫描和智能缓存失效
    /// 4. 提供扫描统计和性能监控
    /// 
    /// 设计目标（第二阶段）：
    /// - 分离扫描和注册职责，CommandScanner专注于类型发现
    /// - 创建独立的CommandRegistry管理注册和生命周期
    /// - 实现延迟注册机制，按需注册类型
    /// - 增强缓存策略，避免重复扫描
    /// - 提供缓存预热机制，提升启动性能
    /// 
    /// 工作流程（优化后）：
    /// 1. CommandScanner负责扫描和发现类型，建立扫描结果缓存
    /// 2. CommandRegistry负责注册和管理类型生命周期
    /// 3. 支持按需注册和延迟注册机制
    /// 5. 缓存预热机制提升系统响应速度
    /// </summary>
    public class CommandScanner
    {
        private readonly ILogger<CommandScanner> _logger;
        private readonly CommandCacheManager _cacheManager;

        // 最后扫描时间记录
        private readonly ConcurrentDictionary<string, DateTime> _lastScanTime;
        
        // 扫描统计
        private readonly ConcurrentDictionary<string, ScanStatistics> _scanStatistics;
        
        // 处理器映射 - 用于管理命令到处理器的映射关系
        private readonly ConcurrentDictionary<CommandId, List<ICommandHandler>> _commandHandlerMap;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheManager">命令缓存管理器</param>
        /// <param name="logger">日志记录器，可选参数</param>
        public CommandScanner(CommandCacheManager cacheManager = null, ILogger<CommandScanner> logger = null)
        {
            _logger = logger;
            _cacheManager = cacheManager ?? new CommandCacheManager();

            // 初始化扫描时间和统计缓存
            _lastScanTime = new ConcurrentDictionary<string, DateTime>();
            _scanStatistics = new ConcurrentDictionary<string, ScanStatistics>();
            
            // 初始化处理器映射
            _commandHandlerMap = new ConcurrentDictionary<CommandId, List<ICommandHandler>>();
        }

        #region 扫描统计类

        /// <summary>
        /// 扫描统计信息
        /// </summary>
        public class ScanStatistics
        {
            /// <summary>
            /// 扫描的程序集名称
            /// </summary>
            public string AssemblyName { get; set; }

            /// <summary>
            /// 扫描时间
            /// </summary>
            public DateTime ScanTime { get; set; }

            /// <summary>
            /// 发现的命令数量
            /// </summary>
            public int CommandsFound { get; set; }

            /// <summary>
            /// 发现的处理器数量
            /// </summary>
            public int HandlersFound { get; set; }

            /// <summary>
            /// 扫描耗时（毫秒）
            /// </summary>
            public long ScanDurationMs { get; set; }

            /// <summary>
            /// 是否扫描成功
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// 错误信息
            /// </summary>
            public string ErrorMessage { get; set; }
        }

        #endregion

        #region 扫描结果类

        /// <summary>
        /// 扫描结果类 - 包含发现的类型信息
        /// </summary>
        public class ScanResult
        {
            /// <summary>
            /// 程序集名称
            /// </summary>
            public string AssemblyName { get; set; }
            
            /// <summary>
            /// 发现的命令类型
            /// </summary>
            public Dictionary<CommandId, Type> CommandTypes { get; set; }
            
            /// <summary>
            /// 发现的处理器类型
            /// </summary>
            public List<Type> HandlerTypes { get; set; }
            
            /// <summary>
            /// 扫描时间
            /// </summary>
            public DateTime ScanTime { get; set; }
            
            /// <summary>
            /// 扫描耗时（毫秒）
            /// </summary>
            public long ScanDurationMs { get; set; }
            
            /// <summary>
            /// 程序集元数据
            /// </summary>
            public CommandCacheManager.AssemblyMetadata AssemblyMetadata { get; set; }
        }

        #endregion

        #region 增强扫描方法（第二阶段优化）

        /// <summary>
        /// 智能扫描程序集 - 扫描和缓存
        /// </summary>
        /// <param name="assembly">要扫描的程序集</param>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="forceFullScan">强制全量扫描</param>
        /// <returns>扫描结果</returns>
        public async Task<ScanResult> ScanAssemblySmartAsync(Assembly assembly, string namespaceFilter = null, bool forceFullScan = false)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            
            var assemblyName = assembly.GetName().Name;
            var stopwatch = Stopwatch.StartNew();
            
            try
            {
                _logger?.LogInformation("开始智能扫描程序集: {AssemblyName} (全量扫描: {ForceFullScan})", assemblyName, forceFullScan);
                
                // 尝试从增强缓存获取扫描结果
                var cacheEntry = await _cacheManager.GetOrCreateScanCacheAsync(assembly, namespaceFilter, forceFullScan);
                
                if (cacheEntry?.ScanResults != null && cacheEntry.ScanResults.Count > 0)
                {
                    // 使用缓存的扫描结果
                    var scanResult = new ScanResult
                    {
                        AssemblyName = assemblyName,
                        CommandTypes = new Dictionary<CommandId, Type>(cacheEntry.ScanResults),
                        HandlerTypes = new List<Type>(), // 处理器类型需要单独扫描
                        ScanTime = DateTime.Now,
                        ScanDurationMs = stopwatch.ElapsedMilliseconds,
                        AssemblyMetadata = cacheEntry.AssemblyInfo
                    };
                    
                    // 扫描处理器类型
                    scanResult.HandlerTypes = ScanHandlerTypes(assembly, namespaceFilter);
                    
                    // 更新扫描统计
                    UpdateScanStatistics(assemblyName, scanResult.CommandTypes.Count, scanResult.HandlerTypes.Count, true, null);
                    
                    _logger?.LogInformation("智能扫描完成（使用缓存）: {AssemblyName}, 发现 {CommandCount} 个命令, {HandlerCount} 个处理器",
                        assemblyName, scanResult.CommandTypes.Count, scanResult.HandlerTypes.Count);
                    
                    return scanResult;
                }
                
                // 执行实际扫描
                var result = await PerformActualScanAsync(assembly, namespaceFilter, forceFullScan);
                result.ScanDurationMs = stopwatch.ElapsedMilliseconds;
                
                // 更新最后扫描时间
                _lastScanTime.AddOrUpdate(assemblyName, DateTime.Now, (key, old) => DateTime.Now);
                
                _logger?.LogInformation("智能扫描完成（实际扫描）: {AssemblyName}, 发现 {CommandCount} 个命令, {HandlerCount} 个处理器",
                    assemblyName, result.CommandTypes.Count, result.HandlerTypes.Count);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "智能扫描程序集失败: {AssemblyName}", assemblyName);
                UpdateScanStatistics(assemblyName, 0, 0, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 执行实际扫描
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="forceFullScan">强制全量扫描</param>
        /// <returns>扫描结果</returns>
        private async Task<ScanResult> PerformActualScanAsync(Assembly assembly, string namespaceFilter, bool forceFullScan)
        {
            var result = new ScanResult
            {
                AssemblyName = assembly.GetName().Name,
                CommandTypes = new Dictionary<CommandId, Type>(),
                HandlerTypes = new List<Type>(),
                ScanTime = DateTime.Now,
        
            };
            
            try
            {
                // 获取程序集中的所有类型
                var types = GetTypesFromAssembly(assembly, namespaceFilter);
                
                // 扫描命令类型
                foreach (var type in types)
                {
                    if (IsCommandType(type))
                    {
                        var commandId = ExtractCommandIdFromType(type);
                    if (commandId != CommandId.Empty)
                    {
                        result.CommandTypes[commandId] = type;
                    }
                    }
                    else if (IsHandlerType(type))
                    {
                        result.HandlerTypes.Add(type);
                    }
                }
                
                // 缓存扫描结果到增强缓存管理器
                var scanResults = result.CommandTypes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                await _cacheManager.CacheScanResultsAsync(assembly, scanResults);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行实际扫描失败: {AssemblyName}", assembly.GetName().Name);
                throw;
            }
        }

        /// <summary>
        /// 批量扫描多个程序集
        /// </summary>
        /// <param name="assemblies">程序集数组</param>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <param name="forceFullScan">强制全量扫描</param>
        /// <returns>扫描结果数组</returns>
        public async Task<ScanResult[]> ScanAssembliesAsync(Assembly[] assemblies, string namespaceFilter = null, bool forceFullScan = false)
        {
            if (assemblies == null || assemblies.Length == 0)
                return Array.Empty<ScanResult>();
            
            var tasks = assemblies.Select(assembly => ScanAssemblySmartAsync(assembly, namespaceFilter, forceFullScan));
            return await Task.WhenAll(tasks);
        }

         

        /// <summary>
        /// 检查是否需要重新扫描程序集
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <returns>是否需要重新扫描</returns>
        private async Task<bool> ShouldRescanAssemblyAsync(Assembly assembly)
        {
            var assemblyName = assembly.GetName().Name;
            
            // 检查缓存管理器中是否有该程序集的扫描记录
            var cacheEntry = await _cacheManager.GetOrCreateScanCacheAsync(assembly, null, false);
            if (cacheEntry == null)
                return true;
            
            // 如果缓存管理器认为需要重新扫描，则返回true
            return cacheEntry?.IsIncremental == false;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 从程序集获取类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <returns>类型数组</returns>
        private Type[] GetTypesFromAssembly(Assembly assembly, string namespaceFilter)
        {
            try
            {
                var types = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface && 
                               // 允许带有CommandHandler特性的泛型类型定义被扫描到
                               (!t.IsGenericTypeDefinition || t.GetCustomAttributes(typeof(CommandHandlerAttribute), false).Any()));
                
                if (!string.IsNullOrEmpty(namespaceFilter))
                {
                    types = types.Where(t => t.Namespace?.StartsWith(namespaceFilter) == true);
                }
                
                return types.ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // 减少警告日志，仅在调试时记录详细信息
                #if DEBUG
                _logger?.LogWarning(ex, "加载程序集类型失败: {AssemblyName}, 部分类型可能不可用", assembly.GetName().Name);
                #else
                _logger?.LogWarning("加载程序集类型失败: {AssemblyName}, 部分类型可能不可用", assembly.GetName().Name);
                #endif
                return ex.Types?.Where(t => t != null).ToArray() ?? Array.Empty<Type>();
            }
        }

        /// <summary>
        /// 判断是否为命令类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为命令类型</returns>
        private bool IsCommandType(Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 判断是否为处理器类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为处理器类型</returns>
        private bool IsHandlerType(Type type)
        {
            return typeof(ICommandHandler).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 扫描处理器类型
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="namespaceFilter">命名空间过滤器</param>
        /// <returns>处理器类型列表</returns>
        private List<Type> ScanHandlerTypes(Assembly assembly, string namespaceFilter)
        {
            var types = GetTypesFromAssembly(assembly, namespaceFilter);
            return types.Where(IsHandlerType).ToList();
        }

        /// <summary>
        /// 更新扫描统计
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="commandCount">命令数量</param>
        /// <param name="handlerCount">处理器数量</param>
        /// <param name="success">是否成功</param>
        /// <param name="errorMessage">错误信息</param>
        private void UpdateScanStatistics(string assemblyName, int commandCount, int handlerCount, bool success, string errorMessage)
        {
            var stats = new ScanStatistics
            {
                AssemblyName = assemblyName,
                ScanTime = DateTime.Now,
                CommandsFound = commandCount,
                HandlersFound = handlerCount,
                Success = success,
                ErrorMessage = errorMessage
            };
            
            _scanStatistics.AddOrUpdate(assemblyName, stats, (key, old) => stats);
        }

        #endregion

        /// <summary>
        /// 根据类型名称获取命令类型
        /// </summary>
        /// <param name="typeName">类型名称（完整名称或短名称）</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            // 从缓存管理器中获取所有缓存的命令类型
            var cachedCommandTypes = _cacheManager.GetAllCachedCommandTypes();
            foreach (var commandType in cachedCommandTypes)
            {
                if (commandType.FullName == typeName || commandType.Name == typeName)
                {
                    return commandType;
                }
            }

            // 尝试从当前应用程序域中查找类型
            try
            {
                var foundType = Type.GetType(typeName);
                if (foundType != null && typeof(ICommand).IsAssignableFrom(foundType))
                {
                    return foundType;
                }
            }
            catch
            {
                // 忽略类型查找异常
            }

            return null;
        }

        /// <summary>
        /// 获取所有已发现的命令类型
        /// </summary>
        /// <returns>命令类型字典（命令代码 -> 类型）</returns>
        public IReadOnlyDictionary<CommandId, Type> GetAllCommandTypes()
        {
            var allTypes = new Dictionary<CommandId, Type>();
            
            // 从缓存管理器中获取所有缓存的命令类型
            var cachedCommandTypes = _cacheManager.GetAllCachedCommandTypes();
            foreach (var commandType in cachedCommandTypes)
            {
                // 从类型中提取命令ID
                var commandId = ExtractCommandIdFromType(commandType);
                if (commandId != CommandId.Empty)
                {
                    allTypes[commandId] = commandType;
                }
            }
            
            return allTypes;
        }

 
 
        /// <summary>
        /// 清理扫描结果和统计
        /// </summary>
        public void Clear()
        {
            var commandCount = _cacheManager.GetAllCachedCommandTypes().Count;
            var handlerCount = _cacheManager.GetAllCachedHandlerTypes().Count;
            
            _lastScanTime.Clear();
            _scanStatistics.Clear();

            _logger?.LogInformation("清理所有扫描结果，共清理 {CommandCount} 个命令类型和 {HandlerCount} 个处理器类型", 
                commandCount, handlerCount);
        }

        #region 类型发现和辅助方法

        /// <summary>
        /// 更新扫描统计信息
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="commandCount">命令类型数量</param>
        /// <param name="handlerCount">处理器类型数量</param>
        private void UpdateScanStatistics(string assemblyName, int commandCount, int handlerCount)
        {
            var stats = new ScanStatistics
            {
                AssemblyName = assemblyName,
                CommandsFound = commandCount,
                HandlersFound = handlerCount,
                ScanTime = DateTime.Now,
                Success = true
            };

            _scanStatistics[assemblyName] = stats;
        }

        /// <summary>
        /// 获取指定程序集的最后扫描时间
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns>最后扫描时间，如果未扫描过则返回null</returns>
        public DateTime? GetLastScanTime(string assemblyName)
        {
            return _lastScanTime.TryGetValue(assemblyName, out var time) ? time : (DateTime?)null;
        }

        /// <summary>
        /// 检查程序集是否需要重新扫描
        /// </summary>
        /// <param name="assembly">程序集</param>
        /// <param name="minInterval">最小扫描间隔</param>
        /// <returns>是否需要重新扫描</returns>
        public bool NeedsRescan(Assembly assembly, TimeSpan minInterval)
        {
            if (assembly == null)
                return false;

            var assemblyName = assembly.GetName().Name;
            var lastScan = GetLastScanTime(assemblyName);
            
            if (!lastScan.HasValue)
                return true;

            return DateTime.Now - lastScan.Value >= minInterval;
        }

    
        /// <summary>
        /// 根据命令ID获取命令类型（优化：直接使用缓存）
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>命令类型，如果找不到则返回null</returns>
        public Type GetCommandType(CommandId commandId)
        {
            // 直接使用缓存管理器的GetCachedCommandType方法，避免遍历所有类型
            return _cacheManager.GetCachedCommandType(commandId);
        }

        /// <summary>
        /// 获取缓存管理器
        /// </summary>
        /// <returns>命令缓存管理器</returns>
        public CommandCacheManager GetCacheManager()
        {
            return _cacheManager;
        }


        /// <summary>
        /// 从类型中提取命令ID - 高性能版本（避免重复创建实例）
        /// </summary>
        private CommandId ExtractCommandIdFromType(Type commandType)
        {
            if (commandType == null)
                throw new ArgumentNullException(nameof(commandType));

            try
            {
                // 如果是泛型类型定义，直接使用类型名称哈希作为备用方案
                if (commandType.IsGenericTypeDefinition)
                {
                    _logger?.LogDebug("泛型类型定义 {TypeName} 使用类型名称哈希作为命令ID", commandType.Name);
                    uint tempId = (uint)(commandType.FullName?.GetHashCode() ?? commandType.Name.GetHashCode());
                    if (tempId == 0 || tempId == uint.MaxValue)
                    {
                        tempId = (uint)commandType.Name.GetHashCode();
                    }
                    return CommandId.FromUInt16((ushort)tempId);
                }

                // 优先使用静态字段或属性获取命令ID，避免创建实例
                
                // 1. 查找静态的 CommandId 常量字段
                var constFields = commandType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(f => f.FieldType == typeof(CommandId) && f.IsLiteral);
                
                foreach (var field in constFields)
                {
                    var value = field.GetValue(null);
                    if (value is CommandId commandId)
                    {
                        return commandId;
                    }
                }

                // 2. 查找静态属性
                var staticProps = commandType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(p => p.PropertyType == typeof(CommandId) && p.CanRead);
                
                foreach (var prop in staticProps)
                {
                    try
                    {
                        var value = prop.GetValue(null);
                        if (value is CommandId commandId)
                        {
                            return commandId;
                        }
                    }
                    catch
                    {
                        // 忽略属性访问异常
                    }
                }

                // 3. 查找静态的 DefaultInstance 或 Instance 属性
                var instanceProp = commandType.GetProperty("DefaultInstance", BindingFlags.Public | BindingFlags.Static) ??
                                   commandType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                
                if (instanceProp?.CanRead == true)
                {
                    try
                    {
                        var instance = instanceProp.GetValue(null);
                        if (instance is ICommand commandInstance)
                        {
                            return commandInstance.CommandIdentifier;
                        }
                    }
                    catch
                    {
                        // 忽略实例访问异常
                    }
                }

                // 4. 最后才尝试创建实例（带缓存）
                return CreateInstanceAndGetCommandId(commandType);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "从类型 {TypeName} 提取命令ID失败，使用类型名称哈希", commandType.Name);
                
                // 备用方案：使用类型名称哈希
                uint tempId = (uint)(commandType.FullName?.GetHashCode() ?? commandType.Name.GetHashCode());
                if (tempId == 0 || tempId == uint.MaxValue)
                {
                    tempId = (uint)commandType.Name.GetHashCode();
                }
                return CommandId.FromUInt16((ushort)tempId);
            }
        }

        /// <summary>
        /// 创建实例获取命令ID（带异常处理和缓存）
        /// </summary>
        private CommandId CreateInstanceAndGetCommandId(Type commandType)
        {
            // 检查是否是泛型类型定义
            if (commandType.IsGenericTypeDefinition)
            {
                _logger?.LogDebug("泛型类型定义 {TypeName} 使用类型名称哈希作为命令ID", commandType.Name);
                uint tempId = (uint)(commandType.FullName?.GetHashCode() ?? commandType.Name.GetHashCode());
                if (tempId == 0 || tempId == uint.MaxValue)
                {
                    tempId = (uint)commandType.Name.GetHashCode();
                }
                return CommandId.FromUInt16((ushort)tempId);
            }

            // 检查是否有无参构造函数
            var ctor = commandType.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException($"类型 {commandType.Name} 没有无参构造函数");
            }

            try
            {
                // 使用表达式树编译创建实例，比Activator.CreateInstance更快
                var lambda = Expression.Lambda<Func<ICommand>>(Expression.New(ctor)).Compile();
                var instance = lambda();
                return instance.CommandIdentifier;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "创建 {TypeName} 实例获取命令ID失败", commandType.Name);
                throw;
            }
        }

        /// <summary>
        /// 内部统一扫描方法 - 同时处理指令和处理器
        /// </summary>
        private (List<Type> CommandTypes, List<Type> HandlerTypes) ScanAssemblyInternal(Assembly assembly, string namespaceFilter)
        {
            var commandTypes = new List<Type>();
            var handlerTypes = new List<Type>();

            try
            {
                // 获取程序集类型（处理加载异常）
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                    _logger?.LogWarning("程序集 {Assembly} 部分类型加载失败", assembly.GetName().Name);
                }

                foreach (var type in types)
                {
                    // 跳过无效类型，但允许带有CommandHandlerAttribute的泛型类型定义
                    if (type == null || type.IsAbstract || type.IsInterface)
                        continue;

                    // 命名空间过滤
                    if (!string.IsNullOrEmpty(namespaceFilter) && 
                        (type.Namespace == null || !type.Namespace.StartsWith(namespaceFilter)))
                        continue;

                    // 扫描指令类型
                    if (typeof(ICommand).IsAssignableFrom(type) && !type.IsGenericTypeDefinition)
                    {
                        commandTypes.Add(type);
                    }
                    
                    // 扫描处理器类型
                    if (typeof(ICommandHandler).IsAssignableFrom(type) && 
                        type.GetCustomAttribute<CommandHandlerAttribute>() != null)
                    {
                        handlerTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "扫描程序集 {Assembly} 失败", assembly.GetName().Name);
            }

            return (commandTypes, handlerTypes);
        }

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令类型（仅扫描，不注册）
        /// </summary>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数，只扫描指定命名空间下的命令</param>
        /// <returns>扫描结果</returns>
        public ScanResult ScanCommandsOnly(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var result = new ScanResult
            {
                AssemblyName = assemblies?.FirstOrDefault()?.GetName().Name ?? "Unknown",
                ScanTime = DateTime.Now,
                CommandTypes = new Dictionary<CommandId, Type>(),
                HandlerTypes = new List<Type>()
            };

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var (commandTypes, handlerTypes) = ScanAssemblyInternal(assembly, namespaceFilter);
                    
                    // 处理命令类型
                    foreach (var commandType in commandTypes)
                    {
                        try
                        {
                            var commandId = ExtractCommandIdFromType(commandType);
                            
                            // 检查重复
                            if (!result.CommandTypes.ContainsKey(commandId))
                            {
                                result.CommandTypes[commandId] = commandType;
                            }
                            else
                            {
                                _logger?.LogWarning("发现重复命令ID {CommandId}，类型 {TypeName} 将被忽略", 
                                    commandId, commandType.Name);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogWarning(ex, "处理命令类型 {TypeName} 失败", commandType.FullName);
                        }
                    }
                    
                    // 收集处理器类型
                    result.HandlerTypes.AddRange(handlerTypes);
                    
                    // 更新统计信息
                    UpdateScanStatistics(assembly.GetName().Name, commandTypes.Count, handlerTypes.Count);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "扫描程序集 {AssemblyName} 失败", assembly.FullName);
                }
            }

            _logger?.LogInformation("扫描完成，发现 {CommandCount} 个命令类型和 {HandlerCount} 个处理器类型", 
                result.CommandTypes.Count, result.HandlerTypes.Count);
            
            return result;
        }

        /// <summary>
        /// 扫描并注册命令到命令类型助手（使用CommandRegistry）
        /// </summary>
        /// <param name="registry">命令注册表</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        public async Task ScanAndRegisterCommandsAsync(CommandRegistry registry, string namespaceFilter = null, params Assembly[] assemblies)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var scanResult = ScanCommandsOnly(namespaceFilter, assemblies);
            
            // 注册到命令注册表
            await registry.RegisterCommandsAsync(scanResult.CommandTypes);
            
            _logger?.LogInformation("扫描并注册完成，共注册 {Count} 个命令类型", scanResult.CommandTypes.Count);
        }

        /// <summary>
        /// 扫描当前程序集中的所有命令类型并注册
        /// </summary>
        /// <param name="registry">命令注册表</param>
        public async Task ScanCurrentAssemblyAsync(CommandRegistry registry)
        {
            await ScanAndRegisterCommandsAsync(registry, null, Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 扫描指定程序集中的所有命令类型并注册
        /// </summary>
        /// <param name="registry">命令注册表</param>
        /// <param name="assembly">要扫描的程序集</param>
        public async Task ScanAssemblyAsync(CommandRegistry registry, Assembly assembly)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            await ScanAndRegisterCommandsAsync(registry, null, assembly);
        }

        /// <summary>
        /// 扫描指定命名空间下的所有命令类型并注册
        /// </summary>
        /// <param name="registry">命令注册表</param>
        /// <param name="assembly">要扫描的程序集</param>
        /// <param name="namespacePrefix">命名空间前缀</param>
        public async Task ScanNamespaceAsync(CommandRegistry registry, Assembly assembly, string namespacePrefix)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (string.IsNullOrEmpty(namespacePrefix))
                throw new ArgumentNullException(nameof(namespacePrefix));

            await ScanAndRegisterCommandsAsync(registry, namespacePrefix, assembly);
        }

        /// <summary>
        /// 内部统一扫描方法 - 仅扫描处理器类型
        /// </summary>
        private List<Type> ScanAssemblyForHandlersOnly(Assembly assembly, string namespaceFilter)
        {
            var handlerTypes = new List<Type>();

            try
            {
                // 获取程序集类型（处理加载异常）
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray();
                    _logger?.LogWarning("程序集 {Assembly} 部分类型加载失败", assembly.GetName().Name);
                }

                foreach (var type in types)
                {
                    // 跳过无效类型，但允许带有CommandHandlerAttribute的泛型类型定义
                    if (type == null || type.IsAbstract || type.IsInterface)
                        continue;

                    // 命名空间过滤
                    if (!string.IsNullOrEmpty(namespaceFilter) && 
                        (type.Namespace == null || !type.Namespace.StartsWith(namespaceFilter)))
                        continue;

                    // 扫描处理器类型
                    if (typeof(ICommandHandler).IsAssignableFrom(type) && 
                        type.GetCustomAttribute<CommandHandlerAttribute>() != null)
                    {
                        handlerTypes.Add(type);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "扫描程序集 {Assembly} 中的处理器失败", assembly.GetName().Name);
            }

            return handlerTypes;
        }

        /// <summary>
        /// 扫描并获取指定程序集中的所有处理器类型（仅扫描，不注册）
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <returns>扫描结果</returns>
        public ScanResult ScanHandlersOnly(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var result = new ScanResult
            {
                AssemblyName = assemblies?.FirstOrDefault()?.GetName().Name ?? "Unknown",
                ScanTime = DateTime.Now,
                CommandTypes = new Dictionary<CommandId, Type>(),
                HandlerTypes = new List<Type>()
            };

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var handlerTypes = ScanAssemblyForHandlersOnly(assembly, namespaceFilter);
                    result.HandlerTypes.AddRange(handlerTypes);
                    
                    // 更新统计信息
                    UpdateScanStatistics(assembly.GetName().Name, 0, handlerTypes.Count);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "扫描程序集 {AssemblyName} 中的处理器失败", assembly.FullName);
                }
            }

            _logger?.LogInformation("扫描完成，发现 {HandlerCount} 个处理器类型", 
                result.HandlerTypes.Count);
            
            return result;
        }

        /// <summary>
        /// 扫描并注册处理器到命令注册表
        /// </summary>
        /// <param name="registry">命令注册表</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        public async Task ScanAndRegisterHandlersAsync(CommandRegistry registry, string namespaceFilter = null, params Assembly[] assemblies)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            var scanResult = ScanHandlersOnly(namespaceFilter, assemblies);
            
            // 注册处理器
            await registry.RegisterHandlersAsync(scanResult.HandlerTypes);
            
            _logger?.LogInformation("扫描并注册处理器完成，共注册 {Count} 个处理器类型", scanResult.HandlerTypes.Count);
        }

        #region 命令处理器扫描功能（仅扫描，不注册）

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令处理器类型（仅扫描，不注册）
        /// </summary>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <returns>命令处理器类型列表</returns>
        public List<Type> ScanCommandHandlers(string namespaceFilter = null, params Assembly[] assemblies)
        {
            var handlerTypes = new List<Type>();

            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new[] { Assembly.GetCallingAssembly() };
            }

            foreach (var assembly in assemblies)
            {
                try
                {
                    var (_, handlers) = ScanAssemblyInternal(assembly, namespaceFilter);
                    handlerTypes.AddRange(handlers);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "扫描程序集 {AssemblyName} 中的处理器失败", assembly.FullName);
                }
            }

            _logger?.LogInformation("扫描完成，发现 {Count} 个处理器类型", handlerTypes.Count);
            return handlerTypes;
        }

        /// <summary>
        /// 按优先级排序处理器类型
        /// </summary>
        /// <param name="handlerTypes">处理器类型列表</param>
        /// <returns>排序后的处理器类型列表</returns>
        private List<Type> SortHandlersByPriority(List<Type> handlerTypes)
        {
            return handlerTypes.OrderBy(type =>
            {
                var attr = type.GetCustomAttribute<CommandHandlerAttribute>();
                return attr?.Priority ?? 0;
            }).ToList();
        }

        /// <summary>
        /// 扫描并获取指定程序集中的所有命令处理器类型（仅扫描，不注册）- 已废弃
        /// </summary>
        /// <param name="namespaceFilter">命名空间过滤器，可选参数</param>
        /// <param name="assemblies">要扫描的程序集，可选参数</param>
        /// <returns>处理器类型列表</returns>
        [Obsolete("请使用 ScanCommandHandlers 或 ScanAndRegisterHandlersAsync 方法，此方法只扫描不注册")]
        public List<Type> ScanAndRegisterCommandHandlers(string namespaceFilter = null, params Assembly[] assemblies)
        {
            // 方法名保留兼容性，但实际只扫描不注册
            _logger?.LogWarning("ScanAndRegisterCommandHandlers 方法只扫描不注册，请使用 ScanAndRegisterHandlersAsync 方法进行真正注册");
            return ScanCommandHandlers(namespaceFilter, assemblies);
        }



        #endregion

        #region 处理器映射管理（从CommandDispatcher迁移）

        /// <summary>
        /// 注册处理器到内部映射
        /// </summary>
        /// <param name="handler">处理器实例</param>
        public void RegisterHandlerToMapping(ICommandHandler handler)
        {
            if (handler == null)
            {
                _logger?.LogError("尝试为null处理器更新命令映射");
                return;
            }

            try
            {
                // 安全获取支持的命令列表，避免空引用
                var supportedCommands = handler.SupportedCommands ?? Array.Empty<CommandId>();

                if (!supportedCommands.Any())
                {
                    _logger?.LogWarning($"处理器 {handler.Name} [ID: {handler.HandlerId}] 未定义支持的命令类型");
                    return;
                }

                foreach (var commandCode in supportedCommands)
                {
                    _commandHandlerMap.AddOrUpdate(
                        commandCode,
                        new List<ICommandHandler> { handler },
                        (key, existingHandlers) =>
                        {
                            if (!existingHandlers.Contains(handler))
                            {
                                existingHandlers.Add(handler);
                            }
                            return existingHandlers;
                        });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"更新处理器 {handler.Name} 命令映射时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 从映射中移除处理器
        /// </summary>
        /// <param name="handler">处理器实例</param>
        public void RemoveHandlerFromMapping(ICommandHandler handler)
        {
            if (handler == null)
            {
                _logger?.LogError("尝试从映射中移除null处理器");
                return;
            }

            try
            {
                // 安全获取支持的命令列表，避免空引用
                var supportedCommands = handler.SupportedCommands ?? Array.Empty<CommandId>();

                if (!supportedCommands.Any())
                {
                    _logger?.LogWarning($"处理器 {handler.Name} [ID: {handler.HandlerId}] 未定义支持的命令类型，无法从映射中移除");
                    return;
                }

                foreach (var commandCode in supportedCommands)
                {
                    if (_commandHandlerMap.TryGetValue(commandCode, out var handlers))
                    {
                        handlers.Remove(handler);

                        if (!handlers.Any())
                        {
                            _commandHandlerMap.TryRemove(commandCode, out _);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"从映射中移除处理器 {handler.Name} 时发生异常");
                throw;
            }
        }

        /// <summary>
        /// 根据命令ID获取处理器列表
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>处理器列表，如果没有找到则返回空列表</returns>
        public List<ICommandHandler> GetHandlers(CommandId commandId)
        {
            if (_commandHandlerMap.TryGetValue(commandId, out var handlers))
            {
                return handlers.ToList(); // 返回副本以避免外部修改
            }
            return new List<ICommandHandler>();
        }

        /// <summary>
        /// 获取命令类型的构造函数（优化：使用缓存构造函数）
        /// </summary>
        /// <param name="commandCode">命令代码</param>
        /// <returns>构造函数委托，如果找不到则返回null</returns>
        public Func<ICommand> GetCommandCtor(CommandId commandCode)
        {
            try
            {
                // 获取命令类型
                var commandType = GetCommandType(commandCode);
                if (commandType == null)
                {
                    _logger?.LogWarning("找不到命令类型: {CommandCode}", commandCode);
                    return null;
                }

                // 使用缓存管理器的GetOrCreateConstructor方法，避免重复创建构造函数
                return _cacheManager.GetOrCreateConstructor(commandType);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取命令构造函数失败: {CommandCode}", commandCode);
                return null;
            }
        }

        /// <summary>
        /// 获取所有处理器映射
        /// </summary>
        /// <returns>命令ID到处理器列表的映射字典</returns>
        public IReadOnlyDictionary<CommandId, List<ICommandHandler>> GetAllHandlerMappings()
        {
            return _commandHandlerMap;
        }

        /// <summary>
        /// 获取处理器映射数量
        /// </summary>
        /// <returns>映射数量</returns>
        public int GetHandlerMappingCount()
        {
            return _commandHandlerMap.Count;
        }

        /// <summary>
        /// 清理所有处理器映射
        /// </summary>
        public void ClearHandlerMappings()
        {
            _commandHandlerMap.Clear();
            _logger?.LogInformation("已清理所有处理器映射");
        }

        /// <summary>
        /// 异步移除处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功移除</returns>
        public async Task<bool> RemoveHandlerAsync(string handlerId, CancellationToken cancellationToken = default)
        {
            try
            {
                // 查找处理器
                var handler = _commandHandlerMap.Values
                    .SelectMany(list => list)
                    .FirstOrDefault(h => h.HandlerId == handlerId);

                if (handler == null)
                {
                    return false;
                }

                // 停止处理器
                await handler.StopAsync(cancellationToken);
                handler.Dispose();

                // 从映射中移除处理器
                RemoveHandlerFromMapping(handler);

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "移除处理器 {HandlerId} 时发生异常", handlerId);
                return false;
            }
        }

        /// <summary>
        /// 异步注册处理器 - 同时注册到缓存和映射
        /// </summary>
        /// <param name="handler">处理器实例</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>是否成功注册</returns>
        public async Task<bool> RegisterHandlerAsync(ICommandHandler handler, CancellationToken cancellationToken = default)
        {
            if (handler == null)
            {
                _logger?.LogError("尝试注册null处理器");
                return false;
            }

            try
            {
                // 启动处理器
                await handler.StartAsync(cancellationToken);
                
                // 注册到内部映射
                RegisterHandlerToMapping(handler);
                
                // 同时注册到缓存管理器
                if (_cacheManager != null && handler.GetType() != null)
                {
                    await _cacheManager.CacheCommandHandlerAsync(handler.GetType(), cancellationToken);
                    _logger?.LogDebug("处理器类型 {HandlerType} 已缓存", handler.GetType().Name);
                }
                
                _logger?.LogInformation("处理器 {HandlerName} [ID: {HandlerId}] 注册成功", handler.Name, handler.HandlerId);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "注册处理器 {HandlerName} [ID: {HandlerId}] 时发生异常", handler.Name, handler.HandlerId);
                return false;
            }
        }

  

        #endregion
        #endregion
    }
}
