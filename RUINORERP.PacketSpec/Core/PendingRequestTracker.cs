using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Core
{
    /// <summary>
    /// 通用待处理请求追踪器
    /// 统一管理请求注册、完成、超时清理,避免多处重复实现
    /// </summary>
    /// <typeparam name="TResult">响应结果类型</typeparam>
    public class PendingRequestTracker<TResult> : IDisposable
    {
        private class PendingRequestItem
        {
            public TaskCompletionSource<TResult> Tcs { get; set; }
            public DateTime CreatedAt { get; set; }
            public string RequestId { get; set; }
        }

        private readonly ConcurrentDictionary<string, PendingRequestItem> _requests;
        private readonly Timer _cleanupTimer;
        private readonly TimeSpan _defaultTimeout;
        private readonly int _maxPendingCount;
        
        private bool _disposed = false;

        /// <summary>
        /// 当前待处理请求数量
        /// </summary>
        public int Count => _requests.Count;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="defaultTimeout">默认超时时间</param>
        /// <param name="cleanupInterval">清理间隔</param>
        /// <param name="maxPendingCount">最大待处理数量(0=无限制)</param>
        public PendingRequestTracker(
            TimeSpan defaultTimeout,
            TimeSpan cleanupInterval,
            int maxPendingCount = 0)
        {
            _requests = new ConcurrentDictionary<string, PendingRequestItem>();
            _defaultTimeout = defaultTimeout;
            _maxPendingCount = maxPendingCount;
            
            // 定期清理超时请求
            _cleanupTimer = new Timer(CleanupTimeoutRequests, null, cleanupInterval, cleanupInterval);
        }

        /// <summary>
        /// 注册新请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>TaskCompletionSource,用于设置结果</returns>
        /// <exception cref="InvalidOperationException">超过最大待处理数量</exception>
        public TaskCompletionSource<TResult> Register(string requestId)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PendingRequestTracker<TResult>));

            if (string.IsNullOrEmpty(requestId))
                throw new ArgumentException("请求ID不能为空", nameof(requestId));

            // 检查是否超过最大数量
            if (_maxPendingCount > 0 && _requests.Count >= _maxPendingCount)
            {
                throw new InvalidOperationException(
                    $"待处理请求数量已达上限({_maxPendingCount}),请稍后重试");
            }

            var item = new PendingRequestItem
            {
                Tcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously),
                CreatedAt = DateTime.UtcNow,
                RequestId = requestId
            };

            if (!_requests.TryAdd(requestId, item))
            {
                throw new InvalidOperationException($"请求ID已存在: {requestId}");
            }

            return item.Tcs;
        }

        /// <summary>
        /// 尝试完成请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="result">结果</param>
        /// <returns>是否成功完成</returns>
        public bool TryComplete(string requestId, TResult result)
        {
            if (_requests.TryRemove(requestId, out var item))
            {
                return item.Tcs.TrySetResult(result);
            }
            return false;
        }

        /// <summary>
        /// 尝试完成请求(异常)
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="exception">异常</param>
        /// <returns>是否成功完成</returns>
        public bool TryCompleteException(string requestId, Exception exception)
        {
            if (_requests.TryRemove(requestId, out var item))
            {
                return item.Tcs.TrySetException(exception);
            }
            return false;
        }

        /// <summary>
        /// 尝试取消请求
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>是否成功取消</returns>
        public bool TryCancel(string requestId)
        {
            if (_requests.TryRemove(requestId, out var item))
            {
                return item.Tcs.TrySetCanceled();
            }
            return false;
        }

        /// <summary>
        /// 获取请求的创建时间
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <param name="createdAt">创建时间</param>
        /// <returns>是否存在</returns>
        public bool TryGetCreatedAt(string requestId, out DateTime createdAt)
        {
            if (_requests.TryGetValue(requestId, out var item))
            {
                createdAt = item.CreatedAt;
                return true;
            }
            createdAt = default;
            return false;
        }

        /// <summary>
        /// 清理超时请求
        /// </summary>
        /// <param name="timeout">超时时间(null=使用默认值)</param>
        /// <returns>清理的请求数量</returns>
        public int CleanupTimeoutRequests(TimeSpan? timeout = null)
        {
            var actualTimeout = timeout ?? _defaultTimeout;
            var now = DateTime.UtcNow;
            var cleanedCount = 0;

            foreach (var kvp in _requests)
            {
                if ((now - kvp.Value.CreatedAt) > actualTimeout)
                {
                    if (_requests.TryRemove(kvp.Key, out var item))
                    {
                        item.Tcs.TrySetException(new TimeoutException(
                            $"请求超时: {kvp.Key}, 耗时: {(now - item.CreatedAt).TotalSeconds:F1}秒"));
                        cleanedCount++;
                    }
                }
            }

            return cleanedCount;
        }

        /// <summary>
        /// 清理所有请求
        /// </summary>
        /// <param name="exception">设置的异常</param>
        /// <returns>清理的数量</returns>
        public int ClearAll(Exception exception = null)
        {
            var count = 0;
            foreach (var kvp in _requests)
            {
                if (_requests.TryRemove(kvp.Key, out var item))
                {
                    if (exception != null)
                    {
                        item.Tcs.TrySetException(exception);
                    }
                    else
                    {
                        item.Tcs.TrySetCanceled();
                    }
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 清理定时器回调
        /// </summary>
        private void CleanupTimeoutRequests(object state)
        {
            try
            {
                var cleaned = CleanupTimeoutRequests();
                if (cleaned > 0)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[PendingRequestTracker] 清理了 {cleaned} 个超时请求");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[PendingRequestTracker] 清理超时请求失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _cleanupTimer?.Dispose();
                ClearAll(new ObjectDisposedException(nameof(PendingRequestTracker<TResult>)));
                _disposed = true;
            }
        }
    }
}
