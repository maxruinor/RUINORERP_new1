using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace RUINORERP.TopServer.ConfigurationManagement
{
    /// <summary>
    /// 应用配置类
    /// 封装所有配置项，提供强类型访问
    /// </summary>
    public class AppConfiguration
    {
        /// <summary>
        /// 服务器选项配置
        /// </summary>
        public ServerOptions ServerOptions { get; set; } = new();

        /// <summary>
        /// 心跳配置
        /// </summary>
        public HeartbeatConfiguration HeartbeatConfiguration { get; set; } = new();

        /// <summary>
        /// 数据库配置
        /// </summary>
        public DatabaseConfiguration DatabaseConfiguration { get; set; } = new();

        /// <summary>
        /// Redis配置
        /// </summary>
        public RedisConfiguration RedisConfiguration { get; set; } = new();

        /// <summary>
        /// Token服务配置
        /// </summary>
        public TokenServiceConfiguration TokenService { get; set; } = new();

        /// <summary>
        /// 授权配置
        /// </summary>
        public AuthorizationConfiguration AuthorizationConfiguration { get; set; } = new();

        /// <summary>
        /// 用户监控配置
        /// </summary>
        public UserMonitoringConfiguration UserMonitoring { get; set; } = new();

        /// <summary>
        /// 日志配置
        /// </summary>
        public LoggingConfiguration Logging { get; set; } = new();

        /// <summary>
        /// 安全配置
        /// </summary>
        public SecurityConfiguration SecurityConfiguration { get; set; } = new();

        /// <summary>
        /// 性能配置
        /// </summary>
        public PerformanceConfiguration PerformanceConfiguration { get; set; } = new();
    }

    /// <summary>
    /// 服务器选项配置
    /// </summary>
    public class ServerOptions
    {
        /// <summary>
        /// 服务器名称
        /// </summary>
        public string Name { get; set; } = "RUINORERP.ManagementServer";

        /// <summary>
        /// 监听IP地址
        /// </summary>
        public string Ip { get; set; } = "0.0.0.0";

        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; set; } = 8090;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxConnectionCount { get; set; } = 500;

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 8192;

        /// <summary>
        /// 发送缓冲区大小
        /// </summary>
        public int SendBufferSize { get; set; } = 8192;

        /// <summary>
        /// 接收超时(毫秒)
        /// </summary>
        public int ReceiveTimeout { get; set; } = 30000;

        /// <summary>
        /// 发送超时(毫秒)
        /// </summary>
        public int SendTimeout { get; set; } = 30000;

        /// <summary>
        /// 是否启用Keep-Alive
        /// </summary>
        public bool KeepAliveEnabled { get; set; } = true;

        /// <summary>
        /// Keep-Alive时间(秒)
        /// </summary>
        public int KeepAliveTime { get; set; } = 7200;

        /// <summary>
        /// Keep-Alive间隔(秒)
        /// </summary>
        public int KeepAliveInterval { get; set; } = 75;
    }

    /// <summary>
    /// 心跳配置
    /// </summary>
    public class HeartbeatConfiguration
    {
        /// <summary>
        /// 心跳间隔(毫秒)
        /// </summary>
        public int HeartbeatInterval { get; set; } = 30000;

        /// <summary>
        /// 心跳超时时间(毫秒)
        /// </summary>
        public int HeartbeatTimeout { get; set; } = 90000;

        /// <summary>
        /// 检查间隔(毫秒)
        /// </summary>
        public int CheckInterval { get; set; } = 10000;

        /// <summary>
        /// 最大允许丢失的心跳次数
        /// </summary>
        public int MaxMissedHeartbeats { get; set; } = 3;
    }

    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "Server=localhost;Database=RUINORERP_ManagementServer;User Id=sa;Password=your_password;TrustServerCertificate=True;";

        /// <summary>
        /// 是否启用调试模式
        /// </summary>
        public bool UseDebug { get; set; } = true;

        /// <summary>
        /// 是否启用调用方法跟踪
        /// </summary>
        public bool EnableCallerMethod { get; set; } = false;

        /// <summary>
        /// 命令超时(秒)
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// 连接超时(秒)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 30;
    }

    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisConfiguration
    {
        /// <summary>
        /// Redis服务器地址
        /// </summary>
        public string Server { get; set; } = "localhost:6379";

        /// <summary>
        /// Redis密码
        /// </summary>
        public string Password { get; set; } = "";

        /// <summary>
        /// 默认数据库
        /// </summary>
        public int DefaultDatabase { get; set; } = 0;

        /// <summary>
        /// 连接超时(毫秒)
        /// </summary>
        public int ConnectTimeout { get; set; } = 5000;

        /// <summary>
        /// 同步超时(毫秒)
        /// </summary>
        public int SyncTimeout { get; set; } = 5000;

        /// <summary>
        /// 是否允许管理操作
        /// </summary>
        public bool AllowAdmin { get; set; } = true;
    }

    /// <summary>
    /// Token服务配置
    /// </summary>
    public class TokenServiceConfiguration
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecretKey { get; set; } = "topserver-secret-key-at-least-32-chars-length-min";

        /// <summary>
        /// 默认过期时间(小时)
        /// </summary>
        public int DefaultExpiryHours { get; set; } = 24;

        /// <summary>
        /// 刷新令牌过期时间(天)
        /// </summary>
        public int RefreshTokenExpiryDays { get; set; } = 7;
    }

    /// <summary>
    /// 授权配置
    /// </summary>
    public class AuthorizationConfiguration
    {
        /// <summary>
        /// 检查间隔(毫秒)
        /// </summary>
        public int CheckInterval { get; set; } = 86400000;

        /// <summary>
        /// 警告阈值(天)
        /// </summary>
        public int WarningThresholdDays { get; set; } = 7;

        /// <summary>
        /// 严重阈值(天)
        /// </summary>
        public int CriticalThresholdDays { get; set; } = 3;

        /// <summary>
        /// 是否启用自动提醒
        /// </summary>
        public bool EnableAutoReminder { get; set; } = true;
    }

    /// <summary>
    /// 用户监控配置
    /// </summary>
    public class UserMonitoringConfiguration
    {
        /// <summary>
        /// 是否启用监控
        /// </summary>
        public bool EnableMonitoring { get; set; } = true;

        /// <summary>
        /// 更新间隔(毫秒)
        /// </summary>
        public int UpdateInterval { get; set; } = 5000;

        /// <summary>
        /// 最大活动记录数
        /// </summary>
        public int MaxActivityRecords { get; set; } = 100;

        /// <summary>
        /// 会话超时时间(分钟)
        /// </summary>
        public int SessionTimeoutMinutes { get; set; } = 30;
    }

    /// <summary>
    /// 日志配置
    /// </summary>
    public class LoggingConfiguration
    {
        /// <summary>
        /// 日志级别配置
        /// </summary>
        public Dictionary<string, string> LogLevel { get; set; } = new();

        /// <summary>
        /// 控制台日志配置
        /// </summary>
        public ConsoleLoggingConfiguration Console { get; set; } = new();

        /// <summary>
        /// 文件日志配置
        /// </summary>
        public FileLoggingConfiguration File { get; set; } = new();
    }

    /// <summary>
    /// 控制台日志配置
    /// </summary>
    public class ConsoleLoggingConfiguration
    {
        /// <summary>
        /// 是否包含作用域
        /// </summary>
        public bool IncludeScopes { get; set; } = true;
    }

    /// <summary>
    /// 文件日志配置
    /// </summary>
    public class FileLoggingConfiguration
    {
        /// <summary>
        /// 是否启用文件日志
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// 日志文件路径
        /// </summary>
        public string Path { get; set; } = "Logs";

        /// <summary>
        /// 最大文件大小(MB)
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 10;

        /// <summary>
        /// 最大文件数量
        /// </summary>
        public int MaxFiles { get; set; } = 30;
    }

    /// <summary>
    /// 安全配置
    /// </summary>
    public class SecurityConfiguration
    {
        /// <summary>
        /// 是否启用加密
        /// </summary>
        public bool EnableEncryption { get; set; } = true;

        /// <summary>
        /// 是否启用身份验证
        /// </summary>
        public bool EnableAuthentication { get; set; } = true;

        /// <summary>
        /// 最大登录尝试次数
        /// </summary>
        public int MaxLoginAttempts { get; set; } = 5;

        /// <summary>
        /// 锁定时长(分钟)
        /// </summary>
        public int LockoutDurationMinutes { get; set; } = 15;
    }

    /// <summary>
    /// 性能配置
    /// </summary>
    public class PerformanceConfiguration
    {
        /// <summary>
        /// 是否启用性能监控
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = true;

        /// <summary>
        /// 性能更新间隔(毫秒)
        /// </summary>
        public int PerformanceUpdateInterval { get; set; } = 10000;

        /// <summary>
        /// 统计数据保留天数
        /// </summary>
        public int StatisticsRetentionDays { get; set; } = 30;
    }

    /// <summary>
    /// 配置加载器
    /// </summary>
    public static class ConfigurationLoader
    {
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="environment">环境名称</param>
        /// <returns>应用配置</returns>
        public static AppConfiguration Load(string environment = "Development")
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            // 添加环境特定的配置文件
            if (!string.IsNullOrEmpty(environment))
            {
                builder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
            }

            var configuration = builder.Build();
            var appConfig = configuration.Get<AppConfiguration>();

            return appConfig ?? new AppConfiguration();
        }

        /// <summary>
        /// 获取当前环境名称
        /// </summary>
        /// <returns>环境名称</returns>
        public static string GetCurrentEnvironment()
        {
            // 从环境变量读取
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            if (!string.IsNullOrEmpty(environment))
            {
                return environment;
            }

            // 默认返回 Development
            return "Development";
        }
    }
}
