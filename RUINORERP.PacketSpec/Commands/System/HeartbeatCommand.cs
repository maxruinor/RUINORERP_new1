using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Commands.System
{
    /// <summary>
    /// 心跳命令
    /// 用于客户端向服务器发送心跳包，维持连接活跃状态
    /// </summary>
    [PacketCommandAttribute("Heartbeat", CommandCategory.System, Description = "心跳命令")]
    public class HeartbeatCommand : RUINORERP.PacketSpec.Commands.BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => SystemCommands.Heartbeat;

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 会话令牌
        /// </summary>
        public string SessionToken { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HeartbeatCommand()
        {
            Priority = CommandPriority.Normal;
            TimeoutMs = 10000; // 心跳命令超时时间10秒
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        public HeartbeatCommand(string clientId)
        {
            ClientId = clientId;
            Priority = CommandPriority.Normal;
            TimeoutMs = 10000; // 心跳命令超时时间10秒
            Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <param name="sessionToken">会话令牌</param>
        /// <param name="userId">用户ID</param>
        public HeartbeatCommand(string clientId, string sessionToken, long userId)
        {
            ClientId = clientId;
            SessionToken = sessionToken;
            UserId = userId;
            Priority = CommandPriority.Normal;
            TimeoutMs = 10000; // 心跳命令超时时间10秒
            Timestamp = DateTime.UtcNow;
        }

        // 客户端信息属性，由客户端设置
        public string ClientVersion { get; set; }
        public string ClientIp { get; set; }
        public string ClientStatus { get; set; }
        public int ProcessUptime { get; set; }

        // 网络延迟信息，由客户端设置
        public int NetworkLatency { get; set; }

        // 客户端资源使用情况，由客户端设置
        public ClientResourceUsage ResourceUsage { get; set; }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的心跳数据</returns>
        public override object GetSerializableData()
        {
            try
            {
                // 确保ResourceUsage不为空
                var resourceUsage = this.ResourceUsage ?? new ClientResourceUsage();
                
                // 设置ProcessUptime到ResourceUsage中
                if (this.ProcessUptime > 0)
                {
                    resourceUsage.ProcessUptime = this.ProcessUptime;
                }
                
                return new HeartbeatRequest
                {
                    ClientId = this.ClientId,
                    SessionToken = this.SessionToken,
                    UserId = this.UserId,
                    Timestamp = this.Timestamp,
                    ClientTime = DateTime.UtcNow,
                    NetworkLatency = this.NetworkLatency,
                    ClientVersion = this.ClientVersion,
                    ClientIp = this.ClientIp,
                    ResourceUsage = resourceUsage,
                    ClientStatus = this.ClientStatus
                };
            }
            catch (Exception ex)
            {
                // 发生异常时返回基本数据
                return new
                {
                    ClientId = this.ClientId,
                    Timestamp = this.Timestamp,
                    ErrorMessage = "Failed to collect full heartbeat data"
                };
            }
        }

        /// <summary>
        /// 命令执行的具体逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        protected override async Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // 验证客户端ID
                if (string.IsNullOrEmpty(ClientId))
                {
                    return ConvertToApiResponse(ResponseBase.CreateError("客户端ID不能为空", 400)
                        .WithMetadata("ErrorCode", "EMPTY_CLIENT_ID"));
                }

                // 构建心跳数据
                var heartbeatData = GetSerializableData();

                // 返回成功结果，实际的网络请求由通信服务处理
                var result = new ResponseBase
                {
                    IsSuccess = true,
                    Message = "心跳命令构建成功",
                    Code = 200,
                    Timestamp = DateTime.UtcNow
                };
                result.WithMetadata("Data", heartbeatData);
                return result;
            }
            catch (Exception ex)
            {
                return ConvertToApiResponse(ResponseBase.CreateError($"心跳命令执行异常: {ex.Message}", 500)
                    .WithMetadata("ErrorCode", "HEARTBEAT_EXCEPTION")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace));
            }
        }

        /// <summary>
        /// 验证命令数据
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            // 调用基类验证
            var baseResult = base.Validate();
            if (!baseResult.IsValid)
            {
                return baseResult;
            }

            // 验证客户端ID
            if (string.IsNullOrEmpty(ClientId))
            {
                return CommandValidationResult.Failure("客户端ID不能为空");
            }

            // 验证会话令牌（如果设置了）
            if (UserId > 0 && string.IsNullOrEmpty(SessionToken))
            {
                return CommandValidationResult.Failure("已登录用户必须提供会话令牌");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 将ResponseBase转换为ApiResponse
        /// </summary>
        /// <param name="baseResponse">基础响应对象</param>
        /// <returns>ApiResponse对象</returns>
        private ResponseBase ConvertToApiResponse(ResponseBase baseResponse)
        {
            var response = new ResponseBase
            {
                IsSuccess = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                Code = baseResponse.Code,
                Timestamp = baseResponse.Timestamp,
                RequestId = baseResponse.RequestId,
                Metadata = baseResponse.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                ExecutionTimeMs = baseResponse.ExecutionTimeMs
            };
            return response;
        }
    }
}