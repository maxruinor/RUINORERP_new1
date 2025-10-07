using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.PacketSpec.Commands
{
    public class CommandExecutionContext
    {
        public string SessionId { get; set; }
        public string ClientId { get; set; }
        public string RequestId { get; set; }
        public string ClientIp { get; set; }
        public string Token { get; set; } // 添加Token支持
        public DateTime ReceivedTime { get; set; }
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();

        // 添加用户认证信息
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsAuthenticated { get; set; }

        public static CommandExecutionContext CreateFromPacket(PacketModel packet)
        {
            return new CommandExecutionContext
            {
                SessionId = packet.SessionId,
                ClientId = packet.ClientId,
                RequestId = packet.Extensions["RequestId"].ToString(),
                Token = packet.Token, // 从数据包中获取Token
                ReceivedTime = packet.CreatedTimeUtc,
                Extensions = new Dictionary<string, object>(packet.Extensions)
            };
        }
    }
}
