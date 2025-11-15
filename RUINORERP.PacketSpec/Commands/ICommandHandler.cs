using RUINORERP.PacketSpec.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器接口
    /// </summary>
    public interface ICommandHandler : IDisposable
    {
        /// <summary>
        /// 处理器唯一标识
        /// </summary>
        string HandlerId { get; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 是否已初始化
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 支持的命令类型 - 使用CommandId类型提供更好的类型安全性和可读性
        /// </summary>
        IReadOnlyList<CommandId> SupportedCommands { get; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        HandlerStatus Status { get; }

        /// <summary>
        /// 异步处理命令数据包
        /// </summary>
        /// <param name="cmd">队列命令对象，包含数据包和任务完成源</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>处理结果</returns>
        Task<IResponse> HandleAsync(QueuedCommand cmd, CancellationToken cancellationToken = default);

        /// <summary>
        /// 判断是否可以处理该命令数据包
        /// </summary>
        /// <param name="cmd">队列命令对象</param>
        /// <returns>是否可以处理</returns>
        bool CanHandle(QueuedCommand cmd);


        /// <summary>
        /// 处理器初始化
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>初始化是否成功</returns>
        Task<bool> InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 处理器启动
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>启动是否成功</returns>
        Task<bool> StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 处理器停止
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>停止是否成功</returns>
        Task<bool> StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取处理器统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        HandlerStatistics GetStatistics();

        /// <summary>
        /// 重置处理器统计信息
        /// </summary>
        void ResetStatistics();
    }

    /// <summary>
    /// 处理器状态
    /// </summary>
    public enum HandlerStatus
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        Uninitialized = 0,

        /// <summary>
        /// 已初始化
        /// </summary>
        Initialized = 1,

        /// <summary>
        /// 运行中
        /// </summary>
        Running = 2,

        /// <summary>
        /// 已停止
        /// </summary>
        Stopped = 3,

        /// <summary>
        /// 错误状态
        /// </summary>
        Error = 4,

        /// <summary>
        /// 已释放
        /// </summary>
        Disposed = 5
    }

    /// <summary>
    /// 处理器统计信息
    /// </summary>
    public class HandlerStatistics
    {
        /// <summary>
        /// 处理器启动时间（UTC时间）
        /// </summary>
        public DateTime StartTimeUtc { get; set; }

        /// <summary>
        /// 总处理命令数
        /// </summary>
        public long TotalCommandsProcessed { get; set; }

        /// <summary>
        /// 成功处理命令数
        /// </summary>
        public long SuccessfulCommands { get; set; }

        /// <summary>
        /// 失败处理命令数
        /// </summary>
        public long FailedCommands { get; set; }

        /// <summary>
        /// 平均处理时间（毫秒）
        /// </summary>
        public double AverageProcessingTimeMs { get; set; }

        /// <summary>
        /// 最大处理时间（毫秒）
        /// </summary>
        public long MaxProcessingTimeMs { get; set; }

        /// <summary>
        /// 最小处理时间（毫秒）
        /// </summary>
        public long MinProcessingTimeMs { get; set; }

        /// <summary>
        /// 最后处理时间（UTC时间）
        /// </summary>
        public DateTime LastProcessTime { get; set; }

        /// <summary>
        /// 当前正在处理的命令数
        /// </summary>
        public int CurrentProcessingCount { get; set; }

        /// <summary>
        /// 超时次数（处理时间超过阈值的次数）
        /// </summary>
        public long TimeoutCount { get; set; }

        /// <summary>
        /// 最后一次错误信息
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// 最后一次错误时间（UTC时间）
        /// </summary>
        public DateTime? LastErrorTime { get; set; }

        /// <summary>
        /// 最后一次错误堆栈跟踪
        /// </summary>
        public string LastErrorStackTrace { get; set; }

        /// <summary>
        /// 运行时间
        /// </summary>
        public TimeSpan Uptime => DateTime.UtcNow - StartTimeUtc;

        /// <summary>
        /// 启动时间（本地时间，仅用于调试显示）
        /// </summary>
        [JsonIgnore]
        public DateTime StartTimeLocal => StartTimeUtc.ToLocalTime();

        /// <summary>
        /// 最后处理时间（本地时间，仅用于调试显示）
        /// </summary>
        [JsonIgnore]
        public DateTime LastProcessTimeLocal => LastProcessTime.ToLocalTime();

        /// <summary>
        /// 最后一次错误时间（本地时间，仅用于调试显示）
        /// </summary>
        [JsonIgnore]
        public DateTime? LastErrorTimeLocal => LastErrorTime?.ToLocalTime();

        /// <summary>
        /// 成功率
        /// </summary>
        public double SuccessRate => TotalCommandsProcessed > 0
            ? (double)SuccessfulCommands / TotalCommandsProcessed * 100
            : 0;

        /// <summary>
        /// 平均处理速度（命令/秒）
        /// </summary>
        public double AverageProcessingRate => Uptime.TotalSeconds > 0
            ? TotalCommandsProcessed / Uptime.TotalSeconds
            : 0;

        /// <summary>
        /// 获取详细的统计信息报告
        /// </summary>
        /// <returns>统计信息报告</returns>
        public string GetStatisticsReport()
        {
            return $"处理器统计信息:\n" +
                   $"  运行时间: {Uptime}\n" +
                   $"  总处理命令数: {TotalCommandsProcessed}\n" +
                   $"  成功处理命令数: {SuccessfulCommands}\n" +
                   $"  失败处理命令数: {FailedCommands}\n" +
                   $"  成功率: {SuccessRate:F2}%\n" +
                   $"  平均处理时间: {AverageProcessingTimeMs:F2}ms\n" +
                   $"  最大处理时间: {MaxProcessingTimeMs}ms\n" +
                   $"  最小处理时间: {MinProcessingTimeMs}ms\n" +
                   $"  超时次数: {TimeoutCount}\n" +
                   $"  当前处理中: {CurrentProcessingCount}\n" +
                   $"  平均处理速度: {AverageProcessingRate:F2} 命令/秒";
        }
    }
}
