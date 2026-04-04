using System;
using System.Collections.Concurrent;

namespace RUINORERP.Model.Base.StatusManager.PerformanceMonitoring
{
    /// <summary>
    /// 性能监控桥接器 - 用于在不同层之间传递性能数据
    /// 使用静态事件机制，避免直接依赖
    /// 在Model层定义事件，业务层触发事件，UI层订阅事件
    /// </summary>
    public static class PerformanceMonitoringBridge
    {
        /// <summary>
        /// 方法执行性能数据事件
        /// </summary>
        public static event EventHandler<MethodExecutionData> OnMethodExecution;

        /// <summary>
        /// 数据库操作性能数据事件
        /// </summary>
        public static event EventHandler<DatabaseOperationData> OnDatabaseOperation;

        /// <summary>
        /// 网络请求性能数据事件
        /// </summary>
        public static event EventHandler<NetworkRequestData> OnNetworkRequest;

        /// <summary>
        /// 死锁事件
        /// </summary>
        public static event EventHandler<DeadlockEventData> OnDeadlock;

        /// <summary>
        /// 记录方法执行性能数据
        /// </summary>
        public static void RaiseMethodExecution(string methodName, string className, long executionTimeMs, bool isSuccess, string exceptionMessage = null)
        {
            if (!PerformanceMonitorSwitch.IsEnabled || !PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.MethodExecution))
                return;

            OnMethodExecution?.Invoke(null, new MethodExecutionData
            {
                MethodName = methodName,
                ClassName = className,
                ExecutionTimeMs = executionTimeMs,
                IsSuccess = isSuccess,
                ExceptionMessage = exceptionMessage,
                Timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// 记录数据库操作性能数据
        /// </summary>
        public static void RaiseDatabaseOperation(string sqlText, string operationType, string tableName, long executionTimeMs, int affectedRows, bool isSuccess, string errorMessage = null, bool isDeadlock = false)
        {
            if (!PerformanceMonitorSwitch.IsEnabled || !PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Database))
                return;

            OnDatabaseOperation?.Invoke(null, new DatabaseOperationData
            {
                SqlText = sqlText,
                OperationType = operationType,
                TableName = tableName,
                ExecutionTimeMs = executionTimeMs,
                AffectedRows = affectedRows,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                IsDeadlock = isDeadlock,
                Timestamp = DateTime.Now
            });

            if (isDeadlock)
            {
                OnDeadlock?.Invoke(null, new DeadlockEventData
                {
                    SqlText = sqlText,
                    OperationType = operationType,
                    TableName = tableName,
                    ExecutionTimeMs = executionTimeMs,
                    ErrorMessage = errorMessage,
                    Timestamp = DateTime.Now
                });
            }
        }

        /// <summary>
        /// 记录网络请求性能数据
        /// </summary>
        public static void RaiseNetworkRequest(string commandName, long executionTimeMs, bool isSuccess, string errorMessage = null)
        {
            if (!PerformanceMonitorSwitch.IsEnabled || !PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Network))
                return;

            OnNetworkRequest?.Invoke(null, new NetworkRequestData
            {
                CommandName = commandName,
                ExecutionTimeMs = executionTimeMs,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage,
                Timestamp = DateTime.Now
            });
        }

        /// <summary>
        /// 清除所有事件订阅（用于清理）
        /// </summary>
        public static void ClearAllSubscriptions()
        {
            OnMethodExecution = null;
            OnDatabaseOperation = null;
            OnNetworkRequest = null;
            OnDeadlock = null;
        }
    }

    /// <summary>
    /// 方法执行数据
    /// </summary>
    public class MethodExecutionData
    {
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 数据库操作数据
    /// </summary>
    public class DatabaseOperationData
    {
        public string SqlText { get; set; }
        public string OperationType { get; set; }
        public string TableName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public int AffectedRows { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsDeadlock { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 网络请求数据
    /// </summary>
    public class NetworkRequestData
    {
        public string CommandName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 死锁事件数据
    /// </summary>
    public class DeadlockEventData
    {
        public string SqlText { get; set; }
        public string OperationType { get; set; }
        public string TableName { get; set; }
        public long ExecutionTimeMs { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
