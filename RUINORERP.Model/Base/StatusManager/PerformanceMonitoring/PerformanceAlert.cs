using System;

namespace RUINORERP.Model.Base.StatusManager.PerformanceMonitoring
{
    /// <summary>
    /// 性能告警级别
    /// </summary>
    public enum AlertLevel
    {
        /// <summary>
        /// 警告级别
        /// </summary>
        Warning,
        
        /// <summary>
        /// 严重级别
        /// </summary>
        Critical
    }

    /// <summary>
    /// 性能告警数据模型
    /// </summary>
    public class PerformanceAlert
    {
        /// <summary>
        /// 告警时间
        /// </summary>
        public DateTime AlertTime { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 告警类型
        /// </summary>
        public PerformanceMonitorType AlertType { get; set; }

        /// <summary>
        /// 告警标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 告警详情
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// 告警级别
        /// </summary>
        public AlertLevel Level { get; set; }

        /// <summary>
        /// 转换为字符串描述
        /// </summary>
        public override string ToString()
        {
            return $"[{AlertTime:HH:mm:ss}] {Level} - {Title}: {Details}";
        }
    }
}
