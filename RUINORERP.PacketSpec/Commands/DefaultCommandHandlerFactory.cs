using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 默认命令处理器工厂,不作为DI容器中的服务，而是作为DI容器的扩展方法，用于创建命令处理器
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
        /// 注册处理器工厂方法
        /// </summary>
        public void RegisterHandler<T>(Func<T> factory) where T : class, ICommandHandler
        {
            _handlerFactories.TryAdd(typeof(T), () => factory());
        }

        /// <summary>
        /// 注册处理器
        /// </summary>
        public void RegisterHandler(Type handlerType)
        {
            if (typeof(ICommandHandler).IsAssignableFrom(handlerType))
            {
                _handlerFactories.TryAdd(handlerType, () => (ICommandHandler)Activator.CreateInstance(handlerType));
            }
        }
    }
}
