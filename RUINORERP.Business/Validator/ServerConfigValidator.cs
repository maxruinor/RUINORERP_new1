using System;
using System.IO;
using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RUINORERP.Model.ConfigModel;

namespace RUINORERP.Business
{
    /// <summary>
    /// ServerConfig配置验证器
    /// 用于验证服务器配置的合法性
    /// </summary>
    public class ServerConfigValidator : BaseValidatorGeneric<ServerConfig>
    {
        // 配置全局参数
        public readonly IOptionsMonitor<GlobalValidatorConfig> ValidatorConfig;

        public ServerConfigValidator(IOptionsMonitor<GlobalValidatorConfig> config = null)
        {
            ValidatorConfig = config;

            // 服务器基础配置验证
            RuleFor(x => x.ServerName).NotEmpty().WithMessage("服务器名称不能为空");
            RuleFor(x => x.ServerName).MaximumLength(50).WithMessage("服务器名称长度不能超过50个字符");

            RuleFor(x => x.ServerPort).InclusiveBetween(1024, 65535).WithMessage("服务器端口必须在1024-65535之间");

            RuleFor(x => x.MaxConnections).GreaterThan(0).WithMessage("最大连接数必须大于0");
            RuleFor(x => x.MaxConnections).LessThanOrEqualTo(10000).WithMessage("最大连接数不能超过10000");

            RuleFor(x => x.HeartbeatInterval).GreaterThanOrEqualTo(1000).WithMessage("心跳间隔不能小于1000毫秒");
            RuleFor(x => x.HeartbeatInterval).LessThanOrEqualTo(3600000).WithMessage("心跳间隔不能超过1小时(3600000毫秒)");

            // 数据库配置验证
            //RuleFor(x => x.DbConnectionString).NotEmpty().WithMessage("数据库连接字符串不能为空");
            //RuleFor(x => x.DbType).NotEmpty().WithMessage("数据库类型不能为空");
            //RuleFor(x => x.DbType).Must(BeValidDbType).WithMessage("不支持的数据库类型");

            // 缓存配置验证
            RuleFor(x => x.CacheType).Must(BeValidCacheType).WithMessage("不支持的缓存类型");
            RuleFor(x => x.CacheConnectionString).NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.CacheType) && x.CacheType != "Memory")
                .WithMessage("Redis缓存连接字符串不能为空");

            // 日志配置验证
            RuleFor(x => x.LogLevel).Must(BeValidLogLevel).WithMessage("不支持的日志级别");

            // 文件存储配置验证
            RuleFor(x => x.FileStoragePath).NotEmpty().WithMessage("文件存储路径不能为空");
            RuleFor(x => x.FileStoragePath).Must(BeValidPathFormat).WithMessage("无效的文件存储路径格式");
            RuleFor(x => x.FileStoragePath).Must(BeValidPathWithEnvironmentVariable)
                .When(x => x.FileStoragePath.Contains("%"))
                .WithMessage("文件存储路径中的环境变量格式无效");

            RuleFor(x => x.MaxFileSizeMB).GreaterThan(0).WithMessage("单个文件最大上传大小必须大于0MB");
            RuleFor(x => x.MaxFileSizeMB).LessThanOrEqualTo(1024).WithMessage("单个文件最大上传大小不能超过1024MB");

            //// 文件分类路径验证
            //RuleFor(x => x.PaymentVoucherPath).NotEmpty().WithMessage("付款凭证文件存储路径不能为空");
            //RuleFor(x => x.PaymentVoucherPath).Must(BeValidRelativePath).WithMessage("付款凭证路径必须是有效的相对路径");
            //RuleFor(x => x.PaymentVoucherPath).Must(NotContainInvalidPathChars).WithMessage("付款凭证路径包含无效字符");

            //RuleFor(x => x.ProductImagePath).NotEmpty().WithMessage("产品图片文件存储路径不能为空");
            //RuleFor(x => x.ProductImagePath).Must(BeValidRelativePath).WithMessage("产品图片路径必须是有效的相对路径");
            //RuleFor(x => x.ProductImagePath).Must(NotContainInvalidPathChars).WithMessage("产品图片路径包含无效字符");

            //RuleFor(x => x.BOMManualPath).NotEmpty().WithMessage("BOM配方手册文件存储路径不能为空");
            //RuleFor(x => x.BOMManualPath).Must(BeValidRelativePath).WithMessage("BOM配方手册路径必须是有效的相对路径");
            //RuleFor(x => x.BOMManualPath).Must(NotContainInvalidPathChars).WithMessage("BOM配方手册路径包含无效字符");

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
            if (string.IsNullOrEmpty(cacheType))
                return true;

            var validTypes = new[] { "Memory", "Redis" };
            return Array.Exists(validTypes, t => t.Equals(cacheType, StringComparison.OrdinalIgnoreCase));
        }

        // 日志级别验证
        private bool BeValidLogLevel(string logLevel)
        {
            var validLevels = new[] { "Debug", "Info", "Warning", "Error", "Critical" };
            return Array.Exists(validLevels, l => l.Equals(logLevel, StringComparison.OrdinalIgnoreCase));
        }

        // 路径格式验证
        private bool BeValidPathFormat(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // 检查路径是否包含无效字符
            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                return false;

            // 对于绝对路径的验证
            if (Path.IsPathRooted(path))
            {
                return true;
            }

            // 对于包含环境变量的路径，至少应该有正确的格式
            if (path.Contains("%"))
            {
                return true; // 环境变量的具体验证在BeValidPathWithEnvironmentVariable中处理
            }

            // 相对路径在文件分类设置中已经单独验证
            return false;
        }

        // 环境变量格式验证
        private bool BeValidPathWithEnvironmentVariable(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // 检查环境变量格式是否正确 %VAR_NAME%
            var envVarPattern = new Regex(@"%[a-zA-Z0-9_]+%");
            var matches = envVarPattern.Matches(path);

            return matches.Count > 0;
        }

        // 相对路径验证
        private bool BeValidRelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            // 相对路径不应包含根路径标识符
            if (Path.IsPathRooted(path))
                return false;

            // 不应包含父目录引用
            if (path.Contains(".."))
                return false;

            return true;
        }

        // 检查路径是否包含无效字符
        private bool NotContainInvalidPathChars(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return path.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}