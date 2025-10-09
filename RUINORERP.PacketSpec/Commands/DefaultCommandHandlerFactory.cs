using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        /// 创建处理器 - 非泛型版本（重构版：提取处理器创建流程）
        /// </summary>
        public ICommandHandler CreateHandler(Type commandType)
        {
            if (commandType == null)
            {
                throw new ArgumentNullException(nameof(commandType));
            }

            try
            {
                return CreateHandlerInternal(commandType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 内部处理器创建逻辑 - 提取为独立方法
        /// </summary>
        private ICommandHandler CreateHandlerInternal(Type commandType)
        {
            // 尝试从缓存中获取工厂方法
            if (_handlerFactories.TryGetValue(commandType, out var factory))
            {
                return factory();
            }

            // 尝试反射创建处理器实例
            var handlerType = FindHandlerType(commandType);
            if (handlerType != null)
            {
                return CreateAndCacheHandler(commandType, handlerType);
            }

            return null;
        }

        /// <summary>
        /// 创建并缓存处理器 - 提取为独立方法
        /// </summary>
        private ICommandHandler CreateAndCacheHandler(Type commandType, Type handlerType)
        {
            var handler = Activator.CreateInstance(handlerType) as ICommandHandler;
            if (handler != null)
            {
                // 缓存处理器实例创建方法
                _handlerFactories.TryAdd(commandType, () => Activator.CreateInstance(handlerType) as ICommandHandler);
                return handler;
            }

            return null;
        }

        /// <summary>
        /// 查找处理器类型（重构版：优化查找逻辑）
        /// </summary>
        private Type FindHandlerType(Type commandType)
        {
            try
            {
                var handlerTypes = GetRegisteredHandlerTypes();
                return FindMatchingHandlerType(handlerTypes, commandType);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取已注册的处理器类型 - 提取为独立方法
        /// </summary>
        private List<Type> GetRegisteredHandlerTypes()
        {
            return _handlerFactories.Keys.Distinct().ToList();
        }

        /// <summary>
        /// 查找匹配的处理器类型 - 提取为独立方法
        /// </summary>
        private Type FindMatchingHandlerType(List<Type> handlerTypes, Type commandType)
        {
            foreach (var handlerType in handlerTypes)
            {
                if (CanHandleCommand(handlerType, commandType))
                {
                    return handlerType;
                }
            }

            return null;
        }

        /// <summary>
        /// 检查处理器是否能处理指定命令类型（重构版：优化检查逻辑）
        /// </summary>
        private bool CanHandleCommand(Type handlerType, Type commandType)
        {
            try
            {
                return CheckInterfaceSupport(handlerType, commandType) || CheckBaseClassSupport(handlerType, commandType);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查接口支持 - 提取为独立方法
        /// </summary>
        private bool CheckInterfaceSupport(Type handlerType, Type commandType)
        {
            var interfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler))
                .ToList();

            foreach (var interfaceType in interfaces)
            {
                var genericArguments = interfaceType.GetGenericArguments();
                if (genericArguments.Length == 1 && genericArguments[0].IsAssignableFrom(commandType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检查基类支持 - 提取为独立方法
        /// </summary>
        private bool CheckBaseClassSupport(Type handlerType, Type commandType)
        {
            var baseType = handlerType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType)
                {
                    var genericArguments = baseType.GetGenericArguments();
                    if (genericArguments.Length > 0 && genericArguments[0].IsAssignableFrom(commandType))
                    {
                        return true;
                    }
                }
                baseType = baseType.BaseType;
            }

            return false;
        }

        /// <summary>
        /// 创建命令处理器（重构版：泛型约束优化）
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
