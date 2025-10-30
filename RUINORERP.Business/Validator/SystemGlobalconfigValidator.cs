using System;
using System.IO;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business
{
    /// <summary>
    /// SystemGlobalconfig配置验证器
    /// 用于验证系统全局配置的合法性
    /// </summary>
    public class SystemGlobalconfigValidator : BaseValidatorGeneric<SystemGlobalConfig>
    {
        // 配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public SystemGlobalconfigValidator(IOptionsMonitor<GlobalValidatorConfig> config = null)
        {
            ValidatorConfig = config;

            // 销售模块配置验证
            // IsFromPlatform是布尔值，无需额外验证
            // OpenProdTypeForSaleCheck是布尔值，无需额外验证

            // 打印设置验证
            // DirectPrinting是布尔值，无需额外验证
            // AutoPrintAfterSave是布尔值，无需额外验证
            // UseSharedPrinter是布尔值，无需额外验证

            // 通用配置设置验证
            RuleFor(x => x.SomeSetting).MaximumLength(200).WithMessage("通用配置设置长度不能超过200个字符");

            // 服务器配置验证
            RuleFor(x => x.ServerName).MaximumLength(50).WithMessage("服务器名称长度不能超过50个字符");
            RuleFor(x => x.ServerName).NotEmpty().When(x => !string.IsNullOrEmpty(x.ServerName)).WithMessage("服务器名称不能为空");

            RuleFor(x => x.ServerPort).InclusiveBetween(1024, 65535).When(x => x.ServerPort > 0)
                .WithMessage("服务器端口必须在1024-65535之间");

            RuleFor(x => x.MaxConnections).GreaterThan(0).When(x => x.MaxConnections > 0)
                .WithMessage("最大连接数必须大于0");
            RuleFor(x => x.MaxConnections).LessThanOrEqualTo(10000).When(x => x.MaxConnections > 0)
                .WithMessage("最大连接数不能超过10000");

            RuleFor(x => x.HeartbeatInterval).GreaterThanOrEqualTo(1000).When(x => x.HeartbeatInterval > 0)
                .WithMessage("心跳间隔不能小于1000毫秒");
            RuleFor(x => x.HeartbeatInterval).LessThanOrEqualTo(3600000).When(x => x.HeartbeatInterval > 0)
                .WithMessage("心跳间隔不能超过1小时(3600000毫秒)");

            // 数据库配置验证
            RuleFor(x => x.DbConnectionString).NotEmpty().When(x => !string.IsNullOrEmpty(x.DbConnectionString))
                .WithMessage("数据库连接字符串不能为空");

            RuleFor(x => x.DbType).Must(BeValidDbType).When(x => !string.IsNullOrEmpty(x.DbType))
                .WithMessage("不支持的数据库类型");

            // 缓存配置验证
            RuleFor(x => x.CacheType).Must(BeValidCacheType).When(x => !string.IsNullOrEmpty(x.CacheType))
                .WithMessage("不支持的缓存类型");

            RuleFor(x => x.CacheConnectionString).NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.CacheType) && x.CacheType != "Memory" && !string.IsNullOrEmpty(x.CacheConnectionString))
                .WithMessage("Redis缓存连接字符串不能为空");

            // 日志配置验证 - EnableLogging是布尔值，无需额外验证
            RuleFor(x => x.LogLevel).Must(BeValidLogLevel).When(x => !string.IsNullOrEmpty(x.LogLevel))
                .WithMessage("不支持的日志级别");

            Initialize();
        }

        // 数据库类型验证
        private bool BeValidDbType(string dbType)
        {
            var validTypes = new[] { "SqlServer", "MySQL", "SQLite", "Oracle", "PostgreSQL" };
            return Array.Exists(validTypes, t => t.Equals(dbType, StringComparison.OrdinalIgnoreCase));
        }

        // 缓存类型验证
        private bool BeValidCacheType(string cacheType)
        {
            var validTypes = new[] { "Memory", "Redis" };
            return Array.Exists(validTypes, t => t.Equals(cacheType, StringComparison.OrdinalIgnoreCase));
        }

        // 日志级别验证
        private bool BeValidLogLevel(string logLevel)
        {
            var validLevels = new[] { "Debug", "Info", "Warning", "Error", "Critical" };
            return Array.Exists(validLevels, l => l.Equals(logLevel, StringComparison.OrdinalIgnoreCase));
        }
    }
}