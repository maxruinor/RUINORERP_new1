using System;using System.Collections.Generic;using System.Linq;
using RUINORERP.UI.Network.TimeoutStatistics;

namespace RUINORERP.UI.Network.ErrorHandling
{
    /// <summary>
    /// 错误处理策略工厂
    /// 负责创建和管理不同类型的错误处理策略
    /// 根据错误类型返回适当的处理策略
    /// </summary>
    public class ErrorHandlingStrategyFactory
    {
        // 存储错误处理策略的集合
        private readonly Dictionary<NetworkErrorType, IErrorHandlingStrategy> _strategies;
        
        // 默认错误处理策略
        private readonly IErrorHandlingStrategy _defaultStrategy;
        
        /// <summary>
        /// 构造函数
        /// 初始化错误处理策略工厂
        /// </summary>
        public ErrorHandlingStrategyFactory()
        {
            _strategies = new Dictionary<NetworkErrorType, IErrorHandlingStrategy>();
            
            // 注册默认的错误处理策略
            _defaultStrategy = new DefaultErrorHandlingStrategy();
            
            // 注册各种具体的错误处理策略
            RegisterStrategy(new ConnectionErrorHandlingStrategy());
            RegisterStrategy(new AuthenticationErrorHandlingStrategy());
            RegisterStrategy(new TimeoutErrorHandlingStrategy());
            // 可以根据需要注册更多的错误处理策略
        }
        
        /// <summary>
        /// 注册错误处理策略
        /// </summary>
        /// <param name="strategy">错误处理策略</param>
        public void RegisterStrategy(IErrorHandlingStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException(nameof(strategy));
            
            // 确定策略支持的错误类型并注册
            foreach (NetworkErrorType errorType in Enum.GetValues(typeof(NetworkErrorType)))
            {
                if (strategy.SupportsErrorType(errorType))
                {
                    _strategies[errorType] = strategy;
                }
            }
        }
        
        /// <summary>
        /// 获取指定错误类型的处理策略
        /// 如果没有找到特定的处理策略，则返回默认策略
        /// </summary>
        /// <param name="errorType">错误类型</param>
        /// <returns>错误处理策略</returns>
        public IErrorHandlingStrategy GetStrategy(NetworkErrorType errorType)
        {
            if (_strategies.TryGetValue(errorType, out var strategy))
            {
                return strategy;
            }
            return _defaultStrategy;
        }
        
        /// <summary>
        /// 默认错误处理策略
        /// 用于处理没有特定策略的错误类型
        /// </summary>
        private class DefaultErrorHandlingStrategy : IErrorHandlingStrategy
        {
            public async System.Threading.Tasks.Task HandleErrorAsync(NetworkErrorType errorType, string errorMessage, string commandId)
            {
                // 实现默认的错误处理逻辑
                // 例如：记录错误日志、向用户显示通用错误消息等
                
                // 这里仅作为示例，实际实现需要根据项目具体需求进行完善
                
                await System.Threading.Tasks.Task.CompletedTask;
            }
            
            public bool SupportsErrorType(NetworkErrorType errorType)
            {
                // 默认策略支持所有错误类型
                return true;
            }
        }
    }
}