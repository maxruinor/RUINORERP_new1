using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Lock;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 锁请求去重和限流器
    /// 
    /// 职责：
    /// - 防止短时间内重复的锁请求（去重）
    /// - 限制并发请求数量（限流）
    /// - 自动清理已完成的请求（资源管理）
    /// 
    /// 设计原则：
    /// - 单一职责：只负责请求去重和限流
    /// - 线程安全：使用 ConcurrentDictionary 和 SemaphoreSlim
    /// - 自动清理：定时清理已完成的请求，避免内存泄漏
    /// </summary>
    public class LockRequestDeduplicator : IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 待处理请求字典
        /// Key: 请求唯一标识（如 "CheckLock_123_456"）
        /// Value: 正在执行的请求任务
        /// </summary>
        private readonly ConcurrentDictionary<string, Task<LockResponse>> _pendingRequests;

        /// <summary>
        /// 并发控制信号量
        /// 限制同时执行的请求数量，防止服务器过载
        /// </summary>
        private readonly SemaphoreSlim _requestSemaphore;

        /// <summary>
        /// 清理定时器
        /// 定期清理已完成的请求，释放内存
        /// </summary>
        private readonly Timer _cleanupTimer;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<LockRequestDeduplicator> _logger;

        /// <summary>
        /// 是否已释放资源
        /// </summary>
        private bool _isDisposed = false;

        /// <summary>
        /// 最大并发请求数
        /// </summary>
        private readonly int _maxConcurrentRequests;

        /// <summary>
        /// 清理间隔（秒）
        /// </summary>
        private const int CLEANUP_INTERVAL_SECONDS = 30;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxConcurrentRequests">最大并发请求数，默认10</param>
        /// <param name="logger">日志记录器</param>
        public LockRequestDeduplicator(int maxConcurrentRequests = 10, ILogger<LockRequestDeduplicator> logger = null)
        {
            _maxConcurrentRequests = maxConcurrentRequests;
            _logger = logger;
            _pendingRequests = new ConcurrentDictionary<string, Task<LockResponse>>();
            _requestSemaphore = new SemaphoreSlim(maxConcurrentRequests, maxConcurrentRequests);
            
            // 启动清理定时器，每30秒清理一次已完成的请求
            _cleanupTimer = new Timer(CleanupCompletedRequests, null,
                TimeSpan.FromSeconds(CLEANUP_INTERVAL_SECONDS),
                TimeSpan.FromSeconds(CLEANUP_INTERVAL_SECONDS));
        }

        #endregion

        #region 核心方法

        /// <summary>
        /// 执行带去重和限流的请求
        /// 
        /// 工作流程：
        /// 1. 检查是否有相同的请求正在执行，如果有则返回现有任务（去重）
        /// 2. 获取信号量许可，限制并发数（限流）
        /// 3. 执行实际请求
        /// 4. 清理资源（移除待处理记录、释放信号量）
        /// </summary>
        /// <param name="requestKey">请求唯一标识</param>
        /// <param name="operation">实际执行的异步操作</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>锁响应结果</returns>
        public async Task<LockResponse> ExecuteWithDeduplicationAsync(
            string requestKey,
            Func<Task<LockResponse>> operation,
            CancellationToken cancellationToken = default)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(LockRequestDeduplicator));

            if (string.IsNullOrEmpty(requestKey))
                throw new ArgumentException("请求Key不能为空", nameof(requestKey));

            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            // 步骤1: 检查是否有相同请求正在执行（去重）
            if (_pendingRequests.TryGetValue(requestKey, out var existingTask))
            {
                _logger?.LogDebug("检测到重复请求，返回现有任务: {RequestKey}", requestKey);
                return await existingTask;
            }

            // 步骤2: 获取信号量许可（限流）
            var semaphoreAcquired = false;
            try
            {
                semaphoreAcquired = await _requestSemaphore.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
                if (!semaphoreAcquired)
                {
                    _logger?.LogWarning("获取信号量超时，请求被拒绝: {RequestKey}", requestKey);
                    return LockResponseFactory.CreateFailedResponse("系统繁忙，请稍后重试");
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogDebug("请求被取消: {RequestKey}", requestKey);
                return LockResponseFactory.CreateFailedResponse("请求已取消");
            }

            // 双重检查：在获取信号量后再次检查是否有重复请求
            if (_pendingRequests.TryGetValue(requestKey, out existingTask))
            {
                _requestSemaphore.Release();
                _logger?.LogDebug("双重检查发现重复请求，返回现有任务: {RequestKey}", requestKey);
                return await existingTask;
            }

            // 步骤3: 创建并注册新任务
            var tcs = new TaskCompletionSource<LockResponse>(TaskCreationOptions.RunContinuationsAsynchronously);
            _pendingRequests.TryAdd(requestKey, tcs.Task);

            try
            {
                // 执行实际操作
                var result = await operation();
                
                // 完成任务
                tcs.TrySetResult(result);
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "执行锁请求时发生异常: {RequestKey}", requestKey);
                
                // 设置异常结果
                tcs.TrySetException(ex);
                
                // 返回错误响应
                return LockResponseFactory.CreateFailedResponse($"请求执行失败: {ex.Message}");
            }
            finally
            {
                // 步骤4: 清理资源
                _pendingRequests.TryRemove(requestKey, out _);
                
                if (semaphoreAcquired)
                {
                    _requestSemaphore.Release();
                }
            }
        }

        /// <summary>
        /// 获取当前待处理请求数量
        /// </summary>
        public int PendingRequestCount => _pendingRequests.Count;

        /// <summary>
        /// 获取当前可用信号量数量
        /// </summary>
        public int AvailableSemaphoreCount => _requestSemaphore.CurrentCount;

        #endregion

        #region 私有方法

        /// <summary>
        /// 清理已完成的请求
        /// 由定时器定期调用，防止内存泄漏
        /// </summary>
        private void CleanupCompletedRequests(object state)
        {
            if (_isDisposed)
                return;

            try
            {
                // 查找所有已完成的请求
                var completedKeys = _pendingRequests
                    .Where(kvp => kvp.Value.IsCompleted)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // 移除已完成的请求
                foreach (var key in completedKeys)
                {
                    _pendingRequests.TryRemove(key, out _);
                }

                // 记录日志（仅在有需要清理时）
                if (completedKeys.Count > 0 && _logger != null)
                {
                    _logger.LogDebug("清理了 {Count} 个已完成的待处理请求", completedKeys.Count);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理待处理请求时发生错误");
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源（受保护版本）
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // 停止清理定时器
                    _cleanupTimer?.Dispose();

                    // 释放信号量
                    _requestSemaphore?.Dispose();

                    // 清空待处理请求
                    _pendingRequests.Clear();
                }

                _isDisposed = true;
            }
        }

        ~LockRequestDeduplicator()
        {
            Dispose(false);
        }

        #endregion
    }
}
