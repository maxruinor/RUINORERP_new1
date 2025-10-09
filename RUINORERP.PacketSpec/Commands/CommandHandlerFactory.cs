using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一命令处理器工厂 - 支持DI容器和手动创建
    /// 1. 有参构造 → 走 DI 容器（委托表）
    /// 2. 无参构造 → 回退 Activator
    /// </summary>
    public sealed class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, Func<ICommandHandler>> _factoryCache = new();

        /// <summary>
        /// 构造函数 - 支持DI容器
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        public CommandHandlerFactory(IServiceProvider serviceProvider = null)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 创建处理器实例
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        /// <returns>处理器实例</returns>
        public ICommandHandler CreateHandler(Type handlerType)
        {
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));

            // 优先使用缓存的工厂方法
            if (_factoryCache.TryGetValue(handlerType, out var factory))
                return factory();

            // 创建并缓存新的工厂方法
            factory = CreateFactoryMethod(handlerType);
            _factoryCache.TryAdd(handlerType, factory);
            return factory();
        }

        /// <summary>
        /// 创建处理器实例 - 泛型版本
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        /// <returns>处理器实例</returns>
        public T CreateHandler<T>() where T : class, ICommandHandler
        {
            return CreateHandler(typeof(T)) as T;
        }

        /// <summary>
        /// 注册处理器类型
        /// </summary>
        /// <param name="handlerType">处理器类型</param>
        public void RegisterHandler(Type handlerType)
        {
            if (handlerType == null)
                throw new ArgumentNullException(nameof(handlerType));

            if (!_factoryCache.ContainsKey(handlerType))
            {
                var factory = CreateFactoryMethod(handlerType);
                _factoryCache.TryAdd(handlerType, factory);
            }
        }

        /// <summary>
        /// 创建工厂方法 - 支持DI容器和反射创建
        /// </summary>
        /// <param name="type">处理器类型</param>
        /// <returns>工厂方法</returns>
        private Func<ICommandHandler> CreateFactoryMethod(Type type)
        {
            var constructors = type.GetConstructors();
            if (!constructors.Any())
                throw new InvalidOperationException($"类型 {type.FullName} 没有公共构造函数");

            // 选择参数最多的构造函数
            var constructor = constructors.OrderByDescending(c => c.GetParameters().Length).First();
            var parameters = constructor.GetParameters();

            // 无参构造函数 - 使用Activator
            if (!parameters.Any())
            {
                return Expression.Lambda<Func<ICommandHandler>>(Expression.New(constructor)).Compile();
            }

            // 有参构造函数 - 优先使用DI容器，回退到反射
            if (_serviceProvider != null)
            {
                return CreateDiFactoryMethod(constructor, parameters);
            }
            else
            {
                return CreateReflectionFactoryMethod(constructor);
            }
        }

        /// <summary>
        /// 创建DI容器工厂方法
        /// </summary>
        private Func<ICommandHandler> CreateDiFactoryMethod(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            var paramExpressions = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var paramType = parameters[i].ParameterType;
                var resolveMethod = typeof(ServiceProviderServiceExtensions)
                    .GetMethod("GetRequiredService", new[] { typeof(IServiceProvider) })
                    .MakeGenericMethod(paramType);
                paramExpressions[i] = Expression.Call(null, resolveMethod, Expression.Constant(_serviceProvider));
            }

            var newExpression = Expression.New(constructor, paramExpressions);
            return Expression.Lambda<Func<ICommandHandler>>(newExpression).Compile();
        }

        /// <summary>
        /// 创建反射工厂方法（无DI容器时）
        /// </summary>
        private Func<ICommandHandler> CreateReflectionFactoryMethod(ConstructorInfo constructor)
        {
            return () => (ICommandHandler)Activator.CreateInstance(constructor.DeclaringType);
        }
    }
}