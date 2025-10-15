﻿﻿﻿using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;
using MessagePack;
using RUINORERP.Model.CommonModel;

namespace RUINORERP.PacketSpec.Commands.System
{
    /// <summary>
    /// 心跳命令
    /// 用于客户端向服务器发送心跳包，维持连接活跃状态
    /// </summary>
    [PacketCommandAttribute("Heartbeat", CommandCategory.System, Description = "心跳命令")]
    [MessagePackObject(AllowPrivate = true)]
    public class HeartbeatCommand :  BaseCommand<HeartbeatRequest, HeartbeatResponse>
    {
        /// <summary>
        /// 心跳请求数据
        /// </summary>
        [Key(1000)]
        public HeartbeatRequest HeartbeatRequest
        {
            get => Request;
            set => Request = value;
        }

        /// <summary>
        /// 心跳响应数据
        /// </summary>
        [Key(1001)]
        public HeartbeatResponse HeartbeatResponse
        {
            get => Response;
            set => Response = value;
        }
 



        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="sessionToken">会话令牌</param>
        /// <param name="userId">用户ID</param>
        public HeartbeatCommand(string clientId)
        {
            Request = new HeartbeatRequest
            {
                ClientId = clientId,
              
        };

            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = SystemCommands.Heartbeat;
        }

        // 客户端信息属性，由客户端设置
        [Key(1002)]
        public string ClientVersion { get; set; }
    
        [Key(1005)]
        public string ClientStatus { get; set; }
        [Key(1006)]
        public int ProcessUptime { get; set; }

        // 网络延迟信息，由客户端设置
        [Key(1007)]
        public int NetworkLatency { get; set; }

        // 客户端资源使用情况，由客户端设置
        [Key(1008)]
        public ClientResourceUsage ResourceUsage { get; set; }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的心跳数据</returns>
        [Obsolete("用新的数据容器，不再需要重写GetSerializableData")]
#pragma warning disable CS0809 // 过时成员重写未过时成员
        public override object GetSerializableData()
#pragma warning restore CS0809 // 过时成员重写未过时成员
        {
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            try
            {
                // 确保ResourceUsage不为空
                var resourceUsage = this.ResourceUsage ?? new ClientResourceUsage();

                // 设置ProcessUptime到ResourceUsage中
                if (this.ProcessUptime > 0)
                {
                    resourceUsage.ProcessUptime = this.ProcessUptime;
                }

                // 使用Request中的值
                var request = this.Request ?? new HeartbeatRequest();
                return new HeartbeatRequest
                {
                    SessionToken = request.SessionToken,
                    UserId = request.UserId,
                    ClientTime = DateTime.Now,
                    NetworkLatency = this.NetworkLatency,
                    ClientVersion = this.ClientVersion,
                    ResourceUsage = resourceUsage,
                    ClientStatus = this.ClientStatus
                };
            }
            catch (Exception ex)
            {
                // 发生异常时返回基本数据
                return new HeartbeatResponse
                {
                    Timestamp = DateTime.Now,
                    ErrorMessage = "Failed to collect full heartbeat data"
                };
            }
#pragma warning restore CS0168 // 声明了变量，但从未使用过
        }

 
      
    }
}
