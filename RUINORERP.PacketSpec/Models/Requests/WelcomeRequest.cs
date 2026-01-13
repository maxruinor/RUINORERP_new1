using System;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 欢迎请求 - 服务器向客户端发送的连接欢迎消息
    /// 包含服务器信息和连接验证要求
    /// </summary>
    [Serializable]
    public class WelcomeRequest : RequestBase
    {
        /// <summary>
        /// 服务器版本
        /// </summary>
        public string ServerVersion { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public DateTime ServerTime { get; set; }

        /// <summary>
        /// 连接会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 心跳间隔（秒）
        /// </summary>
        public int HeartbeatInterval { get; set; } = 30;

        /// <summary>
        /// 最大允许空闲时间（秒）
        /// </summary>
        public int MaxIdleTime { get; set; } = 300;

        /// <summary>
        /// 系统公告内容
        /// </summary>
        public string Announcement { get; set; }


        /// <summary>
        /// 创建欢迎请求(带公告信息)
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="serverVersion">服务器版本</param>
        /// <param name="welcomeMessage">欢迎消息</param>
        /// <param name="announcement">系统公告内容</param>
        /// <returns>欢迎请求</returns>
        public static WelcomeRequest CreateWithAnnouncement(
            string sessionId,
            string serverVersion = "2.0.0",
            string announcement = null)
        {
            return new WelcomeRequest
            {
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.Welcome),
                ServerVersion = serverVersion,
                ServerTime = DateTime.Now,
                SessionId = sessionId,
                HeartbeatInterval = 30,
                MaxIdleTime = 300,
                Announcement = announcement
            };
        }
    }
}
