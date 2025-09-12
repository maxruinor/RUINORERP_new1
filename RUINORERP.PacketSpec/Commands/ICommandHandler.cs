using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器标记特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandHandlerAttribute : Attribute
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public int Priority { get; }

        public CommandHandlerAttribute(string name = null, int priority = 0)
        {
            Name = name;
            Priority = priority;
        }
    }

    /// <summary>
    /// 命令处理器接口
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 异步处理命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<CommandResult> HandleAsync(ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// 判断是否可以处理该命令
        /// </summary>
        /// <param name="command">命令对象</param>
        /// <param name="queue">命令队列（用于重新排队）</param>
        /// <returns>是否可以处理</returns>
        bool CanHandle(ICommand command, BlockingCollection<ICommand> queue = null);

        /// <summary>
        /// 处理器初始化
        /// </summary>
        /// <returns>初始化是否成功</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 处理器清理
        /// </summary>
        /// <returns>清理是否成功</returns>
        Task<bool> CleanupAsync();
    }

    /// <summary>
    /// 命令处理器基类
    /// </summary>
    public abstract class BaseCommandHandler : ICommandHandler
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public virtual int Priority => 0;

        /// <summary>
        /// 是否已初始化
        /// </summary>
        protected bool IsInitialized { get; private set; }

        /// <summary>
        /// 异步处理命令
        /// </summary>
        public abstract Task<CommandResult> HandleAsync(ICommand command, CancellationToken cancellationToken = default);

        /// <summary>
        /// 判断是否可以处理该命令
        /// </summary>
        public abstract bool CanHandle(ICommand command, BlockingCollection<ICommand> queue = null);

        /// <summary>
        /// 处理器初始化
        /// </summary>
        public virtual Task<bool> InitializeAsync()
        {
            IsInitialized = true;
            return Task.FromResult(true);
        }

        /// <summary>
        /// 处理器清理
        /// </summary>
        public virtual Task<bool> CleanupAsync()
        {
            IsInitialized = false;
            return Task.FromResult(true);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        protected virtual void LogMessage(string message, Exception ex = null)
        {
            Console.WriteLine($"[{Name}] {message}");
            if (ex != null)
            {
                Console.WriteLine($"[{Name}] Exception: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 命令处理器工厂接口
    /// </summary>
    public interface ICommandHandlerFactory
    {
        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>处理器实例</returns>
        ICommandHandler CreateHandler(Type handlerType);

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <returns>处理器实例</returns>
        T CreateHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 获取处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <returns>处理器实例</returns>
        T GetHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 注册处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <param name="handler">处理器实例</param>
        void RegisterHandler<T>(T handler) where T : class, ICommandHandler;

        /// <summary>
        /// 注销处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        void UnregisterHandler<T>() where T : class, ICommandHandler;

        /// <summary>
        /// 注册处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        void RegisterHandler(Type handlerType);

        /// <summary>
        /// 获取所有已注册的处理器类型
        /// </summary>
        /// <returns>处理器类型列表</returns>
        IEnumerable<Type> GetRegisteredHandlerTypes();
    }

    /// <summary>
    /// 默认命令处理器工厂
    /// </summary>
    public class DefaultCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly ConcurrentDictionary<Type, Func<ICommandHandler>> _handlerFactories;

        public DefaultCommandHandlerFactory()
        {
            _handlerFactories = new ConcurrentDictionary<Type, Func<ICommandHandler>>();
        }

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        public ICommandHandler CreateHandler(Type handlerType)
        {
            if (_handlerFactories.TryGetValue(handlerType, out var factory))
            {
                return factory();
            }

            // 使用反射创建实例
            return (ICommandHandler)Activator.CreateInstance(handlerType);
        }

        /// <summary>
        /// 创建命令处理器
        /// </summary>
        public T CreateHandler<T>() where T : class, ICommandHandler
        {
            return CreateHandler(typeof(T)) as T;
        }

        /// <summary>
        /// 获取处理器
        /// </summary>
        public T GetHandler<T>() where T : class, ICommandHandler
        {
            return CreateHandler<T>();
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        public void RegisterHandler<T>(T handler) where T : class, ICommandHandler
        {
            _handlerFactories.TryAdd(typeof(T), () => handler);
        }

        /// <summary>
        /// 注销处理器
        /// </summary>
        public void UnregisterHandler<T>() where T : class, ICommandHandler
        {
            _handlerFactories.TryRemove(typeof(T), out _);
        }

        /// <summary>
        /// 注册处理器类型
        /// </summary>
        public void RegisterHandler(Type handlerType)
        {
            if (typeof(ICommandHandler).IsAssignableFrom(handlerType))
            {
                _handlerFactories.TryAdd(handlerType, () => (ICommandHandler)Activator.CreateInstance(handlerType));
            }
        }

        /// <summary>
        /// 注册处理器工厂方法
        /// </summary>
        public void RegisterHandler<T>(Func<T> factory) where T : class, ICommandHandler
        {
            _handlerFactories.TryAdd(typeof(T), () => factory());
        }

        /// <summary>
        /// 获取所有已注册的处理器类型
        /// </summary>
        public IEnumerable<Type> GetRegisteredHandlerTypes()
        {
            return _handlerFactories.Keys;
        }
    }
}