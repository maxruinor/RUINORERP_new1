using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands
{
    [Serializable]
    [MessagePackObject]
    public class CommandExecutionContext
    {
        [Key(0)]
        public string SessionId { get; set; }
        [Key(1)]
        public string ClientId { get; set; }
        [Key(2)]
        public string RequestId { get; set; }
        [Key(3)]
        public string ClientIp { get; set; }
        [Key(4)]
        public TokenInfo Token { get; set; }
        [Key(5)]
        public DateTime ReceivedTime { get; set; }
        [Key(6)]
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();

        // 添加用户认证信息
        [Key(7)]
        public string UserId { get; set; }
        [Key(8)]
        public string UserName { get; set; }
        [Key(9)]
        public bool IsAuthenticated { get; set; }

        [Key(10)]
        public Type RequestType { get; set; }
        [Key(11)]
        public Type ResponseType { get; set; }

        [Key(12)]
        public Type CommandType { get; set; }

        /// <summary>
        /// 克隆当前ExecutionContext实例
        /// </summary>
        /// <returns>新的ExecutionContext副本</returns>
        public CommandExecutionContext Clone()
        {
            return new CommandExecutionContext
            {
                SessionId = this.SessionId,
                ClientId = this.ClientId,
                RequestId = this.RequestId,
                ClientIp = this.ClientIp,
                Token = this.Token,
                ReceivedTime = this.ReceivedTime,
                Extensions = new Dictionary<string, object>(this.Extensions),
                UserId = this.UserId,
                UserName = this.UserName,
                IsAuthenticated = this.IsAuthenticated,
                RequestType = this.RequestType,
                ResponseType = this.ResponseType,
                CommandType=this.CommandType,
            };
        }



    }
}
