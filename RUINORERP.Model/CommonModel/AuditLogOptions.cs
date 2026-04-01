using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Model.CommonModel
{
    /// <summary>
    /// 审计日志配置类 - 优化版
    /// 支持队列限流、超时保护和性能监控
    /// </summary>
    public class AuditLogOptions
    {
        /// <summary>
        /// 批量写入大小, 默认50条
        /// 建议范围：20-100, 根据数据库性能调整
        /// </summary>
        public int BatchSize { get; set; } = 50;

        /// <summary>
        /// 自动刷新间隔(毫秒), 默认1000ms
        /// 建议范围：500-3000, 平衡实时性和性能
        /// </summary>
        public int FlushInterval { get; set; } = 1000;

        /// <summary>
        /// 是否启用审计日志
        /// </summary>
        public bool EnableAudit { get; set; } = true;

        /// <summary>
        /// 队列最大容量, 默认10000条
        /// 超过此限制将丢弃新日志并记录警告
        /// </summary>
        public int MaxQueueSize { get; set; } = 10000;

        /// <summary>
        /// 刷新操作超时时间(毫秒), 默认30000ms
        /// 防止数据库操作异常导致长时间阻塞
        /// </summary>
        public int FlushTimeout { get; set; } = 30000;

        /// <summary>
        /// 数据库写入失败时的最大重试次数, 默认3次
        /// 超过此限制将丢弃日志并记录错误
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// 是否启用性能监控, 默认true
        /// 监控队列大小、刷新耗时、成功/失败次数
        /// </summary>
        public bool EnableMetrics { get; set; } = true;

        /// <summary>
        /// 日志丢弃时的回调委托（可选）
        /// 可用于自定义丢弃日志的处理逻辑
        /// </summary>
        public Action<string, object> OnLogDropped { get; set; }

        /// <summary>
        /// 验证配置有效性
        /// </summary>
        /// <exception cref="ArgumentException">配置无效时抛出</exception>
        public void Validate()
        {
            if (BatchSize < 1)
                throw new ArgumentException("BatchSize必须大于0", nameof(BatchSize));

            if (BatchSize > 1000)
                throw new ArgumentException("BatchSize不应超过1000", nameof(BatchSize));

            if (FlushInterval < 100)
                throw new ArgumentException("FlushInterval不应小于100ms", nameof(FlushInterval));

            if (FlushInterval > 60000)
                throw new ArgumentException("FlushInterval不应超过60000ms", nameof(FlushInterval));

            if (MaxQueueSize < 100)
                throw new ArgumentException("MaxQueueSize不应小于100", nameof(MaxQueueSize));

            if (MaxQueueSize > 100000)
                throw new ArgumentException("MaxQueueSize不应超过100000", nameof(MaxQueueSize));

            if (FlushTimeout < 1000)
                throw new ArgumentException("FlushTimeout不应小于1000ms", nameof(FlushTimeout));

            if (MaxRetryCount < 0)
                throw new ArgumentException("MaxRetryCount不能为负数", nameof(MaxRetryCount));
        }

        /// <summary>
        /// 创建优化后的默认配置
        /// </summary>
        public static AuditLogOptions CreateOptimized()
        {
            return new AuditLogOptions
            {
                BatchSize = 50,
                FlushInterval = 1000,
                EnableAudit = true,
                MaxQueueSize = 10000,
                FlushTimeout = 30000,
                MaxRetryCount = 3,
                EnableMetrics = true
            };
        }

        /// <summary>
        /// 创建高性能配置（适用于高并发场景）
        /// </summary>
        public static AuditLogOptions CreateHighPerformance()
        {
            return new AuditLogOptions
            {
                BatchSize = 100,
                FlushInterval = 500,
                EnableAudit = true,
                MaxQueueSize = 50000,
                FlushTimeout = 15000,
                MaxRetryCount = 2,
                EnableMetrics = true
            };
        }

        /// <summary>
        /// 创建低延迟配置（适用于需要高实时性的场景）
        /// </summary>
        public static AuditLogOptions CreateLowLatency()
        {
            return new AuditLogOptions
            {
                BatchSize = 20,
                FlushInterval = 200,
                EnableAudit = true,
                MaxQueueSize = 5000,
                FlushTimeout = 5000,
                MaxRetryCount = 3,
                EnableMetrics = true
            };
        }
    }
}
