using System;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 心跳响应数据模型
    /// </summary>
    [Serializable]
    public class HeartbeatResponse
    {
        /// <summary>
        /// 心跳是否成功处理
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>
        /// 服务器负载情况
        /// </summary>
        public ServerLoadInfo ServerLoad { get; set; }

        /// <summary>
        /// 是否需要客户端执行特定操作
        /// </summary>
        public string RequiredAction { get; set; }

        /// <summary>
        /// 下次心跳建议间隔（毫秒）
        /// </summary>
        public int NextHeartbeatInterval { get; set; }

        /// <summary>
        /// 服务器状态信息
        /// </summary>
        public string ServerStatus { get; set; }

        /// <summary>
        /// 创建成功心跳响应
        /// </summary>
        public static HeartbeatResponse CreateSuccess(int nextInterval = 30000)
        {
            return new HeartbeatResponse
            {
                Success = true,
                Message = "心跳正常",
                ServerTime = DateTime.UtcNow,
                NextHeartbeatInterval = nextInterval,
                ServerStatus = "Normal"
            };
        }

        /// <summary>
        /// 创建需要操作的心跳响应
        /// </summary>
        public static HeartbeatResponse CreateActionRequired(string action, string message = "需要执行操作")
        {
            return new HeartbeatResponse
            {
                Success = true,
                Message = message,
                ServerTime = DateTime.UtcNow,
                RequiredAction = action,
                NextHeartbeatInterval = 5000, // 缩短间隔
                ServerStatus = "ActionRequired"
            };
        }

        /// <summary>
        /// 创建失败心跳响应
        /// </summary>
        public static HeartbeatResponse CreateFailure(string message)
        {
            return new HeartbeatResponse
            {
                Success = false,
                Message = message,
                ServerTime = DateTime.UtcNow,
                NextHeartbeatInterval = 10000, // 失败时缩短间隔
                ServerStatus = "Error"
            };
        }

        /// <summary>
        /// 设置服务器负载信息
        /// </summary>
        public HeartbeatResponse WithServerLoad(ServerLoadInfo loadInfo)
        {
            ServerLoad = loadInfo;
            return this;
        }

        /// <summary>
        /// 验证响应有效性
        /// </summary>
        public bool IsValid()
        {
            return ServerTime <= DateTime.UtcNow.AddMinutes(5) && 
                   ServerTime >= DateTime.UtcNow.AddMinutes(-5) &&
                   NextHeartbeatInterval >= 1000 && 
                   NextHeartbeatInterval <= 120000; // 1秒到2分钟
        }

        /// <summary>
        /// 是否需要立即重新连接
        /// </summary>
        public bool RequiresReconnect()
        {
            return RequiredAction == "Reconnect" || 
                   (Message?.IndexOf("重新连接", StringComparison.OrdinalIgnoreCase) >= 0);
        }

        /// <summary>
        /// 是否需要客户端重启
        /// </summary>
        public bool RequiresRestart()
        {
            return RequiredAction == "Restart" || 
                   (Message?.IndexOf("重启", StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }

    /// <summary>
    /// 服务器负载信息
    /// </summary>
    [Serializable]
    public class ServerLoadInfo
    {
        /// <summary>
        /// CPU使用率（百分比）
        /// </summary>
        public float CpuUsage { get; set; }

        /// <summary>
        /// 内存使用量（MB）
        /// </summary>
        public long MemoryUsage { get; set; }

        /// <summary>
        /// 当前连接数
        /// </summary>
        public int CurrentConnections { get; set; }

        /// <summary>
        /// 最大连接数限制
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// 数据库连接池使用情况
        /// </summary>
        public int DatabaseConnections { get; set; }

        /// <summary>
        /// 请求处理速率（请求/秒）
        /// </summary>
        public float RequestRate { get; set; }

        /// <summary>
        /// 平均响应时间（毫秒）
        /// </summary>
        public float AvgResponseTime { get; set; }

        /// <summary>
        /// 服务器运行时间（秒）
        /// </summary>
        public long ServerUptime { get; set; }

        /// <summary>
        /// 创建服务器负载信息
        /// </summary>
        public static ServerLoadInfo Create(float cpuUsage = 0, long memoryUsage = 0, 
            int connections = 0, int maxConnections = 1000, int dbConnections = 0,
            float requestRate = 0, float avgResponseTime = 0, long uptime = 0)
        {
            return new ServerLoadInfo
            {
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage,
                CurrentConnections = connections,
                MaxConnections = maxConnections,
                DatabaseConnections = dbConnections,
                RequestRate = requestRate,
                AvgResponseTime = avgResponseTime,
                ServerUptime = uptime
            };
        }

        /// <summary>
        /// 计算负载等级
        /// </summary>
        public string GetLoadLevel()
        {
            if (CpuUsage > 90 || MemoryUsage > 90 || CurrentConnections > MaxConnections * 0.9)
                return "High";
            if (CpuUsage > 70 || MemoryUsage > 70 || CurrentConnections > MaxConnections * 0.7)
                return "Medium";
            return "Low";
        }

        /// <summary>
        /// 是否过载
        /// </summary>
        public bool IsOverloaded()
        {
            return CpuUsage > 95 || 
                   MemoryUsage > 95 || 
                   CurrentConnections >= MaxConnections;
        }
    }
}