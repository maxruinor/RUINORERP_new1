using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
        /// 初始化调度器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public virtual Task<bool> InitializeAsync()
        {
            lock (_lockObject)
            {
                if (!IsRunning)
                {
                    // 可以在这里进行一些初始化操作
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
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
                LogWarning($"处理器已存在: {handler.HandlerId}");
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
                    LogWarning($"处理器在异步操作期间已被注册: {handler.HandlerId}");
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

                LogInfo($"成功注册处理器: {handler.Name} (ID: {handler.HandlerId}), 支持 {handler.SupportedCommands.Count} 个命令");
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
                    LogWarning($"未找到处理器: {handlerId}");
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
                    LogWarning($"处理器在异步操作期间已被移除: {handlerId}");
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

                LogInfo($"成功移除处理器: {handler.Name} (ID: {handlerId})");
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
                LogWarning("调度器未运行，无法分发命令");
                return false;
            }

            if (packet == null)
            {
                LogWarning("收到空的数据包");
                return false;
            }

            var handler = FindHandler(packet);
            if (handler == null)
            {
                LogWarning($"未找到能处理命令ID {packet.CommandId.FullCode} 的处理器");
                return false;
            }

            try
            {
                LogDebug($"分发命令ID {packet.CommandId.FullCode} 到处理器 {handler.Name}");
                await handler.HandleAsync(packet);
                return true;
            }
            catch (Exception ex)
            {
                LogError($"处理器 {handler.Name} 处理命令时出错: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// 扫描并注册程序集中的命令处理器
        /// </summary>
        /// <returns>扫描并注册的处理器数量</returns>
        public virtual async Task<int> ScanAndRegisterHandlersAsync()
        {
            int registeredCount = 0;

            try
            {
                // 获取当前程序集
                var currentAssembly = Assembly.GetExecutingAssembly();

                // 查找所有实现了IClientCommandHandler接口并且带有ClientCommandHandlerAttribute特性的类
                var handlerTypes = currentAssembly.GetTypes()
                    .Where(t => typeof(IClientCommandHandler).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                                && t.GetCustomAttribute<ClientCommandHandlerAttribute>() != null)
                    .ToList();

                LogInfo($"扫描到 {handlerTypes.Count} 个命令处理器类型");

                // 创建并注册每个处理器
                foreach (var type in handlerTypes)
                {
                    try
                    {
                        // 使用无参构造函数创建实例
                        var handler = (IClientCommandHandler)Activator.CreateInstance(type);
                        if (handler != null)
                        {
                            bool success = await RegisterHandlerAsync(handler);
                            if (success)
                            {
                                registeredCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError($"创建处理器 {type.Name} 实例时出错: {ex.Message}", ex);
                    }
                }

                LogInfo($"成功注册 {registeredCount} 个命令处理器");
            }
            catch (Exception ex)
            {
                LogError($"扫描程序集时出错: {ex.Message}", ex);
            }

            return registeredCount;
        }

        /// <summary>
        /// 记录调试日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected virtual void LogDebug(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] DEBUG [ClientCommandDispatcher] {message}");
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected virtual void LogInfo(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] INFO [ClientCommandDispatcher] {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        /// <param name="message">日志消息</param>
        protected virtual void LogWarning(string message)
        {
            Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] WARNING [ClientCommandDispatcher] {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="ex">异常对象</param>
        protected virtual void LogError(string message, Exception ex = null)
        {
            if (ex != null)
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ERROR [ClientCommandDispatcher] {message} - {ex.Message}\n{ex.StackTrace}");
            }
            else
            {
                Debug.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] ERROR [ClientCommandDispatcher] {message}");
            }
        }
    }
}