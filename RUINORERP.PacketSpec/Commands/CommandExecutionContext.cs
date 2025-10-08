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
        [Key(16)]
        public string SessionId { get; set; }
        [Key(17)]
        public string ClientId { get; set; }
        [Key(18)]
        public string RequestId { get; set; }
        [Key(19)]
        public string ClientIp { get; set; }
        [Key(20)]
        public TokenInfo Token { get; set; }
        [Key(21)]
        public DateTime ReceivedTime { get; set; }



        [MessagePack.IgnoreMember]
        public IDictionary<string, object> Extensions { get; set; } = new Dictionary<string, object>();

        // 添加用户认证信息
        [Key(23)]
        public string UserId { get; set; }
        [Key(24)]
        public string UserName { get; set; }
        [Key(25)]
        public bool IsAuthenticated { get; set; }

        [Key(26)]
        public string RequestTypeName { get; set; }
        [Key(27)]
        public string ResponseTypeName { get; set; }

        [Key(28)]
        public string CommandTypeName { get; set; }

        // 类型属性的访问器
        [IgnoreMember]
        public Type RequestType 
        { 
            get => !string.IsNullOrEmpty(RequestTypeName) ? Type.GetType(RequestTypeName) : null;
            set => RequestTypeName = value?.FullName;
        }
        
        [IgnoreMember]
        public Type ResponseType 
        { 
            get => !string.IsNullOrEmpty(ResponseTypeName) ? Type.GetType(ResponseTypeName) : null;
            set => ResponseTypeName = value?.FullName;
        }
        
        [IgnoreMember]
        public Type CommandType 
        { 
            get => !string.IsNullOrEmpty(CommandTypeName) ? Type.GetType(CommandTypeName) : null;
            set => CommandTypeName = value?.FullName;
        }

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
                RequestTypeName = this.RequestTypeName,
                ResponseTypeName = this.ResponseTypeName,
                CommandTypeName = this.CommandTypeName,
            };
        }



    }
}
