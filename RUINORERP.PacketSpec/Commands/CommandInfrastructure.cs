using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令调度器接口
    /// </summary>
    public interface ICommandDispatcher
    {
        Task<CommandResult> DispatchAsync(IServerCommand command, CancellationToken cancellationToken);
        Task<CommandResult> DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : IServerCommand;
        void RegisterCommandType(Type commandType);
        void UnregisterCommandType(Type commandType);
        Type[] GetRegisteredCommandTypes();
    }

    /// <summary>
    /// 命令队列接口
    /// </summary>
    public interface ICommandQueue
    {
        int Size { get; }
        int PendingCount { get; }
        int MaxQueueSize { get; set; }
        
        Task<bool> EnqueueAsync(IServerCommand command, CancellationToken cancellationToken);
        Task<IServerCommand> DequeueAsync(CancellationToken cancellationToken);
        void Clear();
        bool Contains(IServerCommand command);
        CommandQueueStatus GetStatus();
    }

    /// <summary>
    /// 命令队列状态
    /// </summary>
    public class CommandQueueStatus
    {
        public int Size { get; set; }
        public int PendingCount { get; set; }
        public int MaxQueueSize { get; set; }
        public DateTime LastActivity { get; set; }
        public long TotalProcessed { get; set; }
        public long TotalErrors { get; set; }

        public double GetErrorRate()
        {
            return TotalProcessed > 0 ? (double)TotalErrors / TotalProcessed : 0;
        }

        public bool IsOverloaded()
        {
            return PendingCount > MaxQueueSize * 0.8;
        }
    }

    /// <summary>
    /// 命令调度器接口
    /// </summary>
    public interface ICommandScheduler
    {
        Task<ScheduleResult> ScheduleAsync(IServerCommand command, DateTime executeAt, CancellationToken cancellationToken);
        Task<ScheduleResult> ScheduleAsync(IServerCommand command, TimeSpan delay, CancellationToken cancellationToken);
        Task<bool> CancelScheduleAsync(Guid scheduleId, CancellationToken cancellationToken);
        Task<ScheduleStatus> GetScheduleStatusAsync(Guid scheduleId, CancellationToken cancellationToken);
        Task<ScheduleInfo[]> GetAllSchedulesAsync(CancellationToken cancellationToken);
        Task ClearAllSchedulesAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// 调度结果
    /// </summary>
    public class ScheduleResult
    {
        public bool Success { get; set; }
        public Guid ScheduleId { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 调度状态
    /// </summary>
    public class ScheduleStatus
    {
        public Guid ScheduleId { get; set; }
        public ScheduleState State { get; set; }
        public DateTime ScheduledTime { get; set; }
        public DateTime? ActualExecuteTime { get; set; }
        public string StatusMessage { get; set; }
    }

    /// <summary>
    /// 调度信息
    /// </summary>
    public class ScheduleInfo
    {
        public Guid ScheduleId { get; set; }
        public Type CommandType { get; set; }
        public DateTime ScheduledTime { get; set; }
        public ScheduleState State { get; set; }
    }

    /// <summary>
    /// 调度状态枚举
    /// </summary>
    public enum ScheduleState
    {
        Pending = 0,
        Executing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4
    }
}