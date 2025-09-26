using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.RetryStrategy
{
    /// <summary>
    /// 增强的重试策略
    /// 提供更灵活和智能的重试机制
    /// </summary>
    public class EnhancedRetryStrategy
    {
        private readonly RetryPolicy _policy;

        public EnhancedRetryStrategy(RetryPolicy policy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        /// <summary>
        /// 执行带重试的操作
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="operation">操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>操作结果</returns>
        public async Task<TResult> ExecuteAsync<TResult>(Func<CancellationToken, Task<TResult>> operation, CancellationToken cancellationToken = default)
        {
            var attempt = 0;
            Exception lastException = null;
            var retryContext = new RetryContext();

            while (attempt <= _policy.MaxRetryAttempts)
            {
                try
                {
                    // 执行操作
                    var result = await operation(cancellationToken);
                    
                    // 如果成功，重置失败计数
                    retryContext.ConsecutiveFailures = 0;
                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    attempt++;
                    retryContext.ConsecutiveFailures++;
                    retryContext.LastException = ex;

                    // 检查是否应该重试
                    if (attempt > _policy.MaxRetryAttempts || !_policy.ShouldRetry(retryContext))
                    {
                        throw new RetryException($"操作失败，已重试 {attempt - 1} 次", ex, attempt - 1);
                    }

                    // 计算延迟时间
                    var delay = CalculateDelay(attempt, retryContext);
                    
                    // 触发重试前事件
                    _policy.OnRetry?.Invoke(new RetryAttempt(attempt, delay, ex));

                    // 等待重试延迟
                    if (delay > TimeSpan.Zero)
                    {
                        await Task.Delay(delay, cancellationToken);
                    }
                }
            }

            throw new RetryException($"操作失败，已重试 {_policy.MaxRetryAttempts} 次", lastException, _policy.MaxRetryAttempts);
        }

        /// <summary>
        /// 执行带重试的无返回值操作
        /// </summary>
        /// <param name="operation">操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async (ct) =>
            {
                await operation(ct);
                return true; // 返回一个占位值
            }, cancellationToken);
        }

        /// <summary>
        /// 计算延迟时间
        /// </summary>
        /// <param name="attempt">尝试次数</param>
        /// <param name="context">重试上下文</param>
        /// <returns>延迟时间</returns>
        private TimeSpan CalculateDelay(int attempt, RetryContext context)
        {
            // 使用策略指定的延迟策略
            var delay = _policy.DelayStrategy.GetDelay(attempt, context);

            // 应用抖动（如果启用）
            if (_policy.UseJitter)
            {
                delay = ApplyJitter(delay);
            }

            // 应用最大延迟限制
            if (_policy.MaxDelay.HasValue && delay > _policy.MaxDelay.Value)
            {
                delay = _policy.MaxDelay.Value;
            }

            return delay;
        }

        /// <summary>
        /// 应用抖动
        /// </summary>
        /// <param name="delay">原始延迟</param>
        /// <returns>应用抖动后的延迟</returns>
        private TimeSpan ApplyJitter(TimeSpan delay)
        {
            var jitter = delay.TotalMilliseconds * 0.1; // 10%的抖动
            var random = new Random();
            var jitterValue = (random.NextDouble() * 2 - 1) * jitter; // -10% 到 +10%
            return TimeSpan.FromMilliseconds(Math.Max(0, delay.TotalMilliseconds + jitterValue));
        }
    }

    /// <summary>
    /// 重试策略
    /// </summary>
    public class RetryPolicy
    {
        /// <summary>
        /// 最大重试次数
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// 延迟策略
        /// </summary>
        public IDelayStrategy DelayStrategy { get; set; } = new ExponentialBackoffDelayStrategy();

        /// <summary>
        /// 是否使用抖动
        /// </summary>
        public bool UseJitter { get; set; } = true;

        /// <summary>
        /// 最大延迟时间
        /// </summary>
        public TimeSpan? MaxDelay { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 重试条件判断函数
        /// </summary>
        public Func<RetryContext, bool> ShouldRetry { get; set; } = DefaultShouldRetry;

        /// <summary>
        /// 重试时的回调函数
        /// </summary>
        public Action<RetryAttempt> OnRetry { get; set; }

        /// <summary>
        /// 默认重试条件
        /// </summary>
        /// <param name="context">重试上下文</param>
        /// <returns>是否应该重试</returns>
        private static bool DefaultShouldRetry(RetryContext context)
        {
            // 不重试以下类型的异常
            return !(context.LastException is OperationCanceledException);
        }

        /// <summary>
        /// 创建线性重试策略
        /// </summary>
        /// <param name="maxRetryAttempts">最大重试次数</param>
        /// <param name="delay">固定延迟时间</param>
        /// <returns>重试策略</returns>
        public static RetryPolicy CreateLinearRetry(int maxRetryAttempts, TimeSpan delay)
        {
            return new RetryPolicy
            {
                MaxRetryAttempts = maxRetryAttempts,
                DelayStrategy = new LinearDelayStrategy(delay)
            };
        }

        /// <summary>
        /// 创建指数退避重试策略
        /// </summary>
        /// <param name="maxRetryAttempts">最大重试次数</param>
        /// <param name="initialDelay">初始延迟时间</param>
        /// <param name="maxDelay">最大延迟时间</param>
        /// <returns>重试策略</returns>
        public static RetryPolicy CreateExponentialBackoffRetry(int maxRetryAttempts, TimeSpan initialDelay, TimeSpan? maxDelay = null)
        {
            return new RetryPolicy
            {
                MaxRetryAttempts = maxRetryAttempts,
                DelayStrategy = new ExponentialBackoffDelayStrategy(initialDelay),
                MaxDelay = maxDelay
            };
        }

        /// <summary>
        /// 创建基于异常类型的重试策略
        /// </summary>
        /// <param name="maxRetryAttempts">最大重试次数</param>
        /// <param name="retryableExceptions">可重试的异常类型</param>
        /// <returns>重试策略</returns>
        public static RetryPolicy CreateExceptionBasedRetry(int maxRetryAttempts, params Type[] retryableExceptions)
        {
            return new RetryPolicy
            {
                MaxRetryAttempts = maxRetryAttempts,
                ShouldRetry = context => retryableExceptions.Length == 0 || 
                    Array.Exists(retryableExceptions, t => t.IsInstanceOfType(context.LastException))
            };
        }
    }

    /// <summary>
    /// 延迟策略接口
    /// </summary>
    public interface IDelayStrategy
    {
        /// <summary>
        /// 获取延迟时间
        /// </summary>
        /// <param name="attempt">尝试次数</param>
        /// <param name="context">重试上下文</param>
        /// <returns>延迟时间</returns>
        TimeSpan GetDelay(int attempt, RetryContext context);
    }

    /// <summary>
    /// 线性延迟策略
    /// </summary>
    public class LinearDelayStrategy : IDelayStrategy
    {
        private readonly TimeSpan _delay;

        public LinearDelayStrategy(TimeSpan delay)
        {
            _delay = delay;
        }

        public TimeSpan GetDelay(int attempt, RetryContext context)
        {
            return _delay;
        }
    }

    /// <summary>
    /// 指数退避延迟策略
    /// </summary>
    public class ExponentialBackoffDelayStrategy : IDelayStrategy
    {
        private readonly TimeSpan _initialDelay;

        public ExponentialBackoffDelayStrategy(TimeSpan initialDelay)
        {
            _initialDelay = initialDelay;
        }

        public ExponentialBackoffDelayStrategy()
        {
            _initialDelay = TimeSpan.FromSeconds(1);
        }

        public TimeSpan GetDelay(int attempt, RetryContext context)
        {
            // 指数退避：delay = initialDelay * 2^(attempt-1)
            var multiplier = Math.Pow(2, attempt - 1);
            return TimeSpan.FromMilliseconds(_initialDelay.TotalMilliseconds * multiplier);
        }
    }

    /// <summary>
    /// 重试上下文
    /// </summary>
    public class RetryContext
    {
        /// <summary>
        /// 连续失败次数
        /// </summary>
        public int ConsecutiveFailures { get; set; }

        /// <summary>
        /// 最后一次异常
        /// </summary>
        public Exception LastException { get; set; }

        /// <summary>
        /// 重试历史
        /// </summary>
        public List<RetryAttempt> RetryHistory { get; set; } = new List<RetryAttempt>();
    }

    /// <summary>
    /// 重试尝试信息
    /// </summary>
    public class RetryAttempt
    {
        /// <summary>
        /// 尝试次数
        /// </summary>
        public int AttemptNumber { get; }

        /// <summary>
        /// 延迟时间
        /// </summary>
        public TimeSpan Delay { get; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// 尝试时间
        /// </summary>
        public DateTime AttemptTime { get; }

        public RetryAttempt(int attemptNumber, TimeSpan delay, Exception exception)
        {
            AttemptNumber = attemptNumber;
            Delay = delay;
            Exception = exception;
            AttemptTime = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// 重试异常
    /// </summary>
    public class RetryException : Exception
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; }

        /// <summary>
        /// 最后一次异常
        /// </summary>
        public Exception LastException { get; }

        public RetryException(string message, Exception lastException, int retryCount) : base(message, lastException)
        {
            RetryCount = retryCount;
            LastException = lastException;
        }
    }
}