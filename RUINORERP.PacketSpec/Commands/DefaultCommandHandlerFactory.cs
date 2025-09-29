using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 默认命令处理器工厂
    /// </summary>
    public class DefaultCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly ConcurrentDictionary<Type, Func<ICommandHandler>> _handlerFactories;
        private readonly ICommandHandlerFactory _handlerFactory;

        public DefaultCommandHandlerFactory(ICommandHandlerFactory handlerFactory = null)
        {
            _handlerFactories = new ConcurrentDictionary<Type, Func<ICommandHandler>>();
            _handlerFactory = handlerFactory;
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

            // 如果有注入的工厂，则使用它创建实例
            if (_handlerFactory != null)
            {
                return _handlerFactory.CreateHandler(handlerType);
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
                // 如果有注入的工厂，则使用它创建实例
                if (_handlerFactory != null)
                {
                    _handlerFactories.TryAdd(handlerType, () => _handlerFactory.CreateHandler(handlerType));
                }
                else
                {
                    _handlerFactories.TryAdd(handlerType, () => (ICommandHandler)Activator.CreateInstance(handlerType));
                }
            }
        }
    }
}
