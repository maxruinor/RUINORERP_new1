using System;
using System.Threading;

namespace RUINORERP.Server.Network.Models
{
    /// <summary>
    /// 欢迎握手统计 - 记录欢迎握手的统计信息
    /// 用于监控系统健康状况和性能指标
    /// </summary>
    public class WelcomeHandshakeStatistics
    {
        /// <summary>
        /// 总连接数
        /// </summary>
        private long _totalConnections = 0;

        /// <summary>
        /// 成功握手数
        /// </summary>
        private long _successfulHandshakes = 0;

        /// <summary>
        /// 失败握手数
        /// </summary>
        private long _failedHandshakes = 0;

        /// <summary>
        /// 超时握手数
        /// </summary>
        private long _timeoutHandshakes = 0;

        /// <summary>
        /// 版本不兼容握手数
        /// </summary>
        private long _versionIncompatibleHandshakes = 0;

        /// <summary>
        /// 总连接数
        /// </summary>
        public long TotalConnections
        {
            get => Interlocked.Read(ref _totalConnections);
            set => Interlocked.Exchange(ref _totalConnections, value);
        }

        /// <summary>
        /// 成功握手数
        /// </summary>
        public long SuccessfulHandshakes
        {
            get => Interlocked.Read(ref _successfulHandshakes);
            set => Interlocked.Exchange(ref _successfulHandshakes, value);
        }

        /// <summary>
        /// 失败握手数
        /// </summary>
        public long FailedHandshakes
        {
            get => Interlocked.Read(ref _failedHandshakes);
            set => Interlocked.Exchange(ref _failedHandshakes, value);
        }

        /// <summary>
        /// 超时握手数
        /// </summary>
        public long TimeoutHandshakes
        {
            get => Interlocked.Read(ref _timeoutHandshakes);
            set => Interlocked.Exchange(ref _timeoutHandshakes, value);
        }

        /// <summary>
        /// 版本不兼容握手数
        /// </summary>
        public long VersionIncompatibleHandshakes
        {
            get => Interlocked.Read(ref _versionIncompatibleHandshakes);
            set => Interlocked.Exchange(ref _versionIncompatibleHandshakes, value);
        }

        /// <summary>
        /// 握手成功率
        /// </summary>
        public double SuccessRate
        {
            get
            {
                var total = TotalConnections;
                return total > 0 ? (double)SuccessfulHandshakes / total * 100 : 0;
            }
        }

        /// <summary>
        /// 握手失败率
        /// </summary>
        public double FailureRate
        {
            get
            {
                var total = TotalConnections;
                return total > 0 ? (double)FailedHandshakes / total * 100 : 0;
            }
        }

        /// <summary>
        /// 记录欢迎消息发送
        /// </summary>
        public void RecordWelcomeSent()
        {
            Interlocked.Increment(ref _totalConnections);
        }

        /// <summary>
        /// 记录握手成功
        /// </summary>
        public void RecordWelcomeSuccess()
        {
            Interlocked.Increment(ref _successfulHandshakes);
        }

        /// <summary>
        /// 记录握手失败
        /// </summary>
        /// <param name="reason">失败原因</param>
        public void RecordWelcomeFailure(WelcomeFailureReason reason)
        {
            Interlocked.Increment(ref _failedHandshakes);

            switch (reason)
            {
                case WelcomeFailureReason.Timeout:
                    Interlocked.Increment(ref _timeoutHandshakes);
                    break;
                case WelcomeFailureReason.VersionIncompatible:
                    Interlocked.Increment(ref _versionIncompatibleHandshakes);
                    break;
                case WelcomeFailureReason.Other:
                    // 其他原因，只增加失败计数
                    break;
            }
        }

        /// <summary>
        /// 获取统计摘要
        /// </summary>
        /// <returns>统计摘要字符串</returns>
        public string GetSummary()
        {
            return $"欢迎握手统计: 总连接={TotalConnections}, " +
                   $"成功={SuccessfulHandshakes} ({SuccessRate:F2}%), " +
                   $"失败={FailedHandshakes} ({FailureRate:F2}%), " +
                   $"超时={TimeoutHandshakes}, 版本不兼容={VersionIncompatibleHandshakes}";
        }

        /// <summary>
        /// 重置统计数据
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref _totalConnections, 0);
            Interlocked.Exchange(ref _successfulHandshakes, 0);
            Interlocked.Exchange(ref _failedHandshakes, 0);
            Interlocked.Exchange(ref _timeoutHandshakes, 0);
            Interlocked.Exchange(ref _versionIncompatibleHandshakes, 0);
        }
    }

    /// <summary>
    /// 欢迎握手失败原因
    /// </summary>
    public enum WelcomeFailureReason
    {
        /// <summary>
        /// 超时
        /// </summary>
        Timeout = 1,

        /// <summary>
        /// 版本不兼容
        /// </summary>
        VersionIncompatible = 2,

        /// <summary>
        /// 其他原因
        /// </summary>
        Other = 99
    }
}
