using System;

namespace RUINORERP.UI.Network.TimeoutStatistics
{
    /// <summary>
    /// 超时趋势数据类
    /// 记录特定时间段内的超时和请求统计信息
    /// 用于分析超时趋势和系统性能变化
    /// </summary>
    public class TimeoutTrendData
    {
        /// <summary>
        /// 记录时间点
        /// </summary>
        public DateTime Timestamp { get; }
        
        /// <summary>
        /// 命令类型ID
        /// </summary>
        public string CommandId { get; }
        
        /// <summary>
        /// 是否为超时记录
        /// </summary>
        public bool IsTimeout { get; }
        
        /// <summary>
        /// 处理时间（毫秒）
        /// 如果是超时记录，则为超时时间；如果是成功记录，则为实际处理时间
        /// </summary>
        public long ProcessingTimeMs { get; }
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="timestamp">记录时间点</param>
        /// <param name="commandId">命令类型ID</param>
        /// <param name="isTimeout">是否为超时记录</param>
        /// <param name="processingTimeMs">处理时间（毫秒）</param>
        public TimeoutTrendData(DateTime timestamp, string commandId, bool isTimeout, long processingTimeMs)
        {
            Timestamp = timestamp;
            CommandId = commandId;
            IsTimeout = isTimeout;
            ProcessingTimeMs = processingTimeMs;
        }
    }
}