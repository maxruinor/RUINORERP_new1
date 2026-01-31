using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Threading;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 事务状态枚举
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// 未启动
        /// </summary>
        NotStarted,
        
        /// <summary>
        /// 活跃中
        /// </summary>
        Active,
        
        /// <summary>
        /// 已提交
        /// </summary>
        Committed,
        
        /// <summary>
        /// 已回滚
        /// </summary>
        RolledBack,
        
        /// <summary>
        /// 已超时
        /// </summary>
        Timeout,
        
        /// <summary>
        /// 僵尸事务（异常状态）
        /// </summary>
        Zombie
    }
    
    /// <summary>
    /// 事务上下文信息，用于追踪和管理事务生命周期
    /// </summary>
    public class TransactionContext
    {
        /// <summary>
        /// 事务唯一标识
        /// </summary>
        public Guid TransactionId { get; set; }
        
        /// <summary>
        /// 事务创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 事务最后活动时间
        /// </summary>
        public DateTime LastActivityAt { get; set; }
        
        /// <summary>
        /// 事务超时时间（秒）
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;
        
        /// <summary>
        /// 调用者方法信息
        /// </summary>
        public string CallerMethod { get; set; }
        
        /// <summary>
        /// 调用堆栈信息，用于调试
        /// </summary>
        public string StackTrace { get; set; }
        
        /// <summary>
        /// 当前事务状态
        /// </summary>
        public TransactionStatus Status { get; set; }
        
        /// <summary>
        /// 事务深度（嵌套层级）
        /// </summary>
        public int Depth { get; set; }
        
        /// <summary>
        /// 是否需要回滚
        /// </summary>
        public bool ShouldRollback { get; set; }
        
        /// <summary>
        /// 保存点名称栈（用于嵌套事务）
        /// </summary>
        public ConcurrentStack<string> SavePointStack { get; set; }
        
        /// <summary>
        /// 专用数据库连接（用于隔离事务）
        /// </summary>
        public DbConnection DedicatedConnection { get; set; }
        
        /// <summary>
        /// 原始SqlSugarClient实例
        /// </summary>
        public object OriginalSqlSugarClient { get; set; }
        
        /// <summary>
        /// 关联的AsyncLocal上下文
        /// </summary>
        public object AsyncLocalContext { get; set; }
        
        /// <summary>
        /// 线程ID（用于调试多线程问题）
        /// </summary>
        public int ThreadId { get; set; }
        
        /// <summary>
        /// 异步操作ID（用于追踪异步调用链）
        /// </summary>
        public string OperationId { get; set; }
        
        /// <summary>
        /// 自定义数据字典，可用于存储额外的业务信息
        /// </summary>
        public ConcurrentDictionary<string, object> CustomData { get; set; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public TransactionContext()
        {
            TransactionId = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            LastActivityAt = CreatedAt;
            Status = TransactionStatus.NotStarted;
            SavePointStack = new ConcurrentStack<string>();
            CustomData = new ConcurrentDictionary<string, object>();
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            OperationId = Guid.NewGuid().ToString("N").Substring(0, 8);
        }
        
        /// <summary>
        /// 更新最后活动时间
        /// </summary>
        public void UpdateActivityTime()
        {
            LastActivityAt = DateTime.Now;
        }
        
        /// <summary>
        /// 检查是否已超时
        /// </summary>
        public bool IsTimeout()
        {
            return (DateTime.Now - LastActivityAt).TotalSeconds > TimeoutSeconds;
        }
        
        /// <summary>
        /// 获取事务持续时间
        /// </summary>
        public TimeSpan GetDuration()
        {
            return DateTime.Now - CreatedAt;
        }
        
        /// <summary>
        /// 转换为可读的字符串表示
        /// </summary>
        public override string ToString()
        {
            return $"[Transaction-{TransactionId}] Depth={Depth}, Status={Status}, Caller={CallerMethod}, Thread={ThreadId}, Duration={GetDuration().TotalSeconds:F2}s";
        }
        
        /// <summary>
        /// 获取调试信息
        /// </summary>
        public string GetDebugInfo()
        {
            return $"Transaction ID: {TransactionId}\r\n" +
                   $"Status: {Status}\r\n" +
                   $"Depth: {Depth}\r\n" +
                   $"Created At: {CreatedAt:yyyy-MM-dd HH:mm:ss.fff}\r\n" +
                   $"Last Activity: {LastActivityAt:yyyy-MM-dd HH:mm:ss.fff}\r\n" +
                   $"Duration: {GetDuration().TotalSeconds:F2} seconds\r\n" +
                   $"Caller Method: {CallerMethod}\r\n" +
                   $"Thread ID: {ThreadId}\r\n" +
                   $"Operation ID: {OperationId}\r\n" +
                   $"Should Rollback: {ShouldRollback}\r\n" +
                   $"Save Points: {SavePointStack.Count}\r\n" +
                   $"Stack Trace:\r\n{StackTrace}";
        }
    }
}