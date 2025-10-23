using System;
using System.Collections.Generic;
using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models.Core;
namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 心跳请求数据模型
    /// </summary>
    [Serializable]
    
    public class HeartbeatRequest : RequestBase
    {
        /// <summary>
        /// 客户端会话令牌
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 客户端进程运行时间（秒）
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 客户端时间戳
        /// </summary>
        public DateTime ClientTime { get; set; }

        /// <summary>
        /// 客户端状态信息
        /// </summary>
        public string ClientStatus { get; set; }

        /// <summary>
        /// 网络延迟（毫秒）
        /// </summary>
        public int NetworkLatency { get; set; }

        /// <summary>
        /// 客户端资源使用情况
        /// </summary>
        public ClientResourceUsage ResourceUsage { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public string ClientIp { get; set; }


        /// <summary>
        /// 创建心跳请求
        /// </summary>
        public static HeartbeatRequest Create(string sessionToken, long userId, 
            string clientStatus = "Normal", string clientId = null)
        {
            return new HeartbeatRequest
            {
                SessionToken = sessionToken,
                UserId = userId,
                ClientTime = DateTime.Now,
                ClientStatus = clientStatus,
                ClientId = clientId
            };
        }

        /// <summary>
        /// 验证请求有效性
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(SessionToken) &&
                   UserId > 0 &&
                   ClientTime <= DateTime.Now.AddMinutes(5) &&
                   ClientTime >= DateTime.Now.AddMinutes(-5);
        }

        /// <summary>
        /// 安全清理敏感信息
        /// </summary>
        public void ClearSensitiveData()
        {
            SessionToken = null;
        }
    }

    /// <summary>
    /// 客户端资源使用情况
    /// </summary>
    [Serializable]
    
    public class ClientResourceUsage
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
        /// 网络带宽使用（KB/s）
        /// </summary>
        public float NetworkUsage { get; set; }

        /// <summary>
        /// 磁盘可用空间（GB）
        /// </summary>
        public float DiskFreeSpace { get; set; }

        /// <summary>
        /// 客户端进程运行时间（秒）
        /// </summary>
        public long ProcessUptime { get; set; }




        /// <summary>
        /// 创建资源使用信息
        /// </summary>
        public static ClientResourceUsage Create(float cpuUsage = 0, long memoryUsage = 0, 
            float networkUsage = 0, float diskFreeSpace = 0, long processUptime = 0)
        {
            return new ClientResourceUsage
            {
                CpuUsage = cpuUsage,
                MemoryUsage = memoryUsage,
                NetworkUsage = networkUsage,
                DiskFreeSpace = diskFreeSpace,
                ProcessUptime = processUptime
            };
        }
    }
   
}
