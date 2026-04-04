using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;

namespace RUINORERP.Business.Helper
{
    /// <summary>
    /// 死锁重试帮助类
    /// </summary>
    public static class DeadlockRetryHelper
    {
        private const int MaxRetries = 3;
        private const int BaseDelayMs = 100;

        /// <summary>
        /// 带死锁重试的执行方法
        /// </summary>
        public static async Task<T> ExecuteWithDeadlockRetry<T>(
            Func<Task<T>> operation,
            ILogger logger,
            string operationName = "数据库操作",
            string tableName = "")
        {
            var stopwatch = Stopwatch.StartNew();
            Exception lastException = null;
            bool isDeadlock = false;

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var result = await operation();
                    stopwatch.Stop();

                    // 记录成功操作
                    PerformanceMonitoringBridge.RaiseDatabaseOperation(
                        sqlText: operationName,
                        operationType: "ExecuteWithRetry",
                        tableName: tableName,
                        executionTimeMs: stopwatch.ElapsedMilliseconds,
                        affectedRows: 0,
                        isSuccess: true);

                    return result;
                }
                catch (Exception ex) when (IsDeadlockException(ex) && attempt < MaxRetries)
                {
                    lastException = ex;
                    isDeadlock = true;

                    // 记录死锁事件
                    PerformanceMonitoringBridge.RaiseDatabaseOperation(
                        sqlText: operationName,
                        operationType: "ExecuteWithRetry",
                        tableName: tableName,
                        executionTimeMs: stopwatch.ElapsedMilliseconds,
                        affectedRows: 0,
                        isSuccess: false,
                        errorMessage: ex.Message,
                        isDeadlock: true);

                    // 指数退避：100ms, 200ms, 400ms
                    int delay = BaseDelayMs * (int)Math.Pow(2, attempt - 1);

                    logger.LogWarning(
                        $"{operationName} 检测到死锁，第 {attempt}/{MaxRetries} 次重试，" +
                        $"等待 {delay}ms - 异常: {ex.Message}");

                    await Task.Delay(delay);
                    stopwatch.Restart();
                }
            }

            stopwatch.Stop();

            // 记录最终失败
            PerformanceMonitoringBridge.RaiseDatabaseOperation(
                sqlText: operationName,
                operationType: "ExecuteWithRetry",
                tableName: tableName,
                executionTimeMs: stopwatch.ElapsedMilliseconds,
                affectedRows: 0,
                isSuccess: false,
                errorMessage: lastException?.Message,
                isDeadlock: isDeadlock);

            throw new InvalidOperationException(
                $"{operationName} 在 {MaxRetries} 次重试后仍然失败", lastException);
        }

        /// <summary>
        /// 同步版本带死锁重试的执行方法
        /// </summary>
        public static T ExecuteWithDeadlockRetry<T>(
            Func<T> operation,
            ILogger logger,
            string operationName = "数据库操作",
            string tableName = "")
        {
            var stopwatch = Stopwatch.StartNew();
            Exception lastException = null;
            bool isDeadlock = false;

            for (int attempt = 1; attempt <= MaxRetries; attempt++)
            {
                try
                {
                    var result = operation();
                    stopwatch.Stop();

                    // 记录成功操作
                    PerformanceMonitoringBridge.RaiseDatabaseOperation(
                        sqlText: operationName,
                        operationType: "ExecuteWithRetry",
                        tableName: tableName,
                        executionTimeMs: stopwatch.ElapsedMilliseconds,
                        affectedRows: 0,
                        isSuccess: true);

                    return result;
                }
                catch (Exception ex) when (IsDeadlockException(ex) && attempt < MaxRetries)
                {
                    lastException = ex;
                    isDeadlock = true;

                    // 记录死锁事件
                    PerformanceMonitoringBridge.RaiseDatabaseOperation(
                        sqlText: operationName,
                        operationType: "ExecuteWithRetry",
                        tableName: tableName,
                        executionTimeMs: stopwatch.ElapsedMilliseconds,
                        affectedRows: 0,
                        isSuccess: false,
                        errorMessage: ex.Message,
                        isDeadlock: true);

                    // 指数退避：100ms, 200ms, 400ms
                    int delay = BaseDelayMs * (int)Math.Pow(2, attempt - 1);

                    logger.LogWarning(
                        $"{operationName} 检测到死锁，第 {attempt}/{MaxRetries} 次重试，" +
                        $"等待 {delay}ms - 异常: {ex.Message}");

                    Task.Delay(delay).GetAwaiter().GetResult();
                    stopwatch.Restart();
                }
            }

            stopwatch.Stop();

            // 记录最终失败
            PerformanceMonitoringBridge.RaiseDatabaseOperation(
                sqlText: operationName,
                operationType: "ExecuteWithRetry",
                tableName: tableName,
                executionTimeMs: stopwatch.ElapsedMilliseconds,
                affectedRows: 0,
                isSuccess: false,
                errorMessage: lastException?.Message,
                isDeadlock: isDeadlock);

            throw new InvalidOperationException(
                $"{operationName} 在 {MaxRetries} 次重试后仍然失败", lastException);
        }
        
        /// <summary>
        /// 检测是否为死锁异常
        /// </summary>
        public static bool IsDeadlockException(Exception ex)
        {
            if (ex == null) return false;

            string message = ex.Message.ToLower();
            return message.Contains("deadlock") ||
                   message.Contains("1205") ||
                   message.Contains("1092") ||
                   message.Contains("lock") ||
                   message.Contains("timeout") ||
                   message.Contains("was deadlocked");
        }
    }
}