using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 业务处理器工厂 - 根据业务复杂度选择合适的处理器
    /// 提供统一的处理器创建和管理机制
    /// </summary>
    public class BusinessProcessorFactory
    {
        /// <summary>
        /// 服务提供器
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<BusinessProcessorFactory> _logger;

        /// <summary>
        /// 处理器缓存
        /// </summary>
        private readonly ConcurrentDictionary<string, ICommandHandler> _handlerCache;

        /// <summary>
        /// 业务复杂度配置
        /// </summary>
        private readonly Dictionary<string, BusinessComplexity> _complexityConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessProcessorFactory(IServiceProvider serviceProvider, ILogger<BusinessProcessorFactory> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerCache = new ConcurrentDictionary<string, ICommandHandler>();
            _complexityConfig = new Dictionary<string, BusinessComplexity>();

            InitializeDefaultConfig();
        }

        /// <summary>
        /// 初始化默认配置
        /// </summary>
        private void InitializeDefaultConfig()
        {
            // 简单业务
            _complexityConfig["SimpleRequest"] = BusinessComplexity.Simple;
            _complexityConfig["HeartbeatRequest"] = BusinessComplexity.Simple;
            _complexityConfig["StatusRequest"] = BusinessComplexity.Simple;

            // 中等复杂度业务
            _complexityConfig["UserRequest"] = BusinessComplexity.Medium;
            _complexityConfig["ProductRequest"] = BusinessComplexity.Medium;
            _complexityConfig["OrderRequest"] = BusinessComplexity.Medium;

            // 复杂业务
            _complexityConfig["LoginRequest"] = BusinessComplexity.Complex;
            _complexityConfig["PaymentRequest"] = BusinessComplexity.Complex;
            _complexityConfig["WorkflowRequest"] = BusinessComplexity.Complex;
        }

        /// <summary>
        /// 创建处理器
        /// </summary>
        /// <param name="requestType">请求类型</param>
        /// <returns>处理器实例</returns>
        public ICommandHandler CreateProcessor(string requestType)
        {
            if (string.IsNullOrEmpty(requestType))
            {
                throw new ArgumentException("请求类型不能为空", nameof(requestType));
            }

            // 检查缓存
            if (_handlerCache.TryGetValue(requestType, out var cachedHandler))
            {
                return cachedHandler;
            }

            // 根据业务复杂度选择合适的处理器
            var complexity = GetBusinessComplexity(requestType);
            var handler = CreateProcessorByComplexity(requestType, complexity);

            // 缓存处理器
            if (handler != null)
            {
                _handlerCache.TryAdd(requestType, handler);
            }

            return handler;
        }

        /// <summary>
        /// 获取业务复杂度
        /// </summary>
        private BusinessComplexity GetBusinessComplexity(string requestType)
        {
            return _complexityConfig.TryGetValue(requestType, out var complexity) ? complexity : BusinessComplexity.Medium;
        }

        /// <summary>
        /// 根据复杂度创建处理器
        /// </summary>
        private ICommandHandler CreateProcessorByComplexity(string requestType, BusinessComplexity complexity)
        {
            _logger.LogInformation($"创建处理器：{requestType}，复杂度：{complexity}");

            return complexity switch
            {
                BusinessComplexity.Simple => CreateSimpleProcessor(requestType),
                BusinessComplexity.Medium => CreateMediumProcessor(requestType),
                BusinessComplexity.Complex => CreateComplexProcessor(requestType),
                _ => CreateDefaultProcessor(requestType)
            };
        }

        /// <summary>
        /// 创建简单业务处理器
        /// </summary>
        private ICommandHandler CreateSimpleProcessor(string requestType)
        {
            _logger.LogInformation($"创建简单业务处理器：{requestType}");
            return ActivatorUtilities.CreateInstance<SimpleBusinessHandler>(_serviceProvider);
        }

        /// <summary>
        /// 创建中等复杂度处理器
        /// </summary>
        private ICommandHandler CreateMediumProcessor(string requestType)
        {
            _logger.LogInformation($"创建中等复杂度处理器：{requestType}");

            return requestType switch
            {
                "UserRequest" => ActivatorUtilities.CreateInstance<UserCrudHandler>(_serviceProvider),
                _ => CreateGenericCrudProcessor(requestType)
            };
        }

        /// <summary>
        /// 创建复杂业务处理器
        /// </summary>
        private ICommandHandler CreateComplexProcessor(string requestType)
        {
            _logger.LogInformation($"创建复杂业务处理器：{requestType}");

            // 复杂业务使用专门的强类型处理器
            return requestType switch
            {
                "LoginRequest" => ActivatorUtilities.CreateInstance<LoginCommandHandler>(_serviceProvider),
                _ => CreateDynamicProcessor(requestType)
            };
        }

        /// <summary>
        /// 创建默认处理器
        /// </summary>
        private ICommandHandler CreateDefaultProcessor(string requestType)
        {
            _logger.LogInformation($"创建默认处理器：{requestType}");
            return CreateDynamicProcessor(requestType);
        }

        /// <summary>
        /// 创建泛型CRUD处理器
        /// </summary>
        private ICommandHandler CreateGenericCrudProcessor(string requestType)
        {
            // 这里可以根据请求类型动态创建泛型CRUD处理器
            // 实际项目中可以通过反射创建泛型类型
            _logger.LogInformation($"创建泛型CRUD处理器：{requestType}");
            return ActivatorUtilities.CreateInstance<UserCrudHandler>(_serviceProvider); // 默认使用用户CRUD处理器作为示例
        }

        /// <summary>
        /// 创建动态处理器
        /// </summary>
        private ICommandHandler CreateDynamicProcessor(string requestType)
        {
            _logger.LogInformation($"创建动态处理器：{requestType}");

            // 创建动态路由配置
            var config = new DynamicRouterConfig();
            config.Routes.Add(new DynamicRouterConfig.RouteConfig
            {
                CommandType = requestType,
                HandlerTypeName = typeof(FallbackGenericCommandHandler).FullName,
                HandlerType = typeof(FallbackGenericCommandHandler),
                Priority = 0
            });

            var dynamicRouter = ActivatorUtilities.CreateInstance<DynamicCommandRouter>(_serviceProvider, config);
            return dynamicRouter;
        }

        /// <summary>
        /// 配置业务复杂度
        /// </summary>
        public void ConfigureComplexity(string requestType, BusinessComplexity complexity)
        {
            _complexityConfig[requestType] = complexity;
            _logger.LogInformation($"配置业务复杂度：{requestType} -> {complexity}");

            // 清除缓存
            _handlerCache.TryRemove(requestType, out _);
        }

        /// <summary>
        /// 批量配置业务复杂度
        /// </summary>
        public void ConfigureComplexities(Dictionary<string, BusinessComplexity> configurations)
        {
            foreach (var config in configurations)
            {
                ConfigureComplexity(config.Key, config.Value);
            }
        }

        /// <summary>
        /// 清除处理器缓存
        /// </summary>
        public void ClearCache()
        {
            _handlerCache.Clear();
            _logger.LogInformation("处理器缓存已清除");
        }

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        public ProcessorStatistics GetStatistics()
        {
            return new ProcessorStatistics
            {
                CachedHandlersCount = _handlerCache.Count,
                ComplexityConfigurationsCount = _complexityConfig.Count,
                SimpleHandlersCount = _complexityConfig.Count(c => c.Value == BusinessComplexity.Simple),
                MediumHandlersCount = _complexityConfig.Count(c => c.Value == BusinessComplexity.Medium),
                ComplexHandlersCount = _complexityConfig.Count(c => c.Value == BusinessComplexity.Complex)
            };
        }
    }

    /// <summary>
    /// 业务复杂度枚举
    /// </summary>
    public enum BusinessComplexity
    {
        /// <summary>
        /// 简单业务 - 使用SimpleRequest/SimpleResponse
        /// </summary>
        Simple,

        /// <summary>
        /// 中等复杂度 - 使用泛型CRUD处理器
        /// </summary>
        Medium,

        /// <summary>
        /// 复杂业务 - 使用专用强类型处理器
        /// </summary>
        Complex
    }

    /// <summary>
    /// 处理器统计信息
    /// </summary>
    public class ProcessorStatistics
    {
        /// <summary>
        /// 缓存的处理器数量
        /// </summary>
        public int CachedHandlersCount { get; set; }

        /// <summary>
        /// 复杂度配置数量
        /// </summary>
        public int ComplexityConfigurationsCount { get; set; }

        /// <summary>
        /// 简单处理器数量
        /// </summary>
        public int SimpleHandlersCount { get; set; }

        /// <summary>
        /// 中等复杂度处理器数量
        /// </summary>
        public int MediumHandlersCount { get; set; }

        /// <summary>
        /// 复杂处理器数量
        /// </summary>
        public int ComplexHandlersCount { get; set; }
    }
}