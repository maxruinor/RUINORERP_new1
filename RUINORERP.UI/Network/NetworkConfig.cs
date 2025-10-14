/**
 * 文件: NetworkConfig.cs
 * 描述: 统一的网络配置管理类，集中管理所有网络相关配置参数
 * 作者: RUINORERP Team
 * 创建时间: 2025
 * 最后修改: 2025
 */

using System;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 网络配置统一管理中心
    /// 集中管理连接、请求、重试、心跳等所有网络相关配置参数
    /// </summary>
    public class NetworkConfig
    {
        #region 连接配置

        /// <summary>
        /// 连接超时时间（毫秒）
        /// 默认: 30000ms (30秒)
        /// </summary>
        public int ConnectTimeoutMs { get; set; } = 30000;

        /// <summary>
        /// 最大重连尝试次数
        /// 默认: 5次
        /// </summary>
        public int MaxReconnectAttempts { get; set; } = 5;

        /// <summary>
        /// 重连延迟时间
        /// 默认: 5秒
        /// </summary>
        public TimeSpan ReconnectDelay { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// 是否启用自动重连
        /// 默认: true
        /// </summary>
        public bool AutoReconnect { get; set; } = true;

        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; } = 8888;

        #endregion

        #region 请求配置

        /// <summary>
        /// 默认请求超时时间（毫秒）
        /// 默认: 30000ms (30秒)
        /// </summary>
        public int DefaultRequestTimeoutMs { get; set; } = 30000;

        /// <summary>
        /// 最大并发请求数
        /// 默认: 100
        /// </summary>
        public int MaxConcurrentRequests { get; set; } = 100;

        /// <summary>
        /// 请求清理间隔时间（分钟）
        /// 用于清理超时请求
        /// 默认: 5分钟
        /// </summary>
        public int RequestCleanupIntervalMinutes { get; set; } = 5;

        /// <summary>
        /// 请求ID长度
        /// 默认: 32位GUID
        /// </summary>
        public int RequestIdLength { get; set; } = 32;

        #endregion

        #region 重试配置

        /// <summary>
        /// 最大重试次数
        /// 默认: 3次
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// 重试基础延迟时间（毫秒）
        /// 默认: 1000ms (1秒)
        /// </summary>
        public int RetryBaseDelayMs { get; set; } = 1000;

        /// <summary>
        /// 重试延迟乘数（指数退避）
        /// 默认: 2.0
        /// </summary>
        public double RetryMultiplier { get; set; } = 2.0;

        /// <summary>
        /// 最大重试延迟时间（毫秒）
        /// 默认: 30000ms (30秒)
        /// </summary>
        public int MaxRetryDelayMs { get; set; } = 30000;

        /// <summary>
        /// 默认重试策略
        /// </summary>
        //public IRetryStrategy DefaultRetryStrategy { get; set; }

        #endregion

        #region 心跳配置

        /// <summary>
        /// 心跳间隔时间（毫秒）
        /// 默认: 30000ms (30秒)
        /// </summary>
        public int HeartbeatIntervalMs { get; set; } = 30000;

        /// <summary>
        /// 心跳超时时间（毫秒）
        /// 默认: 5000ms (5秒)
        /// </summary>
        public int HeartbeatTimeoutMs { get; set; } = 5000;

        /// <summary>
        /// 最大心跳失败次数
        /// 达到此次数后触发重连
        /// 默认: 3次
        /// </summary>
        public int MaxHeartbeatFailures { get; set; } = 3;

        /// <summary>
        /// 是否启用心跳检测
        /// 默认: true
        /// </summary>
        public bool EnableHeartbeat { get; set; } = true;

        #endregion

        #region 缓存配置

        /// <summary>
        /// 连接状态缓存时间（秒）
        /// 默认: 60秒
        /// </summary>
        public int ConnectionStatusCacheSeconds { get; set; } = 60;

        /// <summary>
        /// 统计信息缓存时间（秒）
        /// 默认: 300秒 (5分钟)
        /// </summary>
        public int StatisticsCacheSeconds { get; set; } = 300;

        /// <summary>
        /// 趋势数据最大保留点数
        /// 默认: 1000个点
        /// </summary>
        public int MaxTrendDataPoints { get; set; } = 1000;

        #endregion

        #region 安全配置

        /// <summary>
        /// 是否启用数据包加密
        /// 默认: true
        /// </summary>
        public bool EnableEncryption { get; set; } = true;

        /// <summary>
        /// 加密密钥（如果启用加密）
        /// </summary>
        public string EncryptionKey { get; set; } = "DefaultEncryptionKey1234567890";

        /// <summary>
        /// Token刷新最大重试次数
        /// 默认: 3次
        /// </summary>
        public int MaxTokenRefreshRetries { get; set; } = 3;

        /// <summary>
        /// Token刷新重试延迟（毫秒）
        /// 默认: 2000ms (2秒)
        /// </summary>
        public int TokenRefreshRetryDelayMs { get; set; } = 2000;

        #endregion

        #region 性能配置

        /// <summary>
        /// 发送缓冲区大小（字节）
        /// 默认: 8192字节 (8KB)
        /// </summary>
        public int SendBufferSize { get; set; } = 8192;

        /// <summary>
        /// 接收缓冲区大小（字节）
        /// 默认: 8192字节 (8KB)
        /// </summary>
        public int ReceiveBufferSize { get; set; } = 8192;

        /// <summary>
        /// 数据包最大大小（字节）
        /// 默认: 1048576字节 (1MB)
        /// </summary>
        public int MaxPacketSize { get; set; } = 1048576; // 1MB

        /// <summary>
        /// 序列化超时时间（毫秒）
        /// 默认: 5000ms (5秒)
        /// </summary>
        public int SerializationTimeoutMs { get; set; } = 5000;

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// 初始化默认配置值
        /// </summary>
        public NetworkConfig()
        {
        }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="serverPort">服务器端口</param>
        public NetworkConfig(string serverAddress, int serverPort) : this()
        {
            ServerAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
            ServerPort = serverPort;
        }

        #endregion

        #region 配置验证

        /// <summary>
        /// 验证配置参数的有效性
        /// </summary>
        /// <exception cref="ArgumentException">当配置参数无效时抛出</exception>
        public void Validate()
        {
            if (ConnectTimeoutMs <= 0)
                throw new ArgumentException("连接超时时间必须大于0", nameof(ConnectTimeoutMs));

            if (MaxReconnectAttempts < 0)
                throw new ArgumentException("最大重连次数不能为负数", nameof(MaxReconnectAttempts));

            if (ReconnectDelay <= TimeSpan.Zero)
                throw new ArgumentException("重连延迟必须大于0", nameof(ReconnectDelay));

            if (DefaultRequestTimeoutMs <= 0)
                throw new ArgumentException("默认请求超时时间必须大于0", nameof(DefaultRequestTimeoutMs));

            if (MaxConcurrentRequests <= 0)
                throw new ArgumentException("最大并发请求数必须大于0", nameof(MaxConcurrentRequests));

            if (MaxRetryAttempts < 0)
                throw new ArgumentException("最大重试次数不能为负数", nameof(MaxRetryAttempts));

            if (RetryBaseDelayMs <= 0)
                throw new ArgumentException("重试基础延迟必须大于0", nameof(RetryBaseDelayMs));

            if (RetryMultiplier <= 1.0)
                throw new ArgumentException("重试延迟乘数必须大于1", nameof(RetryMultiplier));

            if (MaxRetryDelayMs <= 0)
                throw new ArgumentException("最大重试延迟必须大于0", nameof(MaxRetryDelayMs));

            if (HeartbeatIntervalMs <= 0)
                throw new ArgumentException("心跳间隔必须大于0", nameof(HeartbeatIntervalMs));

            if (HeartbeatTimeoutMs <= 0)
                throw new ArgumentException("心跳超时时间必须大于0", nameof(HeartbeatTimeoutMs));

            if (MaxHeartbeatFailures <= 0)
                throw new ArgumentException("最大心跳失败次数必须大于0", nameof(MaxHeartbeatFailures));

            if (string.IsNullOrWhiteSpace(ServerAddress))
                throw new ArgumentException("服务器地址不能为空", nameof(ServerAddress));

            if (ServerPort <= 0 || ServerPort > 65535)
                throw new ArgumentException("服务器端口必须在1-65535之间", nameof(ServerPort));

            if (SendBufferSize <= 0)
                throw new ArgumentException("发送缓冲区大小必须大于0", nameof(SendBufferSize));

            if (ReceiveBufferSize <= 0)
                throw new ArgumentException("接收缓冲区大小必须大于0", nameof(ReceiveBufferSize));

            if (MaxPacketSize <= 0)
                throw new ArgumentException("数据包最大大小必须大于0", nameof(MaxPacketSize));

            if (SerializationTimeoutMs <= 0)
                throw new ArgumentException("序列化超时时间必须大于0", nameof(SerializationTimeoutMs));
        }

        #endregion

        #region 配置克隆

        /// <summary>
        /// 创建配置的深拷贝
        /// </summary>
        /// <returns>新的配置实例</returns>
        public NetworkConfig Clone()
        {
            return new NetworkConfig
            {
                // 连接配置
                ConnectTimeoutMs = this.ConnectTimeoutMs,
                MaxReconnectAttempts = this.MaxReconnectAttempts,
                ReconnectDelay = this.ReconnectDelay,
                AutoReconnect = this.AutoReconnect,
                ServerAddress = this.ServerAddress,
                ServerPort = this.ServerPort,

                // 请求配置
                DefaultRequestTimeoutMs = this.DefaultRequestTimeoutMs,
                MaxConcurrentRequests = this.MaxConcurrentRequests,
                RequestCleanupIntervalMinutes = this.RequestCleanupIntervalMinutes,
                RequestIdLength = this.RequestIdLength,

                // 重试配置
                MaxRetryAttempts = this.MaxRetryAttempts,
                RetryBaseDelayMs = this.RetryBaseDelayMs,
                RetryMultiplier = this.RetryMultiplier,
                MaxRetryDelayMs = this.MaxRetryDelayMs,
                //DefaultRetryStrategy = this.DefaultRetryStrategy,

                // 心跳配置
                HeartbeatIntervalMs = this.HeartbeatIntervalMs,
                HeartbeatTimeoutMs = this.HeartbeatTimeoutMs,
                MaxHeartbeatFailures = this.MaxHeartbeatFailures,
                EnableHeartbeat = this.EnableHeartbeat,

                // 缓存配置
                ConnectionStatusCacheSeconds = this.ConnectionStatusCacheSeconds,
                StatisticsCacheSeconds = this.StatisticsCacheSeconds,
                MaxTrendDataPoints = this.MaxTrendDataPoints,

                // 安全配置
                EnableEncryption = this.EnableEncryption,
                EncryptionKey = this.EncryptionKey,
                MaxTokenRefreshRetries = this.MaxTokenRefreshRetries,
                TokenRefreshRetryDelayMs = this.TokenRefreshRetryDelayMs,

                // 性能配置
                SendBufferSize = this.SendBufferSize,
                ReceiveBufferSize = this.ReceiveBufferSize,
                MaxPacketSize = this.MaxPacketSize,
                SerializationTimeoutMs = this.SerializationTimeoutMs
            };
        }

        #endregion

        #region 静态配置实例

        /// <summary>
        /// 默认配置实例
        /// </summary>
        public static NetworkConfig Default => new NetworkConfig();

        /// <summary>
        /// 开发环境配置
        /// 更短的超时时间和更积极的重试策略
        /// </summary>
        public static NetworkConfig Development => new NetworkConfig
        {
            ConnectTimeoutMs = 10000,      // 10秒
            DefaultRequestTimeoutMs = 15000, // 15秒
            HeartbeatIntervalMs = 10000,   // 10秒
            HeartbeatTimeoutMs = 3000,     // 3秒
            MaxRetryAttempts = 5,          // 5次重试
            RetryBaseDelayMs = 500,        // 500ms基础延迟
            ReconnectDelay = TimeSpan.FromSeconds(2) // 2秒重连延迟
        };

        /// <summary>
        /// 生产环境配置
        /// 更保守的超时时间和稳定的重试策略
        /// </summary>
        public static NetworkConfig Production => new NetworkConfig
        {
            ConnectTimeoutMs = 30000,      // 30秒
            DefaultRequestTimeoutMs = 30000, // 30秒
            HeartbeatIntervalMs = 30000,   // 30秒
            HeartbeatTimeoutMs = 5000,     // 5秒
            MaxRetryAttempts = 3,          // 3次重试
            RetryBaseDelayMs = 1000,       // 1秒基础延迟
            ReconnectDelay = TimeSpan.FromSeconds(5) // 5秒重连延迟
        };

        /// <summary>
        /// 高性能配置
        /// 适合内网或低延迟环境
        /// </summary>
        public static NetworkConfig HighPerformance => new NetworkConfig
        {
            ConnectTimeoutMs = 5000,       // 5秒
            DefaultRequestTimeoutMs = 10000, // 10秒
            HeartbeatIntervalMs = 15000,   // 15秒
            HeartbeatTimeoutMs = 2000,     // 2秒
            MaxRetryAttempts = 2,          // 2次重试
            RetryBaseDelayMs = 300,        // 300ms基础延迟
            ReconnectDelay = TimeSpan.FromSeconds(1), // 1秒重连延迟
            MaxConcurrentRequests = 200,     // 200并发
            SendBufferSize = 16384,         // 16KB发送缓冲
            ReceiveBufferSize = 16384      // 16KB接收缓冲
        };

        #endregion
    }

    /// <summary>
    /// 网络配置构建器
    /// 用于链式配置构建
    /// </summary>
    public class NetworkConfigBuilder
    {
        private readonly NetworkConfig _config;

        public NetworkConfigBuilder()
        {
            _config = new NetworkConfig();
        }

        public NetworkConfigBuilder WithServer(string address, int port)
        {
            _config.ServerAddress = address;
            _config.ServerPort = port;
            return this;
        }

        public NetworkConfigBuilder WithTimeouts(int connectTimeout, int requestTimeout)
        {
            _config.ConnectTimeoutMs = connectTimeout;
            _config.DefaultRequestTimeoutMs = requestTimeout;
            return this;
        }

        public NetworkConfigBuilder WithRetry(int maxAttempts, int baseDelayMs)
        {
            _config.MaxRetryAttempts = maxAttempts;
            _config.RetryBaseDelayMs = baseDelayMs;
            return this;
        }

        public NetworkConfigBuilder WithHeartbeat(int intervalMs, int timeoutMs)
        {
            _config.HeartbeatIntervalMs = intervalMs;
            _config.HeartbeatTimeoutMs = timeoutMs;
            return this;
        }

        public NetworkConfigBuilder WithAutoReconnect(bool enabled, int maxAttempts, TimeSpan delay)
        {
            _config.AutoReconnect = enabled;
            _config.MaxReconnectAttempts = maxAttempts;
            _config.ReconnectDelay = delay;
            return this;
        }

        public NetworkConfigBuilder WithEncryption(bool enabled, string key = null)
        {
            _config.EnableEncryption = enabled;
            if (!string.IsNullOrEmpty(key))
                _config.EncryptionKey = key;
            return this;
        }

        public NetworkConfigBuilder WithPerformance(int maxConcurrent, int bufferSize)
        {
            _config.MaxConcurrentRequests = maxConcurrent;
            _config.SendBufferSize = bufferSize;
            _config.ReceiveBufferSize = bufferSize;
            return this;
        }

        public NetworkConfig Build()
        {
            _config.Validate();
            return _config;
        }
    }
}