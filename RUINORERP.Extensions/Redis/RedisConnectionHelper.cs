using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace RUINORERP.Extensions.Redis
{
    /// <summary>
    /// Redis连接帮助类
    /// 提供Redis连接的管理、创建和缓存功能
    /// </summary>
    public class RedisConnectionHelper : IDisposable
    {
        // 日志记录器（可通过依赖注入获取）
        private static ILogger _logger;
        public static ILogger Logger { get => _logger; set => _logger = value; }

        // 连接字符串，使用volatile确保多线程可见性
        private static volatile string _connectionString;
        public static string ConnectionString 
        { 
            get => _connectionString; 
            set => _connectionString = value; 
        }

        /// <summary>
        /// Redis连接实例，使用volatile确保多线程可见性
        /// </summary>
        private static volatile ConnectionMultiplexer _connection;
        private static readonly object _locker = new object();
        private static readonly object _cacheLocker = new object();

        /// <summary>
        /// 缓存连接实例的字典，支持多连接字符串
        /// </summary>
        private static readonly Dictionary<string, ConnectionMultiplexer> _connectionCache =
            new Dictionary<string, ConnectionMultiplexer>();
        
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 1000;

        /// <summary>
        /// 获取Redis连接实例（单例模式）
        /// 实现了双重检查锁定模式，确保线程安全
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {
                // 第一次检查，避免不必要的锁定
                if (_connection == null || !_connection.IsConnected)
                {
                    // 加锁确保线程安全
                    lock (_locker)
                    {
                        // 第二次检查，避免在等待锁的过程中已经被其他线程初始化
                        if (_connection == null || !_connection.IsConnected)
                        {
                            try
                            {
                                // 关闭现有连接（如果存在）
                                _connection?.Dispose();
                                
                                // 创建新连接
                                _connection = CreateConnection(ConnectionString);
                                
                                LogInfo("Redis连接已创建或重新连接");
                            }
                            catch (Exception ex)
                            {
                                LogError($"创建Redis连接失败: {ex.Message}", ex);
                                throw;
                            }
                        }
                    }
                }
                return _connection;
            }
        }

        /// <summary>
        /// 创建Redis连接
        /// 支持重试机制和详细配置
        /// </summary>
        /// <param name="connectionString">Redis连接字符串</param>
        /// <param name="retryCount">重试次数</param>
        /// <returns>Redis连接实例</returns>
        /// <exception cref="ArgumentException">当连接字符串为空时抛出</exception>
        public static ConnectionMultiplexer CreateConnection(string connectionString = null, int retryCount = 0)
        {
            // 验证连接字符串
            string connectStr = connectionString ?? _connectionString;
            if (string.IsNullOrEmpty(connectStr))
            {
                throw new ArgumentException("Redis连接字符串不能为空", nameof(connectionString));
            }

            try
            {
                // 使用ConfigurationOptions明确配置
                var config = ConfigurationOptions.Parse(connectStr);

                // 关键配置项
                config.Proxy = Proxy.None;              // 禁用代理模式
                config.AsyncTimeout = 5000;             // 设置异步操作超时（毫秒）
                config.SyncTimeout = 5000;              // 设置同步操作超时（毫秒）
                config.ConnectRetry = 3;                // 连接重试次数
                config.ConnectTimeout = 5000;           // 连接超时时间（毫秒）
                config.ReconnectRetryPolicy = new ExponentialRetry(1000); // 指数退避重试策略
                config.AbortOnConnectFail = false;      // 连接失败时不中止，允许后续自动重连
                config.PreserveAsyncOrder = false;      // 提高性能，不保证异步操作顺序

                // 创建连接
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

                // 注册事件处理程序
                RegisterConnectionEvents(redis);
                
                LogInfo($"成功连接到Redis服务器: {connectStr}");
                return redis;
            }
            catch (SocketException ex) when (retryCount < MaxRetryAttempts)
            {
                // 网络相关异常，进行重试
                retryCount++;
                LogWarning($"Redis连接失败（{retryCount}/{MaxRetryAttempts}）: {ex.Message}，将在{RetryDelayMs}ms后重试");
                Thread.Sleep(RetryDelayMs);
                return CreateConnection(connectionString, retryCount);
            }
            catch (RedisConnectionException ex) when (retryCount < MaxRetryAttempts)
            {
                // Redis连接异常，进行重试
                retryCount++;
                LogWarning($"Redis连接异常（{retryCount}/{MaxRetryAttempts}）: {ex.Message}，将在{RetryDelayMs}ms后重试");
                Thread.Sleep(RetryDelayMs);
                return CreateConnection(connectionString, retryCount);
            }
            catch (Exception ex)
            {
                LogError($"创建Redis连接失败: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// 获取指定连接字符串的Redis连接实例
        /// </summary>
        /// <param name="connectionString">Redis连接字符串</param>
        /// <returns>Redis连接实例</returns>
        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("连接字符串不能为空", nameof(connectionString));
            }

            // 加锁确保线程安全
            lock (_cacheLocker)
            {
                // 如果连接不存在或已断开，则创建新连接
                if (!_connectionCache.ContainsKey(connectionString) || 
                    !_connectionCache[connectionString].IsConnected)
                {
                    try
                    {
                        // 关闭现有连接（如果存在）
                        if (_connectionCache.ContainsKey(connectionString))
                        {
                            _connectionCache[connectionString].Dispose();
                            _connectionCache.Remove(connectionString);
                        }

                        // 创建新连接并缓存
                        _connectionCache[connectionString] = CreateConnection(connectionString);
                        LogInfo($"为连接字符串 '{connectionString}' 创建新的Redis连接");
                    }
                    catch (Exception ex)
                    {
                        LogError($"获取Redis连接失败: {ex.Message}", ex);
                        throw;
                    }
                }
                return _connectionCache[connectionString];
            }
        }

        /// <summary>
        /// 注册Redis连接事件处理程序
        /// </summary>
        /// <param name="connection">Redis连接实例</param>
        private static void RegisterConnectionEvents(ConnectionMultiplexer connection)
        {
            // 连接失败事件
            connection.ConnectionFailed += (sender, e) =>
            {
                LogError($"Redis连接失败: {e.Exception.Message}, 失败类型: {e.FailureType}, 端点: {e.EndPoint}", e.Exception);
            };

            // 连接恢复事件
            connection.ConnectionRestored += (sender, e) =>
            {
                LogInfo($"Redis连接已恢复: 端点: {e.EndPoint}");
            };

            // 配置更改事件
            connection.ConfigurationChanged += (sender, e) =>
            {
                LogInfo($"Redis配置已更改: {e.EndPoint}");
            };

            // 错误事件
            connection.ErrorMessage += (sender, e) =>
            {
                LogWarning($"Redis错误消息: {e.Message}");
            };

            // 注：StackExchange.Redis 2.8.24版本不支持ServerRemoved和ServerAdded事件
            // 连接状态可以通过ConnectionFailed和ConnectionRestored事件监控
        }

        /// <summary>
        /// 释放所有Redis连接资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                // 释放单例连接
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    LogInfo("已释放Redis单例连接");
                }

                // 释放缓存的连接
                lock (_cacheLocker)
                {
                    foreach (var connection in _connectionCache.Values)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                    _connectionCache.Clear();
                    LogInfo("已释放所有缓存的Redis连接");
                }
            }
            catch (Exception ex)
            {
                LogError($"释放Redis连接资源失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 静态释放方法，用于应用程序关闭时清理资源
        /// </summary>
        public static void Shutdown()
        {
            new RedisConnectionHelper().Dispose();
        }

        #region 日志辅助方法
        
        private static void LogInfo(string message)
        {
            if (_logger != null)
                _logger.LogInformation(message);
            else
                Console.WriteLine($"[INFO] {message}");
        }

        private static void LogWarning(string message)
        {
            if (_logger != null)
                _logger.LogWarning(message);
            else
                Console.WriteLine($"[WARNING] {message}");
        }

        private static void LogError(string message, Exception ex = null)
        {
            if (_logger != null)
            {
                if (ex != null)
                    _logger.LogError(ex, message);
                else
                    _logger.LogError(message);
            }
            else
            {
                Console.WriteLine($"[ERROR] {message}");
                if (ex != null)
                    Console.WriteLine($"[EXCEPTION] {ex.Message}\n{ex.StackTrace}");
            }
        }
        
        #endregion
    }
}
