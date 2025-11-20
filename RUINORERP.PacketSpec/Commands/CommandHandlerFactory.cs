using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 统一命令处理器工厂 - 支持DI容器和手动创建
    /// 1. 有参构造 → 走 DI 容器（委托表）
    /// 2. 无参构造 → 回退 Activator
    /// </summary>
    public sealed class CommandHandlerFactory : ICommandHandlerFactory
    {
        private volatile IServiceProvider _serviceProvider;
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

            // 检查是否是泛型类型定义
            if (handlerType.IsGenericTypeDefinition)
            {
                // 对于泛型类型定义，我们无法直接创建实例
                // 这种情况应该由调用方在运行时根据具体类型参数来创建
                throw new InvalidOperationException($"无法直接创建泛型类型定义的实例: {handlerType.FullName}。请使用具体的泛型类型。");
            }

            // 优先使用缓存的工厂方法
            if (_factoryCache.TryGetValue(handlerType, out var factory))
                return factory();

            // 创建并缓存新的工厂方法
            factory = CreateFactoryMethod(handlerType);
            _factoryCache.TryAdd(handlerType, factory);
            return factory();
        }

        /// <summary>
        /// 创建泛型处理器实例
        /// </summary>
        /// <param name="genericHandlerType">泛型处理器类型定义</param>
        /// <param name="typeArguments">类型参数</param>
        /// <returns>处理器实例</returns>
        public ICommandHandler CreateGenericHandler(Type genericHandlerType, params Type[] typeArguments)
        {
            if (genericHandlerType == null)
                throw new ArgumentNullException(nameof(genericHandlerType));
            
            if (!genericHandlerType.IsGenericTypeDefinition)
                throw new ArgumentException($"类型 {genericHandlerType.FullName} 不是泛型类型定义", nameof(genericHandlerType));
            
            if (typeArguments == null || typeArguments.Length == 0)
                throw new ArgumentException("必须提供类型参数", nameof(typeArguments));

            // 构造具体的泛型类型
            var concreteType = genericHandlerType.MakeGenericType(typeArguments);
            
            // 创建并返回实例
            return CreateHandler(concreteType);
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
        /// 更新服务提供者 - 支持从外部（如CommandDispatcher）设置全局服务提供者
        /// 这使得处理器可以访问Startup中注册的所有全局服务
        /// </summary>
        /// <param name="serviceProvider">新的服务提供者</param>
        public void UpdateServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            // 清除缓存，强制重新创建工厂方法以使用新的服务提供者
            _factoryCache.Clear();
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
            return () =>
            {
                try
                {
                    var args = new object[parameters.Length];
                    
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramType = parameters[i].ParameterType;
                        
                        // 处理泛型类型参数
                        if (paramType.IsGenericType && !paramType.IsConstructedGenericType)
                        {
                            // 尝试构造具体的泛型类型
                            var genericDefinition = paramType.GetGenericTypeDefinition();
                            var typeArgs = constructor.DeclaringType.GetGenericArguments();
                            
                            if (typeArgs.Length > 0)
                            {
                                // 使用处理器类型的泛型参数构造具体的泛型类型
                                var concreteType = genericDefinition.MakeGenericType(typeArgs);
                                
                                // 确保所有泛型参数都已替换
                                if (concreteType.IsGenericType && concreteType.ContainsGenericParameters)
                                {
                                    // 如果仍然包含泛型参数，回退到反射创建
                                    return CreateReflectionFactoryMethod(constructor)();
                                }
                                
                                // 使用DI容器获取具体类型的实例
                                var getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions)
                                    .GetMethod("GetRequiredService", new[] { typeof(IServiceProvider) })
                                    .MakeGenericMethod(concreteType);
                                
                                args[i] = getRequiredServiceMethod.Invoke(null, new object[] { _serviceProvider });
                            }
                            else
                            {
                                // 如果无法构造具体类型，使用默认值
                                args[i] = paramType.IsValueType ? Activator.CreateInstance(paramType) : null;
                            }
                        }
                        else
                        {
                            // 确保非泛型类型也不包含泛型参数
                            if (paramType.ContainsGenericParameters)
                            {
                                // 如果包含泛型参数，回退到反射创建
                                return CreateReflectionFactoryMethod(constructor)();
                            }
                            
                            // 非泛型类型，直接使用DI容器获取
                            var getRequiredServiceMethod = typeof(ServiceProviderServiceExtensions)
                                .GetMethod("GetRequiredService", new[] { typeof(IServiceProvider) })
                                .MakeGenericMethod(paramType);
                            
                            args[i] = getRequiredServiceMethod.Invoke(null, new object[] { _serviceProvider });
                        }
                    }
                    
                    return (ICommandHandler)Activator.CreateInstance(constructor.DeclaringType, args);
                }
                catch (Exception ex)
                {
                    // 如果DI创建失败，回退到反射创建
                    return CreateReflectionFactoryMethod(constructor)();
                }
            };
        }

        /// <summary>
        /// 创建反射工厂方法（无DI容器时）
        /// </summary>
        private Func<ICommandHandler> CreateReflectionFactoryMethod(ConstructorInfo constructor)
        {
            return () =>
            {
                try
                {
                    var parameters = constructor.GetParameters();
                    var args = new object[parameters.Length];
                    
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var paramType = parameters[i].ParameterType;
                        
                        // 处理泛型类型参数
                        if (paramType.IsGenericType && !paramType.IsConstructedGenericType)
                        {
                            // 尝试构造具体的泛型类型
                            var genericDefinition = paramType.GetGenericTypeDefinition();
                            var typeArgs = constructor.DeclaringType.GetGenericArguments();
                            
                            if (typeArgs.Length > 0)
                            {
                                // 使用处理器类型的泛型参数构造具体的泛型类型
                                var concreteType = genericDefinition.MakeGenericType(typeArgs);
                                
                                // 尝试创建默认实例
                                if (concreteType.IsValueType)
                                {
                                    args[i] = Activator.CreateInstance(concreteType);
                                }
                                else if (paramType == typeof(ILogger<>).MakeGenericType(typeArgs))
                                {
                                    // 为ILogger创建默认实例 - 确保logger不为null
                                    try
                                    {
                                        var loggerFactory = new LoggerFactory();
                                        args[i] = loggerFactory.CreateLogger(constructor.DeclaringType);
                                    }
                                    catch
                                    {
                                        // 如果创建失败，使用NullLogger作为最后的保障
                                        var nullLoggerType = typeof(NullLogger<>).MakeGenericType(constructor.DeclaringType);
                                        args[i] = Activator.CreateInstance(nullLoggerType);
                                    }
                                }
                                else
                                {
                                    // 尝试使用无参构造函数
                                    args[i] = Activator.CreateInstance(concreteType);
                                }
                            }
                            else
                            {
                                // 如果无法构造具体类型，使用默认值
                                args[i] = paramType.IsValueType ? Activator.CreateInstance(paramType) : null;
                            }
                        }
                        else
                        {
                            // 非泛型类型，使用默认值
                            args[i] = paramType.IsValueType ? Activator.CreateInstance(paramType) : null;
                        }
                    }
                    
                    return (ICommandHandler)Activator.CreateInstance(constructor.DeclaringType, args);
                }
                catch (Exception)
                {
                    // 如果带参数创建失败，尝试无参创建
                    try
                    {
                        return (ICommandHandler)Activator.CreateInstance(constructor.DeclaringType);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"无法创建命令处理器实例: {constructor.DeclaringType.FullName}", ex);
                    }
                }
            };
        }
    }
}
