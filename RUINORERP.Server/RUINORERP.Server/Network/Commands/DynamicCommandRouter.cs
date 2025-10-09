using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// 动态路由处理器 - 根据配置动态路由命令到对应的处理器
    /// 支持运行时注册和配置命令处理逻辑
    /// </summary>
    public class DynamicCommandRouter : BaseCommandHandler
    {
        /// <summary>
        /// 命令路由表 - 存储命令类型到处理逻辑的映射
        /// </summary>
        private readonly ConcurrentDictionary<string, CommandRouteInfo> _commandRoutes;

        /// <summary>
        /// 服务提供器 - 用于获取依赖服务
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 动态路由配置
        /// </summary>
        private readonly DynamicRouterConfig _config;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<DynamicCommandRouter> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DynamicCommandRouter(IServiceProvider serviceProvider, DynamicRouterConfig config, ILogger<DynamicCommandRouter> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _commandRoutes = new ConcurrentDictionary<string, CommandRouteInfo>();

            InitializeRoutes();
        }

        /// <summary>
        /// 初始化路由配置
        /// </summary>
        private void InitializeRoutes()
        {
            if (_config.Routes == null || !_config.Routes.Any())
            {
                _logger.LogWarning("动态路由配置为空，未注册任何路由");
                return;
            }

            foreach (var route in _config.Routes)
            {
                RegisterRoute(route.CommandType, route.HandlerType, route.Priority);
            }

            _logger.LogInformation($"动态路由初始化完成，共注册 {_commandRoutes.Count} 个路由");
        }

        /// <summary>
        /// 注册命令路由
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="handlerType">处理器类型</param>
        /// <param name="priority">优先级</param>
        public void RegisterRoute(string commandType, Type handlerType, int priority = 0)
        {
            if (string.IsNullOrEmpty(commandType))
            {
                throw new ArgumentException("命令类型不能为空", nameof(commandType));
            }

            if (handlerType == null)
            {
                throw new ArgumentNullException(nameof(handlerType));
            }

            if (!typeof(ICommandHandler).IsAssignableFrom(handlerType))
            {
                throw new ArgumentException($"类型 {handlerType.Name} 必须实现 ICommandHandler 接口", nameof(handlerType));
            }

            var routeInfo = new CommandRouteInfo
            {
                CommandType = commandType,
                HandlerType = handlerType,
                Priority = priority,
                CreatedTime = DateTime.UtcNow
            };

            _commandRoutes.AddOrUpdate(commandType, routeInfo, (key, existing) =>
            {
                if (priority > existing.Priority)
                {
                    _logger.LogInformation($"更新命令路由 {commandType}，处理器从 {existing.HandlerType.Name} 变更为 {handlerType.Name}");
                    return routeInfo;
                }
                return existing;
            });

            _logger.LogInformation($"注册命令路由：{commandType} -> {handlerType.Name} (优先级: {priority})");
        }

        /// <summary>
        /// 取消注册命令路由
        /// </summary>
        /// <param name="commandType">命令类型</param>
        public void UnregisterRoute(string commandType)
        {
            if (_commandRoutes.TryRemove(commandType, out var removedRoute))
            {
                _logger.LogInformation($"取消注册命令路由：{commandType} -> {removedRoute.HandlerType.Name}");
            }
        }

        /// <summary>
        /// 获取命令路由信息
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <returns>路由信息</returns>
        public CommandRouteInfo GetRouteInfo(string commandType)
        {
            return _commandRoutes.TryGetValue(commandType, out var routeInfo) ? routeInfo : null;
        }

        /// <summary>
        /// 获取所有已注册的路由
        /// </summary>
        /// <returns>路由信息列表</returns>
        public IEnumerable<CommandRouteInfo> GetAllRoutes()
        {
            return _commandRoutes.Values.OrderByDescending(r => r.Priority).ThenBy(r => r.CreatedTime);
        }

        /// <summary>
        /// 执行核心处理逻辑
        /// </summary>
        /// <param name="cmd">队列命令</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>响应结果</returns>
        protected override async Task<ResponseBase> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            var request = cmd?.Command;
            var commandType = request?.GetType().Name ?? "Unknown";
            
            if (!_commandRoutes.TryGetValue(commandType, out var routeInfo))
            {
                _logger.LogWarning($"未找到命令类型 {commandType} 的路由配置");
                return ResponseBase.CreateError($"不支持的命令类型: {commandType}");
            }

            try
            {
                _logger.LogInformation($"动态路由：{commandType} -> {routeInfo.HandlerType.Name}");

                // 创建处理器实例
                var handler = ActivatorUtilities.CreateInstance(_serviceProvider, routeInfo.HandlerType) as ICommandHandler;
                if (handler == null)
                {
                    return ResponseBase.CreateError($"无法创建处理器实例: {routeInfo.HandlerType.Name}");
                }

                // 执行命令处理
                var result = await handler.HandleAsync(cmd, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"动态路由处理失败：{commandType} -> {routeInfo.HandlerType.Name}");
                return ResponseBase.CreateError($"处理命令时发生错误: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// 命令路由信息
    /// </summary>
    public class CommandRouteInfo
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public string CommandType { get; set; }

        /// <summary>
        /// 处理器类型
        /// </summary>
        public Type HandlerType { get; set; }

        /// <summary>
        /// 优先级（数值越大优先级越高）
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 动态路由配置
    /// </summary>
    public class DynamicRouterConfig
    {
        /// <summary>
        /// 路由配置列表
        /// </summary>
        public List<RouteConfig> Routes { get; set; } = new List<RouteConfig>();

        /// <summary>
        /// 是否启用动态路由
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 默认处理器类型（当找不到匹配路由时使用）
        /// </summary>
        public Type DefaultHandlerType { get; set; }

        /// <summary>
        /// 路由配置
        /// </summary>
        public class RouteConfig
        {
            /// <summary>
            /// 命令类型名称
            /// </summary>
            public string CommandType { get; set; }

            /// <summary>
            /// 处理器类型名称（完整类型名）
            /// </summary>
            public string HandlerTypeName { get; set; }

            /// <summary>
            /// 处理器类型（运行时解析）
            /// </summary>
            public Type HandlerType { get; set; }

            /// <summary>
            /// 优先级
            /// </summary>
            public int Priority { get; set; }

            /// <summary>
            /// 描述信息
            /// </summary>
            public string Description { get; set; }
        }
    }
}