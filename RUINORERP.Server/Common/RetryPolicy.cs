using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RUINORERP.Server.Common
{
    /// <summary>
    /// 统一重试策略类
    /// 提供可配置的重试机制，处理临时性错误
    /// </summary>
    public class RetryPolicy
    {
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy _defaultPolicy;
        private readonly AsyncRetryPolicy _databasePolicy;
        private readonly AsyncRetryPolicy _networkPolicy;

        /// <summary>
        /// 重试配置
        /// </summary>
        public class RetryConfig
        {
            /// <summary>最大重试次数</summary>
            public int MaxRetries { get; set; } = 3;

            /// <summary>初始重试延迟（毫秒）</summary>
            public int InitialDelayMs { get; set; } = 100;

            /// <summary>重试延迟乘数（指数退避）</summary>
            public double BackoffMultiplier { get; set; } = 2.0;

            /// <summary>最大重试延迟（毫秒）</summary>
            public int MaxDelayMs { get; set; } = 5000;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RetryPolicy(ILogger<RetryPolicy> logger, RetryConfig config = null)
        {
            _logger = logger;
            config = config ?? new RetryConfig();

            // 默认重试策略：处理所有异常
            _defaultPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: config.MaxRetries,
                    sleepDurationProvider: retryAttempt =>
                    {
                        var delay = Math.Min(
                            config.InitialDelayMs * Math.Pow(config.BackoffMultiplier, retryAttempt - 1),
                            config.MaxDelayMs);
                        return TimeSpan.FromMilliseconds(delay);
                    },
                    onRetry: (exception, delay, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "重试执行 (尝试 {RetryCount}/{MaxRetries}), 延迟 {Delay}ms",
                            retryCount, config.MaxRetries, delay.TotalMilliseconds);
                    });

            // 数据库重试策略：处理数据库特定异常
            _databasePolicy = Policy
                .Handle<Exception>(ex =>
                {
                    // 处理数据库连接异常、超时、死锁等
                    var message = ex?.Message?.ToLower();
                    return message != null && (
                        message.Contains("timeout") ||
                        message.Contains("connection") ||
                        message.Contains("deadlock") ||
                        message.Contains("locked"));
                })
                .WaitAndRetryAsync(
                    retryCount: config.MaxRetries,
                    sleepDurationProvider: retryAttempt =>
                    {
                        var delay = Math.Min(
                            config.InitialDelayMs * Math.Pow(config.BackoffMultiplier, retryAttempt - 1),
                            config.MaxDelayMs);
                        return TimeSpan.FromMilliseconds(delay);
                    },
                    onRetry: (exception, delay, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "数据库操作重试 (尝试 {RetryCount}/{MaxRetries}), 延迟 {Delay}ms",
                            retryCount, config.MaxRetries, delay.TotalMilliseconds);
                    });

            // 网络重试策略：处理网络异常
            _networkPolicy = Policy
                .Handle<Exception>(ex =>
                {
                    // 处理网络异常、超时、连接拒绝等
                    return ex is TimeoutException ||
                           ex is WebException ||
                           (ex?.Message?.ToLower().Contains("network") == true) ||
                           (ex?.Message?.ToLower().Contains("connection") == true);
                })
                .WaitAndRetryAsync(
                    retryCount: config.MaxRetries,
                    sleepDurationProvider: retryAttempt =>
                    {
                        var delay = Math.Min(
                            config.InitialDelayMs * Math.Pow(config.BackoffMultiplier, retryAttempt - 1),
                            config.MaxDelayMs);
                        return TimeSpan.FromMilliseconds(delay);
                    },
                    onRetry: (exception, delay, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "网络操作重试 (尝试 {RetryCount}/{MaxRetries}), 延迟 {Delay}ms",
                            retryCount, config.MaxRetries, delay.TotalMilliseconds);
                    });
        }

        /// <summary>
        /// 使用默认重试策略执行异步操作
        /// </summary>
        public async Task ExecuteAsync(Func<Task> action, string operationName = null)
        {
            await _defaultPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用默认重试策略执行异步操作并返回结果
        /// </summary>
        public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> func, string operationName = null)
        {
            return await _defaultPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用数据库重试策略执行异步操作
        /// </summary>
        public async Task ExecuteDatabaseAsync(Func<Task> action, string operationName = null)
        {
            await _databasePolicy.ExecuteAsync(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "数据库操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知数据库操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用数据库重试策略执行异步操作并返回结果
        /// </summary>
        public async Task<TResult> ExecuteDatabaseAsync<TResult>(Func<Task<TResult>> func, string operationName = null)
        {
            return await _databasePolicy.ExecuteAsync(async () =>
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "数据库操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知数据库操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用网络重试策略执行异步操作
        /// </summary>
        public async Task ExecuteNetworkAsync(Func<Task> action, string operationName = null)
        {
            await _networkPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "网络操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知网络操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用网络重试策略执行异步操作并返回结果
        /// </summary>
        public async Task<TResult> ExecuteNetworkAsync<TResult>(Func<Task<TResult>> func, string operationName = null)
        {
            return await _networkPolicy.ExecuteAsync(async () =>
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "网络操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知网络操作", 3);
                    throw;
                }
            });
        }

        /// <summary>
        /// 使用自定义重试策略执行异步操作
        /// </summary>
        public async Task ExecuteAsync(Func<Task> action, Func<Exception, bool> shouldRetry, int maxRetries = 3, string operationName = null)
        {
            var policy = Policy
                .Handle<Exception>(shouldRetry)
                .WaitAndRetryAsync(
                    retryCount: maxRetries,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt - 1)),
                    onRetry: (exception, delay, retryCount, context) =>
                    {
                        _logger.LogWarning(exception,
                            "自定义策略重试 (尝试 {RetryCount}/{MaxRetries}), 延迟 {Delay}ms",
                            retryCount, maxRetries, delay.TotalMilliseconds);
                    });

            await policy.ExecuteAsync(async () =>
            {
                try
                {
                    await action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "操作 '{OperationName}' 在 {MaxRetries} 次重试后仍然失败",
                        operationName ?? "未知操作", maxRetries);
                    throw;
                }
            });
        }
    }
}
