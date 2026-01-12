using System;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Responses
{
    /// <summary>
    /// 欢迎响应 - 客户端对服务器欢迎消息的确认回复
    /// 服务器收到此消息后将连接标记为已验证
    /// </summary>
    [Serializable]
    public class WelcomeResponse : ResponseBase
    {
        /// <summary>
        /// 客户端版本
        /// </summary>
        public string ClientVersion { get; set; }

        /// <summary>
        /// 客户端操作系统信息
        /// </summary>
        public string ClientOS { get; set; }

        /// <summary>
        /// 客户端机器名
        /// </summary>
        public string ClientMachineName { get; set; }

        /// <summary>
        /// 客户端CPU信息
        /// </summary>
        public string ClientCPU { get; set; }

        /// <summary>
        /// 客户端内存大小（MB）
        /// </summary>
        public long ClientMemoryMB { get; set; }

        /// <summary>
        /// 创建欢迎响应
        /// </summary>
        /// <param name="clientVersion">客户端版本</param>
        /// <param name="clientOS">客户端操作系统</param>
        /// <param name="clientMachineName">客户端机器名</param>
        /// <param name="clientCPU">客户端CPU信息</param>
        /// <param name="clientMemoryMB">客户端内存大小</param>
        /// <returns>欢迎响应</returns>
        public static WelcomeResponse Create(
            string clientVersion = "",
            string clientOS = "",
            string clientMachineName = "",
            string clientCPU = "",
            long clientMemoryMB = 0)
        {
            return new WelcomeResponse
            {
                RequestId = IdGenerator.GenerateRequestId(SystemCommands.WelcomeAck),
                IsSuccess = true,
                Message = "欢迎确认",
                ClientVersion = clientVersion,
                ClientOS = clientOS,
                ClientMachineName = clientMachineName,
                ClientCPU = clientCPU,
                ClientMemoryMB = clientMemoryMB
            };
        }
    }
}
