using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器 - 负责将命令分发给相应的处理器
    /// </summary>
    public class CommandDispatcher : IDisposable
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers;
        private readonly ICommandHandlerFactory _handlerFactory;
        private readonly ConcurrentDictionary<string, DateTime> _commandHistory;
        private bool _disposed = false;

        /// <summary>
        /// 处理器数量
        /// </summary>
        public int HandlerCount => _handlers.Count;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handlerFactory">处理器工厂</param>
        public CommandDispatcher(ICommandHandlerFactory handlerFactory = null)
        {
            _handlerFactory = handlerFactory ?? new DefaultCommandHandlerFactory();
            _handlers = new Dictionary<Type, ICommandHandler>();
            _commandHistory = new ConcurrentDictionary<string, DateTime>();
            
            // 自动发现并注册处理器
            AutoDiscoverHandlers();
        }

        /// <summary>
        /// 自动发现处理器
        /// </summary>
        private void AutoDiscoverHandlers()
        {
            try
            {
                var handlerTypes = Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => typeof(ICommandHandler).IsAssignableFrom(t) && 
                               !t.IsInterface && 
                               !t.IsAbstract &&
                               t.GetCustomAttribute<CommandHandlerAttribute>() != null)
                    .OrderBy(t => t.GetCustomAttribute<CommandHandlerAttribute>()?.Priority ?? 0);

                foreach (var handlerType in handlerTypes)
                {
                    RegisterHandler(handlerType);
                }
            }
            catch (Exception ex)
            {
                LogError($"自动发现处理器时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        public void RegisterHandler(Type handlerType)
        {
            try
            {
                var handler = _handlerFactory.CreateHandler(handlerType);
                _handlers[handlerType] = handler;
                
                // 初始化处理器
                _ = Task.Run(async () => await handler.InitializeAsync());
                
                LogInfo($"注册处理器: {handler.Name}");
            }
            catch (Exception ex)
            {
                LogError($"注册处理器 {handlerType.Name} 时出错: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        public void RegisterHandler<T>() where T : class, ICommandHandler
        {
            RegisterHandler(typeof(T));
        }

        /// <summary>
        /// 同步分发命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>处理结果</returns>
        public CommandResult Dispatch(ICommand command)
        {
            return DispatchAsync(command).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 异步分发命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<CommandResult> DispatchAsync(ICommand command, CancellationToken cancellationToken = default)
        {
            if (command == null)
            {
                return CommandResult.CreateError("命令对象不能为空");
            }

            var startTime = DateTime.UtcNow;
            string commandId = GenerateCommandId(command);

            try
            {
                // 记录命令历史
                _commandHistory.TryAdd(commandId, startTime);

                // 查找匹配的处理器
                var handler = FindHandler(command);
                if (handler == null)
                {
                    return CommandResult.CreateError($"没有找到适合的处理器处理命令: {command.GetType().Name}");
                }

                // 验证命令是否可以执行
                if (!command.CanExecute())
                {
                    return CommandResult.CreateError("命令无法执行，请检查命令状态");
                }

                LogInfo($"开始处理命令: {command.GetType().Name} [ID: {commandId}]");

                // 执行命令
                var result = await handler.HandleAsync(command, cancellationToken);
                
                // 计算执行时间
                result.ExecutionTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;

                LogInfo($"命令处理完成: {command.GetType().Name} [ID: {commandId}] - {result.ExecutionTimeMs}ms");

                return result;
            }
            catch (OperationCanceledException)
            {
                LogWarning($"命令处理被取消: {command.GetType().Name} [ID: {commandId}]");
                return CommandResult.CreateError("命令处理被取消");
            }
            catch (Exception ex)
            {
                LogError($"处理命令 {command.GetType().Name} [ID: {commandId}] 时出错: {ex.Message}", ex);
                return CommandResult.CreateError($"命令处理异常: {ex.Message}", ex.GetType().Name);
            }
            finally
            {
                // 清理历史记录
                CleanupCommandHistory();
            }
        }

        /// <summary>
        /// 异步分发命令到队列
        /// </summary>
        /// <param name="queue">命令队列</param>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        public async Task<CommandResult> DispatchAsync(BlockingCollection<ICommand> queue, ICommand command, CancellationToken cancellationToken = default)
        {
            var handler = _handlers.Values.FirstOrDefault(h => h.CanHandle(command, queue));
            
            if (handler != null)
            {
                return await handler.HandleAsync(command, cancellationToken);
            }

            return CommandResult.CreateError("没有找到适合的处理器");
        }

        /// <summary>
        /// 查找合适的处理器
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>处理器实例</returns>
        private ICommandHandler FindHandler(ICommand command)
        {
            // 先尝试精确匹配
            var commandType = command.GetType();
            if (_handlers.TryGetValue(commandType, out var exactHandler))
            {
                return exactHandler;
            }

            // 然后按优先级查找能处理的处理器
            return _handlers.Values
                .Where(h => h.CanHandle(command))
                .OrderByDescending(h => h.Priority)
                .FirstOrDefault();
        }

        /// <summary>
        /// 生成命令ID
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <returns>命令ID</returns>
        private string GenerateCommandId(ICommand command)
        {
            return $"{command.GetType().Name}_{DateTime.UtcNow.Ticks}_{Thread.CurrentThread.ManagedThreadId}";
        }

        /// <summary>
        /// 清理命令历史
        /// </summary>
        private void CleanupCommandHistory()
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-30); // 保留30分钟的历史
            var keysToRemove = _commandHistory
                .Where(kvp => kvp.Value < cutoff)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in keysToRemove)
            {
                _commandHistory.TryRemove(key, out _);
            }
        }

        /// <summary>
        /// 记录信息日志
        /// </summary>
        private void LogInfo(string message)
        {
            Console.WriteLine($"[CommandDispatcher] INFO: {message}");
        }

        /// <summary>
        /// 记录警告日志
        /// </summary>
        private void LogWarning(string message)
        {
            Console.WriteLine($"[CommandDispatcher] WARNING: {message}");
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        private void LogError(string message, Exception ex = null)
        {
            Console.WriteLine($"[CommandDispatcher] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[CommandDispatcher] Exception: {ex}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var handler in _handlers.Values)
                {
                    try
                    {
                        _ = Task.Run(async () => await handler.CleanupAsync());
                    }
                    catch (Exception ex)
                    {
                        LogError($"清理处理器 {handler.Name} 时出错: {ex.Message}", ex);
                    }
                }

                _handlers.Clear();
                _commandHistory.Clear();
                _disposed = true;
            }

            GC.SuppressFinalize(this);
        }
    }
}