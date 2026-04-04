using System;
using System.Collections.Generic;
using System.ComponentModel;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;

namespace RUINORERP.Model.ConfigModel
{
    /// <summary>
    /// 性能监控配置
    /// 用于配置客户端性能监控的开关、阈值和监控项
    /// </summary>
    [DisplayName("性能监控配置")]
    public class PerformanceMonitorConfig : BaseConfig
    {
        /// <summary>
        /// 是否启用性能监控
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// 启用的监控项列表
        /// </summary>
        public List<PerformanceMonitorType> EnabledMonitors { get; set; } = new List<PerformanceMonitorType>();

        /// <summary>
        /// 数据上报间隔（秒）
        /// </summary>
        public int UploadIntervalSeconds { get; set; } = 30;

        /// <summary>
        /// 内存数据保留时间（分钟）
        /// </summary>
        public int MemoryRetentionMinutes { get; set; } = 60;

        /// <summary>
        /// 方法执行时间阈值（毫秒）- 超过此值记录警告
        /// </summary>
        public int MethodExecutionThresholdMs { get; set; } = 1000;

        /// <summary>
        /// 数据库查询时间阈值（毫秒）- 超过此值记录警告
        /// </summary>
        public int DatabaseQueryThresholdMs { get; set; } = 5000;

        /// <summary>
        /// 网络请求时间阈值（毫秒）- 超过此值记录警告
        /// </summary>
        public int NetworkRequestThresholdMs { get; set; } = 3000;

        /// <summary>
        /// 内存警告阈值（MB）
        /// </summary>
        public int MemoryWarningThresholdMB { get; set; } = 1024;

        /// <summary>
        /// 内存临界阈值（MB）
        /// </summary>
        public int MemoryCriticalThresholdMB { get; set; } = 2048;

        /// <summary>
        /// 初始化默认值
        /// </summary>
        public override void InitDefault()
        {
            IsEnabled = false;
            EnabledMonitors = new List<PerformanceMonitorType>
            {
                PerformanceMonitorType.Database,
                PerformanceMonitorType.Network,
                PerformanceMonitorType.Memory
            };
            UploadIntervalSeconds = 30;
            MemoryRetentionMinutes = 60;
            MethodExecutionThresholdMs = 1000;
            DatabaseQueryThresholdMs = 5000;
            NetworkRequestThresholdMs = 3000;
            MemoryWarningThresholdMB = 1024;
            MemoryCriticalThresholdMB = 2048;
        }

        /// <summary>
        /// 验证配置
        /// </summary>
        public override ConfigValidationResult Validate()
        {
            var result = new ConfigValidationResult { IsValid = true };

            if (UploadIntervalSeconds < 5 || UploadIntervalSeconds > 300)
            {
                result.AddError(nameof(UploadIntervalSeconds), "数据上报间隔应在5-300秒之间");
            }

            if (MemoryRetentionMinutes < 1 || MemoryRetentionMinutes > 1440)
            {
                result.AddError(nameof(MemoryRetentionMinutes), "内存数据保留时间应在1-1440分钟之间");
            }

            if (MethodExecutionThresholdMs < 100)
            {
                result.AddError(nameof(MethodExecutionThresholdMs), "方法执行时间阈值应大于100毫秒");
            }

            if (DatabaseQueryThresholdMs < 100)
            {
                result.AddError(nameof(DatabaseQueryThresholdMs), "数据库查询时间阈值应大于100毫秒");
            }

            if (MemoryWarningThresholdMB < 100)
            {
                result.AddError(nameof(MemoryWarningThresholdMB), "内存警告阈值应大于100MB");
            }

            if (MemoryCriticalThresholdMB <= MemoryWarningThresholdMB)
            {
                result.AddError(nameof(MemoryCriticalThresholdMB), "内存临界阈值应大于内存警告阈值");
            }

            return result;
        }
    }
}
