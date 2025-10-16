using System;

namespace RUINORERP.UI.Network.RetryStrategy
{
    /// <summary>
    /// 指数退避重试策略
    /// 重试间隔时间呈指数增长，避免在网络拥塞时持续重试导致情况恶化
    /// </summary>
    public class ExponentialBackoffRetryStrategy : IRetryStrategy
    {
        private readonly int _maxAttempts;
        private readonly int _initialDelayMs;
        private readonly double _multiplier;
        private readonly int _maxDelayMs;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxAttempts">最大重试次数</param>
        /// <param name="initialDelayMs">初始重试间隔（毫秒）</param>
        /// <param name="multiplier">指数乘数因子</param>
        /// <param name="maxDelayMs">最大重试间隔（毫秒）</param>
        public ExponentialBackoffRetryStrategy(int maxAttempts = 3, int initialDelayMs = 1000, double multiplier = 2.0, int maxDelayMs = 30000)
        {
            if (maxAttempts < 1)
                throw new ArgumentOutOfRangeException(nameof(maxAttempts), "最大重试次数必须大于0");
            if (initialDelayMs < 0)
                throw new ArgumentOutOfRangeException(nameof(initialDelayMs), "初始重试间隔不能为负数");
            if (multiplier <= 1.0)
                throw new ArgumentOutOfRangeException(nameof(multiplier), "乘数因子必须大于1");
            if (maxDelayMs < initialDelayMs)
                throw new ArgumentOutOfRangeException(nameof(maxDelayMs), "最大重试间隔不能小于初始重试间隔");
            
            _maxAttempts = maxAttempts;
            _initialDelayMs = initialDelayMs;
            _multiplier = multiplier;
            _maxDelayMs = maxDelayMs;
        }
        
        /// <summary>
        /// 获取下一次重试的等待时间
        /// 计算公式：initialDelayMs * (multiplier ^ attempt)
        /// 结果不超过maxDelayMs
        /// </summary>
        /// <param name="attempt">当前重试次数</param>
        /// <returns>指数增长的等待时间（毫秒）</returns>
        public int GetNextDelay(int attempt)
        {
            // 计算指数退避延迟，并限制最大值
            double delay = _initialDelayMs * Math.Pow(_multiplier, attempt - 1); // 从第0次开始计算
            return Math.Min((int)delay, _maxDelayMs);
        }
        
        /// <summary>
        /// 判断是否应该继续重试
        /// </summary>
        /// <param name="attempt">当前重试次数</param>
        /// <returns>如果当前重试次数小于最大重试次数，则返回true，否则返回false</returns>
        public bool ShouldContinue(int attempt)
        {
            return attempt < _maxAttempts;
        }
    }
}