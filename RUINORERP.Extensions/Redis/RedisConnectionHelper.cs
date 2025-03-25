using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Extensions.Redis
{
    public class RedisConnectionHelper
    {
        private static string connectionString;
        public static string ConnectionString { get => connectionString; set => connectionString = value; }

        /// <summary>
        /// 连接redis server 类
        /// </summary>
        public static ConnectionMultiplexer _connection;
        public readonly static object _locker = new object();



        /// <summary>
        /// 缓存连接实例
        /// </summary>
        public static readonly Dictionary<string, ConnectionMultiplexer> _connectionCache =
            new Dictionary<string, ConnectionMultiplexer>();
        /// <summary>
        /// 连接实例  (单例模式)
        /// </summary>
        public static ConnectionMultiplexer Instance
        {

            get
            {

                if (_connection == null)
                {
                    lock (_locker)
                    {
                        if (_connection == null || !_connection.IsConnected)
                        {
                            _connection = CreateConnection(ConnectionString);
                            //_connection = ConnectionMultiplexer.Connect(ConfigurationOptions);
                        }
                    }
                }
                return _connection;
            }
        }


        /// <summary>
        /// https://stackexchange.github.io/StackExchange.Redis/Basics
        /// </summary>
        /// <param name="connectionString"></param>
        public static ConnectionMultiplexer CreateConnection(string connectionString = null)
        {
            ////连接字符串
             string connectStr = connectionString ?? "192.168.0.254:6379";
            //ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectStr);
            ////redis 事件注册
            //redis.ConnectionFailed += Redis_ConnectionFailed;

            // 使用 ConfigurationOptions 明确配置
            var config = ConfigurationOptions.Parse(connectStr);

            // 关键配置项
            config.Proxy = Proxy.None;              // 禁用代理模式
            //config.BeforeSocketConnect = false;     // 禁用共享 Socket 池
            config.AsyncTimeout = 5000;             // 设置异步操作超时（毫秒）
            config.SyncTimeout = 5000;              // 设置同步操作超时（毫秒）

            // 创建连接
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(config);

            // 注册事件
            redis.ConnectionFailed += Redis_ConnectionFailed;
            return redis;
        }

        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!_connectionCache.ContainsKey(connectionString))
            {
                _connectionCache[connectionString] = CreateConnection(connectionString);
            }
            return _connectionCache[connectionString];
        }
        /// <summary>
        /// redis 服务连接失败事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Redis_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine(e.Exception);
        }
    }

}
