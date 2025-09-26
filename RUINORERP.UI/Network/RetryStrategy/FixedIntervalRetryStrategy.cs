using System;

namespace RUINORERP.UI.Network.RetryStrategy
{
    /// <summary>
    /// 固定间隔重试策略
    /// 每次重试使用固定的等待时间间隔
    /// </summary>
    public class FixedIntervalRetryStrategy : IRetryStrategy
    {
        private readonly int _maxAttempts;
        private readonly int _delayMs;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxAttempts">最大重试次数</param>
        /// <param name="delayMs">每次重试的间隔时间（毫秒）</param>
        public FixedIntervalRetryStrategy(int maxAttempts, int delayMs)
        {
            if (maxAttempts < 1)
                throw new ArgumentOutOfRangeException(nameof(maxAttempts), "最大重试次数必须大于0");
            if (delayMs < 0)
                throw new ArgumentOutOfRangeException(nameof(delayMs), "重试间隔时间不能为负数");
            
            _maxAttempts = maxAttempts;
            _delayMs = delayMs;
        }
        
        /// <summary>
        /// 获取下一次重试的等待时间
        /// </summary>
        /// <param name="attempt">当前重试次数</param>
        /// <returns>固定的等待时间（毫秒）</returns>
        public int GetNextDelay(int attempt)
        {
            return _delayMs;
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