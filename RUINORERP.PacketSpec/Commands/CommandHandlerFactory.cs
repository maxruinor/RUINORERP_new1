using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 1. 有参构造 → 走 DI 容器（委托表），
    /// 2. 无参构造 → 回退 Activator
    /// </summary>
    public sealed class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider _sp;
        private readonly ConcurrentDictionary<Type, Func<ICommandHandler>> _ctorCache = new();

        public CommandHandlerFactory(IServiceProvider sp) => _sp = sp;

        public ICommandHandler CreateHandler(Type handlerType)
        {
            // 1. 优先用委托（性能高，支持任意参数）
            if (_ctorCache.TryGetValue(handlerType, out var factory))
                return factory();

            // 2. 委托没配，就现场"反射 + 表达式"编译一个，并缓存
            factory = CompileFactory(handlerType);
            _ctorCache.TryAdd(handlerType, factory);
            return factory();
        }

        public T CreateHandler<T>() where T : class, ICommandHandler
        {
            return (T)CreateHandler(typeof(T));
        }

        public void RegisterHandler(Type handlerType)
        {
            // 注册处理器类型到缓存中
            if (!_ctorCache.ContainsKey(handlerType))
            {
                var factory = CompileFactory(handlerType);
                _ctorCache.TryAdd(handlerType, factory);
            }
        }

        private Func<ICommandHandler> CompileFactory(Type type)
        {
            var ctors = type.GetConstructors();
            // 选一个参数最多的构造器
            var ctor = ctors.OrderByDescending(c => c.GetParameters().Length).First();

            // 如果 0 参，直接 Activator
            if (!ctor.GetParameters().Any())
                return Expression.Lambda<Func<ICommandHandler>>(
                    Expression.New(ctor)).Compile();

            // 有参 → 从 IServiceProvider 解析
            var parameters = ctor.GetParameters();
            var paramExprs = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var pType = parameters[i].ParameterType;
                var resolveMi = typeof(ServiceProviderServiceExtensions)
                                .GetMethod("GetRequiredService", new[] { typeof(IServiceProvider) })
                                .MakeGenericMethod(pType);
                paramExprs[i] = Expression.Call(null, resolveMi, Expression.Constant(_sp));
            }

            var newExpr = Expression.New(ctor, paramExprs);
            return Expression.Lambda<Func<ICommandHandler>>(newExpr).Compile();
        }
    }
}