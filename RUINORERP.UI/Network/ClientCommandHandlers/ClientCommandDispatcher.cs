using Autofac;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令调度器实现
    /// 负责管理所有的客户端命令处理器，并根据命令ID将命令分发到对应的处理器
    /// </summary>
    public class ClientCommandDispatcher : IClientCommandDispatcher
    {
        private readonly object _lockObject = new object();
        private readonly Dictionary<string, IClientCommandHandler> _handlers = new Dictionary<string, IClientCommandHandler>();
        private readonly Dictionary<ulong, List<IClientCommandHandler>> _commandToHandlersMap = new Dictionary<ulong, List<IClientCommandHandler>>();
        private readonly ILogger<ClientCommandDispatcher> _logger;

        /// <summary>
        /// 已注册的处理器列表
        /// </summary>
        public IReadOnlyList<IClientCommandHandler> Handlers
        {
            get
            {
                lock (_lockObject)
                {
                    return _handlers.Values.ToList().AsReadOnly();
                }
            }
        }

        /// <summary>
        /// 调度器状态
        /// </summary>
        public bool IsRunning { get; private set; } = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ClientCommandDispatcher(ILogger<ClientCommandDispatcher> logger = null)
        {
            _logger = logger;
        }

        /// <summary>
        /// 初始化调度器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public virtual Task<bool> InitializeAsync()
        {
            lock (_lockObject)
            {
                if (!IsRunning)
                {
                    // 进行初始化操作
                    _handlers.Clear();
                    _commandToHandlersMap.Clear();
                    _logger?.LogDebug("调度器初始化成功");
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }
        
        /// <summary>
        /// 初始化并启动调度系统，包括自动扫描和注册处理器
        /// 此方法提供完整的一键式初始化流程
        /// </summary>
        /// <param name="assemblies">可选，指定要扫描的程序集，不指定则扫描当前程序集</param>
        /// <returns>初始化是否成功以及注册的处理器数量</returns>
        public virtual async Task<(bool success, int registeredCount)> InitializeAndStartAsync(IEnumerable<Assembly> assemblies = null)
        {
            try
            {
                _logger?.LogDebug("开始初始化并启动命令处理系统");
                
                // 初始化调度器
                bool initialized = await InitializeAsync();
                if (!initialized)
                {
                    _logger?.LogWarning("命令调度器初始化失败或已初始化");
                    return (false, 0);
                }
                _logger?.LogDebug("命令调度器初始化完成");
                
                // 启动调度器
                bool started = await StartAsync();
                if (!started)
                {
                    _logger?.LogWarning("命令调度器启动失败或已启动");
                    return (false, 0);
                }
                _logger?.LogDebug("命令调度器启动完成");
                
                // 扫描并注册处理器
                assemblies = assemblies ?? new[] { Assembly.GetExecutingAssembly() };
                int registeredCount = await ScanAndRegisterHandlersAsync(assemblies);
                
                _logger?.LogDebug($"命令处理系统初始化完成，共注册 {registeredCount} 个命令处理器");
                return (true, registeredCount);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "初始化并启动命令处理系统失败");
                return (false, 0);
            }
        }

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns>启动是否成功</returns>
        public virtual Task<bool> StartAsync()
        {
            lock (_lockObject)
            {
                if (!IsRunning)
                {
                    IsRunning = true;
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 停止调度器
        /// </summary>
        /// <returns>停止是否成功</returns>
        public virtual async Task<bool> StopAsync()
        {
            List<IClientCommandHandler> handlersToStop;
            lock (_lockObject)
            {
                if (!IsRunning)
                {
                    return false;
                }
                IsRunning = false;
                // 获取需要停止的处理器列表的副本
                handlersToStop = _handlers.Values.ToList();
            }

            // 在lock外异步停止所有处理器
            foreach (var handler in handlersToStop)
            {
                await handler.StopAsync();
            }

            return true;
        }

        /// <summary>
        /// 注册单个命令处理器
        /// </summary>
        /// <param name="handler">命令处理器</param>
        /// <returns>注册是否成功</returns>
        public virtual async Task<bool> RegisterHandlerAsync(IClientCommandHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            // 先检查处理器是否已注册
            bool isRegistered;
            lock (_lockObject)
            {
                isRegistered = _handlers.ContainsKey(handler.HandlerId);
            }

            if (isRegistered)
            {
                _logger?.LogWarning($"处理器已存在: {handler.HandlerId}");
                return false;
            }

            // 在lock外执行异步操作
            await handler.InitializeAsync();
            await handler.StartAsync();

            // 注册处理器到集合
            lock (_lockObject)
            {
                // 再次检查以避免竞争条件
                if (_handlers.ContainsKey(handler.HandlerId))
                {
                    _logger?.LogWarning($"处理器在异步操作期间已被注册: {handler.HandlerId}");
                    return false;
                }

                _handlers[handler.HandlerId] = handler;

                // 更新命令到处理器的映射
                foreach (var command in handler.SupportedCommands)
                {
                    if (!_commandToHandlersMap.ContainsKey(command.FullCode))
                    {
                        _commandToHandlersMap[command.FullCode] = new List<IClientCommandHandler>();
                    }
                    _commandToHandlersMap[command.FullCode].Add(handler);
                    // 按优先级排序，优先级高的处理器先执行
                    _commandToHandlersMap[command.FullCode].Sort((h1, h2) => h2.Priority.CompareTo(h1.Priority));
                }

                _logger?.LogDebug($"成功注册处理器: {handler.Name} (ID: {handler.HandlerId}), 支持 {handler.SupportedCommands.Count} 个命令");
                return true;
            }
        }

        /// <summary>
        /// 批量注册命令处理器
        /// </summary>
        /// <param name="handlers">命令处理器集合</param>
        /// <returns>注册是否成功</returns>
        public virtual async Task<bool> RegisterHandlersAsync(IEnumerable<IClientCommandHandler> handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            bool allSuccess = true;
            foreach (var handler in handlers)
            {
                bool success = await RegisterHandlerAsync(handler);
                allSuccess = allSuccess && success;
            }
            return allSuccess;
        }

        /// <summary>
        /// 取消注册命令处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>取消注册是否成功</returns>
        public virtual async Task<bool> UnregisterHandlerAsync(string handlerId)
        {
            if (string.IsNullOrEmpty(handlerId))
                throw new ArgumentNullException(nameof(handlerId));

            // 先获取处理器引用
            IClientCommandHandler handler;
            lock (_lockObject)
            {
                if (!_handlers.ContainsKey(handlerId))
                {
                    _logger?.LogWarning($"未找到处理器: {handlerId}");
                    return false;
                }
                handler = _handlers[handlerId];
            }

            // 在lock外执行异步操作
            await handler.StopAsync();

            // 移除处理器的引用
            lock (_lockObject)
            {
                // 再次检查以避免竞争条件
                if (!_handlers.ContainsKey(handlerId))
                {
                    _logger?.LogWarning($"处理器在异步操作期间已被移除: {handlerId}");
                    return false;
                }

                // 从命令映射中移除
                foreach (var command in handler.SupportedCommands)
                {
                    if (_commandToHandlersMap.ContainsKey(command.FullCode))
                    {
                        _commandToHandlersMap[command.FullCode].Remove(handler);
                        if (_commandToHandlersMap[command.FullCode].Count == 0)
                        {
                            _commandToHandlersMap.Remove(command.FullCode);
                        }
                    }
                }

                // 从处理器列表中移除
                _handlers.Remove(handlerId);

                _logger?.LogDebug($"成功移除处理器: {handler.Name} (ID: {handlerId})");
                return true;
            }
        }

        /// <summary>
        /// 根据命令ID查找合适的处理器
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>找到的处理器，未找到返回null</returns>
        public virtual IClientCommandHandler FindHandler(PacketModel packet)
        {
            if (packet == null)
                return null;

            lock (_lockObject)
            {
                if (_commandToHandlersMap.TryGetValue(packet.CommandId.FullCode, out var handlers))
                {
                    // 返回第一个可以处理该命令的处理器（按优先级排序）
                    return handlers.FirstOrDefault(h => h.CanHandle(packet));
                }
                return null;
            }
        }

        /// <summary>
        /// 分发命令到合适的处理器
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理是否成功</returns>
        public virtual async Task<bool> DispatchAsync(PacketModel packet)
        {
            if (!IsRunning)
            {
                _logger?.LogWarning("调度器未运行，无法分发命令");
                return false;
            }

            if (packet == null)
            {
                _logger?.LogWarning("收到空的数据包");
                return false;
            }

            var handler = FindHandler(packet);
            if (handler == null)
            {
                _logger?.LogDebug($"未找到能处理命令ID {packet.CommandId.FullCode} 的处理器");
                return false;
            }

            try
            {
                _logger?.LogDebug($"分发命令ID {packet.CommandId.FullCode} 到处理器 {handler.Name}");
                await handler.HandleAsync(packet);
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"处理器 {handler.Name} 处理命令时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 扫描并注册程序集中的命令处理器（仅扫描当前程序集）
        /// </summary>
        /// <returns>扫描并注册的处理器数量</returns>
        public virtual async Task<int> ScanAndRegisterHandlersAsync()
        {
            // 默认扫描当前程序集，不使用依赖注入
            return await ScanAndRegisterHandlersAsync(new[] { Assembly.GetExecutingAssembly() });
        }

        /// <summary>
        /// 扫描并注册指定程序集中的命令处理器
        /// </summary>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>扫描并注册的处理器数量</returns>
        public virtual async Task<int> ScanAndRegisterHandlersAsync(IEnumerable<Assembly> assemblies)
        {
            int registeredCount = 0;

            try
            {
                // 确保assemblies不为null
                assemblies = assemblies ?? new[] { Assembly.GetExecutingAssembly() };

                List<Type> handlerTypes = new List<Type>();

                // 扫描所有指定的程序集
                foreach (var assembly in assemblies)
                {
                    var types = assembly.GetTypes()
                        .Where(t => typeof(IClientCommandHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                                    && t.GetCustomAttribute<ClientCommandHandlerAttribute>() != null)
                        .ToList();

                    handlerTypes.AddRange(types);
                }

                _logger?.LogDebug($"从{assemblies.Count()}个程序集中扫描到 {handlerTypes.Count} 个命令处理器类型");

                // 创建并注册每个处理器
                foreach (var type in handlerTypes)
                {
                    try
                    {
                        IClientCommandHandler handler = null;
                        // 优先使用依赖注入容器创建实例 - 支持两种方式：Autofac和Microsoft DI
                        try
                        {
                            // 方式1: 尝试从Autofac容器获取
                            var lifetimeScope = Startup.GetFromFac<ILifetimeScope>();
                            if (lifetimeScope != null)
                            {
                                try
                                {
                                    handler = (IClientCommandHandler)lifetimeScope.Resolve(type);
                                    _logger?.LogDebug($"通过Autofac依赖注入创建处理器 {type.Name}");
                                }
                                catch (Exception ex1)
                                {
                                    _logger?.LogDebug(ex1, $"使用Autofac创建处理器 {type.Name} 失败，尝试直接注册解析");
                                    
                                    // 尝试直接注册然后解析（适用于没有在模块中注册的类型）
                                    try
                                    {
                                        handler = (IClientCommandHandler)Activator.CreateInstance(type);
                                        _logger?.LogDebug($"通过构造函数直接创建处理器 {type.Name}");
                                    }
                                    catch (Exception ex2)
                                    {
                                        _logger?.LogError(ex2, $"直接创建处理器 {type.Name} 失败");
                                    }
                                }
                            }
                            // 方式2: 如果Autofac不可用，尝试Microsoft DI容器
                            else if (Startup.ServiceProvider != null)
                            {
                                try
                                {
                                    handler = (IClientCommandHandler)Startup.ServiceProvider.GetService(type);
                                    if (handler != null)
                                    {
                                        _logger?.LogDebug($"通过Microsoft DI创建处理器 {type.Name}");
                                    }
                                    else
                                    {
                                        // 如果服务未注册，尝试Activator创建
                                        handler = (IClientCommandHandler)Activator.CreateInstance(type);
                                        _logger?.LogDebug($"通过构造函数直接创建处理器 {type.Name}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError(ex, $"使用Microsoft DI创建处理器 {type.Name} 失败");
                                }
                            }
                            // 方式3: 如果没有可用的DI容器，回退到直接创建
                            else
                            {
                                try
                                {
                                    handler = (IClientCommandHandler)Activator.CreateInstance(type);
                                    _logger?.LogDebug($"通过构造函数直接创建处理器 {type.Name}");
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError(ex, $"直接创建处理器 {type.Name} 失败，且没有可用的依赖注入容器");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, $"创建处理器 {type.Name} 时发生未预期的错误");
                        }

                        if (handler != null)
                        {
                            bool success = await RegisterHandlerAsync(handler);
                            if (success)
                            {
                                registeredCount++;
                            }
                        }
                        else
                        {
                            _logger?.LogWarning($"无法创建处理器 {type.Name} 的实例");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"创建处理器 {type.Name} 实例时出错: {ex.Message}");
                    }
                }

                _logger?.LogDebug($"成功注册 {registeredCount} 个命令处理器");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"扫描程序集时出错: {ex.Message}");
            }

            return registeredCount;
        }


    }
}