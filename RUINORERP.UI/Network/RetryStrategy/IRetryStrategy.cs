using System;

namespace RUINORERP.UI.Network.RetryStrategy
{
    /// <summary>
    /// 重试策略接口
    /// 定义请求重试的行为规范，包括获取下一次重试的等待时间和判断是否应该继续重试
    /// </summary>
    public interface IRetryStrategy
    {
        /// <summary>
        /// 获取下一次重试的等待时间
        /// </summary>
        /// <param name="attempt">当前重试次数</param>
        /// <returns>等待时间（毫秒）</returns>
        int GetNextDelay(int attempt);
        
        /// <summary>
        /// 是否应该继续重试
        /// </summary>
        /// <param name="attempt">当前重试次数</param>
        /// <returns>是否继续重试</returns>
        bool ShouldContinue(int attempt);
    }
}