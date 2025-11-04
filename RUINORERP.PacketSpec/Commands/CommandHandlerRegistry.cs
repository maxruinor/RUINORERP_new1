using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器注册表
    /// 负责统一管理命令处理器的扫描、注册、缓存以及命令ID与响应类型的映射关系
    /// 优化反射创建实例的性能
    /// </summary>
    public class CommandHandlerRegistry : IDisposable
    {
        #region 字段和属性

        /// <summary>
        /// 命令处理器工厂接口
        /// </summary>
        private readonly ICommandHandlerFactory _handlerFactory;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<CommandHandlerRegistry> _logger;

        /// <summary>
        /// 命令处理器缓存 - 按处理器ID索引（主要存储）
        /// </summary>
        private readonly ConcurrentDictionary<string, ICommandHandler> _handlersById = new ConcurrentDictionary<string, ICommandHandler>();

        /// <summary>
        /// 命令ID到处理器的映射缓存（派生映射，基于_handlersById）
        /// </summary>
        private readonly ConcurrentDictionary<CommandId, List<ICommandHandler>> _commandHandlersMap = new ConcurrentDictionary<CommandId, List<ICommandHandler>>();

 
        /// <summary>
        /// 类型到响应类型的映射缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, Type> _responseTypeCache = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// 同步锁，用于确保线程安全
        /// </summary>
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// 是否已初始化
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// 已注册的处理器数量
        /// </summary>
        public int HandlerCount => _handlersById.Count;

        /// <summary>
        /// 命令ID到处理器映射的数量
        /// </summary>
        public int CommandHandlerMapCount => _commandHandlersMap.Count;

       

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化命令处理器注册表
        /// </summary>
        /// <param name="handlerFactory">命令处理器工厂</param>
        /// <param name="logger">日志记录器</param>
        public CommandHandlerRegistry(ICommandHandlerFactory handlerFactory, ILogger<CommandHandlerRegistry> logger = null)
        {
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _logger = logger;
        }

        #endregion

        #region 初始化方法


        /// <summary>
        /// 已扫描的程序集缓存，用于避免重复扫描
        /// </summary>
        private readonly HashSet<string> _scannedAssemblies = new HashSet<string>();

        /// <summary>
        /// 异步初始化注册表，扫描并注册命令处理器
        /// 优化：避免重复扫描和注册相同的程序集和处理器
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="assemblies">要扫描的程序集，如果为空则扫描当前执行程序集</param>
        /// <returns>初始化是否成功</returns>
        public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default, params Assembly[] assemblies)
        {
            try
            {
                _lock.EnterWriteLock();
                try
                {
                    // 如果没有指定程序集，则扫描当前执行程序集
                    if (assemblies == null || assemblies.Length == 0)
                    {
                        assemblies = new[] { Assembly.GetExecutingAssembly() };
                        _logger?.LogDebug("未指定程序集，使用当前执行程序集: {AssemblyName}", assemblies[0].FullName);
                    }

                    // 筛选出之前未扫描过的程序集
                    var newAssemblies = assemblies.Where(a => !_scannedAssemblies.Contains(a.FullName)).ToArray();
                    
                    // 记录已扫描的程序集
                    foreach (var assembly in assemblies)
                    {
                        _scannedAssemblies.Add(assembly.FullName);
                    }

                    // 检查是否已经初始化
                    if (IsInitialized)
                    {
                        // 如果已经初始化且有新的程序集需要扫描
                        if (newAssemblies.Length > 0)
                        {
                            _logger?.LogDebug("命令处理器注册表已经初始化，现在将扫描新的程序集并添加新的处理器: {AssemblyCount} 个程序集", newAssemblies.Length);
                            await ScanAndRegisterHandlersAsync(newAssemblies, cancellationToken);
                            _logger?.LogDebug("新程序集扫描完成，更新后的处理器数量: {HandlerCount}，命令映射数量: {CommandCount}",
                                HandlerCount, CommandHandlerMapCount);
                        }
                        else
                        {
                            _logger?.LogDebug("命令处理器注册表已经初始化，所有指定的程序集都已扫描过，无需重复扫描");
                        }
                        return true;
                    }

                    // 首次初始化时扫描并注册命令处理器
                    _logger?.LogDebug("首次初始化，开始扫描并注册命令处理器: {AssemblyCount} 个程序集", assemblies.Length);
                    await ScanAndRegisterHandlersAsync(assemblies, cancellationToken);

                    IsInitialized = true;
                    _logger?.LogDebug("命令处理器注册表初始化成功: 已注册 {HandlerCount} 个处理器，{CommandCount} 个命令映射",
                        HandlerCount, CommandHandlerMapCount);
                    return true;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "命令处理器注册表初始化失败");
                return false;
            }
        }

        #endregion

        #region 扫描和注册方法

        /// <summary>
        /// 扫描指定程序集并注册命令处理器
        /// </summary>
        /// <param name="assemblies">要扫描的程序集</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        private async Task ScanAndRegisterHandlersAsync(IEnumerable<Assembly> assemblies, CancellationToken cancellationToken)
        {
            foreach (var assembly in assemblies)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    _logger?.LogDebug("正在扫描程序集: {AssemblyName}", assembly.FullName);

                    // 获取所有实现ICommandHandler接口的非抽象类型
                    var handlerTypes = assembly.GetTypes()
                        .Where(t => !t.IsAbstract && !t.IsInterface && typeof(ICommandHandler).IsAssignableFrom(t))
                        .ToList();

                    _logger?.LogDebug("在程序集 {AssemblyName} 中找到 {Count} 个命令处理器类型", assembly.FullName, handlerTypes.Count);

                    // 并行注册处理器
                    var registrationTasks = handlerTypes.Select(async handlerType =>
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        try
                        {
                            await RegisterHandlerAsync(handlerType, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "注册命令处理器类型失败: {HandlerType}", handlerType.FullName);
                        }
                    });

                    // 过滤掉可能的null任务，防止ArgumentException异常
                    var validRegistrationTasks = registrationTasks.Where(t => t != null).ToList();
                    if (validRegistrationTasks.Any())
                    {
                        await Task.WhenAll(validRegistrationTasks);
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger?.LogError(ex, "扫描程序集类型失败: {AssemblyName}，错误: {Message}",
                        assembly.FullName, ex.Message);
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        _logger?.LogError(loaderException, "类型加载错误");
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "扫描程序集失败: {AssemblyName}", assembly.FullName);
                }
            }
        }

        /// <summary>
        /// 注册单个命令处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>异步任务</returns>
        private async Task RegisterHandlerAsync(Type handlerType, CancellationToken cancellationToken)
        {
            try
            {
                // 使用工厂创建处理器实例
                var handler = _handlerFactory.CreateHandler(handlerType);
                if (handler == null)
                {
                    _logger?.LogWarning("处理器工厂未能创建处理器实例: {HandlerType}", handlerType.FullName);
                    return;
                }

                // 初始化处理器
                if (!handler.IsInitialized)
                {
                    var initialized = await handler.InitializeAsync(cancellationToken);
                    if (!initialized)
                    {
                        _logger?.LogWarning("处理器初始化失败: {HandlerName}({HandlerId})", handler.Name, handler.HandlerId);
                        handler.Dispose();
                        return;
                    }
                }

                // 启动处理器
                var started = await handler.StartAsync(cancellationToken);
                if (!started)
                {
                    _logger?.LogWarning("处理器启动失败: {HandlerName}({HandlerId})", handler.Name, handler.HandlerId);
                    handler.Dispose();
                    return;
                }

                // 注册处理器
                if (_handlersById.TryAdd(handler.HandlerId, handler))
                {
                    _logger?.Debug("成功注册命令处理器: {HandlerName}({HandlerId})", handler.Name, handler.HandlerId);

                    // 建立命令ID到处理器的映射
                    UpdateCommandHandlerMap(handler, add: true);
                }
                else
                {
                    _logger?.LogWarning("处理器ID冲突，注册失败: {HandlerId}", handler.HandlerId);
                    handler.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "注册命令处理器失败: {HandlerType}", handlerType.FullName);
                throw;
            }
        }

        

        #endregion

        #region 查找和获取方法

        /// <summary>
        /// 根据命令ID获取对应的处理器列表
        /// </summary>
        /// <param name="commandId">命令ID</param>
        /// <returns>处理器列表，如果没有找到则返回空列表</returns>
        public List<ICommandHandler> GetHandlersByCommandId(CommandId commandId)
        {
            _lock.EnterReadLock();
            try
            {
                if (_commandHandlersMap.TryGetValue(commandId, out var handlers))
                {
                    return new List<ICommandHandler>(handlers);
                }
                return new List<ICommandHandler>();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 根据处理器ID获取处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>处理器实例，如果没有找到则返回null</returns>
        public ICommandHandler GetHandlerById(string handlerId)
        {
            _lock.EnterReadLock();
            try
            {
                _handlersById.TryGetValue(handlerId, out var handler);
                return handler;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 获取所有注册的处理器
        /// </summary>
        /// <returns>处理器列表</returns>
        public List<ICommandHandler> GetAllHandlers()
        {
            _lock.EnterReadLock();
            try
            {
                return _handlersById.Values.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

    
        /// <summary>
        /// 根据类型名称获取响应类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <returns>响应类型，如果没有找到则返回null</returns>
        public Type GetResponseTypeByName(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                return null;

            // 先检查缓存
            if (_responseTypeCache.TryGetValue(typeName, out var cachedType))
                return cachedType;

            // 尝试获取类型
            Type responseType = null;
            try
            {
                // 通过类型名称反射获取Type对象
                responseType = Type.GetType(typeName, false);

                // 如果直接获取失败，尝试在当前程序集中查找
                if (responseType == null)
                {
                    // 首先尝试当前执行程序集
                    var currentAssembly = Assembly.GetExecutingAssembly();
                    responseType = currentAssembly.GetType(typeName);

                    // 如果仍然找不到，搜索所有已加载的程序集
                    if (responseType == null)
                    {
                        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            // 跳过动态程序集
                            if (assembly.IsDynamic) continue;

                            responseType = assembly.GetType(typeName);
                            if (responseType != null)
                                break;
                        }
                    }
                }

                // 缓存找到的类型
                if (responseType != null)
                {
                    _responseTypeCache.TryAdd(typeName, responseType);
                }

                return responseType;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取响应类型失败: {TypeName}", typeName);
                return null;
            }
        }

        /// <summary>
        /// 根据命令上下文创建响应实例
        /// </summary>
        /// <param name="context">命令上下文</param>
        /// <returns>响应实例，如果创建失败则返回null</returns>
        public IResponse CreateResponseInstance(CommandContext context)
        {
            if (context == null)
                return null;

            // 优先使用上下文中的响应类型
            if (!string.IsNullOrEmpty(context.ExpectedResponseTypeName))
            {
                var responseType = GetResponseTypeByName(context.ExpectedResponseTypeName);
                if (responseType != null)
                {
                    return CreateInstance(responseType);
                }
            }
            return null;
        }

        /// <summary>
        /// 创建指定类型的实例
        /// </summary>
        /// <param name="type">要创建的类型</param>
        /// <returns>创建的实例，如果失败则返回null</returns>
        public IResponse CreateInstance(Type type)
        {
            try
            {
                // 如果是接口，创建默认的ResponseBase实例
                if (type.IsInterface)
                {
                    var baseResponse = new ResponseBase();
                    if (typeof(IResponse).IsAssignableFrom(type))
                    {
                        return baseResponse as IResponse;
                    }
                    return null;
                }

                // 正常创建实例
                return Activator.CreateInstance(type) as IResponse;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "创建响应实例失败: {TypeName}", type.Name);
                return null;
            }
        }

        #endregion

        #region 注册管理方法

        /// <summary>
        /// 手动注册命令处理器
        /// </summary>
        /// <param name="handler">命令处理器实例</param>
        /// <returns>注册是否成功</returns>
        public bool RegisterHandler(ICommandHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            _lock.EnterWriteLock();
            try
            {
                if (_handlersById.TryAdd(handler.HandlerId, handler))
                {
                    // 更新命令处理器映射
                    UpdateCommandHandlerMap(handler, add: true);

                    _logger?.Debug("手动注册命令处理器成功: {HandlerName}({HandlerId})", handler.Name, handler.HandlerId);
                    return true;
                }

                return false;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
 

        /// <summary>
        /// 清理注册表
        /// </summary>
        public void Clear()
        {
            _lock.EnterWriteLock();
            try
            {
                // 释放所有处理器
                foreach (var handler in _handlersById.Values)
                {
                    try
                    {
                        handler.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "释放处理器失败: {HandlerId}", handler.HandlerId);
                    }
                }

                // 清空所有集合
                _handlersById.Clear();
                _commandHandlersMap.Clear();
                _responseTypeCache.Clear();

               
                IsInitialized = false;
                _logger?.Debug("命令处理器注册表已清理");
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 更新命令ID到处理器的映射
        /// </summary>
        /// <param name="handler">处理器实例</param>
        /// <param name="add">是否添加映射（true为添加，false为移除）</param>
        private void UpdateCommandHandlerMap(ICommandHandler handler, bool add)
        {
            foreach (var commandId in handler.SupportedCommands)
            {
                if (add)
                {
                    // 添加映射
                    _commandHandlersMap.AddOrUpdate(commandId,
                        (key) => new List<ICommandHandler> { handler },
                        (key, handlers) => { handlers.Add(handler); return handlers; }
                    );
                }
                else
                {
                    // 移除映射
                    if (_commandHandlersMap.TryGetValue(commandId, out var handlers))
                    {
                        handlers.Remove(handler);
                        // 如果处理器列表为空，移除该命令ID的映射
                        if (handlers.Count == 0)
                        {
                            _commandHandlersMap.TryRemove(commandId, out _);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除指定的命令处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>移除是否成功</returns>
        public bool RemoveHandler(string handlerId)
        {
            if (string.IsNullOrEmpty(handlerId))
                return false;

            _lock.EnterWriteLock();
            try
            {
                if (_handlersById.TryRemove(handlerId, out var handler))
                {
                    // 从命令映射中移除处理器
                    UpdateCommandHandlerMap(handler, add: false);

                    // 释放处理器资源
                    handler.Dispose();

                    _logger?.Debug("命令处理器已移除: {HandlerId}", handlerId);
                    return true;
                }

                _logger?.LogWarning("移除处理器失败，处理器不存在: {HandlerId}", handlerId);
                return false;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #endregion


        #region IDisposable 实现


        /// <summary>
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _lock.Dispose();
            Clear();

        }




        #endregion
    }
}
