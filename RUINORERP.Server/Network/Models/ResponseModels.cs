using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Models
{
    /// <summary>
    /// 响应处理结果
    /// </summary>
    public class ResponseProcessingResult
    {
        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// 处理消息
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 错误信息（如果失败）
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 处理时间（毫秒）
        /// </summary>
        public long ProcessingTimeMs { get; private set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static ResponseProcessingResult Success(string message = "处理成功")
        {
            return new ResponseProcessingResult
            {
                IsSuccess = true,
                Message = message
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static ResponseProcessingResult Failure(string errorMessage)
        {
            return new ResponseProcessingResult
            {
                IsSuccess = false,
                ErrorMessage = errorMessage
            };
        }
    }

    /// <summary>
    /// 响应统计信息
    /// 使用私有字段以支持 Interlocked 操作
    /// </summary>
    public class ResponseStatistics
    {
        /// <summary>
        /// 总处理的响应数量
        /// </summary>
        public long TotalProcessedResponses { get { return _totalProcessedResponses; } set { _totalProcessedResponses = value; } }
        private long _totalProcessedResponses = 0;

        /// <summary>
        /// 匹配待处理请求的数量
        /// </summary>
        public long MatchedPendingRequests { get { return _matchedPendingRequests; } set { _matchedPendingRequests = value; } }
        private long _matchedPendingRequests = 0;

        /// <summary>
        /// 自定义处理器处理的数量
        /// </summary>
        public long HandledByCustomHandlers { get { return _handledByCustomHandlers; } set { _handledByCustomHandlers = value; } }
        private long _handledByCustomHandlers = 0;

        /// <summary>
        /// 默认处理器处理的数量
        /// </summary>
        public long HandledByDefaultHandler { get { return _handledByDefaultHandler; } set { _handledByDefaultHandler = value; } }
        private long _handledByDefaultHandler = 0;

        /// <summary>
        /// 处理错误数量
        /// </summary>
        public long ProcessingErrors { get { return _processingErrors; } set { _processingErrors = value; } }
        private long _processingErrors = 0;

        /// <summary>
        /// 收到的欢迎确认数量
        /// </summary>
        public long WelcomeAckReceived { get { return _welcomeAckReceived; } set { _welcomeAckReceived = value; } }
        private long _welcomeAckReceived = 0;

        /// <summary>
        /// 欢迎确认错误数量
        /// </summary>
        public long WelcomeAckErrors { get { return _welcomeAckErrors; } set { _welcomeAckErrors = value; } }
        private long _welcomeAckErrors = 0;

        /// <summary>
        /// 总欢迎握手耗时（毫秒）
        /// </summary>
        public long TotalWelcomeHandshakeTimeMs { get { return _totalWelcomeHandshakeTimeMs; } set { _totalWelcomeHandshakeTimeMs = value; } }
        private long _totalWelcomeHandshakeTimeMs = 0;

        /// <summary>
        /// 注册的待处理请求数量
        /// </summary>
        public long PendingRequestsRegistered { get { return _pendingRequestsRegistered; } set { _pendingRequestsRegistered = value; } }
        private long _pendingRequestsRegistered = 0;

        /// <summary>
        /// 移除的待处理请求数量
        /// </summary>
        public long PendingRequestsRemoved { get { return _pendingRequestsRemoved; } set { _pendingRequestsRemoved = value; } }
        private long _pendingRequestsRemoved = 0;

        /// <summary>
        /// 清理的过期待处理请求数量
        /// </summary>
        public long ExpiredPendingRequestsCleaned { get { return _expiredPendingRequestsCleaned; } set { _expiredPendingRequestsCleaned = value; } }
        private long _expiredPendingRequestsCleaned = 0;

        /// <summary>
        /// 当前待处理请求数量
        /// </summary>
        public int CurrentPendingRequestsCount { get; set; }

        /// <summary>
        /// 获取平均欢迎握手耗时（毫秒）
        /// </summary>
        public double GetAverageWelcomeHandshakeTimeMs()
        {
            return WelcomeAckReceived > 0 
                ? (double)TotalWelcomeHandshakeTimeMs / WelcomeAckReceived 
                : 0;
        }

        /// <summary>
        /// 获取处理成功率
        /// </summary>
        public double GetSuccessRate()
        {
            return TotalProcessedResponses > 0 
                ? (double)(TotalProcessedResponses - ProcessingErrors) / TotalProcessedResponses * 100 
                : 0;
        }

        #region 内部字段引用（用于 Interlocked 操作）

        /// <summary>
        /// 增加已处理响应计数（线程安全）
        /// </summary>
        public void IncrementTotalProcessedResponses() => Interlocked.Increment(ref _totalProcessedResponses);

        /// <summary>
        /// 增加匹配待处理请求计数（线程安全）
        /// </summary>
        public void IncrementMatchedPendingRequests() => Interlocked.Increment(ref _matchedPendingRequests);

        /// <summary>
        /// 增加自定义处理器处理计数（线程安全）
        /// </summary>
        public void IncrementHandledByCustomHandlers() => Interlocked.Increment(ref _handledByCustomHandlers);

        /// <summary>
        /// 增加默认处理器处理计数（线程安全）
        /// </summary>
        public void IncrementHandledByDefaultHandler() => Interlocked.Increment(ref _handledByDefaultHandler);

        /// <summary>
        /// 增加处理错误计数（线程安全）
        /// </summary>
        public void IncrementProcessingErrors() => Interlocked.Increment(ref _processingErrors);

        /// <summary>
        /// 增加欢迎确认接收计数（线程安全）
        /// </summary>
        public void IncrementWelcomeAckReceived() => Interlocked.Increment(ref _welcomeAckReceived);

        /// <summary>
        /// 增加欢迎确认错误计数（线程安全）
        /// </summary>
        public void IncrementWelcomeAckErrors() => Interlocked.Increment(ref _welcomeAckErrors);

        /// <summary>
        /// 增加欢迎握手耗时（线程安全）
        /// </summary>
        public void AddTotalWelcomeHandshakeTimeMs(long milliseconds) => Interlocked.Add(ref _totalWelcomeHandshakeTimeMs, milliseconds);

        /// <summary>
        /// 增加待处理请求注册计数（线程安全）
        /// </summary>
        public void IncrementPendingRequestsRegistered() => Interlocked.Increment(ref _pendingRequestsRegistered);

        /// <summary>
        /// 增加待处理请求移除计数（线程安全）
        /// </summary>
        public void IncrementPendingRequestsRemoved() => Interlocked.Increment(ref _pendingRequestsRemoved);

        /// <summary>
        /// 增加过期清理计数（线程安全）
        /// </summary>
        public void AddExpiredPendingRequestsCleaned(int count) => Interlocked.Add(ref _expiredPendingRequestsCleaned, count);

        #endregion
    }
}
