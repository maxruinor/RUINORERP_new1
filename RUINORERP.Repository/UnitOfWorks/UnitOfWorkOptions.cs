using Microsoft.Extensions.Options;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// ✅ P8优化: UnitOfWork 配置选项
    /// 通过 IOptions<UnitOfWorkOptions> 注入,替代硬编码参数
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 最大事务嵌套深度
        /// 默认: 15
        /// </summary>
        public int MaxTransactionDepth { get; set; } = 15;

        /// <summary>
        /// 长事务警告阈值(秒)
        /// 超过此时间记录警告日志
        /// 默认: 60秒
        /// </summary>
        public int LongTransactionWarningSeconds { get; set; } = 60;

        /// <summary>
        /// 超长事务严重警告阈值(秒)
        /// 超过此时间记录错误日志并告警
        /// 默认: 300秒(5分钟)
        /// </summary>
        public int CriticalTransactionWarningSeconds { get; set; } = 300;

        /// <summary>
        /// 默认事务超时时间(秒)
        /// 默认: 60秒
        /// </summary>
        public int DefaultTransactionTimeoutSeconds { get; set; } = 60;

        /// <summary>
        /// 最大重试次数
        /// 默认: 3次
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// 最大重试延迟(毫秒)
        /// 默认: 2000ms
        /// </summary>
        public int MaxRetryDelayMs { get; set; } = 2000;

        /// <summary>
        /// 初始重试延迟(毫秒)
        /// 默认: 100ms
        /// </summary>
        public int InitialRetryDelayMs { get; set; } = 100;

        /// <summary>
        /// 是否启用事务性能监控
        /// 默认: true
        /// </summary>
        public bool EnableTransactionMetrics { get; set; } = true;

        /// <summary>
        /// 是否启用自动事务超时
        /// 默认: true (生产环境强烈建议启用)
        /// </summary>
        public bool EnableAutoTransactionTimeout { get; set; } = true;

        /// <summary>
        /// 自动超时后是否强制回滚
        /// 默认: true
        /// </summary>
        public bool ForceRollbackOnTimeout { get; set; } = true;

        /// <summary>
        /// 可重试的SQL错误码列表
        /// 默认包含: 1205(死锁), 1222(锁超时), 40197, 40501, 40613, -2
        /// </summary>
        public int[] RetryableSqlErrorCodes { get; set; } = new[] 
        { 
            1205,  // 死锁
            1222,  // 锁超时
            40197, // Azure SQL: 服务繁忙
            40501, // Azure SQL: 资源限制
            40613, // Azure SQL: 数据库不可用
            -2     // 查询超时
        };

        /// <summary>
        /// 是否记录详细的事务调试信息
        /// 默认: false (生产环境建议关闭)
        /// </summary>
        public bool EnableVerboseTransactionLogging { get; set; } = false;
    }

    /// <summary>
    /// 配置验证器
    /// </summary>
    public class UnitOfWorkOptionsValidator : IValidateOptions<UnitOfWorkOptions>
    {
        public ValidateOptionsResult Validate(string name, UnitOfWorkOptions options)
        {
            if (options.MaxTransactionDepth <= 0 || options.MaxTransactionDepth > 50)
            {
                return ValidateOptionsResult.Fail("MaxTransactionDepth 必须在 1-50 之间");
            }

            if (options.DefaultTransactionTimeoutSeconds <= 0 || options.DefaultTransactionTimeoutSeconds > 600)
            {
                return ValidateOptionsResult.Fail("DefaultTransactionTimeoutSeconds 必须在 1-600 之间");
            }

            if (options.MaxRetryCount < 0 || options.MaxRetryCount > 10)
            {
                return ValidateOptionsResult.Fail("MaxRetryCount 必须在 0-10 之间");
            }

            if (options.RetryableSqlErrorCodes == null || options.RetryableSqlErrorCodes.Length == 0)
            {
                return ValidateOptionsResult.Fail("RetryableSqlErrorCodes 不能为空");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
