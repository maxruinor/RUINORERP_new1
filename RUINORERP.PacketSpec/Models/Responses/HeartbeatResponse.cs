using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Core;
namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 心跳响应数据模型
    /// </summary>
    [Serializable]
    
    public class HeartbeatResponse : ResponseBase
    {
        /// <summary>
        /// 响应状态
        /// </summary>
        public string Status { get; set; } = "OK";

        /// <summary>
        /// 服务器时间戳
        /// </summary>
        public long ServerTimestamp { get; set; }

        /// <summary>
        /// 下次心跳间隔（毫秒）
        /// </summary>
        public int NextIntervalMs { get; set; } = 30000;

        /// <summary>
        /// 服务器信息
        /// </summary>
        public Dictionary<string, object> ServerInfo { get; set; } = new();

        /// <summary>
        /// 创建心跳响应
        /// </summary>
        public static HeartbeatResponse Create(bool isSuccess = true, string message = "心跳成功")
        {
            return new HeartbeatResponse
            {
                IsSuccess = isSuccess,
                Message = message,
                Status = isSuccess ? "OK" : "ERROR",
                ServerTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ServerInfo = new Dictionary<string, object>
                {
                    ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ["ServerVersion"] = "1.0.0"
                }
            };
        }

        /// <summary>
        /// 创建错误响应
        /// </summary>
        public static HeartbeatResponse CreateError(string errorMessage, string errorCode = null)
        {
            return new HeartbeatResponse
            {
                IsSuccess = false,
                Message = errorMessage,
                Status = "ERROR",
                ServerTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                ServerInfo = new Dictionary<string, object>
                {
                    ["ErrorCode"] = errorCode ?? "UNKNOWN_ERROR",
                    ["ServerTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };
        }

        /// <summary>
        /// 设置下次心跳间隔
        /// </summary>
        public HeartbeatResponse WithNextInterval(int intervalMs)
        {
            NextIntervalMs = intervalMs;
            return this;
        }

        /// <summary>
        /// 添加服务器信息
        /// </summary>
        public HeartbeatResponse WithServerInfo(string key, object value)
        {
            ServerInfo[key] = value;
            return this;
        }
    }
}
